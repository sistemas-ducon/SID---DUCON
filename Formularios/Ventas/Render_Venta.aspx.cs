using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using SISTEMA_INTEGRAL_DUCON.Formularios.Ventas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes;
using Button = System.Web.UI.WebControls.Button;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Control = System.Web.UI.Control;
using Label = System.Web.UI.WebControls.Label;
using ListItem = System.Web.UI.WebControls.ListItem;
using Path = System.IO.Path;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Render_Venta : System.Web.UI.Page
    {

        private bool isModalVisible = false;
        string mensaje = "";

        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Evaluar los permisos del usuarioa y que departamento Pertenece 

            if (!IsPostBack)
            {

                if (Session["usuariologueado"] != null)
                {
                    CargarAsesoresEnDropDownList();
                    DepartamentoAsesor();
                    CargarVariablesDeSesion();

                    if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                    {
                        Button btnTrabajarRender = FindControl("btnTrabajarRender") as Button;
                        if (btnTrabajarRender != null)
                        {
                            btnTrabajarRender.Enabled = false;
                            btnTrabajarRender.CssClass = "btn btn-sm btn-outline-secondary";
                        }

                        Button btnDesprogramarRender = FindControl("btnDesprogramarRender") as Button;
                        if (btnDesprogramarRender != null)
                        {
                            btnDesprogramarRender.Enabled = false;
                            btnDesprogramarRender.CssClass = "btn btn-sm btn-outline-secondary";
                        }


                        Button btnProgramarRender = FindControl("btnProgramarRender") as Button;
                        if (btnProgramarRender != null)
                        {
                            btnProgramarRender.Enabled = false;
                            btnProgramarRender.CssClass = "btn btn-warning";

                        }

                       

                    }
                    else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                    {
                        Button btnTrabajarRender = FindControl("btnTrabajarRender") as Button;
                        if (btnTrabajarRender != null)
                        {
                            btnTrabajarRender.Enabled = false;
                            btnTrabajarRender.CssClass = "btn btn-sm btn-outline-secondary";
                        }

                        Button btnDesprogramarRender = FindControl("btnDesprogramarRender") as Button;
                        if (btnDesprogramarRender != null)
                        {
                            btnDesprogramarRender.Enabled = false;
                            btnDesprogramarRender.CssClass = "btn btn-sm btn-outline-secondary";
                        }


                        Button btnProgramarRender = FindControl("btnProgramarRender") as Button;
                        if (btnProgramarRender != null)
                        {
                            btnProgramarRender.Text = "Terminar";
                            btnProgramarRender.Enabled = false;
                            btnProgramarRender.CssClass = "btn btn-warning";

                        }

                        // limpiar las variables para el doble Click
                        Session.Remove("ID_Render1");
                        Session.Remove("ClickCountRender");

                       

                    }


                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }

            }

        }

        public void CargarVariablesDeSesion()
        {
            Dictionary<string, Control> variablesDeSesionYControles = new Dictionary<string, Control>
            {
                { "DiseñoSession", tbDiseño },
                { "FecIngresoSession", tbIngreso },
                { "FechaUltActiv", tbUltActiv },
                { "FecEntregaSession", tbEntrega },
                { "FechaOKSession", tbFechaOk },
                { "ClienteSession", tbCliente },
                { "AsesorSession", ddlAsesor },
                { "ProyectoSession", tbProyecto },
                { "ContactoSession", tbContacto },
                { "CelularSession", tbCelular },
                { "MailSession", tbMail },
                { "TelefonoSession", tbTelefono },
                { "PlanoSession", tbPlano },
                { "ZonaSession", ddlZona },
                { "ImagenSession", tbImagenes },
                { "LineaSession", tbLinea },
                { "SupSession", tbSup },
                { "AccSession", tbAcc },
                { "CantosSession", tbCantos },
                { "PerfilSession", tbPerfil },
                { "PanelesSession", tbPaneles },
                { "ArcSession", tbArch },
                { "SillasSession", tbSillas },
                { "PantallasSession", tbPantallas },
                { "EspArqSession", chxAnimacion },
                { "AcabPisSession", tbAcaPisZoc },
                { "AcabMuroSession", tbAcaMuros },
                { "IluSession", tbIluminacion },
                { "AntSession", tbAntepecho },
                { "AmbientacionSession", chxAmbientacion },
                { "AnimacionSession", chxAnimacion },
                { "NumeroRenderCargar", NumeroRender },
               
            };


            foreach (var kvp in variablesDeSesionYControles)
            {
                string valorSesion = Session[kvp.Key] as string;
                if (!string.IsNullOrEmpty(valorSesion))
                {
                    if (kvp.Value is TextBox)
                    {
                        if (DateTime.TryParse(valorSesion, out DateTime fecha))
                        {
                            // Formatear como fecha y hora
                            ((TextBox)kvp.Value).Text = fecha.ToString("yyyy-MM-ddTHH:mm");
                        }
                        else
                        {
                            ((TextBox)kvp.Value).Text = valorSesion;
                        }
                    }
                    else if (kvp.Value is DropDownList)
                    {
                        ((DropDownList)kvp.Value).SelectedItem.Text = valorSesion;
                    }
                    else if (kvp.Value is CheckBox)
                    {
                        ((CheckBox)kvp.Value).Checked = Convert.ToBoolean(valorSesion);
                    }
                    else if (kvp.Value is Label)
                    {
                        ((Label)kvp.Value).Text = valorSesion;
                    }

                    Session.Remove(kvp.Key);
                }
            }

            string Area = Session["AreaSession"] as string;
            if (!string.IsNullOrEmpty(Area))
            {
                txAreaRender.InnerText = Area;
                Session.Remove("AreaSession");
            }
            string ObservacionVenta = Session["ObVentaSession"] as string;
            if (!string.IsNullOrEmpty(ObservacionVenta))
            {
                txObsVentas.InnerText = ObservacionVenta;
                Session.Remove("ObVentaSession");
            }

            string Mueble = Session["MuebleSession"] as string;
            if (!string.IsNullOrEmpty(Mueble))
            {
                txMuebles.InnerText = Mueble;
                Session.Remove("MuebleSession");
            }

            string ObsDibujo = Session["ObsDibujoSession"] as string;
            if (!string.IsNullOrEmpty(ObsDibujo))
            {
                txObsDibujo.InnerText = ObsDibujo;
                Session.Remove("ObsDibujoSession");
            }

            string SeguPausas = Session["SegPauSession"] as string;
            if (!string.IsNullOrEmpty(SeguPausas))
            {
                txSegPausas.InnerText = SeguPausas;
                Session.Remove("SegPauSession");
            }


            if (NumeroRender.Text.ToUpper() == "POR DEFINIR" || NumeroRender.Text == "")
            {
                Button btnProgramarRender = FindControl("btnProgramarRender") as Button;
                if (btnProgramarRender != null)
                {
                    btnProgramarRender.Enabled = false;
                    btnProgramarRender.CssClass = "btn btn-warning";

                }
            }
            else
            {
                Button btnProgramarRender = FindControl("btnProgramarRender") as Button;
                if (btnProgramarRender != null)
                {
                    btnProgramarRender.Enabled = !ConsultarTerminadoVentas();
                    btnProgramarRender.CssClass = "btn btn-warning";

                }

            }


        }
        private bool ConsultarTerminadoVentas()
        {
            bool ok = false;
            string query = "SELECT ProgramadoVentas FROM tblRender WHERE Id_Render = @ID_Render";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            int programadoVentas = Convert.ToInt32(result);
                            ok = (programadoVentas == 1);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones (puedes registrar el error o manejarlo según tus necesidades)
                        // Console.WriteLine("Error: " + ex.Message); // Ejemplo para depuración
                    }
                }
            }

            return ok;
        }

        protected void ddlAsesores_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlAsesor.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlZona2_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlZona2.Items.Insert(0, new ListItem("Todas", "0"));

        }

        protected void chxConvenciones_CheckedChanged(object sender, EventArgs e)
        {
            if (chxConvenciones.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#myModal').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial  order by Nombre";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto"; // Campos que se mostrará en el DropDownLi
                ddlAsesor.DataValueField = "Cedula";

                ddlAsesor.DataBind();

                reader.Close();
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

        } // Campo se podria Cargar en el login

        protected void RenderPorZonaX(object sender, EventArgs e)
        {

            string valorSeleccionado = ddlZona2.SelectedValue;
            int numeroEntero = int.Parse(valorSeleccionado);
            CambiarSqlDataSource(numeroEntero);

            string script = @"ControlCamporYBotones();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ControlCamporYBotones", script, true);

        }

        private void CambiarSqlDataSource(int valorSeleccionado)
        {
            if (valorSeleccionado == 1)
            {
                DataGridRenders.DataSourceID = "RenderPorZona";
            }
            else if (valorSeleccionado == 2)
            {
                DataGridRenders.DataSourceID = "RenderPorZona";
            }
            else
            {
                DataGridRenders.DataSourceID = "CargarRenders";
            }

            DataGridRenders.DataBind();

            // se limpiar las variables del doble click 
            Session.Remove("ID_Render1");
            Session.Remove("ClickCountRender");

            // Coltrol de Boton de Trabajar Y Desprogramar Render 

            btnTrabajarRender.Enabled = false;
            btnTrabajarRender.CssClass = "btn btn-sm btn-outline-secondary";

            btnDesprogramarRender.Enabled = false;
            btnDesprogramarRender.CssClass = "btn btn-sm btn-outline-secondary";


        }

        protected void DataGridRenders_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                // Aplica la clase "fila-clickeable" a todas las filas
                e.Item.CssClass += " fila-clickeable";


                // Obtener los valores de las columnas ocultas
                int programadoVentas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas"));
                int pausado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Pausado"));
                int terminadoDibujo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "TerminadoRender"));

                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));


                if (terminadoDibujo == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#77a765"); //Verde
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (pausado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#70ede4"); // Aqua
                }
                else if (fechaProgramada <= DateTime.Now && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868");    //rojo 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (programadoVentas == 0)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#72459b");    //Morado 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#efdd79");//amarillo 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                }


            }

        }

        protected void DataGridRenders_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerRenders")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridRenders.Items[rowIndex];

                // Se utiliza para darle el color solo a la fila seleccionada 
                foreach (DataGridItem item in DataGridRenders.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada";


                string IdRender = row.Cells[2].Text;
                string NomRender = row.Cells[43].Text.Replace("&nbsp;", "");
                string FechaUltimaActivacion = row.Cells[4].Text;
                DateTime FechaUltimaActivacionFormat = DateTime.Parse(FechaUltimaActivacion);
                string FechaEntrega = row.Cells[5].Text;
                DateTime FechaEntregaFormat = DateTime.Parse(FechaEntrega);
                string Asesor = row.Cells[6].Text;
                //Realizado por [7]
                string Zona = row.Cells[8].Text;
                string FechaRenderOK = row.Cells[9].Text;
                DateTime FechaRenderOKFormat = DateTime.Parse(FechaRenderOK);
                string NumeroDiseño = row.Cells[10].Text.Replace("&nbsp;", "");
                string NombreRender = row.Cells[11].Text;
                string NombreContacto = row.Cells[12].Text.Replace("&nbsp;", "");
                string FechaIngreso = row.Cells[13].Text;
                DateTime FechaIngresoFormat = DateTime.Parse(FechaIngreso);
                string Celular = row.Cells[14].Text.Replace("&nbsp;", "");
                string Mail = row.Cells[15].Text.Replace("&nbsp;", "");
                string Telefono = row.Cells[16].Text.Replace("&nbsp;", "");
                string Plano = row.Cells[17].Text.Replace("&nbsp;", "");

                string Imagenes = row.Cells[18].Text.Replace("&nbsp;", "");
                string Areas = row.Cells[19].Text.Replace("&nbsp;", "");
                string ObsVentas = row.Cells[20].Text.Replace("&nbsp;", "");
                string SegPausa = row.Cells[21].Text.Replace("&nbsp;", "");
                string Observacion_Dibujo = row.Cells[22].Text.Replace("&nbsp;", "");
                string Linea = row.Cells[23].Text.Replace("&nbsp;", "");
                string Superficie = row.Cells[24].Text.Replace("&nbsp;", "");
                string Accesorios = row.Cells[25].Text.Replace("&nbsp;", "");
                string Cantos = row.Cells[26].Text.Replace("&nbsp;", "");
                string Perfileria = row.Cells[27].Text.Replace("&nbsp;", "");
                string Paneles = row.Cells[28].Text.Replace("&nbsp;", "");
                string Archivadores = row.Cells[29].Text.Replace("&nbsp;", "");
                string Sillas = row.Cells[30].Text.Replace("&nbsp;", "");
                string Pantallas = row.Cells[31].Text.Replace("&nbsp;", "");
                string EspArquitectonico = row.Cells[32].Text;
                string Muebles = row.Cells[33].Text.Replace("&nbsp;", "");
                string PisoyZocalo = row.Cells[34].Text.Replace("&nbsp;", "");
                string Muros = row.Cells[35].Text.Replace("&nbsp;", "");
                string Iluminacion = row.Cells[36].Text.Replace("&nbsp;", "");
                string Antepecho = row.Cells[37].Text.Replace("&nbsp;", "");
                string Ambientacion = row.Cells[38].Text;
                string Animacion = row.Cells[39].Text;
                string TerminadoVentas = row.Cells[40].Text;
                string pausado = row.Cells[41].Text;

                Session["Pausado"] = pausado;

                string TerminadoDibujo = row.Cells[42].Text;

                NumeroRender.Text = IdRender;
                tbCliente.Text = NomRender;
                tbUltActiv.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-ddTHH:mm");
                tbUltActivServidor.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-ddTHH:mm");
                tbEntrega.Text = FechaEntregaFormat.ToString("yyyy-MM-ddTHH:mm");
                tbEntregaServidor.Text = FechaEntregaFormat.ToString("yyyy-MM-ddTHH:mm");
                string nombreBuscado = Asesor; // El nombre que deseas buscar
                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == nombreBuscado)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true; // Selecciona el elemento si se encuentra
                        break; // Rompe el bucle una vez que se encuentra una coincidencia
                    }
                }

                string ZonaBus = Zona; // El nombre que deseas buscar
                foreach (ListItem item in ddlZona.Items)
                {
                    if (item.Text == ZonaBus)
                    {
                        ddlZona.ClearSelection();
                        item.Selected = true; // Selecciona el elemento si se encuentra
                        break; // Rompe el bucle una vez que se encuentra una coincidencia
                    }
                }
                tbFechaOk.Text = FechaRenderOKFormat.ToString("yyyy-MM-ddTHH:mm");
                tbFechaOkServidor.Text = FechaRenderOKFormat.ToString("yyyy-MM-ddTHH:mm");
                tbDiseño.Text = NumeroDiseño;
                tbProyecto.Text = NombreRender;
                tbContacto.Text = NombreContacto;
                tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-ddTHH:mm");
                tbIngresoServidor.Text = tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-ddTHH:mm");
                tbIngresoServidor.Text = FechaIngresoFormat.ToString("yyyy-MM-ddTHH:mm"); // valor para manejar en el servidor 
                tbCelular.Text = Celular;
                tbMail.Text = Mail;
                tbTelefono.Text = Telefono;
                tbPlano.Text = Plano;
                tbImagenes.Text = Imagenes;
                txAreaRender.InnerText = Areas;
                txObsVentas.InnerText = ObsVentas;
                txSegPausas.InnerText = SegPausa;
                txObsDibujo.InnerText = Observacion_Dibujo;
                tbLinea.Text = Linea;
                tbSup.Text = Superficie;
                tbAcc.Text = Accesorios;
                tbCantos.Text = Cantos;
                tbPaneles.Text = Paneles;
                tbPerfil.Text = Perfileria;
                tbArch.Text = Archivadores;
                tbSillas.Text = Sillas;
                tbPantallas.Text = Pantallas;
                if (EspArquitectonico == "True")
                {
                    chxEspArq.Checked = true;
                }
                else
                {
                    chxEspArq.Checked = false;
                }
                txMuebles.InnerText = Muebles;
                tbAcaPisZoc.Text = PisoyZocalo;
                tbAcaMuros.Text = Muros;
                tbIluminacion.Text = Iluminacion;
                tbAntepecho.Text = Antepecho;

                if (Ambientacion == "True")
                {
                    chxAmbientacion.Checked = true;
                }
                else
                {
                    chxAmbientacion.Checked = false;
                }

                if (Animacion == "True")
                {
                    chxAnimacion.Checked = true;
                }
                else
                {
                    chxAnimacion.Checked = false;
                }
                tbTerminadoVentas.Text = TerminadoVentas;
                tbTerminadoDibujo.Text = TerminadoDibujo;

                // Mantener Deshabilitado ddlAsesor y ddZona
                ddlAsesor.Enabled = false;
                ddlAsesor.CssClass = "form-control disabled";

                ddlZona.Enabled = false;
                ddlZona.CssClass = "form-control disabled";


                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                tbId_Fila.Text = rowIndex.ToString();

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {

                    if (TerminadoVentas == "True")
                    {
                        btnProgramarRender.Enabled = false;
                        btnProgramarRender.CssClass = "btn btn btn-warning";

                    }
                    else
                    {
                        btnProgramarRender.Enabled = true;
                        btnProgramarRender.CssClass = "btn btn btn-warning";

                    }

                    // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                    string script = "<script>HabilitarEnlaces1();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);


                    // Se activa el tap de Render 
                    ScriptManager.RegisterStartupScript(this, GetType(), "ActivateTab", "activarTab('Render-Content');", true);

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {

                    // Se compara si el click es en la misma fila con el id del Render
                    if (row.Cells[2].Text == Session["ID_Render1"]?.ToString())
                    {
                        // Incrementar la variable de sesión "ClickCount" en el servidor
                        int clickCount = Convert.ToInt32(Session["ClickCountRender"]) + 1;
                        Session["ClickCountRender"] = clickCount;

                        // se valida si es el segundo click en la misma fila 
                        if (clickCount == 2)
                        {

                            // Se activa el tap de Render 
                            ScriptManager.RegisterStartupScript(this, GetType(), "ActivateTab", "activarTab('Render-Content');", true);

                            // Reiniciar la variable de sesión "ClickCountRender" a 0 para la próxima interacción                        
                            Session.Remove("ID_Render1");
                            Session.Remove("ClickCountRender");

                        }

                    }
                    else
                    {
                        btnTrabajarRender.Enabled = true;
                        btnTrabajarRender.CssClass = "btn btn btn-outline-primary";


                        btnDesprogramarRender.Enabled = true;
                        btnDesprogramarRender.CssClass = "btn btn btn-outline-primary";


                        btnProgramarRender.Enabled = true;
                        btnProgramarRender.CssClass = "btn btn btn-warning";




                        if (TerminadoVentas == "True")
                        {

                            // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                            string script = "<script>HabilitarEnlacesDibujo1();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlacesDibujo1", script, false);


                            if (pausado == "True")
                            {
                                // activa  boton para despausar y deshabilirar el boton de pausar 
                                string script1 = "<script>HabEnlRenderPausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }
                            else
                            {
                                // poner el boton de pausar
                                string script1 = "<script>HabEnlRenderPausado1();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }

                        }
                        else
                        {
                            // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                            string script = "<script>HabilitarEnlacesDibujo2();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlacesDibujo1", script, false);

                            if (pausado == "True")
                            {
                                // Control del boton despausar                                
                                string script1 = "<script>HabEnlRenderPausado2();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }
                            else
                            {
                                // Control del boton pausar 
                                string script1 = "<script>HabEnlRenderPausado1();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }
                        }


                        // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                        Session["ClickCountRender"] = 1;
                        Session["ID_Render1"] = row.Cells[2].Text;
                    }

                }

            }
        }

        protected void ConsultarRender(object sender, EventArgs e)
        {

            // Validamos si el campo esta vacio para ejecurar un sqldatasource sino usamoos el otr 


            if (FechaIni.Text != "" && FechaFin.Text != "" && tbNumeroRender.Text != "")
            {
                BuscarRender.DataSourceID = "RenderXIdRender";
                BuscarRender.DataBind();
            }
            else if (FechaIni.Text != "" && FechaFin.Text != "" && tbClienteX.Text != "")
            {
                BuscarRender.DataSourceID = "RenderCliente";
                BuscarRender.DataBind();
            }
            else if (FechaIni.Text != "" && FechaFin.Text != "" && tbProyectoX.Text != "")
            {
                BuscarRender.DataSourceID = "RenderNombreRender";
                BuscarRender.DataBind();
            }
            else if (FechaIni.Text != "" && FechaFin.Text != "")
            {
                BuscarRender.DataSourceID = "RenderFecha";
                BuscarRender.DataBind();
            }
            else if (FechaIni.Text != "" && FechaFin.Text != "")
            {
                mensaje = "No has elegido un medio de búsqueda.";
                string script = "<script>AlertaBuscar('" + mensaje + "');</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertaBuscar", script, false);
            }
            else
            {
                mensaje = "Por favor, seleccione una fecha de búsqueda.";
                string script = "<script>AlertaBuscar('" + mensaje + "');</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertaBuscar", script, false);

            }

        }

        protected void GuardarModificarRender(object sender, EventArgs e)
        {

            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {
                if (Session["InsertUpdateRender"].ToString() == "Insertar")
                {
                    //insercion 

                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                    DateTime fechaIngreso;
                    if (!DateTime.TryParse(tbIngresoServidor.Text, out fechaIngreso))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('La fecha ingresada no es valida .');", true);
                        return;
                    }

                    DateTime fechaEntrega;
                    if (!DateTime.TryParse(tbEntregaServidor.Text, out fechaEntrega))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError1", "alert('La fecha de ingreso  no es valida .');", true);
                        return;
                    }

                    DateTime fechaUltimaActivacion;
                    if (!DateTime.TryParse(tbUltActivServidor.Text, out fechaUltimaActivacion))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError2", "alert('La fecha de Ultima Activacion  no es valida .');", true);
                        return;
                    }

                    DateTime fechaOk;
                    if (!DateTime.TryParse(tbFechaOkServidor.Text, out fechaOk))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError3", "alert('La fecha Ok no es valida .');", true);
                        return;
                    }
                    if (!IsValidEmail(tbMail.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError4", "alert('El formato del correo electrónico no es válido.');", true);
                        return;
                    }



                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        using (SqlCommand cmd = new SqlCommand("sp_InsertarRender", connection))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;


                            cmd.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);
                            cmd.Parameters.AddWithValue("@Fecha_Programada_Entrega", fechaEntrega);
                            cmd.Parameters.AddWithValue("@UltimaActivacion", fechaUltimaActivacion);
                            cmd.Parameters.AddWithValue("@FechaRenderOk", fechaOk);
                            cmd.Parameters.AddWithValue("@Numero_Diseño", tbDiseño.Text);
                            cmd.Parameters.AddWithValue("@Cliente", tbCliente.Text);
                            cmd.Parameters.AddWithValue("@Asesor", ddlAsesor.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@Nombre_Render", tbProyecto.Text);
                            cmd.Parameters.AddWithValue("@Contacto", tbContacto.Text);
                            cmd.Parameters.AddWithValue("@Celular", tbCelular.Text);
                            cmd.Parameters.AddWithValue("@Mail", tbMail.Text);
                            cmd.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                            cmd.Parameters.AddWithValue("@Plano", tbPlano.Text);
                            cmd.Parameters.AddWithValue("@Areas", txAreaRender.Value);
                            cmd.Parameters.AddWithValue("@Observaciones_Ventas", txObsVentas.Value);
                            cmd.Parameters.AddWithValue("@Linea", tbLinea.Text);
                            cmd.Parameters.AddWithValue("@AcabadoSuperficie", tbSup.Text);
                            cmd.Parameters.AddWithValue("@AcabadoAccesorios", tbAcc.Text);

                            cmd.Parameters.AddWithValue("@AcabadoPaneles", tbPaneles.Text);
                            cmd.Parameters.AddWithValue("@AcabadoPerfileria", tbPerfil.Text);
                            cmd.Parameters.AddWithValue("@Sillas", tbSillas.Text);
                            cmd.Parameters.AddWithValue("@Archivadores", tbArch.Text);
                            cmd.Parameters.AddWithValue("@Ambientacion", chxAmbientacion.Checked);
                            cmd.Parameters.AddWithValue("@Animacion", chxAnimacion.Checked);
                            cmd.Parameters.AddWithValue("@EspacioArquitectonico", chxConvenciones.Checked);
                            cmd.Parameters.AddWithValue("@PisoyZocalo", tbAcaPisZoc.Text);
                            cmd.Parameters.AddWithValue("@Muros", tbAcaMuros.Text);
                            cmd.Parameters.AddWithValue("@Iluminacion", tbIluminacion.Text);
                            cmd.Parameters.AddWithValue("@Sillar", tbAntepecho.Text);
                            cmd.Parameters.AddWithValue("@Imagenes", tbImagenes.Text);
                            cmd.Parameters.AddWithValue("@Cantos", tbCantos.Text);
                            cmd.Parameters.AddWithValue("@Pantallas", tbPantallas.Text);
                            cmd.Parameters.AddWithValue("@Muebles", txMuebles.Value);
                            cmd.Parameters.AddWithValue("@Observacion_Dibujo", txObsDibujo.Value);
                            cmd.Parameters.AddWithValue("@Zona", ddlZona.SelectedItem.Text);

                            connection.Open();


                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                // Crear Variables de Session o Cookies para guardar los datos del guardado



                                Session["FecIngresoSession"] = tbIngresoServidor.Text;
                                Session["FechaUltActiv"] = tbUltActivServidor.Text;
                                Session["FecEntregaSession"] = tbEntregaServidor.Text;
                                Session["FechaOKSession"] = tbFechaOkServidor.Text;
                                Session["DiseñoSession"] = tbDiseño.Text;
                                Session["ClienteSession"] = tbCliente.Text;
                                Session["AsesorSession"] = ddlAsesor.SelectedItem.Text;
                                Session["ProyectoSession"] = tbProyecto.Text;
                                Session["ContactoSession"] = tbContacto.Text;
                                Session["CelularSession"] = tbCelular.Text;
                                Session["MailSession"] = tbMail.Text;
                                Session["TelefonoSession"] = tbMail.Text;
                                Session["PlanoSession"] = tbPlano.Text;
                                Session["ZonaSession"] = ddlZona.SelectedItem.Text;
                                Session["ImagenSession"] = tbImagenes.Text;
                                Session["AreaSession"] = txAreaRender.InnerText;
                                Session["ObVentaSession"] = txObsVentas.InnerText;
                                Session["LineaSession"] = tbLinea.Text;
                                Session["SupSession"] = tbSup.Text;
                                Session["AccSession"] = tbAcc.Text;
                                Session["CantosSession"] = tbCantos.Text;
                                Session["PerfilSession"] = tbPerfil.Text;
                                Session["PanelesSession"] = tbPaneles.Text;
                                Session["ArcSession"] = tbArch.Text;
                                Session["SillasSession"] = tbSillas.Text;
                                Session["PantallasSession"] = tbPantallas.Text;
                                Session["EspArqSession"] = chxEspArq.Checked;
                                Session["MuebleSession"] = txMuebles.InnerText;
                                Session["AcabPisSession"] = tbAcaPisZoc.Text;
                                Session["AcabMuroSession"] = tbAcaMuros.Text;
                                Session["IluSession"] = tbIluminacion.Text;
                                Session["AntSession"] = tbAntepecho.Text;
                                Session["AmbientacionSession"] = chxAmbientacion.Checked;
                                Session["AnimacionSession"] = chxAnimacion.Checked;
                                Session["NumeroRenderCargar"] = ConsultarNumeroRenderInsertado();

                                string mensajePersonalizado = "El Render ha sido ingresado con éxito";
                                string urlRedireccion = "Ventas/Render_Venta.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }
                            else
                            {
                                string mensajePersonalizado = "¡Ups! En Render no se ingresó correctamente.Por favor, comunicate con el Departamento Sistemas para obtener ayuda.";
                                string urlRedireccion = "Ventas/Render_Venta.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }




                        }
                    }


                }

                else if (Session["InsertUpdateRender"].ToString() == "Actualizar")
                {

                    if (ValidarAsesorRender())
                    {

                        string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                        DateTime fechaIngreso;
                        if (!DateTime.TryParse(tbIngresoServidor.Text, out fechaIngreso))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('La fecha ingresada no es valida .');", true);
                            return;
                        }

                        DateTime fechaEntrega;
                        if (!DateTime.TryParse(tbEntregaServidor.Text, out fechaEntrega))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError1", "alert('La fecha de ingreso  no es valida .');", true);
                            return;
                        }

                        DateTime fechaUltimaActivacion;
                        if (!DateTime.TryParse(tbUltActivServidor.Text, out fechaUltimaActivacion))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError2", "alert('La fecha de Ultima Activacion  no es valida .');", true);
                            return;
                        }

                        DateTime fechaOk;
                        if (!DateTime.TryParse(tbFechaOkServidor.Text, out fechaOk))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError3", "alert('La fecha Ok no es valida .');", true);
                            return;
                        }


                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {

                            using (SqlCommand cmd = new SqlCommand("sp_ActualizarRender", connection))
                            {
                                // Establecer el tipo de comando como procedimiento almacenado
                                cmd.CommandType = CommandType.StoredProcedure;

                                // Agregar los parámetros necesarios para la actualización
                                cmd.Parameters.Add("@IDRender", SqlDbType.Int).Value = NumeroRender.Text;
                                cmd.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);
                                cmd.Parameters.AddWithValue("@Fecha_Programada_Entrega", fechaEntrega);
                                cmd.Parameters.AddWithValue("@UltimaActivacion", fechaUltimaActivacion);
                                cmd.Parameters.AddWithValue("@FechaRenderOk", fechaOk);
                                cmd.Parameters.AddWithValue("@Numero_Diseño", tbDiseño.Text);
                                cmd.Parameters.AddWithValue("@Cliente", tbCliente.Text);
                                cmd.Parameters.AddWithValue("@Asesor", ddlAsesor.SelectedItem.Text);
                                cmd.Parameters.AddWithValue("@Nombre_Render", tbProyecto.Text);
                                cmd.Parameters.AddWithValue("@Contacto", tbContacto.Text);
                                cmd.Parameters.AddWithValue("@Celular", tbCelular.Text);
                                cmd.Parameters.AddWithValue("@Mail", tbMail.Text);
                                cmd.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                                cmd.Parameters.AddWithValue("@Plano", tbPlano.Text);
                                cmd.Parameters.AddWithValue("@Areas", txAreaRender.Value);
                                cmd.Parameters.AddWithValue("@Observaciones_Ventas", txObsVentas.Value);
                                cmd.Parameters.AddWithValue("@Linea", tbLinea.Text);
                                cmd.Parameters.AddWithValue("@AcabadoSuperficie", tbSup.Text);
                                cmd.Parameters.AddWithValue("@AcabadoAccesorios", tbAcc.Text);

                                cmd.Parameters.AddWithValue("@AcabadoPaneles", tbPaneles.Text);
                                cmd.Parameters.AddWithValue("@AcabadoPerfileria", tbPerfil.Text);
                                cmd.Parameters.AddWithValue("@Sillas", tbSillas.Text);
                                cmd.Parameters.AddWithValue("@Archivadores", tbArch.Text);
                                cmd.Parameters.AddWithValue("@Ambientacion", chxAmbientacion.Checked);
                                cmd.Parameters.AddWithValue("@Animacion", chxAnimacion.Checked);
                                cmd.Parameters.AddWithValue("@EspacioArquitectonico", chxConvenciones.Checked);
                                cmd.Parameters.AddWithValue("@PisoyZocalo", tbAcaPisZoc.Text);
                                cmd.Parameters.AddWithValue("@Muros", tbAcaMuros.Text);
                                cmd.Parameters.AddWithValue("@Iluminacion", tbIluminacion.Text);
                                cmd.Parameters.AddWithValue("@Sillar", tbAntepecho.Text);
                                cmd.Parameters.AddWithValue("@Imagenes", tbImagenes.Text);
                                cmd.Parameters.AddWithValue("@Cantos", tbCantos.Text);
                                cmd.Parameters.AddWithValue("@Pantallas", tbPantallas.Text);
                                cmd.Parameters.AddWithValue("@Muebles", txMuebles.Value);
                                cmd.Parameters.AddWithValue("@Observacion_Dibujo", txObsDibujo.Value);
                                cmd.Parameters.AddWithValue("@Zona", ddlZona.SelectedItem.Text);


                                connection.Open();
                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {


                                    Session["FecIngresoSession"] = tbIngresoServidor.Text;
                                    Session["FechaUltActiv"] = tbUltActivServidor.Text;
                                    Session["FecEntregaSession"] = tbEntregaServidor.Text;
                                    Session["FechaOKSession"] = tbFechaOkServidor.Text;
                                    Session["DiseñoSession"] = tbDiseño.Text;
                                    Session["ClienteSession"] = tbCliente.Text;
                                    Session["AsesorSession"] = ddlAsesor.SelectedItem.Text;
                                    Session["ProyectoSession"] = tbProyecto.Text;
                                    Session["ContactoSession"] = tbContacto.Text;
                                    Session["CelularSession"] = tbCelular.Text;
                                    Session["MailSession"] = tbMail.Text;
                                    Session["TelefonoSession"] = tbTelefono.Text;
                                    Session["PlanoSession"] = tbPlano.Text;
                                    Session["ZonaSession"] = ddlZona.SelectedItem.Text;
                                    Session["ImagenSession"] = tbImagenes.Text;
                                    Session["AreaSession"] = txAreaRender.InnerText;
                                    Session["ObVentaSession"] = txObsVentas.InnerText;
                                    Session["LineaSession"] = tbLinea.Text;
                                    Session["SupSession"] = tbSup.Text;
                                    Session["AccSession"] = tbAcc.Text;
                                    Session["CantosSession"] = tbCantos.Text;
                                    Session["PerfilSession"] = tbPerfil.Text;
                                    Session["PanelesSession"] = tbPaneles.Text;
                                    Session["ArcSession"] = tbArch.Text;
                                    Session["SillasSession"] = tbSillas.Text;
                                    Session["PantallasSession"] = tbPantallas.Text;
                                    Session["EspArqSession"] = chxEspArq.Checked;
                                    Session["MuebleSession"] = txMuebles.InnerText;
                                    Session["AcabPisSession"] = tbAcaPisZoc.Text;
                                    Session["AcabMuroSession"] = tbAcaMuros.Text;
                                    Session["IluSession"] = tbIluminacion.Text;
                                    Session["AntSession"] = tbAntepecho.Text;
                                    Session["AmbientacionSession"] = chxAmbientacion.Checked;
                                    Session["AnimacionSession"] = chxAnimacion.Checked;
                                    Session["NumeroRenderCargar"] = NumeroRender.Text;


                                    string mensajePersonalizado = "El Render ha sido Actualizado con éxito";
                                    string urlRedireccion = "Ventas/Render_Venta.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }
                                else
                                {
                                    string mensajePersonalizado = "¡El Render No ha sido Actualizado Correctamente, Intentelo Nuevamente!";
                                    string urlRedireccion = "Ventas/Render_Venta.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }

                            }

                        }
                    }
                    else
                    {
                        string mensajePersonalizado = "Solo el creador del render o alguien con los permisos necesarios puede modificarlo.";
                        string urlRedireccion = "Ventas/Render_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }


                }
                else
                {

                    // Se activo el Boton de guardar de alguina otra manera  y se debe mostrar la excepcion o la denegacion de pero
                    string mensajePersonalizado = "Ocurrió un error, por favor intentelo nuevamente";
                    string urlRedireccion = "Ventas/Render_Venta.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
            else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
            {
                if (ActualizarRenderDibujo())
                {

                    // control del tap del dibujante
                    Session["ActivarTapBitaRender"] = "1";

                    DataTable DatosReder = ConsultarInfoRender();

                    // Accede a la primera fila del DataTable
                    DataRow row = DatosReder.Rows[0];

                    // Obtén los valores de las columnas                   
                    bool programadoVentas = Convert.ToBoolean(row["ProgramadoVentas"]);
                    bool terminadoRender = Convert.ToBoolean(row["TerminadoRender"]);
                    bool pausado = Convert.ToBoolean(row["Pausado"]);

                    // VARIABLES EVALUACION Y CONTROL DIBUJANTE 

                    Session["terVenta"] = programadoVentas;
                    Session["terDibujo"] = terminadoRender;
                    Session["pausadoRender"] = pausado;



                    string mensajePersonalizado = "El render fue actualizado correctamente.";
                    string urlRedireccion = "Ventas/Render_Venta.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                }
                else
                {
                    string mensajePersonalizado = "Por favor intente nuevamente..";
                    string urlRedireccion = "Ventas/Render_Venta.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                }
            }

            //Pendiente Validaciones que el Render tenga un numero de Diseño Asociado 

        }

        private bool ActualizarRenderDibujo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblRender SET Observacion_Dibujo = @ObsDibujo WHERE Id_Render = @ID_Render  ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ObsDibujo", txObsDibujo.InnerText);
                    cmd.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if (CantidadFilasAfectada > 0)
                    {
                        DataTable DatosRender = ConsultarInfoRender();
                        DataRow row = DatosRender.Rows[0];

                        Session["FecIngresoSession"] = row["Fecha_Ingreso"].ToString();
                        Session["FechaUltActiv"] = row["UltimaActivacion"].ToString();
                        Session["FecEntregaSession"] = row["Fecha_Programada_Entrega"].ToString();
                        Session["FechaOKSession"] = row["FechaRenderOK"].ToString();
                        Session["DiseñoSession"] = row["Numero_Diseño"].ToString();
                        Session["ClienteSession"] = row["Cliente"].ToString();
                        Session["AsesorSession"] = row["Asesor"].ToString();
                        Session["ProyectoSession"] = row["Nombre_Render"].ToString();
                        Session["ContactoSession"] = row["Contacto"].ToString();
                        Session["CelularSession"] = row["Celular"].ToString();
                        Session["MailSession"] = row["Mail"].ToString();
                        Session["TelefonoSession"] = row["Telefono"].ToString();
                        Session["PlanoSession"] = row["Plano"].ToString();
                        Session["ZonaSession"] = row["Zona"].ToString();
                        Session["ImagenSession"] = row["Imagenes"].ToString();
                        Session["AreaSession"] = row["Areas"].ToString();
                        Session["ObVentaSession"] = row["Observaciones_Ventas"].ToString();
                        Session["LineaSession"] = row["Linea"].ToString();
                        Session["SupSession"] = row["AcabadoSuperficie"].ToString();
                        Session["AccSession"] = row["AcabadoAccesorios"].ToString();
                        Session["CantosSession"] = row["Cantos"].ToString();
                        Session["PerfilSession"] = row["AcabadoPerfileria"].ToString();
                        Session["PanelesSession"] = row["AcabadoPaneles"].ToString();
                        Session["ArcSession"] = row["Archivadores"].ToString();
                        Session["SillasSession"] = row["Sillas"].ToString();
                        Session["PantallasSession"] = row["Pantallas"].ToString();
                        Session["EspArqSession"] = row["EspacioArquitectonico"].ToString();
                        Session["MuebleSession"] = row["Muebles"].ToString();
                        Session["AcabPisSession"] = row["PisoyZocalo"].ToString();
                        Session["AcabMuroSession"] = row["Muros"].ToString();
                        Session["IluSession"] = row["Iluminacion"].ToString();
                        Session["AntSession"] = row["Sillar"].ToString();
                        Session["AmbientacionSession"] = row["Ambientacion"].ToString();
                        Session["AnimacionSession"] = row["Animacion"].ToString();
                        Session["NumeroRenderCargar"] = NumeroRender.Text;
                        Session["ObsDibujoSession"] = txObsDibujo.InnerText;
                        Session["SegPauSession"] = row["SeguimientoPausa"].ToString();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public string ConsultarNumeroRenderInsertado()
        {
            string maxID = "0";
            string query = "SELECT MAX(tblRender.ID_Render) FROM tblRender";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            maxID = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones (puedes registrar el error o manejarlo según tus necesidades)
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return maxID;
        }

        private bool ValidarAsesorRender()
        {
            bool ok = false;
            string query = "SELECT Asesor FROM tblRender where ID_Render = @Id_Render";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Render", NumeroRender.Text);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            string asesor = result.ToString();
                            string usuarioLogueado = Session["usuariologueado"]?.ToString();

                            if (!string.IsNullOrEmpty(usuarioLogueado) && asesor == usuarioLogueado)
                            {
                                ok = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return ok;
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, emailPattern);
        }

        [WebMethod] // Cambiar estado de variable de Session cuando dan click en NuevoRender
        public static void NuevoRender()
        {
            HttpContext.Current.Session["InsertUpdateRender"] = "Insertar";
        }

        [WebMethod] // Cambiar estado de variable de Session cuando dan click en NuevoRender 
        public static void ModificarRender()
        {
            HttpContext.Current.Session["InsertUpdateRender"] = "Actualizar";
        }

        [WebMethod]
        public static void EliminarTapActRender()
        {
            HttpContext.Current.Session["ActivarTapBitaRender"] = null;
            HttpContext.Current.Session["terVenta"] = null;
            HttpContext.Current.Session["terDibujo"] = null;
            HttpContext.Current.Session["pausadoRender"] = null;
 
        }


        protected void DataGridBuscarRender_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                // Aplica la clase "fila-clickeable" a todas las filas
                e.Item.CssClass += " fila-clickeable";


                // Obtener los valores de las columnas ocultas
                int programadoVentas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas"));
                int pausado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Pausado"));
                int terminadoDibujo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "TerminadoRender"));

                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));

                // Cambiar el color de fondo de la fila en función de los valores de las columnas
                if (terminadoDibujo == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#77a765"); //Verde
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (fechaProgramada <= DateTime.Now && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868");    //rojo 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (programadoVentas == 0)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#72459b");    //Morado 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else
                {
                    if (pausado == 1 && programadoVentas == 1)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#70ede4"); // Aqua
                    }

                    else
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#efdd79");//amarillo 
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    }

                }


            }



        }

        protected void DataGridBuscarRenders_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerRenders2")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = BuscarRender.Items[rowIndex];


                foreach (DataGridItem item in BuscarRender.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }


                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada";

                string IdRender = row.Cells[2].Text;
                string NomRender = row.Cells[43].Text.Replace("&nbsp;", "");
                string FechaUltimaActivacion = row.Cells[4].Text;
                DateTime FechaUltimaActivacionFormat = DateTime.Parse(FechaUltimaActivacion);
                string FechaEntrega = row.Cells[5].Text;
                DateTime FechaEntregaFormat = DateTime.Parse(FechaEntrega);
                string Asesor = row.Cells[6].Text;
                //Realizado por [7]
                string Zona = row.Cells[8].Text;
                string FechaRenderOK = row.Cells[9].Text;
                DateTime FechaRenderOKFormat = DateTime.Parse(FechaRenderOK);
                string NumeroDiseño = row.Cells[10].Text.Replace("&nbsp;", "");
                string NombreRender = row.Cells[11].Text.Replace("&nbsp;", "");
                string NombreContacto = row.Cells[12].Text.Replace("&nbsp;", "");
                string FechaIngreso = row.Cells[13].Text;
                DateTime FechaIngresoFormat = DateTime.Parse(FechaIngreso);
                string Celular = row.Cells[14].Text.Replace("&nbsp;", "");
                string Mail = row.Cells[15].Text.Replace("&nbsp;", "");
                string Telefono = row.Cells[16].Text.Replace("&nbsp;", "");
                string Plano = row.Cells[17].Text.Replace("&nbsp;", "");

                string Imagenes = row.Cells[18].Text.Replace("&nbsp;", "");
                string Areas = row.Cells[19].Text.Replace("&nbsp;", "");
                string ObsVentas = row.Cells[20].Text.Replace("&nbsp;", "");
                string SegPausa = row.Cells[21].Text.Replace("&nbsp;", "");
                string Observacion_Dibujo = row.Cells[22].Text.Replace("&nbsp;", "");
                string Linea = row.Cells[23].Text.Replace("&nbsp;", "");
                string Superficie = row.Cells[24].Text.Replace("&nbsp;", "");
                string Accesorios = row.Cells[25].Text.Replace("&nbsp;", "");
                string Cantos = row.Cells[26].Text.Replace("&nbsp;", "");
                string Perfileria = row.Cells[27].Text.Replace("&nbsp;", "");
                string Paneles = row.Cells[28].Text.Replace("&nbsp;", "");
                string Archivadores = row.Cells[29].Text.Replace("&nbsp;", "");
                string Sillas = row.Cells[30].Text.Replace("&nbsp;", "");
                string Pantallas = row.Cells[31].Text.Replace("&nbsp;", "");
                string EspArquitectonico = row.Cells[32].Text;
                string Muebles = row.Cells[33].Text.Replace("&nbsp;", "");
                string PisoyZocalo = row.Cells[34].Text.Replace("&nbsp;", "");
                string Muros = row.Cells[35].Text.Replace("&nbsp;", "");
                string Iluminacion = row.Cells[36].Text.Replace("&nbsp;", "");
                string Antepecho = row.Cells[37].Text.Replace("&nbsp;", "");
                string Ambientacion = row.Cells[38].Text;
                string Animacion = row.Cells[39].Text;
                string TerminadoVentas = row.Cells[40].Text;
                string pausado = row.Cells[41].Text;
                Session["Pausado"] = pausado;
                string TerminadoDibujo = row.Cells[42].Text;

                NumeroRender.Text = IdRender;
                tbCliente.Text = NomRender;
                tbUltActiv.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-ddTHH:mm");
                tbUltActivServidor.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-ddTHH:mm");
                tbEntrega.Text = FechaEntregaFormat.ToString("yyyy-MM-ddTHH:mm");
                tbEntregaServidor.Text = FechaEntregaFormat.ToString("yyyy-MM-ddTHH:mm");
                string nombreBuscado = Asesor; // El nombre que deseas buscar
                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == nombreBuscado)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true; // Selecciona el elemento si se encuentra
                        break; // Rompe el bucle una vez que se encuentra una coincidencia
                    }
                }

                string ZonaBus = Zona; // El nombre que deseas buscar
                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == ZonaBus)
                    {
                        ddlZona.ClearSelection();
                        item.Selected = true; // Selecciona el elemento si se encuentra
                        break; // Rompe el bucle una vez que se encuentra una coincidencia
                    }
                }
                tbFechaOk.Text = FechaRenderOKFormat.ToString("yyyy-MM-ddTHH:mm");
                tbFechaOkServidor.Text = FechaRenderOKFormat.ToString("yyyy-MM-dd");
                tbDiseño.Text = NumeroDiseño;
                tbProyecto.Text = NombreRender;
                tbContacto.Text = NombreContacto;
                tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-ddTHH:mm");
                tbIngresoServidor.Text = tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-ddTHH:mm");
                tbIngresoServidor.Text = FechaIngresoFormat.ToString("yyyy-MM-ddTHH:mm"); // valor para manejar en el servidor 
                tbCelular.Text = Celular;
                tbMail.Text = Mail;
                tbTelefono.Text = Telefono;
                tbPlano.Text = Plano;
                tbImagenes.Text = Imagenes;
                txAreaRender.InnerText = Areas;
                txObsVentas.InnerText = ObsVentas;
                txSegPausas.InnerText = SegPausa;
                txObsDibujo.InnerText = Observacion_Dibujo;
                tbLinea.Text = Linea;
                tbSup.Text = Superficie;
                tbAcc.Text = Accesorios;
                tbCantos.Text = Cantos;
                tbPaneles.Text = Paneles;
                tbPerfil.Text = Perfileria;
                tbArch.Text = Archivadores;
                tbSillas.Text = Sillas;
                tbPantallas.Text = Pantallas;
                if (EspArquitectonico == "True")
                {
                    chxEspArq.Checked = true;
                }
                else
                {
                    chxEspArq.Checked = false;
                }
                txMuebles.InnerText = Muebles;
                tbAcaPisZoc.Text = PisoyZocalo;
                tbAcaMuros.Text = Muros;
                tbIluminacion.Text = Iluminacion;
                tbAntepecho.Text = Antepecho;

                if (Ambientacion == "True")
                {
                    chxAmbientacion.Checked = true;
                }
                else
                {
                    chxAmbientacion.Checked = false;
                }

                if (Animacion == "True")
                {
                    chxAnimacion.Checked = true;
                }
                else
                {
                    chxAnimacion.Checked = false;
                }

                tbTerminadoVentas.Text = TerminadoVentas;
                tbTerminadoDibujo.Text = TerminadoDibujo;

                // Mantener Deshabilitado ddlAsesor y ddZona
                ddlAsesor.Enabled = false;
                ddlAsesor.CssClass = "form-control disabled";

                ddlZona.Enabled = false;
                ddlZona.CssClass = "form-control disabled";


                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {

                    if (TerminadoVentas == "True")
                    {
                        btnProgramarRender.Enabled = false;
                        btnProgramarRender.CssClass = "btn btn btn-warning";

                    }
                    else
                    {
                        btnProgramarRender.Enabled = true;
                        btnProgramarRender.CssClass = "btn btn btn-warning";

                    }

                    // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                    string script = "<script>HabilitarEnlaces1();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {

                    if (TerminadoDibujo != "True")
                    {
                        btnTrabajarRender.Enabled = true;
                        btnTrabajarRender.CssClass = "btn btn btn-outline-primary";


                        btnDesprogramarRender.Enabled = true;
                        btnDesprogramarRender.CssClass = "btn btn btn-outline-primary";


                        btnProgramarRender.Enabled = true;
                        btnProgramarRender.CssClass = "btn btn btn-warning";

                        // Control de botones para renders no terminados por dibujo 
                        string script = "<script>HabilitarEnlacesDibujo1();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlacesDibujo1", script, false);



                        if (TerminadoVentas == "True")
                        {
                            if (pausado == "True")
                            {
                                // activa  boton para despausar y deshabilirar el boton de pausar 
                                string script1 = "<script>HabEnlRenderPausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }
                            else
                            {
                                // poner el boton de pausar
                                string script1 = "<script>HabEnlRenderPausado1();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }

                        }
                        else
                        {
                            if (pausado == "True")
                            {
                                // Control del boton despausar                                
                                string script1 = "<script>HabEnlRenderPausado2();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }
                            else
                            {
                                // Control del boton pausar 
                                string script1 = "<script>HabEnlRenderPausado1();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlRenderPausado", script1, false);
                            }
                        }



                    }
                    else
                    {
                        btnTrabajarRender.Enabled = false;
                        btnTrabajarRender.CssClass = "btn btn btn-outline-secondary";


                        btnDesprogramarRender.Enabled = false;
                        btnDesprogramarRender.CssClass = "btn btn btn-outline-secondary";


                        btnProgramarRender.Enabled = false;
                        btnProgramarRender.CssClass = "btn btn btn-warning";

                        // Control de los botones de dibujo para render terminados por dibujo Modificar , Pausar, Devolver 

                        // Control de botones para renders no terminados por dibujo 
                        string script = "<script>HabilitarEnlacesDibujo3();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlacesDibujo1", script, false);
                    }


                }

            }
        }


        // Programar o Terminar Renders 
        protected void ProgramarRender(object sender, EventArgs e)
        {

            switch (Session["Departamento"].ToString().ToUpper())
            {
                case "VENTAS":

                    if (Session["Pausado"]?.ToString() == "True")
                    {
                        string mensajePersonalizado = "Este render esta pausado no se puede programar.";
                        string urlRedireccion = "Ventas/Render_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        
                        return;
                    }

                    DateTime FechaIngresoRender = DateTime.Now;
                    DateTime UltimaActivacionRender = FechaIngresoRender;


                    while (UltimaActivacionRender.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionRender.DayOfWeek == DayOfWeek.Sunday)
                    {
                        UltimaActivacionRender = UltimaActivacionRender.AddDays(1);
                        UltimaActivacionRender = new DateTime(UltimaActivacionRender.Year, UltimaActivacionRender.Month, UltimaActivacionRender.Day, 8, 0, 0);
                    }


                    if (chxAnimacion.Checked)
                    {
                        FechaIngresoRender = UltimaActivacionRender.AddDays(1);

                    }
                    DateTime FechaEntrega = CalcularFechaEntrega(FechaIngresoRender);

                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        //Realizamos la Actualizacion 
                        string query = "UPDATE tblRender SET ProgramadoVentas = 1, Fecha_Ingreso = @FechaIngreso, UltimaActivacion = @UltimaActivacion," +
                            " Fecha_Programada_Entrega = @FechaProgramadaEntrega WHERE Id_Render = @IdRender";


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Aquí defines los parámetros de la consulta
                            command.Parameters.AddWithValue("@FechaIngreso", FechaIngresoRender);
                            command.Parameters.AddWithValue("@UltimaActivacion", UltimaActivacionRender);
                            command.Parameters.AddWithValue("@FechaProgramadaEntrega", FechaEntrega);
                            command.Parameters.AddWithValue("@IdRender", NumeroRender.Text);

                            command.ExecuteNonQuery();
                        }

                        // Mensaje de éxito
                        string mensajeExito = "El Render " + tbProyecto.Text.Trim() + " ha sido programado exitosamente.";
                        string scriptExito = "alert('" + mensajeExito + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);


                    }



                    break;

                case "DESARROLLO DE PRODUCTO":// Boton Programar  Departamento Compras Desarrollo Producto
                case "DISEÑO":

                    // Realizar el update de terminar el render

                    if (MetodoTerminarRenderDibujo())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#AdjuntarDocYTerminar').modal('show');", true);
                    }
                    else
                    {
                        // Mensaje Ocurrió un problema al terminar el render, por  favor intentelo nuevamente.
                        string mensajePersonalizado = "Ocurrió un problema al terminar el render, por  favor intentelo nuevamente.";
                        string urlRedireccion = "Ventas/Render_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }

                    break;


                default:
                    // Error Con el despartamento de ese usuario Validar con Sistemas 
                    break;
            }



        }

        // Programar Render Dibujo y Adjuntar Documentos 
        protected void btnAdjuntarYTerminarRender_Click(object sender, EventArgs e)
        {
            // Ruta de destino en el servidor apuntar a la ruta de temporales en el servidor 3
            string serverPath = @"\\SRVFS\S_I_Ducon$\TemporalAdjunto";

            // Numero de render para guardar los archivos con numero de render 
            string NumRender = NumeroRender.Text;

            string Adjuntos = "";

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {

                        string fileName = "Render_" + NumRender + "_" + file.FileName;
                        string savePath = Path.Combine(serverPath, fileName);
                        Adjuntos = Adjuntos + ";" + savePath;
                        file.SaveAs(savePath);

                    }
                }


            }

            // Contruimos el Correo de Terminar Render para dibujo 

            // Enviar el correo electronico 
            string correoEmisor = ConsultarCorreoEmisor();
            string correoAsesor = ConsultarCorreoAsesorRender();

            DataTable DatosRender = ConsultarInfoRender();
            DataRow row = DatosRender.Rows[0];

            string destinatarios = correoEmisor + ";" + correoAsesor;
            string cuerpo = @"
                    <!DOCTYPE html>
                    <html lang='es'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                line-height: 1.6;
                                margin: 0;
                                padding: 0;
                                background-color: #f9f9f9;
                            }
                            .container {
                                max-width: 40rem;
                                margin: 20px auto;
                                padding: 20px;
                                border: 1px solid #ccc;
                                border-radius: 5px;
                                background-color: #fff;
                            }
                            h2 {
                                color: #333;
                                font-size: 24px;
                                margin-bottom: 20px;
                            }
                            p {
                                margin-bottom: 10px;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h3>Render Terminado </h3>
                             <p>Estimado(a) asesor(a), por medio de la presente se informa que el Render: " + NumeroRender.Text + ", ha sido terminado, bajo los siguientes parametros:" + @"</p>
                             <p><strong>Fecha Ingreso </strong> " + row["Fecha_Ingreso"].ToString() + "<strong> Fecha Ok Render </strong>" + row["FechaRenderOK"].ToString() + @"</p>
                             <p><strong>Cliente </strong> " + row["Cliente"].ToString() + "<strong> Contacto: </strong> " + row["Contacto"].ToString() + @"</p>
                             <p><strong>Plano </strong> " + row["Plano"].ToString() + @"</p>
                             <p><strong>Areas a renderizar: </strong> " + row["Areas"].ToString() + @"</p>
                             <p><strong>Obs. Ventas: </strong> " + row["Observaciones_Ventas"].ToString() + @"</p>
                             <p><strong>Realizada Por: </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>     
                             <p><strong>Observaciones Dibujo y Despiece: </strong> " + row["Observacion_Dibujo"].ToString() + @"</p>
                             <h3>Acabados </h3>
                             <p><strong>Linea: </strong> " + row["Linea"].ToString() + @" </p>
                             <p><strong>Superficies: </strong> " + row["AcabadoSuperficie"].ToString() + @" </p>
                             <p><strong>Accesorios: </strong> " + row["AcabadoAccesorios"].ToString() + @" </p>
                             <p><strong>Paneles: </strong> " + row["AcabadoPaneles"].ToString() + @" </p>
                             <p><strong>Perfileria: </strong> " + row["AcabadoPerfileria"].ToString() + @"</p>
                             <p><strong>Sillas: </strong> " + row["Sillas"].ToString() + @"</p>
                             <p><strong>Archivadores: </strong> " + row["Archivadores"].ToString() + @"</p>
                             <p><strong>Piso y Zocalo: </strong> " + row["PisoyZocalo"].ToString() + @"</p>
                             <p><strong>Muros: </strong> " + row["Muros"].ToString() + @"</p>              
                             <p><strong>Iluminación y tipo de Lamparas:: </strong> " + row["Iluminacion"].ToString() + @"</p>
                             <p><strong>Sillar y Antepecho: </strong> " + row["Sillar"].ToString() + @"</p>
                             <p><strong>Imagenes: </strong> " + row["Imagenes"].ToString() + @"</p>
                             <p><strong>Cualquier inquietud no dude en comunicarse con: </strong> " + Session["usuariologueado"].ToString() + @"</p>
                        </div>
                    </body>
                    </html>";

            string asunto = "Render Terminado " + NumeroRender.Text + "-" + row["Cliente"].ToString();



            if (EnviarCorreoRenderTerminadoDibujo(destinatarios, cuerpo, Adjuntos, asunto))
            {
                //Mensaje de exito Render terminado y notificado
                string mensajePersonalizado = "El render fue terminado y notificado exitosamente.";
                string urlRedireccion = "Ventas/Render_Venta.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
            else
            {
                // Mensjae de error  Render Terminado pero no notificado ( validar confirmacion) 
                string mensajePersonalizado = "El render fue terminado, se cargaron los documentos y se notificó correctamente.";
                string urlRedireccion = "Ventas/Render_Venta.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }

        }
        public bool EnviarCorreoRenderTerminadoDibujo(string destinatarios, string cuerpo, string adjuntos,string asunto)
        {
            string nombreProcedimiento = "duc_sp_Correo";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(nombreProcedimiento, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Definir los parámetros del procedimiento almacenado
                        command.Parameters.AddWithValue("@Destinatarios", destinatarios.TrimEnd(';'));
                        command.Parameters.AddWithValue("@asunto", asunto);
                        command.Parameters.AddWithValue("@cuerpo", cuerpo);
                        command.Parameters.AddWithValue("@adjuntos", adjuntos.TrimStart(';').TrimEnd(';'));
                        command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejar la excepción (opcional)
                // Loggear la excepción o hacer algo con ella
                return false;
            }
        }
        private DataTable ConsultarInfoRender()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblRender WHERE Id_Render = @ID_Render";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    
                    cmd.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private bool MetodoTerminarRenderDibujo()
        {

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblRender SET RealizadoPor = @RealizadoPor,ProgramadoVentas =1," +
                          "TerminadoRender =1, FechaRenderOk= @fechaOk  WHERE Id_Render = @ID_Render  ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@RealizadoPor", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@fechaOk", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if (CantidadFilasAfectada > 0)
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



        //Metodos para Programar la solicitud  Ventas 
        public DateTime CalcularFechaEntrega(DateTime FechaIngreso)
        {
            DateTime UltimaActivacionRender = FechaIngreso;
            DateTime FechaEntrega = DateTime.Now;

            //Se valida  si ingresan la solicitud un dia sabado o domingo 
            while (UltimaActivacionRender.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionRender.DayOfWeek == DayOfWeek.Sunday)
            {
                UltimaActivacionRender = UltimaActivacionRender.AddDays(1);
                UltimaActivacionRender = new DateTime(UltimaActivacionRender.Year, UltimaActivacionRender.Month, UltimaActivacionRender.Day, 8, 0, 0);
            }

            FechaEntrega = SumarDiaLaboral(UltimaActivacionRender, 4);



            return FechaEntrega;
        }
        private DateTime SumarDiaLaboral(DateTime fecha, int CantDias)
        {
            int diaHabilAdd = 0;

            while (diaHabilAdd < CantDias)
            {
                // sumamos un dia  a la fecha inicial 
                fecha = fecha.AddDays(1);

                // Verificar si el día actual no es sábado ni domingo
                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                {
                    // Se consulta si es un dia fectivo 
                    bool festivo = ConsultarDiaFestivo(fecha);

                    if (!festivo)
                    {
                        // Si es un día hábil y no es un día no laboral, se incrementa diaHabilAdd
                        diaHabilAdd++;
                    }

                }
            }

            return fecha;
        }
        private bool ConsultarDiaFestivo(DateTime fecha)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();

                string consulta = "SELECT COUNT(*) FROM tblDiaNoLaboral WHERE dnlFecha = @Fecha";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@Fecha", fecha.Date);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }



        // programar dibujante para render 
        protected void btnTrabajarRender_Click(object sender, EventArgs e)
        {

            Session["ClickCountRender"] = 0;
            NumRender1.InnerText = NumeroRender.Text;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalConRender').modal('show');", true);

        }
        protected void btnTrabajarRender_SI_Click(object sender, EventArgs e)
        {
            // Asignar Renderizador
            ProgramarDibujanteRender(NumeroRender.Text);
            DataGridRenders.DataBind();

            int rowIndex = Convert.ToInt32(tbId_Fila.Text);
            string script = $"SeleccionarFilayEnfocarRender({rowIndex});";
            ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarRender", script, true);

        }
        private void ProgramarDibujanteRender(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblRender set RealizadoPor = @NombreUsuario where Id_Render= @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@NombreUsuario", Session["usuariologueado"].ToString());

                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Desprogramar dibujante para render 
        protected void btnDesprogramarRender_Click(object sender, EventArgs e)
        {

            Session["ClickCountRender"] = 0;

            NumRender2.InnerText = NumeroRender.Text;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ConfirDespRender').modal('show');", true);
        }
        protected void btnDesprogramarRender_SI_Click(object sender, EventArgs e)
        {
            // Quitar  Renderizador 
            DesprogramarDibujantRender(NumeroRender.Text);
            DataGridRenders.DataBind();

            int rowIndex = Convert.ToInt32(tbId_Fila.Text);
            string script = $"SeleccionarFilayEnfocarRender({rowIndex});";
            ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarRender", script, true);
        }
        private void DesprogramarDibujantRender(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblRender set RealizadoPor = 'PENDIENTE' where Id_Render= @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ID", ID);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Eliminar   render 
        protected void btnEliminarRender_SI_Click(object sender, EventArgs e)
        {

            if (EliminarRenderMetodo())
            {
                //Mensaje de Exito 

                string mensajePersonalizado = "El Render ha sido eliminado con éxito";
                string urlRedireccion = "Ventas/Render_Venta.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
            else
            {
                //Mensaje de Error
                string mensajePersonalizado = "El Render no se pudo eliminar , por favor intenlo nuevamente";
                string urlRedireccion = "Ventas/Render_Venta.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
        }
        private bool EliminarRenderMetodo()
        {
            string query = "DELETE FROM tblRender WHERE Id_Render = @ID_Render";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            try
            {
                // Crea una conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abre la conexión a la base de datos
                    connection.Open();

                    // Crea un comando SQL para ejecutar la consulta DELETE
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Añade el parámetro al comando
                        cmd.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                        // Ejecuta el comando y obtiene el número de filas afectadas
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        // Si se afectó al menos una fila, la eliminación fue exitosa
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en fallo de eliminacion 
                return false;
            }
        }


        // Pausar Render
        protected void btnPausarRender_Si_Click(object sender, EventArgs e)
        {
            if (PausarRenderDibujo())
            {

                DataGridRenders.DataBind();
                int index = Convert.ToInt32(tbId_Fila.Text); // Ajusta el índice según sea necesario
                DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                    DataGridRenders.Items[index],
                    DataGridRenders,
                    new CommandEventArgs("VerRenders", index)
                );
                DataGridRenders_LinkButton(DataGridRenders, args);


                // Enviar el correo electronico 
                string correoEmisor = ConsultarCorreoEmisor();
                string correoAsesor = ConsultarCorreoAsesorRender();
                string correoPordefecto = ConsultarCorreoTerminadoDiseño();

                string destinatarios = correoEmisor + ";" + correoAsesor + ";" + correoPordefecto;


                string cuerpo = @"
                    <!DOCTYPE html>
                    <html lang='es'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                line-height: 1.6;
                                margin: 0;
                                padding: 0;
                                background-color: #f9f9f9;
                            }
                            .container {
                                max-width: 40rem;
                                margin: 20px auto;
                                padding: 20px;
                                border: 1px solid #ccc;
                                border-radius: 5px;
                                background-color: #fff;
                            }
                            h2 {
                                color: #333;
                                font-size: 24px;
                                margin-bottom: 20px;
                            }
                            p {
                                margin-bottom: 10px;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h3>Render pausado </h3>
                            <p>Estimado(a) asesor(a)</p>
                             <p><strong> Su solicitud de Render ha sido pausada por : </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>
                            <p> <strong> Fecha: </strong> " + DateTime.Now.ToString() + @"</p>                                                     
                            <p><strong>Render N°: </strong> " + NumeroRender.Text + @"</p>
                            <p><strong>Cliente: </strong> " + tbCliente.Text + @"</p>
                            <p><strong>Proyecto: </strong> " + tbProyecto.Text + @"</p>
                            <p><strong>Observación dibujo: </strong> " + txObsDibujo.InnerText + @"</p>
                            <p><strong>Historial de pausas y devoluciones: </strong> " + txSegPausas.InnerText + @"</p>
                            <p><strong>Cualquier inquietud no dude en comunicarse con: </strong> " + Session["usuariologueado"].ToString() + @"</p>
                        </div>
                    </body>
                    </html>";



                if (EnviarCorreoRenderPausado(destinatarios, cuerpo))
                {
                    string mensajeExito = "El render se pausó con exito y fue notificado por correo electronico";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }
                else
                {
                    string mensajeExito = "El render se pausó con exito, pero hubo problemas para notificar por correo electronico";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }

            }
            else
            {


                string mensajeExito = "Hubo un problema al pausar el render , por favor intentelo mas nuevamente ";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                int index = Convert.ToInt32(tbId_Fila.Text); // Ajusta el índice según sea necesario
                DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                    DataGridRenders.Items[index],
                    DataGridRenders,
                    new CommandEventArgs("VerRenders", index)
                );
                DataGridRenders_LinkButton(DataGridRenders, args);


            }

        }
        private bool PausarRenderDibujo()
        {
            DateTime fecha = DateTime.Now;
            string SegPausa = "(Render pausado por el dibujante: " + Session["usuariologueado"].ToString() + "el " + fecha + "\n Razón: " + txJusticiacionPausaRender.InnerText + ")";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblRender SET Pausado = 1,  SeguimientoPausa = @seguimientoPausa WHERE Id_Render = @ID_Render  ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@seguimientoPausa", SegPausa);
                    cmd.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if (CantidadFilasAfectada > 0)
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
        public string ConsultarCorreoAsesorRender()
        {
            string correo = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT E.Mail  FROM tblRender AS R INNER JOIN tblEmpleado AS E " +
                               "ON  E.Nombre + ' ' + Apellidos = R.Asesor WHERE R.Id_Render = @ID_Render ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);
                    try
                    {
                        connection.Open();
                        correo = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción 
                        //Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }

            return correo;
        }
        public string ConsultarCorreoEmisor()
        {
            string correo = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT mail FROM tblEmpleado WHERE Cedula = @cedulalogueada";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@cedulalogueada", Session["CedulaLogeada"].ToString());
                    try
                    {
                        connection.Open();
                        correo = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción 
                        //Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }

            return correo;
        }
        public string ConsultarCorreoTerminadoDiseño()
        {
            string correo = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select  mail from tblUsosVarios where ObjetivoMail = 'mailTerminadoDiseñoDibujo'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        correo = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción 
                        //Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }

            return correo;
        }
        public bool EnviarCorreoRenderPausado(string destinatarios, string cuerpo)
        {
            string nombreProcedimiento = "duc_sp_Correo";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(nombreProcedimiento, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Definir los parámetros del procedimiento almacenado
                        command.Parameters.AddWithValue("@Destinatarios", destinatarios.TrimEnd(';'));
                        command.Parameters.AddWithValue("@asunto", "Pausa Render N° " + NumeroRender.Text + "-" + tbCliente.Text);
                        command.Parameters.AddWithValue("@cuerpo", cuerpo);
                        command.Parameters.AddWithValue("@adjuntos", "");
                        command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejar la excepción (opcional)
                // Loggear la excepción o hacer algo con ella
                return false;
            }
        }


        //Despausar Render
        protected void btnDespausarRender_SI_Click(object sender, EventArgs e)
        {


            // calculamos la nueva fecha de entrega
            DateTime FechaIngresoRender = DateTime.Now;
            DateTime UltimaActivacionRender = FechaIngresoRender;


            while (UltimaActivacionRender.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionRender.DayOfWeek == DayOfWeek.Sunday)
            {
                UltimaActivacionRender = UltimaActivacionRender.AddDays(1);
                UltimaActivacionRender = new DateTime(UltimaActivacionRender.Year, UltimaActivacionRender.Month, UltimaActivacionRender.Day, 8, 0, 0);
            }

            DateTime FechaEntrega = CalcularFechaEntrega(UltimaActivacionRender);

            // agregamos a seguimiento las fechas de activacion 
            string SeguPausas = "(Render Reactivado el  " + FechaIngresoRender + " nueva fecha de entrega: " + FechaEntrega + ")" + "\n" + txSegPausas.InnerText;

            // realizamos la Actualizacion  en la base de datos 
            if (MetodoDespausarRender(SeguPausas, FechaEntrega, UltimaActivacionRender))
            {
                // Se actualizo correctamente
                string mensajeExito = "El render fue reactivado correctamente.";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }
            else
            {
                // Manejar la excepción y mostrar mensaje de "intente nuevamente"
                string mensajeExito = "ocurrió un error al reactivar el render, Por favor intente nuevamente.";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

            }

            DataGridRenders.DataBind();
            int index = Convert.ToInt32(tbId_Fila.Text); // Ajusta el índice según sea necesario
            DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                DataGridRenders.Items[index],
                DataGridRenders,
                new CommandEventArgs("VerRenders", index)
            );
            DataGridRenders_LinkButton(DataGridRenders, args);



        }
        private bool MetodoDespausarRender(string segPau, DateTime fechaEntrega, DateTime UltimaActivacionRender)
        {


            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblRender SET Pausado = 0,UltimaActivacion = @FechaUltAct, " +
                          " Fecha_Programada_Entrega = @fechaEntrega, SeguimientoPausa = @seguimientoPausa WHERE ID_Render = @ID_Render  ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow

                    cmd.Parameters.AddWithValue("@FechaUltAct", UltimaActivacionRender);
                    cmd.Parameters.AddWithValue("@fechaEntrega", fechaEntrega);
                    cmd.Parameters.AddWithValue("@seguimientoPausa", segPau);
                    cmd.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if (CantidadFilasAfectada > 0)
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


        //Devolver Render 
        protected void btnDevolverRender_SI_Click(object sender, EventArgs e)
        {
            string script = @"mostralMoldalDevolverRenderJustificacion();";
            ScriptManager.RegisterStartupScript(this, GetType(), "mostralMoldalDevolverRenderJustificacion", script, true);

        }
        protected void btnDevolverJustificacion_SI_Click(object sender, EventArgs e)
        {
            if (DevolverRenderDibujo())
            {

                DataGridRenders.DataBind();
                int index = Convert.ToInt32(tbId_Fila.Text); // Ajusta el índice según sea necesario
                DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                    DataGridRenders.Items[index],
                    DataGridRenders,
                    new CommandEventArgs("VerRenders", index)
                );
                DataGridRenders_LinkButton(DataGridRenders, args);


                // Enviar el correo electronico 
                string correoEmisor = ConsultarCorreoEmisor();
                string correoAsesor = ConsultarCorreoAsesorRender();
                string correoPordefecto = ConsultarCorreoTerminadoDiseño();


                string destinatarios = correoEmisor + ";" + correoAsesor + ";" + correoPordefecto;
                string cuerpo = @"
                    <!DOCTYPE html>
                    <html lang='es'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                line-height: 1.6;
                                margin: 0;
                                padding: 0;
                                background-color: #f9f9f9;
                            }
                            .container {
                                max-width: 40rem;
                                margin: 20px auto;
                                padding: 20px;
                                border: 1px solid #ccc;
                                border-radius: 5px;
                                background-color: #fff;
                            }
                            h2 {
                                color: #333;
                                font-size: 24px;
                                margin-bottom: 20px;
                            }
                            p {
                                margin-bottom: 10px;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h3>Devolución Render </h3>
                            <p>Estimado(a) asesor(a)</p>
                             <p><strong> Su solicitud de Render ha sido devuelta por : </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>                                                                          
                            <p><strong>Render N°: </strong> " + NumeroRender.Text + @"</p>
                            <p><strong>Cliente: </strong> " + tbCliente.Text + @"</p>
                            <p><strong>Proyecto: </strong> " + tbProyecto.Text + @"</p>
                            <p><strong>Observación dibujo: </strong> " + txObsDibujo.InnerText + @"</p>
                            <p><strong>Historial de pausas y devoluciones: </strong> " + txSegPausas.InnerText + @"</p>
                            <p><strong>Cualquier inquietud no dude en comunicarse con: </strong> " + Session["usuariologueado"].ToString() + @"</p>
                        </div>
                    </body>
                    </html>";

                if (EnviarCorreoRenderDevuelto(destinatarios, cuerpo))
                {
                    string mensajeExito = "El render se devolvió con exito y fue notificado por correo electronico";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                }
                else
                {
                    string mensajeExito = "El render se devolvió con exito, pero hubo problemas para notificar por correo electronico";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                }
            }
            else
            {
                //Mensaje de Error al devolver el render 
                string mensajeExito = "Ocurrió un error al devolver el render. por favor inntentelo nuevamente.";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }
        }
        private bool DevolverRenderDibujo()
        {

            DateTime fecha = DateTime.Now;
            string SegPausa = "(Render devuelto por el dibujante: " + Session["usuariologueado"].ToString() + " el " + fecha + " Razón: " + txJustificacionDevolucion.InnerText + ")";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblRender SET ProgramadoVentas=0, SeguimientoPausa= @seguimientoPausa WHERE Id_Render = @ID_Render  ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@seguimientoPausa", SegPausa);
                    cmd.Parameters.AddWithValue("@ID_Render", NumeroRender.Text);

                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if (CantidadFilasAfectada > 0)
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
        public bool EnviarCorreoRenderDevuelto(string destinatarios, string cuerpo)
        {
            string nombreProcedimiento = "duc_sp_Correo";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(nombreProcedimiento, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Definir los parámetros del procedimiento almacenado
                        command.Parameters.AddWithValue("@Destinatarios", destinatarios.TrimEnd(';'));
                        command.Parameters.AddWithValue("@asunto", "Devolución del Render N° " + NumeroRender.Text + "-" + tbCliente.Text);
                        command.Parameters.AddWithValue("@cuerpo", cuerpo);
                        command.Parameters.AddWithValue("@adjuntos", "");
                        command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejar la excepción (opcional)
                // Loggear la excepción o hacer algo con ella
                return false;
            }
        }


        // Buscador de Renders 
        protected void btnBuscarRender_Click(object sender, EventArgs e)
        {
            if(ID_Render_Buscado.Text == "")
            {
                string mensajeExito = "Por favor, digite un número de render.";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                DataGridRenders.DataSourceID = "CargarRenders";
                ID_Render_Buscado.Focus();
            }
            else
            {
                if (ValidarExisteRender(ID_Render_Buscado.Text))
                {

                    if (ID_Render_Buscado.Text != "")
                    {
                        DataGridRenders.DataSourceID = "RenderPorId";

                    }
                    else if (ddlZona2.SelectedItem.Text != "Todas")
                    {
                        DataGridRenders.DataSourceID = "RenderPorZona";
                    }
                    else
                    {
                        DataGridRenders.DataSourceID = "CargarRenders";
                    }

                    // Coltrol de Boton de Trabajar Y Desprogramar Render 

                    btnTrabajarRender.Enabled = false;
                    btnTrabajarRender.CssClass = "btn btn-sm btn-outline-secondary";

                    btnDesprogramarRender.Enabled = false;
                    btnDesprogramarRender.CssClass = "btn btn-sm btn-outline-secondary";

                    string script = @"ControlCamporYBotones();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ControlCamporYBotones", script, true);


                   
                }
                else
                {
                    string mensajeExito = "El render buscado no se encuentra en programación";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                    ID_Render_Buscado.Focus();
                }

                DataGridRenders.DataBind();

            }
      
           

        }
        private bool ValidarExisteRender(string ID_Render)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT * FROM tblRender WHERE  Id_Render = @ID";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@ID", ID_Render);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
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

    }
}



