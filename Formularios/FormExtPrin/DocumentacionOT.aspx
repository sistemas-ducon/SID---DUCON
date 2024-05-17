<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentacionOT.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.DocumentacionOT" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/FormExtPrin/NitOTS.css" />
    <title>Documentación OT</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />


</head>
<body translate="no">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <asp:UpdatePanel ID="Panel_DocOt" runat="server" UpdateMode="Conditional" DefaultButton="btnSubmit">
            <ContentTemplate>

                <div class=" container pb-3 mb-3  pt-1 mt-1 ">

                    <div class="row text-center pt-2 mt-2">
                        <h4>Documentos Orden de Trabajo</h4>
                    </div>

                    <!--Fila checkBox-->
                    <div class="row p-2 mt-2  text-end">
                        <div class="col-12">
                            <asp:CheckBox ID="chxDocumento" runat="server" OnCheckedChanged="chxDocumento_CheckedChanged" AutoPostBack="true" />
                            <asp:Label ID="Label4" runat="server" Text="Documentación completa"></asp:Label>
                        </div>
                    </div>

                    <div class="row justify-content-center  pb-2 mb-2">
                        <div class="border rounded p-1" style="margin-left: 2rem">
                            <div class="row">
                                <div class="col-12">
                                    <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                        <h5 class="datagrid-header text-center">Documentación</h5>
                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridDoc" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="DocumentacionFiltrada" OnItemCommand="DataGridDoc_ItemCommand" OnItemDataBound="DataGridDoc_ItemDataBound">
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
                                                <asp:BoundColumn DataField="MuebleEspecial" HeaderText="Esp" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="ID_Documento" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Id_OT" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Pedido" Visible="false" ItemStyle-CssClass="auto-width-column" />

                                                <asp:TemplateColumn HeaderText="...">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkView1" ToolTip="VerDocumento" runat="server" CommandName="VerDocumento1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-eye'></i>" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:SqlDataSource runat="server" ID="DocumentosOt" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select * from tblDocumentacion where ID_OT= @IdOt">
                                            <SelectParameters>
                                                <asp:Parameter Name="IdOt" Type="string" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                        <asp:SqlDataSource runat="server" ID="DocumentacionFiltrada" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select * from tblDocumentacion where ID_OT= @IdOt and Pedido = @Pedido">
                                            <SelectParameters>
                                                <asp:Parameter Name="IdOt" Type="String" />
                                                <asp:Parameter Name="Pedido" Type="String" />
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


                    <div class="row  pb-1 mb-1">

                        <div class="col-5">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="Label1" class=" col-form-label-sm" Text="Tipo Documento" runat="server"></asp:Label>
                                <asp:DropDownList class="form-control form-control-sm" ID="ddlTipoDoc" runat="server" onchange="ddlTipoDocChanged()">
                                    <asp:ListItem Value="">-- Seleccione --</asp:ListItem>
                                    <asp:ListItem Value="CONTROL DIBUJO">CONTROL DIBUJO</asp:ListItem>
                                    <asp:ListItem Value="CONTABLE">CONTABLE</asp:ListItem>
                                    <asp:ListItem Value="OPERATIVO">OPERATIVO</asp:ListItem>
                                    <asp:ListItem Value="PRODUCTIVO">PRODUCTIVO</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-1">
                        </div>

                        <div class="col-6">
                            <div class="input-group input-group-sm ">
                                <asp:FileUpload CssClass="form-control" ID="DoctOT" runat="server" />
                                <asp:Button ID="ValidarEspecial" CssClass="btn btn-outline-success" runat="server" Text="ValidarEspecial" OnClick="ValidarEspecial_Click" />
                                <asp:Button ID="btnAdjuntar" CssClass="btn btn-outline-primary" runat="server" Text="Adjuntar" OnClick="btnAdjuntar_Click" OnClientClick="return ValidarAdjuntar();" />
                                <asp:Button ID="bntElimnar" CssClass="btn btn-outline-danger" runat="server" Text="Elimnar" OnClick="bntElimnar_Click" OnClientClick="return ValidarEliminacion(event);" />
                            </div>

                        </div>


                    </div>

                    <div class="row pb-1 mb-1">

                        <div class="col-3">
                            <div class="input-group-sm gap-2  ">
                                <asp:Label ID="lbObservacion" Text="Observación" runat="server"></asp:Label>
                                <asp:TextBox ID="tbObservacion" type="Text" class="form-control " runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3">
                            <div class="input-group-sm gap-2  ">
                                <asp:Label ID="lbCategoria" Text="Categoría" runat="server"></asp:Label>
                                <asp:TextBox ID="tbCategoria" type="Text" class="form-control " runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3 pt-4">
                            <asp:CheckBox ID="chxMespecial" runat="server" />
                            <asp:Label ID="lbMespecial" runat="server" Text="M. Especial"></asp:Label>
                        </div>

                        <div class="col-3">
                            <div class="input-group-sm gap-2  ">
                                <asp:Label ID="lbCantidad" Text="Cantidad" runat="server"></asp:Label>
                                <asp:TextBox ID="tbCantidad" type="number" class="form-control " runat="server"></asp:TextBox>
                                <span id="ErrorValidacionDoc" style="color: red;" runat="server"></span>
                            </div>
                        </div>

                    </div>

                </div>

                <div class="container pt-1 mt-1">

                    <div class="row text-center pt-2 mt-2">
                        <h4>Documentación Solicitudes Especiales</h4>
                    </div>

                    <div class="row ">

                        <div class="col-7">
                            <div class="row justify-content-center pt-4  pb-1 mb-1">
                                <div class="border rounded p-1" style="margin-left: 2rem">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Documentos Solicitudes Especiales </h5>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridSolicitudEspecial" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="DocEspeciales" OnItemCommand="DataGridSolicitudEspecial_ItemCommand">
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" ToolTip="Seleccionar Documento" CommandName="VerDocumento3" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:BoundColumn DataField="Archivo" HeaderText="Archivo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="TipoDocumento" HeaderText="Tipo Archivo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="" HeaderText="Subido" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="DocEspeciales" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select Archivo, TipoDocumento from tbldocumentacion where Id_OT like '%PE' + @solicitud +'-%' And TipoDocumento <> 'BOSQUEJO'">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbSolicitud" PropertyName="Text" Name="solicitud"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>



                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-1"></div>

                        <div class="col-4">
                            <div class="row pt-1 mt-1 pb-1 mb-1">
                                <div class="col-8">
                                    <div class="input-group-sm gap-2 ">
                                        <asp:Label ID="lbSolicitud" class="form-check-label" Text="Solicitud" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbSolicitud" type="text" class="form-control" runat="server" AutoPostBack="true"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm pt-4">
                                        <asp:Button ID="btnSubirAdjuntar" type="button" Text="Adjuntar Especial" class="btn btn-outline-secondary" runat="server" OnClick="btnSubirAdjuntar_Click" OnClientClick="return ValidarCantidad();"></asp:Button>
                                    </div>
                                </div>
                            </div>
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


            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="btnAdjuntar" />
                <asp:PostBackTrigger ControlID="bntElimnar" />
                <asp:PostBackTrigger ControlID="DataGridDoc" />
                <asp:PostBackTrigger ControlID="ValidarEspecial" />
            </Triggers>

        </asp:UpdatePanel>


    </form>


    <script>
        //funcion para validar la cantidad al copiar un documento especial 
        function ValidarCantidad() {

            var cantidad = document.getElementById("tbCantidad").value;
            var tipodoc = document.getElementById("ddlTipoDoc").value;

            var isValid = true;

            if (tipodoc === "DLLO.ESPECIAL" && cantidad <= 0) {
                ErrorValidacionDoc.innerHTML = "Por favor ingrese la cantidad.";
                isValid = false;
            } else {

                isValid = true;
            }

            return isValid;

        }

        // funcion para validar si seleccion de tipo doc y si es  especial la cantidad.
        function ValidarAdjuntar() {

            var tipodoc = document.getElementById("ddlTipoDoc").value;
            var cantidad = document.getElementById("tbCantidad").value;

            var isValid = true;

            if (tipodoc === "DLLO.ESPECIAL" && cantidad <= 0) {
                ErrorValidacionDoc.innerHTML = "Por favor ingrese la cantidad.";
                isValid = false;
            } else if (tipodoc === "") {
                ErrorValidacionDoc.innerHTML = "Por Favor seleccione el tipo de documento.";
                isValid = false;
            } else {
                isValid = true;
            }
            return isValid;
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
        function ocultarModal() {
            $('#ModalInfoEspecial').modal('hide');
        }
    </script>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            if (!sessionStorage.getItem('modalShown')) {
                setTimeout(function () { mostrarModal(); }, 500);
                sessionStorage.setItem('modalShown', 'true');
            }
        });
    </script>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>


</body>
</html>
