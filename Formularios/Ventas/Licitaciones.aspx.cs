using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Licitaciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }


        // Marca el método como un servicio web
        [WebMethod]
        public static List<NombreCompletoItem> Ejemplo()
        {
            List<NombreCompletoItem> nombres = new List<NombreCompletoItem>();

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Agrega el nombre completo al listado
                    nombres.Add(new NombreCompletoItem
                    {
                        Cedula = reader["Cedula"].ToString(),
                        NombreCompleto = reader["NombreCompleto"].ToString()
                    });
                }

                reader.Close();
            }

            return nombres; // Retorna la lista de nombres
        }

        // Clase para representar el objeto de nombre completo (Cedula y NombreCompleto)
        public class NombreCompletoItem
        {
            public string Cedula { get; set; }
            public string NombreCompleto { get; set; }
        }

    }
}