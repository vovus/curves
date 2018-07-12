using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;

namespace DataLayer
{
	public class TermRateExcel
	{
		private DataSet ds = null;

		const string ConnStringSql = "Provider=sqloledb;" 
			+ "Data Source=dinoch-8;Initial Catalog=Northwind;Integrated Security=SSPI;";

		const string OutputFilename = "ycData.xls";

		string ConnStringExcel = string.Empty;
		
		const string sqlSelect = @"SELECT [Term], [Rate], [Id] FROM [ycData$]";
		const string sqlInsert = @"INSERT INTO [ycData$]([Term],[Rate], [Id]) VALUES(@term, @rate, @id)";
		const string sqlUpdate = @"UPDATE [ycData$] SET [Term]=@term, [Rate]=@rate "
									+ @"WHERE [Id] = @id";
		const string sqlDelete = @"DELETE from [ycData$] WHERE [Id] = @id)";

		DbProviderFactory dbFactories;

		public TermRateExcel()
		{
			string dbPath = AppDomain.CurrentDomain.BaseDirectory + OutputFilename;

			ConnStringExcel = "Provider=Microsoft.Jet.OLEDB.4.0;" 
				+ "Data Source=" + dbPath + ";" 
				+ "Extended Properties=\"Excel 8.0;HDR=yes;\"";  // FIRSTROWHASNAMES=1;READONLY=false\"

			dbFactories = DbProviderFactories.GetFactory("System.Data.OleDb");
		}

		public DataSet LoadData()
		{
			DbConnection ConnSql = dbFactories.CreateConnection();
			ConnSql.ConnectionString = ConnStringExcel;
			DbDataAdapter da = dbFactories.CreateDataAdapter();

			da.SelectCommand = ConnSql.CreateCommand();
			da.SelectCommand.CommandText = sqlSelect;
			da.SelectCommand.Connection = ConnSql;

			ds = new DataSet();
			da.Fill(ds);

			return ds;
		}

