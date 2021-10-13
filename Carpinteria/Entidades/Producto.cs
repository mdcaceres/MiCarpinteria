﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpinteria
{
    class Producto
    {
        public int ProductoNro { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public bool Activo {get; set;}

        public Producto(int nro, string nombre, double precio)
        {
            ProductoNro = nro;
            Nombre = nombre;
            Precio = precio;
            Activo = true;
        }

        public override string ToString()
        {
            return Nombre;
        }


    }
}
