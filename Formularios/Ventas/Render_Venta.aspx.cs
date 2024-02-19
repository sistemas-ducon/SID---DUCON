using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

                    Button btnTrabajarRender = FindControl("btnTrabajarRender") as Button;
                    if (btnTrabajarRender != null)
                    {
                        btnTrabajarRender.Enabled = false;
                        btnTrabajarRender.CssClass = "bnt btn-outline-secoundary";
                    }


                    Button btnProgramarRender = FindControl("btnProgramarRender") as Button;
                    if (btnProgramarRender != null)
                    {
                        btnProgramarRender.Enabled = false;
                        btnProgramarRender.CssClass = "btn btn-warning";

                    }

                    CargarVariablesDeSesion();



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
                { "AnimacionSession", chxAnimacion }
            };

            foreach (var kvp in variablesDeSesionYControles)
            {
                string valorSesion = Session[kvp.Key] as string;
                if (!string.IsNullOrEmpty(valorSesion))
                {
                    if (kvp.Value is TextBox)
                    {
                        ((TextBox)kvp.Value).Text = valorSesion;
                    }
                    else if (kvp.Value is DropDownList)
                    {
                        ((DropDownList)kvp.Value).SelectedItem.Text = valorSesion;
                    }
                    else if (kvp.Value is CheckBox)
                    {
                        ((CheckBox)kvp.Value).Checked = Convert.ToBoolean(valorSesion);
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
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525"); //Verde
                }
                else if (pausado == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2"); // Aqua
                }
                else if (fechaProgramada <= DateTime.Now && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F71A27");    //rojo 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (programadoVentas == 0)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b");    //Morado 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1FF43");//amarillo 
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
                string NomRender = row.Cells[43].Text;
                string FechaUltimaActivacion = row.Cells[4].Text;
                DateTime FechaUltimaActivacionFormat = DateTime.Parse(FechaUltimaActivacion);
                string FechaEntrega = row.Cells[5].Text;
                DateTime FechaEntregaFormat = DateTime.Parse(FechaEntrega);
                string Asesor = row.Cells[6].Text;
                //Realizado por [7]
                string Zona = row.Cells[8].Text;
                string FechaRenderOK = row.Cells[9].Text;
                DateTime FechaRenderOKFormat = DateTime.Parse(FechaRenderOK);
                string NumeroDiseño = row.Cells[10].Text;
                string NombreRender = row.Cells[11].Text;
                string NombreContacto = row.Cells[12].Text;
                string FechaIngreso = row.Cells[13].Text;
                DateTime FechaIngresoFormat = DateTime.Parse(FechaIngreso);
                string Celular = row.Cells[14].Text;
                string Mail = row.Cells[15].Text;
                string Telefono = row.Cells[16].Text;
                string Plano = row.Cells[17].Text;

                string Imagenes = row.Cells[18].Text;
                string Areas = row.Cells[19].Text;
                string ObsVentas = row.Cells[20].Text;
                string SegPausa = row.Cells[21].Text;
                string Observacion_Dibujo = row.Cells[22].Text;
                string Linea = row.Cells[23].Text;
                string Superficie = row.Cells[24].Text;
                string Accesorios = row.Cells[25].Text;
                string Cantos = row.Cells[26].Text;
                string Perfileria = row.Cells[27].Text;
                string Paneles = row.Cells[28].Text;
                string Archivadores = row.Cells[29].Text;
                string Sillas = row.Cells[30].Text;
                string Pantallas = row.Cells[31].Text;
                string EspArquitectonico = row.Cells[32].Text;
                string Muebles = row.Cells[33].Text;
                string PisoyZocalo = row.Cells[34].Text;
                string Muros = row.Cells[35].Text;
                string Iluminacion = row.Cells[36].Text;
                string Antepecho = row.Cells[37].Text;
                string Ambientacion = row.Cells[38].Text;
                string Animacion = row.Cells[39].Text;
                string TerminadoVentas = row.Cells[40].Text;

                NumeroRender.Text = IdRender;
                tbCliente.Text = NomRender;
                tbUltActiv.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-dd");
                tbUltActivServidor.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-dd");
                tbEntrega.Text = FechaEntregaFormat.ToString("yyyy-MM-dd");
                tbEntregaServidor.Text = FechaEntregaFormat.ToString("yyyy-MM-dd");
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
                tbFechaOk.Text = FechaRenderOKFormat.ToString("yyyy-MM-dd");
                tbFechaOkServidor.Text = FechaRenderOKFormat.ToString("yyyy-MM-dd");
                tbDiseño.Text = NumeroDiseño;
                tbProyecto.Text = NombreRender;
                tbContacto.Text = NombreContacto;
                tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-dd");
                tbIngresoServidor.Text = tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-dd");
                tbIngresoServidor.Text = FechaIngresoFormat.ToString("yyyy-MM-dd"); // valor para manejar en el servidor 
                tbCelular.Text = Celular;
                tbMail.Text = Mail;
                tbTelefono.Text = Telefono;
                tbPlano.Text = Plano;
                tbImagenes.Text = Imagenes;

                if (Areas == "&nbsp;")
                {
                    string AreasRep = Areas.Replace("&nbsp;", "");
                    txAreaRender.InnerText = AreasRep;
                }
                else
                {
                    txAreaRender.InnerText = Areas;
                }

                if (ObsVentas == "&nbsp;")
                {
                    string ObsVentasRep = ObsVentas.Replace("&nbsp;", "");

                    txObsVentas.InnerText = ObsVentasRep;

                }
                else
                {
                    txObsVentas.InnerText = ObsVentas;
                }

                if (SegPausa == "&nbsp;")
                {
                    string SegPausaRep = SegPausa.Replace("&nbsp;", "");
                    txSegPausas.InnerText = SegPausaRep;
                }

                if (Observacion_Dibujo == "&nbsp;")
                {
                    string Observacion_DibujoRep = SegPausa.Replace("&nbsp;", "");

                    txObsDibujo.InnerText = Observacion_DibujoRep;

                }

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


                // Mantener Deshabilitado ddlAsesor y ddZona
                ddlAsesor.Enabled = false;
                ddlAsesor.CssClass = "form-control disabled";

                ddlZona.Enabled = false;
                ddlZona.CssClass = "form-control disabled";


                // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                string script = "<script>HabilitarEnlaces1();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);




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
            else if(FechaIni.Text != "" && FechaFin.Text != "" && tbClienteX.Text != "")
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
                 mensaje = "Por favor, selecciona una fecha de búsqueda.";
                string script = "<script>AlertaBuscar('" + mensaje + "');</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "AlertaBuscar", script, false);

            }

        }

        protected void GuardarModificarRender(object sender, EventArgs e)
        {

            //Pendiente Validaciones que el Render tenga un numero de Diseño Asociado 

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
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525");
                }
                else if (fechaProgramada <= DateTime.Now && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F71A27");
                }
                else if (programadoVentas == 0)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b");
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else
                {
                    if (pausado == 1 && programadoVentas == 1)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2");
                    }

                    else
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1FF43");//amarillo 
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
                string NomRender = row.Cells[43].Text;
                string FechaUltimaActivacion = row.Cells[4].Text;
                DateTime FechaUltimaActivacionFormat = DateTime.Parse(FechaUltimaActivacion);
                string FechaEntrega = row.Cells[5].Text;
                DateTime FechaEntregaFormat = DateTime.Parse(FechaEntrega);
                string Asesor = row.Cells[6].Text;
                //Realizado por [7]
                string Zona = row.Cells[8].Text;
                string FechaRenderOK = row.Cells[9].Text;
                DateTime FechaRenderOKFormat = DateTime.Parse(FechaRenderOK);
                string NumeroDiseño = row.Cells[10].Text;
                string NombreRender = row.Cells[11].Text;
                string NombreContacto = row.Cells[12].Text;
                string FechaIngreso = row.Cells[13].Text;
                DateTime FechaIngresoFormat = DateTime.Parse(FechaIngreso);
                string Celular = row.Cells[14].Text;
                string Mail = row.Cells[15].Text;
                string Telefono = row.Cells[16].Text;
                string Plano = row.Cells[17].Text;

                string Imagenes = row.Cells[18].Text;
                string Areas = row.Cells[19].Text;
                string ObsVentas = row.Cells[20].Text;
                string SegPausa = row.Cells[21].Text;
                string Observacion_Dibujo = row.Cells[22].Text;
                string Linea = row.Cells[23].Text;
                string Superficie = row.Cells[24].Text;
                string Accesorios = row.Cells[25].Text;
                string Cantos = row.Cells[26].Text;
                string Perfileria = row.Cells[27].Text;
                string Paneles = row.Cells[28].Text;
                string Archivadores = row.Cells[29].Text;
                string Sillas = row.Cells[30].Text;
                string Pantallas = row.Cells[31].Text;
                string EspArquitectonico = row.Cells[32].Text;
                string Muebles = row.Cells[33].Text;
                string PisoyZocalo = row.Cells[34].Text;
                string Muros = row.Cells[35].Text;
                string Iluminacion = row.Cells[36].Text;
                string Antepecho = row.Cells[37].Text;
                string Ambientacion = row.Cells[38].Text;
                string Animacion = row.Cells[39].Text;
                string TerminadoVentas = row.Cells[40].Text;

                NumeroRender.Text = IdRender;
                tbCliente.Text = NomRender;
                tbUltActiv.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-dd");
                tbUltActivServidor.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-dd");
                tbEntrega.Text = FechaEntregaFormat.ToString("yyyy-MM-dd");
                tbEntregaServidor.Text = FechaEntregaFormat.ToString("yyyy-MM-dd");
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
                tbFechaOk.Text = FechaRenderOKFormat.ToString("yyyy-MM-dd");
                tbFechaOkServidor.Text = FechaRenderOKFormat.ToString("yyyy-MM-dd");
                tbDiseño.Text = NumeroDiseño;
                tbProyecto.Text = NombreRender;
                tbContacto.Text = NombreContacto;
                tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-dd");
                tbIngresoServidor.Text = tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-dd");
                tbIngresoServidor.Text = FechaIngresoFormat.ToString("yyyy-MM-dd"); // valor para manejar en el servidor 
                tbCelular.Text = Celular;
                tbMail.Text = Mail;
                tbTelefono.Text = Telefono;
                tbPlano.Text = Plano;
                tbImagenes.Text = Imagenes;

                txAreaRender.InnerText = Areas;
                txObsVentas.InnerText = ObsVentas;

                if (SegPausa == "&nbsp;")
                {
                    string SegPausaRep = SegPausa.Replace("&nbsp;", "N/A");
                    txSegPausas.InnerText = SegPausaRep;
                }
                else
                {
                    txSegPausas.InnerText = SegPausa;
                }

                if (Observacion_Dibujo == "&nbsp;")
                {
                    string Observacion_DibujoRep = Observacion_Dibujo.Replace("&nbsp;", "N/A");

                    txObsDibujo.InnerText = Observacion_DibujoRep;

                }
                else
                {
                    txObsDibujo.InnerText = Observacion_Dibujo;
                }

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


                // Mantener Deshabilitado ddlAsesor y ddZona
                ddlAsesor.Enabled = false;
                ddlAsesor.CssClass = "form-control disabled";

                ddlZona.Enabled = false;
                ddlZona.CssClass = "form-control disabled";


                // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                string script = "<script>HabilitarEnlaces1();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);

                PanelRender.Update();



            }
        }

        protected void ProgramarRender(object sender, EventArgs e)
        {

            switch (Session["Departamento"].ToString().ToUpper())
            {
                case "VENTAS":

                    DateTime FechaIngresoRender = DateTime.Now;
                    DateTime UltimaActivacionRender = FechaIngresoRender;


                    while (UltimaActivacionRender.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionRender.DayOfWeek == DayOfWeek.Sunday)
                    {
                        UltimaActivacionRender = UltimaActivacionRender.AddDays(1);
                        UltimaActivacionRender = new DateTime(UltimaActivacionRender.Year, UltimaActivacionRender.Month, UltimaActivacionRender.Day, 8, 0, 0);
                    }
                    DateTime FechaEntrega = UltimaActivacionRender.AddDays(4);
                    if (chxAnimacion.Checked)
                    {
                        FechaEntrega = FechaEntrega.AddDays(1);

                    }
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    // Calcula el día siguiente a la fecha de entrega
                    DateTime DiaSiguiente = FechaEntrega.AddDays(1);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Consulta SQL para verificar si la fecha de entrega es un día feriado
                        string query = "SELECT COUNT(*) FROM tblDiaNoLaboral WHERE dnlFecha BETWEEN @UltimaActivacionRender AND @FechaEntrega OR dnlFecha = @DiaSiguiente";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agrega el parámetro para la fecha de entrega
                            command.Parameters.AddWithValue("@FechaEntrega", FechaEntrega);
                            command.Parameters.AddWithValue("@UltimaActivacionRender", UltimaActivacionRender);
                            command.Parameters.AddWithValue("@DiaSiguiente", DiaSiguiente);

                            int count = (int)command.ExecuteScalar(); // Ejecuta la consulta y obtén el resultado

                            if (count > 0)
                            {
                                // Si la fecha de entrega o el día siguiente son días feriados, agrega el número correcto de días adicionales a la fecha de entrega
                                FechaEntrega = FechaEntrega.AddDays(count);
                            }
                        }
                    }

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
                        string mensajeExito = "El Render " + tbProyecto.Text.Trim() + " ha sido agregado exitosamente.";
                        string scriptExito = "alert('" + mensajeExito + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);


                    }



                    break;

                case "COMPRA": //Boton Programar  Departamento Compras

                    break;

                case "DESARROLLO DE PRODUCTO": // Boton Programar  Departamento Compras Desarrollo Producto


                    break;


                default:
                    // Error Con el despartamento de ese usuario Validar con Sistemas 
                    break;
            }



        }

        protected void PausarRender1(Object serder, EventArgs eventArgs)
        {
            // Para Dibujo 
        }

        protected void DevolverRender1(Object serder, EventArgs eventArgs)
        {
            // Para Dibujo 
        }

        protected void EliminareRender1(Object serder, EventArgs eventArgs)
        {
            // Para Dibujo 
        }


    }

}
