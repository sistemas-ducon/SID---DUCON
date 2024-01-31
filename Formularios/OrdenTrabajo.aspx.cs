using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.TextFormatting;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using DataTable = System.Data.DataTable;
using ListItem = System.Web.UI.WebControls.ListItem;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using static SISTEMA_INTEGRAL_DUCON.Formularios.OrdenTrabajo;
using System.Web.UI.WebControls.WebParts;
using DocumentFormat.OpenXml.Bibliography;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class OrdenTrabajo : System.Web.UI.Page
    {
        private string id;
        private string pedido;
        private List<TextBox> listaTextBoxes;
        private List<DropDownList> listaDropDownLists;

        private List<int> ID_Acabados = new List<int>();
        private List<int> ID_GruposObjetoparaAcabados = new List<int>();
        private List<string> Detalles_Adicionales = new List<string>();
        private List<string> AcabadosVentas = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

                if (!IsPostBack)
                {

                    tbVenta.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    dtpFechaEntregaDibujoDespiece.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    dtpFechaEntregaProduccion.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    DateTime fechaActual = DateTime.Now;
                    DateTime fechaMas10Dias = fechaActual.AddDays(10);
                    dtpEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");
                    dtpRealEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");

                    Session["CargarOTsEjecutada"] = null;

                    habilitarbotones();

                    DeshabilitarBotones(sender, e);

                    BotonesPorDefectoPlano(sender, e);

                    BotonesPorDefectoObjetos(sender, e);

                    BotonesPorDefectoModulos(sender, e);

                    BotonesPorDefectoInsumos(sender, e);

                    listaTextBoxes = new List<TextBox>
                         {
                    tbObra,tbDir,tbContac,tbEmail,tbRecibe,tbTel,tbCel,tbPais,tbHTotal,tbVenta,dtpFechaEntregaDibujoDespiece,dtpFechaEntregaProduccion,dtpEmpaque,dtpRealEmpaque,tbSupervisor,
                    tbBolsa,tbValorPedido,txtNit,txtNombreEmp,txtcontacto,txtMail,txtDireccion,txtMunicipio,txtTelefono,txtCotizacion,txtValorSugerido,txtVcsd,txtVccd,txtOrdenCompra,txtAsesor,txtComision,
                    txtDiseño,txtSaldo,txtVenta,txtDcto,txtDctoValor,txtVtte,txtVvia,txtGtotal,txtPlano,txtCliente,txtArea,txtContactoPlano,txtAsesorPlano,txtDibuja,txtBolsa

                        };

                    listaDropDownLists = new List<DropDownList>
                         {
                   ddlNumbers,ddlZona,dtacboTipoPedido,cboPedidoBase,DtaCboTipoAprobacion,ddlFabrica1,ddlInstala,ddlAsesor,ddlCiudad

                        };

                    txObs2.Disabled = true;
                    txObs1.Disabled = true;
                    txResumen.Disabled = true;

                    ValorPorDefectoTexArea();
                    CargarAsesoresEnDropDownList();
                    DeshabilitarTextBoxes(listaTextBoxes);
                    DeshabilitarDropDownLists(listaDropDownLists);
                    Nit.Enabled = false;
                    Nit.CssClass = "bi bf  btn btn-outline-secondary";
                    btnCotizacion.Enabled = false;
                    btnCotizacion.CssClass = "bi bf  btn btn-outline-secondary";
                    tbPedDepen.DataBind();
                    tbPedDepen.Items.Insert(0, new ListItem(" "));
                    cboPedidoBase.DataBind();
                    cboPedidoBase.Items.Insert(0, new ListItem(" "));
                    DtaCboTipoAprobacion.DataBind();
                    DtaCboTipoAprobacion.Items.Insert(0, new ListItem(" "));
                    dtacboTipoPedido.DataBind();
                    dtacboTipoPedido.Items.Insert(0, new ListItem(" "));
                    DepartamentoAsesor(); // Se Deberia cargar desde el login 

                    // Validacion para Cargar el Plano  Con variables de Session

                    if (Session["Id_OT2"] != null && Session["pedido2"] != null)
                    {

                        // Este bloque carga solo el plano ya que el Id_OT2 es igual al texto Nula
                        if (Session["Id_OT2"].ToString() == "Nula")
                        {
                            if (Session["Id_Plano"] != null)
                            {
                                Cargar_Plano2(Session["Id_Plano"].ToString());
                                Session.Remove("Id_Plano");
                                Session.Remove("Id_OT2");
                                Session.Remove("pedido2");
                                HabilitarBotonesPlano();
                            }

                        }
                        // Este bloque consulta la OT con variables de Session de afuera del formulario 
                        else if (Session["Id_OT2"] != null && Session["pedido2"] != null)
                        {


                            Cargar_OTs2();
                            List<int> numeros = ObtenerNumerosDesdeLaBaseDeDatos(Session["Id_OT2"].ToString());

                            ddlNumbers.Items.Clear(); // Limpiar las opciones existentes

                            foreach (int numero in numeros)
                            {
                                ddlNumbers.Items.Add(numero.ToString());

                            }

                            ddlNumbers.SelectedValue = Session["pedido2"].ToString();
                            Session.Remove("Id_OT2");
                            Session.Remove("pedido2");
                        }

                    }


                    CargarVariablesDeSesionContable();
                }


            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }

        private void ValorPorDefectoTexArea()
        {
            txObs1.Value = "Tipo de Sujeción: \n\n" +
                                 "Perfil Refuerzo Superior: \n\n" +
                                 "Tipo y Color de Sillas: \n\n" +
                                 "Observaciones Generales: \n\n" +
                                 "\nObservación para Producción: \n\n" +
                                 "\nObservación para Compras: \n\n" +
                                 "\nObservación para Despacho: ";
            txObs2.Value = "Tipo de Sujeción: \n\n" +
                                 "Perfil Refuerzo Superior: \n\n" +
                                 "Tipo y Color de Sillas: \n\n" +
                                 "Observaciones Generales: \n\n" +
                                 "\nObservación para Producción: \n\n" +
                                 "\nObservación para Compras: \n\n" +
                                 "\nObservación para Despacho: ";
        }

        //MODIFICADO POR CARLOS PINEDA
        protected void Cancelar_Click(object sender, EventArgs e)
        {

            if (Session["CargarOTsEjecutada"] != null && (bool)Session["CargarOTsEjecutada"])
            {
                tbOT.Text = "Por Asig";

            }
            else
            {
                // Coloca aquí el código que deseas ejecutar si Cargar_OTs no se ha ejecutado
                GrabarOt.Enabled = false;
                GrabarOt.CssClass = "btn btn-sm shadow button-disabled";
                Cancelar.Enabled = false;
                Cancelar.CssClass = "btn btn-sm shadow button-disabled";
                NuevaOt.Enabled = true;
                NuevaOt.CssClass = "btn btn-sm shadow button-enabled";
                ObservacionesOt.Enabled = true;
                ObservacionesOt.CssClass = "btn btn-sm shadow button-enabled";
                OtPendientes.Enabled = true;
                OtPendientes.CssClass = "btn btn-sm shadow button-enabled";

                tbOT.Text = string.Empty;

                listaTextBoxes = new List<TextBox>
                {
                    tbObra,tbDir,tbContac,tbEmail,tbRecibe,tbTel,tbCel,tbPais,tbHTotal,tbVenta,dtpFechaEntregaDibujoDespiece,dtpFechaEntregaProduccion,dtpEmpaque,dtpRealEmpaque,tbSupervisor,
                    tbBolsa,tbValorPedido,txtNit,txtNombreEmp,txtcontacto,txtMail,txtDireccion,txtMunicipio,txtTelefono,txtCotizacion,txtValorSugerido,txtVcsd,txtVccd,txtOrdenCompra,txtAsesor,txtComision,
                    txtDiseño,txtSaldo,txtVenta,txtDcto,txtDctoValor,txtVtte,txtVvia,txtGtotal

                };

                DeshabilitarTextBoxes(listaTextBoxes);

                listaDropDownLists = new List<DropDownList>
                {
                   ddlNumbers,ddlZona,dtacboTipoPedido,cboPedidoBase,DtaCboTipoAprobacion,ddlFabrica1,ddlInstala,ddlAsesor,ddlCiudad

                };

                DeshabilitarDropDownLists(listaDropDownLists);

                tbOT.Enabled = true;
                tbOT.CssClass = "form-control";

                txObs1.Disabled = true;
                txObs2.Disabled = true;

                ObservacionCont.Disabled = true;
                TextTNegociacion.Disabled = true;

                Nit.Enabled = false;
                Nit.CssClass = "btn btn-sm shadow button-disabled";

            }

            // Limpia la variable de sesión después de su uso
            Session["CargarOTsEjecutada"] = null;

            if (tbOT.Text == "Por Asig")
            {
                NuevaOt.Enabled = true;
                NuevaOt.CssClass = "btn btn-sm shadow button-enabled";

                CopiarOt.Enabled = true;
                CopiarOt.CssClass = "btn btn-sm shadow button-enabled";

                ModificarOt.Enabled = true;
                ModificarOt.CssClass = "btn btn-sm shadow button-enabled";

                AnularPedido.Enabled = true;
                AnularPedido.CssClass = "btn btn-sm shadow button-enabled";

                DocumentacionOt.Enabled = true;
                DocumentacionOt.CssClass = "btn btn-sm shadow button-enabled";

                ObservacionesOt.Enabled = true;
                ObservacionesOt.CssClass = "btn btn-sm shadow button-enabled";

                imprimirOt.Enabled = true;
                imprimirOt.CssClass = "btn btn-sm shadow button-enabled";

                ConsultarBolsa.Enabled = true;
                ConsultarBolsa.CssClass = "btn btn-sm shadow button-enabled";

                OtPendientes.Enabled = true;
                OtPendientes.CssClass = "btn btn-sm shadow button-enabled";

                GrabarOt.Enabled = false;
                GrabarOt.CssClass = "btn btn-sm shadow button-disabled";

                ReimprimirOt.Enabled = false;
                ReimprimirOt.CssClass = "btn btn-sm shadow button-disabled";

                Cancelar.Enabled = false;
                Cancelar.CssClass = "btn btn-sm shadow button-disabled";

                listaTextBoxes = new List<TextBox>
                {
                    tbObra,tbDir,tbContac,tbEmail,tbRecibe,tbTel,tbCel,tbPais,tbHTotal,tbVenta,dtpFechaEntregaDibujoDespiece,dtpFechaEntregaProduccion,dtpEmpaque,dtpRealEmpaque,tbSupervisor,
                    tbBolsa,tbValorPedido,txtNit,txtNombreEmp,txtcontacto,txtMail,txtDireccion,txtMunicipio,txtTelefono,txtCotizacion,txtValorSugerido,txtVcsd,txtVccd,txtOrdenCompra,txtAsesor,txtComision,
                    txtDiseño,txtSaldo,txtVenta,txtDcto,txtDctoValor,txtVtte,txtVvia,txtGtotal

                };

                tbOT.Enabled = true;
                tbOT.CssClass = "form-control";

                tbPedDepen.Enabled = true;
                tbPedDepen.CssClass = "form-control";

                DeshabilitarTextBoxes(listaTextBoxes);

                listaDropDownLists = new List<DropDownList>
                {
                   ddlNumbers,ddlZona,dtacboTipoPedido,cboPedidoBase,DtaCboTipoAprobacion,ddlFabrica1,ddlInstala,ddlAsesor,ddlCiudad

                };

                DeshabilitarDropDownLists(listaDropDownLists);

                Cargar_OTs();

                Session["CargarOTsEjecutada"] = true;

                txObs1.Disabled = true;
                txObs2.Disabled = true;

                ObservacionCont.Disabled = true;
                TextTNegociacion.Disabled = true;

                Nit.Enabled = false;
                Nit.CssClass = "btn btn-sm shadow button-disabled";
            }




            Session.Remove("BtnModificarEjecutado");
            Session.Remove("NuevaOTEjecutada");


        }

        protected void NuevaOT_Click(object sender, EventArgs e)
        {

            // Verificar si Cargar_OTs se ha ejecutado
            if (Session["CargarOTsEjecutada"] != null && (bool)Session["CargarOTsEjecutada"])
            {
                tbOT.Text = "Por Asig";

            }
            else
            {
                NuevaOt.Enabled = false;
                NuevaOt.CssClass = "btn btn-sm shadow button-disabled";
                ObservacionesOt.Enabled = false;
                ObservacionesOt.CssClass = "btn btn-sm shadow button-disabled";
                OtPendientes.Enabled = false;
                OtPendientes.CssClass = "btn btn-sm shadow button-disabled";
                GrabarOt.Enabled = true;
                GrabarOt.CssClass = "btn btn-sm shadow button-enabled";
                Cancelar.Enabled = true;
                Cancelar.CssClass = "btn btn-sm shadow button-enabled";

                tbOT.Text = "Por Asig.";
            }
            if (tbOT.Text == "Por Asig")
            {
                NuevaOt.Enabled = false;
                NuevaOt.CssClass = "btn btn-sm shadow button-disabled";
                CopiarOt.Enabled = false;
                CopiarOt.CssClass = "btn btn-sm shadow button-disabled";
                OtPendientes.Enabled = false;
                OtPendientes.CssClass = "btn btn-sm shadow button-disabled";
                DocumentacionOt.Enabled = false;
                DocumentacionOt.CssClass = "btn btn-sm shadow button-disabled";
                ObservacionesOt.Enabled = false;
                ObservacionesOt.CssClass = "btn btn-sm shadow button-disabled";
                imprimirOt.Enabled = false;
                imprimirOt.CssClass = "btn btn-sm shadow button-disabled";
                ReimprimirOt.Enabled = false;
                ReimprimirOt.CssClass = "btn btn-sm shadow button-disabled";
                ConsultarBolsa.Enabled = false;
                ConsultarBolsa.CssClass = "btn btn-sm shadow button-disabled";
                ObraReactivada.Enabled = false;
                ObraReactivada.CssClass = "btn btn-sm shadow button-disabled";
                ExportarPedido.Enabled = false;
                ExportarPedido.CssClass = "btn btn-sm shadow button-disabled";
                GrabarOt.Enabled = true;
                GrabarOt.CssClass = "btn btn-sm shadow button-enabled";
                Cancelar.Enabled = true;
                Cancelar.CssClass = "btn btn-sm shadow button-enabled";
                ObraReactivada.Enabled = true;
                ObraReactivada.CssClass = "btn btn-sm shadow button-enabled";
                ModificarOt.Enabled = false;
                ModificarOt.CssClass = "btn btn-sm shadow button-disabled";
                AnularPedido.Enabled = false;
                AnularPedido.CssClass = "btn btn-sm shadow button-disabled";

                LabelOTCerrada.Visible = false;

                List<string> elementIds = new List<string>
{
                "LabelOTCerrada", "LiteralFechaCierre",
                "dtacboTipoPedido", "DtaCboTipoAprobacion", "tbObra", "tbDir",
                "tbContac", "tbEmail", "tbRecibe", "ddlCiudad", "tbTel", "tbCel", "tbPais",
                "txObs1", "txObs2", "tbVenta", "dtpFechaEntregaDibujoDespiece",
                "dtpFechaEntregaProduccion", "dtpEmpaque", "dtpRealEmpaque", "tbSupervisor",
                "ddlFabrica1", "ddlInstala", "ObservacionCont", "txtCotizacion", "txtOrdenCompra",
                "txtAsesor", "TextTNegociacion", "tbBolsa", "ddlAsesor", "txtDcto", "txtVtte",
                "txtVvia", "txtVenta", "tbValorPedido","txtNit","txtNombreEmp","txtcontacto",
                "txtMail","txtDireccion","txtMunicipio","txtTelefono","txtValorSugerido","txtVcsd",
                "txtVccd","txtDiseño","txtComision","txtSaldo","txtVenta","txtDcto","txtDctoValor",
                "txtVtte","txtVvia","txtGtotal"
                };

                foreach (string elementId in elementIds)
                {
                    var element = Page.FindControl(elementId);

                    if (element is TextBox)
                    {
                        TextBox textBox = (TextBox)element;
                        textBox.Text = string.Empty; // Limpia el contenido del TextBox
                    }
                    else if (element is DropDownList)
                    {
                        DropDownList dropDownList = (DropDownList)element;
                        dropDownList.ClearSelection(); // Limpia la selección del DropDownList
                    }
                    else if (element is Label)
                    {
                        Label label = (Label)element;
                        label.Text = string.Empty; // Limpia el texto del Label
                    }

                }

                LimpiarTextAreayDropDownList();

                HabilitarTodosLosTextBoxes();

                btnOk.Enabled = false;
                btnOk.CssClass = "btn btn-sm shadow button-disabled fw-bold";

                btnAcabados.Enabled = false;
                btnAcabados.CssClass = "btn btn-sm shadow button-disabled fw-bold";

                btnNuevoPedido.Enabled = false;
                btnNuevoPedido.CssClass = "btn btn-sm shadow button-disabled fw-bold";

                tbOT.Enabled = false;
                tbOT.CssClass = "form-control";

                ddlNumbers.Enabled = false;
                ddlNumbers.CssClass = "form-control";

                cboPedidoBase.Enabled = false;

                tbPedDepen.Enabled = false;

                dtacboTipoPedido.Enabled = true;

                ddlAsesor.Enabled = true;
                ddlAsesor.CssClass = "form-control";

                ddlCiudad.Enabled = true;
                ddlCiudad.CssClass = "form-control";

                ddlInstala.Enabled = true;
                ddlInstala.CssClass = "form-control";

                tbVenta.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dtpFechaEntregaDibujoDespiece.Text = DateTime.Now.ToString("yyyy-MM-dd");
                dtpFechaEntregaProduccion.Text = DateTime.Now.ToString("yyyy-MM-dd");
                DateTime fechaActual = DateTime.Now;
                DateTime fechaMas10Dias = fechaActual.AddDays(10);
                dtpEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");
                dtpRealEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");

                tbPedDepen.DataBind();
                tbPedDepen.Items.Insert(0, new ListItem(" "));
                cboPedidoBase.DataBind();
                cboPedidoBase.Items.Insert(0, new ListItem(" "));
                dtacboTipoPedido.DataBind();
                dtacboTipoPedido.Items.Insert(0, new ListItem(" "));


            }


            if (tbOT.Text == "Por Asig.")
            {


                HabilitarTodosLosTextBoxes();


                string zonaLogeada = Session["ZonaLogeada"] as string; // Obtén el valor de la variable de sesión

                // Establece el valor seleccionado en el DropDownList ddlZona
                ddlZona.SelectedValue = zonaLogeada;


            }

            Nit.Enabled = true;
            Nit.CssClass = "btn btn-sm shadow button-enabled";

            Session["NuevaOTEjecutada"] = true;
        }

        protected void LimpiarTextAreayDropDownList()
        {
            tbPedDepen.ClearSelection();
            tbPedDepen.Items.Clear();

            cboPedidoBase.ClearSelection();
            cboPedidoBase.Items.Clear();

            dtacboTipoPedido.ClearSelection();
            dtacboTipoPedido.Items.Clear();

            txObs1.InnerText = string.Empty;

            txObs2.InnerText = string.Empty;

            ObservacionCont.InnerText = string.Empty;

            TextTNegociacion.InnerText = string.Empty;
        }

        protected void ImprimirOt_Click(object sender, EventArgs e)
        {

        }

        protected void BtnObservaciones_Click(object sender, EventArgs e)
        {
            // Verifica si el LinkButton está habilitado
            if (EstaHabilitado())
            {
                string url = "FormExtPrin/ObservacionesOT.aspx";
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
            }
        }

        private bool EstaHabilitado()
        {
            // Agrega tu lógica para determinar si el LinkButton está habilitado o no.
            // Devuelve true si está habilitado y false si no lo está.
            // Ejemplo: Siempre habilitado
            return true;
        }

        protected void habilitarbotones()
        {
            NuevaOt.Enabled = true;
            NuevaOt.CssClass = "btn btn-sm shadow button-enabled";

            ObservacionesOt.Enabled = true;
            ObservacionesOt.CssClass = "btn btn-sm shadow button-enabled";

            OtPendientes.Enabled = true;
            OtPendientes.CssClass = "btn btn-sm shadow button-enabled";

        }

        protected void DeshabilitarBotones(object sender, EventArgs e)
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {

                CopiarOt,
                GrabarOt,
                ModificarOt,
                AnularPedido,
                DocumentacionOt,
                imprimirOt,
                ReimprimirOt,
                ConsultarBolsa,
                Cancelar,
                ActPedImp,
                ImpPedAse,
                ImpPedSed,
                HabilitarPedido,
                DeshabilitarOt,
                ObraReactivada,
                RegPedSisAdm,
                CierraOt,
                SimularPedido,
                ExportarPedido,
                EntregaPerfecta,
                AnularObra,
                btnNuevoPedido,
                btnAcabados,
                btnOk
            };

            string cssClass = "btn btn-sm shadow button-disabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;
                    linkButton.Enabled = false;
                    linkButton.CssClass = cssClass;
                }
            }

        }

        protected void OtPendientes_Click(object sender, EventArgs e)
        {
            // Verifica si el LinkButton está habilitado
            if (EstaHabilitado2())
            {
                string url = "FormExtPrin/OTsPendientes.aspx";
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

            }

        }

        private bool EstaHabilitado2()
        {
            // Agrega tu lógica para determinar si el LinkButton está habilitado o no.
            // Devuelve true si está habilitado y false si no lo está.
            // Ejemplo: Siempre habilitado
            return true;
        }

        //FIN

        public class DatosFiltrados
        {
            public string ID { get; set; }
            public string Descripcion { get; set; }
            public string Altura { get; set; }
            public string Ancho { get; set; }
            public string Cantidad { get; set; }
            public string ValorUnd { get; set; }
            public string SubTotal { get; set; }
            public string Id_Panel { get; set; }
            public string Tipo { get; internal set; }
            public string Titulo { get; internal set; }
            public bool RevisadoDibujo { get; set; }
            public string peso { get; set; }
            public string profundidad { get; set; }
            public string ajusteCub { get; set; }

          

        }

        protected void ddlCiudad_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlCiudad.Items.Insert(0, new ListItem(" ", ""));
        }

        protected void ddlGrupoObjeto_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlGrupo.Items.Insert(0, new ListItem("Seleccione", ""));

        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT *, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial WHERE Activo = 1   order by Apellidos";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto"; // Campos que se mostrará en el DropDownLi
                ddlAsesor.DataValueField = "Cedula";

                ddlAsesor.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem(" ", "0"));
        }

        protected void ddlAsesor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAsesor.SelectedValue != "0")
            {
                txtAsesor.Text = ddlAsesor.SelectedValue; // Asigna el valor de Cedula al TextBox
                txtAsesor.Enabled = false;
                txtAsesor.CssClass = "form-control";
            }
            else
            {
                txtAsesor.Text = string.Empty; // Si se selecciona el elemento inicial, se borra el TextBox
            }
        }

        public void DeshabilitarTextBoxes(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox == txtValorSugerido || textBox == txtVcsd || textBox == txtVccd || textBox == txtComision || textBox == txtDiseño || textBox == txtSaldo || textBox == txtVenta || textBox == txtDctoValor ||
                   textBox == txtVtte || textBox == txtVvia || textBox == txtGtotal || textBox == tbBolsa || textBox == tbValorPedido)
                {
                    if (textBox == tbBolsa || textBox == tbValorPedido || textBox == txtValorSugerido || textBox == txtVcsd || textBox == txtVccd || textBox == txtGtotal ||
                       textBox == txtComision || textBox == txtDiseño || textBox == txtSaldo || textBox == txtVenta || textBox == txtDctoValor || textBox == txtDcto ||
                       textBox == txtVtte || textBox == txtVvia)
                    {
                        textBox.Enabled = false;
                        textBox.CssClass = "form-control text-end fw-bold";
                    }
                    else
                    {
                        textBox.Enabled = false;
                        textBox.CssClass = "form-control text-end ";
                    }




                }
                else
                {
                    textBox.Enabled = false;
                    textBox.CssClass = "form-control ";
                }

            }

            dtacboTipoPedido.Enabled = false;
            tbPedDepen.Enabled = false;

            ObservacionCont.Disabled = true;
            TextTNegociacion.Disabled = true;
        }

        protected void HabilitarTodosLosTextBoxes()
        {

            tbObra.Enabled = true;
            tbDir.Enabled = true;
            tbContac.Enabled = true;
            tbEmail.Enabled = true;
            tbRecibe.Enabled = true;
            tbTel.Enabled = true;
            tbCel.Enabled = true;
            tbPais.Enabled = true;
            tbHTotal.Enabled = true;

            dtpEmpaque.Enabled = true;

            ddlAsesor.Enabled = true;
            ddlAsesor.CssClass = "form-control";

            tbOT.Enabled = false;
            tbOT.CssClass = "form-control";

            ddlNumbers.Enabled = false;
            ddlNumbers.CssClass = "form-control";

            txtCotizacion.Enabled = true;

            txtOrdenCompra.Enabled = true;
            txtAsesor.Enabled = true;


            dtacboTipoPedido.Enabled = true;
            DtaCboTipoAprobacion.Enabled = true;

            ddlCiudad.Enabled = true;

            ddlFabrica1.Enabled = true;
            ddlInstala.Enabled = true;


            // Asignar estilos de CSS si es necesario
            ddlFabrica1.CssClass = "form-control";
            ddlInstala.CssClass = "form-control";
            ddlCiudad.CssClass = "form-control";
            DtaCboTipoAprobacion.CssClass = "form-control";
            dtacboTipoPedido.CssClass = "form-control";
            tbObra.CssClass = "form-control";
            tbDir.CssClass = "form-control";
            tbContac.CssClass = "form-control";
            tbEmail.CssClass = "form-control";
            tbRecibe.CssClass = "form-control";
            tbTel.CssClass = "form-control";
            tbCel.CssClass = "form-control";
            tbPais.CssClass = "form-control";
            tbHTotal.CssClass = "form-control";
            tbVenta.CssClass = "form-control";
            dtpFechaEntregaDibujoDespiece.CssClass = "form-control";
            dtpFechaEntregaProduccion.CssClass = "form-control";
            dtpEmpaque.CssClass = "form-control";
            dtpRealEmpaque.CssClass = "form-control";
            tbSupervisor.CssClass = "form-control";
            tbBolsa.CssClass = "form-control";
            tbValorPedido.CssClass = "form-control";
            txtNit.CssClass = "form-control";
            txtNombreEmp.CssClass = "form-control";

            txtCotizacion.CssClass = "form-control";

            txtOrdenCompra.CssClass = "form-control";
            txtAsesor.CssClass = "form-control";

            txObs1.Disabled = false;
            txObs2.Disabled = false;

            ObservacionCont.Disabled = false;
            TextTNegociacion.Disabled = false;

        }

        public void DeshabilitarDropDownLists(List<DropDownList> dropDownLists)
        {
            foreach (DropDownList dropDownList in dropDownLists)
            {
                dropDownList.Enabled = false;
                dropDownList.CssClass = "form-control";
            }
        }

        protected void ObtenerInfoOt(object sender, EventArgs e)
        {
            string id = tbOT.Text.Trim();
            Session["Id_OT"] = id;
            Session["pedido"] = 1;

            Cargar_OTs();
            if (!string.IsNullOrEmpty(id))
            {

                string inputData = tbOT.Text;
                List<int> numeros = ObtenerNumerosDesdeLaBaseDeDatos(inputData);

                ddlNumbers.Items.Clear(); // Limpiar las opciones existentes

                foreach (int numero in numeros)
                {
                    ddlNumbers.Items.Add(numero.ToString());
                }

            }
        }

        private List<int> ObtenerNumerosDesdeLaBaseDeDatos(string dato)
        {
            List<int> numeros = new List<int>();

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblOT WHERE Id_OT = @IdOT";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdOT", dato);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Supongamos que el número que deseas obtener está en la columna "Consecutivo_Pedido" de la tabla
                            int numero = reader.GetInt16(reader.GetOrdinal("Consecutivo_Pedido"));
                            numeros.Add(numero);
                        }
                    }
                }
            }

            return numeros;
        }

        protected void CambioDePediido(object sender, EventArgs e)

        {
            string pedido = ddlNumbers.SelectedValue;
            Session["pedido"] = pedido;

            Cargar_OTs();

        }

        //MODIFICADO POR CARLOS PINEDA

        public void Cargar_OTs()
        {
            id = Session["Id_OT"]?.ToString();
            pedido = Session["pedido"]?.ToString();


            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString))
            {
                sqlconectar.Open();

                if (TryExecuteOTQuery(id, pedido, sqlconectar, out SqlDataReader leer))
                {
                    if (leer.Read())
                    {

                        if (ValidarPermisoInfoContable(leer))
                        {
                            string IDCLienteConstacto = leer["IDContacto_Cliente"].ToString();
                            CargarDatosContables(IDCLienteConstacto);

                        }


                        // Extraer datos y asignarlos a controles
                        AssignDataToControls(leer);

                        EnableButtons();

                        HabilitarBotonesPlano();
                        // Obtener datos de cotización y asignarlos a controles
                        AssignCotizacionData(id, pedido, txtCotizacion.Text);
                      

                    }
                }
            }



            Cargar_Plano(id, pedido);
            Cargar_Despiece_Plano();


            ddlNumbers.Enabled = true;

            tbPedDepen.Enabled = true;
            tbPedDepen.CssClass = "form-control";

            string valorTextBox = tbObra.Text.Trim();

            // Guardar el valor en una variable de sesión
            Session["ValorDeObra"] = valorTextBox;

            Session["CargarOTsEjecutada"] = true;
        }

        private bool TryExecuteOTQuery(string id, string pedido, SqlConnection connection, out SqlDataReader leer)
        {
            SqlCommand cmd = new SqlCommand("ctaOT", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
            cmd.Parameters.Add("@Con", SqlDbType.VarChar, 30).Value = pedido;

            leer = cmd.ExecuteReader();
            return leer.HasRows;
        }

        private void AssignDataToControls(SqlDataReader leer)
        {
            tbPedDepen.DataBind();

            cboPedidoBase.DataBind();
            cboPedidoBase.Items.Insert(0, new ListItem(" "));

            dtacboTipoPedido.DataBind();
            dtacboTipoPedido.Items.Insert(0, new ListItem(" "));

            bool cerrada = leer.GetBoolean(leer.GetOrdinal("Cerrada")); // Variable para OTCerrada

            if (cerrada)
            {
                DateTime fechaCierre = (DateTime)leer["Fecha_Cierre"];
                LabelOTCerrada.Visible = true;
                LabelOTCerrada.Text = "OT cerrada el día " + fechaCierre.ToString("dd/MM/yyyy");
                LiteralFechaCierre.Text = fechaCierre.ToString("dd/MM/yyyy");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                LabelOTCerrada.Visible = false;
            }
            tbOT.Text = leer["Id_OT"].ToString();
            ddlNumbers.Text = leer["Consecutivo_Pedido"].ToString();
            ddlZona.Text = leer["Zona"].ToString();
            dtacboTipoPedido.Text = leer["Id_TipoPedido"].ToString();
            cboPedidoBase.SelectedValue = leer["PedidoBase"].ToString();
            DtaCboTipoAprobacion.Text = leer["TipoAprobacion"].ToString();
            tbObra.Text = leer["Nombre_Obra"].ToString();
            tbDir.Text = leer["Dirección"].ToString();
            tbContac.Text = leer["Persona_Receptora"].ToString();
            tbEmail.Text = leer["mail_Contacto"].ToString();
            tbRecibe.Text = leer["RecibeElPedido"].ToString();
            string Ciudad = leer["Ciudad"].ToString() + " - " + leer["Región"].ToString();
            foreach (ListItem item in ddlCiudad.Items)
            {
                if (item.Text == Ciudad)
                {
                    ddlCiudad.ClearSelection();
                    item.Selected = true;
                    break;
                }
            }
            tbTel.Text = leer["TelDomicilio"].ToString();
            tbCel.Text = leer["CelularContacto"].ToString();
            tbPais.Text = leer["País"].ToString();
            txObs1.Value = leer["Observacion_Pedido"].ToString();
            txObs2.Value = leer["Observacion_Dibujo"].ToString();
            DateTime Dato = (DateTime)leer["Fecha_Confirmacion_Venta"];
            DateTime Dato2 = (DateTime)leer["Fecha_Entrega_Produccion"];
            DateTime Dato3 = (DateTime)leer["Fecha_Empaque"];
            DateTime Dato4 = (DateTime)leer["Fecha_Real_Empaque"];
            tbVenta.Text = Dato.ToString("yyyy-MM-dd");
            dtpFechaEntregaDibujoDespiece.Text = Dato.ToString("yyyy-MM-dd");
            dtpFechaEntregaProduccion.Text = Dato2.ToString("yyyy-MM-dd");
            dtpEmpaque.Text = Dato3.ToString("yyyy-MM-dd");
            dtpRealEmpaque.Text = Dato4.ToString("yyyy-MM-dd");
            tbSupervisor.Text = leer["Supervisor"].ToString();
            ddlFabrica1.Text = leer["FabricadoPor"].ToString();
            ddlInstala.Text = leer["InstaladaPor"].ToString();
            ObservacionCont.Value = leer["Observaciones_Contables"].ToString();
            txtCotizacion.Text = leer["Cotizacion"].ToString();
            txtOrdenCompra.Text = leer["OrdendeCompra"].ToString();
            txtAsesor.Text = leer["Codigo_Asesor"].ToString();
            TextTNegociacion.Value = leer["Forma_Pago"].ToString();
            tbBolsa.Text = leer["ValorBolsa"].ToString();
            ddlAsesor.SelectedValue = leer["Codigo_Asesor"].ToString();
            txtDcto.Text = leer["Descuento"].ToString();
            txtVtte.Text = leer["ValorTteVia"].ToString();
            txtVvia.Text = leer["ValorViatico"].ToString();
            txtVenta.Text = leer["Precio_Venta"].ToString();
            tbValorPedido.Text = leer["ValorPedido"].ToString();
            tbHTotal.Text = leer["AlturaPT"].ToString();

            btnCotizacion.Enabled = true;

            calcularDescuento();
            calcularGranTotal();

            // variable de para IDCOntactoCliente 
            Session["ID_ContactoBD"] = leer["IDContacto_Cliente"].ToString();

        }

        private void AssignCotizacionData(string id, string pedido, string cotizacion)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cotzita = new SqlCommand("Sp_DatosCotizacionyPlano", connection))
                {
                    cotzita.CommandType = CommandType.StoredProcedure;
                    cotzita.Parameters.AddWithValue("@Id_OT", id);
                    cotzita.Parameters.AddWithValue("@Consecutivo_Pedido", pedido);
                    cotzita.Parameters.AddWithValue("@Cotizacion", cotizacion);


                    SqlDataReader drcot = cotzita.ExecuteReader();

                    if (drcot.Read())

                    {
                        txtValorSugerido.Text = drcot["ValorSugerido"].ToString();
                        txtVcsd.Text = drcot["Valor"].ToString();
                        txtVccd.Text = drcot["VCCD"].ToString();
                        txtComision.Text = drcot["DescuentoComision"].ToString();
                        txtDiseño.Text = drcot["Diseño"].ToString();
                        txtSaldo.Text = drcot["Saldo"].ToString();




                    }
                    else if (cotizacion.ToUpper() == "NO TIENE" || string.IsNullOrEmpty(cotizacion))
                    {
                        txtValorSugerido.Text = "0";
                        txtVcsd.Text = "0";
                        txtVccd.Text = "0";
                        txtComision.Text = "0";
                        txtDiseño.Text = "0";
                        txtSaldo.Text = "0";
                        txtDctoValor.Text = "0";
                        txtGtotal.Text = "0";
                    }

                    drcot.Close();
                }
            }
        }

        private bool EstaCerrada(string id, string pedido)
        {
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString))
            {
                sqlconectar.Open();
                using (SqlCommand cmd = new SqlCommand("select * from tblOT where Id_OT = @Id and Consecutivo_Pedido = @Con and Cerrada = '0'", sqlconectar))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Con", pedido);

                    SqlDataReader leer = cmd.ExecuteReader();

                    return leer.Read();
                }
            }
        }

        private void EnableButtons()
        {




            bool estaAbierta = false;



            // Obtenga los valores de las variables
            string id = Session["Id_OT"]?.ToString();
            string pedido = Session["pedido"]?.ToString();

            bool estaCerrada = EstaCerrada(id, pedido);

            // Realice la consulta
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString))
            {
                sqlconectar.Open();
                using (SqlCommand cmd = new SqlCommand("select * from tblOT where Id_OT = @Id and Consecutivo_Pedido = @Con and Terminado_Ventas = @C", sqlconectar))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Con", pedido);
                    cmd.Parameters.AddWithValue("@C", 0);

                    SqlDataReader leer = cmd.ExecuteReader();

                    if (leer.Read())
                    {
                        estaAbierta = true;
                    }
                }
            }

            // Habilite o deshabilite el botón
            if (estaAbierta)
            {
                btnOk.Enabled = true;
                btnOk.CssClass = "btn btn-sm shadow button-enabled rojo fw-bold";

                ModificarOt.Enabled = true;
                ModificarOt.CssClass = "btn btn-sm shadow button-enabled";

                AnularPedido.Enabled = true;
                AnularPedido.CssClass = "btn btn-sm shadow button-enabled ";

                ReimprimirOt.Enabled = false;
                ReimprimirOt.CssClass = "btn btn-sm shadow button-disabled";
            }
            else
            {
                btnOk.Enabled = false;
                btnOk.CssClass = "btn btn-sm shadow button-disabled fw-bold";

                ModificarOt.Enabled = false;
                ModificarOt.CssClass = "btn btn-sm shadow button-disabled";

                AnularPedido.Enabled = false;
                AnularPedido.CssClass = "btn btn-sm shadow button-disabled ";

                ReimprimirOt.Enabled = true;
                ReimprimirOt.CssClass = "btn btn-sm shadow button-enabled";
            }



            if (estaCerrada) // Habilitar solo si está abierta y no está cerrada
            {
                btnNuevoPedido.Enabled = true;
                btnNuevoPedido.CssClass = "btn btn-sm shadow button-enabled";

                ExportarPedido.Enabled = true;
                ExportarPedido.CssClass = "btn btn-sm shadow button-enabled";

            }
            else
            {
                btnNuevoPedido.Enabled = false;
                btnNuevoPedido.CssClass = "btn btn-sm shadow button-disabled";

                ExportarPedido.Enabled = false;
                ExportarPedido.CssClass = "btn btn-sm shadow button-disabled";
            }


            CopiarOt.Enabled = true;
            CopiarOt.CssClass = "btn btn-sm shadow button-enabled";

            DocumentacionOt.Enabled = true;
            DocumentacionOt.CssClass = "btn btn-sm shadow button-enabled";

            imprimirOt.Enabled = true;
            imprimirOt.CssClass = "btn btn-sm shadow button-enabled";


            ConsultarBolsa.Enabled = true;
            ConsultarBolsa.CssClass = "btn btn-sm shadow button-enabled";

            ObraReactivada.Enabled = true;
            ObraReactivada.CssClass = "btn btn-sm shadow button-enabled";

            btnAcabados.Enabled = true;
            btnAcabados.CssClass = "btn btn-sm shadow button-enabled";
        }

        //FIN MODIFICACION

        private void HabilitarBotonesPlano()
        {

            if (Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                BtnAdiObjPla.Enabled = true;
                BtnAdiObjPla.CssClass = "btn btn-sm shadow button-enabled";

                BtnEliObjPla.Enabled = true;
                BtnEliObjPla.CssClass = "btn btn-sm shadow button-enabled";

                BtnAcaPla.Enabled = true;
                BtnAcaPla.CssClass = "btn btn-sm shadow button-enabled ";

                BtnLeeArcDesAca.Enabled = true;
                BtnLeeArcDesAca.CssClass = "btn btn-sm shadow";


                BtnCarArcTxtXy.Enabled = true;
                BtnCarArcTxtXy.CssClass = "btn btn-sm shadow button-enabled";

                BtnPlaBlo.Enabled = true;
                BtnPlaBlo.CssClass = "btn btn-sm shadow button-enabled";


                BtnCreRefBol.Enabled = true;
                BtnCreRefBol.CssClass = "btn btn-sm shadow button-enabled";

                BtnAdiRemEleBol.Enabled = true;
                BtnAdiRemEleBol.CssClass = "btn btn-sm shadow button-enabled";



                BtnDesPla.Enabled = true;
                BtnDesPla.CssClass = "btn btn-sm shadow button-enabled";

                BtnGenTxt.Enabled = true;
                BtnGenTxt.CssClass = "btn btn-sm shadow button-enabled";

                BtnGuaTxt.Enabled = true;
                BtnGuaTxt.CssClass = "btn btn-sm shadow button-enabled";


                BtnExpPlaOrdTra.Enabled = true;
                BtnExpPlaOrdTra.CssClass = "btn btn-sm shadow button-enabled";

                BtnVisGenCot.Enabled = true;
                BtnVisGenCot.CssClass = "btn btn-sm shadow button-enabled";

                BtnObjNoExi.Enabled = true;
                BtnObjNoExi.CssClass = "btn btn-sm shadow button-enabled";

                BtnActPrePro.Enabled = true;
                BtnActPrePro.CssClass = "btn btn-sm shadow button-enabled";

                BtnGenForCerOrd.Enabled = true;
                BtnGenForCerOrd.CssClass = "btn btn-sm shadow button-enabled";

                BtnImpPlaActBlo.Enabled = true;
                BtnImpPlaActBlo.CssClass = "btn btn-sm shadow button-enabled";

            }
            else if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {

                BtnVisGenCot.Enabled = true;
                BtnVisGenCot.CssClass = "btn btn-sm shadow button-enabled";
            }


        }





        // VER COTIZACION Y CAMBIO COTIZACION 
        protected void btnCotizacion_Click(object sender, EventArgs e)
        {
            if (txtCotizacion.Text.ToUpper() != "NO TIENE")
            {
                // Construir la ruta al archivo de Excel
                string rutaArchivo = @"\\172.16.30.6\Recepcion\Cotizaciones Excel\" + ddlZona.SelectedValue + @"\" + tbVenta.Text.Substring(0, 4) + @"\" + ObtenerNombreMes() + @"\" + txtCotizacion.Text + ".xls";

                try
                {
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
                        string mensajeExito = "La Cotización  " + txtCotizacion.Text.Trim() + " ha sido cambiada o borrada en el servidor.";
                        string scriptExito = "alert('" + mensajeExito + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                    }
                }
                catch (Exception ex)
                {

                    string mensajeExito = "Error al intentar abrir el archivo, Por favor intente mas tarde o comuniquese con Sistemas.";
                    string scriptExito = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

                }
            }



        }

        private string ObtenerNombreMes()
        {
            int numeroMes = int.Parse(tbVenta.Text.Substring(5, 2));
            string nombreMes = new DateTime(DateTime.Now.Year, numeroMes, 1).ToString("MMM");
            return nombreMes.Replace(".", "");
        }

        protected void txtCotizacion_TextChanged(object sender, EventArgs e)
        {
            if (txtCotizacion.Text.ToUpper() != "NO TIENE")
            {
                ConsultarCotizacion(txtCotizacion.Text);
            }
            else
            {
                // Para Cuando la cotizacion es igual a NO TIENE
                CargarValoresCotizacion();
            }
        }

        private void CargarValoresCotizacion()
        {
            txtValorSugerido.Text = "0";
            txtVcsd.Text = "0";
            txtVccd.Text = "0";
            txtSaldo.Text = "0";
            txtDcto.Text = "0";
            txtVenta.Text = "0";
            txtDcto.Text = "0";
            txtDctoValor.Text = "0";
            txtVtte.Text = "0";
            txtVvia.Text = "0";
            txtGtotal.Text = "0";
            txtComision.Text = "0";
            txtDiseño.Text = "";
        }

        private void ConsultarCotizacion(string cotizacion)
        {
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString))
            {
                sqlconectar.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM tblCotización WHERE cotización = @cotizacion", sqlconectar))
                {
                    cmd.Parameters.AddWithValue("@cotizacion", cotizacion);

                    SqlDataReader leer = cmd.ExecuteReader();

                    if (leer.Read())
                    {
                        // Llenar los TextBox con los valores obtenidos
                        txtValorSugerido.Text = leer["ValorSugerido"].ToString();
                        txtVccd.Text = leer["VCCD"].ToString();
                        txtVcsd.Text = leer["Valor"].ToString();
                        txtSaldo.Text = leer["Saldo"].ToString();
                        txtDiseño.Text = leer["Diseño"].ToString();

                        if (Convert.ToBoolean(Session["NuevaOTEjecutada"]?.ToString()) == true || Convert.ToBoolean(Session["BtnModificarEjecutado"]?.ToString()) == true)
                        {
                            txtVenta.Text = leer["Saldo"].ToString();
                        }
                        txtDcto.Text = leer["Descuento"].ToString();
                        txtComision.Text = leer["DescuentoComision"].ToString();

                        txtVtte.Text = "0";
                        txtVvia.Text = "0";

                        if (leer["Saldo"].ToString() == leer["Valor"].ToString())
                        {
                            txtVtte.Text = leer["ValorTteVia"].ToString(); ;
                            txtVvia.Text = leer["ValorViatico"].ToString();
                        }



                        DateTime fechaCotizacion = Convert.ToDateTime(leer["Fecha_Cotización"].ToString());
                        if ((DateTime.Now - fechaCotizacion).Days > 30)
                        {
                            // Mostrar mensaje de alerta para la fecha
                            string scriptFechaExcedida = "alert('La cotización digitada excede los 30 días.\\nEl sistema le permite grabar la orden de pedido, pero no deja terminar el pedido (Pasar el pedido a Dibujo y Despiece).');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showFechaExcedida", scriptFechaExcedida, true);
                        }

                        txtDctoValor.Text = (Convert.ToDouble(txtVenta.Text) * Convert.ToDouble(txtDcto.Text)).ToString();
                        txtGtotal.Text = (Convert.ToDouble(txtVenta.Text) + Convert.ToDouble(txtVtte.Text) + Convert.ToDouble(txtVvia.Text)).ToString();


                        if (Convert.ToInt32(leer["Saldo"].ToString()) == 0)
                        {
                            txtValorSugerido.Text = "";
                            txtVccd.Text = "";
                            txtVcsd.Text = "";
                            txtSaldo.Text = "";
                            txtDiseño.Text = "";
                            txtDcto.Text = "";
                            txtComision.Text = "";
                            txtVenta.Text = "";
                            txtVtte.Text = "";
                            txtVvia.Text = "";
                            txtDctoValor.Text = "";
                            txtGtotal.Text = "";
                            string scriptSaldoMayorCero = "alert('La cotización digitada, no tiene saldo para un nuevo pedido. Por favor digite nuevamente un numero de cotización');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSaldo", scriptSaldoMayorCero, true);
                            ScriptManager.GetCurrent(this).SetFocus(txtCotizacion);
                            txtCotizacion.Text = "";

                        }


                    }
                    else
                    {
                        // Mensaje de Fallo que la cotizacion no existe  ¿ Desea volver a digitar la cotización ?
                        string scriptSaldoMayorCero = "alert('La cotización digitada no existe. Por favor vuelva a digitar');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSaldo", scriptSaldoMayorCero, true);
                        ScriptManager.GetCurrent(this).SetFocus(txtCotizacion);
                        txtCotizacion.Text = "";

                    }
                }
            }
        }

        protected bool ValidarPermisoInfoContable()
        {
            // Obtener la cédula del usuario logueado de la variable de sesión
            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();

            // Realizar la consulta para verificar los permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string query = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = @CedulaLogueada AND ID_Permiso = '22'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer parámetro para evitar SQL Injection
                    command.Parameters.AddWithValue("@CedulaLogueada", cedulaLogueada);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    bool tienePermisos = reader.HasRows;

                    reader.Close();

                    return tienePermisos;
                }
            }
        }

        protected bool ValidarPermisoCompartido(string id)
        {
            string consultaActual = "SELECT CompartidoCon FROM tblClienteObra INNER JOIN tblClienteObraContacto ON tblClienteObra.Nit = tblClienteObraContacto.cocNIT WHERE tblClienteObraContacto.IdContacto = @IDContacto_Cliente";
            string cadenaActual = ""; // Aquí almacenaremos la cadena actual de nombres

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(consultaActual, connection))
                {
                    command.Parameters.AddWithValue("@IDContacto_Cliente", id);

                    // Ejecutar la consulta y manejar el resultado
                    object result = command.ExecuteScalar();

                    // Verificar si el resultado es DBNull.Value o null
                    if (result != null && result != DBNull.Value)
                    {
                        cadenaActual = result.ToString();
                    }
                }
            }

            // Verificar si la cadenaActual contiene el usuario logueado
            return cadenaActual.Contains(Session["usuariologueado"].ToString());
        }


        private bool ValidarPermisoInfoContable(SqlDataReader Datos)
        {

            if (Session["CedulaLogeada"].ToString() == Datos["Codigo_Asesor"].ToString() || ValidarPermisoInfoContable() || ValidarPermisoCompartido(Datos["IDContacto_Cliente"].ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        private void CargarDatosContables(string id)
        {
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString))
            {
                sqlconectar.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT tblClienteObraContacto.*, tblClienteObra.* FROM tblClienteObra INNER JOIN tblClienteObraContacto ON tblClienteObra.Nit = tblClienteObraContacto.cocNIT WHERE tblClienteObraContacto.IdContacto = @IdContactoCliente", sqlconectar))
                {
                    cmd.Parameters.AddWithValue("@IdContactoCliente", id);

                    SqlDataReader leer = cmd.ExecuteReader();

                    if (leer.Read())
                    {
                        txtNit.Text = leer["CocNIT"].ToString();
                        txtNombreEmp.Text = leer["RazonSocial"].ToString() + " " + leer["SegundoApellido"].ToString() + leer["Nombre"].ToString();
                        txtcontacto.Text = leer["cocNombre"].ToString();
                        txtMail.Text = leer["cocMail"].ToString();
                        txtDireccion.Text = leer["cocDireccion"].ToString();
                        txtMunicipio.Text = leer["cocCiudad"].ToString();
                        txtTelefono.Text = leer["cocTelefono"].ToString();
                    }
                }
            }
        }

        // FIN 

        public void Cargar_OTs2()
        {
            id = Session["Id_OT2"]?.ToString();
            pedido = Session["pedido2"]?.ToString();


            try
            {
                using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString))
                {
                    sqlconectar.Open();

                    if (TryExecuteOTQuery(id, pedido, sqlconectar, out SqlDataReader leer))
                    {
                        if (leer.Read())
                        {

                            // Extraer datos y asignarlos a controles
                            AssignDataToControls(leer);

                            EnableButtons();

                            HabilitarBotonesPlano();
                            // Obtener datos de cotización y asignarlos a controles
                            AssignCotizacionData(id, pedido, txtCotizacion.Text);


                        }
                    }
                  
                }


            }
            catch (Exception ex)
            {
                // Manejo de excepciones
            }

            Cargar_Plano(id, pedido);
            Cargar_Despiece_Plano();

            string valorTextBox = tbObra.Text.Trim(); // Obtener el valor del TextBox

            // Guardar el valor en una variable de sesión
            Session["ValorDeObra"] = valorTextBox;

            Session["CargarOTsEjecutada"] = true;


        }

        protected void calcularDescuento()
        {
            // Harley variables
            double valorVenta = double.Parse(txtVenta.Text);
            double valorDescuento = double.Parse(txtDcto.Text);


            // Formulas
            double valorDescuentoCalculado = valorVenta * valorDescuento / 100;


            //  valor del descuento en el textbox
            txtDctoValor.Text = string.Format("{0:N0}", double.Parse(valorDescuentoCalculado.ToString()));

        }

        protected void calcularGranTotal()
        {
            double valorVenta = double.Parse(txtVenta.Text);
            double valorDesPesos = double.Parse(txtDctoValor.Text);
            double ValorTransporte = double.Parse(txtVtte.Text);
            double ValorViatico = double.Parse(txtVvia.Text);

            //Formula
            double ValorGranTotal = valorVenta - valorDesPesos + ValorTransporte + ValorViatico;

            //valor del descuento en el textbox
            txtGtotal.Text = string.Format("{0:N0}", double.Parse(ValorGranTotal.ToString()));
        }

        protected void NuevoPedido(object sender, EventArgs e)
        {
            HabilitarTodosLosTextBoxes();

            ValorPorDefectoTexArea();

            cboPedidoBase.ClearSelection();
            cboPedidoBase.Items.Clear();

            dtacboTipoPedido.ClearSelection();
            dtacboTipoPedido.Items.Clear();

            ObservacionCont.InnerText = string.Empty;

            TextTNegociacion.InnerText = string.Empty;

            txtCotizacion.Text = string.Empty;

            NuevaOt.Enabled = false;
            NuevaOt.CssClass = "btn btn-sm shadow button-disabled";

            GrabarOt.Enabled = true;
            GrabarOt.CssClass = "btn btn-sm shadow button-enabled";

            ModificarOt.Enabled = false;
            ModificarOt.CssClass = "btn btn-sm shadow button-disabled";

            AnularPedido.Enabled = false;
            AnularPedido.CssClass = "btn btn-sm shadow button-disabled";

            imprimirOt.Enabled = false;
            imprimirOt.CssClass = "btn btn-sm shadow button-disabled";

            Cancelar.Enabled = true;
            Cancelar.CssClass = "btn btn-sm shadow button-enabled";

            btnNuevoPedido.Enabled = false;
            btnNuevoPedido.CssClass = "btn btn-sm shadow button-disabled";

        }

        //MODIFICADO POR CARLOS PINEDA
        protected void Acabados_Click(object sender, EventArgs e)
        {
            // Verifica si el LinkButton está habilitado
            if (EstaHabilitado3())
            {
                string url = "FormExtPrin/AcabadosOT.aspx";
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

            }
        }

        private bool EstaHabilitado3()
        {
            // Agrega tu lógica para determinar si el LinkButton está habilitado o no.
            // Devuelve true si está habilitado y false si no lo está.
            // Ejemplo: Siempre habilitado
            return true;
        }

        protected void Boton_Ok(object sender, EventArgs e)
        {

        }

        //FIN

        // Logica de tap de plano 

        public void Cargar_Plano(string id, string pedido)
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("CtaPlano_OT", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
            cmd.Parameters.Add("@Conse", SqlDbType.VarChar, 30).Value = pedido;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                lbPlano.Text = dr["Plano"].ToString();
                txtPlano.Text = dr["Plano"].ToString();
                txtCliente.Text = dr["Nombre_Cliente"].ToString();
                txtArea.Text = dr["Area"].ToString();
                txtAsesorPlano.Text = dr["AsesorComercial"].ToString();
                txtDibuja.Text = dr["RealizadoPor"].ToString();
                txtBolsa.Text = dr["Bolsa"].ToString();
                txtContactoPlano.Text = dr["Contacto_Cliente"].ToString();
                txtAsesorPlano.Text = dr["AsesorComercial"].ToString();
            }

        }

        public void Cargar_Plano2(string plano)
        {
            // Realizamos la consulta SQL para obtener los datos necesarios
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT *	FROM tblPlano 	WHERE Plano  =  @Plano	ORDER BY Plano";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Plano", plano);
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    lbPlano.Text = dr["Plano"].ToString();
                    txtPlano.Text = dr["Plano"].ToString();
                    txtCliente.Text = dr["Nombre_Cliente"].ToString();
                    txtArea.Text = dr["Area"].ToString();
                    txtAsesorPlano.Text = dr["AsesorComercial"].ToString();
                    txtDibuja.Text = dr["RealizadoPor"].ToString();
                    txtBolsa.Text = dr["Bolsa"].ToString();
                    txtContactoPlano.Text = dr["Contacto_Cliente"].ToString();

                    // Se carga el despiece
                    Cargar_Despiece_Plano();
                }

                dr.Close();

            }

        }

        public void Cargar_Despiece_Plano()
        {
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cn))
            {

                SqlCommand command = new SqlCommand("cta_Plano_Paneles", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Plan", SqlDbType.VarChar, 30).Value = txtPlano.Text;


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                StringBuilder resumen = new StringBuilder();
                var gruposUnicos = dataTable.AsEnumerable().Select(r => r.Field<string>("Descripcion_Grupo")).Distinct();

                List<DatosFiltrados> datosFiltradosList = new List<DatosFiltrados>();


                foreach (var grupo in gruposUnicos)
                {

                    if (resumen.Length > 0)
                    {
                        resumen.Append(" - ");
                    }

                    int sumaCantidad = dataTable.AsEnumerable()
                        .Where(r => r.Field<string>("Descripcion_Grupo") == grupo)
                        .Sum(r => Convert.ToInt32(r["Cantidad"]));

                    resumen.Append(grupo.Substring(0, 3) + " (" + sumaCantidad + ")");


                    datosFiltradosList.Add(new DatosFiltrados { Tipo = "Titulo", Titulo = "<b>" + grupo });

                    var datosFiltrados = dataTable.AsEnumerable().Where(r => r.Field<string>("Descripcion_Grupo") == grupo).Select(r => new DatosFiltrados
                    {

                        ID = r["Id_Numerico"].ToString(),
                        Descripcion = r["Descripcion_Panel"].ToString(),
                        Altura = r["Altura"].ToString(),
                        Ancho = r["Ancho"].ToString(),
                        Cantidad = r["Cantidad"].ToString(),
                        ValorUnd = r["Precio_Venta"].ToString(),
                        SubTotal = (Convert.ToDecimal(r["Cantidad"]) * Convert.ToDecimal(r["Precio_Venta"])).ToString(),
                        Id_Panel = r["Id_Panel"].ToString(),
                        RevisadoDibujo = Convert.ToBoolean(r["RevisadoDibujo"].ToString())



                    });



                    datosFiltradosList.AddRange(datosFiltrados);

                    decimal totalVenta = datosFiltrados.Sum(d => Convert.ToDecimal(d.SubTotal));
                    datosFiltradosList.Add(new DatosFiltrados { Tipo = "Total", SubTotal = "<b>" + totalVenta.ToString() + "</b>" });


                }


                // Agregar el resumen al del plano 
                txResumen.InnerText = resumen.ToString();

                decimal totalGeneral = datosFiltradosList.Where(d => d.Tipo != "Titulo" && d.Tipo != "Total").Sum(d => Convert.ToDecimal(d.SubTotal));
                decimal Cantidad = datosFiltradosList.Where(d => d.Tipo != "Titulo" && d.Tipo != "Total").Sum(d => Convert.ToDecimal(d.Cantidad));

                // Agregar la fila de total general al final del DataGrid
                datosFiltradosList.Add(new DatosFiltrados { Tipo = "Total", Titulo = "<b>Totales</b>", Cantidad = "<b>" + Cantidad.ToString() + "</b>", SubTotal = "<b>" + totalGeneral.ToString() + "</b>" });

                lblCantidad1.Text = Cantidad.ToString();
                lblValorDespiece1.Text = totalGeneral.ToString();


                DataGridDespiecePlano.DataSource = datosFiltradosList;
                DataGridDespiecePlano.DataBind();
            }
        }

        protected void CarteraVencida()
        {
            string consulta = "SELECT edvcliente, edvtipodocuclie, edvnumedocuclie, edvfechexpe, edvformapago, edvfechvenc, edvtotamoneloca, edvsalddoculoca " +
                              "FROM ca_encdocvta " +
                              "WHERE edvcliente = '800225057' " +
                              "AND edvsalddocunego > 0 " +
                              "AND edvfechvenc < GETDATE() " +
                              "AND edvsignodocu = 1 " +
                              "ORDER BY edvfechvenc DESC";

            string connectionString = ConfigurationManager.ConnectionStrings["SSF_PRUEBAS"].ConnectionString;

            // Crear una lista para almacenar los resultados
            List<CustomObject> listaResultados = new List<CustomObject>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(consulta, connection);

                connection.Open();

                // Utilizar SqlDataAdapter para obtener los datos de la consulta
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // Recorrer cada fila del DataTable y guardarla en la lista de objetos
                foreach (DataRow row in dataTable.Rows)
                {
                    CustomObject obj = new CustomObject();
                    obj.EdvCliente = row["edvcliente"].ToString();
                    obj.EdvTipoDocuClie = row["edvtipodocuclie"].ToString();
                    obj.edvnumedocuclie = row["edvnumedocuclie"].ToString();
                    obj.edvfechexpe = row["edvfechexpe"].ToString();
                    obj.edvformapago = row["edvformapago"].ToString();
                    obj.edvfechvenc = row["edvfechvenc"].ToString();
                    obj.edvtotamoneloca = row["edvtotamoneloca"].ToString();
                    obj.edvsalddoculoca = row["edvsalddoculoca"].ToString();


                    listaResultados.Add(obj);
                }
            }

            string contenidoModal = "El cliente: " + txtNombreEmp.Text + " tiene CARTERA VENCIDA de: ";

            foreach (CustomObject obj in listaResultados)
            {
                contenidoModal += $"{obj.EdvTipoDocuClie} {obj.edvnumedocuclie},";
            }

            contenidoModal += " por un valor de: ";

            decimal sumaSaldoDocuLoca = listaResultados.Sum(x => decimal.TryParse(x.edvsalddoculoca, out decimal result) ? result : 0);
            DateTime ultimaFechaExpe = listaResultados.Any() ? DateTime.Parse(listaResultados.Last().edvfechvenc) : DateTime.MinValue;

            contenidoModal += $"{sumaSaldoDocuLoca:C2} desde el: {ultimaFechaExpe:d}";




            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#CarteraVencida').modal('show'); $('#CarteraVencida2').text('" + contenidoModal + "');", true);

            txtMensaje.Text = "CARTERA VENCIDA " + sumaSaldoDocuLoca.ToString();

            txtMensaje.Visible = true;


        }

        // Clase personalizada para almacenar los resultados
        public class CustomObject
        {
            public string EdvCliente { get; set; }
            public string EdvTipoDocuClie { get; set; }

            public string edvnumedocuclie { get; set; }

            public string edvfechexpe { get; set; }

            public string edvformapago { get; set; }

            public string edvfechvenc { get; set; }

            public string edvtotamoneloca { get; set; }

            public string edvsalddoculoca { get; set; }

        }



        protected void DataGridDespiecePlano_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DatosFiltrados datos = (DatosFiltrados)e.Item.DataItem;

                if (datos.Tipo == "Total" && datos.Titulo == "<b>Totales</b>")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#7aeaff");
                }
                else if (!string.IsNullOrEmpty(datos.ID))
                {
                    if (datos.RevisadoDibujo == true)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#47ca4b");
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    }

                }

            }
        }

        protected void DataGridDespiecePlano_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerPlano")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDespiecePlano.Items[rowIndex];


                foreach (DataGridItem item in DataGridDespiecePlano.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada";
                string ID = row.Cells[2].Text;
                string Descri = row.Cells[9].Text;
                string Ancho = row.Cells[5].Text;

                LlenarDataGridObjeto(ID);




                // Se envia la descripcion como parametro de busqueda al tap objetos  y se refresca el panel deobjetos 
                txtCantidad.Text = Ancho;
                tbCriterio.Text = Descri;
                PanelObjeto.Update();

                btnCambiar.Enabled = true;
                btnCambiar.CssClass = "btn btn-outline-secondary";

                Session["IdObjetoEliminarSession"] = ID;
                BtnQuiObjPla.Enabled = true;
                BtnQuiObjPla.CssClass = "btn btn-sm shadow button-enabled";

            }
        }

        // Se Debe Modificar el procedimienato almacenado
        public void LlenarDataGridObjeto(string idPanelNum)
        {
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand("Sp_ObtenerDatosModulo", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IdPanelNum", SqlDbType.VarChar, 30).Value = idPanelNum;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                DataGridDescripcionObjetos.DataSource = dataTable;
                DataGridDescripcionObjetos.DataBind();

            }
        }

        public void LlenarDataGridModuloObjeto(string idPanelNum)
        {
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand("sp_ObtenerDatosModulo1", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IdPanelNum", SqlDbType.VarChar, 30).Value = idPanelNum;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                DataGridModuloObjetos.DataSource = dataTable;
                DataGridModuloObjetos.DataBind();

            }
        }

        public void LlennarDatagridAcabado(string Idmodulo, string IdFamilia)
        {
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand("sp_ObtenerAcabadosItemPlano", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Plano", SqlDbType.VarChar, 30).Value = txtPlano.Text;
                command.Parameters.Add("@IdModulo", SqlDbType.VarChar, 30).Value = Idmodulo;
                command.Parameters.Add("@IdFamilia", SqlDbType.VarChar, 30).Value = IdFamilia;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    DataGridAcabado.DataSource = dataTable;
                    DataGridAcabado.DataBind();
                }
                else
                {
                    // El procedimiento no devolvió datos, ejecutar otro procedimiento o manejar la lógica correspondiente
                    // Por ejemplo, podrías llamar a otro procedimiento almacenado aquí
                    SqlCommand command1 = new SqlCommand("sp_ObtenerAcabadosItemPlano2", connection);
                    command1.CommandType = CommandType.StoredProcedure;

                    command1.Parameters.Add("@Plano", SqlDbType.VarChar, 30).Value = txtPlano.Text;
                    command1.Parameters.Add("@IdModulo", SqlDbType.VarChar, 30).Value = Idmodulo;

                    SqlDataAdapter adapter2 = new SqlDataAdapter(command1);
                    DataTable dataTable2 = new DataTable();
                    adapter2.Fill(dataTable2);

                    DataGridAcabado.DataSource = dataTable2;
                    DataGridAcabado.DataBind();
                }

            }
        }

        protected void DataGridDescripcionObjeto_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int chequeado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Chequeado"));

                TableCell cell = e.Item.Cells[4];
                cell.Text = (chequeado == 1) ? "Si" : "No";


            }
        }

        protected void DataGridDescripcionObjeto_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerAcabado")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDescripcionObjetos.Items[rowIndex];

                string IdModulo = row.Cells[1].Text;
                string IdFamilia = row.Cells[12].Text;
                LlennarDatagridAcabado(IdModulo, IdFamilia);

                foreach (DataGridItem item in DataGridDescripcionObjetos.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada";


            }
        }

        protected void DataGridDespacho_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int DespachoInterno = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DespachoInterno"));
                int DespachoCoordinado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DespachoCoordinado"));
                int Terminado_despacho = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Terminado_despacho"));
                int Entregado_Transporte = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Entregado_Transporte"));

                TableCell cell = e.Item.Cells[1];
                cell.Text = (DespachoInterno == 1) ? "Si" : "No";

                TableCell cell2 = e.Item.Cells[2];
                cell2.Text = (DespachoCoordinado == 1) ? "Si" : "No";

                TableCell cell3 = e.Item.Cells[5];
                cell3.Text = (Terminado_despacho == 1) ? "Si" : "No";

                TableCell cell4 = e.Item.Cells[7];
                cell4.Text = (Entregado_Transporte == 1) ? "Si" : "No";


            }
        }

        // Logica Tap de Objetos 

        protected void BuscarObjeto(object sender, EventArgs e)
        {


            if (chxBloques.Checked == true)
            {
                if (rbObjeto.SelectedValue == "Objeto")
                {
                    ObtenerDatosObjetos.SelectCommand = "sp_ObtenerDatosObjetoActivo";
                    ObtenerDatosObjetos.DataBind();
                    DataGridObjetos.DataBind();

                }
                else if (rbObjeto.SelectedValue == "Descripcion")
                {
                    ObtenerDatosObjetos.SelectCommand = "sp_ObtenerDatosDescripActivo";
                    ObtenerDatosObjetos.DataBind();
                    DataGridObjetos.DataBind();
                }
            }
            else
            {
                if (rbObjeto.SelectedValue == "Objeto")
                {
                    ObtenerDatosObjetos.SelectCommand = "sp_ObtenerDatosObjetoIdNoActivo";
                    ObtenerDatosObjetos.DataBind();
                    DataGridObjetos.DataBind();
                }
                else if (rbObjeto.SelectedValue == "Descripcion")
                {
                    ObtenerDatosObjetos.SelectCommand = "sp_ObtenerDatosObjetoDescripcionNoActivo";
                    ObtenerDatosObjetos.DataBind();
                    DataGridObjetos.DataBind();

                }


            }

            DataGridModuloObjetos.DataBind();
            lbTituloObjeto.Text = "Descripcion Objeto";
            ValorlbDipLa2.Text = "0 Cms";
            ValorlbDipLa3.Text = "0 Cms";
        }

        protected void DataGridObtenerDatosObjetos_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int OK = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Chequeado"));
                int Activo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Activo"));
                int Esc = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Escalable"));

                TableCell cell = e.Item.Cells[10];
                cell.Text = (OK == 1) ? "Si" : "No";

                TableCell cell1 = e.Item.Cells[17];
                cell1.Text = (Activo == 1) ? "Si" : "No";

                TableCell cell2 = e.Item.Cells[18];
                cell2.Text = (Esc == 1) ? "Si" : "No";




            }
        }

        protected void DataGridObtenerDatosObjetos_LinkButton(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerObjetoDet")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridObjetos.Items[rowIndex];

                foreach (DataGridItem item in DataGridObjetos.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = "";
                    }
                }

                e.Item.CssClass = "fila-seleccionada";

                // Variable de Session para consultar informacion del modulo en la pagina Despiece 
                string Id_PanelNum = row.Cells[8].Text;
                Session["Id_PanelNum_Session"] = Id_PanelNum;

                // Variable de Session para consultar Campos del objeto en la pagina despiece 
                string Id_Objeto = row.Cells[1].Text;
                Session["Id_ObjetoSession"] = Id_Objeto;


                string ValorVenta = row.Cells[6].Text;
                Session["ValorVentaSession"] = ValorVenta;


                string descrip = row.Cells[2].Text;
                string Altura = row.Cells[4].Text;



                LlenarDataGridModuloObjeto(Id_PanelNum);

                lbTituloObjeto.Text = descrip;
                ValorlbDipLa2.Text = Altura + " Cms";
                ValorlbDipLa3.Text = Altura + " Cms";


                // Se compara si el click es en la misma fila con el id del plano 
                if (row.Cells[8].Text == Session["Id_PanelNum_Session1"]?.ToString())
                {
                    // Incrementar la variable de sesión "ClickCount" en el servidor
                    int clickCount = Convert.ToInt32(Session["ClickCount1"]) + 1;
                    Session["ClickCount1"] = clickCount;

                    // se valida si es el segundo click en la misma fila 
                    if (clickCount == 2)
                    {
                        string url = "FormExtPrin/ObjetoDespiece.aspx";
                        string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

                        // Reiniciar la variable de sesión "ClickCount" a 0 para la próxima interacción
                        Session["ClickCount1"] = 0;
                    }

                }
                else
                {
                    // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                    Session["ClickCount1"] = 1;
                    Session["Id_PanelNum_Session1"] = row.Cells[8].Text;
                }

            }
        }

        protected void DataGridModuloObjeto_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int Chequeado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Chequeado"));

                TableCell cell = e.Item.Cells[4];
                cell.Text = (Chequeado == 1) ? "Si" : "No";




            }
        }

        protected void Reedireccion_ObjetoDespiece(object sender, EventArgs e)
        {
            string url = "FormExtPrin/ObjetoDespiece.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
        }

        protected void Redireccion_Nit(object sender, EventArgs e)
        {
            string url = "FormExtPrin/NitOts.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
        }

        protected void Redireccion_Plano1(object sender, EventArgs e)
        {
            string url = "FormExtPrin/Plano1.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
        }

        // Colsultar Bolsa
        protected void ConsultarBolsa1(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalBolsa').modal('show');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ActualizarValor", "actualizarValor();", true);
        }

        protected void DataGridBolsa_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int saldo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Saldo"));
                TableCell saldoCell = e.Item.Cells[4];


                if (saldo > 0)
                {
                    saldoCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1FF43"); // Amarillo 
                    saldoCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                }
                else if (saldo == 0)
                {
                    saldoCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525"); // Verde
                }
                else if (saldo < 1)
                {
                    saldoCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F71A27"); // Rojo 
                    saldoCell.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
            }
        }

        //FIN

   
        public void CargarVariablesDeSesionContable()
        {
            if (!string.IsNullOrEmpty(Session["IdContactoFactSession"]?.ToString()) && !string.IsNullOrEmpty(Session["IdClienteFactSession"]?.ToString()))
            {
                //Cargar los Datos del cliente 

                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM tblClienteObraContacto INNER JOIN tblClienteObra ON Nit = cocNIT where cocNIT= @ParametroCliente and IdContacto = @ParametroClienteContacto";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ParametroCliente", (Session["IdClienteFactSession"].ToString()));
                        command.Parameters.AddWithValue("@ParametroClienteContacto", (Session["IdContactoFactSession"].ToString()));

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("Nit")))
                                {
                                    txtNit.Text = reader["Nit"].ToString();

                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("RazonSocial")))
                                {
                                    txtNombreEmp.Text = reader["RazonSocial"].ToString();

                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("cocNombre")))
                                {
                                    txtcontacto.Text = reader["cocNombre"].ToString();

                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("cocMail")))
                                {
                                    txtMail.Text = reader["cocMail"].ToString();

                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("cocDireccion")))
                                {
                                    txtDireccion.Text = reader["cocDireccion"].ToString();

                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("cocCiudad")))
                                {
                                    txtMunicipio.Text = reader["cocCiudad"].ToString();

                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("cocTelefono")))
                                {
                                    txtTelefono.Text = reader["cocTelefono"].ToString();

                                }

                                Session.Remove("IdClienteFactSession");
                                Session.Remove("IdContactoFactSession");

                            }
                        }
                    }
                }
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            // Realiza la validación de campos
            string campoFaltante = ValidarCampos();

            if (string.IsNullOrEmpty(campoFaltante))
            {

                // Verificar si se ha ejecutado el evento NuevaOT_Click
                if (Session["NuevaOTEjecutada"] != null && (bool)Session["NuevaOTEjecutada"])
                {
                    try
                    {


                        InsertarOT();
                        InsertarConsecutivo();
                        InsertarPlano();
                    

                        if (Session["CopiarInfOTEjecutada"] != null && (bool)Session["CopiarInfOTEjecutada"])
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#CopiarAcabados').modal('show');", true);
                        }
                        else
                        {
                           

                            Session.Remove("CopiarInfOTEjecutada");

                        }


                        Session.Remove("CopiarInfOTEjecutada");
                        Session.Remove("CopiarInfOTEjecutada");
                    }
                    catch (Exception ex)
                    {

                    }

                }
                // Verificar si se ha ejecutado el evento BtnModificar_Click
                else if (Session["BtnModificarEjecutado"] != null && (bool)Session["BtnModificarEjecutado"])
                {
                    //try
                    //{
                    ValidarUsuario();
                    Session.Remove("OTinsertada");
                    Session.Remove("PedidoInsertado");

                    //}
                    //catch (Exception ex)
                    //{
                    //    // Manejo de excepciones
                    //}
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ErrorPermiso').modal('show');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalll').modal('show'); $('#campoFaltante').text('" + campoFaltante + "');", true);
            }

        }

        protected void InsertarOT()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtener el valor para @Id_OT
                string consultaObtenerId_OT = "SELECT ValorPrincipal FROM tblConsecutivo WHERE Descripcion = 'OT' + @ZonaLogeada ORDER BY ValorPrincipal DESC OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

                using (SqlCommand cmdObtenerId_OT = new SqlCommand(consultaObtenerId_OT, connection))
                {
                    // Suponiendo que 'ZonaLogeada' es el nombre de la variable de sesión
                    cmdObtenerId_OT.Parameters.AddWithValue("@ZonaLogeada", Session["ZonaLogeada"]);

                    object valorPrincipal = cmdObtenerId_OT.ExecuteScalar();


                    // Verificar si se obtuvo un valor
                    if (valorPrincipal != null)
                    {
                        string zonaLogeada = Session["ZonaLogeada"].ToString();
                        int valorPrincipalInt = Convert.ToInt32(valorPrincipal);
                        int nuevoId_OT = valorPrincipalInt + 1;

                        string nuevoIdOTConcatenado = zonaLogeada + nuevoId_OT;

                        string[] valoresDDL = ddlCiudad.SelectedValue.Split('-');

                        if (valoresDDL.Length == 2)
                        {
                            string ciudadSeleccionada = valoresDDL[0].Trim();
                            string regionSeleccionada = valoresDDL[1].Trim();


                            using (SqlCommand command = new SqlCommand("sp_InsertarDatos_TBLOT", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                // Asignar el valor generado para @Id_OT
                                command.Parameters.AddWithValue("@Id_OT", nuevoIdOTConcatenado);
                                command.Parameters.AddWithValue("@Consecutivo_Pedido", 1);

                                command.Parameters.AddWithValue("@Observacion_Pedido", txObs1.Value);
                                command.Parameters.AddWithValue("@Direccion", tbDir.Text);



                                command.Parameters.AddWithValue("@Ciudad", ciudadSeleccionada);
                                command.Parameters.AddWithValue("@Region", regionSeleccionada);



                                command.Parameters.AddWithValue("@Pais", tbPais.Text);
                                command.Parameters.AddWithValue("@TelDomicilio", tbTel.Text);
                                command.Parameters.AddWithValue("@CelularContacto", tbCel.Text);
                                command.Parameters.AddWithValue("@Persona_Receptora", tbContac.Text);
                                command.Parameters.AddWithValue("@Fecha_Confirmacion_Venta", tbVenta.Text);
                                command.Parameters.AddWithValue("@Fecha_Entrega_Dibujo_Despiece", dtpFechaEntregaDibujoDespiece.Text);
                                command.Parameters.AddWithValue("@Fecha_Entrega_Produccion", dtpFechaEntregaProduccion.Text);
                                command.Parameters.AddWithValue("@Fecha_Despacho_Produccion", dtpEmpaque.Text);
                                command.Parameters.AddWithValue("@Fecha_Real_Despacho_Produccion", dtpRealEmpaque.Text);
                                command.Parameters.AddWithValue("@Fecha_Instalacion", dtpRealEmpaque.Text);
                                command.Parameters.AddWithValue("@Nombre_Obra", tbObra.Text);
                                command.Parameters.AddWithValue("@IDContacto_Cliente", "20583");
                                command.Parameters.AddWithValue("@Codigo_Asesor", txtAsesor.Text);
                                command.Parameters.AddWithValue("@Zona", ddlZona.Text);
                                command.Parameters.AddWithValue("@Descuento", txtDcto.Text);
                                command.Parameters.AddWithValue("@Precio_Venta", txtVenta.Text);
                                command.Parameters.AddWithValue("@Forma_Pago", TextTNegociacion.Value);
                                command.Parameters.AddWithValue("@Cotizacion", txtCotizacion.Text);
                                command.Parameters.AddWithValue("@Observaciones_Contables", ObservacionCont.Value);
                                command.Parameters.AddWithValue("@Mail_Contacto", tbEmail.Text);
                                command.Parameters.AddWithValue("@Fecha_Despacho_Terceros", dtpRealEmpaque.Text);
                                command.Parameters.AddWithValue("@Id_TipoPedido", dtacboTipoPedido.SelectedValue);
                                command.Parameters.AddWithValue("@Id_OT_secundario", nuevoIdOTConcatenado);
                                command.Parameters.AddWithValue("@RecibeElPedido", tbRecibe.Text);
                                command.Parameters.AddWithValue("@Chequeada", "1");
                                command.Parameters.AddWithValue("@AlturaPT", tbHTotal.Text);
                                command.Parameters.AddWithValue("@TipoAprobacion", DtaCboTipoAprobacion.SelectedValue);
                                command.Parameters.AddWithValue("@FabricadoPor", ddlFabrica1.SelectedValue);
                                command.Parameters.AddWithValue("@InstaladaPor", ddlInstala.SelectedValue);
                                command.Parameters.AddWithValue("@Observacion_Dibujo", txObs2.Value);
                                command.Parameters.AddWithValue("@AbiertoPor", txtAsesor.Text);
                                command.Parameters.AddWithValue("@DescuentoparaComision", txtComision.Text);
                                command.Parameters.AddWithValue("@ValorTteVia", txtVtte.Text);
                                command.Parameters.AddWithValue("@PedidoBase", 1);
                                command.Parameters.AddWithValue("@ValorViatico", txtVvia.Text);
                                command.Parameters.AddWithValue("@Fecha_Empaque", dtpEmpaque.Text);
                                command.Parameters.AddWithValue("@Fecha_Real_Empaque", dtpRealEmpaque.Text);
                                command.Parameters.AddWithValue("@OrdendeCompra", txtOrdenCompra.Text);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Session["OTinsertada"] = nuevoIdOTConcatenado;
                                    Session["PedidoInsertado"] = 1;
                                }
                                else
                                {

                                }


                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
        }

        protected void InsertarConsecutivo()
        {


            string zonaLogeada = Session["ZonaLogeada"].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE tblConsecutivo SET ValorPrincipal = ValorPrincipal + 1 " +
                                     $"WHERE Descripcion = 'OT{zonaLogeada}'";

                SqlCommand command = new SqlCommand(updateQuery, connection);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // La actualización se realizó con éxito
                }
                else
                {
                    // No se actualizaron filas (podrías manejar esta situación si es necesario)
                }

            }
        }

        protected void InsertarPlano()
        {
            string idOT = Session["OTinsertada"].ToString();
            string pedido = Session["PedidoInsertado"].ToString();
            string nombreUsuario = Session["usuariologueado"].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string inserQuery = @"INSERT INTO tblPlano 
                            (Plano, ID_OT, Consecutivo_Pedido, Nombre_Cliente, Contacto_Cliente, 
                             Fecha_Entrega_Bitacora, Fecha_Termino_Diseño, area, Historial, 
                             AsesorComercial, RealizadoPor)
                            VALUES (
                                'PL' + CONVERT(VARCHAR, @IdOT) + '-' + CONVERT(VARCHAR, @Pedido),
                                @IdOT,
                                @Pedido,
                                @NombreCliente,
                                @Receptor,
                                @FechaActual, 
                                @FechaActual,
                                @NombreObra,
                                '',
                                @AsesorComercial,
                                @NombreUsuario
                            )";

                SqlCommand command = new SqlCommand(inserQuery, connection);
                command.Parameters.AddWithValue("@IdOT", idOT);
                command.Parameters.AddWithValue("@Pedido", pedido);
                command.Parameters.AddWithValue("@NombreCliente", txtNombreEmp.Text);
                command.Parameters.AddWithValue("@Receptor", tbRecibe.Text);
                command.Parameters.AddWithValue("@FechaActual", tbVenta.Text);
                command.Parameters.AddWithValue("@NombreObra", tbObra.Text);
                command.Parameters.AddWithValue("@AsesorComercial", ddlAsesor.SelectedItem.Text);
                command.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {

                }
                else
                {
                    // Ocurrió un problema al realizar la inserción
                }
            }
        }

        protected void MostrarModal()
        {
            string idOT = Session["OTinsertada"].ToString();
            string contenidoModalOT = "la Orden de trabajo: " + idOT + " queda asignada a la Obra: " + tbObra.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#OTingresada').modal('show'); $('#OTingresada2').text('" + contenidoModalOT + "');", true);

        }

        private string ValidarCampos()
        {
            string campoFaltante = string.Empty;

            if (dtacboTipoPedido.SelectedItem == null)
            {
                campoFaltante = "Tipo de Pedido";
            }
            else if (DtaCboTipoAprobacion.SelectedItem == null)
            {
                campoFaltante = "Tipo de Aprobacion";
            }
            else if (string.IsNullOrEmpty(tbObra.Text))
            {
                campoFaltante = "Obra";
            }
            else if (string.IsNullOrEmpty(tbRecibe.Text))
            {
                campoFaltante = "Recibe";
            }
            else if (string.IsNullOrEmpty(tbTel.Text))
            {
                campoFaltante = "Telefono";
            }
            else if (string.IsNullOrEmpty(tbCel.Text))
            {
                campoFaltante = "Celular";
            }
            else if (string.IsNullOrEmpty(tbPais.Text))
            {
                campoFaltante = "Pais";
            }
            else if (string.IsNullOrEmpty(tbHTotal.Text))
            {
                campoFaltante = "H.Total";
            }
            else if (string.IsNullOrEmpty(tbDir.Text))
            {
                campoFaltante = "Direccion";
            }
            else if (string.IsNullOrEmpty(tbContac.Text))
            {
                campoFaltante = "Contac";
            }
            else if (string.IsNullOrEmpty(tbEmail.Text))
            {
                campoFaltante = "Email";
            }
            else if (ddlCiudad.SelectedItem == null)
            {
                campoFaltante = "Ciudad";
            }
            else if (string.IsNullOrEmpty(dtpEmpaque.Text))
            {
                campoFaltante = "Empaque";
            }
            else if (ddlFabrica1.SelectedItem == null)
            {
                campoFaltante = "Fabrica";
            }
            else if (ddlInstala.SelectedItem == null)
            {
                campoFaltante = "Instala";
            }
            else if (string.IsNullOrEmpty(ObservacionCont.InnerText))
            {
                campoFaltante = "Observacion Contable";
            }
            else if (string.IsNullOrEmpty(txtCotizacion.Text))
            {
                campoFaltante = "Cotizacion";
            }
            else if (string.IsNullOrEmpty(txtOrdenCompra.Text))
            {
                campoFaltante = "Orden de Compra";
            }
            else if (ddlAsesor.SelectedItem == null)
            {
                campoFaltante = "Asesor";
            }
            else if (string.IsNullOrEmpty(TextTNegociacion.InnerText))
            {
                campoFaltante = "Tipo de Negociacion";
            }

            return campoFaltante;
        }

        protected void ValidarUsuario()
        {


            // Obtener la cédula del usuario logueado de la variable de sesión
            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();

            // Obtener la cédula ingresada en el TextBox txtAsesor
            string cedulaTextBox = txtAsesor.Text;

            // Verificar si las cédulas son iguales
            if (cedulaLogueada == cedulaTextBox)
            {

                ActualizarDatos();
            }
            else
            {

                ValidarPermiso();

            }
        }

        protected void ValidarPermiso()
        {
            // Obtener la cédula del usuario logueado de la variable de sesión
            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();

            // Realizar la consulta para verificar los permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string query = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = @CedulaLogueada AND ID_Permiso = '22'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer parámetro para evitar SQL Injection
                    command.Parameters.AddWithValue("@CedulaLogueada", cedulaLogueada);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ActualizarDatos();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
                    }

                    reader.Close();
                }
            }
        }

        protected void ActualizarDatos()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {


                string[] valoresDDL = ddlCiudad.SelectedValue.Split('-');

                if (valoresDDL.Length == 2)
                {
                    string ciudadSeleccionada = valoresDDL[0].Trim();
                    string regionSeleccionada = valoresDDL[1].Trim();

                    using (SqlCommand command = new SqlCommand("sp_ActualizarDatos_TBLOT", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros al procedimiento almacenado
                        command.Parameters.AddWithValue("@Id_OT", tbOT.Text);
                        command.Parameters.AddWithValue("@Consecutivo_Pedido", ddlNumbers.Text);
                        command.Parameters.AddWithValue("@Observacion_Pedido", txObs1.Value);
                        command.Parameters.AddWithValue("@Direccion", tbDir.Text);
                        command.Parameters.AddWithValue("@Ciudad", ciudadSeleccionada);
                        command.Parameters.AddWithValue("@Region", regionSeleccionada);
                        command.Parameters.AddWithValue("@Pais", tbPais.Text);
                        command.Parameters.AddWithValue("@TelDomicilio", tbTel.Text);
                        command.Parameters.AddWithValue("@CelularContacto", tbCel.Text);
                        command.Parameters.AddWithValue("@Persona_Receptora", tbContac.Text);
                        command.Parameters.AddWithValue("@Nombre_Obra", tbObra.Text);
                        command.Parameters.AddWithValue("@IDContacto_Cliente", "20583");
                        command.Parameters.AddWithValue("@Codigo_Asesor", txtAsesor.Text);
                        command.Parameters.AddWithValue("@Descuento", txtDcto.Text);
                        command.Parameters.AddWithValue("@Precio_Venta", txtVenta.Text);
                        command.Parameters.AddWithValue("@Forma_Pago", TextTNegociacion.Value);
                        command.Parameters.AddWithValue("@Cotizacion", txtCotizacion.Text);
                        command.Parameters.AddWithValue("@Observaciones_Contables", ObservacionCont.Value);
                        command.Parameters.AddWithValue("@Mail_Contacto", tbEmail.Text);
                        command.Parameters.AddWithValue("@Id_TipoPedido", dtacboTipoPedido.SelectedValue);
                        command.Parameters.AddWithValue("@RecibeElPedido", tbRecibe.Text);
                        command.Parameters.AddWithValue("@AlturaPT", tbHTotal.Text);
                        command.Parameters.AddWithValue("@TipoAprobacion", DtaCboTipoAprobacion.SelectedValue);
                        command.Parameters.AddWithValue("@InstaladaPor", ddlInstala.SelectedValue);
                        command.Parameters.AddWithValue("@Observacion_Dibujo", txObs2.Value);
                        command.Parameters.AddWithValue("@DescuentoparaComision", txtComision.Text);
                        command.Parameters.AddWithValue("@ValorTteVia", txtVtte.Text);
                        command.Parameters.AddWithValue("@ValorViatico", txtVvia.Text);
                        command.Parameters.AddWithValue("@Fecha_Empaque", dtpEmpaque.Text);
                        command.Parameters.AddWithValue("@OrdendeCompra", txtOrdenCompra.Text);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();

                        // Verificar si se actualizaron filas
                        if (rowsAffected > 0)
                        {
                            // La actualización se realizó con éxito
                            // Realiza acciones adicionales si es necesario
                        }
                        else
                        {
                            // No se actualizó ninguna fila
                            // Puedes manejar el caso en que la actualización no se realice
                        }
                    }
                }
            }

        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Session["BtnModificarEjecutado"] = true;

            NuevaOt.Enabled = false;
            NuevaOt.CssClass = "btn btn-sm shadow button-disabled";

            CopiarOt.Enabled = false;
            CopiarOt.CssClass = "btn btn-sm shadow button-disabled";

            GrabarOt.Enabled = true;
            GrabarOt.CssClass = "btn btn-sm shadow button-enabled";

            ModificarOt.Enabled = false;
            ModificarOt.CssClass = "btn btn-sm shadow button-disabled";

            AnularPedido.Enabled = false;
            AnularPedido.CssClass = "btn btn-sm shadow button-disabled";

            DocumentacionOt.Enabled = false;
            DocumentacionOt.CssClass = "btn btn-sm shadow button-disabled";

            ObservacionesOt.Enabled = false;
            ObservacionesOt.CssClass = "btn btn-sm shadow button-disabled";

            imprimirOt.Enabled = false;
            imprimirOt.CssClass = "btn btn-sm shadow button-disabled";

            OtPendientes.Enabled = false;
            OtPendientes.CssClass = "btn btn-sm shadow button-disabled";

            ExportarPedido.Enabled = false;
            ExportarPedido.CssClass = "btn btn-sm shadow button-disabled";

            ObraReactivada.Enabled = true;
            ObraReactivada.CssClass = "btn btn-sm shadow button-enabled";

            Cancelar.Enabled = true;
            Cancelar.CssClass = "btn btn-sm shadow button-enabled";

            btnNuevoPedido.Enabled = false;
            btnNuevoPedido.CssClass = "btn btn-sm shadow button-disabled";

            btnAcabados.Enabled = false;
            btnAcabados.CssClass = "btn btn-sm shadow button-disabled";



            HabilitarTodosLosTextBoxes();

            tbPedDepen.Enabled = false;

            ddlFabrica1.Enabled = false;



        }

        protected void ValidarFecha(object sender, EventArgs e)
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaSeleccionada;

            if (DateTime.TryParse(dtpEmpaque.Text, out fechaSeleccionada))
            {
                DateTime fechaMinima = CalcularFechaMinima(fechaActual, fechaSeleccionada);

                if (fechaSeleccionada < fechaMinima)
                {


                    DateTime fechaActuall = DateTime.Now.AddDays(10);

                    // Establecer el valor en el TextBox
                    dtpEmpaque.Text = fechaActuall.ToString("yyyy-MM-dd");

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModallll').modal('show');", true);


                }
                else
                {

                }
            }
            else
            {
                // La entrada de fecha no es válida
                string campoFaltantee = "Ingrese una fecha válida en el formato correcto.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModallll').modal('show'); $('#campoFaltantee').text('" + campoFaltantee + "');", true);
            }
        }

        private DateTime CalcularFechaMinima(DateTime fechaActual, DateTime fechaSeleccionada)
        {
            int diasHabiles = 3;
            DateTime fechaMinima = fechaActual.AddDays(1).Date; // Truncar la hora, minutos, segundos y milisegundos

            List<DateTime> diasNoLaborales = ObtenerDiasNoLaboralesDesdeBaseDeDatos(); // Obtener días festivos de la base de datos

            while (diasHabiles > 0)
            {
                if (fechaMinima.DayOfWeek != DayOfWeek.Saturday &&
                    fechaMinima.DayOfWeek != DayOfWeek.Sunday &&
                    !diasNoLaborales.Contains(fechaMinima.Date)) // Verificar si la fecha es un día festivo
                {
                    diasHabiles--;
                }
                fechaMinima = fechaMinima.AddDays(1);
            }

            return fechaMinima;
        }

        private List<DateTime> ObtenerDiasNoLaboralesDesdeBaseDeDatos()
        {
            List<DateTime> diasNoLaborales = new List<DateTime>();

            // Conectarse a la base de datos y obtener los días festivos posteriores a la fecha actual
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT dnlFecha FROM tblDiaNoLaboral WHERE dnlFecha > GETDATE();";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime diaFestivo = reader.GetDateTime(0);
                    diasNoLaborales.Add(diaFestivo.Date);
                }
            }

            return diasNoLaborales;
        }

        protected void BtnCopInfNueOT_Click(object sender, EventArgs e)
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {

                NuevaOt,
                CopiarOt,
                ModificarOt,
                AnularPedido,
              ObservacionesOt,
                imprimirOt,
                ReimprimirOt,


                ActPedImp,
                ImpPedAse,
                ImpPedSed,
                HabilitarPedido,
                DeshabilitarOt,

                RegPedSisAdm,
                CierraOt,
                SimularPedido,
                OtPendientes,
                EntregaPerfecta,
                AnularObra,
                btnNuevoPedido,
            };

            string cssClass = "btn btn-sm shadow button-disabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;
                    linkButton.Enabled = false;
                    linkButton.CssClass = cssClass;
                }
            }

            GrabarOt.Enabled = true;
            GrabarOt.CssClass = "btn btn-sm shadow button-enabled";

            Cancelar.Enabled = true;
            Cancelar.CssClass = "btn btn-sm shadow button-enabled";

            Nit.Enabled = true;
            Nit.CssClass = "btn btn-sm shadow button-enabled";


            HabilitarTodosLosTextBoxes();

            tbOT.Text = "Por Asig";

            txtCotizacion.Text = string.Empty;

            ObservacionCont.InnerText = string.Empty;

            TextTNegociacion.InnerText = string.Empty;

            tbPedDepen.Enabled = false;


            ValorPorDefectoTexArea();

            LabelOTCerrada.Visible = false;

            Session["NuevaOTEjecutada"] = true;

            Session["CopiarInfOTEjecutada"] = true;

            // Obtener la cédula del usuario logueado de la variable de sesión
            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();

            // Obtener la cédula ingresada en el TextBox txtAsesor
            string cedulaTextBox = txtAsesor.Text;

            // Verificar si las cédulas son iguales
            if (cedulaLogueada == cedulaTextBox)
            {
                Session["NuevaOTEjecutada"] = true;

            }
            else
            {

                Session["NuevaOTEjecutada"] = false;

            }

            //string cedulalogeada = Session["usuariologueado"].ToString();

            //if (cedulalogeada)
            //{
            //    Session["NuevaOTEjecutada"] = true;
            //}
            //else
            //{

            //}

        }

        protected void BtnSi_Click(object sender, EventArgs e)
        {
            string id = Session["Id_OT"]?.ToString();
            string pedido = Session["pedido"]?.ToString();

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pedido))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT ID_Acabado, ID_GrupoObjetoparaAcabado, Detalle_Adicional, AcabadoVentas " +
                                   "FROM tblOTAcabados " +
                                   "WHERE Id_OT = @Id_OT " +
                                   "AND Consecutivo_Pedido = @Consecutivo_Pedido";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id_OT", id);
                    command.Parameters.AddWithValue("@Consecutivo_Pedido", pedido);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Almacena los valores en listas para cada columna
                        ID_Acabados.Add(Convert.ToInt32(reader["ID_Acabado"]));
                        ID_GruposObjetoparaAcabados.Add(Convert.ToInt32(reader["ID_GrupoObjetoparaAcabado"]));
                        Detalles_Adicionales.Add(reader["Detalle_Adicional"].ToString());
                        AcabadosVentas.Add(reader["AcabadoVentas"].ToString());
                    }

                    reader.Close();

                    // Luego de almacenar todos los datos, procede con la inserción
                    RealizarInserciones();


                }
            }
            else
            {

            }

        
        }

        protected void BtnNo_Click(object sender, EventArgs e)
        {
             Session.Remove("OTinsertada");
            Session.Remove("PedidoInsertado");
        }

        protected void RealizarInserciones()
        {
            string OTinsertada = Session["OTinsertada"]?.ToString();
            string PedidoInsertado = Session["PedidoInsertado"]?.ToString();

            if (!string.IsNullOrEmpty(OTinsertada) && !string.IsNullOrEmpty(PedidoInsertado))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Iterar sobre las listas y realizar inserciones
                    for (int i = 0; i < ID_Acabados.Count; i++)
                    {
                        string query = "INSERT INTO tblOTAcabados (Id_OT, Consecutivo_Pedido, id_Acabado, Id_GrupoObjetoParaAcabado, Detalle_Adicional, AcabadoVentas) " +
                                       "VALUES (@Id_OT, @Consecutivo_Pedido, @ID_Acabado, @ID_GrupoObjetoParaAcabado, @Detalle_Adicional, @AcabadoVentas)";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Id_OT", OTinsertada);
                        command.Parameters.AddWithValue("@Consecutivo_Pedido", PedidoInsertado);
                        command.Parameters.AddWithValue("@ID_Acabado", ID_Acabados[i]);
                        command.Parameters.AddWithValue("@ID_GrupoObjetoParaAcabado", ID_GruposObjetoparaAcabados[i]);
                        command.Parameters.AddWithValue("@Detalle_Adicional", Detalles_Adicionales[i]);
                        command.Parameters.AddWithValue("@AcabadoVentas", AcabadosVentas[i]);

                        int rowsAffected = command.ExecuteNonQuery();

                        MostrarModal();
                    }
                }
            }
            else
            {

            }

            Session.Remove("OTinsertada");
            Session.Remove("PedidoInsertado");
        }


        //TAB PLANO

        protected void BotonesPorDefectoPlano(object sender, EventArgs e)
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {

                BtnAdiObjPla,
                BtnQuiObjPla,
                BtnEliObjPla,
                BtnAcaPla,
                BtnLeeArcDesAca,
                BtnCarArcTxtXy,
                BtnPlaBlo,
                BtnCreRefBol,
                BtnAdiRemEleBol,
                BtnDesPla,
                BtnGenTxt,
                BtnGuaTxt,
                BtnExpPlaOrdTra,
                BtnVisGenCot,
                BtnObjNoExi,
                BtnImpPlaActBlo,
                BtnActPrePro,
                BtnGenForCerOrd,

            };

            string cssClass = "btn btn-sm shadow button-disabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;
                    linkButton.Enabled = false;
                    linkButton.CssClass = cssClass;
                }
            }

        }

        protected void BotonesPorDefectoObjetos(object sender, EventArgs e)
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {

                BtnNueObj,
                Btnnnn,
                BtnModObj,
                BtnConObj,
                BtnEliObj,
                BtnBusObj,
                BtnCopObj,
                BtnActPre,
                BtnChe,

            };

            string cssClass = "btn btn-sm shadow button-disabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;
                    linkButton.Enabled = false;
                    linkButton.CssClass = cssClass;
                }
            }

            BtnGenLisPre.Enabled = true;
            BtnGenLisPre.CssClass = "btn btn-sm shadow button-enabled";

            BtnIrObjAnt.Enabled = true;
            BtnIrObjAnt.CssClass = "btn btn-sm shadow button-enabled";

        }

        //TAB MODULOS
        protected void BotonesPorDefectoModulos(object sender, EventArgs e)
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {

                LinkButton1,
                LinkButton2,
                LinkButton3,
                LinkButton4,
                LinkButton5,
                LinkButton6,
                LinkButton7,
                LinkButton8

            };

            string cssClass = "btn btn-sm shadow button-disabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;
                    linkButton.Enabled = false;
                    linkButton.CssClass = cssClass;
                }
            }


        }

        //TAB INSUMOS

        protected void BotonesPorDefectoInsumos(object sender, EventArgs e)
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {
                LinkButton9,
                LinkButton10,
                LinkButton11,
                LinkButton12,
                LinkButton13,
                LinkButton14,
                LinkButton15
            };

            string cssClass = "btn btn-sm shadow button-disabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;
                    linkButton.Enabled = false;
                    linkButton.CssClass = cssClass;
                }
            }


        }


        // Plano  

        //***** Iinicio Boton Adicionar redireccion a Objetos Pendiente  Implementacion  *****
        protected void BtnAdiObjPla_Click(object sender, EventArgs e)
        {
            // Se valida  que el plano este o no bloqueado
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el plano está bloqueado
                string sSqlBloqueado = "SELECT Bloqueado FROM tblPlano WHERE Plano = @plano";
                SqlCommand commandBloqueado = new SqlCommand(sSqlBloqueado, connection);
                commandBloqueado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                bool planoBloqueado = (bool)commandBloqueado.ExecuteScalar();

                if (planoBloqueado)
                {
                    // El plano está bloqueado y no se puede eliminar
                    string scriptNoPermiso = "alert('El plano se encuentra bloqueado.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                }
                else
                {

                    // Consulta para verificar si el plano está vinculado a un pedido

                    string sSqlVinculado = "SELECT ID_OT, Consecutivo_Pedido,AfectaBolsa FROM tblPlano WHERE Plano = @plano";
                    SqlCommand commandVinculado = new SqlCommand(sSqlVinculado, connection);
                    commandVinculado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                    SqlDataReader reader = commandVinculado.ExecuteReader();

                    if (reader.Read())
                    {
                        // Verificar si el plano está vinculado a un pedido
                        string idOT = reader["ID_OT"].ToString();
                        string consecutivoPedido = reader["Consecutivo_Pedido"].ToString();
                        bool terminadoVentas = ConsultarTerminadoVenta(idOT, consecutivoPedido);
                        bool afectaBolsa = (bool)reader["AfectaBolsa"];

                        reader.Close();

                        if (idOT != "Nula" && terminadoVentas || afectaBolsa)
                        {
                            string mensajeExito = "No se puede Modificar Ningún Objeto, ya que el plano:  " + txtPlano.Text.Trim() + " esta vinculado a un pedido aprobado para producción o esta afectando a alguna bolsa.";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

                        }
                        else
                        {
                            Session["Id_OT2"] = tbOT.Text;
                            Session["pedido2"] = ddlNumbers.SelectedItem.Text;
                            Session["Numero_Plano"] = txtPlano.Text;

                            string url = "FormExtprin/Objetos.aspx";
                            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
                        }
                    }

                    reader.Close();
                }

            }

        }
        //***** Fin Boton Adicionar redireccion a Objetos Pendiente  Implementacion  *****


        //***** Iinicio Boton Quitar Objetos del plano Pendiente  Implementacion  *****
        protected void BtnQuiObjPla_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el plano está bloqueado
                string sSqlBloqueado = "SELECT Bloqueado FROM tblPlano WHERE Plano = @plano";
                SqlCommand commandBloqueado = new SqlCommand(sSqlBloqueado, connection);
                commandBloqueado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                bool planoBloqueado = (bool)commandBloqueado.ExecuteScalar();

                if (!planoBloqueado || Session["usuariologueado"].ToString() == txtDibuja.Text)
                {

                    // Consulta para verificar si el plano está vinculado a un pedido

                    string sSqlVinculado = "SELECT ID_OT, Consecutivo_Pedido,AfectaBolsa FROM tblPlano WHERE Plano = @plano";
                    SqlCommand commandVinculado = new SqlCommand(sSqlVinculado, connection);
                    commandVinculado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                    SqlDataReader reader = commandVinculado.ExecuteReader();

                    if (reader.Read())
                    {
                        // Verificar si el plano está vinculado a un pedido
                        string idOT = reader["ID_OT"].ToString();
                        string consecutivoPedido = reader["Consecutivo_Pedido"].ToString();
                        bool terminadoVentas = ConsultarTerminadoVenta(idOT, consecutivoPedido);
                        bool afectaBolsa = (bool)reader["AfectaBolsa"];

                        reader.Close();

                        if (idOT != "Nula" && terminadoVentas || afectaBolsa)
                        {
                            string mensajeExito = "No se puede Modificar Ningún Objeto, ya que el plano:  " + txtPlano.Text.Trim() + " esta vinculado a un pedido aprobado para producción o esta afectando a alguna bolsa";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                            BtnQuiObjPla.Enabled = false;
                            BtnQuiObjPla.CssClass = "btn btn-sm shadow button-disabled";

                        }
                        else
                        {


                            bool eliminacionExitosa = QuitarObjetoPlano(txtPlano.Text, Session["IdObjetoEliminarSession"].ToString());

                            if (eliminacionExitosa)
                            {
                                Cargar_Despiece_Plano();
                                string mensajeExito = "El objeto con ID " + Session["IdObjetoEliminarSession"].ToString() + " se eliminó correctamente del plano: " + txtPlano.Text;
                                string scriptExito = "alert('" + mensajeExito + "');";
                                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

                                BtnQuiObjPla.Enabled = false;
                                BtnQuiObjPla.CssClass = "btn btn-sm shadow button-disabled";
                            }
                            else
                            {
                                // La eliminación no fue exitosa, mostrar mensajes o tomar acciones adicionales
                                string mensajeError = "No se pudo eliminar el objeto del plano.";
                                ScriptManager.RegisterStartupScript(this, GetType(), "showError", $"alert('{mensajeError}');", true);
                            }


                        }
                    }

                    reader.Close();


                }
                else
                {

                    // El plano está bloqueado y no se puede eliminar
                    string scriptNoPermiso = "alert('El plano se encuentra bloqueado .');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                }
            }
        }
        private bool ConsultarTerminadoVenta(string IdOt, string pedido)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT Terminado_Ventas FROM tblOT WHERE Id_OT = @IdOT AND Consecutivo_Pedido = @Pedido";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdOT", IdOt);
                        command.Parameters.AddWithValue("@Pedido", pedido);

                        object result = command.ExecuteScalar();

                        // Si result no es nulo y es convertible a bool, entonces devuelve su valor
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToBoolean(result);
                        }
                        else
                        {
                            // Valor por defecto si no se encuentra un valor adecuado
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción si es necesario
                return false;
            }
        }
        private bool QuitarObjetoPlano(string plano, string idObjeto)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM tblPlano_Panel WHERE ID_Plano = @plano AND Id_PanelNum = @idObjeto";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@plano", plano);
                        command.Parameters.AddWithValue("@idObjeto", idObjeto);

                        int filasAfectadas = command.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        //***** Fin  Boton Quitar Objetos del plano Pendiente  Implementacion  *****


        //***** Iinicio Eliminar Objeto del plano Pendiente  Implementacion  *****     
        protected void BtnEliObjPla_Click(object sender, EventArgs e)
        {
            // Se valida  que el plano este o no bloqueado
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el plano está bloqueado
                string sSqlBloqueado = "SELECT Bloqueado FROM tblPlano WHERE Plano = @plano";
                SqlCommand commandBloqueado = new SqlCommand(sSqlBloqueado, connection);
                commandBloqueado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                bool planoBloqueado = (bool)commandBloqueado.ExecuteScalar();

                if (!planoBloqueado)
                {

                    // Consulta para verificar si el plano está vinculado a un pedido

                    string sSqlVinculado = "SELECT ID_OT, Consecutivo_Pedido,AfectaBolsa FROM tblPlano WHERE Plano = @plano";
                    SqlCommand commandVinculado = new SqlCommand(sSqlVinculado, connection);
                    commandVinculado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                    SqlDataReader reader = commandVinculado.ExecuteReader();

                    if (reader.Read())
                    {
                        // Verificar si el plano está vinculado a un pedido
                        string idOT = reader["ID_OT"].ToString();
                        string consecutivoPedido = reader["Consecutivo_Pedido"].ToString();
                        bool terminadoVentas = ConsultarTerminadoVenta(idOT, consecutivoPedido);
                        bool afectaBolsa = (bool)reader["AfectaBolsa"];

                        reader.Close();

                        if (idOT != "Nula" && terminadoVentas || afectaBolsa)
                        {
                            string mensajeExito = "No se puede Modificar Ningún Objeto, ya que el plano:  " + txtPlano.Text.Trim() + " esta vinculado a un pedido aprobado para producción o esta afectando a alguna bolsa";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

                        }
                        else
                        {


                            bool eliminacionExitosa = EliminarObjetosPlano(txtPlano.Text);

                            if (eliminacionExitosa)
                            {
                                Cargar_Despiece_Plano();
                                string mensajeExito = "Los objetos se han eliminado correectamente.";
                                string scriptExito = "alert('" + mensajeExito + "');";
                                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                            }
                            else
                            {
                                // La eliminación no fue exitosa, mostrar mensajes o tomar acciones adicionales
                                string mensajeError = "No se pudo eliminar los objeto del plano.";
                                ScriptManager.RegisterStartupScript(this, GetType(), "showError", $"alert('{mensajeError}');", true);
                            }


                        }

                    }

                    reader.Close();


                }
                else
                {
                    // El plano está bloqueado y no se puede eliminar
                    string scriptNoPermiso = "alert('El plano se encuentra bloqueado .');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);

                }

            }
        }
        private bool EliminarObjetosPlano(string plano)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM tblPlano_Panel WHERE ID_Plano = @plano ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@plano", plano);


                        int filasAfectadas = command.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        //***** Fin Eliminar Objeto del plano Pendiente  Implementacion  *****


        //***** Iinicio Boton Acabados  Pendiente  Implementacion  ******
        protected void BtnAcaPla_Click(object sender, EventArgs e)
        {

            if ((Session["Departamento"].ToString() == "Diseño" || Session["Departamento"].ToString() == "Ventas") && ((tbOT.Text != "" && btnOk.Enabled == true) || tbOT.Text == ""))
            {
                // Pendiente de realizar algunas actualizaciones !!!!! OJO validar !!!!!!!
            }

            string script = @"mostrarModal();";
            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", script, true);
        }
        protected void DataGridAcabadoVentas_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            DataGridItem row = DataGridAcabadoVentas.Items[rowIndex];

            // Se utiliza para darle el color solo a la fila seleccionada 
            foreach (DataGridItem item in DataGridAcabadoVentas.Items)
            {
                if (item != row)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            //se usa Para darle un color a la fila seleccionada  anderson
            e.Item.CssClass = "fila-seleccionada";

            Session["DescripAcabadoSession"] = row.Cells[3].Text;
            Session["DetalleAdicionalSession"] = row.Cells[4].Text;
            Session["DescripGrupoSession"] = row.Cells[5].Text;



            btnAgregarAcabado.Enabled = true;
            btnAgregarAcabado.CssClass = "btn btn-sm btn-outline-secondary";


            string script = @"mostrarModal();";
            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", script, true);

        }
        protected void btnAgregarAcabado_Click(object sender, EventArgs e)
        {
            if (AgregarAcabado())
            {

                // El usuario no tiene permisos para realizar la acción
                string scriptError = "alert('El acado no se puedo agregar con exito, intentelo nuevamente mas tarde.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptError, true);

                string script = @"mostrarModal();";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", script, true);
            }
            else
            {
                // El usuario no tiene permisos para realizar la acción
                DataGridAcabados1.DataBind();
                string scriptExito = "alert('El acabado fue agregado con exito');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptExito, true);

                string script = @"mostrarModal();";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", script, true);

            }

        }
        public bool AgregarAcabado()
        {

            string consultaActual = "INSERT INTO tblOTAcabadoDefinitivo (oadPLano,oadAplicacionAcabado,oadDescripcionAcabado,oadDescripcionGrupoObjeto)" +
                " VALUES (@plano,@AA, @DescripcionAcabado,@DescripcionGrupo)";

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(consultaActual, connection))
                {
                    command.Parameters.AddWithValue("@plano", txtPlano.Text);
                    command.Parameters.AddWithValue("@AA", "E");
                    command.Parameters.AddWithValue("@DescripcionAcabado", Session["DescripAcabadoSession"].ToString() + " - " + Session["DetalleAdicionalSession"].ToString());
                    command.Parameters.AddWithValue("@DescripcionGrupo", Session["DescripGrupoSession"].ToString());

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();

                        // Cerramos las variables de session 
                        Session.Remove("DescripAcabadoSession");
                        Session.Remove("DescripGrupoSession");
                        Session.Remove("DetalleAdicionalSession");

                        return true;


                    }
                    else
                    {
                        return false;
                    }
                }
            }

        } // Campo se podria Cargar en el login
        public void DepartamentoAsesor()
        {

            string consultaActual = "SELECT B.Descripcion FROM tblEmpleado As A INNER join tblDepartamento As B on B.ID_Departamento = A.Dependencia WHERE  Cedula = @Cedula";

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

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

        //***** Fin  Boton Acabados  Pendiente  Implementacion  ******


        //*****  Incio Boton Leer Autocad Pendiente Implementacion ******
        protected void BtnLeeArcDesAca_Click(object sender, EventArgs e)
        {

            //Variables de Session para volver a cargar el plano
            Session["Id_OT2"] = tbOT.Text;
            Session["pedido2"] = ddlNumbers.Text;

            // Consultamos que el plano no este bloqueado o ya este ligado a un pedido o afecta alguna bolsa 

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSqlBloqueado = "SELECT Bloqueado FROM tblPlano WHERE Plano = @plano";
                SqlCommand commandBloqueado = new SqlCommand(sSqlBloqueado, connection);
                commandBloqueado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                bool planoBloqueado = (bool)commandBloqueado.ExecuteScalar();

                if (!planoBloqueado || txtDibuja.Text == Session["usuariologueado"].ToString())
                {

                    string sSqlVinculado = "SELECT ID_OT, Consecutivo_Pedido,AfectaBolsa FROM tblPlano WHERE Plano = @plano";
                    SqlCommand commandVinculado = new SqlCommand(sSqlVinculado, connection);
                    commandVinculado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                    SqlDataReader reader = commandVinculado.ExecuteReader();

                    if (reader.Read())
                    {
                        string idOT = reader["ID_OT"].ToString();
                        string consecutivoPedido = reader["Consecutivo_Pedido"].ToString();
                        bool terminadoVentas = ConsultarTerminadoVenta(idOT, consecutivoPedido);
                        bool afectaBolsa = (bool)reader["AfectaBolsa"];

                        reader.Close();

                        if (idOT != "Nula" && terminadoVentas || afectaBolsa)
                        {
                            string mensajeExito = "No se puede Modificar Ningún Objeto, ya que el plano:  " + txtPlano.Text.Trim() + " esta vinculado a un pedido aprobado para producción o esta afectando a alguna bolsa";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

                        }
                        else
                        {
                            if (ValidarPLanoBolsa())
                            {
                                if (!PermisoModfiicarBolsa())
                                {
                                    string scriptNoPermiso = "alert('No tiene permiso para modificar una bolsa que ha tenido descargas.');";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showPermisoBolsa", scriptNoPermiso, true);
                                }

                            }
                            //Modal para Cargar el archivo tXT
                            string script = @"mostrarModalArchivo();";
                            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalArchivo", script, true);
                        }

                    }

                    reader.Close();

                }
                else
                {
                    string scriptNoPermiso = "alert('El plano se encuentra bloqueado .');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                }

            }
        }
        protected void btnCargarAcad_Click(object sender, EventArgs e)
        {


            if (LeerAcad.HasFile)
            {
                string fileName = LeerAcad.FileName;
                string fileExtension = Path.GetExtension(fileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                if (fileNameWithoutExtension != txtPlano.Text)
                {
                    string mensajePersonalizado = "No coinciden los nombres de los planos. No se Puede Cargar.";
                    string urlRedireccion = "OrdenTrabajo.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                    switch (fileExtension)
                    {
                        case ".txt":
                            BorrarPenelesDelPlano();
                            DataTable anchosViejos = AnchosViejos();
                            DataTable Paneles;

                            using (StreamReader reader = new StreamReader(LeerAcad.PostedFile.InputStream))
                            {
                                // Leemos cada línea del archivo TXT 
                                while (!reader.EndOfStream)
                                {
                                    bool VarControl = false;
                                    int idObjeto;
                                    bool Desmonte;
                                    bool reinstalacion;
                                    bool Escalable;
                                    int precioVenta = 0;
                                    string ancho;
                                    float Ancho = 0; // Inicializar Ancho aquí
                                    string linea = reader.ReadLine();
                                    string Objeto = linea.Substring(0, Math.Min(50, linea.Length));
                                    DataRow[] FilasEncontradas = anchosViejos.Select("Id_Panel = '" + Objeto + "'");

                                    if (FilasEncontradas.Length > 0)
                                    {
                                        ancho = FilasEncontradas[0]["AnchoNew"].ToString().Replace(",", ".");
                                        if (float.TryParse(ancho, out Ancho))
                                        {
                                            Ancho /= 100;
                                        }
                                        else
                                        {
                                            string mensajePersonalizado3 = "El archivo no es compatible con el formato. Causas: \\n 1. El archivo fue Manipulado o Modificado. \\n 2. El archivo no fue generado con el formato preestablecido en AutoCad  ";
                                            string urlRedireccion3 = "OrdenTrabajo.aspx";
                                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado3)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion3)}");
                                        }
                                    }
                                    else
                                    {
                                        ancho = linea.Substring(50, 50).Trim().Replace(",", ".");
                                        if (float.TryParse(ancho, out Ancho))
                                        {

                                        }
                                        else
                                        {
                                            string mensajePersonalizado3 = "El archivo no es compatible con el formato. Causas: \\n 1. El archivo fue Manipulado o Modificado. \\n 2. El archivo no fue generado con el formato preestablecido en AutoCad  ";
                                            string urlRedireccion3 = "OrdenTrabajo.aspx";
                                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado3)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion3)}");
                                        }

                                    }

                                    Desmonte = (linea.Substring(100, 10).Trim()).Contains("1");
                                    reinstalacion = (linea.Substring(110, 10).Trim()).Contains("1");

                                    do
                                    {
                                        Paneles = ConsultarObjeto(Objeto);

                                        if (Paneles.Rows.Count > 0)
                                        {
                                            Escalable = Convert.ToBoolean(Paneles.Rows[0]["Escalable"].ToString());
                                            Ancho *= 100;
                                            DataRow[] PanelAncho = Paneles.Select("Ancho = '" + Ancho + "'");

                                            if (PanelAncho.Length <= 0)
                                            {
                                                if (Escalable)
                                                {
                                                    // Se crea el Objeto a la Medida requerida, siempre y cuando sea escalable
                                                    CreacionObjPersonalizado(Objeto, Ancho, Paneles);
                                                    // se busca el idNumerico del objeto que se acaba de crear 
                                                    idObjeto = Consultar_Id_Numerico(Objeto, Ancho);
                                                    CalcularPrecioVenta(idObjeto, out precioVenta);

                                                }
                                                else
                                                {
                                                    //Se consultan los anchos viejos 
                                                    ConsultarAnchosViejos(Objeto, Ancho);
                                                    if (Ancho == 0)
                                                    {
                                                        // Si ancho es igual a cero no existe en anchos viejos  se agrega al Datagrid de no existentes
                                                        // Con la observacion que no es escalable
                                                        // InsertarNoExistentes();
                                                    }

                                                }
                                            }

                                        }
                                        else
                                        {
                                            // se agrega al Datagrid de no existentes
                                            // con la observacion que el elemento no existe 
                                            //InsertarNoExistentes();
                                        }

                                        idObjeto = Consultar_Id_Numerico(Objeto, Ancho);
                                        int cantidad = ObjetoIncluidoPlano(txtPlano.Text, idObjeto);
                                        //Insertar  los datos al plano , Calcular precio de venta
                                        if (cantidad > 0)
                                        {
                                            if (Objeto.Substring(0, 3).ToUpper() == "DMS")
                                            {
                                                // Se revisa si es un Desmonte o si es una Reinstalacion
                                                if (Desmonte)
                                                {
                                                    ActualizarCantidad(idObjeto, cantidad);
                                                }

                                                if (reinstalacion)
                                                {
                                                    //Pendiente validar que hacer ya que tenia una etiqueta que llebava atras 
                                                }

                                            }
                                            else
                                            {
                                                //Actualizamos la Cantidad ya que esta en el plano 
                                                ActualizarCantidad(idObjeto, cantidad);
                                            }
                                        }
                                        else
                                        {
                                            // Como el elemento no esta en el plano se revisa primero el valor del precio de venta y se calcula
                                            var resultado = Paneles.AsEnumerable().Where(row => Convert.ToInt32(row["Id_Numerico"]) == idObjeto &&
                                                            Convert.ToDouble(row["Ancho"]) == Ancho)
                                                            .Select(row => Convert.ToInt32(row["Precio_Venta"]))
                                                           .FirstOrDefault();


                                            if (resultado == 0)
                                            {
                                                CalcularPrecioVenta(idObjeto, out precioVenta);
                                            }
                                            else
                                            {
                                                precioVenta = resultado;
                                            }
                                            // se Valida si el objeto empieza por EX y si es asi se inserta en el plano
                                            if (Objeto.Substring(0, 3).ToUpper() == "EX")
                                            {
                                                bool Existentes = false;
                                                if (Existentes)
                                                {
                                                    // Realzar insercion en el plano
                                                }

                                            }
                                            else
                                            {
                                                // Se valida si el plano empieza por DSM y se valida si es un Desmonte o una reintalacion
                                                if (Objeto.Substring(0, 3).ToUpper() == "DMS")
                                                {
                                                    if (Desmonte)
                                                    {
                                                        //Insertar Plano
                                                    }
                                                    if (reinstalacion && !VarControl)
                                                    {
                                                        //Se debe cambiar en el nombre de bloque el DSM por
                                                        //REINST y hacer el proceso desde consultar paneles nuevamente con el objeto diferente 
                                                        Objeto = Objeto.Replace("DMS", "REINST");
                                                        VarControl = true;
                                                    }

                                                }
                                                else
                                                {
                                                    VarControl = false;
                                                    InsertarObjetoPlano(idObjeto, cantidad, "", resultado);
                                                    //Realizar la insercion al plano 
                                                }
                                            }


                                        }
                                    } while (VarControl == true);



                                }

                                //Se valida  si hay datos en el panel con numero de plano y si hay se actualiza el precio 
                                // Recorrer los datos y cargar  CalcularPrecioVenta(idObjeto)

                            }

                            break;

                        case ".xls":
                            // Pendiente para Cuando se este trabajando para dibujo 
                            break;

                        default:
                            string mensajePersonalizado4 = "La extención del archivo no es permitida";
                            string urlRedireccion4 = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado4)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion4)}");
                            break;
                    }

                    //Si el plano es una Bolsa 
                    if (txtPlano.Text.ToUpper().Substring(0, 3) == "BSA" && txtPlano.Text.Contains("_") && txtPlano.Text.Length > 10)
                    {
                        string OTBolsa = txtPlano.Text.ToUpper().Substring(3, txtPlano.Text.ToUpper().IndexOf("-", 3) - 3);
                        string PedidoBolsa = txtPlano.Text.ToUpper().Substring(txtPlano.Text.ToUpper().IndexOf("-", 2) + 1);

                        if (OTBolsa != "" && PedidoBolsa != "")
                        {
                            if (!PedidoFacturable(OTBolsa, PedidoBolsa))
                            {
                                string mensajePersonalizado5 = "El pedido: " + OTBolsa + "-" + PedidoBolsa + ", No existe pedido o no es un pedido facturable";
                                string urlRedireccion5 = "OrdenTrabajo.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado5)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion5)}");

                            }
                            else
                            {
                                DataTable ResumenPed = ResumenPedido(txtPlano.Text);
                                ActulizarTblOTBolsa(txtPlano.Text);

                                foreach (DataRow row in ResumenPed.Rows)
                                {
                                    if (!ConsultarBolsaMetodo(txtPlano.Text, row["id_GrupoObjeto"].ToString()))
                                    {
                                        //Insertar                                                    
                                        InsertarTblOtBolsa(txtPlano.Text, OTBolsa, PedidoBolsa, row);
                                    }
                                    else
                                    {
                                        //Actualizar
                                        ActualizarTblOtBolsa(txtPlano.Text, row);
                                    }
                                }
                            }

                        }

                    }

                    //Actualizar Fecha Lectura Despiece 
                    ActualizarFechaLecturaDespiece(txtPlano.Text);

                    string mensajePersonalizado = "TXT  Cargado.";
                    string urlRedireccion = "OrdenTrabajo.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                }

            }
            else
            {
                string mensajePersonalizado = "Por favor seleccione un archivo para cargar.";
                string urlRedireccion = "OrdenTrabajo.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
        }
        private bool ValidarPLanoBolsa()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT tblOTBolsa.otbolBolsa, tblOTBolsa.otbolCantidadPedida FROM tblOTBolsa WHERE tblOTBolsa.otbolBolsa = @plano AND tblOTBolsa.otbolCantidadPedida > 0";
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);
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
        private bool PermisoModfiicarBolsa()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = '" + Session["CedulaLogeada"].ToString() + "' AND ID_Permiso = 39";
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        // La consulta devolvió registros, lo cual significa que el usuario tiene permisos
                        return true;
                    }
                    else
                    {
                        // La consulta no devolvió registros, lo cual significa que el usuario no tiene permisos se valida si el es dueño del plano
                        if (txtDibuja.Text == Session["usuariologueado"].ToString())
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
        private void BorrarPenelesDelPlano()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "Delete  from tblPlano_Panel where ID_Plano= @plano";
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);
                    SqlDataReader reader = cmd.ExecuteReader();
                }
            }
        }
        private DataTable AnchosViejos()
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM tblPanelAnchoOld";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
        private DataTable ConsultarObjeto(string id)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tblPanel WHERE Id_Panel = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar el parámetro Id
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        private bool CreacionObjPersonalizado(string objeto, float ancho, DataTable panel)
        {
            double altura = Convert.ToDouble(panel.Rows[0]["Altura"].ToString());
            double profundidad = Convert.ToDouble(panel.Rows[0]["profundidad"].ToString());
            double Cubicaje = Math.Round((ancho * altura * profundidad) / 1000000, 5);

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string sSql = "INSERT INTO tblPanel " +
                          "(Id_Panel, Ancho, Descripcion_Panel, Id_GrupoObjeto, Altura, " +
                          "Id_linea, Divisiones, Holgura, Escalable, profundidad, CubicajeM3, " +
                          "Chequeado, Responsable, FechaChequeo, UndxPaquete, Descripcion_Tecnica) " +
                          "VALUES (@Id_Panel, @Ancho, @Descripcion_Panel, @Id_GrupoObjeto, @Altura, " +
                          "@Id_linea, @Divisiones, @Holgura, @Escalable, @profundidad, @CubicajeM3, " +
                          "@Chequeado, @Responsable, @FechaChequeo, @UndxPaquete, @Descripcion_Tecnica)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    // Aquí ajusta los valores según los tipos de datos reales de tu base de datos
                    cmd.Parameters.AddWithValue("@Id_Panel", objeto);
                    cmd.Parameters.AddWithValue("@Ancho", ancho);
                    cmd.Parameters.AddWithValue("@Descripcion_Panel", panel.Rows[0]["Descripcion_Panel"].ToString());
                    cmd.Parameters.AddWithValue("@Id_GrupoObjeto", panel.Rows[0]["Id_GrupoObjeto"].ToString());
                    cmd.Parameters.AddWithValue("@Altura", altura);
                    cmd.Parameters.AddWithValue("@Id_linea", Convert.ToInt16(panel.Rows[0]["Id_linea"].ToString()));
                    cmd.Parameters.AddWithValue("@Divisiones", Convert.ToInt16(panel.Rows[0]["Divisiones"].ToString()));
                    cmd.Parameters.AddWithValue("@Holgura", Convert.ToDouble(panel.Rows[0]["Holgura"].ToString()));
                    cmd.Parameters.AddWithValue("@Escalable", Convert.ToBoolean(panel.Rows[0]["Escalable"].ToString()));
                    cmd.Parameters.AddWithValue("@profundidad", Convert.ToDouble(panel.Rows[0]["profundidad"].ToString()));
                    cmd.Parameters.AddWithValue("@CubicajeM3", Cubicaje);
                    cmd.Parameters.AddWithValue("@Chequeado", Convert.ToBoolean(panel.Rows[0]["Chequeado"].ToString()));
                    cmd.Parameters.AddWithValue("@Responsable", panel.Rows[0]["Responsable"].ToString());
                    cmd.Parameters.AddWithValue("@FechaChequeo", Convert.ToDateTime(panel.Rows[0]["FechaChequeo"].ToString()));
                    cmd.Parameters.AddWithValue("@UndxPaquete", Convert.ToInt32(panel.Rows[0]["UndxPaquete"]));
                    cmd.Parameters.AddWithValue("@Descripcion_Tecnica", panel.Rows[0]["Descripcion_Tecnica"].ToString());

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        private int Consultar_Id_Numerico(string objeto, float ancho)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string sSql = "SELECT Id_Numerico FROM tblPanel WHERE Id_Panel = @Id_Panel AND Ancho = @Ancho";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@Id_Panel", objeto);
                    cmd.Parameters.AddWithValue("@Ancho", ancho);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            //Este es el id del objeto que acabamos de agregar 
                            int idPanel = reader.GetInt32(0);
                            // Se consulta los datos de los modulos  con ese idPanel
                            ConsultarDatosModuloPanel(idPanel, objeto);
                            return idPanel;
                        }
                        else
                        {
                            return 0; // No hay datos, puedes manejar esto de acuerdo a tus necesidades
                        }
                    }
                }
            }
        }
        private void ConsultarDatosModuloPanel(int IdNumerico, string objeto)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string sSql = "Select * from tblPanel_Modulo where Id_PanelNum = @Id_Numerico";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Id_Numerico", IdNumerico);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Si hay Datos es por que el objeto ya estaba en un modulo 

                        }
                        else
                        {
                            // si no hay datos consultamos los valores con el IdPanel   
                            DataTable obj = ConsultarObjeto(objeto);

                            //Tomamos el primer registro 
                            object primerDato = obj.Rows[0][0];

                            // Seleecionamos el id de ese registro  
                            string id = primerDato.ToString();
                            // con ese id se consultan los datos del modulo 
                            DataTable modulo = Consultarmodulo(id);

                            //Con los datos del modulo y el IdNumerico del elemento a agregar realizamos la insercion 
                            AgregarModuloPanel(IdNumerico, modulo);
                            // Calcular_Precio_Venta_Objeto(IdNumerico, precioVenta);
                        }
                    }

                }
            }
        }
        private DataTable Consultarmodulo(string id)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "Select * from tblPanel_Modulo where Id_PanelNum = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar el parámetro Id
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        private void AgregarModuloPanel(int id, DataTable modulo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string sSql = "INSERT INTO tblPanel_Modulo(Id_PanelNum,Id_Modulo,Ubicacion_Modulo,Lado,Cantidad,Observaciones,PanModResponsable,FechaConfiguracion) VALUES" +
                "(@Id_Numerico, @ID_Modulo, @Ubicacion_Modulo,@Lado,@Cantidad,@Observaciones,@PanModResponsable,@FechaConfiguracion )";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataRow row in modulo.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand(sSql, connection))
                    {
                        // Aquí ajusta los valores según los nombres de columnas reales en tu DataTable
                        cmd.Parameters.AddWithValue("@Id_Numerico", id);
                        cmd.Parameters.AddWithValue("@ID_Modulo", row["Id_Modulo"]);
                        cmd.Parameters.AddWithValue("@Ubicacion_Modulo", row["Ubicacion_Modulo"]);
                        cmd.Parameters.AddWithValue("@Lado", row["Lado"]);
                        cmd.Parameters.AddWithValue("@Cantidad", row["Cantidad"]);
                        cmd.Parameters.AddWithValue("@Observaciones", row["Observaciones"]);
                        cmd.Parameters.AddWithValue("@PanModResponsable", row["PanModResponsable"]);
                        cmd.Parameters.AddWithValue("@FechaConfiguracion", row["FechaConfiguracion"]);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public void CalcularPrecioVenta(int idPanelNumerico, out int precioVenta)
        {
            precioVenta = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Ejecutar el procedimiento almacenado para actualizar el precio del objeto
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarPrecioObjeto", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Objeto", idPanelNumerico);
                    cmd.ExecuteNonQuery();
                }

                // Consultar la información actualizada del objeto
                string selectQuery = "SELECT Precio_Venta FROM tblPanel WHERE Id_Numerico = @ID_panelNumerico";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@ID_panelNumerico", idPanelNumerico);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener los valores del precio venta
                            precioVenta = Convert.ToInt32(reader["Precio_Venta"]);

                        }
                    }
                }
            }
        }
        private double ConsultarAnchosViejos(string objeto, double ancho)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "select * from tblPanelAnchoOld where Id_Panel =@Objeto and ancho =@ancho";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@Objeto", objeto);
                    cmd.Parameters.AddWithValue("@ancho", ancho);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        ancho = Convert.ToDouble(reader["AnchoNew"].ToString());
                    }
                    else
                    {
                        ancho = 0;
                    }

                    return ancho;

                }

            }
        }
        private int ObjetoIncluidoPlano(string plano, int idObjeto)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int cantidad = 0;
                string sSql = "SELECT cantidad FROM tblPlano_Panel WHERE Id_Plano = @plano AND Id_PanelNum = @id";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", plano);
                    cmd.Parameters.AddWithValue("@id", idObjeto);

                    // Utiliza ExecuteScalar para obtener un solo valor
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cantidad = Convert.ToInt32(result);
                    }
                    return cantidad;
                }
            }
        }
        private void ActualizarFechaLecturaDespiece(string plano)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "Update tblPlano set PlaFechalecturaDespiece=Getdate() where Plano= @plano";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", plano.ToUpper());
                    cmd.ExecuteNonQuery();
                }

            }

        }
        private void ActualizarCantidad(int idNumerico, int cantidad)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "Update tblPLano_Panel set Cantidad= @cantidad where Id_Plano= @plano and Id_PanelNum = @idNumerico ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad + 1);
                    cmd.Parameters.AddWithValue("@idNumerico", idNumerico);

                    cmd.ExecuteNonQuery();
                }

            }
        }
        private void InsertarObjetoPlano(int idNumerico, int cantidad, string observaciones, int precioventa)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblPlano_Panel(Id_Plano,Id_Panelnum,Cantidad,Observaciones,Precio_Venta)" +
                    "Values (@plano, @IdNumerico, @Cantidad, @Observaciones,@Precio_Venta) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);
                    cmd.Parameters.AddWithValue("@idNumerico", idNumerico);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@Observaciones", observaciones);
                    cmd.Parameters.AddWithValue("@idNumerico", precioventa);

                    cmd.ExecuteNonQuery();
                }

            }
        }

        // Metodos para cuando el plano es una Bolsa 
        private bool PedidoFacturable(string IdOT, string pedido)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT tblTipoPedido.EstadisticaVenta, tblOT.Id_OT, tblOT.Consecutivo_Pedido " +
                              "GROUP BY tblTipoPedido.EstadisticaVenta, tblOT.Id_OT, tblOT.Consecutivo_Pedido " +
                              "HAVING (((tblTipoPedido.EstadisticaVenta)=1) AND ((tblOT.Id_OT)= @IdOT ) AND ((tblOT.Consecutivo_Pedido)= @pedido )) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", IdOT);
                    cmd.Parameters.AddWithValue("@pedido", pedido);
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
        private DataTable ResumenPedido(string plano)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT tblPlano_Panel.Id_Plano,tblGrupoObjeto.ID_GrupoObjeto,tblGrupoObjeto.Descripcion_Grupo," +
                                "Sum(tblPlano_Panel.Cantidad) AS Cantidad,Sum(tblPlano_Panel.Cantidad*tblPlano_Panel.Precio_Venta) AS SubTotal," +
                                "tblGrupoObjeto.GOBloqueaPedido FROM (tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto) " +
                                "INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum " +
                                "GROUP BY tblPlano_Panel.Id_Plano, tblGrupoObjeto.ID_GrupoObjeto, tblGrupoObjeto.Descripcion_Grupo, tblGrupoObjeto.GOBloqueaPedido, tblGrupoObjeto.Cotizar " +
                                "HAVING (((tblPlano_Panel.Id_Plano)='@plano') AND ((tblGrupoObjeto.Cotizar)=1)) ORDER BY tblGrupoObjeto.Descripcion_Grupo";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar el parámetro Id
                    command.Parameters.AddWithValue("@plano", id);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        private void ActulizarTblOTBolsa(string plano)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "update tblOTBolsa set otbolCantidadCotizada=0, otbolValorCotizado=0 where OTBolBolsa= @plano";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", plano.ToUpper());
                    cmd.ExecuteNonQuery();
                }

            }

        }
        private bool ConsultarBolsaMetodo(string plano, string id_GrupoObjeto)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "Select * from tblOTBolsa where OTBolBolsa= @plano and otbolIDGrupoObjeto= @idGrupoObjeto";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", plano.ToUpper());
                    cmd.Parameters.AddWithValue("@idGrupoObjeto", id_GrupoObjeto);
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
        private void InsertarTblOtBolsa(string plano, string IdOt, string pedido, DataRow fila)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string sSql = "INSERT INTO tblOtBolsa(otbolBolsa,otbolId_OT,otbolPedido,otbolIDGrupoObjeto,otbolGrupoObjeto,otbolCantidadCotizada,otbolValorCotizado,otbolCantidadPedida,otbolBloqueaPedido) " +
                "VALUES (@plano,@IdOT,@pedido,@id_GrupoObjeto,@Descripcion_Grupo,@Cantidad,@SubTotal,@CantidadPedida,@GOBloqueaPedido)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@plano", plano);
                    cmd.Parameters.AddWithValue("@IdOT", IdOt);
                    cmd.Parameters.AddWithValue("@pedido", Convert.ToInt16(pedido));
                    cmd.Parameters.AddWithValue("@id_GrupoObjeto", Convert.ToInt32(fila["id_GrupoObjeto"].ToString()));
                    cmd.Parameters.AddWithValue("@Descripcion_Grupo", fila["Descripcion_Grupo"].ToString());
                    cmd.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(fila["Cantidad"].ToString()));
                    cmd.Parameters.AddWithValue("@SubTotal", Convert.ToInt32(fila["SubTotal"].ToString()));
                    cmd.Parameters.AddWithValue("@CantidadPedida", "0");
                    cmd.Parameters.AddWithValue("@GOBloqueaPedido", Convert.ToBoolean(fila["GOBloqueaPedido"].ToString()));

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void ActualizarTblOtBolsa(string plano, DataRow fila)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string sSql = "update tblOTBolsa set otbolCantidadCotizada= @Cantidad, otbolValorCotizado = @SubTotal WHERE OTBolBolsa= @plano AND otbolIDGrupoObjeto= @id_GrupoObjeto  ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@plano", plano);
                    cmd.Parameters.AddWithValue("@id_GrupoObjeto", Convert.ToInt32(fila["id_GrupoObjeto"].ToString()));
                    cmd.Parameters.AddWithValue("@Cantidad", Convert.ToInt32(fila["Cantidad"].ToString()));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //*****  Fin Boton Leer Autocad Pendiente Implementacion ******



        //*****  Inicio Boton Leer Archivo XY Pendiente Implementacion ******
        protected void BtnCarArcTxtXy_Click(object sender, EventArgs e)
        {
            // Se valida que el plano este o no bloqueado
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string sSqlBloqueado = "SELECT Bloqueado FROM tblPlano WHERE Plano = @plano";
                SqlCommand commandBloqueado = new SqlCommand(sSqlBloqueado, connection);
                commandBloqueado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                bool planoBloqueado = (bool)commandBloqueado.ExecuteScalar();

                if (planoBloqueado)
                {

                    string scriptNoPermiso = "alert('El plano se encuentra bloqueado .');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                }
                else
                {


                    string sSqlVinculado = "SELECT ID_OT, Consecutivo_Pedido,AfectaBolsa FROM tblPlano WHERE Plano = @plano";
                    SqlCommand commandVinculado = new SqlCommand(sSqlVinculado, connection);
                    commandVinculado.Parameters.AddWithValue("@plano", txtPlano.Text.Trim());

                    SqlDataReader reader = commandVinculado.ExecuteReader();

                    if (reader.Read())
                    {

                        string idOT = reader["ID_OT"].ToString();
                        string consecutivoPedido = reader["Consecutivo_Pedido"].ToString();
                        bool afectaBolsa = (bool)reader["AfectaBolsa"];

                        reader.Close();

                        if (idOT == "Nula" && !afectaBolsa)
                        {
                            // No está vinculado a ningún pedido y no afecta ninguna bolsa Cargar_Arch_Acad_XY 

                        }
                        else
                        {
                            string mensajeExito = "No se puede Modificar Ningún Objeto, ya que el plano:  " + txtPlano.Text.Trim() + " esta vinculado a un pedido aprobado para producción o esta afectando a alguna bolsa";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                        }
                    }

                    reader.Close();
                }

            }
        }

        //*****  Fin Boton Leer Archivo XY Pendiente Implementacion ******



        //*****  Inicio Boton Bloquear Desbloquear Plano  Pendiente Implementacion ******
        protected void BtnPlaBlo_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSql = "SELECT Bloqueado FROM tblPlano WHERE Plano ='" + txtPlano.Text.Trim() + "'";
                SqlCommand command = new SqlCommand(sSql, connection);
                SqlDataReader reader = null;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    bool bloqueado = reader.GetBoolean(reader.GetOrdinal("Bloqueado"));

                    // Ahora, puedes realizar acciones dependiendo de si el campo Bloqueado es verdadero o falso
                    if (bloqueado)
                    {
                        // Validar si el usuario tiene el permiso para bloquear o desbloquear plano o si es el dueño 
                        if (UsuarioTienePermiso())
                        {
                            // Cambiar en campo bloqueado en la base de datos a 0
                            ActualizarEstadoBloqueado(0);
                            string scriptNoPermiso = "alert('El plano ha sido desbloqueado');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }
                        else
                        {
                            // El usuario no tiene permisos para realizar la acción
                            string scriptNoPermiso = "alert('No tienes permisos para desbloquear o bloquear el plano.');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }



                    }
                    else
                    {

                        // Validar si el usuario tiene el permiso para bloquear o desbloquear plano o si es el dueño 
                        if (UsuarioTienePermiso())
                        {
                            // Cambiar en campo bloqueado en la base de datos a 1
                            ActualizarEstadoBloqueado(1);
                            string scriptNoPermiso = "alert('El plano ha sido bloqueado');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }
                        else
                        {
                            // El usuario no tiene permisos para realizar la acción
                            string scriptNoPermiso = "alert('No tienes permisos para desbloquear o bloquear el plano.');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }

                    }


                }

                reader.Close();

            }
        }
        private bool UsuarioTienePermiso()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = '" + Session["CedulaLogeada"].ToString() + "' AND ID_Permiso = 43";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // La consulta devolvió registros, lo cual significa que el usuario tiene permisos
                        return true;
                    }
                    else
                    {
                        // La consulta no devolvió registros, lo cual significa que el usuario no tiene permisos se valida si el es dueño del plano
                        if (txtDibuja.Text == Session["usuariologueado"].ToString())
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
        private void ActualizarEstadoBloqueado(int nuevoEstado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSql = "UPDATE tblPlano SET Bloqueado = @NuevoEstado WHERE Plano = @Plano";
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@Plano", txtPlano.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //*****  Fin Boton Bloquear Desbloquear Plano  Pendiente Implementacion ******
        protected void BtnVisGenCot_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos un instancia de excel 
                var excelApp = new Excel.Application();

                //Validamos que excel este instalada en el equipo 
                if (excelApp == null)
                {
                    Console.WriteLine("Excel no está instalado en esta máquina.");
                    return;
                }

                // Creamos un nuevo libro excel 
                var workbook = excelApp.Workbooks.Add();

                // Creamos una nueva        Hoja Cotizacion 
                var worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                worksheet.Name = "Cotizacion";
           
                // Altura de la Fila 1
                worksheet.Rows[1].RowHeight = 60;

                // Fecha del dia 
                DateTime fechaActual = DateTime.Now;
                string nombreMes = fechaActual.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                int diaActual = fechaActual.Day;
                int añoActual = fechaActual.Year;

                // Logo Ducon 
                var imagen = worksheet.Range["B1"];
                imagen.Value = "";
                imagen.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                imagen.EntireRow.Font.Color = System.Drawing.Color.Black;

                string rutaImagen = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                worksheet.Shapes.AddPicture(rutaImagen,
                    Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue,
                    imagen.Left, imagen.Top, 140, 40);


                // Sede y Fecha del dia 
                string tableTitle = "Sabaneta, " + nombreMes + " " + diaActual + " de " + añoActual;
                var titleRange = worksheet.Range["B2"];
                titleRange.Value = tableTitle;
                titleRange.Font.Name = "Century Gothic";
                titleRange.Font.Size = 11;
                titleRange.Font.Bold = true;
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                titleRange.EntireRow.Font.Color = System.Drawing.Color.Black;

                // Cotizacion 
                string titleCot = "Cotización Nº";
                var Cell = worksheet.Range["E1"];
                Cell.Value = titleCot;
                Cell.Font.Name = "Century Gothic";
                Cell.Font.Size = 11;
                Cell.Font.Bold = true;
                Cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                Cell.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellE2 = worksheet.Range["E2"];
                CellE2.Value = txtPlano.Text;
                CellE2.Font.Name = "Century Gothic";
                CellE2.Font.Size = 11;
                CellE2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                CellE2.EntireRow.Font.Color = System.Drawing.Color.Black;

                // Dirigido a
                var CellB4 = worksheet.Range["B4"];
                CellB4.Value = "Señores";
                CellB4.Font.Name = "Century Gothic";
                CellB4.Font.Size = 11;
                CellB4.Font.Bold = true;
                CellB4.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB4.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellB5 = worksheet.Range["B5"];
                CellB5.Value = txtContactoPlano.Text;
                CellB5.Font.Name = "Century Gothic";
                CellB5.Font.Size = 11;
                CellB5.Font.Bold = true;
                CellB5.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB5.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellB6 = worksheet.Range["B6"];
                CellB6.Value = txtCliente.Text;
                CellB6.Font.Name = "Century Gothic";
                CellB6.Font.Size = 11;
                CellB6.Font.Bold = true;
                CellB6.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB6.EntireRow.Font.Color = System.Drawing.Color.Black;

                // Ciudad 
                var CellB7 = worksheet.Range["B7"];
                CellB7.Value = "Ciudad";
                CellB7.Font.Name = "Century Gothic";
                CellB7.Font.Size = 11;
                CellB7.Font.Bold = true;
                CellB7.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB7.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellB9 = worksheet.Range["B9"];
                CellB9.Value = "Ref. " + txtArea.Text;
                CellB9.Font.Name = "Century Gothic";
                CellB9.Font.Size = 11;
                CellB9.Font.Bold = true;
                CellB9.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB9.EntireRow.Font.Color = System.Drawing.Color.Black;

                // Saludos 
                var CellB11 = worksheet.Range["B11", "G11"];
                CellB11.Merge();
                CellB11.Value = "Atendiendo su amable solicitud con gusto presentamos cotización de las partes y ";
                CellB11.Font.Name = "Century Gothic";
                CellB11.Font.Size = 11;
                CellB11.Font.Bold = true;
                CellB11.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB11.EntireRow.Font.Color = System.Drawing.Color.Black;


                var CellB12 = worksheet.Range["B12", "G12"];
                CellB12.Merge();
                CellB12.Value = "elementos del  sistema  Modular  Ducon SMD, en nuestra línea 3500, tal como sigue ";
                CellB12.Font.Name = "Century Gothic";
                CellB12.Font.Size = 11;
                CellB12.Font.Bold = true;
                CellB12.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB12.EntireRow.Font.Color = System.Drawing.Color.Black;

                // Comienzo de encabezados 
                int headerIndex = 2;
                int rowIndex = 14;

                // Encabezados  de la tabal del despiece del plano 
                string[] encabezados = { "DESCRIPCIÓN", "Ancho (Cms)", "CANT     ", "Valor Und    ", "Total        ", "Imagen             ","","", "Cubicaje", "Peso" };

                foreach (string encabezado in encabezados)
                {

                    worksheet.Cells[rowIndex, headerIndex] = encabezado;

                    var headerCell = (Excel.Range)worksheet.Cells[rowIndex, headerIndex];
                    headerCell.Font.Bold = true;  // Establecer el texto en negrita
                    headerCell.Font.Name = "Century Gothic";

                    // Aplicar bordes a la celda de encabezado
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                    headerIndex++;
                }

                // Zona 
                var CellB16 = worksheet.Range["B15"];
                CellB16.Value = "Zona ";
                CellB16.Font.Name = "Century Gothic";
                CellB16.Font.Size = 11;
                CellB16.Font.Bold = true;
                CellB16.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB16.EntireRow.Font.Color = System.Drawing.Color.Black;
                rowIndex++;


                var Ref = worksheet.Range[$"B{rowIndex}:G{rowIndex}"];
                Ref.Interior.Color = System.Drawing.Color.DarkGray;
                Ref.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                Ref.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                Ref.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                Ref.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                Ref.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                //Trabajo
                var CellB17 = worksheet.Range["B16"];
                CellB17.Value = txtArea.Text;
                CellB17.Font.Name = "Century Gothic";
                CellB17.Font.Size = 11;
                CellB17.Font.Bold = true;
                CellB17.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB17.EntireRow.Font.Color = System.Drawing.Color.Black;
                rowIndex++;

                var Zona = worksheet.Range[$"B{rowIndex}:G{rowIndex}"];
                Zona.Interior.Color = System.Drawing.Color.DarkGray;
                Zona.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                rowIndex++;

                // Plano
                var CellF15 = worksheet.Range["F15"];
                CellF15.Value = "Plano ";
                CellF15.Font.Name = "Century Gothic";
                CellF15.Font.Size = 11;
                CellF15.Font.Bold = true;
                CellF15.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellF15.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellF16 = worksheet.Range["F16"];
                CellF16.Value = txtPlano.Text;
                CellF16.Font.Name = "Century Gothic";
                CellF16.Font.Size = 11;
                CellF16.Font.Bold = true;
                CellF16.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellF16.EntireRow.Font.Color = System.Drawing.Color.Black;

                Zona.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                Zona.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                Zona.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                Zona.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;


                // Control de Ancho de la columna A y G 
                worksheet.Columns["B"].ColumnWidth = 40;
                worksheet.Columns["G"].ColumnWidth = 70;
                rowIndex++;

                // Se cargan los datos del despiece del plano en una lista 
                List<DatosFiltrados> datosFiltradosList = CargarDatosExcel();
                Dictionary<string, string> descripcionesPlano = ObtenerDescripcionesPlano(txtPlano.Text);
                List<string> descripcionesAsignadas = new List<string>();

                foreach (var datosFiltrados in datosFiltradosList)
                {
                    if (datosFiltrados.Tipo == "Titulo")
                    {
                        titleRange = worksheet.Range[$"B{rowIndex}:C{rowIndex}"];
                        titleRange.Merge(); // Combinar celdas para el título
                        titleRange.Interior.Color = System.Drawing.Color.LightGray; // Puedes ajustar el color de fondo según tu preferencia

                        string titulo = datosFiltrados.Titulo.ToString();
                        if (descripcionesPlano.ContainsKey(titulo))
                        {
                            string descripcion = descripcionesPlano[titulo];

                            if (!string.IsNullOrEmpty(descripcion))
                            {
                                // Si la descripción no está vacía, aplicar formato y ajustar la altura
                                worksheet.Rows[rowIndex].RowHeight = 70;
                                string tituloConDescripcion = $"{datosFiltrados.Titulo}{Environment.NewLine}{descripcion}";
                                titleRange.Value = tituloConDescripcion;
                                titleRange.Font.Bold = true;
                                titleRange.Font.Name = "Century Gothic";
                                titleRange.Font.Size = 11;
                                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                titleRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                                titleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                titleRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                                titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                                titleRange.WrapText = true;

                            }
                            else
                            {

                                worksheet.Rows[rowIndex].RowHeight = 30;
                                string tituloConDescripcion = $"{datosFiltrados.Titulo}{Environment.NewLine}{descripcion}";
                                titleRange.Value = tituloConDescripcion;
                                titleRange.Font.Bold = true;
                                titleRange.Font.Name = "Century Gothic";
                                titleRange.Font.Size = 11;

                                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                titleRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                                titleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                                titleRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                                titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                                titleRange.WrapText = true;
                            }

                        }

                        var color = worksheet.Range[$"D{rowIndex}:G{rowIndex}"];
                        color.Merge();
                        color.Interior.Color = System.Drawing.Color.LightGray;
                        color.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        color.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        color.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;


                        rowIndex++;
                    }
                    else if (datosFiltrados.Tipo == "Total")
                    {
                        var totalTitleRange = worksheet.Range[$"B{rowIndex}:E{rowIndex}"];                     
                        var totalSubtotalRange = worksheet.Range[$"F{rowIndex}"];

                        totalTitleRange.Merge(); // Combinar celdas para el título

                        if (!string.IsNullOrEmpty(datosFiltrados.Titulo))
                        {
                            totalTitleRange.Value = datosFiltrados.Titulo;
                            totalTitleRange.Font.Bold = true;
                            totalTitleRange.Font.Name = "Century Gothic";
                            totalTitleRange.Font.Size = 11;
                            totalTitleRange.Interior.Color = System.Drawing.Color.LightGray; // Puedes ajustar el color de fondo según tu preferencia
                            totalTitleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }
                        else
                        {
                            totalTitleRange.Value = "Subtotal ";
                            totalTitleRange.Font.Bold = true;
                            totalTitleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                            totalTitleRange.Font.Name = "Century Gothic";
                            totalTitleRange.Font.Size = 11;
                        }

                        totalTitleRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        rowIndex++;
                        totalSubtotalRange.Value = datosFiltrados.SubTotal;
                        totalSubtotalRange.Font.Bold = true;
                        totalSubtotalRange.Font.Name = "Century Gothic";
                        totalSubtotalRange.Font.Size = 11;
                        totalSubtotalRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        rowIndex++;
                    }
                    else
                    {                    
                        // Validar si existe una imagen en la ruta especificada
                        string imagePath = Path.Combine(@"\\172.16.30.6\Dibujo\DUCON\ONLINE\Dropbox\BLOQUES\IMAGENES", $"{datosFiltrados.Id_Panel}.jpg");

                        if (File.Exists(imagePath))
                        {
                            // Insertar la imagen en la celda G
                            worksheet.Shapes.AddPicture(imagePath,
                                Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue,
                                worksheet.Cells[rowIndex, 7].Left, worksheet.Cells[rowIndex, 7].Top, 50, 50);

                            worksheet.Rows[rowIndex].RowHeight = 80;
                        }

                        string Descrip = datosFiltrados.Descripcion;

                        if (!descripcionesAsignadas.Contains(Descrip))
                        {
                            descripcionesAsignadas.Add(Descrip);
                            var cellDescripcion = (Excel.Range)worksheet.Cells[rowIndex, 2];
                            cellDescripcion.Value = datosFiltrados.Descripcion;
                            cellDescripcion.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                            cellDescripcion.WrapText = true;
                            cellDescripcion.Font.Name = "Century Gothic";
                            cellDescripcion.Font.Size = 11;
                        }

                        

                        var cellRange = worksheet.Range[$"B{rowIndex}:F{rowIndex}"];
                        cellRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        var ancho = worksheet.Cells[rowIndex, 3];
                        ancho.Value = datosFiltrados.Ancho;
                        ancho.Font.Name = "Century Gothic";
                        ancho.Font.Size = 11;
                        ancho.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                        var Cantidad = (Excel.Range)worksheet.Cells[rowIndex, 4];
                        Cantidad.Value = datosFiltrados.Cantidad;
                        Cantidad.Font.Name = "Century Gothic";
                        Cantidad.Font.Size = 11;
                        Cantidad.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                        var ValorUnd = (Excel.Range)worksheet.Cells[rowIndex, 5];
                        ValorUnd.Value = datosFiltrados.ValorUnd;
                        ValorUnd.Font.Name = "Century Gothic";
                        ValorUnd.Font.Size = 11;
                        ValorUnd.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                        var SubTotal = (Excel.Range)worksheet.Cells[rowIndex, 6];
                        SubTotal.Value = datosFiltrados.SubTotal;
                        SubTotal.Font.Name = "Century Gothic";
                        SubTotal.Font.Size = 11;
                        SubTotal.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;

                        var peso = (Excel.Range)worksheet.Cells[rowIndex, 11];
                        peso.Value = Convert.ToDouble( datosFiltrados.peso ) * Convert.ToDouble( datosFiltrados.Cantidad);
                        peso.Font.Name = "Century Gothic";
                        peso.Font.Size = 11;

                        var Cubicaje = (Excel.Range)worksheet.Cells[rowIndex, 10];
                        double valorCub = (Convert.ToDouble(datosFiltrados.Cantidad) * (Convert.ToDouble( datosFiltrados.Ancho) * Convert.ToDouble(datosFiltrados.Altura) * Convert.ToDouble(datosFiltrados.profundidad))/1000000);
                        Cubicaje.Value = valorCub ;
                        Cubicaje.Font.Name = "Century Gothic";
                        Cubicaje.Font.Size = 11;

                        var cellImageRange = worksheet.Range[$"G{rowIndex}"];
                        cellImageRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        rowIndex++;
                    }
                }

                rowIndex++;

                var Cub = worksheet.Range["B" + rowIndex];
                Cub.Value = "Cubicaje Aproximado del plano ";
                Cub.Font.Name = "Century Gothic";
                Cub.Font.Size = 11;
                Cub.Font.Bold = true;
                Cub.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;


                string formulaSuma = "=SUM(J18:J150) + SUM(J18:J150) * 0.2";
                var ValorCub = worksheet.Range["D" + rowIndex];
                ValorCub.Formula = formulaSuma;
                ValorCub.Font.Name = "Arial";
                ValorCub.Font.Size = 11;
                ValorCub.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
               
                // Hoja Cotizacion Detallada
                var worksheet2 = (Excel.Worksheet)workbook.Sheets.Add();
                worksheet2.Name = "Cotizacion Detallada";

                worksheet2.Rows[1].RowHeight = 60;
                worksheet2.Columns["B"].ColumnWidth = 35;
                worksheet2.Columns["G"].ColumnWidth = 35;



                DateTime fechaActual2 = DateTime.Now;
                string nombreMes2 = fechaActual2.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                int diaActual2 = fechaActual2.Day;
                int añoActual2 = fechaActual2.Year;

                // Logo Ducon 
                var imagen2 = worksheet2.Range["B1"];
                imagen2.Value = "";
                imagen2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                imagen2.EntireRow.Font.Color = System.Drawing.Color.Black;

                string rutaImagen2 = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                worksheet2.Shapes.AddPicture(rutaImagen2,
                    Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue,
                    imagen2.Left, imagen2.Top, 140, 40);


                // Sede y Fecha del dia 
                string tableTitle2 = "Sabaneta, " + nombreMes2 + " " + diaActual2 + " de " + añoActual2;
                var titleRange2 = worksheet2.Range["B2"];
                titleRange2.Value = tableTitle2;
                titleRange2.Font.Name = "Century Gothic";
                titleRange2.Font.Size = 11;
                titleRange2.Font.Bold = true;
                titleRange2.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                titleRange2.EntireRow.Font.Color = System.Drawing.Color.Black;

                // Cotizacion 
                string titleCot2 = "Cotización Nº";
                var CellP2 = worksheet2.Range["E1"];
                CellP2.Value = titleCot2;
                CellP2.Font.Name = "Century Gothic";
                CellP2.Font.Size = 11;
                CellP2.Font.Bold = true;
                CellP2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                CellP2.EntireRow.Font.Color = System.Drawing.Color.Black;


                // Dirigido a
                var CellB4P2 = worksheet2.Range["B4"];
                CellB4P2.Value = "Señores";
                CellB4P2.Font.Name = "Century Gothic";
                CellB4P2.Font.Size = 11;
                CellB4P2.Font.Bold = true;
                CellB4P2.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB4P2.EntireRow.Font.Color = System.Drawing.Color.Black;


                // Ciudad 
                var CellB7P2 = worksheet2.Range["B7"];
                CellB7P2.Value = "Ciudad";
                CellB7P2.Font.Name = "Century Gothic";
                CellB7P2.Font.Size = 11;
                CellB7P2.Font.Bold = true;
                CellB7P2.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB7P2.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellB9P2 = worksheet2.Range["B9"];
                CellB9P2.Value = "Ref. ";
                CellB9P2.Font.Name = "Century Gothic";
                CellB9P2.Font.Size = 11;
                CellB9P2.Font.Bold = true;
                CellB9P2.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB9P2.EntireRow.Font.Color = System.Drawing.Color.Black;

                // Saludos 
                var CellB11P2 = worksheet2.Range["B11", "G11"];
                CellB11P2.Merge();
                CellB11P2.Value = "Atendiendo su amable solicitud con gusto presentamos cotización de las partes y ";
                CellB11P2.Font.Size = 11;
                CellB11P2.Font.Name = "Century Gothic";
                CellB11P2.Font.Bold = true;
                CellB11P2.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB11P2.EntireRow.Font.Color = System.Drawing.Color.Black;


                var CellB12P2 = worksheet2.Range["B12", "G12"];
                CellB12P2.Merge();
                CellB12P2.Value = "elementos del  sistema  Modular  Ducon SMD, en nuestra línea 3500, tal como sigue ";
                CellB12P2.Font.Size = 11;
                CellB12P2.Font.Name = "Century Gothic";
                CellB12P2.Font.Bold = true;
                CellB12P2.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB12P2.EntireRow.Font.Color = System.Drawing.Color.Black;


                // Comienzo de encabezados 
                int headerIndex2 = 2;
                int rowIndex2 = 14;

                foreach (string encabezado in encabezados)
                {

                    worksheet2.Cells[rowIndex2, headerIndex2] = encabezado;

                    var headerCell2 = (Excel.Range)worksheet2.Cells[rowIndex2, headerIndex2];
                    headerCell2.Font.Bold = true;  // Establecer el texto en negrita
                    headerCell2.Font.Name = "Century Gothic";
                    headerCell2.Font.Size = 11;
                    // Aplicar bordes a la celda de encabezado
                    headerCell2.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell2.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell2.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    headerCell2.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

                    headerIndex2++;
                }


                // Hoja Condiciones Generales 

                var worksheet3 = (Excel.Worksheet)workbook.Sheets.Add();
                worksheet3.Name = "Condiciones Comerciales ";

                worksheet3.Columns["B"].ColumnWidth = 40;

                var CellB3P3 = worksheet3.Range["B3"];
                CellB3P3.Value = "CONDICIONES GENERALES DE VENTA:";
                CellB3P3.Font.Name= "Century Gothic";
                CellB3P3.Font.Size = 11;
                CellB3P3.Font.Bold = true;
                CellB3P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB3P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                // TIEMPO DE ENTREGA
                var CellB5P3 = worksheet3.Range["B5"];
                CellB5P3.Value = "1. TIEMPO DE ENTREGA:";
                CellB5P3.Font.Size = 11;
                CellB5P3.Font.Name = "Century Gothic";
                CellB5P3.Font.Bold = true;
                CellB5P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB5P3.EntireRow.Font.Color = System.Drawing.Color.Black;

              
                string[] tiempoDeEntrega = {
                    "Para proyectos de hasta 100 puestos de trabajo, con diseño y",
                    "acabados de línea, DUCON S.A.S normalmente, se tomará 21 días para despachar;",
                    "Sin embargo, los tiempos reales de entrega deberán ser definidos con el ejecutivo de",
                    "proyectos y estarán sujetos a las características de la obra, a la disponibilidad de",
                    "acabados y materiales del proyecto.",
                    "La confirmación de dichos tiempos se realizará una vez la obra ingrese al sistema de",
                    "información DUCON y la obra sea analizada por nuestro personal de compras y de producción.",
                    "El plazo se establece luego del anticipo, entrega de orden de compra, firma de planos y",
                    "definición de acabados.",
                    "El tiempo de entrega de proyectos con productos o acabados especiales puede",
                    "incrementarse según disponibilidad de proveedor."
                };

                AgregarTextoDesdeArray(worksheet3, tiempoDeEntrega, "B6", 12,false);

                // INSTALACIÓN
                var CellB18P3 = worksheet3.Range["B18"];
                CellB18P3.Value = "2. INSTALACIÓN:";
                CellB18P3.Font.Size = 11;
                CellB18P3.Font.Name = "Century Gothic";
                CellB18P3.Font.Bold = true;
                CellB18P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB18P3.EntireRow.Font.Color = System.Drawing.Color.Black;
               
                string[] instalacion = {
                        "El valor de la instalación ya está incluido en la cotización, obras",
                        "fuera del área metropolitana de Medellín y Bogotá podrán tener recargo por concepto de",
                        "viáticos y transporte según características del proyecto.",
                        "Cualquier solicitud de cambio en la distribución pactada o en las especificaciones de",
                        "producto, durante o después de la instalación, se reprogramará después de firmada.",
                        "el acta de entrega",
                        "SI el cliente requiere postergar la entrega e instalación del proyecto, se reprogramará",
                        "según disponibilidad de la compañía, si la prórroga es superior a 15 días calendarios, podrá",
                        "generar recargos por concepto de bodegaje a tasa de almacén de depósito vigente."
                };

                AgregarTextoDesdeArray(worksheet3, instalacion, "B19", 12, false);

                //  OBSERVACIONES GENERALES
                var CellB29P3 = worksheet3.Range["B29"];
                CellB29P3.Value = "3. OBSERVACIONES GENERALES:";
                CellB29P3.Font.Size = 11;
                CellB29P3.Font.Name = "Century Gothic";
                CellB29P3.Font.Bold = true;
                CellB29P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB29P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                string[] observacionesGenerales = {
                    "Para garantizar la entrega a satisfacción del proyecto El cliente debe garantizar:",
                    "*  Cielos terminados",
                    "*  Paredes estucadas y pintadas",
                    "*  Pisos pulidos y brillados",
                    "*  Ventanería instalada",
                    "*  Luminarias instaladas y funcionando",
                    " ",
                    "Una  vez  entregado  el  material estará bajo la responsabilidad del cliente, este deberá",
                    "proveer de un lugar con condiciones de higiene y seguridad  adecuadas para ",
                    "el  producto."
                };
                AgregarTextoDesdeArray(worksheet3, observacionesGenerales, "B30", 12,false);

                //  NOTAS
                string[] NotaImportante = {
                    "Importante: Al recibir su pedido,  revise que las cantidades y el estado de la mercancía",
                    "coincidan con la remisión y no presenten averías.",
                    "Si detecta deterioro de la mercancía o faltantes, agradecemos  dejar constancia en la",
                    "remisión y notificar a su coordinador logístico."
                };
                AgregarTextoDesdeArray(worksheet3, NotaImportante, "B42", 12, true);

                var CellB47P3 = worksheet3.Range["B47"];
                CellB47P3.Value = "Nota 1: ";
                CellB47P3.Font.Size = 11;
                CellB47P3.Font.Name = "Century Gothic";
                CellB47P3.Font.Bold = true;
                CellB47P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB47P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                string[] Nota1 = {
                    "Para  garantizar el funcionamiento adecuado del producto  recomendamos",
                    "que la instalación cableado estructurado  voz  y  datos, se realice por un experto.",
                    "obedeciendo indicaciones mínimas del personal de instalación DUCON"
              
                };
                AgregarTextoDesdeArray(worksheet3, Nota1, "B48", 12, false);

                var CellB52P3 = worksheet3.Range["B52"];
                CellB52P3.Value = "Nota 2: ";
                CellB52P3.Font.Size = 11;
                CellB52P3.Font.Name = "Century Gothic";
                CellB52P3.Font.Bold = true;
                CellB52P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB52P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                string[] Nota2 = {
                    "Se debe tener especial cuidado con Las pantallas  y separadores en vidrio de puestos",
                    "de trabajo, debido a que pueden fisurarse si son golpeadas o sometidas a presión excesiva al ",
                    "recostarse en ellas. DUCON S.A.S. no se hace responsable por daños o perjuicios  por este tipo",
                    "de eventos."

                };
                AgregarTextoDesdeArray(worksheet3, Nota2, "B53", 12, false);

                // COORDINACIÓN
                var CellB58P3 = worksheet3.Range["B58"];
                CellB58P3.Value = "4. COORDINACIÓN: ";
                CellB58P3.Font.Size = 11;
                CellB58P3.Font.Name = "Arial";
                CellB58P3.Font.Bold = true;
                CellB58P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB58P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                string[] coordinacion = {
                    "Para  nosotros  es  importante  que  usted  este informado  en todo ",
                    "momento  del  estado de  su  pedido, por lo tanto además del ejecutivo de proyecto, usted ",
                    "cuenta con un coordinador logístico que le será asignado por la compañía y se pondrá ",
                    "en contacto con usted durante la ejecución del proyecto."

                };
                AgregarTextoDesdeArray(worksheet3, coordinacion, "B59", 12, false);

                //GARANTÍA
                var CellB65P3 = worksheet3.Range["B65"];
                CellB65P3.Value = "5. GARANTÍA:";
                CellB65P3.Font.Size = 11;
                CellB65P3.Font.Name = "Century Gothic";
                CellB65P3.Font.Bold = true;
                CellB65P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB65P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                string[] garantia = {
                    "DUCON S.A.S   ofrece  garantía  por  cinco  (5)  años contra  defectos de ",
                    "fábrica  para mobiliario y  un (1) año para  sillas, elementos de ",
                    "reposición como chapas y correderas. La garantía no cubre daños por uso inadecuado, ",
                    "sabotaje o daños o ocasionados por personas ajenas a DUCON."

                };
                AgregarTextoDesdeArray(worksheet3, garantia, "B66", 12, false);


                //SERVICIO POSVENTA
                var CellB71P3 = worksheet3.Range["B71"];
                CellB71P3.Value = "6. SERVICIO POSVENTA:";
                CellB71P3.Font.Size = 11;
                CellB71P3.Font.Name = "Century Gothic";
                CellB71P3.Font.Bold = true;
                CellB71P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB71P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                string[] postventa = {
                    "DUCON S.A.S ofrece a solicitud del cliente, y dentro de los 3 meses ",
                    "seguidos a la instalación, el servicio de visita posventa; visita preventiva para verificar el ",
                    "estado de la obra.",
                    "Para solicitar este servicio, llame a los telefónos: 302 11 67 ext  109 - 110 - 111   ",
                    "ó    288 98 98 ext. 129."

                };
                AgregarTextoDesdeArray(worksheet3, postventa, "B72", 12, false);


                // FORMA DE PAGO
                var CellB78P3 = worksheet3.Range["B78"];
                CellB78P3.Value = "7. FORMA DE PAGO: ";
                CellB78P3.Font.Size = 11;
                CellB78P3.Font.Name = "Century Gothic";
                CellB78P3.Font.Bold = true;
                CellB78P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB78P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellB79P3 = worksheet3.Range["B79"];
                CellB79P3.Value = "60% Anticipo        40% A la Entrega de la obra";
                CellB79P3.Font.Size = 11;
                CellB79P3.Font.Name = "Century Gothic";
                CellB79P3.Font.Bold = false;
                CellB79P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB79P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellB81P3 = worksheet3.Range["B81"];
                CellB81P3.Value = "IMPORTANTE Los descuentos otorgados pierden validez con el incumplimiento de: ";
                CellB81P3.Font.Size = 11;
                CellB81P3.Font.Name = "Century Gothic";
                CellB81P3.Font.Bold = true;
                CellB81P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB81P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                string[] importante = {
                    "Las condiciones comerciales de venta, específicamente en las formas de pago tanto del",
                    "anticipo como en el pago de facturas a la fecha de vencimiento."
                };
                AgregarTextoDesdeArray(worksheet3, importante, "B82", 12, false);


                // Nota en Amarilla 
                var CellB85P3 = worksheet3.Range["B85", "F90"];
                CellB85P3.Merge();
                CellB85P3.Value = "POR CONTRATO POR MANDATO: En virtud del los artículos 1634 y 1635 del código civil colombiano realizar los pagos a nombre de VISION EMPRESARIAL G2  S.A.S con Nit 900.314.150-1 EN BANCOLOMBIA  CUENTA CORRIENTE No. 01757718995. (Si requiere copia del contrato y certificado favor solicitarlo al correo carteraducon@ducon.com.co  - laurarestrepo@ducon.com.co)   ";
                CellB85P3.Font.Size = 11;
                CellB85P3.Font.Name = "Century Gothic";
                CellB85P3.Font.Bold = true;
                CellB85P3.EntireRow.Font.Color = System.Drawing.Color.Black;
                CellB85P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB85P3.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                CellB85P3.Interior.Color = System.Drawing.Color.Yellow;
                CellB85P3.WrapText = true;
                // Aplicar grosor a los bordes
                CellB85P3.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlThick;
                CellB85P3.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThick;
                CellB85P3.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlThick;
                CellB85P3.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThick;

                // FINANCIACIÓN
                var CellB92P3 = worksheet3.Range["B92"];
                CellB92P3.Value = "8. FINANCIACIÓN:";
                CellB92P3.Font.Size = 11;
                CellB92P3.Font.Name = "Century Gothic";
                CellB92P3.Font.Bold = true;
                CellB92P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB92P3.EntireRow.Font.Color = System.Drawing.Color.Black;
                string[] finaciacion = {
                    "DUCON ofrece las siguientes alternativas de financiación de su proyecto de oficina",
                    "RENTING de Infraestructura: No afecta cupo de endeudamiento, recomendado para realizar",
                    "estrategias tributarias.",
                    "Si desea conocer más de este producto, comuníquese con su ejecutivo de proyectos.",
                    " ",
                    "Banco Corp Banca  Leasing o crédito: Contacto Medellín  Juan Manuel Penagos.",
                    "correo electrónico jpenagossilva@corpbanca.com.co",
                    "Teléfono  fijo  : 4-604 18 18  op 2 ext 3454 , celular  317 364 34 78 Este  proceso  debe  ser  ",
                    "adelantado  directamente por el cliente con el banco."


                };
                AgregarTextoDesdeArray(worksheet3, finaciacion, "B93", 12, false);

                //  VALIDEZ DE LA PROPUESTA
                var CellB104P3 = worksheet3.Range["B104"];
                CellB104P3.Value = "9. VALIDEZ DE LA PROPUESTA:  ";
                CellB104P3.Font.Size = 11;
                CellB104P3.Font.Bold = true;
                CellB104P3.Font.Name = "Century Gothic";
                CellB104P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB104P3.EntireRow.Font.Color = System.Drawing.Color.Black;
                string[] validez = {
                    "30  Días calendario.",
                    "",
                    "NOTA:   Somos autoretenedores Resolución  000075  -  Junio  24/93,  no somos  grandes",
                    "contribuyentes, resolución 000041 de enero 30 del 2014, contribuyente industrial de ICA en ",
                    "Sabaneta, somos auto retenedores de CREE, exentos de RETEICA, según articulo 77 ley 49     ",
                    "de 1990."
                };
                AgregarTextoDesdeArray(worksheet3, validez, "B105", 12, false);


                //  DEVOLUCIONES
                var CellB112P3 = worksheet3.Range["B112"];
                CellB112P3.Value = "10. DEVOLUCIONES:";
                CellB112P3.Font.Size = 11;
                CellB112P3.Font.Name = "Century Gothic";
                CellB112P3.Font.Bold = true;
                CellB112P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB112P3.EntireRow.Font.Color = System.Drawing.Color.Black;
               
                string[] devoluciones = {
                    "Una vez aprobados planos y especificaciones no se aceptan ",
                    "devoluciones. Casos especiales serán analizados para su devolución y se reconocerá ",
                    "hasta por un monto máximo del 50% del valor cotizado. Esta condición aplica para ",
                    "productos manufacturados a la medida del cliente como panelería, puestos de trabajo, ",
                    "gavetas, credenzas, bibliotecas, entre otros.",
                    " No aplica para productos como sillas y archivadores cuya devolución puede ascender",
                    " al 100% del valor cotizado luego de control de calidad."
                };
                AgregarTextoDesdeArray(worksheet3, devoluciones, "B113", 12, false);


                //Firma Gerencial 
                var CellB1121P3 = worksheet3.Range["B121" ,"B122"];
                CellB1121P3.Merge();
                CellB1121P3.Value = "Ejecutivo de Proyectos";
                CellB1121P3.Font.Size = 11;
                CellB1121P3.Font.Name = "Century Gothic";
                CellB1121P3.Font.Bold = true;
                CellB1121P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellB1121P3.EntireRow.Font.Color = System.Drawing.Color.Black;


                var CellD121P3 = worksheet3.Range["D121", "F121"];
                CellD121P3.Merge();
                CellD121P3.Value = "JAIME RENDÓN LONDOÑO";
                CellD121P3.Font.Size = 11;
                CellD121P3.Font.Name = "Century Gothic";
                CellD121P3.Font.Bold = true;
                CellD121P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellD121P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                var CellD122P3 = worksheet3.Range["D122", "E122"];
                CellD122P3.Merge();
                CellD122P3.Value = "Gerente Comercial";
                CellD122P3.Font.Size = 11;
                CellD122P3.Font.Name = "Century Gothic";
                CellD122P3.Font.Bold = true;
                CellD122P3.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                CellD122P3.EntireRow.Font.Color = System.Drawing.Color.Black;

                AdjustMargins(worksheet.PageSetup);
                AdjustMargins(worksheet2.PageSetup);
                AdjustMargins(worksheet3.PageSetup);
                worksheet.Columns.AutoFit();
                // Mostrar la aplicación de Excel
                excelApp.Visible = true;



                string script = @"CerrarCargarExcel();";
                ScriptManager.RegisterStartupScript(this, GetType(), "CerrarCargarExcel", script, true);


                // Liberar el objeto Worksheet
                Marshal.ReleaseComObject(worksheet);

                // Liberar el objeto Workbook
                Marshal.ReleaseComObject(workbook);

                // Liberar el objeto Application
                Marshal.ReleaseComObject(excelApp);


                // Pendiente validar que los excel no queden abiertos ne segundo plano 
                // Obtener el ID del proceso Excel
                int processId = excelApp.Hwnd;
                // Finalizar el proceso Excel
                System.Diagnostics.Process.GetProcessById(processId).Kill();
               
             

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar a Excel: " + ex.Message);
            }


        }

        static void AdjustMargins(Excel.PageSetup pageSetup)
        {
            // Ajustar los márgenes según tus necesidades
            // Los valores están en pulgadas, pero puedes ajustar según tus preferencias
            pageSetup.LeftMargin = 0.5;
            pageSetup.RightMargin = 0.5;
            pageSetup.TopMargin = 4;
            pageSetup.BottomMargin = 0.5;

        }

        public List<DatosFiltrados> CargarDatosExcel()
        {
            List<DatosFiltrados> datosFiltradosList = new List<DatosFiltrados>();
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand("cta_Plano_Paneles", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Plan", SqlDbType.VarChar, 30).Value = txtPlano.Text;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                var gruposUnicos = dataTable.AsEnumerable().Select(r => r.Field<string>("Descripcion_Grupo")).Distinct();

                foreach (var grupo in gruposUnicos)
                {
                    datosFiltradosList.Add(new DatosFiltrados { Tipo = "Titulo", Titulo = grupo });

                    var datosFiltrados = dataTable.AsEnumerable().Where(r => r.Field<string>("Descripcion_Grupo") == grupo).Select(r => new DatosFiltrados
                    {
                        Descripcion = r["Descripcion"].ToString(),
                        Ancho = r["Ancho"].ToString(),
                        Cantidad = r["Cantidad"].ToString(),
                        ValorUnd = r["Precio_Venta"].ToString(),
                        SubTotal = (Convert.ToDecimal(r["Cantidad"]) * Convert.ToDecimal(r["Precio_Venta"])).ToString(),
                        Id_Panel = r["Id_Panel"].ToString(),
                        peso = r["PesoKG"].ToString(),
                        profundidad = r["Profundidad"].ToString(),
                        ajusteCub = r["AjusteCubicaje"].ToString(),
                        Altura = r["Altura"].ToString()
                    });

                    datosFiltradosList.AddRange(datosFiltrados);

                    decimal totalVenta = datosFiltrados.Sum(d => Convert.ToDecimal(d.SubTotal));
                    datosFiltradosList.Add(new DatosFiltrados { Tipo = "Total", SubTotal = totalVenta.ToString() });
                }

                decimal totalGeneral = datosFiltradosList.Where(d => d.Tipo != "Titulo" && d.Tipo != "Total").Sum(d => Convert.ToDecimal(d.SubTotal));
                // Agregar la fila de total general al final de la lista
                datosFiltradosList.Add(new DatosFiltrados { Tipo = "Total", Titulo = "Total Despiece", SubTotal = totalGeneral.ToString() });
            }

            return datosFiltradosList;
        }

        private Dictionary<string, string> ObtenerDescripcionesPlano(string plano)
        {
            Dictionary<string, string> descripciones = new Dictionary<string, string>();

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            string sSql = "SELECT Distinct tblGrupoObjeto.Descripcion_Grupo,tblGrupoObjeto.GODescripcionTecnica " +
                          "FROM tblPlano " +
                          "INNER JOIN tblGrupoObjeto " +
                          "INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto " +
                          "INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum ON tblPlano.Plano = tblPlano_Panel.Id_Plano " +
                          "WHERE tblPlano.Plano = @plano";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", plano);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string idPanel = reader["Descripcion_Grupo"].ToString();
                            string descripcion = reader["GODescripcionTecnica"].ToString();

                            descripciones.Add(idPanel, descripcion);
                        }
                    }
                }
            }

            return descripciones;
        }

        protected void AgregarTextoDesdeArray(Excel.Worksheet worksheet, string[] textoArray, string celdaInicio, int fontSize = 12, bool bold = false)
        {
            int fila = int.Parse(celdaInicio.Substring(1));  // Extraemos el número de fila de la celda de inicio
            int columna = celdaInicio[0] - 'A' + 1;  // Obtenemos el número de columna de la celda de inicio

            for (int i = 0; i < textoArray.Length; i++)
            {
                // Calculamos la celda de inicio para cada línea
                string celdaInicioLinea = $"{(char)('A' + columna - 1)}{fila + i}";

                // Calcular la celda de fin para cada línea (columna F)
                string celdaFinLinea = $"{(char)('F')}{fila + i}";

                // Fusionar las celdas de la columna B a la F para cada línea
                var cellRange = worksheet.Range[celdaInicioLinea, celdaFinLinea];
                cellRange.Merge();
                cellRange.Value = textoArray[i];
                cellRange.Font.Size = fontSize;
                cellRange.Font.Bold = bold;
                cellRange.Font.Name = "Century Gothic";
                cellRange.EntireRow.Font.Color = System.Drawing.Color.Black;
                cellRange.WrapText = true;
            }
        }

        protected void DocumentacionOt_Click(object sender, EventArgs e)
        {

            Session["Id_OT2"] = tbOT.Text;
            Session["pedido2"] = ddlNumbers.Text;

            string url = "FormExtPrin/DocumentacionOT.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
        }
    }

}


