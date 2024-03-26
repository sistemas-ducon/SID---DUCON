using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Consultas
{
    public partial class DocumentacionReproceso : System.Web.UI.Page
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
                    // Obtener valores de las variables de sesión
                    string idOt = Session["OTReproceso"] as string;
                    string pedido = Session["pedReproceso"] as string;

                    // Asignar valores a los parámetros de SqlDataSource

                    DSDocReproceso.SelectParameters["OT"].DefaultValue = idOt;
                    DSDocReproceso.SelectParameters["Ped"].DefaultValue = pedido;

                    DataGridDocRepro.DataBind();
                }
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }




        }

        protected void btnAdjuntarDoc_Click(object sender, EventArgs e)
        {
            if (DocReproceso.HasFile)
            {
                // Obtener el tamaño máximo permitido en bytes(por ejemplo, 50 MB)
                int maxSizeBytes = 50 * 1024 * 1024; // 50 MB

                // Verificar si el tamaño del archivo excede el límite permitido
                if (DocReproceso.PostedFile.ContentLength > maxSizeBytes)
                {
                    string mensajePersonalizado = "El tamaño del archivo excede el límite permitido de 50 MB.";
                    string urlRedireccion = "Consultas/DocumentacionReproceso.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                }

                if (ValidarCampos())
                {
                    string carpetaNombre = Session["OTReproceso"].ToString();
                    string Consecutivo = Session["pedReproceso"].ToString();

                    String rutaBase = @"\\Srvfs\s_i_ducon$\Documentacion de Obras";
                    //string rutaBase = @"P:\SISTEMAS\PruebaDocumentacion"; // Reemplaza con tu ruta base

                    string rutaCompleta = Path.Combine(rutaBase, carpetaNombre);

                    // Verificamos si la carpeta existe
                    if (!Directory.Exists(rutaCompleta))
                    {
                        try
                        {
                            // Si no existe, se crea  la carpeta
                            Directory.CreateDirectory(rutaCompleta);
                        }
                        catch (Exception ex)
                        {
                            ErrorValidacionDoc.InnerText = ex.Message;

                            string mensajePersonalizadoMB = "Se ha producido un error al intentar crear la carpeta. Intenta nuevamente ";
                            string urlRedireccion = "Consultas/DocumentacionReproceso.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizadoMB)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                    }

                    string nombreArchivo = carpetaNombre + "-" + Consecutivo + " " + DocReproceso.FileName; // Reemplaza con el nombre que quieras

                    // Ruta completa para guardar el archivo
                    string rutaArchivo = Path.Combine(rutaCompleta, nombreArchivo);

                    try
                    {


                        // Realizaos la Insercion 

                        string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "INSERT INTO tblDocumentacionOT (Id_OT, Pedido, Archivo, Observacion, TipoDocumento,Usuario,FechaRegistro) " +
                                           "VALUES (@Id_OT, @Pedido, @Archivo, @Observacion, @TipoDocumento, @usuario,@FechaRegistro )";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {

                                command.Parameters.AddWithValue("@Id_OT", Session["OTReproceso"].ToString());
                                command.Parameters.AddWithValue("@Pedido", Session["pedReproceso"].ToString());
                                command.Parameters.AddWithValue("@Archivo", nombreArchivo);
                                command.Parameters.AddWithValue("@Observacion", tbObservacion.Text);
                                command.Parameters.AddWithValue("@TipoDocumento", ddlTipoDoc.SelectedItem.Text);
                                command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());
                                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);




                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Guardar el archivo en la ruta 
                                    DocReproceso.SaveAs(rutaArchivo);

                                    string mensajePersonalizadoExito = "El documento se ha guardado exitosamente.";
                                    string urlRedireccion = "Consultas/DocumentacionReproceso.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizadoExito)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }
                                else
                                {
                                    string mensajePersonalizadoFalla = "Ha ocurrido un error al guardar el documento.";
                                    string urlRedireccion = "Consultas/DocumentacionReproceso.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizadoFalla)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }

                            }

                        }

                    }

                    catch (Exception ex)
                    {
                        ErrorValidacionDoc.InnerText = ex.Message;
                        string scriptNoSeleccionado = "alert('Se ha producido un error al intentar guardar el archivo.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado, true);

                    }
                }




            }
            else
            {
                // El usuario no  seleccionó ningun archivo a guardar 
                string scriptNoSeleccionado1 = "alert('Por favor seleccione un archivo a guardar .');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado1, true);
            }

        }

        private bool ValidarCampos()
        {
            if (ddlTipoDoc.SelectedValue == "")
            {
                string scriptNoSelect = "alert('Por favor, seleccione un  tipo de Documento.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptNoSelect, true);
                return false;
            }

            return true;
        }

        protected void btnElimnarDoc_Click(object sender, EventArgs e)
        {
            string idDocumento = idDocmuento.Text;
            string NombreArchivo = nombreArchivo.Text;
            string NombreCarpeta = NombreCarpeta1.Text;



            if (idDocumento == "")
            {
                string mensajePersonalizado4 = "Por favor, seleccione un archivo para eliminar";
                string urlRedireccion4 = "Consultas/DocumentacionReproceso.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado4)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion4)}");
            }
            else
            {
                // Eliminamos el documento de la carpeta
                string rutaBase = @"\\Srvfs\s_i_ducon$\Documentacion de Obras\" + NombreCarpeta;
                // string rutaBase = @"P:\SISTEMAS\PruebaDocumentacion\" + NombreCarpeta;
                string rutaArchivo = Path.Combine(rutaBase, NombreArchivo);

                try
                {
                    if (File.Exists(rutaArchivo))
                    {
                        File.Delete(rutaArchivo);
                    }

                }
                catch (UnauthorizedAccessException ex)
                {
                    string scriptNoSeleccionado = "alert('Se ha producido un error al intentar guardar el archivo.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado, true);
                }


                try
                {
                    // Eliminar el documento de la base de datos
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "DELETE FROM tblDocumentacionOT WHERE ID_Documento = @idDocumento";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@idDocumento", idDocumento);

                            int filasAfectadas = command.ExecuteNonQuery();

                            if (filasAfectadas > 0)
                            {
                                string mensajePersonalizado1 = "El documento ha sido eliminado correctamente.";
                                string urlRedireccion1 = "Consultas/DocumentacionReproceso.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado1)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion1)}");
                            }
                            else
                            {
                                string mensajePersonalizado1 = "No ocurrio un error al eliminar el archivo. intentelo nuevamente o comuniquese con el departamento de sistemas";
                                string urlRedireccion1 = "Consultas/DocumentacionReproceso.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado1)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion1)}");
                            }


                        }
                    }
                }
                catch
                {
                    string scriptNoSeleccionado = "alert('Se ha producido un error al intentar guardar el archivo.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado, true);
                }


            }


        }

        protected void DataGridDocRepro_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerDocumento")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDocRepro.Items[rowIndex];

                string NombreArchivo = row.Cells[1].Text;
                string Id_documento = row.Cells[6].Text;
                string Id_OT = row.Cells[7].Text;


                idDocmuento.Text = Id_documento;
                nombreArchivo.Text = NombreArchivo;
                NombreCarpeta1.Text = Id_OT;

                foreach (DataGridItem item in DataGridDocRepro.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = "";
                    }
                }

                //se usa Para darle un color a la fila seleccionada
                e.Item.CssClass = "fila-seleccionada";


            }

        }
    }
}