using System;
using System.Collections.Generic;
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
	}
}