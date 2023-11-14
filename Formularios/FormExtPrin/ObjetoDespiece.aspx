<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObjetoDespiece.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.ObjetoDespiece" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/FormExtPrin/ObjetoDespiece.css" />
    <title>Objetos</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center ">
                <ul class="nav nav-tabs gap-5" id="miPestañas">


                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="InfObjetos-tab" data-bs-toggle="tab" href="#InfObjetos-Content">Información Objetos</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="DespiecePrecio-tab" data-bs-toggle="tab" href="#DespiecePrecio-Content">Despiece y Precio del Objeto</a>
                    </li>

                </ul>
            </div>
        </nav>

        <div class="tab-content">

            <div class="tab-pane fade show active" id="InfObjetos-Content">
                <asp:UpdatePanel ID="PanelInfObjetos" runat="server" UpdateMode="Conditional" DefaultButton="btnSubmit">
                    <ContentTemplate>

                        <div class="container-fluid m-3 p-3 ">

                            <div class="row pt-2 mt-2">
                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Objeto" runat="server" ID="lbObj"></asp:Label>
                                        <asp:TextBox ID="tbObj" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Divisiones" runat="server" ID="lbDiv"></asp:Label>
                                        <asp:TextBox ID="tbDiv" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Línea" runat="server" ID="lbLinea"></asp:Label>
                                        <asp:TextBox ID="tbLinea" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group-sm gap-1">
                                        <asp:CheckBox ID="chxEsc" runat="server" CssClass="form-check-input" />
                                        <asp:Label ID="lbEsc" runat="server" Text="Esc" CssClass="form-label"></asp:Label>
                                    </div>
                                </div>

                            </div>

                            <div class="row pt-1 mt-1">

                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Grupo" runat="server" ID="lbGrupo"></asp:Label>
                                        <asp:TextBox ID="tbGrupo" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Ancho" runat="server" ID="lbAncho"></asp:Label>
                                        <asp:TextBox ID="tbAncho" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Altura" runat="server" ID="lbAltura"></asp:Label>
                                        <asp:TextBox ID="tbAltura" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Profundidad" runat="server" ID="lbProfundidad"></asp:Label>
                                        <asp:TextBox ID="tbProfunididad" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Holgura" runat="server" ID="lbHolgura"></asp:Label>
                                        <asp:TextBox ID="tbHolgura" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row pt-2 mt-2">

                                <div class="col-sm-7">
                                    <div class=" mb-2 gap-1">
                                        <asp:Label class="form-label" Text="Descripción Sistema Integral Ducon" runat="server" ID="lbDesSid"></asp:Label>
                                        <asp:TextBox ID="tbDesSid" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <h1></h1>
                                </div>

                                <div class="col-sm-3">
                                    <div class="mb-2 gap-1">
                                        <asp:Label class="form-label" Text="Valor" runat="server" ID="lbValor"></asp:Label>
                                        <asp:TextBox ID="tbValor" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                        </div>

                        <div class="container-fluid">
                            <div class="row justify-content-center pt-3 mt-3">

                                <div class="border rounded m-2 p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-1" style="max-height: 20rem; overflow-x: auto;">
                                                <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridObjetos" runat="server">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>
                                                        <asp:BoundColumn DataField="Id_Modulo" HeaderText="Módulo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Familia" HeaderText="Familia" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_TipoModulo" HeaderText="Tipo Módulo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Ubicacion_Modulo" HeaderText="Ubicación" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Ancho" HeaderText="Ancho" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Lado" HeaderText="Lado" ItemStyle-CssClass="auto-width-column" />

                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>

                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-1" style="max-height: 20rem; overflow-x: auto;">
                                                <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridDespieceModulo" runat="server">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>

                                                        <asp:BoundColumn DataField="Id_Insumo" HeaderText="Comp" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Pieza" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Abreviado" HeaderText="Und" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion" HeaderText="T. Insumo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Valor_Unitario" HeaderText="Vlr. Und" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Acabado" HeaderText="Acabado" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Factor_Ganancia" HeaderText="F. Gan" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Factor_Desperdicio" HeaderText="F. Desp" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DescuentoAncho" HeaderText="D. Ancho" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DescuentoAltura" HeaderText="D. Alto" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ID_Inventario" HeaderText="Cod. Inv" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion_Areas_Concatenadas" HeaderText="Destino" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Sentido" HeaderText="Sentido" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Costear" HeaderText="Costear" ItemStyle-CssClass="auto-width-column" />




                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>

                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade " id="DespiecePrecio-Content">
                <asp:UpdatePanel ID="PanelDespiecePrecio" runat="server" UpdateMode="Conditional" DefaultButton="btnSubmit">
                    <ContentTemplate>
                        <div class=" container-fluid">

                            <div class="row justify-content-center pt-2 mt-2 pb-4 mb-4">
                                <div class="border rounded  m-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-1" style="max-height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Despiece y Precios </h5>
                                                <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridDespieceAsesor" runat="server" OnItemDataBound="DataGridDespieceAsesor_ItemDataBound">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>
                                                        <asp:BoundColumn DataField="Item" HeaderText="Item" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ID_Inventario" HeaderText="Cod. PSL" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Pieza" HeaderText="Insumo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="LongitudInsumo" HeaderText="A/P" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="AlturaInsumo" HeaderText="L/H" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Factor_Desperdicio" HeaderText="Desp" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ValorUndVenta" HeaderText="V. Unit" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Abreviado" HeaderText="Und" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="SubTotal" HeaderText="Sub Total" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Valor_Costo" HeaderText="Costo" ItemStyle-CssClass="auto-width-column" />


                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row ">

                                <div class="col-3 mt-auto text-center ">
                                    <asp:Button ID="btnGenerarDespiece" runat="server" Text="Despiece" CssClass="btn  btn-lg btn-outline-secondary" />
                                </div>

                                <div class="col-3 justify-content-lg-start mb-auto">
                                    <div class=" mb-2 gap-1">
                                        <asp:Label class="form-label" Text="Valor Venta" runat="server" ID="lbValorVenta"></asp:Label>
                                        <asp:TextBox ID="tbValorVenta" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-1 ">
                                    
                                </div>
                                <div class="col-3 justify-content-lg-start mb-auto">
                                    <div class=" mb-2 gap-1">
                                        <asp:Label class="form-label" Text="Valor Costo" runat="server" ID="lbValorCosto"></asp:Label>
                                        <asp:TextBox ID="tbValorCosto" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>

    </form>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>



</body>
</html>
