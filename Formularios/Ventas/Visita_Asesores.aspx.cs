using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Button = System.Web.UI.WebControls.Button;
using Excel = Microsoft.Office.Interop.Excel;




namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Visita_Asesores : System.Web.UI.Page
    {
        // Variables de clase para almacenar las fechas seleccionadas
        private DateTime fechaInicioSeleccionada;
        private DateTime fechaFinSeleccionada;
        private string Fechas;
        private object filePath;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["usuariologueado"] != null)
                {
                    string usuariologueado = Session["usuariologueado"].ToString();
                    // Llamar al método para cargar los datos en el DropDownList
                    CargarAsesoresEnDropDownList();
                    if (Session["AsesorDiseño"] != null)
                    {
                        string asesorSeleccionado = Session["AsesorDiseño"].ToString();
                        ddlAsesor.SelectedValue = asesorSeleccionado;
                    }
                    // Aquí se  obtiene y muestra el rango de fechas en el título del DataGrid
                    DateRangeLiteral.Text = GetDateRange();
                    CargarClienteYContacto();

                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }

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
                    string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "SELECT X.NombreCompañía, X.Teléfono, Y.NombreContacto, Y.MailContacto " +
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
                                        tbCliente.Text = reader["NombreCompañía"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Teléfono")))
                                    {
                                        tbTelefono.Text = reader["Teléfono"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreContacto")))
                                    {
                                        tbContacto.Text = reader["NombreContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("MailContacto")))
                                    {
                                        tbMailCont.Text = reader["MailContacto"].ToString();
                                    }

                                    Session.Remove("ID_ContactoBD");
                                    Session.Remove("Id_ClienteBD");

                                }
                            }
                        }
                    }

                  

                }


            }
        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial WHERE activo =1 order by Nombre";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto"; // Campos que se mostrará en el DropDownLi
                ddlAsesor.DataValueField = "Cedula";

                ddlAsesor.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }

        protected void ActualizarTituloDataGrid()
        {
            // Verificamos que ambas fechas estén seleccionadas antes de actualizar el título.
            if (fechaInicioSeleccionada != DateTime.MinValue && fechaFinSeleccionada != DateTime.MinValue)
            {
                DateRangeLiteral.Text = "Desde " + fechaInicioSeleccionada.ToString("dd/MM/yyyy") + " hasta " + fechaFinSeleccionada.ToString("dd/MM/yyyy");
            }
        }

        protected string GetDateRange()
        {
            string fechaInicio = fecha1.Text;
            string fechaFin = fecha2.Text;


            return "Desde " + fechaInicio + " hasta " + fechaFin;
        }

        public void Consultar(object sender, EventArgs e)
        {
            ActualizarTituloDataGrid();
            DateRangeLiteral.Text = GetDateRange();
            // habilitar el botón "Cliente"
            Button btnCliente = FindControl("btnCliente") as Button;
            if (btnCliente != null)
            {
                btnCliente.Enabled = false;
                btnCliente.CssClass = "btn btn-outline-secondary";
            }

        }

        public void Cambio(object sender, EventArgs e)
        {

            Session["AsesorDiseño"] = ddlAsesor.SelectedValue;
            Session["AsesorDiseñoNombre"] = ddlAsesor.SelectedItem.Text;

            DateRangeLiteral.Text = GetDateRange();
            Response.Redirect(Request.Url.ToString());
        }

        public void miDataGrid_PreRender(object sender, EventArgs e)
        {
            // Realiza el conteo de filas en el DataGrid y muestra el resultado en el Label

            int cantidadFilas = DataGrid1.Items.Count;
            lbVisitas.InnerText = cantidadFilas.ToString();
            lbFechas.InnerText = "(" + fecha1.Text + ") - (" + fecha2.Text + ")";


            // Textos a buscar en el DataGrid
            List<string> textosABuscar = new List<string>();
            textosABuscar.Add("Visita Levantamiento");
            textosABuscar.Add("Visita Diseño");
            textosABuscar.Add("Visita Cliente Nuevo");
            textosABuscar.Add("Visita Cierre");
            textosABuscar.Add("Seguimiento Cotización");
            textosABuscar.Add("Mantenimiento");
            textosABuscar.Add("Entrega cotización");
            textosABuscar.Add("Cartera");




            // Contador para cada texto buscado
            Dictionary<string, int> conteoPorTexto = new Dictionary<string, int>();

            // Inicializa el conteo para cada texto en 0
            foreach (string texto in textosABuscar)
            {
                conteoPorTexto[texto] = 0;
            }

            // Recorre las filas del DataGrid y cuenta las filas que contienen cada texto
            foreach (DataGridItem item in DataGrid1.Items)
            {
                foreach (string texto in textosABuscar)
                {
                    // Asegúrate de ajustar el índice (en Cells[0]) según la columna en la que deseas buscar el texto
                    if (item.Cells[4].Text.Contains(texto))
                    {
                        conteoPorTexto[texto]++;
                    }
                }
            }

            // Luego en tu código principal, puedes llamar la función para cada campo:
            double VisLev = conteoPorTexto["Visita Levantamiento"];
            lbLev.InnerText = CalcularPorcentaje(VisLev, cantidadFilas).ToString() + " %";

            double VisitaDiseño = conteoPorTexto["Visita Diseño"];
            lbDis.InnerText = CalcularPorcentaje(VisitaDiseño, cantidadFilas).ToString() + " %";

            double VisCliNuevo = conteoPorTexto["Visita Cliente Nuevo"];
            lbCli.InnerText = CalcularPorcentaje(VisCliNuevo, cantidadFilas).ToString() + " %";


            double VisCierre = conteoPorTexto["Visita Cierre"];
            lblCierre.InnerText = CalcularPorcentaje(VisCierre, cantidadFilas).ToString() + " %";

            double SegCotizacion = conteoPorTexto["Seguimiento Cotización"];
            lbSegCot.InnerText = CalcularPorcentaje(SegCotizacion, cantidadFilas).ToString() + " %";

            double VisMantenimiento = conteoPorTexto["Mantenimiento"];
            lbMantenimiento.InnerText = CalcularPorcentaje(VisMantenimiento, cantidadFilas).ToString() + " %";


            double EntregaCotizacion = conteoPorTexto["Entrega cotización"];
            lbEntregaCot.InnerText = CalcularPorcentaje(EntregaCotizacion, cantidadFilas).ToString() + " %";

            double Cartera = conteoPorTexto["Cartera"];
            lbCartera.InnerText = CalcularPorcentaje(Cartera, cantidadFilas).ToString() + " %";


            double suma = VisLev + VisitaDiseño + VisCliNuevo + VisCierre + SegCotizacion + VisMantenimiento + EntregaCotizacion + Cartera;
            lbTotal.InnerText = CalcularPorcentaje(suma, cantidadFilas).ToString() + " %";



        }


        // Definimos una función para calcular el porcentaje redondeado con dos decimales
        public static double CalcularPorcentaje(double conteo, int cantidadFilas)
        {
            if (cantidadFilas != 0)
            {
                double porcentaje = ((double)conteo / cantidadFilas) * 100;
                return Math.Round(porcentaje, 2);
            }
            return 0; // Si cantidadFilas es 0, devolver 0 para evitar división por cero.
        }

        protected void btnCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clientes.aspx");
        }

        protected void ConsultarEstadisticas(object sender, EventArgs e)
        {

            // Realizamos la consulta SQL para obtener los datos necesarios
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                            tblAsesorComercial.Nombre + ' ' + tblAsesorComercial.Apellidos AS Asesor,
                            COUNT(tblVisitaAsesor.Causa) AS CuentaDeCausa
                        FROM 
                            ((tblAsesorComercial
                            LEFT JOIN tblVisitaAsesor ON tblAsesorComercial.Cedula = tblVisitaAsesor.Asesor)
                            FULL JOIN tblCausaVisita ON tblVisitaAsesor.Causa = tblCausaVisita.Id_Causa)
                        WHERE 
                            tblVisitaAsesor.FechaVisita BETWEEN @FechaInicio AND @FechaFin
                            OR tblVisitaAsesor.FechaVisita IS NULL
                        GROUP BY 
                            tblAsesorComercial.CodigoAsesor, tblAsesorComercial.Nombre, tblAsesorComercial.Apellidos
                        ORDER BY 
                            COUNT(tblVisitaAsesor.Causa) DESC";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FechaInicio", fecha5.Text);
                command.Parameters.AddWithValue("@FechaFin", fecha6.Text);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Lista para almacenar los nombres y cantidades
                List<string> nombres = new List<string>();
                List<int> cantidades = new List<int>();

                while (reader.Read())
                {
                    string nombre = reader["Asesor"].ToString();
                    int cantidad = Convert.ToInt32(reader["CuentaDeCausa"]);

                    if (cantidad > 0) // Filtra los asesores con 0 visitas
                    {
                        nombres.Add(nombre);
                        cantidades.Add(cantidad);
                    }
                }

                reader.Close();

                // Generamos la gráfica con los datos obtenidos
                string script = string.Format(@"var nombres = {0}; var cantidades = {1};
                                  GenerarGrafica(nombres, cantidades);",
                                              new JavaScriptSerializer().Serialize(nombres),
                                              new JavaScriptSerializer().Serialize(cantidades));

                ScriptManager.RegisterStartupScript(this, GetType(), "GenerarGrafica", script, true);


            }

            string fechaInicio = fecha5.Text;
            string fechaFin = fecha6.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Nombre del procedimiento almacenado
                string procedimientoAlmacenado = "EstadisticasTipoVisita";

                // Creamos  el comando y asignamos los parámetros
                SqlCommand command = new SqlCommand(procedimientoAlmacenado, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@FechaFin", fechaFin);


                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);


                // Calculamos el total de CuentaDeCausa
                int totalCausas = 0;
                foreach (DataRow row in dt.Rows)
                {
                    totalCausas += Convert.ToInt32(row["CuentaDeCausa"]);
                }

                // Agregampos la columna "Porcentaje" y calculamos los porcentajes para cada fila
                dt.Columns.Add("Porcentaje", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    int cuentaDeCausa = Convert.ToInt32(row["CuentaDeCausa"]);
                    double porcentaje = (cuentaDeCausa / (double)totalCausas) * 100;
                    row["Porcentaje"] = porcentaje.ToString("0.00") + " %";
                }

                // Llenamos el DataGrid con los datos obtenidos
                DataGrid3.DataSource = dt;
                DataGrid3.DataBind();
            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                             tblCausaVisita.NombreCausa AS Nombre,
                             Count(tblVisitaAsesor.Causa) AS CuentaDeCausa
                        FROM 
                            tblCausaVisita
                            LEFT JOIN
                            tblVisitaAsesor ON tblCausaVisita.Id_Causa = tblVisitaAsesor.Causa 
                            AND tblVisitaAsesor.FechaVisita BETWEEN @FechaInicio AND @FechaFin
                        WHERE 
                             tblVisitaAsesor.FechaVisita BETWEEN @FechaInicio AND @FechaFin
                        GROUP BY 
                           tblCausaVisita.Id_Causa, tblCausaVisita.NombreCausa 
                        ORDER BY 
                           CuentaDeCausa DESC, tblCausaVisita.NombreCausa DESC";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FechaInicio", fecha5.Text);
                command.Parameters.AddWithValue("@FechaFin", fecha6.Text);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Lista para almacenar los nombres y cantidades
                List<string> nombres1 = new List<string>();
                List<int> cantidades1 = new List<int>();

                while (reader.Read())
                {
                    string nombre1 = reader["Nombre"].ToString();
                    int cantidad1 = Convert.ToInt32(reader["CuentaDeCausa"]);

                    if (cantidad1 > 0) // Filtra los asesores con 0 visitas
                    {
                        nombres1.Add(nombre1);
                        cantidades1.Add(cantidad1);
                    }
                }

                reader.Close();



                // Generamos la gráfica con los datos obtenidos
                string script = string.Format(@"var nombres1 = {0}; var cantidades1 = {1};
                                  GenerarGrafica1(nombres1, cantidades1);",
                                              new JavaScriptSerializer().Serialize(nombres1),
                                              new JavaScriptSerializer().Serialize(cantidades1));

                ScriptManager.RegisterStartupScript(this, GetType(), "GenerarGrafica1", script, true);

            }

        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Agregamos el atributo onclick a cada fila para capturar el evento de clic y agregmos el cursor
                e.Item.Attributes["onclick"] = "seleccionarFila('" + e.Item.ItemIndex + "')";
                e.Item.Style["cursor"] = "pointer";
            }
        }

        protected void DataGrid2_ItemDataBound(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid2.Items[rowIndex];

                string Asesor = row.Cells[1].Text;
                string FechaInicio = fecha5.Text;
                string FechaFin = fecha6.Text;

                LlenarDetalle.SelectParameters["Asesor"].DefaultValue = Asesor;
                LlenarDetalle.SelectParameters["FechaInicio"].DefaultValue = FechaInicio;
                LlenarDetalle.SelectParameters["FechaFin"].DefaultValue = FechaFin;

                // Actualizar el segundo DataGrid con los datos del procedimiento almacenado
                DataGrid2.DataBind();
            }
        }

        protected void ExportarExel(object sender, EventArgs e)
        {
            try
            {
                // Mostrar el modal de carga
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowLoadingModal", "mostrarModal();", true);



                // Crear una nueva instancia de Excel
                var excelApp = new Excel.Application();

                if (excelApp == null)
                {
                    Console.WriteLine("Excel no está instalado en esta máquina.");
                    return;
                }
                // Crear un nuevo libro y hoja de Excel
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Agregar título a la tabla
                var tableTitle = "Ducon S.A.S";
                var titleRange = worksheet.Range["B1", "D1"];
                titleRange.Merge(); // Fusionar celdas para el título
                titleRange.Value = tableTitle;
                titleRange.Font.Size = 16;  // Tamaño de fuente
                titleRange.Font.Bold = true;  // Texto en negrita
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // Centrar el título
                titleRange.EntireRow.Font.Color = System.Drawing.Color.Black;  // Cambiar el color de fuente

                titleRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                // Agregar subtítulo
                var subtitleRange = worksheet.Range["B2", "D2"];
                subtitleRange.Merge(); // Fusionar celdas para el subtítulo
                subtitleRange.Value = "Visita Asesores entre: " + fecha5.Text + " Y " + fecha6.Text; // Cambiar por el subtítulo deseado
                subtitleRange.Font.Size = 12;
                subtitleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Aplicar bordes a la celda de subtítulo
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;



                int rowIndexx = 4; // Comenzar a escribir la tabla a partir de la fila 2


                // Escribir el encabezado de la tabla
                int colIndex = 1; // Columna 1 en Excel
                foreach (DataGridColumn column in DataGrid2.Columns)
                {
                    // Excluir la primera columna (LinkButton)
                    if (colIndex != 1)
                    {
                        // Escribe el valor del encabezado en la hoja de Excel
                        worksheet.Cells[rowIndexx - 1, colIndex] = column.HeaderText;
                        // Obtener el rango de la celda de encabezado
                        var headerCell = (Excel.Range)worksheet.Cells[rowIndexx - 1, colIndex];
                        headerCell.Font.Bold = true;  // Establecer el texto en negrita
                        headerCell.Interior.Color = System.Drawing.Color.LightGray;  // Cambiar el color de fondo

                        // Aplicar bordes a la celda de encabezado
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;


                    }
                    colIndex++;
                }

                foreach (DataGridItem item in DataGrid2.Items)
                {
                    colIndex = 1; // Comenzar en la columna 1 de Excel
                    foreach (TableCell cell in item.Cells)
                    {
                        // Excluir la primera columna (LinkButton)
                        if (colIndex != 1)
                        {
                            // Escribe el valor de la celda en la hoja de Excel
                            worksheet.Cells[rowIndexx, colIndex] = cell.Text;
                            // Obtener el rango de la celda actual
                            var cellRange = (Excel.Range)worksheet.Cells[rowIndexx, colIndex];

                            // Aplicar bordes a la celda actual
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        colIndex++;
                    }
                    rowIndexx++;
                }

                // Agregar un gráfico de barras utilizando las columnas "C" y "D" (Nombres y Cantidad Visitas)
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Excel.ChartObject chartObject = chartObjects.Add(100, 100, 400, 250);
                Excel.Chart chart = chartObject.Chart;

                // Definir el rango de datos para el gráfico (columnas "C" y "D")
                Excel.Range chartRange = worksheet.Range["C4", "D" + (rowIndexx - 1)]; // Columnas "C" y "D"
                chart.SetSourceData(chartRange, Excel.XlRowCol.xlColumns);

                // Cambiar el tipo de gráfico a barras verticales
                chart.ChartType = Excel.XlChartType.xlColumnClustered;

                // Configurar el eje X para que muestre los nombres de las barras (columna "C")
                Excel.Axis xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                xAxis.CategoryNames = worksheet.Range["C4", "C" + (rowIndexx - 1)];

                // Agregar título al gráfico
                chart.HasTitle = true;
                chart.ChartTitle.Text = "Vista Asesores";


                // Refrescar la página después de cerrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);

                worksheet.Columns.AutoFit();
                // Mostrar la aplicación de Excel
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar a Excel: " + ex.Message);
            }

        }

        protected void ExportarExel2(object sender, EventArgs e)
        {
            try
            {
                // Crear una nueva instancia de Excel
                var excelApp = new Excel.Application();

                if (excelApp == null)
                {
                    Console.WriteLine("Excel no está instalado en esta máquina.");
                    return;
                }

                // Crear un nuevo libro y hoja de Excel
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Agregar título a la tabla
                var tableTitle = "Detalle Visitas " + ddlAsesor.SelectedItem.Text;
                var titleRange = worksheet.Range["A1", "G1"];
                titleRange.Merge(); // Fusionar celdas para el título
                titleRange.Value = tableTitle;
                titleRange.Font.Size = 16;  // Tamaño de fuente
                titleRange.Font.Bold = true;  // Texto en negrita
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // Centrar el título
                titleRange.EntireRow.Font.Color = System.Drawing.Color.Black;  // Cambiar el color de fuente

                // Agregar subtítulo
                var subtitleRange = worksheet.Range["A2", "G2"];
                subtitleRange.Merge(); // Fusionar celdas para el subtítulo
                subtitleRange.Value = "Visita  entre: " + fecha1.Text + " Y " + fecha2.Text; // Cambiar por el subtítulo deseado
                subtitleRange.Font.Size = 12;
                subtitleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                int rowIndexx = 4; // Comenzar a escribir la tabla a partir de la fila 4

                // Escribir el encabezado de la primera tabla
                int colIndex = 1; // Columna 1 en Excel

                foreach (DataGridColumn column in DataGrid1.Columns)
                {
                    if (colIndex != 8)
                    {
                        // Escribe el valor del encabezado en la hoja de Excel
                        worksheet.Cells[rowIndexx - 1, colIndex] = column.HeaderText;
                        // Obtener el rango de la celda de encabezado
                        var headerCell = (Excel.Range)worksheet.Cells[rowIndexx - 1, colIndex];
                        headerCell.Font.Bold = true;  // Establecer el texto en negrita
                        headerCell.Interior.Color = System.Drawing.Color.LightGray;  // Cambiar el color de fondo

                        // Aplicar bordes a la celda de encabezado
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    }

                    colIndex++;
                }

                foreach (DataGridItem item in DataGrid1.Items)
                {
                    colIndex = 1; // Comenzar en la columna 1 de Excel
                    foreach (TableCell cell in item.Cells)
                    {
                        if (colIndex != 8)
                        {
                            // Reemplazar "&nbsp;" con un valor vacío
                            string cellValue = cell.Text.Replace("&nbsp;", string.Empty);

                            // Escribe el valor de la celda en la hoja de Excel
                            worksheet.Cells[rowIndexx, colIndex] = cellValue;
                            // Obtener el rango de la celda actual
                            var cellRange = (Excel.Range)worksheet.Cells[rowIndexx, colIndex];

                            // Aplicar bordes a la celda actual
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        }

                        colIndex++;
                    }
                    rowIndexx++;
                }

                // Dejar dos filas de espacio
                rowIndexx += 2;

                // Agregar título a la segunda tabla (tabla2)
                var table2Title = "Estadisticas Visitas"; // Cambiar el título deseado
                var table2TitleRange = worksheet.Range["A" + rowIndexx.ToString(), "G" + rowIndexx.ToString()];
                table2TitleRange.Merge(); // Fusionar celdas para el título de la segunda tabla
                table2TitleRange.Value = table2Title;
                table2TitleRange.Font.Size = 14;  // Tamaño de fuente
                table2TitleRange.Font.Bold = true;  // Texto en negrita
                table2TitleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // Centrar el título

                // Incrementar la fila para empezar a escribir los encabezados de la segunda tabla
                rowIndexx++;

                // Crear encabezados manuales para la segunda tabla
                string[] encabezados = { "Rango Fechas", "Visitas", "Visita Levantamiento %", "Visita Diseño %", "Visita Cliente Nuevo %",
                             "Visita Cierre %", "Seguimiento Cotizacion" ,"Mantenimiento%","Entrega cotización%","Cartera%","%Total" };
                int headerIndex = 1;

                // Escribe los encabezados en la hoja de Excel para la segunda tabla
                foreach (string encabezado in encabezados)
                {
                    // Escribe el valor del encabezado en la hoja de Excel
                    worksheet.Cells[rowIndexx, headerIndex] = encabezado;

                    // Obtener el rango de la celda de encabezado
                    var headerCell = (Excel.Range)worksheet.Cells[rowIndexx, headerIndex];
                    headerCell.Font.Bold = true;  // Establecer el texto en negrita
                    headerCell.Interior.Color = System.Drawing.Color.LightGray;  // Cambiar el color de fondo

                    // Aplicar bordes a la celda de encabezado
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                    headerIndex++;
                }

                // Obtener los datos desde las etiquetas (labels) para la segunda tabla
                string[] datosTabla2 = { fecha1.Text + " Y " + fecha1.Text, lbVisitas.InnerText, lbLev.InnerText, lbDis.InnerText, lbCli.InnerText, lblCierre.InnerText, lbSegCot.InnerText,
                             lbMantenimiento.InnerText, lbEntregaCot.InnerText, lbCartera.InnerText, lbTotal.InnerText};

                // Escribir los datos en la hoja de Excel para la segunda tabla
                rowIndexx++; // Avanzar a la siguiente fila
                colIndex = 1; // Reiniciar la columna

                foreach (string dato in datosTabla2)
                {
                    // Escribe el valor de la celda en la hoja de Excel
                    worksheet.Cells[rowIndexx, colIndex] = dato;
                    // Obtener el rango de la celda actual
                    var cellRange = (Excel.Range)worksheet.Cells[rowIndexx, colIndex];

                    // Aplicar bordes a la celda actual
                    cellRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    cellRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    cellRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    cellRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    // Alinear el contenido de la celda al centro
                    cellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Avanzar a la siguiente columna
                    colIndex++;
                }

                // Ajustar el ancho de las columnas para ambas tablas
                worksheet.Columns.AutoFit();
                // Mostrar la aplicación de Excel
                excelApp.Visible = true;

                Response.Redirect(Request.Url.ToString());



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar a Excel: " + ex.Message);
            }


        }

        protected void ExportarExel3(object sender, EventArgs e)


        {
            try
            {
                // Crear una nueva instancia de Excel
                var excelApp = new Excel.Application();

                if (excelApp == null)
                {
                    Console.WriteLine("Excel no está instalado en esta máquina.");
                    return;
                }
                // Crear un nuevo libro y hoja de Excel
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Agregar título a la tabla
                var tableTitle = "Ducon S.A.S";
                var titleRange = worksheet.Range["B1", "D1"];
                titleRange.Merge(); // Fusionar celdas para el título
                titleRange.Value = tableTitle;
                titleRange.Font.Size = 16;  // Tamaño de fuente
                titleRange.Font.Bold = true;  // Texto en negrita
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // Centrar el título
                titleRange.EntireRow.Font.Color = System.Drawing.Color.Black;  // Cambiar el color de fuente

                titleRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                // Agregar subtítulo
                var subtitleRange = worksheet.Range["B2", "D2"];
                subtitleRange.Merge(); // Fusionar celdas para el subtítulo
                subtitleRange.Value = "Visita Asesores entre: " + fecha5.Text + " Y " + fecha6.Text; // Cambiar por el subtítulo deseado
                subtitleRange.Font.Size = 12;
                subtitleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Aplicar bordes a la celda de subtítulo
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                subtitleRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;



                int rowIndexx = 4; // Comenzar a escribir la tabla a partir de la fila 2


                // Escribir el encabezado de la tabla
                int colIndex = 1; // Columna 1 en Excel
                foreach (DataGridColumn column in DataGrid3.Columns)
                {
                    // Excluir la primera columna (LinkButton)
                    if (colIndex != 1)
                    {
                        // Escribe el valor del encabezado en la hoja de Excel
                        worksheet.Cells[rowIndexx - 1, colIndex] = column.HeaderText;
                        // Obtener el rango de la celda de encabezado
                        var headerCell = (Excel.Range)worksheet.Cells[rowIndexx - 1, colIndex];
                        headerCell.Font.Bold = true;  // Establecer el texto en negrita
                        headerCell.Interior.Color = System.Drawing.Color.LightGray;  // Cambiar el color de fondo

                        // Aplicar bordes a la celda de encabezado
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;


                    }
                    colIndex++;
                }

                foreach (DataGridItem item in DataGrid3.Items)
                {
                    colIndex = 1; // Comenzar en la columna 1 de Excel
                    foreach (TableCell cell in item.Cells)
                    {
                        // Excluir la primera columna (LinkButton)
                        if (colIndex != 1)
                        {
                            // Escribe el valor de la celda en la hoja de Excel
                            worksheet.Cells[rowIndexx, colIndex] = cell.Text;
                            // Obtener el rango de la celda actual
                            var cellRange = (Excel.Range)worksheet.Cells[rowIndexx, colIndex];

                            // Aplicar bordes a la celda actual
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        colIndex++;
                    }
                    rowIndexx++;
                }

                // Agregar un gráfico de barras utilizando las columnas "C" y "D" (Nombres y Cantidad Visitas)
                Excel.ChartObjects chartObjects = (Excel.ChartObjects)worksheet.ChartObjects(Type.Missing);
                Excel.ChartObject chartObject = chartObjects.Add(100, 100, 400, 250);
                Excel.Chart chart = chartObject.Chart;

                // Definir el rango de datos para el gráfico (columnas "C" y "D")
                Excel.Range chartRange = worksheet.Range["B4", "C" + (rowIndexx - 1)]; // Columnas "C" y "D"
                chart.SetSourceData(chartRange, Excel.XlRowCol.xlColumns);

                // Cambiar el tipo de gráfico a barras verticales
                chart.ChartType = Excel.XlChartType.xlColumnClustered;

                // Configurar el eje X para que muestre los nombres de las barras (columna "C")
                Excel.Axis xAxis = (Excel.Axis)chart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
                xAxis.CategoryNames = worksheet.Range["B4", "C" + (rowIndexx - 1)];

                // Agregar título al gráfico
                chart.HasTitle = true;
                chart.ChartTitle.Text = "Estadistica por Tipo Visita";

                worksheet.Columns.AutoFit();
                // Mostrar la aplicación de Excel
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar a Excel: " + ex.Message);
            }

        }


        protected void GuardarModificarCliente(object sender, EventArgs e)
        {
            bool guardarCliente = chkEstadoGuardarCliente.Checked;

         
            if (guardarCliente)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Realizamos la Insercion 
                    string query = "INSERT INTO tblvisitaasesor (Observacion, Fechavisita, Causa, Cotizacion, Id_ClienteContacto,Asesor) " +
                                   "VALUES (@Observacion, @Fechavisita, @Causa,@Cotizacion, @Id_ClienteContacto, @Asesor)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Observacion", txObs.Value);
                        command.Parameters.AddWithValue("@Fechavisita", fecha.Value);
                        command.Parameters.AddWithValue("@Causa", ddlVisitasPor.SelectedValue);
                        command.Parameters.AddWithValue("@Cotizacion", tbCotizacion.Text);
                        command.Parameters.AddWithValue("@Id_ClienteContacto", Session["Id_contactoBD"].ToString());
                        command.Parameters.AddWithValue("@Asesor", ddlAsesor.SelectedValue);

                        command.ExecuteNonQuery();
                    }

                    // Define el mensaje personalizado
                    string mensajePersonalizado = "La Visita ha sido ingresada con éxito";

                    // Define la URL de redirección
                    string urlRedireccion = "Ventas/Visita_Asesores.aspx";

                    // Redirige a la página de éxito con el mensaje personalizado y la URL de redirección como parámetros
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                }


            }

            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Realizamos actualización
                    string query = "UPDATE tblvisitaasesor SET " +
                                   "Observacion = @Observacion, Fechavisita = @Fechavisita, Causa = @Causa, Cotizacion = @Cotizacion, Id_ClienteContacto = @Id_ClienteContacto " +
                                   "WHERE Id = @IdVisita AND Id_Clientecontacto = @Id_ClienteContacto";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Observacion", txObs.Value);
                        command.Parameters.AddWithValue("@Fechavisita", fecha.Value);
                        command.Parameters.AddWithValue("@Causa", ddlVisitasPor.SelectedValue);
                        command.Parameters.AddWithValue("@Cotizacion", tbCotizacion.Text);
                        command.Parameters.AddWithValue("@IdVisita", tbIdVisita.Text);
                        command.Parameters.AddWithValue("@Id_ClienteContacto", tbIdContacto.Text);
                      

                        command.ExecuteNonQuery();
                    }
                    // Define el mensaje personalizado
                    string mensajePersonalizado = "La visita ha sido actualizada con éxito";
                    // Define la URL de redirección
                    string urlRedireccion = "Ventas/Visita_Asesores.aspx";
                    // Redirige a la página de éxito con el mensaje personalizado y la URL de redirección como parámetros
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                }

            }

           


        }

    }
}