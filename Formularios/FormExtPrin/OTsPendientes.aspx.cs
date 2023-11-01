using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin
{
    public partial class OTsPendientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string nombreObra = TextDir.Text.Trim();
            string codigoAsesor = TextBox2.Text.Trim();
            int maxRegistros = 100;

            // Obtener valores de los TextBox de fecha
            string fechaDesde = TextBox3.Text;
            string fechaHasta = TextBox1.Text;

            // Obtener el nombre del campo de fecha seleccionado en el DropDownList
            string campoFecha = DropDownList1.SelectedValue;

            bool checkBox18Marcado = CheckBox18.Checked;
            bool checkBox2Marcado = CheckBox2.Checked;

            if (checkBox18Marcado && !checkBox2Marcado)
            {
                // Filtrar por Terminado_Ventas = '0'
                SqlDataSource2.SelectCommand = $"SELECT TOP {maxRegistros} Id_OT, Consecutivo_Pedido, Nombre_Obra, Codigo_Asesor, Fecha_Confirmacion_Venta, Fecha_Entrega_Dibujo_Despiece, RealizadoPor, Importacion, Fecha_Empaque, Fecha_Despacho_Produccion, Fecha_Real_Despacho_Produccion, Fecha_Instalacion, Terminada_Almacen, Terminada_Produccion, ValorViatico, Terminado_Diseño, Terminada_Empaque, Terminada_Despacho, Terminada_Compras, Terminada_Facturacion, ResumenObra, Id_OT_secundario FROM tblOT WHERE Terminado_Ventas = '0'";
            }
            else if (checkBox2Marcado && !checkBox18Marcado)
            {
                // Filtrar por Terminado_Ventas = '1'
                SqlDataSource2.SelectCommand = $"SELECT TOP {maxRegistros} Id_OT, Consecutivo_Pedido, Nombre_Obra, Codigo_Asesor, Fecha_Confirmacion_Venta, Fecha_Entrega_Dibujo_Despiece, RealizadoPor, Importacion, Fecha_Empaque, Fecha_Despacho_Produccion, Fecha_Real_Despacho_Produccion, Fecha_Instalacion, Terminada_Almacen, Terminada_Produccion, ValorViatico, Terminado_Diseño, Terminada_Empaque, Terminada_Despacho, Terminada_Compras, Terminada_Facturacion, ResumenObra, Id_OT_secundario FROM tblOT WHERE Terminado_Ventas = '1'";
            }

            else if (!string.IsNullOrEmpty(nombreObra))
            {
                SqlDataSource2.SelectParameters.Clear();
                SqlDataSource2.SelectParameters.Add("NombreObra", nombreObra);

                SqlDataSource2.SelectCommand = $"SELECT TOP {maxRegistros} Id_OT, Consecutivo_Pedido, Nombre_Obra, Codigo_Asesor, Fecha_Confirmacion_Venta, Fecha_Entrega_Dibujo_Despiece, RealizadoPor, Importacion, Fecha_Empaque, Fecha_Despacho_Produccion, Fecha_Real_Despacho_Produccion, Fecha_Instalacion, Terminada_Almacen, Terminada_Produccion, ValorViatico, Terminado_Diseño, Terminada_Empaque, Terminada_Despacho, Terminada_Compras, Terminada_Facturacion, ResumenObra, Id_OT_secundario FROM tblOT WHERE Nombre_Obra LIKE '%' + @NombreObra + '%'";
            }
            else if (!string.IsNullOrEmpty(codigoAsesor))
            {
                SqlDataSource2.SelectParameters.Clear();
                SqlDataSource2.SelectParameters.Add("CodigoAsesor", codigoAsesor);

                SqlDataSource2.SelectCommand = $"SELECT TOP {maxRegistros} Id_OT, Consecutivo_Pedido, Nombre_Obra, Codigo_Asesor, Fecha_Confirmacion_Venta, Fecha_Entrega_Dibujo_Despiece, RealizadoPor, Importacion, Fecha_Empaque, Fecha_Despacho_Produccion, Fecha_Real_Despacho_Produccion, Fecha_Instalacion, Terminada_Almacen, Terminada_Produccion, ValorViatico, Terminado_Diseño, Terminada_Empaque, Terminada_Despacho, Terminada_Compras, Terminada_Facturacion, ResumenObra, Id_OT_secundario FROM tblOT WHERE Codigo_Asesor = @CodigoAsesor";
            }
            else
            {
                SqlDataSource2.SelectParameters.Clear();
                SqlDataSource2.SelectParameters.Add("FechaDesde", fechaDesde);
                SqlDataSource2.SelectParameters.Add("FechaHasta", fechaHasta);
                SqlDataSource2.SelectParameters.Add("CampoFecha", campoFecha);

                // Usar el campo de fecha seleccionado dinámicamente
                SqlDataSource2.SelectCommand = $"SELECT TOP {maxRegistros} Id_OT, Consecutivo_Pedido, Nombre_Obra, Codigo_Asesor, Fecha_Confirmacion_Venta, Fecha_Entrega_Dibujo_Despiece, RealizadoPor, Importacion, Fecha_Empaque, Fecha_Despacho_Produccion, Fecha_Real_Despacho_Produccion, Fecha_Instalacion, Terminada_Almacen, Terminada_Produccion, ValorViatico, Terminado_Diseño, Terminada_Empaque, Terminada_Despacho, Terminada_Compras, Terminada_Facturacion, ResumenObra, Id_OT_secundario FROM tblOT WHERE {campoFecha} BETWEEN @FechaDesde AND @FechaHasta";

            }

            DataGrid1.DataBind();

            if (DataGrid1.Items.Count == 0)
            {
                NoResultsLabel.Visible = true;
            }
            else
            {
                NoResultsLabel.Visible = false;
            }
        }



    }
}