// Copyright (c) 2006, Microsoft Corporation
//
//  Author: Alazel Acheson

namespace Ncqrs.Eventing.Storage.SQL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    // Allows almost-automated re-use of connections across multiple call levels
    //  while still controlling connection lifetimes.  Multiple connections are supported within a single scope.
    // To use:
    //  Create a new connection scope object in a using statement at the level within which you 
    //      want to scope connections.
    //  Use Current.AddConnection() and Current.GetConnection() to store/retrieve specific connections based on your
    //      own keys.
    //  Simpler alternative: Use Current.GetOpenConnection(factory, connection string) where you need to use the connection
    //
    // Example of simple case:
    //  void TopLevel() {
    //      using (DbConnectionScope scope = new DbConnectionScope()) {
    //          // Code that eventually calls LowerLevel a couple of times.
    //          // The first time LowerLevel is called, it will allocate and open the connection
    //          // Subsequent calls will use the already-opened connection, INCLUDING running in the same 
    //          //   System.Transactions transaction without using DTC (assuming only one connection string)!
    //      }
    //  }
    //
    //  void LowerLevel() {
    //      string connectionString = <...get connection string from config or somewhere...>;
    //      SqlCommand cmd = new SqlCommand("Some TSQL code");
    //      cmd.Connection = (SqlConnection) DbConnectionScope.Current.GetOpenConnection(SqlClientFactory.Instance, connectionString);
    //      ... finish setting up command and execute it
    //  }

    /// <summary>
    /// Class to assist in managing connection lifetimes inside scopes on a particular thread.
    /// </summary>
    sealed public class DbConnectionScope : IDisposable
    {
        #region class fields
        [ThreadStatic()]
        private static DbConnectionScope __currentScope = null;      // Scope that is currently active on this thread
        private static Object __nullKey = new Object();   // used to allow null as a key
        #endregion

        #region instance fields
        private DbConnectionScope _priorScope;    // previous scope in stack of scopes on this thread
        private Dictionary<object, DbConnection> _connections;   // set of connections contained by this scope.
        #endregion

        #region public class methods and properties
        /// <summary>
        /// Obtain the currently active connection scope
        /// </summary>
        public static DbConnectionScope Current
        {
            get
            {
                return __currentScope;
            }
        }
        #endregion

        #region public instance methods and properties
        /// <summary>
        /// Constructor
        /// </summary>
        public DbConnectionScope()
        {
            // Devnote:  Order of initial assignment is important in cases of failure!
            //  _priorScope first makes sure we know who we need to restore
            //  _connections second, to make sure we no-op dispose until we're as close to
            //      correct setup as possible
            //  __currentScope last, to make sure the thread static only holds validly set up objects
            _priorScope = __currentScope;
            _connections = new Dictionary<object, DbConnection>();
            __currentScope = this;
        }

        /// <summary>
        /// Convenience constructor to add an initial connection
        /// </summary>
        /// <param name="key">Key to associate with connection</param>
        /// <param name="connection">Connection to add</param>
        public DbConnectionScope(object key, DbConnection connection)
            : this()
        {
            AddConnection(key, connection);
        }

        /// <summary>
        /// Add a connection and associate it with the given key
        /// </summary>
        /// <param name="key">Key to associate with the connection</param>
        /// <param name="connection">Connection to add</param>
        public void AddConnection(object key, DbConnection connection)
        {
            CheckDisposed();
            if (null == key)
            {
                key = __nullKey;
            }
            _connections[key] = connection;
        }

        /// <summary>
        /// Check to see if there is a connection associated with this key
        /// </summary>
        /// <param name="key">Key to use for lookup</param>
        /// <returns>true if there is a connection, false otherwise</returns>
        public bool ContainsKey(object key)
        {
            CheckDisposed();
            return _connections.ContainsKey(key);
        }

        /// <summary>
        /// Shut down this instance.  Disposes all connections it holds and restores the prior scope.
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                // Firstly, remove ourselves from the stack (but, only if we are the one on the stack)
                //  Note: Thread-local _currentScope, and requirement that scopes not be disposed on other threads
                //      means we can get away with not locking.
                if (__currentScope == this)
                {
                    // In case the user called dispose out of order, skip up the chain until we find
                    //  an undisposed scope.
                    DbConnectionScope prior = _priorScope;
                    while (null != prior && prior.IsDisposed)
                    {
                        prior = prior._priorScope;
                    }
                    __currentScope = prior;
                }

                // secondly, make sure our internal state is set to "Disposed"
                IDictionary<object, DbConnection> connections = _connections;
                _connections = null;

                // Lastly, clean up the connections we own
                foreach (DbConnection connection in connections.Values)
                {
                    connection.Dispose();
                }
            }
        }

        /// <summary>
        /// Get the connection associated with this key. Throws if there is no entry for the key.
        /// </summary>
        /// <param name="key">Key to use for lookup</param>
        /// <returns>Associated connection</returns>
        public DbConnection GetConnection(object key)
        {
            CheckDisposed();

            // allow null-ref as key
            if (null == key)
            {
                key = __nullKey;
            }

            return _connections[key];
        }

        /// <summary>
        /// This method gets the connection using the connection string as a key.  If no connection is
        /// associated with the string, the connection factory is used to create the connection.
        /// Finally, if the resulting connection is in the closed state, it is opened.
        /// </summary>
        /// <param name="factory">Factory to use to create connection if it is not already present</param>
        /// <param name="connectionString">Connection string to use</param>
        /// <returns>Connection in open state</returns>
        public DbConnection GetOpenConnection(DbProviderFactory factory, string connectionString)
        {
            CheckDisposed();
            object key;

            // allow null-ref as key
            if (null == connectionString)
            {
                key = __nullKey;
            }
            else
            {
                key = connectionString;
            }

            // go get the connection
            DbConnection result;
            if (!_connections.TryGetValue(key, out result))
            {
                // didn't find it, so create it.
                result = factory.CreateConnection();
                result.ConnectionString = connectionString;
                _connections[key] = result;
            }

            // however we got it, open it if it's closed.
            //  note: don't open unless state is unambiguous that it's ok to open
            if (ConnectionState.Closed == result.State)
            {
                result.Open();
            }

            return result;
        }
        #endregion

        #region private methods and properties
        /// <summary>
        /// Was this instance previously disposed?
        /// </summary>
        private bool IsDisposed
        {
            get
            {
                return null == _connections;
            }
        }

        /// <summary>
        /// Handle calling API function after instance has been disposed
        /// </summary>
        private void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("DbConnectionScope");
            }
        }

        #endregion
    }
}
