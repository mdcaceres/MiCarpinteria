using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpinteria.Data
{
    class HelperDao
    {
        private static HelperDao instance;

        private string strConnection;
        public SqlConnection conn { get; }
        private string ConnectionString { get; set; }
        public SqlCommand command { get; }
        private DataTable table { get; set; }

        //patron singleton 
        private HelperDao()
        {
            strConnection = Properties.Resources.connectionString; 
            conn = new SqlConnection(strConnection);
            command = new SqlCommand();
            table = new DataTable();
        }

        public static HelperDao GetInstance()
        {
            if(instance == null)
            {
                instance = new HelperDao();
            } 
            return instance;
        }

        private ConnectionState ConnState()
        {
            return conn.State;
        }
        
        public void Open()
        {
            if (ConnState().Equals(ConnectionState.Closed))
                conn.Open();
        } 

        public void Close()
        {
            if (ConnState().Equals(ConnectionState.Open))
                conn.Close();
        } 

        public void CleanTable()
        {
            table.Clear();
        }

        public void SetCommand(CommandType type, string CommandText)
        {
            command.Connection = conn;
            command.CommandType = type;
            command.CommandText = CommandText;
        }

        public void SetCommand(CommandType type, string CommandText, SqlTransaction transact)
        {
            command.Connection = conn;
            command.CommandType = type;
            command.CommandText = CommandText;
            command.Transaction = transact; 
        }

        public DataTable ReturnTable(CommandType type, string CommandText)
        {
            try
            {
                Open();
                CleanTable();
                ClearParams();
                SetCommand(type, CommandText);
                table.Load(command.ExecuteReader());
                return table;
            } 
            catch(SqlException ex)
            {
                throw ex; 
            }
            finally
            {
                Close();
            }
            
        }

        public bool IsStoreProcedure(CommandType type)
        {
            if (type == CommandType.StoredProcedure)
                return true;
            else
                return false;
        }

        public void AddParameters(SqlParameter param)
        {
            command.Parameters.Add(param);
        }

        public void AddParamsValue(Dictionary<String, object> parameters)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Key, item.Value);
            }
        }

        public void AddParamsValue(Dictionary<String, int> parameters)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Key, item.Value);
            }
        }

        public void ClearParams()
        {
            command.Parameters.Clear(); 
        }

        public SqlParameter SetParameter(string paramName, SqlDbType type, ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(paramName, type);
            param.Direction = direction;
            AddParameters(param);
            return param;
        }

        public int BringStoreProcInt(CommandType type, string CommandText, string ParamName, SqlDbType sqlType, ParameterDirection direction)
        {
            if (IsStoreProcedure(type))
            {
                Open();
                SetCommand(type, CommandText);
                SqlParameter param = SetParameter(ParamName, sqlType, direction);
                command.ExecuteReader();
                Close();
                return (int)param.Value;
            }
            return 0;
        }

        //public int ExecSql(string SpName, Dictionary<String,object> parameters)
        //{
        //    int affectRows = 0;

        //    try
        //    {
        //        Open();

        //        SetCommand(CommandType.StoredProcedure, SpName);
                
        //        foreach (var item in parameters)
        //        {
        //            command.Parameters.AddWithValue(item.Key, item.Value);
        //        }

        //        affectRows = command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        Close(); 
        //    }

        //    return affectRows;   
        //}
    }
}
