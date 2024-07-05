<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Objetos.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.Objetos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>Objetos</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/Objetos.css" rel="stylesheet" />
</head>
<body translate="no">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <asp:UpdatePanel ID="panelObjeto" runat="server">
            <ContentTemplate>

                <div class="container-fluid p-1 m-1 ">

                    <nav class="navbar navbar-expand-sm navbar-light bg-light p-1 ">
                        <div class="container-fluid">

                            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2"
                                aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                <span class="navbar-toggler-icon"></span>
                            </button>
                            <div class="collapse navbar-collapse" id="ObjetosIconos">
                                <ul class="navbar-nav mx-auto">

                                    <div class="contenedor-icono">

                                        <asp:LinkButton runat="server" title="Nuevo Panel" ID="BtnNuePan">
                                                    <i class="bi bi-file-earmark"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" title="Grabar" ID="Grabar">
                                                 <%-- <i class="bi bi-save2"></i>--%>
                                            <i class="bi bi-sd-card-fill"></i> <%--Icono Guardar--%>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" title="Modificar Panel" ID="BtnModPan">
                                                   <i class="bi bi-wrench"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" title="Eliminar Panel" ID="BtnEliPan">
                                                   <i class="bi bi-database-x"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" title="Buscar Panel por Descripcion" ID="BtnBuscas">
                                                  <i class="bi bi-file-earmark-ruled"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" title="Copiar Panel" ID="CopiarPanel">
                                                   <i class="bi bi-files"></i>
                                        </asp:LinkButton>


                                    </div>
                            </div>
                    </nav>

                    <div class="row p-2 m-2 pt-2 pb-2 mb-2 border shadow-sm rounded">

                        <div class="col-sm-2">
                            <div class="form-check">
                                <asp:RadioButtonList ID="rbObjeto" runat="server">
                                    <asp:ListItem Selected="True" Value="Objeto">Por Objeto </asp:ListItem>
                                    <asp:ListItem Value="Descripcion">Por Descripción</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                        </div>

                        <div class="col-sm-2">
                            <div class="input-group-sm">
                                <asp:Label class="form-label" Text="Grupo" runat="server" ID="lbGrupo"></asp:Label>
                                <asp:DropDownList class="form-control" ID="ddlGrupo" runat="server" DataTextField="Descripcion" DataValueField="Descripcion" OnDataBound="ddlGrupoObjeto_DataBound" DataSourceID="GrupoObjetos" AutoPostBack="true"></asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="GrupoObjetos" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select  ID_GrupoObjeto AS Valor,Descripcion_Grupo AS Descripcion from tblGrupoObjeto  order by Descripcion_Grupo "></asp:SqlDataSource>

                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="input-group-sm">
                                <asp:Label class="form-label" Text="Criterio" runat="server" ID="lbCriterio"></asp:Label>
                                <asp:TextBox ID="tbCriterio" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="input-group-sm">
                                <asp:Label class="form-label" Text="Altura." runat="server" ID="lbAltura"></asp:Label>
                                <asp:TextBox ID="tbAltura" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="input-group-sm">
                                <asp:Label class="form-label" Text="Ancho" runat="server" ID="lbAncho"></asp:Label>
                                <asp:TextBox ID="tbAncho" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3 pt-4 pb-2">
                            <div class="input-group-sm">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-sm btn-outline-secondary" OnClick="BuscarObjeto" />
                                <asp:HiddenField ID="Id_Objeto_Hid" runat="server" />
                                <asp:HiddenField ID="Nombre_Objeto_Hid" runat="server" />
                                <asp:HiddenField ID="Ancho_Objeto_Hid" runat="server" />

                            </div>
                        </div>


                    </div>

                    <div class="row d-flex p-1 m-1 pt-3 justify-content-between">

                        <div class="col-8 border rounded shadow-sm">
                            <div class="row justify-content-center p-2" style="height: 31rem">
                                <div class="border rounded">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-1" style="max-height: 30rem; height: 31rem; overflow-x: auto;">
                                                <h5 class="datagrid-header-title text-center">Paneles</h5>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" ID="DataGridObjetos" AutoGenerateColumns="false" runat="server" DataSourceID="ObtenerDatosObjetos" OnItemCommand="DataGridObtenerDatosObjetos_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkObjetoDetallado" runat="server" CommandName="VerObjetoDet" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Id_Panel" HeaderText="Id Objeto" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Panel" HeaderText="Descripcion" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Ancho" HeaderText="Ancho" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Linea" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Grupo" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_Numerico" Visible="false" />
                                                        <asp:BoundColumn DataField="Precio_Venta" Visible="false" />
                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource ID="ObtenerDatosObjetos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="sp_ObtenerDatosObjetoActivo" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbCriterio" PropertyName="Text" DefaultValue="%" Name="Criterio" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbAltura" PropertyName="Text" DefaultValue="%" Name="Altura" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbAncho" PropertyName="Text" DefaultValue="%" Name="Ancho" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="ddlGrupo" PropertyName="SelectedValue" DefaultValue="%" Name="Grupo" Type="String"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-4 border rounded shadow-sm ml-3">
                            <div class="row">
                                <div class="col-1 p-0 m-0"></div>
                                <div class="col-10">
                                    <div class="input-group-sm">
                                        <asp:Label class="form-label" Text="Observaciones:" runat="server" ID="lbObservaciones"></asp:Label>
                                        <textarea class="form-control form-control-sm" id="txObs1" runat="server" cols="4" rows="4"></textarea>
                                    </div>
                                </div>
                                <div class="col-1"></div>
                            </div>

                            <div class="row pt-2 mt-2 pb-1 mb-1">
                                <div class="col-1"></div>
                                <div class="col-10">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="D. Altura" runat="server" ID="lbdAltura"></asp:Label>
                                        <asp:TextBox ID="tbAlturaD" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-1"></div>
                            </div>

                            <div class="row pb-1 mb-1">
                                <div class="col-1"></div>
                                <div class="col-10">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="D. Ancho" runat="server" ID="lbAnchoD"></asp:Label>
                                        <asp:TextBox ID="tbAnchoD" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-1"></div>
                            </div>

                            <div class="row pb-1 mb-1">
                                <div class="col-1"></div>
                                <div class="col-10">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Cantidad" runat="server" ID="lbCantidad"></asp:Label>
                                        <asp:TextBox ID="tbCantidad" type="number" class="form-control" runat="server" OnTextChanged="tbCantidad_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-1"></div>
                            </div>

                            <div class="row">
                                <div class="col-1"></div>
                                <div class="col-10">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="P. Venta" runat="server" ID="lbPrecioVenta"></asp:Label>
                                        <asp:TextBox ID="tbPrecioVenta" type="number" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-1"></div>
                            </div>

                            <div class="row text-center">
                                <span id="ErrorValidacionCantidad" style="color: red;"></span>
                            </div>

                            <div class="row pt-4 mt-4 justify-content-center">
                                <div class="col-2">
                                </div>
                                <div class="col-5">
                                    <div class="input-group input-group-sm">
                                        <asp:Button ID="Adicionar" runat="server" Text="Adicionar" class="bi bf btn btn-outline-secondary" OnClick="Adicionar_Click" OnClientClick="return ValidarFormularioCantidad();" Style="width: 5.5rem;" />
                                    </div>
                                </div>

                                <div class="col-5">
                                    <div class="input-group input-group-sm">
                                        <asp:Button ID="Cerrar" runat="server" Text="Cerrar" class="bi bf btn btn-outline-secondary" OnClientClick="enviarFormulario();" Style="width: 5.5rem;" />
                                    </div>
                                </div>

                                <div class="col-2"></div>
                            </div>
                        </div>

                    </div>


                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </form>


    <script>
        function enviarFormulario() {
            // Realiza el procesamiento necesario en el formulario 2
            window.close();
            // Actualiza el formulario 1
            window.opener.location.reload(); // Recarga el formulario padre

            $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Objetos.aspx/ControlTapPlano", // La URL debe apuntar al método en el servidor
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // La llamada al servidor fue exitosa, puedes realizar acciones adicionales aquí
                },
                error: function (error) {
                    // Manejar errores si los hay
                }
            });


        }

        function ValidarFormularioCantidad() {
            var isValid = true;

            var cantidad = document.getElementById("tbCantidad").value;

            // Validar si la cantidad está vacía
            if (cantidad === "") {
                ErrorValidacionCantidad.innerHTML = "El campo cantidad es obligatorio.";
                isValid = false;
            }
            // Validar si la cantidad no es un número
            else if (isNaN(cantidad)) {
                ErrorValidacionCantidad.innerHTML = "El campo cantidad debe ser un número.";
                isValid = false;
            } else {
                // Limpiar el mensaje de error si la validación es exitosa
                ErrorValidacionCantidad.innerHTML = "";
            }

            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
        }

        function focusAndScrollToRow(rowId) {
            var row = document.getElementById(rowId);
            if (row) {
                row.setAttribute('tabindex', '-1'); // Make it focusable
                row.focus();
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });


            }
        }

    </script>



</body>
</html>
