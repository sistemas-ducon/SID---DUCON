using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using TableCell = System.Web.UI.WebControls.TableCell;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Render_Venta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Llamar al método para cargar los datos en el DropDownList
                CargarAsesoresEnDropDownList();


            }
        }

        protected void ddlAsesores_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlAsesor.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        protected void ddlZona2_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlZona2.Items.Insert(0, new ListItem("%", "0"));
        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial WHERE activo =1 order by Nombre";

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto"; // Campos que se mostrará en el DropDownLi
                ddlAsesor.DataValueField = "Cedula";

                ddlAsesor.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }

        protected void RenderPorZonaX(object sender, EventArgs e)
        {

            string valorSeleccionado = ddlZona2.SelectedValue;
            int numeroEntero = int.Parse(valorSeleccionado);
            CambiarSqlDataSource(numeroEntero);

        }

        private void CambiarSqlDataSource(int valorSeleccionado)
        {
            if (valorSeleccionado == 1)
            {
                DataGridRenders.DataSourceID = "RenderPorZona";
            }
            else if (valorSeleccionado == 2)
            {
                DataGridRenders.DataSourceID = "RenderPorZona";
            }
            else
            {
                DataGridRenders.DataSourceID = "CargarRenders";
            }

            DataGridRenders.DataBind();

        }


        protected void DataGridRenders_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                // Aplica la clase "fila-clickeable" a todas las filas
                e.Item.CssClass += " fila-clickeable";


                // Obtener los valores de las columnas ocultas
                int programadoVentas = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas"));
                int pausado = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Pausado"));
                int terminadoDibujo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "TerminadoRender"));

                // Obtener la fecha programada
                DateTime fechaProgramada = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega"));

                // Cambiar el color de fondo de la fila en función de los valores de las columnas

                // Verificar si la fecha programada ha pasado
                if (fechaProgramada <= DateTime.Now)
                {
                    e.Item.BackColor = System.Drawing.Color.FromName("#F71A27");
                }
                else if (programadoVentas == 0)
                {
                    e.Item.BackColor = System.Drawing.Color.FromName("#673f8b");
                    e.Item.ForeColor = System.Drawing.Color.FromName("#ffffff");
                }
                else
                {
                    if (pausado == 1)
                    {
                        e.Item.BackColor = System.Drawing.Color.FromName("#08F4E2");
                    }
                    else if (terminadoDibujo == 1)
                    {
                        e.Item.BackColor = System.Drawing.Color.FromName("#57F525");
                    }
                    else
                    {
                        e.Item.BackColor = System.Drawing.Color.FromName("#F1FF43");//amarillo 
                        e.Item.ForeColor = System.Drawing.Color.FromName("#000000");
                    }

                }
            }




        }

        protected void DataGridRenders_LinkButton(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "VerRenders")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridRenders.Items[rowIndex];

                string IdRender = row.Cells[2].Text;
                string NomRender = row.Cells[3].Text;
                string FechaUltimaActivacion = row.Cells[4].Text;
                DateTime FechaUltimaActivacionFormat = DateTime.Parse(FechaUltimaActivacion);
                string FechaEntrega = row.Cells[5].Text;
                DateTime FechaEntregaFormat = DateTime.Parse(FechaEntrega);
                string Asesor = row.Cells[6].Text;
                //Realizado por [7]
                string Zona = row.Cells[8].Text;
                string FechaRenderOK = row.Cells[9].Text;
                DateTime FechaRenderOKFormat = DateTime.Parse(FechaRenderOK);
                string NumeroDiseño = row.Cells[10].Text;
                string NombreRender = row.Cells[11].Text;
                string NombreContacto = row.Cells[12].Text;
                string FechaIngreso = row.Cells[13].Text;
                DateTime FechaIngresoFormat = DateTime.Parse(FechaIngreso);
                string Celular = row.Cells[14].Text;
                string Mail = row.Cells[15].Text;
                string Telefono = row.Cells[16].Text;
                string Plano = row.Cells[17].Text;

                string Imagenes = row.Cells[18].Text;
                string Areas = row.Cells[19].Text;
                string ObsVentas = row.Cells[20].Text;
                string SegPausa = row.Cells[21].Text;
                string Observacion_Dibujo = row.Cells[22].Text;
                string Linea = row.Cells[23].Text;
                string Superficie = row.Cells[24].Text;
                string Accesorios = row.Cells[25].Text;
                string Cantos = row.Cells[26].Text;
                string Perfileria = row.Cells[27].Text;
                string Paneles = row.Cells[28].Text;
                string Archivadores = row.Cells[29].Text;
                string Sillas = row.Cells[30].Text;
                string Pantallas = row.Cells[31].Text;
                string EspArquitectonico = row.Cells[32].Text;
                string Muebles = row.Cells[33].Text;
                string PisoyZocalo = row.Cells[34].Text;
                string Muros = row.Cells[35].Text;
                string Iluminacion = row.Cells[36].Text;
                string Antepecho = row.Cells[37].Text;
                string Ambientacion = row.Cells[38].Text;
                string Animacion = row.Cells[39].Text;


                NumeroRender.Text = IdRender;
                tbCliente.Text = NomRender;
                tbUltActiv.Text = FechaUltimaActivacionFormat.ToString("yyyy-MM-dd");
                tbEntrega.Text = FechaEntregaFormat.ToString("yyyy-MM-dd");
                string nombreBuscado = Asesor; // El nombre que deseas buscar
                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == nombreBuscado)
                    {
                        ddlAsesor.ClearSelection();
                        item.Selected = true; // Selecciona el elemento si se encuentra
                        break; // Rompe el bucle una vez que se encuentra una coincidencia
                    }
                }

                string ZonaBus = Zona; // El nombre que deseas buscar
                foreach (ListItem item in ddlAsesor.Items)
                {
                    if (item.Text == ZonaBus)
                    {
                        ddlZona.ClearSelection();
                        item.Selected = true; // Selecciona el elemento si se encuentra
                        break; // Rompe el bucle una vez que se encuentra una coincidencia
                    }
                }
                tbFechaOk.Text = FechaRenderOKFormat.ToString("yyyy-MM-dd");
                tbDiseño.Text = NumeroDiseño;
                tbProyecto.Text = NombreRender;
                tbContacto.Text = NombreContacto;
                tbIngreso.Text = FechaIngresoFormat.ToString("yyyy-MM-dd");
                tbCelular.Text = Celular;
                tbMail.Text = Mail;
                tbTelefono.Text = Telefono;
                tbPlano.Text = Plano;
                tbImagenes.Text = Imagenes;

                txAreaRender.InnerText = Areas;
                txObsVentas.InnerText = ObsVentas;

                if (SegPausa == "&nbsp;")
                {
                    string SegPausaRep = SegPausa.Replace("&nbsp;", "N/A");
                    txSegPausas.InnerText = SegPausaRep;
                }

                if (Observacion_Dibujo == "&nbsp;")
                {
                    string Observacion_DibujoRep = SegPausa.Replace("&nbsp;", "N/A");

                    txObsDibujo.InnerText = Observacion_DibujoRep;

                }

                tbLinea.Text = Linea;
                tbSup.Text = Superficie;
                tbAcc.Text = Accesorios;
                tbCantos.Text = Cantos;
                tbPaneles.Text = Paneles;
                tbPerfil.Text = Perfileria;
                tbArch.Text = Archivadores;
                tbSillas.Text = Sillas;
                tbPantallas.Text = Pantallas;
                if (EspArquitectonico == "True")
                {
                    chxEspArq.Checked = true;
                }
                else
                {
                    chxEspArq.Checked = false;
                }
                txMuebles.InnerText = Muebles;
                tbAcaPisZoc.Text = PisoyZocalo;
                tbAcaMuros.Text = Muros;
                tbIluminacion.Text = Iluminacion;
                tbAntepecho.Text = Antepecho;





                if (Ambientacion == "True")
                {
                    chxAmbientacion.Checked = true;
                }
                else
                {
                    chxAmbientacion.Checked = false;
                }

                if (Animacion == "True")
                {
                    chxAnimacion.Checked = true;
                }
                else
                {
                    chxAnimacion.Checked = false;
                }

                // Mantener Deshabilitado ddlAsesor y ddZona
                ddlAsesor.Enabled = false;
                ddlAsesor.CssClass = "form-control disabled";

                ddlZona.Enabled = false;
                ddlZona.CssClass = "form-control disabled";


                // Ejemplo el script para habilitar el enlace de moficar despues de selecionar la fila  usando RegisterStartupScript:
                string script = "<script>HabilitarEnlaces1();</script>";
                ScriptManager.RegisterStartupScript(this, GetType(), "HabilitarEnlaces1", script, false);



                // Actualizar el segundo DataGrid con los datos del procedimiento almacenado
                DataGridRenders.DataBind();
            }
        }


        protected void GuardarModificarRender(object sender, EventArgs e)
        {
            bool guardarCliente = chkEstadoGuardrRender.Checked;

            




            if (guardarCliente)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //Realizamos la Insercion 
                    string query = "query para insertar";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Paarametro1", "TextBox");
                        command.Parameters.AddWithValue("@Paarametro...", "TextBox");
                    

                        command.ExecuteNonQuery();
                    }


                }


            }

            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Realizamos actualización
                    string query = "query para actualizar";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Parametro1", "TextBox");
                        command.Parameters.AddWithValue("@Paarametro...", "TextBox");
                      


                        command.ExecuteNonQuery();
                    }


                }

            }

            Response.Redirect("../SuccessMessage.aspx");

        }



    }

}
