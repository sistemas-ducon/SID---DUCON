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
        private string id;
        private string pedido;

        protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if(!IsPostBack)
            {
                tbOT.Attributes.Add("onkeypress", "return handleEnter(event)");
                string usuariologueado = Session["usuariologueado"].ToString();

                if (Session["Id_OT"] != null && Session["pedido"] != null)
                {
                    id = Session["Id_OT"].ToString();
                    pedido = Session["pedido"].ToString();
                    Cargar_OT();
                }

            }
           


        }

        protected void tbOT_TextChanged(object sender, EventArgs e)
        {
            
            string id = tbOT.Text.Trim();
            Session["Id_OT"] = id;
          
            if (!string.IsNullOrEmpty(id))
            {
                
                    string inputData = tbOT.Text;
                    List<int> numeros = ObtenerNumerosDesdeLaBaseDeDatos(inputData);

                    ddlNumbers.Items.Clear(); // Limpiar las opciones existentes

                    foreach (int numero in numeros)
                    {
                        ddlNumbers.Items.Add(numero.ToString());
                    }
                
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Volver a llenar el combo en cada postback antes de renderizar la página
            string inputData = tbOT.Text;
            List<int> numeros = ObtenerNumerosDesdeLaBaseDeDatos(inputData);

            ddlNumbers.Items.Clear(); // Limpiar las opciones existentes

            foreach (int numero in numeros)
            {
                ddlNumbers.Items.Add(numero.ToString());
            }
            ddlNumbers.SelectedValue = pedido;
        }

        protected void ddlNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pedido = ddlNumbers.SelectedValue;
             Session["pedido"] = pedido;

            Cargar_OT();
        }

        private List<int> ObtenerNumerosDesdeLaBaseDeDatos(string dato)
        {
            List<int> numeros = new List<int>();

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString; ; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblOT WHERE Id_OT = @IdOT";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdOT", dato);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Supongamos que el número que deseas obtener está en la columna "Consecutivo_Pedido" de la tabla
                            int numero = reader.GetInt16(reader.GetOrdinal("Consecutivo_Pedido"));
                            numeros.Add(numero);
                        }
                    }
                }
            }

            return numeros;
        }

        public void Cargar_OT()
		{

            id = Session["Id_OT"].ToString();
            pedido = Session["pedido"].ToString();
            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
			SqlConnection sqlconectar = new SqlConnection(cn);
			SqlCommand cmd = new SqlCommand("ctaOT", sqlconectar)
			{
				CommandType = CommandType.StoredProcedure
			};
			cmd.Connection.Open();
			cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
			cmd.Parameters.Add("@Con", SqlDbType.VarChar, 30).Value = pedido;
			SqlDataReader dr = cmd.ExecuteReader();
			if (dr.Read())
			{
                tbOT.Text = dr["Id_OT"].ToString();
                ddlNumbers.Text = dr["Consecutivo_Pedido"].ToString();
                tbZona.Text = dr["Zona"].ToString();
                tbTped.Text = dr["Descripcion_TipoPedido"].ToString();
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
                if (DateTime.TryParse(dr["Fecha_Confirmacion_Venta"].ToString(), out DateTime fecha))
                {
                    tbVenta.Text = fecha.ToString("dd/MM/yyyy");
                }
                else
                {
                    // El valor no se pudo convertir a DateTime correctamente
                    // Puedes manejar el caso de error de alguna manera adecuada
                    tbVenta.Text = "Fecha inválida";
                }

                Observacion5Id.Value = dr["Observacion_Pedido"].ToString();
                Observacion1Id.Value = dr["Observacion_Dibujo"].ToString();
                tbSupervisor.Text = dr["Supervisor"].ToString();
                TextFabrica.Text = dr["FabricadoPor"].ToString();
                TextInstala.Text = dr["InstaladaPor"].ToString();

                txtcontacto.Text = dr["Persona_Receptora"].ToString();
                txtMail.Text = dr["mail_Contacto"].ToString();
                txtDireccion.Text = dr["Dirección"].ToString();
                txtMunicipio.Text = dr["Ciudad"].ToString();
                txtTelefono.Text = dr["TelDomicilio"].ToString();
                txtObs.Text = dr["Observaciones_Contables"].ToString();
                txtCotizacion.Text = dr["Cotizacion"].ToString();
                txtOrdenCompra.Text = dr["OrdendeCompra"].ToString();
                txtAsesor.Text = dr["Codigo_Asesor"].ToString();
                TextTNegociacion.Value = dr["Forma_Pago"].ToString();

            }
		
		}

        protected void DataGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}








	

