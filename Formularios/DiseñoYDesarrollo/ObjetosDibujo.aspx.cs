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
using DocumentFormat.OpenXml.Office2010.CustomUI;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Runtime.InteropServices;
using SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin;
using System.Web.Configuration;
using System.Security.Cryptography;
using Org.BouncyCastle.Utilities;
using System.Runtime.ConstrainedExecution;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Diseño_Venta;
using DocumentFormat.OpenXml.Office.Word;
using System;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo
{
    public partial class ObjetosDibujo : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (Session["usuariologueado"] != null)
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

                    btnActuaValComercial.Enabled = true;
                    btnActuaValComercial.CssClass = "btn btn-sm shadow ColorAzulActivo border";


                    btnEliminarInsumo.Enabled = false;
                    btnEliminarInsumo.CssClass = "btn btn-sm btn-outline-secondary";

                    btnAgregarInsumo.Enabled = false;
                    btnAgregarInsumo.CssClass = "btn btn-sm btn-outline-secondary";

                    // Manejar  el evento de Anadir un modulo a un Objeto y/o Eliminar 

                    if (Session["ControlTapConfigurar"]?.ToString() == "1")
                    {
                        string script = @"ActivarTapConfigurarControl();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ActivarTapConfigurarControl", script, true);

                        Session.Remove("ControlTapConfigurar");
                    }


                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }

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
            (TextAreaDescripTec, TextAreaDescripTec.Text.Trim()),
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
        (TextAreaDescripTec, TextAreaDescripTec.Attributes["Tag"], val => TextAreaDescripTec.Text.Trim() != val, val => true),
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

                BtnCancelarObjetosPanel.CssClass = "form-control ColorAzulActivo";
                BtnGrabarObjetosPanel.CssClass = "form-control ColorAzulActivo";

                string script = @"DesactivarTapConfigurar();";
                ScriptManager.RegisterStartupScript(this, GetType(), "DesactivarTapConfigurar", script, true);
            }
            else
            {
                BtnCancelarObjetosPanel.Enabled = false;
                BtnGrabarObjetosPanel.Enabled = false;

                BtnCancelarObjetosPanel.CssClass = "form-control ";
                BtnGrabarObjetosPanel.CssClass = "form-control ";

                if (TextObjeto.Text != "" && DropLinea.SelectedItem.Text != "" && DropDivisiones.SelectedItem.Text != "" && TextHolgura.Text != "" &&
                    TextObjeto.Text != "" && TextDesInt.Text != "" && DropDesGrupo.SelectedItem.Text != "")
                {
                    string script = @"ActivarTapConfigurar();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "DesactivarTapConfigurar", script, true);
                }
                else
                {
                    string script = @"DesactivarTapConfigurar();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "DesactivarTapConfigurar", script, true);
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

            if (Session["CrudObjetosDibujo"].ToString() == "Consultar")
            {
                TextObjeto.Enabled = false;
                TextObjeto.CssClass = "form-control form-control-sm";

                DropLinea.Enabled = false;
                DropLinea.CssClass = "form-control form-control-sm";

                DropDesGrupo.Enabled = false;
                DropDesGrupo.CssClass = "form-control form-control-sm";

                TextAncho.Enabled = false;
                TextAncho.CssClass = "form-control form-control-sm";

                TextProfundidad.Enabled = false;
                TextProfundidad.CssClass = "form-control form-control-sm";

                TextAltura.Enabled = false;
                TextAltura.CssClass = "form-control form-control-sm";

                TextCubicaje.Enabled = false;
                TextCubicaje.CssClass = "form-control form-control-sm";

                DropDivisiones.Enabled = false;
                DropDivisiones.CssClass = "form-control form-control-sm";

                TextHolgura.Enabled = false;
                TextHolgura.CssClass = "form-control form-control-sm";

                TextUndXPaq.Enabled = false;
                TextUndXPaq.CssClass = "form-control form-control-sm";

                TextIndReferencia.Enabled = false;
                TextIndReferencia.CssClass = "form-control form-control-sm";

                TextDesInt.Enabled = false;
                TextDesInt.CssClass = "form-control form-control-sm";

                TextAreaDescripTec.Enabled = false;
                TextAreaDescripTec.CssClass = "form-control form-control-sm";

                CheckApliCodPSLOT.Enabled = false;
                CheckApliCodPSLOT.CssClass = "form-control-sm ";

                CheckActivo.Enabled = false;
                CheckActivo.CssClass = "form-control-sm ";

                CheckEstable.Enabled = false;
                CheckEstable.CssClass = "form-control-sm ";


                TextIdInsumo.Enabled = false;
                TextIdInsumo.CssClass = "form-control form-control-sm";


            }

            else if (Session["CrudObjetosDibujo"].ToString() == "Nuevo")
            {
                CheckActivo.Checked = true;

                lbActivo.BackColor = System.Drawing.Color.LightGreen;
                lbActivo.ForeColor = System.Drawing.Color.Black;
                CheckActivo.BackColor = System.Drawing.Color.LightGreen;

                TextIdInsumo.Enabled = false;
                TextIdInsumo.CssClass = "form-control form-control-sm";


                string script = @"DesactivarTapConfigurar();";
                ScriptManager.RegisterStartupScript(this, GetType(), "DesactivarTapConfigurar", script, true);
            }

            else if (Session["CrudObjetosDibujo"].ToString() == "Modificar")
            {
                TextIdInsumo.Enabled = true;
                TextIdInsumo.CssClass = "form-control form-control-sm";
            }

            else if (Session["CrudObjetosDibujo"].ToString() == "Copiar")
            {
                TextIdInsumo.Enabled = true;
                TextIdInsumo.CssClass = "form-control form-control-sm";
            }

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
            BtnCerrarObjetosPanel.CssClass = "form-control ColorAzulActivo";

            Buscar.Enabled = true;
            Buscar.CssClass = "btn btn-sm shadow ColorAzulActivo";

            btnAdicionarModulo.Enabled = false;
            btnAdicionarModulo.CssClass = "btn btn-sm button-disabled shadow";

            btnCopiarModulo.Enabled = false;
            btnCopiarModulo.CssClass = "btn btn-sm button-disabled shadow";

            btnEliminarModuloObjeto.Enabled = false;
            btnEliminarModuloObjeto.CssClass = "btn btn-sm button-disabled shadow";

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
                                    DropLinea.SelectedValue = reader["Id_Linea"].ToString();
                                    DropDesGrupo.SelectedValue = reader["Id_GrupoObjeto"].ToString();
                                    TextAncho.Text = reader["Ancho"].ToString();
                                    TextProfundidad.Text = reader["Profundidad"].ToString();
                                    TextAltura.Text = reader["Altura"].ToString();
                                    TextCubicaje.Text = reader["CubicajeM3"].ToString();
                                    DropDivisiones.SelectedValue = reader["Divisiones"].ToString();
                                    TextHolgura.Text = reader["Holgura"].ToString();
                                    TextUndXPaq.Text = reader["UndxPaquete"].ToString();
                                    TextIdNum.Text = reader["Id_Numerico"].ToString();
                                    TextPeso.Text = reader["PesoKG"].ToString();
                                    TextIndReferencia.Text = reader["idInsumoReferencia"].ToString();
                                    TextDesInt.Text = reader["Descripcion_Panel"].ToString();
                                    TextAreaDescripTec.Text = reader["Descripcion_Tecnica"].ToString();
                                    TextValorComercial.Text = reader["Precio_Venta"].ToString();
                                    string descripcionPanelCotizacion = reader["Descripcion_Panel"].ToString();
                                    TextDescripcionPanelCotizacion.Text = ReplaceNomenclatura(descripcionPanelCotizacion);

                                    // Funcionalidad Agregar Insumos al objeto 
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


                                    if (isActive == true)
                                    {
                                        lbActivo.BackColor = System.Drawing.Color.LightGreen;
                                        lbActivo.ForeColor = System.Drawing.Color.Black;
                                        CheckActivo.BackColor = System.Drawing.Color.LightGreen;
                                    }
                                    else
                                    {
                                        lbActivo.BackColor = System.Drawing.ColorTranslator.FromHtml("#efb6b6");
                                        lbActivo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                                        CheckActivo.BackColor = System.Drawing.ColorTranslator.FromHtml("#efb6b6");

                                    }

                                    // Llenar el DataGrid
                                    LoadDataGridPanel();

                                    float disponibleA = 0;
                                    float disponibleB = 0;

                                    float holgura = 0;

                                    // Verificar si el valor en TextHolgura no es nulo o vacío y convertirlo a float
                                    if (!string.IsNullOrEmpty(TextHolgura.Text))
                                    {
                                        float.TryParse(TextHolgura.Text, out holgura);
                                    }

                                    // Verificamos si hay alturas disponibles 
                                    Alturas_Disponibles(TextIdNum.Text, ref disponibleA, ref disponibleB, holgura);

                                    lbDisLA.Text = "Dis.LA " + disponibleA + "cms";
                                    lbDisLB.Text = "Dis.LB " + disponibleB + "cms";

                                    // llenamos la Ubicacion 
                                    LlenarUbicacion(Convert.ToInt32(DropDivisiones.SelectedValue));

                                    if (CalcularUbicacionModulo(TextIdNum.Text) == 0)
                                    {
                                        CheckBase.Checked = true;
                                        CheckBase.Enabled = false;

                                        CheckComplementarios.Checked = false;


                                        ddlUbicacion.Enabled = false;
                                        ddlUbicacion.CssClass = "form-control form-control-sm";

                                        ddlCantidad.Enabled = false;
                                        ddlCantidad.CssClass = "form-control form-control-sm";

                                        ddlLado.Enabled = false;
                                        ddlLado.CssClass = "form-control form-control-sm";



                                    }
                                    else
                                    {
                                        CheckBase.Checked = false;
                                        CheckBase.Enabled = false;

                                        CheckComplementarios.Checked = true;

                                        ddlUbicacion.Enabled = true;
                                        ddlUbicacion.CssClass = "form-control form-control-sm";

                                        ddlCantidad.Enabled = true;
                                        ddlCantidad.CssClass = "form-control form-control-sm";

                                        ddlLado.Enabled = true;
                                        ddlLado.CssClass = "form-control form-control-sm";
                                    }

                                    BindDataGrid();


                                    if (CheckBase.Checked)
                                    {

                                    }
                                    else
                                    {

                                    }

                                    CargarModulosAsociadosAlObjeto(idNumericoDise);

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

        private void CargarModulosAsociadosAlObjeto(string idNumerico)
        {
            string idPanelNum = TextIdNum.Text; // Obtener el valor del TextBox 'TextIdNum'

            if (!string.IsNullOrEmpty(idPanelNum))
            {
                string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"           
                            SELECT 
                            tblModulo.*,
                            tblPanel_Modulo.Cantidad,
                            tblPanel_Modulo.Ubicacion_Modulo,
                            tblPanel_Modulo.Lado,
                            tblPanel_Modulo.Observaciones,
                            tblPanel_Modulo.Id_PanelNum,
                            tblPanel_Modulo.PanModResponsable,
                            tblFamiliaModulo.Descripcion_Familia 
                            FROM (tblFamiliaModulo 
                            INNER JOIN tblModulo ON tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia) 
                            INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo 
                            WHERE (((tblPanel_Modulo.Id_PanelNum)=@IDNumerico))ORDER BY tblPanel_Modulo.Ubicacion_Modulo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IDNumerico", idNumerico);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridModulosAsociados.DataSource = dt;
                            DataGridModulosAsociados.DataBind();

                        }
                    }
                }
            }
        }

        public void Alturas_Disponibles(string IdPanelNum, ref float DisponibleA, ref float DisponibleB, float Holgura)
        {
            float AlturaA = 0, AlturaB = 0, AlturaAB = 0;

            string sSql = "SELECT SUM(tblModulo.Altura) AS AlturaLado, tblPanel_Modulo.Lado " +
                          "FROM tblTipoModulo " +
                          "INNER JOIN (tblModulo INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo) " +
                          "ON tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                          "WHERE tblTipoModulo.Id_TipoModulo <> 1 " +
                          "GROUP BY tblPanel_Modulo.Lado, tblPanel_Modulo.Id_PanelNum " +
                          "HAVING tblPanel_Modulo.Id_PanelNum = @IdPanelNum";

            string sSqlMarco = "SELECT tblModulo.Altura, tblTipoModulo.Id_TipoModulo " +
                               "FROM tblTipoModulo " +
                               "INNER JOIN (tblModulo INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo) " +
                               "ON tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                               "WHERE tblPanel_Modulo.Id_PanelNum = @IdPanelNum " +
                               "AND tblTipoModulo.Id_TipoModulo = 1";

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Ejecuta la primera consulta
                using (SqlCommand cmdAlturasDisponibles = new SqlCommand(sSql, connection))
                {
                    cmdAlturasDisponibles.Parameters.AddWithValue("@IdPanelNum", IdPanelNum);

                    using (SqlDataReader readerAlturas = cmdAlturasDisponibles.ExecuteReader())
                    {
                        while (readerAlturas.Read())
                        {
                            string lado = readerAlturas["Lado"].ToString();
                            float alturaLado = Convert.ToSingle(readerAlturas["AlturaLado"]);

                            switch (lado)
                            {
                                case "A":
                                    AlturaA = alturaLado;
                                    break;
                                case "B":
                                    AlturaB = alturaLado;
                                    break;
                                case "AB":
                                    AlturaAB = alturaLado;
                                    break;
                            }
                        }
                    }
                }

                // Ejecuta la segunda consulta
                using (SqlCommand cmdAlturaMarco = new SqlCommand(sSqlMarco, connection))
                {
                    cmdAlturaMarco.Parameters.AddWithValue("@IdPanelNum", IdPanelNum);

                    using (SqlDataReader readerMarco = cmdAlturaMarco.ExecuteReader())
                    {
                        if (readerMarco.Read())
                        {
                            float alturaMarco = Convert.ToSingle(readerMarco["Altura"]);

                            // Calcula los valores disponibles para A y B
                            DisponibleA = (float)Math.Round(alturaMarco - (AlturaA + AlturaAB) - Holgura, 2);
                            DisponibleB = (float)Math.Round(alturaMarco - (AlturaB + AlturaAB) - Holgura, 2);
                        }
                        else
                        {
                            DisponibleA = 0;
                            DisponibleB = 0;
                        }
                    }
                }
            }
        }

        private void LlenarUbicacion(int numeroMaximo)
        {
            // Limpiar el DropDownList en caso de que tenga elementos
            ddlUbicacion.Items.Clear();

            // Añadir un valor vacío al principio
            ListItem itemVacio = new ListItem("", "");
            ddlUbicacion.Items.Add(itemVacio);

            // Llenar el DropDownList con números desde 1 hasta el número máximo
            for (int i = 1; i <= numeroMaximo; i++)
            {
                // Crear un nuevo ListItem con el mismo valor y texto (el número)
                ListItem item = new ListItem(i.ToString(), i.ToString());

                // Añadir el item al DropDownList
                ddlUbicacion.Items.Add(item);
            }
        }

        public int CalcularUbicacionModulo(string Id_Numerico)
        {
            int resultado = 0;
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // SQL para llamar al procedimiento almacenado
            string sSql = "ctaMaxUbicacionModulo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sSql, connection))
                    {
                        // Especificar que se trata de un procedimiento almacenado
                        command.CommandType = CommandType.StoredProcedure;

                        // Añadir el parámetro necesario (@IDpanelnum)
                        command.Parameters.AddWithValue("@IDpanelnum", Id_Numerico);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Verificar si hay resultados
                            if (reader.Read())
                            {
                                // Si el segundo campo (índice 1) es mayor que 0, retornar 0, si no, retornar 1
                                if (Convert.ToInt32(reader[1]) > 0)
                                {
                                    resultado = 0;
                                }
                                else
                                {
                                    resultado = 1;
                                }
                            }
                            else
                            {
                                // Si no hay resultados, retornar 0
                                resultado = 0;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return resultado;
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
            string GrupoElementos = "";

            if (DropDesGrupo.SelectedItem.Text.ToUpper() == "PANELES LINEA 3500")
            {
                GrupoElementos = "3500";
            }
            else if (DropDesGrupo.SelectedItem.Text.ToUpper() == "PANELES LINEA 7000")
            {
                GrupoElementos = "7000";
            }
            else
            {
                GrupoElementos = "%";
            }


            if (CheckBase.Checked)
            {
                // Consulta para optBases = True
                consultaBase = "SELECT TOP 200  tblModulo.*, tblTipoModulo.Id_TipoModulo, tblModulo.Descripcion_Modulo, " +
                               "tblFamiliaModulo.Descripcion_Familia, tblTipoModulo.Descripcion_TipoModulo " +
                               "FROM tblTipoModulo " +
                               "INNER JOIN tblFamiliaModulo " +
                               "INNER JOIN tblModulo ON tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia " +
                               "ON tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                               "WHERE tblTipoModulo.Id_TipoModulo = 1 " +
                               "AND tblModulo.Descripcion_Modulo LIKE '%' + @Criterio + '%' " +
                               "AND tblModulo.Descripcion_Modulo LIKE '%' + @GrupoElementos + '%' " +
                               "AND tblModulo.Altura LIKE @Altura + '%' " +
                               "AND tblFamiliaModulo.Descripcion_Familia LIKE @FamiliaModulo + '%' " +
                               "ORDER BY tblModulo.Descripcion_Modulo, tblModulo.Altura";

                // Limpiar el ComboBox de Ubicación
                ddlUbicacion.Items.Clear();
                ddlUbicacion.Items.Add("0");
                ddlUbicacion.SelectedIndex = 0; // Establecer el primer elemento como seleccionado
                ddlUbicacion.Enabled = false; // Deshabilitar el ComboBox de Ubicación

                // Establecer la cantidad
                ddlCantidad.Items.Clear();
                ddlCantidad.Items.Add("1");
                ddlCantidad.SelectedIndex = 0; // Asignar 1 como cantidad
                ddlCantidad.Enabled = false; // Deshabilitar el ComboBox de Cantidad

                // Limpiar y agregar elementos al ComboBox de Lado
                ddlLado.Items.Clear();
                ddlLado.Items.Add("AB");
                ddlLado.SelectedIndex = 0; // Establecer el primer elemento como seleccionado
                ddlLado.Enabled = false; // Deshabilitar el ComboBox de Lado



            }
            else
            {
                // Consulta para optBases = False
                consultaBase = "SELECT TOP 200 tblModulo.*, tblTipoModulo.Id_TipoModulo, tblModulo.Descripcion_Modulo, " +
                               "tblFamiliaModulo.Descripcion_Familia, tblTipoModulo.Descripcion_TipoModulo " +
                               "FROM tblTipoModulo " +
                               "INNER JOIN tblFamiliaModulo " +
                               "INNER JOIN tblModulo ON tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia " +
                               "ON tblTipoModulo.Id_TipoModulo = tblModulo.Id_TipoModulo " +
                               "WHERE (tblTipoModulo.Id_TipoModulo = 2 OR tblTipoModulo.Id_TipoModulo = 3) " +
                               "AND tblModulo.Descripcion_Modulo LIKE '%' + @Criterio + '%' " +
                               "AND tblModulo.Descripcion_Modulo LIKE '%' + @GrupoElementos + '%' " +
                               "AND tblModulo.Altura LIKE @Altura + '%' " +
                               "AND tblFamiliaModulo.Descripcion_Familia LIKE @FamiliaModulo + '%' " +
                               "ORDER BY tblModulo.Descripcion_Modulo, tblModulo.Altura";


                // Invocar el metodo HabilitarAdicionar();
                ddlCantidad.Items.Clear();
                ddlCantidad.Items.Add("");
                ddlCantidad.Items.Add("1");
                ddlCantidad.Items.Add("2");

                ddlLado.Items.Clear();
                ddlLado.Items.Add("");
                ddlLado.Items.Add("A");
                ddlLado.Items.Add("B");
                ddlLado.Items.Add("AB");


                ddlUbicacion.Enabled = true;
                ddlCantidad.Enabled = true;
                ddlLado.Enabled = true;

                // llenamos la Ubicacion 
                LlenarUbicacion(Convert.ToInt32(DropDivisiones.SelectedValue));

            }


            // Conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(consultaBase, connection);

                // Agregar parámetros de manera segura
                command.Parameters.AddWithValue("@Criterio", TextCriterio.Text.Trim());
                command.Parameters.AddWithValue("@GrupoElementos", GrupoElementos);
                command.Parameters.AddWithValue("@Altura", TextAlturaConfigurar.Text.Trim());
                command.Parameters.AddWithValue("@FamiliaModulo", ddlFamiliaModulo.SelectedValue.Trim());

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

                // llamar el metodo Habilitar Adicionar 

                tbIdModuloAdicionar.Text = row.Cells[1].Text;
                lbNombreModuloAgregar.Text = row.Cells[2].Text;

                Session["IDModulo"] = row.Cells[1].Text;


                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);

                Habilitar_BotonAdicionar();

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

                // Funcionalidad Agregar Insumo al objeto

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
                        cmd.Parameters.AddWithValue("@Id_Panel", TextObjetoo.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Descripcion_Panel", TextDesInt.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@Id_GrupoObjeto", DropDesGrupo.SelectedValue);
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
                        cmd.Parameters.AddWithValue("@Descripcion_Tecnica", TextAreaDescripTec.Text.ToUpper());
                        cmd.Parameters.AddWithValue("@idInsumoReferencia", TextIndReferencia.Text);
                        cmd.Parameters.AddWithValue("@UndxPaquete", TextUndXPaq.Text);

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


                string ID_NumericoCreado = ConsultarID_Nuevo_Modificar();
                Session["Id_numericoDise"] = ID_NumericoCreado;
                Session["CrudObjetosDibujo"] = "Modificar";

                string mensajePersonalizado = "El Usuario con cédula: " + CedulaLogeada + " Ingresa el objeto: " + TextObjetoo;
                string urlRedireccion = "DiseñoYDesarrollo/ObjetosDibujo.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");



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

                decimal anchoAnterior = Convert.ToDecimal(dr["AnchoTag"].ToString());
                decimal alturaAnterior = Convert.ToDecimal(dr["AlturaTag"].ToString());


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

                    // Funcionalidad Agregar Insumo al objeto

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
                        updateCmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text.Trim().ToUpper());
                        updateCmd.Parameters.AddWithValue("@UndxPaquete", TextUndXPaq.Text.Trim());
                        updateCmd.Parameters.AddWithValue("@Descripcion_Panel", TextDesInt.Text.Trim().ToUpper());
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
                        updateCmd.Parameters.AddWithValue("@Descripcion_Tecnica", TextAreaDescripTec.Text.Trim().ToUpper());
                        updateCmd.Parameters.AddWithValue("@idInsumoReferencia", TextIndReferencia.Text.Trim());
                        updateCmd.Parameters.AddWithValue("@Id_PanelTag", dr["IdPanelTag"].ToString());
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
                        UPDATE tblPanel SET  Id_Panel = @Id_Panel, UndxPaquete = @UndxPaquete,Descripcion_Panel = @Descripcion_Panel, 
                        Id_GrupoObjeto = @Id_GrupoObjeto, Id_Linea = @Id_Linea, Divisiones = @Divisiones, HOLGURA = @HOLGURA, 
                        Precio_Venta = 0, Profundidad = @Profundidad, Escalable = @Escalable, RegistradoSag = 0, chequeado = @chequeado, 
                        Responsable = @Responsable, FechaChequeo = @FechaChequeo,
                        Descripcion_Tecnica = dbo.fn_DescripcionObjeto (@goSPDescripcionObjto,tblPanel.Id_Numerico, @goDescripcionbASEObjeto)
                        WHERE Id_Panel = @Id_PanelTag";

                    using (SqlCommand updateRelatedCmd = new SqlCommand(updateRelatedPanelsSql, con))
                    {
                        updateRelatedCmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text.Trim().ToUpper());
                        updateRelatedCmd.Parameters.AddWithValue("@UndxPaquete", TextUndXPaq.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@Descripcion_Panel", TextDesInt.Text.Trim().ToUpper());
                        updateRelatedCmd.Parameters.AddWithValue("@Id_GrupoObjeto", DropDesGrupo.SelectedValue);
                        updateRelatedCmd.Parameters.AddWithValue("@Id_Linea", DropLinea.SelectedValue);
                        updateRelatedCmd.Parameters.AddWithValue("@Divisiones", DropDivisiones.Text);
                        updateRelatedCmd.Parameters.AddWithValue("@HOLGURA", TextHolgura.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@Profundidad", TextProfundidad.Text.Trim());
                        updateRelatedCmd.Parameters.AddWithValue("@Escalable", CheckEstable.Checked);
                        updateRelatedCmd.Parameters.AddWithValue("@chequeado", CheckChequeado.Checked);
                        updateRelatedCmd.Parameters.AddWithValue("@Responsable", nombreEmpleado);
                        updateRelatedCmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now.ToString("MM/dd/yyyy HH:mm"));


                        DataTable DatoDescriObjeto = ConsultarDatosGrupoObjeto();

                        string goSPDescripcionObjto = "";
                        string goDescripcionbASEObjeto = "";


                        if (DatoDescriObjeto != null && DatoDescriObjeto.Rows.Count > 0)
                        {
                            DataRow fila = DatoDescriObjeto.Rows[0];
                            goSPDescripcionObjto = fila["goSPDescripcionObjto"].ToString(); ;
                            goDescripcionbASEObjeto = fila["goDescripcionbASEObjeto"].ToString(); ;

                        }

                        updateRelatedCmd.Parameters.AddWithValue("@goSPDescripcionObjto", goSPDescripcionObjto);
                        updateRelatedCmd.Parameters.AddWithValue("@goDescripcionbASEObjeto", goDescripcionbASEObjeto);
                        updateRelatedCmd.Parameters.AddWithValue("@Id_PanelTag", dr["IdPanelTag"].ToString());

                        updateRelatedCmd.ExecuteNonQuery();
                    }

                    string update2 = @"
                        UPDATE tblPanel Set Precio_Venta = 0 
                        WHERE (Id_Panel IN (
                        SELECT tblPlano.Plano FROM tblPlano 
                        INNER JOIN tblPlano_Panel ON tblPlano.Plano = tblPlano_Panel.Id_Plano 
                        INNER JOIN tblPanel AS tblPanel_1 ON tblPlano_Panel.Id_PanelNum = tblPanel_1.Id_Numerico 
                        WHERE  (tblPlano.Tipologia = 1) 
                        AND (tblPanel_1.Id_Panel = @ID_Panel) 
                        GROUP BY tblPlano.Plano))";

                    using (SqlCommand update2Cmd = new SqlCommand(update2, con))
                    {
                        update2Cmd.Parameters.AddWithValue("@Id_Panel", "N" + TextObjeto.Text.Trim());

                        update2Cmd.ExecuteNonQuery();
                    }
                }

                string ID_NumericoCreado = ConsultarID_Nuevo_Modificar();
                Session["Id_numericoDise"] = ID_NumericoCreado;
                Session["CrudObjetosDibujo"] = "Modificar";

                string mensajePersonalizado = "El Usuario con cédula: " + CedulaLogeada + " Modifica el objeto: " + TextObjetoo;
                string urlRedireccion = "DiseñoYDesarrollo/ObjetosDibujo.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

            }

            else if (tipoAccion == "Copiar")
            {

                DataTable dt = ViewState["PanelTags"] as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    // Manejar el caso en que el DataTable no esté inicializado o esté vacío
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error: DataTable no está inicializado.');", true);
                    return;
                }

                DataRow dr = dt.Rows[0];
                string ObjetoNoModi = dr["IdPanelTag"].ToString();

                if (ObjetoNoModi != TextObjeto.Text)
                {
                    if (!ValidarFamiliaExistente())
                    {
                        CheckChequeado.Checked = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El Objeto " + TextObjeto.Text.Trim() + " no es escalable automaticamente, requiere de proceso(s) manual(es), de lo contrario puede contener errores en el despiece.');", true);
                        return;
                    }

                }
                else
                {
                    if (CheckEstable.Checked == false)
                    {
                        SpanId_Objeto.InnerText = TextObjeto.Text;
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirContinuarNoEscalable').modal('show');", true);
                        return;

                    }
                    else
                    {
                        if (ValidarExistenciaObjetoAnchoAltura())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El Objeto " + TextObjeto.Text.Trim() + " con ancho" + TextAncho.Text + " ya existe. ');", true);
                            return;
                        }

                    }
                }


                if (ProcesoInsertarObjetoPanel())
                {
                    Session["CrudObjetosDibujo"] = "Modificar";
                    string mensajePersonalizado = "El Usuario con cédula: " + CedulaLogeada + " copia el objeto: " + TextObjeto.Text;
                    string urlRedireccion = "DiseñoYDesarrollo/ObjetosDibujo.aspx";
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Ocurrio un error al copiar el objeto.');", true);
                }

            }

        }

        public bool ValidarFamiliaExistente()
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select 1 from tblPanel where Id_Panel = @ID_Panel";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Panel", TextObjeto.Text);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            existe = true;
                        }
                    }
                }
            }

            return existe;
        }

        public bool ValidarExistenciaObjetoAnchoAltura()
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 1 FROM tblPanel WHERE Id_Panel= @ID_Panel AND Ancho = @Ancho AND Altura = @Altura";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Panel", TextObjeto.Text);
                    command.Parameters.AddWithValue("@Ancho", TextAncho.Text);
                    command.Parameters.AddWithValue("@Altura", TextAltura.Text);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            existe = true;
                        }
                    }
                }
            }

            return existe;
        }

        protected void btnContinuarProceso_SI_Click(object sender, EventArgs e)
        {
            string CedulaLogeada = Session["CedulaLogeada"].ToString();

            if (ProcesoInsertarObjetoPanel())
            {
                Session["CrudObjetosDibujo"] = "Modificar";
                string mensajePersonalizado = "El Usuario con cédula:" + CedulaLogeada + " copia el objeto: " + TextObjeto.Text;
                string urlRedireccion = "DiseñoYDesarrollo/ObjetosDibujo.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Ocurrio un error al copiar el objeto.');", true);
            }
        }

        private DataTable ConsultarDatoModulo(string ID_Numerico)
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblPanel_Modulo WHERE Id_PanelNum = @ID_Numerico";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@ID_Numerico", ID_Numerico);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }

        private bool ProcesoInsertarObjetoPanel()
        {
            bool procesoExitoso = false;
            DataTable datoModulo = ConsultarDatoModulo(TextIdNum.Text);

            if (InsertarObjetoPanel())
            {
                string ID_NumericoCreado = ConsultarID_NumericoCreado();


                foreach (DataRow row in datoModulo.Rows)
                {

                    string ID_Modulo = row["ID_Modulo"].ToString();
                    string Ubicacion_Modulo = row["Ubicacion_Modulo"].ToString();
                    string Lado = row["Lado"].ToString();
                    string Cantidad = row["Cantidad"].ToString();
                    string Observaciones = row["Observaciones"].ToString();

                    if (InsertarPanelModulo(ID_NumericoCreado, ID_Modulo, Ubicacion_Modulo, Lado, Cantidad, Observaciones))
                    {
                        // Mensaje de Exito 

                    }
                    else
                    {
                        // Mensaje Error al crear al panel_modulo 
                    }
                }
                Session["Id_numericoDise"] = ID_NumericoCreado;
                procesoExitoso = true;
            }
            else
            {
                // Mensaje Error al crear el objeto 
            }

            return procesoExitoso;

        }

        private bool InsertarObjetoPanel()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblPanel(Id_Panel,Descripcion_Panel,Id_GrupoObjeto,Ancho,Altura,Id_Linea,Divisiones,HOLGURA," +
                              "Profundidad,Escalable,cubicajem3,Activo,chequeado,Responsable,FechaChequeo,Descripcion_Tecnica,UndxPaquete) " +
                              "VALUES (@Id_Panel,@Descripcion_Panel,@Id_GrupoObjeto,@Ancho,@Altura,@Id_Linea,@Divisiones,@HOLGURA,@Profundidad," +
                              "@Escalable,@cubicajem3,@Activo,@chequeado,@Responsable,@FechaChequeo,@Descripcion_Tecnica,@UndxPaquete)";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text);
                    cmd.Parameters.AddWithValue("@Descripcion_Panel", TextDesInt.Text);
                    cmd.Parameters.AddWithValue("@Id_GrupoObjeto", DropDesGrupo.SelectedValue);
                    cmd.Parameters.AddWithValue("@Ancho", TextAncho.Text);
                    cmd.Parameters.AddWithValue("@Altura", TextAltura.Text);
                    cmd.Parameters.AddWithValue("@Id_Linea", DropLinea.SelectedValue);
                    cmd.Parameters.AddWithValue("@Divisiones", DropDivisiones.Text);
                    cmd.Parameters.AddWithValue("@HOLGURA", TextHolgura.Text);
                    cmd.Parameters.AddWithValue("@Profundidad", TextProfundidad.Text);
                    cmd.Parameters.AddWithValue("@Escalable", CheckEstable.Checked);
                    cmd.Parameters.AddWithValue("@cubicajem3", TextCubicaje.Text);
                    cmd.Parameters.AddWithValue("@Activo", CheckActivo.Checked);
                    cmd.Parameters.AddWithValue("@chequeado", CheckChequeado.Checked);
                    cmd.Parameters.AddWithValue("@Responsable", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Descripcion_Tecnica", TextAreaDescripTec.Text);
                    cmd.Parameters.AddWithValue("@UndxPaquete", TextUndXPaq.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
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

        protected string ConsultarID_NumericoCreado()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string ID = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_Numerico FROM tblpanel WHERE Id_Panel = @ID_Panel AND Ancho = @Ancho AND  Altura = @Altura";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Panel", TextObjeto.Text);
                    command.Parameters.AddWithValue("@Ancho", TextAncho.Text);
                    command.Parameters.AddWithValue("@Altura", TextAltura.Text);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ID = reader["id_Numerico"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo, loguear el error
                // También puedes lanzar una excepción o devolver un mensaje de error
            }
            return ID;
        }

        protected string ConsultarID_Nuevo_Modificar()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string ID = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_Numerico FROM tblpanel WHERE Id_Panel = @ID_Panel AND Ancho = @Ancho";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ID_Panel", TextObjeto.Text);
                    command.Parameters.AddWithValue("@Ancho", TextAncho.Text);


                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        ID = reader["id_Numerico"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo, loguear el error
                // También puedes lanzar una excepción o devolver un mensaje de error
            }
            return ID;
        }

        private bool InsertarPanelModulo(string ID_NumericoCreado, string ID_Modulo, string Ubicacion_Modulo, string Lado, string Cantidad, string Observaciones)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblPanel_Modulo(Id_PanelNum,Id_Modulo,Ubicacion_Modulo,Lado,Cantidad,Observaciones,PanModResponsable,FechaConfiguracion)" +
                              "VALUES (@Id_PanelNum,@Id_Modulo,@Ubicacion_Modulo,@Lado,@Cantidad,@Observaciones,@PanModResponsable,@FechaConfiguracion)";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Id_PanelNum", ID_NumericoCreado);
                    cmd.Parameters.AddWithValue("@Id_Modulo", ID_Modulo);
                    cmd.Parameters.AddWithValue("@Ubicacion_Modulo", Ubicacion_Modulo);
                    cmd.Parameters.AddWithValue("@Lado", Lado);
                    cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
                    cmd.Parameters.AddWithValue("@Observaciones", Observaciones);
                    cmd.Parameters.AddWithValue("@PanModResponsable", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@FechaConfiguracion", DateTime.Now);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
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

        protected void TextObjeto_TextChanged(object sender, EventArgs e)
        {

            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void DropLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            EstadoGrabarCancelar();
        }

        protected void DropDesGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void TextAncho_TextChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void TextProfundidad_TextChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void TextCubicaje_TextChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void DropDivisiones_SelectedIndexChanged(object sender, EventArgs e)
        {
            EstadoGrabarCancelar();
        }

        protected void TextHolgura_TextChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void TextUndXPaq_TextChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void TextIndReferencia_TextChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void TextAreaDescripTec_TextChanged(object sender, EventArgs e)
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion == "Consultar")
            {
                // Validar Accion 
            }
            else
            {
                EstadoGrabarCancelar();
            }
        }

        protected void CheckApliCodPSLOT_CheckedChanged(object sender, EventArgs e)
        {
            EstadoGrabarCancelar();

            if (CheckApliCodPSLOT.Checked)
            {
                TextIdInsumo.Enabled = true;
                TextIdInsumo.CssClass = "form-control form-control-sm";
            }
            else
            {
                TextIdInsumo.Enabled = false;
                TextIdInsumo.CssClass = "form-control form-control-sm";

                TextIdInsumo.Text = "";
                TextCodPSL.Text = "";
                TextInRelOtNoOai.Text = "";
            }
        }

        protected void btnPosback_Click(object sender, EventArgs e)
        {
            // solo para activar el postback para generar el evento de cambio de id de insumo 
        }

        protected void TextIdInsumo_TextChanged(object sender, EventArgs e)
        {
            btnEliminarInsumo.Enabled = false;
            btnEliminarInsumo.CssClass = "btn btn-sm btn-outline-secondary";


            if (TextIdInsumo.Text == "")
            {
                // Mensaje modal de si o no
                // El campo de id insumo está vacío, debe ingresar el id del insumo asociado.
                //¿Desea aseociar al objeto algún id de insumo para las OT diferentes a OAI?

                TextCodPSL.Text = "";
                TextInRelOtNoOai.Text = "";
                TextIdInsumo.Text = "";
                TextIdInsumo.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El ID de insumo no puede estar vacio.');", true);
                EstadoGrabarCancelar();
                CheckApliCodPSLOT.Checked = false;
                return;
            }
            else
            {

                // Buscar con el ID_Insumo los valores o mostrar el mensaje si no existe
                int insumoId;
                bool esNumero = int.TryParse(TextIdInsumo.Text, out insumoId);

                // Rango de SMALLINT: de -32,768 a 32,767
                if (esNumero && insumoId >= -32768 && insumoId <= 32767)
                {
                    // El valor es un número válido en el rango de SMALLINT
                    DataTable DatosXIdInsumo = ConsultarDatosXIdInsumo(TextIdInsumo.Text);

                    if (DatosXIdInsumo != null && DatosXIdInsumo.Rows.Count > 0)
                    {
                        DataRow fila = DatosXIdInsumo.Rows[0];

                        TextCodPSL.Text = fila["ID_Inventario"].ToString();
                        TextInRelOtNoOai.Text = fila["Descripcion_Insumo"].ToString();
                        CheckApliCodPSLOT.Checked = true;
                    }
                    else
                    {
                        // No existen datos para el ID proporcionado
                        TextCodPSL.Text = "";
                        TextInRelOtNoOai.Text = "";
                        TextIdInsumo.Text = "";
                        TextIdInsumo.Focus();
                        CheckApliCodPSLOT.Checked = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El insumo no existe, por favor revise el ID ingresado');", true);
                    }
                }
                else
                {
                    // El número está fuera del rango de SMALLINT o no es un número válido
                    TextCodPSL.Text = "";
                    TextInRelOtNoOai.Text = "";
                    TextIdInsumo.Text = "";
                    TextIdInsumo.Focus();
                    CheckApliCodPSLOT.Checked = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El insumo no existe, por favor revise el ID ingresado.');", true);
                }

                EstadoGrabarCancelar();

            }

        }

        private DataTable ConsultarDatosXIdInsumo(string ID_Insumo)
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT ID_Inventario, Descripcion_Insumo FROM tblInsumo WHERE Id_Insumo =@ID_Insumo";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@ID_Insumo", ID_Insumo);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }

        private DataTable ConsultarDatosGrupoObjeto()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT goDescripcionBaseObjeto,goSPDescripcionObjto FROM tblGrupoObjeto " +
                              "WHERE ID_GrupoObjeto = @ID_Grupo ORDER BY Descripcion_Grupo";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@ID_Grupo", DropDesGrupo.SelectedValue);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }

        protected void BtnCancelarObjetosPanel_Click(object sender, EventArgs e)
        {
            LoadData();
            DisposicionInicialBotones();
            EstadoGrabarCancelarInicial();
            DataGridInicial();
        }

        protected void btnActuaValComercial_Click(object sender, EventArgs e)
        {

            if (TextIdNum.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No se ha selecciona ninguna familia.');", true);
                return;
            }
            else
            {
                PanelActualizar.InnerText = TextObjeto.Text;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirmarActualizarValorComercial').modal('show');", true);
                return;
            }



        }

        protected void btnActualizarValorComercial_SI_Click(object sender, EventArgs e)
        {
            DataTable DatosPanel = ConsultarDatosPanel();

            if (DatosPanel != null && DatosPanel.Rows.Count > 0)
            {
                foreach (DataRow row in DatosPanel.Rows)
                {
                    string ID_Numerico = row["Id_Numerico"].ToString();

                    EjecutarActualizarPrecioObjeto(ID_Numerico);

                }
            }

            TextValorComercial.Text = ConsultarPrecioVenta();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Se actualizo correctamente el valor venta de la familia:" + TextObjeto.Text.Trim() + "');", true);
            return;

        }

        private DataTable ConsultarDatosPanel()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblpanel WHERE Id_Panel= @ID_Panel";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@ID_Panel", TextObjeto.Text);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }

        // Método para ejecutar el procedimiento almacenado sp_ActualizarPrecioObjeto
        public void EjecutarActualizarPrecioObjeto(string idPanelNumerico)
        {

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStringSID))
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_ActualizarPrecioObjeto", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Objeto", SqlDbType.Int)).Value = idPanelNumerico;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores (puedes registrar el error o lanzar una excepción)
                Console.WriteLine("Error al ejecutar el procedimiento: " + ex.Message);
                throw;
            }
        }

        protected string ConsultarPrecioVenta()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string PrecioVenta = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Precio_Venta FROM tblPanel WHERE Id_Numerico = @Id_Numerico";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id_Numerico", TextIdNum.Text);


                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        PrecioVenta = reader["Precio_Venta"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo, loguear el error
                // También puedes lanzar una excepción o devolver un mensaje de error
            }
            return PrecioVenta;
        }

        protected void CheckComplementarios_CheckedChanged(object sender, EventArgs e)
        {
            CheckBase.Checked = false;
            CheckBase.Enabled = true;

            CheckComplementarios.Checked = true;

            btnCopiarModulo.Enabled = false;
            btnCopiarModulo.CssClass = "btn btn-sm button-disabled shadow";

            btnAdicionarModulo.Enabled = false;
            btnAdicionarModulo.CssClass = "btn btn-sm button-disabled shadow";

            btnEliminarModuloObjeto.Enabled = false;
            btnEliminarModuloObjeto.CssClass = "btn btn-sm button-disabled shadow";

            BindDataGrid();
        }

        protected void ddlUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLadosPanel();
            Habilitar_BotonAdicionar();
        }

        protected void ddlCantidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLadosPanel();
            Habilitar_BotonAdicionar();
        }

        protected void ddlLado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Habilitar_BotonAdicionar();
        }

        private void CargarLadosPanel()
        {
            // Limpiar los items actuales del DropDownList
            ddlLado.Items.Clear();


            if (ddlCantidad.SelectedItem.Text == "1")
            {
                LlenarLado(TextIdNum.Text, ddlUbicacion.SelectedValue);
            }
            else if (ddlCantidad.SelectedItem.Text == "2")
            {
                // Agregar los elementos para cuando la cantidad es 2
                ddlLado.Items.Add(new ListItem("AB", "AB"));

            }
        }

        private void LlenarLado(string panelNum, string ubicacion)
        {
            // Limpiar el DropDownList antes de llenarlo
            ddlLado.Items.Clear();


            string ladoExistente = ObtenerLadoExistente(panelNum, ubicacion);

            if (!string.IsNullOrEmpty(ladoExistente))
            {
                // Si el procedimiento almacenado retorna "A" o "B", se agrega el lado opuesto
                switch (ladoExistente)
                {
                    case "A":
                        ddlLado.Items.Add(new ListItem("B", "B"));
                        break;
                    case "B":
                        ddlLado.Items.Add(new ListItem("A", "A"));
                        break;
                }
            }
            else
            {
                // Si no hay lado existente y la ubicación no es 0, se agregan "A" y "B"
                if (ubicacion != "0")
                {
                    ddlLado.Items.Add(new ListItem("A", "A"));
                    ddlLado.Items.Add(new ListItem("B", "B"));
                }
                else
                {
                    // Si la ubicación es 0, se agrega "AB"
                    ddlLado.Items.Add(new ListItem("AB", "AB"));
                }
            }

            // Seleccionar el primer elemento de la lista
            if (ddlLado.Items.Count > 0)
            {
                ddlLado.SelectedIndex = 0;
            }
        }

        private string ObtenerLadoExistente(string panelNum, string ubicacion)
        {
            string ladoExistente = null;

            // Define la conexión a la base de datos (asegúrate de cambiar la cadena de conexión)
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Define el comando y el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand("ctaLadoExistente", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros necesarios
                    cmd.Parameters.AddWithValue("@P", panelNum);
                    cmd.Parameters.AddWithValue("@U", ubicacion);

                    // Abrir conexión
                    conn.Open();

                    // Ejecutar la consulta
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Si hay resultados, obtener el valor del lado
                    if (reader.Read())
                    {
                        ladoExistente = reader["Lado"].ToString();
                    }

                    reader.Close();
                }
            }

            return ladoExistente;
        }

        private void Habilitar_BotonAdicionar()
        {
            string tipoAccion = Session["CrudObjetosDibujo"] as string;

            if (tipoAccion.ToUpper() != "CONSULTAR" && tbIdModuloAdicionar.Text.Trim() != "")
            {
                btnCopiarModulo.Enabled = true;
                btnCopiarModulo.CssClass = "btn btn-sm  shadow ColorAzulActivo";

                if (ddlCantidad.SelectedValue.Trim() != "" && ddlLado.SelectedValue.Trim() != "" && ddlUbicacion.SelectedValue.Trim() != "")
                {
                    btnAdicionarModulo.Enabled = true;
                    btnAdicionarModulo.CssClass = "btn btn-sm  shadow ColorAzulActivo";

                    VerificarCantidadModulo(TextIdNum.Text, ddlUbicacion.SelectedItem.Text.Trim());
                }
                else
                {
                    btnAdicionarModulo.Enabled = false;
                    btnAdicionarModulo.CssClass = "btn button-disabled btn-sm   shadow ";
                }

            }
        }

        private void VerificarCantidadModulo(string panelNum, string ubicacion)
        {
            string sqlQuery = "ctaCantidadModuloEnUnPanelRespectoUbicacion";
            int cantidad = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@P", panelNum.ToString());
                    cmd.Parameters.AddWithValue("@U", ubicacion.ToString());

                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        cantidad = 0;

                        // Si la ubicación no es 0 habilitar adicionar modulo
                        if (ubicacion != "0")
                        {
                            ddlCantidad.Enabled = true;
                            btnAdicionarModulo.Enabled = true;
                        }
                    }
                    else
                    {
                        // Si hay resultados, obtener la cantidad
                        reader.Read();
                        cantidad = reader.GetInt32(0);

                        if (cantidad == 2)
                        {
                            // Si la cantidad es 2, deshabilitar el botón adicionar modulo
                            btnAdicionarModulo.Enabled = false;
                            btnAdicionarModulo.CssClass = "btn btn-sm button-disabled shadow";
                        }
                        else
                        {

                            string val = ddlCantidad.SelectedValue;

                            ddlCantidad.Items.Clear();
                            // De lo contrario, habilitar el boton adicionar modulo           
                            ddlCantidad.Items.Add(new ListItem("", ""));
                            ddlCantidad.Items.Add(new ListItem("1", "1"));
                            ddlCantidad.Items.Add(new ListItem("2", "2"));

                            ddlCantidad.SelectedValue = val;


                            ddlCantidad.Enabled = true;
                            btnAdicionarModulo.Enabled = true;
                        }
                    }

                    reader.Close();
                }
            }
        }

        protected void DataGridModulosAsociados_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "ModuloAsociado")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridModulosAsociados.Items[rowIndex];

                if (e.Item.CssClass == "fila-seleccionada1")
                {
                    e.Item.CssClass = "";
                }
                else
                {
                    e.Item.CssClass = "fila-seleccionada1";
                }


                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


                // llamar el metodo Habilitar Adicionar 

                tbIdModuloEliminar.Text = row.Cells[1].Text;
                lbNombreModuloEliminar.Text = row.Cells[2].Text;

                string tipoAccion = Session["CrudObjetosDibujo"] as string;

                if (tipoAccion.ToUpper() != "CONSULTAR" && tbIdModuloEliminar.Text.Trim() != "")
                {
                    btnEliminarModuloObjeto.Enabled = true;
                    btnEliminarModuloObjeto.CssClass = "btn btn-sm  shadow RojoCancelar";
                }
                else
                {
                    btnEliminarModuloObjeto.Enabled = false;
                    btnEliminarModuloObjeto.CssClass = "btn button-disabled btn-sm   shadow ";
                }



            }
        }

        protected void CheckBase_CheckedChanged(object sender, EventArgs e)
        {

            if (CheckBase.Checked)
            {
                CheckBase.Checked = true;
                CheckBase.Enabled = true;

                CheckComplementarios.Checked = false;

                CheckBase.Enabled = false;

                BindDataGrid();
            }
            else
            {
                CheckBase.Checked = false;
                CheckBase.Enabled = false;

                CheckComplementarios.Checked = true;

                BindDataGrid();
            }




        }


        // Metodos Adicionar modulo al objeto 
        protected void btnAdicionarModulo_Click(object sender, EventArgs e)
        {

            //Agregamos los nombres del módulo y objeto al datagrid 
            NombreModulo.InnerText = lbNombreModuloAgregar.Text;
            NombreObjeto.InnerText = TextObjeto.Text;

            // mostrar el modal de  confirmar adicionar modulo
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirmarAdicionarModulo').modal('show');", true);
            return;

        }

        protected void btnAdicionarObjeto_SI_Click(object sender, EventArgs e)
        {
            bool ValidarExistenciaPanelModulo = ConsultarExistenciaPanelModulo(TextIdNum.Text);

            double Altura = ConsultarAlturaModuloAdicionar() + ConsultarIncrementoxGrupoObjeto();



            if (ddlUbicacion.SelectedValue == "0" || !ValidarExistenciaPanelModulo)
            {
                // Actualizar la altura del panel 
                ActualizarAlturaObjeto(Altura);
            }

            if (!CheckEstable.Checked)
            {

                if (tbIdModuloAdicionar.Text.Trim() != "")
                {
                    //SE AGREGA EL MODULO AL OBJETO
                    InsertarModuloAlObjeto();

                    //SE MODIFICA CAMPOS VARIOS DEL OBJETO
                    ActualizarPanel(TextIdNum.Text);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No se ha seleccionado ningun módulo para agregar');", true);
                    return;
                }

            }
            else
            {
                // CONSULTAR DATOS OBJETO
                DataTable DatoObjetoAltura = ConsultarDatosObjetoXAltura();

                if (DatoObjetoAltura != null && DatoObjetoAltura.Rows.Count > 0)
                {

                    foreach (DataRow row in DatoObjetoAltura.Rows)
                    {

                        string ID_Numerico = row["Id_Numerico"].ToString();

                        // Se consulta la tabla tblPanel_Modulo
                        if (!ValidarExisteciaPanel_Modulo())
                        {
                            //SE AGREGA EL MODULO A LA FAMILIA DEL OBJETO
                            if (!InsertarPanelModulo(ID_Numerico, tbIdModuloAdicionar.Text, ddlUbicacion.SelectedValue, ddlLado.SelectedValue, ddlCantidad.SelectedItem.Text, txObservacion.InnerText))
                            {
                                // Error al realizar la insercion en la tabla Panel Modulo;
                            }

                        }
                    }

                    // SE MODIFICA CAMPOS VARIOS DE LA FAMILIA DEL OBJETO
                    ActualizarPanel1();

                }


            }


            // SE REGISTRA EL MOVIMIENTO DE CREACION DEL OBJETO ASOCIADO AL USUARIO QUE LO CREÓ 
            // El Usuario Con cedúla: + NUMEROCEDULA + Adicionó al objeto + NOMBREOBJETO + el modulo + NOMBREMODULO

            Session["ControlTapConfigurar"] = 1;

            string mensajePersonalizado = "El Usuario con cédula: " + Session["CedulaLogeada"].ToString() + " Adicionó al objeto " + TextObjeto.Text + "El módulo " + NombreModulo.InnerText;
            string urlRedireccion = "DiseñoYDesarrollo/ObjetosDibujo.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
        }

        public double ConsultarAlturaModuloAdicionar()
        {
            double altura = 0;
            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT Altura FROM tblModulo WHERE Id_Modulo = @ID_Modulo";

            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Modulo", tbIdModuloAdicionar.Text);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar(); // Obtener el primer valor de la consulta

                        if (result != null && result != DBNull.Value)
                        {
                            altura = Convert.ToDouble(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier excepción
                        Console.WriteLine("Error al consultar la altura: " + ex.Message);
                    }
                }
            }

            return altura;
        }

        private bool ActualizarAlturaObjeto(double altura)
        {

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblPanel set Altura =@Altura  WHERE Id_Panel = @Id_Panel AND Altura = @AlturaAnterior ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@Altura", altura);
                    cmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text);
                    cmd.Parameters.AddWithValue("@AlturaAnterior", TextAltura.Text);


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

        public double ConsultarIncrementoxGrupoObjeto()
        {
            double altura = 0;
            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT goIncrementarAlto FROM tblGrupoObjeto WHERE ID_GrupoObjeto = @ID_Grupo";

            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Grupo", DropDesGrupo.SelectedValue);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar(); // Obtener el primer valor de la consulta

                        if (result != null && result != DBNull.Value)
                        {
                            altura = Convert.ToDouble(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier excepción
                        Console.WriteLine("Error al consultar la altura: " + ex.Message);
                    }
                }
            }

            return altura;
        }

        private bool InsertarModuloAlObjeto()
        {

            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblPanel_Modulo(Id_PanelNum,Id_Modulo,Ubicacion_Modulo,Cantidad,Observaciones,Lado,PanModResponsable,FechaConfiguracion) " +
                              "VALUES (@Id_PanelNum,@Id_Modulo,@Ubicacion_Modulo,@Cantidad,@Observaciones,@Lado,@PanModResponsable,@FechaConfiguracion)";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Id_PanelNum", TextIdNum.Text);
                    cmd.Parameters.AddWithValue("@Id_Modulo", tbIdModuloAdicionar.Text);
                    cmd.Parameters.AddWithValue("@Ubicacion_Modulo", ddlUbicacion.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Cantidad", ddlCantidad.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@Observaciones", txObservacion.InnerText);
                    cmd.Parameters.AddWithValue("@Lado", ddlLado.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@PanModResponsable", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@FechaConfiguracion", DateTime.Now);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
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

        private bool ActualizarPanel(string ID_Numerico)
        {
            DataTable DatoDescriObjeto = ConsultarDatosGrupoObjeto();

            string goSPDescripcionObjto = "";
            string goDescripcionbASEObjeto = "";


            if (DatoDescriObjeto != null && DatoDescriObjeto.Rows.Count > 0)
            {
                DataRow fila = DatoDescriObjeto.Rows[0];
                goSPDescripcionObjto = fila["goSPDescripcionObjto"].ToString(); ;
                goDescripcionbASEObjeto = fila["goDescripcionbASEObjeto"].ToString(); ;

            }

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblPanel SET registradoSag= 0, Precio_venta=0, chequeado=0, Responsable=@Responsable," +
                          "FechaChequeo = @fechaCheq,Descripcion_Tecnica= dbo.fn_DescripcionObjeto (@goDescObj,tblPanel.Id_Numerico,@goDescripcionbASEObjeto)" +
                          " WHERE Id_numerico = @ID_Numerico ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@Responsable", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@fechaCheq", DateTime.Now);
                    cmd.Parameters.AddWithValue("@goDescObj", goSPDescripcionObjto);
                    cmd.Parameters.AddWithValue("@goDescripcionbASEObjeto", goDescripcionbASEObjeto);
                    cmd.Parameters.AddWithValue("@ID_Numerico", ID_Numerico);

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

        private bool ActualizarPanel2(string ID_Panel)
        {
            DataTable DatoDescriObjeto = ConsultarDatosGrupoObjeto();

            string goSPDescripcionObjto = "";
            string goDescripcionbASEObjeto = "";


            if (DatoDescriObjeto != null && DatoDescriObjeto.Rows.Count > 0)
            {
                DataRow fila = DatoDescriObjeto.Rows[0];
                goSPDescripcionObjto = fila["goSPDescripcionObjto"].ToString(); ;
                goDescripcionbASEObjeto = fila["goDescripcionbASEObjeto"].ToString(); ;

            }

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblPanel SET registradoSag= 0, Precio_venta=0, chequeado=0, Responsable=@Responsable," +
                          "FechaChequeo = @fechaCheq,Descripcion_Tecnica= dbo.fn_DescripcionObjeto (@goDescObj,tblPanel.Id_Numerico,@goDescripcionbASEObjeto)" +
                          " WHERE Id_Panel = @ID_Panel ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@Responsable", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@fechaCheq", DateTime.Now);
                    cmd.Parameters.AddWithValue("@goDescObj", goSPDescripcionObjto);
                    cmd.Parameters.AddWithValue("@goDescripcionbASEObjeto", goDescripcionbASEObjeto);
                    cmd.Parameters.AddWithValue("@ID_Panel", ID_Panel);

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

        private bool ActualizarPanel1()
        {
            DataTable DatoDescriObjeto = ConsultarDatosGrupoObjeto();

            string goSPDescripcionObjto = "";
            string goDescripcionbASEObjeto = "";


            if (DatoDescriObjeto != null && DatoDescriObjeto.Rows.Count > 0)
            {
                DataRow fila = DatoDescriObjeto.Rows[0];
                goSPDescripcionObjto = fila["goSPDescripcionObjto"].ToString(); ;
                goDescripcionbASEObjeto = fila["goDescripcionbASEObjeto"].ToString(); ;

            }

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblPanel SET registradoSag= 0, Precio_venta=0, chequeado=0, Responsable=@Responsable," +
                          "FechaChequeo = @fechaCheq,Descripcion_Tecnica= dbo.fn_DescripcionObjeto (@goDescObj,tblPanel.Id_Numerico,@goDescripcionbASEObjeto)" +
                          " WHERE Id_Panel = @Id_Panel AND Altura= @Altura ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@Responsable", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@fechaCheq", DateTime.Now);
                    cmd.Parameters.AddWithValue("@goDescObj", goSPDescripcionObjto);
                    cmd.Parameters.AddWithValue("@goDescripcionbASEObjeto", goDescripcionbASEObjeto);
                    cmd.Parameters.AddWithValue("@Id_Panel", TextObjeto.Text);
                    cmd.Parameters.AddWithValue("@Altura", TextAltura.Text);

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

        private DataTable ConsultarDatosObjetoXAltura()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblpanel WHERE Id_Panel= @ID_Panel  AND Altura = @Altura";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@ID_Panel", TextObjeto.Text);
                    cmd.Parameters.AddWithValue("@Altura", TextAltura.Text);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }

        private bool ConsultarExistenciaPanelModulo(string IDNumerico)
        {
            bool existe = false;
            string consulta = "SELECT COUNT(*) FROM tblPanel_Modulo WHERE Id_PanelNum = @IDNumerico";
            string connectionString = WebConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    // Agregar el parámetro para el IDNumerico
                    command.Parameters.AddWithValue("@IDNumerico", IDNumerico);

                    try
                    {
                        connection.Open();
                        // Ejecutar el COUNT y convertir el resultado a entero
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Si el conteo es mayor que 0, significa que el registro existe
                        if (count > 0)
                        {
                            existe = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar errores aquí (por ejemplo, registrar el error)
                        throw new ApplicationException("Error al verificar la existencia del registro", ex);
                    }
                }
            }

            return existe;
        }

        private bool ValidarExisteciaPanel_Modulo()
        {
            bool existe = false;
            string consulta = "SELECT COUNT(*) FROM tblPanel_Modulo WHERE Id_PanelNum = @IDNumerico AND Id_Modulo = @ID_Modulo " +
                              "AND Ubicacion_Modulo = @Ubicacion AND Lado =@lado ";
            string connectionString = WebConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    // Agregar el parámetro para el IDNumerico
                    command.Parameters.AddWithValue("@IDNumerico", TextIdNum.Text);
                    command.Parameters.AddWithValue("@ID_Modulo", tbIdModuloAdicionar.Text);
                    command.Parameters.AddWithValue("@Ubicacion", ddlUbicacion.SelectedValue);
                    command.Parameters.AddWithValue("@lado", ddlLado.SelectedValue);


                    try
                    {
                        connection.Open();
                        // Ejecutar el COUNT y convertir el resultado a entero
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Si el conteo es mayor que 0, significa que el registro existe
                        if (count > 0)
                        {
                            existe = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejar errores aquí (por ejemplo, registrar el error)
                        throw new ApplicationException("Error al verificar la existencia del registro", ex);
                    }
                }
            }

            return existe;
        }


        // Eliminar modulo asociado al objeto 
        protected void btnEliminarModuloObjeto_Click(object sender, EventArgs e)
        {

            // mostrar el modal de  confirmar adicionar modulo
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#confirmarQuitarModulo').modal('show');", true);
            return;
        }

        protected void btnQuitarModulo_SI_Click(object sender, EventArgs e)
        {
            List<(string IdModulo, string UbicacionModulo, string ID_Numerico, string Lado)> filasSeleccionadas = new List<(string, string, string, string)>();

            // Iterar sobre cada fila en el DataGrid
            foreach (DataGridItem item in DataGridModulosAsociados.Items)
            {
                // Verificar si la fila tiene la clase de seleccionada
                if (item.CssClass == "fila-seleccionada1")
                {

                    string idModulo = item.Cells[1].Text;
                    string ubicacionModulo = item.Cells[4].Text;
                    string Lado = item.Cells[7].Text;
                    string IDNumerico = item.Cells[11].Text;

                    // Agregar los valores como tupla a la lista
                    filasSeleccionadas.Add((idModulo, ubicacionModulo, IDNumerico, Lado));
                }
            }

            // Iterar sobre la lista de filas seleccionadas y eliminar los módulos según la condición
            foreach (var fila in filasSeleccionadas)
            {
                string idModulo = fila.IdModulo;
                string ubicacionModulo = fila.UbicacionModulo;
                string IDNumerico = fila.ID_Numerico;
                string Lado = fila.Lado;


                // Si Ubicacion_Modulo es 0
                if (ubicacionModulo == "0")
                {
                    // Solo permitir eliminar si es el único módulo seleccionado
                    if (filasSeleccionadas.Count == 1)
                    {
                        // Permitir eliminar
                        QuitarModulo(IDNumerico, idModulo, ubicacionModulo, Lado);
                    }

                }
                else
                {
                    // Eliminar normalmente los módulos que no tienen Ubicacion_Modulo = 0
                    QuitarModulo(IDNumerico, idModulo, ubicacionModulo, Lado);
                }
            }


            Session["ControlTapConfigurar"] = 1;

            string mensajePersonalizado = "Los módulos seleccionados se eliminaron correctamente";
            string urlRedireccion = "DiseñoYDesarrollo/ObjetosDibujo.aspx";
            Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");

        }

        private void QuitarModulo(string ID_Numerico, string idModulo, string Ubicacion, string Lado)
        {
            if (CheckEstable.Checked)
            {
                //Eliminar 
                if (EliminarPanelModulo(ID_Numerico, idModulo, Ubicacion, Lado))
                {
                    bool ValidarExistenciaPanelModulo = ConsultarExistenciaPanelModulo(ID_Numerico);
                    if (Ubicacion == "0" || !ValidarExistenciaPanelModulo)
                    {
                        ActualizarAlturaObjeto1(ID_Numerico);
                    }

                    ActualizarPanel(ID_Numerico);




                }
                else
                {

                    // Error al eliminar 
                }

                // Aqui va un registrar movimiento 

            }
            else
            {
                if (EliminarPanelModuloNoEscalable(TextObjeto.Text, idModulo, Ubicacion, Lado, TextAltura.Text))
                {
                    bool ValidarExistenciaPanelModulo = ConsultarExistenciaPanelModulo(ID_Numerico);
                    if (Ubicacion == "0" || !ValidarExistenciaPanelModulo)
                    {
                        ActualizarAlturaObjeto2(TextObjeto.Text, TextAltura.Text);
                    }

                    ActualizarPanel(ID_Numerico);
                }
                ActualizarPanel2(TextObjeto.Text);


                //Se consulta si el objeto hace parte de algun prototipo, en caso afirmativo se busca y se coloca el precio venta de los prototipos en 0(Cero).
                ActializarObjetoSiPrototipo(TextObjeto.Text);

                // Aqui va un registrar movimiento 

            }
        }

        private bool EliminarPanelModulo(string idNumerico, string idModulo, string ubicacionModulo, string lado)
        {
            bool exito = false;
            string connectionString = WebConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = @"DELETE FROM tblPanel_Modulo WHERE Id_PanelNum = @ID_Numerico AND Id_Modulo = @Id_Modulo 
                             AND Ubicacion_Modulo = @Ubicacion_Modulo AND Lado = @Lado";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar los parámetros con valores
                    command.Parameters.AddWithValue("@ID_Numerico", idNumerico);
                    command.Parameters.AddWithValue("@Id_Modulo", idModulo);
                    command.Parameters.AddWithValue("@Ubicacion_Modulo", ubicacionModulo);
                    command.Parameters.AddWithValue("@Lado", lado);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            exito = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        // Console.WriteLine("Error al eliminar el registro de tblPanel_Modulo: " + ex.Message);
                        exito = false;
                    }
                }
            }

            return exito;
        }

        private void ActualizarAlturaObjeto1(string idNumerico)
        {

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblPanel set Altura = 0  WHERE Id_Numerico = @Id_Numerico  ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@Id_Numerico", idNumerico);



                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();


                }
            }
        }

        private bool EliminarPanelModuloNoEscalable(string ID_Panel, string idModulo, string ubicacionModulo, string lado, string Altura)
        {
            bool exito = false;
            string connectionString = WebConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = @"DELETE FROM tblPanel_Modulo FROM tblPanel_Modulo INNER JOIN tblPanel ON tblPanel_Modulo.Id_PanelNum = tblPanel.Id_Numerico 
                            WHERE (tblPanel.Id_Panel = @ID_Panel ) AND (tblPanel_Modulo.Ubicacion_Modulo = @Ubicacion_Modulo) 
                            AND (tblPanel_Modulo.Id_Modulo = @ID_Modulo) AND  (tblPanel_Modulo.Lado = @Lado) AND (tblPanel.Altura = @Altura)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar los parámetros con valores
                    command.Parameters.AddWithValue("@ID_Panel", ID_Panel);
                    command.Parameters.AddWithValue("@Id_Modulo", idModulo);
                    command.Parameters.AddWithValue("@Ubicacion_Modulo", ubicacionModulo);
                    command.Parameters.AddWithValue("@Lado", lado);
                    command.Parameters.AddWithValue("@Altura", Altura);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            exito = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        // Console.WriteLine("Error al eliminar el registro de tblPanel_Modulo: " + ex.Message);
                        exito = false;
                    }
                }
            }

            return exito;
        }

        private void ActualizarAlturaObjeto2(string ID_Panel, string Altura)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            string sSql = "UPDATE tblPanel SET Altura = 0  WHERE Id_Panel = @ID_Panel AND Altura = @Altura";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@ID_Panel", ID_Panel);
                    cmd.Parameters.AddWithValue("@Altura", Altura);



                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();


                }
            }
        }

        private void ActializarObjetoSiPrototipo(string ID_Panel)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            string sSql = "UPDATE tblPanel Set Precio_Venta = 0 WHERE (Id_Panel IN (SELECT tblPlano.Plano " +
                          "FROM tblPlano INNER JOIN tblPlano_Panel ON tblPlano.Plano = tblPlano_Panel.Id_Plano " +
                          "INNER JOIN tblPanel AS tblPanel_1 ON tblPlano_Panel.Id_PanelNum = tblPanel_1.Id_Numerico " +
                          "WHERE  (tblPlano.Tipologia = 1) AND (tblPanel_1.Id_Panel = @ID_Panel)  GROUP BY tblPlano.Plano))";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@ID_Panel", ID_Panel);




                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();


                }
            }
        }


        // Copiar un modulo
        protected void btnCopiarModulo_Click(object sender, EventArgs e)
        {
            Session["Modulo"] = "Copiar";
            // Crea el script de JavaScript para abrir la nueva pestaña
            string url = "/Formularios/DiseñoYDesarrollo/Modulo.aspx";
            string script = $"window.open('{url}', '_blank');";

            // Registra el script para ejecutarlo en el lado del cliente usando ScriptManager
            ScriptManager.RegisterStartupScript(this, this.GetType(), "openTab", script, true);
        }


        // ----------------------   Empieza metodos asociados  --------------------------------------------------

        // para objetos asociados 
        protected void btnAgregarInsumo_Click(object sender, EventArgs e)
        {
            // Validar que haya un insumo seleccionado 
            if (TextIdInsumo.Text.Trim() != "")
            {
                if (!ValidarExistenciaInsumoObjeto())
                {
                    // Realizar la insercion en la tabla tblPanelInsumo 
                    if (AsociarInsumoObjeto())
                    {
                        // Se actualiza el campo en Apunta a PSL en la tabla panel 
                        ActualizarCampoAplicaPSl(1);

                        // Mostrar Mensjae de exito al Agregar  
                        span_mensaje_Exito.InnerText = "El Insumo ha sido agregado correctamente";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalInsumoAsociadoExito').modal('show');", true);
                    }
                    else
                    {
                        // No se pudo asociar el insumo 
                    }
                }
                else
                {
                    // Mensaje de no se ha seleccionado ningun insumo 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El insumo seleccionado ya se encuentra asociado');", true);
                    return;
                }

            }
            else
            {
                // Mensaje de no se ha seleccionado ningun insumo 
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No se ha seleccionado ningún insumo para asociar');", true);
                TextIdInsumo.Focus();
                return;
            }

        }

        // para objetos asociados 
        protected void btnEliminarInsumo_Click(object sender, EventArgs e)
        {
            // Validar que un elemento se haya seleccionado   
            if (TextIdInsumo.Text.Trim() != "")
            {
                // Realizar la insercion en la tabla tblPanelInsumo 
                if (EliminarInsumoAsociado())
                {
                    if (ValidarExistenciaInsumoObjeto1())
                    {
                        ActualizarCampoAplicaPSl(1);
                    }
                    else
                    {
                        ActualizarCampoAplicaPSl(0);
                    }

                    // Mostrar Mensjae de exito al eliminar insumo
                    span_mensaje_Exito.InnerText = "El Insumo ha sido eliminado correctamente";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalInsumoAsociadoExito').modal('show');", true);
                }
                else
                {
                    // No se pudo eliminar el insumo 
                }
            }
            else
            {
                // Mensaje de no se ha seleccionado ningun insumo 
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No se ha seleccionado ningún insumo para eliminar');", true);
            }


            // Realizar el delete en la tabla tblPanelInsumo con ID_Numerico y Id_Insumo 


            // Mostrar Mensaje de exito al eliminar 

        }
        
        // para objetos asociados 
        private bool AsociarInsumoObjeto()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblPanelInsumo (IdPanel,IdInsumo) " +
                              "VALUES (@IdPanel,@IdInsumo)";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@IdPanel", TextIdNum.Text);
                    cmd.Parameters.AddWithValue("@IdInsumo", TextIdInsumo.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
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

        // para objetos asociados 
        private bool EliminarInsumoAsociado()
        {
            bool exito = false;
            string connectionString = WebConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = @"DELETE FROM tblPanelInsumo WHERE IdPanel = @ID_Numerico AND IdInsumo = @Id_Insumo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar los parámetros con valores
                    command.Parameters.AddWithValue("@ID_Numerico", TextIdNum.Text);
                    command.Parameters.AddWithValue("@Id_Insumo", TextIdInsumo.Text);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            exito = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        // Console.WriteLine("Error al eliminar el registro de tblPanel_Modulo: " + ex.Message);
                        exito = false;
                    }
                }
            }

            return exito;
        }

        // para objetos asociados 
        private bool ValidarExistenciaInsumoObjeto()
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 1 FROM tblPanelInsumo WHERE IdPanel = @ID_Numerico AND IdInsumo = @Id_Insumo";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Numerico", TextIdNum.Text);
                    command.Parameters.AddWithValue("@Id_Insumo", TextIdInsumo.Text);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            existe = true;
                        }
                    }
                }
            }

            return existe;
        }

        // para objetos asociados 
        private bool ValidarExistenciaInsumoObjeto1()
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 1 FROM tblPanelInsumo WHERE IdPanel = @ID_Numerico ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Numerico", TextIdNum.Text);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            existe = true;
                        }
                    }
                }
            }

            return existe;
        }

        // para objetos asociados 
        private void ActualizarCampoAplicaPSl(int ApuntaPSL)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "UPDATE tblPanel set Apunta_Cod_PSL = @ApuntaPSl  WHERE Id_Numerico = @Id_Numerico";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {

                    cmd.Parameters.AddWithValue("@Id_Numerico", TextIdNum.Text);
                    cmd.Parameters.AddWithValue("@ApuntaPSl", ApuntaPSL);

                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();


                }
            }
        }

        // para objetos asociados 
        protected void DataGridInsumosObeto_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "InsumoAsociado")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridInsumosObeto.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 
                foreach (DataGridItem item in DataGridInsumosObeto.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";


                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);


                TextIdInsumoPrueba.Text = row.Cells[1].Text;
                TextInRelOtNoOaiPrueba.Text = row.Cells[2].Text;
                TextCodPSLPrueba.Text = row.Cells[3].Text;


                btnEliminarInsumo.Enabled = false;
                btnEliminarInsumo.CssClass = "btn btn-sm btn-outline-secondary";


            }
        }

        // para objetos asociados 
        protected void btnAceptartProcesoExitoso_Click(object sender, EventArgs e)
        {
            LoadData();
            DataGridInsumosObeto.DataBind();
            string script = @"ActivarTapInsumosAsociados();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ActivarTapInsumosAsociados", script, true);
        }
    }

}