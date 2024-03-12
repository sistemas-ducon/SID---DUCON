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
                    CargarAsesoresEnDropDownList();
                    DisposicionDeBotonesPg();

                    listaTextBoxes = new List<TextBox>
                         {
                   textCotizacion, TextBox1, TextBox2, TextContacto, TextTelefono, TextMail, TextCompe, TextCausa, TextFcot, TextFrta, TextPlano, TextObs, TextProyecto,
                   TextBox3, TextBox4, TextBox5, TextBox11, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10

                        };
                    DeshabilitarTextBoxes(listaTextBoxes);
                    listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,ddlAsesor,DropDownListEstado

                };
                    DeshabilitarDropDownLists(listaDropDownLists);
                    CargarDropDownListEstado();

                    TextFcot.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextFrta.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    TextCompe.Text = "POR DEFINIR";
                    TextCausa.Text = "POR DEFINIR";

                    CargarClienteYContacto();
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
                        string query = "SELECT X.NombreCompañía, X.Dirección, Y.NombreContacto, Y.MailContacto, Y.Telefono,  X.Asesor " +
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

                    Session.Remove("Id_ClienteBD");
                    Session.Remove("ID_ContactoBD");

                    habilitarTextBoxes();

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
                if (textBox == textCotizacion || textBox == TextBox1 || textBox == TextBox2 || textBox == TextContacto || textBox == TextTelefono || textBox == TextMail || textBox == TextCompe || textBox == TextCausa ||
                   textBox == TextFcot || textBox == TextFrta || textBox == TextPlano || textBox == TextObs || textBox == TextProyecto || textBox == TextBox3 || textBox == TextBox4 || textBox == TextBox5 || textBox == TextBox11 || textBox == TextBox6
                   || textBox == TextBox7 || textBox == TextBox8 || textBox == TextBox9 || textBox == TextBox10)

                {
                    textBox.Enabled = false;
                    textBox.CssClass = "form-control form-control-sm button-disabled linkButtonClicked";
                }


            }



            BtnCliente.Enabled = false;
            BtnCliente.CssClass = "btn btn-sm button-disabled shadow-sm linkButtonClicked";

            LinkButton5.Enabled = false;
            LinkButton5.CssClass = "btn shadow btn-light button-disabled linkButtonClicked";
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

            EliminarCot.Enabled = true;
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
                if (zona != null)
                {
                    ddlZona.SelectedValue = zona.ToString();
                    ddlZona.Enabled = false;
                    ddlZona.CssClass = "form-control";
                }
                else
                {

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
                   textCotizacion, TextBox1, TextBox2, TextContacto, TextTelefono, TextMail, TextCompe, TextCausa, TextFcot, TextFrta, TextPlano, TextObs, TextProyecto,
                   TextBox3, TextBox4, TextBox5, TextBox11, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10

                        };
            DeshabilitarTextBoxes(listaTextBoxes);
            listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,ddlAsesor,DropDownListEstado

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
    cli.NombreCompañía AS Cliente2 
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
                            TextBox1.Text = reader["Cliente2"].ToString();
                            TextBox2.Text = reader["Diseño"].ToString();
                            TextContacto.Text = reader["Contacto_Cotizacion"].ToString();
                            TextTelefono.Text = reader["Teléfono"].ToString();
                            TextMail.Text = reader["Correo_Electronico"].ToString();
                            TextCompe.Text = reader["NombreCompetencia"].ToString();
                            TextCausa.Text = reader["CausaRechazoCotizacion"].ToString();

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

                            TextBox3.Text = reader["ValorSugerido"].ToString();
                            TextBox4.Text = reader["Valor"].ToString();
                            TextBox5.Text = reader["DescuentoComision"].ToString();
                            TextBox11.Text = reader["Descuento"].ToString();
                            TextBox6.Text = reader["ValorMO"].ToString();
                            TextBox7.Text = reader["VCCD"].ToString();
                            TextBox8.Text = reader["ValorTteVia"].ToString();
                            TextBox9.Text = reader["ValorViatico"].ToString();

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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#llenarCliente').modal('show');", true);

            habilitarTextBoxes();
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            listaTextBoxes = new List<TextBox>
                         {
                   textCotizacion, TextBox1, TextBox2, TextContacto, TextTelefono, TextMail, TextCompe, TextCausa, TextFcot, TextFrta, TextPlano, TextObs, TextProyecto,
                   TextBox3, TextBox4, TextBox5, TextBox11, TextBox6, TextBox7, TextBox8, TextBox9, TextBox10

                        };
            DeshabilitarTextBoxes(listaTextBoxes);
            listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,ddlAsesor,DropDownListEstado

                };
            DeshabilitarDropDownLists(listaDropDownLists);
        }

        protected void BtnCliente_Click(object sender, EventArgs e)
        {
            // Verificar si el DropDownList ddlAsesor tiene contenido seleccionado
            if (!string.IsNullOrEmpty(ddlAsesor.SelectedValue))
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + "Clientes.aspx" + "', '_blank');", true);
            }
            else
            {
                // Mostrar el modal para indicar al usuario que debe llenar el DropDownList ddlAsesor
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#llenarCliente').modal('show');", true);
            }
        }

        protected void txtCotizacion_TextChanged(object sender, EventArgs e)
        {
            // Ruta del archivo de Excel
            string filePath = @"\\172.16.30.6\PruebaDocumentacion\COTIZACION\C92717.xls";

            // Texto a buscar
            string textoABuscar = "vccd";

            // Variables para almacenar los valores de las celdas
            double valorE14 = 0;
            double valorE15 = 0;
            double valorD14 = 0;
            double valorD15 = 0;
            double valorF18 = 0;
            double resultadoFinal = 0;

            // Verificar si el archivo existe
            if (File.Exists(filePath))
            {
                // Leer el contenido del archivo de Excel
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = null;

                    // Determinar el tipo de archivo Excel (XLS o XLSX)
                    if (Path.GetExtension(filePath).Equals(".xls"))
                    {
                        workbook = new HSSFWorkbook(fs); // Para archivos .xls (Excel 97-2003)
                    }
                    else if (Path.GetExtension(filePath).Equals(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(fs); // Para archivos .xlsx (Excel 2007 y posteriores)
                    }

                    // Obtener el primer worksheet
                    ISheet sheet = workbook.GetSheetAt(0);

                    // Iterar sobre las filas del worksheet para buscar el texto
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            // Buscar el texto en todas las celdas de la fila
                            foreach (ICell cell in row.Cells)
                            {
                                if (cell.ToString().Contains(textoABuscar))
                                {
                                    // Obtener los valores de las celdas E14, E15, D14, D15 y F18
                                    valorE14 = GetCellValue(sheet.GetRow(13).GetCell(4)); // Fila 14, Columna 'E'
                                    valorE15 = GetCellValue(sheet.GetRow(14).GetCell(4)); // Fila 15, Columna 'E'
                                    valorD14 = GetCellValue(sheet.GetRow(13).GetCell(3)); // Fila 14, Columna 'D'
                                    valorD15 = GetCellValue(sheet.GetRow(14).GetCell(3)); // Fila 15, Columna 'D'
                                    valorF18 = GetCellValue(sheet.GetRow(17).GetCell(5)); // Fila 18, Columna 'F'

                                    // Realizar las multiplicaciones y sumas
                                    resultadoFinal = (valorE14 * valorD14) + (valorE15 * valorD15) + valorF18;

                                    // Mostrar el resultado en el TextBox7
                                    TextBox7.Text = resultadoFinal.ToString();

                                    // Salir del bucle externo
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // El archivo de Excel no existe
                // Realiza alguna acción en consecuencia
            }
        }

        // Método para obtener el valor de la celda, manejar celdas nulas y devolver un valor numérico
        private double GetCellValue(ICell cell)
        {
            if (cell == null)
                return 0; // Si la celda es nula, devolver 0
            else if (cell.CellType == CellType.Numeric)
                return cell.NumericCellValue; // Si la celda contiene un valor numérico, devolver el valor
            else
                return 0; // En cualquier otro caso, devolver 0
        }

    }
}