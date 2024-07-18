<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OT_ManualesSid.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Consultas.OT_ManualesSid" %>

<!DOCTYPE html>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/OrdenTrabajo.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScripManager1" runat="server"></asp:ScriptManager>

        <div class="tab-content">
            <div class="tab-pane fade show active" id="Fecha_Modulo">
                <asp:UpdatePanel ID="FechaM" UpdateMode="Conditional" runat="server">

                    <ContentTemplate>
                        <div class="container-fluid">
                            <div class="row pt-2 mt-3">
                                <div class="col-1"></div>

                                <div class="col-3">
                                    <asp:Label class="form-label" Text="Fecha Real del despacho" runat="server" ID="lbFechaPedido"></asp:Label>
                                </div>

                                <div class="col-2">

                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:TextBox ID="tbfechaIni" type="date" runat="server" class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2 ">
                                        <asp:TextBox ID="tbfechaFinal" type="date" runat="server" Class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-1 pb-1">
                                    <div class="input btn-group-sm">
                                        <asp:Button CssClass="btn btn-secondary" ID="Mostrar" runat="server" Text="Mostar O.T.s" OnClick="Mstrar" />

                                    </div>

                                </div>



                                <div class="col-1">
                                    <div class="input-group input-group-sm">
                                        <asp:LinkButton CssClass="icong" title="Exportar Excel" ID="Exportar" runat="server" OnClick="ExportE">
                                            <h1> <i class="bi bi-rocket"></i> </h1>
                                        </asp:LinkButton>
                                    </div>
                                </div>


                            </div>

                            <div class="row justify-content-center pb-2 p-4">
                                <div class=" border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-5 " style="height: 21rem; overflow-x: auto">
                                                <h6 class="datagrid-header text-center">panel</h6>

                                                <asp:DataGrid CssClass="table table-bordered    table-sm table-hover form-control-sm" ID="DataModulo" runat="server" OnItemDataBound="DataGridBusDis_ItemDataBound"   OnItemCommand="DataModulo_ItemCommand"   AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">

                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="color" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i> ">
                                                                <!--  con un icono de lápiz -->
                                                                    </asp:LinkButton>
                                                            </ItemTemplate>
                                                            
                                                               
                                                          

                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="" HeaderText="item" ItemStyle-CssClass="auto-width-column " />
                                                        <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column " />
                                                        <asp:BoundColumn DataField="Id_OT_Secundario" HeaderText="OT Alt" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Nombre de la Obra" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Codigo_Asesor" HeaderText="Ases" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Real_Despacho" HeaderText="Empaque" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Terminada_Empaque" HeaderText="Emp" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Terminada_Empaque" HeaderText="T.Empaque" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Terminada_Despacho" HeaderText="Desp" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Terminada_Despacho" HeaderText="Despacho" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ResumenObra" HeaderText="Resumen  Obra" ItemStyle-CssClass="auto-width-column" />


                                                        <asp:BoundColumn DataField="" Visible="false" />

                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>



                            <div class="row justify-content-center p-4">

                                <div class="border rounded p-3">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="max-height: 10rem; overflow-x: auto;">

                                                <h6 class="datagrid-header text-center">titulo</h6>
                                                <asp:DataGrid ID="DataGridDetalleSolicitud" runat="server" CssClass="table table-bordered table-sm table-hover form-control-sm"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>



                                                        <asp:BoundColumn DataField="TOTALES" HeaderText="TOTALES" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="EMPAQUE" HeaderText="EMPAQUE" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="%E" HeaderText="%" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DESPACHO" HeaderText="DESPACHO" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="%D" HeaderText="%" ItemStyle-CssClass="auto-width-column" />




                                                        <asp:BoundColumn DataField="" Visible="false" />




                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>


                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="Exportar" />
                    </Triggers>

                </asp:UpdatePanel>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>

