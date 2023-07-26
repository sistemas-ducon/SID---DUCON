<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Consulta_Cotizacion.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Consulta_Cotizacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/Ventas/Consulta_Cotizacion.css" rel="stylesheet" />
    <title>Consultas de Cotizaciones</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
    

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

             <%-- TAB POR VENDEDOR--%>

                <div class="tab-pane fade show active" id="PorVendedor-content">
                    <asp:UpdatePanel runat="server" ID="UpdatePanelPorVendedor">
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

                       <div class="col-2">
                           <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm" runat="server" Text="Consultar" />
                       </div>

                       </div>

                     <div class="container mt-4">
                    <div class="row justify-content-center">
                        <div class="border rounded p-3">


               <div class="row">
                       <div class="col-12">
                           <div class="table-responsive mb-2 gap-2" style="max-height: 300px; overflow-x: auto;">
                               <asp:DataGrid Class="table table-bordered table-hover" ID="DataGrid1" runat="server" DataSourceID="DataGridConsultaCotizaciones" AutoGenerateColumns="false">
                               <Columns>
                                   <asp:BoundColumn DataField="Cotización" HeaderText="Cotizacion"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Estado" HeaderText="Estado"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Competencia" HeaderText="Competencia"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Valor" HeaderText="Valor"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Descuento" HeaderText="Dto(%)"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="VCCD" HeaderText="Valor Neto"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Cliente" HeaderText="Cliente"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Obra" HeaderText="Obra"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Asesor" HeaderText="Asesor"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Fecha_Cotización" HeaderText="F.Cotizacion"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Fecha_Respuesta" HeaderText="F.Respuesta"></asp:BoundColumn>
                                   <asp:BoundColumn DataField="Plano" HeaderText="Plano"></asp:BoundColumn>
                               </Columns>
                           </asp:DataGrid>
                               <asp:SqlDataSource runat="server" ID="DataGridConsultaCotizaciones" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="cta_Cotizaciones_Por_Vendedor" SelectCommandType="StoredProcedure">
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

                        </ContentTemplate>
                    </asp:UpdatePanel>
                

            </div>

            <%-- TAB POR ESTADO--%>

            <div class="tab-pane fade" id="PorEstado-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelPorEstado">
                    <ContentTemplate>
                      
                        <h6>Criterios para la estadistica</h6>

                   <div class="row">

                       <div class="col-5">
                   
                   <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label CssClass="col-form-label-sm" runat="server">Contizacion Entre</asp:Label>
                       <asp:TextBox id="TextCotizacionEntreInicio2" CssClass="form-control" runat="server" Type="date"></asp:TextBox>
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
                               <asp:DropDownList ID="ddlEstadoCotizacion" CssClass="form-control" runat="server" OnLoad="LoadEstados"></asp:DropDownList>
                           </div>

                       </div>

                       <div class="col-1">
                           <asp:Button CssClass="btn-outline-dark btn btn-light btn-sm" runat="server" Text="Consultar" ID="BtnConsultarTab2" OnClick="BtnConsultarTab2_Click" />
                       </div>
                   </div>

                        <div class="container mt-4">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3">

                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 300px; overflow-x: auto;">

                                                <asp:DataGrid Class="table table-bordered table-hover" ID="DataGrid2" runat="server" DataSourceID="DataGridPorEstado" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Cotización" HeaderText="Cotizacion"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="" HeaderText="Opc"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Valor" HeaderText="Valor"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Descuento" HeaderText="Dto(%)"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="VCCD" HeaderText="Valor Neto"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Cliente" HeaderText="Cliente"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="" HeaderText="Contacto"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Obra" HeaderText="Obra"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Plano" HeaderText="Plano"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Fecha_Cotización" HeaderText="F.Cotizacion"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Fecha_Respuesta" HeaderText="F.Respuesta"></asp:BoundColumn>

                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource runat="server" ID="DataGridPorEstado" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="cta_Cotizaciones_Por_Vendedor" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="TextAsesortab2" PropertyName="Text" Name="NombreAsesor" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="TextCotizacionEntreInicio2" PropertyName="Text" DbType="Date" Name="FechaInicio"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="TextCotizacionEntreFinal2" PropertyName="Text" DbType="Date" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <%-- TAB SEGUIMIENTO--%>

            <div class="tab-pane fade" id="Seguimiento-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelSeguimiento">
                    <ContentTemplate>
                       
                        <h6>Criterios para la estadistica</h6>

                   <div class="row">

                       <div class="col-5">
                   
                   <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label CssClass="col-form-label-sm" runat="server">F.Seguimiento Entre:</asp:Label>
                       <asp:TextBox CssClass="form-control" runat="server" Type="date"></asp:TextBox>
                       <asp:Label CssClass="col-form-label-sm" runat="server">y</asp:Label>
                       <asp:TextBox CssClass="form-control" runat="server" Type="Date"></asp:TextBox>

                       </div>
                   </div>

                       <div class="col-5">
                           
                           <div class="input-group input-group-sm mb-2 gap-2">
                               <asp:Label CssClass="col-form-label-sm" runat="server">Asesor:</asp:Label>
                               <asp:TextBox CssClass="form-control" runat="server"></asp:TextBox>
                           </div>

                       </div>

                       <div class="col-2">
                           <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm" runat="server" Text="Consultar" />
                       </div>

                   </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <%--TAB TOTALES--%>

            <div class="tab-pane fade" id="Totales-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelTotales">
                    <ContentTemplate>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <%--TAB EN ESTUDIO--%>

            <div class="tab-pane fade" id="EnEstudio-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelEnEstudio">
                    <ContentTemplate>
                        <h6 class="text-center">RESUMEN DE COTIZACIONES EN ESTUDIO</h6>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <%--TAB ULTIMO CONTACTO--%>

            <div class="tab-pane fade" id="UltimoContacto-content">

                <asp:UpdatePanel runat="server" ID="UpdatePanelUltimoContacto">
                    <ContentTemplate>
                     
                        <div class="row">

                       <div class="col-5">
                   
                   <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label CssClass="col-form-label-sm" runat="server">Ultimo Contacto Comercial</asp:Label>
                       <asp:TextBox CssClass="form-control" runat="server" Type="date"></asp:TextBox>
                       </div>
                   </div>

                       <div class="col-2">
                           <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm" runat="server" Text="Consultar" />
                       </div>

                       <div class="col-2">
                           <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm ms-auto" runat="server" Text="Exportar" />
                       </div>

                       <div class="col-2">
                           <asp:Button CssClass="btn-outline-dark  btn btn-light btn-sm ms-auto" runat="server" Text="UCCM" />
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
