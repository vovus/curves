using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DataLayer;

namespace YieldCurveWcf
{
    //[ServiceContract ( SessionMode=SessionMode.Required)]
	[ServiceContract]
    public interface IYieldCurveSrv
    {
		[OperationContract]
		ResponseInit Init(DateTime settlementDate);

		[OperationContract]
		ResponseYcFamilyData GetYcFamilyDataList();

		[OperationContract]
		ResponseYieldCurveData GetYieldCurveDataDic();

        [OperationContract]
        ResponseCurrencyData GetCurrencyDataDic();

		[OperationContract]
		ResponseRateData GetRateDataDic();

        [OperationContract]
        ResponseBondData GetBondDataDic();

		[OperationContract]
		ResponseDayCounterData GetDayCounterDic();

        [OperationContract]
        ResponseEntryPointHistory GetEntryPointHistoryList(DateTime settlementDate, long? idYc);

        [OperationContract]
        ResponseDiscountsList CalculateDiscountedRateList(YieldCurveData ycd, DateTime settlementDate, HashSet<DateTime> discountDates, bool ifToDraw);

		[OperationContract]
		ResponseExchangeRateData GetExchangeRateDataDic();

		[OperationContract]
		ResponseEntryPointHistory GetExchangeRateHistoryList(DateTime settlementDate, long? idYc);

		#region INFLATION CURVE

		[OperationContract]
		ResponseMaturityDatesData GetMaturityDatesList(List<Instrument> instruments, DateTime d);

		[OperationContract]
		ResponseInflationInit InflationInit(DateTime settlementDate);

		[OperationContract]
		[ServiceKnownType(typeof(Instrument))]
		[ServiceKnownType(typeof(InflationBond))]
		[ServiceKnownType(typeof(InflationRate))]
		ResponseDiscountsList CalculateInflationRateList(InflationCurveSnapshot ycd, DateTime settlementDate, HashSet<DateTime> discountDates, bool ifToDraw);

		#endregion

		//
		// DataFeed (has to be in session context of authenticated users)
		//

		//[OperationContract(IsInitiating = false, IsTerminating = false)]
		[OperationContract]
		ResponseInitData InitData();

		//[OperationContract(IsInitiating = false, IsTerminating = false)]
		[OperationContract]
		ResponseGetAllEntryPointsData GetAllEntryPoints(bool isDemo);

		//[OperationContract(IsInitiating = false, IsTerminating = false)]
		[OperationContract]
		ResponseEntryPointsByInstrument GetEntryPointByInstrument(Instrument instr);

		//[OperationContract(IsInitiating = false, IsTerminating = false)]
		[OperationContract]
		ResponseUpdateEntryPoint UpdateEntryPoint(EntryPoint ep);

		//[OperationContract(IsInitiating = false, IsTerminating = false)]
		[OperationContract]
		ResponseAddEntryPoint AddEntryPoint(EntryPoint ep);

		//[OperationContract(IsInitiating = false, IsTerminating = false)]
		[OperationContract]
		ResponseAddEntryPointHistory AddEntryPointHistory(List<EntryPoint> epl);

		[OperationContract]
		ResponseYieldCurveDefinition GetYieldCurveDefinitions();

		[OperationContract]
		ResponseEntryPointHistory GetYcHistoricEntryPoints(DateTime settlementDate, YieldCurveDefinition ycDef);

		[OperationContract]
		ResponseDiscountedData CalculateDiscountedData(YieldCurveDefinition ycDef, DateTime settlementDate, List<EntryPoint> entryPoints, List<DateTime> discountDates);

		//
		// User Account
		//

		[OperationContract]
		bool CreateUser(string name, string pass);

		[OperationContract]
		bool ConfirmUser(string code);

		//[OperationContract (IsInitiating = true, IsTerminating = false)]
		[OperationContract]
		bool LoginUser(string name, string pass);

		//[OperationContract(IsInitiating = false, IsTerminating = true)]
		[OperationContract]
		void LogoutUser();
    }

	/*
	//
	// User Account
	//
	[DataContract]
	public class ResponseUser
	{ 
		[DataMember]
		public UserAccounts.FCUser user;
	}
	*/

	//
	// DataFeed
	// 

	[DataContract]
	[KnownType(typeof(Bond))]
	[KnownType(typeof(Rate))]
	[KnownType(typeof(InflationBond))]
	[KnownType(typeof(InflationIndex))]
	[KnownType(typeof(InflationRate))]

	public class ResponseInitData
	{
		//[DataMember]
		//public ResponseDayCounterData dc;

		[DataMember]
		public List<Currency> ccyList;

