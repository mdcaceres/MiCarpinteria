using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpinteria
{
    class Gestor //this is the client of the factory
    {
        public IDao dao { get; set; }

        public Gestor(AbstractDaoFactory factory)
        {
            dao = factory.CreatePresupuestoDao(); 
        }

        public int NextId()
        {
            return dao.GetNextId(); 
        }

        public void LoadComboBox(ComboBox combo, CommandType type, string CommandText, string display, string value)
        {
            DataTable table = dao.ReturnTable(type, CommandText);
            combo.DataSource = table;
            combo.DisplayMember = display;
            combo.ValueMember = value;
        }

        public bool ConfirmObject(Presupuesto myObject)
        {
            return dao.Create(myObject);
        }
    }
}
