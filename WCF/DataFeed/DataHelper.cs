//#define _MYSQL_

#if _MYSQL_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using MySql.Data.MySqlClient;

using DataLayer;
using ConnectionContext;

namespace DataFeed
{
    /// <summary>
    /// The Data Helper.
    /// </summary>
    internal class DataHelper
    {
        //
		// DataFeed
		//
		internal static List<EntryPoint> GetAllEntryPoints(ConnectionContextMySQL ctx)
		{
			List<EntryPoint> res = new List<EntryPoint>();

			var command = SqlHelper.GetAllEntryPoints(ctx);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var ep = new EntryPoint
					{
						Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? -1 : reader.GetInt32(reader.GetOrdinal("Id")),
						Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? "" : reader.GetString(reader.GetOrdinal("Type")),
						YieldCurveId = reader.IsDBNull(reader.GetOrdinal("YieldCurveId")) ? -1 : reader.GetInt32(reader.GetOrdinal("YieldCurveId")),
						Length = reader.IsDBNull(reader.GetOrdinal("Length")) ? -1 : reader.GetInt32(reader.GetOrdinal("Length")),
						TimeUnit = reader.IsDBNull(reader.GetOrdinal("TimeUnit")) ? "" : reader.GetString(reader.GetOrdinal("TimeUnit")),
						ValidDateBegin = reader.GetDateTime(reader.GetOrdinal("DateStart")),
						ValidDateEnd = reader.GetDateTime(reader.GetOrdinal("DateFinish")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? -1 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? "" : reader.GetString(reader.GetOrdinal("DataReference"))
						//UserCreated = reader.IsDBNull(reader.GetOrdinal("UserCreated")) ? 0 : reader.GetString(reader.GetOrdinal("UserCreated")),
						//IpCreated = reader.IsDBNull(reader.GetOrdinal("IpCreated")) ? 0 : reader.GetString(reader.GetOrdinal("IpCreated"))
					};

					ep.Instrument = null;
					long rateId = reader.IsDBNull(reader.GetOrdinal("RateId")) ? -1 : reader.GetInt32(reader.GetOrdinal("RateId"));
					if (rateId != -1)
						ep.Instrument = (ep.Type == "bond"
									? (Instrument)DataLayer.DataHelper.GetBond(reader.GetInt32(reader.GetOrdinal("RateId")))
									: (Instrument)DataLayer.DataHelper.GetRate(reader.GetInt32(reader.GetOrdinal("RateId")))
									);

					res.Add(ep);
				}
			}
			return res;
		}

		internal static void AddEntryPoint(ConnectionContextMySQL ctx, EntryPoint ep)
		{
			var command = SqlHelper.AddEntryPoint(ctx, ep);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{ }
		}

		internal static void UpdateEntryPoint(ConnectionContextMySQL ctx, EntryPoint ep)
		{
			var command = SqlHelper.UpdateEntryPoint(ctx, ep);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{ }
		}

		internal static void GetEntryPointByRateIdType(ConnectionContextMySQL ctx, ref EntryPoint ep)
		{
			List<EntryPoint> res = new List<EntryPoint>();

			var command = SqlHelper.GetEntryPointByRateIdType(ctx, ep);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var tmp = new EntryPoint
					{
						Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? -1 : reader.GetInt32(reader.GetOrdinal("Id")),
						Type = reader.IsDBNull(reader.GetOrdinal("Type")) ? "" : reader.GetString(reader.GetOrdinal("Type")),
						YieldCurveId = reader.IsDBNull(reader.GetOrdinal("YieldCurveId")) ? -1 : reader.GetInt32(reader.GetOrdinal("YieldCurveId")),
						Length = reader.IsDBNull(reader.GetOrdinal("Length")) ? -1 : reader.GetInt32(reader.GetOrdinal("Length")),
						TimeUnit = reader.IsDBNull(reader.GetOrdinal("TimeUnit")) ? "" : reader.GetString(reader.GetOrdinal("TimeUnit")),
						ValidDateBegin = reader.GetDateTime(reader.GetOrdinal("DateStart")),
						ValidDateEnd = reader.GetDateTime(reader.GetOrdinal("DateFinish")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? -1 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? "" : reader.GetString(reader.GetOrdinal("DataReference"))
						//UserCreated = reader.IsDBNull(reader.GetOrdinal("UserCreated")) ? 0 : reader.GetString(reader.GetOrdinal("UserCreated")),
						//IpCreated = reader.IsDBNull(reader.GetOrdinal("IpCreated")) ? 0 : reader.GetString(reader.GetOrdinal("IpCreated"))
					};

					tmp.Instrument = null;
					long rateId = reader.IsDBNull(reader.GetOrdinal("RateId")) ? -1 : reader.GetInt32(reader.GetOrdinal("RateId"));
					if (rateId != -1)
						tmp.Instrument = (tmp.Type == "bond"
									? (Instrument)DataLayer.DataHelper.GetBond(reader.GetInt32(reader.GetOrdinal("RateId")))
									: (Instrument)DataLayer.DataHelper.GetRate(reader.GetInt32(reader.GetOrdinal("RateId")))
									);

					res.Add(tmp);
				}

				ep = res[0];
			}
		}

		internal static void AddEntryPointHistory(ConnectionContextMySQL ctx, List<EntryPoint> epl)
		{
			foreach (var ep in epl)
			{
				var command = SqlHelper.AddEntryPointHistory(ctx, ep);
				try
				{
					command.ExecuteNonQuery();
				}
				catch (Exception ex)
				{ }
			}
		}
    }
}

#endif // _MYSQL_