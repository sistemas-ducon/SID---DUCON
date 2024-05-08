 using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Web.Services;
using System.IO;
using Button = System.Web.UI.WebControls.Button;
using System.Diagnostics;
using DocumentFormat.OpenXml.Office2010.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Media.TextFormatting;
using System.Globalization;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Xml;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Reflection.Emit;
using System.Drawing;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Text;
using NPOI.XSSF.UserModel;


namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Diseño_Venta : System.Web.UI.Page
    {

        private string CadenaConexionSID = "BD_SIDSQL";

        private string CadenaConexionISID = "BD_ISIDSQL";

        private string CadenaConexionSSF = "BD_SSF";

        private bool isModalVisible = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {


                if (!IsPostBack)
                {
                    cargarSiempre();

                    string tipoAccion = Session["Diseno"] as string;
                    if (tipoAccion == "Ventas")
                    {
                        Page_LoadVentas();
                    }
                    else if (tipoAccion == "Recepcion")
                    {
                        Page_LoadRecep();
                    }
                    else if (tipoAccion == "Diseño")
                    {
                        Page_LoadDiseño();
                    }
                }
            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
        }

        protected void cargarSiempre()
        {
            if (Session["ZonaLogeada"] != null)
            {
                string zonaLogeada = Session["ZonaLogeada"].ToString();
                DropDownListOptions.SelectedValue = zonaLogeada;     
            }
            ChecUrgent.Enabled = false;
            ApplyButtonStyles();
            DropDownList1.DataBind();
            DropDownList1.Items.Insert(0, new ListItem(""));
            TextCiuPro.DataBind();
            TextCiuPro.Items.Insert(0, new ListItem(""));
            habilitarbotonesDise();
            DeshabilitarDivYContenido(miDiv);
            CheckBox22.Checked = isModalVisible;



            LinkButton1.Enabled = false;
            LinkButton1.CssClass = "btn btn-sm button-disabled";

            LinkButton3.Enabled = false;
            LinkButton3.CssClass = "btn btn-sm button-disabled";

            LinkButton5.Enabled = false;
            LinkButton5.CssClass = "btn btn-sm button-disabled";

            LinkButton4.Enabled = false;
            LinkButton4.CssClass = "btn btn-sm button-disabled";

            LinkButton2.Enabled = false;
            LinkButton2.CssClass = "btn btn-sm button-disabled";

            Session.Remove("EventoItemCommandEjecutado");

        }

        protected void Page_LoadVentas()
        {
            elementosllenosalcargarlapagina();

            if (Session["NumeroDiseño2"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño2"].ToString()))
            {                 
                    AccionesAlCargarDiseño();
                    ProcesarNumeroDiseño2(null);     
                    scripTabDise();
            }
            else
            {
                // Manejar el caso cuando Session["NumeroDiseño"] es null o vacío
            }

            if (Session["Id_ClienteBD"] != null && !string.IsNullOrEmpty(Session["Id_ClienteBD"].ToString()))
            {

                if (Session["ID_ContactoBD"] != null && !string.IsNullOrEmpty(Session["ID_ContactoBD"].ToString()))
                {
                        scripTabDise();

                        NuevoLimpiar();
                    }
                else
                {
                    Session.Remove("Id_ClienteBD");

                    elementosllenosalcargarlapagina();

                    Session.Remove("lnkClieClicked");
                    Session.Remove("lnkClieeClicked");
                }
            }
            else
            {
                Session.Remove("lnkClieClicked");
                Session.Remove("lnkClieeClicked");
            }

            CargarClienteYContacto();

            CargarDatagridVentas();

            Session.Remove("NumeroDiseño5");
            Session.Remove("NumeroDiseño2");
            Session.Remove("SelectedIdOT");
            Session.Remove("SelectedFileName");
            Session.Remove("Documentacion");
          
        }

        protected void CargarDatagridDise()
        {
            DataGridOTsDise();
            DataGridDiseD();
            DataGridSCDise();
            DataGridRenderDise();
            ResumenDibujanteDibujo();
            UpdateDataGrids();
        }

        protected void DataGridOTsDise()
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ProBitacoraOTs", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro de zona si es necesario
                string selectedValue = DropDownListOptions.SelectedValue;
                if (!string.IsNullOrEmpty(selectedValue) && selectedValue != "%")
                {
                    command.Parameters.AddWithValue("@Zona", selectedValue);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGrid1.DataSource = dataTable;
            DataGrid1.DataBind();
        }

        protected void DataGridDiseD()
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT [Numero_Diseño], [Cliente], [Nombre_Diseño], [Asesor],
                            [UltimaActivacion], [Fecha_Programada_Entrega], [RealizadoPor],
                            A.[Zona], [PactodeEntrega], [ProgramadoVentas], [PasarACotizar], [TerminadoDibujo], [Pausado], B.[Cedula], A.[id_CiudadProyecto], A.[Urgente]
                        FROM [tblDiseño] AS A 
                            INNER JOIN tblAsesorComercial AS B ON (B.Nombre +' '+ B.Apellidos) = A.Asesor
                        WHERE ProgramadoVentas = '1' AND TerminadoDibujo = '0' AND B.Zona LIKE @Zona
                        ORDER BY [UltimaActivacion] ASC";

                SqlCommand command = new SqlCommand(query, connection);

                // Agregar parámetro de zona si es necesario
                string selectedValue = DropDownListOptions.SelectedValue;
                if (!string.IsNullOrEmpty(selectedValue) && selectedValue != "%")
                {
                    command.Parameters.AddWithValue("@Zona", selectedValue);
                }
                else
                {
                    // En caso de que no se seleccione ninguna zona específica, se usa '%' para que coincida con todas las zonas
                    command.Parameters.AddWithValue("@Zona", "%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGrid2.DataSource = dataTable;
        }

        protected void DataGridSCDise()
        {

            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ProBitacoraShowCase", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro de zona si es necesario
                string selectedValue = DropDownListOptions.SelectedValue;
                if (!string.IsNullOrEmpty(selectedValue) && selectedValue != "%")
                {
                    command.Parameters.AddWithValue("@Zona", selectedValue);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGridDiseños.DataSource = dataTable;
        }

        protected void DataGridRenderDise()
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ProBitacoraRender", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro de zona si es necesario
                string selectedValue = DropDownListOptions.SelectedValue;
                if (!string.IsNullOrEmpty(selectedValue) && selectedValue != "%")
                {
                    command.Parameters.AddWithValue("@Zona", selectedValue);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGridRender.DataSource = dataTable;
        }

        protected void ResumenDibujanteDibujo()
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                            string query = @"SELECT D.RealizadoPor, 
                       COALESCE(O.CantidadOt, 0) AS CantidadOt, 
                       COALESCE(C.CantidadRepeticiones, 0) AS CantidadRepeticiones,
                       COALESCE(O.CantidadOt, 0) + COALESCE(C.CantidadRepeticiones, 0) AS Total
                FROM (SELECT DISTINCT RealizadoPor FROM tblDiseño) D
                LEFT JOIN
                (SELECT ot.RealizadoPor, COUNT(*) AS CantidadOt
                    FROM tblOT ot
                    INNER JOIN tblAsesorComercial ac ON ot.Codigo_Asesor = ac.Cedula
                    WHERE ot.Terminado_Diseño = '0' 
                      AND ot.Terminado_Ventas = '1' 
                      AND ot.Anulada = '0'
                     AND OT.Zona = @Zona
                    GROUP BY ot.RealizadoPor) O
                ON D.RealizadoPor = O.RealizadoPor
                LEFT JOIN
                (SELECT A.RealizadoPor, COUNT(*) AS CantidadRepeticiones
                    FROM [tblDiseño] AS A
                    INNER JOIN tblAsesorComercial AS B ON (B.Nombre + ' ' + B.Apellidos) = A.Asesor
                    WHERE ProgramadoVentas = '1' and TerminadoDibujo = '0' AND A.Zona LIKE @Zona
                    GROUP BY A.RealizadoPor) C
                ON D.RealizadoPor = C.RealizadoPor
                ORDER BY Total DESC;";

                SqlCommand command = new SqlCommand(query, connection);

                // Agregar parámetro de zona si es necesario
                string selectedValue = DropDownListOptions.SelectedValue;
                if (!string.IsNullOrEmpty(selectedValue) && selectedValue != "%")
                {
                    command.Parameters.AddWithValue("@Zona", selectedValue);
                }
                else
                {
                    // En caso de que no se seleccione ninguna zona específica, se usa '%' para que coincida con todas las zonas
                    command.Parameters.AddWithValue("@Zona", "%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGrid3.DataSource = dataTable;
            DataGrid3.DataBind();
        }

        protected void DataGridOts(string cedula)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ProBitacoraOTs", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro @Cedula y asignarle el valor proporcionado
                command.Parameters.AddWithValue("@Cedula", cedula);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGrid1.DataSource = dataTable;
        }

        protected void DataGridDise(string cedula)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ProBitacoraDise", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro @Cedula y asignarle el valor proporcionado
                command.Parameters.AddWithValue("@Cedula", cedula);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGrid2.DataSource = dataTable;
     
        }

        protected void DataGridRenderF(string cedula)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ProBitacoraRender", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro @Cedula y asignarle el valor proporcionado
                command.Parameters.AddWithValue("@Cedula", cedula);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGridRender.DataSource = dataTable;

        }

        protected void CargarDatagridVentas()
        {
            string cedula = Session["CedulaLogeada"].ToString();
            string usuariologueado = Session["usuariologueado"].ToString();

            DataGridOts(cedula);
            DataGridDise(cedula);
            DataGridSC(usuariologueado);
            DataGridRenderF(cedula);
            ResumenDibujanteVentas(cedula);
            UpdateDataGrids();  
        }

        protected void ResumenDibujanteVentas(string cedula)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ResumenDibujante", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro @Cedula y asignarle el valor proporcionado
                command.Parameters.AddWithValue("@CodigoAsesor", cedula);

                // Agregar parámetro de zona si es necesario
                string selectedValue = DropDownListOptions.SelectedValue;
                if (!string.IsNullOrEmpty(selectedValue) && selectedValue != "%")
                {
                    command.Parameters.AddWithValue("@Zona", selectedValue);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGrid3.DataSource = dataTable;
            DataGrid3.DataBind();
        }

        protected void DataGridSC(string usuariologueado)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ProBitacoraShowCase", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro @Cedula y asignarle el valor proporcionado
                command.Parameters.AddWithValue("@NombreUsuario", usuariologueado);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }
            DataGridDiseños.DataSource = dataTable;
            DataGridDiseños.DataBind();

        }

        protected void scripTabDise()
        {
            string script = @"
            <script type='text/javascript'>
                window.onload = function () {
                    // Obtener el elemento del tab deseado
                    var tab = document.getElementById('Diseño-BitacoraFPV-001-tab');
                    
                    // Hacer clic en el tab deseado
                    tab.click();
                    
                    // Ocultar el tab actual si es necesario
                    var activeTab = document.querySelector('.nav-item .active');
                    if (activeTab) {
                        activeTab.classList.remove('active');
                    }
                    
                    // Agregar la clase 'active' al tab deseado
                    tab.classList.add('active');
                };
            </script>";

            // Registrar el script en el cliente
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SwitchToDesiredTab", script);
        }

        protected void Page_LoadRecep()
        {
            elementosllenosalcargarlapagina();
            scripTabDise();
            habilitarbotonesRece();
            BtnProgramar.Text = "TERMINAR";
            ValidarBotonTerminarRecep();

            if (Session["NumeroDiseño2"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño2"].ToString()))
            {           
                    ProcesarNumeroDiseño2(null);                       
            }

        }

        protected void Page_LoadDiseño()
        {
            elementosllenosalcargarlapagina();
            CargarDatagridDise();
            BtnProgramar.Text = "TERMINAR";

            if (Session["NumeroDiseño2"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño2"].ToString()))
            {
                ProcesarNumeroDiseño2(null);
            }
        }

        protected void AccionesAlCargarDiseño()
        {
            NuevoDisBit.Enabled = true;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;
            Modificar.CssClass = "btn btn-sm shadow button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

            ValidarBotonOk();

            DeshabilitarDivYContenido(miDiv);

            Session["lnkClieClicked"] = true;
        }

        protected void CargarOT_Click(object sender, EventArgs e)
        {
            Session.Remove("lnkClieClicked");
            Session.Remove("lnkClieeClicked");

            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid1.Items[rowIndex];

            // Almacena el valor de Id_OT en una variable de sesión
            Session["Id_OT2"] = selectedRow.Cells[2].Text;

            // Almacena el nombre del archivo en la variable de sesión
            Session["pedido2"] = selectedRow.Cells[3].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid1.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-row";

            string mensajePersonalizado = "Se cargara la OT seleccionada";
            string urlRedireccion = "OrdenTrabajo.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");



        }

        protected void BtnProgramar_Click(object sender, EventArgs e)
        {
            string tipoAccion = Session["Diseno"] as string;
            if (tipoAccion == "Ventas")
            {
                string diseño = lblNumDise.Text;

                string contenidoModalOT = "Una Vez aprobado el Diseño, no podra realizarle modificaciones, esta seguro de aprobar el diseño: " + diseño + ", para dibujo y despiece?";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ProgramarDiseño').modal('show'); $('#ProgramarDiseño2').text('" + contenidoModalOT + "');", true);

            }
            else if (tipoAccion == "Recepcion")
            {
                string diseño = lblNumDise.Text + " - ";
                string nombreDiseño = TextBox5.Text;

                string contenidoModalOT = "Esta seguro de terminar el diseño: " + diseño + nombreDiseño + " en el proceso de cotizacion?";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ProgramarDiseñoCotizacion').modal('show'); $('#ProgramarDiseñoCotizacion2').text('" + contenidoModalOT + "');", true);
            }

          

        }

        protected void ProgramarDiseño_Click(object sender, EventArgs e)
        {


            // Obtener la fecha programada de entrega desde el TextBox TextEntrega
            DateTime fechaActual = DateTime.Now;

            // Obtener el valor del número de diseño desde el label
            string numeroDiseño = lblNumDise.Text;

            if (ValidarExistenciaDiseño(numeroDiseño))
            {
                DateTime fechaIngresoDiseño = DateTime.Parse(TextIngDis.Text);

                // Obtener la fecha de activación desde el TextBox TextUltAc
                DateTime fechaActivacion = DateTime.Parse(TextUltAc.Text);


                // Sumar 3 días hábiles a partir de la fecha actual
                DateTime fechaProgramadaEntrega = ObtenerProximaFechaHabil(fechaActual, 3);

                // Asignar la fecha programada de entrega al TextBox
                TextEntrega.Text = fechaProgramadaEntrega.ToString("yyyy-MM-ddTHH:mm");

                // Comparación y actualización de la fecha de SC
                DateTime dtpFechaSC = DateTime.Parse(TextFec.Text);

                if (fechaProgramadaEntrega > dtpFechaSC)
                {
                    dtpFechaSC = fechaProgramadaEntrega.Date;
                    // Aquí puedes decidir si también deseas actualizar dtpHoraSC
                }

                // Realizar la actualización en la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE tblDiseño SET ProgramadoVentas = 1, Fecha_Ingreso = @FechaIngreso, Fecha_Programada_Entrega = @FechaProgramadaEntrega, UltimaActivacion = @UltimaActivacion, PactodeEntrega = @PactodeEntrega WHERE Numero_Diseño = @NumeroDiseño";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);
                        cmd.Parameters.AddWithValue("@FechaIngreso", fechaIngresoDiseño);
                        cmd.Parameters.AddWithValue("@FechaProgramadaEntrega", fechaProgramadaEntrega);
                        cmd.Parameters.AddWithValue("@UltimaActivacion", fechaActivacion);
                        cmd.Parameters.AddWithValue("@PactodeEntrega", fechaProgramadaEntrega);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Session["NumeroDiseño2"] = numeroDiseño;


                            string mensajePersonalizado = "Diseño Programado exitosamente!";
                            string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                            // Si se actualizaron filas, se deshabilita el botón y se cambia su clase CSS
                            BtnProgramar.Enabled = false;
                            BtnProgramar.CssClass = "button-disabled form-control fw-bold";
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);

                            // Si no se actualizaron filas, se mantiene el botón habilitado y se cambia su clase CSS
                            BtnProgramar.Enabled = true;
                            BtnProgramar.CssClass = "button-enabled form-control fw-bold";
                        }

                    }

                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
        }

        protected void NOProgramarDiseño_Click(object sender, EventArgs e)
        {
            string numeroDiseño = lblNumDise.Text;

            Session["NumeroDiseño2"] = numeroDiseño;


            string mensajePersonalizado = "El diseño no fue programardo";
            string urlRedireccion = "Ventas/Diseño_Venta.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

        }

        private bool ValidarExistenciaDiseño(string numeroDiseño)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string selectQuery = "SELECT COUNT(*) FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseño";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);
                    connection.Open();
                    int rowCount = (int)cmd.ExecuteScalar();

                    return rowCount > 0; // Retorna true si el diseño existe, false de lo contrario
                }
            }
        }

        protected void ProgramarDiseñoCotizacion_Click(object sender, EventArgs e)
        {
            string diseño = lblNumDise.Text;

            // Actualizar la tabla tblDiseño estableciendo CotizaciónOK en 1
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "UPDATE tblDiseño SET CotizaciónOK = 1 WHERE Numero_Diseño = @Numero_Diseño";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Numero_Diseño", diseño);
                    connection.Open();            
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["NumeroDiseño2"] = diseño;


                        string mensajePersonalizado = "Diseño Terminado exitosamente!";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                        // Si se actualizaron filas, se deshabilita el botón y se cambia su clase CSS
                        BtnProgramar.Enabled = false;
                        BtnProgramar.CssClass = "button-disabled form-control fw-bold";
                    }
                    else
                    {

                    }

                }
            }
        }

        private DateTime ObtenerProximaFechaHabil(DateTime fecha, int cantidadDias)
        {
            int diasHabilesAgregados = 0;

            while (diasHabilesAgregados < cantidadDias)
            {
                // Sumar un día
                fecha = fecha.AddDays(1);

                // Verificar si el día actual no es sábado ni domingo
                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                {
                    // Consultar si la fecha está en la tabla tblDiaNoLaboral
                    bool esDiaNoLaboral = EsDiaNoLaboral(fecha);

                    if (!esDiaNoLaboral)
                    {
                        // Si es un día hábil y no es un día no laboral, incrementar el contador de días hábiles agregados
                        diasHabilesAgregados++;
                    }

                }
            }

            return fecha;
        }

        private bool EsDiaNoLaboral(DateTime fecha)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();

                string consulta = "SELECT COUNT(*) FROM tblDiaNoLaboral WHERE dnlFecha = @Fecha";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@Fecha", fecha.Date);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        protected void SiButton_Click(object sender, EventArgs e)
        {
            // Obtener la fecha y hora actual
            DateTime now = DateTime.Now;

            string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextUltAc.Text = fechaHoraActual;


            DateTime fechaActual = DateTime.Now;

            // Sumar 3 días hábiles a partir de la fecha actual
            DateTime fechaProgramadaEntrega = ObtenerProximaFechaHabil(fechaActual, 3);

            // Asignar la fecha programada de entrega al TextBox
            TextEntrega.Text = fechaProgramadaEntrega.ToString("yyyy-MM-ddTHH:mm");



            string fechaInDis = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextIngDis.Text = fechaInDis;

            string fechaOkDib = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextFecOkDib.Text = fechaOkDib;


            // Limpiar el contenido del TextBox
            TextProyecto.Text = string.Empty;
            TextCliente.Text = string.Empty;
            TextDir.Text = string.Empty;
            TextPla.Text = string.Empty;
            TextContacto.Text = string.Empty;
            TextTel.Text = string.Empty;
            TextMail.Text = string.Empty;
            //TextCiuPro.Text = string.Empty;
            TexHTot.Text = string.Empty;
            TextLin.Text = string.Empty;
            TextMos.Text = string.Empty;
            TextSup.Text = string.Empty;
            TextSop.Text = string.Empty;
            TextGav.Text = string.Empty;
            TextPan.Text = string.Empty;
            TextTapPie.Text = string.Empty;
            TextRep.Text = string.Empty;
            TextTipVid.Text = string.Empty;
            TextPant.Text = string.Empty;
            TextArch.Text = string.Empty;
            TextCoc.Text = string.Empty;
            TextEnt.Text = string.Empty;
            TextPuer.Text = string.Empty;
            TextObsVen.Value = string.Empty;
            TextObsDibDes.Value = string.Empty;
            TextSegPauDev.Value = string.Empty;



            lblNumDise.Text = "Por definir";

            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "form-control form-control-sm fw-bold";

            TextFec.Enabled = false;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = false;
            TextFech.CssClass = "form-control form-control-sm";

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";


            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Session["EventoItemCommandEjecutado"] = false;
            Session.Remove("lnkClieClicked");
            Session.Remove("lnkClieeClicked");
        }

        protected void NoButton_Click(object sender, EventArgs e)
        {
            string diseño = lblNumDise.Text;

            Session["NumeroDiseño5"] = diseño;

            // Obtener la fecha y hora actual
            DateTime now = DateTime.Now;

            string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextUltAc.Text = fechaHoraActual;


            DateTime fechaActual = DateTime.Now;

            // Sumar 3 días hábiles a partir de la fecha actual
            DateTime fechaProgramadaEntrega = ObtenerProximaFechaHabil(fechaActual, 3);

            // Asignar la fecha programada de entrega al TextBox
            TextEntrega.Text = fechaProgramadaEntrega.ToString("yyyy-MM-ddTHH:mm");


            string fechaInDis = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextIngDis.Text = fechaInDis;

            string fechaOkDib = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextFecOkDib.Text = fechaOkDib;

            TextObsDibDes.Value = string.Empty;
            TextSegPauDev.Value = string.Empty;


            lblNumDise.Text = "Por definir";

            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "form-control form-control-sm fw-bold";

            TextFec.Enabled = false;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = false;
            TextFech.CssClass = "form-control form-control-sm";

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";


            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

        }

        private void ApplyButtonStyles()
        {
            ApplyButtonStyle(NuevoDisBit);
            ApplyButtonStyle(Grabar);
            ApplyButtonStyle(Modificar);
            ApplyButtonStyle(DocBitacora);
            ApplyButtonStyle(RegresarDiseño);
            ApplyButtonStyle(AdicionarElemento);
            ApplyButtonStyle(ActualizarDiseno);
            ApplyButtonStyle(PausarDiseño);
            ApplyButtonStyle(Cancelar);
            ApplyButtonStyle(EliminarDiseño);

        }

        private void ApplyButtonStyle(WebControl button)
        {
            button.CssClass = button.Enabled ? "button-enabled" : "button-disabled";
        }

        protected override void OnInit(EventArgs e)
        {
            ViewState["Accion"] = "Insercion";
            base.OnInit(e);
        }

        private void CargarClienteYContacto()
        {
            if (!IsPostBack)
            {
                string IdCLiente = Session["Id_ClienteBD"]?.ToString();
                string IdContaco = Session["ID_ContactoBD"]?.ToString();

                if (!string.IsNullOrEmpty(IdCLiente) && !string.IsNullOrEmpty(IdContaco))
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "SELECT X.NombreCompañía, X.Teléfono, Y.NombreContacto, Y.MailContacto, Y.Telefono, Y.Celular, X.Dirección " +
                                       "FROM tblCliente AS X " +
                                       "INNER JOIN tblClienteContacto AS Y ON Y.Id_Cliente = X.Id_Cliente " +
                                       "WHERE X.Id_Cliente = @ParametroCliente AND Y.Id_ClienteContacto = @ParametroClienteContacto";



                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ParametroCliente", IdCLiente);
                            command.Parameters.AddWithValue("@ParametroClienteContacto", IdContaco);

                            connection.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreCompañía")))
                                    {
                                        TextCliente.Text = reader["NombreCompañía"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Dirección")))
                                    {
                                        TextDir.Text = reader["Dirección"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreContacto")))
                                    {
                                        TextContacto.Text = reader["NombreContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("MailContacto")))
                                    {
                                        TextMail.Text = reader["MailContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Telefono")))
                                    {
                                        TextTel.Text = reader["Telefono"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Celular")))
                                    {
                                        TextCel.Text = reader["Celular"].ToString();
                                    }
                                }
                            }
                        }
                    }

                    TextObsVen.Disabled = false;
                    CheckEsyMat.Enabled = true;

                    Session.Remove("Id_ClienteBD");
                    Session.Remove("ID_ContactoBD");
                }

            }
        }

        protected void elementosllenosalcargarlapagina()
        {
            // Obtener la fecha y hora actual
            DateTime now = DateTime.Now;

            // Asignar la fecha al TextBox de fecha
            TextFec.Text = now.ToString("yyyy-MM-dd");

            // Asignar la hora al TextBox de hora
            TextFech.Text = now.ToString("HH:mm");

            TextIngDis.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

            string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextUltAc.Text = fechaHoraActual;

            TextFecOkDib.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

            DateTime fechaActual = DateTime.Now;

            // Sumar 3 días hábiles a partir de la fecha actual
            DateTime fechaProgramadaEntrega = ObtenerProximaFechaHabil(fechaActual, 3);

            // Asignar la fecha programada de entrega al TextBox
            TextEntrega.Text = fechaProgramadaEntrega.ToString("yyyy-MM-ddTHH:mm");


            DateTime fechaMas5Dias = fechaActual.AddDays(-8);
            TextFechDeIng.Text = fechaMas5Dias.ToString("yyyy-MM-dd");

            DateTime fechaMenos5Dias = fechaActual.AddDays(7);
            Texty.Text = fechaMenos5Dias.ToString("yyyy-MM-dd");

            BtnProgramar.CssClass = "btn btn-warning shadow btn-sm fw-bold";


            TextTel.Text = ("2889898");
            TextCel.Text = ("3002025400");

            ChecConDeCab.Checked = true;
            ChecPiso.Checked = true;
            ChecDiv.Checked = true;
            ChecCie.Checked = true;
            ChecCan.Checked = true;
            ChecBteEle.Checked = true;
            ChecBteSw.Checked = true;
            ChecSujPt.Checked = true;
            ChecAlCie.Checked = true;
            ChecPerRef.Checked = true;
            ChecGuaEsc.Checked = true;
            CheckBox16.Checked = true;
            ChecCot.Checked = true;
            ChecMailTer.Checked = true;
            ChecCotVia.Checked = true;
            CheckBox4.Checked = true;
            ChecMue.Checked = true;
            TextObsVen.Disabled = true;
            CheckEsyMat.Enabled = false;
        }

        protected void habilitarbotonesDise()
        {
            NuevoDisBit.Enabled = true;
            ActualizarDiseno.Enabled = true;
            Cancelar.Enabled = true;

            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled AzulClaro";


            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";
            Cancelar.CssClass = "btn btn-sm shadow button-enabled RojoCancelar";


            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            DocBitacora.Enabled = false;
            DocBitacora.CssClass = "btn btn-sm shadow button-disabled";

            RegresarDiseño.Enabled = false;
            RegresarDiseño.CssClass = "btn btn-sm shadow button-disabled";

            AdicionarElemento.Enabled = false;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-disabled";

            PausarDiseño.Enabled = false;
            PausarDiseño.CssClass = "btn btn-sm shadow button-disabled";

            EliminarDiseño.Enabled = false;
            EliminarDiseño.CssClass = "btn btn-sm shadow button-disabled";
        }

        protected void habilitarbotonesRece()
        {
            NuevoDisBit.Enabled = false;
            ActualizarDiseno.Enabled = true;
            Cancelar.Enabled = true;

            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";


            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";
            Cancelar.CssClass = "btn btn-sm shadow button-enabled RojoCancelar";


            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            DocBitacora.Enabled = false;
            DocBitacora.CssClass = "btn btn-sm shadow button-disabled";

            RegresarDiseño.Enabled = false;
            RegresarDiseño.CssClass = "btn btn-sm shadow button-disabled";

            AdicionarElemento.Enabled = false;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-disabled";

            PausarDiseño.Enabled = false;
            PausarDiseño.CssClass = "btn btn-sm shadow button-disabled";

            EliminarDiseño.Enabled = false;
            EliminarDiseño.CssClass = "btn btn-sm shadow button-disabled";
        }

        private void DeshabilitarDivYContenido(System.Web.UI.Control container)
        {
            TextPla.Enabled = false;
            TextPla.CssClass = "form-control form-control-sm";

            Button16.Enabled = false;
            Button16.CssClass = "form-control form-control-sm";

            Button15.Enabled = false;
            Button15.CssClass = "form-control form-control-sm";

            Button12.Enabled = false;
            Button12.CssClass = "form-control form-control-sm";

            Button13.Enabled = false;
            Button13.CssClass = "form-control form-control-sm";

            Button14.Enabled = false;
            Button14.CssClass = "form-control form-control-sm";

            Button11.Enabled = false;
            Button11.CssClass = "form-control form-control-sm";

            Button9.Enabled = false;
            Button9.CssClass = "form-control form-control-sm";

            Button10.Enabled = false;
            Button10.CssClass = "form-control form-control-sm";




            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Button1.Enabled = false;

            TextDir.Enabled = false;
            TextDir.CssClass = "form-control form-control-sm";

            lblDir.Enabled = false;
            lblDir.CssClass = "col-form-label-sm";

            TextDes.Enabled = false;
            TextDes.CssClass = "form-control form-control-sm";

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            lblDescuento.Enabled = false;
            lblDescuento.CssClass = "col-form-label-sm";

            lblIngDis.Enabled = false;
            lblIngDis.CssClass = "col-form-label-sm";

            lblUltAct.Enabled = false;
            lblUltAct.CssClass = "col-form-label-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            lblPro.Enabled = false;
            lblPro.CssClass = "col-form-label-sm";

            TextProyecto.Enabled = false;
            TextProyecto.CssClass = "form-control form-control-sm";

            lblPla.Enabled = false;
            lblPla.CssClass = "col-form-label-sm";

            lblUrg.Enabled = false;
            lblUrg.CssClass = "col-form-label-sm";



            lblCotizar.Enabled = false;
            lblCotizar.CssClass = "col-form-label-sm";

            ChecCot.Enabled = false;

            lblEnt.Enabled = false;
            lblEnt.CssClass = "col-form-label-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            lblEntDib.Enabled = false;
            lblEntDib.CssClass = "col-form-label-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            lblZon.Enabled = false;
            lblZon.CssClass = "col-form-label-sm";

            TextZona.Enabled = false;
            TextZona.CssClass = "form-control form-control-sm";

            lblCon.Enabled = false;
            lblCon.CssClass = "col-form-label-sm";

            TextContacto.Enabled = false;
            TextContacto.CssClass = "form-control form-control-sm";

            lblTel.Enabled = false;
            lblNumDise.CssClass = "col-form-label-sm";

            TextTel.Enabled = false;
            TextTel.CssClass = "form-control form-control-sm";

            lblMaiTer.Enabled = false;
            lblMaiTer.CssClass = "col-form-label-sm";

            ChecMailTer.Enabled = false;

            lblCotVia.Enabled = false;
            lblCotVia.CssClass = "col-form-label-sm";


            ChecCotVia.Enabled = false;

            lblCotTte.Enabled = false;
            lblCotTte.CssClass = "col-form-label-sm";

            CheckBox4.Enabled = false;

            lblAse.Enabled = false;
            lblAse.CssClass = "col-form-label-sm";

            DropDownList1.Enabled = false;

            lblPre.Enabled = false;
            lblPre.CssClass = "col-form-label-sm";

            TextPre.Enabled = false;
            TextPre.CssClass = "form-control form-control-sm";

            lblCel.Enabled = false;
            lblCel.CssClass = "col-form-label-sm";

            TextCel.Enabled = false;
            TextCel.CssClass = "form-control form-control-sm";

            lblMai.Enabled = false;
            lblMai.CssClass = "col-form-label-sm";

            TextMail.Enabled = false;
            TextMail.CssClass = "form-control form-control-sm";

            lblCiuPro.Enabled = false;
            lblCiuPro.CssClass = "col-form-label-sm";

            TextCiuPro.Enabled = false;
            TextCiuPro.CssClass = "form-control form-control-sm";



            lblConCab.Enabled = false;
            lblCiuPro.CssClass = "form-label";

            ChecConDeCab.Enabled = false;

            lblPis.Enabled = false;
            lblPis.CssClass = "col-form-label-sm g-5";

            ChecPiso.Enabled = false;

            lblDiv.Enabled = false;
            lblDiv.CssClass = "col-form-label-sm g-5";

            ChecDiv.Enabled = false;

            lblCie.Enabled = false;
            lblCie.CssClass = "col-form-label-sm";

            ChecCie.Enabled = false;

            lblCan.Enabled = false;
            lblCan.CssClass = "col-form-label-sm";

            ChecCan.Enabled = false;

            lblBteEle.Enabled = false;
            lblBteEle.CssClass = "col-form-label-sm";

            ChecBteEle.Enabled = false;

            lblBteSw.Enabled = false;
            lblBteSw.CssClass = "col-form-label-sm";

            ChecBteSw.Enabled = false;

            lblSujPt.Enabled = false;
            lblSujPt.CssClass = "col-form-label-sm";

            ChecSujPt.Enabled = false;

            lblAlCie.Enabled = false;
            lblAlCie.CssClass = "col-form-label-sm";

            ChecAlCie.Enabled = false;

            lblPerRef.Enabled = false;
            lblPerRef.CssClass = "col-form-label-sm";

            ChecPerRef.Enabled = false;

            lblGuaEsc.Enabled = false;
            lblGuaEsc.CssClass = "col-form-label-sm";

            ChecGuaEsc.Enabled = false;

            lblHTotCms.Enabled = false;
            lblHTotCms.CssClass = "col-form-label-sm";

            TexHTot.Enabled = false;

            lblEsyMat.Enabled = false;
            lblEsyMat.CssClass = "col-form-label-sm";

            lblLin.Enabled = false;
            lblLin.CssClass = "col-form-label-sm";

            TextLin.Enabled = false;
            TextLin.CssClass = "form-control form-control-sm";

            lblMos.Enabled = false;
            lblMos.CssClass = "col-form-label-sm";

            TextMos.Enabled = false;
            TextMos.CssClass = "form-control form-control-sm";

            lblSup.Enabled = false;
            lblSup.CssClass = "col-form-label-sm";

            TextSup.Enabled = false;
            TextSup.CssClass = "form-control form-control-sm";

            lblBal.Enabled = false;
            lblBal.CssClass = "col-form-label-sm";

            CheckBox16.Enabled = false;

            lblSop.Enabled = false;
            lblSop.CssClass = "col-form-label-sm";

            TextSop.Enabled = false;
            TextSop.CssClass = "form-control form-control-sm";

            lblGav.Enabled = false;
            lblGav.CssClass = "col-form-label-sm";

            TextGav.Enabled = false;
            TextGav.CssClass = "form-control form-control-sm";

            lblPan.Enabled = false;
            lblPan.CssClass = "col-form-label-sm";

            TextPan.Enabled = false;
            TextPan.CssClass = "form-control form-control-sm";

            lblTPie.Enabled = false;
            lblTPie.CssClass = "col-form-label-sm";

            TextTapPie.Enabled = false;
            TextTapPie.CssClass = "form-control form-control-sm";

            lblRep.Enabled = false;
            lblRep.CssClass = "col-form-label-sm";

            TextRep.Enabled = false;
            TextRep.CssClass = "form-control form-control-sm";

            lblTipVid.Enabled = false;
            lblTipVid.CssClass = "col-form-label-sm";

            TextTipVid.Enabled = false;
            TextTipVid.CssClass = "form-control form-control-sm";

            lblPant.Enabled = false;
            lblPant.CssClass = "col-form-label-sm";

            TextPant.Enabled = false;
            TextPant.CssClass = "form-control form-control-sm";

            lblArc.Enabled = false;
            lblArc.CssClass = "col-form-label-sm";

            TextArch.Enabled = false;
            TextArch.CssClass = "form-control form-control-sm";

            lblMue.Enabled = false;
            lblMue.CssClass = "col-form-label-sm";

            ChecMue.Enabled = false;

            lblCoc.Enabled = false;
            lblCoc.CssClass = "col-form-label-sm";

            TextCoc.Enabled = false;
            TextCoc.CssClass = "form-control form-control-sm";

            lblEntr.Enabled = false;
            lblEntr.CssClass = "col-form-label-sm";

            TextEnt.Enabled = false;
            TextEnt.CssClass = "form-control form-control-sm";

            lblPuer.Enabled = false;
            lblPuer.CssClass = "col-form-label-sm";

            TextPuer.Enabled = false;
            TextPuer.CssClass = "form-control form-control-sm";

            CheckBox18.Enabled = false;

            lblPrePpt.Enabled = false;
            lblPrePpt.CssClass = "col-form-label-sm";

            CheckBox19.Enabled = false;

            lblIma.Enabled = false;
            lblIma.CssClass = "col-form-label-sm";

            CheckBox20.Enabled = false;

            lblAcc.Enabled = false;
            lblAcc.CssClass = "col-form-label-sm";

            CheckBox21.Enabled = false;

            lblTieRea.Enabled = false;
            lblTieRea.CssClass = "col-form-label-sm";

            TextFec.Enabled = false;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = false;
            TextFech.CssClass = "form-control form-control-sm";

            lblUbi.Enabled = false;
            TextUbi.CssClass = "col-form-label-sm";


        }

        //BUSCAR DISE
        protected void But_Click(object sender, EventArgs e)
        {
            string consulta = "SELECT tblDiseño.*, tblDiseño.Fecha_Ingreso, tblDiseño.Nombre_Diseño FROM tblDiseño";

            string whereClause = "";

            if (!string.IsNullOrEmpty(TextFechDeIng.Text) && !string.IsNullOrEmpty(Texty.Text))
            {

                if (!string.IsNullOrEmpty(TextBox3.Text) && !string.IsNullOrEmpty(TextBox4.Text) && !string.IsNullOrEmpty(TextBox5.Text))
                {
                    // Agregar la cláusula AND a la consulta
                    whereClause += " AND Fecha_Ingreso BETWEEN '" + TextFechDeIng.Text + "' AND '" + Texty.Text + "'";
                }
                else
                {

                    // Agregar la cláusula WHERE a la consulta
                    whereClause += " WHERE Fecha_Ingreso BETWEEN '" + TextFechDeIng.Text + "' AND '" + Texty.Text + "'";
                }

            }

            if (!string.IsNullOrEmpty(TextBox3.Text))
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += " WHERE Numero_Diseño LIKE '%" + TextBox3.Text + "%'";
                }
                else
                {
                    whereClause += " AND Numero_Diseño LIKE '%" + TextBox3.Text + "%'";
                }
            }
            if (!string.IsNullOrEmpty(TextBox4.Text))
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += " WHERE Cliente LIKE '%" + TextBox4.Text + "%'";
                }
                else
                {
                    whereClause += " AND Cliente LIKE '%" + TextBox4.Text + "%'";
                }
            }

            if (!string.IsNullOrEmpty(TextBox5.Text))
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += " WHERE Nombre_Diseño LIKE '%" + TextBox5.Text + "%'";
                }
                else
                {
                    whereClause += " AND Nombre_Diseño LIKE '%" + TextBox5.Text + "%'";
                }
            }

            consulta += whereClause;

            // Asigna la consulta al control SqlDataSource1
            SqlDataSource3.SelectCommand = consulta;

            // Vincula el DataGrid al SqlDataSource y actualiza su contenido
            DataGrid4.DataSourceID = "SqlDataSource3";
            DataGrid4.DataBind();
        }

        protected void ddlCiudadX_DataBound(object sender, EventArgs e)
        {

        }

        protected void NuevoDisBit_Click(object sender, EventArgs e)
        {
           Session["CrudDise"] = "Insertar";

            bool lnkClieClicked = Session["lnkClieClicked"] as bool? ?? false;
            bool lnkClieeClicked = Session["lnkClieeClicked"] as bool? ?? false;

            if (int.TryParse(lblNumDise.Text, out _))
            {
                // Mostrar el modal si el valor del label es numerico
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modall').modal('show');", true);
            }
            else
            {
                NuevoLimpiar(); 
            }

            TextObsVen.Disabled = false;
            CheckEsyMat.Enabled = true;

            

            }

        protected void NuevoLimpiar()
        {
            string tipoAccion = Session["CrudDise"] as string;
            if (tipoAccion == "Insertar")
            {
                ComportamientoNuevo();
            }

            if (tipoAccion == "Actualizar")
            {
                ComportamientoActualizar();
            }

        }

        protected void ComportamientoActualizar()
        {
            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "btn btn-sm shadow button-disabled fw-bold";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "Grabar"
            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Habilitar el div y su contenido
            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextUltAc.Enabled = false;

            TextFecOkDib.Enabled = false;

            Button9.Enabled = false;
            Button10.Enabled = false;

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";
        }

        protected void ComportamientoNuevo()
        {
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Deshabilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";


            // Habilitar el div y su contenido
            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextUltAc.Enabled = false;
            TextEntrega.Enabled = false;
            TextFecOkDib.Enabled = false;



            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            Cancelar.Enabled = true;
            // Deshabilita los TextBox
            TextFec.Enabled = false;
            TextFech.Enabled = false;

            lblNumDise.Text = "Por Definir";

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "btn btn-sm shadow button-disabled fw-bold";

            if (Session["ZonaLogeada"] != null)
            {
                string zonaLogeada = Session["ZonaLogeada"].ToString();
                TextZona.SelectedValue = zonaLogeada;
            }
            else
            {
                //NO SE ENCONTRO LA VIABLE DE SESSION ZONALOGEADA
            }
            if (Session["CedulaLogeada"] != null)
            {
                string usuariologeado = Session["CedulaLogeada"].ToString();
                if (DropDownList1.Items.FindByValue(usuariologeado) != null)
                {
                    DropDownList1.SelectedValue = usuariologeado;
                }
                else
                {

                    DropDownList1.SelectedValue = "";
                }
            }

            else
            {
                //NO SE ENCONTRO LA VIABLE DE SESSION ZONALOGEADA
            }
        }

        protected void CheckBox21_CheckedChanged(object sender, EventArgs e)
        {
            // Verifica el estado del CheckBox
            if (CheckBox21.Checked)
            {
                // Habilita los TextBox
                TextFec.Enabled = true;
                TextFech.Enabled = true;
            }
            else
            {
                // Deshabilita los TextBox
                TextFec.Enabled = false;
                TextFech.Enabled = false;
            }
        }

        private void HabilitarDivYContenido(System.Web.UI.Control container)
        {
            foreach (System.Web.UI.Control control in container.Controls)
            {
                if (control is System.Web.UI.WebControls.WebControl)
                {
                    // Verificar si el control es el botón BtnProgramar
                    if (control != BtnProgramar)
                    {
                        // Si el control no es el botón BtnProgramar, habilitarlo
                        ((System.Web.UI.WebControls.WebControl)control).Enabled = true;
                    }
                }

                // Si el control es un contenedor, llamar recursivamente a la función
                if (control.HasControls())
                {
                    HabilitarDivYContenido(control);
                }
            }

            ChecUrgent.Enabled = false;
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            string tipoAccion = Session["Diseno"] as string;
            if (tipoAccion == "Ventas")
            {

                string tipoAccionCrud = Session["CrudDise"] as string;
                if (tipoAccionCrud == "Actualizar")
                {
                    // Si se hizo clic en Modificar antes, realiza las s necesarias para volver al estado anterior.
                    NuevoDisBit.Enabled = false;
                    NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

                    Modificar.Enabled = true;
                    Modificar.CssClass = "btn btn-sm shadow button-enabled";

                    ActualizarDiseno.Enabled = false;
                    ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";


                    // Resto de las acciones para volver al estado anterior...
                }
                else
                {
                    // Si no se hizo clic en Modificar antes, simplemente restablece todo como estaba antes de Cancelar.
                    Grabar.Enabled = false;
                    Grabar.CssClass = "btn btn-sm shadow button-disabled";

                    NuevoDisBit.Enabled = true;
                    NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

                    ActualizarDiseno.Enabled = true;
                    ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

                    Modificar.Enabled = false;
                    Modificar.CssClass = "btn btn-sm shadow button-disabled";
                }



                // Deshabilitar el botón "Grabar"
                Grabar.Enabled = false;
                Grabar.CssClass = "btn btn-sm shadow button-disabled";

                // Habilitar el botón "NuevoDisBit"
                NuevoDisBit.Enabled = true;

                // Habilitar el botón "ActualizarDiseno"
                ActualizarDiseno.Enabled = true;
                NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

                // Ocultar el div y su contenido
                DeshabilitarDivYContenido(miDiv);


                lblCotizar.CssClass = "col-form-label-sm text-dark";
                lblCotizar.Font.Bold = false;

                // Manejo del evento DataGridDise_ItemCommand
                bool eventoItemCommandEjecutado = Session["EventoItemCommandEjecutado"] as bool? ?? false;

                if (eventoItemCommandEjecutado)
                {
                    // Si el evento DataGridDise_ItemCommand se ejecutó correctamente,
                    // obtener el valor de la variable de sesión "NumeroDisenoSession" y asignarlo a lblNumDise
                    int numeroDiseno = Session["NumeroDisenoSession"] as int? ?? 0;
                    lblNumDise.Text = numeroDiseno.ToString();

                    Modificar.Enabled = true;
                    Modificar.CssClass = "btn btn-sm shadow button-enabled";
                }
                else
                {
                    Grabar.Enabled = false;
                    Grabar.CssClass = "btn btn-sm shadow button-disabled";

                    NuevoDisBit.Enabled = true;
                    NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

                    ActualizarDiseno.Enabled = true;
                    ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

                    lblNumDise.Text = "Numero";



                    // Limpia la variable de sesión "EventoItemCommandEjecutado" después de utilizarla.
                    Session["EventoItemCommandEjecutado"] = false;
                }

                // Manejo del evento DataGridDise_ItemCommand
                bool eventoNoButtonEjecutado = Session["EventoNoButtonEjecutado"] as bool? ?? false;


                if (eventoNoButtonEjecutado)
                {
                    Modificar.Enabled = true;
                    Modificar.CssClass = "btn btn-sm shadow button-enabled";
                }

                Session.Remove("NumeroDiseño5");
            }
            else if (tipoAccion == "Recepcion")
            {
               
            }

            TextObsVen.Disabled = true;
            CheckEsyMat.Enabled = false;
        }

        protected void lnkClie_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid2.Items[rowIndex];

            // Almacena el nombre del archivo en la variable de sesión
            Session["NumeroDiseño"] = selectedRow.Cells[2].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid2.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }
            AccionesAlCargarDiseño();

        }

        protected void ValidarBotonOk()
        {
            if (Session["NumeroDiseño"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño"].ToString()))
            {
                // Obtener el número de diseño de la sesión
                int numeroDiseno = Convert.ToInt32(Session["NumeroDiseño"]);

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string queryString = "SELECT ProgramadoVentas FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseno";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@NumeroDiseno", numeroDiseno);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            // Obtener el valor de la columna ProgramadoVentas
                            int programadoVentas = Convert.ToInt32(reader["ProgramadoVentas"]);

                            // Ajustar la propiedad Enabled del botón BtnProgramar
                            BtnProgramar.Enabled = (programadoVentas == 0);
                            BtnProgramar.CssClass = "btn btn-sm shadow btn-warning fw-bold";


                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            else if (Session["NumeroDiseño2"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño2"].ToString()))
            {
                // Obtener el número de diseño de la sesión
                int numeroDiseno = Convert.ToInt32(Session["NumeroDiseño2"]);

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string queryString = "SELECT ProgramadoVentas FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseno";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@NumeroDiseno", numeroDiseno);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            // Obtener el valor de la columna ProgramadoVentas
                            int programadoVentas = Convert.ToInt32(reader["ProgramadoVentas"]);

                            // Ajustar la propiedad Enabled del botón BtnProgramar
                            BtnProgramar.Enabled = (programadoVentas == 0);
                            BtnProgramar.CssClass = "btn btn-sm shadow btn-warning fw-bold";

                            // Si el botón está habilitado, actualizar los TextBox con las fechas
                            if (BtnProgramar.Enabled)
                            {
                                // Obtener la fecha y hora actual
                                DateTime now = DateTime.Now;

                                // Asignar la fecha al TextBox de fecha
                                TextFec.Text = now.ToString("yyyy-MM-dd");

                                // Asignar la hora al TextBox de hora
                                TextFech.Text = now.ToString("HH:mm");

                                TextIngDis.Text = now.ToString("yyyy-MM-ddTHH:mm");

                                string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

                                // Asignar la fecha y hora actual al TextBox
                                TextUltAc.Text = fechaHoraActual;

                                TextFecOkDib.Text = now.ToString("yyyy-MM-ddTHH:mm");

                                DateTime fechaActual = DateTime.Now;

                                // Sumar 3 días hábiles a partir de la fecha actual
                                DateTime fechaProgramadaEntrega = ObtenerProximaFechaHabil(fechaActual, 3);

                                // Asignar la fecha programada de entrega al TextBox
                                TextEntrega.Text = fechaProgramadaEntrega.ToString("yyyy-MM-ddTHH:mm");
                            }
                            else
                            {

                            }
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }

        protected void lnkCliee_Click(object sender, EventArgs e)
        {
            
            string tipoAccion = Session["Diseno"] as string;
            if (tipoAccion == "Ventas")
            {
                // Obtén el LinkButton que se hizo clic
                LinkButton lnkSelectRow = (LinkButton)sender;

                // Obtén el índice de fila desde el CommandArgument
                int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

                // Accede a la fila seleccionada en el DataGrid
                DataGridItem selectedRow = DataGrid4.Items[rowIndex];

                // Almacena el nombre del archivo en la variable de sesión
                Session["NumeroDiseño"] = selectedRow.Cells[2].Text;

                // Deselecciona todas las filas previamente seleccionadas
                foreach (DataGridItem item in DataGrid4.Items)
                {
                    if (item != selectedRow)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                Session["lnkClieeClicked"] = true;

                BotonesCargarDise();

                ValidarBotonOk();


                DeshabilitarDivYContenido(miDiv);
            }
            else if (tipoAccion == "Recepcion")
            {

                // Obtén el LinkButton que se hizo clic
                LinkButton lnkSelectRow = (LinkButton)sender;

                // Obtén el índice de fila desde el CommandArgument
                int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

                // Accede a la fila seleccionada en el DataGrid
                DataGridItem selectedRow = DataGrid4.Items[rowIndex];

                // Almacena el nombre del archivo en la variable de sesión
                Session["NumeroDiseño"] = selectedRow.Cells[2].Text;

                // Deselecciona todas las filas previamente seleccionadas
                foreach (DataGridItem item in DataGrid4.Items)
                {
                    if (item != selectedRow)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                Session["lnkClieeClicked"] = true;

                BotonesCargarDise();

                DeshabilitarDivYContenido(miDiv);

                ValidarBotonTerminarRecep();

                ValidarBotonRegresarDise();

                ValidarBotonVisualizarCotActual();
            }

          

        }

        protected void ValidarBotonTerminarRecep()
        {
            // Verificar si la variable de sesión 'NumeroDiseño' tiene contenido
            if (Session["NumeroDiseño"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño"].ToString()))
            {
                string numeroDiseño = Session["NumeroDiseño"].ToString();

                // Consulta SQL para verificar si el diseño está terminado y la cotización no está OK
                string consulta = "SELECT TerminadoDibujo, CotizaciónOK FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseño AND TerminadoDibujo = 1 AND CotizaciónOK = 0";

                // Establecer la conexión con la base de datos y ejecutar la consulta
                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        // Agregar parámetro para evitar SQL injection
                        comando.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);

                        // Abrir conexión y ejecutar consulta
                        conexion.Open();
                        SqlDataReader reader = comando.ExecuteReader();

                        // Verificar si la consulta arrojó resultados
                        if (reader.HasRows)
                        {
                            // Si hay resultados, habilitar el botón BtnProgramar
                            BtnProgramar.Enabled = true;
                            BtnProgramar.CssClass = "btn btn-sm shadow btn-warning fw-bold";      
                        }
                        else
                        {
                            // Si no hay resultados, deshabilitar el botón BtnProgramar
                            BtnProgramar.Enabled = false;
                            BtnProgramar.CssClass = "btn btn-sm shadow btn-warning fw-bold";       
                        }

                        // Cerrar la conexión y liberar recursos
                        reader.Close();
                        conexion.Close();
                    }
                }
            }       
        }

        protected void ValidarBotonVisualizarCotActual()
        {
            // Verificar si la variable de sesión 'NumeroDiseño' tiene contenido
            if (Session["NumeroDiseño"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño"].ToString()))
            {
                string numeroDiseño = Session["NumeroDiseño"].ToString();

                // Consulta SQL para verificar si el diseño está terminado y la cotización no está OK
                string consulta = "SELECT TerminadoDibujo FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseño AND TerminadoDibujo = 1";

                // Establecer la conexión con la base de datos y ejecutar la consulta
                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        // Agregar parámetro para evitar SQL injection
                        comando.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);

                        // Abrir conexión y ejecutar consulta
                        conexion.Open();
                        SqlDataReader reader = comando.ExecuteReader();

                        // Verificar si la consulta arrojó resultados
                        if (reader.HasRows)
                        {       
                            LinkButton2.Enabled = true;
                            LinkButton2.CssClass = "btn btn-sm shadow button-enabled";
                        }
                        else
                        {        
                            LinkButton2.Enabled = false;
                            LinkButton2.CssClass = "btn btn-sm shadow button-disabled";
                        }

                        // Cerrar la conexión y liberar recursos
                        reader.Close();
                        conexion.Close();
                    }
                }
            }
        }

        protected void ValidarBotonRegresarDise()
        {
            // Verificar si la variable de sesión 'NumeroDiseño' tiene contenido
            if (Session["NumeroDiseño"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño"].ToString()))
            {
                string numeroDiseño = Session["NumeroDiseño"].ToString();

                // Consulta SQL para verificar si el diseño está terminado y la cotización no está OK
                string consulta = "SELECT TerminadoDibujo, CotizaciónOK FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseño AND TerminadoDibujo = 1 AND CotizaciónOK = 0";

                // Establecer la conexión con la base de datos y ejecutar la consulta
                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        // Agregar parámetro para evitar SQL injection
                        comando.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);

                        // Abrir conexión y ejecutar consulta
                        conexion.Open();
                        SqlDataReader reader = comando.ExecuteReader();

                        // Verificar si la consulta arrojó resultados
                        if (reader.HasRows)
                        {
                            // Si hay resultados, habilitar el botón BtnProgramar
                            RegresarDiseño.Enabled = true;
                            RegresarDiseño.CssClass = "btn btn-sm shadow button-enabled";
                        }
                        else
                        {
                            // Si no hay resultados, deshabilitar el botón BtnProgramar
                            RegresarDiseño.Enabled = false;
                            RegresarDiseño.CssClass = "btn btn-sm shadow button-disabled";
                        }

                        // Cerrar la conexión y liberar recursos
                        reader.Close();
                        conexion.Close();
                    }
                }
            }
        }

        protected void BotonesCargarDise()
        {
            string tipoAccion = Session["Diseno"] as string;
            if (tipoAccion == "Ventas")
            {
                NuevoDisBit.Enabled = true;
                NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

                Grabar.Enabled = false;
                Grabar.CssClass = "btn btn-sm shadow button-disabled";

                // Habilitar el botón "ActualizarDiseno"
                ActualizarDiseno.Enabled = true;
                ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

                // Habilitar el botón "Modificar"
                Modificar.Enabled = true;
                Modificar.CssClass = "btn btn-sm shadow button-enabled";

                DocBitacora.Enabled = true;
                DocBitacora.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

                // Habilitar el botón "AdicionarElemento"
                AdicionarElemento.Enabled = true;
                AdicionarElemento.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

            }
            else if (tipoAccion == "Recepcion")
            {
                NuevoDisBit.Enabled = false;
                NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

                Grabar.Enabled = false;
                Grabar.CssClass = "btn btn-sm shadow button-disabled";

                // Habilitar el botón "ActualizarDiseno"
                ActualizarDiseno.Enabled = true;
                ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";
         

                // Habilitar el botón "Modificar"
                Modificar.Enabled = false;
                Modificar.CssClass = "btn btn-sm shadow button-disabled";

                DocBitacora.Enabled = true;
                DocBitacora.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

                // Habilitar el botón "AdicionarElemento"
                AdicionarElemento.Enabled = true;
                AdicionarElemento.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

                LinkButton2.Enabled = true;
                LinkButton2.CssClass = "btn btn-sm button-enabled";
            }
        }

        protected void CargarDiseñoSC_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGridDiseños.Items[rowIndex];

            // Almacena el nombre del archivo en la variable de sesión
            Session["NumeroDiseño"] = selectedRow.Cells[2].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGridDiseños.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            Session["lnkClieeClicked"] = true;

            NuevoDisBit.Enabled = true;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;
            Modificar.CssClass = "btn btn-sm shadow button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-enabled AzulClaro";

            ValidarBotonOk();


            DeshabilitarDivYContenido(miDiv);
        }

        protected void Nombreasesor()
        {
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

                // Realizar la conexión a la base de datos y la consulta para obtener el nombre y apellido del usuario
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Cedula, Zona, Nombre, Apellidos FROM tblEmpleado WHERE Login = @nombreUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", usuariologueado);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string Cedula = reader["Cedula"].ToString();
                            string zona = reader["Zona"].ToString();
                            string nombre = reader["Nombre"].ToString();
                            string apellidos = reader["Apellidos"].ToString();

                            TextZona.Text = zona;
                        }

                    }
                }

            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
        }

        protected void Modificar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ShowCase').modal('show');", true);

            TextObsVen.Disabled = false;
            CheckEsyMat.Enabled = true;

            Session["CrudDise"] = "Actualizar";

        }

        protected void BtnModificarNo_Click(object sender, EventArgs e)
        { 
            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "btn btn-sm shadow button-disabled fw-bold";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "Grabar"
            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Habilitar el div y su contenido
            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextUltAc.Enabled = false;

            TextFecOkDib.Enabled = false;

            Button9.Enabled = false;
            Button10.Enabled = false;

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";
        }

        protected void BtnModificarSi_Click(object sender, EventArgs e)
        {
            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "btn btn-sm shadow button-disabled fw-bold";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "Grabar"
            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";


            DeshabilitarDivYContenidoMitad(miDiv);
        }

        private void DeshabilitarDivYContenidoMitad(System.Web.UI.Control container)
        {
            TextPla.Enabled = false;
            TextPla.CssClass = "form-control form-control-sm";

            Button16.Enabled = false;
            Button16.CssClass = "form-control form-control-sm";

            Button15.Enabled = false;
            Button15.CssClass = "form-control form-control-sm";

            Button12.Enabled = false;
            Button12.CssClass = "form-control form-control-sm";

            Button13.Enabled = false;
            Button13.CssClass = "form-control form-control-sm";

            Button14.Enabled = false;
            Button14.CssClass = "form-control form-control-sm";

            Button11.Enabled = false;
            Button11.CssClass = "form-control form-control-sm";

            Button9.Enabled = false;
            Button9.CssClass = "form-control form-control-sm";

            Button10.Enabled = false;
            Button10.CssClass = "form-control form-control-sm";




            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Button1.Enabled = false;

            TextDir.Enabled = false;
            TextDir.CssClass = "form-control form-control-sm";

            lblDir.Enabled = false;
            lblDir.CssClass = "col-form-label-sm";

            TextDes.Enabled = false;
            TextDes.CssClass = "form-control form-control-sm";

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            lblDescuento.Enabled = false;
            lblDescuento.CssClass = "col-form-label-sm";

            lblIngDis.Enabled = false;
            lblIngDis.CssClass = "col-form-label-sm";

            lblUltAct.Enabled = false;
            lblUltAct.CssClass = "col-form-label-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            lblPro.Enabled = false;
            lblPro.CssClass = "col-form-label-sm";

            TextProyecto.Enabled = false;
            TextProyecto.CssClass = "form-control form-control-sm";

            lblPla.Enabled = false;
            lblPla.CssClass = "col-form-label-sm";

            lblUrg.Enabled = false;
            lblUrg.CssClass = "col-form-label-sm";



            lblCotizar.Enabled = false;
            lblCotizar.CssClass = "col-form-label-sm";

            ChecCot.Enabled = false;

            lblEnt.Enabled = false;
            lblEnt.CssClass = "col-form-label-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            lblEntDib.Enabled = false;
            lblEntDib.CssClass = "col-form-label-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            lblZon.Enabled = false;
            lblZon.CssClass = "col-form-label-sm";

            TextZona.Enabled = false;
            TextZona.CssClass = "form-control form-control-sm";

            lblCon.Enabled = false;
            lblCon.CssClass = "col-form-label-sm";

            TextContacto.Enabled = false;
            TextContacto.CssClass = "form-control form-control-sm";

            lblTel.Enabled = false;
            lblNumDise.CssClass = "col-form-label-sm";

            TextTel.Enabled = false;
            TextTel.CssClass = "form-control form-control-sm";

            lblMaiTer.Enabled = false;
            lblMaiTer.CssClass = "col-form-label-sm";

            ChecMailTer.Enabled = false;

            lblCotVia.Enabled = false;
            lblCotVia.CssClass = "col-form-label-sm";


            ChecCotVia.Enabled = false;

            lblCotTte.Enabled = false;
            lblCotTte.CssClass = "col-form-label-sm";

            CheckBox4.Enabled = false;

            lblAse.Enabled = false;
            lblAse.CssClass = "col-form-label-sm";

            DropDownList1.Enabled = false;

            lblPre.Enabled = false;
            lblPre.CssClass = "col-form-label-sm";

            TextPre.Enabled = false;
            TextPre.CssClass = "form-control form-control-sm";

            lblCel.Enabled = false;
            lblCel.CssClass = "col-form-label-sm";

            TextCel.Enabled = false;
            TextCel.CssClass = "form-control form-control-sm";

            lblMai.Enabled = false;
            lblMai.CssClass = "col-form-label-sm";

            TextMail.Enabled = false;
            TextMail.CssClass = "form-control form-control-sm";

            lblCiuPro.Enabled = false;
            lblCiuPro.CssClass = "col-form-label-sm";

            TextCiuPro.Enabled = false;
            TextCiuPro.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "btn btn-warning shadow btn-sm fw-bold";

            lblConCab.Enabled = false;
            lblCiuPro.CssClass = "form-label";

            ChecConDeCab.Enabled = false;

            lblPis.Enabled = false;
            lblPis.CssClass = "col-form-label-sm g-5";

            ChecPiso.Enabled = false;

            lblDiv.Enabled = false;
            lblDiv.CssClass = "col-form-label-sm g-5";

            ChecDiv.Enabled = false;

            lblCie.Enabled = false;
            lblCie.CssClass = "col-form-label-sm";

            ChecCie.Enabled = false;

            lblCan.Enabled = false;
            lblCan.CssClass = "col-form-label-sm";

            ChecCan.Enabled = false;

            lblBteEle.Enabled = false;
            lblBteEle.CssClass = "col-form-label-sm";

            ChecBteEle.Enabled = false;

            lblBteSw.Enabled = false;
            lblBteSw.CssClass = "col-form-label-sm";

            ChecBteSw.Enabled = false;

            lblSujPt.Enabled = false;
            lblSujPt.CssClass = "col-form-label-sm";

            ChecSujPt.Enabled = false;

            lblAlCie.Enabled = false;
            lblAlCie.CssClass = "col-form-label-sm";

            ChecAlCie.Enabled = false;

            lblPerRef.Enabled = false;
            lblPerRef.CssClass = "col-form-label-sm";

            ChecPerRef.Enabled = false;

            lblGuaEsc.Enabled = false;
            lblGuaEsc.CssClass = "col-form-label-sm";

            ChecGuaEsc.Enabled = false;

            lblHTotCms.Enabled = false;
            lblHTotCms.CssClass = "col-form-label-sm";

            TexHTot.Enabled = false;

            lblEsyMat.Enabled = true;
            lblEsyMat.CssClass = "col-form-label-sm";

            lblLin.Enabled = true;
            lblLin.CssClass = "col-form-label-sm";

            TextLin.Enabled = true;
            TextLin.CssClass = "form-control form-control-sm";

            lblMos.Enabled = true;
            lblMos.CssClass = "col-form-label-sm";

            TextMos.Enabled = true;
            TextMos.CssClass = "form-control form-control-sm";

            lblSup.Enabled = true;
            lblSup.CssClass = "col-form-label-sm";

            TextSup.Enabled = true;
            TextSup.CssClass = "form-control form-control-sm";

            lblBal.Enabled = true;
            lblBal.CssClass = "col-form-label-sm";

            CheckBox16.Enabled = false;

            lblSop.Enabled = true;
            lblSop.CssClass = "col-form-label-sm";

            TextSop.Enabled = true;
            TextSop.CssClass = "form-control form-control-sm";

            lblGav.Enabled = true;
            lblGav.CssClass = "col-form-label-sm";

            TextGav.Enabled = true;
            TextGav.CssClass = "form-control form-control-sm";

            lblPan.Enabled = true;
            lblPan.CssClass = "col-form-label-sm";

            TextPan.Enabled = true;
            TextPan.CssClass = "form-control form-control-sm";

            lblTPie.Enabled = true;
            lblTPie.CssClass = "col-form-label-sm";

            TextTapPie.Enabled = true;
            TextTapPie.CssClass = "form-control form-control-sm";

            lblRep.Enabled = true;
            lblRep.CssClass = "col-form-label-sm";

            TextRep.Enabled = true;
            TextRep.CssClass = "form-control form-control-sm";

            lblTipVid.Enabled = true;
            lblTipVid.CssClass = "col-form-label-sm";

            TextTipVid.Enabled = true;
            TextTipVid.CssClass = "form-control form-control-sm";

            lblPant.Enabled = true;
            lblPant.CssClass = "col-form-label-sm";

            TextPant.Enabled = true;
            TextPant.CssClass = "form-control form-control-sm";

            lblArc.Enabled = true;
            lblArc.CssClass = "col-form-label-sm";

            TextArch.Enabled = true;
            TextArch.CssClass = "form-control form-control-sm";

            lblMue.Enabled = true;
            lblMue.CssClass = "col-form-label-sm";

            ChecMue.Enabled = false;

            lblCoc.Enabled = true;
            lblCoc.CssClass = "col-form-label-sm";

            TextCoc.Enabled = true;
            TextCoc.CssClass = "form-control form-control-sm";

            lblEntr.Enabled = true;
            lblEntr.CssClass = "col-form-label-sm";

            TextEnt.Enabled = true;
            TextEnt.CssClass = "form-control form-control-sm";

            lblPuer.Enabled = true;
            lblPuer.CssClass = "col-form-label-sm";

            TextPuer.Enabled = true;
            TextPuer.CssClass = "form-control form-control-sm";

            CheckBox18.Enabled = true;

            lblPrePpt.Enabled = true;
            lblPrePpt.CssClass = "col-form-label-sm";

            CheckBox19.Enabled = true;

            lblIma.Enabled = true;
            lblIma.CssClass = "col-form-label-sm";

            CheckBox20.Enabled = true;

            lblAcc.Enabled = true;
            lblAcc.CssClass = "col-form-label-sm";

            CheckBox21.Enabled = true;

            lblTieRea.Enabled = true;
            lblTieRea.CssClass = "col-form-label-sm";

            TextFec.Enabled = true;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = true;
            TextFech.CssClass = "form-control form-control-sm";

            lblUbi.Enabled = true;
            TextUbi.CssClass = "col-form-label-sm";


        }

        private string ValidarCampos()
        {
            string campoFaltante = string.Empty;

            if (string.IsNullOrEmpty(TextCiuPro.Text))
            {
                campoFaltante = "Ciudad del Proyecto";
            }
            else if (string.IsNullOrEmpty(TextMail.Text))
            {
                campoFaltante = "Email";
            }
            else if (string.IsNullOrEmpty(TextPre.Text))
            {
                campoFaltante = "Pre";
            }
            else if (string.IsNullOrEmpty(TextCel.Text))
            {
                campoFaltante = "Celular";
            }
            else if (string.IsNullOrEmpty(TextTel.Text))
            {
                campoFaltante = "Teléfono";
            }
            else if (string.IsNullOrEmpty(TextContacto.Text))
            {
                campoFaltante = "Contacto";
            }
            else if (string.IsNullOrEmpty(TextZona.Text))
            {
                campoFaltante = "Zona";
            }
            else if (string.IsNullOrEmpty(TextFecOkDib.Text))
            {
                campoFaltante = "Fecha de Dibujo OK";
            }
            else if (string.IsNullOrEmpty(TextEntrega.Text))
            {
                campoFaltante = "Fecha Programada de Entrega";
            }
            else if (string.IsNullOrEmpty(TextPla.Text))
            {
                campoFaltante = "Plano";
            }
            else if (string.IsNullOrEmpty(TextProyecto.Text))
            {
                campoFaltante = "Nombre del Proyecto";
            }
            else if (string.IsNullOrEmpty(TextUltAc.Text))
            {
                campoFaltante = "Última Activación";
            }
            else if (string.IsNullOrEmpty(TextIngDis.Text))
            {
                campoFaltante = "Fecha de Ingreso";
            }
            else if (DropDownList1.SelectedItem == null)
            {
                campoFaltante = "Asesor";
            }
            else if (string.IsNullOrEmpty(TextCliente.Text))
            {
                campoFaltante = "Cliente";
            }
            else if (string.IsNullOrEmpty(TextDir.Text))
            {
                campoFaltante = "Dirección";
            }
            else if (string.IsNullOrEmpty(TextDes.Text))
            {
                campoFaltante = "Descuento";
            }
            else if (string.IsNullOrEmpty(TexHTot.Text))
            {
                campoFaltante = "H.Total";
            }
            else if (string.IsNullOrEmpty(TextLin.Text))
            {
                campoFaltante = "Línea";
            }
            else if (string.IsNullOrEmpty(TextMos.Text))
            {
                campoFaltante = "Tipo de Mostrador";
            }
            else if (string.IsNullOrEmpty(TextSup.Text))
            {
                campoFaltante = "Acabado de Superficie";
            }
            else if (string.IsNullOrEmpty(TextSop.Text))
            {
                campoFaltante = "Tipo de Soporte";
            }
            else if (string.IsNullOrEmpty(TextGav.Text))
            {
                campoFaltante = "Tipo de Gaveta";
            }
            else if (string.IsNullOrEmpty(TextPan.Text))
            {
                campoFaltante = "Acabado de Paneles";
            }
            else if (string.IsNullOrEmpty(TextTapPie.Text))
            {
                campoFaltante = "Tipo de Tapa de Pierna";
            }
            else if (string.IsNullOrEmpty(TextRep.Text))
            {
                campoFaltante = "Tipo de Repisa";
            }
            else if (string.IsNullOrEmpty(TextPant.Text))
            {
                campoFaltante = "Tipo de Pantalla";
            }
            else if (string.IsNullOrEmpty(TextTipVid.Text))
            {
                campoFaltante = "Tipo de vidrio";
            }
            else if (string.IsNullOrEmpty(TextArch.Text))
            {
                campoFaltante = "Tipo de Archivador";
            }
            else if (string.IsNullOrEmpty(TextCoc.Text))
            {
                campoFaltante = "Tipo de mueble coco";
            }
            else if (string.IsNullOrEmpty(TextEnt.Text))
            {
                campoFaltante = "Mueble Entrepaño";
            }
            else if (string.IsNullOrEmpty(TextPuer.Text))
            {
                campoFaltante = "Mueble Puertas";
            }
            else if (string.IsNullOrEmpty(TextObsVen.InnerText))
            {
                campoFaltante = "Observaciones de Ventas";
            }
            return campoFaltante;
        }

        protected void btnInsertar_Click(object sender, EventArgs e)
        {

            // Realiza la validación de campos
            string campoFaltante = ValidarCampos();

            if (string.IsNullOrEmpty(campoFaltante))
            {
                string tipoAccion = Session["CrudDise"] as string;
                if (tipoAccion == "Insertar")
                {
                    // Realiza la inserción
                    if (RealizarInsercion())
                    {
                        Session.Remove("CrudDise");

                        string mensajePersonalizado = "Las Fechas: Ingreso del diseño, Ultima Activación y Entrega, se ajustaran cuando programe el diseño";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                    else
                    {
                        Session.Remove("CrudDise");

                        string mensajePersonalizado = "No se afecto ninguna fila";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                }
                else if (tipoAccion == "Actualizar")
                {
                    // Realiza la actualización
                    if (RealizarActualizacion())
                    {
                        Session.Remove("CrudDise");



                        string mensajePersonalizado = "Las Fechas: Ingreso del diseño, Ultima Activación y Entrega, se ajustaran cuando programe el diseño";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");



                    }
                    else
                    {
                        Session.Remove("CrudDise");

                        string mensajePersonalizado = "El Diseño ya fue aprobado para Dibujo y Despiece, este departamento lo debe habilitar para ser modificado";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                }

                
        
            }
            else
            {
                // Muestra el modal de advertencia si faltan campos
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalll').modal('show'); $('#campoFaltante').text('" + campoFaltante + "');", true);
            }




        }

        protected void ValidarFecha(object sender, EventArgs e)
        {
            DateTime fechaTextBox;
            if (!DateTime.TryParse(TextFec.Text, out fechaTextBox))
            {
                // Manejo de error si el valor en TextFec no es una fecha válida
                return;
            }

            DateTime fechaEntrega;
            if (!DateTime.TryParse(TextEntrega.Text, out fechaEntrega))
            {
                // Manejo de error si el valor en TextEntrega no es una fecha válida
                return;
            }

            // Obtener solo la parte de la fecha (sin la parte de la hora)
            fechaTextBox = fechaTextBox.Date;
            fechaEntrega = fechaEntrega.Date;

            if (fechaTextBox < fechaEntrega)
            {
                DateTime now = DateTime.Now;
                TextFec.Text = now.ToString("yyyy-MM-dd");

                string contenidoModalOT = "La programación del Show Case no puede ser menor a la fecha de entrega del diseño " + TextEntrega.Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#FechaSC').modal('show'); $('#FechaSC2').text('" + contenidoModalOT + "');", true);
            }
        }


        private int ObtenerMaximoNumeroDiseño(SqlConnection connection)
        {
            string queryGetMaxNumeroDiseño = "SELECT MAX(Numero_Diseño) FROM tbldiseño";
            int numerodiseño = 0;

            using (SqlCommand commandGetMaxNumeroDiseño = new SqlCommand(queryGetMaxNumeroDiseño, connection))
            {
                object result = commandGetMaxNumeroDiseño.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    numerodiseño = Convert.ToInt32(result) + 1;
                }
                else
                {
                    numerodiseño = 1;
                }
            }

            Session["MaxNumeroDiseno"] = numerodiseño;

            return numerodiseño;
        }

        private bool RealizarInsercion()
        {
            bool valorChecPiso = ChecPiso.Checked;
            bool valorChecCie = ChecCie.Checked;
            bool valorChecDiv = ChecDiv.Checked;
            bool valorChecCan = ChecCan.Checked;
            bool valorChecBteEle = ChecBteEle.Checked;
            bool valorChecBteSw = ChecBteSw.Checked;
            bool valorChecSujPt = ChecSujPt.Checked;
            bool valorChecPerRef = ChecPerRef.Checked;
            bool valorChecGuaEsc = ChecGuaEsc.Checked;
            bool valorChecCotVia = ChecCotVia.Checked;
            bool valorChecCotTte = CheckBox4.Checked;
            bool valorCheckBox18 = CheckBox18.Checked;
            bool valorCheckBox19 = CheckBox19.Checked;
            bool valorCheckBox20 = CheckBox20.Checked;
            bool valorCheckBox21 = CheckBox21.Checked;
            bool valorCheckBox22 = ChecMailTer.Checked;
            bool valorCheckBox23 = ChecCot.Checked;
            bool valorCheckBox24 = ChecUrgent.Checked;
            bool valorCheckBox25 = CheckBox16.Checked;


            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();

                int numerodiseño = ObtenerMaximoNumeroDiseño(connection);

                if (numerodiseño > 0)
                {
                    using (SqlCommand command = new SqlCommand("sp_InsertarDiseño", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_CiudadProyecto", TextCiuPro.Text);
                        command.Parameters.AddWithValue("@Mail", TextMail.Text);
                        command.Parameters.AddWithValue("@PresentacionCotizacion", TextPre.Text);
                        command.Parameters.AddWithValue("@Celular", TextCel.Text);
                        command.Parameters.AddWithValue("@Cotizartransporte", valorChecCotTte);
                        command.Parameters.AddWithValue("@CotizarViaticos", valorChecCotVia);
                        command.Parameters.AddWithValue("@MailTerminado", valorCheckBox22);
                        command.Parameters.AddWithValue("@Telefono", TextTel.Text);
                        command.Parameters.AddWithValue("@Contacto", TextContacto.Text);
                        command.Parameters.AddWithValue("@Zona", TextZona.Text);

                        DateTime fechaOkDib = DateTime.ParseExact(TextFecOkDib.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@FechaDibujoOK", fechaOkDib);

                        DateTime fechaProEnt = DateTime.ParseExact(TextEntrega.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@Fecha_Programada_Entrega", fechaProEnt);




                        command.Parameters.AddWithValue("@PasarACotizar", valorCheckBox23);
                        command.Parameters.AddWithValue("@Urgente", valorCheckBox24);
                        command.Parameters.AddWithValue("@PlanoBitacora", TextPla.Text);
                        command.Parameters.AddWithValue("@Nombre_Diseño", TextProyecto.Text);

                        // Obtener la fecha y hora del TextBox con type="datetime-local"
                        DateTime fechaHora = DateTime.ParseExact(TextUltAc.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                        // Utilizar el valor formateado en el comando SQL
                        command.Parameters.AddWithValue("@UltimaActivacion", fechaHora);

                        DateTime fechaIngreso = DateTime.ParseExact(TextIngDis.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);




                        command.Parameters.AddWithValue("@Asesor", DropDownList1.SelectedItem.Text);
                        command.Parameters.AddWithValue("@Cliente", TextCliente.Text);
                        command.Parameters.AddWithValue("@Direccion", TextDir.Text);
                        command.Parameters.AddWithValue("@Descuento", TextDes.Text);
                        command.Parameters.AddWithValue("@ConduccionCablesPiso", valorChecPiso);
                        command.Parameters.AddWithValue("@ConduccionCablesDivision", valorChecDiv);
                        command.Parameters.AddWithValue("@ConduccionCablesCielo", valorChecCie);
                        command.Parameters.AddWithValue("@ConduccionCablesCanaleta", valorChecCan);
                        command.Parameters.AddWithValue("@BajantesElectricos", valorChecBteEle);
                        command.Parameters.AddWithValue("@Bajantesswitches", valorChecBteSw);
                        command.Parameters.AddWithValue("@SujecionCielo", valorChecSujPt);
                        command.Parameters.AddWithValue("@PerfilRefuerzo", valorChecPerRef);
                        command.Parameters.AddWithValue("@GuardaEscobas", valorChecGuaEsc);
                        command.Parameters.AddWithValue("@AlturaCielo", TexHTot.Text);
                        command.Parameters.AddWithValue("@Linea", TextLin.Text);
                        command.Parameters.AddWithValue("@TipoMostrador", TextMos.Text);
                        command.Parameters.AddWithValue("@AcabadoSuperficie", TextSup.Text);
                        command.Parameters.AddWithValue("@BalanceSuperficies", valorCheckBox25);
                        command.Parameters.AddWithValue("@TipoSoporte", TextSop.Text);
                        command.Parameters.AddWithValue("@TipoGaveta", TextGav.Text);
                        command.Parameters.AddWithValue("@AcabadoPaneles", TextPan.Text);
                        command.Parameters.AddWithValue("@TipoTapaPierna", TextTapPie.Text);
                        command.Parameters.AddWithValue("@TipoRepisa", TextRep.Text);
                        command.Parameters.AddWithValue("@TipodePantalla", TextPant.Text);

                        command.Parameters.AddWithValue("@TipodeVidrio", TextTipVid.Text);

                        command.Parameters.AddWithValue("@TipoArchivador", TextArch.Text);
                        command.Parameters.AddWithValue("@MuebleCoco", TextCoc.Text);
                        command.Parameters.AddWithValue("@MuebleEntrepano", TextEnt.Text);
                        command.Parameters.AddWithValue("@MueblePuertas", TextPuer.Text);
                        command.Parameters.AddWithValue("@Observaciones_Ventas", TextObsVen.InnerText);
                        command.Parameters.AddWithValue("@Observaciones_Diseño", TextObsDibDes.InnerText);

                        command.Parameters.AddWithValue("@SC_Presentacionppt", valorCheckBox18);
                        command.Parameters.AddWithValue("@SC_Imagenes", valorCheckBox19);
                        command.Parameters.AddWithValue("@SC_Accesorios", valorCheckBox20);
                        command.Parameters.AddWithValue("@SC_Tiemporeal", valorCheckBox21);
                        command.Parameters.AddWithValue("@SC_Fecha", TextFec.Text);
                        command.Parameters.AddWithValue("@SC_Hora", TextFech.Text);
                        command.Parameters.AddWithValue("@SC_Ubicacion", TextUbi.Text);

                        command.Parameters.AddWithValue("@Numero_Diseño", numerodiseño);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Session["NumeroDiseño2"] = numerodiseño;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalExito').modal('show');", true);

                            // Inserción exitosa
                            return true;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);

                            // No se insertó ninguna fila, por lo que la inserción falló
                            return false;
                        }

                    }
                }
                else
                {
                    Response.Write("Error al obtener el número de diseño.");
                    return false;
                }
            }
        }

        private bool RealizarActualizacion()
        {
            bool valorChecPiso = ChecPiso.Checked;
            bool valorChecCie = ChecCie.Checked;
            bool valorChecDiv = ChecDiv.Checked;
            bool valorChecCan = ChecCan.Checked;
            bool valorChecBteEle = ChecBteEle.Checked;
            bool valorChecBteSw = ChecBteSw.Checked;
            bool valorChecSujPt = ChecSujPt.Checked;
            bool valorChecPerRef = ChecPerRef.Checked;
            bool valorChecGuaEsc = ChecGuaEsc.Checked;
            bool valorChecCotVia = ChecCotVia.Checked;
            bool valorChecCotTte = CheckBox4.Checked;
            bool valorCheckBox18 = CheckBox18.Checked;
            bool valorCheckBox19 = CheckBox19.Checked;
            bool valorCheckBox20 = CheckBox20.Checked;
            bool valorCheckBox21 = CheckBox21.Checked;
            bool valorCheckBox22 = ChecMailTer.Checked;
            bool valorCheckBox23 = ChecCot.Checked;
            bool valorCheckBox24 = ChecUrgent.Checked;
            bool valorCheckBox25 = CheckBox16.Checked;


            // Obtén el número de diseño de la etiqueta lblNumDise
            string numeroDiseño = lblNumDise.Text;

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Consulta SQL para verificar el campo ProgramadoVentas
                string consulta = "SELECT ProgramadoVentas FROM tbldiseño WHERE Numero_Diseño = @Numero_Diseño";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    // Asignar el valor del parámetro
                    command.Parameters.AddWithValue("@Numero_Diseño", numeroDiseño);

                    // Ejecutar la consulta y obtener el valor de ProgramadoVentas
                    bool programadoVentas = false; // Suponemos que el valor predeterminado es false

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            programadoVentas = reader.GetBoolean(0);
                        }
                    }

                    // Verificar el valor de ProgramadoVentas
                    if (programadoVentas)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ErrorModiciarDiseno').modal('show');", true);
                        return false;
                    }
                }



                // Crear el nombre del procedimiento almacenado
                string storedProcedureName = "sp_ActualizarDiseño";

                // Crear el comando para ejecutar el procedimiento almacenado
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Especificar que el comando es un procedimiento almacenado
                    command.CommandType = CommandType.StoredProcedure;

                    // Asignar los valores a los parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Nombre_Diseño", TextProyecto.Text);


                    command.Parameters.AddWithValue("@Numero_Diseño", lblNumDise.Text);
                    command.Parameters.AddWithValue("@id_CiudadProyecto", TextCiuPro.Text);
                    command.Parameters.AddWithValue("@Mail", TextMail.Text);
                    command.Parameters.AddWithValue("@PresentacionCotizacion", TextPre.Text);
                    command.Parameters.AddWithValue("@Celular", TextCel.Text);
                    command.Parameters.AddWithValue("@Cotizartransporte", valorChecCotTte);
                    command.Parameters.AddWithValue("@CotizarViaticos", valorChecCotVia);
                    command.Parameters.AddWithValue("@MailTerminado", valorCheckBox22);
                    command.Parameters.AddWithValue("@Telefono", TextTel.Text);
                    command.Parameters.AddWithValue("@Contacto", TextContacto.Text);
                    command.Parameters.AddWithValue("@Zona", TextZona.Text);

                    DateTime fechaOkDib = DateTime.ParseExact(TextFecOkDib.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                    command.Parameters.AddWithValue("@FechaDibujoOK", fechaOkDib);

                    DateTime fechaProEnt = DateTime.ParseExact(TextEntrega.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                    command.Parameters.AddWithValue("@Fecha_Programada_Entrega", fechaProEnt);

                    command.Parameters.AddWithValue("@PasarACotizar", valorCheckBox23);
                    command.Parameters.AddWithValue("@Urgente", valorCheckBox24);
                    command.Parameters.AddWithValue("@PlanoBitacora", TextPla.Text);

                    // Obtener la fecha y hora del TextBox con type="datetime-local"
                    DateTime fechaHora = DateTime.ParseExact(TextUltAc.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                    // Utilizar el valor formateado en el comando SQL
                    command.Parameters.AddWithValue("@UltimaActivacion", fechaHora);

                    DateTime fechaIngreso = DateTime.ParseExact(TextIngDis.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                    command.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);

                    command.Parameters.AddWithValue("@Asesor", DropDownList1.SelectedItem.Text);
                    command.Parameters.AddWithValue("@Cliente", TextCliente.Text);
                    command.Parameters.AddWithValue("@Direccion", TextDir.Text);
                    command.Parameters.AddWithValue("@Descuento", TextDes.Text);
                    command.Parameters.AddWithValue("@ConduccionCablesPiso", valorChecPiso);
                    command.Parameters.AddWithValue("@ConduccionCablesDivision", valorChecDiv);
                    command.Parameters.AddWithValue("@ConduccionCablesCielo", valorChecCie);
                    command.Parameters.AddWithValue("@ConduccionCablesCanaleta", valorChecCan);
                    command.Parameters.AddWithValue("@BajantesElectricos", valorChecBteEle);
                    command.Parameters.AddWithValue("@Bajantesswitches", valorChecBteSw);
                    command.Parameters.AddWithValue("@SujecionCielo", valorChecSujPt);
                    command.Parameters.AddWithValue("@PerfilRefuerzo", valorChecPerRef);
                    command.Parameters.AddWithValue("@GuardaEscobas", valorChecGuaEsc);
                    command.Parameters.AddWithValue("@AlturaCielo", TexHTot.Text);
                    command.Parameters.AddWithValue("@Linea", TextLin.Text);
                    command.Parameters.AddWithValue("@TipoMostrador", TextMos.Text);
                    command.Parameters.AddWithValue("@AcabadoSuperficie", TextSup.Text);
                    command.Parameters.AddWithValue("@BalanceSuperficies", valorCheckBox25);
                    command.Parameters.AddWithValue("@TipoSoporte", TextSop.Text);
                    command.Parameters.AddWithValue("@TipoGaveta", TextGav.Text);
                    command.Parameters.AddWithValue("@AcabadoPaneles", TextPan.Text);
                    command.Parameters.AddWithValue("@TipoTapaPierna", TextTapPie.Text);
                    command.Parameters.AddWithValue("@TipoRepisa", TextRep.Text);
                    command.Parameters.AddWithValue("@TipodePantalla", TextPant.Text);
                    command.Parameters.AddWithValue("@TipodeVidrio", TextTipVid.Text);
                    command.Parameters.AddWithValue("@TipoArchivador", TextArch.Text);
                    command.Parameters.AddWithValue("@MuebleCoco", TextCoc.Text);
                    command.Parameters.AddWithValue("@MuebleEntrepano", TextEnt.Text);
                    command.Parameters.AddWithValue("@MueblePuertas", TextPuer.Text);
                    command.Parameters.AddWithValue("@Observaciones_Ventas", TextObsVen.InnerText);
                    command.Parameters.AddWithValue("@Observaciones_Diseño", TextObsDibDes.InnerText);
                    command.Parameters.AddWithValue("@SeguimientoPausa", TextSegPauDev.InnerText);

                    command.Parameters.AddWithValue("@SC_Presentacionppt", valorCheckBox18);
                    command.Parameters.AddWithValue("@SC_Imagenes", valorCheckBox19);
                    command.Parameters.AddWithValue("@SC_Accesorios", valorCheckBox20);
                    command.Parameters.AddWithValue("@SC_Tiemporeal", valorCheckBox21);
                    command.Parameters.AddWithValue("@SC_Fecha", TextFec.Text);
                    command.Parameters.AddWithValue("@SC_Hora", TextFech.Text);
                    command.Parameters.AddWithValue("@SC_Ubicacion", TextUbi.Text);
                    // Ejecutar el procedimiento almacenado
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["NumeroDiseño2"] = lblNumDise.Text;



                        // Inserción exitosa
                        return true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);

                        // No se insertó ninguna fila, por lo que la inserción falló
                        return false;
                    }
                }
            }


        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button9
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button10
        }

        //DATAGRID DISEÑO
        protected void DataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string programadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();
                string pasarACotizar = DataBinder.Eval(e.Item.DataItem, "PasarACotizar").ToString();
                string terminadoDibujo = DataBinder.Eval(e.Item.DataItem, "TerminadoDibujo").ToString();
                string pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();
                string FechaEntrega = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();
                string urgente = DataBinder.Eval(e.Item.DataItem, "Urgente").ToString();

                if (DateTime.TryParse(FechaEntrega, out DateTime fechaEntrega))
                {
                    if (fechaEntrega < DateTime.Now && programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {

                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /* Rojo */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False" && urgente == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /* Amarillo */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (programadoVentas == "True" && pasarACotizar == "True" && terminadoDibujo == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /* Violeta */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#819cba"); /* Azul */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "True" && pasarACotizar == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#be94b9"); /* Rosado */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (pausado == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#74bec6"); /* Celeste */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (urgente == "True" && programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#e9a270"); /* Naranja */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }





                    // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                    string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                    string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                    TableCell descripcionCell = e.Item.Cells[3];

                    if (descripcionCell != null)
                    {
                        descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                    }
                }
            }
        }

        protected void DataGridDise_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            ProcesarNumeroDiseño(e);

        }

        private void ProcesarNumeroDiseño(DataGridCommandEventArgs e)
        {
            string numeroDiseño = Session["NumeroDiseño"].ToString();


            if (int.TryParse(numeroDiseño, out int numDise))
            {
                // Establecer el valor del parámetro en el SqlDataSource
                SqldatasourceTxt.SelectParameters["NumeroDiseño"].DefaultValue = numDise.ToString();

                // Ejecutar el SqlDataSource
                SqldatasourceTxt.DataBind();

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para verificar el campo ProgramadoVentas
                    string consultaProgramadoVentas = "SELECT ProgramadoVentas FROM tbldiseño WHERE Numero_Diseño = @Numero_Diseño";

                    using (SqlCommand commandProgramadoVentas = new SqlCommand(consultaProgramadoVentas, connection))
                    {
                        commandProgramadoVentas.Parameters.AddWithValue("@Numero_Diseño", numDise);

                        bool programadoVentas = false; // Valor predeterminado

                        using (SqlDataReader readerProgramadoVentas = commandProgramadoVentas.ExecuteReader())
                        {
                            if (readerProgramadoVentas.Read())
                            {
                                programadoVentas = readerProgramadoVentas.GetBoolean(0);
                            }
                        }

                        string tipoAccion = Session["Diseno"] as string;
                        if (tipoAccion == "Ventas")
                        {
                            // Validar ProgramadoVentas
                            if (programadoVentas)
                            {
                                // ProgramadoVentas es 1, deshabilitar los botones
                                BtnProgramar.Enabled = false;
                                BtnProgramar.CssClass = "btn btn-warning shadow btn-sm fw-bold";

                            }
                            else
                            {
                                // ProgramadoVentas es 0, habilitar los botones
                                BtnProgramar.Enabled = true;
                                BtnProgramar.CssClass = "btn btn-warning shadow btn-sm fw-bold";
                            }
                        }
                        else if (tipoAccion == "Recepcion")
                        {
                            ValidarBotonTerminarRecep();
                        }

                       
                    }

                    using (SqlCommand command = new SqlCommand("sp_FormularioDisBita", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NumDise", numDise);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int idCiudadProyecto = reader.GetInt32(reader.GetOrdinal("id_CiudadProyecto"));

                                // Asignar el valor de "id_CiudadProyecto" al DropDownList
                                TextCiuPro.SelectedValue = idCiudadProyecto.ToString();

                                // Asignar el valor de "Numero_Diseño" al lblNumDise
                                int numeroDiseno;
                                if (int.TryParse(reader["Numero_Diseño"].ToString(), out numeroDiseno))
                                {
                                    lblNumDise.Text = numeroDiseno.ToString();
                                    Session["NumeroDisenoSession"] = numeroDiseno;
                                    Session["EventoItemCommandEjecutado"] = true;
                                }
                                else
                                {
                                    lblNumDise.Text = "Valor no válido";
                                    Session["EventoItemCommandEjecutado"] = false;
                                }


                                // Validar ProgramadoVentas
                                bool programadoVentas = reader.GetBoolean(reader.GetOrdinal("ProgramadoVentas"));

                                if (programadoVentas)
                                {
                                    TextIngDis.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso")).ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = reader.GetDateTime(reader.GetOrdinal("UltimaActivacion")).ToString("yyyy-MM-ddTHH:mm");
                                    TextEntrega.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega")).ToString("yyyy-MM-ddTHH:mm");
                                    TextFecOkDib.Text = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK")).ToString("yyyy-MM-ddTHH:mm");

                                    TextFec.Text = reader.GetDateTime(reader.GetOrdinal("SC_Fecha")).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    // Asignar las fechas cuando ProgramadoVentas es 0
                                    DateTime now = DateTime.Now;
                                    TextFec.Text = now.ToString("yyyy-MM-dd");
                                    //TextFech.Text = now.ToString("HH:mm");
                                    TextIngDis.Text = now.ToString("yyyy-MM-ddTHH:mm");
                                    string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = fechaHoraActual;
                                    TextFecOkDib.Text = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK")).ToString("yyyy-MM-ddTHH:mm");
                                    TextEntrega.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega")).ToString("yyyy-MM-ddTHH:mm");
                                }

                                //TextFech.Text = reader.GetDateTime(reader.GetOrdinal("SC_Hora")).ToString("HH:mm");

                                TextZona.Text = GetString(reader, "Zona");

                                TextPre.Text = GetString(reader, "PresentacionCotizacion");

                                string asesor = GetString(reader, "Asesor");
                                ListItem asesorItem = new ListItem(asesor, asesor);
                                DropDownList1.Items.Add(asesorItem);

                                // Selecciona el valor en DropDownList1 para que coincida con el "Asesor" obtenido de la base de datos
                                DropDownList1.SelectedValue = asesor;


                                TextCliente.Text = GetString(reader, "Cliente");
                                TextProyecto.Text = GetString(reader, "Nombre_Diseño");
                                TextContacto.Text = GetString(reader, "Contacto");
                                TextTel.Text = GetString(reader, "Telefono");
                                TextCel.Text = GetString(reader, "Celular");
                                TextMail.Text = GetString(reader, "Mail");
                                TextDir.Text = GetString(reader, "Direccion");
                                TextDes.Text = GetString(reader, "Descuento");
                                TextPla.Text = GetString(reader, "PlanoBitacora");
                                ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("Urgente"));
                                ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("PasarACotizar"));
                                ChecMailTer.Checked = reader.GetBoolean(reader.GetOrdinal("MailTerminado"));

                                ChecCotVia.Checked = reader.GetBoolean(reader.GetOrdinal("CotizarViaticos"));

                                CheckBox4.Checked = reader.GetBoolean(reader.GetOrdinal("Cotizartransporte"));

                                CheckBox16.Checked = reader.GetBoolean(reader.GetOrdinal("BalanceSuperficies"));

                                CheckBox18.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Presentacionppt"));

                                CheckBox19.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Imagenes"));

                                CheckBox20.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Accesorios"));

                                CheckBox21.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Tiemporeal"));

                                TexHTot.Text = GetString(reader, ("AlturaCielo"));

                                TextUbi.Text = GetString(reader, ("SC_Ubicacion"));

                                ChecPiso.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesPiso"));
                                ChecCie.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCielo"));
                                ChecBteEle.Checked = reader.GetBoolean(reader.GetOrdinal("BajantesElectricos"));
                                ChecBteSw.Checked = reader.GetBoolean(reader.GetOrdinal("Bajantesswitches"));
                                ChecDiv.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesDivision"));
                                ChecCan.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCanaleta"));

                                ChecAlCie.Checked = reader.GetBoolean(reader.GetOrdinal("SujecionCielo"));
                                ChecPerRef.Checked = reader.GetBoolean(reader.GetOrdinal("PerfilRefuerzo"));
                                ChecGuaEsc.Checked = reader.GetBoolean(reader.GetOrdinal("GuardaEscobas"));

                                TextSup.Text = GetString(reader, ("AcabadoSuperficie"));
                                TextPan.Text = GetString(reader, ("AcabadoPaneles"));
                                TextTipVid.Text = GetString(reader, ("TipodeVidrio"));
                                TextLin.Text = GetString(reader, ("Linea"));
                                TextSop.Text = GetString(reader, ("TipoSoporte"));
                                TextTapPie.Text = GetString(reader, ("TipoTapaPierna"));
                                TextPant.Text = GetString(reader, ("TipodePantalla"));
                                TextMos.Text = GetString(reader, ("TipoMostrador"));
                                TextGav.Text = GetString(reader, ("TipoGaveta"));
                                TextRep.Text = GetString(reader, ("TipoRepisa"));
                                TextArch.Text = GetString(reader, ("TipoArchivador"));
                                TextCoc.Text = GetString(reader, ("MuebleCoco"));
                                TextEnt.Text = GetString(reader, ("MuebleEntrepano"));
                                TextPuer.Text = GetString(reader, ("MueblePuertas"));
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Ventas")))
                                {
                                    TextObsVen.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Ventas"));
                                }
                                else
                                {
                                    // Si el valor es nulo, puedes manejarlo de alguna manera, por ejemplo, asignar un valor predeterminado a TextObsVen.InnerText
                                    TextObsVen.InnerText = " ";
                                }
                                // Para TextObsDibDes
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Diseño")))
                                {
                                    TextObsDibDes.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Diseño"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextObsDibDes
                                    TextObsDibDes.InnerText = " ";
                                }

                                // Para TextSegPauDev
                                if (!reader.IsDBNull(reader.GetOrdinal("SeguimientoPausa")))
                                {
                                    TextSegPauDev.InnerText = reader.GetString(reader.GetOrdinal("SeguimientoPausa"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextSegPauDev
                                    TextSegPauDev.InnerText = " ";
                                }
                            }
                        }
                    }
                }

                // Actualizar el panel de diseño de bitácora
                UpdateDiseñoBitacora.Update();
            }

            Session.Remove("NumeroDiseño");
        }

        private void ProcesarNumeroDiseño2(DataGridCommandEventArgs e)
        {
            string numeroDiseño = Session["NumeroDiseño2"].ToString();


            if (int.TryParse(numeroDiseño, out int numDise))
            {
                // Establecer el valor del parámetro en el SqlDataSource
                SqldatasourceTxt.SelectParameters["NumeroDiseño"].DefaultValue = numDise.ToString();

                // Ejecutar el SqlDataSource
                SqldatasourceTxt.DataBind();

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL para verificar el campo ProgramadoVentas
                    string consultaProgramadoVentas = "SELECT ProgramadoVentas FROM tbldiseño WHERE Numero_Diseño = @Numero_Diseño";

                    using (SqlCommand commandProgramadoVentas = new SqlCommand(consultaProgramadoVentas, connection))
                    {
                        commandProgramadoVentas.Parameters.AddWithValue("@Numero_Diseño", numDise);

                        bool programadoVentas = false; // Valor predeterminado

                        using (SqlDataReader readerProgramadoVentas = commandProgramadoVentas.ExecuteReader())
                        {
                            if (readerProgramadoVentas.Read())
                            {
                                programadoVentas = readerProgramadoVentas.GetBoolean(0);
                            }
                        }

                        string tipoAccion = Session["Diseno"] as string;
                        if (tipoAccion == "Ventas")
                        {
                            // Validar ProgramadoVentas
                            if (programadoVentas)
                            {
                                // ProgramadoVentas es 1, deshabilitar los botones
                                BtnProgramar.Enabled = false;
                                BtnProgramar.CssClass = "btn btn-warning shadow btn-sm fw-bold";

                            }
                            else
                            {
                                // ProgramadoVentas es 0, habilitar los botones
                                BtnProgramar.Enabled = true;
                                BtnProgramar.CssClass = "btn btn-warning shadow btn-sm fw-bold";
                            }
                        }
                        else if (tipoAccion == "Recepcion")
                        {
                            ValidarBotonTerminarRecep();
                        }
                    }

                    using (SqlCommand command = new SqlCommand("sp_FormularioDisBita", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NumDise", numDise);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int idCiudadProyecto = reader.GetInt32(reader.GetOrdinal("id_CiudadProyecto"));

                                // Asignar el valor de "id_CiudadProyecto" al DropDownList
                                TextCiuPro.SelectedValue = idCiudadProyecto.ToString();

                                // Asignar el valor de "Numero_Diseño" al lblNumDise
                                int numeroDiseno;
                                if (int.TryParse(reader["Numero_Diseño"].ToString(), out numeroDiseno))
                                {
                                    lblNumDise.Text = numeroDiseno.ToString();
                                    Session["NumeroDisenoSession"] = numeroDiseno;
                                    Session["EventoItemCommandEjecutado"] = true;
                                }
                                else
                                {
                                    lblNumDise.Text = "Valor no válido";
                                    Session["EventoItemCommandEjecutado"] = false;
                                }

                                // Validar ProgramadoVentas
                                bool programadoVentas = reader.GetBoolean(reader.GetOrdinal("ProgramadoVentas"));

                                if (programadoVentas)
                                {
                                    TextIngDis.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso")).ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = reader.GetDateTime(reader.GetOrdinal("UltimaActivacion")).ToString("yyyy-MM-ddTHH:mm");
                                    TextEntrega.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega")).ToString("yyyy-MM-ddTHH:mm");
                                    TextFecOkDib.Text = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK")).ToString("yyyy-MM-ddTHH:mm");

                                    TextFec.Text = reader.GetDateTime(reader.GetOrdinal("SC_Fecha")).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    // Asignar las fechas cuando ProgramadoVentas es 0
                                    DateTime now = DateTime.Now;
                                    TextFec.Text = now.ToString("yyyy-MM-dd");
                                    //TextFech.Text = now.ToString("HH:mm");
                                    TextIngDis.Text = now.ToString("yyyy-MM-ddTHH:mm");
                                    string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = fechaHoraActual;
                                    TextFecOkDib.Text = now.ToString("yyyy-MM-ddTHH:mm");
                                    DateTime fechaActual = DateTime.Now;

                                    // Sumar 3 días hábiles a partir de la fecha actual
                                    DateTime fechaProgramadaEntrega = ObtenerProximaFechaHabil(fechaActual, 3);

                                    // Asignar la fecha programada de entrega al TextBox
                                    TextEntrega.Text = fechaProgramadaEntrega.ToString("yyyy-MM-ddTHH:mm");
                                }

                                //TextFech.Text = reader.GetDateTime(reader.GetOrdinal("SC_Hora")).ToString("HH:mm");

                                TextZona.Text = GetString(reader, "Zona");

                                TextPre.Text = GetString(reader, "PresentacionCotizacion");

                                string asesor = GetString(reader, "Asesor");
                                ListItem asesorItem = new ListItem(asesor, asesor);
                                DropDownList1.Items.Add(asesorItem);

                                // Selecciona el valor en DropDownList1 para que coincida con el "Asesor" obtenido de la base de datos
                                DropDownList1.SelectedValue = asesor;


                                TextCliente.Text = GetString(reader, "Cliente");
                                TextProyecto.Text = GetString(reader, "Nombre_Diseño");
                                TextContacto.Text = GetString(reader, "Contacto");
                                TextTel.Text = GetString(reader, "Telefono");
                                TextCel.Text = GetString(reader, "Celular");
                                TextMail.Text = GetString(reader, "Mail");
                                TextDir.Text = GetString(reader, "Direccion");
                                TextDes.Text = GetString(reader, "Descuento");
                                TextPla.Text = GetString(reader, "PlanoBitacora");
                                ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("Urgente"));
                                ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("PasarACotizar"));
                                ChecMailTer.Checked = reader.GetBoolean(reader.GetOrdinal("MailTerminado"));

                                ChecCotVia.Checked = reader.GetBoolean(reader.GetOrdinal("CotizarViaticos"));

                                CheckBox4.Checked = reader.GetBoolean(reader.GetOrdinal("Cotizartransporte"));

                                CheckBox16.Checked = reader.GetBoolean(reader.GetOrdinal("BalanceSuperficies"));

                                CheckBox18.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Presentacionppt"));

                                CheckBox19.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Imagenes"));

                                CheckBox20.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Accesorios"));

                                CheckBox21.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Tiemporeal"));

                                TexHTot.Text = GetString(reader, ("AlturaCielo"));

                                TextUbi.Text = GetString(reader, ("SC_Ubicacion"));

                                ChecPiso.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesPiso"));
                                ChecCie.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCielo"));
                                ChecBteEle.Checked = reader.GetBoolean(reader.GetOrdinal("BajantesElectricos"));
                                ChecBteSw.Checked = reader.GetBoolean(reader.GetOrdinal("Bajantesswitches"));
                                ChecDiv.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesDivision"));
                                ChecCan.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCanaleta"));

                                ChecAlCie.Checked = reader.GetBoolean(reader.GetOrdinal("SujecionCielo"));
                                ChecPerRef.Checked = reader.GetBoolean(reader.GetOrdinal("PerfilRefuerzo"));
                                ChecGuaEsc.Checked = reader.GetBoolean(reader.GetOrdinal("GuardaEscobas"));

                                TextSup.Text = GetString(reader, ("AcabadoSuperficie"));
                                TextPan.Text = GetString(reader, ("AcabadoPaneles"));
                                TextTipVid.Text = GetString(reader, ("TipodeVidrio"));
                                TextLin.Text = GetString(reader, ("Linea"));
                                TextSop.Text = GetString(reader, ("TipoSoporte"));
                                TextTapPie.Text = GetString(reader, ("TipoTapaPierna"));
                                TextPant.Text = GetString(reader, ("TipodePantalla"));
                                TextMos.Text = GetString(reader, ("TipoMostrador"));
                                TextGav.Text = GetString(reader, ("TipoGaveta"));
                                TextRep.Text = GetString(reader, ("TipoRepisa"));
                                TextArch.Text = GetString(reader, ("TipoArchivador"));
                                TextCoc.Text = GetString(reader, ("MuebleCoco"));
                                TextEnt.Text = GetString(reader, ("MuebleEntrepano"));
                                TextPuer.Text = GetString(reader, ("MueblePuertas"));
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Ventas")))
                                {
                                    TextObsVen.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Ventas"));
                                }
                                else
                                {
                                    // Si el valor es nulo, puedes manejarlo de alguna manera, por ejemplo, asignar un valor predeterminado a TextObsVen.InnerText
                                    TextObsVen.InnerText = " ";
                                }
                                // Para TextObsDibDes
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Diseño")))
                                {
                                    TextObsDibDes.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Diseño"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextObsDibDes
                                    TextObsDibDes.InnerText = " ";
                                }

                                // Para TextSegPauDev
                                if (!reader.IsDBNull(reader.GetOrdinal("SeguimientoPausa")))
                                {
                                    TextSegPauDev.InnerText = reader.GetString(reader.GetOrdinal("SeguimientoPausa"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextSegPauDev
                                    TextSegPauDev.InnerText = " ";
                                }
                            }
                        }
                    }
                }

                // Actualizar el panel de diseño de bitácora
                UpdateDiseñoBitacora.Update();
            }

            Session.Remove("NumeroDiseño2");
        }

        protected void DataGridSC(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Numero_Diseño")
            {
                ProcesarNumeroDiseño(e);
            }
        }

        protected void DataGridBusDise_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Numero_Diseño")
            {
                ProcesarNumeroDiseño(e);
            }
        }

        private string GetString(SqlDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return reader.GetString(reader.GetOrdinal(columnName));
            }
            return string.Empty;
        }

        private void UpdateDataGrids()
        {
            DataGrid1.DataBind();
            DataGrid2.DataBind();
            DataGrid3.DataBind();
            DataGridDiseños.DataBind();
            DataGridRender.DataBind();
            UpdatePanel1.Update();
        }

        protected void DropDownListOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["Diseno"] as string;
            if (tipoAccion == "Ventas")
            {
                CargarDatagridVentas();
            }
            else if (tipoAccion == "Diseño")
            {
                CargarDatagridDise();
            } 
        }

        //DATAGRID SHOWCASE
        protected void DataGrid3_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string scFecha = DataBinder.Eval(e.Item.DataItem, "SC_Fecha").ToString();
                DateTime fechaSC;

                // Convertir la cadena de fecha a un objeto DateTime
                if (DateTime.TryParse(scFecha, out fechaSC))
                {
                    // Verificar si la fecha es igual o menor a la fecha actual
                    if (fechaSC <= DateTime.Now)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); // Rojo
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); // Amarillo
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                }


                // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                // Encontrar la celda correspondiente a la columna Descripción por índice
                TableCell descripcionCell = e.Item.Cells[3]; // Ajusta el índice si es necesario

                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                if (descripcionCell != null)
                {
                    descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                }
            }
        }

        protected void DataGrid4_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string terminadoRender = DataBinder.Eval(e.Item.DataItem, "TerminadoRender").ToString();
                string fechaProgramadaString = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();
                string Pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();
                string ProgramadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();


                if (terminadoRender == "False")
                {
                    if (!string.IsNullOrEmpty(fechaProgramadaString) && DateTime.TryParse(fechaProgramadaString, out DateTime fechaProgramada))
                    {
                        if (fechaProgramada < DateTime.Now && Pausado == "False" && ProgramadoVentas == "True")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /*Rojo*/
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }
                        else if (fechaProgramada > DateTime.Now && Pausado == "False" && ProgramadoVentas == "True")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                            e.Item.ForeColor = System.Drawing.Color.Black;
                        }
                        else if (Pausado == "True")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#74bec6"); /*Celeste*/
                            e.Item.ForeColor = System.Drawing.Color.Black;
                        }
                        else if (ProgramadoVentas == "False")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /*Violeta*/
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }

                    }
                }


            }



            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                string nombreRender = DataBinder.Eval(e.Item.DataItem, "Nombre_Render").ToString();

                // Encontrar la celda correspondiente a la columna Descripción por índice
                TableCell descripcionCell = e.Item.Cells[3]; // Ajusta el índice si es necesario

                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                if (descripcionCell != null)
                {
                    descripcionCell.Text = $"{cliente} - {nombreRender}";
                }
            }
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DateTime fechaEntrega = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Entrega_Dibujo_Despiece"));
                DateTime fechaActualMenos2Dias = DateTime.Now.AddDays(-2);

                // Verificar si la fecha de entrega es sábado o domingo
                if (fechaEntrega.DayOfWeek == DayOfWeek.Saturday)
                {
                    // Cambiar la fecha de entrega al próximo lunes
                    fechaEntrega = fechaEntrega.AddDays(2);
                }
                else if (fechaEntrega.DayOfWeek == DayOfWeek.Sunday)
                {
                    // Cambiar la fecha de entrega al próximo martes
                    fechaEntrega = fechaEntrega.AddDays(1);
                }

                // Verificar si la nueva fecha de entrega es menos de 5 días antes de la fecha actual
                if (fechaEntrega < fechaActualMenos2Dias)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /*rojo*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#efdd79"); /*Amarillo*/
                    e.Item.ForeColor = System.Drawing.Color.Black;
                }

                // Acceder a la celda correspondiente y asignarle el valor de fechaEntrega
                TableCell cellFechaEntrega = e.Item.Cells[6]; // Cambia el índice si la columna no está en la sexta posición
                cellFechaEntrega.Text = fechaEntrega.ToString("dd/MM/yyyy hh:mm:ss tt");

                // Obtener la referencia al control Label dentro de la columna de la fecha de entrega
                System.Web.UI.WebControls.Label labelFechaEntrega = (System.Web.UI.WebControls.Label)e.Item.FindControl("Label1");

                // Verificar si se encontró el control Label
                if (labelFechaEntrega != null)
                {
                    // Calcular la fecha de entrega para el control Label y asignarla como texto
                    DateTime nuevaFechaEntrega = fechaEntrega.AddDays(2);
                    labelFechaEntrega.Text = nuevaFechaEntrega.ToString("dd/MM/yyyy hh:mm:ss tt");
                }
            }
        }

        protected void CheckBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox22.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal1').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        protected void CheckBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox23.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal2').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        protected void DataGridBusDis_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string programadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();
                string pasarACotizar = DataBinder.Eval(e.Item.DataItem, "PasarACotizar").ToString();
                string terminadoDibujo = DataBinder.Eval(e.Item.DataItem, "TerminadoDibujo").ToString();
                string pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();
                string CotizacionOk = DataBinder.Eval(e.Item.DataItem, "CotizaciónOK").ToString();
                string FechaEntrega = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();


                if (DateTime.TryParse(FechaEntrega, out DateTime fechaEntrega))
                {
                    if (fechaEntrega < DateTime.Now && programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {

                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /* Rojo */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }

                    else if (programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (programadoVentas == "True" && terminadoDibujo == "True" && CotizacionOk == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#3d803f"); /*Verde*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }

                    else if (programadoVentas == "True" && pasarACotizar == "True" && terminadoDibujo == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /*Violeta*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#819cba"); /*Azul*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "True" && pasarACotizar == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#be94b9"); /*Rosado*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }







                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void DocBitacora_Click(object sender, EventArgs e)
        {
            string lblText = lblNumDise.Text;

            if (int.TryParse(lblText, out int numDise))
            {
                // Si el contenido es un número, puedes proceder con la lógica
                Session["NumeroDiseño"] = lblText;

                //DocBitacora.CssClass = "btn btn-sm shadow button-enabled linkButtonClicked";

                string url = "DocumentacionDise.aspx";
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#NumeroDiseñoNoValido').modal('show');", true);
            }

        }

        protected void lnkSelectRowRender_Click(object sender, EventArgs e)
        {

        }

        protected void CargarVSC_Click(object sender, EventArgs e)
        {
            if (Session["NumeroDiseño5"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño5"].ToString()))
            {
                Session["NumeroDiseño2"] = Session["NumeroDiseño5"];

                Session["AsesorDiseño"] = Session["CedulaLogeada"].ToString();
            }
            else if (int.TryParse(lblNumDise.Text, out _))
            {
                // Si el valor del label es numérico
                string diseño = lblNumDise.Text;

                Session["NumeroDiseño2"] = diseño;

                Session["AsesorDiseño"] = Session["CedulaLogeada"].ToString();
            }
            else
            {
                Session["AsesorDiseño"] = Session["CedulaLogeada"].ToString();

            }

        }

        protected void ChecConDeCab_CheckedChanged(object sender, EventArgs e)
        {
            if (ChecConDeCab.Checked)
            {
                ChecPiso.Checked = true;
                ChecDiv.Checked = true;
                ChecCie.Checked = true;
                ChecCan.Checked = true;
                ChecBteEle.Checked = true;
                ChecBteSw.Checked = true;
            }
            else
            {
                ChecPiso.Checked = false;
                ChecDiv.Checked = false;
                ChecCie.Checked = false;
                ChecCan.Checked = false;
                ChecBteEle.Checked = false;
                ChecBteSw.Checked = false;
            }
        }

        protected void ChecSujPt_CheckedChanged(object sender, EventArgs e)
        {
            if (ChecSujPt.Checked)
            {
                ChecAlCie.Checked = true;
                ChecPerRef.Checked = true;
                ChecGuaEsc.Checked = true;
                TexHTot.Text = "0";
            }
            else
            {
                ChecAlCie.Checked = false;
                ChecPerRef.Checked = false;
                ChecGuaEsc.Checked = false;
                TexHTot.Text = "";
            }
        }

        protected void ChecMue_CheckedChanged(object sender, EventArgs e)
        {
            if (ChecMue.Checked)
            {
                TextCoc.Text = "NA";
                TextEnt.Text = "NA";
                TextPuer.Text = "NA";
            }
            else
            {
                TextCoc.Text = "";
                TextEnt.Text = "";
                TextPuer.Text = "";
            }
        }

        protected void CheckEsyMat_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckEsyMat.Checked)
            {
                TextLin.Text = "NA";
                TextMos.Text = "NA";
                TextSup.Text = "NA";
                TextSop.Text = "NA";
                TextGav.Text = "NA";
                TextPan.Text = "NA";
                TextTapPie.Text = "NA";
                TextRep.Text = "NA";
                TextTipVid.Text = "NA";
                TextPant.Text = "NA";
                TextArch.Text = "NA";
            }
            else
            {
                TextLin.Text = "";
                TextMos.Text = "";
                TextSup.Text = "";
                TextSop.Text = "";
                TextGav.Text = "";
                TextPan.Text = "";
                TextTapPie.Text = "";
                TextRep.Text = "";
                TextTipVid.Text = "";
                TextPant.Text = "";
                TextArch.Text = "";
            }
        }

        //EVENTOS RECEPCION

        //EVENTOS RECEPCION

        protected void VisualizarCotPrecioActual_Click(object sender, EventArgs e)
        {
            // Crear un nuevo libro de Excel
            IWorkbook workbook = new HSSFWorkbook();
    
            CreateCotizacionSheet(workbook.CreateSheet("Cotizacion"));
            CreateCotizacionSheet(workbook.CreateSheet("Cotizacion Detallada"));

            // Guardar y descargar el archivo de Excel
            DownloadExcelFile(workbook);
        }

        // Método para crear una hoja de cotización
        private void CreateCotizacionSheet(ISheet sheet)
        {
            // Llenar la hoja de Cotizacion Detallada
            if (sheet.SheetName == "Cotizacion Detallada")
            {
                FillCotizacionDetalladaSheet(sheet);
            }

            // Llenar la hoja de Cotizacion Detallada
            if (sheet.SheetName == "Cotizacion")
            {
                // Llenar la hoja de Cotizacion
                FillCotizacionSheet(sheet);
            }

            // Crear el estilo de fuente y establecer la fuente
            IFont font = sheet.Workbook.CreateFont();
            font.FontName = "Century Gothic";
            font.FontHeightInPoints = 11;
            font.IsBold = true;

            // Crear el estilo de celda y establecer la fuente
            ICellStyle style = sheet.Workbook.CreateCellStyle();
            style.SetFont(font);

            // Crear la fila y las celdas para la fecha y el número de cotización
            IRow fechaRow = sheet.CreateRow(0);
            ICell fechaCell = fechaRow.CreateCell(1);
            fechaCell.SetCellValue($"Sabaneta, {DateTime.Now.ToString("MMMM d")} de {DateTime.Now.ToString("yyyy")}");
            fechaCell.CellStyle = style;

            // Agregar la imagen
            string imagePath = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg"; // Ruta de la imagen
            if (File.Exists(imagePath))
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // Convertir bytes de la imagen a un objeto HSSFWorkbook
                int pictureIdx = sheet.Workbook.AddPicture(imageBytes, PictureType.JPEG);

                // Crear el ancla de la imagen (ubicación en la hoja)
                IDrawing patriarch = sheet.CreateDrawingPatriarch();
                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 1, 0, 2, 1); // Celda de la esquina superior izquierda

                // Crear la forma de la imagen
                HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

                // Ajustar el tamaño de la imagen (opcional)
                picture.Resize(0.6, 0.8); // Puedes ajustar el tamaño según tu necesidad
            }

            ICell cotizacionCell = fechaRow.CreateCell(4);
            cotizacionCell.SetCellValue("Cotizacion N°");
            cotizacionCell.CellStyle = style;

            fechaRow.HeightInPoints = sheet.DefaultRowHeightInPoints * 6;

            // Agregar fila vacía después del encabezado
            sheet.CreateRow(2);

            // Agregar "Señores" en negrita en la celda B4
            IRow senoresRow1 = sheet.CreateRow(3);
            ICell senoresCell1 = senoresRow1.CreateCell(1);
            senoresCell1.SetCellValue("Señores");
            senoresCell1.CellStyle = style;

            // Agregar datos en negrita desde la celda B5 hacia abajo
            int currentRow = 4;
            foreach (var dato in new List<string> { TextContacto.Text, TextCliente.Text, "Ciudad" })
            {
                IRow dataRow = sheet.CreateRow(currentRow++);
                ICell dataCell = dataRow.CreateCell(1);
                dataCell.SetCellValue(dato);
                dataCell.CellStyle = style;
            }

            // Agregar fila vacía después del encabezado
            sheet.CreateRow(7);

            // Agregar "Diseño: lblNumDise.Text" en negrita en la celda E1
            IRow opcionRow = sheet.CreateRow(1);
            ICell opcionCell = opcionRow.CreateCell(4);
            opcionCell.SetCellValue("Diseño: " + lblNumDise.Text);

            // Agregar "REF. " + TextProyecto.Text en negrita en la celda B8
            IRow refRow = sheet.CreateRow(8);
            ICell refCell = refRow.CreateCell(1);
            refCell.SetCellValue("REF. " + TextProyecto.Text);
            refCell.CellStyle = style;

            // Agregar fila vacía después del encabezado
            sheet.CreateRow(9);

            // Agregar el texto en las celdas fusionadas
            IRow row11 = sheet.CreateRow(10);
            IRow row12 = sheet.CreateRow(11);

            // Fusionar celdas desde B11 hasta F11
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10, 10, 1, 5));
            // Fusionar celdas desde B12 hasta F12
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(11, 11, 1, 5));

            // Establecer el contenido en las celdas fusionadas
            ICell mergedCell11 = row11.CreateCell(1);
            mergedCell11.SetCellValue("Atendiendo su amable solicitud con gusto presentamos cotización de las partes y ");
            ICell mergedCell12 = row12.CreateCell(1);
            mergedCell12.SetCellValue("elementos del sistema Modular Ducon SMD, en nuestra línea 3500, tal como sigue:");
            mergedCell11.CellStyle = style;
            mergedCell12.CellStyle = style;

            sheet.CreateRow(12);
            sheet.CreateRow(13);

            // Agregar el contenido en la fila 14
            IRow row14 = sheet.CreateRow(14);

            // En la celda B15
            ICell cellB15 = row14.CreateCell(1);
            cellB15.SetCellValue("DESCRIPCIÓN");

            // En la celda C15
            ICell cellC15 = row14.CreateCell(2);
            cellC15.SetCellValue("Ancho (cms)");

            // En la celda D15
            ICell cellD15 = row14.CreateCell(3);
            cellD15.SetCellValue("CANT");

            // En la celda E15
            ICell cellE15 = row14.CreateCell(4);
            cellE15.SetCellValue("Valor Unidad");

            // En la celda F15
            ICell cellF15 = row14.CreateCell(5);
            cellF15.SetCellValue("Total");

            // En la celda G15
            ICell cellG15 = row14.CreateCell(6);
            cellG15.SetCellValue("Imagen");

            // Crear el estilo de borde más delgado
            ICellStyle borderStyle = sheet.Workbook.CreateCellStyle();
            borderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            borderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            borderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            borderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;

            // Aplicar el estilo de fuente y centrar el contenido a las celdas desde B15 hasta G15
            for (int i = 1; i <= 6; i++)
            {
                ICell cell = row14.GetCell(i);
                if (cell != null)
                {
                    // Crear el estilo de fuente y establecer la fuente
                    IFont fontCenturyGothic = sheet.Workbook.CreateFont();
                    fontCenturyGothic.FontName = "Century Gothic";
                    fontCenturyGothic.FontHeightInPoints = 10;
                    fontCenturyGothic.IsBold = true;

                    // Crear el estilo de celda y establecer la fuente
                    ICellStyle styleCenturyGothic = sheet.Workbook.CreateCellStyle();
                    styleCenturyGothic.SetFont(fontCenturyGothic);
                    styleCenturyGothic.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                    cell.CellStyle = styleCenturyGothic;

                    // Aplicar el estilo de borde a la celda
                    cell.CellStyle.BorderBottom = borderStyle.BorderBottom;
                    cell.CellStyle.BorderLeft = borderStyle.BorderLeft;
                    cell.CellStyle.BorderRight = borderStyle.BorderRight;
                    cell.CellStyle.BorderTop = borderStyle.BorderTop;
                }
            }

            row14.HeightInPoints = sheet.DefaultRowHeightInPoints * 2;

            sheet.AutoSizeColumn(5);

            // Ajustar el ancho de todas las columnas en la hoja
            for (int i = 0; i < sheet.GetRow(0).LastCellNum; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            int anchoMaximo = 60 * 256;
            sheet.SetColumnWidth(1, anchoMaximo);



        }

        // Método para guardar y descargar el archivo de Excel
        private void DownloadExcelFile(IWorkbook workbook)
        {
            string tempFilePath = Path.GetTempFileName() + ".xls";
            using (FileStream tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(tempFileStream);
            }

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment; filename=Datos.xls");
            Response.TransmitFile(tempFilePath);
            Response.End();
        }

        // Método para llenar la hoja de Cotizacion   
        private void FillCotizacionSheet(ISheet cotizacionSheet)
        {
            // Aplicar la lógica aquí
            // Obtener el número de diseño de la etiqueta lblNumDise
            string numeroDiseño = lblNumDise.Text;

            // Crear estilo para el texto en negrita y color gris oscuro
            IWorkbook workbook = cotizacionSheet.Workbook;
            ICellStyle estiloNegritaGris = workbook.CreateCellStyle();
            IFont fuenteNegrita = workbook.CreateFont();
            fuenteNegrita.IsBold = true;
            fuenteNegrita.Color = IndexedColors.Black.Index;
            fuenteNegrita.FontName = "Century Gothic";
            fuenteNegrita.FontHeightInPoints = 11;
            estiloNegritaGris.SetFont(fuenteNegrita);
            estiloNegritaGris.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            estiloNegritaGris.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            estiloNegritaGris.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            // Consulta SQL para obtener las opciones del número de diseño
            string consultaOpciones = $@"SELECT DISTINCT tblPlanoDiseño.Opcion 
                            FROM tblPlano 
                            INNER JOIN tblPlanoDiseño ON tblPlano.Plano = tblPlanoDiseño.Plano 
                            WHERE tblPlanoDiseño.Numero_Diseño = '{numeroDiseño}'";

            // Lista para almacenar las opciones
            List<string> opcionesList = new List<string>();

            // Ejecutar la consulta SQL para obtener las opciones
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaOpciones, conexion))
                {
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        string opcion = reader["Opcion"].ToString();
                        opcionesList.Add(opcion);
                    }
                }
            }

            // Convertir la lista de opciones a un array
            string[] opciones = opcionesList.ToArray();

            // Fila inicial para insertar los resultados
            int currentRow = 15;

            // Recorrer cada opción
            foreach (string opcion in opciones)
            {
                // Realizar la consulta SQL para obtener los datos de la opción actual
                string consultaSQL = $@"SELECT tblPlanoDiseño.*, 
                    tblPlano.RealizadoPor, 
                    tblPlano.Area, 
                    tblPlano.Plano,

                    ISNULL(
                        (
                            SELECT 
                                SUM(tblPlano_Panel.Cantidad * tblPanel.Precio_Venta) 
                            FROM 
                                tblPlano 
                                INNER JOIN tblPlano_Panel ON tblPlano.Plano = tblPlano_Panel.Id_Plano 
                                INNER JOIN tblPanel ON tblPlano_Panel.Id_PanelNum = tblPanel.Id_Numerico 
                                INNER JOIN tblGrupoObjeto ON tblPanel.Id_GrupoObjeto = tblGrupoObjeto.ID_GrupoObjeto 
                            WHERE 
                                tblGrupoObjeto.Cotizar = 1 
                            GROUP BY 
                                tblPlano.Plano 
                            HAVING 
                                tblPlano.Plano = tblPlanoDiseño.Plano
                        ), 
                        0
                    ) AS SubTotalActualZona 
            FROM tblPlano 
            INNER JOIN tblPlanoDiseño ON tblPlano.Plano = tblPlanoDiseño.Plano 
            WHERE tblPlanoDiseño.Numero_Diseño = '{numeroDiseño}' AND Opcion = '{opcion}'
            ORDER BY opcion, tblPlano.Plano ASC";

                // Ejecutar la consulta y obtener los resultados
                DataTable resultados = new DataTable();
                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                    {
                        conexion.Open();
                        SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                        adaptador.Fill(resultados);
                    }
                }

                // Si hay al menos un resultado, agregar el texto de la opción en negrita
                if (resultados.Rows.Count > 0)
                {
                    // Agregar una fila vacía si no es la primera opción
                    if (currentRow > 15)
                    {
                        currentRow++;
                    }

                    // Agregar el texto de la opción en negrita en la columna B
                    IRow opcionRow = cotizacionSheet.CreateRow(currentRow++);
                    ICell opcionCell = opcionRow.CreateCell(1);
                    opcionCell.SetCellValue($"OPCIÓN: {opcion}");
                    IFont fontOpcion = workbook.CreateFont();
                    fontOpcion.FontName = "Century Gothic";
                    fontOpcion.FontHeightInPoints = 11;
                    fontOpcion.IsBold = true;
                    ICellStyle styleOpcion = workbook.CreateCellStyle();
                    styleOpcion.SetFont(fontOpcion);
                    opcionCell.CellStyle = styleOpcion;

                    // Crear un nuevo estilo para el texto con tipo de letra 'Century Gothic' y tamaño de letra 11
                    ICellStyle estiloCentGothic11 = workbook.CreateCellStyle();
                    IFont fuenteCentGothic11 = workbook.CreateFont();
                    fuenteCentGothic11.FontName = "Century Gothic";
                    fuenteCentGothic11.FontHeightInPoints = 11;
                    estiloCentGothic11.SetFont(fuenteCentGothic11);

                   

                    // Agregar los resultados de la consulta a la hoja de Excel
                    foreach (DataRow fila in resultados.Rows)
                    {
                        // Agregar el valor del área y de la composición en la misma fila y columna B
                        IRow zonaRow2 = cotizacionSheet.CreateRow(currentRow++);
                        ICell areaCell = zonaRow2.CreateCell(1);
                        string areaYComposicion = $"{fila["Area"]}\n{fila["Composicion"]}";
                        areaCell.SetCellValue(areaYComposicion);
                        areaCell.CellStyle = estiloCentGothic11;
                        areaCell.CellStyle.WrapText = true;

                        // Agregar el valor de la cantidad en la columna C
                        ICell cantidadCell = zonaRow2.CreateCell(3);
                        cantidadCell.SetCellValue(Convert.ToDouble(fila["Cantidad"]));
                        cantidadCell.CellStyle = estiloCentGothic11;

                        IDataFormat formatoNumerico = workbook.CreateDataFormat();

                        // Crear un nuevo estilo que combine los estilosCentGothic11 y estiloNumerico
                        ICellStyle estiloSubtotal = workbook.CreateCellStyle();
                        estiloSubtotal.CloneStyleFrom(estiloCentGothic11); // Copiar propiedades del estiloCentGothic11
                        estiloSubtotal.DataFormat = formatoNumerico.GetFormat("#,##0"); // Establecer formato numérico

                        // Agregar el valor del subtotal en la columna D con los estilos combinados
                        ICell subtotalCell = zonaRow2.CreateCell(4);
                        double subtotalActualZona = Convert.ToDouble(fila["SubTotalActualZona"]);
                        subtotalCell.SetCellValue(subtotalActualZona);
                        subtotalCell.CellStyle = estiloSubtotal;

                        // Crear un nuevo estilo que combine los estilosCentGothic11 y estiloNumerico para subtotalMultiplicadoCell
                        ICellStyle estiloSubtotalMultiplicado = workbook.CreateCellStyle();
                        estiloSubtotalMultiplicado.CloneStyleFrom(estiloCentGothic11); // Copiar propiedades del estiloCentGothic11
                        estiloSubtotalMultiplicado.DataFormat = formatoNumerico.GetFormat("#,##0"); // Establecer formato numérico     


                        // Crear celda para mostrar la fórmula
                        ICell resultadoMultiplicacionCell = zonaRow2.CreateCell(5);
                        resultadoMultiplicacionCell.CellStyle = estiloSubtotal;
                        resultadoMultiplicacionCell.CellStyle = estiloSubtotalMultiplicado;
                        // Establecer la fórmula en la celda
                        string formula = string.Format("E{0}*D{0}", currentRow);
                        resultadoMultiplicacionCell.SetCellFormula(formula);


                    }


                }

                // Crear una nueva fila para el total y el texto "TOTAL OPCION"
                IRow totalRow = cotizacionSheet.CreateRow(currentRow++);

                double total = ObtenerTotalSubTotalMultiplicadoCotizacion(numeroDiseño, opcion);

                // Crear la celda en la columna 'B' (índice 1) para el texto "TOTAL OPCION"
                ICell totalLabelCell = totalRow.CreateCell(1);
                totalLabelCell.SetCellValue("TOTAL OPCION: " + opcion);

                // Aplicar los estilos al texto "TOTAL OPCION"
                IFont fontTotalLabel = workbook.CreateFont();
                fontTotalLabel.FontName = "Century Gothic";
                fontTotalLabel.FontHeightInPoints = 11;
                fontTotalLabel.IsBold = true;
                ICellStyle styleTotalLabel = workbook.CreateCellStyle();
                styleTotalLabel.SetFont(fontTotalLabel);
                totalLabelCell.CellStyle = styleTotalLabel;

                // Crear la celda en la columna 'F' (índice 5) para el total
                ICell totalCell = totalRow.CreateCell(5);

                // Establecer el valor del total en la celda
                totalCell.SetCellValue(total);

                IDataFormat formatoNumerico1 = workbook.CreateDataFormat();

                // Crear el estilo para el texto del total
                IFont fontTotal = workbook.CreateFont();
                fontTotal.FontName = "Century Gothic";
                fontTotal.FontHeightInPoints = 11;
                fontTotal.IsBold = true;
                ICellStyle styleTotal = workbook.CreateCellStyle();
                styleTotal.SetFont(fontTotal);

                // Crear un nuevo estilo que combine los estilosCentGothic11 y estiloNumerico para el valor total
                ICellStyle estiloTotal = workbook.CreateCellStyle();
                estiloTotal.CloneStyleFrom(styleTotal); // Copiar propiedades del estiloCentGothic11
                estiloTotal.DataFormat = formatoNumerico1.GetFormat("#,##0"); // Establecer formato numérico

                // Combinar los estilos estiloTotal y styleTotal
                estiloTotal.SetFont(fontTotal);

                // Aplicar los estilos al valor del total
                totalCell.CellStyle = estiloTotal;


            }


        }

        private double ObtenerTotalSubTotalMultiplicadoCotizacion(string numeroDiseno, string opcion)
        {
            double total = 0;

            // Realizar la consulta SQL para obtener el total multiplicado de los subtotales
            string consultaSQL = $@"
        SELECT ISNULL(SUM(tblPanel.Precio_Venta * tblPlano_Panel.Cantidad * tblPlanoDiseño.Cantidad), 0) AS TotalSubTotalMultiplicado
        FROM tblDiseño 
        INNER JOIN tblPlanoDiseño ON tblDiseño.Numero_Diseño = tblPlanoDiseño.Numero_Diseño
        INNER JOIN tblPlano_Panel ON tblPlanoDiseño.Plano = tblPlano_Panel.Id_Plano 
        INNER JOIN tblPanel ON tblPlano_Panel.Id_PanelNum = tblPanel.Id_Numerico 
        INNER JOIN tblGrupoObjeto ON tblPanel.Id_GrupoObjeto = tblGrupoObjeto.ID_GrupoObjeto
        WHERE (tblGrupoObjeto.Cotizar = 1) 
        AND (tblDiseño.Numero_Diseño = '{numeroDiseno}')                            
        AND (tblPlanoDiseño.Opcion = '{opcion}')";

            // Ejecutar la consulta y obtener el total
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    conexion.Open();
                    total = Convert.ToDouble(comando.ExecuteScalar());
                }
            }

            return total;
        }

        private void FillCotizacionDetalladaSheet(ISheet cotizacionDetalladaSheet)
        {
            // Obtener el número de diseño de la etiqueta lblNumDise
            string numeroDiseño = lblNumDise.Text;

            // Crear estilo para el texto en negrita y color gris oscuro
            IWorkbook workbook = cotizacionDetalladaSheet.Workbook;
            ICellStyle estiloNegritaGris = workbook.CreateCellStyle();
            IFont fuenteNegrita = workbook.CreateFont();
            fuenteNegrita.IsBold = true;
            fuenteNegrita.Color = IndexedColors.Black.Index;
            fuenteNegrita.FontName = "Century Gothic";
            fuenteNegrita.FontHeightInPoints = 11;
            estiloNegritaGris.SetFont(fuenteNegrita);
            estiloNegritaGris.FillForegroundColor = IndexedColors.Grey40Percent.Index;
            estiloNegritaGris.FillPattern = FillPattern.SolidForeground;
            estiloNegritaGris.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            estiloNegritaGris.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            estiloNegritaGris.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;

            // Consulta SQL para obtener las opciones del número de diseño
            string consultaOpciones = $@"SELECT DISTINCT tblPlanoDiseño.Opcion 
                            FROM tblPlano 
                            INNER JOIN tblPlanoDiseño ON tblPlano.Plano = tblPlanoDiseño.Plano 
                            WHERE tblPlanoDiseño.Numero_Diseño = '{numeroDiseño}'";

            // Lista para almacenar las opciones
            List<string> opcionesList = new List<string>();

            // Ejecutar la consulta SQL para obtener las opciones
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaOpciones, conexion))
                {
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        string opcion = reader["Opcion"].ToString();
                        opcionesList.Add(opcion);
                    }
                }
            }

            // Convertir la lista de opciones a un array
            string[] opciones = opcionesList.ToArray();

         

            // Fila inicial para insertar los resultados
            int currentRow = 15;

            // Recorrer cada opción
            foreach (string opcion in opciones)
            {
                // Realizar la consulta SQL para obtener los datos de la opción actual
                string consultaSQL = $@"SELECT tblPlanoDiseño.*, 
                                tblPlano.RealizadoPor, 
                                tblPlano.Area, 
                                tblPlano.Plano,

                                ISNULL(
                                    (
                                        SELECT 
                                            SUM(tblPlano_Panel.Cantidad * tblPanel.Precio_Venta) 
                                        FROM 
                                            tblPlano 
                                            INNER JOIN tblPlano_Panel ON tblPlano.Plano = tblPlano_Panel.Id_Plano 
                                            INNER JOIN tblPanel ON tblPlano_Panel.Id_PanelNum = tblPanel.Id_Numerico 
                                            INNER JOIN tblGrupoObjeto ON tblPanel.Id_GrupoObjeto = tblGrupoObjeto.ID_GrupoObjeto 
                                        WHERE 
                                            tblGrupoObjeto.Cotizar = 1 
                                        GROUP BY 
                                            tblPlano.Plano 
                                        HAVING 
                                            tblPlano.Plano = tblPlanoDiseño.Plano
                                    ), 
                                    0
                                ) AS SubTotalActualZona 
                        FROM tblPlano 
                        INNER JOIN tblPlanoDiseño ON tblPlano.Plano = tblPlanoDiseño.Plano 
                        WHERE tblPlanoDiseño.Numero_Diseño = '{numeroDiseño}' AND Opcion = '{opcion}'
                        ORDER BY opcion, tblPlano.Plano ASC";

                // Ejecutar la consulta y obtener los resultados
                DataTable resultados = new DataTable();
                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                    {
                        conexion.Open();
                        SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                        adaptador.Fill(resultados);
                    }
                }

                // Si hay al menos un resultado, agregar el texto de la opción en negrita
                if (resultados.Rows.Count > 0)
                {
                    // Agregar una fila vacía si no es la primera opción
                    if (currentRow > 15)
                    {
                        currentRow++;
                    }

                    // Agregar el texto de la opción en negrita en la columna B
                    IRow opcionRow = cotizacionDetalladaSheet.CreateRow(currentRow++);
                    ICell opcionCell = opcionRow.CreateCell(1);
                    opcionCell.SetCellValue($"OPCIÓN: {opcion}");
                    IFont fontOpcion = workbook.CreateFont();
                    fontOpcion.FontName = "Century Gothic";
                    fontOpcion.FontHeightInPoints = 11;
                    fontOpcion.IsBold = true;
                    ICellStyle styleOpcion = workbook.CreateCellStyle();
                    styleOpcion.SetFont(fontOpcion);
                    opcionCell.CellStyle = styleOpcion;

                    // Agregar los resultados de la consulta a la hoja de Excel
                    foreach (DataRow fila in resultados.Rows)
                    {
                        // Agregar el texto 'Zona' en la fila siguiente y en la misma columna B
                        IRow zonaRow = cotizacionDetalladaSheet.CreateRow(currentRow++);
                        ICell zonaCell = zonaRow.CreateCell(1);
                        zonaCell.SetCellValue("Zona");
                        zonaCell.CellStyle = estiloNegritaGris;

                        // Agregar el valor del área en la misma fila y columna B
                        IRow zonaRow2 = cotizacionDetalladaSheet.CreateRow(currentRow++);
                        ICell areaCell = zonaRow2.CreateCell(1);
                        areaCell.SetCellValue(fila["Area"].ToString());

                        // Agregar el texto 'Plano' en la misma fila y columna G
                        ICell planoCell = zonaRow.CreateCell(5);
                        planoCell.SetCellValue("Plano");
                        planoCell.CellStyle = estiloNegritaGris;

                        // Agregar el valor del plano en la misma fila y columna G
                        ICell planoCellValue = zonaRow2.CreateCell(5);
                        planoCellValue.SetCellValue(fila["Plano"].ToString());
      

                        // Aplicar el estilo a todas las celdas desde la columna 'B' hasta la columna 'G'
                        for (int i = 1; i <= 6; i++)
                        {
                            for (int j = currentRow - 2; j <= currentRow; j++)
                            {
                                IRow row = cotizacionDetalladaSheet.GetRow(j);
                                if (row == null)
                                {
                                    row = cotizacionDetalladaSheet.CreateRow(j);
                                }

                                ICell cell = row.GetCell(i);
                                if (cell == null)
                                {
                                    cell = row.CreateCell(i);
                                }

                                cell.CellStyle = estiloNegritaGris;

                                cell.CellStyle.WrapText = true;
                            }
                        }


                        // Agregar una fila vacía
                        opcionRow = cotizacionDetalladaSheet.CreateRow(currentRow++);
                      

                        
                        string plan = fila["Plano"].ToString();

                        // Obtener el valor de 'Cantidad' de tu consulta SQL
                        double cantidadConsultaSQL = Convert.ToDouble(fila["Cantidad"]);

                        // Llamar a AgregarResultadosProcedimientoAlmacenado y pasar el valor de 'Cantidad'
                        AgregarResultadosProcedimientoAlmacenado(cotizacionDetalladaSheet, workbook, ref currentRow, plan, cantidadConsultaSQL, opcion);

                        // Agregar una fila vacía después de los resultados del procedimiento almacenado
                        currentRow++;

                    }
                }

                // Crear una nueva fila para el total y el texto "TOTAL OPCION"
                IRow totalRow = cotizacionDetalladaSheet.CreateRow(currentRow++);

                double total = ObtenerTotalSubTotalMultiplicadoCotizacion(numeroDiseño, opcion);

                // Crear la celda en la columna 'B' (índice 1) para el texto "TOTAL OPCION"
                ICell totalLabelCell = totalRow.CreateCell(1);
                totalLabelCell.SetCellValue("TOTAL OPCION: " + opcion);

                // Aplicar los estilos al texto "TOTAL OPCION"
                IFont fontTotalLabel = workbook.CreateFont();
                fontTotalLabel.FontName = "Century Gothic";
                fontTotalLabel.FontHeightInPoints = 11;
                fontTotalLabel.IsBold = true;
                ICellStyle styleTotalLabel = workbook.CreateCellStyle();
                styleTotalLabel.SetFont(fontTotalLabel);
                totalLabelCell.CellStyle = styleTotalLabel;

                // Crear la celda en la columna 'F' (índice 5) para el total
                ICell totalCell = totalRow.CreateCell(5);

                // Establecer el valor del total en la celda
                totalCell.SetCellValue(total);

                IDataFormat formatoNumerico1 = workbook.CreateDataFormat();

                // Crear el estilo para el texto del total
                IFont fontTotal = workbook.CreateFont();
                fontTotal.FontName = "Century Gothic";
                fontTotal.FontHeightInPoints = 11;
                fontTotal.IsBold = true;
                ICellStyle styleTotal = workbook.CreateCellStyle();
                styleTotal.SetFont(fontTotal);

                // Crear un nuevo estilo que combine los estilosCentGothic11 y estiloNumerico para el valor total
                ICellStyle estiloTotal = workbook.CreateCellStyle();
                estiloTotal.CloneStyleFrom(styleTotal); // Copiar propiedades del estiloCentGothic11
                estiloTotal.DataFormat = formatoNumerico1.GetFormat("#,##0"); // Establecer formato numérico

                // Combinar los estilos estiloTotal y styleTotal
                estiloTotal.SetFont(fontTotal);

                // Aplicar los estilos al valor del total
                totalCell.CellStyle = estiloTotal;

            }

            // Agregar una fila vacía
            cotizacionDetalladaSheet.CreateRow(currentRow++);

            // Agregar el texto 'RESUMEN DEL PROYECTO POR GRUPOS DE OBJETOS' en la columna B
            IRow resumenRow = cotizacionDetalladaSheet.CreateRow(currentRow++);
            ICell resumenCell = resumenRow.CreateCell(1);
            resumenCell.SetCellValue("RESUMEN DEL PROYECTO POR GRUPOS DE OBJETOS");

            // Aplicar estilo al texto 'RESUMEN DEL PROYECTO POR GRUPOS DE OBJETOS'
            IFont fontResumen = workbook.CreateFont();
            fontResumen.FontName = "Century Gothic";
            fontResumen.FontHeightInPoints = 11;
            fontResumen.IsBold = true;
            ICellStyle styleResumen = workbook.CreateCellStyle();
            styleResumen.SetFont(fontResumen);
            resumenCell.CellStyle = styleResumen;



            // Establecer la altura de la fila en 2 centímetros
            resumenRow.HeightInPoints = 2 * cotizacionDetalladaSheet.DefaultRowHeightInPoints;


            string sSql = "SELECT tblPlanoDiseño.opcion, tblPlanoDiseño.Numero_Diseño, tblGrupoObjeto.Descripcion_Grupo, Sum(tblPlano_Panel.Cantidad) AS Cantidad, Sum(tblPlano_Panel.Cantidad*tblPlano_Panel.Precio_Venta) AS SubTotalPlano, Sum(tblPlano_Panel.Cantidad*tblPanel.Precio_Venta) AS SubTotalActual " +
      "FROM (tblPlano INNER JOIN ((tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto) INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum) ON tblPlano.Plano = tblPlano_Panel.Id_Plano) INNER JOIN tblPlanoDiseño ON tblPlano.Plano = tblPlanoDiseño.Plano " +
      "Where (((tblGrupoObjeto.Cotizar) = 1)) " +
      "GROUP BY tblPlanoDiseño.opcion, tblPlanoDiseño.Numero_Diseño, tblGrupoObjeto.Descripcion_Grupo Having (((tblPlanoDiseño.Numero_Diseño) = '" + numeroDiseño + "')) ORDER BY tblPlanoDiseño.opcion, tblGrupoObjeto.Descripcion_Grupo;";
            DataTable resultadosConsulta = new DataTable();
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(sSql, conexion))
                {
                    conexion.Open();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(resultadosConsulta);
                }
            }

            // Obtener la última fila para agregar los resultados
            int ultimaFila = cotizacionDetalladaSheet.LastRowNum + 1;

            // Agregar los resultados al final del contenido de la hoja de Excel "Cotizacion Detallada"
            foreach (DataRow fila in resultadosConsulta.Rows)
            {
                // Obtener los valores de la fila
                string opcion = fila["opcion"].ToString();
                string descripcionGrupo = fila["Descripcion_Grupo"].ToString();
                int cantidad = Convert.ToInt32(fila["Cantidad"]);
                double subTotalActual = Convert.ToDouble(fila["SubTotalActual"]);

                ICellStyle estiloCentGothic11 = workbook.CreateCellStyle();
                IFont fuenteCentGothic11 = workbook.CreateFont();
                fuenteCentGothic11.FontName = "Century Gothic";
                fuenteCentGothic11.FontHeightInPoints = 11;
                ICellStyle estiloCentGothic111 = workbook.CreateCellStyle();
                estiloCentGothic111.SetFont(fuenteCentGothic11);

                // Agregar los valores a la hoja de Excel
                IRow row = cotizacionDetalladaSheet.CreateRow(ultimaFila++);
       
                ICell totalCellDesOpcGru = row.CreateCell(1);
                totalCellDesOpcGru.SetCellValue("OP" + opcion + "-" + descripcionGrupo);
                totalCellDesOpcGru.CellStyle = estiloCentGothic111;

                ICell totalCellCan = row.CreateCell(3);
                totalCellCan.SetCellValue(cantidad);
                totalCellCan.CellStyle = estiloCentGothic111;

                // Crear la celda en la columna 'F' (índice 5) para el total
                ICell totalCell = row.CreateCell(5);        
                totalCell.SetCellValue(subTotalActual);

                IDataFormat formatoNumerico1 = workbook.CreateDataFormat();

                // Crear un nuevo estilo que combine los estilosCentGothic11 y estiloNumerico para el valor total
                ICellStyle estiloTotal = workbook.CreateCellStyle();
                estiloTotal.CloneStyleFrom(estiloCentGothic111); // Copiar propiedades del estiloCentGothic11
                estiloTotal.DataFormat = formatoNumerico1.GetFormat("#,##0"); // Establecer formato numérico

                // Combinar los estilos estiloTotal y styleTotal
                estiloTotal.SetFont(fuenteCentGothic11);

                // Aplicar los estilos al valor del total
                totalCell.CellStyle = estiloTotal;

              


            }

            IRow sumaRow8 = cotizacionDetalladaSheet.CreateRow(ultimaFila++);


         


            double totalCubicajeAcumulado = CalcularTotalCotizacion(cotizacionDetalladaSheet);


            // Obtener la suma de los valores en la columna 9
            double totalPesoKG = 0;
            for (int rowIndex = 16; rowIndex < ultimaFila; rowIndex++)
            {
                IRow currentRow2 = cotizacionDetalladaSheet.GetRow(rowIndex);
                if (currentRow2 != null)
                {
                    ICell cell = currentRow2.GetCell(10); // Columna 9 (índice 8)
                    if (cell != null && cell.CellType == CellType.Numeric)
                    {
                        totalPesoKG += cell.NumericCellValue;
                    }
                }
            }

            ICellStyle estiloCentGothic1144 = workbook.CreateCellStyle();
            IFont fuenteCentGothic114 = workbook.CreateFont();
            fuenteCentGothic114.FontName = "Century Gothic";
            fuenteCentGothic114.FontHeightInPoints = 11;
            ICellStyle estiloCentGothic1114 = workbook.CreateCellStyle();
            estiloCentGothic1114.SetFont(fuenteCentGothic114);


            double totalCubicajeProyecto2 = CalcularTotalCubicajeProyecto(totalCubicajeAcumulado);

          
           
            IRow sumaRow2 = cotizacionDetalladaSheet.CreateRow(ultimaFila++);
            // Obtener el texto para la primera celda
            string textoCelda1 = "TRANSPORTE DE " + totalCubicajeProyecto2.ToString("#.#") + "M3 A " + TextCiuPro.SelectedItem.Text;

            // Crear una nueva celda en la columna 14 (contando desde 0) de la fila 1
            ICell textoCelda12 = sumaRow2.CreateCell(1);
            textoCelda12.CellStyle = estiloCentGothic1114;

            // Establecer el valor de totalCubicajeProyecto en la celda
            textoCelda12.SetCellValue(textoCelda1);

           
            IRow sumaRow3 = cotizacionDetalladaSheet.CreateRow(ultimaFila++);

            // Obtener el texto para la primera celda
            string textoCelda2 = "PESO DEL PROYECTO: " + totalPesoKG.ToString("#.#") + "KG";

            // Crear una nueva celda en la columna 14 (contando desde 0) de la fila 1
            ICell textoCelda123 = sumaRow3.CreateCell(1);
            textoCelda123.CellStyle = estiloCentGothic1114;

            // Establecer el valor de totalCubicajeProyecto en la celda
            textoCelda123.SetCellValue(textoCelda2);


            // Obtener el ID de la ciudad seleccionada en el DropDownList TextCiuPro
            int idCiudad = Convert.ToInt32(TextCiuPro.SelectedValue);
     
            decimal costoTransporteDecimal = CalcularCostoTransporte(idCiudad, (decimal)totalCubicajeProyecto2);

            // Convertir el costo de transporte de decimal a double
            double costoTransporte = (double)costoTransporteDecimal;

            // Crear una nueva celda en la columna 5 (índice 4) de la fila 'sumaRow3'
            ICell costoTransporteCell = sumaRow3.CreateCell(5);

            // Establecer el estilo de la celda
            costoTransporteCell.CellStyle = estiloCentGothic1114;

            // Establecer el valor del costo de transporte en la celda
            costoTransporteCell.SetCellValue((double)costoTransporte);

            // Obtener el formato de número para aplicar a la celda
            IDataFormat formatoNumerico = workbook.CreateDataFormat();
            short formatoNumero = formatoNumerico.GetFormat("#,##0"); // Formato de número con dos decimales y separador de miles

            // Aplicar el formato numérico a la celda
            ICellStyle estiloNumerico = workbook.CreateCellStyle();
            estiloNumerico.CloneStyleFrom(estiloCentGothic1114); // Clonar el estilo existente
            estiloNumerico.DataFormat = formatoNumero;
            costoTransporteCell.CellStyle = estiloNumerico;



        }
       
        public decimal CalcularCostoTransporte(int idCiudad, decimal totalCubicajeProyecto)
            {
                decimal costoTransporteCalculado = 0;

                string sSql = "SELECT * FROM tblCostoTransporte WHERE tte_ID_Ciudad = @idCiudad ORDER BY tte_RangoFinal DESC";

                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sSql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@idCiudad", idCiudad);
                        conexion.Open();
                        SqlDataReader rsCostoTransporte = cmd.ExecuteReader();

                        while (rsCostoTransporte.Read())
                        {
                            short rangoInicial = rsCostoTransporte.GetInt16(rsCostoTransporte.GetOrdinal("tte_RangoInicial"));
                            short rangoFinal = rsCostoTransporte.GetInt16(rsCostoTransporte.GetOrdinal("tte_RangoFinal"));
                            float valor = rsCostoTransporte.GetFloat(rsCostoTransporte.GetOrdinal("tte_Valor"));

                            if (rangoInicial <= totalCubicajeProyecto && totalCubicajeProyecto <= rangoFinal)
                            {
                                costoTransporteCalculado += (decimal)valor;
                            }
                        }

                        rsCostoTransporte.Close();
                    }
                }

                return costoTransporteCalculado;
            }

        private double CalcularTotalCotizacion(ISheet cotizacionDetalladaSheet)
        {
            double total = 0;
            // Itera sobre las filas relevantes en la hoja "Cotizacion"
            for (int rowIndex = 15; rowIndex <= cotizacionDetalladaSheet.LastRowNum; rowIndex++)
            {
                IRow currentRow = cotizacionDetalladaSheet.GetRow(rowIndex);
                if (currentRow != null)
                {
                    // Obtén la celda en la columna específica (por ejemplo, columna 8)
                    ICell cell = currentRow.GetCell(9); // Ajusta el índice de acuerdo a tu estructura
                    if (cell != null && cell.CellType == CellType.Numeric)
                    {
                        // Suma el valor de la celda al total
                        total += cell.NumericCellValue;
                    }
                }
            }
            return total;
        }

        private void AgregarResultadosProcedimientoAlmacenado(ISheet cotizacionDetalladaSheet, IWorkbook workbook, ref int currentRow, string plan, double cantidadConsultaSQL, string opcion)
        {
           
            // Aplicar los estilos requeridos a la celda de Descripcion_Grupo
            ICellStyle estiloNegritaGris = workbook.CreateCellStyle();
            IFont fuenteNegrita = workbook.CreateFont();
            fuenteNegrita.IsBold = true;
            fuenteNegrita.Color = IndexedColors.Black.Index;
            fuenteNegrita.FontName = "Century Gothic";
            fuenteNegrita.FontHeightInPoints = 11;
            estiloNegritaGris.SetFont(fuenteNegrita);
            estiloNegritaGris.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            estiloNegritaGris.FillPattern = FillPattern.SolidForeground;
            estiloNegritaGris.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

         

            // Ejecutar el procedimiento almacenado
            string consultaSQL = "EXEC cta_Plano_PanelesNue @Plan = '" + plan + "'";
            DataTable resultados = new DataTable();
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    conexion.Open();
                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(resultados);
                }
            }

            // Conjunto para almacenar valores únicos de Descripcion_Grupo por Plano
            Dictionary<string, HashSet<string>> gruposMostradosPorPlano = new Dictionary<string, HashSet<string>>();

            // Diccionario para almacenar la suma de sumaValorActual por cada Id_Plano
            Dictionary<string, double> sumaTotalPorPlano = new Dictionary<string, double>();

            double totalCubicajeAcumulado = 0.0;

            int filaInicioResultadosPlano = currentRow;

            foreach (DataRow fila in resultados.Rows)
            {
                string descripcionGrupo = fila["Descripcion_Grupo"].ToString();
                string idGrupoObjeto = fila["ID_GrupoObjeto"].ToString();
                string planoActual = fila["Id_Plano"].ToString();

                // Verificar si el plano ya está en el diccionario
                if (!gruposMostradosPorPlano.ContainsKey(planoActual))
                {
                    // Si el plano no está en el diccionario, agregarlo con un nuevo HashSet
                    gruposMostradosPorPlano.Add(planoActual, new HashSet<string>());
                }

                // Verificar si el Descripcion_Grupo ya se ha mostrado para este Plano
                if (!gruposMostradosPorPlano[planoActual].Contains(descripcionGrupo))
                {
                    // Si el Descripcion_Grupo no se ha mostrado para este Plano, agregarlo a la hoja de Excel

                    string descripcionTecnica = ObtenerDescripcionTecnicaGrupo(idGrupoObjeto);

                    // Concatenar la descripción técnica con la descripción del grupo con un salto de línea
                    string descripcionCompleta = $"{descripcionGrupo}\n{descripcionTecnica}";

                    // Agregar el valor de Descripcion_Grupo en la columna B
                    IRow row = cotizacionDetalladaSheet.CreateRow(currentRow++);
                    ICell descripcionCell = row.CreateCell(1);
                    descripcionCell.SetCellValue(descripcionCompleta);
                    descripcionCell.CellStyle = estiloNegritaGris;

                    // Aplicar los estilos a las celdas de Descripcion_Grupo
                    for (int i = 1; i <= 6; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            cell = row.CreateCell(i);
                        }
                        cell.CellStyle = estiloNegritaGris;
                        // Establecer WrapText en true para que el texto se ajuste automáticamente
                        cell.CellStyle.WrapText = true;
                    }


                    // Restablecer la suma de ValorActual para la Descripcion_Grupo actual
                    double sumaValorActual = 0.0;
                    double totalCubicaje = 0.0;
                   
                 
                 
                    int filaInicioResultados = currentRow;

                    // Resto de tu código...


                    foreach (DataRow filaGrupo in resultados.Rows)
                    {
                        if (filaGrupo["Descripcion_Grupo"].ToString() == descripcionGrupo && filaGrupo["Id_Plano"].ToString() == planoActual)
                        {
                            string idNumerico = filaGrupo["Id_Numerico"].ToString();
                            string descripcionTecnicaPanel = ObtenerDescripcionTecnicaPanel(idNumerico);
                          
                            double ancho = Convert.ToDouble(filaGrupo["Ancho"]);
                            double cantidad = Convert.ToDouble(filaGrupo["Cantidad"]);
                            double valorActual = Convert.ToDouble(filaGrupo["ValorActual"]);

                            ICellStyle estiloCentGothic11 = workbook.CreateCellStyle();
                            IFont fuenteCentGothic11 = workbook.CreateFont();
                            fuenteCentGothic11.FontName = "Century Gothic";
                            fuenteCentGothic11.FontHeightInPoints = 11;
                            estiloCentGothic11.SetFont(fuenteCentGothic11);
                            estiloCentGothic11.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                            estiloCentGothic11.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                            estiloCentGothic11.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                            ICellStyle estiloCentGothic111 = workbook.CreateCellStyle();
                            estiloCentGothic111.SetFont(fuenteCentGothic11);

                            ICellStyle estiloCentGothic112 = workbook.CreateCellStyle();
                            IFont fuenteCentGothic112 = workbook.CreateFont();
                            fuenteCentGothic112.FontName = "Century Gothic";
                            fuenteCentGothic112.FontHeightInPoints = 11;
                            ICellStyle estiloCentGothic1112 = workbook.CreateCellStyle();
                            estiloCentGothic1112.SetFont(fuenteCentGothic112);


                            // 3. Multiplicar los valores de la columna 'Cantidad' obtenidos en el punto 1 con los de la consulta del procedimiento almacenado
                            double cantidadFinal = cantidadConsultaSQL * cantidad;

                           

                            // Crear una nueva fila y agregar los valores
                            row = cotizacionDetalladaSheet.CreateRow(currentRow++);
                            descripcionCell = row.CreateCell(1);
                            descripcionCell.SetCellValue(descripcionTecnicaPanel);
                           
                            descripcionCell.CellStyle = estiloCentGothic11;

                            // Aplicar el estilo para aumentar el alto de la fila
                            row.HeightInPoints = cotizacionDetalladaSheet.DefaultRowHeightInPoints * 6;

                            descripcionCell.CellStyle.WrapText = true;

                            IDataFormat formatoNumerico2 = workbook.CreateDataFormat();

                            ICell anchoCell = row.CreateCell(2);
                            anchoCell.SetCellValue(ancho); // Establecer el valor como número

                            ICellStyle estiloNumero = workbook.CreateCellStyle();
                            estiloNumero.CloneStyleFrom(estiloCentGothic11); // Copiar propiedades del estiloCentGothic11
                            estiloNumero.DataFormat = formatoNumerico2.GetFormat("0.0");


                            // Combinar los estilos estiloTotal y styleTotal
                            estiloNumero.SetFont(fuenteCentGothic11);

                            // Aplicar los estilos al valor del total
                            anchoCell.CellStyle = estiloNumero;


                            ICell cantidadaCell = row.CreateCell(3);
                            cantidadaCell.SetCellValue(cantidadFinal); // Establecer el valor como número
                            cantidadaCell.CellStyle = estiloCentGothic11;

                            ICellStyle estiloSubtotal = workbook.CreateCellStyle();
                            estiloSubtotal.CloneStyleFrom(estiloCentGothic11); // Copiar propiedades del estiloCentGothic11
                            estiloSubtotal.DataFormat = formatoNumerico2.GetFormat("#,##0"); // Establecer formato numérico

                            // Crear celdas para los valores que deseas enviar como números
                            ICell valorActualCell = row.CreateCell(4);
                            valorActualCell.CellStyle = estiloSubtotal;

                            // Crear celda para mostrar la fórmula
                            ICell resultadoMultiplicacionCell = row.CreateCell(5);
                            resultadoMultiplicacionCell.CellStyle = estiloSubtotal;

                            // Establecer la fórmula en la celda
                            string formula = string.Format("E{0}*D{0}", currentRow);
                            resultadoMultiplicacionCell.SetCellFormula(formula);

                            // Establecer los valores como números en las celdas correspondientes
                            valorActualCell.SetCellValue(Convert.ToDouble(valorActual));
                           

                            double cubicaje = CalcularCubicaje(plan, idNumerico, cantidadFinal);

                            totalCubicajeAcumulado += cubicaje;

                            ICell totalCubicajeCell = row.CreateCell(9);
                            totalCubicajeCell.CellStyle = estiloCentGothic1112;

                            totalCubicajeCell.SetCellValue(cubicaje);

                            double peso = CalcularPesoKG(plan, idNumerico, cantidadFinal);

                         
                            ICell totalPesoCell = row.CreateCell(10);
                            totalPesoCell.CellStyle = estiloCentGothic1112;
                            totalPesoCell.SetCellValue(peso);

                           

                        }
                    }

                 
                        IRow filaSuma = cotizacionDetalladaSheet.CreateRow(currentRow++);
                        ICell sumaCell = filaSuma.CreateCell(5);

                        // Establecer la fórmula de suma en la celda
                        // Establecer la fórmula de suma en la celda
                        string formulaSuma = string.Format("SUM(F{0}:F{1})", filaInicioResultados + 1, currentRow - 1);
                        sumaCell.SetCellFormula(formulaSuma);

                        IDataFormat formatoNumerico3 = workbook.CreateDataFormat();

                        // Aplicar estilo de negrita, tamaño de letra 11 y tipo de letra Century Gothic
                        ICellStyle estiloSuma = workbook.CreateCellStyle();
                        IFont fontSuma = workbook.CreateFont();
                        fontSuma.FontName = "Century Gothic";
                        fontSuma.FontHeightInPoints = 11;
                        fontSuma.IsBold = true;
                        estiloSuma.SetFont(fontSuma);
                        sumaCell.CellStyle = estiloSuma;

                        // Agregar 'Sub Total' en la columna anterior y en la misma fila
                        ICell subTotalCell = filaSuma.CreateCell(4);
                        subTotalCell.SetCellValue("Sub Total");
                        subTotalCell.CellStyle = estiloSuma;

                        // Crear un nuevo estilo que combine los estilosCentGothic11 y estiloNumerico para el valor total
                        ICellStyle estiloTotal = workbook.CreateCellStyle();
                        estiloTotal.CloneStyleFrom(estiloSuma); // Copiar propiedades del estiloCentGothic11
                        estiloTotal.DataFormat = formatoNumerico3.GetFormat("#,##0"); // Establecer formato numérico

                        // Combinar los estilos estiloTotal y styleTotal
                        estiloTotal.SetFont(fontSuma);

                        // Aplicar los estilos al valor del total
                        sumaCell.CellStyle = estiloTotal;
                    


              
                        sumaTotalPorPlano[planoActual] = 0.0;
                    
                    sumaTotalPorPlano[planoActual] += sumaValorActual;

                    // Agregar el Descripcion_Grupo al conjunto de Descripcion_Grupo mostrados para este Plano
                    gruposMostradosPorPlano[planoActual].Add(descripcionGrupo);

                    // Agregar la fila vacía
                    cotizacionDetalladaSheet.CreateRow(currentRow++);
                }
            }

            // Crear un nuevo estilo con negrita y color gris oscuro para el texto
            IFont fontTotalZona = workbook.CreateFont();
            fontTotalZona.FontName = "Century Gothic";
            fontTotalZona.FontHeightInPoints = 11;
            fontTotalZona.IsBold = true;
            fontTotalZona.Color = IndexedColors.Black.Index;

            // Crear un relleno con color gris oscuro
            ICellStyle styleTotalZona = workbook.CreateCellStyle();
            styleTotalZona.SetFont(fontTotalZona);
            styleTotalZona.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            styleTotalZona.FillPattern = FillPattern.SolidForeground;

            foreach (var kvp in sumaTotalPorPlano)
            {
                // Obtener el número de diseño de la etiqueta lblNumDise
                string numeroDiseño = lblNumDise.Text;

                IRow filaSumaTotalPorPlano = cotizacionDetalladaSheet.CreateRow(currentRow++);
                string idPlano = ""; // Variable para almacenar el ID del plano actual

                foreach (DataRow fila in resultados.Rows)
                {
                    idPlano = fila["Id_Plano"].ToString(); // Obtener el ID del plano
                    string area = ObtenerAreaPorIdPlano(idPlano); // Obtener el área

                    // Concatenar el valor de 'Area' con el texto 'Total Zona'
                    string textoTotalZona = $"Total Zona - {area}";
                    ICell totalCell = filaSumaTotalPorPlano.CreateCell(1);
                    totalCell.SetCellValue(textoTotalZona);

                    // Aplicar el estilo al texto 'Total Zona - {area}' en la columna B
                    for (int i = 1; i <= 6; i++)
                    {
                        ICell cell = filaSumaTotalPorPlano.GetCell(i);
                        if (cell == null)
                        {
                            cell = filaSumaTotalPorPlano.CreateCell(i);
                        }
                        cell.CellStyle = styleTotalZona;

                        cell.CellStyle.WrapText = true;
                    }
                }

                IDataFormat formatoNumerico4 = workbook.CreateDataFormat();

                IRow filaSuma = filaSumaTotalPorPlano;
                ICell sumaCell = filaSuma.CreateCell(5);
  
                // Establecer la fórmula de suma en la celda
                string formulaSuma = string.Format("SUM(F{0}:F{1})/2", filaInicioResultadosPlano + 1, currentRow - 1);
                sumaCell.SetCellFormula(formulaSuma);

                // Aplicar estilo a la celda de sumaTotalPorPlanoCell
                ICellStyle styleSubtotal = workbook.CreateCellStyle();
                styleSubtotal.SetFont(fontTotalZona);
                styleSubtotal.FillForegroundColor = IndexedColors.Grey25Percent.Index;
                styleSubtotal.FillPattern = FillPattern.SolidForeground;
                sumaCell.CellStyle = styleSubtotal;

                // Crear un nuevo estilo que combine los estilosCentGothic11 y estiloNumerico para el valor total
                ICellStyle estiloTotal = workbook.CreateCellStyle();
                estiloTotal.CloneStyleFrom(styleSubtotal); // Copiar propiedades del estiloCentGothic11
                estiloTotal.DataFormat = formatoNumerico4.GetFormat("#,##0"); // Establecer formato numérico

                // Combinar los estilos estiloTotal y styleTotal
                estiloTotal.SetFont(fontTotalZona);

                // Aplicar los estilos al valor del total
                sumaCell.CellStyle = estiloTotal;

            }

        

        }

        // Método para obtener la ruta de la imagen desde la base de datos
        private string ObtenerRutaImagenDesdeBaseDeDatos()
        {
            string rutaImagen = ""; // Variable para almacenar la ruta de la imagen

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string consultaSQL = "SELECT mail FROM tblUsosVarios WHERE ObjetivoMail = 'PathImagendeBloques'";

            // Crear y abrir la conexión a la base de datos
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                // Crear el comando SQL
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    // Abrir la conexión
                    conexion.Open();

                    // Ejecutar el comando y leer el resultado
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        // Verificar si se encontró algún resultado
                        if (reader.Read())
                        {
                            // Obtener la ruta de la imagen del primer resultado
                            rutaImagen = reader["mail"].ToString();
                        }
                    }
                }
            }

            // Devolver la ruta de la imagen
            return rutaImagen;
        }


        // Método para calcular el cubicaje
        private double CalcularCubicaje(string plan, string idNumerico, double cantidadFinal)
        {
            double cubicaje = 0.0;

            // Definir la cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Consulta SQL para obtener los datos necesarios para calcular el cubicaje
            string consultaSQL = @"
    SELECT MAX(tblPanel.Id_Numerico) AS Id_Numerico, tblGrupoObjeto.Descripcion_Grupo, tblGrupoObjeto.GODescripcionTecnica, (CASE WHEN tblPanel.Descripcion_Tecnica IS NULL OR tblPanel.Descripcion_Tecnica ='' THEN tblPanel.Descripcion_Panel ELSE tblPanel.Descripcion_Tecnica END) AS Descripcion, tblPanel.Ancho, tblPanel.Altura, tblPanel.Profundidad,  tblPanel.Precio_Venta AS PrecioObjetoActual,tblPanel.PesoKG, tblPlano_Panel.Id_Plano, SUM(tblPlano_Panel.Cantidad) AS Cantidad, tblPlano.Area, MAX(tblPlano_Panel.Precio_Venta) AS PrecioVentaPlano, tblGrupoObjeto.AjusteCubicaje,  MAX(tblPanel.Id_Panel) AS Id_Panel
    FROM tblPlano INNER JOIN 
    tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto INNER JOIN 
    tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum ON tblPlano.Plano = tblPlano_Panel.Id_Plano 
    WHERE (tblGrupoObjeto.Cotizar = 1 AND Id_Numerico = @IdNumerico) 
    GROUP BY tblGrupoObjeto.Descripcion_Grupo, tblGrupoObjeto.GODescripcionTecnica, tblPanel.Ancho, tblPanel.Altura, tblPanel.Profundidad, tblPanel.Precio_Venta,tblPanel.PesoKG, tblPlano_Panel.Id_Plano, tblPlano.Area, tblGrupoObjeto.AjusteCubicaje, (CASE WHEN tblPanel.Descripcion_Tecnica IS NULL OR tblPanel.Descripcion_Tecnica ='' THEN tblPanel.Descripcion_Panel ELSE tblPanel.Descripcion_Tecnica END) 
    HAVING (tblPlano_Panel.Id_Plano = @Plan) ORDER BY tblGrupoObjeto.Descripcion_Grupo asc,(CASE WHEN tblPanel.Descripcion_Tecnica IS NULL OR tblPanel.Descripcion_Tecnica =''  THEN tblPanel.Descripcion_Panel ELSE tblPanel.Descripcion_Tecnica END) asc, tblPanel.Ancho";

            // Crear una nueva conexión a la base de datos
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                // Crear un comando SQL con la consulta y la conexión
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    // Agregar parámetros a la consulta
                    comando.Parameters.AddWithValue("@IdNumerico", idNumerico);
                    comando.Parameters.AddWithValue("@Plan", plan);

                    // Abrir la conexión
                    conexion.Open();

                    // Ejecutar la consulta y obtener los resultados
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        // Verificar si hay filas en el resultado
                        if (reader.Read())
                        {
                            // Obtener los valores necesarios para el cálculo
                            double ancho = Convert.ToDouble(reader["Ancho"]);
                            double altura = Convert.ToDouble(reader["Altura"]);
                            double profundidad = Convert.ToDouble(reader["Profundidad"]);
                            double ajusteCubicaje = Convert.ToDouble(reader["AjusteCubicaje"]);
                            double cantidadZona = Convert.ToDouble(reader["Cantidad"]);

                            // Calcular el cubicaje según la fórmula dada
                            cubicaje = cantidadFinal * (ancho * altura * profundidad * ajusteCubicaje / 1000000);
                            cubicaje = Math.Round(cubicaje, 5);
                        }
                    }
                }
            }

            // Devolver el valor de cubicaje
            return cubicaje;
        }

        private double ObtenerPorcentajeAdicionalCubicaje()
        {
            double porcentajeAdicional = 0.0;

            // Definir la cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Consulta SQL para obtener el valor de mail
            string consultaSQL = "SELECT mail FROM tblUsosVarios WHERE ObjetivoMail = 'PorcentajeAdicionalCubicaje'";

            // Crear una nueva conexión a la base de datos
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                // Crear un comando SQL con la consulta y la conexión
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    // Abrir la conexión
                    conexion.Open();

                    // Ejecutar la consulta y obtener el resultado
                    object resultado = comando.ExecuteScalar();

                    // Verificar si se obtuvo un resultado no nulo
                    if (resultado != null)
                    {
                        // Convertir el resultado a double
                        porcentajeAdicional = Convert.ToDouble(resultado);
                    }
                }
            }

            return porcentajeAdicional;
        }

        private double CalcularTotalCubicajeProyecto(double totalCubicajeAcumulado)
        {
            double porcentajeAdicional = ObtenerPorcentajeAdicionalCubicaje();

            // Calcular el total de cubicaje del proyecto
            double totalCubicajeProyecto = totalCubicajeAcumulado + (totalCubicajeAcumulado * porcentajeAdicional / 100.0);

            return totalCubicajeProyecto;
        }

        private double CalcularPesoKG(string plan, string idNumerico, double cantidadFinal)
        {
            double pesoTotal = 0.0;

            // Definir la cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Consulta SQL para obtener los datos necesarios para calcular el cubicaje
            string consultaSQL = @"
    SELECT MAX(tblPanel.Id_Numerico) AS Id_Numerico, tblGrupoObjeto.Descripcion_Grupo, tblGrupoObjeto.GODescripcionTecnica, (CASE WHEN tblPanel.Descripcion_Tecnica IS NULL OR tblPanel.Descripcion_Tecnica ='' THEN tblPanel.Descripcion_Panel ELSE tblPanel.Descripcion_Tecnica END) AS Descripcion, tblPanel.Ancho, tblPanel.Altura, tblPanel.Profundidad,  tblPanel.Precio_Venta AS PrecioObjetoActual,tblPanel.PesoKG, tblPlano_Panel.Id_Plano, SUM(tblPlano_Panel.Cantidad) AS Cantidad, tblPlano.Area, MAX(tblPlano_Panel.Precio_Venta) AS PrecioVentaPlano, tblGrupoObjeto.AjusteCubicaje,  MAX(tblPanel.Id_Panel) AS Id_Panel
         FROM tblPlano INNER JOIN 
         tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto INNER JOIN 
         tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum ON tblPlano.Plano = tblPlano_Panel.Id_Plano 
         Where (tblGrupoObjeto.Cotizar = 1 AND Id_Numerico = @IdNumerico) 
         GROUP BY tblGrupoObjeto.Descripcion_Grupo, tblGrupoObjeto.GODescripcionTecnica, tblPanel.Ancho, tblPanel.Altura, tblPanel.Profundidad, tblPanel.Precio_Venta,tblPanel.PesoKG, tblPlano_Panel.Id_Plano, tblPlano.Area, tblGrupoObjeto.AjusteCubicaje, (CASE WHEN tblPanel.Descripcion_Tecnica IS NULL OR tblPanel.Descripcion_Tecnica ='' THEN tblPanel.Descripcion_Panel ELSE tblPanel.Descripcion_Tecnica END) 
         HAVING (tblPlano_Panel.Id_Plano = @Plan) ORDER BY tblGrupoObjeto.Descripcion_Grupo asc,(CASE WHEN tblPanel.Descripcion_Tecnica IS NULL OR tblPanel.Descripcion_Tecnica =''  THEN tblPanel.Descripcion_Panel ELSE tblPanel.Descripcion_Tecnica END) asc, tblPanel.Ancho

";

            // Crear una nueva conexión a la base de datos
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                // Crear un comando SQL con la consulta y la conexión
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    // Agregar parámetros a la consulta
                    comando.Parameters.AddWithValue("@IdNumerico", idNumerico);
                    comando.Parameters.AddWithValue("@Plan", plan);

                    // Abrir la conexión
                    conexion.Open();

                    // Ejecutar la consulta y obtener los resultados
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        // Verificar si hay filas en el resultado
                        if (reader.Read())
                        {
                            // Obtener los valores necesarios para el cálculo
                            double pesoKG = Convert.ToDouble(reader["PesoKG"]);

                            // Calcular el cubicaje según la fórmula dada
                            pesoTotal = cantidadFinal * pesoKG;

                            pesoTotal = Math.Round(pesoTotal, 5);
                        }
                    }
                }
            }

            return pesoTotal;
        }

        private string ObtenerDescripcionTecnicaGrupo(string idGrupoObjeto)
        {
            string descripcionTecnica = string.Empty;

            // Realizar la consulta SQL para obtener la descripción técnica del grupo objeto
            string consultaSQL = $"SELECT GODescripcionTecnica FROM tblGrupoObjeto WHERE ID_GrupoObjeto = '{idGrupoObjeto}'";

            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    conexion.Open();
                    descripcionTecnica = comando.ExecuteScalar()?.ToString();
                }
            }

            return descripcionTecnica;
        }

        // Método para ejecutar la consulta SQL y devolver un solo valor
        // Declara un diccionario para almacenar los totales calculados
        private Dictionary<string, Dictionary<string, double>> totalesCalculados = new Dictionary<string, Dictionary<string, double>>();

        public double ObtenerTotalSubTotalMultiplicado(string numeroDiseño, string opcionParaCalcularTotal)
        {
            // Verifica si ya se ha calculado el total para la combinación específica
            if (totalesCalculados.ContainsKey(numeroDiseño) && totalesCalculados[numeroDiseño].ContainsKey(opcionParaCalcularTotal))
            {
                return totalesCalculados[numeroDiseño][opcionParaCalcularTotal];
            }

            // Si no se ha calculado previamente, realiza la consulta SQL
            double totalSubTotalMultiplicado = 0;

            // Consulta SQL
            string consultaSQL = @"
        SELECT ISNULL(SUM(tblPanel.Precio_Venta * tblPlano_Panel.Cantidad * tblPlanoDiseño.Cantidad), 0) AS TotalSubTotalMultiplicado
        FROM tblDiseño 
        INNER JOIN tblPlanoDiseño ON tblDiseño.Numero_Diseño = tblPlanoDiseño.Numero_Diseño
        INNER JOIN tblPlano_Panel ON tblPlanoDiseño.Plano = tblPlano_Panel.Id_Plano 
        INNER JOIN tblPanel ON tblPlano_Panel.Id_PanelNum = tblPanel.Id_Numerico 
        INNER JOIN tblGrupoObjeto ON tblPanel.Id_GrupoObjeto = tblGrupoObjeto.ID_GrupoObjeto
        WHERE (tblGrupoObjeto.Cotizar = 1) 
        AND (tblDiseño.Numero_Diseño = @NumeroDiseño)                            
        AND (tblPlanoDiseño.Opcion = @OpcionParaCalcularTotal)";

            // Establecer la conexión y ejecutar la consulta
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);
                    comando.Parameters.AddWithValue("@OpcionParaCalcularTotal", opcionParaCalcularTotal);

                    try
                    {
                        conexion.Open();
                        object resultado = comando.ExecuteScalar();
                        if (resultado != DBNull.Value)
                        {
                            totalSubTotalMultiplicado = Convert.ToDouble(resultado);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar excepciones
                    }
                }
            }

            // Almacena el total calculado en el diccionario
            if (!totalesCalculados.ContainsKey(numeroDiseño))
            {
                totalesCalculados[numeroDiseño] = new Dictionary<string, double>();
            }
            totalesCalculados[numeroDiseño][opcionParaCalcularTotal] = totalSubTotalMultiplicado;

            return totalSubTotalMultiplicado;
        }

        private string ObtenerAreaPorIdPlano(string idPlano)
        {
            string area = "";
            string consultaArea = $"SELECT Area FROM tblPlano WHERE Plano = '{idPlano}'";

            using (SqlConnection conexionArea = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comandoArea = new SqlCommand(consultaArea, conexionArea))
                {
                    conexionArea.Open();
                    area = comandoArea.ExecuteScalar()?.ToString();
                }
            }

            return area;
        }

        private string ObtenerDescripcionTecnicaPanel(string idNumerico)
        {
            string descripcionTecnica = string.Empty;

            // Construir la consulta SQL
            string consultaSQL = $"SELECT * FROM tblPanel WHERE Id_Numerico = '{idNumerico}'";

            // Ejecutar la consulta y obtener la descripción técnica del panel
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Verificar si la descripcionTecnica es nula
                            descripcionTecnica = reader["Descripcion_Tecnica"] as string;
                            if (string.IsNullOrEmpty(descripcionTecnica))
                            {
                                // Si es nula, devolver el valor de la columna Id_Panel
                                descripcionTecnica = reader["Descripcion_Panel"] as string;
                            }
                            if (string.IsNullOrEmpty(descripcionTecnica))
                            {
                                // Si es nula, devolver el valor de la columna Id_Panel
                                descripcionTecnica = reader["Id_Panel"] as string;
                            }
                        }
                    }
                }
            }

            return descripcionTecnica;
        }

        private string ObtenerIdPanel(string idNumerico)
        {
            string idPanel = string.Empty;

            // Construir la consulta SQL
            string consultaSQL = $"SELECT * FROM tblPanel WHERE Id_Numerico = '{idNumerico}'";

            // Ejecutar la consulta y obtener la descripción técnica del panel
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand(consultaSQL, conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {             
                                // Si es nula, devolver el valor de la columna Id_Panel
                                idPanel = reader["Id_Panel"] as string;
                            
                        }
                    }
                }
            }

            return idPanel;
        }

        protected void RegresarDise_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#RegresarDise').modal('show');", true);
        }

        protected void UpdateRegresarDise_Click(object sender, EventArgs e)
        {
            string tipoAccion = Session["Diseno"] as string;
            if (tipoAccion == "Ventas")
            {

            }
            else if (tipoAccion == "Recepcion")
            {
                string diseño = lblNumDise.Text;

                // Actualizar la tabla tblDiseño estableciendo CotizaciónOK en 1
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string query = "Update tblDiseño set TerminadoDibujo = 0 WHERE Numero_Diseño = @Numero_Diseño";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Numero_Diseño", diseño);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Session["NumeroDiseño2"] = diseño;


                            string mensajePersonalizado = "Diseño regresado exitosamente!";
                            string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                            // Si se actualizaron filas, se deshabilita el botón y se cambia su clase CSS
                            BtnProgramar.Enabled = false;
                            BtnProgramar.CssClass = "button-disabled form-control fw-bold";
                        }
                        else
                        {

                        }

                    }
                }
            }        
        }

     


    }


}