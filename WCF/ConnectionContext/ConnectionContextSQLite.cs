#define _SQLITE_

#if _SQLITE_

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Web;

namespace ConnectionContext
{
	/// <summary>
	/// The Sql Helper.
	/// </summary>
	public class ConnectionContextSQLite : IDisposable
	{
		#region Constants and Fields

		public UserAccounts.FCUser user = null;

		/// <summary>
		/// The connection.
		/// </summary>
		private SQLiteConnection _connection = null;
		public SQLiteConnection connection
		{
			get { return _connection; }
		}

		/// <summary>
		/// The context data.
		/// </summary>
		public object contextData;

		/// <summary>
		/// The transaction.
		/// </summary>
		public SQLiteTransaction transaction;

		/// <summary>
		/// sql storage connection string
		/// </summary>
		//private static string connectionString = null;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SeamContext"/> class. 
		/// Default constructor
		/// </summary>
		
		public ConnectionContextSQLite()
		{
			if (HttpContext.Current != null && HttpContext.Current.Session != null)
				user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
			
			//string connectionString = ConfigurationManager.ConnectionStrings["SQLiteTest"].ConnectionString;
			if (_connection == null)
			{
				string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\";
				string db3file = path + "qlc.db3";
				string connectionString = string.Format("Data Source={0};Version=3;", db3file);

				_connection = new SQLiteConnection(connectionString);
				_connection.Open();
			}

			this.transaction = _connection.BeginTransaction();
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
			
			if (_connection != null)
			{
				_connection.Close();
				_connection.Dispose();
				_connection = null;
			}
			
			this.contextData = null;
		}

		#endregion

		#endregion
	}
}

#endif // _SQLITE_