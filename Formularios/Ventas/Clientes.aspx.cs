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
using Button = System.Web.UI.WebControls.Button;
using Excel = Microsoft.Office.Interop.Excel;



namespace SISTEMA_INTEGRAL_DUCON.Formularios.Ventas
{
    public partial class Clientes : System.Web.UI.Page
    {
        // Variable de control de insercion o actualizacion de un cliente 
        private bool GuardarCliente = false;



        protected void Page_Load(object sender, EventArgs e)
        {

        }


        // Llenar el primerl el elemneto del dropdownlist Ciudad 
        protected void ddlCiudadX_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlCiudaX.Items.Insert(0, new ListItem("Seleccione", ""));
        }

        // Llenar el primerl el elemneto del dropdownlist Procedencia  
        protected void ddlProcedencia_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            ddlprocedencia.Items.Insert(0, new ListItem("", ""));
        }

      
        // logica de  Clientes 
        protected void DataGridCliente_ItemCommand(object source, DataGridCommandEventArgs e)

        {
            // Enviamos los Datos de la columna a los textBox y dropdownlist, listamos los contacto y los asesores
            if (e.CommandName == "VerCliente")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridCliente.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 

                string Nit = row.Cells[1].Text;
                string NombreCompañia = row.Cells[2].Text;
                string Asesor = row.Cells[3].Text;
                string Telefono = row.Cells[5].Text;
                string Direccion = row.Cells[6].Text;
                string Procedencia = row.Cells[7].Text;
                string CompartidoCon = row.Cells[8].Text;

                // Capturamos los datos que tiene en data grid en un arreglo 
                string[] campos = {
                  Nit,NombreCompañia, Asesor, Telefono, Direccion, Procedencia, CompartidoCon
                };

                // Recorremos  todos los campos y reemplazar &nbsp; por nulos o valores vacíos
                for (int i = 0; i < campos.Length; i++)
                {
                    campos[i] = campos[i].Replace("&nbsp;", null);
                }


                tbNit.Text = campos[0];
                tbNombreCliente.Text = campos[1];
                tbTelefono.Text = campos[3];
                tbDireccion.Text = campos[4];
                if (!string.IsNullOrEmpty(campos[5]))
                {
                    ListItem item = ddlprocedencia.Items.FindByValue(campos[5]);
                    if (item != null)
                    {
                        ddlprocedencia.ClearSelection();
                        item.Selected = true;
                    }
                    else
                    {
                        // El valor de campos[8] no está en la lista dejamos la lists vacia 
                        ddlprocedencia.ClearSelection(); // Deseleccionar en este caso
                    }
                }
                else
                {
                    ddlprocedencia.ClearSelection(); // Valor nulo, deseleccionar
                }

                tbCompartido.Text = campos[6];

                // Habilita el botón "Modificar"
                Button btnModificar = FindControl("Modificar") as Button;
                if (btnModificar != null)
                {
                    btnModificar.Enabled = true;
                }


                // Habilita el botón "Eliminar"
                Button btnEliminar = FindControl("Eliminar") as Button;
                if (btnEliminar != null)
                {
                    btnEliminar.Enabled = true;
                }


                //Invocamos el metodo para llenar los contactos del cliente 

                LlenarDataGridContacto(Nit);
                LlenarDataGridCotizacion(Nit);
                LlenarDataGridVisita(Nit);

                // Habilita el botón "Eliminar"
                Button btnNuevo = FindControl("btnNuevoContacto") as Button;
                if (btnNuevo != null)
                {
                    btnNuevo.Enabled = true;
                }

                Button btnCancelar = FindControl("btnCancelar") as Button;
                if (btnCancelar != null)
                {
                    btnCancelar.Enabled = true;
                }



            }



        }

        public void ConsultarClienteFecha(object sender, EventArgs e)
        {


        }

        protected void CrearExcel(object sender, EventArgs e)
        {
            try
            {
                // Crear una nueva instancia de Excel
                var excelApp = new Excel.Application();

                if (excelApp == null)
                {
                    Console.WriteLine("Excel no está instalado en esta máquina.");
                    return;
                }
                // Crear un nuevo libro y hoja de Excel
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // Agregar título a la tabla
                var tableTitle = "Clientes Nuevos (" + FechaI.Text + ") (" + FechaF.Text + ")";
                var titleRange = worksheet.Range["B1", "G1"];
                titleRange.Merge(); // Fusionar celdas para el título
                titleRange.Value = tableTitle;
                titleRange.Font.Size = 16;  // Tamaño de fuente
                titleRange.Font.Bold = true;  // Texto en negrita
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;  // Centrar el título
                titleRange.EntireRow.Font.Color = System.Drawing.Color.Black;  // Cambiar el color de fuente

                titleRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                titleRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;





                int rowIndexx = 3;


                // Escribir el encabezado de la tabla
                int colIndex = 1; // Columna 1 en Excel
                foreach (DataGridColumn column in DataGridClienteFecha.Columns)
                {
                    // Excluir la primera columna (LinkButton)
                    if (colIndex != 1)
                    {
                        // Escribe el valor del encabezado en la hoja de Excel
                        worksheet.Cells[rowIndexx - 1, colIndex] = column.HeaderText;
                        // Obtener el rango de la celda de encabezado
                        var headerCell = (Excel.Range)worksheet.Cells[rowIndexx - 1, colIndex];
                        headerCell.Font.Bold = true;  // Establecer el texto en negrita
                        headerCell.Interior.Color = System.Drawing.Color.LightGray;  // Cambiar el color de fondo

                        // Aplicar bordes a la celda de encabezado
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;


                    }
                    colIndex++;
                }

                foreach (DataGridItem item in DataGridClienteFecha.Items)
                {
                    colIndex = 1; // Comenzar en la columna 1 de Excel
                    foreach (TableCell cell in item.Cells)
                    {
                        // Excluir la primera columna (LinkButton)
                        if (colIndex != 1)
                        {

                            // Verificar si el valor de la celda es igual a "&nbsp;"
                            if (cell.Text != "&nbsp;")
                            {
                                // Escribe el valor de la celda en la hoja de Excel
                                worksheet.Cells[rowIndexx, colIndex] = cell.Text;
                            }

                            // Obtener el rango de la celda actual
                            var cellRange = (Excel.Range)worksheet.Cells[rowIndexx, colIndex];

                            // Aplicar bordes a la celda actual
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                            cellRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                        colIndex++;
                    }
                    rowIndexx++;
                }



                worksheet.Columns.AutoFit();
                // Mostrar la aplicación de Excel
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al exportar a Excel: " + ex.Message);
            }

        }

        protected void CancelarBot(object sender, EventArgs e)
        {
            // Habilita el botón "Modificar"
            Button btnModificar = FindControl("Modificar") as Button;
            if (btnModificar != null)
            {
                btnModificar.Enabled = false;
            }


            // Habilita el botón "Modificar"
            Button btnEliminar = FindControl("Eliminar") as Button;
            if (btnEliminar != null)
            {
                btnEliminar.Enabled = false;
            }

            // Habilita el botón "Modificar"
            Button bntGrabar = FindControl("Grabar") as Button;
            if (bntGrabar != null)
            {
                bntGrabar.Enabled = false;
            }


            // Habilita el botón "Modificar"
            Button btnNuevo = FindControl("Nuevo") as Button;
            if (btnNuevo != null)
            {
                btnNuevo.Enabled = true;
            }




        }

        protected void NuevoCliente(object sender, EventArgs e)
        {

            Session["GuardarCliente"] = true;

            // Habilita el botón "Modificar"
            Button btnModificar = FindControl("Modificar") as Button;
            if (btnModificar != null)
            {
                btnModificar.Enabled = false;
            }

            // Habilita el botón "Modificar"
            Button btnNuevo = FindControl("Nuevo") as Button;
            if (btnNuevo != null)
            {
                btnNuevo.Enabled = false;
            }

            // Habilita el botón "Modificar"
            Button btnEliminar = FindControl("Eliminar") as Button;
            if (btnEliminar != null)
            {
                btnEliminar.Enabled = false;
            }

            // Habilita el botón "Modificar"
            Button bntGrabar = FindControl("Grabar") as Button;
            if (bntGrabar != null)
            {
                bntGrabar.Enabled = true;
            }

           


        }

        protected void ModificarCliente(object sender, EventArgs e)
        {
            Session["GuardarCliente"] = false;



            // Invierte el valor de Enabled para el botón
            tbNombreContacto.ReadOnly = false;
            tbTelefonoContacto.ReadOnly = false;
            tbCelularContacto.ReadOnly = false;
            tbMailContacto.ReadOnly = false;

            tbBuscarContacto.ReadOnly = true;


            // Deshabilitar el botón "Nuevo"
            Button btnNuevoCliente = FindControl("Nuevo") as Button;
            if (btnNuevoCliente != null)
            {
                btnNuevoCliente.Enabled = false;
            }

            // Deshabilitar el botón "Nuevo"
            Button btnModificarCliente = FindControl("Modificar") as Button;
            if (btnNuevoCliente != null)
            {
                btnNuevoCliente.Enabled = false;
            }

            // Deshabilitar el botón "Nuevo"
            Button btnGrabar = FindControl("Grabar") as Button;
            if (btnGrabar != null)
            {
                btnGrabar.Enabled = true;
            }

            


        }


        protected void btn_GuardarCliente(object sender, EventArgs e)
        {
            bool guardarCliente = false;

            if (Session["GuardarCliente"] != null && Session["GuardarCliente"] is bool)
            {
                guardarCliente = (bool)Session["GuardarCliente"];
            }

            if (guardarCliente)
            {
                // Realizar inserción
            }
            else
            {
                // Realizar actualización
            }


        }

        protected void ConsultarCliente(object sender, EventArgs e)
        {

            // Validamos si el campo esta vacio para ejecurar un sqldatasource sino usamoos el otr 

            if (tbNitBuscar.Text != "" && tbNombreBuscar.Text == "")
            {
                DataGridCliente.DataSourceID = "ListarClientesXNit";
                DataGridCliente.DataBind();

            }
            else if (tbNombreBuscar.Text != "" && tbNitBuscar.Text == "")
            {
                DataGridCliente.DataSourceID = "ListarClientesXNombre";
                DataGridCliente.DataBind();
            }

            else
            {
                DataGridCliente.DataSourceID = "ListarClientes";
                DataGridCliente.DataBind();
            }


        }

        private void LlenarDataGridContacto(string idCliente)
        {
            // Realiza la conexión a la base de datos y ejecuta la consulta SQL con el parámetro
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT *
                        FROM tblClienteContacto
                        WHERE Id_Cliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Asigna el DataTable como origen de datos para el DataGrid
                    DataGridContacto.DataSource = dataTable;
                    DataGridContacto.DataBind();
                }
            }


        }

        private void LlenarDataGridCotizacion(string idCliente)
        {

            // Realiza la conexión a la base de datos y ejecuta la consulta SQL con el parámetro
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT c.Nombre + c.Apellidos AS NombreAsesor,
                                a.Cotización,
                                a.Fecha_Cotización,
                                a.Valor,
                                b.Descripción_Estado 
                        FROM tblCotización AS a 
                        INNER JOIN tblEstado_Cotización AS b ON b.Id_Estado = a.Estado
                        INNER JOIN tblAsesorComercial AS c ON c.CodigoAsesor = a.Asesor
                        WHERE Cliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    DataGridCotizacion.DataSource = dataTable;
                    DataGridCotizacion.DataBind();
                }
            }
        }

        private void LlenarDataGridVisita(string idCliente)
        {

            // Realiza la conexión a la base de datos y ejecuta la consulta SQL con el parámetro
            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT c.Nombre + c.Apellidos AS NombreAsesor,
                                a.FechaVisita,
                                d.NombreCausa,
                                b.NombreContacto 
                        FROM tblVisitaAsesor AS a
                        INNER JOIN tblClienteContacto AS b ON b.Id_ClienteContacto = a.Id_ClienteContacto
                        INNER JOIN tblAsesorComercial AS c ON c.Cedula = a.Asesor
                        INNER JOIN tblCausaVisita AS d ON d.Id_Causa = a.Causa
                        WHERE b.Id_Cliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    DataGridVisita.DataSource = dataTable;
                    DataGridVisita.DataBind();
                }
            }
        }

        

        // Logica del Tap Contactos de cliente 

        protected void DataGridContacto_ItemCommand(object source, DataGridCommandEventArgs e)

        {

            if (e.CommandName == "VerContacto")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGridContacto.Items[rowIndex];

                // capturamos los campos de la fila del datagrid 


                string NombreContacto = row.Cells[1].Text;
                string Telefono = row.Cells[2].Text;
                string Celular = row.Cells[3].Text;
                string Mail = row.Cells[4].Text;
                string Id_ContactoCliente = row.Cells[5].Text;

                // Capturamos los datos que tiene en data grid en un arreglo 
                string[] campos = {
                  NombreContacto,Telefono, Celular, Mail,Id_ContactoCliente
                };

                // Recorremos  todos los campos y reemplazar &nbsp; por nulos o valores vacíos
                for (int i = 0; i < campos.Length; i++)
                {
                    campos[i] = campos[i].Replace("&nbsp;", null);
                }


                tbNombreContacto.Text = campos[0];
                tbTelefonoContacto.Text = campos[1];
                tbCelularContacto.Text = campos[2];
                tbMailContacto.Text = campos[3];
                tbId_ContactoCliente.Text = campos[4];




            }




            // Habilita el botón "Modificar"
            Button btnModificar = FindControl("btnModificarContacto") as Button;
            if (btnModificar != null)
            {
                btnModificar.Enabled = true;
            }


        }

        protected void btnModificarContacto_Click(object sender, EventArgs e)
        {
            // Invierte el valor de Enabled para el botón
            tbNombreContacto.ReadOnly = false;
            tbTelefonoContacto.ReadOnly = false;
            tbCelularContacto.ReadOnly = false;
            tbMailContacto.ReadOnly = false;

            tbBuscarContacto.ReadOnly = true;


            // Deshabilitar el botón "Nuevo"
            Button btnNuevoCont = FindControl("btnNuevoContacto") as Button;
            if (btnNuevoCont != null)
            {
                btnNuevoCont.Enabled = false;
            }

            // Habilitar el botón "Grabar"
            Button btnGrabar = FindControl("btnGrabarContacto") as Button;
            if (btnGrabar != null)
            {
                btnGrabar.Enabled = true;
            }

            chkEstadoGuardar.Checked = false;



        }

        protected void btnCancelarContacto_Click(object sender, EventArgs e)
        {
            // Invierte el valor de Enabled para el botón
            tbNombreContacto.ReadOnly = true;
            tbTelefonoContacto.ReadOnly = true;
            tbCelularContacto.ReadOnly = true;
            tbMailContacto.ReadOnly = true;

            tbBuscarContacto.ReadOnly = false;


            tbNombreContacto.Text = "";
            tbTelefonoContacto.Text = "";
            tbCelularContacto.Text = "";
            tbMailContacto.Text = "";

            tbBuscarContacto.Text = "";

            // habilitar el botón "Nuevo"
            Button btnNuevoCont = FindControl("btnNuevoContacto") as Button;
            if (btnNuevoCont != null)
            {
                btnNuevoCont.Enabled = true;
            }

            // Deshabilitar el botón "Grabar"
            Button btnGrabar = FindControl("btnGrabarContacto") as Button;
            if (btnGrabar != null)
            {
                btnGrabar.Enabled = false;
            }

            // Deshabilitar el botón "Grabar"
            Button btnModificar = FindControl("btnModificarContacto") as Button;
            if (btnModificar != null)
            {
                btnModificar.Enabled = false;
            }

        }

        protected void btnNuevoContacto_Click(object sender, EventArgs e)
        {

            // Invierte el valor de Enabled para el botón
            tbNombreContacto.ReadOnly = false;
            tbTelefonoContacto.ReadOnly = false;
            tbCelularContacto.ReadOnly = false;
            tbMailContacto.ReadOnly = false;

            tbBuscarContacto.ReadOnly = true;


            //Limpiamos los campos del formulario 
            tbNombreContacto.Text = "";
            tbTelefonoContacto.Text = "";
            tbCelularContacto.Text = "";
            tbMailContacto.Text = "";
            tbId_ContactoCliente.Text = "";


            // habilitar el botón "Grabar"
            Button btnGrabar = FindControl("btnGrabarContacto") as Button;
            if (btnGrabar != null)
            {
                btnGrabar.Enabled = true;
            }

            // Deshabilitar el botón "Grabar"
            Button btnNuevoCont = FindControl("btnNuevoContacto") as Button;
            if (btnNuevoCont != null)
            {
                btnNuevoCont.Enabled = false;
            }

            chkEstadoGuardar.Checked = true;

        }

        protected void btnGuardarContacto_Click(object sender, EventArgs e)
        {

            bool estado = chkEstadoGuardar.Checked;

            if (estado == true)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO tblClientecontacto (NombreContacto, telefono, Mailcontacto, Id_cliente, Celular) " +
                                   "VALUES (@NombreContacto, @Telefono, @MailContacto, @IdCliente, @Celular)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreContacto", tbNombreContacto.Text);
                        command.Parameters.AddWithValue("@Telefono", tbTelefonoContacto.Text);
                        command.Parameters.AddWithValue("@MailContacto", tbMailContacto.Text);
                        command.Parameters.AddWithValue("@IdCliente", tbNit.Text);
                        command.Parameters.AddWithValue("@Celular", tbCelularContacto.Text);

                        command.ExecuteNonQuery(); // Ejecutar la inserción en la base de datos
                    }
                }
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Realizar actualización
                    string query = "UPDATE tblClienteContacto SET " +
                                   "NombreContacto = @NombreContacto, telefono = @Telefono, Mailcontacto = @MailContacto, Celular = @Celular " +
                                   "WHERE Id_Cliente = @IdCliente AND Id_Clientecontacto = @IdClienteContacto";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreContacto", tbNombreContacto.Text);
                        command.Parameters.AddWithValue("@Telefono", tbTelefonoContacto.Text);
                        command.Parameters.AddWithValue("@MailContacto", tbMailContacto.Text);
                        command.Parameters.AddWithValue("@Celular", tbCelularContacto.Text);
                        command.Parameters.AddWithValue("@IdCliente", tbNit.Text);
                        command.Parameters.AddWithValue("@IdClienteContacto", tbId_ContactoCliente.Text);

                        command.ExecuteNonQuery(); // Ejecutar la actualización en la base de datos
                    }
                }


            }


        }




    }
}