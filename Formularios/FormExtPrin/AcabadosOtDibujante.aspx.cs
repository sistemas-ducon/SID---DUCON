using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class AcabadosOtDibujante : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

                if (!IsPostBack)
                {
                    Plano.Text = Session["planoAcabado"]?.ToString();
                    OT.Text = Session["Id_OT2"]?.ToString();
                    Pedido.Text = Session["pedido2"]?.ToString();


                    if ((Session["Departamento"].ToString() == "Diseño" || Session["Departamento"].ToString() == "Ventas") && ((OT.Text != "" && Convert.ToBoolean(Session["estadoBotonOk"]?.ToString()) == true) || OT.Text == ""))
                    {
                        Cargar_AcabadosPlanoDibujo();
                        DataGridAcabados1.DataBind();
                    }

                }
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }

        }

        //1
        protected void DataGridAcabados1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerAcabadoDib")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridAcabados1.Items[rowIndex];
                string ApliAcabado = row.Cells[6].Text;
                Session["AplicadoASession"] = row.Cells[6].Text;
                Session["IDGruAcaSession"] = row.Cells[8].Text;
                Session["ID_OtAcabDefSession"] = row.Cells[7].Text;
                Session["IDInsumoASession"] = row.Cells[9].Text;
                Session["IDFamiliarSession"] = row.Cells[10].Text;

                // Se utiliza para darle el color solo a la fila seleccionada 
                foreach (DataGridItem item in DataGridAcabados1.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  anderson
                e.Item.CssClass = "fila-seleccionada";

                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
                {

                    // Validar que la OT no este cerrada para dibujo para permitir el doble click  
                    // Validar  cuando la aplicacion del acabado es diferente de 'E'


                    // Se compara si el click es en la misma fila con el id del acabado
                    if (row.Cells[7].Text == Session["ID_Acabado"]?.ToString())
                    {

                        if (Convert.ToBoolean(Session["estadoBotonOk"]?.ToString()) == true)
                        {
                            // Incrementar la variable de sesión "ClickCount" en el servidor
                            int clickCount = Convert.ToInt32(Session["ClickCount3"]) + 1;
                            Session["ClickCount3"] = clickCount;

                            // se valida si es el segundo click en la misma fila 
                            if (clickCount == 2)
                            {

                                if (ApliAcabado != "E")
                                {

                                    // Dependiente del CheckBox Se carga uno u otro DataSource
                                    if (chkTodoAcabados.Checked)
                                    {
                                        DsDefinirAcabado1.SelectParameters["ID_GrupoAcabado"].DefaultValue = Session["IDGruAcaSession"].ToString();

                                        DataGridDefinirAcabado.DataSourceID = "DsDefinirAcabado1";

                                        DataGridDefinirAcabado.DataBind();
                                    }
                                    else
                                    {
                                        DsDefinirAcabado.SelectParameters["ID_GrupoAca"].DefaultValue = Session["IDGruAcaSession"].ToString();

                                        DataGridDefinirAcabado.DataSourceID = "DsDefinirAcabado";

                                        DataGridDefinirAcabado.DataBind();
                                    }

                                    btnVerOrigen.Enabled = true;
                                    btnVerOrigen.CssClass = "icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo";

                                    btnAdicionarAcabado.Enabled = true;
                                    btnAdicionarAcabado.CssClass = "icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo";

                                    btnRefrescar.Enabled = true;
                                    btnRefrescar.CssClass = "icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo";

                                    RefrescarOrigen();

                                    // Muestra un modal para administrar el acabado 

                                    string script2 = @"mostrarTap();";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarTap", script2, true);


                                    // Reiniciar la variable de sesión "ClickCount" a 0 para la próxima interacción                        
                                    Session.Remove("ID_Acabado");
                                    Session.Remove("ClickCount3");


                                    //Limpiar las variables de Session de definicion de acabados 

                                    Session.Remove("ClickCountDefAcab");
                                    Session.Remove("ID_DeF_Acab");

                                }
                                else
                                {
                                    // nombre del acabado a eliminar 
                                    span_NombreAcabado.InnerText = row.Cells[1].Text;


                                    Session["IdAcabadoElimnar"] = row.Cells[7].Text;

                                    // Reiniciar la variable de sesión "ClickCount" a 0 para la próxima interacción                        
                                    Session.Remove("ID_Acabado");
                                    Session.Remove("ClickCount3");


                                    string script2 = @"mostrarModalEliminarAcabado();";
                                    ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalEliminarAcabado", script2, true);

                                }

                            }

                        }
                        else
                        {
                            // Muestra un modal para administrar el acabado 
                            // Reiniciar la variable de sesión "ClickCount" a 0 para la próxima interacción                        
                            Session.Remove("ID_Acabado");
                            Session.Remove("ClickCount3");

                        }



                    }
                    else
                    {
                        // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                        Session["ClickCount3"] = 1;
                        Session["ID_Acabado"] = row.Cells[7].Text;


                        // Llamar a la función JavaScript para enfocar y desplazar la fila
                        ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);

                    }
                }
               
            }
        }
 
        protected void btnCerrarDefinirAcabado_Click(object sender, EventArgs e)
        {
            string script2 = @"mostrarTapAcabados();";
            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarTapAcabados", script2, true);
        }
       
        //2
        protected void btnCerrarAcabadosPlano_Click(object sender, EventArgs e)
        {
            Session.Remove("planoAcabado");
            Session.Remove("estadoBotonOk");

            Response.Redirect("~/Formularios/OrdenTrabajo.aspx");
        }

        //3
        protected void tbBuscarAcaba_TextChanged(object sender, EventArgs e)
        {

            // Dependiente del CheckBox Se carga uno u otro DataSource
            if (chkTodoAcabados.Checked)
            {
                DsDefinirAcabado1.SelectParameters["ID_GrupoAcabado"].DefaultValue = Session["IDGruAcaSession"].ToString();

                DataGridDefinirAcabado.DataSourceID = "DsDefinirAcabado1";


            }
            else
            {
                DsDefinirAcabado.SelectParameters["ID_GrupoAca"].DefaultValue = Session["IDGruAcaSession"].ToString();

                DataGridDefinirAcabado.DataSourceID = "DsDefinirAcabado";

            }


            DataGridDefinirAcabado.DataBind();

        }

        //4
        protected void btnBuscarAcab_Click(object sender, EventArgs e)
        {
           
        }

        // Definir Acabados del plano 
        //5
        protected void chkTodoAcabados_CheckedChanged(object sender, EventArgs e)
        {






            // Dependiente del CheckBox Se carga uno u otro DataSource
            if (chkTodoAcabados.Checked)
            {
                DsDefinirAcabado1.SelectParameters["ID_GrupoAcabado"].DefaultValue = Session["IDGruAcaSession"].ToString();

                DataGridDefinirAcabado.DataSourceID = "DsDefinirAcabado1";


            }
            else
            {
                DsDefinirAcabado.SelectParameters["ID_GrupoAca"].DefaultValue = Session["IDGruAcaSession"].ToString();

                DataGridDefinirAcabado.DataSourceID = "DsDefinirAcabado";

            }

            DataGridDefinirAcabado.DataBind();

        }

        //6
        protected void DataGridDefinirAcabado_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int linea = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "DeLinea"));
                int Activo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Activo"));


                TableCell cell = e.Item.Cells[3];
                cell.Text = (linea == 1) ? "Si" : "No";

                TableCell cell1 = e.Item.Cells[4];
                cell1.Text = (Activo == 1) ? "Si" : "No";

            }
        }

        //7
        protected void DataGridDefinirAcabado_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            DataGridItem row = DataGridDefinirAcabado.Items[rowIndex];
            string CodInv = row.Cells[1].Text;
            string Descripcion = row.Cells[2].Text;

            bool Linea = Convert.ToBoolean(row.Cells[3].Text.Replace("Si", "true").Replace("No", "false"));
            bool Estado = Convert.ToBoolean(row.Cells[4].Text.Replace("Si", "true").Replace("No", "false"));
            string ID_Acabado = row.Cells[9].Text;

            // Se utiliza para darle el color solo a la fila seleccionada 
            foreach (DataGridItem item in DataGridDefinirAcabado.Items)
            {
                if (item != row)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            //se usa Para darle un color a la fila seleccionada  anderson
            e.Item.CssClass = "fila-seleccionada";



            if (row.Cells[9].Text == Session["ID_DeF_Acab"]?.ToString())
            {
                // Incrementar la variable de sesión "ClickCount" en el servidor
                int clickCount = Convert.ToInt32(Session["ClickCountDefAcab"]) + 1;
                Session["ClickCountDefAcab"] = clickCount;

                // se valida si es el segundo click en la misma fila 
                if (clickCount == 2)
                {

                    if (Estado)
                    {
                        // Asigar Acabado
                        ActualizarDefinicionAcabadoPlano();
                        DataGridAcabados1.DataBind();

                        string script2 = @"mostrarTapAcabados();";
                        ScriptManager.RegisterStartupScript(this, GetType(), "mostrarTapAcabados", script2, true);

                        DataGridAcabados1.DataBind();

                        // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                        Session.Remove("ClickCountDefAcab");
                        Session.Remove("ID_DeF_Acab");

                    }
                    else
                    {
                        string scriptNoAcabados1 = $"alert('El acabado {tbDescripAcaba.Text} , se encuentra inactivo. No puede asignarlo');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoAgregado", scriptNoAcabados1, true);

                        // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                        Session["ClickCountDefAcab"] = 1;
                        Session["ID_DeF_Acab"] = row.Cells[9].Text;
                    }

                }
            }
            else
            {
                // Si el clic no es en la misma fila, reiniciar la variable de sesión "ClickCount" a 1
                Session["ClickCountDefAcab"] = 1;
                Session["ID_DeF_Acab"] = row.Cells[9].Text;

                tbCodInventario.Text = CodInv;
                tbDescripAcaba.Text = Descripcion;

                chkAcabadoActivo.Checked = Estado;
                chkLinea.Checked = Linea;

                btnModificarAcabado.Enabled = true;
                btnModificarAcabado.CssClass = "icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo";

                btnGrabarRedAcaMod.Visible = true;
                btnGrabarRedAcabadoNue.Visible = false;

                lb_ID_AcadoMod.Text = ID_Acabado;


                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);

            }

        }

        //8
        private void ActualizarDefinicionAcabadoPlano()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE  tblOTAcabadoDefinitivo SET  oadCodInvDes = @CodAcaDes, oadDescripcionAcabado = @DescripcionAcaba " +
                              " WHERE id_OTAcabadoDefinitivo = @ID_AcabadoDefinitivo ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@CodAcaDes", tbCodInventario.Text);
                    cmd.Parameters.AddWithValue("@DescripcionAcaba", tbDescripAcaba.Text);

                    cmd.Parameters.AddWithValue("@ID_AcabadoDefinitivo", Session["ID_OtAcabDefSession"].ToString());

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //9
        protected void btnEliminarAcabado_SI_Click(object sender, EventArgs e)
        {

            //Se realiza la eliminacion del acabado 
            EliminarAcabaDefinitivo();
            DataGridAcabados1.DataBind();


        }

        //10
        private void EliminarAcabaDefinitivo()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "Delete  from tblOTAcabadoDefinitivo where id_OTAcabadodefinitivo= @IdAcabadoPlano";
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.Parameters.AddWithValue("@IdAcabadoPlano", Session["IdAcabadoElimnar"].ToString());

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    Session.Remove("IdAcabadoElimnar");
                }
            }
        }

        //11
        protected void btneliminarAcabado_NO_Click(object sender, EventArgs e)
        {

            Session.Remove("IdAcabadoElimnar");

        }

        //12
        protected void DataGridAcabadoVentas_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            DataGridItem row = DataGridAcabadoVentas.Items[rowIndex];

            // Se utiliza para darle el color solo a la fila seleccionada 
            foreach (DataGridItem item in DataGridAcabadoVentas.Items)
            {
                if (item != row)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            //se usa Para darle un color a la fila seleccionada  anderson
            e.Item.CssClass = "fila-seleccionada";

            Session["DescripAcabadoSession"] = row.Cells[3].Text;
            Session["DetalleAdicionalSession"] = row.Cells[4].Text;
            Session["DescripGrupoSession"] = row.Cells[5].Text;

            Session["AcacadoSeleccionadoSession"] = "1";

            btnAgregarAcabado.Enabled = true;
            btnAgregarAcabado.CssClass = "btn btn-sm btn-outline-secondary";


           


        }

        //13
        protected void btnAgregarAcabado_Click(object sender, EventArgs e)
        {
            // valida que se haya seleccionado un acabado 
            if (Session["AcacadoSeleccionadoSession"]?.ToString() == "1")
            {
                if (Convert.ToBoolean(Session["estadoBotonOk"]?.ToString()) == true)
                {
                    if (AgregarAcabado())
                    {
                        // El usuario no tiene permisos para realizar la acción
                        DataGridAcabados1.DataBind();
                       
                    }
                    else
                    {
                        // El usuario no tiene permisos para realizar la acción
                        string scriptError = "alert('El acabado no se puedo agregar con exito, intentelo nuevamente mas tarde.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptError, true);

                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ModalAcabados').modal('show');", true);
                    }
                }
                else
                {
                    // El usuario no tiene permisos para realizar la acción
                    string scriptError = "alert('No es posible agregar el acabado, ya que este pedido ya ha sido programado.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptError, true);

                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#ModalAcabados').modal('show');", true);
                }




            }
            else
            {
                // El usuario no tiene permisos para realizar la acción
                string scriptError = "alert('No se ha seleccionado un acabado para agregar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoPermiso", scriptError, true);

                string script1 = @"mostrarModal();";
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", script1, true);
            }

        }

        //14
        public bool AgregarAcabado()
        {
            // Consulta SQL para insertar un nuevo registro
            string consultaActual = "INSERT INTO tblOTAcabadoDefinitivo (oadPLano, oadAplicacionAcabado, oadDescripcionAcabado, oadDescripcionGrupoObjeto) " +
                                    "VALUES (@plano, @AA, @DescripcionAcabado, @DescripcionGrupo)";

            // Obtener la cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Abrir la conexión
                connection.Open();

                using (SqlCommand command = new SqlCommand(consultaActual, connection))
                {
                    // Agregar los parámetros a la consulta
                    command.Parameters.AddWithValue("@plano", Plano.Text);
                    command.Parameters.AddWithValue("@AA", "E");
                    command.Parameters.AddWithValue("@DescripcionAcabado", Session["DescripAcabadoSession"].ToString() + " - " + Session["DetalleAdicionalSession"].ToString());
                    command.Parameters.AddWithValue("@DescripcionGrupo", Session["DescripGrupoSession"].ToString());

                    // Ejecutar la consulta y obtener el número de filas afectadas
                    int filasAfectadas = command.ExecuteNonQuery();

                    // Si se afectó al menos una fila, la operación fue exitosa
                    if (filasAfectadas > 0)
                    {
                        // Limpiar las variables de sesión
                        Session.Remove("DescripAcabadoSession");
                        Session.Remove("DescripGrupoSession");
                        Session.Remove("DetalleAdicionalSession");
                        Session.Remove("AcacadoSeleccionadoSession");


                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }

        //15
        private void Cargar_AcabadosPlanoDibujo()
        {
            bool TerminadoDibujo = Convert.ToBoolean(Session["estadoBotonOk"]?.ToString());

            if (Session["Departamento"].ToString().ToUpper() == "DISEÑO" || Session["Departamento"].ToString().ToUpper() == "VENTAS" && OT.Text != "" && !TerminadoDibujo || Session["Departamento"].ToString().ToUpper() == "DESARROLLO DE PRODUCTO")
            {
                //PARA BORRAR ACABADOS
                //SE COLOCAN TODOS LOS ACABADOS DEL PLANO COMO  INACTIVOS, SE ACTIVAN LOS QUE VAN Y DESPUES SE BORRAN LOS QUE QUEDARON INACTIVOS

                ActualizarAcabadosDefinitivo();

                // CONSULTA PARA DEFINIR LOS ACABADOS DESDE MODULOS - INSUMOS
                DataTable AcaDefModInsumo = ConsultarAcabadoDesdeModInsumo();

                if (AcaDefModInsumo.Rows.Count > 0)
                {
                    foreach (DataRow row in AcaDefModInsumo.Rows)
                    {
                        string ID_Familia = row["ID_Familia"].ToString();
                        string ID_Insumo = row["Id_Insumo"].ToString();
                        string DescriFam = row["Descripcion_Familia"].ToString();
                        string IdInventario = row["Id_Inventario"].ToString();
                        string Descri_Insumo = row["Descripcion_Insumo"].ToString();
                        string ApliAcabado = row["AplicacionAcabado"].ToString();
                        string IdGruAcab = row["IDGrupoAcabado"].ToString();
                        string DescrpGrup = row["Descripcion_Grupo"].ToString();
                        string DescObj = row["DescripcionGrupoObjeto"].ToString();


                        if (!ConsultarAcabadoDefiniIdFamiliaIdInsumo(ID_Familia, ID_Insumo))
                        {
                            //SE INSERTA NUEVO ITEM PARA ACABADO
                            InsertarItemAcabado(ID_Familia, DescriFam, ID_Insumo, IdInventario, Descri_Insumo, ApliAcabado, IdGruAcab, DescrpGrup, DescObj);
                        }
                        else
                        {
                            //SE COLOCA EL ACABADO COMO ACTIVO
                            AcualizarAcabadoInactivo(ID_Familia, ID_Insumo);
                        }
                    }

                }

                //CONSULTA PARA DEFINIR LOS ACABADOS DESDE EL GRUPO DEL INSUMO
                // CONSULTA PARA DEFINIR LOS ACABADOS DESDE MODULOS - INSUMOS
                DataTable AcaGrupoInsumo = ConsultarAcabadoGrupoInsumo();

                if (AcaGrupoInsumo.Rows.Count > 0)
                {
                    foreach (DataRow row in AcaGrupoInsumo.Rows)
                    {
                        string ID_Grupo = row["IDGrupoAcabado"].ToString();
                        string DescripGrupo = row["Descripcion_Grupo"].ToString();

                        if (!ConsultarAcabadoDefiXIdGrupo(ID_Grupo))
                        {
                            //SE INSERTA NUEVO ITEM PARA ACABADO POR ID GRUPO

                            InsertarItemAcabadoIdGrupo(ID_Grupo, DescripGrupo);
                        }
                        else
                        {
                            //SE COLOCA EL ACABADO COMO ACTIVO
                            AcualizarAcabadoInactivoIdGrupo(ID_Grupo);
                        }
                    }
                }

                //SE BORRAN LOS ACABADOS QUE QUEDARON INACTIVOS
                EliminarAcabaDefinitivoInactivo();
            }
        }

        //16
        private void ActualizarAcabadosDefinitivo()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE tblOTAcabadoDefinitivo SET oadActivo=0 WHERE oadPLano = @plano AND oadAplicacionAcabado <> 'E'";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", Plano.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //17
        private DataTable ConsultarAcabadoDesdeModInsumo()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblPlano.Plano, tblPlano.Id_OT, tblPlano.COnsecutivo_Pedido, tblGrupoObjeto.Descripcion_Grupo AS DescripcionGrupoObjeto," +
                    "tblFamiliaModulo.ID_Familia, tblFamiliaModulo.Descripcion_Familia, tblInsumo.Id_Insumo, tblInsumo.ID_Inventario," +
                    "tblInsumo.Descripcion_Insumo, tblTipoInsumo.IDGrupoAcabado, tblGrupodeAcabado.Descripcion_Grupo, tblInsumo.AplicacionAcabado," +
                    " tblPanel.Apunta_Cod_PSL FROM tblPlano INNER JOIN (((tblGrupoObjeto INNER JOIN tblPanel ON tblGrupoObjeto.ID_GrupoObjeto = tblPanel.Id_GrupoObjeto) " +
                    "INNER JOIN (((tblFamiliaModulo INNER JOIN tblModulo ON tblFamiliaModulo.ID_Familia = tblModulo.ID_Familia) " +
                    "INNER JOIN (((tblTipoInsumo INNER JOIN tblGrupodeAcabado ON tblTipoInsumo.IDGrupoAcabado = tblGrupodeAcabado.ID_GrupoAcabado) " +
                    "INNER JOIN tblInsumo ON tblTipoInsumo.Id_TipoInsumo = tblInsumo.Id_TipoInsumo) " +
                    "INNER JOIN tblModulo_Insumo ON tblInsumo.Id_Insumo = tblModulo_Insumo.Id_Insumo) ON tblModulo.Id_Modulo = tblModulo_Insumo.Id_Modulo) " +
                    "INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo) ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum) " +
                    "INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum) ON tblPlano.Plano = tblPlano_Panel.Id_Plano " +
                    "GROUP BY tblPlano.Plano, tblPlano.Id_OT, tblPlano.COnsecutivo_Pedido, tblGrupoObjeto.Descripcion_Grupo, tblFamiliaModulo.ID_Familia," +
                    "tblFamiliaModulo.Descripcion_Familia, tblInsumo.Id_Insumo, tblInsumo.ID_Inventario, tblInsumo.Descripcion_Insumo," +
                    "tblTipoInsumo.IDGrupoAcabado, tblGrupodeAcabado.Descripcion_Grupo, tblInsumo.AplicacionAcabado, tblPanel.Apunta_Cod_PSL " +
                    "HAVING (((tblPlano.Plano)=@plano) AND ((tblInsumo.AplicacionAcabado)='M') AND ((tblPanel.Apunta_Cod_PSL) = 0)) " +
                    "ORDER BY tblFamiliaModulo.Descripcion_Familia, tblGrupoObjeto.Descripcion_Grupo";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@plano", Plano.Text);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }

        //18
        public bool ConsultarAcabadoDefiniIdFamiliaIdInsumo(string ID_Familia, string ID_Insumo)
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblOTAcabadoDefinitivo " +
                               "WHERE oadPlano= @plano AND oadID_Familia= @ID_Familia AND oadId_Insumo = @Id_Insumo ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@plano", Plano.Text);
                    command.Parameters.AddWithValue("@ID_Familia", ID_Familia);
                    command.Parameters.AddWithValue("@Id_Insumo", ID_Insumo);


                    connection.Open();
                    // Ejecutar la consulta y usar SqlDataReader para verificar si hay filas
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

        //19
        private void InsertarItemAcabado(string ID_Familia, string DescriFam, string ID_Insumo, string IdInventario, string Descri_Insumo, string ApliAcabado, string IdGruAcab, string DescrpGrup, string DescObj)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblOTAcabadoDefinitivo (oadPLano,oadID_Familia,oadDescripcion_Familia,oadId_Insumo,oadCodInvOri," +
                              "oadDescripcion_Insumo,oadAplicacionAcabado,oadIDGrupoAcabado,oadDesGrupoAcabado,oadDescripcionGrupoObjeto,oadActivo) " +
                              "VALUES (@plano,@IdFamilia, @DecripcionFamilia, @Id_Insumo, @CodigoInventario, @DescripcionInsumo,@AplicacionAcabado, " +
                              "@ID_GrupoAcabado, @DescripcionGruAcabado,@DescriGrupoObjeto, @Activo )";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", Plano.Text);
                    cmd.Parameters.AddWithValue("@IdFamilia", ID_Familia);
                    cmd.Parameters.AddWithValue("@DecripcionFamilia", DescriFam);
                    cmd.Parameters.AddWithValue("@Id_Insumo", ID_Insumo);
                    cmd.Parameters.AddWithValue("@CodigoInventario", IdInventario);
                    cmd.Parameters.AddWithValue("@DescripcionInsumo", Descri_Insumo);
                    cmd.Parameters.AddWithValue("@AplicacionAcabado", ApliAcabado);
                    cmd.Parameters.AddWithValue("@ID_GrupoAcabado", IdGruAcab);
                    cmd.Parameters.AddWithValue("@DescripcionGruAcabado", DescrpGrup);
                    cmd.Parameters.AddWithValue("@DescriGrupoObjeto", DescObj);
                    cmd.Parameters.AddWithValue("@Activo", 1);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }
        
        //20
        private void AcualizarAcabadoInactivo(string ID_Familia, string ID_Insumo)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE tblOTAcabadoDefinitivo SET oadActivo = 1 " +
                              "WHERE oadPlano = @plano AND oadID_Familia = @ID_Familia AND oadId_Insumo = @ID_Insumo";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@plano", Plano.Text);
                    cmd.Parameters.AddWithValue("@ID_Familia", ID_Familia);
                    cmd.Parameters.AddWithValue("@ID_Insumo", ID_Insumo);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //21
        private DataTable ConsultarAcabadoGrupoInsumo()
        {
            DataTable dataTable = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                string sSql = "SELECT tblPlano.Plano, tblPlano.Id_OT, tblPlano.COnsecutivo_Pedido, tblTipoInsumo.RequiereAcabado," +
                    "tblTipoInsumo.IDGrupoAcabado, tblGrupodeAcabado.Descripcion_Grupo FROM tblPlano " +
                    "INNER JOIN ((tblPanel " +
                    "INNER JOIN ((tblModulo " +
                    "INNER JOIN (((tblTipoInsumo " +
                    "INNER JOIN tblGrupodeAcabado ON tblTipoInsumo.IDGrupoAcabado = tblGrupodeAcabado.ID_GrupoAcabado) " +
                    "INNER JOIN tblInsumo ON tblTipoInsumo.Id_TipoInsumo = tblInsumo.Id_TipoInsumo) " +
                    "INNER JOIN tblModulo_Insumo ON tblInsumo.Id_Insumo = tblModulo_Insumo.Id_Insumo) ON tblModulo.Id_Modulo = tblModulo_Insumo.Id_Modulo) " +
                    "INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo) ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum) " +
                    "INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum) ON tblPlano.Plano = tblPlano_Panel.Id_Plano " +
                    "GROUP BY tblPlano.Plano, tblPlano.Id_OT, tblPlano.COnsecutivo_Pedido, tblTipoInsumo.RequiereAcabado, tblTipoInsumo.IDGrupoAcabado," +
                    "tblGrupodeAcabado.Descripcion_Grupo, tblInsumo.AplicacionAcabado " +
                    "HAVING (((tblPlano.Plano)= @plano) AND ((tblInsumo.AplicacionAcabado)='G'))";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@plano", Plano.Text);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable
            return dataTable;
        }

        //22
        public bool ConsultarAcabadoDefiXIdGrupo(string ID_Grupo)
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblOTAcabadoDefinitivo " +
                               "WHERE oadPlano= @plano AND oadIDGrupoAcabado= @ID_GrupoAca AND oadAplicacionAcabado = 'G' ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@plano", Plano.Text);
                    command.Parameters.AddWithValue("@ID_GrupoAca", ID_Grupo);

                    connection.Open();
                    // Ejecutar la consulta y usar SqlDataReader para verificar si hay filas
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

        //23
        private void InsertarItemAcabadoIdGrupo(string ID_Grupo, string DescripGrupo)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblOTAcabadoDefinitivo (oadPLano,oadIDGrupoAcabado,oadDesGrupoAcabado,oadAplicacionAcabado,oadDescripcionGrupoObjeto,oadDescripcion_Familia)" +
                              "VALUES (@plano,@IdGrupo, @DecripcionGrupoAca, @Aplicacion, @DescriGrupoObjeto, @DescripcionFamilia )";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", Plano.Text);
                    cmd.Parameters.AddWithValue("@IdGrupo", ID_Grupo);
                    cmd.Parameters.AddWithValue("@DecripcionGrupoAca", DescripGrupo);
                    cmd.Parameters.AddWithValue("@Aplicacion", "G");
                    cmd.Parameters.AddWithValue("@DescriGrupoObjeto", DescripGrupo);
                    cmd.Parameters.AddWithValue("@DescripcionFamilia", DescripGrupo);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //24
        private void AcualizarAcabadoInactivoIdGrupo(string ID_Grupo)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE tblOTAcabadoDefinitivo SET oadActivo = 1 " +
                              "WHERE oadPlano = @plano AND oadIDGrupoAcabado = @Id_Grupo AND oadAplicacionAcabado = 'G'";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@plano", Plano.Text);
                    cmd.Parameters.AddWithValue("@Id_Grupo", ID_Grupo);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //25
        private void EliminarAcabaDefinitivoInactivo()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE FROM  tblOTAcabadoDefinitivo WHERE oadPlano = @plano AND oadActivo = 0 ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();

                    cmd.Parameters.AddWithValue("@plano", Plano.Text);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //26
        protected void btnVerOrigen_Click(object sender, EventArgs e)
        {
            if (Session["AplicadoASession"].ToString().ToUpper() == "G")
            {

                if (DivOrigenAcabado.Visible == true)
                {
                    DivOrigenAcabado.Visible = false; // Oculta el div
                }
                else
                {
                    DivOrigenAcabado.Visible = true; // Muestra el div

                    DSOrigenAca.SelectParameters["Id_Acabado"].DefaultValue = Session["IDGruAcaSession"].ToString();
                    DSOrigenAca.SelectParameters["AplicadoA"].DefaultValue = Session["AplicadoASession"].ToString();

                    DataGridOrigenAcabado.DataSourceID = "DSOrigenAca";


                    DataGridOrigenAcabado.DataBind();
                }

            }
            else
            {
                if (DivOrigenAcabado.Visible == true)
                {
                    DivOrigenAcabado.Visible = false; // Oculta el div
                }
                else
                {
                    DivOrigenAcabado.Visible = true; // Muestra el div

                    DsOrigenAcab1.SelectParameters["IdGrupoAcab"].DefaultValue = Session["IDGruAcaSession"].ToString();
                    DsOrigenAcab1.SelectParameters["ID_Insumo"].DefaultValue = Session["IDInsumoASession"].ToString();
                    DsOrigenAcab1.SelectParameters["IdFamilia"].DefaultValue = Session["IDFamiliarSession"].ToString();


                    DataGridOrigenAcabado.DataSourceID = "DsOrigenAcab1";


                    DataGridOrigenAcabado.DataBind();

                }

            }
 
        }

        //NUEVO
        protected void RefrescarOrigen()
        {
            if (Session["AplicadoASession"].ToString().ToUpper() == "G")
            {


                DSOrigenAca.SelectParameters["Id_Acabado"].DefaultValue = Session["IDGruAcaSession"].ToString();
                DSOrigenAca.SelectParameters["AplicadoA"].DefaultValue = Session["AplicadoASession"].ToString();
                DataGridOrigenAcabado.DataSourceID = "DSOrigenAca";
                DataGridOrigenAcabado.DataBind();

            }
            else
            {

                DsOrigenAcab1.SelectParameters["IdGrupoAcab"].DefaultValue = Session["IDGruAcaSession"].ToString();
                DsOrigenAcab1.SelectParameters["ID_Insumo"].DefaultValue = Session["IDInsumoASession"].ToString();
                DsOrigenAcab1.SelectParameters["IdFamilia"].DefaultValue = Session["IDFamiliarSession"].ToString();
                DataGridOrigenAcabado.DataSourceID = "DsOrigenAcab1";
                DataGridOrigenAcabado.DataBind();

            }
        }

        //27
        protected void btnAdicionarAcabado_Click(object sender, EventArgs e)
        {

            // Control de campos 
            tbCodInventario.Enabled = true;
            tbCodInventario.Text = "";

            tbDescripAcaba.Enabled = true;
            tbDescripAcaba.Text = "";

            chkAcabadoActivo.Enabled = true;
            chkAcabadoActivo.Checked = false;

            chkLinea.Enabled = true;
            chkLinea.Checked = false;


            // Control de botones 
            btnGrabarRedAcaMod.Enabled = false;
            btnGrabarRedAcaMod.CssClass = "icong button-disabled btn btn-sm  shadow-sm";

            btnAdicionarAcabado.Enabled = false;
            btnAdicionarAcabado.CssClass = "icong button-disabled btn btn-sm  shadow-sm ";

            btnModificarAcabado.Enabled = false;
            btnModificarAcabado.CssClass = "icong button-disabled btn btn-sm  shadow-sm ";

            btnGrabarRedAcabadoNue.Visible = true;
            btnGrabarRedAcabadoNue.Enabled = true;
            btnGrabarRedAcabadoNue.CssClass = "icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo";

            btnGrabarRedAcaMod.Visible = false;
            btnGrabarRedAcaMod.Enabled = false;
            btnGrabarRedAcaMod.CssClass = "icong button-disabled btn btn-sm  shadow-sm";


        }
        
        //28
        protected void btnModificarAcabado_Click(object sender, EventArgs e)
        {
            // Control de campos 
            tbCodInventario.Enabled = true;
            tbDescripAcaba.Enabled = true;
            chkAcabadoActivo.Enabled = true;
            chkLinea.Enabled = true;


            // Control de botones 
            btnGrabarRedAcaMod.Enabled = true;
            btnGrabarRedAcaMod.CssClass = "icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo";

            btnAdicionarAcabado.Enabled = false;
            btnAdicionarAcabado.CssClass = "icong button-disabled btn btn-sm  shadow-sm ";

            btnModificarAcabado.Enabled = false;
            btnModificarAcabado.CssClass = "icong button-disabled btn btn-sm  shadow-sm ";

            btnGrabarRedAcabadoNue.Visible = false;
            btnGrabarRedAcabadoNue.Enabled = false;
            btnGrabarRedAcabadoNue.CssClass = "icong button-disabled btn btn-sm  shadow-sm";

          
        }

        //29
        protected void btnGrabarRedAcabadoNue_Click(object sender, EventArgs e)
        {
            if (tbCodInventario.Text.Trim() != "" && tbDescripAcaba.Text.Trim() != "")
            {
                if (ValidarExistenciaCodigoInventario(tbCodInventario.Text))
                {
                    string scriptNoAcabados1 = $"alert('El código de inventario {tbCodInventario.Text} ya existe');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoAgregado", scriptNoAcabados1, true);

                }
                else
                {
                    // Realizar la insercion del acabado
                    InsertarDefinicionAcabado();
                    DataGridDefinirAcabado.DataBind();
                    string scriptNoAcabados2 = "alert('La definición de acabado se insertó correctamente.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoAgregado", scriptNoAcabados2, true);
                    Refrescar();
                    return;

                }
            }
            else
            {
                string scriptNoAcabados2 = "alert('Los campos: código de inventario, descripción de acabado, check de linea y check de activo, son obligatorios.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoAgregado", scriptNoAcabados2, true);

            }

         
        }
        //30
        public bool ValidarExistenciaCodigoInventario(string codInventario)
        {
            bool existe = false;
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            // La consulta SQL
            string query = "SELECT COUNT(*) FROM tblacabado WHERE CodInventario = @CodInventario";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar el parámetro
                    command.Parameters.AddWithValue("@CodInventario", codInventario);

                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();

                        // Si el conteo es mayor que 0, el registro existe
                        existe = (count > 0);
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores (opcional)
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return existe;
        }

        //31
        private void InsertarDefinicionAcabado()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblacabado(CodInventario, Descripcion_Acabado, ID_GrupoAcabado, Delinea, Activo,CreadoPor,FechaCreacion," +
                              "ModificadoPor,FechaModificacion) VALUES (@CodInventario, @DescripcionAcaba, @IdGrupAcab, @Linea, @Estado, @UsuarioLogueado," +
                              " GETDATE(), @UsuarioLogueado1, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@CodInventario", tbCodInventario.Text);
                    cmd.Parameters.AddWithValue("@DescripcionAcaba", tbDescripAcaba.Text);
                    cmd.Parameters.AddWithValue("@IdGrupAcab", Session["IDGruAcaSession"].ToString());
                    cmd.Parameters.AddWithValue("@Linea", chkLinea.Checked);
                    cmd.Parameters.AddWithValue("@Estado", chkAcabadoActivo.Checked);
                    cmd.Parameters.AddWithValue("@UsuarioLogueado", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@UsuarioLogueado1", Session["usuariologueado"].ToString());


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //32
        private void Refrescar()
        {
            btnAdicionarAcabado.Enabled = true;
            btnAdicionarAcabado.CssClass = "icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo";

            btnModificarAcabado.Enabled = false;
            btnModificarAcabado.CssClass = "icong button-disabled btn btn-sm  shadow-sm ";

            btnGrabarRedAcaMod.Visible = false;
            btnGrabarRedAcaMod.Enabled = false;
            btnGrabarRedAcaMod.CssClass = "icong button-disabled btn btn-sm  shadow-sm ";

            btnGrabarRedAcabadoNue.Visible = true;
            btnGrabarRedAcabadoNue.Enabled = false;
            btnGrabarRedAcabadoNue.CssClass = "icong button-disabled btn btn-sm  shadow-sm";


            // Control de campos 
            tbCodInventario.Enabled = false;
            tbCodInventario.Text = "";

            tbDescripAcaba.Enabled = false;
            tbDescripAcaba.Text = "";

            chkAcabadoActivo.Enabled = false;
            chkAcabadoActivo.Checked = false;

            chkLinea.Enabled = false;
            chkLinea.Checked = false;


            Session.Remove("ID_DeF_Acab");
            Session.Remove("ClickCountDefAcab");

 
        }

        //33
        protected void btnGrabarRedAcaMod_Click(object sender, EventArgs e)
        {
            if (tbCodInventario.Text.Trim() != "" && tbDescripAcaba.Text.Trim() != "")
            {
                ActualizarDefinicionAcabado();
                DataGridDefinirAcabado.DataBind();
                string scriptNoAcabados2 = "alert('La definición de acabado se actualizó correctamente.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoAgregado", scriptNoAcabados2, true);
                Refrescar();
                return;
            }
            else
            {
                string scriptNoAcabados2 = "alert('Los campos: código de inventario, descripción de acabado, check de linea y check de activo, son obligatorios.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoAgregado", scriptNoAcabados2, true);

            }

        }

        //34
        private void ActualizarDefinicionAcabado()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "UPDATE  tblacabado SET  CodInventario = @CodInventario, Descripcion_Acabado = @DescripcionAcaba, Delinea = @Linea, Activo = @Estado,ModificadoPor = @UsuarioLogueado, " +
                              " FechaModificacion=Getdate() WHERE ID_Acabado = @ID_Acabado  ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@CodInventario", tbCodInventario.Text);
                    cmd.Parameters.AddWithValue("@DescripcionAcaba", tbDescripAcaba.Text);

                    cmd.Parameters.AddWithValue("@Linea", chkLinea.Checked);
                    cmd.Parameters.AddWithValue("@Estado", chkAcabadoActivo.Checked);
                    cmd.Parameters.AddWithValue("@UsuarioLogueado", Session["usuariologueado"].ToString());
                    cmd.Parameters.AddWithValue("@ID_Acabado", lb_ID_AcadoMod.Text); // Pendiente poner el ID_Acabado 

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }

        //35
        protected void btnRefrescar_Click(object sender, EventArgs e)
        {
            Refrescar();

        }


    }
}