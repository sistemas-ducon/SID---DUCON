<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInsumos.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo.frmInsumos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
      <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"/>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/DiseñoYDesarrollo/ObjetosDibujo.css" rel="stylesheet" />
    <title>Insumos - Consultar</title>
          <script>
              function focusAndScrollToRow(rowId) {
                  var row = document.getElementById(rowId);
                  if (row) {
                      row.setAttribute('tabindex', '-1'); // Make it focusable
                      row.focus();
                      row.scrollIntoView({ behavior: 'smooth', block: 'center' });
                  }
              }
          </script>
       <script>
           function activarPestana(pestanaId, contenidoId) {
               // Desactivar la pestaña actualmente activa
               var activeTab = document.querySelector(".nav-link.active");
               if (activeTab) {
                   activeTab.classList.remove("active");
               }

               var activePane = document.querySelector(".tab-pane.show.active");
               if (activePane) {
                   activePane.classList.remove("show", "active");
               }

               // Mostrar y activar la nueva pestaña
               var newTab = document.getElementById(pestanaId);
               var newPane = document.getElementById(contenidoId);

               if (newTab) {
                   newTab.style.display = 'block';
                   newTab.classList.add("active");
               }

               if (newPane) {
                   newPane.classList.add("show", "active");
               }
           }
       </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <nav class="navbar navbar-light bg-light navbar-custom">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">

                    <li class="nav-item">
                         <a class="nav-link text-white active" id="Insumo-tab" data-bs-toggle="tab" href="#Insumo-content">Insumo</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="TipoInsumoGrupoAcabados-tab" data-bs-toggle="tab" href="#TipoInsumoGrupoAcabados-content">Tipo Insumo y Grupo Acabados</a>
                    </li>

                </ul>
            </div>
        </nav>

        <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade show active" id="Insumo-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container border mt-2 shadow fondoSuave" style="min-height: 50rem;">

                            <div id="container" runat="server" class="container mt-2">
                                <div class="card border shadow-sm">
                                    <div class="card-body bg-light">
                                        <h6 class="mb-3">Información general</h6>
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm d-flex custom-gap-in">
                                                    <asp:Label runat="server" class="col-form-label-sm">Insumo</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="textInsumo" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Creacion</asp:Label>
                                                    <asp:TextBox runat="server" type="date" ID="Textbox1" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Act</asp:Label>
                                                    <asp:TextBox runat="server" type="date" ID="Textbox2" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 d-flex justify-content-end">
                                                <div class="input-group input-group-sm d-flex custom-gap-8">
                                                    <asp:Label runat="server" class="col-form-label-sm">Usuario</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox3" class="form-control form-control-sm input-fixed-width" />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Segunda fila -->
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-6">
                                                <div class="input-group input-group-sm d-flex gap-4">
                                                    <asp:Label runat="server" class="col-form-label-sm">Descripción</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox4" class="form-control form-control-sm" OnTextChanged="Control_Changed" AutoPostBack="true"/>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-6 d-flex justify-content-end">
                                                <div class="input-group input-group-sm d-flex custom-gap-7">
                                                    <asp:Label runat="server" class="col-form-label-sm">Tipo</asp:Label>
                                                    <asp:DropDownList ID="dtacboTipoInsumo" runat="server" class="form-control form-control-sm" DataSourceID="TipoInsumo" DataTextField="Descripcion" DataValueField="Id_TipoInsumo" SelectedIndexChanged="Control_Changed" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="TipoInsumo" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select * from tbltipoInsumo ORDER BY Descripcion asc"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Tercera fila -->
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-3">
                                                <div class="input-group input-group-sm d-flex custom-gap-6">
                                                    <asp:Label runat="server" class="col-form-label-sm">Und</asp:Label>
                                                    <asp:DropDownList ID="DropAbreviado" runat="server" class="form-control form-control-sm" DataSourceID="SqlDataSource1" DataTextField="Abreviado" DataValueField="Id_UnidadMedida" SelectedIndexChanged="Control_Changed" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand=" select * from tblUnidad_Medida order by Id_UnidadMedida"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-3">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Valor</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox6" class="form-control form-control-sm" OnTextChanged="Control_Changed" AutoPostBack="true"/>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-12 col-md-6 d-flex justify-content-end">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Acabado desde</asp:Label>
                                                    <asp:DropDownList class="form-control" ID="DropAcabadosDesde" runat="server">
                                                        <asp:ListItem Value=""></asp:ListItem>
                                                        <asp:ListItem Value="G">G</asp:ListItem>
                                                        <asp:ListItem Value="M">M</asp:ListItem>
                                                        <asp:ListItem Value="N">N</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm d-flex custom-gap-in">
                                                    <asp:Label runat="server" class="col-form-label-sm">Cod.Inv</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox9" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">F.Ganancia</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox10" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">F.Desperdicio</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox11" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-3">
                                                <div class="input-group input-group-sm d-flex custom-gap-in">
                                                    <asp:Label runat="server" class="col-form-label-sm">Peso(Kg)</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox12" class="form-control form-control-sm" OnTextChanged="Control_Changed" AutoPostBack="true"/>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-3">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Und x paq</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="Textbox13" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer d-flex justify-content-end gap-3 bg-white">
                                        <asp:Button runat="server" ID="BtnGrabar" class="btn linkButtonClicked2  shadow-sm text-dark btn-sm" Text="GRABAR" />
                                        <asp:Button runat="server" ID="BtnCancelar" class="btn linkButtonClicked2 shadow-sm text-dark btn-sm" Text="CANCELAR" />
                                        <asp:Button runat="server" ID="BtnCerrar" class="btn linkButtonClicked2 shadow-sm text-dark btn-sm" Text="CERRAR" />
                                    </div>
                                </div>
                            </div>
                            <div class="d-flex">
                                <div class="col-12">
                                    <div class=" m-2  shadow-sm bg-light">

                                        <div class="card p-1">
                                            <div class="card-header p-1">
                                                <h6>Usado en</h6>
                                            </div>
                                            <div class="card-body p-1" style="max-height: 11.4rem; max-width: auto; overflow-x: auto;">
                                                <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                    ID="DataGridSolicitudEspecial" runat="server" AutoGenerateColumns="false">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText=". . .">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerDocumento3"
                                                                    CommandArgument='<%# Container.ItemIndex %>'
                                                                    Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Id_Modulo" HeaderText="Modulo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Tipo Archivo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="" HeaderText="Cant" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_TipoModulo" HeaderText="Tipo Módulo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>
                                        </div>


                                    </div>
                            </div>
                        </div>
                          <div class="d-flex">
    <div class="col-12">
        <div class="m-2 shadow-sm bg-light">
            <div class="card p-1">
                <div class="card-header p-1 text-center">
                    <h6>Histórico actualización de insumo</h6>
                </div>
                <div class="card-body p-1" style="max-height: 15rem; overflow-x: auto;">
                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm bg-white shadow-sm" 
                                  ID="DataGrid1" runat="server" AutoGenerateColumns="false">
                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                        <Columns>
                            <asp:TemplateColumn HeaderText=". . .">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="VerDocumento4" 
                                                    CommandArgument='<%# Container.ItemIndex %>' 
                                                    Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Fecha_Actualizacion" HeaderText="Fecha actualización" 
                                             ItemStyle-CssClass="auto-width-column" />
                            <asp:BoundColumn DataField="Valor" HeaderText="Valor Unitario" 
                                             ItemStyle-CssClass="auto-width-column" />
                            <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" 
                                             ItemStyle-CssClass="auto-width-column" />
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

            <div class="tab-pane fade" id="TipoInsumoGrupoAcabados-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container border mt-2 shadow fondoSuave" style="min-height: 50rem;">
                            <div class="d-flex flex-wrap">
                                <div class="col-lg-10 col-md-9 col-sm-12 col-xs-12 mb-3">
                                    <div class="p-3" style="height: 22rem;">
                                        <div class="card h-100" style="max-height: 19.5rem; max-width: 100%; overflow-x: auto;">
                                            <div class="card-header p-1 text-center">
                                                <h6>Tipo de Insumo</h6>
                                            </div>
                                            <div class="card-body p-1">
                                                <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                    ID="DataGrid2" runat="server" AutoGenerateColumns="false" OnItemCommand="DataGrid2_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText=". . .">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridTipoInsumo"
                                                                    CommandArgument='<%# Container.ItemIndex %>'
                                                                    Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Id_TipoInsumo" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DesGrupoAcabado" HeaderText="Grupo Acabado" ItemStyle-CssClass="auto-width-column" />
                                                         <asp:BoundColumn DataField="IDGrupoAcabado" HeaderText="" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>
                                        </div>

                                        <!-- Input group arranged with rows and columns -->
                                        <div class="row mt-2">
                                            <div class="col-2">
                                                <div class="input-group input-group-sm">
                                                </div>
                                            </div>
                                            <div class="col-5 gap-5">
                                                <div class="input-group input-group-sm">
                                                    <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="TextDescripcion"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-5">
                                                <div class="input-group input-group-sm">
                                               <asp:DropDownList ID="DropAdiAca" runat="server" class="form-control form-control-sm" 
    DataSourceID="SqlDataSource2" DataTextField="Descripcion_Grupo" DataValueField="ID_GrupoAcabado">
