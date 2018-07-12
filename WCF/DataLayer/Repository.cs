using System;
using System.Collections.Generic;

using ConnectionContext;

namespace DataLayer
{
    public class Repository
    {
		public static Dictionary<long, Rate> RateDic = new Dictionary<long, Rate>();
		public static Dictionary<long, Bond> BondDic = new Dictionary<long, Bond>();
		public static Dictionary<long, Currency> CurrencyDic = new Dictionary<long, Currency>();
		public static Dictionary<long, DayCounter> DayCounterDic = new Dictionary<long, DayCounter>();
		public static Dictionary<long, ExchangeRate> XRateDic = new Dictionary<long, ExchangeRate>();
		public static Dictionary<long, InflationRate> InflationRateDic = new Dictionary<long, InflationRate>();
		public static Dictionary<long, InflationBond> InflationBondDic = new Dictionary<long, InflationBond>();
		public static Dictionary<long, InflationIndex> InflationIndexDic = new Dictionary<long, InflationIndex>();

		public static long GetDayCounterId(string name)
		{
			foreach (KeyValuePair<long, DayCounter> i in DayCounterDic)
			{
				if (i.Value.Name == name)
					return i.Key;
			}
			return -1;
		}
		
        public static List<Currency> GetCurrency(long? idCurrency)
        {
#if _LINQXML_
            return DataHelperLinqXml.GetCurrency(idCurrency);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetCurrency(ctx, idCurrency);
			}
#else	// _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetCurrency(ctx, idCurrency);
            }
#endif
        }

        public static List<YieldCurveFamily> GetYieldCurveFamily(long? idYcFamily, long? idCurrency)
        {
#if _LINQXML_
            return DataHelperLinqXml.GetYieldCurveFamily(idYcFamily, idCurrency);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetYieldCurveFamily(ctx, idYcFamily, idCurrency);
			}
#else // _MYSQL
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetYieldCurveFamily(ctx, idYcFamily, idCurrency);
            }
#endif
        }

        public static List<YieldCurveData> GetYieldCurveData(long? idYc)
        {
#if _LINQXML_
			return DataHelperLinqXml.GetYieldCurveData(idYc);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetYieldCurveData(ctx, idYc);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetYieldCurveData(ctx, idYc);
            }
#endif
        }
		
		public static List<EntryPoint> GetYieldCurveEntryPoint(long? idYc, DateTime? settlementDate)
		{
#if _LINQXML_
			return DataHelperLinqXml.GetYieldCurveEntryData(idYc, settlementDate);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetYieldCurveEntryPoint(ctx, idYc, settlementDate);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
				return DataHelper.GetYieldCurveEntryData(ctx, idYc, settlementDate);
            }
#endif
		}

		public static List<EntryPointHistory> GetYieldCurveEntryPointHistory(long? idYc, DateTime? settlementDate)
        {
#if _LINQXML_
			return DataHelperLinqXml.GetYieldCurveEntryDataHistory(idYc, settlementDate);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetYieldCurveEntryPointHistory(ctx, idYc, settlementDate);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
				return DataHelper.GetYieldCurveEntryDataHistory(ctx, idYc, settlementDate);
            }
#endif
        }

        public static List<Rate> GetRates()
        {
#if _LINQXML_
            return DataHelperLinqXml.GetRates(null);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetRates(ctx, null);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetRates(ctx, null);
            }
#endif
        }

        public static List<Bond> GetBonds()
        {
#if _LINQXML_
            return DataHelperLinqXml.GetBonds(null);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetBonds(ctx, null);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetBonds(ctx, null);
            }
#endif
        }

		public static List<DayCounter> GetDayCounters()
		{
#if _LINQXML_
			return DataHelperLinqXml.GetDayCounter(null, null);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetDayCounter(ctx, null, null);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetDayCounter(ctx, null, null);
            }
#endif
		}
		/*
		public static DayCounter GetDayCounter(string className)
		{
#if _LINQXML_
			return DataHelperLinqXml.GetDayCounter(null, className)[0];
#else
            using (var ctx = new ConnectionContext())
            {
                return DataHelper.GetDayCounter(ctx, className);
            }
#endif
		}
		*/
		public static List<Calendar> GetCalendars()
        {
#if _LINQXML_
            return DataHelperLinqXml.GetCalendars();
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetCalendars(ctx);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetCalendars(ctx);
            }
#endif
        }
		
		public static List<ExchangeRate> GetExchangeRates()
		{
#if _LINQXML_
			return DataHelperLinqXml.GetXRates(null);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetXRates(ctx, null);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				return DataHelper.GetXRates(ctx, null);
			}
#endif
		}
		
		public static List<EntryPointHistory> GetExchangeRateHistory(long? idYc, DateTime? settlementDate)
		{
#if _LINQXML_
			return DataHelperLinqXml.GetExchangeRateHistory(idYc, settlementDate);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetExchangeRateHistory(ctx, idYc, settlementDate);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				return DataHelper.GetExchangeRateHistory(ctx, idYc, settlementDate);
			}
#endif
		}

		#region INFLATION CURVE

		public static List<YieldCurveFamily> GetInflationCurveFamily(long? idICFamily, long? idCurrency)
		{
#if _LINQXML_
        //    return DataHelperLinqXml.GetInflationCurveFamily(idICFamily, idCurrency);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetInflationCurveFamily(ctx, idICFamily, idCurrency);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetInflationCurveFamily(ctx, idICFamily, idCurrency);
            }
#endif
		}

		public static List<InflationCurveData> GetInflationCurveData(long? idIc)
		{
#if _LINQXML_
//			return DataHelperLinqXml.GetInflationCurveData(idIc);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetInflationCurveData(ctx, idIc);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetInflationCurveData(ctx, idIc);
            }
#endif
		}

		public static List<InflationCurveEntryData> GetInflationCurveEntryData(long? idIc, DateTime? settlementDate)
		{

#if _LINQXML_
//			return DataHelperLinqXml.GetInflationCurveEntryData(idIc, settlementDate);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetInflationCurveEntryData(ctx, idIc, settlementDate);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetInflationCurveEntryData(ctx, idIc, settlementDate);
            }
#endif
		}

		public static List<InflationBond> GetInflationBonds(long? idBond)
		{

#if _LINQXML_
//			return DataHelperLinqXml.GetInflationBonds(idBond);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetInflationBonds(ctx, idBond);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetInflationBonds(ctx, idBond);
            }
#endif
		}

		public static List<InflationIndex> GetInflationIndex(long? idIndex)
		{

#if _LINQXML_
//			return DataHelperLinqXml.GetInflationIndex(idIndex);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetInflationIndex(ctx, idIndex);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetInflationIndex(ctx, idIndex);
            }
#endif
		}

		public static List<RateHistory> GetRateHistory(DateTime? settlementDate)
		{
#if _LINQXML_
//			return DataHelperLinqXml.GetRateHistory(settlementDate);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetRateHistory(ctx, settlementDate);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetRateHistory(ctx, settlementDate);
            }
#endif
		}

		public static List<InflationRate> GetInflationRates()
		{
#if _LINQXML_
      //      return DataHelperLinqXml.GetInflationRates(null);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetInflationRates(ctx, null);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetInflationRates(ctx, null);
            }
#endif
		}

		#endregion
    }
}
