<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IngresarCotizacion.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.IngresarCotizacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
     <link type="text/css" href="../../Recursos/CSS/Ventas/IngresarCotizacion.css" rel="stylesheet" />
    <title>Ingresar Cotizacion</title>
</head>
<body translate="no">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       

               <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="Cotizaciones-tab" data-bs-toggle="tab" href="#Cotizaciones-content">Cotizaciones</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Resumen_Cotizaciones-tab" data-bs-toggle="tab" href="#ResumenCotizaciones-content">Resumen Cotizaciones</a>
                    </li>

                </ul>
            </div>
               </nav>

        <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade show active" id="Cotizaciones-content">
                <asp:UpdatePanel runat="server" ID="UpdateCotizacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container-fluid">
                            <nav class="navbar navbar-expand-sm navbar-light bg-light gap-2">
                                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                    <span class="navbar-toggler-icon"></span>
                                </button>

                                <%-- BOTONES--%>
                                <div class="collapse navbar-collapse" id="ejemplo2">
                                    <ul class="navbar-nav mx-auto contenedor-icono">
                                        <div class="contenedor-icono">

                                            <asp:LinkButton runat="server" ID="NuevaCot" OnClick="NuevaCot_Clik">
                                               <i class="bi bi-file-earmark-plus-fill"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="GuardarCot" OnClick="Grabar_Click">
                                                 <i class="bi bi-save-fill"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="ModificarCot">
                                              <i class="bi bi-wrench-adjustable"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="EliminarCot">
                                               <i class="bi bi-trash-fill"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="CancelarCot" OnClick="Cancelar_Click">
                                                <i class="bi bi-x-circle-fill"></i>
                                            </asp:LinkButton>
                                    </ul>
                                </div>
                            </nav>
                          <div id="d-flex" class="d-flex">
                                <div class="col-lg-9 col-md-12 col-sm-12 col-xs-12">
                                    <div class="p-3 m-2 border bg-light" style="height: 21rem;">
                                        <h6>Información Cotización</h6>
                                        <div class="row">
                                            <div class="col-lg-1 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label class="form-label" Text="Zona" runat="server" ID="lbZona"></asp:Label>
                                                    <asp:DropDownList class="form-control form-control-sm" ID="ddlZona" runat="server">
                                                        <asp:ListItem Value=""></asp:ListItem>
                                                        <asp:ListItem Value="01">01</asp:ListItem>
                                                        <asp:ListItem Value="02">02</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-lg-8 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label class="form-label" runat="server" ID="txtAsesor" Text="Asesor"></asp:Label>
                                                   <asp:DropDownList ID="ddlAsesor" class="form-control form-control-sm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAsesor_SelectedIndexChanged" EnableViewState="true"></asp:DropDownList>


                                                </div>
                                            </div>

                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <div class="input-group input-group-sm mb-2 gap-2">
                                                        <asp:Label class="form-label" Text="Cotizacion" runat="server" ID="lblCotizacion"></asp:Label>
                                                        <asp:TextBox runat="server" ID="textCotizacion" CssClass="form-control form-control-sm" OnTextChanged="txtCotizacion_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-9 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Button runat="server" Text="Cliente" CssClass="btn btn-outline-dark" ID="BtnCliente" OnClick="BtnCliente_Click"/>
                                                    <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label class="form-label form-label" Text="Diseño" runat="server" ID="Label1"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control form-control-sm"></asp:TextBox>
                                                      <asp:LinkButton runat="server" ID="LinkButton5" CssClass="btn shadow btn-light linkButtonClicked2">
                                                          <i class="bi bi-arrow-down-circle-fill"></i>
                                                      </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-9 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblContacto" Text="Contacto"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextContacto" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label runat="server" CssClass="form-label" ID="LblTelefono" Text="Teléfono"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextTelefono" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-9 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-5">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblMail" Text="Mail"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextMail" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblEstado" Text="Estado"></asp:Label>
                                                    <asp:DropDownList ID="DropDownListEstado" class="form-control form-control-sm" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-4">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblCompe" Text="Compe"></asp:Label>
                                                    <asp:DropDownList ID="ddlCompeData" runat="server" class="form-control" DataSourceID="CompeDataS" DataTextField="NombreCompetencia" DataValueField="ID_Competencia">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="CompeDataS" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblCompetencia"></asp:SqlDataSource>
                            
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblCausa" Text="Causa"></asp:Label>
                                                    <asp:DropDownList ID="ddlCausa" runat="server" class="form-control" DataSourceID="CausaDataS" DataTextField="CausaRechazoCotizacion" DataValueField="ID_CausaRechazoCotizacion">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="CausaDataS" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblCausadeCotizacionRechazada"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblFcot" Text="F.cot"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextFcot" CssClass="form-control form-control-sm" type="date"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblFrta" Text="F.Rta"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextFrta" CssClass="form-control form-control-sm" type="date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-4">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblPlano" Text="Plano"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextPlano" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-9 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblObs" Text="Obs."></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextObs" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="lblProyecto" Text="Proyecto"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextProyecto" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-12 col-sm-12 col-xs-12">
                                    <div class="p-3 m-2 border bg-light" style="height: 21rem;">
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label2" Text="VVSU"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox3" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label3" Text="VCSD"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox4" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label4" Text="D.Com"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox5" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label10" Text="D.Fact"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox11" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label5" Text="VMO"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox6" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label6" Text="VCCD"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox7" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label7" Text="VTTE"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox8" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label8" Text="VIA"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox9" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-3">
                                                    <asp:Label runat="server" CssClass="form-label" ID="Label9" Text="TOTAL"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBox10" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="p-3 m-2 border" style="height: 10rem;">

                                         <div class="table-responsive mb-2 gap-2" style="max-height: 9rem; overflow-x: auto;">
                                         <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGrid2" runat="server"
                                                                AutoGenerateColumns="false">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>

                                                                      <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="BtnSelec2" runat="server" 
                                                                                    CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-dark'></i>"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                              
                                                                    <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Cotizacion" HeaderText="Cotización" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Precio_Venta" HeaderText="Venta" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Descuento" HeaderText="Dcto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                                                                                                           
                                                                   <asp:BoundColumn DataField="DescuentoparaComision" HeaderText="Dcto.Com" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ValorBolsa" HeaderText="Valor Tte" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                                                                                            
                                                                </Columns>
                                                            </asp:DataGrid>
                                              </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="p-3 m-2 border" style="height: 15rem;">
                                         <div class="table-responsive mb-2 gap-2" style="max-height: 14rem; overflow-x: auto;">
                                         <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGrid1" runat="server"
                                                                AutoGenerateColumns="false">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>

                                                                      <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="BtnSelec" runat="server" OnClick="lnkSelectRow_Click"
                                                                                    CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-dark'></i>"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                              
                                                                    <asp:BoundColumn DataField="Cotización" HeaderText="Cotización" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Estado" HeaderText="Estado" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Valor" HeaderText="Valor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cliente" HeaderText="Cliente" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Obra" HeaderText="Obra" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                                                                                                           
                                                                   <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Cotización" HeaderText="F.Cotización" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Respuesta" HeaderText="F.Respuesta" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="Plano" HeaderText="Plano" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="NombreCompetencia" HeaderText="Competencia" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="Contacto_Cotizacion" HeaderText="Contacto Cotizacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="CausaRechazoCotizacion" HeaderText="Causa de Rechazo" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
    SelectCommand="SELECT TOP 400 tblCotización.*, tblEstado_Cotización.Descripción_Estado AS Estado, CONCAT(tblAsesorComercial.Nombre, ' ', tblAsesorComercial.Apellidos) AS Asesor, tblCompetencia.NombreCompetencia, tblCausadeCotizacionRechazada.CausaRechazoCotizacion, tblCliente.NombreCompañía AS Cliente2 FROM tblAsesorComercial INNER JOIN (tblEstado_Cotización INNER JOIN (tblCompetencia INNER JOIN (tblCliente INNER JOIN (tblCausadeCotizacionRechazada INNER JOIN tblCotización ON tblCausadeCotizacionRechazada.ID_CausaRechazoCotizacion = tblCotización.ID_CausaRechazoCotizacion) ON tblCliente.Id_Cliente = tblCotización.Cliente) ON tblCompetencia.ID_Competencia = tblCotización.ID_Competencia) ON tblEstado_Cotización.Id_Estado = tblCotización.Estado) ON tblAsesorComercial.Cedula = tblCotización.Asesor">
