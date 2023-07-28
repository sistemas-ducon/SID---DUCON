using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Consulta_Cotizacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (Session["usuariologueado"] != null)
                {
                    string usuariologueado = Session["usuariologueado"].ToString();

                    // Realizar la conexión a la base de datos y la consulta para obtener el nombre y apellido del usuario
                    string connectionString = "Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT Nombre, Apellidos FROM tblEmpleado WHERE Login = @nombreUsuario";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@nombreUsuario", usuariologueado);
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                string nombre = reader["Nombre"].ToString();
                                string apellidos = reader["Apellidos"].ToString();
                                TextAsesor.Text = nombre + " " + apellidos; // Asignar el nombre y apellidos al TextBox
                                TextAsesortab2.Text = nombre + " " + apellidos;
                                TextAsesorSeguimiento.Text = nombre + " " + apellidos;
                            }
                            else
                            {
                                // El usuario no fue encontrado en la tabla tblEmpleado, puedes manejar esta situación según tus necesidades
                            }
                        }
                    }
                }

                else
                {
                    Response.Redirect("/Formularios/Login.aspx");
                }
                DataGrid2.DataBind();
             

                LoadEstados();
                CargarDatosTotales();
            }
        }
        

        protected void LoadEstados()
        {
         
                string connectionString = "Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"; 
                string query = "SELECT Descripción_Estado FROM tblEstado_Cotización";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        ddlEstadoCotizacion.DataSource = reader;
                        ddlEstadoCotizacion.DataTextField = "Descripción_Estado";
                        ddlEstadoCotizacion.DataBind();

                        connection.Close();
                    }
                }

                // Agrega un ítem de selección por defecto si lo deseas:
                ddlEstadoCotizacion.Items.Insert(0, new ListItem("Selecciona un estado", ""));
            }


        protected void BtnConsultarTab2_Click(object sender, EventArgs e)
        {
            // Obtén el valor seleccionado del DropDownList
            string estadoSeleccionado = ddlEstadoCotizacion.SelectedValue;

            // Realiza la consulta utilizando el SqlDataSource
            DataGridPorEstado.SelectCommand = "cta_Cotizaciones_Por_Estado"; // Nombre del nuevo procedimiento almacenado
            DataGridPorEstado.SelectParameters.Clear();
            DataGridPorEstado.SelectParameters.Add("NombreAsesor", TextAsesortab2.Text);
            DataGridPorEstado.SelectParameters.Add("FechaInicio", TextCotizacionEntreInicio2.Text);
            DataGridPorEstado.SelectParameters.Add("FechaFin", TextCotizacionEntreFinal2.Text);
            DataGridPorEstado.SelectParameters.Add("Estado", estadoSeleccionado);

            // Refresca los datos del DataGrid
            DataGridPorEstado.DataBind();
        }

        private void CargarDatosTotales()
        {
            // Crear un DataTable para almacenar los datos
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("");
            dt.Columns.Add("");
            dt.Columns.Add("");

            // Agregar filas de ejemplo (puedes reemplazar esto con tus datos reales)
            dt.Rows.Add("", "", "");
          
            // Asignar el DataTable como fuente de datos del GridView
            GridViewTotales.DataSource = dt;
            GridViewTotales.DataBind();
        }



    }
}