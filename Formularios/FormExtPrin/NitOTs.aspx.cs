using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes;
using CheckBox = DocumentFormat.OpenXml.Wordprocessing.CheckBox;
using ListItem = System.Web.UI.WebControls.ListItem;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class NitOTs : System.Web.UI.Page
    {
        private List<TextBox> listaTextBoxes;
        private List<DropDownList> listaDropDownLists;
        private List<TextBox> listaTextBoxes1;

        private string CadenaConexionSID = "BD_SIDSQL";
       

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuariologueado"] != null)
            {

                listaTextBoxes = new List<TextBox>
                {
                   tbFechaCreacion,tbUltimaAct,tbCompartido,tbNumero,tbTelefono,tbFax,tbPriApellido,tbSegApellido,tbNombre,tbDireccion,tbSegApellido,tbCod,tbNombreContacto,tbDireccion1,
                   tbMailContacto,tbTelefono1,tbCelular,tbSede

                };

                listaDropDownLists = new List<DropDownList>
                {
                   ddlNaturaleza,ddlTipoDoc,ddlTipoCliente,ddlActividad,ddlSector,ddlZona,ddlFormaPago,ddlRegIva,ddlCiudad,ddlCiudad1

                };


                listaTextBoxes1 = new List<TextBox>
                {
                   tbNombreContacto,tbCelular,tbMailContacto,tbNombreContacto,tbTelefono1,tbSede,tbDireccion1

                };

                if (!IsPostBack)
                {


                    DeshabilitarTextBoxes(listaTextBoxes);
                    DeshabilitarDropDownLists(listaDropDownLists);
                    DisposicionBotonesIniciales();
                    CargarActividadesEnDropDownList();
                    DeshabilitarCheckBoxes();
                    CargarCiudadesEnDropDownList();
                    CargarCiudades1EnDropDownList();
                    DepartamentoAsesor();

                }




            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }

        }

        public class Asesor
        {
            public string Nombre { get; set; }
        }

        public int PermisoEmpleado()
        {

            string consultaActual = "select ID_Permiso  from tblPermiso_Empleado As A Inner join tblEmpleado AS B on  B.Cedula = A.ID_Empleado" +
                                    " where A.ID_Empleado = @Cedula And A.ID_Permiso = '6'";
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
                        int permiso = (Int16)command.ExecuteScalar();
                        return permiso;
                    }
                    else
                    {

                        return 0;
                    }
                }
            }

        }   //Este metodo se puede llevar al login 
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

        } // Este  Metodo se podria cargar en el Login



        //Metodos de control Campos, botones  y llenado de Drodownlist 
        public void DeshabilitarTextBoxes(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox == tbNom || textBox == tbNom1)
                {
                    textBox.CssClass = "form-control ";

                }
                else
                {
                    textBox.Enabled = false;
                    textBox.CssClass = "form-control ";
                }

            }
        }

        public void HabilitarTextBoxes(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox == tbNom || textBox == tbNom1)
                {
                    textBox.CssClass = "form-control  ";


                }
                else
                {
                    textBox.Enabled = true;
                    textBox.CssClass = "form-control ";
                    textBox.Text = "";
                    if (textBox == tbFechaCreacion || textBox == tbUltimaAct )
                    {
                        textBox.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        textBox.Enabled = false;
                    }
                    else if (textBox == tbCompartido)
                    {
                        textBox.Text = "";
                        textBox.Enabled = false;
                    }

                }

            }
        }

        public void HabilitarTextBoxesMod(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {

                if (textBox == tbFechaCreacion || textBox == tbUltimaAct || textBox == tbCompartido || textBox == tbNumero)
                {
                    textBox.Enabled = false;
                }
                else
                {
                    textBox.Enabled = true;
                    textBox.CssClass = "form-control ";
                }




            }
        }

        protected void DisposicionBotonesIniciales()
        {
            btnVerRut.Enabled = false;
            btnVerRegCli.Enabled = false;
            btnActRut.Enabled = false;
            btnActRegCli.Enabled = false;
            btnNuevo.Enabled = true;
            btnGrabar.Enabled = false;
            btnModificar.Enabled = false;
            btnCancelar.Enabled = true;
            btnGrabarContacto.Enabled = false;
            btnModificarContacto.Enabled = false;
            btnNuevoContacto.Enabled = true;
            btnCancelar1.Enabled = true;

            btnVerRut.CssClass = "btn btn-outline-secondary ";
            btnVerRegCli.CssClass = "btn btn-outline-secondary";
            btnActRut.CssClass = "btn btn-outline-secondary";
            btnActRegCli.CssClass = "btn btn-outline-secondary ";
            btnGrabar.CssClass = "btn btn-outline-secondary ";
            btnModificar.CssClass = "btn btn-outline-secondary ";
            btnGrabarContacto.CssClass = "btn btn-outline-secondary ";
            btnModificarContacto.CssClass = "btn btn-outline-secondary ";

        }

        protected void ControlBotonesNuevoNodificarCliente()
        {
            btnNuevo.Enabled = false;
            btnNuevo.CssClass = "btn btn-outline-secondary";
            btnGrabar.Enabled = true;
            btnModificar.Enabled = false;
            btnModificar.CssClass = "btn btn-outline-secondary";
            btnActRegCli.Enabled = true;
            btnActRut.Enabled = true;
            btnVerRegCli.Enabled = false;
            btnVerRegCli.CssClass = "btn btn-outline-secondary";
            btnVerRut.Enabled = false;
            btnVerRut.CssClass = "btn btn-outline-secondary";

        }

        public void DeshabilitarDropDownLists(List<DropDownList> dropDownLists)
        {
            foreach (DropDownList dropDownList in dropDownLists)
            {
                dropDownList.Enabled = false;
                dropDownList.CssClass = "form-control";
            }
        }

        public void HabilitarDropDownLists(List<DropDownList> dropDownLists)
        {
            foreach (DropDownList dropDownList in dropDownLists)
            {
                dropDownList.Enabled = true;
                dropDownList.CssClass = "form-control";
                dropDownList.ClearSelection();
            }
        }

        public void HabilitarDropDownListsMod(List<DropDownList> dropDownLists)
        {
            foreach (DropDownList dropDownList in dropDownLists)
            {
                dropDownList.Enabled = true;
                dropDownList.CssClass = "form-control";

            }
        }

        public void DeshabilitarCheckBoxes()
        {
            chxCompartir.Enabled = false;
            chxAgenteRete.Enabled = false;
            chxAutoRete.Enabled = false;
            chxDeclarante.Enabled = false;
            chxGranContri.Enabled = false;
            chxReteIca.Enabled = false;
            chxExento.Enabled = false;


            chxCompartir.Checked = false;
            chxAgenteRete.Checked = false;
            chxAutoRete.Checked = false;
            chxDeclarante.Checked = false;
            chxGranContri.Checked = false;
            chxReteIca.Checked = false;
            chxExento.Checked = false;
        }

        public void HabilitarCheckBoxes()
        {

            chxAgenteRete.Enabled = true;
            chxAutoRete.Enabled = true;
            chxDeclarante.Enabled = true;
            chxGranContri.Enabled = true;
            chxReteIca.Enabled = true;
            chxExento.Enabled = true;

            chxAgenteRete.Checked = true;
            chxAutoRete.Checked = true;
            chxDeclarante.Checked = true;
            chxGranContri.Checked = true;
            chxReteIca.Checked = true;
            chxExento.Checked = true;


        }

        public void HabilitarCheckBoxesMod()
        {

            chxAgenteRete.Enabled = true;
            chxAutoRete.Enabled = true;
            chxDeclarante.Enabled = true;
            chxGranContri.Enabled = true;
            chxReteIca.Enabled = true;
            chxExento.Enabled = true;

        }

        public void LimpiarCampos()
        {

            tbFechaCreacion.Text = "";
            tbUltimaAct.Text = "";
            tbCompartido.Text = "";
            ddlNaturaleza.Text = "";
            ddlTipoDoc.Text = "";
            tbNumero.Text = "";
            ddlTipoCliente.Text = "";
            ddlActividad.Text = "";
            tbTelefono.Text = "";
            ddlSector.Text = "";
            tbFax.Text = "";
            tbPriApellido.Text = "";
            tbSegApellido.Text = "";
            tbNombre.Text = "";
            tbDireccion.Text = "";
            ddlCiudad.Text = "0";
            tbCod.Text = "";
            ddlZona.Text = "";
            ddlFormaPago.Text = "";
            ddlRegIva.Text = "";

        }

        static string ObtenerSubcadena(string cadena)
        {

            int indiceEspacio = cadena.LastIndexOf(' ');

            if (indiceEspacio != -1)
            {
                string subcadena = cadena.Substring(indiceEspacio + 1);
                subcadena = subcadena.Trim();
                return subcadena;
            }


            return cadena;
        }

        private void CargarActividadesEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "select Id, CONCAT(aecCodigo, ' - ',aecDescripcion) As Descripcion from tblActividadEconomica order by aecCodigo asc";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlActividad.DataSource = reader;
                ddlActividad.DataTextField = "Descripcion";
                ddlActividad.DataValueField = "Id";

                ddlActividad.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlActividad.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        private void CargarCiudadesEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = " SELECT concat(tblDepartamentoPais.CodigoDepartamento , tblCiudad.CodigoCiudad)   AS Codigo," +
                                  " concat(tblCiudad.NombreCiudad , ' - ' , tblDepartamentoPais.NombreDepartamento, " +
                                  "concat('  Cod: ',tblDepartamentoPais.CodigoDepartamento , tblCiudad.CodigoCiudad)) AS Ciudad" +
                                  " FROM tblDepartamentoPais " +
                                  " INNER JOIN tblCiudad ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento" +
                                  " ORDER BY tblCiudad.NombreCiudad asc, tblDepartamentoPais.NombreDepartamento asc";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlCiudad.DataSource = reader;
                ddlCiudad.DataTextField = "Ciudad";
                ddlCiudad.DataValueField = "Codigo";

                ddlCiudad.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlCiudad.Items.Insert(0, new ListItem("Seleccione", "0"));

        }

        private void CargarCiudades1EnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = " SELECT concat(tblDepartamentoPais.CodigoDepartamento , tblCiudad.CodigoCiudad)   AS Codigo," +
                                  " concat(tblCiudad.NombreCiudad , ' - ' , tblDepartamentoPais.NombreDepartamento, " +
                                  "concat('  Cod: ',tblDepartamentoPais.CodigoDepartamento , tblCiudad.CodigoCiudad)) AS Ciudad" +
                                  " FROM tblDepartamentoPais " +
                                  " INNER JOIN tblCiudad ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento" +
                                  " ORDER BY tblCiudad.NombreCiudad asc, tblDepartamentoPais.NombreDepartamento asc";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlCiudad1.DataSource = reader;
                ddlCiudad1.DataTextField = "Ciudad";
                ddlCiudad1.DataValueField = "Codigo";

                ddlCiudad1.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlCiudad1.Items.Insert(0, new ListItem("Seleccione", "0"));

        }

        protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtén el valor seleccionado en el DropDownList
            string valorSeleccionado = ddlCiudad.SelectedItem.Text;

            // Extrae los últimos 5 caracteres
            string ultimosCincoCaracteres = valorSeleccionado.Length >= 5 ? valorSeleccionado.Substring(valorSeleccionado.Length - 5) : valorSeleccionado;

            // Asigna el valor al TextBox
            tbCod.Text = ultimosCincoCaracteres;
            tbCod.Enabled = false;
            tbCod.CssClass = "form-control";

            string script = @"cambioNaturalezaCliente();";
            ScriptManager.RegisterStartupScript(this, GetType(), "cambioNaturalezaCliente", script, true);

        }




        //LOGICA CLIENTES FACTURACION  

        protected void DatagridClientes_LinkButton(object source, DataGridCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "VerContactoCliente")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    DataGridItem row = DatagridClientes.Items[rowIndex];

                    // Accede al DataView del DataSource para obtener la fila correspondiente
                    DataView dataView = (DataView)ClientesFacturacion.Select(DataSourceSelectArguments.Empty);

                    if (dataView != null && dataView.Count > rowIndex)
                    {
                        DataRowView rowView = dataView[rowIndex];

                        DateTime fechaCreacion = Convert.ToDateTime(rowView["FechaCreacion"]);
                        DateTime fechaUul = Convert.ToDateTime(rowView["UltimaActualizacion"]);
                        string compartidCon = rowView["CompartidoCon"].ToString();
                        string naturaleza = rowView["Naturaleza"].ToString();
                        string tipoDoc = rowView["Tipo_Documento"].ToString();
                        string id = rowView["Nit"].ToString();
                        string tipoCli = rowView["Tipo_Cliente"].ToString();
                        string actividad = rowView["Actividad"].ToString();
                        string telefono = rowView["Telefono"].ToString();
                        string sector = rowView["Sector"].ToString();
                        string razonSocial = rowView["RazonSocial"].ToString();
                        string segundoApellido = rowView["SegundoApellido"].ToString();
                        string nombre = rowView["Nombre"].ToString();
                        string direccion = rowView["Direccion"].ToString();
                        string ciudad = rowView["Ciudad"].ToString();
                        //string codigo = rowView["RazonSocial"].ToString();
                        string Pago = rowView["Forma_Pago"].ToString();
                        string Zona = rowView["Zona"].ToString();
                        string fax = rowView["Fax"].ToString();
                        string AgenteRetenedor = rowView["AgenteRetenedor"].ToString();
                        string GranContribuyente = rowView["GranContribuyente"].ToString();
                        string AutoRetenedor = rowView["AutoRetenedor"].ToString();
                        string ExentoRetencion = rowView["ExentodeRetencion"].ToString();
                        string DeclaranteRenta = rowView["DeclaranteRenta"].ToString();
                        string RetenedorICA = rowView["RetenedorICA"].ToString();
                        string RegimenIva = rowView["RegimenIVA"].ToString();
                        string ArchivoRut = rowView["ArchivoRUT"].ToString();
                        string ArchivoReg = rowView["ArchivoRegistro"].ToString();

                        Session["ArchivoRutSession"] = ArchivoRut;
                        Session["ArchivoRegSession"] = ArchivoReg;
                        Session["IdClienteFactSession"] = id; // Variable que almacena el NIT del Cliente 
                        tbFechaCreacion.Text = fechaCreacion.ToString("yyyy-MM-dd");
                        tbUltimaAct.Text = fechaUul.ToString("yyyy-MM-dd");
                        tbCompartido.Text = compartidCon;
                        foreach (ListItem item in ddlNaturaleza.Items)
                        {
                            if (item.Text == naturaleza)
                            {
                                ddlNaturaleza.ClearSelection();
                                item.Selected = true;

                                if(naturaleza == "J - Juridica")
                                {
                                    lbPriApellido.Text = "Razón Social";

                                    lbSegApellido.Visible = false;
                                    tbSegApellido.Visible = false;

                                    lbNombre.Visible = false;
                                    tbNombre.Visible = false;
                                }
                                else
                                {
                                    lbPriApellido.Text = "Primer Apellido";


                                    lbSegApellido.Visible = true;
                                    tbSegApellido.Visible = true;

                                    lbNombre.Visible = true;
                                    tbNombre.Visible = true;
                                }


                                break;
                            }
                        }
                        foreach (ListItem item in ddlTipoDoc.Items)
                        {
                            if (item.Text == tipoDoc)
                            {
                                ddlTipoDoc.ClearSelection();
                                item.Selected = true;
                                break;
                            }
                        }
                        tbNumero.Text = id;
                        foreach (ListItem item in ddlTipoCliente.Items)
                        {
                            if (item.Text == tipoCli)
                            {
                                ddlTipoCliente.ClearSelection();
                                item.Selected = true;
                                break;
                            }
                        }
                        foreach (ListItem item in ddlActividad.Items)
                        {
                            if (item.Text == actividad)
                            {
                                ddlActividad.ClearSelection();
                                item.Selected = true;
                                break;
                            }
                        }
                        tbTelefono.Text = telefono;
                        foreach (ListItem item in ddlSector.Items)
                        {
                            if (item.Text == sector)
                            {
                                ddlSector.ClearSelection();
                                item.Selected = true;
                                break;
                            }
                        }
                        tbPriApellido.Text = razonSocial;
                        tbSegApellido.Text = segundoApellido;
                        tbNombre.Text = nombre;
                        tbDireccion.Text = direccion;
                        foreach (ListItem item1 in ddlCiudad.Items)
                        {
                            if (item1.Text == ciudad)
                            {
                                ddlCiudad.ClearSelection();
                                item1.Selected = true;
                                break;
                            }
                        }
                        foreach (ListItem item1 in ddlFormaPago.Items)
                        {
                            if (item1.Text == Pago)
                            {
                                ddlFormaPago.ClearSelection();
                                item1.Selected = true;
                                break;
                            }
                        }
                        foreach (ListItem item1 in ddlZona.Items)
                        {
                            if (item1.Text == Zona)
                            {
                                ddlZona.ClearSelection();
                                item1.Selected = true;
                                break;
                            }
                        }
                        tbFax.Text = fax;
                        tbCod.Text = ObtenerSubcadena(ciudad);

                        chxAgenteRete.Checked = Convert.ToBoolean(AgenteRetenedor);
                        chxGranContri.Checked = Convert.ToBoolean(GranContribuyente);
                        chxAutoRete.Checked = Convert.ToBoolean(AutoRetenedor);
                        chxExento.Checked = Convert.ToBoolean(ExentoRetencion);
                        chxDeclarante.Checked = Convert.ToBoolean(DeclaranteRenta);
                        chxReteIca.Checked = Convert.ToBoolean(RetenedorICA);
                        foreach (ListItem item1 in ddlRegIva.Items)
                        {
                            if (item1.Text == RegimenIva)
                            {
                                ddlRegIva.ClearSelection();
                                item1.Selected = true;
                                break;
                            }
                        }
                        // Se utiliza para darle el color solo a la fila seleccionada 
                        foreach (DataGridItem item in DatagridClientes.Items)
                        {
                            if (item != row)
                            {
                                item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                            }
                        }

                        //se usa Para darle un color a la fila seleccionada  anderson
                        e.Item.CssClass = "fila-seleccionada";


                        DataGridContacto.DataBind();
                        DataGridVentaAsesor.DataBind();
                        PanelContacto.Update();

                        // Accede al DataView del segundo DataSource para obtener todas las cedulas
                        DataView datosView1 = (DataView)UltimasVentas.Select(DataSourceSelectArguments.Empty);

                        if (datosView1 != null)
                        {
                            List<string> cedulasList = new List<string>();

                            if(datosView1.Count > 0)
                            {
                                // Obtener la primera fila del DataView
                                DataRowView primeraFila = datosView1[0];

                                // Obtener la cedula de la primera fila
                                string cedula = primeraFila["Cedula"].ToString();
                                cedulasList.Add(cedula);

                            }
                            

                            // Convierte la lista de cédulas a un array si es necesario
                            string[] cedulasArray = cedulasList.ToArray();
                            int persmiso = PermisoEmpleado();                                                                                                          
                            if (cedulasArray.Contains(Session["CedulaLogeada"].ToString()) && Session["Departamento"]?.ToString().ToUpper() == "VENTAS" || persmiso == 6 || cedulasArray.Length == 0 ||  compartidCon.Contains(Session["usuariologueado"].ToString()))
                            {
                                // Se muestra el div contenedor de la informacion de contacto  y se activan los botones 
                                Contacto.Visible = true;

                                // Se activan los botones 
                                btnModificar.Enabled = true;
                                btnNuevoContacto.Enabled = true;
                                btnCancelar1.Enabled = true;

                                // Se oculta el mensaje de administracion cliente 
                                EstMensaje.Visible = false;

                            }
                            else
                            {
                                EstMensaje.Visible = true;
                                btnModificar.Enabled = false;
                                btnModificar.CssClass = "btn btn-outline-secondary";
                                Contacto.Visible = false;
                                tbTelefono.Text = "";
                                tbDireccion.Text = "";
                                tbFax.Text = "";
                            }

                            // estos botones se activan sin importar si es o no cliente de ese asesor 
                            btnVerRut.Enabled = true;
                            btnVerRegCli.Enabled = true;

                        }

                    }
                }
            }
            catch (Exception ex)
            {

                MensajeError.Text = "Ocurrió un error al procesar la información. Por favor, inténtelo nuevamente o comuníquese con el soporte técnico.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }




        }

        protected void NuevoClienteFact(object sender, EventArgs e)
        {
            HabilitarTextBoxes(listaTextBoxes);
            HabilitarDropDownLists(listaDropDownLists);
            HabilitarCheckBoxes();
            ControlBotonesNuevoNodificarCliente();
            Session["GuardarClienteFact"] = "Insertar";

        }

        protected void MoficarClienteFact(object sender, EventArgs e)
        {
            HabilitarTextBoxesMod(listaTextBoxes);
            HabilitarDropDownListsMod(listaDropDownLists);
            HabilitarCheckBoxesMod();
            ControlBotonesNuevoNodificarCliente();
            chxCompartir.Enabled = true;
            Session["GuardarClienteFact"] = "Actualizar";

        }

        protected void CancelarClienteFact(object sender, EventArgs e)
        {
            DeshabilitarCheckBoxes();
            DeshabilitarDropDownLists(listaDropDownLists);
            DeshabilitarTextBoxes(listaTextBoxes);
            DisposicionBotonesIniciales();
            UltimasVentas.DataBind();
            LimpiarCampos();
        }

        //Metodo para ver el RUT ClienteObra
        protected void VerRut(object sender, EventArgs e)
        {

            string ArchivoRutServidor = Session["ArchivoRutSession"].ToString();

            // Ruta completa del archivo que deseas abrir
            string rutaArchivo = @"\\172.16.30.6\s_i_ducon$\RUT\" + ArchivoRutServidor;

            //string rutaArchivo = @"P:\SISTEMAS\PruebaDocumentacion\RUT\" + ArchivoRutServidor;

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
                    string mensajePersonalizado = "Este cliente no cuenta con RUT en el servidor.";
                    string urlRedireccion = "FormExtPrin/NitOTs.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                   
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Se ha producido un error al intentar abrir el archivo. " + ex.Message + "');", true);

            }


        }

        //Metodo para ver el Registro ClienteObra
        protected void VerRegistro(object sender, EventArgs e)
        {

            string ArchivoRegistroServidor ="R" + Session["ArchivoRegSession"].ToString(); ;

            // Ruta completa del archivo que deseas abrir
            string rutaArchivo = @"\\172.16.30.6\s_i_ducon$\Registro Clientes\" + ArchivoRegistroServidor;

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
                    string mensajePersonalizado = "Este cliente no cuenta con Registro en el servidor.";
                    string urlRedireccion = "FormExtPrin/NitOTs.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                  
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Se ha producido un error al intentar abrir el archivo. " + ex.Message + "');", true);


            }


        }

        // Metodo de validacion existencia de cliente 
        public bool ValidarCliente(string Nit)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Consulta para validar si el NIT ya existe
                string consultaValidacion = "SELECT COUNT(*) FROM tblClienteObra  WHERE Nit = @Nit";

                using (SqlCommand cmdValidacion = new SqlCommand(consultaValidacion, connection))
                {
                    cmdValidacion.Parameters.AddWithValue("@Nit", Nit); // Modificado para utilizar el parámetro Nit
                    connection.Open();

                    int count = (int)cmdValidacion.ExecuteScalar();

                    // Si count es mayor que 0, significa que hay registros con el NIT proporcionado
                    return count > 0;
                }
            }
        }

        // Metodo de Insercion Cliente Obra
        public bool InsertarClienteEnBaseDeDatos()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                string procedimientoAlmacenado = "sp_InsertarClienteObra";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmdInsertar = new SqlCommand(procedimientoAlmacenado, connection))
                    {
                        cmdInsertar.CommandType = CommandType.StoredProcedure;
                        cmdInsertar.Parameters.AddWithValue("@Naturaleza", ddlNaturaleza.Text);
                        cmdInsertar.Parameters.AddWithValue("@Tipo_Documento", ddlTipoDoc.Text);
                        cmdInsertar.Parameters.AddWithValue("@Nit", tbNumero.Text);
                        cmdInsertar.Parameters.AddWithValue("@Actividad", ddlActividad.SelectedItem.Text);
                        cmdInsertar.Parameters.AddWithValue("@Tipo_Cliente", ddlTipoCliente.Text);
                        cmdInsertar.Parameters.AddWithValue("@RazonSocial", tbPriApellido.Text);
                        cmdInsertar.Parameters.AddWithValue("@SegundoApellido", tbSegApellido.Text);
                        cmdInsertar.Parameters.AddWithValue("@Nombre", tbNombre.Text);
                        cmdInsertar.Parameters.AddWithValue("@Direccion", tbDireccion.Text);
                        cmdInsertar.Parameters.AddWithValue("@Ciudad", ddlCiudad.SelectedItem.Text);
                        cmdInsertar.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                        cmdInsertar.Parameters.AddWithValue("@Fax", tbFax.Text);
                        cmdInsertar.Parameters.AddWithValue("@Forma_Pago", ddlFormaPago.Text);
                        cmdInsertar.Parameters.AddWithValue("@Zona", ddlZona.Text);
                        cmdInsertar.Parameters.AddWithValue("@AgenteRetenedor", chxAgenteRete.Checked);
                        cmdInsertar.Parameters.AddWithValue("@GranContribuyente", chxGranContri.Checked);
                        cmdInsertar.Parameters.AddWithValue("@AutoRetenedor", chxAutoRete.Checked);
                        cmdInsertar.Parameters.AddWithValue("@ExentodeRetencion", chxExento.Checked);
                        cmdInsertar.Parameters.AddWithValue("@DeclaranteRenta", chxDeclarante.Checked);
                        cmdInsertar.Parameters.AddWithValue("@RetenedorICA", chxReteIca.Checked);
                        cmdInsertar.Parameters.AddWithValue("@RegimenIVA", ddlRegIva.Text);
                        cmdInsertar.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                        cmdInsertar.Parameters.AddWithValue("@UltimaActualizacion", DateTime.Now);
                        cmdInsertar.Parameters.AddWithValue("@ArchivoRegistro", tbNumero.Text + Path.GetExtension(btnActRegCli.FileName));
                        cmdInsertar.Parameters.AddWithValue("@ArchivoRut", tbNumero.Text + Path.GetExtension(btnActRut.FileName));
                        cmdInsertar.Parameters.AddWithValue("@Sector", ddlSector.Text);

                        connection.Open();
                        int count = cmdInsertar.ExecuteNonQuery();

                        return count > 0; // Retorna true si se afectó al menos un registro, indicando éxito.
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Se ha producido un error al intentar realizar la inserción en la base de datos, intentelo nuevamente mas tarde, o comuniquese con el departamento de sistemas  " + ex.Message + "');", true);
                return false;
            }
        }

        // Metodo para realizar el guadado de archivos en el servidor
        private void GuardarArchivosEnCarpetaServidor(string carpeta, string rutaBase, FileUpload archivo)
        {
            // Combinar la ruta base con el nombre de la carpeta
            string rutaCompletaCarpeta = Path.Combine(rutaBase, carpeta);

            // Verificar si la carpeta existe
            if (!Directory.Exists(rutaCompletaCarpeta))
            {
                try
                {
                    // Si no existe, crear la carpeta
                    Directory.CreateDirectory(rutaCompletaCarpeta);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Se ha producido un error al intentar crear la carpeta en el servidor. " + ex.Message + "');", true);
                    return;
                }
            }

            string extensionArchivo = "";
            string nuevoNombreArchivo = "";
            string rutaCompletaArchivo = "";

            if (carpeta.ToUpper() == "RUT")
            {
                // Obtener la extensión del archivo original
                 extensionArchivo = Path.GetExtension(archivo.FileName);

                // Construir el nuevo nombre del archivo con la nueva extensión
                 nuevoNombreArchivo = tbNumero.Text + extensionArchivo;

                // Combinar la ruta completa de la carpeta con el nuevo nombre del archivo
                 rutaCompletaArchivo = Path.Combine(rutaCompletaCarpeta, nuevoNombreArchivo);
            }
            else
            {
                // Obtener la extensión del archivo original
                 extensionArchivo = Path.GetExtension(archivo.FileName);

                // Construir el nuevo nombre del archivo con la nueva extensión
                 nuevoNombreArchivo = "R" + tbNumero.Text + extensionArchivo;

                // Combinar la ruta completa de la carpeta con el nuevo nombre del archivo
                 rutaCompletaArchivo = Path.Combine(rutaCompletaCarpeta, nuevoNombreArchivo);
            }
            

            try
            {
                // Guardar el archivo con el nuevo nombre
                archivo.SaveAs(rutaCompletaArchivo);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Se ha producido un error al intentar guardar el archivo en el servidor. Vuelve a intentarlo más tarde o comunícate con sistemas " + ex.Message + "');", true);
            }
        }

        // metodo para eliminar archivos del servidor 
        private void EliminarArchivoDelServidor(string carpeta, string nombreArchivo)
        {
            try
            {
                string rutaBase = @"\\172.16.30.6\s_i_ducon$";
                string rutaCompletaCarpeta = Path.Combine(rutaBase, carpeta);
                if(carpeta.ToUpper() == "REGISTRO CLIENTES")
                {
                    nombreArchivo = "R" + nombreArchivo;
                }

                if (Directory.Exists(rutaCompletaCarpeta))
                {
                    string rutaCompletaArchivo = Path.Combine(rutaCompletaCarpeta, nombreArchivo);

                    if (File.Exists(rutaCompletaArchivo))
                    {
                        File.Delete(rutaCompletaArchivo);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Se ha producido un error al intentar eliminar el archivo del servidor. " + ex.Message + "');", true);
            }
        }

        //metodo para realizar la actualizacion cliente Obra
        private bool ActualizarClienteEnBaseDeDatos()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string procedimientoAlmacenado = "Sp_ActualizarClienteObra";

                    using (SqlCommand cmdInsertar = new SqlCommand(procedimientoAlmacenado, connection))
                    {
                        cmdInsertar.CommandType = CommandType.StoredProcedure;

                        cmdInsertar.Parameters.AddWithValue("@Nit", Session["IdClienteFactSession"].ToString());
                        cmdInsertar.Parameters.AddWithValue("@RazonSocial", tbPriApellido.Text);
                        cmdInsertar.Parameters.AddWithValue("@SegundoApellido", tbSegApellido.Text);
                        cmdInsertar.Parameters.AddWithValue("@Nombre", tbNombre.Text);
                        cmdInsertar.Parameters.AddWithValue("@Direccion", tbDireccion.Text);
                        cmdInsertar.Parameters.AddWithValue("@Ciudad", ddlCiudad.SelectedItem.Text);
                        cmdInsertar.Parameters.AddWithValue("@Telefono", tbTelefono.Text);
                        cmdInsertar.Parameters.AddWithValue("@Fax", tbFax.Text);
                        cmdInsertar.Parameters.AddWithValue("@Naturaleza", ddlNaturaleza.Text);
                        cmdInsertar.Parameters.AddWithValue("@Tipo_Documento", ddlTipoDoc.Text);
                        cmdInsertar.Parameters.AddWithValue("@Actividad", ddlActividad.SelectedItem.Text);
                        cmdInsertar.Parameters.AddWithValue("@Forma_Pago", ddlFormaPago.Text);
                        cmdInsertar.Parameters.AddWithValue("@Tipo_Cliente", ddlTipoCliente.Text);

                        cmdInsertar.Parameters.AddWithValue("@Zona", ddlZona.Text);
                        cmdInsertar.Parameters.AddWithValue("@AgenteRetenedor", chxAgenteRete.Checked);
                        cmdInsertar.Parameters.AddWithValue("@GranContribuyente", chxGranContri.Checked);
                        cmdInsertar.Parameters.AddWithValue("@AutoRetenedor", chxAutoRete.Checked);
                        cmdInsertar.Parameters.AddWithValue("@ExentodeRetencion", chxExento.Checked);
                        cmdInsertar.Parameters.AddWithValue("@DeclaranteRenta", chxDeclarante.Checked);
                        cmdInsertar.Parameters.AddWithValue("@RetenedorICA", chxReteIca.Checked);
                        cmdInsertar.Parameters.AddWithValue("@RegimenIVA", ddlRegIva.Text);
                        cmdInsertar.Parameters.AddWithValue("@ArchivoRut", tbNumero.Text + Path.GetExtension(btnActRut.FileName));
                        cmdInsertar.Parameters.AddWithValue("@ArchivoRegistro", tbNumero.Text + Path.GetExtension(btnActRegCli.FileName));
                        cmdInsertar.Parameters.AddWithValue("@Sector", ddlSector.Text);
                        cmdInsertar.Parameters.AddWithValue("@UltimaActualizacion", DateTime.Now);

                        int affectedRows = cmdInsertar.ExecuteNonQuery();

                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('Se ha producido un error al intentar actualizar el cliente en la base de datos. " + ex.Message + "');", true);
                return false;
            }
        }

        protected void GuardarModificarClienteFact(object sender, EventArgs e)
        {

            if (btnActRut.HasFile)
            {
                if (btnActRegCli.HasFile)
                {
                    // Obtener el tamaño máximo permitido en bytes(por ejemplo, 30 MB)
                    int maxSizeBytes = 30 * 1024 * 1024; // 30 MB

                    // Verificar si el tamaño del archivo excede el límite permitido
                    if (btnActRut.PostedFile.ContentLength > maxSizeBytes)
                    {
                        string mensajePersonalizado = "El tamaño del archivo del RUT excede el límite permitido de 10 MB.";
                        string urlRedireccion = "FormExtPrin/NitOTs.aspxx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                    }

                    // Verificar si el tamaño del archivo excede el límite permitido
                    if (btnActRegCli.PostedFile.ContentLength > maxSizeBytes)
                    {
                        string mensajePersonalizado = "El tamaño del archivo del Registro excede el límite permitido de 10 MB";
                        string urlRedireccion = "FormExtPrin/NitOTs.aspx";
                        Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                    }

                    if (Session["GuardarClienteFact"].ToString() == "Insertar")
                    {

                        // Se valida que el cliente no este creado 
                        if (ValidarCliente(tbNumero.Text))
                        {
                            // si ya esta creado me muestra mensaje de error 
                            string mensajeError = "El NIT ya existe en nuestra base de datos. Por favor, verifica el número y realiza la corrección necesaria. Gracias..";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('" + mensajeError + "');", true);
                        }
                        else
                        {
                            // CLiente no existe  se procede a la inserción 
                            if (InsertarClienteEnBaseDeDatos())
                            {
                                // Se invoca el Metodo para guardar archivos en el servidor 
                                GuardarArchivosEnCarpetaServidor("RUT", @"\\172.16.30.6\s_i_ducon$", btnActRut);
                                 //GuardarArchivosEnCarpetaServidor("RUT", @"P:\SISTEMAS\PruebaDocumentacion", btnActRut);

                                GuardarArchivosEnCarpetaServidor("Registro Clientes", @"\\172.16.30.6\s_i_ducon$", btnActRegCli);
                                  //GuardarArchivosEnCarpetaServidor("RegistroClientes", @"P:\SISTEMAS\PruebaDocumentacion", btnActRegCli);

                                string mensajePersonalizado = "Cliente creado exitosamente.";
                                string urlRedireccion = "FormExtPrin/NitOTs.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                            }
                            else
                            {
                                string mensajeError = "Hubo un problema al intentar insertar el cliente, intentalo mas tarde o comunicate con el departamento de sistemas.";
                                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('" + mensajeError + "');", true);
                            }

                        }

                    }
                    else if (Session["GuardarClienteFact"].ToString() == "Actualizar")
                    {
                        // Se valida que el cliente exista para ser modificado 
                        if (ValidarCliente(tbNumero.Text))
                        {
                            // Se eliminan los archivos del servidor 
                            DataView DatoCliente = (DataView)ClientesFacturacion.Select(DataSourceSelectArguments.Empty);

                            if (DatoCliente != null && DatoCliente.Count > 0)
                            {
                                // Accede a la primera fila del DataView

                                DataRow fila = DatoCliente.Table.Rows[0];
                                if (fila["ArchivoRUT"] != null)
                                {
                                    string nombreRut = fila["ArchivoRUT"].ToString();
                                    EliminarArchivoDelServidor("RUT", nombreRut);
                                }
                                if (fila["ArchivoRegistro"] != null)
                                {
                                    string nombreReg = fila["ArchivoRegistro"].ToString();
                                    EliminarArchivoDelServidor("Registro Clientes", nombreReg);
                                }
                            }
                            if (ActualizarClienteEnBaseDeDatos())
                            {

                                // Se invoca el Metodo para guardar archivos en el servidor 
                                GuardarArchivosEnCarpetaServidor("RUT", @"\\172.16.30.6\s_i_ducon$", btnActRut);
                                GuardarArchivosEnCarpetaServidor("Registro Clientes", @"\\172.16.30.6\s_i_ducon$", btnActRegCli);

                                // Puedes mostrar un mensaje de éxito u otra información si es necesario
                                string mensajePersonalizado = "Cliente actualizado exitosamente.";
                                string urlRedireccion = "FormExtPrin/NitOTs.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                            }
                            else
                            {

                            }
                        }
                   
                    }

                }
                else
                {
                    string mensajeError = "Hubo un problema al cargar el Registro, intentalo nuevamente mas tarde o comunicate con el departamento de sistemas.";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('" + mensajeError + "');", true);
                }
            }
            else
            {
                string mensajeError = "Hubo un problema al cargar el Rut, intentalo nuevamente mas tarde o comunicate con el departamento de sistemas.";
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('" + mensajeError + "');", true);
            }


        }



        //  LOGICA PARA COMPARTIR CLINETES POR DIFERENTES ASESORES 
        protected void ChxCompartir_CheckedChanged(object sender, EventArgs e)
        {
            // Obtiene la cadena de nombres Asesores Compartidos
            string nombresAsesores = tbCompartido.Text; // Reemplaza esto con tu lógica de obtención de datos

            // Dividir la cadena en un arreglo de nombres
            string[] arregloNombres = nombresAsesores.Split(';');

            // Crea una lista de objetos Asesor y agrega los nombres
            List<Asesor> asesores = new List<Asesor>();
            foreach (string nombre in arregloNombres)
            {
                asesores.Add(new Asesor { Nombre = nombre });
            }

            // Asigna la lista como origen de datos para el DataGrid
            DataGridClienteCompart.DataSource = asesores;
            DataGridClienteCompart.DataBind();


            if (chxCompartir.Checked)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#myModal1').modal('show');", true);
            }
        }

        protected void AgregarAsesorCompartir(object sender, EventArgs e)
        {
            string nuevoNombreAsesor = tbNombreAsesor3.Text; // Reemplaza con el nombre del nuevo asesor a agregar
            string cedulaCliente = tbNumero.Text; // Reemplaza con la cédula del cliente

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Verificar si el cliente existe
                    using (SqlCommand cmdCliente = new SqlCommand("SELECT COUNT(*) FROM tblClienteObra WHERE Nit = @Nit", connection))
                    {
                        cmdCliente.CommandType = CommandType.Text;
                        cmdCliente.Parameters.AddWithValue("@Nit", cedulaCliente);
                        connection.Open();

                        int countCliente = (int)cmdCliente.ExecuteScalar();

                        if (countCliente > 0)
                        {
                            // El cliente existe, ahora verifica si el asesor ya está en la lista
                            using (SqlCommand cmdAsesor = new SqlCommand("SELECT CompartidoCon FROM tblClienteObra WHERE Nit = @Nit", connection))
                            {
                                cmdAsesor.CommandType = CommandType.Text;
                                cmdAsesor.Parameters.AddWithValue("@Nit", cedulaCliente);

                                // Obtiene la lista actual de asesores compartidos
                                string asesoresActuales = cmdAsesor.ExecuteScalar()?.ToString();

                                if (asesoresActuales != null && asesoresActuales.Contains(nuevoNombreAsesor))
                                {
                                    // El asesor ya está en la lista

                                    // Cerrar el modal después de agregar el asesor
                                    ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);

                                    // Refrescar la página después de cerrar el modal
                                    ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);

                                    string mensajeExito = "El Asesor " + tbNombreAsesor3.Text.Trim() + " se encuentra compartido actualmente.";
                                    string scriptExito = "alert('" + mensajeExito + "');";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

                                }
                                else
                                {


                                    // Actualiza la base de datos con la nueva lista de asesores
                                    asesoresActuales += ";" + nuevoNombreAsesor;
                                    asesoresActuales = asesoresActuales.TrimStart(';');

                                    string consultaActualizar = "UPDATE tblClienteObra SET CompartidoCon = @NuevaCadena WHERE Nit = @Cedula";

                                    using (SqlCommand command = new SqlCommand(consultaActualizar, connection))
                                    {
                                        command.Parameters.AddWithValue("@NuevaCadena", asesoresActuales);
                                        command.Parameters.AddWithValue("@Cedula", cedulaCliente);
                                        command.ExecuteNonQuery();
                                    }

                                    DataGridCompartirCliente.DataBind();
                                    DataGridClienteCompart.DataBind();

                                    // Mostrar mensaje de éxito

                                    // Cerrar el modal después de agregar el asesor
                                    ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);

                                    // Refrescar la página después de cerrar el modal
                                    ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);

                                    string mensajeExito = "El Asesor " + tbNombreAsesor3.Text.Trim() + " ha sido Agregado  exitosamente.";
                                    string scriptExito = "alert('" + mensajeExito + "');";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);


                                }
                            }
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);
                            ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);
                            string mensajeExito = "Error en la consulta a la base de datos. Inténtalo más tarde o contacta soporte.";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Cerrar el modal después de agregar el asesor
                ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);

                // Refrescar la página después de cerrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);

                // Mostrar mensaje de éxito
                string mensajeExito = "Error en la consulta a la base de datos.Inténtalo más tarde o contacta soporte.";
                string scriptExito = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
            }
        }

        protected void EliminarAsesorCompartir(object sender, EventArgs e)

        {
            string nombreAsesorEliminar = tbNombreAsesor3.Text; // Reemplaza con el nombre del asesor a eliminar
            string cedulaClienteEliminar = tbNumero.Text; // Reemplaza con la cédula del cliente

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Verificar si el cliente existe
                    using (SqlCommand cmdCliente = new SqlCommand("SELECT COUNT(*) FROM tblClienteObra WHERE Nit = @Nit", connection))
                    {
                        cmdCliente.CommandType = CommandType.Text;
                        cmdCliente.Parameters.AddWithValue("@Nit", cedulaClienteEliminar);
                        connection.Open();

                        int countCliente = (int)cmdCliente.ExecuteScalar();

                        if (countCliente > 0)
                        {
                            // El cliente existe, ahora verifica si el asesor está en la lista
                            using (SqlCommand cmdAsesor = new SqlCommand("SELECT CompartidoCon FROM tblClienteObra WHERE Nit = @Nit", connection))
                            {
                                cmdAsesor.CommandType = CommandType.Text;
                                cmdAsesor.Parameters.AddWithValue("@Nit", cedulaClienteEliminar);

                                // Obtiene la lista actual de asesores compartidos
                                string asesoresActuales = cmdAsesor.ExecuteScalar()?.ToString();

                                if (asesoresActuales != null && asesoresActuales.Contains(nombreAsesorEliminar))
                                {
                                    // El asesor está en la lista, procede a eliminarlo
                                    asesoresActuales = asesoresActuales.Replace(nombreAsesorEliminar, "").Replace(";;", ";").Trim(';');

                                    string consultaActualizar = "UPDATE tblClienteObra SET CompartidoCon = @NuevaCadena WHERE Nit = @Cedula";

                                    using (SqlCommand command = new SqlCommand(consultaActualizar, connection))
                                    {
                                        command.Parameters.AddWithValue("@NuevaCadena", asesoresActuales);
                                        command.Parameters.AddWithValue("@Cedula", cedulaClienteEliminar);
                                        command.ExecuteNonQuery();
                                    }

                                    // Cerrar el modal después de agregar el asesor
                                    ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);

                                    // Refrescar la página después de cerrar el modal
                                    ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);

                                    // Mostrar mensaje de éxito
                                    string mensajeExito = "El Asesor " + nombreAsesorEliminar + " ha sido eliminado exitosamente.";
                                    string scriptExito = "alert('" + mensajeExito + "');";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessEliminar", scriptExito, true);
                                }
                                else
                                {
                                    // El asesor no está en la lista
                                    // Cerrar el modal después de agregar el asesor
                                    ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);

                                    // Refrescar la página después de cerrar el modal
                                    ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);
                                    string mensajeExito = "El Asesor " + nombreAsesorEliminar + " no se encuentra compartido actualmente.";
                                    string scriptExito = "alert('" + mensajeExito + "');";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessEliminar", scriptExito, true);
                                }
                            }
                        }
                        else
                        {
                            // El cliente no existe
                            // Cerrar el modal después de agregar el asesor
                            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);

                            // Refrescar la página después de cerrar el modal
                            ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);
                            string mensajeExito = "Error en la consulta a la base de datos. Inténtalo más tarde o contacta soporte.";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessEliminar", scriptExito, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Cerrar el modal después de agregar el asesor
                ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#myModal').modal('hide');", true);

                // Refrescar la página después de cerrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage", "window.location.reload();", true);

                // Mostrar mensaje de éxito
                string mensajeExito = "Error en la consulta a la base de datos. Inténtalo más tarde o contacta soporte.";
                string scriptExito = "alert('" + mensajeExito + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
            }
        }




        // LOGICA PARA CONTACTO CLIENTE 

        protected void DatagridContactoClienteFact_LinkButton(object source, DataGridCommandEventArgs e)
        {

            btnModificarContacto.Enabled = true;


            try
            {
                if (e.CommandName == "VerContactoCliente")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    DataGridItem row = DataGridContacto.Items[rowIndex];

                    string idContacto = row.Cells[1].Text;
                    string sede = row.Cells[2].Text;
                    string direccion = row.Cells[3].Text;
                    string nombre = row.Cells[4].Text;
                    string telefono = row.Cells[5].Text;
                    string celular = row.Cells[6].Text;
                    string mail = row.Cells[7].Text;
                    string ciudad = row.Cells[8].Text;

                    Session["IdContactoFactSession"] = idContacto; // Variable que almacena el IdContactoFact
                    tbSede.Text = sede;
                    tbDireccion1.Text = direccion;
                    tbNombreContacto.Text = nombre;
                    tbTelefono1.Text = telefono;
                    tbCelular.Text = celular;
                    tbMailContacto.Text = mail;
                    foreach (ListItem item in ddlCiudad1.Items)
                    {
                        if (item.Text == ciudad)
                        {
                            ddlCiudad1.ClearSelection();
                            item.Selected = true;
                            break;
                        }
                    }


                    // Se compara si el click es en la misma fila con el id del plano 
                    if (row.Cells[1].Text == Session["IdContactoFactSession1"]?.ToString())
                    {
                        // Incrementar la variable de sesión "ClickCount" en el servidor
                        int clickCount = Convert.ToInt32(Session["ClickCount2"]) + 1;
                        Session["ClickCount2"] = clickCount;

                        // se valida si es el segundo click en la misma fila 
                        if (clickCount == 2)
                        {
                            Session["ModalMostrado"] = true;
                            // Llamar el script que recarga el formulario padre de donde salio la pagina 
                            string script = "<script>enviarFormulario();</script>";
                            ScriptManager.RegisterStartupScript(this, GetType(), "enviarFormulario", script, false);

                            // Reiniciar la variable de sesión "ClickCount" a 0 para la próxima interacción                        
                            Session.Remove("IdContactoFactSession1");
                            Session.Remove("ClickCount2");
                            
                        }

                    }
                    else
                    {
                        // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                        Session["ClickCount2"] = 1;
                        Session["IdContactoFactSession1"] = row.Cells[1].Text;
                    
                    }

                }
            }
            catch (Exception ex)
            {

                MensajeError.Text = "Ocurrió un error al procesar la información. Por favor, inténtelo nuevamente o comuníquese con el soporte técnico.";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }




        }

        protected void NuevoContactoFact(object sender, EventArgs e)
        {
            foreach (TextBox textBox in listaTextBoxes1)
            {
                textBox.Enabled = true;
                textBox.Text = "";

            }

            ddlCiudad1.Enabled = true;
            ddlCiudad1.Text = "0";

            btnNuevoContacto.Enabled = false;
            btnNuevoContacto.CssClass = "btn btn-outline-secondary";
            btnGrabarContacto.Enabled = true;
            btnModificarContacto.Enabled = false;
            btnModificarContacto.CssClass = "btn btn-outline-secondary";
            btnCancelar1.Enabled = true;


            // Se cambia de estado la variable de Session para Guardar o modficar Contacto
            Session["GuaModContactoFactSession"] = "Insertar";

        }

        protected void MoficarContactoFact(object sender, EventArgs e)
        {
            foreach (TextBox textBox in listaTextBoxes1)
            {
                textBox.Enabled = true;

            }
            ddlCiudad1.Enabled = true;

            btnNuevoContacto.Enabled = false;
            btnNuevoContacto.CssClass = "btn btn-outline-secondary";
            btnGrabarContacto.Enabled = true;
            btnModificarContacto.Enabled = false;
            btnModificarContacto.CssClass = "btn btn-outline-secondary";
            btnCancelar1.Enabled = true;



            // Se cambia de estado la variable de Session para Guardar o modficar Contacto
            Session["GuaModContactoFactSession"] = "Actualizar";
        }

        protected void CancelarContactoFact(object sender, EventArgs e)
        {

            foreach (TextBox textBox in listaTextBoxes1)
            {
                textBox.Text = "";
                textBox.Enabled = false;
            }
            ddlCiudad1.Enabled = false;
            ddlCiudad1.Text = "0";

            btnNuevoContacto.Enabled = true;
            btnGrabarContacto.Enabled = false;
            btnGrabarContacto.CssClass = "btn btn-outline-secondary";
            btnModificarContacto.Enabled = false;
            btnModificarContacto.CssClass = "btn btn-outline-secondary";
            btnCancelar1.Enabled = true;

            DataGridContacto.DataBind();


        }

        protected void GuardarModificarContactoFact(object sender, EventArgs e)
        {

            if(tbNumero.Text != "")
            {
                if (Session["GuaModContactoFactSession"].ToString() == "Insertar")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();


                        using (SqlCommand getMaxIdCmd = new SqlCommand("Select  Max(cocConsecutivoContacto) from tblClienteObraContacto WHERE cocNIT= @Nit", connection))
                        {
                            getMaxIdCmd.Parameters.AddWithValue("@Nit", tbNumero.Text);

                            object maxIdObj = getMaxIdCmd.ExecuteScalar();
                            int maxId = (maxIdObj != null && maxIdObj != DBNull.Value) ? Convert.ToInt32(maxIdObj) : 0;


                            int Consecutivo = maxId + 1;


                            string IdConsecutivo = Consecutivo.ToString();
                            connection.Close();

                            using (SqlCommand cmd = new SqlCommand("sp_InsertarContactoClienteObra", connection))
                            {

                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@cocNIT", tbNumero.Text);
                                cmd.Parameters.AddWithValue("@cocConsecutivoContacto", IdConsecutivo);
                                cmd.Parameters.AddWithValue("@cocSede", tbSede.Text);
                                cmd.Parameters.AddWithValue("@cocDireccion", tbDireccion1.Text);
                                cmd.Parameters.AddWithValue("@cocNombre", tbNombreContacto.Text);
                                cmd.Parameters.AddWithValue("@cocTelefono", tbTelefono1.Text);
                                cmd.Parameters.AddWithValue("@cocCelular", tbCelular.Text);
                                cmd.Parameters.AddWithValue("@cocMail", tbMailContacto.Text);
                                cmd.Parameters.AddWithValue("@cocCiudad", ddlCiudad1.SelectedItem.Text);
                                cmd.Parameters.AddWithValue("@cocFechaCreacion", DateTime.Now);
                                cmd.Parameters.AddWithValue("@cocUltimaActualizacion", DateTime.Now);


                                connection.Open();


                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    string mensajePersonalizado = "El Contacto ha sido guardado con exito.";
                                    string urlRedireccion = "FormExtPrin/NitOTs.aspx";
                                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                                }
                                else
                                {
                                    // script de error de Insercion  
                                }

                            }
                        }
                    }

                }

                else if (Session["GuaModContactoFactSession"].ToString() == "Actualizar")
                {
                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("Sp_ActualizarconatctoClienteObra", connection))
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdContacto", Session["IdContactoFactSession"].ToString());
                            cmd.Parameters.AddWithValue("@cocSede", tbSede.Text);
                            cmd.Parameters.AddWithValue("@cocDireccion", tbDireccion1.Text);
                            cmd.Parameters.AddWithValue("@cocNombre", tbNombreContacto.Text);
                            cmd.Parameters.AddWithValue("@cocTelefono", tbTelefono1.Text);
                            cmd.Parameters.AddWithValue("@cocCelular", tbCelular.Text);
                            cmd.Parameters.AddWithValue("@cocMail", tbMailContacto.Text);
                            cmd.Parameters.AddWithValue("@cocCiudad", ddlCiudad1.SelectedItem.Text);
                            cmd.Parameters.AddWithValue("@cocUltimaActualizacion", DateTime.Now);


                            connection.Open();


                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                string mensajePersonalizado = "El contacto ha sido actualizado correctamente.";
                                string urlRedireccion = "FormExtPrin/NitOTs.aspx";
                                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

                            }
                            else
                            {
                                //Error de actualizacion  
                            }

                        }
                    }
                }
            }
            else
            {
                // La eliminación no fue exitosa, mostrar mensajes o tomar acciones adicionales
                string mensajeError = "Por favor, seleccione un cliente.";
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", $"alert('{mensajeError}');", true);
            }
          
        }

       
    }

}
