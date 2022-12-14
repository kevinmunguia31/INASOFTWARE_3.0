using INASOFT_3._0.Modelos;
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

namespace INASOFT_3._0.VistaFacturas
{
    public partial class DetalleFactura : Form
    {
        public DetalleFactura()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void obtenerIDFactura()
        {
            string dato = txtIdCliente.Text;
            MySqlDataReader reader = null;
            string consulta = "SELECT id FROM facturas WHERE id_cliente='" + dato + "'";
            MySqlConnection conexionBD = Conexion.getConexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(consulta, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lbIdFactura.Text = reader.GetString("id");
                    }
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Al Buscar: " + ex.Message);
            }
            finally { conexionBD.Close(); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string dato = txtCodigo.Text;
            MySqlDataReader reader = null;
            string consulta = "SELECT id, nombre, existencias, precio_venta FROM productos WHERE codigo='" + dato + "'";
            MySqlConnection conexionBD = Conexion.getConexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(consulta, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txtIdProduc.Text = reader.GetString("id");
                        lbProductName.Text = reader.GetString("nombre");
                        lbExistencias.Text = reader.GetString("existencias");
                        lbPrecio.Text = reader.GetString("precio_venta");
                    }
                }
                else
                {
                    MessageDialogError.Show("No se encontraron Productos Con ese Codigo", "Aviso");
                    txtIdProduc.Text = "";
                    lbProductName.Text = ".....";
                    lbPrecio.Text = ".....";
                    lbExistencias.Text = ".....";
                }

                if (lbExistencias.Text == "0")
                {
                    lbExistencias.ForeColor = Color.Red;
                }else
                {
                    lbExistencias.ForeColor = Color.Green;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Al Buscar: " + ex.Message);
            }
            finally { conexionBD.Close(); }
        }

        string[,] ListaProductos = new string[200, 10];
        int fila = 0;

        private void btnAñadirProducto_Click(object sender, EventArgs e)
        {
            string sql = "CALL Facturar_Productos('" + SpinCantidad.Value + "','" + lbPrecio.Text + "','" + txtIdProduc.Text + "','" + lbIdFactura.Text + "')";
            try
            {

                if (lbProductName.Text != "" && lbPrecio.Text != "" && txtIdProduc.Text != "" && SpinCantidad.Value !=  0)
                {
                    if (lbExistencias.Text != "0")
                    {
                        MySqlConnection conexioBD = Conexion.getConexion();
                        conexioBD.Open();
                        MySqlCommand comando = new MySqlCommand(sql, conexioBD);
                        comando.ExecuteNonQuery();
                        MessageDialogInfo.Show("Produsctos Agregados a la Factura", "AVISO");

                        //Agregar a la tabla
                        ListaProductos[fila, 0] = lbProductName.Text;
                        ListaProductos[fila, 1] = lbPrecio.Text;
                        ListaProductos[fila, 2] = SpinCantidad.Value.ToString();
                        ListaProductos[fila, 3] = (float.Parse(SpinCantidad.Value.ToString()) * float.Parse(lbPrecio.Text)).ToString();
                        ListaProductos[fila, 4] = txtIdProduc.Text;
                        dataGridView1.Rows.Add(ListaProductos[fila, 0], ListaProductos[fila, 1], ListaProductos[fila, 2], ListaProductos[fila, 3], ListaProductos[fila, 4]);
                        //AUMENTA LA FILA EN LA TABLA
                        fila++;
                        txtCodigo.Text = lbProductName.Text = lbPrecio.Text = lbExistencias.Text = txtIdProduc.Text = "";
                        SpinCantidad.Value = 0;
                        txtCodigo.Focus();
                    }
                    else
                    {
                        MessageDialogError.Show("No hay existencias de este producto", "AVISO");
                    }

                }
                else
                {
                    MessageDialogWar.Show("Verifique la Cantidad del Producto", "AVISO");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            Subtotal();
        }
        public void Subtotal()
        {
            float subtotal = 0;
            int conteo = 0;

            conteo = dataGridView1.RowCount; //Cuenta los productos en la tabla

            for (int i = 0; i < conteo; i++)
            {
                subtotal += float.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
            }
            lbSubtotal.Text = subtotal.ToString();
        }

        private void DetalleFactura_Load(object sender, EventArgs e)
        {
            obtenerIDFactura();
        }

        private void eliminarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id_producto = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
            int cantidad = int.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            string sql = "CALL Eliminar_Producto_Facturado('" + txtIdDetalleFactura.Text + "','" + id_producto + "','" + cantidad + "')";
            try
            {
                MySqlConnection conexioBD = Conexion.getConexion();
                conexioBD.Open();
                MySqlCommand comando = new MySqlCommand(sql, conexioBD);
                comando.ExecuteNonQuery();
                MessageDialogInfo.Show("Se elimino el Producto Seleccionado", "AVISO");
                if (dataGridView1.CurrentRow == null)
                    return;
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                txtIdDetalleFactura.Text = "";
                Subtotal();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
            MySqlDataReader reader = null;
            string consulta = "SELECT id FROM detalle_factura WHERE id_producto='" + id + "'";
            MySqlConnection conexionBD = Conexion.getConexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(consulta, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txtIdDetalleFactura.Text = reader.GetString("id");
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron Productos Con ese Codigo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Al Buscar: " + ex.Message);
            }
            finally { conexionBD.Close(); }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
            MySqlDataReader reader = null;
            string consulta = "SELECT id FROM detalle_factura WHERE id_producto='" + id + "'";
            MySqlConnection conexionBD = Conexion.getConexion();
            conexionBD.Open();
            try
            {
                MySqlCommand comando = new MySqlCommand(consulta, conexionBD);
                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        txtIdDetalleFactura.Text = reader.GetString("id");
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron Productos Con ese Codigo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Al Buscar: " + ex.Message);
            }
            finally { conexionBD.Close(); }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            
            FacturaFinal frm = new FacturaFinal();
            frm.lbIdFactura.Text = lbIdFactura.Text;
            frm.lbNombreCliente.Text = lbClienteName.Text;
            frm.lbSubtotal.Text = lbSubtotal.Text;
            frm.txtIdCliente.Text = txtIdCliente.Text;
            frm.ShowDialog();
            this.Dispose();
        }
    }
}
