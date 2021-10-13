using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria
{
    class DetallePresupuesto
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public DetallePresupuesto(Producto product, int cant)
        {
            Producto = product;
            Cantidad = cant; 
        }

        public double CalcularSubtotal()
        {
            return (Producto.Precio * Cantidad); 
        }

    }
}
