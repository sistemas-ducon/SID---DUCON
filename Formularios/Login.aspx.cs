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
        protected void Page_Load(object sender, EventArgs e)
		{
			
		}


		protected void btbIngresar_Click(object sender, EventArgs e)
		{
			//Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
			string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
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

				//Agregamos la vista que queremos mostrar al logearseSession
				Session["usuariologueado"] = tbUsuario.Text;
                CedulaUsuarioLogeado();
				Response.Redirect("Inicio.aspx");
			}
			else
			{

				lblError.Text = "Ingrese sus credenciales";
			}
			cmd.Connection.Close();	
		}


        public void  CedulaUsuarioLogeado()
        {

            string consultaActual = "Select Cedula from tblEmpleado where  Login = @Login";
                                  
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(consultaActual, connection))
                {
                    command.Parameters.AddWithValue("@Login",tbUsuario.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        // Data arrived.
                        string CedulaLogeada = (string)command.ExecuteScalar();
                        Session["CedulaLogeada"] = CedulaLogeada;
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