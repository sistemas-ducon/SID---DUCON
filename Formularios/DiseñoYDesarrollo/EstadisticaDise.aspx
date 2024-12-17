<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadisticaDise.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo.EstadisticaDise" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
 <meta name="viewport" content="width=device-width, initial-scale=1" />
 <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
 <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
   <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
   <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"/>
 <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
 <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

 <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

 <link type="text/css" href="../../Recursos/CSS/DiseñoYDesarrollo/ObjetosDibujo.css" rel="stylesheet" />
    <title>Estadisticas Diseño</title>
</head>
<body>
      <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
               <nav class="navbar navbar-light bg-light navbar-custom">
         <div class="container d-flex justify-content-center">
             <ul class="nav nav-tabs" id="myTabs">

                 <li class="nav-item">
                      <a class="nav-link text-white active" id="EstadisticaDibDes-tab" data-bs-toggle="tab" href="#EstadisticaDibDes-content">Estadisticas Dibujo y Despiece</a>
                 </li>
                 <li class="nav-item">
                     <a class="nav-link text-white" id="CumplimientoRenders-tab" data-bs-toggle="tab" href="#CumplimientoRenders-content">Cumplimiento de Renders</a>
                 </li>

             </ul>
         </div>
     </nav>

          <div class="tab-content" id="myTabContent">
              <div class="tab-pane fade show active" id="EstadisticaDibDes-content">
                  <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                      <ContentTemplate>
                          <div class="container-fluid py-4">
                              <div class="card shadow-sm">
                                  <div class="card-header text-center text-dark">
                                      <div class="row g-3">
                                          <div class="col-md-3 col-sm-6">
                                              <div class="input-group input-group-sm d-flex gap-2">
                                                  <label for="txtPeriodoInicio" class="form-label">Periodo de Consulta del:</label>
                                                  <asp:TextBox
                                                      ID="txtPeriodoInicio"
                                                      runat="server"
                                                      CssClass="form-control form-control-sm"
                                                      TextMode="Date">
                                                  </asp:TextBox>
                                              </div>
                                          </div>
                                          <div class="col-md-2 col-sm-6">
                                              <div class="input-group input-group-sm d-flex gap-2">
                                                  <label for="txtPeriodoFin" class="form-label">Al:</label>
                                                  <asp:TextBox
                                                      ID="txtPeriodoFin"
                                                      runat="server"
                                                      CssClass="form-control form-control-sm"
                                                      TextMode="Date">
                                                  </asp:TextBox>
                                              </div>
                                          </div>
                                          <div class="col-md-3 col-sm-6">
                                              <div class="input-group input-group-sm d-flex gap-2">
                                                  <label for="ddlAsesor" class="form-label">Asesor:</label>
                                                  <asp:DropDownList
                                                      ID="ddlAsesor"
                                                      runat="server"
                                                      CssClass="form-select form-select-sm">
                                                  </asp:DropDownList>
                                              </div>
                                          </div>
                                          <div class="col-md-2 col-sm-6">
                                              <div class="input-group input-group-sm d-flex gap-2">
                                                  <label for="ddlZona" class="form-label ">Zona:</label>
                                                  <asp:DropDownList
                                                      ID="ddlZona"
                                                      runat="server"
                                                      CssClass="form-select form-select-sm">
                                                      <asp:ListItem Value="%" Text="%" />
                                                      <asp:ListItem Value="01" Text="01"></asp:ListItem>
                                                      <asp:ListItem Value="02" Text="02"></asp:ListItem>
                                                  </asp:DropDownList>
                                              </div>
                                          </div>
                                          <div class="col-md-1 col-sm-6">
                                              <div class="col text-start">
                                                  <asp:Button
                                                      ID="btnBuscar"
                                                      runat="server"
                                                      Text="..."
                                                      CssClass="btn btn-primary btn-sm me-2 shadow-sm" Style="width" OnClick="btnBuscar_Click" />
                                              </div>
                                          </div>
                                          <div class="col-md-1 col-sm-6">
                                              <div class="col text-end">
                                                  <asp:Button
                                                      ID="btnCancelar"
                                                      runat="server"
                                                      Text="X"
                                                      CssClass="btn btn-outline-secondary btn-sm shadow-sm" />

                                              </div>
                                          </div>
                                      </div>
                                  </div>
                                  <div class="card-body">
                                      <h5 class="text-center">PERIODO DE CONSULTA</h5>
                                      <div class="row g-3 mt-2">
                                          <div class="col-md-4 col-sm-12">
                                              <div class="card shadow-sm" style="height: 20rem;">
                                                  <div class="card-header text-center bg-light text-dark">
                                                      RESUMEN ESTADÍSTICA POR PEDIDOS
                                                  </div>
                                                  <div class="card-body text-center">
                                                      <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
                                                          <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                              ID="DataGridResumenEstadisticaPorPedido" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridResumenEstadisticaPorPedido_ItemDataBound" OnItemCommand="DataGridResumenEstadisticaPorPedido_ItemCommand">
                                                              <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                              <Columns>
                                                                  <asp:TemplateColumn HeaderText=". . .">
                                                                      <ItemTemplate>
                                                                          <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridEstadisticaPedido"
                                                                              CommandArgument='<%# Container.ItemIndex %>'
                                                                              Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                      </ItemTemplate>
                                                                  </asp:TemplateColumn>
                                                                  <asp:BoundColumn HeaderText="Nº" DataField="" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="RealizadoPor" HeaderText="Realizado Por" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="TotalPedidos" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="" HeaderText="%" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="PedidosCumplidos" HeaderText="Cum" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="PorcentajeCumplido" HeaderText="%"
                                                                      ItemStyle-CssClass="auto-width-column"
                                                                      DataFormatString="{0:#,0.0}%" />
                                                                  <asp:BoundColumn DataField="PedidosNoCumplidos" HeaderText="NO.Cum" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="PorcentajeNoCumplido" HeaderText="%" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:#,0.0}%" />
                                                                  <asp:BoundColumn DataField="VentaNeta" HeaderText="Venta Neta"
                                                                      ItemStyle-CssClass="auto-width-column"
                                                                      DataFormatString="{0:N0}" />
                                                                  <asp:BoundColumn DataField="" HeaderText="%" ItemStyle-CssClass="auto-width-column" />
                                                              </Columns>
                                                          </asp:DataGrid>
                                                      </div>

                                                  </div>
                                              </div>
                                          </div>

                                          <div class="col-md-8 col-sm-12">
                                              <div class="card shadow-sm" style="height: 20rem;">
                                                  <div class="card-header text-center bg-light text-dark">
                                                  <asp:Label runat="server" ID="lblDetallePedidos"></asp:Label> 
                                                  </div>
                                                  <div class="card-body text-center">
                                                      <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
                                                          <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                              ID="DataGridDetalleEstadisiticaPorPedido" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridDetalleEstadisiticaPorPedido_ItemDataBound">
                                                              <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                              <Columns>
                                                                  <asp:TemplateColumn HeaderText=". . .">
                                                                      <ItemTemplate>
                                                                          <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridTipoInsumo"
                                                                              CommandArgument='<%# Container.ItemIndex %>'
                                                                              Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                      </ItemTemplate>
                                                                  </asp:TemplateColumn>
                                                                  <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Nombre_Obra" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Entrega" HeaderText="E" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Fecha_Entrega_Dibujo_Despiece" HeaderText="F.Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Fecha_Entrega_Produccion" HeaderText="F.Ok" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="VentaNeta" HeaderText="V.Neta" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Fecha_Despacho_Produccion" HeaderText="F.Despacho" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Urgente" HeaderText="Urg" ItemStyle-CssClass="auto-width-column" />
                                                              </Columns>
                                                          </asp:DataGrid>
                                                      </div>
                                                  </div>
                                              </div>
                                          </div>
                                      </div>
                                      <div class="row g-3 mt-2">
                                          <div class="col-md-4 col-sm-12">
                                              <div class="card shadow-sm" style="height: 20rem;">
                                                  <div class="card-header text-center bg-light text-dark">
                                                      ESTADISTICA POR DISEÑOS
                                                  </div>
                                                  <div class="card-body text-center">
                                                        <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
                                                      <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                          ID="DataGridEstadisticaPorDiseno" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridEstadisticaPorDiseno_ItemDataBound" OnItemCommand="DataGridEstadisticaPorDiseno_ItemCommand">
                                                          <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                          <Columns>
                                                              <asp:TemplateColumn HeaderText=". . .">
                                                                  <ItemTemplate>
                                                                      <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridDiseño"
                                                                          CommandArgument='<%# Container.ItemIndex %>'
                                                                          Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                  </ItemTemplate>
                                                              </asp:TemplateColumn>
                                                              <asp:BoundColumn HeaderText="Nº" DataField="" ItemStyle-CssClass="auto-width-column" />
                                                              <asp:BoundColumn HeaderText="Dibujante" DataField="Dibujante" ItemStyle-CssClass="auto-width-column" />
                                                              <asp:BoundColumn HeaderText="Dis" DataField="Diseños" ItemStyle-CssClass="auto-width-column" />
                                                              <asp:BoundColumn HeaderText="%" DataField="" ItemStyle-CssClass="auto-width-column" />
                                                              <asp:BoundColumn HeaderText="Cum" DataField="Cumplidos" ItemStyle-CssClass="auto-width-column" />
                                                              <asp:BoundColumn HeaderText="%" DataField="PorcentajeCumplidos" ItemStyle-CssClass="auto-width-column" />
                                                              <asp:BoundColumn HeaderText="No Cum" DataField="NoCumplidos" ItemStyle-CssClass="auto-width-column" />
                                                              <asp:BoundColumn HeaderText="%" DataField="PorcentajeNoCumplidos" ItemStyle-CssClass="auto-width-column" />
                                                          </Columns>
                                                      </asp:DataGrid>
                                                            </div>
                                                  </div>
                                              </div>
                                          </div>
                                          <div class="col-md-8 col-sm-12">
                                              <div class="card shadow-sm" style="height: 20rem;">
                                                  <div class="card-header text-center bg-light text-dark">
                                                      DETALLE ESTADISTICA POR DISEÑOS
                                                  </div>
                                                  <div class="card-body text-center">
                                                      <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
                                                          <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                              ID="DataGridDetalleEstadisticaPorDiseno" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridDetalleEstadisticaPorDiseno_ItemDataBound">
                                                              <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                              <Columns>
                                                                  <asp:TemplateColumn HeaderText=". . .">
                                                                      <ItemTemplate>
                                                                          <asp:LinkButton ID="lnkView" runat="server" CommandName=""
                                                                              CommandArgument='<%# Container.ItemIndex %>'
                                                                              Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                      </ItemTemplate>
                                                                  </asp:TemplateColumn>
                                                                  <asp:BoundColumn HeaderText="Nombre" DataField="ClienteYNombreDiseño" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="Diseño" DataField="Numero_Diseño" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="E" DataField="ENTREGA" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="F.Activación" DataField="UltimaActivacion" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="F.Entrega" DataField="Fecha_Programada_Entrega" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="F.Ok" DataField="FechaDibujoOK" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="Asesor" DataField="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="Urg" DataField="Urgente" ItemStyle-CssClass="auto-width-column"/>
                                                                  <asp:BoundColumn HeaderText="Pausas" DataField="SeguimientoPausa" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="F.Ingreso" DataField="Fecha_Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="F.Ingreso" DataField="Fecha_Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn HeaderText="Zona" DataField="Zona" />
                                                              </Columns>
                                                          </asp:DataGrid>
                                                      </div>

                                                  </div>
                                              </div>
                                          </div>
                                      </div>
                                  </div>
                              </div>

                          </div>
                      </ContentTemplate>
                  </asp:UpdatePanel>
              </div>

              <div class="tab-pane fade" id="CumplimientoRenders-content">
                  <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                      <ContentTemplate>
                         <div class="container-fluid py-4">
     <div class="card shadow-sm">
         <div class="card-header text-center text-dark">
             <div class="row g-3">
                 <div class="col-md-3 col-sm-12">
                     <div class="input-group input-group-sm d-flex gap-2">
                         <label for="txtPeriodoInicio" class="form-label">Periodo de Consulta del:</label>
                         <asp:TextBox
                             ID="TextBoxFechaInicioRenders"
                             runat="server"
                             CssClass="form-control form-control-sm"
                             TextMode="Date">
                         </asp:TextBox>
                     </div>
                 </div>
                 <div class="col-md-3 col-sm-12">
                     <div class="input-group input-group-sm d-flex gap-2">
                         <label for="txtPeriodoFin" class="form-label">Al:</label>
                         <asp:TextBox
                             ID="TextBoxFechaFinRenders"
                             runat="server"
                             CssClass="form-control form-control-sm"
                             TextMode="Date">
                         </asp:TextBox>
                     </div>
                 </div>

                 <div class="col-md-3 col-sm-12">
                     <div class="input-group input-group-sm d-flex gap-2">
                         <label for="ddlZona" class="form-label ">Zona:</label>
                         <asp:DropDownList
                             ID="DropDownList2"
                             runat="server"
                             CssClass="form-select form-select-sm">
                            <asp:ListItem Value="%" Text="%" />
    <asp:ListItem Value="01" Text="01"></asp:ListItem>
       <asp:ListItem Value="02" Text="02"></asp:ListItem>
                         </asp:DropDownList>
                     </div>
                 </div>
                 <div class="col-md-2 col-sm-6">
                     <div class="col text-start">
                           <asp:Button
      ID="BtnConsultar"
      runat="server"
     Text="Consultar"
      CssClass="btn btn-primary btn-sm me-2 shadow-sm" OnClick="BtnConsultar_Click"/>
                        
                     </div>
                 </div>
                 <div class="col-md-1 col-sm-6">
                     <div class="col text-end">
                          <asp:Button
     ID="Button1"
     runat="server"
     Text="X"
     CssClass="btn btn-outline-secondary btn-sm shadow-sm" />
                     </div>
                 </div>

             </div>
         </div>
         <div class="card-body">

             <h5 class="text-center">PERIODO DE CONSULTA</h5>
             <div class="row g-3 mt-2">
                 <div class="col-md-4 col-sm-12">
                     <div class="card shadow-sm" style="height: 20rem;">
                         <div class="card-header text-center bg-light text-dark">
                             RESUMEN ESTADÍSTICA RENDERS
                         </div>
                         <div class="card-body text-center">
                             <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
                                 <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                     ID="DataGridResumenEstadisticaRenders" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridResumenEstadisticaRenders_ItemDataBound" OnItemCommand="DataGridResumenEstadisticaRenders_ItemCommand">
                                     <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                     <Columns>
                                         <asp:TemplateColumn HeaderText=". . .">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridRender"
                                                     CommandArgument='<%# Container.ItemIndex %>'
                                                     Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                             </ItemTemplate>
                                         </asp:TemplateColumn>
                                         <asp:BoundColumn DataField="Numero" HeaderText="Nº"
                                             ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                         <asp:BoundColumn DataField="Responsable" HeaderText="Responsable"
                                             ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                         <asp:BoundColumn DataField="Rend" HeaderText="Rend"
                                             ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                         <asp:BoundColumn DataField="PorcentajeRend" HeaderText="%"
                                             DataFormatString="{0:F2}" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                         <asp:BoundColumn DataField="Cum" HeaderText="Cum"
                                             ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                         <asp:BoundColumn DataField="PorcentajeCum" HeaderText="%"
                                             DataFormatString="{0:F2}" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                         <asp:BoundColumn DataField="NoCum" HeaderText="No Cum"
                                             ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                         <asp:BoundColumn DataField="PorcentajeNoCum" HeaderText="%"
                                             DataFormatString="{0:F2}" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                     </Columns>
                                 </asp:DataGrid>
                             </div>
                         </div>
                     </div>
                 </div>

                 <div class="col-md-8 col-sm-12">
                     <div class="card shadow-sm" style="height: 20rem;">
                         <div class="card-header text-center bg-light text-dark">
                             DETALLE ESTADISTICA POR RENDERS
                         </div>
                         <div class="card-body text-center">
                             <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
                                 <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                     ID="DataGridEstadisticaRenders" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridEstadisticaRenders_ItemDataBound">
                                     <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                     <Columns>
                                         <asp:TemplateColumn HeaderText=". . .">
                                             <ItemTemplate>
                                                 <asp:LinkButton ID="lnkView" runat="server" CommandName=""
                                                     CommandArgument='<%# Container.ItemIndex %>'
                                                     Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                             </ItemTemplate>
                                         </asp:TemplateColumn>
                                         <asp:BoundColumn DataField="ClienteNombre" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="Id_Render" HeaderText="Render" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="Entrega" HeaderText="E (Días)" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="UltimaActivacion" HeaderText="F.Activación" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="FechaRenderOk" HeaderText="F.Ok" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="SeguimientoPausa" HeaderText="Pausas" ItemStyle-CssClass="auto-width-column" />
                                         <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column" />
                                     </Columns>
                                 </asp:DataGrid>
                             </div>
                         </div>
                     </div>
                 </div>

             </div>
             <div class="row g-3 mt-2">
                 <div class="col-md-4 col-sm-12">
                     <div class="card shadow-sm" style="height:20rem;">
                         <div class="card-header text-center bg-light text-dark">
                             ESTADISTICA POR DISEÑOS
                         </div>
                         <div class="card-body text-center">
                              <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
    <asp:DataGrid 
    ID="DataGridResumenEstadisticaShowCase" 
    runat="server" 
    AutoGenerateColumns="false" 
    CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
    OnItemDataBound="DataGridResumenEstadisticaShowCase_ItemDataBound" OnItemCommand="DataGridResumenEstadisticaShowCase_ItemCommand">
    <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
    <Columns>
         <asp:TemplateColumn HeaderText=". . .">
     <ItemTemplate>
         <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridSC"
             CommandArgument='<%# Container.ItemIndex %>'
             Text="<i class='bi bi-pencil-square text-dark'></i>" />
     </ItemTemplate>
 </asp:TemplateColumn>
        <asp:BoundColumn 
            DataField="Nº" 
            HeaderText="Nº" 
            ItemStyle-CssClass="auto-width-column">
        </asp:BoundColumn>
        <asp:BoundColumn 
            DataField="Responsable" 
            HeaderText="Responsable" 
            ItemStyle-CssClass="auto-width-column">
        </asp:BoundColumn>
        <asp:BoundColumn 
            DataField="Visit" 
            HeaderText="Visit" 
            ItemStyle-CssClass="auto-width-column">
        </asp:BoundColumn>
        <asp:BoundColumn 
            DataField="%" 
            HeaderText="%" 
            DataFormatString="{0:F2}" 
            ItemStyle-CssClass="auto-width-column">
        </asp:BoundColumn>
    </Columns>
