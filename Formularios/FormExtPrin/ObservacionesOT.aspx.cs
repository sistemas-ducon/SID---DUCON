using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mail;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Data.SqlClient;



namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class ObservacionesOT : System.Web.UI.Page
    {
        private string id;
        private string pedido;

        private string CadenaConexionSID = "BD_SIDSQL";

        private string CadenaConexionISID = "BD_ISIDSQL";

        private string CadenaConexionSSF = "BD_SSF";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDropDownList();

                // Verificar si la variable de sesión existe antes de acceder a ella
                if (Session["ValorDeObra"] != null)
                {
                    string valorDeObra = Session["ValorDeObra"].ToString();
                    TextBox1.Text = valorDeObra; // Asignar el valor al TextBox en ObservacionesOT
                }
                AsignarValorTextBox();
                ManejarIdOT();
                ManejarPedido();
                EnlazarDataGrid();
                EnlazarDataGrid4();

               
            }

            Session.Remove("Id_OT");
            Session.Remove("pedido");

        }

        private void CargarDropDownList()
        {
            DataTable dt = ObtenerDatosParaDropDownList(); // Llamada a un método para obtener los datos

            if (dt != null && dt.Rows.Count > 0)
            {
                DropDownList1.DataSource = dt;
                DropDownList1.DataTextField = "TipoObservacion"; // Columna que se mostrará
                DropDownList1.DataValueField = "id_TipoObservacion"; // Columna para el valor
                DropDownList1.DataBind();

                // Agregar un elemento por defecto
                DropDownList1.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" "));

            }
        }

        private DataTable ObtenerDatosParaDropDownList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                string query = "SELECT tblTipoObservacion.id_TipoObservacion, CONCAT_WS('-', tblTipoObservacion.Aplicacion, tblTipoObservacion.Descripcion) AS TipoObservacion " +
                               "FROM tblTipoObservacion " +
                               "WHERE tblTipoObservacion.Aplicacion IS NOT NULL AND tblTipoObservacion.Descripcion IS NOT NULL " +
                               "GROUP BY tblTipoObservacion.id_TipoObservacion, tblTipoObservacion.Aplicacion, tblTipoObservacion.Descripcion " +
                               "ORDER BY tblTipoObservacion.Aplicacion";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private void AsignarValorTextBox()
        {
            string id = Session["Id_OT"]?.ToString();
            string pedido = Session["pedido"]?.ToString();

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pedido))
            {
                // Realizar la conexión y la consulta a la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT tblEmpleado.Mail " +
                                   "FROM tblOT " +
                                   "INNER JOIN tblEmpleado ON tblOT.AbiertoPor = tblEmpleado.Cedula " +
                                   "WHERE tblOT.Id_OT = @CedulaLogeada " +
                                   "AND tblOT.Consecutivo_Pedido = @NumeroPedido";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CedulaLogeada", id);
                    command.Parameters.AddWithValue("@NumeroPedido", pedido);

                    SqlDataReader reader = command.ExecuteReader();

                    // Verificar si hay filas devueltas por la consulta
                    if (reader.Read())
                    {
                        string mail = reader["Mail"].ToString();
                        TextBox3.Text = mail; // Asignar el valor a TextBox3
                    }
                    else
                    {
                        TextBox3.Text = string.Empty; // Si no hay resultados, establecer el TextBox3 como vacío
                    }

                    reader.Close();
                }
            }
            else
            {
                TextBox3.Text = string.Empty; // Si los parámetros son nulos o vacíos, establecer el TextBox3 como vacío
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
            if (Session["CedulaLogeada"] != null)
            {
                string cedulaLogeada = Session["CedulaLogeada"].ToString();

                // Modificar el SelectCommand del SqlDataSource con la nueva condición
                SqlDataSource5.SelectCommand = "SELECT tblOTObservacion_Receptor.Nombre_Receptor, tblOTObservacion.*, CASE WHEN tblOTObservacion_Receptor.Leida = 1 THEN 'SI' ELSE 'NO' END AS LeidaTexto, tblOTObservacion.FechaObservacion " +
                    "FROM tblOTObservacion " +
                    "INNER JOIN tblOTObservacion_Receptor ON tblOTObservacion.Id_Observacion = tblOTObservacion_Receptor.Id_Observacion " +
                    "WHERE tblOTObservacion_Receptor.Receptor = @CedulaLogeada AND tblOTObservacion_Receptor.Leida = 0 " +
                    "ORDER BY tblOTObservacion.FechaObservacion";

                // Agregar el parámetro para la variable de sesión
                SqlDataSource5.SelectParameters.Clear();
                SqlDataSource5.SelectParameters.Add("CedulaLogeada", cedulaLogeada);

                // Actualizar el DataGrid con la nueva consulta
                DataGrid5.DataSource = SqlDataSource5;
                DataGrid5.DataBind();
            }
        }
        private void EnlazarDataGrid4()
        {
            if (Session["CedulaLogeada"] != null)
            {
                string cedulaLogeada = Session["CedulaLogeada"].ToString();

                // Modificar el SelectCommand del SqlDataSource4 con la nueva condición
                SqlDataSource4.SelectCommand = "SELECT  CASE WHEN B.Leida = 1 THEN 'SI' ELSE 'NO' END AS LeidaTexto, Id_OT, Consecutivo_Pedido, ID_TipoObservacion, FechaObservacion, B.Nombre_Receptor, Emisor, Observacion, Nombre_Obra " +
                                                "FROM tblOTObservacion AS A " +
                                                "INNER JOIN tblOTObservacion_Receptor AS B ON A.Id_Observacion = B.Id_Observacion " +
                                                "WHERE Emisor = @CedulaLogeada AND Leida = '0'";

                // Agregar el parámetro para la variable de sesión
                SqlDataSource4.SelectParameters.Clear();
                SqlDataSource4.SelectParameters.Add("CedulaLogeada", cedulaLogeada);

                // Actualizar el DataGrid4 con la nueva consulta
                DataGrid4.DataBind();
            }
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

        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid5.Items[rowIndex];

            // Obtener el valor de Id_Observacion desde el DataGrid5
            string idObservacion = selectedRow.Cells[8].Text; // Obtener el Id_Observacion

            // Obtener el valor de Id_Observacion desde el DataGrid5
            string nombreobra = selectedRow.Cells[9].Text; // Obtener el Id_Observacion
            TextBox7.Text = nombreobra;

            string observacion = selectedRow.Cells[7].Text;
            TextArea3.InnerText = observacion;

            string cedulaLogeada = Session["CedulaLogeada"].ToString(); // Obtener el valor de la sesión

            UpdateDatabase(idObservacion, cedulaLogeada);


            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid5.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-row";

          

            // Llamar al método para actualizar el DataGrid6
            UpdateDataGrid6(idObservacion);

        }

        // Método para realizar la actualización en la base de datos
        private void UpdateDatabase(string idObservacion, string cedulaLogeada)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Prepara la consulta SQL con parámetros para evitar la inyección de SQL
                string query = "UPDATE tblOTObservacion_Receptor " +
                               "SET Leida = 1, " +
                               "FechaLectura = CONVERT(VARCHAR, GETDATE(), 101) " +
                               "WHERE Id_Observacion = @IdObservacion " +
                               "AND Receptor = @CedulaLogeada";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdObservacion", idObservacion);
                command.Parameters.AddWithValue("@CedulaLogeada", cedulaLogeada);

                int rowsAffected = command.ExecuteNonQuery();

                // Manejar el resultado de la actualización según sea necesario
                if (rowsAffected > 0)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalExito').modal('show');", true);
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
                }
            }
        }


        protected void UpdateDataGrid6(string idObservacion)
        {
            // Realizar la consulta SQL utilizando el idObservacion obtenido
            string query = "SELECT  CASE WHEN Leida = 1 THEN 'SI' ELSE 'NO' END AS LeidaText,* FROM tblOTObservacion_Receptor WHERE id_Observacion = '" + idObservacion + "'";

              string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            DataGrid6.DataSource = dataSet.Tables[0];
            DataGrid6.DataBind();
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
            selectedRow.CssClass = "selected-row4";


        }

        protected void lnkSelectRow6_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid6.Items[rowIndex];



            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid6.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-row6";


        }




    }
}