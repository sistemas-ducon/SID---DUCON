<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdenTrabajo.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.OrdenTrabajo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>


    <link rel="stylesheet" href="../../Recursos/CSS/OrdenTrabajo.css" />
    <title>Ordenes de Trabajo</title>

</head>



<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center ">
                <ul class="nav nav-tabs gap-5" id="miPestañas">


                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="OTs-tab" data-bs-toggle="tab" href="#OTs-Content">Ordenes Trabajo</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Plano-tab" data-bs-toggle="tab" href="#Plano-Content">Plano</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark " id="Objeto-tab" data-bs-toggle="tab" href="#Objeto-Content">Objetos</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Modulo-tab" data-bs-toggle="tab" href="#Modulo-Content">Modulos</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark " id="Insumo-tab" data-bs-toggle="tab" href="#Insumo-Content">Insumos</a>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="tab-content">

            <div class="tab-pane fade show active" id="OTs-Content">
                <asp:UpdatePanel ID="PanelOt" runat="server" UpdateMode="Conditional" DefaultButton="btnSubmit">
                    <ContentTemplate>

                        <div class="container-fluid">

                            <div class="modal fade" id="myModal" tabindex="-1">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title">Orden de Trabajo</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <p class="text-center">
                                                Cerrada el día:
                                                <asp:Literal ID="LiteralFechaCierre" runat="server"></asp:Literal>
                                            </p>
                                        </div>
                                        <div class="modal-footer">
                                            <!-- Puedes agregar botones adicionales o contenido en el footer si es necesario -->
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Nav iconos OTs-->
                            <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
                                <div class="container-fluid">

                                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                        <span class="navbar-toggler-icon"></span>
                                    </button>
                                    <div class="collapse navbar-collapse" id="ejemplo2">
                                        <ul class="navbar-nav mx-auto contenedor-icono">

                                            <div class="contenedor-icono">


                                                <a class="icong disabled" href="#" title="Nueva OT" id="NuevaOt" onclick="NuevaOt()">
                                                    <i class="bi bi-file-earmark"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Copiar Información en una Nueva OT" id="CopiarOt">
                                                    <i class="bi bi-files"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Grabar Orden de Trabajo" id="GrabarOt">
                                                    <i class="bi bi-save2"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Modificar Orden de Trabajo" id="ModificarOt">
                                                    <i class="bi bi-wrench"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Anular o Eliminar un Pedido" id="AnularPedido">
                                                    <i class="bi bi-file-earmark-excel"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Documentación OT" id="DocumentacionOt">
                                                    <i class="bi bi-paperclip"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Observaciones OT" id="ObservacionesOt">
                                                    <i class="bi bi-eye"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Imprimir Informacion General de la OT" id="imprimirOt">
                                                    <i class="bi bi-printer"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Reimprimir Información Contable" id="ReimprimirOt">
                                                    <i class="bi bi-printer-fill"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Consultar Bolsa" id="ConsultarBolsa">
                                                    <i class="bi bi-coin"></i>
                                                </a>

                                                <a class="icong disabled Cancelar" href="#" title="Cancelar" id="Cancelar" onclick="Cancelar()">
                                                    <i class="bi bi-x-lg"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Visualizar OT Pendientes" id="OtPendientes">
                                                    <i class="bi bi-eyeglasses"></i>
                                                </a>
                                                <a class="icong disabled Actualizar" href="#" title="Actualizar Pedidos Importados" id="ActPedImp">
                                                    <i class="bi bi-check-square"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Importar Pedido Asesor" id="ImpPedAse">
                                                    <i class="bi bi-person-lines-fill"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Importar Pedido Sede " id="ImpPedSed">
                                                    <i class="bi bi-house-up"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Habilitar Pedido para Ventas" id="HabilitarPedido">
                                                    <i class="bi bi-receipt-cutoff"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Deshabilitar Orden de Trabajo para Producción " id="DeshabilitarOt">
                                                    <i class="bi bi-sign-stop"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Indicador Obra Reactivada " id="ObraReactivada">
                                                    <i class="bi bi-bar-chart-line"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Registrar Pedido en el Sistema Administrativo " id="RegPedSisAdm">
                                                    <i class="bi bi-triangle"></i>
                                                </a>
                                                <a class="icong disabled Cerrar" href="#" title="Cierra o Abre una OT " id="CierraOt">
                                                    <i class="bi bi-key"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Simular Pasar Pedido " id="SimularPedido">
                                                    <i class="bi bi-code-square"></i>
                                                </a>

                                                <a class="icong disabled " href="#" title="Exportar Pedido " id="ExportarPedido">
                                                    <i class="bi bi-airplane-engines"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Entrega Perfecta " id="EntregaPerfecta">
                                                    <i class="bi bi-lightning-charge"></i>
                                                </a>


                                                <a class="icong disabled" href="#" title="Anular Obra" id="AnularObra">
                                                    <i class="bi bi-x-square"></i>
                                                </a>



                                                <ul />
                                        </ul>
                                    </div>

                                </div>
                            </nav>

                            <div class="row">

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="OT" runat="server" ID="lblOT"></asp:Label>
                                        <asp:TextBox ID="tbOT" runat="server" CssClass="form-control" OnTextChanged="ObtenerInfoOt" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Pedido" runat="server" ID="lblPedido"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlNumbers" runat="server" OnSelectedIndexChanged="CambioDePediido" AutoPostBack="true"></asp:DropDownList>

                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Zona" runat="server" ID="lbZona"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlZona" runat="server">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="01">01</asp:ListItem>
                                            <asp:ListItem Value="02">02</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="T.Ped" runat="server" ID="lblTped"></asp:Label>
                                        <asp:DropDownList ID="dtacboTipoPedido" runat="server" class="form-control" AutoPostBack="True" DataSourceID="TiposDePedidos" DataTextField="Descripcion_TipoPedido" DataValueField="Descripcion_TipoPedido"></asp:DropDownList>
                                        <asp:SqlDataSource ID="TiposDePedidos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select Descripcion_TipoPedido from tblTipoPedido order by Descripcion_TipoPedido"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Ped.Base" runat="server" ID="lblPedBase"></asp:Label>
                                        <asp:DropDownList ID="cboPedidoBase" runat="server" class="form-control" DataSourceID="PedidoBase" DataTextField="PedidoBase" DataValueField="PedidoBase"></asp:DropDownList>
                                        <asp:SqlDataSource ID="PedidoBase" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT DISTINCT PedidoBase FROM tblOT WHERE (PedidoBase BETWEEN 1 AND 1000) ORDER BY PedidoBase"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Ped.Deped" runat="server" ID="lblPedDepen"></asp:Label>
                                        <asp:TextBox ID="tbPedDepen" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Aprob" runat="server" ID="lblAprob"></asp:Label>
                                        <asp:TextBox ID="tbAprob" type="text" class="form-control" runat="server" Visible="false"></asp:TextBox>
                                        <asp:DropDownList ID="DtaCboTipoAprobacion" runat="server" class="form-control" DataSourceID="Aprob" DataTextField="TipoAprobacion" DataValueField="IdTipoAprobacion"></asp:DropDownList>
                                        <asp:SqlDataSource ID="Aprob" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select * from tblTipoAprobacion order by TipoAprobacion"></asp:SqlDataSource>

                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2 justify-content-around">
                                        <asp:Button ID="btnNuevoPedido" runat="server" Text="Btn1" class="btn btn-secondary" OnClick="NuevoPedido"></asp:Button>
                                        <asp:Button ID="btnAcabados" runat="server" Text="Btn2" class="btn btn-secondary" OnClick="Acabados"></asp:Button>
                                        <asp:Button ID="btnOk" runat="server" type="button" Text="OK" class="btn btn-secondary" OnClick="Boton_Ok"></asp:Button>
                                    </div>
                                </div>

                            </div>

                            <div class="row">

                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Obra" runat="server" ID="lblObra"></asp:Label>
                                        <asp:TextBox ID="tbObra" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-4">
                                        <asp:Label class="form-label" Text="Dir" runat="server" ID="lblDir"></asp:Label>
                                        <asp:TextBox ID="tbDir" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Contac" runat="server" ID="lblContac"></asp:Label>
                                        <asp:TextBox ID="tbContac" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Email" runat="server" ID="lblEmail"></asp:Label>
                                        <asp:TextBox ID="tbEmail" type="email" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>





                            </div>

                            <div class="row">


                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Recibe" runat="server" ID="lblRecibe"></asp:Label>
                                        <asp:TextBox ID="tbRecibe" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Ciudad" runat="server" ID="lblCiudad"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" DataTextField="NombreCiudad" DataValueField="NombreCiudad" DataSourceID="CargarCiudad" OnDataBound="ddlCiudad_DataBound"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="CargarCiudad" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT 
                                            CONCAT(tblDepartamentoPais.CodigoDepartamento ,
                                            tblCiudad.CodigoCiudad)   AS CodCompleto,
                                            tblCiudad.NombreCiudad+' - '+tblDepartamentoPais.NombreDepartamento As NombreCiudad
                                            FROM tblDepartamentoPais 
                                            INNER JOIN tblCiudad
                                            ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento 
                                            ORDER BY CONCAT(tblCiudad.NombreCiudad , '-' , tblDepartamentoPais.NombreDepartamento)"></asp:SqlDataSource>

                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-4">
                                        <asp:Label class="form-label" Text="Tel" runat="server" ID="lblTel"></asp:Label>
                                        <asp:TextBox ID="tbTel" type="tel" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Cel" runat="server" ID="lblCel"></asp:Label>
                                        <asp:TextBox ID="tbCel" type="tel" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="col-2">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Pais" runat="server" ID="lblPais"></asp:Label>
                                        <asp:TextBox ID="tbPais" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="H.Total" runat="server" ID="lblHTotal"></asp:Label>
                                        <asp:TextBox ID="tbHTotal" type="number" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="container-fluid pt-2 Observacion ">

                            <div class="Obs1">
                                <div class="row">
                                    <div class="col-12">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <textarea class="form-control form-control-sm" id="txObs1" runat="server" cols="20" rows="8" disabled="disabled"></textarea>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="Der">

                                <div class="Arriba">

                                    <div class="row">
                                        <div class="col-6">

                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <asp:Label class="form-label" Text="Venta" runat="server" ID="lblVenta"></asp:Label>
                                                <asp:TextBox ID="tbVenta" type="date" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <label class="form-label" runat="server" id="inputOkVenta">Ok.Venta</label>
                                                <asp:TextBox ID="dtpFechaEntregaDibujoDespiece" type="date" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>


                                    </div>

                                    <div class="row">
                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <label class="form-label" runat="server" id="inputDibujo">Ok.Dibujo</label>
                                                <asp:TextBox ID="dtpFechaEntregaProduccion" type="date" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-6">

                                            <asp:Label ID="LabelOTCerrada" ClientIDMode="Static" runat="server" Text="OT cerrada" BackColor="#DD0000" ForeColor="white" Font-Size="X-Large" Width="350px" Visible="false" CssClass="rounded-label"></asp:Label>

                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <label class="form-label" runat="server" id="inputEmpaque">Empaque</label>
                                                <asp:TextBox ID="dtpEmpaque" type="date" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <label class="form-label" runat="server" id="inputRealEmp">Real Emp.</label>
                                                <asp:TextBox ID="dtpRealEmpaque" type="date" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="Abajo">
                                    <div class="row justify-content-center">
                                        <div class="border rounded p-0" style="margin-right: 2rem">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="table-responsive mb-1 " style="max-height: 10rem; overflow-x: auto;">
                                                        <h5 class="datagrid-header text-center">Despacho</h5>
                                                        <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridDespacho" runat="server">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                            <Columns>
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

                        <div class="container-fluid pt-2 Observacion ">

                            <div class="Obs1">
                                <div class="row">
                                    <div class="col-12">
                                        <div class=" input-group-sm  mb-2 gap-2">

                                            <textarea class="form-control form-control-sm" id="txObs2" runat="server" cols="20" rows="8"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="Der">

                                <div class="row">

                                    <div class="col-5">

                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label class="form-label" Text="Supervisor" runat="server" ID="lblSupervisor"></asp:Label>
                                            <asp:TextBox ID="tbSupervisor" type="text" class="form-control" runat="server" />
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <!-- Esta Coluna se Puede ultilzar-->

                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-5">
                                        <div class="input-group input-group-sm mb-2 gap-4">
                                            <asp:Button class="btn btn-outline-secondary" Text="Plano+" runat="server" type="button" disabled="disabled"></asp:Button>
                                            <asp:Label type="text" class="form-label fw-bold" runat="server" ID="lbPlano" Text="Plano" />
                                        </div>
                                    </div>

                                    <div class="col-7">
                                        <div class="input-group   mb-2 gap-2">
                                            <asp:Label class="form-label text-end" Text="Bolsa" runat="server" ID="lblBolsa"></asp:Label>
                                            <asp:TextBox type="text" class="form-control text-end" runat="server" ID="tbBolsa" />
                                        </div>

                                    </div>
                                </div>

                                <div class="row">

                                    <div class="col-4">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label class="form-label" Text="Fabrica" runat="server" ID="lblFabrica"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlFabrica1" runat="server">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="Bogota">Bogota</asp:ListItem>
                                                <asp:ListItem Value="Medellin">Medellin</asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label class="form-label" runat="server" ID="lblSaldoOT" BorderColor="#006600" BackColor="Lime" Font-Size="X-Large" Text="Saldo" Width="10em" Height="1.5em" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-4">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label class="form-label" Text="Instala" runat="server" ID="Instala"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlInstala" runat="server">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="Bogota">Bogota</asp:ListItem>
                                                <asp:ListItem Value="Medellin">Medellin</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-2">
                                        <div class="row">
                                            <div class="col-12 ">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label class="form-label" Text="V. Pedido" runat="server" ID="Label1"></asp:Label>
                                                    <asp:Button class="btn btn-outline-secondary" Text="TXT" runat="server" type="button" disabled="disabled"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="input-group  mb-1 gap-2">
                                            <asp:TextBox ID="tbValorPedido" CssClass="form-control text-end" runat="server"></asp:TextBox>
                                        </div>
                                    </div>


                                </div>

                            </div>
                        </div>

                        <h5 class="p-0 m-0 mb-1 text-center">Informacion Contable </h5>

                        <div class=" Info-Contable">


                            <div class="Datos-Cliente1">

                                <div class="row">

                                    <div class="col-3">

                                        <div class=" input-group input-group-sm">
                                            <asp:Button ID="Nit" runat="server" Text="Nit  ..." class="bi bf btn btn-secondary" />

                                        </div>
                                    </div>

                                    <div class="col-9">
                                        <div class=" input-group input-group-sm gap-1">
                                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtNit"></asp:TextBox>
                                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtNombreEmp"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-3">

                                        <div class=" input-group input-group-sm">
                                            <asp:Label class="form-label" Text="Contacto" runat="server" ID="lblContacto"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-9">
                                        <div class=" input-group input-group-sm ">
                                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtcontacto"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-3">

                                        <div class=" input-group input-group-sm">
                                            <asp:Label class="form-label" Text="Mail" runat="server" ID="lblMail"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-9">
                                        <div class=" input-group input-group-sm ">
                                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtMail"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-3">

                                        <div class=" input-group input-group-sm">
                                            <asp:Label class="form-label" Text="Dirección" runat="server" ID="lblDireccion"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-9">
                                        <div class=" input-group input-group-sm ">
                                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtDireccion"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-3">

                                        <div class=" input-group input-group-sm">
                                            <asp:Label class="form-label" Text="Municipio" runat="server" ID="lblMunicipio"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-9">
                                        <div class=" input-group input-group-sm ">
                                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtMunicipio"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-3">

                                        <div class=" input-group input-group-sm">
                                            <asp:Label class="form-label" Text="Telefono" runat="server" ID="lblTelefono"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-9">
                                        <div class=" input-group input-group-sm ">
                                            <asp:TextBox type="text" class="form-control" runat="server" ID="txtTelefono"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-3">

                                        <div class=" input-group input-group-sm">
                                            <asp:Label class="form-label" Text="Obs. Contable " runat="server" ID="lblObs"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-9">
                                        <div class=" input-group input-group-sm ">
                                            <textarea class="form-control form-control-sm" id="ObservacionCont" runat="server" cols="20" rows="3" high="60px"></textarea>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-12">

                                        <div class=" input-group input-group-sm">
                                            <asp:TextBox type="" class="form-control" runat="server" ID="txtMensaje" Visible="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="Datos-Cliente2">

                                <div class="superior">

                                    <div class="Info2">
                                        <asp:Button ID="btnCotizacion" runat="server" Text="Ver cotización" class="bi bf btn btn-secondary" />
                                        <asp:TextBox type="text" class="form-control" runat="server" ID="txtCotizacion"></asp:TextBox>
                                    </div>

                                    <div class="Info2">
                                        <asp:Button ID="btnValorSugeroido" runat="server" Text="Valor Sugerido" class="bi bf" disabled="true" />
                                        <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtValorSugerido"></asp:TextBox>
                                    </div>

                                    <div class="Info2">
                                        <asp:Button ID="btnVscd" runat="server" Text="VCSD" class="bi bf" disabled="true" />
                                        <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtVcsd"></asp:TextBox>
                                    </div>

                                    <div class="Info2">
                                        <asp:Button ID="btnVccd" runat="server" Text="VCCD" class="bi bf" disabled="true" />
                                        <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtVccd"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="Medio">

                                    <div class="Info_M">
                                        <asp:Button ID="btnOrdenCompra" runat="server" Text="Orden Compra" class="bi bf" disabled="true" />
                                        <asp:TextBox type="text" class="form-control" runat="server" ID="txtOrdenCompra"></asp:TextBox>
                                    </div>

                                    <div class="Info_M2">
                                        <asp:Label ID="lblComisionCompart" class="form-label" Text="Comisión Compartida" runat="server"></asp:Label>
                                        <asp:CheckBox class="" ID="cbxComisionCompart" runat="server" />
                                    </div>
                                </div>

                                <div class="Medio">

                                    <div class="Info_M">
                                        <asp:Button ID="btnAsesor1" runat="server" Text="Asesor" class="bi bf" disabled="true" />
                                        <asp:TextBox type="text" class="form-control" runat="server" ID="txtAsesor"></asp:TextBox>
                                    </div>

                                    <div class="Info_M">
                                        <asp:DropDownList ID="ddlAsesor" class=" form-control form-control-lg" Style="width: 18rem" runat="server"></asp:DropDownList>
                                    </div>


                                </div>

                                <div class="Div_Grid">

                                    <div class="izquierda">
                                        <label>
                                            Venta
                                    <br />
                                            Neta</label>
                                    </div>

                                    <div class=" container-fluid ">
                                        <div class="row justify-content-center m-1 p-1">
                                            <div class="border rounded p-2 m-2">
                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="table-responsive mb-1" style="max-height: 11rem; overflow-x: auto;">
                                                            <h5 class="datagrid-header text-center">Contable</h5>
                                                            <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGrid" runat="server" DataSourceID="InfoContable">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                                <Columns>
                                                                    <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Pedido" />
                                                                    <asp:BoundColumn DataField="Descripcion_TipoPedido" HeaderText="Tipo Pedido" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Precio_Venta" HeaderText="V. Venta" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="ValorPedido" HeaderText="Valor Pedido" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="PedidoBase" HeaderText="Ref" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="ValorBolsa" HeaderText="Total Ref" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="" HeaderText="Saldo" ItemStyle-CssClass="auto-width-column" />

                                                                </Columns>
                                                            </asp:DataGrid><asp:SqlDataSource runat="server" ID="InfoContable" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="SELECT Consecutivo_Pedido,Descripcion_TipoPedido,Precio_Venta,ValorPedido,PedidoBase,ValorBolsa 
                                                                               FROM tblTipoPedido AS TP	INNER JOIN tblTipoAprobacion AS TA	INNER JOIN tblOT As OT
                                                                               ON TA.IdTipoAprobacion = OT.TipoAprobacion ON TP.Id_TipoPedido = OT.Id_TipoPedido
                                                                               WHERE OT.Id_OT = @Id_OT ORDER BY Consecutivo_Pedido DESC ">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="tbOT" PropertyName="Text" Name="Id_OT"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>

                                                        </div>




                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="derecha">
                                        <div class="derecha1">
                                            <label for="">Tipo de Negociación</label>
                                            <textarea class="form-control form-control-sm" id="TextTNegociacion" runat="server" cols="25" rows="7"></textarea>
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="Datos-Cliente3">

                                <div class="Info1">
                                    <asp:Button ID="btnComision1" runat="server" Text="D. Comision" class="bi bf" disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end  " runat="server" ID="txtComision"></asp:TextBox>
                                </div>

                                <div class="Info1">
                                    <asp:Button ID="btnDiseño" runat="server" Text="Diseño" class="bi bf " disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end " runat="server" ID="txtDiseño"></asp:TextBox>
                                </div>

                                <div class="Info1">
                                    <asp:Button ID="btnSaldo" runat="server" Text="Saldo" class="bi bf " disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtSaldo"></asp:TextBox>
                                </div>

                                <div class="Info1">
                                    <asp:Button ID="btnVenta" runat="server" Text="Venta" class="bi bf " disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtVenta"></asp:TextBox>
                                </div>

                                <div class="Info1">
                                    <asp:Button ID="btnDcto" runat="server" Text="%Dcto" class="bi bf" disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtDcto" Width="50px"></asp:TextBox>
                                    <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtDctoValor" Width="130px"></asp:TextBox>
                                </div>

                                <div class="Info1">
                                    <asp:Button ID="btnVtte" runat="server" Text="V. VTte" class="bi bf" disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtVtte"></asp:TextBox>
                                </div>

                                <div class="Info1">
                                    <asp:Button ID="btnVvia" runat="server" Text="V. Via" class="bi bf" disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtVvia"></asp:TextBox>
                                </div>

                                <div class="Info1">
                                    <asp:Button ID="btnGTotal" runat="server" Text="G. Total" class="bi bf" disabled="true" />
                                    <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtGtotal"></asp:TextBox>
                                </div>


                            </div>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>



            <div class="tab-pane fade  " id="Plano-Content">
                <asp:UpdatePanel ID="PanelPlano" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid">
                            <!--Nav iconos Planos-->
                            <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
                                <div class="container-fluid">

                                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                        <span class="navbar-toggler-icon"></span>
                                    </button>
                                    <div class="collapse navbar-collapse" id="PlanoIconos">
                                        <ul class="navbar-nav mx-auto">

                                            <div class="contenedor-icono">

                                                <a class="icong disabled" href="#" title="Adicionar Objeto al Plano">
                                                    <i class="ib bi-pc"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Quitar Objeto del Plano">
                                                    <i class="bi bi-database-check"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Eliminar Objetos del Plano">
                                                    <i class="bi bi-fire"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Acabados del Plano">

                                                    <i class="bi bi-bar-chart-line"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Leer Archivo Despiece Acad">

                                                    <i class="bi bi-border-inner"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Cargar Archivo TXT XY">

                                                    <i class="bi bi-folder-plus"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Plano Bloqueado">

                                                    <i class="bi bi-lock"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Crear o Redefinir Bolsa">

                                                    <i class="bi bi-bag-check"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Adicionar/Remover Elementos de la Bolsa">

                                                    <i class="bi bi-bag-plus"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Despiece del Plano">

                                                    <i class="bi bi-disc-fill"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Generar  TXT">

                                                    <i class="bi bi-filetype-txt"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Guardar TXT">

                                                    <i class="bi bi-save2"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Exportar Plano u Orden de Trabajo">

                                                    <i class="bi bi-arrow-up-left-circle"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Visualizar/Generar Cotizacion">

                                                    <i class="bi bi-bag-plus"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Objetos no Existentes">

                                                    <i class="bi bi-text-indent-left"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Actualizar Precio Prototipo">

                                                    <i class="bi bi-cash-coin"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Generar Formato Certificado de Origen ">

                                                    <i class="bi bi-clipboard-check"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Importar Plano de Actualizacion de Bloques">

                                                    <i class="bi bi-file-arrow-down-fill"></i>
                                                </a>

                                                <ul />
                                        </ul>
                                    </div>

                                </div>
                            </nav>

                            <div class=" container-fluid panel-plano">

                                <div class="container-fluid descripcion-plano">

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Button ID="btnPlano" type="button" Text="Plano" class="btn btn-outline-secondary" runat="server"></asp:Button>
                                            <asp:TextBox ID="txtPlano" type="text" class="form-control  input" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label ID="lblCliente" class="form-label" Text="Cliente" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtCliente" type="text" class="form-control input" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label ID="lblArea" class="form-label" Text="Área" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtArea" type="text" class="form-control input" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label ID="Label2" class="form-label" Text="Contacto" runat="server"> </asp:Label>
                                            <asp:TextBox ID="txtContactoPlano" type="text" class="form-control input" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label ID="lblAsesor" class="form-label" Text="Asesor" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtAsesorPlano" type="text" class="form-control input" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label ID="lblDibuja" class="form-label" Text="Dibuja" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtDibuja" type="text" class="form-control input" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label ID="Label3" class="form-label" Text="Bolsa" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtBolsa" type="text" class="form-control input " runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label fid="lblResumenPlano" class="form-label" Text="Resumen del Plano" runat="server">Resumen del Plano</asp:Label>
                                            <textarea id="txResumen" class="form-control" style="overflow-y: scroll;" runat="server" rows="3"></textarea>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:TextBox ID="cbxImagen" type="checkbox" class="form-check-input" runat="server"></asp:TextBox>
                                            <asp:Label ID="lblVerImagen" class="form-check-label" Text="Ver imagen" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row justify-content-center">
                                        <div class="border rounded p-1 m-1">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="table-responsive mb-1" style="max-height: 11rem; overflow-x: auto;">
                                                        <h5 class="datagrid-header text-center">Acabado objeto</h5>
                                                        <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridAcabado" runat="server">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                            <Columns>

                                                                <asp:BoundColumn DataField="" HeaderText="" />
                                                                <asp:BoundColumn DataField="" HeaderText="" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="" HeaderText="" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="" HeaderText="" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="" HeaderText="" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="" HeaderText="" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="" HeaderText="" ItemStyle-CssClass="auto-width-column" />

                                                            </Columns>
                                                        </asp:DataGrid>

                                                    </div>




                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="container-fluid tabla-plano">
                                    <div class="row justify-content-center">
                                        <div class="border rounded p-1 m-1">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="table-responsive mb-1" style="max-height: 28rem; overflow-x: auto;">
                                                        <h5 class="datagrid-header text-center">Despiece</h5>
                                                        <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridDespiecePlano" runat="server" OnItemDataBound="DataGridDespiecePlano_ItemDataBound" OnItemCommand="DataGridDespiecePlano_LinkButton">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="...">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" runat="server" CommandName="VerPlano" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>"
                                                                            Visible='<%# !string.IsNullOrEmpty(Eval("ID")?.ToString()) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:TemplateColumn ItemStyle-Width="250px">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblHeader" runat="server" Visible="true">Grupo </asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTitulo" runat="server" Text='<%# Eval("Titulo") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:BoundColumn DataField="ID" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Descripcion" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Ancho" HeaderText="Ancho" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ValorUnd" HeaderText="Valor Und" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="SubTotal" HeaderText="Sub Total" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Id_Panel" HeaderText="" Visible="false" />



                                                            </Columns>
                                                        </asp:DataGrid>

                                                    </div>




                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class=" container-fluid panel-tabla  ">

                                <div class="row justify-content-center ">
                                    <div class="border rounded pb-2 mb-2">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="table-responsive mb-1" style="max-height: 11rem; overflow-x: auto;">
                                                    <h5 class="datagrid-header text-center">Descripcion Objetos</h5>
                                                    <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridDescripcionObjetos" runat="server" OnItemDataBound="DataGridDescripcionObjeto_ItemDataBound" OnItemCommand="DataGridDescripcionObjeto_LinkButton">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="...">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="VerAcabado" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Id_Modulo" HeaderText="Módulo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_TipoModulo" HeaderText="Tipo Módulo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Chequeado" HeaderText="OK" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                            <asp:BoundColumn DataField="Ubicacion_Modulo" HeaderText="Pos" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Lado" HeaderText="Lado" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_Familia" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="PanModResponsable" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="FechaChequeo" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />

                                                        </Columns>
                                                    </asp:DataGrid>

                                                </div>




                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row pt-1 mt-1">

                                    <div class="col-3">
                                        <div class="input-group input-group-sm gap-2 ">
                                            <asp:Label ID="lblCantidad" class="form-label" Text="Cantidad" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtCantidad" type="text" class=" form-control form-control-sm" runat="server"></asp:TextBox>
                                            <asp:Button ID="btnCambiar" type="button" class="btn btn-outline-secondary disabled" Text="Cambiar" runat="server"></asp:Button>
                                        </div>
                                    </div>

                                    <div class="col-2 ">
                                        <div class="input-group input-group-sm gap-4 d-flex justify-content-center ">
                                            <asp:Label CssClass="fw-bold fs-6" ID="lblDisp" class="form-label" Text="Dip. LA" runat="server"></asp:Label>
                                            <asp:Label CssClass="fw-bold fs-6" ID="lblValor" class="form-label" runat="server">0</asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-2 ">
                                        <div class="input-group input-group-sm gap-4 d-flex justify-content-center ">
                                            <asp:Label CssClass="fw-bold fs-6" ID="lbDipLB" class="form-label" Text="Dip. LA" runat="server"></asp:Label>
                                            <asp:Label CssClass="fw-bold fs-6" ID="lbValorDipLB" class="form-label" runat="server">0</asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-2 ">
                                        <div class="input-group input-group-sm gap-4 d-flex justify-content-center ">
                                            <asp:Label CssClass="fw-bold fs-6" ID="lblTotalObjeto" Text="Total Objetos" runat="server"></asp:Label>
                                            <asp:Label CssClass="fw-bold fs-6" ID="lblCantidad1" class="form-label" runat="server">0</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-3">
                                        <div class="input-group input-group-sm gap-3 d-flex justify-content-center ">
                                            <asp:Label CssClass="fw-bold fs-6" ID="lblValorDespiece" class="form-label" Text="Valor despiece" runat="server"></asp:Label>
                                            <asp:Label CssClass="fw-bold fs-6" ID="lblValorDespiece1" class="form-label" runat="server">0</asp:Label>
                                        </div>

                                    </div>

                                </div>

                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade " id="Objeto-Content">
                <asp:UpdatePanel ID="PanelObjeto" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <!--Nav iconos Objetos-->
                            <nav class="navbar navbar-expand-sm navbar-light bg-light">
                                <div class="container-fluid">

                                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2"
                                        aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                        <span class="navbar-toggler-icon"></span>
                                    </button>
                                    <div class="collapse navbar-collapse" id="ObjetosIconos">
                                        <ul class="navbar-nav mx-auto">

                                            <div class="contenedor-icono">


                                                <a class="icong disabled" href="#" title="Nuevo Objeto">

                                                    <i class="bi bi-file-earmark"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="...">

                                                    <i class="bi bi-printer"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Modificar Objeto">

                                                    <i class="bi bi-wrench"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Consultar Objeto">

                                                    <i class="bi bi-file-earmark-ruled"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Eliminar Objeto">

                                                    <i class="bi bi-database-x"></i>
                                                </a>

                                                <a class="icong disabled" href="#" title="Buscar Objeto">

                                                    <i class="bi bi-search"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Copiar Objeto">

                                                    <i class="bi bi-files"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Actualizar Precio">

                                                    <i class="bi bi-currency-dollar"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Generar Lista de Precios">

                                                    <i class="bi bi-coin"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Ir al Objeto Anterior">

                                                    <i class="bi bi-disc"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Chequear">

                                                    <i class="bi bi-check-lg"></i>
                                                </a>





                                            </div>
                                    </div>
                            </nav>

                            <div class="container-fluid">

                                <div class="row p-1 m-1">

                                    <div class="col-2">
                                        <div class="form-check">
                                            <asp:RadioButtonList ID="rbObjeto" runat="server">
                                                <asp:ListItem Selected="True" Value="Objeto">Por Objeto </asp:ListItem>
                                                <asp:ListItem Value="Descripcion">Por Descripción</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>

                                    </div>

                                    <div class="col-2">
                                        <div class="input-group-sm">
                                            <asp:Label class="form-label" Text="Grupo" runat="server" ID="lbGrupo"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlGrupo" runat="server" DataTextField="Descripcion" DataValueField="Descripcion" DataSourceID="GrupoObjetos" OnDataBound="ddlGrupoObjeto_DataBound"></asp:DropDownList>
                                            <asp:SqlDataSource runat="server" ID="GrupoObjetos" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select  ID_GrupoObjeto AS Valor,Descripcion_Grupo AS Descripcion from tblGrupoObjeto  order by Descripcion_Grupo "></asp:SqlDataSource>

                                        </div>
                                    </div>

                                    <div class="col-3">
                                        <div class="input-group-sm">
                                            <asp:Label class="form-label" Text="Criterio" runat="server" ID="lbCriterio"></asp:Label>
                                            <asp:TextBox ID="tbCriterio" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-1">
                                        <div class="input-group-sm">
                                            <asp:Label class="form-label" Text="Altura" runat="server" ID="lbAltura"></asp:Label>
                                            <asp:TextBox ID="tbAltura" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-1">
                                        <div class="input-group-sm">
                                            <asp:Label class="form-label" Text="Ancho" runat="server" ID="lbAncho"></asp:Label>
                                            <asp:TextBox ID="tbAncho" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-3">
                                        <div class="input-group-sm">
                                            <asp:CheckBox ID="chxBloques" runat="server" CssClass="form-check-input" Checked="true" />
                                            <asp:Label ID="lbBloquesActivos" runat="server" Text="Solo Bloques Activos" CssClass="form-label"></asp:Label>
                                        </div>
                                        <asp:Button ID="btnBuscarActivos" runat="server" Text="Buscar Sólo activos" CssClass="btn btn-sm btn-outline-secondary" OnClick="BuscarObjeto" />
                                    </div>


                                </div>


                                <div class="row justify-content-center">
                                    <div class="border rounded p-1 m-1">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="table-responsive mb-1" style="max-height: 20rem; overflow-x: auto;">
                                                    <h5 class="datagrid-header text-center">Objeto</h5>
                                                    <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridObjetos" runat="server" DataSourceID="ObtenerDatosObjetos" OnItemCommand=" DataGridObtenerDatosObjetos_LinkButton" OnItemDataBound="DataGridObtenerDatosObjetos_ItemDataBound">
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
                                                            <asp:BoundColumn DataField="Profundidad" HeaderText="Profundidad" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Precio_Venta" HeaderText="Venta" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_Grupo" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Id_Numerico" HeaderText="Ensamble" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="CubicajeM3" HeaderText="Cub(M3)" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Chequeado" HeaderText="Ok" ItemStyle-CssClass="auto-width-column"  DataFormatString="{0:Si;No}"/>
                                                            <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="FechaChequeo" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Divisiones" HeaderText="Div" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Holgura" HeaderText="Hol" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="UndxPaquete" HeaderText="UndxPaq" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="PesoKG" HeaderText="KG" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Activo" HeaderText="Activo" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                            <asp:BoundColumn DataField="Escalable" HeaderText="Esc" ItemStyle-CssClass="auto-width-column" />



                                                        </Columns>
                                                    </asp:DataGrid>
                                                    <asp:SqlDataSource ID="ObtenerDatosObjetos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommandType="StoredProcedure">
                                                        <SelectParameters>
                                                            <asp:ControlParameter Name="Altura" ControlID="tbAltura" PropertyName="Text" DefaultValue="%" Type="String" />
                                                            <asp:ControlParameter Name="Ancho" ControlID="tbAncho" PropertyName="Text" DefaultValue="%" Type="String" />
                                                            <asp:ControlParameter Name="Grupo" ControlID="ddlGrupo" PropertyName="Text" DefaultValue="%" Type="String" />
                                                            <asp:ControlParameter ControlID="tbCriterio" PropertyName="Text" DefaultValue="%" Name="Criterio" Type="String"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>




                                                </div>




                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row pt-2 mt-2">

                                    <div class="col-3 ">
                                        <div class="input-group input-group-sm">
                                            <asp:Label CssClass="fw-bold fs-6" ID="lbTituloObjeto" class="form-label" Text="Descripcion Objeto" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-2 ">
                                        <div class="input-group input-group-sm justify-content-end">
                                            <asp:Button ID="btnDespiece" runat="server" Text="Despiece" CssClass="btn btn-sm btn-outline-secondary" />
                                        </div>
                                    </div>


                                    <div class="col-3 ">
                                        <div class="input-group input-group-sm gap-4 d-flex ">
                                            <asp:Label CssClass="fw-bold fs-6" ID="lbDipLa2" class="form-label" Text="Dip. LA" runat="server"></asp:Label>
                                            <asp:Label CssClass="fw-bold fs-6" ID="ValorlbDipLa2" class="form-label" runat="server">0</asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-3 ">
                                        <div class="input-group input-group-sm gap-4 d-flex ">
                                            <asp:Label CssClass="fw-bold fs-6" ID="lbDipLa3" class="form-label" Text="Dip. LB" runat="server"></asp:Label>
                                            <asp:Label CssClass="fw-bold fs-6" ID="ValorlbDipLa3" class="form-label" runat="server">0</asp:Label>

                                        </div>
                                    </div>

                                </div>

                                <div class="row justify-content-center">
                                    <div class="border rounded pt-3 mt-3">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="table-responsive mb-1" style="max-height: 11rem; overflow-x: auto;">
                                                    <h5 class="datagrid-header text-center">Modulo del Objeto</h5>
                                                    <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridModuloObjetos" runat="server">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="...">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkView2" runat="server" CommandName="VerModulo" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Id_Modulo" HeaderText="Módulo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_TipoModulo" HeaderText="Tipo Módulo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Chequeado" HeaderText="OK" ItemStyle-CssClass="auto-width-column"  />
                                                            <asp:BoundColumn DataField="Ubicacion_Modulo" HeaderText="Pos" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Lado" HeaderText="Lado" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_Familia" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="PanModResponsable" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                           

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

            <div class="tab-pane fade  " id="Modulo-Content">
                <asp:UpdatePanel ID="PanelModulo" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <!--Nav icons Modulos-->
                            <nav class="navbar navbar-expand-sm navbar-light bg-light">
                                <div class="container-fluid">

                                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2"
                                        aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                        <span class="navbar-toggler-icon"></span>
                                    </button>
                                    <div class="collapse navbar-collapse" id="ModuloIconos">
                                        <ul class="navbar-nav mx-auto">

                                            <div class="contenedor-icono">


                                                <a class="icong disabled" href="#" title="">

                                                    <i class="bi bi-file-earmark"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="">

                                                    <i class="bi bi-file-medical"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="">

                                                    <i class="bi bi-wrench"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="">

                                                    <i class="bi bi-file-earmark-ruled"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="">

                                                    <i class="bi bi-database-down"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="">

                                                    <i class="bi bi-files"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="">

                                                    <i class="bi bi-check-lg"></i>
                                                </a>


                                            </div>
                                    </div>
                            </nav>


                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade " id="Insumo-Content">
                <asp:UpdatePanel ID="PanelInsumo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <!--Nav icons Insumos-->
                            <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
                                <div class="container-fluid">

                                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                        <span class="navbar-toggler-icon"></span>
                                    </button>
                                    <div class="collapse navbar-collapse" id="InsumoIconos">
                                        <ul class="navbar-nav mx-auto contenedor-icono">

                                            <div class="contenedor-icono">

                                                <a class="icong disabled" href="#" title="Nuevo Insumo">

                                                    <i class="bi bi-file-earmark"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="...">

                                                    <i class="bi bi-file-earmark-ruled"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Modificar Insumo">

                                                    <i class="bi bi-wrench"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Eliminar Insumo">


                                                    <i class="bi bi-database-x"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Copiar Insumo">

                                                    <i class="bi bi-files"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Buscar Insumo">

                                                    <i class="bi bi-search"></i>
                                                </a>
                                                <a class="icong disabled" href="#" title="Actualizar">

                                                    <i class="bi bi-disc"></i>
                                                </a>

                                            </div>
                                        </ul>
                                    </div>

                                </div>
                            </nav>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>

    </form>


    <script type="text/javascript">
        function openModal() {
            var myModal = new bootstrap.Modal(document.getElementById('myModal'), {
                keyboard: false
            });
            myModal.show();

            // Agrega la funcionalidad de mover el modal cuando se pasa el puntero sobre él
            $('#myModal').hover(function () {
                $(this).css({
                    'margin-top': Math.random() * 100,
                    'margin-left': Math.random() * 100
                });
            });

            // Código para cerrar el modal después de 2 segundos
            setTimeout(function () {
                myModal.hide();
            }, 1000);
        }
    </script>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var checkBox = document.getElementById('<%= chxBloques.ClientID %>');
            var boton = document.getElementById('<%= btnBuscarActivos.ClientID %>');

            checkBox.addEventListener('change', function () {
                if (this.checked) {
                    boton.value = 'Buscar Sólo activos';
                } else {
                    boton.value = 'Buscar Todos';
                }
            });
        });
    </script>







    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
