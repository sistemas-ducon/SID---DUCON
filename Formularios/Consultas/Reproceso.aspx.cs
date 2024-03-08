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
                        // Obtener el valor de la variable de sesión
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
                 

                }
                else
                {
                    Response.Redirect("~/Formularios/Login.aspx");
                }


            }
        }

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
            Cerrar.Enabled = true;
            Cerrar.CssClass = "btn btn-sm shadow button-enabled";

            Agregar.Enabled = true;
            Agregar.CssClass = "btn btn-sm shadow button-enabled";

            Eliminar.Enabled = true;
            Eliminar.CssClass = "btn btn-sm shadow button-enabled";

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
            listaTextBoxes = new List<TextBox>  { tbOT,tbPedido,tbObra,tbCant,tbPq1,tbPq2,tbPq3,tbPq4,tbPq5,tbIdCausa   };

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

        private void limpiarCamposdetalle()
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

            // limpiamos los checkBox 

            chkcerrado.Checked = false;
            chkAcepta.Checked = false;
            chkPlanAccion.Checked = false;

            BotonesIniciales();

        }



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
                        precioObj = 0;
                    }

                    // Convierte el precio a entero
                    int precio = Convert.ToInt32(precioObj);

                    if (precio == 0)
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA721E");    //Naranja 
                        e.Item.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
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

            string Descripcion = row.Cells[12].Text;

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

            if(Id_OT != "" && pedido != "")
            {
                Adjuntar.Enabled = true;
                Adjuntar.CssClass = " btn btn-sm shadow button-enabled";
            }

            limpiarCamposdetalle();

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
            string Causa =  row.Cells[5].Text;
            string responsable = row.Cells[6].Text;
            string correccion = ReemplazarCaracteresEspeciales(row.Cells[7].Text).Replace("*nbsp;", "");
            string comentario = ReemplazarCaracteresEspeciales(row.Cells[8].Text).Replace("*nbsp;", "");
            string acepta = row.Cells[10].Text.Replace("&nbsp;","");
            string pq1 = row.Cells[11].Text.Replace("&nbsp;", "");
            string pq2 = row.Cells[12].Text.Replace("&nbsp;", "");
            string pq3 = row.Cells[13].Text.Replace("&nbsp;", "");
            string pq4 = row.Cells[14].Text.Replace("&nbsp;", "");
            string pq5 = row.Cells[15].Text.Replace("&nbsp;", "");
            string cerrado = row.Cells[18].Text;
            string redirigido = row.Cells[17].Text;

            // Se cargan Causas y Responsables 
            CargarCausa(Area);
            DataTable causas =  CargarCausa2(Area);
            CargarResponsable(Area); ;


            //  Se selecciona causa, rresponsable, elemento y area de consulta 
            if(Area != null && Area != "&nbsp;")
            {
                ddlArea1.SelectedValue = Area;
            }
            //  Se selecciona causa, rresponsable, elemento y area de consulta 
            if (elememto != null && elememto != "&nbsp;")
            {
                ddlElemento.SelectedValue = elememto;
            }
            if (Causa != null && Causa != "&nbsp;")
            {
                ddlCausaRaiz.SelectedValue = Causa;
            }
            if (responsable != null && responsable != "&nbsp;")
            {
                ddlResponsable.SelectedValue = responsable ;
            }

            // Pasamos los campos del datagrid a los textBox
            tbCantidad.Text = cantidad;
            tbPrecio.Text = precio;
            txCorreccion.InnerText = correccion;
            txComentario.InnerText = comentario;
            if(causas.Rows.Count > 0)
            {
                foreach (DataRow fila in causas.Rows)
                {
                    string causa =  fila["Descripcion"].ToString();

                    if(causa == Causa)
                    {
                        tbIdCausa.Text = fila["paIDPlan"].ToString();
                    }
                }
            }
             //vaidamos si el reproceso esta aceptado o no 
            if(acepta == "Si")
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
            if(cerrado == "Si")
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
                ddlResponsable.Items.Insert(0, new ListItem("", ""));
                ddlResponsable.DataBind();

                reader.Close();
            }
        }

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

        protected void chkAcepta_CheckedChanged(object sender, EventArgs e)
        {
            ValidarCambioAcepta();
        }

        private void ValidarCambioAcepta()
        {
            if(ddlElemento.SelectedItem.Value != "")
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
                }
            }
            
        }

        protected void btnConsultar1_Click(object sender, EventArgs e)
        {
           
        }


       
    }
}