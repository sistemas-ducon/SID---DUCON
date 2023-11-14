using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.Word;
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
using static SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes;
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
            if (!IsPostBack)
            {  
                
                Session["CargarOTsEjecutada"] = null;

                habilitarbotones();          

                DeshabilitarBotones(sender, e);

                listaTextBoxes = new List<TextBox>
                {
                    tbPedDepen,tbObra,tbDir,tbContac,tbEmail,tbRecibe,tbTel,tbCel,tbPais,tbHTotal,tbVenta,dtpFechaEntregaDibujoDespiece,dtpFechaEntregaProduccion,dtpEmpaque,dtpRealEmpaque,tbSupervisor,
                    tbBolsa,tbValorPedido,txtNit,txtNombreEmp,txtcontacto,txtMail,txtDireccion,txtMunicipio,txtTelefono,txtCotizacion,txtValorSugerido,txtVcsd,txtVccd,txtOrdenCompra,txtAsesor,txtComision,
                    txtDiseño,txtSaldo,txtVenta,txtDcto,txtDctoValor,txtVtte,txtVvia,txtGtotal

                };

                listaDropDownLists = new List<DropDownList>
                {
                   ddlZona,dtacboTipoPedido,cboPedidoBase,DtaCboTipoAprobacion,ddlFabrica1,ddlInstala,ddlAsesor,ddlCiudad

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


            }
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

                Cargar_OTs();

                Session["CargarOTsEjecutada"] = true;
            }

           
          

          
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
                "LabelOTCerrada", "LiteralFechaCierre", "ddlZona",
                "dtacboTipoPedido", "tbPedDepen", "DtaCboTipoAprobacion", "tbObra", "tbDir",
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


            }

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
        }

        protected void ddlCiudad_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlCiudad.Items.Insert(0, new ListItem("Seleccione", ""));
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

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem("Seleccione", "0"));
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
          
            Cargar_Plano( id, pedido);
            Cargar_Despiece_Plano();

          

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

            tbOT.Text = leer["Id_OT"].ToString();                     // Numero de la OT
            ddlNumbers.Text = leer["Consecutivo_Pedido"].ToString();  // Numero Pedido
            ddlZona.Text = leer["Zona"].ToString();                   // Zona 
            dtacboTipoPedido.Text = leer["Descripcion_TipoPedido"].ToString(); // T.Ped
            tbPedDepen.Text = leer["Consecutivo_Pedido"].ToString();  // Ped.Deped
            DtaCboTipoAprobacion.Text = leer["TipoAprobacion"].ToString(); // Aprob
            tbObra.Text = leer["Nombre_Obra"].ToString();              // Nombre obra
            tbDir.Text = leer["Dirección"].ToString();                 // Direccion
            tbContac.Text = leer["Persona_Receptora"].ToString();       // Contacto
            tbEmail.Text = leer["mail_Contacto"].ToString();            // Mail
            tbRecibe.Text = leer["RecibeElPedido"].ToString();          // Recibe
            string Ciudad = leer["Ciudad"].ToString() + " - " + leer["Región"].ToString(); // Ciudad
            foreach (ListItem item in ddlCiudad.Items)
            {
                if (item.Text == Ciudad)
                {
                    ddlCiudad.ClearSelection();
                    item.Selected = true;
                    break;
                }
            }
            tbTel.Text = leer["TelDomicilio"].ToString();                // Telefono
            tbCel.Text = leer["CelularContacto"].ToString();             // Celular
            tbPais.Text = leer["País"].ToString();                      // Pais
            txObs1.Value = leer["Observacion_Pedido"].ToString();       // Observacion Pedido
            txObs2.Value = leer["Observacion_Dibujo"].ToString();       // Observacion Dibujo
            DateTime Dato = (DateTime)leer["Fecha_Confirmacion_Venta"];
            DateTime Dato2 = (DateTime)leer["Fecha_Entrega_Produccion"];
            DateTime Dato3 = (DateTime)leer["Fecha_Empaque"];
            DateTime Dato4 = (DateTime)leer["Fecha_Real_Empaque"];
            tbVenta.Text = Dato.ToString("yyyy-MM-dd");               // FechaVenta
            dtpFechaEntregaDibujoDespiece.Text = Dato.ToString("yyyy-MM-dd"); // FechaOkVenta
            dtpFechaEntregaProduccion.Text = Dato2.ToString("yyyy-MM-dd");   // FechaOkDibujo
            dtpEmpaque.Text = Dato3.ToString("yyyy-MM-dd");               // FechaEmpaque
            dtpRealEmpaque.Text = Dato4.ToString("yyyy-MM-dd");           // FechaRealEmpaque
            tbSupervisor.Text = leer["Supervisor"].ToString();           // Supervisor
            ddlFabrica1.SelectedItem.Text = leer["FabricadoPor"].ToString();
            ddlInstala.SelectedItem.Text = leer["InstaladaPor"].ToString();

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

            btnCotizacion.Enabled = true;

            calcularDescuento();
            calcularGranTotal();
           
        }

        private void AssignCotizacionData(string id, string pedido, string cotizacion, SqlConnection connection)
        {
            using (SqlCommand cotzita = new SqlCommand("Sp_DatosCotizacionyPlano", connection))
            {
                cotzita.CommandType = CommandType.StoredProcedure;
                cotzita.Parameters.AddWithValue("@Id_OT", id);
                cotzita.Parameters.AddWithValue("@Consecutivo_Pedido", pedido);
                cotzita.Parameters.AddWithValue("@Cotizacion", cotizacion);

                cotzita.Connection.Open();
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
                }
                cotzita.Connection.Close();
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

        public void Cargar_Plano( string id,  string pedido)
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
                        Id_Panel = r["Id_Panel"].ToString()


                        
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

                if (datos.Tipo == "Total" && datos.Titulo == "<b>Totales</b>")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#7aeaff"); // Cambia esto al color que desees
                }
                else if (!string.IsNullOrEmpty(datos.ID)) // Asegúrate de ajustar esta verificación según la columna que contenga el ID
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#47ca4b");
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
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
                SqlCommand command = new SqlCommand("Sp_ObtenerDatosModulo", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IdPanelNum", SqlDbType.VarChar, 30).Value = idPanelNum;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                DataGridModuloObjetos.DataSource = dataTable;
                DataGridModuloObjetos.DataBind();

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



    }
}