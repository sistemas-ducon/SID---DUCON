using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Visita_Asesores : System.Web.UI.Page
    {
        // Variables de clase para almacenar las fechas seleccionadas
        private DateTime fechaInicioSeleccionada;
        private DateTime fechaFinSeleccionada;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Llamar al método para cargar los datos en el DropDownList
                CargarAsesoresEnDropDownList();
                // Aquí puedes obtener y mostrar el rango de fechas en el título del DataGrid
                DateRangeLiteral.Text = GetDateRange();


            }
        }
        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial WHERE activo =1 order by Nombre"; // Reemplaza por tu consulta SQL y tabla de datos

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto"; // Campo que se mostrará en el DropDownLi
                ddlAsesor.DataValueField = "Cedula";

                ddlAsesor.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }
       

        protected void ActualizarTituloDataGrid()
        {
            // Verificamos que ambas fechas estén seleccionadas antes de actualizar el título.
            if (fechaInicioSeleccionada != DateTime.MinValue && fechaFinSeleccionada != DateTime.MinValue)
            {
                DateRangeLiteral.Text = "Desde " + fechaInicioSeleccionada.ToString("dd/MM/yyyy") + " hasta " + fechaFinSeleccionada.ToString("dd/MM/yyyy");
            }
        }

        protected string GetDateRange()
        {
            string fechaInicio = fecha1.Text;
            string fechaFin = fecha2.Text;

            // Puedes personalizar el formato de las fechas según tus necesidades.
            return "Desde " + fechaInicio + " hasta " + fechaFin;
        }




        public void Consultar(object sender, EventArgs e)
        {
            ActualizarTituloDataGrid();
            DateRangeLiteral.Text = GetDateRange();

        }

       

    }
}