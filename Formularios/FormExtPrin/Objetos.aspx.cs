using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SISTEMA_INTEGRAL_DUCON.Formularios.OrdenTrabajo;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class Objetos : System.Web.UI.Page
    {

        private string CadenaConexionSID = "BD_SIDSQL";

        private List<TextBox> listaTextBoxes;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuariologueado"] != null)
            {
                if (!IsPostBack)
                {
                    BotonesIniciales();
                    listaTextBoxes = new List<TextBox>
                    {
                       tbAlturaD,tbAnchoD

                    };
                    DeshabilitarTextBoxes(listaTextBoxes);

                    Adicionar.Enabled = false;
                    Adicionar.CssClass = "bi bf btn btn-lg btn-outline-secondary";

                    BtnNuePan.Enabled = true;
                    BtnNuePan.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

                }
            }
            else
            {
                Response.Redirect("~/Formularios/Login.aspx");
            }


        }

        protected void BotonesIniciales()
        {
            List<System.Web.UI.Control> botones = new List<System.Web.UI.Control>
            {

                BtnNuePan,
                Grabar,
                BtnModPan,
                BtnEliPan,
                BtnBuscas,
                CopiarPanel


            };

            string cssClass = "btn btn-sm shadow button-disabled";

            foreach (System.Web.UI.Control boton in botones)
            {
                if (boton is System.Web.UI.WebControls.LinkButton)
                {
                    System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)boton;
                    linkButton.Enabled = false;
                    linkButton.CssClass = cssClass;
                }
            }

        }

        public void DeshabilitarTextBoxes(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox == tbAnchoD || textBox == tbAlturaD)
                {
                    textBox.Enabled = false;
                    textBox.CssClass = "form-control text-end ";
                    textBox.Text = "0";

                }
                else
                {
                    textBox.CssClass = "form-control text-end";
                }

            }
        }

        protected void ddlGrupoObjeto_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlGrupo.Items.Insert(0, new ListItem("Seleccione", ""));

        }
        protected void BuscarObjeto(object sender, EventArgs e)
        {

            if (rbObjeto.SelectedValue == "Objeto")
            {
                ObtenerDatosObjetos.SelectCommand = "sp_ObtenerDatosObjetoActivo";
                ObtenerDatosObjetos.DataBind();
                DataGridObjetos.DataBind();

            }
            else if (rbObjeto.SelectedValue == "Descripcion")
            {
                ObtenerDatosObjetos.SelectCommand = "sp_ObtenerDatosDescripActivo";
                ObtenerDatosObjetos.DataBind();
                DataGridObjetos.DataBind();
            }

            BotonesIniciales();

            BtnNuePan.Enabled = true;
            BtnNuePan.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            Session.Remove("Id_PanelNum_Session");
            SpanId_ObjetoEliminar.InnerText = "";
            SpanId_NombreObjetoEliminar.InnerText = "";
            spanAnchoEliminar.InnerText = "";
            spanAlturaEliminar.InnerText = "";

        }

        protected void DataGridObtenerDatosObjetos_LinkButton(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerObjetoDet")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridObjetos.Items[rowIndex];

                foreach (DataGridItem item in DataGridObjetos.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada";

                // Almacenar el IdObjeto en el HiddenField
                Nombre_Objeto_Hid.Value = row.Cells[1].Text;
                Ancho_Objeto_Hid.Value = row.Cells[3].Text;
                Id_Objeto_Hid.Value = row.Cells[7].Text;
                tbPrecioVenta.Text = row.Cells[8].Text;

                //Se carga el Id_Numerico para cargar objetos  objeto oculto 
                Session["Id_PanelNum_Session"] = row.Cells[7].Text;
                SpanId_ObjetoEliminar.InnerText = row.Cells[7].Text;

                // Nombre eliminar en el modal 
                SpanId_NombreObjetoEliminar.InnerText = row.Cells[1].Text;

                // Ancho Eliminar en el modal 
                spanAnchoEliminar.InnerText = row.Cells[3].Text;

                // Altura Eliminar modal 
                spanAlturaEliminar.InnerText = row.Cells[4].Text;
               
                
                
                
                // Asignar ID único a la fila
                row.Attributes["id"] = "row_" + rowIndex;

                ControlBotonesCrudObjetos();


                // Controlar la activacion  de los botones del crud 

                // Llamar a la función JavaScript para enfocar y desplazar la fila
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToRow", "focusAndScrollToRow('row_" + rowIndex + "');", true);



            }
        }

        private void ControlBotonesCrudObjetos()
        {
            BtnNuePan.Enabled = false;
            BtnNuePan.CssClass = "btn btn-sm shadow button-disabled";

            Grabar.Enabled = false;
            Grabar.CssClass = "btn btn-sm shadow button-disabled";

            BtnModPan.Enabled = true;
            BtnModPan.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";

            BtnEliPan.Enabled = true;
            BtnEliPan.CssClass = "btn btn-sm shadow button-enabled RojoCancelar";

            BtnBuscas.Enabled = false;
            BtnBuscas.CssClass = "btn btn-sm shadow button-disabled";

            CopiarPanel.Enabled = true;
            CopiarPanel.CssClass = "btn btn-sm shadow button-enabled ColorAzulActivo";
        }

        protected void Adicionar_Click(object sender, EventArgs e)
        {
            string IdObjeto = Id_Objeto_Hid.Value;
            float PrecioVentaCalculado;

            if (!string.IsNullOrEmpty(IdObjeto))
            {
                // Realizar la inserción utilizando el IdObjeto
                if (!ValidarObjetoExistPlano(Session["Numero_Plano"].ToString(), IdObjeto))
                {

                    if (tbPrecioVenta.Text == "0")
                    {
                        // Este método recalcula el precio de venta y trae tambien el peso 
                        CalcularPrecioVenta(Convert.ToInt32(IdObjeto), out PrecioVentaCalculado, out _);

                        tbPrecioVenta.Text = PrecioVentaCalculado.ToString();


                    }

                    if (tbPrecioVenta.Text != "0")
                    {
                        // Llamada al método para insertar un objeto en el plano
                        if(InsertarObjetoEnPlano(IdObjeto, tbCantidad.Text, tbPrecioVenta.Text, txObs1.InnerText))
                        {
                            string mensajeExito = "El objeto se añadió correctamente al plano.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showExito", "alert('" + mensajeExito + "');", true);
                        }

                    }
                    else
                    {
                        if (InsertarObjetoEnPlano(IdObjeto, tbCantidad.Text, tbPrecioVenta.Text, txObs1.InnerText))
                        {
                            string mensajeError = "El objeto a sido añadido al plano, pero el valor del objeto seleccionado no ha sido actualizado.";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('" + mensajeError + "');", true); ;
                        }

                    }

                }
                else
                {
                    // si ya esta creado me muestra mensaje de error 
                    string mensajeError = "El plano: " + Session["Numero_Plano"].ToString() + " ya contiene el objeto " + Nombre_Objeto_Hid.Value + " con ancho igual a " + Ancho_Objeto_Hid.Value;
                    ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('" + mensajeError + "');", true);
                }

                Id_Objeto_Hid.Value = string.Empty;
                Nombre_Objeto_Hid.Value = string.Empty;
                Ancho_Objeto_Hid.Value = string.Empty;

            }
            else
            {
                // No se ha seleccionado ningun panel 
                string mensajeError = "Por favor seleccione el panel que desea adicionar.";
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('" + mensajeError + "');", true);
            }
        }

        private bool ValidarObjetoExistPlano(string plano, string idObjeto)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "select * from tblPLano_Panel where Id_PLano= @plano AND Id_PanelNum = @IdObjeto";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@plano", plano);
                    cmd.Parameters.AddWithValue("@IdObjeto", idObjeto);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
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

        public void CalcularPrecioVenta(int idPanelNumerico, out float precioVenta, out float peso)
        {
            precioVenta = 0;
            peso = 0;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Ejecutar el procedimiento almacenado para actualizar el precio del objeto
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarPrecioObjeto", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Objeto", idPanelNumerico);
                    cmd.ExecuteNonQuery();
                }

                // Consultar la información actualizada del objeto
                string selectQuery = "SELECT Precio_Venta, PesoKG FROM tblPanel WHERE Id_Numerico = @ID_panelNumerico";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@ID_panelNumerico", idPanelNumerico);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener los valores del precio y el peso
                            precioVenta = Convert.ToSingle(reader["Precio_Venta"]);
                            peso = Convert.ToSingle(reader["PesoKG"]);
                        }
                    }
                }
            }
        }

        protected void tbCantidad_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbCantidad.Text, out _))
            {
                // El texto es numérico
                Adicionar.Enabled = true;
                Adicionar.CssClass = "bi bf btn btn-lg btn-primary";
            }
            else
            {
                // El texto no es numérico
                Adicionar.Enabled = false;
                Adicionar.CssClass = "bi bf btn btn-lg btn-outline-secondary";
            }


        }

        private bool InsertarObjetoEnPlano(string IdObjeto, string cantidad, string precioVenta, string observaciones)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Realizamos la Inserción 
                    string query = "INSERT INTO tblPlano_Panel (Id_PLano, Id_PanelNum, Cantidad, Observaciones, Precio_Venta, RevisadoDibujo)  " +
                                   "VALUES (@Id_PLano, @Id_PanelNum, @Cantidad, @Observaciones, @PrecioVenta, 0)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id_PLano", Session["Numero_Plano"].ToString());
                        command.Parameters.AddWithValue("@Id_PanelNum", IdObjeto);
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@PrecioVenta", precioVenta);
                        command.Parameters.AddWithValue("@Observaciones", observaciones);

                        command.ExecuteNonQuery();
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        [WebMethod]
        public static void ControlTapPlano()
        {
            HttpContext.Current.Session["controlTapPlano"] = "1";
        }

        protected void BtnModPan_Click(object sender, EventArgs e)
        {
            if (!ValidarObjetoChequeado())
            {
                string url = "~/Formularios/DiseñoYDesarrollo/ObjetosDibujo.aspx";
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

                Session["Id_numericoDise"] = Session["Id_PanelNum_Session"].ToString();
                Session["CrudObjetosDibujo"] = "Modificar";
            }
            else
            {
                string scriptChequeado = $"alert('El objeto se encuentra chequeado(bloqueado), no puede modificarlo.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoAgregado", scriptChequeado, true);
            }
        }

        protected void CopiarPanel_Click(object sender, EventArgs e)
        {
            string url = "~/Formularios/DiseñoYDesarrollo/ObjetosDibujo.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

            Session["Id_numericoDise"] = Session["Id_PanelNum_Session"].ToString();
            Session["CrudObjetosDibujo"] = "Copiar";
        }

        public bool ValidarObjetoChequeado()
        {
            bool chequeado = false;


            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Consulta SQL
            string query = "SELECT Chequeado FROM tblPanel WHERE Id_Numerico = @IdNumerico";

            // Usamos un bloque using para asegurarnos de liberar los recursos correctamente
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregamos el parámetro a la consulta
                    command.Parameters.AddWithValue("@IdNumerico", Session["Id_PanelNum_Session"].ToString());

                    try
                    {
                        // Abrimos la conexión
                        connection.Open();

                        // Ejecutamos la consulta y obtenemos el valor
                        var result = command.ExecuteScalar();

                        // Si el resultado no es nulo, convertimos el valor a booleano
                        if (result != null)
                        {
                            chequeado = Convert.ToBoolean(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores (puedes registrar el error o manejarlo según tus necesidades)
                        Console.WriteLine("Error al obtener el valor de Chequeado: " + ex.Message);
                    }
                }
            }

            return chequeado;
        }

        protected void BtnNuePan_Click(object sender, EventArgs e)
        {

            string url = "~/Formularios/DiseñoYDesarrollo/ObjetosDibujo.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

            Session.Remove("Id_numericoDise");
            Session["CrudObjetosDibujo"] = "Nuevo";
        }

        protected void BtnEliPan_Click(object sender, EventArgs e)
        {
            if (SpanId_NombreObjetoEliminar.InnerText.Trim() != "")
            {
                if (ValidarObjetoEnOrdenTrabajo())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El Objeto " + SpanId_NombreObjetoEliminar.InnerText + " está vinculado a una o más órdenes de trabajo y no se puede eliminar.');", true);
                    return;
                }
                else
                {

                    // Modal confirmar eliminar objeto 
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModalConfirmarEliminar", "MostrarModalConfirmarEliminar();", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Por favor seleccione un objeto a eliminar.');", true);
                return;
            }
        }

        public bool ValidarObjetoEnOrdenTrabajo()
        {
            bool existe = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT 1 FROM tblPlano INNER JOIN tblPlano_Panel ON tblPlano.Plano = tblPlano_Panel.Id_Plano " +
                               "WHERE (((tblPlano.Id_OT)<>'Nula') AND ((tblPlano_Panel.Id_PanelNum)= @ID_Numerico))";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Numerico", Session["Id_PanelNum_Session"].ToString());

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

        protected void btnEliminarObjeto_SI_Click(object sender, EventArgs e)
        {
            if (EliminarObjeto())
            {
                //Eliminado
                BuscarObjeto(sender, e);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El Objeto se ha eliminado exitosamente.');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('El objeto no ha sido eliminado.');", true);
                return;
                //No Eliminado
            }
        }

        private bool EliminarObjeto()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM tblPanel WHERE Id_Panel = @IdObjeto AND Ancho = @Ancho AND Altura = @Altura ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdObjeto", SpanId_NombreObjetoEliminar.InnerText);
                        command.Parameters.AddWithValue("@Ancho", spanAnchoEliminar.InnerText);
                        command.Parameters.AddWithValue("@Altura", spanAlturaEliminar.InnerText);




                        int filasAfectadas = command.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}