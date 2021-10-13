using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpinteria
{
    interface IDao
    {
        bool Create(Presupuesto myObject);

        void Read();

        void Update();

        void Delete();

        DataTable ReturnTable(CommandType type, string CommandText);

        int GetNextId();
    }
}
