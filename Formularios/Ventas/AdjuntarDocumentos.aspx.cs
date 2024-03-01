using DocumentFormat.OpenXml.Drawing.ChartDrawing;
//using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using Button = System.Web.UI.WebControls.Button;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Ventas
{
    public partial class AdjuntarDocumentos : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id_Detalle"] != null)
            {

                // Para Ponerle tutulo a la pantalla 
                string Id_Detalle = Session["Id_Detalle"].ToString();
                Page.Title = "Documentacion Detalle - " + Id_Detalle;
            }

            if (!IsPostBack)
            {
                if (Session["usuariologueado"] != null)
                {

                    Button bntElimnar = FindControl("bntElimnar") as Button;
                    if (bntElimnar != null)
                    {
                        bntElimnar.Enabled = false;
                        bntElimnar.CssClass = "btn-sm btn-outline-danger";

                    }

                    // Obtener las variables de sesión
                    string variableSesion1Soli = (string)Session["Id_Solicitud"];
                    string variableSesion2Detalle = (string)Session["Id_Detalle"];

                    // Asignar el valor del parámetro en el SqlDataSource para cargar la documentacion
                    Documentos.SelectParameters["Documentacion"].DefaultValue = "PE" + variableSesion1Soli +"-"+ variableSesion2Detalle;

                    // Cargar los datos en el DataGrid
                    DataGridDocumento.DataSourceID = "Documentos";
                    DataGridDocumento.DataBind();

                    TituloSolictud.Text = "Documentacion Solicitud Especial # " + Session["Id_Solicitud"].ToString() + "- Detalle " + Session["Id_Detalle"].ToString();



                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }



            }



        }

        protected void AdjuntarDocumento(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                // Obtener el tamaño máximo permitido en bytes(por ejemplo, 5 MB)
                int maxSizeBytes = 10 * 1024 * 1024; // 5 MB

                // Verificar si el tamaño del archivo excede el límite permitido
                if (FileUpload1.PostedFile.ContentLength > maxSizeBytes)
                {
                    string mensajePersonalizado = "El tamaño del archivo excede el límite permitido de 10 MB.";
                    string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                }



                string carpetaNombre = "PE" + Session["Id_Solicitud"].ToString(); // Reemplaza con el nombre de la carpeta deseada
                string rutaBase = @"\\Srvfs\s_i_ducon$\Documentacion PE"; // Reemplaza con tu ruta base
             //   string rutaBase = @"P:\SISTEMAS\PruebaDocumentacion"; // Reemplaza con tu ruta base

                string rutaCompleta = Path.Combine(rutaBase, carpetaNombre);

                // Verificar si la carpeta existe
                if (!Directory.Exists(rutaCompleta))
                {
                    try
                    {
                        // Si no existe, crear la carpeta
                        Directory.CreateDirectory(rutaCompleta);
                    }
                    catch (Exception ex)
                    {
                        mensaje.InnerText = "Se ha producido un error al intentar crear la carpeta. " + ex.Message;
                        return;
                    }
                }

                // En este punto, la carpeta existe o se ha creado correctamente
                // Ahora puedes guardar el archivo en esa carpeta

                // Nombre de archivo que deseas utilizar
                string nombreArchivo = FileUpload1.FileName; // Reemplaza con el nombre que quieras
                string NombreArchivoCarpeta =carpetaNombre +"-" + Session["Id_Detalle"].ToString() + " " + FileUpload1.FileName ;
                // Ruta completa para guardar el archivo
                string rutaArchivo = Path.Combine(rutaCompleta,NombreArchivoCarpeta);

                try
                {
                    // Guardar el archivo en la ruta especificada
                    FileUpload1.SaveAs(rutaArchivo);


                    // Realizar esta Insercion 


                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        //Realizamos la Insercion 
                        string query = "INSERT INTO tblDocumentacion (Id_OT, Pedido, Archivo, Observacion, TipoDocumento,usuario,FechaRegistro,MuebleEspecial,Cantidad ) " +
                                       "VALUES (@Id_OT, 0, @Archivo, ' ', @TipoDocumento, @usuario,@FechaRegistro , 0, 0)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@Id_OT", carpetaNombre + "-" + Session["Id_Detalle"].ToString());
                            command.Parameters.AddWithValue("@Archivo", carpetaNombre + "-"  + Session["Id_Detalle"].ToString() +" " + nombreArchivo);
                            command.Parameters.AddWithValue("@TipoDocumento", ddlTipoDoc.SelectedItem.Text);
                            command.Parameters.AddWithValue("@usuario", Session["NombreAsesor"].ToString());
                            command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);


                            command.ExecuteNonQuery();
                        }


                    }

                    string mensajePersonalizado = "El documento se ha guardado exitosamente.";
                    string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }

                catch (Exception ex)
                {
                    mensaje.InnerText = "Se ha producido un error al intentar guardar el archivo. " + ex.Message;
                }

            }
            else
            {
                mensaje.InnerText = "Error: No has seleccionado ningún archivo.";
            }


        }

        protected void DataGridDocumento_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int Eps = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "MuebleEspecial"));

                TableCell cell = e.Item.Cells[6];
                cell.Text = (Eps == 1) ? "Si" : "No";   

            }
        }


        protected void DataGridDocumentosPE_LinkButton(object source, DataGridCommandEventArgs e)
        {
            
            if (e.CommandName == "VerDocumento")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDocumento.Items[rowIndex];

                string Id_documento = row.Cells[8].Text;
                string NombreArchivo = row.Cells[1].Text;
                string Id_OT = row.Cells[9].Text;

                Session["Id_Documento"] = Id_documento;
                Session["NombreArchivo"] =  NombreArchivo;
                Session["NombreCarpeta"] = "PE" +Session["Id_Solicitud"].ToString();

                Button bntElimnar = FindControl("bntElimnar") as Button;
                if (bntElimnar != null)
                {
                    bntElimnar.Enabled = true;
                    bntElimnar.CssClass = "btn-sm btn-outline-danger";
                }


                //se usa Para darle un color a la fila seleccionada
                e.Item.CssClass = "fila-seleccionada";

            }
            
            else if(e.CommandName == "VerDocumento1")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDocumento.Items[rowIndex];

                string Id_documento = row.Cells[8].Text;
                string NombreArchivo = row.Cells[1].Text;
               

                Session["Id_Documento"] = Id_documento;
                Session["NombreArchivo"] = NombreArchivo;
                Session["NombreCarpeta"] ="PE"+ Session["Id_Solicitud"].ToString();
               

                // Ruta completa del archivo que deseas abrir
                
                  string rutaArchivo = @"\\Srvfs\s_i_ducon$\Documentacion PE\"+  Session["NombreCarpeta"].ToString() + "\\" + NombreArchivo;
                  // string rutaArchivo = @"P:\SISTEMAS\PruebaDocumentacion\" + Id_OT + "\\" + NombreArchivo;

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
                    string mensajeExito = "El documennto ha sido cambiado o borrado en el servidor.";
                    string scriptExito = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                }










            }

        }

        protected void EliminarDocumento(object sender, EventArgs e)
        {
            // Obtener el id del documento a eliminar
            string idDocumento = Session["Id_Documento"].ToString();
            string NombreArchivo = Session["NombreArchivo"].ToString();
            string NombreCarpeta = Session["NombreCarpeta"].ToString();
            // Eliminar el documento de la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM tblDocumentacion WHERE ID_Documento = @idDocumento";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idDocumento", idDocumento);

                    command.ExecuteNonQuery();
                }
            }

            // Eliminar el documento de la carpeta
            string rutaBase = @"\\Srvfs\s_i_ducon$\Documentacion PE\" + NombreCarpeta;
            // string rutaBase = @"P:\SISTEMAS\PruebaDocumentacion\" + NombreCarpeta;
            string rutaArchivo = Path.Combine(rutaBase, NombreArchivo);

            if (File.Exists(rutaArchivo))
            {
                File.Delete(rutaArchivo);
            }

            string mensajePersonalizado = "El documento ha sido eliminado correctamente.";
            string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


        }

    }
}