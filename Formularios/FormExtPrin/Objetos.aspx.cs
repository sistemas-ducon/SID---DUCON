using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SISTEMA_INTEGRAL_DUCON.Formularios.OrdenTrabajo;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class Objetos : System.Web.UI.Page
    {

        private List<TextBox> listaTextBoxes;
        protected void Page_Load(object sender, EventArgs e)
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
                Nombre_Objeto_Hid.Value = row.Cells[2].Text;
                Ancho_Objeto_Hid.Value = row.Cells[3].Text;
                Id_Objeto_Hid.Value = row.Cells[7].Text;

                tbPrecioVenta.Text = row.Cells[8].Text;
            }
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

                // Llamar el script que recarga el formulario padre de donde salio la pagina 
                string script = "<script>enviarFormulario();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "enviarFormulario", script, false);


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
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

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

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

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
                Adicionar.CssClass = "bi bf btn btn-lg btn-outline-primary";
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
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

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


        // Pendiente los Botones de la barra principal (Definir Funcionalidades y Autorizacion)



    }
}