using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
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

        protected void Correo_Click(object sender, EventArgs e)
        {
            string destinatario = "practicantesistemas@ducon.com.co, practicantesistemas2@ducon.com.co, auxiliarsistemas@ducon.com.co, harleyvidal@ducon.com.co";
            string asunto = "Prueba envio correo";
            string mensaje = "Prueba exitosa de envio de correo.";

            EnviarCorreo(destinatario, asunto, mensaje);

        }

        protected void EnviarCorreo(string destinatario, string asunto, string mensaje)
        {
            // Configuración del servidor SMTP de Office 365
            var smtpServer = "smtp-mail.outlook.com";
            var smtpPort = 587; // Puerto para TLS/STARTTLS

            // Dirección de correo electrónico y contraseña para autenticación
            var email = "sid@ducon.com.co"; // cualquier correo de office funciona 
            var password = "SisDuc123.";    // contraseña del usuario 

            // Configurar el cliente SMTP
            var client = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true, // Habilitar SSL/TLS
            };

            // Crear el mensaje de correo electrónico
            var mail = new MailMessage(email, destinatario)
            {
                Subject = asunto,
                Body = mensaje,
                IsBodyHtml = false // Establecer a true si el cuerpo del mensaje es HTML 
                // se puede modificar para que el cuerpo del documento contenga etiquetas html 
            };

            try
            {
                // Enviar el correo electrónico
                client.Send(mail);
                // Puedes agregar lógica adicional aquí después de enviar el correo electrónico
            }
            catch (Exception ex)
            {
                error.Text = ex.Message;
            }
        }


    }
}