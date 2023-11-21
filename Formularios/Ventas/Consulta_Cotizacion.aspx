<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Consulta_Cotizacion.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Consulta_Cotizacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
    <link href="../../Recursos/CSS/Ventas/Consulta_Cotizacion.css" rel="stylesheet" />
    <title>Consultas de Cotizaciones</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <label class="navbar-brand mb-0 h1">Consulta de Cotizaciones</label>
            </div>
        </nav>


        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="PorVendedor-tab" data-bs-toggle="tab" href="#PorVendedor-content">Por Vendedor</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="PorEstado-tab" data-bs-toggle="tab" href="#PorEstado-content">Por estado</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Seguimiento-tab" data-bs-toggle="tab" href="#Seguimiento-content">Seguimiento</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Totales-tab" data-bs-toggle="tab" href="#Totales-content">Totales</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="EnEstudio-tab" data-bs-toggle="tab" href="#EnEstudio-content">En Estudio</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="UltimoContacto-tab" data-bs-toggle="tab" href="#UltimoContacto-content">Ultimo Contacto</a>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="tab-content container">
            
            <%-- TAB POR VENDEDOR--%> <%--TAB-COMPLETADO-FUNCIONALIDAD--%>

            <div class="tab-pane fade show active" id="PorVendedor-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanelPorVendedor" UpdateMode="Conditional">
                    <ContentTemplate>

                        <h6>Informacion Cotizacion</h6>

                        <div class="row">

                            <div class="col-5">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">Contizacion Entre</asp:Label>
                                    <asp:TextBox CssClass="form-control" runat="server" Type="date" ID="TextBoxStartDate"></asp:TextBox>
                                    <asp:Label CssClass="col-form-label-sm" runat="server">y</asp:Label>
                                    <asp:TextBox CssClass="form-control" runat="server" Type="Date" ID="TextBoxEndDate"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-5">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">Asesor:</asp:Label>
                                    <asp:TextBox ID="TextAsesor" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>

                                </div>

                            </div>

                            <div class="col-1">
                                <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm" runat="server" Text="Consultar" OnClick="TapPorVendedor_Click" />
                            </div>
                            

                        </div>

                        <div class="container mt-4">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3">


                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 300px; overflow-x: auto;">
                                                <asp:DataGrid Class="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid1" runat="server"
                                                    DataSourceID="DataGridConsultaCotizaciones" AutoGenerateColumns="false" OnPreRender="DataGridPorVendedor_PreRender">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Cotización" HeaderText="Cotizacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Estado" HeaderText="Estado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Competencia" HeaderText="Competencia" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Descuento" HeaderText="Dto(%)" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="VCCD" HeaderText="ValorNeto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Cliente" HeaderText="Cliente" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Obra" HeaderText="Obra" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Fecha_Cotización" HeaderText="F.Cotizacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Fecha_Respuesta" HeaderText="F.Respuesta" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Plano" HeaderText="Plano" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource runat="server" ID="DataGridConsultaCotizaciones" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                    SelectCommand="cta_Cotizaciones_Por_Vendedor" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="TextAsesor" PropertyName="Text" Name="NombreAsesor" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="TextBoxStartDate" PropertyName="Text" DbType="Date" Name="FechaInicio"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="TextBoxEndDate" PropertyName="Text" DbType="Date" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>


                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="container mt-4">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3" style="height: 200px">

                                    <div class="table-responsive">                                  
                                    <table class="table table-sm table-hover table-bordered form-control-sm">
                                        <thead class="thead-light">
                                            <tr>
                                                <th style="white-space: nowrap;">Estado</th>
                                                <th style="white-space: nowrap;">Cant</th>
                                                <th style="white-space: nowrap;">%</th>
                                                <th style="white-space: nowrap;">Total Valor Neto</th>
                                                <th style="white-space: nowrap;">%</th>                                              
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                               
                                                <td style="white-space: nowrap;">
                                                    <label runat="server" id="lbEstudio"></label>
                                                      
                                                </td>
                                                <td>
                                                    <label runat="server" id="lbCantEstudio"></label>
                                                </td>
                                                <td>
                                                    <label runat="server" id="lbPorEst"></label>
                                                </td>
                                                 <td>
                                                    <label runat="server" id="lbTotEst"></label>
                                                </td>
                                                 <td>
                                                    <label runat="server" id="lbPorEstTot"></label>
                                                </td>                                              
                                            </tr>
                                            <tr>
                                                <td>
                                                 <label runat="server" id="lbAprobada"></label>
                                                  </td>
                                                <td>
                                                     <label runat="server" id="lbCantApr"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="lbPorApr"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="lbTotApr"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="lbTotPor"></label>
                                                </td>
                                            </tr>
                                               <tr>
                                                <td>
                                                 <label runat="server" id="lbTotales"></label>
                                                  </td>
                                                <td>
                                                     <label runat="server" id="lbCantidad"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="lblTotal"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="Label10"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="lbPorTotal"></label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                     <div class="d-flex justify-content-end align-items-center mt-3">
                                      <asp:ImageButton ID="ImageButton1" runat="server" OnClick="LinkButton_Click"
                                          ImageUrl="https://i.ibb.co/86fR8JK/icons8-microsoft-excel-2019-48.png" AlternateText="Excel Icon" />
                                         </div>
                                </div>
                            </div>
                        </div>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <%-- TAB POR ESTADO--%> 

            <div class="tab-pane fade" id="PorEstado-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelPorEstado" UpdateMode="Conditional">
                    <ContentTemplate>

                        <h6>Criterios para la estadistica</h6>

                        <div class="row">

                            <div class="col-5">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">Contizacion Entre</asp:Label>
                                    <asp:TextBox ID="TextCotizacionEntreInicio2" CssClass="form-control" runat="server" Type="date"></asp:TextBox>
                                    <asp:Label CssClass="col-form-label-sm" runat="server">y</asp:Label>
                                    <asp:TextBox ID="TextCotizacionEntreFinal2" CssClass="form-control" runat="server" Type="Date"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-3">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">Asesor:</asp:Label>
                                    <asp:TextBox ID="TextAsesortab2" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-3">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">Estado Cotizacion:</asp:Label>
                                    <asp:DropDownList ID="ddlEstadoCotizacion" CssClass="form-control" runat="server"></asp:DropDownList>
                                </div>

                            </div>

                            <div class="col-1">
                                <asp:Button CssClass="btn-outline-dark btn btn-light btn-sm" runat="server" Text="Consultar" ID="BtnConsultarTab2" OnClick="BtnConsultarPorEstado_Click" />
                            </div>
                        </div>


                        <div class="row justify-content-center">
                            <div class="border rounded p-3 mt-4">

                                <div class="col-12">
                                    <div class="table-responsive mb-2 gap-2" style="max-height: 300px; overflow-x: auto;">

                                        <asp:DataGrid Class="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid2" runat="server" DataSourceID="DataGridPorEstado" AutoGenerateColumns="false" OnPreRender="DataGridPorEstado_PreRender">
                                             <HeaderStyle Font-Bold="true" CssClass="datagrid-header auto-width-column" />
                                            <Columns>
                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Cotización" HeaderText="Cotizacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="" HeaderText="Opc" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Valor" HeaderText="Valor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Descuento" HeaderText="Dto(%)" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="VCCD" HeaderText="Valor Neto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Cliente" HeaderText="Cliente" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="" HeaderText="Contacto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Obra" HeaderText="Obra" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Plano" HeaderText="Plano" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Fecha_Cotización" HeaderText="F.Cotizacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Fecha_Respuesta" HeaderText="F.Respuesta" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>

                                            </Columns>
                                        </asp:DataGrid>
                                       

                                        <asp:SqlDataSource runat="server" ID="DataGridPorEstado" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="cta_Cotizaciones_Por_Estado" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="TextAsesortab2" PropertyName="Text" Name="NombreAsesor" Type="String"></asp:ControlParameter>
                                                <asp:ControlParameter ControlID="TextCotizacionEntreInicio2" PropertyName="Text" DbType="Date" Name="FechaInicio"></asp:ControlParameter>
                                                <asp:ControlParameter ControlID="TextCotizacionEntreFinal2" PropertyName="Text" DbType="Date" Name="FechaFin"></asp:ControlParameter>
                                                <asp:ControlParameter ControlID="ddlEstadoCotizacion" PropertyName="SelectedValue" Name="Estado" Type="String"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>



                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="container mt-4">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3" style="height: 200px">

                                    <div class="table-responsive">                                  
                                    <table class="table table-sm table-hover table-bordered form-control-sm" id="Table1">
                                        <thead class="thead-light">
                                            <tr>
                                                <th style="white-space: nowrap;">Asesor Comercial</th>
                                                <th style="white-space: nowrap;">Cant</th>
                                                <th style="white-space: nowrap;">%</th>
                                                <th style="white-space: nowrap;">Valor Neto</th>
                                                <th style="white-space: nowrap;">%</th>                                              
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                               
                                                <td style="white-space: nowrap;">
                                                    <label runat="server" id="lbAseCom"></label>
                                                      
                                                </td>
                                                <td>
                                                    <label runat="server" id="Label2"></label>
                                                </td>
                                                <td>
                                                    <label runat="server" id="Label3"></label>
                                                </td>
                                                 <td>
                                                    <label runat="server" id="Label4"></label>
                                                </td>
                                                 <td>
                                                    <label runat="server" id="Label5"></label>
                                                </td>                                              
                                            </tr>
                                            <tr>
                                                <td>
                                                 <label runat="server" id="Label6"></label>
                                                  </td>
                                                <td>
                                                     <label runat="server" id="Label7"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="Label8"></label>
                                                </td>
                                                 <td>
                                                      <label runat="server" id="Label9"></label>
                                                </td>
                                                 <td>
                                                     <label runat="server" id="Label11"></label>
                                                 </td>
                                            </tr>

                                        </tbody>
                                    </table>
                                    </div>

                                    <div class="d-flex justify-content-end align-items-center mt-3">
                                      <asp:ImageButton ID="ImgBtnExportarExcel" CssClass="btn-outline-light btn btn-white btn-sm" runat="server" OnClick="BtnExportarExcelPorEstado_Click"
                                          ImageUrl="https://i.ibb.co/86fR8JK/icons8-microsoft-excel-2019-48.png" AlternateText="Excel Icon" />
                                    </div>


                                </div>
                        </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <%-- TAB SEGUIMIENTO--%>

            <div class="tab-pane fade" id="Seguimiento-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelSeguimiento" UpdateMode="Conditional">
                    <ContentTemplate>

                        <h6>Criterios para la estadistica</h6>

                        <div class="row">

                            <div class="col-5">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">F.Seguimiento Entre:</asp:Label>
                                    <asp:TextBox ID="IdDateInicial" CssClass="form-control" runat="server" Type="date"></asp:TextBox>
                                    <asp:Label CssClass="col-form-label-sm" runat="server">y</asp:Label>
                                    <asp:TextBox ID="IdDateFinal" CssClass="form-control" runat="server" Type="Date"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-5">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">Asesor:</asp:Label>
                                    <asp:TextBox ID="TextAsesorSeguimiento" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-2">
                                <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm" runat="server" Text="Consultar" />
                            </div>

                        </div>

                        <div class="container mt-4">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3">
                                    <div class="row">
                                        <div class="col-2">

                                            <div class="border rounded p-3" style="height: 400px">
                                                <table class="table table-bordered">
                                                    <thead class="table table-sm table-responsive-sm">
                                                        <tr class="ms-auto">
                                                            <h6 class="text-center">Seg Para Hoy</h6>
                                                            <th class="text-center" scope="col">Cotización</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>


                                        <div class="col-10">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 400px; overflow-x: auto;">

                                                <asp:DataGrid Class="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid3" runat="server" DataSourceID="DataGridSeguimiento" AutoGenerateColumns="false">
                                                     <HeaderStyle Font-Bold="true" CssClass="datagrid-header auto-width-column" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Cliente" HeaderText="Cliente" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Teléfono" HeaderText="Teléfono" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Cotización" HeaderText="Cotizacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Fecha_Cotización" HeaderText="F.Cotizacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Proximo_Seguimiento" HeaderText="Prox. Segui" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="VCCD" HeaderText="Valor Neto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Obra" HeaderText="Obra" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource runat="server" ID="DataGridSeguimiento" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_Cotizaciones_Seguimiento" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="TextAsesorSeguimiento" PropertyName="Text" Name="NombreAsesor" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="IdDateInicial" PropertyName="Text" DbType="Date" Name="FechaInicio"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="IdDateFinal" PropertyName="Text" DbType="Date" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-9">
                                <div class="container mt-4">
                                    <div class="row justify-content-center">
                                        <div class="border rounded p-3" style="height: 200px">
                                            <asp:GridView runat="server" ID="GridViewTotales" AutoGenerateColumns="false" CssClass="table table-sm table-hover">
                                                <HeaderStyle Font-Bold="false" />
                                                <Columns>
                                                    <asp:BoundField DataField="" HeaderText="Cotizacion" />
                                                    <asp:BoundField DataField="" HeaderText="F.Seguimiento" />
                                                    <asp:BoundField DataField="" HeaderText="Observacion" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-3">
                                <div class="container mt-4">
                                    <div class="row d-flex justify-content-between">
                                        <div class="col-6">
                                            <asp:Label runat="server" CssClass="col-form-label-sm">Asesor</asp:Label>
                                            <asp:DropDownList runat="server" ID="DropEstado" CssClass="form-control form-control-sm" Enabled="false"></asp:DropDownList>
                                        </div>
                                        <div class="col-6">
                                            <asp:Label runat="server" CssClass="col-form-label-sm">Causa</asp:Label>
                                            <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control form-control-sm" Placeholder="POR DEFINIR" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row d-flex justify-content-between mt-3">
                                        <div class="col-6">
                                            <asp:Label runat="server" CssClass="col-form-label-sm">Competencia</asp:Label>
                                            <asp:TextBox runat="server" ID="TextBox3" CssClass="form-control form-control-sm" Placeholder="POR DEFINIR" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <asp:Button CssClass="btn-outline-dark  btn btn-light" runat="server" Text="Cambiar Estado" Enabled="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-5">
                              <div class="input-group input-group-sm mb-2 gap-2 mt-3">
                                    <asp:Label CssClass="col-form-label-sm" runat="server" Enabled="false">Descripción Seguimiento:</asp:Label>
                                    <textarea class="form-control" runat="server"></textarea>
                                </div>
                        </div>
                            <div class="col-3 mt-3">
                                <asp:Label CssClass="col-form-label-sm" runat="server">Prox. Seguimiento</asp:Label>
                                <asp:TextBox id="TextProxSegui" runat="server" CssClass="form-control form-control-sm" Type="Date" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-2 mt-3">
                                <asp:Button CssClass="btn-outline-dark btn btn-light text-center" runat="server" Text="Grabar Seguimiento" Enabled="false"/>
                            </div>

                            <div class="col-1 mt-3">
                                <asp:Label CssClass="col-form-label-sm" runat="server">Cotizacion Ex</asp:Label>
                            </div>
                             <div class="col-1 mt-3">
                                <asp:Label CssClass="col-form-label-sm" runat="server">Cotizacion PDF</asp:Label>
                            </div>
                            </div>




                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <%--TAB TOTALES--%>

            <div class="tab-pane fade" id="Totales-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanelTotales" UpdateMode="Conditional">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>



            <%--TAB EN ESTUDIO--%>

            <div class="tab-pane fade" id="EnEstudio-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelEnEstudio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h6 class="text-center">RESUMEN DE COTIZACIONES EN ESTUDIO</h6>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <%--TAB ULTIMO CONTACTO--%>

           <div class="tab-pane fade" id="UltimoContacto-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelUltimoContacto" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="row">

                            <div class="col-5">

                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="col-form-label-sm" runat="server">Ultimo Contacto Comercial</asp:Label>
                                    <asp:TextBox ID="TextUltContComer" CssClass="form-control" runat="server" Type="date"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-2">
                                <asp:Button ID="BtnConsultarCC" CssClass="btn-outline-dark btn btn-light btn-sm"
                                    runat="server" Text="Consultar"  OnClick="bntConsultarUlCont_Click"/>
                            </div>

                            <div class="col-2">
                                <asp:Button ID="BtnExportar" CssClass="btn-outline-dark  btn btn-light btn-sm ms-auto" runat="server" Text="Exportar" OnClick="btnExportarUlCont_Click" />
                            </div>

                            <div class="col-2">
                                <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm ms-auto" runat="server" Text="UCCM" OnClick="btnUCCMUlCont_Click"/>
                            </div>

                        </div>

                        <div class="container mt-4">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3">
                                    <div class="col-12">
                                        <div class="table-responsive mb-2 gap-2" style="max-height: 400px; overflow-x: auto;">
                                            <asp:DataGrid CssClass="table table-sm table-bordered table-hover form-control-sm" ID="DataGrid4" runat="server" DataSourceID="DataGridUltimoContacto" OnRowCommand="DataGrid4_RowCommand" AutoGenerateColumns="false">
                                                 <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:BoundColumn DataField="uccNit" HeaderText="Nit" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccRazonSocial" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccAsesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccActivo" HeaderText="Activo" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccFecha" HeaderText="U.Contacto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccNombreContacto" HeaderText="Contacto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccTelefono" HeaderText="Teléfono" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccCelular" HeaderText="Celular" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccMail" HeaderText="Mail" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="uccRazon" HeaderText="Razón" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                            <asp:SqlDataSource runat="server" ID="DataGridUltimoContacto" ConnectionString="<%$
                                   ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                SelectCommand="SELECT [uccNit], [uccRazonSocial], [uccAsesor], [uccActivo], [uccFecha], [uccNombreContacto], [uccTelefono], [uccCelular], [uccMail], [uccRazon] FROM [tblUltiContCome] WHERE ([uccFecha] = @uccFecha)">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="TextUltContComer" PropertyName="Text" Name="uccFecha" Type="DateTime"></asp:ControlParameter>
                                                </SelectParameters>
                                            </asp:SqlDataSource>
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
