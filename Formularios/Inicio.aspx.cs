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
        private string CadenaConexionSID = "BD_SIDSQL";
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

        protected void GerenciaComercial_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                case "EstadisticaVentas":
                    pageURL = "Administrativo/EstadisticasVentas.aspx";
                    break;
                case "SeguimientoCotizaciones":
                    Session["SeguimientoCotizaciones"] = "GerenciaComercial";
                    pageURL = "Ventas/Consulta_Cotizacion.aspx";
                    break;

                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 12); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void Compras_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;               
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 5); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void Diseno_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "BitacoraDiseno":
                    Session["Diseno"] = "Diseño";
                    pageURL = "Ventas/Diseño_Venta.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
                case "BitacoraDesarrollo":
                    pageURL = "Ventas/Solicitud_Especial.aspx";
                    break;

                case "BitacoraRenders":
                    pageURL = "Ventas/Render_Venta.aspx";
                    break;

            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 2); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewPage", "window.open('" + pageURL + "', '_blank');", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);
            }
        }

        protected void DisenoExterior_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 16); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void FacturacionCartera_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 11); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void GestionCalidadAdnom_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 20); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void Instalacion_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 15); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void Recepcion_Click(object sender, EventArgs e)
        {


            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "IngresarCotizacion":
                    pageURL = "Recepcion/IngresarCotizacion.aspx";
                    break;
                case "TablaDiseños":
                    Session["Diseno"] = "Recepcion";
                    pageURL = "Ventas/Diseño_Venta.aspx";
                    break;

                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 4); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {


                ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void Sistemas_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 14); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void Reprocesos_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                case "OT_Manuales":
                    pageURL = "Consultas/OT_ManualesSid.aspx";
                    break;
                case "Reprocesos":
                    pageURL = "Consultas/Reproceso.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 42); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                string url = pageURL;
                string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
                ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);

            }
        }

        protected void ValidarPermiso_Ventas(object sender, EventArgs e)
        {


            LinkButton btn = (LinkButton)sender;

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
                    Session["Diseno"] = "Ventas";
                    pageURL = "Ventas/Diseño_Venta.aspx";
                    break;

                case "ProgramarRender":
                    pageURL = "Ventas/Render_Venta.aspx";
                    break;

                case "SeguimientoCotizacion":
                    Session["SeguimientoCotizaciones"] = "Ventas";
                    pageURL = "Ventas/Consulta_Cotizacion.aspx";
                    break;

                case "SolicitudProductoEspecial":
                    pageURL = "Ventas/Solicitud_Especial.aspx";
                    break;

                case "VisitaAsesores":
                    Session["AsesorDiseño"] = Session["CedulaLogeada"].ToString();
                    pageURL = "Ventas/Visita_Asesores.aspx";
                    break;

                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 1); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void ValidarPermisoCliente_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Clientes.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 6); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        protected void ValidarPermisoEmpleado_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string pageURL = string.Empty;

            switch (btn.CommandName)
            {
                case "PersonaCliente":
                    pageURL = "Ventas/Empleado.aspx";
                    break;
                default:
                    // Si no se encuentra el CommandName, se puede manejar el comportamiento predeterminado aquí
                    break;
            }

            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();
            bool tienePermiso = VerificarPermiso(cedulaLogueada, 9); // Pasar el número de permiso correspondiente

            if (!tienePermiso)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalError').modal('show');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "$('#miModalPendiente').modal('show');", true);

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "openNewTab", "window.open('" + pageURL + "', '_blank');", true);
            }
        }

        private bool VerificarPermiso(string cedulaLogueada, int idPermiso)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Aquí se debe ajustar la consulta SQL para incluir el parámetro del ID del permiso
            string query = $"SELECT COUNT(*) FROM tblPermiso_Empleado WHERE ID_Empleado = '{cedulaLogueada}' AND ID_Permiso = @Permiso";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Permiso", idPermiso); // Agregar el parámetro del ID del permiso
                    connection.Open();
                    int count = (int)command.ExecuteScalar(); // Ejecutar la consulta y obtener el resultado
                    return count > 0; // Devolver verdadero si se encuentra algún registro que cumpla la condición
                }
            }
        }



    }
}