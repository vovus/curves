using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using DataLayer;
using DataFeed;
using FinancialLayer;
using System.Web;

namespace YieldCurveWcf
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    public class YieldCurveSrv : IYieldCurveSrv
    {
		public ResponseInit Init(DateTime settlementDate)
		{
			ResponseInit result = new ResponseInit();
			try
			{
				// needs quantlib class factory
				result.dc = GetDayCounterDic();			// shell be before Rate and YieldCurveData, cos those are using DayCounter inside
				result.ccy = GetCurrencyDataDic();
				result.r = GetRateDataDic();
				result.b = GetBondDataDic();

				// can be in local data storage
				result.yc = GetYieldCurveDataDic();
				result.ycf = GetYcFamilyDataList();
				result.xr = GetExchangeRateDataDic();
				result.edh = GetEntryPointHistoryList(settlementDate, null);		// do it after rate/bond cos it uses Rate / Bond information (from QuantLib) inside
				result.xrh = GetExchangeRateHistoryList(settlementDate, null);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

            if ((result.dc.Error != null) ||
                (result.yc.Error != null) ||
                (result.ycf.Error != null) ||
                (result.ccy.Error != null) ||
                (result.r.Error != null) ||
                (result.b.Error != null) ||
                (result.xr.Error != null) ||
                (result.edh.Error != null) ||
                (result.xrh.Error != null))
            {
                result.Error = new CustomException();
                if (result.dc.Error != null) 
                    result.Error.Message = result.dc.Error.Message;
                if (result.yc.Error != null)
                     result.Error.Message =result.Error.Message+result.yc.Error.Message;
                if (result.ycf.Error != null) 
                     result.Error.Message =result.Error.Message+result.ycf.Error.Message;
                if (result.ccy.Error != null) 
                    result.Error.Message =result.Error.Message+result.ccy.Error.Message;
                if (result.r.Error != null) 
                    result.Error.Message =result.Error.Message+result.r.Error.Message;
                if (result.b.Error != null) 
                    result.Error.Message =result.Error.Message+result.b.Error.Message;
                if (result.xr.Error != null)
                    result.Error.Message =result.Error.Message+result.xr.Error.Message;
                if (result.edh.Error != null)
                    result.Error.Message =result.Error.Message+result.edh.Error.Message;
                if (result.xrh.Error != null)
                    result.Error.Message = result.Error.Message + result.xrh.Error.Message;
               
            }
            return result;
		}

		public ResponseYcFamilyData GetYcFamilyDataList()
		{
			ResponseYcFamilyData result = new ResponseYcFamilyData();
			try 
			{
				result.YcFamilyList = DataLayer.Repository.GetYieldCurveFamily(null, null);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseYieldCurveData GetYieldCurveDataDic()
		{
			ResponseYieldCurveData result = new ResponseYieldCurveData();
			try 
			{
				var ycs = DataLayer.Repository.GetYieldCurveData(null);
				foreach (YieldCurveData ycd in ycs)
				{
					result.YieldCurveDataDic[ycd.Id] = ycd;
				}
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseCurrencyData GetCurrencyDataDic()
		{
			ResponseCurrencyData result = new ResponseCurrencyData();

			try
			{
				var currencies = DataLayer.Repository.GetCurrency(null);

				foreach (var c in currencies)
				{
					if (c != null
						&& !String.IsNullOrEmpty(c.ClassName)
						&& QuantLibAdaptor.GetCurrencyInformation(c)
							)
						result.CurrencyDic[c.Id] = c;
				}
				DataLayer.Repository.CurrencyDic = result.CurrencyDic;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			//we read from the database from te table Currencies
			// assumption is - if ClassName is not null then the rest of the parameters for each currency should be populated for Quantlib
			// otherwise it was manually added and everything should be coming from the database only

			return result;
		}

        public ResponseRateData GetRateDataDic()
        {
            ResponseRateData result = new ResponseRateData();
            
            try
            {
				var rates = DataLayer.Repository.GetRates();

                foreach (var r in rates)
                {
					if (r != null
						&& !String.IsNullOrEmpty(r.ClassName)
						&& QuantLibAdaptor.GetRateInformation(r)
							)
					{
						result.RateDic[r.Id] = r;
					}
                }
				DataLayer.Repository.RateDic = result.RateDic;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                result.Error = new CustomException();
                result.Error.Message = ex.Message + ex.StackTrace;
            }

            //we read from the database from te table Currencies
            // assumption is - if ClassName is not null then the rest of the parameters for each currency should be populated for Quantlib
            // otherwise it was manually added and everything should be coming from the database only

            return result;
        }

        public ResponseBondData GetBondDataDic()
        {
            ResponseBondData result = new ResponseBondData();
            try
            {
				var bonds = DataLayer.Repository.GetBonds();
				foreach (Bond b in bonds)
				{
					result.BondDic[b.Id] = b;
				}
				DataLayer.Repository.BondDic = result.BondDic;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                result.Error = new CustomException();
                result.Error.Message = ex.Message + ex.StackTrace;
            }

            //we read from the database from te table Currencies
            // assumption is - if ClassName is not null then the rest of the parameters for each currency should be populated for Quantlib
            // otherwise it was manually added and everything should be coming from the database only

            return result;
        }

		public ResponseDayCounterData GetDayCounterDic()
		{
			ResponseDayCounterData result = new ResponseDayCounterData();
			try
			{
				var dc = DataLayer.Repository.GetDayCounters();
				foreach (DayCounter i in dc)
				{
                    if (i != null
                        && !String.IsNullOrEmpty(i.ClassName)
                        && QuantLibAdaptor.GetDayCounterInformation(i)
                            )
                        result.DayCounterDic[i.Id] = i;
				}
				DataLayer.Repository.DayCounterDic = result.DayCounterDic;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}

		public ResponseExchangeRateData GetExchangeRateDataDic()
		{
			ResponseExchangeRateData result = new ResponseExchangeRateData();

			try
			{
				var rates = DataLayer.Repository.GetExchangeRates();

				foreach (var r in rates)
				{
					if (r != null)
						result.XRateDic[r.Id] = r;
				}
				DataLayer.Repository.XRateDic = result.XRateDic;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}

		public ResponseEntryPointHistory GetExchangeRateHistoryList(DateTime settlementDate, long? idYc)
		{
			ResponseEntryPointHistory result = new ResponseEntryPointHistory();

			try
			{
				result.EntryPointHistoryList = DataLayer.Repository.GetExchangeRateHistory(idYc, settlementDate);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}

		//
		// Chart: Blue Line 
		//

        public ResponseEntryPointHistory GetEntryPointHistoryList(DateTime settlementDate, long? idYc)
        {
			ResponseEntryPointHistory result = new ResponseEntryPointHistory();
			
            try
            {
				result.EntryPointHistoryList = DataLayer.Repository.GetYieldCurveEntryPointHistory(idYc, settlementDate);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                result.Error = new CustomException();
                result.Error.Message = ex.Message + ex.StackTrace;
            }

            return result;
        }

        // iftoDraw indicates if only computation is required or after computation is done the chart has to be drawn by SL client
        public ResponseDiscountsList CalculateDiscountedRateList(YieldCurveData ycd, DateTime settlementDate, HashSet<DateTime> discountDates, bool ifToDraw)
		{
			ResponseDiscountsList result = new ResponseDiscountsList();
            result.ifToDraw = ifToDraw;

			try
			{
				// will be only one YieldCurveData element if idYc is provided (this is unique key)
				//YieldCurveData ycDesc = Repository.GetYieldCurveData(idYc).Single();
				ycd.settlementDate = settlementDate;
				ycd.entryPointList = DataLayer.Repository.GetYieldCurveEntryPoint(ycd.Id, settlementDate);

				QuantLibAdaptor.CalculateDiscountedRate(ycd, discountDates.ToList(), result.discountPoints);

				/*
				for (int i = 0; i < ycDesc.entryPoints.Count(); i++)
				{
					result.EnabledEntry.Add(ycDesc.entryPoints[i].Enabled);
					result.EntryId.Add(ycDesc.entryPoints[i].Id);
				}
				*/

				result.YcId = ycd.Id;
				result.entryPoints = ycd.entryPointList;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}



		#region INFLATION CURVES

		public ResponseInflationInit InflationInit(DateTime settlementDate)
		{
			ResponseInflationInit result = new ResponseInflationInit();
			try
			{
				result.ib = GetInflationBonds();
				result.ii = GetInflationIndex();
				result.rh = GetRateHistory();
				result.ir = GetInflationRate();
				// the above call 1st, to avoid recursive sql queries
				result.icf = GetInflationCurveFamily();
				result.ic = GetInflationCurveData();
				result.iced = GetInflationCurveEntryData();
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			if ((result.icf.Error != null) ||
				 (result.ic.Error != null) ||
				 (result.iced.Error != null) ||
				 (result.ib.Error != null) ||
				 (result.rh.Error != null))
			{
				result.Error = new CustomException();
				if (result.icf.Error != null)
					result.Error.Message = result.icf.Error.Message;
				if (result.ic.Error != null)
					result.Error.Message = result.Error.Message + result.ic.Error.Message;
				if (result.iced.Error != null)
					result.Error.Message = result.Error.Message + result.iced.Error.Message;
				if (result.ib.Error != null)
					result.Error.Message = result.Error.Message + result.ib.Error.Message;
				if (result.rh.Error != null)
					result.Error.Message = result.Error.Message + result.rh.Error.Message;
			}
			return result;
		}

		public ResponseInflationFamilyData GetInflationCurveFamily()
		{
			ResponseInflationFamilyData result = new ResponseInflationFamilyData();
			try
			{
				result.InflationFamilyList = DataLayer.Repository.GetInflationCurveFamily(null, null);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseInflationCurveData GetInflationCurveData()
		{
			ResponseInflationCurveData result = new ResponseInflationCurveData();
			try
			{
				var ycs = DataLayer.Repository.GetInflationCurveData(null);
				foreach (InflationCurveData icd in ycs)
				{
					result.InflationCurveDataDic[icd.Id] = icd;
				}
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseInflationCurveEntryData GetInflationCurveEntryData()
		{
			ResponseInflationCurveEntryData result = new ResponseInflationCurveEntryData();
			try
			{
				var ycs = DataLayer.Repository.GetInflationCurveEntryData(null, null);
				foreach (InflationCurveEntryData icd in ycs)
				{
					result.InflationCurveEntryDataDic[icd.Id] = icd;
				}
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseInflationBondData GetInflationBonds()
		{
			ResponseInflationBondData result = new ResponseInflationBondData();
			try
			{
				var ycs = DataLayer.Repository.GetInflationBonds(null);
				foreach (InflationBond icd in ycs)
				{
					result.BondDic[icd.Id] = icd;
				}
				DataLayer.Repository.InflationBondDic = result.BondDic;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseInflationIndexData GetInflationIndex()
		{
			ResponseInflationIndexData result = new ResponseInflationIndexData();
			try
			{
				var ycs = DataLayer.Repository.GetInflationIndex(null);
				foreach (InflationIndex iid in ycs)
				{
					result.InflationDic[iid.Id] = iid;
				}
				DataLayer.Repository.InflationIndexDic = result.InflationDic;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseInflationRateData GetInflationRate()
		{
			ResponseInflationRateData result = new ResponseInflationRateData();
			try
			{
				var icr = DataLayer.Repository.GetInflationRates();
				foreach (InflationRate iir in icr)
				{
					result.InflationRateDic[iir.Id] = iir;
				}
				DataLayer.Repository.InflationRateDic = result.InflationRateDic;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseRateHistory GetRateHistory()
		{
			ResponseRateHistory result = new ResponseRateHistory();
			try
			{
				var ycs = DataLayer.Repository.GetRateHistory(null);
				foreach (RateHistory rh in ycs)
				{
					if (rh.Type == "InflationIndex")
						result.InflationIndexHistoryDic[rh.Id] = rh;
					else if (rh.Type == "InflationBond")
						result.InflationBondsHistoryDic[rh.Id] = rh;
					else if (rh.Type == "InflationRate")
						result.InflationRateHistoryDic[rh.Id] = rh;
				}
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseDiscountsList CalculateInflationRateList(InflationCurveSnapshot ycd, DateTime settlementDate, HashSet<DateTime> discountDates, bool ifToDraw)
		{
			ResponseDiscountsList result = new ResponseDiscountsList();
			result.ifToDraw = ifToDraw;
			try
			{
				//CalculateInflationRate1b is working for europe               
				QuantLibAdaptor.IterativeInflationCurveCalculation(ycd, discountDates.ToList(), result.discountPoints);

				//    result.entryPoints = Repository.GetInflationCurveEntryData(ycd.Id, settlementDate);
				result.YcId = ycd.Id;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}

		//this will return maturities for instruments taking into account todays date
		//relevant for rates or bonds with relative maturity
		//instruments calendar is taken into account
		public ResponseMaturityDatesData GetMaturityDatesList(List<Instrument> rate, DateTime d)
		{
			ResponseMaturityDatesData result = new ResponseMaturityDatesData();

			List<DateTime> result1 = new List<DateTime>();

			try
			{
				QuantLibAdaptor.GetInstrumentMaturity(rate, d, result1);
				result.dates = result1;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}

		#endregion

		//
		// DataFeed: set ASP.NET compatibility mode: <serviceHostingEnvironment aspNetCompatibilityEnabled="true" />
		//

		public ResponseInitData InitData()
		{
			/*
			// Check if invoked from web-form
			UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
			if (user == null)
			{
				// --need wsHttpBinding, which is not supported by Silverlight
				
				// Otherwise check if invoked by API
				Guid sessionId = new Guid(OperationContext.Current.SessionId);
				if (!UserAccounts.FCUser.sActiveUsersDic.ContainsKey(sessionId))
				{
					// user is not authenticated, demo mode = ON, read-only access
				}
				else
					user = UserAccounts.FCUser.sActiveUsersDic[sessionId];
			}
			*/

			ResponseInitData result = new ResponseInitData();
			try
			{
				if (HttpContext.Current == null || HttpContext.Current.Session == null)
					throw new Exception("=== no connection made");

				UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
				if (user == null)
				{
					result.Error = new CustomException();
					result.Error.Message = "Client is not authenticated, will work in Demo read-only mode";
				}

				// needs quantlib class factory
				GetDayCounterDic();			// shell be before Rate and YieldCurveData, cos those are using DayCounter inside
				
				foreach (var v in GetCurrencyDataDic().CurrencyDic.Values)
					result.ccyList.Add(v);
				foreach (var v in GetRateDataDic().RateDic.Values)
					result.instrList.Add(v);
				foreach (var v in GetBondDataDic().BondDic.Values)
					result.instrList.Add(v);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}

		public ResponseGetAllEntryPointsData GetAllEntryPoints(bool isDemo)
		{
			ResponseGetAllEntryPointsData result = new ResponseGetAllEntryPointsData();
			
			try
			{
				UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
				if (user == null)
				{
					result.Error = new CustomException();
					result.Error.Message = "Client is not authenticated, will work in Demo read-only mode";
					isDemo = true;
				}

				result.epl = DataFeed.Repository.GetAllEntryPoints(isDemo);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseAddEntryPoint AddEntryPoint(EntryPoint ep)
		{
			ResponseAddEntryPoint result = new ResponseAddEntryPoint();

			try
			{
				// Check if invoked from web-form
				UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
				if (user == null)
					throw new Exception("Client is not authenticated");

				DataFeed.Repository.AddEntryPoint(ep);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		// modify the existing EntryPoint (use GetAllEntryPoints() to get all of them (of Loged In user) 
		// or GetEntryPointByRateIdType() to get the specific one by its id/type
		public ResponseUpdateEntryPoint UpdateEntryPoint(EntryPoint ep)
		{
			ResponseUpdateEntryPoint result = new ResponseUpdateEntryPoint();

			try
			{
				// Check if invoked from web-form
				UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
				if (user == null)
					throw new Exception("Client is not authenticated");

				DataFeed.Repository.UpdateEntryPoint(ep);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		// get complete EntryPoint definition by InstrumentId (ep.Instrument.Id) and InstrumentType (ep.Type = {"bond", "swap", "deposit"})
		public ResponseEntryPointsByInstrument GetEntryPointByInstrument(Instrument instr)
		{
			ResponseEntryPointsByInstrument result = new ResponseEntryPointsByInstrument();
			try 
			{
				UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
				if (user == null)
				{
					result.Error = new CustomException();
					result.Error.Message = "Client is not authenticated, will work in Demo read-only mode";
				}

				result.epList = DataFeed.Repository.GetEntryPointByInstrument(instr);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		public ResponseAddEntryPointHistory AddEntryPointHistory(List<EntryPoint> epl)
		{
			ResponseAddEntryPointHistory result = new ResponseAddEntryPointHistory();
			try
			{
				// Check if invoked from web-form
				UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
				if (user == null)
					throw new Exception("Client is not authenticated");

				DataFeed.Repository.AddEntryPointHistory(epl);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}

		// idYc - each EntryPoint has reference to YieldCurve settings
		public ResponseYieldCurveDefinition GetYieldCurveDefinitions()
		{
			ResponseYieldCurveDefinition result = new ResponseYieldCurveDefinition();
			try
			{
				var ycs = DataLayer.Repository.GetYieldCurveData(null);
				foreach (YieldCurveData ycd in ycs)
				{
					YieldCurveDefinition ycDef = new YieldCurveDefinition();
					ycDef = (YieldCurveDefinition)ycd.settings; ;
					ycDef.Id = ycd.Id;
					ycDef.CurrencyId = ycd.CurrencyId;
					result.ycDefList.Add(ycDef);
				}
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}
		
		// idYc - each EntryPoint has reference to YieldCurve settings
		public ResponseEntryPointHistory GetYcHistoricEntryPoints(DateTime settlementDate, YieldCurveDefinition ycDef)
		{
			ResponseEntryPointHistory result = new ResponseEntryPointHistory();
			try
			{
				UserAccounts.FCUser user = (UserAccounts.FCUser)HttpContext.Current.Session["user"];
				if (user == null)
				{
					result.Error = new CustomException();
					result.Error.Message = "Client is not authenticated, will work in Demo read-only mode";
				}
				result = GetEntryPointHistoryList(settlementDate, ycDef.Id);
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}
			return result;
		}
		
		public ResponseDiscountedData CalculateDiscountedData(YieldCurveDefinition ycDef, 
																DateTime settlementDate,
																List<EntryPoint> entryPoints,
																List<DateTime> discountDates)
		{
			ResponseDiscountedData result = new ResponseDiscountedData();
			try
			{
				YieldCurveData tmp = new YieldCurveData();
				tmp.settlementDate = settlementDate;
				tmp.settings = ycDef;

				if (entryPoints == null)
					//tmp.entryPointList = DataLayer.Repository.GetYieldCurveEntryData(ycd.Id, settlementDate);
					tmp.entryPointList = DataLayer.Repository.GetYieldCurveEntryPoint(ycDef.Id, settlementDate);
				else
					tmp.entryPointList = entryPoints;

				QuantLibAdaptor.CalculateDiscountedRate(tmp, discountDates.ToList(), result.discountPoints);

				result.entryPoints = tmp.entryPointList;
			}
			catch (Exception ex)
			{
				while (ex.InnerException != null)
					ex = ex.InnerException;

				result.Error = new CustomException();
				result.Error.Message = ex.Message + ex.StackTrace;
			}

			return result;
		}

		//
		// User Accounts
		//

		// name is the email to where the confirmation code will be sent
		// don't pass created user details back, as it has confirmation code inside
		// client have to get it by email used as name and confirm by passing back as
		// parameter to ConfirmUser()
		public bool CreateUser(string name, string pass)
		{
			// creates or confirms user
			return (null != UserAccounts.Repository.CreateUser(name, pass));
		}

		public bool ConfirmUser(string code)
		{
			// creates or confirms user
			return UserAccounts.Repository.ConfirmUser(code);
		}

		public bool LoginUser(string name, string pass)
		{
			UserAccounts.FCUser tmp = UserAccounts.Repository.LoginUser(name, pass);
			if (tmp == null
				|| tmp.status != UserAccounts.FCUser.eStatus.eEnabled)
			{
				// can't login
				return false;
			}

			/*
			Guid currentSessionId = new Guid(OperationContext.Current.SessionId);
			UserAccounts.FCUser.sActiveUsersDic[sessionId] = tmp;
			*/

			HttpContext.Current.Session.Timeout = 60;
            HttpContext.Current.Session["LoggedIn"] = true;
			HttpContext.Current.Session["user"] = tmp;
			return true;
		}

		public void LogoutUser()
		{
			/*
			Guid currentSessionId = new Guid(OperationContext.Current.SessionId);
			UserAccounts.FCUser.sActiveUsersDic.Remove(currentSessionId);
			*/
			HttpContext.Current.Session.Abandon();
		}
    }
}
