<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modulo.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo.Modulo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/DiseñoYDesarrollo/ObjetosDibujo.css" rel="stylesheet" />
    <title>Modulo</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <nav class="navbar navbar-light bg-light navbar-custom">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">

                    <li class="nav-item">
                        <a class="nav-link text-white active" id="Informacion-tab" data-bs-toggle="tab" href="#Informacion-content">Información</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="Configuracion-tab" data-bs-toggle="tab" href="#Configuracion-content">Configuración</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="FamiliaModulo-tab" data-bs-toggle="tab" href="#FamiliaModulo-content">Familia Módulo</a>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade show active" id="Informacion-content">
               <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                         <div class="container border mt-2 shadow fondoSuave" style="min-height: 50rem;">
                              <div id="container" runat="server" class="container mt-2">
                                <div class="card border shadow-sm">
                                    <div class="card-body bg-light">
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm gap-5">
                                                    <asp:Label runat="server" class="col-form-label-sm">Módulo</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="textModulo" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-4">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Buscar Familia</asp:Label>
                                                    <asp:TextBox runat="server" ID="TextBusFam" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-4">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Familia</asp:Label>
                                                    <asp:SqlDataSource
                                                        ID="DropDownListGrupoSqlDataS"
                                                        runat="server"
                                                        ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                        SelectCommand="SELECT ID_Familia, Descripcion_Familia FROM tblFamiliaModulo ORDER BY Descripcion_Familia"></asp:SqlDataSource>
                                                    <asp:DropDownList
                                                        ID="DropDownListGrupo"
                                                        runat="server"
                                                        class="form-control form-control-sm"
                                                        AppendDataBoundItems="true"
                                                        OnSelectedIndexChanged="CheckForChanges"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Text="" Value="0" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                           
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-6">
                                                <div class="input-group input-group-sm gap-4">
                                                    <asp:Label runat="server" class="col-form-label-sm">Descripción</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="TextDescripcionModulo" class="form-control form-control-sm" OnTextChanged="CheckForChanges" AutoPostBack="true" />
                                                </div>
                                            </div>
                                          
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-4">
                                                <div class="input-group input-group-sm gap-3">
                                                    <asp:Label runat="server" class="col-form-label-sm">Tipo Módulo</asp:Label>
                                                    <asp:SqlDataSource
                                                        ID="SqlDataSource1"
                                                        runat="server"
                                                        ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                        SelectCommand="Select * from tblTipoModulo order by (Descripcion_TipoModulo)"></asp:SqlDataSource>

                                                    <asp:DropDownList
                                                        ID="DropDownListTipoModulo"
                                                        runat="server"
                                                        class="form-control form-control-sm"
                                                        AppendDataBoundItems="true"
                                                        OnSelectedIndexChanged="CheckForChanges"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Text="" Value="0" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-3">
                                                <div class="input-group input-group-sm gap-5">
                                                    <asp:Label runat="server" class="col-form-label-sm">Altura</asp:Label>
                                                    <asp:TextBox runat="server" type="text" ID="TextAlturaModulo" class="form-control form-control-sm" OnTextChanged="CheckForChanges" AutoPostBack="true" />
                                                    <asp:Label runat="server" class="col-form-label-sm">Cms</asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-12">
                                                <div class="input-group input-group-sm justify-content-center gap-2">
                                                    <asp:LinkButton runat="server" title="Grabar" ID="BtnGrabarInf" OnClick="BtnGrabarInf_Click">               
                                                 <i class="bi bi-floppy-fill"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" title="Cancelar" ID="BtnCancelarInf">               
                                               <i class="bi bi-ban"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card h-100">
                                            <div class="card-header p-1 text-center">
                                                <h6>APLICACIÓN</h6>
                                            </div>
                                            <div class="card-body p-1" style="height: 34rem; max-width: 100%; overflow-x: auto;">
                                                <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                    ID="DataGrid2" runat="server" AutoGenerateColumns="false">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText=". . .">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridTipoInsumo"
                                                                    CommandArgument='<%# Container.ItemIndex %>'
                                                                    Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Id_Panel" HeaderText="Panel" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Ubicacion_Modulo" HeaderText="U" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Lado" HeaderText="Lado" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Grupo" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Reportar_Despacho" HeaderText="R.Despacho" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                              </div>
                         </div>

                        <div class="modal fade" id="ModalRotacionModulo" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-title d-flex align-items-center justify-content-center text-white p-2 AzulOscuroEfecto fw-bold shadow">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>
                                    </div>
                                    <div class="modal-body bg-light form-control-sm">
                                        <div class="row container">
                                            <div class="col-12">
                                                <p><span id="ModalRotacionModulo2"></span></p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center bg-light">
                                        <asp:Button runat="server" type="button" class="btn btn-sm linkButtonClicked2 shadow-sm text-dark btn-outline-success fw-bold" data-bs-dismiss="modal" Text="Si" aria-label="Close" OnClick="btnConfirmarModificacion_Click"></asp:Button>
                                        <asp:Button runat="server" type="button" class="btn btn-sm linkButtonClicked2 shadow-sm text-dark btn-outline-danger fw-bold" data-bs-dismiss="modal" Text="No" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
               </asp:UpdatePanel>
            </div>
            <div class="tab-pane fade" id="Configuracion-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container border mt-2 shadow fondoSuave" style="min-height: 50rem;">
                            <div id="Div1" runat="server" class="container mt-2">
                                <div class="card border shadow-sm">
                                    <div class="card-body bg-light">
                                        <div class="row mb-2">
                                            <div class="col-12 col-sm-6 col-md-4">
                                                <div class="input-group input-group-sm gap-5">
                                                    <asp:Label ID="LblTipoInsumo" runat="server" CssClass="me-2 col-form-label-sm" Text="Tipo Insumo"></asp:Label>
                                                    <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-control form-control-sm" OnTextChanged="DropDownList1_TextChanged" AutoPostBack="true" DataTextField="Descripcion_Insumo" DataValueField="Id_Insumo" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-4">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Criterio</asp:Label>
                                                    <asp:TextBox runat="server" ID="TextCriterio" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                            <div class="col-12 col-sm-6 col-md-2">
                                                <div class="input-group input-group-sm d-flex gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Inv</asp:Label>
                                                    <asp:TextBox runat="server" ID="TextInv" class="form-control form-control-sm" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="d-flex flex-column flex-lg-row">
                                            <div class="col-lg-8 col-md-6 col-sm-12">
                                                <div class="p-3 border" style="height: 25.8rem;">
                                                    <div class="card-body p-1" style="height: 22.8rem; max-width: 100%; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="table table-bordered table-responsive table-sm table-hover form-control-sm bg-white shadow-sm"
                                                            ID="DataGrid1" runat="server" AutoGenerateColumns="false" OnCancelCommand="DataGrid1_CancelCommand">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header shadow-sm" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText=". . .">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" runat="server" CommandName="DatagridInsumo"
                                                                            CommandArgument='<%# Container.ItemIndex %>'
                                                                            Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Id_Insumo" HeaderText="Insumo" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Descripcion_Insumo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Abreviado" HeaderText="UND" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ID_Inventario" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-6 col-sm-12">
                                                <div class="p-1" style="height: 25rem;">
                                                    <div class="container">
                                                        <div class="shadow-sm p-2">
                                                            <div class="row">
                                                                <div class="col-12 d-flex align-items-center">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="me-2 col-form-label-sm" Text="Costear"></asp:Label>
                                                                    <asp:CheckBox runat="server" CssClass="mt-1" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <div class="mb-1">
                                                                        <asp:Label runat="server" class="col-form-label-sm">Sentido</asp:Label>
                                                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control form-control-sm" />
                                                                    </div>
                                                                    <div class="mb-1 d-flex align-items-center">
                                                                        <asp:Label runat="server" class="col-form-label-sm">Cantidad</asp:Label>
                                                                        <asp:TextBox runat="server" ID="TextBox2" class="form-control form-control-sm ms-2" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="mb-1">
                                                                        <asp:Label runat="server" class="col-form-label-sm">Descripción pieza/Uso</asp:Label>
                                                                        <textarea class="form-control form-control-sm mt-2" rows="2" id="txObs1"></textarea>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-12 d-flex align-items-center">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="me-2 col-form-label-sm" Text="Pieza Escalable"></asp:Label>
                                                                    <asp:CheckBox runat="server" CssClass="mt-1" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container border p-2 mt-3 position-relative shadow-sm">
                                                            <span class="position-absolute top-0 start-0 translate-middle-y bg-light px-2" style="margin-left: 10px; font-size: small;">Pieza escalable</span>
                                                            <div class="row mt-2">
                                                                <div class="col-md-6">
                                                                    <div class="input-group input-group-sm gap-1">
                                                                        <asp:Label runat="server" class="col-form-label-sm">Dcto Ancho</asp:Label>
                                                                        <asp:TextBox runat="server" ID="TextBox3" class="form-control form-control-sm" />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="input-group input-group-sm gap-1">
                                                                        <asp:Label runat="server" class="col-form-label-sm">Dcto en Altura</asp:Label>
                                                                        <asp:TextBox runat="server" ID="TextBox1" class="form-control form-control-sm" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="container mt-3">
                                                            <div class="row">
                                                                <div class="col-md-10 border p-2 position-relative shadow-sm">
                                                                    <span class="position-absolute top-0 start-0 translate-middle-y bg-light px-2" style="margin-left: 10px; font-size: small;">Pieza fija</span>
                                                                    <div class="row mt-2">
                                                                        <div class="col-md-6">
                                                                            <div class="input-group input-group-sm gap-1">
                                                                                <asp:Label runat="server" class="col-form-label-sm">Ancho</asp:Label>
                                                                                <asp:TextBox runat="server" ID="TextBox4" class="form-control form-control-sm" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <div class="input-group input-group-sm gap-1">
                                                                                <asp:Label runat="server" class="col-form-label-sm">Alto</asp:Label>
                                                                                <asp:TextBox runat="server" ID="TextBox5" class="form-control form-control-sm" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 d-flex align-items-center">
                                                                    <div class="mb-1">
                                                                        <asp:Label runat="server" class="col-form-label-sm">Divisiones</asp:Label>
                                                                        <asp:TextBox runat="server" ID="TextBox6" class="form-control form-control-sm" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="container mt-2">
                                                            <div class="row mt-3 p-2 shadow-sm">
                                                                <div class="col-md-10">
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div class="input-group input-group-sm gap-1">
                                                                                <asp:LinkButton runat="server" title="Grabar" ID="BtnAgregarConfiguracion">               
                                                                                         <i class="bi bi-floppy-fill"></i>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton runat="server" title="Grabar" ID="LinkButton2">               
                                                                                         <i class="bi bi-floppy-fill"></i>
                                                                                </asp:LinkButton>
                                                                                <asp:LinkButton runat="server" title="Grabar" ID="BtnGrabarConfiguracion">               
                                                                                         <i class="bi bi-floppy-fill"></i>
                                                                                </asp:LinkButton>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2 d-flex align-items-center">
                                                                    <div class="mb-1">
                                                                        <asp:LinkButton runat="server" title="Grabar" ID="BtnX">               
                                                                             <i class="bi bi-floppy-fill"></i>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                               <div id="Div2" runat="server" class="container mt-2">
                                <div class="card border shadow-sm">
                                    <div class="card-body bg-light">
                                          <div class="card-body p-1" style="height: 14rem; max-width: 100%; overflow-x: auto;">
                                                          <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid3" runat="server" AutoGenerateColumns="false">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="SelectInsumoID" runat="server" CommandName="ModuloIns" CommandArgument='<%# Container.ItemIndex %>'
                                                                    Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn HeaderText="Item" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="ID_Inventario" HeaderText="Cod.Inv" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Pieza" HeaderText="Insumo - Pieza" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Abreviado" HeaderText="UND" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DescuentoAncho" HeaderText="Dcto A" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DescuentoAltura" HeaderText="Dcto H" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="AltoFijo" HeaderText="A.Fijo" ItemStyle-CssClass="auto-width-column" />  
                                                        <asp:BoundColumn DataField="Divisiones" HeaderText="Div" ItemStyle-CssClass="auto-width-column" /> 
                                                        <asp:BoundColumn DataField="Sentido" HeaderText="Sentido" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="miResponsable" HeaderText="Resposable" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="FechaSuceso" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_ModuloInsumo" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                         <asp:BoundColumn DataField="Id_Insumo" HeaderText="Insumo" ItemStyle-CssClass="auto-width-column" />
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
            <div class="tab-pane fade" id="FamiliaModulo-content">
                <h6>3</h6>
            </div>



        </div>
    </form>

     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
