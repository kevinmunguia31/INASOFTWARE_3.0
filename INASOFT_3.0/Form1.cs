using INASOFT_3._0.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INASOFT_3._0
{
    public partial class Principal : Form
    {
        int tipoUser;
        public Principal()
        {
            InitializeComponent();

           // VScrollBar vScrollBar = new VScrollBar();
            ///vScrollBar.Height = 15;
            //vScrollBar.Width = 200;
            //vScrollBar.Dock = DockStyle.Bottom;
            //this.Controls.Add(vScrollBar);

            //Cargar El Dashboard
            UC_HOME uc = new UC_HOME();
            addUserControl(uc);

            //Monstra Usuario Logueado
            lbUserName.Text = Modelos.Sesion.nombre;
            tipoUser = Modelos.Sesion.id_tipo;
            if (tipoUser == 1)
            {
                lbTipoUser.Text = "(Administrador)";
                lbTipoUser.ForeColor = Color.Green;
            }
            else
            {
                lbTipoUser.Text = "(Empleado)";
                
            }
        }

        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContenedor.Controls.Clear();
            panelContenedor.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            UC_HOME uc = new UC_HOME();
            addUserControl(uc);
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            UC_Productos uc = new UC_Productos();
            addUserControl(uc);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogaut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login frm = new Login();
            frm.Visible = true;
            Modelos.Sesion sesion = new Modelos.Sesion();
        }

        private void panelContenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Principal_Load(object sender, EventArgs e)
        {

        }

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            UC_Clientes uc = new UC_Clientes();
            addUserControl(uc);
        }

        private void btnProvider_Click(object sender, EventArgs e)
        {
            UC_Proveedor uc = new UC_Proveedor();
            addUserControl(uc);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            UC_Usuarios uc = new UC_Usuarios();
            addUserControl(uc);
        }

        private void btnFactura_Click(object sender, EventArgs e)
        {
            UC_Factura uc = new UC_Factura();
            addUserControl(uc);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            UC_Settings uc = new UC_Settings();
            addUserControl(uc);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            UC_Reportes uc = new UC_Reportes();
            addUserControl(uc);
        }
    }
}
