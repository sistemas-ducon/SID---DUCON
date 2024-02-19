<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NitOTs.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.NitOTs" EnableEventValidation="false" %>

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
    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/NitOts.css" rel="stylesheet" />
    <title>Cliente Obra</title>
     <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">

                    <li class="nav-item active">
                        <a class="nav-link text-dark" id="Cliente-tab" data-bs-toggle="tab" href="#Cliente-content">Cliente</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="ConFac-tab" data-bs-toggle="tab" href="#ConFac_content">Contacto Factura</a>
                    </li>

                </ul>
            </div>
        </nav>

        <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade show active" id="Cliente-content">
                <asp:UpdatePanel runat="server" ID="PanelCliente" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="modal fade" id="myModal" tabindex="-1">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger text-white">
                                        <h5 class="modal-title">Error de Consulta</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        <p class="text-center">
                                            !Ups! 
                                                <asp:Literal ID="MensajeError" runat="server"></asp:Literal>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="myModal1" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-xl ">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Compartir Clientes</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>

                                    <div class="modal-body">
                                        <div class="row justify-content-center mb-3">
                                            <div class="border rounded p-2">
                                                <div class="row">
                                                    <div class="col-6">
                                                        <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                            <h5 class="datagrid-header text-center">Asesores</h5>
                                                            <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridCompartirCliente" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="CargarClientes">
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="...">
                                                                        <ItemTemplate>
                                                                            <a href="#" class="btn btn-link" onclick="compartirAsesor(<%# Container.ItemIndex %>); return false;">
                                                                                <i class="bi bi-pencil-square"></i>
                                                                            </a>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:BoundColumn DataField="Asesor" HeaderText="Nombre Asesor" ItemStyle-CssClass="auto-width-column" />

                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource ID="CargarClientes" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select  concat(Nombre , ' ' , Apellidos) as Asesor from tblAsesorComercial where Activo=1 order by concat(Nombre , ' ' , Apellidos)"></asp:SqlDataSource>

                                                        </div>
                                                    </div>

                                                    <div class="col-6">
                                                        <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                            <h5 class="datagrid-header text-center">Asesores Compartidos </h5>
                                                            <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridClienteCompart" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="...">
                                                                        <ItemTemplate>
                                                                            <a href="#" class="btn btn-link" onclick="EliminarAsesor(<%# Container.ItemIndex %>); return false;">
                                                                                <i class="bi bi-pencil-square"></i>
                                                                            </a>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:BoundColumn HeaderText="Nombres Asesor" DataField="Nombre" />


                                                                </Columns>
                                                            </asp:DataGrid>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-6">
                                                <div class="input-group input-group-md  mb-2 gap-4">
                                                    <asp:Label ID="lbNombreAsesor3" class="form-label" Text="Asesor:" runat="server"></asp:Label>
                                                    <asp:TextBox CssClass="form-control" ID="tbNombreAsesor3" runat="server" disabled="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="row pt-1 mt-1">
                                            <div class="col-12" style="text-align: right">
                                                <asp:Button class="btn btn-danger " ID="btnElimnarCompartir" runat="server" UseSubmitBehavior="false" Text="Eliminar" OnClick="EliminarAsesorCompartir" />
                                            </div>
                                        </div>


                                    </div>

                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                        <asp:Button ID="btnGuardarCompartir" class="btn btn-primary" runat="server" UseSubmitBehavior="false" Text="Agregar" OnClick="AgregarAsesorCompartir" />
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="container-fluid">

                            <div class="row d-flex justify-content-center p-1 m-1 ">

                                <div class="col-6">
                                </div>

                                <div class="col-6">
                                    <div class=" input-group input-group-sm gap-2  ">
                                        <asp:TextBox ID="tbNom" type="Text" class="form-control " runat="server" placeholder="Buscar Nombre" AutoPostBack="true"></asp:TextBox>
                                        <asp:TextBox ID="tbNom1" type="Text" class="form-control " runat="server" AutoPostBack="true" placeholder="Buscar Nit"></asp:TextBox>

                                    </div>
                                </div>
                            </div>

                            <div class="row justify-content-center m-1 p-1 pb-2  mb-2" style="height: 12rem">
                                <div class="border rounded pb-1 mb-1">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-1" style="max-height: 11rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Cliente Facturación</h5>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DatagridClientes" runat="server" DataSourceID="ClientesFacturacion" OnItemCommand="DatagridClientes_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkContacto" runat="server" CommandName="VerContactoCliente" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>



                                                        <asp:BoundColumn DataField="Nit" HeaderText="Nit" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="RazonSocial" HeaderText="Razon Social - Primer Apellido" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="SegundoApellido" HeaderText="Segundo Apellido" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Nombre" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Ciudad" HeaderText="Municipio" ItemStyle-CssClass="auto-width-column" />


                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="ClientesFacturacion" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="
                                                                     SELECT Top 300  tblClienteObra.* 
                                                                     From tblClienteObra 
                                                                     WHERE (((tblClienteObra.RazonSocial) Like '%'+ @NITCliente +'%'  
                                                                     and tblClienteObra.Nit Like '%' + @NombreCliente + '%' )) Order By Nombre asc">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNom" PropertyName="Text" Name="NITCliente" DefaultValue="%"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbNom1" PropertyName="Text" Name="NombreCliente" DefaultValue="%"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row justify-content-center m-1 p-1 pb-1 mb-1" style="height: 10rem">
                                <div class="border rounded pb-2 mb-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-1" style="max-height: 9rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Ultimas Ventas</h5>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridVentaAsesor" runat="server" DataSourceID="UltimasVentas">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Obra" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Activo" HeaderText="Activo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Confirmacion_Venta" HeaderText="Ul. Venta" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cedula" HeaderText="Cedula" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="UltimasVentas" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT
                                                                tblAsesorComercial.Nombre+' '+tblAsesorComercial.Apellidos AS Asesor,
                                                                tblOT.Id_OT,
                                                                tblOT.Consecutivo_Pedido,
                                                                tblOT.Nombre_Obra,
                                                                tblAsesorComercial.Activo,
                                                                tblOT.Fecha_Confirmacion_Venta,
                                                                tblAsesorComercial.Cedula 
                                                                FROM tblClienteObraContacto 
                                                                INNER JOIN (tblOT 
                                                                INNER JOIN tblAsesorComercial ON tblOT.Codigo_Asesor = tblAsesorComercial.CodigoAsesor)
                                                                ON tblClienteObraContacto.IdContacto = tblOT.IDContacto_Cliente 
                                                                WHERE (((tblClienteObraContacto.cocNIT)=@NIT)) 
                                                                ORDER BY tblOT.Fecha_Confirmacion_Venta DESC">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNumero" PropertyName="Text" Name="NIT"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row text-center">
                                <span id="ErrorValidacionClienteFact" style="color: red;"></span>
                            </div>

                            <div class="row pt-2 mt-2">
                                <div class="col-1">
                                    <div class=" input-group input-group-sm  ">
                                        <asp:Label ID="lbFechaCreacion" class="form-label" Text="Fecha Creacion" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class=" input-group input-group-sm gap-2  ">

                                        <asp:TextBox ID="tbFechaCreacion" type="date" class="form-control " runat="server"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class=" input-group input-group-sm gap-2   ">
                                        <asp:Label ID="lbUltimaAct" class="form-label" Text="Ultima Actualización" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbUltimaAct" type="date" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class=" input-group input-group-sm gap-2  ">
                                        <asp:Label ID="lbCompartido" Text="compartido Con" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbCompartido" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group-sm   ">
                                        <asp:Label ID="lbCompartir" class="form-label pt-3" Text="Compartir" runat="server"></asp:Label>
                                        <asp:CheckBox ID="chxCompartir" CssClass="pt-3" runat="server" OnCheckedChanged="ChxCompartir_CheckedChanged" AutoPostBack="true" />
                                    </div>

                                </div>
                            </div>

                            <div class="row pt-2">

                                <div class="col-1">
                                    <div class=" input-group input-group-sm  ">
                                        <asp:Label ID="lbNaturaleza" class="form-label" Text="Naturaleza" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm gap-2 ">

                                        <asp:DropDownList class="form-control" ID="ddlNaturaleza" runat="server" onchange="cambioNaturalezaCliente();">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="J - Juridica">J - Juridica</asp:ListItem>
                                            <asp:ListItem Value="N - Persona Natural">N - Persona Natural</asp:ListItem>
                                            <asp:ListItem Value="Pendiente">Pendiente</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbTipoDocumento" class="form-label" Text="Tipo Documento" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlTipoDoc" runat="server">
                                            <asp:ListItem Value="">Seleccione</asp:ListItem>
                                            <asp:ListItem Value="N - NIT">N - NIT</asp:ListItem>
                                            <asp:ListItem Value="C - Cédula">C - Cédula</asp:ListItem>
                                            <asp:ListItem Value="E - Cédula de Extranjeria">E - Cédula de Extranjeria</asp:ListItem>
                                            <asp:ListItem Value="T - Tarjeta Ident">T - Tarjeta Ident.</asp:ListItem>
                                            <asp:ListItem Value="U - NUIP">U - NUIP</asp:ListItem>
                                            <asp:ListItem Value="Pendiente">Pendiente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbNumero" class="form-label" Text="Numero" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbNumero" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbtipoCliente" class="form-label" Text="Tipo Cliente" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlTipoCliente" runat="server">
                                            <asp:ListItem Value="">Seleccione</asp:ListItem>
                                            <asp:ListItem Value="NAL">NAL</asp:ListItem>
                                            <asp:ListItem Value="EXT">EXT</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="row pt-2">

                                <div class="col-1">
                                    <div class=" input-group input-group-sm  ">
                                        <asp:Label ID="lbActividad" class="form-label" Text="Actividad" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm gap-2">

                                        <asp:DropDownList class="form-control" ID="ddlActividad" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class=" input-group input-group-sm gap-2">
                                        <asp:Label ID="lbTelefono" class="form-label" Text="Telefono" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbTelefono" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbSector" class="form-label" Text="Sector" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlSector" runat="server">
                                            <asp:ListItem Value="">Seleccione</asp:ListItem>
                                            <asp:ListItem Value="Agropecuario">Agropecuario</asp:ListItem>
                                            <asp:ListItem Value="Comunicaciones">Comunicaciones</asp:ListItem>
                                            <asp:ListItem Value="Construcción">Construcción</asp:ListItem>
                                            <asp:ListItem Value="Educativo">Educativo</asp:ListItem>
                                            <asp:ListItem Value="Financiero">Financiero</asp:ListItem>
                                            <asp:ListItem Value="Hospitalario">Hospitalario</asp:ListItem>
                                            <asp:ListItem Value="Industrial">Industrial</asp:ListItem>
                                            <asp:ListItem Value="Minero y Energético">Minero y Energético</asp:ListItem>
                                            <asp:ListItem Value="Petrolera">Petrolera</asp:ListItem>
                                            <asp:ListItem Value="Servicios">Servicios</asp:ListItem>
                                            <asp:ListItem Value="Solidario">Solidario</asp:ListItem>
                                            <asp:ListItem Value="Transporte">Transporte</asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbFax" class="form-label" Text="Fax" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbFax" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="row pt-2" id="FilaNombre">

                                <div class="col-1">
                                    <div class=" input-group input-group-sm  ">
                                        <asp:Label ID="lbPriApellido" class="form-label" Text="Primer Apellido" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class=" input-group input-group-sm gap-2">
                                        <asp:TextBox ID="tbPriApellido" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4" runat="server" id="ColSegApell">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbSegApellido" class="form-label" Text="Segundo Apellido" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbSegApellido" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4" runat="server" id="ColNombre">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbNombre" class="form-label" Text="Nombre" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbNombre" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="row pt-2">
                                <div class="col-1">
                                    <div class=" input-group input-group-sm  ">
                                        <asp:Label ID="lbDireccon" class="form-label" Text="Dirección" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class=" input-group input-group-sm gap-2 ">

                                        <asp:TextBox ID="tbDireccion" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbCiudad" class="form-label" Text="Ciudad" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm gap-2  ">
                                        <asp:TextBox ID="tbCod" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbZona" class="form-label" Text="Zona" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlZona" runat="server">
                                            <asp:ListItem Value="">Seleccione</asp:ListItem>
                                            <asp:ListItem Value="COL001">COL001</asp:ListItem>
                                            <asp:ListItem Value="COL002">COL002</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>


                                <div class="col-2">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbFormaPago" class="form-label" Text="Forma Pago" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlFormaPago" runat="server">
                                            <asp:ListItem Value="">Seleccione</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                            <asp:ListItem Value="60">60</asp:ListItem>
                                            <asp:ListItem Value="90">90</asp:ListItem>
                                            <asp:ListItem Value="Cont">Cont</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>

                            </div>

                            <div class="row pt-2">

                                <div class="col-2 pt-3">

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Button ID="btnVerRut" type="button" Style="width: 10rem; min-width: 5rem;" Text="Ver Rut" class="btn btn-outline-secondary" runat="server" OnClick="VerRut"></asp:Button>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <asp:UpdatePanel ID="Archivo" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="input-group input-group-sm">
                                                    <asp:Label ID="lbActRut" runat="server" Text="Actualizar Rut"></asp:Label>
                                                    <asp:FileUpload CssClass="form-control btn btn-secondary" ID="btnActRut" runat="server" Style="width: 18rem" />

                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnGrabar" />
                                            </Triggers>
                                        </asp:UpdatePanel>



                                    </div>
                                </div>

                                <div class="col-2 pt-3">
                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Button ID="btnVerRegCli" type="button" Style="width: 10rem; min-width: 5rem;" Text="Ver Reg Cliente" class="btn btn-outline-secondary" runat="server" OnClick="VerRegistro"></asp:Button>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="input-group input-group-sm">
                                                    <asp:Label ID="Label1" runat="server" Text="Actualizar Registro"></asp:Label>
                                                    <asp:FileUpload CssClass="form-control btn btn-secondary" ID="btnActRegCli" runat="server" Style="width: 18rem" />

                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnGrabar" />
                                            </Triggers>
                                        </asp:UpdatePanel>



                                    </div>

                                </div>

                                <div class="col-8 border">

                                    <h6>Información Tributaria</h6>

                                    <div class="row">

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:CheckBox ID="chxAgenteRete" runat="server" />
                                                <asp:Label ID="lbAgeRet" class="form-label pt-1 " Text="Agente Retenedor" runat="server"></asp:Label>


                                            </div>
                                        </div>

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:CheckBox ID="chxAutoRete" runat="server" />
                                                <asp:Label ID="lbAutRetendor" class="form-label pt-1" Text="AutoRetenedor" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:CheckBox ID="chxDeclarante" runat="server" />
                                                <asp:Label ID="lbDeclaranteRenta" class="form-label pt-1 " Text="Declarante(Renta)" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:Label ID="lbRegimenIva" class="form-label " Text="Regimen de Iva" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:CheckBox ID="chxGranContri" runat="server" />
                                                <asp:Label ID="lbGranContribuyente" class="form-label pt-1" Text="Gran Contribuyente" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:CheckBox ID="chxExento" runat="server" />
                                                <asp:Label ID="lbExcentoRete" class="form-label pt-1" Text="Exento de Retención" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:CheckBox ID="chxReteIca" runat="server" />
                                                <asp:Label ID="lbAgtRetIca" class="form-label pt-1" Text="Agente Retenedor ICA" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-3">
                                            <div class=" input-group input-group-sm gap-2  ">
                                                <asp:DropDownList class="form-control" ID="ddlRegIva" runat="server">
                                                    <asp:ListItem Value="">Seleccione</asp:ListItem>
                                                    <asp:ListItem Value="C - Común">C - Común</asp:ListItem>
                                                    <asp:ListItem Value="S - Simplificado">S - Simplificado</asp:ListItem>
                                                    <asp:ListItem Value="N - No Responsable">N - No Responsable</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>


                                    </div>

                                </div>

                            </div>

                            <div class="row pt-3">

                                <div class="col-6">
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnNuevo" type="button" Style="width: 10rem; min-width: 5rem;" Text="Nuevo" class="btn btn-outline-secondary" runat="server" OnClick="NuevoClienteFact"></asp:Button>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnGrabar" type="button" Style="width: 10rem; min-width: 5rem;" Text="Grabar" class="btn btn-outline-secondary" runat="server" OnClick="GuardarModificarClienteFact" OnClientClick=" return ValidarFormularioCLiente();"></asp:Button>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnModificar" type="button" Style="width: 10rem; min-width: 5rem;" Text="Modificar" class="btn btn-outline-secondary" runat="server" OnClick="MoficarClienteFact"></asp:Button>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnCancelar" type="button" Style="width: 10rem; min-width: 5rem;" Text="Cancelar" class="btn btn-outline-secondary" runat="server" OnClick="CancelarClienteFact"></asp:Button>
                                    </div>
                                </div>

                            </div>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane fade" id="ConFac_content" runat="server">
                <asp:UpdatePanel runat="server" ID="PanelContacto" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container text-bg-warning  text-center ">
                            <asp:Label CssClass=" text-danger" ID="EstMensaje" runat="server" Text="Lo sentimos, no tienes Administración de este cliente." Style="font-size: 2rem; opacity: 0.5;" Visible="false"></asp:Label>
                        </div>

                        <div class="container-fluid p-5" runat="server" id="Contacto">

                            <div class="row justify-content-center mb-5">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Contacto Factura </h5>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridContacto" runat="server" AutoGenerateColumns="false" DataSourceID="ContacoCliente" OnItemCommand="DatagridContactoClienteFact_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkContacto" runat="server" CommandName="VerContactoCliente" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="IdContacto" HeaderText="Id" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocSede" HeaderText="Sede" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocDireccion" HeaderText="Dirección" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocNombre" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocTelefono" HeaderText="Telefono" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocCelular" HeaderText="Celular " ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocMail" HeaderText="Mail " ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocCiudad" HeaderText="Mail" Visible="false" />

                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="ContacoCliente" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblClienteObraContacto where cocNIT= @NIT">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNumero" PropertyName="Text" Name="NIT"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>


                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row text-center pb2 mb-2">
                                <span id="ErrorValidacionContactoFact" style="color: red;"></span>
                            </div>

                            <div class="row mb-2">

                                <div class="col-1">
                                    <div class="input-group input-group-sm   mb-2 gap-4">
                                        <asp:Label ID="lbNombreContacto" class="form-label" Text="Nombre" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm   mb-2 gap-4">

                                        <asp:TextBox ID="tbNombreContacto" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm   mb-2 gap-4">
                                        <asp:Label ID="lbDireccion" class="form-label" Text="Dirección" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbDireccion1" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm   mb-2 gap-4">
                                        <asp:Label ID="lbCiudad1" class="form-label" Text="Ciudad" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlCiudad1" runat="server" class="form-control"></asp:DropDownList>

                                    </div>
                                </div>

                            </div>

                            <div class="row mb-2">
                                <div class="col-1 ">
                                    <div class="input-group input-group-sm   mb-2 gap-4">
                                        <asp:Label ID="lbMail" class="form-label" Text="Mail" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-3 ">
                                    <div class="input-group input-group-sm   mb-2 gap-4">

                                        <asp:TextBox ID="tbMailContacto" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2 ">
                                    <div class="input-group input-group-sm   mb-2 gap-4">
                                        <asp:Label ID="lbTelefono1" class="form-label" Text="Teléfono" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbTelefono1" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3 ">
                                    <div class="input-group input-group-sm   mb-2 gap-4">
                                        <asp:Label ID="lbCelular" class="form-label" Text="Celular" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbCelular" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3 ">
                                    <div class="input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbSede" class="form-label" Text="Sede" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbSede" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="row pt-2 mt-2 d-flex  justify-content-end ">

                                <div class="col-1">
                                    <div class="input-group  mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnNuevoContacto" runat="server" Text="Nuevo" OnClick="NuevoContactoFact" />
                                    </div>
                                </div>


                                <div class="col-1">
                                    <div class="input-group   mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnGrabarContacto" runat="server" Text="Grabar" OnClick="GuardarModificarContactoFact" OnClientClick="return ValidarFormularioContacto();" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group   mb-2 gap-2">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnModificarContacto" runat="server" Text="Modificar" OnClick="MoficarContactoFact" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group   mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancelar1" runat="server" Text="Cancelar" OnClick="CancelarContactoFact" />
                                    </div>
                                </div>


                            </div>


                           

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </form>

    <script type="text/javascript">

        function ValidarFormularioCLiente() {

            var Naturaleza = document.getElementById("ddlNaturaleza").value;
            var TipoDocumento = document.getElementById("ddlTipoDoc").value;
            var Numero = document.getElementById("tbNumero").value;
            var TipoCliente = document.getElementById("ddlTipoCliente").value;
            var Actividad = document.getElementById("ddlActividad").value;
            var Telefono = document.getElementById("tbTelefono").value;
            var Sector = document.getElementById("ddlSector").value;
            var PriApellido = document.getElementById("tbPriApellido").value;
            var SegApellido = document.getElementById("tbSegApellido").value;
            var Nombre = document.getElementById("tbNombre").value;
            var Direccion = document.getElementById("tbDireccion").value;
            var Ciudad = document.getElementById("ddlCiudad").value;
            var Zona = document.getElementById("ddlZona").value;
            var FormaPago = document.getElementById("ddlFormaPago").value;
            var RegIva = document.getElementById("ddlRegIva").value;
            var rutCargado = document.getElementById('<%= btnActRut.ClientID %>');
            var RegCargado = document.getElementById('<%= btnActRegCli.ClientID %>');

            var isValid = true;

            if (Naturaleza === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Naturaleza es obligatorio.";
                isValid = false;
            } else if (TipoDocumento === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Tipo documento es obligatorio.";
                isValid = false;
            } else if (Numero === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Numero  es obligatorio.";
                isValid = false;
            } else if (TipoCliente === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Tipo cliente es obligatorio.";
                isValid = false;
            } else if (Actividad === "0" || Actividad === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Actividad es obligatorio.";
                isValid = false;
            } else if (Telefono === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Telefono es obligatorio.";
                isValid = false;
            } else if (Sector === "Seleccione") {
                ErrorValidacionClienteFact.innerHTML = "El campo Sector es obligatorio.";
                isValid = false;
            } else if (PriApellido === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo primer apellido o razón social  es obligatorio.";
                isValid = false;
            } else if (Direccion === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Direccion es obligatorio.";
                isValid = false;
            } else if (Ciudad === "0") {
                ErrorValidacionClienteFact.innerHTML = "El campo Ciudad es obligatorio.";
                isValid = false;
            } else if (Zona === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Zona es obligatorio.";
                isValid = false;
            } else if (FormaPago === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Forma Pago es obligatorio.";
                isValid = false;
            } else if (RegIva === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Regimen Iva es obligatorio.";
                isValid = false;
            } else if (Naturaleza === "J - Juridica" && Numero.length < 9) {
                ErrorValidacionClienteFact.innerHTML = "El campo Número debe tener al menos 9 caracteres para Naturaleza Jurídica.";
                isValid = false;
            } else if (Naturaleza === "N - Persona Natural" && SegApellido === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo Segundo apellido es obligario.";
                isValid = false;
            } else if (Naturaleza === "N - Persona Natural" && Nombre === "") {
                ErrorValidacionClienteFact.innerHTML = "El campo  nombre es obligario.";
                isValid = false;
            } else if (rutCargado.value === "") {
                ErrorValidacionClienteFact.innerHTML = "Por favor, Adjunte Rut.";
                isValid = false;
            } else if (RegCargado.value === "") {
                ErrorValidacionClienteFact.innerHTML = "Por favor, Adjunte Registro del cliente.";
                isValid = false;
            }



            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
        }

        function ValidarFormularioContacto() {
            var isValid = true;

            var NombreContacto = document.getElementById("tbNombreContacto").value;
            var Direccion1 = document.getElementById("tbDireccion1").value;
            var Ciudad1 = document.getElementById("ddlCiudad1").value;
            var MailContacto = document.getElementById("tbMailContacto").value;
            var Telefono1 = document.getElementById("tbTelefono1").value;

            if (NombreContacto === "") {
                ErrorValidacionContactoFact.innerHTML = "El campo Nombre es obligatorio.";
                isValid = false;
            } else if (Direccion1 === "") {
                ErrorValidacionContactoFact.innerHTML = "El campo Direccion es obligatorio.";
                isValid = false;
            } else if (Ciudad1 === "") {
                ErrorValidacionContactoFact.innerHTML = "El campo Ciudad  es obligatorio.";
                isValid = false;
            } else if (MailContacto === "") {
                ErrorValidacionContactoFact.innerHTML = "El campo Mail es obligatorio.";
                isValid = false;
            } else if (Telefono1 === "" ) {
                ErrorValidacionContactoFact.innerHTML = "El campo Telefono es obligatorio.";
                isValid = false;
            }
            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
        }

    </script>

    <script type="text/javascript">
        function openModal() {
            var myModal = new bootstrap.Modal(document.getElementById('myModal'), {
                keyboard: false
            });
            myModal.show();

            // Agrega la funcionalidad de mover el modal cuando se pasa el puntero sobre él
            $('#myModal').hover(function () {
                $(this).css({
                    'margin-top': Math.random() * 100,
                    'margin-left': Math.random() * 100
                });
            });

            // Código para cerrar el modal después de 2 segundos
            setTimeout(function () {
                myModal.hide();
            }, 3000);
        }
    </script>

    <script type="text/javascript">
        function cambioNaturalezaCliente() {
            var ddlNaturaleza = document.getElementById('<%= ddlNaturaleza.ClientID %>');

            if (ddlNaturaleza.value === "J - Juridica") {
                NaturalezaJur();
            } else if (ddlNaturaleza.value === "N - Persona Natural" || ddlNaturaleza.value === "Pendiente") {
                NaturalezaPersona();
            }
        }

        function NaturalezaJur() {
            var ColSegApell = document.getElementById('ColSegApell');
            var ColNombre = document.getElementById('ColNombre');

            ColSegApell.style.display = 'none';
            ColNombre.style.display = 'none';

            // Cambiar el texto de un label
            var miLabel = document.getElementById('lbPriApellido');
            if (miLabel) {
                miLabel.innerText = 'Razón Social';
            }
        }

        function NaturalezaPersona() {
            var ColSegApell = document.getElementById('ColSegApell');
            var ColNombre = document.getElementById('ColNombre');

            ColSegApell.style.display = 'block';
            ColNombre.style.display = 'block';

            // Cambiar el texto de un label
            var miLabel = document.getElementById('lbPriApellido');
            if (miLabel) {
                miLabel.innerText = 'PrimerApellido';
            }
        }
    </script>

    <script>
        function compartirAsesor(rowIndex) {
            // Convertir el índice de fila a cero basado en lugar de uno basado
            var adjustedRowIndex = rowIndex + 1;

            // Obtener el nombre y la cédula del asesor de la fila correspondiente en el DataGrid
            var nombreAsesor = $('#<%= DataGridCompartirCliente.ClientID %> tr:eq(' + adjustedRowIndex + ') td:eq(1)').text();


            // Mostrar el nombre y la cédula del asesor en TextBoxes correspondientes
            $('#<%= tbNombreAsesor3.ClientID %>').val(nombreAsesor);

            document.getElementById("btnElimnarCompartir").classList.add("disabled");
            document.getElementById("btnGuardarCompartir").classList.remove("disabled");
        }

        function EliminarAsesor(rowIndex) {
            // Convertir el índice de fila a cero basado en lugar de uno basado
            var adjustedRowIndex = rowIndex + 1;

            // Obtener el nombre y la cédula del asesor de la fila correspondiente en el DataGrid
            var nombreAsesor = $('#<%= DataGridClienteCompart.ClientID %> tr:eq(' + adjustedRowIndex + ') td:eq(1)').text();


            // Mostrar el nombre y la cédula del asesor en TextBoxes correspondientes
            $('#<%= tbNombreAsesor3.ClientID %>').val(nombreAsesor);

            document.getElementById("btnGuardarCompartir").classList.add("disabled");
            document.getElementById("btnElimnarCompartir").classList.remove("disabled");

        }

        function check() {
            document.getElementById("CheckBox1").classList.remove("disabled");
        }

        function enviarFormulario() {
            // Realiza el procesamiento necesario en el formulario 2

            // Actualiza el formulario 1
            window.opener.location.reload(); // Recarga el formulario padre

        }


    </script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
