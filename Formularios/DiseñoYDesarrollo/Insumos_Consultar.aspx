<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Insumos_Consultar.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Insumos_Consultar" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/Insumo_Consultar.css" rel="stylesheet" />

    <title>SID</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <span class="navbar-brand mb-0 h1">Consultar - Insumo</span>
            </div>
        </nav>

       <nav class="navbar navbar-light bg-light">
    <div class="container d-flex justify-content-center">
        <ul class="nav nav-tabs">
            <li class="nav-item">
                <a class="nav-link text-dark active" id="insumo-tab" data-bs-toggle="tab" href="#insumo-content">Insumo</a>
            </li>
            <li class="nav-item">
                <a class="nav-link text-dark" id="tipo-insumo-tab" data-bs-toggle="tab" href="#tipo-insumo-content">Tipo Insumo y Grupo Acabados</a>
            </li>
        </ul>
    </div>
</nav>


        <div class="tab-content">
            <div class="tab-pane fade show active" id="insumo-content">
                <div class="container">
                    <h6>Informacion General</h6>

                    <div class="row">
                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" Id="lblInsumos" class="mt-auto">Insumo</label>
                                <asp:TextBox runat="server" ID="txtInsumos" class="form-control" />
                            </div>
                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Creacion</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox1" class="form-control" />
                            </div>
                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Act</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox2" class="form-control" />
                            </div>
                        </div>

                        <div class="col-4">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Usuario</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox3" class="form-control" />
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Descripcion</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox4" class="form-control" />
                            </div>
                        </div>

                        <div class="col-4">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Tipo</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox5" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Und</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox6" class="form-control" />
                            </div>
                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Valor</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox7" class="form-control" />
                            </div>
                        </div>

                        <div class="col-4">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Acabado desde</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox8" class="form-control" />
                            </div>
                        </div>


                    </div>

                    <div class="row">
                        <div class="col-2 me-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Cod.Inv</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox9" class="form-control" />
                            </div>
                        </div>

                        <div class="col-2 me-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">F.Ganancia</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox10" class="form-control" />
                            </div>
                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">D.Desperdicio</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox11" class="form-control" />
                            </div>
                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Peso(Kg)</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox12" class="form-control" />
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label runat="server" for="txtInsumos" class="mt-auto">Und x Paq</label>
                                <asp:TextBox runat="server" type="text" ID="TextBox13" class="form-control" />
                            </div>
                        </div>


                    </div>


                    <div class="row">
                        <div class="col-7">
                        </div>
                        <div class="col-3 d-flex justify-content-end gap-2">

                            <button class="btn btn-dark">Grabar</button>
                            <button class="btn btn-dark">Cancelar</button>
                            <button class="btn btn-dark">Cerrar</button>
                        </div>
                     </div>

                    <%--DataGrid Pantalla media--%>

                    <h6>Usado en</h6>

                    <div class="table table-responsive custom-grid">

                        <asp:DataGrid Class="table table-responsive custom-grid" ID="DataGrid1" runat="server" DataSourceID="Datagrid" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundColumn DataField="Id_Modulo" HeaderText="Modulo" />
                                  <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripcion" />
                                  <asp:BoundColumn DataField="" HeaderText="Cantidad" />
                                  <asp:BoundColumn DataField="Descripcion_TipoModulo" HeaderText="Tipo Modulo" />
                                  <asp:BoundColumn DataField="Altura" HeaderText="Altura" />
                            </Columns>

                        </asp:DataGrid>

                        <asp:SqlDataSource runat="server" ID="Datagrid" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="cta_Modulos_del_Insumo" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtInsumos" PropertyName="Text" Name="Insumo" Type="String"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>



                    <h6>Historico actualizacion de insumo</h6>

                    <asp:DataGrid ID="DataGrid2" runat="server"></asp:DataGrid>


                </div>


            </div>






            <%--Termina pantalla de Insumo--%>

            <%--DATAGRID #1--%>

            <div class="tab-pane fade container" id="tipo-insumo-content">
                <div class="d-flex justify-content-start mb-3 gap-3">
                    <div class="table-responsive" style="max-height: 200px; max-width: 800px; overflow-x: auto;">
                        <asp:DataGrid Class="table table-bordered table-hover" ID="DataGrid3" runat="server" DataSourceID="DataGridTipoInsumo" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateColumn HeaderText="ID">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CssClass="text-decoration-none text-dark" Text='<%# Eval("Id_TipoInsumo") %>' OnClientClick='<%# "MostrarDatos(\"" + Eval("Descripcion") + "\", \"" + Eval("DesGrupoAcabado") + "\")" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="Descripcion" HeaderText="Descripcion" />
                                <asp:BoundColumn DataField="DesGrupoAcabado" HeaderText="Grupo Acabado" />
                            </Columns>
                        </asp:DataGrid>

                        <script>
                            function MostrarDatos(descripcion, desGrupoAcabado) {
                                document.getElementById('<%= TextOT.ClientID %>').value = descripcion;
                                document.getElementById('<%= TextPedido.ClientID %>').value = desGrupoAcabado;
                            }
                        </script>


                    </div>
                    <div class="btn-group-vertical mb-2 gap-2">
                        <button type="button" id="Button1" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                            <i class="bi bi-plus"></i>
                        </button>

                        <button type="button" id="Button4" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                            <i class="bi bi-wrench-adjustable"></i>
                        </button>

                        <button type="button" id="Button2" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                            <i class="bi bi-file-text"></i>
                        </button>
                    </div>

                </div>

                <div class="row">
                    <div class="col-2">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:TextBox ID="TextOT" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-2">
                        <div class="input-group input-group-sm mb-5 gap-2">

                            <asp:TextBox ID="TextPedido" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <asp:SqlDataSource runat="server" ID="DataGridTipoInsumo" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT [Id_TipoInsumo], [Descripcion], [DesGrupoAcabado] FROM [tblTipoInsumo]"></asp:SqlDataSource>


                        <%-- DATAGRID #2 --%>

                 <div class="row">
                    <div class="col-md-6">

                      <%--  BOTONES --%>

                        <div class="btn-group mb-2 gap-2">
                            <button type="button" id="Button3" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                                <i class="bi bi-plus"></i>
                            </button>

                            <button type="button" id="Button5" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                                <i class="bi bi-wrench-adjustable"></i>
                            </button>

                            <button type="button" id="Button6" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                                <i class="bi bi-file-text"></i>
                            </button>
                        </div>

                     <%--   DATAGRID--%>

                        <div class="table-responsive" style="max-height: 200px; max-width: 800px; overflow-x: auto;">

                            <asp:DataGrid Class="table table-bordered table-hover" ID="DataGrid4" runat="server" DataSourceID="DatagridGrupoDeAcabado" AutoGenerateColumns="false">
                                <Columns>

                                    <asp:TemplateColumn HeaderText="ID">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" CssClass="text-decoration-none text-dark" ID="linkID_GrupoAcabado" Text='<%# Eval("ID_GrupoAcabado") %>' OnClientClick='<%# "MostrarDatos2(\"" + Eval("ID_GrupoAcabado") + "\", \"" + Eval("Descripcion_Grupo") + "\")" %>' ClientIDMode="Static"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>

                                    <asp:BoundColumn DataField="Descripcion_Grupo" HeaderText="Descripcion" />

                                </Columns>
                            </asp:DataGrid>

                            <script>
                                function MostrarDatos2(ID_GrupoAcabado, Descripcion_Grupo) {
                                    document.getElementById('<%= TextBox14.ClientID %>').value = Descripcion_Grupo;
                                    // Redireccionar al DATAGRID #3 y enviar el ID_GrupoAcabado
                                    window.location.href = 'DataGrid5' + ID_GrupoAcabado;
                                }
                            </script>


                        </div>
                        <asp:SqlDataSource runat="server" ID="DatagridGrupoDeAcabado" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT [ID_GrupoAcabado], [Descripcion_Grupo] FROM [tblGrupodeAcabado]"></asp:SqlDataSource>

                       <%-- TEXTBOX--%>

                        <div class="row">
                            <div class="col-5">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:TextBox ID="TextBox14" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-- DATAGRID #3 --%>

                    <div class="col-md-6">

                        <div class="btn-group mb-2 gap-2">
                        <button type="button" id="Button7" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                            <i class="bi bi-plus"></i>
                        </button>

                        <button type="button" id="Button8" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                            <i class="bi bi-wrench-adjustable"></i>
                        </button>

                        <button type="button" id="Button9" runat="server" class="btn btn-light btn-sm" style="font-size: 20px; margin: 5px">
                            <i class="bi bi-file-text"></i>
                        </button>
                    </div>

            <div class="table-responsive" style="max-height: 200px; max-width: 800px; overflow-x: auto;">
                <asp:DataGrid Class="table table-bordered table-hover" ID="DataGrid5" runat="server" DataSourceID="DatagridAcabados" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundColumn DataField="CodInventario" HeaderText="Cod.Inv" />
                        <asp:BoundColumn DataField="Descripcion_Acabado" HeaderText="Descripcion" />
                        <asp:BoundColumn DataField="DeLinea" HeaderText="L" />
                        <asp:BoundColumn DataField="Activo" HeaderText="A" />
                    </Columns>
                </asp:DataGrid>
            </div>
                        <asp:SqlDataSource runat="server" ID="DatagridAcabados" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT [CodInventario], [Descripcion_Acabado], [DeLinea], [Activo] FROM [tblAcabado]"></asp:SqlDataSource>

                        <div class="row">
                            <div class="col-5">
                                <div class="input-group input-group-sm mb-2 gap-2">

                                    <asp:TextBox ID="TextBox15" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-5">
                                <div class="input-group input-group-sm mb-5 gap-2">

                                    <asp:TextBox ID="TextBox16" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-1">
                                <asp:CheckBox runat="server" ID="CheckBoxDeLinea" />
                                <asp:CheckBox runat="server" ID="CheckBoxActivo" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>



    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
