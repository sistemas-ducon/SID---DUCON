using OfficeOpenXml.Style;
using OfficeOpenXml;
using SISTEMA_INTEGRAL_DUCON.Formularios.Consultas;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static SISTEMA_INTEGRAL_DUCON.Formularios.OrdenTrabajo;
using static System.Windows.Forms.MonthCalendar;
using System.Globalization;
using NPOI.SS.Formula.Functions;
using DocumentFormat.OpenXml.Wordprocessing;
using TableCell = System.Web.UI.WebControls.TableCell;
using ListItem = System.Web.UI.WebControls.ListItem;
using System.Security.Policy;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Administrativo
{
    public partial class EstadisticasVentas : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        private string CadenaConexionISID = "BD_ISIDSQL";
        private string CadenaConexionSSF = "BD_SSF";

        // Variable para contar las posiciones de los aseores en estadistica venta (tipo pedido)
        private int posCounter = 1;
        // Arreglo para la mantener los totales de cada columna en Estadistica por mes
        double[] totalesColumnas;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["usuariologueado"] != null)
                {
                    DateTime Fecha = DateTime.Now;
                    DateTime FechaPrimerDia = new DateTime(Fecha.Year, Fecha.Month, 1);

                    tbfechaIni.Text = FechaPrimerDia.ToString("yyyy-MM-dd");
                    tbfechaFin.Text = Fecha.ToString("yyyy-MM-dd");

                    //Fecha para  el grafico de ventas por asesor 
                    FechaI.InnerText = FechaPrimerDia.ToString("yyyy-MM-dd");
                    FechaF.InnerText = Fecha.ToString("yyyy-MM-dd");

                    ddlTipoPedido.DataBind();
                    ddlGrupo.DataBind();

                    CrearGrafico();

                    LlenarAño();

                    Session.Remove("NombreColumnaSession");

                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }
            }
        }

        private void LlenarAño()
        {
            // Agregar opciones para los años desde el año actual hasta 1900
            for (int i = DateTime.Now.Year; i >= 1900; i--)
            {
                ddlAnioBusqueda.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlAnioBusquedaT.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlAnioBusquedaCouTri.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlanioBusquedaM.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlBusquedaAñoRango.Items.Add(new ListItem(i.ToString(), i.ToString()));
                
            }
        }

        protected void ddlTipoPedido_DataBound(object sender, EventArgs e)
        {
            ddlTipoPedido.Items.Insert(0, new ListItem("%", "%"));
        }

        protected void ddlGrupo_DataBound(object sender, EventArgs e)
        {
            ddlGrupo.Items.Insert(0, new ListItem("%", "%"));
        }



        // TAP TIPO PEDIDO

        // Consultar Estadisticas 
        protected void btnConsultar_Click(object sender, EventArgs e)
        {


            FechaI.InnerText = tbfechaFin.Text;
            FechaF.InnerText = tbfechaFin.Text;

            string sSql;
            string sSql1 = "";
            string sSql2 = "";
            if (chkAprod.Checked)
            {
                //query para estadisticas 
                sSql = @"SELECT tblOT.Precio_Venta, tblOT.Descuento, Precio_Venta - Precio_Venta * Descuento / 100 AS ValorNeto,
                tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT, tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra,
                tblOT.Codigo_Asesor, tblOT.Nit_Cliente, tblOT.Fecha_Confirmacion_Venta, tblOT.Codigo_Asesor, tblEmpleado.Grupo,
                (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)
                INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                WHERE (((tblOT.Fecha_Confirmacion_Venta) BETWEEN @FechaIni AND @FechaFin)
                AND ((tblTipoPedido.Descripcion_TipoPedido) LIKE '%' + @TipoPedido + '%' ) AND ((tblOT.Anulada) = 0)
                AND ((tblOT.Terminado_Diseño) = @Aproduccion) AND ((tblTipoPedido.EstadisticaVenta) = 1))
                GROUP BY tblOT.Precio_Venta, tblOT.Descuento, tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT,
                tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra, tblOT.Codigo_Asesor, tblOT.Nit_Cliente, tblOT.Fecha_Confirmacion_Venta,
                tblOT.Codigo_Asesor, tblEmpleado.Grupo,tblEmpleado.Nombre,tblEmpleado.Apellidos
                HAVING (((tblEmpleado.Grupo) LIKE '%' + @Grupo + '%')) ORDER BY tblOT.Codigo_Asesor";

                // query para consolidados 
                sSql1 = @"SELECT
                        tblOT.Codigo_Asesor,
                        Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                        tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos As NombreCompleto
                        FROM (tblTipoPedido 
                        INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                        INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                        WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                        AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%' ) 
                        AND ((tblOT.Terminado_Diseño)= @Aproduccion) AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo)  like '%' + @Grupo + '%')) 
                        GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta,tblEmpleado.Nombre, tblEmpleado.Apellidos
                        Having (((tblTipoPedido.EstadisticaVenta) =1))
                        ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";
                //query para la grafica 
                sSql2 = @"SELECT tblOT.Codigo_Asesor
                        FROM tblTipoPedido
                        INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                        INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                        WHERE
                        (tblOT.Fecha_Confirmacion_Venta BETWEEN @FechaIni AND @FechaFin)
                        AND (tblTipoPedido.Descripcion_TipoPedido LIKE @TipoPedido)
                        AND (tblOT.Terminado_Diseño = @Aproduccion)
                        AND (tblOT.Anulada = 0)
                        AND (tblEmpleado.Grupo LIKE '%' + @Grupo + '%')
                        GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta
                        HAVING tblTipoPedido.EstadisticaVenta = 1";



                if (ddlTipoPedido.Text != "%")
                {

                    //query para estadisticas 
                    sSql = @"SELECT tblOT.Precio_Venta, tblOT.Descuento, Precio_Venta - Precio_Venta * Descuento / 100 AS ValorNeto,
                    tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT, tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra,
                    tblOT.Codigo_Asesor, tblOT.Nit_Cliente, tblOT.Fecha_Confirmacion_Venta, tblOT.Codigo_Asesor, tblEmpleado.Grupo,
                    (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto 
                    FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)
                    INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula 
                    WHERE (((tblOT.Fecha_Confirmacion_Venta) BETWEEN @FechaIni AND @FechaFin )
                    AND ((tblTipoPedido.Descripcion_TipoPedido) LIKE '%'+ @TipoPedido + '%') AND ((tblOT.Anulada) = 0)
                    AND ((tblOT.Terminado_Diseño) = @Aproduccion )) 
                    GROUP BY tblOT.Precio_Venta, tblOT.Descuento, tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT,
                    tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra, tblOT.Codigo_Asesor, tblOT.Nit_Cliente, tblOT.Fecha_Confirmacion_Venta,
                    tblOT.Codigo_Asesor, tblEmpleado.Grupo,tblEmpleado.Nombre,tblEmpleado.Apellidos
                    HAVING (((tblEmpleado.Grupo) LIKE '%' + @Grupo + '%')) ORDER BY tblOT.Codigo_Asesor";



                    //query para consolidado
                    sSql1 = @"SELECT 
                            tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)  
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula 
                            WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                            AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)= @Aproduccion ) 
                            AND ((tblOT.Anulada)= 0 ) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%' )) 
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos
                            ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";

                    //query para la grafica
                    sSql2 = @"SELECT 
                            tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)  
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula 
                            WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                            AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)= @Aproduccion ) 
                            AND ((tblOT.Anulada)= 0 ) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%' )) 
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos";


                }
            }
            else
            {

                //query para estadisticas 
                sSql = @"SELECT tblOT.Precio_Venta, tblOT.Descuento, Precio_Venta - Precio_Venta * Descuento / 100 AS ValorNeto,
                tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT, tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra,
                tblOT.Codigo_Asesor, tblOT.Nit_Cliente, tblOT.Fecha_Confirmacion_Venta, tblOT.Codigo_Asesor, tblEmpleado.Grupo,
                 (tblEmpleado.Nombre + ' '+ tblEmpleado.Apellidos) As NombreCompleto 
                FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)
                INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                WHERE (((tblOT.Fecha_Confirmacion_Venta) BETWEEN @FechaIni AND @FechaFin)
                AND ((tblTipoPedido.Descripcion_TipoPedido) LIKE '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño) = 0)
                AND ((tblOT.Anulada) = 0))
                GROUP BY tblOT.Precio_Venta, tblOT.Descuento, tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT,
                tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra, tblOT.Codigo_Asesor, tblOT.Nit_Cliente,
                tblOT.Fecha_Confirmacion_Venta, tblOT.Codigo_Asesor, tblEmpleado.Grupo, tblEmpleado.Grupo,tblEmpleado.Nombre,tblEmpleado.Apellidos
                HAVING (((tblEmpleado.Grupo) LIKE '%' + @Grupo + '%')) ORDER BY tblOT.Codigo_Asesor";


                //query para consolidado
                sSql1 = @"SELECT
                        tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                        (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                         FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                         INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                         WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                         AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') 
                         AND ((tblOT.Terminado_Diseño)= 0) AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo)  like '%' + @Grupo + '%'))
                         GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta,tblEmpleado.Nombre, tblEmpleado.Apellidos                      
                         ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";

                //query para la grafica
                sSql2 = @"SELECT
                        tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                        (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                         FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                         INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                         WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                         AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') 
                         AND ((tblOT.Terminado_Diseño)= 0) AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo)  like '%' + @Grupo + '%'))
                         GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta,tblEmpleado.Nombre, tblEmpleado.Apellidos ";

                if (ddlTipoPedido.Text != "%")
                {
                    //query para estadisticas 
                    sSql = @"SELECT tblOT.Precio_Venta, tblOT.Descuento, Precio_Venta - Precio_Venta * Descuento / 100 AS ValorNeto,
                    tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT, tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra, tblOT.Codigo_Asesor,
                    tblOT.Nit_Cliente, tblOT.Fecha_Confirmacion_Venta, tblOT.Codigo_Asesor, tblEmpleado.Grupo,
                    (tblEmpleado.Nombre + ' '+ tblEmpleado.Apellidos) As NombreCompleto
                    FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)
                    INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                    WHERE (((tblOT.Fecha_Confirmacion_Venta) BETWEEN @FechaIni AND @FechaFin)
                    AND ((tblTipoPedido.Descripcion_TipoPedido) LIKE '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño) = 0) 
                    AND ((tblOT.Anulada) = 0))
                    GROUP BY tblOT.Precio_Venta, tblOT.Descuento, tblTipoPedido.Descripcion_TipoPedido, tblOT.Id_OT,
                    tblOT.Consecutivo_Pedido, tblOT.Nombre_Obra, tblOT.Codigo_Asesor, tblOT.Nit_Cliente, tblOT.Fecha_Confirmacion_Venta,
                    tblOT.Codigo_Asesor, tblEmpleado.Grupo, tblEmpleado.Grupo,tblEmpleado.Nombre,tblEmpleado.Apellidos
                    HAVING (((tblEmpleado.Grupo) LIKE '%' + @Grupo + '%')) ORDER BY tblOT.Codigo_Asesor";


                    //query para consolidado
                    sSql1 = @"SELECT
                            tblOT.Codigo_Asesor,
                            Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                             WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin)
                             AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)=0) 
                             AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%'))
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos
                            ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";

                    //query para la grafica
                    sSql2 = @"SELECT
                            tblOT.Codigo_Asesor,
                            Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                             WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin)
                             AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)=0) 
                             AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%'))
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos ";


                }
            }


            CargarDataGrid(sSql);

            DataTable dtAsesoresConTotal = ObtenerAsesoresConTotal(sSql1);
            DataTable dtAsesoresSinTotal = ObtenerAsesoresSinTotal(sSql2);

            DataTable dtAsesoresFusionados = FusionarAsesores(dtAsesoresConTotal, dtAsesoresSinTotal);


            // Creamos una variable para almacenar la suma total
            decimal totalSum = 0;

            foreach (DataRow row in dtAsesoresFusionados.Rows)
            {
                // Obtenemos el valor de la columna "Total" de la fila actual
                if (decimal.TryParse(row["Total"].ToString(), out decimal total))
                {
                    // Sumar al total
                    totalSum += total;
                }
            }

            // Creamos una nueva fila para el total
            DataRow totalRow = dtAsesoresFusionados.NewRow();
            totalRow["Codigo_Asesor"] = "Total";
            totalRow["Total"] = totalSum;

            // Agregamos la fila al DataTable
            dtAsesoresFusionados.Rows.Add(totalRow);

            DataGridXAsesor.DataSource = dtAsesoresFusionados;
            DataGridXAsesor.DataBind();

            CrearGrafico();
        }

        // Obtener los asesores con total
        private DataTable ObtenerAsesoresConTotal(string sSql1)
        {
            DataTable dtAsesoresConTotal = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sSql1, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@FechaIni", tbfechaIni.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@FechaFin", tbfechaFin.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@TipoPedido", ddlTipoPedido.SelectedItem.Text == "" ? "%" : ddlTipoPedido.SelectedItem.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@Grupo", ddlGrupo.SelectedItem.Text == "" ? "%" : ddlGrupo.SelectedItem.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@Aproduccion", chkAprod.Checked.ToString());
                    adapter.Fill(dtAsesoresConTotal);
                }
            }

            return dtAsesoresConTotal;
        }

        // Oobtener los asesores sin total
        private DataTable ObtenerAsesoresSinTotal(string sSql2)
        {
            string sSqlAsesoresSinTotal = @"SELECT tblAsesorComercial.Cedula As Codigo_Asesor, tblAsesorComercial.Nombre + tblAsesorComercial.Apellidos As NombreCompleto 
                                    FROM tblEmpleado
                                    INNER JOIN tblAsesorComercial ON tblEmpleado.Cedula = tblAsesorComercial.Cedula
                                    WHERE tblAsesorComercial.Activo = 1
                                    AND tblEmpleado.Grupo LIKE '%' + @Grupo + '%'
                                    AND tblAsesorComercial.Cedula NOT IN  (SELECT Codigo_Asesor
                                     FROM ( " + sSql2 + " ) AS asesoresConTotal);";

            DataTable dtAsesoresSinTotal = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sSqlAsesoresSinTotal, connection))
                {

                    adapter.SelectCommand.Parameters.AddWithValue("@FechaIni", tbfechaIni.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@FechaFin", tbfechaFin.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@TipoPedido", ddlTipoPedido.SelectedItem.Text == "" ? "%" : ddlTipoPedido.SelectedItem.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@Grupo", ddlGrupo.SelectedItem.Text == "" ? "%" : ddlGrupo.SelectedItem.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@Aproduccion", chkAprod.Checked.ToString());
                    adapter.Fill(dtAsesoresSinTotal);
                }
            }

            return dtAsesoresSinTotal;
        }

        // Método para fusionar los resultados de ambos DataTables
        private DataTable FusionarAsesores(DataTable dtAsesoresConTotal, DataTable dtAsesoresSinTotal)
        {
            DataTable dtAsesoresFusionados = dtAsesoresConTotal.Copy();
            dtAsesoresFusionados.Merge(dtAsesoresSinTotal);
            return dtAsesoresFusionados;
        }

        private void CrearGrafico()
        {

            string sSql1 = "";
            string sSql2 = "";
            if (chkAprod.Checked)
            {

                sSql1 = @"SELECT
                        tblOT.Codigo_Asesor,
                        Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                        tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos As NombreCompleto
                        FROM (tblTipoPedido 
                        INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                        INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                        WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                        AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%' ) 
                        AND ((tblOT.Terminado_Diseño)= @Aproduccion) AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo)  like '%' + @Grupo + '%')) 
                        GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta,tblEmpleado.Nombre, tblEmpleado.Apellidos
                        Having (((tblTipoPedido.EstadisticaVenta) =1))
                        ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";

                sSql2 = @"SELECT tblOT.Codigo_Asesor
                        FROM tblTipoPedido
                        INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                        INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                        WHERE
                        (tblOT.Fecha_Confirmacion_Venta BETWEEN @FechaIni AND @FechaFin)
                        AND (tblTipoPedido.Descripcion_TipoPedido LIKE @TipoPedido)
                        AND (tblOT.Terminado_Diseño = @Aproduccion)
                        AND (tblOT.Anulada = 0)
                        AND (tblEmpleado.Grupo LIKE '%' + @Grupo + '%')
                        GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta
                        HAVING tblTipoPedido.EstadisticaVenta = 1";



                if (ddlTipoPedido.Text != "%")
                {

                    sSql1 = @"SELECT 
                            tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)  
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula 
                            WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                            AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)= @Aproduccion ) 
                            AND ((tblOT.Anulada)= 0 ) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%' )) 
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos
                            ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";


                    sSql2 = @"SELECT 
                            tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido)  
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula 
                            WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                            AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)= @Aproduccion ) 
                            AND ((tblOT.Anulada)= 0 ) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%' )) 
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos";


                }
            }
            else
            {


                sSql1 = @"SELECT
                        tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                        (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                         FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                         INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                         WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                         AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') 
                         AND ((tblOT.Terminado_Diseño)= 0) AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo)  like '%' + @Grupo + '%'))
                         GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta,tblEmpleado.Nombre, tblEmpleado.Apellidos                      
                         ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";

                sSql2 = @"SELECT
                        tblOT.Codigo_Asesor, Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                        (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                         FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                         INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                         WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin) 
                         AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') 
                         AND ((tblOT.Terminado_Diseño)= 0) AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo)  like '%' + @Grupo + '%'))
                         GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta,tblEmpleado.Nombre, tblEmpleado.Apellidos ";

                if (ddlTipoPedido.Text != "%")
                {


                    sSql1 = @"SELECT
                            tblOT.Codigo_Asesor,
                            Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                             WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin)
                             AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)=0) 
                             AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%'))
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos
                            ORDER BY Sum(Precio_Venta-Precio_Venta*Descuento/100) DESC";

                    sSql2 = @"SELECT
                            tblOT.Codigo_Asesor,
                            Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total,
                            (tblEmpleado.Nombre + ' ' + tblEmpleado.Apellidos) As NombreCompleto
                            FROM (tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido) 
                            INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula
                             WHERE (((tblOT.Fecha_Confirmacion_Venta) Between @FechaIni And @FechaFin)
                             AND ((tblTipoPedido.Descripcion_TipoPedido) Like '%' + @TipoPedido + '%') AND ((tblOT.Terminado_Diseño)=0) 
                             AND ((tblOT.Anulada)=0) AND ((tblEmpleado.Grupo) like '%' + @Grupo + '%'))
                            GROUP BY tblOT.Codigo_Asesor,tblEmpleado.Nombre, tblEmpleado.Apellidos ";


                }

            }

            DataTable dtAsesoresConTotal = ObtenerAsesoresConTotal(sSql1);
            DataTable dtAsesoresSinTotal = ObtenerAsesoresSinTotal(sSql2);

            DataTable dtAsesoresFusionados = FusionarAsesores(dtAsesoresConTotal, dtAsesoresSinTotal);

            List<string> nombres = new List<string>();
            List<double> cantidades = new List<double>();

            foreach (DataRow row in dtAsesoresFusionados.Rows)
            {
                // Obtener el nombre de la fila actual y agregarlo a la lista de nombres
                string nombre = row["NombreCompleto"].ToString();
                nombres.Add(nombre);

                // Obtener la cantidad de la fila actual y agregarla a la lista de cantidades
                double cantidad;
                if (double.TryParse(row["Total"].ToString(), out cantidad))
                {
                    cantidades.Add(cantidad);
                }
                else
                {
                    // Manejar el caso en que la cantidad no se pueda convertir a double
                    // Por ejemplo, agregar un valor predeterminado o manejar el error de alguna otra manera
                }
            }


            // Generamos la gráfica con los datos obtenidos
            string script = string.Format(@"var nombres = {0}; var cantidades = {1};
                                  GenerarGrafica1(nombres, cantidades);",
                                          new JavaScriptSerializer().Serialize(nombres),
                                          new JavaScriptSerializer().Serialize(cantidades));

            ScriptManager.RegisterStartupScript(this, GetType(), "GenerarGrafica1", script, true);




        }

        // Cargar DataGrid Estadisticas Tipo Pedido
        private void CargarDataGrid(string sSql)
        {
            DSEstadisticaVenta.SelectCommand = sSql;


            DSEstadisticaVenta.SelectParameters["FechaIni"].DefaultValue = tbfechaIni.Text;
            DSEstadisticaVenta.SelectParameters["FechaFin"].DefaultValue = tbfechaFin.Text;

            DSEstadisticaVenta.SelectParameters["TipoPedido"].DefaultValue = ddlTipoPedido.SelectedItem.Text;



            DSEstadisticaVenta.SelectParameters["Grupo"].DefaultValue = ddlGrupo.SelectedItem.Text;
            DSEstadisticaVenta.SelectParameters["Aproduccion"].DefaultValue = chkAprod.Checked.ToString();


            DSEstadisticaVenta.DataBind();

            DatagridEstVentas.DataSource = DSEstadisticaVenta;
            DatagridEstVentas.DataBind();
        }

        protected void DatagridEstVentas_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int currentIndex = e.Item.ItemIndex;

                // Verifica si no es la primera fila
                if (currentIndex > -1)
                {
                    // Obtiene el código de asesor de la fila actual
                    string AsesorActual = DataBinder.Eval(e.Item.DataItem, "Codigo_Asesor").ToString();


                    if (AsesorActual == Session["AsesorAnterior"]?.ToString() && Session["AsesorAnterior"]?.ToString() != null)
                    {
                        Session["AsesorAnterior"] = DataBinder.Eval(e.Item.DataItem, "Codigo_Asesor").ToString();
                        e.Item.BackColor = System.Drawing.Color.White;

                    }
                    else
                    {
                        Session["AsesorAnterior"] = DataBinder.Eval(e.Item.DataItem, "Codigo_Asesor").ToString();
                        e.Item.Cells[0].BackColor = System.Drawing.Color.LightGray;

                    }

                }
            }
        }

        protected void DatagridEstVentas_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerDetalle")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DatagridEstVentas.Items[rowIndex];


                string cedula = row.Cells[1].Text;

                ced.Text = cedula;

                foreach (DataGridItem item in DatagridEstVentas.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  
                e.Item.CssClass = "fila-seleccionada";

                foreach (DataGridItem item in DataGridXAsesor.Items)
                {
                    if (item.Cells[1].Text == cedula)
                    {
                        // Establecer el estilo de la fila correspondiente en el segundo DataGrid
                        item.CssClass = "fila-seleccionada";
                    }
                    else
                    {
                        // Eliminar el estilo de las filas que no coinciden
                        item.CssClass = "";
                    }
                }

                CrearGrafico();

            }
        }

        protected void DataGridXAsesor_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableCell POS = e.Item.Cells[0];
                POS.Text = posCounter.ToString();
                posCounter++;


                TableCell total = e.Item.Cells[3];
                if (total.Text == "" || total.Text == "&nbsp;")
                {
                    total.Text = "0"; // Establece el valor en cero
                }

                if (decimal.TryParse(total.Text, out decimal cellValue))
                {
                    // Formatear el número con dos decimales
                    total.Text = cellValue.ToString("N2");
                }

                double tot = Convert.ToDouble(total.Text.Replace("&nbsp;", "0"));

                double SumaTotal = ObtenerSumaTotal();

                TableCell Porcentaje = e.Item.Cells[4];

                Porcentaje.Text = (tot / SumaTotal * 100).ToString("N2");

            }
        }

        private double ObtenerSumaTotal()
        {
            double Total = 0;

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "SELECT SUM(Total) AS TotalGeneral FROM ( SELECT SUM(Precio_Venta - Precio_Venta * Descuento / 100) AS Total " +
                                "FROM tblTipoPedido   INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido " +
                                "INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula  " +
                                "WHERE tblOT.Fecha_Confirmacion_Venta BETWEEN @FechaIni AND @FechaFin AND tblTipoPedido.Descripcion_TipoPedido LIKE '%' + @TipoPedido + '%'" +
                                "AND tblOT.Terminado_Diseño = @Aproduccion  AND tblOT.Anulada = 0  AND tblEmpleado.Grupo LIKE '%' + @Grupo + '%'" +
                                "   GROUP BY tblOT.Codigo_Asesor, tblTipoPedido.EstadisticaVenta, tblEmpleado.Nombre, tblEmpleado.Apellidos" +
                                "   HAVING tblTipoPedido.EstadisticaVenta = 1) AS Subquery;";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionISID))
                {
                    cmd.Parameters.AddWithValue("@FechaIni", tbfechaIni.Text);
                    cmd.Parameters.AddWithValue("@FechaFin", tbfechaFin.Text);
                    if (ddlGrupo.SelectedItem.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@Grupo", "%");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Grupo", ddlGrupo.SelectedItem.Text);
                    }
                    if (ddlTipoPedido.SelectedItem.Text == "")
                    {
                        cmd.Parameters.AddWithValue("@TipoPedido", "%");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@TipoPedido", ddlTipoPedido.SelectedItem.Text);
                    }

                    cmd.Parameters.AddWithValue("@Aproduccion", chkAprod.Checked);


                    object result = cmd.ExecuteScalar();

                    // Verificar si el resultado no es nulo y convertirlo a double
                    if (result != null && result != DBNull.Value)
                    {
                        Total = Convert.ToDouble(result);
                    }
                }

                return Total;
            }
        }

        // Generar Excel Estadística Venta Tipo Pedido
        protected void ExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos un paquete de Excel 
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregamos la hoja  1
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Consolidado  Por Asesor ");

                    worksheet.Row(1).Height = 60;
                    worksheet.Column(2).Width = 40;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(3).Width = 20;
                    worksheet.Column(4).Width = 20;

                    // Logo Ducon       // validar la ruta de este logo y no se debe eliminar 
                    string rutaImagen = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                    FileInfo image = new FileInfo(rutaImagen);
                    if (image.Exists)
                    {
                        var picture = worksheet.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    var CellC1F1 = worksheet.Cells["D1:F1"];
                    CellC1F1.Merge = true;
                    CellC1F1[1, 4].Value = "Consolidado de Venta por Asesor ";
                    CellC1F1.Style.Font.Name = "Century Gothic";
                    CellC1F1.Style.Font.Size = 17;
                    CellC1F1.Style.Font.Bold = true;
                    CellC1F1.Style.Font.Italic = true;
                    CellC1F1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellC1F1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellC1F1.Style.Font.Color.SetColor(System.Drawing.Color.Black);


                    var CellC2F2 = worksheet.Cells["B2:F2"];
                    CellC2F2.Merge = true;
                    CellC2F2[2, 2].Value = ddlTipoPedido.SelectedItem.Text + " Periodo del " + tbfechaIni.Text + " al " + tbfechaFin.Text;
                    CellC2F2.Style.Font.Name = "Century Gothic";
                    CellC2F2.Style.Font.Size = 15;
                    CellC2F2.Style.Font.Bold = true;
                    CellC2F2.Style.Font.Italic = true;
                    CellC2F2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellC2F2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellC2F2.Style.Font.Color.SetColor(System.Drawing.Color.Black);


                    // Obtener los encabezados de las columnas del DataGrid
                    int colIndex = 2;
                    foreach (DataGridColumn column in DataGridXAsesor.Columns)
                    {
                        worksheet.Cells[4, colIndex].Value = column.HeaderText;
                        var headerCell = worksheet.Cells[4, colIndex];

                        // Establecemos el texto en negrita
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Font.Name = "Century Gothic";
                        headerCell.Style.Font.Size = 13;
                        headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        // Aplicamos bordes a la celda de encabezado
                        headerCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;


                        colIndex++;

                    }

                    // Obtener los datos de las filas del DataGrid
                    int rowIndex = 5;

                    foreach (DataGridItem row in DataGridXAsesor.Items)
                    {
                        colIndex = 2;
                        foreach (TableCell cell in row.Cells)
                        {
                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                            var Dato = worksheet.Cells[rowIndex, colIndex];


                            Dato.Style.Font.Size = 11;
                            Dato.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            Dato.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            Dato.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            Dato.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            Dato.Style.Border.Right.Style = ExcelBorderStyle.Thin;


                            colIndex++;
                        }
                        rowIndex++;
                    }


                    ExcelWorksheet worksheet2 = excelPackage.Workbook.Worksheets.Add("Estadisticas de Ventas ");

                    worksheet2.Row(1).Height = 60;
                    worksheet2.Column(2).Width = 40;
                    worksheet2.Column(5).Width = 20;
                    worksheet2.Column(3).Width = 20;
                    worksheet2.Column(4).Width = 20;

                    // Logo Ducon       // validar la ruta de este logo y no se debe eliminar 

                    if (image.Exists)
                    {
                        var picture = worksheet2.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    var CellP2C1F1 = worksheet2.Cells["D1:F1"];
                    CellP2C1F1.Merge = true;
                    CellP2C1F1[1, 4].Value = "Estadística Ventas ";
                    CellP2C1F1.Style.Font.Name = "Century Gothic";
                    CellP2C1F1.Style.Font.Size = 17;
                    CellP2C1F1.Style.Font.Bold = true;
                    CellP2C1F1.Style.Font.Italic = true;
                    CellP2C1F1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellP2C1F1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellP2C1F1.Style.Font.Color.SetColor(System.Drawing.Color.Black);


                    var CellP2C2F2 = worksheet2.Cells["B2:F2"];
                    CellP2C2F2.Merge = true;
                    CellP2C2F2[2, 2].Value = ddlTipoPedido.SelectedItem.Text + " Periodo del " + tbfechaIni.Text + " al " + tbfechaFin.Text;
                    CellP2C2F2.Style.Font.Name = "Century Gothic";
                    CellP2C2F2.Style.Font.Size = 15;
                    CellP2C2F2.Style.Font.Bold = true;
                    CellP2C2F2.Style.Font.Italic = true;
                    CellP2C2F2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellP2C2F2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellP2C2F2.Style.Font.Color.SetColor(System.Drawing.Color.Black);



                    // Obtener los encabezados de las columnas del DataGrid
                    int colIndex1 = 1; // Comenzar desde la segunda columna
                    foreach (DataGridColumn column in DatagridEstVentas.Columns)
                    {
                        if (colIndex1 != 1)
                        {
                            worksheet2.Cells[4, colIndex1].Value = column.HeaderText;
                            var headerCell1 = worksheet2.Cells[4, colIndex1];

                            // Establecemos el texto en negrita
                            headerCell1.Style.Font.Bold = true;
                            headerCell1.Style.Font.Name = "Century Gothic";
                            headerCell1.Style.Font.Size = 13;
                            headerCell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            // Aplicamos bordes a la celda de encabezado
                            headerCell1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            headerCell1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            headerCell1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            headerCell1.Style.Border.Right.Style = ExcelBorderStyle.Thin;


                        }

                        colIndex1++;

                    }

                    // Obtener los datos de las filas del DataGrid
                    int rowIndex1 = 5;

                    foreach (DataGridItem row in DatagridEstVentas.Items)
                    {
                        colIndex1 = 1; // Comenzar desde la segunda columna
                        foreach (TableCell cell in row.Cells)
                        {
                            // Verificar si no es la primera columna
                            if (colIndex1 != 1)
                            {
                                worksheet2.Cells[rowIndex1, colIndex1].Value = cell.Text;
                                var Dato = worksheet2.Cells[rowIndex1, colIndex1];


                                Dato.Style.Font.Size = 11;
                                Dato.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                Dato.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                Dato.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                Dato.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                Dato.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            }

                            colIndex1++;
                        }
                        rowIndex1++;
                    }

                    // Guardamos el archivo de Excel
                    string filePath = Path.GetTempFileName() + ".xlsx";
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);

                    // Descargamos el archivo de Excel
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=EstadisticasVenta.xlsx");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
            catch
            {
                tbMensaje.Text = "Ocurrió un error al intentar descargar el excel";
            }
        }


        // TAP X MESES 

        protected void DataGridEstXMes_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                // Encabezado del DataGrid
                e.Item.Cells[0].Text = "Mes/Años";
                totalesColumnas = new double[e.Item.Cells.Count - 1];
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Contenido del DataGrid (filas de datos)

                int NumeroMes = e.Item.DataSetIndex + 1; // Número del mes (1 para enero, 2 para febrero, etc.)
                if (NumeroMes <= 12)
                {
                    string NombreMes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(NumeroMes); // Obtenemos el nombre del mes
                    e.Item.Cells[0].Text = $"<b><i>{NombreMes.ToUpper()}</i></b>"; // Establecemos el nombre de los meses en la primera columna
                }
                else
                {
                    e.Item.Cells[0].Text = $"<b><i>Total</i></b>"; // Ponemos el Titulo Total  en la primera columna 

                    // Mostramos el total de cada columna en la última fila del DataGrid
                    for (int i = 1; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].Text = $"<b>{totalesColumnas[i - 1]:N0}</b>"; // Mostramos el total de la columna formateado con separador de miles
                    }
                }

                // Calculamos el total de cada columna
                for (int i = 1; i < e.Item.Cells.Count; i++) // Empezamos desde 1 para omitir la primera columna de los meses
                {
                    double valorCelda;
                    if (double.TryParse(e.Item.Cells[i].Text, out valorCelda))
                    {
                        // Acumulamos el valor de la celda en el total de la columna correspondiente
                        totalesColumnas[i - 1] += valorCelda;
                    }
                }
            }
        }

        // Añadir el año de consulta
        protected void AddAnio_Click(object sender, EventArgs e)
        {
            string año = ddlAnioBusqueda.SelectedItem.Text;
            string zona = ddlZona.SelectedItem.Text;
            string nombreColumna = año + "-(" + zona + ")";


            // Agregamos el nombre de la columna a la lista de nombres de columnas en la sesión
            string columnasSession = Session["NombreColumnaSession"] as string;
            if (string.IsNullOrEmpty(columnasSession))
            {
                // Si la sesión aún no tiene nombres de columnas, solo asigna el nombre actual
                Session["NombreColumnaSession"] = nombreColumna;
            }
            else
            {

                if (Session["NombreColumnaSession"].ToString().Contains(nombreColumna))
                {
                    // La columna ya existe ¿ Realmente necesitan agregar varias veces el mismo año ?
                }
                else
                {
                    // Agrega el nombre de la nueva columna a los nombres de columna existentes en la sesión
                    Session["NombreColumnaSession"] = columnasSession + ";" + nombreColumna;
                }

            }

            // Obtenemos todos los nombres de columnas de la variable de  sesión y dividirlos en un arreglo
            string[] nombresColumnas = Session["NombreColumnaSession"].ToString().Split(';');

            // Creamos una nueva tabla para almacenar todos los datos de las columnas
            DataTable datosCompletos = new DataTable();

            // Creamos columnas en datosCompletos para cada nombre de columna en nombresColumnas
            foreach (string nombre in nombresColumnas)
            {
                datosCompletos.Columns.Add(nombre);
            }

            // Agregamos 13 filas vacías a datosCompletos
            for (int i = 0; i < 13; i++)
            {
                datosCompletos.Rows.Add(datosCompletos.NewRow());
            }


            foreach (string nombre in nombresColumnas)
            {

                // Se crea una nueva columna 
                BoundColumn nuevaColumna = new BoundColumn();

                // Asignamos un nombre único a la nueva columna
                nuevaColumna.DataField = nombre;

                // Asignamos un encabezado único a la nueva columna
                nuevaColumna.HeaderText = nombre;

                // Agregamos la columna al DataGrid
                DataGridEstXMes.Columns.Add(nuevaColumna);


                // Encontraemos el índice del primer paréntesis abierto "(" y cerrado ")"
                int indiceInicio = nombre.IndexOf('(');
                int indiceFin = nombre.IndexOf(')');

                // Extraer el texto entre los paréntesis que es la Zona 
                string zona1 = nombre.Substring(indiceInicio + 1, indiceFin - indiceInicio - 1);

                //Extraemos el año 
                string año1 = nombre.Substring(0, 4);

                // Obtenemos los datos para la columna actual
                DataTable datosColumna = ObtenerSumaMes(nombre, año1, zona1);




                // Agregamos las primeras 12 filas de datosColumna a datosCompletos
                for (int i = 0; i < 12 && i < datosColumna.Rows.Count; i++)
                {
                    // Obtenemos la fila en la posición i de datosCompletos
                    DataRow fila = datosCompletos.Rows[i];

                    // Establecemos el valor de la columna correspondiente en esta fila
                    fila[nombre] = datosColumna.Rows[i][nombre];

                }
            }

            // Obtenemos los datos para la columna actual para generar el grafico
            DataTable datosColumna1 = ObtenerSumaMes(nombreColumna, año, zona);

            List<string> meses = new List<string>()
                    {
                        "Enero",
                        "Febrero",
                        "Marzo",
                        "Abril",
                        "Mayo",
                        "Junio",
                        "Julio",
                        "Agosto",
                        "Septiembre",
                        "Octubre",
                        "Noviembre",
                        "Diciembre"
                    };

            List<double> cantidades = new List<double>();

            foreach (DataRow row in datosColumna1.Rows)
            {


                // Obtener la cantidad de la fila actual y agregarla a la lista de cantidades
                double cantidad;
                if (double.TryParse(row[nombreColumna].ToString(), out cantidad))
                {
                    cantidades.Add(cantidad);
                }
                else
                {
                    // Manejar el caso en que la cantidad no se pueda convertir a double
                    // Por ejemplo, agregar un valor predeterminado o manejar el error de alguna otra manera
                }
            }

            string zonaSpan = "";
            if (zona == "%")
            {
                zonaSpan = "Todas";
            }
            else
            {
                zonaSpan = zona;
            }
            SpanAño.InnerText = año + " Zona: " + zonaSpan;

            // Generamos la gráfica con los datos obtenidos
            string script = string.Format(@"var nombres = {0}; var cantidades = {1};
                                  GenerarGrafica2(nombres, cantidades);",
                                          new JavaScriptSerializer().Serialize(meses),
                                          new JavaScriptSerializer().Serialize(cantidades));
            ScriptManager.RegisterStartupScript(this, GetType(), "GenerarGrafica2", script, true);



            DataGridEstXMes.DataSource = datosCompletos;
            DataGridEstXMes.DataBind();
        }

        // Obtener la suma venta por meses  de un Año especifico
        public DataTable ObtenerSumaMes(string nombre, string añoConsulta, string zona)
        {
            DataTable totalMes = new DataTable();

            // Agregar columna con el nombre proporcionado
            totalMes.Columns.Add(nombre, typeof(string)); // Aquí cambiamos el tipo de dato a string

            for (int mes = 1; mes <= 12; mes++)
            {
                string FechaIni = ObtenerPrimerDiaMes(añoConsulta, mes);
                string FechaFin = ObtenerUltimoDiaMes(FechaIni);


                // Consulta para traer la suma de las ventas de cada mes (todos, Zona 1 ó 2 )
                string query = $"SELECT CAST(SUM(Precio_Venta - Precio_Venta * Descuento / 100) AS BIGINT) AS [{nombre}] " +
                               $"FROM tblTipoPedido " +
                               $"INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido " +
                               $"WHERE (tblOT.Fecha_Confirmacion_Venta BETWEEN @FechaIni AND @FechaFin) " +
                               $"AND (tblTipoPedido.EstadisticaVenta = 1) " +
                               $"AND (tblOT.Zona LIKE @Zona) " +
                               $"AND (tblOT.anulada = 0)";

                string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionStringSID))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {

                        adapter.SelectCommand.Parameters.AddWithValue("@FechaIni", FechaIni);
                        adapter.SelectCommand.Parameters.AddWithValue("@FechaFin", FechaFin);
                        adapter.SelectCommand.Parameters.AddWithValue("@Zona", zona);

                        adapter.Fill(totalMes);
                    }
                }
            }

            //  Ciclo para poner cero en los campos que vienen vacios o nulos de la BD
            foreach (DataRow row in totalMes.Rows)
            {
                if (!row.IsNull(nombre))
                {
                    row[nombre] = Convert.ToInt64(row[nombre]).ToString("#,0");
                }
                else
                {
                    row[nombre] = 0;
                }
            }
            return totalMes;
        }

        // Obtener el primer día del mes teniendo el año de consulta
        private string ObtenerPrimerDiaMes(string añoConsulta, int mes)
        {
            DateTime fecha = new DateTime(Convert.ToInt32(añoConsulta), mes, 1);
            return fecha.ToString("yyyy/MM/dd");
        }

        // Obtener el último día del mesteniendo el año de consulta
        private string ObtenerUltimoDiaMes(string fechaConsulta)
        {
            // Convertir la cadena de fecha a DateTime
            DateTime fecha = DateTime.ParseExact(fechaConsulta, "yyyy/MM/dd", CultureInfo.InvariantCulture);

            // Obtener el último día del mes
            DateTime ultimoDiaMes = new DateTime(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month));

            // Formatear el último día del mes como una cadena en el formato deseado
            return ultimoDiaMes.ToString("yyyy/MM/dd");
        }

        //Limpiar estadisticas X Meses
        protected void Limpiar_Click(object sender, EventArgs e)
        {
            Session.Remove("NombreColumnaSession");
            DataGridEstXMes.DataBind();

        }

        protected void ExportarExcel2_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos un paquete de Excel 
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregamos la hoja  1
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Estadisticas Venta ");

                    worksheet.Row(1).Height = 60;
                    worksheet.Column(2).Width = 40;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(3).Width = 20;
                    worksheet.Column(4).Width = 20;

                    // Logo Ducon       // validar la ruta de este logo y no se debe eliminar 
                    string rutaImagen = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                    FileInfo image = new FileInfo(rutaImagen);
                    if (image.Exists)
                    {
                        var picture = worksheet.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    var CellC1F1 = worksheet.Cells["C1:E1"];
                    CellC1F1.Merge = true;
                    CellC1F1[1, 3].Value = "Comparacion Clientes Por Mes";
                    CellC1F1.Style.Font.Name = "Century Gothic";
                    CellC1F1.Style.Font.Size = 17;
                    CellC1F1.Style.Font.Bold = true;
                    CellC1F1.Style.Font.Italic = true;
                    CellC1F1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellC1F1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellC1F1.Style.Font.Color.SetColor(System.Drawing.Color.Black);



                    int headerIndex = 2;
                    int rowIndex = 3;

                    // Encabezados de la tabla                  
                    string[] encabezados = { "Mes/Año" };

                    string columnasSession = Session["NombreColumnaSession"] as string;
                    if (!string.IsNullOrEmpty(columnasSession))
                    {
                        // Dividir los nombres de las columnas existentes en un array
                        string[] columnasExist = columnasSession.Split(';');

                        // Concatenar los arrays de encabezados existentes y nuevos
                        encabezados = encabezados.Concat(columnasExist).ToArray();
                    }


                    string[] Meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre", "Total" };

                    int fila = 4;
                    int col = 2;
                    foreach (string Mes in Meses)
                    {
                        worksheet.Cells[fila, col].Value = Mes;
                        var filaSty = worksheet.Cells[fila, col];

                        // Establecemos el texto en negrita
                        filaSty.Style.Font.Bold = true;
                        filaSty.Style.Font.Name = "Calibri";

                        // Aplicamos bordes a la celda de encabezado
                        filaSty.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        filaSty.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        filaSty.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        filaSty.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        fila++;
                    }



                    foreach (string encabezado in encabezados)
                    {
                        worksheet.Cells[rowIndex, headerIndex].Value = encabezado;
                        var headerCell = worksheet.Cells[rowIndex, headerIndex];

                        // Establecemos el texto en negrita
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Font.Name = "Century Gothic";
                        headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Aplicamos bordes a la celda de encabezado
                        headerCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        headerIndex++;
                    }

                    DataTable datosColumna;
                    rowIndex++;
                    int colIndex = 3;
                    int contador = 1;
                    double suma = 0;

                    foreach (string encabezado in encabezados)
                    {
                        if (encabezado != "Mes/Año")
                        {
                            // Encontraemos el índice del primer paréntesis abierto "(" y cerrado ")"
                            int indiceInicio = encabezado.IndexOf('(');
                            int indiceFin = encabezado.IndexOf(')');

                            // Extraer el texto entre los paréntesis que es la Zona 
                            string zona1 = encabezado.Substring(indiceInicio + 1, indiceFin - indiceInicio - 1);

                            //Extraemos el año 
                            string año1 = encabezado.Substring(0, 4);

                            // Obtenemos los datos para la columna actual
                            datosColumna = ObtenerSumaMes(encabezado, año1, zona1);
                            rowIndex = 4;

                            // Iterar sobre cada fila de datos
                            foreach (DataRow row in datosColumna.Rows)
                            {
                                var valorCelda = row[encabezado];
                                worksheet.Cells[rowIndex, colIndex].Value = valorCelda.ToString();

                                var celda = worksheet.Cells[rowIndex, colIndex];

                                celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                // Aplicamos bordes a la celdas
                                celda.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                celda.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                celda.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                celda.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                                suma += Convert.ToDouble(valorCelda.ToString());

                                if (contador == 12)
                                {
                                    worksheet.Cells[16, colIndex].Value = suma.ToString("N0");
                                    var celdaStyle = worksheet.Cells[16, colIndex];


                                    // Establecemos el texto en negrita
                                    celdaStyle.Style.Font.Bold = true;
                                    celdaStyle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                                    // Aplicamos bordes a la celda de encabezado
                                    celdaStyle.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    celdaStyle.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    celdaStyle.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    celdaStyle.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                                    contador = 0;
                                    suma = 0;
                                }


                                contador++;
                                rowIndex++;
                            }

                            // Incrementar el índice de la columna para pasar a la siguiente columna
                            colIndex++;


                        }

                    }



                    // Guardamos el archivo de Excel
                    string filePath = Path.GetTempFileName() + ".xlsx";
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);

                    // Descargamos el archivo de Excel
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=EstadisticasVenta.xlsx");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
            catch
            {
                tbMensaje.Text = "Ocurrió un error al intentar descargar el excel";
            }
        }


        // TAP X TRIMESTRE

        // Obtiene estadisticas del trimestre y genera grafica 
        protected void btnConsultaTrimestre_Click(object sender, EventArgs e)
        {
            // Cambiamos el año en el titulo 
            SpanAnioTrimestre.InnerText = ddlAnioBusquedaT.SelectedItem.Text;

            int anio = Convert.ToInt32(ddlAnioBusquedaT.SelectedItem.Text);
            string zona = ddlZonaT.SelectedItem.Text;

            DataTable Datos = ObtenerTotalesTrimestrales(anio, zona);


            // Creamos una variable para almacenar la suma total
            decimal totalSum = 0;

            foreach (DataRow row in Datos.Rows)
            {
                // Obtenemos el valor de la columna "Total" de la fila actual
                if (decimal.TryParse(row["TotalTrimestre"].ToString(), out decimal total))
                {
                    // Sumar al total
                    totalSum += total;
                }
            }

            // Creamos una nueva fila para el total
            DataRow totalRow = Datos.NewRow();
            totalRow["FechaIni"] = "Total";
            totalRow["TotalTrimestre"] = totalSum;

            Datos.Rows.Add(totalRow);



            List<string> Trimestre = new List<string>()
                    {
                        "1. Ene-Mar",
                        "2. Abr-Jun",
                        "3. Jul-Sep",
                        "4. Oct-Dic"
                    };

            List<double> cantidades = new List<double>();

            foreach (DataRow row in Datos.Rows)
            {


                // Obtener la cantidad de la fila actual y agregarla a la lista de cantidades
                double cantidad;
                if (double.TryParse(row["TotalTrimestre"].ToString(), out cantidad))
                {
                    cantidades.Add(cantidad);
                }
                else
                {
                    // Manejar el caso en que la cantidad no se pueda convertir a double

                }
            }


            // Generamos la gráfica con los datos obtenidos
            string script = string.Format(@"var nombres = {0}; var cantidades = {1};
                                  GenerarGrafica3(nombres, cantidades);",
                                          new JavaScriptSerializer().Serialize(Trimestre),
                                          new JavaScriptSerializer().Serialize(cantidades));
            ScriptManager.RegisterStartupScript(this, GetType(), "GenerarGrafica3", script, true);


            DataGridTrimestre.DataSource = Datos;
            DataGridTrimestre.DataBind();
        }

        // Obtiene los valores trimetrales 
        public DataTable ObtenerTotalesTrimestrales(int añoConsulta, string zona)
        {

            DataTable resultados = new DataTable();
            resultados.Columns.Add("Trimestre", typeof(int));
            resultados.Columns.Add("FechaIni", typeof(string));
            resultados.Columns.Add("FEchaFin", typeof(string));
            resultados.Columns.Add("TotalTrimestre", typeof(decimal));

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                connection.Open();

                // Iteramos sobre los cuatro trimestres del año
                for (int trimestre = 1; trimestre <= 4; trimestre++)
                {
                    // Calculamos las fechas de inicio y fin del trimestre actual
                    DateTime fechaInicioTrimestre = new DateTime(añoConsulta, ((trimestre - 1) * 3) + 1, 1);
                    DateTime fechaFinTrimestre = fechaInicioTrimestre.AddMonths(3).AddDays(-1);

                    string query = @"SELECT CAST(SUM(Precio_Venta - Precio_Venta * Descuento / 100) AS BIGINT) AS TotalTrimestre
                                 FROM tblTipoPedido
                                 INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                                 WHERE tblOT.Fecha_Confirmacion_Venta BETWEEN @FechaInicioTrimestre AND @FechaFinTrimestre
                                 AND tblTipoPedido.EstadisticaVenta = 1
                                 AND tblOT.Zona LIKE @Zona
                                 AND tblOT.anulada = 0";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@FechaInicioTrimestre", fechaInicioTrimestre);
                        command.Parameters.AddWithValue("@FechaFinTrimestre", fechaFinTrimestre);
                        command.Parameters.AddWithValue("@Zona", zona + "%");

                        object resultado = command.ExecuteScalar();
                        decimal totalTrimestre = resultado != DBNull.Value ? Convert.ToDecimal(resultado) : 0;

                        // Agregamos los datos del trimestre al DataTable
                        resultados.Rows.Add(trimestre, fechaInicioTrimestre.ToString("dd/MM/yyyy"), fechaFinTrimestre.ToString("dd/MM/yyyy"), totalTrimestre.ToString("N0"));
                    }
                }
            }


            return resultados;
        }
        protected void DataGridTrimestre_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                TableCell cell = e.Item.Cells[3];

                // Validacion si es numero o no 
                double valorCelda;
                if (double.TryParse(cell.Text, out valorCelda))
                {
                    string valorFormateado = valorCelda.ToString("#,0");
                    cell.Text = valorFormateado;
                }

                // Aplicamos negrita a la fila 5 que es Total y el resultado
                if (e.Item.ItemIndex == 4)
                {
                    foreach (TableCell celda in e.Item.Cells)
                    {
                        celda.Font.Bold = true;
                    }
                }
            }
        }
        protected void ExportarExcel3_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos un paquete de Excel 
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregamos la hoja  1
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Estadisticas Venta ");

                    worksheet.Row(1).Height = 60;
                    worksheet.Column(2).Width = 40;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(3).Width = 20;
                    worksheet.Column(4).Width = 20;

                    // Logo Ducon       // validar la ruta de este logo y no se debe eliminar 
                    string rutaImagen = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                    FileInfo image = new FileInfo(rutaImagen);
                    if (image.Exists)
                    {
                        var picture = worksheet.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    var CellC1E1 = worksheet.Cells["C1:E1"];
                    CellC1E1.Merge = true;
                    CellC1E1[1, 3].Value = "Estadistica de Ventas";
                    CellC1E1.Style.Font.Name = "Century Gothic";
                    CellC1E1.Style.Font.Size = 16;
                    CellC1E1.Style.Font.Bold = true;
                    CellC1E1.Style.Font.Italic = true;
                    CellC1E1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellC1E1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellC1E1.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    var CellB2E2 = worksheet.Cells["B2:E2"];
                    CellB2E2.Merge = true;
                    CellB2E2[2, 2].Value = "Comparativo Trimestre del Año: " + ddlAnioBusquedaT.SelectedItem.Text;
                    CellB2E2.Style.Font.Name = "Century Gothic";
                    CellB2E2.Style.Font.Size = 14;
                    CellB2E2.Style.Font.Bold = true;
                    CellB2E2.Style.Font.Italic = true;
                    CellB2E2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellB2E2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellB2E2.Style.Font.Color.SetColor(System.Drawing.Color.Black);

                    int headerIndex = 2;
                    int rowIndex = 4;

                    // Encabezados de la tabla 
                    string[] encabezados = { "Trimestre", "Desde", "Hasta", "Total Trimestres" };

                    foreach (string encabezado in encabezados)
                    {
                        worksheet.Cells[rowIndex, headerIndex].Value = encabezado;
                        var headerCell = worksheet.Cells[rowIndex, headerIndex];

                        // Establecemos el texto en negrita
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Font.Name = "Century Gothic";

                        // Aplicamos bordes a la celda de encabezado
                        headerCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        headerIndex++;
                    }


                    // Escribir los datos de la tabla
                    rowIndex = 5;
                    foreach (DataGridItem item in DataGridTrimestre.Items)
                    {
                        int colIndex = 2;
                        foreach (TableCell cell in item.Cells)
                        {

                            if (rowIndex == 9)
                            {

                                worksheet.Cells[rowIndex, 2].Value = "Total";
                                worksheet.Cells[rowIndex, 5].Value = cell.Text;

                                worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                worksheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                            }
                            else
                            {
                                // Para las demás columnas, simplemente escribir el texto
                                worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                                worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }



                            colIndex++;
                        }
                        rowIndex++;
                    }


                    // Guardamos el archivo de Excel
                    string filePath = Path.GetTempFileName() + ".xlsx";
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);

                    // Descargamos el archivo de Excel
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=EstadisticasVenta.xlsx");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
            catch
            {
                tbMensaje.Text = "Ocurrió un error al intentar descargar el excel";
            }
        }


        // TAP X RANGOS 





        // TAP X CUOTA MENSUAL

        //DataGrid Superior 
        protected void DataGridCoutaMes_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Numerador para asesores
                TableCell Numero = e.Item.Cells[1];
                Numero.Text = posCounter.ToString();
                posCounter++;


                // Se le da formato a la columna Couta 
                TableCell CellCouta = e.Item.Cells[6];
                double couta;
                if (double.TryParse(CellCouta.Text, out couta))
                {
                    string valorFormateado = couta.ToString("#,0");
                    CellCouta.Text = valorFormateado;
                }

            }
        }
        protected void DataGridCoutaMes_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "VerAsesor")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridCoutaMes.Items[rowIndex];

                string cedula = row.Cells[2].Text;
                string nombre = row.Cells[3].Text;

                foreach (DataGridItem item in DataGridCoutaMes.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  
                e.Item.CssClass = "fila-seleccionada";

                tbCedAsignar.Text = cedula;
                tbNombreAsesor.Text = nombre;



            }
        }
        protected void btnConsultarCuoM_Click(object sender, EventArgs e)
        {
            CargarDatGridMes();
        }

        private void CargarDatGridMes()
        {
            bool activo = chkActivos.Checked;

            DataTable FullAsesores = ConsultarAsesoresActivos(activo);

            DataGridCoutaMes.DataSource = FullAsesores;
            DataGridCoutaMes.DataBind();
        }
        private DataTable ConsultarAsesoresActivos(bool activo)
        {

            DataTable dtFulAsesroes = new DataTable();



            string sSqlAsesores = @"SELECT
                            tblAsesorComercial.Cedula,[tblAsesorComercial].[Nombre]+' '+[tblAsesorComercial].[Apellidos] AS Asesor,
                            tblEmpleado.Grupo FROM tblEmpleado 
                            INNER JOIN tblAsesorComercial ON tblEmpleado.Cedula = tblAsesorComercial.Cedula
                            WHERE (((tblAsesorComercial.Activo) = @Activo)) ORDER BY tblEmpleado.Grupo";



            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlAsesores, connection))
                {
                    // Agregar el parámetro @Activo al comando SQL
                    command.Parameters.AddWithValue("@Activo", activo);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dtFulAsesroes);
                    }
                }
            }

            // Agregar una nueva columna para la cuota, couta trimestral y total 
            dtFulAsesroes.Columns.Add("Año", typeof(string));
            dtFulAsesroes.Columns.Add("Cuota", typeof(decimal));

            string año = ddlanioBusquedaM.SelectedItem.Text;

            // Obtener la cuota para cada asesor 
            foreach (DataRow row in dtFulAsesroes.Rows)
            {
                string cedula = row["Cedula"].ToString();

                string anio = ConsultarCuotaAñoMes(cedula, año);

                decimal cuota = ConsultarCuotaAsesorMes(cedula, año);

                row["Año"] = anio;
                row["Cuota"] = cuota;
            }




            return dtFulAsesroes;
        }
        private decimal ConsultarCuotaAsesorMes(string cedula, string año)
        {
            string sSqlCuota = @"SELECT
                                tblCuotaAsesor.Cuota
                                From tblCuotaAsesor 
                                WHERE (((tblCuotaAsesor.Año)= @Año) 
                                AND ((tblCuotaAsesor.ID_Asesor)= @Cedula))";

            decimal cuota = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@Cedula", cedula);
                    command.Parameters.AddWithValue("@Año", año);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cuota = Convert.ToDecimal(result);
                    }
                }
            }

            return cuota;
        }
        private string ConsultarCuotaAñoMes(string cedula, string año)
        {
            string sSqlCuota = @"SELECT
                        tblCuotaAsesor.Año
                        FROM tblCuotaAsesor 
                        WHERE (((tblCuotaAsesor.Año)= @Año) 
                        AND ((tblCuotaAsesor.ID_Asesor)= @Cedula))";

            string anio = ""; // Valor predeterminado

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@Cedula", cedula);
                    command.Parameters.AddWithValue("@Año", año);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        anio = result.ToString(); // Convertir a string
                    }
                }
            }

            return anio;
        }




        // DataGrid Inferioor
        protected void DataGridGeneralMes_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            // Encabezados 
            if (e.Item.ItemType == ListItemType.Header)
            {
                // Titulo de encabezado columna 9 T. Ventas + Año 
                DataGridItem headerItem10 = e.Item;
                headerItem10.Cells[10].Text = "T. Ventas " + ddlanioBusquedaM.SelectedItem.Text;

                // Titulo de encabezado columna 13 T. Vent Año anterior 
                DataGridItem headerItem13 = e.Item;
                int año = Convert.ToInt32(ddlanioBusquedaM.SelectedItem.Text);
                headerItem13.Cells[13].Text = "T. Ventas " + (año - 1);

                // Titulo de encabezado columna 14 Ventas 1 Año anterior 
                DataGridItem headerItem14 = e.Item;
                headerItem14.Cells[14].Text = "Ventas " + ddlMes.SelectedItem.Text + " " + (año - 1);

                // Titulo de encabezado columna 15 Ventas Año actal VS año anterior
                DataGridItem headerItem15 = e.Item;
                headerItem15.Cells[15].Text = "Año " + año + " VS " + (año - 1);

                //Titulo de encabezado columna 15 Ventas Año actal VS año anterior
                DataGridItem headerItem16 = e.Item;
                headerItem16.Cells[16].Text = ddlMes.SelectedItem.Text + " " + año + " VS " + ddlMes.SelectedItem.Text + " " + (año - 1);

            }

            // Filas de Datos 
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Numerador para asesores
                TableCell Numero = e.Item.Cells[0];
                Numero.Text = posCounter.ToString();
                posCounter++;

                // Se le da formato a la columna Couta 
                TableCell CellCouta = e.Item.Cells[6];
                double cuota;
                if (double.TryParse(CellCouta.Text, out cuota))
                {
                    string valorFormateado = cuota.ToString("#,0");
                    CellCouta.Text = valorFormateado;
                }

                // Se establece el mes segun el dropdownlist 
                TableCell CellMes = e.Item.Cells[5];
                CellMes.Text = ddlMes.SelectedValue.ToString();
                // Se convierte en un numero operable el  mes 
                Int32 mes;
                if (Int32.TryParse(CellMes.Text, out mes))
                {

                }

                // Se le da formato a la columna Couta 
                TableCell CellTotal = e.Item.Cells[7];
                double total;
                if (double.TryParse(CellTotal.Text, out total))
                {
                    string valorFormateado = total.ToString("#,0");
                    CellTotal.Text = valorFormateado;
                }

                // celda porcentaje de cumnplimiento mes 
                TableCell CellCumplMes = e.Item.Cells[8];

                // Se convierte en un numero operable el presupuesto mes 
                TableCell CellPreMes = e.Item.Cells[9];
                double Presu = 0;


                // Se valida si el total o la cuota son igual a cero 
                if (total == 0 || cuota == 0)
                {
                    CellCumplMes.Text = "0";
                    CellPreMes.Text = "0";
                }
                else
                {
                    CellCumplMes.Text = string.Format("{0:N2}", (total / cuota * 100)) + " %";
                    CellPreMes.Text = (cuota * mes).ToString("#,0");
                    if (double.TryParse(CellPreMes.Text, out Presu))
                    {

                    }

                }


                // Se le da formato a la columna Tventa y se convierta en un numero operable  
                TableCell CellTVentas = e.Item.Cells[10];
                double Tventas;
                if (double.TryParse(CellTVentas.Text, out Tventas))
                {
                    string valorFormateado = Tventas.ToString("#,0");
                    CellTVentas.Text = valorFormateado;
                }


                // celda porcentaje de cumnplimiento año 
                TableCell CellCumplAño = e.Item.Cells[11];

                // Se valida si TVentas o el Presu son igual a cero 
                if (Tventas == 0 || Presu == 0)
                {
                    CellCumplAño.Text = "0";
                }
                else
                {
                    CellCumplAño.Text = string.Format("{0:N2}", (Tventas / Presu * 100)) + " %";
                }

                // celda porcentaje de cumnplimiento año 
                TableCell CellPromedioMensaul = e.Item.Cells[12];

                if (Tventas == 0)
                {
                    CellPromedioMensaul.Text = "0";
                }
                else
                {
                    CellPromedioMensaul.Text = (Tventas * mes).ToString("#,0");
                }


                // Se formatea la celda Ventas año anterior y se convierte en un numero operable
                TableCell CellTVentasAñoAnte = e.Item.Cells[13];
                double TventasAñoAnte;
                if (double.TryParse(CellTVentasAñoAnte.Text, out TventasAñoAnte))
                {
                    string valorFormateado = TventasAñoAnte.ToString("#,0");
                    CellTVentasAñoAnte.Text = valorFormateado;
                }

                // Se formatea la celda Ventas mes y  año anterior y se convierte en un numero operable
                TableCell CellTVentaMesAñosAnterior = e.Item.Cells[14];
                double TventasMesAñoAnte;
                if (double.TryParse(CellTVentaMesAñosAnterior.Text, out TventasMesAñoAnte))
                {
                    string valorFormateado = TventasMesAñoAnte.ToString("#,0");
                    CellTVentaMesAñosAnterior.Text = valorFormateado;
                }

                // celda porcentaje de cumnplimiento año 
                TableCell CellAñoVsAÑoAnt = e.Item.Cells[15];

                if (Tventas == 0 || TventasAñoAnte == 0)
                {
                    CellAñoVsAÑoAnt.Text = "0";
                }
                else
                {
                    CellAñoVsAÑoAnt.Text = string.Format("{0:N2}", (Tventas / TventasAñoAnte * 100)) + " %";
                }


                // celda porcentaje de cumnplimiento año 
                TableCell CellAñoMesVsAñoMesAnt = e.Item.Cells[16];

                if (total == 0 || TventasMesAñoAnte == 0)
                {
                    CellAñoMesVsAñoMesAnt.Text = "0";
                }
                else
                {
                    CellAñoMesVsAñoMesAnt.Text = string.Format("{0:N2}", (total / TventasMesAñoAnte * 100)) + " %";
                }


            }
        }
        protected void btnConsultarM2_Click(object sender, EventArgs e)
        {
            bool activo = chkActivos.Checked;

            DataTable FullAsesores1 = ConsultarAsesoresActivos1(activo);

            DataGridGeneralMes.DataSource = FullAsesores1;
            DataGridGeneralMes.DataBind();
        }
        private DataTable ConsultarAsesoresActivos1(bool activo)
        {
            DataTable dtFulAsesroes1 = new DataTable();

            string sSqlAsesores = @"SELECT
                                    tblAsesorComercial.Cedula,[tblAsesorComercial].[Nombre]+' '+[tblAsesorComercial].[Apellidos] AS Asesor,
                                    tblEmpleado.Grupo FROM tblEmpleado 
                                    INNER JOIN tblAsesorComercial ON tblEmpleado.Cedula = tblAsesorComercial.Cedula 
                                    Where (((tblAsesorComercial.Activo) = @Activo)) ORDER BY tblEmpleado.Grupo;";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlAsesores, connection))
                {
                    // Agregar el parámetro @Activo al comando SQL
                    command.Parameters.AddWithValue("@Activo", activo);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dtFulAsesroes1);
                    }
                }
            }

            // Agregar una nueva columna para la cuota, couta trimestral y total 
            dtFulAsesroes1.Columns.Add("Año", typeof(string));
            dtFulAsesroes1.Columns.Add("Cuota", typeof(decimal));
            dtFulAsesroes1.Columns.Add("Total", typeof(decimal));
            dtFulAsesroes1.Columns.Add("TVentas", typeof(decimal));
            dtFulAsesroes1.Columns.Add("TVentasAñoAnterior", typeof(decimal));
            dtFulAsesroes1.Columns.Add("TVentasAñoAnteriorMes", typeof(decimal));

            DataTable Año_Couta = new DataTable();

            string año = ddlanioBusquedaM.SelectedItem.Text;
            Int32 añoNumero = Convert.ToInt32(año);
            string mes = ddlMes.SelectedValue.ToString();

            // fechas para consulta total 
            string FechaIni = año + "/" + mes + "/01";
            string FechaFin = ObtenerUltimoDiaMes(FechaIni);


            // fechas para consulta T Ventas
            string FechaIni2 = año + "/01/01";
            DateTime ultimoDiaMes = new DateTime(Convert.ToInt32(año), Convert.ToInt32(mes), DateTime.DaysInMonth(Convert.ToInt32(año), Convert.ToInt32(mes)));
            string fechaFin2 = ultimoDiaMes.ToString("yyyy/MM/dd");



            // fechas para consulta T Ventas año anterior  
            string FechaIni3 = (añoNumero - 1) + "/01/01";
            DateTime ultimoDiaMes3 = new DateTime(Convert.ToInt32(añoNumero - 1), Convert.ToInt32(mes), DateTime.DaysInMonth(Convert.ToInt32(añoNumero - 1), Convert.ToInt32(mes)));
            string FechaFin3 = ultimoDiaMes3.ToString("yyyy/MM/dd");


            // fechas para consulta T Ventas año anterior  ( ASI ESTA EL CODIGO EN EL SID VIEJO VALIDAR SI EL MES SI ES CORRECTO EN LA FECHA INICIAL )
            string FechaIni4 = (añoNumero - 1) + "/01/01";
            DateTime ultimoDiaMes4 = new DateTime(Convert.ToInt32(añoNumero - 1), Convert.ToInt32(mes), DateTime.DaysInMonth(Convert.ToInt32(añoNumero - 1), Convert.ToInt32(mes)));
            string FechaFin4 = ultimoDiaMes3.ToString("yyyy/MM/dd");



            foreach (DataRow row in dtFulAsesroes1.Rows)
            {
                string cedula = row["Cedula"].ToString();

                // Se consulta el año y la cuota 
                Año_Couta = ConsultarAño_Couta(cedula, año);
                if (Año_Couta.Rows.Count > 0)
                {
                    row["Año"] = Año_Couta.Rows[0]["Año"];
                    row["Cuota"] = Año_Couta.Rows[0]["Cuota"];
                }
                else
                {
                    row["Año"] = "";
                    row["Cuota"] = "0";
                }

                // Se Consulta el total ventas por mes  seleccionado  
                decimal total = ConsultarTotalVenta_X_Mes_X_Asesor(cedula, FechaIni, FechaFin);

                if (total > 0)
                {
                    row["Total"] = total;
                }
                else
                {
                    row["Total"] = "0";
                }

                // Se Consulta el total ventas por año  seleccionado
                decimal TVentas = ConsultarTVenta_Año(cedula, FechaIni, FechaFin);

                if (TVentas > 0)
                {
                    row["TVentas"] = TVentas;
                }
                else
                {
                    row["TVentas"] = "0";
                }

                // Se Consulta el total ventas por año anterior  seleccionado
                decimal TVentAñoAnt = ConsultarTVenta_Año_Anterior(cedula, FechaIni3, FechaFin3);

                if (TVentAñoAnt > 0)
                {
                    row["TVentasAñoAnterior"] = TVentAñoAnt;
                }
                else
                {
                    row["TVentasAñoAnterior"] = "0";
                }

                // Se Consulta el total ventas por año anterior  y mes   seleccionado
                decimal TVentAñoAntMes = ConsultarTVenta_Año_AnteriorMes(cedula, FechaIni4, FechaFin4);

                if (TVentAñoAntMes > 0)
                {
                    row["TVentasAñoAnteriorMes"] = TVentAñoAntMes;
                }
                else
                {
                    row["TVentasAñoAnteriorMes"] = "0";
                }

            }

            return dtFulAsesroes1;
        }
        public DataTable ConsultarAño_Couta(string cedula, string año)
        {
            DataTable Año_Couta = new DataTable();


            // Consulta para traer la suma de las ventas de cada mes (todos, Zona 1 ó 2 )
            string query = $"SELECT tblCuotaAsesor.Año,tblCuotaAsesor.Cuota " +
                           $"From tblCuotaAsesor " +
                           $"WHERE (((tblCuotaAsesor.Año)= @año) " +
                           $"AND ((tblCuotaAsesor.ID_Asesor)= @cedula))";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {

                    adapter.SelectCommand.Parameters.AddWithValue("@cedula", cedula);
                    adapter.SelectCommand.Parameters.AddWithValue("@año", año);


                    adapter.Fill(Año_Couta);
                }
            }


            return Año_Couta;
        }
        private decimal ConsultarTotalVenta_X_Mes_X_Asesor(string cedula, string FechaIni, string FechaFin)
        {
            string sSqlCuota = @"SELECT 
                                Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total
                                FROM tblTipoPedido 
                                INNER JOIN (tblOT 
                                INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula)
                                ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                                WHERE (((tblOT.Fecha_Confirmacion_Venta)  Between @fechaIni And @fechaFin) AND ((tblOT.Terminado_Diseño)=1) 
                                AND ((tblEmpleado.Cedula)= @cedula))
                                GROUP BY tblTipoPedido.EstadisticaVenta HAVING (((tblTipoPedido.EstadisticaVenta)=1))";

            decimal total = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@cedula", cedula);
                    command.Parameters.AddWithValue("@fechaIni", FechaIni);
                    command.Parameters.AddWithValue("@fechaFin", FechaFin);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
            }

            return total;
        }
        private decimal ConsultarTVenta_Año(string cedula, string FechaIni, string FechaFin)
        {
            string sSqlCuota = @"SELECT
                                Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total
                                FROM tblTipoPedido INNER JOIN (tblOT 
                                INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula) 
                                ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                                WHERE (((tblOT.Fecha_Confirmacion_Venta)  Between @fechaIni And @fechaFin ) AND ((tblOT.Terminado_Diseño)=1) 
                                AND ((tblEmpleado.Cedula)= @cedula))
                                GROUP BY tblTipoPedido.EstadisticaVenta HAVING (((tblTipoPedido.EstadisticaVenta)=1))";

            decimal TVentas = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@cedula", cedula);
                    command.Parameters.AddWithValue("@fechaIni", FechaIni);
                    command.Parameters.AddWithValue("@fechaFin", FechaFin);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        TVentas = Convert.ToDecimal(result);
                    }
                }
            }

            return TVentas;
        }
        private decimal ConsultarTVenta_Año_Anterior(string cedula, string FechaIni, string FechaFin)
        {
            string sSqlCuota = @"SELECT 
                                Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total
                                FROM tblTipoPedido 
                                INNER JOIN (tblOT 
                                INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula) 
                                ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                                WHERE (((tblOT.Fecha_Confirmacion_Venta)  Between @fechaIni And @fechaFin) 
                                AND ((tblOT.Terminado_Diseño)=1) AND ((tblEmpleado.Cedula)= @cedula))
                                GROUP BY tblTipoPedido.EstadisticaVenta HAVING (((tblTipoPedido.EstadisticaVenta)=1))";

            decimal TVentas = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@cedula", cedula);
                    command.Parameters.AddWithValue("@fechaIni", FechaIni);
                    command.Parameters.AddWithValue("@fechaFin", FechaFin);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        TVentas = Convert.ToDecimal(result);
                    }
                }
            }

            return TVentas;
        }
        private decimal ConsultarTVenta_Año_AnteriorMes(string cedula, string FechaIni, string FechaFin)
        {
            string sSqlCuota = @"SELECT 
                                Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total
                                FROM tblTipoPedido 
                                INNER JOIN (tblOT 
                                INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula) 
                                ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                                WHERE (((tblOT.Fecha_Confirmacion_Venta)  Between @fechaIni And @fechaFin) 
                                AND ((tblOT.Terminado_Diseño)=1) AND ((tblEmpleado.Cedula)= @cedula))
                                GROUP BY tblTipoPedido.EstadisticaVenta HAVING (((tblTipoPedido.EstadisticaVenta)=1))";

            decimal TVentas = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@cedula", cedula);
                    command.Parameters.AddWithValue("@fechaIni", FechaIni);
                    command.Parameters.AddWithValue("@fechaFin", FechaFin);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        TVentas = Convert.ToDecimal(result);
                    }
                }
            }

            return TVentas;
        }
        protected void ExportarExcel5_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos un paquete de Excel 
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregamos la hoja  1
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Estadisticas Venta ");

                    worksheet.Row(1).Height = 60;
                    worksheet.Column(2).Width = 40;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(3).Width = 20;
                    worksheet.Column(4).Width = 20;

                    // Logo Ducon       // validar la ruta de este logo y no se debe eliminar 
                    string rutaImagen = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                    FileInfo image = new FileInfo(rutaImagen);
                    if (image.Exists)
                    {
                        var picture = worksheet.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    var CellC1E1 = worksheet.Cells["C1:E1"];
                    CellC1E1.Merge = true;
                    CellC1E1[1, 3].Value = "Estadistica año " + ddlanioBusquedaM.SelectedItem.Text;
                    CellC1E1.Style.Font.Name = "Century Gothic";
                    CellC1E1.Style.Font.Size = 16;
                    CellC1E1.Style.Font.Bold = true;
                    CellC1E1.Style.Font.Italic = true;
                    CellC1E1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellC1E1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellC1E1.Style.Font.Color.SetColor(System.Drawing.Color.Black);


                    // Obtener los encabezados de las columnas del DataGrid
                    int colIndex = 2;
                    int rowIndex = 2;


                    int año = Convert.ToInt32(ddlanioBusquedaM.SelectedItem.Text);
                    string mes = ddlMes.SelectedItem.Text;

                    string tVentaAño = "T Ventas: " + año;
                    string tVentaAñoAnterior = "T Ventas: " + (año - 1);
                    string tVentaAñoMes = "T Ventas: " + mes + " " + año;
                    string tVentaVsAño = "Año " + año + "  VS " + (año - 1);
                    string tVentaVsAñoMes = mes + " " + año + "  VS " + mes + " " + (año - 1);

                    // Encabezados de la tabla 
                    string[] encabezados = { "N°", "Cédula", "Asesor", "Grupo","Año", "Mes", "P. Mensual", "Venta Mes", "% Cump Mes", "Presupuesto X Mes",tVentaAño,"% Cump Anual",
                                            "Promedio Mensual",tVentaAñoAnterior,tVentaAñoMes, tVentaVsAño, tVentaVsAñoMes};

                    foreach (string encabezado in encabezados)
                    {
                        worksheet.Cells[rowIndex, colIndex].Value = encabezado;
                        var headerCell = worksheet.Cells[rowIndex, colIndex];

                        // Establecemos el texto en negrita
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Font.Name = "Century Gothic";

                        // Aplicamos bordes a la celda de encabezado
                        headerCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        colIndex++;
                    }

                    // Obtener los datos de las filas del DataGrid
                    rowIndex = 3;

                    foreach (DataGridItem row in DataGridGeneralMes.Items)
                    {
                        colIndex = 2;
                        foreach (TableCell cell in row.Cells)
                        {

                            string cellText = cell.Text;

                            // Verificar si el texto de la celda es nulo o "&nbsp;"
                            if (string.IsNullOrEmpty(cellText) || cellText.Trim() == "&nbsp;" || cellText == "")
                            {
                                cellText = ""; // Asignar una cadena vacía en lugar de null o "&nbsp;"
                            }


                            worksheet.Cells[rowIndex, colIndex].Value = cellText;
                            var Dato = worksheet.Cells[rowIndex, colIndex];


                            Dato.Style.Font.Size = 11;
                            Dato.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            Dato.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            Dato.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            Dato.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            Dato.Style.Border.Right.Style = ExcelBorderStyle.Thin;


                            colIndex++;
                        }
                        rowIndex++;
                    }



                    // Guardamos el archivo de Excel
                    string filePath = Path.GetTempFileName() + ".xlsx";
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);

                    // Descargamos el archivo de Excel
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=EstadisticasVenta.xlsx");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
            catch
            {
                tbMensaje.Text = "Ocurrió un error al intentar descargar el excel";
            }
        }


        // Asignar Cuota Venta 
        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            if (tbCedAsignar.Text == "")
            {
                string scriptNoSeleccionado = "alert('Por favor, seleccione un asesor para asignar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowNosleccionado", scriptNoSeleccionado, true);
            }
            else
            {

                if (tbCouta.Text == "")
                {
                    string scriptNoAgregado = "alert('No se ha ingresado ningún valor de cuota.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowNoAgregado", scriptNoAgregado, true);

                }
                else
                {
                    SpanCuota.InnerText = tbCouta.Text;
                    SpanNombre.InnerText = tbNombreAsesor.Text;

                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#asignarCuotaMoodal').modal('show');", true);
                }

            }

        }
        protected void btnAsignar_Si_Click(object sender, EventArgs e)
        {
            DataTable Cuota = ConsultarCoutaAseor();

            if (Cuota.Rows.Count > 0)
            {
                string mensajeError = "No se puede agregar cuota al vendedor:  " + tbNombreAsesor.Text + " porque ya tiene una asignada.";
                string scriptError = "alert('" + mensajeError + "');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);
                return;
            }
            else
            {
                // Agregar Couta a un asesor.
                string cedula = tbCedAsignar.Text;
                string año = ddlanioBusquedaM.SelectedItem.Text;
                int cuota = Convert.ToInt32(tbCouta.Text);
                InsertarCoutaAsesor(cedula, año, cuota);

            }
        }
        private DataTable ConsultarCoutaAseor()
        {
            string sSqlCUOTAAsesor = @"SELECT Cuota FROM tblCuotaAsesor WHERE ID_asesor= @cedula AND Año = @año";

            DataTable dtCouta = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sSqlCUOTAAsesor, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@cedula", tbCedAsignar.Text);
                    adapter.SelectCommand.Parameters.AddWithValue("@año", ddlanioBusquedaM.SelectedItem.Text);
                    adapter.Fill(dtCouta);
                }
            }

            return dtCouta;
        }
        private void InsertarCoutaAsesor(string cedula, string año, int cuota)
        {

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblCuotaAsesor (ID_Asesor,Año,Cuota) " +
                              "VALUES (@cedula, @año,@couta) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@cedula", cedula);
                    cmd.Parameters.AddWithValue("@año", año);
                    cmd.Parameters.AddWithValue("@couta", cuota);

                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if (CantidadFilasAfectada > 0)
                    {
                        //Se refresca el datagrid
                        CargarDatGridMes();

                        // limpiamos los campos de 
                        tbCouta.Text = "";
                        tbCedAsignar.Text = "";
                        tbNombreAsesor.Text = "";


                        string scriptExito = "alert('La cuota ha sido asiganada exitosamente.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                    }



                }

            }
        }



        // TAP X CUOTA TRIMESTRAL 
        protected void DataGridCouTri_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Numerador para asesores
                TableCell Numero = e.Item.Cells[0];
                Numero.Text = posCounter.ToString();
                posCounter++;

                // Se agrega el trimestre seleccionado 
                TableCell Trimestre = e.Item.Cells[3];
                Trimestre.Text = ddlTrimestre.SelectedItem.Text;


                // Se le da formato a la columna Couta 
                TableCell CellCouta = e.Item.Cells[4];
                double couta;
                if (double.TryParse(CellCouta.Text, out couta))
                {
                    string valorFormateado = couta.ToString("#,0");
                    CellCouta.Text = valorFormateado;
                }

                // Se le da formato a la columna Couta Trimestral
                TableCell CellcoutaTri = e.Item.Cells[5];
                double coutaTri;
                if (double.TryParse(CellcoutaTri.Text, out coutaTri))
                {
                    string valorFormateado = coutaTri.ToString("#,0");
                    CellcoutaTri.Text = valorFormateado;
                }

                // Se le da formato a la columna Total 
                TableCell CellTotal = e.Item.Cells[6];
                double total;
                if (double.TryParse(CellTotal.Text, out total))
                {
                    string valorFormateado = total.ToString("#,0");
                    CellTotal.Text = valorFormateado;
                }

                TableCell cumplimineto = e.Item.Cells[7];




                if (coutaTri == 0 || total == 0)
                {
                    e.Item.Cells[7].Text = "0";
                }
                else
                {

                    e.Item.Cells[7].Text = (total / coutaTri * 100).ToString("N2") + " %";
                }


            }

        }
        protected void btnConsultarCouTri_Click(object sender, EventArgs e)
        {

            if (ddlTrimestre.SelectedValue != "")
            {
                string Trimestre = ddlTrimestre.SelectedItem.Text;
                string fechaIniString = "";
                string FechaFinstring = "";

                switch (Trimestre)
                {

                    case "1":
                        fechaIniString = ddlAnioBusquedaCouTri.SelectedItem.Text + "/01/01";
                        FechaFinstring = ddlAnioBusquedaCouTri.SelectedItem.Text + "/03/31";
                        break;

                    case "2":
                        fechaIniString = ddlAnioBusquedaCouTri.SelectedItem.Text + "/04/01";
                        FechaFinstring = ddlAnioBusquedaCouTri.SelectedItem.Text + "/06/30";
                        break;
                    case "3":
                        fechaIniString = ddlAnioBusquedaCouTri.SelectedItem.Text + "/07/01";
                        FechaFinstring = ddlAnioBusquedaCouTri.SelectedItem.Text + "/09/30";
                        break;
                    case "4":
                        fechaIniString = ddlAnioBusquedaCouTri.SelectedItem.Text + "/10/01";
                        FechaFinstring = ddlAnioBusquedaCouTri.SelectedItem.Text + "/12/31";
                        break;
                }

                DataTable FullAsesores = ConsultarAsesoresActivos(fechaIniString, FechaFinstring);

                DataGridCouTri.DataSource = FullAsesores;
                DataGridCouTri.DataBind();

            }
            else
            {
                string scriptNoSelect = "alert('Por favor, seleccione un trimestre.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showPermisoBolsa", scriptNoSelect, true);
            }

        }
        private DataTable ConsultarAsesoresActivos(string FechaIni, string FechaFin)
        {
            string sSqlAsesores = @"SELECT tblAsesorComercial.Cedula,[tblAsesorComercial].[Nombre]+' '+[tblAsesorComercial].[Apellidos] AS Asesor,
                                    tblEmpleado.Grupo FROM tblEmpleado INNER JOIN tblAsesorComercial ON tblEmpleado.Cedula = tblAsesorComercial.Cedula 
                                    Where (((tblAsesorComercial.Activo) = 1))  ORDER BY tblEmpleado.Grupo;";

            DataTable dtFulAsesroes = new DataTable();

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(sSqlAsesores, connection))
                {

                    adapter.Fill(dtFulAsesroes);
                }
            }


            // Agregar una nueva columna para la cuota, couta trimestral y total 
            dtFulAsesroes.Columns.Add("Cuota", typeof(decimal));
            dtFulAsesroes.Columns.Add("CuotaTrimestral", typeof(decimal));
            dtFulAsesroes.Columns.Add("Total", typeof(decimal));

            // Obtener la cuota para cada asesor 
            foreach (DataRow row in dtFulAsesroes.Rows)
            {
                string cedula = row["Cedula"].ToString();

                decimal cuota = ConsultarCuotaAsesor(cedula);
                decimal total = ConsultarTotal(FechaIni, FechaFin, cedula);

                // Si la cuota es 0, cadena vacía o nulo, asignar 0 a las columnas
                if (cuota == 0 || string.IsNullOrEmpty(cuota.ToString()))
                {
                    row["Cuota"] = 0;
                    row["CuotaTrimestral"] = 0;
                }
                else
                {
                    row["Cuota"] = cuota;
                    row["CuotaTrimestral"] = cuota * 3;
                }

                row["Total"] = total;

            }

            return dtFulAsesroes;
        }
        private decimal ConsultarCuotaAsesor(string cedula)
        {
            string sSqlCuota = @"SELECT  tblCuotaAsesor.Cuota 
                         FROM tblCuotaAsesor 
                         WHERE tblCuotaAsesor.Año = @Anio 
                         AND  tblCuotaAsesor.ID_Asesor = @Cedula";

            decimal cuota = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@Cedula", cedula);
                    command.Parameters.AddWithValue("@Anio", ddlAnioBusquedaCouTri.SelectedItem.Text);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cuota = Convert.ToDecimal(result);
                    }
                }
            }

            return cuota;
        }
        private decimal ConsultarTotal(string FechaIni, string FechaFin, string cedula)
        {
            string sSqlCuota = @"SELECT Sum(Precio_Venta-Precio_Venta*Descuento/100) AS Total FROM tblTipoPedido 
                                INNER JOIN (tblOT 
                                INNER JOIN tblEmpleado ON tblOT.Codigo_Asesor = tblEmpleado.Cedula)
                                ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                                WHERE (((tblOT.Fecha_Confirmacion_Venta)  Between @FechaIni And @FechaFin) AND ((tblOT.Terminado_Diseño)=1) 
                                AND ((tblEmpleado.Cedula)=@Cedula))
                                GROUP BY tblTipoPedido.EstadisticaVenta
                                HAVING (((tblTipoPedido.EstadisticaVenta)=1))
                                ";

            decimal total = 0;

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionStringSID))
            {
                using (SqlCommand command = new SqlCommand(sSqlCuota, connection))
                {
                    command.Parameters.AddWithValue("@Cedula", cedula);
                    command.Parameters.AddWithValue("@FechaIni", FechaIni);
                    command.Parameters.AddWithValue("@FechaFin", FechaFin);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
            }

            return total;
        }
        protected void ExportarExcel6_Click(object sender, EventArgs e)
        {
            try
            {
                // Creamos un paquete de Excel 
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Agregamos la hoja  1
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Estadisticas Venta ");

                    worksheet.Row(1).Height = 60;
                    worksheet.Column(2).Width = 40;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(3).Width = 20;
                    worksheet.Column(4).Width = 20;

                    // Logo Ducon       // validar la ruta de este logo y no se debe eliminar 
                    string rutaImagen = @"P:\SISTEMAS\Logo Ducon\Ducon.jpg";
                    FileInfo image = new FileInfo(rutaImagen);
                    if (image.Exists)
                    {
                        var picture = worksheet.Drawings.AddPicture("Logo", image);
                        picture.SetPosition(0, 10, 1, 50);
                        picture.SetSize(150, 60);

                    }

                    var CellC1E1 = worksheet.Cells["C1:E1"];
                    CellC1E1.Merge = true;
                    CellC1E1[1, 3].Value = "Estadistica de Ventas Couta Trimestral";
                    CellC1E1.Style.Font.Name = "Century Gothic";
                    CellC1E1.Style.Font.Size = 16;
                    CellC1E1.Style.Font.Bold = true;
                    CellC1E1.Style.Font.Italic = true;
                    CellC1E1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    CellC1E1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    CellC1E1.Style.Font.Color.SetColor(System.Drawing.Color.Black);


                    int headerIndex = 2;
                    int rowIndex = 3;

                    // Encabezados de la tabla 
                    string[] encabezados = { "N°", "Cedula", "Asesor", "Trimestre", "Couta", "Couta Trimestral", "Total", "% Cumplimiento" };

                    foreach (string encabezado in encabezados)
                    {
                        worksheet.Cells[rowIndex, headerIndex].Value = encabezado;
                        var headerCell = worksheet.Cells[rowIndex, headerIndex];

                        // Establecemos el texto en negrita
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Font.Name = "Century Gothic";

                        // Aplicamos bordes a la celda de encabezado
                        headerCell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        headerIndex++;
                    }


                    // Escribir los datos de la tabla
                    rowIndex = 4;
                    foreach (DataGridItem item in DataGridCouTri.Items)
                    {
                        int colIndex = 2;
                        foreach (TableCell cell in item.Cells)
                        {
                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                            worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            colIndex++;
                        }
                        rowIndex++;
                    }


                    // Guardamos el archivo de Excel
                    string filePath = Path.GetTempFileName() + ".xlsx";
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);

                    // Descargamos el archivo de Excel
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=EstadisticasVenta.xlsx");
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
            catch
            {
                tbMensaje.Text = "Ocurrió un error al intentar descargar el excel";
            }
        }


    }
}