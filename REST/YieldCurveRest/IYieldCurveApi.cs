using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using DataLayer;
using System.IO;

namespace YieldCurveRest
{
	[ServiceContract]
	public interface IYieldCurveApi
	{
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/dc")]
		string GetDayCounters();

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/dc/{id}")]
		string GetDayCounter(string id);

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare,
			UriTemplate = "yc/ccy")]
		Stream GetCurrencies();

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/ccy/{id}")]
		string GetCurrency(string id);

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/rate")]
		string GetRates();

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/rate/{id}")]
		string GetRate(string id);

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/bond")]
		string GetBonds();

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/bond/{id}")]
		string GetBond(string id);

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare,
			UriTemplate = "yc/fam")]
		Stream GetYcFamilies();

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/fam/{id}")]
		string GetYcFamily(string id);

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare,
			UriTemplate = "yc/def")]
		Stream GetYcDefs();

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/def/{id}")]
		string GetYcDef(string id);

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare,
			UriTemplate = "yc/eph/{date}")]
		string GetEpAllHistory(string date);

		// get Entry data raw
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare,
			UriTemplate = "yc/eph/{date}/{id}")]
		string GetEpHistory(string date, string id);

		// get Entry data for Grid
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare,
			UriTemplate = "yc/ephg/{date}/{id}")]
		Stream GetEpHistoryGrid(string date, string id);

		// get Entry data for Chart
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Bare,
			UriTemplate = "yc/ephc/{date}/{id}")]
		Stream GetEpHistoryChart(string date, string id);

		[WebInvoke(Method = "POST",						// pass entry points
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/disc/{date}/{id}/{epl}/{ddl}")]
		string CalculateDiscountedRates(string date, string id, string epl, string ddl);

		[WebInvoke(Method = "GET",						// use stored entry points
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/disc/{date}/{id}")]
		string CalculateDiscountedRatesSimple(string date, string id);

		/*
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/discount/{id}")]
		string CalculateDiscountedData(DateTime settlementDate, long? idYc,
										List<EntryPoint> entryPoints,
										List<DateTime> discountDates);
		
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/xr")]
		string GetExchangeRateDataDic();

		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "yc/xrh/{id}")]
		string GetExchangeRateHistoryList(DateTime settlementDate, long? idYc);
		 */
	}
}