		[DataMember]
		public List<Instrument> instrList;

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseEntryPointsByInstrument
	{
		public List<EntryPoint> epList;

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	[KnownType(typeof(Bond))]
	[KnownType(typeof(Rate))]
	public class ResponseGetAllEntryPointsData
	{
		[DataMember]
		public List<EntryPoint> epl;

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseYieldCurveDefinition
	{
		[DataMember]
		public List<YieldCurveDefinition> ycDefList;

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseAddEntryPoint
	{
		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseUpdateEntryPoint
	{
		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseAddEntryPointHistory
	{
		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	[KnownType(typeof(Bond))]
	[KnownType(typeof(Rate))]
	public class ResponseDiscountedData
	{
		[DataMember]
		public List<DiscountPoint> discountPoints;

		[DataMember]
		public List<EntryPoint> entryPoints;

		public ResponseDiscountedData()
		{
			discountPoints = new List<DiscountPoint>();
			entryPoints = new List<EntryPoint>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

    //
	// SL
	//
	
	[DataContract]
	public class ResponseInit
	{
		[DataMember]
		public ResponseYieldCurveData yc;

		[DataMember]
		public ResponseYcFamilyData ycf;

		[DataMember]
		public ResponseCurrencyData ccy;

		[DataMember]
		public ResponseRateData r;

		[DataMember]
		public ResponseBondData b;

		[DataMember]
		public ResponseEntryPointHistory edh;

		[DataMember]
		public ResponseDayCounterData dc;

		[DataMember]
		public ResponseExchangeRateData xr;

		[DataMember]
		public ResponseEntryPointHistory xrh;

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseYcFamilyData
	{
		[DataMember]
		public List<YieldCurveFamily> YcFamilyList;

		public ResponseYcFamilyData()
		{
			YcFamilyList = new List<YieldCurveFamily>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseYieldCurveData
	{
		[DataMember]
		public Dictionary<long, YieldCurveData> YieldCurveDataDic;

		public ResponseYieldCurveData()
		{
			YieldCurveDataDic = new Dictionary<long, YieldCurveData>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

    [DataContract]
    public class ResponseCurrencyData
    {
        [DataMember]
        public Dictionary<long, Currency> CurrencyDic;

        public ResponseCurrencyData()
        {
            CurrencyDic = new Dictionary<long, Currency>();
        }

        [DataMember]
        public CustomException Error { get; set; }
    }
	
    [DataContract]
    public class ResponseRateData
    {
        [DataMember]
        public Dictionary<long, Rate> RateDic;

        public ResponseRateData()
        {
            RateDic = new Dictionary<long, Rate>();
        }

        [DataMember]
        public CustomException Error { get; set; }
    }
	
    [DataContract]
    public class ResponseBondData
    {
        [DataMember]
        public Dictionary<long, Bond> BondDic;

        public ResponseBondData()
        {
            BondDic = new Dictionary<long, Bond>();
        }

        [DataMember]
        public CustomException Error { get; set; }
    }

	[DataContract]
	public class ResponseDayCounterData
	{ 
		[DataMember]
		public Dictionary<long, DayCounter> DayCounterDic;	// Name (as used in GUI) is a key

		public ResponseDayCounterData()
        {
			DayCounterDic = new Dictionary<long, DayCounter>();
        }

        [DataMember]
        public CustomException Error { get; set; }
	}

    [DataContract]
	[KnownType(typeof(Bond))]
	[KnownType(typeof(Rate))]
	[KnownType(typeof(EntryPoint))]
    public class ResponseEntryPointHistory
    {
        [DataMember]
		public List<EntryPointHistory> EntryPointHistoryList;

        public ResponseEntryPointHistory()
        {
			EntryPointHistoryList = new List<EntryPointHistory>();
        }

        [DataMember]
        public CustomException Error { get; set; }
    }

    [DataContract]
	[KnownType(typeof(Bond))]
	[KnownType(typeof(Rate))]
    public class ResponseDiscountsList
    {
        [DataMember]
        public bool ifToDraw;  //this will indicate whether after receiving the computations SL part will draw the chart
        //in fact it will be passed as argument from the client and pased it back again

        [DataMember]
        public int YcId;
        
        [DataMember]
		public List<DiscountPoint> discountPoints;

        //[DataMember]
        //public List<int> EntryId; // list of entryId and list of EnabledEntry are used together to mark all disable entry points

        //[DataMember]
        //public List<Boolean> EnabledEntry; 

		[DataMember]
		public List<EntryPoint> entryPoints;

		public ResponseDiscountsList()
        {
            ifToDraw = false;
			discountPoints = new List<DiscountPoint>();
			entryPoints = new List<EntryPoint>();
            //EntryId = new List<int>();
            //EnabledEntry=new List<Boolean>();
        }

        [DataMember]
        public CustomException Error { get; set; }
    }

	[DataContract]
	public class ResponseExchangeRateData
	{
		[DataMember]
		public Dictionary<long, ExchangeRate> XRateDic;

		public ResponseExchangeRateData()
		{
			XRateDic = new Dictionary<long, ExchangeRate>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	#region INFLATION CURVE

	[DataContract]
	//[KnownType(typeof(InflationCurveSetting))]
	//[KnownType(typeof(InflationCurveEntryData))]
	//[KnownType(typeof(YieldCurveFamily))]
	//[KnownType(typeof(InflationCurveData))]
	//[KnownType(typeof(Instrument))]
	[KnownType(typeof(InflationBond))]
	[KnownType(typeof(InflationRate))]
	[KnownType(typeof(InflationIndex))]
	//[KnownType(typeof(RateHistory))]
	public class ResponseInflationInit
	{
		[DataMember]
		public ResponseInflationFamilyData icf;

		[DataMember]
		public ResponseInflationCurveData ic;

		[DataMember]
		public ResponseInflationCurveEntryData iced;

		[DataMember]
		public ResponseInflationBondData ib;

		[DataMember]
		public ResponseInflationIndexData ii;

		[DataMember]
		public ResponseInflationRateData ir;

		[DataMember]
		public ResponseRateHistory rh;

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	//[KnownType(typeof(YieldCurveFamily))]
	public class ResponseInflationFamilyData
	{
		[DataMember]
		public List<YieldCurveFamily> InflationFamilyList;

		public ResponseInflationFamilyData()
		{
			InflationFamilyList = new List<YieldCurveFamily>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	//[KnownType(typeof(InflationCurveData))]
	//[KnownType(typeof(InflationCurveSetting))]
	//[KnownType(typeof(InflationCurveEntryData))]
	public class ResponseInflationCurveData
	{
		[DataMember]
		public Dictionary<long, InflationCurveData> InflationCurveDataDic;

		public ResponseInflationCurveData()
		{
			InflationCurveDataDic = new Dictionary<long, InflationCurveData>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	[KnownType(typeof(InflationBond))]
	public class ResponseInflationCurveEntryData
	{
		[DataMember]
		public Dictionary<long, InflationCurveEntryData> InflationCurveEntryDataDic;

		public ResponseInflationCurveEntryData()
		{
			InflationCurveEntryDataDic = new Dictionary<long, InflationCurveEntryData>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	//[KnownType(typeof(Instrument))]
	[KnownType(typeof(InflationBond))]
	public class ResponseInflationBondData
	{
		[DataMember]
		public Dictionary<long, InflationBond> BondDic;

		public ResponseInflationBondData()
		{
			BondDic = new Dictionary<long, InflationBond>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	[KnownType(typeof(InflationIndex))]
	public class ResponseInflationIndexData
	{
		[DataMember]
		public Dictionary<long, InflationIndex> InflationDic;

		public ResponseInflationIndexData()
		{
			InflationDic = new Dictionary<long, InflationIndex>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	//[KnownType(typeof(Instrument))]
	[KnownType(typeof(InflationRate))]
	public class ResponseInflationRateData
	{
		[DataMember]
		public Dictionary<long, InflationRate> InflationRateDic;

		public ResponseInflationRateData()
		{
			InflationRateDic = new Dictionary<long, InflationRate>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	//[KnownType(typeof(RateHistory))]
	//[KnownType(typeof(InflationCurveEntryData))]
	public class ResponseRateHistory
	{
		[DataMember]
		public Dictionary<long, RateHistory> InflationBondsHistoryDic;
		[DataMember]
		public Dictionary<long, RateHistory> InflationIndexHistoryDic;
		[DataMember]
		public Dictionary<long, RateHistory> InflationRateHistoryDic;

		public ResponseRateHistory()
		{
			InflationBondsHistoryDic = new Dictionary<long, RateHistory>();
			InflationIndexHistoryDic = new Dictionary<long, RateHistory>();
			InflationRateHistoryDic = new Dictionary<long, RateHistory>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	[DataContract]
	public class ResponseMaturityDatesData
	{
		[DataMember]
		public List<DateTime> dates;

		public ResponseMaturityDatesData()
		{
			dates = new List<DateTime>();
		}

		[DataMember]
		public CustomException Error { get; set; }
	}

	#endregion

	//////////////////////////////////////////////////////////////////////////

    [DataContract]
    public class CustomException
    {
        [DataMember(Order = 0)]
        public string Message { get; set; }

        [DataMember(Order = 1)]
        public CustomException InnerException;

        public Exception ToException()
        {
            Exception e;
            CustomException ce = this;
            if (ce.InnerException != null)
            {
                Exception inner = ce.InnerException.ToException();
                e = new Exception(ce.Message, inner);
            }
            else
                e = new Exception(ce.Message);
            return e;
        }
    }
}
