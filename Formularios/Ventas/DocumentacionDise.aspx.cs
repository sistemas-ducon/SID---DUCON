using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Ventas
{
   

    public partial class DocumentacionDise : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                ConfigureSqlDataSource();

                ValidarBotonGuardarEliminar();
            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
        }

        protected void ValidarBotonGuardarEliminar()
        {
            string numDise = Session["NumeroDiseño"] != null ? Session["NumeroDiseño"].ToString() : "";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para verificar el campo ProgramadoVentas
                string consultaProgramadoVentas = "SELECT ProgramadoVentas FROM tbldiseño WHERE Numero_Diseño = @Numero_Diseño";

                using (SqlCommand commandProgramadoVentas = new SqlCommand(consultaProgramadoVentas, connection))
                {
                    commandProgramadoVentas.Parameters.AddWithValue("@Numero_Diseño", numDise);

                    bool programadoVentas = false; // Valor predeterminado

                    using (SqlDataReader readerProgramadoVentas = commandProgramadoVentas.ExecuteReader())
                    {
                        if (readerProgramadoVentas.Read())
                        {
                            programadoVentas = readerProgramadoVentas.GetBoolean(0);
                        }
                    }

                    // Validar ProgramadoVentas
                    if (programadoVentas)
                    {
                      
                        GuardarButton.Enabled = false;
                        GuardarButton.CssClass = "btn btn-sm button-disabled";
                        BtnEliminar.Enabled = false;
                        BtnEliminar.CssClass = "btn btn-sm button-disabled";

                    }
                    else
                    {
                      
                        GuardarButton.Enabled = true;
                        GuardarButton.CssClass = "btn btn-sm btn-outline-dark button-enabled";
                        BtnEliminar.Enabled = true;
                        BtnEliminar.CssClass = "btn btn-sm btn-outline-dark button-enabled";


                    }
                }
            }
        }

        private void ConfigureSqlDataSource()
        {
            string numDise2 = Session["NumeroDiseño"] != null ? Session["NumeroDiseño"].ToString() : "";


            string numDise = "DS" + numDise2; // Asegúrate de que lblNumDise esté disponible
            string query = $"SELECT Archivo, Observacion, Usuario, FechaRegistro, Id_OT FROM tblDocumentacion WHERE Id_OT = '{numDise}'";
            SqlDataSource3.SelectCommand = query;
        }

        protected void lnkSelectRow_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGridDocumento.Items[rowIndex];

            // Almacena el valor de Id_OT en una variable de sesión
            Session["SelectedIdOT"] = selectedRow.Cells[5].Text;

            // Almacena el nombre del archivo en la variable de sesión
            Session["SelectedFileName"] = selectedRow.Cells[1].Text;


            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGridDocumento.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            // Aplica la clase CSS a la fila seleccionada
            selectedRow.CssClass = "selected-row";

          



            // Puedes acceder a los datos de la fila si es necesario
            string archivo = selectedRow.Cells[1].Text;
            string observacion = selectedRow.Cells[2].Text;
            string usuario = selectedRow.Cells[3].Text;
            string fechaRegistro = selectedRow.Cells[4].Text;
            string id_OT = selectedRow.Cells[5].Text;
        }

        protected void lnkViewFile_Click(object sender, EventArgs e)
        {
            LinkButton lnkViewFile = (LinkButton)sender;
            int rowIndex = Convert.ToInt32(lnkViewFile.CommandArgument);
            DataGridItem selectedRow = DataGridDocumento.Items[rowIndex];
            string archivo = selectedRow.Cells[1].Text;
            string NombreCarpeta = selectedRow.Cells[5].Text;

            // Construye la ruta completa al archivo
            string rutaArchivo = @"\\Srvfs\s_i_ducon$\Documentacion Bitacora\" + NombreCarpeta + @"\" + archivo;

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

        protected void GuardarButton_Click(object sender, EventArgs e)
        {
            

                if (FileUpload1.HasFile)
                {
                    HttpPostedFile uploadedFile = FileUpload1.PostedFile;

                // Verificar el tamaño del archivo
                if (uploadedFile.ContentLength <= 10240) // 10 MB en bytes
                {

                    // Obtener el nombre del archivo
                    string fileName = Path.GetFileName(uploadedFile.FileName);

                    // Obtener el texto de lblNumDise
                    string lblText = Session["NumeroDiseño"] != null ? Session["NumeroDiseño"].ToString() : "";




                    // Concatenar "DS" con el texto de lblNumDise para obtener el nombre de la carpeta
                    string folderName = "DS" + lblText;

                    // Combinar la ruta de guardado con el nombre de la carpeta \\Srvfs\s_i_ducon$\Prueba Documentacion
                    string savePath = Path.Combine(@"\\Srvfs\s_i_ducon$\Documentacion Bitacora", folderName);

                    //string savePath = Path.Combine(@"\\SRVFS\PruebaDocumentacion", folderName);

                    try
                    {
                        // Verificar si la carpeta no existe y crearla si es necesario
                        if (!Directory.Exists(savePath))
                        {
                            Directory.CreateDirectory(savePath);
                        }

                    }
                    catch (IOException ex)
                    {

                    }


                    string filePath = Path.Combine(savePath, fileName);

                    // Guardar el archivo
                    uploadedFile.SaveAs(filePath);

                    InsertarEnBaseDeDatos(folderName, fileName);


                }
                else
                {
                    string mensajePersonalizado = "El archivo excede el limite de tamaño, por favor comprimalo e intentelo nuevamente";
                    string urlRedireccion = "Ventas/DocumentacionDise.aspx"; // Cambia esto por la URL correcta
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }

                }
                else
                {
                    string mensajePersonalizado = "Seleccione el archivo que desea Guardar";
                    string urlRedireccion = "Ventas/DocumentacionDise.aspx"; // Cambia esto por la URL correcta
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
           
        }

        private void InsertarEnBaseDeDatos(string folderName, string fileName)
        {
            // Establecer la conexión con la base de datos          
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                con.Open();

                // Crear la consulta SQL para insertar en la tabla 'tblDocumentacion'
                string query = "INSERT INTO tblDocumentacion (Id_OT, Pedido, Archivo, Observacion, TipoDocumento, Usuario, FechaRegistro) VALUES (@Id_OT, @Pedido, @Archivo, @Observacion, @TipoDocumento, @Usuario, @FechaRegistro)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Establecer los parámetros de la consulta
                    cmd.Parameters.AddWithValue("@Id_OT", folderName);
                    cmd.Parameters.AddWithValue("@Pedido", 0);
                    cmd.Parameters.AddWithValue("@Archivo", fileName);
                    cmd.Parameters.AddWithValue("@Observacion", TextArea1.Value); // Obtener el valor del textarea
                    cmd.Parameters.AddWithValue("@TipoDocumento", "BITACORA");
                    cmd.Parameters.AddWithValue("@Usuario", Session["usuariologueado"] != null ? Session["usuariologueado"].ToString() : "");
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                    // Ejecutar la consulta
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Comprobar si se actualizó al menos una fila
                    if (rowsAffected > 0)
                    {                                  
                            string mensajePersonalizado = "El documento se ha guardado exitosamente.";
                            string urlRedireccion = "Ventas/DocumentacionDise.aspx"; // Cambia esto por la URL correcta
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");                              
                    }
                    else
                    {
                        string mensajePersonalizado = "El archivo NO se ah guardado exitosamente";
                        string urlRedireccion = "Ventas/DocumentacionDise.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                    }
                }
            }
        }

        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            string lblText = Session["NumeroDiseño"] != null ? Session["NumeroDiseño"].ToString() : "";
            string idOTToDelete = Session["SelectedIdOT"] as string;

            if (!string.IsNullOrEmpty(idOTToDelete))
            {
                string archivoToDelete = Session["SelectedFileName"] as string;

                // Ruta completa del archivo a eliminar
                string filePathToDelete = Path.Combine(@"\\Srvfs\s_i_ducon$\Documentacion Bitacora", idOTToDelete, archivoToDelete);

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM tbldocumentacion WHERE Id_OT = @IdOT AND Archivo = @Archivo";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@IdOT", idOTToDelete);
                        command.Parameters.AddWithValue("@Archivo", archivoToDelete);
                        int rowsAffected = command.ExecuteNonQuery();

                        // Eliminar el archivo del sistema de archivos
                        if (File.Exists(filePathToDelete))
                        {
                            File.Delete(filePathToDelete);
                        }
                        else
                        {

                        }
                        

                        Session["NumeroDiseño"] = lblText;

                        if (rowsAffected > 0)
                        {
                            string mensajePersonalizado = "El documento se ha eliminado exitosamente.";
                            string urlRedireccion = "Ventas/DocumentacionDise.aspx"; // Cambia esto por la URL correcta
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                        else
                        {
                            string mensajePersonalizado = "Seleccione el elemento que desea eliminar";
                            string urlRedireccion = "Ventas/DocumentacionDise.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                    }
                }

                // Vuelve a enlazar los datos en el DataGridDocumento después de la eliminación
                DataGridDocumento.DataBind();
            }
            else
            {

             
            }
        }

    }
}