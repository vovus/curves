#define _SQLITE_

#if _SQLITE_

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using DataLayer;
using ConnectionContext;

namespace DataFeed
{
	internal class SqlHelperSQLite
	{
		//
		// DateFeed
		//

		internal static SQLiteCommand SelectYieldCurveDefCommand(ConnectionContextSQLite ctx, long? idYc/*, long? idYcFamily*/)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);
#if _NO_VIEWS_
			var sb = new StringBuilder(
					@"SELECT 
                        yc.Id as Id, 
						
                        CASE WHEN c.Name IS NOT NULL THEN (c.Name) ELSE ('') END as ZcCompounding, 
                        -- CASE WHEN b.Name IS NOT NULL THEN (b.Name) ELSE ('') END as ZcBasis, 
                        CASE WHEN f.Name IS NOT NULL THEN (f.Name) ELSE ('') END as ZcFrequency,
						yc.ifZC as ifZc,
                        yc.ZcColor as ZcColor,
						yc.ZcBasisId as ZcBasisId,
						yc.ForwBasisId as FrwBasisId,
						yc.FamilyId as FamilyId,

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
						-- CASE WHEN ycf.Name IS NOT NULL THEN (ycf.Name) ELSE ('') END as Family,
						CASE WHEN ycf.CurrencyId IS NOT NULL THEN (ccy.Id) ELSE (0) END as CurrencyId,
                        CASE WHEN yc.CalendarId IS NOT NULL THEN (yc.CalendarId) ELSE ('') END as calId,
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
                        
                        WHERE 1 = 1 AND yc.UserCreated = @user");

			// 1 == Demo User
			SQLiteParameter p = new SQLiteParameter("@user", DbType.Int32);
			p.Value = 1; //TODO (ctx.user == null ? 1 : ctx.user.id);
			command.Parameters.Add(p);
#else
            var sb = new StringBuilder("SELECT * FROM vwYieldCurve WHERE 1 = 1");
#endif
			if (idYc != null)
			{
				sb.Append(" AND Id = @idYc");
				p = new SQLiteParameter("@idYc", DbType.Int32);
				p.Value = (int)idYc;
				command.Parameters.Add(p);
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

		internal static SQLiteCommand GetAllEntryPoints(ConnectionContextSQLite ctx, bool isDemo)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
			@"SELECT  
				Id,
				Type,
				RateId, 
				YieldCurveId,
				Length,
				TimeUnit,
				DateStart,
				DateFinish,
				DataProviderId,
				DataReference,
				UserCreated,
				IpCreated

			FROM YcEntry 
			
			WHERE 1 == 1 AND UserCreated = @user"
			);

			// 1 == Demo User
			SQLiteParameter p = new SQLiteParameter("@user", DbType.Int32);
			p.Value = (isDemo || ctx.user == null ? 1 : ctx.user.id);
			command.Parameters.Add(p);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static SQLiteCommand AddEntryPoint(ConnectionContextSQLite ctx, EntryPoint ep)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(

			@"INSERT into YcEntry (
				Id,
				Type,
				RateId, 
				YieldCurveId,
				Length,
				TimeUnit,
				DateStart,
				DateFinish,
				DataProviderId,
				DataReference,
				UserCreated,
				IpCreated )

			VALUES (
				@Id,
				@Type,
				@RateId, 
				@YieldCurveId,
				@Length,
				@TimeUnit,
				@DateStart,
				@DateFinish,
				@DataProviderId,
				@DataReference,
				@user,
				@IpCreated
				)"
			);

			SQLiteParameter p = new SQLiteParameter("@Id", DbType.Int32);
			p.Value = ep.Id;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@Type", DbType.String);
			p.Value = ep.Type;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@RateId", DbType.Int32);
			p.Value = ep.Instrument.Id;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@YieldCurveId", DbType.Int32);
			p.Value = ep.YieldCurveId;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@Length", DbType.Int32);
			p.Value = ep.Length;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@TimeUnit", DbType.String);
			p.Value = ep.TimeUnit;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DateStart", DbType.DateTime);
			p.Value = ep.ValidDateBegin;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DateEnd", DbType.DateTime);
			p.Value = ep.ValidDateEnd;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DataProviderId", DbType.Int32);
			p.Value = ep.DataProviderId;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DataReference", DbType.String);
			p.Value = ep.DataReference;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@IpCreated", DbType.String);
			p.Value = null;
			command.Parameters.Add(p);

