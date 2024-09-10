using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo
{
    public partial class frmInsumos : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                if (!IsPostBack)
                {
                    InicializarBotones();
                    CargarInsumo();
                    GuardarVistas();
                    DeshabilitarControlesExceptoCerrar(this.container);
                    DropAdiAca.DataBind();
                    DropAdiAca.Items.Insert(0, new ListItem(""));
                    desabilitarTextbox();

                    if (Session["CRUDTipoInsumo"]?.ToString() == "3")
                    {
                        // Activar Tab Plano 
                        string script = "activarPestana('TipoInsumoGrupoAcabados-tab', 'TipoInsumoGrupoAcabados-content');";
                        ClientScript.RegisterStartupScript(this.GetType(), "activarPestanaScript", script, true);
                    }
                }
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }
        }

        protected void desabilitarTextbox()
        {
            TextDescripcion.Enabled = false;
            DropAdiAca.Enabled = false;
            DropAdiAca.CssClass = "form-control form-control-sm";

            TextDescripcion2.Enabled = false;
            TextCodInv.Enabled = false;
            TextDescripcionAcabado.Enabled = false;
            CheckBoxLinea.Enabled = false;
            CheckBoxActivo.Enabled = false;
        }

        protected void GuardarVistas()
        {
            ViewState["TipoInsumo"] = dtacboTipoInsumo.SelectedValue;
            ViewState["UnidadMedida"] = DropAbreviado.SelectedValue;
            ViewState["DescripcionInsumo"] = Textbox4.Text;
            ViewState["AcabadoDesde"] = DropAcabadosDesde.SelectedValue;
            ViewState["ValorUnitario"] = Textbox6.Text;
            ViewState["ID_Inventario"] = Textbox9.Text;
            ViewState["Peso"] = Textbox12.Text;
            ViewState["UndXPaquete"] = Textbox13.Text;
            ViewState["FactorGanancia"] = Textbox10.Text;
            ViewState["FactorDesperdicio"] = Textbox11.Text;
        }

        // Método para evaluar si habilitar los botones
        protected void EvaluarEstadoBotones(string departamento)
        {
            // Comparamos los valores actuales con los valores almacenados en ViewState (equivalente a Tag en VB6)
            bool cambiosDetectados =
               
                
                (Textbox4.Text != (string)ViewState["DescripcionInsumo"]) ||
                (DropAcabadosDesde.SelectedValue != (string)ViewState["AcabadoDesde"]) ||
                (Textbox6.Text != (string)ViewState["ValorUnitario"]) ||
                (Textbox9.Text != (string)ViewState["ID_Inventario"]) ||
                (Textbox12.Text != (string)ViewState["Peso"]) ||
                (Textbox13.Text != (string)ViewState["UndXPaquete"]) ||
                (Textbox10.Text != (string)ViewState["FactorGanancia"]) ||
                (Textbox11.Text != (string)ViewState["FactorDesperdicio"]);

            // Validaciones adicionales
            bool camposValidos =
                !string.IsNullOrWhiteSpace(dtacboTipoInsumo.SelectedValue) &&
                !string.IsNullOrWhiteSpace(DropAbreviado.SelectedValue) &&
                !string.IsNullOrWhiteSpace(Textbox4.Text) &&
                !string.IsNullOrWhiteSpace(DropAcabadosDesde.SelectedValue) &&
                !string.IsNullOrWhiteSpace(Textbox6.Text) &&
                !string.IsNullOrWhiteSpace(Textbox12.Text) &&
                decimal.TryParse(Textbox6.Text, out _) &&  // Verificamos si es numérico
                decimal.TryParse(Textbox12.Text, out _);   // Verificamos si es numérico

            // Lógica basada en el departamento
            if (departamento == "Compra")
            {
                // Aquí se pueden agregar validaciones adicionales específicas para el departamento "Compra"
                BtnGrabar.Enabled = cambiosDetectados && camposValidos;
                BtnCancelar.Enabled = cambiosDetectados;
            }
            else
            {
                bool camposDepartamentoValidos = camposValidos &&
                    !string.IsNullOrWhiteSpace(Textbox10.Text) && // FactorGanancia
                    !string.IsNullOrWhiteSpace(Textbox11.Text) && // FactorDesperdicio
                    decimal.TryParse(Textbox10.Text, out _) &&
                    decimal.TryParse(Textbox11.Text, out _) &&
                    decimal.Parse(Textbox11.Text) >= 1; // Validamos que FactorDesperdicio >= 1

                BtnGrabar.Enabled = cambiosDetectados && camposDepartamentoValidos;
                BtnCancelar.Enabled = cambiosDetectados;
            }
        }

        // Eventos para detectar cambios en los controles
        protected void Control_Changed(object sender, EventArgs e)
        {
            string departamento = "Compra"; // Cambia esto según la lógica de tu aplicación
            EvaluarEstadoBotones(departamento);
        }

        private void InicializarBotones()
        {
            BtnGrabar.Enabled = false;
            BtnGrabar.CssClass = "btn linkButtonClicked2 shadow-sm text-dark btn-sm";
            BtnCancelar.Enabled = false;
            BtnCancelar.CssClass = "btn linkButtonClicked2 shadow-sm text-dark btn-sm";

            BtnAdiAca.Enabled = true;
            BtnAdiAca.CssClass = "btn button-enabled shadow-sm btn-sm mb-2";

            BtnModAca.Enabled = false;
            BtnModAca.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            BtnGraAca.Enabled = false;
            BtnGraAca.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            BtnAdiAca2.Enabled = true;
            BtnAdiAca2.CssClass = "btn button-enabled shadow-sm btn-sm";

            BtnModAca2.Enabled = false;
            BtnModAca2.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            BtnGraAca2.Enabled = false;
            BtnGraAca2.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            BtnAdiAca3.Enabled = true;
            BtnAdiAca3.CssClass = "btn button-enabled shadow-sm btn-sm";

            BtnModAca3.Enabled = false;
            BtnModAca3.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            BtnGraAca3.Enabled = false;
            BtnGraAca3.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";
        }

        protected void CargarInsumo()
        {
            if (Session["Id_Insumo"] != null)
            {
                string insumoId = Session["Id_Insumo"].ToString();

                Insumo insumo = ObtenerInsumoPorId(insumoId);

                if (insumo != null)
                {
                    AsignarDatosAControles(insumo);
                    Cargar_Uso_del_Insumo(insumoId);
                    Cargar_Historico_Actualizacion_Insumo(insumoId);
                    Cargar_Tipo_Insumo();
                    CargarGrupoAcabado();
                }

            }
        }

        private Insumo ObtenerInsumoPorId(string insumoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            Insumo insumo = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "EXEC ctaInsumo @InsumoId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InsumoId", insumoId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            insumo = new Insumo
                            {
                                Id = reader["Id_Insumo"] != DBNull.Value ? reader["Id_Insumo"].ToString() : string.Empty,
                                Descripcion = reader["Descripcion_Insumo"] != DBNull.Value ? reader["Descripcion_Insumo"].ToString() : string.Empty,
                                ValorUnitario = reader["Valor_unitario"] != DBNull.Value ? Convert.ToDecimal(reader["Valor_unitario"]) : 0,
                                TipoInsumoId = reader["Id_TipoInsumo"] != DBNull.Value ? reader["Id_TipoInsumo"].ToString() : string.Empty,
                                UnidadMedidaId = reader["Id_UnidadMedida"] != DBNull.Value ? reader["Id_UnidadMedida"].ToString() : string.Empty,
                                FactorDesperdicio = reader["Factor_Desperdicio"] != DBNull.Value ? Convert.ToDecimal(reader["Factor_Desperdicio"]) : 0,
                                FactorGanancia = reader["Factor_Ganancia"] != DBNull.Value ? Convert.ToDecimal(reader["Factor_Ganancia"]) : 0,
                                InventarioId = reader["Id_Inventario"] != DBNull.Value ? reader["Id_Inventario"].ToString() : string.Empty,
                                AplicacionAcabado = reader["AplicacionAcabado"] != DBNull.Value ? reader["AplicacionAcabado"].ToString() : string.Empty,
                                UndxPaquete = reader["UndxPaquete"] != DBNull.Value ? Convert.ToInt32(reader["UndxPaquete"]) : 0,
                                PesoKG = reader["PesoKG"] != DBNull.Value ? Convert.ToDecimal(reader["PesoKG"]) : 0,
                                FechaCreacion = reader["FechaCreacion"] != DBNull.Value ? (DateTime?)reader["FechaCreacion"] : null,
                                FechaActualizacion = reader["FechaActualizacion"] != DBNull.Value ? (DateTime?)reader["FechaActualizacion"] : null,
                                Responsable = reader["Responsable"] != DBNull.Value ? reader["Responsable"].ToString() : string.Empty
                            };
                        }
                    }
                }
            }
            return insumo;
        }

        protected void DeshabilitarControlesExceptoCerrar(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Verificar si es un control del tipo Button
                if (control is Button btn)
                {
                    // Dejar habilitado únicamente el botón 'BtnCerrar'
                    if (btn.ID == "BtnCerrar")
                    {
                        btn.Enabled = true;
                        btn.CssClass = "btn linkButtonClicked2  shadow-sm text-dark btn-sm";
                    }
                    else
                    {
                        btn.Enabled = false;
                        btn.CssClass = "btn linkButtonClicked2  shadow-sm text-dark btn-sm";
                    }
                }
                // Deshabilitar TextBox, DropDownList, etc.
                else if (control is TextBox txt)
                {
                    txt.Enabled = false;
                    txt.CssClass = "form-control form-control-sm";
                }
                else if (control is DropDownList ddl)
                {
                    ddl.Enabled = false;
                    ddl.CssClass = "form-control form-control-sm";
                }
                // Si el control tiene más hijos, iterar recursivamente
                else if (control.HasControls())
                {
                    DeshabilitarControlesExceptoCerrar(control);
                }
            }
        }


        private void AsignarDatosAControles(Insumo insumo)
        {
            textInsumo.Text = insumo.Id;
            Textbox4.Text = insumo.Descripcion;
            Textbox6.Text = insumo.ValorUnitario.ToString();
            dtacboTipoInsumo.SelectedValue = insumo.TipoInsumoId;
            DropAbreviado.SelectedValue = insumo.UnidadMedidaId;
            Textbox11.Text = insumo.FactorDesperdicio.ToString();
            Textbox10.Text = insumo.FactorGanancia.ToString();
            Textbox9.Text = insumo.InventarioId;
            DropAcabadosDesde.SelectedValue = insumo.AplicacionAcabado;
            Textbox13.Text = insumo.UndxPaquete.ToString();
            Textbox12.Text = insumo.PesoKG.ToString();
            Textbox1.Text = insumo.FechaCreacion.HasValue ? insumo.FechaCreacion.Value.ToString("yyyy-MM-dd") : string.Empty;
            Textbox2.Text = insumo.FechaActualizacion.HasValue ? insumo.FechaActualizacion.Value.ToString("yyyy-MM-dd") : string.Empty;
            Textbox3.Text = insumo.Responsable;
        }


        protected void Cargar_Uso_del_Insumo(string insumoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("cta_Modulos_del_Insumo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Insumo", insumoId);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                     System.Data.DataTable dt = new System.Data.DataTable();


                        connection.Open();
                        adapter.Fill(dt);

                        DataGridSolicitudEspecial.DataSource = dt;
                        DataGridSolicitudEspecial.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeDeError("Error al cargar los módulos del insumo: " + ex.Message);
            }
        }

        protected void CargarGrupoAcabado()
        {
            string query = "SELECT * FROM tblGrupodeAcabado ORDER BY Descripcion_Grupo ASC";

            // Asegúrate de ajustar tu cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Verificamos si hay registros y vinculamos los datos al DataGrid
                    if (reader.HasRows)
                    {
                        DataGrid3.DataSource = reader;
                        DataGrid3.DataBind();
                    }

                    reader.Close();
                }
            }
        }


        private void Cargar_Tipo_Insumo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT * FROM tblTipoInsumo ORDER BY Descripcion ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                DataGrid2.DataSource = dt;
                DataGrid2.DataBind();
            }
        }

        protected void Cargar_Historico_Actualizacion_Insumo(string insumoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Fecha_Actualizacion, Valor, Responsable FROM tblInsumo_Historico_Precios WHERE Id_Insumo = @InsumoId ORDER BY Fecha_Actualizacion DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InsumoId", insumoId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    System.Data.DataTable dt = new System.Data.DataTable();

                    connection.Open();
                    adapter.Fill(dt);  // Llenar el DataTable con los resultados de la consulta

                    // Asignar los datos al DataGrid
                    DataGrid1.DataSource = dt;
                    DataGrid1.DataBind();
                }
            }
        }


        protected void CargarAcabadosPorGrupo(string idGrupoAcabado)
        {
            string query = @"
        SELECT tblAcabado.CodInventario, tblAcabado.Descripcion_Acabado, tblAcabado.DeLinea,
               tblAcabado.Activo, ID_Acabado
        FROM tblGrupodeAcabado
        INNER JOIN tblAcabado ON tblGrupodeAcabado.ID_GrupoAcabado = tblAcabado.ID_GrupoAcabado
        WHERE tblAcabado.ID_GrupoAcabado = @ID_GrupoAcabado
        ORDER BY tblAcabado.Descripcion_Acabado ASC";

            // Asegúrate de ajustar tu cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Pasamos el parámetro
                    cmd.Parameters.AddWithValue("@ID_GrupoAcabado", idGrupoAcabado);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Verificamos si hay registros y vinculamos los datos al DataGrid4
                    if (reader.HasRows)
                    {
                        DataGrid4.DataSource = reader;
                        DataGrid4.DataBind();
                    }

                    reader.Close();
                }
            }
        }

        private void MostrarMensajeDeError(string mensaje)
        {
            
        }

        protected void DataGrid2_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridTipoInsumo")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid2.Items[rowIndex];

                // Capturamos los campos de la fila del DataGrid
                TextDescripcion.Text = (row.Cells[2].Text != "&nbsp;" && !string.IsNullOrEmpty(row.Cells[2].Text)) ? row.Cells[2].Text : string.Empty;
                string valor = row.Cells[4].Text;
                DropAdiAca.SelectedValue = (valor != "&nbsp;" && !string.IsNullOrEmpty(valor) && DropAdiAca.Items.FindByValue(valor) != null)
                    ? valor
                    : string.Empty; // Asigna un valor por defecto si es inválido


                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DataGrid2.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                Session["ID_TipoInsumo"] = row.Cells[1].Text;

                // Store the selected row index in the DataGrid attribute
                DataGrid2.Attributes["SelectedRowIndex"] = rowIndex.ToString();

                // Scroll to the row
                row.Attributes["id"] = "DataGrid2_row_" + rowIndex;
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('DataGrid2_row_" + rowIndex + "');", true);

                // Habilitar el botón Modificar
                BtnModAca.Enabled = true;
                BtnModAca.CssClass = "btn button-enabled shadow-sm text-dark btn-sm mb-2";
            }
        }

        protected void DataGrid3_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "GrupoAcabado")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid3.Items[rowIndex];

                // capturamos los campos de la fila del DataGrid
                Session["Id_GrupoAcabado"] = row.Cells[1].Text;
                string idGrupoAcabado = Session["Id_GrupoAcabado"].ToString();

                TextDescripcion2.Text = row.Cells[2].Text;

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DataGrid3.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                BtnModAca2.Enabled = true;
                BtnModAca2.CssClass = "btn button-enabled shadow-sm text-dark btn-sm";

                // Store the selected row index in the DataGrid attribute
                DataGrid3.Attributes["SelectedRowIndex"] = rowIndex.ToString();

                // Scroll to the row
                row.Attributes["id"] = "DataGrid3_row_" + rowIndex;
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('DataGrid3_row_" + rowIndex + "');", true);

                // Llamamos al método que carga el DataGrid4 con el ID seleccionado
                CargarAcabadosPorGrupo(idGrupoAcabado);
            }
        }

        protected void DataGrid4_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridAcabados")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid4.Items[rowIndex];

                // capturamos los campos de la fila del DataGrid
                TextDescripcionAcabado.Text = row.Cells[2].Text;
                TextCodInv.Text = row.Cells[1].Text;

                Session["IDAcabado"] = row.Cells[5].Text;


                bool deLinea = Convert.ToBoolean(row.Cells[3].Text.Trim()); // Convertir el valor de "DeLinea"
                bool activo = Convert.ToBoolean(row.Cells[4].Text.Trim());  // Convertir el valor de "Activo"

                // Asignar el valor a los CheckBox
                CheckBoxLinea.Checked = deLinea;
                CheckBoxActivo.Checked = activo;

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DataGrid4.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                BtnModAca3.Enabled = true;
                BtnModAca3.CssClass = "btn button-enabled shadow-sm text-dark btn-sm";

                // Store the selected row index in the DataGrid attribute
                DataGrid4.Attributes["SelectedRowIndex"] = rowIndex.ToString();

                // Scroll to the row
                row.Attributes["id"] = "DataGrid4_row_" + rowIndex;
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('DataGrid4_row_" + rowIndex + "');", true);

            }
        }

        protected void BtnAdiAca_Click(object sender, EventArgs e)
        {
            // Habilitar los TextBox y DropDownList
            TextDescripcion.Enabled = true;
            DropAdiAca.Enabled = true; // Si es DropDownList, si no es TextBox cambia a TextGrupoAcabado

            // Limpiar los campos
            TextDescripcion.Text = string.Empty;
            DropAdiAca.ClearSelection(); // Si es DropDownList, si es TextBox usa: TextGrupoAcabado.Text = string.Empty;

            // Colocar el foco en el campo de descripción
            TextDescripcion.Focus();

            // Habilitar/Deshabilitar botones
            BtnGraAca.Enabled = true;
            BtnGraAca.CssClass = "btn button-enabled shadow-sm text-dark btn-sm mb-2";

            BtnModAca.Enabled = false;
            BtnModAca.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            BtnAdiAca.Enabled = false;
            BtnAdiAca.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            Session["CRUDTipoInsumo"] = 1;
        }

        protected void BtnGraAca_Click(object sender, EventArgs e)
        {
            string descripcionTipoInsumo = TextDescripcion.Text;
            string grupoAcabadoID = DropAdiAca.SelectedValue;
            string grupoAcabado = DropAdiAca.SelectedItem.Text;

            // Validar que la descripción no esté vacía
            if (string.IsNullOrEmpty(descripcionTipoInsumo))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#LlenarDescripcion').modal('show');", true);
                return;
            }

            string query = string.Empty;
            int ID_TipoInsumo;

            // Conexión a la base de datos
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                conn.Open();

                if (Session["CRUDTipoInsumo"]?.ToString() == "1")
                {
                    // Obtener el máximo Id_TipoInsumo
                    query = "SELECT ISNULL(MAX(Id_TipoInsumo), 0) + 1 FROM tblTipoInsumo";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    ID_TipoInsumo = Convert.ToInt32(cmd.ExecuteScalar());

                    // Insertar un nuevo registro en tblTipoInsumo
                    if (string.IsNullOrEmpty(grupoAcabado))
                    {
                        query = "INSERT INTO tblTipoInsumo (Id_TipoInsumo, Descripcion) VALUES (@Id_TipoInsumo, @Descripcion)";
                    }
                    else
                    {
                        query = "INSERT INTO tblTipoInsumo (Id_TipoInsumo, Descripcion, IDGrupoAcabado, DesGrupoAcabado) " +
                                "VALUES (@Id_TipoInsumo, @Descripcion, @IDGrupoAcabado, @DesGrupoAcabado)";
                    }

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_TipoInsumo", ID_TipoInsumo);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcionTipoInsumo);
                    cmd.Parameters.AddWithValue("@IDGrupoAcabado", grupoAcabadoID);
                    cmd.Parameters.AddWithValue("@DesGrupoAcabado", grupoAcabado);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeExito').modal('show');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeError').modal('show');", true);
                    }
                }
                if (Session["CRUDTipoInsumo"]?.ToString() == "2")
                {
                    // Modificar el registro existente
                    ID_TipoInsumo = Convert.ToInt32(Session["ID_TipoInsumo"]);  // Asumimos que este valor está almacenado en la sesión

                    if (string.IsNullOrEmpty(grupoAcabado))
                    {
                        query = "UPDATE tblTipoInsumo SET Descripcion = @Descripcion WHERE Id_TipoInsumo = @Id_TipoInsumo";
                    
                    }
                    else
                    {
                        query = "UPDATE tblTipoInsumo SET Descripcion = @Descripcion, IDGrupoAcabado = @IDGrupoAcabado, " +
                                "DesGrupoAcabado = @DesGrupoAcabado WHERE Id_TipoInsumo = @Id_TipoInsumo";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_TipoInsumo", ID_TipoInsumo);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcionTipoInsumo);
                    cmd.Parameters.AddWithValue("@IDGrupoAcabado", grupoAcabadoID);
                    cmd.Parameters.AddWithValue("@DesGrupoAcabado", grupoAcabado);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeExito').modal('show');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeError').modal('show');", true);
                    }
                }
                Session["CRUDTipoInsumo"] = 3;
                Response.Redirect("frmInsumos.aspx");
            }
        }

        protected void BtnModAca_Click(object sender, EventArgs e)
        {
            // Habilitar los campos para editar
            TextDescripcion.Enabled = true;

            DropAdiAca.Enabled = true;

            // Desactivar el botón "Adicionar" mientras se modifica
            BtnAdiAca.Enabled = false;
            BtnAdiAca.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            // Desactivar el botón "Modificar" porque ya estamos en modo de modificación
            BtnModAca.Enabled = false;
            BtnModAca.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            // Habilitar el botón "Grabar" para guardar los cambios
            BtnGraAca.Enabled = true;
            BtnGraAca.CssClass = "btn button-enabled shadow-sm text-dark btn-sm mb-2";

            Session["CRUDTipoInsumo"] = 2;
        }

        protected void BtnGraAca2_Click(object sender, EventArgs e)
        {
            string descripcionGrupo = TextDescripcion2.Text;

            if (!string.IsNullOrEmpty(descripcionGrupo))
            {
                // Conexión a la base de datos
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    conn.Open();

                    string query = string.Empty;

                    if (Session["CRUDGrupoAcabado"]?.ToString() == "1")
                    {
                        query = "INSERT INTO tblGrupodeAcabado (Descripcion_Grupo) VALUES (@DescripcionGrupo)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@DescripcionGrupo", descripcionGrupo);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeExito').modal('show');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeError').modal('show');", true);
                        }
                    }

                    if (Session["CRUDGrupoAcabado"]?.ToString() == "2")
                    {
                        int idGrupoAcabado = Convert.ToInt32(Session["Id_GrupoAcabado"]);  // Se asume que el Id_GrupoAcabado está en la sesión
                        query = "UPDATE tblGrupodeAcabado SET Descripcion_Grupo = @DescripcionGrupo WHERE Id_GrupoAcabado = @IdGrupoAcabado";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@DescripcionGrupo", descripcionGrupo);
                        cmd.Parameters.AddWithValue("@IdGrupoAcabado", idGrupoAcabado);
                        cmd.ExecuteNonQuery();

                        // También se actualiza en tblTipoInsumo
                        query = "UPDATE tblTipoInsumo SET DesGrupoAcabado = @DescripcionGrupo WHERE IdGrupoAcabado = @IdGrupoAcabado";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@DescripcionGrupo", descripcionGrupo);
                        cmd.Parameters.AddWithValue("@IdGrupoAcabado", idGrupoAcabado);
                        cmd.ExecuteNonQuery();
                    }
                    Session["CRUDTipoInsumo"] = 3;
                    Response.Redirect("frmInsumos.aspx");
                }
            }
            else
            {
                // Mostrar alerta si la descripción está vacía
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('El campo: descripción tipo insumo es obligatorio.');", true);
            }
        }

        protected void BtnAdiAca2_Click(object sender, EventArgs e)
        {
            TextDescripcion2.Enabled = true;
            TextDescripcion2.Text = string.Empty;

            BtnAdiAca2.Enabled = false;
            BtnAdiAca2.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            BtnModAca2.Enabled = false;
            BtnModAca2.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            BtnGraAca2.Enabled = true;
            BtnGraAca2.CssClass = "btn button-enabled shadow-sm text-dark btn-sm";

            TextDescripcion2.Focus();

            Session["CRUDGrupoAcabado"] = 1;
        }

        protected void BtnModAca2_Click(object sender, EventArgs e)
        {

            TextDescripcion2.Enabled = true;
            TextDescripcion2.CssClass = "form-control form-control-sm";

            BtnAdiAca2.Enabled = false;
            BtnAdiAca2.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            BtnModAca.Enabled = false;
            BtnAdiAca2.CssClass = "btn button-disabled shadow-sm text-dark btn-sm mb-2";

            BtnGraAca2.Enabled = true;
            BtnGraAca2.CssClass = "btn button-enabled shadow-sm text-dark btn-sm mb-2";

            Session["CRUDGrupoAcabado"] = 2;
        }

        protected void BtnAdiAca3_Click(object sender, EventArgs e)
        {
           
                // Habilitar los campos de texto y checkbox
                TextCodInv.Enabled = true;
                TextDescripcionAcabado.Enabled = true;
                CheckBoxLinea.Enabled = true;
                CheckBoxActivo.Enabled = true;

                // Limpiar los valores de los campos de texto y checkboxes
                TextCodInv.Text = "";
                TextDescripcionAcabado.Text = "";

            // Establecer el foco en el campo txtCodInventario
            TextCodInv.Focus();

                // Habilitar el botón "Grabar"
                BtnGraAca3.Enabled = true;
            BtnGraAca3.CssClass = "btn button-enabled shadow-sm text-dark btn-sm";


            BtnAdiAca3.Enabled = false;
            BtnAdiAca3.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            BtnModAca3.Enabled = false;
            BtnModAca3.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            Session["CRUDAcabados"] = 1;

        }

        protected void BtnModAca3_Click(object sender, EventArgs e)
        {

            // Habilitar los campos de texto y checkboxes para edición
            TextCodInv.Enabled = true;
            TextDescripcionAcabado.Enabled = true;
            CheckBoxLinea.Enabled = true;
            CheckBoxActivo.Enabled = true;

                // Deshabilitar los botones "Adicionar" y "Modificar"
            BtnAdiAca3.Enabled = false;
            BtnAdiAca3.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            BtnModAca3.Enabled = false;
            BtnModAca3.CssClass = "btn button-disabled shadow-sm text-dark btn-sm";

            // Habilitar el botón "Grabar"
            BtnGraAca3.Enabled = true;
            BtnGraAca3.CssClass = "btn button-enabled shadow-sm text-dark btn-sm";

            Session["CRUDAcabados"] = 2;
        }

        protected void BtnGraAca3_Click(object sender, EventArgs e)
        {

            // Verificar si los campos requeridos no están vacíos
            if (!string.IsNullOrWhiteSpace(TextCodInv.Text) &&
                !string.IsNullOrWhiteSpace(TextDescripcionAcabado.Text) &&
                CheckBoxLinea.Checked && CheckBoxActivo.Checked)
            {
                string sSql = "";
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
                {
                    conn.Open();

                    if (Session["CRUDAcabados"]?.ToString() == "1")
                    {
                        string idGrupoAcabado = Session["Id_GrupoAcabado"].ToString();
                        // Insertar nuevo registro en la tabla tblacabado
                        sSql = "INSERT INTO tblacabado (CodInventario, Descripcion_Acabado, ID_GrupoAcabado, Delinea, Activo) " +
                               "VALUES (@CodInventario, @DescripcionAcabado, @IDGrupoAcabado, @Delinea, @Activo)";

                        using (SqlCommand cmd = new SqlCommand(sSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@CodInventario", TextCodInv.Text);
                            cmd.Parameters.AddWithValue("@DescripcionAcabado", TextDescripcionAcabado.Text);
                            cmd.Parameters.AddWithValue("@IDGrupoAcabado", idGrupoAcabado); 
                            cmd.Parameters.AddWithValue("@Delinea", CheckBoxLinea.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@Activo", CheckBoxActivo.Checked ? 1 : 0);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeExito').modal('show');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeError').modal('show');", true);
                            }
                        }
                    }

                    if (Session["CRUDAcabados"]?.ToString() == "2")
                    {
                        string idAcabado = Session["IDAcabado"].ToString();
                        // Actualizar registro existente en la tabla tblacabado
                        sSql = "UPDATE tblacabado SET CodInventario = @CodInventario, Descripcion_Acabado = @DescripcionAcabado, " +
                               "Delinea = @Delinea, Activo = @Activo WHERE ID_Acabado = @IDAcabado";

                        using (SqlCommand cmd = new SqlCommand(sSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@CodInventario", TextCodInv.Text);
                            cmd.Parameters.AddWithValue("@DescripcionAcabado", TextDescripcionAcabado.Text);
                            cmd.Parameters.AddWithValue("@Delinea", CheckBoxLinea.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@Activo", CheckBoxActivo.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@IDAcabado", idAcabado);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeExito').modal('show');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#MensajeError').modal('show');", true);
                            }
                        }
                    }
                }
                Session["CRUDTipoInsumo"] = 3;
                Response.Redirect("frmInsumos.aspx");
            }
            else
            {
               
            }
        }

    }

    // Clase modelo para Insumo
    public class Insumo
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorUnitario { get; set; }
        public string TipoInsumoId { get; set; }
        public string UnidadMedidaId { get; set; }
        public decimal FactorDesperdicio { get; set; }
        public decimal FactorGanancia { get; set; }
        public string InventarioId { get; set; }
        public string AplicacionAcabado { get; set; }
        public int UndxPaquete { get; set; }
        public decimal PesoKG { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string Responsable { get; set; }
    }


}
