using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class NitOTs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarActividadesEnDropDownList();
            CargarCiudadesEnDropDownList();
        }

        protected void DatagridClientes_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerContactoCliente")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DatagridClientes.Items[rowIndex];

                // Accede al DataView del DataSource para obtener la fila correspondiente
                DataView dataView = (DataView)ClientesFacturacion.Select(DataSourceSelectArguments.Empty);

                if (dataView != null && dataView.Count > rowIndex)
                {
                    DataRowView rowView = dataView[rowIndex];

                    DateTime fechaCreacion = Convert.ToDateTime(rowView["FechaCreacion"]);
                    DateTime fechaUul = Convert.ToDateTime(rowView["UltimaActualizacion"]);
                    string compartidCon = rowView["CompartidoCon"].ToString();
                    string naturaleza = rowView["Naturaleza"].ToString();
                    string tipoDoc = rowView["Tipo_Documento"].ToString();
                    string id = rowView["Nit"].ToString();
                    string tipoCli = rowView["Tipo_Cliente"].ToString();
                    string actividad = rowView["Actividad"].ToString();
                    string telefono = rowView["Telefono"].ToString();
                    string sector = rowView["Sector"].ToString();
                    string razonSocial = rowView["RazonSocial"].ToString();
                    string direccion = rowView["Direccion"].ToString();
                    string ciudad = rowView["Ciudad"].ToString();
                    //string codigo = rowView["RazonSocial"].ToString();
                    string Pago = rowView["Forma_Pago"].ToString();
                    string Zona = rowView["Zona"].ToString();
                    string fax = rowView["Fax"].ToString();

                    tbFechaCreacion.Text = fechaCreacion.ToString("yyyy-MM-dd");
                    tbUltimaAct.Text = fechaUul.ToString("yyyy-MM-dd");
                    tbCompartido.Text = compartidCon;
                    foreach (ListItem item in ddlNaturaleza.Items)
                    {
                        if (item.Text == naturaleza)
                        {
                            ddlNaturaleza.ClearSelection();
                            item.Selected = true;
                            break;
                        }
                    }
                    foreach (ListItem item in ddlTipoDoc.Items)
                    {
                        if (item.Text == tipoDoc)
                        {
                            ddlTipoDoc.ClearSelection();
                            item.Selected = true;
                            break;
                        }
                    }
                    tbNumero.Text = id;
                    foreach (ListItem item in ddlTipoCliente.Items)
                    {
                        if (item.Text == tipoCli)
                        {
                            ddlTipoCliente.ClearSelection();
                            item.Selected = true;
                            break;
                        }
                    }
                    foreach (ListItem item in ddlActividad.Items)
                    {
                        if (item.Text == actividad)
                        {
                            ddlActividad.ClearSelection();
                            item.Selected = true;
                            break;
                        }
                    }
                    tbTelefono.Text = telefono;
                    foreach (ListItem item in ddlSector.Items)
                    {
                        if (item.Text == sector)
                        {
                            ddlSector.ClearSelection();
                            item.Selected = true;
                            break;
                        }
                    }
                    tbRazonSocial.Text = razonSocial;
                    tbDireccion.Text = direccion;
                    foreach(ListItem item1 in ddlCiudad.Items)
                    {
                        if(item1.Text == ciudad)
                        {
                            ddlCiudad.ClearSelection();
                            item1.Selected = true;
                            break;
                        }
                    }              
                    foreach (ListItem item1 in ddlFormaPago.Items)
                    {
                        if (item1.Text == Pago)
                        {
                            ddlFormaPago.ClearSelection();
                            item1.Selected = true;
                            break;
                        }
                    }
                    foreach (ListItem item1 in ddlZona.Items)
                    {
                        if (item1.Text == Zona)
                        {
                            ddlZona.ClearSelection();
                            item1.Selected = true;
                            break;
                        }
                    }
                    tbFax.Text = fax;                 
                    tbCod.Text = ObtenerSubcadena( ciudad);



                    // Script para ocultar y mostrar la fila razon social 
                    string script = "ocultarMostrarFilas();";
                    ScriptManager.RegisterStartupScript(this, GetType(), "OcultarMostrarScript", script, true);


                    DataGridContacto.DataBind();
                    DataGridVentaAsesor.DataBind();
                    PanelContacto.Update();

                }
            }

        }

        static string ObtenerSubcadena(string cadena)
        {
            
            int indiceEspacio = cadena.LastIndexOf(' ');

            if (indiceEspacio != -1)
            {
                string subcadena = cadena.Substring(indiceEspacio + 1);
                subcadena = subcadena.Trim();
                return subcadena;
            }

            
            return cadena;
        }



        private void CargarActividadesEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "select Id, CONCAT(aecCodigo, ' - ',aecDescripcion) As Descripcion from tblActividadEconomica order by aecCodigo asc";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlActividad.DataSource = reader;
                ddlActividad.DataTextField = "Descripcion";
                ddlActividad.DataValueField = "Id";

                ddlActividad.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlActividad.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        private void CargarCiudadesEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = " SELECT concat(tblDepartamentoPais.CodigoDepartamento , tblCiudad.CodigoCiudad)   AS Codigo," +
                                  " concat(tblCiudad.NombreCiudad , ' - ' , tblDepartamentoPais.NombreDepartamento, " +
                                  "concat('  Cod: ',tblDepartamentoPais.CodigoDepartamento , tblCiudad.CodigoCiudad)) AS Ciudad" +
                                  " FROM tblDepartamentoPais " +
                                  " INNER JOIN tblCiudad ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento" +
                                  " ORDER BY tblCiudad.NombreCiudad asc, tblDepartamentoPais.NombreDepartamento asc";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlCiudad.DataSource = reader;
                ddlCiudad.DataTextField = "Ciudad";
                ddlCiudad.DataValueField = "Codigo";

                ddlCiudad.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlCiudad.Items.Insert(0, new ListItem("Seleccione", "0"));
        }


    }
}