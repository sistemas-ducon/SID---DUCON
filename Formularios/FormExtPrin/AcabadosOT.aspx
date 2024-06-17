<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcabadosOT.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.AcabadosOT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
     <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"/>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/AcabadosOT.css" rel="stylesheet" />
    <title>Acabados</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container mt-3 shadow p-3">
                            <div class="p-3 m-2 border shadow-sm" style="height: 22rem;">
                                <h5 class="datagrid-header text-center">Acabados</h5>

                                <div class="table-responsive mb-2 gap-2" style="height: 15.1rem; overflow-x: auto;">
                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm"
                                        ID="DataGrid1" runat="server" AutoGenerateColumns="false" >

                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="SelecOt" OnClick="DespieceAcabados_Click" runat="server" CommandName="Select" CommandArgument='<%# Container.ItemIndex %>'
                                                        Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="Aplica a:" DataField="GrupoObjetoParaAcabado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                            <asp:BoundColumn HeaderText="Acabado Definitivo" DataField="Descripcion_Acabado" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Detalle Adicional" DataField="Detalle_Adicional" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Acabado de Ventas" DataField="AcabadoVentas" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Entrega" DataField="Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>          
                                              <asp:BoundColumn DataField ="ID_GrupoObjetoParaAcabado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                              <asp:BoundColumn DataField ="Descripcion_Grupo" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                             <asp:BoundColumn DataField ="ID_GrupoAcabado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                              <asp:BoundColumn DataField ="ID_Acabado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                             <asp:BoundColumn DataField ="Id_OTAcabados" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>         
                                </div>

                                <div class="container-fluid  mt-3">
                                    <div class="row justify-content-between">
                                        <div class="col-6">
                                            <asp:Button ID="Button1" runat="server" Text="Eliminar Acabado" CssClass="btn btn-dark btn-sm" Enabled="false" OnClick="EliminarAcabado_Click"/>
                                        </div>
                                        <div class="col-6 text-end">
                                            <asp:Button ID="Button2" runat="server" Text="Cambiar Acabado" CssClass="btn btn-dark btn-sm" Enabled="false" />
                                        </div>
                                    </div>
                                </div>


                            </div>
                             
                        <div class="row justify-content-center">
                            <div class="p-3 m-2" style="height: 25rem;">
                                <div class="row">
                                    <div class="col-12">

                                        <div class="d-flex">

                                            <div class="col-4">
                                                <div class="p-1 m-1 border shadow-sm" style="height: 14rem;">
                                                    <h6>Aplicar Acabado a:</h6>

                                                    <div class="mb-2 gap-2" style="max-height: 11.5rem; overflow-x: auto;">

                                                        <asp:DataGrid CssClass="form-control-sm form-control border-white"
                                                            ID="DataGrid2" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource2"
                                                            ShowHeader="false" OnItemDataBound="DataGrid1_ItemDataBound">
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSelectRow" OnClick="lnkSelectRow_Click" runat="server" CommandName="Select" CommandArgument='<%# Container.ItemIndex %>'
                                                                            Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="GrupoObjetoparaAcabado" ItemStyle-CssClass="auto-width-column2"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ID_GrupoObjetoparaAcabado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>

                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                            SelectCommand="SELECT * FROM tblGrupoObjetoparaAcabado ORDER BY GrupoObjetoparaAcabado ASC;">
                                                            <SelectParameters>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-4">
                                                <div class="p-1 m-1 border shadow-sm" style="height: 14rem;">
                                                    <h6>Grupo de Acabado:</h6>

                                                    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false" CssClass="form-control-sm"></asp:Label>

                                                    <div class="mb-2 gap-2" style="max-height: 11.5rem; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="form-control-sm form-control border-white" ID="DataGrid4" runat="server" AutoGenerateColumns="false" ShowHeader="false" DataSourceID="SqlDataSource4" Visible="false">
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSelectRoww" runat="server" OnClick="lnkSelectRow4_Click" CommandName="Select" CommandArgument='<%# Container.ItemIndex %>'
                                                                            Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="ID_GrupoObjetoparaAcabado" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                                <asp:BoundColumn DataField="ID_GrupoAcabado" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                                <asp:BoundColumn DataField="Descripcion_Grupo" ItemStyle-CssClass="auto-width-column" />
                                                            </Columns>
                                                        </asp:DataGrid>
                                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                            SelectCommand="SELECT
                                                                tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado,
                                                                tblGrupodeAcabado.ID_GrupoAcabado,
                                                                tblGrupodeAcabado.Descripcion_Grupo
                                                            FROM
                                                                tblGrupodeAcabado
                                                            INNER JOIN
                                                                tblGrupoAcab_GrupoObjtAcab ON tblGrupodeAcabado.ID_GrupoAcabado = tblGrupoAcab_GrupoObjtAcab.ID_GrupoAcabado
                                                            INNER JOIN
                                                                tblGrupoObjetoParaAcabado ON tblGrupoAcab_GrupoObjtAcab.ID_GrupoObjetoparaAcabado = tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado
                                                            WHERE
                                                                tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado = '';">
                                                            <SelectParameters>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-4">
                                                <div class="p-1 m-1 border shadow-sm" style="height: 14rem;">
                                                    <h6>Acabado Definitivo:</h6>
                                                    <div class="mb-2 gap-2" style="max-height: 11.5rem; overflow-x: auto;">

                                                        <asp:DataGrid CssClass="form-control-sm form-control border-white"
                                                            ID="DataGrid3" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource3"
                                                            ShowHeader="false">
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSelectRoww3" runat="server" OnClick="lnkSelectRow3_Click" CommandName="Select" CommandArgument='<%# Container.ItemIndex %>'
                                                                            Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Acab" ItemStyle-CssClass="auto-width-column2"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Id_GrupoAcabado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="ID_Acabado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Descripcion_Acabado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="DeLinea" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>

                                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                                            ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                            SelectCommand="SELECT *, CONCAT(Descripcion_Acabado, ' (', Entrega, 'D)') AS Acab FROM tblacabado WHERE Id_GrupoAcabado = @ID_GrupoObjetoParaAcabado AND Activo = 1 ORDER BY Descripcion_Acabado ASC">
                                                            <SelectParameters>
                                                                <asp:Parameter Name="ID_GrupoObjetoParaAcabado" Type="String" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="d-flex">
                                                        <div class="col-11">
                                                            <div class="row">
                                                                <div class="col-10">
                                                                    <asp:Label ID="Label2" runat="server" Text="Aplicar Acabado a:" CssClass="col-form-label-sm"></asp:Label>
                                                                    <asp:Label ID="Label3" runat="server" Text="" Visible="false" CssClass="fw-bold form-control-sm"></asp:Label>
                                                                    <asp:Label ID="Label8" runat="server" Text="" Visible="false" CssClass="fw-bold form-control-sm"></asp:Label>
                                                                </div>
                                                                <div class="col-2">
                                                                    <asp:Label ID="Label9" runat="server" Text="Copiar Acab. del ped" CssClass="fw-bold"></asp:Label>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-12">
                                                                    <asp:Label ID="Label4" runat="server" Text="Acabado Definitivo:" CssClass="col-form-label-sm"></asp:Label>
                                                                    <asp:Label ID="Label5" CssClass="form-control-sm" runat="server" Text="" Visible="false"></asp:Label>
                                                                    <asp:Label ID="Label10" runat="server" Text="" Visible="false" CssClass="fw-bold form-control-sm"></asp:Label>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-10">
                                                                    <div class="input-group input-group-sm gap-2">
                                                                        <asp:Label ID="Label6" runat="server" Text="Detalle Adicional" CssClass="col-form-label-sm"></asp:Label>
                                                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-2">
                                                                    <asp:Button ID="Button3" runat="server" Text="Grabar Acabado" CssClass="btn btn-dark btn-sm" Enabled="false" OnClick="BtnGrabar_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-1 m-1">
                                                            <div class="d-flex flex-wrap">
                                                                <div class="col-8 p-2">

                                                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control shadow grande linkButtonClicked2" MaxLength="4"></asp:TextBox>
                                                                </div>
                                                                <div class="col-4 p-2">
                                                                    <asp:LinkButton runat="server" ID="BtnCopAca" CssClass="btn shadow btn-light linkButtonClicked grande" OnClick="BtnCopAca_Click">
                                                                        <i class="bi-floppy-fill" style="color: #0863a4;"></i>
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
                   
                </div>

        <div class="modal" id="miModalll" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Acabado de la obra</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Debe seleccionar a que le va aplicar el acabado y el acabado definitivo</span></p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

                  <div class="modal" id="ErrorCopAca" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Copiar Acabados</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p><span id="ErrorCopAca2"></span></p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

                <div class="modal" id="miModalError" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Error</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Debe seleccionar el acabado que desea eliminar</span></p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

      <div class="modal" id="DefinirAcabado" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Definir Acabado</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Esta seguro de aplicar a:  <span id="valorLabelSpan"></span> ?</span></p>
                    </div>
                    <div class="modal-footer d-flex align-items-center justify-content-center">
                        <asp:Button runat="server" Text="Si" class="btn btn-sm btn-outline-success" OnClick="BotonSi_Click" data-bs-dismiss="modal" aria-label="Close" />
                        <asp:Button runat="server" Text="No" class="btn btn-sm btn-outline-secondary" data-bs-dismiss="modal" aria-label="Close" />
                    </div>
                </div>
            </div>
        </div>

                   <div class="modal" id="EliminarAcabado" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Eliminar Acabado</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Esta seguro de Eliminar el acabado: <span id="valorLabelSpanEliminar"></span> ?</span></p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" Text="Si" class="btn btn-sm btn-outline-success" OnClick="BotonSiEliminar_Click" data-bs-dismiss="modal" aria-label="Close" />
                        <asp:Button runat="server" Text="No" class="btn btn-sm btn-outline-secondary" data-bs-dismiss="modal" aria-label="Close" />
                    </div>
                </div>
            </div>
        </div>

                                <!-- Modal -->
                <div id="myModal" class="modal">
                  <!-- Modal content -->
                  <div class="modal-content">
                    <span class="close">&times;</span>
                    <p><span id="spanMessage"></span></p>
                  </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

    <script type="text/javascript">
       function scrollDataGrid() {
           var grid = document.getElementById('<%= DataGrid2.ClientID %>');
           var rows = grid.getElementsByTagName("tr");

           for (var i = 0; i < rows.length; i++) {
               if (rows[i].getAttribute("data-selected") === "true") {
                   rows[i].scrollIntoView();
                   break;
               }
           }
       }
   </script>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
