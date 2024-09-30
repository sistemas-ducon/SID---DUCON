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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                CargarSiempre();

                string tipoAccion = Session["Modulo"] as string;
                if (tipoAccion == "Nuevo")
                {

                }
                if (tipoAccion == "Modificar")
                {
                    if (Session["IDModulo"] != null)
                    {
                        LlenarControles();
                        ViewState["OriginalFamilia"] = DropDownListGrupo.SelectedItem.Text;
                    }
                }
                if (tipoAccion == "Copiar")
                {
                    DeshabilitarElementos();
                    LlenarControles();
                }



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
        }

        protected void CargarSiempre()
        {
            textModulo.Enabled = false;
            textModulo.CssClass = "form-control form-control-sm";

            BtnGrabarInf.Enabled = false;
            BtnGrabarInf.CssClass = "btn btn-sm shadow button-disabled";

            BtnAgregarConfiguracion.Enabled = false;
            BtnAgregarConfiguracion.CssClass = "btn btn-sm shadow button-disabled";

            LinkButton2.Enabled = false;
            LinkButton2.CssClass = "btn btn-sm shadow button-disabled";

            BtnGrabarConfiguracion.Enabled = false;
            BtnGrabarConfiguracion.CssClass = "btn btn-sm shadow button-disabled";

            BtnX.Enabled = false;
            BtnX.CssClass = "btn btn-sm shadow button-disabled";

            BtnCancelarInf.Enabled = false;
            BtnCancelarInf.CssClass = "btn btn-sm shadow button-disabled";

            CargarTipoInsumo();
            CargarDropDownListGrupo();
            CargarDropDownListTipoModulo();
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
                        FROM tblInsumo 
                        INNER JOIN tblTipoInsumo ON tblInsumo.Id_TipoInsumo = tblTipoInsumo.ID_TipoInsumo
                        INNER JOIN tblUnidad_Medida ON tblInsumo.Id_unidadMedida = tblUnidad_Medida.Id_unidadMedida
                        WHERE 1=1";

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
            if (!string.IsNullOrEmpty(tipoInsumo) && tipoInsumo != "-- Seleccione --")
            {
                condiciones.Add("tblTipoInsumo.Descripcion LIKE @tipoInsumo");
                parametros.Add(new SqlParameter("@tipoInsumo", tipoInsumo + "%"));
            }

            // Agregar condiciones al WHERE
            if (condiciones.Count > 0)
            {
                consulta += " AND " + string.Join(" AND ", condiciones);
            }

            // Agregar ORDER BY
            consulta += " ORDER BY tblInsumo.Descripcion_Insumo ASC";

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
        }


        private bool ValoresCambiaron()
        {
            bool grupoCambiado = ViewState["Grupo_Original"] != null && DropDownListGrupo.SelectedValue != ViewState["Grupo_Original"].ToString();
            bool descripcionCambiada = ViewState["DescripcionModulo_Original"] != null && TextDescripcionModulo.Text.Trim() != ViewState["DescripcionModulo_Original"].ToString();
            bool tipoModuloCambiado = ViewState["TipoModulo_Original"] != null && DropDownListTipoModulo.SelectedValue != ViewState["TipoModulo_Original"].ToString();
            bool alturaModuloCambiada = ViewState["AlturaModulo_Original"] != null && TextAlturaModulo.Text.Trim() != ViewState["AlturaModulo_Original"].ToString();

            return grupoCambiado || descripcionCambiada || tipoModuloCambiado || alturaModuloCambiada;
        }


        private void EstadoGrabarCancelar()
        {
            if (ValoresCambiaron())
            {
                BtnGrabarInf.Enabled = true;
                BtnGrabarInf.CssClass = "btn btn-sm shadow button-enabled";

                BtnCancelarInf.Enabled = true;
                BtnCancelarInf.CssClass = "btn btn-sm shadow button-enabled";
            }
            else
            {
                BtnGrabarInf.Enabled = false;
                BtnGrabarInf.CssClass = "btn btn-sm shadow button-disabled";

                BtnCancelarInf.Enabled = false;
                BtnCancelarInf.CssClass = "btn btn-sm shadow button-disabled";
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
                if (opcionModulo == "Nuevo")
                {
                    Calcular_IdModulo(); // Método para calcular el Id del módulo
                    string insertQuery = @"INSERT INTO tblModulo(Id_Modulo, Descripcion_Modulo, Id_TipoModulo, Altura, ID_Familia, Responsable, FechaChequeo) 
                                   VALUES (@IdModulo, @DescripcionModulo, @IdTipoModulo, @Altura, @IDFamilia, @Responsable, @FechaChequeo)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdModulo", idModulo);
                        cmd.Parameters.AddWithValue("@DescripcionModulo", descripcionModulo);
                        cmd.Parameters.AddWithValue("@IdTipoModulo", tipoModulo);
                        cmd.Parameters.AddWithValue("@Altura", altura);
                        cmd.Parameters.AddWithValue("@IDFamilia", idFamilia);
                        cmd.Parameters.AddWithValue("@Responsable", nombreUsuario);
                        cmd.Parameters.AddWithValue("@FechaChequeo", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Verificar si el módulo tiene al menos un panel asociado
                    string countQuery = "SELECT COUNT(Id_Modulo) FROM tblPanel_Modulo WHERE Id_Modulo = @IdModulo";
                    using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                    {
                        countCmd.Parameters.AddWithValue("@IdModulo", idModulo);
                        int count = Convert.ToInt32(countCmd.ExecuteScalar());

                        if (count == 0) // Si no hay paneles asociados, se puede modificar
                        {
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

                            // Actualizar el sentido en tblModulo_insumo
                            string updateSentidoQuery = "";
                            if (tipoModulo == "3")
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
                        }
                        else
                        {
                            string contenidoModalOT = "El módulo: " + textModulo.Text + " conforma al menos un objeto, Quiere Modificar el módulo?";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal1", "$('#ModalRotacionModulo').modal('show'); $('#ModalRotacionModulo2').text('" + contenidoModalOT + "');", true);
                        }
                    }
                }

                // Recargar los datos después de la inserción o modificación
                //Requery_Modulo(idModulo);
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

        protected void DataGrid1_CancelCommand(object source, DataGridCommandEventArgs e)
        {

        }
    }
}