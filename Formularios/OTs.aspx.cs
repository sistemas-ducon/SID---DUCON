using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


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
                    Cargar_OT();
                }

            }


        }

        protected void tbOT_TextChanged(object sender, EventArgs e)
        {
            string id = tbOT.Text.Trim();
            Session["Id_OT"] = id;
            Session["pedido"] = 1;





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
        protected void ddlNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pedido = ddlNumbers.SelectedValue;
            Session["pedido"] = pedido;



        }


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

        public void Cargar_OT()

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




            SqlDataReader dr = cmd.ExecuteReader();



            if (dr.Read())
            {
                bool cerrada = dr.GetBoolean(dr.GetOrdinal("Cerrada")); // Variable para OTCerrada
                string valorPedidoBase = dr["PedidoBase"].ToString(); // Variable para Pedido Base
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


                tbOT.Text = dr["Id_OT"].ToString();
                ddlNumbers.Text = dr["Consecutivo_Pedido"].ToString();
                tbZona.Text = dr["Zona"].ToString();
                dtacboTipoPedido.Text = dr["Descripcion_TipoPedido"].ToString();
                DtaCboTipoAprobacion.Text = dr["TipoAprobacion"].ToString();
                tbPedDepen.Text = dr["Consecutivo_Pedido"].ToString();
                tbObra.Text = dr["Nombre_Obra"].ToString();
                tbDir.Text = dr["Dirección"].ToString();
                tbContac.Text = dr["Persona_Receptora"].ToString();
                tbEmail.Text = dr["mail_Contacto"].ToString();
                tbRecibe.Text = dr["RecibeElPedido"].ToString();
                tbCiudad.Text = dr["Ciudad"].ToString();
                tbTel.Text = dr["TelDomicilio"].ToString();
                tbCel.Text = dr["CelularContacto"].ToString();
                tbPais.Text = dr["País"].ToString();
                DateTime Dato = (DateTime)dr["Fecha_Confirmacion_Venta"];
                DateTime Dato2 = (DateTime)dr["Fecha_Entrega_Produccion"];
                DateTime Dato3 = (DateTime)dr["Fecha_Empaque"];
                DateTime Dato4 = (DateTime)dr["Fecha_Real_Empaque"];
                tbVenta.Text = Dato.ToString("yyyy-MM-dd");
                dtpFechaEntregaDibujoDespiece.Text = Dato.ToString("yyyy-MM-dd");
                dtpFechaEntregaProduccion.Text = Dato2.ToString("yyyy-MM-dd");
                dtpEmpaque.Text = Dato3.ToString("yyyy-MM-dd");
                dtpRealEmpaque.Text = Dato4.ToString("yyyy-MM-dd");
                Observacion5Id.Value = dr["Observacion_Pedido"].ToString();
                Observacion1Id.Value = dr["Observacion_Dibujo"].ToString();
                tbSupervisor.Text = dr["Supervisor"].ToString();
                TextFabrica.Text = dr["FabricadoPor"].ToString();
                TextInstala.Text = dr["InstaladaPor"].ToString();

                txtcontacto.Text = dr["Persona_Receptora"].ToString();
                txtMail.Text = dr["mail_Contacto"].ToString();
                txtDireccion.Text = dr["Dirección"].ToString();
                txtMunicipio.Text = dr["Ciudad"].ToString();
                txtTelefono.Text = dr["TelDomicilio"].ToString();
                ObservacionCont.Value = dr["Observaciones_Contables"].ToString();
                txtCotizacion.Text = dr["Cotizacion"].ToString();
                txtOrdenCompra.Text = dr["OrdendeCompra"].ToString();
                txtAsesor.Text = dr["Codigo_Asesor"].ToString();
                TextTNegociacion.Value = dr["Forma_Pago"].ToString();
                tbBolsa.Text = dr["ValorBolsa"].ToString();
                txtValorPedido.Text = dr["ValorPedido"].ToString();
                txtDcto.Text = dr["Descuento"].ToString();
                txtVtte.Text = dr["ValorTteVia"].ToString();
                txtVvia.Text = dr["ValorViatico"].ToString();
                txtVenta.Text = dr["Precio_Venta"].ToString();
                msgOtCerrada.Text = "CERRADA EL:" + dr["Fecha_Cierre"].ToString();






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


    }
}