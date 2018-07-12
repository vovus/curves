//#define _MYSQL_

#if _MYSQL_

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Text;

using DataLayer;
using ConnectionContext;

namespace DataFeed
{
    /// <summary>
    /// The Sql Helper.
    /// </summary>
    internal class SqlHelper
    {
        //
		// DataFeed
		//

		internal static MySqlCommand GetAllEntryPoints(ConnectionContextMySQL ctx)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
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
				WHERE 1 == 1"
			);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand AddEntryPoint(ConnectionContextMySQL ctx, EntryPoint ep)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
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
				@UserCreated,
				@IpCreated
				)"
			);

			MySqlParameter p = new MySqlParameter("@Id", MySqlDbType.Int32);
			p.Value = ep.Id;
			command.Parameters.Add(p);
			p = new MySqlParameter("@Type", MySqlDbType.String);
			p.Value = ep.Type;
			command.Parameters.Add(p);
			p = new MySqlParameter("@RateId", MySqlDbType.Int32);
			p.Value = ep.Instrument.Id;
			command.Parameters.Add(p);
			p = new MySqlParameter("@YieldCurveId", MySqlDbType.Int32);
			p.Value = ep.YieldCurveId;
			command.Parameters.Add(p);
			p = new MySqlParameter("@Length", MySqlDbType.Int32);
			p.Value = ep.Length;
			command.Parameters.Add(p);
			p = new MySqlParameter("@TimeUnit", MySqlDbType.String);
			p.Value = ep.TimeUnit;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DateStart", MySqlDbType.DateTime);
			p.Value = ep.ValidDateBegin;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DateEnd", MySqlDbType.DateTime);
			p.Value = ep.ValidDateEnd;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DataProviderId", MySqlDbType.Int32);
			p.Value = ep.DataProviderId;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DataReference", MySqlDbType.String);
			p.Value = ep.DataReference;
			command.Parameters.Add(p);
			p = new MySqlParameter("@UserCreated", MySqlDbType.String);
			p.Value = null;
			command.Parameters.Add(p);
			p = new MySqlParameter("@IpCreated", MySqlDbType.String);
			p.Value = null;
			command.Parameters.Add(p);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand UpdateEntryPoint(ConnectionContextMySQL ctx, EntryPoint ep)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
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
				UserCreated = @UserCreated,
				IpCreated = @IpCreated

				WHERE 1 == 1"
			);

			MySqlParameter p = new MySqlParameter("@Id", MySqlDbType.Int32);
			p.Value = ep.Id;
			command.Parameters.Add(p);
			p = new MySqlParameter("@Type", MySqlDbType.String);
			p.Value = ep.Type;
			command.Parameters.Add(p);
			p = new MySqlParameter("@RateId", MySqlDbType.Int32);
			p.Value = ep.Instrument.Id;
			command.Parameters.Add(p);
			p = new MySqlParameter("@YieldCurveId", MySqlDbType.Int32);
			p.Value = ep.YieldCurveId;
			command.Parameters.Add(p);
			p = new MySqlParameter("@Length", MySqlDbType.Int32);
			p.Value = ep.Length;
			command.Parameters.Add(p);
			p = new MySqlParameter("@TimeUnit", MySqlDbType.String);
			p.Value = ep.TimeUnit;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DateStart", MySqlDbType.DateTime);
			p.Value = ep.ValidDateBegin;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DateEnd", MySqlDbType.DateTime);
			p.Value = ep.ValidDateEnd;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DataProviderId", MySqlDbType.Int32);
			p.Value = ep.DataProviderId;
			command.Parameters.Add(p);
			p = new MySqlParameter("@DataReference", MySqlDbType.String);
			p.Value = ep.DataReference;
			command.Parameters.Add(p);
			p = new MySqlParameter("@UserCreated", MySqlDbType.String);
			p.Value = null;
			command.Parameters.Add(p);
			p = new MySqlParameter("@IpCreated", MySqlDbType.String);
			p.Value = null;
			command.Parameters.Add(p);

			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand GetEntryPointByRateIdType(ConnectionContextMySQL ctx, EntryPoint ep)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
@"SELECT  
				Id,
				Type
				FROM YcEntry 
				WHERE 1 == 1"
			);

			if (ep != null)	// otherwise return ALL entry point's ids
			{
				sb.Append(@" RateId = @rateId AND Type = @type");
				MySqlParameter p = new MySqlParameter("@rateId", MySqlDbType.Int32);
				p.Value = ep.Instrument.Id;
				command.Parameters.Add(p);

				p = new MySqlParameter("@type", MySqlDbType.String);
				p.Value = ep.Type;
				command.Parameters.Add(p);
			}
			command.CommandText = sb.ToString();
			return command;
		}

		internal static MySqlCommand AddEntryPointHistory(ConnectionContextMySQL ctx, EntryPoint ep)
		{
			var command = new MySqlCommand(null, ctx.connection, ctx.transaction);
			var sb = new StringBuilder(
@"INSERT 
				into YcEntryHistory 
				(YcEntryId, Value, Date)
				values
				(@epId, @epVal, @epDate)"
			);

			MySqlParameter p = new MySqlParameter("@epId", MySqlDbType.Int32);
			p.Value = ep.Id;
			command.Parameters.Add(p);

			p = new MySqlParameter("@epVal", MySqlDbType.Double);
			p.Value = ep.epValue.Value;
			command.Parameters.Add(p);

			p = new MySqlParameter("@epDate", MySqlDbType.DateTime);
			p.Value = ep.epValue.Date;
			command.Parameters.Add(p);

			command.CommandText = sb.ToString();
			return command;
		}
    }
}

#endif // _MYSQL_