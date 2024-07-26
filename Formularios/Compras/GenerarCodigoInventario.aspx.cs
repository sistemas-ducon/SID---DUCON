using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Compras
{
    public partial class GenerarCodigoInventario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Session["usuariologueado"] != null)
                {

                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }

            }
        }
    }
}