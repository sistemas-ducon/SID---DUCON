<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObjetosDibujo.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo.ObjetosDibujo" %>

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

    <link type="text/css" href="../../Recursos/CSS/DiseñoYDesarrollo/ObjetosDibujo.css" rel="stylesheet" />

    <title>Objetos</title>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">

                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="InformacionObjeto-tab" data-bs-toggle="tab" href="#InformacionObjeto-content">Información del Objeto</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Configurar-tab" data-bs-toggle="tab" href="#Configurar-content">Configurar</a>
                    </li>

                </ul>
            </div>
        </nav>

        <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade show active" id="InformacionObjeto-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container border mt-2 shadow" style="min-height: 50rem;">
                            <div class="d-flex border m-2 shadow-sm bg-light">
                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                    <div class="p-1 m-2">
                                        <div class="d-flex">
                                            <div class="col-lg-8 col-md-6 col-sm-6 col-xs-6 p-2">
                                                <asp:Label runat="server" ID="lblObjeto" CssClass="form-label" Text="Objeto"></asp:Label>
                                                <asp:TextBox runat="server" ID="TextObjeto" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-6 p-2">
                                                <asp:Label runat="server" ID="lblLinea" CssClass="form-label" Text="Línea"></asp:Label>
                                                <asp:DropDownList runat="server" ID="DropLinea" CssClass="form-control form-control-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                    <div class="p-1 m-2">
                                        <div class="d-flex flex-wrap">
                                            <div class="col-6 p-2">
                                                <asp:Label runat="server" ID="lblGrupo" CssClass="form-label" Text="Grupo"></asp:Label>
                                                <asp:DropDownList runat="server" ID="DropDesGrupo" CssClass="form-control form-control-sm" OnCheckedChanged="Control_ValueChanged" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                            <div class="col p-2">
                                                <asp:Label runat="server" ID="Label1" CssClass="form-label col-form-label-sm" Text="A(Cms)"></asp:Label>
                                                <asp:TextBox runat="server" ID="TextAncho" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                            </div>
                                            <div class="col p-2">
                                                <asp:Label runat="server" ID="Label2" CssClass="form-label col-form-label-sm" Text="P(Cms)"></asp:Label>
                                                <asp:TextBox runat="server" ID="TextProfundidad" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                            </div>
                                            <div class="col p-2">
                                                <asp:Label runat="server" ID="Label3" CssClass="form-label col-form-label-sm" Text="H(Cms)"></asp:Label>
                                                <asp:TextBox runat="server" ID="TextAltura" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                            </div>
                                            <div class="col p-2">
                                                <asp:Label runat="server" ID="Label4" CssClass="form-label col-form-label-sm" Text="M3"></asp:Label>
                                                <asp:TextBox runat="server" ID="TextCubicaje" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="d-flex">
                                <div class="col-lg-10 col-md-6 col-sm-6 col-xs-6">
                                    <div class="p-3 m-2 border shadow-sm bg-light" style="min-height: 26rem; max-height: 26rem;">
                                       
                                        <div class="container">
                                                <div class="d-flex">
                                                    <div class="col-lg-1 col-md-6 col-sm-6 col-xs-6 p-2">
                                                    </div>
                                                    <div class="col-lg-1 col-md-6 col-sm-6 col-xs-6 p-2">
                                                        <asp:Label runat="server" ID="Label5" CssClass="form-label col-form-label-sm" Text="Divisiones"></asp:Label>
                                                        <asp:DropDownList runat="server" ID="DropDivisiones" CssClass="form-control form-control-sm text-center"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-1 col-md-6 col-sm-6 col-xs-6 p-2">
                                                        <asp:Label runat="server" ID="Label6" CssClass="form-label col-form-label-sm" Text="Holgura"></asp:Label>
                                                        <asp:TextBox runat="server" ID="TextHolgura" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-1 col-md-6 col-sm-6 col-xs-6 p-2">
                                                        <asp:Label runat="server" ID="Label7" CssClass="form-label col-form-label-sm" Text="Und X Paq"></asp:Label>
                                                        <asp:TextBox runat="server" ID="TextUndXPaq" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-3 col-md-6 col-sm-6 col-xs-6 p-2">
                                                        <asp:Label runat="server" ID="Label8" CssClass="form-label col-form-label-sm" Text="Valor Comercial"></asp:Label>
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:TextBox runat="server" ID="TextValorComercial" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                            <asp:Button runat="server" ID="BtnP" CssClass="btn btn-sm border" Text="X" />
                                                        </div>
                                                    </div>
                                                    <div class="d-flex flex-wrap">
                                                        <div class="col p-2">
                                                            <asp:Label runat="server" ID="Label9" CssClass="form-label col-form-label-sm" Text="Id Num"></asp:Label>
                                                            <asp:TextBox runat="server" ID="TextIdNum" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                        </div>
                                                        <div class="col p-2">
                                                            <asp:Label runat="server" ID="Label10" CssClass="form-label col-form-label-sm" Text="Peso(KG)"></asp:Label>
                                                            <asp:TextBox runat="server" ID="TextPeso" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                        </div>
                                                        <div class="col p-2">
                                                            <asp:Label runat="server" ID="Label11" CssClass="form-label col-form-label-sm" Text="I.Referencia"></asp:Label>
                                                            <asp:TextBox runat="server" ID="TextIndReferencia" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>          

                                        <div class="d-flex">

                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm  gap-2">
                                                    <asp:Label runat="server" ID="lblDesInt" CssClass="form-label col-form-label-sm" Text="Descripción Interna"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextDesInt" CssClass="form-control form-control-sm" OnTextChanged="Control_ValueChanged" AutoPostBack="True"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="d-flex mt-2">

                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" ID="Label12" CssClass="form-label col-form-label-sm" Text="Descrip. Cotización"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextDescripcionPanelCotizacion" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="d-flex mt-2">

                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm gap-4">
                                                    <asp:Label runat="server" ID="Label13" CssClass="form-label col-form-label-sm" Text="Descrip. Tecnica"></asp:Label>
                                                    <textarea id="TextAreaDescripTec" runat="server" rows="4" class="form-control shadow-sm"></textarea>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="d-flex mt-2">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                                <div class="d-flex flex-wrap">
                                                    <div class="col-6 p-1">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox runat="server" ID="CheckApliCodPSLOT" class="form-control-sm" />
                                                            <asp:Label runat="server" CssClass="form-label col-form-label-sm" Text="Aplica código PSL para OT"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col p-1">
                                                        <asp:Label runat="server" ID="Label15" CssClass="form-label col-form-label-sm" Text="Id. Insumo"></asp:Label>
                                                        <asp:TextBox runat="server" ID="TextIdInsumo" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                    </div>
                                                    <div class="col p-1">
                                                        <asp:Label runat="server" ID="Label16" CssClass="form-label col-form-label-sm" Text="Cod. PSL"></asp:Label>
                                                        <asp:TextBox runat="server" ID="TextCodPSL" CssClass="form-control form-control-sm text-center"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                                <div class="col p-1">
                                                    <asp:Label runat="server" ID="Label14" CssClass="form-label col-form-label-sm" Text="Insumo relacionado para OT que no sea OAI"></asp:Label>
                                                    <asp:TextBox runat="server" ID="TextInRelOtNoOai" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="d-flex">
                                            <div class="col-lg-12 col-md-6 col-sm-6 col-xs-6">
                                                <div class="d-flex flex-wrap">
                                                    <div class="col p-1">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" CssClass="form-label" Text="Chequeado"></asp:Label>
                                                            <asp:CheckBox runat="server" ID="CheckChequeado" class="form-control-sm" />
                                                        </div>
                                                    </div>
                                                    <div class="col p-1">
                                                        <asp:Button runat="server" ID="BtnGrabarObjetosPanel" CssClass="form-control" Text="Grabar" />
                                                    </div>
                                                    <div class="col p-1">
                                                        <asp:Button runat="server" ID="BtnCancelarObjetosPanel" CssClass="form-control" Text="Cancelar" />

                                                    </div>
                                                    <div class="col p-1">
                                                        <asp:Button runat="server" ID="BtnCerrarObjetosPanel" CssClass="form-control" Text="Cerrar" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-6">
                                    <div class="p-1 m-2 border shadow-sm bg-light" style="min-height: 26rem; max-height: 26rem;">

                                        <div class="d-flex flex-wrap">
                                                    <div class="col p-1">
                                                        <div class="input-group input-group-sm">
                                                            <asp:Label runat="server" CssClass="form-label col-form-label-sm" Text="Activo"></asp:Label>
                                                            <asp:CheckBox runat="server" ID="CheckActivo" class="form-control-sm" OnCheckedChanged="Control_ValueChanged" AutoPostBack="True"/>
                                                        </div>
                                                    </div>

                                            <div class="col p-1">
                                                        <div class="input-group input-group-sm">
                                                            <asp:Label runat="server" CssClass="form-label col-form-label-sm" Text="Escalable"></asp:Label>
                                                            <asp:CheckBox runat="server" ID="CheckEstable" class="form-control-sm" OnCheckedChanged="Control_ValueChanged" AutoPostBack="True"/>
                                                        </div>
                                                    </div>
                                            </div>

                                         <div class="d-flex">
                                             <div class="container-fluid border bg-white" style="min-height: 22rem; max-height: 22rem;">
                                             </div>
                                         </div>

                                    </div>
                                </div>
                            </div>
                            <div class="d-flex">

                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="p-3 m-2 border" style="min-height: 19rem; max-height: 19rem;">

                                         <div class="table-responsive mb-2 gap-2" style="max-height: 18rem; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGridPanel" runat="server"
                                                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" PageSize="5"
                                                                AllowSorting="true">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm bg-light" />
                                                                <Columns>

                                                                    <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="BtnCargarOT" runat="server" CommandName="Id_OT"
                                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>                                                             
                                                                    <asp:BoundColumn DataField="Id_Modulo" HeaderText="Mod" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Chequeado" HeaderText="OK" ItemStyle-CssClass="auto-width-column2"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Ubicacion_Modulo" HeaderText="Pos" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                
                                                                    <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Lado" HeaderText="Lado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Descripcion_Familia" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>  
                                                                    <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn> 
                                                                    <asp:BoundColumn DataField="FechaChequeo" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn> 
                                                                </Columns>
                                                            </asp:DataGrid>

                                    </div>
                                </div>

                            </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Configurar-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container mt-2 shadow" style="min-height: 50rem;">
                            <div class="row mt-2">
                                <div class="col-lg-5 col-md-6 col-sm-12">
                                    <div class="p-1 m-2">
                                        <div class="input-group input-group-sm gap-2">
                                            <asp:Label runat="server" ID="Label17" CssClass="form-label" Text="Grupo"></asp:Label>
                                            <asp:DropDownList runat="server" ID="DropDownList4" CssClass="form-control form-control-sm" DataSourceID="SqlDataSource3"
                                                DataValueField="ID_GrupoObjeto" DataTextField="Descripcion_Grupo" AppendDataBoundItems="True">
                                                <asp:ListItem Text="%" Value="" />
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT ID_GrupoObjeto, Descripcion_Grupo FROM tblGrupoObjeto ORDER BY Descripcion_Grupo"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-lg-4 col-md-6 col-sm-12">
                                    <div class="p-1 m-2">
                                        <div class="input-group input-group-sm gap-2">
                                            <asp:Label runat="server" ID="Label18" CssClass="form-label" Text="Criterio: "></asp:Label>
                                            <asp:TextBox runat="server" ID="TextCriterio" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-6 col-sm-12">
                                    <div class="p-1 m-2">
                                        <div class="input-group input-group-sm gap-2">
                                            <asp:Label runat="server" ID="Label19" CssClass="form-label" Text="Altura Mod"></asp:Label>
                                            <asp:TextBox runat="server" ID="TextAlturaConfigurar" CssClass="form-control form-control-sm"></asp:TextBox>
                                            <asp:Label runat="server" ID="Label20" CssClass="form-label" Text="cms"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <div class="p-1 m-2">
                                        <asp:Label runat="server" CssClass="form-label fw-bold" Text="Dis.LA: 25.28Cms Disp.LB.54.8Cms" Visible="false"></asp:Label>
                                        <asp:Button runat="server" Text="Buscar" ID="Buscar" OnClick="ButtonBuscar_Click" CssClass="btn btn-sm" />
                                    </div>
                                </div>

                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <div class="p-1 m-2">
                                        <div class="row">
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-12 p-2">
                                                <asp:Label runat="server" ID="Label21" CssClass="form-label" Text="Bases"></asp:Label>
                                                <asp:CheckBox runat="server" ID="CheckBase" class="form-control-sm" />
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-12 p-2">
                                                <asp:Label runat="server" ID="Label22" CssClass="form-label" Text="Complementarios"></asp:Label>
                                                <asp:CheckBox runat="server" ID="CheckComplementarios" class="form-control-sm" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-10 col-md-6 col-sm-12">
                                    <div class="p-3 m-2 border shadow-sm bg-light" style="min-height: 23rem; max-height: 23rem;">
                                        <div class="table-responsive mb-1 gap-2" style="max-height: 20.7rem; overflow-x: auto;">
                                            <asp:DataGrid Class="table table-bordered table-hover table-sm form-control-sm" ID="DataGridConfigurar" runat="server"
                                                AutoGenerateColumns="false" OnItemCommand="DataGridConfigurar_ItemCommand">
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkCliee" runat="server" CommandName="Id_Modulo"
                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Id_Modulo" HeaderText="Modulo" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Descripcion_Familia" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Descripcion_TipoModulo" HeaderText="Tipo Modulo" ItemStyle-CssClass="auto-width-column" />
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-md-6 col-sm-12">
                                    <div class="p-1 m-2 border shadow-sm bg-light p-3" style="min-height: 23rem; max-height: 23rem;">
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-12 p-2">
                                                <asp:Label runat="server" ID="Label23" CssClass="form-label col-form-label-sm" Text="Observaciones:"></asp:Label>
                                                <textarea id="TextArea2" runat="server" rows="4" class="form-control shadow-sm"></textarea>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" ID="Label24" CssClass="col-form-label-sm" Text="Ubicación"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="DropDownList5" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                                <div class="input-group input-group-sm gap-3">
                                                    <asp:Label runat="server" ID="Label25" CssClass="col-form-label-sm" Text="Cantidad"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="DropDownList6" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-1">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                                <div class="input-group input-group-sm gap-4">
                                                    <asp:Label runat="server" ID="Label26" CssClass="col-form-label-sm" Text="Lado"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="DropDownList7" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="d-flex justify-content-center mt-5">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                                <div class="input-group input-group-sm gap-2 justify-content-around">
                                                    <asp:Button runat="server" ID="Button1" CssClass="btn border bg-white" Text="X" />
                                                    <asp:Button runat="server" ID="Button2" CssClass="btn border bg-white" Text="X" />
                                                    <asp:Button runat="server" ID="Button3" CssClass="btn border bg-white" Text="X" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <div class="p-3 m-2 border shadow-sm bg-light" style="min-height: 15rem; max-height: 15rem;">
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
