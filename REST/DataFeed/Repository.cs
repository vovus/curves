using System;
using System.Collections.Generic;
using DataLayer;

using ConnectionContext;

namespace DataFeed
{
    public class Repository
    {
		public static Dictionary<long, Currency> ccyDic = new Dictionary<long, Currency>();
		public static Dictionary<long, YieldCurveFamily> ycFamDic = new Dictionary<long, YieldCurveFamily>();
		public static Dictionary<long, YieldCurveDefinition> ycDefDic = new Dictionary<long, YieldCurveDefinition>();
		
		//
		// DataFeed
		//

		public static List<YieldCurveDefinition> GetYieldCurveDef(long? idYc)
		{
#if _LINQXML_
			return DataHelperLinqXml.GetYieldCurveDef(idYc);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetYieldCurveDef(ctx, idYc);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
            {
                return DataHelper.GetYieldCurveData(ctx, idYc);
            }
#endif
		}

		public static List<EntryPoint> GetAllEntryPoints(bool isDemo)
		{
#if _LINQXML_
			return DataHelperLinqXml.GetAllEntryPoints();
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetAllEntryPoints(ctx, isDemo);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				return DataHelper.GetAllEntryPoints(ctx);
			}
#endif
		}

		public static void AddEntryPoint(EntryPoint ep)
		{
#if _LINQXML_
			DataHelperLinqXml.AddEntryPoint(ep);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				DataHelperSQLite.AddEntryPoint(ctx, ep);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				DataHelper.AddEntryPoint(ctx, ep);
			}
#endif
		}

		public static void UpdateEntryPoint(EntryPoint ep)
		{
#if _LINQXML_
			DataHelperLinqXml.UpdateEntryPoint(ep);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				DataHelperSQLite.UpdateEntryPoint(ctx, ep);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				DataHelper.UpdateEntryPoint(ctx, ep);
			}
#endif
		}

		public static List<EntryPoint> GetEntryPointByInstrument(Instrument instr)
		{
#if _LINQXML_
			DataHelperLinqXml.GetEntryPointByRateIdType(ref ep);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				return DataHelperSQLite.GetEntryPointByInstrument(ctx, instr);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				DataHelper.GetEntryPointByRateIdType(ctx, ref ep);
			}
#endif
		}

		public static void AddEntryPointHistory(List<EntryPoint> epl)
		{
#if _LINQXML_
			DataHelperLinqXml.AddEntryPointHistory(epl);
#elif _SQLITE_
			using (var ctx = new ConnectionContextSQLite())
			{
				DataHelperSQLite.AddEntryPointHistory(ctx, epl);
			}
#else // _MYSQL_
			using (var ctx = new ConnectionContextMySQL())
			{
				DataHelper.AddEntryPointHistory(ctx, epl);
			}
#endif
		}
    }
}
