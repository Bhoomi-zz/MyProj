﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Ncqrs.Eventing.ServiceModel.Bus;
using Ncqrs.Eventing.Sourcing.Snapshotting;
using Ncqrs.Eventing.Storage;

namespace Ncqrs.Domain.Storage
{
    public class DomainRepository : IDomainRepository
    {
        private const int SnapshotIntervalInEvents = 15;

        private readonly IEventBus _eventBus;
        private readonly IEventStore _store;
        private readonly ISnapshotStore _snapshotStore;
        private readonly IAggregateRootCreationStrategy _aggregateRootCreator = new SimpleAggregateRootCreationStrategy();

        public DomainRepository(IEventStore store, IEventBus eventBus, ISnapshotStore snapshotStore = null, IAggregateRootCreationStrategy aggregateRootCreationStrategy = null)
        {
            Contract.Requires<ArgumentNullException>(store != null);
            Contract.Requires<ArgumentNullException>(eventBus != null);

            _store = store;
            _eventBus = eventBus;
            _snapshotStore = snapshotStore;
            _aggregateRootCreator = aggregateRootCreationStrategy ?? new SimpleAggregateRootCreationStrategy();
        }

        private bool ShouldCreateSnapshot(AggregateRoot aggregateRoot)
        {
            return (_snapshotStore != null)&&(aggregateRoot.Version % SnapshotIntervalInEvents) == 0;
        }

        /// <summary>
        /// Gets aggregate root by eventSourceId.
        /// </summary>
        /// <typeparam name="T">The type of the aggregate root.</typeparam>
        /// <param name="eventSourceId">The eventSourceId of the aggregate root.</param>
        /// <exception cref="AggregateRootNotFoundException">Occurs when the aggregate root with the 
        /// specified event source id could not be found.</exception>
        /// <returns>
        /// A new instance of the aggregate root that contains the latest known state.
        /// </returns>
        public T GetById<T>(Guid eventSourceId) where T : AggregateRoot
        {
            return (T)GetById(typeof(T), eventSourceId);
        }

        /// <summary>
        /// Gets aggregate root by <see cref="AggregateRoot.EventSourcId">event source id</see>.
        /// </summary>
        /// <param name="aggregateRootType">Type of the aggregate root.</param>
        /// <param name="eventSourceId">The eventSourceId of the aggregate root.</param>
        /// <exception cref="AggregateRootNotFoundException">Occurs when the aggregate root with the 
        /// specified event source id could not be found.</exception>
        /// <returns>
        /// A new instance of the aggregate root that contains the latest known state.
        /// </returns>
        public AggregateRoot GetById(Type aggregateRootType, Guid eventSourceId)
        {
            AggregateRoot aggregate = null;

            if(_snapshotStore != null)
            {
                var snapshot = _snapshotStore.GetSnapshot(eventSourceId);

                if (snapshot != null)
                {
                    aggregate = GetByIdFromSnapshot(aggregateRootType, snapshot);
                }
            }

            if(aggregate == null)
            {
                aggregate = GetByIdFromScratch(aggregateRootType, eventSourceId);
            }

            if(aggregate == null)
            {
                var msg = string.Format("Aggregate root with EventSourceId {0} does not exist.",
                                        eventSourceId);

                throw new AggregateRootNotFoundException(msg);
            }

            return aggregate;
        }

        protected AggregateRoot GetByIdFromSnapshot(Type aggregateRootType, ISnapshot snapshot)
        {
            AggregateRoot aggregateRoot = null;

            if(AggregateRootSupportsSnapshot(aggregateRootType, snapshot))
            {
                aggregateRoot = CreateEmptyAggRoot(aggregateRootType);
                var memType = GetSnapshotInterfaceType(aggregateRootType);
                var restoreMethod = memType.GetMethod("RestoreFromSnapshot");

                restoreMethod.Invoke(aggregateRoot, new object[] { snapshot });

                var events = _store.GetAllEventsSinceVersion(aggregateRoot.EventSourceId, snapshot.EventSourceVersion);
                aggregateRoot.InitializeFromHistory(events);
            }
            else
            {
                aggregateRoot = GetByIdFromScratch(aggregateRootType, snapshot.EventSourceId);
            }

            return aggregateRoot;
        }

        protected AggregateRoot GetByIdFromScratch(Type aggregateRootType, Guid id)
        {
            AggregateRoot aggregateRoot = null;

            var events = _store.GetAllEvents(id);

            if (events.Count() > 0)
            {
                aggregateRoot = CreateEmptyAggRoot(aggregateRootType);
                aggregateRoot.InitializeFromHistory(events);
            }

            return aggregateRoot;
        }

        protected AggregateRoot CreateEmptyAggRoot(Type aggregateRootType)
        {
            return _aggregateRootCreator.CreateAggregateRoot(aggregateRootType);
        }

        private bool AggregateRootSupportsSnapshot(Type aggType, ISnapshot snapshot)
        {
            var memType = GetSnapshotInterfaceType(aggType);
            return memType == typeof(ISnapshotable<>).MakeGenericType(memType);
        }

        public void Save(AggregateRoot aggregateRoot)
        {
            var events = aggregateRoot.GetUncommittedEvents();

            _store.Save(aggregateRoot);

            _eventBus.Publish(events);

            // Accept the changes.
            aggregateRoot.AcceptChanges();

            // TODO: Snapshot should not effect saving.
            if (ShouldCreateSnapshot(aggregateRoot))
            {
                var snapshot = GetSnapshot(aggregateRoot);

                if (snapshot != null) _snapshotStore.SaveShapshot(snapshot);
            }
        }

        private ISnapshot GetSnapshot(AggregateRoot aggregateRoot)
        {
            var memType = GetSnapshotInterfaceType(aggregateRoot.GetType());

            if (memType != null)
            {
                var createMethod = memType.GetMethod("CreateSnapshot");

                return (ISnapshot)createMethod.Invoke(aggregateRoot, new object[0]);
            }
            else
            {
                return null;
            }
        }

        private Type GetSnapshotInterfaceType(Type aggType)
        {
            // Query all ISnapshotable interfaces. We only allow only
            // one ISnapshotable interface per aggregate root type.
            var snapshotables = from i in aggType.GetInterfaces()
                                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISnapshotable<>)
                                select i;

            // Aggregate does not implement any ISnapshotable interface.
            if (snapshotables.Count() == 0)
            {
                return null;
            }
            // Aggregate does implement multiple ISnapshotable interfaces.
            if (snapshotables.Count() > 1)
            {
                return null;
            }

            return snapshotables.Single();
        }
    }
}
