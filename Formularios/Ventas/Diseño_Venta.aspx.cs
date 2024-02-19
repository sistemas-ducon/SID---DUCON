 using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Web.Services;
using System.IO;
using Button = System.Web.UI.WebControls.Button;
using System.Diagnostics;
using DocumentFormat.OpenXml.Office2010.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Media.TextFormatting;
using System.Globalization;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Xml;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Diseño_Venta : System.Web.UI.Page
    {
       
        private string CadenaConexionSID = "BD_SIDSQL";

        private string CadenaConexionISID = "BD_ISIDSQL";

        private string CadenaConexionSSF = "BD_SSF";

        private bool isModalVisible = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {         

                if (Session["usuariologueado"] != null)
                {
                    string usuariologueado = Session["usuariologueado"].ToString();
                    DataGridDiseñosPorFecha.SelectParameters["NombreUsuario"].DefaultValue = usuariologueado;
                }
                else
                {
                    Response.Redirect("/Formularios/Login.aspx");
                }

                if (Session["CedulaLogeada"] != null)
                {
                    string cedulaLogeada = Session["CedulaLogeada"].ToString();
                    SqlDataSource1.SelectParameters["Cedula"].DefaultValue = cedulaLogeada;
                    DataGridDiseño.SelectParameters["Cedula"].DefaultValue = cedulaLogeada;                                
                    DataGridRenderPorFechaYAsesor.SelectParameters["Cedula"].DefaultValue = cedulaLogeada;
                    SqlDataSource4.SelectParameters.Add("CodigoAsesor", cedulaLogeada);
                }

                LinkButton1.Enabled = false;
                LinkButton1.CssClass = "btn btn-sm button-disabled";

                LinkButton3.Enabled = false;
                LinkButton3.CssClass = "btn btn-sm button-disabled";

                LinkButton5.Enabled = false;
                LinkButton5.CssClass = "btn btn-sm button-disabled";

                LinkButton4.Enabled = false;
                LinkButton4.CssClass = "btn btn-sm button-disabled";

                LinkButton2.Enabled = false;
                LinkButton2.CssClass = "btn btn-sm button-disabled";

                ConfigureSqlDataSource();
                UpdateDivsVisibility();

                ApplyButtonStyles();      
                DropDownList1.DataBind();
                DropDownList1.Items.Insert(0, new ListItem(""));
                TextCiuPro.DataBind();
                TextCiuPro.Items.Insert(0, new ListItem(""));
                habilitarbotones();
                DeshabilitarDivYContenido(miDiv);
                CheckBox22.Checked = isModalVisible;              
                elementosllenosalcargarlapagina();
                CargarClienteYContacto();

                if (Session["NumeroDiseño2"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño2"].ToString()))
                {
                    if (Session["Documentacion"] != null && (bool)Session["Documentacion"])
                    {
                        // Realizar las acciones necesarias cuando Documentacion es true
                                   
                        ProcesarNumeroDiseño2(null);
                        AccionesAlCargarDiseño();

                        ToggleDivsVisibility();
                        ConfigureSqlDataSource();
                        DataGridDocumento.DataBind();
                    }
                    else
                    {
                        // Si Documentacion es false, ejecutar solo estos métodos
                        ProcesarNumeroDiseño2(null);
                        AccionesAlCargarDiseño();
                    
                    }

                    string script = @"
            <script type='text/javascript'>
                window.onload = function () {
                    // Obtener el elemento del tab deseado
                    var tab = document.getElementById('Diseño-BitacoraFPV-001-tab');
                    
                    // Hacer clic en el tab deseado
                    tab.click();
                    
                    // Ocultar el tab actual si es necesario
                    var activeTab = document.querySelector('.nav-item .active');
                    if (activeTab) {
                        activeTab.classList.remove('active');
                    }
                    
                    // Agregar la clase 'active' al tab deseado
                    tab.classList.add('active');
                };
            </script>";

                    // Registrar el script en el cliente
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SwitchToDesiredTab", script);
                }
                else
                {
                    // Manejar el caso cuando Session["NumeroDiseño"] es null o vacío
                }

                // Eliminar la variable de sesión "NumeroDiseño" después de usarla
                Session.Remove("NumeroDiseño2");
                Session.Remove("SelectedIdOT");
                Session.Remove("SelectedFileName");
                Session.Remove("Documentacion");
                Session.Remove("lnkClieClicked");
                Session.Remove("lnkClieeClicked");




            }
        }

        protected void AccionesAlCargarDiseño()
        {
            NuevoDisBit.Enabled = true;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled";

            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled";

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;
            Modificar.CssClass = "btn btn-sm shadow button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled";

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-enabled";

            ValidarBotonOk();

            DeshabilitarDivYContenido(miDiv);

            Session["lnkClieClicked"] = true;
        }

        protected void CargarOT_Click(object sender, EventArgs e)
        {
            Session.Remove("lnkClieClicked");
            Session.Remove("lnkClieeClicked");

            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid1.Items[rowIndex];

            // Almacena el valor de Id_OT en una variable de sesión
            Session["Id_OT2"] = selectedRow.Cells[2].Text;

            // Almacena el nombre del archivo en la variable de sesión
            Session["pedido2"] = selectedRow.Cells[3].Text;

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

            string mensajePersonalizado = "Se cargara la OT seleccionada";
            string urlRedireccion = "OrdenTrabajo.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

         

        }

        protected void BtnProgramar_Click(object sender, EventArgs e)
        {
            string diseño = lblNumDise.Text + " - " ;

            string contenidoModalOT = "Una Vez aprobado el Diseño, no podra realizarle modificaciones, esta seguro de aprobar el diseño: " + diseño + ", para dibujo y despiece?";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ProgramarDiseño').modal('show'); $('#ProgramarDiseño2').text('" + contenidoModalOT + "');", true);

        }

        protected void ProgramarDiseño_Click(object sender, EventArgs e)
        {
            // Obtener la fecha programada de entrega desde el TextBox TextEntrega
            DateTime fechaActual = DateTime.Now;

            // Obtener el valor del número de diseño desde el label
            string numeroDiseño = lblNumDise.Text;


            DateTime fechaIngresoDiseño = DateTime.Parse(TextIngDis.Text);

            // Obtener la fecha de activación desde el TextBox TextUltAc
            DateTime fechaActivacion = DateTime.Parse(TextUltAc.Text);
       
            DateTime fechaProgramadaEntrega = fechaActual.AddDays(5);
            TextEntrega.Text = fechaProgramadaEntrega.ToString("yyyy-MM-ddTHH:mm");

            // Comparación y actualización de la fecha de SC
            DateTime dtpFechaSC = DateTime.Parse(TextFec.Text);

            if (fechaProgramadaEntrega > dtpFechaSC)
            {
                dtpFechaSC = fechaProgramadaEntrega.Date;
                // Aquí puedes decidir si también deseas actualizar dtpHoraSC
            }

            // Realizar la actualización en la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE tblDiseño SET ProgramadoVentas = 1, Fecha_Ingreso = @FechaIngreso, Fecha_Programada_Entrega = @FechaProgramadaEntrega, UltimaActivacion = @UltimaActivacion, PactodeEntrega = @PactodeEntrega WHERE Numero_Diseño = @NumeroDiseño";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);
                    cmd.Parameters.AddWithValue("@FechaIngreso", fechaIngresoDiseño);
                    cmd.Parameters.AddWithValue("@FechaProgramadaEntrega", fechaProgramadaEntrega);
                    cmd.Parameters.AddWithValue("@UltimaActivacion", fechaActivacion);
                    cmd.Parameters.AddWithValue("@PactodeEntrega", fechaProgramadaEntrega);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["NumeroDiseño2"] = numeroDiseño;


                        string mensajePersonalizado = "Diseño Programado exitosamente!";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                        // Si se actualizaron filas, se deshabilita el botón y se cambia su clase CSS
                        BtnProgramar.Enabled = false;
                        BtnProgramar.CssClass = "button-disabled form-control";
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);

                        // Si no se actualizaron filas, se mantiene el botón habilitado y se cambia su clase CSS
                        BtnProgramar.Enabled = true;
                        BtnProgramar.CssClass = "button-enabled form-control";
                    }
                }
            }
        }

        protected void SiButton_Click(object sender, EventArgs e)
        {
            // Obtener la fecha y hora actual
            DateTime now = DateTime.Now;

            string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextUltAc.Text = fechaHoraActual;


            string fechaEntrega = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextEntrega.Text = fechaEntrega;


            string fechaInDis = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextIngDis.Text = fechaInDis;

            string fechaOkDib = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextFecOkDib.Text = fechaOkDib;


            // Limpiar el contenido del TextBox
            TextProyecto.Text = string.Empty;
            TextCliente.Text = string.Empty;
            TextDir.Text = string.Empty;
            TextPla.Text = string.Empty;
            TextContacto.Text = string.Empty;
            TextTel.Text = string.Empty;
            TextMail.Text = string.Empty;
            //TextCiuPro.Text = string.Empty;
            TexHTot.Text = string.Empty;
            TextLin.Text = string.Empty;
            TextMos.Text = string.Empty;
            TextSup.Text = string.Empty;
            TextSop.Text = string.Empty;
            TextGav.Text = string.Empty;
            TextPan.Text = string.Empty;
            TextTapPie.Text = string.Empty;
            TextRep.Text = string.Empty;
            TextTipVid.Text = string.Empty;
            TextPant.Text = string.Empty;
            TextArch.Text = string.Empty;
            TextCoc.Text = string.Empty;
            TextEnt.Text = string.Empty;
            TextPuer.Text = string.Empty;
            TextObsVen.Value = string.Empty;
            TextObsDibDes.Value = string.Empty;
            TextSegPauDev.Value = string.Empty;

          

            lblNumDise.Text = "Por definir";

            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "form-control form-control-sm";

            TextFec.Enabled = false;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = false;
            TextFech.CssClass = "form-control form-control-sm";

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

            
            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Session["EventoItemCommandEjecutado"] = false;
            Session.Remove("lnkClieClicked");
            Session.Remove("lnkClieeClicked");
        }

        protected void NoButton_Click(object sender, EventArgs e)
        {
           

            // Obtener la fecha y hora actual
            DateTime now = DateTime.Now;

            string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextUltAc.Text = fechaHoraActual;

          
            string fechaEntrega = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextEntrega.Text = fechaEntrega;


            string fechaInDis = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextIngDis.Text = fechaInDis;

            string fechaOkDib = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextFecOkDib.Text = fechaOkDib;

            


            lblNumDise.Text = "Por definir";

            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "form-control form-control-sm";

            TextFec.Enabled = false;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = false;
            TextFech.CssClass = "form-control form-control-sm";

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";


            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Session["EventoNoButtonEjecutado"] = true;
           
        }

        private void ApplyButtonStyles()
        {
            ApplyButtonStyle(NuevoDisBit);
            ApplyButtonStyle(Grabar);
            ApplyButtonStyle(Modificar);
            ApplyButtonStyle(DocBitacora);
            ApplyButtonStyle(RegresarDiseño);
            ApplyButtonStyle(AdicionarElemento);
            ApplyButtonStyle(ActualizarDiseno);
            ApplyButtonStyle(PausarDiseño);
            ApplyButtonStyle(Cancelar);
            ApplyButtonStyle(EliminarDiseño);
          
        }

        private void ApplyButtonStyle(WebControl button)
        {
            button.CssClass = button.Enabled ? "button-enabled" : "button-disabled";
        }

        protected override void OnInit(EventArgs e)
        {
            ViewState["Accion"] = "Insercion";
            base.OnInit(e);
        }

        private void CargarClienteYContacto()
        {
            if (!IsPostBack)
            {
                string IdCLiente = Session["Id_ClienteBD"]?.ToString();
                string IdContaco = Session["ID_ContactoBD"]?.ToString();

                if (!string.IsNullOrEmpty(IdCLiente) && !string.IsNullOrEmpty(IdContaco))
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "SELECT X.NombreCompañía, X.Dirección, Y.NombreContacto, Y.MailContacto " +
                                       "FROM tblCliente AS X " +
                                       "INNER JOIN tblClienteContacto AS Y ON Y.Id_Cliente = X.Id_Cliente " +
                                       "WHERE X.Id_Cliente = @ParametroCliente AND Y.Id_ClienteContacto = @ParametroClienteContacto";



                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ParametroCliente", IdCLiente);
                            command.Parameters.AddWithValue("@ParametroClienteContacto", IdContaco);

                            connection.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreCompañía")))
                                    {
                                        TextCliente.Text = reader["NombreCompañía"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Dirección")))
                                    {
                                        TextDir.Text = reader["Dirección"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreContacto")))
                                    {
                                        TextContacto.Text = reader["NombreContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("MailContacto")))
                                    {
                                        TextMail.Text = reader["MailContacto"].ToString();
                                    }
                                }
                            }
                        }
                    }

                    Session.Remove("Id_ClienteBD");
                    Session.Remove("ID_ContactoBD");
                }

            }
        }

        protected void elementosllenosalcargarlapagina()
        {
            // Obtener la fecha y hora actual
            DateTime now = DateTime.Now;

            // Asignar la fecha al TextBox de fecha
            TextFec.Text = now.ToString("yyyy-MM-dd");

            // Asignar la hora al TextBox de hora
            TextFech.Text = now.ToString("HH:mm");

            TextIngDis.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

            string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

            // Asignar la fecha y hora actual al TextBox
            TextUltAc.Text = fechaHoraActual;

            TextFecOkDib.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

            DateTime fechaActual = DateTime.Now;

            // Agregar 5 días a la fecha actual
            DateTime fechaEntrega = fechaActual.AddDays(5);

            // Establecer el valor por defecto en el TextBox
            TextEntrega.Text = fechaEntrega.ToString("yyyy-MM-ddTHH:mm");

            BtnProgramar.CssClass = "btn btn-warning shadow btn-sm";
 
 
            TextTel.Text = ("2889898");
            TextCel.Text = ("3002025400");

            ChecConDeCab.Checked = true;
            ChecPiso.Checked = true;
            ChecDiv.Checked = true;
            ChecCie.Checked = true;
            ChecCan.Checked = true;
            ChecBteEle.Checked = true;
            ChecBteSw.Checked = true;
            ChecSujPt.Checked = true;
            ChecAlCie.Checked = true;
            ChecPerRef.Checked = true;
            ChecGuaEsc.Checked = true;
            CheckBox16.Checked = true;
            ChecCot.Checked = true;
            ChecMailTer.Checked = true;
            ChecCotVia.Checked = true;
            CheckBox4.Checked = true;
            ChecMue.Checked = true;
        }

        protected void habilitarbotones()
        {
            NuevoDisBit.Enabled = true;
            ActualizarDiseno.Enabled = true;
            Cancelar.Enabled = true;

            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled";
          

            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled";
            Cancelar.CssClass = "btn btn-sm shadow button-enabled";


            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            DocBitacora.Enabled = false;
            DocBitacora.CssClass = "btn btn-sm shadow button-disabled";

            RegresarDiseño.Enabled = false;
            RegresarDiseño.CssClass = "btn btn-sm shadow button-disabled";

            AdicionarElemento.Enabled = false;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-disabled";

            PausarDiseño.Enabled = false;
            PausarDiseño.CssClass = "btn btn-sm shadow button-disabled";

            EliminarDiseño.Enabled = false;
            EliminarDiseño.CssClass = "btn btn-sm shadow button-disabled";
        }

        private void DeshabilitarDivYContenido(System.Web.UI.Control container)
        {
            TextPla.Enabled = false;
            TextPla.CssClass = "form-control form-control-sm";

            Button16.Enabled = false;
            Button16.CssClass = "form-control form-control-sm";

            Button15.Enabled = false;
            Button15.CssClass = "form-control form-control-sm";

            Button12.Enabled = false;
            Button12.CssClass = "form-control form-control-sm";

            Button13.Enabled = false;
            Button13.CssClass = "form-control form-control-sm";

            Button14.Enabled = false;
            Button14.CssClass = "form-control form-control-sm";

            Button11.Enabled = false;
            Button11.CssClass = "form-control form-control-sm";

            Button9.Enabled = false;
            Button9.CssClass = "form-control form-control-sm";

            Button10.Enabled = false;
            Button10.CssClass = "form-control form-control-sm";


           
           
            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Button1.Enabled = false;
           
            TextDir.Enabled = false;
            TextDir.CssClass = "form-control form-control-sm";

            lblDir.Enabled = false;
            lblDir.CssClass = "col-form-label-sm";

            TextDes.Enabled = false;
            TextDes.CssClass = "form-control form-control-sm";

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            lblDescuento.Enabled = false;
            lblDescuento.CssClass = "col-form-label-sm";

            lblIngDis.Enabled = false;
            lblIngDis.CssClass = "col-form-label-sm";

            lblUltAct.Enabled = false;
            lblUltAct.CssClass = "col-form-label-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            lblPro.Enabled = false;
            lblPro.CssClass = "col-form-label-sm";

            TextProyecto.Enabled = false;
            TextProyecto.CssClass = "form-control form-control-sm";

            lblPla.Enabled = false;
            lblPla.CssClass = "col-form-label-sm";

            lblUrg.Enabled = false;
            lblUrg.CssClass = "col-form-label-sm";

            ChecUrgent.Enabled = false;

            lblCotizar.Enabled = false;
            lblCotizar.CssClass = "col-form-label-sm";

            ChecCot.Enabled = false;

            lblEnt.Enabled = false;
            lblEnt.CssClass = "col-form-label-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            lblEntDib.Enabled = false;
            lblEntDib.CssClass = "col-form-label-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            lblZon.Enabled = false;
            lblZon.CssClass = "col-form-label-sm";

            TextZona.Enabled = false;
            TextZona.CssClass = "form-control form-control-sm";

            lblCon.Enabled = false;
            lblCon.CssClass = "col-form-label-sm";

            TextContacto.Enabled = false;
            TextContacto.CssClass = "form-control form-control-sm";

            lblTel.Enabled = false;
            lblNumDise.CssClass = "col-form-label-sm";

            TextTel.Enabled = false;
            TextTel.CssClass = "form-control form-control-sm";

            lblMaiTer.Enabled = false;
            lblMaiTer.CssClass = "col-form-label-sm";

            ChecMailTer.Enabled = false;

            lblCotVia.Enabled = false;
            lblCotVia.CssClass = "col-form-label-sm";


            ChecCotVia.Enabled = false;

            lblCotTte.Enabled = false;
            lblCotTte.CssClass = "col-form-label-sm";

            CheckBox4.Enabled = false;

            lblAse.Enabled = false;
            lblAse.CssClass = "col-form-label-sm";

            DropDownList1.Enabled = false;

            lblPre.Enabled = false;
            lblPre.CssClass = "col-form-label-sm";

            TextPre.Enabled = false;
            TextPre.CssClass = "form-control form-control-sm";

            lblCel.Enabled = false;
            lblCel.CssClass = "col-form-label-sm";

            TextCel.Enabled = false;
            TextCel.CssClass = "form-control form-control-sm";

            lblMai.Enabled = false;
            lblMai.CssClass = "col-form-label-sm";

            TextMail.Enabled = false;
            TextMail.CssClass = "form-control form-control-sm";

            lblCiuPro.Enabled = false;
            lblCiuPro.CssClass = "col-form-label-sm";

            TextCiuPro.Enabled = false;
            TextCiuPro.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;

            lblConCab.Enabled = false;
            lblCiuPro.CssClass = "form-label";

            ChecConDeCab.Enabled = false;

            lblPis.Enabled = false;
            lblPis.CssClass = "col-form-label-sm g-5";

            ChecPiso.Enabled = false;

            lblDiv.Enabled = false;
            lblDiv.CssClass = "col-form-label-sm g-5";

            ChecDiv.Enabled = false;

            lblCie.Enabled = false;
            lblCie.CssClass = "col-form-label-sm";

            ChecCie.Enabled = false;

            lblCan.Enabled = false;
            lblCan.CssClass = "col-form-label-sm";

            ChecCan.Enabled = false;

            lblBteEle.Enabled = false;
            lblBteEle.CssClass = "col-form-label-sm";

            ChecBteEle.Enabled = false;

            lblBteSw.Enabled = false;
            lblBteSw.CssClass = "col-form-label-sm";

            ChecBteSw.Enabled = false;

            lblSujPt.Enabled = false;
            lblSujPt.CssClass = "col-form-label-sm";

            ChecSujPt.Enabled = false;

            lblAlCie.Enabled = false;
            lblAlCie.CssClass = "col-form-label-sm";

            ChecAlCie.Enabled = false;

            lblPerRef.Enabled = false;
            lblPerRef.CssClass = "col-form-label-sm";

            ChecPerRef.Enabled = false;

            lblGuaEsc.Enabled = false;
            lblGuaEsc.CssClass = "col-form-label-sm";

            ChecGuaEsc.Enabled = false;

            lblHTotCms.Enabled = false;
            lblHTotCms.CssClass = "col-form-label-sm";

            TexHTot.Enabled = false;

            lblEsyMat.Enabled = false;
            lblEsyMat.CssClass = "col-form-label-sm";

            lblLin.Enabled = false;
            lblLin.CssClass = "col-form-label-sm";

            TextLin.Enabled = false;
            TextLin.CssClass = "form-control form-control-sm";

            lblMos.Enabled = false;
            lblMos.CssClass = "col-form-label-sm";

            TextMos.Enabled = false;
            TextMos.CssClass = "form-control form-control-sm";

            lblSup.Enabled = false;
            lblSup.CssClass = "col-form-label-sm";

            TextSup.Enabled = false;
            TextSup.CssClass = "form-control form-control-sm";

            lblBal.Enabled = false;
            lblBal.CssClass = "col-form-label-sm";

            CheckBox16.Enabled = false;

            lblSop.Enabled = false;
            lblSop.CssClass = "col-form-label-sm";

            TextSop.Enabled = false;
            TextSop.CssClass = "form-control form-control-sm";

            lblGav.Enabled = false;
            lblGav.CssClass = "col-form-label-sm";

            TextGav.Enabled = false;
            TextGav.CssClass = "form-control form-control-sm";

            lblPan.Enabled = false;
            lblPan.CssClass = "col-form-label-sm";

            TextPan.Enabled = false;
            TextPan.CssClass = "form-control form-control-sm";

            lblTPie.Enabled = false;
            lblTPie.CssClass = "col-form-label-sm";

            TextTapPie.Enabled = false;
            TextTapPie.CssClass = "form-control form-control-sm";

            lblRep.Enabled = false;
            lblRep.CssClass = "col-form-label-sm";

            TextRep.Enabled = false;
            TextRep.CssClass = "form-control form-control-sm";

            lblTipVid.Enabled = false;
            lblTipVid.CssClass = "col-form-label-sm";

            TextTipVid.Enabled = false;
            TextTipVid.CssClass = "form-control form-control-sm";

            lblPant.Enabled = false;
            lblPant.CssClass = "col-form-label-sm";

            TextPant.Enabled = false;
            TextPant.CssClass = "form-control form-control-sm";

            lblArc.Enabled = false;
            lblArc.CssClass = "col-form-label-sm";

            TextArch.Enabled = false;
            TextArch.CssClass = "form-control form-control-sm";

            lblMue.Enabled = false;
            lblMue.CssClass = "col-form-label-sm";

            ChecMue.Enabled = false;

            lblCoc.Enabled = false;
            lblCoc.CssClass = "col-form-label-sm";

            TextCoc.Enabled = false;
            TextCoc.CssClass = "form-control form-control-sm";

            lblEntr.Enabled = false;
            lblEntr.CssClass = "col-form-label-sm";

            TextEnt.Enabled = false;
            TextEnt.CssClass = "form-control form-control-sm";

            lblPuer.Enabled = false;
            lblPuer.CssClass = "col-form-label-sm";

            TextPuer.Enabled = false;
            TextPuer.CssClass = "form-control form-control-sm";

            CheckBox18.Enabled = false;

            lblPrePpt.Enabled = false;
            lblPrePpt.CssClass = "col-form-label-sm";

            CheckBox19.Enabled = false;

            lblIma.Enabled = false;
            lblIma.CssClass = "col-form-label-sm";

            CheckBox20.Enabled = false;

            lblAcc.Enabled = false;
            lblAcc.CssClass = "col-form-label-sm";

            CheckBox21.Enabled = false;

            lblTieRea.Enabled = false;
            lblTieRea.CssClass = "col-form-label-sm";

            TextFec.Enabled = false;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = false;
            TextFech.CssClass = "form-control form-control-sm";

            lblUbi.Enabled = false;
            TextUbi.CssClass = "col-form-label-sm";


        }

        //BUSCAR DISE
        protected void But_Click(object sender, EventArgs e)
        {
            // Verificar cuáles campos tienen datos y seleccionar el SqlDataSource correspondiente.
            if (!string.IsNullOrEmpty(TextFechDeIng.Text) && !string.IsNullOrEmpty(Texty.Text))
            {
                DataGrid4.DataSource = SqlDataSourceFecha;
            }
            else if (!string.IsNullOrEmpty(TextBox3.Text))
            {
                DataGrid4.DataSource = SqlDataSourceNumeroDis;
            }
            else if (!string.IsNullOrEmpty(TextBox5.Text))
            {
                DataGrid4.DataSource = SqlDataSourceNombreDiseño;
            }
            else if (!string.IsNullOrEmpty(TextBox4.Text))
            {
                DataGrid4.DataSource = SqlDataSourceCliente;
            }

            // Ejecutar la consulta y enlazar los datos al DataGrid.
            DataGrid4.DataBind();
        }

        protected void ddlCiudadX_DataBound(object sender, EventArgs e)
        {
           
        }

        protected void NuevoDisBit_Click(object sender, EventArgs e)
        {
            bool lnkClieClicked = Session["lnkClieClicked"] as bool? ?? false;
            bool lnkClieeClicked = Session["lnkClieeClicked"] as bool? ?? false;

            if (lnkClieClicked || lnkClieeClicked)
            {
                // Mostrar el modal si se hizo clic en lnkClie o lnkCliee
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modall').modal('show');", true);
                Session["NuevoDisBitEjecutado"] = true;

            }
            else
            {
                // Deshabilitar el botón "NuevoDisBit"
                NuevoDisBit.Enabled = false;

                Modificar.Enabled = false;

                // Aplicar clases CSS para botones deshabilitados
                NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";
                Modificar.CssClass = "btn btn-sm shadow button-disabled";

                // Habilitar el botón "Grabar"
                Grabar.Enabled = true;
                Grabar.CssClass = "btn btn-sm shadow button-enabled";

                // Deshabilitar el botón "ActualizarDiseno"
                ActualizarDiseno.Enabled = false;
                ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";


                // Habilitar el div y su contenido
                HabilitarDivYContenido(miDiv);

                TextIngDis.Enabled = false;
                TextUltAc.Enabled = false;
                TextEntrega.Enabled = false;
                TextFecOkDib.Enabled = false;

               

                // Cambiar el color del Label lblCotizar
                lblCotizar.CssClass = "col-form-label-sm text-danger";
                lblCotizar.Font.Bold = true;

                Cancelar.Enabled = true;
                // Deshabilita los TextBox
                TextFec.Enabled = false;
                TextFech.Enabled = false;

                lblNumDise.Text = "Por Definir";

                TextCliente.Enabled = false;
                TextCliente.CssClass = "form-control form-control-sm";

            }
            
           

           
        }

        protected void CheckBox21_CheckedChanged(object sender, EventArgs e)
        {
            // Verifica el estado del CheckBox
            if (CheckBox21.Checked)
            {
                // Habilita los TextBox
                TextFec.Enabled = true;
                TextFech.Enabled = true;
            }
            else
            {
                // Deshabilita los TextBox
                TextFec.Enabled = false;
                TextFech.Enabled = false;
            }
        }

        private void HabilitarDivYContenido(System.Web.UI.Control container)
        {
            foreach (System.Web.UI.Control control in container.Controls)
            {
                if (control is System.Web.UI.WebControls.WebControl)
                {
                    // Verificar si el control es el botón BtnProgramar
                    if (control != BtnProgramar)
                    {
                        // Si el control no es el botón BtnProgramar, habilitarlo
                        ((System.Web.UI.WebControls.WebControl)control).Enabled = true;
                    }
                }

                // Si el control es un contenedor, llamar recursivamente a la función
                if (control.HasControls())
                {
                    HabilitarDivYContenido(control);
                }
            }
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {

            // Verifica si la variable de sesión "Modificado" está establecida como true.
            bool modificado = Session["ModificarEjecutado"] as bool? ?? false;

            if (modificado)
            {
                // Si se hizo clic en Modificar antes, realiza las acciones necesarias para volver al estado anterior.
                NuevoDisBit.Enabled = false;
                NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

                Modificar.Enabled = true;
                Modificar.CssClass = "btn btn-sm shadow button-enabled";

                ActualizarDiseno.Enabled = false;
                ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";


                // Resto de las acciones para volver al estado anterior...
            }
            else
            {
                // Si no se hizo clic en Modificar antes, simplemente restablece todo como estaba antes de Cancelar.
                Grabar.Enabled = false;
                Grabar.CssClass = "btn btn-sm shadow button-disabled";

                NuevoDisBit.Enabled = true;
                NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled";

                ActualizarDiseno.Enabled = true;
                ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled";

                Modificar.Enabled = false;
                Modificar.CssClass = "btn btn-sm shadow button-disabled";
            }

         

            // Deshabilitar el botón "Grabar"
            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = true;

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled";

            // Ocultar el div y su contenido
            DeshabilitarDivYContenido(miDiv);

           
            lblCotizar.CssClass = "col-form-label-sm text-dark";
            lblCotizar.Font.Bold = false;

            // Manejo del evento DataGridDise_ItemCommand
            bool eventoItemCommandEjecutado = Session["EventoItemCommandEjecutado"] as bool? ?? false;

            if (eventoItemCommandEjecutado)
            {
                // Si el evento DataGridDise_ItemCommand se ejecutó correctamente,
                // obtener el valor de la variable de sesión "NumeroDisenoSession" y asignarlo a lblNumDise
                int numeroDiseno = Session["NumeroDisenoSession"] as int? ?? 0;
                lblNumDise.Text = numeroDiseno.ToString();

                Modificar.Enabled = true;
                Modificar.CssClass = "btn btn-sm shadow button-enabled";
            }
            else
            {
                Grabar.Enabled = false;
                Grabar.CssClass = "btn btn-sm shadow button-disabled";

                NuevoDisBit.Enabled = true;
                NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled";

                ActualizarDiseno.Enabled = true;
                ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled";

                lblNumDise.Text = "Numero";

              

                // Limpia la variable de sesión "EventoItemCommandEjecutado" después de utilizarla.
                Session["EventoItemCommandEjecutado"] = false;
            }

            // Manejo del evento DataGridDise_ItemCommand
            bool eventoNoButtonEjecutado = Session["EventoNoButtonEjecutado"] as bool? ?? false;


            if (eventoNoButtonEjecutado)
            {
                Modificar.Enabled = true;
                Modificar.CssClass = "btn btn-sm shadow button-enabled";
            }

            // Limpia la variable de sesión "Modificado" después de utilizarla.
            Session["ModificarEjecutado"] = false;

        } 

        protected void lnkClie_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid2.Items[rowIndex];

            // Almacena el nombre del archivo en la variable de sesión
            Session["NumeroDiseño"] = selectedRow.Cells[2].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid2.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            

            AccionesAlCargarDiseño();


        }

        protected void ValidarBotonOk()
        {
            if (Session["NumeroDiseño"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño"].ToString()))
            {
                // Obtener el número de diseño de la sesión
                int numeroDiseno = Convert.ToInt32(Session["NumeroDiseño"]);

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string queryString = "SELECT ProgramadoVentas FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseno";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@NumeroDiseno", numeroDiseno);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            // Obtener el valor de la columna ProgramadoVentas
                            int programadoVentas = Convert.ToInt32(reader["ProgramadoVentas"]);

                            // Ajustar la propiedad Enabled del botón BtnProgramar
                            BtnProgramar.Enabled = (programadoVentas == 0);

                          
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            else if (Session["NumeroDiseño2"] != null && !string.IsNullOrEmpty(Session["NumeroDiseño2"].ToString()))
            {
                // Obtener el número de diseño de la sesión
                int numeroDiseno = Convert.ToInt32(Session["NumeroDiseño2"]);

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string queryString = "SELECT ProgramadoVentas FROM tblDiseño WHERE Numero_Diseño = @NumeroDiseno";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@NumeroDiseno", numeroDiseno);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            // Obtener el valor de la columna ProgramadoVentas
                            int programadoVentas = Convert.ToInt32(reader["ProgramadoVentas"]);

                            // Ajustar la propiedad Enabled del botón BtnProgramar
                            BtnProgramar.Enabled = (programadoVentas == 0);

                            // Si el botón está habilitado, actualizar los TextBox con las fechas
                            if (BtnProgramar.Enabled)
                            {
                                // Obtener la fecha y hora actual
                                DateTime now = DateTime.Now;

                                // Asignar la fecha al TextBox de fecha
                                TextFec.Text = now.ToString("yyyy-MM-dd");

                                // Asignar la hora al TextBox de hora
                                TextFech.Text = now.ToString("HH:mm");

                                TextIngDis.Text = now.ToString("yyyy-MM-ddTHH:mm");

                                string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");

                                // Asignar la fecha y hora actual al TextBox
                                TextUltAc.Text = fechaHoraActual;

                                TextFecOkDib.Text = now.ToString("yyyy-MM-ddTHH:mm");

                                DateTime fechaActual = DateTime.Now;

                                // Agregar 5 días a la fecha actual
                                DateTime fechaEntrega = fechaActual.AddDays(5);

                                // Establecer el valor por defecto en el TextBox
                                TextEntrega.Text = fechaEntrega.ToString("yyyy-MM-ddTHH:mm");
                            }
                            else
                            {

                            }
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción
                        Console.WriteLine(ex.Message);
                    }
                }

            }
        }


        protected void lnkCliee_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGrid4.Items[rowIndex];

            // Almacena el nombre del archivo en la variable de sesión
            Session["NumeroDiseño"] = selectedRow.Cells[2].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGrid4.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            Session["lnkClieeClicked"] = true;

            NuevoDisBit.Enabled = true;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled";

            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled";

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;
            Modificar.CssClass = "btn btn-sm shadow button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled";

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-enabled";

            ValidarBotonOk();


            DeshabilitarDivYContenido(miDiv);
        
        }

        protected void CargarDiseñoSC_Click(object sender, EventArgs e)
        {
            // Obtén el LinkButton que se hizo clic
            LinkButton lnkSelectRow = (LinkButton)sender;

            // Obtén el índice de fila desde el CommandArgument
            int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

            // Accede a la fila seleccionada en el DataGrid
            DataGridItem selectedRow = DataGridDiseños.Items[rowIndex];

            // Almacena el nombre del archivo en la variable de sesión
            Session["NumeroDiseño"] = selectedRow.Cells[2].Text;

            // Deselecciona todas las filas previamente seleccionadas
            foreach (DataGridItem item in DataGridDiseños.Items)
            {
                if (item != selectedRow)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            Session["lnkClieeClicked"] = true;

            NuevoDisBit.Enabled = true;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-enabled";

            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-enabled";

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;
            Modificar.CssClass = "btn btn-sm shadow button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled";

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;
            AdicionarElemento.CssClass = "btn btn-sm shadow button-enabled";

            ValidarBotonOk();


            DeshabilitarDivYContenido(miDiv);
        }

        protected void Nombreasesor()
        {
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

                // Realizar la conexión a la base de datos y la consulta para obtener el nombre y apellido del usuario
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Cedula, Zona, Nombre, Apellidos FROM tblEmpleado WHERE Login = @nombreUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", usuariologueado);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string Cedula = reader["Cedula"].ToString();
                            string zona = reader["Zona"].ToString();
                            string nombre = reader["Nombre"].ToString();
                            string apellidos = reader["Apellidos"].ToString();
                           
                            TextZona.Text = zona;
                        }

                    }
                }

            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
        }
    
        protected void Modificar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ShowCase').modal('show');", true);

            Session["ModificarEjecutado"] = true;
          
        }

        protected void BtnModificarNo_Click(object sender, EventArgs e)
        {
            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "btn btn-sm shadow button-disabled";
        
            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "Grabar"
            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled";

            // Habilitar el div y su contenido
            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextUltAc.Enabled = false;

            TextFecOkDib.Enabled = false;

            Button9.Enabled = false;
            Button10.Enabled = false;

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";
        }

        protected void BtnModificarSi_Click(object sender, EventArgs e)
        {
            BtnProgramar.Enabled = false;
            BtnProgramar.CssClass = "btn btn-sm shadow button-disabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "btn btn-sm shadow button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "btn btn-sm shadow button-disabled";

            // Habilitar el botón "Grabar"
            Grabar.Enabled = true;
            Grabar.CssClass = "btn btn-sm shadow button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "btn btn-sm shadow button-enabled";

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            NuevoDisBit.CssClass = "btn btn-sm shadow button-disabled";

            DeshabilitarDivYContenidoMitad(miDiv);
        }

        private void DeshabilitarDivYContenidoMitad(System.Web.UI.Control container)
        {
            TextPla.Enabled = false;
            TextPla.CssClass = "form-control form-control-sm";

            Button16.Enabled = false;
            Button16.CssClass = "form-control form-control-sm";

            Button15.Enabled = false;
            Button15.CssClass = "form-control form-control-sm";

            Button12.Enabled = false;
            Button12.CssClass = "form-control form-control-sm";

            Button13.Enabled = false;
            Button13.CssClass = "form-control form-control-sm";

            Button14.Enabled = false;
            Button14.CssClass = "form-control form-control-sm";

            Button11.Enabled = false;
            Button11.CssClass = "form-control form-control-sm";

            Button9.Enabled = false;
            Button9.CssClass = "form-control form-control-sm";

            Button10.Enabled = false;
            Button10.CssClass = "form-control form-control-sm";




            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Button1.Enabled = false;

            TextDir.Enabled = false;
            TextDir.CssClass = "form-control form-control-sm";

            lblDir.Enabled = false;
            lblDir.CssClass = "col-form-label-sm";

            TextDes.Enabled = false;
            TextDes.CssClass = "form-control form-control-sm";

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            lblDescuento.Enabled = false;
            lblDescuento.CssClass = "col-form-label-sm";

            lblIngDis.Enabled = false;
            lblIngDis.CssClass = "col-form-label-sm";

            lblUltAct.Enabled = false;
            lblUltAct.CssClass = "col-form-label-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            lblPro.Enabled = false;
            lblPro.CssClass = "col-form-label-sm";

            TextProyecto.Enabled = false;
            TextProyecto.CssClass = "form-control form-control-sm";

            lblPla.Enabled = false;
            lblPla.CssClass = "col-form-label-sm";

            lblUrg.Enabled = false;
            lblUrg.CssClass = "col-form-label-sm";

            ChecUrgent.Enabled = false;

            lblCotizar.Enabled = false;
            lblCotizar.CssClass = "col-form-label-sm";

            ChecCot.Enabled = false;

            lblEnt.Enabled = false;
            lblEnt.CssClass = "col-form-label-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            lblEntDib.Enabled = false;
            lblEntDib.CssClass = "col-form-label-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            lblZon.Enabled = false;
            lblZon.CssClass = "col-form-label-sm";

            TextZona.Enabled = false;
            TextZona.CssClass = "form-control form-control-sm";

            lblCon.Enabled = false;
            lblCon.CssClass = "col-form-label-sm";

            TextContacto.Enabled = false;
            TextContacto.CssClass = "form-control form-control-sm";

            lblTel.Enabled = false;
            lblNumDise.CssClass = "col-form-label-sm";

            TextTel.Enabled = false;
            TextTel.CssClass = "form-control form-control-sm";

            lblMaiTer.Enabled = false;
            lblMaiTer.CssClass = "col-form-label-sm";

            ChecMailTer.Enabled = false;

            lblCotVia.Enabled = false;
            lblCotVia.CssClass = "col-form-label-sm";


            ChecCotVia.Enabled = false;

            lblCotTte.Enabled = false;
            lblCotTte.CssClass = "col-form-label-sm";

            CheckBox4.Enabled = false;

            lblAse.Enabled = false;
            lblAse.CssClass = "col-form-label-sm";

            DropDownList1.Enabled = false;

            lblPre.Enabled = false;
            lblPre.CssClass = "col-form-label-sm";

            TextPre.Enabled = false;
            TextPre.CssClass = "form-control form-control-sm";

            lblCel.Enabled = false;
            lblCel.CssClass = "col-form-label-sm";

            TextCel.Enabled = false;
            TextCel.CssClass = "form-control form-control-sm";

            lblMai.Enabled = false;
            lblMai.CssClass = "col-form-label-sm";

            TextMail.Enabled = false;
            TextMail.CssClass = "form-control form-control-sm";

            lblCiuPro.Enabled = false;
            lblCiuPro.CssClass = "col-form-label-sm";

            TextCiuPro.Enabled = false;
            TextCiuPro.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;

            lblConCab.Enabled = false;
            lblCiuPro.CssClass = "form-label";

            ChecConDeCab.Enabled = false;

            lblPis.Enabled = false;
            lblPis.CssClass = "col-form-label-sm g-5";

            ChecPiso.Enabled = false;

            lblDiv.Enabled = false;
            lblDiv.CssClass = "col-form-label-sm g-5";

            ChecDiv.Enabled = false;

            lblCie.Enabled = false;
            lblCie.CssClass = "col-form-label-sm";

            ChecCie.Enabled = false;

            lblCan.Enabled = false;
            lblCan.CssClass = "col-form-label-sm";

            ChecCan.Enabled = false;

            lblBteEle.Enabled = false;
            lblBteEle.CssClass = "col-form-label-sm";

            ChecBteEle.Enabled = false;

            lblBteSw.Enabled = false;
            lblBteSw.CssClass = "col-form-label-sm";

            ChecBteSw.Enabled = false;

            lblSujPt.Enabled = false;
            lblSujPt.CssClass = "col-form-label-sm";

            ChecSujPt.Enabled = false;

            lblAlCie.Enabled = false;
            lblAlCie.CssClass = "col-form-label-sm";

            ChecAlCie.Enabled = false;

            lblPerRef.Enabled = false;
            lblPerRef.CssClass = "col-form-label-sm";

            ChecPerRef.Enabled = false;

            lblGuaEsc.Enabled = false;
            lblGuaEsc.CssClass = "col-form-label-sm";

            ChecGuaEsc.Enabled = false;

            lblHTotCms.Enabled = false;
            lblHTotCms.CssClass = "col-form-label-sm";

            TexHTot.Enabled = false;

            lblEsyMat.Enabled = true;
            lblEsyMat.CssClass = "col-form-label-sm";

            lblLin.Enabled = true;
            lblLin.CssClass = "col-form-label-sm";

            TextLin.Enabled = true;
            TextLin.CssClass = "form-control form-control-sm";

            lblMos.Enabled = true;
            lblMos.CssClass = "col-form-label-sm";

            TextMos.Enabled = true;
            TextMos.CssClass = "form-control form-control-sm";

            lblSup.Enabled = true;
            lblSup.CssClass = "col-form-label-sm";

            TextSup.Enabled = true;
            TextSup.CssClass = "form-control form-control-sm";

            lblBal.Enabled = true;
            lblBal.CssClass = "col-form-label-sm";

            CheckBox16.Enabled = false;

            lblSop.Enabled = true;
            lblSop.CssClass = "col-form-label-sm";

            TextSop.Enabled = true;
            TextSop.CssClass = "form-control form-control-sm";

            lblGav.Enabled = true;
            lblGav.CssClass = "col-form-label-sm";

            TextGav.Enabled = true;
            TextGav.CssClass = "form-control form-control-sm";

            lblPan.Enabled = true;
            lblPan.CssClass = "col-form-label-sm";

            TextPan.Enabled = true;
            TextPan.CssClass = "form-control form-control-sm";

            lblTPie.Enabled = true;
            lblTPie.CssClass = "col-form-label-sm";

            TextTapPie.Enabled = true;
            TextTapPie.CssClass = "form-control form-control-sm";

            lblRep.Enabled = true;
            lblRep.CssClass = "col-form-label-sm";

            TextRep.Enabled = true;
            TextRep.CssClass = "form-control form-control-sm";

            lblTipVid.Enabled = true;
            lblTipVid.CssClass = "col-form-label-sm";

            TextTipVid.Enabled = true;
            TextTipVid.CssClass = "form-control form-control-sm";

            lblPant.Enabled = true;
            lblPant.CssClass = "col-form-label-sm";

            TextPant.Enabled = true;
            TextPant.CssClass = "form-control form-control-sm";

            lblArc.Enabled = true;
            lblArc.CssClass = "col-form-label-sm";

            TextArch.Enabled = true;
            TextArch.CssClass = "form-control form-control-sm";

            lblMue.Enabled = true;
            lblMue.CssClass = "col-form-label-sm";

            ChecMue.Enabled = false;

            lblCoc.Enabled = true;
            lblCoc.CssClass = "col-form-label-sm";

            TextCoc.Enabled = true;
            TextCoc.CssClass = "form-control form-control-sm";

            lblEntr.Enabled = true;
            lblEntr.CssClass = "col-form-label-sm";

            TextEnt.Enabled = true;
            TextEnt.CssClass = "form-control form-control-sm";

            lblPuer.Enabled = true;
            lblPuer.CssClass = "col-form-label-sm";

            TextPuer.Enabled = true;
            TextPuer.CssClass = "form-control form-control-sm";

            CheckBox18.Enabled = true;

            lblPrePpt.Enabled = true;
            lblPrePpt.CssClass = "col-form-label-sm";

            CheckBox19.Enabled = true;

            lblIma.Enabled = true;
            lblIma.CssClass = "col-form-label-sm";

            CheckBox20.Enabled = true;

            lblAcc.Enabled = true;
            lblAcc.CssClass = "col-form-label-sm";

            CheckBox21.Enabled = true;

            lblTieRea.Enabled = true;
            lblTieRea.CssClass = "col-form-label-sm";

            TextFec.Enabled = true;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = true;
            TextFech.CssClass = "form-control form-control-sm";

            lblUbi.Enabled = true;
            TextUbi.CssClass = "col-form-label-sm";


        }

        private string ValidarCampos()
        {
            string campoFaltante = string.Empty;

            if (string.IsNullOrEmpty(TextCiuPro.Text))
            {
                campoFaltante = "Ciudad del Proyecto";
            }
            else if (string.IsNullOrEmpty(TextMail.Text))
            {
                campoFaltante = "Email";
            }
            else if (string.IsNullOrEmpty(TextPre.Text))
            {
                campoFaltante = "Pre";
            }
            else if (string.IsNullOrEmpty(TextCel.Text))
            {
                campoFaltante = "Celular";
            }           
            else if (string.IsNullOrEmpty(TextTel.Text))
            {
                campoFaltante = "Teléfono";
            }
            else if (string.IsNullOrEmpty(TextContacto.Text))
            {
                campoFaltante = "Contacto";
            }
            else if (string.IsNullOrEmpty(TextZona.Text))
            {
                campoFaltante = "Zona";
            }
            else if (string.IsNullOrEmpty(TextFecOkDib.Text))
            {
                campoFaltante = "Fecha de Dibujo OK";
            }
            else if (string.IsNullOrEmpty(TextEntrega.Text))
            {
                campoFaltante = "Fecha Programada de Entrega";
            }       
            else if (string.IsNullOrEmpty(TextPla.Text))
            {
                campoFaltante = "Plano";
            }
            else if (string.IsNullOrEmpty(TextProyecto.Text))
            {
                campoFaltante = "Nombre del Proyecto";
            }
            else if (string.IsNullOrEmpty(TextUltAc.Text))
            {
                campoFaltante = "Última Activación";
            }
            else if (string.IsNullOrEmpty(TextIngDis.Text))
            {
                campoFaltante = "Fecha de Ingreso";
            }
            else if (DropDownList1.SelectedItem == null)
            {
                campoFaltante = "Asesor";
            }
            else if (string.IsNullOrEmpty(TextCliente.Text))
            {
                campoFaltante = "Cliente";
            }
            else if (string.IsNullOrEmpty(TextDir.Text))
            {
                campoFaltante = "Dirección";
            }
            else if (string.IsNullOrEmpty(TextDes.Text))
            {
                campoFaltante = "Descuento";
            } 
            else if (string.IsNullOrEmpty(TexHTot.Text))
            {
                campoFaltante = "H.Total";
            }
            else if (string.IsNullOrEmpty(TextLin.Text))
            {
                campoFaltante = "Línea";
            }
            else if (string.IsNullOrEmpty(TextMos.Text))
            {
                campoFaltante = "Tipo de Mostrador";
            }
            else if (string.IsNullOrEmpty(TextSup.Text))
            {
                campoFaltante = "Acabado de Superficie";
            } 
            else if (string.IsNullOrEmpty(TextSop.Text))
            {
                campoFaltante = "Tipo de Soporte";
            }
            else if (string.IsNullOrEmpty(TextGav.Text))
            {
                campoFaltante = "Tipo de Gaveta";
            }
            else if (string.IsNullOrEmpty(TextPan.Text))
            {
                campoFaltante = "Acabado de Paneles";
            }
            else if (string.IsNullOrEmpty(TextTapPie.Text))
            {
                campoFaltante = "Tipo de Tapa de Pierna";
            }
            else if (string.IsNullOrEmpty(TextRep.Text))
            {
                campoFaltante = "Tipo de Repisa";
            }
            else if (string.IsNullOrEmpty(TextPant.Text))
            {
                campoFaltante = "Tipo de Pantalla";
            }
            else if (string.IsNullOrEmpty(TextTipVid.Text))
            {
                campoFaltante = "Tipo de vidrio";
            }
            else if (string.IsNullOrEmpty(TextArch.Text))
            {
                campoFaltante = "Tipo de Archivador";
            }
            else if (string.IsNullOrEmpty(TextCoc.Text))
            {
                campoFaltante = "Tipo de mueble coco";
            }
            else if (string.IsNullOrEmpty(TextEnt.Text))
            {
                campoFaltante = "Mueble Entrepaño";
            }
            else if (string.IsNullOrEmpty(TextPuer.Text))
            {
                campoFaltante = "Mueble Puertas";
            }
            else if (string.IsNullOrEmpty(TextObsVen.InnerText))
            {
                campoFaltante = "Observaciones de Ventas";
            }
            return campoFaltante;
        }

        protected void btnInsertar_Click(object sender, EventArgs e)
        {
            // Verifica si "NuevoDisBit" se ejecutó previamente
            bool nuevoDisBitEjecutado = Session["NuevoDisBitEjecutado"] != null && (bool)Session["NuevoDisBitEjecutado"];

            // Realiza la validación de campos
            string campoFaltante = ValidarCampos();

            if (string.IsNullOrEmpty(campoFaltante))
            {
                if (nuevoDisBitEjecutado)
                {
                    // Realiza la inserción
                    if (RealizarInsercion())
                    {                     
                        string mensajePersonalizado = "Las Fechas: Ingreso del diseño, Ultima Activación y Entrega, se ajustaran cuando programe el diseño";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }
                    else
                    {
                       
                        string mensajePersonalizado = "No se afecto ninguna fila";
                        string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }

                    Session.Remove("NuevoDisBitEjecutado");
                }
                else
                {
                    // Realiza solo la actualización 
                    if (Session["ModificarEjecutado"] != null && (bool)Session["ModificarEjecutado"])
                    {
                        // Realiza la actualización
                        if (RealizarActualizacion())
                        {                       
                            string mensajePersonalizado = "Las Fechas: Ingreso del diseño, Ultima Activación y Entrega, se ajustaran cuando programe el diseño";
                            string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
         
                        }
                        else
                        {                       
                            string mensajePersonalizado = "El Diseño ya fue aprobado para Dibujo y Despiece, este departamento lo debe habilitar para ser modificado";
                            string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                        Session.Remove("ModificarEjecutado");
                    }
                }
            }
            else
            {
                // Muestra el modal de advertencia si faltan campos
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalll').modal('show'); $('#campoFaltante').text('" + campoFaltante + "');", true);
            }

          
        }

        private int ObtenerMaximoNumeroDiseño(SqlConnection connection)
        {
            string queryGetMaxNumeroDiseño = "SELECT MAX(Numero_Diseño) FROM tbldiseño";
            int numerodiseño = 0;

            using (SqlCommand commandGetMaxNumeroDiseño = new SqlCommand(queryGetMaxNumeroDiseño, connection))
            {
                object result = commandGetMaxNumeroDiseño.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    numerodiseño = Convert.ToInt32(result) + 1;
                }
                else
                {
                    numerodiseño = 1;
                }
            }

            Session["MaxNumeroDiseno"] = numerodiseño;

            return numerodiseño;
        }

        private bool RealizarInsercion()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();

                int numerodiseño = ObtenerMaximoNumeroDiseño(connection);

                if (numerodiseño > 0)
                {
                    using (SqlCommand command = new SqlCommand("sp_InsertarDiseño", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@id_CiudadProyecto", TextCiuPro.Text);
                        command.Parameters.AddWithValue("@Mail", TextMail.Text);
                        command.Parameters.AddWithValue("@PresentacionCotizacion", TextPre.Text);
                        command.Parameters.AddWithValue("@Celular", TextCel.Text);
                        command.Parameters.AddWithValue("@Cotizartransporte", CheckBox4.Checked);
                        command.Parameters.AddWithValue("@CotizarViaticos", ChecCotVia.Checked);
                        command.Parameters.AddWithValue("@MailTerminado", ChecMailTer.Checked);
                        command.Parameters.AddWithValue("@Telefono", TextTel.Text);
                        command.Parameters.AddWithValue("@Contacto", TextContacto.Text);
                        command.Parameters.AddWithValue("@Zona", TextZona.Text);

                        DateTime fechaOkDib = DateTime.ParseExact(TextFecOkDib.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@FechaDibujoOK", fechaOkDib);

                        DateTime fechaProEnt = DateTime.ParseExact(TextEntrega.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@Fecha_Programada_Entrega", fechaProEnt);

                      


                        command.Parameters.AddWithValue("@PasarACotizar", ChecCot.Checked);
                        command.Parameters.AddWithValue("@Urgente", ChecUrgent.Checked);
                        command.Parameters.AddWithValue("@PlanoBitacora", TextPla.Text);
                        command.Parameters.AddWithValue("@Nombre_Diseño", TextProyecto.Text);

                        // Obtener la fecha y hora del TextBox con type="datetime-local"
                        DateTime fechaHora = DateTime.ParseExact(TextUltAc.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                        // Utilizar el valor formateado en el comando SQL
                        command.Parameters.AddWithValue("@UltimaActivacion", fechaHora);

                        DateTime fechaIngreso = DateTime.ParseExact(TextIngDis.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                       
                        command.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);

                       


                        command.Parameters.AddWithValue("@Asesor", DropDownList1.SelectedItem.Text);
                        command.Parameters.AddWithValue("@Cliente", TextCliente.Text);
                        command.Parameters.AddWithValue("@Direccion", TextDir.Text);
                        command.Parameters.AddWithValue("@Descuento", TextDes.Text);
                        command.Parameters.AddWithValue("@ConduccionCablesPiso", ChecPiso.Checked);
                        command.Parameters.AddWithValue("@ConduccionCablesDivision", ChecDiv.Checked);
                        command.Parameters.AddWithValue("@ConduccionCablesCielo", ChecCie.Checked);
                        command.Parameters.AddWithValue("@ConduccionCablesCanaleta", ChecCan.Checked);
                        command.Parameters.AddWithValue("@BajantesElectricos", ChecBteEle.Checked);
                        command.Parameters.AddWithValue("@Bajantesswitches", ChecBteSw.Checked);
                        command.Parameters.AddWithValue("@SujecionCielo", ChecSujPt.Checked);
                        command.Parameters.AddWithValue("@PerfilRefuerzo", ChecPerRef.Checked);
                        command.Parameters.AddWithValue("@GuardaEscobas", ChecGuaEsc.Checked);
                        command.Parameters.AddWithValue("@AlturaCielo", TexHTot.Text);
                        command.Parameters.AddWithValue("@Linea", TextLin.Text);
                        command.Parameters.AddWithValue("@TipoMostrador", TextMos.Text);
                        command.Parameters.AddWithValue("@AcabadoSuperficie", TextSup.Text);
                        command.Parameters.AddWithValue("@BalanceSuperficies", CheckBox16.Checked);
                        command.Parameters.AddWithValue("@TipoSoporte", TextSop.Text);
                        command.Parameters.AddWithValue("@TipoGaveta", TextGav.Text);
                        command.Parameters.AddWithValue("@AcabadoPaneles", TextPan.Text);
                        command.Parameters.AddWithValue("@TipoTapaPierna", TextTapPie.Text);
                        command.Parameters.AddWithValue("@TipoRepisa", TextRep.Text);
                        command.Parameters.AddWithValue("@TipodePantalla", TextPant.Text);

                        command.Parameters.AddWithValue("@TipodeVidrio", TextTipVid.Text);

                        command.Parameters.AddWithValue("@TipoArchivador", TextArch.Text);
                        command.Parameters.AddWithValue("@MuebleCoco", TextCoc.Text);
                        command.Parameters.AddWithValue("@MuebleEntrepano", TextEnt.Text);
                        command.Parameters.AddWithValue("@MueblePuertas", TextPuer.Text);
                        command.Parameters.AddWithValue("@Observaciones_Ventas", TextObsVen.InnerText);
                        command.Parameters.AddWithValue("@Observaciones_Diseño", TextObsDibDes.InnerText);
                        command.Parameters.AddWithValue("@SeguimientoPausa", TextSegPauDev.InnerText);
                        command.Parameters.AddWithValue("@SC_Presentacionppt", CheckBox18.Checked);
                        command.Parameters.AddWithValue("@SC_Imagenes", CheckBox19.Checked);
                        command.Parameters.AddWithValue("@SC_Accesorios", CheckBox20.Checked);
                        command.Parameters.AddWithValue("@SC_Tiemporeal", CheckBox21.Checked);
                        command.Parameters.AddWithValue("@SC_Fecha", TextFec.Text);
                        command.Parameters.AddWithValue("@SC_Ubicacion", TextUbi.Text);

                        command.Parameters.AddWithValue("@Numero_Diseño", numerodiseño);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Session["NumeroDiseño2"] = numerodiseño;

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalExito').modal('show');", true);

                            // Inserción exitosa
                            return true;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);

                            // No se insertó ninguna fila, por lo que la inserción falló
                            return false;
                        }

                    }
                }
                else
                {
                    Response.Write("Error al obtener el número de diseño.");
                    return false;
                }
            }
        }

        private bool RealizarActualizacion()
        {
            // Obtén el número de diseño de la etiqueta lblNumDise
            string numeroDiseño = lblNumDise.Text;

            // Crear la conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Consulta SQL para verificar el campo ProgramadoVentas
                string consulta = "SELECT ProgramadoVentas FROM tbldiseño WHERE Numero_Diseño = @Numero_Diseño";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    // Asignar el valor del parámetro
                    command.Parameters.AddWithValue("@Numero_Diseño", numeroDiseño);

                    // Ejecutar la consulta y obtener el valor de ProgramadoVentas
                    bool programadoVentas = false; // Suponemos que el valor predeterminado es false

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            programadoVentas = reader.GetBoolean(0);
                        }
                    }

                    // Verificar el valor de ProgramadoVentas
                    if (programadoVentas)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ErrorModiciarDiseno').modal('show');", true);
                        return false;
                    }
                }

              
                
                    // Crear el nombre del procedimiento almacenado
                    string storedProcedureName = "sp_ActualizarDiseño";

                    // Crear el comando para ejecutar el procedimiento almacenado
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        // Especificar que el comando es un procedimiento almacenado
                        command.CommandType = CommandType.StoredProcedure;

                        // Asignar los valores a los parámetros del procedimiento almacenado
                        command.Parameters.AddWithValue("@Nombre_Diseño", TextProyecto.Text);


                        command.Parameters.AddWithValue("@Numero_Diseño", lblNumDise.Text);
                        command.Parameters.AddWithValue("@id_CiudadProyecto", TextCiuPro.Text);
                        command.Parameters.AddWithValue("@Mail", TextMail.Text);
                        command.Parameters.AddWithValue("@PresentacionCotizacion", TextPre.Text);
                        command.Parameters.AddWithValue("@Celular", TextCel.Text);
                        command.Parameters.AddWithValue("@Cotizartransporte", CheckBox4.Checked);
                        command.Parameters.AddWithValue("@CotizarViaticos", ChecCotVia.Checked);
                        command.Parameters.AddWithValue("@MailTerminado", ChecMailTer.Checked);
                        command.Parameters.AddWithValue("@Telefono", TextTel.Text);
                        command.Parameters.AddWithValue("@Contacto", TextContacto.Text);
                        command.Parameters.AddWithValue("@Zona", TextZona.Text);

                        DateTime fechaOkDib = DateTime.ParseExact(TextFecOkDib.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@FechaDibujoOK", fechaOkDib);

                        DateTime fechaProEnt = DateTime.ParseExact(TextEntrega.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@Fecha_Programada_Entrega", fechaProEnt);

                        command.Parameters.AddWithValue("@PasarACotizar", ChecCot.Checked);
                        command.Parameters.AddWithValue("@Urgente", ChecUrgent.Checked);
                        command.Parameters.AddWithValue("@PlanoBitacora", TextPla.Text);

                        // Obtener la fecha y hora del TextBox con type="datetime-local"
                        DateTime fechaHora = DateTime.ParseExact(TextUltAc.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

                        // Utilizar el valor formateado en el comando SQL
                        command.Parameters.AddWithValue("@UltimaActivacion", fechaHora);

                        DateTime fechaIngreso = DateTime.ParseExact(TextIngDis.Text, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);


                        command.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);

                        command.Parameters.AddWithValue("@Asesor", DropDownList1.SelectedItem.Text);
                        command.Parameters.AddWithValue("@Cliente", TextCliente.Text);
                        command.Parameters.AddWithValue("@Direccion", TextDir.Text);
                        command.Parameters.AddWithValue("@Descuento", TextDes.Text);
                        command.Parameters.AddWithValue("@ConduccionCablesPiso", ChecPiso.Checked);
                        command.Parameters.AddWithValue("@ConduccionCablesDivision", ChecDiv.Checked);
                        command.Parameters.AddWithValue("@ConduccionCablesCielo", ChecCie.Checked);
                        command.Parameters.AddWithValue("@ConduccionCablesCanaleta", ChecCan.Checked);
                        command.Parameters.AddWithValue("@BajantesElectricos", ChecBteEle.Checked);
                        command.Parameters.AddWithValue("@Bajantesswitches", ChecBteSw.Checked);
                        command.Parameters.AddWithValue("@SujecionCielo", ChecSujPt.Checked);
                        command.Parameters.AddWithValue("@PerfilRefuerzo", ChecPerRef.Checked);
                        command.Parameters.AddWithValue("@GuardaEscobas", ChecGuaEsc.Checked);
                        command.Parameters.AddWithValue("@AlturaCielo", TexHTot.Text);
                        command.Parameters.AddWithValue("@Linea", TextLin.Text);
                        command.Parameters.AddWithValue("@TipoMostrador", TextMos.Text);
                        command.Parameters.AddWithValue("@AcabadoSuperficie", TextSup.Text);
                        command.Parameters.AddWithValue("@BalanceSuperficies", CheckBox16.Text);
                        command.Parameters.AddWithValue("@TipoSoporte", TextSop.Text);
                        command.Parameters.AddWithValue("@TipoGaveta", TextGav.Text);
                        command.Parameters.AddWithValue("@AcabadoPaneles", TextPan.Text);
                        command.Parameters.AddWithValue("@TipoTapaPierna", TextTapPie.Text);
                        command.Parameters.AddWithValue("@TipoRepisa", TextRep.Text);
                        command.Parameters.AddWithValue("@TipodePantalla", TextPant.Text);
                        command.Parameters.AddWithValue("@TipodeVidrio", TextTipVid.Text);
                        command.Parameters.AddWithValue("@TipoArchivador", TextArch.Text);
                        command.Parameters.AddWithValue("@MuebleCoco", TextCoc.Text);
                        command.Parameters.AddWithValue("@MuebleEntrepano", TextEnt.Text);
                        command.Parameters.AddWithValue("@MueblePuertas", TextPuer.Text);
                        command.Parameters.AddWithValue("@Observaciones_Ventas", TextObsVen.InnerText);
                        command.Parameters.AddWithValue("@Observaciones_Diseño", TextObsDibDes.InnerText);
                        command.Parameters.AddWithValue("@SeguimientoPausa", TextSegPauDev.InnerText);
                        command.Parameters.AddWithValue("@SC_Presentacionppt", CheckBox18.Checked);
                        command.Parameters.AddWithValue("@SC_Imagenes", CheckBox19.Checked);
                        command.Parameters.AddWithValue("@SC_Accesorios", CheckBox20.Checked);
                        command.Parameters.AddWithValue("@SC_Tiemporeal", CheckBox21.Checked);
                        command.Parameters.AddWithValue("@SC_Fecha", TextFec.Text);
                        command.Parameters.AddWithValue("@SC_Ubicacion", TextUbi.Text);
                        // Ejecutar el procedimiento almacenado
                        int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["NumeroDiseño2"] = lblNumDise.Text;

                       

                        // Inserción exitosa
                        return true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);

                        // No se insertó ninguna fila, por lo que la inserción falló
                        return false;
                    }
                }
                }
               
            
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button9
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button10
        }

        //DATAGRID DISEÑO
        protected void DataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string programadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();
                string pasarACotizar = DataBinder.Eval(e.Item.DataItem, "PasarACotizar").ToString();
                string terminadoDibujo = DataBinder.Eval(e.Item.DataItem, "TerminadoDibujo").ToString();
                string pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();
                string FechaEntrega = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();
                string urgente = DataBinder.Eval(e.Item.DataItem, "Urgente").ToString();

                if (DateTime.TryParse(FechaEntrega, out DateTime fechaEntrega))
                {
                    if (fechaEntrega < DateTime.Now && programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {

                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /* Rojo */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False" && urgente == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /* Amarillo */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (programadoVentas == "True" && pasarACotizar == "True" && terminadoDibujo == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /* Violeta */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#819cba"); /* Azul */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "True" && pasarACotizar == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#be94b9"); /* Rosado */
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (pausado == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#74bec6"); /* Celeste */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (urgente == "True" && programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#e9a270"); /* Naranja */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }





                    // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                    string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                    string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                    TableCell descripcionCell = e.Item.Cells[3];

                    if (descripcionCell != null)
                    {
                        descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                    }
                }
            }
        }

        protected void DataGridDise_ItemCommand(object source, DataGridCommandEventArgs e)
        {
           
                ProcesarNumeroDiseño(e);
           
        }

        private void ProcesarNumeroDiseño(DataGridCommandEventArgs e)
        {
            string numeroDiseño = Session["NumeroDiseño"].ToString();


            if (int.TryParse(numeroDiseño, out int numDise))
            {
                // Establecer el valor del parámetro en el SqlDataSource
                SqldatasourceTxt.SelectParameters["NumeroDiseño"].DefaultValue = numDise.ToString();

                // Ejecutar el SqlDataSource
                SqldatasourceTxt.DataBind();

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
                            // ProgramadoVentas es 1, deshabilitar los botones
                            BtnProgramar.Enabled = false;
                            GuardarButton.Enabled = false;
                            GuardarButton.CssClass = "btn btn-sm button-disabled";
                            BtnEliminar.Enabled = false;
                            BtnEliminar.CssClass = "btn btn-sm button-disabled";
           
                        }
                        else
                        {
                            // ProgramadoVentas es 0, habilitar los botones
                            BtnProgramar.Enabled = true;
                            GuardarButton.Enabled = true;
                            GuardarButton.CssClass = "btn btn-sm btn-outline-dark button-enabled";
                            BtnEliminar.Enabled = true;
                            BtnEliminar.CssClass = "btn btn-sm btn-outline-dark button-enabled";
                  

                        }
                    }

                    using (SqlCommand command = new SqlCommand("sp_FormularioDisBita", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NumDise", numDise);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int idCiudadProyecto = reader.GetInt32(reader.GetOrdinal("id_CiudadProyecto"));

                                // Asignar el valor de "id_CiudadProyecto" al DropDownList
                                TextCiuPro.SelectedValue = idCiudadProyecto.ToString();

                                // Asignar el valor de "Numero_Diseño" al lblNumDise
                                int numeroDiseno;
                                if (int.TryParse(reader["Numero_Diseño"].ToString(), out numeroDiseno))
                                {
                                    lblNumDise.Text = numeroDiseno.ToString();
                                    Session["NumeroDisenoSession"] = numeroDiseno;
                                    Session["EventoItemCommandEjecutado"] = true;
                                }
                                else
                                {
                                    lblNumDise.Text = "Valor no válido";
                                    Session["EventoItemCommandEjecutado"] = false;
                                }


                                // Validar ProgramadoVentas
                                bool programadoVentas = reader.GetBoolean(reader.GetOrdinal("ProgramadoVentas"));

                                if (programadoVentas)
                                {
                                    TextIngDis.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso")).ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = reader.GetDateTime(reader.GetOrdinal("UltimaActivacion")).ToString("yyyy-MM-ddTHH:mm");
                                    TextEntrega.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega")).ToString("yyyy-MM-ddTHH:mm");
                                    TextFecOkDib.Text = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK")).ToString("yyyy-MM-ddTHH:mm");

                                    TextFec.Text = reader.GetDateTime(reader.GetOrdinal("SC_Fecha")).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    // Asignar las fechas cuando ProgramadoVentas es 0
                                    DateTime now = DateTime.Now;
                                    TextFec.Text = now.ToString("yyyy-MM-dd");
                                    //TextFech.Text = now.ToString("HH:mm");
                                    TextIngDis.Text = now.ToString("yyyy-MM-ddTHH:mm");
                                    string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = fechaHoraActual;
                                    TextFecOkDib.Text = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK")).ToString("yyyy-MM-ddTHH:mm");                                                           
                                    TextEntrega.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega")).ToString("yyyy-MM-ddTHH:mm");
                                }

                                //TextFech.Text = reader.GetDateTime(reader.GetOrdinal("SC_Hora")).ToString("HH:mm");

                                TextZona.Text = GetString(reader, "Zona");

                                TextPre.Text = GetString(reader, "PresentacionCotizacion");

                                string asesor = GetString(reader, "Asesor");
                                ListItem asesorItem = new ListItem(asesor, asesor);
                                DropDownList1.Items.Add(asesorItem);

                                // Selecciona el valor en DropDownList1 para que coincida con el "Asesor" obtenido de la base de datos
                                DropDownList1.SelectedValue = asesor;


                                TextCliente.Text = GetString(reader, "Cliente");
                                TextProyecto.Text = GetString(reader, "Nombre_Diseño");
                                TextContacto.Text = GetString(reader, "Contacto");
                                TextTel.Text = GetString(reader, "Telefono");
                                TextCel.Text = GetString(reader, "Celular");
                                TextMail.Text = GetString(reader, "Mail");
                                TextDir.Text = GetString(reader, "Direccion");
                                TextDes.Text = GetString(reader, "Descuento");
                                TextPla.Text = GetString(reader, "PlanoBitacora");
                                ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("Urgente"));
                                ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("PasarACotizar"));
                                ChecMailTer.Checked = reader.GetBoolean(reader.GetOrdinal("MailTerminado"));

                                ChecCotVia.Checked = reader.GetBoolean(reader.GetOrdinal("CotizarViaticos"));

                                CheckBox4.Checked = reader.GetBoolean(reader.GetOrdinal("Cotizartransporte"));

                                CheckBox16.Checked = reader.GetBoolean(reader.GetOrdinal("BalanceSuperficies"));

                                CheckBox18.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Presentacionppt"));

                                CheckBox19.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Imagenes"));

                                CheckBox20.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Accesorios"));

                                CheckBox21.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Tiemporeal"));

                                TexHTot.Text = GetString(reader, ("AlturaCielo"));

                                TextUbi.Text = GetString(reader, ("SC_Ubicacion"));

                                ChecPiso.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesPiso"));
                                ChecCie.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCielo"));
                                ChecBteEle.Checked = reader.GetBoolean(reader.GetOrdinal("BajantesElectricos"));
                                ChecBteSw.Checked = reader.GetBoolean(reader.GetOrdinal("Bajantesswitches"));
                                ChecDiv.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesDivision"));
                                ChecCan.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCanaleta"));

                                ChecAlCie.Checked = reader.GetBoolean(reader.GetOrdinal("SujecionCielo"));
                                ChecPerRef.Checked = reader.GetBoolean(reader.GetOrdinal("PerfilRefuerzo"));
                                ChecGuaEsc.Checked = reader.GetBoolean(reader.GetOrdinal("GuardaEscobas"));

                                TextSup.Text = GetString(reader, ("AcabadoSuperficie"));
                                TextPan.Text = GetString(reader, ("AcabadoPaneles"));
                                TextTipVid.Text = GetString(reader, ("TipodeVidrio"));
                                TextLin.Text = GetString(reader, ("Linea"));
                                TextSop.Text = GetString(reader, ("TipoSoporte"));
                                TextTapPie.Text = GetString(reader, ("TipoTapaPierna"));
                                TextPant.Text = GetString(reader, ("TipodePantalla"));
                                TextMos.Text = GetString(reader, ("TipoMostrador"));
                                TextGav.Text = GetString(reader, ("TipoGaveta"));
                                TextRep.Text = GetString(reader, ("TipoRepisa"));
                                TextArch.Text = GetString(reader, ("TipoArchivador"));
                                TextCoc.Text = GetString(reader, ("MuebleCoco"));
                                TextEnt.Text = GetString(reader, ("MuebleEntrepano"));
                                TextPuer.Text = GetString(reader, ("MueblePuertas"));
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Ventas")))
                                {
                                    TextObsVen.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Ventas"));
                                }
                                else
                                {
                                    // Si el valor es nulo, puedes manejarlo de alguna manera, por ejemplo, asignar un valor predeterminado a TextObsVen.InnerText
                                    TextObsVen.InnerText = " ";
                                }
                                // Para TextObsDibDes
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Diseño")))
                                {
                                    TextObsDibDes.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Diseño"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextObsDibDes
                                    TextObsDibDes.InnerText = " ";
                                }

                                // Para TextSegPauDev
                                if (!reader.IsDBNull(reader.GetOrdinal("SeguimientoPausa")))
                                {
                                    TextSegPauDev.InnerText = reader.GetString(reader.GetOrdinal("SeguimientoPausa"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextSegPauDev
                                    TextSegPauDev.InnerText = " ";
                                }
                            }
                        }
                    }
                }

                // Actualizar el panel de diseño de bitácora
                UpdateDiseñoBitacora.Update();
            }
        }

        private void ProcesarNumeroDiseño2(DataGridCommandEventArgs e)
        {
            string numeroDiseño = Session["NumeroDiseño2"].ToString();


            if (int.TryParse(numeroDiseño, out int numDise))
            {
                // Establecer el valor del parámetro en el SqlDataSource
                SqldatasourceTxt.SelectParameters["NumeroDiseño"].DefaultValue = numDise.ToString();

                // Ejecutar el SqlDataSource
                SqldatasourceTxt.DataBind();

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
                            // ProgramadoVentas es 1, deshabilitar los botones
                            BtnProgramar.Enabled = false;
                            GuardarButton.Enabled = false;
                            GuardarButton.CssClass = "btn btn-sm button-disabled";
                            BtnEliminar.Enabled = false;
                            BtnEliminar.CssClass = "btn btn-sm button-disabled";
                        }
                        else
                        {
                            // ProgramadoVentas es 0, habilitar los botones
                            BtnProgramar.Enabled = true;
                            GuardarButton.Enabled = true;
                            GuardarButton.CssClass = "btn btn-sm btn-outline-dark button-enabled";
                            BtnEliminar.Enabled = true;
                            BtnEliminar.CssClass = "btn btn-sm btn-outline-dark button-enabled";
                        }
                    }

                    using (SqlCommand command = new SqlCommand("sp_FormularioDisBita", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NumDise", numDise);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int idCiudadProyecto = reader.GetInt32(reader.GetOrdinal("id_CiudadProyecto"));

                                // Asignar el valor de "id_CiudadProyecto" al DropDownList
                                TextCiuPro.SelectedValue = idCiudadProyecto.ToString();

                                // Asignar el valor de "Numero_Diseño" al lblNumDise
                                int numeroDiseno;
                                if (int.TryParse(reader["Numero_Diseño"].ToString(), out numeroDiseno))
                                {
                                    lblNumDise.Text = numeroDiseno.ToString();
                                    Session["NumeroDisenoSession"] = numeroDiseno;
                                    Session["EventoItemCommandEjecutado"] = true;
                                }
                                else
                                {
                                    lblNumDise.Text = "Valor no válido";
                                    Session["EventoItemCommandEjecutado"] = false;
                                }

                                // Validar ProgramadoVentas
                                bool programadoVentas = reader.GetBoolean(reader.GetOrdinal("ProgramadoVentas"));

                                if (programadoVentas)
                                {
                                    TextIngDis.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso")).ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = reader.GetDateTime(reader.GetOrdinal("UltimaActivacion")).ToString("yyyy-MM-ddTHH:mm");
                                    TextEntrega.Text = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega")).ToString("yyyy-MM-ddTHH:mm");
                                    TextFecOkDib.Text = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK")).ToString("yyyy-MM-ddTHH:mm");

                                    TextFec.Text = reader.GetDateTime(reader.GetOrdinal("SC_Fecha")).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    // Asignar las fechas cuando ProgramadoVentas es 0
                                    DateTime now = DateTime.Now;
                                    TextFec.Text = now.ToString("yyyy-MM-dd");
                                    //TextFech.Text = now.ToString("HH:mm");
                                    TextIngDis.Text = now.ToString("yyyy-MM-ddTHH:mm");
                                    string fechaHoraActual = now.ToString("yyyy-MM-ddTHH:mm");
                                    TextUltAc.Text = fechaHoraActual;
                                    TextFecOkDib.Text = now.ToString("yyyy-MM-ddTHH:mm");
                                    DateTime fechaActual = DateTime.Now;
                                    DateTime fechaEntrega = fechaActual.AddDays(5);
                                    TextEntrega.Text = fechaEntrega.ToString("yyyy-MM-ddTHH:mm");
                                }

                                //TextFech.Text = reader.GetDateTime(reader.GetOrdinal("SC_Hora")).ToString("HH:mm");

                                TextZona.Text = GetString(reader, "Zona");

                                TextPre.Text = GetString(reader, "PresentacionCotizacion");

                                string asesor = GetString(reader, "Asesor");
                                ListItem asesorItem = new ListItem(asesor, asesor);
                                DropDownList1.Items.Add(asesorItem);

                                // Selecciona el valor en DropDownList1 para que coincida con el "Asesor" obtenido de la base de datos
                                DropDownList1.SelectedValue = asesor;


                                TextCliente.Text = GetString(reader, "Cliente");
                                TextProyecto.Text = GetString(reader, "Nombre_Diseño");
                                TextContacto.Text = GetString(reader, "Contacto");
                                TextTel.Text = GetString(reader, "Telefono");
                                TextCel.Text = GetString(reader, "Celular");
                                TextMail.Text = GetString(reader, "Mail");
                                TextDir.Text = GetString(reader, "Direccion");
                                TextDes.Text = GetString(reader, "Descuento");
                                TextPla.Text = GetString(reader, "PlanoBitacora");
                                ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("Urgente"));
                                ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("PasarACotizar"));
                                ChecMailTer.Checked = reader.GetBoolean(reader.GetOrdinal("MailTerminado"));

                                ChecCotVia.Checked = reader.GetBoolean(reader.GetOrdinal("CotizarViaticos"));

                                CheckBox4.Checked = reader.GetBoolean(reader.GetOrdinal("Cotizartransporte"));

                                CheckBox16.Checked = reader.GetBoolean(reader.GetOrdinal("BalanceSuperficies"));

                                CheckBox18.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Presentacionppt"));

                                CheckBox19.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Imagenes"));

                                CheckBox20.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Accesorios"));

                                CheckBox21.Checked = reader.GetBoolean(reader.GetOrdinal("SC_Tiemporeal"));

                                TexHTot.Text = GetString(reader, ("AlturaCielo"));

                                TextUbi.Text = GetString(reader, ("SC_Ubicacion"));

                                ChecPiso.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesPiso"));
                                ChecCie.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCielo"));
                                ChecBteEle.Checked = reader.GetBoolean(reader.GetOrdinal("BajantesElectricos"));
                                ChecBteSw.Checked = reader.GetBoolean(reader.GetOrdinal("Bajantesswitches"));
                                ChecDiv.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesDivision"));
                                ChecCan.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCanaleta"));

                                ChecAlCie.Checked = reader.GetBoolean(reader.GetOrdinal("SujecionCielo"));
                                ChecPerRef.Checked = reader.GetBoolean(reader.GetOrdinal("PerfilRefuerzo"));
                                ChecGuaEsc.Checked = reader.GetBoolean(reader.GetOrdinal("GuardaEscobas"));

                                TextSup.Text = GetString(reader, ("AcabadoSuperficie"));
                                TextPan.Text = GetString(reader, ("AcabadoPaneles"));
                                TextTipVid.Text = GetString(reader, ("TipodeVidrio"));
                                TextLin.Text = GetString(reader, ("Linea"));
                                TextSop.Text = GetString(reader, ("TipoSoporte"));
                                TextTapPie.Text = GetString(reader, ("TipoTapaPierna"));
                                TextPant.Text = GetString(reader, ("TipodePantalla"));
                                TextMos.Text = GetString(reader, ("TipoMostrador"));
                                TextGav.Text = GetString(reader, ("TipoGaveta"));
                                TextRep.Text = GetString(reader, ("TipoRepisa"));
                                TextArch.Text = GetString(reader, ("TipoArchivador"));
                                TextCoc.Text = GetString(reader, ("MuebleCoco"));
                                TextEnt.Text = GetString(reader, ("MuebleEntrepano"));
                                TextPuer.Text = GetString(reader, ("MueblePuertas"));
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Ventas")))
                                {
                                    TextObsVen.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Ventas"));
                                }
                                else
                                {
                                    // Si el valor es nulo, puedes manejarlo de alguna manera, por ejemplo, asignar un valor predeterminado a TextObsVen.InnerText
                                    TextObsVen.InnerText = " ";
                                }
                                // Para TextObsDibDes
                                if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Diseño")))
                                {
                                    TextObsDibDes.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Diseño"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextObsDibDes
                                    TextObsDibDes.InnerText = " ";
                                }

                                // Para TextSegPauDev
                                if (!reader.IsDBNull(reader.GetOrdinal("SeguimientoPausa")))
                                {
                                    TextSegPauDev.InnerText = reader.GetString(reader.GetOrdinal("SeguimientoPausa"));
                                }
                                else
                                {
                                    // Manejo del valor nulo o vacío para TextSegPauDev
                                    TextSegPauDev.InnerText = " ";
                                }
                            }
                        }
                    }
                }

                // Actualizar el panel de diseño de bitácora
                UpdateDiseñoBitacora.Update();
            }
        }

        protected void DataGridSC(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Numero_Diseño")
            {
                ProcesarNumeroDiseño(e);
            }
        }

        protected void DataGridBusDise_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Numero_Diseño")
            {
                ProcesarNumeroDiseño(e);
            }
        }

        private string GetString(SqlDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return reader.GetString(reader.GetOrdinal(columnName));
            }
            return string.Empty;
        }

        private void UpdateDataGrids()
        {
            DataGrid1.DataBind();
            DataGrid2.DataBind();
            DataGridDiseños.DataBind();
            DataGridRender.DataBind();
            UpdatePanel1.Update();
        }

        protected void DropDownListOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = DropDownListOptions.SelectedValue;

            if (selectedValue == "01")
            {
                SqlDataSource1.FilterExpression = "Zona = '01'";
                DataGridDiseño.FilterExpression = "Zona = '01'";
                DataGridDiseñosPorFecha.FilterExpression = "Zona = '01'";
                DataGridRenderPorFechaYAsesor.FilterExpression = "Zona = '01'";
            }
            else if (selectedValue == "02")
            {
                SqlDataSource1.FilterExpression = "Zona = '02'";
                DataGridDiseño.FilterExpression = "Zona = '02'";
                DataGridDiseñosPorFecha.FilterExpression = "Zona = '02'";
                DataGridRenderPorFechaYAsesor.FilterExpression = "Zona = '02'";
            }
            else
            {
                SqlDataSource1.FilterExpression = "";
                DataGridDiseño.FilterExpression = "";
                DataGridDiseñosPorFecha.FilterExpression = "";
                DataGridRenderPorFechaYAsesor.FilterExpression = "";
            }

            UpdateDataGrids();
        }

        protected void Button88_Click(object sender, EventArgs e)
        {
            UpdateDataGrids();
        }

        //DATAGRID SHOWCASE
        protected void DataGrid3_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string scFecha = DataBinder.Eval(e.Item.DataItem, "SC_Fecha").ToString();
                DateTime fechaSC;

                // Convertir la cadena de fecha a un objeto DateTime
                if (DateTime.TryParse(scFecha, out fechaSC))
                {
                    // Verificar si la fecha es igual o menor a la fecha actual
                    if (fechaSC <= DateTime.Now)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); // Rojo
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); // Amarillo
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                }


                // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                // Encontrar la celda correspondiente a la columna Descripción por índice
                TableCell descripcionCell = e.Item.Cells[3]; // Ajusta el índice si es necesario

                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                if (descripcionCell != null)
                {
                    descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                }
            }
            }
        
        protected void DataGrid4_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string terminadoRender = DataBinder.Eval(e.Item.DataItem, "TerminadoRender").ToString();
                string fechaProgramadaString = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();
                string Pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();
                string ProgramadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();


                if (terminadoRender == "False")
                {
                    if (!string.IsNullOrEmpty(fechaProgramadaString) && DateTime.TryParse(fechaProgramadaString, out DateTime fechaProgramada))
                    {
                        if (fechaProgramada < DateTime.Now && Pausado == "False" && ProgramadoVentas == "True")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /*Rojo*/
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }
                        else if (fechaProgramada > DateTime.Now && Pausado == "False" && ProgramadoVentas == "True")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                            e.Item.ForeColor = System.Drawing.Color.Black;
                        }
                        else if (Pausado == "True")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#74bec6"); /*Celeste*/
                            e.Item.ForeColor = System.Drawing.Color.Black;
                        }
                        else if (ProgramadoVentas == "False")
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /*Violeta*/
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }

                    }
                }
              
               
            }



            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
               string nombreRender = DataBinder.Eval(e.Item.DataItem, "Nombre_Render").ToString();

                // Encontrar la celda correspondiente a la columna Descripción por índice
               TableCell descripcionCell = e.Item.Cells[3]; // Ajusta el índice si es necesario
                                
                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
               if (descripcionCell != null)
                {
                  descripcionCell.Text = $"{cliente} - {nombreRender}";
               }
            }
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DateTime fechaEntrega = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Entrega_Dibujo_Despiece"));
                DateTime fechaActualMenos2Dias = DateTime.Now.AddDays(-2);

                // Verificar si la fecha de entrega es sábado o domingo
                if (fechaEntrega.DayOfWeek == DayOfWeek.Saturday)
                {
                    // Cambiar la fecha de entrega al próximo lunes
                    fechaEntrega = fechaEntrega.AddDays(2);
                }
                else if (fechaEntrega.DayOfWeek == DayOfWeek.Sunday)
                {
                    // Cambiar la fecha de entrega al próximo martes
                    fechaEntrega = fechaEntrega.AddDays(1);
                }

                // Verificar si la nueva fecha de entrega es menos de 5 días antes de la fecha actual
                if (fechaEntrega < fechaActualMenos2Dias)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /*rojo*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#efdd79"); /*Amarillo*/
                    e.Item.ForeColor = System.Drawing.Color.Black;
                }

                // Acceder a la celda correspondiente y asignarle el valor de fechaEntrega
                TableCell cellFechaEntrega = e.Item.Cells[6]; // Cambia el índice si la columna no está en la sexta posición
                cellFechaEntrega.Text = fechaEntrega.ToString("dd/MM/yyyy hh:mm:ss tt");

                // Obtener la referencia al control Label dentro de la columna de la fecha de entrega
                System.Web.UI.WebControls.Label labelFechaEntrega = (System.Web.UI.WebControls.Label)e.Item.FindControl("Label1");

                // Verificar si se encontró el control Label
                if (labelFechaEntrega != null)
                {
                    // Calcular la fecha de entrega para el control Label y asignarla como texto
                    DateTime nuevaFechaEntrega = fechaEntrega.AddDays(2);
                    labelFechaEntrega.Text = nuevaFechaEntrega.ToString("dd/MM/yyyy hh:mm:ss tt");
                }
            }
        }

        protected void CheckBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox22.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal1').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        protected void CheckBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox23.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal2').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        } 

        protected void DataGridBusDis_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string programadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();
                string pasarACotizar = DataBinder.Eval(e.Item.DataItem, "PasarACotizar").ToString();
                string terminadoDibujo = DataBinder.Eval(e.Item.DataItem, "TerminadoDibujo").ToString();
                string pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();
                string CotizacionOk = DataBinder.Eval(e.Item.DataItem, "CotizaciónOK").ToString();
                string FechaEntrega = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();


                if (DateTime.TryParse(FechaEntrega, out DateTime fechaEntrega))
                {
                    if (fechaEntrega < DateTime.Now && programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {

                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868"); /* Rojo */
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }

                    else if (programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                        e.Item.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (programadoVentas == "True" && terminadoDibujo == "True" && CotizacionOk == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#3d803f"); /*Verde*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }

                    else if (programadoVentas == "True" && pasarACotizar == "True" && terminadoDibujo == "True")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /*Violeta*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#819cba"); /*Azul*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }
                    else if (programadoVentas == "True" && pasarACotizar == "False")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#be94b9"); /*Rosado*/
                        e.Item.ForeColor = System.Drawing.Color.White;
                    }






             
                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
           
        }

        protected void DocBitacora_Click(object sender, EventArgs e)
        {
            ToggleDivsVisibility();
            ConfigureSqlDataSource();
            DataGridDocumento.DataBind();
        }

        private void ConfigureSqlDataSource()
        {
            string numDise = "DS" + lblNumDise.Text; // Asegúrate de que lblNumDise esté disponible
            string query = $"SELECT Archivo, Observacion, Usuario, FechaRegistro, Id_OT FROM tblDocumentacion WHERE Id_OT = '{numDise}'";
            SqlDataSource3.SelectCommand = query;
        }

        private void ToggleDivsVisibility()
        {
            if (miDiv.Style["display"] == "block")
            {
                miDiv.Style["display"] = "none";
                Documentacion.Style["display"] = "block";
            }
            else
            {
                miDiv.Style["display"] = "block";
                Documentacion.Style["display"] = "none";
            }
        }

        private void UpdateDivsVisibility()
        {
            miDiv.Style["display"] = "block";
            Documentacion.Style["display"] = "none";
        }

        protected void GuardarButton_Click(object sender, EventArgs e)
        {


            if (FileUpload1.HasFile)
            {
                HttpPostedFile uploadedFile = FileUpload1.PostedFile;

                // Obtener el nombre del archivo
                string fileName = Path.GetFileName(uploadedFile.FileName);

                // Obtener el texto de lblNumDise
                string lblText = lblNumDise.Text;

                // Concatenar "DS" con el texto de lblNumDise para obtener el nombre de la carpeta
                string folderName = "DS" + lblText;

                // Combinar la ruta de guardado con el nombre de la carpeta
                string savePath = Path.Combine(@"\\SRVFS\PruebaDocumentacion", folderName);

                // Verificar si la carpeta no existe y crearla si es necesario
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string filePath = Path.Combine(savePath, fileName);

                // Guardar el archivo
                uploadedFile.SaveAs(filePath);

                InsertarEnBaseDeDatos(folderName, fileName);
          
                ToggleDivsVisibility();

                Session["NumeroDiseño2"] = lblText;

                Session["Documentacion"] = true;

                string mensajePersonalizado = "El documento se ha guardado exitosamente.";
                string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

            }
            else
            {
                string mensajePersonalizado = "Seleccione el archivo que desea adjuntar";
                string urlRedireccion = "Ventas/Diseño_Venta.aspx";
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
                    cmd.Parameters.AddWithValue("@Usuario", DropDownList1.SelectedValue); // Obtener el valor del DropDownList
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

                    // Ejecutar la consulta
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Comprobar si se actualizó al menos una fila
                    if (rowsAffected > 0)
                    {

                    }
                    else
                    {

                    }
                }
            }
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

        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            string lblText = lblNumDise.Text;
            string idOTToDelete = Session["SelectedIdOT"] as string;

            if (!string.IsNullOrEmpty(idOTToDelete))
            {
                string archivoToDelete = Session["SelectedFileName"] as string;

                // Ruta completa del archivo a eliminar
                string filePathToDelete = Path.Combine(@"\\SRVFS\PruebaDocumentacion\", idOTToDelete, archivoToDelete);

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

                        ToggleDivsVisibility();

                        Session["NumeroDiseño"] = lblText;

                        if (rowsAffected > 0)
                        {
                            string mensajePersonalizado = "El documento se ha eliminado exitosamente.";
                            string urlRedireccion = "Ventas/Diseño_Venta.aspx"; // Cambia esto por la URL correcta
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                        else
                        {
                            string mensajePersonalizado = "Seleccione el elemento que desea eliminar";
                            string urlRedireccion = "Ventas/Diseño_Venta.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                    }
                }

                // Vuelve a enlazar los datos en el DataGridDocumento después de la eliminación
                DataGridDocumento.DataBind();
            }
        }

        protected void lnkViewFile_Click(object sender, EventArgs e)
        {
            LinkButton lnkViewFile = (LinkButton)sender;
            int rowIndex = Convert.ToInt32(lnkViewFile.CommandArgument);
            DataGridItem selectedRow = DataGridDocumento.Items[rowIndex];
            string archivo = selectedRow.Cells[1].Text;

            // Construye la ruta completa al archivo
            string rutaArchivo = @"P:\SISTEMAS\PruebaDocumentacion\" + archivo;

            try
            {
                System.Diagnostics.Process.Start(rutaArchivo);
            }
            catch (Exception ex)
            {
                litModalScript.Text = "<script type='text/javascript'>$(document).ready(function () { $('#miModalErrorAdj').modal('show'); });</script>";
            }
        }

        protected void lnkSelectRowRender_Click(object sender, EventArgs e)
        {
                
        }

        //protected void btnUpload_Click(object sender, EventArgs e)
        // {
        //    if (FileUpload1.HasFile)
        //    {


        //        string fileName = FileUpload1.FileName;

        //        // Obtener el texto de lblNumDise
        //        string lblText = lblNumDise.Text;

        //        // Concatenar "DS" con el texto de lblNumDise para obtener el nombre de la carpeta
        //        string folderName = "DS" + lblText;

        //        // Combina la ruta de guardado con el nombre de la carpeta
        //        string savePath = Path.Combine(@"P:\SISTEMAS\PruebaDocumentacion", folderName);

        //        // Verifica si la carpeta no existe y créala si es necesario
        //        if (!Directory.Exists(savePath))
        //        {
        //            Directory.CreateDirectory(savePath);
        //        }

        //        string filePath = Path.Combine(savePath, fileName);

        //        FileUpload1.SaveAs(filePath);

        //        InsertarEnBaseDeDatos(folderName, fileName);

        //        // Puedes guardar la ruta del archivo en tu base de datos si es necesario
        //        // GuardarRutaEnBaseDeDatos(filePath);

        //        // Actualiza el TextBox con el nombre del archivo seleccionado
        //        TextBox2.Text = fileName;

        //               // Muestra un mensaje de éxito o realiza otras acciones necesarias
        //              lblMessage.Text = "Archivo subido exitosamente.";

        //                  // Actualiza el UpdatePanel para reflejar los cambios en la página

        //   }
        //                        else
        //                        {
        //                            lblMessage.Text = "Por favor, selecciona un archivo para subir.";
        //                        }




        //}


        //private void InsertarEnBaseDeDatos(string folderName, string fileName)
        //{
        //    // Establecer la conexión con la base de datos
        //    using (SqlConnection con = new SqlConnection("Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"))
        //    {
        //        con.Open();

        //        // Crear la consulta SQL para insertar en la tabla 'tblDocumentacion'
        //        string query = "INSERT INTO tblDocumentacion (Id_OT, Pedido, Archivo, Observacion, TipoDocumento, Usuario, FechaRegistro) VALUES (@Id_OT, @Pedido, @Archivo, @Observacion, @TipoDocumento, @Usuario, @FechaRegistro)";

        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            // Establecer los parámetros de la consulta
        //            cmd.Parameters.AddWithValue("@Id_OT", folderName);
        //            cmd.Parameters.AddWithValue("@Pedido", 0);
        //            cmd.Parameters.AddWithValue("@Archivo", fileName);
        //            cmd.Parameters.AddWithValue("@Observacion", Textarea1.Value); // Obtener el valor del textarea
        //            cmd.Parameters.AddWithValue("@TipoDocumento", "BITACORA");
        //            cmd.Parameters.AddWithValue("@Usuario", DropDownList1.SelectedValue); // Obtener el valor del DropDownList
        //            cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);

        //            // Ejecutar la consulta
        //            int rowsAffected = cmd.ExecuteNonQuery();

        //            // Comprobar si se actualizó al menos una fila
        //            if (rowsAffected > 0)
        //            {


        //            }
        //            else
        //            {


        //            }
        //        }
        //    }


        //}

        //protected void btnUploadd_Click(object sender, EventArgs e)
        //{
        //    Documentacion.Visible = !Documentacion.Visible;
        //    miDiv.Visible = !miDiv.Visible;

        //    // Obtener el valor del Label
        //    string numDise = "DS" + lblNumDise.Text;

        //    // Construir la consulta SQL con el valor del Label en el WHERE
        //    string consultaSql = $"SELECT Archivo, Observacion, Usuario, FechaRegistro, Id_OT FROM tblDocumentacion WHERE Id_OT = '{numDise}'";

        //    // Actualizar el comando SQL del SqlDataSource con la nueva consulta
        //    SqlDataSource3.SelectCommand = consultaSql;

        //    // Actualizar el DataGrid
        //    DataGridDocumento.DataBind();


        //}

        ////SELECCIONA
        //protected void lnkSelectRow_Click(object sender, EventArgs e)
        //{


        //    // Obtén el LinkButton que se hizo clic
        //    LinkButton lnkSelectRow = (LinkButton)sender;

        //    // Obtén el índice de fila desde el CommandArgument
        //    int rowIndex = Convert.ToInt32(lnkSelectRow.CommandArgument);

        //    // Accede a la fila seleccionada en el DataGrid
        //    DataGridItem selectedRow = DataGridDocumento.Items[rowIndex];

        //    // Almacena el valor de Id_OT en una variable de sesión
        //    Session["SelectedIdOT"] = selectedRow.Cells[5].Text;

        //    // Almacena el nombre del archivo en la variable de sesión
        //    Session["SelectedFileName"] = selectedRow.Cells[1].Text;

        //    // Deselecciona todas las filas previamente seleccionadas
        //    foreach (DataGridItem item in DataGridDocumento.Items)
        //    {
        //        if (item != selectedRow)
        //        {
        //            item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
        //        }
        //    }

        //    // Aplica la clase CSS a la fila seleccionada
        //    selectedRow.CssClass = "selected-row";

        //    // Puedes acceder a los datos de la fila si es necesario
        //    string archivo = selectedRow.Cells[1].Text;
        //    string observacion = selectedRow.Cells[2].Text;
        //    string usuario = selectedRow.Cells[3].Text;
        //    string fechaRegistro = selectedRow.Cells[4].Text;
        //    string id_OT = selectedRow.Cells[5].Text;

        //}

        ////VER
        //protected void lnkViewFile_Click(object sender, EventArgs e)
        //{
        //    LinkButton lnkViewFile = (LinkButton)sender;
        //    int rowIndex = Convert.ToInt32(lnkViewFile.CommandArgument);
        //    DataGridItem selectedRow = DataGridDocumento.Items[rowIndex];
        //    string archivo = selectedRow.Cells[1].Text;

        //    // Construye la ruta completa al archivo
        //    string rutaArchivo = @"P:\SISTEMAS\PruebaDocumentacion\" + archivo;


        //    try
        //   {
        //        System.Diagnostics.Process.Start(rutaArchivo);
        //   }
        //    catch (Exception ex)
        //   {
        //        // Maneja cualquier excepción que pueda ocurrir al abrir el archivo
        //       // Puedes registrar el error o mostrar un mensaje al usuario si es necesario
        //    }
        //}

        ////ELIMINAR
        //protected void LinkButton3_Click(object sender, EventArgs e)
        //{
        //    // Recupera el valor de Id_OT de la variable de sesión
        //    string idOTToDelete = Session["SelectedIdOT"] as string;

        //    if (!string.IsNullOrEmpty(idOTToDelete))
        //    {

        //        // Recupera el valor de "Archivo" de la variable de sesión
        //        string archivoToDelete = Session["SelectedFileName"] as string;

        //        // Conexión a la base de datos y consulta DELETE
        //        string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            string deleteQuery = "DELETE FROM tbldocumentacion WHERE Id_OT = @IdOT AND Archivo = @Archivo";
        //            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
        //            {
        //                command.Parameters.AddWithValue("@IdOT", idOTToDelete);
        //                command.Parameters.AddWithValue("@Archivo", archivoToDelete);
        //                command.ExecuteNonQuery();
        //            }
        //        }


        //        DataGridDocumento.DataBind();
        //    }
        //}


        //protected void SelectRow_Click(object sender, EventArgs e)
        //{       
        //}

        //protected void Unnamed_Click(object sender, EventArgs e)
        //{
        //    lblNumDise.Text = "Por definir";
        //}

        //protected void Button3_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalDoc').modal('show');", true);
        //}

        //protected void Unnamed_Click1(object sender, EventArgs e)
        //{
        //    if (FileUpload1.HasFile)
        //    {
        //        string fileName = FileUpload1.FileName;

        //        // Obtener el texto de lblNumDise
        //        string lblText = lblNumDise.Text;

        //        // Concatenar "DS" con el texto de lblNumDise para obtener el nombre de la carpeta
        //        string folderName = "DS" + lblText;

        //        // Combina la ruta de guardado con el nombre de la carpeta
        //        string savePath = Path.Combine(@"P:\SISTEMAS\PruebaDocumentacion", folderName);

        //        // Verifica si la carpeta no existe y créala si es necesario
        //        if (!Directory.Exists(savePath))
        //        {
        //            Directory.CreateDirectory(savePath);
        //        }

        //        string filePath = Path.Combine(savePath, fileName);

        //        FileUpload1.SaveAs(filePath);

        //        InsertarEnBaseDeDatos(folderName, fileName);

        //        // Actualiza el TextBox con el nombre del archivo seleccionado
        //        TextBox2.Text = fileName;

        //        // Muestra un mensaje de éxito o realiza otras acciones necesarias
        //        lblMessage.Text = "Archivo subido exitosamente.";

        //        // Actualiza solo el contenido del UpdatePanel3
        //        UpdatePanel3.Update();

        //    }
        //    else
        //    {
        //        lblMessage.Text = "Por favor, selecciona un archivo para subir.";
        //    }
        //}

        //protected void Unnamed_Click2(object sender, EventArgs e)
        //{

        //}
    }


}