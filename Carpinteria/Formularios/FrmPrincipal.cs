using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpinteria.Formularios
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void nuevoPresupuestoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Form vista = new FrmNuevoPresupuesto();
            //vista.Show(); 
            vista.ShowDialog();

        }

        private void consultarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form vista = new frmConsultar();
            //vista.Show(); 
            vista.ShowDialog();
        }
    }
}
