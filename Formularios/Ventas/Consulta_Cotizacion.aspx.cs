using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Configuration;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Consulta_Cotizacion : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";

        private string CadenaConexionISID = "BD_ISIDSQL";

        private string CadenaConexionSSF = "BD_SSF";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ConsultarDatos();
                DataGrid2.DataBind();
                LoadEstados();
               

                if (Session["CedulaLogeada"] != null)
                {
                    string usuariologueado = Session["CedulaLogeada"].ToString();

                    // Realizar la conexión a la base de datos y la consulta para obtener el nombre y apellido del usuario
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT Nombre, Apellidos, cedula FROM tblEmpleado WHERE cedula = @nombreUsuario";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@nombreUsuario", usuariologueado);
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                string nombre = reader["Nombre"].ToString();
                                string apellidos = reader["Apellidos"].ToString();
                                TextAsesor.Text = nombre + " " + apellidos; // Asignar el nombre y apellidos al TextBox
                                TextAsesortab2.Text = nombre + " " + apellidos;
                                TextAsesorSeguimiento.Text = nombre + " " + apellidos;
                            }
                            
                        }
                    }
                }

                else
                {
                    Response.Redirect("/Formularios/Login.aspx");
                }
               
            }


        }

        //POR VENDEDOR
        protected void TapPorVendedor_Click(object sender, EventArgs e)
        {


            // Realiza la consulta utilizando el SqlDataSource
            DataGridConsultaCotizaciones.SelectCommand = "cta_Cotizaciones_Por_Vendedor"; // Nombre del nuevo procedimiento almacenado
            DataGridConsultaCotizaciones.SelectParameters.Clear();
            DataGridConsultaCotizaciones.SelectParameters.Add("NombreAsesor", TextAsesor.Text);
            DataGridConsultaCotizaciones.SelectParameters.Add("FechaInicio", TextBoxStartDate.Text);
            DataGridConsultaCotizaciones.SelectParameters.Add("FechaFin", TextBoxEndDate.Text);

            // Refresca los datos del DataGrid
            DataGridPorEstado.DataBind();
            UpdatePanelPorVendedor.Update();
        }

        protected void LoadEstados()
        {

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT Descripción_Estado FROM tblEstado_Cotización";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    ddlEstadoCotizacion.DataSource = reader;
                    ddlEstadoCotizacion.DataTextField = "Descripción_Estado";
                    ddlEstadoCotizacion.DataBind();

                    connection.Close();
                }
            }

            // Agrega un ítem de selección por defecto si lo deseas:
            ddlEstadoCotizacion.Items.Insert(0, new ListItem("Selecciona un estado", ""));
        }

        protected void BtnConsultarPorEstado_Click(object sender, EventArgs e)
        {
            
                // Obtén el valor seleccionado del DropDownList
                string estadoSeleccionado = ddlEstadoCotizacion.SelectedValue;

                // Realiza la consulta utilizando el SqlDataSource
                DataGridPorEstado.SelectCommand = "cta_Cotizaciones_Por_Estado"; // Nombre del nuevo procedimiento almacenado
                DataGridPorEstado.SelectParameters.Clear();
                DataGridPorEstado.SelectParameters.Add("NombreAsesor", TextAsesortab2.Text);
                DataGridPorEstado.SelectParameters.Add("FechaInicio", TextCotizacionEntreInicio2.Text);
                DataGridPorEstado.SelectParameters.Add("FechaFin", TextCotizacionEntreFinal2.Text);
                DataGridPorEstado.SelectParameters.Add("Estado", estadoSeleccionado);

            // Refresca los datos del DataGrid
            DataGrid2.DataBind();
            UpdatePanelPorEstado.Update();
           
        }
        
        //TABLA POR VENDEDOR

        public void DataGridPorVendedor_PreRender(object sender, EventArgs e)
        {
            // Realiza el conteo de filas en el DataGrid y muestra el resultado en el Label

            int cantidadFilas = DataGrid1.Items.Count;
            lbCantidad.InnerText = cantidadFilas.ToString();
            lbEstudio.InnerText = ("Estudio").ToString();
            lbAprobada.InnerText = ("Aprobada").ToString();
            lbTotales.InnerText = ("Totales").ToString();

            // Textos a buscar en el DataGrid
            List<string> textosABuscar = new List<string>();
            textosABuscar.Add("Cant");
            textosABuscar.Add("Estudio");
            textosABuscar.Add("Aprobada");



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
                    if (item.Cells[1].Text.Contains(texto))
                    {
                        conteoPorTexto[texto]++;
                    }

                }
            }




            // Luego en tu código principal, puedes llamar la función para cada campo:
            double CanEst = conteoPorTexto["Estudio"];
            lbCantEstudio.InnerText = (CanEst).ToString();

            double CanApr = conteoPorTexto["Aprobada"];
            lbCantApr.InnerText = (CanApr).ToString();

            double PorEst = conteoPorTexto["Estudio"];
            lbPorEst.InnerText = CalcularPorcentaje(PorEst, cantidadFilas).ToString() + " %";

            double PorApr = conteoPorTexto["Aprobada"];
            lbPorApr.InnerText = CalcularPorcentaje(PorApr, cantidadFilas).ToString() + " %";

            double suma = PorEst + PorApr; lblTotal.InnerText = CalcularPorcentaje(suma, cantidadFilas).ToString() + " %";


            double sumaValorNetoEstudio = 0;
            double sumaValorNetoAprobada = 0;

            // Recorre las filas del DataGrid y suma los valores de la columna "VCCD" para cada estado
            foreach (DataGridItem item in DataGrid1.Items)
            {
                string estado = item.Cells[1].Text;
                string valorNetoStr = item.Cells[5].Text;

                if (double.TryParse(valorNetoStr, out double valorNeto))
                {
                    if (estado.Trim().Equals("Estudio", StringComparison.OrdinalIgnoreCase))
                    {
                        sumaValorNetoEstudio += valorNeto;
                    }
                    else if (estado.Trim().Equals("Aprobada", StringComparison.OrdinalIgnoreCase))
                    {
                        sumaValorNetoAprobada += valorNeto;
                    }
                }
            }

            // Asigna las sumas totales a los Labels correspondientes en la tabla
            lbTotEst.InnerText = sumaValorNetoEstudio.ToString("C");
            lbTotApr.InnerText = sumaValorNetoAprobada.ToString("C");

            double sumaTotal = sumaValorNetoEstudio + sumaValorNetoAprobada;

            // Asigna la suma total al Label correspondiente en la tabla
            Label10.InnerText = sumaTotal.ToString("C");

            // Calcula el porcentaje de lbTotEst respecto al valor total
            double porcentajeEstudio = 0;
            if (sumaTotal != 0)
            {
                porcentajeEstudio = (sumaValorNetoEstudio / sumaTotal) * 100;
            }

            lbPorEstTot.InnerText = porcentajeEstudio.ToString("F1") + "%";

            double porcentajeAprobada = 0;
            if (sumaTotal != 0)
            {
                porcentajeAprobada = (sumaValorNetoAprobada / sumaTotal) * 100;
            }

            lbTotPor.InnerText = porcentajeAprobada.ToString("F1") + "%";

            double porcentajeTotal = porcentajeEstudio + porcentajeAprobada;

            lbPorTotal.InnerText = porcentajeTotal.ToString("F1") + "%";
        }
        public static double CalcularPorcentaje(double conteo, int cantidadFilas)
        {
            if (cantidadFilas != 0)
            {
                double porcentaje = ((double)conteo / cantidadFilas) * 100;
                return Math.Round(porcentaje, 2);
            }
            return 0; // Si cantidadFilas es 0, devolver 0 para evitar división por cero.
        }

        //TABLA POR ESTADO

        public void DataGridPorEstado_PreRender(object sender, EventArgs e)
        {
            int cantidadFilas = DataGrid2.Items.Count;

            Label2.InnerText = cantidadFilas.ToString();
            Label7.InnerText = cantidadFilas.ToString();

            Label6.InnerText = ("Totalizados").ToString();

            if (double.TryParse(Label7.InnerText, out double valorTotal) && valorTotal != 0 && cantidadFilas != 0)
            {
                // Calcular el porcentaje de Label4 respecto al valor total
                double porcentaje = (cantidadFilas / valorTotal) * 100;

                // Mostrar el porcentaje en el Label5 con solo 3 dígitos después del punto decimal
                Label3.InnerText = porcentaje.ToString("F1") + "%";
            }

          


            if (DataGrid2.Items.Count > 0)
            {

                DataGridItem primeraFila = DataGrid2.Items[0];


                string asesorComercial = primeraFila.Cells[0].Text;


                lbAseCom.InnerText = asesorComercial;
            }

            double sumaValorNeto = 0;
            foreach (DataGridItem item in DataGrid2.Items)
            {

                string valorNetoStr = item.Cells[5].Text;
                if (double.TryParse(valorNetoStr, out double valorNeto))
                {
                    sumaValorNeto += valorNeto;
                }
            }


            Label4.InnerText = sumaValorNeto.ToString();
            Label9.InnerText = Label4.InnerText;

            if (double.TryParse(Label9.InnerText, out double valorTotal2) && valorTotal2 != 0 && sumaValorNeto != 0)
            {
                // Calcular el porcentaje de Label4 respecto al valor total
                double porcentaje = (sumaValorNeto / valorTotal2) * 100;

                // Mostrar el porcentaje en el Label5 con solo 3 dígitos después del punto decimal
                Label5.InnerText = porcentaje.ToString("F1") + "%";
            }




        }

        public void BtnExportarExcelPorEstado_Click(object sender, EventArgs e)
        {// Crear una nueva instancia de Excel
            var excelApp = new Excel.Application();

            // Crear un nuevo libro y hoja de Excel
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Excel.Worksheet)workbook.ActiveSheet;

            // Agregar el encabezado
            Excel.Range headerRange = worksheet.Range["A1:H1"];
            headerRange.Merge();
            headerRange.Value = "DUCON S.A.S";
            headerRange.Font.Size = 22;
            headerRange.Font.Bold = true;
            headerRange.Font.Name = "Times New Roman";
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            // Agregar el subtítulo
            Excel.Range subtitleRange = worksheet.Range["A2:H2"];
            subtitleRange.Merge();
            subtitleRange.Value = "Estadisticas de Cotizaciones";
            subtitleRange.Font.Size = 14;
            subtitleRange.Font.Bold = true;
            subtitleRange.Font.Name = "Times New Roman";
            subtitleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            subtitleRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            // Agregar datos del TextBox y texto "Intervalo de Fecha"
            Excel.Range textBoxRange = worksheet.Range["A3:D3"];
            textBoxRange.Merge();
            textBoxRange.Value = ((System.Web.UI.WebControls.TextBox)FindControl("TextAsesortab2")).Text;
            textBoxRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            textBoxRange.Font.Bold = true;
            textBoxRange.Font.Name = "Times New Roman";

            Excel.Range intervalRange = worksheet.Range["E3:H3"];
            intervalRange.Merge();
            intervalRange.Value = "Intervalo de Fecha";
            intervalRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            intervalRange.Font.Bold = true;
            intervalRange.Font.Name = "Times New Roman";

            // Agregar el texto "Estado:" y el valor seleccionado del DropDownList
            Excel.Range estadoLabelRange = worksheet.Range["A4:D4"];
            estadoLabelRange.Merge();
            estadoLabelRange.Value = "Estado:" + ((System.Web.UI.WebControls.DropDownList)FindControl("ddlEstadoCotizacion")).SelectedItem.Text;
            estadoLabelRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            estadoLabelRange.Font.Bold = true;
            estadoLabelRange.Font.Name = "Times New Roman";


            // Agregar los textos y valores de fecha
            Excel.Range fechaInicioLabelRange = worksheet.Range["E4:H4"];
            fechaInicioLabelRange.Merge();
            fechaInicioLabelRange.Value = "Fecha Inicial:" + ((System.Web.UI.WebControls.TextBox)FindControl("TextCotizacionEntreInicio2")).Text +
                "Fecha Final:" + ((System.Web.UI.WebControls.TextBox)FindControl("TextCotizacionEntreFinal2")).Text; 
            fechaInicioLabelRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            fechaInicioLabelRange.Font.Bold = true;
            fechaInicioLabelRange.Font.Name = "Times New Roman";


            int rowIndex = 7; // Comenzar a escribir la tabla a partir de la fila 6

            // Escribir el encabezado de la tabla
            worksheet.Cells[rowIndex - 1, 1] = "Asesor Comercial";
            worksheet.Cells[rowIndex - 1, 2] = "Cantidad";
            worksheet.Cells[rowIndex - 1, 3] = "%";
            worksheet.Cells[rowIndex - 1, 4] = "Valor";
            worksheet.Cells[rowIndex - 1, 5] = "%";


            // Obtener los datos de la tabla desde los Label en el aspx.cs
            string asesor = lbAseCom.InnerText;
            string cantidad = Label2.InnerText;
            string porcentaje1 = Label3.InnerText;
            string valor = Label4.InnerText;
            string porcentaje2 = Label5.InnerText;


            string totalizados = Label6.InnerText;
            string cantidad2 = Label2.InnerText;
            string valor2 = Label4.InnerText;


            Excel.Range headerRow = worksheet.Range[worksheet.Cells[rowIndex - 1, 1], worksheet.Cells[rowIndex - 1, 5]];
            headerRow.Value = new object[] { "Asesor Comercial", "Cantidad", "%", "Valor", "%" };
            headerRow.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            headerRow.Font.Bold = true;
            headerRow.Font.Name = "Times New Roman";


            // Escribir los datos de la tabla en la hoja de Excel
            worksheet.Cells[rowIndex, 1] = asesor;
            worksheet.Cells[rowIndex, 2] = cantidad;
            worksheet.Cells[rowIndex, 3] = porcentaje1;
            worksheet.Cells[rowIndex, 4] = valor;
            worksheet.Cells[rowIndex, 5] = porcentaje2;

            Excel.Range dataRange = worksheet.Range[worksheet.Cells[rowIndex, 1], worksheet.Cells[rowIndex, 5]];
            dataRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

            rowIndex++;

            worksheet.Cells[rowIndex, 1] = totalizados;
            worksheet.Cells[rowIndex, 2] = cantidad2;
            worksheet.Cells[rowIndex, 4] = valor2;


            Excel.Range totalRange = worksheet.Range[worksheet.Cells[rowIndex, 1], worksheet.Cells[rowIndex, 5]];
            totalRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            totalRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            totalRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            totalRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            totalRange.Font.Bold = true;
            totalRange.Font.Name = "Times New Roman";


            int rowIndexx = 11; // Comenzar a escribir la tabla a partir de la fila 6

            // Escribir el encabezado de la tabla
            int colIndex = 1; // Columna 1 en Excel
            foreach (DataGridColumn column in DataGrid2.Columns)
            {
                if (colIndex == 1 || colIndex == 2 || colIndex == 11 || colIndex == 6 || colIndex == 10 || colIndex == 7) // Añadido colIndex == 12 para la columna Fecha_Cotización
                {
                    // Escribe el valor del encabezado en la hoja de Excel
                    worksheet.Cells[rowIndexx - 1, colIndex] = column.HeaderText;

                    // Formatea el encabezado con una letra más negra y que resalte
                   
                }

                colIndex++;
            }

            foreach (DataGridItem item in DataGrid2.Items)
            {
                colIndex = 1; // Comenzar en la columna 1 de Excel
                foreach (TableCell cell in item.Cells)
                {
                    if (colIndex == 1 || colIndex == 2 || colIndex == 11 || colIndex == 6 || colIndex == 10 || colIndex == 7)
                    {
                        // Verificar si el valor de la celda es igual a "&nbsp;"
                        if (cell.Text != "&nbsp;")
                        {
                            // Escribe el valor de la celda en la hoja de Excel
                            worksheet.Cells[rowIndexx, colIndex] = cell.Text;
                        }
                    }

                    colIndex++;
                }
                rowIndexx++;
            }

            worksheet.Columns.AutoFit();
            // Mostrar la aplicación de Excel
            excelApp.Visible = true;

        }

        //BOTONES ULTIMO CONTACTO

        protected void bntConsultarUlCont_Click(object sender, EventArgs e)
        {
            ConsultarDatos();
            BtnGraSeg.Enabled = true;
            BtnGraSeg.CssClass = "button-disabled";
        }

        protected void btnExportarUlCont_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia de Excel
            var excelApp = new Application();

            // Crear un nuevo libro y hoja de Excel
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Worksheet)workbook.ActiveSheet;

            // Puedes agregar datos en la hoja aquí si lo deseas
            // Ejemplo: worksheet.Cells[1, 1] = "Ejemplo";

            // Mostrar la aplicación de Excel
            excelApp.Visible = true;
        }

        protected void btnUCCMUlCont_Click(object sender, EventArgs e)
        {
           
            
        }

        protected void ConsultarDatos()
        {
            DateTime fechaConsulta;
            string formatoFecha = "dd/MM/yyyy"; // O el formato que corresponda al control TextUltContComer

            if (DateTime.TryParseExact(TextUltContComer.Text, formatoFecha, null, System.Globalization.DateTimeStyles.None, out fechaConsulta))
            {
                // La conversión fue exitosa, ahora puedes usar fechaConsulta
                // Asignar la fecha como valor del parámetro "uccFecha" en el SqlDataSource
                DataGridUltimoContacto.SelectParameters["uccFecha"].DefaultValue = fechaConsulta.ToString();

                // Vincular y mostrar los datos en el DataGrid
                DataGrid4.DataBind();

                UpdatePanelUltimoContacto.Update();
            }
            else
            {
                // La conversión falló, muestra un mensaje de error o toma alguna acción en consecuencia
                // Por ejemplo: Response.Write("Fecha inválida");
            }
        }

        protected void DataGrid4_RowCommand(object sender, DataGridCommandEventArgs e)
        {
            // Si tienes algún otro botón o control dentro del DataGrid y deseas realizar acciones específicas al hacer clic en ellos, puedes manejar esos eventos aquí.
            // Puedes identificar el botón o control que fue clickeado usando e.CommandName y e.CommandArgument.
            // Por ejemplo:
            if (e.CommandName == "NombreDelComando")
            {
                // Lógica para el botón o control con el comando "NombreDelComando" y el argumento e.CommandArgument
            }
        }

        protected void LinkButton_Click(object sender, EventArgs e)
        {
            // Crear una nueva instancia de Excel
            var excelApp = new Application();

            // Crear un nuevo libro y hoja de Excel
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Worksheet)workbook.ActiveSheet;

            // Establecer el formato para el encabezado
            Range headerRange1 = worksheet.Range["A1:L1"]; // Rango desde la celda 1A hasta 1L
            headerRange1.Merge(); // Combinar celdas
            headerRange1.Font.Size = 22; // Tamaño de letra 22
            headerRange1.Font.Bold = true; // Tipo de letra en negrita
            headerRange1.HorizontalAlignment = XlHAlign.xlHAlignCenter; // Centrar el texto horizontalmente

            // Agregar el texto en dos líneas
            headerRange1.Value = "Ducon LTDA\nEstadistica Cotizaciones"; // Texto del encabezado con salto de línea (\n)
            headerRange1.WrapText = true; // Activar el ajuste de texto automático para que las líneas se muestren correctamente

            // Establecer bordes al encabezado
            headerRange1.Borders.LineStyle = XlLineStyle.xlContinuous;
            headerRange1.Borders.Weight = XlBorderWeight.xlThin;




            // Obtener el contenido de los TextBox
            string asesorComercial = TextAsesor.Text;
            string startDate = TextBoxStartDate.Text;
            string endDate = TextBoxEndDate.Text;

            // Escribir los datos de los TextBox en la hoja de Excel
            worksheet.Cells[2, 1] = "Asesor Comercial:";
            worksheet.Cells[2, 2] = asesorComercial;
            worksheet.Cells[2, 7] = "Fecha de inicio:" + startDate;         
            worksheet.Cells[3, 7] = "Fecha de fin:" + endDate;
           

            // Obtener los datos de la tabla y escribirlos en la hoja de Excel
            int rowIndex = 6; // Comenzar a escribir la tabla a partir de la fila 6

            // Escribir el encabezado de la tabla
            worksheet.Cells[rowIndex - 1, 5] = "Estado";
            worksheet.Cells[rowIndex - 1, 6] = "Cant";
            worksheet.Cells[rowIndex - 1, 7] = "%";
            worksheet.Cells[rowIndex - 1, 8] = "Total Valor Neto";
            worksheet.Cells[rowIndex - 1, 9] = "%";

            // Obtener los datos de la tabla desde los Label en el aspx.cs
            string estudio = lbEstudio.InnerText;
            string cantEstudio = lbCantEstudio.InnerText;
            string porEstudio = lbPorEst.InnerText;
            string totEstudio = lbTotEst.InnerText;
            string porTotEstudio = lbPorEstTot.InnerText;

            string aprobada = lbAprobada.InnerText;
            string cantAprobada = lbCantApr.InnerText;
            string porAprobada = lbPorApr.InnerText;
            string totAprobada = lbTotApr.InnerText;
            string porTotAprobada = lbTotPor.InnerText;

            string totales = lbTotales.InnerText;
            string cantidadFilas = lbCantidad.InnerText;
            string totalNetoEstudio = lbTotEst.InnerText;
            string totalNetoAprobada = lbTotApr.InnerText;
            string totalNeto = Label10.InnerText;
            string porcentajeTotal = lbPorTotal.InnerText;

            // Escribir los datos de la tabla en la hoja de Excel
            worksheet.Cells[rowIndex, 5] = estudio;
            worksheet.Cells[rowIndex, 6] = cantEstudio;
            worksheet.Cells[rowIndex, 7] = porEstudio;
            worksheet.Cells[rowIndex, 8] = totEstudio;
            worksheet.Cells[rowIndex, 9] = porTotEstudio;

            rowIndex++;

            worksheet.Cells[rowIndex, 5] = aprobada;
            worksheet.Cells[rowIndex, 6] = cantAprobada;
            worksheet.Cells[rowIndex, 7] = porAprobada;
            worksheet.Cells[rowIndex, 8] = totAprobada;
            worksheet.Cells[rowIndex, 9] = porTotAprobada;

            rowIndex++;

            worksheet.Cells[rowIndex, 5] = totales;
            worksheet.Cells[rowIndex, 6] = cantidadFilas;
            worksheet.Cells[rowIndex, 7] = porcentajeTotal;
            worksheet.Cells[rowIndex, 8] = totalNeto;
            worksheet.Cells[rowIndex, 9] = porcentajeTotal;


            int rowIndexx = 11; // Comenzar a escribir la tabla a partir de la fila 6

            // Escribir el encabezado de la tabla
            int colIndex = 1; // Columna 1 en Excel
             foreach (DataGridColumn column in DataGrid1.Columns)
               {
              if (colIndex == 1 || colIndex == 2 || colIndex == 12 || colIndex == 4 || colIndex == 6 || colIndex == 7 || colIndex == 10)
        {
            // Escribe el valor del encabezado en la hoja de Excel
            worksheet.Cells[rowIndexx - 1, colIndex] = column.HeaderText;

            // Formatea el encabezado con una letra más negra y que resalte
            Range headerRange = worksheet.Cells[rowIndexx - 1, colIndex];
            headerRange.Font.Bold = true;
            headerRange.Font.Color = System.Drawing.Color.Black;
        }

        colIndex++;
         }

            foreach (DataGridItem item in DataGrid1.Items)
            {
                colIndex = 1; // Comenzar en la columna 1 de Excel
                foreach (TableCell cell in item.Cells)
                {
                    if (colIndex == 1 || colIndex == 2 || colIndex == 12 || colIndex == 4 || colIndex == 6 || colIndex == 7 || colIndex == 10)
                    {
                        // Verificar si el valor de la celda es igual a "&nbsp;"
                        if (cell.Text != "&nbsp;")
                        {
                            // Escribe el valor de la celda en la hoja de Excel
                            worksheet.Cells[rowIndexx, colIndex] = cell.Text;
                        }
                    }

                    colIndex++;
                }
                rowIndexx++;
            }


            // Ajustar el ancho de las columnas para que los datos se muestren correctamente
            worksheet.Columns.AutoFit();

            Range totalRow = worksheet.Rows[8];
            totalRow.Font.Bold = true;

            // Mostrar la aplicación de Excel
            excelApp.Visible = true;

            Range headerRow = worksheet.Rows[5];
            headerRow.Font.Bold = true;
           

            // Establecer un estilo de fuente para los datos de la tabla
            Range dataRows = worksheet.Range["A8:E" + (rowIndex - 1)];
            dataRows.Font.Color = System.Drawing.Color.Black;

            // Establecer un estilo de bordes para toda la tabla
            Range tableRange = worksheet.Range["E5:I" + (rowIndex - 0)];
            tableRange.Borders.LineStyle = XlLineStyle.xlContinuous;
            tableRange.Borders.Weight = XlBorderWeight.xlThin;

            // Establecer el formato de número para columnas específicas (por ejemplo, las columnas con valores numéricos)
            Range numericColumns = worksheet.Range["C:C"];
            numericColumns.NumberFormat = "0.00%";

            // Ajustar el ancho de las columnas para que los datos se muestren correctamente
            worksheet.Columns.AutoFit();
        }

        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid3.Items[rowIndex];

            Session["SelectCotizacion"] = selectedRow.Cells[4].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid3.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-row";

            BtnGraSeg.Enabled = true;
            BtnGraSeg.CssClass = "button-enabled btn-outline-dark btn btn-light text-center";



            ConsultaBD();

        }

        protected void ConsultaBD()
        {
            try
            {
                string cotizacion = Session["SelectCotizacion"] as string;

                // Consulta SQL para llenar DataGrid6 con los datos correspondientes
                string consulta = "SELECT * FROM tblSeguimientoCotizacion WHERE Cotización = @Cotizacion ORDER BY Fecha_Seguimiento DESC";

                // Utilizar un SqlConnection y un SqlCommand para ejecutar la consulta
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(consulta, connection))
                    {
                        // Agregar parámetro para @Cotizacion
                        cmd.Parameters.AddWithValue("@Cotizacion", cotizacion);

                        // Crear un SqlDataAdapter para obtener los datos y llenar un DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        adapter.Fill(dt);

                        // Asignar el DataTable al DataGrid6
                        DataGrid6.DataSource = dt;
                        DataGrid6.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción: puedes mostrar un mensaje, registrar el error, etc.
                // Aquí, solo se imprime el mensaje de la excepción
                Response.Write("Error: " + ex.Message);
            }
        }

        protected void btnGraSeg_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener la Cotización de la sesión
                string cotizacion = Session["SelectCotizacion"] as string;

                // Tu cadena de conexión a la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                // Consulta SQL para la inserción
                string consulta = "INSERT INTO tblSeguimientoCotizacion (Cotización, Fecha_Seguimiento, Observacion) VALUES (@Cotizacion, GETDATE(), @Observacion)";

                // Crear conexión y comando SQL
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(consulta, connection))
                    {
                        // Agregar parámetros
                        command.Parameters.AddWithValue("@Cotizacion", cotizacion);
                        command.Parameters.Add("@Observacion", SqlDbType.VarChar).Value = string.IsNullOrEmpty(TextDesSeg.Value) ? (object)DBNull.Value : TextDesSeg.Value;

                        // Abrir la conexión y ejecutar el comando
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                // Limpiar el campo de texto Observacion después de la inserción
                TextDesSeg.Value = string.Empty;
                ConsultaBD(); // Llama a la función para actualizar la consulta en tu página
            }
            catch (Exception ex)
            {
                // Manejo de la excepción: puedes mostrar un mensaje, registrar el error, etc.
                // Aquí, solo se imprime el mensaje de la excepción
                Response.Write("Error: " + ex.Message);
            }
        }

        protected void Consultar_Click(object sender, EventArgs e)
        {
            DataGrid3.DataSource = DataGridSeguimiento;
            DataGrid3.DataBind();
            BtnGraSeg.Enabled = false;
            BtnGraSeg.CssClass = "button-disabled btn-outline-dark btn btn-light text-center";
            DataGrid6.DataBind();
        }
    }


    }
    
