using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo
{
    public partial class EstadisticaDise : System.Web.UI.Page
    {
        public int TotalRender { get; set; }
        public int TotalCumplidos { get; set; }
        private string CadenaConexionSID = "BD_SIDSQL";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                if (!IsPostBack)
                {
                    txtPeriodoInicio.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    txtPeriodoFin.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    TextBoxFechaInicioRenders.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    TextBoxFechaFinRenders.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    LlenarDropDownListAsesores();


                    System.Web.UI.WebControls.Label lblDetallePedidos = this.lblDetallePedidos;
                    lblDetallePedidos.Text = "DETALLE ESTADISTICA POR PEDIDOS";
                }
            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
        }

        protected string FormatPercentage(double value)
        {
            return (value * 100).ToString("F1");
        }

   

        private void LlenarDropDownListAsesores()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT cedula, Nombre + ' ' + Apellidos AS Asesor FROM tblAsesorComercial WHERE activo = 1 ORDER BY Nombre ASC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    ddlAsesor.DataSource = reader;
                    ddlAsesor.DataTextField = "Asesor"; // Campo a mostrar
                    ddlAsesor.DataValueField = "cedula"; // Valor asociado
                    ddlAsesor.DataBind();
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    // Log ex.Message o mostrar un mensaje de error si es necesario
                }
                finally
                {
                    connection.Close();
                }
            }

            // Agregar un elemento predeterminado
            ddlAsesor.Items.Insert(0, new ListItem("%", "%"));
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string onedriveFileUrl = "https://duconco-my.sharepoint.com/:x:/g/personal/sistemas_ducon_com_co/EYFkwyrg_GpFodl-v5aOWC8BI3ialfbwbaDQbpwlPEYmzg?e=6SyMXD";

            // Redirige al usuario al archivo
            Response.Redirect(onedriveFileUrl);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenardatagriddetalle();
            llenarDatagridEstadisticaResumen();
            LlenarDataGridDetalleEstadistica();
            CargarEstadisticaPorDibujante();
        }

        protected void llenarDatagridEstadisticaResumen()
        {
            // Convertir las fechas de los TextBox a DateTime
            DateTime fechaInicial = Convert.ToDateTime(txtPeriodoInicio.Text).Date;
            DateTime fechaFinal = Convert.ToDateTime(txtPeriodoFin.Text).Date.AddDays(1).AddTicks(-1);

            // Obtener los valores de zona y asesor
            string zona = ddlZona.SelectedValue;
            string asesorCedula = ddlAsesor.SelectedValue;

            // Define la conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Consulta SQL corregida
            string query = @"
    SELECT 
        tblPlano.RealizadoPor, 
        tblOT.Id_OT, 
        tblOT.Consecutivo_Pedido, 
        tblOT.Terminado_Diseño, 
        CAST(0 AS DECIMAL(10, 2)) AS Entrega, 
        tblOT.Nombre_Obra, 
        tblOT.Fecha_Entrega_Produccion, 
        tblOT.Fecha_Entrega_Dibujo_Despiece, 
        tblOT.Fecha_Despacho_Produccion, 
        tblOT.Precio_Venta, 
        tblOT.Descuento, 
        tblOT.Precio_Venta - (tblOT.Precio_Venta * tblOT.Descuento / 100) AS VentaNeta, 
        tblOT.Zona, 
        tblOT.Codigo_Asesor
    FROM tblOT 
    RIGHT JOIN tblPlano 
    ON tblOT.Id_OT = tblPlano.Id_OT 
    AND tblOT.Consecutivo_Pedido = tblPlano.Consecutivo_Pedido
    WHERE 
        tblPlano.RealizadoPor LIKE '%' 
        AND tblOT.Terminado_Diseño = 1
        AND tblOT.Fecha_Entrega_Produccion BETWEEN @FechaInicio AND @FechaFin
        AND tblOT.Zona LIKE '%' + @Zona + '%'
        AND tblOT.Codigo_Asesor LIKE @AsesorCedula + '%'
        AND tblOT.Id_TipoPedido NOT IN ('11', '12', '13', '14', '15', '17')
    ORDER BY tblPlano.RealizadoPor ASC";

            // Variables para totales
            decimal totalPedidos = 0;
            decimal totalVentaNeta = 0;
            decimal totalPedidosUrgentes = 0;
            decimal totalPedidosNoDibujo = 0;
            decimal totalCumplidos = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicial);
                    command.Parameters.AddWithValue("@FechaFin", fechaFinal);
                    command.Parameters.AddWithValue("@Zona", zona);
                    command.Parameters.AddWithValue("@AsesorCedula", asesorCedula);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Crear una tabla para almacenar los datos
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id_OT");
                    dataTable.Columns.Add("Consecutivo_Pedido");
                    dataTable.Columns.Add("Nombre_Obra");
                    dataTable.Columns.Add("Entrega");
                    dataTable.Columns.Add("Fecha_Entrega_Dibujo_Despiece");
                    dataTable.Columns.Add("Fecha_Entrega_Produccion");
                    dataTable.Columns.Add("VentaNeta");
                    dataTable.Columns.Add("Zona");
                    dataTable.Columns.Add("Fecha_Despacho_Produccion");
                    dataTable.Columns.Add("Urgente");

                    while (reader.Read())
                    {
                        DataRow row = dataTable.NewRow();
                        row["Id_OT"] = reader["Id_OT"];
                        row["Consecutivo_Pedido"] = reader["Consecutivo_Pedido"];
                        row["Nombre_Obra"] = reader["Nombre_Obra"];
                        row["Entrega"] = 0;
                        row["Fecha_Entrega_Dibujo_Despiece"] = Convert.ToDateTime(reader["Fecha_Entrega_Dibujo_Despiece"]).ToString("dd/MM/yyyy HH:mm");
                        row["Fecha_Entrega_Produccion"] = Convert.ToDateTime(reader["Fecha_Entrega_Produccion"]).ToString("dd/MM/yyyy HH:mm");
                        row["VentaNeta"] = reader["VentaNeta"];
                        row["Zona"] = reader["Zona"];
                        row["Fecha_Despacho_Produccion"] = Convert.ToDateTime(reader["Fecha_Despacho_Produccion"]).ToString("dd/MM/yyyy");

                        // Calcular urgencia
                        DateTime fechaEntrega = Convert.ToDateTime(reader["Fecha_Entrega_Dibujo_Despiece"]);
                        DateTime fechaDespacho = Convert.ToDateTime(reader["Fecha_Despacho_Produccion"]);
                        int diasHabiles = CalcularDiasHabiles(fechaEntrega, fechaDespacho);

                        if (diasHabiles <= 2)
                        {
                            row["Urgente"] = "SI";
                            totalPedidosUrgentes++;
                        }
                        else
                        {
                            row["Urgente"] = "NO";
                        }

                        // Incrementar totales
                        totalPedidos++;
                        totalVentaNeta += Convert.ToDecimal(reader["VentaNeta"]);
                        if (fechaEntrega.Hour != 0) totalPedidosNoDibujo++;

                        // Aquí es donde corregimos la lógica de "Cumplidos":
                        // Un pedido es "cumplido" si la fecha de entrega es posterior o igual a la fecha de producción.
                        if (fechaEntrega >= Convert.ToDateTime(reader["Fecha_Entrega_Produccion"]))
                        {
                            totalCumplidos++;
                        }

                        dataTable.Rows.Add(row);
                    }

                    reader.Close();

                    // Agregar filas de totales
                    DataRow totalRow1 = dataTable.NewRow();
                    totalRow1["Id_OT"] = "T. Ped";
                    totalRow1["Consecutivo_Pedido"] = "";
                    totalRow1["Nombre_Obra"] =  totalPedidos;
                    totalRow1["VentaNeta"] = "Externo a Dibujo";
                    totalRow1["Zona"] = totalPedidosNoDibujo;
                    totalRow1["Fecha_Despacho_Produccion"] = "Urgentes";
                    totalRow1["Urgente"] = totalPedidosUrgentes;
                    dataTable.Rows.Add(totalRow1);

                    DataRow totalRow2 = dataTable.NewRow();
                    totalRow2["Id_OT"] = "T. Cum";
                    totalRow2["Consecutivo_Pedido"] = "";
                    totalRow2["Nombre_Obra"] = "";
                    totalRow2["Entrega"] = $"{(totalCumplidos / totalPedidos * 100):0.0}%";
                    totalRow2["Zona"] = "";
                    totalRow2["Urgente"] = $"{(totalPedidosUrgentes / totalPedidosNoDibujo * 100):0.0}%";
                    dataTable.Rows.Add(totalRow2);

                    // Asignar el DataTable al DataGrid
                    DataGridDetalleEstadisiticaPorPedido.DataSource = dataTable;
                    DataGridDetalleEstadisiticaPorPedido.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Método para calcular días hábiles
        private int CalcularDiasHabiles(DateTime inicio, DateTime fin)
        {
            int diasHabiles = 0;
            for (DateTime fecha = inicio; fecha <= fin; fecha = fecha.AddDays(1))
            {
                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasHabiles++;
                }
            }
            return diasHabiles;
        }


        protected void llenardatagriddetalle()
        {
            // Convertir las fechas de texto a DateTime
            DateTime fechaInicial = Convert.ToDateTime(txtPeriodoInicio.Text).Date;
            DateTime fechaFinal = Convert.ToDateTime(txtPeriodoFin.Text).Date.AddDays(1).AddTicks(-1);

            string zona = ddlZona.Text;
            string asesor = ddlAsesor.Text;

            // Consulta SQL con parámetros
            string query = @"
    SELECT 
        tblPlano.RealizadoPor, 
        tblOT.Id_OT, 
        tblOT.Consecutivo_Pedido, 
        tblOT.Terminado_Diseño, 
        tblOT.Nombre_Obra, 
        tblOT.Fecha_Entrega_Produccion, 
        tblOT.Fecha_Entrega_Dibujo_Despiece,
        tblOT.Fecha_Despacho_Produccion, 
        tblOT.Precio_Venta, 
        tblOT.Descuento, 
        tblOT.Precio_Venta - tblOT.Precio_Venta * tblOT.Descuento / 100 AS VentaNeta, 
        tblOT.Zona, 
        tblOT.Codigo_Asesor 
    FROM 
        tblOT 
    RIGHT JOIN 
        tblPlano 
    ON 
        tblOT.Id_OT = tblPlano.Id_OT 
        AND tblOT.Consecutivo_Pedido = tblPlano.Consecutivo_Pedido
    WHERE 
        tblPlano.RealizadoPor LIKE '%' 
        AND tblOT.Terminado_Diseño = 1
        AND tblOT.Fecha_Entrega_Produccion BETWEEN @FechaInicial AND @FechaFinal
        AND tblOT.Zona LIKE '%' + @Zona + '%'
        AND tblOT.Codigo_Asesor LIKE @Asesor + '%'
        AND tblOT.Id_TipoPedido NOT IN ('11', '12', '13', '14', '15', '17')
    ORDER BY 
        tblPlano.RealizadoPor ASC";

            List<ResumenEstadistica> resumenList = new List<ResumenEstadistica>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicial", fechaInicial);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFinal);
                    command.Parameters.AddWithValue("@Zona", zona);
                    command.Parameters.AddWithValue("@Asesor", asesor);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        string currentDibujante = string.Empty;

                        while (reader.Read())
                        {
                            string realizadoPor = reader["RealizadoPor"].ToString();
                            decimal ventaNeta = Convert.ToDecimal(reader["VentaNeta"]);
                            DateTime fechaEntregaProduccion = Convert.ToDateTime(reader["Fecha_Entrega_Produccion"]);
                            DateTime fechaEntregaDibujo = Convert.ToDateTime(reader["Fecha_Entrega_Dibujo_Despiece"]);

                            if (realizadoPor != currentDibujante)
                            {
                                if (!string.IsNullOrEmpty(currentDibujante))
                                {
                                    var resumen = resumenList.Last();
                                    resumen.PorcentajeCumplido = (decimal)resumen.PedidosCumplidos / resumen.TotalPedidos * 100;
                                    resumen.PorcentajeNoCumplido = (decimal)resumen.PedidosNoCumplidos / resumen.TotalPedidos * 100;
                                }

                                resumenList.Add(new ResumenEstadistica
                                {
                                    RealizadoPor = realizadoPor,
                                    TotalPedidos = 1,
                                    PedidosCumplidos = (fechaEntregaProduccion >= fechaEntregaDibujo) ? 1 : 0,
                                    PedidosNoCumplidos = (fechaEntregaProduccion < fechaEntregaDibujo) ? 1 : 0,
                                    VentaNeta = ventaNeta
                                });

                                currentDibujante = realizadoPor;
                            }
                            else
                            {
                                var resumen = resumenList.Last();
                                resumen.TotalPedidos += 1;
                                resumen.VentaNeta += ventaNeta;

                                // Determinar fecha de ingreso
                                DateTime fechaIngreso = fechaEntregaDibujo; // Ajustar si es DateTime.Now o un valor de tu lógica

                                // Determinar plazo de entrega (ejemplo: suma 2 días hábiles)
                                float plazoEntrega = 2;

                                // Calcular fechas
                                DateTime fechaActivacion;
                                CalcularFechaEntrega(fechaIngreso, out fechaActivacion, out fechaEntregaDibujo, plazoEntrega);

                                // Comparar con la fecha de entrega original
                                if (fechaEntregaProduccion <= fechaEntregaDibujo)
                                {
                                    resumen.PedidosCumplidos++;
                                }
                                else
                                {
                                    resumen.PedidosNoCumplidos++;
                                }
                            }
                        }

                        // Si hay resúmenes, calcular los porcentajes y agregar fila de totales
                        if (resumenList.Any())
                        {
                            var resumen = resumenList.Last();
                            resumen.PorcentajeCumplido = (decimal)resumen.PedidosCumplidos / resumen.TotalPedidos * 100;
                            resumen.PorcentajeNoCumplido = (decimal)resumen.PedidosNoCumplidos / resumen.TotalPedidos * 100;

                            // Agregar fila de totales
                            resumenList.Add(new ResumenEstadistica
                            {
                                RealizadoPor = "TOTAL PEDIDOS",
                                TotalPedidos = resumenList.Sum(r => r.TotalPedidos),
                                PedidosCumplidos = resumenList.Sum(r => r.PedidosCumplidos),
                                PedidosNoCumplidos = resumenList.Sum(r => r.PedidosNoCumplidos),
                                VentaNeta = resumenList.Sum(r => r.VentaNeta),
                                PorcentajeCumplido = (decimal)resumenList.Sum(r => r.PedidosCumplidos) / resumenList.Sum(r => r.TotalPedidos) * 100,
                                PorcentajeNoCumplido = (decimal)resumenList.Sum(r => r.PedidosNoCumplidos) / resumenList.Sum(r => r.TotalPedidos) * 100
                            });
                        }

                        // Asignar el DataSource y vincularlo al GridView
                        DataGridResumenEstadisticaPorPedido.DataSource = resumenList;
                        DataGridResumenEstadisticaPorPedido.DataBind();
                    }
                }
            }

         
        }


        public void CalcularFechaEntrega(DateTime fechaIngreso, out DateTime fechaActivacion, out DateTime fechaEntrega, float plazoEntrega)
        {
            fechaActivacion = fechaIngreso;
            float tiempoAdicional;

            // Ajustar hora de activación si es antes de las 7 AM
            if (fechaActivacion.Hour < 7)
            {
                tiempoAdicional = 7 / 24f - (fechaActivacion.Hour / 24f + fechaActivacion.Minute / 1440f);
                fechaActivacion = fechaActivacion.AddDays(tiempoAdicional);
            }

            // Ajustar hora de activación si es después de las 5 PM
            if (fechaActivacion.Hour + fechaActivacion.Minute / 60f > 17)
            {
                tiempoAdicional = (fechaActivacion.Hour / 24f + fechaActivacion.Minute / 1440f) - 7 / 24f;
                fechaActivacion = fechaActivacion.AddDays(1).AddDays(-tiempoAdicional);
            }

            // Ajustar si es un día no laborable
            while (EsDiaNoLaborable(fechaActivacion))
            {
                tiempoAdicional = (fechaActivacion.Hour / 24f + fechaActivacion.Minute / 1440f) - 7 / 24f;
                fechaActivacion = fechaActivacion.AddDays(1).AddDays(-tiempoAdicional);
            }

            // Calcular la fecha de entrega
            fechaEntrega = fechaActivacion;

            // Incrementar días hasta alcanzar el plazo, evitando días no laborables
            int diasContados = 0;
            while (diasContados < plazoEntrega)
            {
                fechaEntrega = fechaEntrega.AddDays(1);

                if (!EsDiaNoLaborable(fechaEntrega))
                {
                    diasContados++;
                }
            }
        }

        private bool EsDiaNoLaborable(DateTime fecha)
        {
            // Consulta la base de datos para verificar si es día no laborable
            string query = "SELECT COUNT(*) FROM tblDiaNoLaboral WHERE dnlFecha = @Fecha";
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Fecha", fecha.Date);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0 || fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday;
            }
        }

        public class ResumenEstadistica
        {
            public string RealizadoPor { get; set; }
            public int TotalPedidos { get; set; }
            public int PedidosCumplidos { get; set; }
            public int PedidosNoCumplidos { get; set; }
            public decimal VentaNeta { get; set; }
            public decimal PorcentajeCumplido { get; set; }
            public decimal PorcentajeNoCumplido { get; set; }
            public decimal TotalVentaNeta { get; set; }
        }
        protected void LlenarDataGridDetalleEstadistica()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = @"
        SELECT 
            CONCAT(tblDiseño.Cliente, ' - ', tblDiseño.Nombre_Diseño) AS ClienteYNombreDiseño,
            tblDiseño.Numero_Diseño,
            tblDiseño.Fecha_Ingreso,
            tblDiseño.FechaDibujoOK,
            tblDiseño.UltimaActivacion,
            DATEDIFF(hour, tblDiseño.UltimaActivacion, tblDiseño.FechaDibujoOK) * 1.0 / 24 AS ENTREGA,
            tblDiseño.Puestos,
            tblDiseño.Asesor,
            tblDiseño.RealizadoPor,
            tblDiseño.SeguimientoPausa,
            tblDiseño.Urgente,
            tblDiseño.Fecha_Programada_Entrega,
            tblDiseño.Zona
        FROM tblDiseño
        WHERE tblDiseño.FechaDibujoOK BETWEEN @FechaInicial AND @FechaFinal
          AND tblDiseño.Asesor LIKE @Asesor
          AND tblDiseño.Zona LIKE @Zona
          AND tblDiseño.TerminadoDibujo = 1
        ORDER BY tblDiseño.RealizadoPor DESC, tblDiseño.Numero_Diseño DESC;
    ";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FechaInicial", txtPeriodoInicio.Text);
                    cmd.Parameters.AddWithValue("@FechaFinal", txtPeriodoFin.Text + " 23:59:59");
                    cmd.Parameters.AddWithValue("@Asesor", ddlAsesor.SelectedItem.Text + "%");
                    cmd.Parameters.AddWithValue("@Zona", "%" + ddlZona.SelectedValue + "%");

                    try
                    {
                        con.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            // Cálculo de totales
                            int totalPedidos = dt.Rows.Count;
                            int totalCumplidos = dt.AsEnumerable().Count(row => Convert.ToBoolean(row["Urgente"]));

                            // Agregar las filas de resumen
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["ClienteYNombreDiseño"] = "TOTAL DISEÑOS";
                            dt.Rows[dt.Rows.Count - 1]["Numero_Diseño"] = totalPedidos;
                            dt.Rows[dt.Rows.Count - 1]["FechaDibujoOK"] = DBNull.Value;
                            dt.Rows[dt.Rows.Count - 1]["Entrega"] = DBNull.Value;
                            dt.Rows[dt.Rows.Count - 1]["Puestos"] = DBNull.Value;
                            dt.Rows[dt.Rows.Count - 1]["Asesor"] = DBNull.Value;
                            dt.Rows[dt.Rows.Count - 1]["RealizadoPor"] = DBNull.Value;
                            dt.Rows[dt.Rows.Count - 1]["SeguimientoPausa"] = DBNull.Value;
                            dt.Rows[dt.Rows.Count - 1]["Urgente"] = totalCumplidos;
                            dt.Rows[dt.Rows.Count - 1]["Fecha_Programada_Entrega"] = DBNull.Value;
                            dt.Rows[dt.Rows.Count - 1]["Zona"] = DBNull.Value;

                           



                            DataGridDetalleEstadisticaPorDiseno.DataSource = dt;
                            DataGridDetalleEstadisticaPorDiseno.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                    }
                }
            }
        }


        protected void DataGridDetalleEstadisticaPorDiseno_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               

                if (e.Item.Cells[1].Text == "TOTAL DISEÑOS")
                {
                    e.Item.Cells[0].Text = string.Empty;

                    e.Item.Font.Bold = true;
                }
                else
                {
                    object urgenteObj = DataBinder.Eval(e.Item.DataItem, "Urgente");

                    // Verificar si 'Urgente' es null o no es bool
                    if (urgenteObj == DBNull.Value || !(urgenteObj is bool))
                    {
                        // Omitir este paso si no es un valor válido o no es booleano
                        e.Item.Cells[8].Text = string.Empty;
                    }
                    else
                    {
                        bool urgente = Convert.ToBoolean(urgenteObj);

                        // Asignar "SI" o "NO" según el valor booleano
                        e.Item.Cells[8].Text = urgente ? "SI" : "NO";
                    }
                }

            }
        }



        protected void CargarEstadisticaPorDibujante()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            string query = @"
    SELECT 
        tblDiseño.RealizadoPor AS Dibujante,
        COUNT(tblDiseño.Numero_Diseño) AS Diseños,
        SUM(CASE 
            WHEN tblDiseño.Fecha_Programada_Entrega >= tblDiseño.FechaDibujoOK THEN 1 
            ELSE 0 
        END) AS Cumplidos,
        SUM(CASE 
            WHEN tblDiseño.Fecha_Programada_Entrega < tblDiseño.FechaDibujoOK THEN 1 
            ELSE 0 
        END) AS NoCumplidos
    FROM tblDiseño
    WHERE tblDiseño.FechaDibujoOK BETWEEN @FechaInicial AND @FechaFinal
        AND tblDiseño.Asesor LIKE @Asesor
        AND tblDiseño.TerminadoDibujo = 1
        AND tblDiseño.Zona LIKE @Zona
    GROUP BY tblDiseño.RealizadoPor
    ORDER BY Dibujante DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FechaInicial", txtPeriodoInicio.Text);
                cmd.Parameters.AddWithValue("@FechaFinal", txtPeriodoFin.Text + " 23:59:59");
                cmd.Parameters.AddWithValue("@Asesor", $"{ddlAsesor.SelectedItem.Text}%");
                cmd.Parameters.AddWithValue("@Zona", $"%{ddlZona.SelectedValue}%");

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    // Agregar columnas para porcentajes
                    dt.Columns.Add("PorcentajeCumplidos", typeof(string));
                    dt.Columns.Add("PorcentajeNoCumplidos", typeof(string));

                    int totalDiseños = 0;
                    int totalCumplidos = 0;
                    int totalNoCumplidos = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        int totalDis = Convert.ToInt32(row["Diseños"]);
                        int cumplidos = Convert.ToInt32(row["Cumplidos"]);
                        int noCumplidos = Convert.ToInt32(row["NoCumplidos"]);

                        totalDiseños += totalDis;
                        totalCumplidos += cumplidos;
                        totalNoCumplidos += noCumplidos;

                        // Calcular porcentajes
                        row["PorcentajeCumplidos"] = totalDis > 0 ? $"{(cumplidos * 100.0 / totalDis):F1}%" : "0%";
                        row["PorcentajeNoCumplidos"] = totalDis > 0 ? $"{(noCumplidos * 100.0 / totalDis):F1}%" : "0%";
                    }

                    // Agregar fila de totales
                    DataRow totalRow = dt.NewRow();
                    totalRow["Dibujante"] = "TOTAL DISEÑOS";
                    totalRow["Diseños"] = totalDiseños;
                    totalRow["Cumplidos"] = totalCumplidos;
                    totalRow["NoCumplidos"] = totalNoCumplidos;
                    totalRow["PorcentajeCumplidos"] = totalDiseños > 0 ? $"{(totalCumplidos * 100.0 / totalDiseños):F1}%" : "0%";
                    totalRow["PorcentajeNoCumplidos"] = totalDiseños > 0 ? $"{(totalNoCumplidos * 100.0 / totalDiseños):F1}%" : "0%";
                    dt.Rows.Add(totalRow);

                    // Asignar el DataTable al DataGrid
                    DataGridEstadisticaPorDiseno.DataSource = dt;
                    DataGridEstadisticaPorDiseno.DataBind();
                }
            }
        }



        protected void DataGridEstadisticaPorDiseno_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            // Verificar si el ítem es un DataRow
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Obtener el índice de la fila y agregar 1 (ya que comienza desde 0)
                int rowIndex = e.Item.DataSetIndex + 1;

                // Asignar el valor al primer celda (columna "Nº")
                e.Item.Cells[1].Text = rowIndex.ToString();

                if (e.Item.Cells[2].Text == "TOTAL DISEÑOS")
                {
                    e.Item.Cells[1].Text = string.Empty;
                    e.Item.Cells[0].Text = string.Empty;
                    // Aplica formato en negrita a toda la fila
                    e.Item.Font.Bold = true;
                }
            }
        }

        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            llenarEstadisticaRender();
            CargarResumenEstadisticaRenders();
            llenarEstadisticaSC();
            llenarResumenEstadisticaShowCase();
        }

        protected void llenarEstadisticaRender()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Definir la consulta SQL con parámetros
            string sSql = "SELECT CONCAT(tblRender.Cliente, ' - ', tblRender.Nombre_Render) AS ClienteNombre, " +
                          "tblRender.Id_Render, tblRender.Fecha_Ingreso, tblRender.Fecha_Programada_Entrega, " +
                          "tblRender.FechaRenderOK, tblRender.UltimaActivacion, " +
                          "DATEDIFF(DAY, tblRender.UltimaActivacion, tblRender.FechaRenderOK) AS Entrega, " + // Usamos DATEDIFF para calcular la diferencia en días
                          "tblRender.Asesor, tblRender.RealizadoPor, tblRender.SeguimientoPausa, tblRender.TerminadoRender, tblRender.Zona " +
                          "FROM tblRender " +
                          "WHERE tblRender.FechaRenderOK BETWEEN @FechaInicial AND @FechaFinal " +
                          "AND tblRender.TerminadoRender = 1 " +
                          "AND tblRender.Zona LIKE @Zona " +
                          "ORDER BY tblRender.RealizadoPor;";

            // Establecer conexión y ejecutar consulta
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Crear un comando SQL
                using (SqlCommand cmd = new SqlCommand(sSql, conn))
                {
                    // Agregar los parámetros con valores
                    cmd.Parameters.AddWithValue("@FechaInicial", TextBoxFechaInicioRenders.Text);
                    cmd.Parameters.AddWithValue("@FechaFinal", TextBoxFechaFinRenders.Text + " 23:59:59");
                    cmd.Parameters.AddWithValue("@Zona", "%" + DropDownList2.SelectedValue + "%");

                    // Crear un adaptador de datos y llenar el DataTable
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Calcular los totales para los renders y cumplidos
                    int totalRenders = 0;
                    int totalCumplidos = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        totalRenders++; // Incrementar el total de renders
                        if (row["Entrega"] != DBNull.Value && Convert.ToDecimal(row["Entrega"]) > 0) // Si hay cumplimiento
                        {
                            totalCumplidos++;
                        }
                    }

                    // Agregar la fila de TOTAL RENDERS
                    DataRow totalRenderRow = dt.NewRow();
                    totalRenderRow["ClienteNombre"] = "TOTAL RENDERS"; // Texto "TOTAL RENDERS"
                    totalRenderRow["Entrega"] = totalRenders.ToString(); // Total de renders (en la columna 3)
                    dt.Rows.Add(totalRenderRow);

                    // Agregar la fila de CUMPLIDOS
                    DataRow cumplidosRow = dt.NewRow();
                    cumplidosRow["ClienteNombre"] = "CUMPLIDOS"; // Texto "CUMPLIDOS"
                    cumplidosRow["Entrega"] = totalCumplidos.ToString(); // Total de cumplidos (en la columna 3)
                    dt.Rows.Add(cumplidosRow);

                    // Asignar el DataTable al DataGrid
                    DataGridEstadisticaRenders.DataSource = dt;
                    DataGridEstadisticaRenders.DataBind();
                }
            }
        }

        protected void DataGridEstadisticaRenders_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            // Verificar si la fila es de totales
            if (e.Item.DataItem is DataRowView rowView)
            {
                string clienteNombre = rowView["ClienteNombre"].ToString();

                if (clienteNombre == "TOTAL RENDERS" || clienteNombre == "CUMPLIDOS")
                {
                    // Poner en negrita todo el contenido de la fila de totales
                    e.Item.Font.Bold = true;

                    // Ocultar la columna con el botón (columna 0)
                    e.Item.Cells[0].Visible = false;

                    // Mover el texto "TOTAL RENDERS" y "CUMPLIDOS" a la columna 1
                    e.Item.Cells[1].Text = clienteNombre; // Columna 1 tiene el texto "TOTAL RENDERS" o "CUMPLIDOS"

                    // Mover los totales a la columna 3 (columna 'Entrega')
                    e.Item.Cells[3].Text = rowView["Entrega"].ToString(); // Total de renders o cumplidos

                    // Vaciar las columnas que no necesitamos (columna 2 y columna 4 en adelante)
                    e.Item.Cells[2].Text = ""; // Vaciar columna 2 (Render)
                    for (int i = 4; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].Text = ""; // Vaciar columnas adicionales
                    }
                }
            }
        }

        protected void CargarResumenEstadisticaRenders()
        {
            string consultaSql = @"
    SELECT tblRender.Cliente, tblRender.Id_Render, tblRender.Nombre_Render, 
           tblRender.Fecha_Ingreso, tblRender.Fecha_Programada_Entrega, 
           tblRender.FechaRenderOK, tblRender.UltimaActivacion, 
           tblRender.FechaRenderOK - tblRender.UltimaActivacion AS Entrega, 
           tblRender.Asesor, tblRender.RealizadoPor, tblRender.SeguimientoPausa, 
           tblRender.TerminadoRender, tblRender.Zona 
    FROM tblRender 
    WHERE tblRender.FechaRenderOK BETWEEN @FechaInicio AND @FechaFin
      AND tblRender.TerminadoRender = 1
      AND tblRender.Zona LIKE '%' + @Zona + '%' 
    ORDER BY tblRender.RealizadoPor";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(consultaSql, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicio", TextBoxFechaInicioRenders.Text);
                    command.Parameters.AddWithValue("@FechaFin", TextBoxFechaFinRenders.Text + " 23:59:59");
                    command.Parameters.AddWithValue("@Zona", DropDownList2.SelectedValue);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }

            var resumen = dataTable.AsEnumerable()
                .GroupBy(row => row["RealizadoPor"].ToString())
                .Select((group, index) => new
                {
                    Numero = index + 1,
                    Responsable = group.Key,
                    Rend = group.Count(),
                    Cum = group.Count(r => DateTime.Parse(r["FechaRenderOK"].ToString()) < DateTime.Parse(r["Fecha_Programada_Entrega"].ToString())),
                    NoCum = group.Count(r => DateTime.Parse(r["FechaRenderOK"].ToString()) >= DateTime.Parse(r["Fecha_Programada_Entrega"].ToString()))
                })
                .Select(r => new
                {
                    r.Numero,
                    r.Responsable,
                    r.Rend,
                    PorcentajeRend = (double)r.Rend / dataTable.Rows.Count * 100,
                    r.Cum,
                    PorcentajeCum = (double)r.Cum / dataTable.Rows.Count * 100,
                    r.NoCum,
                    PorcentajeNoCum = (double)r.NoCum / dataTable.Rows.Count * 100
                })
                .ToList();

            int totalRender = resumen.Sum(r => r.Rend);
            int totalCumplidos = resumen.Sum(r => r.Cum);
            int totalNoCumplidos = resumen.Sum(r => r.NoCum);

            resumen.Add(new
            {
                Numero = 0, // Usamos 0 o cualquier valor distintivo para los totales
                Responsable = "TOTAL",
                Rend = totalRender,
                PorcentajeRend = 100.0, // Ahora es un double
                Cum = totalCumplidos,
                PorcentajeCum = totalRender > 0 ? (double)totalCumplidos / totalRender * 100 : 0,
                NoCum = totalNoCumplidos,
                PorcentajeNoCum = totalRender > 0 ? (double)totalNoCumplidos / totalRender * 100 : 0
            });

            DataGridResumenEstadisticaRenders.DataSource = resumen;
            DataGridResumenEstadisticaRenders.DataBind();
        }

        protected void DataGridResumenEstadisticaRenders_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            // Verifica que sea una fila de datos
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dataItem = e.Item.DataItem;

                // Comprueba si la fila es la de totales
                if (dataItem != null && dataItem.GetType().GetProperty("Responsable").GetValue(dataItem).ToString() == "TOTAL")
                {
                    // Aplica estilo en negrita a toda la fila
                    e.Item.Font.Bold = true;

                    // Oculta el contenido de la columna "Numero" (asume que es la primera columna)
                    e.Item.Cells[0].Text = string.Empty;
                    e.Item.Cells[1].Text = string.Empty;
                }
            }
        }

        protected void llenarEstadisticaSC()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            // Definir la consulta SQL con parámetros
            string sSql = "SELECT tblDiseño.Numero_Diseño, " +
                          "tblDiseño.SC_Dibujante, " +
                          "tblDiseño.SC_FechaTerminado, " +
                          "tblDiseño.SC_Fecha, " +
                          "tblDiseño.SC_Terminado, " +
                          "tblDiseño.Asesor, " +
                          "tblDiseño.Nombre_Diseño, " +
                          "tblDiseño.Zona, " +
                          "tblDiseño.SC_Tiemporeal, " +
                          "tblDiseño.Cliente, " +
                          "CONCAT(tblDiseño.Cliente, ' - ', tblDiseño.Nombre_Diseño) AS ClienteNombre " + // Nueva columna concatenada
                          "FROM tblDiseño " +
                          "WHERE (((tblDiseño.SC_FechaTerminado) BETWEEN @FechaInicial AND @FechaFinal) " +
                          "AND ((tblDiseño.SC_Terminado) = 1) AND ((tblDiseño.SC_Tiemporeal) = 1)) " +
                          "AND ((tblDiseño.Zona) LIKE @Zona) " +
                          "ORDER BY tblDiseño.SC_Dibujante;";

            // Establecer conexión y ejecutar la consulta
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Crear un comando SQL
                using (SqlCommand cmd = new SqlCommand(sSql, conn))
                {
                    // Agregar los parámetros con valores
                    cmd.Parameters.AddWithValue("@FechaInicial", TextBoxFechaInicioRenders.Text);
                    cmd.Parameters.AddWithValue("@FechaFinal", TextBoxFechaFinRenders.Text + " 23:59:59");
                    cmd.Parameters.AddWithValue("@Zona", "%" + DropDownList2.SelectedValue + "%");

                    // Crear un adaptador de datos y llenar el DataTable
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Agregar la fila de "TOTAL VISITAS" al final
                    DataRow totalRow = dt.NewRow();
                    totalRow["ClienteNombre"] = "TOTAL VISITAS"; // Texto de la fila total
                    totalRow["Numero_Diseño"] = dt.Rows.Count.ToString(); // Número total de filas
                    dt.Rows.Add(totalRow);

                    // Asignar el DataTable al DataGrid
                    DataGrid1.DataSource = dt;
                    DataGrid1.DataBind();
                }
            }
        }

        protected void DataGridEstadisticaSC_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            // Verifica si es la última fila (la fila de "TOTAL VISITAS")
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView rowView = (DataRowView)e.Item.DataItem;

                // Comprobar si la fila contiene el texto "TOTAL VISITAS"
                if (rowView["ClienteNombre"].ToString() == "TOTAL VISITAS")
                {
                    // Aplicar formato en negrita a la fila completa
                    e.Item.Font.Bold = true;

                    // También puedes personalizar el formato de las celdas específicas si es necesario
                    e.Item.Cells[0].Text = "TOTAL VISITAS";
                    e.Item.Cells[1].Text = "";
                    e.Item.Cells[2].Text = rowView["Numero_Diseño"].ToString(); // Total de filas
                }
            }
        }

        protected void llenarResumenEstadisticaShowCase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string sSql = "SELECT tblDiseño.Numero_Diseño, tblDiseño.SC_Dibujante, tblDiseño.SC_FechaTerminado, tblDiseño.SC_Fecha, tblDiseño.SC_Terminado, " +
                          "tblDiseño.Asesor, tblDiseño.Nombre_Diseño, tblDiseño.Zona, tblDiseño.SC_Tiemporeal, tblDiseño.Cliente " +
                          "FROM tblDiseño " +
                          "WHERE tblDiseño.SC_FechaTerminado BETWEEN @FechaInicial AND @FechaFinal " +
                          "AND tblDiseño.SC_Terminado = 1 AND tblDiseño.SC_Tiemporeal = 1 " +
                          "AND tblDiseño.Zona LIKE @Zona " +
                          "ORDER BY tblDiseño.SC_Dibujante";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, conn))
                {
                    cmd.Parameters.AddWithValue("@FechaInicial", TextBoxFechaInicioRenders.Text);
                    cmd.Parameters.AddWithValue("@FechaFinal", TextBoxFechaFinRenders.Text + " 23:59:59");
                    cmd.Parameters.AddWithValue("@Zona", "%" + DropDownList2.SelectedValue + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataTable resumenEstadistica = new DataTable();
                    resumenEstadistica.Columns.Add("Nº", typeof(int));
                    resumenEstadistica.Columns.Add("Responsable", typeof(string));
                    resumenEstadistica.Columns.Add("Visit", typeof(int));
                    resumenEstadistica.Columns.Add("%", typeof(decimal));

                    string dibujante = string.Empty;
                    int totalRender = 0;

                    // Contar las visitas por responsable (agrupando por SC_Dibujante)
                    foreach (DataRow row in dt.Rows)
                    {
                        string currentDibujante = row["SC_Dibujante"].ToString();
                        if (dibujante == currentDibujante)
                        {
                            // Incrementar las visitas si es el mismo responsable
                            resumenEstadistica.Rows[resumenEstadistica.Rows.Count - 1]["Visit"] =
                                (int)resumenEstadistica.Rows[resumenEstadistica.Rows.Count - 1]["Visit"] + 1;
                        }
                        else
                        {
                            // Si el responsable cambia, agregar una nueva fila
                            DataRow newRow = resumenEstadistica.NewRow();
                            newRow["Nº"] = resumenEstadistica.Rows.Count + 1;
                            newRow["Responsable"] = currentDibujante;
                            newRow["Visit"] = 1;
                            resumenEstadistica.Rows.Add(newRow);

                            dibujante = currentDibujante;
                        }

                        totalRender++;
                    }

                    // Calcular el porcentaje y agregar la fila de totales
                    foreach (DataRow row in resumenEstadistica.Rows)
                    {
                        row["%"] = ((int)row["Visit"] / (decimal)totalRender) * 100;
                    }

                    // Agregar la fila de totales
                    DataRow totalRow = resumenEstadistica.NewRow();
                    totalRow["Responsable"] = "TOTAL VISITAS";
                    totalRow["Visit"] = totalRender;
                    totalRow["%"] = 100; // Porcentaje total es 100%
                    resumenEstadistica.Rows.Add(totalRow);

                    // Asignar el DataTable al DataGrid
                    DataGridResumenEstadisticaShowCase.DataSource = resumenEstadistica;
                    DataGridResumenEstadisticaShowCase.DataBind();
                }
            }
        }

        protected void DataGridResumenEstadisticaShowCase_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Verifica si la fila es la de "TOTAL VISITAS"
                if (e.Item.Cells[2].Text == "TOTAL VISITAS")
                {
                    e.Item.Cells[0].Text = string.Empty;
                    // Aplica formato en negrita a toda la fila
                    e.Item.Font.Bold = true;
                }
            }
        }

        protected void DataGridDetalleEstadisiticaPorPedido_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Verifica si la fila es la de "TOTAL VISITAS"
                if (e.Item.Cells[1].Text == "T. Ped" || e.Item.Cells[1].Text == "T. Cum")
                {
                    e.Item.Cells[0].Text = string.Empty;

                    // Aplica formato en negrita a toda la fila
                    e.Item.Font.Bold = true;
                }
            }
        }

        protected void DataGridResumenEstadisticaPorPedido_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Verifica si la fila es la de "TOTAL VISITAS"
                if (e.Item.Cells[2].Text == "TOTAL PEDIDOS")
                {
                    e.Item.Cells[0].Text = string.Empty;
                    e.Item.Cells[1].Text = string.Empty;
                    // Aplica formato en negrita a toda la fila
                    e.Item.Font.Bold = true;
                }
            }
        }

        protected void DataGridResumenEstadisticaPorPedido_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridEstadisticaPedido")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridResumenEstadisticaPorPedido.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 
                foreach (DataGridItem item in DataGridResumenEstadisticaPorPedido.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                string realizadoPor = row.Cells[2].Text;

                CargarDetallePedidos(realizadoPor);
            }
        }

        private void CargarDetallePedidos(string realizadoPor)
        {
            DateTime fechaInicial = Convert.ToDateTime(txtPeriodoInicio.Text).Date;

            // Establecer conexión y consulta SQL.
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = @"
    SELECT 
        tblPlano.RealizadoPor,
        tblOT.Id_OT,
        tblOT.Consecutivo_Pedido,
        tblOT.Terminado_Diseño,
        DATEDIFF(DAY, tblOT.Fecha_Entrega_Dibujo_Despiece, tblOT.Fecha_Entrega_Produccion) AS ENTREGA,
        tblOT.Nombre_Obra,
        tblOT.Fecha_Entrega_Produccion,
        tblOT.Fecha_Entrega_Dibujo_Despiece,
        tblOT.Fecha_Despacho_Produccion,
        tblOT.Precio_Venta,
        tblOT.Descuento,
        tblOT.Precio_Venta - tblOT.Precio_Venta * tblOT.Descuento / 100 AS VentaNeta,
        tblOT.Zona,
        tblOT.Codigo_Asesor,
        '' AS Urgente
    FROM tblOT
    RIGHT JOIN tblPlano ON tblOT.Id_OT = tblPlano.Id_OT AND tblOT.Consecutivo_Pedido = tblPlano.Consecutivo_Pedido
    WHERE tblPlano.RealizadoPor LIKE @RealizadoPor
      AND tblOT.Terminado_Diseño = 1
      AND tblOT.Fecha_Entrega_Produccion BETWEEN @FechaInicio AND @FechaFin
      AND tblOT.Zona LIKE @Zona
      AND tblOT.Codigo_Asesor LIKE @CodigoAsesor
    ORDER BY DATEDIFF(DAY, tblOT.Fecha_Entrega_Dibujo_Despiece, tblOT.Fecha_Entrega_Produccion) DESC";

            lblDetallePedidos.Text = $"DETALLE PEDIDOS - {realizadoPor}";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@RealizadoPor", $"%{realizadoPor}%");
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicial);
                cmd.Parameters.AddWithValue("@FechaFin", txtPeriodoFin.Text + " 23:59:59");
                cmd.Parameters.AddWithValue("@CodigoAsesor", ddlAsesor.SelectedItem.Text + "%");
                cmd.Parameters.AddWithValue("@Zona", "%" + ddlZona.SelectedValue + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                int totalPedidos = dataTable.Rows.Count;
                int totalCumplidos = 0;

                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime fechaEntregaDespiece = Convert.ToDateTime(row["Fecha_Entrega_Dibujo_Despiece"]);
                    DateTime fechaEntregaProduccion = Convert.ToDateTime(row["Fecha_Entrega_Produccion"]);

                    int diasNoLaborales = ObtenerDiasNoLaborales(fechaEntregaDespiece, fechaEntregaProduccion);

                    if (fechaEntregaDespiece.AddDays(diasNoLaborales) <= fechaEntregaProduccion)
                    {
                        totalCumplidos++;
                    }
                    else
                    {

                    }
                }

                DataRow filaTPed = dataTable.NewRow();
                filaTPed["Id_OT"] = "T. Ped";
                filaTPed["Nombre_Obra"] = totalPedidos; 
                dataTable.Rows.Add(filaTPed);

                DataRow filaTCum = dataTable.NewRow();
                filaTCum["Id_OT"] = "T. Cum";
                filaTCum["Nombre_Obra"] = "";
                dataTable.Rows.Add(filaTCum);


                DataGridDetalleEstadisiticaPorPedido.DataSource = dataTable;
                DataGridDetalleEstadisiticaPorPedido.DataBind();
                UpdatePanel1.Update();
            }
        }

        private int ObtenerDiasNoLaborales(DateTime fechaInicio, DateTime fechaFin)
        {
            int diasNoLaborales = 0;

            while (fechaInicio <= fechaFin)
            {
                if (fechaInicio.DayOfWeek == DayOfWeek.Saturday || fechaInicio.DayOfWeek == DayOfWeek.Sunday || EsDiaFestivo(fechaInicio))
                {
                    diasNoLaborales++;
                }
                fechaInicio = fechaInicio.AddDays(1);
            }

            return diasNoLaborales;
        }

        private bool EsDiaFestivo(DateTime fecha)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT COUNT(*) FROM tblDiaNoLaboral WHERE dnlFecha = @Fecha";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Fecha", fecha.Date);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        protected void DataGridEstadisticaPorDiseno_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridDiseño")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridEstadisticaPorDiseno.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 
                foreach (DataGridItem item in DataGridEstadisticaPorDiseno.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                string dibujante = row.Cells[2].Text;

                string asesor = ddlAsesor.SelectedItem.Text.Trim();
                DateTime fechaInicio = DateTime.Parse(txtPeriodoInicio.Text);
                DateTime fechaFin = DateTime.Parse(txtPeriodoFin.Text);
                string zona = ddlZona.SelectedValue;

                CargarDetalleEstadistica(asesor, dibujante, fechaInicio, fechaFin, zona);
            }
        }

        protected void CargarDetalleEstadistica(string asesor, string dibujante, DateTime fechaInicio, DateTime fechaFin, string zona)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = @"
