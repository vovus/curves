//#define _MYSQL_

#if _MYSQL_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Text;

using ConnectionContext;

namespace DataLayer
{
    /// <summary>
    /// The Sql Helper.
    /// </summary>
    internal class SqlHelper
    {
        #region Methods

		internal static MySqlCommand SelectDayCounterCommand(ConnectionContextMySQL ctx, long? idDC, string className)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder("SELECT * FROM DayCounter WHERE 1 = 1");
			
			if (idDC != null)
			{
				sb.Append(" AND Id = @id");
				command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32)).Value = idDC.Value;
			}
			if (!string.IsNullOrEmpty(className))
			{
				sb.Append(" AND ClassName = @cn");
				command.Parameters.Add(new MySqlParameter("@cn", MySqlDbType.String)).Value = className;
			}

			command.CommandText = sb.ToString();
			return command;
		}

        /// <summary>
        /// The select currency command rate.
        /// </summary>
        /// <param name="ctx">
        /// The connection context
        /// </param>
        /// <param name="idCurrency">
        /// The currency id
        /// </param>
        /// <returns> MySqlCommand object
        /// </returns>
		internal static MySqlCommand SelectCurrencyCommand(ConnectionContextMySQL ctx, long? idCurrency)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
#if _NO_VIEWS_
            var sb = new StringBuilder("SELECT * FROM Currency");
#else
			var sb = new StringBuilder("SELECT * FROM vwCurrency");
#endif
            if (idCurrency != null)
            {
                sb.Append(" WHERE Id = @id");
                command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32)).Value = idCurrency.Value;
            }

            command.CommandText = sb.ToString();
            return command;
        }

        /// <summary>
        /// The select yield curve family data command.
        /// </summary>
        /// <param name="ctx">
        /// The connection context
        /// </param>
        /// <param name="idYcFamily">
        /// The yield curve family id
        /// </param>
        /// <param name="idCcy">
        /// The currency id
        /// </param>
        /// <returns> MySqlCommand object
        /// </returns>
		internal static MySqlCommand SelectYieldCurveFamilyCommand(ConnectionContextMySQL ctx, long? idYcFamily, long? idCcy)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
#if _NO_VIEWS_
			var sb = new StringBuilder("SELECT * FROM YcFamily WHERE 1 = 1");
#else
			var sb = new StringBuilder("SELECT * FROM vwYcFamily WHERE 1 = 1");
#endif
            if (idYcFamily != null)
            {
                sb.Append(" AND Id = @id");
                command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32)).Value = (int)idYcFamily;
            }
            if (idCcy != null)
            {
                sb.Append(" AND CurrencyId = @idCcy");
                command.Parameters.Add(new MySqlParameter("@idCcy", MySqlDbType.Int32)).Value = (int)idCcy;
            }

            command.CommandText = sb.ToString();
            return command;
        }

        /// <summary>
        /// The select yield curve command.
        /// </summary>
        /// <param name="ctx"> 
        /// The connection context
        /// </param>
        /// <param name="idYc"> 
        /// The YieldCurveData Id
        /// </param>
        /// <param name="idycFamily"> 
        /// The YieldCurveFamily Id
        /// </param>
        /// <returns> MySqlCommand object
        /// </returns>
		internal static MySqlCommand SelectYieldCurveDataCommand(ConnectionContextMySQL ctx, long? idYc/*, long? idYcFamily*/)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
