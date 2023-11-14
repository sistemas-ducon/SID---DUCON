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
using System.Text;
using TextBox = System.Web.UI.WebControls.TextBox;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Presentation;
using TableCell = System.Web.UI.WebControls.TableCell;
using DataGrid = System.Web.UI.WebControls.DataGrid;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class ObjetoDespiece : System.Web.UI.Page
    {
        private List<TextBox> listaTextBoxes;
        private double FactorImprevisto;
        private double FactorMod;
        private double TotalSubtotal;
        private double TotaImpr;
        private double TotalMO;
        private double TotalVenta;
        protected void Page_Load(object sender, EventArgs e)
        {
            Cargar_Informacion_Modulo();
            Cargar_Informacion_DespieceModulo();
            Cargar_Informacion_DespieceModuloAsesor();
            listaTextBoxes = new List<TextBox>
                {
                    tbObj,tbDiv,tbLinea,tbGrupo,tbAncho,tbAltura,tbProfunididad,tbHolgura,tbDesSid,tbValor,tbValorVenta,

                };
            DeshabilitarTextBoxes(listaTextBoxes);

        }


        public void DeshabilitarTextBoxes(List<TextBox> textBoxes)
        {
            foreach (TextBox textBox in textBoxes)
            {

                textBox.Enabled = false;
                textBox.CssClass = "form-control ";
            }
        }

        public void Cargar_Informacion_Modulo()
        {
            string IdPanelNum = Session["Id_PanelNum_Session"]?.ToString();

            if (IdPanelNum != null)
            {
                string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                DataTable DatosModulo = new DataTable();

                using (SqlConnection connection = new SqlConnection(cn))
                {
                    SqlCommand command = new SqlCommand("sp_DatoModulo", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id_PanelNum", SqlDbType.VarChar, 30).Value = IdPanelNum;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(DatosModulo);
                }

                string Id_Modulo = DatosModulo.Rows[0]["Id_Modulo"].ToString();
                Session["Id_ModuloSession"] = Id_Modulo;

                tbObj.Text = DatosModulo.Rows[0]["Id_Panel"].ToString();
                tbDiv.Text = DatosModulo.Rows[0]["Divisiones"].ToString();
                tbLinea.Text = "";
                tbGrupo.Text = DatosModulo.Rows[0]["Descripcion_familia"].ToString(); ;
                tbAncho.Text = DatosModulo.Rows[0]["Ancho"].ToString() + " Cms";
                tbAltura.Text = DatosModulo.Rows[0]["Altura"].ToString() + " Cms";
                tbProfunididad.Text = DatosModulo.Rows[0]["Profundidad"].ToString();
                tbHolgura.Text = DatosModulo.Rows[0]["Holgura"].ToString();
                tbDesSid.Text = DatosModulo.Rows[0]["Descripcion_Panel"].ToString();
                tbValor.Text = DatosModulo.Rows[0]["Precio_Venta"].ToString();
                tbValorVenta.Text = Session["ValorVentaSession"].ToString();


                DataGridObjetos.DataSource = DatosModulo;
                DataGridObjetos.DataBind();
            }

        }
        public void Cargar_Informacion_DespieceModulo()
        {

            string IdModulo = Session["Id_ModuloSession"]?.ToString();

            if (IdModulo != null)
            {
                string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                DataTable DatosModulo1 = new DataTable();

                using (SqlConnection connection = new SqlConnection(cn))
                {
                    SqlCommand command = new SqlCommand("ctaModulo_Insumos", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Modulo", SqlDbType.VarChar, 30).Value = IdModulo;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(DatosModulo1);


                }

                StringBuilder valorConcatenado = new StringBuilder();
                string Id_ModuloInsumo = "";
                // Añadimos una columna personalizada para almacenar el concatenado de las descripciones de las áreas
                DatosModulo1.Columns.Add("Descripcion_Areas_Concatenadas", typeof(string));

                foreach (DataRow row in DatosModulo1.Rows)
                {
                    Id_ModuloInsumo = row["Id_ModuloInsumo"].ToString();

                    using (SqlConnection connection = new SqlConnection(cn))
                    {
                        SqlCommand command = new SqlCommand("SELECT tblAreaProduccion.*, tblRotacionInsumo.* FROM tblAreaProduccion INNER JOIN tblRotacionInsumo ON tblAreaProduccion.Id_Area = tblRotacionInsumo.riId_Area WHERE (((tblRotacionInsumo.riId_ModuloInsumo)=@Id_Insumo)) order by tblRotacionInsumo.riEstacion", connection);
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add("@Id_Insumo", SqlDbType.VarChar, 30).Value = Id_ModuloInsumo;
                        DataTable DatosDespieceModulo = new DataTable();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(DatosDespieceModulo);



                        foreach (DataRow row1 in DatosDespieceModulo.Rows)
                        {

                            valorConcatenado.Append(row1["Descripcion_Area"].ToString() + ", ");
                        }

                    }
                    valorConcatenado.Remove(valorConcatenado.Length - 2, 2);
                    row["Descripcion_Areas_Concatenadas"] = valorConcatenado.ToString();
                    valorConcatenado.Clear();
                }


                DataGridDespieceModulo.DataSource = DatosModulo1;
                DataGridDespieceModulo.DataBind();
            }


        }


        //Este Datagrid esta pendiente por revisar las consultas 
        public void Cargar_Informacion_DespieceModuloAsesor()
        {

            string IdModulo = Session["Id_ModuloSession"]?.ToString();
            string IdPanelNum = Session["Id_PanelNum_Session"]?.ToString();


            // Se Cargan los Factores de imprevisto y MODucon 
            if (IdPanelNum != null)
            {
                string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                DataTable Factores = new DataTable();
                using (SqlConnection connection = new SqlConnection(cn))
                {
                    SqlCommand command = new SqlCommand("SELECT  tblPanel.Id_Numerico, tblGrupoObjeto.FactorMODucon, tblGrupoObjeto.FactorImprevistoDucon FROM tblPanel INNER JOIN tblGrupoObjeto ON tblPanel.Id_GrupoObjeto = tblGrupoObjeto.ID_GrupoObjeto   Where tblPanel.Id_Numerico = @Id_PanelNum", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@Id_PanelNum", SqlDbType.VarChar, 30).Value = IdPanelNum;
                    DataTable DatosDespieceModulo = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(Factores);


                }
                foreach (DataRow row in Factores.Rows)
                {
                    FactorImprevisto = Convert.ToDouble(row["FactorImprevistoDucon"]);
                    FactorMod = Convert.ToDouble(row["FactorMODucon"]);

                }




            }

            // Se carga el dataridDespieceAsesor y se realizan los totales 
            if (IdModulo != null)
            {
                string cn = ConfigurationManager.ConnectionStrings["BD_SIDSQL_PRUEBA"].ConnectionString;
                DataTable DatosModulo1 = new DataTable();

                using (SqlConnection connection = new SqlConnection(cn))
                {
                    SqlCommand command = new SqlCommand("DespieceObjeto", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@P", SqlDbType.VarChar, 30).Value = IdPanelNum;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(DatosModulo1);


                }
                DatosModulo1.Columns.Add("Item", typeof(int));
                DatosModulo1.Columns.Add("Pieza", typeof(string));

                int itemCount = 1;
                double subtotal;

                foreach (DataRow row in DatosModulo1.Rows)
                {
                    row["Item"] = itemCount;
                    itemCount++;
                    row["Pieza"] = row["Descripcion_Insumo"].ToString() + " - " + row["DescripcionPieza"].ToString();



                    if ((bool)row["Costear"])
                    {
                        // Si Costear es verdadero, asignar el valor de SubTotal desde la base de datos
                        subtotal = Convert.ToDouble(row["SubTotal"]);
                        row["SubTotal"] = (int)Math.Round(subtotal);
                        TotalSubtotal += subtotal;

                        // Puedes realizar otras operaciones relacionadas con Costear aquí si es necesario
                    }
                    else
                    {
                       
                        row["SubTotal"] = 0;
                        row["Valor_Costo"] = 0;


                    }

                }
                // Agregar una fila adicional para el título y la suma de los subtotales
                DatosModulo1.Rows.Add();

                // Agregar una fila adicional para el título y la suma de los subtotales
                DataRow FilaTotal = DatosModulo1.NewRow();
                FilaTotal["Pieza"] = "Total Insumos";
                FilaTotal["SubTotal"] = (int)Math.Round(TotalSubtotal);
                DatosModulo1.Rows.Add(FilaTotal);

                DataRow FilaImprevistos = DatosModulo1.NewRow();
                FilaImprevistos["Pieza"] = "Por Imprevistos";
                TotaImpr = Math.Round(TotalSubtotal * FactorImprevisto / 100);
                FilaImprevistos["SubTotal"] = TotaImpr;
                DatosModulo1.Rows.Add(FilaImprevistos);

                DataRow FilaFactorMOD = DatosModulo1.NewRow();
                FilaFactorMOD["Pieza"] = "MO Ducon";
                TotalMO = Math.Round(TotalSubtotal * FactorMod / 100);
                FilaFactorMOD["SubTotal"] = TotalMO;
                DatosModulo1.Rows.Add(FilaFactorMOD);

                DataRow filaValorVenta = DatosModulo1.NewRow();
                 filaValorVenta["Pieza"] = "Valor Venta";
                TotalVenta = TotalSubtotal + TotaImpr + TotalMO;
                filaValorVenta["SubTotal"] = Math.Ceiling(TotalVenta / 1000) * 1000; 
                DatosModulo1.Rows.Add(filaValorVenta);




                DataGridDespieceAsesor.DataSource = DatosModulo1;
                DataGridDespieceAsesor.DataBind();
            }


        }


        protected void DataGridDespieceAsesor_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView rowView = (DataRowView)e.Item.DataItem;
                string piezaValue = rowView["Pieza"].ToString();
                if (piezaValue == "Total Insumos" || piezaValue == "Por Imprevistos" || piezaValue == "MO Ducon" || piezaValue == "Valor Venta")
                {
                    e.Item.Font.Bold = true;

                    if(piezaValue == "Valor Venta")
                    {
                        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#33FFE9");
                    }

                    

                }

              
            }
        }




    }
}