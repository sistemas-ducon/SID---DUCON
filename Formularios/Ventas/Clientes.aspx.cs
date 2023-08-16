using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;



namespace SISTEMA_INTEGRAL_DUCON.Formularios.Ventas
{
    public partial class Clientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ConsultarClienteFecha(object sender, EventArgs e)
        {
         

        }

        protected void CrearExcel(object sender, EventArgs e)
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
                var tableTitle = "Clientes Nuevos (" +FechaI.Text+") ("+FechaF.Text+")";
                var titleRange = worksheet.Range["B1", "G1"];
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

               



                int rowIndexx = 3;


                // Escribir el encabezado de la tabla
                int colIndex = 1; // Columna 1 en Excel
                foreach (DataGridColumn column in DataGridClienteFecha.Columns)
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

                foreach (DataGridItem item in DataGridClienteFecha.Items)
                {
                    colIndex = 1; // Comenzar en la columna 1 de Excel
                    foreach (TableCell cell in item.Cells)
                    {
                        // Excluir la primera columna (LinkButton)
                        if (colIndex != 1)
                        {

                            // Verificar si el valor de la celda es igual a "&nbsp;"
                            if (cell.Text != "&nbsp;")
                            {
                                // Escribe el valor de la celda en la hoja de Excel
                                worksheet.Cells[rowIndexx, colIndex] = cell.Text;
                            }

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

              

                worksheet.Columns.AutoFit();
                // Mostrar la aplicación de Excel
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar a Excel: " + ex.Message);
            }

        }
    }
}