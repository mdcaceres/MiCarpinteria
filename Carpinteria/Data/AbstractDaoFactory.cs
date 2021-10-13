using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria
{
    abstract class AbstractDaoFactory
    {
        public abstract IDao CreatePresupuestoDao();
    }
}
