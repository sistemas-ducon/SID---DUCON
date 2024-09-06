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
            if (!IsPostBack)
            {
                InicializarBotones();
                CargarInsumo();
                GuardarVistas();
                DeshabilitarControlesExceptoCerrar(this.container);
                DropAdiAca.DataBind();
                DropAdiAca.Items.Insert(0, new ListItem(" "));

            }
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
               tblAcabado.Activo
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
                DropAdiAca.Text = (row.Cells[3].Text != "&nbsp;" && !string.IsNullOrEmpty(row.Cells[3].Text)) ? row.Cells[3].Text : string.Empty;

                // Aplicamos la clase CSS a la fila seleccionada
                foreach (DataGridItem item in DataGrid2.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

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
                string idGrupoAcabado = row.Cells[1].Text;
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
                TextAcabados.Text = row.Cells[2].Text;
                TextAcabadosPequeño.Text = row.Cells[1].Text;


                bool deLinea = Convert.ToBoolean(row.Cells[3].Text.Trim()); // Convertir el valor de "DeLinea"
                bool activo = Convert.ToBoolean(row.Cells[4].Text.Trim());  // Convertir el valor de "Activo"

                // Asignar el valor a los CheckBox
                CheckBox.Checked = deLinea;
                CheckBox2.Checked = activo;

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
