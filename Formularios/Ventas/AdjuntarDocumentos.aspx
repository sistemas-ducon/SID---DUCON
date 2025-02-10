<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdjuntarDocumentos.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.AdjuntarDocumentos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/AdjuntarDocumento.css" />
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
    <title>Documentos Solicitud Especial</title>

</head>
<body translate="no">
    <form id="form1" runat="server">

        <div class="container-fluid border shadow mt-5" style="width: 95%;">

            <div class="row">
                <div class="col-md-12">
                    <h4 class="text-center p-1 mt-2">
                        <asp:Literal runat="server" ID="TituloSolictud"></asp:Literal></h4>
                </div>
            </div>

            <div class="row justify-content-center p-2 m-2">
                <div class="border rounded p-2">
                    <div class="row">
                        <div class="col-12">
                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; height: 25rem; overflow-x: auto;">
                                <h6 class="datagrid-header text-start">Documentacion Detalle</h6>
                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGridDocumento" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemDataBound="DataGridDocumento_ItemDataBound" OnItemCommand="DataGridDocumentosPE_LinkButton">
                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="...">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" ToolTip="Seleccionar Documento" CommandName="VerDocumento" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
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
                                <asp:SqlDataSource ID="Documentos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblDocumentacion WHERE Id_OT = @Documentacion">
                                    <SelectParameters>
                                        <asp:Parameter Name="Documentacion" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>





                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row text-center pb-1 mb-1">
                <div class="col-6">
                </div>
                <div class="col-6" style="font-size: 1.1rem;">
                    <asp:Label CssClass=" alert-success" ID="mensaje" runat="server" Text="" Visible="false"></asp:Label>

                </div>

            </div>

            <div class="row p-2 m-2 g-2">

                <div class="col-lg-6 col-md-6 col-sm-12">
                    <div class=" input-group input-group-sm gap-2  ">
                        <asp:Label ID="lbTipoDoc" class=" col-form-label-sm" Text="Tipo Documento" runat="server"></asp:Label>
                        <asp:DropDownList class="form-control form-control-sm" ID="ddlTipoDoc" runat="server" onchange="ddlTipoDocChanged()">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-lg-6 col-md-6 col-sm-12">

                    <div class="input-group input-group-sm gap-2 ">
                        <asp:FileUpload CssClass="form-control" ID="FileUpload1" runat="server" />
                        <asp:Button ID="ValidarEspecial" CssClass="btn btn-outline-success" runat="server" Text="ValidarEspecial" OnClick="ValidarEspecial_Click" />
                        <asp:Button ID="Button1" CssClass="btn btn-outline-primary" runat="server" Text="Adjuntar" OnClick="AdjuntarDocumento" OnClientClick="return validarDocumento();" />
                        <asp:Button ID="bntElimnar" CssClass="btn btn-outline-danger" runat="server" Text="Elimnar" OnClick="EliminarDocumento" OnClientClick="return ValidarEliminacion(event);" />
                    </div>



                </div>

            </div>

            <!--Modal Informativo Documentacion Especial -->
            <div id="ModalInfoEspecial" class="modal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" style="display: none;">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header bg-danger text-white">
                            <h5 class="modal-title text-center">Documentación Especial</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>

                        </div>
                        <div class="modal-body border rounded">
                            <div class="container-fluid">
                                <h5>!Estimado usuario! Recuerde...</h5>
                                <h6>Para adjuntar el archivo de excel generado por dibujo para desarrollo especial, debe validar el documento primero.  ¡Gracias!</h6>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                            </div>

                        </div>
                    </div>
                </div>
            </div>

          

        </div>

        <script type="text/javascript">

            //funcion para cambio de documento y ocultar Validar Especial si no es necesario 
            function ddlTipoDocChanged() {

                var ddlTipoDoc = document.getElementById('<%= ddlTipoDoc.ClientID %>');
                 var validarEspecial = document.getElementById('<%= ValidarEspecial.ClientID %>');

                if (ddlTipoDoc.value !== '') {
                    validarEspecial.style.display = 'none';

                } else {
                    validarEspecial.style.display = 'block';

                }
            }
        </script>

        <script>


            // Mostrar y Ocultar  acabados plano
            function mostrarModal() {
                $('#ModalInfoEspecial').modal('show');
            }

            document.addEventListener('DOMContentLoaded', function () {
                var AreaDepar = '<%= Session["Departamento"] %>';
                if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {
                    if (!sessionStorage.getItem('modalShown')) {
                        setTimeout(function () { mostrarModal(); }, 500);
                        sessionStorage.setItem('modalShown', 'true');
                    }
                }

            });
        </script>

        <script type="text/javascript">
            function validarDocumento() {
                var tipoDocumento = document.getElementById('<%= ddlTipoDoc.ClientID %>').value; // Asegúrate de reemplazar "ddlTipoDocumento" con el ID correcto de tu dropdown o input

                if (tipoDocumento === " " || tipoDocumento === "--Seleccione--") {
                    alert("Debe seleccionar un tipo de documento antes de adjuntar.");
                    ddlTipoDoc.focus();
                    return false; // Evita que se ejecute el evento OnClick del servidor
                }
                return true; // Permite que se ejecute el evento OnClick del servidor
            }

            // funcion para validar confirmar eliminacion 
            function ValidarEliminacion(event) {
                var mensaje = "Está seguro que desea eliminar el archivo seleccionado ? ";

                var result = confirm(mensaje);
                if (result) {

                    $(event.target).removeAttr('onclick');
                    $(event.target).click();
                }
                return false;
            }
        </script>


    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>




</body>
</html>
