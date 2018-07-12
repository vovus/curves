using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
	public class YieldCurveMgr : Dictionary<long, Dictionary<long, EntryPointHistory>>	// long-1 => ycId, long-2 => epId 
	{
		YieldCurveMgr() { }

		public YieldCurveMgr(Dictionary<long, Dictionary<long, EntryPointHistory>> data)
			: base(data)
		{ }

		// yield curve enty points on particular date
		// key == yc curve Id and date, value - set of entry points for this date (the closest one, but all are of same date)
		public Dictionary<KeyValuePair<long, DateTime>, KeyValuePair<List<EntryPoint>, List<DiscountPoint>>> ycEntryPointsByDateDic =
			new Dictionary<KeyValuePair<long, DateTime>, KeyValuePair<List<EntryPoint>, List<DiscountPoint>>>();

		public List<EntryPoint> GetEntryPoints(long ycId, DateTime date, ref long maxDay)
		{
			//DateTime? consitantDate = null;

			Dictionary<long, EntryPointHistory> eph = this[ycId];

			KeyValuePair<long, DateTime> epk = new KeyValuePair<long, DateTime>(ycId, date);
			KeyValuePair<List<EntryPoint>, List<DiscountPoint>> epv = new KeyValuePair<List<EntryPoint>, List<DiscountPoint>>();

			foreach (var ep in eph)
			{
				EntryPointHistory ych = (ep.Value);

				if ((ych.ValidDateBegin > date) || (ych.ValidDateEnd < date))
					continue;

				if (ych.Instrument is Bond && (ych.Instrument as Bond).MaturityDate <= date)	// skip if maturity date is not in future
					continue;

				HashSet<HistoricValue> vph = ych.epValueHistory;
				HistoricValue vp = vph.LastOrDefault(i => i.Date <= date);

				// TODO - do we need to check that all entry points are of the same date ?
				//if (consitantDate == null)
				//	consitantDate = vp.Date;
				//else if (vp.Date != consitantDate)
				//	continue;
				//

				// entry points
				ych.epValue = vp; // last historic matching the date
				epv.Key.Add(ych);
				//
				maxDay = Math.Max(maxDay, ych.Duration + 2);
			}
			ycEntryPointsByDateDic[epk] = epv;

			(ycEntryPointsByDateDic[epk].Key).Sort(new EntryPointCompare());	// sort by durations

			return ycEntryPointsByDateDic[epk].Key;
		}

		/// <summary>
		/// take all durations of entry points across all yield curves
		/// </summary>
		public HashSet<DateTime> GetCommonDates(DateTime referenceDate)
		{
			HashSet<DateTime> CommonDates = new HashSet<DateTime>();
			foreach (Dictionary<long, EntryPointHistory> eph in this.Values)
			{
				foreach (var ep in eph)
				{
					EntryPointHistory ych = (ep.Value);
					CommonDates.Add(referenceDate.AddDays(ych.Duration));
				}
			}
			return CommonDates;
		}

		/// <summary>
		/// common duratins (dates) of entry points across two yield curves
		/// </summary>
		/// <param name="CurveId1"></param>
		/// <param name="CurveId2"></param>
		/// <returns></returns>
		public HashSet<DateTime> GetCommonCalendarFromCachedCurves(int CurveId1, int CurveId2, DateTime referenceDate)
		{
			HashSet<DateTime> Durations = new HashSet<DateTime>();

			Dictionary<long, EntryPointHistory> eph = this[CurveId1];
			foreach (var ep in eph)
			{
				EntryPointHistory ych = (ep.Value);
				Durations.Add(referenceDate.AddDays(ych.Duration));
			}

			eph = this[CurveId2];
			foreach (var ep in eph)
			{
				EntryPointHistory ych = (ep.Value);
				Durations.Add(referenceDate.AddDays(ych.Duration));
			}
			return Durations;
		}

		public static YieldCurveMgr sYcm = new YieldCurveMgr();
	}
}
