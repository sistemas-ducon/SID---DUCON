<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdjuntarDocumentos.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.AdjuntarDocumentos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
       <link rel="stylesheet" href="../../Recursos/CSS/Ventas/AdjuntarDocumento.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">

            <h4 class="text-center p-3 m-3">
                <asp:Literal runat="server" ID="TituloSolictud"></asp:Literal></h4>

            <div class="row justify-content-center p-2 m-2">
                <div class="border rounded p-2">
                    <div class="row">
                        <div class="col-12">
                            <div class="table-responsive mb-2 gap-2" style="max-height: 12rem; overflow-x: auto;">
                                <h6 class="datagrid-header text-start">Documentacion Detalle</h6>
                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGridDocumento" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemDataBound="DataGridDocumento_ItemDataBound" OnItemCommand="DataGridDocumentosPE_LinkButton">
                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="...">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView" runat="server" ToolTip="Seleccionar Documento" CommandName="VerDocumento" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>

                                        <asp:BoundColumn DataField="Archivo" HeaderText="Archivo" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Observacion" HeaderText="Observacion" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="TipoDocumento" HeaderText="Tipo Documento" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="FechaRegistro" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="MuebleEspecial" HeaderText="Esp" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="ID_Documento" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Id_OT" Visible="false" ItemStyle-CssClass="auto-width-column" />

                                        <asp:TemplateColumn HeaderText="...">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView1" ToolTip="VerDocumento" runat="server" CommandName="VerDocumento1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-eye'></i>" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>



                                    </Columns>
                                </asp:DataGrid>
                                <asp:SqlDataSource ID="Documentos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT * FROM tblDocumentacion WHERE Id_OT = @Documentacion">
                                    <SelectParameters>
                                        <asp:Parameter Name="Documentacion" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>





                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row p-2 m-2">

                <div class="col-6">
                    <div class=" input-group input-group-sm gap-2  ">
                        <asp:Label ID="lbTipoDoc" class=" col-form-label-sm" Text="Tipo Documento" runat="server"></asp:Label>
                        <asp:DropDownList class="form-control form-control-sm" ID="ddlTipoDoc" runat="server">
                            <asp:ListItem Value=" ">-- Seleccione --</asp:ListItem>
                            <asp:ListItem Value="BOSQUEJO">BOSQUEJO</asp:ListItem>
                            <asp:ListItem Value="CONTABLE">CONTABLE</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-6">

                    <div class="input-group input-group-sm ">
                        <asp:FileUpload CssClass="form-control" ID="FileUpload1" runat="server" />
                        <asp:Button ID="Button1" CssClass="btn btn-outline-primary" runat="server" Text="Adjuntar" OnClick="AdjuntarDocumento" />
                        <asp:Button ID="bntElimnar" CssClass="btn btn-outline-danger" runat="server" Text="Elimnar" OnClick="EliminarDocumento" />
                    </div>

                    <label id="mensaje" runat="server"></label>

                </div>

            </div>

        </div>




    </form>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

 


</body>
</html>
