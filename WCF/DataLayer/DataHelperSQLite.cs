#define _SQLITE_

#if _SQLITE_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Data.SQLite;

using ConnectionContext;

namespace DataLayer
{
	/// <summary>
	/// The Data Helper.
	/// </summary>
	public class DataHelperSQLite
	{
		#region Methods

		internal static List<Currency> GetCurrency(ConnectionContextSQLite ctx, long? idCurrency)
		{
			List<Currency> list = new List<Currency>();

			var command = SqlHelperSQLite.SelectCurrencyCommand(ctx, idCurrency);
			using (var reader = command.ExecuteReader())
			{
				//Repository.CurrencyDic.Clear();

				while (reader.Read())
				{
					var r = new Currency
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName")) ? null : reader.GetString(reader.GetOrdinal("ClassName")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code")),
						NumericCode = reader.IsDBNull(reader.GetOrdinal("NumericCode")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumericCode")),
						FractionsPerUnit = reader.IsDBNull(reader.GetOrdinal("FractionsPerUnit")) ? 0 : reader.GetInt32(reader.GetOrdinal("FractionsPerUnit")),
						FractionSymbol = reader.IsDBNull(reader.GetOrdinal("FractionSymbol")) ? null : reader.GetString(reader.GetOrdinal("FractionSymbol")),
						Symbol = reader.IsDBNull(reader.GetOrdinal("Symbol")) ? null : reader.GetString(reader.GetOrdinal("Symbol"))
					};

					list.Add(r);
					//Repository.CurrencyDic[r.Id] = r;
				}
			}
			return list;
		}

		/// Retrieves the list of yield curve families
		/// </summary>
		/// <param name="ctx"> 
		/// The connection context
		/// </param>
		/// <param name="idYcFamily"> 
		/// The id of yield curve family
		/// </param>
		/// <param name="idCcy"> 
		/// The currency id
		/// </param>
		/// <returns>
		/// List of yield curve families
		/// </returns>

		internal static List<YieldCurveFamily> GetYieldCurveFamily(ConnectionContextSQLite ctx, long? idYcFamily, long? idCcy)
		{
			List<YieldCurveFamily> list = new List<YieldCurveFamily>();

			var command = SqlHelperSQLite.SelectYieldCurveFamilyCommand(ctx, idYcFamily, idCcy);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var r = new YieldCurveFamily
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						CurrencyId = reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						Name = reader.GetString(reader.GetOrdinal("Name"))
					};

					list.Add(r);
				}
			}
			return list;
		}

		/// <summary>
		/// Retrieves the list of yield curve data
		/// </summary>
		/// <param name="ctx"> 
		/// The connection context
		/// </param>
		/// <param name="idYc">
		/// The yield curve id
		/// </param>
		/// <param name="idYc">
		/// The date
		/// </param>
		/// <returns>
		/// List of yield curve data
		/// </returns>
		/// 
		// ??? obsolete method

		internal static List<YieldCurveData> GetYieldCurveData(ConnectionContextSQLite ctx,
																		long? idYc)
		{
			List<YieldCurveData> ycDesc = new List<YieldCurveData>();

			var command = SqlHelperSQLite.SelectYieldCurveDataCommand(ctx, idYc);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var ycs = new YieldCurveSetting
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

					ycDesc.Add(new YieldCurveData
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.GetString(reader.GetOrdinal("Name")),
						Family = reader.GetString(reader.GetOrdinal("Family")),
						CurrencyId = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? -1 : reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						settings = ycs,
						entryPointList = null,
						discountPointList = null
					});
				}
			}

			return ycDesc;
		}

		internal static List<EntryPoint> GetYieldCurveEntryPoint(ConnectionContextSQLite ctx, long? idYc, DateTime? settlementDate)
		{
			List<EntryPoint> outEntryPointList = new List<EntryPoint>();

			var command = SqlHelperSQLite.SelectYieldCurveEntryPointCommand(ctx, idYc, settlementDate);

			using (var reader = command.ExecuteReader())
			{
				//Dictionary<long, HistoricValue> tmpEntryPointDic = new Dictionary<long, HistoricValue>();

				while (reader.Read())
				{
					var d = new EntryPoint
					{
						YieldCurveId = reader.GetInt32(reader.GetOrdinal("YieldCurveId")),
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						Length = reader.IsDBNull(reader.GetOrdinal("Length")) ? 0 : reader.GetInt32(reader.GetOrdinal("Length")),
						TimeUnit = reader.IsDBNull(reader.GetOrdinal("TimeUnit")) ? null : reader.GetString(reader.GetOrdinal("TimeUnit")),
						DataProviderId = reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.GetString(reader.GetOrdinal("DataReference")),
						Enabled = true, // default
						ValidDateBegin = reader.GetDateTime(reader.GetOrdinal("ValidDateStart")),
						ValidDateEnd = reader.GetDateTime(reader.GetOrdinal("ValidDateEnd")),

						epValue = new HistoricValue
						{
							Value = reader.GetDouble(reader.GetOrdinal("Value")),
							Date = reader.GetDateTime(reader.GetOrdinal("Date"))
						}
					};

					d.Instrument = (d.Type == "bond"
									? (Instrument)GetBond(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									: (Instrument)GetRate(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									);

					if (d.Type == "bond" && (d.Instrument as Bond).MaturityDate <= settlementDate)
						continue;		// skip if bond and maturity date is not in future
					/*
					// if does not exist yet -> will create new record with values
					tmpEntryPointDic[d.Id] = new HistoricValue
					{
						Value = reader.GetDouble(reader.GetOrdinal("Value")),
						Date = reader.GetDateTime(reader.GetOrdinal("Date"))
					};
					*/
					outEntryPointList.Add(d);
				}
				/*
				// when all data are read - put entry points values into output list
				foreach (EntryPoint y in outEntryPointList)
				{
					y.epValue = tmpEntryPointDic[y.Id];
				}
				*/

				// sort list by durations
				outEntryPointList.Sort(new EntryPointCompare());
			}

			return outEntryPointList;
		}

		internal static List<EntryPointHistory> GetYieldCurveEntryPointHistory(ConnectionContextSQLite ctx,
																	  long? idYc,
																	  DateTime? settlementDate)
		{
			List<EntryPointHistory> outEntryPointHistoryList = new List<EntryPointHistory>();

			var command = SqlHelperSQLite.SelectYieldCurveEntryPointHistoryCommand(ctx, idYc, null);

			using (var reader = command.ExecuteReader())
			{
				Dictionary<long, HashSet<HistoricValue>> tmpEntryPointListDic = new Dictionary<long, HashSet<HistoricValue>>();

				while (reader.Read())
				{
					var d = new EntryPointHistory
					{
						YieldCurveId = reader.GetInt32(reader.GetOrdinal("YieldCurveId")),
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						Length = reader.IsDBNull(reader.GetOrdinal("Length")) ? 0 : reader.GetInt32(reader.GetOrdinal("Length")),
						TimeUnit = reader.IsDBNull(reader.GetOrdinal("TimeUnit")) ? null : reader.GetString(reader.GetOrdinal("TimeUnit")),
						DataProviderId = reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.GetString(reader.GetOrdinal("DataReference")),
						ValidDateBegin = reader.GetDateTime(reader.GetOrdinal("ValidDateBegin")),
						ValidDateEnd = reader.GetDateTime(reader.GetOrdinal("ValidDateEnd")),
						Enabled = true,
						epValueHistory = null
					};

					d.Instrument = (d.Type == "bond"
									? (Instrument)GetBond(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									: (Instrument)GetRate(reader.GetInt32(reader.GetOrdinal("RateId")), ctx)
									);

					if (d.Type == "bond" && (d.Instrument as Bond).MaturityDate <= settlementDate)
						continue;		// skip if bond and maturity date is not in future

					// entry point is read 1st time (1st historic entry) => create new entry in output list + create history hashset
					if (!tmpEntryPointListDic.ContainsKey(d.Id))
					{
						tmpEntryPointListDic[d.Id] = new HashSet<HistoricValue>(new HistoricValueComparer());
						outEntryPointHistoryList.Add(d);
					}

					// add new historic value to the hashset
					tmpEntryPointListDic[d.Id].Add(new HistoricValue
					{
						Value = reader.GetDouble(reader.GetOrdinal("Value")),
						Date = reader.GetDateTime(reader.GetOrdinal("Date"))
					});
				}

				// entryPointSet of output entry points is still null - assign historic values from tmp dictionary constructed
				foreach (EntryPointHistory y in outEntryPointHistoryList)
				{
					y.epValueHistory = tmpEntryPointListDic[y.Id];
					y.epValue = y.epValueHistory.LastOrDefault(i => i.Date <= settlementDate);
				}

				// sort by durations
				outEntryPointHistoryList.Sort(new EntryPointHistoryCompare());
			}

			return outEntryPointHistoryList;
		}

		#region ---------------------------All  Rates  --------------------------------------------
		/// <summary>
		/// Retrieves the list of all rates and their data (only for custom rates, for Quantlib rates -Class names)
		/// </summary>
		/// <param name="ctx"> 
		/// The connection context
		/// </param>
		/// </returns>

		public static Rate GetRate(long? idRate, ConnectionContextSQLite ctx)
		{
			if (idRate != null && Repository.RateDic.ContainsKey(idRate.Value))
				return Repository.RateDic[idRate.Value];

			//var ctx = new ConnectionContextSQLite();
			return GetRates(ctx, idRate)[0];
		}
		internal static List<Rate> GetRates(ConnectionContextSQLite ctx, long? idRate)
		{
			List<Rate> list = new List<Rate>();

			var command = SqlHelperSQLite.SelectRatesCommand(ctx, idRate);

			using (var reader = command.ExecuteReader())
			{
				//Repository.RateDic.Clear();

				while (reader.Read())
				{
					Rate tmp = new Rate
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? null : reader.GetString(reader.GetOrdinal("DataReference")),
						IdCcy = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						Duration = reader.IsDBNull(reader.GetOrdinal("Length")) ? 0 : reader.GetInt32(reader.GetOrdinal("Length")),
						TimeUnit = reader.IsDBNull(reader.GetOrdinal("TimeUnit")) ? null : reader.GetString(reader.GetOrdinal("TimeUnit")),
						Accuracy = reader.IsDBNull(reader.GetOrdinal("Accuracy")) ? 0 : reader.GetInt32(reader.GetOrdinal("Accuracy")),
						Spread = reader.IsDBNull(reader.GetOrdinal("Spread")) ? 0 : reader.GetDouble(reader.GetOrdinal("Spread")),
						SettlementDays = reader.IsDBNull(reader.GetOrdinal("SettlementDays")) ? 0 : reader.GetInt32(reader.GetOrdinal("SettlementDays")),
						FixingPlace = reader.IsDBNull(reader.GetOrdinal("FixingPlace")) ? null : reader.GetString(reader.GetOrdinal("FixingPlace")), //??? temporary solution. fixing place in database is id whereas here we are looking for quantlibclassname of calendar
						IdIndex = reader.IsDBNull(reader.GetOrdinal("IndexId")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndexId")),

						BasisId = reader.IsDBNull(reader.GetOrdinal("BasisId")) ? -1 : reader.GetInt32(reader.GetOrdinal("BasisId")),
						Frequency = reader.IsDBNull(reader.GetOrdinal("Frequency")) ? null : reader.GetString(reader.GetOrdinal("Frequency")),
						Compounding = reader.IsDBNull(reader.GetOrdinal("Compounding")) ? null : reader.GetString(reader.GetOrdinal("Compounding")),
						IndexDuration = reader.IsDBNull(reader.GetOrdinal("IndexLength")) ? 0 : reader.GetInt32(reader.GetOrdinal("IndexLength")),
						IndexTimeUnit = reader.IsDBNull(reader.GetOrdinal("IndexTimeUnit")) ? null : reader.GetString(reader.GetOrdinal("IndexTimeUnit")),
						IndexName = reader.IsDBNull(reader.GetOrdinal("IndexName")) ? null : reader.GetString(reader.GetOrdinal("IndexName")),
						ClassNameIndex = reader.IsDBNull(reader.GetOrdinal("IndexClassName")) ? null : reader.GetString(reader.GetOrdinal("IndexClassName")),

						BasisIndexId = reader.IsDBNull(reader.GetOrdinal("IndexBasisId")) ? -1 : reader.GetInt32(reader.GetOrdinal("IndexBasisId")),
						FrequencyIndex = reader.IsDBNull(reader.GetOrdinal("IndexFrequency")) ? null : reader.GetString(reader.GetOrdinal("IndexFrequency")),
						CompoundingIndex = reader.IsDBNull(reader.GetOrdinal("IndexCompounding")) ? null : reader.GetString(reader.GetOrdinal("IndexCompounding")),
					};

					list.Add(tmp);
					//Repository.RateDic[tmp.Id] = tmp;
				}
			}

			return list;
		}

		#endregion ----------------------------------------------------------------------------------


		#region ---------------------------All  Bonds  --------------------------------------------
		/// <summary>
		/// Retrieves the list of all bonds and their data
		/// </summary>
		/// <param name="ctx"> 
		/// The connection context
		/// </param>
		/// </returns>

		public static Bond GetBond(long? idBond, ConnectionContextSQLite ctx)
		{
			if (idBond != null && Repository.BondDic.ContainsKey(idBond.Value))
				return Repository.BondDic[idBond.Value];

			//var ctx = new ConnectionContextSQLite();
			return GetBonds(ctx, idBond)[0];
		}
		internal static List<Bond> GetBonds(ConnectionContextSQLite ctx, long? idBond)
		{
			List<Bond> list = new List<Bond>();

			var command = SqlHelperSQLite.SelectBondsCommand(ctx, idBond);

			using (var reader = command.ExecuteReader())
			{
				//Repository.BondDic.Clear();

				while (reader.Read())
				{
					Bond b = new Bond
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? null : reader.GetString(reader.GetOrdinal("DataReference")),
						IdCcy = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
						MaturityDate = reader.GetDateTime(reader.GetOrdinal("MaturityDate")),
						DateGeneration = reader.IsDBNull(reader.GetOrdinal("DateGeneration")) ? null : reader.GetString(reader.GetOrdinal("DateGeneration")),
						CouponFrequency = reader.IsDBNull(reader.GetOrdinal("CouponFrequency")) ? null : reader.GetString(reader.GetOrdinal("CouponFrequency")),
						Coupon = reader.IsDBNull(reader.GetOrdinal("Coupon")) ? 0 : reader.GetDouble(reader.GetOrdinal("Coupon")),
						CouponConvention = reader.IsDBNull(reader.GetOrdinal("CouponConvention")) ? null : reader.GetString(reader.GetOrdinal("CouponConvention")),
						//CouponBasis = reader.IsDBNull(reader.GetOrdinal("CouponBasis")) ? null : reader.GetString(reader.GetOrdinal("CouponBasis")),
						CouponBasisId = reader.IsDBNull(reader.GetOrdinal("CouponBasisId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CouponBasisId")),
						Redemption = reader.IsDBNull(reader.GetOrdinal("Redemption")) ? 0 : reader.GetDouble(reader.GetOrdinal("Redemption")),
						Nominal = reader.IsDBNull(reader.GetOrdinal("Nominal")) ? 0 : reader.GetDouble(reader.GetOrdinal("Nominal")),
						SettlementDays = reader.IsDBNull(reader.GetOrdinal("SettlementDays")) ? 0 : reader.GetInt32(reader.GetOrdinal("SettlementDays")),
						//YieldCurveId = reader.IsDBNull(reader.GetOrdinal("YieldCurveId")) ? 0 : reader.GetInt32(reader.GetOrdinal("YieldCurveId")),

					};

					list.Add(b);
					//Repository.BondDic[b.Id] = b;
				}
			}

			return list;
		}

		#endregion ----------------------------------------------------------------------------------

		#region --------------------------All  Markets Method --------------------------------------
		/// <summary>
		/// Retrieves the list of all markets and their data (only for custom markets, for Quantlib markets -Class names and Market names)
		/// </summary>
		/// <param name="ctx"> 
		/// The connection context
		/// </param>
		/// </returns>

		internal static List<Calendar> GetCalendars(ConnectionContextSQLite ctx)
		{
			List<Calendar> list = new List<Calendar>();

			var command = SqlHelperSQLite.SelectCalendarsCommand(ctx);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					Calendar m = new Calendar
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
						MarketName = reader.GetString(reader.GetOrdinal("MarketName")),
					};

					list.Add(m);
				}
			}
			return list;
		}

		internal static List<DayCounter> GetDayCounter(ConnectionContextSQLite ctx, long? idDC, string className)
		{
			List<DayCounter> list = new List<DayCounter>();

			var command = SqlHelperSQLite.SelectDayCounterCommand(ctx, idDC, className);
			using (var reader = command.ExecuteReader())
			{
				//Repository.DayCounterDic.Clear();
				while (reader.Read())
				{
					var r = new DayCounter
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName")) ? null : reader.GetString(reader.GetOrdinal("ClassName"))
					};

					string param = reader.IsDBNull(reader.GetOrdinal("ClassNameParam")) ? null : reader.GetString(reader.GetOrdinal("ClassNameParam"));
					if (!string.IsNullOrEmpty(param))
						r.ClassName += ("::" + param);

					list.Add(r);
					//Repository.DayCounterDic[r.Id] = r;
				}
			}

			return list;
		}

		internal static ExchangeRate GetXRate(long? idXRate)
		{
			if (idXRate != null && Repository.XRateDic.ContainsKey(idXRate.Value))
				return Repository.XRateDic[idXRate.Value];

			var ctx = new ConnectionContextSQLite();
			return GetXRates(ctx, idXRate)[0];
		}

		internal static List<ExchangeRate> GetXRates(ConnectionContextSQLite ctx, long? idXRate)
		{
			List<ExchangeRate> list = new List<ExchangeRate>();

			var command = SqlHelperSQLite.SelectXRatesCommand(ctx, idXRate);

			using (var reader = command.ExecuteReader())
			{
				//Repository.XRateDic.Clear();
				while (reader.Read())
				{
					ExchangeRate tmp = new ExchangeRate
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName")) ? null : reader.GetString(reader.GetOrdinal("ClassName")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? null : reader.GetString(reader.GetOrdinal("DataReference")),
						SettlementDays = reader.IsDBNull(reader.GetOrdinal("SettlementDays")) ? 0 : reader.GetInt32(reader.GetOrdinal("SettlementDays")),
						FixingPlace = reader.IsDBNull(reader.GetOrdinal("FixingPlace")) ? null : reader.GetString(reader.GetOrdinal("FixingPlace")), //??? temporary solution. fixing place in database is id whereas here we are looking for quantlibclassname of calendar
						PrimaryCurrencyId = reader.IsDBNull(reader.GetOrdinal("PrimaryCurrencyCode")) ? 0 : reader.GetInt32(reader.GetOrdinal("PrimaryCurrencyCode")),
						SecondaryCurrencyId = reader.IsDBNull(reader.GetOrdinal("SecondaryCurrencyCode")) ? 0 : reader.GetInt32(reader.GetOrdinal("SecondaryCurrencyCode"))
					};

					list.Add(tmp);
					//Repository.XRateDic[tmp.Id] = tmp;
				}
			}

			return list;
		}

		#endregion ----------------------------------------------------------------------------------

		internal static List<EntryPointHistory> GetExchangeRateHistory(ConnectionContextSQLite ctx,
																				long? RateId,
																				DateTime? settlementDate)
		{
			List<EntryPointHistory> outExchangeRateHistoryList = new List<EntryPointHistory>();

			var command = SqlHelperSQLite.SelectExchangeRateHistoryCommand(ctx, RateId, null);

			using (var reader = command.ExecuteReader())
			{
				Dictionary<long, HashSet<HistoricValue>> tmpExchangeRateListDic = new Dictionary<long, HashSet<HistoricValue>>();

				while (reader.Read())
				{
					var d = new EntryPointHistory
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? null : reader.GetString(reader.GetOrdinal("DataReference")),

						epValueHistory = null
					};

					if (!tmpExchangeRateListDic.ContainsKey(d.Id))
					{
						tmpExchangeRateListDic[d.Id] = new HashSet<HistoricValue>(new HistoricValueComparer());
						outExchangeRateHistoryList.Add(d);
					}

					tmpExchangeRateListDic[d.Id].Add(new HistoricValue
					{
						Value = reader.GetDouble(reader.GetOrdinal("Value")),
						Date = reader.GetDateTime(reader.GetOrdinal("Date"))
					});
				}

				foreach (EntryPointHistory y in outExchangeRateHistoryList)
				{
					y.epValueHistory = tmpExchangeRateListDic[y.Id];
				}

				outExchangeRateHistoryList.Sort(new EntryPointHistoryCompare());
			}

			return outExchangeRateHistoryList;
		}

		#endregion --------------------------------------------------------------------

		#region INFLATION CURVE

		internal static List<YieldCurveFamily> GetInflationCurveFamily(ConnectionContextSQLite ctx, long? idICFamily, long? idCcy)
		{
			List<YieldCurveFamily> list = new List<YieldCurveFamily>();

			var command = SqlHelperSQLite.SelectInflationCurveFamilyCommand(ctx, idICFamily, idCcy);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var r = new YieldCurveFamily
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						CurrencyId = reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						Name = reader.GetString(reader.GetOrdinal("Name"))
					};

					list.Add(r);
				}
			}
			return list;
		}

		// settings and name
		internal static List<InflationCurveData> GetInflationCurveData(ConnectionContextSQLite ctx, long? idIC)
		{
			List<InflationCurveData> ic = new List<InflationCurveData>();

			var command = SqlHelperSQLite.SelectInflationCurveDataCommand(ctx, idIC);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var ics = new InflationCurveSetting
					{
						ZcBasisId = reader.GetInt32(reader.GetOrdinal("ZcBasisId")),
						ZcCompounding = reader.GetString(reader.GetOrdinal("ZcCompounding")),
						ZcFrequency = reader.GetString(reader.GetOrdinal("ZcFrequency")),
						ifZc = reader.GetBoolean(reader.GetOrdinal("ifZC")),
						ZCColor = reader.IsDBNull(reader.GetOrdinal("ZCColor")) ? "" : reader.GetString(reader.GetOrdinal("ZCColor")),
						ifIndex = reader.GetBoolean(reader.GetOrdinal("ifIndex")),
						IndexColor = reader.IsDBNull(reader.GetOrdinal("IndexColor")) ? "" : reader.GetString(reader.GetOrdinal("IndexColor"))
					};

					//   Calendar cal = new Calendar
					//   {
					//       Id = reader.IsDBNull(reader.GetOrdinal("calId")) ? -1 : reader.GetInt32(reader.GetOrdinal("calId")),
					//       ClassName = reader.IsDBNull(reader.GetOrdinal("calClassName")) ? null : reader.GetString(reader.GetOrdinal("calClassName")),
					//       MarketName = reader.IsDBNull(reader.GetOrdinal("calMarketName")) ? null : reader.GetString(reader.GetOrdinal("calMarketName"))
					//   };

					//   ycs.Calendar = cal;

					ic.Add(new InflationCurveData
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.GetString(reader.GetOrdinal("Name")),
						Family = reader.GetString(reader.GetOrdinal("Family")),
						//CurrencyId = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? -1 : reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						//InflationIndexId = reader.IsDBNull(reader.GetOrdinal("InflationIndex")) ? -1 : reader.GetInt32(reader.GetOrdinal("InflationIndex")),
						CurrencyId = reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						InflationIndexId = reader.GetInt32(reader.GetOrdinal("InflationIndex")),

						settings = ics
					});
				}
			}

			return ic;
		}

		// instruments used for callibration and valid start date and valid end date
		internal static List<InflationCurveEntryData> GetInflationCurveEntryData(ConnectionContextSQLite ctx, long? idIc, DateTime? settlementDate)
		{
			List<InflationCurveEntryData> EntryDataList = new List<InflationCurveEntryData>();

			var command = SqlHelperSQLite.SelectInflationCurveEntryDataCommand(ctx, idIc, settlementDate);

			using (var reader = command.ExecuteReader())
			{
				// Dictionary<long, ValuePair> entryPointDic = new Dictionary<long, ValuePair>();

				while (reader.Read())
				{
					var d = new InflationCurveEntryData
					{
						InflationCurveId = reader.GetInt32(reader.GetOrdinal("InflationCurveId")),
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						Enabled = true, // default
						ValidDateBegin = reader.GetDateTime(reader.GetOrdinal("ValidDateStart")),
						ValidDateEnd = reader.GetDateTime(reader.GetOrdinal("ValidDateEnd")),
					};

					if (d.Type.ToLower() == "inflationbond")
					{
						d.Instrument = (InflationBond)GetInflationBond(reader.GetInt32(reader.GetOrdinal("RateId")));
					}
					else if (d.Type.ToLower() == "swap")
					{
						d.Instrument = (InflationRate)GetInflationRate(reader.GetInt32(reader.GetOrdinal("RateId")));
					}
					/*    else if (d.Type.ToLower() == "bond")
						{
							d.Instrument = (Instrument)GetBond(reader.GetInt32(reader.GetOrdinal("RateId")));
						}
						else
						{
							d.Instrument = (Instrument)GetRate(reader.GetInt32(reader.GetOrdinal("RateId")));
						} */


					//    if (((d.Type == "inflationbond") || (d.Type == "bond")) && (d.Instrument as Bond).MaturityDate <= settlementDate)
					//         continue;		// skip if bond and maturity date is not in future

					if ((d.Type == "inflationbond") && (d.Instrument as InflationBond).MaturityDate <= settlementDate)
						//     if ((d.Type == "inflationbond") && (d.Instrument as Bond).MaturityDate <= settlementDate)
						continue;		// skip if bond and maturity date is not in future


					EntryDataList.Add(d);
				}
			}

			EntryDataList.Sort(new InflationCurveEntryDataCompare());
			return EntryDataList;
		}

		internal static List<InflationBond> GetInflationBonds(ConnectionContextSQLite ctx, long? idBond)
		//  internal static List<Bond> GetInflationBonds(ConnectionContext ctx, long? idBond)
		{
			List<InflationBond> list = new List<InflationBond>();
			//  List<Bond> list = new List<Bond>();

			var command = SqlHelperSQLite.SelectInflationBondsCommand(ctx, idBond);

			using (var reader = command.ExecuteReader())
			{
				//Repository.BondDic.Clear();

				while (reader.Read())
				{
					InflationBond b = new InflationBond
					//    Bond b = new Bond
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						InflationIndexId = reader.IsDBNull(reader.GetOrdinal("InflationIndexId")) ? 0 : reader.GetInt32(reader.GetOrdinal("InflationIndexId")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? null : reader.GetString(reader.GetOrdinal("DataReference")),
						IdCcy = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
						MaturityDate = reader.GetDateTime(reader.GetOrdinal("MaturityDate")),
						DateGeneration = reader.IsDBNull(reader.GetOrdinal("DateGeneration")) ? null : reader.GetString(reader.GetOrdinal("DateGeneration")),
						CouponFrequency = reader.IsDBNull(reader.GetOrdinal("CouponFrequency")) ? null : reader.GetString(reader.GetOrdinal("CouponFrequency")),
						Coupon = reader.IsDBNull(reader.GetOrdinal("Coupon")) ? 0 : reader.GetDouble(reader.GetOrdinal("Coupon")),
						CouponConvention = reader.IsDBNull(reader.GetOrdinal("CouponConvention")) ? null : reader.GetString(reader.GetOrdinal("CouponConvention")),
						//CouponBasis = reader.IsDBNull(reader.GetOrdinal("CouponBasis")) ? null : reader.GetString(reader.GetOrdinal("CouponBasis")),
						CouponBasisId = reader.IsDBNull(reader.GetOrdinal("CouponBasisId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CouponBasisId")),
						Redemption = reader.IsDBNull(reader.GetOrdinal("Redemption")) ? 0 : reader.GetDouble(reader.GetOrdinal("Redemption")),
						Nominal = reader.IsDBNull(reader.GetOrdinal("Nominal")) ? 0 : reader.GetDouble(reader.GetOrdinal("Nominal")),
						SettlementDays = reader.IsDBNull(reader.GetOrdinal("SettlementDays")) ? 0 : reader.GetInt32(reader.GetOrdinal("SettlementDays")),
						//YieldCurveId = reader.IsDBNull(reader.GetOrdinal("YieldCurveId")) ? 0 : reader.GetInt32(reader.GetOrdinal("YieldCurveId")),

					};

					list.Add(b);
					//Repository.BondDic[b.Id] = b;
				}
			}

			return list;
		}

		internal static InflationBond GetInflationBond(long? idInflationBond)
		//     internal static Bond GetInflationBond(long? idInflationBond)
		{
			//  if (idInflationBond != null && Repository.inf.BondDic.ContainsKey(idBond.Value))
			//      return Repository.BondDic[idBond.Value];

			var ctx = new ConnectionContextSQLite();
			return GetInflationBonds(ctx, idInflationBond)[0];
		}

		internal static List<InflationIndex> GetInflationIndex(ConnectionContextSQLite ctx, long? idIndex)
		{
			List<InflationIndex> list = new List<InflationIndex>();

			var command = SqlHelperSQLite.SelectInflationIndexCommand(ctx, idIndex);

			using (var reader = command.ExecuteReader())
			{
				//Repository.BondDic.Clear();

				while (reader.Read())
				{
					InflationIndex b = new InflationIndex
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName")) ? null : reader.GetString(reader.GetOrdinal("ClassName")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? null : reader.GetString(reader.GetOrdinal("DataReference")),
						IdCcy = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CurrencyId")),

					};

					list.Add(b);

				}
			}

			return list;
		}

		//pair of params can be added later - typy (bond, rate etc) and instrument id
		//note that id is not unique as rate history contains different types of instruments from different tables (ids are unique only within one table)
		internal static List<RateHistory> GetRateHistory(ConnectionContextSQLite ctx, DateTime? settlementDate)
		{
			List<RateHistory> RateHistoryList = new List<RateHistory>();

			var command = SqlHelperSQLite.SelectRateHistoryCommand(ctx, null, settlementDate);

			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var d = new RateHistory
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						RateId = reader.GetInt32(reader.GetOrdinal("RateId")),
						Date = reader.GetDateTime(reader.GetOrdinal("Date")),
						High = reader.IsDBNull(reader.GetOrdinal("High")) ? -1 : reader.GetDouble(reader.GetOrdinal("High")),
						Low = reader.IsDBNull(reader.GetOrdinal("Low")) ? -1 : reader.GetDouble(reader.GetOrdinal("Low")),
						Open = reader.IsDBNull(reader.GetOrdinal("Open")) ? -1 : reader.GetDouble(reader.GetOrdinal("Open")),
						Close = reader.IsDBNull(reader.GetOrdinal("Close")) ? -1 : reader.GetDouble(reader.GetOrdinal("Close")),
						Theoretical = reader.IsDBNull(reader.GetOrdinal("Theoretical")) ? -1 : reader.GetDouble(reader.GetOrdinal("Theoretical"))
					};
					RateHistoryList.Add(d);
				}
			}

			return RateHistoryList;
		}

		internal static InflationRate GetInflationRate(long? idRate)
		{
			if (idRate != null && Repository.InflationRateDic.ContainsKey(idRate.Value))
				return Repository.InflationRateDic[idRate.Value];

			var ctx = new ConnectionContextSQLite();
			return GetInflationRates(ctx, idRate)[0];
		}

		internal static List<InflationRate> GetInflationRates(ConnectionContextSQLite ctx, long? idRate)
		{
			List<InflationRate> list = new List<InflationRate>();

			var command = SqlHelperSQLite.SelectInflationRatesCommand(ctx, idRate);

			using (var reader = command.ExecuteReader())
			{
				//Repository.RateDic.Clear();

				while (reader.Read())
				{
					InflationRate tmp = new InflationRate
					{
						Id = reader.GetInt32(reader.GetOrdinal("Id")),
						Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
						ClassName =
						reader.IsDBNull(reader.GetOrdinal("ClassName")) ? null : reader.GetString(reader.GetOrdinal("ClassName")),
						Type = reader.GetString(reader.GetOrdinal("Type")),
						DataProviderId = reader.IsDBNull(reader.GetOrdinal("DataProviderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("DataProviderId")),
						DataReference = reader.IsDBNull(reader.GetOrdinal("DataReference")) ? null : reader.GetString(reader.GetOrdinal("DataReference")),
						IdCcy = reader.IsDBNull(reader.GetOrdinal("CurrencyId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CurrencyId")),
						InflationCurveId = reader.IsDBNull(reader.GetOrdinal("InflationCurveId")) ? 0 : reader.GetInt32(reader.GetOrdinal("InflationCurveId")),

						Duration = reader.IsDBNull(reader.GetOrdinal("Length")) ? 0 : reader.GetInt32(reader.GetOrdinal("Length")),
						TimeUnit = reader.IsDBNull(reader.GetOrdinal("TimeUnit")) ? null : reader.GetString(reader.GetOrdinal("TimeUnit")),
						Accuracy = reader.IsDBNull(reader.GetOrdinal("Accuracy")) ? 0 : reader.GetInt32(reader.GetOrdinal("Accuracy")),
						SettlementDays = reader.IsDBNull(reader.GetOrdinal("SettlementDays")) ? 0 : reader.GetInt32(reader.GetOrdinal("SettlementDays")),

						InflationIndexId = reader.IsDBNull(reader.GetOrdinal("InflationIndexId")) ? 0 : reader.GetInt32(reader.GetOrdinal("InflationIndexId")),
						InflationLag = reader.IsDBNull(reader.GetOrdinal("InflationLag")) ? 0 : reader.GetInt32(reader.GetOrdinal("InflationLag")),
						InflationLagTimeUnit = reader.IsDBNull(reader.GetOrdinal("InflationLagTimeUnit")) ? null : reader.GetString(reader.GetOrdinal("InflationLagTimeUnit")),

						BasisId = reader.IsDBNull(reader.GetOrdinal("BasisId")) ? -1 : reader.GetInt32(reader.GetOrdinal("BasisId")),
						FrequencyId = reader.IsDBNull(reader.GetOrdinal("FrequencyId")) ? -1 : reader.GetInt32(reader.GetOrdinal("FrequencyId")),
						Frequency = reader.IsDBNull(reader.GetOrdinal("Frequency")) ? null : reader.GetString(reader.GetOrdinal("Frequency")),
						CompoundingId = reader.IsDBNull(reader.GetOrdinal("CompoundingId")) ? -1 : reader.GetInt32(reader.GetOrdinal("CompoundingId")),
						Compounding = reader.IsDBNull(reader.GetOrdinal("Compounding")) ? null : reader.GetString(reader.GetOrdinal("Compounding")),
						Spread = reader.IsDBNull(reader.GetOrdinal("Spread")) ? 0 : reader.GetDouble(reader.GetOrdinal("Spread")),
					};

					list.Add(tmp);
				}
			}

			return list;
		}

		#endregion 
	}
}

#endif // _SQLITE_