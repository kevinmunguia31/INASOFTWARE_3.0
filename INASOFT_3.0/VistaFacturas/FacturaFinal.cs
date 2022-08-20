using INASOFT_3._0.Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INASOFT_3._0.VistaFacturas
{
    public partial class FacturaFinal : Form
    {
        public FacturaFinal()
        {
            InitializeComponent();
            InstalledPrintersCombo();
        }

        public DataTable InfoProducts()
        {
            string dato = txtIdCliente.Text;
            string idFact = lbIdFactura.Text;
            MySqlDataReader reader = null;
            //string sql = " SELECT a.Cantidad, b.Nombre, a.Precio, a.Total FROM Detalle_Factura a INNER JOIN Productos b ON a.ID_Producto = b.ID INNER JOIN Facturas c ON a.ID_Factura = c.ID INNER JOIN Clientes d ON c.ID_Cliente = d.ID WHERE d.ID = '"+ dato +"' && a.ID_Factura = '" + idFact +"'";
            string sql = "SELECT b.Nombre, a.Precio, a.Cantidad, a.Total FROM Detalle_Factura a INNER JOIN Productos b ON a.ID_Producto = b.ID INNER JOIN Facturas c ON a.ID_Factura = c.ID INNER JOIN Clientes d ON c.ID_Cliente = d.ID WHERE d.ID = '" + dato + "' && a.ID_Factura = '" + idFact + "'";

            MySqlConnection conexioBD = Conexion.getConexion();
            conexioBD.Open();
            //MySqlCommand comando = new MySqlCommand(sql, conexioBD);
            // reader = comando.ExecuteReader();
            MySqlDataAdapter adp = new MySqlDataAdapter(sql, conexioBD);
            DataTable consulta = new DataTable();
            adp.Fill(consulta);

            return consulta;
        }

        private void InstalledPrintersCombo()
        {
            // Add list of installed printers found to the combo box.
            // The pkInstalledPrinters string will be used to provide the display string.
            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                cbImpresoras.Items.Add(pkInstalledPrinters);

            }
            cbImpresoras.Text = "PDFLite";
        }

        private void FacturaFinal_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = InfoProducts();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
