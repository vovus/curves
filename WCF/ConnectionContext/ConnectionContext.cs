//#define _MYSQL_

#if _MYSQL_

using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ConnectionContext
{
    /// <summary>
    /// The Sql Helper.
    /// </summary>
    public class ConnectionContextMySQL : IDisposable
    {
        #region Constants and Fields

        /// <summary>
        /// The connection.
        /// </summary>
        public MySqlConnection connection;

        /// <summary>
        /// The context data.
        /// </summary>
        public object contextData;

        /// <summary>
        /// The transaction.
        /// </summary>
        public MySqlTransaction transaction;
        
        /// <summary>
        /// sql storage connection string
        /// </summary>
        private static string connectionString = null;

        #endregion

        #region Constructors and Destructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SeamContext"/> class. 
        /// Default constructor
        /// </summary>
		public ConnectionContextMySQL()
        {
			string connectionString = ConfigurationManager.ConnectionStrings["MySqlTest"].ConnectionString;
            this.connection = new MySqlConnection(connectionString);
            this.connection.Open();
            this.transaction = this.connection.BeginTransaction();
            this.contextData = null;
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// IDisposable method implementation
        /// </summary>
        public void Dispose()
        {
            if (this.transaction != null)
            {
                this.transaction.Dispose();
                this.transaction = null;
            }

            if (this.connection != null)
            {
                this.connection.Dispose();
                this.connection = null;
            }

            this.contextData = null;
        }

        #endregion

        #endregion
    }
}

#endif // _MYSQL_