</asp:SqlDataSource>

                                              </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-2">
                                <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="form-label" ID="Label11" Text="Cotización"></asp:Label>
                                        <asp:TextBox runat="server" ID="TextBox12" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6 col-sm-12 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="form-label" ID="Label12" Text="Asesor"></asp:Label>
                                        <asp:DropDownList ID="DropDownListAsesor" class="form-control form-control-sm" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-md-6 col-sm-12 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="form-label" ID="Label13" Text="Plano"></asp:Label>
                                        <asp:TextBox runat="server" ID="TextBox14" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-3">
                                        <asp:Label runat="server" CssClass="form-label" ID="Label14" Text="Nit"></asp:Label>
                                        <asp:TextBox runat="server" ID="TextBox15" CssClass="form-control form-control-sm gap-3"></asp:TextBox>
                                      <asp:LinkButton runat="server" ID="BtnPuntos" CssClass="btn shadow btn-light linkButtonClicked2" Text="..." OnClick="Buscar_Click"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>



            <div class="tab-pane fade" id="ResumenCotizaciones-content">
                <asp:UpdatePanel runat="server" ID="UpdateResumenCotizaciones" UpdateMode="Conditional">
                    <ContentTemplate>

                        <h6>BIENVENIDO</h6>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>

                      <div class="modal fade" id="llenarCliente" data-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">llenar cliente</h5>
                                    </div>
                                    <div class="modal-body form-control-sm">
                                        <p>
                                            Por favor, empiece por diligenciar el asesor comercial
                                        </p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">                        
                                    </div>
                                </div>
                            </div>
                        </div>

        
                        <div class="modal fade" id="ErrorMCotizacion" data-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Ver cotización pedido</h5>
                                         <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body form-control-sm">
                                        <p>
                                           El sistema no puede encontrar la cotizacion en los 6 meses anteriores.
                                        </p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                      
                                    </div>
                                </div>
                            </div>
                        </div>

          <div class="modal fade" id="campoFaltante" data-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">NIT</h5>
                                    </div>
                                    <div class="modal-body form-control-sm">
                                          <p>Falta llenar el campo: <span id="campoFaltante2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                       
                                    </div>
                                </div>
                            </div>
                        </div>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
