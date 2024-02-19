using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes;
using Button = System.Web.UI.WebControls.Button;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Label = System.Web.UI.WebControls.Label;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Solicitud_Especial : System.Web.UI.Page
    {

        private bool isModalVisible = false;
        private bool terminadoVentas;
        public bool VariableSolicitudSesion;

        private string CadenaConexionSID = "BD_SIDSQL";


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["usuariologueado"] != null)
                {
                    NombreAsesorLogeado();
                    CargarAsesoresEnDropDownList();
                    CargarClienteYContacto();
                    ZonaAsesorLog();
                    DepartamentoAsesor();
                    CargarSession();
                    CargarVariablesDeSesion();

                    // Programar es para varios departamento
                    Button btnProgramarRender = FindControl("btnProgramarSolicitud") as Button;
                    if (btnProgramarRender != null)
                    {
                        btnProgramarRender.Enabled = false;
                        btnProgramarRender.CssClass = "btn btn-warning";

                    }


                    //Solo para el departamento de Desarrollo
                    Button btnConUrgente = FindControl("btnConUrgente") as Button;
                    if (btnConUrgente != null)
                    {
                        btnConUrgente.Enabled = false;
                        btnConUrgente.CssClass = "btn-sm btn-outline-secondary";

                    }

                    //Solo para el departamento de Desarrollo
                    Button btnComplejo = FindControl("ConfirmarComplejo") as Button;
                    if (btnComplejo != null)
                    {
                        btnComplejo.Enabled = false;
                        btnComplejo.CssClass = "btn-sm btn-outline-secondary";

                    }

                    //Solo para el departamento de Desarrollo
                    Button btnTrabajarSolicitud = FindControl("btnTrabajarSolicitud") as Button;
                    if (btnTrabajarSolicitud != null)
                    {
                        btnTrabajarSolicitud.Enabled = false;
                        btnTrabajarSolicitud.CssClass = "btn-sm btn-outline-secondary";

                    }

                    //Solo para el departamento de Desarrollo
                    Button btnDesprogramar = FindControl("btnDesprogramar") as Button;
                    if (btnDesprogramar != null)
                    {
                        btnDesprogramar.Enabled = false;
                        btnDesprogramar.CssClass = "btn-sm btn-outline-secondary";

                    }

                    //Solo para el departamento de Desarrollo
                    Button btnTrbajarCotizacion = FindControl("btnTrbajarCotizacion") as Button;
                    if (btnTrbajarCotizacion != null)
                    {
                        btnTrbajarCotizacion.Enabled = false;
                        btnTrbajarCotizacion.CssClass = "btn-sm btn-outline-secondary";

                    }

                    //Solo para el departamento de Desarrollo
                    Button btnDesprogramar1 = FindControl("btnDesprogramar1") as Button;
                    if (btnDesprogramar1 != null)
                    {
                        btnDesprogramar1.Enabled = false;
                        btnDesprogramar1.CssClass = "btn-sm btn-outline-secondary";

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
                        ddlDirigido.DataBind();
                        ddlTipo.DataBind();
                        ddlCiudad.DataBind();
                      
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
                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));


                if (terminado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525"); //Verde
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
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
                    if (programadoVentas == 1 && pausado == 1)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2"); // Aqua
                    }

                    else
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1FF43");//amarillo 
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    }

                }


            }


        }


        public void CambioZona(object sender, EventArgs e)
        {
            string valorSeleccionado = ddlZona.SelectedValue;

            CambiarSqlDataSource(valorSeleccionado);




        }


        private void CambiarSqlDataSource(string valorSeleccionado)
        {
            if (valorSeleccionado == "01" || valorSeleccionado == "02")
            {
                DataGrid1.DataSourceID = "Desarrollo";
            }
            else
            {
                DataGrid1.DataSourceID = "CargarDesarrollos";
            }
            string script = "<script>ControlBtnCliente();</script>";
            ScriptManager.RegisterStartupScript(this, GetType(), "ControlBtnCliente", script, false);
            DataGrid1.DataBind();

        }


        public void CambioZona2(object sender, EventArgs e)
        {
            string valorSeleccionado = ddlZona.SelectedValue;

            CambiarSqlDataSource2(valorSeleccionado);

        }


        private void CambiarSqlDataSource2(string valorSeleccionado)
        {

            if (valorSeleccionado == "01" || valorSeleccionado == "02")
            {
                DataGrid2.DataSourceID = "Cotizaciones";
            }
            else
            {
                DataGrid2.DataSourceID = "CargarCotizaciones";
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
                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));


                if (terminado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525"); //Verde
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
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
                    if (pausado == 1)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2"); // Aqua
                    }

                    else
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1FF43");//amarillo 
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    }

                }


            }


        }


        protected void ConsultarSolicitud(object sender, EventArgs e)
        {

            // Validamos si el campo esta vacio para ejecurar un sqldatasource sino usamoos el otr 

            if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbProyectoX.Text != "")
            {
                BuscarDesarrollo.DataSourceID = "SolicXProyecto";
                BuscarDesarrollo.DataBind();

            }
            else if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbClienteX.Text != "")
            {
                BuscarDesarrollo.DataSourceID = "solicitudXCliente";
                BuscarDesarrollo.DataBind();
            }

            else if (tbFechaIni.Text != "" && tbFechaFin.Text != "" && tbSolicitud1.Text != "")
            {
                BuscarDesarrollo.DataSourceID = "SolicitudXID";
                BuscarDesarrollo.DataBind();
            }
            else
            {
                BuscarDesarrollo.DataSourceID = "SolicXFecha";
                BuscarDesarrollo.DataBind();
            }

            string script = "<script>HabilitarEnlaces1();</script>";
            ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);

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
                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));


                if (terminado == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#57F525"); //Verde
                }
                else if (Urgente == 1 && programadoVentas == 1)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                    e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
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
                    if (pausado == 1)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#08F4E2"); // Aqua
                    }

                    else
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1FF43");//amarillo 
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    }

                }


            }


        }


        protected void DataGridSolicitudPE_LinkButton(object source, DataGridCommandEventArgs e)
        {
            //Este Codigo se puede optimizar para no repetir el mismo proceso , solo cambiaria el Datagrid con otros metodos ma pequeños 

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
                string NombreProyecto = row.Cells[2].Text;
                string Asesor = row.Cells[3].Text;
                string FechaIngreso = row.Cells[4].Text;
                DateTime FechaIngresoForm = DateTime.Parse(FechaIngreso);
                string Dirigidoa = row.Cells[5].Text;
                string Tipo = row.Cells[6].Text;
                string RealizadoPor = row.Cells[7].Text;
                string termiVenta = row.Cells[8].Text;
                Session["ProVenSolicitud"] = termiVenta;




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

                lbNumeroSolicitud.Text = IdSolicitud;
                tbProyecto.Text = NombreProyecto;

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
                chxUrgente.Checked = false;


                if (termiVenta != "True")
                {
                    btnProgramarSolicitud.Enabled = true;
                    btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                    string script = "<script>HabilitarEnlaces1();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);
                }
                else
                {

                    btnProgramarSolicitud.Enabled = false;
                    btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                    string script = "<script>HabilitarEnlaces4();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);
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
                string NombreProyecto = row.Cells[2].Text;
                string Asesor = row.Cells[3].Text;
                string FechaIngreso = row.Cells[4].Text;
                DateTime FechaIngresoForm = DateTime.Parse(FechaIngreso);
                string Dirigidoa = row.Cells[5].Text;
                string Tipo = row.Cells[6].Text;
                string RealizadoPor = row.Cells[7].Text;
                string termiVenta = row.Cells[8].Text;
                Session["ProVenSolicitud"] = termiVenta;


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

                lbNumeroSolicitud.Text = IdSolicitud;
                tbProyecto.Text = NombreProyecto;

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
                chxUrgente.Checked = false;



                if (termiVenta != "True")
                {
                    btnProgramarSolicitud.Enabled = true;
                    btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                    string script = "<script>HabilitarEnlaces1();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);
                }
                else
                {

                    btnProgramarSolicitud.Enabled = false;
                    btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                    string script = "<script>HabilitarEnlaces4();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);
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
                Session["ProVenSolicitud"] = termiVenta;


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

                lbNumeroSolicitud.Text = IdSolicitud;
                tbProyecto.Text = NombreProyecto;

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
                chxUrgente.Checked = false;

                if (termiVenta != "True")
                {
                    btnProgramarSolicitud.Enabled = true;
                    btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                    string script = "<script>HabilitarEnlaces1();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);
                }
                else
                {

                    btnProgramarSolicitud.Enabled = false;
                    btnProgramarSolicitud.CssClass = "btn btn btn-warning";

                    string script = "<script>HabilitarEnlaces4();</script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces4", script, false);
                }





            }


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
                                Session["numeroSolicitudSession"] = IdSolicitud;

                                Session["ScriptEspecifico"] = "ActivarBotonDetalle();";
                            
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

            else if(insertUpdate == "Actualizar")
            {

                //CalcularFechaEntregaSolicitudEspecial() de momento se envia fecha del primer dia del año  !!!!IMPORTANTE !!!!

                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("sp_ActualizarSolicitudDiseñoEspecial", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;



                        cmd.Parameters.AddWithValue("@ID_Solicitud", Session["Id_Solicitud"].ToString());
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

                            Session["ScriptEspecifico"] = "ActivarBotonDetalle();";

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


                    if (tbCliente.Text != "" )
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

            while (UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Saturday || UltimaActivacionSolicitud.DayOfWeek == DayOfWeek.Sunday)
            {
                UltimaActivacionSolicitud = UltimaActivacionSolicitud.AddDays(1);
                UltimaActivacionSolicitud = new DateTime(UltimaActivacionSolicitud.Year, UltimaActivacionSolicitud.Month, UltimaActivacionSolicitud.Day, 8, 0, 0);
            }
            DateTime FechaEntrega = UltimaActivacionSolicitud.AddDays(5);

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
                    command.Parameters.AddWithValue("@UltimaActivacionRender", UltimaActivacionSolicitud);
                    command.Parameters.AddWithValue("@DiaSiguiente", DiaSiguiente);

                    int count = (int)command.ExecuteScalar(); // Ejecuta la consulta y obtén el resultado

                    if (count > 0)
                    {
                        // Si la fecha de entrega o el día siguiente son días feriados, agrega el número correcto de días adicionales a la fecha de entrega
                        FechaEntrega = FechaEntrega.AddDays(count);
                    }
                }
            }


            return FechaEntrega;
        }


        
        [WebMethod] // Cambiar estado de variable de Session cuando dan click en NuevaSolicitud 
        public static void NuevaSolicitud()
        {
            HttpContext.Current.Session["InsertUpdate"] = "Insertar";
        }

        [WebMethod]  // Cambiar estado de variable de Session cuando dan click en Modificarsolicitud 
        public static void ModificarSolicitud()
        {
            HttpContext.Current.Session["InsertUpdate"] = "Actualizar";
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
                string numeroFormateado = string.Format("{0:N0}", double.Parse(precioSugerido));
                string Proveedor = row.Cells[13].Text;
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



                Session["Id_Detalle"] = IdDetalle;
                lbIdDetalle.Text = IdDetalle;
                tbAncho.Text = Ancho;
                tbAltura.Text = Alto;
                tbProfundidad.Text = Profundidad;

                tbProveedor.Text = Proveedor;
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

                                string mensajePersonalizado = "El detalle " + Session["Id_Detalle"].ToString() + " se eliminó con éxito,";
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
           

            if (Session["InsertUpdateDetalle"].ToString() == "Insertar")
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

            else if(Session["InsertUpdateDetalle"].ToString() == "Actualizar")
            {

                //CalcularFechaEntregaSolicitudEspecial() de momento se envia fecha del primer dia del año  !!!!IMPORTANTE !!!!

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
                            Session["CantidadSession"] =tbCantidad.Text;
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

    }
}