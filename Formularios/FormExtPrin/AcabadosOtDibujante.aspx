<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcabadosOtDibujante.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.AcabadosOtDibujante" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/AcabadosOtDibujante.css" rel="stylesheet" />
    <title>Acabados Dibujante</title>

    <script>

        function mostrarTap() {
            // Oculta la pestaña de "Acabados Plano"
            var acabadosTab = document.getElementById("Acabados-tab");
            acabadosTab.style.display = "none";

            // Muestra la pestaña de "Definir Acabados" y activa su contenido
            var definirAcabadoTab = document.getElementById("DefinirAcabado-tab");
            definirAcabadoTab.style.display = "block";
            definirAcabadoTab.classList.add("active");



            // Cambia el contenido activo
            var acabadosContent = document.getElementById("Acabados-content");
            var definirAcabadoContent = document.getElementById("DefinirAcabado-content");

            acabadosContent.classList.remove("show", "active");
            definirAcabadoContent.classList.add("show", "active");

            // Activa visualmente la pestaña
            var miPestañas = new bootstrap.Tab(definirAcabadoTab);
            miPestañas.show();

            // Muestra el botón de cerrar para "Definir Acabados" y oculta el de "Acabados Plano"
            document.getElementById("btnCerrarDefinirAcabado").style.display = "block";
            document.getElementById("btnCerrarAcabadosPlano").style.display = "none";


        }

        function mostrarTapAcabados() {
            // Muestra la pestaña de "Acabados Plano"
            var acabadosTab = document.getElementById("Acabados-tab");
            acabadosTab.style.display = "block";
            acabadosTab.classList.add("active");

            // Oculta la pestaña de "Definir Acabados"
            var definirAcabadoTab = document.getElementById("DefinirAcabado-tab");
            definirAcabadoTab.style.display = "none";
            definirAcabadoTab.classList.remove("active");

            // Cambia el contenido activo
            var acabadosContent = document.getElementById("Acabados-content");
            var definirAcabadoContent = document.getElementById("DefinirAcabado-content");

            acabadosContent.classList.add("show", "active");
            definirAcabadoContent.classList.remove("show", "active");

            // Activa visualmente la pestaña de "Acabados Plano"
            var miPestañas = new bootstrap.Tab(acabadosTab);
            miPestañas.show();

            // Muestra el botón de cerrar para "Acabados Plano" y oculta el de "Definir Acabados"
            document.getElementById("btnCerrarAcabadosPlano").style.display = "block";
            document.getElementById("btnCerrarDefinirAcabado").style.display = "none";
        }


        // Poner el foco en una fila seleccionada 
        function focusAndScrollToRow(rowId) {
            var row = document.getElementById(rowId);
            if (row) {
                row.setAttribute('tabindex', '-1'); // Make it focusable
                row.focus();
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });


            }
        }

        function mostrarModalEliminarAcabado() {
            $('#confirmarEliminarAcabado').modal('show');
        }


    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <nav class="navbar navbar-light bg-light navbar-custom">
            <div class="container d-flex justify-content-between align-items-center">
                <ul class="nav nav-tabs gap-4" id="miPestañas">
                    <li class="nav-item">
                        <a class="nav-link text-white active fw-bold" id="Acabados-tab" data-bs-toggle="tab" href="#Acabados-content" onclick="mostrarCerrarAcabados()">Acabados Plano
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white fw-bold" id="DefinirAcabado-tab" data-bs-toggle="tab" href="#DefinirAcabado-content" style="display: none;" onclick="mostrarCerrarDefinir()">Definir Acabados 
                            <asp:Label ID="lb_ID_AcadoMod" runat="server" Text="" Visible="true"></asp:Label>
                        </a>
                    </li>
                </ul>

                <!-- Botones de cerrar, uno para cada pestaña -->
                <div class="btn-group">
                    <asp:LinkButton ID="btnCerrarAcabadosPlano"  runat="server"  
                        ToolTip="Cerrar" 
                        Style="color: white !important; margin-right: 1.5rem; font-size: 1.6rem; text-decoration: none;"
                        OnClick="btnCerrarAcabadosPlano_Click">
                          <i class="bi bi-x-circle"></i>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnCerrarDefinirAcabado" runat="server"
                        ToolTip="Volver Acabados Plano"
                        Style="color: white !important; margin-right: 1.5rem; font-size: 1.6rem; text-decoration: none; display: none"
                        OnClientClick="mostrarTapAcabados(); return false;">
                        <i class="bi bi-arrow-left-circle"></i>
                    </asp:LinkButton>

                </div>
            </div>
        </nav>


        <div class="tab-content">

            <div class="tab-pane fade  show active" id="Acabados-content">
                <asp:UpdatePanel ID="PanelAcabados" runat="server">
                    <ContentTemplate>

                        <!-- campo ocultos control acabados  -->
                        <div class="row" style="display: none;">
                            <asp:TextBox ID="Plano" type="text" class="form-control  input" runat="server"></asp:TextBox>
                            <asp:TextBox ID="OT" type="text" class="form-control  input" runat="server"></asp:TextBox>
                            <asp:TextBox ID="Pedido" type="text" class="form-control  input" runat="server"></asp:TextBox>
                        </div>

                        <div class="container-fluid p-0">
                            <div class="card p-0">

                                <div class="card-body">

                                    <div class="card shadow-sm">

                                        <div class="card-header">
                                            <h5 class=" text-center">Acabados</h5>
                                        </div>

                                        <div class="card-body p-0 ">
                                            <div class="row pb-1 mb-1">
                                                <div class="col-12">
                                                    <div class="table-responsive mb-1 gap-2" style="max-height: 15rem; height: 15rem; overflow-x: auto;">

                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridAcabados1" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" DataSourceID="AcabadosFinales" OnItemCommand="DataGridAcabados1_ItemCommand" OnItemDataBound="DataGridAcabados1_ItemDataBound">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="...">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkAcabT" CssClass="Tam" ToolTip="Selecccionar Acabado" runat="server" CommandName="VerAcabadoDib" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="oadDescripcionGrupoObjeto" HeaderText="Apliaca a" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="oadDescripcion_Familia" HeaderText="Familia Módulo" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="oadDescripcion_Insumo" HeaderText="Insumo" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="oadCodInvDes" HeaderText="Codigo Destino" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="oadDescripcionAcabado" HeaderText="Acabado" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="oadAplicacionAcabado" HeaderText="A.A" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="id_OTAcabadoDefinitivo" HeaderText="" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                                <asp:BoundColumn DataField="oadIDGrupoAcabado" HeaderText="" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                                <asp:BoundColumn DataField="oadId_Insumo" HeaderText="" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                                <asp:BoundColumn DataField="oadID_Familia" HeaderText="" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                            </Columns>
                                                        </asp:DataGrid><asp:SqlDataSource runat="server" ID="AcabadosFinales" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT
                                          tblOTAcabadoDefinitivo.id_OTAcabadoDefinitivo,
                                          tblOTAcabadoDefinitivo.oadPLano,
                                          tblOTAcabadoDefinitivo.oadDescripcionGrupoObjeto,
                                          tblOTAcabadoDefinitivo.oadID_Familia,
                                          tblOTAcabadoDefinitivo.oadDescripcion_Familia,
                                          tblOTAcabadoDefinitivo.oadIDGrupoAcabado,
                                          tblOTAcabadoDefinitivo.oadDesGrupoAcabado,
                                          tblOTAcabadoDefinitivo.oadId_Insumo,
                                          tblOTAcabadoDefinitivo.oadCodInvOri,
                                          tblOTAcabadoDefinitivo.oadDescripcion_Insumo,
                                          tblOTAcabadoDefinitivo.oadCodInvDes,
                                          tblOTAcabadoDefinitivo.oadDescripcionAcabado,
                                          tblOTAcabadoDefinitivo.oadAplicacionAcabado,
                                          tblOTAcabadoDefinitivo.oadActivo 
                                          From tblOTAcabadoDefinitivo 
                                          WHERE (((tblOTAcabadoDefinitivo.[oadPlano])=@plano))
                                          order by tblOTAcabadoDefinitivo.oadDescripcion_Familia 
                                          asc, tblOTAcabadoDefinitivo.oadDescripcionGrupoObjeto 
                                          asc,tblOTAcabadoDefinitivo.oadAplicacionAcabado asc">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="Plano" PropertyName="Text" Name="plano"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row  mt-4 m-1 p-2 border rounded shadow-sm">

                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12 p-0 ">

                                            <div class="card">

                                                <div class="card-header">
                                                    <h5 class="text-center">Acabado Ventas</h5>
                                                </div>

                                                <div class="card-body p-0">
                                                    <div class="table-responsive gap-2" style="max-height: 18rem; height: 18rem; overflow-x: auto;">

                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridAcabadoVentas" runat="server" AutoGenerateColumns="false" DataSourceID="AcabadosVentas" OnItemCommand="DataGridAcabadoVentas_ItemCommand">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="...">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkAcabV" CssClass="Tam" ToolTip="Seleccionar Acabado Ventas" runat="server" CommandName="VerDocumento1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:BoundColumn HeaderText="Acabado de Ventas" DataField="AcabadoVentas" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn HeaderText="Entrega" DataField="Entrega" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Descripcion_Acabado" Visible="false" />
                                                                <asp:BoundColumn DataField="Detalle_Adicional" Visible="false" />
                                                                <asp:BoundColumn DataField="GrupoObjetoparaAcabado" Visible="false" />

                                                            </Columns>
                                                        </asp:DataGrid><asp:SqlDataSource runat="server" ID="AcabadosVentas" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT
                                                 tblOTAcabados.*,
                                                 tblGrupoObjetoParaAcabado.*,
                                                 tblAcabado.* 
                                                 FROM tblAcabado 
                                                 INNER JOIN (tblGrupoObjetoParaAcabado 
                                                 INNER JOIN tblOTAcabados 
                                                 ON tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado = tblOTAcabados.ID_GrupoObjetoparaAcabado) 
                                                 ON tblAcabado.ID_Acabado = tblOTAcabados.ID_Acabado 
                                                 WHERE (((tblOTAcabados.Id_OT)=@Id_Ot) 
                                                 AND ((tblOTAcabados.Consecutivo_Pedido)=@Pedido))">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="OT" PropertyName="Text" Name="Id_Ot"></asp:ControlParameter>
                                                                <asp:ControlParameter ControlID="Pedido" PropertyName="Text" Name="Pedido"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                    </div>
                                                </div>

                                            </div>


                                        </div>

                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12 pt-3 mt-4 text-end">

                                            <asp:Button ID="btnAgregarAcabado" runat="server" Text="Acabado de Plano" ToolTip="Agregar Acabado" class="btn btn-sm btn-outline-secondary" OnClick="btnAgregarAcabado_Click" />
                                        </div>

                                    </div>


                                </div>
                            </div>
                        </div>

                        <!--Modal Eliminar Acabado Plano  -->
                        <div id="confirmarEliminarAcabado" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger text-white">
                                        <h5 class="modal-title text-center">Eliminar Acabado</h5>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>Desea eliminar el acabado: <span runat="server" id="span_NombreAcabado"></span> para objeto especial? </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnEliminarAcabado_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-danger" Style="width: 5rem;" OnClick="btnEliminarAcabado_SI_Click" />
                                            <asp:Button runat="server" ID="btneliminarAcabado_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-secondary" Style="width: 5rem;" OnClick="btneliminarAcabado_NO_Click" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>


                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <div class="tab-pane fade" id="DefinirAcabado-content">
                <asp:UpdatePanel ID="PanelDefinirAcabado" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid">
                            <div class="card p-0 m-3">

                                <div class="card-body">

                                    <div class="row p-1 g-2 pt-2">

                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12 pt-1" style="padding-left: 1rem;">
                                            <div class="input-group input-group-sm gap-2">
                                                <asp:Label ID="lbbus" runat="server" Text="Buscar"></asp:Label>
                                                <asp:TextBox ID="tbBuscarAcaba" CssClass="form-control form-control-sm" runat="server" OnTextChanged="tbBuscarAcaba_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:LinkButton class="icong button-enabled btn btn-sm  shadow-sm ColorAzulActivo " runat="server" ToolTip="Buscar" ID="btnBuscarAcab" OnClick="btnBuscarAcab_Click">                                 
                                                   <i class="bi bi-search"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>

                                        <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 pt-1">
                                            <div class="input-group input-group-sm gap-1 justify-content-center">
                                                <asp:CheckBox ID="chkTodoAcabados" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodoAcabados_CheckedChanged" />
                                                <asp:Label ID="lbTodos" runat="server" Text="Todos los Acabados"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                            <div class="input-group input-group-sm  justify-content-end p-1 ">

                                                <div class="contenedor-icono gap-2">
                                                    <asp:LinkButton class="icong button-disabled btn btn-sm  shadow-sm " runat="server" ToolTip="Ver Origen" ID="btnVerOrigen" OnClick="btnVerOrigen_Click">                                 
                    <i class="bi bi-star-half"></i>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton class="icong button-disabled  btn btn-sm shadow-sm" runat="server" ToolTip="Adicionar Acabado" ID="btnAdicionarAcabado" OnClick="btnAdicionarAcabado_Click">                                 
                       <i class="bi bi-plus-circle-fill"></i>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton class="icong button-disabled  btn btn-sm   shadow-sm " runat="server" ToolTip="Modificar Acabado" ID="btnModificarAcabado" OnClick="btnModificarAcabado_Click">                                 
                       <i class="bi bi-wrench-adjustable"></i>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton class="icong button-disabled btn btn-sm  shadow-sm   " runat="server" ToolTip="Grabar" ID="btnGrabarRedAcabadoNue" OnClick="btnGrabarRedAcabadoNue_Click">                               
                        <i class="bi bi-floppy-fill"></i>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton class="icong button-disabled btn btn-sm shadow-sm " Visible="false" runat="server" ToolTip="Grabar" ID="btnGrabarRedAcaMod" OnClick="btnGrabarRedAcaMod_Click">                               
                        <i class="bi bi-floppy-fill"></i>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton class="icong button-disabled btn btn-sm shadow-sm " runat="server" ToolTip="Refrescar" ID="btnRefrescar" OnClick="btnRefrescar_Click">                               
                       <i class="bi bi-arrow-clockwise"></i>
                                                    </asp:LinkButton>

                                                </div>

                                            </div>
                                        </div>

                                    </div>

                                    <div class="row justify-content-center mb-1">

                                        <div class="border rounded shadow-sm ">

                                            <div class="row p-1">

                                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                                    <div class="table-responsive mb-1 gap-2" style="max-height: 23rem; height: 23rem; overflow-x: auto;">
                                                        <h6 class="datagrid-header text-start">Acabados</h6>
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridDefinirAcabado" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemCommand="DataGridDefinirAcabado_ItemCommand" OnItemDataBound="DataGridDefinirAcabado_ItemDataBound">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>

                                                                <asp:TemplateColumn HeaderText=". . .">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkRedAca" CssClass="Tam" ToolTip="VerRedAcabado" runat="server" CommandName="VerDocumento1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:BoundColumn DataField="CodInventario" HeaderText="Cod Inventario" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Descripcion_Acabado" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="DeLinea" HeaderText="Linea" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Activo" HeaderText="Activo" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="CreadoPor" HeaderText="Creado Por" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="FechaCreacion" HeaderText="Fecha Creación" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ModificadoPor" HeaderText="Modificado Por" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="FechaModificacion" HeaderText="Fecha Modificacón" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ID_Acabado" Visible="false" ItemStyle-CssClass="auto-width-column" />

                                                            </Columns>
                                                        </asp:DataGrid><asp:SqlDataSource runat="server" ID="DsDefinirAcabado" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblGrupodeAcabado 
                 INNER JOIN tblAcabado ON tblGrupodeAcabado.ID_GrupoAcabado = tblAcabado.ID_GrupoAcabado 
                 INNER JOIN  tblOTAcabados ON tblAcabado.ID_Acabado = tblOTAcabados.ID_Acabado 
                 WHERE (tblAcabado.Descripcion_Acabado LIKE '%'+ @DescriAcabado +'%') 
                 AND (tblAcabado.ID_GrupoAcabado = @ID_GrupoAca) 
                 AND (tblOTAcabados.Id_OT = @OT) 
                 AND (tblOTAcabados.Consecutivo_Pedido = @Ped) 
                 ORDER BY tblAcabado.Descripcion_Acabado">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="tbBuscarAcaba" PropertyName="Text" DefaultValue="%" Name="DescriAcabado"></asp:ControlParameter>
                                                                <asp:Parameter Name="ID_GrupoAca"></asp:Parameter>
                                                                <asp:ControlParameter ControlID="OT" PropertyName="Text" DefaultValue="" Name="OT"></asp:ControlParameter>
                                                                <asp:ControlParameter ControlID="Pedido" PropertyName="Text" DefaultValue="" Name="Ped"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                        <asp:SqlDataSource runat="server" ID="DsDefinirAcabado1" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT 
         tblAcabado.* FROM tblGrupodeAcabado INNER JOIN  tblAcabado ON tblGrupodeAcabado.ID_GrupoAcabado = tblAcabado.ID_GrupoAcabado 
         WHERE tblAcabado.Descripcion_Acabado LIKE '%' + @DescripcionAcabado + '%' AND tblAcabado.ID_GrupoAcabado = @ID_GrupoAcabado 
         ORDER BY  tblAcabado.Descripcion_Acabado;">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="tbBuscarAcaba" PropertyName="Text" DefaultValue="%" Name="DescripcionAcabado"></asp:ControlParameter>
                                                                <asp:Parameter Name="ID_GrupoAcabado"></asp:Parameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                    </div>
                                                </div>

                                                <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" runat="server" id="DivOrigenAcabado" visible="false">
                                                    <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; height: 20rem; overflow-x: auto;">
                                                        <h6 class="datagrid-header text-center">Origen</h6>
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridOrigenAcabado" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="Descripcion_Insumo" HeaderText="Insumo" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Módulo" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Descripcion_Panel" HeaderText="Objeto" ItemStyle-CssClass="auto-width-column" />
                                                            </Columns>
                                                        </asp:DataGrid><asp:SqlDataSource runat="server" ID="DSOrigenAca" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT
                             tblPanel.Descripcion_Panel,
                             tblModulo.Descripcion_Modulo,
                             tblPlano_Panel.Id_Plano,
                             tblInsumo.Descripcion_Insumo, 
                             tblInsumo.AplicacionAcabado 
                             FROM (tblPanel
                             INNER JOIN ((tblModulo 
                             INNER JOIN ((tblTipoInsumo 
                             INNER JOIN tblInsumo ON tblTipoInsumo.Id_TipoInsumo = tblInsumo.Id_TipoInsumo) 
                             INNER JOIN tblModulo_Insumo ON tblInsumo.Id_Insumo = tblModulo_Insumo.Id_Insumo) ON tblModulo.Id_Modulo = tblModulo_Insumo.Id_Modulo) 
                             INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo) ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum) 
                             INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum 
                             WHERE 
                             (((tblPlano_Panel.Id_Plano)=@Plano) 
                             AND ((tblInsumo.AplicacionAcabado)= @AplicadoA) 
                             AND ((tblTipoInsumo.IDGrupoAcabado)= @Id_Acabado))">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="Plano" PropertyName="Text" Name="Plano"></asp:ControlParameter>
                                                                <asp:Parameter Name="AplicadoA"></asp:Parameter>
                                                                <asp:Parameter Name="Id_Acabado"></asp:Parameter>

                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                        <asp:SqlDataSource runat="server" ID="DsOrigenAcab1" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT 
                                         tblPanel.Descripcion_Panel,
                                         tblModulo.Descripcion_Modulo,
                                         tblPlano_Panel.Id_Plano,
                                         tblInsumo.Descripcion_Insumo,
                                         tblModulo.ID_Familia 
                                         FROM (tblPanel 
                                         INNER JOIN ((tblModulo 
                                         INNER JOIN ((tblTipoInsumo 
                                         INNER JOIN tblInsumo ON tblTipoInsumo.Id_TipoInsumo = tblInsumo.Id_TipoInsumo) 
                                         INNER JOIN tblModulo_Insumo ON tblInsumo.Id_Insumo = tblModulo_Insumo.Id_Insumo) ON tblModulo.Id_Modulo = tblModulo_Insumo.Id_Modulo) 
                                         INNER JOIN tblPanel_Modulo ON tblModulo.Id_Modulo = tblPanel_Modulo.Id_Modulo) ON tblPanel.Id_Numerico = tblPanel_Modulo.Id_PanelNum) 
                                         INNER JOIN tblPlano_Panel ON tblPanel.Id_Numerico = tblPlano_Panel.Id_PanelNum
                                         WHERE (((tblPlano_Panel.Id_Plano)=@Plano) 
                                         AND ((tblTipoInsumo.IDGrupoAcabado)=@IdGrupoAcab) 
                                         AND ((tblInsumo.Id_Insumo)= @ID_Insumo) 
                                         AND ((tblModulo.ID_Familia)=@IdFamilia))">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="Plano" PropertyName="Text" Name="Plano"></asp:ControlParameter>
                                                                <asp:Parameter Name="ID_Insumo"></asp:Parameter>
                                                                <asp:Parameter Name="IdGrupoAcab"></asp:Parameter>
                                                                <asp:Parameter Name="IdFamilia"></asp:Parameter>

                                                            </SelectParameters>
                                                        </asp:SqlDataSource>


                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                    </div>

                                    <div class="row g-2 pt-3">
                                        <div class=" col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                            <asp:TextBox ID="tbCodInventario" MaxLength="8" Enabled="false" CssClass="form-control form-control-sm" placeHolder="Codigo Inventario" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12">
                                            <asp:TextBox ID="tbDescripAcaba" Enabled="false" CssClass="form-control form-control-sm" placeHolder="Descripción" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-lg-2 col-md-2 col-sm-1 col-xs-12"></div>

                                        <div class="col-lg-2 col-md-2 col-sm-5 col-xs-12">
                                            <asp:CheckBox ID="chkAcabadoActivo" Enabled="false" ToolTip="Activo" runat="server" />
                                            <asp:Label ID="lbEstado" runat="server" Text="Estado Acabado"></asp:Label>
                                        </div>

                                        <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                            <asp:CheckBox ID="chkLinea" Enabled="false" ToolTip="Linea" runat="server" />
                                            <asp:Label ID="lbLinea" runat="server" Text="Linea"></asp:Label>
                                        </div>

                                    </div>

                                </div>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

        </div>

    </form>



    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
