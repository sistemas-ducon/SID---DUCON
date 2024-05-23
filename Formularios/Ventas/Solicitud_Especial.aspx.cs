using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using MathNet.Numerics;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                    NombreAsesorLogeado();
                    CargarAsesoresEnDropDownList();
                    CargarClienteYContacto();
                    ZonaAsesorLog();
                    DepartamentoAsesor();
                    CargarSession();
                    CargarVariablesDeSesion();

                    // Para Ventas y Asesor 

                    if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                    {

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
                                btnProgramarRender.CssClass = "btn btn-sm btn-warning";

                            }
                        }

                        //Disposicion Botones para Ventas 
                        ControlBotonesVentas();


                        //Se oculta Compas para ventas 
                        DerCompras.Visible = false;


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
                                btnProgramarRender.CssClass = "btn  btn-sm btn-warning";

                            }
                        }

                        //Disposicion Botones para Dibujo                        
                        ControlBotonesDiseño();

                        // Se Cargan los DataGrid con los Datos para Dibujo 
                        CargarDesarrollos_Cotizaciones();

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
                btnTrabajarSolicitud.Enabled = true;
                btnTrabajarSolicitud.CssClass = "btn btn-sm btn-outline-primary";

            }

            //Activar el Boton Trabajar en Desprogramar  
            Button btnDesprogramar = FindControl("btnDesprogramar") as Button;
            if (btnDesprogramar != null)
            {
                btnDesprogramar.Enabled = true;
                btnDesprogramar.CssClass = "btn btn-sm btn-outline-primary";

            }

            //Activar el Boton Trabajar en TrabajarCotizacion 
            Button btnTrbajarCotizacion = FindControl("btnTrbajarCotizacion") as Button;
            if (btnTrbajarCotizacion != null)
            {
                btnTrbajarCotizacion.Enabled = true;
                btnTrbajarCotizacion.CssClass = "btn btn-sm btn-outline-primary";

            }

            //Activar el Boton Trabajar en Desprogramar1 
            Button btnDesprogramar1 = FindControl("btnDesprogramar1") as Button;
            if (btnDesprogramar1 != null)
            {
                btnDesprogramar1.Enabled = true;
                btnDesprogramar1.CssClass = "btn btn-sm btn-outline-primary";

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
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525");    //Verde 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }

                else if (programadoVentas == 1 && pausado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2");    // Aqua

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


        public void CambioZona(object sender, EventArgs e)
        {
            string valorSeleccionado = ddlZona.SelectedValue;

            CambiarSqlDataSource(valorSeleccionado);
            CambiarSqlDataSource2(valorSeleccionado);
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
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525");    //Verde 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }

                else if (programadoVentas == 1 && pausado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2");    // Aqua

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

                string script = "<script>HabilEnla1Ventas();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);

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
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525");    //Verde 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                }

                else if (programadoVentas == 1 && pausado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2");    // Aqua

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




                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    if (termiVenta != "True")
                    {

                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        string script = "<script>HabilEnla1Ventas();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);
                    }
                    else
                    {

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                        string script = "<script>HabilitarEnlaces4();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);
                    }

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {
                    CargarSolicitudPrimerafilaDesarrollo();


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
                            string script = "<script>HabEnlDiseño();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);
                        }
                        else
                        {
                            string script = "<script>HabEnlDiseño3();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);
                        }

                    }
                    else
                    {
                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";


                        ConfirmarComplejo.Enabled = false;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-secondary";

                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = false;
                        chxUrgente.Enabled = false;

                        string script = "<script>HabEnlDiseño2();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);


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


                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    if (termiVenta != "True")
                    {

                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        string script = "<script>HabilEnla1Ventas();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);
                    }
                    else
                    {

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                        string script = "<script>HabilitarEnlaces4();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);
                    }

                }
                else if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {

                    CargarSolicitudPrimerafilaCotizacion();

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
                            string script = "<script>HabEnlDiseño();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);
                        }
                        else
                        {
                            string script = "<script>HabEnlDiseño3();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);
                        }

                    }
                    else
                    {

                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";


                        ConfirmarComplejo.Enabled = false;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-secondary";

                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = false;
                        chxUrgente.Enabled = false;

                        string script = "<script>HabEnlDiseño2();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);


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


                if (Session["Departamento"].ToString().ToUpper() == "VENTAS")
                {
                    if (termiVenta != "True")
                    {

                        Session["ProVenSolicitud"] = termiVenta;

                        btnProgramarSolicitud.Enabled = true;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";

                        string script = "<script>HabilEnla1Ventas();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabilEnla1Ventas", script, false);
                    }
                    else
                    {

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                        string script = "<script>HabilitarEnlaces4();</script>";
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
                            string script = "<script>HabEnlDiseño();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño", script, false);
                        }
                        else
                        {
                            string script = "<script>HabEnlDiseño3();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);
                        }

                    }
                    else
                    {

                        Session["ProDiSolicitud"] = TermiDiseño;

                        btnProgramarSolicitud.Enabled = false;
                        btnProgramarSolicitud.CssClass = "btn btn-sm btn-warning";


                        ConfirmarComplejo.Enabled = false;
                        ConfirmarComplejo.CssClass = "btn btn-sm btn-outline-secondary";

                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn btn-sm btn-outline-secondary";

                        chxDesComplejo.Enabled = false;
                        chxUrgente.Enabled = false;

                        string script = "<script>HabEnlDiseño2();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "HabEnlDiseño2", script, false);


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
                            cmd.Parameters.AddWithValue("@Fecha_Ingreso", tbFechaIngresoServidor.Text);
                            cmd.Parameters.AddWithValue("@Fecha_Programada_Entrega", tbFechaEntregaServidor.Text);
                            cmd.Parameters.AddWithValue("@FechaRespuesta", tbFechaRespuestaServidor.Text);

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
                        cmd.Parameters.AddWithValue("@Fecha_Ingreso", tbFechaIngresoServidor.Text);
                        cmd.Parameters.AddWithValue("@Fecha_Programada_Entrega", tbFechaEntregaServidor.Text);
                        cmd.Parameters.AddWithValue("@FechaRespuesta", tbFechaRespuestaServidor.Text);

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

            Response.Redirect("~/Formularios/Ventas/Solicitud_Especial.aspx");


        }

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
                                // Se debe Realizar Validacion  aun no esta clara  ?????????????? Penidiente 

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

                case "DISEÑO": //Boton Programar  Departamento Compras

                    break;

                case "RECEPCION": // Boton Programar  Departamento Compras Desarrollo Producto


                    break;


                default:
                    // Error Con el despartamento de ese usuario Validar con Sistemas 
                    break;
            }




        }


        //Calculo de la Fecha de entrega 
        public DateTime CalcularFechaEntrega(DateTime FechaIngreso)
        {
            DateTime UltimaActivacionSolicitud = FechaIngreso;

            //Se valida  si ingresan la solicitud un dia sabado o domingo 
            while (UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Sunday)
            {
                UltimaActivacionSolicitud = UltimaActivacionSolicitud.AddDays(1);
                UltimaActivacionSolicitud = new DateTime(UltimaActivacionSolicitud.Year, UltimaActivacionSolicitud.Month, UltimaActivacionSolicitud.Day, 8, 0, 0);
            }
            DateTime FechaEntrega = SumarDiaLaboral(UltimaActivacionSolicitud, 5);

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
        }

        [WebMethod]  // Cambiar estado de variable de Session cuando dan click en Modificarsolicitud 
        public static void ModificarSolicitud1()
        {

            HttpContext.Current.Session["nuevaSol"] = null;
        }

        [WebMethod]  // Cambiar estado de variable de Session cuando dan click en Modificarsolicitud 
        public static void Cancelar()
        {

            HttpContext.Current.Session.Remove("nuevaSol");
            HttpContext.Current.Session.Remove("Insertar");
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
                string Urgente = row.Cells[22].Text;
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
                if (Urgente == "True")
                {
                    chxUrgente.Checked = true;
                }
                else
                {
                    chxUrgente.Checked = false;
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


            }
            else
            {
                // La solcitud de origen no tiene detalle para importar  (mensaje)
                string scriptEncontrado = "alert('La solicitud de origen no tiene detalles para importar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showEncontrado", scriptEncontrado, true);
            }

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
                string mensajeExito = "Por favor, seleccione una solicitud de  Desarrollo ";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }

        }
        protected void btnTraSol_Si_Click(object sender, EventArgs e)
        {
            ProgramarDibujanteDesarrollo(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarSolicitudPrimerafilaDesarrollo();
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
                string mensajeExito = "Por favor, seleccione una solicitud de Desarrollo ";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }



        }
        protected void btnDesprogramar_SI_Click(object sender, EventArgs e)
        {
            DesprogramarDibujanteDesarrollo(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarSolicitudPrimerafilaDesarrollo();
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
                string mensajeExito = "Por favor, seleccione una solicitud de  Cotización";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }


        }
        protected void btnProCot_SI_Click(object sender, EventArgs e)
        {
            ProgramarDibujanteCotizacion(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarSolicitudPrimerafilaCotizacion();
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
                string mensajeExito = "Por favor, seleccione una solicitud de  Cotización  ";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
            }

        }
        protected void btnDesCot_SI_Click(object sender, EventArgs e)
        {
            DesprogramarDibujanteCotizacion(lbNumeroSolicitud.Text);
            ContadorClic.Text = "";
            CargarSolicitudPrimerafilaCotizacion();
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

        protected void BuscarSol_Click(object sender, EventArgs e)
        {
            // Se limpia contador de Click
            ContadorClic.Text = "";


            if (ID_Sol_Dib.Text != "")
            {
                if (ValidarExisteSolicitud(ID_Sol_Dib.Text))
                {
                    DataGrid1.DataSourceID = "SolUnica";
                    DataGrid1.DataBind();
                }
                else
                {
                    string mensajeExito = "El desarrollo buscado no existe";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }
            }
            else
            {
                string mensajeExito = "Por favor, digite una solicitud  o seleccione una de la tabla de desarrollo.";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                CargarDesarrollos_Cotizaciones();
            }

        }


        // Buscar Cotizacion  
        protected void ID_Cot_Dib_TextChanged(object sender, EventArgs e)
        {
            if (ID_Cot_Dib.Text == "")
            {
                CargarDesarrollos_Cotizaciones();
            }
            else
            {
                BuscarCot_Click(sender, e);
            }
        }
        protected void BuscarCot_Click(object sender, EventArgs e)
        {
            // Se limpiar contador Click
            ContadorClic.Text = "";

            if (ID_Cot_Dib.Text != "")
            {
                if (ValidarExisteSolicitud(ID_Cot_Dib.Text))
                {
                    DataGrid2.DataSourceID = "CotUnica";
                    DataGrid2.DataBind();
                }
                else
                {
                    string mensajeExito = "La cotización buscada no existe";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }
            }
            else
            {
                string mensajeExito = "Por favor, digite una solicitud  o seleccione una de la tabla de cotizaciones.";
                string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                CargarDesarrollos_Cotizaciones();
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


        // Mantener el datagrid  de Desarrollo fila selecccionada
        private void CargarSolicitudPrimerafilaDesarrollo()
        {
            // actualiza la consulta del SqlDatasource
            CargarDesarrollos.SelectCommand = " SELECT * FROM tblSoliciDiseEspe  WHERE Terminado = 0  AND Dirigidoa='DESARROLLO DE PRODUCTO'  " +
                                              "AND  ProgramadoVentas = 1  AND TipoSolicitud ='DESARROLLO' ORDER BY " +
                                              "CASE     WHEN ID_Solicitud = @Id_Sol  THEN 0     ELSE 1   END,   Fecha_Ingreso ASC";


            // Limpiar los parámetros anteriores
            CargarDesarrollos.SelectParameters.Clear();

            int num = Convert.ToInt32(lbNumeroSolicitud.Text);
            CargarDesarrollos.SelectParameters.Add("Id_Sol", num.ToString());

            DataGrid1.DataSourceID = "CargarDesarrollos";
            DataGrid1.DataBind();
            if (DataGrid1.Items.Count > 0)
            {
                DataGridItem primeraFila = DataGrid1.Items[0];
                primeraFila.CssClass = "fila-seleccionada1"; // Asigna la clase CSS
            }
        }


        // Mantener el datagrid Cotizacion fila selecccionada 
        private void CargarSolicitudPrimerafilaCotizacion()
        {
            // actualiza la consulta del SqlDatasource
            CargarCotizaciones.SelectCommand = "SELECT * FROM tblSoliciDiseEspe  WHERE Terminado = 0  AND Dirigidoa='DESARROLLO DE PRODUCTO' " +
                                               " AND  ProgramadoVentas = 1  AND TipoSolicitud ='COTIZACIÓN' ORDER BY " +
                                               "CASE     WHEN ID_Solicitud = @Id_Sol  THEN 0     ELSE 1   END,   Fecha_Ingreso ASC";


            // Limpiar los parámetros anteriores
            CargarCotizaciones.SelectParameters.Clear();

            int num = Convert.ToInt32(lbNumeroSolicitud.Text);
            CargarCotizaciones.SelectParameters.Add("Id_Sol", num.ToString());

            DataGrid2.DataSourceID = "CargarCotizaciones";
            DataGrid2.DataBind();
            if (DataGrid2.Items.Count > 0)
            {
                DataGridItem primeraFila = DataGrid2.Items[0];
                primeraFila.CssClass = "fila-seleccionada1"; // Asigna la clase CSS
            }
        }


        // Confirmar Solicitud Especial Complejo 
        protected void ConfirmarComplejo_Click(object sender, EventArgs e)
        {
            SpanId_sol.InnerText = lbNumeroSolicitud.Text;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirDesCompl').modal('show');", true);

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
                    ActulizarComplejo(IdSolicitud, chkComplejo);
                    string mensajeExito = "Desarrollo Complejo Activado vence en 20 dias";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);

                    if (ddlTipo.SelectedItem.Text == "DESARROLLO")
                    {
                        CargarSolicitudPrimerafilaDesarrollo();
                    }
                    else
                    {
                        CargarSolicitudPrimerafilaCotizacion();
                    }
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
                    ActulizarComplejo(IdSolicitud, chkComplejo);
                    string mensajeExito = "Desarrollo Complejo Desactivado";
                    string scriptNoSeleccionado = "alert('" + mensajeExito + "');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptNoSeleccionado, true);
                }

            }
        }
        private void ActulizarComplejo(string ID, bool Complejo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "Update tblSoliciDiseEspe SET DesarrolloComplejo = @Complejo where ID_Solicitud = @ID_Solicitud ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    // Aquí ajusta los valores según los nombres de columnas reales en tu DataRow
                    cmd.Parameters.AddWithValue("@ID_Solicitud", ID);
                    cmd.Parameters.AddWithValue("@Complejo", Complejo);

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


        // Confirmar Solicitud Especial Urgente 
        protected void btnConUrgente_Click(object sender, EventArgs e)
        {
            SpanId_Sol_Urg.InnerText = lbNumeroSolicitud.Text;
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirSolUrgente').modal('show');", true);
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

                    if (ddlTipo.SelectedItem.Text == "DESARROLLO")
                    {
                        CargarSolicitudPrimerafilaDesarrollo();
                    }
                    else
                    {
                        CargarSolicitudPrimerafilaCotizacion();
                    }
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


            // Se abre el modal de la observacion 
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolver').modal('show');", true);
        }
        private void ConsultarCorreo(string ID_Solicitud)
        {
            // Realizar la conexión y la consulta a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                    string query = "SELECT E.Mail  FROM tblSoliciDiseEspe AS  SE INNER JOIN tblEmpleado AS E " +
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
        protected void btnCerrarDevolver_Click(object sender, EventArgs e)
        {
            // Refrescar la página después de cerrar el modal        
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalCerrarDev').modal('show');", true);
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


                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolver').modal('show');", true);


            }
        }
        protected void btnRedireccionar_Sol_Click(object sender, EventArgs e)
        {
            Response.Redirect("Solicitud_Especial.aspx");
        }
        protected void ddlTipoObservacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // metodo por si en algun momento agregan correos por defecto para devoluciones a  verntas 
            string CorreoTipoObser = ConsultarCorreoPorTipoObservacion();

            tbRecepTipoObs.Text = "";
            tbRecepTipoObs.Text = CorreoTipoObser;


            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ObservacionDevolver').modal('show');", true);
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

    }
}