using AjaxControlToolkit;
using DocumentFormat.OpenXml.Office.Word;
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

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class DocumentacionOT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuariologueado"] != null)
            {

                if (!IsPostBack)
                {
                    // Obtener valores de las variables de sesión
                    string idOt = Session["Id_OT2"] as string;
                    string pedido = Session["pedido2"] as string;

                    // Asignar valores a los parámetros de SqlDataSource
                    DocumentosOt.SelectParameters["IdOt"].DefaultValue = idOt;
                    DocumentacionFiltrada.SelectParameters["IdOt"].DefaultValue = idOt;
                    DocumentacionFiltrada.SelectParameters["Pedido"].DefaultValue = pedido;

                    tbCategoria.Enabled = false;
                    tbCategoria.CssClass = "form-control ";

                    bool estado = ValidarOTCerrada();


                    if (estado)
                    {
                        btnAdjuntar.Enabled = false;
                        btnAdjuntar.CssClass = "btn btn-outline-primary";
                    }
                    else
                    {
                        btnAdjuntar.Enabled = true;
                    }

                    bntElimnar.Enabled = false;
                    bntElimnar.CssClass = "btn btn-outline-danger";

                    btnSubirAdjuntar.Enabled = false;
                    btnSubirAdjuntar.CssClass = "btn btn-outline-secondary";

                }

                chxMespecial.Enabled = false;
                tbCantidad.Enabled = false;
                tbCantidad.CssClass = "form-control ";

            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }
        }
        private bool ValidarOTCerrada()
        {
            // Realizar la consulta para verificar los permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string query = "select Terminado_Ventas from tblOT where Id_OT = @idOt and Consecutivo_Pedido = @pedido";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer parámetros para evitar SQL Injection
                    command.Parameters.AddWithValue("@idOt", Session["Id_OT2"]?.ToString());
                    command.Parameters.AddWithValue("@pedido", Session["pedido2"]?.ToString());

                    connection.Open();
                    object result = command.ExecuteScalar();

                    int estado = Convert.ToInt32(result);

                    if (estado > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }


            }
        }
        protected void chxDocumento_CheckedChanged(object sender, EventArgs e)
        {
            bool check = chxDocumento.Checked;

            if (check)
            {
                DataGridDoc.DataSourceID = "DocumentosOt";
                DataGridDoc.DataBind();


            }
            else
            {
                DataGridDoc.DataSourceID = "DocumentacionFiltrada";
                DataGridDoc.DataBind();
            }
        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {

            if (DoctOT.HasFile)
            {

                string carpetaNombre = Session["Id_OT2"].ToString();
                string Consecutivo = Session["pedido2"].ToString();
                string rutaBase = @"P:\SISTEMAS\PruebaDocumentacion"; // Reemplaza con tu ruta base

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
                        ErrorValidacionDoc.InnerText = "Se ha producido un error al intentar crear la carpeta. " + ex.Message;
                        return;
                    }
                }

                string nombreArchivo = carpetaNombre + "-" + Consecutivo + " " + DoctOT.FileName; // Reemplaza con el nombre que quieras

                // Ruta completa para guardar el archivo
                string rutaArchivo = Path.Combine(rutaCompleta, nombreArchivo);

                try
                {
                    // Guardar el archivo en la ruta 
                    DoctOT.SaveAs(rutaArchivo);

                    // Realizaos la Insercion 

                    string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "INSERT INTO tblDocumentacion (Id_OT, Pedido, Archivo, Observacion, TipoDocumento,usuario,FechaRegistro,MuebleEspecial,Cantidad ) " +
                                       "VALUES (@Id_OT, @Pedido, @Archivo, @Observacion, @TipoDocumento, @usuario,@FechaRegistro , 0, 0)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@Id_OT", Session["Id_OT2"].ToString());
                            command.Parameters.AddWithValue("@Pedido", Session["pedido2"].ToString());
                            command.Parameters.AddWithValue("@Archivo", nombreArchivo);
                            command.Parameters.AddWithValue("@Observacion", tbObservacion.Text);
                            command.Parameters.AddWithValue("@TipoDocumento", ddlTipoDoc.SelectedItem.Text);
                            command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());
                            command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);


                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                string mensajePersonalizado = "El documento se ha guardado exitosamente.";
                                string urlRedireccion = "FormExtPrin/DocumentacionOT.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }
                            else
                            {
                                string mensajePersonalizado = "Ha ocurrido un error al guardar el documento.";
                                string urlRedireccion = "FormExtPrin/DocumentacionOT.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }

                        }

                    }

                }

                catch (Exception ex)
                {

                    string scriptNoSeleccionado = "alert('Se ha producido un error al intentar guardar el archivo.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado, true);
                }

            }
            else
            {
                // El usuario no  seleccionó ningun archivo a copiar 
                string scriptNoSeleccionado1 = "alert('Por favor seleccione un archivo a copiar .');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado1, true);
            }

        }

        protected void bntElimnar_Click(object sender, EventArgs e)
        {


            string idDocumento = Session["Id_DocumentoOT"].ToString();
            string NombreArchivo = Session["NombreArchivoOT"].ToString();
            string NombreCarpeta = Session["NombreCarpetaOT"].ToString();


            // Eliminamos el documento de la carpeta
            string rutaBase = @"P:\SISTEMAS\PruebaDocumentacion\" + NombreCarpeta;
            string rutaArchivo = Path.Combine(rutaBase, NombreArchivo);
            try
            {
                // Eliminar el documento de la base de datos
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

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
            }
            catch
            {
                string mensajePersonalizado4 = "Ocurrió un error al eliminar el archivo. intentelo de nuevo mas tarde";
                string urlRedireccion4 = "FormExtPrin/DocumentacionOT.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado4)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion4)}");
            }

        

          
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    File.Delete(rutaArchivo);
                }

            }
            catch (UnauthorizedAccessException ex)
            {
                string mensajePersonalizado3 = "Ocurrió un error al eliminar el archivo en el servidor ,por favor cominiquese con el departamento de sistemas.";
                string urlRedireccion3 = "FormExtPrin/DocumentacionOT.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado3)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion3)}");
            }

            string mensajePersonalizado1 = "El documento ha sido eliminado correctamente.";
            string urlRedireccion1 = "FormExtPrin/DocumentacionOT.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado1)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion1)}");

        }

        protected void DataGridDoc_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerDocumento")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDoc.Items[rowIndex];



                string Id_documento = row.Cells[8].Text;
                string NombreArchivo = row.Cells[1].Text;
                string Id_OT = row.Cells[9].Text;

                Session["Id_DocumentoOT"] = Id_documento;
                Session["NombreArchivoOT"] = NombreArchivo;
                Session["NombreCarpetaOT"] = Id_OT;

                if (!ValidarOTCerrada())
                {
                    Button bntElimnar = FindControl("bntElimnar") as Button;
                    if (bntElimnar != null)
                    {
                        bntElimnar.Enabled = true;
                        bntElimnar.CssClass = "btn-sm btn-outline-danger";
                    }

                }


                foreach (DataGridItem item in DataGridDoc.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = "";
                    }
                }

                //se usa Para darle un color a la fila seleccionada
                e.Item.CssClass = "fila-seleccionada";

            }

            else if (e.CommandName == "VerDocumento1")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDoc.Items[rowIndex];

                string NombreArchivo = row.Cells[1].Text;
                string Id_OT = row.Cells[9].Text;

                // Ruta completa del archivo
                string rutaArchivo = @"P:\SISTEMAS\PruebaDocumentacion\" + Id_OT + "\\" + NombreArchivo;

                try
                {
                    // Verificar si el archivo existe antes de intentar abrirlo
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        Process.Start(rutaArchivo);
                        Response.Redirect("~/Formularios/FormExtPrin/DocumentacionOT.aspx");
                    }
                    else
                    {
                        string mensajePersonalizado = "El archivo que estás tratando de abrir no se encuentra en la carpeta. Por favor, comunícate con el administrador para obtener asistencia.";
                        string urlRedireccion = "FormExtPrin/DocumentacionOT.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                    }
                }
                catch (Exception ex)
                {

                    ErrorValidacionDoc.InnerText = "Se ha producido un error al intentar abrir el archivo. " + ex.Message;
                }
            }
        }

        protected void DataGridSolicitudEspecial_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerDocumento3")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridSolicitudEspecial.Items[rowIndex];


                // Asegurarse de obtener el valor actual del campo "Subido"
                string subidoActualText = row.Cells[3].Text;

                int subidoActual;
                if (!string.IsNullOrEmpty(subidoActualText) && int.TryParse(subidoActualText, out subidoActual))
                {
                    subidoActual++;
                    row.Cells[3].Text = subidoActual.ToString();
                }
                else
                {
                    // Si el campo está vacío, asumir un valor de 0
                    row.Cells[3].Text = "1";
                }



                // Darle color a la fila 
                foreach (DataGridItem item in DataGridSolicitudEspecial.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = "";
                    }
                }
                e.Item.CssClass = "fila-seleccionada";


                //Validar si la OT esta cerrada para no no habilitar el boton cargar 
                if (!ValidarOTCerrada())
                {
                    btnSubirAdjuntar.Enabled = true;
                    btnAdjuntar.Enabled = false;
                    btnAdjuntar.CssClass = "btn btn-outline-primary";
                }


                string nombreArchvio = row.Cells[1].Text;
                string tipoDoc = row.Cells[2].Text;

                //Se crea la variable  de session con el nombre del archivo  para realizar la insercion 
                Session["NomArchOT"] = nombreArchvio;

                // Validar si el tipo de documento es "DLLO.ESPECIAL"
                if (tipoDoc == "DLLO.ESPECIAL")
                {
                    ListItem newItem = new ListItem(tipoDoc, tipoDoc);
                    if (ddlTipoDoc.Items.FindByText(tipoDoc) == null)
                    {
                        ddlTipoDoc.Items.Add(newItem);
                        ddlTipoDoc.ClearSelection();
                        newItem.Selected = true;
                        ddlTipoDoc.Enabled = false;
                        ddlTipoDoc.CssClass = "form-control form-control-sm";
                        chxMespecial.Checked = true;
                        tbCantidad.Enabled = true;

                    }
                }
                else
                {

                    ListItem item = ddlTipoDoc.Items.FindByText(tipoDoc);
                    if (item != null)
                    {
                        ddlTipoDoc.ClearSelection();
                        item.Selected = true;
                    }
                }


            }
        }

        protected void btnSubirAdjuntar_Click(object sender, EventArgs e)
        {
            // Validar si la variable de sesión no es null
            if (Session["NomArchOT"] != null)
            {
                // Construimos la ruta del archivo de origen 
                string RutaPE = @"\\172.16.30.6\s_I_ducon$\Documentacion PE";
                string NombreArchivoCopiar = Session["NomArchOT"].ToString();
                string CarpetaOrigen;
                // Obtener caracteres hasta el primer guion
                int indiceGuion = NombreArchivoCopiar.IndexOf('-');
                CarpetaOrigen = NombreArchivoCopiar.Substring(0, indiceGuion);
                string RutaCompletaCopia = Path.Combine(RutaPE, CarpetaOrigen, NombreArchivoCopiar);

                //Contruimos la ruta de la carpeta de Destino 

                // Se debe Cambiar la Ruta para que apunte al servidor 
                string RutaDestino = @"P:\SISTEMAS\PruebaDocumentacion";
                string carpetaIdOt = Session["Id_OT2"].ToString();
                string pedido = Session["pedido2"].ToString();
                string NombreFinalArchivo = carpetaIdOt + "-" + pedido + " " + NombreArchivoCopiar;

                string RutaCompletaDestinoArchivo = Path.Combine(RutaDestino, carpetaIdOt, NombreFinalArchivo);

                try
                {
                    if (File.Exists(RutaCompletaDestinoArchivo))
                    {
                        // No se encontró el archivo en la ruta y hubo falo 
                        string scriptExiste = "alert('El archivo ya fue copiado.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptExiste, true);
                    }
                    else
                    {
                        File.Copy(RutaCompletaCopia, RutaCompletaDestinoArchivo, true);

                        string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string query = "INSERT INTO tblDocumentacion (Id_OT, Pedido, Archivo, Observacion, TipoDocumento,usuario,FechaRegistro,MuebleEspecial,Cantidad ) " +
                                           "VALUES (@Id_OT, @Pedido, @Archivo, @Observacion, @TipoDocumento, @usuario,@FechaRegistro , @MuebleEspecial, @Cantidad)";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {

                                command.Parameters.AddWithValue("@Id_OT", Session["Id_OT2"].ToString());
                                command.Parameters.AddWithValue("@Pedido", Session["pedido2"].ToString());
                                command.Parameters.AddWithValue("@Archivo", NombreFinalArchivo);
                                command.Parameters.AddWithValue("@Observacion", tbObservacion.Text);
                                command.Parameters.AddWithValue("@TipoDocumento", ddlTipoDoc.SelectedItem.Text);
                                command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());
                                command.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                                command.Parameters.AddWithValue("@MuebleEspecial", chxMespecial.Checked);
                                command.Parameters.AddWithValue("@Cantidad", tbCantidad.Text);


                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    string mensajePersonalizado = "El documento se ha copiado exitosamente.";
                                    string urlRedireccion = "FormExtPrin/DocumentacionOT.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }
                                else
                                {
                                    string mensajePersonalizado = "Ha ocurrido un error al copiar el documento.";
                                    string urlRedireccion = "FormExtPrin/DocumentacionOT.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    // No se encontró el archivo en la ruta y hubo falo 
                    string scriptNoSeleccionado = "alert('No se encuentra archivo origen.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado, true);
                }
            }
            else
            {
                // El usuario no  seleccionó ningun archivo a copiar 
                string scriptNoSeleccionado = "alert('Por favor seleccione un archivo a copiar .');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSeleccionado", scriptNoSeleccionado, true);
            }


            Session.Remove("NomArchOT");
        }

       
    }
}