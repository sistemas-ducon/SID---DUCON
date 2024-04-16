using AjaxControlToolkit;
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
using System.Data.Entity.Core.Objects;
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
using System.Runtime.InteropServices;
using static SISTEMA_INTEGRAL_DUCON.Formularios.OrdenTrabajo;
using System.Web.UI.WebControls.WebParts;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class OrdenTrabajo : System.Web.UI.Page
    {
        private string id;
        private string pedido;
        private List<TextBox> listaTextBoxes;
        private List<DropDownList> listaDropDownLists;

        private string CadenaConexionSID = "BD_SIDSQL";
        private string CadenaConexionISID = "BD_ISIDSQL";
        private string CadenaConexionSSF = "BD_SSF";

        //Variable para Calcular Fecha Empaque 
        private int DiasMinimoparaProduccion = 0;
        private int DiasPorDefectoParaProduccion = 0;
        private int DiasHabiles = 0;

        public double TotalObraMas = 0;
        public double TotalObraMenos = 0;
        public double TotalSaldo;

        private DateTime FechaEmpaque;
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
                    // Verificar si la variable de sesión 'MostrarModal' tiene contenido y es true
                    if (Session["ModalMostrado"] != null && (bool)Session["ModalMostrado"] == true)
                    {
                        NuevaOTDespuesDeCargarNIT();
                        Session.Remove("ModalMostrado");
                    }
                    else
                    {
                        // Si 'MostrarModal' es false o null, establecer 'ModalMostrado' en null
                        Session["ModalMostrado"] = null;
                    }

                    CargarVariablesDeSesionContable();

                }


            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }


        protected void BotonesModificar()
        {
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

            Nit.Enabled = true;

            cbxComisionCompart.Enabled = true;
        }

        protected void dtacboTipoPedido_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Obtener el valor de EstadisticaVenta del dtacboTipoPedido seleccionado
            bool estadisticaVenta = ObtenerEstadisticaVenta(dtacboTipoPedido.SelectedValue);

            // Habilitar o deshabilitar cboPedidoBase según el valor de EstadisticaVenta
            cboPedidoBase.Enabled = !estadisticaVenta;

            // Si EstadisticaVenta es false, llenar cboPedidoBase con los valores de la consulta
            if (!estadisticaVenta)
            {
                LlenarCboPedidoBase();

                // Establecer el texto del TextBox
                txtCotizacion.Text = "NO TIENE";
                txtOrdenCompra.Text = "NA";
                cbxComisionCompart.Enabled = false;

                // Invocar manualmente el evento OnTextChanged
                EventArgs args = new EventArgs();
                txtCotizacion_TextChanged(txtCotizacion, args);
            }
            else
            {
                txtCotizacion.Text = "";
                txtOrdenCompra.Text = "";
                cbxComisionCompart.Enabled = true;
            }

            cboPedidoBase.DataBind();
            cboPedidoBase.Items.Insert(0, new ListItem(" "));
        }

        protected void cbxComisionCompart_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxComisionCompart.Checked)
            {
                if (!txtVenta.Enabled && !txtDcto.Enabled) // Si ambos TextBox están deshabilitados
                {
                    txtVenta.Enabled = true;
                    txtDcto.Enabled = true;
                }
                else // Si alguno o ambos TextBox ya están habilitados, deshabilitarlos
                {
                    txtVenta.Enabled = false;
                    txtDcto.Enabled = false;
                    cbxComisionCompart.Checked = false; // Deseleccionar el CheckBox
                }
            }
            else
            {
                txtVenta.Enabled = false;
                txtDcto.Enabled = false;
            }
        }


        private bool ObtenerEstadisticaVenta(string idTipoPedido)
        {
            bool estadisticaVenta = false; // Valor predeterminado

            // Realizar la consulta para obtener el valor de EstadisticaVenta según el Id_TipoPedido
            // Puedes utilizar la lógica de acceso a datos que prefieras, por ejemplo, SqlConnection y SqlCommand
            // Aquí es un ejemplo simplificado
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT EstadisticaVenta FROM tblTipoPedido WHERE Id_TipoPedido = @Id_TipoPedido", connection))
                {
                    command.Parameters.AddWithValue("@Id_TipoPedido", idTipoPedido);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        estadisticaVenta = Convert.ToBoolean(result);
                    }
                }
            }

            return estadisticaVenta;
        }

        private void LlenarCboPedidoBase()
        {


            // Obtener el valor de la variable de sesión "Id_OT"
            string idOT = Session["Id_OT"]?.ToString();

            // Verificar que la variable de sesión "Id_OT" no sea nula o vacía
            if (!string.IsNullOrEmpty(idOT))
            {
                // Realizar la consulta para obtener los valores de PedidoBase y EstadisticaVenta

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT Consecutivo_Pedido, EstadisticaVenta FROM tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido WHERE tblOT.Id_OT = @Id_OT AND EstadisticaVenta = '1' ORDER BY tblOT.Consecutivo_Pedido DESC", connection))
                    {
                        command.Parameters.AddWithValue("@Id_OT", idOT);

                        // Crear un lector para obtener los resultados de la consulta
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Limpiar los elementos existentes en cboPedidoBase
                            cboPedidoBase.Items.Clear();

                            // Agregar los nuevos elementos desde la consulta
                            while (reader.Read())
                            {
                                string pedidoBase = reader["Consecutivo_Pedido"].ToString();
                                cboPedidoBase.Items.Add(new ListItem(pedidoBase, pedidoBase));
                            }
                        }
                    }
                }
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
            if (Session["ModalMostrado"] == null)
            {
                // Mostrar el modal solo si no se ha mostrado antes
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#LlenarNIT').modal('show');", true);
            }


        }

        protected void NuevaOTDespuesDeCargarNIT()
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

            txtAsesor.Enabled = false;


            Nit.Enabled = false;
            Nit.CssClass = "btn btn-sm shadow button-disabled";

            TiposDePedidos.SelectCommand = "SELECT Descripcion_TipoPedido, Id_TipoPedido, EstadisticaVenta FROM tblTipoPedido WHERE Activo = '1' AND EstadisticaVenta = '1' ORDER BY Descripcion_TipoPedido";


            dtacboTipoPedido.DataBind();
            dtacboTipoPedido.Items.Insert(0, new ListItem(" "));

            cbxComisionCompart.Enabled = true;

            Session["NuevaOTEjecutada"] = true;
            Session.Remove("BtnModificarEjecutado");
            Session.Remove("NuevoPedido");
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
                
                string url = "FormExtPrin/ObservacionesOT.aspx";
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
            
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
            ddlCiudad.Items.Insert(0, new ListItem(" ", " "));
        }

        protected void ddlGrupoObjeto_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlGrupo.Items.Insert(0, new ListItem("Seleccione", ""));

        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT *, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial  order by Apellidos";

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
            ddlAsesor.Items.Insert(0, new ListItem(" ", " "));
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

            cbxComisionCompart.Enabled = false;
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
            txObs2.Disabled = true;

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

        protected void ObtenerInfoOt3()
        {

            string id = Session["Id_OT3"] as string;

            int perdidoMaximo = CargarPedidoMaximo(id);

            Session["pedidoMax"] = perdidoMaximo;

            CargarOtInsertada3();
            if (!string.IsNullOrEmpty(id))
            {

                string inputData = tbOT.Text;
                List<int> numeros = ObtenerNumerosDesdeLaBaseDeDatos(inputData);

                ddlNumbers.Items.Clear(); // Limpiar las opciones existentes

                foreach (int numero in numeros)
                {
                    ddlNumbers.Items.Add(numero.ToString());
                }

                ddlNumbers.SelectedValue = perdidoMaximo.ToString();

            }
        }

        protected void ObtenerInfoOt(object sender, EventArgs e)
        {

            string id = tbOT.Text.Trim();
            Session["Id_OT"] = id;

            int perdidoMaximo = CargarPedidoMaximo(id);

            Session["pedido"] = perdidoMaximo;

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

                ddlNumbers.SelectedValue = perdidoMaximo.ToString();

            }
        }

        private int CargarPedidoMaximo(string id)
        {
            int numero = 0;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MAX (Consecutivo_Pedido) FROM tblOT WHERE Id_OT = @IdOT";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdOT", id);

                    connection.Open();

                    Object nombre = command.ExecuteScalar();

                    if (nombre != null && nombre != DBNull.Value)
                    {
                        numero = Convert.ToInt32(nombre);
                    }

                }
            }

            return numero;
        }


        private List<int> ObtenerNumerosDesdeLaBaseDeDatos(string dato)
        {
            List<int> numeros = new List<int>();

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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


            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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

                            Session["IdContactoFactSession"] = IDCLienteConstacto;

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

            CarteraVencida();

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

            ListItem item2 = ddlNumbers.Items.FindByValue(leer["Consecutivo_Pedido"].ToString());
            if (item2 != null)
            {
                ddlNumbers.SelectedValue = item2.Value;
            }

            //pedido = leer["Consecutivo_Pedido"].ToString();

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
            ddlCiudad.DataBind(); // Forzar el enlace de datos
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
            CalcularSaldo();

            sqlDataSource1.DataBind();
            tbPedDepen.DataBind();

            // variable de para IDCOntactoCliente 
            Session["ID_ContactoBD"] = leer["IDContacto_Cliente"].ToString();

        }

        private void AssignCotizacionData(string id, string pedido, string cotizacion)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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

        private void EnableButtons2()
        {
            bool estaAbierta = false;

            // Obtenga los valores de las variables
            string id = Session["Id_OT2"]?.ToString();
            string pedido = Session["pedido2"]?.ToString();

            bool estaCerrada = EstaCerrada(id, pedido);

            // Realice la consulta
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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

        private void EnableButtons3()
        {
            bool estaAbierta = false;

            // Obtenga los valores de las variables
            string id = Session["Id_OT3"]?.ToString();
            string pedido = Session["pedidoMax"]?.ToString();

            bool estaCerrada = EstaCerrada(id, pedido);

            // Realice la consulta
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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
                   ddlZona,dtacboTipoPedido,cboPedidoBase,DtaCboTipoAprobacion,ddlFabrica1,ddlInstala,ddlAsesor,ddlCiudad

                };

            DeshabilitarDropDownLists(listaDropDownLists);





            txObs1.Disabled = true;
            txObs2.Disabled = true;

            ObservacionCont.Disabled = true;
            TextTNegociacion.Disabled = true;

            ddlNumbers.Enabled = true;




            Nit.Enabled = false;
            Nit.CssClass = "btn btn-sm shadow button-disabled";
        }

        //FIN MODIFICACION

        protected void ValidarAsesor()
        {
            string cedula = txtAsesor.Text.Trim(); // Obtener el valor del TextBox txtAsesor

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string consulta = "SELECT Activo FROM tblAsesorComercial WHERE Cedula = @Cedula ORDER BY Apellidos";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(consulta, connection);
                command.Parameters.AddWithValue("@Cedula", cedula);

                connection.Open();

                object resultado = command.ExecuteScalar();

                if (resultado != null)
                {
                    int activo = Convert.ToInt32(resultado);

                    if (activo == 0)
                    {
                        string contenidoModalValAse = "El asesor con Codigo: " + cedula + " de este pedido esta inactivo, o fue borrado del sistema ";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ValidarAsesor').modal('show'); $('#ValidarAsesor1').text('" + contenidoModalValAse + "');", true);
                    }
                    else if (activo == 1)
                    {

                    }
                    else
                    {
                        // Mostrar mensaje de error porque el valor no es válido                 
                    }
                }
                else
                {
                    // Mostrar mensaje de error porque no se encontró ningún registro

                }
            }
        }

        protected void CarteraVencida()
        {
            // Obtener el valor del textbox txtNit
            string nitCliente = txtNit.Text.Trim();

            string consulta = "SELECT edvcliente, edvtipodocuclie, edvnumedocuclie, edvfechexpe, edvformapago, edvfechvenc, edvtotamoneloca, edvsalddoculoca " +
                              "FROM ca_encdocvta " +
                              "WHERE edvcliente = @Nit " +
                              "AND edvsalddocunego > 0 " +
                              "AND edvfechvenc < GETDATE() " +
                              "AND edvsignodocu = 1 " +
                              "ORDER BY edvfechvenc DESC";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSSF].ConnectionString;

            // Crear una lista para almacenar los resultados
            List<CustomObject> listaResultados = new List<CustomObject>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(consulta, connection);

                // Asignar valor al parámetro @Nit
                command.Parameters.AddWithValue("@Nit", nitCliente);

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

            // Verificar si la lista tiene elementos (es decir, si la consulta trajo resultados)
            if (listaResultados.Count > 0)
            {
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
            else
            {
                txtMensaje.Visible = false; ;
            }
        }

        // Los métodos MostrarMensajeExito, MostrarMensaje y MostrarMensajeError permanecen igual como en la respuesta anterior.


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
                BtnAdiObjPla.Enabled = true;
                BtnAdiObjPla.CssClass = "btn btn-sm shadow button-enabled";

                BtnEliObjPla.Enabled = true;
                BtnEliObjPla.CssClass = "btn btn-sm shadow button-enabled";

                BtnVisGenCot.Enabled = true;
                BtnVisGenCot.CssClass = "btn btn-sm shadow button-enabled";

                BtnPlaBlo.Enabled = true;
                BtnPlaBlo.CssClass = "btn btn-sm shadow button-enabled";


                BtnAcaPla.Enabled = true;
                BtnAcaPla.CssClass = "btn btn-sm shadow button-enabled ";
            }


        }

        // VER COTIZACION Y CAMBIO COTIZACION 
        protected void btnCotizacion_Click(object sender, EventArgs e)
        {
            if (txtCotizacion.Text.ToUpper() != "NO TIENE")
            {
                DateTime fechaVentaAño = DateTime.ParseExact(tbVenta.Text, "yyyy-MM-dd", null);
                DateTime fechaVenta = DateTime.ParseExact(tbVenta.Text, "yyyy-MM-dd", null);
                fechaVenta = fechaVenta.AddMonths(1);
                string rutaBase = @"\\172.16.30.6\Recepcion\Cotizaciones Excel\" + ddlZona.SelectedValue + @"";

                for (int i = 0; i <= 6; i++)
                {
                    // Obtener el mes y el año correspondientes
                    DateTime fechaMes = fechaVenta.AddMonths(-i);
                    int mes = fechaMes.Month;
                    int año = fechaMes.Year;

                    // Si estamos en diciembre, retroceder al año anterior
                    if (mes == 12 && i > 0)
                    {
                        int año1 = fechaVentaAño.Year;
                        año1--;
                        año = año1;
                    }

                    // Obtener la abreviatura del nombre del mes
                    string nombreMes = ObtenerNombreMesAbreviado(fechaMes);

                    // Construir la ruta del archivo para este mes
                    string rutaArchivo = Path.Combine(rutaBase, año.ToString(), nombreMes, txtCotizacion.Text + ".xls");

                    // Verificar si el archivo existe
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

                        // Si se encuentra el archivo, salir del bucle
                        break;
                    }
                }


                string mensajeExito = "La Cotización  " + txtCotizacion.Text.Trim() + " no se encuentra en los ultimos 6 meses.";
                string scriptExito = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

            }
        }

        public string ObtenerNombreMesAbreviado(DateTime fecha)
        {
            return fecha.ToString("MMM").TrimEnd('.');
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
            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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

                        double Valorventa = Convert.ToDouble(txtVenta.Text);
                        double Descuento = Convert.ToDouble(txtDcto.Text);
                        double ValorDescuento = Valorventa * Descuento / 100;

                        txtDctoValor.Text = Convert.ToString(ValorDescuento);
                        txtGtotal.Text = (Valorventa - ValorDescuento + Convert.ToDouble(txtVtte.Text) + Convert.ToDouble(txtVvia.Text)).ToString();


                        if (Convert.ToInt32(leer["Saldo"].ToString()) == 0)
                        {
                            txtValorSugerido.Text = "0";
                            txtVccd.Text = "0";
                            txtVcsd.Text = "0";
                            txtSaldo.Text = "0";
                            txtDiseño.Text = "0";
                            txtDcto.Text = "0";
                            txtComision.Text = "0";
                            txtVenta.Text = "0";
                            txtVtte.Text = "0";
                            txtVvia.Text = "0";
                            txtDctoValor.Text = "0";
                            txtGtotal.Text = "0";
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
                txtNit.Text = string.Empty;
                txtNombreEmp.Text = string.Empty;
                txtcontacto.Text = string.Empty;
                txtMail.Text = string.Empty;
                txtDireccion.Text = string.Empty;
                txtMunicipio.Text = string.Empty;
                txtTelefono.Text = string.Empty;

                return false;
            }


        }

        private void CargarDatosContables(string id)
        {


            using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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


            Session["Id_OT"] = Session["Id_OT2"]?.ToString();
            Session["pedido"] = Session["pedido2"]?.ToString();

            try
            {
                using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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

                            EnableButtons2();

                            HabilitarBotonesPlano();
                            // Obtener datos de cotización y asignarlos a controles
                            AssignCotizacionData(id, pedido, txtCotizacion.Text);


                        }
                    }

                }


            }
            catch (Exception ex)
            {
         
            }

            Cargar_Plano(id, pedido);
            Cargar_Despiece_Plano();



            if (Session["NuevoPedido"] != null && (bool)Session["NuevoPedido"])
            {

                BotonesNuevoPedido();

            }

            string valorTextBox = tbObra.Text.Trim(); // Obtener el valor del TextBox

            ddlNumbers.Enabled = true;

            // Guardar el valor en una variable de sesión
            Session["ValorDeObra"] = valorTextBox;

            Session["CargarOTsEjecutada"] = true;


        }

        protected void BotonesNuevoPedido()
        {
            HabilitarTodosLosTextBoxes();

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

            Nit.Enabled = false;
            Nit.CssClass = "btn btn-sm shadow button-disabled";

            tbPedDepen.Enabled = false;

            tbVenta.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dtpFechaEntregaDibujoDespiece.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dtpFechaEntregaProduccion.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime fechaActual = DateTime.Now;
            DateTime fechaMas10Dias = fechaActual.AddDays(10);
            dtpEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");
            dtpRealEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");

            cboPedidoBase.DataBind();
            cboPedidoBase.Items.Insert(0, new ListItem(" "));
            dtacboTipoPedido.DataBind();
            dtacboTipoPedido.Items.Insert(0, new ListItem(" "));

            txtOrdenCompra.Text = string.Empty;


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

        protected void CalcularSaldo()
        {
            DataTable InfoOT = ConsultarInformacionPedidoSaldo();

            foreach (DataRow Row in InfoOT.Rows)
            {
                bool AfectaBola = Convert.ToBoolean(Row["AfectaBolsa"].ToString());
                double ValorBolsa = double.Parse(Row["ValorBolsa"].ToString());
                double ValorPedido = double.Parse(Row["ValorPedido"].ToString());

                if (AfectaBola)
                {
                    TotalObraMas += ValorBolsa;
                    TotalObraMenos += ValorPedido;
                }

            }

            lblSaldoOT.Text = (TotalObraMas - TotalObraMenos).ToString("#,##0");
            double saldo = double.Parse(lblSaldoOT.Text.Replace(",", ""));

            // Asignar el color de fondo dependiendo del valor del saldo
            if (saldo < 0)
            {
                lblSaldoOT.BackColor = System.Drawing.Color.Red;

                if (TotalObraMas > 0)
                {
                    if (Math.Abs(saldo) * 100 / TotalObraMas >= 25)
                    {
                        string mensajeError = "El saldo esta en  un " + Math.Abs(saldo) + " % en contra";
                        string scriptError = "alert('" + mensajeError + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);
                    }
                }

            }
            else
            {
                lblSaldoOT.BackColor = System.Drawing.Color.Lime;
            }

            if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "COMPRAS")
            {
                lblSaldoOT.Visible = true;
                lbsaldo.Visible = true;
            }



        }

        private DataTable ConsultarInformacionPedidoSaldo()
        {

            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblOT.Consecutivo_Pedido,tblTipoPedido.*,tblOT.Terminada_Facturacion,tblOT.Fecha_Factura,tblOT.Precio_Venta," +
                              "tblOT.ValorPedido,tblOT.ValorBolsa,tblOT.Descuento,tblOT.Precio_Venta-tblOT.Descuento*tblOT.Precio_Venta/100 AS Subtotal," +
                              "tblTipoPedido.EstadisticaVenta,tblOT.PedidoBase" +
                              " FROM tblTipoPedido " +
                              "INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido " +
                              "WHERE (((tblOT.Id_OT)=@Id_OT)) ORDER BY tblOT.Consecutivo_Pedido DESC";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@Id_OT", tbOT.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }


            // Retorna la DataTable
            return dataTable;

        }

        protected void NuevoPedido_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#NuevoPedido').modal('show');", true);



            Session["NuevoPedido"] = true;
            Session.Remove("BtnModificarEjecutado");
            Session.Remove("NuevaOTEjecutada");

            Session["Id_OT2"] = tbOT.Text;


        }

        protected void BtnSiNuevoPedido_Click(object sender, EventArgs e)
        {
            BotonesNuevoPedido();
            LimpiarCamposCotizacion();
        }

        protected void BtnNoNuevoPedido_Click(object sender, EventArgs e)
        {
            BotonesNuevoPedido();
            LimpiarCamposCotizacion();

            txObs1.Value = "Altura Total: \r\nLínea: \r\nTipo de Sujeción: \r\nPerfil Refuerzo Superior: \r\nTipo y Color de Sillas: \r\nObservaciones: \r\n\r\nALMACEN:\r\nCORTE: \r\nMOLDURADO: \r\nCARPINTERIA: \r\nTAPIZADO: \r\nENSAMBLE VIDRIO: \r\nENSAMBLE: \r\nEMPAQUE:  ";
            txObs2.Value = "Altura Total: \r\nLínea: \r\nTipo de Sujeción: \r\nPerfil Refuerzo Superior: \r\nTipo y Color de Sillas: \r\nObservaciones: \r\n\r\nALMACEN:\r\nCORTE: \r\nMOLDURADO: \r\nCARPINTERIA: \r\nTAPIZADO: \r\nENSAMBLE VIDRIO: \r\nENSAMBLE: \r\nEMPAQUE:  ";
        }



  private void LimpiarCamposCotizacion()

        {

            txtValorSugerido.Text = "0";

            txtVcsd.Text = "0";

            txtVccd.Text = "0";

            txtDiseño.Text = "0";

            txtComision.Text = "0";

            txtSaldo.Text = "0";

            txtVenta.Text = "0";

            txtDctoValor.Text = "0";

            txtDcto.Text = "0";

            txtVtte.Text = "0";

            txtVvia.Text = "0";

            txtGtotal.Text = "0";

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

        //FIN

        // Logica de tap de plano 

        public void Cargar_Plano(string id, string pedido)
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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

                //se usa Para darle un color a la fila seleccionada  
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


                ObjetoEliminar.InnerText = Descri;
                anchoEliminar.InnerText = Ancho;
            }
        }

        // Se Debe Modificar el procedimienato almacenado
        public void LlenarDataGridObjeto(string idPanelNum)
        {
            string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
                int DespachoInterno = 0;
                int DespachoCoordinado = 0;
                int Terminado_despacho = 0;
                int Entregado_Transporte = 0;

                if (DataBinder.Eval(e.Item.DataItem, "DespachoInterno") != DBNull.Value)
                    DespachoInterno = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DespachoInterno"));

                if (DataBinder.Eval(e.Item.DataItem, "DespachoCoordinado") != DBNull.Value)
                    DespachoCoordinado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DespachoCoordinado"));

                if (DataBinder.Eval(e.Item.DataItem, "Terminado_despacho") != DBNull.Value)
                    Terminado_despacho = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Terminado_despacho"));

                if (DataBinder.Eval(e.Item.DataItem, "Entregado_Transporte") != DBNull.Value)
                    Entregado_Transporte = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Entregado_Transporte"));

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
                        Session.Remove("ClickCount1");
                        Session.Remove("Id_PanelNum_Session1");
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

            Session["Id_OT2"] = Session["Id_OT2"].ToString();

            Session["pedido2"] = ddlNumbers.SelectedItem.Text;

            string url = "FormExtPrin/NitOts.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
        }

        protected void Redireccion_Nit_Click(object sender, EventArgs e)
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

        //REDIRECCION A DOCUMENTACION ORDENES DE TRABAJO 
        protected void DocumentacionOt_Click(object sender, EventArgs e)
        {

            Session["Id_OT2"] = tbOT.Text;
            Session["pedido2"] = ddlNumbers.Text;

            string url = "FormExtPrin/DocumentacionOT.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
        }


        public void CargarVariablesDeSesionContable()
        {
            if (!string.IsNullOrEmpty(Session["IdContactoFactSession"]?.ToString()) && !string.IsNullOrEmpty(Session["IdClienteFactSession"]?.ToString()))
            {
                //Cargar los Datos del cliente 

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
                        MostrarModal();

                        Session.Remove("NuevaOTEjecutada");


                    }
                    catch (Exception ex)
                    {

                    }
                }

                // Verificar si se ha ejecutado el evento BtnModificar_Click
                else if (Session["BtnModificarEjecutado"] != null && (bool)Session["BtnModificarEjecutado"])
                {
                    MostrarModalModificar();

                    Session.Remove("BtnModificarEjecutado");

                }

                else if (Session["NuevoPedido"] != null && (bool)Session["NuevoPedido"])
                {

                    ValidarMesesDesdeUltimaVenta();

                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ErrorPermiso').modal('show');", true);
                }

                Session.Remove("NuevoPedido");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalll').modal('show'); $('#campoFaltante').text('" + campoFaltante + "');", true);
            }

        }

        protected void BtnSiModificar_Click(object sender, EventArgs e)
        {
            ValidarUsuario();
        }

        protected void BtnNoModificar_Click(object sender, EventArgs e)
        {
            Session["Id_OT2"] = tbOT.Text;
            Session["Pedido2"] = ddlNumbers.Text;

            string mensajePersonalizado = "Se cargara nuevamente la OT";
            string urlRedireccion = "OrdenTrabajo.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
        }
        protected void ValidarMesesDesdeUltimaVenta()
        {
            // Obtener el valor del NIT desde el TextBox
            string nit = txtNit.Text.Trim();

            // Verificar que el NIT no esté vacío
            if (!string.IsNullOrEmpty(nit))
            {
                // Utilizar un bloque using para garantizar la liberación de recursos
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    connection.Open();

                    // Crear un nuevo comando SQL
                    using (SqlCommand command = new SqlCommand("SELECT DATEDIFF(MONTH, MAX(ot.Fecha_Confirmacion_Venta), GETDATE()) AS MesesDesdeUltimaVenta " +
                                                                "FROM tblClienteObraContacto coc " +
                                                                "INNER JOIN tblOT ot ON coc.IdContacto = ot.IDContacto_Cliente " +
                                                                "INNER JOIN tblAsesorComercial ac ON ot.Codigo_Asesor = ac.CodigoAsesor " +
                                                                "WHERE coc.cocNIT = @NIT;", connection))
                    {
                        // Añadir parámetro
                        command.Parameters.AddWithValue("@NIT", nit);

                        // Ejecutar la consulta y obtener el resultado
                        object result = command.ExecuteScalar();

                        // Verificar si el resultado no es nulo
                        if (result != null && result != DBNull.Value)
                        {
                            // Convertir el resultado a entero
                            int mesesDesdeUltimaVenta = Convert.ToInt32(result);

                            // Verificar si el valor es menor a 13
                            if (mesesDesdeUltimaVenta < 13)
                            {

                                InsertarNuevoPedido();
                                InsertarPlanoPedido();

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#ActualizarCliente').modal('show');", true);
                            }
                        }
                    }
                }
            }
            else
            {
                //MODAL VACIO
            }
        }

        protected void InsertarPlanoPedido()
        {
            string idOT = Session["Id_OT3"].ToString();
            string pedido = Session["pedido3"].ToString();
            string nombreUsuario = Session["usuariologueado"].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
                    ObtenerInfoOt3();

                    string contenidoModalOT = "Se agrego el pedido " + pedido + " A la Orden de trabajo " + idOT;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#PedidoIngresado').modal('show'); $('#PedidoIngresado2').text('" + contenidoModalOT + "');", true);

                


                    Session["Id_OT"] = Session["Id_OT3"]?.ToString();
                    Session["pedido"] = Session["pedido3"]?.ToString();

                }
                else
                {
                    // Ocurrió un problema al realizar la inserción
                }
            }
        }

        protected void InsertarNuevoPedido()
        {

            string IDCLienteConstactoSession = Session["IdContactoFactSession"] as string;


            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string[] valoresDDL = ddlCiudad.SelectedValue.Split('-');

                if (valoresDDL.Length == 2)
                {
                    string ciudadSeleccionada = valoresDDL[0].Trim();
                    string regionSeleccionada = valoresDDL[1].Trim();

                    int nuevoConsecutivo = ObtenerConsecutivoPedido();


                    string idOTn = tbOT.Text;

                    int pedidoBaseValue = ObtenerPedidoBaseValue();



                    using (SqlCommand command = new SqlCommand("sp_InsertarDatos_TBLOT", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Asignar el valor generado para @Id_OT
                        command.Parameters.AddWithValue("@Id_OT", idOTn);
                        command.Parameters.AddWithValue("@Consecutivo_Pedido", nuevoConsecutivo);


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

                        command.Parameters.AddWithValue("@IDContacto_Cliente", IDCLienteConstactoSession);

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
                        command.Parameters.AddWithValue("@Id_OT_secundario", tbOT.Text);
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



                        command.Parameters.AddWithValue("@PedidoBase", pedidoBaseValue);


                        command.Parameters.AddWithValue("@ValorViatico", txtVvia.Text);
                        command.Parameters.AddWithValue("@Fecha_Empaque", dtpEmpaque.Text);
                        command.Parameters.AddWithValue("@Fecha_Real_Empaque", dtpRealEmpaque.Text);
                        command.Parameters.AddWithValue("@OrdendeCompra", txtOrdenCompra.Text);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Session["Id_OT3"] = idOTn;
                            Session["pedido3"] = nuevoConsecutivo;



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

        private int ObtenerPedidoBaseValue()
        {
            int pedidoBaseValue = 1; // Valor predeterminado

            // Obtener el valor de ObtenerConsecutivoPedido y sumar 1
            int consecutivoPedido = ObtenerConsecutivoPedido();

            // Verificar si cboPedidoBase tiene datos y si el valor es un número
            if (!string.IsNullOrEmpty(cboPedidoBase.SelectedValue) && int.TryParse(cboPedidoBase.SelectedValue, out int cboValue))
            {
                pedidoBaseValue = cboValue;
            }
            else
            {
                if (consecutivoPedido != 0)
                {
                    pedidoBaseValue = consecutivoPedido;
                }
                else
                {

                }
            }

            return pedidoBaseValue;
        }


        protected int ObtenerConsecutivoPedido()
        {
            int nuevoConsecutivo = 0;

            // Utilizar un bloque using para garantizar la liberación de recursos
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();

                // Crear un nuevo comando SQL
                using (SqlCommand command = new SqlCommand("SELECT MAX(Consecutivo_Pedido) FROM tblOT WHERE Id_OT = LTRIM(RTRIM(@Id_OT));", connection))
                {
                    // Añadir parámetro
                    command.Parameters.AddWithValue("@Id_OT", tbOT.Text);

                    // Ejecutar la consulta y obtener el resultado
                    object result = command.ExecuteScalar();

                    // Verificar si el resultado no es nulo
                    if (result != null && result != DBNull.Value)
                    {
                        // Convertir el resultado a entero
                        nuevoConsecutivo = Convert.ToInt32(result) + 1;
                    }
                }
            }

            return nuevoConsecutivo;
        }

        protected void InsertarOT()
        {
            string IDCLienteConstactoSession = Session["IdContactoFactSession"] as string;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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

                            int pedidoBaseValue = ObtenerPedidoBaseValue();


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
                                command.Parameters.AddWithValue("@IDContacto_Cliente", IDCLienteConstactoSession);
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

                                command.Parameters.AddWithValue("@PedidoBase", pedidoBaseValue);

                                command.Parameters.AddWithValue("@ValorViatico", txtVvia.Text);
                                command.Parameters.AddWithValue("@Fecha_Empaque", dtpEmpaque.Text);
                                command.Parameters.AddWithValue("@Fecha_Real_Empaque", dtpRealEmpaque.Text);
                                command.Parameters.AddWithValue("@OrdendeCompra", txtOrdenCompra.Text);

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Session["Id_OT2"] = nuevoIdOTConcatenado;
                                    Session["Pedido2"] = 1;
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

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string idOT = Session["Id_OT2"].ToString();
            string pedido = Session["Pedido2"].ToString();
            string nombreUsuario = Session["usuariologueado"].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string idOT = Session["Id_OT2"].ToString();
            string contenidoModalOT = "la Orden de trabajo: " + idOT + " queda asignada a la Obra: " + tbObra.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#OTingresada').modal('show'); $('#OTingresada2').text('" + contenidoModalOT + "');", true);

        }

        protected void MostrarModalModificar()
        {

            string contenidoModalOT = "Esta seguro de modificar la Orden de trabajo " + tbOT.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#OTModificada').modal('show'); $('#OTModificada1').text('" + contenidoModalOT + "');", true);

        }

        protected void MonstrasrModalAcabados_Click(object sender, EventArgs e)
        {
            if (Session["CopiarInfOTEjecutada"] != null && (bool)Session["CopiarInfOTEjecutada"])
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#CopiarAcabados').modal('show');", true);

            }
            else
            {
                string mensajePersonalizado = "Se guardaron los datos exitosamente";
                string urlRedireccion = "OrdenTrabajo.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

            }

            Session.Remove("CopiarInfOTEjecutada");

        }

        private string ValidarCampos()
        {
            string campoFaltante = string.Empty;

            if (dtacboTipoPedido.SelectedItem.Value == " ")
            {
                campoFaltante = "Tipo de pedido";
            }
            else if (DtaCboTipoAprobacion.SelectedItem.Value == " ")
            {
                campoFaltante = "Tipo de aprobacion";
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
            else if (ddlCiudad.SelectedItem.Value == " ")
            {
                campoFaltante = "Ciudad";
            }
            else if (string.IsNullOrEmpty(dtpEmpaque.Text))
            {
                campoFaltante = "Empaque";
            }
            else if (ddlFabrica1.SelectedItem.Value == " ")
            {
                campoFaltante = "Fabrica";
            }
            else if (ddlInstala.SelectedItem.Value == " ")
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
            else if (ddlAsesor.SelectedItem.Value == " ")
            {
                campoFaltante = "Asesor";
            }
            else if (string.IsNullOrEmpty(TextTNegociacion.InnerText))
            {
                campoFaltante = "Tipo de Negociacion";
            }

            else if (cboPedidoBase.Enabled)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(cboPedidoBase.SelectedValue, @"\d"))
                {
                    campoFaltante = "Pedido Base";
                }
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(ddlCiudad.SelectedValue, @"\D"))
            {
                campoFaltante = "Ciudad";
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
                        string mensajePersonalizado = "Se modifico exitosamente la Orden de trabajo";
                        string urlRedireccion = "OrdenTrabajo.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }

                    reader.Close();
                }
            }
        }

        protected void ActualizarDatos()
        {
            string IDCLienteConstactoSession = Session["IdContactoFactSession"] as string;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
                        command.Parameters.AddWithValue("@IDContacto_Cliente", IDCLienteConstactoSession);
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
                            Session["Id_OT2"] = tbOT.Text;
                            Session["Pedido2"] = ddlNumbers.Text;

                            string mensajePersonalizado = "Se modifico exitosamente la Orden de trabajo";
                            string urlRedireccion = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                        else
                        {
                            string mensajePersonalizado = "No Se modifico exitosamente la Orden de trabajo";
                            string urlRedireccion = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                    }
                }
            }

        }

        protected void BtnModificar_Click(object sender, EventArgs e)
        {
            Session["BtnModificarEjecutado"] = true;

            Session.Remove("NuevaOTEjecutada");
            Session.Remove("NuevoPedido");

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

            Nit.Enabled = false;

            cbxComisionCompart.Enabled = true;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            Session["Id_OT2"] = tbOT.Text;
         
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


            TiposDePedidos.SelectCommand = "SELECT Descripcion_TipoPedido, Id_TipoPedido, EstadisticaVenta FROM tblTipoPedido WHERE Activo = '1' AND EstadisticaVenta = '1' ORDER BY Descripcion_TipoPedido";


            dtacboTipoPedido.DataBind();
            dtacboTipoPedido.Items.Insert(0, new ListItem(" "));

            txtOrdenCompra.Text = string.Empty;

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
            Session.Remove("BtnModificarEjecutado");
            Session.Remove("NuevoPedido");

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

            tbVenta.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dtpFechaEntregaDibujoDespiece.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dtpFechaEntregaProduccion.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime fechaActual = DateTime.Now;
            DateTime fechaMas10Dias = fechaActual.AddDays(10);
            dtpEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");
            dtpRealEmpaque.Text = fechaMas10Dias.ToString("yyyy-MM-dd");

            LimpiarCamposCotizacion();

        }

        protected void BtnSi_Click(object sender, EventArgs e)
        {
            string id = Session["Id_OT"]?.ToString();
            string pedido = Session["pedido"]?.ToString();

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pedido))
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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

            string mensajePersonalizado = "Se guardaron los datos exitosamente";
            string urlRedireccion = "OrdenTrabajo.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


        }

        protected void RealizarInserciones()
        {
            string OTinsertada = Session["Id_OT2"]?.ToString();
            string PedidoInsertado = Session["Pedido2"]?.ToString();

            if (!string.IsNullOrEmpty(OTinsertada) && !string.IsNullOrEmpty(PedidoInsertado))
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
                        if (rowsAffected <= 0)
                        {
                            // Si alguna inserción falla, detenemos el proceso y mostramos un mensaje de error
                            string mensajePersonalizado2 = "No fue posible realizar copiar los acabados";
                            string urlRedireccion2 = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado2)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion2)}");
                            return; // Salir del método para evitar más intentos de inserción
                        }
                    }

                    // Si todas las inserciones fueron exitosas, redireccionamos con un mensaje de éxito
                    string mensajePersonalizado = "Se insertaron correctamente los acabados de la OT copiada";
                    string urlRedireccion = "OrdenTrabajo.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
        }



        protected void CargarOtInsertada3()
        {

            id = Session["Id_OT3"]?.ToString();
            pedido = Session["pedidoMax"]?.ToString();
            try
            {
                using (SqlConnection sqlconectar = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
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

                                Session["IdContactoFactSession"] = IDCLienteConstacto;

                            }

                            // Extraer datos y asignarlos a controles
                            AssignDataToControls(leer);

                            EnableButtons3();

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

            tbPedDepen.Enabled = true;
            tbPedDepen.CssClass = "form-control";

            string valorTextBox = tbObra.Text.Trim(); // Obtener el valor del TextBox

            // Guardar el valor en una variable de sesión
            Session["ValorDeObra"] = valorTextBox;

            Session["CargarOTsEjecutada"] = true;

            NuevaOt.Enabled = true;
            NuevaOt.CssClass = "btn btn-sm shadow button-enabled";

            ModificarOt.Enabled = true;
            ModificarOt.CssClass = "btn btn-sm shadow button-enabled";

            AnularPedido.Enabled = true;
            AnularPedido.CssClass = "btn btn-sm shadow button-enabled";

            ObservacionesOt.Enabled = true;
            ObservacionesOt.CssClass = "btn btn-sm shadow button-enabled";

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

        //***** ADICIONAR  OBJETOS AL PLANO *****
        protected void BtnAdiObjPla_Click(object sender, EventArgs e)
        {
            // Se valida  que el plano este o no bloqueado
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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



        //***** QUITAR UN OBJETO DEL PLANO  *****
        protected void QuitarObjeto_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#EliminarObjeto').modal('show');", true);
        }
        protected void BtnQuiObjPla_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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



        //*****ELIMINAR TODOS LOS OBJETOS DEL PLANO  *****

        protected void EliminarObjetos_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "actualizarPlanoEliminar", "actualizarPlanoEliminar();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#EliminarObjetos').modal('show');", true);
        }
        protected void BtnEliObjPla_Click(object sender, EventArgs e)
        {
            // Se valida  que el plano este o no bloqueado
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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



        //***** MOSTRAR ACABADOS DEL PLANO   ******
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

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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



        //*****  Incio Boton Leer Autocad Pendiente Implementacion ******
        protected void BtnLeeArcDesAca_Click(object sender, EventArgs e)
        {

            //Variables de Session para volver a cargar el plano
            Session["Id_OT2"] = tbOT.Text;
            Session["pedido2"] = ddlNumbers.Text;

            // Consultamos que el plano no este bloqueado o ya este ligado a un pedido o afecta alguna bolsa 

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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



        //***** DESBLOQUEAR O BLOQUEAR UN PLANO******
        protected void BtnPlaBlo_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

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

        // DESCARGAR LA COTIZACION EN EL TAP DE PLANO 
        protected void BtnVisGenCot_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos un paquete de Excel 
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregamos la hoja  1
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Cotizacion");

                    // Altura de Filas y columnas 
                    worksheet.Row(1).Height = 60;
                    worksheet.Column(2).Width = 40;
                    worksheet.Column(5).Width = 15;
                    worksheet.Column(3).Width = 15;
                    worksheet.Column(4).Width = 15;
                    worksheet.Column(7).Width = 15;
                
                    // Fecha del día
                    DateTime fechaActual = DateTime.Now;
                    string nombreMes = fechaActual.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                    int diaActual = fechaActual.Day;
                    int añoActual = fechaActual.Year;

                    // Logo Ducon       // validar la ruta de este logo y no se debe eliminar 
                    string rutaImagen = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                    FileInfo image = new FileInfo(rutaImagen);
                    if (image.Exists)
                    {
                        var picture = worksheet.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    // Sede y Fecha del día
                    string tableTitle = "Sabaneta, " + nombreMes + " " + diaActual + " de " + añoActual;
                    var titleRange = worksheet.Cells["B2"];
                    titleRange.Value = tableTitle;
                    titleRange.Style.Font.Name = "Century Gothic";
                    titleRange.Style.Font.Size = 11;
                    titleRange.Style.Font.Bold = true;
                    titleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Cotización
                    string titleCot = "Cotización Nº";
                    var CellE1 = worksheet.Cells["E1"];
                    CellE1.Value = titleCot;
                    CellE1.Style.Font.Name = "Century Gothic";
                    CellE1.Style.Font.Size = 11;
                    CellE1.Style.Font.Bold = true;
                    CellE1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //Id Plano 
                    var CellE2 = worksheet.Cells["E2"];
                    CellE2.Value = txtPlano.Text;
                    CellE2.Style.Font.Name = "Century Gothic";
                    CellE2.Style.Font.Size = 11;
                    CellE2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Dirigido a
                    var CellB4 = worksheet.Cells["B4"];
                    CellB4.Value = "Señores";
                    CellB4.Style.Font.Name = "Century Gothic";
                    CellB4.Style.Font.Size = 11;
                    CellB4.Style.Font.Bold = true;
                    CellB4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Contacto
                    var CellB5 = worksheet.Cells["B5"];
                    CellB5.Value = txtContactoPlano.Text;
                    CellB5.Style.Font.Name = "Century Gothic";
                    CellB5.Style.Font.Size = 11;
                    CellB5.Style.Font.Bold = true;
                    CellB5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Cliente 
                    var CellB6 = worksheet.Cells["B6"];
                    CellB6.Value = txtCliente.Text;
                    CellB6.Style.Font.Name = "Century Gothic";
                    CellB6.Style.Font.Size = 11;
                    CellB6.Style.Font.Bold = true;
                    CellB6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Ciudad 
                    var CellB7 = worksheet.Cells["B7"];
                    CellB7.Value = "Ciudad";
                    CellB7.Style.Font.Name = "Century Gothic";
                    CellB7.Style.Font.Size = 11;
                    CellB7.Style.Font.Bold = true;
                    CellB7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB7.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var CellB9 = worksheet.Cells["B9"];
                    CellB9.Value = "Ref. " + txtArea.Text;
                    CellB9.Style.Font.Name = "Century Gothic";
                    CellB9.Style.Font.Size = 11;
                    CellB9.Style.Font.Bold = true;
                    CellB9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB9.Style.Font.Color.SetColor(System.Drawing.Color.Black);


                    // Saludos 
                    var CellB11 = worksheet.Cells["B11:F11"];
                    CellB11.Merge = true;
                    CellB11[11, 2].Value = "Atendiendo su amable solicitud con gusto presentamos cotización de las partes y ";
                    CellB11.Style.Font.Name = "Century Gothic";
                    CellB11.Style.Font.Size = 11;
                    CellB11.Style.Font.Bold = true;
                    CellB11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB11.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var CellB12 = worksheet.Cells["B12:F12"];
                    CellB12.Merge = true;
                    CellB12[12, 2].Value = "elementos del  sistema  Modular  Ducon SMD, en nuestra línea 3500, tal como sigue ";
                    CellB12.Style.Font.Name = "Century Gothic";
                    CellB12.Style.Font.Size = 11;
                    CellB12.Style.Font.Bold = true;
                    CellB12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB12.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    // Comienzo de encabezados 
                    int headerIndex = 2;
                    int rowIndex = 14;

                    // Encabezados de la tabla del despiece del plano 
                    string[] encabezados = { "DESCRIPCIÓN", "Ancho (Cms)", "CANT", "Valor Und", "Total", "Imagen", "", "", "Cubicaje", "Peso" };

                    foreach (string encabezado in encabezados)
                    {
                        worksheet.Cells[rowIndex, headerIndex].Value = encabezado;
                        var headerCell = worksheet.Cells[rowIndex, headerIndex];

                        // Establecemos el texto en negrita
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Font.Name = "Century Gothic";

                        // Aplicamos bordes a la celda de encabezado
                        headerCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        headerIndex++;
                    }

                    // Zona
                    var CellB16 = worksheet.Cells["B15"];
                    CellB16.Value = "Zona";
                    CellB16.Style.Font.Name = "Century Gothic";
                    CellB16.Style.Font.Size = 11;
                    CellB16.Style.Font.Bold = true;
                    CellB16.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB16.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    rowIndex++;

                    var Ref = worksheet.Cells[$"B{rowIndex}:F{rowIndex}"];
                    Ref.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Ref.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkGray);
                    Ref.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    var border = Ref.Style.Border;
                    border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    // Trabajo
                    var CellB17 = worksheet.Cells["B16"];
                    CellB17.Value = txtArea.Text;
                    CellB17.Style.Font.Name = "Century Gothic";
                    CellB17.Style.Font.Size = 11;
                    CellB17.Style.Font.Bold = true;
                    CellB17.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB17.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    rowIndex++;

                    var zona = worksheet.Cells[$"B{rowIndex}:F{rowIndex}"];
                    zona.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    zona.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkGray);
                    zona.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    var border1 = zona.Style.Border;
                    border1.Top.Style = border1.Bottom.Style = border1.Left.Style = border1.Right.Style = ExcelBorderStyle.Thin;

                    var CellF15 = worksheet.Cells["F15"];
                    CellF15.Value = "Plano ";
                    CellF15.Style.Font.Name = "Century Gothic";
                    CellF15.Style.Font.Size = 11;
                    CellF15.Style.Font.Bold = true;
                    CellF15.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellF15.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var cellF16 = worksheet.Cells["F16"];
                    cellF16.Value = txtPlano.Text;
                    cellF16.Style.Font.Name = "Century Gothic";
                    cellF16.Style.Font.Size = 11;
                    cellF16.Style.Font.Bold = true;
                    cellF16.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cellF16.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    rowIndex++;
                    rowIndex++;

                    // Se cargan los datos del despiece del plano en una lista 
                    List<DatosFiltrados> datosFiltradosList = CargarDatosExcel();
                    Dictionary<string, string> descripcionesPlano = ObtenerDescripcionesPlano(txtPlano.Text);
                    List<string> descripcionesAsignadas = new List<string>();

                    foreach (var datosFiltrados in datosFiltradosList)
                    {
                        if (datosFiltrados.Tipo == "Titulo")
                        {
                            // Combinar celdas para el título
                            var titleRange1 = worksheet.Cells[$"B{rowIndex}:F{rowIndex}"];
                            titleRange1.Merge = true;

                            // Puedes ajustar el color de fondo según tu preferencia
                            titleRange1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleRange1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            titleRange1.Style.Font.Bold = true;

                            titleRange1.Style.Font.Name = "Century Gothic";
                            titleRange1.Style.Font.Size = 11;
                            titleRange1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            titleRange1.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            titleRange1.Style.WrapText = true;
                            titleRange1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            titleRange1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            titleRange1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            titleRange1.Style.Border.Right.Style = ExcelBorderStyle.Thin;


                            string titulo = datosFiltrados.Titulo.ToString();
                            if (descripcionesPlano.ContainsKey(titulo))
                            {
                                string descripcion = descripcionesPlano[titulo];

                                if (!string.IsNullOrEmpty(descripcion))
                                {
                                    // Si la descripción no está vacía, aplicar formato y ajustar la altura
                                    worksheet.Row(rowIndex).Height = 60;
                                    string tituloConDescripcion = $"{datosFiltrados.Titulo}\n{descripcion}";
                                    titleRange1[rowIndex, 2].Value = tituloConDescripcion;
                                }
                                else
                                {
                                    worksheet.Row(rowIndex).Height = 30;
                                    titleRange1[rowIndex, 2].Value = datosFiltrados.Titulo;
                                }


                            }
                            rowIndex++;
                        }
                        else if (datosFiltrados.Tipo == "Total")
                        {
                            var totalTitleRange = worksheet.Cells[$"B{rowIndex}:E{rowIndex}"];
                            var totalSubtotalRange = worksheet.Cells[$"F{rowIndex}"];

                            totalTitleRange.Merge = true;
                            totalTitleRange.Style.Font.Bold = true;
                            totalTitleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            totalTitleRange.Style.Font.Name = "Century Gothic";
                            totalTitleRange.Style.Font.Size = 11;

                            totalTitleRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            totalTitleRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            totalTitleRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            totalTitleRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            if (!string.IsNullOrEmpty(datosFiltrados.Titulo))
                            {
                                totalTitleRange[rowIndex, 2].Value = datosFiltrados.Titulo;
                                totalTitleRange.Style.Font.Bold = true;
                                totalTitleRange.Style.Font.Name = "Century Gothic";
                                totalTitleRange.Style.Font.Size = 11;
                                totalTitleRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                totalTitleRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                totalTitleRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                            else
                            {
                                totalTitleRange[rowIndex, 2].Value = "Subtotal";

                            }



                            rowIndex++;
                            totalSubtotalRange.Value = datosFiltrados.SubTotal;
                            totalSubtotalRange.Style.Font.Bold = true;
                            totalSubtotalRange.Style.Font.Name = "Century Gothic";
                            totalSubtotalRange.Style.Font.Size = 11;
                            totalSubtotalRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            totalSubtotalRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            totalSubtotalRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            rowIndex++;
                        }
                        else
                        {
                            // Validar si existe una imagen en la ruta especificada
                            string imagePath = Path.Combine(@"\\172.16.30.6\Dibujo\DUCON\ONLINE\Dropbox\BLOQUES\IMAGENES", $"{datosFiltrados.Id_Panel}.jpg");

                            if (File.Exists(imagePath))
                            {
                                // Insertar la imagen en la celda G
                                var picture = worksheet.Drawings.AddPicture($"Imagen_{rowIndex}", new FileInfo(imagePath));
                                picture.SetPosition(rowIndex - 1, 15, 6, 15); // Fila, FilaOffset, Columna, ColumnaOffset
                                picture.SetSize(55, 55);

                                worksheet.Row(rowIndex).Height = 60;
                            }

                            string Descrip = datosFiltrados.Descripcion;

                            if (!descripcionesAsignadas.Contains(Descrip))
                            {
                                descripcionesAsignadas.Add(Descrip);
                                var cellDescripcion = worksheet.Cells[rowIndex, 2];
                                cellDescripcion.Value = datosFiltrados.Descripcion;
                                cellDescripcion.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                cellDescripcion.Style.WrapText = true;
                                cellDescripcion.Style.Font.Name = "Century Gothic";
                                cellDescripcion.Style.Font.Size = 11;
                            }

                            var cellRange = worksheet.Cells[$"B{rowIndex}:F{rowIndex}"];
                            cellRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            cellRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            cellRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            cellRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            var ancho = worksheet.Cells[rowIndex, 3];
                            ancho.Value = Convert.ToDouble(datosFiltrados.Ancho);
                            ancho.Style.Font.Name = "Century Gothic";
                            ancho.Style.Font.Size = 11;
                            ancho.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                            var Cantidad = worksheet.Cells[rowIndex, 4];
                            Cantidad.Value = Convert.ToDouble(datosFiltrados.Cantidad);
                            Cantidad.Style.Font.Name = "Century Gothic";
                            Cantidad.Style.Font.Size = 11;
                            Cantidad.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                            var ValorUnd = worksheet.Cells[rowIndex, 5];
                            ValorUnd.Value = Convert.ToDouble(datosFiltrados.ValorUnd);
                            ValorUnd.Style.Font.Name = "Century Gothic";
                            ValorUnd.Style.Font.Size = 11;
                            ValorUnd.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                            var SubTotal = worksheet.Cells[rowIndex, 6];
                            SubTotal.Value = Convert.ToDouble(datosFiltrados.SubTotal);
                            SubTotal.Style.Font.Name = "Century Gothic";
                            SubTotal.Style.Font.Size = 11;
                            SubTotal.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                            var peso = worksheet.Cells[rowIndex, 11];
                            peso.Value = Convert.ToDouble(datosFiltrados.peso) * Convert.ToDouble(datosFiltrados.Cantidad);
                            peso.Style.Font.Name = "Century Gothic";
                            peso.Style.Font.Size = 11;

                            var Cubicaje = worksheet.Cells[rowIndex, 10];
                            double valorCub = (Convert.ToDouble(datosFiltrados.Cantidad) * (Convert.ToDouble(datosFiltrados.Ancho) * Convert.ToDouble(datosFiltrados.Altura) * Convert.ToDouble(datosFiltrados.profundidad)) / 1000000);
                            Cubicaje.Value = valorCub;
                            Cubicaje.Style.Font.Name = "Century Gothic";
                            Cubicaje.Style.Font.Size = 11;

                            var cellImageRange = worksheet.Cells[rowIndex, 7];
                            cellImageRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            cellImageRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            cellImageRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                        }

                    }


                    var Cub = worksheet.Cells["B" + rowIndex];
                    Cub.Value = "Cubicaje Aproximado del plano ";
                    Cub.Style.Font.Name = "Century Gothic";
                    Cub.Style.Font.Size = 11;
                    Cub.Style.Font.Bold = true;
                    Cub.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    string formulaSuma = "=ROUND(SUM(J18:J150) + SUM(J18:J150) * 0.2, 3)";
                    var ValorCub = worksheet.Cells["D" + rowIndex];
                    ValorCub.Formula = formulaSuma;
                    ValorCub.Style.Font.Name = "Arial";
                    ValorCub.Style.Font.Size = 11;
                    ValorCub.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                    // Agregamos la hoja numero 2 
                    ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets.Add("Cotizacion Detallada");

                    // Altura de filas y columnas 
                    worksheet2.Row(1).Height = 60;
                    worksheet2.Column(2).Width = 40;
                    worksheet2.Column(5).Width = 15;
                    worksheet2.Column(3).Width = 15;
                    worksheet2.Column(4).Width = 15;
                    worksheet2.Column(7).Width = 15;



                    if (image.Exists)
                    {
                        var picture = worksheet2.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    // Sede y Fecha del día
                    string tableTitle2 = "Sabaneta, " + nombreMes + " " + diaActual + " de " + añoActual;
                    var titleRange2 = worksheet2.Cells["B2"];
                    titleRange2.Value = tableTitle2;
                    titleRange2.Style.Font.Name = "Century Gothic";
                    titleRange2.Style.Font.Size = 11;
                    titleRange2.Style.Font.Bold = true;
                    titleRange2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Cotización
                    string titleCot2 = "Cotización Nº";
                    var Cell2E1 = worksheet2.Cells["E1"];
                    Cell2E1.Value = titleCot2;
                    Cell2E1.Style.Font.Name = "Century Gothic";
                    CellE1.Style.Font.Size = 11;
                    Cell2E1.Style.Font.Bold = true;
                    Cell2E1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var Cell2B4 = worksheet2.Cells["B4"];
                    Cell2B4.Value = "Señores";
                    Cell2B4.Style.Font.Name = "Century Gothic";
                    Cell2B4.Style.Font.Size = 11;
                    Cell2B4.Style.Font.Bold = true;
                    Cell2B4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;





                    // Ciudad 
                    var Cell2B7 = worksheet2.Cells["B7"];
                    Cell2B7.Value = "Ciudad";
                    Cell2B7.Style.Font.Name = "Century Gothic";
                    Cell2B7.Style.Font.Size = 11;
                    Cell2B7.Style.Font.Bold = true;
                    Cell2B7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell2B7.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var Cell2B9 = worksheet2.Cells["B9"];
                    Cell2B9.Value = "Ref. " + txtArea.Text;
                    Cell2B9.Style.Font.Name = "Century Gothic";
                    Cell2B9.Style.Font.Size = 11;
                    Cell2B9.Style.Font.Bold = true;
                    Cell2B9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell2B9.Style.Font.Color.SetColor(System.Drawing.Color.Black);


                    // Saludos 
                    var Cell2B11 = worksheet2.Cells["B11:F11"];
                    Cell2B11.Merge = true;
                    Cell2B11[11, 2].Value = "Atendiendo su amable solicitud con gusto presentamos cotización de las partes y ";
                    Cell2B11.Style.Font.Name = "Century Gothic";
                    Cell2B11.Style.Font.Size = 11;
                    Cell2B11.Style.Font.Bold = true;
                    Cell2B11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell2B11.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var Cell2B12 = worksheet2.Cells["B12:F12"];
                    Cell2B12.Merge = true;
                    Cell2B12[12, 2].Value = "elementos del  sistema  Modular  Ducon SMD, en nuestra línea 3500, tal como sigue ";
                    Cell2B12.Style.Font.Name = "Century Gothic";
                    Cell2B12.Style.Font.Size = 11;
                    Cell2B12.Style.Font.Bold = true;
                    Cell2B12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell2B12.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    int headerIndex2 = 2;
                    int rowIndex2 = 14;


                    foreach (string encabezado in encabezados)
                    {
                        worksheet2.Cells[rowIndex2, headerIndex2].Value = encabezado;
                        var headerCell2 = worksheet2.Cells[rowIndex2, headerIndex2];

                        // Se Establece el texto en negrita
                        headerCell2.Style.Font.Bold = true;
                        headerCell2.Style.Font.Name = "Century Gothic";

                        // Aplicamos bordes a la celda de encabezado
                        headerCell2.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCell2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCell2.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCell2.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        headerIndex2++;
                    }

                    // Agregamos la hoja 3 

                    ExcelWorksheet worksheet3 = excelPackage.Workbook.Worksheets.Add("Condiciones Comerciales");

                    worksheet3.Column(2).Width = 50;
                    worksheet3.Column(4).Width = 30;

                    var Cell3B3 = worksheet3.Cells["B3"];
                    Cell3B3.Value = "CONDICIONES GENERALES DE VENTA:";
                    Cell3B3.Style.Font.Name = "Century Gothic";
                    Cell3B3.Style.Font.Size = 11;
                    Cell3B3.Style.Font.Bold = true;
                    Cell3B3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B3.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    // TIEMPO DE ENTREGA
                    var Cell3B5 = worksheet3.Cells["B5"];
                    Cell3B5.Value = "1. TIEMPO DE ENTREGA:";
                    Cell3B5.Style.Font.Name = "Century Gothic";
                    Cell3B5.Style.Font.Size = 11;
                    Cell3B5.Style.Font.Bold = true;
                    Cell3B5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B5.Style.Font.Color.SetColor(System.Drawing.Color.Black);

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

                    AgregarTextoDesdeArray(worksheet3, tiempoDeEntrega, "B6", 11, false);

                    // INSTALACIÓN
                    var Cell3B18 = worksheet3.Cells["B18"];
                    Cell3B18.Value = "2. INSTALACIÓN:";
                    Cell3B18.Style.Font.Name = "Century Gothic";
                    Cell3B18.Style.Font.Size = 11;
                    Cell3B18.Style.Font.Bold = true;
                    Cell3B18.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B18.Style.Font.Color.SetColor(System.Drawing.Color.Black);

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
                    var Cell3B29 = worksheet3.Cells["B29"];
                    Cell3B29.Value = "3. OBSERVACIONES GENERALES:";
                    Cell3B29.Style.Font.Name = "Century Gothic";
                    Cell3B29.Style.Font.Size = 11;
                    Cell3B29.Style.Font.Bold = true;
                    Cell3B29.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B29.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string[] observacionesGenerales = {
                    "Para garantizar la entrega a satisfacción del proyecto El cliente debe garantizar:",
                    "*  Cielos terminados",
                    "*  Paredes estucadas y pintadas",
                    "*  Pisos pulidos y brillados",
                    "*  Ventanería instalada",
                    "*  Luminarias instaladas y funcionando",
                    "*  Obra libre de escombros",
                    " ",
                    "Una  vez  entregado  el  material estará bajo la responsabilidad del cliente, este deberá",
                    "proveer de un lugar con condiciones de higiene y seguridad  adecuadas para ",
                    "el  producto."
                };
                    AgregarTextoDesdeArray(worksheet3, observacionesGenerales, "B30", 12, false);

                    //  NOTAS
                    string[] NotaImportante = {
                    "Importante: Al recibir su pedido,  revise que las cantidades y el estado de la mercancía",
                    "coincidan con la remisión y no presenten averías.",
                    "Si detecta deterioro de la mercancía o faltantes, agradecemos  dejar constancia en la",
                    "remisión y notificar a su coordinador logístico."
                };
                    AgregarTextoDesdeArray(worksheet3, NotaImportante, "B42", 12, true);


                    var Cell3B47 = worksheet3.Cells["B47"];
                    Cell3B47.Value = "Nota 1:";
                    Cell3B47.Style.Font.Name = "Century Gothic";
                    Cell3B47.Style.Font.Size = 11;
                    Cell3B47.Style.Font.Bold = true;
                    Cell3B47.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B47.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string[] Nota1 = {
                    "Para  garantizar el funcionamiento adecuado del producto  recomendamos",
                    "que la instalación cableado estructurado  voz  y  datos, se realice por un experto.",
                    "obedeciendo indicaciones mínimas del personal de instalación DUCON"

                };
                    AgregarTextoDesdeArray(worksheet3, Nota1, "B48", 12, false);

                    var Cell3B52 = worksheet3.Cells["B52"];
                    Cell3B52.Value = "Nota 2:";
                    Cell3B52.Style.Font.Name = "Century Gothic";
                    Cell3B52.Style.Font.Size = 11;
                    Cell3B52.Style.Font.Bold = true;
                    Cell3B52.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B52.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string[] Nota2 = {
                    "Se debe tener especial cuidado con Las pantallas  y separadores en vidrio de puestos",
                    "de trabajo, debido a que pueden fisurarse si son golpeadas o sometidas a presión excesiva al ",
                    "recostarse en ellas. DUCON S.A.S. no se hace responsable por daños o perjuicios  por este tipo",
                    "de eventos."

                };
                    AgregarTextoDesdeArray(worksheet3, Nota2, "B53", 12, false);

                    var Cell3B58 = worksheet3.Cells["B58"];
                    Cell3B58.Value = "4. COORDINACIÓN:";
                    Cell3B58.Style.Font.Name = "Century Gothic";
                    Cell3B58.Style.Font.Size = 11;
                    Cell3B58.Style.Font.Bold = true;
                    Cell3B58.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B58.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string[] coordinacion = {
                    "Para  nosotros  es  importante  que  usted  este informado  en todo ",
                    "momento  del  estado de  su  pedido, por lo tanto además del ejecutivo de proyecto, usted ",
                    "cuenta con un coordinador logístico que le será asignado por la compañía y se pondrá ",
                    "en contacto con usted durante la ejecución del proyecto."

                };
                    AgregarTextoDesdeArray(worksheet3, coordinacion, "B59", 12, false);

                    var Cell3B65 = worksheet3.Cells["B65"];
                    Cell3B65.Value = "5. GARANTÍA:";
                    Cell3B65.Style.Font.Name = "Century Gothic";
                    Cell3B65.Style.Font.Size = 11;
                    Cell3B65.Style.Font.Bold = true;
                    Cell3B65.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B65.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string[] garantia = {
                    "DUCON S.A.S   ofrece  garantía  por  cinco  (5)  años contra  defectos de ",
                    "fábrica  para mobiliario y  un (1) año para  sillas, elementos de ",
                    "reposición como chapas y correderas. La garantía no cubre daños por uso inadecuado, ",
                    "sabotaje o daños o ocasionados por personas ajenas a DUCON."

                };
                    AgregarTextoDesdeArray(worksheet3, garantia, "B66", 12, false);

                    var Cell3B71 = worksheet3.Cells["B71"];
                    Cell3B71.Value = "6. SERVICIO POSVENTA:";
                    Cell3B71.Style.Font.Name = "Century Gothic";
                    Cell3B71.Style.Font.Size = 11;
                    Cell3B71.Style.Font.Bold = true;
                    Cell3B71.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B71.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string[] postventa = {
                    "DUCON S.A.S ofrece a solicitud del cliente, y dentro de los 3 meses ",
                    "seguidos a la instalación, el servicio de visita posventa; visita preventiva para verificar el ",
                    "estado de la obra.",
                    "Para solicitar este servicio, llame a los telefónos: 302 11 67 ext  109 - 110 - 111   ",
                    "ó    288 98 98 ext. 129."

                };
                    AgregarTextoDesdeArray(worksheet3, postventa, "B72", 12, false);

                    var Cell3B78 = worksheet3.Cells["B78"];
                    Cell3B78.Value = "7. FORMA DE PAGO:";
                    Cell3B78.Style.Font.Name = "Century Gothic";
                    Cell3B78.Style.Font.Size = 11;
                    Cell3B78.Style.Font.Bold = true;
                    Cell3B78.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B78.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var Cell3B79 = worksheet3.Cells["B79"];
                    Cell3B79.Value = "60% Anticipo        40% A la Entrega de la obra";
                    Cell3B79.Style.Font.Name = "Century Gothic";
                    Cell3B79.Style.Font.Size = 11;
                    Cell3B79.Style.Font.Bold = true;
                    Cell3B79.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B79.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var Cell3B81 = worksheet3.Cells["B81"];
                    Cell3B81.Value = "IMPORTANTE Los descuentos otorgados pierden validez con el incumplimiento de:";
                    Cell3B81.Style.Font.Name = "Century Gothic";
                    Cell3B81.Style.Font.Size = 11;
                    Cell3B81.Style.Font.Bold = true;
                    Cell3B81.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell3B81.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    string[] importante = {
                    "Las condiciones comerciales de venta, específicamente en las formas de pago tanto del",
                    "anticipo como en el pago de facturas a la fecha de vencimiento."
                };
                    AgregarTextoDesdeArray(worksheet3, importante, "B82", 12, false);

                    // Nota en Amarilla 
                    var Cell2BF85 = worksheet3.Cells["B85:F90"];
                    Cell2BF85.Merge = true;
                    Cell2BF85[85, 2].Value = "POR CONTRATO POR MANDATO: En virtud del los artículos 1634 y 1635 del código civil colombiano realizar los pagos a nombre de VISION EMPRESARIAL G2  S.A.S con Nit 900.314.150-1 EN BANCOLOMBIA  CUENTA CORRIENTE No. 01757718995. (Si requiere copia del contrato y certificado favor solicitarlo al correo carteraducon@ducon.com.co  - laurarestrepo@ducon.com.co) ";
                    Cell2BF85.Style.Font.Name = "Century Gothic";
                    Cell2BF85.Style.Font.Size = 11;
                    Cell2BF85.Style.Font.Bold = true;
                    Cell2BF85.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    Cell2BF85.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    Cell2BF85.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    Cell2BF85.Style.WrapText = true;
                    ;
                    Cell2BF85.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Cell2BF85.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                    Cell2BF85.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    Cell2BF85.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    Cell2BF85.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    Cell2BF85.Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    // FINANCIACIÓN
                    var CellB92P3 = worksheet3.Cells["B92"];
                    CellB92P3.Value = "8. FINANCIACIÓN:";
                    CellB92P3.Style.Font.Size = 11;
                    CellB92P3.Style.Font.Name = "Century Gothic";
                    CellB92P3.Style.Font.Bold = true;
                    CellB92P3.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    CellB92P3.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    string[] financiacion = {
                        "DUCON ofrece las siguientes alternativas de financiación de su proyecto de oficina",
                        "RENTING de Infraestructura: No afecta cupo de endeudamiento, recomendado para realizar",
                        "estrategias tributarias.",
                        "Si desea conocer más de este producto, comuníquese con su ejecutivo de proyectos.",
                        "",
                        "Banco Corp Banca  Leasing o crédito: Contacto Medellín  Juan Manuel Penagos.",
                        "correo electrónico jpenagossilva@corpbanca.com.co",
                        "Teléfono  fijo  : 4-604 18 18  op 2 ext 3454 , celular  317 364 34 78 Este  proceso  debe  ser  ",
                        "adelantado  directamente por el cliente con el banco."
                    };
                    AgregarTextoDesdeArray(worksheet3, financiacion, "B93", 12, false);

                    // VALIDEZ DE LA PROPUESTA
                    var CellB104P3 = worksheet3.Cells["B104"];
                    CellB104P3.Value = "9. VALIDEZ DE LA PROPUESTA:  ";
                    CellB104P3.Style.Font.Size = 11;
                    CellB104P3.Style.Font.Bold = true;
                    CellB104P3.Style.Font.Name = "Century Gothic";
                    CellB104P3.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    CellB104P3.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    string[] validez = {
                        "30  Días calendario.",
                        "",
                        "NOTA:   Somos autoretenedores Resolución  000075  -  Junio  24/93,  no somos  grandes",
                        "contribuyentes, resolución 000041 de enero 30 del 2014, contribuyente industrial de ICA en ",
                        "Sabaneta, somos auto retenedores de CREE, exentos de RETEICA, según articulo 77 ley 49     ",
                        "de 1990."
                    };
                    AgregarTextoDesdeArray(worksheet3, validez, "B105", 12, false);

                    // DEVOLUCIONES
                    var CellB112P3 = worksheet3.Cells["B112"];
                    CellB112P3.Value = "10. DEVOLUCIONES:";
                    CellB112P3.Style.Font.Size = 11;
                    CellB112P3.Style.Font.Name = "Century Gothic";
                    CellB112P3.Style.Font.Bold = true;
                    CellB112P3.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    CellB112P3.Style.Font.Color.SetColor(System.Drawing.Color.Black);
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

                    // Firma Gerencial 
                    var CellB1121P3 = worksheet3.Cells["B121:B122"];
                    CellB1121P3[121, 2].Merge = true;
                    CellB1121P3.Value = "Ejecutivo de Proyectos";
                    CellB1121P3.Style.Font.Size = 11;
                    CellB1121P3.Style.Font.Name = "Century Gothic";
                    CellB1121P3.Style.Font.Bold = true;
                    CellB1121P3.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    CellB1121P3.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var CellD121P3 = worksheet3.Cells["D121:F121"];
                    CellD121P3[121, 4].Merge = true;
                    CellD121P3.Value = "JAIME RENDÓN LONDOÑO";
                    CellD121P3.Style.Font.Size = 11;
                    CellD121P3.Style.Font.Name = "Century Gothic";
                    CellD121P3.Style.Font.Bold = true;
                    CellD121P3.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    CellD121P3.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var CellD122P3 = worksheet3.Cells["D122:E122"];
                    CellD122P3.Merge = true;
                    CellD122P3[122, 4].Value = "Gerente Comercial";
                    CellD122P3.Style.Font.Size = 11;
                    CellD122P3.Style.Font.Name = "Century Gothic";
                    CellD122P3.Style.Font.Bold = true;
                    CellD122P3.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    CellD122P3.Style.Font.Color.SetColor(System.Drawing.Color.Black);




                    // Guardamos el archivo de Excel
                    string filePath = Path.GetTempFileName() + ".xlsx";
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);

                    // Descargamos el archivo de Excel
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=Cotizacion.xlsx");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
            catch
            {
                txtMensaje.Text = "Ocurrió un error al intentar descargar el excel";
            }

        }

        public List<DatosFiltrados> CargarDatosExcel()
        {
            List<DatosFiltrados> datosFiltradosList = new List<DatosFiltrados>();
            string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand("sp_ObtenerDatosPanelPorPlano", connection);
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

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
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

        protected void AgregarTextoDesdeArray(ExcelWorksheet worksheet, string[] textoArray, string celdaInicio, int fontSize = 12, bool bold = false)
        {
            int fila = int.Parse(celdaInicio.Substring(1));  // Extraemos el número de fila de la celda de inicio

            for (int i = 0; i < textoArray.Length; i++)
            {
                // Calcular la celda de inicio para cada línea
                string celdaInicioLinea = $"B{fila + i}";

                // Fusionar las celdas de la columna B a la F para cada línea
                ExcelRange cellRange = worksheet.Cells[$"B{fila + i}:E{fila + i}"];
                cellRange.Merge = true;
                cellRange.Style.Font.Size = fontSize;
                cellRange.Style.Font.Bold = bold;
                cellRange.Style.Font.Name = "Century Gothic";
                cellRange.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                cellRange.Style.WrapText = true;

                // Escribir el texto en la celda de inicio de la línea
                worksheet.Cells[celdaInicioLinea].Value = textoArray[i];
            }
        }
        protected void Terminar(object sender, EventArgs e)
        {
            // Esta redireccion se deja por si la descarga demora un poco mas de lo normal 

            Session["Id_OT2"] = tbOT.Text;
            Session["pedido2"] = ddlNumbers.SelectedItem.Text;

            Response.Redirect("OrdenTrabajo.aspx");

        }

        // INICIO LOGICA DEL BOTON OK

        protected void Boton_Ok1(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "actualizarValorBotonOk", "actualizarValorBotonOk();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#BotonOk').modal('show');", true);

            Session["Id_OT2"] = tbOT.Text;
            Session["pedido2"] = ddlNumbers.SelectedItem.Text;

        }
        protected void Boton_Ok(object sender, EventArgs e)
        {

            string departamento = Session["Departamento"].ToString();

            switch (departamento.ToUpper())
            {
                //Para el departamento de Ventas e Instalación 
                case "VENTAS":
                case "Instalación":

                    //Validar que la cotización sea dieferente de NO TIENE
                    string cotizacion = txtCotizacion.Text; // Asegúrate de reemplazar 'tuTextBox' con el nombre correcto de tu TextBox.

                    if (cotizacion.ToUpper() != "NO TIENE")
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString; // Reemplaza con tu cadena de conexión.

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            string sSql = "SELECT * FROM tblCotización WHERE cotización = @cotizacion";
                            using (SqlCommand command = new SqlCommand(sSql, connection))
                            {
                                command.Parameters.AddWithValue("@cotizacion", cotizacion);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    // Valimadamos que la cotizacion exista luego
                                    if (reader.Read())
                                    {

                                        // Consultamos el saldo de la  cotizacion y validamos que el saldo  no puede ser menor que el precio de venta 
                                        if (Convert.ToDecimal(reader["Saldo"].ToString()) < Convert.ToDecimal(txtVenta.Text))
                                        {

                                            string mensajePersonalizado = "La cotización digitada no tiene saldo suficiente para el pedido.";
                                            string urlRedireccion = "OrdenTrabajo.aspx";
                                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                                        }
                                        //Validamos que la fecha de la cotizacion no puede exceder los 30 dias 
                                        DateTime fechaCotizacion = Convert.ToDateTime(reader["Fecha_Cotización"]);
                                        if ((DateTime.Now - fechaCotizacion).Days > 30)
                                        {

                                            // Error al realizar las inserciones al ISID
                                            string mensajePersonalizado = "La fecha de la cotización digitada excede los 30 días. Para terminar el Pedido, debe actualizar la cotización.";
                                            string urlRedireccion = "OrdenTrabajo.aspx";
                                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                        }
                                    }
                                    else
                                    {

                                        // Error al realizar las inserciones al ISID
                                        string mensajePersonalizado = "La cotización digitada no existe. Para terminar el Pedido, debe modificar este campo.";
                                        string urlRedireccion = "OrdenTrabajo.aspx";
                                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                                    }
                                }
                            }
                        }
                    }

                    // Consultamos el tipo de pedido y  si es pedidio facturable
                    if (ValidarPedidoFacturable(dtacboTipoPedido.SelectedValue))
                    {
                        //Validamos que el precio ser mayor que cero 
                        if (Convert.ToDouble(txtVenta.Text) <= 0)
                        {
                            // Mensaje SI EL PEDIDO ES FACTURABLE, EL VALOR VENTA DEBE SER MAYOR A CERO                       
                            string mensajePersonalizado = "El tipo de pedido es facturable, el valor venta debe ser superior a Cero(0).";
                            string urlRedireccion = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                    }
                    else
                    {
                        // Validamos que el precio de venta sea igual cero 
                        if (Convert.ToDouble(txtVenta.Text) != 0)
                        {
                            // Mensaje SI EL PEDIDO NO ES FACTURABLE, EL VALOR VENTA DEBE SER CERO
                            string mensajePersonalizado = "El tipo de pedido no es facturable, el valor venta debe ser Cero(0).";
                            string urlRedireccion = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                        }

                    }

                    // Se verifica que la informacion del cliente este actualizada 
                    if (VerificarActualizacionCliente(txtNit.Text))
                    {

                        string mensajePersonalizado = "No puede pasar un pedido, si la información del cliente no esta actualizada";
                        string urlRedireccion = "OrdenTrabajo.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }

                    // Se verifica que la informacion del contacto este actualizada 
                    if (VerificarActualizacionContacto(txtNit.Text, Session["IdContactoFactSession"].ToString()))
                    {

                        string mensajePersonalizado = "No puede pasar un pedido, si la información del contacto del cliente no esta actualizada";
                        string urlRedireccion = "OrdenTrabajo.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }


                    // Se realiza la validacon de TotalObraMas (Pendiente hasta validar que es la variable TotalObraMas) !!!Verificar 
                    // Este campo es importante para validar que el saldo de ese pedido no sea negativo 
                    DataTable InfoOT = ConsultarInformacionPedidoSaldo();
                    bool afectaBolsa = ConsultaAfectaBolsa();


                    CalcularSaldo2(InfoOT);

                    if ((TotalObraMas > 0 && Convert.ToDouble(lblSaldoOT.Text) < 0) && afectaBolsa == true)
                    {
                        if ((Math.Abs(Convert.ToDouble(lblSaldoOT.Text) * 100 / TotalObraMas) > 3))
                        {
                            string mensajeError = "El pedido tiene un saldo:  " + Math.Abs(Convert.ToDouble(lblSaldoOT.Text)) + " % en contra no se puede pasar el pedido";
                            string scriptError = "alert('" + mensajeError + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);
                            return;
                        }
                    }

                    // SE VERIFICA SI LA FECHA DE EMPAQUE CUMPLE CON LOS TIEMPO MINIMOS
                    if (!ValidarFechaEmpaque())
                    {

                        string mensajePersonalizado = "La fecha de empaque debe estar " + +DiasMinimoparaProduccion + " días hábiles por encima de la fecha actual.";
                        string urlRedireccion = "OrdenTrabajo.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                    }


                    //Valida si RequiereDespice 
                    bool valid = ValidarDespiece(dtacboTipoPedido.SelectedValue);

                    if (!valid)
                    {
                        //No requiere despiece

                        ActualizarTerminadoVenta(FechaEmpaque, 1);
                        ActualizarCotizacion();
                        EliminarReporteOT();
                        CrearReporteOt();
                        EliminarReportePlano();
                        InsertarReportePLano();
                        EliminarPlanoPanelCot();
                        InsertarPLanoPanelCot();
                        EliminarReporteDespiece();
                        InsertarReporteDespiece();

                        //SE REGISTRA EL PEDIDO EN EL ISID      
                        if (PasarPedidoISID(true))
                        {

                            //SE CREA PLANO PARA LA BOLSA
                            ValidarBolsaPlano();


                            // Error al realizar las inserciones al ISID
                            string mensajePersonalizado = "El pedido fue registrado directamente al ISID y habilitado para producción.";
                            string urlRedireccion = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                        }
                        else
                        {
                            //Registro exitoso en el ISID 
                            string scriptContCliNoAct = "alert('NO. NO fue satisfactorio el registro del pedido en el ISID, favor terminar nuevamente el pedido.');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showFacturable", scriptContCliNoAct, true);
                            return;
                        }



                    }
                    else
                    {
                        //Requiere Despiece 
                        ActualizarTerminadoVenta(FechaEmpaque, 0);
                        ActualizarCotizacion();
                        EliminarReporteOT();
                        CrearReporteOt();


                        // SE CONSULTAN LOS ACABADOS QUE TIENE EL PEDIDO DEFINIDOS POR VENTAS
                        DataTable AcabadoVentas = ConsultarAcabadosSID();
                        string acabado = "";
                        if (AcabadoVentas.Rows.Count > 0)
                        {
                            foreach (DataRow row in AcabadoVentas.Rows)
                            {
                                acabado = acabado + row["GrupoObjetoparaAcabado"].ToString() + ": " + row["AcabadoVentas"].ToString() + "\n";
                            }
                        }
                        acabado = acabado + "\n" + txObs2.InnerText;
                        ActualizarReporteOT_Acabado_SID(acabado);


                        //SE CREA LA BOLSA A PARTIR DEL DISEÑO DE LA COTIZACION
                        DataTable TipoPedido = ObtenerTipoPedidoSID();
                        bool EstadisticaVenta = Convert.ToBoolean(TipoPedido.Rows[0]["EstadisticaVenta"].ToString());

                        if (EstadisticaVenta && txtDiseño.Text != "")
                        {
                            DataTable ResumenPedido = ConsultarResumePedidoSID();
                            ActualizarOtBolsaSID();

                            foreach (DataRow row in ResumenPedido.Rows)
                            {
                                string id_GrupoObjeto = row["id_GrupoObjeto"].ToString();
                                string Descripcion_Grupo = row["Descripcion_Grupo"].ToString();
                                string Cantidad = row["Cantidad"].ToString();
                                string SubTotal = row["SubTotal"].ToString();
                                string GOBloqueaPedido = row["GOBloqueaPedido"].ToString();

                                DataTable InfoBolsa = ConsultarBolsaOT_SID(id_GrupoObjeto);

                                if (InfoBolsa.Rows.Count < 0)
                                {
                                    CrearBolsaObjeto(id_GrupoObjeto, Descripcion_Grupo, Cantidad, SubTotal, GOBloqueaPedido);
                                }
                                else
                                {
                                    ActualizarBolsaObjeto(id_GrupoObjeto, Cantidad, SubTotal);
                                }


                            }


                        }


                        //SE REGISTRA EL PEDIDO EN EL ISID
                        if (PasarPedidoISID(false))
                        {

                            string mensajePersonalizado = "El pedido fue registrado directamente al ISID para el chequeo financiero.";
                            string urlRedireccion = "OrdenTrabajo.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                        }
                        else
                        {
                            //Registro exitoso en el ISID 
                            string scriptContCliNoAct = "alert('NO. NO fue satisfactorio el registro del pedido en el ISID, favor terminar nuevamente el pedido.');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showFacturable", scriptContCliNoAct, true);
                        }



                    }

                    break;

                case "Diseño":
                case "COMPRAS":
                    // Aqui va la parte del boton Ok para el área de diseño  y compras                  
                    break;

                default:
                    // Aqui va alguna exepcion que pueda pasar con la variable departamento 
                    break;
            }

        }

        //Valiacion de pedido facturable 
        private bool ValidarPedidoFacturable(string idPedido)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT EstadisticaVenta FROM tblTipoPedido WHERE Id_TipoPedido = @idPedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@idPedido", idPedido);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Si se encuentra el pedido y la columna EstadisticaVenta es verdadera, retorna true
                        return reader.Read() && reader.GetBoolean(0);
                    }
                }
            }
        }

        //Validacion de fecha acutalizacion cliente 
        private bool VerificarActualizacionCliente(string nit)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT UltimaActualizacion FROM tblClienteObra WHERE Nit = @nit";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@nit", nit);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Verificar si la fecha de última actualización ha pasado más de un año  y un mes
                            DateTime ultimaActualizacion = reader.GetDateTime(0);
                            TimeSpan diferencia = DateTime.Now - ultimaActualizacion;
                            int diasTranscurridos = diferencia.Days;

                            // Si han pasado más de 395 días, se considera que ha pasado más de un año y un mes
                            return diasTranscurridos > 3960;
                        }
                    }
                }
            }

            // Si no se encontró la información, se considera que ha pasado más de un año y un mes 
            return true;
        }

        //Validacion de fecha acutalizacion contacto cliente 
        private bool VerificarActualizacionContacto(string nit, string idcontacto)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "select cocUltimaActualizacion from tblClienteObraContacto where  cocNIT = @nit and  IdContacto = @idContacto";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@nit", nit);
                    cmd.Parameters.AddWithValue("@idContacto", idcontacto);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Verificar si la fecha de última actualización ha pasado más de un año  y un mes
                            DateTime ultimaActualizacion = reader.GetDateTime(0);
                            TimeSpan diferencia = DateTime.Now - ultimaActualizacion;
                            int diasTranscurridos = diferencia.Days;

                            return diasTranscurridos > 395;
                        }
                    }
                }
            }

            // Si no se encontró la información, se considera que ha pasado más de un año y un mes 
            return true;
        }

        private bool ConsultaAfectaBolsa()
        {
            bool afectaBolsa = false;

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT SUM (tblOT.ValorPedido) FROM tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido " +
                              "WHERE (((tblOT.Id_OT)= @OT)) and Consecutivo_Pedido= @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@OT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);


                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado no es nulo y convertirlo a double
                    if (result != null && result != DBNull.Value)
                    {
                        afectaBolsa = Convert.ToBoolean(result);
                    }
                }

                return afectaBolsa;
            }
        }
        protected void CalcularSaldo2(DataTable InfoOT)
        {

            foreach (DataRow Row in InfoOT.Rows)
            {
                bool AfectaBola = Convert.ToBoolean(Row["AfectaBolsa"].ToString());
                double ValorBolsa = double.Parse(Row["ValorBolsa"].ToString());
                double ValorPedido = double.Parse(Row["ValorPedido"].ToString());

                if (AfectaBola)
                {
                    TotalObraMas += ValorBolsa;
                    TotalObraMenos += ValorPedido;
                }

            }
        }

        //Validacion fecha de empaque 
        private bool ValidarFechaEmpaque()
        {
            //Se consultan los dias minimos para produccion 
            ConsultarDiasMinProduccion();

            // Este metodo se ejecuta pero el valor no se usa !!!!Verificar 
            ConsultarDiasPorDefectoProduccion();

            FechaEmpaque = Convert.ToDateTime(dtpEmpaque.Text);

            // validar de donde sale el valor de Val(chkDefinirAcabados.Tag) para asignar DiasPorDefectoparaProduccion = Val(chkDefinirAcabados.Tag) !!!Veficar 

            ConsultarDiasHabiles(DateTime.Now, FechaEmpaque);

            if (DiasHabiles <= DiasMinimoparaProduccion)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        // Consultar Fecha de produccion minima, defecto  y dias habiles 
        private void ConsultarDiasMinProduccion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT mail FROM tblUsosVarios WHERE ObjetivoMail = 'DiasMinimoparaProduccion'";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Intentar convertir el valor a entero
                            if (int.TryParse(reader["mail"].ToString(), out DiasMinimoparaProduccion))
                            {
                                // Conversión exitosa
                            }
                            else
                            {

                                DiasMinimoparaProduccion = 3;
                            }
                        }
                        else
                        {
                            // Valor predeterminado si no se encuentra el valor en la base de datos
                            DiasMinimoparaProduccion = 3;
                        }
                    }
                }
            }
        }
        private void ConsultarDiasPorDefectoProduccion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT mail FROM tblUsosVarios WHERE ObjetivoMail = 'DiasPorDefectoparaProduccion'";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Intentar convertir el valor a entero
                            if (int.TryParse(reader["mail"].ToString(), out DiasPorDefectoParaProduccion))
                            {
                            }
                            else
                            {
                                DiasPorDefectoParaProduccion = 10;
                            }
                        }
                        else
                        {
                            // Valor predeterminado si no se encuentra el valor en la base de datos
                            DiasPorDefectoParaProduccion = 10;
                        }
                    }
                }
            }
        }
        private void ConsultarDiasHabiles(DateTime fechaInicial, DateTime fechaFinal)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DUC_DIASHABILES", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    cmd.Parameters.Add("@FechaInicial", SqlDbType.Date).Value = fechaInicial;
                    cmd.Parameters.Add("@FechaFinal", SqlDbType.Date).Value = fechaFinal;

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtén los valores devueltos por el procedimiento almacenado                        
                            DiasHabiles = reader.GetInt32(reader.GetOrdinal("DiasHabiles"));
                        }
                    }
                }
            }
        }

        //Validacion si rquiere o no despiece 
        private bool ValidarDespiece(string idPedido)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT RequiereDespiece FROM tblTipoPedido WHERE Id_TipoPedido = @idPedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@idPedido", idPedido);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Si se encuentra el pedido y la columna EstadisticaVenta es verdadera, retorna true
                        return reader.Read() && reader.GetBoolean(0);
                    }
                }
            }
        }

        // SE REALIZAN LOS BORRADOS , ACTUALIZACION E INSERCIONES DE LOS REPORTES EN EL SID 
        private void ActualizarTerminadoVenta(DateTime FechaEmpaque, int TerminadoDiseño)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE tblOT SET Terminado_Ventas=1,Terminado_Diseño= @TerminadoDiseño, Fecha_Entrega_Dibujo_Despiece= GETDATE()," +
                              "Fecha_Entrega_Produccion= GETDATE(),Fecha_Despacho_Produccion= @FechaEmpaque, ResumenObra= @ResumenPlano " +
                              "WHERE tblOT.Id_OT= @IdOT AND Consecutivo_Pedido= @pedido ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@FechaEmpaque", FechaEmpaque);
                    cmd.Parameters.AddWithValue("@TerminadoDiseño", TerminadoDiseño);
                    cmd.Parameters.AddWithValue("@ResumenPlano", txResumen.InnerText);
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void ActualizarCotizacion()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE tblCotización SET Estado=2, Fecha_Respuesta = @FechaConfirVenta, Saldo = Saldo - @PrecioVenta," +
                              " Id_OT= @IdOT, Pedido = @pedido WHERE Cotización = @cotizacion  ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@FechaConfirVenta", dtpFechaEntregaDibujoDespiece.Text);
                    cmd.Parameters.AddWithValue("@PrecioVenta", txtVenta.Text);
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@cotizacion", txtCotizacion.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void EliminarReporteOT()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReporteOT WHERE Id_OT= @IdOT AND Consecutivo_Pedido= @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void CrearReporteOt()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Nombre del procedimiento almacenado
                string storedProcedureName = "ctaReporteOT";

                using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                {
                    connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    cmd.Parameters.AddWithValue("@OT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@PED", ddlNumbers.SelectedItem.Text);

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void EliminarReportePlano()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblreportePlano WHERE Id_OT= @IdOT AND Consecutivo_Pedido= @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void InsertarReportePLano()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblreportePlano ( Plano, Id_OT, COnsecutivo_Pedido, Area, Dibujante )" +
                              " SELECT tblPlano.Plano, tblPlano.Id_OT, tblPlano.COnsecutivo_Pedido, tblPlano.Area, tblPlano.RealizadoPor From tblPlano" +
                              " WHERE tblPlano.Id_OT = @IdOT AND tblPlano.COnsecutivo_Pedido = @pedido ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void EliminarPlanoPanelCot()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblPlano_Panel_Cotizacion WHERE Id_Plano = @plano ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void InsertarPLanoPanelCot()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblPlano_Panel_Cotizacion ( Id_Plano, Id_PanelNum, Cantidad, Observaciones ) " +
                              "SELECT tblPlano_Panel.Id_Plano, tblPlano_Panel.Id_PanelNum, tblPlano_Panel.Cantidad, tblPlano_Panel.Observaciones " +
                              "FROM tblPlano_Panel WHERE (((tblPlano_Panel.Id_Plano) = @plano )) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void EliminarReporteDespiece()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReporteDespiece WHERE Id_Plano = @plano OR OT = @IdOT AND PEDIDO = @pedido ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void InsertarReporteDespiece()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblReporteDespiece ( OT, Pedido, Id_Plano, Id_Panel, Ancho_Panel, Descripcion_Panel, Cantidad," +
                              "Descripcion_Grupo, Id_Numerico, Altura, Profundidad, Descripcion_Linea, Precio_Venta, Cotizar )" +
                              "SELECT tblPlano.Id_OT, tblPlano.COnsecutivo_Pedido, tblPlano.Plano, tblPanel.Id_Panel, tblPanel.Ancho," +
                              "tblPanel.Descripcion_Panel, tblPlano_Panel.Cantidad, tblGrupoObjeto.Descripcion_Grupo, tblPanel.Id_Numerico," +
                              "tblPanel.Altura, tblPanel.Profundidad, tblLinea.Descripcion_Linea, tblPanel.Precio_Venta, tblGrupoObjeto.Cotizar " +
                              "FROM tblPlano INNER JOIN ((tblLinea INNER JOIN (tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto)" +
                              "ON tblLinea.Id_Linea = tblPanel.Id_Linea) INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum)" +
                              "ON tblPlano.Plano = tblPlano_Panel.Id_Plano WHERE (((tblPlano.Plano)= @plano )) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", txtPlano.Text);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }


        // VALIDAR Y CREAR PLANO PARA UNA BOLSA 
        private void ValidarBolsaPlano()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString; // Reemplaza con tu cadena de conexión.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSql = "Select * from tblPLano where plano= 'BSA'+'@IdOT-@pedido'";
                using (SqlCommand command = new SqlCommand(sSql, connection))
                {
                    command.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    command.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Valimadamos que la cotizacion exista luego
                        if (!reader.Read())
                        {
                            CrearBolsaPlano();
                        }
                        else
                        {

                        }
                    }

                }

            }
        }
        private void CrearBolsaPlano()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "Insert into tblPlano(Plano,Nombre_Cliente,Contacto_Cliente,Fecha_Entrega_Bitacora,Fecha_Termino_Diseño,area," +
                             " Historial,AsesorComercial,RealizadoPor)  Values (@plano,@NombreCliente,@receptor,GETDATE(),GETDATE(), @NombreObra,'',@asesor,@realizadoPor) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", "BSA" + tbOT.Text + "-" + ddlNumbers.SelectedItem.Text); // Validar cuando se crea una bolsa ya que en el sid llega vacio en consecutivo 
                    cmd.Parameters.AddWithValue("@NombreCliente", txtCliente.Text);
                    cmd.Parameters.AddWithValue("@receptor", tbRecibe.Text);
                    cmd.Parameters.AddWithValue("@NombreObra", tbObra.Text);
                    cmd.Parameters.AddWithValue("@asesor", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@realizadoPor", Session["usuariologueado"].ToString());
                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }


        // SE PASA EL PEDIDIO AL ISID
        private bool PasarPedidoISID(bool OKDibujo)
        {
            int ID_CONTACTO;
            bool PararProduccion = false;
            bool PararDespacho = false;
            string OrientacionPedido;

            bool PasarPedidoIsid = false;

            //SE CONSULTA EL TIPO DE PEDIDO Y SI NO EXISTE EN EL ISID SE AGREGA 
            DataTable TipoPedido = ObtenerTipoPedidoSID();
            ConsultarInsertarTipoPedidoEnISID(TipoPedido);

            //SE EXPORTA EL CLIENTE Y EL CONTACTO DE LA OBRA
            DataTable datosCliente = ObtenerDatoClienteSID();
            ConsultarClienteISID(datosCliente);

            //REGISTRAR CONTACTO CLIENTE
            if (!ExistenciaContacto(datosCliente))
            {
                InsertarContactoClienteISID(datosCliente);
            }
            else
            {
                // Validar Fecha contacto y si es menor actualizar contacto 
                if (!UltimaActualizacionContactoCliente(datosCliente))
                {
                    ActualizarContactoClienteISID(datosCliente);
                }
            }

            // Se usará para adicionar el pedido o actualizarlo 
            ID_CONTACTO = ConsultarIDContacto(datosCliente);


            //SE EXPORTA EL ASESOR DE LA OBRA A LA BD DEL ISID
            DataTable DatoAsesor = ObtenerAsesorComercialSID();
            ConsultarAsesorISID(DatoAsesor);


            // CONSULTANDO LA INF DEL PEDIDO EN EL SID 
            DataTable InfoPedidoSID = ObtenerInfoPedidoSID();

            //SE CONSULTA SI EL PEDIDO BASE DEL PEDIDO INGRESADO ESTA DETENIDO YA SEA POR PRODUCCION O POR DESPACHO, EL PEDIDO NUEVO ENTRARA CON LAS MISMAS CONDICIONES

            DataTable PedDetProduDesp = ObtenerPedDetProduDespISID(InfoPedidoSID);
            if (PedDetProduDesp.Rows.Count < 0)
            {
                PararProduccion = Convert.ToBoolean(PedDetProduDesp.Rows[0]["PararProduccion"].ToString());
                PararDespacho = Convert.ToBoolean(PedDetProduDesp.Rows[0]["PararDespacho"].ToString());
            }

            //SE REEMPLAZA EL PEDIDO SI EXISTE EN LA BD DEL ISID

            if (!ExistenciaPedidoISID())
            {
                InsertarPedidoISID(InfoPedidoSID, OKDibujo, ID_CONTACTO, PararProduccion, PararDespacho);
            }
            else
            {
                ActualizarPedidoISID(InfoPedidoSID, OKDibujo, ID_CONTACTO, PararProduccion, PararDespacho);
            }

            //SI EL PEDIDO ESTA CONFIGURADO PARA INGRESAR UNA OBSERVACION POR DEFECTO

            int idObservacion = Convert.ToInt32(TipoPedido.Rows[0]["TipPedidObsAuto"].ToString());

            DataTable InfoObservacion = ConsultarObservacionAuto(idObservacion);

            if (InfoObservacion.Rows.Count > 0)
            {
                InsertarObservacionAutoISID(InfoObservacion, InfoPedidoSID);
            }

            //SE EXPORTA LA DOCUMENTACION DE LA OT
            ExportarDocumentacionOT();

            //DEPENDIENDO LA ORIENTACION DEL TIPO PEDIDO SE PROGRAMA LA OBRA POR DEFECTO A LOS PROCESOS BASES
            EliminarProgramacion();
            OrientacionPedido = TipoPedido.Rows[0]["orientacion"].ToString();

            switch (OrientacionPedido.ToUpper())
            {
                case "COMERCIAL":
                    DataTable PrograamacionProducccion = ObtenerProgProduccion();

                    if (PrograamacionProducccion.Rows.Count <= 0)
                    {
                        RealizarProgramacionPedidoISID(InfoPedidoSID);
                    }
                    else
                    {
                        DateTime FechaInicioProceso = DateTime.Now;
                        DateTime FechaFinProceso = DateTime.Now;
                        int Turno = 0;

                        // Ahora procesamos cada fila de la tabla y realizamos la lógica requerida
                        foreach (DataRow row in PrograamacionProducccion.Rows)
                        {

                            bool EntregaAlFinal = Convert.ToBoolean(row["EntregaAlFinal"]);

                            if (!EntregaAlFinal)
                            {
                                // Si no se entrega al final, se establece la fecha de inicio como la fecha actual
                                FechaInicioProceso = DateTime.Now;
                                FechaFinProceso = DateTime.Now.Add(TimeSpan.FromDays(Convert.ToInt32(row["TiempoRespuesta"])));
                            }
                            else
                            {
                                // Si se entrega al final, se calculan las fechas en función de la fecha de despacho de producción
                                DateTime FechaDespachoProduccion = Convert.ToDateTime(InfoPedidoSID.Rows[0]["Fecha_Despacho_Produccion"].ToString());
                                FechaInicioProceso = FechaDespachoProduccion.Subtract(TimeSpan.FromDays(Convert.ToInt32(row["TiempoRespuesta"])));
                                FechaFinProceso = FechaDespachoProduccion.AddDays(-1);
                            }


                            string responsable = row["Responsable"].ToString();
                            string Descripcion_Proceso = row["Descripcion_Proceso"].ToString();
                            string Id_Area = row["Id_Area"].ToString();
                            string EntregaAlFinal1 = row["EntregaAlFinal"].ToString();


                            RealizarProgramacionPedidoISID2(InfoPedidoSID, FechaFinProceso, FechaInicioProceso, Turno, responsable, Descripcion_Proceso, Id_Area, EntregaAlFinal1, "Proceso Base");
                        }



                    }

                    break;

                case "OAI":
                    DataTable PrograamacionProducccionOAI = ObtenerProgProduccionOAI();

                    if (PrograamacionProducccionOAI.Rows.Count <= 0)
                    {
                        RealizarProgramacionPedidoISID(InfoPedidoSID);
                    }
                    else
                    {
                        DateTime FechaInicioProceso = DateTime.Now;
                        DateTime FechaFinProceso = DateTime.Now;
                        int Turno = 0;

                        // Ahora procesamos cada fila de la tabla y realizamos la lógica requerida
                        foreach (DataRow row in PrograamacionProducccionOAI.Rows)
                        {

                            bool EntregaAlFinal = Convert.ToBoolean(row["EntregaAlFinal"]);

                            if (!EntregaAlFinal)
                            {
                                // Si no se entrega al final, se establece la fecha de inicio como la fecha actual
                                FechaInicioProceso = DateTime.Now;
                                FechaFinProceso = DateTime.Now.Add(TimeSpan.FromDays(Convert.ToInt32(row["TiempoRespuesta"])));
                            }
                            else
                            {
                                // Si se entrega al final, se calculan las fechas en función de la fecha de despacho de producción
                                DateTime FechaDespachoProduccion = Convert.ToDateTime(InfoPedidoSID.Rows[0]["Fecha_Despacho_Produccion"].ToString());
                                FechaInicioProceso = FechaDespachoProduccion.Subtract(TimeSpan.FromDays(Convert.ToInt32(row["TiempoRespuesta"])));
                                FechaFinProceso = FechaDespachoProduccion.AddDays(-1);
                            }

                            bool ObjetivoEspecifico = Convert.ToBoolean(row["ObjetivoEspecifico"]);
                            string responsable = row["Responsable"].ToString();
                            string Descripcion_Proceso = row["Descripcion_Proceso"].ToString();
                            string Descripcion_Area2 = row["Descripcion_Area2"].ToString();
                            string Id_Area = row["Id_Area"].ToString();
                            string EntregaAlFinal1 = row["EntregaAlFinal"].ToString();
                            bool ProgramacionUnica = Convert.ToBoolean(row["ProgramacionUnica"].ToString());

                            if (!ObjetivoEspecifico)
                            {

                                RealizarProgramacionPedidoISID2(InfoPedidoSID, FechaFinProceso, FechaInicioProceso, Turno, responsable, Descripcion_Proceso, Id_Area, EntregaAlFinal1, "TODO");
                            }
                            else
                            {
                                string DescripcionProceso = row["Descripcion_Proceso"].ToString();
                                DataTable ProductosXProceso = ObtenerProductosXProcesoSID(InfoPedidoSID, DescripcionProceso);

                                if (ProductosXProceso.Rows.Count > 0)
                                {
                                    if (ProgramacionUnica)
                                    {
                                        //SE INSERTA UNA SOLA PROGRAMACION PARA TODOS LOS PRODUCTOS A ENTREGAR
                                        RealizarProgramacionPedidoISID2(InfoPedidoSID, FechaFinProceso, FechaInicioProceso, Turno, responsable, Descripcion_Proceso, Id_Area, EntregaAlFinal1, "TODO");
                                    }
                                    else
                                    {
                                        foreach (DataRow row1 in ProductosXProceso.Rows)
                                        {
                                            string producto = row["DescripcionFull"].ToString();
                                            string DescripcionFull = row["DescripcionFull"].ToString();
                                            int cantidad = Convert.ToInt32(row["cantidad"].ToString());

                                            if (!ExistenciaProductoISID(InfoPedidoSID, Id_Area, producto))
                                            {
                                                //SE INSERTA LA PROGRAMACION DEL PROCESO SEGUN LOS PRODUCTOS 
                                                RealizarProgramacionPedidoxProductoISID(InfoPedidoSID, FechaFinProceso, FechaInicioProceso, Turno, responsable, Descripcion_Proceso, Descripcion_Area2, Id_Area, EntregaAlFinal1, DescripcionFull, cantidad);

                                            }
                                            else
                                            {
                                                // SE ACTUALIZA LA PROGRAMACION 
                                                AcutualizarProgramacionPedidoxProductoISID(InfoPedidoSID, FechaFinProceso, Turno, responsable, Id_Area, DescripcionFull, cantidad);
                                            }


                                        }
                                    }

                                }

                            }
                        }
                    }

                    break;
                default:

                    break;
            }

            // SI TRUE OK DIBUJO CONTROLADOR  

            if (OKDibujo)
            {
                // SE EXPORTA EL PLANO A LA BD DEL ISID

                //Se consulta reporte plano en el SID
                DataTable ReportePlanoSID = ConsultarReportePlanoSID();
                string plano = ReportePlanoSID.Rows[0]["Plano"].ToString();

                //Se consulta reporte plano en el ISID
                DataTable ReportePlanoISID = ConsultarReportePlanoISID(plano);

                //Se elimina el reporte plano en el ISID
                EliminarReportePlanoISID(plano);

                if (ReportePlanoISID.Rows.Count <= 0)
                {
                    InsertarReportePlanoISID(ReportePlanoSID);
                }
                else
                {
                    string IdOt = tbOT.Text;
                    string pedido = ddlNumbers.SelectedItem.Text;
                    string mensaje = "No se puede registrar la Obra, ya que el plano: " + plano + " pertenece a la OT: " + IdOt + "-" + pedido;
                    string scriptNoReportePlanoISID = "alert('" + mensaje + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoReportePlanoISID", scriptNoReportePlanoISID, true);
                }

                //EXPORTANDO EL DESPIECE DEL PLANO A LA BD DEL ISID

                //Se consulta reporte Despiece en el ISID
                DataTable ReporteDespieceSID = ConsultarReporteDespiceSID();

                //Se elimina el reporte Despiece en el ISID
                EliminarReporteDespieceISID();

                if (ReporteDespieceSID.Rows.Count > 0)
                {
                    // Construimos el codigo Sag que de momento no se esta utilizando en el codigo ????                 
                    string paraEnsamble = ObtenerParaEnsamble();
                    string Codigo = ReporteDespieceSID.Rows[0]["Id_Numerico"].ToString();
                    string Cod_Sag = "ES" + paraEnsamble + Codigo;

                    InsertarDatosReporteDespieceISID(ReporteDespieceSID);
                }

                //ENVIAMOS LOS ACABADOS

                //Borramos los acabados si los hay del ISID y luego enviamos los actuales 
                EliminarAcabadosISID();

                DataTable Acabados = ConsultarAcabadosSID();

                if (Acabados.Rows.Count > 0)
                {
                    InsertarAcabadosISID(Acabados);

                }

                //EXPORTAMOS MEDIDAS DE CORTE A LA BD DEL ISID

                //Consultamos las medidas de corte  en el SID
                DataTable MedidasCorte = ConsultarReporteMedidasSID();

                //Eliminamos medidas de corte en el ISID
                EliminarMedidasCorteISID();

                if (MedidasCorte.Rows.Count > 0)
                {
                    InsertarMedidasCorteISID(MedidasCorte);
                }

                //EXPORTAR MEDIDAS FINALES DEL MODULO 

                //obtenemos la familia modulo despacho troja
                string FamiliaModuloDespachoTroja = "";
                FamiliaModuloDespachoTroja = ObtenerFamiliaModuloDespachoTroja().ToUpper();

                // Consultamos  el reporte modulo medida final edl ISID
                DataTable MedidaFinal = ConsultarMedidaFinal();

                //Eliminar medida final ISID 
                EliminarMedidaFinalISID();

                if (MedidaFinal.Rows.Count > 0)
                {

                    InsertarMedidaFinalISID(MedidaFinal, FamiliaModuloDespachoTroja);
                }

                // EXPORTAR MANO DE OBRA 

                //Consultar  reporte mano obra SID
                DataTable ManoObra = ConsultarManoObraSID();

                // Eliminar reporte mano obra ISID
                EliminarReporteManoObraISID();

                if (ManoObra.Rows.Count > 0)
                {
                    InsertarReporteManoObraISID(ManoObra);
                }

                // EXPORTAR LISTADO PARA EMPAQUE

                //Consultar  Empaque SID
                DataTable Empaque = ConsultarEmpaqueSID();

                // Eliminar reporte mano obra ISID
                EliminarEmpaqueISID();

                if (Empaque.Rows.Count > 0)
                {
                    string plano1 = Empaque.Rows[0]["Plano"].ToString();
                    InsertarEmpaqueISID(plano1);
                    InsertarEmpaque2ISID(Empaque);
                }


            }


            PasarPedidoIsid = true;


            //retorno para confirmar el proceso de pasar el pedido al ISID
            return PasarPedidoIsid;
        }


        //EXPORTAR TIPO DE PEDIDO AL ISID
        private DataTable ObtenerTipoPedidoSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblTipoPedido.* FROM tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido" +
                              " WHERE tblOT.Id_OT = @IdOT AND tblOT.Consecutivo_Pedido = @Pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void ConsultarInsertarTipoPedidoEnISID(DataTable tipoPedidoDataTable)
        {
            if (tipoPedidoDataTable.Rows.Count > 0)
            {
                string idTipoPedido = tipoPedidoDataTable.Rows[0]["Id_TipoPedido"].ToString();

                // Verificar si el IdTipoPedido ya existe en la otra base de datos (ISID)
                if (!ExisteTipoPedidoEnISID(idTipoPedido))
                {
                    // Si no existe, realizar la inserción
                    InsertarTipoPedidoEnISID(tipoPedidoDataTable);
                }

            }
        }
        private bool ExisteTipoPedidoEnISID(string idTipoPedido)
        {
            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT COUNT(*) FROM tblTipoPedido WHERE Id_TipoPedido = @Id_TipoPedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@Id_TipoPedido", idTipoPedido);

                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        int rowCount = Convert.ToInt32(result);
                        // Si rowCount es mayor que cero, el tipoPedido existe en ISID
                        return rowCount > 0;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }

                }
            }
        }
        private void InsertarTipoPedidoEnISID(DataTable tipoPedidoDataTable)
        {
            if (tipoPedidoDataTable.Rows.Count > 0)
            {
                string idTipoPedido = tipoPedidoDataTable.Rows[0]["Id_TipoPedido"].ToString();
                string descripcionTipoPedido = tipoPedidoDataTable.Rows[0]["Descripcion_TipoPedido"].ToString();
                string estadisticaVenta = tipoPedidoDataTable.Rows[0]["EstadisticaVenta"].ToString();
                string tipPedidObsAuto = tipoPedidoDataTable.Rows[0]["TipPedidObsAuto"].ToString();

                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblTipoPedido (Id_TipoPedido, Descripcion_TipoPedido, EstadisticaVenta, TipPedidObsAuto) " +
                                  "VALUES (@Id_TipoPedido, @Descripcion_TipoPedido, @EstadisticaVenta, @TipPedidObsAuto)";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Id_TipoPedido", idTipoPedido);
                        cmdInsert.Parameters.AddWithValue("@Descripcion_TipoPedido", descripcionTipoPedido);
                        cmdInsert.Parameters.AddWithValue("@EstadisticaVenta", estadisticaVenta);
                        cmdInsert.Parameters.AddWithValue("@TipPedidObsAuto", tipPedidObsAuto);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }



        //EXPORTAR O ACTUALIZAR  CLIENTE  AL ISID
        private DataTable ObtenerDatoClienteSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblClienteObraContacto.*, tblClienteObra.*, tblOT.Consecutivo_Pedido " +
                              "FROM tblClienteObra INNER JOIN (tblClienteObraContacto INNER JOIN tblOT " +
                              "ON tblClienteObraContacto.IdContacto = tblOT.IDContacto_Cliente) ON tblClienteObra.Nit = tblClienteObraContacto.cocNIT " +
                              "WHERE tblOT.Id_OT= @IdOT  AND tblOT.Consecutivo_Pedido = @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable 
            return dataTable;
        }
        private void ConsultarClienteISID(DataTable datosCliente)
        {
            if (datosCliente.Rows.Count > 0)
            {
                string Nit = datosCliente.Rows[0]["Nit"].ToString();
                string UltimaActualizacionSID = datosCliente.Rows[0]["UltimaActualizacion"].ToString();
                if (!ExistenciaClienteISID(Nit))
                {
                    // Si no existe, realizar la inserción
                    InsertarClienteEnISID(datosCliente);
                }
                else
                {
                    if (!UltimaActualizacionCliente(datosCliente))
                    {
                        ActualizarClienteEnISID(datosCliente);
                    }

                }


            }
        }
        private bool ExistenciaClienteISID(string Nit)
        {
            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT COUNT(*) FROM tblClienteObra WHERE Nit = @Nit";
                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@Nit", Nit);

                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        int rowCount = Convert.ToInt32(result);
                        // Si rowCount es mayor que cero, el cliente existe en ISID
                        return rowCount > 0;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }
                }
            }
        }
        private void InsertarClienteEnISID(DataTable datosCliente)
        {
            if (datosCliente.Rows.Count > 0)
            {
                string Naturaleza = datosCliente.Rows[0]["Naturaleza"].ToString();
                string Tipo_Documento = datosCliente.Rows[0]["Tipo_Documento"].ToString();
                string Nit = datosCliente.Rows[0]["Nit"].ToString();
                string Actividad = datosCliente.Rows[0]["Actividad"].ToString();
                string Tipo_cliente = datosCliente.Rows[0]["Tipo_cliente"].ToString();
                string RazonSocial = datosCliente.Rows[0]["RazonSocial"].ToString();
                string SegundoApellido = datosCliente.Rows[0]["SegundoApellido"].ToString();
                string Nombre = datosCliente.Rows[0]["Nombre"].ToString();
                string Direccion = datosCliente.Rows[0]["Direccion"].ToString();
                string Ciudad = datosCliente.Rows[0]["Ciudad"].ToString();
                string Telefono = datosCliente.Rows[0]["Telefono"].ToString();
                string Fax = datosCliente.Rows[0]["Fax"].ToString();
                string Forma_Pago = datosCliente.Rows[0]["Forma_Pago"].ToString();
                string Zona = datosCliente.Rows[0]["Zona"].ToString();
                string AgenteRetenedor = datosCliente.Rows[0]["AgenteRetenedor"].ToString();
                string GranContribuyente = datosCliente.Rows[0]["GranContribuyente"].ToString();
                string AutoRetenedor = datosCliente.Rows[0]["AutoRetenedor"].ToString();
                string ExentodeRetencion = datosCliente.Rows[0]["ExentodeRetencion"].ToString();
                string DeclaranteRenta = datosCliente.Rows[0]["DeclaranteRenta"].ToString();
                string RetenedorICA = datosCliente.Rows[0]["RetenedorICA"].ToString();
                string RegimenIVA = datosCliente.Rows[0]["RegimenIVA"].ToString();
                string UltimaActualizacion = datosCliente.Rows[0]["UltimaActualizacion"].ToString();
                string FechaCreacion = datosCliente.Rows[0]["FechaCreacion"].ToString();
                string ArchivoRUT = datosCliente.Rows[0]["ArchivoRUT"].ToString();
                string Responsable = datosCliente.Rows[0]["Responsable"].ToString();
                string ArchivoRegistro = datosCliente.Rows[0]["ArchivoRegistro"].ToString();
                string Sector = datosCliente.Rows[0]["sector"].ToString();

                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblClienteObra (Naturaleza, Tipo_Documento, Nit, Actividad,Tipo_cliente,RazonSocial,SegundoApellido,Nombre," +
                                  "Direccion,Ciudad,Telefono,Fax,Forma_Pago,Zona,AgenteRetenedor,GranContribuyente,AutoRetenedor,ExentodeRetencion,DeclaranteRenta," +
                                  "RetenedorICA,RegimenIVA,UltimaActualizacion,FechaCreacion,ArchivoRUT,Responsable,ArchivoRegistro,Sector) " +
                                  "VALUES (@Naturaleza, @Tipo_Documento, @Nit, @Actividad,@Tipo_cliente,@RazonSocial,@SegundoApellido,@Nombre," +
                                  "@Direccion,@Ciudad,@Telefono,@Fax,@Forma_Pago,@Zona,@AgenteRetenedor,@GranContribuyente,@AutoRetenedor,@ExentodeRetencion,@DeclaranteRenta," +
                                  "@RetenedorICA,@RegimenIVA,@UltimaActualizacion,@FechaCreacion,@ArchivoRUT,@Responsable,@ArchivoRegistro,@Sector)";


                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Naturaleza", Naturaleza);
                        cmdInsert.Parameters.AddWithValue("@Tipo_Documento", Tipo_Documento);
                        cmdInsert.Parameters.AddWithValue("@Nit", Nit);
                        cmdInsert.Parameters.AddWithValue("@Actividad", Actividad);
                        cmdInsert.Parameters.AddWithValue("@Tipo_cliente", Tipo_cliente);
                        cmdInsert.Parameters.AddWithValue("@RazonSocial", RazonSocial);
                        cmdInsert.Parameters.AddWithValue("@SegundoApellido", SegundoApellido);
                        cmdInsert.Parameters.AddWithValue("@Nombre", Nombre);
                        cmdInsert.Parameters.AddWithValue("@Direccion", Direccion);
                        cmdInsert.Parameters.AddWithValue("@Ciudad", Ciudad);
                        cmdInsert.Parameters.AddWithValue("@Telefono", Telefono);
                        cmdInsert.Parameters.AddWithValue("@Fax", Fax);
                        cmdInsert.Parameters.AddWithValue("@Forma_Pago", Forma_Pago);
                        cmdInsert.Parameters.AddWithValue("@Zona", Zona);
                        cmdInsert.Parameters.AddWithValue("@AgenteRetenedor", AgenteRetenedor);
                        cmdInsert.Parameters.AddWithValue("@GranContribuyente", GranContribuyente);
                        cmdInsert.Parameters.AddWithValue("@AutoRetenedor", AutoRetenedor);
                        cmdInsert.Parameters.AddWithValue("@ExentodeRetencion", ExentodeRetencion);
                        cmdInsert.Parameters.AddWithValue("@DeclaranteRenta", DeclaranteRenta);
                        cmdInsert.Parameters.AddWithValue("@RetenedorICA", RetenedorICA);
                        cmdInsert.Parameters.AddWithValue("@RegimenIVA", RegimenIVA);
                        cmdInsert.Parameters.AddWithValue("@UltimaActualizacion", Convert.ToDateTime(UltimaActualizacion));
                        cmdInsert.Parameters.AddWithValue("@FechaCreacion", Convert.ToDateTime(FechaCreacion));
                        cmdInsert.Parameters.AddWithValue("@ArchivoRUT", ArchivoRUT);
                        cmdInsert.Parameters.AddWithValue("@Responsable", Responsable);
                        cmdInsert.Parameters.AddWithValue("@ArchivoRegistro", ArchivoRegistro);
                        cmdInsert.Parameters.AddWithValue("@Sector", Sector);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }
        private bool UltimaActualizacionCliente(DataTable datoClienteSID)
        {
            string Nit = datoClienteSID.Rows[0]["Nit"].ToString();
            DateTime FechaActSID = Convert.ToDateTime(datoClienteSID.Rows[0]["UltimaActualizacion"]);

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT UltimaActualizacion FROM tblClienteObra WHERE Nit = @Nit";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@Nit", Nit);


                    DateTime fechaBaseDatosISID;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fechaBaseDatosISID = reader.GetDateTime(0);
                        }
                        else
                        {
                            // No se encontró ningún registro para el NIT dado, retornamos false
                            return false;
                        }
                    }

                    // Comparamos las fechas y retornar el resultado booleano
                    return fechaBaseDatosISID < FechaActSID;
                }
            }
        }
        private void ActualizarClienteEnISID(DataTable datosCliente)
        {
            if (datosCliente.Rows.Count > 0)
            {
                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "UPDATE tblClienteObra SET Naturaleza = @Naturaleza, Tipo_Documento = @Tipo_Documento, Actividad = @Actividad, " +
                                  "Tipo_cliente = @Tipo_cliente, RazonSocial = @RazonSocial, SegundoApellido = @SegundoApellido, Nombre = @Nombre, " +
                                  "Direccion = @Direccion, Ciudad = @Ciudad, Telefono = @Telefono, Fax = @Fax, Forma_Pago = @Forma_Pago, " +
                                  "Zona = @Zona, AgenteRetenedor = @AgenteRetenedor, GranContribuyente = @GranContribuyente, AutoRetenedor = @AutoRetenedor, " +
                                  "ExentodeRetencion = @ExentodeRetencion, DeclaranteRenta = @DeclaranteRenta, RetenedorICA = @RetenedorICA, " +
                                  "RegimenIVA = @RegimenIVA, UltimaActualizacion = @UltimaActualizacion, FechaCreacion = @FechaCreacion, " +
                                  "ArchivoRUT = @ArchivoRUT, Responsable = @Responsable, ArchivoRegistro = @ArchivoRegistro, Sector = @Sector " +
                                  "WHERE Nit = @Nit";

                    using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                    {
                        // Obtener los valores de los parámetros del cliente
                        string Nit = datosCliente.Rows[0]["Nit"].ToString();
                        string Naturaleza = datosCliente.Rows[0]["Naturaleza"].ToString();
                        string Tipo_Documento = datosCliente.Rows[0]["Tipo_Documento"].ToString();
                        string Actividad = datosCliente.Rows[0]["Actividad"].ToString();
                        string Tipo_cliente = datosCliente.Rows[0]["Tipo_cliente"].ToString();
                        string RazonSocial = datosCliente.Rows[0]["RazonSocial"].ToString();
                        string SegundoApellido = datosCliente.Rows[0]["SegundoApellido"].ToString();
                        string Nombre = datosCliente.Rows[0]["Nombre"].ToString();
                        string Direccion = datosCliente.Rows[0]["Direccion"].ToString();
                        string Ciudad = datosCliente.Rows[0]["Ciudad"].ToString();
                        string Telefono = datosCliente.Rows[0]["Telefono"].ToString();
                        string Fax = datosCliente.Rows[0]["Fax"].ToString();
                        string Forma_Pago = datosCliente.Rows[0]["Forma_Pago"].ToString();
                        string Zona = datosCliente.Rows[0]["Zona"].ToString();
                        string AgenteRetenedor = datosCliente.Rows[0]["AgenteRetenedor"].ToString();
                        string GranContribuyente = datosCliente.Rows[0]["GranContribuyente"].ToString();
                        string AutoRetenedor = datosCliente.Rows[0]["AutoRetenedor"].ToString();
                        string ExentodeRetencion = datosCliente.Rows[0]["ExentodeRetencion"].ToString();
                        string DeclaranteRenta = datosCliente.Rows[0]["DeclaranteRenta"].ToString();
                        string RetenedorICA = datosCliente.Rows[0]["RetenedorICA"].ToString();
                        string RegimenIVA = datosCliente.Rows[0]["RegimenIVA"].ToString();
                        string UltimaActualizacion = datosCliente.Rows[0]["UltimaActualizacion"].ToString();
                        string FechaCreacion = datosCliente.Rows[0]["FechaCreacion"].ToString();
                        string ArchivoRUT = datosCliente.Rows[0]["ArchivoRUT"].ToString();
                        string Responsable = datosCliente.Rows[0]["Responsable"].ToString();
                        string ArchivoRegistro = datosCliente.Rows[0]["ArchivoRegistro"].ToString();
                        string Sector = datosCliente.Rows[0]["Sector"].ToString();

                        // Asignar los valores de los parámetros
                        cmdUpdate.Parameters.AddWithValue("@Naturaleza", Naturaleza);
                        cmdUpdate.Parameters.AddWithValue("@Tipo_Documento", Tipo_Documento);
                        cmdUpdate.Parameters.AddWithValue("@Actividad", Actividad);
                        cmdUpdate.Parameters.AddWithValue("@Tipo_cliente", Tipo_cliente);
                        cmdUpdate.Parameters.AddWithValue("@RazonSocial", RazonSocial);
                        cmdUpdate.Parameters.AddWithValue("@SegundoApellido", SegundoApellido);
                        cmdUpdate.Parameters.AddWithValue("@Nombre", Nombre);
                        cmdUpdate.Parameters.AddWithValue("@Direccion", Direccion);
                        cmdUpdate.Parameters.AddWithValue("@Ciudad", Ciudad);
                        cmdUpdate.Parameters.AddWithValue("@Telefono", Telefono);
                        cmdUpdate.Parameters.AddWithValue("@Fax", Fax);
                        cmdUpdate.Parameters.AddWithValue("@Forma_Pago", Forma_Pago);
                        cmdUpdate.Parameters.AddWithValue("@Zona", Zona);
                        cmdUpdate.Parameters.AddWithValue("@AgenteRetenedor", AgenteRetenedor);
                        cmdUpdate.Parameters.AddWithValue("@GranContribuyente", GranContribuyente);
                        cmdUpdate.Parameters.AddWithValue("@AutoRetenedor", AutoRetenedor);
                        cmdUpdate.Parameters.AddWithValue("@ExentodeRetencion", ExentodeRetencion);
                        cmdUpdate.Parameters.AddWithValue("@DeclaranteRenta", DeclaranteRenta);
                        cmdUpdate.Parameters.AddWithValue("@RetenedorICA", RetenedorICA);
                        cmdUpdate.Parameters.AddWithValue("@RegimenIVA", RegimenIVA);
                        cmdUpdate.Parameters.AddWithValue("@UltimaActualizacion", Convert.ToDateTime(UltimaActualizacion));
                        cmdUpdate.Parameters.AddWithValue("@FechaCreacion", Convert.ToDateTime(FechaCreacion));
                        cmdUpdate.Parameters.AddWithValue("@ArchivoRUT", ArchivoRUT);
                        cmdUpdate.Parameters.AddWithValue("@Responsable", Responsable);
                        cmdUpdate.Parameters.AddWithValue("@ArchivoRegistro", ArchivoRegistro);
                        cmdUpdate.Parameters.AddWithValue("@Sector", Sector);
                        cmdUpdate.Parameters.AddWithValue("@Nit", Nit);

                        // Ejecutar la actualización
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
        }


        //EXPORTAR O ACTUALIZAR  CONTACTO AL ISID
        private bool ExistenciaContacto(DataTable DatoCliente)
        {
            string Nit = DatoCliente.Rows[0]["Nit"].ToString();
            string consecutivo = DatoCliente.Rows[0]["cocConsecutivoContacto"].ToString();

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT tblClienteObraContacto.* FROM tblClienteObraContacto where cocNit= @Nit AND cocConsecutivoContacto= @consecutivo ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@Nit", Nit);
                    cmd.Parameters.AddWithValue("@consecutivo", consecutivo);
                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        int rowCount = Convert.ToInt32(result);
                        // Si rowCount es mayor que cero, el Contacto existe en ISID
                        return rowCount > 0;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }
                }
            }
        }
        private void InsertarContactoClienteISID(DataTable datosCliente)
        {
            if (datosCliente.Rows.Count > 0)
            {
                string cocNIT = datosCliente.Rows[0]["cocNIT"].ToString();
                string cocConsecutivoContacto = datosCliente.Rows[0]["cocConsecutivoContacto"].ToString();
                string cocSede = datosCliente.Rows[0]["cocSede"].ToString();
                string cocDireccion = datosCliente.Rows[0]["cocDireccion"].ToString();
                string cocCiudad = datosCliente.Rows[0]["cocCiudad"].ToString();
                string cocNombre = datosCliente.Rows[0]["cocNombre"].ToString();
                string cocTelefono = datosCliente.Rows[0]["cocTelefono"].ToString();
                string cocCelular = datosCliente.Rows[0]["cocCelular"].ToString();
                string cocMail = datosCliente.Rows[0]["cocMail"].ToString();
                string cocFechaCreacion = datosCliente.Rows[0]["cocFechaCreacion"].ToString();
                string cocUltimaActualizacion = datosCliente.Rows[0]["cocUltimaActualizacion"].ToString();


                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblClienteObraContacto (cocNIT, cocConsecutivoContacto, cocSede, cocDireccion,cocCiudad,cocNombre,cocTelefono," +
                                  "cocCelular,cocMail,cocFechaCreacion,cocUltimaActualizacion) " +
                                  "VALUES (@cocNIT, @cocConsecutivoContacto, @cocSede, @cocDireccion,@cocCiudad,@cocNombre,@cocTelefono,@cocCelular,@cocMail," +
                                  "@cocFechaCreacion,@cocUltimaActualizacion)";


                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@cocNIT", cocNIT);
                        cmdInsert.Parameters.AddWithValue("@cocConsecutivoContacto", cocConsecutivoContacto);
                        cmdInsert.Parameters.AddWithValue("@cocSede", cocSede);
                        cmdInsert.Parameters.AddWithValue("@cocDireccion", cocDireccion);
                        cmdInsert.Parameters.AddWithValue("@cocCiudad", cocCiudad);
                        cmdInsert.Parameters.AddWithValue("@cocNombre", cocNombre);
                        cmdInsert.Parameters.AddWithValue("@cocTelefono", cocTelefono);
                        cmdInsert.Parameters.AddWithValue("@cocCelular", cocCelular);
                        cmdInsert.Parameters.AddWithValue("@cocMail", cocMail);
                        cmdInsert.Parameters.AddWithValue("@cocFechaCreacion", string.IsNullOrEmpty(cocFechaCreacion) ? (object)DBNull.Value : Convert.ToDateTime(cocFechaCreacion));
                        cmdInsert.Parameters.AddWithValue("@cocUltimaActualizacion", Convert.ToDateTime(cocUltimaActualizacion));


                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }
        private bool UltimaActualizacionContactoCliente(DataTable datoClienteSID)
        {
            string Nit = datoClienteSID.Rows[0]["Nit"].ToString();
            string consecutivo = datoClienteSID.Rows[0]["cocConsecutivoContacto"].ToString();
            DateTime FechaActSID = Convert.ToDateTime(datoClienteSID.Rows[0]["UltimaActualizacion"].ToString());

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT cocUltimaActualizacion FROM tblClienteObraContacto where cocNit= @Nit AND cocConsecutivoContacto= @consecutivo";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@Nit", Nit);
                    cmd.Parameters.AddWithValue("@consecutivo", consecutivo);

                    DateTime fechaBaseDatosISID;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            fechaBaseDatosISID = reader.GetDateTime(0);
                        }
                        else
                        {
                            // No se encontró ningún registro para el NIT dado, retornamos false
                            return false;
                        }
                    }

                    // Comparamos las fechas y retornar el resultado booleano
                    return fechaBaseDatosISID < FechaActSID;
                }
            }
        }
        private void ActualizarContactoClienteISID(DataTable datosCliente)
        {
            if (datosCliente.Rows.Count > 0)
            {
                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "UPDATE tblClienteObraContacto SET cocNIT = @cocNIT, cocConsecutivoContacto= @cocConsecutivoContacto," +
                                  "cocSede = @cocSede, cocDireccion = @cocDireccion,cocCiudad = @cocCiudad, cocNombre = @cocNombre, cocTelefono = @cocTelefono, " +
                                  "cocCelular = @cocCelular, cocMail = @cocMail, cocFechaCreacion = @cocFechaCreacion, " +
                                  "cocUltimaActualizacion = @cocUltimaActualizacion WHERE cocNIT = @cocNIT AND " +
                                  "cocConsecutivoContacto = @cocConsecutivoContacto";

                    using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                    {

                        string cocNIT = datosCliente.Rows[0]["cocNIT"].ToString();
                        string cocConsecutivoContacto = datosCliente.Rows[0]["cocConsecutivoContacto"].ToString();
                        string cocSede = datosCliente.Rows[0]["cocSede"].ToString();
                        string cocDireccion = datosCliente.Rows[0]["cocDireccion"].ToString();
                        string cocCiudad = datosCliente.Rows[0]["cocCiudad"].ToString();
                        string cocNombre = datosCliente.Rows[0]["cocNombre"].ToString();
                        string cocTelefono = datosCliente.Rows[0]["cocTelefono"].ToString();
                        string cocCelular = datosCliente.Rows[0]["cocCelular"].ToString();
                        string cocMail = datosCliente.Rows[0]["cocMail"].ToString();
                        string cocFechaCreacion = datosCliente.Rows[0]["cocFechaCreacion"].ToString();
                        string cocUltimaActualizacion = datosCliente.Rows[0]["cocUltimaActualizacion"].ToString();

                        // Asignar los valores de los parámetros
                        cmdUpdate.Parameters.AddWithValue("@cocNIT", cocNIT);
                        cmdUpdate.Parameters.AddWithValue("@cocConsecutivoContacto", cocConsecutivoContacto);
                        cmdUpdate.Parameters.AddWithValue("@cocSede", cocSede);
                        cmdUpdate.Parameters.AddWithValue("@cocDireccion", cocDireccion);
                        cmdUpdate.Parameters.AddWithValue("@cocCiudad", cocCiudad);
                        cmdUpdate.Parameters.AddWithValue("@cocNombre", cocNombre);
                        cmdUpdate.Parameters.AddWithValue("@cocTelefono", cocTelefono);
                        cmdUpdate.Parameters.AddWithValue("@cocCelular", cocCelular);
                        cmdUpdate.Parameters.AddWithValue("@cocMail", cocMail);
                        cmdUpdate.Parameters.AddWithValue("@cocFechaCreacion", string.IsNullOrEmpty(cocFechaCreacion) ? (object)DBNull.Value : Convert.ToDateTime(cocFechaCreacion));
                        cmdUpdate.Parameters.AddWithValue("@cocUltimaActualizacion", Convert.ToDateTime(cocUltimaActualizacion));

                        // Ejecutar la actualización
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
        }
        private int ConsultarIDContacto(DataTable datoClienteSID)
        {
            string Nit = datoClienteSID.Rows[0]["Nit"].ToString();
            string consecutivo = datoClienteSID.Rows[0]["cocConsecutivoContacto"].ToString();
            int IDContacto = 0; // Inicializamos como 0, podría ser otro valor predeterminado si es apropiado

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT IdContacto FROM tblClienteObraContacto WHERE cocNit = @Nit AND cocConsecutivoContacto = @consecutivo";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@Nit", Nit);
                    cmd.Parameters.AddWithValue("@consecutivo", consecutivo);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IDContacto = reader.GetInt32(0);
                        }
                    }

                    return IDContacto;
                }
            }
        }


        // INSERTAR ASESOR SI NO EXISTE  AL ISID
        private DataTable ObtenerAsesorComercialSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblAsesorComercial.* FROM tblOT INNER JOIN tblAsesorComercial ON tblOT.Codigo_Asesor = tblAsesorComercial.CodigoAsesor" +
                              " WHERE (((tblOT.Id_OT)= @IdOT ) AND ((tblOT.Consecutivo_Pedido)= @Pedido )) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable que puede contener cero o más filas de resultados
            return dataTable;
        }
        private void ConsultarAsesorISID(DataTable DatoAsesor)
        {
            if (DatoAsesor.Rows.Count > 0)
            {
                string Cedula = DatoAsesor.Rows[0]["Cedula"].ToString();

                // Verificar si el IdTipoPedido ya existe en la otra base de datos (ISID)
                if (!ExisteAsesorEnISID(Cedula))
                {
                    // Si no existe, realizar la inserción
                    InsertarAsesorEnISID(DatoAsesor);
                }

            }
        }
        private bool ExisteAsesorEnISID(string Cedula)
        {
            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT * FROM tblAsesorComercial where Cedula=  @Cedula";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@Cedula", Cedula);

                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        int rowCount = Convert.ToInt32(result);
                        // Si rowCount es mayor que cero, el Asesor ya existe en ISID
                        return rowCount > 0;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }
                }
            }
        }
        private void InsertarAsesorEnISID(DataTable DatoAsesor)
        {
            if (DatoAsesor.Rows.Count > 0)
            {
                string Cedula = DatoAsesor.Rows[0]["Cedula"].ToString();
                string Apellidos = DatoAsesor.Rows[0]["Apellidos"].ToString();
                string Nombre = DatoAsesor.Rows[0]["Nombre"].ToString();
                string Direccion = DatoAsesor.Rows[0]["Dirección"].ToString();
                string Ciudad = DatoAsesor.Rows[0]["Ciudad"].ToString();
                string TelDomicilio = DatoAsesor.Rows[0]["TelDomicilio"].ToString();
                string codigoasesor = DatoAsesor.Rows[0]["codigoasesor"].ToString();
                string mail = DatoAsesor.Rows[0]["mail"].ToString();
                string Activo = DatoAsesor.Rows[0]["Activo"].ToString();
                string Zona = DatoAsesor.Rows[0]["Zona"].ToString();

                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblAsesorComercial (Cedula, Apellidos, Nombre, Dirección,Ciudad,TelDomicilio,CodigoAsesor,mail,Activo,Zona) " +
                                  "VALUES (@Cedula, @Apellidos,@Nombre, @Direccion, @Ciudad,@TelDomicilio,@codigoasesor,@mail,@Activo,@Zona)";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Cedula", Cedula);
                        cmdInsert.Parameters.AddWithValue("@Apellidos", Apellidos);
                        cmdInsert.Parameters.AddWithValue("@Nombre", Nombre);
                        cmdInsert.Parameters.AddWithValue("@Direccion", Direccion);
                        cmdInsert.Parameters.AddWithValue("@Ciudad", Ciudad);
                        cmdInsert.Parameters.AddWithValue("@TelDomicilio", TelDomicilio);
                        cmdInsert.Parameters.AddWithValue("@codigoasesor", codigoasesor);
                        cmdInsert.Parameters.AddWithValue("@mail", mail);
                        cmdInsert.Parameters.AddWithValue("@Activo", Activo);
                        cmdInsert.Parameters.AddWithValue("@Zona", Zona);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }


        //Obtener los datos de pedido de la tabla reportes 
        private DataTable ObtenerInfoPedidoSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblreporteOT WHERE Id_OT= @IdOT AND Consecutivo_Pedido = @Pedido ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable que puede contener cero o más filas de resultados
            return dataTable;
        }


        //Obtener  pedido si esta  detenido por produccion o despacho
        private DataTable ObtenerPedDetProduDespISID(DataTable infoPed)
        {

            string pedidoBase = infoPed.Rows[0]["PedidoBase"].ToString();
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  * FROM tblreporteOT WHERE  Id_OT= @IdOT AND Consecutivo_Pedido= @pedidoBase AND (PararProduccion=1 or PararDespacho=1)";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedidoBase", pedidoBase);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable que puede contener cero o más filas de resultados
            return dataTable;
        }


        //Reemplazr pedido en ISID 
        private bool ExistenciaPedidoISID()
        {
            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT COUNT(*) FROM tblReporteOT WHERE Id_OT= @IdOT AND Consecutivo_Pedido = @pedido ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        int rowCount = Convert.ToInt32(result);
                        // Si rowCount es mayor que cero, el pedido existe en ISID
                        return rowCount > 0;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }

                }
            }
        }
        private void InsertarPedidoISID(DataTable InfoPedidoSID, bool OKDibujo, int ID_CONTACTO, bool PararProduccion, bool PararDespacho)
        {
            if (InfoPedidoSID.Rows.Count > 0)
            {
                string IdOT = InfoPedidoSID.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = InfoPedidoSID.Rows[0]["Consecutivo_Pedido"].ToString();
                string Nombre_Obra = InfoPedidoSID.Rows[0]["Nombre_Obra"].ToString();
                string Observacion_Pedido = InfoPedidoSID.Rows[0]["Observacion_Pedido"].ToString();
                string Dirección = InfoPedidoSID.Rows[0]["Dirección"].ToString();
                string Ciudad = InfoPedidoSID.Rows[0]["Ciudad"].ToString();
                string Región = InfoPedidoSID.Rows[0]["Región"].ToString();
                string País = InfoPedidoSID.Rows[0]["País"].ToString();
                string TelDomicilio = InfoPedidoSID.Rows[0]["TelDomicilio"].ToString();
                string Fecha_Confirmacion_Venta = InfoPedidoSID.Rows[0]["Fecha_Confirmacion_Venta"].ToString();
                string Fecha_Entrega_Dibujo_Despiece = InfoPedidoSID.Rows[0]["Fecha_Entrega_Dibujo_Despiece"].ToString();
                string Fecha_Entrega_Produccion = InfoPedidoSID.Rows[0]["Fecha_Entrega_Produccion"].ToString();
                string Fecha_Despacho_Produccion = InfoPedidoSID.Rows[0]["Fecha_Despacho_Produccion"].ToString();
                string Fecha_Real_Despacho_Produccion = InfoPedidoSID.Rows[0]["Fecha_Real_Despacho_Produccion"].ToString();
                string Fecha_Instalacion = InfoPedidoSID.Rows[0]["Fecha_Instalacion"].ToString();
                string Persona_Receptora = InfoPedidoSID.Rows[0]["Persona_Receptora"].ToString();
                string Codigo_Asesor = InfoPedidoSID.Rows[0]["Codigo_Asesor"].ToString();
                string Zona = InfoPedidoSID.Rows[0]["Zona"].ToString();
                string Descuento = InfoPedidoSID.Rows[0]["Descuento"].ToString();
                string Precio_Venta = InfoPedidoSID.Rows[0]["Precio_Venta"].ToString();
                string Forma_Pago = InfoPedidoSID.Rows[0]["Forma_Pago"].ToString();
                string cotizacion = InfoPedidoSID.Rows[0]["cotizacion"].ToString();
                string Observaciones_Contables = InfoPedidoSID.Rows[0]["Observaciones_Contables"].ToString();
                string Mail_Contacto = InfoPedidoSID.Rows[0]["Mail_Contacto"].ToString();
                string OC_Sag = InfoPedidoSID.Rows[0]["OC_Sag"].ToString();
                string OF_Sag = InfoPedidoSID.Rows[0]["OF_Sag"].ToString();
                string Id_OT_Secundario = InfoPedidoSID.Rows[0]["Id_OT_Secundario"].ToString();
                string Id_TipoPedido = InfoPedidoSID.Rows[0]["Id_TipoPedido"].ToString();
                string RecibeElPedido = InfoPedidoSID.Rows[0]["RecibeElPedido"].ToString();
                string ResumenObra = InfoPedidoSID.Rows[0]["ResumenObra"].ToString();
                string Aplica_Empaque = InfoPedidoSID.Rows[0]["Aplica_Empaque"].ToString();
                string Supervisor = InfoPedidoSID.Rows[0]["Supervisor"].ToString();
                string Reactivada = InfoPedidoSID.Rows[0]["Reactivada"].ToString();
                string chequeada = InfoPedidoSID.Rows[0]["chequeada"].ToString();
                string FabricadoPor = InfoPedidoSID.Rows[0]["FabricadoPor"].ToString();
                string InstaladaPor = InfoPedidoSID.Rows[0]["InstaladaPor"].ToString();
                string ValorPedido = InfoPedidoSID.Rows[0]["ValorPedido"].ToString();
                string Observacion_Pedido1 = InfoPedidoSID.Rows[0]["Observacion_Pedido"].ToString();
                string CelularContacto = InfoPedidoSID.Rows[0]["CelularContacto"].ToString();
                string DescuentoparaComision = InfoPedidoSID.Rows[0]["DescuentoparaComision"].ToString();
                string ValorTteVia = InfoPedidoSID.Rows[0]["ValorTteVia"].ToString();
                string ValorBolsa = InfoPedidoSID.Rows[0]["ValorBolsa"].ToString();
                string Pedidobase = InfoPedidoSID.Rows[0]["Pedidobase"].ToString();
                string ValorViatico = InfoPedidoSID.Rows[0]["ValorViatico"].ToString();
                string Fecha_Empaque = InfoPedidoSID.Rows[0]["Fecha_Empaque"].ToString();
                string Fecha_Empaque_Venta = InfoPedidoSID.Rows[0]["Fecha_Empaque_Venta"].ToString();
                string OrdendeCompra = InfoPedidoSID.Rows[0]["OrdendeCompra"].ToString();


                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblReporteOT (Id_OT,Consecutivo_Pedido,Nombre_Obra,Observacion_Pedido,Dirección,Ciudad,Región,país,TelDomicilio," +
                                  "Fecha_Confirmacion_Venta,Fecha_Entrega_Dibujo_Despiece,Fecha_Entrega_Produccion,Fecha_Despacho_Produccion,Fecha_Real_Despacho_Produccion," +
                                  "Fecha_Instalacion,Fecha_Terminada_Empaque,Fecha_Terminada_Despacho,Fecha_Despacho_Terceros,Fecha_Factura,Persona_Receptora,Terminado_Diseño," +
                                  "Terminado_Ventas,Terminada_Instalacion,Terminada_Facturacion,Terminada_Empaque,IDContacto_Cliente,Codigo_Asesor,Zona,Descuento,Precio_Venta," +
                                  " Forma_Pago, Cotizacion, Observaciones_Contables, Plano, bitacora, ordencompra, chkCotizacion, Chequeo_Medidas, Tipo_Sujecion," +
                                  " Perfil_Refuerzo_Superior, Espesor_Tipo_Superficies, Color_Tipo_PVC, Datos_PasaCables, Bajantes_Electricos, Datos_Troquel, Mail_Contacto," +
                                  " OC_Sag, OF_Sag, Acta_Entrega, Id_OT_Secundario, Id_TipoPedido, Cerrada, Anulada, RecibeElPedido, NoFactura, ResumenObra, Aplica_Empaque," +
                                  " Fecha_Cierre, Fecha_Final_Instalacion, Fecha_Anulada, Supervisor, Fecha_Terminada_Almacen, Reactivada, chequeada, FabricadoPor," +
                                  " InstaladaPor, Observacion_Ventas,ValorPedido,CelularContacto,DescuentoparaComision,ValorTteVia, ValorBolsa, PararProduccion," +
                                  " PararDespacho,PedidoBase, ValorViatico, Fecha_Empaque, Fecha_Empaque_Venta,Fecha_Habilitada_paraProducir,OrdendeCompra) " +

                                  "VALUES (@Id_OT, @Consecutivo_Pedido, @Nombre_Obra, @Observacion_Pedido, @Dirección, @Ciudad, @Región, @País, @TelDomicilio," +
                                  "@Fecha_Confirmacion_Venta, @Fecha_Entrega_Dibujo_Despiece, @Fecha_Entrega_Produccion,@Fecha_Despacho_Produccion, @Fecha_Real_Despacho_Produccion," +
                                  "@Fecha_Instalacion,Null, Null, Null, Null, @Persona_Receptora, @OKDibujo, 1, 0, 0, 0, @ID_CONTACTO, @Codigo_Asesor, @Zona, @Descuento, @Precio_Venta," +
                                  "@Forma_Pago, @cotizacion, @Observaciones_Contables, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, @Mail_Contacto, @OC_Sag, @OF_Sag, 0, @Id_OT_Secundario," +
                                  "@Id_TipoPedido, 0, 0 , @RecibeElPedido, 0, @ResumenObra,@Aplica_Empaque, Null, Null, Null, @Supervisor, Null, @Reactivada, @chequeada, @FabricadoPor, @InstaladaPor," +
                                  "@Observacion_Pedido1, @ValorPedido, @CelularContacto, @DescuentoparaComision, @ValorTteVia, @ValorBolsa, @PararProduccion, @PararDespacho, @Pedidobase," +
                                  "@ValorViatico, @Fecha_Empaque, @Fecha_Empaque_Venta, @Fecha_Entrega_Produccion ,@OrdendeCompra )";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Id_OT", IdOT);
                        cmdInsert.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdInsert.Parameters.AddWithValue("@Nombre_Obra", Nombre_Obra);
                        cmdInsert.Parameters.AddWithValue("@Observacion_Pedido", Observacion_Pedido);
                        cmdInsert.Parameters.AddWithValue("@Dirección", Dirección);
                        cmdInsert.Parameters.AddWithValue("@Ciudad", Ciudad);
                        cmdInsert.Parameters.AddWithValue("@Región", Región);
                        cmdInsert.Parameters.AddWithValue("@País", País);
                        cmdInsert.Parameters.AddWithValue("@TelDomicilio", TelDomicilio);
                        cmdInsert.Parameters.AddWithValue("@Fecha_Confirmacion_Venta", Convert.ToDateTime(Fecha_Confirmacion_Venta));
                        cmdInsert.Parameters.AddWithValue("@Fecha_Entrega_Dibujo_Despiece", Convert.ToDateTime(Fecha_Entrega_Dibujo_Despiece));
                        cmdInsert.Parameters.AddWithValue("@Fecha_Entrega_Produccion", Convert.ToDateTime(Fecha_Entrega_Produccion));
                        cmdInsert.Parameters.AddWithValue("@Fecha_Despacho_Produccion", Convert.ToDateTime(Fecha_Despacho_Produccion));
                        cmdInsert.Parameters.AddWithValue("@Fecha_Real_Despacho_Produccion", Convert.ToDateTime(Fecha_Real_Despacho_Produccion));
                        cmdInsert.Parameters.AddWithValue("@Fecha_Instalacion", Convert.ToDateTime(Fecha_Instalacion));
                        cmdInsert.Parameters.AddWithValue("@Persona_Receptora", Persona_Receptora);
                        cmdInsert.Parameters.AddWithValue("@OKDibujo", OKDibujo);
                        cmdInsert.Parameters.AddWithValue("@ID_CONTACTO", ID_CONTACTO);
                        cmdInsert.Parameters.AddWithValue("@Codigo_Asesor", Codigo_Asesor);
                        cmdInsert.Parameters.AddWithValue("@Zona", Zona);
                        cmdInsert.Parameters.AddWithValue("@Descuento", Descuento);
                        cmdInsert.Parameters.AddWithValue("@Precio_Venta", Precio_Venta);
                        cmdInsert.Parameters.AddWithValue("@Forma_Pago", Forma_Pago);
                        cmdInsert.Parameters.AddWithValue("@cotizacion", cotizacion);
                        cmdInsert.Parameters.AddWithValue("@Observaciones_Contables", Observaciones_Contables);
                        cmdInsert.Parameters.AddWithValue("@Mail_Contacto", Mail_Contacto);
                        cmdInsert.Parameters.AddWithValue("@OC_Sag", OC_Sag);
                        cmdInsert.Parameters.AddWithValue("@OF_Sag", OF_Sag);
                        cmdInsert.Parameters.AddWithValue("@Id_OT_Secundario", Id_OT_Secundario);
                        cmdInsert.Parameters.AddWithValue("@Id_TipoPedido", Id_TipoPedido);
                        cmdInsert.Parameters.AddWithValue("@RecibeElPedido", RecibeElPedido);
                        cmdInsert.Parameters.AddWithValue("@ResumenObra", ResumenObra);
                        cmdInsert.Parameters.AddWithValue("@Aplica_Empaque", Aplica_Empaque);
                        cmdInsert.Parameters.AddWithValue("@Supervisor", Supervisor);
                        cmdInsert.Parameters.AddWithValue("@Reactivada", Reactivada);
                        cmdInsert.Parameters.AddWithValue("@chequeada", chequeada);
                        cmdInsert.Parameters.AddWithValue("@FabricadoPor", FabricadoPor);
                        cmdInsert.Parameters.AddWithValue("@InstaladaPor", InstaladaPor);
                        cmdInsert.Parameters.AddWithValue("@Observacion_Pedido1", Observacion_Pedido1);
                        cmdInsert.Parameters.AddWithValue("@ValorPedido", ValorPedido);
                        cmdInsert.Parameters.AddWithValue("@CelularContacto", CelularContacto);
                        cmdInsert.Parameters.AddWithValue("@DescuentoparaComision", DescuentoparaComision);
                        cmdInsert.Parameters.AddWithValue("@ValorTteVia", ValorTteVia);
                        cmdInsert.Parameters.AddWithValue("@ValorBolsa", ValorBolsa);
                        cmdInsert.Parameters.AddWithValue("@PararProduccion", PararProduccion);
                        cmdInsert.Parameters.AddWithValue("@PararDespacho", PararDespacho);
                        cmdInsert.Parameters.AddWithValue("@Pedidobase", Pedidobase);
                        cmdInsert.Parameters.AddWithValue("@ValorViatico", ValorViatico);
                        cmdInsert.Parameters.AddWithValue("@Fecha_Empaque", Convert.ToDateTime(Fecha_Empaque));
                        cmdInsert.Parameters.AddWithValue("@Fecha_Empaque_Venta", Convert.ToDateTime(Fecha_Empaque_Venta));
                        cmdInsert.Parameters.AddWithValue("@OrdendeCompra", OrdendeCompra);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }
        private void ActualizarPedidoISID(DataTable InfoPedidoSID, bool OKDibujo, int ID_CONTACTO, bool PararProduccion, bool PararDespacho)
        {
            if (InfoPedidoSID.Rows.Count > 0)
            {
                string IdOT = InfoPedidoSID.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = InfoPedidoSID.Rows[0]["Consecutivo_Pedido"].ToString();
                string Nombre_Obra = InfoPedidoSID.Rows[0]["Nombre_Obra"].ToString();
                string Observacion_Pedido = InfoPedidoSID.Rows[0]["Observacion_Pedido"].ToString();
                string Dirección = InfoPedidoSID.Rows[0]["Dirección"].ToString();
                string Ciudad = InfoPedidoSID.Rows[0]["Ciudad"].ToString();
                string Región = InfoPedidoSID.Rows[0]["Región"].ToString();
                string País = InfoPedidoSID.Rows[0]["País"].ToString();
                string TelDomicilio = InfoPedidoSID.Rows[0]["TelDomicilio"].ToString();
                string Fecha_Confirmacion_Venta = InfoPedidoSID.Rows[0]["Fecha_Confirmacion_Venta"].ToString();
                string Fecha_Entrega_Dibujo_Despiece = InfoPedidoSID.Rows[0]["Fecha_Entrega_Dibujo_Despiece"].ToString();
                string Fecha_Entrega_Produccion = InfoPedidoSID.Rows[0]["Fecha_Entrega_Produccion"].ToString();
                string Fecha_Despacho_Produccion = InfoPedidoSID.Rows[0]["Fecha_Despacho_Produccion"].ToString();
                string Fecha_Real_Despacho_Produccion = InfoPedidoSID.Rows[0]["Fecha_Real_Despacho_Produccion"].ToString();
                string Fecha_Instalacion = InfoPedidoSID.Rows[0]["Fecha_Instalacion"].ToString();
                string Persona_Receptora = InfoPedidoSID.Rows[0]["Persona_Receptora"].ToString();
                string Codigo_Asesor = InfoPedidoSID.Rows[0]["Codigo_Asesor"].ToString();
                string Zona = InfoPedidoSID.Rows[0]["Zona"].ToString();
                string Descuento = InfoPedidoSID.Rows[0]["Descuento"].ToString();
                string Precio_Venta = InfoPedidoSID.Rows[0]["Precio_Venta"].ToString();
                string Forma_Pago = InfoPedidoSID.Rows[0]["Forma_Pago"].ToString();
                string cotizacion = InfoPedidoSID.Rows[0]["cotizacion"].ToString();
                string Observaciones_Contables = InfoPedidoSID.Rows[0]["Observaciones_Contables"].ToString();
                string Mail_Contacto = InfoPedidoSID.Rows[0]["Mail_Contacto"].ToString();
                string OC_Sag = InfoPedidoSID.Rows[0]["OC_Sag"].ToString();
                string OF_Sag = InfoPedidoSID.Rows[0]["OF_Sag"].ToString();
                string Id_OT_Secundario = InfoPedidoSID.Rows[0]["Id_OT_Secundario"].ToString();
                string Id_TipoPedido = InfoPedidoSID.Rows[0]["Id_TipoPedido"].ToString();
                string RecibeElPedido = InfoPedidoSID.Rows[0]["RecibeElPedido"].ToString();
                string ResumenObra = InfoPedidoSID.Rows[0]["ResumenObra"].ToString();
                string Aplica_Empaque = InfoPedidoSID.Rows[0]["Aplica_Empaque"].ToString();
                string Supervisor = InfoPedidoSID.Rows[0]["Supervisor"].ToString();
                string Reactivada = InfoPedidoSID.Rows[0]["Reactivada"].ToString();
                string FabricadoPor = InfoPedidoSID.Rows[0]["FabricadoPor"].ToString();
                string ValorPedido = InfoPedidoSID.Rows[0]["ValorPedido"].ToString();
                string Observacion_Pedido1 = InfoPedidoSID.Rows[0]["Observacion_Pedido"].ToString();
                string CelularContacto = InfoPedidoSID.Rows[0]["CelularContacto"].ToString();
                string DescuentoparaComision = InfoPedidoSID.Rows[0]["DescuentoparaComision"].ToString();
                string ValorTteVia = InfoPedidoSID.Rows[0]["ValorTteVia"].ToString();
                string ValorBolsa = InfoPedidoSID.Rows[0]["ValorBolsa"].ToString();
                string Pedidobase = InfoPedidoSID.Rows[0]["Pedidobase"].ToString();
                string ValorViatico = InfoPedidoSID.Rows[0]["ValorViatico"].ToString();
                string Fecha_Empaque = InfoPedidoSID.Rows[0]["Fecha_Empaque"].ToString();
                string Fecha_Empaque_Venta = InfoPedidoSID.Rows[0]["Fecha_Empaque_Venta"].ToString();
                string Fecha_Entrega_Produccion1 = InfoPedidoSID.Rows[0]["Fecha_Entrega_Produccion"].ToString();
                string OrdendeCompra = InfoPedidoSID.Rows[0]["OrdendeCompra"].ToString();

                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "UPDATE tblReporteOT SET Nombre_Obra = @Nombre_Obra, Observacion_Pedido = @Observacion_Pedido, Dirección = @Dirección, Ciudad = @Ciudad, Región = @Región," +
                                  "país = @País, TelDomicilio = @TelDomicilio, Fecha_Confirmacion_Venta= @Fecha_Confirmacion_Venta, Fecha_Entrega_Dibujo_Despiece= @Fecha_Entrega_Dibujo_Despiece," +
                                  "Fecha_Entrega_Produccion = @Fecha_Entrega_Produccion, Fecha_Despacho_Produccion= @Fecha_Despacho_Produccion, Fecha_Real_Despacho_Produccion= @Fecha_Real_Despacho_Produccion," +
                                  "Fecha_Instalacion = @Fecha_Instalacion, Persona_Receptora= @Persona_Receptora, Terminado_Diseño =  @OKDibujo, IDContacto_Cliente = @ID_CONTACTO, Codigo_Asesor = @Codigo_Asesor," +
                                  "Zona = @Zona, Descuento= @Descuento, Precio_Venta= @Precio_Venta, Forma_Pago = @Forma_Pago, Cotizacion = @cotizacion, Observaciones_Contables = @Observaciones_Contables," +
                                  "Mail_Contacto= @Mail_Contacto, OC_Sag = @OC_Sag, OF_Sag= @OF_Sag, Acta_Entrega=0, Id_OT_Secundario = @Id_OT_Secundario, Id_TipoPedido = @Id_TipoPedido, ValorBolsa = @ValorBolsa," +
                                  "RecibeElPedido = @RecibeElPedido, ResumenObra = @ResumenObra, Aplica_Empaque = @Aplica_Empaque, Supervisor = @Supervisor, Reactivada= @Reactivada, FabricadoPor= @FabricadoPor," +
                                  "Observacion_Ventas= @Observacion_Pedido1, ValorPedido = @ValorPedido, ValorViatico = @ValorViatico, CelularContacto = @CelularContacto, DescuentoparaComision = @DescuentoparaComision," +
                                  " ValorTteVia = @ValorTteVia, PararProduccion= @PararProduccion, PararDespacho = @PararDespacho, PedidoBase= @Pedidobase, Fecha_Empaque= @Fecha_Empaque," +
                                  "Fecha_Empaque_Venta= @Fecha_Empaque_Venta, Fecha_Habilitada_paraProducir= @Fecha_Entrega_Produccion1, OrdendeCompra = @OrdendeCompra " +
                                  " WHERE Id_OT = @Id_OT AND Consecutivo_Pedido = @Consecutivo_Pedido ";

                    using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                    {
                        cmdUpdate.Parameters.AddWithValue("@Id_OT", IdOT);
                        cmdUpdate.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdUpdate.Parameters.AddWithValue("@Nombre_Obra", Nombre_Obra);
                        cmdUpdate.Parameters.AddWithValue("@Observacion_Pedido", Observacion_Pedido);
                        cmdUpdate.Parameters.AddWithValue("@Dirección", Dirección);
                        cmdUpdate.Parameters.AddWithValue("@Ciudad", Ciudad);
                        cmdUpdate.Parameters.AddWithValue("@Región", Región);
                        cmdUpdate.Parameters.AddWithValue("@País", País);
                        cmdUpdate.Parameters.AddWithValue("@TelDomicilio", TelDomicilio);
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Confirmacion_Venta", Convert.ToDateTime(Fecha_Confirmacion_Venta));
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Entrega_Dibujo_Despiece", Convert.ToDateTime(Fecha_Entrega_Dibujo_Despiece));
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Entrega_Produccion", Convert.ToDateTime(Fecha_Entrega_Produccion));
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Despacho_Produccion", Convert.ToDateTime(Fecha_Despacho_Produccion));
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Real_Despacho_Produccion", Convert.ToDateTime(Fecha_Real_Despacho_Produccion));
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Instalacion", Convert.ToDateTime(Fecha_Instalacion));
                        cmdUpdate.Parameters.AddWithValue("@Persona_Receptora", Persona_Receptora);
                        cmdUpdate.Parameters.AddWithValue("@OKDibujo", OKDibujo);
                        cmdUpdate.Parameters.AddWithValue("@ID_CONTACTO", ID_CONTACTO);
                        cmdUpdate.Parameters.AddWithValue("@Codigo_Asesor", Codigo_Asesor);
                        cmdUpdate.Parameters.AddWithValue("@Zona", Zona);
                        cmdUpdate.Parameters.AddWithValue("@Descuento", Descuento);
                        cmdUpdate.Parameters.AddWithValue("@Precio_Venta", Precio_Venta);
                        cmdUpdate.Parameters.AddWithValue("@Forma_Pago", Forma_Pago);
                        cmdUpdate.Parameters.AddWithValue("@cotizacion", cotizacion);
                        cmdUpdate.Parameters.AddWithValue("@Observaciones_Contables", Observaciones_Contables);
                        cmdUpdate.Parameters.AddWithValue("@Mail_Contacto", Mail_Contacto);
                        cmdUpdate.Parameters.AddWithValue("@OC_Sag", OC_Sag);
                        cmdUpdate.Parameters.AddWithValue("@OF_Sag", OF_Sag);
                        cmdUpdate.Parameters.AddWithValue("@Id_OT_Secundario", Id_OT_Secundario);
                        cmdUpdate.Parameters.AddWithValue("@Id_TipoPedido", Id_TipoPedido);
                        cmdUpdate.Parameters.AddWithValue("@ValorBolsa", ValorBolsa);
                        cmdUpdate.Parameters.AddWithValue("@RecibeElPedido", RecibeElPedido);
                        cmdUpdate.Parameters.AddWithValue("@ResumenObra", ResumenObra);
                        cmdUpdate.Parameters.AddWithValue("@Aplica_Empaque", Aplica_Empaque);
                        cmdUpdate.Parameters.AddWithValue("@Supervisor", Supervisor);
                        cmdUpdate.Parameters.AddWithValue("@Reactivada", Reactivada);
                        cmdUpdate.Parameters.AddWithValue("@FabricadoPor", FabricadoPor);
                        cmdUpdate.Parameters.AddWithValue("@Observacion_Pedido1", Observacion_Pedido1);
                        cmdUpdate.Parameters.AddWithValue("@ValorPedido", ValorPedido);
                        cmdUpdate.Parameters.AddWithValue("@ValorViatico", ValorViatico);
                        cmdUpdate.Parameters.AddWithValue("@CelularContacto", CelularContacto);
                        cmdUpdate.Parameters.AddWithValue("@DescuentoparaComision", DescuentoparaComision);
                        cmdUpdate.Parameters.AddWithValue("@ValorTteVia", ValorTteVia);
                        cmdUpdate.Parameters.AddWithValue("@PararProduccion", PararProduccion);
                        cmdUpdate.Parameters.AddWithValue("@PararDespacho", PararDespacho);
                        cmdUpdate.Parameters.AddWithValue("@Pedidobase", Pedidobase);
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Empaque", Convert.ToDateTime(Fecha_Empaque));
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Empaque_Venta", Convert.ToDateTime(Fecha_Empaque_Venta));
                        cmdUpdate.Parameters.AddWithValue("@Fecha_Entrega_Produccion1", Convert.ToDateTime(Fecha_Entrega_Produccion1));
                        cmdUpdate.Parameters.AddWithValue("@OrdendeCompra", OrdendeCompra);



                        cmdUpdate.ExecuteNonQuery();
                    }
                }
            }
        }


        // Consultar Observacion por Defecto 
        private DataTable ConsultarObservacionAuto(int idObservacion)

        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * From tblTipoObservacion where ID_TipoObservacion = @idObservacion";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@idObservacion", idObservacion);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable 
            return dataTable;
        }
        private void InsertarObservacionAutoISID(DataTable datoObservacion, DataTable datoPedido)
        {
            if (datoObservacion.Rows.Count > 0)
            {
                string Id_OT = datoPedido.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = datoPedido.Rows[0]["Consecutivo_Pedido"].ToString();
                string Nombre_Obra = datoPedido.Rows[0]["Nombre_Obra"].ToString();
                string ID_TipoObservacion = datoObservacion.Rows[0]["ID_TipoObservacion"].ToString();
                string Codigo_Asesor = datoPedido.Rows[0]["Codigo_Asesor"].ToString();
                string DestinatarioPorDefecto = datoObservacion.Rows[0]["DestinatarioPorDefecto"].ToString();


                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblOtObservacion (Id_OT,Consecutivo_Pedido,Nombre_Obra,Observacion,FechaObservacion,Emisor,Nombre_Emisor,ID_TipoObservacion," +
                                  "FechaAnteriorDespacho,FechaNuevaDespacho,ID_Programacion,CedulaAsesor, FechaActividad, Destinatarios) " +
                                  "VALUES (@Id_OT,@Consecutivo_Pedido, @Nombre_Obra, @Observacion, Getdate(), @Cedula, @NombreUsuario, @ID_TipoObservacion,Getdate(), Getdate(), 0, @Codigo_Asesor, Getdate() + 8, @DestinatarioPorDefecto  )";


                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Id_OT", Id_OT);
                        cmdInsert.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdInsert.Parameters.AddWithValue("@Nombre_Obra", Nombre_Obra);
                        cmdInsert.Parameters.AddWithValue("@Observacion", "Recoger material en prestamo (Observación generada automáticamente por el sistema)");
                        cmdInsert.Parameters.AddWithValue("@Cedula", Session["CedulaLogeada"].ToString());
                        cmdInsert.Parameters.AddWithValue("@NombreUsuario", Session["usuariologueado"].ToString());
                        cmdInsert.Parameters.AddWithValue("@ID_TipoObservacion", ID_TipoObservacion);
                        cmdInsert.Parameters.AddWithValue("@Codigo_Asesor", Codigo_Asesor);
                        cmdInsert.Parameters.AddWithValue("@DestinatarioPorDefecto", DestinatarioPorDefecto);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }


        // EXPORTAR DOCUMENTACION ISID

        public void ExportarDocumentacionOT()
        {
            DataTable documentacionOT = ConsultarDocumentacionOT_SID();
            if (documentacionOT != null && documentacionOT.Rows.Count > 0)
            {
                BorrarDocumentacionOT_ISID();
                InsertarDocumentacionOT_ISID(documentacionOT);
            }
        }
        public DataTable ConsultarDocumentacionOT_SID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();
                string sSql = "SELECT * FROM tblDocumentacion WHERE ID_OT = @Id_OT AND Pedido = @Pedido";
                using (SqlCommand cmdSelect = new SqlCommand(sSql, connectionSID))
                {
                    cmdSelect.Parameters.AddWithValue("@Id_OT", tbOT.Text);
                    cmdSelect.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        public void BorrarDocumentacionOT_ISID()
        {
            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();
                string sSql = "DELETE FROM tblDocumentacionOT WHERE ID_OT = @Id_OT AND Pedido = @Pedido";
                using (SqlCommand cmdDelete = new SqlCommand(sSql, connectionISID))
                {
                    cmdDelete.Parameters.AddWithValue("@Id_OT", tbOT.Text);
                    cmdDelete.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);
                    cmdDelete.ExecuteNonQuery();
                }
            }
        }
        public void InsertarDocumentacionOT_ISID(DataTable documentacionOT)
        {
            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();
                string sSql = "INSERT INTO tblDocumentacionOT (Id_OT, Pedido, Archivo, Observacion, Tipodocumento, usuario, FechaRegistro, Categoria, cantidad, MuebleEspecial) " +
                              "VALUES (@Id_OT, @Pedido, @Archivo, @Observacion, @TipoDocumento, @Usuario, @FechaRegistro, @Categoria, @Cantidad, @MuebleEspecial)";
                using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                {
                    foreach (DataRow row in documentacionOT.Rows)
                    {
                        cmdInsert.Parameters.Clear();
                        cmdInsert.Parameters.AddWithValue("@Id_OT", row["Id_OT"].ToString());
                        cmdInsert.Parameters.AddWithValue("@Pedido", row["Pedido"]);
                        cmdInsert.Parameters.AddWithValue("@Archivo", row["Archivo"].ToString());
                        cmdInsert.Parameters.AddWithValue("@Observacion", row["Observacion"].ToString());
                        cmdInsert.Parameters.AddWithValue("@TipoDocumento", row["TipoDocumento"].ToString());
                        cmdInsert.Parameters.AddWithValue("@Usuario", row["Usuario"].ToString());
                        cmdInsert.Parameters.AddWithValue("@FechaRegistro", Convert.ToDateTime(row["FechaRegistro"]));
                        cmdInsert.Parameters.AddWithValue("@Categoria", row["Categoria"].ToString());
                        cmdInsert.Parameters.AddWithValue("@Cantidad", row["Cantidad"]);
                        cmdInsert.Parameters.AddWithValue("@MuebleEspecial", Convert.ToBoolean(row["MuebleEspecial"]));
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        //DEPENDIENDO LA ORIENTACION DEL TIPO PEDIDO SE PROGRAMA LA OBRA POR DEFECTO A LOS PROCESOS BASES
        // ELiminar Programacion de Areas 
        public void EliminarProgramacion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            string query = "DELETE tblProgramacion FROM tblProcesoProduccion INNER JOIN tblProgramacion ON tblProcesoProduccion.Id_Area = tblProgramacion.id_Proceso " +
                           "WHERE (tblProgramacion.OT = @IdOT) AND (tblProgramacion.Pedido = @pedido) AND (tblProcesoProduccion.Base = 1)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetros
                    command.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    command.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Abrir conexión y ejecutar el comando
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }

        // Consultar Programación Proceso de producción e Insertar dependiendo de la orientacion COMERCIAL o OAI

        //COMERCIAL y OAI
        private DataTable ObtenerProgProduccion()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM  tblProcesoProduccion WHERE  Base=1 AND Activo=1";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void RealizarProgramacionPedidoISID(DataTable datoPedido)
        {
            if (datoPedido.Rows.Count > 0)
            {
                string Id_OT = datoPedido.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = datoPedido.Rows[0]["Consecutivo_Pedido"].ToString();
                string Nombre_Obra = datoPedido.Rows[0]["Nombre_Obra"].ToString();
                string Fecha_Despacho_Produccion = datoPedido.Rows[0]["Fecha_Despacho_Produccion"].ToString();
                string Ciudad = datoPedido.Rows[0]["Ciudad"].ToString();
                string TelDomicilio = datoPedido.Rows[0]["TelDomicilio"].ToString();
                string codigoasesor = datoPedido.Rows[0]["codigoasesor"].ToString();
                string mail = datoPedido.Rows[0]["mail"].ToString();
                string Activo = datoPedido.Rows[0]["Activo"].ToString();
                string Zona = datoPedido.Rows[0]["Zona"].ToString();

                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblProgramacion (OT,Pedido,Responsable,Nombre_Obra,FechaProgramacionProceso,FechaFinalProceso,Terminada," +
                                  "FechaRealFinalProceso,Descripcion_Proceso,Impresa,FechaImpresionReporte,ProcesoBase,EntregaAlFinal) " +
                                  "VALUES (@Id_OT, @Consecutivo_Pedido,800014574, @Nombre_Obra,GETDATE(), @Fecha_Despacho_Produccion, 0, GETDATE(), Por Definir, 0, GETDATE(), 1, 0)";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Id_OT", Id_OT);
                        cmdInsert.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdInsert.Parameters.AddWithValue("@Nombre_Obra", Nombre_Obra);
                        cmdInsert.Parameters.AddWithValue("@Fecha_Despacho_Produccion", Fecha_Despacho_Produccion);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }
        private void RealizarProgramacionPedidoISID2(DataTable datoPedido, DateTime FechaFinProc, DateTime FechaInicioProceso, int turno, string responsable, string Descripcion_Proceso, string Id_Area, string EntregaAlFinal, string procesar)
        {
            if (datoPedido.Rows.Count > 0)
            {
                string Id_OT = datoPedido.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = datoPedido.Rows[0]["Consecutivo_Pedido"].ToString();
                string Nombre_Obra = datoPedido.Rows[0]["Nombre_Obra"].ToString();




                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblProgramacion (OT,Pedido,Nombre_Obra,Responsable,FechaProgramacionProceso,FechaFinalProceso,Descripcion_Proceso,FechaRealFinalProceso,liquidado,turno," +
                                  "FechaOriginalFinalProceso,Procesar,Cantidad,ProcesoBase,FechaInicioProceso,id_Proceso,EntregaAlFinal) " +
                                  " VALUES (@Id_OT, @Consecutivo_Pedido, @Nombre_Obra, @responsable, GETDATE(), @FechaFinProceso, @Descripcion_Proceso,@FechaRealFinalProceso, 0, @turno," +
                                  " @FechaOriginalFinalProceso, @procesar, 1, 1, @FechaInicioProceso, @id_Proceso, @EntregaAlFinal )";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Id_OT", Id_OT);
                        cmdInsert.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdInsert.Parameters.AddWithValue("@Nombre_Obra", Nombre_Obra);
                        cmdInsert.Parameters.AddWithValue("@responsable", responsable);
                        cmdInsert.Parameters.AddWithValue("@FechaFinProceso", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@Descripcion_Proceso", Descripcion_Proceso);
                        cmdInsert.Parameters.AddWithValue("@FechaRealFinalProceso", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@turno", turno);
                        cmdInsert.Parameters.AddWithValue("@FechaOriginalFinalProceso", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@procesar", procesar);
                        cmdInsert.Parameters.AddWithValue("@FechaInicioProceso", FechaInicioProceso);
                        cmdInsert.Parameters.AddWithValue("@id_Proceso", Id_Area);
                        cmdInsert.Parameters.AddWithValue("@EntregaAlFinal", EntregaAlFinal);


                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        private DataTable ObtenerProgProduccionOAI()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblProcesoProduccion WHERE (ORIENTACION='OAI' OR ORIENTACION='TODO') AND Activo=1";
                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private DataTable ObtenerProductosXProcesoSID(DataTable datoPedido, string Descripcion_Proceso)
        {
            DataTable dataTable = new DataTable();

            string Id_OT = datoPedido.Rows[0]["Id_OT"].ToString();
            string pedido = datoPedido.Rows[0]["Consecutivo_Pedido"].ToString();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  Concat(Descripción , '-' , DescripcionPieza) AS DescripcionFull,Ancho, Altura, sum(Cant) as Cant, Und,'@Descripcion_Proceso' AS AreaProduccion,Id_Inventario " +
                              "FROM tblReporteMedidasdeCorte WHERE (Reportar = 1) AND OT= @IdOT AND PEDIDO= @pedido AND AreaProduccion LIKE '%@Descripcion_Proceso%' " +
                              "group by concat(Descripción , '-' , DescripcionPieza),Ancho,Altura,Und,Id_Inventario ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {

                    cmd.Parameters.AddWithValue("@Id_OT", Id_OT);
                    cmd.Parameters.AddWithValue("@pedido", pedido);
                    cmd.Parameters.AddWithValue("@Descripcion_Proceso", Descripcion_Proceso);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private bool ExistenciaProductoISID(DataTable datoPedido, string idArea, string producto)
        {
            string IdOT = datoPedido.Rows[0]["IdOT"].ToString();
            string pedido = datoPedido.Rows[0]["Consecutivo_Pedido"].ToString();

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT COUNT(*) FROM tblProgramacion where OT= @IdOT AND pedido= @pedido AND id_proceso = @IdArea AND Procesar= @producto ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", IdOT);
                    cmd.Parameters.AddWithValue("@pedido", pedido);
                    cmd.Parameters.AddWithValue("@IdArea", idArea);
                    cmd.Parameters.AddWithValue("@producto", producto);


                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        int rowCount = Convert.ToInt32(result);
                        // Si rowCount es mayor que cero, el pedido existe en ISID
                        return rowCount > 0;
                    }
                    else
                    {
                        // Si no se encontraron filas, retornamos false
                        return false;
                    }

                }
            }
        }
        private void RealizarProgramacionPedidoxProductoISID(DataTable datoPedido, DateTime FechaFinProc, DateTime FechaInicioProceso, int turno, string responsable, string Descripcion_Proceso, string Descripcion_Area2, string Id_Area, string EntregaAlFinal, string DescripcionFull, int cantidad)
        {
            if (datoPedido.Rows.Count > 0)
            {
                string Id_OT = datoPedido.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = datoPedido.Rows[0]["Consecutivo_Pedido"].ToString();
                string Nombre_Obra = datoPedido.Rows[0]["Nombre_Obra"].ToString();




                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblProgramacion (OT,Pedido,Nombre_Obra,Responsable,FechaProgramacionProceso,FechaFinalProceso,Descripcion_Proceso,FechaRealFinalProceso,FechaImpresionReporte," +
                                  "liquidado,ManodeObra,turno,FechaOriginalFinalProceso,Procesar,Cantidad,FechaInicioProceso,id_Proceso,EntregaAlFinal) " +
                                  " VALUES (@Id_OT, @Consecutivo_Pedido, @Nombre_Obra, @responsable, GETDATE(), @FechaFinProceso, @Descripcion_Proceso,@FechaRealFinalProceso, GETDATE(), 0," +
                                  " @Descripcion_Area2, @turno, @FechaOriginalFinalProceso, @procesar, @cantidad, @FechaFin, @id_Proceso, @EntregaAlFinal )";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Id_OT", Id_OT);
                        cmdInsert.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdInsert.Parameters.AddWithValue("@Nombre_Obra", Nombre_Obra);
                        cmdInsert.Parameters.AddWithValue("@responsable", responsable);
                        cmdInsert.Parameters.AddWithValue("@FechaFinProceso", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@Descripcion_Proceso", Descripcion_Proceso);
                        cmdInsert.Parameters.AddWithValue("@FechaRealFinalProceso", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@Descripcion_Area2", Descripcion_Area2);
                        cmdInsert.Parameters.AddWithValue("@turno", turno);
                        cmdInsert.Parameters.AddWithValue("@FechaOriginalFinalProceso", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@procesar", DescripcionFull);
                        cmdInsert.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdInsert.Parameters.AddWithValue("@FechaFin", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@id_Proceso", Id_Area);
                        cmdInsert.Parameters.AddWithValue("@EntregaAlFinal", EntregaAlFinal);


                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }
        private void AcutualizarProgramacionPedidoxProductoISID(DataTable datoPedido, DateTime FechaFinProc, int turno, string responsable, string Id_Area, string DescripcionFull, int cantidad)
        {
            if (datoPedido.Rows.Count > 0)
            {
                string Id_OT = datoPedido.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = datoPedido.Rows[0]["Consecutivo_Pedido"].ToString();

                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "UPDATE tblProgramacion SET Cantidad= @cantidad, FechaProgramacionProceso = GETDATE(), FechaFinalProceso = @FechaFinProceso, " +
                                  "Impresa=0, Terminada=0, turno= @turno, Responsable= @responsable  " +
                                  "WHERE OT= @Id_OT AND pedido= @Consecutivo_Pedido AND id_proceso= @id_Proceso, and Procesar= @procesar";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdInsert.Parameters.AddWithValue("@FechaFinProceso", FechaFinProc);
                        cmdInsert.Parameters.AddWithValue("@turno", turno);
                        cmdInsert.Parameters.AddWithValue("@responsable", responsable);
                        cmdInsert.Parameters.AddWithValue("@Id_OT", Id_OT);
                        cmdInsert.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdInsert.Parameters.AddWithValue("@id_Proceso", Id_Area);
                        cmdInsert.Parameters.AddWithValue("@procesar", DescripcionFull);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }


        // ******* INICIO OK DIBUJO CONTROLADOR  *******


        //EXPORTAR PLANO A LA BD ISID
        private DataTable ConsultarReportePlanoSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  * FROM tblReportePlano WHERE Id_OT= @IdOT AND Consecutivo_Pedido = @Pedido ";
                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private DataTable ConsultarReportePlanoISID(string plano)
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblReportePlano WHERE Plano = @plano ";
                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@plano", plano);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void EliminarReportePlanoISID(string plano)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReportePlano WHERE Plano = @plano";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@plano", plano);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private void InsertarReportePlanoISID(DataTable datoReportePlano)
        {
            if (datoReportePlano.Rows.Count > 0)
            {
                string plano = datoReportePlano.Rows[0]["Plano"].ToString();
                string Id_OT = datoReportePlano.Rows[0]["Id_OT"].ToString();
                string Consecutivo_Pedido = datoReportePlano.Rows[0]["Consecutivo_Pedido"].ToString();
                string Area = datoReportePlano.Rows[0]["Area"].ToString();
                string Id_Dibujante = datoReportePlano.Rows[0]["Id_Dibujante"].ToString();
                string Dibujante = datoReportePlano.Rows[0]["Dibujante"].ToString();



                string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblReportePlano (Plano, Id_OT, Consecutivo_Pedido, Area, Id_Dibujante, Dibujante) " +
                                  "VALUES (@plano, @Id_OT, @Consecutivo_Pedido, @Area, @Id_Dibujante, @Dibujante)";

                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@plano", plano);
                        cmdInsert.Parameters.AddWithValue("@Id_OT", Id_OT);
                        cmdInsert.Parameters.AddWithValue("@Consecutivo_Pedido", Consecutivo_Pedido);
                        cmdInsert.Parameters.AddWithValue("@Area", Area);
                        cmdInsert.Parameters.AddWithValue("@Id_Dibujante", Id_Dibujante);
                        cmdInsert.Parameters.AddWithValue("@Dibujante", Dibujante);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        // EXPORTAMOS DESPIECE AL ISID 
        private DataTable ConsultarReporteDespiceSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  * FROM tblReporteDespiece WHERE OT= @IdOT AND Pedido = @Pedido ";
                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void EliminarReporteDespieceISID()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReporteDespiece WHERE  OT= @IdOT AND Pedido = @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        public string ObtenerParaEnsamble()
        {
            string paraEnsamble = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ParaEnsamble FROM tblSede WHERE Activada = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        paraEnsamble = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción según tus necesidades
                        Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }

            return paraEnsamble;
        }
        public void InsertarDatosReporteDespieceISID(DataTable datoDespiece)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            foreach (DataRow row in datoDespiece.Rows)
            {
                string OT = row["OT"].ToString();
                Int16 pedido = Convert.ToInt16(row["Pedido"]);
                string idPlano = row["Id_Plano"].ToString();
                string idPanel = row["Id_Panel"].ToString();
                double anchoPanel = Convert.ToDouble(row["Ancho_Panel"]);
                int cantidad = Convert.ToInt32(row["Cantidad"]);
                int idNumerico = Convert.ToInt32(row["Id_Numerico"]);
                double altura = Convert.ToDouble(row["Altura"]);
                double profundidad = Convert.ToDouble(row["Profundidad"]);
                string descripcionPanel = row["Descripcion_Panel"].ToString();
                string descripcionGrupo = row["Descripcion_Grupo"].ToString();
                string descripcionLinea = row["Descripcion_Linea"].ToString();
                int precioVenta = Convert.ToInt32(row["Precio_Venta"]);
                bool cotizar = Convert.ToBoolean(row["Cotizar"]);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tblReporteDespiece (OT,Pedido,Id_Plano,Id_Panel,Ancho_Panel,Cantidad,Id_Numerico,Altura,Profundidad," +
                                   "Descripcion_Panel,Descripcion_Grupo,Descripcion_Linea,Precio_Venta,Cotizar)" +
                                   " VALUES (@OT, @Pedido, @Id_Plano, @Id_Panel, @Ancho_Panel, @Cantidad, @Id_Numerico, @Altura, @Profundidad," +
                                   " @Descripcion_Panel, @Descripcion_Grupo, @Descripcion_Linea, @Precio_Venta, @Cotizar)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OT", OT);
                        command.Parameters.AddWithValue("@Pedido", pedido);
                        command.Parameters.AddWithValue("@Id_Plano", idPlano);
                        command.Parameters.AddWithValue("@Id_Panel", idPanel);
                        command.Parameters.AddWithValue("@Ancho_Panel", anchoPanel);
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@Id_Numerico", idNumerico);
                        command.Parameters.AddWithValue("@Altura", altura);
                        command.Parameters.AddWithValue("@Profundidad", profundidad);
                        command.Parameters.AddWithValue("@Descripcion_Panel", descripcionPanel);
                        command.Parameters.AddWithValue("@Descripcion_Grupo", descripcionGrupo);
                        command.Parameters.AddWithValue("@Descripcion_Linea", descripcionLinea);
                        command.Parameters.AddWithValue("@Precio_Venta", precioVenta);
                        command.Parameters.AddWithValue("@Cotizar", cotizar);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al insertar datos: " + ex.Message);
                        }
                    }
                }
            }
        }

        // ENVIAMOS LOS ACABADOS
        private void EliminarAcabadosISID()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReporteOT_Acabado WHERE  OT= @IdOT AND Pedido = @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private DataTable ConsultarAcabadosSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblOTAcabados.*, tblGrupoObjetoParaAcabado.*, tblAcabado.* FROM tblAcabado " +
                              "INNER JOIN (tblGrupoObjetoParaAcabado " +
                              "INNER JOIN tblOTAcabados ON tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado = tblOTAcabados.ID_GrupoObjetoparaAcabado) " +
                              "ON tblAcabado.ID_Acabado = tblOTAcabados.ID_Acabado " +
                              "WHERE (((tblOTAcabados.Id_OT)= @IdOT) AND ((tblOTAcabados.Consecutivo_Pedido)= @Pedido)) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        public void InsertarAcabadosISID(DataTable datoAcabados)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            foreach (DataRow row in datoAcabados.Rows)
            {
                string Id_Acabado = row["Id_Acabado"].ToString();
                string GruposSag = row["GruposSag"].ToString();
                string idPanel = row["ColorGrupoSag"].ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO tblReporteOT_Acabado (OT,Pedido,Id_Acabado,GrupoSag,FechaActual,Cod_Color)
                                   VALUES (@OT, @Pedido, @Id_Acabado, @GruposSag, GETDATE(), @ColorGrupoSag)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OT", tbOT.ToString());
                        command.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);
                        command.Parameters.AddWithValue("@Id_Acabado", Id_Acabado);
                        command.Parameters.AddWithValue("@GruposSag", GruposSag);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al insertar datos: " + ex.Message);
                        }
                    }
                }
            }
        }


        //EXPORTAMOS MEDIDAS DE CORTE A LA BD DEL ISID
        private DataTable ConsultarReporteMedidasSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  * FROM tblReporteMedidasdeCorte WHERE OT= @IdOT AND Pedido = @Pedido Order By Orden asc ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void EliminarMedidasCorteISID()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReporteMedidasdeCorte WHERE  OT= @IdOT AND Pedido = @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        public void InsertarMedidasCorteISID(DataTable datoMedidas)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            foreach (DataRow row in datoMedidas.Rows)
            {
                string Plano = row["Plano"].ToString();
                string Item_Modulo = row["Item_Modulo"].ToString();
                string Descripción = row["Descripción"].ToString();
                string Ancho = row["Ancho"].ToString();
                string Altura = row["Altura"].ToString();
                string Cant = row["Cant"].ToString();
                string UND = row["UND"].ToString();
                string Reportar = row["Reportar"].ToString();
                string AreaProduccion = row["AreaProduccion"].ToString();
                string Reportar_Despacho = row["Reportar_Despacho"].ToString();
                string Reporte = row["Reporte"].ToString();
                string Orden = row["Orden"].ToString();
                string Id_Inventario = row["Id_Inventario"].ToString();
                string ValorUND = row["ValorUND"].ToString();
                string Factor_Desperdicio = row["Factor_Desperdicio"].ToString();
                string CantidadMaterial = row["CantidadMaterial"].ToString();
                string DescripcionPieza = row["DescripcionPieza"].ToString();
                string TipoInsumo = row["TipoInsumo"].ToString();
                string ID_FamiliaModulo = row["ID_FamiliaModulo"].ToString();
                string PesoKG = row["PesoKG"].ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO tblReporteMedidasdeCorte (OT,Pedido,Plano,Item_Modulo,Descripción,Ancho,Altura,Cant,UND,Reportar,AreaProduccion,Reportar_Despacho,
                                   Reporte,Orden,Id_Inventario,ValorUND,Factor_Desperdicio,CantidadMaterial,DescripcionPieza,TipoInsumo,ID_FamiliaModulo,PesoKg) 
                                   VALUES (@OT, @Pedido, @Plano, @Item_Modulo, Descripción, @Ancho, @Altura, @Cant, @UND, @Reportar, @AreaProduccion, @Reportar_Despacho,Reporte, 
                                   @Orden, @Id_Inventario, @ValorUND, @Factor_Desperdicio, @CantidadMaterial, @DescripcionPieza, @TipoInsumo, @ID_FamiliaModulo, @PesoKG)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OT", tbOT.Text);
                        command.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);
                        command.Parameters.AddWithValue("@Plano", Plano);
                        command.Parameters.AddWithValue("@Item_Modulo", Item_Modulo);
                        command.Parameters.AddWithValue("@Descripción", Descripción);
                        command.Parameters.AddWithValue("@Ancho", Ancho);
                        command.Parameters.AddWithValue("@Altura", Altura);
                        command.Parameters.AddWithValue("@Cant", Cant);
                        command.Parameters.AddWithValue("@UND", UND);
                        command.Parameters.AddWithValue("@Reportar", Convert.ToBoolean(Reportar));
                        command.Parameters.AddWithValue("@AreaProduccion", AreaProduccion);
                        command.Parameters.AddWithValue("@Reportar_Despacho", Convert.ToBoolean(Reportar_Despacho));
                        command.Parameters.AddWithValue("@Reporte", Reporte);
                        command.Parameters.AddWithValue("@Orden", Orden);
                        command.Parameters.AddWithValue("@Id_Inventario", Id_Inventario);
                        command.Parameters.AddWithValue("@ValorUND", ValorUND);
                        command.Parameters.AddWithValue("@Factor_Desperdicio", Factor_Desperdicio);
                        command.Parameters.AddWithValue("@CantidadMaterial", CantidadMaterial);
                        command.Parameters.AddWithValue("@DescripcionPieza", DescripcionPieza);
                        command.Parameters.AddWithValue("@TipoInsumo", TipoInsumo);
                        command.Parameters.AddWithValue("@ID_FamiliaModulo", ID_FamiliaModulo);
                        command.Parameters.AddWithValue("@PesoKG", PesoKG);



                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al insertar datos: " + ex.Message);
                        }
                    }
                }
            }
        }

        //EXPORTAE MEDIDAS FINALES 
        public string ObtenerFamiliaModuloDespachoTroja()
        {
            string FamiliaModuloDespachoTroja = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT mail FROM tblUsosVarios WHERE ObjetivoMail = 'FamiliaModuloDespachoTroja'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        // Ejecutar el comando y obtener el resultado
                        FamiliaModuloDespachoTroja = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción según tus necesidades
                        Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }

            return FamiliaModuloDespachoTroja;
        }
        private DataTable ConsultarMedidaFinal()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  * FROM tblReporteModuloMedidaFinal WHERE OT= @IdOT AND Pedido = @Pedido Order By Orden asc ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void EliminarMedidaFinalISID()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReporteModuloMedidaFinal WHERE  OT= @IdOT AND Pedido = @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        public void InsertarMedidaFinalISID(DataTable datoMedidaFinal, string FamiliaModuloDespachoTroja)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            foreach (DataRow row in datoMedidaFinal.Rows)
            {
                string OT = row["OT"].ToString();
                string Pedido = row["Pedido"].ToString();
                string Plano = row["Plano"].ToString();
                string Item_Modulo = row["Item_Modulo"].ToString();
                string Descripción = row["Descripción"].ToString();
                string Ancho = row["Ancho"].ToString();
                string Altura = row["Altura"].ToString();
                string Cant = row["Cant"].ToString();
                string Familia_Modulo = row["Familia_Modulo"].ToString();
                string UND = row["UND"].ToString();
                string ValorUnidad = row["ValorUnidad"].ToString();
                string AreaProduccion = row["AreaProduccion"].ToString();
                string SubTotal = row["SubTotal"].ToString();
                string Reporte = row["Reporte"].ToString();
                string Orden = row["Orden"].ToString();
                string ID_FamiliaModulo = row["ID_FamiliaModulo"].ToString();


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO tblReporteModuloMedidaFinal (OT,Pedido,Plano,Item_Modulo,Descripción,Ancho, 
                                   Altura,Cant,Familia_Modulo,UND , ValorUnidad, AreaProduccion, SubTotal, Reporte,Orden,ID_FamiliaModulo)
                                   VALUES (@OT, @Pedido, @Plano, @Item_Modulo, Descripción, @Ancho, @Altura, @Cant, @Familia_Modulo, @UND, @ValorUnidad, @AreaProduccion, @SubTotal,Reporte, 
                                   @Orden, @ID_FamiliaModulo)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OT", OT);
                        command.Parameters.AddWithValue("@Pedido", Pedido);
                        command.Parameters.AddWithValue("@Plano", Plano);
                        command.Parameters.AddWithValue("@Item_Modulo", Item_Modulo);
                        command.Parameters.AddWithValue("@Descripción", Descripción);
                        command.Parameters.AddWithValue("@Ancho", Ancho);
                        command.Parameters.AddWithValue("@Altura", Altura);
                        command.Parameters.AddWithValue("@Cant", Cant);
                        command.Parameters.AddWithValue("@Familia_Modulo", Familia_Modulo);
                        command.Parameters.AddWithValue("@UND", UND);
                        command.Parameters.AddWithValue("@ValorUnidad", ValorUnidad);
                        command.Parameters.AddWithValue("@AreaProduccion", AreaProduccion);
                        command.Parameters.AddWithValue("@SubTotal", SubTotal);
                        command.Parameters.AddWithValue("@Reporte", Reporte);
                        command.Parameters.AddWithValue("@Orden", Orden);
                        command.Parameters.AddWithValue("@ID_FamiliaModulo", ID_FamiliaModulo);


                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al insertar datos: " + ex.Message);
                        }
                    }

                }

                if (FamiliaModuloDespachoTroja.Contains(Familia_Modulo))
                {

                    InsertarEmpaqueXModuloISID(datoMedidaFinal);
                }

            }
        }
        public void InsertarEmpaqueXModuloISID(DataTable datoMedidas)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            foreach (DataRow row in datoMedidas.Rows)
            {
                string OT = row["OT"].ToString();
                string Pedido = row["Pedido"].ToString();
                string Plano = row["Plano"].ToString();
                string Item_Modulo = row["Item_Modulo"].ToString();
                string Descripción = row["Descripción"].ToString();
                string Ancho = row["Ancho"].ToString();
                string Altura = row["Altura"].ToString();
                string Cant = row["Cant"].ToString();
                string Familia_Modulo = row["Familia_Modulo"].ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tblEmpaquexModulo (OT,Pedido,Plano,Item_Modulo,Descripción,Ancho,Altura,Cant,Familia_Modulo) " +
                                   "VALUES (@OT, @Pedido, @Plano, @Item_Modulo, @Descripción, @Ancho, @Altura, @Familia_Modulo )";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OT", OT);
                        command.Parameters.AddWithValue("@Pedido", Pedido);
                        command.Parameters.AddWithValue("@Plano", Plano);
                        command.Parameters.AddWithValue("@Item_Modulo", Item_Modulo);
                        command.Parameters.AddWithValue("@Descripción", Descripción);
                        command.Parameters.AddWithValue("@Ancho", Ancho);
                        command.Parameters.AddWithValue("@Altura", Altura);
                        command.Parameters.AddWithValue("@Cant", Cant);
                        command.Parameters.AddWithValue("@Familia_Modulo", Familia_Modulo);


                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al insertar datos: " + ex.Message);
                        }
                    }
                }
            }
        }

        // EXPORTAR MANO DE OBRA 
        private DataTable ConsultarManoObraSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  * FROM tblReporteManodeObra WHERE OT= @IdOT AND Pedido = @Pedido Order By Orden asc ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void EliminarReporteManoObraISID()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReporteManodeObra WHERE  OT= @IdOT AND Pedido = @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        public void InsertarReporteManoObraISID(DataTable datoManoObra)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            foreach (DataRow row in datoManoObra.Rows)
            {
                string OT = row["OT"].ToString();
                string Pedido = row["Pedido"].ToString();
                string Plano = row["Plano"].ToString();
                string Descripción = row["Descripción"].ToString();
                string Ancho = row["Ancho"].ToString();
                string Altura = row["Altura"].ToString();
                string Cant = row["Cant"].ToString();
                string Familia_Modulo = row["Familia_Modulo"].ToString();
                string UND = row["UND"].ToString();
                string ValorUnidad = row["ValorUnidad"].ToString();
                string AreaProduccion = row["AreaProduccion"].ToString();
                string SubTotal = row["SubTotal"].ToString();
                string Reporte = row["Reporte"].ToString();
                string Orden = row["Orden"].ToString();


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tblReporteManodeObra (OT,Pedido,Plano,Descripción,Ancho,Altura," +
                                   "Cant,Familia_Modulo,UND,ValorUnidad,AreaProduccion,SubTotal,Reporte ,Orden) " +
                                   "VALUES (@OT, @Pedido, @Plano, @Descripción, @Ancho, @Altura," +
                                   " @Familia_Modulo, @UND, @ValorUnidad, @AreaProduccion, @SubTotal, @Reporte, @Orden )";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OT", OT);
                        command.Parameters.AddWithValue("@Pedido", Pedido);
                        command.Parameters.AddWithValue("@Plano", Plano);
                        command.Parameters.AddWithValue("@Descripción", Descripción.Substring(0, 50));
                        command.Parameters.AddWithValue("@Ancho", Ancho);
                        command.Parameters.AddWithValue("@Altura", Altura);
                        command.Parameters.AddWithValue("@Cant", Cant);
                        command.Parameters.AddWithValue("@Familia_Modulo", Familia_Modulo);
                        command.Parameters.AddWithValue("@UND", UND);
                        command.Parameters.AddWithValue("@ValorUnidad", ValorUnidad);
                        command.Parameters.AddWithValue("@AreaProduccion", AreaProduccion);
                        command.Parameters.AddWithValue("@SubTotal", SubTotal);
                        command.Parameters.AddWithValue("@Reporte", Reporte);
                        command.Parameters.AddWithValue("@Orden", Orden);
                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al insertar datos: " + ex.Message);
                        }
                    }
                }
            }
        }

        // EXPORTAR LISTADO PARA EMPAQUE 
        private DataTable ConsultarEmpaqueSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT  * FROM tblempaque WHERE Id_OT= @IdOT AND Pedido = @Pedido ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }
        private void EliminarEmpaqueISID()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblEmpaque WHERE  ID_OT= @IdOT AND Pedido = @pedido";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@IdOT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        public void InsertarEmpaqueISID(string plano)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblEmpaque (ID_OT,Pedido,Plano,Objeto,Ancho,Altura,Profundidad,Descripción_Objeto,Cantidad_Solicitada, " +
                               "Cantidad_Empacada,Paquete_Inicial,Paquete_Final,Fecha_De_Empaque,Procedencia,Descripcion_Grupo,UndXPaquete,EmpAutomatico) " +
                               "VALUES (@OT, @Pedido, @Plano,'BOLSA PARA BASURA',0,0,0,'BOLSA PARA BASURA',1,0,0,0,null,'SID','MENUDA',0,0)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OT", tbOT.Text);
                    command.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);
                    command.Parameters.AddWithValue("@Plano", plano);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al insertar datos: " + ex.Message);
                    }
                }
            }
        }
        public void InsertarEmpaque2ISID(DataTable datoEmpaque)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            foreach (DataRow row in datoEmpaque.Rows)
            {

                string Plano = row["Plano"].ToString();
                string Descripción = row["Descripción"].ToString();
                string Objeto = row["Objeto"].ToString();
                string Ancho = row["Ancho"].ToString();
                string Altura = row["Altura"].ToString();
                string Profundidad = row["Profundidad"].ToString();
                string Descripción_Objeto = row["Descripción_Objeto"].ToString();
                string Cantidad_Solicitada = row["Cantidad_Solicitada"].ToString();
                string Procedencia = row["Procedencia"].ToString();
                string Descripcion_Grupo = row["Descripcion_Grupo"].ToString();
                string UndxPaquete = row["UndxPaquete"].ToString();
                string EmpAutomatico = row["EmpAutomatico"].ToString();
                string DescripcionPieza = row["DescripcionPieza"].ToString();
                string Precio_Venta = row["Precio_Venta"].ToString();
                string PesoKG = row["PesoKG"].ToString();


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tblEmpaque (ID_OT,Pedido,Plano,Objeto,Ancho,Altura,Profundidad,Descripción_Objeto,Cantidad_Solicitada,Cantidad_Empacada," +
                                   "Paquete_Inicial,Paquete_Final,Fecha_De_Empaque,Procedencia,Descripcion_Grupo,UndXPaquete,EmpAutomatico,DescripcionPieza,Precio_Venta,PesoKg) " +
                                   "VALUES (@OT, @Pedido, @Plano, @Objeto, @Ancho, @Altura, @Profundidad, @Descripción_Objeto, @Cantidad_Solicitada, 0, 0, 0, NULL," +
                                   " @Procedencia, @Descripcion_Grupo, @UndxPaquete, @EmpAutomatico, @DescripcionPieza, @Precio_Venta, @PesoKG )";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OT", tbOT.Text);
                        command.Parameters.AddWithValue("@Pedido", ddlNumbers.SelectedItem.Text);
                        command.Parameters.AddWithValue("@Plano", Plano);
                        command.Parameters.AddWithValue("@Objeto", Objeto); ;
                        command.Parameters.AddWithValue("@Ancho", Ancho);
                        command.Parameters.AddWithValue("@Altura", Altura);
                        command.Parameters.AddWithValue("@Profundidad", Profundidad);
                        command.Parameters.AddWithValue("@Descripción_Objeto", Descripción_Objeto);
                        command.Parameters.AddWithValue("@Cantidad_Solicitada", Cantidad_Solicitada);
                        command.Parameters.AddWithValue("@Procedencia", Procedencia);
                        command.Parameters.AddWithValue("@Descripcion_Grupo", Descripcion_Grupo);
                        command.Parameters.AddWithValue("@UndxPaquete", UndxPaquete);
                        command.Parameters.AddWithValue("@EmpAutomatico", Convert.ToBoolean(EmpAutomatico));
                        command.Parameters.AddWithValue("@DescripcionPieza", DescripcionPieza);
                        command.Parameters.AddWithValue("@Precio_Venta", Precio_Venta);
                        command.Parameters.AddWithValue("@PesoKG", PesoKG);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al insertar datos: " + ex.Message);
                        }
                    }
                }
            }
        }

        //******* FIN OK DIBUJO CONTROLADOR  *******



        // SE CONSULTAN LOS ACABADOS DE VENTAS      
        private void ActualizarReporteOT_Acabado_SID(string acabado)
        {

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "UPDATE tblReporteOT SET Observacion_Pedido= @acabado WHERE Id_OT= @Id_OT AND Consecutivo_Pedido= @pedido ";

                using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                {

                    // Asignar los valores de los parámetros
                    cmdUpdate.Parameters.AddWithValue("@acabado", acabado);
                    cmdUpdate.Parameters.AddWithValue("@Id_OT", tbOT.Text);
                    cmdUpdate.Parameters.AddWithValue("@pedido", ddlNumbers.SelectedItem.Text);


                    // Ejecutar la actualización
                    cmdUpdate.ExecuteNonQuery();
                }
            }

        }


        //CREAR BOLSA A PARTIR DEL DISEÑO DE LA COTIZACION 
        private DataTable ConsultarResumePedidoSID()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblGrupoObjeto.ID_GrupoObjeto," +
                    "tblGrupoObjeto.Descripcion_Grupo,Sum(tblPlano_Panel.Cantidad) AS Cantidad," +
                    "Sum(tblPlano_Panel.Cantidad*tblPlano_Panel.Precio_Venta) AS SubTotal," +
                    "tblGrupoObjeto.GOBloqueaPedido " +
                    "FROM (tblPlano INNER JOIN ((tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto) " +
                    "INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum) ON tblPlano.Plano = tblPlano_Panel.Id_Plano) " +
                    "INNER JOIN tblPlanoDiseño ON tblPlano.Plano = tblPlanoDiseño.Plano " +
                    "GROUP BY tblGrupoObjeto.ID_GrupoObjeto, tblGrupoObjeto.Descripcion_Grupo," +
                    "tblGrupoObjeto.GOBloqueaPedido, tblPlanoDiseño.Numero_Diseño, tblGrupoObjeto.Cotizar " +
                    "HAVING (((tblPlanoDiseño.Numero_Diseño)=@diseño) AND ((tblGrupoObjeto.Cotizar)=1))";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@diseño", txtDiseño.Text);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable que puede contener cero o más filas de resultados
            return dataTable;
        }
        private void ActualizarOtBolsaSID()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "update tblOTBolsa set otbolCantidadCotizada=0, otbolValorCotizado=0 where OTBolBolsa= @bolsa ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@bolsa", "BSA" + tbOT.Text + "-" + ddlNumbers.SelectedItem.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        private DataTable ConsultarBolsaOT_SID(string id_GrupoObjeto)
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblOTBolsa WHERE OTBolBolsa= @bolsa AND otbolIDGrupoObjeto= @id_GrupoObjeto ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@bolsa", "BSA" + tbOT.Text + "-" + ddlNumbers.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@id_GrupoObjeto", id_GrupoObjeto);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable que puede contener cero o más filas de resultados
            return dataTable;
        }
        private void CrearBolsaObjeto(string id_GrupoObjeto, string Descripcion_Grupo, string Cantidad, string SubTotal, string GOBloqueaPedido)
        {

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "INSERT INTO tblOtBolsa (otbolBolsa,otbolId_OT,otbolPedido,otbolIDGrupoObjeto," +
                              "otbolGrupoObjeto,otbolCantidadCotizada,otbolValorCotizado,otbolCantidadPedida,otbolBloqueaPedido) " +
                              "VALUES (@bolsa, @OT, @pedido, @id_GrupoObjeto, @Descripcion_Grupo, @Cantidad, @SubTotal, 0, @GOBloqueaPedido) ";


                using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                {
                    cmdInsert.Parameters.AddWithValue("@bolsa", "BSA" + tbOT.Text + "-" + ddlNumbers.SelectedItem.Text);
                    cmdInsert.Parameters.AddWithValue("@OT", tbOT.Text);
                    cmdInsert.Parameters.AddWithValue("@pedido", pedido);

                    cmdInsert.Parameters.AddWithValue("@id_GrupoObjeto", id_GrupoObjeto);
                    cmdInsert.Parameters.AddWithValue("@Descripcion_Grupo", Descripcion_Grupo);
                    cmdInsert.Parameters.AddWithValue("@Cantidad", Cantidad);
                    cmdInsert.Parameters.AddWithValue("@SubTotal", SubTotal);
                    cmdInsert.Parameters.AddWithValue("@GOBloqueaPedido", Convert.ToBoolean(GOBloqueaPedido));



                    cmdInsert.ExecuteNonQuery();
                }
            }

        }
        private void ActualizarBolsaObjeto(string id_GrupoObjeto, string Cantidad, string SubTotal)
        {

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "UPDATE tblOTBolsa SET otbolCantidadCotizada= @Cantidad, otbolValorCotizado= @SubTotal " +
                    "WHERE OTBolBolsa= @bolsa and otbolIDGrupoObjeto= @id_GrupoObjeto ";


                using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                {

                    cmdUpdate.Parameters.AddWithValue("@Cantidad", Cantidad);
                    cmdUpdate.Parameters.AddWithValue("@SubTotal", SubTotal);
                    cmdUpdate.Parameters.AddWithValue("@bolsa", "BSA" + tbOT.Text + "-" + ddlNumbers.SelectedItem.Text);
                    cmdUpdate.Parameters.AddWithValue("@id_GrupoObjeto", id_GrupoObjeto);

                    cmdUpdate.ExecuteNonQuery();
                }
            }

        }

        // FIN  LOGICA DEL BOTON OK

        //DataGrid Informacion contable 
        protected void DataGrid_RowDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Accede a los datos de la fila actual
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string pedidoB = drv["PedidoBase"].ToString();
                string consecutivoPedido = drv["Consecutivo_Pedido"].ToString();
                bool afectaBolsa = Convert.ToBoolean(drv["AfectaBolsa"]);

                if (afectaBolsa)
                {
                    // Se consulta la suma del ValorPedido X pedido Base
                    double sumaValorPedido = ConsultarValorPedido(pedidoB);

                    // Verificamos si el pedidoBase igual al consecutivo pedido y asigamos los valores  
                    if (pedidoB == consecutivoPedido)
                    {
                        e.Item.Cells[6].Text = sumaValorPedido.ToString();
                        e.Item.Cells[7].Text = (Convert.ToInt32(e.Item.Cells[3].Text) - Convert.ToInt32(e.Item.Cells[6].Text)).ToString();

                    }
                }
            }
        }
        private double ConsultarValorPedido(string pedidoBase)
        {
            double valorPedido = 0;

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT SUM (tblOT.ValorPedido) FROM tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido " +
                              "WHERE (((tblOT.Id_OT)= @OT)) and PedidoBase = @pedidoBase and AfectaBolsa = 1";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@OT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedidoBase", pedidoBase);


                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado no es nulo y convertirlo a double
                    if (result != null && result != DBNull.Value)
                    {
                        valorPedido = Convert.ToDouble(result);
                    }
                }

                return valorPedido;
            }
        }

    }

}





