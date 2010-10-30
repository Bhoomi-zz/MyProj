using System;
using Ncqrs.Eventing.Storage;

namespace Ncqrs.Eventing.Sourcing
{
    public interface ISourcedEvent : IEvent
    {
        /// <summary>
        /// Gets the id of the event source that caused the event.
        /// </summary>
        /// <value>The id of the event source that caused the event.</value>
        Guid EventSourceId { get; }

        /// <summary>
        /// Gets the event sequence number.
        /// </summary>
        /// <remarks>
        /// An sequence of events always starts with <c>0</c>. So the first event in a sequence has the <see cref="EventSequence"/> value of <c>0</c>.
        /// </remarks>
        /// <value>A number that represents the order of where this events occurred in the sequence.</value>
        long EventSequence { get; }

        void InitializeFrom(StoredEvent stored);
    }
}
