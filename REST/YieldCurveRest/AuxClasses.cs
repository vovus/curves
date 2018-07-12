using System.Collections.Generic;
using DataLayer;
using System;
//using Google.DataTable.Net.Wrapper;
using DataFeed;

namespace YieldCurveRest 
{
	public class StringContainer
	{
		public String Value { get; set; }
	}
	/*
	public class ResponseInit
	{
		//public ResponseYieldCurveData yc;
		public ResponseDayCounterData dc;
		public ResponseCurrencyData ccy;
		public ResponseRateData r;
		public ResponseBondData b;
		public ResponseYcFamilyData ycf;
		public ResponseEntryPointHistory edh;
		public ResponseExchangeRateData xr;
		public ResponseEntryPointHistory xrh;
		public CustomException Error { get; set; }
	}
	*/
	/*
	public class ResponseYieldCurveData
	{
		public Dictionary<long, YieldCurveData> YieldCurveDataDic;
		public ResponseYieldCurveData()
		{
			YieldCurveDataDic = new Dictionary<long, YieldCurveData>();
		}
		public CustomException Error { get; set; }
	}
	*/
	public class ResponseDayCounterData
	{
		public Dictionary<long, DayCounter> DayCounterDic;	// Name (as used in GUI) is a key
		public ResponseDayCounterData()
		{
			DayCounterDic = new Dictionary<long, DayCounter>();
		}
		public CustomException Error { get; set; }
	}
	/*
	public class ResponseCurrencyData
	{
		public Dictionary<long, Currency> CurrencyDic;
		public ResponseCurrencyData()
		{
			CurrencyDic = new Dictionary<long, Currency>();
		}
		public CustomException Error { get; set; }
	}
	*/
	public class ResponseCurrencyData
	{
		public List<Currency> ccyList;
		public ResponseCurrencyData()
		{
			ccyList = new List<Currency>();
		}
		public CustomException Error { get; set; }
	}
	public class ResponseRateData
	{
		public Dictionary<long, Rate> RateDic;
		public ResponseRateData()
		{
			RateDic = new Dictionary<long, Rate>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseBondData
	{	
		public Dictionary<long, Bond> BondDic;
		public ResponseBondData()
		{
			BondDic = new Dictionary<long, Bond>();
		}
		public CustomException Error { get; set; }
	}
	/*
	public class ResponseYcFamilyData
	{
		public Dictionary<long, YieldCurveFamily> YcFamilyDic;
		public ResponseYcFamilyData()
		{
			YcFamilyDic = new Dictionary<long, YieldCurveFamily>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseYieldCurveDefinition
	{
		public Dictionary<long, YieldCurveDefinition> ycDefDic;
		public ResponseYieldCurveDefinition()
		{
			ycDefDic = new Dictionary<long, YieldCurveDefinition>();
		}
		public CustomException Error { get; set; }
	}
	*/
	public class ResponseYcFamilyData
	{
		public List<YieldCurveFamily> ycFamList;
		public ResponseYcFamilyData()
		{
			ycFamList = new List<YieldCurveFamily>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseYieldCurveDefinition
	{
		public List<YieldCurveDefinition> ycDefList;
		public ResponseYieldCurveDefinition()
		{
			ycDefList = new List<YieldCurveDefinition>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseEntryPointHistory
	{	
		public List<EntryPointHistory> epHistoryList;
		public ResponseEntryPointHistory()
		{
			epHistoryList = new List<EntryPointHistory>();
		}
		public CustomException Error { get; set; }
		/*
        internal DataTable ToGoogleDataTable()
        {
            var dt = new Google.DataTable.Net.Wrapper.DataTable();

            dt.AddColumn(new Column(ColumnType.Number, "Duration", "Duration"));
            dt.AddColumn(new Column(ColumnType.Number, "Rate", "Rate"));
			//dt.AddColumn(new Column(ColumnType.Datetime, "Term", "Term"));

            foreach (var i in epHistoryList)
            {
				DateTime tmp = DateTime.Today;
				string formattedTerm = string.Empty;

				switch (i.TimeUnit)
				{
					case "Days":
						tmp = tmp.AddDays(i.Length);
						formattedTerm = String.Format("{0}d", i.Length);
						break;
					case "Weeks":
						tmp = tmp.AddDays(7 * i.Length);
						formattedTerm = String.Format("{0}w", i.Length);
						break;
					case "Months":
						tmp = tmp.AddMonths(i.Length);
						formattedTerm = String.Format("{0}m", i.Length);
						break;
					case "Years":
						tmp = tmp.AddYears(i.Length);
						formattedTerm = String.Format("{0}y", i.Length);
						break;
				}
				
                //string formattedDuration = String.Format("{0}{1}", i.Length, i.TimeUnit);
                string formattedRate = String.Format("{0:p2}", i.epValue.Value);

                Row r = dt.NewRow();
                r.AddCellRange(new Cell[]
				{
                    new Cell(i.Duration, formattedTerm),
                    new Cell(i.epValue.Value, formattedRate)//,
					//new Cell(tmp, formattedTerm)
				});
                dt.AddRow(r);
            }

            return dt;
        }
		*/
	}

	public class ResponseDiscountsList
	{
		public bool ifToDraw;	//this will indicate whether after receiving the computations SL part will draw the chart
								//in fact it will be passed as argument from the client and pased it back again
		public int YcId;
		public List<DiscountPoint> discountPoints;		
		public List<EntryPoint> entryPoints;

		public ResponseDiscountsList()
		{
			ifToDraw = false;
			discountPoints = new List<DiscountPoint>();
			entryPoints = new List<EntryPoint>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseExchangeRateData
	{
		public Dictionary<long, ExchangeRate> XRateDic;
		public ResponseExchangeRateData()
		{
			XRateDic = new Dictionary<long, ExchangeRate>();
		}
		public CustomException Error { get; set; }
	}

	public class CustomException
	{
		public string Message { get; set; }
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

	#region INFLATION CURVE

	public class ResponseInflationInit
	{
		public ResponseInflationFamilyData icf;
		public ResponseInflationCurveData ic;
		public ResponseInflationCurveEntryData iced;
		public ResponseInflationBondData ib;
		public ResponseInflationIndexData ii;
		public ResponseInflationRateData ir;
		public ResponseRateHistory rh;
		public CustomException Error { get; set; }
	}

	public class ResponseInflationFamilyData
	{
		public List<YieldCurveFamily> InflationFamilyList;
		public ResponseInflationFamilyData()
		{
			InflationFamilyList = new List<YieldCurveFamily>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseInflationCurveData
	{
		public Dictionary<long, InflationCurveData> InflationCurveDataDic;
		public ResponseInflationCurveData()
		{
			InflationCurveDataDic = new Dictionary<long, InflationCurveData>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseInflationCurveEntryData
	{
		public Dictionary<long, InflationCurveEntryData> InflationCurveEntryDataDic;
		public ResponseInflationCurveEntryData()
		{
			InflationCurveEntryDataDic = new Dictionary<long, InflationCurveEntryData>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseInflationBondData
	{
		public Dictionary<long, InflationBond> BondDic;
		public ResponseInflationBondData()
		{
			BondDic = new Dictionary<long, InflationBond>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseInflationIndexData
	{
		public Dictionary<long, InflationIndex> InflationDic;
		public ResponseInflationIndexData()
		{
			InflationDic = new Dictionary<long, InflationIndex>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseInflationRateData
	{
		public Dictionary<long, InflationRate> InflationRateDic;
		public ResponseInflationRateData()
		{
			InflationRateDic = new Dictionary<long, InflationRate>();
		}
		public CustomException Error { get; set; }
	}

	public class ResponseRateHistory
	{
		public Dictionary<long, RateHistory> InflationBondsHistoryDic;
		public Dictionary<long, RateHistory> InflationIndexHistoryDic;
		public Dictionary<long, RateHistory> InflationRateHistoryDic;

		public ResponseRateHistory()
		{
			InflationBondsHistoryDic = new Dictionary<long, RateHistory>();
			InflationIndexHistoryDic = new Dictionary<long, RateHistory>();
			InflationRateHistoryDic = new Dictionary<long, RateHistory>();
		}

		public CustomException Error { get; set; }
	}

	public class ResponseMaturityDatesData
	{
		public List<DateTime> dates;
		public ResponseMaturityDatesData()
		{
			dates = new List<DateTime>();
		}
		public CustomException Error { get; set; }
	}

	#endregion

	public class ResponseInitData
	{
		public List<Currency> ccyList;
		public List<Instrument> instrList;
		public CustomException Error { get; set; }
	}

	public class ResponseEntryPointsByInstrument
	{
		public List<EntryPoint> epList;
		public CustomException Error { get; set; }
	}

	public class ResponseGetAllEntryPointsData
	{
		public List<EntryPoint> epl;
		public CustomException Error { get; set; }
	}

	public class ResponseAddEntryPoint
	{
		public CustomException Error { get; set; }
	}

	public class ResponseUpdateEntryPoint
	{
		public CustomException Error { get; set; }
	}

	public class ResponseAddEntryPointHistory
	{
		public CustomException Error { get; set; }
	}

	public class ResponseDiscountedData
	{
		public List<DiscountPoint> discountPoints;
		public List<EntryPoint> entryPoints;
		public ResponseDiscountedData()
		{
			discountPoints = new List<DiscountPoint>();
			entryPoints = new List<EntryPoint>();
		}
		public CustomException Error { get; set; }
	}
}
