using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Carpinteria.Data;
using System.Globalization;

namespace Carpinteria.Formularios
{
    public partial class FrmNuevoPresupuesto : Form
    {
        //declaramos una variable presupuesto
        private Presupuesto Nuevo;
        private Gestor gestor;

        public FrmNuevoPresupuesto()
        {
            InitializeComponent();
            //instalnciamos el presupuesto
            Nuevo = new Presupuesto();
            gestor = new Gestor(new DaoFactory());
        }

        private void txtsubTotal_Click(object sender, EventArgs e)
        {

        }

        private void FrmNuevoPresupuesto_Load(object sender, EventArgs e)
        {
            //mostramos el numero del presupuesto
            LblNumeroPresupuesto.Text += gestor.NextId();

            //cargamos el combobox
            gestor.LoadComboBox(cboProductos, CommandType.StoredProcedure, "SP_CONSULTAR_PRODUCTOS", "n_producto", "id_producto");
            //rellenamos los datos de los txt
            //DateTime.Today retorna la fecha
            //DateTime.Now retorna fecha y hora
            txtFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtCliente.Text = "Consumidor Final";
            txtDescuento.Text = "0";
        }

        public bool validate()
        {
            //Validamos
            if (cboProductos.Text.Equals(string.Empty))
            {
                MessageBox.Show("You must select a product", "Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtCantidad.Text) || !int.TryParse(txtCantidad.Text, out _) || !int.TryParse(txtDescuento.Text, out _))
            {
                MessageBox.Show("Please write a valid amount", "control", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
                
            }

            //validamos si el item ya esta agregado 
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                //si el valor de la celda colProductos es igual a cboProductos.Text
                if (row.Cells["ColProductos"].Value.ToString().Equals(cboProductos.Text))
                {
                    MessageBox.Show("this product it's already added");
                    return false;
                }
            }
            return true; 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (validate())
            {
                //creamos un DataRowView
                //cada fila va a ser un item de mi grilla
                //lo debemos castear
                //el iten seleccionado el en combo lo voy a comvertir en una fila de la grilla
                DataRowView item = (DataRowView)cboProductos.SelectedItem;
                //creo unas variables para almacenar los datos del producto o item seleccioando
                int nroProducto = Convert.ToInt32(item.Row.ItemArray[0]);
                string nom = item.Row.ItemArray[1].ToString();
                double pre = Convert.ToDouble(item.Row.ItemArray[2]);

                //Creamos una instancia de productos
                Producto Producto = new Producto(nroProducto, nom, pre);

                int cant = Convert.ToInt32(txtCantidad.Text);

                //Creamos un detalle presupuesto con el producto y con la cantidad
                DetallePresupuesto detalle = new DetallePresupuesto(Producto, cant);

                //agregamos el detalle al presupuesto con el metodo agregarDetalle()
                Nuevo.AgregarDetalle(detalle);

                //mostramos el detalle en la grilla con el metodo add y hacemos un new object[]
                dgvDetalles.Rows.Add(new object[] { nroProducto, nom, pre, cant });

                txtSubTotal.Text = Nuevo.CalcularTotal().ToString();
                double descuento = Nuevo.CalcularTotal() * Convert.ToDouble(txtDescuento.Text) / 100;
                txtTotal.Text = (Nuevo.CalcularTotal() - descuento).ToString();
            } 
        }

        private void LblNumeroPresupuesto_Click(object sender, EventArgs e)
        {
        }

        private void cboProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //quitar el detalle
            //si la coluna de la grilla es igual a cuatro
            if (dgvDetalles.CurrentCell.ColumnIndex == 4)
            {
                //quitar el detalle del presupuesto
                Nuevo.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                //quitamos el detalle de la grilla
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);

                //Volvemos a calcular el total y el subtotal... deberia ser un metodo
                txtSubTotal.Text = Nuevo.CalcularTotal().ToString();
                double descuento = Nuevo.CalcularTotal() * Convert.ToDouble(txtDescuento.Text) / 100;
                txtTotal.Text = (Nuevo.CalcularTotal() - descuento).ToString();
            }

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Validar
            if(txtCliente.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar un nombre de cliente", "control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCliente.Focus();
                return; 
            }

            if(dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos un detalle", "control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboProductos.Focus();
                return; 
            }

            //grabar maestro y detalles (presupuesto) 

            GuardarPresupesto(); 
        }

        private void GuardarPresupesto()
        {
            Nuevo.Fecha = DateTime.ParseExact(txtFecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Nuevo.Cliente = txtCliente.Text;
            Nuevo.Descuento = double.Parse(txtDescuento.Text);
            Nuevo.Total = Convert.ToDouble(txtTotal.Text);

            if(gestor.ConfirmObject(Nuevo))
            {
                MessageBox.Show("El presupuesto se grabo correctamente", "control", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose(); 
            } 
            else
            {
                MessageBox.Show("El presupuesto No se pudo grabar", "control", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Dispose();    
            }
        }
    }
}
