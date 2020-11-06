using WebApiCursos.Models;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;

namespace WebApiCursos.DAL
{
    public class DataAccessModule
    {
        public const string DestinoBD = "BD_Destino";

        private readonly IConfiguration _configuration;
        private readonly SqlConnection mConnection;
        private readonly DataHandler _dataHandler;

        public DataAccessModule(IConfiguration config)
        {
            _configuration = config;
            mConnection = new SqlConnection();
            _dataHandler = new DataHandler();
        }

        public string GetConnectionStrings(string bd)
        {
            //switch (bd)
            //{
            //    case DestinoBD:
            //        return _configuration.GetConnectionString("BD_DestinoConnectionString");
            //    default:
            //        return "";
            //}
            return bd switch
            {
                DestinoBD => _configuration.GetConnectionString("BD_DestinoConnectionString"),
                _ => ""
            };
        }

        private void OpenConnection(string database)
        {
            if (this.mConnection.State != ConnectionState.Open)
            {
                var conString = GetConnectionStrings(database);
                mConnection.ConnectionString = conString;
                mConnection.Open();
            }
        }

        private void CloseConnection()
        {
            if (this.mConnection.State == ConnectionState.Open)
            {
                mConnection.Close();
            }
        }

        public void ExecuteNonQuery(string database, string procedure, ArrayList parameters)
        {
            try
            {
                OpenConnection(database);

                using (SqlCommand mComando = new SqlCommand(procedure, mConnection) { CommandType = CommandType.StoredProcedure })
                {
                    foreach (SqlParameter param in parameters)
                    {
                        mComando.Parameters.Add(param);
                    }

                    mComando.ExecuteNonQuery();                    
                }
            }
            catch
            {
                CloseConnection();
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataSet Fill(string database, string procedure, ArrayList parameters)
        {
            DataSet mDataSet = new DataSet();

            try
            {
                OpenConnection(database);

                using (SqlCommand mComando = new SqlCommand(procedure, mConnection))
                {
                    using (SqlDataAdapter mDataAdapter = new SqlDataAdapter(mComando))
                    {
                        mComando.CommandType = CommandType.StoredProcedure;

                        foreach (SqlParameter param in parameters)
                        {
                            mComando.Parameters.Add(param);
                        }

                        mDataAdapter.Fill(mDataSet);
                                                
                        mDataSet.ExtendedProperties.Add("NewId", mComando.ExecuteScalar());
                    }
                }

                return mDataSet;
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task<List<Course>> GetCourses(string database, string procedure, ArrayList parameters)
        {
            try
            {
                OpenConnection(database);

                using SqlCommand mComando = new SqlCommand(procedure, mConnection) { CommandType = CommandType.StoredProcedure };
                foreach (SqlParameter param in parameters)
                {
                    mComando.Parameters.Add(param);
                }

                return _dataHandler.DataReaderHandler(await mComando.ExecuteReaderAsync());
            }
            catch 
            {
                CloseConnection();
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
