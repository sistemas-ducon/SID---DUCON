using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using Org.BouncyCastle.Asn1.Cmp;
using System.Windows.Forms;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using SISTEMA_INTEGRAL_DUCON.Formularios.Login;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlTypes;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo
{
    public partial class GenerarCodigoInventario : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        private string CadenaConexionSSF = "BD_SSF";
        //public string TipoItem;
        public string TipoItem { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //CargarUnidadesMedida();
                if (Session["usuariologueado"] != null)
                {
                    CargarGrupos();
                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }

            }

            if (IsPostBack)
            {
                string eventTarget = Request["__EVENTTARGET"];
                string eventArgument = Request["__EVENTARGUMENT"];

                if (eventTarget == "tb_Buscar_Insumo_Filtro_Changed" && lbl_Codigo_Grupo.Text != "")
                {
                    string inputValue = eventArgument;
                    BuscarInsumoPorDescripcion(inputValue);

                    // Generar script para reestablecer el foco
                    //string script = $"document.getElementById('{txtCodigoGrupo.ClientID}').focus();";
                    ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "restoreFocus();", true);
                    //txtCodigoGrupo.Focus();


                }
            }
        }

        private void CargarGrupos()
        {

            // Conexión
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            //Cargar el select grupo de insumos
            string query = "SELECT *, ID_Grupo + ' - ' + Descripción_Grupo AS Grupo FROM tblGrupo WHERE Activo = 1 ORDER BY ID_Grupo ASC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    DropDownList1.Items.Clear();

                    // Agregar un ítem inicial como select y disabled
                    DropDownList1.Items.Add(new ListItem("Seleccione un grupo", ""));
                    DropDownList1.Items[0].Attributes["disabled"] = "disabled";
                    DropDownList1.Items[0].Attributes["selected"] = "selected";


                    // Recorrer resultados y agregar al DropDownList
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string valor = row["Grupo"].ToString();
                        string texto = row["Grupo"].ToString();
                        DropDownList1.Items.Add(new ListItem(texto, valor));
                    }
                }
            }

            listCodigosGenerados.Items.Clear();


        }

        protected void DropDownGrupoChange(object sender, EventArgs e)
        {
            string itemSeleccionado = DropDownList1.SelectedValue;
            string itemSeleccionado1 = DropDownList1.SelectedItem.Text;

            string codigoGrupo = itemSeleccionado.Substring(0, 2);
            string grupo = itemSeleccionado.Substring(5);

            lbl_Codigo_Grupo.Text = codigoGrupo;
            Grupo.Text = grupo;

            //llamada a funciones necesarias 

            //CargarGrupos();

            LimitarNuevoNombreInsumo();

            CargarTablaInsumosPorGrupo();

            CargarUnidadesMedida();
        }


        private void LimitarNuevoNombreInsumo()
        {
            string codigoGrupo = lbl_Codigo_Grupo.Text;

            DropDownList1.Items[0].Attributes["disabled"] = "disabled";

            //bool tieneColor = false;
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            string query = "SELECT tblGrupo_Color.ID_Grupo, tblGrupo_Color.ID_Color FROM tblGrupo_Color WHERE tblGrupo_Color.ID_Grupo LIKE @CodigoGrupo ORDER BY ID_Grupo ASC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CodigoGrupo", codigoGrupo + "%");
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            tb_Nuevo_Nombre_Insumo.MaxLength = 20;
                        }
                        else
                        {
                            tb_Nuevo_Nombre_Insumo.MaxLength = 30;
                        }

                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        Console.WriteLine("Error: " + ex.Message);

                    }
                }
            }
        }

        protected void CargarTablaInsumosPorGrupo()
        {
            string codGrupo = lbl_Codigo_Grupo.Text; // Obtener el código del grupo desde el Label
            string grupoPSL = "INV0" + codGrupo; // Construir el valor de búsqueda
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            string query = @"
            SELECT [SSF_PRUEBAS].[dbo].[in_items].* 
            FROM [SSF_PRUEBAS].[dbo].[in_items] 
            WHERE [SSF_PRUEBAS].[dbo].[in_items].itegrupo LIKE @GrupoPSL 
            ORDER BY itecodigo";

            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GrupoPSL", grupoPSL + "%");

                    try
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);

                        // Vincular datos al GridView
                        gv_Datos_Insumos_Por_Grupo.DataSource = dt;
                        gv_Datos_Insumos_Por_Grupo.DataBind();
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
                    }
                }
            }
        }


        private void BuscarInsumoPorDescripcion(string filtro) //Corregir la recarga
        {
            string codGrupo = lbl_Codigo_Grupo.Text; // Obtener el código del grupo desde el Label
            string grupoPSL = "INV0" + codGrupo;


            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Consulta SQL con filtro
            string query = @"
            SELECT [SSF_PRUEBAS].[dbo].[in_items].* 
            FROM [SSF_PRUEBAS].[dbo].[in_items] 
            WHERE [SSF_PRUEBAS].[dbo].[in_items].itegrupo LIKE @GrupoPSL
            AND [SSF_PRUEBAS].[dbo].[in_items].itedesclarg LIKE @Filtro
            ORDER BY itecodigo ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GrupoPSL", grupoPSL + "%");
                    command.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");

                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Enlazar los resultados al GridView
                        gv_Datos_Insumos_Por_Grupo.DataSource = dataTable;
                        gv_Datos_Insumos_Por_Grupo.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        private void CargarUnidadesMedida()
        {
            string codigoGrupo = lbl_Codigo_Grupo.Text;


            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;


            //Cargar el select grupo de insumos
            string query = @"
            SELECT tblUnidad_Medida.Abreviado2 AS UnidadDeMedida 
            FROM tblUnidad_Medida INNER JOIN tblGrupo_UnidadMedida ON tblUnidad_Medida.Id_UnidadMedida = tblGrupo_UnidadMedida.ID_UnidadMedida 
            WHERE tblGrupo_UnidadMedida.ID_Grupo LIKE @CodigoGrupo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CodigoGrupo", codigoGrupo + "%");

                    try
                    {

                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Limpiar el DropDownList antes de agregar nuevos elementos
                        DropDown_Unidad_Medida.Items.Clear();


                        DropDown_Unidad_Medida.Items.Add(new ListItem("Seleccione una medida", ""));
                        DropDown_Unidad_Medida.Items[0].Attributes["disabled"] = "disabled";
                        DropDown_Unidad_Medida.Items[0].Attributes["selected"] = "selected";


                        // Recorrer resultados y agregar al DropDownList
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string valor = row["UnidadDeMedida"].ToString();
                            string texto = row["UnidadDeMedida"].ToString();
                            DropDown_Unidad_Medida.Items.Add(new ListItem(texto, valor));
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

        }

        protected void ConsultarCodigosInusumos(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbl_Codigo_Grupo.Text))
            {
                MostrarErrorModal("Debe seleccionar un grupo.");
                return;
            }
            else if (string.IsNullOrEmpty(tb_Nuevo_Nombre_Insumo.Text))
            {
                MostrarErrorModal("Ingrese nombre de insumo.");
                return;
            }
            else
            {
                //lblStatus.Text = "Consultando código(s), por favor espere...";
                ConsultarColoresAsociados(lbl_Codigo_Grupo.Text, tb_Nuevo_Nombre_Insumo.Text);
            }



        }

        private void ConsultarColoresAsociados(string codGrupo, string nombreInsumo)
        {
            //string connectionString = "TuCadenaDeConexion";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            int consecutivo = 0;
            //bool existe = true;
            listCodigosGenerados.Items.Clear();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Consulta el tipo de ítem
                string tipoItemQuery = "SELECT Tipo_Item FROM tblGrupo WHERE ID_Grupo LIKE @CodGrupo";
                SqlCommand tipoItemCmd = new SqlCommand(tipoItemQuery, connection);
                tipoItemCmd.Parameters.AddWithValue("@CodGrupo", codGrupo + "%");
                var tipoItem = tipoItemCmd.ExecuteScalar();
                
                Session["TipoItem"] = tipoItem.ToString();

                // Consulta colores asociados al grupo
                string coloresQuery = "SELECT ID_Grupo, ID_Color FROM tblGrupo_Color WHERE ID_Grupo LIKE @CodGrupo";
                SqlCommand coloresCmd = new SqlCommand(coloresQuery, connection);
                coloresCmd.Parameters.AddWithValue("@CodGrupo", codGrupo + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(coloresCmd);
                DataTable dtColores = new DataTable();
                adapter.Fill(dtColores);

                // Si no hay colores asociados
                if (dtColores.Rows.Count == 0)
                {
                    GenerarCodigos(connection, codGrupo, "00", consecutivo, nombreInsumo, false);
                }
                else
                {
                    GenerarCodigos(connection, codGrupo, null, consecutivo, nombreInsumo, true);
                }
            }
        }

        private void GenerarCodigos(SqlConnection connection, string codGrupo, string codiColor, int consecutivo, string nombreInsumo, bool tieneColores)
        {

            bool existe = true;
            DataTable dtColors = new DataTable();

            while (existe)
            {
                List<string> codigosCompletosGenerados = new List<string>();
                existe = false;

                if (tieneColores)
                {
                    //Valida que dtColors este vacio para hacer la consulta
                    if (dtColors.Rows.Count == 0 || dtColors == null)
                    {
                        // Genera códigos con colores asociados
                        string colorQuery = "SELECT DISTINCT Cod_Color, Descripción_Color FROM tblGrupo_Color " +
                                            "LEFT JOIN tblColor ON tblGrupo_Color.ID_Color = tblColor.ID_Color " +
                                            "WHERE ID_Grupo LIKE @CodGrupo ";

                        SqlCommand cmd = new SqlCommand(colorQuery, connection);
                        cmd.Parameters.AddWithValue("@CodGrupo", codGrupo + "%");
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dtColors);
                    }

                    //QUE ´E LA QUE HAY
                    MostrarTodoConDataTable(dtColors);

                    foreach (DataRow row in dtColors.Rows)
                    {
                        codiColor = row["Cod_Color"].ToString();
                        string descripcionColor = row["Descripción_Color"].ToString();

                        string codigo = GenerarCodigo(codGrupo, codiColor, consecutivo);

                        string codigoCompleto = $"{codigo} - {nombreInsumo} {descripcionColor}";
                        codigosCompletosGenerados.Add(codigoCompleto);

                        //System.Diagnostics.Debug.WriteLine($"Valor de la fila: {dtColors.Rows}");
                        //System.Diagnostics.Debug.WriteLine($"Valor de yo: {row}");


                        var variableExisteCodigo = ExisteCodigo(connection, codigo);

                        if (variableExisteCodigo)
                        {
                            //AgregarCodigoGenerado(codigo, nombreInsumo, row["Descripción_Color"].ToString());

                            consecutivo++;
                            existe = true;
                        }
                    }
                    if (!existe)
                    {
                        AgregarCodigoGenerado(codigosCompletosGenerados, null);
                    }

                }
                else
                {
                    // Genera códigos sin colores asociados
                    string codigo = GenerarCodigo(codGrupo, codiColor, consecutivo);

                    string codigoGenerado = $"{codigo} - {nombreInsumo}";

                    if (!ExisteCodigo(connection, codigo))
                    {
                        AgregarCodigoGenerado(null,codigoGenerado);
                    }
                    else
                    {
                        consecutivo++;
                        existe = true;
                    }
                }
            }

            //lblStatus.Text = "Listo";
        }

        private string GenerarCodigo(string codGrupo, string codiColor, int consecutivo)
        {
            var codigo = consecutivo <= 9
                ? $"{codGrupo}{codiColor}0{consecutivo}"
                : $"{codGrupo}{codiColor}{consecutivo}";

            return codigo.Replace(" ", "");
        }

        private bool ExisteCodigo(SqlConnection connection, string codigo)
        {
            string query = "SELECT * FROM [SSF_PRUEBAS].[dbo].[in_items] WHERE itecodigo = @Codigo";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Codigo", codigo);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dtItems = new DataTable();
            adapter.Fill(dtItems);

            var resultado = cmd.ExecuteScalar() != null;

            System.Diagnostics.Debug.WriteLine($"Valor de codigo a buscar para validar: {codigo}");
            System.Diagnostics.Debug.WriteLine($"Resultado de la consulta: {resultado}");


            return resultado;
        }

        private void AgregarCodigoGenerado(List<string> codigosConColores, string codigo)
        {
            if (codigosConColores != null)
            {
                foreach (var codigoColor in codigosConColores)
                {
                    listCodigosGenerados.Items.Add(new ListItem(codigoColor));
                }
            }
            else
            {
                listCodigosGenerados.Items.Add(new ListItem(codigo));
            }
        }



        protected void BtnCrearNuevoInsumos(object sender, EventArgs e)
        {
            // Validar si hay al menos un código seleccionado
            //bool tieneSeleccionCodigo = listCodigosGenerados.Items.Cast<ListItem>().Any(item => item.Selected);
            
            // Obtener solo los códigos seleccionados
            var codigosSeleccionados = listCodigosGenerados.Items
                .Cast<ListItem>() // Convierte la colección a IEnumerable<ListItem>
                .Where(item => item.Selected) // Filtra los seleccionados
                .Select(item => item.Value) // Selecciona el valor del ítem
                .ToList(); // Convierte a lista

            // Validar si se seleccionó una unidad de medida
            bool tieneUnidadMedida = !string.IsNullOrEmpty(DropDown_Unidad_Medida.SelectedValue) && DropDown_Unidad_Medida.SelectedIndex > 0;
            

            if (!codigosSeleccionados.Any() && !tieneUnidadMedida)
            {
                MostrarErrorModal("Debe seleccionar al menos un código y una unidad de medida.");
                return;
            }

            if (!codigosSeleccionados.Any())
            {
                MostrarErrorModal("Debe seleccionar al menos un código.");
                return;
            }

            if (!tieneUnidadMedida)
            {
                MostrarErrorModal("Debe seleccionar una unidad de medida.");
                return;
            }


            CrearNuevosInsumos(codigosSeleccionados);

        }

        private void CrearNuevosInsumos(List<string> insumosSeleccionados)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSSF].ConnectionString;

            string nombreUsuario = Session["usuariologueado"].ToString();
            string receptormail = string.Empty;
            string asuntoMail = string.Empty;
            string descripcionMail = string.Empty;
            string enviadoA = string.Empty;

            string unidadMedida = DropDown_Unidad_Medida.Text;
            string tipoItem = Session["TipoItem"].ToString();

            string correoUsuario = ObtenerCorreoUsuario(nombreUsuario);

            // Obtener correos de destinatarios
            enviadoA = ObtenerDestinatarios("mailGenerarCodigoInv");

            // Validar y construir cadena de correos
            receptormail = ConstruirCadenaDeCorreos(enviadoA, correoUsuario);

            foreach (var item in insumosSeleccionados)
            {
                string codigo = item.ToString().Substring(0, 6); // Extraer código
                string descripcion = item.ToString().Substring(9); // Extraer descripción

                // Llamar a función para agregar el ítem
                Agregar_Items_PSL(codigo, descripcion,unidadMedida,tipoItem,connectionString);


                // Construir asunto y descripción del correo
                asuntoMail = $"Nuevo código de inventario: {item}";
                descripcionMail = $@"
                        Fecha: {DateTime.Now.ToString("dd/MM/yyyy hh:mm tt")}<br>
                        Señores DUCON SAS<br><br>Departamento de Almacén<br><br>
                        Se informa que el usuario: {nombreUsuario}, acaba de crear el ítem de inventario: {item}.<br>
                        Favor verificar su información y configuración.";


                // Enviar correo
                //EnviarCorreo(receptormail, asuntoMail, descripcionMail, nombreUsuario);
            }
            lblStatus.Text = insumosSeleccionados.Count > 1 ? "Insumos creados satisfatoriamente" : "Insumo creado satisfatoriamente";


        }

        // Métodos auxiliares

        private string ObtenerCorreoUsuario(string nombreUsuario)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string correoUsuario = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string consulta = "SELECT Mail FROM tblEmpleado WHERE CONCAT(Nombre, ' ', Apellidos) = @nombreUsuario";
                SqlCommand cmd = new SqlCommand(consulta, connection);
                cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);


                // Ejecutar la consulta y leer el resultado
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // Si hay resultados
                    {
                        correoUsuario = reader["Mail"].ToString(); // Obtén el valor de la columna 'Mail'
                    }
                }
            }

            return correoUsuario;
        }

        private string ObtenerDestinatarios(string objetivoMail)
        {
            // Lógica para obtener correos de destinatarios desde la base de datos
            string destinatarios = string.Empty;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT mail FROM tblUsosVarios WHERE ObjetivoMail = @ObjetivoMail";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ObjetivoMail", objetivoMail);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    destinatarios = reader["mail"].ToString();
                }
            }

            return destinatarios;
        }

        private string ConstruirCadenaDeCorreos(string destinatarios, string mailUsuario)
        {
            var listaCorreos = new List<string>();

            foreach (var correo in destinatarios.Split(';'))
            {
                if (Validar_CadenaMail(correo))
                {
                    listaCorreos.Add(correo);
                }
            }

            if (Validar_CadenaMail(mailUsuario))
            {
                listaCorreos.Insert(0, mailUsuario);
            }

            //return string.Join(";", listaCorreos);

            return "escuderocristian65@gmail.com;harleyvidal@ducon.com.co";
        }

        private void EnviarCorreo(string receptores, string asunto, string descripcion, string usuario)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Lógica para ejecutar el procedimiento almacenado
            string query = "EXEC duc_sp_correo @Receptores, @Asunto, @Descripcion, '', @Usuario";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Receptores", receptores);
                command.Parameters.AddWithValue("@Asunto", asunto);
                command.Parameters.AddWithValue("@Descripcion", descripcion);
                command.Parameters.AddWithValue("@Usuario", usuario);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private bool Validar_CadenaMail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void Agregar_Items_PSL(string codigoNuevo, string nombreItem, string UMP, string tipoItem, string connectionString)
        {
            // Lógica para agregar ítems

            string grupoPSL = $"INV0{lbl_Codigo_Grupo.Text}"; // Asignar el valor basado en Cod_Grupo
            double identificador;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Configurar el comando para el procedimiento almacenado sp_registrar_proceso
                    using (SqlCommand cmd = new SqlCommand("sp_registrar_proceso", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Añadir los parámetros esperados por el procedimiento almacenado
                        cmd.Parameters.AddWithValue("@as_cia" , "01");
                        cmd.Parameters.AddWithValue("@as_usuario", "PCADMIN");
                        cmd.Parameters.AddWithValue("@as_parametros", "NULL");
                        cmd.Parameters.AddWithValue("@as_origen", "ITE");

                        // Añadir un parámetro de salida para el identificador
                        SqlParameter outputParam = new SqlParameter("@an_identificador", SqlDbType.Decimal)
                        {
                            Direction = ParameterDirection.Output,
                            Precision = 10,
                            Scale = 0
                        };
                        cmd.Parameters.Add(outputParam);

                        // Crear el parámetro de salida para el mensaje de error
                        SqlParameter outputParam2 = new SqlParameter("@as_mensajeerror", SqlDbType.VarChar,4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam2);


                        cmd.ExecuteNonQuery();
                        identificador = Convert.ToDouble(outputParam.Value); // Obtener el valor del parámetro de salida
                    }

                    // Insertar el ítem en la tabla in_imporitems
                    string insertQuery = @"
                        INSERT INTO in_imporitems (impcontrol, impcodigoitem, impcompania, impdivision, impdesccort, impdesclarg, impgrupo, impump, impvendible, impconsdire, impproducido, impestado, impidentificador, imptipoitem, impdescxl)
                        VALUES ('IMPA', @codigoNuevo, '01', '01', @nombreItem, @nombreItem, @grupoPSL, @UMP, 'N', 'NU', 'N', 'AC', @identificador, @TipoItem, @nombreItem)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@codigoNuevo", codigoNuevo);
                        insertCmd.Parameters.AddWithValue("@nombreItem", nombreItem);
                        insertCmd.Parameters.AddWithValue("@grupoPSL", grupoPSL);
                        insertCmd.Parameters.AddWithValue("@UMP", UMP); // Variable UMP debe estar definida
                        insertCmd.Parameters.AddWithValue("@identificador", identificador);
                        insertCmd.Parameters.AddWithValue("@TipoItem", tipoItem); // Variable Tipo_Item debe estar definida

                        insertCmd.ExecuteNonQuery();
                    }

                    // Ejecutar el procedimiento almacenado sp_procesar_importacion
                    using (SqlCommand processCmd = new SqlCommand("sp_procesar_importacion", connection))
                    {
                        processCmd.CommandType = CommandType.StoredProcedure;

                        processCmd.Parameters.AddWithValue("@as_cia", "01");

                        // Parámetro de entrada/salida @an_identificador
                        SqlParameter outputIdentificador = new SqlParameter("@an_identificador", SqlDbType.Decimal)
                        {
                            Direction = ParameterDirection.InputOutput,
                            Precision = 10,
                            Scale = 0,
                            Value = identificador // Pasar el valor actual como entrada
                        };
                        processCmd.Parameters.Add(outputIdentificador);

                        // Parámetro de salida @as_mensajeerror
                        SqlParameter outputMensajeError = new SqlParameter("@as_mensajeerror", SqlDbType.VarChar, 4000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        processCmd.Parameters.Add(outputMensajeError);

                        // Ejecutar el procedimiento almacenado
                        processCmd.ExecuteNonQuery();

                        // Capturar los valores de salida
                        decimal resultadoIdentificador = (decimal)outputIdentificador.Value;
                        string mensajeError = outputMensajeError.Value.ToString();


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    string verboseError = GetVerboseError(ex);
                    System.Diagnostics.Debug.WriteLine($"Valor del error {verboseError}");
                    System.Diagnostics.Debug.WriteLine($"Valor del error {ex.Message}");

                    throw; // Lanza la excepción para manejarla externamente si es necesario
                }
            }
        }



        static string GetVerboseError(Exception ex)
        {
            // Construir el mensaje verboso del error
            System.Text.StringBuilder errorDetails = new System.Text.StringBuilder();

            errorDetails.AppendLine("===== ERROR DETALLADO =====");
            errorDetails.AppendLine($"Mensaje: {ex.Message}");
            errorDetails.AppendLine($"Tipo: {ex.GetType()}");
            errorDetails.AppendLine($"Origen: {ex.Source}");
            errorDetails.AppendLine($"Método: {ex.TargetSite}");
            errorDetails.AppendLine("Pila de llamadas:");
            errorDetails.AppendLine(ex.StackTrace);

            // Incluir excepciones internas, si las hay
            if (ex.InnerException != null)
            {
                errorDetails.AppendLine("----- EXCEPCIÓN INTERNA -----");
                errorDetails.AppendLine($"Mensaje: {ex.InnerException.Message}");
                errorDetails.AppendLine($"Tipo: {ex.InnerException.GetType()}");
                errorDetails.AppendLine($"Origen: {ex.InnerException.Source}");
                errorDetails.AppendLine($"Pila de llamadas:");
                errorDetails.AppendLine(ex.InnerException.StackTrace);
            }

            // Agregar datos adicionales (si existen)
            if (ex.Data.Count > 0)
            {
                errorDetails.AppendLine("----- DATOS ADICIONALES -----");
                foreach (var key in ex.Data.Keys)
                {
                    errorDetails.AppendLine($"{key}: {ex.Data[key]}");
                }
            }

            errorDetails.AppendLine("=============================");

            return errorDetails.ToString();
        }





        private void MostrarErrorModal(string mensaje)
        {
            lblModalErrorMessage.Text = mensaje;

            ScriptManager.RegisterStartupScript(this, GetType(),
                "ShowModal", "var modal = new bootstrap.Modal(document.getElementById('Generar_Codigos_Inventario_Modal_Error')); modal.show();", true);

        }


        public void MostrarTodoConDataTable(DataTable dataTable)
        {

            try
            {

                // Mostrar nombres de las columnas
                foreach (DataColumn column in dataTable.Columns)
                {
                    System.Diagnostics.Debug.Write($"{column.ColumnName}\t");
                }
                System.Diagnostics.Debug.WriteLine("");

                // Mostrar cada fila
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        System.Diagnostics.Debug.Write($"{item}\t");
                    }
                    System.Diagnostics.Debug.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }



    }
}