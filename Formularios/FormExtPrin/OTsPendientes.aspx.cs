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
            // Crear una nueva instancia de Excel
            var excelApp = new Application();

            // Crear un nuevo libro y hoja de Excel
            var workbook = excelApp.Workbooks.Add();
            var worksheet = (Worksheet)workbook.ActiveSheet;

            // Establecer un encabezado para el archivo Excel
            Range headerRange = worksheet.Range["A1:L1"]; // Rango desde la celda A1 hasta L1
            headerRange.Merge(); // Combinar celdas
            headerRange.Font.Size = 22; // Tamaño de letra 22
            headerRange.Font.Bold = true; // Tipo de letra en negrita
            headerRange.HorizontalAlignment = XlHAlign.xlHAlignCenter; // Centrar el texto horizontalmente

            // Agregar el texto en dos líneas
            headerRange.Value = "Obra"; // Texto del encabezado con salto de línea (\n)
            headerRange.WrapText = true; // Activar el ajuste de texto automático para que las líneas se muestren correctamente

            // Establecer bordes al encabezado
            headerRange.Borders.LineStyle = XlLineStyle.xlContinuous;
            headerRange.Borders.Weight = XlBorderWeight.xlThin;

            // Ajustar el ancho de las columnas para que los datos se muestren correctamente
            worksheet.Columns.AutoFit();

            int rowIndex = 3; // Comenzar a escribir la tabla a partir de la fila 3

            // Escribir los encabezados de las columnas
            int colIndex = 1;
            foreach (DataGridColumn column in DataGrid1.Columns)
            {
                worksheet.Cells[rowIndex - 1, colIndex] = column.HeaderText;

                // Establecer el formato y estilo del encabezado de la columna
                Range headerCell = worksheet.Cells[rowIndex - 1, colIndex];
                headerCell.Font.Bold = true;
                headerCell.Interior.Color = Color.LightGray; // Color de fondo del encabezado

                colIndex++;
            }

            // Obtener los datos de la tabla en el DataGrid y escribirlos en la hoja de Excel
            foreach (DataGridItem item in DataGrid1.Items)
            {
                colIndex = 1; // Comenzar en la columna 1 de Excel

                foreach (TableCell cell in item.Cells)
                {
                    // Verificar si el valor de la celda es igual a "&nbsp;"
                    if (cell.Text != "&nbsp;")
                    {
                        // Escribe el valor de la celda en la hoja de Excel
                        worksheet.Cells[rowIndex, colIndex] = cell.Text;
                    }

                    colIndex++;
                }

                rowIndex++;
            }

            // Mostrar la aplicación de Excel
            excelApp.Visible = true;
        }



    }
}