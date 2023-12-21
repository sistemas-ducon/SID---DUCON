using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class Plano1 : System.Web.UI.Page
    {
        private List<TextBox> listaTextBoxes;
        private List<DropDownList> listaDropDownLists;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuariologueado"] != null)
            {
                listaTextBoxes = new List<TextBox>
                {
                    tbPlano,tbPor,tbFecha,tbCliente,tbArea,tbContacto,tbBolsa

                };
                listaDropDownLists = new List<DropDownList>
                {
                    ddlAsesor,ddlHistorial

                };

                if (!IsPostBack)
                {
                    ControlInicialLinkButtons();
                    ControlInicialButtons();
                    DeshabilitarTextBoxes(listaTextBoxes);
                    DeshabilitarDropDownLists(listaDropDownLists);
                    DeshabilitarCheckBoxes();
                    CargarAsesoresEnDropDownList();
                    Session["ClickCount"] = 0;
                    Session["Id_Plano1"] = "";
                }

            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }


        }

        // Controles Botones, Texbox, Dropdownlist, CheckBo...
        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial  order by Nombre";

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
            ddlAsesor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione", "0"));
        }

        protected void ControlInicialLinkButtons()
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {
                NuevoPlano,
                GurdarPlano,
                ModificarPlano,
                ELiminarPlano,
                BuscarPlano,
                Cancelar,
                Bloqueado


            };

            string cssClass = "btn btn-sm shadow button-disabled";
            string cssClassEnabled = "btn btn-sm shadow button-enabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;

                    if (linkButton.ID == "NuevoPlano" || linkButton.ID == "BuscarPlano" || linkButton.ID == "Cancelar")
                    {
                        linkButton.Enabled = true;
                        linkButton.CssClass = cssClassEnabled;
                    }
                    else
                    {
                        linkButton.Enabled = false;
                        linkButton.CssClass = cssClass;
                    }

                }
            }
        }

        protected void ControlBotonesNuevoPlano()
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {
                NuevoPlano,
                GurdarPlano,
                ModificarPlano,
                ELiminarPlano,
                BuscarPlano,
                Cancelar,
                Bloqueado


            };

            string cssClass = "btn btn-sm shadow button-disabled";
            string cssClassEnabled = "btn btn-sm shadow button-enabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;

                    if (linkButton.ID == "GurdarPlano" || linkButton.ID == "BuscarPlano" || linkButton.ID == "Cancelar")
                    {
                        linkButton.Enabled = true;
                        linkButton.CssClass = cssClassEnabled;
                    }
                    else
                    {
                        linkButton.Enabled = false;
                        linkButton.CssClass = cssClass;
                    }

                }
            }
        }

        protected void ControlBotonesModificarPlanoDataGrid()
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {
                NuevoPlano,
                GurdarPlano,
                ModificarPlano,
                ELiminarPlano,
                BuscarPlano,
                Cancelar,
                Bloqueado


            };

            string cssClass = "btn btn-sm shadow button-disabled";
            string cssClassEnabled = "btn btn-sm shadow button-enabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;

                    if (linkButton.ID == "GurdarPlano")
                    {
                        linkButton.Enabled = false;
                        linkButton.CssClass = cssClass;
                    }
                    else
                    {
                        linkButton.Enabled = true;
                        linkButton.CssClass = cssClassEnabled;
                    }

                }
            }
        }

        protected void ControlBotonesModificarPlano()
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {
                NuevoPlano,
                GurdarPlano,
                ModificarPlano,
                ELiminarPlano,
                BuscarPlano,
                Cancelar,
                Bloqueado


            };

            string cssClass = "btn btn-sm shadow button-disabled";
            string cssClassEnabled = "btn btn-sm shadow button-enabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;

                    if (linkButton.ID == "GurdarPlano" || linkButton.ID == "Cancelar")
                    {
                        linkButton.Enabled = true;
                        linkButton.CssClass = cssClassEnabled;
                    }
                    else
                    {
                        linkButton.Enabled = false;
                        linkButton.CssClass = cssClass;
                    }

                }
            }
        }

        protected void ControlInicialButtons()
        {
            btnCargarPlano.Enabled = false;
            btnCargarPlano.CssClass = "btn btn-sm btn-outline-secondary";

            btnAsignar.Enabled = false;
            btnAsignar.CssClass = "btn btn-sm btn-outline-secondary";
        }

        public void DeshabilitarTextBoxes(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Enabled = false;
                textBox.CssClass = "form-control ";
            }
        }

        public void HabilitarTextBoxesNuevoPlano(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox == tbCliente || textBox == tbPor || textBox == tbContacto)
                {
                    textBox.Enabled = false;
                }
                else
                {
                    textBox.Enabled = true;
                }


            }
        }

        public void limpiarCampos(List<TextBox> textBoxes, List<DropDownList> listaDropDownLists)
        {
            foreach (TextBox textBox in textBoxes)
            {
                textBox.Text = "";

            }

            foreach (DropDownList dropDownList in listaDropDownLists)
            {
                dropDownList.ClearSelection();
            }


        }

        public void DeshabilitarDropDownLists(List<DropDownList> dropDownLists)
        {
            foreach (DropDownList dropDownList in dropDownLists)
            {
                dropDownList.Enabled = false;
                dropDownList.CssClass = "form-control";
            }
        }

        public void HabilitarDropDownListsNuevoPlano(List<DropDownList> dropDownLists)
        {
            foreach (DropDownList dropDownList in dropDownLists)
            {
                dropDownList.Enabled = true;

            }
        }

        public void DeshabilitarCheckBoxes()
        {
            chxTipologia.Enabled = false;
            chxHistorial.Enabled = false;
            chxAfecta.Enabled = false;



        }

        public void HabilitarCheckBoxesModificarNuevo()
        {
            chxTipologia.Enabled = true;
            chxHistorial.Enabled = true;

        }

        // Funcionalidades y metodos pagina plano1 
        protected void DataGridPlano1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerPlano1")
            {


                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridPlano1.Items[rowIndex];

                // Deshabilitamos los campos activos 
                tbPlano.Enabled = false;
                tbArea.Enabled = false;
                tbBolsa.Enabled = false;


                // Cargamos los Campos en los textBox 

                tbPlano.Text = row.Cells[1].Text;
                tbPor.Text = row.Cells[11].Text;
                foreach (System.Web.UI.WebControls.ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == row.Cells[9].Text)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true;
                        break;
                    }
                }
                string fechaCelda = row.Cells[10].Text;
                if (!string.IsNullOrEmpty(fechaCelda) && fechaCelda != "&nbsp;")
                {
                    tbFecha.Text = Convert.ToDateTime(fechaCelda).ToString("yyyy-MM-dd");
                }
                else
                {

                    tbFecha.Text = string.Empty;
                }
                tbArea.Text = row.Cells[7].Text;
                chxTipologia.Checked = Convert.ToBoolean(row.Cells[12].Text);



                // habilitamos los botones necesarios 


                ModificarPlano.Enabled = true;
                ModificarPlano.CssClass = "btn btn-sm shadow button-enabled";

                Bloqueado.Enabled = true;
                Bloqueado.CssClass = "btn btn-sm shadow button-enabled";

                ELiminarPlano.Enabled = true;
                ELiminarPlano.CssClass = "btn btn-sm shadow button-enabled";

                btnCargarPlano.Enabled = true;

                Session["Id_OT2"] = row.Cells[4].Text;              
                Session["Id_Plano"] = row.Cells[1].Text;
            

                ControlBotonesModificarPlanoDataGrid();


                // Se compara si el click es en la misma fila con el id del plano 
                if (row.Cells[1].Text == Session["Id_Plano1"].ToString())
                {
                    // Incrementar la variable de sesión "ClickCount" en el servidor
                    int clickCount = Convert.ToInt32(Session["ClickCount"]) + 1;
                    Session["ClickCount"] = clickCount;

                    // se valida si es el segundo click en la misma fila 
                    if (clickCount == 2)
                    {
                        string script = "<script>enviarFormulario();</script>";
                        ScriptManager.RegisterStartupScript(this, GetType(), "enviarFormulario", script, false);

                        // Reiniciar la variable de sesión "ClickCount" a 0 para la próxima interacción
                        Session["ClickCount"] = 0;
                    }
 
                }
                else
                {
                    // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                    Session["ClickCount"] = 1;

                    Session["pedido2"] = row.Cells[5].Text;
                    Session["Id_Plano1"] = row.Cells[1].Text;
                }



            }
        }

        protected void BuscarPlano_Click(object sender, EventArgs e)
        {
            // Se habilitar el Texbox de busqueda de Plano 
            tbPlano.Enabled = true;

        }

        protected void tbPlano_TextChanged(object sender, EventArgs e)
        {
            DataGridPlano1.DataSourceID = "CargarPlano";
            DataGridPlano1.DataBind();
        }

        private bool UsuarioTienePermiso()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = '" + Session["CedulaLogeada"].ToString() + "' AND ID_Permiso = 43";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // La consulta devolvió registros, lo cual significa que el usuario tiene permisos
                        return true;
                    }
                    else
                    {
                        // La consulta no devolvió registros, lo cual significa que el usuario no tiene permisos se valida si el es dueño del plano
                        if (tbPor.Text == Session["usuariologueado"].ToString())
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
        }
        private void ActualizarEstadoBloqueado(int nuevoEstado)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSql = "UPDATE tblPlano SET Bloqueado = @NuevoEstado WHERE Plano = @Plano";
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    cmd.Parameters.AddWithValue("@Plano", Session["Id_Plano1"].ToString());
                    cmd.ExecuteNonQuery();
                }
            }
        }



        //Funcionalidades de menu Botones Pantalla Plano1 

        protected void NuevoPlano_Click(object sender, EventArgs e)
        {
            HabilitarTextBoxesNuevoPlano(listaTextBoxes);
            tbPor.Text = Session["usuariologueado"].ToString();
            tbPlano.Text = "";


            HabilitarDropDownListsNuevoPlano(listaDropDownLists);
            tbFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");


            //Cambiar varible se sesion para insertar o actualizar 
            Session["InsertarActulizarPlano1"] = "Insertar";

            ControlBotonesNuevoPlano();
            HabilitarCheckBoxesModificarNuevo();


        }

        protected void GuardarModifcarPlano(Object sender, EventArgs e)
        {
            // Realizar la validacion si es guardar o acualizar un plano
            string GuardarPlano = Session["InsertarActulizarPlano1"] as string;
            if (GuardarPlano == "Insertar")
            {

                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sSql = "SELECT * FROM tblPlano WHERE Plano ='" + tbPlano.Text.Trim() + "'";
                    SqlCommand command = new SqlCommand(sSql, connection);
                    SqlDataReader reader = null;

                    try
                    {
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            // Se valida si el cliente ya existe  y se muestra un mensaje 
                            string mensaje = "El Plano  " + tbPlano.Text.Trim() + " ya existe";
                            string script = "alert('" + mensaje + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", script, true);
                            return;
                        }

                        reader.Close();



                        string sSqlInsert = "INSERT INTO tblPlano(Plano,Nombre_Cliente,Contacto_Cliente,Fecha_Entrega_Bitacora,Fecha_Termino_Diseño,area, Historial,AsesorComercial,RealizadoPor,Bolsa,AfectaBolsa,Tipologia) " +
                     "VALUES (@Plano, @NombreCliente, @ContactoCliente, @FechaEntregaBitacora, GETDATE(), @Area, @Historial, @AsesorComercial, @RealizadoPor, @Bolsa, 0, @Tipologia)";

                        SqlCommand commandInsert = new SqlCommand(sSqlInsert, connection);
                        commandInsert.Parameters.AddWithValue("@Plano", tbPlano.Text.Trim().ToUpper());
                        commandInsert.Parameters.AddWithValue("@NombreCliente", tbCliente.Text.Trim());
                        commandInsert.Parameters.AddWithValue("@ContactoCliente", tbContacto.Text.Trim());
                        commandInsert.Parameters.AddWithValue("@FechaEntregaBitacora", tbFecha.Text);
                        commandInsert.Parameters.AddWithValue("@Area", tbArea.Text.Trim().ToUpper());
                        commandInsert.Parameters.AddWithValue("@Historial", chxHistorial.Checked);
                        commandInsert.Parameters.AddWithValue("@AsesorComercial", ddlAsesor.Text);
                        commandInsert.Parameters.AddWithValue("@RealizadoPor", tbPor.Text.Trim());
                        commandInsert.Parameters.AddWithValue("@Bolsa", tbBolsa.Text.Trim());
                        commandInsert.Parameters.AddWithValue("@Tipologia", chxTipologia.Checked);

                        try
                        {

                            commandInsert.ExecuteNonQuery();
                            // Mensaje de éxito
                            string mensajeExito = "El plano " + tbPlano.Text.Trim() + " ha sido agregado exitosamente.";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                        }
                        catch (SqlException ex)
                        {
                            // Manejo de errores específicos de SQL
                            string mensajeError = "Error de SQL al ejecutar la consulta: " + ex.Message;
                            string scriptError = "alert('" + mensajeError + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);
                        }



                    }
                    catch (Exception ex)
                    {
                        string mensajeError = "Error al ejecutar la consulta: " + ex.Message;
                        string scriptError = "alert('" + mensajeError + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);

                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }


                }

            }
            else if (GuardarPlano == "Actualizar")
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sSql = "SELECT tblPlano.*, tblPlano.Plano, tblOT.Terminado_Diseño " +
                        "FROM tblOT " +
                        "RIGHT JOIN tblPlano ON tblOT.Consecutivo_Pedido = tblPlano.COnsecutivo_Pedido AND tblOT.Id_OT = tblPlano.Id_OT " +
                        "Where tblPlano.Plano = ' " + tbPlano.Text + "' And tblOT.Terminado_Diseño = 1";
                    SqlCommand command = new SqlCommand(sSql, connection);
                    SqlDataReader reader = null;

                    try
                    {
                        reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            // Se valida si el cliente ya existe  y se muestra un mensaje 
                            string mensaje = "El Plano  " + tbPlano.Text.Trim() + " está vinculado a un pedido aprobado para producción o afecta a una bolsa. No se puede modificar.";
                            string script = "alert('" + mensaje + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", script, true);
                            return;
                        }

                        reader.Close();



                        string sSqlInsert = "Update tblPlano  set Plano = @Plano, Nombre_Cliente = @NombreCliente,Fecha_Entrega_Bitacora = @FechaEntregaBitacora," +
                            " Fecha_Termino_Diseño = GETDATE() ,area = @Area, Historial = @Historial,AsesorComercial= @Asesor, RealizadoPor= @RealizadoPor, Bolsa = @Bolsa,Tipologia = @Tipologia " +
                            "Where Plano = @Plano1  ";

                        SqlCommand commandInsert = new SqlCommand(sSqlInsert, connection);
                        commandInsert.Parameters.AddWithValue("@Plano", tbPlano.Text.Trim().ToUpper());
                        commandInsert.Parameters.AddWithValue("@NombreCliente", tbCliente.Text.Trim());
                        commandInsert.Parameters.AddWithValue("@FechaEntregaBitacora", tbFecha.Text);
                        commandInsert.Parameters.AddWithValue("@Area", tbArea.Text.Trim().ToUpper());
                        commandInsert.Parameters.AddWithValue("@Historial", chxHistorial.Checked);
                        commandInsert.Parameters.AddWithValue("@Asesor", ddlAsesor.SelectedItem.Text);
                        commandInsert.Parameters.AddWithValue("@RealizadoPor", tbPor.Text.Trim());
                        commandInsert.Parameters.AddWithValue("@Bolsa", tbBolsa.Text.Trim());
                        commandInsert.Parameters.AddWithValue("@Tipologia", chxTipologia.Checked);
                        commandInsert.Parameters.AddWithValue("@Plano1", Session["Id_Plano1"].ToString());

                        try
                        {

                            commandInsert.ExecuteNonQuery();
                            // Mensaje de éxito
                            string mensajeExito = "El plano " + tbPlano.Text.Trim() + " ha sido modificado exitosamente.";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                        }
                        catch (SqlException ex)
                        {
                            // Manejo de errores específicos de SQL
                            string mensajeError = "Error de SQL al ejecutar la consulta: " + ex.Message;
                            string scriptError = "alert('" + mensajeError + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);
                        }



                    }
                    catch (Exception ex)
                    {
                        string mensajeError = "Error al ejecutar la consulta: " + ex.Message;
                        string scriptError = "alert('" + mensajeError + "');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);

                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }


                }




                // Validar si el plano esta vinculado a un pedido que esta aprobado para producccion 

                // Validar si el plano esta bloqueado 


                // Realizar la actualizacion 

            }



        }

        protected void ModificarPlano_Click(object sender, EventArgs e)
        {
            // se debe validar si el plano esta bloqueado 

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSql = "SELECT Bloqueado FROM tblPlano WHERE Plano ='" + tbPlano.Text.Trim() + "'";
                SqlCommand command = new SqlCommand(sSql, connection);
                SqlDataReader reader = null;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    bool bloqueado = reader.GetBoolean(reader.GetOrdinal("Bloqueado"));

                    // Ahora, puedes realizar acciones dependiendo de si el campo Bloqueado es verdadero o falso
                    if (bloqueado && tbPor.Text != Session["usuariologueado"].ToString())
                    {
                        string scriptNoPermiso = "alert('El plano está bloqueado. Solo el creador tiene permisos para modificarlo o desbloquearlo.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                    }
                    else
                    {
                        if (tbPor.Text == Session["usuariologueado"].ToString())
                        {
                            HabilitarTextBoxesNuevoPlano(listaTextBoxes);
                            HabilitarDropDownListsNuevoPlano(listaDropDownLists);
                            HabilitarCheckBoxesModificarNuevo();
                            ControlBotonesModificarPlano();
                        }

                    }


                }

                reader.Close();

            }

            //Cambiar varible se sesion para insertar o actualizar 
            Session["InsertarActulizarPlano1"] = "Actualizar";

        }

        protected void Bloqueado_Click(object sender, EventArgs e)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSql = "SELECT Bloqueado FROM tblPlano WHERE Plano ='" + tbPlano.Text.Trim() + "'";
                SqlCommand command = new SqlCommand(sSql, connection);
                SqlDataReader reader = null;

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    bool bloqueado = reader.GetBoolean(reader.GetOrdinal("Bloqueado"));

                    // Ahora, puedes realizar acciones dependiendo de si el campo Bloqueado es verdadero o falso
                    if (bloqueado)
                    {
                        // Validar si el usuario tiene el permiso para bloquear o desbloquear plano o si es el dueño 
                        if (UsuarioTienePermiso())
                        {
                            // Cambiar en campo bloqueado en la base de datos a 0
                            ActualizarEstadoBloqueado(0);
                            string scriptNoPermiso = "alert('El plano ha sido desbloqueado');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }
                        else
                        {
                            // El usuario no tiene permisos para realizar la acción
                            string scriptNoPermiso = "alert('No tienes permisos para desbloquear o bloquear el plano.');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }



                    }
                    else
                    {

                        // Validar si el usuario tiene el permiso para bloquear o desbloquear plano o si es el dueño 
                        if (UsuarioTienePermiso())
                        {
                            // Cambiar en campo bloqueado en la base de datos a 1
                            ActualizarEstadoBloqueado(1);
                            string scriptNoPermiso = "alert('El plano ha sido bloqueado');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }
                        else
                        {
                            // El usuario no tiene permisos para realizar la acción
                            string scriptNoPermiso = "alert('No tienes permisos para desbloquear o bloquear el plano.');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }




                    }


                }

                reader.Close();

            }
        }

        protected void BuscarPlano_Boton(object sender, EventArgs e)
        {
            DataGridPlano1.DataSourceID = "CargarPlano";
            DataGridPlano1.DataBind();

            ControlInicialLinkButtons();
            ControlInicialButtons();
            DeshabilitarTextBoxes(listaTextBoxes);
            DeshabilitarDropDownLists(listaDropDownLists);
            DeshabilitarCheckBoxes();


        }

        protected void EliminarPlano_Click(Object sender, EventArgs e)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el plano está bloqueado
                string sSqlBloqueado = "SELECT Bloqueado FROM tblPlano WHERE Plano = @plano";
                SqlCommand commandBloqueado = new SqlCommand(sSqlBloqueado, connection);
                commandBloqueado.Parameters.AddWithValue("@plano", tbPlano.Text.Trim());

                bool planoBloqueado = (bool)commandBloqueado.ExecuteScalar();

                if (planoBloqueado)
                {
                    // El plano está bloqueado y no se puede eliminar
                    string scriptNoPermiso = "alert('El plano está bloqueado y no puede ser eliminado.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                }
                else
                {
                    // El plano está desbloqueado, realizar validación adicional

                    // Consulta para verificar si el plano está vinculado a un pedido

                    string sSqlVinculado = "SELECT ID_OT, Consecutivo_Pedido, AfectaBolsa FROM tblPlano WHERE Plano = @plano";
                    SqlCommand commandVinculado = new SqlCommand(sSqlVinculado, connection);
                    commandVinculado.Parameters.AddWithValue("@plano", tbPlano.Text.Trim());

                    SqlDataReader reader = commandVinculado.ExecuteReader();

                    if (reader.Read())
                    {
                        // Verificar si el plano está vinculado a un pedido
                        string idOT = reader["ID_OT"].ToString();
                        string consecutivoPedido = reader["Consecutivo_Pedido"].ToString();
                        bool afectaBolsa = (bool)reader["AfectaBolsa"];

                        reader.Close();

                        if (idOT == "Nula" && !afectaBolsa)
                        {
                            // No está vinculado a ningún pedido y no afecta ninguna bolsa, se puede eliminar

                            string sSqlEliminar = "DELETE FROM tblPlano WHERE Plano = @plano";
                            SqlCommand commandEliminar = new SqlCommand(sSqlEliminar, connection);
                            commandEliminar.Parameters.AddWithValue("@plano", tbPlano.Text.Trim());


                            commandEliminar.ExecuteNonQuery();

                            // Mensaje de éxito
                            string mensajeExito = "El plano " + tbPlano.Text.Trim() + " ha sido eliminado exitosamente.";
                            string scriptExito = "alert('" + mensajeExito + "');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", scriptExito, true);
                        }
                        else
                        {
                            // El plano está vinculado a un pedido o afecta una bolsa, no se puede eliminar
                            string scriptNoPermiso = "alert('El plano está vinculado a un pedido o afecta una bolsa y no puede ser eliminado.');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptNoPermiso, true);
                        }
                    }

                    reader.Close();
                }
            }

        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            ControlInicialLinkButtons();
            ControlInicialButtons();
            DeshabilitarTextBoxes(listaTextBoxes);
            DeshabilitarDropDownLists(listaDropDownLists);
            DeshabilitarCheckBoxes();
            limpiarCampos(listaTextBoxes, listaDropDownLists);
        }

        protected void btnCargarPlano_Click(object sender, EventArgs e)
        {
            string script = "<script>enviarFormulario();</script>";
            ScriptManager.RegisterStartupScript(this, GetType(), "enviarFormulario", script, false);
        }
    }

}