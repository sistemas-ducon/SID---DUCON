using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Consultas
{
    public partial class OT_ManualesSid : System.Web.UI.Page
    {

        private string CadenaConexionSID = "BD_SIDSQL"; // se declara una variable privada  llamada BD_SIDSQL es una cadena de conexion a la base de datos llamada BD_SIDSQL
        private string CadenaConexionSSF = "BD_SSF";

        protected int contador = 1; //esta variable protegida llamada contador de tipo int  para contar elementos 
        protected int PendienteDespachoCount = 1;
        protected int PendienteEmpaqueCount = 1;


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DataGridBusDis_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //cuando se están renderizando filas de datos en el DataGrid.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // string fechaTerminadaEmpaqueObj = el valor obtenido se asigna a la variable   fechaTerminadaEmpaqueObj, que es una cadena que representa la fecha de terminación de empaque para la fila actual del DataGrid
                // este metodo Eval de Databinder recupera el valor de una propiedad o columna llamada  "Fecha_Terminada_Empaque"  de la fila de datos actual (DataItem) enlazada al elemento Item del DataGrid.
                // ?.ToString  es para que permita datos nulos  si el valor es devuelto por DataBinder.Eval() es nulo, esto se evaluara como nula  y no se llamara al metodo ToString y si no fuera nulo se llamara a ToString
                string fechaTerminadaEmpaqueObj = DataBinder.Eval(e.Item.DataItem, "Fecha_Terminada_Empaque")?.ToString();
                // el valor de una propiedad o columna llamada "Real_Despacho" de la fila de datos actual enlazada al elemento Item del DataGrid.
                string realDespachoObj = DataBinder.Eval(e.Item.DataItem, "Real_Despacho")?.ToString();


                // !string.IsNullOrEmpty(fechaTerminadaEmpaqueObj) && !string.IsNullOrEmpty(realDespachoObj) esta condiccion verifica si tanto  fechaTerminadaEmpaqueObj como realDespachoObj no son nulos ni cadenas vacías.
                //si algunas de las variables son nulas o una cadena vacia la condicion sera falsa   el bloque de codigo  interno no se ejecutara  
                if (!string.IsNullOrEmpty(fechaTerminadaEmpaqueObj) && !string.IsNullOrEmpty(realDespachoObj))
                {
                    // este bloque de codigo intenta convertir  las cadenas  fechaTerminadaEmpaqueObj y realDespachoObj en objetos DateTime, que representan horas y fechas y horas 
                    // a función DateTime.TryParse intenta analizar la cadena como una fecha y, si tiene éxito, asigna el valor analizado al parámetro de salida fechaTerminadaEmpaque y realDespacho, r
                    if (DateTime.TryParse(fechaTerminadaEmpaqueObj, out DateTime fechaTerminadaEmpaque) &&
                        DateTime.TryParse(realDespachoObj, out DateTime realDespacho))

                    {
                        // este bloque se ejecutara cuando la fecha terminada empaque es menor  a la fecha real despacho real menos un dia
                        // si es menor resta un dia a la fecha realDespacho  y se ejecutara  el bloque de codigo interno 
                        if (fechaTerminadaEmpaque < realDespacho.AddDays(-1))
                        {
                            // Aplicar estilos a las celdas [TableCell] individuales  se selecciona  la celda  utilizando su indice en el elemento Cells[index] en este caso la celdas
                            // 7,8,9 y 10 que corresponde al datagrid
                            TableCell cellEmp = e.Item.Cells[7];
                            TableCell cellEmpaque = e.Item.Cells[8];
                            TableCell cellDesp = e.Item.Cells[9];
                            TableCell cellDespacho = e.Item.Cells[10];

                            // antes de poner el color a las celdas  verificas si las variables de las celdas  (cellEmp, cellEmpaque, cellDesp, cellDespacho)  son nulas para poder pintar  las celdas de color rojo 

                            // System.Drawing.ColorTranslator.FromHtml("#F50303"): Este método convierte una cadena hexadecimal de color rojo
                            // cellEmp =  a las celda 7 
                            if (cellEmp != null)
                                // System.Drawing.ColorTranslator.FromHtml("#F50303"): Este método convierte una cadena hexadecimal de color rojo 
                                cellEmp.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                            // cellEmp =  a las celda 8 
                            if (cellEmpaque != null)
                                cellEmpaque.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                            // cellEmp =  a las celda 9
                            if (cellDesp != null)
                                cellDesp.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                            // cellEmp =  a las celda 10
                            if (cellDespacho != null)
                                cellDespacho.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                        }

                        else
                        {
                            PendienteEmpaqueCount++;

                        }

                    }
                }
                else
                {
                    // si  Terminada_Empaque = True
                    // Aplicar clase CSS red-cell a toda la fila

                    e.Item.Cells[7].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000"); // Rojo
                    e.Item.Cells[8].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000"); // Rojo
                    e.Item.Cells[7].ForeColor = System.Drawing.ColorTranslator.FromHtml("#F3E8E8"); //Blanco
                    e.Item.Cells[8].ForeColor = System.Drawing.ColorTranslator.FromHtml("#F3E8E8"); //Blanco

                }


                string TerminaDespachoOT = DataBinder.Eval(e.Item.DataItem, "Terminada_Despacho")?.ToString();
                string fechaTerminadaDespachoOT = DataBinder.Eval(e.Item.DataItem, "Fecha_Terminada_Despacho")?.ToString();

                // se comprueba si ambas  cadenas no son nulas ni vacias si cualquiera de los dos valores es nulos se salta al else 
                if (!string.IsNullOrEmpty(TerminaDespachoOT) && !string.IsNullOrEmpty(fechaTerminadaDespachoOT))
                {
                    //intenta parsearse  se intenta convertir las cadenas a objetos "Datatime", DateTime.TryParse,
                    //devuelve true si la conversión es exitosa y asigna las fechas convertidas a Termina_Despacho y Fecha_Terminada_Despacho. 
                    if (DateTime.TryParse(TerminaDespachoOT, out DateTime Termina_Despacho) &&
                   DateTime.TryParse(TerminaDespachoOT, out DateTime Fecha_Terminada_Despacho))
                    {
                        //compara las fechas  Si Fecha_Terminada_Despacho es mayor que Termina_Despacho, se aplican estilos a ciertas celdas de la fila actual
                        if (Fecha_Terminada_Despacho > Termina_Despacho)
                        {
                            // Aplicar estilos a las celdas individuales
                            TableCell cellEmp = e.Item.Cells[7];
                            TableCell cellEmpaque = e.Item.Cells[8];
                            TableCell cellDesp = e.Item.Cells[9];
                            TableCell cellDespacho = e.Item.Cells[10];

                            if (cellEmp != null)
                                cellEmp.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                            if (cellEmpaque != null)
                                cellEmpaque.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                            if (cellDesp != null)
                                cellDesp.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                            if (cellDespacho != null)
                                cellDespacho.BackColor = System.Drawing.ColorTranslator.FromHtml("#F50303");
                        }
                        else
                        {
                            PendienteDespachoCount++;


                        }


                    }
                }
                else
                {
                    // No Terminada_Empaque = True
                    // Aplicar clase CSS red-cell a toda la fila
                    // si algiuno de los valores no es nulo o vacio se  pinta de estos colores
                    e.Item.Cells[9].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000"); // Rojo
                    e.Item.Cells[10].BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000"); // Rojo
                    e.Item.Cells[9].ForeColor = System.Drawing.ColorTranslator.FromHtml("#F3E8E8"); // Blanco
                    e.Item.Cells[10].ForeColor = System.Drawing.ColorTranslator.FromHtml("#F3E8E8"); //Blanco
                }

                TableCell cell1 = e.Item.Cells[1];
                cell1.Text = contador.ToString();
                // Incrementa el contador para la próxima fila
                contador++;

            }

        }






        protected void Mstrar(object sender, EventArgs e)
        {
            //tbfechaIni.Text: Texto del control de entrada de fecha inicial.
            string fechaInicio = tbfechaIni.Text;
            // tbfechaFinal.Text: Texto del control de entrada de fecha final.
            string fechaFin = tbfechaFinal.Text;

            //System.Configuration.ConfigurationManager.ConnectionStrings: Obtiene las cadenas de conexión configuradas en Web.config.
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            //using: Garantiza que el SqlConnection se cierre y se libere correctamente después de su uso,
            //qlConnection: Representa una conexión abierta a la base de datos.
            //connection.Open(): Abre la conexión a la base de datos.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                //query: Cadena que contiene la consulta SQL.
                //INNER JOIN: Une dos tablas tblOT y tblPlano donde las filas cumplen con la condición especificada.
                //BETWEEN @FechaInicial AND @FechaFinal: Filtra registros entre las fechas proporcionadas.
                //AND tblOT.Aplica_Empaque = 1: Filtra registros donde Aplica_Empaque es igual a 1.
                //ORDER BY: Ordena los resultados por Fecha_Real_Despacho_Produccion y Fecha_Instalacion.

                {
                    connection.Open();
                    string query = @"SELECT tblOT.*, tblOT.Fecha_Real_Despacho_Produccion AS Real_Despacho, tblPlano.Plano
                                 FROM tblOT
                                 INNER JOIN tblPlano ON tblOT.Id_OT = tblPlano.ID_OT
                                 WHERE tblOT.Fecha_Real_Despacho_Produccion BETWEEN @FechaInicial AND @FechaFinal
                                 AND tblOT.Aplica_Empaque = 1
                                 AND tblOT.Consecutivo_Pedido = tblPlano.Consecutivo_Pedido
                                 ORDER BY tblOT.Fecha_Real_Despacho_Produccion, tblOT.Fecha_Instalacion ASC";

                    //SqlCommand: Representa una instrucción SQL que se ejecutará en la base de datos.
                    // Parameters.AddWithValue: Añade los parámetros @FechaInicial y @FechaFinal a la consulta.
                    //SqlDataAdapter: Utiliza el SqlCommand para llenar un DataTable con los resultados de la consulta.
                    //DataTable: Estructura en memoria que almacena datos tabulares

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FechaInicial", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFin);

                    SqlDataAdapter Data = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    Data.Fill(dataTable);

                    //DataSource: Establece la fuente de datos del DataGrid
                    //DataBind(): Enlaza los datos del DataTable al DataGrid.
                    DataModulo.DataSource = dataTable;
                    DataModulo.DataBind();



                    // Llamar a llenarData2 para procesar y mostrar datos en DataGridDetalleSolicitud
                    llenarData2(dataTable);

                }

            }


        }



        protected void ExportE(object sender, EventArgs e)
        {
            // Aquí se crea una nueva instancia de ExcelPackage, que es la clase principal en EPPlus para manejar archivos de Excel.
            using (var excelPackage = new ExcelPackage())
            {
                // Agregar una hoja de trabajo al paquete, Se agrega una nueva hoja de trabajo al paquete de Excel con el nombre "Detalle_OT".
                var worksheet = excelPackage.Workbook.Worksheets.Add("Detalle_OT");

                // Agregar título a la primera tabla, Se configura un título para la hoja que se coloca en la celda A1, abarcando desde A1 hasta K1. 
                string tableTitle = "Detalle OT";
                worksheet.Cells["A1:K1"].Merge = true;
                worksheet.Cells["A1"].Value = tableTitle;
                worksheet.Cells["A1"].Style.Font.Size = 16;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1"].Style.Font.Color.SetColor(Color.Black);


                // Se establece la fila de inicio (fila 3) y la columna de inicio (columna 1) para escribir los datos de la tabla.
                // maxColumnIndex define el máximo índice de columna a considerar (columna 12).
                int rowIndex = 3; // Comenzar a escribir la tabla a partir de la fila 3
                // Escribir el encabezado de la primera tabla
                int colIndex = 1; //columna 1 en excel 
                int maxColumnIndex = 12;

                //se repite sobre todas las columnas del datagrid llamado datamodulo
                foreach (DataGridColumn column in DataModulo.Columns)
                {
                    // aqui se verifica si la columna actual no es la primera (colIndex != 1) y si el índice de la columna es menor o igual al índice máximo
                    // de columna permitido (maxColumnIndex)   Esto significa que se omite la primera columna y se procesan solo las columnas hasta maxColumnIndex
                    if (colIndex != 1 && colIndex <= maxColumnIndex)
                    {
                        // escribe el valor del encabezado en la hoja de exel
                        // Esta línea asigna el valor del encabezado de la columna actual (column.HeaderText)
                        // a la celda correspondiente en la hoja de Excel. rowIndex - 1 indica la fila para los encabezados
                        // (una fila antes de rowIndex), y colIndex - 1 indica la columna.
                        worksheet.Cells[rowIndex - 1, colIndex - 1].Value = column.HeaderText;


                        //estilo del encabezado , Aquí se define un rango que incluye solo la celda del encabezado actual.
                        using (var range = worksheet.Cells[rowIndex - 1, colIndex - 1, rowIndex - 1, colIndex - 1])
                        {
                            //Negrita: true,

                            range.Style.Font.Bold = true;
                            //Fondo Gris Claro:
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            //Se aplica un borde delgado alrededor de la celda del encabezado.
                            range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }
                    }
                    //crementa el índice de columna (colIndex++) para pasar a la siguiente columna en la próxima iteración del bucle.
                    colIndex++;
                }
                foreach (DataGridItem item in DataModulo.Items)
                {
                    colIndex = 1; // comenzar en la columna 1 del excel
                    foreach (TableCell cell in item.Cells)
                    {
                        // : Solo se escriben celdas si colIndex != 1 (omite la primera columna) y colIndex <= maxColumnIndex
                        if (colIndex != 1 && colIndex <= maxColumnIndex)
                        {
                            // Reemplazar "&nbsp;" con un valor vacío, Se reemplaza &nbsp; por un valor vacío.
                            string cellValue = cell.Text.Replace("&nbsp;", string.Empty);

                            // Escribe el valor de la celda en la hoja de Excel, Se escribe el valor de la celda en la hoja de Excel.
                            worksheet.Cells[rowIndex, colIndex - 1].Value = cellValue;

                            // Estilo para las celdas de datos  
                            using (var range = worksheet.Cells[rowIndex, colIndex - 1])
                            {
                                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }
                        }
                        colIndex++;
                    }
                    rowIndex++;

                }

                //Título: Se escribe el título "Calculo" en la celda A de la fila actual.
                // Agregar título a la segunda tabla 
                string table2Title = "Calculo";
                worksheet.Cells["A" + rowIndex.ToString()].Value = table2Title;
                // Unión de Celdas: Se combinan las celdas de A a G en la fila actual.
                worksheet.Cells["A" + rowIndex.ToString() + ":G" + rowIndex.ToString()].Merge = true;
                //Estilo del Título: Se establece el tamaño de fuente, negrita y alineación horizontal centrada.
                worksheet.Cells["A" + rowIndex.ToString()].Style.Font.Size = 14;
                worksheet.Cells["A" + rowIndex.ToString()].Style.Font.Bold = true;
                //Estilo del Título: Se establece el tamaño de fuente, negrita y alineación horizontal centrada.
                worksheet.Cells["A" + rowIndex.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex++; // Incrementar la fila para empezar a escribir los encabezados de la segunda tabla 

                colIndex = 0; // reset column index for the second table

                // Se reoite  sobre cada columna en DataGridDetalleSolicitud.Columns.
                foreach (DataGridColumn column in DataGridDetalleSolicitud.Columns)
                {
                    // Solo se escriben encabezados si colIndex >= 0 y colIndex < maxColumnIndex.
                    if (colIndex >= 0 && colIndex < maxColumnIndex)
                    {

                        //Escritura del Encabezado: Se escribe el encabezado de la columna en la hoja de Excel.
                        worksheet.Cells[rowIndex, colIndex + 1].Value = column.HeaderText;
                        //Estilo del Encabezado: Se aplica negrita, fondo gris claro y borde delgado.
                        worksheet.Cells[rowIndex, colIndex + 1].Style.Font.Bold = true;
                        worksheet.Cells[rowIndex, colIndex + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowIndex, colIndex + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        //Incremento del Índice de Columna y Fila: Se incrementan los índices de columna y fila según corresponda.
                        worksheet.Cells[rowIndex, colIndex + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    colIndex++;
                }

                rowIndex++; // Increment row index for data rows

                // Escribir los datos de la segunda tabla,, Iteración de Filas: Se itera sobre cada DataGridItem en DataGridDetalleSolicitud.Items.
                foreach (DataGridItem item in DataGridDetalleSolicitud.Items)
                {
                    colIndex = 0; // comenzar en la columna 0 del excel

                    // Iteración de Celdas: Dentro de cada fila, se itera sobre cada TableCell.
                    foreach (TableCell cell in item.Cells)


                    {
                        if (colIndex >= 0 && colIndex < maxColumnIndex)


                        {
                            //Se reemplaza &nbsp; por un valor vacío.
                            string cellValue = cell.Text.Replace("&nbsp;", string.Empty);
                            worksheet.Cells[rowIndex, colIndex + 1].Value = cellValue;

                            // Estilo para las celdas de datos
                            using (var range = worksheet.Cells[rowIndex, colIndex + 1])
                            {
                                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }
                        }
                        colIndex++;
                    }
                    rowIndex++;
                }






                // Ajustar el ancho de las columnas
                worksheet.Cells.AutoFitColumns();

                // Guardar el archivo de Excel
                string filePath = Path.GetTempFileName() + ".xlsx";
                FileInfo excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);

                // Descargar el archivo de Excel
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=Manuales_OT.xlsx");
                Response.TransmitFile(filePath);
                Response.End();


            }





        }



        protected void DataModulo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "color")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem item = DataModulo.Items[rowIndex];

                if (item.BackColor == System.Drawing.Color.Black)
                {
                    // La fila ya está seleccionada, deseleccionarla
                    item.BackColor = System.Drawing.Color.White;
                    // Restaurar el color del texto a negro
                    foreach (TableCell cell in item.Cells)
                    {
                        cell.ForeColor = System.Drawing.Color.Black;
                    }
                }
                else
                {
                    // La fila no está seleccionada, seleccionarla
                    item.BackColor = System.Drawing.Color.Black;
                    // Cambiar el color del texto de todas las celdas a blanco
                    foreach (TableCell cell in item.Cells)
                    {
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }

        }




        private void llenarData2(DataTable datos)
        {


            if (datos.Rows.Count > 0)
            {

                // Crear una nueva DataTable para los datos procesados
                DataTable tabla = new DataTable();
                tabla.Columns.Add("TOTALES", typeof(string));
                tabla.Columns.Add("EMPAQUE", typeof(string));  // O el tipo de datos adecuado
                tabla.Columns.Add("%E", typeof(string));
                tabla.Columns.Add("DESPACHO", typeof(string)); // O el tipo de datos adecuado
                tabla.Columns.Add("%D", typeof(string));


                // Realizar cálculos sobre los datos
                //contadores 
                int totalPedidos = 0;
                int totalOkDespacho = 0;
                int CumplimientoDespacho = 0;
                int totalOkEmpaque = 0;
                int CumplimientoEmpaque = 0;
                int PendienteEmpaque = 0;
                int PendienteDespacho = 0;




                foreach (DataRow row in datos.Rows)
                {

                    bool Terminada_Empaque = Convert.ToBoolean(row["Terminada_Empaque"].ToString());
                    DateTime? fechaTerminadaEmpaque = row["Fecha_Terminada_Empaque"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["Fecha_Terminada_Empaque"]) : null;
                    DateTime Real_Despacho = Convert.ToDateTime(row["Real_Despacho"].ToString());
                    DateTime? Fecha_Terminada_Despacho = row["Fecha_Terminada_Despacho"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["Fecha_Terminada_Despacho"]) : null;

                    totalPedidos++;


                    if (Terminada_Empaque == true)
                    {
                        totalOkEmpaque = totalOkEmpaque + 1;

                        if (fechaTerminadaEmpaque > Real_Despacho  /* - dias de cumplimiento*/)
                        {
                            //se pintaba columnas

                        }
                        else
                        {
                            CumplimientoEmpaque = CumplimientoEmpaque + 1;
                        };

                    }
                    else
                    {
                        PendienteEmpaque = PendienteEmpaque + 1;
                    };



                    bool Terminada_Despacho = Convert.ToBoolean(row["Terminada_Despacho"].ToString());

                    if (Terminada_Despacho == true)
                    {
                        totalOkDespacho = totalOkDespacho + 1;

                        // no se si debo volver a convertir el real_despacho
                        if (Fecha_Terminada_Despacho > Real_Despacho)
                        {
                            //pintaba
                        }
                        else
                        {
                            CumplimientoDespacho = CumplimientoDespacho + 1;
                        }
                    }
                    else
                    {
                        PendienteDespacho = PendienteDespacho + 1;
                    }



                }
                string Porcentaje_cumplir_empaque = ((CumplimientoEmpaque * 100) / totalPedidos).ToString();
                string Porcentaje_cumplir_Despacho = ((CumplimientoDespacho * 100) / totalPedidos).ToString();


                // Crear una nueva fila en la tabla
                DataRow fila1 = tabla.NewRow();
                fila1["TOTALES"] = "PEDIDOS";
                fila1["EMPAQUE"] = totalPedidos;
                fila1["%E"] = totalPedidos;
                fila1["DESPACHO"] = "";
                fila1["%D"] = "";

                //agregar  la fila a la tabla 
                tabla.Rows.Add(fila1);

                // Repetir el proceso para agregar más filas con otros datos
                DataRow fila2 = tabla.NewRow();
                fila2["TOTALES"] = "PENDIENTES";
                fila2["EMPAQUE"] = PendienteEmpaque;
                fila2["%E"] = "";
                fila2["DESPACHO"] = PendienteDespacho;
                fila2["%D"] = "";

                //agregar  la fila a la tabla 
                tabla.Rows.Add(fila2);


                DataRow fila3 = tabla.NewRow();
                fila3["TOTALES"] = "OK";
                fila3["EMPAQUE"] = totalOkEmpaque;
                fila3["%E"] = "";
                fila3["DESPACHO"] = totalOkDespacho;
                fila3["%D"] = "";


                tabla.Rows.Add(fila3);

                DataRow fila4 = tabla.NewRow();
                fila4["TOTALES"] = "CUMPLIMIENTO";
                fila4["EMPAQUE"] = CumplimientoEmpaque;
                fila4["%E"] = Porcentaje_cumplir_empaque;
                fila4["DESPACHO"] = CumplimientoDespacho;
                fila4["%D"] = Porcentaje_cumplir_Despacho;


                tabla.Rows.Add(fila4);



                DataGridDetalleSolicitud.DataSource = tabla;
                DataGridDetalleSolicitud.DataBind();





            }



        }

    }
}