#if _NO_VIEWS_
			var sb = new StringBuilder (
                    @"SELECT 
                        yc.Id as Id, 
						
                        CASE WHEN c.Name IS NOT NULL THEN (c.Name) ELSE ('') END as ZcCompounding, 
                        -- CASE WHEN b.Name IS NOT NULL THEN (b.Name) ELSE ('') END as ZcBasis, 
                        CASE WHEN f.Name IS NOT NULL THEN (f.Name) ELSE ('') END as ZcFrequency,
						yc.ifZC as ifZc,
                        yc.ZcColor as ZcColor,
						yc.ZcBasisId as ZcBasisId,
						yc.ForwBasisId as FrwBasisId,

                        CASE WHEN fc.Name IS NOT NULL THEN (fc.Name) ELSE ('') END as FrwCompounding, 
                        -- CASE WHEN fb.Name IS NOT NULL THEN (fb.Name) ELSE ('') END as FrwBasis, 
                        CASE WHEN ff.Name IS NOT NULL THEN (ff.Name) ELSE ('') END as FrwFrequency, 
						yc.ForwTermBase as FrwTermBase,
						yc.ForwTerm as FrwTerm,
						yc.ifForw as ifFrw,
                        yc.ForwColor as FrwColor,

						yc.SpreadType as SpreadType, 
						yc.SpreadSize as SpreadSize, 
						yc.SpreadFamily as SpreadFamily,

						yc.Name as Name,
						CASE WHEN ycf.Name IS NOT NULL THEN (ycf.Name) ELSE ('') END as Family,
						CASE WHEN ycf.CurrencyId IS NOT NULL THEN (ccy.Id) ELSE (0) END as CurrencyId,
                        CASE WHEN yc.CalendarId IS NOT NULL THEN (yc.CalendarId) ELSE (0) END as calId,
                        CASE WHEN yc.CalendarId IS NOT NULL THEN (cal.ClassName) ELSE ('') END as calClassName,
                        CASE WHEN yc.CalendarId IS NOT NULL THEN (cal.MarketName) ELSE ('') END as calMarketName

                        FROM YieldCurve yc 

                        LEFT JOIN EnumCompounding c ON c.Id = yc.ZcCompoundingId  
                        -- LEFT JOIN EnumBasis b ON b.Id = yc.ZcBasisId 
                        LEFT JOIN EnumFrequency f ON f.Id = yc.ZcFrequencyId

                        LEFT JOIN EnumCompounding fc ON fc.Id = yc.ForwCompoundingId  
                        -- LEFT JOIN EnumBasis fb ON fb.Id = yc.ForwBasisId 
                        LEFT JOIN EnumFrequency ff ON ff.Id = yc.ForwFrequencyId  

						LEFT JOIN YcFamily ycf ON ycf.Id = yc.FamilyId  
                        LEFT JOIN Currency ccy ON ccy.Id = ycf.CurrencyId  
                        LEFT JOIN Calendar cal ON cal.Id = yc.CalendarId  
                        
                        WHERE 1 = 1");
#else
            var sb = new StringBuilder("SELECT * FROM vwYieldCurve WHERE 1 = 1");
#endif
            if (idYc != null)
            {
                sb.Append(" AND Id = @idYc");
                command.Parameters.Add(new MySqlParameter("@idYc", MySqlDbType.Int32)).Value = (int)idYc;
            }
			/*
            if (idYcFamily != null)
            {
                sb.Append(" AND FamilyId = @idYcFamily");
                command.Parameters.Add(new MySqlParameter("@idYcFamily", MySqlDbType.Int32)).Value = (int)idYcFamily;
            }
			*/
            command.CommandText = sb.ToString();
            return command;
        }

        /// <summary>
        /// The select yield curve data command.
        /// </summary>
        /// <param name="ctx">
        /// The connection context
        /// </param>
        /// <param name="idYc">
        /// The yield curve id
        /// </param>
        /// <returns> MySqlCommand object
        /// </returns>
		internal static MySqlCommand SelectYieldCurveEntryDataCommand(ConnectionContextMySQL ctx, long? idYc, DateTime? settlementDate)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
#if _NO_VIEWS_
@"SELECT 
					yce.YieldCurveId as YieldCurveId,
                    yce.Id as Id, 
					CASE WHEN yce.Type IS NOT NULL THEN (yce.Type) ELSE ('') END as Type, 
                    CASE WHEN yce.Length IS NOT NULL THEN (yce.Length) ELSE (0) END as Length,
                    CASE WHEN yce.TimeUnit IS NOT NULL THEN (yce.TimeUnit) ELSE ('') END as TimeUnit,
					CASE WHEN yce.DataProviderId IS NOT NULL THEN (yce.DataProviderId) ELSE (0) END as DataProviderId,
					CASE WHEN yce.DataReference IS NOT NULL THEN (yce.DataReference) ELSE ('') END as DataReference,
				
                    yce.RateId as RateId,
                    yce.DateStart as ValidDateStart,
                    yce.DateFinish as ValidDateEnd,

                    yceh.YcEntryId as YcEntryId, 
                    yceh.Date as Date, yceh.Value as Value 

                    FROM YcEntry yce 

                    JOIN YcEntryHistory yceh ON yce.Id = yceh.YcEntryId  
                    
                    WHERE 1 = 1"
                );
#else
			@"SELECT 
                    yce.Id, yce.YieldCurveId, yce.Type, yce.Length, yce.TimeUnit, yce.RateId, 
					yceh.Date, yceh.Value 

					FROM vwYcEntryHistory yceh 
					
					JOIN vwYcEntry yce ON yceh.YcEntryId = yce.Id 
					
					WHERE 1 = 1");
