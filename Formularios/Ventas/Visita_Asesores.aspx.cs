using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Visita_Asesores : System.Web.UI.Page
    {
        // Variables de clase para almacenar las fechas seleccionadas
        private DateTime fechaInicioSeleccionada;
        private DateTime fechaFinSeleccionada;
        private string Fechas;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Llamar al método para cargar los datos en el DropDownList
                CargarAsesoresEnDropDownList();
                // Aquí puedes obtener y mostrar el rango de fechas en el título del DataGrid
                DateRangeLiteral.Text = GetDateRange();



            }
        }

        private void CargarAsesoresEnDropDownList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Cedula, CONCAT(Nombre, ' ', Apellidos) AS NombreCompleto FROM tblAsesorComercial WHERE activo =1 order by Nombre"; // Reemplaza por tu consulta SQL y tabla de datos

                SqlCommand command = new SqlCommand(consulta, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlAsesor.DataSource = reader;
                ddlAsesor.DataTextField = "NombreCompleto"; // Campo que se mostrará en el DropDownLi
                ddlAsesor.DataValueField = "Cedula";

                ddlAsesor.DataBind();

                reader.Close();
            }

            // Agregar un elemento inicial si lo deseas
            ddlAsesor.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }


        protected void ActualizarTituloDataGrid()
        {
            // Verificamos que ambas fechas estén seleccionadas antes de actualizar el título.
            if (fechaInicioSeleccionada != DateTime.MinValue && fechaFinSeleccionada != DateTime.MinValue)
            {
                DateRangeLiteral.Text = "Desde " + fechaInicioSeleccionada.ToString("dd/MM/yyyy") + " hasta " + fechaFinSeleccionada.ToString("dd/MM/yyyy");
            }
        }

        protected string GetDateRange()
        {
            string fechaInicio = fecha1.Text;
            string fechaFin = fecha2.Text;

            // Puedes personalizar el formato de las fechas según tus necesidades.
            return "Desde " + fechaInicio + " hasta " + fechaFin;
        }

        public void Consultar(object sender, EventArgs e)
        {
            ActualizarTituloDataGrid();
            DateRangeLiteral.Text = GetDateRange();


        }

        public void Cambio(object sender, EventArgs e)
        {

            DateRangeLiteral.Text = GetDateRange();

        }

        public void miDataGrid_PreRender(object sender, EventArgs e)
        {
            // Realiza el conteo de filas en el DataGrid y muestra el resultado en el Label

            int cantidadFilas = DataGrid1.Items.Count;
            lbVisitas.InnerText = cantidadFilas.ToString();
            lbFechas.InnerText = "("+ fecha1.Text + ") - (" + fecha2.Text +")";


            // Textos a buscar en el DataGrid
            List<string> textosABuscar = new List<string>();
            textosABuscar.Add("Visita Levantamiento");
            textosABuscar.Add("Visita Diseño");
            textosABuscar.Add("Visita Cliente Nuevo");
            textosABuscar.Add("Visita Cierre");
            textosABuscar.Add("Seguimiento Cotización");
            textosABuscar.Add("Mantenimiento");
            textosABuscar.Add("Entrega cotización");
            textosABuscar.Add("Cartera");




            // Contador para cada texto buscado
            Dictionary<string, int> conteoPorTexto = new Dictionary<string, int>();

            // Inicializa el conteo para cada texto en 0
            foreach (string texto in textosABuscar)
            {
                conteoPorTexto[texto] = 0;
            }

            // Recorre las filas del DataGrid y cuenta las filas que contienen cada texto
            foreach (DataGridItem item in DataGrid1.Items)
            {
                foreach (string texto in textosABuscar)
                {
                    // Asegúrate de ajustar el índice (en Cells[0]) según la columna en la que deseas buscar el texto
                    if (item.Cells[4].Text.Contains(texto))
                    {
                        conteoPorTexto[texto]++;
                    }
                }
            }

            // Luego en tu código principal, puedes llamar la función para cada campo:
            double VisLev = conteoPorTexto["Visita Levantamiento"];
            lbLev.InnerText = CalcularPorcentaje(VisLev, cantidadFilas).ToString() + " %";

            double VisitaDiseño = conteoPorTexto["Visita Diseño"];
            lbDis.InnerText = CalcularPorcentaje(VisitaDiseño, cantidadFilas).ToString() + " %";

            double VisCliNuevo = conteoPorTexto["Visita Cliente Nuevo"];
            lbCli.InnerText = CalcularPorcentaje(VisCliNuevo, cantidadFilas).ToString() + " %";


            double VisCierre = conteoPorTexto["Visita Cierre"];
            lblCierre.InnerText = CalcularPorcentaje(VisCierre, cantidadFilas).ToString() + " %";

            double SegCotizacion = conteoPorTexto["Seguimiento Cotización"];
            lbSegCot.InnerText = CalcularPorcentaje(SegCotizacion, cantidadFilas).ToString() + " %";

            double VisMantenimiento = conteoPorTexto["Mantenimiento"];
            lbMantenimiento.InnerText = CalcularPorcentaje(VisMantenimiento, cantidadFilas).ToString() + " %";


            double EntregaCotizacion = conteoPorTexto["Entrega cotización"];
            lbEntregaCot.InnerText = CalcularPorcentaje(EntregaCotizacion, cantidadFilas).ToString() + " %";

            double Cartera = conteoPorTexto["Cartera"];
            lbCartera.InnerText = CalcularPorcentaje(Cartera, cantidadFilas).ToString() + " %";


            double suma = VisLev + VisitaDiseño + VisCliNuevo + VisCierre + SegCotizacion + VisMantenimiento + EntregaCotizacion + Cartera;
            lbTotal.InnerText = CalcularPorcentaje(suma, cantidadFilas).ToString() + " %";
            // Muestra los otros resultados en sus respectivos Labels




        }


        // Definir una función para calcular el porcentaje redondeado con dos decimales
        public static double CalcularPorcentaje(double conteo, int cantidadFilas)
        {
            if (cantidadFilas != 0)
            {
                double porcentaje = ((double)conteo / cantidadFilas) * 100;
                return Math.Round(porcentaje, 2);
            }
            return 0; // Si cantidadFilas es 0, devolver 0 para evitar división por cero.
        }

        protected void btnCliente_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmClienteDiseño.aspx");
        }

        protected void ConsultarEstadisticas(object sender, EventArgs e)
        {

            // Realiza la consulta SQL para obtener los datos necesarios
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                            tblAsesorComercial.Nombre + ' ' + tblAsesorComercial.Apellidos AS Asesor,
                            COUNT(tblVisitaAsesor.Causa) AS CuentaDeCausa
                        FROM 
                            ((tblAsesorComercial
                            LEFT JOIN tblVisitaAsesor ON tblAsesorComercial.Cedula = tblVisitaAsesor.Asesor)
                            FULL JOIN tblCausaVisita ON tblVisitaAsesor.Causa = tblCausaVisita.Id_Causa)
                        WHERE 
                            tblVisitaAsesor.FechaVisita BETWEEN @FechaInicio AND @FechaFin
                            OR tblVisitaAsesor.FechaVisita IS NULL
                        GROUP BY 
                            tblAsesorComercial.CodigoAsesor, tblAsesorComercial.Nombre, tblAsesorComercial.Apellidos
                        ORDER BY 
                            COUNT(tblVisitaAsesor.Causa) DESC";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FechaInicio", fecha5.Text);
                command.Parameters.AddWithValue("@FechaFin", fecha6.Text);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Lista para almacenar los nombres y cantidades
                List<string> nombres = new List<string>();
                List<int> cantidades = new List<int>();

                while (reader.Read())
                {
                    string nombre = reader["Asesor"].ToString();
                    int cantidad = Convert.ToInt32(reader["CuentaDeCausa"]);

                    if (cantidad > 0) // Filtra los asesores con 0 visitas
                    {
                        nombres.Add(nombre);
                        cantidades.Add(cantidad);
                    }
                }

                reader.Close();

                // Generar la gráfica con los datos obtenidos
                string script = string.Format(@"var nombres = {0}; var cantidades = {1};
                                  GenerarGrafica(nombres, cantidades);",
                                              new JavaScriptSerializer().Serialize(nombres),
                                              new JavaScriptSerializer().Serialize(cantidades));

                ScriptManager.RegisterStartupScript(this, GetType(), "GenerarGrafica", script, true);


            }

            string fechaInicio = fecha5.Text;
            string fechaFin = fecha6.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Nombre del procedimiento almacenado
                string procedimientoAlmacenado = "EstadisticasTipoVisita";

                // Crea el comando y asigna los parámetros
                SqlCommand command = new SqlCommand(procedimientoAlmacenado, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@FechaFin", fechaFin);

                // Abre la conexión y ejecuta el comando
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);


                // Calcula el total de CuentaDeCausa
                int totalCausas = 0;
                foreach (DataRow row in dt.Rows)
                {
                    totalCausas += Convert.ToInt32(row["CuentaDeCausa"]);
                }

                // Agrega la columna "Porcentaje" y calcula los porcentajes para cada fila
                dt.Columns.Add("Porcentaje", typeof(string));
                foreach (DataRow row in dt.Rows)
                {
                    int cuentaDeCausa = Convert.ToInt32(row["CuentaDeCausa"]);
                    double porcentaje = (cuentaDeCausa / (double)totalCausas) * 100;
                    row["Porcentaje"] = porcentaje.ToString("0.00") + " %";
                }

                // Llena el DataGrid con los datos obtenidos
                DataGrid3.DataSource = dt;
                DataGrid3.DataBind();
            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                             tblCausaVisita.NombreCausa AS Nombre,
                             Count(tblVisitaAsesor.Causa) AS CuentaDeCausa
                        FROM 
                            tblCausaVisita
                            LEFT JOIN
                            tblVisitaAsesor ON tblCausaVisita.Id_Causa = tblVisitaAsesor.Causa 
                            AND tblVisitaAsesor.FechaVisita BETWEEN @FechaInicio AND @FechaFin
                        WHERE 
                             tblVisitaAsesor.FechaVisita BETWEEN @FechaInicio AND @FechaFin
                        GROUP BY 
                           tblCausaVisita.Id_Causa, tblCausaVisita.NombreCausa 
                        ORDER BY 
                           CuentaDeCausa DESC, tblCausaVisita.NombreCausa DESC";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FechaInicio", fecha5.Text);
                command.Parameters.AddWithValue("@FechaFin", fecha6.Text);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Lista para almacenar los nombres y cantidades
                List<string> nombres1 = new List<string>();
                List<int> cantidades1 = new List<int>();

                while (reader.Read())
                {
                    string nombre1 = reader["Nombre"].ToString();
                    int cantidad1 = Convert.ToInt32(reader["CuentaDeCausa"]);

                    if (cantidad1 > 0) // Filtra los asesores con 0 visitas
                    {
                        nombres1.Add(nombre1);
                        cantidades1.Add(cantidad1);
                    }
                }

                reader.Close();



                // Generar la gráfica con los datos obtenidos
                string script = string.Format(@"var nombres1 = {0}; var cantidades1 = {1};
                                  GenerarGrafica1(nombres1, cantidades1);",
                                              new JavaScriptSerializer().Serialize(nombres1),
                                              new JavaScriptSerializer().Serialize(cantidades1));

                ScriptManager.RegisterStartupScript(this, GetType(), "GenerarGrafica1", script, true);



            }





        }
    }
}