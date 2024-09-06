using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Windows.Forms.MonthCalendar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using OfficeOpenXml;
using System.IO;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Globalization;
using System.Windows.Media.Animation;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Ventas
{
    public partial class IngresarCotizacion : System.Web.UI.Page
    {
        private List<TextBox> listaTextBoxes;
        private List<DropDownList> listaDropDownLists;
        private string CadenaConexionSID = "BD_SIDSQL";
        private string CadenaConexionISID = "BD_ISIDSQL";
        private string CadenaConexionSSF = "BD_SSF";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                if (!IsPostBack)
                {
                   

                    ddlCompeData.DataBind();

                    ddlCausa.DataBind();

                    CargarAsesoresEnDropDownList();
                    DisposicionDeBotonesPg();

                    listaTextBoxes = new List<TextBox>
                         {
                   textCotizacion, TextBox1, TextBox2, TextContacto, TextTelefono, TextMail, TextFcot, TextFrta, TextPlano, TextObs, TextProyecto,
                   TextBox3, TextBox4, TextBox5, TextBox11, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10

                        };
                    DeshabilitarTextBoxes(listaTextBoxes);
                    listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,ddlAsesor,DropDownListEstado,ddlCompeData,ddlCausa

                };
                    DeshabilitarDropDownLists(listaDropDownLists);
                    CargarDropDownListEstado();

                    TextFcot.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextFrta.Text = DateTime.Now.ToString("yyyy-MM-dd");
                                      
                    CargarClienteYContacto();

                    Session.Remove("Cotizacion");

                    if (Session["Cotizacion2"] != null && !string.IsNullOrEmpty(Session["Cotizacion2"].ToString()))
                    {
                        llenarCampos();
                    }
                    else 
                    { 
                        
                    }
                        Session.Remove("Cotizacion2");
                   
                }
            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
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
                        string query = "SELECT X.NombreCompañía, X.Dirección, Y.NombreContacto, Y.MailContacto, Y.Telefono,  X.Asesor, X.Id_Cliente " +
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
                                        TextBox1.Text = reader["NombreCompañía"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Telefono")))
                                    {
                                        TextTelefono.Text = reader["Telefono"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreContacto")))
                                    {
                                        TextContacto.Text = reader["NombreContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("MailContacto")))
                                    {
                                        TextMail.Text = reader["MailContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Asesor")))
                                    {
                                        ddlAsesor.SelectedValue = reader["Asesor"].ToString();
                                    }                      
                                }
                            }
                        }
                    }

                    CargarAsesorYzona();
                    DropDownListEstado.SelectedValue = "1";             

                    habilitarTextBoxes();

                    Session["Id_ClienteBD2"] = IdCLiente;

                    Session.Remove("Id_ClienteBD");
                    Session.Remove("ID_ContactoBD");

                }

            }
        }

        protected void CargarDropDownListEstado()
        {
            // Define la cadena de conexión a tu base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Define la consulta SQL
            string query = "SELECT Id_Estado, Descripción_Estado FROM tblEstado_Cotización ORDER BY Descripción_Estado ASC";

            // Crea una conexión a la base de datos y un comando SQL
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Abre la conexión y ejecuta el comando
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Limpia cualquier elemento anterior en el DropDownList
                DropDownListEstado.Items.Clear();

                // Itera a través de los resultados y agrega elementos al DropDownList
                while (reader.Read())
                {
                    ListItem item = new ListItem(reader["Descripción_Estado"].ToString(), reader["Id_Estado"].ToString());
                    DropDownListEstado.Items.Add(item);
                }

                // Cierra la conexión y el lector
                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            DropDownListEstado.Items.Insert(0, new ListItem("", ""));
        }

        public void DeshabilitarDropDownLists(List<DropDownList> dropDownLists)
        {
            foreach (DropDownList dropDownList in dropDownLists)
            {
                dropDownList.Enabled = false;
                dropDownList.CssClass = "form-control form-control-sm button-disabled linkButtonClicked";
            }
        }

        public void DeshabilitarTextBoxes(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox == textCotizacion || textBox == TextBox1 || textBox == TextBox2 || textBox == TextContacto || textBox == TextTelefono || textBox == TextMail || 
                   textBox == TextFcot || textBox == TextFrta || textBox == TextPlano || textBox == TextObs || textBox == TextProyecto || textBox == TextBox3 || textBox == TextBox4 || textBox == TextBox5 || textBox == TextBox11 || textBox == TextBox6
                   || textBox == TextBox7 || textBox == TextBox8 || textBox == TextBox9 || textBox == TextBox10)

                {
                    textBox.Enabled = false;
                    textBox.CssClass = "form-control form-control-sm button-disabled linkButtonClicked";
                }


            }

            BtnCliente.Enabled = false;
            BtnCliente.CssClass = "btn btn-sm button-disabled shadow-sm linkButtonClicked";

            BtnDiseno.Enabled = false;
            BtnDiseno.CssClass = "btn shadow btn-light button-disabled linkButtonClicked";
        }

        protected void habilitarTextBoxes()
        {
            ddlAsesor.Enabled = true;
            ddlAsesor.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            textCotizacion.Enabled = true;
            textCotizacion.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            BtnCliente.Enabled = true;
            BtnCliente.CssClass = "btn btn-sm button-enabled shadow-sm linkButtonClicked2";

            TextBox2.Enabled = true;
            TextBox2.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextContacto.Enabled = true;
            TextContacto.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextTelefono.Enabled = true;
            TextTelefono.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextMail.Enabled = true;
            TextMail.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextFcot.Enabled = true;
            TextFcot.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextFrta.Enabled = true;
            TextFrta.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextPlano.Enabled = true;
            TextPlano.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextObs.Enabled = true;
            TextObs.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

            TextProyecto.Enabled = true;
            TextProyecto.CssClass = "form-control button-enabled shadow-sm linkButtonClicked2";

        }

        protected void DisposicionDeBotonesPg()
        {
            NuevaCot.Enabled = true;
            NuevaCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2 AzulClaro";

            GuardarCot.Enabled = false;
            GuardarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

            ModificarCot.Enabled = false;
            ModificarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

            EliminarCot.Enabled = false;
            EliminarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

            CancelarCot.Enabled = true;
            CancelarCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2 RojoCancelar";

        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT *, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial WHERE Activo = 1 order by NombreCompleto ASC";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto";
                ddlAsesor.DataValueField = "Cedula";



                ddlAsesor.DataBind();

                reader.Close();

                reader = command.ExecuteReader();

                DropDownListAsesor.DataSource = reader;
                DropDownListAsesor.DataTextField = "NombreCompleto";
                DropDownListAsesor.DataValueField = "Cedula";
                DropDownListAsesor.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem("", ""));
            DropDownListAsesor.Items.Insert(0, new ListItem("", ""));
        }

        protected void ddlAsesor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarAsesorYzona();
        }

        protected void CargarAsesorYzona()
        {
            // Obtener la cedula seleccionada
            string cedulaSeleccionada = ddlAsesor.SelectedValue;

            // Consulta para obtener la zona correspondiente a la cedula seleccionada
            string consulta = "SELECT Zona FROM tblAsesorComercial WHERE Activo = 1 AND Cedula = @Cedula";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(consulta, connection);
                command.Parameters.AddWithValue("@Cedula", cedulaSeleccionada);

                connection.Open();

                // Ejecutar la consulta
                object zona = command.ExecuteScalar();

                // Asignar la zona al DropDownList de Zona
                if (zona != null && zona.ToString() != "01" && zona.ToString() != "02")
                {
                    string mensajePersonalizado = "por favor seleccione un asesor valido, ya que la zona del Asesor seleccionado es diferente a 01 o 02 o es null";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                    ddlZona.SelectedValue = zona.ToString();
                    ddlZona.Enabled = false;
                    ddlZona.CssClass = "form-control";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#llenarClienteP').modal('show');", true);
                }


            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            string consulta = "SELECT TOP 400" +
                                "tblCotización.*, " +
                                "tblEstado_Cotización.Descripción_Estado AS Estado, " +
                                "CONCAT(tblAsesorComercial.Nombre, ' ', tblAsesorComercial.Apellidos) AS Asesor, " +
                                "tblCompetencia.NombreCompetencia, " +
                                "tblCausadeCotizacionRechazada.CausaRechazoCotizacion, " +
                                "tblCliente.NombreCompañía AS Cliente2 " +
                            "FROM " +
                                "tblAsesorComercial " +
                            "INNER JOIN " +
                                "(tblEstado_Cotización " +
                                "INNER JOIN " +
                                "(tblCompetencia " +
                                "INNER JOIN " +
                                "(tblCliente " +
                                "INNER JOIN " +
                                "(tblCausadeCotizacionRechazada " +
                                "INNER JOIN " +
                                "tblCotización ON tblCausadeCotizacionRechazada.ID_CausaRechazoCotizacion = tblCotización.ID_CausaRechazoCotizacion) " +
                                "ON tblCliente.Id_Cliente = tblCotización.Cliente) " +
                                "ON tblCompetencia.ID_Competencia = tblCotización.ID_Competencia) " +
                                "ON tblEstado_Cotización.Id_Estado = tblCotización.Estado) " +
                            "ON tblAsesorComercial.Cedula = tblCotización.Asesor ";

            string whereClause = "";

            // Verificar si DropDownListAsesor tiene un valor seleccionado
            if (!string.IsNullOrEmpty(DropDownListAsesor.SelectedValue))
            {
                // Construir la cláusula WHERE adicional
                if (string.IsNullOrEmpty(TextBox12.Text) && string.IsNullOrEmpty(TextBox14.Text))
                {
                    whereClause += " WHERE tblAsesorComercial.Cedula LIKE '%" + DropDownListAsesor.SelectedValue + "%'";
                }
                else
                {
                    whereClause += " AND tblAsesorComercial.Cedula LIKE '%" + DropDownListAsesor.SelectedValue + "%'";
                }
            }

            // Agregar lógica existente para TextBox12 y TextBox14
            if (!string.IsNullOrEmpty(TextBox12.Text))
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += " WHERE tblCotización.Cotización LIKE '%" + TextBox12.Text + "%'";
                }
                else
                {
                    whereClause += " AND tblCotización.Cotización LIKE '%" + TextBox12.Text + "%'";
                }
            }

            if (!string.IsNullOrEmpty(TextBox14.Text))
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += " WHERE tblCotización.Plano LIKE '%" + TextBox14.Text + "%'";
                }
                else
                {
                    whereClause += " AND tblCotización.Plano LIKE '%" + TextBox14.Text + "%'";
                }
            }
            if (!string.IsNullOrEmpty(TextBox15.Text))
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += " WHERE tblCotización.Cliente LIKE '%" + TextBox15.Text + "%'";
                }
                else
                {
                    whereClause += " AND tblCotización.Cliente LIKE '%" + TextBox15.Text + "%'";
                }
            }

            consulta += whereClause;

            // Asigna la consulta al control SqlDataSource1
            SqlDataSource1.SelectCommand = consulta;

            // Vincula el DataGrid al SqlDataSource y actualiza su contenido
            DataGrid1.DataSourceID = "SqlDataSource1";
            DataGrid1.DataBind();

            listaTextBoxes = new List<TextBox>
                         {
                   textCotizacion, TextBox1, TextBox2, TextContacto, TextTelefono, TextMail, TextFcot, TextFrta, TextPlano, TextObs, TextProyecto,
                   TextBox3, TextBox4, TextBox5, TextBox11, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10

                        };
            DeshabilitarTextBoxes(listaTextBoxes);
            listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,ddlAsesor,DropDownListEstado,ddlCompeData,ddlCausa

                };
            DeshabilitarDropDownLists(listaDropDownLists);
        }

        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid1.Items[rowIndex];


            string Cotizacion = selectedRow.Cells[1].Text;
            string idOT = selectedRow.Cells[14].Text;
            string id_Cliente = selectedRow.Cells[4].Text;

            Session["Id_ClienteBD2"] = id_Cliente;
            Session["Cotizacion"] = Cotizacion;
            // Asigna los valores a variables de sesión
            Session["Id_OTCot"] = idOT;


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


            llenarCampos();

            if (!string.IsNullOrEmpty(Session["Cotizacion"] as string) && !string.IsNullOrEmpty(Session["Id_OTCot"] as string))
            {
                llenarDatagrid2();
            }
            else
            {
                // Limpia los datos del DataGrid2
                DataGrid2.DataSource = null;
                DataGrid2.DataBind();
            }

            listaTextBoxes = new List<TextBox>
                         {
                   textCotizacion, TextBox1, TextBox2, TextContacto, TextTelefono, TextMail, TextFcot, TextFrta, TextPlano, TextObs, TextProyecto,
                   TextBox3, TextBox4, TextBox5, TextBox11, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10

                        };
            DeshabilitarTextBoxes(listaTextBoxes);

            listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,ddlAsesor,DropDownListEstado,ddlCompeData,ddlCausa

                };
            DeshabilitarDropDownLists(listaDropDownLists);

            ModificarCot.Enabled = true;
            ModificarCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2";
            EliminarCot.Enabled = true;
            EliminarCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2";
            ModificarCot.Enabled = true;
            ModificarCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2 AzulGris";
            NuevaCot.Enabled = true;
            NuevaCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2 AzulClaro";
       
        }

        protected void llenarCampos()
        {
            // Verificar si existe la variable de sesión
            if (Session["Cotizacion"] != null)
            {
                // Obtener el valor de la variable de sesión
                string cotizacion = Session["Cotizacion"].ToString();

                // Construir la conexión y el comando SQL
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string query = @"SELECT 
    c.*, 
    ec.Descripción_Estado AS Estado, 
    CONCAT(ac.Nombre, ' ', ac.Apellidos) AS Asesor, 
    cmp.NombreCompetencia, 
    ccc.CausaRechazoCotizacion, 
    cli.NombreCompañía AS Cliente3 
FROM 
    tblCotización c
    INNER JOIN tblEstado_Cotización ec ON ec.Id_Estado = c.Estado
    INNER JOIN tblCompetencia cmp ON cmp.ID_Competencia = c.ID_Competencia
    INNER JOIN tblCliente cli ON cli.Id_Cliente = c.Cliente
    INNER JOIN tblCausadeCotizacionRechazada ccc ON ccc.ID_CausaRechazoCotizacion = c.ID_CausaRechazoCotizacion
    INNER JOIN tblAsesorComercial ac ON ac.Cedula = c.Asesor
WHERE 
    c.Cotización = @Cotizacion
";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar el parámetro
                        command.Parameters.AddWithValue("@Cotizacion", cotizacion);

                        // Abrir la conexión y ejecutar el comando
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Verificar si hay filas devueltas
                        if (reader.Read())
                        {
                            ddlZona.SelectedValue = reader["Zona"].ToString();
                            string asesor = reader["Asesor"].ToString();

                            // Verificar si el valor del asesor existe en el DropDownList
                            ListItem item = ddlAsesor.Items.FindByValue(asesor);
                            if (item != null)
                            {
                                // Si el valor existe, seleccionarlo
                                ddlAsesor.SelectedValue = item.Value;
                            }
                            else
                            {
                                // Si el valor no existe, establecer el DropDownList en blanco
                                ddlAsesor.SelectedValue = "";
                            }
                            textCotizacion.Text = reader["Cotización"].ToString();
                            TextBox1.Text = reader["Cliente3"].ToString();
                            TextBox2.Text = reader["Diseño"].ToString();
                            TextContacto.Text = reader["Contacto_Cotizacion"].ToString();
                            TextTelefono.Text = reader["Teléfono"].ToString();
                            TextMail.Text = reader["Correo_Electronico"].ToString();
                            ddlCompeData.SelectedItem.Text = reader["NombreCompetencia"].ToString();
                            ddlCausa.SelectedItem.Text = reader["CausaRechazoCotizacion"].ToString();

                            // Convertir y formatear las fechas
                            DateTime fechaCotizacion;
                            DateTime fechaRespuesta;
                            if (DateTime.TryParse(reader["Fecha_Cotización"].ToString(), out fechaCotizacion))
                            {
                                TextFcot.Text = fechaCotizacion.ToString("yyyy-MM-dd");
                            }
                            if (DateTime.TryParse(reader["Fecha_Respuesta"].ToString(), out fechaRespuesta))
                            {
                                TextFrta.Text = fechaRespuesta.ToString("yyyy-MM-dd");
                            }


                            TextPlano.Text = reader["Plano"].ToString();
                            TextObs.Text = reader["Observación"].ToString();
                            TextProyecto.Text = reader["Obra"].ToString();
                            DropDownListEstado.SelectedValue = reader["Estado"].ToString();

                            TextBox3.Text = double.Parse(reader["ValorSugerido"].ToString()).ToString("#,##0");
                            TextBox4.Text = double.Parse(reader["Valor"].ToString()).ToString("#,##0");
                            TextBox5.Text = double.Parse(reader["DescuentoComision"].ToString()).ToString("#,##0");
                            TextBox11.Text = double.Parse(reader["Descuento"].ToString()).ToString("#,##0");
                            TextBox6.Text = reader["ValorMO"] != DBNull.Value ? double.Parse(reader["ValorMO"].ToString()).ToString("#,##0") : "0";
                            TextBox7.Text = double.Parse(reader["VCCD"].ToString()).ToString("#,##0");
                            TextBox8.Text = double.Parse(reader["ValorTteVia"].ToString()).ToString("#,##0");
                            TextBox9.Text = double.Parse(reader["ValorViatico"].ToString()).ToString("#,##0");


                            double valorTextBoxVCCD = double.Parse(TextBox7.Text);
                            double valorTextBoxVTTE = double.Parse(TextBox8.Text);
                            double valorTextBoxVIA = double.Parse(TextBox9.Text);
                            double valorTextBoxTOTAL = valorTextBoxVCCD + valorTextBoxVTTE + valorTextBoxVIA;
                            TextBox10.Text = valorTextBoxTOTAL.ToString("N0");

                        }

                        // Cerrar el lector y la conexión
                        reader.Close();
                        connection.Close();

                        
                    }
                }
            }
            else if (Session["Cotizacion2"] != null)
            {
                // Obtener el valor de la variable de sesión
                string cotizacion = Session["Cotizacion2"].ToString();

                // Construir la conexión y el comando SQL
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string query = @"SELECT 
    c.*, 
    ec.Descripción_Estado AS Estado, 
    CONCAT(ac.Nombre, ' ', ac.Apellidos) AS Asesor, 
    cmp.NombreCompetencia, 
    ccc.CausaRechazoCotizacion, 
    cli.NombreCompañía AS Cliente3 
FROM 
    tblCotización c
    INNER JOIN tblEstado_Cotización ec ON ec.Id_Estado = c.Estado
    INNER JOIN tblCompetencia cmp ON cmp.ID_Competencia = c.ID_Competencia
    INNER JOIN tblCliente cli ON cli.Id_Cliente = c.Cliente
    INNER JOIN tblCausadeCotizacionRechazada ccc ON ccc.ID_CausaRechazoCotizacion = c.ID_CausaRechazoCotizacion
    INNER JOIN tblAsesorComercial ac ON ac.Cedula = c.Asesor
WHERE 
    c.Cotización = @Cotizacion
";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Agregar el parámetro
                        command.Parameters.AddWithValue("@Cotizacion", cotizacion);

                        // Abrir la conexión y ejecutar el comando
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Verificar si hay filas devueltas
                        if (reader.Read())
                        {
                            ddlZona.SelectedValue = reader["Zona"].ToString();
                            string asesor = reader["Asesor"].ToString();

                            // Verificar si el valor del asesor existe en el DropDownList
                            ListItem item = ddlAsesor.Items.FindByValue(asesor);
                            if (item != null)
                            {
                                // Si el valor existe, seleccionarlo
                                ddlAsesor.SelectedValue = item.Value;
                            }
                            else
                            {
                                // Si el valor no existe, establecer el DropDownList en blanco
                                ddlAsesor.SelectedValue = "";
                            }
                            textCotizacion.Text = reader["Cotización"].ToString();
                            TextBox1.Text = reader["Cliente3"].ToString();
                            TextBox2.Text = reader["Diseño"].ToString();
                            TextContacto.Text = reader["Contacto_Cotizacion"].ToString();
                            TextTelefono.Text = reader["Teléfono"].ToString();
                            TextMail.Text = reader["Correo_Electronico"].ToString();
                            ddlCompeData.SelectedItem.Text = reader["NombreCompetencia"].ToString();
                            ddlCausa.SelectedItem.Text = reader["CausaRechazoCotizacion"].ToString();

                            // Convertir y formatear las fechas
                            DateTime fechaCotizacion;
                            DateTime fechaRespuesta;
                            if (DateTime.TryParse(reader["Fecha_Cotización"].ToString(), out fechaCotizacion))
                            {
                                TextFcot.Text = fechaCotizacion.ToString("yyyy-MM-dd");
                            }
                            if (DateTime.TryParse(reader["Fecha_Respuesta"].ToString(), out fechaRespuesta))
                            {
                                TextFrta.Text = fechaRespuesta.ToString("yyyy-MM-dd");
                            }


                            TextPlano.Text = reader["Plano"].ToString();
                            TextObs.Text = reader["Observación"].ToString();
                            TextProyecto.Text = reader["Obra"].ToString();
                            DropDownListEstado.SelectedValue = reader["Estado"].ToString();

                            TextBox3.Text = double.Parse(reader["ValorSugerido"].ToString()).ToString("#,##0");
                            TextBox4.Text = double.Parse(reader["Valor"].ToString()).ToString("#,##0");
                            TextBox5.Text = double.Parse(reader["DescuentoComision"].ToString()).ToString("#,##0");
                            TextBox11.Text = double.Parse(reader["Descuento"].ToString()).ToString("#,##0");
                            TextBox6.Text = double.Parse(reader["ValorMO"].ToString()).ToString("#,##0");
                            TextBox7.Text = double.Parse(reader["VCCD"].ToString()).ToString("#,##0");
                            TextBox8.Text = double.Parse(reader["ValorTteVia"].ToString()).ToString("#,##0");
                            TextBox9.Text = double.Parse(reader["ValorViatico"].ToString()).ToString("#,##0");

                            double valorTextBoxVCCD = double.Parse(TextBox7.Text);
                            double valorTextBoxVTTE = double.Parse(TextBox8.Text);
                            double valorTextBoxVIA = double.Parse(TextBox9.Text);
                            double valorTextBoxTOTAL = valorTextBoxVCCD + valorTextBoxVTTE + valorTextBoxVIA;
                            TextBox10.Text = valorTextBoxTOTAL.ToString("N0");

                        }

                        // Cerrar el lector y la conexión
                        reader.Close();
                        connection.Close();


                    }
                }
            }

        }

        protected void llenarDatagrid2()
        {
            // Obtener los valores de las variables de sesión
            string idOT = Session["Id_OTCot"] as string;
            string cotizacion = Session["Cotizacion"] as string;

            // Definir la conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Definir la consulta SQL
                string query = @"SELECT tblOT.Id_OT, 
                                tblOT.Consecutivo_Pedido, 
                                tblOT.Cotizacion, 
                                tblOT.Precio_Venta, 
                                tblOT.Descuento, 
                                tblOT.DescuentoparaComision, 
                                tblOT.ValorBolsa 
                         FROM tblOT 
                         WHERE tblOT.Id_OT = @Id_OT 
                               AND tblOT.Cotizacion = @Cotizacion";

                // Abrir la conexión y crear el comando SQL
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                // Asignar los valores de los parámetros
                command.Parameters.AddWithValue("@Id_OT", idOT);
                command.Parameters.AddWithValue("@Cotizacion", cotizacion);

                // Ejecutar el comando y obtener los resultados
                SqlDataReader reader = command.ExecuteReader();

                // Enlazar los resultados al DataGrid
                DataGrid2.DataSource = reader;
                DataGrid2.DataBind();

                // Cerrar la conexión y el lector
                reader.Close();
                connection.Close();
            }
        }

        protected void NuevaCot_Clik(object sender, EventArgs e)
        {
          
            Session["TipoAccion"] = "Insertar";

            DropDownListEstado.SelectedValue = "1";

            habilitarTextBoxes();
            LimpiarTextBoxes();
        }

        protected void LimpiarTextBoxes()
        {
            ddlAsesor.SelectedValue = "";
            ddlZona.SelectedValue = "";
            DropDownListEstado.SelectedValue = "1";
            textCotizacion.Text = string.Empty;
            TextBox1.Text = string.Empty;
            TextBox2.Text = string.Empty;
            TextContacto.Text = string.Empty;
            TextTelefono.Text = string.Empty;
            TextMail.Text = string.Empty;      
            TextPlano.Text = string.Empty;
            TextObs.Text = string.Empty;
            TextProyecto.Text = string.Empty;
            TextBox3.Text = string.Empty;
            TextBox4.Text = string.Empty;
            TextBox11.Text = string.Empty;
            TextBox5.Text = string.Empty;
            TextBox6.Text = string.Empty;
            TextBox7.Text = string.Empty;
            TextBox8.Text = string.Empty;
            TextBox9.Text = string.Empty;
            TextBox10.Text = string.Empty;
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            listaTextBoxes = new List<TextBox>
                         {
                   textCotizacion, TextBox1, TextBox2, TextContacto, TextTelefono, TextMail, TextFcot, TextFrta, TextPlano, TextObs, TextProyecto,
                   TextBox3, TextBox4, TextBox5, TextBox11, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10

                        };
            DeshabilitarTextBoxes(listaTextBoxes);
            listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,ddlAsesor,DropDownListEstado,ddlCompeData,ddlCausa

                };
            DeshabilitarDropDownLists(listaDropDownLists);

            GuardarCot.Enabled = false;
            GuardarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";
            ModificarCot.Enabled = false;
            ModificarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";
            EliminarCot.Enabled = false;
            EliminarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

            NuevaCot.Enabled = true;
            NuevaCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2 AzulClaro";


        }

        protected void BtnCliente_Click(object sender, EventArgs e)
        {
            // Verificar si el DropDownList ddlAsesor tiene contenido seleccionado
            if (!string.IsNullOrEmpty(ddlAsesor.SelectedValue))
            {
                Session["Cotizacion2"] = textCotizacion.Text.Trim();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + "/Formularios/Ventas/Clientes.aspx" + "', '_blank');", true);
            }
            else
            {
                // Mostrar el modal para indicar al usuario que debe llenar el DropDownList ddlAsesor
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#llenarCliente').modal('show');", true);
            }
        }

        protected void txtCotizacion_TextChanged(object sender, EventArgs e)
        {
            // Realiza la validación de campos
            string campoFaltante = ValidarAsesor();

            if (string.IsNullOrEmpty(campoFaltante))
            {
                // Obtener el valor del DropDownList
                string zonaSeleccionada = ddlZona.SelectedValue;

                // Obtener el valor del TextBox
                string nombreArchivo = textCotizacion.Text;

                // Construir la nueva ruta del archivo de Excel
                string rutaBase = @"\\172.16.30.6\Recepcion\Cotizaciones Excel\";

                // Buscar recursivamente el archivo en la zona seleccionada
                string filePath = BuscarArchivoEnZona(rutaBase, zonaSeleccionada, nombreArchivo + ".xls");


                // Textos a buscar
                string[] textosABuscar = { "vccd", "vvsu", "vcsd", "vmo", "vtte", "viat", "vcsd=vccd" };

                // Inicializar variables para almacenar los valores de las columnas 'F'
                string valorColumnaF_vccd = string.Empty;
                string valorColumnaF_vvsu = string.Empty;
                string valorColumnaF_vcsd = string.Empty;
                string valorColumnaF_vmo = string.Empty;
                string valorColumnaF_vtte = string.Empty;
                string valorColumnaF_viat = string.Empty;
                string valorColumnaF_vcsd_vccd = string.Empty;

                IWorkbook workbook = null;

                if (File.Exists(filePath))
                {
                    // Leer el contenido del archivo de Excel con NPOI
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        // Determinar el tipo de archivo Excel
                        if (Path.GetExtension(filePath).Equals(".xls"))
                            {
                            workbook = new HSSFWorkbook(fs); // Para archivos .xls (Excel 97-2003)
                        }
                        else if (Path.GetExtension(filePath).Equals(".xlsx"))
                        {
                            workbook = new XSSFWorkbook(fs); // Para archivos .xlsx (Excel 2007 y posteriores)
                        }

                        // Buscar y obtener valores para 'vccd', 'vvsu', 'vcsd', 'vmo'
                        BuscarYObtenerValores(workbook, textosABuscar, out valorColumnaF_vccd, out valorColumnaF_vvsu, out valorColumnaF_vcsd, out valorColumnaF_vmo, out valorColumnaF_vtte, out valorColumnaF_viat, out valorColumnaF_vcsd_vccd);
                    }
                

                // Asignar los valores obtenidos a los TextBox
                TextBox7.Text = SumarValoresSiNecesario(workbook, textosABuscar[0], valorColumnaF_vccd) ?? "0";
                    TextBox3.Text = SumarValoresSiNecesario(workbook, textosABuscar[1], valorColumnaF_vvsu) ?? "0";
                    TextBox4.Text = SumarValoresSiNecesario(workbook, textosABuscar[2], valorColumnaF_vcsd) ?? "0";
                    TextBox6.Text = SumarValoresSiNecesario(workbook, textosABuscar[3], valorColumnaF_vmo) ?? "0";
                    TextBox8.Text = SumarValoresSiNecesario(workbook, textosABuscar[4], valorColumnaF_vtte) ?? "0";
                    TextBox9.Text = SumarValoresSiNecesario(workbook, textosABuscar[5], valorColumnaF_viat) ?? "0";

                    string valorColumnaF_vcsd_vccd2 = SumarValoresSiNecesario(workbook, textosABuscar[6], valorColumnaF_vcsd_vccd);

                    // Verificar si se encontró el texto 'vcsd=vccd'
                    if (!string.IsNullOrEmpty(valorColumnaF_vcsd_vccd))
                    {
                        // Asignar el valor de la columna F a TextBox4 y TextBox7
                        TextBox4.Text = valorColumnaF_vcsd_vccd2;
                        TextBox7.Text = valorColumnaF_vcsd_vccd2;
                    }
                    else
                    {

                    }
                    // Intentamos convertir los textos de los TextBox a números double
                    double valorTextBox7, valorTextBox8, valorTextBox9, valorTextBox10;
                    bool isParsed7 = double.TryParse(TextBox7.Text, out valorTextBox7);
                    bool isParsed8 = double.TryParse(TextBox8.Text, out valorTextBox8);
                    bool isParsed9 = double.TryParse(TextBox9.Text, out valorTextBox9);

                    // Formateamos los valores si la conversión fue exitosa, de lo contrario, asignamos "0"
                    TextBox7.Text = isParsed7 ? valorTextBox7.ToString("#,##0.##") : "0";
                    TextBox8.Text = isParsed8 ? valorTextBox8.ToString("#,##0.##") : "0";
                    TextBox9.Text = isParsed9 ? valorTextBox9.ToString("#,##0.##") : "0";

                    // Calculamos la suma de los valores
                    valorTextBox10 = valorTextBox7 + valorTextBox8 + valorTextBox9;

                    // Asignamos el resultado formateado al TextBox correspondiente
                    TextBox10.Text = valorTextBox10.ToString("#,##0.##");

                    TextBox10.Text = valorTextBox10.ToString("N0");

                    TextBox5.Text = "0";
                    TextBox11.Text = "0";


                    double valorSugerido = double.Parse(TextBox3.Text);
                    double valorClienteConDescuento = double.Parse(TextBox7.Text);
                    double valorClienteSinDescuento = double.Parse(TextBox4.Text);

                    if (valorSugerido != 0)
                    {
                        if (valorSugerido > valorClienteConDescuento)
                        {
                            double descuentoComision = ((valorSugerido - valorClienteConDescuento) / valorSugerido) * 100;
                            TextBox5.Text = descuentoComision.ToString("#0");

                            double descuentoFactura = ((valorClienteSinDescuento - valorClienteConDescuento) / valorClienteSinDescuento) * 100;
                            TextBox11.Text = descuentoFactura.ToString("#.#0");
                        }

                    }

                    GuardarCot.Enabled = true;
                    GuardarCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2 AzulGuardarHab";
                }
                else
                {
                    GuardarCot.Enabled = false;
                    GuardarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

                    TextBox7.Text = "0";
                    TextBox3.Text = "0";
                    TextBox4.Text = "0";
                    TextBox6.Text = "0";
                    TextBox8.Text = "0";
                    TextBox9.Text = "0";
                    TextBox10.Text = "0";
                    TextBox11.Text = "0";
                    TextBox5.Text = "0";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ErrorMCotizacion').modal('show');", true);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#llenarCliente').modal('show');", true);
            }
        }

        private string BuscarArchivoEnZona(string rutaBase, string zonaSeleccionada, string nombreArchivo)
        {
            // Construir la ruta de la zona seleccionada
            string rutaZona = Path.Combine(rutaBase, zonaSeleccionada);

            // Verificar si la carpeta de la zona existe
            if (Directory.Exists(rutaZona))
            {
                // Buscar el archivo en la zona seleccionada y sus subcarpetas
                string[] archivos = Directory.GetFiles(rutaZona, nombreArchivo, SearchOption.AllDirectories);

                // Verificar si se encontró el archivo
                if (archivos.Length > 0)
                {
                    // Obtener la ruta completa del archivo encontrado
                    string rutaCompletaArchivo = archivos[0];

                    // Obtener la ruta relativa del archivo encontrado
                    string rutaRelativaArchivo = ObtenerRutaRelativa(rutaBase, rutaZona, rutaCompletaArchivo);

                    // Obtener el año y el mes actual
                    int añoActual = DateTime.Now.Year;
                    string mesActual = DateTime.Now.ToString("MMM");

                    // Obtener el año y el mes de la ruta relativa
                    int añoRuta = int.Parse(rutaRelativaArchivo.Split('\\')[0]);
                    string nombreMesRuta = rutaRelativaArchivo.Split('\\')[1];

                    // Obtener el número de mes a partir del nombre del mes
                    int numeroMesRuta = ObtenerNumeroMes(nombreMesRuta);

                    // Verificar si se pudo obtener el número de mes
                    if (numeroMesRuta != -1)
                    {
                        // Calcular la fecha actual menos 6 meses
                        DateTime fechaLimite = DateTime.Now.AddMonths(-7);

                        // Crear la fecha de la ruta relativa
                        DateTime fechaRuta = new DateTime(añoRuta, numeroMesRuta, 1);

                        // Comparar la fecha de la ruta relativa con la fecha límite
                        if (fechaRuta < fechaLimite)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ErrorMCotizacion').modal('show');", true);

                            return string.Empty;
                        }
                        else
                        {
                            // Si la fecha de la ruta relativa es mayor a 6 meses antes de la fecha actual, continuar con el proceso
                            // y devolver la ruta completa del archivo
                            return rutaCompletaArchivo;
                        }
                    }
                    else
                        {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ErrorMCotizacion').modal('show');", true);
                        return string.Empty;
                    }

                }
            }

            // Si no se encontró el archivo, retornar una cadena vacía
            return string.Empty;
        }

        private int ObtenerNumeroMes(string nombreMes)
        {
            // Diccionario para mapear nombres de mes a números de mes
            Dictionary<string, int> meses = new Dictionary<string, int>
    {
        {"Ene", 1}, {"Feb", 2}, {"Mar", 3}, {"Abr", 4}, {"May", 5}, {"Jun", 6},
        {"Jul", 7}, {"Ago", 8}, {"Sept", 9}, {"Oct", 10}, {"Nov", 11}, {"Dic", 12}
    };

            // Intentar obtener el número de mes del diccionario
            if (meses.ContainsKey(nombreMes))
            {
                return meses[nombreMes];
            }
            else
            {
                // Si el nombre del mes no está en el diccionario, devuelve -1 o lanza una excepción según sea necesario
                // Aquí estoy devolviendo -1, pero puedes modificar esto según tus necesidades
                return -1;
            }
        }

        private string ObtenerRutaRelativa(string rutaBase, string rutaZona, string rutaCompletaArchivo)
        {
            // Obtener la longitud de la ruta de la zona
            int longitudRutaZona = rutaZona.Length;

            // Obtener la posición de la ruta de la zona en la ruta completa del archivo
            int indiceRutaZona = rutaCompletaArchivo.IndexOf(rutaZona);

            // Verificar si se encontró la ruta de la zona en la ruta completa del archivo
            if (indiceRutaZona != -1)
            {
                // Obtener la posición del nombre del archivo
                int indiceNombreArchivo = rutaCompletaArchivo.LastIndexOf(Path.GetFileName(rutaCompletaArchivo));

                // Verificar si se encontró el nombre del archivo
                if (indiceNombreArchivo != -1)
                {
                    // Obtener la parte de la ruta entre la ruta de la zona y el nombre del archivo
                    string rutaRelativa = rutaCompletaArchivo.Substring(indiceRutaZona + longitudRutaZona + 1, indiceNombreArchivo - indiceRutaZona - longitudRutaZona - 1);

                    // Retornar la ruta relativa
                    return rutaRelativa;
                }
            }

            // Si no se puede obtener la ruta relativa, retornar una cadena vacía
            return string.Empty;
        }

        // Método para buscar y obtener valores de las columnas 'F' para cada texto
        private void BuscarYObtenerValores(IWorkbook workbook, string[] textosABuscar, out string valorColumnaF_vccd, out string valorColumnaF_vvsu, out string valorColumnaF_vcsd, out string valorColumnaF_vmo, out string valorColumnaF_vtte, out string valorColumnaF_viat, out string valorColumnaF_vcsd_vccd)
        {
            valorColumnaF_vccd = BuscarTextoEnHoja(workbook, "Cotizacion", textosABuscar[0]);
            valorColumnaF_vvsu = BuscarTextoEnHoja(workbook, "ducon", textosABuscar[1]);
            valorColumnaF_vcsd = BuscarTextoEnHoja(workbook, "Cotizacion", textosABuscar[2]);
            valorColumnaF_vmo = BuscarTextoEnHoja(workbook, "ducon", textosABuscar[3]);
            valorColumnaF_vtte = BuscarTextoEnHoja(workbook, "Cotizacion", textosABuscar[4]);
            valorColumnaF_viat = BuscarTextoEnHoja(workbook, "Cotizacion", textosABuscar[5]);
            valorColumnaF_vcsd_vccd = BuscarTextoEnHoja(workbook, "Cotizacion", textosABuscar[6]);
        }

        // Método para buscar el texto en una hoja específica y obtener los valores de la columna 'F'
        private string BuscarTextoEnHoja(IWorkbook workbook, string sheetName, string textoABuscar)
        {
            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet != null)
            {
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        // Obtener la celda en la columna 'A' (índice 0)
                        ICell cellColumnaA = row.GetCell(0);
                        if (cellColumnaA != null)
                        {
                            // Verificar si el textoABuscar está presente en la celda de la columna 'A'
                            if (cellColumnaA.ToString().IndexOf(textoABuscar, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                ICell cellColumnaF = row.GetCell(5); // Columna 'F' (índice 5)
                                if (cellColumnaF != null)
                                {
                                    // Verificar si la celda contiene una fórmula
                                    if (cellColumnaF.CellType == CellType.Formula)
                                    {
                                        // Obtener el valor numérico resultante de la fórmula
                                        double valorNumerico = cellColumnaF.NumericCellValue;
                                        // Devolver el valor numérico como una cadena formateada
                                        return valorNumerico.ToString();
                                    }
                                    else
                                    {
                                        // Si la celda no contiene una fórmula, devolver el valor numérico original
                                        return cellColumnaF.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        // Método para sumar valores si se encuentra el texto más de una vez
        private string SumarValoresSiNecesario(IWorkbook workbook, string textoABuscar, string valorColumnaF)
        {
            if (!string.IsNullOrEmpty(valorColumnaF))
            {
                List<string> valoresColumnaF = BuscarTextoYObtenerColumnaFEnHoja(workbook, "Cotizacion", textoABuscar);
                if (valoresColumnaF.Count > 1)
                {
                    double sumaValores = 0;
                    foreach (string valor in valoresColumnaF)
                    {
                        double numero;
                        if (double.TryParse(valor, out numero))
                        {
                            sumaValores += numero;
                        }
                    }
                    return sumaValores.ToString();
                }
                else
                {
                    return valorColumnaF;
                }
            }
            else
            {
                return "0"; // Si no se encuentra el texto, asignar cero
            }
        }

        // Método para buscar el texto en una hoja específica y obtener los valores de la columna 'F'
        private List<string> BuscarTextoYObtenerColumnaFEnHoja(IWorkbook workbook, string sheetName, string textoABuscar)
        {
            List<string> valoresColumnaF = new List<string>();
            ISheet sheet = workbook.GetSheet(sheetName);
            if (sheet != null)
            {
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        foreach (ICell cell in row.Cells)
                        {
                            if (cell.ToString().Contains(textoABuscar))
                            {
                                ICell cellColumnaF = row.GetCell(5);
                                if (cellColumnaF != null)
                                {
                                    // Verificar si la celda contiene una fórmula
                                    if (cellColumnaF.CellType == CellType.Formula)
                                    {
                                        // Obtener el valor numérico resultante de la fórmula
                                        double valorNumerico = cellColumnaF.NumericCellValue;
                                        // Agregar el valor numérico a la lista
                                        valoresColumnaF.Add(valorNumerico.ToString());
                                    }
                                    else
                                    {
                                        // Si la celda no contiene una fórmula, agregar el valor numérico original a la lista
                                        valoresColumnaF.Add(cellColumnaF.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return valoresColumnaF;
        }

        protected void Modificar_Click(object sender, EventArgs e)
        {
            if (DropDownListEstado.SelectedValue == "2")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#CotizacionAprobada').modal('show');", true);
            }
            else
            {
                Session["TipoAccion"] = "Actualizar";

                ModificarCot.Enabled = false;
                ModificarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

                GuardarCot.Enabled = true;
                GuardarCot.CssClass = "btn btn-sm button-enabled shadow linkButtonClicked2 AzulGuardarHab";

                EliminarCot.Enabled = false;
                EliminarCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

                NuevaCot.Enabled = false;
                NuevaCot.CssClass = "btn btn-sm button-disabled shadow linkButtonClicked";

                habilitarTextBoxes();

                DropDownListEstado.Enabled = true;
                DropDownListEstado.CssClass = "form-control linkButtonClicked2";

                Session["Cotizacion"] = textCotizacion.Text;

            }
        }

        protected void Grabar_Click(object sender, EventArgs e)
        {
            // Realizar la consulta
            string codigoAsesorSeleccionado = ddlAsesor.SelectedValue;
            string zonaSeleccionada = ddlZona.SelectedValue;
            bool validacionCorrecta = false;

            // Realizar la consulta SQL para verificar si el código de asesor y la zona coinciden
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT CodigoAsesor, Zona FROM tblAsesorComercial WHERE Activo = '1'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string codigoAsesorBD = reader["CodigoAsesor"].ToString();
                        string zonaBD = reader["Zona"].ToString();

                        if (codigoAsesorBD == codigoAsesorSeleccionado && zonaBD == zonaSeleccionada)
                        {
                            validacionCorrecta = true;
                            break;
                        }
                    }
                }
            }

            // Si la validación es correcta, continuar con la acción según el tipo de acción
            if (validacionCorrecta)
            {
                string campoFaltante = ValidarCampos();

                if (string.IsNullOrEmpty(campoFaltante))
                {
                    string tipoAccion = Session["TipoAccion"] as string;
                    if (tipoAccion == "Insertar")
                    {
                        Insercion();
                    }
                    else if (tipoAccion == "Actualizar")
                    {
                        ValidarClienteAsesor();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#campoFaltante').modal('show'); $('#campoFaltante2').text('" + campoFaltante + "');", true);
                }
            }
            else
            {
               
            }
        }

        protected void ValidarClienteAsesor()
        {

            // Obtener el ID del cliente
            string idCliente = ObtenerIdCliente();

            string sSql;
            string asesor = ddlAsesor.SelectedValue;

            // Consulta para obtener el asesor del cliente
            sSql = "SELECT Asesor FROM tblCliente WHERE Id_Cliente = @IdCliente";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sSql, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string asesorCliente = reader["Asesor"].ToString();

                            // Comparar el asesor del cliente con el seleccionado en el DropDownList
                            if (asesorCliente != asesor)
                            {
                                // Mostrar modal de confirmación
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ConfimacionActualizarCliente').modal('show');", true);
                            }
                            else
                            {
                                // Si el cliente y el asesor coinciden, procede con la actualización normal
                                Actualizacion();
                            }
                        }
                    }
                }
            }
        }

        protected void ActualizarCliente_Click(object sender, EventArgs e)
        {
            // Obtener el ID del cliente
            string idCliente = ObtenerIdCliente();

            string sSql;
            string asesor = ddlAsesor.SelectedValue;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString))
            {
                connection.Open();

                // Actualizar el asesor del cliente en la base de datos
                sSql = "UPDATE tblCliente SET Asesor = @Asesor WHERE Id_Cliente = @IdCliente";
                using (SqlCommand updateCommand = new SqlCommand(sSql, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Asesor", asesor);
                    updateCommand.Parameters.AddWithValue("@IdCliente", idCliente);
                     int rowsAffected =  updateCommand.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        Actualizacion();
                    }
                    else
                    {
                      

                        Session.Remove("TipoAccion");

                      

                        string mensajePersonalizado = "NO se pudo Actualizar el cliente";
                        string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                }
            }
        }
        protected void NOActualizarCliente_Click(object sender, EventArgs e)
        {
            

            Session.Remove("TipoAccion");

          

            string mensajePersonalizado = "NO se actualizo la cotizacion";
            string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
        }

        protected void Insercion()
        {

            // Obtener la fecha de cotización con la lógica correspondiente
            string fecha = ObtenerFechaCotizacion();

            // Obtener el descuento del primer dígito
            int primerDigito = ObtenerPrimerDigitoDescuento();

            // Obtener el ID del cliente
            string idCliente = ObtenerIdCliente();

            // Obtener el usuario logueado
            string usuarioLogueado = ObtenerUsuarioLogueado();

            // Conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();

                // Verificar si la cotización ya existe
                bool existeCotizacion = ExisteCotizacion(connection);

                if (!existeCotizacion)
                {
                    // Insertar cotización si no existe
                    InsertarCotizacion(connection, fecha, primerDigito, idCliente, usuarioLogueado);
                }
                else
                {
                    // Mostrar mensaje de cotización existente
                    MostrarMensajeCotizacionExistente();
                }

                // Verificar si existe un registro con el mismo Nit
                bool existeRegistroUltiContCome = ExisteRegistroUltiContCome(connection, idCliente);

                if (!existeRegistroUltiContCome)
                {
                    // Insertar registro en tblUltiContCome si no existe
                    InsertarRegistroUltiContCome(connection, idCliente);
                }
                else
                {
                    // Actualizar registro en tblUltiContCome si ya existe
                    ActualizarRegistroUltiContCome(connection, idCliente, fecha);
                }
            }
        }

        private string ObtenerFechaCotizacion()
        {
            string fecha = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy");
            string dia = DateTime.Now.AddDays(3).DayOfWeek.ToString();
            if (dia == "Saturday" || dia == "Sunday" || dia == "Monday")
            {
                fecha = DateTime.Now.AddDays(5).ToString("dd/MM/yyyy");
            }
            return fecha;
        }

        private int ObtenerPrimerDigitoDescuento()
        {
            double descuentoValue = double.Parse(TextBox11.Text);
            int primerDigito = (int)descuentoValue;
            return primerDigito;
        }

        private string ObtenerIdCliente()
        {
            string IdCliente = Session["Id_ClienteBD2"]?.ToString();
            if (!string.IsNullOrEmpty(IdCliente))
            {
                IdCliente = new string(IdCliente.Where(char.IsDigit).ToArray());
            }
            return IdCliente;
        }

        private string ObtenerUsuarioLogueado()
        {
            string usuarioLogueado = Session["usuariologueado"]?.ToString();
            return usuarioLogueado;
        }

        private bool ExisteCotizacion(SqlConnection connection)
        {
            string cotizacion = textCotizacion.Text.Trim();
            string selectQuery = "SELECT COUNT(*) FROM tblCotización WHERE cotización = @cotizacion";
            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@cotizacion", cotizacion);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private void InsertarCotizacion(SqlConnection connection, string fecha, int primerDigito, string idCliente, string usuarioLogueado)
        {
            string insertQuery = "INSERT INTO tblCotización (Cotización, Estado, Asesor, Cliente, Valor, Fecha_Cotización, Fecha_Respuesta, Plano, Descuento, Observación, obra, Proximo_Seguimiento, Contacto_Cotizacion, Teléfono, Correo_Electronico, Diseño, Zona, ValorSugerido, VCCD, DescuentoComision, Saldo, ValorTteVia, ValorMO, CreadaPor, FechadeCreacion, ModificadaPor, UltmActualizacion, ValorViatico) " +
                                 "VALUES (@cotizacion, @estado, @asesor, @cliente, @valor, @fechaCotizacion, @fechaRespuesta, @plano, @descuento, @observacion, @obra, @proximoSeguimiento, @contactoCotizacion, @telefono, @correoElectronico, @diseno, @zona, @valorSugerido, @vccd, @descuentoComision, @saldo, @valorTteVia, @valorMO, @creadaPor, @fechadeCreacion, @modificadaPor, @ultmActualizacion, @valorViatico)";
            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@cotizacion", textCotizacion.Text.Trim());
                insertCommand.Parameters.AddWithValue("@estado", DropDownListEstado.SelectedItem.Value);
                insertCommand.Parameters.AddWithValue("@asesor", ddlAsesor.SelectedItem.Value);
                insertCommand.Parameters.AddWithValue("@cliente", idCliente);
                insertCommand.Parameters.AddWithValue("@valor", float.Parse(TextBox4.Text));
                insertCommand.Parameters.AddWithValue("@fechaCotizacion", TextFcot.Text);
                insertCommand.Parameters.AddWithValue("@fechaRespuesta", TextFrta.Text);
                insertCommand.Parameters.AddWithValue("@plano", TextPlano.Text);
                insertCommand.Parameters.AddWithValue("@descuento", primerDigito);
                insertCommand.Parameters.AddWithValue("@observacion", TextObs.Text);
                insertCommand.Parameters.AddWithValue("@obra", TextProyecto.Text);
                insertCommand.Parameters.AddWithValue("@proximoSeguimiento", DateTime.Parse(TextFcot.Text).AddDays(7));
                insertCommand.Parameters.AddWithValue("@contactoCotizacion", TextContacto.Text);
                insertCommand.Parameters.AddWithValue("@telefono", TextTelefono.Text);
                insertCommand.Parameters.AddWithValue("@correoElectronico", TextMail.Text);
                insertCommand.Parameters.AddWithValue("@diseno", TextBox2.Text);
                insertCommand.Parameters.AddWithValue("@zona", ddlZona.SelectedItem.Text);
                insertCommand.Parameters.AddWithValue("@valorSugerido", float.Parse(TextBox3.Text));
                insertCommand.Parameters.AddWithValue("@vccd", float.Parse(TextBox7.Text));
                insertCommand.Parameters.AddWithValue("@descuentoComision", TextBox5.Text);
                insertCommand.Parameters.AddWithValue("@saldo", float.Parse(TextBox4.Text));
                insertCommand.Parameters.AddWithValue("@valorTteVia", float.Parse(TextBox8.Text));
                insertCommand.Parameters.AddWithValue("@valorMO", float.Parse(TextBox6.Text));
                insertCommand.Parameters.AddWithValue("@creadaPor", usuarioLogueado);
                insertCommand.Parameters.AddWithValue("@fechadeCreacion", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@modificadaPor", usuarioLogueado);
                insertCommand.Parameters.AddWithValue("@ultmActualizacion", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@valorViatico", float.Parse(TextBox9.Text));

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {                  
                   
                }
                else
                {
                    

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = textCotizacion.Text;

                    string mensajePersonalizado = "NO se pudo Insertar la cotizacion";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
        }

        private void MostrarMensajeCotizacionExistente()
        {
           

            Session.Remove("TipoAccion");

            string mensajePersonalizado = "La cotizacion ya existe, NO se pudo Insertar la cotizacion";
            string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
        }

        private bool ExisteRegistroUltiContCome(SqlConnection connection, string idCliente)
        {
            string selectUltiContComeQuery = "SELECT COUNT(*) FROM tblUltiContCome WHERE uccNit = @clienteNit";
            using (SqlCommand command = new SqlCommand(selectUltiContComeQuery, connection))
            {
                command.Parameters.AddWithValue("@clienteNit", idCliente);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private void InsertarRegistroUltiContCome(SqlConnection connection, string idCliente)
        {
            string insertUltiContComeQuery = "INSERT INTO tblUltiContCome (uccNit, uccRazonSocial, uccAsesor, uccActivo, uccFecha, uccNombreContacto, uccTelefono, uccMail, uccRazon) " +
                                              "VALUES (@clienteNit, @razonSocial, @asesor, 1, @fecha, @nombreContacto, @telefono, @correo, 'COTIZACIÓN')";
            using (SqlCommand insertCommand = new SqlCommand(insertUltiContComeQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@clienteNit", idCliente);
                insertCommand.Parameters.AddWithValue("@razonSocial", TextBox1.Text);
                insertCommand.Parameters.AddWithValue("@asesor", ddlAsesor.SelectedItem.Text);
                insertCommand.Parameters.AddWithValue("@fecha", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@nombreContacto", TextContacto.Text);
                insertCommand.Parameters.AddWithValue("@telefono", TextTelefono.Text);
                insertCommand.Parameters.AddWithValue("@correo", TextMail.Text);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                   

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = textCotizacion.Text;

                    string mensajePersonalizado = "Se inserto la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                  

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = textCotizacion.Text;

                    string mensajePersonalizado = "NO se pudo insertar la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
        }

        private void ActualizarRegistroUltiContCome(SqlConnection connection, string idCliente, string fecha)
        {


            string updateUltiContComeQuery = "UPDATE tblUltiContCome SET uccRazonSocial = @razonSocial, uccAsesor = @asesor, uccActivo = 1, uccFecha = @fecha, " +
                                             "uccNombreContacto = @nombreContacto, uccTelefono = @telefono, uccMail = @correo, uccRazon = 'COTIZACIÓN' WHERE uccNit = @clienteNit";
            using (SqlCommand updateCommand = new SqlCommand(updateUltiContComeQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@razonSocial", TextBox1.Text);
                updateCommand.Parameters.AddWithValue("@asesor", ddlAsesor.SelectedItem.Text);
                updateCommand.Parameters.AddWithValue("@fecha", DateTime.Now);
                updateCommand.Parameters.AddWithValue("@nombreContacto", TextContacto.Text);
                updateCommand.Parameters.AddWithValue("@telefono", TextTelefono.Text);
                updateCommand.Parameters.AddWithValue("@correo", TextMail.Text);
                updateCommand.Parameters.AddWithValue("@clienteNit", idCliente);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Actualización exitosa
                }
                else
                {
                   

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = textCotizacion.Text;

                    string mensajePersonalizado = "NO se pudo Actualizar la informacion del cliente";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }

            string updateClienteQuery = "UPDATE tblCliente SET Ok_seguimiento = 0, Proximo_Seguimiento = @proximoSeguimiento, Responsable = NULL WHERE Id_Cliente = @clienteId";
            using (SqlCommand updateCommand = new SqlCommand(updateClienteQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@proximoSeguimiento", DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                updateCommand.Parameters.AddWithValue("@clienteId", idCliente);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                   

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = textCotizacion.Text;

                    string mensajePersonalizado = "Se Actualizo la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                    

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = textCotizacion.Text;

                    string mensajePersonalizado = "NO se pudo Actualizar el proximo seguimiento la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
        }

        private string ObtenerValorSessionCotizacion()
        {
            // Obtener el valor de la variable de sesión "Cotizacion"
            string vSessionCotizacion = Session["Cotizacion"]?.ToString();

            // Verificar si la variable de sesión "Cotizacion" tiene contenido
            if (!string.IsNullOrEmpty(vSessionCotizacion))
            {
                return vSessionCotizacion; // Retornar el valor de "Cotizacion"
            }
            else
            {

                // Retornar el valor de "Cotizacion2" (incluso si es nulo)
                return vSessionCotizacion;
            }
        }


        protected void Actualizacion()
        {
            string vSessionCotizacion = ObtenerValorSessionCotizacion();
            // Obtener los datos necesarios para la actualización
            string cotizacion = textCotizacion.Text.Trim();
            string estado = DropDownListEstado.SelectedItem.Value;
            string asesor = ddlAsesor.SelectedItem.Value;
            string fechaCotizacion = TextFcot.Text;
            string fechaRespuesta = TextFrta.Text;
            string contactoCotizacion = TextContacto.Text.Trim();
            string telefonoCotizacion = TextTelefono.Text.Trim();
            string correoCotizacion = TextMail.Text.Trim();
            string diseño = TextBox2.Text.Trim();
            string zona = ddlZona.SelectedItem.Value;
            string descuentoComision = TextBox5.Text.Trim();
            int descuentoFactura = ObtenerPrimerDigitoDescuento();
            double valorClienteSinDescuento = double.Parse(TextBox4.Text);
            double valorClienteConDescuento = double.Parse(TextBox7.Text);
            double valorSugerido = double.Parse(TextBox3.Text);
            double valorTteVia = double.Parse(TextBox8.Text);
            double valorMO = double.Parse(TextBox6.Text);
            double valorViatico = double.Parse(TextBox9.Text);
            string obra = TextProyecto.Text.Trim();
            string plano = TextPlano.Text.Trim();
            string observacion = TextObs.Text.Trim();
            string nombreUsuario = Session["usuariologueado"]?.ToString();

            // Obtener el ID del cliente
            string idCliente = ObtenerIdCliente();

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Realizar la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
            
                    // Actualizar la cotización según el estado
                    switch (estado.ToUpper())
                    {
                        case "3": // RECHAZADA
                            ActualizarCotizacionRechazada(connection, cotizacion, vSessionCotizacion, estado, asesor, idCliente, fechaCotizacion, fechaRespuesta, contactoCotizacion, telefonoCotizacion, correoCotizacion, diseño, zona, descuentoComision, descuentoFactura, valorClienteSinDescuento, valorClienteConDescuento, valorSugerido, valorTteVia, valorMO, nombreUsuario, valorViatico, obra, plano, observacion);
                            break;

                        case "2": // APROBADA
                            ActualizarCotizacionAprobada(connection, cotizacion, vSessionCotizacion, estado, valorClienteSinDescuento, descuentoFactura, valorClienteConDescuento, descuentoComision, valorSugerido, valorTteVia, valorMO, nombreUsuario, valorViatico);
                            break;

                        default: // Otros estados
                            ActualizarCotizacionDefault(connection, cotizacion, vSessionCotizacion, estado, asesor, idCliente, fechaCotizacion, fechaRespuesta, contactoCotizacion, telefonoCotizacion, correoCotizacion, diseño, zona, descuentoComision, descuentoFactura, valorClienteSinDescuento, valorClienteConDescuento, valorSugerido, valorTteVia, valorMO, nombreUsuario, valorViatico, obra, plano, observacion);
                            break;
                    }              
            }
        }
    
        // Método para actualizar la cotización en estado RECHAZADA
        private void ActualizarCotizacionRechazada(SqlConnection connection, string cotizacion, string vSessionCotizacion, string estado, string asesor, string idCliente, string fechaCotizacion, string fechaRespuesta, string contactoCotizacion, string telefonoCotizacion, string correoCotizacion, string diseño, string zona, string descuentoComision, int descuentoFactura, double valorClienteSinDescuento, double valorClienteConDescuento, double valorSugerido, double valorTteVia, double valorMO, string nombreUsuario, double valorViatico, string obra, string plano, string observacion)
        {
            string updateQuery = "UPDATE tblCotización SET Estado=@estado, Asesor=@asesor, Cliente=@cliente, Cotización=@cotizacionUpdate, Valor=@valor, Fecha_Cotización=@fechaCotizacion, Fecha_Respuesta=@fechaRespuesta, Plano=@plano, Observación=@observacion, Descuento=@descuentoFactura, Obra=@obra, ID_CausaRechazoCotizacion=@idCausaRechazoCotizacion, ID_Competencia=@idCompetencia, Contacto_Cotizacion=@contactoCotizacion, Teléfono=@telefonoCotizacion, Correo_Electronico=@correoCotizacion, Diseño=@diseño, Zona=@zona, VCCD=@valorClienteConDescuento, DescuentoComision=@descuentoComision, Saldo=@valorClienteSinDescuento, ValorSugerido=@valorSugerido, ValorTteVia=@valorTteVia, ValorMO=@valorMO, ModificadaPor=@nombreUsuario, UltmActualizacion=@fechaActualizacion, ValorViatico=@valorViatico WHERE cotización=@cotizacion";

            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@estado", estado);
                updateCommand.Parameters.AddWithValue("@cotizacionUpdate", cotizacion);
                updateCommand.Parameters.AddWithValue("@cotizacion", vSessionCotizacion); 
                updateCommand.Parameters.AddWithValue("@asesor", asesor);
                updateCommand.Parameters.AddWithValue("@cliente", idCliente);
                updateCommand.Parameters.AddWithValue("@valor", valorClienteSinDescuento);
                updateCommand.Parameters.AddWithValue("@fechaCotizacion", fechaCotizacion);
                updateCommand.Parameters.AddWithValue("@fechaRespuesta", fechaRespuesta);
                updateCommand.Parameters.AddWithValue("@plano", plano);
                updateCommand.Parameters.AddWithValue("@observacion", observacion);
                updateCommand.Parameters.AddWithValue("@descuentoFactura", descuentoFactura);
                updateCommand.Parameters.AddWithValue("@obra", obra);
                updateCommand.Parameters.AddWithValue("@idCausaRechazoCotizacion", 1); // Reemplazar con el valor correcto
                updateCommand.Parameters.AddWithValue("@idCompetencia", 1); // Reemplazar con el valor correcto
                updateCommand.Parameters.AddWithValue("@contactoCotizacion", contactoCotizacion);
                updateCommand.Parameters.AddWithValue("@telefonoCotizacion", telefonoCotizacion);
                updateCommand.Parameters.AddWithValue("@correoCotizacion", correoCotizacion);
                updateCommand.Parameters.AddWithValue("@diseño", diseño);
                updateCommand.Parameters.AddWithValue("@zona", zona);
                updateCommand.Parameters.AddWithValue("@valorClienteConDescuento", valorClienteConDescuento);
                updateCommand.Parameters.AddWithValue("@descuentoComision", descuentoComision);
                updateCommand.Parameters.AddWithValue("@valorClienteSinDescuento", valorClienteSinDescuento);
                updateCommand.Parameters.AddWithValue("@valorSugerido", valorSugerido);
                updateCommand.Parameters.AddWithValue("@valorTteVia", valorTteVia);
                updateCommand.Parameters.AddWithValue("@valorMO", valorMO);
                updateCommand.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                updateCommand.Parameters.AddWithValue("@fechaActualizacion", DateTime.Now);
                updateCommand.Parameters.AddWithValue("@valorViatico", valorViatico);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                   

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = cotizacion;

                    string mensajePersonalizado = "Se Actualizo la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                    

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = cotizacion;

                    string mensajePersonalizado = "NO se pudo Actualizar la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
        }

        // Método para actualizar la cotización en estado APROBADA
        private void ActualizarCotizacionAprobada(SqlConnection connection, string cotizacion, string vSessionCotizacion, string estado, double valorClienteSinDescuento, int descuentoFactura, double valorClienteConDescuento, string descuentoComision, double valorSugerido, double valorTteVia, double valorMO, string nombreUsuario, double valorViatico)
        {
            string updateQuery = "UPDATE tblCotización SET Cotización=@cotizacionUpdate, Valor=@valorClienteSinDescuento, Descuento=@descuentoFactura, VCCD=@valorClienteConDescuento, DescuentoComision=@descuentoComision, Saldo=0, ValorSugerido=@valorSugerido, ValorTteVia=@valorTteVia, ValorMO=@valorMO, ModificadaPor=@nombreUsuario, UltmActualizacion=@fechaActualizacion, ValorViatico=@valorViatico WHERE cotización=@cotizacion";

            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@cotizacion", vSessionCotizacion); 
                updateCommand.Parameters.AddWithValue("@cotizacionUpdate", cotizacion);
                updateCommand.Parameters.AddWithValue("@valorClienteSinDescuento", valorClienteSinDescuento);
                updateCommand.Parameters.AddWithValue("@descuentoFactura", descuentoFactura);
                updateCommand.Parameters.AddWithValue("@valorClienteConDescuento", valorClienteConDescuento);
                updateCommand.Parameters.AddWithValue("@descuentoComision", descuentoComision);
                updateCommand.Parameters.AddWithValue("@valorSugerido", valorSugerido);
                updateCommand.Parameters.AddWithValue("@valorTteVia", valorTteVia);
                updateCommand.Parameters.AddWithValue("@valorMO", valorMO);
                updateCommand.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                updateCommand.Parameters.AddWithValue("@fechaActualizacion", DateTime.Now);
                updateCommand.Parameters.AddWithValue("@valorViatico", valorViatico);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                   

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = cotizacion;

                    string mensajePersonalizado = "Se Actualizo la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                    

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = cotizacion;

                    string mensajePersonalizado = "NO se pudo Actualizar la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
        }

        // Método para actualizar la cotización en otros estados
        private void ActualizarCotizacionDefault(SqlConnection connection, string cotizacion, string vSessionCotizacion, string estado, string asesor, string idCliente, string fechaCotizacion, string fechaRespuesta, string contactoCotizacion, string telefonoCotizacion, string correoCotizacion, string diseño, string zona, string descuentoComision, int descuentoFactura, double valorClienteSinDescuento, double valorClienteConDescuento, double valorSugerido, double valorTteVia, double valorMO, string nombreUsuario, double valorViatico, string obra, string plano, string observacion)
        {
            string updateQuery = "UPDATE tblCotización SET Estado=@estado, Asesor=@asesor, Cliente=@cliente, Cotización=@cotizacionUpdate, Valor=@valor, Fecha_Cotización=@fechaCotizacion, Fecha_Respuesta=@fechaRespuesta, Plano=@plano, Observación=@observacion, Descuento=@descuentoFactura, Obra=@obra, ID_CausaRechazoCotizacion=1, ID_Competencia=1, Contacto_Cotizacion=@contactoCotizacion, Teléfono=@telefonoCotizacion, Correo_Electronico=@correoCotizacion, Diseño=@diseño, Zona=@zona, VCCD=@valorClienteConDescuento, DescuentoComision=@descuentoComision, Saldo=@valorClienteSinDescuento, ValorSugerido=@valorSugerido, ValorTteVia=@valorTteVia, ValorMO=@valorMO, ValorViatico=@valorViatico WHERE cotización=@cotizacion";

            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@estado", estado);
                updateCommand.Parameters.AddWithValue("@cotizacion", vSessionCotizacion);
                updateCommand.Parameters.AddWithValue("@cotizacionUpdate", cotizacion);
                updateCommand.Parameters.AddWithValue("@asesor", asesor);
                updateCommand.Parameters.AddWithValue("@cliente", idCliente);
                updateCommand.Parameters.AddWithValue("@valor", valorClienteSinDescuento);
                updateCommand.Parameters.AddWithValue("@fechaCotizacion", fechaCotizacion);
                updateCommand.Parameters.AddWithValue("@fechaRespuesta", fechaRespuesta);
                updateCommand.Parameters.AddWithValue("@plano", plano);
                updateCommand.Parameters.AddWithValue("@observacion", observacion);
                updateCommand.Parameters.AddWithValue("@descuentoFactura", descuentoFactura);
                updateCommand.Parameters.AddWithValue("@obra", obra);
                updateCommand.Parameters.AddWithValue("@contactoCotizacion", contactoCotizacion);
                updateCommand.Parameters.AddWithValue("@telefonoCotizacion", telefonoCotizacion);
                updateCommand.Parameters.AddWithValue("@correoCotizacion", correoCotizacion);
                updateCommand.Parameters.AddWithValue("@diseño", diseño);
                updateCommand.Parameters.AddWithValue("@zona", zona);
                updateCommand.Parameters.AddWithValue("@valorClienteConDescuento", valorClienteConDescuento);
                updateCommand.Parameters.AddWithValue("@descuentoComision", descuentoComision);
                updateCommand.Parameters.AddWithValue("@valorClienteSinDescuento", valorClienteSinDescuento);
                updateCommand.Parameters.AddWithValue("@valorSugerido", valorSugerido);
                updateCommand.Parameters.AddWithValue("@valorTteVia", valorTteVia);
                updateCommand.Parameters.AddWithValue("@valorMO", valorMO);
                updateCommand.Parameters.AddWithValue("@valorViatico", valorViatico);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                   
                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = cotizacion;

                    string mensajePersonalizado = "Se Actualizo la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                   

                    Session.Remove("TipoAccion");

                    Session["Cotizacion2"] = cotizacion;

                    string mensajePersonalizado = "NO se pudo Actualizar la cotizacion exitosamente!";
                    string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
        }

        private string ValidarCampos()
        {
            string campoFaltante = string.Empty;

            if (string.IsNullOrEmpty(TextBox3.Text))
            {
                campoFaltante = "VVSU";
            }
            else if (string.IsNullOrEmpty(TextBox4.Text))
            {
                campoFaltante = "VCSD";
            }
            else if (string.IsNullOrEmpty(TextBox7.Text))
            {
                campoFaltante = "VCCD";
            }
            else if (string.IsNullOrEmpty(TextBox8.Text))
            {
                campoFaltante = "VTTE";
            }
            else if (string.IsNullOrEmpty(TextBox6.Text))
            {
                campoFaltante = "VMO";
            }
            else if (string.IsNullOrEmpty(TextBox9.Text))
            {
                campoFaltante = "VIA";
            }
            else if (string.IsNullOrEmpty(TextBox11.Text))
            {
                campoFaltante = "D.Fact";
            }
            else if (string.IsNullOrEmpty(TextFcot.Text))
            {
                campoFaltante = "Fecha Cotizacion";
            }
            else if (string.IsNullOrEmpty(TextFrta.Text))
            {
                campoFaltante = "Fecha Respuesta";
            }              
            else if (string.IsNullOrEmpty(TextProyecto.Text))
            {
                campoFaltante = "Proyecto";
            }   
            else if (DropDownListEstado.SelectedItem == null)
            {
                campoFaltante = "Estado";
            }
            else if (ddlAsesor.SelectedValue == null)
            {
                campoFaltante = "Asesor";
            }
            else if (ddlZona.SelectedItem == null)
            {
                campoFaltante = "Zona";
            }    
            else if (string.IsNullOrEmpty(TextBox5.Text))
            {
                campoFaltante = "D.Com";
            }
            return campoFaltante;
        }

        private string ValidarAsesor()
        {
            string campoFaltante = string.Empty;

             if (ddlAsesor.SelectedValue == "")
                {
                    campoFaltante = "Asesor";
                }
            return campoFaltante;
        }

        protected void ConfirmarEliminarCot_Click(object sender, EventArgs e)
        {
           
            Session.Remove("TipoAccion");
        
            string cotizacion = textCotizacion.Text.Trim();
            string contenidoModalOT = "Esta seguro que desea eliminar la cotizacion: " + cotizacion;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ConfirmarEliminarCot').modal('show'); $('#ConfirmarEliminarCot2').text('" + contenidoModalOT + "');", true);
        }

        protected void NOEliminarCot_Click(object sender, EventArgs e)
        {
            Session["Cotizacion2"] = textCotizacion.Text.Trim();

            string mensajePersonalizado = "No se elimino la cotizacion";
            string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
        }

        protected void EliminarCot_Click(object sender, EventArgs e)
        {
            // Obtener el número de cotización de la sesión
            string cotizacion = Session["Cotizacion"] as string;

            // Verificar si la cotización existe en la sesión
            if (!string.IsNullOrEmpty(cotizacion))
            {
                // Construir el comando SQL para eliminar la cotización
                string deleteQuery = "DELETE FROM tblCotización WHERE Cotización = @cotizacion";

                // Obtener la cadena de conexión desde el archivo de configuración
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                // Crear la conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Crear el comando SQL con los parámetros
                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        // Asignar el valor del parámetro cotizacion
                        command.Parameters.AddWithValue("@cotizacion", cotizacion);

                        // Ejecutar el comando DELETE
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si se eliminó alguna fila
                        if (rowsAffected > 0)
                        {
                            Session.Remove("Cotizacion");

                            string mensajePersonalizado = "La cotizacion se elimino exitosamente!";
                            string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                        else
                        {
                            Session.Remove("Cotizacion");

                            string mensajePersonalizado = "No se eliminó ninguna fila, es posible que la cotización no exista";
                            string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                    }
                }
            }
            else
            {
                Session.Remove("Cotizacion");

                // La variable de sesión Cotizacion no está definida o es nula
                string mensajePersonalizado = " La variable de sesión Cotizacion no está definida o es nula";
                string urlRedireccion = "/Formularios/Recepcion/IngresarCotizacion.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }

           
        }

        protected void BtnDiseno_Click(Object sender, EventArgs e)
        {
            string diseñoNumero = TextBox2.Text.Trim();

            if (string.IsNullOrEmpty(diseñoNumero))
            {
                // Si el campo de diseño está vacío, no se hace nada
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;

           
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Obtener información del diseño
                    ObtenerInformacionDelDiseño(connection, diseñoNumero);
                }
           
        }

        private void ObtenerInformacionDelDiseño(SqlConnection connection, string diseñoNumero)
        {
            string sSql = "SELECT * FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseño";

            using (SqlCommand command = new SqlCommand(sSql, connection))
            {
                command.Parameters.AddWithValue("@NumeroDiseño", diseñoNumero);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // Leer los datos del diseño
                        while (reader.Read())
                        {
                            TextContacto.Text = reader["Contacto"].ToString();
                            TextTelefono.Text = reader["Telefono"].ToString();
                            TextMail.Text = reader["mail"].ToString();
                            ddlAsesor.SelectedItem.Text = reader["asesor"].ToString();
                            ddlZona.SelectedItem.Text = reader["Zona"].ToString();
                            TextProyecto.Text = reader["Nombre_Diseño"].ToString();
                            TextPlano.Text = reader["Plano"].ToString();

                            string clienteNombre = reader["Cliente"].ToString();
                            ObtenerInformacionDelCliente(connection, clienteNombre);
                        }
                    }
                    else
                    {
                        // Limpiar el campo de diseño si no se encontraron resultados
                        TextBox2.Text = "";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#DiseNoEncontrado').modal('show');", true);
                    }
                }

               
            }
        }

        private void ObtenerInformacionDelCliente(SqlConnection connection, string clienteNombre)
        {
            string sSql = "SELECT Id_Cliente FROM tblCliente WHERE NombreCompañía = @ClienteNombre";

            using (SqlCommand commandCliente = new SqlCommand(sSql, connection))
            {
                commandCliente.Parameters.AddWithValue("@ClienteNombre", clienteNombre);

                // Crear una nueva conexión y DataReader para obtener información del cliente
                using (SqlConnection clientConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString))
                {
                    clientConnection.Open();

                    commandCliente.Connection = clientConnection;

                    using (SqlDataReader readerCliente = commandCliente.ExecuteReader())
                    {
                        if (readerCliente.Read())
                        {
                            TextBox1.Text = clienteNombre;

                            // Guardar el ID del cliente en una variable de sesión
                            Session["Id_ClienteBD2"] = readerCliente["Id_Cliente"].ToString();
                        }
                        else
                        {
                            // Limpiar los campos si no se encontró el cliente
                            LimpiarCamposCliente();
                            // Mostrar mensaje de advertencia
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ClienteNoEncontrado').modal('show');", true);
                        }
                    }
                }
            }
        }

        private void LimpiarCamposCliente()
        {
            TextContacto.Text = "";
            TextTelefono.Text = "";
            TextMail.Text = "";
            TextProyecto.Text = "";
            TextBox1.Text = "";
            TextPlano.Text = "";
        }

        protected void ddlDise_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnDiseno.Enabled = true;
            BtnDiseno.CssClass = "btn shadow btn-light button-enabled linkButtonClicked2";
        }

    }
}
