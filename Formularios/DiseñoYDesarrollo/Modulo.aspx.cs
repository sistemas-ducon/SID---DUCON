using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Diseño_Venta;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo
{


    public partial class Modulo : System.Web.UI.Page
    {



        private string CadenaConexionSID = "BD_SIDSQL";
        private string CadenaConexionISID = "BD_ISIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                if (!IsPostBack)
                {
                    Session.Remove("CRUDFAMILIAMODULO"); 
                    Session.Remove("CRUDModuloTabConfiguracion");
                    CargarSiempre();

                    string tipoAccion = Session["Modulo"] as string;
                    if (tipoAccion == "Nuevo")
                    {
                        string scriptDisableTabs = @"
        document.getElementById('Configuracion-tab').classList.add('disabled');
        document.getElementById('Configuracion-Content').classList.add('d-none');";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "disableTabsScript", scriptDisableTabs, true);
                   

                    }
                    if (tipoAccion == "Modificar")
                    {
                        if (Session["IDModulo"] != null)
                        {
                            LlenarControles();
                            ViewState["OriginalFamilia"] = DropDownListGrupo.SelectedItem.Text;

                            BtnModificarConfiguracion.Enabled = false;
                            BtnModificarConfiguracion.CssClass = "btn btn-sm shadow button-disabled2";
                        }
                    }
                    if (tipoAccion == "Copiar")
                    {
                        DeshabilitarElementos();
                        LlenarControles();
                    }
                    if (tipoAccion == "Consultar")
                    {
                        BtnAdicionarConfiguracion.Enabled = false;
                        BtnAdicionarConfiguracion.CssClass = "btn btn-sm shadow button-disabled2";

                        DropDownList2.Enabled = false;
                        DropDownList2.CssClass = "form-control form-control-sm";

                        TextCriterio.Enabled = false;
                        TextCriterio.CssClass = "form-control form-control-sm";

                        TextInv.Enabled = false;
                        TextInv.CssClass = "form-control form-control-sm";

                        LlenarControles();
                    }
                }
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }
        }

        protected void DeshabilitarElementos()
        {
            DropDownListGrupo.Enabled = false;
            DropDownListGrupo.CssClass = "form-control form-control-sm";

            TextDescripcionModulo.Enabled = false;
            TextDescripcionModulo.CssClass = "form-control form-control-sm";

            DropDownListTipoModulo.Enabled = false;
            DropDownListTipoModulo.CssClass = "form-control form-control-sm";
        }

        protected void LlenarControles()
        {
            string moduloID = Session["IDModulo"].ToString();
            LlenarControlesModulo(moduloID);

            List<PanelData> paneles = ObtenerDatosPanel(moduloID);
            DataGrid2.DataSource = paneles;
            DataGrid2.DataBind();

            LlenarDataGrid3(moduloID);
        }

        protected void CargarSiempre()
        {
            ViewState.Clear();

            CargarAreaProduccion();

            llenarDatagridFamilia();

            llenarDatagridProcesoProductivo();

            deshabilitarElementosFamiliaModulo();

            BtnCancelar.Enabled = true;
            BtnCancelar.CssClass = "btn btn-sm shadow button-enabled2 RojoCancelar";

            if (ValidarPermisoArea(33))
            {
                BtnNuevoFamilia.Enabled = true;
                BtnNuevoFamilia.CssClass = "btn btn-sm shadow button-enabled2 AzulClaro";
            }
        

            textModulo.Enabled = false;
            textModulo.CssClass = "form-control form-control-sm";

            BtnGrabarInf.Enabled = false;
            BtnGrabarInf.CssClass = "btn btn-sm shadow button-disabled";

            BtnAdicionarConfiguracion.Enabled = true;
            BtnAdicionarConfiguracion.CssClass = "btn btn-sm shadow button-enabled2 Verde";

            BtnModificarConfiguracion.Enabled = false;
            BtnModificarConfiguracion.CssClass = "btn btn-sm shadow button-disabled2";

            BtnGrabarConfiguracion.Enabled = true;
            BtnGrabarConfiguracion.CssClass = "btn btn-sm shadow button-enabled2 ColorAzulActivo";

            BtnX.Enabled = false;
            BtnX.CssClass = "btn btn-sm shadow button-disabled2";

            BtnCancelarInf.Enabled = false;
            BtnCancelarInf.CssClass = "btn btn-sm shadow button-disabled";

            BtnAdicionarRotacion.Enabled = false;
            BtnAdicionarRotacion.CssClass = "btn btn-sm shadow button-disabled";

            BtnSubirEstacion.Enabled = false;
            BtnSubirEstacion.CssClass = "btn btn-sm shadow button-disabled";

            BtnBajarEstacion.Enabled = false;
            BtnBajarEstacion.CssClass = "btn btn-sm shadow button-disabled";

            BtnX2.Enabled = false;
            BtnX2.CssClass = "btn btn-sm shadow button-disabled";



            CargarTipoInsumo();
            CargarDropDownListGrupo();
            CargarDropDownListTipoModulo();


            // Deshabilitar y limpiar controles
            TextCantidadCon.Enabled = false;
            TextCantidadCon.Text = "0";
            TextCantidadCon.CssClass = "form-control form-control-sm ms-2";
            TextDctoAltura.Text = "0";
            TextDctoAltura.Enabled = false;
            TextDctoAltura.CssClass = "form-control form-control-sm";
            TextDctoAncho.Text = "0";
            TextDctoAncho.Enabled = false;
            TextDctoAncho.CssClass = "form-control form-control-sm";
            TextAncho.Text = "0";
            TextAncho.Enabled = false;
            TextAncho.CssClass = "form-control form-control-sm";
            TextAlto.Text = "0";
            TextAlto.Enabled = false;
            TextAlto.CssClass = "form-control form-control-sm";
            DropDownList1.Enabled = false;

            txObs1.Disabled = true;
            txObs1.InnerText = string.Empty;

            BtnSubirEstacion.Enabled = false;
            BtnSubirEstacion.CssClass = "btn btn-sm shadow button-disabled";

            BtnBajarEstacion.Enabled = false;
            BtnBajarEstacion.CssClass = "btn btn-sm shadow button-disabled";

            BtnX2.Enabled = false;
            BtnX2.CssClass = "btn btn-sm shadow button-disabled";

            CheckCostearCon.Enabled = false;
            CheckPieEsCon.Enabled = false;
            TextDivisionesCon.Enabled = false;
            TextDivisionesCon.Text = "1";
            TextDivisionesCon.Attributes["data-tag"] = "1";
            TextDivisionesCon.CssClass = "form-control form-control-sm";

            DropDownList1.DataBind();

            ViewState["bordeDerechoVisible"] = false;
        }

        protected void deshabilitarElementosFamiliaModulo()
        {
            TextDescripcion.Enabled = false;
            TextDescripcion.CssClass = "form-control form-control-sm";

            TextDescripcionParaObjeto.Enabled = false;
            TextDescripcionParaObjeto.CssClass = "form-control form-control-sm";

            TextResponsableFamilia.Enabled = false;
            TextResponsableFamilia.CssClass = "form-control form-control-sm";

            TextUlAct.Enabled = false;
            TextUlAct.CssClass = "form-control form-control-sm";

            TextDescuentoFinal.Enabled = false;
            TextDescuentoFinal.CssClass = "form-control form-control-sm";

            BtnNuevoFamilia.Enabled = false;
            BtnNuevoFamilia.CssClass = "btn btn-sm shadow button-disabled";

            BtnModificarFamilia.Enabled = false;
            BtnModificarFamilia.CssClass = "btn btn-sm shadow button-disabled";

            BtnGrabarFamilia.Enabled = false;
            BtnGrabarFamilia.CssClass = "btn btn-sm shadow button-disabled";
        }

        private bool ValidarPermisoArea(int IdPermiso)
        {
            bool tienePermiso = false;
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = @ID_Empleado AND ID_Permiso = @ID_Permiso";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Empleado", Session["CedulaLogeada"].ToString());
                    command.Parameters.AddWithValue("@ID_Permiso", IdPermiso);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Si el lector tiene filas, el permiso existe.
                            tienePermiso = reader.HasRows;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Maneja cualquier error de conexión o consulta aquí
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return tienePermiso;
        }


        private void llenarDatagridFamilia()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblFamiliaModulo ORDER BY Descripcion_Familia";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dt);
                    IDDatagridFamilia.DataSource = dt;
                    IDDatagridFamilia.DataBind();
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        private void llenarDatagridFamiliaFiltro()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select * from tblFamiliaModulo where Descripcion_Familia like @DescripcionFamilia order by Descripcion_Familia";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                
                    string descripcionFamilia = "%" + TextBuscarFamiliaModulo.Text.Trim() + "%";
                    command.Parameters.AddWithValue("@DescripcionFamilia", descripcionFamilia);

                    connection.Open();
                    adapter.Fill(dt);
                    IDDatagridFamilia.DataSource = dt;
                    IDDatagridFamilia.DataBind();
              
                  
                
            }
        }

        private void llenarDatagridProcesoProductivo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select * from tblProcesoProduccion where activo = 1 order by Descripcion_Proceso asc";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dt);
                    DatagridProcesoProductivo.DataSource = dt;
                    DatagridProcesoProductivo.DataBind();
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        // Método para cargar el DropDownList Grupo
        private void CargarDropDownListGrupo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ID_Familia, Descripcion_Familia FROM tblFamiliaModulo ORDER BY Descripcion_Familia";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                DropDownListGrupo.Items.Clear(); // Limpiar los elementos anteriores
                DropDownListGrupo.Items.Add(new ListItem("", "0")); // Agregar un elemento vacío

                while (reader.Read())
                {
                    DropDownListGrupo.Items.Add(new ListItem(reader["Descripcion_Familia"].ToString(), reader["ID_Familia"].ToString()));
                }

                connection.Close();
            }
        }

        // Método para cargar el DropDownList TipoModulo
        private void CargarDropDownListTipoModulo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id_TipoModulo, Descripcion_TipoModulo FROM tblTipoModulo ORDER BY Descripcion_TipoModulo";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                DropDownListTipoModulo.Items.Clear(); // Limpiar los elementos anteriores
                DropDownListTipoModulo.Items.Add(new ListItem("", "0")); // Agregar un elemento vacío

                while (reader.Read())
                {
                    DropDownListTipoModulo.Items.Add(new ListItem(reader["Descripcion_TipoModulo"].ToString(), reader["Id_TipoModulo"].ToString()));
                }

                connection.Close();
            }
        }

        private void CargarTipoInsumo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "Select Id_TipoInsumo, Descripcion from tblTipoInsumo order by descripcion";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "TipoInsumo");

                DropDownList2.DataSource = ds.Tables["TipoInsumo"];
                DropDownList2.DataTextField = "Descripcion";
                DropDownList2.DataValueField = "Id_TipoInsumo";

                DropDownList2.DataBind();
                DropDownList2.Items.Insert(0, new ListItem(" "));
            }
        }

        private List<PanelData> ObtenerDatosPanel(string idModulo)
        {
            List<PanelData> listaPaneles = new List<PanelData>();

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT tblPanel.Id_Panel, 
                                    tblPanel.Altura, 
                                    tblPanel_Modulo.Ubicacion_Modulo, 
                                    tblPanel_Modulo.Lado,
                                    tblPanel_Modulo.Cantidad, 
                                    tblGrupoObjeto.Descripcion_Grupo, 
                                    tblGrupoObjeto.Reportar_Despacho
                             FROM tblGrupoObjeto 
                             INNER JOIN (tblPanel 
                             INNER JOIN tblPanel_Modulo ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum)
                             ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto 
                             WHERE tblPanel_Modulo.ID_Modulo = @IDModulo
                             GROUP BY tblPanel.Id_Panel, 
                                      tblPanel.Altura, 
                                      tblPanel_Modulo.Ubicacion_Modulo, 
                                      tblPanel_Modulo.Lado,
                                      tblPanel_Modulo.Cantidad, 
                                      tblGrupoObjeto.Descripcion_Grupo, 
                                      tblGrupoObjeto.Reportar_Despacho 
                             ORDER BY tblPanel.Id_Panel";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IDModulo", idModulo);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaPaneles.Add(new PanelData
                            {
                                Id_Panel = reader["Id_Panel"].ToString(),
                                Altura = reader["Altura"].ToString(),
                                Ubicacion_Modulo = reader["Ubicacion_Modulo"].ToString(),
                                Lado = reader["Lado"].ToString(),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Descripcion_Grupo = reader["Descripcion_Grupo"].ToString(),
                                Reportar_Despacho = Convert.ToBoolean(reader["Reportar_Despacho"])
                            });
                        }
                    }
                }
            }

            return listaPaneles;
        }

        private void LlenarControlesModulo(string moduloID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("ctaModulo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Modulo", moduloID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Asignar valores a los controles
                            textModulo.Text = reader["Id_Modulo"].ToString();
                            DropDownListGrupo.SelectedValue = reader["ID_Familia"].ToString();
                            TextDescripcionModulo.Text = reader["Descripcion_Modulo"].ToString();
                            DropDownListTipoModulo.SelectedValue = reader["Id_TipoModulo"].ToString();
                            TextAlturaModulo.Text = reader["Altura"].ToString();

                            ViewState["Grupo_Original"] = DropDownListGrupo.SelectedValue;
                            ViewState["DescripcionModulo_Original"] = TextDescripcionModulo.Text.Trim();
                            ViewState["TipoModulo_Original"] = DropDownListTipoModulo.SelectedValue;
                            ViewState["AlturaModulo_Original"] = TextAlturaModulo.Text.Trim();
                        }
                    }
                }
            }
        }

        // Método para llenar DataGrid3 con los resultados del procedimiento almacenado
        private void LlenarDataGrid3(string moduloID)
        {
            // Define la cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    // Abre la conexión a la base de datos
                    con.Open();

                    // Configura el comando para llamar al procedimiento almacenado
                    using (SqlCommand cmd = new SqlCommand("ctaModulo_Insumos", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Modulo", moduloID);

                        // Ejecuta el comando y obtiene los resultados
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            // Asigna los resultados al DataGrid3
                            DataGrid3.DataSource = dt;
                            DataGrid3.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Maneja cualquier excepción que ocurra
                    // Puedes registrar el error o mostrar un mensaje de error en la interfaz
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public class PanelData
        {
            public string Id_Panel { get; set; }
            public string Altura { get; set; }
            public string Ubicacion_Modulo { get; set; }
            public string Lado { get; set; }
            public int Cantidad { get; set; }
            public string Descripcion_Grupo { get; set; }
            public bool Reportar_Despacho { get; set; }
        }

        protected void DropDownList1_TextChanged(object sender, EventArgs e)
        {
            // Obtener los valores de los controles de búsqueda
            string criterioInsumo = TextCriterio.Text.Trim();
            string codInventario = TextInv.Text.Trim();
            string tipoInsumo = DropDownList2.SelectedItem.Text; // Asumiendo que 'DropDownList2' es el DropDownList con los tipos de insumo

            // Consulta base
            string consulta = @"SELECT tblInsumo.*, tblTipoInsumo.Descripcion AS TipoInsumoDescripcion, 
                        tblUnidad_Medida.Abreviado 
                        FROM tblInsumo, tblTipoInsumo, tblUnidad_Medida 
                        WHERE 1=1"; // Usar '1=1' para facilitar la concatenación de condiciones

            // Lista de condiciones para el WHERE
            List<string> condiciones = new List<string>();

            // Lista de parámetros para la consulta
            List<SqlParameter> parametros = new List<SqlParameter>();

            // Si el campo código de inventario no está vacío
            if (!string.IsNullOrEmpty(codInventario))
            {
                condiciones.Add("tblInsumo.ID_Inventario LIKE @codInventario");
                parametros.Add(new SqlParameter("@codInventario", "%" + codInventario + "%"));
            }

            // Si el campo criterio de descripción no está vacío
            if (!string.IsNullOrEmpty(criterioInsumo))
            {
                condiciones.Add("tblInsumo.Descripcion_Insumo LIKE @criterioInsumo");
                parametros.Add(new SqlParameter("@criterioInsumo", "%" + criterioInsumo + "%"));
            }

            // Si el tipo de insumo no está vacío
            if (!string.IsNullOrEmpty(tipoInsumo) && tipoInsumo != " ")
            {
                condiciones.Add("tblTipoInsumo.Descripcion LIKE @tipoInsumo");
                parametros.Add(new SqlParameter("@tipoInsumo", tipoInsumo + "%"));
            }

            // Condiciones para las relaciones entre las tablas
            condiciones.Add("tblTipoInsumo.ID_TipoInsumo = tblInsumo.Id_TipoInsumo");
            condiciones.Add("tblUnidad_Medida.Id_unidadMedida = tblInsumo.Id_unidadMedida");

            // Agregar condiciones al WHERE
            if (condiciones.Count > 0)
            {
                consulta += " AND " + string.Join(" AND ", condiciones);
            }

            // Agregar ORDER BY
            consulta += " ORDER BY tblInsumo.Descripcion_Insumo";

            // Conexión a la base de datos
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(consulta, conn);

                // Agregar los parámetros al comando
                cmd.Parameters.AddRange(parametros.ToArray());

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        // Llenar el DataGrid con los resultados
                        DataGrid1.DataSource = dt;
                        DataGrid1.DataBind();
                    }
                    else
                    {
                        // Limpiar el DataGrid si no hay resultados
                        DataGrid1.DataSource = null;
                        DataGrid1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Manejar errores (puedes agregar un log o mostrar un mensaje de error)
                }
            }


            PanelContenido.Attributes["class"] = PanelContenido.Attributes["class"].Replace("d-none", "").Trim();

            PanelContenido.Visible = true;
        }

        private bool ValoresCambiaron()
        {
            // Verificar si los valores han cambiado respecto al valor original almacenado en ViewState
            bool grupoCambiado = ViewState["Grupo_Original"] != null && DropDownListGrupo.SelectedValue != ViewState["Grupo_Original"].ToString();
            bool descripcionCambiada = ViewState["DescripcionModulo_Original"] != null && TextDescripcionModulo.Text.Trim() != ViewState["DescripcionModulo_Original"].ToString();
            bool tipoModuloCambiado = ViewState["TipoModulo_Original"] != null && DropDownListTipoModulo.SelectedValue != ViewState["TipoModulo_Original"].ToString();
            bool alturaModuloCambiada = ViewState["AlturaModulo_Original"] != null && TextAlturaModulo.Text.Trim() != ViewState["AlturaModulo_Original"].ToString();

            // Retornar true si hubo algún cambio
            return grupoCambiado || descripcionCambiada || tipoModuloCambiado || alturaModuloCambiada;
        }

        private bool CamposSonValidos()
        {
            // Validar si los campos no están vacíos
            bool grupoValido = !string.IsNullOrWhiteSpace(DropDownListGrupo.SelectedValue) && DropDownListGrupo.SelectedValue != "0";
            bool descripcionValida = !string.IsNullOrWhiteSpace(TextDescripcionModulo.Text.Trim());
            bool tipoModuloValido = !string.IsNullOrWhiteSpace(DropDownListTipoModulo.SelectedValue) && DropDownListTipoModulo.SelectedValue != "0";

            // Validar que la altura no esté vacía y sea un valor numérico
            bool alturaValida = !string.IsNullOrWhiteSpace(TextAlturaModulo.Text.Trim()) && double.TryParse(TextAlturaModulo.Text.Trim(), out _);

            // Retornar true si todos los campos son válidos
            return grupoValido && descripcionValida && tipoModuloValido && alturaValida;
        }

        private void EstadoGrabarCancelar()
        {
            string tipoAccion = Session["Modulo"] as string;
            if (tipoAccion == "Nuevo")
            {
                if (CamposSonValidos())
                {
                    BtnGrabarInf.Enabled = true;
                    BtnGrabarInf.CssClass = "btn btn-sm shadow button-enabled";

                    BtnCancelarInf.Enabled = true;
                    BtnCancelarInf.CssClass = "btn btn-sm shadow button-enabled2 RojoCancelar";
                }
                else
                {
                    BtnGrabarInf.Enabled = false;
                    BtnGrabarInf.CssClass = "btn btn-sm shadow button-disabled";

                    BtnCancelarInf.Enabled = false;
                    BtnCancelarInf.CssClass = "btn btn-sm shadow button-disabled";
                }
            }
            if (tipoAccion == "Modificar")
            {
                if (ValoresCambiaron() && CamposSonValidos())
                {
                    BtnGrabarInf.Enabled = true;
                    BtnGrabarInf.CssClass = "btn btn-sm shadow button-enabled";

                    BtnCancelarInf.Enabled = true;
                    BtnCancelarInf.CssClass = "btn btn-sm shadow button-enabled2 RojoCancelar";
                }
                else
                {
                    BtnGrabarInf.Enabled = false;
                    BtnGrabarInf.CssClass = "btn btn-sm shadow button-disabled";

                    BtnCancelarInf.Enabled = false;
                    BtnCancelarInf.CssClass = "btn btn-sm shadow button-disabled";
                }
            }
        }



        protected void CheckForChanges(object sender, EventArgs e)
        {
            EstadoGrabarCancelar();
        }

        protected void BtnGrabarInf_Click(object sender, EventArgs e)
        {
            string tipoAccion = Session["Modulo"] as string;
            if (tipoAccion == "Copiar")
            {
                copiar_Modulo();
            }
            else
            {
                AgregarModificar_Modulo();
            }
        }

        protected void AgregarModificar_Modulo()
        {
            // Obtener las variables de sesión y los controles
            string opcionModulo = Session["Modulo"].ToString().ToUpper(); // Valor de opción (Nuevo o Modificar)
            string nombreUsuario = Session["usuariologueado"].ToString(); // Nombre del usuario
            string idModulo = textModulo.Text.Trim(); // Id del módulo
            string descripcionModulo = TextDescripcionModulo.Text.Trim(); // Descripción del módulo
            string tipoModulo = DropDownListTipoModulo.SelectedValue; // Tipo de módulo seleccionado
            string altura = TextAlturaModulo.Text; // Altura del módulo seleccionado
            string idFamilia = DropDownListGrupo.SelectedValue; // Id de la familia del módulo

            // Variables para conexión y comandos SQL
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si la opción es "Nuevo"
                if (opcionModulo == "NUEVO")
                {
                    Calcular_IdModulo(); // Método para calcular el Id del módulo
                    string insertQuery = @"INSERT INTO tblModulo(Id_Modulo, Descripcion_Modulo, Id_TipoModulo, Altura, ID_Familia, Responsable, FechaChequeo) 
                                   VALUES (@IdModulo, @DescripcionModulo, @IdTipoModulo, @Altura, @IDFamilia, @Responsable, @FechaChequeo)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdModulo", textModulo.Text);
                        cmd.Parameters.AddWithValue("@DescripcionModulo", descripcionModulo);
                        cmd.Parameters.AddWithValue("@IdTipoModulo", tipoModulo);
                        cmd.Parameters.AddWithValue("@Altura", altura);
                        cmd.Parameters.AddWithValue("@IDFamilia", idFamilia);
                        cmd.Parameters.AddWithValue("@Responsable", nombreUsuario);
                        cmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now);

                        int CantidadFilasAfectada = cmd.ExecuteNonQuery();


                        if (CantidadFilasAfectada > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#InsercionExitosa').modal('show');", true);
                            idModulo = textModulo.Text;
                           
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#Noseinsertaronregistros').modal('show');", true);
                        }
                    }
                }
                if (opcionModulo == "MODIFICAR")
                {
                    try
                    {
                        // Verificar si el módulo tiene al menos un panel asociado
                        string countQuery = "SELECT COUNT(Id_Modulo) FROM tblPanel_Modulo WHERE Id_Modulo = @IdModulo";
                        using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                        {
                            countCmd.Parameters.AddWithValue("@IdModulo", idModulo);
                            int count = Convert.ToInt32(countCmd.ExecuteScalar());

                            if (count == 0) // Si no hay paneles asociados, se puede modificar
                            {
                                // Actualización de tblModulo
                                string updateQuery = @"UPDATE tblModulo 
                                       SET Descripcion_Modulo = @DescripcionModulo, 
                                           Id_TipoModulo = @IdTipoModulo, 
                                           Altura = @Altura, 
                                           ID_Familia = @IDFamilia, 
                                           Responsable = @Responsable, 
                                           FechaChequeo = @FechaChequeo 
                                       WHERE Id_Modulo = @IdModulo";

                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                                {
                                    updateCmd.Parameters.AddWithValue("@IdModulo", idModulo);
                                    updateCmd.Parameters.AddWithValue("@DescripcionModulo", descripcionModulo);
                                    updateCmd.Parameters.AddWithValue("@IdTipoModulo", tipoModulo);
                                    updateCmd.Parameters.AddWithValue("@Altura", altura);
                                    updateCmd.Parameters.AddWithValue("@IDFamilia", idFamilia);
                                    updateCmd.Parameters.AddWithValue("@Responsable", nombreUsuario);
                                    updateCmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now);

                                    updateCmd.ExecuteNonQuery();
                                }

                                // Actualización de sentido en tblModulo_insumo
                                string updateSentidoQuery = tipoModulo == "3"
                                    ? "UPDATE tblModulo_insumo SET Sentido = 'PROFUNDIDAD' WHERE Id_Modulo = @IdModulo AND Sentido = 'HORIZONTAL'"
                                    : "UPDATE tblModulo_insumo SET Sentido = 'HORIZONTAL' WHERE Id_Modulo = @IdModulo AND Sentido = 'PROFUNDIDAD'";

                                using (SqlCommand updateSentidoCmd = new SqlCommand(updateSentidoQuery, connection))
                                {
                                    updateSentidoCmd.Parameters.AddWithValue("@IdModulo", idModulo);
                                    updateSentidoCmd.ExecuteNonQuery();
                                }

                                // Si todas las actualizaciones son exitosas, mostrar mensaje de éxito
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", "alert('El módulo ha sido actualizado exitosamente.');", true);

                            }
                            else
                            {
                                // Si el módulo tiene al menos un panel asociado, mostrar modal de confirmación
                                string contenidoModalOT = "El módulo: " + textModulo.Text + " conforma al menos un objeto. ¿Quiere modificar el módulo?";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ModalRotacionModulo').modal('show'); $('#ModalRotacionModulo2').text('" + contenidoModalOT + "');", true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Si ocurre algún error, mostrar mensaje de error
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", "alert('Ocurrió un error al intentar actualizar el módulo: " + ex.Message + "');", true);
                    }
                }


                Session["IDModulo"] = idModulo;
                LlenarControles();
                BtnGrabarInf.Enabled = false;
                BtnGrabarInf.CssClass = "btn btn-sm shadow button-disabled";

                BtnCancelarInf.Enabled = false;
                BtnCancelarInf.CssClass = "btn btn-sm shadow button-disabled";
            }
        }

        protected void btnConfirmarModificacion_Click(object sender, EventArgs e)
        {

            string nombreUsuario = Session["usuariologueado"].ToString();
            string idModulo = textModulo.Text.Trim();
            string descripcionModulo = TextDescripcionModulo.Text.Trim();
            int tipoModulo = Convert.ToInt32(DropDownListTipoModulo.SelectedValue);
            string altura = TextAlturaModulo.Text;
            string idFamilia = DropDownListGrupo.SelectedValue;

            // Llamar al método
            ModificarModuloConfirmado(idModulo, descripcionModulo, tipoModulo, altura, idFamilia, nombreUsuario);
        }

        protected void ModificarModuloConfirmado(string idModulo, string descripcionModulo, int tipoModulo, string altura, string idFamilia, string nombreUsuario)
        {
            // Conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                connection.Open();

                try
                {
                    // Paso 1: Actualizar la tabla tblModulo
                    string updateModuloQuery = @"UPDATE tblModulo 
                                         SET Descripcion_Modulo = @DescripcionModulo, 
                                             Id_TipoModulo = @IdTipoModulo, 
                                             Altura = @Altura, 
                                             ID_Familia = @IDFamilia, 
                                             Responsable = @Responsable, 
                                             FechaChequeo = @FechaChequeo 
                                         WHERE Id_Modulo = @IdModulo";

                    using (SqlCommand updateModuloCmd = new SqlCommand(updateModuloQuery, connection))
                    {
                        updateModuloCmd.Parameters.AddWithValue("@IdModulo", idModulo);
                        updateModuloCmd.Parameters.AddWithValue("@DescripcionModulo", descripcionModulo);
                        updateModuloCmd.Parameters.AddWithValue("@IdTipoModulo", tipoModulo);
                        updateModuloCmd.Parameters.AddWithValue("@Altura", altura);
                        updateModuloCmd.Parameters.AddWithValue("@IDFamilia", idFamilia);
                        updateModuloCmd.Parameters.AddWithValue("@Responsable", nombreUsuario);
                        updateModuloCmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now);

                        updateModuloCmd.ExecuteNonQuery();
                    }

                    // Paso 2: Actualizar el sentido en tblModulo_insumo dependiendo del tipo de módulo
                    string updateSentidoQuery;
                    if (tipoModulo == 3)
                    {
                        updateSentidoQuery = "UPDATE tblModulo_insumo SET Sentido = 'PROFUNDIDAD' WHERE Id_Modulo = @IdModulo AND Sentido = 'HORIZONTAL'";
                    }
                    else
                    {
                        updateSentidoQuery = "UPDATE tblModulo_insumo SET Sentido = 'HORIZONTAL' WHERE Id_Modulo = @IdModulo AND Sentido = 'PROFUNDIDAD'";
                    }

                    using (SqlCommand updateSentidoCmd = new SqlCommand(updateSentidoQuery, connection))
                    {
                        updateSentidoCmd.Parameters.AddWithValue("@IdModulo", idModulo);
                        updateSentidoCmd.ExecuteNonQuery();
                    }

                    string originalFamilia = ViewState["OriginalFamilia"] as string;
                    string selectedFamilia = DropDownListGrupo.SelectedItem.Text;

                    // Comparar el valor original con el nuevo valor seleccionado
                    if (!string.Equals(originalFamilia, selectedFamilia))
                    {
                        string updateDescripcionQuery = @"UPDATE tblPanel 
                                                  SET Descripcion_Tecnica = dbo.fn_DescripcionObjeto(tblGrupoObjeto.goSPDescripcionObjto, tblPanel.Id_Numerico, tblGrupoObjeto.goDescripcionBaseObjeto) 
                                                  FROM tblPanel 
                                                  INNER JOIN tblGrupoObjeto ON tblPanel.Id_GrupoObjeto = tblGrupoObjeto.ID_GrupoObjeto 
                                                  INNER JOIN tblPanel_Modulo ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum 
                                                  INNER JOIN tblModulo ON tblPanel_Modulo.Id_Modulo = tblModulo.Id_Modulo 
                                                  INNER JOIN tblFamiliaModulo ON tblModulo.ID_Familia = tblFamiliaModulo.ID_Familia 
                                                  WHERE tblGrupoObjeto.goDescripcionBaseObjeto IS NOT NULL 
                                                  AND tblGrupoObjeto.goSPDescripcionObjto IS NOT NULL 
                                                  AND tblModulo.Id_Modulo = @IdModulo";

                        using (SqlCommand updateDescripcionCmd = new SqlCommand(updateDescripcionQuery, connection))
                        {
                            updateDescripcionCmd.Parameters.AddWithValue("@IdModulo", idModulo);
                            updateDescripcionCmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    // Puedes loguear el error o mostrar un mensaje al usuario
                    throw new Exception("Ocurrió un error al modificar el módulo: " + ex.Message);
                }
                finally
                {
                    // Cerrar la conexión
                    connection.Close();
                }
            }
        }

        protected void copiar_Modulo()
        {
            // Variables para conexiones y comandos SQL
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string nombreUsuario = Session["usuariologueado"].ToString();
            string moduloID = Session["IDModulo"].ToString();
            double IDModuloInsumo = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el módulo ya existe
                string query = "SELECT * FROM tblModulo WHERE Descripcion_Modulo = @DescripcionModulo AND Altura = @Altura";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DescripcionModulo", TextDescripcionModulo.Text);
                    command.Parameters.AddWithValue("@Altura", TextAlturaModulo.Text);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable moduloTable = new DataTable();
                    adapter.Fill(moduloTable);

                    if (moduloTable.Rows.Count == 0) // Si no hay registros, el módulo no existe
                    {
                        Calcular_IdModulo();

                        // Insertar nuevo módulo en la tabla tblModulo
                        query = @"INSERT INTO tblModulo (Id_Modulo, Descripcion_Modulo, Id_TipoModulo, Altura, ID_Familia, Responsable, FechaChequeo) 
                          VALUES (@IdModulo, @DescripcionModulo, @IdTipoModulo, @Altura, @IDFamilia, @Responsable, @FechaChequeo)";
                        using (SqlCommand insertCmd = new SqlCommand(query, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@IdModulo", textModulo.Text);
                            insertCmd.Parameters.AddWithValue("@DescripcionModulo", TextDescripcionModulo.Text);
                            insertCmd.Parameters.AddWithValue("@IdTipoModulo", DropDownListTipoModulo.SelectedValue);
                            insertCmd.Parameters.AddWithValue("@Altura", TextAlturaModulo.Text);
                            insertCmd.Parameters.AddWithValue("@IDFamilia", DropDownListGrupo.SelectedValue);
                            insertCmd.Parameters.AddWithValue("@Responsable", nombreUsuario);
                            insertCmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now);

                            insertCmd.ExecuteNonQuery();
                        }

                        // Consultar Insumos del módulo copiado
                        query = "SELECT * FROM tblModulo_Insumo WHERE Id_Modulo = @IdModuloCopiado";
                        using (SqlCommand insumoCmd = new SqlCommand(query, connection))
                        {
                            insumoCmd.Parameters.AddWithValue("@IdModuloCopiado", moduloID);
                            SqlDataAdapter insumoAdapter = new SqlDataAdapter(insumoCmd);
                            DataTable insumoTable = new DataTable();
                            insumoAdapter.Fill(insumoTable);

                            foreach (DataRow insumoRow in insumoTable.Rows)
                            {
                                // Insertar insumos en tblModulo_Insumo
                                string insertInsumoQuery = @"INSERT INTO tblModulo_Insumo (Id_Modulo, Id_Insumo, Cantidad, DescripcionPieza, Responsable, FechaSuceso, DescuentoAltura, DescuentoAncho, Sentido, Costear, PiezaEscalable, AnchoFijo, AltoFijo, Divisiones)
                                                     VALUES (@IdModulo, @IdInsumo, @Cantidad, @DescripcionPieza, @Responsable, @FechaSuceso, @DescuentoAltura, @DescuentoAncho, @Sentido, @Costear, @PiezaEscalable, @AnchoFijo, @AltoFijo, @Divisiones)";
                                using (SqlCommand insertInsumoCmd = new SqlCommand(insertInsumoQuery, connection))
                                {
                                    insertInsumoCmd.Parameters.AddWithValue("@IdModulo", textModulo.Text);
                                    insertInsumoCmd.Parameters.AddWithValue("@IdInsumo", insumoRow["Id_Insumo"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@Cantidad", insumoRow["Cantidad"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@DescripcionPieza", insumoRow["DescripcionPieza"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@Responsable", insumoRow["Responsable"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@FechaSuceso", insumoRow["FechaSuceso"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@DescuentoAltura", insumoRow["DescuentoAltura"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@DescuentoAncho", insumoRow["DescuentoAncho"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@Sentido", insumoRow["Sentido"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@Costear", Convert.ToBoolean(insumoRow["Costear"]));
                                    insertInsumoCmd.Parameters.AddWithValue("@PiezaEscalable", Convert.ToBoolean(insumoRow["PiezaEscalable"]));
                                    insertInsumoCmd.Parameters.AddWithValue("@AnchoFijo", insumoRow["AnchoFijo"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@AltoFijo", insumoRow["AltoFijo"]);
                                    insertInsumoCmd.Parameters.AddWithValue("@Divisiones", insumoRow["Divisiones"]);

                                    insertInsumoCmd.ExecuteNonQuery();
                                }

                                // Obtener el último Id_ModuloInsumo insertado
                                SqlCommand getMaxIdCmd = new SqlCommand("SELECT MAX(id_ModuloInsumo) FROM tblModulo_Insumo", connection);
                                IDModuloInsumo = Convert.ToDouble(getMaxIdCmd.ExecuteScalar());

                                // Consultar y copiar rotación de insumos
                                string queryRotacion = @"SELECT tblRotacionInsumo.* FROM tblAreaProduccion 
                                                 INNER JOIN tblRotacionInsumo ON tblAreaProduccion.Id_Area = tblRotacionInsumo.riId_Area 
                                                 WHERE tblRotacionInsumo.riId_ModuloInsumo = @riIdModuloInsumo
                                                 ORDER BY tblRotacionInsumo.riEstacion";
                                using (SqlCommand rotacionCmd = new SqlCommand(queryRotacion, connection))
                                {
                                    rotacionCmd.Parameters.AddWithValue("@riIdModuloInsumo", insumoRow["Id_ModuloInsumo"]);
                                    SqlDataAdapter rotacionAdapter = new SqlDataAdapter(rotacionCmd);
                                    DataTable rotacionTable = new DataTable();
                                    rotacionAdapter.Fill(rotacionTable);

                                    foreach (DataRow rotacionRow in rotacionTable.Rows)
                                    {
                                        string insertRotacionQuery = @"INSERT INTO tblRotacionInsumo (riId_ModuloInsumo, riId_Area, riEstacion) 
                                                               VALUES (@IdModuloInsumo, @IdArea, @Estacion)";
                                        using (SqlCommand insertRotacionCmd = new SqlCommand(insertRotacionQuery, connection))
                                        {
                                            insertRotacionCmd.Parameters.AddWithValue("@IdModuloInsumo", IDModuloInsumo);
                                            insertRotacionCmd.Parameters.AddWithValue("@IdArea", rotacionRow["riId_Area"]);
                                            insertRotacionCmd.Parameters.AddWithValue("@Estacion", rotacionRow["riEstacion"]);
                                            insertRotacionCmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Si el módulo ya existe, mostrar mensaje
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('El módulo ya existe con la altura seleccionada');", true);
                    }
                }
            }
        }

        protected void Calcular_IdModulo()
        {
            string sSql = "SELECT MAX(Id_Modulo) FROM tblModulo";
            int nuevoIdModulo = 1;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sSql, connection);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado no es nulo
                    if (result != DBNull.Value && result != null)
                    {
                        // Incrementar el Id_Modulo si ya existe
                        nuevoIdModulo = Convert.ToInt32(result) + 1;
                    }

                    // Asignar el nuevo Id a un control de interfaz (por ejemplo, un TextBox)
                    textModulo.Text = nuevoIdModulo.ToString();
                }
                catch (Exception ex)
                {
                    // Manejo de errores (puedes implementar un logger o mostrar un mensaje)
                    Console.WriteLine("Error al calcular el Id_Modulo: " + ex.Message);
                }
            }
        }
        protected void EliminarViewState()
        {
            ViewState.Remove("DctoAncho");
            ViewState.Remove("DctoAltura");
            ViewState.Remove("Ancho");
            ViewState.Remove("Alto");
            ViewState.Remove("Divisiones");
        }

        protected void deshabilitarElementos()
        {
            // Deshabilitar y limpiar controles
            TextCantidadCon.Enabled = false;
            TextCantidadCon.Text = "0";
            TextCantidadCon.CssClass = "form-control form-control-sm ms-2";
            TextDctoAltura.Text = "0";
            TextDctoAltura.Enabled = false;
            TextDctoAltura.CssClass = "form-control form-control-sm";
            TextDctoAncho.Text = "0";
            TextDctoAncho.Enabled = false;
            TextDctoAncho.CssClass = "form-control form-control-sm";
            TextAncho.Text = "0";
            TextAncho.Enabled = false;
            TextAncho.CssClass = "form-control form-control-sm";
            TextAlto.Text = "0";
            TextAlto.Enabled = false;
            TextAlto.CssClass = "form-control form-control-sm";
            DropDownList1.Enabled = false;
            if (DropDownList1.SelectedItem != null && !string.IsNullOrEmpty(DropDownList1.SelectedItem.Text))
            {
                DropDownList1.DataBind();
            }
            txObs1.Disabled = true;
            txObs1.InnerText = string.Empty;

            BtnSubirEstacion.Enabled = false;
            BtnSubirEstacion.CssClass = "btn btn-sm shadow button-disabled";

            BtnBajarEstacion.Enabled = false;
            BtnBajarEstacion.CssClass = "btn btn-sm shadow button-disabled";

            BtnX2.Enabled = false;
            BtnX2.CssClass = "btn btn-sm shadow button-disabled";
        }

        protected void BtnAdicionarConfiguracion_Click(object sender, EventArgs e)
        {
            EliminarViewState();

            deshabilitarElementos();

            CheckCostearCon.Enabled = false;
            CheckPieEsCon.Enabled = false;
            TextDivisionesCon.Enabled = false;
            TextDivisionesCon.Text = "1";
            TextDivisionesCon.Attributes["data-tag"] = "1";
            TextDivisionesCon.CssClass = "form-control form-control-sm";

            BtnModificarConfiguracion.Enabled = false;
            BtnModificarConfiguracion.CssClass = "btn btn-sm shadow button-disabled2";

            TextCantidadCon.Enabled = true;
            CheckPieEsCon.Enabled = true;
            txObs1.Disabled = false;
            DropDownList1.Enabled = true;



            // Verificar si bordeDerecho está visible
            bool bordeDerechoVisible = ViewState["bordeDerechoVisible"] != null && (bool)ViewState["bordeDerechoVisible"];

            if (bordeDerechoVisible)
            {
                // Limpiar DataGrid4 y asegurarse de que esté vacío
                DataGrid4.DataSource = null;
                DataGrid4.DataBind();



                bordeDerecho.Attributes["class"] += " d-none"; // Oculta el panel

                bordeDerecho.Visible = false;

                ViewState["bordeDerechoVisible"] = false;

                // Deshabilitar la acción de DataGrid3
                foreach (DataGridItem item in DataGrid3.Items)
                {
                    LinkButton linkButton = item.FindControl("SelectInsumoID") as LinkButton;
                    if (linkButton != null)
                    {
                        linkButton.Enabled = true;
                    }
                }

                deshabilitarElementos();

                BtnAdicionarConfiguracion.CssClass = "btn btn-sm shadow button-enabled2 Verde";
                CheckPieEsCon.Checked = false;
                CheckPieEsCon.Enabled = false;
                CheckCostearCon.Enabled = false;

            }
            else
            {

                bordeDerecho.Attributes["class"] = bordeDerecho.Attributes["class"].Replace("d-none", "").Trim(); // Muestra el bordeDerecho


                bordeDerecho.Visible = true;

                ViewState["bordeDerechoVisible"] = true;

                // Deshabilitar la acción de DataGrid3
                foreach (DataGridItem item in DataGrid3.Items)
                {
                    LinkButton linkButton = item.FindControl("SelectInsumoID") as LinkButton;
                    if (linkButton != null)
                    {
                        linkButton.Enabled = false;
                    }
                }

                DropDownList3.Enabled = true;
                DropDownList3.CssClass = "form-control form-control-sm";


                BtnAdicionarConfiguracion.CssClass = "btn btn-sm shadow linkButtonClicked8 Verde";
                CheckPieEsCon.Checked = true;
                CheckPieEsCon.Enabled = true;
                CheckCostearCon.Enabled = true;
            }

            Session["CRUDModuloTabConfiguracion"] = "Nuevo";
        }

        protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "ModuloIns")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid3.Items[rowIndex];

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DataGrid3.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                ViewState["ModificarClickCount"] = null;

                DropDownList1.Items.Clear();
                string valorAsignado = row.Cells[4].Text;
                DropDownList1.Items.Add(valorAsignado);


                // Agregar elementos a cboSentido basado en el valor de Abreviado (similar a la lógica de VB6)
                switch (row.Cells[6].Text.ToUpper()) // Suponiendo que la columna 6 es "Abreviado"
                {
                    case "UND":

                        DropDownList1.Items.Add("NO APLICA");
                        DropDownList1.SelectedIndex = 0;
                        break;

                    case "CML":
                        // Supongamos que cboTipoModulo.SelectedValue es el equivalente a cboTipoModulo.ItemData(cboTipoModulo.ListIndex)
                        if (DropDownList2.SelectedValue == "3") // Módulo Complementario de Profundidad
                        {

                            DropDownList1.Items.Add("PROFUNDIDAD");
                            DropDownList1.Items.Add("VERTICAL");
                        }
                        else
                        {

                            DropDownList1.Items.Add("HORIZONTAL");
                            DropDownList1.Items.Add("VERTICAL");
                        }
                        break;

                    case "CM2":

                        DropDownList1.Items.Add("NO APLICA");
                        DropDownList1.SelectedIndex = 0;
                        break;
                }



                string tipoAccion = Session["Modulo"] as string;
                if (tipoAccion == "Consultar")
                {
                    BtnAdicionarConfiguracion.Enabled = false;
                    BtnAdicionarConfiguracion.CssClass = "btn btn-sm shadow button-disabled2";

                    BtnX.Enabled = false;
                    BtnX.CssClass = "btn btn-sm shadow button-disabled2";

                    BtnModificarConfiguracion.Enabled = false;
                    BtnModificarConfiguracion.CssClass = "btn btn-sm shadow button-disabled2";
                }
                else
                {
                    BtnX.Enabled = true;
                    BtnX.CssClass = "btn btn-sm shadow button-enabled2 RojoCancelar";

                    BtnModificarConfiguracion.Enabled = true;
                    BtnModificarConfiguracion.CssClass = "btn btn-sm shadow button-enabled2 Cafe";
                }

                // Asignar valores de la fila a los controles correspondientes
                CheckCostearCon.Checked = Convert.ToBoolean(row.Cells[13].Text);
                TextCantidadCon.Text = row.Cells[5].Text;
                TextDctoAltura.Text = row.Cells[9].Text;
                TextDctoAncho.Text = row.Cells[8].Text;
                txObs1.InnerText = row.Cells[18].Text;

                TextAncho.Text = row.Cells[10].Text;
                TextAlto.Text = row.Cells[11].Text;
                CheckPieEsCon.Checked = Convert.ToBoolean(row.Cells[7].Text);
                TextDivisionesCon.Text = row.Cells[12].Text;

                DropDownList3.Enabled = false;
                DropDownList3.CssClass = "form-control form-control-sm";
                CargarAreaProduccion();


                // Llenar el DataGrid1 con los datos de la consulta basada en el riId_ModuloInsumo
                string riId_ModuloInsumo = row.Cells[17].Text;
                Session["Id_ModuloInsumoDatagrid3"] = riId_ModuloInsumo;
                LlenarDataGrid1(riId_ModuloInsumo);

                string idInsumo = row.Cells[15].Text;
                Session["Id_InsumoDatagrid3"] = idInsumo;

                // Store the selected row index in the DataGrid attribute
                DataGrid3.Attributes["SelectedRowIndex"] = rowIndex.ToString();

                // Scroll to the row
                row.Attributes["id"] = "DataGrid3_row_" + rowIndex;
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('DataGrid3_row_" + rowIndex + "');", true);

                bordeDerecho.Attributes["class"] = bordeDerecho.Attributes["class"].Replace("d-none", "").Trim(); // Muestra el bordeDerecho

                bordeDerecho.Visible = true;


                BtnAdicionarRotacion.Enabled = false;
                BtnAdicionarRotacion.CssClass = "btn btn-sm shadow button-disabled";

                BtnSubirEstacion.Enabled = false;
                BtnSubirEstacion.CssClass = "btn btn-sm shadow button-disabled";

                BtnBajarEstacion.Enabled = false;
                BtnBajarEstacion.CssClass = "btn btn-sm shadow button-disabled";

                BtnX2.Enabled = false;
                BtnX2.CssClass = "btn btn-sm shadow button-disabled";


                ViewState["bordeDerechoVisible"] = true;

            }
        }

        private void LlenarDataGrid1(string riId_ModuloInsumo)
        {
            string sSql = @"SELECT tblAreaProduccion.*, tblRotacionInsumo.*
                    FROM tblAreaProduccion
                    INNER JOIN tblRotacionInsumo ON tblAreaProduccion.Id_Area = tblRotacionInsumo.riId_Area
                    WHERE tblRotacionInsumo.riId_ModuloInsumo = @riId_ModuloInsumo
                    ORDER BY tblRotacionInsumo.riEstacion";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sSql, conn);
                cmd.Parameters.AddWithValue("@riId_ModuloInsumo", riId_ModuloInsumo);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        DataGrid4.DataSource = dt;
                        DataGrid4.DataBind();
                    }
                    else
                    {

                        DataGrid4.DataSource = null;
                        DataGrid4.DataBind();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        protected void DataGrid4_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "ModuloRotacion")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid4.Items[rowIndex];

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DataGrid4.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                row.CssClass = "fila-seleccionada1"; // Cambiado para usar row.CssClass

                Session["riId"] = row.Cells[3].Text;

                // Habilitar botones según la lógica existente
                if (ViewState["ModificarClickCount"] != null)
                {
                    if ((int)ViewState["ModificarClickCount"] == 1)
                    {
                        BtnSubirEstacion.Enabled = true;
                        BtnSubirEstacion.CssClass = "btn btn-sm shadow button-enabled2 Verde";

                        BtnBajarEstacion.Enabled = true;
                        BtnBajarEstacion.CssClass = "btn btn-sm shadow button-enabled2 Verde";

                        BtnX2.Enabled = true;
                        BtnX2.CssClass = "btn btn-sm shadow button-enabled2 RojoCancelar";
                    }
                    else
                    {
                        BtnSubirEstacion.Enabled = false;
                        BtnSubirEstacion.CssClass = "btn btn-sm shadow button-disabled";

                        BtnBajarEstacion.Enabled = false;
                        BtnBajarEstacion.CssClass = "btn btn-sm shadow button-disabled";

                        BtnX2.Enabled = false;
                        BtnX2.CssClass = "btn btn-sm shadow button-disabled";
                    }
                }
            }
        }


        public void CargarAreaProduccion()
        {
            // Define la cadena de conexión (ajústala según tu configuración)
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Define la consulta SQL
            string sSql = "SELECT Id_Area, Descripcion_Area FROM tblAreaProduccion ORDER BY Descripcion_Area ASC";

            try
            {
                // Conexión a la base de datos
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Abre la conexión
                    conn.Open();

                    // Define el comando SQL
                    using (SqlCommand cmd = new SqlCommand(sSql, conn))
                    {
                        // Ejecuta la consulta y obtiene los resultados
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Limpia el DropDownList antes de llenarlo
                            DropDownList3.Items.Clear();

                            DropDownList3.Items.Add(new ListItem(string.Empty, string.Empty));

                            // Agrega los elementos al DropDownList
                            while (reader.Read())
                            {
                                // Crea un nuevo ListItem con la descripción y el Id como valor
                                ListItem item = new ListItem(reader["Descripcion_Area"].ToString(), reader["Id_Area"].ToString());

                                // Agrega el ListItem al DropDownList
                                DropDownList3.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes ajustar este mensaje según tus necesidades)
                Console.WriteLine("Error al cargar las áreas de producción: " + ex.Message);
            }

        }

        protected void CheckPieEsCon_CheckedChanged(object sender, EventArgs e)
        {
            // Deshabilitar los elementos inicialmente
            TextDctoAncho.Enabled = false;
            TextDctoAltura.Enabled = false;
            TextAncho.Enabled = false;
            TextAlto.Enabled = false;
            TextDivisionesCon.Enabled = false;

            // Aplicar la lógica según el valor del checkbox 'CheckPieEsCon'
            if (CheckPieEsCon.Checked)
            {
                // Caso 1: Checkbox 'CheckPieEsCon' está marcado (habilitar Pieza Escalable)
                TextDctoAncho.Enabled = true;
                TextDctoAltura.Enabled = true;
                TextDivisionesCon.Enabled = true;

                // Restaurar valores desde ViewState, si existen
                if (ViewState["DctoAncho"] != null)
                {
                    TextDctoAncho.Text = ViewState["DctoAncho"].ToString();
                }

                if (ViewState["DctoAltura"] != null)
                {
                    TextDctoAltura.Text = ViewState["DctoAltura"].ToString();
                }

                if (ViewState["Divisiones"] != null)
                {
                    TextDivisionesCon.Text = ViewState["Divisiones"].ToString();
                }

                // Desactivar valores de la pieza fija
                TextAncho.Text = "0";
                TextAlto.Text = "0";
            }
            else
            {
                // Caso 0: Checkbox 'CheckPieEsCon' no está marcado (habilitar Pieza Fija)
                TextAncho.Enabled = true;
                TextAlto.Enabled = true;

                // Restaurar valores desde ViewState, si existen
                if (ViewState["Ancho"] != null)
                {
                    TextAncho.Text = ViewState["Ancho"].ToString();
                }

                if (ViewState["Alto"] != null)
                {
                    TextAlto.Text = ViewState["Alto"].ToString();
                }

                // Desactivar valores de la pieza escalable
                TextDctoAncho.Text = "0";
                TextDctoAltura.Text = "0";
            }

        }

        protected void BtnModificarConfiguracion_Click(object sender, EventArgs e)
        {
            // Guardar los valores en el ViewState
            ViewState["DctoAncho"] = TextDctoAncho.Text;
            ViewState["DctoAltura"] = TextDctoAltura.Text;
            ViewState["Ancho"] = TextAncho.Text;
            ViewState["Alto"] = TextAlto.Text;
            ViewState["Divisiones"] = TextDivisionesCon.Text;

            // Deshabilitar todos los controles inicialmente
            TextCantidadCon.Enabled = false;
            CheckPieEsCon.Enabled = false;
            txObs1.Disabled = true; // Deshabilitar el TextArea
            DropDownList1.Enabled = false; // Asumiendo que este es cboAreaProduccion
            CheckCostearCon.Enabled = false; // Asumiendo que es chkCostear
            TextDivisionesCon.Enabled = false; // Asumiendo que es txtDivisiones

            // Comprobar si es la primera vez que se hace clic en el botón
            if (ViewState["ModificarClickCount"] == null)
            {
                // Primera vez
                ViewState["ModificarClickCount"] = 1; // Inicializa la cuenta
            }
            else
            {
                // Incrementar el contador
                ViewState["ModificarClickCount"] = (int)ViewState["ModificarClickCount"] + 1;
            }

            // Lógica para habilitar controles
            if ((int)ViewState["ModificarClickCount"] == 1)
            {
                // Lógica para el primer clic
                TextCantidadCon.Enabled = true;
                CheckPieEsCon.Enabled = true;
                txObs1.Disabled = false; // Habilitar el TextArea
                DropDownList1.Enabled = true; // Habilitar cboAreaProduccion
                CheckCostearCon.Enabled = true;
                TextDivisionesCon.Enabled = CheckPieEsCon.Checked;

                DropDownList3.Enabled = true;
                DropDownList3.CssClass = "form-control form-control-sm";

                BtnAdicionarRotacion.Enabled = true;
                BtnAdicionarRotacion.CssClass = "btn btn-sm shadow button-enabled2 ColorAzulActivo";

                CheckPieEsCon_CheckedChanged(sender, e);

                BtnModificarConfiguracion.CssClass = "btn btn-sm shadow linkButtonClicked8 Cafe";
            }
            else
            {
                DropDownList3.Enabled = false;
                DropDownList3.CssClass = "form-control form-control-sm";

                BtnAdicionarRotacion.Enabled = false;
                BtnAdicionarRotacion.CssClass = "btn btn-sm shadow button-disabled";

                // Deshabilitar y limpiar controles
                TextCantidadCon.Enabled = false;
                TextCantidadCon.CssClass = "form-control form-control-sm ms-2";


                TextDctoAltura.Enabled = false;
                TextDctoAltura.CssClass = "form-control form-control-sm";

                TextDctoAncho.Enabled = false;
                TextDctoAncho.CssClass = "form-control form-control-sm";

                TextAncho.Enabled = false;
                TextAncho.CssClass = "form-control form-control-sm";

                TextAlto.Enabled = false;
                TextAlto.CssClass = "form-control form-control-sm";

                DropDownList1.Enabled = false;

                txObs1.Disabled = true;

                BtnSubirEstacion.Enabled = false;
                BtnSubirEstacion.CssClass = "btn btn-sm shadow button-disabled";

                BtnBajarEstacion.Enabled = false;
                BtnBajarEstacion.CssClass = "btn btn-sm shadow button-disabled";

                BtnX2.Enabled = false;
                BtnX2.CssClass = "btn btn-sm shadow button-disabled";

                BtnModificarConfiguracion.CssClass = "btn btn-sm shadow button-enabled2 Cafe";

                ViewState["ModificarClickCount"] = null;
            }

            Session["CRUDModuloTabConfiguracion"] = "Modificar";

        }

        protected void BtnGrabarConfiguracion_Click(object sender, EventArgs e)
        {

            string NombreUsuario = Session["usuariologueado"].ToString();
            string tipoAccion = Session["CRUDModuloTabConfiguracion"] as string;
            int idInsumo = Convert.ToInt32(Session["Id_InsumoDatagrid3"]);
            int IDModuloInsumo = Convert.ToInt32(Session["Id_ModuloInsumoDatagrid3"]);
            string moduloID = textModulo.Text;

            // Obtener la cadena de conexión 
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;


            if (tipoAccion == "Nuevo")
            {
                // Verificar si hay una fila seleccionada en el DataGrid
                string selectedRowIndexStr = DataGrid1.Attributes["SelectedRowIndex2"];
                if (string.IsNullOrEmpty(selectedRowIndexStr) || !int.TryParse(selectedRowIndexStr, out int selectedRowIndex))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Señor usuario, por favor seleccione el Insumo');", true);
                    return; // Detener la ejecución si no hay fila seleccionada
                }
                else
                {

                }
                // Validar cantidad
                if (string.IsNullOrWhiteSpace(TextCantidadCon.Text) || Convert.ToDouble(TextCantidadCon.Text) <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Digite una cantidad válida del Insumo en el Módulo');", true);
                    TextCantidadCon.Focus();
                    return;
                }

                // Validar otros campos
                if (
                    double.TryParse(TextCantidadCon.Text, out _) &&
                    double.TryParse(TextDctoAltura.Text, out _) &&
                    double.TryParse(TextDctoAncho.Text, out _) &&
                    !string.IsNullOrWhiteSpace(DropDownList1.SelectedItem.Text) &&
                    !string.IsNullOrWhiteSpace(DropDownList3.SelectedItem.Text) &&
                    CheckCostearCon.Checked &&
                    double.TryParse(TextAncho.Text, out _) &&
                    double.TryParse(TextAlto.Text, out _) &&

                    Convert.ToDouble(TextDivisionesCon.Text) > 0)
                {
                    try
                    {
                        // Se ingresa el nuevo insumo del módulo
                        int idModuloInsumo = InsertarModuloInsumo(connectionString, textModulo.Text, idInsumo, NombreUsuario);
                        if (idModuloInsumo > 0)
                        {
                            // Se calcula la estación
                            int estacion = CalcularEstacion(connectionString, idModuloInsumo);

                            // Se inserta la rotación del insumo
                            InsertarRotacionInsumo(connectionString, idModuloInsumo, DropDownList3.SelectedValue, estacion);

                            // Se coloca el módulo como no chequeado
                            ActualizarModuloChequeado(connectionString, textModulo.Text);

                            LlenarDataGrid3(moduloID);

                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Los registros se insertaron correctamente');", true);

                            bool swCambiosenelDespiece = true;

                            // Si hubo cambios en el despiece, se actualiza la base de datos con la lógica migrada
                            if (swCambiosenelDespiece)
                            {
                                ActualizarDespiece(connectionString, moduloID);
                            }

                        }
                    }
                    catch (SqlException ex)
                    {
                        // Manejo de excepciones
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Error al realizar la operación: {ex.Message}');", true);
                        Response.Redirect("DiseñoYDesarrollo/Modulo.aspx");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Los campos en rojo son obligatorios');", true);
                }
            }

            if (tipoAccion == "Modificar")
            {
                if (
                    double.TryParse(TextCantidadCon.Text, out _) &&
                    double.TryParse(TextDctoAltura.Text, out _) &&
                    double.TryParse(TextDctoAncho.Text, out _) &&
                    CheckCostearCon.Checked &&
                    !string.IsNullOrWhiteSpace(DropDownList1.SelectedItem.Text))
                {
                    bool updateSuccess, unmarkSuccess;

                    ActualizarModuloInsumo(IDModuloInsumo, connectionString, NombreUsuario, out updateSuccess);

                    //Cargar_Panel_Modulos_Insumos(dtgdModulo_Insumos, textModulo.Text);
                    //chkModificarComposicion.Enabled = false;
                    //chkModificarComposicion.Checked = false;

                    //rsModulo_Insumos.Find("id_ModuloInsumo='" + IDModuloInsumo + "'");
                    //dtgdModulo_Insumos.SelBookmarks.Add(rsModulo_Insumos.Bookmark);
                    //dtgdModulo_Insumos_SelChange(1);

                    DesmarcarModulo(connectionString, out unmarkSuccess);
                    LlenarDataGrid3(moduloID);

                    if (updateSuccess && unmarkSuccess)
                    {
                        // Aquí colocamos la variable de control que indica que hubo cambios en el despiece
                        bool swCambiosenelDespiece = true;

                        // Si hubo cambios en el despiece, se actualiza la base de datos con la lógica migrada
                        if (swCambiosenelDespiece)
                        {
                            ActualizarDespiece(connectionString, moduloID);
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Los registros se actualizaron correctamente');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('No se pudo realizar la actualización, por favor intente nuevamente.');", true);
                        Response.Redirect("DiseñoYDesarrollo/Modulo.aspx");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Los campos en rojo son obligatorios');", true);
                }
            }

            Session.Remove("CRUDModuloTabConfiguracion");
            Session.Remove("Id_InsumoDatagrid3");
            Session.Remove("Id_ModuloInsumoDatagrid3");
        }

        private void ActualizarDespiece(string connectionString, string moduloID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Primera consulta para actualizar tblPanel
                string sSql1 = "UPDATE tblPanel SET Chequeado = 0, Precio_Venta = 0 FROM tblPanel INNER JOIN tblPanel_Modulo ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum " +
                               "WHERE tblPanel_Modulo.Id_Modulo = @moduloID";

                using (SqlCommand cmd1 = new SqlCommand(sSql1, conn))
                {
                    cmd1.Parameters.AddWithValue("@moduloID", moduloID);
                    cmd1.ExecuteNonQuery();
                }

                // Segunda consulta para verificar y actualizar prototipos
                string sSql2 = "UPDATE tblPanel SET Precio_Venta = 0 WHERE Id_Panel IN " +
                               "(SELECT tblPlano.Plano FROM tblPanel " +
                               "INNER JOIN tblPanel_Modulo ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum " +
                               "INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum " +
                               "INNER JOIN tblPlano ON tblPlano_Panel.Id_Plano = tblPlano.Plano " +
                               "WHERE tblPanel_Modulo.Id_Modulo = @moduloID AND tblPlano.Tipologia = 1 " +
                               "GROUP BY tblPlano.Plano)";

                using (SqlCommand cmd2 = new SqlCommand(sSql2, conn))
                {
                    cmd2.Parameters.AddWithValue("@moduloID", moduloID);
                    cmd2.ExecuteNonQuery();
                }
            }
        }


        private int InsertarModuloInsumo(string connectionString, string idModulo, int idInsumo, string NombreUsuario)
        {
            string sSql = @"
        INSERT INTO tblModulo_Insumo 
        (Id_Modulo, Id_Insumo, Cantidad, DescripcionPieza, Responsable, FechaSuceso, 
        DescuentoAltura, DescuentoAncho, Sentido, Costear, PiezaEscalable, 
        AnchoFijo, AltoFijo, Divisiones) 
        VALUES 
        (@IdModulo, @IdInsumo, @Cantidad, @DescripcionPieza, @Responsable, 
        GETDATE(), @DescuentoAltura, @DescuentoAncho, @Sentido, 
        @Costear, @PiezaEscalable, @AnchoFijo, @AltoFijo, @Divisiones); 
        SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sSql, connection))
            {
                command.Parameters.AddWithValue("@IdModulo", idModulo);
                command.Parameters.AddWithValue("@IdInsumo", idInsumo);
                command.Parameters.AddWithValue("@Cantidad", Convert.ToDouble(TextCantidadCon.Text));
                command.Parameters.AddWithValue("@DescripcionPieza", txObs1.Value);
                command.Parameters.AddWithValue("@Responsable", NombreUsuario);
                command.Parameters.AddWithValue("@DescuentoAltura", Convert.ToDouble(TextDctoAltura.Text));
                command.Parameters.AddWithValue("@DescuentoAncho", Convert.ToDouble(TextDctoAncho.Text));
                command.Parameters.AddWithValue("@Sentido", DropDownList1.SelectedItem.Text);
                command.Parameters.AddWithValue("@Costear", CheckCostearCon.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@PiezaEscalable", CheckPieEsCon.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@AnchoFijo", Convert.ToDouble(TextAncho.Text));
                command.Parameters.AddWithValue("@AltoFijo", Convert.ToDouble(TextAlto.Text));
                command.Parameters.AddWithValue("@Divisiones", Convert.ToDouble(TextDivisionesCon.Text));

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()); // Devuelve el ID del nuevo módulo insumo
            }
        }

        private int CalcularEstacion(string connectionString, int idModuloInsumo)
        {
            string sSql = "SELECT COALESCE(MAX(riEstacion), 0) + 1 FROM tblRotacionInsumo WHERE riId_ModuloInsumo=@IdModuloInsumo;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sSql, connection))
            {
                command.Parameters.AddWithValue("@IdModuloInsumo", idModuloInsumo);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private void InsertarRotacionInsumo(string connectionString, int idModuloInsumo, string idArea, int estacion)
        {
            string sSql = "INSERT INTO tblRotacionInsumo (riId_ModuloInsumo, riId_Area, riEstacion) VALUES (@IdModuloInsumo, @IdArea, @Estacion);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sSql, connection))
            {
                command.Parameters.AddWithValue("@IdModuloInsumo", idModuloInsumo);
                command.Parameters.AddWithValue("@IdArea", idArea);
                command.Parameters.AddWithValue("@Estacion", estacion);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void ActualizarModuloChequeado(string connectionString, string idModulo)
        {
            string sSql = "UPDATE tblModulo SET Chequeado = 0 WHERE Id_Modulo = @IdModulo;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sSql, connection))
            {
                command.Parameters.AddWithValue("@IdModulo", idModulo);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ActualizarModuloInsumo(int idModuloInsumo, string connectionString, string NombreUsuario, out bool success)
        {
            string sSql = @"
    UPDATE tblModulo_Insumo 
    SET 
        Costear = @Costear, 
        Cantidad = @Cantidad, 
        DescripcionPieza = @DescripcionPieza, 
        Responsable = @Responsable, 
        FechaSuceso = @FechaSuceso, 
        DescuentoAltura = @DescuentoAltura, 
        DescuentoAncho = @DescuentoAncho, 
        Sentido = @Sentido, 
        PiezaEscalable = @PiezaEscalable, 
        AnchoFijo = @AnchoFijo, 
        AltoFijo = @AltoFijo, 
        Divisiones = @Divisiones 
    WHERE ID_moduloinsumo = @IDModuloInsumo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sSql, connection))
            {
                // Asignar los parámetros de forma segura
                command.Parameters.AddWithValue("@Costear", CheckCostearCon.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@Cantidad", Convert.ToDouble(TextCantidadCon.Text));
                command.Parameters.AddWithValue("@DescripcionPieza", txObs1.Value);
                command.Parameters.AddWithValue("@Responsable", NombreUsuario);
                command.Parameters.AddWithValue("@FechaSuceso", DateTime.Now);
                command.Parameters.AddWithValue("@DescuentoAltura", Convert.ToDouble(TextDctoAltura.Text));
                command.Parameters.AddWithValue("@DescuentoAncho", Convert.ToDouble(TextDctoAncho.Text));
                command.Parameters.AddWithValue("@Sentido", DropDownList1.SelectedItem.Text);
                command.Parameters.AddWithValue("@PiezaEscalable", CheckPieEsCon.Checked ? 1 : 0);
                command.Parameters.AddWithValue("@AnchoFijo", Convert.ToDouble(TextAncho.Text));
                command.Parameters.AddWithValue("@AltoFijo", Convert.ToDouble(TextAlto.Text));
                command.Parameters.AddWithValue("@Divisiones", Convert.ToDouble(TextDivisionesCon.Text));
                command.Parameters.AddWithValue("@IDModuloInsumo", idModuloInsumo);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    success = rowsAffected > 0; // Retorna true si se actualizó al menos una fila
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error al actualizar el módulo insumo: {ex.Message}");
                    success = false; // Retorna false si ocurre una excepción
                }
            }
        }

        public void DesmarcarModulo(string connectionString, out bool success)
        {
            int idModulo = int.Parse(textModulo.Text);

            string sSql = @"
    UPDATE tblModulo 
    SET Chequeado = @Chequeado 
    WHERE Id_Modulo = @IdModulo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sSql, connection))
            {
                command.Parameters.AddWithValue("@Chequeado", 0);
                command.Parameters.AddWithValue("@IdModulo", idModulo);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    success = rowsAffected > 0; // Retorna true si se actualizó al menos una fila
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error al desmarcar el módulo: {ex.Message}");
                    success = false; // Retorna false si ocurre una excepción
                }
            }
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridInsumo")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid1.Items[rowIndex];

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DataGrid1.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                Session["Id_InsumoDatagrid3"] = row.Cells[1].Text;




                // Store the selected row index in the DataGrid attribute
                DataGrid1.Attributes["SelectedRowIndex2"] = rowIndex.ToString();

                // Asignar ID único a la fila
                row.Attributes["id"] = "DataGrid2_row_" + rowIndex;
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('DataGrid2_row_" + rowIndex + "');", true);

            }
        }

        protected void BtnX_Click(object sender, EventArgs e)
        {
            string idInsumo = Session["Id_InsumoDatagrid3"].ToString();
            string contenidoModalOT = "Esta seguro que quiere eliminar el insumo: " + idInsumo + " del módulo " + textModulo.Text + " - " + TextDescripcionModulo.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#EliminarInsumo').modal('show'); $('#EliminarInsumo2').text('" + contenidoModalOT + "');", true);
        }

        protected void eliminarInsumoModulo_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string idModuloInsumo = Session["Id_ModuloInsumoDatagrid3"].ToString();
            string idModulo = textModulo.Text;

            //// Registrar el movimiento (esto depende de tu implementación de 'Registrar_Movimiento')
            //string cedula = Session["Cedula"].ToString(); // Suponiendo que la cédula del usuario está en la sesión.
            //RegistrarMovimiento($"El Usuario Con cédula: {cedula} Elimina Insumo {idInsumo} con el modulo {idModulo}");

            string sSql = $"DELETE FROM tblModulo_Insumo WHERE Id_ModuloInsumo = {idModuloInsumo}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sSql, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }

            // Actualizar 'tblModulo' para marcar como no chequeado
            sSql = $"UPDATE tblModulo SET Chequeado = 0 WHERE Id_Modulo = {idModulo}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sSql, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }

            ActualizarDespiece(connectionString, idModulo);

            // Deshabilitar y limpiar los campos en la interfaz de usuario
            TextCantidadCon.Enabled = false;
            TextCantidadCon.Text = "0";
            CheckPieEsCon.Enabled = false;
            TextDctoAltura.Text = "0";
            TextDctoAncho.Text = "0";
            txObs1.Disabled = true;
            txObs1.Value = string.Empty;
            DropDownList3.Enabled = false;
            DropDownList3.SelectedIndex = -1;
            DropDownList1.DataBind();
            CheckCostearCon.Enabled = false;
            TextAncho.Text = "0";
            TextAlto.Text = "0";
            CheckPieEsCon.Checked = false;

            LlenarDataGrid3(idModulo);

            BtnModificarConfiguracion.Enabled = false;
            BtnX.Enabled = false; // Deshabilitar el botón de eliminación.
        }

        protected void BtnAdicionarRotacion_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Validar que se haya seleccionado un valor en el combo de área de producción.
            if (!string.IsNullOrEmpty(DropDownList3.SelectedValue))
            {
                double Estacion = 0;
                string idModuloInsumo = Session["Id_ModuloInsumoDatagrid3"].ToString();
                string idArea = DropDownList3.SelectedValue;

                // Verificar si el registro ya existe.
                string checkSql = $"SELECT COUNT(1) FROM tblRotacionInsumo WHERE riId_ModuloInsumo = {idModuloInsumo} AND riId_Area = {idArea}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(checkSql, connection);
                    connection.Open();

                    // Ejecutar la consulta para verificar la existencia del registro.
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        // Si ya existe el registro, mostrar un mensaje de advertencia.
                        string script = "alert('El registro con el mismo módulo e id de área ya existe.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
                        return;
                    }
                }

                // Si el registro no existe, proceder con la inserción.
                // Construir la consulta SQL para obtener el máximo de 'riEstacion'.
                string sSql = $"SELECT ISNULL(MAX(riEstacion), 0) FROM tblRotacionInsumo WHERE riId_ModuloInsumo = {idModuloInsumo}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sSql, connection);
                    connection.Open();

                    // Ejecutar la consulta y obtener el valor de 'Estacion'.
                    object result = command.ExecuteScalar();
                    Estacion = 1 + Convert.ToDouble(result ?? 0);
                }

                // Construir la consulta SQL para insertar la nueva rotación del insumo.
                sSql = $"INSERT INTO tblRotacionInsumo (riId_ModuloInsumo, riId_Area, riEstacion) VALUES ({idModuloInsumo}, {idArea}, {Estacion})";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sSql, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Llamar al método para recargar los datos en el DataGrid.
                LlenarDataGrid1(idModuloInsumo);
            }
            else
            {
                // Mostrar un mensaje de advertencia al usuario si no se ha seleccionado un área de producción.
                string script = "alert('Seleccionar el destino del insumo');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
            }
        }

        protected void BtnSubirEstacion_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string idModuloInsumo = Session["Id_ModuloInsumoDatagrid3"].ToString();
            int riId = Convert.ToInt32(Session["riId"]); // Obtén el riId desde la sesión.

            int riEstacion = 0;

            // Abrir la conexión y realizar la consulta para obtener el valor de riEstacion.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtener el valor de riEstacion desde la base de datos.
                string query = "SELECT riEstacion FROM tblRotacionInsumo WHERE riId = @riId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@riId", riId);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        riEstacion = Convert.ToInt32(result);
                    }
                }

                // Realizar las actualizaciones si se obtuvo un riEstacion válido.
                if (riEstacion > 1)
                {
                    // Actualizar la estación superior (incrementarla en 1).
                    string sSql1 = $"UPDATE tblRotacionInsumo SET riEstacion = riEstacion + 1 WHERE riId_ModuloInsumo = @idModuloInsumo AND riEstacion = @riEstacionMenosUno";
                    using (SqlCommand command1 = new SqlCommand(sSql1, connection))
                    {
                        command1.Parameters.AddWithValue("@idModuloInsumo", idModuloInsumo);
                        command1.Parameters.AddWithValue("@riEstacionMenosUno", riEstacion - 1);
                        command1.ExecuteNonQuery();
                    }

                    // Actualizar la estación actual (decrementarla en 1).
                    string sSql2 = $"UPDATE tblRotacionInsumo SET riEstacion = riEstacion - 1 WHERE riId = @riId";
                    using (SqlCommand command2 = new SqlCommand(sSql2, connection))
                    {
                        command2.Parameters.AddWithValue("@riId", riId);
                        command2.ExecuteNonQuery();
                    }
                }
                else
                {
                    string script = "alert('No se encontró el riEstacion para el riId especificado.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
                }
            }

            // Recargar la grilla de rotación de insumos.
            LlenarDataGrid1(idModuloInsumo);
        }

        protected void BtnBajarEstacion_Click(object sender, EventArgs e)
        {
            // Obtener la cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Obtener el Id_ModuloInsumo desde la sesión (o desde otro lugar adecuado)
            string idModuloInsumo = Session["Id_ModuloInsumoDatagrid3"].ToString();

            // Obtener el valor de riId desde la sesión (asegúrate de haberlo almacenado al seleccionar la fila)
            int riId = Convert.ToInt32(Session["riId"]);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtener el valor actual de riEstacion para el riId seleccionado
                int riEstacionActual = 0;
                string selectQuery = "SELECT riEstacion FROM tblRotacionInsumo WHERE riId = @riId";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@riId", riId);
                    riEstacionActual = Convert.ToInt32(selectCommand.ExecuteScalar());
                }

                // Verificar si la estación actual es menor que el número total de registros para este módulo de insumo
                string countQuery = "SELECT COUNT(*) FROM tblRotacionInsumo WHERE riId_ModuloInsumo = @idModuloInsumo";
                int totalEstaciones = 0;
                using (SqlCommand countCommand = new SqlCommand(countQuery, connection))
                {
                    countCommand.Parameters.AddWithValue("@idModuloInsumo", idModuloInsumo);
                    totalEstaciones = Convert.ToInt32(countCommand.ExecuteScalar());
                }

                if (riEstacionActual < totalEstaciones)
                {
                    // Disminuir la estación de la fila siguiente (riEstacion+1)
                    string updateQuery1 = "UPDATE tblRotacionInsumo SET riEstacion = riEstacion - 1 WHERE riId_ModuloInsumo = @idModuloInsumo AND riEstacion = @riEstacion";
                    using (SqlCommand updateCommand1 = new SqlCommand(updateQuery1, connection))
                    {
                        updateCommand1.Parameters.AddWithValue("@idModuloInsumo", idModuloInsumo);
                        updateCommand1.Parameters.AddWithValue("@riEstacion", riEstacionActual + 1);
                        updateCommand1.ExecuteNonQuery();
                    }

                    // Aumentar la estación del registro seleccionado
                    string updateQuery2 = "UPDATE tblRotacionInsumo SET riEstacion = riEstacion + 1 WHERE riId = @riId";
                    using (SqlCommand updateCommand2 = new SqlCommand(updateQuery2, connection))
                    {
                        updateCommand2.Parameters.AddWithValue("@riId", riId);
                        updateCommand2.ExecuteNonQuery();
                    }

                    // Recargar la grilla de rotación de insumos
                    LlenarDataGrid1(idModuloInsumo);
                }
            }
        }

        protected void BtnX2_Click(object sender, EventArgs e)
        {
            // Obtener la cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Obtener el Id_ModuloInsumo desde la sesión (o desde otro lugar adecuado)
            string idModuloInsumo = Session["Id_ModuloInsumoDatagrid3"].ToString();

            // Obtener el valor de riId desde la sesión (asegúrate de haberlo almacenado al seleccionar la fila)
            int riId = Convert.ToInt32(Session["riId"]);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtener el valor actual de riEstacion para el riId seleccionado
                int riEstacionActual = 0;
                string selectQuery = "SELECT riEstacion FROM tblRotacionInsumo WHERE riId = @riId";
                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@riId", riId);
                    riEstacionActual = Convert.ToInt32(selectCommand.ExecuteScalar());
                }

                // Verificar la cantidad de registros para este módulo de insumo
                string countQuery = "SELECT COUNT(*) FROM tblRotacionInsumo WHERE riId_ModuloInsumo = @idModuloInsumo";
                int totalRegistros = 0;
                using (SqlCommand countCommand = new SqlCommand(countQuery, connection))
                {
                    countCommand.Parameters.AddWithValue("@idModuloInsumo", idModuloInsumo);
                    totalRegistros = Convert.ToInt32(countCommand.ExecuteScalar());
                }

                if (totalRegistros > 1)
                {
                    // Disminuir la estación de los registros cuya riEstacion sea mayor a la del registro eliminado
                    string updateQuery = "UPDATE tblRotacionInsumo SET riEstacion = riEstacion - 1 WHERE riId_ModuloInsumo = @idModuloInsumo AND riEstacion > @riEstacion";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@idModuloInsumo", idModuloInsumo);
                        updateCommand.Parameters.AddWithValue("@riEstacion", riEstacionActual);
                        updateCommand.ExecuteNonQuery();
                    }

                    // Eliminar el registro de rotación de insumo
                    string deleteQuery = "DELETE FROM tblRotacionInsumo WHERE riId = @riId";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@riId", riId);
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Recargar la grilla de rotación de insumos
                    LlenarDataGrid1(idModuloInsumo);
                }
                else
                {
                    // Mostrar un mensaje de advertencia si solo queda un registro y se intenta eliminar
                    string script = "alert('Como mínimo debe quedar un destino para el Insumo');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
                }
            }
        }

        protected void IDDatagridFamilia_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridFamilia")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = IDDatagridFamilia.Items[rowIndex];

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in IDDatagridFamilia.Items)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
                row.CssClass = "fila-seleccionada1";
                if (ValidarPermisoArea(33))
                {
                    BtnModificarFamilia.Enabled = true;
                    BtnModificarFamilia.CssClass = "btn btn-sm shadow button-enabled2 Cafe";
                }
                else
                {
                    BtnModificarFamilia.Enabled = false;
                    BtnModificarFamilia.CssClass = "btn btn-sm shadow button-disabled";
                }
              

                // Verificar y asignar valores a los TextBox, manejando el caso de &nbsp;
                TextDescripcion.Text = row.Cells[2].Text != "&nbsp;" ? row.Cells[2].Text : string.Empty;
                TextDescuentoFinal.Text = row.Cells[3].Text != "&nbsp;" ? row.Cells[3].Text : string.Empty;
                TextResponsableFamilia.Text = row.Cells[6].Text != "&nbsp;" ? row.Cells[6].Text : string.Empty;
                TextDescripcionParaObjeto.Text = row.Cells[4].Text != "&nbsp;" ? row.Cells[4].Text : string.Empty;

                // Convertir el valor de la celda 5 a DateTime
                string fechaTexto = row.Cells[5].Text;
                if (fechaTexto != "&nbsp;" && DateTime.TryParse(fechaTexto, out DateTime fechaModificacion))
                {
                    // Asignar el valor formateado al TextBox como "yyyy-MM-dd"
                    TextUlAct.Text = fechaModificacion.ToString("yyyy-MM-dd");
                }
                else
                {
                    // Si la conversión falla o es &nbsp;, dejar el campo vacío
                    TextUlAct.Text = string.Empty;
                }

                string idFamilia = row.Cells[1].Text;
                Session["ID_Familia"] = idFamilia;
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
            SELECT tblProcesoModulo.*, tblProcesoProduccion.* 
            FROM tblProcesoModulo 
            INNER JOIN tblProcesoProduccion ON tblProcesoModulo.Id_Area = tblProcesoProduccion.Id_Area 
            WHERE tblProcesoModulo.ID_Familia = @ID_Familia";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Familia", idFamilia);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    try
                    {
                        connection.Open();
                        adapter.Fill(dt);

                        // Asume que tienes otro DataGrid para mostrar los procesos
                        DatagridFamiliaModuloPorcesoPro.DataSource = dt;
                        DatagridFamiliaModuloPorcesoPro.DataBind();
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Response.Write("Error: " + ex.Message);
                    }
                }

                // Guardar el índice de la fila seleccionada
                DataGrid3.Attributes["SelectedRowIndexFamilia"] = rowIndex.ToString();

                // Desplazarse a la fila seleccionada
                row.Attributes["id"] = "IDDatagridFamilia_row_" + rowIndex;
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow",
                    $"focusAndScrollToRow('IDDatagridFamilia_row_{rowIndex}');", true);
            }
        }


        protected void DatagridProcesoProductivo_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "ProcesoProductivo")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DatagridProcesoProductivo.Items[rowIndex];

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DatagridProcesoProductivo.Items)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
                row.CssClass = "fila-seleccionada1";


                // Guardar el índice de la fila seleccionada
                DataGrid3.Attributes["SelectedRowIndexProPro"] = rowIndex.ToString();

                // Desplazarse a la fila seleccionada
                row.Attributes["id"] = "DatagridProcesoProductivo_row_" + rowIndex;
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow",
                    $"focusAndScrollToRow('DatagridProcesoProductivo_row_{rowIndex}');", true);
            }
        }

        protected void BtnNuevoFamilia_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            Session["CRUDFAMILIAMODULO"] = "Nuevo";
        }

        protected void BtnModificarFamilia_Click(object sender, EventArgs e)
        {
            habilitarCampos();

            Session["CRUDFAMILIAMODULO"] = "Modificar";
        }

        protected void habilitarCampos()
        {
            BtnNuevoFamilia.Enabled = false;
            BtnNuevoFamilia.CssClass = "btn btn-sm shadow button-disabled";

            BtnModificarFamilia.Enabled = false;
            BtnModificarFamilia.CssClass = "btn btn-sm shadow button-disabled";

            BtnGrabarFamilia.Enabled = true;
            BtnGrabarFamilia.CssClass = "btn btn-sm shadow button-enabled2 ColorAzulActivo";

            TextDescripcion.Enabled = true;
            TextDescripcion.CssClass = "form-control form-control-sm";
            TextDescripcion.Focus();

            TextDescripcionParaObjeto.Enabled = true;
            TextDescripcionParaObjeto.CssClass = "form-control form-control-sm";

            TextDescuentoFinal.Enabled = true;
            TextDescuentoFinal.CssClass = "form-control form-control-sm";
        }

        protected void BtnGrabarFamilia_Click(object sender, EventArgs e)
        { 
            if (string.IsNullOrWhiteSpace(TextDescripcion.Text) ||
                string.IsNullOrWhiteSpace(TextDescuentoFinal.Text) ||
                string.IsNullOrWhiteSpace(TextDescripcionParaObjeto.Text))
            {
              
                Response.Write("<script>alert('Por favor, complete todos los campos requeridos.');</script>");
                return; 
            }
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string crudAction = Session["CRUDFAMILIAMODULO"]?.ToString(); // Obtiene la acción (Nuevo o Modificar) desde la sesión

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    if (crudAction == "Nuevo")
                    {
                        // Obtener el nuevo ID_Familia
                        command.CommandText = "SELECT MAX(ID_Familia) FROM tblFamiliaModulo";
                        int nuevoIdFamilia = Convert.ToInt32(command.ExecuteScalar()) + 1;

                        // Insertar nueva familia
                        command.CommandText = @"
                INSERT INTO tblFamiliaModulo 
                (ID_Familia, Descripcion_Familia, Descuento, Descripcion_paraObj, FechaModificacion, Responsable)
                VALUES 
                (@ID_Familia, @Descripcion, @Descuento, @DescripcionParaObj, @FechaModificacion, @Responsable)";
                        command.Parameters.AddWithValue("@ID_Familia", nuevoIdFamilia);
                        command.Parameters.AddWithValue("@Descripcion", TextDescripcion.Text);
                        command.Parameters.AddWithValue("@Descuento", TextDescuentoFinal.Text);
                        command.Parameters.AddWithValue("@DescripcionParaObj", TextDescripcionParaObjeto.Text);
                        command.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                        command.Parameters.AddWithValue("@Responsable", Session["usuariologueado"].ToString());

                        command.ExecuteNonQuery();

                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Los registros se insertaron correctamente');", true);

                        // Actualizar el combo con la nueva descripción (simulado)
                        // dtacboFamiliaModulo.Text = TextDescripcion.Text; // Esto sería el control de combo en la interfaz
                    }
                    else if (crudAction == "Modificar")
                    {
                        // Asegúrate de tener el ID de la familia a modificar almacenado en el ViewState
                        if (Session["ID_Familia"] != null)
                        {
                            int idFamilia = Convert.ToInt32(Session["ID_Familia"]);

                            // Actualizar la familia existente
                            command.CommandText = @"
                    UPDATE tblFamiliaModulo 
                    SET Descripcion_Familia = @Descripcion, 
                        Descuento = @Descuento, 
                        Descripcion_paraObj = @DescripcionParaObj, 
                        FechaModificacion = @FechaModificacion, 
                        Responsable = @Responsable
                    WHERE ID_Familia = @ID_Familia";

                            command.Parameters.AddWithValue("@ID_Familia", idFamilia);
                            command.Parameters.AddWithValue("@Descripcion", TextDescripcion.Text);
                            command.Parameters.AddWithValue("@Descuento", TextDescuentoFinal.Text);
                            command.Parameters.AddWithValue("@DescripcionParaObj", TextDescripcionParaObjeto.Text);
                            command.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                            command.Parameters.AddWithValue("@Responsable", Session["usuariologueado"].ToString());

                            command.ExecuteNonQuery();

                            // Actualizar la descripción técnica de los objetos
                            command.CommandText = @"
                    UPDATE tblPanel 
                    SET Descripcion_Tecnica = dbo.fn_DescripcionObjeto(tblGrupoObjeto.goSPDescripcionObjto, tblPanel.Id_Numerico, tblGrupoObjeto.goDescripcionBaseObjeto)
                    FROM tblPanel
                    INNER JOIN tblGrupoObjeto ON tblPanel.Id_GrupoObjeto = tblGrupoObjeto.ID_GrupoObjeto
                    INNER JOIN tblPanel_Modulo ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum
                    INNER JOIN tblModulo ON tblPanel_Modulo.Id_Modulo = tblModulo.Id_Modulo
                    INNER JOIN tblFamiliaModulo ON tblModulo.ID_Familia = tblFamiliaModulo.ID_Familia
                    WHERE tblGrupoObjeto.goDescripcionBaseObjeto IS NOT NULL 
                        AND tblGrupoObjeto.goSPDescripcionObjto IS NOT NULL 
                        AND tblFamiliaModulo.ID_Familia = @ID_Familia";

                            command.ExecuteNonQuery();

                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Los registros se Actualizaron correctamente');", true);
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();

                    llenarDatagridFamiliaFiltro();

                    // Deshabilitar los campos después de guardar
                    TextDescripcion.Enabled = false;
                    TextDescuentoFinal.Enabled = false;
                    TextDescripcionParaObjeto.Enabled = false;

                    // Habilitar los botones
                    BtnNuevoFamilia.Enabled = true;
                    BtnNuevoFamilia.CssClass = "btn btn-sm shadow button-enabled2 AzulClaro";

                    BtnModificarFamilia.Enabled = true;
                    BtnModificarFamilia.CssClass = "btn btn-sm shadow button-enabled2 Cafe";

                    BtnGrabarFamilia.Enabled = false;
                    BtnGrabarFamilia.CssClass = "btn btn-sm shadow button-disabled";
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if something failed
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Ocurrio un error intentalo nuevamente por favor');", true);
                }
            }

            // Limpia la acción después de grabar para evitar repeticiones innecesarias
            Session["CRUDFAMILIAMODULO"] = null;
        }

        protected void TextBuscarFamiliaModulo_TextChanged(object sender, EventArgs e)
        {
            llenarDatagridFamiliaFiltro();
        }
    }
}