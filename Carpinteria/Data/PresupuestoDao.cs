using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Carpinteria.Data
{
    class PresupuestoDao : IDao
    {
        private SqlConnection conn { get; set; }

        private string ConnectionString { get; set; }

        private SqlCommand command { get; set; }

        private DataTable table { get; set; }

        private HelperDao helper { get; set; }

        public PresupuestoDao()
        {
            conn = new SqlConnection();
            command = new SqlCommand();
            table = new DataTable();
            helper = HelperDao.GetInstance(); 
        }

        public virtual void Read() { }

        public virtual void Update() { }

        public virtual void Delete() { }

        public int GetNextId()
        {
            return helper.BringStoreProcInt(CommandType.StoredProcedure, "SP_PROXIMO_ID", "@next", SqlDbType.Int, ParameterDirection.Output);
        }

        public DataTable ReturnTable(CommandType type, string CommandText)
        {
           return helper.ReturnTable(type, CommandText);
        }

        public bool Create(Presupuesto myObject)
        {
            SqlTransaction transact = null;
            bool flag = true; 
            try
            {
                //insertar MAESTRO
                helper.Open();
                //incio la transaccion 
                transact = helper.conn.BeginTransaction();
                //creo un comando sp con transaccion 
                helper.ClearParams(); 
                helper.SetCommand(CommandType.StoredProcedure, "SP_INSERTAR_MAESTRO", transact);

                //creo un diccionario 
                Dictionary<string, object> masterParams = new Dictionary<string, object>();
                masterParams.Add("@cliente", myObject.Cliente);
                masterParams.Add("@dto", myObject.Descuento);
                masterParams.Add("@total", myObject.Total);
                helper.AddParamsValue(masterParams);
                //evito tener que hacer ---> helper.command.Paramaters.AddWithValue("@cliente", myObject.Cliente)

                //recibiendo el parametro de salida
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@presupuesto_nro";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;

                helper.AddParameters(param);

                helper.command.ExecuteNonQuery();

                int nroPresupuesto = (int)param.Value;
                
                myObject.PresupuestoNro = nroPresupuesto;

                Dictionary<string, int> detParams = new Dictionary<string, int>();

                int detNro = 1;
                foreach (DetallePresupuesto det in myObject.Detalles)
                {
                    helper.ClearParams();
                    helper.SetCommand(CommandType.StoredProcedure, "SP_INSERTAR_DETALLE", transact);

                    detParams.Add("@presupuesto_nro", myObject.PresupuestoNro);
                    detParams.Add("@detalle", detNro);
                    detParams.Add("@id_producto", det.Producto.ProductoNro);
                    detParams.Add("@cantidad", det.Cantidad);
                    helper.AddParamsValue(detParams);

                    helper.command.ExecuteNonQuery();
                    detNro++;
                }
                transact.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                helper.Close(); 
            }
            return flag;
        }
    }
}
