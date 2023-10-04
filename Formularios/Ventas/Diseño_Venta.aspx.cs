using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Web.Services;
using System.IO;


namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Diseño_Venta : System.Web.UI.Page
    {
       
        private bool isModalVisible = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

               

                ApplyButtonStyles();
                Session["ModificarEjecutado"] = false;

                //ViewState["Accion"] = "Insercion";

                TextIngDis.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TextUltAc.Text = DateTime.Now.ToString("yyyy-MM-dd");
                TextFecOkDib.Text = DateTime.Now.ToString("yyyy-MM-dd");

                DateTime fechaActual = DateTime.Now;

                // Agregar 5 días a la fecha actual
                DateTime fechaEntrega = fechaActual.AddDays(5);

                // Establecer el valor por defecto en el TextBox
                TextEntrega.Text = fechaEntrega.ToString("yyyy-MM-dd");

                DropDownList1.DataBind();

                DropDownList1.SelectedValue = "";

                habilitarbotones();
                DeshabilitarDivYContenido(miDiv);

                CheckBox22.Checked = isModalVisible;

                Nombreasesor();
                elementosllenosalcargarlapagina();
                CargarClienteYContacto();
            }
           


        }

        protected void BtnProgramar_Click(object sender, EventArgs e)
        {
            // Obtener el valor del label lblNumDise
            string numeroDiseño = lblNumDise.Text;

            // Realizar la actualización en la base de datos
            string connectionString = "Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"; // Reemplaza con tu cadena de conexión real

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE tblDiseño SET ProgramadoVentas = 1 WHERE Numero_Diseño = @NumeroDiseño";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NumeroDiseño", numeroDiseño);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalExitoso').modal('show');", true);
                        BtnProgramar.Enabled = false;
                        BtnProgramar.CssClass = "button-disabled";
                    }
                    else
                    {
                        BtnProgramar.Enabled = true;
                        BtnProgramar.CssClass = "button-enabled";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalError').modal('show');", true);
                    }
                }
            }
        }


        protected void SiButton_Click(object sender, EventArgs e)
        {
            // Limpiar el contenido del TextBox
            TextProyecto.Text = string.Empty;
            TextCliente.Text = string.Empty;
            TextDir.Text = string.Empty;
            TextPla.Text = string.Empty;
            TextContacto.Text = string.Empty;
            TextTel.Text = string.Empty;
            TextMail.Text = string.Empty;
            TextCiuPro.Text = string.Empty;
            TexHTot.Text = string.Empty;
            TextLin.Text = string.Empty;
            TextMos.Text = string.Empty;
            TextSup.Text = string.Empty;
            TextSop.Text = string.Empty;
            TextGav.Text = string.Empty;
            TextPan.Text = string.Empty;
            TextTapPie.Text = string.Empty;
            TextRep.Text = string.Empty;
            TextTipVid.Text = string.Empty;
            TextPant.Text = string.Empty;
            TextArch.Text = string.Empty;
            TextCoc.Text = string.Empty;
            TextEnt.Text = string.Empty;
            TextPuer.Text = string.Empty;
            TextObsVen.Value = string.Empty;
            TextObsDibDes.Value = string.Empty;
            TextSegPauDev.Value = string.Empty;
        }

        private void ApplyButtonStyles()
        {
            ApplyButtonStyle(NuevoDisBit);
            ApplyButtonStyle(Grabar);
            ApplyButtonStyle(Modificar);
            ApplyButtonStyle(DocBitacora);
            ApplyButtonStyle(RegresarDiseño);
            ApplyButtonStyle(AdicionarElemento);
            ApplyButtonStyle(ActualizarDiseno);
            ApplyButtonStyle(PausarDiseño);
            ApplyButtonStyle(Cancelar);
            ApplyButtonStyle(EliminarDiseño);
            ApplyButtonStyle(LinkButton1);
            ApplyButtonStyle(LinkButton2);
            ApplyButtonStyle(LinkButton3);
        }
        private void ApplyButtonStyle(WebControl button)
        {
            button.CssClass = button.Enabled ? "button-enabled" : "button-disabled";
        }

        protected override void OnInit(EventArgs e)
        {
            ViewState["Accion"] = "Insercion";
            base.OnInit(e);
        }

        private void CargarClienteYContacto()
        {
            if (!IsPostBack)
            {
                string IdCLiente = Session["Id_ClienteBD"]?.ToString();
                string IdContaco = Session["ID_ContactoBD"]?.ToString();

                if (!string.IsNullOrEmpty(IdCLiente) && !string.IsNullOrEmpty(IdContaco))
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "SELECT X.NombreCompañía, X.Dirección, Y.NombreContacto, Y.MailContacto " +
                                       "FROM tblCliente AS X " +
                                       "INNER JOIN tblClienteContacto AS Y ON Y.Id_Cliente = X.Id_Cliente " +
                                       "WHERE X.Id_Cliente = @ParametroCliente AND Y.Id_ClienteContacto = @ParametroClienteContacto";



                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ParametroCliente", IdCLiente);
                            command.Parameters.AddWithValue("@ParametroClienteContacto", IdContaco);

                            connection.Open();

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreCompañía")))
                                    {
                                        TextCliente.Text = reader["NombreCompañía"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("Dirección")))
                                    {
                                        TextDir.Text = reader["Dirección"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("NombreContacto")))
                                    {
                                        TextContacto.Text = reader["NombreContacto"].ToString();
                                    }
                                    if (!reader.IsDBNull(reader.GetOrdinal("MailContacto")))
                                    {
                                        TextMail.Text = reader["MailContacto"].ToString();
                                    }
                                }
                            }
                        }
                    }

                    Session.Remove("Id_ClienteBD");
                    Session.Remove("ID_ContactoBD");
                }

            }
        }

        protected void elementosllenosalcargarlapagina()
        {
            BtnProgramar.CssClass = "btn btn-warning shadow btn-sm";

            DateTime fechaActual = DateTime.Now;

           
 
            TextTel.Text = fechaActual.ToString("2889898");
            TextCel.Text = fechaActual.ToString("3002025400");

            ChecConDeCab.Checked = true;
            ChecPiso.Checked = true;
            ChecDiv.Checked = true;
            ChecCie.Checked = true;
            ChecCan.Checked = true;
            ChecBteEle.Checked = true;
            ChecBteSw.Checked = true;
            ChecSujPt.Checked = true;
            ChecAlCie.Checked = true;
            ChecPerRef.Checked = true;
            ChecGuaEsc.Checked = true;
            CheckBox16.Checked = true;
            ChecCot.Checked = true;
            ChecMailTer.Checked = true;
            ChecCotVia.Checked = true;
            CheckBox4.Checked = true;
            ChecMue.Checked = true;
        }

        protected void habilitarbotones()
        {
            NuevoDisBit.Enabled = true;
            ActualizarDiseno.Enabled = true;
            Cancelar.Enabled = true;

            NuevoDisBit.CssClass = "button-enabled";
            ActualizarDiseno.CssClass = "button-enabled";
            Cancelar.CssClass = "button-enabled";
        }

        private void DeshabilitarDivYContenido(System.Web.UI.Control container)
        {
            Button16.Enabled = false;
            Button16.CssClass = "form-control form-control-sm";

            Button15.Enabled = false;
            Button15.CssClass = "form-control form-control-sm";

            Button12.Enabled = false;
            Button12.CssClass = "form-control form-control-sm";

            Button13.Enabled = false;
            Button13.CssClass = "form-control form-control-sm";

            Button14.Enabled = false;
            Button14.CssClass = "form-control form-control-sm";

            Button11.Enabled = false;
            Button11.CssClass = "form-control form-control-sm";

           

            lblNumDise.Enabled = false;
           
            TextCliente.Enabled = false;
            TextCliente.CssClass = "form-control form-control-sm";

            Button1.Enabled = false;
           
            TextDir.Enabled = false;
            TextDir.CssClass = "form-control form-control-sm";

            lblDir.Enabled = false;
            lblDir.CssClass = "col-form-label-sm";

            TextDes.Enabled = false;
            TextDes.CssClass = "form-control form-control-sm";

            TextIngDis.Enabled = false;
            TextIngDis.CssClass = "form-control form-control-sm";

            lblDescuento.Enabled = false;
            lblDescuento.CssClass = "col-form-label-sm";

            lblIngDis.Enabled = false;
            lblIngDis.CssClass = "col-form-label-sm";

            lblUltAct.Enabled = false;
            lblUltAct.CssClass = "col-form-label-sm";

            TextUltAc.Enabled = false;
            TextUltAc.CssClass = "form-control form-control-sm";

            lblPro.Enabled = false;
            lblPro.CssClass = "col-form-label-sm";

            TextProyecto.Enabled = false;
            TextProyecto.CssClass = "form-control form-control-sm";

            lblPla.Enabled = false;
            lblPla.CssClass = "col-form-label-sm";

            lblUrg.Enabled = false;
            lblUrg.CssClass = "col-form-label-sm";

            ChecUrgent.Enabled = false;

            lblCotizar.Enabled = false;
            lblCotizar.CssClass = "col-form-label-sm";

            ChecCot.Enabled = false;

            lblEnt.Enabled = false;
            lblEnt.CssClass = "col-form-label-sm";

            TextEntrega.Enabled = false;
            TextEntrega.CssClass = "form-control form-control-sm";

            lblEntDib.Enabled = false;
            lblEntDib.CssClass = "col-form-label-sm";

            TextFecOkDib.Enabled = false;
            TextFecOkDib.CssClass = "form-control form-control-sm";

            lblZon.Enabled = false;
            lblZon.CssClass = "col-form-label-sm";

            TextZona.Enabled = false;
            TextZona.CssClass = "form-control form-control-sm";

            lblCon.Enabled = false;
            lblCon.CssClass = "col-form-label-sm";

            TextContacto.Enabled = false;
            TextContacto.CssClass = "form-control form-control-sm";

            lblTel.Enabled = false;
            lblNumDise.CssClass = "col-form-label-sm";

            TextTel.Enabled = false;
            TextTel.CssClass = "form-control form-control-sm";

            lblMaiTer.Enabled = false;
            lblMaiTer.CssClass = "col-form-label-sm";

            ChecMailTer.Enabled = false;

            lblCotVia.Enabled = false;
            lblCotVia.CssClass = "col-form-label-sm";


            ChecCotVia.Enabled = false;

            lblCotTte.Enabled = false;
            lblCotTte.CssClass = "col-form-label-sm";

            CheckBox4.Enabled = false;

            lblAse.Enabled = false;
            lblAse.CssClass = "col-form-label-sm";

            DropDownList1.Enabled = false;

            lblPre.Enabled = false;
            lblPre.CssClass = "col-form-label-sm";

            TextPre.Enabled = false;
            TextPre.CssClass = "form-control form-control-sm";

            lblCel.Enabled = false;
            lblCel.CssClass = "col-form-label-sm";

            TextCel.Enabled = false;
            TextCel.CssClass = "form-control form-control-sm";

            lblMai.Enabled = false;
            lblMai.CssClass = "col-form-label-sm";

            TextMail.Enabled = false;
            TextMail.CssClass = "form-control form-control-sm";

            lblCiuPro.Enabled = false;
            lblCiuPro.CssClass = "col-form-label-sm";

            TextCiuPro.Enabled = false;
            TextCiuPro.CssClass = "form-control form-control-sm";

            BtnProgramar.Enabled = false;

            lblConCab.Enabled = false;
            lblCiuPro.CssClass = "form-label";

            ChecConDeCab.Enabled = false;

            lblPis.Enabled = false;
            lblPis.CssClass = "col-form-label-sm g-5";

            ChecPiso.Enabled = false;

            lblDiv.Enabled = false;
            lblDiv.CssClass = "col-form-label-sm g-5";

            ChecDiv.Enabled = false;

            lblCie.Enabled = false;
            lblCie.CssClass = "col-form-label-sm";

            ChecCie.Enabled = false;

            lblCan.Enabled = false;
            lblCan.CssClass = "col-form-label-sm";

            ChecCan.Enabled = false;

            lblBteEle.Enabled = false;
            lblBteEle.CssClass = "col-form-label-sm";

            ChecBteEle.Enabled = false;

            lblBteSw.Enabled = false;
            lblBteSw.CssClass = "col-form-label-sm";

            ChecBteSw.Enabled = false;

            lblSujPt.Enabled = false;
            lblSujPt.CssClass = "col-form-label-sm";

            ChecSujPt.Enabled = false;

            lblAlCie.Enabled = false;
            lblAlCie.CssClass = "col-form-label-sm";

            ChecAlCie.Enabled = false;

            lblPerRef.Enabled = false;
            lblPerRef.CssClass = "col-form-label-sm";

            ChecPerRef.Enabled = false;

            lblGuaEsc.Enabled = false;
            lblGuaEsc.CssClass = "col-form-label-sm";

            ChecGuaEsc.Enabled = false;

            lblHTotCms.Enabled = false;
            lblHTotCms.CssClass = "col-form-label-sm";

            TexHTot.Enabled = false;

            lblEsyMat.Enabled = false;
            lblEsyMat.CssClass = "col-form-label-sm";

            lblLin.Enabled = false;
            lblLin.CssClass = "col-form-label-sm";

            TextLin.Enabled = false;
            TextLin.CssClass = "form-control form-control-sm";

            lblMos.Enabled = false;
            lblMos.CssClass = "col-form-label-sm";

            TextMos.Enabled = false;
            TextMos.CssClass = "form-control form-control-sm";

            lblSup.Enabled = false;
            lblSup.CssClass = "col-form-label-sm";

            TextSup.Enabled = false;
            TextSup.CssClass = "form-control form-control-sm";

            lblBal.Enabled = false;
            lblBal.CssClass = "col-form-label-sm";

            CheckBox16.Enabled = false;

            lblSop.Enabled = false;
            lblSop.CssClass = "col-form-label-sm";

            TextSop.Enabled = false;
            TextSop.CssClass = "form-control form-control-sm";

            lblGav.Enabled = false;
            lblGav.CssClass = "col-form-label-sm";

            TextGav.Enabled = false;
            TextGav.CssClass = "form-control form-control-sm";

            lblPan.Enabled = false;
            lblPan.CssClass = "col-form-label-sm";

            TextPan.Enabled = false;
            TextPan.CssClass = "form-control form-control-sm";

            lblTPie.Enabled = false;
            lblTPie.CssClass = "col-form-label-sm";

            TextTapPie.Enabled = false;
            TextTapPie.CssClass = "form-control form-control-sm";

            lblRep.Enabled = false;
            lblRep.CssClass = "col-form-label-sm";

            TextRep.Enabled = false;
            TextRep.CssClass = "form-control form-control-sm";

            lblTipVid.Enabled = false;
            lblTipVid.CssClass = "col-form-label-sm";

            TextTipVid.Enabled = false;
            TextTipVid.CssClass = "form-control form-control-sm";

            lblPant.Enabled = false;
            lblPant.CssClass = "col-form-label-sm";

            TextPant.Enabled = false;
            TextPant.CssClass = "form-control form-control-sm";

            lblArc.Enabled = false;
            lblArc.CssClass = "col-form-label-sm";

            TextArch.Enabled = false;
            TextArch.CssClass = "form-control form-control-sm";

            lblMue.Enabled = false;
            lblMue.CssClass = "col-form-label-sm";

            ChecMue.Enabled = false;

            lblCoc.Enabled = false;
            lblCoc.CssClass = "col-form-label-sm";

            TextCoc.Enabled = false;
            TextCoc.CssClass = "form-control form-control-sm";

            lblEntr.Enabled = false;
            lblEntr.CssClass = "col-form-label-sm";

            TextEnt.Enabled = false;
            TextEnt.CssClass = "form-control form-control-sm";

            lblPuer.Enabled = false;
            lblPuer.CssClass = "col-form-label-sm";

            TextPuer.Enabled = false;
            TextPuer.CssClass = "form-control form-control-sm";

            CheckBox18.Enabled = false;

            lblPrePpt.Enabled = false;
            lblPrePpt.CssClass = "col-form-label-sm";

            CheckBox19.Enabled = false;

            lblIma.Enabled = false;
            lblIma.CssClass = "col-form-label-sm";

            CheckBox20.Enabled = false;

            lblAcc.Enabled = false;
            lblAcc.CssClass = "col-form-label-sm";

            CheckBox21.Enabled = false;

            lblTieRea.Enabled = false;
            lblTieRea.CssClass = "col-form-label-sm";

            TextFec.Enabled = false;
            TextFec.CssClass = "form-control form-control-sm";

            TextFech.Enabled = false;
            TextFech.CssClass = "form-control form-control-sm";

            lblUbi.Enabled = false;
            TextUbi.CssClass = "col-form-label-sm";
        }

        protected void But_Click(object sender, EventArgs e)
        {
            // Verificar cuáles campos tienen datos y seleccionar el SqlDataSource correspondiente.
            if (!string.IsNullOrEmpty(TextFechDeIng.Text) && !string.IsNullOrEmpty(Texty.Text))
            {
                DataGrid4.DataSource = SqlDataSourceFecha;
            }
            else if (!string.IsNullOrEmpty(TextBox3.Text))
            {
                DataGrid4.DataSource = SqlDataSourceNumeroDis;
            }
            else if (!string.IsNullOrEmpty(TextBox5.Text))
            {
                DataGrid4.DataSource = SqlDataSourceNombreDiseño;
            }
            else if (!string.IsNullOrEmpty(TextBox4.Text))
            {
                DataGrid4.DataSource = SqlDataSourceCliente;
            }

            // Ejecutar la consulta y enlazar los datos al DataGrid.
            DataGrid4.DataBind();
        }


        protected void ddlCiudadX_DataBound(object sender, EventArgs e)
        {
            // Agregar el primer  elemento de los datagrid como "Seleccione"
            TextPre.Items.Insert(0, new ListItem("Seleccione", ""));
        }




        protected void NuevoDisBit_Click(object sender, EventArgs e)
        {
            bool lnkClieClicked = Session["lnkClieClicked"] as bool? ?? false;
            bool lnkClieeClicked = Session["lnkClieeClicked"] as bool? ?? false;

            if (lnkClieClicked || lnkClieeClicked)
            {
                // Mostrar el modal si se hizo clic en lnkClie o lnkCliee
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modall').modal('show');", true);
            }
            else
            {
                // Deshabilitar el botón "NuevoDisBit"
                NuevoDisBit.Enabled = false;

                Modificar.Enabled = false;

                // Aplicar clases CSS para botones deshabilitados
                NuevoDisBit.CssClass = "button-disabled";
                Modificar.CssClass = "button-disabled";

                // Habilitar el botón "Grabar"
                Grabar.Enabled = true;
                Grabar.CssClass = "button-enabled";

                // Deshabilitar el botón "ActualizarDiseno"
                ActualizarDiseno.Enabled = false;
                ActualizarDiseno.CssClass = "button-disabled";

                // Habilitar el div y su contenido
                HabilitarDivYContenido(miDiv);

                TextIngDis.Enabled = false;
                TextUltAc.Enabled = false;
                TextEntrega.Enabled = false;
                TextFecOkDib.Enabled = false;

               

                // Cambiar el color del Label lblCotizar
                lblCotizar.CssClass = "col-form-label-sm text-danger";
                lblCotizar.Font.Bold = true;


               
                
            }
            Session.Remove("lnkClieClicked");

        }

       
        private void HabilitarDivYContenido(System.Web.UI.Control container)
        {
            foreach (System.Web.UI.Control control in container.Controls)
            {
                if (control is System.Web.UI.WebControls.WebControl)
                {
                    // Si el control es un control web (como un botón), habilitarlo
                    ((System.Web.UI.WebControls.WebControl)control).Enabled = true;
                }

                // Si el control es un contenedor, llamar recursivamente a la función
                if (control.HasControls())
                {
                    HabilitarDivYContenido(control);
                }
            }
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            // Verifica si la variable de sesión "Modificado" está establecida como true.
            bool modificado = Session["Modificado"] as bool? ?? false;

            if (modificado)
            {
                // Si se hizo clic en Modificar antes, realiza las acciones necesarias para volver al estado anterior.
                NuevoDisBit.Enabled = false;
                NuevoDisBit.CssClass = "button-disabled";

                Modificar.Enabled = true;
                Modificar.CssClass = "button-enabled";

                ActualizarDiseno.Enabled = false;
                ActualizarDiseno.CssClass = "button-disabled";


                // Resto de las acciones para volver al estado anterior...
            }
            else
            {
                // Si no se hizo clic en Modificar antes, simplemente restablece todo como estaba antes de Cancelar.
                Grabar.Enabled = false;
                Grabar.CssClass = "button-disabled";

                NuevoDisBit.Enabled = true;
                NuevoDisBit.CssClass = "button-enabled";

                ActualizarDiseno.Enabled = true;
                ActualizarDiseno.CssClass = "button-enabled";

                // Resto de las acciones para cancelar...
            }

            // Limpia la variable de sesión "Modificado" después de utilizarla.
            Session["Modificado"] = false;

            // Deshabilitar el botón "Grabar"
            Grabar.Enabled = false;
            Grabar.CssClass = "button-disabled";

            // Habilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = true;

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            NuevoDisBit.CssClass = "button-enabled";

            // Ocultar el div y su contenido
            DeshabilitarDivYContenido(miDiv);

            lblNumDise.Text = "Número";
            lblCotizar.CssClass = "col-form-label-sm text-dark";
            lblCotizar.Font.Bold = false;
        }

        protected void lnkClie_Click(object sender, EventArgs e)
        {
          

            Grabar.Enabled = false;
            Grabar.CssClass = "button-disabled";

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            ActualizarDiseno.CssClass = "button-enabled";

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;
            Modificar.CssClass = "button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "button-enabled";

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;
            AdicionarElemento.CssClass = "button-enabled";

            BtnProgramar.CssClass = "btn-outline-dark btn btn-white shadow btn-sm";

            DeshabilitarDivYContenido(miDiv);

           

            ;
        }

        protected void lnkCliee_Click(object sender, EventArgs e)
        {
            Session["lnkClieeClicked"] = true;

            Grabar.Enabled = false;
            Grabar.CssClass = "button-disabled";

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;
            ActualizarDiseno.CssClass = "button-enabled";

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;
            Modificar.CssClass = "button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "button-enabled";

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;
            AdicionarElemento.CssClass = "button-enabled";

            BtnProgramar.CssClass = "btn-outline-dark btn btn-white shadow btn-sm";

            DeshabilitarDivYContenido(miDiv);



            ;
        }
        protected void Nombreasesor()
        {
            if (Session["usuariologueado"] != null)
            {
                string usuariologueado = Session["usuariologueado"].ToString();

                // Realizar la conexión a la base de datos y la consulta para obtener el nombre y apellido del usuario
                string connectionString = "Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Cedula, Zona, Nombre, Apellidos FROM tblEmpleado WHERE Login = @nombreUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", usuariologueado);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string Cedula = reader["Cedula"].ToString();
                            string zona = reader["Zona"].ToString();
                            string nombre = reader["Nombre"].ToString();
                            string apellidos = reader["Apellidos"].ToString();
                            DropDownList1.SelectedValue = Cedula; // Asignar el nombre y apellidos al TextBox
                            TextZona.Text = zona;
                        }

                    }
                }

            }
            else
            {
                Response.Redirect("/Formularios/Login.aspx");
            }
        }

       
        protected void Modificar_Click(object sender, EventArgs e)
        {
         

            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;
            Grabar.CssClass = "button-disabled";

            Modificar.Enabled = false;
            Modificar.CssClass = "button-disabled";

            ActualizarDiseno.Enabled = false;
            ActualizarDiseno.CssClass = "button-disabled";

            // Habilitar el botón "Grabar"
            Grabar.Enabled = true;
            Grabar.CssClass = "button-enabled";

            DocBitacora.Enabled = true;
            DocBitacora.CssClass = "button-enabled";

            // Habilitar el div y su contenido
            HabilitarDivYContenido(miDiv);

            TextIngDis.Enabled = false;
            TextUltAc.Enabled = false;
           
            TextFecOkDib.Enabled = false;

            Button9.Enabled = false;
            Button10.Enabled = false;

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;


            Session["ModificarEjecutado"] = true;
        }

        protected void btnInsertar_Click(object sender, EventArgs e)
        {
            if (Session["ModificarEjecutado"] != null && (bool)Session["ModificarEjecutado"])
            {

                Session["ModificarEjecutado"] = false;

                // Crear la consulta SQL para la actualización
                string query = "update tblDiseño set " +
                    "Nombre_Diseño = @Nombre_Diseño," +
                    " id_CiudadProyecto = @id_CiudadProyecto," +
                    "Mail = @Mail, Celular = @Celular, Cotizartransporte = @Cotizartransporte, CotizarViaticos = @CotizarViaticos," +
                    "MailTerminado = @MailTerminado, Telefono = @Telefono, Contacto = @Contacto, Zona = @Zona, FechaDibujoOK = @FechaDibujoOK," +
                    "Fecha_Programada_Entrega = @Fecha_Programada_Entrega, PasarACotizar = @PasarACotizar, Urgente = @Urgente, Plano = @Plano," +
                    "UltimaActivacion = @UltimaActivacion, Fecha_Ingreso = @Fecha_Ingreso, Asesor = @Asesor, Cliente = @Cliente, Direccion = @Direccion," +
                    "Descuento = @Descuento, ConduccionCablesPiso = @ConduccionCablesPiso, ConduccionCablesDivision = @ConduccionCablesDivision," +
                    "ConduccionCablesCielo = @ConduccionCablesCielo, ConduccionCablesCanaleta = @ConduccionCablesCanaleta, BajantesElectricos = @BajantesElectricos," +
                    "Bajantesswitches = @Bajantesswitches, SujecionCielo = @SujecionCielo, PerfilRefuerzo = @PerfilRefuerzo, GuardaEscobas = @GuardaEscobas," +
                    "Linea = @Linea, TipoMostrador = @TipoMostrador, AcabadoSuperficie = @AcabadoSuperficie, BalanceSuperficies = @BalanceSuperficies, TipoSoporte = @TipoSoporte," +
                    "TipoGaveta = @TipoGaveta, AcabadoPaneles = @AcabadoPaneles, TipoTapaPierna = @TipoTapaPierna, TipoRepisa = @TipoRepisa, TipodePantalla = @TipodePantalla," +
                    "TipoArchivador = @TipoArchivador, MuebleEntrepano = @MuebleEntrepano, MueblePuertas = @MueblePuertas, Observaciones_Ventas = @Observaciones_Ventas," +
                    "SeguimientoPausa = @SeguimientoPausa, SC_Presentacionppt = @SC_Presentacionppt, SC_Imagenes = @SC_Imagenes, SC_Accesorios = @SC_Accesorios," +
                    "SC_Tiemporeal = @SC_Tiemporeal, SC_Fecha = @SC_Fecha, SC_Ubicacion = @SC_Ubicacion " +
                    " where Numero_Diseño = @Numero_Diseño";

                using (SqlConnection connection = new SqlConnection("Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"))
                {

                    string Proyect = TextProyecto.Text;
                    string NumDis = lblNumDise.Text;

                    string cliente = TextCliente.Text;
                    string direccion = TextDir.Text;
                    string descuento = TextDes.Text;
                    string nombreDiseño = TextProyecto.Text;
                    string fechaIngreso = TextIngDis.Text;

                    string Asesor = DropDownList1.SelectedItem.Text;
                    string UltAct = TextUltAc.Text;

                    string Plano = TextPla.Text;
                    bool Urgente = ChecUrgent.Checked;
                    bool Cotizar = ChecCot.Checked;
                    string Entrega = TextEntrega.Text;
                    string FecOkDib = TextFecOkDib.Text;
                    string Zona = TextZona.Text;
                    string Contacto = TextContacto.Text;
                    string Tel = TextTel.Text;
                    bool MaiTer = ChecMailTer.Checked;
                    bool CotVia = ChecCotVia.Checked;
                    bool CotTte = CheckBox4.Checked;
                    string Cel = TextCel.Text;
                    string Mail = TextMail.Text;
                    string CiuPro = TextCiuPro.Text;
                    bool Pis = ChecPiso.Checked;
                    bool Div = ChecDiv.Checked;
                    bool Cie = ChecCie.Checked;
                    bool Can = ChecCan.Checked;
                    bool BteEle = ChecBteEle.Checked;
                    bool BteSw = ChecBteSw.Checked;
                    bool SujPt = ChecSujPt.Checked;
                    bool PerRef = ChecPerRef.Checked;
                    bool GuaEsc = ChecGuaEsc.Checked;
                    string Lin = TextLin.Text;
                    string Most = TextMos.Text;
                    string Superficies = TextSup.Text;
                    string Balance = CheckBox16.Text;
                    string Soporte = TextSop.Text;
                    string Gaveta = TextGav.Text;
                    string Paneles = TextPan.Text;
                    string TapPier = TextTapPie.Text;
                    string Repisa = TextRep.Text;
                    string Pantalla = TextTipVid.Text;
                    string Arch = TextArch.Text;
                    string Entrepano = TextEnt.Text;
                    string Puertas = TextPuer.Text;
                    string ObservacionVenta = TextObsVen.InnerText;
                    string ObservacionDibDes = TextObsDibDes.InnerText;
                    string ObservacionPauDev = TextSegPauDev.InnerText;
                    bool PresentacionPtt = CheckBox18.Checked;
                    bool Imagenes = CheckBox19.Checked;
                    bool Accesorios = CheckBox20.Checked;
                    bool TiempoReal = CheckBox21.Checked;
                    string SCFecha = TextFec.Text;
                    string ubicacionSC = TextUbi.Text;
                    // Abrir la conexión
                    connection.Open();


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar valores a los parámetros de actualización
                        command.Parameters.AddWithValue("@Nombre_Diseño", Proyect);
                        command.Parameters.AddWithValue("@Numero_Diseño", NumDis);

                        command.Parameters.AddWithValue("@id_CiudadProyecto", CiuPro);

                        command.Parameters.AddWithValue("@Mail", Mail);
                        command.Parameters.AddWithValue("@Celular", Cel);
                        command.Parameters.AddWithValue("@Cotizartransporte", CotTte);
                        command.Parameters.AddWithValue("@CotizarViaticos", CotVia);
                        command.Parameters.AddWithValue("@MailTerminado", MaiTer);
                        command.Parameters.AddWithValue("@Telefono", Tel);
                        command.Parameters.AddWithValue("@Contacto", Contacto);
                        command.Parameters.AddWithValue("@Zona", Zona);
                        command.Parameters.AddWithValue("@FechaDibujoOK", FecOkDib);
                        command.Parameters.AddWithValue("@Fecha_Programada_Entrega", Entrega);
                        command.Parameters.AddWithValue("@PasarACotizar", Cotizar);
                        command.Parameters.AddWithValue("@Urgente", Urgente);
                        command.Parameters.AddWithValue("@Plano", Plano);

                        command.Parameters.AddWithValue("@UltimaActivacion", UltAct);
                        command.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);
                        command.Parameters.AddWithValue("@Asesor", Asesor);
                        command.Parameters.AddWithValue("@Cliente", cliente);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@Descuento", descuento);
                        command.Parameters.AddWithValue("@ConduccionCablesPiso", Pis);
                        command.Parameters.AddWithValue("@ConduccionCablesDivision", Div);
                        command.Parameters.AddWithValue("@ConduccionCablesCielo", Cie);
                        command.Parameters.AddWithValue("@ConduccionCablesCanaleta", Can);
                        command.Parameters.AddWithValue("@BajantesElectricos", BteEle);
                        command.Parameters.AddWithValue("@Bajantesswitches", BteSw);
                        command.Parameters.AddWithValue("@SujecionCielo", SujPt);
                        command.Parameters.AddWithValue("@PerfilRefuerzo", PerRef);
                        command.Parameters.AddWithValue("@GuardaEscobas", GuaEsc);
                        command.Parameters.AddWithValue("@Linea", Lin);
                        command.Parameters.AddWithValue("@TipoMostrador", Most);
                        command.Parameters.AddWithValue("@AcabadoSuperficie", Superficies);
                        command.Parameters.AddWithValue("@BalanceSuperficies", Balance);
                        command.Parameters.AddWithValue("@TipoSoporte", Soporte);
                        command.Parameters.AddWithValue("@TipoGaveta", Gaveta);
                        command.Parameters.AddWithValue("@AcabadoPaneles", Paneles);
                        command.Parameters.AddWithValue("@TipoTapaPierna", TapPier);
                        command.Parameters.AddWithValue("@TipoRepisa", Repisa);
                        command.Parameters.AddWithValue("@TipodePantalla", Pantalla);
                        command.Parameters.AddWithValue("@TipoArchivador", Arch);
                        command.Parameters.AddWithValue("@MuebleEntrepano", Entrepano);
                        command.Parameters.AddWithValue("@MueblePuertas", Puertas);
                        command.Parameters.AddWithValue("@Observaciones_Ventas", ObservacionVenta);
                        command.Parameters.AddWithValue("@Observaciones_Diseño", ObservacionDibDes);
                        command.Parameters.AddWithValue("@SeguimientoPausa", ObservacionPauDev);
                        command.Parameters.AddWithValue("@SC_Presentacionppt", PresentacionPtt);
                        command.Parameters.AddWithValue("@SC_Imagenes", Imagenes);
                        command.Parameters.AddWithValue("@SC_Accesorios", Accesorios);
                        command.Parameters.AddWithValue("@SC_Tiemporeal", TiempoReal);
                        command.Parameters.AddWithValue("@SC_Fecha", SCFecha);
                        command.Parameters.AddWithValue("@SC_Ubicacion", ubicacionSC);


                        // Ejecutar la consulta SQL de actualización
                        int rowsAffected = command.ExecuteNonQuery();

                        // Comprobar si se actualizó al menos una fila
                        if (rowsAffected > 0)
                        {
                            // Éxito: los datos se actualizaron correctamente
                            Response.Write("Datos actualizados correctamente.");
                        }
                        else
                        {
                            // Error: no se actualizó ninguna fila (puede deberse a un número de diseño no válido)
                            Response.Write("No se encontró ningún diseño para actualizar.");
                        }
                    }
                }


            }

            else
            {
                // Obtener la cadena de conexión desde Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;

                // Obtener los valores de los TextBox
                string cliente = TextCliente.Text;
                string direccion = TextDir.Text;
                string descuento = TextDes.Text;
                string nombreDiseño = TextProyecto.Text;
                string fechaIngreso = TextIngDis.Text;

                string Asesor = DropDownList1.SelectedItem.Text;
                string UltAct = TextUltAc.Text;
                string Proyecto = TextProyecto.Text;
                string Plano = TextPla.Text;
                bool Urgente = ChecUrgent.Checked;
                bool Cotizar = ChecCot.Checked;
                string Entrega = TextEntrega.Text;
                string FecOkDib = TextFecOkDib.Text;
                string Zona = TextZona.Text;
                string Contacto = TextContacto.Text;
                string Tel = TextTel.Text;
                bool MaiTer = ChecMailTer.Checked;
                bool CotVia = ChecCotVia.Checked;
                bool CotTte = CheckBox4.Checked;
                string Cel = TextCel.Text;
                string Mail = TextMail.Text;
                string CiuPro = TextCiuPro.Text;
                bool Pis = ChecPiso.Checked;
                bool Div = ChecDiv.Checked;
                bool Cie = ChecCie.Checked;
                bool Can = ChecCan.Checked;
                bool BteEle = ChecBteEle.Checked;
                bool BteSw = ChecBteSw.Checked;
                bool SujPt = ChecSujPt.Checked;
                bool PerRef = ChecPerRef.Checked;
                bool GuaEsc = ChecGuaEsc.Checked;
                string Lin = TextLin.Text;
                string Most = TextMos.Text;
                string Superficies = TextSup.Text;
                string Balance = CheckBox16.Text;
                string Soporte = TextSop.Text;
                string Gaveta = TextGav.Text;
                string Paneles = TextPan.Text;
                string TapPier = TextTapPie.Text;
                string Repisa = TextRep.Text;
                string Pantalla = TextTipVid.Text;
                string Arch = TextArch.Text;
                string Entrepano = TextEnt.Text;
                string Puertas = TextPuer.Text;
                string ObservacionVenta = TextObsVen.InnerText;
                string ObservacionDibDes = TextObsDibDes.InnerText;
                string ObservacionPauDev = TextSegPauDev.InnerText;
                bool PresentacionPtt = CheckBox18.Checked;
                bool Imagenes = CheckBox19.Checked;
                bool Accesorios = CheckBox20.Checked;
                bool TiempoReal = CheckBox21.Checked;
                string SCFecha = TextFec.Text;
                string ubicacionSC = TextUbi.Text;

                // Crear una consulta SQL para obtener el valor máximo actual de Numero_Diseño y calcular el siguiente número consecutivo
                string queryGetMaxNumeroDiseño = "SELECT MAX(Numero_Diseño) FROM tbldiseño";
                int numerodiseño = 0;

                // Crear una conexión SQL
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Crear un comando SQL para obtener el valor máximo actual de Numero_Diseño
                    using (SqlCommand commandGetMaxNumeroDiseño = new SqlCommand(queryGetMaxNumeroDiseño, connection))
                    {
                        // Ejecutar la consulta para obtener el valor máximo actual
                        object result = commandGetMaxNumeroDiseño.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            // Si hay un valor máximo actual, incrementarlo en 1
                            numerodiseño = Convert.ToInt32(result) + 1;
                        }
                        else
                        {
                            // Si no hay registros en la tabla, comenzar desde 1
                            numerodiseño = 1;
                        }
                    }
                }

                // Crear la consulta SQL para la inserción
                string query = "INSERT INTO tbldiseño (id_CiudadProyecto, Numero_Diseño, Nombre_Diseño, Asesor, Fecha_Ingreso, Fecha_Programada_Entrega, UltimaActivacion, Contacto, Telefono, Mail, Celular, Zona, Cotizartransporte, CotizarViaticos, MailTerminado, FechaDibujoOK, PasarACotizar, Urgente, Plano, Cliente, Direccion, Descuento, ConduccionCablesPiso, ConduccionCablesDivision, ConduccionCablesCielo, ConduccionCablesCanaleta, BajantesElectricos, Bajantesswitches, SujecionCielo, PerfilRefuerzo, GuardaEscobas, Linea, TipoMostrador, AcabadoSuperficie, BalanceSuperficies, TipoSoporte, TipoGaveta, AcabadoPaneles, TipoTapaPierna, TipoRepisa, TipodePantalla, TipoArchivador, MuebleEntrepano, MueblePuertas, Observaciones_Ventas, Observaciones_Diseño, SeguimientoPausa, SC_Presentacionppt, SC_Imagenes, SC_Accesorios, SC_Tiemporeal, SC_Fecha, SC_Ubicacion)" +
                    " VALUES (@id_CiudadProyecto, @Numero_Diseño, @Nombre_Diseño, @Asesor, @Fecha_Ingreso, @Fecha_Programada_Entrega, @UltimaActivacion, @Contacto, @Telefono, @Mail, @Celular, @Zona, @Cotizartransporte, @CotizarViaticos, @MailTerminado, @FechaDibujoOK, @PasarACotizar, @Urgente, @Plano, @Cliente, @Direccion, @Descuento, @ConduccionCablesPiso, @ConduccionCablesDivision, @ConduccionCablesCielo, @ConduccionCablesCanaleta, @BajantesElectricos, @Bajantesswitches, @SujecionCielo, @PerfilRefuerzo, @GuardaEscobas, @Linea, @TipoMostrador, @AcabadoSuperficie, @BalanceSuperficies, @TipoSoporte, @TipoGaveta, @AcabadoPaneles, @TipoTapaPierna, @TipoRepisa, @TipodePantalla, @TipoArchivador, @MuebleEntrepano, @MueblePuertas, @Observaciones_Ventas, @Observaciones_Diseño, @SeguimientoPausa, @SC_Presentacionppt, @SC_Imagenes, @SC_Accesorios, @SC_Tiemporeal, @SC_Fecha, @SC_Ubicacion)";


                // Crear una conexión SQL
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Crear un comando SQL
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar valores a los parámetros
                        command.Parameters.AddWithValue("@id_CiudadProyecto", CiuPro);
                        command.Parameters.AddWithValue("@Numero_Diseño", numerodiseño);
                        command.Parameters.AddWithValue("@Mail", Mail);
                        command.Parameters.AddWithValue("@Celular", Cel);
                        command.Parameters.AddWithValue("@Cotizartransporte", CotTte);
                        command.Parameters.AddWithValue("@CotizarViaticos", CotVia);
                        command.Parameters.AddWithValue("@MailTerminado", MaiTer);
                        command.Parameters.AddWithValue("@Telefono", Tel);
                        command.Parameters.AddWithValue("@Contacto", Contacto);
                        command.Parameters.AddWithValue("@Zona", Zona);
                        command.Parameters.AddWithValue("@FechaDibujoOK", FecOkDib);
                        command.Parameters.AddWithValue("@Fecha_Programada_Entrega", Entrega);
                        command.Parameters.AddWithValue("@PasarACotizar", Cotizar);
                        command.Parameters.AddWithValue("@Urgente", Urgente);
                        command.Parameters.AddWithValue("@Plano", Plano);
                        command.Parameters.AddWithValue("@Nombre_Diseño", Proyecto);
                        command.Parameters.AddWithValue("@UltimaActivacion", UltAct);
                        command.Parameters.AddWithValue("@Fecha_Ingreso", fechaIngreso);
                        command.Parameters.AddWithValue("@Asesor", Asesor);
                        command.Parameters.AddWithValue("@Cliente", cliente);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@Descuento", descuento);
                        command.Parameters.AddWithValue("@ConduccionCablesPiso", Pis);
                        command.Parameters.AddWithValue("@ConduccionCablesDivision", Div);
                        command.Parameters.AddWithValue("@ConduccionCablesCielo", Cie);
                        command.Parameters.AddWithValue("@ConduccionCablesCanaleta", Can);
                        command.Parameters.AddWithValue("@BajantesElectricos", BteEle);
                        command.Parameters.AddWithValue("@Bajantesswitches", BteSw);
                        command.Parameters.AddWithValue("@SujecionCielo", SujPt);
                        command.Parameters.AddWithValue("@PerfilRefuerzo", PerRef);
                        command.Parameters.AddWithValue("@GuardaEscobas", GuaEsc);
                        command.Parameters.AddWithValue("@Linea", Lin);
                        command.Parameters.AddWithValue("@TipoMostrador", Most);
                        command.Parameters.AddWithValue("@AcabadoSuperficie", Superficies);
                        command.Parameters.AddWithValue("@BalanceSuperficies", Balance);
                        command.Parameters.AddWithValue("@TipoSoporte", Soporte);
                        command.Parameters.AddWithValue("@TipoGaveta", Gaveta);
                        command.Parameters.AddWithValue("@AcabadoPaneles", Paneles);
                        command.Parameters.AddWithValue("@TipoTapaPierna", TapPier);
                        command.Parameters.AddWithValue("@TipoRepisa", Repisa);
                        command.Parameters.AddWithValue("@TipodePantalla", Pantalla);
                        command.Parameters.AddWithValue("@TipoArchivador", Arch);
                        command.Parameters.AddWithValue("@MuebleEntrepano", Entrepano);
                        command.Parameters.AddWithValue("@MueblePuertas", Puertas);
                        command.Parameters.AddWithValue("@Observaciones_Ventas", ObservacionVenta);
                        command.Parameters.AddWithValue("@Observaciones_Diseño", ObservacionDibDes);
                        command.Parameters.AddWithValue("@SeguimientoPausa", ObservacionPauDev);
                        command.Parameters.AddWithValue("@SC_Presentacionppt", PresentacionPtt);
                        command.Parameters.AddWithValue("@SC_Imagenes", Imagenes);
                        command.Parameters.AddWithValue("@SC_Accesorios", Accesorios);
                        command.Parameters.AddWithValue("@SC_Tiemporeal", TiempoReal);
                        command.Parameters.AddWithValue("@SC_Fecha", SCFecha);
                        command.Parameters.AddWithValue("@SC_Ubicacion", ubicacionSC);

                        // Ejecutar la consulta SQL
                        int rowsAffected = command.ExecuteNonQuery();

                        // Comprobar si se insertaron filas
                        if (rowsAffected > 0)
                        {
                            // Éxito: los datos se insertaron correctamente
                            Response.Write("Datos insertados correctamente.");
                        }
                        else
                        {
                            // Error: los datos no se insertaron
                            Response.Write("Error al insertar los datos.");
                        }
                    }
                }

            }

           
           

         
        }



        protected void Button9_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button9
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            // Lógica para el botón Button10
        }



        //DATAGRID DISEÑO
        protected void DataGrid2_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string programadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();
                string pasarACotizar = DataBinder.Eval(e.Item.DataItem, "PasarACotizar").ToString();
                string terminadoDibujo = DataBinder.Eval(e.Item.DataItem, "TerminadoDibujo").ToString();
                string pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();

                if (programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                    e.Item.ForeColor = System.Drawing.Color.Black;
                }
                else if (programadoVentas == "True" && pasarACotizar == "True" && terminadoDibujo == "True")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /*Violeta*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else if (programadoVentas == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#819cba"); /*Azul*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else if (programadoVentas == "True" && pasarACotizar == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#be94b9"); /*Rosado*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }

                else if (pausado == "True")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#74bec6"); /*Celeste*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }




                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                
                TableCell descripcionCell = e.Item.Cells[2];

               
                if (descripcionCell != null)
                {
                    descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                }
            }
        }
        protected void DataGridDise_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Numero_Diseño")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid2.Items[rowIndex];

                string numeroDiseño = row.Cells[1].Text.Replace("&nbsp;", null);

                if (int.TryParse(numeroDiseño, out int numDise))
                {
                    // Establecer el valor del parámetro en el SqlDataSource
                    SqldatasourceTxt.SelectParameters["NumeroDiseño"].DefaultValue = numDise.ToString();

                    // Ejecutar el SqlDataSource
                    SqldatasourceTxt.DataBind();

                    string connectionString = "Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("sp_FormularioDisBita", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@NumDise", numDise);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                   




                                    int numeroDiseno;
                                    if (int.TryParse(reader["Numero_Diseño"].ToString(), out numeroDiseno))
                                    {
                                        // El valor se pudo convertir a int, asignarlo a lblNumDise como int
                                        lblNumDise.Text = numeroDiseno.ToString();
                                    }
                                    else
                                    {
                                        // No se pudo convertir a int, manejar el error o asignar un valor predeterminado
                                        lblNumDise.Text = "Valor no válido"; // O cualquier otro valor predeterminado
                                    }

                                    // Cambiar el formato de Fecha_Ingreso
                                    DateTime fechaIngreso = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso"));
                                    TextIngDis.Text = fechaIngreso.ToString("yyyy-MM-dd");

                                    // Cambiar el formato de UltimaActivacion
                                    DateTime ultimaActivacion = reader.GetDateTime(reader.GetOrdinal("UltimaActivacion"));
                                    TextUltAc.Text = ultimaActivacion.ToString("yyyy-MM-dd");

                                    // Cambiar el formato de Fecha_Programada_Entrega
                                    DateTime fechaEntrega = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega"));
                                    TextEntrega.Text = fechaEntrega.ToString("yyyy-MM-dd");

                                    // Cambiar el formato de FechaDibujoOK
                                    DateTime fechaDibujoOK = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK"));
                                    TextFecOkDib.Text = fechaDibujoOK.ToString("yyyy-MM-dd");

                                    TextZona.Text = GetString(reader, "Zona");

                                    string presentacionCotizacion = GetString(reader, "PresentacionCotizacion");
                                    ListItem presentacionCotizacionItem = new ListItem(presentacionCotizacion, presentacionCotizacion);
                                    TextPre.Items.Add(presentacionCotizacionItem);

                                    TextCliente.Text = GetString(reader, "Cliente");
                                    TextProyecto.Text = GetString(reader, "Nombre_Diseño");
                                    TextContacto.Text = GetString(reader, "Contacto");
                                    TextTel.Text = GetString(reader, "Telefono");
                                    TextCel.Text = GetString(reader, "Celular");
                                    TextMail.Text = GetString(reader, "Mail");
                                    TextDir.Text = GetString(reader, "Direccion");
                                    TextDes.Text = GetString(reader, "Descuento");
                                    TextPla.Text = GetString(reader, "PlanoBitacora");
                                    ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("Urgente"));
                                    ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("PasarACotizar"));
                                    ChecMailTer.Checked = reader.GetBoolean(reader.GetOrdinal("MailTerminado"));

                                    //TextCiuPro.Text = GetString(reader, "CiudadYDepartamento");
                                    //string ciudadProvincia = GetString(reader, "CiudadYDepartamento");
                                    //ListItem ciudadProvinciaItem = new ListItem(ciudadProvincia, ciudadProvincia);
                                    //TextCiuPro.Items.Add(ciudadProvinciaItem);

                                    ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("CotizarViaticos"));
                                    ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("Cotizartransporte"));

                                    ChecPiso.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesPiso"));
                                    ChecCie.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCielo"));
                                    ChecBteEle.Checked = reader.GetBoolean(reader.GetOrdinal("BajantesElectricos"));
                                    ChecBteSw.Checked = reader.GetBoolean(reader.GetOrdinal("Bajantesswitches"));
                                    ChecDiv.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesDivision"));
                                    ChecCan.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCanaleta"));

                                    ChecAlCie.Checked = reader.GetBoolean(reader.GetOrdinal("SujecionCielo"));
                                    ChecPerRef.Checked = reader.GetBoolean(reader.GetOrdinal("PerfilRefuerzo"));
                                    ChecGuaEsc.Checked = reader.GetBoolean(reader.GetOrdinal("GuardaEscobas"));

                                    TextSup.Text = GetString(reader, ("AcabadoSuperficie"));
                                    TextPan.Text = GetString(reader, ("AcabadoPaneles"));
                                    TextTipVid.Text = GetString(reader, ("TipodeVidrio"));
                                    TextLin.Text = GetString(reader, ("Linea"));
                                    TextSop.Text = GetString(reader, ("TipoSoporte"));
                                    TextTapPie.Text = GetString(reader, ("TipoTapaPierna"));
                                    TextPant.Text = GetString(reader, ("TipodePantalla"));
                                    TextMos.Text = GetString(reader, ("TipoMostrador"));
                                    TextGav.Text = GetString(reader, ("TipoGaveta"));
                                    TextRep.Text = GetString(reader, ("TipoRepisa"));
                                    TextArch.Text = GetString(reader, ("TipoArchivador"));
                                    TextCoc.Text = GetString(reader, ("MuebleCoco"));
                                    TextEnt.Text = GetString(reader, ("MuebleEntrepano"));
                                    TextPuer.Text = GetString(reader, ("MueblePuertas"));
                                    if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Ventas")))
                                    {
                                        TextObsVen.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Ventas"));
                                    }
                                    else
                                    {
                                        // Si el valor es nulo, puedes manejarlo de alguna manera, por ejemplo, asignar un valor predeterminado a TextObsVen.InnerText
                                        TextObsVen.InnerText = " ";
                                    }
                                    // Para TextObsDibDes
                                    if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Diseño")))
                                    {
                                        TextObsDibDes.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Diseño"));
                                    }
                                    else
                                    {
                                        // Manejo del valor nulo o vacío para TextObsDibDes
                                        TextObsDibDes.InnerText = " ";
                                    }

                                    // Para TextSegPauDev
                                    if (!reader.IsDBNull(reader.GetOrdinal("SeguimientoPausa")))
                                    {
                                        TextSegPauDev.InnerText = reader.GetString(reader.GetOrdinal("SeguimientoPausa"));
                                    }
                                    else
                                    {
                                        // Manejo del valor nulo o vacío para TextSegPauDev
                                        TextSegPauDev.InnerText = " ";
                                    }

                                }
                            }
                        }
                    }

                    UpdateDiseñoBitacora.Update();
                    

                }
            }
        }

        protected void DataGridBusDise_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Numero_Diseño")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataGridItem row = DataGrid4.Items[rowIndex];

                string numeroDiseño = row.Cells[1].Text.Replace("&nbsp;", null);

                if (int.TryParse(numeroDiseño, out int numDise))
                {
                    // Establecer el valor del parámetro en el SqlDataSource
                    SqldatasourceTxt.SelectParameters["NumeroDiseño"].DefaultValue = numDise.ToString();

                    // Ejecutar el SqlDataSource
                    SqldatasourceTxt.DataBind();

                    string connectionString = "Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("sp_FormularioDisBita", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@NumDise", numDise);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int numeroDiseno = reader.GetInt32(reader.GetOrdinal("Numero_Diseño"));




                                    lblNumDise.Text = numeroDiseño.ToString();
                                    // Cambiar el formato de Fecha_Ingreso
                                    DateTime fechaIngreso = reader.GetDateTime(reader.GetOrdinal("Fecha_Ingreso"));
                                    TextIngDis.Text = fechaIngreso.ToString("yyyy-MM-dd");

                                    // Cambiar el formato de UltimaActivacion
                                    DateTime ultimaActivacion = reader.GetDateTime(reader.GetOrdinal("UltimaActivacion"));
                                    TextUltAc.Text = ultimaActivacion.ToString("yyyy-MM-dd");

                                    // Cambiar el formato de Fecha_Programada_Entrega
                                    DateTime fechaEntrega = reader.GetDateTime(reader.GetOrdinal("Fecha_Programada_Entrega"));
                                    TextEntrega.Text = fechaEntrega.ToString("yyyy-MM-dd");

                                    // Cambiar el formato de FechaDibujoOK
                                    DateTime fechaDibujoOK = reader.GetDateTime(reader.GetOrdinal("FechaDibujoOK"));
                                    TextFecOkDib.Text = fechaDibujoOK.ToString("yyyy-MM-dd");
                                    TextZona.Text = GetString(reader, "Zona");

                                    string presentacionCotizacion = GetString(reader, "PresentacionCotizacion");
                                    ListItem presentacionCotizacionItem = new ListItem(presentacionCotizacion, presentacionCotizacion);
                                    TextPre.Items.Add(presentacionCotizacionItem);

                                    TextCliente.Text = GetString(reader, "Cliente");
                                    TextProyecto.Text = GetString(reader, "Nombre_Diseño");
                                    TextContacto.Text = GetString(reader, "Contacto");
                                    TextTel.Text = GetString(reader, "Telefono");
                                    TextCel.Text = GetString(reader, "Celular");
                                    TextMail.Text = GetString(reader, "Mail");
                                    TextDir.Text = GetString(reader, "Direccion");
                                    TextDes.Text = GetString(reader, "Descuento");
                                    TextPla.Text = GetString(reader, "PlanoBitacora");
                                    ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("Urgente"));
                                    ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("PasarACotizar"));
                                    ChecMailTer.Checked = reader.GetBoolean(reader.GetOrdinal("MailTerminado"));

                                    //TextCiuPro.Text = GetString(reader, "CiudadYDepartamento");
                                    //string ciudadProvincia = GetString(reader, "CiudadYDepartamento");
                                    //ListItem ciudadProvinciaItem = new ListItem(ciudadProvincia, ciudadProvincia);
                                    //TextCiuPro.Items.Add(ciudadProvinciaItem);

                                    ChecUrgent.Checked = reader.GetBoolean(reader.GetOrdinal("CotizarViaticos"));
                                    ChecCot.Checked = reader.GetBoolean(reader.GetOrdinal("Cotizartransporte"));

                                    ChecPiso.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesPiso"));
                                    ChecCie.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCielo"));
                                    ChecBteEle.Checked = reader.GetBoolean(reader.GetOrdinal("BajantesElectricos"));
                                    ChecBteSw.Checked = reader.GetBoolean(reader.GetOrdinal("Bajantesswitches"));
                                    ChecDiv.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesDivision"));
                                    ChecCan.Checked = reader.GetBoolean(reader.GetOrdinal("ConduccionCablesCanaleta"));

                                    ChecAlCie.Checked = reader.GetBoolean(reader.GetOrdinal("SujecionCielo"));
                                    ChecPerRef.Checked = reader.GetBoolean(reader.GetOrdinal("PerfilRefuerzo"));
                                    ChecGuaEsc.Checked = reader.GetBoolean(reader.GetOrdinal("GuardaEscobas"));

                                    TextSup.Text = GetString(reader, ("AcabadoSuperficie"));
                                    TextPan.Text = GetString(reader, ("AcabadoPaneles"));
                                    TextTipVid.Text = GetString(reader, ("TipodeVidrio"));
                                    TextLin.Text = GetString(reader, ("Linea"));
                                    TextSop.Text = GetString(reader, ("TipoSoporte"));
                                    TextTapPie.Text = GetString(reader, ("TipoTapaPierna"));
                                    TextPant.Text = GetString(reader, ("TipodePantalla"));
                                    TextMos.Text = GetString(reader, ("TipoMostrador"));
                                    TextGav.Text = GetString(reader, ("TipoGaveta"));
                                    TextRep.Text = GetString(reader, ("TipoRepisa"));
                                    TextArch.Text = GetString(reader, ("TipoArchivador"));
                                    TextCoc.Text = GetString(reader, ("MuebleCoco"));
                                    TextEnt.Text = GetString(reader, ("MuebleEntrepano"));
                                    TextPuer.Text = GetString(reader, ("MueblePuertas"));
                                    if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Ventas")))
                                    {
                                        TextObsVen.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Ventas"));
                                    }
                                    else
                                    {
                                        // Si el valor es nulo, puedes manejarlo de alguna manera, por ejemplo, asignar un valor predeterminado a TextObsVen.InnerText
                                        TextObsVen.InnerText = " ";
                                    }
                                    // Para TextObsDibDes
                                    if (!reader.IsDBNull(reader.GetOrdinal("Observaciones_Diseño")))
                                    {
                                        TextObsDibDes.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Diseño"));
                                    }
                                    else
                                    {
                                        // Manejo del valor nulo o vacío para TextObsDibDes
                                        TextObsDibDes.InnerText = " ";
                                    }

                                    // Para TextSegPauDev
                                    if (!reader.IsDBNull(reader.GetOrdinal("SeguimientoPausa")))
                                    {
                                        TextSegPauDev.InnerText = reader.GetString(reader.GetOrdinal("SeguimientoPausa"));
                                    }
                                    else
                                    {
                                        // Manejo del valor nulo o vacío para TextSegPauDev
                                        TextSegPauDev.InnerText = " ";
                                    }

                                }
                            }
                        }
                    }

                    UpdateDiseñoBitacora.Update();


                }
            }
        }

        private string GetDateTimeAsString(SqlDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                DateTime dateValue = reader.GetDateTime(reader.GetOrdinal(columnName));
                return dateValue.ToString("yyyy-MM-dd hh:mm tt");
            }
            return string.Empty;
        }

        private string GetString(SqlDataReader reader, string columnName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return reader.GetString(reader.GetOrdinal(columnName));
            }
            return string.Empty;
        }


        private void UpdateDataGrids()
        {
            DataGrid1.DataBind();
            DataGrid2.DataBind();
            DataGridDiseños.DataBind();
            DataGridRender.DataBind();
            UpdatePanel1.Update();
        }



        protected void DropDownListOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = DropDownListOptions.SelectedValue;

            if (selectedValue == "01")
            {
                SqlDataSource1.FilterExpression = "Zona = '01'";
                DataGridDiseño.FilterExpression = "Zona = '01'";
                DataGridDiseñosPorFecha.FilterExpression = "Zona = '01'";
                DataGridRenderPorFechaYAsesor.FilterExpression = "Zona = '01'";
            }
            else if (selectedValue == "02")
            {
                SqlDataSource1.FilterExpression = "Zona = '02'";
                DataGridDiseño.FilterExpression = "Zona = '02'";
                DataGridDiseñosPorFecha.FilterExpression = "Zona = '02'";
                DataGridRenderPorFechaYAsesor.FilterExpression = "Zona = '02'";
            }
            else
            {
                SqlDataSource1.FilterExpression = "";
                DataGridDiseño.FilterExpression = "";
                DataGridDiseñosPorFecha.FilterExpression = "";
                DataGridRenderPorFechaYAsesor.FilterExpression = "";
            }

            UpdateDataGrids();
        }

        protected void Button88_Click(object sender, EventArgs e)
        {
            UpdateDataGrids();
        }



        //DATAGRID SHOWCASE
        protected void DataGrid3_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
          

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string scimaganes = DataBinder.Eval(e.Item.DataItem, "SC_Imagenes").ToString();
                string scterminado = DataBinder.Eval(e.Item.DataItem, "SC_Terminado").ToString();

                if (scimaganes == "True" && scterminado == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }

            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                    string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                    string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();

                    // Encontrar la celda correspondiente a la columna Descripción por índice
                    TableCell descripcionCell = e.Item.Cells[2]; // Ajusta el índice si es necesario

                    // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                    if (descripcionCell != null)
                    {
                        descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                    }
                }
            }
        
            protected void DataGrid4_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string terminadoRender = DataBinder.Eval(e.Item.DataItem, "TerminadoRender").ToString();
                string fechaProgramadaString = DataBinder.Eval(e.Item.DataItem, "Fecha_Programada_Entrega").ToString();

                if (terminadoRender == "False")
                {
                    if (!string.IsNullOrEmpty(fechaProgramadaString) && DateTime.TryParse(fechaProgramadaString, out DateTime fechaProgramada))
                    {
                        if (fechaProgramada < DateTime.Now)
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868");
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }
                        else if (fechaProgramada > DateTime.Now)
                        {
                            e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                            e.Item.ForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            }



            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               // Obtener los valores de Cliente y Nombre_Diseño de la fila actual
                string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
               string nombreRender = DataBinder.Eval(e.Item.DataItem, "Nombre_Render").ToString();

                // Encontrar la celda correspondiente a la columna Descripción por índice
               TableCell descripcionCell = e.Item.Cells[2]; // Ajusta el índice si es necesario
                                
                // Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
               if (descripcionCell != null)
                {
                  descripcionCell.Text = $"{cliente} - {nombreRender}";
               }
            }
        }
        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    System.Web.UI.WebControls.Label lbDibujante = (System.Web.UI.WebControls.Label)e.Item.FindControl("lbDibujante");

            //    if (lbDibujante != null)
            //    {
            //        // Verifica si el dibujante ya ha sido mostrado previamente
            //        if (e.Item.ItemIndex > 0 && lbDibujante.Text == ((System.Web.UI.WebControls.Label)DataGrid1.Items[e.Item.ItemIndex - 1].FindControl("lbDibujante")).Text)
            //        {
            //            lbDibujante.Visible = false; // Oculta el Label si el dibujante es igual al anterior
            //        }
            //    }
            //}

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DateTime fechaEntrega = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "Fecha_Entrega_Dibujo_Despiece"));
                DateTime fechaActual = DateTime.Now;

                if (fechaEntrega < fechaActual)
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#efdd79");
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#c86868");
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
            }
        }

       
    

        protected void CheckBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox22.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal1').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }

        protected void CheckBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox23.Checked)
            {
                isModalVisible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modal2').modal('show');", true);
            }
            else
            {
                isModalVisible = false;
            }
        }



        protected void DataGridBusDis_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string programadoVentas = DataBinder.Eval(e.Item.DataItem, "ProgramadoVentas").ToString();
                string pasarACotizar = DataBinder.Eval(e.Item.DataItem, "PasarACotizar").ToString();
                string terminadoDibujo = DataBinder.Eval(e.Item.DataItem, "TerminadoDibujo").ToString();
                string pausado = DataBinder.Eval(e.Item.DataItem, "Pausado").ToString();
                string CotizacionOk = DataBinder.Eval(e.Item.DataItem, "CotizaciónOK").ToString();

                if (programadoVentas == "True" && terminadoDibujo == "False" && pausado == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#ead97b"); /*Amarillo*/
                    e.Item.ForeColor = System.Drawing.Color.Black;
                }
                else if (programadoVentas == "True" && terminadoDibujo == "True" && CotizacionOk == "True")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#3d803f"); /*Verde*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }

                else if (programadoVentas == "True" && pasarACotizar == "True" && terminadoDibujo == "True")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#673f8b"); /*Violeta*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else if (programadoVentas == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#819cba"); /*Azul*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }
                else if (programadoVentas == "True" && pasarACotizar == "False")
                {
                    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#be94b9"); /*Rosado*/
                    e.Item.ForeColor = System.Drawing.Color.White;
                }

               




                //// Combinar los valores de Cliente y Nombre_Diseño en la celda de Descripción
                //string cliente = DataBinder.Eval(e.Item.DataItem, "Cliente").ToString();
                //string nombreDiseño = DataBinder.Eval(e.Item.DataItem, "Nombre_Diseño").ToString();


                //TableCell descripcionCell = e.Item.Cells[2];


                //if (descripcionCell != null)
                //{
                //    descripcionCell.Text = $"{cliente} - {nombreDiseño}";
                //}
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                // Obtiene el nombre del archivo y el contenido
                string fileName = FileUpload1.FileName;
                byte[] fileContent = FileUpload1.FileBytes;

                // Aquí puedes escribir código para guardar la información del TextBox y el archivo en la base de datos (SQL Server)
                // Por ejemplo, puedes usar SqlConnection y SqlCommand para insertar datos en la base de datos.

                // Luego, puedes realizar cualquier acción adicional que necesites.

                // Borra el contenido del TextBox después de guardar los datos
                TextBox1.Text = string.Empty;
            }
            else
            {
                // Muestra un mensaje de error si no se selecciona un archivo
                // Puedes personalizar este mensaje según tus necesidades
                Response.Write("Por favor, selecciona un archivo.");
            }
        }

        protected void btnMostrarUbicacion_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                // Obtiene el nombre del archivo seleccionado y lo muestra en TextBox1
                TextBox1.Text = FileUpload1.FileName;

                // Obtiene la ubicación del archivo en el sistema de archivos del servidor y lo muestra en TextBox2
                TextBox2.Text = Server.MapPath("~/uploads/") + FileUpload1.FileName;
                // La ruta mostrada en TextBox2 se refiere a la ubicación en el servidor donde se almacenaría el archivo.
                // Debes asegurarte de que esta carpeta exista y tenga los permisos necesarios para guardar archivos.
            }
            else
            {
                // Muestra un mensaje de error si no se ha seleccionado un archivo
                TextBox1.Text = "No se ha seleccionado ningún archivo.";
                TextBox2.Text = "No se ha seleccionado ningún archivo.";
            }
        }

    }
}