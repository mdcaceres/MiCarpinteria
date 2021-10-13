using Carpinteria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria
{
    class DaoFactory : AbstractDaoFactory
    {
        //factories handle the details of object creation 
        public override IDao CreatePresupuestoDao()
        {
            return new PresupuestoDao(); 
        }
    } 
}
