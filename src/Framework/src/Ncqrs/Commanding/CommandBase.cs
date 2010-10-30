﻿using System;
using System.Diagnostics.Contracts;
using Ncqrs.Commanding.CommandExecution.Mapping.Attributes;

namespace Ncqrs.Commanding
{
    /// <summary>
    /// The base of a command message. A command should contain all the
    /// information and intend that is needed to execute an corresponding
    /// action.
    /// </summary>
    [Serializable]
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Gets the unique identifier for this command.
        /// </summary>
        [ExcludeInMapping]
        public Guid CommandIdentifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// This initializes the <see cref="CommandIdentifier"/> with the given
        /// id from <paramref name="commandIdentifier"/>.
        /// </summary>
        /// <param name="commandIdentifier">The command identifier.</param>
        protected CommandBase(Guid commandIdentifier)
        {
            CommandIdentifier = commandIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// This initializes the <see cref="CommandIdentifier"/> with the result
        /// of the <see cref="IUniqueIdentifierGenerator">generator</see> set in 
        /// the <see cref="NcqrsEnvironment"/>.
        /// </summary>
        /// <remarks>
        /// This uses the <see cref="NcqrsEnvironment.Get{IUniqueIdentifierGenerator}"/> to get
        /// the generator to use to generate the command identifier.
        /// </remarks>
        protected CommandBase() : this(NcqrsEnvironment.Get<IUniqueIdentifierGenerator>())
        {
            // Nothing.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class. This will set
        /// the <see cref="CommandIdentifier"/> to a identifier value generated by the 
        /// <paramref name="idGenerator"/>.
        /// </summary>
        /// <param name="idGenerator">The id generator. This cannot be <c>null</c>.</param>
        protected CommandBase(IUniqueIdentifierGenerator idGenerator)
        {
            Contract.Requires<ArgumentNullException>(idGenerator != null);

            CommandIdentifier = idGenerator.GenerateNewId();
        }
    }
}