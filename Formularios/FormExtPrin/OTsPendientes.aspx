<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OTsPendientes.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.OTsPendientes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/OTsPendientes.css" rel="stylesheet" />

   <title>
    Ordenes de Trabajo Pendientes - Departamento de Ventas
    
    <i class="bi bi-icono-aqui"></i> <!-- Reemplaza "bi-icono-aqui" con la clase del icono que desees -->
</title>

</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container-fluid mt-2">

            <nav class="navbar navbar-light bg-light">
                <div class="container">
                    <div class="navbar-brand mx-auto">                     
                            <h5 class="bi bi-gear"> Ordenes de Trabajo Pendientes - Departamento de Ventas</h5>                      
                    </div>

                </div>
            </nav>

            <asp:UpdatePanel ID="PanelOrdPen" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-12">
                                <div class="border rounded-3 container-fluid mt-2" style="height: 54rem;">

                                    <div class="container-fluid mt-2">
                                        <div class="row">
                                            <div class="col-md-2 col-3">
                                                <div class="input-group input-group-sm gap-1">
                                                   <asp:RadioButton ID="RadioButton18" runat="server" GroupName="filtroGroup" CssClass="custom-radio-button" />
                                                    <asp:Label ID="lblPrePpt" runat="server" class="col-form-label-sm">Pendientes</asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-2 col-3">
                                                <div class="input-group input-group-sm gap-1">
                                                   <asp:RadioButton ID="RadioButton1" runat="server" GroupName="filtroGroup" CssClass="custom-radio-button" />
                                                    <asp:Label ID="Label1" runat="server" class="col-form-label-sm">No Importadas</asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-2 col-3">
                                                <div class="input-group input-group-sm gap-1">
                                                     <asp:RadioButton ID="RadioButton2" runat="server" GroupName="filtroGroup" CssClass="custom-radio-button" />
                                                    <asp:Label ID="Label2" runat="server" class="col-form-label-sm">Todos</asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-2 col-3">
                                                <div class="input-group input-group-sm gap-1">
                                                    <asp:Label runat="server" ID="Label3" class="col-form-label-sm">Asesor</asp:Label>
                                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-3">
                                                <div class="input-group input-group-sm gap-1">
                                                    <asp:Label runat="server" ID="lblDir" class="col-form-label-sm">Obra</asp:Label>
                                                    <asp:TextBox ID="TextDir" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1 col-3">
                                                <div class="input-group input-group-sm gap-1">
                                                    <asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="form-control-sm btn-sm btn btn-outline-dark" OnClick="Button1_Click" />
                                                </div>
                                            </div>

                                        </div>
                                        <div class="d-flex">
                                            <div class="col-11">
                                                <div class="row mt-2">
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm gap-1">
                                                            <asp:Label runat="server" ID="Label4" class="col-form-label-sm">F.Busqueda</asp:Label>
                                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                                                ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                                SelectCommand="SELECT COLUMN_NAME 
                                                                                            FROM INFORMATION_SCHEMA.COLUMNS 
                                                                                            WHERE TABLE_NAME = 'tblOT' 
                                                                                            AND COLUMN_NAME IN (
                                                                                                'Fecha_Real_Despacho_Produccion', 
                                                                                                'Fecha_Entrega_Produccion', 
                                                                                                'Fecha_Entrega_Dibujo_Despiece', 
                                                                                                'Fecha_Terminada_Despacho', 
                                                                                                'Fecha_Terminada_Empaque', 
                                                                                                'Fecha_Instalacion', 
                                                                                                'Fecha_Final_Instalacion'
                                                                                            )
                                                                                            ORDER BY 
                                                                                                CASE 
                                                                                                    COLUMN_NAME 
                                                                                                    WHEN 'Fecha_Real_Despacho_Produccion' THEN 1
                                                                                                    WHEN 'Fecha_Entrega_Produccion' THEN 2
                                                                                                    WHEN 'Fecha_Entrega_Dibujo_Despiece' THEN 3
                                                                                                    WHEN 'Fecha_Terminada_Despacho' THEN 4
                                                                                                    WHEN 'Fecha_Terminada_Empaque' THEN 5
                                                                                                    WHEN 'Fecha_Instalacion' THEN 6
                                                                                                    WHEN 'Fecha_Final_Instalacion' THEN 7
                                                                                                END"></asp:SqlDataSource>

                                                            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="COLUMN_NAME" DataValueField="COLUMN_NAME" CssClass="form-control-sm form-control">
                                                            </asp:DropDownList>


                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-3">
                                                        <div class="input-group input-group-sm">
                                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control form-control-sm" type="Date"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="Label6" class="col-form-label-sm">Y</asp:Label>
                                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm" type="Date"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2 col-3">
                                                        <asp:Button ID="Button2" runat="server" Text="Terminar Pedido" CssClass="form-control btn btn-outline-dark" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-1">
                                                <div class="row">
                                                    <div class="col-12">
                                                        <asp:ImageButton ID="Button3" runat="server" OnClick="Button3_Click"
                                                            ImageUrl="https://i.ibb.co/86fR8JK/icons8-microsoft-excel-2019-48.png" AlternateText="Excel Icon" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>


                                    </div>

                                    <div class="container-fluid">
                                        <div class="row mt-2">
                                            <div class="col-12">
                                                <div class="border rounded-3 mt-2">
                                                    <h6 class="text-center">PEDIDOS PENDIENTES - VENTAS</h6>
                                                     <div class="table-container"> 
                                                    <div class="table-responsive table-responsive-sm mb-2 gap-2 form-control-sm" style="max-height: 40rem; overflow-x: auto;">
                                                        <asp:DataGrid Class="table table-bordered table-hover table-sm form-control-sm" ID="DataGrid1" runat="server"
                                                            AutoGenerateColumns="false">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSelectRow" runat="server" OnClick="lnkSelectRow_Click"
                                                                            CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Nombre Obra" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Codigo_Asesor" HeaderText="Vend" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Entrega_Dibujo_Despiece" HeaderText="F.Ok.Venta" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Entrega_Produccion" HeaderText="F.Ok.Dib" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="TDibujo" HeaderText="T.Dib" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="RealizadoPor" HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Importacion" HeaderText="Imp" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Empaque" HeaderText="F.Ok.Emp" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="TPccion" HeaderText="Pcción" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Despacho_Produccion" HeaderText="Des Prod" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Real_Despacho_Produccion" HeaderText="F.R.Desp" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="CumpPccion" HeaderText="Cump" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Instalacion" HeaderText="F.Inst" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="EnPlanta" HeaderText="Planta" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Final_Instalacion" HeaderText="F.F.Inst" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="TInstala" HeaderText="Inst" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Factura" HeaderText="F.Fact" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminada_Almacen" HeaderText="Alm" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminada_Produccion" HeaderText="Prod" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ValorViatico" HeaderText="Vta" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminado_Diseño" HeaderText="Dib" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminada_Empaque" HeaderText="Emp" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminada_Despacho" HeaderText="Desp" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminada_Compras" HeaderText="Comp" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminada_Facturacion" HeaderText="F.y.C" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ResumenObra" HeaderText="ResumenObra" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Id_OT_secundario" HeaderText="Altern" ItemStyle-CssClass="auto-width-column" />
                                                            </Columns>
                                                        </asp:DataGrid>
                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                            ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                            SelectCommand="sp_OTsPendientesVPD"
                                                            SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                                                    </div>
                                                </div>
                                                    </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Label ID="NoResultsLabel" runat="server" Visible="false" CssClass="text-danger">No se encontraron resultados.</asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
