//using NuGet.Protocol.Plugins;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

                    DepartamentoAsesor();
                    CargarTiposDeDocumento();


                    Button bntElimnar = FindControl("bntElimnar") as Button;
                    if (bntElimnar != null)
                    {
                        bntElimnar.Enabled = false;
                        bntElimnar.CssClass = "btn btn-sm btn-outline-danger";

                    }

                    // Obtener las variables de sesión
                    string variableSesion1Soli = (string)Session["Id_Solicitud"];
                    string variableSesion2Detalle = (string)Session["Id_Detalle"];

                    // Asignar el valor del parámetro en el SqlDataSource para cargar la documentacion
                    Documentos.SelectParameters["Documentacion"].DefaultValue = "PE" + variableSesion1Soli + "-" + variableSesion2Detalle;

                    // Cargar los datos en el DataGrid
                    DataGridDocumento.DataSourceID = "Documentos";
                    DataGridDocumento.DataBind();

                    TituloSolictud.Text = "Documentacion Solicitud Especial # " + Session["Id_Solicitud"].ToString() + "- Detalle " + Session["Id_Detalle"].ToString();


                    //Revisar Rol para dibujante para activar los botones 

                    if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                    {

                      
                        if (!ConsultarTerminadoVentas())
                        {
                            Button1.Enabled = false;
                            Button1.CssClass = "btn btn-sm btn-outline-primary";
                        }


                        if (Session["ControlEspecialDes"]?.ToString() == "Especial")
                        {
                            ListItem newItem = new ListItem("DLLO.ESPECIAL");
                            ddlTipoDoc.Items.Add(newItem);
                            ddlTipoDoc.ClearSelection();
                            newItem.Selected = true;
                            ddlTipoDoc.Enabled = false;
                            ddlTipoDoc.CssClass = "form-control form-control-sm";




                            ValidarEspecial.Visible = false;



                            mensaje.Visible = true;
                            mensaje.Text = "Por favor cargue nuevamente el mismo  archivo y presione adjuntar";

                            Session.Remove("ControlEspecialDes");

                        }


                    }
                    else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                    {

                        ValidarEspecial.Visible = true;

                        // Consultar terminado dibujo para controlar el boton de Adjuntar y validar el permiso de control de Documentacion OT
                        if (!ConsultarTerminadoDibujo())
                        {
                            Button1.Enabled = false;
                            Button1.CssClass = "btn btn-sm btn-outline-primary";

                            ValidarEspecial.Enabled = false;
                            Button1.CssClass = "btn btn-sm btn-outline-primary";

                        }

                        if (Session["ControlEspecialDes"]?.ToString() == "Especial")
                        {
                            ListItem newItem = new ListItem("DLLO.ESPECIAL");
                            ddlTipoDoc.Items.Add(newItem);
                            ddlTipoDoc.ClearSelection();
                            newItem.Selected = true;
                            ddlTipoDoc.Enabled = false;
                            ddlTipoDoc.CssClass = "form-control form-control-sm";




                            ValidarEspecial.Visible = false;



                            mensaje.Visible = true;
                            mensaje.Text = "Por favor cargue nuevamente el mismo  archivo y presione adjuntar";

                            Session.Remove("ControlEspecialDes");

                        }

                    }
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
                // Obtener el tamaño máximo permitido en bytes(por ejemplo, 30 MB)
                int maxSizeBytes = 80 * 1024 * 1024; // 80 MB

                // Verificar si el tamaño del archivo excede el límite permitido
                if (FileUpload1.PostedFile.ContentLength > maxSizeBytes)
                {
                    string mensajePersonalizado = "El tamaño del archivo excede el límite permitido de 10 MB.";
                    string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                }

                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    if (!ConsultarTerminadoVentas())
                    {
                        string mensajePersonalizado = "La solicitud ya ha sido programda para ventas y no puede ser modificada.";
                        string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                }
                else if(Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {
                    if (!ConsultarTerminadoDibujo())
                    {
                        string mensajePersonalizado = "La solicitud ya ha sido programda  y no puede ser modificada.";
                        string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                }


                // Validar si el archivo ya existe en la solicitud 
                string NombreCompletoArchivo = "PE" + Session["Id_Solicitud"].ToString() + "-"+ Session["Id_Detalle"].ToString() +" "+ FileUpload1.FileName;

                if (ValidarExistenciaArchivo(NombreCompletoArchivo))
                {
                    string mensajePersonalizado = "El archivo " + FileUpload1.FileName + ", ya pertenece a la solicitud, no se puede adicionar";
                    string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    return;
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
                        mensaje.Text = "Se ha producido un error al intentar crear la carpeta. " + ex.Message;
                        return;
                    }
                }

                // En este punto, la carpeta existe o se ha creado correctamente
                // Ahora puedes guardar el archivo en esa carpeta

                // Nombre de archivo que deseas utilizar
                string nombreArchivo = FileUpload1.FileName; // Reemplaza con el nombre que quieras
                string NombreArchivoCarpeta = carpetaNombre + "-" + Session["Id_Detalle"].ToString() + " " + FileUpload1.FileName;
                // Ruta completa para guardar el archivo
                string rutaArchivo = Path.Combine(rutaCompleta, NombreArchivoCarpeta);

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
                            command.Parameters.AddWithValue("@Archivo", carpetaNombre + "-" + Session["Id_Detalle"].ToString() + " " + nombreArchivo);
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
                    mensaje.Text = "Se ha producido un error al intentar guardar el archivo. " + ex.Message;
                }

            }
            else
            {
                string mensajePersonalizado = "Por favor seleccione un documento";
                string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }


        }

        protected void DataGridDocumento_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int Eps = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "MuebleEspecial"));

                TableCell cell = e.Item.Cells[6];
                cell.Text = (Eps == 1) ? "Si" : "No";

                string archivo = DataBinder.Eval(e.Item.DataItem, "Archivo").ToString();
                e.Item.Cells[1].ToolTip = archivo;


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
                Session["NombreArchivo"] = NombreArchivo;
                Session["NombreCarpeta"] = "PE" + Session["Id_Solicitud"].ToString();




                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    Button bntElimnar = FindControl("bntElimnar") as Button;
                    if (bntElimnar != null)
                    {
                        bntElimnar.Enabled = true;
                        bntElimnar.CssClass = "btn btn-sm btn-outline-danger";
                    }
                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {


                    // Consultar terminado dibujo para controlar el boton de Adjuntar y validar el permiso de control de Documentacion OT
                    if (!ConsultarTerminadoDibujo())
                    {
                        if (VerificarPermiso(Session["CedulaLogeada"]?.ToString(), 28))
                        {

                            Button bntElimnar = FindControl("bntElimnar") as Button;
                            if (bntElimnar != null)
                            {
                                bntElimnar.Enabled = true;
                                bntElimnar.CssClass = "btn btn-sm btn-outline-danger";
                            }


                        }
                        else
                        {
                            Button1.Enabled = false;
                            Button1.CssClass = "btn btn-sm btn-outline-primary";
                        }
                    }
                    else
                    {
                        Button bntElimnar = FindControl("bntElimnar") as Button;
                        if (bntElimnar != null)
                        {
                            bntElimnar.Enabled = true;
                            bntElimnar.CssClass = "btn btn-sm btn-outline-danger";
                        }
                    }
                }

                foreach (DataGridItem item in DataGridDocumento.Items)
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
                DataGridItem row = DataGridDocumento.Items[rowIndex];

                string Id_documento = row.Cells[8].Text;
                string NombreArchivo = row.Cells[1].Text;


                Session["Id_Documento"] = Id_documento;
                Session["NombreArchivo"] = NombreArchivo;
                Session["NombreCarpeta"] = "PE" + Session["Id_Solicitud"].ToString();


                // Ruta completa del archivo que deseas abrir

                string rutaArchivo = @"\\Srvfs\s_i_ducon$\Documentacion PE\" + Session["NombreCarpeta"].ToString() + "\\" + NombreArchivo;
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
                    Response.WriteFile(rutaArchivo);   //Agregar Mensaje salida si el archivo esta en uso 

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

            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {
                if (!ConsultarTerminadoVentas())
                {
                    string mensajePersonalizado1 = "La solicitud ya ha sido programda para ventas y no puede ser modificada.";
                    string urlRedireccion1 = "Ventas/AdjuntarDocumentos.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado1)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion1)}");
                }
            }
            else if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
            {
                if (!ConsultarTerminadoDibujo())
                {
                    string mensajePersonalizado1 = "La solicitud ya ha sido programda  y no puede ser modificada.";
                    string urlRedireccion1 = "Ventas/AdjuntarDocumentos.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado1)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion1)}");
                }
            }


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

        public bool ConsultarTerminadoVentas()
        {
            string consultaActual = "SELECT  ProgramadoVentas FROM tblSoliciDiseEspe WHERE ID_Solicitud = @solicitud";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(consultaActual, connection))
                {
                    cmd.Parameters.AddWithValue("@solicitud", Session["Id_Solicitud"].ToString());
                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        bool rowCount = Convert.ToBoolean(result);
                        // Si rowCount es igual a 1, retornamos true; de lo contrario, retornamos false
                        return rowCount == false;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }
                }
            }
        }

        public bool ConsultarTerminadoDibujo()
        {
            string consultaActual = "SELECT  Terminado FROM tblSoliciDiseEspe WHERE ID_Solicitud = @solicitud";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(consultaActual, connection))
                {
                    cmd.Parameters.AddWithValue("@solicitud", Session["Id_Solicitud"].ToString());
                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        bool rowCount = Convert.ToBoolean(result);
                        // Si rowCount es igual a 1, retornamos true; de lo contrario, retornamos false
                        return rowCount == false;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }
                }
            }
        }

        public void DepartamentoAsesor()
        {

            string consultaActual = "SELECT B.Descripcion FROM tblEmpleado As A INNER join tblDepartamento As B on B.ID_Departamento = A.Dependencia WHERE  Cedula = @Cedula";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(consultaActual, connection))
                {
                    command.Parameters.AddWithValue("@Cedula", Session["CedulaLogeada"].ToString());
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        // Data arrived.
                        string Departamento = (string)command.ExecuteScalar();
                        Session["Departamento"] = Departamento;

                    }


                }
            }

        }

        private bool VerificarPermiso(string cedulaLogueada, int idPermiso)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Aquí se debe ajustar la consulta SQL para incluir el parámetro del ID del permiso
            string query = $"SELECT COUNT(*) FROM tblPermiso_Empleado WHERE ID_Empleado = '{cedulaLogueada}' AND ID_Permiso = @Permiso";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Permiso", idPermiso); // Agregar el parámetro del ID del permiso
                    connection.Open();
                    int count = (int)command.ExecuteScalar(); // Ejecutar la consulta y obtener el resultado
                    return count > 0; // Devolver verdadero si se encuentra algún registro que cumpla la condición
                }
            }
        }


        //Adiciones o cambio 

        private void CargarTiposDeDocumento()
        {
            if (Session["Departamento"] != null)
            {
                string departamento = Session["Departamento"].ToString().ToUpper();

                // Limpiar el DropDownList antes de llenarlo
                ddlTipoDoc.Items.Clear();

                // Agregar opción por defecto
                ddlTipoDoc.Items.Add(new ListItem("--Seleccione--", ""));

                // Llenar el DropDownList según el departamento
                if (departamento == "VENTAS")
                {
                    ddlTipoDoc.Items.Add(new ListItem("BOSQUEJO", "BOSQUEJO"));
                    ddlTipoDoc.Items.Add(new ListItem("CONTABLE", "CONTABLE"));
                }
                else if (departamento == "DISEÑO" || departamento == "DESARROLLO DE PRODUCTO")
                {
                    ddlTipoDoc.Items.Add(new ListItem("COMPRAS", "COMPRAS"));
                    ddlTipoDoc.Items.Add(new ListItem("PRODUCTIVO", "PRODUCTIVO"));
                }
            }
        }

        protected void ValidarEspecial_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                HttpPostedFile file = FileUpload1.PostedFile;
                string extension = Path.GetExtension(FileUpload1.FileName);

                // Crear una copia temporal del archivo en la carpeta temporal del sistema
                string archivoTemporal = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
                FileUpload1.SaveAs(archivoTemporal);

                // Leer el contenido del archivo Excel
                using (FileStream fs = new FileStream(archivoTemporal, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = null;

                    // Determinar el tipo de archivo Excel (XLS o XLSX)
                    if (extension.Equals(".xls"))
                    {
                        // Para archivos .xls (Excel 97-2003)
                        workbook = new HSSFWorkbook(fs);
                    }
                    else if (extension.Equals(".xlsx"))
                    {
                        // Para archivos .xlsx (Excel 2007 y posteriores)
                        workbook = new XSSFWorkbook(fs);
                    }
                    else
                    {
                        string mensajePersonalizado = "El archivo cargado no corresponde al formato";
                        string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }

                    // Obtener el primer worksheet
                    ISheet sheet = workbook.GetSheetAt(0);

                    // Leer el texto de las celdas necesarias
                    string CeldaA1 = sheet.GetRow(0)?.GetCell(0)?.ToString();
                    string CeldaA2 = sheet.GetRow(1)?.GetCell(0)?.ToString();
                    string CeldaB2 = sheet.GetRow(1)?.GetCell(1)?.ToString();
                    string CeldaC2 = sheet.GetRow(1)?.GetCell(2)?.ToString();
                    string CeldaD2 = sheet.GetRow(1)?.GetCell(3)?.ToString();
                    string CeldaE2 = sheet.GetRow(1)?.GetCell(4)?.ToString();
                    string CeldaA4 = sheet.GetRow(0)?.GetCell(4)?.ToString();

                    // Verificar si el contenido es el esperado
                    if (CeldaA1 == "DESPIECE PRODUCTO ESPECIAL" && CeldaA2 == "N.º" && CeldaB2 == "PARTE" && CeldaC2 == "CANT." && CeldaD2 == "COD. INV." && CeldaE2 == "AREA")
                    {

                        Session["ControlEspecialDes"] = "Especial";

                        string mensajePersonalizado = "Archivo de Excel Validado";
                        string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                    else
                    {

                        string mensajePersonalizado = "El archivo ha sido modificado. o no es un desarrollo especial.";
                        string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                    }

                }

            }
            else
            {
                string mensajePersonalizado = "Por favor seleccione un documento";
                string urlRedireccion = "Ventas/AdjuntarDocumentos.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
        }

        private bool ValidarExistenciaArchivo(string NombreArchivo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Aquí se debe ajustar la consulta SQL para incluir el parámetro del ID del permiso
            string query = $"SELECT COUNT(*) FROM tblDocumentacion WHERE Archivo = @NombreArchivo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreArchivo", NombreArchivo); // Agregar el parámetro del ID del permiso
                    connection.Open();
                    int count = (int)command.ExecuteScalar(); // Ejecutar la consulta y obtener el resultado
                    return count > 0; // Devolver verdadero si se encuentra algún registro que cumpla la condición
                }
            }
        }

    }
}