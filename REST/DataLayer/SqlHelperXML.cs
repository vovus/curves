using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace DataLayer
{
    /// <summary>
    /// The Sql Helper.
    /// </summary>
    internal class SqlHelperXml
    {
        //static string path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data/xml_datastore/");
        static string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\qlc\\xmldb\\";

        static XDocument EnumCompoundingXml = XDocument.Load(path + "EnumCompounding.xml");
        static XDocument DayCounterXml = XDocument.Load(path + "DayCounter.xml");
        static XDocument EnumFrequencyXml = XDocument.Load(path + "EnumFrequency.xml");
        static XDocument EnumBusinessDayConventionXml = XDocument.Load(path + "EnumBusinessDayConvention.xml");
        static XDocument EnumDateGenerationXml = XDocument.Load(path + "EnumDateGeneration.xml");

        static XDocument CurrencyXml = XDocument.Load(path + "Currency.xml");
        static XDocument BondXml = XDocument.Load(path + "Bond.xml");
        static XDocument RateXml = XDocument.Load(path + "Rate.xml");
        static XDocument CalendarXml = XDocument.Load(path + "Calendar.xml");
		static XDocument ExchangeRateXml = XDocument.Load(path + "ExchangeRate.xml");
		static XDocument ExchangeRateHistoryXml = XDocument.Load(path + "ExchangeRateHistory.xml");

        static XDocument YieldCurveXml = XDocument.Load(path + "YieldCurve.xml");
        static XDocument YcFamilyXml = XDocument.Load(path + "YcFamily.xml");
        static XDocument YcEntryHistoryXml = XDocument.Load(path + "YcEntryHistory.xml");
        static XDocument YcEntryXml = XDocument.Load(path + "YcEntry.xml");

		// obj Factory
		//static Dictionary<string, XDocument> classFactoryDic; //XDocument.Load(path + "dayCounter.xml");

        #region Methods

		internal static void SelectDayCounterCommand(out List<DayCounter> res, long? idDC, string className)
		{
			IEnumerable<DayCounter> res0 =
			from c in DayCounterXml.Descendants("daycounter")
			where (idDC == null 
					? (String.IsNullOrEmpty(className) ? true : c.Element("ClassName").Value == className) 
					: c.Element("Id").Value == idDC.ToString())
					
			select new DayCounter
			{
				Id = Convert.ToInt32(c.Element("Id").Value),
				/*
				ClassName = c.Element("ClassName").Value + 
								(c.Element("ClassName").HasAttributes
									? "::" + c.Element("ClassName").Attribute("Param").Value 
									: String.Empty),
				*/
				ClassName = c.Element("ClassName").Value + 
								(String.IsNullOrEmpty(c.Element("ClassNameParam").Value) 
								? String.Empty 
								: "::" + c.Element("ClassNameParam").Value),
				Name = c.Element("Name").Value
			};

			res = res0.ToList();
		}

		/*
		public static void Init(string baseName, string className, string classNameParam, 
								string name /* user friendly name for GUI, default will be the same as className*)
        {
			string fileXml = baseName + ".xml";

			XDocument docXml = null;

			if (File.Exists(path + fileXml))
				docXml = XDocument.Load(path + fileXml);
			else
				docXml = new XDocument();

			if (!classFactoryDic.ContainsKey(baseName))
				classFactoryDic.Add(baseName, new XDocument());

			XElement dc = new XElement(baseName,
							new XElement("Id", classFactoryDic.Count),
							new XElement("Name", string.IsNullOrEmpty(name) ? className : name),
							new XElement("ClassName", className, new XAttribute("Param", classNameParam))
					  );

			classFactoryDic[baseName].Element("DataTable").Add(dc);

			classFactoryDic[baseName].Save(path + fileXml);
        }
		*/
        internal static void SelectCurrencyCommand(out List<Currency> res, long? idCurrency)
        {
            IEnumerable<Currency> res0 =
                
			from c in CurrencyXml.Descendants("currency")
            where idCurrency == null ? true : c.Element("Id").Value == idCurrency.ToString()
            select new Currency
            {
				Id = Convert.ToInt32(c.Element("Id").Value),
                ClassName = c.Element("ClassName").Value,
                Name = c.Element("Name").Value,
                Code = c.Element("Code").Value,
                NumericCode = String.IsNullOrEmpty(c.Element("NumericCode").Value) ? 0 : Convert.ToInt32(c.Element("NumericCode").Value),
                FractionsPerUnit = String.IsNullOrEmpty(c.Element("FractionsPerUnit").Value) ? 0 : Convert.ToInt32(c.Element("FractionsPerUnit").Value),
                FractionSymbol = c.Element("FractionSymbol").Value,
                Symbol = c.Element("Symbol").Value
            };

			res = res0.ToList();
        }

        internal static void SelectYieldCurveFamilyCommand(out List<YieldCurveFamily> res, long? idYcFamily, long? idCcy)
        {
            IEnumerable<YieldCurveFamily> res0 = 
				
			from ycf in YcFamilyXml.Descendants("ycfamily")
			where (idYcFamily == null ? true : ycf.Element("Id").Value == idYcFamily.ToString())
                       && (idCcy == null ? true : ycf.Element("CurrencyId").Value == idCcy.ToString())
            select new YieldCurveFamily
            {
				Id = Convert.ToInt32(ycf.Element("Id").Value),
                CurrencyId = Convert.ToInt32(ycf.Element("CurrencyId").Value),
                Name = ycf.Element("Name").Value
            };

			res = res0.ToList();
        }
		
		/* -- select all data points of Yield Curve which are the most resent in respect of specified date 
		  (settlement date or date of Yield Curve)
		  
		  this is wrong: select yce.Id, yce.YieldCurveId, yce.RateId, yce.Type, 
					yceh.Value, max(yceh.Date)
			from ycEntry yce, YcEntryHistory yceh 
			where yce.YieldCurveId = 10 and yce.id = yceh.YcEntryId and yceh.Date <= '2012-01-20'
			group by yce.Id
		  
		  OR 
		 
		  input parameters are:
		  123 - Yield Curve Id
		  '2011-03-15' - settlement date 
		 
		 SELECT 
			yc.idYc, yc.zcBasis, yc.zcCompounding, 
			yce.idycEntry, yce.type, yce.length, yce.timeunit, yce.idRate,
			yceh.date, yceh.value,
			rd.basis, rd.compounding, rd.fixingPlace, rd.settlementDays
			r.className
			FROM ycentryhistory yceh 
			JOIN ycentry yce ON yceh.idycentry = yce.idycEntry 
			JOIN yieldcurve yc ON yce.idYC = yc.idYC
			JOIN rate r ON yce.idRate = r.idRate
			LEFT JOIN ratedefinition rd ON yce.idRate = rd.idRate 
			WHERE 1 = 1
			AND yce.idYC = 123
			AND yceh.date = 
				(select max(date) from ycentryhistory yceh2
					where date <= '2011-03-15' and yceh2.idycEntry = yce.idycEntry)
		 */


		internal static void SelectYieldCurveDataCommand(out List<YieldCurveData> res,
														long? idYc)
		{

			IEnumerable<YieldCurveData> res0 =

			from yc in YieldCurveXml.Descendants("yieldcurve")

			join c0 in EnumCompoundingXml.Descendants("enumcompounding") on yc.Element("ZcCompoundingId").Value equals c0.Element("Id").Value
				into leftJointC0
			from lc0 in leftJointC0.DefaultIfEmpty()
			/*
			join b0 in DayCounterXml.Descendants("daycounter") on yc.Element("ZcBasisId").Value equals b0.Element("Id").Value
				into leftJointB0
			from lb0 in leftJointB0.DefaultIfEmpty()
			*/
			join f0 in EnumFrequencyXml.Descendants("enumfrequency") on yc.Element("ZcFrequencyId").Value equals f0.Element("Id").Value
				into leftJointF0
			from lf0 in leftJointF0.DefaultIfEmpty()

			join c1 in EnumCompoundingXml.Descendants("enumcompounding") on yc.Element("ForwCompoundingId").Value equals c1.Element("Id").Value
				into leftJointC1
			from lc1 in leftJointC1.DefaultIfEmpty()
			/*
			join b1 in DayCounterXml.Descendants("daycounter") on yc.Element("ForwBasisId").Value equals b1.Element("Id").Value
				into leftJointB1
			from lb1 in leftJointB1.DefaultIfEmpty()
			*/
			join f1 in EnumFrequencyXml.Descendants("enumfrequency") on yc.Element("ForwFrequencyId").Value equals f1.Element("Id").Value
				into leftJointF1
			from lf1 in leftJointF1.DefaultIfEmpty()

			join ycf in YcFamilyXml.Descendants("ycfamily") on yc.Element("FamilyId").Value equals ycf.Element("Id").Value
				into leftJointYCF
			from lYCF in leftJointYCF.DefaultIfEmpty()
			join ccy in CurrencyXml.Descendants("currency")
				on (lYCF == null ? "" : lYCF.Element("CurrencyId").Value) equals ccy.Element("Id").Value
				into leftJointCCY
			from lCCY in leftJointCCY.DefaultIfEmpty()

			where (idYc == null ? true : yc.Element("Id").Value == idYc.ToString())
			//		&& (idYcFamily == null ? true : yc.Element("FamilyId").Value == idYcFamily.ToString()))

			select new YieldCurveData
			{
				Id = Convert.ToInt32(yc.Element("Id").Value),
				
                settings = new YieldCurveSetting 
                {
                    ZcBasisId = String.IsNullOrEmpty(yc.Element("ZcBasisId").Value) ? -1 : Convert.ToInt32(yc.Element("ZcBasisId").Value),
                    ZcCompounding = (lc0 == null ? "" : lc0.Element("Name").Value),
                    ZcFrequency = (lf0 == null ? "" : lf0.Element("Name").Value),
                    ifZc = Convert.ToBoolean(yc.Element("ifZC").Value),
					ZCColor = yc.Element("ZcColor").Value,
				    FrwBasisId = String.IsNullOrEmpty(yc.Element("ForwBasisId").Value) ? -1 : Convert.ToInt32(yc.Element("ForwBasisId").Value),
				    FrwCompounding = (lc1 == null ? "" : lc1.Element("Name").Value),
	    			FrwFrequency = (lf1 == null ? "" : lf1.Element("Name").Value),
    				FrwTermBase = yc.Element("ForwTermBase").Value,
		    		FrwTerm = String.IsNullOrEmpty(yc.Element("ForwTerm").Value) ? -1 : Convert.ToInt32(yc.Element("ForwTerm").Value),
			    	ifFrw = Convert.ToBoolean(yc.Element("ifForw").Value),
					FrwColor = yc.Element("ForwColor").Value,

			    	SpreadType = String.IsNullOrEmpty(yc.Element("SpreadType").Value) ? -1 : Convert.ToInt32(yc.Element("SpreadType").Value),
				    SpreadSize = String.IsNullOrEmpty(yc.Element("SpreadSize").Value) ? -1 : Convert.ToInt32(yc.Element("SpreadSize").Value),
				    SpreadFamily = String.IsNullOrEmpty(yc.Element("SpreadFamily").Value) ? -1 : Convert.ToInt32(yc.Element("SpreadFamily").Value),
                },
				Name = yc.Element("Name").Value,
				Family = (lYCF == null ? "" : lYCF.Element("Name").Value),
				CurrencyId = (lCCY == null ? -1 : Convert.ToInt32(lCCY.Element("Id").Value)),

				entryPointList = null
				/*
				entryPoints = (idYcFamily == null) ? null : (

					from yce in YcEntryXml.Descendants("ycentry")
					where yce.Element("YieldCurveId").Value == yc.Element("Id").Value
					orderby (int)yce.Element("YieldCurveId") ascending

					select new EntryPoint
					{
						YieldCurveId = Convert.ToInt32(yce.Element("YieldCurveId").Value),
						Id = Convert.ToInt32(yce.Element("Id").Value),
						Type = yce.Element("Type").Value,
						Length = String.IsNullOrEmpty(yce.Element("Length").Value) ? 0 : Convert.ToInt32(yce.Element("Length").Value),
						TimeUnit = yce.Element("TimeUnit").Value,
						DataProviderId = Convert.ToInt32(yce.Element("DataProviderId").Value),
						DataReference = yce.Element("DataReference").Value,

						Instrument = (yce.Element("Type").Value == "bond"
								? (Instrument)GetBond(Convert.ToInt32(yce.Element("RateId").Value))
								: (Instrument)GetRate(Convert.ToInt32(yce.Element("RateId").Value))
							),

						EntryPoint = (

							from yceh in YcEntryHistoryXml.Descendants("ycentryhistory")
							where ((yceh.Element("YcEntryId").Value == yce.Element("Id").Value)
									&& (settlementDate == null
										? Convert.ToDateTime(yceh.Element("Date").Value) <= DateTime.Now
										: Convert.ToDateTime(yceh.Element("Date").Value) <= settlementDate.Value))
							orderby (DateTime)yceh.Element("Date") descending

							select new HistoricValue
							{
								Date = Convert.ToDateTime(yceh.Element("Date").Value),
								Value = Convert.ToDouble(yceh.Element("Value").Value)
							}
						).ToList().Max()
					}
				).ToList()
				*/
			};

			res = res0.ToList();
		}

		internal static void SelectYieldCurveEntryDataCommand(out List<EntryPoint> res, long? idYc, DateTime? settlementDate)
		{
			IEnumerable<EntryPoint> res0 =

			from yce in YcEntryXml.Descendants("ycentry")
			where ((idYc == null ? true : yce.Element("YieldCurveId").Value == idYc.ToString())
					&& (settlementDate == null 
						? true 
						: (Convert.ToDateTime(yce.Element("DateStart").Value) >= settlementDate 
								&& Convert.ToDateTime(yce.Element("DateFinish").Value) <= settlementDate)
						)
					)
			orderby (int)yce.Element("YieldCurveId") ascending

			select new EntryPoint
			{
				YieldCurveId = Convert.ToInt32(yce.Element("YieldCurveId").Value),
				Id = Convert.ToInt32(yce.Element("Id").Value),	// entry id, e.g. single point which will have several dates in ycentryhistory
				Type = yce.Element("Type").Value,
				Length = String.IsNullOrEmpty(yce.Element("Length").Value) ? 0 : Convert.ToInt32(yce.Element("Length").Value),
				TimeUnit = yce.Element("TimeUnit").Value,
				DataProviderId = String.IsNullOrEmpty(yce.Element("DataProviderId").Value) ? 0 : Convert.ToInt32(yce.Element("DataProviderId").Value),
				DataReference = yce.Element("DataReference").Value,
				Instrument = (0 == String.Compare(yce.Element("Type").Value, "bond", StringComparison.OrdinalIgnoreCase)
									? (Instrument)GetBond(Convert.ToInt32(yce.Element("RateId").Value))
									: (Instrument)GetRate(Convert.ToInt32(yce.Element("RateId").Value))
									),
				Enabled = true, //default
				ValidDateBegin = Convert.ToDateTime(yce.Element("DateStart").Value),
				ValidDateEnd = Convert.ToDateTime(yce.Element("DateFinish").Value),
				
				epValue = (
								from yceh in YcEntryHistoryXml.Descendants("ycentryhistory")
								where
								(
									(yceh.Element("YcEntryId").Value == yce.Element("Id").Value)
									&& (settlementDate == null
										? (0 == String.Compare(yce.Element("Type").Value, "bond", StringComparison.OrdinalIgnoreCase)
											? Convert.ToDateTime(yceh.Element("Date").Value) <= DateTime.Now
												&& GetBond(Convert.ToInt32(yce.Element("RateId").Value)).MaturityDate > DateTime.Now
											: Convert.ToDateTime(yceh.Element("Date").Value) <= DateTime.Now)
										: (0 == String.Compare(yce.Element("Type").Value, "bond", StringComparison.OrdinalIgnoreCase)
											? Convert.ToDateTime(yceh.Element("Date").Value) <= settlementDate.Value
												&& GetBond(Convert.ToInt32(yce.Element("RateId").Value)).MaturityDate > settlementDate.Value
											: Convert.ToDateTime(yceh.Element("Date").Value) <= settlementDate.Value)
										)
								)
								orderby (DateTime)yceh.Element("Date") descending

								select new HistoricValue
								{
									Date = Convert.ToDateTime(yceh.Element("Date").Value),
									Value = Convert.ToDouble(yceh.Element("Value").Value)
								}
							).ToList().Max()
			};

			res = res0.ToList();
			res.Sort(new EntryPointCompare());
		}
        
		internal static void SelectYieldCurveEntryDataHistoryCommand(out List<EntryPointHistory> res, long? idYc, DateTime? settlementDate)
		{
			IEnumerable<EntryPointHistory> res0 =

			from yce in YcEntryXml.Descendants("ycentry")
			where (idYc == null ? true : yce.Element("YieldCurveId").Value == idYc.ToString())
			orderby (int)yce.Element("YieldCurveId") ascending

			select new EntryPointHistory
			{
				YieldCurveId = Convert.ToInt32(yce.Element("YieldCurveId").Value),
				Id = Convert.ToInt32(yce.Element("Id").Value),	// entry id, e.g. single point which will have several dates in ycentryhistory
				Type = yce.Element("Type").Value,
				Length = String.IsNullOrEmpty(yce.Element("Length").Value) ? 0 : Convert.ToInt32(yce.Element("Length").Value),
				TimeUnit = yce.Element("TimeUnit").Value,
				DataProviderId = String.IsNullOrEmpty(yce.Element("DataProviderId").Value) ? 0 : Convert.ToInt32(yce.Element("DataProviderId").Value),
				DataReference = yce.Element("DataReference").Value,
				Instrument = (0 == String.Compare(yce.Element("Type").Value, "bond", StringComparison.OrdinalIgnoreCase)
									? (Instrument)GetBond(Convert.ToInt32(yce.Element("RateId").Value))
									: (Instrument)GetRate(Convert.ToInt32(yce.Element("RateId").Value))
									),

				epValueHistory = (
								from yceh in YcEntryHistoryXml.Descendants("ycentryhistory")
								where 
								(
									(yceh.Element("YcEntryId").Value == yce.Element("Id").Value)
									&& (settlementDate == null
										? (0 == String.Compare(yce.Element("Type").Value, "bond", StringComparison.OrdinalIgnoreCase)
											? Convert.ToDateTime(yceh.Element("Date").Value) <= DateTime.Now
												&& GetBond(Convert.ToInt32(yce.Element("RateId").Value)).MaturityDate > DateTime.Now
											: Convert.ToDateTime(yceh.Element("Date").Value) <= DateTime.Now)
										: (0 == String.Compare(yce.Element("Type").Value, "bond", StringComparison.OrdinalIgnoreCase)
											? Convert.ToDateTime(yceh.Element("Date").Value) <= settlementDate.Value
												&& GetBond(Convert.ToInt32(yce.Element("RateId").Value)).MaturityDate > settlementDate.Value
											: Convert.ToDateTime(yceh.Element("Date").Value) <= settlementDate.Value)
										)
								)
								orderby (DateTime)yceh.Element("Date") descending

								select new HistoricValue
								{
									Date = Convert.ToDateTime(yceh.Element("Date").Value),
									Value = Convert.ToDouble(yceh.Element("Value").Value)
								}
							).ToHashSet()
			};

			res = res0.ToList();
			res.Sort(new EntryPointHistoryCompare());
		}
        /*
        internal static void SelectYieldCurveEntryDataHistoryCommand(out List<EntryPointHistory> res, long? idYc, DateTime? settlementDate)
        {
            IEnumerable<EntryPointHistory> res0 =

            from yce in YcEntryXml.Descendants("ycentry")
			where (idYc == null ? true : yce.Element("YieldCurveId").Value == idYc.ToString())
			orderby (int)yce.Element("YieldCurveId") ascending

            join yceh in YcEntryHistoryXml.Descendants("ycentryhistory") 
            on yce.Element("Id").Value equals yceh.Element("YcEntryId").Value  
            into tmp
            //from i in tmp
			
            where (idYc == null ? true : yce.Element("YieldCurveId").Value == idYc.ToString())
            && (settlementDate == null
                        ? (yce.Element("Type").Value == "bond"
							? Convert.ToDateTime(yceh.Element("Date").Value) <= DateTime.Now
                                && GetBond(Convert.ToInt32(yce.Element("RateId").Value)).MaturityDate > DateTime.Now
							: Convert.ToDateTime(yceh.Element("Date").Value) <= DateTime.Now)
                        : (yce.Element("Type").Value == "bond"
							? Convert.ToDateTime(yceh.Element("Date").Value) <= settlementDate.Value
                                && GetBond(Convert.ToInt32(yce.Element("RateId").Value)).MaturityDate > settlementDate.Value
							: Convert.ToDateTime(yceh.Element("Date").Value) <= settlementDate.Value)
                          )
     
            select new EntryPointHistory
            {
                YieldCurveId = Convert.ToInt32(yce.Element("YieldCurveId").Value),
                Id = Convert.ToInt32(yce.Element("Id").Value),	// entry id, e.g. single point which will have several dates in ycentryhistory
                Type = yce.Element("Type").Value,
                Length = String.IsNullOrEmpty(yce.Element("Length").Value) ? 0 : Convert.ToInt32(yce.Element("Length").Value),
                TimeUnit = yce.Element("TimeUnit").Value,
                DataProviderId = String.IsNullOrEmpty(yce.Element("DataProviderId").Value) ? 0 : Convert.ToInt32(yce.Element("DataProviderId").Value),
                DataReference = yce.Element("DataReference").Value,
                Instrument = (yce.Element("Type").Value == "bond"
                                    ? (Instrument)GetBond(Convert.ToInt32(yce.Element("RateId").Value))
                                    : (Instrument)GetRate(Convert.ToInt32(yce.Element("RateId").Value))
                                    ),

                EntryDataHistory = ( from i in tmp
									 orderby (DateTime)i.Element("Date") descending
                    select new HistoricValue
                    {
                        Date = Convert.ToDateTime(i.Element("Date").Value),
                        Value = Convert.ToDouble(i.Element("Value").Value)
                    }).ToList()
            };

            res = res0.ToList();
            res.Sort(new EntryPointHistoryCompare());
        }
		*/
#region --------------------All rates command------------------

		internal static Rate GetRate(long? idRate)
		{
            if (idRate != null && Repository.RateDic.ContainsKey(idRate.Value))
                return Repository.RateDic[idRate.Value];

			List<Rate> res = null;
			SelectRatesCommand(out res, idRate);
			return res[0];
		}

		internal static void SelectRatesCommand(out List<Rate> res, long? idRate)
        {
			IEnumerable<Rate> res0 =

			from r in RateXml.Descendants("rate")
			// left join --start
			join c in EnumCompoundingXml.Descendants("enumcompounding") on r.Element("CompoundingId").Value equals c.Element("Id").Value
				into leftJointC
				from c in leftJointC.DefaultIfEmpty()
				/*
			join b in DayCounterXml.Descendants("daycounter") on r.Element("BasisId").Value equals b.Element("Id").Value
				into leftJointB
				from b in leftJointB.DefaultIfEmpty
				 */
			join f in EnumFrequencyXml.Descendants("enumfrequency") on r.Element("FrequencyId").Value equals f.Element("Id").Value
				into leftJointF
				from f in leftJointF.DefaultIfEmpty()
			join r1 in RateXml.Descendants("rate") on r.Element("IndexId").Value equals r1.Element("Id").Value
				into leftJointR1
				from lr1 in leftJointR1.DefaultIfEmpty()  
			// the same: compounding/basis/frequency, but for Rate1
			join c1 in EnumCompoundingXml.Descendants("enumcompounding") 
				on (lr1 == null ? "" : lr1.Element("CompoundingId").Value) equals c1.Element("Id").Value
				into leftJointC1
				from lc1 in leftJointC1.DefaultIfEmpty()
				/*
			join b1 in DayCounterXml.Descendants("daycounter") 
				on (lr1 == null ? "" : lr1.Element("BasisId").Value) equals b1.Element("Id").Value
				into leftJointB1
				from lb1 in leftJointB1.DefaultIfEmpty() 
				 */
			join f1 in EnumFrequencyXml.Descendants("enumfrequency") 
				on (lr1 == null ? "" : lr1.Element("FrequencyId").Value) equals f1.Element("Id").Value
				into leftJointF1
				from lf1 in leftJointF1.DefaultIfEmpty() 
				// left join --end
			where (idRate == null ? true : r.Element("Id").Value == idRate.ToString())
			select new Rate 
			{
				Id = Convert.ToInt32(r.Element("Id").Value),
				Name = r.Element("Name").Value,
				ClassName = r.Element("ClassName").Value,
				Type = r.Element("Type").Value,
				DataProviderId = String.IsNullOrEmpty(r.Element("DataProviderId").Value) ? 0 : Convert.ToInt32(r.Element("DataProviderId").Value),
				DataReference = r.Element("DataReference").Value,
				IdCcy = String.IsNullOrEmpty(r.Element("CurrencyId").Value) ? 0 : Convert.ToInt32(r.Element("CurrencyId").Value),
				Duration = String.IsNullOrEmpty(r.Element("Length").Value) ? 0 : Convert.ToInt32(r.Element("Length").Value),
				TimeUnit = r.Element("TimeUnit").Value,
				Accuracy = String.IsNullOrEmpty(r.Element("Accuracy").Value) ? 0 : Convert.ToInt32(r.Element("Accuracy").Value),
				Spread = String.IsNullOrEmpty(r.Element("Spread").Value) ? 0 : Convert.ToDouble(r.Element("Spread").Value),
				SettlementDays = String.IsNullOrEmpty(r.Element("SettlementDays").Value) ? 0 : Convert.ToInt32(r.Element("SettlementDays").Value),
				//YieldCurveId = String.IsNullOrEmpty(r.Element("YieldCurveId").Value) ? 0 : Convert.ToInt32(r.Element("YieldCurveId").Value),
				FixingPlace = r.Element("FixingPlace").Value, //??? temporary solution. fixing place in database is id whereas here we are looking for quantlibclassname of calendar
				//IdFixingPlace=reader.GetInt32(reader.GetOrdinal("FixingPlace")),
				IdIndex = String.IsNullOrEmpty(r.Element("IndexId").Value) ? 0 : Convert.ToInt32(r.Element("IndexId").Value),
				/*
				Basis = (b == null ? null : new DayCounter
				{
					Id = Convert.ToInt32(b.Element("Id").Value),
					Name = b.Element("Name").Value,
					ClassName = b.Element("ClassName").Value
				}),*/
				BasisId = (String.IsNullOrEmpty(r.Element("BasisId").Value) ? -1 : Convert.ToInt32(r.Element("BasisId").Value)),

				Frequency = (f == null ? "" : f.Element("Name").Value),
				Compounding = (c == null ? "" : c.Element("Name").Value),
					
				IndexDuration = (lr1 == null || String.IsNullOrEmpty(lr1.Element("Length").Value)) ? 0 : Convert.ToInt32(lr1.Element("Length").Value),
				IndexTimeUnit = (lr1 == null ? "" : lr1.Element("TimeUnit").Value),
                IndexName = (lr1 == null ? "" : lr1.Element("Name").Value),
                ClassNameIndex = (lr1 == null ? "" : lr1.Element("ClassName").Value),
				/*
				BasisIndex = (lb1 == null ? null : new DayCounter
				{
					Id = Convert.ToInt32(lb1.Element("Id").Value),
					Name = lb1.Element("Name").Value,
					ClassName = lb1.Element("ClassName").Value
				}),*/
				BasisIndexId = (lr1 == null || String.IsNullOrEmpty(lr1.Element("BasisId").Value)) ? -1 : Convert.ToInt32(lr1.Element("BasisId").Value),

                FrequencyIndex = (lf1 == null ? "" : lf1.Element("Name").Value),
                CompoundingIndex = (lc1 == null ? "" : lc1.Element("Name").Value)
			};

			res = res0.ToList();
        }
#endregion----------------------------------------------------


#region ------------------  All bonds command ------------------------

		internal static Bond GetBond(long? idBond)
		{
            if (idBond != null && Repository.BondDic.ContainsKey(idBond.Value))
                return Repository.BondDic[idBond.Value];

			List<Bond> res = null;
			SelectBondsCommand(out res, idBond);
			return res[0];
		}

        internal static void SelectBondsCommand(out List<Bond> res, long? idBond)
        {
			IEnumerable<Bond> res0 =

			from b in BondXml.Descendants("bond")
			//left join --begin
			join dg in EnumDateGenerationXml.Descendants("enumdategeneration") on b.Element("DateGenerationId").Value equals dg.Element("Id").Value
				into leftJoinDG
				from ldg in leftJoinDG.DefaultIfEmpty()
			join f in EnumFrequencyXml.Descendants("enumfrequency") on b.Element("CouponFrequencyId").Value equals f.Element("Id").Value
				into leftJoinF
				from lf in leftJoinF.DefaultIfEmpty()
			join bdc in EnumBusinessDayConventionXml.Descendants("enumbusinessdayconvention") on b.Element("CouponConventionId").Value equals bdc.Element("Id").Value
				into leftJoinBDC
				from lbdc in leftJoinBDC.DefaultIfEmpty()
				/*
			join cb in DayCounterXml.Descendants("daycounter") on b.Element("CouponBasisId").Value equals cb.Element("Id").Value
				into leftJoinCB
				from lcb in leftJoinCB.DefaultIfEmpty()
				 */
			//left join --end
			where idBond == null ? true : b.Element("Id").Value == idBond.ToString()
			select new Bond 
			{
				Id = Convert.ToInt32(b.Element("Id").Value),
				Name = b.Element("Name").Value,
				Type = b.Element("Type").Value,
				DataProviderId = String.IsNullOrEmpty(b.Element("DataProviderId").Value) ? 0 : Convert.ToInt32(b.Element("DataProviderId").Value),
				DataReference = b.Element("DataReference").Value,
				IdCcy = String.IsNullOrEmpty(b.Element("CurrencyId").Value) ? 0 : Convert.ToInt32(b.Element("CurrencyId").Value),
				Coupon = String.IsNullOrEmpty(b.Element("Coupon").Value) ? 0 : Convert.ToDouble(b.Element("Coupon").Value),
				Redemption = String.IsNullOrEmpty(b.Element("Redemption").Value) ? 0 : Convert.ToDouble(b.Element("Redemption").Value),
				Nominal = String.IsNullOrEmpty(b.Element("Nominal").Value) ? 0 : Convert.ToDouble(b.Element("Nominal").Value),
				SettlementDays = String.IsNullOrEmpty(b.Element("SettlementDays").Value) ? 0 : Convert.ToInt32(b.Element("SettlementDays").Value),
				//YieldCurveId = String.IsNullOrEmpty(b.Element("YieldCurveId").Value) ? 0 : Convert.ToInt32(b.Element("YieldCurveId").Value),
				IssueDate = Convert.ToDateTime(b.Element("IssueDate").Value),
				MaturityDate = Convert.ToDateTime(b.Element("MaturityDate").Value),

				CouponConvention = (lbdc == null ? "" : lbdc.Element("Name").Value),
				//CouponBasis = (lcb == null ? "" : lcb.Element("Name").Value),
				CouponBasisId = (String.IsNullOrEmpty(b.Element("CouponBasisId").Value) ? -1 : Convert.ToInt32(b.Element("CouponBasisId").Value)),
				DateGeneration = (ldg == null ? "" : ldg.Element("Name").Value),
				CouponFrequency = (lf == null ? "" : lf.Element("Name").Value)
			};

			res = res0.ToList();
        }
#endregion  ---------------------------------------------------------------------------


#region ------------------- All calendars command ---------------------
        internal static void SelectCalendarsCommand(out List<Calendar> res)
        {
			IEnumerable<Calendar> res0 =

			from c in CalendarXml.Descendants("calendar")
			where true
			select new Calendar
			{
				Id = Convert.ToInt32(c.Element("Id").Value),
				Name = c.Element("Name").Value,
				ClassName = c.Element("ClassName").Value,
				MarketName = c.Element("MarketName").Value
			};

			res = res0.ToList();
		}
#endregion

		internal static void SelectXRatesCommand(out List<ExchangeRate> res, long? idXRate)
        {
			IEnumerable<ExchangeRate> res0 =

			from xr in ExchangeRateXml.Descendants("exchangerate")
			where (idXRate == null ? true : xr.Element("Id").Value == idXRate.ToString())
			select new ExchangeRate 
			{ 
				Id = Convert.ToInt32(xr.Element("Id").Value),
				Name = xr.Element("Name").Value,
				ClassName = xr.Element("ClassName").Value,
				DataProviderId = String.IsNullOrEmpty(xr.Element("DataProviderId").Value) ? 0 : Convert.ToInt32(xr.Element("DataProviderId").Value), 
				DataReference = xr.Element("DataReference").Value, 
				SettlementDays = String.IsNullOrEmpty(xr.Element("SettlementDays").Value) ? 0 : Convert.ToInt32(xr.Element("SettlementDays").Value),
				FixingPlace = xr.Element("FixingPlace").Value,
				PrimaryCurrencyId = String.IsNullOrEmpty(xr.Element("PrimaryCurrencyCode").Value) ? 0 : Convert.ToInt32(xr.Element("PrimaryCurrencyCode").Value),
				SecondaryCurrencyId = String.IsNullOrEmpty(xr.Element("SecondaryCurrencyCode").Value) ? 0 : Convert.ToInt32(xr.Element("SecondaryCurrencyCode").Value)
			};

			res = res0.ToList();
		}

		internal static void SelectExchangeRateHistoryCommand(out List<EntryPointHistory> res,
																long? idXRate,
																DateTime? settlementDate)
		{
			IEnumerable<EntryPointHistory> res0 =

			from xr in ExchangeRateXml.Descendants("exchangerate")
			where (idXRate == null ? true : xr.Element("Id").Value == idXRate.ToString())
			orderby (int)xr.Element("Id") ascending

			select new EntryPointHistory
			{
				Id = Convert.ToInt32(xr.Element("Id").Value),
				DataProviderId = String.IsNullOrEmpty(xr.Element("DataProviderId").Value) ? 0 : Convert.ToInt32(xr.Element("DataProviderId").Value),
				DataReference = xr.Element("DataReference").Value,

				epValueHistory = (
								from xrh in ExchangeRateHistoryXml.Descendants("exchangeratehistory")
								where 
								( 
									(xrh.Element("RateId").Value == xr.Element("Id").Value)
										&& (settlementDate == null
											? true
											: Convert.ToDateTime(xrh.Element("Date").Value) <= settlementDate.Value
											)
								)
								orderby (DateTime)xrh.Element("Date") descending

								select new HistoricValue
								{
									Value = Convert.ToDouble(xrh.Element("Close").Value),
									Date = Convert.ToDateTime(xrh.Element("Date").Value)
								}
							).ToHashSet()
			};

			res = res0.ToList();
		}
#endregion
	}
}
