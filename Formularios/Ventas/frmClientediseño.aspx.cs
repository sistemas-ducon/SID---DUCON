using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Windows.Forms;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Ventas
{
    public partial class frmClientediseño : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgrdCliente.SelectedIndex >= 0)
                {
                    //Seleccionar fila
                    string Nit = dgrdCliente.SelectedRow.Cells[1].Text;
                    string Cliente = dgrdCliente.SelectedRow.Cells[2].Text;
                    string Telefono = dgrdCliente.SelectedRow.Cells[4].Text;
                    string Direccion = dgrdCliente.SelectedRow.Cells[5].Text;
                    string procedencia = dgrdCliente.SelectedRow.Cells[6].Text;




                    //Textboxt recibiendo datos de la fila seleccionada del datadrid
                    txtId_Cliente.Text = Nit;
                    txtNombre_Compañia.Text = Cliente;
                    txttelcliente.Text = Telefono;
                    txtDir.Text = Direccion;
                    dtacboProcedencia.Text = procedencia;



                }
            }
            catch (Exception error)
            {


                Console.WriteLine("Algo esta mal" + error.Message);

            }
        }
    }
}


