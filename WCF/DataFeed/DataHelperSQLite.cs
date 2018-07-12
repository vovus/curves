#define _SQLITE_

#if _SQLITE_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Data.SQLite;

using DataLayer;
using ConnectionContext;

namespace DataFeed
{
	/// <summary>
	/// The Data Helper.
	/// </summary>
	internal class DataHelperSQLite
	{
		//
		// DateFeed
		//
		internal static List<YieldCurveDefinition> GetYieldCurveDef(ConnectionContextSQLite ctx,
																		long? idYc)
		{
			List<YieldCurveDefinition> ycDesc = new List<YieldCurveDefinition>();

			var command = SqlHelperSQLite.SelectYieldCurveDefCommand(ctx, idYc);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var ycs = new YieldCurveDefinition
					{
						ZcBasisId = reader.GetInt32(reader.GetOrdinal("ZcBasisId")),
						ZcCompounding = reader.GetString(reader.GetOrdinal("ZcCompounding")),
						ZcFrequency = reader.GetString(reader.GetOrdinal("ZcFrequency")),
						FrwCompounding = reader.GetString(reader.GetOrdinal("FrwCompounding")),
						ifZc = reader.GetBoolean(reader.GetOrdinal("ifZC")),

						FrwBasisId = reader.GetInt32(reader.GetOrdinal("FrwBasisId")),
						ZCColor = reader.IsDBNull(reader.GetOrdinal("ZCColor")) ? null : reader.GetString(reader.GetOrdinal("ZCColor")),
						FrwFrequency = reader.GetString(reader.GetOrdinal("FrwFrequency")),
						FrwTermBase = reader.GetString(reader.GetOrdinal("FrwTermBase")),
						FrwTerm = reader.IsDBNull(reader.GetOrdinal("FrwTerm")) ? -1 : reader.GetInt32(reader.GetOrdinal("FrwTerm")),
						FrwColor = reader.IsDBNull(reader.GetOrdinal("FrwColor")) ? null : reader.GetString(reader.GetOrdinal("FrwColor")),
						ifFrw = reader.GetBoolean(reader.GetOrdinal("ifFrw")),

						SpreadType = reader.IsDBNull(reader.GetOrdinal("SpreadType")) ? -1 : reader.GetInt32(reader.GetOrdinal("SpreadType")),
						SpreadSize = reader.IsDBNull(reader.GetOrdinal("SpreadSize")) ? -1 : reader.GetInt32(reader.GetOrdinal("SpreadSize")),
						SpreadFamily = reader.IsDBNull(reader.GetOrdinal("SpreadFamily")) ? -1 : reader.GetInt32(reader.GetOrdinal("SpreadFamily"))
					};

					Calendar cal = new Calendar
					{
						Id = reader.IsDBNull(reader.GetOrdinal("calId")) ? -1 : reader.GetInt32(reader.GetOrdinal("calId")),
						ClassName = reader.IsDBNull(reader.GetOrdinal("calClassName")) ? null : reader.GetString(reader.GetOrdinal("calClassName")),
						MarketName = reader.IsDBNull(reader.GetOrdinal("calMarketName")) ? null : reader.GetString(reader.GetOrdinal("calMarketName"))
					};

					ycs.Calendar = cal;

					ycs.Id = reader.GetInt32(reader.GetOrdinal("Id"));
					ycs.Name = reader.GetString(reader.GetOrdinal("Name"));
					ycs.FamilyId = reader.GetInt32(reader.GetOrdinal("FamilyId"));
					ycs.CurrencyId = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? -1 : reader.GetInt32(reader.GetOrdinal("CurrencyId"));
						
					ycDesc.Add(ycs);
				}
			}

			return ycDesc;
		}
	
		internal static List<EntryPoint> GetAllEntryPoints(ConnectionContextSQLite ctx, bool isDemo)
		{
			List<EntryPoint> res = new List<EntryPoint>();

			var command = SqlHelperSQLite.GetAllEntryPoints(ctx, isDemo);

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
									? (Instrument)DataLayer.DataHelperSQLite.GetBond(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									: (Instrument)DataLayer.DataHelperSQLite.GetRate(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									);

					res.Add(ep);
				}
			}
			return res;
		}

		internal static void AddEntryPoint(ConnectionContextSQLite ctx, EntryPoint ep)
		{
			var command = SqlHelperSQLite.AddEntryPoint(ctx, ep);
			try
			{
				command.ExecuteNonQuery();
				command.CommandText = "Commit";
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{ }
		}

		internal static void UpdateEntryPoint(ConnectionContextSQLite ctx, EntryPoint ep)
		{
			var command = SqlHelperSQLite.UpdateEntryPoint(ctx, ep);
			try
			{
				command.ExecuteNonQuery();
				command.CommandText = "Commit";
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{ }
		}

		internal static List<EntryPoint> GetEntryPointByInstrument(ConnectionContextSQLite ctx, Instrument instr)
		{
			List<EntryPoint> res = new List<EntryPoint>();

			var command = SqlHelperSQLite.GetEntryPointByInstrument(ctx, instr);

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
									? (Instrument)DataLayer.DataHelperSQLite.GetBond(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									: (Instrument)DataLayer.DataHelperSQLite.GetRate(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									);

					res.Add(tmp);
				}
			}
			return res;
		}

		internal static void AddEntryPointHistory(ConnectionContextSQLite ctx, List<EntryPoint> epl)
		{
			int i = 0;
			foreach (var ep in epl)
			{
				var command = SqlHelperSQLite.AddEntryPointHistory(ctx, ep);
				try
				{
					command.ExecuteNonQuery();
			
					// do commit on last one
					i++;
					if (i == epl.Count)
					{
						command.CommandText = "Commit";
						command.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{ }
			}
		}
	}
}

#endif // _SQLITE_