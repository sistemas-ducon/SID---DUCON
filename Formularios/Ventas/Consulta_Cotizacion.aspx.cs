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
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Drawing;
using System.IO;
using OfficeOpenXml.Style;
using OfficeOpenXml;


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
              
               

                if (Session["CedulaLogeada"] != null)
                {
                    ConsultarDatos();
                    DataGrid2.DataBind();
                    LoadEstados();

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

                    DateTime fechaActual = DateTime.Now;
                    DateTime fechaMenosUnMesUnDia = fechaActual.AddMonths(-1).AddDays(-1);          
                    TextBoxStartDate.Text = fechaMenosUnMesUnDia.ToString("yyyy-MM-dd");
                    TextBoxEndDate.Text = fechaActual.ToString("yyyy-MM-dd");
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

        //public void BtnExportarExcelPorEstado_Click(object sender, EventArgs e)
        //{  
        //    //// Creamos un nuevo libro de Excel
        //    //ExcelPackage excelPackage = new ExcelPackage();

        //    //// Agregamos una nueva hoja al libro
        //    //ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Resumen Estadisticas X Estado");

        //    //// Obtener el DataGrid y sus datos
        //    //System.Web.UI.WebControls.DataGrid dataGrid = DataGrid2; 
        //    //int rowCount = dataGrid.Items.Count;
        //    //int colCount = dataGrid.Columns.Count;

        //    //// Llenar el archivo de Excel con los datos del DataGrid
        //    //for (int i = 0; i < rowCount; i++)
        //    //{
        //    //    for (int j = 0; j < colCount; j++)
        //    //    {
        //    //        TableCell cell = dataGrid.Items[i].Cells[j];
        //    //        worksheet.Cells[i + 1, j + 1].Value = cell.Text;

        //    //        // Agregar estilo a las celdas
        //    //        if (i == 0) // Estilo para las celdas del encabezado
        //    //        {
        //    //            worksheet.Cells[i + 1, j + 1].Style.Font.Bold = true;
        //    //            worksheet.Cells[i + 1, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //    //            worksheet.Cells[i + 1, j + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        //    //        }
        //    //    }
        //    //}

        //    //// Guardar el archivo de Excel en una ubicación temporal
        //    //string filePath = Path.GetTempFileName() + ".xlsx";
        //    //FileStream fileStream = new FileStream(filePath, FileMode.Create);
        //    //excelPackage.SaveAs(fileStream);
        //    //fileStream.Close();

        //    //// Descargar el archivo de Excel
        //    //Response.Clear();
        //    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    //Response.AddHeader("content-disposition", "attachment; filename=Datos.xlsx");
        //    //Response.TransmitFile(filePath);
        //    //Response.End();

        //}

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
            // Creamos un nuevo libro de Excel
            ExcelPackage excelPackage = new ExcelPackage();

            // Agregamos una nueva hoja al libro
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Estadistica Por Vendedor");

            // Encabezado
            // Combinar celdas del rango "A1:L1"
            // Combinar celdas para el encabezado
            worksheet.Cells["A1:H1"].Merge = true;

            // Establecer el texto con un salto de línea
            var richText = worksheet.Cells["A1"].RichText.Add("Ducon LTDA \nEstadisticas Cotizaciones");
            worksheet.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Establecer el estilo del texto
            richText.Bold = true;
            richText.Size = 22;
            richText.FontName = "Times New Roman";

            // Ajustar la altura de la fila para mostrar el título completo
            worksheet.Row(1).Height = 70; // Ajusta la altura según sea necesario

            // Establecer el borde alrededor del rango combinado
            var border = worksheet.Cells["A1:H1"].Style.Border;

            // Establecer un borde más grueso
            border.BorderAround(ExcelBorderStyle.Thick);

            // Establecer el color del borde en negro
            border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thick;
            border.Top.Color.SetColor(Color.Black);
            border.Bottom.Color.SetColor(Color.Black);
            border.Left.Color.SetColor(Color.Black);
            border.Right.Color.SetColor(Color.Black);




            // Datos de los TextBox
            worksheet.Cells["A2"].Value = "Asesor Comercial:";
            worksheet.Cells["B2"].Value = TextAsesor.Text;
            worksheet.Cells["G2"].Value = "Fecha de inicio:" + TextBoxStartDate.Text;
            worksheet.Cells["G3"].Value = "Fecha de fin:" + TextBoxEndDate.Text;

            // Establecer el estilo de fuente para el contenido, excluyendo el encabezado
            using (var range = worksheet.Cells["A2:H3"])
            {
                range.Style.Font.Name = "Aptos Narrow";
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 16;
            }


            // Agregar encabezados de tabla
            int rowIndex = 5;
            foreach (DataGridColumn column in DataGrid1.Columns)
            {
                if (column.HeaderText != "&nbsp;" && (column.HeaderText == "Estado" || column.HeaderText == "Cant" || column.HeaderText == "%" || column.HeaderText == "Total Valor Neto" || column.HeaderText == "%"))
                {
                    worksheet.Cells[rowIndex, 5].Value = column.HeaderText;
                    worksheet.Cells[rowIndex, 5].Style.Font.Bold = true;
                    worksheet.Cells[rowIndex, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowIndex, 5].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }
            }

            // Obtener datos adicionales de las etiquetas HTML
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

            // Escribir datos adicionales en el archivo Excel
            worksheet.Cells[5, 2].Value = "Estado";
            worksheet.Cells[5, 3].Value = "Cant";
            worksheet.Cells[5, 4].Value = "%";
            worksheet.Cells[5, 5].Value = "Total Valor Neto";
            worksheet.Cells[5, 6].Value = "%";

            

            // Establecer el estilo del encabezado
            using (ExcelRange headerRange = worksheet.Cells["B5:F5"])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            }

            worksheet.Cells[6, 2].Value = estudio;
            worksheet.Cells[6, 3].Value = cantEstudio;
            worksheet.Cells[6, 4].Value = porEstudio;
            worksheet.Cells[6, 5].Value = totEstudio;
            worksheet.Cells[6, 6].Value = porTotEstudio;

            worksheet.Cells[7, 2].Value = aprobada;
            worksheet.Cells[7, 3].Value = cantAprobada;
            worksheet.Cells[7, 4].Value = porAprobada;
            worksheet.Cells[7, 5].Value = totAprobada;
            worksheet.Cells[7, 6].Value = porTotAprobada;

            worksheet.Cells[8, 2].Value = totales;
            worksheet.Cells[8, 3].Value = cantidadFilas;
            worksheet.Cells[8, 4].Value = porcentajeTotal;
            worksheet.Cells[8, 5].Value = totalNeto;
            worksheet.Cells[8, 6].Value = porcentajeTotal;


            // Especifica el rango de datos para aplicar bordes
            ExcelRange dataRange = worksheet.Cells[5, 2, 8, 6];

            // Establece el estilo de borde para el rango de datos
            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Escribir datos del DataGrid en el archivo Excel

            // Escribir el encabezado en el rango A11:H11
            worksheet.Cells["A11"].Value = "Cotizacion";
            worksheet.Cells["B11"].Value = "Estado";
            worksheet.Cells["C11"].Value = "Plano";
            worksheet.Cells["D11"].Value = "Valor";
            worksheet.Cells["E11"].Value = "Dto(%)";
            worksheet.Cells["F11"].Value = "Valor Neto";
            worksheet.Cells["G11"].Value = "Cliente";
            worksheet.Cells["H11"].Value = "F.Cot.";

            // Establecer el estilo del encabezado
            using (ExcelRange headerRange = worksheet.Cells["A11:H11"])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            }


            rowIndex = 12; // Comenzar desde la fila 12
            foreach (DataGridItem item in DataGrid1.Items)
            {
                int colIndex = 1; // Comenzar desde la columna 1
                foreach (TableCell cell in item.Cells)
                {
                    if (colIndex == 1 || colIndex == 2 || colIndex == 4 || colIndex == 6 || colIndex == 5 || colIndex == 7)
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                        }
                    }
                    else if (colIndex == 12) // Si es la columna 12, escribir en la columna C en lugar de 12
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            worksheet.Cells[rowIndex, 3].Value = cell.Text; // Escribir en la columna C
                        }
                    }

                    else if (colIndex == 10) // Si es la columna 12, escribir en la columna C en lugar de 12
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            worksheet.Cells[rowIndex, 8].Value = cell.Text; // Escribir en la columna R
                        }
                    }
                    colIndex++;
                }
                rowIndex++;
            }

            // Especifica el rango de datos para aplicar bordes
            ExcelRange dataGridRange = worksheet.Cells[11, 1, rowIndex - 1, 8];

            // Establece el estilo de borde para el rango de datos del DataGrid
            dataGridRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataGridRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataGridRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataGridRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;


            // Ajustar el ancho de las columnas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Guardar el archivo de Excel en una ubicación temporal
            string filePath = Path.GetTempFileName() + ".xlsx";
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            excelPackage.SaveAs(fileStream);
            fileStream.Close();

            // Descargar el archivo de Excel
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=Datos.xlsx");
            Response.TransmitFile(filePath);
            Response.End();
        }




        protected void EliminarDoc_Click(object sender, EventArgs e)
        {
            // Asumiendo que tienes una forma de obtener o generar el mismo nombre de archivo.
            // Si nombreArchivo se genera en base a algún input del usuario o una variable,
            // asegúrate de reconstruirlo de la misma manera aquí.
            string nombreArchivo = GenerarNombreArchivo(); // Asume que esta función genera el nombre del archivo basado en la lógica que ya tienes.

            string filePath = @"\\172.16.30.6\PruebaDocumentacion\COTIZACION\" + nombreArchivo;

            // Verifica si el archivo existe antes de intentar eliminarlo.
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                // Opcionalmente, puedes notificar al usuario que el archivo fue eliminado.
                // Por ejemplo: lblMensaje.Text = "Archivo eliminado con éxito.";
            }
            else
            {
                // Opcionalmente, notifica al usuario que el archivo no se encontró.
                // Por ejemplo: lblMensaje.Text = "El archivo no existe.";
            }
        }

        
        private string GenerarNombreArchivo()
        {
            // Aquí iría la lógica para generar el nombre del archivo, asegúrate de que
            // sea la misma lógica utilizada en LinkButton_Click para garantizar que
            // el nombre del archivo sea el correcto.
            // Ejemplo simple basado en tu código actual:
            string asesorComercial = TextAsesor.Text; // Asume que TextAsesor es accesible aquí.
            string nombreArchivo = asesorComercial + "CotVen" + ".xls";
            return nombreArchivo;
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
    
