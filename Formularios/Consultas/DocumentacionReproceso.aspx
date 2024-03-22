<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentacionReproceso.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Consultas.DocumentacionReproceso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/FormExtPrin/NitOTS.css" />
    <title>Documentación Reproceso</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <div class="container pb-3 mb-3  pt-3 mt-3">

            <div class="row justify-content-center  pb-2 mb-2 pt-3">
                <div class="border rounded p-1" style="margin-left: 2rem">
                    <div class="row">
                        <div class="col-12">
                            <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                <h5 class="datagrid-header text-center">Documentación Reproceso</h5>
                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridDocRepro" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="DSDocReproceso" OnItemCommand="DataGridDocRepro_ItemCommand">
                                    <Columns>

                                        <asp:TemplateColumn HeaderText="...">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView" runat="server" ToolTip="Seleccionar Documento" CommandName="VerDocumento" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>


                                        <asp:BoundColumn DataField="Archivo" HeaderText="Archivo" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Observacion" HeaderText="Observación" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="TipoDocumento" HeaderText="Tipo Documento" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="FechaRegistro" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="ID_Documento" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Id_OT" Visible="false" ItemStyle-CssClass="auto-width-column" />

                                        <asp:TemplateColumn HeaderText="...">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView1" ToolTip="Ver Documento" runat="server" CommandName="VerDocumento1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-eye'></i>" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="DSDocReproceso" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT * FROM tblDocumentacionOT WHERE ID_OT= @OT AND pedido= @Ped AND (TipoDocumento='REPROCESOS' )">
                                    <SelectParameters>
                                        <asp:Parameter Name="OT"></asp:Parameter>
                                        <asp:Parameter Name="Ped"></asp:Parameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>




                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-3">
                    <span id="ErrorValidacionDoc" style="color: red;" runat="server" visible="false"></span>
                </div>

                <div class="9">
                    <asp:TextBox ID="idDocmuento" runat="server" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="nombreArchivo" runat="server" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="NombreCarpeta1" runat="server" Visible="false"></asp:TextBox>
                </div>


            </div>

            <div class="row pt-3 mt-3">

                <div class="col-4">
                    <div class="input-group-sm gap-2  ">
                        <asp:Label ID="Label3" class=" col-form-label-sm" Text="Tipo Documento" runat="server"></asp:Label>
                        <asp:DropDownList class="form-control form-control-sm" ID="ddlTipoDoc" runat="server">
                            <asp:ListItem Value=""> </asp:ListItem>
                            <asp:ListItem Value="REPROCESOS">REPROCESOS</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-1"></div>

                <div class="col-6 pt-4">
                    <div class=" input-group input-group-sm gap-2  ">
                        <asp:FileUpload CssClass="form-control" ID="DocReproceso" runat="server" />

                    </div>
                </div>

            </div>

            <div class="row pt-2 mt-2">

                <div class="col-4">
                    <div class="input-group-sm">
                        <asp:Label ID="lbObs" runat="server" Text="Observacion"></asp:Label>
                        <asp:TextBox ID="tbObservacion" type="Text" class="form-control" runat="server"></asp:TextBox>
                    </div>

                </div>

                <div class="col-1">
                </div>

                <div class="col-2 pt-4 text-end">
                    <asp:Button runat="server" ID="btnAdjuntarDoc" Text="Adjuntar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-primary" Style="width: 5rem;" OnClick="btnAdjuntarDoc_Click" />
                </div>

                <div class="col-1">
                </div>

                <div class="col-2 pt-4 text-start">
                    <asp:Button runat="server" ID="btnElimnarDoc" Text="Eliminar" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-sm btn-outline-danger" Style="width: 5rem;" OnClick="btnElimnarDoc_Click" />
                </div>


            </div>

        </div>

    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
