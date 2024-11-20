using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MathNet.Numerics;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using SISTEMA_INTEGRAL_DUCON.Formularios.Ventas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes;
using Button = System.Web.UI.WebControls.Button;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Control = System.Web.UI.Control;
using DataTable = System.Data.DataTable;
using Label = System.Web.UI.WebControls.Label;
using ListItem = System.Web.UI.WebControls.ListItem;
using TextBox = System.Web.UI.WebControls.TextBox;
using System.Drawing;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Solicitud_Especial : System.Web.UI.Page
    {

        private bool isModalVisible = false;
        private bool terminadoVentas;
        public bool VariableSolicitudSesion;

        private string CadenaConexionSID = "BD_SIDSQL";
        private string CadenaConexionISID = "BD_ISIDSQL";


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["usuariologueado"] != null)
                {
                    // Para todos los usuarios 
                    DepartamentoAsesor();
                    NombreAsesorLogeado();
                    CargarAsesoresEnDropDownList();
                    CargarClienteYContacto();
                    ZonaAsesorLog();
                    CargarSession();
                    CargarVariablesDeSesion();

                    // Para Ventas y Asesor 

                    if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                    {
                        BusCotDiv.Visible = false;
                        BusDesDiv.Visible = false;

                        Session["ProVenSolicitud"] = false;
                        if (ConsultarTerminadoVentas())
                        {
                            // Programar es para varios departamento
                            Button btnProgramarRender = FindControl("btnProgramarSolicitud") as Button;
                            if (btnProgramarRender != null)
                            {
                                btnProgramarRender.Enabled = true;
                                btnProgramarRender.CssClass = "btn btn-sm  btn-warning";

                            }

                            string script = @"ActiBotDetalleVentas();";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ActiBotDetalleVentas", script, true);

                        }
                        else
                        {
                            // Programar es para varios departamento
                            Button btnProgramarRender = FindControl("btnProgramarSolicitud") as Button;
                            if (btnProgramarRender != null)
                            {
                                btnProgramarRender.Enabled = false;
                                btnProgramarRender.CssClass = "btn btn-sm btn-secondary";

                            }
                        }

                        //Disposicion Botones para Ventas 
                        ControlBotonesVentas();


                        //Se oculta Compas para ventas 
                        DerCompras.Visible = false;


                        if (Session["CargarSolicitud"]?.ToString() == "1")
                        {
                            CargarSolictudEspecial(Session["CargarSolicitud_ID"].ToString());

                            Session.Remove("CargarSolicitud_ID");
                            Session.Remove("CargarSolicitud");
                        }


                    }
                    else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                    {
                        // Se le cambia el texto al boton programar 
                        btnProgramarSolicitud.Text = "Terminar";
                        DateTime fechahoy = DateTime.Now;

                        tbFechaPactoentrega.Text = fechahoy.ToString("yyyy-MM-dd");

                        // Se crea la variable de control del TerminadoDiseño
                        Session["ProDiSolicitud"] = false;
                        if (ConsultarTerminadoDiseño())
                        {
                            // Programar es para varios departamento
                            Button btnProgramarRender = FindControl("btnProgramarSolicitud") as Button;
                            if (btnProgramarRender != null)
                            {
                                btnProgramarRender.Enabled = true;
                                btnProgramarRender.CssClass = "btn btn-sm btn-warning";

                            }

                        }
                        else
                        {
                            // Programar es para varios departamento
                            Button btnProgramarRender = FindControl("btnProgramarSolicitud") as Button;
                            if (btnProgramarRender != null)
                            {
                                btnProgramarRender.Enabled = false;
                                btnProgramarRender.CssClass = "btn  btn-sm btn-secondary";

                            }
                        }

                        //Disposicion Botones para Dibujo                        
                        ControlBotonesDiseño();

                        // Se Cargan los DataGrid con los Datos para Dibujo 
                        CargarDesarrollos_Cotizaciones();


                        if (Session["CargarSolicitud"]?.ToString() == "1")
                        {
                            CargarSolictudEspecial(Session["CargarSolicitud_ID"].ToString());

                            Session.Remove("CargarSolicitud_ID");
                            Session.Remove("CargarSolicitud");
                        }

                    }

                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }


            }


        }

        private void ControlBotonesVentas()
        {
            //Solo para el departamento de Desarrollo
            Button btnComplejo = FindControl("ConfirmarComplejo") as Button;
            if (btnComplejo != null)
            {
                btnComplejo.Enabled = false;
                btnComplejo.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Solo para el departamento de Desarrollo
            Button btnTrabajarSolicitud = FindControl("btnTrabajarSolicitud") as Button;
            if (btnTrabajarSolicitud != null)
            {
                btnTrabajarSolicitud.Enabled = false;
                btnTrabajarSolicitud.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Solo para el departamento de Desarrollo
            Button btnDesprogramar = FindControl("btnDesprogramar") as Button;
            if (btnDesprogramar != null)
            {
                btnDesprogramar.Enabled = false;
                btnDesprogramar.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Solo para el departamento de Desarrollo
            Button btnTrbajarCotizacion = FindControl("btnTrbajarCotizacion") as Button;
            if (btnTrbajarCotizacion != null)
            {
                btnTrbajarCotizacion.Enabled = false;
                btnTrbajarCotizacion.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Solo para el departamento de Desarrollo
            Button btnDesprogramar1 = FindControl("btnDesprogramar1") as Button;
            if (btnDesprogramar1 != null)
            {
                btnDesprogramar1.Enabled = false;
                btnDesprogramar1.CssClass = "btn btn-sm btn-outline-secondary";

            }

            // Para Dibujo y Despiece 
            //Solo para el departamento de Desarrollo
            Button btnConUrgente = FindControl("btnConUrgente") as Button;
            if (btnConUrgente != null)
            {
                btnConUrgente.Enabled = false;
                btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

            }
        }

        private void ControlBotonesDiseño()
        {
            //Deshablitamos el Boton confirmar Complejo
            Button btnComplejo = FindControl("ConfirmarComplejo") as Button;
            if (btnComplejo != null)
            {
                btnComplejo.Enabled = false;
                btnComplejo.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Activar el Boton Trabajar en Solicitud 
            Button btnTrabajarSolicitud = FindControl("btnTrabajarSolicitud") as Button;
            if (btnTrabajarSolicitud != null)
            {
                btnTrabajarSolicitud.Enabled = false;
                btnTrabajarSolicitud.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Activar el Boton Trabajar en Desprogramar  
            Button btnDesprogramar = FindControl("btnDesprogramar") as Button;
            if (btnDesprogramar != null)
            {
                btnDesprogramar.Enabled = false;
                btnDesprogramar.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Activar el Boton Trabajar en TrabajarCotizacion 
            Button btnTrbajarCotizacion = FindControl("btnTrbajarCotizacion") as Button;
            if (btnTrbajarCotizacion != null)
            {
                btnTrbajarCotizacion.Enabled = false;
                btnTrbajarCotizacion.CssClass = "btn btn-sm btn-outline-secondary";

            }

            //Activar el Boton Trabajar en Desprogramar1 
            Button btnDesprogramar1 = FindControl("btnDesprogramar1") as Button;
            if (btnDesprogramar1 != null)
            {
                btnDesprogramar1.Enabled = false;
                btnDesprogramar1.CssClass = "btn btn-sm btn-outline-secondary";

            }

            // Para Dibujo y Despiece 
            //Solo para el departamento de Desarrollo
            Button btnConUrgente = FindControl("btnConUrgente") as Button;
            if (btnConUrgente != null)
            {
                btnConUrgente.Enabled = false;
                btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

            }
        }

        public void CargarVariablesDeSesion()
        {
            Dictionary<string, Control> variablesDeSesionYControles = new Dictionary<string, Control>
            {
                //Variables de Session de la solicitud
                { "FecIngrSolSession", tbFechaIngresoServidor },
                { "FecEntregaSolSession", tbFechaEntregaServidor },
                { "FechaRespuestaSession", tbFechaRespuestaServidor },
                { "DirigidoSession", ddlDirigido },
                { "TipoSession", ddlTipo },
                { "SolOrigenSession", tbSolicitudOrigen },
                { "ProyectoSolSession", tbProyecto },
                { "CiudadSession", ddlCiudad },
                { "ViaticoSession", chxViaticos },
                { "CotizacionSession", tbCotizacionEsp },
                { "ClienteSolSession", tbClienteServidor },
                { "ContactoSolSession", tbContactoServidor },
                { "TelSeolSession", tbTelefonoServidor },
                { "CelularSolSession", tbCelularServidor },
                { "Mailsolsession", tbMailServidor },
                { "DirecccionSolSession", tbDireccionServidor },
                { "AsesorSolSession", ddlAsesor},
                { "numeroSolicitudSession", lbNumeroSolicitud},
                //Variables de Session de Detalle
                { "ProductoSession", txDescProduc},
                { "ProveedorVentaSession", tbProveedor},
                { "AnchoSession", tbAncho},
                { "AlturaSession", tbAltura},
                { "ProfundidadSession", tbProfundidad},
                { "MaterialSession", tbMaterial},
                { "CantidadSession", tbCantidad},
                { "EspGeneralSession", txEspGen}

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
                    if (kvp.Key == "FecIngrSolSession")
                    {

                        tbFechaIngreso.Text = valorSesion;
                    }
                    if (kvp.Key == "FecEntregaSolSession")
                    {

                        tbFechaEntrega.Text = valorSesion;
                    }
                    if (kvp.Key == "FechaRespuestaSession")
                    {

                        tbFechaRespuesta.Text = valorSesion;
                    }
                    if (kvp.Key == "ClienteSolSession")
                    {

                        tbCliente.Text = valorSesion;
                    }
                    if (kvp.Key == "ContactoSolSession")
                    {

                        tbContacto.Text = valorSesion;
                    }
                    if (kvp.Key == "TelSeolSession")
                    {

                        tbTelefono.Text = valorSesion;
                    }
                    if (kvp.Key == "CelularSolSession")
                    {

                        tbCelular.Text = valorSesion;
                    }
                    if (kvp.Key == "Mailsolsession")
                    {

                        tbMail.Text = valorSesion;
                    }
                    if (kvp.Key == "DirecccionSolSession")
                    {

                        tbDireccion.Text = valorSesion;
                    }
                    else if (kvp.Value is DropDownList)
                    {

                        if (kvp.Key == "TipoSession" || kvp.Key == "SolOrigenSession" || kvp.Key == "DirigidoSession" || kvp.Key == "AsesorSolSession")
                        {
                            ddlDirigido.DataBind();
                            ddlTipo.DataBind();
                            ((DropDownList)kvp.Value).SelectedItem.Text = valorSesion;
                            ((DropDownList)kvp.Value).SelectedItem.Value = valorSesion;
                        }

                        if (kvp.Key == "CiudadSession")
                        {
                            ddlCiudad.DataBind();
                            foreach (ListItem item in ddlCiudad.Items)
                            {
                                if (item.Text == valorSesion)
                                {
                                    ddlCiudad.ClearSelection();
                                    item.Selected = true;
                                    break;
                                }
                            }


                        }



                    }
                    else if (kvp.Value is CheckBox)
                    {
                        ((CheckBox)kvp.Value).Checked = Convert.ToBoolean(valorSesion);
                    }
                    else if (kvp.Value is Label)
                    {
                        ((Label)kvp.Value).Text = valorSesion;
                    }

                    string Produc = Session["ProductoSession"] as string;
                    if (!string.IsNullOrEmpty(Produc))
                    {
                        txDescProduc.InnerText = Produc;
                        Session.Remove("ProductoSession");
                    }
                    string EspGeneral = Session["EspGeneralSession"] as string;
                    if (!string.IsNullOrEmpty(EspGeneral))
                    {
                        txEspGen.InnerText = EspGeneral;
                        Session.Remove("EspGeneralSession");
                    }



                    Session.Remove(kvp.Key);
                }
            }

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
                        string query = "SELECT X.NombreCompañía, X.Teléfono, Y.NombreContacto, Y.MailContacto, Y.Celular, X.Dirección " +
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
                                        tbCliente.Text = reader["NombreCompañía"].ToString();
                                        tbClienteServidor.Text = reader["NombreCompañía"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Teléfono")))
                                    {
                                        tbTelefono.Text = reader["Teléfono"].ToString();
                                        tbTelefonoServidor.Text = reader["Teléfono"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreContacto")))
                                    {
                                        tbContacto.Text = reader["NombreContacto"].ToString();
                                        tbContactoServidor.Text = reader["NombreContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("MailContacto")))
                                    {
                                        tbMail.Text = reader["MailContacto"].ToString();
                                        tbMailServidor.Text = reader["MailContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Celular")))
                                    {
                                        tbCelular.Text = reader["Celular"].ToString();
                                        tbCelularServidor.Text = reader["Celular"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Dirección")))
                                    {
                                        tbDireccion.Text = reader["Dirección"].ToString();
                                        tbDireccionServidor.Text = reader["Dirección"].ToString();
                                    }


                                    Session.Remove("ID_ContactoBD");
                                    Session.Remove("Id_ClienteBD");



                                }
                            }
                        }
                    }



                }


            }



        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial WHERE activo =1 order by Nombre";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto"; // Campos que se mostrará en el DropDownLi
                ddlAsesor.DataValueField = "Cedula";

                ddlAsesor.DataBind();

                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    ddlAsesor.SelectedValue = Session["CedulaLogeada"].ToString();
                }

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        protected void ddlZona_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlZona.Items.Insert(0, new ListItem("Todas", ""));

        }


        protected void ddlCiudad_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlCiudad.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        public void NombreAsesorLogeado()
        {

            string consultaActual = "Select Nombre +' '+ Apellidos, Zona from tblEmpleado where  Cedula = @Cedula";

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
                        string NombreAsesor = (string)command.ExecuteScalar();
                        Session["NombreAsesor"] = NombreAsesor;
                        tbNombreAsesor.Text = NombreAsesor;
                    }


                }
            }

        }


        public void ZonaAsesorLog()
        {

            string consultaActual = "Select  Zona from tblEmpleado where  Cedula = @Cedula";

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
                        string Zona = (string)command.ExecuteScalar();
                        Session["ZonaAsesor"] = Zona;

                    }


                }
            }

        }  // Este  Campo se podria cargar en el Login

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

        } // Este  Campo se podria cargar en el Login

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


            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {
                string script = @"ControlHeaderCard();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlHeaderCard", script, true);
            }
            else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
            {
                string script = @"ControlBtnCliente();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);
            }



        }


        protected void DataGridDesarrollo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {


                // Obtener los valores de las columnas ocultas
                int programadoVentas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas"));
                object pausadoObj = DataBinder.Eval(e.Item.DataItem, "Pausado");
                int pausado = pausadoObj != DBNull.Value ? (bool)pausadoObj ? 1 : 0 : 0;
                int terminado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Terminado"));
                int Urgente = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Urgente"));
                int DesComplejo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DesarrolloComplejo"));
                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));



                if (DesComplejo == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#77a765");    //Verde 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#e9a270");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }

                else if (programadoVentas == 1 && pausado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#70ede4");    // Aqua

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


        public void CambioZona(object sender, EventArgs e)
        {
            string valorSeleccionado = ddlZona.SelectedValue;

            CambiarSqlDataSource(valorSeleccionado);
            CambiarSqlDataSource2(valorSeleccionado);
            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {
                string script = @"ControlHeaderCard();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlHeaderCard", script, true);
            }
            else
            {
                ControlBotonesDiseño();
                string script = @"ControlBtnCliente();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlHeaderCard", script, true);
            }

        }

        private void CambiarSqlDataSource(string valorSeleccionado)
        {
            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {
                if (valorSeleccionado == "01" || valorSeleccionado == "02")
                {
                    // Actualizar el DataGrid para que use este SqlDataSource
                    DataGrid1.DataSourceID = "Desarrollo";
                    DataGrid1.DataBind();
                }
                else
                {
                    DataGrid1.DataSourceID = "CargarDesarrollos";
                }
            }
            else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
            {
                if (valorSeleccionado == "01" || valorSeleccionado == "02")
                {
                    Desarrollo.SelectCommand = "spObteSoliEspeDiseño";

                    // Limpiar los parámetros existentes si es necesario
                    Desarrollo.SelectParameters.Clear();

                    // Agregar el parámetro Zona
                    Desarrollo.SelectParameters.Add("Zona", ddlZona.SelectedValue);

                    // Actualizar el DataGrid para que use este SqlDataSource
                    DataGrid1.DataSourceID = "Desarrollo";
                    DataGrid1.DataBind();
                }
                else
                {
                    // Cargar Desarrollos

                    CargarDesarrollos.SelectCommand = " SELECT * FROM tblSoliciDiseEspe  " +
                                                                   " WHERE Terminado = 0 AND Dirigidoa='DESARROLLO DE PRODUCTO'AND  ProgramadoVentas = 1  " +
                                                                   " AND TipoSolicitud ='DESARROLLO' ORDER BY Fecha_Ingreso ASC ";

                    DataGrid1.DataSourceID = "CargarDesarrollos";
                    DataGrid1.DataBind();
                }

            }



            string script = "<script>ControlBtnCliente();</script>";
            ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, false);
            DataGrid1.DataBind();

        }


        // Este metodo es por si desean cambiar el la zona por separado
        public void CambioZona2(object sender, EventArgs e)
        {
            string valorSeleccionado = ddlZona.SelectedValue;

            CambiarSqlDataSource2(valorSeleccionado);

        }

        private void CambiarSqlDataSource2(string valorSeleccionado)
        {

            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {
                if (valorSeleccionado == "01" || valorSeleccionado == "02")
                {
                    // Actualizar el DataGrid para que use este SqlDataSource
                    DataGrid2.DataSourceID = "Cotizaciones";
                    DataGrid2.DataBind();
                }
                else
                {
                    DataGrid2.DataSourceID = "CargarCotizaciones";
                }
            }
            else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
            {
                if (valorSeleccionado == "01" || valorSeleccionado == "02")
                {
                    Cotizaciones.SelectCommand = "spObteSoliEspeCotDiseño";

                    // Limpiar los parámetros existentes si es necesario
                    Cotizaciones.SelectParameters.Clear();

                    // Agregar el parámetro Zona
                    Cotizaciones.SelectParameters.Add("Zona", ddlZona.SelectedValue);

                    // Actualizar el DataGrid para que use este SqlDataSource
                    DataGrid2.DataSourceID = "Cotizaciones";
                    DataGrid2.DataBind();
                }
                else
                {
                    // Cargar Cotizaciones 

                    CargarCotizaciones.SelectCommand = "SELECT * FROM tblSoliciDiseEspe " +
                                                       "WHERE Terminado = 0 AND TipoSolicitud ='COTIZACIÓN' AND Dirigidoa = 'DESARROLLO DE PRODUCTO' " +
                                                       "AND ProgramadoVentas = 1 ORDER BY Fecha_Ingreso ASC;";



                    DataGrid2.DataSourceID = "CargarCotizaciones";
                    DataGrid2.DataBind();
                }

            }


            string script = "<script>ControlBtnCliente();</script>";
            ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, false);
            DataGrid2.DataBind();

        }

        protected void DataGridCotizacion_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                // Aplica la clase "fila-clickeable" a todas las filas
                e.Item.CssClass += " fila-clickeable";


                // Obtener los valores de las columnas ocultas
                int programadoVentas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas"));
                object pausadoObj = DataBinder.Eval(e.Item.DataItem, "Pausado");
                int pausado = pausadoObj != DBNull.Value ? (bool)pausadoObj ? 1 : 0 : 0;
                int terminado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Terminado"));
                int Urgente = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Urgente"));
                int DesComplejo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DesarrolloComplejo"));
                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));



                if (DesComplejo == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#77a765");    //Verde 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#e9a270");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }

                else if (programadoVentas == 1 && pausado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#70ede4");    // Aqua

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


        protected void ConsultarSolicitud(object sender, EventArgs e)
        {

            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {
                // Validamos si el campo esta vacio para ejecurar un sqldatasource sino usamoos el otr 
                if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbSolicitud1.Text != "")
                {
                    BuscarDesarrollo.DataSourceID = "SolicitudXID";
                    BuscarDesarrollo.DataBind();
                }
                else if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbProyectoX.Text != "")
                {
                    BuscarDesarrollo.DataSourceID = "SolicXProyecto";
                    BuscarDesarrollo.DataBind();

                }
                else if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbClienteX.Text != "")
                {
                    BuscarDesarrollo.DataSourceID = "solicitudXCliente";
                    BuscarDesarrollo.DataBind();
                }
                else
                {
                    BuscarDesarrollo.DataSourceID = "SolicXFecha";
                    BuscarDesarrollo.DataBind();
                }

                // Control de la activacion del boton de cliente 
                string script = "<script>ControlBuscarSolicitud();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBuscarSolicitud", script, false);

            }
            else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" /* || ControlDeDiseño() */)
            {
                // Validamos si el campo esta vacio para ejecurar un sqldatasource sino usamoos el otr 
                if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbSolicitud1.Text != "")
                {
                    SolicitudXID.SelectCommand = "SELECT *  FROM tblSoliciDiseEspe " +
                                             " WHERE  ID_Solicitud  LIKE '%' + @Solicitud + '%' " +
                                             "AND Fecha_Ingreso between @FechaIni and @FechaFin  ";


                    BuscarDesarrollo.DataSourceID = "SolicitudXID";
                    BuscarDesarrollo.DataBind();
                }
                else if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbProyectoX.Text != "")
                {

                    SolicXProyecto.SelectCommand = "SELECT * FROM tblSoliciDiseEspe  WHERE " +
                                                   "Proyecto  LIKE '%' + @Proyecto + '%' " +
                                                   "AND Fecha_Ingreso between @FechaIni and @FechaFin  ";


                    BuscarDesarrollo.DataSourceID = "SolicXProyecto";
                    BuscarDesarrollo.DataBind();

                }
                else if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbClienteX.Text != "")
                {

                    solicitudXCliente.SelectCommand = " SELECT *  FROM tblSoliciDiseEspe " +
                                                      "  WHERE  Cliente  LIKE '%' + @Cliente + '%' " +
                                                      "  AND Fecha_Ingreso between @FechaIni and @FechaFin ";

                    BuscarDesarrollo.DataSourceID = "solicitudXCliente";
                    BuscarDesarrollo.DataBind();
                }
                else
                {
                    SolicXFecha.SelectCommand = "SELECT * FROM tblSoliciDiseEspe  " +
                                                "WHERE  Fecha_Ingreso between @FechaIni and @FechaFin  ";



                    BuscarDesarrollo.DataSourceID = "SolicXFecha";
                    BuscarDesarrollo.DataBind();
                }


                // Control de la activacion del boton de cliente 
                string script = "<script>ControlBtnCliente();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, false);


            }




        }

        protected void DataGridBuscarDesarrollo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {


                // Obtener los valores de las columnas ocultas
                int programadoVentas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas"));
                object pausadoObj = DataBinder.Eval(e.Item.DataItem, "Pausado");
                int pausado = pausadoObj != DBNull.Value ? (bool)pausadoObj ? 1 : 0 : 0;
                int terminado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Terminado"));
                int Urgente = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Urgente"));
                int DesComplejo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DesarrolloComplejo"));
                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));


                if (terminado == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                }
                else if (DesComplejo == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#77a765");    //Verde 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#e9a270");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }

                else if (programadoVentas == 1 && pausado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#70ede4");    // Aqua

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


        protected void DataGridSolicitudPE_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerDesarrollo")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid1.Items[rowIndex];

                // Se utiliza para darle el color solo a la fila seleccionada 
                foreach (DataGridItem item in DataGrid1.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada1";



                string IdSolicitud = row.Cells[1].Text;
                Session["Id_Solicitud"] = IdSolicitud;
                ID_Sol_Dib.Text = IdSolicitud;
                string NombreProyecto = row.Cells[2].Text;
                string Asesor = row.Cells[3].Text;
                string FechaIngreso = row.Cells[4].Text;
                DateTime FechaIngresoForm = DateTime.Parse(FechaIngreso);
                string Dirigidoa = row.Cells[5].Text;
                string Tipo = row.Cells[6].Text;
                string RealizadoPor = row.Cells[7].Text;
                string termiVenta = row.Cells[8].Text;
                string pausado = row.Cells[9].Text;
                string TermiDiseño = row.Cells[10].Text;
                string FechaEntrega = row.Cells[11].Text;
                DateTime FechaEntregaForm = DateTime.Parse(FechaEntrega);
                string FechaRespuesta = row.Cells[12].Text;
                DateTime FechaRespuestaForm = DateTime.Parse(FechaRespuesta);
                string SoliOrigen = row.Cells[13].Text;
                string Ciudad = row.Cells[14].Text;
                string Viatico = row.Cells[15].Text;
                string Cotizacion = row.Cells[16].Text;
                string Cliente = row.Cells[17].Text;
                string Contacto = row.Cells[18].Text;
                string Telefono = row.Cells[19].Text;
                string Celular = row.Cells[20].Text;
                string Mail = row.Cells[21].Text;
                string Direccion = row.Cells[22].Text;
                string SegPausa = row.Cells[23].Text;
                string DesComplejo = row.Cells[24].Text;
                string Urgente = row.Cells[25].Text;

                lbNumeroSolicitud.Text = IdSolicitud;
                tbProyecto.Text = NombreProyecto;
                tbProyectoServidor.Text = NombreProyecto;

                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == Asesor)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbFechaIngreso.Text = FechaIngresoForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaIngresoServidor.Text = FechaIngresoForm.ToString("yyyy-MM-ddTHH:mm");
                foreach (ListItem item in ddlDirigido.Items)
                {
                    if (item.Text == Dirigidoa)
                    {
                        ddlDirigido.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                foreach (ListItem item in ddlTipo.Items)
                {
                    if (item.Text == Tipo)
                    {
                        ddlTipo.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbDesarrollaPor.Text = RealizadoPor.Replace("&nbsp;", "PENDIENTE");
                tbFechaEntrega.Text = FechaEntregaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaEntregaServidor.Text = FechaEntregaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaRespuesta.Text = FechaRespuestaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaRespuestaServidor.Text = FechaRespuestaForm.ToString("yyyy-MM-ddTHH:mm");
                tbSolicitudOrigen.Text = SoliOrigen;

                foreach (ListItem item in ddlCiudad.Items)
                {
                    if (item.Text == Ciudad)
                    {
                        ddlCiudad.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }

                if (Viatico == "True")
                {
                    chxViaticos.Checked = true;
                }
                else
                {
                    chxViaticos.Checked = false;
                }

                tbCotizacionEsp.Text = Cotizacion;
                tbCliente.Text = Cliente;
                tbClienteServidor.Text = Cliente;
                tbContacto.Text = Contacto;
                tbContactoServidor.Text = Contacto;
                tbTelefono.Text = Telefono;
                tbTelefonoServidor.Text = Telefono;
                tbCelular.Text = Celular;
                tbCelularServidor.Text = Celular;
                tbMail.Text = Mail;
                tbMailServidor.Text = Mail;
                tbDireccion.Text = Direccion;
                tbDireccionServidor.Text = Direccion;

                if (SegPausa == "&nbsp;")
                {
                    string SegPausaRep = SegPausa.Replace("&nbsp;", "");
                    txObsDesarrollo.InnerText = SegPausaRep;
                }
                else
                {
                    txSegPausa.InnerText = SegPausa;
                }
                chxUrgente.Checked = Convert.ToBoolean(Urgente);
                chxDesComplejo.Checked = Convert.ToBoolean(DesComplejo);

                //Limpiamos Campos de Detalle 
                LimpiarCamposDetalle();

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                tbId_Fila.Text = rowIndex.ToString();





                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {

                    DataGrid2.DataSourceID = "CargarCotizaciones";
                    DataGrid2.DataBind();
                    BuscarDesarrollo.DataBind();

                    if (termiVenta != "True")
                    {

                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        string script = "<script>HabilEnla1Ventas();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                    }
                    else
                    {
                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn btn-secondary";

                        string script = "<script>HabilitarEnlaces4();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                    }

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {

                    //Se refrescan los otros datagrid 
                    CargarCotizaciones_Metodo();

                    if (TermiDiseño != "True")
                    {
                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        ConfirmarComplejo.Enabled = true;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-primary";

                        btnConUrgente.Enabled = true;
                        btnConUrgente.CssClass = "btn btn-sm btn-primary";

                        btnTrabajarSolicitud.Enabled = true;
                        btnTrabajarSolicitud.CssClass = "btn btn-sm btn-outline-primary  btn-dept";

                        btnDesprogramar.Enabled = true;
                        btnDesprogramar.CssClass = "btn btn-sm btn-outline-primary btn-dept";

                        btnTrbajarCotizacion.Enabled = false;
                        btnTrbajarCotizacion.CssClass = "btn btn-sm btn-outline-secondary";

                        btnDesprogramar1.Enabled = false;
                        btnDesprogramar1.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = true;
                        chxUrgente.Enabled = true;

                        if (termiVenta == "True")
                        {
                            if (pausado == "True")
                            {
                                string script = "<script>HabEnlDiseñoPausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                            else
                            {
                                string script = "<script>HabEnlDiseño();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }

                        }
                        else
                        {
                            if (pausado == "True")
                            {
                                string script = "<script>HabEnlDiseño3Pausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                            else
                            {
                                string script = "<script>HabEnlDiseño3();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                        }

                    }
                    else
                    {
                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-secondary";


                        ConfirmarComplejo.Enabled = false;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-secondary";

                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = false;
                        chxUrgente.Enabled = false;

                        string script = "<script>HabEnlDiseño2();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


                    }
                }


            }

            if (e.CommandName == "VerCotizacion")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid2.Items[rowIndex];

                // Se utiliza para darle el color solo a la fila seleccionada 
                foreach (DataGridItem item in DataGrid2.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada1";

                string IdSolicitud = row.Cells[1].Text;
                Session["Id_Solicitud"] = IdSolicitud;
                ID_Cot_Dib.Text = IdSolicitud;
                string NombreProyecto = row.Cells[2].Text;
                string Asesor = row.Cells[3].Text;
                string FechaIngreso = row.Cells[4].Text;
                DateTime FechaIngresoForm = DateTime.Parse(FechaIngreso);
                string Dirigidoa = row.Cells[5].Text;
                string Tipo = row.Cells[6].Text;
                string RealizadoPor = row.Cells[7].Text;
                string termiVenta = row.Cells[8].Text;
                string pausado = row.Cells[9].Text;
                string TermiDiseño = row.Cells[10].Text;
                string FechaEntrega = row.Cells[11].Text;
                DateTime FechaEntregaForm = DateTime.Parse(FechaEntrega);
                string FechaRespuesta = row.Cells[12].Text;
                DateTime FechaRespuestaForm = DateTime.Parse(FechaRespuesta);
                string SoliOrigen = row.Cells[13].Text;
                string Ciudad = row.Cells[14].Text;
                string Viatico = row.Cells[15].Text;
                string Cotizacion = row.Cells[16].Text;
                string Cliente = row.Cells[17].Text;
                string Contacto = row.Cells[18].Text;
                string Telefono = row.Cells[19].Text;
                string Celular = row.Cells[20].Text;
                string Mail = row.Cells[21].Text;
                string Direccion = row.Cells[22].Text;
                string SegPausa = row.Cells[23].Text;
                string DesComplejo = row.Cells[24].Text;
                string Urgente = row.Cells[25].Text;

                lbNumeroSolicitud.Text = IdSolicitud;
                tbProyecto.Text = NombreProyecto;
                tbProyectoServidor.Text = NombreProyecto;

                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == Asesor)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbFechaIngreso.Text = FechaIngresoForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaIngresoServidor.Text = FechaIngresoForm.ToString("yyyy-MM-ddTHH:mm");
                foreach (ListItem item in ddlDirigido.Items)
                {
                    if (item.Text == Dirigidoa)
                    {
                        ddlDirigido.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                foreach (ListItem item in ddlTipo.Items)
                {
                    if (item.Text == Tipo)
                    {
                        ddlTipo.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbDesarrollaPor.Text = RealizadoPor.Replace("&nbsp;", "PENDIENTE");
                tbFechaEntrega.Text = FechaEntregaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaEntregaServidor.Text = FechaEntregaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaRespuesta.Text = FechaRespuestaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaRespuestaServidor.Text = FechaRespuestaForm.ToString("yyyy-MM-ddTHH:mm");
                tbSolicitudOrigen.Text = SoliOrigen;


                foreach (ListItem item in ddlCiudad.Items)
                {
                    if (item.Text == Ciudad)
                    {
                        ddlCiudad.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }

                if (Viatico == "True")
                {
                    chxViaticos.Checked = true;
                }
                else
                {
                    chxViaticos.Checked = false;
                }

                tbCotizacionEsp.Text = Cotizacion;
                tbCliente.Text = Cliente;
                tbClienteServidor.Text = Cliente;
                tbContacto.Text = Contacto;
                tbContactoServidor.Text = Contacto;
                tbTelefono.Text = Telefono;
                tbTelefonoServidor.Text = Telefono;
                tbCelular.Text = Celular;
                tbCelularServidor.Text = Celular;
                tbMail.Text = Mail;
                tbMailServidor.Text = Mail;
                tbDireccion.Text = Direccion;
                tbDireccionServidor.Text = Direccion;

                if (SegPausa == "&nbsp;")
                {
                    string SegPausaRep = SegPausa.Replace("&nbsp;", "");
                    txObsDesarrollo.InnerText = SegPausaRep;
                }
                else
                {
                    txSegPausa.InnerText = SegPausa;
                }
                chxUrgente.Checked = Convert.ToBoolean(Urgente);
                chxDesComplejo.Checked = Convert.ToBoolean(DesComplejo);

                //Limpiamos Campos de Detalle 
                LimpiarCamposDetalle();

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                tbId_Fila.Text = rowIndex.ToString();





                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {

                    DataGrid1.DataSourceID = "CargarDesarrollos"; DataGrid1.DataSourceID = "CargarDesarrollos";
                    DataGrid1.DataBind();
                    BuscarDesarrollo.DataBind();

                    if (termiVenta != "True")
                    {


                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        string script = "<script>HabilEnla1Ventas();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                    }
                    else
                    {

                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn btn-secondary";

                        string script = "<script>HabilitarEnlaces4();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                    }

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {

                    // se refrescan lo demas datagrid
                    CargarDesarrollos_Metodo();

                    if (TermiDiseño != "True")
                    {

                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        ConfirmarComplejo.Enabled = true;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-primary";

                        btnConUrgente.Enabled = true;
                        btnConUrgente.CssClass = "btn btn-sm btn-primary";


                        btnTrbajarCotizacion.Enabled = true;
                        btnTrbajarCotizacion.CssClass = "btn btn-sm btn-outline-primary btn-depth ";

                        btnDesprogramar1.Enabled = true;
                        btnDesprogramar1.CssClass = "btn btn-sm btn-outline-primary btn-dept";

                        btnTrabajarSolicitud.Enabled = false;
                        btnTrabajarSolicitud.CssClass = "btn btn-sm btn-outline-secondary";

                        btnDesprogramar.Enabled = false;
                        btnDesprogramar.CssClass = "btn btn-sm btn-outline-secondary";


                        chxDesComplejo.Enabled = true;
                        chxUrgente.Enabled = true;

                        if (termiVenta == "True")
                        {
                            if (pausado == "True")
                            {
                                string script = "<script>HabEnlDiseñoPausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                            else
                            {
                                string script = "<script>HabEnlDiseño();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }

                        }
                        else
                        {
                            if (pausado == "True")
                            {
                                string script = "<script>HabEnlDiseño3Pausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                            else
                            {
                                string script = "<script>HabEnlDiseño3();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                        }

                    }
                    else
                    {

                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-secondary";


                        ConfirmarComplejo.Enabled = false;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-secondary";

                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = false;
                        chxUrgente.Enabled = false;

                        string script = "<script>HabEnlDiseño2();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


                    }
                }




            }

            if (e.CommandName == "VerBuscado")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = BuscarDesarrollo.Items[rowIndex];

                // Se utiliza para darle el color solo a la fila seleccionada 
                foreach (DataGridItem item in BuscarDesarrollo.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada1";


                string IdSolicitud = row.Cells[1].Text;
                Session["Id_Solicitud"] = IdSolicitud;
                string NombreProyecto = row.Cells[2].Text;
                string Asesor = row.Cells[3].Text;
                string FechaIngreso = row.Cells[4].Text;
                DateTime FechaIngresoForm = DateTime.Parse(FechaIngreso);
                string Dirigidoa = row.Cells[5].Text;
                string Tipo = row.Cells[6].Text;
                string RealizadoPor = row.Cells[7].Text;
                string termiVenta = row.Cells[8].Text;
                string pausado = row.Cells[9].Text;
                string TermiDiseño = row.Cells[10].Text;
                string FechaEntrega = row.Cells[11].Text;
                DateTime FechaEntregaForm = DateTime.Parse(FechaEntrega);
                string FechaRespuesta = row.Cells[12].Text;
                DateTime FechaRespuestaForm = DateTime.Parse(FechaRespuesta);
                string SoliOrigen = row.Cells[13].Text;
                string Ciudad = row.Cells[14].Text;
                string Viatico = row.Cells[15].Text;
                string Cotizacion = row.Cells[16].Text;
                string Cliente = row.Cells[17].Text;
                string Contacto = row.Cells[18].Text;
                string Telefono = row.Cells[19].Text;
                string Celular = row.Cells[20].Text;
                string Mail = row.Cells[21].Text;
                string Direccion = row.Cells[22].Text;
                string SegPausa = row.Cells[23].Text;
                string DesComplejo = row.Cells[24].Text;
                string Urgente = row.Cells[25].Text;

                lbNumeroSolicitud.Text = IdSolicitud;
                tbProyecto.Text = NombreProyecto;
                tbProyectoServidor.Text = NombreProyecto;
                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == Asesor)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbFechaIngreso.Text = FechaIngresoForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaIngresoServidor.Text = FechaIngresoForm.ToString("yyyy-MM-ddTHH:mm");
                foreach (ListItem item in ddlDirigido.Items)
                {
                    if (item.Text == Dirigidoa)
                    {
                        ddlDirigido.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                foreach (ListItem item in ddlTipo.Items)
                {
                    if (item.Text == Tipo)
                    {
                        ddlTipo.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbDesarrollaPor.Text = RealizadoPor.Replace("&nbsp;", "PENDIENTE");
                tbFechaEntrega.Text = FechaEntregaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaEntregaServidor.Text = FechaEntregaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaRespuesta.Text = FechaRespuestaForm.ToString("yyyy-MM-ddTHH:mm");
                tbFechaRespuestaServidor.Text = FechaRespuestaForm.ToString("yyyy-MM-ddTHH:mm");
                tbSolicitudOrigen.Text = SoliOrigen;


                foreach (ListItem item in ddlCiudad.Items)
                {
                    if (item.Text == Ciudad)
                    {
                        ddlCiudad.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }

                if (Viatico == "True")
                {
                    chxViaticos.Checked = true;
                }
                else
                {
                    chxViaticos.Checked = false;
                }

                tbCotizacionEsp.Text = Cotizacion;
                tbCliente.Text = Cliente;
                tbClienteServidor.Text = Cliente;
                tbContacto.Text = Contacto;
                tbContactoServidor.Text = Contacto;
                tbTelefono.Text = Telefono;
                tbTelefonoServidor.Text = Telefono;
                tbCelular.Text = Celular;
                tbCelularServidor.Text = Celular;
                tbMail.Text = Mail;
                tbMailServidor.Text = Mail;
                tbDireccion.Text = Direccion;
                tbDireccionServidor.Text = Direccion;

                if (SegPausa == "&nbsp;")
                {
                    string SegPausaRep = SegPausa.Replace("&nbsp;", "");
                    txObsDesarrollo.InnerText = SegPausaRep;
                }
                else
                {
                    txSegPausa.InnerText = SegPausa;
                }
                chxUrgente.Checked = Convert.ToBoolean(Urgente);
                chxDesComplejo.Checked = Convert.ToBoolean(DesComplejo);

                //Limpiamos Campos de Detalle 
                LimpiarCamposDetalle();

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;




                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    DataGrid1.DataSourceID = "CargarDesarrollos";
                    DataGrid1.DataBind();
                    DataGrid2.DataSourceID = "CargarCotizaciones";
                    DataGrid2.DataBind();

                    if (termiVenta != "True")
                    {

                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        string script = "<script>HabilEnla1Ventas();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);


                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                    }
                    else
                    {
                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn btn-secondary";

                        string script = "<script>HabilitarEnlaces4();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                    }

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {
                    //Se refrescan lo demas datagrid
                    CargarDesarrollos_Metodo();
                    CargarCotizaciones_Metodo();

                    if (TermiDiseño != "True")
                    {

                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        ConfirmarComplejo.Enabled = true;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-primary";

                        btnConUrgente.Enabled = true;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-primary";

                        chxDesComplejo.Enabled = true;
                        chxUrgente.Enabled = true;

                        if (termiVenta == "True")
                        {
                            if (pausado == "True")
                            {
                                string script = "<script>HabEnlDiseñoPausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                            else
                            {
                                string script = "<script>HabEnlDiseño();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }

                        }
                        else
                        {
                            if (pausado == "True")
                            {
                                string script = "<script>HabEnlDiseño3Pausado();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                            else
                            {
                                string script = "<script>HabEnlDiseño3();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);

                                // Llamar a la función JavaScript para enfocar y desplazar la fila
                                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);
                            }
                        }

                    }
                    else
                    {

                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-secondary";


                        ConfirmarComplejo.Enabled = false;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-secondary";

                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = false;
                        chxUrgente.Enabled = false;

                        string script = "<script>HabEnlDiseño2();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);

                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


                    }
                }

            }

        }

        private void LimpiarCamposDetalle()
        {
            txDescProduc.InnerText = "";
            tbProveedor.Text = "";
            tbAncho.Text = "";
            tbAltura.Text = "";
            tbProfundidad.Text = "";
            tbMaterial.Text = "";
            tbCantidad.Text = "";
            txEspGen.InnerText = "";
            txobsCompras.InnerText = "";
            txObsDesarrollo.InnerText = "";
            tbCostoC.Text = "";
            tbFactorC.Text = "";
            tbProve.Text = "";
            tbCostoD.Text = "";
            tbFactorD.Text = "";
            tbPrecioSugerido.Text = "";
        }

        protected void GuardarModificarSolicitud(object sender, EventArgs e)
        {

            string insertUpdate = Session["InsertUpdate"] as string;

            if (insertUpdate == "Insertar")
            {
                // Validar Campos de cliente 

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    using (SqlCommand getMaxIdCmd = new SqlCommand("SELECT Max(ID_Solicitud) FROM tblSoliciDiseEspe", connection))
                    {
                        object maxIdObj = getMaxIdCmd.ExecuteScalar();
                        int maxId = (maxIdObj != null && maxIdObj != DBNull.Value) ? Convert.ToInt32(maxIdObj) : 0;


                        int nuevoId = maxId + 1;


                        string IdSolicitud = nuevoId.ToString();
                        connection.Close();

                        using (SqlCommand cmd = new SqlCommand("sp_InsertarSolicitudDiseñoEspecial", connection))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;


                            cmd.Parameters.AddWithValue("@ID_Solicitud", IdSolicitud);
                            cmd.Parameters.AddWithValue("@Fecha_Ingreso", Convert.ToDateTime(tbFechaIngresoServidor.Text));
                            cmd.Parameters.AddWithValue("@Fecha_Programada_Entrega", Convert.ToDateTime(tbFechaEntregaServidor.Text));
                            cmd.Parameters.AddWithValue("@FechaRespuesta", Convert.ToDateTime(tbFechaRespuestaServidor.Text));

                            cmd.Parameters.AddWithValue("@Zona", Session["ZonaAsesor"].ToString());
                            cmd.Parameters.AddWithValue("@Asesor", ddlAsesor.SelectedItem.Text);

                            cmd.Parameters.AddWithValue("@Proyecto", tbProyecto.Text);
                            cmd.Parameters.AddWithValue("@Cliente", tbClienteServidor.Text);
                            cmd.Parameters.AddWithValue("@Contacto", tbContactoServidor.Text);
                            cmd.Parameters.AddWithValue("@Telefono", tbTelefonoServidor.Text);
                            cmd.Parameters.AddWithValue("@Celular", tbCelularServidor.Text);
                            cmd.Parameters.AddWithValue("@Mail", tbMailServidor.Text);

                            cmd.Parameters.AddWithValue("@Direccion", tbDireccionServidor.Text);
                            cmd.Parameters.AddWithValue("@TipoSolicitud", ddlTipo.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@Dirigidoa", ddlDirigido.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@id_SolicitudOrigen", tbSolicitudOrigen.Text);
                            cmd.Parameters.AddWithValue("@CiudadProyecto", ddlCiudad.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@CotizarViaTte", chxViaticos.Checked);
                            cmd.Parameters.AddWithValue("@Cotizacion", tbCotizacionEsp.Text);
                            cmd.Parameters.AddWithValue("@RealizadoPor", tbDesarrollaPor.Text);
                            cmd.Parameters.AddWithValue("@DesarrolloComplejo", chxDesComplejo.Checked);
                            connection.Open();


                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                //Variables de Session para mantener a la hora de Guardar, Se pueden eliminar si cargan mucho el proceso  

                                Session["FecIngrSolSession"] = tbFechaIngresoServidor.Text;
                                Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
                                Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
                                Session["DirigidoSession"] = ddlDirigido.SelectedItem.Text;
                                Session["TipoSession"] = ddlTipo.SelectedItem.Text;
                                Session["SolOrigenSession"] = tbSolicitudOrigen.Text;
                                Session["ProyectoSolSession"] = tbProyecto.Text;
                                Session["CiudadSession"] = ddlCiudad.SelectedItem.Text;
                                Session["ViaticoSession"] = chxViaticos.Checked;
                                Session["CotizacionSession"] = tbCotizacionEsp.Text;
                                Session["ClienteSolSession"] = tbClienteServidor.Text;
                                Session["ContactoSolSession"] = tbContactoServidor.Text;
                                Session["TelSeolSession"] = tbTelefonoServidor.Text;
                                Session["CelularSolSession"] = tbCelularServidor.Text;
                                Session["Mailsolsession"] = tbMailServidor.Text;
                                Session["DirecccionSolSession"] = tbDireccionServidor.Text;
                                Session["AsesorSolSession"] = ddlAsesor.SelectedItem.Text;
                                Session["numeroSolicitudSession"] = IdSolicitud;

                                Session["ScriptEspecifico"] = "ActiBotDetalleVentas();";

                                string mensajePersonalizado = "La solicitud ha sido ingresada con éxito.";
                                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");



                            }
                            else
                            {
                                string mensajePersonalizado = "¡Ups! La solicitud no se ingresó correctamente. Por favor, comuníquese con el Departamento de Sistemas para obtener ayuda.";
                                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }

                        }
                    }
                }
            }

            else if (insertUpdate == "Actualizar")
            {

                // Validar si la solicitud ya ha sido a aprobada por Ventas(validacion Boton de modificar )

                if (!ConsultarTerminadoVentas())
                {
                    string mensajePersonalizado = "La solicitud ya ha sido programda para ventas y no puede ser modificada.";
                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }


                //CalcularFechaEntregaSolicitudEspecial() de momento se envia fecha del primer dia del año  !!!!IMPORTANTE !!!!

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("sp_ActualizarSolicitudDiseñoEspecial", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;



                        cmd.Parameters.AddWithValue("@ID_Solicitud", lbNumeroSolicitud.Text);
                        cmd.Parameters.AddWithValue("@Fecha_Ingreso", Convert.ToDateTime(tbFechaIngresoServidor.Text));
                        cmd.Parameters.AddWithValue("@Fecha_Programada_Entrega", Convert.ToDateTime(tbFechaEntregaServidor.Text));
                        cmd.Parameters.AddWithValue("@FechaRespuesta", Convert.ToDateTime(tbFechaRespuestaServidor.Text));

                        cmd.Parameters.AddWithValue("@Zona", Session["ZonaAsesor"].ToString());
                        cmd.Parameters.AddWithValue("@Asesor", ddlAsesor.SelectedItem.Text);

                        cmd.Parameters.AddWithValue("@Proyecto", tbProyecto.Text);
                        cmd.Parameters.AddWithValue("@Cliente", tbClienteServidor.Text);
                        cmd.Parameters.AddWithValue("@Contacto", tbContactoServidor.Text);
                        cmd.Parameters.AddWithValue("@Telefono", tbTelefonoServidor.Text);
                        cmd.Parameters.AddWithValue("@Celular", tbCelularServidor.Text);
                        cmd.Parameters.AddWithValue("@Mail", tbMailServidor.Text);

                        cmd.Parameters.AddWithValue("@Direccion", tbDireccionServidor.Text);
                        cmd.Parameters.AddWithValue("@TipoSolicitud", ddlTipo.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@Dirigidoa", ddlDirigido.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@id_SolicitudOrigen", tbSolicitudOrigen.Text);
                        cmd.Parameters.AddWithValue("@CiudadProyecto", ddlCiudad.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@CotizarViaTte", chxViaticos.Checked);
                        cmd.Parameters.AddWithValue("@Cotizacion", tbCotizacionEsp.Text);
                        cmd.Parameters.AddWithValue("@RealizadoPor", tbDesarrollaPor.Text);
                        cmd.Parameters.AddWithValue("@DesarrolloComplejo", chxDesComplejo.Checked);

                        connection.Open();


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Session["FecIngrSolSession"] = tbFechaIngresoServidor.Text;
                            Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
                            Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
                            Session["DirigidoSession"] = ddlDirigido.SelectedItem.Text;
                            Session["TipoSession"] = ddlTipo.SelectedItem.Text;
                            Session["SolOrigenSession"] = tbSolicitudOrigen.Text;
                            Session["ProyectoSolSession"] = tbProyecto.Text;
                            Session["CiudadSession"] = ddlCiudad.Text;
                            Session["ViaticoSession"] = chxViaticos.Checked;
                            Session["CotizacionSession"] = tbCotizacionEsp.Text;
                            Session["ClienteSolSession"] = tbClienteServidor.Text;
                            Session["ContactoSolSession"] = tbContactoServidor.Text;
                            Session["TelSeolSession"] = tbTelefonoServidor.Text;
                            Session["CelularSolSession"] = tbCelularServidor.Text;
                            Session["Mailsolsession"] = tbMailServidor.Text;
                            Session["DirecccionSolSession"] = tbDireccionServidor.Text;
                            Session["AsesorSolSession"] = ddlAsesor.SelectedItem.Text;
                            Session["numeroSolicitudSession"] = lbNumeroSolicitud.Text;

                            Session["ScriptEspecifico"] = "ActiBotDetalleVentas();";

                            string mensajePersonalizado = "La solicitud  ha sido actualizada con éxito";
                            string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }
                        else
                        {
                            string mensajePersonalizado = "¡Ups! La Solicitud no se Actualizó correctamente.Por favor Comuniquese con el Departamento de Sistemas para obtener ayuda";
                            string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                        }




                    }
                }

            }
            else
            {
                string mensajePersonalizado = "La solicitud ya ha sido programda para ventas y no puede ser modificada.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }


        }

        protected void GuardarDatosSesion(object sender, EventArgs e)
        {

            Session["FecIngrSolSession"] = tbFechaIngresoServidor.Text;
            Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
            Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
            Session["ProyectoSession"] = tbProyecto.Text;
            Session["SolicitudOrigen"] = tbSolicitudOrigen.Text;
            Session["Cotizacion"] = tbCotizacionEsp.Text;
            Session["Desarrollado"] = tbDesarrollaPor.Text;
            Session["Ciudad"] = ddlCiudad.SelectedItem.Text;
            Session["Dirigido"] = ddlDirigido.SelectedItem.Text;
            Session["Tipo"] = ddlTipo.SelectedItem.Text;
            Session["AsesorSol"] = ddlAsesor.SelectedItem.Text;
            Session["Id_Solicitud_Pantalla"] = lbNumeroSolicitud.Text;

        }

        private void CargarSession()
        {

            string proyecto = Session["ProyectoSession"]?.ToString();
            string SoliOrigen = Session["SolicitudOrigen"]?.ToString();
            string Cotizacion = Session["Cotizacion"]?.ToString();
            string Desarrolado = Session["Desarrollado"]?.ToString();
            string Ciudad = Session["Ciudad"]?.ToString();
            string Dirigido = Session["Dirigido"]?.ToString();
            string Tipo = Session["Tipo"]?.ToString();
            string AsesorSol = Session["AsesorSol"]?.ToString();
            string NumeroSolPantalla = Session["Id_Solicitud_Pantalla"]?.ToString();

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(proyecto) || !string.IsNullOrEmpty(SoliOrigen) || !string.IsNullOrEmpty(Cotizacion) || !string.IsNullOrEmpty(Desarrolado) || !string.IsNullOrEmpty(Ciudad) || !string.IsNullOrEmpty(Dirigido))
                {
                    tbProyecto.Text = proyecto;
                    tbSolicitudOrigen.Text = SoliOrigen;
                    tbCotizacionEsp.Text = Cotizacion;
                    tbDesarrollaPor.Text = Desarrolado;

                    // Asigna el valor de la variable de sesión 'Ciudad' al DropDownList
                    ddlCiudad.DataBind();
                    CargarAsesoresEnDropDownList();

                    if (!string.IsNullOrEmpty(Ciudad))
                    {
                        ListItem ciudadItem = ddlCiudad.Items.FindByText(Ciudad);
                        if (ciudadItem != null)
                        {

                            ciudadItem.Selected = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(Dirigido))
                    {
                        ListItem dirigido = ddlDirigido.Items.FindByText(Dirigido);
                        if (dirigido != null)
                        {

                            dirigido.Selected = true;
                        }
                    }


                    if (!string.IsNullOrEmpty(Tipo))
                    {
                        ListItem Tipo1 = ddlTipo.Items.FindByText(Tipo);
                        if (Tipo1 != null)
                        {

                            Tipo1.Selected = true;
                        }
                    }



                    if (!string.IsNullOrEmpty(Tipo))
                    {
                        ListItem AsesorSol1 = ddlAsesor.Items.FindByText(AsesorSol);
                        if (AsesorSol1 != null)
                        {

                            AsesorSol1.Selected = true;
                        }
                    }

                    //Cargar Numero solicitud
                    lbNumeroSolicitud.Text = NumeroSolPantalla;


                    if (tbCliente.Text != "")
                    {
                        Session.Remove("ProyectoSession");
                        Session.Remove("SolicitudOrigen");
                        Session.Remove("Cotizacion");
                        Session.Remove("Desarrolado");
                        Session.Remove("Ciudad");
                        Session.Remove("Dirigido");
                        Session.Remove("Tipo");
                        Session.Remove("AsesorSol");
                        Session.Remove("Id_Solicitud_Pantalla");

                        string script = "<script>ActivarGuardar();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ActivarGuardar", script, false);

                    }


                }


            }


        }

        protected void LimpiarCampos(object sender, EventArgs e)
        {

            Session.Remove("ProyectoSession");
            Session.Remove("SolicitudOrigen");
            Session.Remove("Cotizacion");
            Session.Remove("Desarrolado");
            Session.Remove("Ciudad");
            Session.Remove("Dirigido");
            Session.Remove("Tipo");
            Session.Remove("AsesorSol");
            Session.Remove("Id_Solicitud_Pantalla");

            Response.Redirect("~/Formularios/Ventas/Solicitud_Especial.aspx");


        }



        // Programar y Terminar Solicitud Especial 
        protected void ProgramarSolicitud(object sender, EventArgs e)
        {

            bool DetalleEntrados = false;
            bool PrecioSugerido = false;
            DateTime FechaIngreso = DateTime.Now;
            DateTime FechaEntrega;

            //Verificar Cual departamento de la boton 
            switch (Session["Departamento"].ToString().ToUpper())
            {
                case "VENTAS":


                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string consultaSQL = "SELECT COUNT(*) FROM tblSoliciDiseEspeDeta WHERE ID_Solicitud = @IdSolicitud";

                        using (SqlCommand cmd = new SqlCommand(consultaSQL, connection))
                        {
                            cmd.Parameters.AddWithValue("@IdSolicitud", lbNumeroSolicitud.Text);

                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)
                            {

                                DetalleEntrados = true;
                                connection.Close();
                            }
                        }
                    }

                    if (DetalleEntrados) // Si  tiene detalles validamos 
                    {
                        // validamos si es destinatario es Desarrollo 
                        if (ddlTipo.Text == "DESARROLLO")
                        {

                            //Vaidamos si alguno de los detalles tiene valor sugerido =< 0
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();

                                string consultaSQL = "SELECT COUNT(*) FROM tblSoliciDiseEspeDeta WHERE ID_Solicitud = @IdSolicitud AND PrecioSugerido<=0";

                                using (SqlCommand cmd = new SqlCommand(consultaSQL, connection))
                                {
                                    cmd.Parameters.AddWithValue("@IdSolicitud", lbNumeroSolicitud.Text);

                                    int count = (int)cmd.ExecuteScalar();
                                    if (count > 0)
                                    {
                                        // Se encontraron registros que cumplen la condición
                                        PrecioSugerido = true;
                                    }
                                }
                            }


                            if (PrecioSugerido)
                            {
                                //Se calcula la Fecha de entrega 
                                FechaEntrega = CalcularFechaEntrega(FechaIngreso);

                                // Se encontraron Detalles de esa solicitud con PrecioSugerido =< 0 
                                // Se debe Realizar Validacion  aun no esta clara  ?????????????? Pendiente  

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();
                                    //Realizamos la Actializacion 
                                    string query = "Update  tblSoliciDiseEspe set ProgramadoVentas = 1,Fecha_Ingreso = @FechaIngreso," +
                                        "Fecha_Programada_Entrega = @FechaProgramadaEntrega where Id_Solicitud =  @Id_Solicitud";


                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {

                                        command.Parameters.AddWithValue("@FechaIngreso", FechaIngreso);
                                        command.Parameters.AddWithValue("@FechaProgramadaEntrega", FechaEntrega);
                                        command.Parameters.AddWithValue("@Id_Solicitud", lbNumeroSolicitud.Text);

                                        command.ExecuteNonQuery();
                                    }

                                    // Mensaje de éxito
                                    string mensajePersonalizado = "La Solicitud. " + lbNumeroSolicitud.Text + " ha sido programada éxitosamente";
                                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                                }

                            }

                            else
                            {
                                //Se  Realizar Calculo de fecha entrega y
                                FechaEntrega = CalcularFechaEntrega(FechaIngreso);

                                //  se realiza la la actualizacion en la base de datos del campo programar

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();
                                    //Realizamos la Actializacion 
                                    string query = "Update  tblSoliciDiseEspe set ProgramadoVentas = 1,Fecha_Ingreso = @FechaIngreso," +
                                        "Fecha_Programada_Entrega = @FechaProgramadaEntrega where Id_Solicitud =  @Id_Solicitud";


                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        // Aquí defines los parámetros de la consulta
                                        command.Parameters.AddWithValue("@FechaIngreso", FechaIngreso);
                                        command.Parameters.AddWithValue("@FechaProgramadaEntrega", FechaEntrega);
                                        command.Parameters.AddWithValue("@Id_Solicitud", lbNumeroSolicitud.Text);

                                        command.ExecuteNonQuery();
                                    }

                                    // Mensaje de éxito
                                    string mensajePersonalizado = "La Solicitud. " + lbNumeroSolicitud.Text + " ha sido programada éxitosamente";
                                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                                }



                            }

                        }

                        else
                        {
                            // Es para Compras o Desarralo de Producto  simplemente se calcula la fecha de entrega y se realiza la 
                            FechaEntrega = CalcularFechaEntrega(FechaIngreso);

                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                //Realizamos la Actializacion 
                                string query = "Update  tblSoliciDiseEspe set ProgramadoVentas = 1,Fecha_Ingreso = @FechaIngreso," +
                                       "Fecha_Programada_Entrega = @FechaProgramadaEntrega where Id_Solicitud =  @Id_Solicitud";


                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    // Aquí defines los parámetros de la consulta
                                    command.Parameters.AddWithValue("@FechaIngreso", FechaIngreso);
                                    command.Parameters.AddWithValue("@FechaProgramadaEntrega", FechaEntrega);
                                    command.Parameters.AddWithValue("@Id_Solicitud", lbNumeroSolicitud.Text);

                                    command.ExecuteNonQuery();
                                }

                                // Mensaje de éxito
                                string mensajePersonalizado = "La Solicitud: " + lbNumeroSolicitud.Text + " ha sido programada éxitosamente";
                                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                            }

                        }
                    }
                    else
                    {
                        string mensajePersonalizado = "La solicitud no tiene ningun detalle asociado, No se puede programar en este momento";
                        string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");


                    }

                    break;

                case "COMPRAS": //Boton Programar  Departamento Compras y desarrollo de producto 
                case "DESARROLLO DE PRODUCTO":
                case "DISEÑO":

                    // Realizar el update de terminado 
                    if (TerminarSolicitudEspecialDibujo())
                    {
                        // Realizar la creacion del archivo de control de Pedidos especiales y se guarda en temporales ((( PENDIENTE )))
                        CreacionArchivoControlSolicitudEspecial(lbNumeroSolicitud.Text);


                        //string destinatario = "andersonbetancur@ducon.com.co" para realizar pruebas de correo;

                        // Se consulta el correo del asesor de la solicitud 
                        string destinatario = ConsultarCorreoAsesor();

                        // Se consulta el correo del dibujante que termina la solicitud  (( REVISAR SI  ES NOTICADO EL DIBUJANTE ))
                        string CorreoDibujante = ConsultarCorreoEmisor();

                        destinatario = destinatario + ";" + CorreoDibujante;


                        // Se valida  correo para cuando viaticos esta chekeado y se agrega 
                        if (chxViaticos.Checked)
                        {
                            string correoViatico = ConsultarCorreoViaticos();
                            destinatario = destinatario + ";" + correoViatico;
                        }

                        // se crea el cuerpo de correo 
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
                                    <h3>Notificación Solicitud Especial terminada </h3>
                                    <p>  Estimado(a) Asesor(a), por medio de la presente se informa que la solicitud de producto especial: " + lbNumeroSolicitud.Text + @"</p>
                                    <p><strong> Cliente : </strong>  " + tbCliente.Text + @"</p>
                                    <p><strong> Proyecto : </strong>  " + tbProyecto.Text + @"</p>
                                    <p><strong> Contacto : </strong>  " + tbContacto.Text + @"</p>
                                    <p><strong> Solicitud N. : </strong>  " + lbNumeroSolicitud.Text + @"</p>
                                    <p><strong> Realizado por : </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>
                                     <p><strong> Tipo solicitud : </strong>  " + ddlTipo.SelectedValue + @"</p>
                                    <p><strong>Se adjuntan: : </strong></p>
                                    <p>1. Documento de Excel con la cotización </p>
                                    <p>2. Archivos de desarrollo de producto </p>
     
       
                                </div>
                            </body>
                            </html>";


                        // Se consultan el archivo de control PE 
                        string ControlPE = "\\\\SRVDBAPPS\\S_I_Ducon$\\TemporalAdjunto\\Solicitud_N" + lbNumeroSolicitud.Text + ".xlsx";

                        // Se consultan los archivos de la solicitud  que no sean bosquejos
                        string ArchivosSPE = ConsultarRutasDocumentosSPE(lbNumeroSolicitud.Text);

                        // Rutas de los archivos de adjuntos 
                        string adjuntos = ControlPE + ";" + ArchivosSPE;


                        if (destinatario != "")
                        {
                            // Se realiza el envio del correo electronico
                            EnviarCorreoConAdjuntosTerminadoDibujo(destinatario, cuerpo, adjuntos.Trim(';'));
                        }
                        else
                        {
                            string mensajePersonalizado1 = "La solicitud ha sido terminada, pero no ha sido posible notificar por  correo electronico.";
                            string urlRedireccion1 = "Ventas/Solicitud_Especial.aspx";
                            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado1)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion1)}");
                        }



                        // Se muestra el mensaje de exito
                        string mensajePersonalizado = "La solicitud ha sido terminada y notificada.";
                        string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                    }
                    else
                    {
                        string mensajePersonalizado = "Ocurrió un error al terminar la solicitud, por favor intentalo nuevamente.";
                        string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }

                    break;

                case "RECEPCION": // Boton Programar  Departamento Compras Desarrollo Producto


                    break;


                default:
                    // Error Con el despartamento de ese usuario Validar con Sistemas 
                    break;
            }




        }


        //Metodos Terminar Solicitud Especial Dibujo 
        private bool TerminarSolicitudEspecialDibujo()
        {

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblSoliciDiseEspe SET Terminado=1, FechaRespuesta= @fechaRespuesta ,RealizadoPor = @RealizadoPor WHERE id_Solicitud = @Id_Solicitud";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@RealizadoPor", Session["usuariologueado"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@fechaRespuesta", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id_Solicitud", lbNumeroSolicitud.Text);

                    int filaAfectada = cmd.ExecuteNonQuery();

                    if (filaAfectada > 0)
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
        public bool EnviarCorreoConAdjuntosTerminadoDibujo(string destinatarios, string cuerpo, string adjuntos)
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
                        command.Parameters.AddWithValue("@Destinatarios", destinatarios);
                        command.Parameters.AddWithValue("@asunto", "Solicitud PE Terminado: " + lbNumeroSolicitud.Text + "-" + tbProyecto.Text);
                        command.Parameters.AddWithValue("@cuerpo", cuerpo);
                        command.Parameters.AddWithValue("@adjuntos", adjuntos);
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
        private void CreacionArchivoControlSolicitudEspecial(string ID_Solicitud)
        {
            try
            {
                // Crear un nuevo paquete de Excel
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregar una hoja de trabajo al paquete
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Control PE");

                    // Contrlamos los anchos:

                    //filas
                    worksheet.Row(1).Height = 90;
                    worksheet.Row(2).Height = 25;

                    // Columnas
                    worksheet.Column(1).Width = 16;
                    worksheet.Column(2).Width = 24;
                    worksheet.Column(3).Width = 16;
                    worksheet.Column(4).Width = 16;
                    worksheet.Column(5).Width = 32;
                    worksheet.Column(6).Width = 16;
                    worksheet.Column(7).Width = 24;
                    worksheet.Column(8).Width = 16;
                    worksheet.Column(9).Width = 16;
                    worksheet.Column(10).Width = 16;
                    worksheet.Column(11).Width = 16;
                    worksheet.Column(12).Width = 16;
                    worksheet.Column(13).Width = 24;


                    // Configurar bordes para la  seleccion A3:O50
                    var rangeB = worksheet.Cells["A3:O50"];

                    rangeB.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rangeB.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rangeB.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rangeB.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    rangeB.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    rangeB.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    rangeB.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    rangeB.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);

                    // Aplicar borde inferior a la fila 4 desde la columna A hasta la M
                    worksheet.Cells["A4:M4"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    worksheet.Cells["A4:M4"].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);

                    // Aplicar borde derecha a la fila 4 columna M
                    worksheet.Cells["M3"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    worksheet.Cells["M3"].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    // Aplicar borde derecha a la fila 4 columna M
                    worksheet.Cells["M4"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    worksheet.Cells["M4"].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);


                    // Aplicar borde inferior a la fila 5 desde la columna A hasta la M
                    worksheet.Cells["A5:M5"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    worksheet.Cells["A5:M5"].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);


                    // Definir el rango de celdas (de la fila 6 a la 25 en la columna A)
                    var CeldaA6A25 = worksheet.Cells["A6:A25"];

                    // Aplicar estilo a las celdas del rango
                    CeldaA6A25.Style.Fill.PatternType = ExcelFillStyle.Solid; // Definir el patrón de relleno
                    CeldaA6A25.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#76933C")); // Establecer el color de fondo

                    // Estilos adicionales si es necesario
                    CeldaA6A25.Style.Font.Name = "Calibri";
                    CeldaA6A25.Style.Font.Size = 36;

                    CeldaA6A25.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CeldaA6A25.Style.VerticalAlignment = ExcelVerticalAlignment.Top;



                    // Definir el rango de celdas (de la fila 6 a la 25 en la columna A)
                    var CeldaB6G25 = worksheet.Cells["B6:G25"];

                    // Aplicar estilo a las celdas del rango
                    CeldaB6G25.Style.Fill.PatternType = ExcelFillStyle.Solid; // Definir el patrón de relleno
                    CeldaB6G25.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D2E9B1")); // Establecer el color de fondo

                    // Estilos adicionales si es necesario
                    CeldaB6G25.Style.Font.Name = "Calibri";
                    CeldaB6G25.Style.Font.Size = 11;

                    CeldaB6G25.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CeldaB6G25.Style.VerticalAlignment = ExcelVerticalAlignment.Top;


                    // Definir el rango de celdas (de la fila 6 a la 25 en la columna A)
                    var CeldaH6K25 = worksheet.Cells["H6:K25"];

                    // Aplicar estilo a las celdas del rango
                    CeldaH6K25.Style.Fill.PatternType = ExcelFillStyle.Solid; // Definir el patrón de relleno
                    CeldaH6K25.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FDE9D9")); // Establecer el color de fondo

                    // Estilos adicionales si es necesario
                    CeldaH6K25.Style.Font.Name = "Calibri";
                    CeldaH6K25.Style.Font.Size = 11;

                    CeldaH6K25.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CeldaH6K25.Style.VerticalAlignment = ExcelVerticalAlignment.Top;


                    // Establecer la altura de las filas del rango
                    for (int row = 6; row <= 25; row++)
                    {
                        worksheet.Row(row).Height = 240; // Ajusta este valor según la altura deseada
                    }

                    // Logo Ducon  
                    string rutaImagen = @"\\Srvfs\sistemas2\Logo Ducon\Ducon.jpg";
                    FileInfo image = new FileInfo(rutaImagen);
                    if (image.Exists)
                    {
                        var picture = worksheet.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 4, 50);
                        picture.SetSize(180, 80);

                    }

                    // Titulo de encabezado  
                    var CellB2O2 = worksheet.Cells["A2:M2"];
                    CellB2O2.Merge = true;
                    worksheet.Cells["A2"].Value = "FORMATO CONTROL PRODUCTO ESPECIAL (Solicitud N." + lbNumeroSolicitud.Text + ")";
                    CellB2O2.Style.Font.Name = "Calibri";
                    CellB2O2.Style.Font.Size = 16;
                    CellB2O2.Style.Font.Bold = true;
                    CellB2O2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellB2O2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellB2O2.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    // Configurar el color de fondo
                    CellB2O2.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellB2O2.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#A6A6A6"));

                    // Configurar los bordes
                    CellB2O2.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    CellB2O2.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    CellB2O2.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    CellB2O2.Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    // Configurar los colores de los bordes
                    CellB2O2.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    CellB2O2.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    CellB2O2.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    CellB2O2.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    // Aplicar bordes exteriores gruesos
                    worksheet.Cells["A2"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    worksheet.Cells["M2"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    worksheet.Cells["A2:M2"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    worksheet.Cells["A2:M2"].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells["A2:M2"].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells["A2:M2"].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    worksheet.Cells["A2:M2"].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);



                    // Datos Fila 3 

                    // fecha Solicitud                 
                    var CellA3 = worksheet.Cells["A3"];
                    CellA3.Value = "F. Solicitud";
                    CellA3.Style.Font.Name = "Calibri";
                    CellA3.Style.Font.Size = 12;
                    CellA3.Style.Font.Bold = true;
                    CellA3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellA3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellA3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));



                    // fecha Solicitud valor;
                    var CellB3 = worksheet.Cells["B3"];
                    CellB3.Value = tbFechaIngreso.Text;
                    CellB3.Style.Font.Name = "Calibri";
                    CellB3.Style.Font.Size = 11;
                    CellB3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellB3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellB3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Fecha Respuesta
                    string FechaRes = "F.Respuesta";
                    var CellC3 = worksheet.Cells["C3"];
                    CellC3.Value = FechaRes;
                    CellC3.Style.Font.Name = "Calibri";
                    CellC3.Style.Font.Size = 12;
                    CellC3.Style.Font.Bold = true;
                    CellC3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellC3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellC3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    // Asesor
                    string Asesor = "Asesor";
                    var CellD3 = worksheet.Cells["D3"];
                    CellD3.Value = Asesor;
                    CellD3.Style.Font.Name = "Calibri";
                    CellD3.Style.Font.Size = 12;
                    CellD3.Style.Font.Bold = true;
                    CellD3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellD3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellD3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // fecha Asesor Nombre;
                    var CellE3 = worksheet.Cells["E3"];
                    CellE3.Value = ddlAsesor.SelectedItem.Text;
                    CellE3.Style.Font.Name = "Calibri";
                    CellE3.Style.Font.Size = 11;
                    CellE3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellE3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellE3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellE3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Cliente
                    string Cliente = "Cliente";
                    var CellF3 = worksheet.Cells["F3"];
                    CellF3.Value = Cliente;
                    CellF3.Style.Font.Name = "Calibri";
                    CellF3.Style.Font.Size = 12;
                    CellF3.Style.Font.Bold = true;
                    CellF3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellF3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellF3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    // Cliente a Valor;
                    var CellG3H3 = worksheet.Cells["G3:H3"];
                    CellG3H3.Merge = true;
                    worksheet.Cells["G3"].Value = tbCliente.Text;
                    CellG3H3.Style.Font.Name = "Calibri";
                    CellG3H3.Style.Font.Size = 11;
                    CellG3H3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellG3H3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellG3H3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellG3H3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Cliente
                    string Contacto = "Contacto";
                    var CellI3 = worksheet.Cells["i3"];
                    CellI3.Value = Contacto;
                    CellI3.Style.Font.Name = "Calibri";
                    CellI3.Style.Font.Size = 12;
                    CellI3.Style.Font.Bold = true;
                    CellI3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellI3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellI3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // contacto 
                    var CellJ3K3 = worksheet.Cells["J3:K3"];
                    CellJ3K3.Merge = true;
                    worksheet.Cells["J3"].Value = tbContacto.Text;
                    CellJ3K3.Style.Font.Name = "Calibri";
                    CellJ3K3.Style.Font.Size = 11;
                    CellJ3K3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellJ3K3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellJ3K3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellJ3K3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Celular 

                    var CellL3 = worksheet.Cells["L3"];
                    CellL3.Value = "Celular";
                    CellL3.Style.Font.Name = "Calibri";
                    CellL3.Style.Font.Size = 12;
                    CellL3.Style.Font.Bold = true;
                    CellL3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellL3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellL3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // Celular  valor  ;
                    var CellM3 = worksheet.Cells["M3"];
                    CellM3.Value = tbCelular.Text;
                    CellM3.Style.Font.Name = "Calibri";
                    CellM3.Style.Font.Size = 11;
                    CellM3.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellM3.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellM3.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellM3.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Datos fila 4 


                    // fecha Entrega                 
                    var CellA4 = worksheet.Cells["A4"];
                    CellA4.Value = "F. Entrega";
                    CellA4.Style.Font.Name = "Calibri";
                    CellA4.Style.Font.Size = 12;
                    CellA4.Style.Font.Bold = true;
                    CellA4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellA4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellA4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    // fecha Entrega valor;
                    var CellB4 = worksheet.Cells["B4"];
                    CellB4.Value = tbFechaEntrega.Text;
                    CellB4.Style.Font.Name = "Calibri";
                    CellB4.Style.Font.Size = 11;
                    CellB4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellB4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellB4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellB4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // fecha Respuesta valor;
                    var CellC4 = worksheet.Cells["C4"];
                    CellC4.Value = (DateTime.Now).ToString("yyyy-MM/dd");
                    CellC4.Style.Font.Name = "Calibri";
                    CellC4.Style.Font.Size = 11;
                    CellC4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellC4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellC4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellC4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Dirigido a                 
                    var CellD4 = worksheet.Cells["D4"];
                    CellD4.Value = "Dirigido a:";
                    CellD4.Style.Font.Name = "Calibri";
                    CellD4.Style.Font.Size = 12;
                    CellD4.Style.Font.Bold = true;
                    CellD4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellD4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellD4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // Dirigido a Valor;
                    var CellE4 = worksheet.Cells["E4"];
                    CellE4.Value = ddlDirigido.SelectedItem.Text;
                    CellE4.Style.Font.Name = "Calibri";
                    CellE4.Style.Font.Size = 11;
                    CellE4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellE4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellE4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellE4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Proyecto;
                    var CellF4 = worksheet.Cells["F4"];
                    CellF4.Value = "Proyecto";
                    CellF4.Style.Font.Name = "Calibri";
                    CellF4.Style.Font.Size = 12;
                    CellF4.Style.Font.Bold = true;
                    CellF4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellF4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellF4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // Proyecto a Valor;
                    var CellG4H4 = worksheet.Cells["G4:H4"];
                    CellG4H4.Merge = true;
                    worksheet.Cells["G4"].Value = tbProyecto.Text;
                    CellG4H4.Style.Font.Name = "Calibri";
                    CellG4H4.Style.Font.Size = 11;
                    CellG4H4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellG4H4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellG4H4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellG4H4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Mail;
                    var CellI4 = worksheet.Cells["i4"];
                    CellI4.Value = "Mail";
                    CellI4.Style.Font.Name = "Calibri";
                    CellI4.Style.Font.Size = 12;
                    CellI4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Configurar el color de fondo
                    CellI4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellI4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // Mail valor  ;
                    var CellJ4K4 = worksheet.Cells["J4:K4"];
                    CellJ4K4.Merge = true;
                    worksheet.Cells["J4"].Value = tbMail.Text;
                    CellJ4K4.Style.Font.Name = "Calibri";
                    CellJ4K4.Style.Font.Size = 11;
                    CellJ4K4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellJ4K4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellJ4K4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellJ4K4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));


                    // Tipo de solicitud 
                    var CellL4 = worksheet.Cells["L4"];
                    CellL4.Value = "T. solicitud";
                    CellL4.Style.Font.Name = "Calibri";
                    CellL4.Style.Font.Size = 12;
                    CellL4.Style.Font.Bold = true;
                    CellL4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellL4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellL4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // Tipo  solicitud  valor  ;
                    var CellM4 = worksheet.Cells["M4"];
                    CellM4.Value = ddlTipo.SelectedItem.Text;
                    CellM4.Style.Font.Name = "Calibri";
                    CellM4.Style.Font.Size = 11;
                    CellM4.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellM4.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    // Configurar el color de fondo
                    CellM4.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellM4.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D2E9B1"));



                    //Datos Filas 5 

                    // Tipo de ID 
                    var CellA5 = worksheet.Cells["A5"];
                    CellA5.Value = "ID";
                    CellA5.Style.Font.Name = "Calibri";
                    CellA5.Style.Font.Size = 20;
                    CellA5.Style.Font.Bold = true;
                    CellA5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    CellA5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellA5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    // Tipo de Producto 
                    var CellB5 = worksheet.Cells["B5"];
                    CellB5.Value = "Producto";
                    CellB5.Style.Font.Name = "Calibri";
                    CellB5.Style.Font.Size = 11;
                    CellB5.Style.Font.Bold = true;
                    CellB5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellB5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellB5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    // Tipo de Dimensiones 
                    var CellC5 = worksheet.Cells["C5"];
                    CellC5.Value = "Dimensiones";
                    CellC5.Style.Font.Name = "Calibri";
                    CellC5.Style.Font.Size = 11;
                    CellC5.Style.Font.Bold = true;
                    CellC5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellC5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellC5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    // Tipo de Material 
                    var CellD5 = worksheet.Cells["D5"];
                    CellD5.Value = "Material";
                    CellD5.Style.Font.Name = "Calibri";
                    CellD5.Style.Font.Size = 11;
                    CellD5.Style.Font.Bold = true;
                    CellD5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellD5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellD5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // Especificaciones Tecnicas 
                    var CellE5 = worksheet.Cells["E5"];
                    CellE5.Value = "Especificaciones Tecnicas";
                    CellE5.Style.Font.Name = "Calibri";
                    CellE5.Style.Font.Size = 11;
                    CellE5.Style.Font.Bold = true;
                    CellE5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellE5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellE5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    //  Cantidad 
                    var CellF5 = worksheet.Cells["F5"];
                    CellF5.Value = "Cantidad";
                    CellF5.Style.Font.Name = "Calibri";
                    CellF5.Style.Font.Size = 11;
                    CellF5.Style.Font.Bold = true;
                    CellF5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellF5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellF5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));

                    //  Proveedor Sugerido 
                    var CellG5 = worksheet.Cells["G5"];
                    CellG5.Value = "Proveedor sugerido";
                    CellG5.Style.Font.Name = "Calibri";
                    CellG5.Style.Font.Size = 11;
                    CellG5.Style.Font.Bold = true;
                    CellG5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    CellG5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellG5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#76933C"));


                    // Observacion Compras 
                    var CellH5 = worksheet.Cells["H5"];
                    CellH5.Value = "Observación\nCompras";
                    CellH5.Style.Font.Name = "Calibri";
                    CellH5.Style.Font.Size = 11;
                    CellH5.Style.Font.Bold = true;
                    CellH5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellH5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellH5.Style.WrapText = true;
                    CellH5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellH5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC000"));

                    // Observacion desarrollo 
                    var CellI5 = worksheet.Cells["I5"];
                    CellI5.Value = "Observación\nDesarrollo";
                    CellI5.Style.Font.Name = "Calibri";
                    CellI5.Style.Font.Size = 11;
                    CellI5.Style.Font.Bold = true;
                    CellI5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellI5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellI5.Style.WrapText = true;
                    CellI5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellI5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC000"));

                    // Precio sugerido 
                    var CellJ5 = worksheet.Cells["J5"];
                    CellJ5.Value = "Precio Sugerido";
                    CellJ5.Style.Font.Name = "Calibri";
                    CellJ5.Style.Font.Size = 11;
                    CellJ5.Style.Font.Bold = true;
                    CellJ5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellJ5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellJ5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellJ5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC000"));

                    // Subtotal Sugerido 
                    var CellK5 = worksheet.Cells["K5"];
                    CellK5.Value = "Sub Total\nSugerido";
                    CellK5.Style.Font.Name = "Calibri";
                    CellK5.Style.Font.Size = 11;
                    CellK5.Style.Font.Bold = true;
                    CellK5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    CellK5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellK5.Style.WrapText = true;
                    CellK5.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    CellK5.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC000"));

                    // consultamos los detalles para terminar de llenar el exel 

                    DataTable InformacionDetalleSol = ConsultarDetalleSolicitud(lbNumeroSolicitud.Text);

                    // Insertar los datos del DataTable en la hoja de excel
                    int startRow = 6;
                    int currentRow = startRow;

                    foreach (DataRow row in InformacionDetalleSol.Rows)
                    {
                        worksheet.Cells[currentRow, 1].Value = row["ID_Solicitud"];
                        worksheet.Cells[currentRow, 2].Value = row["Producto"];
                        worksheet.Cells[currentRow, 3].Value = row["Ancho"] + "X" + row["Alto"] + "X" + row["Profundidad"];
                        worksheet.Cells[currentRow, 4].Value = row["Material"];
                        worksheet.Cells[currentRow, 5].Value = row["EspecificacionesTecnicas"];
                        worksheet.Cells[currentRow, 6].Value = Convert.ToInt32(row["Cantidad"]);
                        worksheet.Cells[currentRow, 7].Value = row["ProveedorSugerido"];
                        worksheet.Cells[currentRow, 8].Value = row["observacionCompras"];
                        worksheet.Cells[currentRow, 9].Value = row["observacionDesarrollo"];
                        worksheet.Cells[currentRow, 10].Value = row["PrecioSugerido"];
                        worksheet.Cells[currentRow, 11].Formula = $"F{currentRow}*J{currentRow}";

                        // Ajustar el texto para cada celda en la fila actual
                        for (int col = 1; col <= 11; col++) // Ajusta el número de columnas según tus datos
                        {
                            worksheet.Cells[currentRow, col].Style.WrapText = true;
                     
                        }

                        currentRow++;
                    }




                    // Definimos la ruta y el archivo 
                    string networkPath = @"\\SRVDBAPPS\S_I_Ducon$\TemporalAdjunto";
                    string fileName = $"Solicitud_N{ID_Solicitud}.xlsx";
                    string fullPath = Path.Combine(networkPath, fileName);

                    // Guardar el archivo de Excel en la ruta de red
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        excelPackage.SaveAs(fileStream);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar a Excel: " + ex.Message);
            }
        }
        private string ConsultarRutasDocumentosSPE(string ID_Solicitud)
        {
            string RutaBase = @"\\Srvfs\s_i_ducon$\Documentacion PE";
            string CarpetaBase = "PE" + ID_Solicitud;
            string rutas = "";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT Archivo FROM tblDocumentacion WHERE ID_OT LIKE @ID_OT AND TipoDocumento <> 'BOSQUEJO'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_OT", "%" + "PE" + ID_Solicitud + "-%");

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string archivo = reader["Archivo"].ToString();
                        string rutaCompleta = Path.Combine(RutaBase, CarpetaBase, archivo);

                        if (rutas != "")
                        {
                            rutas += ";";
                        }
                        rutas += rutaCompleta;
                    }
                }
            }

            return rutas;
        }
        private string ConsultarCorreoViaticos()
        {
            string mail = "";
            // Realizar la conexión y la consulta a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT mail FROM tblUsosVarios WHERE ObjetivoMail = 'MailCotizarViaticosTransporte'";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Verificar si hay filas devueltas por la consulta
                if (reader.Read())
                {
                    mail = reader["Mail"].ToString();
                }

            }

            return mail;
        }
        public DataTable ConsultarDetalleSolicitud(string ID_Solicitud)
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();
                string sSql = "SELECT * FROM tblSoliciDiseEspeDeta WHERE ID_Solicitud = @Id_Solcitud";
                using (SqlCommand cmdSelect = new SqlCommand(sSql, connectionSID))
                {
                    cmdSelect.Parameters.AddWithValue("@Id_Solcitud", ID_Solicitud);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmdSelect))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }


        //Metodos para Programar la solicitud  Ventas 
        public DateTime CalcularFechaEntrega(DateTime FechaIngreso)
        {
            DateTime UltimaActivacionSolicitud = FechaIngreso;
            DateTime FechaEntrega = DateTime.Now;

            //Se valida  si ingresan la solicitud un dia sabado o domingo 
            while (UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Sunday)
            {
                UltimaActivacionSolicitud = UltimaActivacionSolicitud.AddDays(1);
                UltimaActivacionSolicitud = new DateTime(UltimaActivacionSolicitud.Year, UltimaActivacionSolicitud.Month, UltimaActivacionSolicitud.Day, 8, 0, 0);
            }

            // Coltrol de tres dias para la cotizacion y 5 dias para desarrollos 
            if(ddlTipo.SelectedItem.Text .ToUpper() == "DESARROLLO")
            {
                 FechaEntrega = SumarDiaLaboral(UltimaActivacionSolicitud, 5);
            }
            else if(ddlTipo.SelectedItem.Text.ToUpper() == "COTIZACIÓN")
            {
                 FechaEntrega = SumarDiaLaboral(UltimaActivacionSolicitud, 3);
            }
           

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



        [WebMethod] // Cambiar estado de variable de Session cuando dan click en NuevaSolicitud 
        public static void NuevaSolicitud()
        {
            HttpContext.Current.Session["InsertUpdate"] = "Insertar";
            HttpContext.Current.Session["nuevaSol"] = "1";
        }

        [WebMethod]  // Cambiar estado de variable de Session cuando dan click en Modificarsolicitud 
        public static void ModificarSolicitud()
        {
            HttpContext.Current.Session["InsertUpdate"] = "Actualizar";
            HttpContext.Current.Session["nuevaSol"] = "2";
        }

        [WebMethod] // Cambiar estado de variable de Session cuando dan click en NuevaSolicitud 
        public static void NuevaSolicitud1()
        {

            HttpContext.Current.Session["nuevaSol"] = null;
            HttpContext.Current.Session.Remove("controlBotones");
        }

        [WebMethod]  // Cambiar estado de variable de Session cuando dan click en Modificarsolicitud 
        public static void ModificarSolicitud1()
        {

            HttpContext.Current.Session["nuevaSol"] = null;
            HttpContext.Current.Session.Remove("controlBotones");

        }

        [WebMethod]  // Cambiar estado de variable de Session cuando dan click en Modificarsolicitud 
        public static void Cancelar()
        {

            HttpContext.Current.Session.Remove("nuevaSol");
            HttpContext.Current.Session.Remove("Insertar");
        }

        [WebMethod]  // Cambiar estado de variable de Session cuando dan click en Modificarsolicitud 
        public static void LimpiarSessionError()
        {
            HttpContext.Current.Session.Remove("ProyectoSession");
            HttpContext.Current.Session.Remove("SolicitudOrigen");
            HttpContext.Current.Session.Remove("Cotizacion");
            HttpContext.Current.Session.Remove("Desarrollado");
            HttpContext.Current.Session.Remove("Ciudad");
            HttpContext.Current.Session.Remove("Dirigido");
            HttpContext.Current.Session.Remove("Tipo");
            HttpContext.Current.Session.Remove("AsesorSol");
            HttpContext.Current.Session.Remove("Id_Solicitud_Pantalla");

        }



        // Detalle solicitud
        protected void DataGridDetalleSolicitud_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerDetalleSolicitud")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridDetalleSolicitud.Items[rowIndex];

                // Se utiliza para darle el color solo a la fila seleccionada 
                foreach (DataGridItem item in DataGridDetalleSolicitud.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada";

                string IdDetalle = row.Cells[1].Text;
                string DescripProducto = row.Cells[2].Text;
                string precioSugerido = row.Cells[5].Text;
                string RedirigidoaCompras = row.Cells[6].Text;
                string OkCompras = row.Cells[8].Text;
                string numeroFormateado = string.Format("{0:N0}", double.Parse(precioSugerido));
                string ProveedorVenta = row.Cells[13].Text;
                string Ancho = row.Cells[14].Text;
                string Alto = row.Cells[15].Text;
                string Profundidad = row.Cells[16].Text;
                string Material = row.Cells[17].Text;
                string Cantidad = row.Cells[18].Text;
                string EspGenerales = row.Cells[19].Text;
                string ObsCompra = row.Cells[20].Text;
                string ObsDesarrollo = row.Cells[21].Text;
                string InfoDetOrigen = row.Cells[23].Text;
                string CostoCompras = row.Cells[24].Text;
                string FactorCompra = row.Cells[25].Text;
                string Costo = row.Cells[26].Text;
                string Factor = row.Cells[27].Text;
                string Categoria = row.Cells[28].Text; // Pendiente por establecer en el ddl
                string Prove2 = row.Cells[29].Text;




                // Cargamos el id de Detalle para la documentacion 
                Session["Id_Detalle"] = IdDetalle;

                // Cargamos el id de la Solicitud para la documentacion 
                Session["Id_Solicitud"] = lbNumeroSolicitud.Text;


                lbIdDetalle.Text = IdDetalle;
                tbAncho.Text = Ancho;
                tbAltura.Text = Alto;
                tbProfundidad.Text = Profundidad;

                tbProveedor.Text = ProveedorVenta;
                txDescProduc.InnerText = DescripProducto;
                tbMaterial.Text = Material;
                tbCantidad.Text = Cantidad;
                txEspGen.InnerText = EspGenerales;
                tbPrecioSugerido.Text = numeroFormateado;


                if (ObsCompra == "&nbsp;")
                {
                    string ObsCompraRep = ObsCompra.Replace("&nbsp;", "");
                    txobsCompras.InnerText = ObsCompraRep;
                }
                else
                {
                    txobsCompras.InnerText = ObsCompra;
                }

                if (ObsDesarrollo == "&nbsp;")
                {
                    string ObsDesarrolloRep = ObsDesarrollo.Replace("&nbsp;", "");
                    txObsDesarrollo.InnerText = ObsDesarrolloRep;
                }
                else
                {
                    txObsDesarrollo.InnerText = ObsDesarrollo;
                }
                txInformacionDetalle.InnerText = InfoDetOrigen;
                tbCostoC.Text = CostoCompras;
                tbFactorC.Text = FactorCompra;
                tbCostoD.Text = Costo;
                tbFactorD.Text = Factor;
                tbProve.Text = Prove2.Replace("&nbsp;", "");


                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    if (Session["ProVenSolicitud"].ToString() != "True")
                    {
                        string script = "<script>HabilitarEnlaces2();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces2", script, false);
                    }
                    else
                    {
                        string script = "<script>HabilitarEnlaces3();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces3", script, false);
                    }

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {
                    if (Session["ProDiSolicitud"].ToString() != "True")
                    {
                        if (RedirigidoaCompras.ToUpper() == "NO")
                        {
                            string script = "<script>HabilitarBotDetalleD();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarBotDetalleD", script, false);
                        }
                        else
                        {
                            if (OkCompras.ToUpper() == "SI")
                            {
                                string script = "<script>HabilitarEnlaces3();</script>";
                                ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarBotDetalleD", script, false);
                            }

                        }
                    }
                    else
                    {
                        string script = "<script>HabilitarEnlaces3();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarBotDetalleD", script, false);
                    }
                }


            }
        }
        protected void DataGridDetalleSolicitud_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int ComprasOk = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ComprasOk"));
                int RedirigidoaCompras = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "RedirigidoaCompras"));

                TableCell cell = e.Item.Cells[8];
                cell.Text = (ComprasOk == 1) ? "Si" : "No";

                TableCell cell2 = e.Item.Cells[6]; // Reemplaza "IndiceDeLaColumna" con el índice de la columna que deseas cambiar
                cell2.Text = (RedirigidoaCompras == 1) ? "Si" : "No";
            }
        }


        protected void EliminarDetalle(object sender, EventArgs e)
        {
            if (!ConsultarTerminadoVentas())
            {
                string mensajePersonalizado = "La solicitud ya ha sido programda para ventas y no puede ser modificada.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }

            string consultaActual = "Select * from tblDocumentacion where ID_OT='PE" + lbNumeroSolicitud.Text + "-" + Session["Id_Detalle"].ToString() + "'";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(consultaActual, connection))
                {

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Session["ProductoSession"] = txDescProduc.InnerText;
                        Session["ProveedorVentaSession"] = tbProveedor.Text;
                        Session["AnchoSession"] = tbAncho.Text;
                        Session["AlturaSession"] = tbAltura.Text;
                        Session["ProfundidadSession"] = tbProfundidad.Text;
                        Session["MaterialSession"] = tbMaterial.Text;
                        Session["CantidadSession"] = tbCantidad.Text;
                        Session["EspGeneralSession"] = txEspGen.InnerText;

                        // Variables de Session de la solicitud 
                        Session["FecIngrSolSession"] = tbFechaIngresoServidor.Text;
                        Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
                        Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
                        Session["DirigidoSession"] = ddlDirigido.SelectedItem.Text;
                        Session["TipoSession"] = ddlTipo.SelectedItem.Text;
                        Session["SolOrigenSession"] = tbSolicitudOrigen.Text;
                        Session["ProyectoSolSession"] = tbProyecto.Text;
                        Session["CiudadSession"] = ddlCiudad.Text;
                        Session["ViaticoSession"] = chxViaticos.Checked;
                        Session["CotizacionSession"] = tbCotizacionEsp.Text;
                        Session["ClienteSolSession"] = tbClienteServidor.Text;
                        Session["ContactoSolSession"] = tbContactoServidor.Text;
                        Session["TelSeolSession"] = tbTelefonoServidor.Text;
                        Session["CelularSolSession"] = tbCelularServidor.Text;
                        Session["Mailsolsession"] = tbMailServidor.Text;
                        Session["DirecccionSolSession"] = tbDireccionServidor.Text;
                        Session["AsesorSolSession"] = ddlAsesor.SelectedItem.Text;
                        Session["numeroSolicitudSession"] = lbNumeroSolicitud.Text;

                        string mensajePersonalizado = "Antes de eliminar un detalle, debe eliminar la documentación relacionada";
                        string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                        reader.Close();

                    }
                    else
                    {
                        reader.Close();

                        string consultaEliminacion = "Delete from tblSoliciDiseEspeDeta where Id_SolicitudDetalle = @IdSolicitudDetalle";

                        using (SqlCommand deleteCommand = new SqlCommand(consultaEliminacion, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@IdSolicitudDetalle", Session["Id_Detalle"].ToString());
                            int rowsAffected = deleteCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {


                                // Variables de Session de la solicitud 
                                Session["FecIngrSolSession"] = tbFechaIngresoServidor.Text;
                                Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
                                Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
                                Session["DirigidoSession"] = ddlDirigido.SelectedItem.Text;
                                Session["TipoSession"] = ddlTipo.SelectedItem.Text;
                                Session["SolOrigenSession"] = tbSolicitudOrigen.Text;
                                Session["ProyectoSolSession"] = tbProyecto.Text;
                                Session["CiudadSession"] = ddlCiudad.Text;
                                Session["ViaticoSession"] = chxViaticos.Checked;
                                Session["CotizacionSession"] = tbCotizacionEsp.Text;
                                Session["ClienteSolSession"] = tbClienteServidor.Text;
                                Session["ContactoSolSession"] = tbContactoServidor.Text;
                                Session["TelSeolSession"] = tbTelefonoServidor.Text;
                                Session["CelularSolSession"] = tbCelularServidor.Text;
                                Session["Mailsolsession"] = tbMailServidor.Text;
                                Session["DirecccionSolSession"] = tbDireccionServidor.Text;
                                Session["AsesorSolSession"] = ddlAsesor.SelectedItem.Text;
                                Session["numeroSolicitudSession"] = lbNumeroSolicitud.Text;

                                Session.Remove("Id_Detalle");

                                string mensajePersonalizado = "El detalle " + lbIdDetalle.Text + " se eliminó con éxito,";
                                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                            }
                        }


                    }
                }


            }


        }

        protected void GuardarModificarDetalle(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
            {

                if (Session["InsertUpdateDetalle"]?.ToString() == "Insertar")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand getMaxIdCmd = new SqlCommand("SELECT Max(tblSoliciDiseEspeDeta.Id_SolicitudDetalle) FROM tblSoliciDiseEspeDeta", connection))
                        {
                            object maxIdObj = getMaxIdCmd.ExecuteScalar();
                            int maxId = (maxIdObj != null && maxIdObj != DBNull.Value) ? Convert.ToInt32(maxIdObj) : 0;

                            int nuevoId = maxId + 1;

                            string IdSolicitud = nuevoId.ToString();
                            connection.Close();
                            using (SqlCommand cmd = new SqlCommand("Sp_InsertarDetalleSolicitud", connection))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@Id_SolicitudDetalle", IdSolicitud);
                                cmd.Parameters.AddWithValue("@ID_Solicitud", lbNumeroSolicitud.Text);
                                cmd.Parameters.AddWithValue("@Producto", txDescProduc.InnerText);
                                cmd.Parameters.AddWithValue("@ProveedorSugerido", tbProveedor.Text);

                                cmd.Parameters.AddWithValue("@Ancho", tbAncho.Text);
                                cmd.Parameters.AddWithValue("@Alto", tbAltura.Text);

                                cmd.Parameters.AddWithValue("@Profundidad", tbProfundidad.Text);
                                cmd.Parameters.AddWithValue("@Material", tbMaterial.Text);
                                cmd.Parameters.AddWithValue("@EspecificacionesTecnicas", txEspGen.InnerText);
                                cmd.Parameters.AddWithValue("@Cantidad", tbCantidad.Text);


                                connection.Open();


                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    // Variables de session de Detalle 
                                    Session["ProductoSession"] = txDescProduc.InnerText;
                                    Session["ProveedorVentaSession"] = tbProveedor.Text;
                                    Session["AnchoSession"] = tbAncho.Text;
                                    Session["AlturaSession"] = tbAltura.Text;
                                    Session["ProfundidadSession"] = tbProfundidad.Text;
                                    Session["MaterialSession"] = tbMaterial.Text;
                                    Session["CantidadSession"] = tbCantidad.Text;
                                    Session["EspGeneralSession"] = txEspGen.InnerText;

                                    // Variables de Session de la solicitud 
                                    Session["FecIngrSolSession"] = tbFechaIngresoServidor.Text;
                                    Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
                                    Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
                                    Session["DirigidoSession"] = ddlDirigido.SelectedItem.Text;
                                    Session["TipoSession"] = ddlTipo.SelectedItem.Text;
                                    Session["SolOrigenSession"] = tbSolicitudOrigen.Text;
                                    Session["ProyectoSolSession"] = tbProyecto.Text;
                                    Session["CiudadSession"] = ddlCiudad.SelectedItem.Text;
                                    Session["ViaticoSession"] = chxViaticos.Checked;
                                    Session["CotizacionSession"] = tbCotizacionEsp.Text;
                                    Session["ClienteSolSession"] = tbClienteServidor.Text;
                                    Session["ContactoSolSession"] = tbContactoServidor.Text;
                                    Session["TelSeolSession"] = tbTelefonoServidor.Text;
                                    Session["CelularSolSession"] = tbCelularServidor.Text;
                                    Session["Mailsolsession"] = tbMailServidor.Text;
                                    Session["DirecccionSolSession"] = tbDireccionServidor.Text;
                                    Session["AsesorSolSession"] = ddlAsesor.SelectedItem.Text;
                                    Session["numeroSolicitudSession"] = lbNumeroSolicitud.Text; ;

                                    Session["ScriptEspecifico"] = "ActivarBotonDetalle1();";


                                    string mensajePersonalizado = "El detalle  ha sido ingresado con éxito";
                                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }
                                else
                                {
                                    string mensajePersonalizado = "¡Ups! El detalle no se ingresó correctamente.Por favor comuniquese con el Departamento de Sistemas para obtener ayuda";
                                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                                }




                            }
                        }
                    }
                }

                else if (Session["InsertUpdateDetalle"]?.ToString() == "Actualizar")
                {

                    //CalcularFechaEntregaSolicitudEspecial() de momento se envia fecha del primer dia del año  !!!!IMPORTANTE !!!!

                    if (!ConsultarTerminadoVentas())
                    {
                        string mensajePersonalizado = "La solicitud ya ha sido programda para ventas y no puede ser modificada.";
                        string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                    }

                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        using (SqlCommand cmd = new SqlCommand("Sp_ActualizarDetalleSolicitud", connection))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id_SolicitudDetalle", Session["Id_Detalle"].ToString());
                            cmd.Parameters.AddWithValue("@ID_Solicitud", lbNumeroSolicitud.Text);
                            cmd.Parameters.AddWithValue("@Producto", txDescProduc.InnerText);
                            cmd.Parameters.AddWithValue("@ProveedorSugerido", tbProveedor.Text);

                            cmd.Parameters.AddWithValue("@Ancho", tbAncho.Text);
                            cmd.Parameters.AddWithValue("@Alto", tbAltura.Text);

                            cmd.Parameters.AddWithValue("@Profundidad", tbProfundidad.Text);
                            cmd.Parameters.AddWithValue("@Material", tbMaterial.Text);
                            cmd.Parameters.AddWithValue("@EspecificacionesTecnicas", txEspGen.InnerText);
                            cmd.Parameters.AddWithValue("@Cantidad", tbCantidad.Text);



                            connection.Open();


                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                // Variables de session de Detalle 

                                Session["ProductoSession"] = txDescProduc.InnerText;
                                Session["ProveedorVentaSession"] = tbProveedor.Text;
                                Session["AnchoSession"] = tbAncho.Text;
                                Session["AlturaSession"] = tbAltura.Text;
                                Session["ProfundidadSession"] = tbProfundidad.Text;
                                Session["MaterialSession"] = tbMaterial.Text;
                                Session["CantidadSession"] = tbCantidad.Text;
                                Session["EspGeneralSession"] = txEspGen.InnerText;

                                // Variables de Session de la solicitud 
                                Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
                                Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
                                Session["DirigidoSession"] = ddlDirigido.SelectedItem.Text;
                                Session["TipoSession"] = ddlTipo.SelectedItem.Text;
                                Session["SolOrigenSession"] = tbSolicitudOrigen.Text;
                                Session["ProyectoSolSession"] = tbProyecto.Text;
                                Session["CiudadSession"] = ddlCiudad.Text;
                                Session["ViaticoSession"] = chxViaticos.Checked;
                                Session["CotizacionSession"] = tbCotizacionEsp.Text;
                                Session["ClienteSolSession"] = tbClienteServidor.Text;
                                Session["ContactoSolSession"] = tbContactoServidor.Text;
                                Session["TelSeolSession"] = tbTelefonoServidor.Text;
                                Session["CelularSolSession"] = tbCelularServidor.Text;
                                Session["Mailsolsession"] = tbMailServidor.Text;
                                Session["DirecccionSolSession"] = tbDireccionServidor.Text;
                                Session["AsesorSolSession"] = ddlAsesor.SelectedItem.Text;
                                Session["numeroSolicitudSession"] = lbNumeroSolicitud.Text;

                                Session["ScriptEspecifico"] = "ActivarBotonDetalle1();";


                                string mensajePersonalizado = "¡El detalle  ha sido actualizado con exito!";
                                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }
                            else
                            {
                                string mensajePersonalizado = "¡Ups! El detalle no se Actualizó correctamente.por favor, comuniquese con el Departamento de Sistemas para obtener ayuda";
                                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }




                        }
                    }

                }
                else
                {
                    string mensajePersonalizado = "La solicitud ya ha sido programda para ventas y no puede ser modificada.";
                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }


            }
            else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
            {
                if (ActualizarDetalleSolicitudEspecial())
                {

                    // control del tap del dibujante
                    Session["ActivarTapBita"] = "1";

                    DataTable DatosSol = CosultarDatosSol();

                    // Accede a la primera fila del DataTable
                    DataRow row = DatosSol.Rows[0];

                    // Obtén los valores de las columnas
                    bool terminado = Convert.ToBoolean(row["Terminado"]);
                    bool programadoVentas = Convert.ToBoolean(row["ProgramadoVentas"]);
                    bool pausado = Convert.ToBoolean(row["Pausado"]);

                    // VARIABLES EVALUACION Y CONTROL DIBUJANTE 

                    Session["terDis"] = terminado;
                    Session["terVen"] = programadoVentas;
                    Session["pausado"] = pausado;


                    //Mensaje Exito             
                    string mensajePersonalizado = "El detalle ha sido actualizado con exito.";
                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {
                    string mensajePersonalizado = "Ocurrió un error, por favor intenta nuevamente, ";
                    string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }


        }

        private bool ActualizarDetalleSolicitudEspecial()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblSoliciDiseEspeDeta SET observacionDesarrollo = @ObsDibujo, Costo = @costo, Factor = @factor," +
                          " PrecioSugerido = @precioSugerido, Categoria = 'PENDIENTE' WHERE Id_SolicitudDetalle = @Id_Detalle";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ObsDibujo", txObsDesarrollo.InnerText);
                    cmd.Parameters.AddWithValue("@costo", tbCostoD.Text);
                    cmd.Parameters.AddWithValue("@factor", tbFactorD.Text); // Aquí faltaba ".Text"
                    cmd.Parameters.AddWithValue("@precioSugerido",Convert.ToInt32(tbPrecioSugerido.Text.Replace(",","")));
                    cmd.Parameters.AddWithValue("@Id_Detalle", lbIdDetalle.Text);

                    int filaAfectada = cmd.ExecuteNonQuery();

                    if (filaAfectada > 0)
                    {
                        // Variables de session de Detalle 
                        Session["ProductoSession"] = txDescProduc.InnerText;
                        Session["ProveedorVentaSession"] = tbProveedor.Text;
                        Session["AnchoSession"] = tbAncho.Text;
                        Session["AlturaSession"] = tbAltura.Text;
                        Session["ProfundidadSession"] = tbProfundidad.Text;
                        Session["MaterialSession"] = tbMaterial.Text;
                        Session["CantidadSession"] = tbCantidad.Text;
                        Session["EspGeneralSession"] = txEspGen.InnerText;

                        // Variables de Session de la solicitud 
                        Session["FecIngrSolSession"] = tbFechaIngresoServidor.Text;
                        Session["FecEntregaSolSession"] = tbFechaEntregaServidor.Text;
                        Session["FechaRespuestaSession"] = tbFechaRespuestaServidor.Text;
                        Session["DirigidoSession"] = ddlDirigido.SelectedItem.Text;
                        Session["TipoSession"] = ddlTipo.SelectedItem.Text;
                        Session["SolOrigenSession"] = tbSolicitudOrigen.Text;
                        Session["ProyectoSolSession"] = tbProyecto.Text;
                        Session["CiudadSession"] = ddlCiudad.SelectedItem.Text;
                        Session["ViaticoSession"] = chxViaticos.Checked;
                        Session["CotizacionSession"] = tbCotizacionEsp.Text;
                        Session["ClienteSolSession"] = tbClienteServidor.Text;
                        Session["ContactoSolSession"] = tbContactoServidor.Text;
                        Session["TelSeolSession"] = tbTelefonoServidor.Text;
                        Session["CelularSolSession"] = tbCelularServidor.Text;
                        Session["Mailsolsession"] = tbMailServidor.Text;
                        Session["DirecccionSolSession"] = tbDireccionServidor.Text;
                        Session["AsesorSolSession"] = ddlAsesor.SelectedItem.Text;
                        Session["numeroSolicitudSession"] = lbNumeroSolicitud.Text; ;

                    }

                    return filaAfectada > 0;

                }
            }
        }

        public DataTable CosultarDatosSol()
        {
            DataTable DetallesOrigen = new DataTable();

            string query = "select Terminado, ProgramadoVentas,Pausado from tblSoliciDiseEspe where ID_Solicitud = @ID_Sol";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {

                    adapter.SelectCommand.Parameters.AddWithValue("@ID_Sol", lbNumeroSolicitud.Text);
                    adapter.Fill(DetallesOrigen);
                }
            }

            return DetallesOrigen;
        }


        [WebMethod]
        public static void NuevoDetalle()
        {
            HttpContext.Current.Session["InsertUpdateDetalle"] = "Insertar";
        }

        [WebMethod]
        public static void ModificarDetalle()
        {
            HttpContext.Current.Session["InsertUpdateDetalle"] = "Actualizar";
        }

        [WebMethod]
        public static void EliminarTapAct()
        {
            HttpContext.Current.Session["ActivarTapBita"] = null;
            HttpContext.Current.Session["terDis"] = null;
            HttpContext.Current.Session["terVen"] = null;
            HttpContext.Current.Session["pausado"] = null;
        }

        [WebMethod]
        public static void LimpiarVaribleSessiondetalle()
        {
            HttpContext.Current.Session["ScriptEspecifico"] = null;
        }

        public bool ConsultarTerminadoVentas()
        {
            string consultaActual = "select ProgramadoVentas from tblSoliciDiseEspe where ID_Solicitud = @solicitud";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(consultaActual, connection))
                {
                    cmd.Parameters.AddWithValue("@solicitud", lbNumeroSolicitud.Text);
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

        protected void ImportarDetalle_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirmarImportar').modal('show');", true);
        }

        protected void btnImportar_Si_Click(object sender, EventArgs e)
        {

            DataTable DetalleImportado = ConsultarInfoDetalle();


            if (DetalleImportado.Rows.Count > 0)
            {
                foreach (DataRow row in DetalleImportado.Rows)
                {
                    string Id_Detalle = row["ID_SolicitudDetalle"].ToString();

                    if (!ValidarDetalle(Id_Detalle))
                    {
                        // Se consulta el consecutivo de detalle 
                        int Id_DetalleNuevo = ConsultarConsecutivoDetalle();


                        // Se crea un nuevo DataTable que contenga solo la fila actual
                        DataTable DetalleFilaActual = DetalleImportado.Clone();
                        DetalleFilaActual.ImportRow(row);

                        // Se realiza la Inserción solo con la fila actual
                        InsertarDetalle(DetalleFilaActual, Id_DetalleNuevo + 1);

                        DataGridDetalleSolicitud.DataBind();

                    }

                }

                string scriptAgregado = "alert('Detalles Agregados.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showAgregado", scriptAgregado, true);

                string script = "<script>ControlHeaderCard();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, false);

            }
            else
            {
                // La solcitud de origen no tiene detalle para importar  (mensaje)
                string scriptEncontrado = "alert('La solicitud de origen no tiene detalles para importar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showEncontrado", scriptEncontrado, true);

                string script = "<script>ControlHeaderCard();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, false);

            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //string script = "<script>ControlBtnCliente();</script>";
            //ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, false);

            Session["CargarSolicitud"] = "1";
            Session["CargarSolicitud_ID"] = lbNumeroSolicitud.Text;
            Response.Redirect("Solicitud_Especial.aspx");

        }

        public DataTable ConsultarInfoDetalle()
        {
            DataTable DetallesOrigen = new DataTable();

            string query = $"Select *, CONCAT('Producto: ',producto,', ancho: ',ancho,', Alto: ',alto,'," +
                           $" Profundidad: ',profundidad,', Material: ',material,', Especificaciones: '," +
                           $"EspecificacionesTecnicas) as DetalleOrigen from tblSoliciDiseEspeDeta where ID_Solicitud= @IdDetalleOrigen";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {

                    adapter.SelectCommand.Parameters.AddWithValue("@IdDetalleOrigen", tbSolicitudOrigen.Text);
                    adapter.Fill(DetallesOrigen);
                }
            }

            return DetallesOrigen;
        }

        private bool ValidarDetalle(string IdDetalle)
        {

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblSoliciDiseEspeDeta WHERE ID_Solicitud= @Solicitud and Id_SolicitudDetalleOrigen= @Id_Detalle ";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@Solicitud", lbNumeroSolicitud.Text);
                    cmd.Parameters.AddWithValue("@Id_Detalle", IdDetalle);
                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado es null o no
                    if (result != null)
                    {
                        int rowCount = Convert.ToInt32(result);
                        // Si rowCount es mayor que cero se retorne true 
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

        private int ConsultarConsecutivoDetalle()
        {
            int numDetalle = 0; // Se inicializa como 0 en caso de que no haya resultados en la consulta

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Max(tblSoliciDiseEspeDeta.Id_SolicitudDetalle) FROM tblSoliciDiseEspeDeta";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        numDetalle = Convert.ToInt32(result);
                    }
                }
            }

            return numDetalle;
        }

        private void InsertarDetalle(DataTable DatosDetalle, int ID_DetalleNuevo)
        {
            if (DatosDetalle.Rows.Count > 0)
            {
                string Producto = DatosDetalle.Rows[0]["Producto"].ToString();
                string ProveedorSugerido = DatosDetalle.Rows[0]["ProveedorSugerido"].ToString();
                string Ancho = DatosDetalle.Rows[0]["Ancho"].ToString();
                string Alto = DatosDetalle.Rows[0]["Alto"].ToString();
                string Profundidad = DatosDetalle.Rows[0]["Profundidad"].ToString();
                string Material = DatosDetalle.Rows[0]["Material"].ToString();
                string EspecificacionesTecnicas = DatosDetalle.Rows[0]["EspecificacionesTecnicas"].ToString();
                string Cantidad = DatosDetalle.Rows[0]["Cantidad"].ToString();
                string observacionDesarrollo = DatosDetalle.Rows[0]["observacionDesarrollo"].ToString();
                string Proveedor = DatosDetalle.Rows[0]["Proveedor"].ToString();
                string PrecioSugerido = DatosDetalle.Rows[0]["PrecioSugerido"].ToString();
                string observacionCompras = DatosDetalle.Rows[0]["observacionCompras"].ToString();
                string Costo = DatosDetalle.Rows[0]["Costo"].ToString();
                string Factor = DatosDetalle.Rows[0]["Factor"].ToString();
                string Detalle = DatosDetalle.Rows[0]["Detalle"].ToString();
                string RedirigidoaCompras = DatosDetalle.Rows[0]["RedirigidoaCompras"].ToString();
                string Redirigidoel = DatosDetalle.Rows[0]["Redirigidoel"].ToString();
                string ComprasOk = DatosDetalle.Rows[0]["ComprasOk"].ToString();
                string FechaComprasOk = DatosDetalle.Rows[0]["FechaComprasOk"].ToString();
                string CostoCompras = DatosDetalle.Rows[0]["CostoCompras"].ToString();
                string FactorCompras = DatosDetalle.Rows[0]["FactorCompras"].ToString();
                string Categoria = DatosDetalle.Rows[0]["Categoria"].ToString();
                string RealizadoPor = DatosDetalle.Rows[0]["RealizadoPor"].ToString();
                string Id_SolicitudDetalle = DatosDetalle.Rows[0]["ID_SolicitudDetalle"].ToString();
                string DetalleOrigen = DatosDetalle.Rows[0]["DetalleOrigen"].ToString();




                string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connectionISID = new SqlConnection(connectionStringSID))
                {
                    connectionISID.Open();

                    string sSql = "INSERT INTO tblSoliciDiseEspeDeta (Id_SolicitudDetalle,ID_Solicitud,Producto,ProveedorSugerido,Ancho,Alto,Profundidad," +
                                  "Material,EspecificacionesTecnicas,Cantidad,observacionDesarrollo,Proveedor,PrecioSugerido,observacionCompras,Costo,Factor," +
                                  "Detalle,RedirigidoaCompras,Redirigidoel,ComprasOk,FechaComprasOk,CostoCompras,FactorCompras,Categoria,RealizadoPor," +
                                  " Id_SolicitudDetalleOrigen,ID_SolicitudOrigen,informacionDetalleOrigen) " +
                                  "VALUES (@Id_SolicitudDetalle, @ID_Solicitud, @Producto, @ProveedorSugerido, @Ancho, @Alto, @Profundidad, @Material, " +
                                  "@EspecificacionesTecnicas, @Cantidad, @observacionDesarrollo, @Proveedor, @PrecioSugerido, @observacionCompras, @Costo, @Factor," +
                                  "@Detalle, @RedirigidoaCompras, @Redirigidoel, @ComprasOk, @FechaComprasOk, @CostoCompras, @FactorCompras, @Categoria,  " +
                                  "@RealizadoPor, @Id_SolicitudDetalleOrigen, @SolicitudOrigen, @DetalleOrigen)";


                    using (SqlCommand cmdInsert = new SqlCommand(sSql, connectionISID))
                    {
                        cmdInsert.Parameters.AddWithValue("@Id_SolicitudDetalle", ID_DetalleNuevo);
                        cmdInsert.Parameters.AddWithValue("@Id_Solicitud", lbNumeroSolicitud.Text);
                        cmdInsert.Parameters.AddWithValue("@Producto", Producto);
                        cmdInsert.Parameters.AddWithValue("@ProveedorSugerido", ProveedorSugerido);
                        cmdInsert.Parameters.AddWithValue("@Ancho", Ancho);
                        cmdInsert.Parameters.AddWithValue("@Alto", Alto);
                        cmdInsert.Parameters.AddWithValue("@Profundidad", Profundidad);
                        cmdInsert.Parameters.AddWithValue("@Material", Material);
                        cmdInsert.Parameters.AddWithValue("@EspecificacionesTecnicas", EspecificacionesTecnicas);
                        cmdInsert.Parameters.AddWithValue("@Cantidad", Cantidad);
                        cmdInsert.Parameters.AddWithValue("@observacionDesarrollo", observacionDesarrollo);
                        cmdInsert.Parameters.AddWithValue("@Proveedor", Proveedor);
                        cmdInsert.Parameters.AddWithValue("@PrecioSugerido", PrecioSugerido);
                        cmdInsert.Parameters.AddWithValue("@observacionCompras", observacionCompras);
                        cmdInsert.Parameters.AddWithValue("@Costo", Costo);
                        cmdInsert.Parameters.AddWithValue("@Factor", Factor);
                        cmdInsert.Parameters.AddWithValue("@Detalle", Detalle);
                        cmdInsert.Parameters.AddWithValue("@RedirigidoaCompras", RedirigidoaCompras != null ? (bool.TryParse(RedirigidoaCompras.ToString(), out var parsedValue) ? (object)parsedValue : false) : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("@Redirigidoel", string.IsNullOrEmpty(Redirigidoel) ? (object)DBNull.Value : Convert.ToDateTime(Redirigidoel));
                        cmdInsert.Parameters.AddWithValue("@ComprasOk", ComprasOk != null ? (bool.TryParse(ComprasOk.ToString(), out var parsedValue1) ? (object)parsedValue1 : false) : DBNull.Value);
                        cmdInsert.Parameters.AddWithValue("@FechaComprasOk", string.IsNullOrEmpty(FechaComprasOk) ? (object)DBNull.Value : Convert.ToDateTime(FechaComprasOk));
                        cmdInsert.Parameters.AddWithValue("@CostoCompras", CostoCompras);
                        cmdInsert.Parameters.AddWithValue("@FactorCompras", FactorCompras);
                        cmdInsert.Parameters.AddWithValue("@Categoria", Categoria);
                        cmdInsert.Parameters.AddWithValue("@RealizadoPor", RealizadoPor);
                        cmdInsert.Parameters.AddWithValue("@Id_SolicitudDetalleOrigen", Id_SolicitudDetalle);
                        cmdInsert.Parameters.AddWithValue("@SolicitudOrigen", tbSolicitudOrigen.Text);
                        cmdInsert.Parameters.AddWithValue("@DetalleOrigen", DetalleOrigen);


                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }


        [WebMethod] // Metdodo estatico para cambiar las variables de session para cargar observaciones 
        public static void ObservacionesRedirect(string solicitud, string cliente, string proyecto)
        {

            HttpContext.Current.Session["Id_OT"] = "SPE" + solicitud;
            HttpContext.Current.Session["pedido"] = "0";
            HttpContext.Current.Session["ValorDeObra"] = cliente + " " + proyecto;
        }



        // METODOS PARA EL ROL DESARROOLLO DE PRODUCTO Y DISEÑO 
        public bool ConsultarTerminadoDiseño()
        {
            string consultaActual = "select Terminado from tblSoliciDiseEspe where ID_Solicitud = @solicitud";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(consultaActual, connection))
                {
                    cmd.Parameters.AddWithValue("@solicitud", lbNumeroSolicitud.Text);
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
        private void CargarDesarrollos_Cotizaciones()
        {
            // Cargar Desarrollos 

            CargarDesarrollos.SelectCommand = " SELECT * FROM tblSoliciDiseEspe  " +
                                                           " WHERE Terminado = 0 AND Dirigidoa='DESARROLLO DE PRODUCTO'AND  ProgramadoVentas = 1  " +
                                                           " AND TipoSolicitud ='DESARROLLO' ORDER BY Fecha_Ingreso ASC ";



            DataGrid1.DataSourceID = "CargarDesarrollos";
            DataGrid1.DataBind();

            // Cargar Cotizaciones 

            CargarCotizaciones.SelectCommand = "SELECT * FROM tblSoliciDiseEspe " +
                                               "WHERE Terminado = 0 AND TipoSolicitud ='COTIZACIÓN' AND Dirigidoa = 'DESARROLLO DE PRODUCTO' " +
                                               "AND ProgramadoVentas = 1 ORDER BY Fecha_Ingreso ASC;";



            DataGrid2.DataSourceID = "CargarCotizaciones";
            DataGrid2.DataBind();



        }
        private void CargarDesarrollos_Metodo()
        {
            // Cargar Desarrollos 

            CargarDesarrollos.SelectCommand = " SELECT * FROM tblSoliciDiseEspe  " +
                                                           " WHERE Terminado = 0 AND Dirigidoa='DESARROLLO DE PRODUCTO'AND  ProgramadoVentas = 1  " +
                                                           " AND TipoSolicitud ='DESARROLLO' ORDER BY Fecha_Ingreso ASC ";



            DataGrid1.DataSourceID = "CargarDesarrollos";
            DataGrid1.DataBind();


        }
        private void CargarCotizaciones_Metodo()
        {

            // Cargar Cotizaciones 

            CargarCotizaciones.SelectCommand = "SELECT * FROM tblSoliciDiseEspe " +
                                               "WHERE Terminado = 0 AND TipoSolicitud ='COTIZACIÓN' AND Dirigidoa = 'DESARROLLO DE PRODUCTO' " +
                                               "AND ProgramadoVentas = 1 ORDER BY Fecha_Ingreso ASC;";



            DataGrid2.DataSourceID = "CargarCotizaciones";
            DataGrid2.DataBind();



        }


        // Asignar Dibujante en Solcitud Especial (Desarrollo)
        protected void btnTrabajarSolicitud_Click(object sender, EventArgs e)
        {
            if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "DESARROLLO")
            {
                NumSol.InnerText = lbNumeroSolicitud.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirTrabaSol').modal('show');", true);
            }
            else
            {
                string script = @"ControlBtnCliente();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);


                string mensajeExito = "Por favor, seleccione una solicitud de  Desarrollo ";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }

        }
        protected void btnTraSol_Si_Click(object sender, EventArgs e)
        {
            ProgramarDibujanteDesarrollo(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarDesarrollos_Metodo();



            int rowIndex = Convert.ToInt32(tbId_Fila.Text);
            string script = $"SeleccionarFilayEnfocarDesarrollo({rowIndex});";
            ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarDesarrollo", script, true);


        }
        private void ProgramarDibujanteDesarrollo(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblSoliciDiseEspe set RealizadoPor = @NombreUsuario where id_Solicitud= @ID";

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


        // Desprogramar Dibujante en Solcitud Especial (Desarrollo)
        protected void btnDesprogramar_Click(object sender, EventArgs e)
        {

            if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "DESARROLLO")
            {
                NumSol1.InnerText = lbNumeroSolicitud.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirDespSol').modal('show');", true);
            }
            else
            {
                string script = @"ControlBtnCliente();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);


                string mensajeExito = "Por favor, seleccione una solicitud de Desarrollo ";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }



        }
        protected void btnDesprogramar_SI_Click(object sender, EventArgs e)
        {
            DesprogramarDibujanteDesarrollo(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarDesarrollos_Metodo();

            int rowIndex = Convert.ToInt32(tbId_Fila.Text);
            string script = $"SeleccionarFilayEnfocarDesarrollo({rowIndex});";
            ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarDesarrollo", script, true);


        }
        private void DesprogramarDibujanteDesarrollo(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblSoliciDiseEspe set RealizadoPor = 'PENDIENTE' where id_Solicitud= @ID";

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


        // Asignar Dibujante en Solcitud Especial (Cotizacion)
        protected void btnTrbajarCotizacion_Click(object sender, EventArgs e)
        {

            if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "COTIZACIÓN")
            {
                NumSol2.InnerText = lbNumeroSolicitud.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirTrabaCot').modal('show');", true);
            }
            else
            {
                string script = @"ControlBtnCliente();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);


                string mensajeExito = "Por favor, seleccione una solicitud de  Cotización";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }


        }
        protected void btnProCot_SI_Click(object sender, EventArgs e)
        {
            ProgramarDibujanteCotizacion(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarCotizaciones_Metodo();

            int rowIndex = Convert.ToInt32(tbId_Fila.Text);
            string script = $"SeleccionarFilayEnfocarCotizacion({rowIndex});";
            ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarDesarrollo", script, true);
        }
        private void ProgramarDibujanteCotizacion(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblSoliciDiseEspe set RealizadoPor = @NombreUsuario where id_Solicitud= @ID";

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


        // Desprogramar Dibujante en Solcitud Especial (Cotizacion)
        protected void btnDesprogramar1_Click(object sender, EventArgs e)
        {
            if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "COTIZACIÓN")
            {
                NumSol3.InnerText = lbNumeroSolicitud.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirDespCot').modal('show');", true);
            }
            else
            {

                string script = @"ControlBtnCliente();";
                ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);


                string mensajeExito = "Por favor, seleccione una solicitud de  Cotización  ";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }

        }
        protected void btnDesCot_SI_Click(object sender, EventArgs e)
        {
            DesprogramarDibujanteCotizacion(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarCotizaciones_Metodo();

            int rowIndex = Convert.ToInt32(tbId_Fila.Text);
            string script = $"SeleccionarFilayEnfocarCotizacion({rowIndex});";
            ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarDesarrollo", script, true);
        }
        private void DesprogramarDibujanteCotizacion(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblSoliciDiseEspe set RealizadoPor = 'PENDIENTE' where id_Solicitud= @ID";

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


        // Buscar Desarrollo 
        protected void ID_Sol_Dib_TextChanged(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                ContadorClic.Text = "";

                if (ID_Sol_Dib.Text == "")
                {
                    CargarDesarrollos_Cotizaciones();
                }
                else
                {
                    BuscarSol_Click(sender, e);
                }
            }

        }
        protected void BuscarSol_Click(object sender, EventArgs e)
        {

            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                // Se limpia contador de Click
                ContadorClic.Text = "";
                ControlBotonesDiseño();


                if (ID_Sol_Dib.Text != "")
                {
                    if (ValidarExisteSolicitud(ID_Sol_Dib.Text))
                    {
                        DataGrid1.DataSourceID = "SolUnica";
                        DataGrid1.DataBind();

                        string script = @"ControlBtnCliente();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);
                    }
                    else
                    {
                        string mensajeExito = "El desarrollo buscado no existe";
                        string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                        string script = @"ControlBtnCliente();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);

                    }
                }
                else
                {
                    string mensajeExito = "Por favor, digite una solicitud  o seleccione una de la tabla de desarrollo.";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                    CargarDesarrollos_Cotizaciones();

                    string script = @"ControlBtnCliente();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);
                }
            }

        }


        // Buscar Cotizacion  
        protected void ID_Cot_Dib_TextChanged(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                if (ID_Cot_Dib.Text == "")
                {
                    CargarDesarrollos_Cotizaciones();
                    string script = @"ControlBtnCliente();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);
                }
                else
                {
                    BuscarCot_Click(sender, e);
                }
            }

        }
        protected void BuscarCot_Click(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                // Se limpiar contador Click
                ContadorClic.Text = "";
                ControlBotonesDiseño();

                if (ID_Cot_Dib.Text != "")
                {
                    if (ValidarExisteSolicitud(ID_Cot_Dib.Text))
                    {
                        DataGrid2.DataSourceID = "CotUnica";
                        DataGrid2.DataBind();

                        string script = @"ControlBtnCliente();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);
                    }
                    else
                    {
                        string mensajeExito = "La cotización buscada no existe";
                        string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                        string script = @"ControlBtnCliente();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);
                    }
                }
                else
                {
                    string mensajeExito = "Por favor, digite una solicitud  o seleccione una de la tabla de cotizaciones.";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                    CargarDesarrollos_Cotizaciones();

                    string script = @"ControlBtnCliente();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);
                }
            }

        }


        // Validar Existencia de Solcitud 
        private bool ValidarExisteSolicitud(string ID_Sol)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT * FROM tblSoliciDiseEspe WHERE  ID_Solicitud = @ID";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@ID", ID_Sol);
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


        // Confirmar Solicitud Especial Complejo 
        protected void ConfirmarComplejo_Click(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                SpanId_sol.InnerText = lbNumeroSolicitud.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirDesCompl').modal('show');", true);
            }
            else
            {
                string mensajePersonalizado = "No cuentas con los permisos necesarios para cambiar una solicitud a desarrolo complejo.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }

        }
        protected void btnConfComlplejo_SI_Click(object sender, EventArgs e)
        {
            bool chkComplejo = chxDesComplejo.Checked;
            string IdSolicitud = lbNumeroSolicitud.Text;

            if (chkComplejo)
            {
                bool ComplejoDB = ConsultarComplejo(IdSolicitud);
                if (ComplejoDB == chkComplejo)
                {
                    string mensajeExito = "La solicitud ya está marcada como Desarrollo Complejo.";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }
                else
                {

                    ActivarComplejo(IdSolicitud, chkComplejo);
                    string mensajeExito = "Desarrollo Complejo Activado vence en 20 dias";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                }

            }
            else
            {
                bool ComplejoDB = ConsultarComplejo(IdSolicitud);

                if (ComplejoDB == chkComplejo)
                {
                  
                    string mensajeExito = "La solicitud ya está marcada como No Desarrollo Complejo.";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }
                else
                {
                    DesactivarComplejo(IdSolicitud, chkComplejo);
                    string mensajeExito = "Desarrollo Complejo Desactivado";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }

            }


            CargarDesarrollos_Cotizaciones();
            if (ddlTipo.SelectedValue.ToUpper() == "DESARROLLO")
            {
                int rowIndex = Convert.ToInt32(tbId_Fila.Text);
                string script = $"SeleccionarFilayEnfocarDesarrollo({rowIndex});";
                ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarDesarrollo", script, true);
            }
            else if (ddlTipo.SelectedValue.ToUpper() == "COTIZACIÓN")
            {
                int rowIndex = Convert.ToInt32(tbId_Fila.Text);
                string script = $"SeleccionarFilayEnfocarCotizacion({rowIndex});";
                ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarCotizacion", script, true);
            }

        }
        private void ActivarComplejo(string ID, bool Complejo)
        {
            DateTime FechaIngresoActual = Convert.ToDateTime(tbFechaIngreso.Text);
            DateTime FechaEntregaActualizda20Dias = CalcularFechaEntregaComplejo(FechaIngresoActual);
           
            // el calculo de la fecha esta Ok solo falta hacer el update 

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblSoliciDiseEspe SET DesarrolloComplejo = @Complejo, Fecha_Programada_Entrega = @FechaEntrega where ID_Solicitud = @ID_Solicitud ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID);
                    cmd.Parameters.AddWithValue("@Complejo", Complejo);
                    cmd.Parameters.AddWithValue("@FechaEntrega", FechaEntregaActualizda20Dias);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void DesactivarComplejo(string ID, bool Complejo)
        {

            DateTime FechaIngresoActual = Convert.ToDateTime(tbFechaIngreso.Text);
            DateTime FechaProEntrega = CalcularFechaEntrega(FechaIngresoActual);

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblSoliciDiseEspe SET DesarrolloComplejo = @Complejo, Fecha_Programada_Entrega = @FechaEntrega where ID_Solicitud = @ID_Solicitud ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID);
                    cmd.Parameters.AddWithValue("@Complejo", Complejo);
                    cmd.Parameters.AddWithValue("@FechaEntrega", FechaProEntrega);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        private bool ConsultarComplejo(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "SELECT DesarrolloComplejo FROM tblSoliciDiseEspe WHERE ID_Solicitud = @ID_Solicitud";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID);
                    object result = cmd.ExecuteScalar();

                    if (result != null && bool.TryParse(result.ToString(), out bool Complejo))
                    {
                        return Complejo;
                    }
                    else if (result != null && (result is int || result is byte))
                    {
                        // En caso de que la columna Urgente sea int o byte en la base de datos
                        return Convert.ToInt32(result) == 1;
                    }
                    else
                    {
                        // Manejar el caso donde el resultado es nulo o no se puede convertir
                        return false;
                    }
                }
            }
        }
        public DateTime CalcularFechaEntregaComplejo(DateTime FechaIngreso)
        {
            DateTime UltimaActivacionSolicitud = FechaIngreso;
            

            //Se valida  si ingresan la solicitud un dia sabado o domingo 
            while (UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Sunday)
            {
                UltimaActivacionSolicitud = UltimaActivacionSolicitud.AddDays(1);
                UltimaActivacionSolicitud = new DateTime(UltimaActivacionSolicitud.Year, UltimaActivacionSolicitud.Month, UltimaActivacionSolicitud.Day, 8, 0, 0);
            }

             DateTime  FechaEntrega = SumarDiaLaboral(UltimaActivacionSolicitud, 20);
            
            return FechaEntrega;
        }


        // Confirmar Solicitud Especial Urgente 
        protected void btnConUrgente_Click(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                SpanId_Sol_Urg.InnerText = lbNumeroSolicitud.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirSolUrgente').modal('show');", true);
            }
            else
            {
                string mensajePersonalizado = "No cuentas con los permisos necesarios para cambiar una solicitud a urgente.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }

        }
        protected void btnConUrg_Click(object sender, EventArgs e)
        {
            bool chkUrgente = chxUrgente.Checked;
            string IdSolicitud = lbNumeroSolicitud.Text;

            if (chkUrgente)
            {
                bool UrgenteDB = ConsultarUrgente(IdSolicitud);

                if (UrgenteDB == chkUrgente)
                {
                    string mensajeExito = "La solicitud ya está marcada como Urgente.";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                }
                else
                {
                    ActulizarUrgente(IdSolicitud, chkUrgente);

                    string mensajeExito = "La solicitud ha sido marcada como Urgente";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);


                }




            }
            else
            {

                bool UrgenteDB = ConsultarUrgente(IdSolicitud);

                if (UrgenteDB == chkUrgente)
                {
                    string mensajeExito = "La solicitud ya está marcada como No Urgente.";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }
                else
                {
                    ActulizarUrgente(IdSolicitud, chkUrgente);
                    string mensajeExito = "La solicitud ha sido marcada como No Urgente.";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }



            }

            CargarDesarrollos_Cotizaciones();

            if (ddlTipo.SelectedValue.ToUpper() == "DESARROLLO")
            {
                int rowIndex = Convert.ToInt32(tbId_Fila.Text);
                string script = $"SeleccionarFilayEnfocarDesarrollo({rowIndex});";
                ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarDesarrollo", script, true);
            }
            else if (ddlTipo.SelectedValue.ToUpper() == "COTIZACIÓN")
            {
                int rowIndex = Convert.ToInt32(tbId_Fila.Text);
                string script = $"SeleccionarFilayEnfocarCotizacion({rowIndex});";
                ScriptManager.RegisterStartupScript(this, GetType(), "SeleccionarFilayEnfocarCotizacion", script, true);
            }




        }
        private void ActulizarUrgente(string ID, bool urgente)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblSoliciDiseEspe SET  Urgente = @Urgente WHERE ID_Solicitud = @ID_Solicitud";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID);
                    cmd.Parameters.AddWithValue("@Urgente", urgente);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private bool ConsultarUrgente(string ID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "SELECT Urgente FROM tblSoliciDiseEspe WHERE ID_Solicitud = @ID_Solicitud";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID);
                    object result = cmd.ExecuteScalar();

                    if (result != null && bool.TryParse(result.ToString(), out bool urgente))
                    {
                        return urgente;
                    }
                    else if (result != null && (result is int || result is byte))
                    {
                        // En caso de que la columna Urgente sea int o byte en la base de datos
                        return Convert.ToInt32(result) == 1;
                    }
                    else
                    {
                        // Manejar el caso donde el resultado es nulo o no se puede convertir
                        return false;
                    }
                }
            }
        }


        // Devolver Solicitud a Proceso de Ventas      
        protected void btnDevolverSolicitud_SI_Click(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                // Se ponen las variables en los textbox del modal 
                string IdSolicitud = lbNumeroSolicitud.Text;
                tbObra.Text = tbCliente.Text + "-" + tbProyecto.Text;
                tbOt.Text = "SPE" + IdSolicitud;
                tbPed.Text = "0";

                // Agregamod vacio en ddlTipoObservacion 
                ddlTipoObservacion.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" "));
                ddlTipoObservacion.SelectedIndex = 0;

                // Ponemos la fecha del dia por defecto 
                DateTime Fecha = DateTime.Now;
                tbfechaActividad.Text = Fecha.ToString("yyyy-MM-dd");
                tbfechaActividad.Enabled = false;

                // Consultamos el correo por defecto de la solicitud especial 
                ConsultarCorreo(IdSolicitud);

                // ocultar boton de grabar observacion detener
                btnGrabarObservacionDetener.Visible = false;
                btnCerrarDetener.Visible = false;

                // Se abre el modal de la observacion 
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolverDetener').modal('show');", true);
            }
            else
            {
                string mensajePersonalizado = "No cuentas con los permisos necesarios para devolver una solicitud.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }



        }
        protected void btnCerrarDevolver_Click(object sender, EventArgs e)
        {
            // Refrescar la página después de cerrar el modal        
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalCerrarDev').modal('show');", true);
        }


        // Metodos cargar observacion en modal 
        private void ConsultarCorreo(string ID_Solicitud)
        {
            // Realizar la conexión y la consulta a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT E.Mail  FROM tblSoliciDiseEspe AS  SE INNER JOIN tblAsesorComercial AS E " +
                               " ON  E.Nombre + ' ' + Apellidos =  SE.Asesor WHERE SE.ID_Solicitud = @Id_Solicitud";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id_Solicitud", ID_Solicitud);

                SqlDataReader reader = command.ExecuteReader();

                // Verificar si hay filas devueltas por la consulta
                if (reader.Read())
                {
                    string mail = reader["Mail"].ToString();
                    tbReceptorCorreo.Text = mail; // Asignar el valor a TextBox3
                }
                else
                {
                    tbReceptorCorreo.Text = string.Empty; // Si no hay resultados, establecer el TextBox3 como vacío
                }

            }

        }
        protected void DataGridReceptorMail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerMail")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridReceptorMail.Items[rowIndex];

                string Nombre = row.Cells[2].Text;
                string mailAgregar = row.Cells[3].Text;
                string cedula = row.Cells[4].Text;


                // Tomamos los mail ya agregados y las cedulas agregadas
                string MailAgregados = tbReceptorCorreo.Text;
                string cedulaAgregadas = tbCedulaRecp.Text;
                string NombreAgregado = tbNombreRecp.Text;

                if (!MailAgregados.Contains(mailAgregar))
                {
                    //Agregamos el correo del  receptor
                    tbReceptorCorreo.Text = MailAgregados + ";" + mailAgregar;

                    // Agregamos la cedula del Receptor 

                    if (tbCedulaRecp.Text != "")
                    {
                        tbCedulaRecp.Text = cedulaAgregadas + ";" + cedula;
                        tbNombreRecp.Text = NombreAgregado + ";" + Nombre;
                    }
                    else
                    {
                        tbCedulaRecp.Text = cedula;
                        tbNombreRecp.Text = Nombre;
                    }


                    //se usa Para darle un color a la fila seleccionada  
                    e.Item.CssClass = "fila-seleccionada";
                }
                else
                {
                    // Eliminamos el correo del receptor  
                    tbReceptorCorreo.Text = MailAgregados.Replace(";" + mailAgregar, "");

                    // Eliminamos la cedula del receptor 
                    tbCedulaRecp.Text = cedulaAgregadas.Replace(cedula, "").TrimEnd(';').Replace(";;", ";");

                    // Eliminamos el  nombre  del receptor 
                    tbNombreRecp.Text = NombreAgregado.Replace(Nombre, "").TrimEnd(';').Replace(";;", ";");


                    e.Item.CssClass = "fila-seleccionada2";
                }


                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolverDetener').modal('show');", true);

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


            }
        }
        protected void ddlTipoObservacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // metodo por si en algun momento agregan correos por defecto para devoluciones a  verntas 
            string CorreoTipoObser = ConsultarCorreoPorTipoObservacion();

            tbRecepTipoObs.Text = "";
            tbRecepTipoObs.Text = CorreoTipoObser;


            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolverDetener').modal('show');", true);
        }
        protected string ConsultarCorreoPorTipoObservacion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            string correo = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DestinatarioPorDefecto FROM tblTipoObservacion " +
                                   "WHERE UsoEspecifico = 0 AND Activa = 1 AND ID_TipoObservacion = @IdObservacion";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IdObservacion", ddlTipoObservacion.SelectedValue);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        correo = reader["DestinatarioPorDefecto"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo, loguear el error
                // También puedes lanzar una excepción o devolver un mensaje de error
            }

            return correo;
        }
        protected void btnRedireccionar_Sol_Click(object sender, EventArgs e)
        {
            Session["CargarSolicitud"] = "1";
            Session["CargarSolicitud_ID"] = lbNumeroSolicitud.Text;
            Response.Redirect("Solicitud_Especial.aspx");
        }

        // Grabar observacion y devolver solicitud 
        protected void BtnGrabarObservacion_Click(object sender, EventArgs e)
        {
            // Validar que tenga los campos necesarios
            if (ValidarCamposRequeridos())
            {
                InsertarObservacion();

                string ID_Solicitud = tbOt.Text;
                if (ID_Solicitud.StartsWith("SPE"))
                {
                    ID_Solicitud = ID_Solicitud.Substring(3).TrimStart();
                }

                // Traemos el Id_MaxObservacion 
                string Id_Observacion = ConsultarId_Observacion();

                // Se valida si hay receptores seleccionados             
                if (tbCedulaRecp.Text != "")
                {
                    string cedulasNotificar = tbCedulaRecp.Text.Trim(';');
                    string NombresNotificar = tbNombreRecp.Text.Trim(';');

                    string[] CedNot = cedulasNotificar.Split(';');
                    string[] NomNot = NombresNotificar.Split(';');

                    for (int i = 0; i < CedNot.Length; i++)
                    {
                        // Llamamos al método InsertarObservacionReceptores con la cédula y el nombre actuales
                        InsertarObservacionReceptores(Id_Observacion, CedNot[i], NomNot[i]);

                    }
                }

                //Consultamos el area de aplicacion  con el codigo del ddlTipoObservacion              
                string Aplicacion = ConsultarAreaAplicacion();

                //Enviar la notificacion por Correo 
                string destinatarios = (tbReceptorCorreo.Text + ";" + tbRecepTipoObs.Text).Trim(';').Trim(' ');
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
                            <h3>Notificación de Observación: </h3>
                            <p> <strong> Fecha de Observación: </strong> " + DateTime.Now.ToString() + @"</p>
                            <p><strong> Emisor : </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>
                            <p><strong>Nombre de la Obra: </strong> " + tbObra.Text + @"</p>
                            <p><strong>Tipo Observacion : </strong> " + ddlTipoObservacion.SelectedItem.Text + @"</p>
                            <p><strong>Detalle Observación: </strong> " + txObservacion.InnerText + @"</p>
                            <p><strong>Fin Observación </strong> </p>
                           
                        </div>
                    </body>
                    </html>";

                //ejecutar el procedimiento almacenado que envia el correo 
                EnviarCorreoDevolucionPE(destinatarios, cuerpo, Aplicacion);

                DevolverSolicitudaVentas(ID_Solicitud);


                string mensajePersonalizado = "La solicitud se devolvió con éxito y se notificó por correo electrónico.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

            }
        }
        private bool ValidarCamposRequeridos()
        {
            bool valido = true;

            if (ddlTipoObservacion.SelectedValue == " ")
            {
                // Mensaje de alerta
                string script1 = "alert('Por favor seleccione el tipo de observación.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", script1, true);
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolverDetener').modal('show');", true);
                valido = false;
            }

            if (txObservacion.InnerText.Trim() == "")
            {
                // Mensaje de alerta
                string script1 = "alert('Por favor escriba  la justificación de la observación.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", script1, true);
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolverDetener').modal('show');", true);
                valido = false;
            }

            if (tbOt.Text == "")
            {
                // Mensaje de alerta
                string script1 = "alert('No se ha seleccionado una OT o una Solicitud Especial para generar una observacion');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", script1, true);
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolverDetener').modal('show');", true);
                valido = false;

            }


            return valido;
        }
        public void InsertarObservacion()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblOtObservacion (Id_OT,Consecutivo_Pedido,Nombre_Obra,Observacion,FechaObservacion,Emisor,Nombre_Emisor," +
                               "ID_TipoObservacion,FechaAnteriorDespacho,FechaNuevaDespacho,CedulaAsesor, FechaActividad,  Destinatarios) " +
                               "VALUES(@OT, @Pedido, @NombreObra, @Observacion, @fechaObsercion,@Emisor, @Nombre_Emisor, @ID_TipoObservacion," +
                               " @FechaAnteriorDespacho,@FechaNuevaDespacho, @CedulaAsesor, @FechaActividad,@Destinatarios)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OT", tbOt.Text);
                    command.Parameters.AddWithValue("@Pedido", tbPed.Text);
                    command.Parameters.AddWithValue("@NombreObra", tbObra.Text);
                    command.Parameters.AddWithValue("@Observacion", txObservacion.InnerText);
                    command.Parameters.AddWithValue("@fechaObsercion", DateTime.Now);
                    command.Parameters.AddWithValue("@Emisor", Session["CedulaLogeada"].ToString());
                    command.Parameters.AddWithValue("@Nombre_Emisor", Session["usuariologueado"].ToString());
                    command.Parameters.AddWithValue("@ID_TipoObservacion", ddlTipoObservacion.SelectedValue);
                    command.Parameters.AddWithValue("@FechaAnteriorDespacho", DateTime.Now);
                    command.Parameters.AddWithValue("@FechaNuevaDespacho", DateTime.Now);
                    command.Parameters.AddWithValue("@CedulaAsesor", ddlAsesor.SelectedValue);
                    command.Parameters.AddWithValue("@FechaActividad", tbfechaActividad.Text);
                    command.Parameters.AddWithValue("@Destinatarios", tbReceptorCorreo.Text);


                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Cambiar el mensaje de error
                        //Console.WriteLine("Error al insertar datos: " + ex.Message);
                    }
                }
            }
        }
        public string ConsultarId_Observacion()
        {
            string Id_Observacion = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select max(id_Observacion) from tblOTObservacion";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        Id_Observacion = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción 
                        //Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                    }
                }
            }

            return Id_Observacion;
        }
        public void InsertarObservacionReceptores(string Id_Observacion, string cedula, string nombre)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblOTObservacion_Receptor (Id_Observacion,Receptor,Nombre_Receptor) Values(@ID_Observacion, @CedulaRecep , @NombreReceptor)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Observacion", Id_Observacion);
                    command.Parameters.AddWithValue("@CedulaRecep", cedula);
                    command.Parameters.AddWithValue("@NombreReceptor", nombre);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Cambiar el mensaje de error
                        // Console.WriteLine("Error al insertar datos: " + ex.Message);
                    }
                }
            }
        }
        public string ConsultarAreaAplicacion()
        {
            string areaAplicacion = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Aplicacion FROM tblTipoObservacion WHERE UsoEspecifico=1  AND  Activa=1  AND  Aplicacion like '%' AND ID_TipoObservacion = @TipoObs";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@TipoObs", ddlTipoObservacion.SelectedValue);


                    try
                    {
                        connection.Open();
                        areaAplicacion = Convert.ToString(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return areaAplicacion;
        }
        public void EnviarCorreoDevolucionPE(string destinatarios, string cuerpo, string aplicacion)
        {
            string nombreProcedimiento = "duc_sp_Correo";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(nombreProcedimiento, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Definir los parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Destinatarios", destinatarios);
                    command.Parameters.AddWithValue("@asunto", "Observacion: " + aplicacion + " APL: " + tbOt.Text + "-" + tbPed.Text);
                    command.Parameters.AddWithValue("@cuerpo", cuerpo);
                    command.Parameters.AddWithValue("@adjuntos", "");
                    command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                    catch (SqlException ex)
                    {
                        // Manejar la excepción (opcional)
                        //error.Visible = true;
                        // error.Text = ex.Message;
                    }
                }
            }
        }
        private void DevolverSolicitudaVentas(string ID_Solicitud)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE tblSoliciDiseEspe SET ProgramadoVentas=0, Terminado=0, FechaRespuesta= @fecha WHERE id_Solicitud= @ID_Solicitud";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID_Solicitud);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                    DevolverDetallesSolicitudaVentas(ID_Solicitud);
                }

            }
        }
        private void DevolverDetallesSolicitudaVentas(string ID_Solicitud)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "Update tblSoliciDiseEspeDeta set ComprasOk=0 where Id_Solicitud= @ID_Solicitud";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID_Solicitud);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }


        // Metodo para cargar la solicitud si tiene ID_Solicitud 
        private void CargarSolictudEspecial(string idSolcitud)
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblSoliciDiseEspe WHERE ID_Solicitud = @IdSolcitud ORDER BY Fecha_Ingreso ASC";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@IdSolcitud", idSolcitud);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                string IdSolicitud = row["ID_Solicitud"].ToString();
                Session["Id_Solicitud"] = IdSolicitud;
                ID_Sol_Dib.Text = IdSolicitud;
                string NombreProyecto = row["Proyecto"].ToString();
                string Asesor = row["Asesor"].ToString();
                string FechaIngreso = row["Fecha_Ingreso"].ToString();
                DateTime FechaIngresoForm = DateTime.Parse(FechaIngreso);
                string Dirigidoa = row["Dirigidoa"].ToString();
                string Tipo = row["TipoSolicitud"].ToString();
                string RealizadoPor = row["RealizadoPor"].ToString();
                string termiVenta = row["ProgramadoVentas"].ToString();
                string TermiDiseño = row["Terminado"].ToString();
                string FechaEntrega = row["Fecha_Programada_Entrega"].ToString();
                DateTime FechaEntregaForm = DateTime.Parse(FechaEntrega);
                string FechaRespuesta = row["FechaRespuesta"].ToString();
                DateTime FechaRespuestaForm = DateTime.Parse(FechaRespuesta);
                string SoliOrigen = row["id_SolicitudOrigen"].ToString();
                string Ciudad = row["CiudadProyecto"].ToString();
                string Viatico = row["CotizarViaTte"].ToString();
                string Cotizacion = row["Cotizacion"].ToString();
                string Cliente = row["Cliente"].ToString();
                string Contacto = row["Contacto"].ToString();
                string Telefono = row["Telefono"].ToString();
                string Celular = row["Celular"].ToString();
                string Mail = row["Mail"].ToString();
                string Direccion = row["Direccion"].ToString();
                string SegPausa = row["SeguimientoPausa"].ToString();
                string DesComplejo = row["DesarrolloComplejo"].ToString();
                string Urgente = row["Urgente"].ToString();

                lbNumeroSolicitud.Text = IdSolicitud;
                tbProyecto.Text = NombreProyecto;
                tbProyectoServidor.Text = NombreProyecto;

                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == Asesor)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbFechaIngreso.Text = FechaIngresoForm.ToString("yyyy-MM-dd");
                tbFechaIngresoServidor.Text = FechaIngresoForm.ToString("yyyy-MM-dd");
                foreach (ListItem item in ddlDirigido.Items)
                {
                    if (item.Text == Dirigidoa)
                    {
                        ddlDirigido.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                foreach (ListItem item in ddlTipo.Items)
                {
                    if (item.Text == Tipo)
                    {
                        ddlTipo.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                tbDesarrollaPor.Text = RealizadoPor;
                tbFechaEntrega.Text = FechaEntregaForm.ToString("yyyy-MM-dd");
                tbFechaEntregaServidor.Text = FechaEntregaForm.ToString("yyyy-MM-dd");
                tbFechaRespuesta.Text = FechaRespuestaForm.ToString("yyyy-MM-dd");
                tbFechaRespuestaServidor.Text = FechaRespuestaForm.ToString("yyyy-MM-dd");
                tbSolicitudOrigen.Text = SoliOrigen;

                foreach (ListItem item in ddlCiudad.Items)
                {
                    if (item.Text == Ciudad)
                    {
                        ddlCiudad.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }

                chxViaticos.Checked = Viatico == "True";
                tbCotizacionEsp.Text = Cotizacion;
                tbCliente.Text = Cliente;
                tbClienteServidor.Text = Cliente;
                tbContacto.Text = Contacto;
                tbContactoServidor.Text = Contacto;
                tbTelefono.Text = Telefono;
                tbTelefonoServidor.Text = Telefono;
                tbCelular.Text = Celular;
                tbCelularServidor.Text = Celular;
                tbMail.Text = Mail;
                tbMailServidor.Text = Mail;
                tbDireccion.Text = Direccion;
                tbDireccionServidor.Text = Direccion;

                if (SegPausa == "&nbsp;")
                {
                    string SegPausaRep = SegPausa.Replace("&nbsp;", "");
                    txObsDesarrollo.InnerText = SegPausaRep;
                }
                else
                {
                    txSegPausa.InnerText = SegPausa;
                }
                chxUrgente.Checked = Convert.ToBoolean(Urgente);
                chxDesComplejo.Checked = Convert.ToBoolean(DesComplejo);

                LimpiarCamposDetalle();

                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    if (termiVenta != "True")
                    {
                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        string script = "<script>setTimeout(function() { HabilEnla1Ventas(); }, 100);</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);
                    }
                    else
                    {
                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn btn-secondary";

                        string script = "<script>setTimeout(function() { HabilitarEnlaces4(); }, 100);</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);
                    }
                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {


                    if (TermiDiseño != "True")
                    {
                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        ConfirmarComplejo.Enabled = true;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-primary";

                        btnConUrgente.Enabled = true;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-primary";

                        chxDesComplejo.Enabled = true;
                        chxUrgente.Enabled = true;

                        if (termiVenta == "True")
                        {
                            string script = "<script>setTimeout(function() { HabEnlDiseño(); }, 100);</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);
                        }
                        else
                        {
                            string script = "<script>setTimeout(function() { HabEnlDiseño3(); }, 100);</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño3", script, false);
                        }
                    }
                    else
                    {
                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-secondary";

                        ConfirmarComplejo.Enabled = false;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-secondary";

                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = false;
                        chxUrgente.Enabled = false;

                        string script = "<script>setTimeout(function() { HabEnlDiseño2(); }, 100);</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);
                    }
                }
            }
        }



        // Pausar Solicitud Especial
        protected void btnPausar_Si_Click(object sender, EventArgs e)
        {
            if (PausarSolicitudEspecial())
            {
                // Consultar el correo electrónico del asesor a notificar 
                string emisor = ConsultarCorreoEmisor();
                string destinatario = ConsultarCorreoNotificar();
                string destinatarios = emisor.TrimEnd(';') + ";" + destinatario;

                // Separar y validar correos electrónicos
                var correos = destinatarios.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> correosValidos = new List<string>();
                List<string> correosInvalidos = new List<string>();

                foreach (var correo in correos)
                {
                    if (EsCorreoValido(correo))
                    {
                        correosValidos.Add(correo);
                    }
                    else
                    {
                        correosInvalidos.Add(correo);
                    }
                }

                if (correosValidos.Count > 0)
                {
                    string correosValidosString = string.Join(";", correosValidos);

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
                            max-width: 40em;
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
                        <h3>Notificación Desarrollo Pausado: </h3>
                        <p> <strong> Fecha: </strong> " + DateTime.Now.ToString() + @"</p>
                        <p><strong> Responsable pausa :  </strong> " + Session["usuariologueado"].ToString() + @"</p>
                        <p><strong>Desarrollo N°: </strong>  " + lbNumeroSolicitud.Text + @"</p>
                        <p><strong>Cliente: </strong> " + tbCliente.Text + @"</p>
                        <p><strong>Proyecto: </strong> " + tbProyecto.Text + @"</p>
                        <p><strong>Detalle pausa: </strong> " + txJustificacionPausa.InnerText + @"</p>
                        <p> Cualquier inquietud no dude en comunicarse con: " + Session["usuariologueado"].ToString() + @"</p>
                        <p><strong>Departamento Dibujo y Despiece: </strong></p>
                        <p><strong>Fin Notificación </strong> </p>
                    </div>
                </body>
                </html>";

                    bool correoEnviado = EnviarCorreoPausarSolEspe(correosValidosString, cuerpo);

                    if (correoEnviado)
                    {

                        if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "COTIZACIÓN")
                        {
                            ID_Cot_Dib.Text = lbNumeroSolicitud.Text;
                            BuscarCot_Click(sender, e);


                            DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                                DataGrid2.Items[0],
                                DataGrid2,
                                new CommandEventArgs("VerCotizacion", 0)
                            );
                            DataGridSolicitudPE_LinkButton(DataGrid2, args);
                        }
                        else if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "DESARROLLO")
                        {
                            ID_Sol_Dib.Text = lbNumeroSolicitud.Text;
                            BuscarSol_Click(sender, e);


                            DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                                DataGrid1.Items[0],
                                DataGrid1,
                                new CommandEventArgs("VerDesarrollo", 0)
                            );
                            DataGridSolicitudPE_LinkButton(DataGrid1, args);
                        }

                        // Mostrar mensaje de éxito
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", "alert('El desarrollo fue puasado y notificado a los correos: \\n " + correosValidosString + "');", true);
                    }
                    else
                    {
                        // Mostrar mensaje de error
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Error al enviar el correo. Por favor, intente nuevamente.');", true);
                    }
                }

                if (correosInvalidos.Count > 0)
                {
                    string correosInvalidosString = string.Join(", ", correosInvalidos);
                    ScriptManager.RegisterStartupScript(this, GetType(), "showInvalidEmails", "alert('El desarrollo fue puasado, pero las siguientes direcciones de correo son inválidas: " + correosInvalidosString + " y no fueron notificadas');", true);
                }
            }
            else
            {
                // Manejar la excepción y mostrar mensaje de "intente nuevamente"
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Error al pausar la solicitud especial. Por favor, intente nuevamente.');", true);
            }
        }
        private bool PausarSolicitudEspecial()
        {
            DateTime fecha = DateTime.Now;
            string SegPausa = "(Desarrollo pausado por el dibujante: " + Session["usuariologueado"].ToString() + "el " + fecha + "Razón " + txJustificacionPausa.InnerText + ")" + txSegPausa.InnerText;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblSoliciDiseEspe SET Pausado = 1,  SeguimientoPausa = @seguimientoPausa WHERE ID_Solicitud = @ID_Solicitud  ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@seguimientoPausa", SegPausa);
                    cmd.Parameters.AddWithValue("@ID_Solicitud", lbNumeroSolicitud.Text);

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
        public string ConsultarCorreoNotificar()
        {
            string correo = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT AC.Mail FROM tblSoliciDiseEspe AS SE INNER JOIN " +
                               "tblAsesorComercial AS AC ON AC.Nombre + ' ' + AC.Apellidos = SE.Asesor WHERE ID_Solicitud = @ID_Solicitud";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@ID_Solicitud", lbNumeroSolicitud.Text);
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
                string query = "select mail from tblEmpleado where Cedula = @cedulalogueada";
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
        public bool EnviarCorreoPausarSolEspe(string destinatarios, string cuerpo)
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
                        command.Parameters.AddWithValue("@Destinatarios", destinatarios);
                        command.Parameters.AddWithValue("@asunto", "Desarrollo " + lbNumeroSolicitud.Text + " pausado");
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
        private bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(correo);
                return addr.Address == correo;
            }
            catch
            {
                return false;
            }
        }


        //Despausar solicitud 
        protected void btnDespausar_SI_Click(object sender, EventArgs e)
        {
            // calculamos la nueva fecha de entrega

            DateTime FechaIngreso = DateTime.Now;
            DateTime FechaEntrega = CalcularFechaEntrega(FechaIngreso);
            // agregamos a seguimiento las fechas de activacion 

            string SeguPausas = "(Diseño Reactivado el " + FechaIngreso + " , Fecha Ingreso anterior: " + tbFechaIngreso.Text + ")" + "\n" + txSegPausa.InnerText;

            // realizamos la Actualizacion  en la base de datos 
            if (DespausarSolicitudEspecial(SeguPausas, FechaEntrega))
            {
                if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "COTIZACIÓN")
                {
                    ID_Cot_Dib.Text = lbNumeroSolicitud.Text;
                    BuscarCot_Click(sender, e);

                   
                    DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                        DataGrid2.Items[0],
                        DataGrid2,
                        new CommandEventArgs("VerCotizacion", 0)
                    );
                    DataGridSolicitudPE_LinkButton(DataGrid2, args);
                }
                else if (lbNumeroSolicitud.Text != "" && ddlTipo.SelectedItem.Text == "DESARROLLO")
                {
                    ID_Sol_Dib.Text = lbNumeroSolicitud.Text;
                    BuscarSol_Click(sender, e);

                    
                    DataGridCommandEventArgs args = new DataGridCommandEventArgs(
                        DataGrid1.Items[0],
                        DataGrid1,
                        new CommandEventArgs("VerDesarrollo", 0)
                    );
                    DataGridSolicitudPE_LinkButton(DataGrid1, args);
                }

               


                // Se actualizo correctamente
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('El desarrollo de reactivo exitosamente.');", true);
            }
            else
            {
                // Manejar la excepción y mostrar mensaje de "intente nuevamente"
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Error al despausar la solicitud especial. Por favor, intente nuevamente.');", true);
            }


        }
        private bool DespausarSolicitudEspecial(string segPau, DateTime fechaentrega)
        {
            DateTime fecha = DateTime.Now;


            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblSoliciDiseEspe SET Fecha_Ingreso = @fechaIngreso,  Pausado = 0, UltimaActivacion = @fechaentrega1, Fecha_Programada_Entrega = @fechaEntrega, SeguimientoPausa = @seguimientoPausa WHERE ID_Solicitud = @ID_Solicitud  ";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@fechaIngreso", fecha);
                    cmd.Parameters.AddWithValue("@fechaentrega1", fechaentrega);
                    cmd.Parameters.AddWithValue("@fechaEntrega", fechaentrega);
                    cmd.Parameters.AddWithValue("@seguimientoPausa", segPau);
                    cmd.Parameters.AddWithValue("@ID_Solicitud", lbNumeroSolicitud.Text);

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



        // control de vista para los Dibujantes Programacion 
        protected void chkVerDes_CheckedChanged(object sender, EventArgs e)
        {

            if (chkVerDes.Checked)
            {
                bodyDes.Visible = false;
                tituloDes.Visible = true;
            }
            else
            {
                bodyDes.Visible = true;
                tituloDes.Visible = false;
            }

            string script = @"ControlBtnCliente();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);

        }
        protected void chkVerCot_CheckedChanged(object sender, EventArgs e)
        {

            if (chkVerCot.Checked)
            {
                bodyCot.Visible = false;
                tituloCot.Visible = true;
            }
            else
            {
                bodyCot.Visible = true;
                tituloCot.Visible = false;
            }

            string script = @"ControlBtnCliente();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, true);

        }


        // Detener Pedido Especial
        protected void btnDetenerPE_SI_Click(object sender, EventArgs e)
        {
            if (Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO" || Session["Departamento"].ToString().ToUpper() == "DISEÑO")
            {
                // Se ponen las variables en los textbox del modal 
                string IdSolicitud = lbNumeroSolicitud.Text;
                tbObra.Text = tbCliente.Text + "-" + tbProyecto.Text;
                tbOt.Text = "SPE" + IdSolicitud;
                tbPed.Text = "0";

                // Cambiar la consulta del SqlDataSource
                TipoObservacion.SelectCommand = "SELECT Id_TipoObservacion, Aplicacion, Descripcion, Aplicacion + ' - ' + Descripcion as TipoObservacion, " +
                                                 "DestinatarioPorDefecto, Programable, AlDirectorComercial " +
                                                 "FROM tblTipoObservacion " +
                                                 "WHERE Aplicacion Like '%PARAR PEDIDO PE%' " +
                                                 "AND Activa = 1 " +
                                                 "ORDER BY Aplicacion ASC, Descripcion ASC";
                ddlTipoObservacion.DataBind();

                // Agregamos vacio en ddlTipoObservacion 
                ddlTipoObservacion.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" "));
                ddlTipoObservacion.SelectedIndex = 0;



                // Ponemos la fecha del dia por defecto 
                DateTime Fecha = DateTime.Now;
                tbfechaActividad.Text = Fecha.ToString("yyyy-MM-dd");
                tbfechaActividad.Enabled = false;

                // Consultamos el correo por defecto de la solicitud especial 
                ConsultarCorreo(IdSolicitud);

                // ocultar boton de grabar observacion y cerrar  detener
                BtnGrabarObservacion.Visible = false;
                btnCerrarDevolver.Visible = false;

                // Se abre el modal de la observacion 
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolverDetener').modal('show');", true);

            }
            else
            {
                string mensajePersonalizado = "No cuentas con los permisos necesarios para devolver una solicitud.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
        }
        protected void btnGrabarObservacionDetener_Click(object sender, EventArgs e)
        {
            // Validar que tenga los campos necesarios
            if (ValidarCamposRequeridos())
            {
                InsertarObservacion();

                string ID_Solicitud = tbOt.Text;
                if (ID_Solicitud.StartsWith("SPE"))
                {
                    ID_Solicitud = ID_Solicitud.Substring(3).TrimStart();
                }

                // Traemos el Id_MaxObservacion 
                string Id_Observacion = ConsultarId_Observacion();

                // Se valida si hay receptores seleccionados             
                if (tbCedulaRecp.Text != "")
                {
                    string cedulasNotificar = tbCedulaRecp.Text.Trim(';');
                    string NombresNotificar = tbNombreRecp.Text.Trim(';');

                    string[] CedNot = cedulasNotificar.Split(';');
                    string[] NomNot = NombresNotificar.Split(';');

                    for (int i = 0; i < CedNot.Length; i++)
                    {
                        // Llamamos al método InsertarObservacionReceptores con la cédula y el nombre actuales
                        InsertarObservacionReceptores(Id_Observacion, CedNot[i], NomNot[i]);

                    }
                }

                //Consultamos el area de aplicacion  con el codigo del ddlTipoObservacion              
                string Aplicacion = ConsultarAreaAplicacion();

                //Enviar la notificacion por Correo 
                string destinatarios = "andersonbetancur@ducon.com.co"; //(tbReceptorCorreo.Text + ";" + tbRecepTipoObs.Text).Trim(';').Trim(' ');
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
                            <h3>Notificación de Observación: </h3>
                            <p> <strong> Fecha de Observación: </strong> " + DateTime.Now.ToString() + @"</p>
                            <p><strong> Emisor : </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>
                            <p><strong>Nombre de la Obra: </strong> " + tbObra.Text + @"</p>
                            <p><strong>Tipo Observacion : </strong> " + ddlTipoObservacion.SelectedItem.Text + @"</p>
                            <p><strong>Detalle Observación: </strong> " + txObservacion.InnerText + @"</p>
                            <p><strong>Fin Observación </strong> </p>
                           
                        </div>
                    </body>
                    </html>";

                //ejecutar el procedimiento almacenado que envia el correo 
                EnviarCorreoDevolucionPE(destinatarios, cuerpo, Aplicacion);

                DetenerSolicitudEspecial(ID_Solicitud);


                string mensajePersonalizado = "El pedido se Detuvo con éxito y se notificó por correo electrónico.";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
        }
        protected void btnCerrarDetener_Click(object sender, EventArgs e)
        {
            // Refrescar la página después de cerrar el modal        
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalCerrarDet').modal('show');", true);
        }
        private void DetenerSolicitudEspecial(string ID_Solicitud)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE tblSoliciDiseEspe SET Terminado=0, FechaRespuesta = @FechaRes WHERE Id_Solicitud= @ID_Solicitud";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@FechaRes", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID_Solicitud);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }


        //Redirigir detalle a  compras 
        protected void RedirigirCompras_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalRedirigirCompras').modal('show');", true);
        }
        protected void btnRedirigir_SI_Click(object sender, EventArgs e)
        {
            //Realizar la actualizacones 
            ActualizarSolicitudRediCompras();
            ActualizarDetalleRediCompras();

            // Enviar el correo electronico 
            string correoEmisor = ConsultarCorreoEmisor();
            string correoCompras = ConsultarCorreoCompras();
            string correoAsesor = ConsultarCorreoAsesor();

            string destinatarios = correoEmisor + ";" + correoAsesor + ";" + correoCompras;


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
                            <h3>Redirección  a Compras </h3>
                            <p> <strong> Fecha: </strong> " + DateTime.Now.ToString() + @"</p>
                            <p><strong> Realizado por : </strong> <strong> " + Session["usuariologueado"].ToString() + @"</strong></p>
                            <p><strong>solicitud </strong> " + lbNumeroSolicitud.Text + "_" + lbIdDetalle.Text + @"</p>
                            <p>Ver documentación asignada a COMPRAS </p>
                            <p><strong>Prueba de sistemas SID nuevo Anderson, hacer caso omiso </strong> </p>
                           
                        </div>
                    </body>
                    </html>";



            if (EnviarCorreRediCompras(destinatarios, cuerpo))
            {
                string mensajePersonalizado = "La solicitud a sido redirigida a compras y notificada por correo electronico";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
            else
            {
                string mensajePersonalizado = "La Solicitud ha sido redireccionada a compras, pero no fue notificada, por favor confimar notificación .";
                string urlRedireccion = "Ventas/Solicitud_Especial.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }




        }
        private void ActualizarSolicitudRediCompras()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblSoliciDiseEspe SET  RedirigidoCompras = 1 WHERE id_Solicitud= @ID_Solicitud ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@ID_Solicitud", lbNumeroSolicitud.Text);

                    int filaAfectada = cmd.ExecuteNonQuery();
                }
            }
        }
        private void ActualizarDetalleRediCompras()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblSoliciDiseEspeDeta SET RedirigidoaCompras=1, Redirigidoel= @FechaRed, ComprasOk= 0 WHERE Id_SolicitudDetalle= @ID_Detalle ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@FechaRed", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ID_Detalle", lbIdDetalle.Text);

                    int filaAfectada = cmd.ExecuteNonQuery();

                }
            }
        }
        public string ConsultarCorreoCompras()
        {
            string correo = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select  mail from tblUsosVarios where ObjetivoMail = 'MailSolicitudPECOMPRAS01'";
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
        public string ConsultarCorreoAsesor()
        {
            string correo = "";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = " SELECT E.Mail  FROM tblSoliciDiseEspe AS SE INNER JOIN tblEmpleado AS E " +
                               "ON  E.Nombre + ' ' + Apellidos = SE.Asesor WHERE SE.ID_Solicitud = @Id_Solicitud";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Solicitud", lbNumeroSolicitud.Text);
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
        public bool EnviarCorreRediCompras(string destinatarios, string cuerpo)
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
                        command.Parameters.AddWithValue("@Destinatarios", destinatarios);
                        command.Parameters.AddWithValue("@asunto", "Solicitud PE:" + lbNumeroSolicitud.Text + "-" + lbIdDetalle.Text + " " + tbProyecto.Text);
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



    }
}