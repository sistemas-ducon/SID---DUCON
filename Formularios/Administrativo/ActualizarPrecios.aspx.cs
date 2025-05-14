using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.WebControls;


namespace SISTEMA_INTEGRAL_DUCON.Formularios.Administrativo
{
    public partial class ActualizarPrecios : System.Web.UI.Page
    {
        //Inicializacion de variables para el Actualizar precio
        int totalRegistros
        {
            get { return ViewState["totalRegistros"] != null ? (int)ViewState["totalRegistros"] : 0; }
            set { ViewState["totalRegistros"] = value; }
        }

        int indiceActual
        {
            get { return ViewState["indiceActual"] != null ? (int)ViewState["indiceActual"] : 0; }
            set { ViewState["indiceActual"] = value; }
        }

        DataTable datosPanel
        {
            get { return ViewState["datosPanel"] as DataTable; }
            set { ViewState["datosPanel"] = value; }
        }

        //Cadena de conexion
        private readonly string CadenaConexionSID = "BD_SIDSQL";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["usuariologueado"] != null)
                {

                    CargarTipos();
                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }

            }
        }

        //Cargar lo datos dentro del Combobox
        private void CargarTipos()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * from tblGrupoObjeto order by Descripcion_Grupo asc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                ddlGrupo.DataSource = reader;
                ddlGrupo.DataTextField = "Descripcion_Grupo";
                ddlGrupo.DataValueField = "Id_GrupoObjeto";
                ddlGrupo.DataBind();

                // Agrega un item por defecto
                ddlGrupo.Items.Insert(0, new ListItem("Seleccione", ""));
            }
        }

        //Trae todos los Modulos a actualizar
        protected void btnActualizarPrecios_Click(object sender, EventArgs e)
        {
            string conexion = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT TOP 100 * FROM tblpanel", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                datosPanel = dt;
                totalRegistros = dt.Rows.Count;
                indiceActual = 0;
            }

            Timer1.Enabled = true;
            btnActualizarPrecios.Enabled = false;
        }

        //Actualiza el texto dentro del boton
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (indiceActual < totalRegistros)
            {
                DataRow fila = datosPanel.Rows[indiceActual];

                int idNumerico = Convert.ToInt32(fila["Id_Numerico"]);

                CalcularPrecioVentaObjeto(idNumerico); // método que tú defines

                btnActualizarPrecios.Text = $"Actualizando: {indiceActual + 1} de {totalRegistros}";
                indiceActual++;
            }
            else
            {
                Timer1.Enabled = false;
                btnActualizarPrecios.Text = "Actualizar Precios Objetos";
                btnActualizarPrecios.Enabled = true;
            }
        }

        //Hace el ajute de precios en base de datos
        private decimal CalcularPrecioVentaObjeto(int id)
        {
            decimal precioVenta = 0;
            float peso = 0;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarPrecioObjeto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_panelNumerico", id);
                    cmd.ExecuteNonQuery();
                }

                // Obtener los valores actualizados
                using (SqlCommand cmd = new SqlCommand("SELECT Precio_Venta, PesoKG FROM tblPanel WHERE Id_Numerico = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            peso = reader["PesoKG"] != DBNull.Value ? Convert.ToSingle(reader["PesoKG"]) : 0;
                            return precioVenta = (decimal)(reader["Precio_Venta"] != DBNull.Value ? Convert.ToSingle(reader["Precio_Venta"]) : 0);
                        }
                    }
                }
            }
            return precioVenta;
        }

        //Atrapa el elemento seleccionado para consultarlo y mostrarlo en la tabla
        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlGrupo.SelectedValue))
                return;
            int idGrupoObjeto = int.Parse(ddlGrupo.SelectedValue);
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            List<EstacionTrabajoDTO> estaciones = new List<EstacionTrabajoDTO>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblPanel WHERE Id_GrupoObjeto = @idGrupo ORDER BY Descripcion_Panel";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idGrupo", idGrupoObjeto);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    decimal valorActual = reader["Precio_Venta"] != DBNull.Value ? Convert.ToDecimal(reader["Precio_Venta"]) : 0;
                    decimal valorNuevo = PrecioVentaEstacionTrabajo((string)reader["Id_Panel"]); // este método deberías implementarlo
                    string razon = "";
                    string color = "white";

                    if (valorNuevo == 0)
                    {
                        razon = "En el Despiece de esta estación se encontró al menos un elemento sin despiece";
                        color = "blue";
                    }
                    else if (valorNuevo == -1)
                    {
                        razon = "No Existe el despiece de esta estación";
                        color = "red";
                    }
                    else if (valorActual < valorNuevo)
                    {
                        color = "green";
                    }

                    estaciones.Add(new EstacionTrabajoDTO
                    {
                        IdPanel = (string)reader["Id_Panel"],
                        ValorActual = valorActual,
                        ValorNuevo = valorNuevo,
                        RazonNoActualizacion = razon,
                        ColorFila = color
                    });
                }
            }

            DatagridActPrecio.DataSource = estaciones;
            DatagridActPrecio.DataBind();
        }

        //Rellena la tabla
        protected void DatagridActPrecio_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EstacionTrabajoDTO item = (EstacionTrabajoDTO)e.Item.DataItem;
                e.Item.ForeColor = ColorTranslator.FromHtml(item.ColorFila);
                btnActualizarPreciosEstacionesTrabajo.Enabled = true;
            }
        }

        //Trae los precios de las estacion de trabajo seleccionada
        public decimal PrecioVentaEstacionTrabajo(string idEstacion)
        {
            decimal precioVentaTotal = 0m;
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT 
            tblPlano_Panel.Id_Plano, 
            tblPlano_Panel.Id_PanelNum, 
            tblPanel.Id_Panel, 
            tblPanel.Ancho, 
            tblPlano_Panel.Cantidad, 
            tblPanel.Precio_Venta, 
            tblGrupoObjeto.Descripcion_Grupo,
            tblGrupoObjeto.Cotizar,
            tblPanel.Id_Numerico
        FROM tblGrupoObjeto 
        INNER JOIN (
            tblPanel 
            INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum
        ) ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto
        WHERE tblPlano_Panel.Id_Plano = @idEstacion";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idEstacion", idEstacion);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return -1; // No hay despiece

                        while (reader.Read())
                        {
                            bool cotizar = reader["Cotizar"] != DBNull.Value && (bool)reader["Cotizar"];
                            if (cotizar)
                            {
                                decimal precioObjeto = 0m;
                                decimal precioVenta = reader["Precio_Venta"] != DBNull.Value ? Convert.ToDecimal(reader["Precio_Venta"]) : 0m;
                                int cantidad = reader["Cantidad"] != DBNull.Value ? Convert.ToInt32(reader["Cantidad"]) : 0;
                                int idNumerico = reader["Id_Numerico"] != DBNull.Value ? Convert.ToInt32(reader["Id_Numerico"]) : 0;

                                if (precioVenta == 0)
                                {
                                    precioObjeto = CalcularPrecioVentaObjeto(idNumerico);
                                }
                                else
                                {
                                    precioObjeto = precioVenta;
                                }

                                if (precioObjeto == 0)
                                {
                                    return 0; // Al menos un elemento no tiene despiece válido
                                }

                                precioVentaTotal += precioObjeto * cantidad;
                            }
                        }
                    }
                }
            }

            return precioVentaTotal;
        }

        protected void btnActualizarPreciosEstacionesTrabajo_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            {
                foreach (DataGridItem row in DatagridActPrecio.Items)
                {
                    // Verifica si la fila no tiene el color de fondo rojo
                    if (row.Cells[0].ForeColor != Color.Red)
                    {
                        string idPanel = row.Cells[0].Text;
                        decimal valorNuevo = Convert.ToDecimal(row.Cells[1].Text);
                        decimal valorActual = Convert.ToDecimal(row.Cells[2].Text);

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            string sSql = "UPDATE tblPanel SET Precio_Venta = @PrecioNuevo, Precio_Anterior = @PrecioActual WHERE Id_Panel = @IdPanel";
                            SqlCommand cmd = new SqlCommand(sSql, conn);
                            cmd.Parameters.AddWithValue("@PrecioNuevo", valorNuevo);
                            cmd.Parameters.AddWithValue("@PrecioActual", valorActual);
                            cmd.Parameters.AddWithValue("@IdPanel", idPanel);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Llamar a otra función o evento si es necesario para actualizar la vista
                ddlGrupo_SelectedIndexChanged(ddlGrupo, EventArgs.Empty);
            }
        }

    }
    public class EstacionTrabajoDTO
    {
        public string IdPanel { get; set; }
        public decimal ValorActual { get; set; }
        public decimal ValorNuevo { get; set; }
        public string RazonNoActualizacion { get; set; }
        public string ColorFila { get; set; }
    }
}