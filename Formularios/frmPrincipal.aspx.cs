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
	public partial class OrdenesDeTrabajo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Cargar_OT();

		}

		public void Cargar_OT()
		{
			//Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
			string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
			SqlConnection sqlconectar = new SqlConnection(cn);
			SqlCommand cmd = new SqlCommand("ctaOT", sqlconectar)
			{
				CommandType = CommandType.StoredProcedure
			};
			cmd.Connection.Open();
			cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = tbOT.Text;
			cmd.Parameters.Add("@Con", SqlDbType.VarChar, 30).Value = tbPedido.Text;
			SqlDataReader dr = cmd.ExecuteReader();
			if (dr.Read())
			{
				tbOT.Text = dr["Id_OT"].ToString();
				tbPedido.Text = dr["Consecutivo_Pedido"].ToString();
				tbZona.Text = dr["Zona"].ToString();
				tbTped.Text = dr["Id_TipoPedido"].ToString();
				tbPedBase.Text = dr["PedidoBase"].ToString();
				tbAprob.Text = dr["TipoAprobacion"].ToString();
				tbObra.Text = dr["Nombre_Obra"].ToString();
				tbDir.Text = dr["Dirección"].ToString();
				tbContac.Text = dr["Persona_Receptora"].ToString();
				tbEmail.Text = dr["mail_Contacto"].ToString();
				tbRecibe.Text = dr["RecibeElPedido"].ToString();
				tbCiudad.Text = dr["Ciudad"].ToString();
				tbTel.Text = dr["TelDomicilio"].ToString();
				tbCel.Text = dr["CelularContacto"].ToString();
				tbPais.Text = dr["País"].ToString();
				tbVenta.Text = dr["Fecha_Confirmacion_Venta"].ToString();

			}
			//if (!IsPostBack)
			//{
			//	using (SqlConnection conn=new SqlConnection(ConfigurationManager.ConnectionStrings["ListI_Connection"].ConnectionString))
			//	{
			//		SqlCommand cmd = new SqlCommand();
			//		cmd.CommandType = CommandType.StoredProcedure;
			//		cmd.CommandText = "ctaInsumos";
			//		cmd.Connection= conn;
			//		conn.Open();
			//		GridView1.DataSource = cmd.ExecuteReader();
			//		GridView1.DataBind();
			//	}
			//}
		}
	}
}








	

