#define _SQLITE_

#if _SQLITE_

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using ConnectionContext;

namespace UserAccounts
{
	internal class SqlHelperSQLite
	{
		//
		// UserAccounts
		//

		internal static SQLiteCommand CheckUser(ConnectionContextSQLite ctx, UserAccounts.FCUser user)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);

			SQLiteParameter p = new SQLiteParameter("@name", DbType.String);
			p.Value = user.name;
			command.Parameters.Add(p);

			// check if exists, check guid, set Enabled if ok
			var sb = new StringBuilder(
			@"SELECT Name
				FROM Users
				WHERE Name = @name "
			);
		
			command.CommandText = sb.ToString();
			return command;
		}

		internal static SQLiteCommand CreateUser(ConnectionContextSQLite ctx, ref UserAccounts.FCUser user)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);
			
			SQLiteParameter p = new SQLiteParameter("@name", DbType.String);
			p.Value = user.name;
			command.Parameters.Add(p);

			p = new SQLiteParameter("@pass", DbType.String);
			p.Value = user.pass;
			command.Parameters.Add(p);

			p = new SQLiteParameter("@guid", DbType.String);
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

		internal static SQLiteCommand ConfirmUser(ConnectionContextSQLite ctx, string userGuid)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);

			SQLiteParameter p = new SQLiteParameter("@guid", DbType.String);
			//p.Value = userGuid.ToString("B");
			command.Parameters.Add(p);

			// check if exists, check guid, set Enabled if ok
			var sb = new StringBuilder(
			@"UPDATE Users SET Status = 1
				WHERE 
				Guid = @guid"
			);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static SQLiteCommand LoginUser(ConnectionContextSQLite ctx, string name, string pass)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);

			SQLiteParameter p = new SQLiteParameter("@name", DbType.String);
			p.Value = name;
			command.Parameters.Add(p);

			p = new SQLiteParameter("@pass", DbType.String);
			p.Value = pass;
			command.Parameters.Add(p);

			// check if exists, check guid, set Enabled if ok
			var sb = new StringBuilder(
			@"SELECT 
				Id, Name, Pass, Guid, Status
				FROM Users
				WHERE Name = @name AND Pass = @pass"
			);
			/*
			if (user.g != Guid.Empty)
			{
				sb.Append(" AND Guid = @g");
				p = new SQLiteParameter("@g", DbType.String);
				p.Value = user.g.ToString("B");
				command.Parameters.Add(p);
			}
			*/
			command.CommandText = sb.ToString();
			return command;
		}
	}
}

#endif // _SQLITE_