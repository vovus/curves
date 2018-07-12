//#define _MYSQL_

#if _MYSQL_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Text;

using ConnectionContext;

namespace UserAccounts
{
    /// <summary>
    /// The Sql Helper.
    /// </summary>
    internal class SqlHelper
    {
		//
		// UserAccounts
		//

		internal static MySqlCommand CreateUser(ConnectionContextMySQL ctx, UserAccounts.FCUser user)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);

			MySqlParameter p = new MySqlParameter("@name", MySqlDbType.String);
			p.Value = user.name;
			command.Parameters.Add(p);

			p = new MySqlParameter("@pass", MySqlDbType.String);
			p.Value = user.pass;
			command.Parameters.Add(p);

			p = new MySqlParameter("@guid", MySqlDbType.String);
			p.Value = user.g.ToString("B");
			command.Parameters.Add(p);

			// if not exists - create as !Enabled
			var sb = new StringBuilder(
			@"INSERT 
				into Users 
				(Name, Pass, Guid, Status, UserCreated, IpCreated)
				values
				(@name, @pass, @guid, 0, 1, '192.168.0.0')"		// 0 == disabled
			);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand ConfirmUser(ConnectionContextMySQL ctx, UserAccounts.FCUser user)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);

			MySqlParameter p = new MySqlParameter("@name", MySqlDbType.String);
			p.Value = user.name;
			command.Parameters.Add(p);

			p = new MySqlParameter("@pass", MySqlDbType.String);
			p.Value = user.pass;
			command.Parameters.Add(p);

			p = new MySqlParameter("@guid", MySqlDbType.String);
			p.Value = user.g.ToString("B");
			command.Parameters.Add(p);

			// check if exists, check guid, set Enabled if ok
			var sb = new StringBuilder(
			@"UPDATE Users SET Status = 1
				WHERE 
				Name = @name AND Pass = @pass AND Guid = @guid"
			);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand CheckSignInUser(ConnectionContextMySQL ctx, UserAccounts.FCUser user)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);

			MySqlParameter p = new MySqlParameter("@name", MySqlDbType.String);
			p.Value = user.name;
			command.Parameters.Add(p);

			p = new MySqlParameter("@pass", MySqlDbType.String);
			p.Value = user.pass;
			command.Parameters.Add(p);

			// check if exists, check guid, set Enabled if ok
			var sb = new StringBuilder(
			@"SELECT 
				Name, Pass, Guid, Status
				FROM Users
				WHERE Name = @name AND Pass = @pass"
			);

			if (user.g != Guid.Empty)
			{
				sb.Append(" AND Guid = @g");
				p = new MySqlParameter("@g", MySqlDbType.String);
				p.Value = user.g.ToString("B");
				command.Parameters.Add(p);
			}

			command.CommandText = sb.ToString();
			return command;
		}
    }
}

#endif // _MYSQL_