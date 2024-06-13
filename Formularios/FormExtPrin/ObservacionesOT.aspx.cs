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
            if (Session["usuariologueado"] != null)
            {
                if (!IsPostBack)
                {
                    CargarDropDownList();

                    // Verificar si la variable de sesión existe antes de acceder a ella
                    if (Session["ValorDeObra"] != null)
                    {
                        string valorDeObra = Session["ValorDeObra"].ToString();
                        tbNombreObra.Text = valorDeObra; // Asignar el valor al TextBox en ObservacionesOT
                        tbNombreObra.Enabled = false;
                        tbNombreObra.CssClass = "form-control";

                    }
                    ConsultarCorreosPorDefecto();
                    ManejarIdOT();
                    ManejarPedido();
                    EnlazarDataGrid();
                    EnlazarDataGrid4();

                    DateTime fecha = DateTime.Now;
                    tbfechaActividad.Text = fecha.ToString("yyyy-MM-dd");
                    tbfechaActividad.Enabled = false;
                    tbfechaActividad.CssClass = "form-control";


                }
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }

            Session.Remove("Id_OT");
            Session.Remove("pedido");

        }

        private void CargarDropDownList()
        {
            DataTable dt = ObtenerDatosParaDropDownList(); // Llamada a un método para obtener los datos

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlTipoObservacion.DataSource = dt;
                ddlTipoObservacion.DataTextField = "TipoObservacion"; // Columna que se mostrará
                ddlTipoObservacion.DataValueField = "id_TipoObservacion"; // Columna para el valor
                ddlTipoObservacion.DataBind();

                // Agregar un elemento por defecto
                ddlTipoObservacion.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" "));

            }
        }

        private DataTable ObtenerDatosParaDropDownList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString))
            {
                string query = "SELECT Id_TipoObservacion,Aplicacion,Descripcion,Aplicacion + ' - ' + Descripcion as TipoObservacion ,DestinatarioPorDefecto,Programable,AlDirectorComercial" +
                               " FROM tblTipoObservacion WHERE UsoEspecifico=0  AND  Activa=1  AND  Aplicacion like '%' ORDER BY Aplicacion ASC , Descripcion ASC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            return dt;
        }

        private void ConsultarCorreosPorDefecto()
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

                    // Se valida si la observacion  viene de solicitudes especiales 
                    if (id.StartsWith("SPE"))
                    {
                        string query = "SELECT E.Mail  FROM tblSoliciDiseEspe AS  SE INNER JOIN tblEmpleado AS E " +
                                       " ON  E.Nombre + ' ' + Apellidos =  SE.Asesor WHERE SE.ID_Solicitud = @Id_Solicitud";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id_Solicitud", id.Substring(3));

                        SqlDataReader reader = command.ExecuteReader();

                        // Verificar si hay filas devueltas por la consulta
                        if (reader.Read())
                        {
                            string mail = reader["Mail"].ToString();
                            tbReceptorCorreo.Text = mail; // Asignar el valor a TextBox3
                        }
                        else
                        {
                            tbReceptorCorreo.Text = string.Empty; // Si no hay resultados, establecer el TextBox3 como vacío
                        }

                        reader.Close();
                    }
                    else
                    {
                        string query = "SELECT  Mail FROM tblAsesorComercial  AS AC " +
                                       "INNER JOIN tblOT AS OT ON OT.Codigo_Asesor = AC.Cedula " +
                                       "WHERE OT.Id_OT = @OT AND Consecutivo_Pedido =  @pedido ";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ot", id);
                        command.Parameters.AddWithValue("@pedido", pedido);
                        SqlDataReader reader = command.ExecuteReader();

                        // Verificar si hay filas devueltas por la consulta
                        if (reader.Read())
                        {
                            string mail = reader["Mail"].ToString();
                            string mailOK = ValidarMail(id, mail);
                            tbReceptorCorreo.Text = mailOK; // Asignar el valor a TextBox3
                        }
                        else
                        {
                            tbReceptorCorreo.Text = string.Empty; // Si no hay resultados, establecer el TextBox3 como vacío
                        }

                        reader.Close();
                    }


                }
            }
            else
            {
                tbReceptorCorreo.Text = string.Empty; // Si los parámetros son nulos o vacíos, establecer el TextBox3 como vacío
            }

            tbReceptorCorreo.Enabled = false;
            tbReceptorCorreo.CssClass = "form-control form-contro-sm";

            tbRecepTipoObs.Enabled = false;
            tbRecepTipoObs.CssClass = "form-control form-contro-sm";
        }

        private string ValidarMail(string idOT, string mail)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            string correos = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT ISNULL(SUBSTRING((SELECT ';' + tblAsesorComercial.Mail FROM tblReporteOT, tblAsesorComercial 
                                WHERE Id_OT = @OT and tblAsesorComercial.Cedula=Codigo_Asesor  and tblAsesorComercial.Activo=1 and Id_OT
                                NOT IN ('0109700','0102000')  GROUP BY Id_OT,Codigo_Asesor,tblAsesorComercial.Mail 
                                FOR XML PATH('')),2,9999), @mail)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OT", idOT);
                command.Parameters.AddWithValue("@mail",mail); 

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    correos = reader[0].ToString();
                }

                reader.Close();
            }

            return correos;
        }

        private void ManejarIdOT()
        {
            string idOT = Session["Id_OT"] as string;
            bool habilitarObservaciones = !string.IsNullOrEmpty(idOT);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "HabilitarObservaciones", $"var habilitarObservaciones = {habilitarObservaciones.ToString().ToLower()};", true);

            if (!string.IsNullOrEmpty(idOT))
            {
                SqlDataSource1.SelectParameters["Id_OT"].DefaultValue = idOT;
                tbOT.Text = idOT;
                tbOT.Enabled = false;
                tbOT.CssClass = "form-control";

            }
        }

        private void ManejarPedido()
        {
            if (Session["pedido"] != null)
            {
                tbPedido.Text = Session["pedido"].ToString();
                tbPedido.Enabled = false;
                tbPedido.CssClass = "form-control";
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

                DataGridItem row = DataGrid1.Items[rowIndex];

                // Se utiliza para darle el color solo a la fila seleccionada 
                foreach (DataGridItem item in DataGrid1.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada";


                if (rowIndex >= 0 && rowIndex < DataGrid1.Items.Count)
                {
                    // Obtiene el valor de la columna "Observacion" en la fila seleccionada
                    string observacion = DataGrid1.Items[rowIndex].Cells[7].Text;

                    // Obtiene los valores de "Id_OT" y "Consecutivo_Pedido" de la fila seleccionada
                    string idOT = DataGrid1.Items[rowIndex].Cells[1].Text;
                    string consecutivoPedido = DataGrid1.Items[rowIndex].Cells[2].Text;

                    // Asigna la observación al textarea
                    txObservacion.Value = observacion;


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


        // Grabar Observacion 
        protected void GrabarObservacion_Click(object sender, EventArgs e)
        {
            // Validar que tenga los campos necesarios
            if (ValidarCamposRequeridos())
            {
                // Se realiza la Insercion de la Observacion   
                InsertarObservacion();
                DataGrid1.DataBind();

                // Traemos el Id_MaxObservacion 
                string Id_Observacion = ConsultarId_Observacion();

                // Se valida si hay receptores seleccionados             
                if(tbCedulaRecp.Text != "")
                {
                    string cedulasNotificar = tbCedulaRecp.Text.Trim(';');
                    string NombresNotificar = tbNombreRecp.Text.Trim(';');

                    string[] CedNot = cedulasNotificar.Split(';');
                    string[] NomNot = NombresNotificar.Split(';');

                    for (int i = 0; i < CedNot.Length; i++)
                    {
                        // Llamamos al método InsertarObservacionReceptores con la cédula y el nombre actuales
                         InsertarObservacionReceptores(Id_Observacion, CedNot[i], NomNot[i]);

                    }

                    // Mostrar mensaje de éxito
                    string script1 = "alert('La observación ha sido grabada para los siguientes usuarios del sistema: " + tbNombreRecp.Text.Replace(";"," - ") + @"');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", script1, true);

                }



                //Consultamos el area de aplicacion  con el codigo del ddlTipoObservacion              
                string Aplicacion = ConsultarAreaAplicacion();

                //Enviar la notificacion por Correo 
                string destinatarios = (tbReceptorCorreo.Text + ";" + tbRecepTipoObs.Text).Trim(';').Trim(' ');
                string cuerpo = @"
                    <!DOCTYPE html>
                    <html lang='es'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                line-height: 1.6;
                                margin: 0;
                                padding: 0;
                                background-color: #f9f9f9;
                            }
                            .container {
                                max-width: 37rem;
                                margin: 20px auto;
                                padding: 20px;
                                border: 1px solid #ccc;
                                border-radius: 5px;
                                background-color: #fff;
                            }
                            h2 {
                                color: #333;
                                font-size: 24px;
                                margin-bottom: 20px;
                            }
                            p {
                                margin-bottom: 10px;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Notificación de Observación </h2>
                            <p> <strong> Fecha de Observación: </strong> " + DateTime.Now.ToString() + @"</p>
                            <p><strong> Emisor : </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>
                            <p><strong>Nombre de la Obra: </strong> " + tbNombreObra.Text + @"</p>
                            <p><strong>Tipo Observacion : </strong> " + ddlTipoObservacion.SelectedItem.Text + @"</p>
                            <p><strong>Detalle Observación: </strong> " + txObservacion.InnerText + @"</p>
                            <p><strong>Fin Observación </strong> </p>
                           
                        </div>
                    </body>
                    </html>";

                //ejecutar el procedimiento almacenado que envia el correo 
                EnviarCorreoReproceso(destinatarios, cuerpo, Aplicacion);

                CargarDropDownList();

                txObservacion.InnerText = "";
            }


        }

        public void EnviarCorreoReproceso(string destinatarios, string cuerpo, string aplicacion)
        {
            string nombreProcedimiento = "duc_sp_Correo";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(nombreProcedimiento, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Definir los parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Destinatarios", destinatarios);
                    command.Parameters.AddWithValue("@asunto", "Observacion: " + aplicacion + " APL: " + tbOT.Text + "-" + tbPedido.Text);
                    command.Parameters.AddWithValue("@cuerpo", cuerpo);
                    command.Parameters.AddWithValue("@adjuntos", "");
                    command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        // Mostrar mensaje  de exito del envio de correo 
                        string script2 = "alert('La notificación de la observación ha sido enviada por correo electronico a los  destinatarios " + tbReceptorCorreo.Text.Replace(";", " - ") + @"');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess2", script2, true);

                    }
                    catch (SqlException ex)
                    {
                        // Manejar la excepción (opcional)
                        //error.Visible = true;
                        // error.Text = ex.Message;
                    }
                }
            }
        }

        private bool ValidarCamposRequeridos()
        {
            bool valido = true;

            if (ddlTipoObservacion.SelectedValue == " ")
            {
                // Mensaje de alerta
                string script1 = "alert('Por favor seleccione el tipo de observación.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", script1, true);
                valido = false;
            }

            if (txObservacion.InnerText == "")
            {
                // Mensaje de alerta
                string script1 = "alert('Por favor escriba  la justificaci{on de la observación.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", script1, true);
                valido = false;
            }

            if (tbOT.Text == "")
            {
                // Mensaje de alerta
                string script1 = "alert('No se ha seleccionado una OT o una Solicitud Especial para generar una observacion');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", script1, true);
                valido = false;
            }


            return valido;
        }

        public string ConsultarAreaAplicacion()
        {
            string areaAplicacion = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Aplicacion FROM tblTipoObservacion WHERE UsoEspecifico=0  AND  Activa=1  AND  Aplicacion like '%' AND ID_TipoObservacion = @TipoObs";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TipoObs", ddlTipoObservacion.SelectedValue);


                    try
                    {
                        connection.Open();
                        areaAplicacion = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return areaAplicacion;
        }

        public string ConsultarMailReproceso()
        {
            string mailResponsable = null;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            string query = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cedula", "");

                    connection.Open();
                    object result = command.ExecuteScalar();

                    // Verificar si el resultado no es nulo
                    if (result != null)
                    {
                        // Convertir el resultado a string
                        mailResponsable = result.ToString();
                    }
                }
            }

            return mailResponsable;
        }

        protected void DataGridReceptorMail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerMail")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridReceptorMail.Items[rowIndex];

                string Nombre = row.Cells[2].Text;
                string mailAgregar = row.Cells[3].Text;
                string cedula = row.Cells[4].Text;


                // Tomamos los mail ya agregados y las cedulas agregadas
                string MailAgregados = tbReceptorCorreo.Text;
                string cedulaAgregadas = tbCedulaRecp.Text;
                string NombreAgregado = tbNombreRecp.Text;

                if (!MailAgregados.Contains(mailAgregar))
                {
                    //Agregamos el correo del  receptor
                    tbReceptorCorreo.Text = MailAgregados + ";" + mailAgregar;

                    // Agregamos la cedula del Receptor 

                    if (tbCedulaRecp.Text != "")
                    {
                        tbCedulaRecp.Text = cedulaAgregadas + ";" + cedula;
                        tbNombreRecp.Text = NombreAgregado + ";" + Nombre;
                    }
                    else
                    {
                        tbCedulaRecp.Text = cedula;
                        tbNombreRecp.Text = Nombre;
                    }


                    //se usa Para darle un color a la fila seleccionada  
                    e.Item.CssClass = "fila-seleccionada";
                }
                else
                {
                    // Eliminamos el correo del receptor  
                    tbReceptorCorreo.Text = MailAgregados.Replace(";" + mailAgregar, "");

                    // Eliminamos la cedula del receptor 
                    tbCedulaRecp.Text = cedulaAgregadas.Replace(cedula, "").TrimEnd(';').Replace(";;", ";");

                    // Eliminamos el  nombre  del receptor 
                    tbNombreRecp.Text = NombreAgregado.Replace(Nombre, "").TrimEnd(';').Replace(";;", ";");


                    e.Item.CssClass = "fila-seleccionada1";
                }

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


            }

        }

        public void InsertarObservacion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblOtObservacion (Id_OT,Consecutivo_Pedido,Nombre_Obra,Observacion,FechaObservacion,Emisor,Nombre_Emisor," +
                               "ID_TipoObservacion,FechaAnteriorDespacho,FechaNuevaDespacho,CedulaAsesor, FechaActividad,  Destinatarios) " +
                               "VALUES(@OT, @Pedido, @NombreObra, @Observacion, @fechaObsercion,@Emisor, @Nombre_Emisor, @ID_TipoObservacion," +
                               " @FechaAnteriorDespacho,@FechaNuevaDespacho, @CedulaAsesor, @FechaActividad,@Destinatarios)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OT", tbOT.Text);
                    command.Parameters.AddWithValue("@Pedido", tbPedido.Text);
                    command.Parameters.AddWithValue("@NombreObra", tbNombreObra.Text);
                    command.Parameters.AddWithValue("@Observacion", txObservacion.InnerText);
                    command.Parameters.AddWithValue("@fechaObsercion", DateTime.Now);
                    command.Parameters.AddWithValue("@Emisor", Session["CedulaLogeada"].ToString());
                    command.Parameters.AddWithValue("@Nombre_Emisor", Session["usuariologueado"].ToString());
                    command.Parameters.AddWithValue("@ID_TipoObservacion", ddlTipoObservacion.SelectedValue);
                    command.Parameters.AddWithValue("@FechaAnteriorDespacho", DateTime.Now);
                    command.Parameters.AddWithValue("@FechaNuevaDespacho", DateTime.Now);
                    command.Parameters.AddWithValue("@CedulaAsesor", Session["CedulaLogeada"].ToString());
                    command.Parameters.AddWithValue("@FechaActividad", tbfechaActividad.Text);
                    command.Parameters.AddWithValue("@Destinatarios", tbReceptorCorreo.Text);


                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Cambiar el mensaje de error
                        //Console.WriteLine("Error al insertar datos: " + ex.Message);
                    }
                }
            }
        }

        public string ConsultarId_Observacion()
        {
            string Id_Observacion = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select max(id_Observacion) from tblOTObservacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        Id_Observacion = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción 
                        //Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }

            return Id_Observacion;
        }

        public void InsertarObservacionReceptores(string Id_Observacion, string cedula, string nombre)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblOTObservacion_Receptor (Id_Observacion,Receptor,Nombre_Receptor) Values(@ID_Observacion, @CedulaRecep , @NombreReceptor)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Observacion", Id_Observacion);
                    command.Parameters.AddWithValue("@CedulaRecep", cedula);
                    command.Parameters.AddWithValue("@NombreReceptor", nombre);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Cambiar el mensaje de error
                        // Console.WriteLine("Error al insertar datos: " + ex.Message);
                    }
                }
            }
        }

        protected void ddlTipoObservacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CorreoTipoObser = ConsultarCorreoPorTipoObservacion();

            tbRecepTipoObs.Text = "";
            tbRecepTipoObs.Text = CorreoTipoObser;

        }

        protected string ConsultarCorreoPorTipoObservacion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            string correo = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DestinatarioPorDefecto FROM tblTipoObservacion " +
                                   "WHERE UsoEspecifico = 0 AND Activa = 1 AND ID_TipoObservacion = @IdObservacion";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IdObservacion", ddlTipoObservacion.SelectedValue);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        correo = reader["DestinatarioPorDefecto"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo, loguear el error
                // También puedes lanzar una excepción o devolver un mensaje de error
            }

            return correo;
        }


    }
}