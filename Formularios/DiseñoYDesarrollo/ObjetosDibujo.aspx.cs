using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo
{
    public partial class ObjetosDibujo : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                LoadGrupoObjeto();
                LoadLinea();
                dropdivisiones();
                LoadData();
                DisposicionInicialBotones();
                EstadoGrabarCancelarInicial();
                DataGridInicial();

                // Inicializar DataTable y almacenarlo en ViewState
                DataTable dt = new DataTable();
                dt.Columns.Add("AnchoTag");
                dt.Columns.Add("AlturaTag");
                dt.Columns.Add("IdPanelTag");

                DataRow dr = dt.NewRow();
                dr["AnchoTag"] = TextAncho.Text.Trim();
                dr["AlturaTag"] = TextAltura.Text.Trim();
                dr["IdPanelTag"] = TextObjeto.Text.Trim();
                dt.Rows.Add(dr);

                ViewState["PanelTags"] = dt;
            }


        }

        protected void UpdateDataTable()
        {
            DataTable dt = ViewState["PanelTags"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                dr["AnchoTag"] = TextAncho.Text.Trim();
                dr["AlturaTag"] = TextAltura.Text.Trim();
                dr["IdPanelTag"] = TextObjeto.Text.Trim();
            }
        }


        protected void EstadoGrabarCancelarInicial()
        {
            var controles = new (System.Web.UI.Control control, string tag)[]
        {
            (CheckEstable, CheckEstable.Checked.ToString()),
            (CheckActivo, CheckActivo.Checked.ToString()),
            (DropDesGrupo, DropDesGrupo.SelectedItem.Text),
            (TextDesInt, TextDesInt.Text.Trim()),
            (DropLinea, DropLinea.SelectedItem.Text),
            (DropDivisiones, DropDivisiones.SelectedItem.Text),
            (TextAncho, TextAncho.Text),
            (TextProfundidad, TextProfundidad.Text),
            (TextObjeto, TextObjeto.Text),
            (TextHolgura, TextHolgura.Text.Trim()),
            (TextCubicaje, TextCubicaje.Text.Trim()),
            (TextAreaDescripTec, TextAreaDescripTec.Value.Trim()),
            (TextUndXPaq, TextUndXPaq.Text.Trim()),
            (TextIndReferencia, TextIndReferencia.Text.Trim()),
            (TextIdInsumo, TextIdInsumo.Text),
            (CheckApliCodPSLOT, CheckApliCodPSLOT.Checked.ToString())
        };

            foreach (var (control, tag) in controles)
            {
                switch (control)
                {
                    case System.Web.UI.WebControls.TextBox textBox:
                        textBox.Attributes["Tag"] = tag;
                        break;
                    case System.Web.UI.WebControls.DropDownList dropDownList:
                        dropDownList.Attributes["Tag"] = tag;
                        break;
                    case System.Web.UI.WebControls.CheckBox checkBox:
                        checkBox.Attributes["Tag"] = tag;
                        break;
                    case HtmlTextArea htmlTextArea:
                        htmlTextArea.Attributes["Tag"] = tag;
                        break;
                        // Agrega más casos según sea necesario para otros tipos de controles.
                }
            }

            // Llamar al método para evaluar el estado de los botones
            EstadoGrabarCancelar();

        }

        private void EstadoGrabarCancelar()
        {


            // Lista de controles y sus atributos "Tag"
            var controles = new (System.Web.UI.Control control, string tag, Func<string, bool> isChanged, Func<string, bool> isValid)[]
            {
        (CheckEstable, CheckEstable.Attributes["Tag"], val => CheckEstable.Checked != Convert.ToBoolean(val), val => true),
        (CheckActivo, CheckActivo.Attributes["Tag"], val => CheckActivo.Checked != Convert.ToBoolean(val), val => true),
        (DropDesGrupo, DropDesGrupo.Attributes["Tag"], val => DropDesGrupo.SelectedItem.Text != val, val => !string.IsNullOrEmpty(val)),
        (TextDesInt, TextDesInt.Attributes["Tag"], val => TextDesInt.Text.Trim() != val, val => !string.IsNullOrEmpty(val)),
        (DropLinea, DropLinea.Attributes["Tag"], val => DropLinea.SelectedItem.Text != val, val => !string.IsNullOrEmpty(val)),
        (DropDivisiones, DropDivisiones.Attributes["Tag"], val => DropDivisiones.SelectedItem.Text != val, val => !string.IsNullOrEmpty(val)),
        (TextAncho, TextAncho.Attributes["Tag"], val => TextAncho.Text != val, val => IsNumeric(val)),
        (TextProfundidad, TextProfundidad.Attributes["Tag"], val => TextProfundidad.Text != val, val => IsNumeric(val)),
        (TextObjeto, TextObjeto.Attributes["Tag"], val => TextObjeto.Text != val, val => !string.IsNullOrEmpty(val)),
        (TextHolgura, TextHolgura.Attributes["Tag"], val => TextHolgura.Text.Trim() != val, val => IsNumeric(val)),
        (TextCubicaje, TextCubicaje.Attributes["Tag"], val => TextCubicaje.Text.Trim() != val, val => IsNumeric(val)),
        (TextAreaDescripTec, TextAreaDescripTec.Attributes["Tag"], val => TextAreaDescripTec.Value.Trim() != val, val => true),
        (TextUndXPaq, TextUndXPaq.Attributes["Tag"], val => TextUndXPaq.Text.Trim() != val, val => IsNumeric(val)),
        (TextIndReferencia, TextIndReferencia.Attributes["Tag"], val => TextIndReferencia.Text.Trim() != val, val => !string.IsNullOrEmpty(val)),
        (TextIdInsumo, TextIdInsumo.Attributes["Tag"], val => TextIdInsumo.Text != val, val => true),
        (CheckApliCodPSLOT, CheckApliCodPSLOT.Attributes["Tag"], val => CheckApliCodPSLOT.Checked != Convert.ToBoolean(val), val => true)
            };

            bool valoresCambiados = controles.Any(c => c.isChanged(c.tag));
            bool camposRequeridosLlenos = controles.All(c => c.isValid(c.control is System.Web.UI.WebControls.TextBox ? (c.control as System.Web.UI.WebControls.TextBox).Text : c.control is DropDownList ? (c.control as DropDownList).SelectedItem.Text : c.control is HtmlTextArea ? (c.control as HtmlTextArea).Value : ""));

            if (valoresCambiados && camposRequeridosLlenos)
            {
                BtnCancelarObjetosPanel.Enabled = true;
                BtnGrabarObjetosPanel.Enabled = true;
            }
            else
            {
                BtnCancelarObjetosPanel.Enabled = false;
                BtnGrabarObjetosPanel.Enabled = false;
                if (camposRequeridosLlenos)
                {

                }
            }
        }

        // Método para verificar si una cadena es numérica
        private bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }

        // Evento genérico para detectar cambios en los controles
        protected void Control_ValueChanged(object sender, EventArgs e)
        {
            EstadoGrabarCancelar();
        }

        protected void DisposicionInicialBotones()
        {
            TextValorComercial.Enabled = false;
            TextValorComercial.CssClass = "form-control form-control-sm text-center";

            TextIdNum.Enabled = false;
            TextIdNum.CssClass = "form-control form-control-sm text-center";

            TextPeso.Enabled = false;
            TextPeso.CssClass = "form-control form-control-sm text-center";

            TextDescripcionPanelCotizacion.Enabled = false;
            TextDescripcionPanelCotizacion.CssClass = "form-control form-control-sm";

            TextCodPSL.Enabled = false;
            TextCodPSL.CssClass = "form-control form-control-sm text-center";

            TextInRelOtNoOai.Enabled = false;
            TextInRelOtNoOai.CssClass = "form-control form-control-sm";

            CheckChequeado.Enabled = false;
            CheckChequeado.CssClass = "form-control-sm";

            BtnGrabarObjetosPanel.Enabled = false;
            BtnGrabarObjetosPanel.CssClass = "form-control";

            BtnCancelarObjetosPanel.Enabled = false;
            BtnCancelarObjetosPanel.CssClass = "form-control";

            BtnCerrarObjetosPanel.Enabled = true;
            BtnCerrarObjetosPanel.CssClass = "form-control";

        }

        protected void dropdivisiones()
        {
            // Lógica inicial de la página
            DropDivisiones.Items.Add(new ListItem("", "")); // Agregar un elemento vacío al principio
            for (int i = 2; i <= 20; i++)
            {
                DropDivisiones.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void LoadGrupoObjeto()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM tblGrupoObjeto ORDER BY Descripcion_Grupo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DropDesGrupo.DataSource = reader;
                        DropDesGrupo.DataTextField = "Descripcion_Grupo";
                        DropDesGrupo.DataValueField = "ID_GrupoObjeto"; 
                        DropDesGrupo.DataBind();
                    }
                }
            }
            DropDesGrupo.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }

        private void LoadLinea()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM tblLinea";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DropLinea.DataSource = reader;
                        DropLinea.DataTextField = "Descripcion_Linea";
                        DropLinea.DataValueField = "Id_Linea";
                        DropLinea.DataBind();
                    }
                }
            }
            DropLinea.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }

        private void LoadData()
        {
            string idNumericoDise = Session["Id_numericoDise"] as string;

            if (!string.IsNullOrEmpty(idNumericoDise))
            {
                // Obtener Id_Panel
                string idPanel = GetIdPanelByIdNumerico(idNumericoDise);

                if (!string.IsNullOrEmpty(idPanel))
                {
                    // Obtener el ancho usando el Id_Panel
                    string ancho = GetAnchoByIdNumerico(idNumericoDise);

                    string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = @"
                SELECT tblPanel.*, tblLinea.Descripcion_Linea, tblPanel.Ancho, tblGrupoObjeto.Descripcion_Grupo, tblPanel.Id_Panel 
                FROM tblLinea 
                INNER JOIN (tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto) 
                ON tblLinea.Id_Linea = tblPanel.Id_Linea 
                WHERE tblPanel.Id_Panel = @Id_Panel AND tblPanel.Ancho = @Ancho 
                ORDER BY tblGrupoObjeto.Descripcion_Grupo, tblPanel.Descripcion_Panel, tblPanel.Id_Panel, tblPanel.Ancho";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id_Panel", idPanel);
                            command.Parameters.AddWithValue("@Ancho", ancho);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    TextObjeto.Text = reader["Id_Panel"].ToString();
                                    DropLinea.SelectedItem.Text = reader["Descripcion_Linea"].ToString();
                                    DropDesGrupo.SelectedItem.Text = reader["Descripcion_Grupo"].ToString();
                                    TextAncho.Text = reader["Ancho"].ToString();
                                    TextProfundidad.Text = reader["Profundidad"].ToString();
                                    TextAltura.Text = reader["Altura"].ToString();
                                    TextCubicaje.Text = reader["CubicajeM3"].ToString();
                                    DropDivisiones.SelectedItem.Text = reader["Divisiones"].ToString();
                                    TextHolgura.Text = reader["Holgura"].ToString();
                                    TextUndXPaq.Text = reader["UndxPaquete"].ToString();
                                    TextIdNum.Text = reader["Id_Numerico"].ToString();
                                    TextPeso.Text = reader["PesoKG"].ToString();
                                    TextIndReferencia.Text = reader["idInsumoReferencia"].ToString();
                                    TextDesInt.Text = reader["Descripcion_Panel"].ToString();
                                    TextAreaDescripTec.Value = reader["Descripcion_Tecnica"].ToString();
                                    TextValorComercial.Text = reader["Precio_Venta"].ToString();
                                    string descripcionPanelCotizacion = reader["Descripcion_Panel"].ToString();
                                    TextDescripcionPanelCotizacion.Text = ReplaceNomenclatura(descripcionPanelCotizacion);
                                    TextIdInsumo.Text = reader["Id_Insumo"].ToString();
                                    TextCodPSL.Text = reader["ID_Inventario"].ToString();
                                    TextInRelOtNoOai.Text = reader["Descripcion_Insumo"].ToString();
                                    bool isActive = reader["Activo"] != DBNull.Value && Convert.ToBoolean(reader["Activo"]);
                                    CheckActivo.Checked = isActive;
                                    bool Escalable = reader["Escalable"] != DBNull.Value && Convert.ToBoolean(reader["Escalable"]);
                                    CheckEstable.Checked = Escalable;
                                    bool Chequeado = reader["Chequeado"] != DBNull.Value && Convert.ToBoolean(reader["Chequeado"]);
                                    CheckChequeado.Checked = Chequeado;
                                    bool Apunta_Cod_PSL = reader["Apunta_Cod_PSL"] != DBNull.Value && Convert.ToBoolean(reader["Apunta_Cod_PSL"]);
                                    CheckApliCodPSLOT.Checked = Apunta_Cod_PSL;

                                    // Llenar el DataGrid
                                    LoadDataGridPanel();

                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoadDataGridPanel()
        {
            string idPanelNum = TextIdNum.Text; // Obtener el valor del TextBox 'TextIdNum'

            if (!string.IsNullOrEmpty(idPanelNum))
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
            SELECT ROW_NUMBER() OVER(ORDER BY Ubicacion_Modulo) AS Num_Fila, 
                   tblModulo.*, 
                   tblPanel_Modulo.Cantidad,
                   tblPanel_Modulo.Ubicacion_Modulo, 
                   tblPanel_Modulo.Lado, 
                   tblPanel_Modulo.Observaciones, 
                   tblPanel_Modulo.Id_PanelNum,
                   tblPanel_Modulo.PanModResponsable, 
                   tblFamiliaModulo.Descripcion_Familia, 
                   tblTipoModulo.Descripcion_TipoModulo 
            FROM tblTipoModulo 
            INNER JOIN ((tblFamiliaModulo 
            INNER JOIN tblModulo ON tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia) 
            INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo) 
            ON tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo 
            WHERE tblPanel_Modulo.Id_PanelNum = @Id_PanelNum 
            ORDER BY tblPanel_Modulo.Ubicacion_Modulo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id_PanelNum", idPanelNum);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridPanel.DataSource = dt;
                            DataGridPanel.DataBind();
                            UpdatePanel1.Update();
                        }
                    }
                }
            }
        }

        private string GetAnchoByIdNumerico(string idNumerico)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            string ancho = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Ancho FROM tblPanel WHERE Id_Numerico = @Id_Numerico";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Numerico", idNumerico);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        ancho = result.ToString();
                    }
                }
            }

            return ancho;
        }

        private string GetIdPanelByIdNumerico(string idNumerico)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString;
            string idPanel = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id_Panel FROM tblPanel WHERE Id_Numerico = @Id_Numerico";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Numerico", idNumerico);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        idPanel = result.ToString();
                    }
                }
            }

            return idPanel;
        }

        private string ReplaceNomenclatura(string descripcionPanel)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Nomenclatura, DescripcionNomenclatura FROM tblNomenclatura";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nomenclatura = reader["Nomenclatura"].ToString();
                            string descripcionNomenclatura = reader["DescripcionNomenclatura"].ToString();

                            if (descripcionPanel.Contains(nomenclatura))
                            {
                                descripcionPanel = descripcionPanel.Replace(nomenclatura, descripcionNomenclatura);
                            }
                        }
                    }
                }
            }

            return descripcionPanel;
        }

        protected void DataGridInicial()
        {
            string GrupoElementos = ""; // Aquí debes obtener el valor de GrupoElementos según tu lógica
            string criterio = TextCriterio.Text.Trim();
            string alturaMod = TextAlturaConfigurar.Text.Trim();
            string familiaModulo = ""; // Aquí debes obtener el valor de DtaCboFamiliaModulo.Text según tu lógica

            string sSql = "";
            if (CheckBase.Checked)
            {
                // Consulta para optBases = True
                sSql = "SELECT TOP 200 tblModulo.*, tblTipoModulo.Id_TipoModulo, tblModulo.Descripcion_Modulo, " +
                       "tblFamiliaModulo.Descripcion_Familia, tblTipoModulo.Descripcion_TipoModulo " +
                       "FROM tblTipoModulo INNER JOIN (tblFamiliaModulo INNER JOIN tblModulo ON " +
                       "tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia) ON " +
                       "tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                       "WHERE (((tblTipoModulo.Id_TipoModulo)=1) " +
                       "AND ((tblModulo.Descripcion_Modulo) Like @Criterio " +
                       "AND (tblModulo.Descripcion_Modulo) Like @GrupoElementos) " +
                       "AND ((tblModulo.Altura) Like @AlturaModulo) " +
                       "AND (tblFamiliaModulo.Descripcion_Familia like @FamiliaModulo)) " +
                       "ORDER BY tblModulo.Descripcion_Modulo, tblModulo.Altura";
            }
            else
            {
                // Consulta para optBases = False
                sSql = "SELECT TOP 200 tblModulo.*, tblTipoModulo.Id_TipoModulo, tblModulo.Descripcion_Modulo, " +
                       "tblFamiliaModulo.Descripcion_Familia, tblTipoModulo.Descripcion_TipoModulo " +
                       "FROM tblTipoModulo INNER JOIN (tblFamiliaModulo INNER JOIN tblModulo ON " +
                       "tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia) ON " +
                       "tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                       "WHERE (((tblTipoModulo.Id_TipoModulo)=2 or (tblTipoModulo.Id_TipoModulo)=3) " +
                       "AND ((tblModulo.Descripcion_Modulo) Like @Criterio " +
                       "AND (tblModulo.Descripcion_Modulo) Like @GrupoElementos) " +
                       "AND ((tblModulo.Altura) Like @AlturaModulo) " +
                       "AND (tblFamiliaModulo.Descripcion_Familia like @FamiliaModulo)) " +
                       "ORDER BY tblModulo.Descripcion_Modulo, tblModulo.Altura";
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BD_SIDSQL"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, con))
                {
                    cmd.Parameters.AddWithValue("@Criterio", "%" + criterio + "%");
                    cmd.Parameters.AddWithValue("@GrupoElementos", "%" + GrupoElementos + "%");
                    cmd.Parameters.AddWithValue("@AlturaModulo", alturaMod + "%");
                    cmd.Parameters.AddWithValue("@FamiliaModulo", familiaModulo + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataGridConfigurar.DataSource = dt;
                    DataGridConfigurar.DataBind();
                }
            }
        }

        private void BindDataGrid()
        {
            string consultaBase = "";

            if (CheckBase.Checked)
            {
                // Consulta para optBases = True
                consultaBase = "SELECT TOP 200 tblModulo.*, tblTipoModulo.Id_TipoModulo, tblModulo.Descripcion_Modulo, " +
                       "tblFamiliaModulo.Descripcion_Familia, tblTipoModulo.Descripcion_TipoModulo " +
                       "FROM tblTipoModulo INNER JOIN (tblFamiliaModulo INNER JOIN tblModulo ON " +
                       "tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia) ON " +
                       "tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                       "WHERE (((tblTipoModulo.Id_TipoModulo)=1)";
            }
            else
            {
                // Consulta para optBases = False
                consultaBase = "SELECT TOP 200 tblModulo.*, tblTipoModulo.Id_TipoModulo, tblModulo.Descripcion_Modulo, " +
                      "tblFamiliaModulo.Descripcion_Familia, tblTipoModulo.Descripcion_TipoModulo " +
                      "FROM tblTipoModulo INNER JOIN (tblFamiliaModulo INNER JOIN tblModulo ON " +
                      "tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia) ON " +
                      "tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                      "WHERE (((tblTipoModulo.Id_TipoModulo)=2 or (tblTipoModulo.Id_TipoModulo)=3))";
            }

            // Inicializa la cláusula WHERE
            string whereClause = "";

            // Agrega la condición de texto de búsqueda si se proporciona
            if (!string.IsNullOrEmpty(TextCriterio.Text))
            {
                whereClause += " AND tblModulo.Descripcion_Modulo LIKE '%" + TextCriterio.Text + "%'";
            }
            if (!string.IsNullOrEmpty(TextAlturaConfigurar.Text))
            {
                whereClause += " AND tblModulo.Altura LIKE '%" + TextAlturaConfigurar.Text + "%'";
            }

            // Combina la consulta base con la cláusula WHERE
            consultaBase += whereClause;


            // Conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(consultaBase, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();


                connection.Open();
                adapter.Fill(dataTable);

                // Asigna los datos al DataGrid
                DataGridConfigurar.DataSource = dataTable;
                DataGridConfigurar.DataBind();

            }
        }

        protected void ButtonBuscar_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }

        protected void DataGridConfigurar_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Id_Modulo")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridConfigurar.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 
                foreach (DataGridItem item in DataGridConfigurar.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";


            }
        }

        protected void GrabarObjetosDibujo_Click(object sender, EventArgs e)
        {
            bool CheckApliCodPSLOTT = CheckApliCodPSLOT.Checked;
            string nombreEmpleado = Session["usuariologueado"].ToString();
            string CedulaLogeada = Session["CedulaLogeada"].ToString();

            string TextObjetoo = TextObjeto.Text;

            string tipoAccion = Session["CrudObjetosDibujo"] as string;
            if (tipoAccion == "Nuevo")
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                string sSql = "";
                if (!CheckApliCodPSLOTT)  // Si CheckApliCodPSLOT está desmarcado
                {
                    sSql = "INSERT INTO tblPanel(Id_Panel, Descripcion_Panel, Id_GrupoObjeto, Ancho, Altura, Id_Linea, Divisiones, HOLGURA, Profundidad, Escalable, cubicajem3, Activo, chequeado, Responsable, FechaChequeo, Descripcion_Tecnica, idInsumoReferencia, UndxPaquete, Apunta_Cod_PSL, ID_Inventario, Id_Insumo, Descripcion_Insumo) " +
                           "VALUES(@Id_Panel, @Descripcion_Panel, @Id_GrupoObjeto, @Ancho, 0, @Id_Linea, @Divisiones, @HOLGURA, @Profundidad, @Escalable, @cubicajem3, @Activo, 0, @Responsable, @FechaChequeo, @Descripcion_Tecnica, @idInsumoReferencia, @UndxPaquete, 0, NULL, NULL, NULL)";
                }
                else  // Si CheckApliCodPSLOT está marcado
                {
                    sSql = "INSERT INTO tblPanel(Id_Panel, Descripcion_Panel, Id_GrupoObjeto, Ancho, Altura, Id_Linea, Divisiones, HOLGURA, Profundidad, Escalable, cubicajem3, Activo, chequeado, Responsable, FechaChequeo, Descripcion_Tecnica, idInsumoReferencia, UndxPaquete, Apunta_Cod_PSL, ID_Inventario, Id_Insumo, Descripcion_Insumo) " +
                           "VALUES(@Id_Panel, @Descripcion_Panel, @Id_GrupoObjeto, @Ancho, 0, @Id_Linea, @Divisiones, @HOLGURA, @Profundidad, @Escalable, @cubicajem3, @Activo, 0, @Responsable, @FechaChequeo, @Descripcion_Tecnica, @idInsumoReferencia, @UndxPaquete, 1, @CodPSL, @IdInsumo, @Descripcion_Insumo)";
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sSql, con))
                    {
                        cmd.Parameters.AddWithValue("@Id_Panel", TextObjetoo.Trim());
                        cmd.Parameters.AddWithValue("@Descripcion_Panel", TextDesInt.Text);
                        cmd.Parameters.AddWithValue("@Id_GrupoObjeto", DropDesGrupo.DataValueField);
                        cmd.Parameters.AddWithValue("@Ancho", TextAncho.Text);
                        cmd.Parameters.AddWithValue("@Id_Linea", DropLinea.SelectedValue);
                        cmd.Parameters.AddWithValue("@Divisiones", DropDivisiones.Text);
                        cmd.Parameters.AddWithValue("@HOLGURA", DropDivisiones.Text);
                        cmd.Parameters.AddWithValue("@Profundidad", TextProfundidad.Text);
                        cmd.Parameters.AddWithValue("@Escalable", CheckEstable.Checked);
                        cmd.Parameters.AddWithValue("@cubicajem3", TextCubicaje.Text);
                        cmd.Parameters.AddWithValue("@Activo", CheckActivo.Checked);
                        cmd.Parameters.AddWithValue("@Responsable", nombreEmpleado);
                        cmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                        cmd.Parameters.AddWithValue("@Descripcion_Tecnica", TextAreaDescripTec.Value);
                        cmd.Parameters.AddWithValue("@idInsumoReferencia", TextIndReferencia.Text);
                        cmd.Parameters.AddWithValue("@UndxPaquete", TextUndXPaq);

                        if (CheckApliCodPSLOTT)
                        {
                            cmd.Parameters.AddWithValue("@CodPSL", TextCodPSL.Text);
                            cmd.Parameters.AddWithValue("@IdInsumo", TextIdInsumo.Text);
                            cmd.Parameters.AddWithValue("@Descripcion_Insumo", TextInRelOtNoOai.Text);
                        }

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                string contenidoModalOT = "El Usuario Con cédula: " + CedulaLogeada + " Ingresa el Objeto " + TextObjetoo + "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ModalObjetoIngresadoUsuario').modal('show'); $('#ModalObjetoIngresadoUsuario2').text('" + contenidoModalOT + "');", true);

            }
            else if (tipoAccion == "Modificar")
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

                DataTable dt = ViewState["PanelTags"] as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    // Manejar el caso en que el DataTable no esté inicializado o esté vacío
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error: DataTable no está inicializado.');", true);
                    return;
                }

                DataRow dr = dt.Rows[0];

                decimal anchoAnterior = Convert.ToDecimal(dr["AnchoTag"]);
                decimal alturaAnterior = Convert.ToDecimal(dr["AlturaTag"]);

             
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();

                        // Check if Id_Panel has changed
                        if (TextObjeto.Text.Trim() != dr["IdPanelTag"].ToString())
                        {
                            string checkPanelSql = "SELECT * FROM tblPanel WHERE Id_Panel = @Id_Panel";
                            using (SqlCommand checkCmd = new SqlCommand(checkPanelSql, con))
                            {
                                checkCmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text.Trim());
                                using (SqlDataReader reader = checkCmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('La familia del Objeto " + TextObjeto.Text.Trim() + " ya existe');", true);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (TextAncho.Text.Trim() != dr["AnchoTag"].ToString())
                            {
                                string checkPanelAnchoSql = "SELECT * FROM tblPanel WHERE Id_Panel = @Id_Panel AND Ancho = @Ancho AND Altura = @Altura";
                                using (SqlCommand checkAnchoCmd = new SqlCommand(checkPanelAnchoSql, con))
                                {
                                    checkAnchoCmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text.Trim());
                                    checkAnchoCmd.Parameters.AddWithValue("@Ancho", TextAncho.Text.Trim());
                                    checkAnchoCmd.Parameters.AddWithValue("@Altura", TextAltura.Text.Trim());
                                    using (SqlDataReader reader = checkAnchoCmd.ExecuteReader())
                                    {
                                        if (reader.HasRows)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El Objeto " + TextObjeto.Text.Trim() + " con ancho " + TextAncho.Text.Trim() + " ya existe');", true);
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        // Update the panel
                        string updateSql = "";
                        if (!CheckApliCodPSLOTT) // If CheckApliCodPSLOT is unchecked
                        {
                            updateSql = "UPDATE tblPanel SET Id_Panel = @Id_Panel, UndxPaquete = @UndxPaquete, Descripcion_Panel = @Descripcion_Panel, " +
                                        "Id_GrupoObjeto = @Id_GrupoObjeto, Ancho = @Ancho, Altura = @Altura, Id_Linea = @Id_Linea, Divisiones = @Divisiones, " +
                                        "HOLGURA = @HOLGURA, Precio_Venta = 0, Profundidad = @Profundidad, Escalable = @Escalable, CubicajeM3 = @CubicajeM3, " +
                                        "Activo = @Activo, RegistradoSag = 0, chequeado = @chequeado, Responsable = @Responsable, FechaChequeo = @FechaChequeo, " +
                                        "Descripcion_Tecnica = @Descripcion_Tecnica, idInsumoReferencia = @idInsumoReferencia, Apunta_Cod_PSL = 0, " +
                                        "ID_Inventario = NULL, Id_Insumo = NULL, Descripcion_Insumo = NULL " +
                                        "WHERE Id_Panel = @Id_PanelTag AND Ancho = @AnchoAnterior AND Altura = @AlturaAnterior";
                        }
                        else // If CheckApliCodPSLOT is checked
                        {
                            updateSql = "UPDATE tblPanel SET Id_Panel = @Id_Panel, UndxPaquete = @UndxPaquete, Descripcion_Panel = @Descripcion_Panel, " +
                                        "Id_GrupoObjeto = @Id_GrupoObjeto, Ancho = @Ancho, Altura = @Altura, Id_Linea = @Id_Linea, Divisiones = @Divisiones, " +
                                        "HOLGURA = @HOLGURA, Precio_Venta = 0, Profundidad = @Profundidad, Escalable = @Escalable, CubicajeM3 = @CubicajeM3, " +
                                        "Activo = @Activo, RegistradoSag = 0, chequeado = @chequeado, Responsable = @Responsable, FechaChequeo = @FechaChequeo, " +
                                        "Descripcion_Tecnica = @Descripcion_Tecnica, idInsumoReferencia = @idInsumoReferencia, Apunta_Cod_PSL = 1, " +
                                        "ID_Inventario = @CodPSL, Id_Insumo = @IdInsumo, Descripcion_Insumo = @Descripcion_Insumo " +
                                        "WHERE Id_Panel = @Id_PanelTag AND Ancho = @AnchoAnterior AND Altura = @AlturaAnterior";
                        }

                        using (SqlCommand updateCmd = new SqlCommand(updateSql, con))
                        {
                            updateCmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@UndxPaquete", TextUndXPaq.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Descripcion_Panel", TextDesInt.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Id_GrupoObjeto", DropDesGrupo.SelectedValue);
                            updateCmd.Parameters.AddWithValue("@Ancho", TextAncho.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Altura", TextAltura.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Id_Linea", DropLinea.SelectedValue);
                            updateCmd.Parameters.AddWithValue("@Divisiones", DropDivisiones.Text);
                            updateCmd.Parameters.AddWithValue("@HOLGURA", TextHolgura.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Profundidad", TextProfundidad.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Escalable", CheckEstable.Checked);
                            updateCmd.Parameters.AddWithValue("@CubicajeM3", TextCubicaje.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Activo", CheckActivo.Checked);
                            updateCmd.Parameters.AddWithValue("@chequeado", CheckChequeado.Checked);
                            updateCmd.Parameters.AddWithValue("@Responsable", nombreEmpleado);
                            updateCmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
                            updateCmd.Parameters.AddWithValue("@Descripcion_Tecnica", TextAreaDescripTec.Value.Trim());
                            updateCmd.Parameters.AddWithValue("@idInsumoReferencia", TextIndReferencia.Text.Trim());
                            updateCmd.Parameters.AddWithValue("@Id_PanelTag", dr["IdPanelTag"]);
                            updateCmd.Parameters.AddWithValue("@AnchoAnterior", anchoAnterior);
                            updateCmd.Parameters.AddWithValue("@AlturaAnterior", alturaAnterior);

                            if (CheckApliCodPSLOTT)
                            {
                                updateCmd.Parameters.AddWithValue("@CodPSL", TextCodPSL.Text.Trim());
                                updateCmd.Parameters.AddWithValue("@IdInsumo", TextIdInsumo.Text.Trim());
                                updateCmd.Parameters.AddWithValue("@Descripcion_Insumo", TextInRelOtNoOai.Text.Trim());
                            }

                            updateCmd.ExecuteNonQuery();
                        }

                    string updateRelatedPanelsSql = @"
    UPDATE tblPanel SET 
        Id_Panel = @Id_Panel, 
        UndxPaquete = @UndxPaquete, 
        Descripcion_Panel = @Descripcion_Panel, 
        Id_GrupoObjeto = @Id_GrupoObjeto, 
        Id_Linea = @Id_Linea, 
        Divisiones = @Divisiones, 
        HOLGURA = @HOLGURA, 
        Precio_Venta = 0, 
        Profundidad = @Profundidad, 
        Escalable = @Escalable, 
        RegistradoSag = 0, 
        chequeado = @chequeado, 
        Responsable = @Responsable, 
        FechaChequeo = @FechaChequeo, 
        Descripcion_Tecnica = dbo.fn_DescripcionObjeto (@goSPDescripcionObjto, @Id_Numerico, @goDescripcionbASEObjeto)
    WHERE Id_Panel = @Id_PanelTag";

                    using (SqlCommand updateRelatedCmd = new SqlCommand(updateRelatedPanelsSql, con))
                    {
                        updateRelatedCmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@UndxPaquete", TextUndXPaq.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@Descripcion_Panel", TextDesInt.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@Id_GrupoObjeto", DropDesGrupo.SelectedValue);
                        updateRelatedCmd.Parameters.AddWithValue("@Id_Linea", DropLinea.SelectedValue);
                        updateRelatedCmd.Parameters.AddWithValue("@Divisiones", DropDivisiones.Text);
                        updateRelatedCmd.Parameters.AddWithValue("@HOLGURA", TextHolgura.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@Profundidad", TextProfundidad.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@Escalable", CheckEstable.Checked);
                        updateRelatedCmd.Parameters.AddWithValue("@chequeado", CheckChequeado.Checked);
                        updateRelatedCmd.Parameters.AddWithValue("@Responsable", nombreEmpleado);
                        updateRelatedCmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));

                        // Se obtiene el valor de @goSPDescripcionObjto, @Id_Numerico, y @goDescripcionbASEObjeto
                        string goSPDescripcionObjto = "";  // Reemplaza esto con el valor real
                        string goDescripcionbASEObjeto = "";  // Reemplaza esto con el valor real
                        string idNumerico = "";  // Reemplaza esto con el valor real

                        updateRelatedCmd.Parameters.AddWithValue("@goSPDescripcionObjto", goSPDescripcionObjto);
                        updateRelatedCmd.Parameters.AddWithValue("@Id_Numerico", idNumerico);
                        updateRelatedCmd.Parameters.AddWithValue("@goDescripcionbASEObjeto", goDescripcionbASEObjeto);
                        updateRelatedCmd.Parameters.AddWithValue("@Id_PanelTag", dr["IdPanelTag"]);

                        updateRelatedCmd.ExecuteNonQuery();
                    }

                }

              
               
            }


            else if (tipoAccion == "Copiar")
            {

            }

        }
    }
}