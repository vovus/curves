using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace YieldCurveSL
{
    using YieldCurveSrv;

    #region -------------------------------------------------enums---------------------------------------------

    public enum basis
    {
        Actual360,
        Actual365Fixed,
        ActualActual,
        Business252,
        Thirty360
    }

    public enum compounding
    {
        Simple,
        Compounded,
        Continuous
    }

    public enum frequency
    {
        Once,
        Annual,
        Semiannual,
        Quarterly,
        EveryFourthMonth,
        Bimonthly,
        Monthly,
        EveryFourthWeek,
        Biweekly,
        Weekly,
        Daily
    }

    public enum termbase
    {
        Days,
        Weeks,
        Months,
        Years
    }

    public enum ratetype
    {
        deposit,
        swap,
        bond
    }

    public enum bondtype
    {
        Fixed,
        Floating
    }

    public enum BusinessDayConvention
    {
        Following,
        ModifiedFollowing,
        Preceding,
        ModifiedPreceding,
        Unadjusted
    }

#endregion

    public static class EnumConversion
    {

#region getStringFromEnum
        // ------- enums for basis and compounding corresponding to quantlib classes

        public static String getStringFromEnum(basis b)
        {
            String Result = "";

            switch (b)
            {
                case basis.Actual360:
                    Result = "Actual/360";
                    break;
                case basis.Actual365Fixed:
                    Result = "Actual/365 (Fixed)";
                    break;
                case basis.ActualActual:
                    Result = "Actual/Actual (ISDA)";
                    break;
                case basis.Business252:
                    Result = "Business/252";
                    break;
                case basis.Thirty360:
                    Result = "30/360 (Bond Basis)";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Result;
        }

        public static String getStringFromEnum(frequency b)
        {
            String Result = "";

            switch (b)
            {
                case frequency.Once:
                    Result = "Once";
                    break;
                case frequency.Annual:
                    Result = "Annual";
                    break;
                case frequency.Semiannual:
                    Result = "Semiannual";
                    break;
                case frequency.EveryFourthMonth:
                    Result = "Every-Fourth-Month";
                    break;
                case frequency.Quarterly:
                    Result = "Quarterly";
                    break;
                case frequency.Bimonthly:
                    Result = "Bimonthly";
                    break;
                case frequency.Monthly:
                    Result = "Monthly";
                    break;
                case frequency.EveryFourthWeek:
                    Result = "Every-fourth-week";
                    break;
                case frequency.Biweekly:
                    Result = "Biweekly";
                    break;
                case frequency.Weekly:
                    Result = "Weekly";
                    break;
                case frequency.Daily:
                    Result = "Daily";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return Result;
        }

        public static String getStringFromEnum(BusinessDayConvention b)
        {
            String Result;

            switch (b)
            {
                case BusinessDayConvention.Following:
                    Result = "Following";
                    break;
                case BusinessDayConvention.ModifiedFollowing:
                    Result = "Modified Following";
                    break;
                case BusinessDayConvention.Preceding:
                    Result = "Preceding";
                    break;
                case BusinessDayConvention.ModifiedPreceding:
                    Result = "Modified Preceding";
                    break;
                case BusinessDayConvention.Unadjusted:
                    Result = "Unadjusted";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Result;
        }

        public static String getStringFromEnum(termbase b)
        {
            return b.ToString();
        }

        public static String getStringFromEnum(ratetype b)
        {
            return b.ToString();
        }

        public static String getStringFromEnum(bondtype b)
        {
            return b.ToString();
        }

        public static String getStringFromEnum(compounding b)
        {
            return b.ToString();
        }

#endregion getStringFromEnum

#region  getEnumFromString

        public static basis getBasisFromString(String b)
        {
            basis Result;

            switch (b)
            {
                case "Actual/360":
                    Result = basis.Actual360;
                    break;
                case "Actual/365 (Fixed)":
                    Result = basis.Actual365Fixed;
                    break;
                case "Actual/Actual (ISDA)":
                    Result = basis.ActualActual;
                    break;
                case "Business/252":
                    Result = basis.Business252;
                    break;
                case "30/360 (Bond Basis)":
                    Result = basis.Thirty360;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Result;
        }

        public static frequency getFrequencyFromString(String b)
        {
            frequency Result;

            switch (b)
            {
                case "Every-Fourth-Month":
                    Result = frequency.EveryFourthMonth;
                    break;
                case "Every-fourth-week":
                    Result = frequency.EveryFourthWeek;
                    break;
                case "Once":
                case "Annual":
                case "Semiannual":
                case "Quarterly":
                case "Bimonthly":
                case "Monthly":
                case "Biweekly":
                case "Weekly":
                case "Daily":
                    Result = (frequency)Enum.Parse(typeof(frequency), b, true);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return Result;
        }

        public static BusinessDayConvention getBusinessDayConventionFromString(String b)
        {
            BusinessDayConvention Result;

            switch (b)
            {
                case "Following":
                case "Preceding":
                case "Unadjusted":
                    Result = (BusinessDayConvention)Enum.Parse(typeof(BusinessDayConvention), b, true);
                    break;
                case "Modified Following":
                    Result = BusinessDayConvention.ModifiedFollowing;
                    break;
                case "Modified Preceding":
                    Result = BusinessDayConvention.ModifiedPreceding;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Result;
        }

        public static termbase getTermbaseFromString(String b)
        {
            termbase Result = (termbase)Enum.Parse(typeof(termbase), b, true);
            return Result;
        }

        public static ratetype getRateTypeFromString(String b)
        {
            ratetype Result = (ratetype)Enum.Parse(typeof(ratetype), b, true);
            return Result;
        }

        public static bondtype getBondTypeFromString(String b)
        {
            bondtype Result = (bondtype)Enum.Parse(typeof(bondtype), b, true);
            return Result;
        }

        public static compounding getCompoundingFromString(String b)
        {
            compounding Result = (compounding)Enum.Parse(typeof(compounding), b, true);
            return Result;
        }

#endregion  getEnumFromString

        public static frequency GetFrequencyFromMaturity(int l, termbase t)
        {
            frequency result = frequency.Annual;
            switch (t)
            {
                case termbase.Days:
                    result = frequency.Daily;
                    break;
                case termbase.Months:
                    switch (l)
                    {
                        case 1:
                            result = frequency.Monthly;
                            break;
                        case 2:
                            result = frequency.Bimonthly;
                            break;
                        case 3:
                            result = frequency.Quarterly;
                            break;
                        case 4:
                            result = frequency.EveryFourthMonth;
                            break;
                        case 6:
                            result = frequency.Semiannual;
                            break;
                        default:
                            result = frequency.Semiannual;
                            break;
                    }
                    break;
                case termbase.Weeks:
                    switch (l)
                    {
                        case 2:
                            result = frequency.Biweekly;
                            break;
                        case 4:
                            result = frequency.EveryFourthWeek;
                            break;
                        case 1:
                            result = frequency.Weekly;
                            break;
                        default:
                            result = frequency.Weekly;
                            break;
                    }
                    break;
                case termbase.Years:
                    result = frequency.Annual;
                    break;
                default:
                    result = frequency.Once;
                    break;
            }

            return result;
        }

        public static void InitializeComboFromEnum<T>(ComboBox c)
        {
            c.Items.Clear();

            Type enumType = typeof(T);
            foreach (FieldInfo com in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (enumType == typeof(basis))
                    c.Items.Add(EnumConversion.getStringFromEnum((basis)Enum.Parse(typeof(basis), com.Name, true)));
                else if (enumType == typeof(BusinessDayConvention))
                    c.Items.Add(EnumConversion.getStringFromEnum((BusinessDayConvention)Enum.Parse(typeof(BusinessDayConvention), com.Name, true)));
                else if (enumType == typeof(compounding))
                    c.Items.Add(EnumConversion.getStringFromEnum((compounding)Enum.Parse(typeof(compounding), com.Name, true)));
                else if (enumType == typeof(frequency))
                    c.Items.Add(EnumConversion.getStringFromEnum((frequency)Enum.Parse(typeof(frequency), com.Name, true)));
                else if (enumType == typeof(ratetype))
                    c.Items.Add(EnumConversion.getStringFromEnum((ratetype)Enum.Parse(typeof(ratetype), com.Name, true)));
                else if (enumType == typeof(bondtype))
                    c.Items.Add(EnumConversion.getStringFromEnum((bondtype)Enum.Parse(typeof(bondtype), com.Name, true)));
                else if (enumType == typeof(termbase))
                    c.Items.Add(EnumConversion.getStringFromEnum((termbase)Enum.Parse(typeof(termbase), com.Name, true)));

                // c.Items.Add(com.Name);
            }
        }

    }

    public static class CachedData
    {
		//
		// TEST
		//
		
		static public List<YieldCurveFamily> YcFamilyList = new List<YieldCurveFamily>();
		//
		//
		//

#region ------------------------------Currency1----------------------------------------------------------------
        
		public static Dictionary<long, YieldCurveSrv.Currency> CachedCurrencyDic = null; //new List<Currency>();

        public static void InitializeCurrencyComboFromCache(ComboBox c)
        {
			if (CachedCurrencyDic == null)
				return;
            c.Items.Clear();
            foreach (YieldCurveSrv.Currency cur in CachedCurrencyDic.Values.ToList())
                c.Items.Add(cur.Code);

        }
		/*
        public static void InitializeSettingsListFromCache()
        {
			if (CachedCurrencyList == null)
				return;

            YcSettingsList.SettList.Clear();

            foreach (Currency cur in CachedData.CachedCurrencyList)
            {
                foreach (YieldCurveFamily ycf in cur.YieldCurveFamilyList)
                {
                    foreach (YieldCurveData yc in ycf.YieldCurveList)
                    {
						YcSettingsList.SettList.Add(new Setting()
						{
							YCName = yc.Name,
							YCId = yc.Id,
							ZCbasis = EnumConversion.getBasisFromString(yc.ZcBasis),
							ZCcompounding = EnumConversion.getCompoundingFromString(yc.ZcCompounding),
							ZCfrequency = EnumConversion.getFrequencyFromString(yc.ZcFrequency),
							ifZCCurve = yc.ifZc,
							Forwbasis = EnumConversion.getBasisFromString(yc.FrwBasis),
							Forwcompounding = EnumConversion.getCompoundingFromString(yc.FrwCompounding),
							Forwfrequency = EnumConversion.getFrequencyFromString(yc.FrwFrequency),
							ifForwardCurve = yc.ifFrw
						});
                    }
                }
            }
        }
		*/

		public static int GetCurrencyIDbyName(String Name)
		{
			foreach (var i in CachedData.CachedCurrencyDic)
			{
				if (i.Value.Code == Name)
					return i.Value.Id;
			}

			MessageBox.Show("no such currency exists");
			return -100;
		}

        public static void InitializeYieldCurveComboFromCache(ComboBox c)
        {
            foreach (YcSettings ycd in YcSettingsDic.ycdDic.Values.ToList())
                c.Items.Add(ycd.ycd.Name);

			//c.DataContext = YcSettingsDic.ycdDic.Values.ToList();
			/*
            c.Items.Clear();

            foreach (Currency cur in CachedData.CachedCurrency)
              foreach (YieldCurveFamily ycf in cur.YieldCurveFamilyList)
                foreach (YieldCurve yc in ycf.YieldCurveList)
                    c.Items.Add(yc.Name);
			 */
         }

        public  static int GetYieldCurveIDbyName(String Name)
        {
			/*
            int res=-100;
			
            foreach (Currency cur in CachedData.CachedCurrency)
                foreach (YieldCurveFamily ycf in cur.YieldCurveFamilyList)
                    foreach (YieldCurve yc in ycf.YieldCurveList)
                    {
                        if (yc.Name == Name)
                            res = yc.Id;
                    }
			*/
			foreach (var i in YcSettingsDic.ycdDic)
			{
				if (i.Value.ycd.Name == Name)
					return i.Value.ycd.Id;
			}
           
			MessageBox.Show("no such yc exists");
            return -100;
        }

		//for a moment it wil return the first yc id in the list with specified currency
		public static int GetDefaultYieldCurveIDbyCurrencyName(String Name)
		{
			int CurId = GetCurrencyIDbyName(Name);

			foreach (var i in YcSettingsDic.ycdDic)
			{
				if (i.Value.ycd.CurrencyId == CurId)
					return i.Value.ycd.Id;
			}

			MessageBox.Show("no such yc exists");
			return -100;
		}

#endregion ------------------------------------------------------------------------------------------------

#region ------------------------------rates----------------------------------------------------------------
        
		public static Dictionary<long, Rate> CachedRatesDic = null;// new List<Rate>();

        public static void InitializeRateComboFromCache(ComboBox c)
        {
			if (CachedRatesDic == null)
				return;

			c.DataContext = CachedData.CachedRatesDic.Values.ToList();

			/*
            c.Items.Clear();

            for (int i = 0; i < CachedData.CachedRatesList.Count; i++) // Loop through List with for
            {
                c.Items.Add(CachedData.CachedRatesList[i].Name);
            }
			*/
        }
		/*
        public static void InitializeRateComboFromCache(ComboBox c, String cur)
        {
            c.Items.Clear();

            for (int i = 0; i < CachedData.CachedRatesList.Count; i++) // Loop through List with for
            {
                if (CachedData.CachedRatesList[i].CcyName == cur)
                    c.Items.Add(CachedData.CachedRatesList[i].Name);
            }
        }
		*/
#endregion ------------------------------------------------------------------------------------------------

#region ------------------------------bonds----------------------------------------------------------------
        
		public static Dictionary<long, Bond> CachedBondsDic = null;// new List<Bond>();
		/*
        public static void InitializeBondComboFromCache(ComboBox c, String cur )
        {
            c.Items.Clear();

            if (cur != null)
            {
                for (int i = 0; i < CachedData.CachedBondsList.Count; i++) // Loop through List with for
                {
                    if (CachedData.CachedBondsList[i].CcyName == cur)
                        c.Items.Add(CachedData.CachedBondsList[i].Name);
                }
            }
            else if (cur == null)
            {
                for (int i = 0; i < CachedData.CachedBondsList.Count; i++) // Loop through List with for
                {
                    c.Items.Add(CachedData.CachedBondsList[i].Name);
                }
            }

        }
		*/
#endregion ------------------------------------------------------------------------------------------------

#region ------------------------------Exchange rates----------------------------------------------------------------

        public static Dictionary<long, ExchangeRate> CachedExchangeRatesDic = null;// new List<Rate>();
        public static int GetExchangeRateIdbyPair(String code1, String code2)
        {
            string name = code1 + code2;
            int code=0;
            foreach (ExchangeRate er in CachedExchangeRatesDic.Values.ToList())
                   if (er.Name==name)
                       code=er.Id;
            return code;
        }


		public static List<EntryPointHistory> CachedExchangeRateHistory = new List<EntryPointHistory>();
        public static double GetExchangeRateValue (String code1, String code2, DateTime dt)
        {
            bool reverse = false;
            if (code1 == code2)
                return 1;

            int code = GetExchangeRateIdbyPair(code1, code2);
            if (code == 0)
            {
                reverse = true;
                code = GetExchangeRateIdbyPair(code2, code1);
            }

            if (code == 0)
                return 0;

            double val = 0;
			foreach (EntryPointHistory y in CachedExchangeRateHistory)
            {
                if (y.Id != code)
                    continue;

                	// here EntryDataHistory assumed be sorted by Date descending (latest at the begining)
                for (int i = 0; i < y.epValueHistory.Count; i++)
                {
                    if (y.epValueHistory[i].Date <= dt)
                    {
						val = (reverse == false) ? y.epValueHistory[i].Value : Math.Round(1 / y.epValueHistory[i].Value, 4);
						break;
                    }
                }
            }
            return val;
        }
       

#endregion ------------------------------------------------------------------------------------------------


#region ----------Entry points for yield curves with dates----------------------------------------------------------------

        //for a moment the idea is only to cache the parametrization for last available date
        // if in the future we decided to cache parametrization of YC in the past (ie audited parametrization we will need to differentiate
        //between YC definitions in terms of entry rates and last prices of entry rates = two different lists
        //public static List<YieldCurveEntryData> CachedEntryData = new List<YieldCurveEntryData>();
        static public Dictionary<long, YieldCurveData> YieldCurveDataDic = new Dictionary<long, YieldCurveData>();
		public static List<EntryPointHistory> CachedEntryPointHistoryList = new List<EntryPointHistory>();
       
#endregion ------------------------------------------------------------------------------------------------

#region ----------Common schedule----------------------------------------------------------------

        //this is the list of all dates for all curves. It is needed to compare curves and to build forward forex
#if __VC10__
        public static HashSet<DateTime> CommonDates = new HashSet<DateTime>();
        public static HashSet<DateTime> GetCommonCalendarFromCachedCurves(int CurveId1, int CurveId2)
        {
            HashSet<DateTime> Durations = new HashSet<DateTime>();
#else
		public static Dictionary<DateTime, bool> CommonDates = new Dictionary<DateTime, bool>();
		public static Dictionary<DateTime, bool> GetCommonCalendarFromCachedCurves(int CurveId1, int CurveId2)
		{
			Dictionary<DateTime, bool> Durations = new Dictionary<DateTime, bool>();
#endif
			foreach (EntryPointHistory ycedh in CachedData.CachedEntryPointHistoryList)
                if  ((ycedh.YieldCurveId == CurveId1) || (ycedh.YieldCurveId == CurveId2))
#if __VC10__
                    Durations.Add(CurrentElements.CurrentDate.AddDays(ycedh.Duration));
#else
					Durations[CurrentElements.CurrentDate.AddDays(ycedh.Duration)] = true;
#endif
            //Durations.Sort();
           
            //foreach (long dur in Durations.Distinct())
            //    result.Add(CurrentElements.CurrentDate.AddDays(dur));

            return Durations;
        }
       

#endregion ------------------------------------------------------------------------------------------------


#region ----------zc and forward rates ----------------------------------------------------------------

        public static Dictionary<long, YieldCurveComputed> CachedYieldCurvesDic = new Dictionary<long, YieldCurveComputed>();

#endregion ------------------------------------------------------------------------------------------------

		public static Dictionary<long, DayCounter> CachedDayCounterDic = new Dictionary<long, DayCounter>();

		#region INFLATION CURVE

		public static Dictionary<long, InflationBond> CachedInflationBondsDic = null;
		public static Dictionary<long, InflationIndex> CachedInflationIndexDic = null;
		public static Dictionary<long, InflationRate> CachedInflationRateDic = null;
		public static List<YieldCurveFamily> CachedInflationCurveFamily = null;
		public static Dictionary<long, InflationCurveData> CachedInflationCurveDataDic = null;
		public static Dictionary<long, InflationCurveEntryData> CachedInflationCurveEntryDataDic = null;
		public static Dictionary<long, RateHistory> CachedInflationBondsHistoryDic = null; //id of the ratehistory point not the bond
		public static Dictionary<long, RateHistory> CachedInflationIndexHistoryDic = null;
		public static Dictionary<long, RateHistory> CachedInflationRateHistoryDic = null;

		//this will return the history for a specific bond by Id
		// only for dates prior to dt
		public static List<RateHistory> getBondsHistoryById(int ibId, DateTime? dt)
		{
			List<RateHistory> result = new List<RateHistory>();
			foreach (RateHistory rh in CachedInflationBondsHistoryDic.Values.ToList())
			{
				if (rh.RateId == ibId)
				{
					if (dt != null)
					{
						if (rh.Date <= dt)
							result.Add(rh);
					}
					else
						result.Add(rh);
				}
				result.Sort((x, y) => y.Date.CompareTo(x.Date));
			}
			return result;
		}

		public static List<RateHistory> getRateHistoryById(int ibId, DateTime? dt)
		{
			List<RateHistory> result = new List<RateHistory>();
			foreach (RateHistory rh in CachedInflationRateHistoryDic.Values.ToList())
			{
				if (rh.RateId == ibId)
				{
					if (dt != null)
					{
						if (rh.Date <= dt)
							result.Add(rh);
					}
					else
						result.Add(rh);
				}
				result.Sort((x, y) => y.Date.CompareTo(x.Date));
			}
			return result;
		}

		public static List<RateHistory> getIndexHistoryById(int ibId, DateTime? dt)
		{
			List<RateHistory> result = new List<RateHistory>();
			foreach (RateHistory rh in CachedInflationIndexHistoryDic.Values.ToList())
			{
				if (rh.RateId == ibId)
				{
					if (dt != null)
					{
						if (rh.Date <= dt)
							result.Add(rh);
					}
					else
						result.Add(rh);
				}
				result.Sort((x, y) => y.Date.CompareTo(x.Date));

			}
			return result;
		}

		public static InflationCurveSnapshot GetInflationCurveSnapshot(int InflationCurveID, DateTime d)
		{
			InflationCurveSnapshot ics = new InflationCurveSnapshot();
			ics.Id = InflationCurveID;
			ics.settlementDate = d;
			ics.Name = CachedInflationCurveDataDic[InflationCurveID].Name;
			ics.Family = CachedInflationCurveDataDic[InflationCurveID].Family;
			ics.CurrencyId = CachedInflationCurveDataDic[InflationCurveID].CurrencyId;
			ics.InflationIndexId = CachedInflationCurveDataDic[InflationCurveID].InflationIndexId;
			ics.InflationIndex = CachedInflationIndexDic[ics.InflationIndexId];
			ics.settings = CachedInflationCurveDataDic[InflationCurveID].settings;

			List<EntryValuePair> ListEVP = new List<EntryValuePair>();

			foreach (InflationCurveEntryData iced in CachedInflationCurveEntryDataDic.Values.ToList())
			{
				EntryValuePair evp = new EntryValuePair(); //to be added to ListEVP

				if ((iced.InflationCurveId == InflationCurveID)
						&& (iced.ValidDateBegin <= d)
						&& (iced.ValidDateEnd >= d)
					)
				{
					//  ics.EntryList.Add(iced);
					//now for that entry we need to find the most recent historical value

					evp.iced = iced;

					if (iced.Type == "inflationbond")
					{
						List<RateHistory> bh = getBondsHistoryById(iced.Instrument.Id, d);
						RateHistory rh = bh[bh.Count() - 1];
						if (rh.RateId != iced.Instrument.Id)
							MessageBox.Show("something is wrong in GetInflationCurveSnapshot");
						else
						{
							evp.value = bh[bh.Count() - 1].Close;
							//  ics.ValueList.Add(bh[bh.Count() - 1].Close);
						}
					}
					else if (iced.Type == "swap")
					{
						List<RateHistory> bh = getRateHistoryById(iced.Instrument.Id, d);
						RateHistory rh = bh[bh.Count() - 1];
						if (rh.RateId != iced.Instrument.Id)
							MessageBox.Show("something is wrong in GetInflationCurveSnapshot");
						else
						{
							//  ics.ValueList.Add(bh[bh.Count() - 1].Close);
							evp.value = bh[bh.Count() - 1].Close;
						}
					}
					ListEVP.Add(evp);
				}
			}
			//   ics.EntryList.Sort((x, y) => y.Date.CompareTo(x.Date));

			ListEVP.Sort();
			ics.EntryList = new ObservableCollection<InflationCurveEntryData>();
			ics.ValueList = new ObservableCollection<double>();
			foreach (EntryValuePair evp in ListEVP)
			{
				ics.EntryList.Add(evp.iced);
				ics.ValueList.Add(evp.value);
			}

			List<RateHistory> iihistory = getIndexHistoryById(ics.InflationIndexId, d);
			ics.IndexHistory = new ObservableCollection<HistoricValue>();
			foreach (RateHistory iirh in iihistory)
			{
				HistoricValue vp = new HistoricValue();
				vp.Date = iirh.Date;
				vp.Value = iirh.Close;
				ics.IndexHistory.Add(vp);
			}

			return ics;
		}

		static public Dictionary<long, InflationCurveSnapshot> InflationCurveSnapshotDic = new Dictionary<long, InflationCurveSnapshot>();
		public static Dictionary<long, YieldCurveComputed> CachedInflationCurvesDic = new Dictionary<long, YieldCurveComputed>();

		//just in order to sort entry data by duration
		// more general implementatiom in needed
		public class EntryValuePair : IComparable<EntryValuePair>
		{
			public InflationCurveEntryData iced { get; set; }
			public double value { get; set; }

			int IComparable<EntryValuePair>.CompareTo(EntryValuePair other)
			{
				if (other.iced.Duration > iced.Duration) // this date is less the other
					return -1;
				else if (iced.Duration == other.iced.Duration)
					return 0;
				else
					return 1;
			}
		}

		#endregion
    }

    // this will be used instead of Settings to have settings per currency
	
    class YcSettings
    {
		public bool ifZCCurve = true;
		public bool ifForwardCurve = false;
		public Color ZCColor = Colors.Black;
		public Color FrwColor = Colors.Red;

		public YieldCurveData ycd = null;

	/*	public YcSettings(YieldCurveData y) 
		{
			ifZCCurve = true;
			ifForwardCurve = true;
			ZCColor = Colors.Black;
			FrwColor = Colors.Red;

			ycd = y;
			//
			// overwrite stings like "Actual365 (Fixed)" to "Actual365Fixed" to be consistent with ObjectFactory
			//
			//ycd.ZcBasis = y.ZcBasis;
			ycd.ZcBasisId = y.ZcBasisId;
			ycd.ZcCompounding = EnumConversion.getCompoundingFromString(y.ZcCompounding).ToString();
			ycd.ZcFrequency = EnumConversion.getFrequencyFromString(y.ZcFrequency).ToString();

			//ycd.FrwBasis = y.FrwBasis;
			ycd.FrwBasisId = y.FrwBasisId;
			ycd.FrwCompounding = EnumConversion.getCompoundingFromString(y.FrwCompounding).ToString();
			ycd.FrwFrequency = EnumConversion.getFrequencyFromString(y.FrwFrequency).ToString();
			ycd.FrwTermBase = EnumConversion.getTermbaseFromString(y.FrwTermBase).ToString();
		}
        */
        public YcSettings(YieldCurveData y)
        {
            //??? I am re implementing here clone method for YieldCurveData although it needs to be implemented in Data layer
            //??? the only reason is because for a moment I dont know how to expose it from there
            //??? to be fixed
            ifZCCurve = true;
            ifForwardCurve = true;
            ZCColor = ColorFromString.ToColor(y.settings.ZCColor); //Colors.Black;  //not relevant anymore
            FrwColor = ColorFromString.ToColor(y.settings.FrwColor);  //Colors.Red; //not relevant anymore

            ycd = new YieldCurveData();
            ycd.CurrencyId = y.CurrencyId;
            ycd.Family = y.Family;
            ycd.Id = y.Id;
            ycd.Name = y.Name;
            ycd.settlementDate = y.settlementDate;
            ycd.settings = new YieldCurveSetting();
            //??? here I will use only a soft copy which is not important for current intended usage
            ycd.entryPointList = y.entryPointList;

            //???cloning settings - just repeating wht has already been done in Data layer
            ycd.settings.ZcBasisId = y.settings.ZcBasisId;
            ycd.settings.ZcCompounding = y.settings.ZcCompounding;
            ycd.settings.ZcFrequency = y.settings.ZcFrequency;
            ycd.settings.ifZc = y.settings.ifZc;
            ycd.settings.ZCColor = y.settings.ZCColor;
            ycd.settings.FrwBasisId = y.settings.FrwBasisId;
            ycd.settings.FrwCompounding = y.settings.FrwCompounding;
            ycd.settings.FrwFrequency = y.settings.FrwFrequency;
            ycd.settings.ifFrw = y.settings.ifFrw;
            ycd.settings.FrwColor = y.settings.FrwColor;
            ycd.settings.FrwTerm = y.settings.FrwTerm;
            ycd.settings.FrwTermBase = y.settings.FrwTermBase;
            ycd.settings.SpreadType = y.settings.SpreadType;
            ycd.settings.SpreadSize = y.settings.SpreadSize;
            ycd.settings.SpreadFamily = y.settings.SpreadFamily;
            ycd.settings.Calendar = y.settings.Calendar;


          
          //  ycd.settings=y.settings.
            //
            // overwrite stings like "Actual365 (Fixed)" to "Actual365Fixed" to be consistent with ObjectFactory
            //
            //ycd.ZcBasis = y.ZcBasis;
            /*
            ycd.settings.ZcBasisId = y.settings.ZcBasisId;
            ycd.settings.ZcCompounding = EnumConversion.getCompoundingFromString(y.settings.ZcCompounding).ToString();
            ycd.settings.ZcFrequency = EnumConversion.getFrequencyFromString(y.settings.ZcFrequency).ToString();

            //ycd.FrwBasis = y.FrwBasis;
            ycd.settings.FrwBasisId = y.settings.FrwBasisId;
            ycd.settings.FrwCompounding = EnumConversion.getCompoundingFromString(y.settings.FrwCompounding).ToString();
            ycd.settings.FrwFrequency = EnumConversion.getFrequencyFromString(y.settings.FrwFrequency).ToString();
            ycd.settings.FrwTermBase = EnumConversion.getTermbaseFromString(y.settings.FrwTermBase).ToString();  */
        }

		public YcSettings()
		{
			ifZCCurve = true;
			ifForwardCurve = false;
			ZCColor = Colors.Black;
			FrwColor = Colors.Red;

			ycd = new YieldCurveData	// the Default setting for YielCurve
			{
				Id = -1,

				Name = String.Empty,
				Family = String.Empty,
				CurrencyId = -1,				// TODO: what is default ? check other "defautls" as well 

                /*  to be deleted
				//ZcBasis = new DayCounter { Id = -1, ClassName = "Actual365Fixed", Name = "Actual/365 (Fixed)" },
				ZcBasisId = -1,
				ZcCompounding = compounding.Continuous.ToString(),
				ZcFrequency = frequency.Once.ToString(),
				ifZc = true,

				//FrwBasis = new DayCounter { Id = -1, ClassName = "Actual365Fixed", Name = "Actual/365 (Fixed)" },
				FrwBasisId = -1,
				FrwCompounding = compounding.Continuous.ToString(),
				FrwFrequency = frequency.Once.ToString(),
				ifFrw = false,

				FrwTerm = 1,
				FrwTermBase = termbase.Years.ToString(),

				SpreadType = 0,
				SpreadSize = 0.0,
				SpreadFamily = 0  */
			};

            ycd.settings=new YieldCurveSetting();
            ycd.settings.ZcCompounding = compounding.Continuous.ToString();
            ycd.settings.ZcFrequency = frequency.Once.ToString();
            ycd.settings.ifZc = true;
            ycd.settings.FrwBasisId = -1;
            ycd.settings.FrwCompounding = compounding.Continuous.ToString();
            ycd.settings.FrwFrequency = frequency.Once.ToString();
            ycd.settings.ifFrw = false;
            ycd.settings.FrwTerm = 1;
            ycd.settings.FrwTermBase = termbase.Years.ToString();
            ycd.settings.SpreadType = 0;
            ycd.settings.SpreadSize = 0.0;
            ycd.settings.SpreadFamily = 0;
            ycd.settings.ZCColor = Colors.Blue.ToString();
            ycd.settings.FrwColor = Colors.Red.ToString();
		}
        
        public YieldCurveData getYcSettings()
        {
			return ycd;
        }
    }

    // this class contains settings for all ycurves
    static class YcSettingsDic
    {
        public static int CurrentYCId = -1;
		
		public static Dictionary<long, YcSettings> ycdDic = new Dictionary<long, YcSettings>();

		public static YcSettings GetYcSett(int ycId)
        {
			try
			{
				return ycdDic[ycId];
			}
			catch (KeyNotFoundException)
			{
                // TODO: OR return NULL instead ?
				ycdDic[ycId] = new YcSettings();    // create new one with default value
                ycdDic[ycId].ycd.Id = ycId;         // set Id instead of default -1
				return ycdDic[ycId];            // put it into cache
			}
        }

        public static void SetYcSett(YieldCurveData ycd)
        {
			ycdDic[ycd.Id] = new YcSettings(ycd);
        }

        public static List<String> NotEmptyCurrency()
        {
            List<String> res=new List<String>();

            foreach (YcSettings ycf in ycdDic.Values.ToList())
            {
                String CurCode = (CachedData.CachedCurrencyDic.ContainsKey(ycf.ycd.CurrencyId) 
									? CachedData.CachedCurrencyDic[ycf.ycd.CurrencyId].Code
									: "");
                if (!res.Contains(CurCode))
                 res.Add(CurCode);
                
            }


            return res;

        }
    }
    
    static class AppSettings
    {
        // it returns the string which shows the format - like "#0.0000" for 4 decimals        
        public static String DecimalsString()
        {
            String result = "#0.";
            String a = "0";
            for (int i = 0; i < Decimals; i++)
            {
                result = result + a;
            }
            return result;
        }

       public static int Decimals = 6;
       public static Color FirstCurveColor = Colors.Blue;
       public static Color SecondCurveColor = Colors.Red;

       public static int ActiveWindow = 1; //1 if it is one curve mode and 2 if it is 2 curve mode
    }

    public static class CurrentElements
    {
        public static class FXElement
        {
            public static int RowId; // row number of the fx elementy in the datagrid
            public static int ColId; // column number of the fx element in teh datagrid
            public static String FirstCurrency;
            public static String SecondCurrency;
            public static Button btn; //button which was clicked for fx datagrid
        }

        public static int CurrentYCId = -1;
        public static DateTime CurrentDate;
		// public static FXElement fxButton;

		public static int CurrentICId = -1;
        public static List<DateTime> InflationDates;
        public static InflationCurveSnapshot CurrentInflationCurveSnapshot = new InflationCurveSnapshot();
	}
}