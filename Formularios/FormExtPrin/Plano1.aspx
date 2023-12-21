<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Plano1.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.Plano1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Plano</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/Plano1.css" rel="stylesheet" />

    <script>
        function confirmarBloquearPlano(event) {

            var plano = document.getElementById("tbPlano").value;

            var mensaje = "Estás seguro que deseas bloquear o desbloquear el plano " + plano + " ? ";
            var result = confirm(mensaje);
            if (!result) {
                event.preventDefault(); // Cancelar el postback
            }
            return result; // Devolver el resultado de la confirmación
        }

        function confirmarEliminarPlano(event) {

            var plano = document.getElementById("tbPlano").value;

            var mensaje = "Estás seguro que deseas eliminar el plano " + plano + " ? ";
            var result = confirm(mensaje);
            if (!result) {
                event.preventDefault(); // Cancelar el postback
            }
            return result; // Devolver el resultado de la confirmación
        }



    </script>



</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="panelPlano1" runat="server">
            <ContentTemplate>
                <div class="container-fluid">

                    <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
                        <div class="container-fluid">

                            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                <span class="navbar-toggler-icon"></span>
                            </button>

                            <div class="collapse navbar-collapse" id="ejemplo2">
                                <ul class="navbar-nav mx-auto contenedor-icono">
                                    <div class="contenedor-icono">


                                        <asp:LinkButton runat="server" Text="Nuevo Plano" ID="NuevoPlano" OnClick="NuevoPlano_Click" title="Nuevo Plano">
                                                      <i class="bi bi-file-earmark"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" Text="Guardar Plano" ID="GurdarPlano" title="Guardar Plano" OnClick="GuardarModifcarPlano" OnClientClick=" return validarFormulario();">
                                                  <i class="bi bi-save2"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" Text="Modificar Plano" ID="ModificarPlano" OnClick="ModificarPlano_Click" title="Modificar Plano">
                                                   <i class="bi bi-wrench"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" Text="Bloqueado" ID="Bloqueado" title="Bloquear o Desbloquear Plano" OnClick="Bloqueado_Click" OnClientClick="return confirmarBloquearPlano(event);">
                                                  <i class="bi bi-lock-fill"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" Text="Anular o Eliminar Plano" ID="ELiminarPlano" title="Eliminar Plano" OnClick="EliminarPlano_Click" OnClientClick="return confirmarEliminarPlano(event);">
                                                 <i class="bi bi-file-earmark-excel"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" Text="Buscar" ID="BuscarPlano" OnClick="BuscarPlano_Click" title="Buscar">
                                                    <i class="bi bi-search"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" Text="Cancelar" ID="Cancelar" OnClick="Cancelar_Click" title="Cancelar">
                                                 <i class="bi bi-x-square"></i>
                                        </asp:LinkButton>

                                    </div>



                                </ul>
                            </div>



                        </div>
                    </nav>

                    <div class="row text-center">
                        <span id="ErrorValidacion1" style="color: red;"></span>
                    </div>

                    <div class="row justify-content-center m-1 p-1 pb-3 mb-3" style="height: 25rem">
                        <div class="border rounded pb-2 mb-2">
                            <div class="row">
                                <div class="col-12">
                                    <div class="table-responsive mb-1" style="max-height: 23rem; overflow-x: auto;">
                                        <h5 class="datagrid-header text-center">Planos</h5>
                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridPlano1" runat="server" OnItemCommand="DataGridPlano1_ItemCommand">
                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                            <Columns>
                                                <asp:TemplateColumn HeaderText="...">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPlano1" runat="server" CommandName="VerPlano1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Plano" HeaderText="Plano" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Bolsa" HeaderText="Bolsa" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="AfectaBolsa" HeaderText="Afecta" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Id_OT" HeaderText="O.T" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="COnsecutivo_Pedido" HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Nombre_CLiente" HeaderText="Cliente" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Area" HeaderText="Area" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Contacto_Cliente" HeaderText="Contacto" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="AsesorComercial" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Fecha_Entrega_Bitacora" HeaderText="Fecha Diseño " ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="RealizadoPor" Visible="false" />
                                                <asp:BoundColumn DataField="Tipologia" Visible="false" />
                                            </Columns>
                                        </asp:DataGrid><asp:SqlDataSource runat="server" ID="CargarPlano" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT TOP 300 * FROM tblPlano WHERE Plano  LIKE '%' + @Plano + '%' AND Nombre_Cliente  LIKE '%' + @Cliente+ '%'ORDER BY Plano">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="tbPlano" PropertyName="Text" Name="Plano" DefaultValue="%"></asp:ControlParameter>
                                                <asp:ControlParameter ControlID="tbCliente" PropertyName="Text" DefaultValue="%" Name="Cliente"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row pb-2 mb-2">

                        <div class="col-1">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="lbPlano" Text="Plano" runat="server"></asp:Label>
                            </div>
                        </div>


                        <div class="col-3">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:TextBox ID="tbPlano" type="Text" class="form-control mayusculas " runat="server" AutoPostBack="true" OnTextChanged="tbPlano_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="lbPor" Text="Por" runat="server"></asp:Label>
                                <asp:TextBox ID="tbPor" type="Text" class="form-control mayusculas " runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="lbAsesor" Text="Asesor" runat="server"></asp:Label>
                                <asp:DropDownList class="form-control" ID="ddlAsesor" runat="server"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-2">
                            <div class=" input-group input-group-sm gap-2   ">
                                <asp:Label ID="lbFecha" class="form-label" Text="Fecha" runat="server"></asp:Label>
                                <asp:TextBox ID="tbFecha" type="date" class="form-control " runat="server"></asp:TextBox>
                            </div>
                        </div>


                    </div>

                    <div class="row s mb-2">
                        <div class="col-1">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="lbCliente" Text="Cliente" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="col-3">
                            <div class=" input-group input-group-sm gap-2  ">

                                <asp:TextBox ID="tbCliente" type="Text" class="form-control mayusculas " runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="lbArea" Text="Área" runat="server"></asp:Label>
                                <asp:TextBox ID="tbArea" type="Text" class="form-control mayusculas " runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="lbContacto" Text="Contacto" runat="server"></asp:Label>
                                <asp:TextBox ID="tbContacto" type="text" class="form-control mayusculas " runat="server"></asp:TextBox>
                            </div>
                        </div>


                    </div>

                    <div class="row pb-2 mb-2">
                        <div class="col-1">
                            <div class=" input-group-sm   ">
                                <asp:Label ID="lbTipologia" class="form-label pt-3" Text="Tipología" runat="server"></asp:Label>
                                <asp:CheckBox ID="chxTipologia" CssClass="pt-3" runat="server" />
                            </div>
                        </div>

                        <div class="col-3">
                            <div class=" input-group input-group-sm gap-2  ">
                                <asp:Label ID="lbHistorial" class="form-label " Text="Historial" runat="server"></asp:Label>
                                <asp:CheckBox ID="chxHistorial" runat="server" />
                                <asp:DropDownList class="form-control form-control-sm" ID="ddlHistorial" runat="server"></asp:DropDownList>
                            </div>
                        </div>


                        <div class="col-1">
                            <div class=" input-group-sm   ">
                                <asp:Label ID="lbAfecta" class="form-label pt-3" Text="Afecta" runat="server"></asp:Label>
                                <asp:CheckBox ID="chxAfecta" CssClass="pt-3" runat="server" />
                            </div>
                        </div>
                        <div class="col-2">
                            <div class=" input-group input-group-sm gap-2   ">
                                <asp:Label ID="lbBolsa" class="form-label" Text="Bolsa" runat="server"></asp:Label>
                                <asp:TextBox ID="tbBolsa" type="text" class="form-control " runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-5">
                            <div class=" input-group input-group-sm gap-2  justify-content-around  ">
                                <asp:Button ID="btnCargarPlano" CssClass="btn btn-outline-secondary" runat="server" Text="Cargar Plano" OnClick="btnCargarPlano_Click" />
                                <asp:Button ID="btnAsignar" CssClass="btn btn-outline-secondary" runat="server" Text="Asignar" />
                                <asp:Button ID="btnBuscar" CssClass="btn btn-outline-secondary" runat="server" Text="Buscar" OnClick="BuscarPlano_Boton" />

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

            // Actualiza el formulario 1
            window.opener.location.reload(); // Recarga el formulario padre

        }

        function validarFormulario() {
            var plano = document.getElementById("tbPlano").value;

            var isValid = true;

            if (plano === "") {
                ErrorValidacion1.innerHTML = "por favor ingrese el nombre del plano .";
                isValid = false;
            }

            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
        }



    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            console.log("Documento listo");
            $(".mayusculas").on("input", function () {
                console.log("Evento input activado");
                $(this).val($(this).val().toUpperCase());
                console.log("Nuevo valor: " + $(this).val());
            });
        });
    </script>








    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
