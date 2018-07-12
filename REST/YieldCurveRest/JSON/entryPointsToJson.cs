using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using Newtonsoft.Json;

namespace YieldCurveRest
{
	public class CEntryPointsToJson
	{
		/*
		"rows": [
			{
				"id": 3,
				"cell": [
					"1d",
					0.87523,
					"Deposit",
					"LIBOR-022"
				] 
			},
			{}
		]
		*/
		static public string ToGrid(ResponseEntryPointHistory eph)
		{
			if (eph == null)
				return String.Format("{\"rows\": [{ \"id\":\"\", \"cell\": [\"\", \"\", \"\", \"\"]}]}");

			List<EntryPointHistory> epHistoryList = eph.epHistoryList;
			/*
				"page": "1",
				"records": "10",
				"total": "2",
			 */
			string jsonStr = String.Format("{{\"page\": \"{0}\", \"records\": \"{1}\", \"total\": \"{2}\", \"rows\": [",
												1, epHistoryList.Count, epHistoryList.Count);

			int k = 0;
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

				jsonStr += String.Format("{{\"id\": {0}, \"cell\": [\"{1}\", {2}, \"{3}\", \"{4}\"]}}, ",
								k++, formattedTerm, i.epValue.Value, i.Type, i.DataReference);

				//jsonStr += String.Format("{ \"id\": {0}, ", k);
				//jsonStr += String.Format("\"cell\": ");
				//jsonStr += String.Format("[{1}, {2}, {3}, {4}]}, ", formattedTerm, formattedRate, i.Type, i.DataReference);
			}
			jsonStr += "]}";

			//string jsonStr = JsonConvert.SerializeObject(dt, Formatting.Indented);

			return jsonStr;
		}

		static public string ToChart(ResponseEntryPointHistory eph)
		{
			List<EntryPointHistory> epHistoryList = eph.epHistoryList;

			System.Data.DataTable dt = new System.Data.DataTable();

			dt.Columns.Add("Duration", typeof(int));
			dt.Columns.Add("Rate", typeof(double));
			dt.Columns.Add("Term", typeof(string));
			dt.Columns.Add("Rate%", typeof(string));

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
				
				System.Data.DataRow r = dt.NewRow();
				r["Duration"] = i.Duration;
				r["Rate"] = i.epValue.Value;
				r["Term"] = formattedTerm;
				r["Rate%"] = formattedRate;

				dt.Rows.Add(r);
			}

			string jsonStr = JsonConvert.SerializeObject(dt, Formatting.Indented);

			return jsonStr;
		}
	}
}