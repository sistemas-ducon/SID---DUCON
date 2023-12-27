using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Inicio
{
    public partial class Inicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();
                lblBienvenida.Text = "Bienvenid@ " + usuariologueado + "  |";
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }
        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
            Session.Remove("usuariologueado");
            Response.Redirect("Login.aspx");
        }


        protected void bOrdeDeTraba_Click(object sender, EventArgs e)
        {
            
		}
		protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
		{

		}

        protected void ValidarPermisos(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "GestionComercial":
                    pageURL = "Ventas/Gestion_Comercial.aspx";
                    break;
                case "Licitaciones":
                    pageURL = "Ventas/Licitaciones.aspx";
                    break;

                case "OrdendeTrabajo":
                    pageURL = "OrdenTrabajo.aspx";
                    break;      

                case "ProgramarDiseno":
                    pageURL = "Ventas/Diseño_Venta.aspx";
                    break;

                case "ProgramarRender":
                    pageURL = "Ventas/Render_Venta.aspx";
                    break;

                case "SeguimientoCotizacion":
                    pageURL = "Ventas/Consulta_Cotizacion.aspx";
                    break;

                case "SolicitudProductoEspecial":
                    pageURL = "Ventas/Solicitud_Especial.aspx";
                    break;

                case "VisitaAsesores":
                    pageURL = "Ventas/Visita_Asesores.aspx";
                    break;

                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada);

            if (tienePermiso)
            {
                Response.Redirect(pageURL);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
        }

        private bool VerificarPermiso(string cedulaLogueada)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            // Aquí debes ajustar tu consulta SQL para verificar los permisos
            string query = $"SELECT COUNT(*) FROM tblPermiso_Empleado WHERE ID_Empleado = '{cedulaLogueada}' AND ID_Permiso = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar(); // Ejecutar la consulta y obtener el resultado
                    return count > 0; // Devolver verdadero si se encuentra algún registro que cumpla la condición
                }
            }
        }


    }
}