</asp:DataGrid>

 </div>
                         </div>
                     </div>
                 </div>
                 <div class="col-md-8 col-sm-12">
                     <div class="card shadow-sm" style="height:20rem;">
                         <div class="card-header text-center bg-light text-dark">
                             DETALLE ESTADISTICA POR DISEÑOS
                         </div>
                         <div class="card-body text-center">
                              <div class="table-responsive table-responsive-sm border shadow-sm" style="height: 15rem; overflow-x: auto;">
     <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
         ID="DataGrid1" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridEstadisticaSC_ItemDataBound">
         <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
         <Columns>
             <asp:TemplateColumn HeaderText=". . .">
                 <ItemTemplate>
                     <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridTipoInsumo"
                         CommandArgument='<%# Container.ItemIndex %>'
                         Text="<i class='bi bi-pencil-square text-dark'></i>" />
                 </ItemTemplate>
             </asp:TemplateColumn>
             <asp:BoundColumn DataField="ClienteNombre" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column" />
             <asp:BoundColumn DataField="Numero_Diseño" HeaderText="N°Diseño" ItemStyle-CssClass="auto-width-column" />
             <asp:BoundColumn DataField="SC_FechaTerminado" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column" />
             <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
         </Columns>
     </asp:DataGrid>
 </div>
                         </div>
                     </div>
                 </div>
             </div>
         </div>
     </div>
 </div>
                      </ContentTemplate>
                  </asp:UpdatePanel>
              </div>

          </div>

      </form>
      <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
