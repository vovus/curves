#define _LINQXML_

#if _LINQXML_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataLayer
{
	/// <summary>
	/// The Data Helper.
	/// </summary>
	internal class DataHelperLinqXml
	{
		#region Methods

		internal static List<Currency> GetCurrency(long? idCurrency)
        {
            List<Currency> list = null;
            SqlHelperXml.SelectCurrencyCommand(out list, idCurrency);
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

        internal static List<YieldCurveFamily> GetYieldCurveFamily(long? idYcFamily, long? idCcy)
        {
			List<YieldCurveFamily> list = null;
            SqlHelperXml.SelectYieldCurveFamilyCommand(out list, idYcFamily, idCcy);
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

		internal static List<YieldCurveData> GetYieldCurveData(long? idYc)
        {
            List<YieldCurveData> ycDesc = null;

			SqlHelperXml.SelectYieldCurveDataCommand(out ycDesc, idYc);

            return ycDesc;
        }

		internal static List<EntryPoint> GetYieldCurveEntryData(long? idYc, DateTime? settlementDate)
		{
			List<EntryPoint> EntryDataList = null;

			SqlHelperXml.SelectYieldCurveEntryDataCommand(out EntryDataList, idYc, settlementDate);
			
			return EntryDataList;
		}

		internal static List<EntryPointHistory> GetYieldCurveEntryDataHistory(long? idYc, DateTime? settlementDate)
        {
            List<EntryPointHistory> outEntryDataHistoryList = new List<EntryPointHistory>();

			List<EntryPointHistory> tmp = null;
			SqlHelperXml.SelectYieldCurveEntryDataHistoryCommand(out tmp, idYc, settlementDate);
			
			// remove this empty list of history (don't know how to sort them out on linq level)
			foreach (EntryPointHistory y in tmp)
				if (y.epValueHistory.Count != 0)
				{
					y.epValue = y.epValueHistory.LastOrDefault(i => i.Date <= settlementDate);
					outEntryDataHistoryList.Add(y);
				}

            return outEntryDataHistoryList;
        }

		#region ---------------------------All  Rates  --------------------------------------------
		/// <summary>
		/// Retrieves the list of all rates and their data (only for custom rates, for Quantlib rates -Class names)
		/// </summary>
		/// <param name="ctx"> 
		/// The connection context
		/// </param>
		/// </returns>

        internal static List<Rate> GetRates(long? idRate)
        {
            List<Rate> list = null;
			SqlHelperXml.SelectRatesCommand(out list, idRate);
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

        internal static List<Bond> GetBonds(long? idBond)
        {
			List<Bond> list = null;
			SqlHelperXml.SelectBondsCommand(out list, idBond);
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

        internal static List<Calendar> GetCalendars()
        {
			List<Calendar> list = null;
			SqlHelperXml.SelectCalendarsCommand(out list);
            return list;
        }

		internal static List<DayCounter> GetDayCounter(long? idDC, string className)
		{
			List<DayCounter> list = null;
			SqlHelperXml.SelectDayCounterCommand(out list, idDC, (idDC == null ? className : String.Empty));
			return list;
		}

		internal static List<ExchangeRate> GetXRates(long? idXRate)
        {
			List<ExchangeRate> list = null;
			SqlHelperXml.SelectXRatesCommand(out list, idXRate);
			return list;
        }

		#endregion ----------------------------------------------------------------------------------

		internal static List<EntryPointHistory> GetExchangeRateHistory(long? RateId,
                                                                                    DateTime? settlementDate)
        {
			List<EntryPointHistory> ExchangeRateHistoryList = null;

			SqlHelperXml.SelectExchangeRateHistoryCommand(out ExchangeRateHistoryList, RateId, settlementDate);
			ExchangeRateHistoryList.Sort(new EntryPointHistoryCompare());

			return ExchangeRateHistoryList;
        }

		#endregion --------------------------------------------------------------------
	}
}

#endif // _LINQXML_