SELECT 
   Cliente + ' - ' + Nombre_Diseño AS ClienteYNombreDiseño,
Numero_Diseño,
DATEDIFF(DAY, Fecha_Ingreso, FechaDibujoOK) AS ENTREGA,
FORMAT(UltimaActivacion, 'dd/MM/yyyy HH:mm') AS UltimaActivacion,
FORMAT(Fecha_Programada_Entrega, 'dd/MM/yyyy HH:mm') AS Fecha_Programada_Entrega,
FORMAT(FechaDibujoOK, 'dd/MM/yyyy HH:mm') AS FechaDibujoOK,
Asesor,
Urgente,
SeguimientoPausa,
FORMAT(Fecha_Ingreso, 'dd/MM/yyyy HH:mm') AS Fecha_Ingreso,
Zona
FROM tblDiseño
WHERE 
    FechaDibujoOK BETWEEN @FechaInicio AND @FechaFin
    AND Asesor LIKE @Asesor + '%'
    AND RealizadoPor LIKE @RealizadoPor + '%'
    AND TerminadoDibujo = 1
    AND Zona LIKE '%' + @Zona + '%'
ORDER BY RealizadoPor DESC, Numero_Diseño ASC";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin.AddDays(1).AddSeconds(-1)); // Hasta el final del día
                    command.Parameters.AddWithValue("@Asesor", asesor);
                    command.Parameters.AddWithValue("@RealizadoPor", dibujante);
                    command.Parameters.AddWithValue("@Zona", zona);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }

            int CantTotalDiseño = dataTable.Rows.Count;


            // Procesar los resultados para añadir filas de TOTAL DISEÑOS y CUMPLIDOS
            DataRow rowTotal = dataTable.NewRow();
            rowTotal["ClienteYNombreDiseño"] = "TOTAL DISEÑOS";
            rowTotal["ENTREGA"] = CantTotalDiseño;
            rowTotal["Fecha_Programada_Entrega"] = DBNull.Value;
            dataTable.Rows.Add(rowTotal);


            // Re-binder DataGrid para reflejar los nuevos datos
            DataGridDetalleEstadisticaPorDiseno.DataSource = dataTable;
            DataGridDetalleEstadisticaPorDiseno.DataBind();
        }

        protected void DataGridResumenEstadisticaRenders_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridRender")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridResumenEstadisticaRenders.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 
                foreach (DataGridItem item in DataGridResumenEstadisticaRenders.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                string responsable = row.Cells[2].Text;

                LlenarDataGridEstadisticaRenders(responsable);
            }
        }
        protected void LlenarDataGridEstadisticaRenders(string responsable)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            string sSql = "SELECT tblRender.Cliente, tblRender.Id_Render, tblRender.Nombre_Render, tblRender.Fecha_Ingreso, tblRender.Fecha_Programada_Entrega, tblRender.FechaRenderOK," +
                " tblRender.UltimaActivacion, DATEDIFF(DAY, tblRender.UltimaActivacion, tblRender.FechaRenderOK) as Entrega, tblRender.Asesor, tblRender.RealizadoPor," +
                " tblRender.SeguimientoPausa, tblRender.TerminadoRender, tblRender.Zona,  Cliente + ' - ' + Nombre_Render AS ClienteNombre " +
                         "FROM tblRender " +
                         "WHERE tblRender.FechaRenderOK BETWEEN @FechaInicial AND @FechaFinal " +
                         "AND tblRender.TerminadoRender = 1 " +
                         "AND tblRender.RealizadoPor = @RealizadoPor " +
                         "AND tblRender.Zona LIKE @Zona " +
                         "ORDER BY tblRender.RealizadoPor;";

            using (SqlConnection ConectSID = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, ConectSID))
                {
                    cmd.Parameters.AddWithValue("@FechaInicial", TextBoxFechaInicioRenders.Text);
                    cmd.Parameters.AddWithValue("@FechaFinal", DateTime.Parse(TextBoxFechaFinRenders.Text));
                    cmd.Parameters.AddWithValue("@RealizadoPor", responsable);
                    cmd.Parameters.AddWithValue("@Zona", "%" + DropDownList2.SelectedValue + "%");

                    ConectSID.Open();

                    using (SqlDataReader rsEstadisticasRenders = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(rsEstadisticasRenders);
                        DataGridEstadisticaRenders.DataSource = dt;
                        DataGridEstadisticaRenders.DataBind();
                    }
                }
            }
        }

        protected void DataGridResumenEstadisticaShowCase_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "DatagridSC")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridResumenEstadisticaShowCase.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 
                foreach (DataGridItem item in DataGridResumenEstadisticaShowCase.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                e.Item.CssClass = "fila-seleccionada1";

                string responsable = row.Cells[2].Text;

                LlenarDataGridEstadisticaSC(responsable);
            }
        }

        protected void LlenarDataGridEstadisticaSC(string responsable)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            string sSql = "SELECT tblDiseño.Cliente, tblDiseño.Numero_Diseño, tblDiseño.SC_FechaTerminado, tblDiseño.Asesor, tblDiseño.Zona, Cliente + ' - ' + Nombre_Diseño AS ClienteNombre " +
                         "FROM tblDiseño " +
                         "WHERE tblDiseño.SC_FechaTerminado BETWEEN @FechaInicial AND @FechaFinal " +
                         "AND tblDiseño.SC_Terminado = 1 " +
                         "AND tblDiseño.SC_Dibujante = @Responsable " +
                         "AND tblDiseño.SC_Tiemporeal = 1 " +
                         "AND tblDiseño.Zona LIKE @Zona " +
                         "ORDER BY tblDiseño.SC_Dibujante;";

            using (SqlConnection ConectSID = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sSql, ConectSID))
                {
                    cmd.Parameters.AddWithValue("@FechaInicial", TextBoxFechaInicioRenders.Text);
                    cmd.Parameters.AddWithValue("@FechaFinal", DateTime.Parse(TextBoxFechaFinRenders.Text));
                    cmd.Parameters.AddWithValue("@Responsable", responsable);
                    cmd.Parameters.AddWithValue("@Zona", "%" + DropDownList2.SelectedValue + "%");

                    ConectSID.Open();

                    using (SqlDataReader rsEstadisticaSC = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(rsEstadisticaSC);
                        DataGrid1.DataSource = dt;
                        DataGrid1.DataBind();
                    }
                }
            }
        }

    }
}