#endif
            if (idYc != null)
            {
                sb.Append(" AND yce.YieldCurveId = @idYc ");
                command.Parameters.Add(new MySqlParameter("@idYc", MySqlDbType.Int32)).Value = idYc;
            }

            if (settlementDate != null)
            {
#if _NO_VIEWS_
                //String d = ((DateTime)settlementDate).ToString("yyyy-MM-dd"); 
				sb.Append(@" AND yce.DateStart <=  @date AND yce.DateFinish >  @date");
                //string app = @" AND yce.DateStart <= '"+ d + "' AND yce.DateFinish >  '" + d +"'";
                //sb.Append(app);
                
                sb.Append(@" AND yceh.Date = 
								(select max(yceh2.Date) 
									FROM YcEntryHistory yceh2 
									WHERE yceh2.Date <= @date AND yceh2.YcEntryId = yce.Id order by yceh2.Date desc)");
				
#else
                sb.Append(@" AND yceh.date = 
								(select max(yceh2.Date) 
									FROM vwYcEntryHistory yceh2 
									WHERE yceh2.Date <= @date AND yceh2.YcEntryId = yce.Id)");
#endif
				
                command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = settlementDate;
      //          command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = d;
            }

			sb.Append(" order by yce.YieldCurveId asc");

            command.CommandText = sb.ToString();
            return command;
        }


		/* -- select all data points of Yield Curve which are the most resent in respect of specified date 
		  (settlement date or date of Yield Curve)
		  
		  input parameters are:
		  123 - Yield Curve Id
		  '2011-03-15' - settlement date 
		  
		  select yce.Id, yce.YieldCurveId, yce.RateId, yce.Type, yceh.Value, max(yceh.Date)
			from ycEntry yce, YcEntryHistory yceh 
			where yce.YieldCurveId = 10 and yce.id = yceh.YcEntryId and yceh.Date <= '2012-01-20'
			group by yce.Id
		  
		 OR
		 
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

		internal static MySqlCommand SelectYieldCurveEntryDataHistoryCommand(ConnectionContextMySQL ctx,
                                                                           long? idYc,
                                                                           DateTime? settlementDate)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
            var sb = new StringBuilder(

#if _NO_VIEWS_
@"SELECT 
					yce.YieldCurveId as YieldCurveId,
                    yce.Id as Id, 
					CASE WHEN yce.Type IS NOT NULL THEN (yce.Type) ELSE ('') END as Type, 
                    CASE WHEN yce.Length IS NOT NULL THEN (yce.Length) ELSE (0) END as Length,
                    CASE WHEN yce.TimeUnit IS NOT NULL THEN (yce.TimeUnit) ELSE ('') END as TimeUnit,
					CASE WHEN yce.DataProviderId IS NOT NULL THEN (yce.DataProviderId) ELSE (0) END as DataProviderId,
					CASE WHEN yce.DataReference IS NOT NULL THEN (yce.DataReference) ELSE ('') END as DataReference,
				
                    yce.RateId as RateId,
                    yceh.YcEntryId as YcEntryId,
                    yceh.Date as Date, 
                    yceh.Value as Value,
                    yce.DateStart as ValidDateBegin,
                    yce.DateFinish as ValidDateEnd

				FROM YcEntry yce 
			    JOIN YcEntryHistory yceh ON yce.Id = yceh.YcEntryId 
				WHERE 1 = 1"
            );
#else
			@"SELECT 
					yc.Id, 
					yce.Id, yce.Type, yce.Length, yce.TimeUnit, yce.RateId, yce.DataProviderId, yce.DataReference,
					yceh.Date, yceh.Value
										
					FROM vwYcEntry yce 
					JOIN vwYieldCurve yc ON yce.YieldCurveId = yc.Id 
					
					WHERE 1 = 1");
#endif
            if (idYc != null)
            {
                sb.Append(" AND yce.YieldCurveId = @idYc ");
                command.Parameters.Add(new MySqlParameter("@idYc", MySqlDbType.Int32)).Value = idYc;
            }

            if (settlementDate != null)
            {
#if _NO_VIEWS_
                //String d = ((DateTime)settlementDate).ToString("yyyy-MM-dd"); 
				sb.Append(@" AND yce.DateStart <=  @date AND yce.DateFinish >  @date");
               // sb.Append(@" AND yce.DateStart <= d AND yce.DateFinish > d");

                //string app = @" AND yce.DateStart <= '" + d + "' AND yce.DateFinish >  '" + d + "'";
                //sb.Append(app);
               
                sb.Append(@" AND yceh.Date <= @date order by yceh.Date desc");
#else
                sb.Append(@" AND yceh.date = 
								(select max(yceh2.Date) 
									FROM vwYcEntryHistory yceh2 
									WHERE yceh2.Date <= @date AND yceh2.YcEntryId = yce.Id)");
#endif
				command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = settlementDate;
                //command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = d;
            }

			sb.Append(" order by yce.YieldCurveId asc, yceh.Date desc");

            command.CommandText = sb.ToString();
            return command;
        }

#region --------------------All rates command------------------

		internal static MySqlCommand SelectRatesCommand(ConnectionContextMySQL ctx, long? idRate)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);

            var sb = new StringBuilder(

#if _NO_VIEWS_
			@"SELECT 
				r.Id as Id, 
				r.Name as Name, 
				r.ClassName as ClassName, 
				r.Type as Type, 
				r.DataProviderId as DataProviderId, 
				r.DataReference as DataReference, 
				r.CurrencyId as CurrencyId, 
				r.Length as Length, 
				r.TimeUnit as TimeUnit, 
				r.Accuracy as Accuracy, 
				r.Spread as Spread, 
				r.SettlementDays as SettlementDays, 
				r.YieldCurveId as YieldCurveId, 
				r.FixingPlace as FixingPlace, 
				r.IndexId as IndexId,   
				r.BasisId as BasisId,
				-- CASE WHEN b.Name IS NOT NULL THEN (b.Name) ELSE ('') END as Basis, 
				CASE WHEN c.Name IS NOT NULL THEN (c.Name) ELSE ('') END as Compounding,   
				CASE WHEN f.Name IS NOT NULL THEN (f.Name) ELSE ('') END as Frequency,
				CASE WHEN r.IndexId IS NOT NULL THEN r1.Length ELSE (0) END as IndexLength,
				CASE WHEN r.IndexId IS NOT NULL THEN r1.TimeUnit ELSE ('') END as IndexTimeUnit, 
				CASE WHEN r.IndexId IS NOT NULL THEN r1.Name ELSE ('') END as IndexName,
				CASE WHEN r.IndexId IS NOT NULL THEN r1.ClassName ELSE ('') END as IndexClassName,
				-- CASE WHEN b1.Name IS NOT NULL THEN (b1.Name) ELSE ('') END as IndexBasis, 
				r1.BasisId as IndexBasisId,
				CASE WHEN c1.Name IS NOT NULL THEN (c1.Name) ELSE ('') END as IndexCompounding,   
				CASE WHEN f1.Name IS NOT NULL THEN (f1.Name) ELSE ('') END as IndexFrequency
				FROM rate r
				LEFT JOIN EnumCompounding c ON c.Id = r.CompoundingId 
				-- LEFT JOIN EnumBasis b ON b.Id = r.BasisId  
				LEFT JOIN EnumFrequency f ON f.Id = r.FrequencyId
				LEFT JOIN rate r1 ON r1.Id = r.IndexId
				LEFT JOIN EnumCompounding c1 ON c1.Id = r1.CompoundingId 
				-- LEFT JOIN EnumBasis b1 ON b1.Id = r1.BasisId  
				LEFT JOIN EnumFrequency f1 ON f1.Id = r1.FrequencyId
				WHERE 1 = 1"
			);
            
#else
			@"SELECT r.Id, r.Name, r.ClassName, r.Type, r.DataProviderId, r.DataReference, r.CurrencyId, r.Length, r.TimeUnit, 
                r.Accuracy, r.Spread, r.SettlementDays, r.YieldCurveId, r.FixingPlace, r.IndexId, r.Basis, r.Frequency, r.Compounding, 
            CASE WHEN r.IndexId IS NOT NULL THEN r1.Length ELSE (null) END as IndexLength,
            CASE WHEN r.IndexId IS NOT NULL THEN r1.TimeUnit ELSE (null) END as IndexTimeUnit, 
            CASE WHEN r.IndexId IS NOT NULL THEN r1.Name ELSE (null) END as IndexName,
            CASE WHEN r.IndexId IS NOT NULL THEN r1.ClassName ELSE (null) END as IndexClassName,
            CASE WHEN r.IndexId IS NOT NULL THEN r1.Basis ELSE (null) END as IndexBasis, 
            CASE WHEN r.IndexId IS NOT NULL THEN r1.Frequency ELSE (null) END as IndexFrequency,
            CASE WHEN r.IndexId IS NOT NULL THEN r1.Compounding ELSE (null) END as IndexCompounding
            FROM vwrate r
            Left JOIN vwrate r1 ON r1.Id = r.IndexId  
            WHERE 1 = 1");
#endif
            if (idRate != null)
            {
                sb.Append(" AND r.Id = @idRate ");
                command.Parameters.Add(new MySqlParameter("@idRate", MySqlDbType.Int32)).Value = idRate;
            }

            command.CommandText = sb.ToString();
            return command;
        }

#endregion----------------------------------------------------



#region ------------------  All bonds command ------------------------
		internal static MySqlCommand SelectBondsCommand(ConnectionContextMySQL ctx, long? idBond)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
            var sb = new StringBuilder(               
#if _NO_VIEWS_
			@"SELECT 
				b.Id, 
				b.Name, 
				b.Type, 
				b.DataProviderId, 
				b.DataReference, 
				b.CurrencyId,  
				-- b.DateGenerationId, 
				-- b.CouponFrequencyId, 
				b.Coupon, 
				-- b.CouponConventionId, 
				b.CouponBasisId, 
				b.Redemption, 
				b.Nominal,
				-- b.CalendarId, 
				b.SettlementDays, 
				b.IssueDate, 
				b.MaturityDate,
				-- b.YieldCurveId, 
				-- b.UserCreated, 
				-- b.IpCreated, 
				CASE WHEN c.Name IS NOT NULL THEN (c.Name) ELSE ('') END as DateGeneration,
				CASE WHEN d.Name IS NOT NULL THEN (d.Name) ELSE ('') END as CouponFrequency, 
				CASE WHEN e.Name IS NOT NULL THEN (e.Name) ELSE ('') END as CouponConvention 
				-- CASE WHEN f.Name IS NOT NULL THEN (f.Name) ELSE ('') END as CouponBasis 
				FROM bond b
				LEFT JOIN EnumDateGeneration c ON c.Id = b.DateGenerationId 
				LEFT JOIN EnumFrequency d ON d.Id = b.CouponFrequencyId
				LEFT JOIN EnumBusinessDayConvention e ON e.Id = b.CouponConventionId 
				-- LEFT JOIN EnumBasis f ON f.Id = b.CouponBasisId  
				WHERE 1 = 1"
			);

#else
			@"SELECT * FROM vwbond");
#endif
            if (idBond != null)
            {
                sb.Append(" AND b.Id = @idBond ");
                command.Parameters.Add(new MySqlParameter("@idBond", MySqlDbType.Int32)).Value = idBond;
            }

            command.CommandText = sb.ToString();
            return command;
        }
#endregion  ---------------------------------------------------------------------------



#region ------------------- All calendars command ---------------------
		internal static MySqlCommand SelectCalendarsCommand(ConnectionContextMySQL ctx)
        {
            var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
            var sb = new StringBuilder(

#if _NO_VIEWS_
			@"SELECT 
				m.Id, 
				m.Name, 
				m.ClassName, 
				m.MarketName   
			FROM calendar m
			WHERE 1 = 1");

#else
			@"SELECT m.Id, m.Name, m.ClassName, m.MarketName   
    FROM calendar m
    WHERE 1 = 1");
#endif

            command.CommandText = sb.ToString();
            return command;
        }
        #endregion

		#region --------------------All ExchangeRates command------------------

		internal static MySqlCommand SelectXRatesCommand(ConnectionContextMySQL ctx, long? idXRate)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);

			var sb = new StringBuilder(
#if _NO_VIEWS_
			@"SELECT 
				x.Id as Id, 
				x.Name as Name, 
				x.ClassName as ClassName, 
				x.DataProviderId as DataProviderId, 
				x.DataReference as DataReference, 
				x.SettlementDays as SettlementDays, 
				x.FixingPlace as FixingPlace,
                x.PrimaryCurrency as PrimaryCurrencyCode,
                x.SecondaryCurrency as SecondaryCurrencyCode
                FROM ExchangeRate x
				WHERE 1 = 1"
			);

#else
			x.Id as Id, 
				x.Name as Name, 
				x.ClassName as ClassName, 
				x.DataProviderId as DataProviderId, 
				x.DataReference as DataReference, 
				x.SettlementDays as SettlementDays, 
				x.FixingPlace as FixingPlace,
                x.PrimaryCurrency as PrimaryCurrencyCode,
                x.SecondaryCurrency as SecondaryCurrencyCode
				FROM vwExchangeRate x
				WHERE 1 = 1");
#endif
			if (idXRate != null)
			{
				sb.Append(" AND r.Id = @idXRate ");
				command.Parameters.Add(new MySqlParameter("@idXRate", MySqlDbType.Int32)).Value = idXRate;
			}

			command.CommandText = sb.ToString();
			return command;
		}

		#endregion----------------------------------------------------


		#region -----------------------ExchangeRateHistory---------------------------

		internal static MySqlCommand SelectExchangeRateHistoryCommand(ConnectionContextMySQL ctx,
																	  long? RateId,
																	  DateTime? settlementDate)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(

#if _NO_VIEWS_
			@"SELECT 
				xh.Id as Id,
                xh.RateId as RateId,
                xh.Date as Date, 
                xh.Close as Value, 
                CASE WHEN xe.DataProviderId IS NOT NULL THEN (xe.DataProviderId) ELSE (0) END as DataProviderId,
				CASE WHEN xe.DataReference IS NOT NULL THEN (xe.DataReference) ELSE ('') END as DataReference
				FROM exchangeratehistory xh 
                JOIN exchangerate xe ON xe.Id = xh.RateId 
			    WHERE 1 = 1"
			);
#else
			@"SELECT 
                    xh.Id as Id,
					xh.RateId as RateId,
                    xh.Date as Date, 
                    xh.Close as Value,
                    CASE WHEN xe.DataProviderId IS NOT NULL THEN (xe.DataProviderId) ELSE (0) END as DataProviderId,
					CASE WHEN xe.DataReference IS NOT NULL THEN (xe.DataReference) ELSE ('') END as DataReference
				FROM vwexchangeratehistory xh 
                JOIN vwexchangerate xe ON xe.Id = xh.RateId 
			    WHERE 1 = 1");
#endif
			if (RateId != null)
			{
				sb.Append(" AND xh.RateId = @RateId ");
				command.Parameters.Add(new MySqlParameter("@RateId", MySqlDbType.Int32)).Value = RateId;
			}

			if (settlementDate != null)
			{
#if _NO_VIEWS_
				sb.Append(@" AND xh.Date <= @date order by xh.Date desc");
#else
                sb.Append(@" AND xh.date = 
								(select max(xh2.Date) 
									FROM vwexchangeratehistory xh2 
									WHERE xh2.Date <= @date AND xh2.RateId = xh.Id)");
#endif
				command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = settlementDate;
			}

			sb.Append(" order by xh.RateId asc, xh.Date desc");

			command.CommandText = sb.ToString();
			return command;
		}

		#endregion

        #endregion

		#region INFLATION CURVE

		internal static MySqlCommand SelectInflationCurveFamilyCommand(ConnectionContextMySQL ctx, long? idInflationFamily, long? idCcy)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
#if _NO_VIEWS_
			var sb = new StringBuilder("SELECT * FROM InflationFamily WHERE 1 = 1");
#else
			var sb = new StringBuilder("SELECT * FROM vwInflationFamily WHERE 1 = 1");
#endif
			if (idInflationFamily != null)
			{
				sb.Append(" AND Id = @id");
				command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32)).Value = idInflationFamily;
			}
			if (idCcy != null)
			{
				sb.Append(" AND CurrencyId = @idCcy");
				command.Parameters.Add(new MySqlParameter("@idCcy", MySqlDbType.Int32)).Value = idCcy;
			}

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand SelectInflationCurveDataCommand(ConnectionContextMySQL ctx, long? idIC/*, long? idYcFamily*/)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
#if _NO_VIEWS_
			var sb = new StringBuilder(
					@"SELECT 
                        yc.Id as Id, 
						
                        CASE WHEN c.Name IS NOT NULL THEN (c.Name) ELSE ('') END as ZcCompounding, 
                        CASE WHEN f.Name IS NOT NULL THEN (f.Name) ELSE ('') END as ZcFrequency,
						yc.ifZC as ifZc,
                        yc.ZcColor as ZcColor,
						yc.ZcBasisId as ZcBasisId,
                        CASE WHEN yc.InflationIndex IS NOT NULL THEN (yc.InflationIndex) ELSE (0) END as InflationIndex,
						yc.ifIndex as ifIndex,
                        yc.IndexColor as IndexColor,

						yc.Name as Name,
						CASE WHEN ycf.Name IS NOT NULL THEN (ycf.Name) ELSE ('') END as Family,
						CASE WHEN ycf.CurrencyId IS NOT NULL THEN (ccy.Id) ELSE (0) END as CurrencyId
                       
                        FROM InflationCurve yc 

                        LEFT JOIN EnumCompounding c ON c.Id = yc.ZcCompoundingId  
                        LEFT JOIN EnumFrequency f ON f.Id = yc.ZcFrequencyId

                       	LEFT JOIN InflationFamily ycf ON ycf.Id = yc.FamilyId  
                        LEFT JOIN Currency ccy ON ccy.Id = ycf.CurrencyId  
                                               
                        WHERE 1 = 1");
#else
            var sb = new StringBuilder("SELECT * FROM vwInflationCurve WHERE 1 = 1");
#endif
			if (idIC != null)
			{
				sb.Append(" AND Id = @idIC");
				command.Parameters.Add(new MySqlParameter("@idIC", MySqlDbType.Int32)).Value = idIC;
			}
			/*
			if (idYcFamily != null)
			{
				sb.Append(" AND FamilyId = @idYcFamily");
				command.Parameters.Add(new MySqlParameter("@idYcFamily", MySqlDbType.Int32)).Value = (int)idYcFamily;
			}
			*/
			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand SelectInflationCurveEntryDataCommand(ConnectionContextMySQL ctx, long? idInflationCurve, DateTime? settlementDate)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
#if _NO_VIEWS_
@"SELECT 
					ice.InflationCurveId as InflationCurveId,
                    ice.Id as Id, 
					CASE WHEN ice.Type IS NOT NULL THEN (ice.Type) ELSE ('') END as Type, 
                    ice.RateId as RateId,
                    ice.DateStart as ValidDateStart,
                    ice.DateFinish as ValidDateEnd
                    
                    FROM InflationcurveEntry ice 
                    WHERE 1 = 1"
				);