		public void SaveData()
		{
			DbConnection ConnExcel = dbFactories.CreateConnection();
			ConnExcel.ConnectionString = ConnStringExcel;
			DbDataAdapter da = dbFactories.CreateDataAdapter();

			//bool isNotified = false;
			//bool isDoTheSave = false;

			// need to update the row so the DA does the insert...
			foreach (DataRow r in ds.Tables[0].Rows)
			{
				if (r.RowState == DataRowState.Added)
				{
					/*
					if (!isNotified)
					{
						DialogResult res = 
							MessageBox.Show("Do you want to SaveChanges?",
										"Yield Curve",
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Question);
						
						if(res == DialogResult.Yes)
						{
							isDoTheSave = true;
						}
						isNotified = true;
					}

					if (!isDoTheSave)
						continue;
					*/
					da.InsertCommand = ConnExcel.CreateCommand();
					da.InsertCommand.CommandText = sqlInsert;
					da.InsertCommand.Connection = ConnExcel;

					DbParameter param1 = da.InsertCommand.CreateParameter();
					param1.ParameterName = "@term";
					param1.DbType = System.Data.DbType.String;
					DbParameter param2 = da.InsertCommand.CreateParameter();
					param2.ParameterName = "@rate";
					param2.DbType = System.Data.DbType.Double;
					DbParameter param3 = da.InsertCommand.CreateParameter();
					param3.ParameterName = "@id";
					param3.DbType = System.Data.DbType.Int32;

					da.InsertCommand.Parameters.Add(param1);
					da.InsertCommand.Parameters.Add(param2);
					da.InsertCommand.Parameters.Add(param3);
					da.InsertCommand.Parameters["@term"].Value = r["Term"];
					da.InsertCommand.Parameters["@rate"].Value = r["Rate"];
					da.InsertCommand.Parameters["@id"].Value = r["Id"];
					
					da.Update(ds);
				}
				else if (r.RowState == DataRowState.Deleted)
				{
					/*
					if (!isNotified)
					{
						DialogResult res =
							MessageBox.Show("Do you want to SaveChanges?",
										"Yield Curve",
										MessageBoxButtons.OKCancel,
										MessageBoxIcon.Question);

						if (res == DialogResult.OK)
						{
							isDoTheSave = true;
						}
						isNotified = true;
					}

					if (!isDoTheSave)
						continue;
					*/
					da.DeleteCommand = ConnExcel.CreateCommand();
					da.DeleteCommand.CommandText = sqlDelete;
					da.DeleteCommand.Connection = ConnExcel;

					DbParameter param = da.DeleteCommand.CreateParameter();
					param.ParameterName = "@id";
					param.DbType = System.Data.DbType.Int32;
					
					da.DeleteCommand.Parameters.Add(param);
					da.DeleteCommand.Parameters["@id"].Value = r["Id"];

					da.Update(ds);
				}
				else if (r.RowState == DataRowState.Modified)
				{
					/*
					if (!isNotified)
					{
						DialogResult res =
							MessageBox.Show("Do you want to SaveChanges?",
										"Yield Curve",
										MessageBoxButtons.OKCancel,
										MessageBoxIcon.Question);

						if (res == DialogResult.OK)
						{
							isDoTheSave = true;
						}
						isNotified = true;
					}

					if (!isDoTheSave)
						continue;
					*/
					da.UpdateCommand = ConnExcel.CreateCommand();
					da.UpdateCommand.CommandText = sqlUpdate;
					da.UpdateCommand.Connection = ConnExcel;

					DbParameter param1 = da.UpdateCommand.CreateParameter();
					param1.ParameterName = "@term";
					param1.DbType = System.Data.DbType.String;
					DbParameter param2 = da.UpdateCommand.CreateParameter();
					param2.ParameterName = "@rate";
					param2.DbType = System.Data.DbType.Double;
					DbParameter param3 = da.UpdateCommand.CreateParameter();
					param3.ParameterName = "@id";
					param3.DbType = System.Data.DbType.Int32;

					da.UpdateCommand.Parameters.Add(param1);
					da.UpdateCommand.Parameters.Add(param2);
					da.UpdateCommand.Parameters.Add(param3);
					da.UpdateCommand.Parameters["@term"].Value = r["Term"];
					da.UpdateCommand.Parameters["@rate"].Value = r["Rate"];
					da.UpdateCommand.Parameters["@id"].Value = r["Id"];

					da.Update(ds);
				}
			}
		}
		/*
        string connectionString = string.Empty;
		string sqlSelect = string.Empty;
		string sqlInsert = string.Empty;

        DbProviderFactory dbFactories = DbProviderFactories.GetFactory("System.Data.OleDb");
        DbConnection connection = null;
		DataSet ycData = null;

		public TermRateExcel()
        {
            connection =  dbFactories.CreateConnection();
            
			string dbPath = AppDomain.CurrentDomain.BaseDirectory + "ycData.xls";

			connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;
									Data Source=" + dbPath + @";Extended Properties=
										""Excel 8.0;HDR=YES;"""; // FIRSTROWHASNAMES=1;READONLY=false\"

			connection.ConnectionString = connectionString;
        }

        public void SaveData(string term, double rate)
         {
            try
            {
                using (DbCommand cmdInsert = connection.CreateCommand())
                {
					string commandText = "INSERT INTO [ycData$]([Term],[Rate]) VALUES(@term,@rate)";
                    DbParameter param = cmdInsert.CreateParameter();
                    param.ParameterName = "@term";
                    param.DbType = System.Data.DbType.String;
                    DbParameter param1 = cmdInsert.CreateParameter();
                    param1.ParameterName = "@rate";
                    param1.DbType = System.Data.DbType.Double;
                    cmdInsert.CommandText = commandText;
                    cmdInsert.Parameters.Add(param);
                    cmdInsert.Parameters.Add(param1);
                    cmdInsert.Parameters["@term"].Value = term;
                    cmdInsert.Parameters["@rate"].Value = rate;
                    connection.Open();
                    cmdInsert.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

		public void SaveData()
		{
			try
			{
				connection.Open();

				DbDataAdapter adapter = dbFactories.CreateDataAdapter();
				DbCommand updateCommand = connection.CreateCommand();

				updateCommand.CommandText = "INSERT INTO [ycData$]([Term],[Rate]) VALUES(@term,@rate)";
				updateCommand.Connection = connection;

				adapter.UpdateCommand = updateCommand;
				adapter.Update(ycData);
				connection.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				connection.Close();
			}
		}

        public DataSet LoadData()
        {
            DbDataAdapter adapter = dbFactories.CreateDataAdapter();
            DbCommand selectCommand = connection.CreateCommand();

            selectCommand.CommandText = "SELECT Term, Rate FROM [ycData$]";
            selectCommand.Connection = connection;

			ycData = new DataSet();

            try
            {
                //connection.Open();
                adapter.SelectCommand = selectCommand;
				adapter.Fill(ycData);
            }
            catch (SystemException e)
            {
				MessageBox.Show(e.Message);
            }
            finally
            {
                //connection.Close();
            }
			return ycData;
        }
		
		/*
		public TermRateExcel()
		{
		}
		public DataSet LoadData()
		{
			string dbPath = AppDomain.CurrentDomain.BaseDirectory + "ycData.xls";

			string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;"
									 + @"Data Source=" + dbPath 
									 + @";Extended Properties='Excel 8.0;HDR=YES;'";

			DbProviderFactory dbFactories = DbProviderFactories.GetFactory("System.Data.OleDb");

			DbConnection con = dbFactories.CreateConnection();
			con.ConnectionString = connectionString;

			DbCommand selectCommand = con.CreateCommand();
			selectCommand.CommandText = @"SELECT Term, Rate FROM [ycData$]";
			selectCommand.Connection = con;

			DataSet ycData = new DataSet();
			try
			{
				//con.Open();

				DbDataAdapter adapter = dbFactories.CreateDataAdapter();
				adapter.SelectCommand = selectCommand;
				adapter.Fill(ycData);
			}
			catch (SystemException e)
			{
				MessageBox.Show(e.Message);
			}
			finally
			{
				//con.Close();
			}
			return ycData;
		}
		*/
    }
}
