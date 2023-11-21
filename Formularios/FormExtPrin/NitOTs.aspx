<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NitOTs.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.NitOTs" %>

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
                                                <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DatagridClientes" runat="server" DataSourceID="ClientesFacturacion" OnItemCommand="DatagridClientes_LinkButton">
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
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="ClientesFacturacion" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="
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
                                                <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridVentaAsesor" runat="server" DataSourceID="UltimasVentas">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Obra" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Activo" HeaderText="Activo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Confirmacion_Venta" HeaderText="Ul. Venta" ItemStyle-CssClass="auto-width-column" />

                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="UltimasVentas" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT
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

                            <div class="row pt-2 mt-2">
                                <div class="col-3">
                                    <div class=" input-group input-group-sm gap-2  ">
                                        <asp:Label ID="lbFechaCreacion" class="form-label" Text="Fecha Creacion" runat="server"></asp:Label>
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
                                        <asp:CheckBox ID="chxCompartir" CssClass="pt-3" runat="server" />
                                    </div>

                                </div>
                            </div>

                            <div class="row pt-2">
                                <div class="col-3">
                                    <div class=" input-group input-group-sm gap-2 ">
                                        <asp:Label ID="lbNaturaleza" class="form-label" Text="Naturaleza" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlNaturaleza" runat="server">
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

                                <div class="col-3">
                                    <div class=" input-group input-group-sm gap-2">
                                        <asp:Label ID="lbActividad" class="form-label" Text="Actividad" runat="server"></asp:Label>
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
                                            <asp:ListItem Value="Seleccione"></asp:ListItem>
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

                                <div class="col-4">
                                    <div class=" input-group input-group-sm gap-2">
                                        <asp:Label ID="lbPriApellido" class="form-label" Text="Primer Apellido" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbPriApellido" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbSegApellido" class="form-label" Text="Segundo Apellido" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbSegApellido" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbNombre" class="form-label" Text="Nombre" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbNombre" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="row pt-2" id="FilaRazonSocial" style="display: none">
                                <div class="col-6">
                                    <div class=" input-group input-group-sm gap-2 ">
                                        <asp:Label ID="lbRaonsocial" class="form-label" Text="Razón Social" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbRazonSocial" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row pt-2">

                                <div class="col-3">
                                    <div class=" input-group input-group-sm gap-2 ">
                                        <asp:Label ID="lbDireccon" class="form-label" Text="Dirección" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbDireccion" type="Text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm gap-2">
                                        <asp:Label ID="lbCiudad" class="form-label" Text="Ciudad" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" ></asp:DropDownList>
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
                                            <asp:Button ID="btnVerRut" type="button" Style="width: 10rem; min-width: 5rem;" Text="Ver Rut" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Button ID="btnActRut" type="button" Style="width: 10rem; min-width: 5rem;" Text="Actualizar Rut" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-2 pt-3">
                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Button ID="btnVerRegCli" type="button" Style="width: 10rem; min-width: 5rem;" Text="Ver Reg Cliente" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Button ID="btnActRegCli" type="button" Style="width: 10rem; min-width: 5rem;" Text="Act Reg Cliente" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                        </div>
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
                                                <asp:Label ID="lbDeclranteRenta" class="form-label pt-1 " Text="Declarante(Renta)" runat="server"></asp:Label>
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
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnNuevo" type="button" Style="width: 10rem; min-width: 5rem;" Text="Nuevo" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnGrabar" type="button" Style="width: 10rem; min-width: 5rem;" Text="Grabar" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnModificar" type="button" Style="width: 10rem; min-width: 5rem;" Text="Modificar" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Button ID="btnCancelar" type="button" Style="width: 10rem; min-width: 5rem;" Text="Cancelar" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="tab-pane fade" id="ConFac_content"  runat="server" >
                <asp:UpdatePanel runat="server" ID="PanelContacto" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container-fluid p-5">

                            <div class="row justify-content-center mb-5">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Contacto Factura </h5>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridContacto" runat="server" AutoGenerateColumns="false" DataSourceID="ContacoCliente">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkContacto" runat="server" CommandName="VerContacto" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="IdContacto" HeaderText="Id" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocSede" HeaderText="Sede" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocDireccion" HeaderText="Dirección" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocNombre" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocTelefono" HeaderText="Telefono" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocCelular" HeaderText="Celular " ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="cocMail" HeaderText="Mail " ItemStyle-CssClass="auto-width-column" />

                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="ContacoCliente" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT * FROM tblClienteObraContacto where cocNIT= @NIT">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNumero" PropertyName="Text" Name="NIT"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>


                                            </div>


                                        </div>
                                    </div>
                                </div>
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
                                        <asp:TextBox ID="tbDirección1" type="text" class="form-control " runat="server"></asp:TextBox>
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
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnNuevoContacto" runat="server" Text="Nuevo" />
                                    </div>
                                </div>


                                <div class="col-1">
                                    <div class="input-group   mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnGrabarContacto" runat="server" Text="Grabar" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group   mb-2 gap-2">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnModificarContacto" runat="server" Text="Modificar" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group   mb-2 ">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancelar1" runat="server" Text="Cancelar" />
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
        function ocultarMostrarFilas() {
            var filaNombre = document.getElementById('FilaNombre');
            var filaRazonSocial = document.getElementById('FilaRazonSocial');

            if (filaNombre && filaRazonSocial) {
                filaNombre.style.display = 'none';
                filaRazonSocial.style.display = 'block';
            }
        }
    </script>



    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
