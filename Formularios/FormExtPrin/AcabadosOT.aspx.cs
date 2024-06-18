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
using System.Reflection.Emit;


namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class AcabadosOT : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";

        private string CadenaConexionISID = "BD_ISIDSQL";

        private string CadenaConexionSSF = "BD_SSF";

        private List<int> ID_Acabados = new List<int>();
        private List<int> ID_GruposObjetoparaAcabados = new List<int>();
        private List<string> Detalles_Adicionales = new List<string>();
        private List<string> AcabadosVentas = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                botonGrabarValidacion();
                CargarDatos();
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }


        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            // Realiza la validación de campos
            string campoFaltante = ValidarCampos();

            if (string.IsNullOrEmpty(campoFaltante))
            {
                string valorLabel = Label3.Text + " el acabado: " + Label5.Text + "-";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#valorLabelSpan').text('" + valorLabel + "'); $('#DefinirAcabado').modal('show');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalll').modal('show');", true);
            }
        }

        protected void BotonSi_Click(object sender, EventArgs e)
        {
            string idOT = Session["Id_OT"]?.ToString();
            string consecutivoPedido = Session["pedido"]?.ToString();

            string valorIDAcabado = Label10.Text;

            string valorIDGrupoParaAcabado = Label8.Text;

            // Obtener el valor del TextBox1 y Label3 + Label5
            string valorTextBox1 = TextBox1.Text;
            string valorLabel = Label3.Text + ":" + Label5.Text + "-" + TextBox1.Text;

            // Verificar si algún campo está vacío o nulo

            // Realizar la inserción en la base de datos
            string consultaInsert = "INSERT INTO tblOTAcabados (Id_OT, Consecutivo_Pedido, ID_Acabado, ID_GrupoObjetoparaAcabado, Detalle_Adicional, AcabadoVentas) " +
                                    "VALUES ('" + idOT + "', '" + consecutivoPedido + "', " + valorIDAcabado + ", " + valorIDGrupoParaAcabado + ", '" + valorTextBox1 + "', '" + valorLabel + "');";

            string cadenaConexion = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open(); // Abre la conexión a la base de datos

                // Crea el comando SQL con la consulta de inserción y la conexión
                using (SqlCommand comando = new SqlCommand(consultaInsert, conexion))
                {
                    int rowsAffected = comando.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        CargarDatos();
                        TextBox1.Text = string.Empty;
                        Label3.Text = string.Empty;
                        Label8.Text = string.Empty;
                        Label5.Text = string.Empty;



                    }
                    else
                    {

                    }

                }

                // Cierra la conexión
                conexion.Close();
            }



        }

        protected void BotonNo_Click(object sender, EventArgs e)
        {

        }

        private string ValidarCampos()
        {
            string campoFaltante = string.Empty;


            if (string.IsNullOrEmpty(Label5.Text))
            {
                campoFaltante = "Acabado Definitivo";
            }

            return campoFaltante;
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
                            Button1.Enabled = true;
                            BtnCopAca.Enabled = true;
                            BtnCopAca.CssClass = "btn shadow btn-light linkButtonClicked2 grande button-enabled";
                            TextBox2.Enabled = true;
                            TextBox2.CssClass = "form-control shadow grande linkButtonClicked button-enabled";
                        }
                        else
                        {
                            Button3.Enabled = false;
                            habilitarLinkButton = false;
                            Button1.Enabled = false;
                            BtnCopAca.Enabled = false;
                            BtnCopAca.CssClass = "btn shadow btn-light linkButtonClicked2 grande button-disabled";
                            TextBox2.Enabled = false;
                            TextBox2.CssClass = "form-control shadow grande linkButtonClicked button-disabled";
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
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
                                    AND O.Consecutivo_Pedido = @ConsecutivoPedido";

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
                        DataGrid1.DataSource = dataTable;
                        DataGrid1.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void DespieceAcabados_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid1.Items[rowIndex];

            Session["Id_OTAcabadosSeleccionado"] = selectedRow.Cells[10].Text;

            Session["Id_GrupoObjetoParaAcabadoSeleccionado"] = selectedRow.Cells[6].Text;

            Session["Id_GrupoAcabadoSeleccionado"] = selectedRow.Cells[8].Text;

            SqlDataSource3.SelectParameters["ID_GrupoObjetoParaAcabado"].DefaultValue = Session["Id_GrupoAcabadoSeleccionado"].ToString();

            DataGrid3.Visible = true;

            foreach (DataGridItem item in DataGrid1.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = "";
                }

            }
            selectedRow.CssClass = "selected-row";

            string idGrupoObjeto = selectedRow.Cells[6].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid2.Items)
            {
                item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                item.Attributes.Remove("data-selected");
            }

            // Encuentra y selecciona la fila deseada
            DataGridItem selectedRowInGrid2 = FindRowInGridByID(DataGrid2, idGrupoObjeto);
            if (selectedRowInGrid2 != null)
            {
                selectedRowInGrid2.CssClass = "selected-roww";
                selectedRowInGrid2.Attributes["data-selected"] = "true";

                string script = "<script>scrollDataGrid();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollDataGrid", script, false);
            }


            string detalleAdicional = selectedRow.Cells[3].Text;

            TextBox1.Text = !string.IsNullOrEmpty(detalleAdicional) && detalleAdicional != "&nbsp;"
                ? detalleAdicional
                : string.Empty;


            Label1.Text = selectedRow.Cells[7].Text;
            Label1.Visible = true;

            Label10.Text = selectedRow.Cells[9].Text;


            Label8.Text = selectedRow.Cells[6].Text;


            Label5.Text = selectedRow.Cells[2].Text;
            Label5.Visible = true;

            Label3.Text = selectedRow.Cells[1].Text;
            Label3.Visible = true;



            DataGrid4.Visible = false;



        }

        private DataGridItem FindRowInGridByID(System.Web.UI.WebControls.DataGrid grid, string id)
        {
            foreach (DataGridItem item in grid.Items)
            {
                if (item.Cells[2].Text == id) // Ajusta el índice según la posición de ID_GrupoObjetoParaAcabado en tu DataGrid
                {
                    return item;
                }
            }
            return null;
        }

        protected void EliminarAcabado_Click(object sender, EventArgs e)
        {
            // Realiza la validación de campos
            string campoFaltante = ValidarEli();

            if (string.IsNullOrEmpty(campoFaltante))
            {
                string valorLabel = Label5.Text + " - aplicado a: " + Label3.Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#valorLabelSpanEliminar').text('" + valorLabel + "'); $('#EliminarAcabado').modal('show');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }


        }

        private string ValidarEli()
        {
            string campoFaltante = string.Empty;


            if (string.IsNullOrEmpty(Label5.Text))
            {
                campoFaltante = "Acabado Definitivo";
            }

            return campoFaltante;
        }

        protected void BotonSiEliminar_Click(object sender, EventArgs e)
        {
            // Definir la cadena de conexión
            string cadenaConexion = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Obtener el valor de id_OTAcabados
            string valor_id_OTAcabados = ObtenerValorId_OTAcabados(); // Ajusta esto según cómo obtienes el valor

            // Construir la consulta DELETE
            string consultaDelete = "DELETE FROM tblotAcabados WHERE Id_OTAcabados = '" + valor_id_OTAcabados + "';";

            // Crear y abrir la conexión
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();

                // Crear el comando SQL con la consulta DELETE y la conexión
                using (SqlCommand comando = new SqlCommand(consultaDelete, conexion))
                {
                    // Ejecutar la consulta DELETE
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        CargarDatos();
                    }
                    else
                    {

                    }
                }
            }
        }

        private string ObtenerValorId_OTAcabados()
        {

            // Verifica si hay un valor almacenado en la variable de sesión
            if (Session["Id_OTAcabadosSeleccionado"] != null)
            {
                // Obtiene el valor almacenado en la variable de sesión
                return Session["Id_OTAcabadosSeleccionado"].ToString();
            }
            else
            {
                // Si no hay un valor en la variable de sesión, devuelve un valor predeterminado
                return "";
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


            Session["Id_GrupoObjetoAcabadoSeleccionado2"] = selectedRow.Cells[2].Text;

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
            string idGrupoObjeto = selectedRowDataGrid2.Cells[2].Text;

            // Modifica dinámicamente la consulta del SqlDataSource4 con el nuevo valor
            SqlDataSource4.SelectCommand = "SELECT tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado, tblGrupodeAcabado.ID_GrupoAcabado, tblGrupodeAcabado.Descripcion_Grupo " +
                                            "FROM tblGrupodeAcabado " +
                                            "INNER JOIN tblGrupoAcab_GrupoObjtAcab ON tblGrupodeAcabado.ID_GrupoAcabado = tblGrupoAcab_GrupoObjtAcab.ID_GrupoAcabado " +
                                            "INNER JOIN tblGrupoObjetoParaAcabado ON tblGrupoAcab_GrupoObjtAcab.ID_GrupoObjetoparaAcabado = tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado " +
                                            "WHERE tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado = '" + idGrupoObjeto + "';";

            // Actualiza el DataGrid4
            DataGrid4.DataBind();
            DataGrid4.Visible = true;





            // Establece la visibilidad de los Labels
            Label3.Visible = true;


            // Obtén el valor de la columna "GrupoObjetoparaAcabado" de la fila seleccionada en DataGrid2
            string grupoObjetoSeleccionado = selectedRowDataGrid2.Cells[1].Text;
            string idgrupoObjetoSeleccionado = selectedRowDataGrid2.Cells[2].Text;

            Label8.Text = idgrupoObjetoSeleccionado;


            // Asigna el valor al Label3
            Label3.Text = grupoObjetoSeleccionado;
            Label3.Visible = true;

            Label1.Visible = false;



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
            Label5.Visible = false;



        }

        protected void lnkSelectRow3_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid3.Items[rowIndex];

            Session["Id_AcabadoSeleccionado"] = selectedRow.Cells[3].Text;

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




            // Establece la visibilidad de los Labels



            DataGridItem selectedRowDataGrid3 = DataGrid3.Items[rowIndex];

            // Obtén el valor de la columna "GrupoObjetoparaAcabado" de la fila seleccionada en DataGrid2
            string descripcionAcabado = selectedRowDataGrid3.Cells[4].Text;
            string idAcabado = selectedRowDataGrid3.Cells[3].Text;

            // Asigna el valor al Label3
            Label10.Text = idAcabado;


            // Asigna el valor al Label3
            Label5.Text = descripcionAcabado;
            Label5.Visible = true;



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

        protected void BtnCopAca_Click(object sender, EventArgs e)
        {
            string id = Session["Id_OT"]?.ToString();
            string pedido = TextBox2.Text;

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pedido))
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT ID_Acabado, ID_GrupoObjetoparaAcabado, Detalle_Adicional, AcabadoVentas " +
                                   "FROM tblOTAcabados " +
                                   "WHERE Id_OT = @Id_OT " +
                                   "AND Consecutivo_Pedido = @Consecutivo_Pedido";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id_OT", id);
                    command.Parameters.AddWithValue("@Consecutivo_Pedido", pedido);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Almacena los valores en listas para cada columna
                        ID_Acabados.Add(Convert.ToInt32(reader["ID_Acabado"]));
                        ID_GruposObjetoparaAcabados.Add(Convert.ToInt32(reader["ID_GrupoObjetoparaAcabado"]));
                        Detalles_Adicionales.Add(reader["Detalle_Adicional"].ToString());
                        AcabadosVentas.Add(reader["AcabadoVentas"].ToString());
                    }

                    reader.Close();

                    // Luego de almacenar todos los datos, procede con la inserción
                    RealizarInserciones();

                }
            }
            else
            {

            }
        }

        protected void RealizarInserciones()
        {
            // Verificar si la lista de ID_Acabados está vacía
            if (ID_Acabados.Count == 0)
            {
                string pedido = TextBox2.Text;
                string contenidoModalOT = "El pedido " + pedido + ", seleccionado para copiar los acabados. No tiene acabados asociados o no existe.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ErrorCopAca').modal('show'); $('#ErrorCopAca2').text('" + contenidoModalOT + "');", true);
            }
            else
            {

                string OTinsertada = Session["Id_OT"]?.ToString();
                string PedidoInsertado = Session["Pedido"]?.ToString();

                if (!string.IsNullOrEmpty(OTinsertada) && !string.IsNullOrEmpty(PedidoInsertado))
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Iterar sobre las listas y realizar inserciones
                        for (int i = 0; i < ID_Acabados.Count; i++)
                        {
                            string query = "INSERT INTO tblOTAcabados (Id_OT, Consecutivo_Pedido, id_Acabado, Id_GrupoObjetoParaAcabado, Detalle_Adicional, AcabadoVentas) " +
                                           "VALUES (@Id_OT, @Consecutivo_Pedido, @ID_Acabado, @ID_GrupoObjetoParaAcabado, @Detalle_Adicional, @AcabadoVentas)";

                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@Id_OT", OTinsertada);
                            command.Parameters.AddWithValue("@Consecutivo_Pedido", PedidoInsertado);
                            command.Parameters.AddWithValue("@ID_Acabado", ID_Acabados[i]);
                            command.Parameters.AddWithValue("@ID_GrupoObjetoParaAcabado", ID_GruposObjetoparaAcabados[i]);
                            command.Parameters.AddWithValue("@Detalle_Adicional", Detalles_Adicionales[i]);
                            command.Parameters.AddWithValue("@AcabadoVentas", AcabadosVentas[i]);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected <= 0)
                            {
                                // Si alguna inserción falla, detenemos el proceso y mostramos un mensaje de error
                                string mensajePersonalizado2 = "No fue posible realizar copiar los acabados";
                                string urlRedireccion2 = "FormExtPrin/AcabadosOT.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado2)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion2)}");
                                return; // Salir del método para evitar más intentos de inserción
                            }
                        }

                        // Si todas las inserciones fueron exitosas, redireccionamos con un mensaje de éxito
                        string mensajePersonalizado = "Se insertaron correctamente los acabados de la OT copiada";
                        string urlRedireccion = "FormExtPrin/AcabadosOT.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                }
                else
                {
                    string pedido = TextBox2.Text;
                    string contenidoModalOT = "El pedido " + pedido + ", seleccionado para copiar los acabados. No tiene acabados asociados o no existe.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ErrorCopAca').modal('show'); $('#ErrorCopAca2').text('" + contenidoModalOT + "');", true);
                }
            }
        }

        protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid2.Items[rowIndex];

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);

            }
        }

        protected void DataGrid4_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid4.Items[rowIndex];

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);

            }
        }

        protected void DataGrid3_ItemCommand1(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid3.Items[rowIndex];

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);

            }
        }
    }
}