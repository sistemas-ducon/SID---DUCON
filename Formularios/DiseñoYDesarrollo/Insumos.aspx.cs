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
using DataGrid = System.Web.UI.WebControls.DataGrid;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
	public partial class Insumos : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "RedirectToInsumoConsultar")
            {
                int index = e.Item.ItemIndex;
                DataGrid grid = (DataGrid)source;
                string idInsumo = grid.DataKeys[index].ToString();

                // Redirigir a Insumo_consultar.aspx y pasar el valor del Id_Insumo en la URL
                Response.Redirect("Insumos_Consultar.aspx?Id_Insumo=" + idInsumo);
            }
        }


    }
}
