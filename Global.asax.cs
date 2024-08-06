using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace SISTEMA_INTEGRAL_DUCON
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // Establecer el contexto de la licencia de EPPlus
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
           Session.Timeout = 120;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Obtener la última excepción
            Exception ex = Server.GetLastError();

            // Verificar si la excepción es un HttpException
            if (ex is HttpException httpException)
            {
                // Verificar si el código de estado HTTP es 400 (Bad Request)
                if (httpException.GetHttpCode() == 400)
                {
                    // Error de tamaño de solicitud excedido
                    string mensajePersonalizado = "Ha ocurrido un error. Por favor, inténtalo de nuevo. Si el problema persiste, ponte en contacto con soporte.";
                    string urlRedireccion = "Ventas/DocumentacionDise.aspx"; // Cambia esto por la URL correcta
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
                // Otros tipos de errores HTTP
                else
                {
                    // Error de tamaño de solicitud excedido
                    string mensajePersonalizado = "Ha ocurrido un error. Por favor, inténtalo de nuevo. Si el problema persiste, ponte en contacto con soporte.";
                    string urlRedireccion = "Inicio.aspx"; // Cambia esto por la URL correcta
                    Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
                }
            }
            else
            {
                // Manejar otros tipos de excepciones que no sean HttpException
                // Puedes registrar la excepción o redirigir a una página de error genérica
            }
        }


        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}