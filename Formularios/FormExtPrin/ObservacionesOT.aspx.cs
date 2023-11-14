using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mail;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Office2010.Excel;


namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class ObservacionesOT : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ManejarIdOT();
                ManejarPedido();
                EnlazarDataGrid();
            }
        }

        private void ManejarIdOT()
        {
            string idOT = Session["Id_OT"] as string;
            bool habilitarObservaciones = !string.IsNullOrEmpty(idOT);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "HabilitarObservaciones", $"var habilitarObservaciones = {habilitarObservaciones.ToString().ToLower()};", true);

            if (!string.IsNullOrEmpty(idOT))
            {
                SqlDataSource1.SelectParameters["Id_OT"].DefaultValue = idOT;
                TextBox5.Text = idOT;
            }
        }

        private void ManejarPedido()
        {
            if (Session["pedido"] != null)
            {
                TextBox4.Text = Session["pedido"].ToString();
            }
        }

        private void EnlazarDataGrid()
        {
            DataGrid5.DataSource = SqlDataSource4;
            DataGrid5.DataBind();
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);


                if (rowIndex >= 0 && rowIndex < DataGrid1.Items.Count)
                {
                    // Obtiene el valor de la columna "Observacion" en la fila seleccionada
                    string observacion = DataGrid1.Items[rowIndex].Cells[7].Text;

                    // Obtiene los valores de "Id_OT" y "Consecutivo_Pedido" de la fila seleccionada
                    string idOT = DataGrid1.Items[rowIndex].Cells[1].Text;
                    string consecutivoPedido = DataGrid1.Items[rowIndex].Cells[2].Text;

                    // Asigna la observación al textarea
                    TextArea1.Value = observacion;


                }
            }
        }
        protected void DataGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int rowIndex = DataGrid1.SelectedIndex;

            if (rowIndex >= 0)
            {
                // Obtiene el valor de "Id_Observacion" de la fila seleccionada
                string idObservacion = DataGrid1.DataKeys[rowIndex].ToString();

                // Asigna el valor a un parámetro del segundo SqlDataSource (SqlDataSource3)
                SqlDataSource3.SelectParameters["Id_Observacion"].DefaultValue = idObservacion;

                // Actualiza el segundo DataGrid (DataGrid3) para cargar los datos filtrados
                DataGrid3.DataBind();
            }
        }

        protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "SelectOb")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);


                if (rowIndex >= 0 && rowIndex < DataGrid4.Items.Count)
                {
                    // Obtiene el valor de la columna "Observacion" en la fila seleccionada
                    string Obra = DataGrid4.Items[rowIndex].Cells[8].Text;
                    string Observacion = DataGrid4.Items[rowIndex].Cells[9].Text;

                    // Obtiene los valores de "Id_OT" y "Consecutivo_Pedido" de la fila seleccionada
                    string idOT = DataGrid4.Items[rowIndex].Cells[1].Text;
                    string consecutivoPedido = DataGrid4.Items[rowIndex].Cells[2].Text;

                    TextBox6.Text = Obra;
                    TextArea2.Value = Observacion;
                }
            }

        }









    }
}