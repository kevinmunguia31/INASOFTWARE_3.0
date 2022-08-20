using INASOFT_3._0.Modelos;
using INASOFT_3._0.VistaFacturas;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INASOFT_3._0.UserControls
{
    public partial class UC_Factura : UserControl
    {
        public UC_Factura()
        {
            InitializeComponent();
            Facturas();
        }

        public void Facturas()
        {
            string sql = "SELECT c.ID, c.Fecha, d.Nombre, SUM(a.cantidad) AS Total_de_Productos, c.Total_Final, c.Efectivo, c.Devolucion, e.Nombre AS Le_Atendio FROM Detalle_Factura a RIGHT JOIN Productos b ON a.ID_Producto = b.ID RIGHT JOIN Facturas c ON a.ID_Factura = c.ID RIGHT JOIN Clientes d ON c.ID_Cliente = d.ID RIGHT JOIN Usuarios e ON c.ID_Usuario = e.ID GROUP BY c.ID";
            //string sql = "SELECT c.ID, c.Fecha, d.Nombre AS Cliente, SUM(a.cantidad) AS Total_de_Productos, c.Total_Final, c.Efectivo, c.Devolucion, e.Nombre AS Le_Atendio FROM Detalle_Factura a INNER JOIN Productos b ON a.ID_Producto = b.ID INNER JOIN Facturas c ON a.ID_Factura = c.ID INNER JOIN Clientes d ON c.ID_Cliente = d.ID INNER JOIN Usuarios e ON c.ID_Usuario = e.ID";
            try
            {
                MySqlConnection conexioBD = Conexion.getConexion();
                conexioBD.Open();
                MySqlCommand comando = new MySqlCommand(sql, conexioBD);
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridFatura.DataSource = dt;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        private void txtNewInvoice_Click(object sender, EventArgs e)
        {
            Facturar1 facturar = new Facturar1();
            facturar.ShowDialog();
        }
    }
}