</asp:DropDownList>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
    ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" 
    SelectCommand="SELECT ID_GrupoAcabado, Descripcion_Grupo FROM tblGrupodeAcabado ORDER BY Descripcion_Grupo ASC">
</asp:SqlDataSource>
            </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-lg-2 col-md-3 col-sm-12 col-xs-12 mb-3">
                                    <div class="p-3 h-100">
                                        <div class="d-flex flex-column col-3 justify-content-end">
                                            <asp:LinkButton runat="server" title="Adicionar Acabado" ID="BtnAdiAca" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2 justify-content-around" OnClick="BtnAdiAca_Click">
                                               <i class="bi bi-plus-square-fill Grande"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" title="Modificar Acabado" ID="BtnModAca" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnModAca_Click">
                                               <i class="bi bi-wrench-adjustable Grande"></i> 
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" title="Grabar" ID="BtnGraAca" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnGraAca_Click">
                                               <i class="bi-floppy-fill Grande"></i> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="d-flex flex-wrap">
                                <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12 mb-3">
                                    <div class="p-3" style="height: 24rem;">
                                        <div class="input-group input-group-sm mt-2 gap-2">
                                            <asp:LinkButton runat="server" title="Adicionar Acabado" ID="BtnAdiAca2" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnAdiAca2_Click">
                                               <i class="bi bi-plus-square-fill Grande"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" title="Modificar Acabado" ID="BtnModAca2" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnModAca2_Click">
                                               <i class="bi bi-wrench-adjustable Grande"></i> 
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" title="Grabar" ID="BtnGraAca2" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnGraAca2_Click">
                                               <i class="bi-floppy-fill Grande"></i> 
                                            </asp:LinkButton>
                                        </div>
                                        <div class="card h-100 mt-2" style="max-height: 19rem; max-width: auto; overflow-x: auto;">
                                            <div class="card-header p-1 text-center">
                                                <h6>Grupo de Acabado</h6>
                                            </div>     
                                            <div class="card-body p-1">
                                                <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm" OnItemCommand="DataGrid3_ItemCommand"
                                                    ID="DataGrid3" runat="server" AutoGenerateColumns="false">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText=". . .">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="GrupoAcabado"
                                                                    CommandArgument='<%# Container.ItemIndex %>'
                                                                    Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="ID_GrupoAcabado" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Grupo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>
                                        </div>

                                        <div class="input-group input-group-sm mt-2 gap-2">
                                            <asp:TextBox runat="server" CssClass="form-control form-control-sm" ID="TextDescripcion2"></asp:TextBox>
                                        </div>

                                    </div>
                                </div>

                                <div class="col-lg-8 col-md-6 col-sm-12 col-xs-12 mb-3">
                                    <div class="p-3" style="height: 24rem;">
                                        <div class="input-group input-group-sm mt-2 gap-2">
                                            <asp:LinkButton runat="server" title="Adicionar Acabado" ID="BtnAdiAca3" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnAdiAca3_Click">
                                               <i class="bi bi-plus-square-fill Grande"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" title="Modificar Acabado" ID="BtnModAca3" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnModAca3_Click">
                                               <i class="bi bi-wrench-adjustable Grande"></i> 
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" title="Grabar" ID="BtnGraAca3" CssClass="btn linkButtonClicked2 shadow-sm text-dark btn-sm mb-2" OnClick="BtnGraAca3_Click">
                                               <i class="bi-floppy-fill Grande"></i> 
                                            </asp:LinkButton>
                                        </div>
                                        <div class="card h-100 mt-2" style="max-height: 19rem; max-width: auto; overflow-x: auto;">
                                            <div class="card-header p-1 text-center">
                                                <h6>Acabados</h6>
                                            </div>
                                            <div class="card-body p-1">
                                                <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                    ID="DataGrid4" runat="server" AutoGenerateColumns="false" OnItemCommand="DataGrid4_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText=". . .">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridAcabados"
                                                                    CommandArgument='<%# Container.ItemIndex %>'
                                                                    Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="CodInventario" HeaderText="Cod.Inv" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Acabado" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DeLinea" HeaderText="L" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Activo" HeaderText="A" ItemStyle-CssClass="auto-width-column" />
                                                         <asp:BoundColumn DataField="ID_Acabado" HeaderText="A" ItemStyle-CssClass="auto-width-column" Visible="false"/>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>
                                        </div>

                                       <div class="input-group input-group-sm mt-2 gap-2">
    <asp:TextBox runat="server" CssClass="form-control form-control-sm col-lg-1" ID="TextCodInv"></asp:TextBox>
    
    <asp:TextBox runat="server" CssClass="form-control form-control-sm col-lg-9" ID="TextDescripcionAcabado"></asp:TextBox>
    
    <asp:CheckBox runat="server" CssClass="form-check col-lg-1" ID="CheckBoxLinea" />

    <asp:CheckBox runat="server" CssClass="form-check col-lg-1" ID="CheckBoxActivo" />
</div>


                                    </div>
                                </div>
                            </div>
                        </div>


                        
                <div class="modal" id="LlenarDescripcion" tabindex="-1" style="display: none;">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header RojoEfecto fw-bold shadow">
                                <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON </h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Los campos descripcion y tipo insumo son obligatorios</span></p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

                        <div class="modal" id="MensajeExito" tabindex="-1" style="display: none;">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header VerdeEfecto fw-bold shadow">
                                <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Los datos se insertaron exitosamente</span></p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

                          <div class="modal" id="MensaMensajeExitoActualizacionjeExito" tabindex="-1" style="display: none;">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header VerdeEfecto fw-bold shadow">
                                <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Los datos se actualizaron exitosamente</span></p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

                        
                        <div class="modal" id="MensajeError" tabindex="-1" style="display: none;">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header RojoEfecto fw-bold shadow">
                                <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Porfavor intentelo de nuevo, y si el error persiste comuniquese con el departamento de sistemas</span></p>
                    </div>
                    <div class="modal-footer">
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
