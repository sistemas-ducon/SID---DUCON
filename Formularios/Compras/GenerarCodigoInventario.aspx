<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerarCodigoInventario.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Compras.GenerarCodigoInventario" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/OrdenTrabajo.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <!-- se utiliza para administrar los scripts y servicios que necesitan los controles AJAX en la página.-->

        <!--- aqui define un contenedor para el contenido de la pestaña -->
        <div class="tab-content">
            <!--esta es una pequeña especifica, que por defecto se  muestra activa por defecto--->
            <div class="tab-pane fade show active" id="Grupo">
                <!--aqui  se utiliza un UpdatePanel de ASP.NET para realizar actualizaciones para el contenido---->
                <asp:UpdatePanel ID="Codi_Grup" UpdateMode="Conditional" runat="server">

                    <ContentTemplate>
                        <div class="container border   border-solid  p-3  shadow   mt-5">
                            <!-- Primera fila -->
                            <div class="container border   border-solid  p-3  shadow mt-3 ">
                                <div class="row pt-2 mt-4 ">

                                    <div class="col-2">
                                        <asp:Label class="form-label" Text="Seleccione un Grupo: " runat="server" ID="Selec_Grup"></asp:Label>
                                    </div>

                                    <div class="col-2">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-select">
                                                <asp:ListItem Text="Opción 1" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Opción 2" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Opción 3" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-1"></div>

                                    <div class="col-3">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label CssClass="form-label" Text="codigo grupo" runat="server" ID="C_grupo"></asp:Label>
                                            <div class="col-1"></div>
                                            <asp:TextBox ID="Grupo1" type="text" runat="server" class="form-control"></asp:TextBox>
                                        </div>

                                    </div>

                                    <div class="col-2">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label CssClass="form-label" Text="Grupo" runat="server" ID="Grupo_a"></asp:Label>
                                            <div class="col-1"></div>
                                            <asp:TextBox ID="TextBox5" type="text" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <!-- Segunda fila -->
                            <div class="container border   border-solid  p-3  shadow mt-3 ">
                                <div class="row pt-2 mt-3 container ">

                                    <div class="col-2">
                                        <asp:Label CssClass="form-label" Text="Nuevo Nombre insumo:" runat="server" ID="Label1"></asp:Label>
                                    </div>


                                    <div class="col-2">
                                        <div class="input-group input-group-sm mb-2">
                                            <asp:TextBox ID="TextBox1" type="text" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-1"></div>

                                    <div class="col-2 ">
                                        <asp:Label CssClass="form-label" Text="Nombre insumo:" runat="server" ID="Label2"></asp:Label>
                                    </div>

                                    <div class="col-5">
                                        <div class="input-group input-group-sm mb-2">
                                            <asp:TextBox ID="TextBox2" type="text" runat="server" class="form-control"></asp:TextBox>

                                        </div>

                                    </div>
                                </div>
                            </div>

                            <!-- Tercera fila -->
                            <div class="container border border-solid p-3 shadow mt-3">
                                <div class="row  pt-1 mt-3 container">
                                    <!-- Sección de la DropDownList y el Botón -->
                                    <div class="col-6">
                                        <div class="form-group">
                                            <asp:Label ID="Label3" runat="server" Text="Unidad de medida"></asp:Label>
                                            <div class="row">
                                                <div class="col-12 ">
                                                    <div class="input-group  gap-2 ">
                                                        <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-control  form-control-sm ">
                                                            <asp:ListItem Text="Opción 1" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Opción 2" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Opción 3" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <div class="input-group-append">
                                                            <asp:Button CssClass="btn btn-outline-secondary" ID="Button2" runat="server" Text="Crear Codigo(s) para nuevo(s) insumo(s)" />
                                                        </div>
                                                    </div>

                                                    <asp:Label CssClass="form-label" Text="Seleccione los insumos a crear" runat="server" ID="Label4"></asp:Label>
                                                    <asp:TextBox ID="TextArea1" TextMode="MultiLine" Rows="4" Columns="10" runat="server" class="form-control"></asp:TextBox>

                                                    <div class="text-center mt-3">
                                                        <!-- Aquí añadimos la clase text-center para centrar el botón -->
                                                        <asp:Button CssClass="btn btn-outline-secondary btn-light" ID="Button1" runat="server" Text="Crear Insumos Seleccionados" />
                                                    </div>

                                                </div>
                                            </div>

                                        </div>


                                    </div>


                                    <!-- Sección del  datagrid -->
                                    <div class="col-6 ">
                                        <div class="row justify-content-center">
                                            <div class="border rounded p-2">
                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="table-responsive  mb-2 gap-2" style="max-height: 20rem; height: 20rem; overflow-x: auto;">
                                                            <h6 class="datagrid-header text-center">Modulo</h6>
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataModulo" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="...">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="color" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="" HeaderText="Codigo" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="" HeaderText="Descripcion" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="" HeaderText="Un. De Medida" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="" HeaderText="" ItemStyle-CssClass="auto-width-column" />






                                                                </Columns>
                                                            </asp:DataGrid>

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
            </div>
        </div>

    </form>

</body>
</html>

