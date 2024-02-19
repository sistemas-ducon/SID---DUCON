using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static SISTEMA_INTEGRAL_DUCON.Formularios.Licitaciones;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Licitaciones : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void btn1_Click(object sender, EventArgs e)
        {

            // Validamos si el campo esta vacio para ejecurar un sqldatasource sino usamoos el otr 

            if (tbLicitacion1.Text != "")
            {
                DataGrid1.DataSourceID = "LicitacionProceso2";
                DataGrid1.DataBind();

            }
            else
            {

                DataGrid1.DataSourceID = "LicitacionProceso";
                DataGrid1.DataBind();
            }


        }

        protected void dataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)

        {

            if (e.CommandName == "Ver")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid1.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 





                string CodigoLicita = row.Cells[1].Text;
                string NombreCompañia = row.Cells[2].Text;
                string FechaApertura = row.Cells[3].Text;
                DateTime FechaAperturaCon = DateTime.Parse(FechaApertura);
                string FechaClausura = row.Cells[4].Text;
                DateTime FechaClausuraCon = DateTime.Parse(FechaClausura);
                string Valor = row.Cells[5].Text;
                string Presupuesto = row.Cells[6].Text;
                string LimObs = row.Cells[7].Text;
                DateTime LimObsCon = DateTime.Parse(LimObs);
                string Adendas = row.Cells[8].Text;
                DateTime AdendasCon = DateTime.Parse(Adendas);
                string Proceso = row.Cells[9].Text;
                string Asesor = row.Cells[10].Text;
                string Contacto = row.Cells[11].Text;
                string Telefono = row.Cells[12].Text;
                string CelularCon = row.Cells[13].Text;
                string mail = row.Cells[14].Text;
                string id = row.Cells[15].Text;
                string FechaRegistro = row.Cells[16].Text;
                DateTime FechaRegistroCon;
                if (DateTime.TryParse(FechaRegistro, out FechaRegistroCon))
                {
                    // La conversión se realizó correctamente
                }
                else
                {
                    // La conversión falló, FechaRegistroCon se establecerá como DateTime.MinValue
                    FechaRegistroCon = DateTime.MinValue;
                }

                string EstadoLic = row.Cells[17].Text;
                string DirEntrega = row.Cells[18].Text;
                string CiudadEntrega = row.Cells[19].Text;
                string link = row.Cells[20].Text;
                string NombreProyecto = row.Cells[21].Text;
                string DireccionPro = row.Cells[22].Text;
                string CiudadProyecto = row.Cells[23].Text;
                string Observacion = row.Cells[24].Text;
                string Causa = row.Cells[25].Text;


                // Capturamos los datos que tiene en data grid en un arreglo 
                string[] campos = {
                    CodigoLicita, NombreCompañia, FechaApertura, FechaClausura,
                    Valor, Presupuesto, LimObs, Adendas, Proceso, Asesor,
                    Contacto, Telefono, CelularCon, mail, id, FechaRegistro,
                    EstadoLic, DirEntrega, CiudadEntrega, link, NombreProyecto,
                    DireccionPro, CiudadProyecto, Observacion, Causa
                };

                // Recorremos  todos los campos y reemplazar &nbsp; por nulos o valores vacíos
                for (int i = 0; i < campos.Length; i++)
                {
                    campos[i] = campos[i].Replace("&nbsp;", null);
                }





                // Enviamos esos datos a los campos textBox y Dropdownlist 

                tbLicitacion.Text = campos[0];
                tbCampoBlanco.Text = campos[1];
                tbApertura.Text = FechaAperturaCon.ToString("yyyy-MM-dd");
                tbClausura.Text = FechaClausuraCon.ToString("yyyy-MM-dd");
                tbValor.Text = campos[4];
                tbPresupuesto.Text = campos[5];
                tbObs.Text = LimObsCon.ToString("yyyy-MM-dd");
                tbAdendas.Text = AdendasCon.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(campos[8]))
                {
                    ListItem item = ddlProceso.Items.FindByValue(campos[8]);
                    if (item != null)
                    {
                        ddlProceso.ClearSelection();
                        item.Selected = true;
                    }
                    else
                    {
                        // El valor de campos[8] no está en la lista dejamos la lists vacia 
                        ddlProceso.ClearSelection(); // Deseleccionar en este caso
                    }
                }
                else
                {
                    ddlProceso.ClearSelection(); // Valor nulo, deseleccionar
                }


                if (!string.IsNullOrEmpty(campos[9]))
                {
                    ListItem item = ddlAsesor.Items.FindByValue(campos[9]);
                    if (item != null)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true;
                    }
                    else
                    {
                        ddlAsesor.ClearSelection();
                    }
                }
                else
                {
                    ddlAsesor.ClearSelection();
                }



                tbContacto1.Text = campos[10];
                tbtelefono.Text = campos[11];
                tbCelular.Text = campos[12];
                tbMail.Text = campos[12];
                tbID.Text = campos[14];
                tbFRegistro.Text = FechaRegistroCon.ToString("yyyy-MM-dd");

                if (!string.IsNullOrEmpty(campos[17]))
                {
                    ListItem item = ddlEstado.Items.FindByValue(campos[17]);
                    if (item != null)
                    {
                        ddlEstado.ClearSelection();
                        item.Selected = true;
                    }
                    else
                    {
                        ddlEstado.ClearSelection();
                    }
                }
                else
                {
                    ddlEstado.ClearSelection();
                }




                tbDirEntrega.Text = campos[17];



                if (!string.IsNullOrEmpty(campos[18]))
                {
                    ListItem item = ddlCiudad.Items.FindByValue(campos[18]);
                    if (item != null)
                    {
                        ddlCiudad.ClearSelection();
                        item.Selected = true;
                    }
                    else
                    {
                        ddlCiudad.ClearSelection();
                    }
                }
                else
                {
                    ddlCiudad.ClearSelection();
                }

                tbLink.Text = campos[19];
                tbProyecto.Text = campos[20];
                tbDirProyecto.Text = campos[21];

                if (!string.IsNullOrEmpty(campos[22]))
                {
                    ListItem item = ddlCiudadP.Items.FindByValue(campos[22]);
                    if (item != null)
                    {
                        ddlCiudadP.ClearSelection();
                        item.Selected = true;
                    }
                    else
                    {
                        ddlCiudadP.ClearSelection();
                    }
                }
                else
                {
                    ddlCiudadP.ClearSelection();
                }

                tbObsGen.InnerText = campos[23];

                if (!string.IsNullOrEmpty(campos[24]))
                {
                    ListItem item = ddlCausa.Items.FindByValue(campos[24]);
                    if (item != null)
                    {
                        ddlCausa.ClearSelection();
                        item.Selected = true;
                    }
                    else
                    {
                        ddlCausa.ClearSelection();
                    }
                }
                else
                {
                    ddlCausa.ClearSelection();
                }


                // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                string script = "<script>HabilitarEnlaces1();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);


            }
        }

        protected void ddlAsesores_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlAsesor.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlEstado_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlEstado.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlProceso_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlProceso.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlCiudad_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlCiudad.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlCiudad1_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlCiudadP.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlCausa_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlCausa.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlEstado1_DataBound(object sender, EventArgs e)
        {
            // Agregar el elemento deseado en la primera posición del DropDownList
            ddlEstado1.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void Grabar(object sender, EventArgs e)
        {

            bool estado = estadoLicitacion.Checked;

            if (estado == true)
            {
                // insertar
            }
            else
            {
                //actualizar 
            }

        }




    }



}