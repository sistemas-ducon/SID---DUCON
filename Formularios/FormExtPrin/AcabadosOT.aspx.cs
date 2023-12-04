using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Windows.Forms;


namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class AcabadosOT : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable emptyDataTable = new DataTable(); // Crear un DataTable vacío
            DataGrid1.DataSource = emptyDataTable; // Asignar el DataTable vacío al DataGrid
            DataGrid1.DataBind();
            botonGrabarValidacion();
            CargarDatos();
            
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

        }
        private void botonGrabarValidacion()
        {
            try
            {
                if (!IsPostBack)
                {
                    string idOT = Session["Id_OT"]?.ToString();
                    string pedido = Session["pedido"]?.ToString();

                    bool habilitarLinkButton = true;

                    // Verifica si hay id_OT y pedido o si están vacíos
                    if (string.IsNullOrEmpty(idOT) || string.IsNullOrEmpty(pedido))
                    {
                        habilitarLinkButton = true;
                    }
                    else
                    {
                        // Si hay id_OT y pedido, verifica la condición
                        bool condicionCumplida = VerificarCondicion(idOT, pedido);
                        if (condicionCumplida)
                        {
                            Button3.Enabled = true;
                            habilitarLinkButton = true;
                        }
                        else
                        {
                            Button3.Enabled = false;
                            habilitarLinkButton = false;
                        }
                    }

                    // Habilita el LinkButton si se cumplen las condiciones
                    if (habilitarLinkButton)
                    {
                        foreach (DataGridItem item in DataGrid2.Items)
                        {
                            LinkButton lnkSelectRow = item.FindControl("lnkSelectRow") as LinkButton;
                            if (lnkSelectRow != null)
                            {
                                lnkSelectRow.Enabled = true;
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores: puedes mostrar el mensaje de error en un log o en un control visual.
                Response.Write("Error al procesar: " + ex.Message);
            }
        }

        private bool VerificarCondicion(string idOT, string consecutivoPedido)
        {
          

            bool condicionCumplida = false;

            try
            {
               string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                // Consulta SQL para verificar la condición en la base de datos
                string consultaSQL = "SELECT COUNT(*) FROM tblOT " +
                                     $"WHERE Id_OT = '{idOT}' AND Consecutivo_Pedido = '{consecutivoPedido}' AND Terminado_Ventas = '0'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(consultaSQL, connection))
                    {
                        connection.Open();
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Si se obtiene al menos un resultado (count > 0), la condición se cumple
                        condicionCumplida = (count > 0);
                    }
                }

            }
            catch (Exception ex)
            {
                
                Response.Write("Error al verificar la condición desde la base de datos: " + ex.Message);
            }

            // Retorna el resultado de la verificación de la condición
            return condicionCumplida;
        }

        private void CargarDatos()
        {
            try
            {
                string idOT = Session["Id_OT"]?.ToString();
                string consecutivoPedido = Session["pedido"]?.ToString();

                if (string.IsNullOrEmpty(idOT) || string.IsNullOrEmpty(consecutivoPedido))
                {
                    throw new Exception("Las variables de sesión no contienen valores válidos.");
                }

                string consultaSQL = @"
                                    SELECT GA.*, A.*, O.*, GOA.GrupoObjetoParaAcabado
                                    FROM tblGrupodeAcabado GA
                                    INNER JOIN tblAcabado A ON GA.ID_GrupoAcabado = A.ID_GrupoAcabado
                                    INNER JOIN tblOTAcabados O ON A.ID_Acabado = O.ID_Acabado
                                    INNER JOIN tblGrupoObjetoparaAcabado GOA ON O.ID_GrupoObjetoParaAcabado = GOA.ID_GrupoObjetoparaAcabado
                                    WHERE O.Id_OT = @IdOT
                                    AND O.Consecutivo_Pedido = @ConsecutivoPedido
                                    ORDER BY A.Descripcion_Acabado";


                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(consultaSQL, connection);
                    command.Parameters.AddWithValue("@IdOT", idOT);
                    command.Parameters.AddWithValue("@ConsecutivoPedido", consecutivoPedido);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataGrid1.DataSource = dataTable;
                        DataGrid1.DataBind();
                    }
                    else
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
              
            }
        }

        protected void DespieceAcabados_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkSelectRow = (LinkButton)sender;
                int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);
                DataGridItem selectedRow = DataGrid1.Items[rowIndex];

                string idGrupoObjeto = selectedRow.Cells[7].Text;

                SqlDataSource2.SelectParameters.Clear();
                SqlDataSource2.SelectParameters.Add("ID_GrupoObjetoparaAcabado", idGrupoObjeto);
                DataGrid2.DataBind();

                foreach (DataGridItem item in DataGrid2.Items)
                {
                    if (item.Cells[2].Text == idGrupoObjeto)
                    {
                        item.CssClass = "selected-roww";
                        item.Attributes["data-selected"] = "true";

                        string script = "<script>scrollDataGrid();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollDataGrid", script, false);
                    }
                }

                foreach (DataGridItem item in DataGrid1.Items)
                {
                    if (item != selectedRow)
                    {
                        item.CssClass = "";
                    }
                }

                selectedRow.CssClass = "selected-row";

                Label1.Text = selectedRow.Cells[8].Text;
                Label1.Visible = true;
               

                Label5.Text = selectedRow.Cells[2].Text;
                Label5.Visible = true;

                Label3.Text = selectedRow.Cells[1].Text;
                Label3.Visible = true;

                string detalleAdicional = selectedRow.Cells[3].Text;

                // Verificar si el detalle adicional es nulo o "&nbsp;"
                TextBox1.Text = string.IsNullOrEmpty(detalleAdicional) || detalleAdicional == "&nbsp;"
                    ? string.Empty
                    : detalleAdicional;

                string idGrupoAcabado = selectedRow.Cells[9].Text;

                SqlDataSource3.SelectParameters.Clear();
                SqlDataSource3.SelectParameters.Add("ID_GrupoObjetoParaAcabado", idGrupoAcabado);
                DataGrid3.DataBind();
            }
            catch (Exception ex)
            {
                // Manejo de la excepción (puedes mostrar un mensaje de error, registrar la excepción, etc.)
                // Por ejemplo:
                // Response.Write("Ocurrió un error: " + ex.Message);
                // o
                // Logger.Log(ex);
            }
        }

        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid2.Items[rowIndex];

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid2.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-roww";

            DataGridItem selectedRowDataGrid2 = DataGrid2.Items[rowIndex];

            // Obtén el valor de la columna ID_GrupoObjetoparaAcabado de la fila seleccionada en DataGrid2
            string idGrupoObjeto = selectedRowDataGrid2.Cells[2].Text; // Asegúrate de que Cells[1] sea el índice correcto

            // Modifica dinámicamente la consulta del SqlDataSource4 con el nuevo valor
            SqlDataSource4.SelectCommand = "SELECT tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado, tblGrupodeAcabado.ID_GrupoAcabado, tblGrupodeAcabado.Descripcion_Grupo " +
                                            "FROM tblGrupodeAcabado " +
                                            "INNER JOIN tblGrupoAcab_GrupoObjtAcab ON tblGrupodeAcabado.ID_GrupoAcabado = tblGrupoAcab_GrupoObjtAcab.ID_GrupoAcabado " +
                                            "INNER JOIN tblGrupoObjetoParaAcabado ON tblGrupoAcab_GrupoObjtAcab.ID_GrupoObjetoparaAcabado = tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado " +
                                            "WHERE tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado = '" + idGrupoObjeto + "';";

            // Actualiza el DataGrid4
            DataGrid4.DataBind();
            DataGrid4.Visible = true;

            System.Web.UI.WebControls.Label Label3 = (System.Web.UI.WebControls.Label)FindControl("Label3");
           

            // Establece la visibilidad de los Labels
            Label3.Visible = true;
         

            // Obtén el valor de la columna "GrupoObjetoparaAcabado" de la fila seleccionada en DataGrid2
            string grupoObjetoSeleccionado = selectedRowDataGrid2.Cells[1].Text; // Asegúrate de que Cells[1] sea el índice correcto

            // Asigna el valor al Label3
            Label3.Text = grupoObjetoSeleccionado;


        }

        protected void lnkSelectRow4_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid4.Items[rowIndex];

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid4.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-roww";


            string idGrupoAcabado = selectedRow.Cells[2].Text; // Asegúrate de que Cells[1] sea el índice correcto

            // Actualiza el parámetro del SqlDataSource3 con el valor obtenido
            SqlDataSource3.SelectParameters["ID_GrupoObjetoParaAcabado"].DefaultValue = idGrupoAcabado;

            // Actualiza el DataGrid3 con los nuevos datos
            DataGrid3.DataBind();

        }

        protected void lnkSelectRow3_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid3.Items[rowIndex];

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid3.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-roww";

          
            System.Web.UI.WebControls.Label Label5 = (System.Web.UI.WebControls.Label)FindControl("Label5");

            // Establece la visibilidad de los Labels
           
            Label5.Visible = true;

            DataGridItem selectedRowDataGrid3 = DataGrid3.Items[rowIndex];

            // Obtén el valor de la columna "GrupoObjetoparaAcabado" de la fila seleccionada en DataGrid2
            string descripcionAcabado = selectedRowDataGrid3.Cells[4].Text; 

            // Asigna el valor al Label3
            Label5.Text = descripcionAcabado;

            //// Obtener el valor de la columna "DeLinea" de la fila seleccionada en DataGrid3
            //string deLinea = selectedRowDataGrid3.Cells[5].Text; // Asegúrate de que Cells[5] sea el índice correcto

            //if (deLinea == "false")
            //{
            //    // Realizar la consulta a la base de datos
            //    string idAcabado = selectedRowDataGrid3.Cells[3].Text; // Obtener el ID_Acabado de la fila seleccionada
            //    string query = "SELECT * FROM tblAcabado WHERE ID_Acabado = '" + idAcabado + "' AND DeLinea = '0'";
        
            //    bool tuVariableConsulta = true; // Esto es un ejemplo, deberías tener la lógica real para determinar si la consulta fue exitosa

            //    if (tuVariableConsulta)
            //    {
            //        // Si la consulta es exitosa, muestra el modal con el mensaje de éxito usando JavaScript/jQuery
            //        ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#myModal').css('display', 'block');", true);
                   
            //    }
                
            //}
            //else
            //{

            //}
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
           {
                LinkButton lnkSelectRow = e.Item.FindControl("lnkSelectRow") as LinkButton;

                if (lnkSelectRow != null)
               {
                    // Aquí se deshabilita el LinkButton
                    lnkSelectRow.Enabled = false;
                    // Se almacena el estado del LinkButton en una variable de sesión
                   Session["LinkButtonEnabled"] = false;
                }
           }
        }

    }

}