#else
			@"SELECT 
                    yce.Id, yce.YieldCurveId, yce.Type, yce.Length, yce.TimeUnit, yce.RateId, 
					yceh.Date, yceh.Value 

					FROM vwYcEntryHistory yceh 
					
					JOIN vwYcEntry yce ON yceh.YcEntryId = yce.Id 
					
					WHERE 1 = 1");
#endif
			if (idInflationCurve != null)
			{
				sb.Append(" AND ice.InflationCurveId = @idInflationCurve ");
				command.Parameters.Add(new MySqlParameter("@idInflationCurve", MySqlDbType.Int32)).Value = idInflationCurve;
			}

			if (settlementDate != null)
			{
#if _NO_VIEWS_
				//String d = ((DateTime)settlementDate).ToString("yyyy-MM-dd");
				sb.Append(@" AND yce.DateStart <=  @date AND yce.DateFinish >  @date");
				//string app = @" AND ice.DateStart <= '" + d + "' AND ice.DateFinish >  '" + d + "'";
				//sb.Append(app);


#else
                
#endif
				command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = settlementDate;
				//          command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = d;
			}

			sb.Append(" order by ice.InflationCurveId asc");

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand SelectInflationBondsCommand(ConnectionContextMySQL ctx, long? idBond)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
#if _NO_VIEWS_
@"SELECT 
				b.Id, 
				b.Name, 
                b.InflationIndexId,
				b.Type, 
				b.DataProviderId, 
				b.DataReference, 
				b.CurrencyId,  
				b.Coupon, 
				b.CouponBasisId, 
				b.Redemption, 
				b.Nominal,
				b.SettlementDays, 
				b.IssueDate, 
				b.MaturityDate,
				CASE WHEN c.Name IS NOT NULL THEN (c.Name) ELSE ('') END as DateGeneration,
				CASE WHEN d.Name IS NOT NULL THEN (d.Name) ELSE ('') END as CouponFrequency, 
				CASE WHEN e.Name IS NOT NULL THEN (e.Name) ELSE ('') END as CouponConvention 
				FROM inflationbond b
				LEFT JOIN EnumDateGeneration c ON c.Id = b.DateGenerationId 
				LEFT JOIN EnumFrequency d ON d.Id = b.CouponFrequencyId
				LEFT JOIN EnumBusinessDayConvention e ON e.Id = b.CouponConventionId 
				WHERE 1 = 1"
			);

