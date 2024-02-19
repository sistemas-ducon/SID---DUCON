using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Login
{
    public partial class Login : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
		{
			
		}


		protected void btbIngresar_Click(object sender, EventArgs e)
		{
			//Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
			string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
			SqlConnection sqlconectar = new SqlConnection(cn);
			SqlCommand cmd = new SqlCommand("ctaIngreso_SIDucon", sqlconectar)
			{
				CommandType = CommandType.StoredProcedure
			};
			cmd.Connection.Open();
			cmd.Parameters.Add("@Log", SqlDbType.VarChar, 30).Value = tbUsuario.Text;
			cmd.Parameters.Add("@Pass", SqlDbType.VarChar, 30).Value = tbPassword.Text;
			SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string login = tbUsuario.Text;
                string password = tbPassword.Text;

                // Obtener el Nombre del empleado basado en las credenciales ingresadas
                string nombreEmpleado = ObtenerNombreEmpleado(login, password);

                if (!string.IsNullOrEmpty(nombreEmpleado))
                {
                    // Establecer la variable de sesión 'usuariologueado' con el Nombre del empleado obtenido
                    Session["usuariologueado"] = nombreEmpleado;
                    CedulaUsuarioLogeado();
                    Response.Redirect("Inicio.aspx");
                }
                else
                {
                    lblError.Text = "Credenciales inválidas";
                }
            }
            else
            {
                lblError.Text = "Ingrese sus credenciales";
            }

            cmd.Connection.Close();
        }

        public string ObtenerNombreEmpleado(string login, string password)
        {
            string nombreEmpleado = string.Empty;

            string cn = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection sqlconectar = new SqlConnection(cn))
            {
                SqlCommand cmd = new SqlCommand("SELECT CONCAT(Nombre, ' ', Apellidos) AS Nombre FROM tblEmpleado WHERE Login = @Login AND Password = @Password", sqlconectar);
                cmd.Parameters.Add("@Login", SqlDbType.VarChar, 30).Value = login;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 30).Value = password;

                sqlconectar.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    nombreEmpleado = result.ToString();
                }
            }

            return nombreEmpleado;
        }


        public void  CedulaUsuarioLogeado()
        {

            string consultaActual = "Select Cedula, Zona from tblEmpleado where  Login = @Login";
                                  
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(consultaActual, connection))
                {
                    command.Parameters.AddWithValue("@Login",tbUsuario.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read(); // Mueve el lector al primer registro devuelto.

                        string CedulaLogeada = reader["Cedula"].ToString();
                        string ZonaLogeada = reader["Zona"].ToString();

                        reader.Close();

                        Session["CedulaLogeada"] = CedulaLogeada;
                        Session["ZonaLogeada"] = ZonaLogeada;
                    }
                    
                   
                }
            }

        }



        //login();


        //private void login()
        //      {


        //}




        //         SqlConnection cn = new SqlConnection("Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL;User ID=pcadmin;Password=password");
        //         cn.Open();
        //         SqlCommand cm = new SqlCommand("select Login,Password from tblEmpleado where Login='" + tbUsuario.Text + "' and Password= '" + tbPassword.Text + "'", cn);
        //         SqlDataReader dr = cm.ExecuteReader();
        //         if (dr.Read())
        //         {

        //             Response.Redirect("Inicio.aspx");

        //         }
        //         else
        //         {
        //	Response.Redirect("Login.aspx");

        //}





    }

}