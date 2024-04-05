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

                    //query para consolidado
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

                // query para consolidado


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

                    //query para consolidado
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



                // query para consolidado


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



        // TAP X MESES 

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
            if(zona == "%")
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


                    string[] Meses = { "Enero","Febrero","Marzo","Abril","Mayo","Junio", "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre","Total" };

                    int fila = 4;
                    int col = 2;
                    foreach(string Mes in Meses)
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
                        if(encabezado != "Mes/Año")
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

                                if(contador == 12)
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

    }
}