#else
			@"SELECT * FROM vwinflationbond");
#endif
			if (idBond != null)
			{
				sb.Append(" AND b.Id = @idBond ");
				command.Parameters.Add(new MySqlParameter("@idBond", MySqlDbType.Int32)).Value = idBond;
			}

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand SelectInflationIndexCommand(ConnectionContextMySQL ctx, long? idIndex)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
#if _NO_VIEWS_
@"SELECT 
				b.Id, 
				b.Name, 
                b.ClassName,
				b.Type, 
				b.DataProviderId, 
				b.DataReference, 
				b.CurrencyId
				FROM inflationIndex b
				WHERE 1 = 1"
			);

#else
			@"SELECT * FROM vwinflationindex");
#endif
			if (idIndex != null)
			{
				sb.Append(" AND b.Id = @idIndex ");
				command.Parameters.Add(new MySqlParameter("@idIndex", MySqlDbType.Int32)).Value = idIndex;
			}

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand SelectInflationRatesCommand(ConnectionContextMySQL ctx, long? idRate)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);

			var sb = new StringBuilder(

#if _NO_VIEWS_
@"SELECT 
				r.Id as Id,
                r.InflationCurveId as InflationCurveId, 
				r.Name as Name, 
				r.ClassName as ClassName, 
				r.Type as Type, 
                r.CurrencyId as CurrencyId, 
                r.InflationIndexId as InflationIndexId, 
                r.Length as Length, 
				r.TimeUnit as TimeUnit, 
				r.Accuracy as Accuracy, 
                r.BasisId as BasisId,
                r.FrequencyId as FrequencyId,
                r.CompoundingId as CompoundingId,
                r.Spread as Spread,
                r.SettlementLag as SettlementDays,
                r.InflationLag as InflationLag,
                r.InflationLagTimeUnit as InflationLagTimeUnit,
				r.DataProviderId as DataProviderId, 
				r.DataReference as DataReference, 
				CASE WHEN c.Name IS NOT NULL THEN (c.Name) ELSE ('') END as Compounding,   
				CASE WHEN f.Name IS NOT NULL THEN (f.Name) ELSE ('') END as Frequency,
				CASE WHEN r.InflationIndexId IS NOT NULL THEN r1.Name ELSE ('') END as IndexName,
				CASE WHEN r.InflationIndexId IS NOT NULL THEN r1.ClassName ELSE ('') END as IndexClassName
				FROM inflationrate r
				LEFT JOIN EnumCompounding c ON c.Id = r.CompoundingId 
				LEFT JOIN EnumFrequency f ON f.Id = r.FrequencyId
				LEFT JOIN inflationindex r1 ON r1.Id = r.InflationIndexId
				WHERE 1 = 1"
			);

