using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Media.TextFormatting;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using DataTable = System.Data.DataTable;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class OrdenTrabajo : System.Web.UI.Page
    {
        private string id;
        private string pedido;
        private List<TextBox> listaTextBoxes;
        private List<DropDownList> listaDropDownLists;

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

                    listaTextBoxes = new List<TextBox>
                {
                    tbObra,tbDir,tbContac,tbEmail,tbRecibe,tbTel,tbCel,tbPais,tbHTotal,tbVenta,dtpFechaEntregaDibujoDespiece,dtpFechaEntregaProduccion,dtpEmpaque,dtpRealEmpaque,tbSupervisor,
                    tbBolsa,tbValorPedido,txtNit,txtNombreEmp,txtcontacto,txtMail,txtDireccion,txtMunicipio,txtTelefono,txtCotizacion,txtValorSugerido,txtVcsd,txtVccd,txtOrdenCompra,txtAsesor,txtComision,
                    txtDiseño,txtSaldo,txtVenta,txtDcto,txtDctoValor,txtVtte,txtVvia,txtGtotal

                        };

                    listaDropDownLists = new List<DropDownList>
                {
                   ddlNumbers,ddlZona,dtacboTipoPedido,cboPedidoBase,DtaCboTipoAprobacion,ddlFabrica1,ddlInstala,ddlAsesor,ddlCiudad

                        };

                    txObs2.Disabled = true;
                    txObs1.Disabled = true;

                    CargarAsesoresEnDropDownList();
                    DeshabilitarTextBoxes(listaTextBoxes);
                    DeshabilitarDropDownLists(listaDropDownLists);
                    Nit.Enabled = false;
                    Nit.CssClass = "bi bf  btn btn-outline-secondary";
                    btnCotizacion.Enabled = false;
                    btnCotizacion.CssClass = "bi bf  btn btn-outline-secondary";

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
                            }

                        }
                        // Este bloque consulta la OT con variables de Session de afuera del formulario 
                        else if (Session["Id_OT2"] != null && Session["pedido2"] != null)
                        {
                    dtacboTipoPedido.DataBind();
                    dtacboTipoPedido.Items.Insert(0, new ListItem(" "));

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
                    tbPedDepen.DataBind();
                    tbPedDepen.Items.Insert(0, new ListItem(" "));

                    //Cargando los datos del nit 
                    CargarVariablesDeSesionContable();
                    cboPedidoBase.DataBind();
                    cboPedidoBase.Items.Insert(0, new ListItem(" "));

                    DtaCboTipoAprobacion.DataBind();
                    DtaCboTipoAprobacion.Items.Insert(0, new ListItem(" "));



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
                "txtVvia", "txtVenta", "tbValorPedido"
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

             
            }


            if (tbOT.Text == "Por Asig.")
            {
                HabilitarTodosLosTextBoxes();

              
                string zonaLogeada = Session["ZonaLogeada"] as string; // Obtén el valor de la variable de sesión

                // Establece el valor seleccionado en el DropDownList ddlZona
                ddlZona.SelectedValue = zonaLogeada;

               
            }


            Nit.Enabled = true;

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

                            // Extraer datos y asignarlos a controles
                            AssignDataToControls(leer);

                            EnableButtons();

                            // Obtener datos de cotización y asignarlos a controles
                            AssignCotizacionData(id, pedido, txtCotizacion.Text);


                        }
                    }
                }
            
         
          
            Cargar_Plano( id, pedido);
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
            dtacboTipoPedido.DataBind();

            tbPedDepen.DataBind();

            cboPedidoBase.DataBind();

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
                        lbPlano.Text = drcot["PlanoOk"].ToString();

                        if (cotizacion.ToUpper() == "NO TIENE" || string.IsNullOrEmpty(cotizacion))
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

                            // Obtener datos de cotización y asignarlos a controles
                            AssignCotizacionData(id, pedido, txtCotizacion.Text, sqlconectar);


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
                        RevisadoDibujo = Convert.ToBoolean( r["RevisadoDibujo"].ToString())



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

        protected void DataGridDespiecePlano_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DatosFiltrados datos = (DatosFiltrados)e.Item.DataItem;

                if (datos.Tipo == "Total" && datos.Titulo == "<b>Totales</b>" )
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#7aeaff");
                }
                else if (!string.IsNullOrEmpty(datos.ID ) ) 
                {
                    if(datos.RevisadoDibujo == true)
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

                string url = "FormExtPrin/ObjetoDespiece.aspx";
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

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


        // Consultar Documentacio 
        protected void chxFiltrarDocumentacion_CheckedChanged(object sender, EventArgs e)
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


        //metodo pendiente para adjuntar documentacion a la Ot
        protected void AdjuntarDocumento(object sender, EventArgs e)
        {
            if (DocOt.HasFile)
            {

            }
            else
            {

            }


        }

        // metodo pendiente par eliminar documentos
        protected void EliminarDocumento(object sender, EventArgs e)
        {
            if (DocOt.HasFile)
            {

            }
            else
            {

            }


        }


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

                        Session.Remove("OTinsertada");
                        Session.Remove("PedidoInsertado");
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
                    // La inserción se realizó con éxito
                }
                else
                {
                    // Ocurrió un problema al realizar la inserción
                }
            }
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


        }


    }

}