			// 1 == Demo User
			p = new SQLiteParameter("@user", DbType.Int32);
			p.Value = (ctx.user == null ? 1 : ctx.user.id);
			command.Parameters.Add(p);
			
			command.CommandText = sb.ToString();
			return command;
		}

		internal static SQLiteCommand UpdateEntryPoint(ConnectionContextSQLite ctx, EntryPoint ep)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(

			@"UPDATE  YcEntry SET 
				Id = @Id,
				Type = @Type,
				RateId = @RateId, 
				YieldCurveId = @YieldCurveId,
				Length = @Length,
				TimeUnit = @TimeUnit,
				DateStart = @DateStart,
				DateFinish = @DateFinish,
				DataProviderId = @DataProviderId,
				DataReference = @DataReference,
				IpCreated = @IpCreated

				WHERE 1 == 1 AND UserCreated = @user"
			);

			// 1 == Demo User
			SQLiteParameter p = new SQLiteParameter("@user", DbType.Int32);
			p.Value = (ctx.user == null ? 1 : ctx.user.id);
			command.Parameters.Add(p);
			
			p = new SQLiteParameter("@Id", DbType.Int32);
			p.Value = ep.Id;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@Type", DbType.String);
			p.Value = ep.Type;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@RateId", DbType.Int32);
			p.Value = ep.Instrument.Id;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@YieldCurveId", DbType.Int32);
			p.Value = ep.YieldCurveId;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@Length", DbType.Int32);
			p.Value = ep.Length;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@TimeUnit", DbType.String);
			p.Value = ep.TimeUnit;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DateStart", DbType.DateTime);
			p.Value = ep.ValidDateBegin;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DateEnd", DbType.DateTime);
			p.Value = ep.ValidDateEnd;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DataProviderId", DbType.Int32);
			p.Value = ep.DataProviderId;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@DataReference", DbType.String);
			p.Value = ep.DataReference;
			command.Parameters.Add(p);
			p = new SQLiteParameter("@IpCreated", DbType.String);
			p.Value = null;
			command.Parameters.Add(p);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static SQLiteCommand GetEntryPointByInstrument(ConnectionContextSQLite ctx, Instrument instr)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
			@"SELECT  
				Id,
				Type,
				RateId,
				YieldCurveId,
				Length,
				TimeUnit,
				DateStart,
				DateFinish,
				DataProviderId,
				DataReference

				FROM YcEntry 

				WHERE 1 == 1 AND UserCreated = @user"
			);

			// 1 == Demo User
			SQLiteParameter p = new SQLiteParameter("@user", DbType.Int32);
			p.Value = (ctx.user == null ? 1 : ctx.user.id);
			command.Parameters.Add(p);

			if (instr != null)	// otherwise return ALL entry point's ids
			{
				sb.Append(@" AND RateId = @rateId AND Type = @type");
				p = new SQLiteParameter("@rateId", DbType.Int32);
				p.Value = instr.Id;
				command.Parameters.Add(p);

				p = new SQLiteParameter("@type", DbType.String);
				p.Value = instr.Type;
				command.Parameters.Add(p);
			}

			command.CommandText = sb.ToString();
			return command;
		}

		internal static SQLiteCommand AddEntryPointHistory(ConnectionContextSQLite ctx, EntryPoint ep)
		{
			var command = new SQLiteCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
			@"INSERT 
				into YcEntryHistory 
				(YcEntryId, Value, Date, UserCreated, IpCreated)
				values
				(@epId, @epVal, @epDate, @user, '127.0.0.1')"
			);

			// 1 == Demo User
			SQLiteParameter p = new SQLiteParameter("@user", DbType.Int32);
			p.Value = (ctx.user == null ? 1 : ctx.user.id);
			command.Parameters.Add(p);

			p = new SQLiteParameter("@epId", DbType.Int32);
			p.Value = ep.Id;
			command.Parameters.Add(p);

			p = new SQLiteParameter("@epVal", DbType.Double);
			p.Value = ep.epValue.Value;
			command.Parameters.Add(p);

			p = new SQLiteParameter("@epDate", DbType.DateTime);
			p.Value = ep.epValue.Date;
			command.Parameters.Add(p);

			command.CommandText = sb.ToString();
			return command;
		}
	}
}

#endif // _SQLITE_