using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class OTsPendientes : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";

        private string CadenaConexionISID = "BD_ISIDSQL";

        private string CadenaConexionSSF = "BD_SSF";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                if (!IsPostBack)
                {
                    CargarDatosPorDefecto();

                    RadioButton18.Checked = true;

                    Button2.Enabled = false;
                    Button2.CssClass = "form-control-sm btn-sm btn btn-outline-dark button-disabled";

                    DateTime fechaActual = DateTime.Now;
                    DateTime fechaMenosUnMesUnDia = fechaActual.AddDays(-7);
                    TextBox3.Text = fechaMenosUnMesUnDia.ToString("yyyy-MM-dd");
                    TextBox1.Text = fechaActual.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }

           
          
        }

        private void CargarDatosPorDefecto()
        {
            // Nombre de la conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_OTsPendientesVPD", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        DataGrid1.DataSource = dataSet;
                        DataGrid1.DataBind();
                    }
                }
            }

            if (DataGrid1.Items.Count == 0)
            {
                NoResultsLabel.Visible = true;
            }
            else
            {
                NoResultsLabel.Visible = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string nombreObra = TextDir.Text.Trim();
            string codigoAsesor = TextBox2.Text.Trim();
            

            string fechaDesde = TextBox3.Text;
            string fechaHasta = TextBox1.Text;
            string campoFecha = DropDownList1.SelectedValue;

            bool radioButton18Marcado = RadioButton18.Checked;
            bool radioButton2Marcado = RadioButton2.Checked;

            // Nombre de la conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_ObtenerDatosConFiltros", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@NombreObra", SqlDbType.NVarChar, 255).Value = string.IsNullOrEmpty(nombreObra) ? (object)DBNull.Value : nombreObra;
                    command.Parameters.Add("@CodigoAsesor", SqlDbType.NVarChar, 255).Value = string.IsNullOrEmpty(codigoAsesor) ? (object)DBNull.Value : codigoAsesor;
                    command.Parameters.Add("@TerminadoVentas", SqlDbType.Bit).Value = radioButton18Marcado ? false : radioButton2Marcado ? true : (object)DBNull.Value;
                    command.Parameters.Add("@FechaDesde", SqlDbType.Date).Value = string.IsNullOrEmpty(fechaDesde) ? (object)DBNull.Value : Convert.ToDateTime(fechaDesde);
                    command.Parameters.Add("@FechaHasta", SqlDbType.Date).Value = string.IsNullOrEmpty(fechaHasta) ? (object)DBNull.Value : Convert.ToDateTime(fechaHasta);
                    command.Parameters.Add("@CampoFecha", SqlDbType.NVarChar, 255).Value = string.IsNullOrEmpty(campoFecha) ? (object)DBNull.Value : campoFecha;

                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        DataGrid1.DataSource = dataSet;
                        DataGrid1.DataBind();
                    }
                }
            }

            if (DataGrid1.Items.Count == 0)
            {
                NoResultsLabel.Visible = true;
            }
            else
            {
                NoResultsLabel.Visible = false;
            }
        }


        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid1.Items[rowIndex];

            // Captura los valores de las columnas Id_OT y Consecutivo_Pedido
            string idOT = selectedRow.Cells[1].Text; // Índice 1 para la columna "Id_OT"
            string consecutivoPedido = selectedRow.Cells[2].Text; // Índice 2 para la columna "Consecutivo_Pedido"

            // Asigna los valores a variables de sesión
            Session["Id_OT2"] = idOT;
            Session["pedido2"] = consecutivoPedido;

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

            string mensajePersonalizado = "Cargar la OT selecciona";
            string urlRedireccion = "OrdenTrabajo.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            // Creamos un nuevo libro de Excel
            ExcelPackage excelPackage = new ExcelPackage();

            // Agregamos una nueva hoja al libro
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Programación Planta");

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["A1"].Value = "DUCON S.A.S - PROGRAMACIÓN DE OBRAS";

            // Combinar celdas para el encabezado
            ExcelRange headerRange = worksheet.Cells["A1:R1"];
            headerRange.Merge = true;

            // Establecer el estilo del encabezado
            headerRange.Style.Font.Name = "Times New Roman";
            headerRange.Style.Font.Size = 14;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["A2"].Value = "Obras filtradas: " + ((System.Web.UI.WebControls.DropDownList)FindControl("DropDownList1")).SelectedItem.Text;

            // Combinar celdas para el encabezado
            ExcelRange headerRange5 = worksheet.Cells["A2:I2"];
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
            worksheet.Cells["J2"].Value = " Del " + ((System.Web.UI.WebControls.TextBox)FindControl("TextBox3")).Text +
            " Al " + ((System.Web.UI.WebControls.TextBox)FindControl("TextBox1")).Text;

            // Combinar celdas para el encabezado
            ExcelRange headerRange6 = worksheet.Cells["J2:R2"];
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

            // Establecer el valor del encabezado en la celda A1
            worksheet.Cells["A3"].Value = "Obras";

            // Combinar celdas para el encabezado
            ExcelRange headerRange2 = worksheet.Cells["A3:R3"];
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


         
    System.Web.UI.WebControls.DataGrid dataGrid = DataGrid1;
            int rowCount = dataGrid.Items.Count;
            int colCount = dataGrid.Columns.Count;

            // Llenar el archivo de Excel con los datos del DataGrid
            // Mostrar encabezados en negrita
            for (int j = 0; j < colCount; j++)
            {
                worksheet.Cells[5, j + 1].Value = dataGrid.Columns[j].HeaderText;
                worksheet.Cells[5, j + 1].Style.Font.Bold = true;
            }

            // Llenar el archivo de Excel con los datos del DataGrid
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    TableCell cell = dataGrid.Items[i].Cells[j];
                    // Verificar si el valor de la celda es nulo y escribirlo en el archivo Excel
                    if (cell.Text != null && cell.Text != "")
                    {
                        worksheet.Cells[i + 6, j + 1].Value = cell.Text;
                    }
                    else
                    {
                        worksheet.Cells[i + 6, j + 1].Value = ""; // Si el valor es nulo, escribir una celda vacía
                    }
                }
            }

            // Establecer borde alrededor del contenido
            ExcelRange range = worksheet.Cells[5, 2, rowCount + 5, colCount];
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;

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
    }
}