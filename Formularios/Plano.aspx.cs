using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Plano : System.Web.UI.Page
    {
        private string id;
        private string pedido;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

                if (Session["Id_OT"] != null && Session["pedido"] != null)
                {
                    id = Session["Id_OT"].ToString();
                    pedido = Session["pedido"].ToString();
                    Cargar_Plano();
                    Resumen_Plano();
                    Panel_Bajo();
                }

            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }//PageLoad

        public void Cargar_Plano()
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("CtaPlano_OT", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
            cmd.Parameters.Add("@Conse", SqlDbType.VarChar, 30).Value = pedido;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtPlano.Text = dr["Plano"].ToString();
                txtCliente.Text = dr["Nombre_Cliente"].ToString();
                txtArea.Text = dr["Area"].ToString();
                txtContacto.Text = dr["Contacto_Cliente"].ToString();
                txtAsesor.Text = dr["AsesorComercial"].ToString();
                txtDibuja.Text = dr["RealizadoPor"].ToString();
                txtBolsa.Text = dr["Bolsa"].ToString();

            }

        } //CargarPlano 

        public void Resumen_Plano()
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("CtaOT", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
            cmd.Parameters.Add("@Con", SqlDbType.VarChar, 30).Value = pedido;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txResumen.Value = dr["ResumenObra"].ToString();


            }
        }
        public void Panel_Bajo()
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("cta_Plano_Paneles", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@Plan", SqlDbType.VarChar, 30).Value = txtPlano.Text;

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtCantidad.Text = dr["Cantidad"].ToString();


            }
        }






    } //Clase Plano 


}//NameSpace
