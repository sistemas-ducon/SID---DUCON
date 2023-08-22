<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.Clientes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/Clientes.css" />
</head>
<body>

    <nav class="navbar navbar-light bg-light">
        <div class="container d-flex justify-content-center ">
            <ul class="nav nav-tabs gap-5">


                <li class="nav-item">
                    <a class="nav-link text-dark active" id="Cliente-tab" data-bs-toggle="tab" href="#Cliente-content">Cliente</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" id="Contacto-tab" data-bs-toggle="tab" href="#Contacto-content">Contacto</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark " id="Consulta-tab" data-bs-toggle="tab" href="#Consulta-content">Consultas </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" id="ClienteNuevo-tab" data-bs-toggle="tab" href="#ClienteNuevo-content">Clientes Nuevos</a>
                </li>
            </ul>
        </div>
    </nav>


    <form id="Clientes" runat="server">
        <asp:ScriptManager runat="server" />

        <div class="tab-content">

            <div class="tab-pane fade show active" id="Cliente-content">
                <asp:UpdatePanel ID="PanelCliente" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid mt-3 p-4  ">

                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Clientes </h5>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridCliente" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemCommand="DataGridCliente_ItemCommand" DataSourceID="ListarClientes">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkClie" runat="server" CommandName="VerCliente" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="Nit" HeaderText="Nit" />
                                                        <asp:BoundColumn DataField="Nombre_Compañia" HeaderText="Nombre Compañia" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="AsesorComercial" HeaderText="Asesor Comercial " ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="FCreación" HeaderText="Fecha Creacion" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Teléfono" ItemStyle-CssClass="d-none" />
                                                        <asp:BoundColumn DataField="Dirección" ItemStyle-CssClass="d-none" />
                                                        <asp:BoundColumn DataField="Procedencia" ItemStyle-CssClass="d-none" />
                                                        <asp:BoundColumn DataField="CompartidoCon" ItemStyle-CssClass="d-none" />

                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource runat="server" ID="ListarClientes" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ObtenerClientes1" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

                                                <asp:SqlDataSource ID="ListarClientesXNit" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ClientesConFiltro" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNitBuscar" PropertyName="Text" Name="IdCliente" Type="String"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="ListarClientesXNombre" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ClientesXNombre" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreBuscar" PropertyName="Text" Name="NombreCompania" Type="String"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3 mb-2">
                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbNit" class="form-label" Text="Nit" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbNit" type="text" class="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbNombreCliente" class="form-label" Text="Cliente" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbNombreCliente" type="text" class="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbTelefono" class="form-label" Text="Telefono" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbTelefono" type="text" class="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <asp:Label class="form-label" Text="Procedencia" runat="server" ID="lbProcedencia"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlprocedencia" runat="server" DataTextField="Procedencia" DataValueField="Procedencia" DataSourceID="CargarProcedencias" OnDataBound="ddlProcedencia_DataBound" disabled="true"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="CargarProcedencias" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select * from tblProcedenciaCliente"></asp:SqlDataSource>
                                    </div>
                                </div>

                            </div>

                            <div class="row mb-2">
                                <div class="col-6">
                                    <div class="input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbDireccion" class="form-label" Text="Direccion" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbDireccion" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-6">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <asp:Label class="form-label" Text="Ciudad" runat="server" ID="lbCiudad"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlCiudaX" runat="server" DataTextField="NombreCiudad" DataValueField="NombreCiudad" DataSourceID="CargarCiudades" OnDataBound="ddlCiudadX_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="CargarCiudades" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT 
                                            CONCAT(tblDepartamentoPais.CodigoDepartamento ,
                                            tblCiudad.CodigoCiudad)   AS CodCompleto,
                                            tblCiudad.NombreCiudad +'/'+tblDepartamentoPais.NombreDepartamento As NombreCiudad
                                            FROM tblDepartamentoPais 
                                            INNER JOIN tblCiudad
                                            ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento 
                                            ORDER BY CONCAT(tblCiudad.NombreCiudad , ' - ' , tblDepartamentoPais.NombreDepartamento)"></asp:SqlDataSource>
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-2">
                                <div class="col-7">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbCompartido" class="form-label" Text="Compartido Con:" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbCompartido" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-5">
                                    <div class="input-group input-group-sm  mb-2 gap-2 text-end ">
                                        <asp:CheckBox ID="CheckBox1" runat="server" Enabled="false" />
                                        <asp:Label ID="chxCompartir" class="form-label" Text="Compartir:" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="row justify-content-between">
                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbNitBscar" class="form-label" Text="Nit" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbNitBuscar" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbNombreBuscar" class="form-label" Text="Nombre" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbNombreBuscar" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="Buscar" runat="server" Text="Buscar" OnClick="ConsultarCliente" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="Nuevo" runat="server" Text="Nuevo" OnClick="NuevoCliente" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="Modificar" runat="server" Text="Modificar" Enabled="false"  OnClick="ModificarCliente"/>
                                    </div>
                                </div>


                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="Grabar" runat="server" Text="Grabar" Enabled="false" OnClick="btn_GuardarCliente" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="Eliminar" runat="server" Text="Eliminar" Enabled="false" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="Cancelar" runat="server" Text="Cancelar" OnClick="CancelarBot" />
                                    </div>
                                </div>



                            </div>


                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <div class="tab-pane fade  " id="Contacto-content">
                <asp:UpdatePanel ID="PanelContacto" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid p-5">

                            <div class="row justify-content-center mb-5">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Nombres Contactos Cliente </h5>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridContacto" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemCommand="DataGridContacto_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkContacto" runat="server" CommandName="VerContacto" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="NombreContacto" HeaderText="Nombre Contacto" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Telefono" HeaderText="Telefono" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Celular" HeaderText="Celular " ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="MailContacto" HeaderText="Mail " ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_ClienteContacto" ItemStyle-CssClass="d-none"   />
                                                    </Columns>
                                                </asp:DataGrid>


                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-2">

                                <div class="col-1" style="padding-right: 8.1rem">
                                    <div class="input-group   mb-2 gap-4">
                                        <asp:Label ID="Label4" class="form-label" Text="Nombre Contacto" runat="server"></asp:Label>

                                    </div>
                                </div>


                                <div class="col-3">
                                    <div class="input-group   mb-2 gap-4">

                                        <asp:TextBox ID="tbNombreContacto" type="text" class="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group   mb-2 gap-4">
                                        <asp:Label ID="lbTelefonoContacto" class="form-label" Text="Teléfono" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbTelefonoContacto" type="text" class="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group   mb-2 gap-4">
                                        <asp:Label ID="lbCelularContacto" class="form-label" Text="Celular" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbCelularContacto" type="text" class="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="row mb-2">

                                <div class="col-1" style="padding-right: 8.1rem">
                                    <div class="input-group   mb-2 gap-4">
                                        <asp:Label ID="Label2" class="form-label" Text="Mail" runat="server"></asp:Label>

                                    </div>
                                </div>



                                <div class="col-4 ">
                                    <div class="input-group   mb-2 gap-4">

                                        <asp:TextBox ID="tbMailContacto" type="text" class="form-control " runat="server" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1 ">
                                    <div class="input-group   mb-2 gap-4">
                                        <asp:CheckBox ID="chkEstadoGuardar" runat="server" Enabled="false"  Visible="false"   />
                                    </div>
                                </div>

                                <div class="col-1 ">
                                    <div class="input-group   mb-2 gap-4">
                                        <asp:TextBox ID="tbId_ContactoCliente" type="text" class="form-control" runat="server" Visible="false"  ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>


                            </div>

                            <div class="row ">
                                <div class="col-1" style="padding-right: 8.1rem">
                                    <div class="input-group  mb-2 gap-4">
                                        <asp:Label ID="Label3" class="form-label" Text="Buscar Nombre" runat="server"></asp:Label>
                                    </div>
                                </div>


                                <div class="col-3">
                                    <div class="input-group   mb-2 gap-4">

                                        <asp:TextBox ID="tbBuscarContacto" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1">
                                </div>

                                <div class="col-1">
                                    <div class="input-group  mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnNuevoContacto" runat="server" Text="Nuevo" Enabled="false" OnClick="btnNuevoContacto_Click" />
                                    </div>
                                </div>


                                <div class="col-1">
                                    <div class="input-group   mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnGrabarContacto" runat="server" Text="Grabar" Enabled="false" OnClick="btnGuardarContacto_Click" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group   mb-2 gap-2">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnModificarContacto" runat="server" Text="Modificar" Enabled="false" OnClick="btnModificarContacto_Click" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group   mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancelar" runat="server" Text="Cancelar" Enabled="false" OnClick="btnCancelarContacto_Click" />
                                    </div>
                                </div>


                            </div>


                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade  " id="Consulta-content">
                <asp:UpdatePanel ID="PanelConsulta" runat="server">
                    <ContentTemplate>
                        <div class="container p-1 mt-4">
                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Cotizaciones </h5>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridCotizacion" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCotizacion" runat="server" CommandName="VerCoticacion" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="NombreAsesor" HeaderText="Asesor Comercial" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cotización" HeaderText="Cotización" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Cotización" HeaderText="Fecha Cotización" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Valor" HeaderText="Valor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripción_Estado" HeaderText="Estado Cotización" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid>


                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row" style="height:4rem"></div>

                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Visitas </h5>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridVisita" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkVisitas" runat="server" CommandName="VerVisita" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="NombreAsesor" HeaderText="Asesor Comercial" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="FechaVisita" HeaderText="Fecha Visita" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="NombreCausa" HeaderText="Causa Visita" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="NombreContacto" HeaderText="Contacto" ItemStyle-CssClass="auto-width-column" />

                                                    </Columns>
                                                </asp:DataGrid>


                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade" id="ClienteNuevo-content">
                <asp:UpdatePanel ID="PanelClienteNuevo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container p-1">

                            <div class="row pt-4 mb-3">

                                <div class="col-7">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="Label1" class="form-label" Text="Fecha Consulta: " runat="server"></asp:Label>
                                        <asp:TextBox ID="FechaI" type="date" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label ID="lbAl" class="form-label" Text=" Al " runat="server"></asp:Label>
                                        <asp:TextBox ID="FechaF" type="date" runat="server" class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2">

                                        <asp:Button ID="bntConsultar" type="button" Text="Consultar" class="btn btn-outline-secondary"
                                            runat="server" OnClick="ConsultarClienteFecha"></asp:Button>
                                    </div>
                                </div>

                                <div class="col-1">
                                </div>

                                <div class="col-1">
                                    <asp:LinkButton ID="CrearExelClientes" runat="server" OnClick="CrearExcel">
                                      <i class="custom-icon2"></i>
                                    </asp:LinkButton>
                                </div>


                            </div>

                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row pt-2">
                                        <div class="col-12">
                                            <div class=" table-responsive mb-2 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Clientes </h5>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridClienteFecha" runat="server" DataSourceID="ClientexFecha" AutoGenerateColumns="false">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkClieFecha" runat="server" CommandName="..." CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="Fecha_Creacion" HeaderText="Fecha Creacion" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_Cliente" HeaderText="Nit" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="NombreCompañía" HeaderText="Nombre Cliente" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Teléfono" HeaderText="Telefono" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Correo_Electronico" HeaderText="Correo Electronico" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="NombreAsesor" HeaderText="Asesor Comercial" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="ClientexFecha" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ClientesNuevoXFecha" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="FechaI" PropertyName="Text" DbType="Date" Name="FechaIncial"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaF" PropertyName="Text" DbType="Date" Name="FechaFinal"></asp:ControlParameter>
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




        </div>
    </form>

  

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>


</body>
</html>