#else
		//	@"SELECT r.Id, r.Name, r.ClassName, r.Type, r.DataProviderId, r.DataReference, r.CurrencyId, r.Length, r.TimeUnit, 
        //        r.Accuracy, r.Spread, r.SettlementDays, r.YieldCurveId, r.FixingPlace, r.IndexId, r.Basis, r.Frequency, r.Compounding, 
        //    CASE WHEN r.IndexId IS NOT NULL THEN r1.Length ELSE (null) END as IndexLength,
        //    CASE WHEN r.IndexId IS NOT NULL THEN r1.TimeUnit ELSE (null) END as IndexTimeUnit, 
        //    CASE WHEN r.IndexId IS NOT NULL THEN r1.Name ELSE (null) END as IndexName,
        //    CASE WHEN r.IndexId IS NOT NULL THEN r1.ClassName ELSE (null) END as IndexClassName,
        //    CASE WHEN r.IndexId IS NOT NULL THEN r1.Basis ELSE (null) END as IndexBasis, 
        //    CASE WHEN r.IndexId IS NOT NULL THEN r1.Frequency ELSE (null) END as IndexFrequency,
        //    CASE WHEN r.IndexId IS NOT NULL THEN r1.Compounding ELSE (null) END as IndexCompounding
        //    FROM vwrate r
        //    Left JOIN vwrate r1 ON r1.Id = r.IndexId  
        //    WHERE 1 = 1");
