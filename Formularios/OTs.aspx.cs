using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Spreadsheet;
using Label = System.Windows.Forms.Label;



namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class OTs : System.Web.UI.Page
    {
        private string id;
        private string pedido;
        public double TotalObraMas = 0;
        public double TotalObraMenos = 0;
        public double TotalSaldo;
        string cadenaConexion = "Server=SRVDBAPPS;Database=BD_SIDSQL_PRUEBA;User Id=pcadmin;Password=password";
        
        //Validacion de Usuario Logueado
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                tbOT.Attributes.Add("onkeypress", "return handleEnter(event)");
                string usuariologueado = Session["usuariologueado"].ToString();

                if (Session["Id_OT"] != null && Session["pedido"] != null)
                {
                    id = Session["Id_OT"].ToString();
                    pedido = Session["pedido"].ToString();
                    Cargar_OTs();
                    Cargar_Plano();
                    Resumen_Plano();
                    Panel_Bajo();

                }

            }


        }
        //FIN Validacion de Usuario Logueado


        //CONTROL DEL TEXTBOX TBOT (NÚMERO DE LA OT)
        protected void tbOT_TextChanged(object sender, EventArgs e)
        {
            string id = tbOT.Text.Trim();
            Session["Id_OT"] = id;
            Session["pedido"] = 1;

            Cargar_OTs();
            Cargar_Plano();
            Resumen_Plano();
            Panel_Bajo();



            if (!string.IsNullOrEmpty(id))
            {

                string inputData = tbOT.Text;
                List<int> numeros = ObtenerNumerosDesdeLaBaseDeDatos(inputData);

                ddlNumbers.Items.Clear(); // Limpiar las opciones existentes

                foreach (int numero in numeros)
                {
                    ddlNumbers.Items.Add(numero.ToString());
                }

            }
        }
        //FIN CONTROL DEL TEXTBOX TBOT (NÚMERO DE LA OT)
       
       
        
        
        //CONTROL DEL COMBO QUE VALIDA LOS PEDIDOS QUE TIENE UNA OT
        protected void ddlNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pedido = ddlNumbers.SelectedValue;
            Session["pedido"] = pedido;



        }

        // FIN CONTROL DEL COMBO QUE VALIDA LOS PEDIDOS QUE TIENE UNA OT


        private List<int> ObtenerNumerosDesdeLaBaseDeDatos(string dato)
        {
            List<int> numeros = new List<int>();

            string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM tblOT WHERE Id_OT = @IdOT";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdOT", dato);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Supongamos que el número que deseas obtener está en la columna "Consecutivo_Pedido" de la tabla
                            int numero = reader.GetInt16(reader.GetOrdinal("Consecutivo_Pedido"));
                            numeros.Add(numero);
                        }
                    }
                }
            }

            return numeros;
        }

        public void Cargar_OTs()

        {


            id = Session["Id_OT"].ToString();
            pedido = Session["pedido"].ToString();



            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("ctaOT", sqlconectar)



            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
            cmd.Parameters.Add("@Con", SqlDbType.VarChar, 30).Value = pedido;




            SqlDataReader leer = cmd.ExecuteReader();



            if (leer.Read())
            {
                bool cerrada = leer.GetBoolean(leer.GetOrdinal("Cerrada")); // Variable para OTCerrada
                string valorPedidoBase = leer["PedidoBase"].ToString(); // Variable para Pedido Base
                bool valorExiste = false;

                //IF PARA OT CERRADA
                if (cerrada)
                {
                    // Si el campo "Cerrada" es true, muestra el label
                    msgOtCerrada.Visible = true;
                }
                else
                {
                    // Si el campo "Cerrada" es false, oculta el label
                    msgOtCerrada.Visible = false;
                }
                //FIN DE IF PARA OT CERRADA
                // -------------------------------

                //  Foreach PARA PEDEIDO BASE

                foreach (var item in cboPedidoBase.Items)
                {
                    if (item.ToString() == valorPedidoBase)
                    {
                        valorExiste = true;
                        break;
                    }
                }

                if (valorExiste)
                {
                    cboPedidoBase.Text = valorPedidoBase;
                }
                else
                {
                    string mensajeError = "HAY ERROR EN EL PEDIDO BASE.";
                    string script = "alert('" + mensajeError + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", script, true);
                }


                tbOT.Text = leer["Id_OT"].ToString();
                ddlNumbers.Text = leer["Consecutivo_Pedido"].ToString();
                tbZona.Text = leer["Zona"].ToString();
                dtacboTipoPedido.Text = leer["Descripcion_TipoPedido"].ToString();
                DtaCboTipoAprobacion.Text = leer["TipoAprobacion"].ToString();
                tbPedDepen.Text = leer["Consecutivo_Pedido"].ToString();
                tbObra.Text = leer["Nombre_Obra"].ToString();
                tbDir.Text = leer["Dirección"].ToString();
                tbContac.Text = leer["Persona_Receptora"].ToString();
                tbEmail.Text = leer["mail_Contacto"].ToString();
                tbRecibe.Text = leer["RecibeElPedido"].ToString();
                tbCiudad.Text = leer["Ciudad"].ToString();
                tbTel.Text = leer["TelDomicilio"].ToString();
                tbCel.Text = leer["CelularContacto"].ToString();
                tbPais.Text = leer["País"].ToString();
                DateTime Dato = (DateTime)leer["Fecha_Confirmacion_Venta"];
                DateTime Dato2 = (DateTime)leer["Fecha_Entrega_Produccion"];
                DateTime Dato3 = (DateTime)leer["Fecha_Empaque"];
                DateTime Dato4 = (DateTime)leer["Fecha_Real_Empaque"];
                tbVenta.Text = Dato.ToString("yyyy-MM-dd");
                dtpFechaEntregaDibujoDespiece.Text = Dato.ToString("yyyy-MM-dd");
                dtpFechaEntregaProduccion.Text = Dato2.ToString("yyyy-MM-dd");
                dtpEmpaque.Text = Dato3.ToString("yyyy-MM-dd");
                dtpRealEmpaque.Text = Dato4.ToString("yyyy-MM-dd");
                Observacion5Id.Value = leer["Observacion_Pedido"].ToString();
                Observacion1Id.Value = leer["Observacion_Dibujo"].ToString();
                tbSupervisor.Text = leer["Supervisor"].ToString();
                TextFabrica.Text = leer["FabricadoPor"].ToString();
                TextInstala.Text = leer["InstaladaPor"].ToString();

                txtcontacto.Text = leer["Persona_Receptora"].ToString();
                txtMail.Text = leer["mail_Contacto"].ToString();
                txtDireccion.Text = leer["Dirección"].ToString();
                txtMunicipio.Text = leer["Ciudad"].ToString();
                txtTelefono.Text = leer["TelDomicilio"].ToString();
                ObservacionCont.Value = leer["Observaciones_Contables"].ToString();
                txtCotizacion.Text = leer["Cotizacion"].ToString();
                txtOrdenCompra.Text = leer["OrdendeCompra"].ToString();
                txtAsesor.Text = leer["Codigo_Asesor"].ToString();
                TextTNegociacion.Value = leer["Forma_Pago"].ToString();
                tbBolsa.Text = leer["ValorBolsa"].ToString();
                txtValorPedido.Text = leer["ValorPedido"].ToString();
                txtDcto.Text = leer["Descuento"].ToString();
                txtVtte.Text = leer["ValorTteVia"].ToString();
                txtVvia.Text = leer["ValorViatico"].ToString();
                txtVenta.Text = leer["Precio_Venta"].ToString();
                msgOtCerrada.Text = "CERRADA EL:" + leer["Fecha_Cierre"].ToString();






                CalcularSaldo();




            }



            cmd.Connection.Close();

            //Codigo Harley llamado de datos de la cotización

            SqlCommand cotzita = new SqlCommand("sp_datoscotizacion", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };

            cotzita.Connection.Open();
            cotzita.Parameters.AddWithValue("@cotizacion", txtCotizacion.Text);
            cotzita.Parameters.AddWithValue("@NombreCliente", txtNombreEmp.Text);

            SqlDataReader drcot = cotzita.ExecuteReader();


            if (drcot.Read())
            {
                calcularDescuento();
                calcularGranTotal();
                txtNit.Text = drcot["Cliente"].ToString();
                txtValorSugerido.Text = drcot["ValorSugerido"].ToString();
                txtVcsd.Text = drcot["Valor"].ToString();
                txtVccd.Text = drcot["VCCD"].ToString();
                txtNombreEmp.Text = drcot["NombreCompañía"].ToString();
                txtComision.Text = drcot["DescuentoComision"].ToString();
                txtDiseño.Text = drcot["Diseño"].ToString();
                txtSaldo.Text = drcot["Saldo"].ToString();
                tbPlano.Text = drcot["Plano"].ToString();

            }
            cotzita.Connection.Close();


            string idOT = tbOT.Text; // Reemplaza esto con la forma de obtener idOT
            string consecutivoPedido = ddlNumbers.Text; // Reemplaza esto con la forma de obtener consecutivoPedido



            // Llama al método CargarPedidoBase para cargar los datos en el DropDownList.
            CargarPedidoBase(cboPedidoBase, idOT, consecutivoPedido);



        }
        protected void CargarPedidoBase(DropDownList cboPedidoBase, string idOT, string consecutivoPedido)
        {
            cboPedidoBase.Items.Clear(); // Limpiar el DropDownList antes de cargar los resultados.

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    // Consulta SQL con parámetros
                    string consulta = "SELECT PedidoBase FROM tblOT WHERE Id_OT = @IdOT AND ValorBolsa > 0 ";

                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        // Agregar parámetros
                        comando.Parameters.AddWithValue("@IdOT", idOT);
                        //    comando.Parameters.AddWithValue("@ConsecutivoPedido", consecutivoPedido);

                        // Ejecutar la consulta y cargar los resultados en el DropDownList
                        SqlDataReader reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            cboPedidoBase.Items.Add(reader["PedidoBase"].ToString());
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    // Manejar excepciones, por ejemplo, registrar el error.
                    Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                }
            }
        }

        protected void calcularDescuento()
        {
            // Harley variables
            double valorVenta = double.Parse(txtVenta.Text);
            double valorDescuento = double.Parse(txtDcto.Text);


            // Formulas
            double valorDescuentoCalculado = valorVenta * valorDescuento / 100;


            //  valor del descuento en el textbox
            txtDctoValor.Text = valorDescuentoCalculado.ToString();

        }
        protected void calcularGranTotal()
        {
            double valorVenta = double.Parse(txtVenta.Text);
            double valorDesPesos = double.Parse(txtDctoValor.Text);
            double ValorTransporte = double.Parse(txtVtte.Text);
            double ValorViatico = double.Parse(txtVvia.Text);

            //Formula
            double ValorGranTotal = valorVenta - valorDesPesos + ValorTransporte + ValorViatico;

            //valor del descuento en el textbox
            txtGtotal.Text = ValorGranTotal.ToString();
        }

        protected void CalcularSaldo()
        {

            TotalObraMas = double.Parse(tbBolsa.Text);
            TotalObraMenos = double.Parse(txtValorPedido.Text);


            if (TotalObraMenos > 0)
            {
                TotalObraMenos = TotalObraMas;

            }

            double TotalSaldo = TotalObraMas - TotalObraMenos;

            lblSaldoOT.Text = "Saldo:" + TotalSaldo;

        }
        //Logica de llamada de datos del plano.
        public void Cargar_Plano()
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("CtaPlano_OT", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
            cmd.Parameters.Add("@Conse", SqlDbType.VarChar, 30).Value = pedido;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtPlano.Text = dr["Plano"].ToString();
                txtCliente.Text = dr["Nombre_Cliente"].ToString();
                txtArea.Text = dr["Area"].ToString();
                txtAsesor.Text = dr["AsesorComercial"].ToString();
                txtDibuja.Text = dr["RealizadoPor"].ToString();
                txtBolsa.Text = dr["Bolsa"].ToString();
                txtContactoPlano.Text = dr["Contacto_Cliente"].ToString();
                txtAsesorPlano.Text = dr["AsesorComercial"].ToString();
            }

        }

        public void Resumen_Plano()
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("CtaOT", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@OT", SqlDbType.VarChar, 30).Value = id;
            cmd.Parameters.Add("@Con", SqlDbType.VarChar, 30).Value = pedido;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txResumen.Value = dr["ResumenObra"].ToString();


            }
        }


        public void Panel_Bajo()
        {

            //Conexion a la BD_SIDSQL y traemos el procedimiento almacenado
            string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
            SqlConnection sqlconectar = new SqlConnection(cn);
            SqlCommand cmd = new SqlCommand("cta_Plano_Paneles", sqlconectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@Plan", SqlDbType.VarChar, 30).Value = txtPlano.Text;

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtCantidad.Text = dr["Cantidad"].ToString();


            }
        }

    }
}

