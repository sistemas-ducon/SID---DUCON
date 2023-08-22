using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Diseño_Venta : System.Web.UI.Page
    {
        private bool isModalVisible = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckBox22.Checked = isModalVisible;
                if (Session["usuariologueado"] != null)
                {
                    string usuariologueado = Session["usuariologueado"].ToString();

                    // Realizar la conexión a la base de datos y la consulta para obtener el nombre y apellido del usuario
                    string connectionString = "Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT Nombre, Apellidos FROM tblEmpleado WHERE Login = @nombreUsuario";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@nombreUsuario", usuariologueado);
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                string nombre = reader["Nombre"].ToString();
                                string apellidos = reader["Apellidos"].ToString();
                                TextBox5.Text = nombre + " " + apellidos; // Asignar el nombre y apellidos al TextBox                              
                            }

                        }
                    }

                }
                else
                {
                    Response.Redirect("/Formularios/Login.aspx");
                }     
            }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button9
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button10
        }

        //DATAGRID DISEÑO
        protected void DataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string programadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();
                string pasarACotizar = DataBinder.Eval(e.Item.DataItem, "PasarACotizar").ToString();
                string terminadoDibujo = DataBinder.Eval(e.Item.DataItem, "TerminadoDibujo").ToString();
                string pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();

                if (programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else if (programadoVentas == "True" && pasarACotizar == "True" && terminadoDibujo == "True")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /*Violeta*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else if (programadoVentas == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#819cba"); /*Azul*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else if (programadoVentas == "True" && pasarACotizar == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#be94b9"); /*Rosado*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }

                else if (pausado == "True")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#74bec6"); /*Celeste*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }




                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                
                TableCell descripcionCell = e.Item.Cells[2];

               
                if (descripcionCell != null)
                {
                    descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                }
            }
        }


        //DATAGRID SHOWCASE
        protected void DataGrid3_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string scimaganes = DataBinder.Eval(e.Item.DataItem, "SC_Imagenes").ToString();
                string scterminado = DataBinder.Eval(e.Item.DataItem, "SC_Terminado").ToString();

                if (scimaganes == "True" && scterminado == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }

            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                    string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                    string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                    // Encontrar la celda correspondiente a la columna Descripción por índice
                    TableCell descripcionCell = e.Item.Cells[2]; // Ajusta el índice si es necesario

                    // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                    if (descripcionCell != null)
                    {
                        descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                    }
                }
            }
        
            protected void DataGrid4_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string terminadoRender = DataBinder.Eval(e.Item.DataItem, "TerminadoRender").ToString();
                string fechaProgramadaString = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();

                if (terminadoRender == "False")
                {
                    if (!string.IsNullOrEmpty(fechaProgramadaString) && DateTime.TryParse(fechaProgramadaString, out DateTime fechaProgramada))
                    {
                        if (fechaProgramada < DateTime.Now)
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868");
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }
                        else if (fechaProgramada > DateTime.Now)
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            }



            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
               string nombreRender = DataBinder.Eval(e.Item.DataItem, "Nombre_Render").ToString();

                // Encontrar la celda correspondiente a la columna Descripción por índice
               TableCell descripcionCell = e.Item.Cells[2]; // Ajusta el índice si es necesario
                                
                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
               if (descripcionCell != null)
                {
                  descripcionCell.Text = $"{cliente} - {nombreRender}";
               }
            }
        }
        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DateTime fechaEntrega = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Entrega_Dibujo_Despiece"));
                DateTime fechaActual = DateTime.Now;

                if (fechaEntrega < fechaActual)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#efdd79");
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868");
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
            }
        }

        protected void CheckBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox22.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }


    }
}