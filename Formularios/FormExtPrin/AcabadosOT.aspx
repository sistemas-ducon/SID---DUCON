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

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/AcabadosOT.css" rel="stylesheet" />
    <title>Acabados</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <div class="p-3 m-2 border" style="height: 28rem;">
                                <h6 class="text-center">Acabados</h6>

                                <div class="table-responsive mb-2 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm"
                                        ID="DataGrid1" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource1">

                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="SelecOt" runat="server" OnClick="lnkSelectRow_Click" CommandName="Select" CommandArgument='<%# Container.ItemIndex %>'
                                                        Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="Aplica a:" DataField="GrupoObjetoparaAcabado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                            <asp:BoundColumn HeaderText="Acabado Definitivo" DataField="Descripcion_Acabado" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Detalle Adicional" DataField="Detalle_Adicional" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Acabado de Ventas" DataField="AcabadoVentas" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Entrega" DataField="Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA%>"
                                        SelectCommand="SELECT GOA.GrupoObjetoparaAcabado,  AC.Descripcion_Acabado, A.Detalle_Adicional, A.AcabadoVentas, AC.Entrega
                                FROM tblOTAcabados A
                                INNER JOIN tblGrupoObjetoParaAcabado GOA ON A.ID_GrupoObjetoparaAcabado = GOA.ID_GrupoObjetoparaAcabado
                                INNER JOIN tblAcabado AC ON A.ID_Acabado = AC.ID_Acabado
                                 WHERE Id_OT = @Id_OT">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="Id_OT" SessionField="Id_OT" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                <div class="container-fluid  mt-3">
                                    <div class="row justify-content-between">
                                        <div class="col-6">
                                            <asp:Button ID="Button1" runat="server" Text="Eliminar Acabado" CssClass="btn btn-dark btn-sm" enabled="false"/>
                                        </div>
                                        <div class="col-6 text-end">
                                            <asp:Button ID="Button2" runat="server" Text="Cambiar Acabado" CssClass="btn btn-dark btn-sm" enabled="false"/>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                    <div class="container-fluid m-2">
                        <div class="row justify-content-center">
                            <div class="border rounded p-2" style="height: 25rem;">
                                <div class="row">
                                    <div class="col-12">

                                        <div class="d-flex">

                                            <div class="col-4">
                                                <div class="p-1 m-1 border" style="height: 14rem;">
                                                    <h6>Aplicar Acabado a:</h6>

                                                    <div class="mb-2 gap-2" style="max-height: 11.5rem; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="form-control-sm form-control border-white"
                                                            ID="DataGrid2" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource2"
                                                            ShowHeader="false">
                                                            <Columns>
                                                                <asp:BoundColumn DataField="GrupoObjetoparaAcabado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>

                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                                            ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA%>"
                                                            SelectCommand="select GrupoObjetoparaAcabado from tblGrupoObjetoParaAcabado"></asp:SqlDataSource>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-4">
                                                <div class="p-1 m-1 border" style="height: 14rem;">
                                                    <h6>Grupo de Acabado:</h6>
                                                    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false" CssClass="form-control-sm"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-4">
                                                <div class="p-1 m-1 border" style="height: 14rem;">
                                                    <h6>Acabado Definitivo:</h6>
                                                    <textarea id="TextArea1" runat="server" cols="20" rows="2" class="form-control form-control-sm border-white" style="height: 11.5rem;"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-11">                                      
                                                    <div class="row">
                                                        <div class="col-10">
                                                            <asp:Label ID="Label2" runat="server" Text="Aplicar Acabado a:" CssClass="col-form-label-sm"></asp:Label>
                                                            <asp:Label ID="Label3" runat="server" Text="" Visible="false" CssClass="fw-bold"></asp:Label>
                                                        </div>
                                                          <div class="col-2">
                                                            <asp:Label ID="Label9" runat="server" Text="Copiar Acab. del ped" CssClass="fw-bold"></asp:Label>                                                    
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-12">
                                                            <asp:Label ID="Label4" runat="server" Text="Acabado Definitivo:" CssClass="col-form-label-sm"></asp:Label>
                                                            <asp:Label ID="Label5" runat="server" Text="" Visible="false"></asp:Label>
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
                                                            <asp:Button ID="Button3" runat="server" Text="Grabar Acabado"  CssClass="btn btn-dark btn-sm" enabled="false"/>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-1">                                               
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                            <asp:Label ID="Label7" runat="server" Text="0" CssClass="border p-3 mt-2 shadow"></asp:Label>

                                                                 <asp:LinkButton runat="server" ID="button10" Enabled="false">
                                                <i class="bi bi-file-earmark btn btn-sm border p-3 mt-2 shadow grande"></i> 
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>




    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
