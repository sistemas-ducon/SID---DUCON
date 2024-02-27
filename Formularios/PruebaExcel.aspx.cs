using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class PruebaExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PruebaExcel_Click(object sender, EventArgs e)
        {
            // Creamos un nuevo libro de Excel
            ExcelPackage excelPackage = new ExcelPackage();

            // Agregamos una nueva hoja al libro
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Datos");

            // Obtener el DataGrid y sus datos
            System.Web.UI.WebControls.DataGrid dataGrid = DataGridPruebaExcel; // Cambiar "DataGridPruebaExcel" por el ID real de tu DataGrid
            int rowCount = dataGrid.Items.Count;
            int colCount = dataGrid.Columns.Count;

            // Llenar el archivo de Excel con los datos del DataGrid
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    TableCell cell = dataGrid.Items[i].Cells[j];
                    worksheet.Cells[i + 1, j + 1].Value = cell.Text;

                    // Agregar estilo a las celdas
                    if (i == 0) // Estilo para las celdas del encabezado
                    {
                        worksheet.Cells[i + 1, j + 1].Style.Font.Bold = true;
                        worksheet.Cells[i + 1, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[i + 1, j + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }
                }
            }

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
    }
}