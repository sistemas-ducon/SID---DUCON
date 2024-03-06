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

                   
                    DateTime fechaMenosUnMes4dias = fechaActual.AddMonths(-1).AddDays(-4);
                    TextCotizacionEntreInicio2.Text = fechaMenosUnMes4dias.ToString("yyyy-MM-dd");
                    TextCotizacionEntreFinal2.Text = fechaActual.ToString("yyyy-MM-dd");

                    DateTime fechaMenosTresMes = fechaActual.AddMonths(-3);
                    IdDateInicial.Text = fechaMenosTresMes.ToString("yyyy-MM-dd");
                    IdDateFinal.Text = fechaActual.ToString("yyyy-MM-dd");


                    TextUltContComer.Text = fechaActual.ToString("yyyy-MM-dd");


                    BtnExcelCot.Enabled = false;
                    BtnExcelCot.CssClass = "btn shadow btn-light linkButtonClicked button-disabled grande";

                    BtnPDFCot.Enabled = false;
                    BtnPDFCot.CssClass = "btn shadow btn-light linkButtonClicked button-disabled grande";
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
        {
            // Creamos un nuevo libro de Excel
            ExcelPackage excelPackage = new ExcelPackage();

            // Agregamos una nueva hoja al libro
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Datos");

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["A1"].Value = "DUCON S.A.S";

            // Combinar celdas para el encabezado
            ExcelRange headerRange = worksheet.Cells["A1:F1"];
            headerRange.Merge = true;

            // Establecer el estilo del encabezado
            headerRange.Style.Font.Name = "Times New Roman";
            headerRange.Style.Font.Size = 22;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["A2"].Value = "Estadisticas de Cotizaciones";

            // Combinar celdas para el encabezado
            ExcelRange headerRange2 = worksheet.Cells["A2:F2"];
            headerRange2.Merge = true;

            // Establecer el estilo del encabezado
            headerRange2.Style.Font.Name = "Times New Roman";
            headerRange2.Style.Font.Size = 14;
            headerRange2.Style.Font.Bold = true;
            headerRange2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange2.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            headerRange2.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange2.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange2.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["A3"].Value = lbAseCom.InnerText;

            // Combinar celdas para el encabezado
            ExcelRange headerRange3 = worksheet.Cells["A3:C3"];
            headerRange3.Merge = true;

            // Establecer el estilo del encabezado
            headerRange3.Style.Font.Name = "Times New Roman";
            headerRange3.Style.Font.Size = 16;
            headerRange3.Style.Font.Bold = true;
            headerRange3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange3.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            headerRange3.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange3.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange3.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange3.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["D3"].Value = "Intervalo de Fecha";

            // Combinar celdas para el encabezado
            ExcelRange headerRange4 = worksheet.Cells["D3:F3"];
            headerRange4.Merge = true;

            // Establecer el estilo del encabezado
            headerRange4.Style.Font.Name = "Times New Roman";
            headerRange4.Style.Font.Size = 16;
            headerRange4.Style.Font.Bold = true;
            headerRange4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange4.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            headerRange4.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange4.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange4.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange4.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["A4"].Value = "Estado:" + ((System.Web.UI.WebControls.DropDownList)FindControl("ddlEstadoCotizacion")).SelectedItem.Text;

            // Combinar celdas para el encabezado
            ExcelRange headerRange5 = worksheet.Cells["A4:C4"];
            headerRange5.Merge = true;

            // Establecer el estilo del encabezado
            headerRange5.Style.Font.Name = "Times New Roman";
            headerRange5.Style.Font.Size = 16;
            headerRange5.Style.Font.Bold = true;
            headerRange5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange5.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            headerRange5.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange5.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange5.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange5.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["D4"].Value = "Fecha Inicial: " + ((System.Web.UI.WebControls.TextBox)FindControl("TextCotizacionEntreInicio2")).Text +
            " Fecha Final: " + ((System.Web.UI.WebControls.TextBox)FindControl("TextCotizacionEntreFinal2")).Text;

            // Combinar celdas para el encabezado
            ExcelRange headerRange6 = worksheet.Cells["D4:F4"];
            headerRange6.Merge = true;

            // Establecer el estilo del encabezado
            headerRange6.Style.Font.Name = "Times New Roman";
            headerRange6.Style.Font.Size = 10;
            headerRange6.Style.Font.Bold = true;
            headerRange6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Establece el estilo de borde para el rango de datos
            headerRange6.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange6.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange6.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange6.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Comenzar a escribir la tabla a partir de la fila 6
            int rowIndex = 7;

            // Escribir el encabezado de la tabla
            worksheet.Cells[rowIndex - 1, 1].Value = "Asesor Comercial";
            worksheet.Cells[rowIndex - 1, 2].Value = "Cantidad";
            worksheet.Cells[rowIndex - 1, 3].Value = "%";
            worksheet.Cells[rowIndex - 1, 4].Value = "Valor";
            worksheet.Cells[rowIndex - 1, 5].Value = "%";

            // Establecer el estilo del encabezado
            using (ExcelRange headerRange7 = worksheet.Cells["A6:E6"])
            {
                headerRange7.Style.Font.Bold = true;
                headerRange7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange7.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                headerRange7.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange7.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                headerRange7.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                headerRange7.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                headerRange7.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                headerRange7.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            // Obtener los datos de la tabla desde los Label en el aspx.cs
            string asesor = lbAseCom.InnerText;
            string cantidad = Label2.InnerText;
            string porcentaje1 = Label3.InnerText;
            string valor = Label4.InnerText;
            string porcentaje2 = Label5.InnerText;

            string totalizados = Label6.InnerText;
            string cantidad2 = Label2.InnerText;
            string valor2 = Label4.InnerText;

            // Escribir los datos de la tabla en la hoja de Excel
            worksheet.Cells[rowIndex, 1].Value = asesor;
            worksheet.Cells[rowIndex, 2].Value = cantidad;
            worksheet.Cells[rowIndex, 3].Value = porcentaje1;
            worksheet.Cells[rowIndex, 4].Value = valor;
            worksheet.Cells[rowIndex, 5].Value = porcentaje2;

            using (ExcelRange headerRange9 = worksheet.Cells["A7:E7"])
            {      
                headerRange9.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                headerRange9.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                headerRange9.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                headerRange9.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                headerRange9.Style.Font.Name = "Times New Roman";
            }

            rowIndex++;

            worksheet.Cells[rowIndex, 1].Value = totalizados;
            worksheet.Cells[rowIndex, 2].Value = cantidad2;
            worksheet.Cells[rowIndex, 4].Value = valor2;

            // Establecer el estilo del encabezado
            using (ExcelRange headerRange8 = worksheet.Cells["A8:E8"])
            {
                headerRange8.Style.Font.Bold = true;
                headerRange8.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                headerRange8.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                headerRange8.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                headerRange8.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                headerRange8.Style.Font.Name = "Times New Roman";
            }


            // Escribir el encabezado en el rango A11:H11
            worksheet.Cells["A10"].Value = "Asesor Comercial";
            worksheet.Cells["B10"].Value = "Cotizacion";
            worksheet.Cells["C10"].Value = "Fecha C.";
            worksheet.Cells["D10"].Value = "Valor Neto";
            worksheet.Cells["E10"].Value = "Plano";
            worksheet.Cells["F10"].Value = "Cliente"; 

            // Establecer el estilo del encabezado
            using (ExcelRange headerRange11 = worksheet.Cells["A10:F10"])
            {
                headerRange11.Style.Font.Bold = true;
                headerRange11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange11.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                headerRange11.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange11.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                headerRange11.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            }


            rowIndex = 11; // Comenzar desde la fila 12
            foreach (DataGridItem item in DataGrid2.Items)
            {
                int colIndex = 1; // Comenzar desde la columna 1
                foreach (TableCell cell in item.Cells)
                {
                    if (colIndex == 1 || colIndex == 2)
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                        }
                    }
                    else if (colIndex == 11) 
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            worksheet.Cells[rowIndex, 3].Value = cell.Text; 
                        }
                    }

                    else if (colIndex == 4) 
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            worksheet.Cells[rowIndex, 4].Value = cell.Text; 
                        }
                    }
                    else if (colIndex == 10) 
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            worksheet.Cells[rowIndex, 5].Value = cell.Text;
                        }
                    }
                    else if (colIndex == 7)
                    {
                        if (cell.Text != "&nbsp;")
                        {
                            // Obtener el valor de la columna 8
                            string valorColumna8 = item.Cells[8].Text;

                            // Concatenar los valores de las columnas 8 y 6 con un guion "-" en medio
                            string resultado = valorColumna8 + " - " + cell.Text;

                            // Mostrar el resultado en la celda 6
                            worksheet.Cells[rowIndex, 6].Value = resultado;
                        }
                    }



                    colIndex++;
                }
                rowIndex++;
            }

            // Especifica el rango de datos para aplicar bordes
            ExcelRange dataGridRange10 = worksheet.Cells[10, 1, rowIndex - 1, 6];

            // Establece el estilo de borde para el rango de datos del DataGrid
            dataGridRange10.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataGridRange10.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataGridRange10.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataGridRange10.Style.Border.Right.Style = ExcelBorderStyle.Thin;


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


        //POR SEGUIMIENTO

        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid3.Items[rowIndex];

            Session["SelectCotizacion"] = selectedRow.Cells[4].Text;

            Session["FechaCot"] = selectedRow.Cells[5].Text;

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

            BtnExcelCot.Enabled = true;
            BtnExcelCot.CssClass = "btn shadow btn-light linkButtonClicked2 button-enabled grande";

            BtnPDFCot.Enabled = true;
            BtnPDFCot.CssClass = "btn shadow btn-light linkButtonClicked2 button-enabled grande";

            ConsultaBD();

        }

        protected void DescargarCotizacionExcel_Click(object sender, EventArgs e)
        {
            // Verificar si la variable de sesión SelectCotizacion está presente y tiene un valor asignado
            if (Session["SelectCotizacion"] != null && Session["ZonaLogeada"] != null && Session["FechaCot"] != null)
            {
                string nombreArchivo = Session["SelectCotizacion"].ToString(); // Obtener el nombre del archivo de la variable de sesión
                string zonaLogeada = Session["ZonaLogeada"].ToString(); // Obtener la zona logeada de la variable de sesión
                DateTime fechaCot = DateTime.Parse(Session["FechaCot"].ToString()); // Obtener la fecha de cotización de la variable de sesión

                string year = fechaCot.ToString("yyyy");

                // Mapear los números de mes a sus respectivos nombres abreviados
                            Dictionary<int, string> mesesAbreviados = new Dictionary<int, string>
                    {
                        { 1, "Ene" },
                        { 2, "Feb" },
                        { 3, "Mar" },
                        { 4, "Abr" },
                        { 5, "May" },
                        { 6, "Jun" },
                        { 7, "Jul" },
                        { 8, "Ago" },
                        { 9, "Sep" },
                        { 10, "Oct" },
                        { 11, "Nov" },
                        { 12, "Dic" }
                    };

                string mesAbreviado = mesesAbreviados[fechaCot.Month]; // Obtener el nombre abreviado del mes

                // Construir la ruta completa al archivo
                string rutaArchivo = @"\\172.16.30.6\Recepcion\Cotizaciones Excel\" + zonaLogeada + @"\" + year + @"\" + mesAbreviado + @"\" + nombreArchivo + ".xls";


                if (File.Exists(rutaArchivo))
                {
                    // Establecer las cabeceras para la descarga del archivo
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(rutaArchivo));
                    Response.AppendHeader("X-Content-Type-Options", "nosniff");
                    Response.AppendHeader("X-Frame-Options", "SAMEORIGIN");
                    Response.AppendHeader("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

                    // Escribir el archivo al flujo de respuesta
                    Response.WriteFile(rutaArchivo);

                    // Enviar todos los encabezados al cliente antes de finalizar la respuesta
                    Response.Flush();
                    // Finalizar la respuesta
                    Response.End();
                }
                else
                {
                    string mensajePersonalizado = "El archivo seleccionado no existe";
                    string urlRedireccion = "Ventas/Consulta_Cotizacion.aspx"; // Cambia esto por la URL correcta
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
            else
            {
                // Si la variable de sesión SelectCotizacion, ZonaLogeada o FechaCot no están presentes o no tienen un valor asignado, redireccionar o manejar según sea necesario
                string mensajePersonalizado = "No se selecciono la cotizacion";
                string urlRedireccion = "Ventas/Consulta_Cotizacion.aspx"; // Cambia esto por la URL correcta
                Response.Redirect($"~/Formularios/ErrorMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
        }
        protected void DescargarPDF_Click(object sender, EventArgs e)
        {
            // Verificar si la variable de sesión SelectCotizacion está presente y tiene un valor asignado
            if (Session["SelectCotizacion"] != null && Session["ZonaLogeada"] != null && Session["FechaCot"] != null)
            {
                string nombreArchivo = Session["SelectCotizacion"].ToString(); // Obtener el nombre del archivo de la variable de sesión
                string zonaLogeada = Session["ZonaLogeada"].ToString(); // Obtener la zona logeada de la variable de sesión
                DateTime fechaCot = DateTime.Parse(Session["FechaCot"].ToString()); // Obtener la fecha de cotización de la variable de sesión

                string year = fechaCot.ToString("yyyy");

                // Mapear los números de mes a sus respectivos nombres abreviados
                Dictionary<int, string> mesesAbreviados = new Dictionary<int, string>
                    {
                        { 1, "Ene" },
                        { 2, "Feb" },
                        { 3, "Mar" },
                        { 4, "Abr" },
                        { 5, "May" },
                        { 6, "Jun" },
                        { 7, "Jul" },
                        { 8, "Ago" },
                        { 9, "Sep" },
                        { 10, "Oct" },
                        { 11, "Nov" },
                        { 12, "Dic" }
                    };

                string mesAbreviado = mesesAbreviados[fechaCot.Month]; // Obtener el nombre abreviado del mes

                // Construir la ruta completa al archivo
                string rutaArchivo = @"\\172.16.30.6\Recepcion\Arcexcel\" + zonaLogeada + @"\" + year + @"\" + mesAbreviado + @"\" + nombreArchivo + ".pdf";


                if (File.Exists(rutaArchivo))
                {
                    // Establecer las cabeceras para la descarga del archivo
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(rutaArchivo));
                    Response.AppendHeader("X-Content-Type-Options", "nosniff");
                    Response.AppendHeader("X-Frame-Options", "SAMEORIGIN");
                    Response.AppendHeader("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

                    // Escribir el archivo al flujo de respuesta
                    Response.WriteFile(rutaArchivo);

                    // Enviar todos los encabezados al cliente antes de finalizar la respuesta
                    Response.Flush();
                    // Finalizar la respuesta
                    Response.End();
                }
                else
                {
                    string mensajePersonalizado = "El archivo seleccionado no existe";
                    string urlRedireccion = "Ventas/DocumentacionDise.aspx"; // Cambia esto por la URL correcta
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
            else
            {
                // Si la variable de sesión SelectCotizacion, ZonaLogeada o FechaCot no están presentes o no tienen un valor asignado, redireccionar o manejar según sea necesario
                string mensajePersonalizado = "La variable de sesión SelectCotizacion, ZonaLogeada o FechaCot no están disponibles";
                string urlRedireccion = "Ventas/DocumentacionDise.aspx"; // Cambia esto por la URL correcta
                Response.Redirect($"~/Formularios/ErrorMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
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
    
