using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Insumos_Consultar : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificar si se pasó el parámetro Id_Insumo en la URL
                if (Request.QueryString["Id_Insumo"] != null)
                {
                    // Obtener el valor del parámetro y asignarlo al TextBox
                    string idInsumo = Request.QueryString["Id_Insumo"];
                    txtInsumos.Text = idInsumo;
                }

            }

            CargarDatos();

        }

       




        public void CargarDatos()
        {
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("ctaInsumo", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();

            cmd.Parameters.Add("@Insumo", SqlDbType.VarChar, 30).Value = txtInsumos.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                TextBox2.Text = dr["FechaActualizacion"].ToString();
                TextBox3.Text = dr["Responsable"].ToString();
                TextBox4.Text = dr["Descripcion_Insumo"].ToString();
                TextBox5.Text = dr["Descripcion"].ToString();
                TextBox6.Text = dr["Abreviado"].ToString();
                TextBox7.Text = dr["Valor_Unitario"].ToString();
                TextBox8.Text = dr["AplicacionAcabado"].ToString();
                TextBox9.Text = dr["ID_Inventario"].ToString();
                TextBox10.Text = dr["Factor_Ganancia"].ToString();
                TextBox11.Text = dr["Factor_Desperdicio"].ToString();
                TextBox12.Text = dr["PesoKG"].ToString();
                TextBox13.Text = dr["UndxPaquete"].ToString();
            }




        }

        
    }
}