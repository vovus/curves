//#define _MYSQL_

#if _MYSQL_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using MySql.Data.MySqlClient;

using ConnectionContext;

namespace UserAccounts
{
    /// <summary>
    /// The Data Helper.
    /// </summary>
    internal class DataHelper
    {
		//
		// User Accounts
		//

		internal static bool CreateUser(ConnectionContextMySQL ctx, UserAccounts.FCUser user)
		{
			UserAccounts.FCUser tmp = CheckSignInUser(ctx, user);
			if (tmp != null)
			{
				Utilities.Logger.WriteError(String.Format("CreateUser() failed - user already exists: {0}", user.name));
				return false;
			}
			// creates or confirms user
			var command = SqlHelper.CreateUser(ctx, user);
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

		internal static bool ConfirmUser(ConnectionContextMySQL ctx, UserAccounts.FCUser user)
		{
			UserAccounts.FCUser tmp = CheckSignInUser(ctx, user);
			if (tmp == null
				|| tmp.g == Guid.Empty
				|| tmp.status != UserAccounts.FCUser.eStatus.eDisabled
				)
				return false;

			// creates or confirms user
			var command = SqlHelper.ConfirmUser(ctx, user);
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

		internal static UserAccounts.FCUser CheckSignInUser(ConnectionContextMySQL ctx, UserAccounts.FCUser user)
		{
			var command = SqlHelper.CheckSignInUser(ctx, user);
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
				)
				return users[0];

			return null;
		}
    }
}

#endif // _MYSQL_