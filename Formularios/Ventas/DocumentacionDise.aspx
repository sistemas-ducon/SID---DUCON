<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentacionDise.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.DocumentacionDise" %>

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

    <link type="text/css" href="../../Recursos/CSS/Ventas/DocumentacionDise.css" rel="stylesheet" />
    <title>Documentacion - Departamento de Ventas</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container mt-3 shadow" style="height: 50rem;">
                    <h5 class="text-center">Documentación bitacora</h5>

                    <div class="container">
                        <div class="row m-1">

                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                <div class="row justify-content-center">
                                    <div class="rounded p-1 col-md-12 col-12 shadow-sm border" style="height: auto; min-height: 30rem;">

                                        <div class="table-responsive mb-2 gap-2">

                                            <asp:DataGrid CssClass="table table-bordered table-sm custom-grid table-hover custom-data-grid form-control-sm"
                                                ID="DataGridDocumento" runat="server" AutoGenerateColumns="false"
                                                DataSourceID="SqlDataSource3" DataKeyField="Id_OT">
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="lnkSelectRow" runat="server"
                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" OnClick="lnkSelectRow_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Archivo" HeaderText="Archivo" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Observacion" HeaderText="Observacion" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="FechaRegistro" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Id_OT" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkViewFile" runat="server"
                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-eye'></i>" OnClick="lnkViewFile_Click" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>

                                                </Columns>
                                            </asp:DataGrid>

                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                SelectCommand="SELECT TOP 0 Archivo, Observacion, Usuario, FechaRegistro, Id_OT FROM tblDocumentacion"></asp:SqlDataSource>

                                        </div>


                                    </div>
                                </div>

                            </div>
                                        </div>
                                        <div class="row mt-2">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm ">
                                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                                                    <asp:Button ID="GuardarButton" runat="server" Text="Guardar" OnClick="GuardarButton_Click" />
                                                    <asp:Button ID="BtnEliminar" runat="server" Text="Eliminar" OnClick="BtnEliminar_Click" />

                                                </div>
                                            </div>
                                        </div>
                                        <div class="row mt-1">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <textarea id="TextArea1" runat="server" rows="8" class="form-control shadow-sm"></textarea>
                                            </div>
                                        </div>

                                        <div class="modal fade" id="miModalDoc" tabindex="-1" role="dialog" aria-labelledby="miModalLabel" aria-hidden="true">
                                            <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h5 class="modal-title" id="miModalLabelDoc">Título del Modal</h5>


                                                    </div>
                                                    <div class="modal-body">
                                                        <p>Desea Guardar el archivo?</p>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <asp:Button runat="server" type="button" data-dismiss="modal" Text="Si"></asp:Button>
                                                        <asp:Button runat="server" type="button" data-dismiss="modal" Text="No"></asp:Button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="GuardarButton" />
                <asp:PostBackTrigger ControlID="BtnEliminar" />
                <asp:PostBackTrigger ControlID="DataGridDocumento" />
            </Triggers>
        </asp:UpdatePanel>

    </form>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
