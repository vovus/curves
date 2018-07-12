using System;
using System.Collections.Generic;

using ConnectionContext;

namespace UserAccounts
{
    public class Repository
    {
		//
		// User Accounts
		//
		public static bool sIsLogedIn = false;

		public static FCUser CreateUser(string n, string p)
		{
			FCUser newUser = new FCUser { name = n, pass = p, status = FCUser.eStatus.eDisabled, g = Guid.NewGuid() };

			// creates or confirms user
#if _LINQXML_
			DataHelperLinqXml.AddEntryPointHistory(epl);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				if (!DataHelperSQLite.CreateUser(ctx, newUser))
					return null;

				return newUser;
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				return DataHelper.CreateUser(ctx, user);
			}
#endif
		}

		public static bool ConfirmUser(string code)
		{
			// creates or confirms user
#if _LINQXML_
			DataHelperLinqXml.AddEntryPointHistory(epl);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.ConfirmUser(ctx, code);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				return DataHelper.ConfirmUser(ctx, user);
			}
#endif
		}

		public static UserAccounts.FCUser LoginUser(string name, string pass)
		{
#if _LINQXML_
			DataHelperLinqXml.AddEntryPointHistory(epl);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.LoginUser(ctx, name, pass);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				return DataHelper.CheckSignInUser(ctx, user);
			}
#endif
		}
    }
}
