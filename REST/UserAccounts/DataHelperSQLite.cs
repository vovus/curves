#define _SQLITE_

#if _SQLITE_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Data.SQLite;

using ConnectionContext;

namespace UserAccounts
{
	/// <summary>
	/// The Data Helper.
	/// </summary>
	internal class DataHelperSQLite
	{
		//
		// User Accounts
		//

		internal static bool CreateUser(ConnectionContextSQLite ctx, UserAccounts.FCUser user)
		{
			//
			// Check if already exists with such name
			//
			var command = SqlHelperSQLite.CheckUser(ctx, user);
			string name = string.Empty;

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "" : reader.GetString(reader.GetOrdinal("Name"));
					if (string.IsNullOrEmpty(name))
						return false;
				}
			}

			// such user already exists
			if (!string.IsNullOrEmpty(name))
				return false;

			//
			// creates non-confirmed user
			//
			command = SqlHelperSQLite.CreateUser(ctx, ref user);
			try
			{
				command.ExecuteNonQuery();
				command.CommandText = "Commit";
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Utilities.Logger.WriteError(String.Format("CreateUser() failed - {0}", ex.Message));
				return false;
			}
			return true;
		}

		internal static bool ConfirmUser(ConnectionContextSQLite ctx, string userGuid)
		{
			// confirms user
			var command = SqlHelperSQLite.ConfirmUser(ctx, userGuid);
			try
			{
				command.ExecuteNonQuery();
				command.CommandText = "Commit";
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}

		internal static UserAccounts.FCUser LoginUser(ConnectionContextSQLite ctx, string name, string pass)
		{
			var command = SqlHelperSQLite.LoginUser(ctx, name, pass);
			List<UserAccounts.FCUser> users = new List<UserAccounts.FCUser>();

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var s = reader.IsDBNull(reader.GetOrdinal("Status")) ? 0 : reader.GetInt32(reader.GetOrdinal("Status"));
					if (s <= 0) 
						s = 0;

					var g = reader.IsDBNull(reader.GetOrdinal("Guid")) ? "" : reader.GetString(reader.GetOrdinal("Guid"));

					var tmp = new UserAccounts.FCUser 
					{
						id = reader.IsDBNull(reader.GetOrdinal("Status")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
						name = reader.IsDBNull(reader.GetOrdinal("Name")) ? "" : reader.GetString(reader.GetOrdinal("Name")),
						pass = reader.IsDBNull(reader.GetOrdinal("Pass")) ? "" : reader.GetString(reader.GetOrdinal("Pass")),
						g = string.IsNullOrEmpty(g) ? Guid.Empty : new Guid(g),
						status = (UserAccounts.FCUser.eStatus)s
					};

					users.Add(tmp);
				}
			}
			
			if (users.Count == 1 
				&& users[0].g != Guid.Empty
				//&& users[0].status != FCUser.eStatus.eDisabled
				)
				return users[0];

			return null;
		}
	}
}

#endif // _SQLITE_