﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPrincipal.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.OrdenesDeTrabajo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/frmPrincipal.css" rel="stylesheet" />
    <title>Ordenes de trabajo</title>
</head>
<body>



    <div class="wrapper">

        <header>
            <nav class="navbar navbar-expand-lg navbar-light bg-light pt-0">

                <div class="container-fluid" style="background-color: #081a2c">

                    <div class="collapse navbar-collapse" id="navbarScroll">

                        <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="Departamento" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Departamento</a>
                                <ul class="dropdown-menu" aria-labelledby="Departamento">

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle  " href="#" id="Administrativo" role="button" data-bs-toggle="dropdown" aria-expanded="false">Administrativo </a>
                                        <ul class="dropdown-menu">
                                            <li class="nav-item dropdown ">
                                                <a class="nav-link dropdown-toggle " href="#" id="Gerencia" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="padding-left: 1rem">Gerencia Comercial </a>
                                                <ul class="dropdown-menu ">
                                                    <li><a class="dropdown-item" href="#">Actualizar Precios</a></li>
                                                    <li><a class="dropdown-item" href="#">Estadisticas de Venta</a></li>
                                                    <li><a class="dropdown-item" href="#">Seguimiento de Cotizaciones</a></li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Compras" role="button" data-bs-toggle="dropdown" aria-expanded="false">Compras</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Generar codigo de inventario</a></li>
                                            <li><a class="dropdown-item" href="#">Solicitud de producto especial</a></li>
                                            <li><a class="dropdown-item" href="#">Orden de abastecimiento interna</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Dise_Desa" role="button" data-bs-toggle="dropdown" aria-expanded="false">Diseño | Desarrollo</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Estadisticas de diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Estadisticas desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Generar código de inventario</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                            <li><a class="dropdown-item" href="#">Bitacora renders</a></li>
                                            <li><a class="dropdown-item" href="#">Bitacora desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Bitacora diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Estadisticas desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Estadisticas dibujo</a></li>
                                            <li><a class="dropdown-item" href="#">Reproceso dibujo</a></li>
                                            <li><a class="dropdown-item" href="#">Reproceso desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Diseño en el exterior</a></li>
                                            <li><a class="dropdown-item" href="#">Tabla de diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Plano</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Fact_Cart" role="button" data-bs-toggle="dropdown" aria-expanded="false">Facturacion y cartera</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Cierre de obra</a></li>
                                            <li><a class="dropdown-item" href="#">Control de obra</a></li>
                                            <li><a class="dropdown-item" href="#">Despacho de obras</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                            <li><a class="dropdown-item" href="#">Programación ordenes de T'S de SID</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Gest_Cali_Adno" role="button" data-bs-toggle="dropdown" aria-expanded="false">Gestio de calidad adnom</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Acciones de mejora</a></li>
                                            <li><a class="dropdown-item" href="#">Entrega perfecta</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Gest_Cali_Usr" role="button" data-bs-toggle="dropdown" aria-expanded="false">Gestion de calidad usr</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Accion de mejora</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Instalacion" role="button" data-bs-toggle="dropdown" aria-expanded="false">Instalacion</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Cierre de obra</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Recepcion" role="button" data-bs-toggle="dropdown" aria-expanded="false">Recepcion</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Generar cotización</a></li>
                                            <li><a class="dropdown-item" href="#">Ingresar cotizacion</a></li>
                                            <li><a class="dropdown-item" href="#">Tabla de diseños</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Sistemas" role="button" data-bs-toggle="dropdown" aria-expanded="false">Sistemas</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Administracion</a></li>
                                            <li><a class="dropdown-item" href="#">Asignar permiso</a></li>
                                            <li><a class="dropdown-item" href="#">Configurar sede</a></li>
                                            <li><a class="dropdown-item" href="#">Ventana principal</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Ventas" role="button" data-bs-toggle="dropdown" aria-expanded="false">Ventas</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Gestión comecial</a></li>
                                            <li><a class="dropdown-item" href="#">Licitaciones</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                            <li><a class="dropdown-item" href="#">Programar diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Programar render</a></li>
                                            <li><a class="dropdown-item" href="#">Seguimiento cotizaciones</a></li>
                                            <li><a class="dropdown-item" href="#">Solicitud producto especial</a></li>
                                            <li><a class="dropdown-item" href="#">Visitas asesores</a></li>
                                        </ul>
                                    </li>
                                </ul>

                            </li>


                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="Personas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Personas </a>
                                <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                    <li><a class="dropdown-item" href="#">Cliente</a></li>
                                    <li><a class="dropdown-item" href="#">Empleado</a></li>

                                </ul>
                            </li>

                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="Consultas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Consultas</a>
                                <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                    <li><a class="dropdown-item" href="#">Reprocesos</a></li>
                                    <li><a class="dropdown-item" href="#">Despacho de obra</a></li>
                                    <li class="nav-item dropend ">
                                        <a class="nav-link dropdown-toggle " href="#" id="OT" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="padding-left: 1rem">Ordenes de trabajo </a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Programacion de OT</a></li>
                                            <li><a class="dropdown-item" href="#">Todas las OT (Manuales /SID)</a></li>
                                        </ul>
                                    </li>

                                </ul>

                            </li>
                        </ul>

                        <asp:Label ID="lblBienvenida" runat="server" ForeColor="White"></asp:Label>

                    </div>

                </div>

            </nav>
        </header>


        <%--Comienza Panel principal de nombres--%>

        <nav class="navbar navbar-expand-sm navbar-light bg-light">
            <div class="container">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">



                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="#">Ordenes de trabajo</a>
                        </li>
                    </ul>


                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Plano.aspx">Plano</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav mx-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Objetos.aspx">Objetos</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="modulo.aspx">Modulos</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Insumos.aspx">Insumos</a>
                        </li>
                    </ul>





                </div>
            </div>
        </nav>

        <%--Termina Panel principal de nombres--%>


        <%--Comienza Panel de iconos--%>

        <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
            <div class="container-fluid">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="ejemplo2">
                    <ul class="navbar-nav mx-auto contenedor-icono">



                        <div class="contenedor-icono">



                            <%--Comienza Nueva OT--%>

                            <a class="icong" href="#" title="Nueva OT">
                                <i class="bi bi-file-earmark"></i>
                            </a>

                            <a class="icong" href="#" title="Copiar Información en una Nueva OT" disabled="true">
                                <i class="bi bi-files"></i>
                            </a>
                            <a class="icong" href="#" title="Grabar Orden de Trabajo">
                                <i class="bi bi-save2"></i>
                            </a>

                            <a class="icong" href="#" title="Modificar Orden de Trabajo">
                                <i class="bi bi-wrench"></i>
                            </a>

                            <a class="icong" href="#" title="Anular o Eliminar un Pedido">
                                <i class="bi bi-file-earmark-excel"></i>
                            </a>

                            <a class="icong" href="#" title="Documentación OT">
                                <i class="bi bi-paperclip"></i>
                            </a>

                            <a class="icong" href="#" title="Observaciones OT">
                                <i class="bi bi-eye"></i>
                            </a>
                            <a class="icong" href="#" title="Imprimir Informacion General de la OT">
                                <i class="bi bi-printer"></i>
                            </a>

                            <a class="icong" href="#" title="Reimprimir Información Contable">
                                <i class="bi bi-printer-fill"></i>
                            </a>

                            <a class="icong" href="#" title="Consultar Bolsa">
                                <i class="bi bi-coin"></i>
                            </a>

                            <a class="icong" href="#" title="Cancelar">
                                <i class="bi bi-x-lg"></i>
                            </a>

                            <a class="icong" href="#" title="Visualizar OT Pendientes">
                                <i class="bi bi-eyeglasses"></i>
                            </a>
                            <a class="icong" href="#" title="Actualizar Pedidos Importados">
                                <i class="bi bi-check-square"></i>
                            </a>

                            <a class="icong" href="#" title="Importar Pedido Asesor">
                                <i class="bi bi-person-lines-fill"></i>
                            </a>

                            <a class="icong" href="#" title="Importar Pedido Sede ">
                                <i class="bi bi-house-up"></i>
                            </a>
                            <a class="icong" href="#" title="Habilitar Pedido para Ventas">
                                <i class="bi bi-receipt-cutoff"></i>
                            </a>

                            <a class="icong" href="#" title="Deshabilitar Orden de Trabajo para Producción ">
                                <i class="bi bi-sign-stop"></i>
                            </a>
                            <a class="icong" href="#" title="Indicador Obra Reactivada ">
                                <i class="bi bi-bar-chart-line"></i>
                            </a>

                            <a class="icong" href="#" title="Registrar Pedido en el Sistema Administrativo ">
                                <i class="bi bi-triangle"></i>
                            </a>
                            <a class="icong" href="#" title="Cierra o Abre una OT ">
                                <i class="bi bi-key"></i>
                            </a>
                            <a class="icong" href="#" title="Simular Pasar Pedido ">
                                <i class="bi bi-code-square"></i>
                            </a>

                            <a class="icong" href="#" title="Exportar Pedido ">
                                <i class="bi bi-airplane-engines"></i>
                            </a>

                            <a class="icong" href="#" title="Entrega Perfecta ">
                                <i class="bi bi-lightning-charge"></i>
                            </a>


                            <a class="icong" href="#" title="Anular Obra">
                                <i class="bi bi-x-square"></i>
                            </a>



                            <ul />
                    </ul>
                </div>

            </div>
        </nav>

        <%--Termina Panel de iconos--%>
    
    </div><%--Fin div wrapper --%>

    




    <%--Comienza Formulario Principal--%>


    <form class="frmPrincipal" runat="server">

        <div class="container-fluid">

            <div class="row">


                <div class="col-1">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="OT" runat="server" ID="lblOT"></asp:Label>
                        <asp:TextBox ID="tbOT" runat="server" CssClass="form-control" OnTextChanged="tbOT_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </div>
                </div>


                <div class="col-1">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Pedido" runat="server" ID="lblPedido"></asp:Label>
                        <asp:DropDownList class="form-control" ID="ddlNumbers" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNumbers_SelectedIndexChanged"></asp:DropDownList>

                    </div>
                </div>
                <div class="col-1">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label ID="LblZona" Text="Zona" CssClass="form-label" runat="server"></asp:Label>
                        <asp:TextBox ID="tbZona" type="text" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-3">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="T.Ped" runat="server" ID="lblTped"></asp:Label>
                        <asp:TextBox ID="tbTped" type="text" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-1">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Ped.Base" runat="server" ID="lblPedBase"></asp:Label>
                        <asp:TextBox ID="tbPedBase" type="number" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-2">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Ped.Depen" runat="server" ID="lblPedDepen"></asp:Label>
                        <asp:TextBox ID="tbPedDepen" type="number" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-2">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Aprob" runat="server" ID="lblAprob"></asp:Label>
                        <asp:TextBox ID="tbAprob" type="text" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>


                <div class="col-1">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Button ID="btnNuevoPedido" runat="server" class="bi bi-file-earmark btn btn-secondary "></asp:Button>
                        <asp:Button ID="btnAcabados" runat="server" class="btn btn-secondary"></asp:Button>
                        <asp:Button ID="btnOk" runat="server" type="button" Text="OK" class="btn btn-secondary"></asp:Button>
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
                        <div class="input-group input-group-sm mb-2 gap-2">
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

                    <div class="row">


                        <div class="col-3">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="Recibe" runat="server" ID="lblRecibe"></asp:Label>
                                <asp:TextBox ID="tbRecibe" type="text" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-3">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="Ciudad" runat="server" ID="lblCiudad"></asp:Label>
                                <asp:TextBox ID="tbCiudad" type="text" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="Tel" runat="server" ID="lblTel"></asp:Label>
                                <asp:TextBox ID="tbTel" type="tel" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-1">
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
            </div>
        </div>















        <%--Pantalla intermedia--%>




        <div class="container-fluid">
            <div class="observaciones">

                <div class="div-1">


                    <div class="row">
                        <div class="mb-2 gap-2">
                            <textarea id="Observacion5Id" runat="server" class="form-control form-control-ms"></textarea>
                        </div>
                    </div>
                </div>





                <div class="div-2">

                    <div class="row">
                        <div class="col-6">

                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="Venta" runat="server" ID="lblVenta"></asp:Label>
                                <asp:TextBox ID="tbVenta" type="text" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label class="form-label" runat="server" id="inputOkVenta">Ok.Venta</label>
                                <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputOkVenta" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="inputDibujo">Ok.Dibujo</label>
                                    <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputDibujo" />
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-6">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="inputEmpaque">Empaque</label>
                                    <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputEmpaque" />
                                </div>
                            </div>

                            <div class="col-6">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="inputRealEmp">Real Emp.</label>
                                    <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputRealEmp" />
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-12">
                                <div class="table-responsive table-secondary overflow-auto m-2 ">

                                    <h6 class="datagrid-header text-center">Grid 1</h6>

                                    <asp:DataGrid ID="DataGrid1" runat="server"></asp:DataGrid>
                                    <asp:SqlDataSource runat="server" ID="DataGridDespacho"></asp:SqlDataSource>


                                </div>
                            </div>
                        </div>


                    </div>

                </div>

            </div>
        </div>

        <div class="container-fluid">
            <div class="observaciones">

                <div class="div-1">


                    <div class="row">
                        <div class="mb-2 gap-2">
                            <textarea id="Observacion1Id" runat="server" class="form-control form-control-sm"></textarea>
                        </div>
                    </div>
                </div>





                <div class="div-3">



                    <div class="row">

                        <div class="col-4">

                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="Supervisor" runat="server" ID="lblSupervisor"></asp:Label>
                                <asp:TextBox ID="tbSupervisor" type="text" class="form-control" runat="server" />
                            </div>
                        </div>

                    </div>

                    <div class="row">


                        <div class="col-5">
                            <div class="input-group input-group-sm mb-2">
                                <div class="input-group-prepend">
                                    <asp:Button class="btn btn-outline-secondary" Text="Plano+" runat="server" type="button"></asp:Button>
                                </div>
                                <asp:TextBox type="text" class="form-control" runat="server" ID="tbPlano" />
                            </div>
                        </div>



                        <div class="col-7">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="Bolsa" runat="server" ID="lblBolsa"></asp:Label>
                                <asp:TextBox type="text" class="form-control" runat="server" ID="tbBolsa" />
                            </div>
                        </div>
                    </div>

                    <div class="row">

                        <div class="col-4">

                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="Fabrica" runat="server" ID="lblFabrica"></asp:Label>
                                <asp:TextBox type="text" class="form-control" runat="server" ID="TextFabrica" />

                            </div>

                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" Text="V.Pedido" runat="server" ID="lblVPedido"></asp:Label>
                            </div>
                        </div>


                        <div class="col-6">
                            <%--<input type="text" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputSaldo" />--%>
                        </div>

                        <div class="row">

                            <div class="col-4">

                                <div class="input-group input-group-sm mb-2 gap-3">
                                    <asp:Label class="form-label" Text="Instala" runat="server" ID="lblInstala"></asp:Label>
                                    <asp:TextBox type="text" class="form-control" runat="server" ID="TextInstala" />

                                </div>

                            </div>
                            <div class="col-8">

                                <div class="input-group-sm mb-1 gap-2">

                                    <div class="input-group mb-3">
                                        <div class="input-group-prepend">
                                            <asp:Button class="btn btn-outline-secondary" runat="server" Text="TXT" type="button"></asp:Button>
                                        </div>
                                        <input type="text" class="form-control" placeholder="" aria-label="" aria-describedby="basic-addon1" />
                                    </div>
                                </div>
                            </div>



                        </div>

                    </div>


                </div>


            </div>
        </div>


        <div class=" container-fluid Info-Contable">

            <div class="Datos-Cliente1">

                <div class="Info1 input-group input-group-sm">
                    <asp:Label class="form-label" Text="NIT" runat="server" ID="lblNit1"></asp:Label>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtNit"></asp:TextBox>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtNombreEmp"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Label class="form-label" Text="Contacto" runat="server" ID="lblContacto"></asp:Label>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtcontacto"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Label class="form-label" Text="Mail" runat="server" ID="lblMail"></asp:Label>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtMail"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Label class="form-label" Text="Dirección" runat="server" ID="lblDireccion"></asp:Label>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtDireccion"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Label class="form-label" Text="Municipio" runat="server" ID="lblMunicipio"></asp:Label>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtMunicipio"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Label class="form-label" Text="Telefono" runat="server" ID="lblTelefono"></asp:Label>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtTelefono"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Label class="form-label" Text="Obs. Contable " runat="server" ID="lblObs"></asp:Label>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtObs"></asp:TextBox>
                </div>

                <div class="Info_F ">

                    <asp:TextBox type="" class="form-control" runat="server" ID="txtMensaje" Visible="False"></asp:TextBox>
                </div>

            </div>

            <div class="Datos-Cliente2">

                <div class="superior">
                    <div class="Info2">
                        <asp:Button ID="btnCotizacion" runat="server" Text="Ver cotización" class="bi bf" />
                        <asp:TextBox type="text" class="form-control" runat="server" ID="txtCotizacion"></asp:TextBox>
                    </div>

                    <div class="Info2">
                        <asp:Button ID="btnValorSugeroido" runat="server" Text="Valor Sugerido" class="bi bf" disabled="true" />
                        <asp:TextBox type="text" class="form-control" runat="server" ID="txtValorSugerido"></asp:TextBox>
                    </div>

                    <div class="Info2">
                        <asp:Button ID="btnVscd" runat="server" Text="VCSD" class="bi bf" disabled="true" />
                        <asp:TextBox type="text" class="form-control" runat="server" ID="txtVcsd"></asp:TextBox>
                    </div>

                    <div class="Info2">
                        <asp:Button ID="btnVccd" runat="server" Text="VCCD" class="bi bf" disabled="true" />
                        <asp:TextBox type="text" class="form-control" runat="server" ID="txtVccd"></asp:TextBox>
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
                        <asp:DropDownList ID="DblNombreAsesor" class="form-control" runat="server"></asp:DropDownList>
                    </div>


                </div>


                <div class="Div_Grid">

                    <div class="izquierda">
                        <label>
                            Venta
                            <br />
                            Neta</label>
                    </div>

                    <div class="  overflow-auto centro">
                        <h6 class="text-center">Grid 2</h6>

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
                    <asp:TextBox type="text" class="form-control " runat="server" ID="txtComision"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Button ID="btnDiseño" runat="server" Text="Diseño" class="bi bf " disabled="true" />
                    <asp:TextBox type="text" class="form-control " runat="server" ID="txtDiseño"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Button ID="btnSaldo" runat="server" Text="Saldo" class="bi bf " disabled="true" />
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtSaldo"></asp:TextBox>
                </div>
                <div class="Info1">
                    <asp:Button ID="btnVenta" runat="server" Text="Venta" class="bi bf " disabled="true" />
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtVenta"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Button ID="btnDcto" runat="server" Text="%Dcto" class="bi bf" disabled="true" />
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtDcto"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Button ID="btnVtte" runat="server" Text="V. VTte" class="bi bf" disabled="true" />
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtVtte"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Button ID="btnVvia" runat="server" Text="V. Via" class="bi bf" disabled="true" />
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtVvia"></asp:TextBox>
                </div>

                <div class="Info1">
                    <asp:Button ID="btnGTotal" runat="server" Text="G. Total" class="bi bf" disabled="true" />
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtGtotal"></asp:TextBox>
                </div>


            </div>




        </div>



    </form>




















    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>


