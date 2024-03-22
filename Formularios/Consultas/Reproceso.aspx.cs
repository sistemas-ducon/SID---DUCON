using DocumentFormat.OpenXml.Drawing.Charts;
using SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataTable = System.Data.DataTable;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;
using TextBox = System.Web.UI.WebControls.TextBox;
using NPOI.SS.Formula.Functions;
using System.Data;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.Consultas
{
    public partial class Reproceso : System.Web.UI.Page
    {
        private string CadenaConexionSID = "BD_SIDSQL";
        private string CadenaConexionISID = "BD_ISIDSQL";
        private string CadenaConexionSSF = "BD_SSF";
        protected int contador = 1;
        private List<TextBox> listaTextBoxes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["usuariologueado"] != null)
                {
                    if (ValidarPermiso())
                    {
                        ddlArea.DataSourceID = "ConPermiso";
                    }
                    else
                    {
                        // Obtener el valor de la variable de sesión del usuario logueado
                        string cedula = Session["CedulaLogeada"] as string;

                        // Asignar el valor de la variable de sesión al parámetro del SqlDataSource
                        SinPermiso.SelectParameters["Cedula"].DefaultValue = "%" + cedula + "%";
                        ddlArea.DataSourceID = "SinPermiso";

                    }

                    ddlArea.DataBind();

                    // Obtener la fecha actual
                    DateTime today = DateTime.Today;

                    // Calcular el primer día del mes actual
                    DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

                    // Establecer las fechas en los TextBoxes
                    fechaIni.Text = firstDayOfMonth.ToString("yyyy-MM-dd");
                    fechaFin.Text = today.ToString("yyyy-MM-dd");

                    ddlEstado.Items.Add("Por Asignar");

                    BotonesIniciales();
                    DisposicionInicial();
                    LlenarAño();

                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }


            }
        }


        // Metodos Iniciales y PageLoad  para tap1 y tap2 
        protected bool ValidarPermiso()
        {
            // Obtener la cédula del usuario logueado de la variable de sesión
            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();

            // Realizar la consulta para verificar los permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = @CedulaLogueada AND ID_Permiso = '42'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer parámetro para evitar SQL Injection
                    command.Parameters.AddWithValue("@CedulaLogueada", cedulaLogueada);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        return false;

                    }
                }
            }
        }

        protected void BotonesIniciales()
        {
            CerrarReproceso.Enabled = true;
            CerrarReproceso.CssClass = "btn btn-sm shadow button-enabled";

            Agregar.Enabled = true;
            Agregar.CssClass = "btn btn-sm shadow button-enabled";

            EliminarElemento1.Enabled = true;
            EliminarElemento1.CssClass = "btn btn-sm shadow button-enabled";

            Guardar.Enabled = true;
            Guardar.CssClass = "btn btn-sm shadow button-enabled";

            Adjuntar.Enabled = false;
            Adjuntar.CssClass = "btn btn-sm shadow button-disabled";

            Redireccionar.Enabled = false;
            Redireccionar.CssClass = "btn btn-sm shadow button-disabled";


        }

        protected void DisposicionInicial()
        {
            // deshabilitamos  los textbox  dropdownlist  texArea y chekBox
            listaTextBoxes = new List<TextBox> { tbOT, tbPedido, tbObra, tbCant, tbPq1, tbPq2, tbPq3, tbPq4, tbPq5, tbIdCausa };

            foreach (TextBox textBox in listaTextBoxes)
            {
                textBox.Enabled = false;
                textBox.CssClass = "form-control";
            }

            ddlResponsable.Enabled = false;
            ddlResponsable.CssClass = "form-control";

            ddlCausaRaiz.Enabled = false;
            ddlCausaRaiz.CssClass = "form-control";

            txObs.Disabled = true;
            txCorreccion.Disabled = true;

            chkAcepta.Enabled = false;
            chkcerrado.Enabled = false;
            chkPlanAccion.Enabled = false;

        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEstado.Items.Clear();
            if (ddlArea.SelectedItem.Text != "")
            {
                ddlEstado.Items.Add("Aceptado");
                ddlEstado.Items.Add("No Aceptado");
                ddlEstado.Items.Add("Pendiente");
                ddlEstado.Items.Add("Pendiente por precio");
                ddlEstado.Items.Add("Todos");
            }
            else
            {
                ddlEstado.Items.Add("Por Asignar");
            }

            ddlEstado.DataBind();
            DatagridReproceso.DataBind();
        }

        protected void ddlArea_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como ""
            ddlArea.Items.Insert(0, new ListItem("", ""));
        }

        protected void ddlArea1_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como ""
            ddlArea1.Items.Insert(0, new ListItem("", ""));
        }

        protected void ddlElemento_DataBound(object sender, EventArgs e)
        {
            ddlElemento.Items.Insert(0, new ListItem("", ""));
        }

        private void limpiarCampos()
        {
            // limpiamos los datagrids
            DatagridReproceso.DataBind();
            DatagridDetalleReproceso.DataBind();

            // Limpiamos los textbox y dropdownlist 
            listaTextBoxes = new List<TextBox>
                         { tbOT,tbPedido,tbObra,tbCant,tbPq1,tbPq2,tbPq3,tbPq4,tbPq5,tbIdCausa,tbCantidad,tbPrecio   };

            foreach (TextBox textBox in listaTextBoxes)
            {
                textBox.Text = "";
            }

            ddlResponsable.Items.Clear();
            ddlCausaRaiz.Items.Clear();
            ddlElemento.DataBind();
            ddlArea1.DataBind();

            // limpieamos los TextArea 
            txObs.InnerText = "";
            txComentario.InnerText = "";
            txCorreccion.InnerText = "";

            // limpiamos los checkBox 

            chkcerrado.Checked = false;
            chkAcepta.Checked = false;
            chkPlanAccion.Checked = false;

            BotonesIniciales();

        }

        private void limpiarCamposdetalle1()
        {
            // limpiamos los datagrids          
            DatagridDetalleReproceso.DataBind();

            // Limpiamos los textbox y dropdownlist 
            listaTextBoxes = new List<TextBox>
                         { tbPq1,tbPq2,tbPq3,tbPq4,tbPq5,tbIdCausa,tbCantidad,tbPrecio,tbCant   };

            foreach (TextBox textBox in listaTextBoxes)
            {
                textBox.Text = "";
            }

            ddlResponsable.Items.Clear();
            ddlCausaRaiz.Items.Clear();

            ddlElemento.DataBind();
            ddlArea1.DataBind();


            // limpieamos los TextArea 
            txComentario.InnerText = "";
            txCorreccion.InnerText = "";



        }

        private string ConsultarConsulta()
        {
            string sSql = "";
            string estado = ddlEstado.SelectedItem.Text;


            switch (estado)
            {
                case "Aceptado":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN @fechaIni AND @FechaFin) 
                                AND(tblReprocesoDetalle.Acepta = 1)
                            ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;

                case "Pendiente por precio":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReprocesoDetalle.Precio = 0) AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN @fechaIni AND @FechaFin ) ORDER BY tblReporteOT.Fecha_Entrega_Produccion";

                    break;

                case "No Aceptado":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN @fechaIni AND @FechaFin) 
                        AND (tblReprocesoDetalle.Acepta = 0) AND (tblReprocesoDetalle.Cerrado = 0) 
                    ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;

                case "Todos":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN  @fechaIni AND @FechaFin) 
                    ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;

                case "Pendiente":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN  @fechaIni AND @FechaFin) 
                        AND (tblReprocesoDetalle.Acepta IS NULL) 
                    ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;
               
            }

            return sSql;
        }

        private void CargarDataGrid()
        {
            // Asigna los valores a los parámetros del SqlDataSource
            Reprocesos.SelectParameters["Descripcion"].DefaultValue = ddlArea.SelectedItem.Value;
            Reprocesos.SelectParameters["fechaIni"].DefaultValue = fechaIni.Text;
            Reprocesos.SelectParameters["FechaFin"].DefaultValue = fechaFin.Text;

            string sqlNuevo = ConsultarConsulta();

            Reprocesos.SelectCommand = sqlNuevo;

            // Enlaza el SqlDataSource al DataGrid
            DatagridReproceso.DataSource = Reprocesos;
            DatagridReproceso.DataBind();
            DatagridDetalleReproceso.DataBind();
        }


        // TAP REPROCESOS CALIDAD 


        // Convenciones 
        protected void chkConvenciones_CheckedChanged(object sender, EventArgs e)
        {

            if (chkConvenciones.Checked)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#convenciones').modal('show');", true);

            }

        }
        protected void moldaCerrar_Click(object sender, EventArgs e)
        {
            chkConvenciones.Checked = false;
        }


        // click en consultar  carga (reprosod segun el filtro )
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            string estado = ddlEstado.SelectedItem.Value;
            string sSql = "";

            switch (estado)
            {
                case "Aceptado":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN @fechaIni AND @FechaFin) 
                                AND(tblReprocesoDetalle.Acepta = 1)
                            ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;

                case "Pendiente por precio":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReprocesoDetalle.Precio = 0) AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN @fechaIni AND @FechaFin ) ORDER BY tblReporteOT.Fecha_Entrega_Produccion";

                    break;

                case "No Aceptado":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN @fechaIni AND @FechaFin) 
                        AND (tblReprocesoDetalle.Acepta = 0) AND (tblReprocesoDetalle.Cerrado = 0) 
                    ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;

                case "Todos":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN  @fechaIni AND @FechaFin) 
                    ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;

                case "Pendiente":
                    sSql = @"SELECT tblReporteOT.Id_OT, tblReporteOT.Consecutivo_Pedido, tblReporteOT.Nombre_Obra, tblReporteOT.Observacion_Pedido, tblReporteOT.Fecha_Entrega_Produccion, 
                           tblReprocesoDetalle.Id_Area, tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Precio, tblReprocesoDetalle.Cerrado, tblReprocesoDetalle.Origen, tblReprocesoDetalle.Redirigido, tblAreaReproceso.Descripcion 
                    FROM tblReporteOT 
                    INNER JOIN tblReprocesoDetalle ON tblReporteOT.Id_OT = tblReprocesoDetalle.Ot AND tblReporteOT.Consecutivo_Pedido = tblReprocesoDetalle.Pedido 
                    INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                    WHERE (tblReporteOT.Id_TipoPedido = '8') AND (tblAreaReproceso.Descripcion LIKE @Descripcion) 
                        AND (tblReporteOT.Fecha_Entrega_Produccion BETWEEN  @fechaIni AND @FechaFin) 
                        AND (tblReprocesoDetalle.Acepta IS NULL) 
                    ORDER BY tblReporteOT.Fecha_Entrega_Produccion";
                    break;

                case "Por Asignar":
                    DatagridReproceso.DataSource = PorAsignar;
                    DatagridReproceso.DataBind();
                    break;
            }


            if (estado != "Por Asignar")
            {
                // Ajustar @FechaFin para incluir todas las horas del día
                DateTime fechaFin1 = Convert.ToDateTime(fechaFin.Text).Date.AddDays(1).AddTicks(-1);


                // Asigna los valores a los parámetros del SqlDataSource
                Reprocesos.SelectParameters["Descripcion"].DefaultValue = ddlArea.SelectedItem.Value;
                Reprocesos.SelectParameters["fechaIni"].DefaultValue = fechaIni.Text;
                Reprocesos.SelectParameters["FechaFin"].DefaultValue = fechaFin1.ToString("yyyy-MM-dd HH:mm:ss");
                // Asigna la consulta al SqlDataSource
                Reprocesos.SelectCommand = sSql;

                // Enlaza el SqlDataSource al DataGrid
                DatagridReproceso.DataSource = Reprocesos;


                limpiarCampos();
                DisposicionInicial();
            }

        }
        protected void DatagridReproceso_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                TableCell cell0 = e.Item.Cells[1];
                cell0.Text = contador.ToString();
                // Incrementa el contador para la próxima fila
                contador++;

                // Para  poner si 0 No en la col Completo
                object CerradoObj = DataBinder.Eval(e.Item.DataItem, "Cerrado");
                int completo = (CerradoObj != DBNull.Value) ? Convert.ToInt32(CerradoObj) : 0;

                TableCell cell = e.Item.Cells[7];
                cell.Text = (completo == 1) ? "Si" : "No";

                // Pra Darle color yy formato a la colomna 
                string estado = ddlEstado.SelectedItem.Text;

                if (estado != "Por Asignar")
                {
                    object aceptaObj = DataBinder.Eval(e.Item.DataItem, "Acepta");
                    int Acepta = (aceptaObj != DBNull.Value) ? Convert.ToInt32(aceptaObj) : -1;

                    // Obtiene el valor del precio del DataItem
                    object precioObj = DataBinder.Eval(e.Item.DataItem, "Precio");

                    // Verifica si el precio es nulo o está vacío
                    if (precioObj == null || precioObj == DBNull.Value || string.IsNullOrEmpty(precioObj.ToString()))
                    {
                        // Asigna cero como valor predeterminado para el precio
                        precioObj = -1;
                    }

                    // Convierte el precio a entero
                    int precio = Convert.ToInt32(precioObj);

                    if (precio == 0)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                    }
                    else if (precio == -1)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffffff");    //blanco 
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00000000");
                    }

                    // para validar el color y el texto de Aceptado
                    if (Acepta == 1 || Acepta == 0)
                    {
                        TableCell cell8 = e.Item.Cells[8];
                        cell8.Text = (Acepta == 1) ? "Si" : "No";

                        if (Acepta == 1)
                        {
                            //Color para la celda si el valor  aceptado es true 
                            cell8.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffffff");//blanco ;
                            cell8.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00000000");
                        }
                        else
                        {
                            //Color para la celda si el valor  aceptado es false 
                            cell8.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b");//morado ;
                            cell8.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                        }
                    }
                    else if (Acepta == -1)
                    {
                        //Color y texto  para la celda si el valor  aceptado es null  
                        TableCell cell8 = e.Item.Cells[8];
                        cell8.Text = "Pend";
                        cell8.BackColor = System.Drawing.ColorTranslator.FromHtml("#F1FF43");//amarillo ;
                        cell8.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");

                    }
                }

            }
        }
        protected void DatagridReproceso_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            DataGridItem row = DatagridReproceso.Items[rowIndex];

            string Id_OT = row.Cells[2].Text;
            string pedido = row.Cells[3].Text;
            string Obra = row.Cells[4].Text;
            string Observacion = ReemplazarCaracteresEspeciales(row.Cells[5].Text);

            string Descripcion = ddlArea.SelectedItem.Text;

            foreach (DataGridItem item in DatagridReproceso.Items)
            {
                if (item != row)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            //se usa Para darle un color a la fila seleccionada  
            e.Item.CssClass = "fila-seleccionada";

            tbOT.Text = Id_OT;
            tbPedido.Text = pedido;
            tbObra.Text = Obra;
            txObs.Value = Observacion;
            DetalleReproceso.SelectParameters["Descripcion"].DefaultValue = Descripcion;

            DatagridDetalleReproceso.DataBind();

            Guardar.Enabled = true;
            Guardar.CssClass = "btn btn-sm shadow button-enabled";

            Adjuntar.Enabled = true;
            Adjuntar.CssClass = "btn btn-sm shadow button-enabled";


            if (ObtenerEstadoTerminadoReproceso(Id_OT, pedido))
            {
                CerrarReproceso.Enabled = false;
                CerrarReproceso.CssClass = "btn btn-sm shadow button-disabled";

                ddlArea1.Enabled = false;
                ddlArea1.CssClass = "form-control";

                ddlElemento.Enabled = false;
                ddlElemento.CssClass = "form-control";

                ddlCausaRaiz.Enabled = false;
                ddlCausaRaiz.CssClass = "form-control";

                tbPrecio.Enabled = false;
                tbPrecio.CssClass = "form-control";

                ddlResponsable.Enabled = false;
                ddlResponsable.CssClass = "form-control";

                txComentario.Disabled = true;
                txCorreccion.Disabled = true;

                tbCantidad.Enabled = false;
                tbCantidad.CssClass = "form-control";

                chkAcepta.Enabled = false;


                Guardar.Enabled = false;
                Guardar.CssClass = "btn btn-sm shadow button-disabled";

                Adjuntar.Enabled = false;
                Adjuntar.CssClass = "btn btn-sm shadow button-disabled";

                Agregar.Enabled = false;
                Agregar.CssClass = "btn btn-sm shadow button-disabled";

                EliminarElemento1.Enabled = false;
                EliminarElemento1.CssClass = "btn btn-sm shadow button-disabled";

                DatagridDetalleReproceso.Enabled = false;

            }
            else
            {
                if (ValidarPermisoReproceso())
                {
                    CerrarReproceso.Enabled = true;
                    CerrarReproceso.CssClass = "btn btn-sm shadow button-enabled";



                    ddlArea1.Enabled = true;
                    ddlArea1.CssClass = "form-control";

                    ddlElemento.Enabled = true;
                    ddlElemento.CssClass = "form-control";

                    tbCantidad.Enabled = true;
                    tbCantidad.CssClass = "form-control";

                    tbPrecio.Enabled = true;
                    tbPrecio.CssClass = "form-control";


                    Agregar.Enabled = true;
                    Agregar.CssClass = "btn btn-sm shadow button-enabled";

                    EliminarElemento1.Enabled = true;
                    EliminarElemento1.CssClass = "btn btn-sm shadow button-enabled";

                }
                DatagridDetalleReproceso.Enabled = true;
                chkAcepta.Enabled = true;

            }

            limpiarCamposdetalle1();

        }
        public bool ObtenerEstadoTerminadoReproceso(string idOT, string consecutivoPedido)
        {
            bool terminado = false;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            string query = "SELECT Terminada_Reproceso FROM tblReporteOT WHERE Id_OT = @OT AND Consecutivo_Pedido = @pedido";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OT", idOT);
                    command.Parameters.AddWithValue("@pedido", consecutivoPedido);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    // Verificar si el resultado no es nulo
                    if (result != null)
                    {
                        // Convertir el resultado a booleano
                        terminado = Convert.ToBoolean(result);
                    }
                }
            }

            return terminado;
        }
        protected bool ValidarPermisoReproceso()
        {
            // Obtener la cédula del usuario logueado de la variable de sesión
            string cedulaLogueada = Session["CedulaLogeada"]?.ToString();

            // Realizar la consulta para verificar los permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;
            string query = "SELECT * FROM tblPermiso_Empleado WHERE ID_Empleado = @CedulaLogueada AND ID_Permiso = '42'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Establecer parámetro para evitar SQL Injection
                    command.Parameters.AddWithValue("@CedulaLogueada", cedulaLogueada);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Close();
                        return true;

                    }
                    else
                    {
                        reader.Close();
                        return false;

                    }


                }
            }
        }


        protected void DatagridDetalleReproceso_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            DataGridItem row = DatagridDetalleReproceso.Items[rowIndex];

            foreach (DataGridItem item in DatagridDetalleReproceso.Items)
            {
                if (item != row)
                {
                    item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                }
            }

            //se usa Para darle un color a la fila seleccionada  
            e.Item.CssClass = "fila-seleccionada";


            // traemos los campos del datagrid 
            string Area = row.Cells[1].Text;
            string elememto = row.Cells[2].Text;
            string cantidad = row.Cells[3].Text;
            string precio = row.Cells[4].Text;
            string Causa = row.Cells[5].Text;
            string responsable = row.Cells[6].Text;
            string IdDetalle = row.Cells[9].Text;
            string correccion = ReemplazarCaracteresEspeciales(row.Cells[7].Text).Replace("*nbsp;", "");
            string comentario = ReemplazarCaracteresEspeciales(row.Cells[8].Text).Replace("*nbsp;", "");
            string acepta = row.Cells[10].Text.Replace("&nbsp;", "");
            string pq1 = row.Cells[11].Text.Replace("&nbsp;", "");
            string pq2 = row.Cells[12].Text.Replace("&nbsp;", "");
            string pq3 = row.Cells[13].Text.Replace("&nbsp;", "");
            string pq4 = row.Cells[14].Text.Replace("&nbsp;", "");
            string pq5 = row.Cells[15].Text.Replace("&nbsp;", "");
            string cerrado = row.Cells[18].Text;
            string IdArea = row.Cells[20].Text;
            string idElemento = row.Cells[21].Text;

            // Se cargan Causas y Responsables 
            CargarCausa(Area);
            DataTable causas = CargarCausa2(Area);
            CargarResponsable(Area); ;

            //Cargamos id del detalle y el area del detalle en label ocultos 
            Id_detalle.Text = IdDetalle;
            lbNombAreaRepro.Text = Area;
            lbIdElemnto.Text = idElemento;

            //  Se selecciona causa, rresponsable, elemento y area de consulta 
            if (Area != null && Area != "&nbsp;")
            {
                ddlArea1.SelectedValue = IdArea;
            }
            //  Se selecciona causa, rresponsable, elemento y area de consulta 
            if (elememto != null && elememto != "&nbsp;")
            {
                ddlElemento.SelectedValue = idElemento;
            }
            if (Causa != null && Causa != "&nbsp;")
            {
                ddlCausaRaiz.SelectedValue = Causa;
            }
            if (responsable != null && responsable != "&nbsp;")
            {
                ddlResponsable.SelectedValue = responsable;
            }

            // Pasamos los campos del datagrid a los textBox
            tbCantidad.Text = cantidad;
            tbPrecio.Text = precio;
            txCorreccion.InnerText = correccion;
            txComentario.InnerText = comentario;

            if (causas.Rows.Count > 0)
            {
                foreach (DataRow fila in causas.Rows)
                {
                    string causa = fila["Descripcion"].ToString();

                    if (causa == Causa)
                    {
                        tbIdCausa.Text = fila["paIDPlan"].ToString();
                    }
                }
            }
            //vaidamos si el reproceso esta aceptado o no 
            if (acepta == "Si")
            {
                chkAcepta.Checked = true;
                chkAcepta.Enabled = true;
                chkPlanAccion.Enabled = true;
                ValidarCambioAcepta();
            }
            else
            {
                chkPlanAccion.Enabled = false;
                chkAcepta.Checked = false;
                Redireccionar.Enabled = true;
                ValidarCambioAcepta();
            }

            tbPq1.Text = pq1;
            tbPq2.Text = pq2;
            tbPq3.Text = pq3;
            tbPq4.Text = pq4;
            tbPq5.Text = pq5;

            //Validamos si el reproceso esta cerrado 
            if (cerrado == "Si")
            {
                chkcerrado.Checked = true;
            }
            else
            {
                chkcerrado.Checked = false;
            }


            //Activamos el checkBox de acepta 
            chkAcepta.Enabled = true;


        }

        // Para llenar el dropdownlist de Causa Raiz
        private void CargarCausa(string Area)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT * FROM tblCausasReproceso WHERE activo = 1 AND Area= @area ORDER BY descripcion";

                SqlCommand command = new SqlCommand(consulta, connection);
                command.Parameters.AddWithValue("@area", Area);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                ddlCausaRaiz.DataSource = reader;
                ddlCausaRaiz.DataTextField = "Descripcion"; // Campos que se mostrará en el DropDownLi
                ddlCausaRaiz.DataValueField = "Descripcion";

                ddlCausaRaiz.DataBind();
                ddlCausaRaiz.Items.Insert(0, new ListItem("", ""));
                reader.Close();
            }
        }

        // Necesario para filtrar el Id Causa 
        private DataTable CargarCausa2(string Area)
        {

            DataTable dataTable = new DataTable();

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringISID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblCausasReproceso WHERE activo = 1 AND Area= @area ORDER BY descripcion";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@area", Area);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable que puede contener cero o más filas de resultados
            return dataTable;
        }

        // Para llenar el dropdownlist de Responsable
        private void CargarResponsable(string Area)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string consulta = "SELECT Nombre + ' ' + apellidos as NombreCompleto FROM tblEmpleado WHERE Activo=1 AND Cargo= @area ORDER BY Nombre + ' ' + apellidos ASC ";

                SqlCommand command = new SqlCommand(consulta, connection);
                command.Parameters.AddWithValue("@area", Area);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                ddlResponsable.DataSource = reader;
                ddlResponsable.DataTextField = "NombreCompleto"; // Campos que se mostrará en el DropDownLi
                ddlResponsable.DataValueField = "NombreCompleto";
                ddlResponsable.DataBind();
                ddlResponsable.Items.Insert(0, new ListItem("", ""));

                reader.Close();
            }
        }

        // Depuracion de caracteres especiales de los textArea
        private string ReemplazarCaracteresEspeciales(string texto)
        {
            // Lista de caracteres peligrosos que deseas reemplazar
            string[] caracteresPeligrosos = { "<", ">", "'", "\"", "&" };

            // Reemplaza cada caracter peligroso por un asterisco
            foreach (string caracter in caracteresPeligrosos)
            {
                texto = texto.Replace(caracter, "*");
            }

            return texto;
        }


        // Para Activar los campos si el checkBox esta activado y activar la redireccion y si no esta aceptado 
        protected void chkAcepta_CheckedChanged(object sender, EventArgs e)
        {
            ValidarCambioAcepta();
        }
        private void ValidarCambioAcepta()
        {
            if (ddlElemento.SelectedItem.Value != "")
            {
                if (chkAcepta.Checked)
                {
                    // activamos dropdownlist
                    ddlCausaRaiz.Enabled = true;
                    ddlResponsable.Enabled = true;
                    ddlArea1.Enabled = false;
                    ddlArea1.CssClass = "form-control";

                    //Actiamos los textBox
                    tbPq1.Enabled = true;
                    tbPq2.Enabled = true;
                    tbPq3.Enabled = true;
                    tbPq4.Enabled = true;
                    tbPq5.Enabled = true;
                    tbPrecio.Enabled = true;

                    // Activamos los TextArea
                    txComentario.Disabled = false;
                    txCorreccion.Disabled = false;

                    //Deshabilitamos Redireccionar 
                    Redireccionar.Enabled = false;
                    Redireccionar.CssClass = "btn btn-sm shadow button-disabled";
                }
                else
                {
                    // activamos dropdownlist
                    ddlCausaRaiz.Enabled = false;
                    ddlResponsable.Enabled = false;
                    ddlArea1.Enabled = true;

                    //Actiamos los textBox
                    tbPq1.Enabled = false;
                    tbPq2.Enabled = false;
                    tbPq3.Enabled = false;
                    tbPq4.Enabled = false;
                    tbPq5.Enabled = false;
                    tbPrecio.Enabled = true;

                    // Activamos los TextArea
                    txComentario.Disabled = false;
                    txCorreccion.Disabled = true;

                    //Deshabilitamos Redireccionar 
                    Redireccionar.Enabled = true;
                    Redireccionar.CssClass = "shadow btn btn-sm button-enabled";

                }
            }

        }



        // Notificar Reproceso
        protected void btnNotificar_Click(object sender, EventArgs e)
        {
            if (DatagridReproceso.Items.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#notificarReproMoodal').modal('show');", true);
            }
            else
            {
                string scriptExito = "alert('Por favor, seleccione los reprocesos a notificar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
            }

           
        }

        protected void btnNotRepro_SI_Click(object sender, EventArgs e)
        {
            // Se envía notificación por correo de los reprocesos a los jefes de área
            // Se debe revisar debido a que el procedimiento almacenado no existe  en la BD SID 
            // duc_sp_correo_NotificacionReprocesos
        }


        // Cerrar Rerproceso
        protected void CerrarReproceso_Click(object sender, EventArgs e)
        {
            if (tbOT.Text == "" || tbPedido.Text == "")
            {
                CargarDataGrid();
                string scriptExito = "alert('Por favor, seleccione un reproceso a cerrar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);

            }
            else
            {
                //Mostrar Modal de si desea cerrar el reproceso 
                ScriptManager.RegisterStartupScript(this, GetType(), "actualizarValorOt", "actualizarValorOt();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#cerrarReprocesoModal').modal('show');", true);
            }

        }
        protected void cerrarReproceso1_Click1(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            int filasActualizadas = 0; // Variable para contar las filas actualizadas

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sSql = "UPDATE tblReporteOT SET Terminada_Reproceso = 1 WHERE Id_OT = @OT AND Consecutivo_Pedido = @pedido";
                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    cmd.Parameters.AddWithValue("@OT", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", tbPedido.Text);
                    filasActualizadas = cmd.ExecuteNonQuery();
                    
                    if(filasActualizadas > 0)
                    {
                        //Se refrescan los datagrids
                        CargarDataGrid();
                        string scriptExito = "alert('El detalle de reproceso ha sido cerrado.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showExito", scriptExito, true);
                    }
                }
            }

            // Verificar si se actualizó al menos una fila
            if (filasActualizadas > 0)
            {
                string scriptExito = "alert('El proceso ha sido cerrado exitosamente.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showExito", scriptExito, true);
            }
            else
            {
                string scriptError = "alert('No se pudo cerrar el proceso.intentelo nuevamente.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", scriptError, true);
            }
        }


        //Eliminar elemento de Detalle reproceso  
        protected void EliminarElemento1_Click(object sender, EventArgs e)
        {
            if (Id_detalle.Text == "")
            {
                string scriptExito = "alert('Por favor, seleccione un detalle a eliminar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#eliminarReprocesoModal').modal('show');", true);
            }
        }
        protected void EliminarElemento_Click(object sender, EventArgs e)
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "DELETE  FROM tblReprocesoDetalle  WHERE Id_Detalle = @idDetalle";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@idDetalle", Id_detalle.Text);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if(CantidadFilasAfectada > 0)
                    {
                        //Se refrescan los datagrids
                        CargarDataGrid();
                        string scriptExito = "alert('El elemento ha sido eliminado.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                    }
                    else
                    {
                        string scriptExito = "alert('Ocurrió un error al eliminar el elemento, intentalo nuevamente mas tarde.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                    }
                }

            }
        }



        //Agregar Reproceso 
        protected void Agregar_Click(object sender, EventArgs e)
        {
            if (ValidarAgregar())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#agregarReproMoodal').modal('show');", true);
            }

        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            // realizar la insercion
            InsertarReprocesoDetalle();

            //Se refrescan los datagrids
            CargarDataGrid();

            string destinatarios = ConsultarMailReproceso();
            string cuerpo = @"
                    <!DOCTYPE html>
                    <html lang='es'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Notificación de Reproceso</title>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                line-height: 1.6;
                                margin: 0;
                                padding: 0;
                                background-color: #f9f9f9;
                            }
                            .container {
                                max-width: 37rem;
                                margin: 20px auto;
                                padding: 20px;
                                border: 1px solid #ccc;
                                border-radius: 5px;
                                background-color: #fff;
                            }
                            h2 {
                                color: #333;
                                font-size: 24px;
                                margin-bottom: 20px;
                            }
                            p {
                                margin-bottom: 10px;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Notificación de Reproceso</h2>
                            <p>Fecha de observación: " + DateTime.Now.ToString() + @"</p>
                            <p>Por medio de la presente se informa que se ha asignado un reproceso al área: <strong> " + ddlArea1.SelectedItem.Text + @"</strong>, de la cual es responsable.</p>
                            <p>Proyecto: " + tbOT.Text + "-" + tbPedido.Text + " " + tbObra.Text + @"</p>
                            <p>Elemento: " + ddlElemento.SelectedItem.Text + @"</p>
                            <p>Agradecemos su colaboración ingresando al sistema de reprocesos.</p>
                        </div>
                    </body>
                    </html>";

            //ejecutar el procedimiento almacenado que envia el correo 
            EnviarCorreoReproceso(destinatarios, cuerpo);


        }
        private bool ValidarAgregar()
        {
            if (tbOT.Text == "" || tbPedido.Text == "")
            {
                string scriptExito = "alert('No se ha seleccionado ninguna Orden de trabajo.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                return false;
            }
            if (ddlArea1.SelectedItem.Value == "")
            {
                string scriptExito = "alert('Por favor, seleccione el area.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                return false;
            }
            if (ddlElemento.SelectedItem.Value == "")
            {
                string scriptExito = "alert('Por favor, seleccione el elemento.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                return false;
            }
            if (tbCantidad.Text == "")
            {
                string scriptExito = "alert('Por favor, ingrese la cantidad.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                return false;
            }



            return true;
        }
        private void InsertarReprocesoDetalle()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblReprocesoDetalle(Id_Elemento,Id_Area,OT,Pedido,Precio,cantidad) " +
                              "VALUES (@idElemento, @IdArea,@Ot, @pedido, @precio, @cantidad ) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@idElemento", ddlElemento.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdArea", ddlArea1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Ot", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", tbPedido.Text);
                    cmd.Parameters.AddWithValue("@precio", tbPrecio.Text);
                    cmd.Parameters.AddWithValue("@cantidad", tbCantidad.Text);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();

                    if(CantidadFilasAfectada > 0)
                    {
                        string scriptExito = "alert('El reproceso ha sido agragdo con exito.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptExito, true);
                    }
                   
                }

            }
        }
        public string ConsultarMailReproceso()
        {
            string mailResponsable = null;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            string query = "SELECT mailResponsable FROM tblAreaReproceso WHERE Descripcion like @area";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@area", ddlArea1.SelectedItem.Text);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    // Verificar si el resultado no es nulo
                    if (result != null)
                    {
                        // Convertir el resultado a string
                        mailResponsable = result.ToString();
                    }
                }
            }

            return mailResponsable;
        }
        public void EnviarCorreoReproceso(string destinatarios, string cuerpo)
        {
            string nombreProcedimiento = "duc_sp_Correo";
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionSID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(nombreProcedimiento, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Definir los parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@Destinatarios", destinatarios);
                    command.Parameters.AddWithValue("@asunto", "Notificación Reproceso");
                    command.Parameters.AddWithValue("@cuerpo", cuerpo);
                    command.Parameters.AddWithValue("@adjuntos", "");
                    command.Parameters.AddWithValue("@usuario", Session["usuariologueado"].ToString());

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Manejar la excepción (opcional)
                        error.Visible = true;
                        error.Text = ex.Message;
                    }
                }
            }
        }


        // Documentacion 
        protected void Adjuntar_Click(object sender, EventArgs e)
        {
            Session["OTReproceso"] = tbOT.Text;

            Session["pedReproceso"] = tbPedido.Text;

            string url = "DocumentacionReproceso.aspx";
            string script = "window.open('" + ResolveUrl(url) + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, GetType(), "openNewTab", script, true);
        }

        


        // Guardar Cambio de Reproceso 
        protected void Guardar_Click(object sender, EventArgs e)
        {
            if (chkAcepta.Checked)
            {
                if (ValidarGuardar())
                {
                    if (tbPrecio.Text != precioHidden.Text)
                    {
                        //LA PARTE DE MODEIFICAR EL PRECIO SOLO LO HACE DOÑA MARIAN DE MOMENTO ESTA PENDIENTE 
                        //CREAR EL METODO CambiarPrecioReproceso(); 
                        //string scriptNoSelect = "alert('Favor Justificar el cambio de precio en el reproceso.');";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showPrecioChanghe", scriptNoSelect, true);

                    }


                    DataTable Causas = ConsultarCausa();
                    if (Causas.Rows.Count <= 0)
                    {
                        // INSERTAR LA CAUSA SI ES NUEVA 
                        InsertarDetalleReproceso();
                    }

                    // ACTUALIZAR DETALLE DEL REPROCESO QUE FUE ACEPTADO

                    string idCausa = Causas.Rows[0]["id_Causa"].ToString();
                    ActualizarDetalleReprocesoAcepta(idCausa);

                    //Se refrescan los datagrids
                    CargarDataGrid();
                    string scriptExito = "alert('El proceso ha sido actualizado como aceptado.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showExito", scriptExito, true);


                }
            }
            else
            {
                if (txComentario.InnerText != "")
                {
                    if (tbPrecio.Text != precioHidden.Text)
                    {
                        //LA PARTE DE MODIFICAR EL PRECIO SOLO LO HACE DOÑA MARIAN DE MOMENTO ESTA PENDIENTE  

                    }

                    //PENDIENTE VALIDAR CUANDO EL ACEPTA TOME UN VALOR QUE 2 QUE NO SE ENTIENDE YA QUE ES CAMPO BIT
                    //CREAR EL METODO CambiarPrecioReproceso(); 

                    
                    if(Id_detalle.Text != "")
                    {
                        // ACTUALIZAMOS EL DETALLE DE REPROCESO QUE NO FUE ACEPTADO
                        ActualizarDetalleReprocesoNoAcepta();

                        //CREAR EL METODO CambiarPrecioReproceso(); 
                        
                        //Se refrescan los datagrids
                        CargarDataGrid();
                        string scriptExito = "alert('El proceso ha sido actualizado como no aceptado.');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showExito", scriptExito, true);

                       
                    }
                    else
                    {
                        string scriptExito = "alert('No se ha seleccionado ningun detalle de reproceso  .');";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showExito", scriptExito, true);
                    }
                  
                  

                }
                else
                {
                    string scriptExito = "alert('Se debe diligenciar el campo comentario antes de enviar.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showExito", scriptExito, true);
                }

            }


        }
        private DataTable ConsultarCausa()
        {

            DataTable dataTable = new DataTable();

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringISID))
            {
                connectionSID.Open();

                string sSql = "SELECT * FROM tblCausasReproceso WHERE activo = 1 AND Descripcion = @descripcion AND Area = @area  ORDER BY descripcion";

                using (SqlCommand cmd = new SqlCommand(sSql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@descripcion", ddlCausaRaiz.SelectedItem.Text);
                    cmd.Parameters.AddWithValue("@area", ddlArea1.SelectedItem.Text);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Retorna la DataTable que puede contener cero o más filas de resultados
            return dataTable;
        }
        public void InsertarDetalleReproceso()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblCausasReproceso (Descripcion,Activo,Area) " +
                               "VALUES (@descripcion,1, @area) ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@descripcion", ddlCausaRaiz.SelectedItem.Text);
                    command.Parameters.AddWithValue("@area", ddlArea1.SelectedItem.Text);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al insertar datos: " + ex.Message);
                    }
                }
            }
        }
        private void ActualizarDetalleReprocesoAcepta(string idcausa)
        {

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "UPDATE tblReprocesoDetalle SET id_Causa= @idcausa, Precio= @precio, Responsable = @responsable, correccion = @correccion," +
                              " Cerrado=1, Acepta=1, PorQue1= @pq1, PorQue2 = @pq2, PorQue3= @pq3, PorQue4 = @pq4, PorQue5 = @pq5  WHERE Id_Detalle= @idDetalle ";

                using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                {

                    // Asignar los valores de los parámetros
                    cmdUpdate.Parameters.AddWithValue("@idcausa", idcausa);
                    cmdUpdate.Parameters.AddWithValue("@precio", tbPrecio.Text);
                    cmdUpdate.Parameters.AddWithValue("@responsable", ddlResponsable.SelectedItem.Text);
                    cmdUpdate.Parameters.AddWithValue("@correccion", txCorreccion.InnerText);
                    cmdUpdate.Parameters.AddWithValue("@pq1", tbPq1.Text);
                    cmdUpdate.Parameters.AddWithValue("@pq2", tbPq2.Text);
                    cmdUpdate.Parameters.AddWithValue("@pq3", tbPq3.Text);
                    cmdUpdate.Parameters.AddWithValue("@pq4", tbPq4.Text);
                    cmdUpdate.Parameters.AddWithValue("@pq5", tbPq5.Text);
                    cmdUpdate.Parameters.AddWithValue("@pq5", tbPq5.Text);
                    cmdUpdate.Parameters.AddWithValue("@idDetalle", Id_detalle.Text);



                    // Ejecutar la actualización
                    cmdUpdate.ExecuteNonQuery();
                }
            }

        }
        private void ActualizarDetalleReprocesoNoAcepta()
        {

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "UPDATE tblReprocesoDetalle SET Observacion= @observacion, Precio= @precio, Cerrado = @cerrado," +
                              " Acepta=0  WHERE Id_Detalle= @idDetalle ";

                using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                {

                    // Asignar los valores de los parámetros
                    cmdUpdate.Parameters.AddWithValue("@observacion", txComentario.InnerText);
                    cmdUpdate.Parameters.AddWithValue("@precio", tbPrecio.Text);
                    cmdUpdate.Parameters.AddWithValue("@cerrado", chkAcepta.Checked);
                    cmdUpdate.Parameters.AddWithValue("@idDetalle", Id_detalle.Text);



                    // Ejecutar la actualización
                    cmdUpdate.ExecuteNonQuery();
                }
            }

        }
        private bool ValidarGuardar()
        {
            if (ddlResponsable.SelectedValue == "")
            {
                string scriptNoSelect = "alert('Por favor, seleccione un  responsable.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptNoSelect, true);
                return false;
            }
            if (ddlCausaRaiz.SelectedItem.Text == "")
            {
                string scriptNoSelect = "alert('Por favor, seleccione la causa.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptNoSelect, true);
                return false;
            }
            if (tbPq1.Text == "" || tbPq2.Text == "" || tbPq3.Text == "")
            {
                string scriptNoSelect = "alert('Falata ingresar algunos de los tres(3) primeros por qué.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoSelect", scriptNoSelect, true);
                return false;
            }

            return true;
        }



        //Redireccionar reproceso 
        protected void Redireccionar_Click(object sender, EventArgs e)
        {
            if (txComentario.InnerText != "" )
            {
                if (lbNombAreaRepro.Text != ddlArea1.SelectedItem.Text)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#redireccionarReproMoodal').modal('show');", true);
                }
                else
                {
                    string script = "alert('Se debe redireccionar el reproceso a un área diferente a la actual.  ');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoExi", script, true);
                }
              
            }

            else
            {
                string script = "alert('Debe diligenciar el campo Comentario antes de continuar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNoExi", script, true);
            }
        } 
        protected void btnRediret_Click(object sender, EventArgs e)
        {
            
                if (!AreaReprocesoIncluida(Id_detalle.Text, ddlArea1.SelectedValue))
                {
                    // continua el proceso de redireccion del reproceso 

                    if (tbPrecio.Text != precioHidden.Text)
                    {
                        //LA PARTE DE MODIFICAR EL PRECIO SOLO LO HACE DOÑA MARIAN GOMEZ  DE MOMENTO ESTA PENDIENTE 
                        //CREAR EL METODO CambiarPrecioReproceso(); 
                        //string scriptNoSelect = "alert('Favor Justificar el cambio de precio en el reproceso.');";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showPrecioChanghe", scriptNoSelect, true);

                    }


                    //ACTUALIZAR REPROCECESO 
                    ActualizarDetalleReprocesoRedireccionado();

                    //INSERTAR EL NUEVO REPROCESO 
                    InsertarReprocesoDetalleRedireccion();


                    // ENVIAR CORREO 
                    string destinatarios = ConsultarMailReproceso();
                string cuerpo = @"
                                <!DOCTYPE html>
                                <html lang='es'>
                                <head>
                                    <meta charset='UTF-8'>
                                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                    <title>Notificación de Reproceso</title>
                                  <style>
                                        body {
                                            font-family: Arial, sans-serif;
                                            font-size: 14px;
                                            line-height: 1.6;
                                            margin: 0;
                                            padding: 0;
                                            background-color: #f9f9f9;
                                        }
                                        .container {
                                            max-width: 600px; /* Ampliar el ancho para una mejor legibilidad en escritorios */
                                            margin: 20px auto;
                                            padding: 20px;
                                            border: 1px solid #ccc;
                                            border-radius: 5px;
                                            background-color: #fff;
                                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* Agregar una sombra suave */
                                        }
                                        h2 {
                                            color: #333;
                                            font-size: 24px;
                                            margin-bottom: 20px;
                                        }
                                        p {
                                            margin-bottom: 10px;
                                            color: #666; /* Cambiar el color del texto para mayor contraste */
                                        }
                                        strong {
                                            font-weight: bold;
                                            color: #000; /* Cambiar el color del texto fuerte */
                                        }
                                        em {
                                            font-style: italic; /* Agregar estilo cursiva */
                                        }
                                        a {
                                            color: #007bff; /* Cambiar el color del enlace */
                                            text-decoration: none; /* Quitar el subrayado predeterminado */
                                        }
                                        a:hover {
                                            text-decoration: underline; /* Subrayar el enlace al pasar el mouse */
                                        }
                                    </style>

                                </head>
                                <body>
                                    <div class='container'>
                                        <h2>Notificación de Reproceso.</h2>
                                        <p>Fecha de observación: " + DateTime.Now.ToString() + @"</p>
                                        <p>Por medio de la presente se informa que el área: <strong>" + lbNombAreaRepro.Text + @"</strong> ha redireccionado un reproceso al área: <strong>" + ddlArea1.SelectedItem.Text + @"</strong> de la cual es responsable.</p>
                                        <p>Proyecto: " + tbOT.Text + "-" + tbPedido.Text + " " + tbObra.Text + @"</p>
                                       <p>Elemento: <strong><em>" + ddlElemento.SelectedItem.Text + @"</em></strong></p>
                                        <p>Comentario: " + txComentario.InnerText + @"</p>
                                        <p>Agradecemos su colaboración ingresando al sistema de reprocesos.</p>
                                    </div>
                                </body>
                                </html>";


                    //ejecutar el procedimiento almacenado que envia el correo 
                    EnviarCorreoReproceso(destinatarios, cuerpo);
                
                //Se refrescan los datagrids
                CargarDataGrid();
               
                string script = "alert('La redirección del reproceso ha sido notificada.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoExi", script, true);


                }
                else
                {
                    string nombreArea = ddlArea1.SelectedItem.Text;
                    string script = $"alert('El área {nombreArea} es origen del reproceso, el sistema NO permite Delvolver el reproceso, por favor comunicarse con el responsable del area.');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNoExi", script, true);
                }
           
        }
        private bool AreaReprocesoIncluida(string idDetalle, string idArea)
        {
            DataTable detalleReproceso = ConsultarDetalle(idDetalle);

            if (detalleReproceso.Rows.Count > 0)
            {
                foreach (DataRow row in detalleReproceso.Rows)
                {
                    string idAreaRedirigido = row["Id_DetalleOrigen"]?.ToString();

                    if (idAreaRedirigido == null || idAreaRedirigido == "")
                    {
                        // no tiene asiganado ningun area esta vacia o null , se puede redireccionar 
                    }
                    else
                    {
                        string id = ConsultarIdDetalle(idAreaRedirigido);

                        if (id == idArea)
                        {
                            return true;
                        }
                    }

                }
            }




            return false;
        }
        private DataTable ConsultarDetalle(string idDetalle)
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT  * FROM tblReprocesoDetalle WHERE id_Detalle= @idDetalle";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idDetalle", idDetalle);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        public string ConsultarIdDetalle(string idDetRedirigido)
        {
            string idDetalle = null;

            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            string query = "Select Id_Area from tblReprocesoDetalle where id_Detalle= @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", idDetRedirigido);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    // Verificar si el resultado no es nulo
                    if (result != null)
                    {
                        // Convertir el resultado a string
                        idDetalle = result.ToString();
                    }
                }
            }

            return idDetalle;
        }
        private void ActualizarDetalleReprocesoRedireccionado()
        {

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionISID = new SqlConnection(connectionStringISID))
            {
                connectionISID.Open();

                string sSql = "UPDATE tblReprocesoDetalle SET Observacion= @observacion, Precio= @precio, Cerrado=0, Acepta=0,Redirigido = @redirigido WHERE Id_Detalle= @idDetalle ";

                using (SqlCommand cmdUpdate = new SqlCommand(sSql, connectionISID))
                {

                    // Asignar los valores de los parámetros
                    cmdUpdate.Parameters.AddWithValue("@observacion", txComentario.InnerText);
                    cmdUpdate.Parameters.AddWithValue("@precio", tbPrecio.Text);
                    cmdUpdate.Parameters.AddWithValue("@redirigido", ddlArea1.SelectedItem.Text);
                    cmdUpdate.Parameters.AddWithValue("@idDetalle", Id_detalle.Text);



                    // Ejecutar la actualización
                    cmdUpdate.ExecuteNonQuery();
                }
            }

        }
        private void InsertarReprocesoDetalleRedireccion()
        {
            // Consulta para verificar si el usuario tiene permisos
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sSql = "INSERT INTO tblReprocesoDetalle(Id_Elemento,Id_Area,OT,Pedido,Precio,cantidad,Origen,Id_DetalleOrigen) " +
                              "VALUES (@idElemento, @IdArea,@Ot, @pedido, @precio, @cantidad,@Origen,@Id_DetalleOrigen ) ";

                using (SqlCommand cmd = new SqlCommand(sSql, connection))
                {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@idElemento", ddlElemento.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdArea", ddlArea1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Ot", tbOT.Text);
                    cmd.Parameters.AddWithValue("@pedido", tbPedido.Text);
                    cmd.Parameters.AddWithValue("@precio", tbPrecio.Text);
                    cmd.Parameters.AddWithValue("@cantidad", tbCantidad.Text);
                    cmd.Parameters.AddWithValue("@Origen", lbNombAreaRepro.Text);
                    cmd.Parameters.AddWithValue("@Id_DetalleOrigen", Id_detalle.Text);


                    // Variable para validar en depuracion si se afecto alguna linea con este query 
                    int CantidadFilasAfectada = cmd.ExecuteNonQuery();
                }

            }
        }


        // TAP ESTADISTICAS CONSOLIDADO 


        // Carga los años en el dropdownlist de año 
        private void LlenarAño()
        {
            // Agregar opciones para los años desde el año actual hasta 1900
            for (int i = DateTime.Now.Year; i >= 1900; i--)
            {
                ddlAnioBusqueda.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }


        // click en consultar ( carga Reprocesos consolidados, cantidad de reprocesos por area y por mes ,  precio reproceso por area y por mes )
        protected void btnConsultar1_Click(object sender, EventArgs e)
        {
            // Calcular las estadisticas consolidadas de Reprocesos
            ConsultarConsolidado();

            // Calcular las Cantidades Reproceso por Area 
            ConsultarCantidadReproXArea();

            // Calcular los Precios de Reproceso por Área
            ConsultarPrecioReproXArea();

        }
        protected double ObtenerValor(string query)
        {
            double result = 0.0;
            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionStringISID))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        // Ejecutar la consulta y obtener el valor escalar
                        object scalarValue = cmd.ExecuteScalar();

                        // Verificar si el valor obtenido no es nulo y convertirlo a double
                        if (scalarValue != null && scalarValue != DBNull.Value)
                        {
                            result = Convert.ToDouble(scalarValue);
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Manejar la excepción, registrarla, mostrar un mensaje de error, etc.
                    }
                }
            }

            return result;
        }
        private void ConsultarConsolidado()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mes", typeof(string));
            dt.Columns.Add("CantRepr", typeof(int));
            dt.Columns.Add("CantOT", typeof(int));
            dt.Columns.Add("PorcRepr", typeof(string));
            dt.Columns.Add("CostoTotales", typeof(string));
            dt.Columns.Add("CostoDucon", typeof(string));
            dt.Columns.Add("VentasMes", typeof(string));
            dt.Columns.Add("PorcRepVent", typeof(string));
            dt.Columns.Add("Meta", typeof(string));

            double totalCantRepr = 0;
            double totalCantOT = 0;
            double totalPorReproce = 0;
            double totalCostoTotales = 0;
            double totalCostoDucon = 0;
            double totalVentasMes = 0;
            double totalRepVen = 0;

            for (int mes = 1; mes <= 12; mes++)
            {
                DataRow dr = dt.NewRow();
                dr["Mes"] = DateTimeFormatInfo.CurrentInfo.GetMonthName(mes).ToUpper();

                // Obtenemos  la cantidad de reprocesos por mes
                string sSqlRepr = $"SELECT COUNT(Id_OT) AS CantRepr FROM tblReporteOT " +
                                  $"WHERE (Id_TipoPedido = '8') AND (MONTH(Fecha_Entrega_Produccion) = {mes}) " +
                                  $"AND (YEAR(Fecha_Entrega_Produccion) = {ddlAnioBusqueda.SelectedItem.Text})";
                double cantRepr = ObtenerValor(sSqlRepr);

                dr["CantRepr"] = cantRepr;

                // Obtenemos la cantidad de OT por mes
                string sSqlOT = $"SELECT COUNT(Id_OT) AS CantOT FROM tblReporteOT" +
                                $" WHERE (MONTH(Fecha_Entrega_Produccion) = {mes}) " +
                                $"AND (YEAR(Fecha_Entrega_Produccion) = {ddlAnioBusqueda.SelectedItem.Text})";
                double cantOT = ObtenerValor(sSqlOT);
                dr["CantOT"] = cantOT;

                // Calculamos en porcentaje de Reproceros x Obra 
                double Porcentaje = Math.Round((cantRepr / cantOT) * 100, 2);
                dr["PorcRepr"] = Porcentaje.ToString("0.##") + "%";

                // Consultamos  el costo total
                string sSqlCosto = $"SELECT SUM(CAST(tblReprocesoDetalle.Precio AS int)) AS Costo " +
                                   $"FROM tblReprocesoDetalle INNER JOIN tblReporteOT " +
                                   $"ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT " +
                                   $"AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                                   $"WHERE (MONTH(tblReporteOT.Fecha_Entrega_Produccion) = {mes}) " +
                                   $"AND (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = {ddlAnioBusqueda.SelectedItem.Text}) " +
                                   $"AND (tblReprocesoDetalle.Acepta = 1)";
                double costoTotal = ObtenerValor(sSqlCosto);
                dr["CostoTotales"] = costoTotal.ToString("$ ###,###,##0");

                // Consultamos el costo de los reprocesos aceptados
                string sSqlCostoDucon = $"SELECT SUM(CAST(tblReprocesoDetalle.Precio AS int)) AS CostoPr " +
                                        $"FROM tblReprocesoDetalle INNER JOIN tblReporteOT " +
                                        $"ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT " +
                                        $"AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                                        $"WHERE (MONTH(tblReporteOT.Fecha_Entrega_Produccion) = {mes}) " +
                                        $"AND (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = {ddlAnioBusqueda.SelectedItem.Text}) " +
                                        $"AND (tblReprocesoDetalle.Acepta = 1)";
                double costoDucon = ObtenerValor(sSqlCostoDucon);
                if (costoDucon != 0.0)
                {
                    // Restar el valor de CostoPr de CostoTotales
                    double costoTotal1 = costoTotal - costoDucon;
                    dr["CostoDucon"] = costoTotal1.ToString("$ ###,###,##0");
                }
                else
                {
                    dr["CostoDucon"] = costoTotal.ToString("$ ###,###,##0");
                }

                double ventaMes = 0.0;
                string sSqlVentaMes = $"SELECT Sum(Precio_Venta-Precio_Venta*Descuento/100) AS TotalMes " +
                                      $"FROM tblTipoPedido INNER JOIN tblReporteOT " +
                                      $"ON tblTipoPedido.Id_TipoPedido = tblReporteOT.Id_TipoPedido " +
                                      $"WHERE ((MONTH(Fecha_Confirmacion_Venta) = {mes}) " +
                                      $"AND (YEAR(Fecha_Confirmacion_Venta) = {ddlAnioBusqueda.SelectedItem.Text}) " +
                                      $"AND ((tblTipoPedido.EstadisticaVenta)='1'))";

                ventaMes = ObtenerValor(sSqlVentaMes);

                // Verificamo si obtuvimos un valor para TotalMes
                double PorcRepVent = 0;
                if (ventaMes != 0.0)
                {
                    dr["VentasMes"] = ventaMes.ToString("$ ###,###,##0");
                }
                else
                {
                    dr["VentasMes"] = "0";
                }

                if (costoTotal == 0 || ventaMes == 0)
                {
                    dr["PorcRepVent"] = 0;
                }
                else
                {
                    PorcRepVent = Math.Round((costoTotal / ventaMes) * 100, 2);
                    dr["PorcRepVent"] = PorcRepVent.ToString("0.##") + "%"; ;

                }

                double MetaDou = Math.Round(5.00, 2);
                string MetaSting = MetaDou.ToString() + " %";
                dr["Meta"] = MetaSting;

                totalCantRepr += cantRepr;
                totalCantOT += cantOT;
                totalPorReproce += Porcentaje;
                totalCostoTotales += costoTotal;
                totalCostoDucon += costoDucon - costoTotal;
                totalVentasMes += ventaMes;
                totalRepVen += PorcRepVent;



                dt.Rows.Add(dr);
            }

            // Agregar la fila de totales
            DataRow totalRow = dt.NewRow();
            totalRow["Mes"] = "TOTAL";
            totalRow["CantRepr"] = totalCantRepr;
            totalRow["CantOT"] = totalCantOT;
            totalRow["PorcRepr"] = ((totalCantRepr / totalCantOT) * 100).ToString("0.##") + "%"; ;
            totalRow["CostoTotales"] = totalCostoTotales.ToString("$ ###,###,##0");
            totalRow["CostoDucon"] = totalCostoDucon.ToString("$ ###,###,##0");
            totalRow["VentasMes"] = totalVentasMes.ToString("$ ###,###,##0");
            totalRow["PorcRepVent"] = ((totalCostoTotales / totalVentasMes) * 100).ToString("0.##") + "%";

            totalRow["Meta"] = "5.00%";

            dt.Rows.Add(totalRow);


            DatagridConsolidado.DataSource = dt;
            DatagridConsolidado.DataBind();

        }
        private DataTable ConsultarCantidadReproXArea()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Responsable", typeof(string));
            for (int i = 1; i <= 12; i++)
            {
                dt.Columns.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), typeof(string));
            }
            dt.Columns.Add("Total", typeof(int));
            dt.Columns.Add("Porcentaje", typeof(string));

            DataTable Areas = ObtenerNombreAreas();

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (var connection = new SqlConnection(connectionStringISID))
            {
                connection.Open();

                int sumaTotal = 0; // Inicializar la suma total

                foreach (DataRow rowArea in Areas.Rows)
                {
                    DataRow row = dt.NewRow();

                    row["Responsable"] = rowArea["Descripcion"];
                    int totalArea = 0;

                    int idArea = (int)rowArea["Id_Area"];

                    for (int mes = 1; mes <= 12; mes++)
                    {
                        int valorMes = ObtenerCantidadReprocesoPorMes(connection, mes, ddlAnioBusqueda.SelectedItem.Text, idArea);
                        string MesString = DateTimeFormatInfo.CurrentInfo.GetMonthName(mes);
                        row[MesString] = valorMes;
                        totalArea += valorMes;
                    }

                    row["Total"] = totalArea;
                    sumaTotal += totalArea; // Sumar al total general
                    dt.Rows.Add(row);
                }

                // Calcular los porcentajes después de llenar el DataTable
                foreach (DataRow row in dt.Rows)
                {
                    double porcentaje = ((int)row["Total"] / (double)sumaTotal) * 100;
                    row["Porcentaje"] = porcentaje.ToString("#0.##") + "%";
                }
            }

            // Enlazar DataTable a tu DataGrid
            DatagridCantidad.DataSource = dt;
            DatagridCantidad.DataBind();

            return dt;
        }
        private void ConsultarPrecioReproXArea()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Responsable", typeof(string));
            for (int i = 1; i <= 12; i++)
            {
                dt.Columns.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), typeof(string));
            }
            dt.Columns.Add("Total", typeof(string));


            DataTable Areas = ObtenerNombreAreas();

            string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
            using (var connection = new SqlConnection(connectionStringISID))
            {
                connection.Open();

                foreach (DataRow rowArea in Areas.Rows)
                {
                    DataRow row = dt.NewRow();

                    row["Responsable"] = rowArea["Descripcion"];
                    double totalArea = 0; // Variable para almacenar el total por área

                    int idArea = (int)rowArea["Id_Area"];

                    for (int mes = 1; mes <= 12; mes++)
                    {
                        double valorMes = ObtenerValorReprocesoPorMes(connection, mes, ddlAnioBusqueda.SelectedItem.Text, idArea);
                        string MesString = DateTimeFormatInfo.CurrentInfo.GetMonthName(mes);
                        row[MesString] = valorMes.ToString("$ ###,###,##0");
                        totalArea += valorMes; // Agregar al total por área
                    }

                    row["Total"] = totalArea.ToString("$ ###,###,##0"); // Asignar el total por área
                    dt.Rows.Add(row);
                }

            }




            // Enlazar DataTable a tu DataGrid
            DatagridPrecioArea.DataSource = dt;
            DatagridPrecioArea.DataBind();
        }
        private DataTable ObtenerNombreAreas()
        {
            DataTable dataTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id_Area, Descripcion FROM tblAreaReproceso ORDER BY Descripcion";

                using (SqlCommand command = new SqlCommand(query, connection))
                {


                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        private double ObtenerValorReprocesoPorMes(SqlConnection connection, int mes, string ano, int idArea)
        {
            string sSql = "SELECT SUM(CAST(tblReprocesoDetalle.Precio AS int)) AS Valor FROM tblReprocesoDetalle " +
                          "INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                          "WHERE (MONTH(tblReporteOT.Fecha_Entrega_Produccion) = @mes) AND (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio) " +
                          "AND (tblReprocesoDetalle.Id_Area = @idArea) AND (tblReprocesoDetalle.Acepta = 1)";

            using (var cmdDetalle = new SqlCommand(sSql, connection))
            {
                cmdDetalle.Parameters.AddWithValue("@mes", mes);
                cmdDetalle.Parameters.AddWithValue("@anio", ano);
                cmdDetalle.Parameters.AddWithValue("@idArea", idArea);

                object valor = cmdDetalle.ExecuteScalar();
                double valorDouble = (valor != DBNull.Value) ? Convert.ToDouble(valor) : 0.0;
                return valorDouble;
            }
        }
        private int ObtenerCantidadReprocesoPorMes(SqlConnection connection, int mes, string anio, int idArea)
        {
            string sSql = "SELECT COUNT( tblReprocesoDetalle.Id_Causa) AS Cantidad FROM tblReprocesoDetalle " +
                          "INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                          "WHERE (MONTH(tblReporteOT.Fecha_Entrega_Produccion) = @mes) AND (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio ) " +
                          "AND (tblReprocesoDetalle.Id_Area = @idArea) AND (tblReprocesoDetalle.Acepta = 1)";

            using (var cmdDetalle = new SqlCommand(sSql, connection))
            {
                cmdDetalle.Parameters.AddWithValue("@mes", mes);
                cmdDetalle.Parameters.AddWithValue("@anio", anio);
                cmdDetalle.Parameters.AddWithValue("@idArea", idArea);

                object valor = cmdDetalle.ExecuteScalar();
                int cantidad = (valor != DBNull.Value) ? Convert.ToInt32(valor) : 0;
                return cantidad;
            }
        }



        // Click en el DataGrid de Cantidad de reprocesos (Carga DataGrid Resposables y Datagrid Causas) 
        protected void DatagridCantidad_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            string area = ((LinkButton)e.Item.FindControl("Responsable")).Text;
            string anio = ddlAnioBusqueda.SelectedItem.Text;

            // Recorre todas las celdas y elimina el color de fondo
            foreach (DataGridItem item in DatagridCantidad.Items)
            {
                // Encuentra los LinkButton correspondientes a cada mes
                LinkButton enero = (LinkButton)item.FindControl("Enero");
                LinkButton febrero = (LinkButton)item.FindControl("Febrero");
                LinkButton marzo = (LinkButton)item.FindControl("Marzo");
                LinkButton abril = (LinkButton)item.FindControl("Abril");
                LinkButton mayo = (LinkButton)item.FindControl("Mayo");
                LinkButton junio = (LinkButton)item.FindControl("Junio");
                LinkButton julio = (LinkButton)item.FindControl("Julio");
                LinkButton agosto = (LinkButton)item.FindControl("Agosto");
                LinkButton septiembre = (LinkButton)item.FindControl("Septiembre");
                LinkButton octubre = (LinkButton)item.FindControl("Octubre");
                LinkButton noviembre = (LinkButton)item.FindControl("Noviembre");
                LinkButton diciembre = (LinkButton)item.FindControl("Diciembre");
                LinkButton responsable = (LinkButton)item.FindControl("Responsable");
                // Verifica si se encontró cada LinkButton y elimina el color de fondo
                if (enero != null)
                {
                    enero.Style.Remove("background-color");
                    enero.Style.Remove("color");
                }
                if (febrero != null)
                {
                    febrero.Style.Remove("background-color");
                    febrero.Style.Remove("color");
                }
                if (marzo != null)
                {
                    marzo.Style.Remove("background-color");
                    marzo.Style.Remove("color");
                }
                if (abril != null)
                {
                    abril.Style.Remove("background-color");
                    abril.Style.Remove("color");
                }
                if (mayo != null)
                {
                    mayo.Style.Remove("background-color");
                    mayo.Style.Remove("color");
                }
                if (junio != null)
                {
                    junio.Style.Remove("background-color");
                    junio.Style.Remove("color");
                }
                if (julio != null)
                {
                    julio.Style.Remove("background-color");
                    julio.Style.Remove("color");
                }
                if (agosto != null)
                {
                    agosto.Style.Remove("background-color");
                    agosto.Style.Remove("color");
                }
                if (septiembre != null)
                {
                    septiembre.Style.Remove("background-color");
                    septiembre.Style.Remove("color");
                }
                if (octubre != null)
                {
                    octubre.Style.Remove("background-color");
                    octubre.Style.Remove("color");
                }
                if (noviembre != null)
                {
                    noviembre.Style.Remove("background-color");
                    noviembre.Style.Remove("color");
                }
                if (diciembre != null)
                {
                    diciembre.Style.Remove("background-color");
                    diciembre.Style.Remove("color");
                }
                if (responsable != null)
                {
                    responsable.Style.Remove("background-color");
                    responsable.Style.Remove("color");
                }

            }

            switch (e.CommandName)
            {


                case "VerFullDetalle":

                    LinkButton respon = (LinkButton)e.Item.FindControl("Responsable");
                    respon.Style["background-color"] = "black";
                    respon.Style["color"] = "white";

                    CagarDatagridCausa(anio, area);
                    CargarDatagridResponsable(anio, area);

                    break;

                case "VerEnero":

                    LinkButton ene = (LinkButton)e.Item.FindControl("Enero");
                    ene.Style["background-color"] = "black";
                    ene.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Jan");
                    CargarDatagridResponsableXMes(anio, area, "Jan");
                    break;

                case "VerFebrero":

                    LinkButton feb = (LinkButton)e.Item.FindControl("Febrero");
                    feb.Style["background-color"] = "black";
                    feb.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Feb");
                    CargarDatagridResponsableXMes(anio, area, "Feb");
                    break;

                case "VerMarzo":

                    LinkButton mar = (LinkButton)e.Item.FindControl("Marzo");
                    mar.Style["background-color"] = "black";
                    mar.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Mar");
                    CargarDatagridResponsableXMes(anio, area, "Mar");
                    break;

                case "VerAbril":
                    LinkButton abr = (LinkButton)e.Item.FindControl("Abril");
                    abr.Style["background-color"] = "black";
                    abr.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Apr");
                    CargarDatagridResponsableXMes(anio, area, "Apr");
                    break;

                case "VerMayo":

                    LinkButton may = (LinkButton)e.Item.FindControl("Mayo");
                    may.Style["background-color"] = "black";
                    may.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "May");
                    CargarDatagridResponsableXMes(anio, area, "May");
                    break;

                case "VerJunio":


                    LinkButton jun = (LinkButton)e.Item.FindControl("Junio");
                    jun.Style["background-color"] = "black";
                    jun.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Jun");
                    CargarDatagridResponsableXMes(anio, area, "Jun");
                    break;

                case "VerJulio":

                    LinkButton jul = (LinkButton)e.Item.FindControl("Julio");
                    jul.Style["background-color"] = "black";
                    jul.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Jul");
                    CargarDatagridResponsableXMes(anio, area, "Jul");
                    break;

                case "VerAgosto":

                    LinkButton ago = (LinkButton)e.Item.FindControl("Agosto");
                    ago.Style["background-color"] = "black";
                    ago.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Aug");
                    CargarDatagridResponsableXMes(anio, area, "Aug");
                    break;

                case "VerSeptiembre":

                    LinkButton sep = (LinkButton)e.Item.FindControl("Septiembre");
                    sep.Style["background-color"] = "black";
                    sep.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Sep");
                    CargarDatagridResponsableXMes(anio, area, "Sep");
                    break;


                case "VerOctubre":

                    LinkButton oct = (LinkButton)e.Item.FindControl("Octubre");
                    oct.Style["background-color"] = "black";
                    oct.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Oct");
                    CargarDatagridResponsableXMes(anio, area, "Oct");
                    break;

                case "VerNoviembre":

                    LinkButton nov = (LinkButton)e.Item.FindControl("Noviembre");
                    nov.Style["background-color"] = "black";
                    nov.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Nov");
                    CargarDatagridResponsableXMes(anio, area, "Nov");
                    break;

                case "VerDiciembre":

                    LinkButton dic = (LinkButton)e.Item.FindControl("Diciembre");
                    dic.Style["background-color"] = "black";
                    dic.Style["color"] = "white";

                    CagarDatagridCausaXMes(anio, area, "Dec");
                    CargarDatagridResponsableXMes(anio, area, "Dec");
                    break;


            }

            // limpiamos el DataGrid Causa Resposanble 
            DatagridRespoCausa.DataBind();

        }
        private void CagarDatagridCausa(string anio, string area)
        {
            DataTable dataTable = new DataTable();

            // query para obtener los responsables de reproceso de ese año 
            string sql = $"SELECT COUNT(tblCausasReproceso.Descripcion) AS Cantidad, tblCausasReproceso.Descripcion AS Causa,tblAreaReproceso.Descripcion," +
                                  $"Mes = '%' FROM tblReprocesoDetalle  " +
                                  $"INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                                  $"INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                                  $"INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido  " +
                                  $"WHERE (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio ) AND (tblAreaReproceso.Descripcion = @Area) " +
                                  $"GROUP BY tblCausasReproceso.Descripcion, tblReprocesoDetalle.Acepta,tblAreaReproceso.Descripcion  Having (tblReprocesoDetalle.Acepta = 1) ORDER BY Causa ";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DataGridCausa.DataSource = dataTable;
            DataGridCausa.DataBind();
        }
        private void CargarDatagridResponsable(string anio, string area)
        {
            DataTable dataTable = new DataTable();

            // query para obtener las causas de reproceso de ese año 
            string sql = $"SELECT COUNT(tblReprocesoDetalle.Responsable) AS Cantidad, tblReprocesoDetalle.Responsable,tblAreaReproceso.Descripcion, Mes = '%' " +
                                        $"FROM tblReprocesoDetalle INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                                        $"INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT " +
                                        $"AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                                        $"INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                                        $"WHERE (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio)  AND   (tblAreaReproceso.Descripcion = @area) " +
                                        $"GROUP BY tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Responsable,tblAreaReproceso.Descripcion Having (tblReprocesoDetalle.Acepta = 1) ";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DatagridResponsable.DataSource = dataTable;
            DatagridResponsable.DataBind();
        }
        private void CagarDatagridCausaXMes(string anio, string area, string mes)
        {
            DataTable dataTable = new DataTable();

            // query para obtener las causas  de reprocesos por mes
            string sql = $"SELECT COUNT(tblCausasReproceso.Descripcion) AS Cantidad, tblCausasReproceso.Descripcion AS Causa," +
                                 $" tblAreaReproceso.Descripcion,CONVERT(char(3), tblReporteOT.Fecha_Entrega_Produccion, 0) As Mes FROM tblReprocesoDetalle  " +
                                 $"INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                                 $"INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                                 $"INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido  " +
                                 $"WHERE(YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio ) AND (CONVERT(char(3), tblReporteOT.Fecha_Entrega_Produccion, 0) = @mes)  " +
                                 $" AND (tblAreaReproceso.Descripcion = @Area) " +
                                 $"GROUP BY tblCausasReproceso.Descripcion, tblReprocesoDetalle.Acepta, tblAreaReproceso.Descripcion," +
                                 $"CONVERT(char(3), tblReporteOT.Fecha_Entrega_Produccion, 0) Having (tblReprocesoDetalle.Acepta = 1) ORDER BY Causa ";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@mes", mes);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DataGridCausa.DataSource = dataTable;
            DataGridCausa.DataBind();
        }
        private void CargarDatagridResponsableXMes(string anio, string area, string mes)
        {
            DataTable dataTable = new DataTable();

            // query para obtener el responsable   de reprocesos por mes
            string sql = $"SELECT COUNT(tblReprocesoDetalle.Responsable) AS Cantidad, tblReprocesoDetalle.Responsable,tblAreaReproceso.Descripcion, " +
                                        $"CONVERT(char(3), tblReporteOT.Fecha_Entrega_Produccion, 0) As Mes FROM tblReprocesoDetalle INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                                        $"INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT " +
                                        $"AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                                        $"INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                                        $"WHERE (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio)AND (CONVERT(char(3), tblReporteOT.Fecha_Entrega_Produccion, 0) = @mes) " +
                                        $" AND  (tblAreaReproceso.Descripcion = @area) " +
                                        $"GROUP BY tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Responsable,tblAreaReproceso.Descripcion," +
                                        $"CONVERT(char(3), tblReporteOT.Fecha_Entrega_Produccion, 0) Having (tblReprocesoDetalle.Acepta = 1) ";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@mes", mes);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DatagridResponsable.DataSource = dataTable;
            DatagridResponsable.DataBind();
        }



        // Click en Datagrids de Responsable y Causas (carga DataGrid CausaResposable)
        protected void DatagridResponsable_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            // Causa por Responsable 
            if (e.CommandName == "VerRespXCausa")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DatagridResponsable.Items[rowIndex];


                foreach (DataGridItem item in DatagridResponsable.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  
                e.Item.CssClass = "fila-seleccionada";



                string respon = row.Cells[1].Text;
                string area = row.Cells[3].Text;
                string mes = row.Cells[4].Text;
                string anio = ddlAnioBusqueda.SelectedItem.Text;


                DataGridColumn columna = DatagridRespoCausa.Columns[0];


                if (mes == "%")
                {
                    //Filtro Por mes 
                    columna.HeaderText = "Causa x Responsable " + anio;
                    CargarDetalleResponsableCausaFull(respon, area, anio);


                }
                else
                {
                    //Filtro por año
                    columna.HeaderText = "Causa x Responsable " + ObtenerNombreMes(mes) + " " + anio;
                    CargarDetalleResponsableCausaMes(respon, area, anio, mes);

                }


            }
        }
        protected void DataGridCausa_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            // Resposable X Causa

            if (e.CommandName == "VerRespXCausa1")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridCausa.Items[rowIndex];

                foreach (DataGridItem item in DataGridCausa.Items)
                {
                    if (item != row)
                    {
                        item.CssClass = ""; // Elimina la clase CSS de las filas no seleccionadas
                    }
                }

                //se usa Para darle un color a la fila seleccionada  
                e.Item.CssClass = "fila-seleccionada";



                string respon = row.Cells[1].Text;
                string area = row.Cells[3].Text;
                string mes = row.Cells[4].Text;
                string anio = ddlAnioBusqueda.SelectedItem.Text;

                DataGridColumn columna = DatagridRespoCausa.Columns[0];


                if (mes == "%")
                {
                    //Filtro Por mes  
                    columna.HeaderText = "Responsable x Causa " + anio;
                    CargarDetalleCausaXResponsableFull(respon, area, anio);

                }
                else
                {
                    //Filtro sin año
                    columna.HeaderText = "Responsable x Causa " + ObtenerNombreMes(mes) + " " + anio;
                    CargarDetalleCausaXResponsableMes(respon, area, anio, mes);


                }

            }

        }
        private void CargarDetalleResponsableCausaFull(string respo, string area, string anio)
        {
            DataTable dataTable = new DataTable();

            // query para obtener las causa por responsable  de reprocesos por año
            string sql = $"SELECT tblCausasReproceso.Descripcion AS RespoCausa, COUNT(tblCausasReproceso.Descripcion) AS Cantidad " +
                         $"FROM tblReprocesoDetalle INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                         $"INNER JOIN  tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                         $"INNER JOIN  tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT " +
                         $"AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                         $"WHERE (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio) " +
                         $"AND (tblAreaReproceso.Descripcion = @area) AND (tblReprocesoDetalle.Responsable = @respo) " +
                         $"GROUP BY tblCausasReproceso.Descripcion, tblReprocesoDetalle.Acepta Having (tblReprocesoDetalle.Acepta = 1) ORDER BY RespoCausa";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@respo", respo);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DatagridRespoCausa.DataSource = dataTable;
            DatagridRespoCausa.DataBind();
        }
        private void CargarDetalleResponsableCausaMes(string respo, string area, string anio, string mes)
        {
            DataTable dataTable = new DataTable();

            // query para obtener el responsable por causa de reprocesos por mes
            string sql = $"SELECT tblCausasReproceso.Descripcion AS RespoCausa, COUNT(tblCausasReproceso.Descripcion) AS Cantidad " +
                         $"FROM tblReprocesoDetalle INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                         $"INNER JOIN  tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                         $"INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                         $"WHERE (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio) " +
                         $"AND (tblAreaReproceso.Descripcion = @area) AND (CONVERT(char(3),tblReporteOT.Fecha_Entrega_Produccion, 0) = @mes) " +
                         $"AND (tblReprocesoDetalle.Responsable = @respo)GROUP BY tblCausasReproceso.Descripcion, tblReprocesoDetalle.Acepta " +
                         $"Having (tblReprocesoDetalle.Acepta = 1) ORDER BY RespoCausa";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@respo", respo);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@mes", mes);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DatagridRespoCausa.DataSource = dataTable;
            DatagridRespoCausa.DataBind();
        }
        private void CargarDetalleCausaXResponsableFull(string respo, string area, string anio)
        {
            DataTable dataTable = new DataTable();

            // query para obtener las causas por responsable   de reprocesos por año
            string sql = $"SELECT tblReprocesoDetalle.Responsable AS RespoCausa , COUNT(tblReprocesoDetalle.Responsable) AS Cantidad " +
                         $"FROM tblReprocesoDetalle INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                         $"INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                         $"INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                         $"WHERE (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio) " +
                         $"AND (tblAreaReproceso.Descripcion = @area) AND (tblCausasReproceso.Descripcion = @respo) " +
                         $"GROUP BY tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Responsable Having (tblReprocesoDetalle.Acepta = 1)";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@respo", respo);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DatagridRespoCausa.DataSource = dataTable;
            DatagridRespoCausa.DataBind();



        }
        private void CargarDetalleCausaXResponsableMes(string respo, string area, string anio, string mes)
        {
            DataTable dataTable = new DataTable();

            // query para obtener la causa por responsable de reprocesos por mes

            string sql = $"SELECT tblReprocesoDetalle.Responsable As RespoCausa,COUNT(tblReprocesoDetalle.Responsable) AS Cantidad " +
                         $"FROM tblReprocesoDetalle INNER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa " +
                         $"INNER JOIN tblReporteOT ON tblReprocesoDetalle.Ot = tblReporteOT.Id_OT AND tblReprocesoDetalle.Pedido = tblReporteOT.Consecutivo_Pedido " +
                         $"INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area " +
                         $"WHERE (YEAR(tblReporteOT.Fecha_Entrega_Produccion) = @anio) AND (tblAreaReproceso.Descripcion = @area) " +
                         $"AND (CONVERT(char(3),tblReporteOT.Fecha_Entrega_Produccion, 0) = @mes) AND (tblCausasReproceso.Descripcion = @respo) " +
                         $"GROUP BY tblReprocesoDetalle.Acepta, tblReprocesoDetalle.Responsable Having (tblReprocesoDetalle.Acepta = 1)";

            string connectionStringSID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;

            using (SqlConnection connectionSID = new SqlConnection(connectionStringSID))
            {
                connectionSID.Open();

                using (SqlCommand cmd = new SqlCommand(sql, connectionSID))
                {
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@respo", respo);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Enlazar el DataTable al DataGrid
            DatagridRespoCausa.DataSource = dataTable;
            DatagridRespoCausa.DataBind();


        }


        // Metodo para obtner el nombre del Mes teniendo la abreviatura 
        private string ObtenerNombreMes(string mesAbr)
        {
            // Diccionario de abreviaturas de meses y sus nombres completos
            Dictionary<string, string> meses = new Dictionary<string, string>
            {
                { "Jan", "Enero" },
                { "Feb", "Febrero" },
                { "Mar", "Marzo" },
                { "Apr", "Abril" },
                { "May", "Mayo" },
                { "Jun", "Junio" },
                { "Jul", "Julio" },
                { "Aug", "Agosto" },
                { "Sep", "Septiembre" },
                { "Oct", "Octubre" },
                { "Nov", "Noviembre" },
                { "Dec", "Diciembre" }
            };

            // Verificar si la abreviatura está en el diccionario y devolver el nombre del mes correspondiente
            if (meses.ContainsKey(mesAbr))
            {
                return meses[mesAbr];
            }
            else
            {
                return "";
            }
        }


        // Exportamos las estadisticas de reproceso a excel 
        protected void ExportarExcel_Click(object sender, EventArgs e)
        {

            if (DatagridCantidad.Items.Count == 0)
            {
                string mensajePersonalizado = "No hay Datos para la construcción del Excel";
                string urlRedireccion = "Consultas/Reproceso.aspx";
                Response.Redirect($"~/Formularios/SuccessMessage.aspx?message={HttpUtility.UrlEncode(mensajePersonalizado)}&redirectUrl={HttpUtility.UrlEncode(urlRedireccion)}");
            }
            else
            {

                // Construccion del excel 

                try
                {
                    // Crear un nuevo paquete de Excel
                    ExcelPackage excelPackage = new ExcelPackage();

                    // Agregar una hoja de trabajo al paquete
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Estadistica Reproceso");

                    //Altura Fila 1
                    worksheet.Row(1).Height = 60;

                    // Definir el título y el subtítulo
                    worksheet.Cells["B1:J1"].Merge = true;
                    worksheet.Cells["B1"].Value = "Ducon S.A.S";
                    worksheet.Cells["B1"].Style.Font.Size = 18;
                    worksheet.Cells["B1"].Style.Font.Bold = true;
                    worksheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells["B2:J2"].Merge = true;
                    worksheet.Cells["B2"].Value = "Estadística Reproceso Consolidado";
                    worksheet.Cells["B2"].Style.Font.Size = 12;
                    worksheet.Cells["B2"].Style.Font.Bold = true;
                    worksheet.Cells["B2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Escribir el encabezado de la tabla
                    int colIndex = 2;
                    foreach (DataGridColumn column in DatagridConsolidado.Columns)
                    {

                        worksheet.Cells[3, colIndex].Value = column.HeaderText;
                        worksheet.Cells[3, colIndex].Style.Font.Bold = true;
                        worksheet.Cells[3, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[3, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[3, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        colIndex++;
                    }

                    // Escribir los datos de la tabla
                    int rowIndex = 4;
                    foreach (DataGridItem item in DatagridConsolidado.Items)
                    {
                        colIndex = 2;
                        foreach (TableCell cell in item.Cells)
                        {

                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                            worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            colIndex++;
                        }
                        rowIndex++;
                    }

                    rowIndex = rowIndex + 2;


                    worksheet.Cells["B" + rowIndex + ":P" + rowIndex].Merge = true;
                    worksheet.Cells["B" + rowIndex].Value = "Cantidad de Reprocesos por Área";
                    worksheet.Cells["B" + rowIndex].Style.Font.Size = 12;
                    worksheet.Cells["B" + rowIndex].Style.Font.Bold = true;
                    worksheet.Cells["B" + rowIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    rowIndex++;
                    rowIndex++;

                    DataTable datos = ConsultarCantidadReproXArea();
                    // Escribir los encabezados del DataTable con un color de fondo diferente

                    colIndex = 2;
                    for (int i = 0; i < datos.Columns.Count; i++)
                    {
                        worksheet.Cells[rowIndex - 1, colIndex + i].Value = datos.Columns[i].ColumnName;
                        worksheet.Cells[rowIndex - 1, colIndex + i].Style.Font.Bold = true;
                        worksheet.Cells[rowIndex - 1, colIndex + i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowIndex - 1, colIndex + i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[rowIndex - 1, colIndex + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    // Escribir los datos de la tabla

                    foreach (DataRow row in datos.Rows)
                    {
                        colIndex = 2;
                        foreach (DataColumn column in datos.Columns)
                        {
                            worksheet.Cells[rowIndex, colIndex].Value = row[column];
                            worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            colIndex++;
                        }
                        rowIndex++;
                    }

                    rowIndex = rowIndex + 2;


                    worksheet.Cells["B" + rowIndex + ":P" + rowIndex].Merge = true;
                    worksheet.Cells["B" + rowIndex].Value = "Valor de Reprocesos por Área";
                    worksheet.Cells["B" + rowIndex].Style.Font.Size = 12;
                    worksheet.Cells["B" + rowIndex].Style.Font.Bold = true;
                    worksheet.Cells["B" + rowIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    rowIndex++;
                    colIndex = 2;
                    foreach (DataGridColumn column in DatagridPrecioArea.Columns)
                    {

                        worksheet.Cells[rowIndex, colIndex].Value = column.HeaderText;
                        worksheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                        worksheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        colIndex++;
                    }

                    // Escribir los datos de la tabla
                    rowIndex++; ;
                    foreach (DataGridItem item in DatagridPrecioArea.Items)
                    {
                        colIndex = 2;
                        foreach (TableCell cell in item.Cells)
                        {

                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                            worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            colIndex++;
                        }
                        rowIndex++;
                    }


                    rowIndex = rowIndex + 2;


                    worksheet.Cells["B" + rowIndex + ":F" + rowIndex].Merge = true;
                    worksheet.Cells["B" + rowIndex].Value = "Responsable Reproceso";
                    worksheet.Cells["B" + rowIndex].Style.Font.Size = 12;
                    worksheet.Cells["B" + rowIndex].Style.Font.Bold = true;
                    worksheet.Cells["B" + rowIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    rowIndex++;
                    colIndex = 2;
                    foreach (DataGridColumn column in DatagridResponsable.Columns)
                    {

                        worksheet.Cells[rowIndex, colIndex].Value = column.HeaderText;
                        worksheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                        worksheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        colIndex++;
                    }

                    // Escribir los datos de la tabla
                    rowIndex++; ;
                    foreach (DataGridItem item in DatagridResponsable.Items)
                    {
                        colIndex = 2;
                        foreach (TableCell cell in item.Cells)
                        {

                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                            worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            colIndex++;
                        }
                        rowIndex++;
                    }


                    rowIndex = rowIndex + 2;


                    worksheet.Cells["B" + rowIndex + ":F" + rowIndex].Merge = true;
                    worksheet.Cells["B" + rowIndex].Value = "Causas Reproceso";
                    worksheet.Cells["B" + rowIndex].Style.Font.Size = 12;
                    worksheet.Cells["B" + rowIndex].Style.Font.Bold = true;
                    worksheet.Cells["B" + rowIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    rowIndex++;
                    colIndex = 2;
                    foreach (DataGridColumn column in DataGridCausa.Columns)
                    {

                        worksheet.Cells[rowIndex, colIndex].Value = column.HeaderText;
                        worksheet.Cells[rowIndex, colIndex].Style.Font.Bold = true;
                        worksheet.Cells[rowIndex, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowIndex, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        colIndex++;
                    }

                    // Escribir los datos de la tabla
                    rowIndex++; ;
                    foreach (DataGridItem item in DataGridCausa.Items)
                    {
                        colIndex = 2;
                        foreach (TableCell cell in item.Cells)
                        {

                            worksheet.Cells[rowIndex, colIndex].Value = cell.Text;
                            worksheet.Cells[rowIndex, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            colIndex++;
                        }
                        rowIndex++;
                    }


                    worksheet.Cells.AutoFitColumns();

                    //*****************************************************************
                    // ************* Pendiente la imagen  y la redireccion ************
                    //*****************************************************************

                    // Guardar el archivo de Excel
                    string filePath = Path.GetTempFileName() + ".xlsx";
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    excelPackage.SaveAs(fileStream);
                    fileStream.Close();

                    // Descargar el archivo de Excel
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=EstadisticasReprocesos.xlsx");
                    Response.TransmitFile(filePath);
                    Response.End();


                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al exportar a Excel: " + ex.Message);
                }





            }


        }

       



        ////                  (((((((((((((((((((((((     METODO QUE CAMBIE EL CALCULO DE BUSQUEDA DE LAS ESTADISTICAS DE REPROCESO    )))))))))))))))))))))))

        //private int ObtenerCantidadReprocesoPorMes(SqlConnection connection, string mes, string ano, int idarea)
        //{
        //    string ssql = "select count(tblareareproceso.descripcion) as cantidad " +
        //                  "from tblreporteot inner join tblreprocesodetalle on tblreporteot.id_ot = tblreprocesodetalle.ot " +
        //                  "and tblreporteot.consecutivo_pedido = tblreprocesodetalle.pedido " +
        //                  "inner join tblareareproceso on tblreprocesodetalle.id_area = tblareareproceso.id_area " +
        //                  "where (tblreporteot.id_tipopedido = '8') and (tblareareproceso.id_area = @idarea) " +
        //                  "and (convert(char(3),tblreporteot.fecha_entrega_produccion, 0) = @mes) and (year(tblreporteot.fecha_entrega_produccion) = @anio ) ";

        //    using (var cmddetalle = new SqlCommand(ssql, connection))
        //    {
        //        cmddetalle.Parameters.AddWithValue("@mes", mes);
        //        cmddetalle.Parameters.AddWithValue("@anio", ano);
        //        cmddetalle.Parameters.AddWithValue("@idarea", idarea);

        //        object valor = cmddetalle.ExecuteScalar();
        //        int cantidad = (valor != DBNull.Value) ? Convert.ToInt32(valor) : 0;
        //        return cantidad;
        //    }
        //}

        ////                  (((((((((((((((((((((((       este metodo se necesitaria para poder que el nuevo calculo funcione             )))))))))))))))))))))))

        //private string ObtenerNombre_Mes(int mesnum)
        //{
        //    // diccionario de abreviaturas de meses y sus nombres completos
        //    Dictionary<int, string> meses = new Dictionary<int, string>
        //        {
        //            { 1, "jan" },
        //            { 2, "feb" },
        //            { 3, "mar" },
        //            { 4, "apr" },
        //            { 5, "may" },
        //            { 6, "jun" },
        //            { 7, "jul" },
        //            { 8, "aug" },
        //            { 9, "sep" },
        //            { 10, "oct" },
        //            { 11, "nov" },
        //            { 12, "dec" }
        //        };

        //    // verificar si el número de mes está en el diccionario y devolver el nombre del mes correspondiente
        //    if (meses.ContainsKey(mesnum))
        //    {
        //        return meses[mesnum];
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //// (((((((((((((((((((((((metodpo que cambie el calculo de busqueda de las estadisticas de reproceso    )))))))))))))))))))))))

        //private void ConsultarCantidadReproXArea()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Responsable", typeof(string));
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        dt.Columns.Add(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), typeof(string));
        //    }
        //    dt.Columns.Add("Total", typeof(int));
        //    dt.Columns.Add("Porcentaje", typeof(string));

        //    DataTable Areas = ObtenerNombreAreas();

        //    string connectionStringISID = ConfigurationManager.ConnectionStrings[CadenaConexionISID].ConnectionString;
        //    using (var connection = new SqlConnection(connectionStringISID))
        //    {
        //        connection.Open();

        //        int sumaTotal = 0; // Inicializar la suma total

        //        foreach (DataRow rowArea in Areas.Rows)
        //        {
        //            DataRow row = dt.NewRow();

        //            row["Responsable"] = rowArea["Descripcion"];
        //            int totalArea = 0;

        //            int idArea = (int)rowArea["Id_Area"];

        //            for (int mes = 1; mes <= 12; mes++)
        //            {
        //                string nombreMes = ObtenerNombre_Mes(mes);
        //                int valorMes = ObtenerCantidadReprocesoPorMes(connection, nombreMes, ddlAnioBusqueda.SelectedItem.Text, idArea);

        //                string MesString = DateTimeFormatInfo.CurrentInfo.GetMonthName(mes);
        //                row[MesString] = valorMes;
        //                totalArea += valorMes;
        //            }

        //            row["Total"] = totalArea;
        //            sumaTotal += totalArea; // Sumar al total general
        //            dt.Rows.Add(row);
        //        }

        //        // Calcular los porcentajes después de llenar el DataTable
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            double porcentaje = ((int)row["Total"] / (double)sumaTotal) * 100;
        //            row["Porcentaje"] = porcentaje.ToString("#0.##") + "%";
        //        }
        //    }

        //    // Enlazar DataTable a tu DataGrid
        //    DatagridCantidad.DataSource = dt;
        //    DatagridCantidad.DataBind();
        //}


        //Metodo para buscar las cantidades de Reproceso (ASI ESTÁ ACTUALEMNTE EL SISTEMA) 
    }
}