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



namespace SISTEMA_INTEGRAL_DUCON.Formularios
{
    public partial class Diseño_Venta : System.Web.UI.Page
    {
        private bool isModalVisible = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                            
                DropDownList1.DataBind();

                DropDownList1.SelectedValue = "";

                habilitarbotones();
                DeshabilitarDivYContenido(miDiv);

                CheckBox22.Checked = isModalVisible;

                Nombreasesor();
                elementosllenosalcargarlapagina();
            }


           
        }



        protected void elementosllenosalcargarlapagina()
        {
            BtnProgramar.CssClass = "btn btn-warning shadow btn-sm";

            DateTime fechaActual = DateTime.Now;

           
            // Establece la fecha actual en los TextBox
            TextIngDis.Text = fechaActual.ToString("dd/MM/yyyy hh:mm tt");
            TextUltAc.Text = fechaActual.ToString("dd/MM/yyyy hh:mm tt"); // Puedes personalizar el formato según tus necesidades
            TextEntrega.Text = fechaActual.ToString("dd/MM/yyyy hh:mm tt");
            TextFecOkDib.Text = fechaActual.ToString("dd/MM/yyyy hh:mm tt");
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
        }

        private void DeshabilitarDivYContenido(System.Web.UI.Control container)
        {
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







        protected void NuevoDisBit_Click(object sender, EventArgs e)
        {
            // Deshabilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = false;

            Modificar.Enabled = false;

            // Habilitar el botón "Grabar"
            Grabar.Enabled = true;

            // Deshabilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = false;

            // Habilitar el div y su contenido
            HabilitarDivYContenido(miDiv);

            lblNumDise.Text = "Por Definir";

            // Cambiar el color del Label lblCotizar
            lblCotizar.CssClass = "col-form-label-sm text-danger";
            lblCotizar.Font.Bold = true;

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
            // Deshabilitar el botón "Grabar"
            Grabar.Enabled = false;

            // Habilitar el botón "NuevoDisBit"
            NuevoDisBit.Enabled = true;

            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;

            // Ocultar el div y su contenido
            DeshabilitarDivYContenido(miDiv);

            lblNumDise.Text = "Número";
            lblCotizar.CssClass = "col-form-label-sm text-dark";
            lblCotizar.Font.Bold = false;
        }

        protected void lnkClie_Click(object sender, EventArgs e)
        {
           

            Grabar.Enabled = false;
            // Habilitar el botón "ActualizarDiseno"
            ActualizarDiseno.Enabled = true;

            // Habilitar el botón "Modificar"
            Modificar.Enabled = true;

            DocBitacora.Enabled = true;

            // Habilitar el botón "AdicionarElemento"
            AdicionarElemento.Enabled = true;

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
                    string query = "SELECT Zona, Nombre, Apellidos FROM tblEmpleado WHERE Login = @nombreUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", usuariologueado);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            string zona = reader["Zona"].ToString();
                            string nombre = reader["Nombre"].ToString();
                            string apellidos = reader["Apellidos"].ToString();
                            DropDownList1.Text = nombre + " " + apellidos; // Asignar el nombre y apellidos al TextBox
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
                                    TextIngDis.Text = GetDateTimeAsString(reader, "Fecha_Ingreso");
                                    TextUltAc.Text = GetDateTimeAsString(reader, "UltimaActivacion");
                                    TextEntrega.Text = GetDateTimeAsString(reader, "Fecha_Programada_Entrega");
                                    TextFecOkDib.Text = GetDateTimeAsString(reader, "FechaDibujoOK");

                                    TextZona.Text = GetString(reader, "Zona");
                                    TextPre.Text = GetString(reader, "PresentacionCotizacion");
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
                                    TextCiuPro.Text = reader.GetString(reader.GetOrdinal("CiudadYDepartamento"));                                 
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
                                    TextObsVen.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Ventas"));
                                    TextObsDibDes.InnerText = reader.GetString(reader.GetOrdinal("Observaciones_Diseño"));
                                    TextSegPauDev.InnerText = reader.GetString(reader.GetOrdinal("SeguimientoPausa"));
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

      
        }
}