#endif
			if (idRate != null)
			{
				sb.Append(" AND r.Id = @idRate ");
				command.Parameters.Add(new MySqlParameter("@idRate", MySqlDbType.Int32)).Value = idRate;
			}

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand SelectRateHistoryCommand(ConnectionContextMySQL ctx,
																	  long? RateId,
																	  DateTime? settlementDate)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(

#if _NO_VIEWS_
@"SELECT 
				xh.Id as Id,
                xh.Type as Type,
                xh.RateId as RateId,
                xh.Date as Date,
                xh.High as High, 
                xh.Low as Low,
                xh.Open as Open,  
                xh.Close as Close, 
                xh.Theoretical as Theoretical, 
                xh.UserCreated as UserCreated, 
                xh.IpCreated as IpCreated 
                FROM ratehistory xh 
                WHERE 1 = 1"
			);
#else
			@"SELECT 
                    xh.Id as Id,
                    xh.Type as Type,
					xh.RateId as RateId,
                    xh.Date as Date, 
                    xh.Close as Value,
     			FROM vwratehistory xh 
                WHERE 1 = 1");
#endif
			if (RateId != null)
			{
				sb.Append(" AND xh.RateId = @RateId ");
				command.Parameters.Add(new MySqlParameter("@RateId", MySqlDbType.Int32)).Value = RateId;
			}

			if (settlementDate != null)
			{
#if _NO_VIEWS_
				sb.Append(@" AND xh.Date <= @date order by xh.Date desc");
#else
                sb.Append(@" AND xh.date = 
								(select max(xh2.Date) 
									FROM vwratehistory xh2 
									WHERE xh2.Date <= @date AND xh2.RateId = xh.Id)");
#endif
				command.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date)).Value = settlementDate;
			}

			sb.Append(" order by xh.RateId asc, xh.Date desc");

			command.CommandText = sb.ToString();
			return command;
		}

		#endregion
    }
}

#endif // _MYSQL_