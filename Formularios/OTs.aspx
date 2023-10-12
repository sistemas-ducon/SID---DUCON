<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OTs.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.OTs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
          <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

       <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />


      <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/frmPrincipal.css" rel="stylesheet" />
    <link href="../../Recursos/CSS/DiseñoYDesarrollo/Plano.css" rel="stylesheet" />

    <title>Ordenes de trabajo</title>
</head>
<body>
  <form id="frmPrincipal" runat="server">
             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
   <div class="wrapper">
       <%--MENU PRINCIPAL--%>
         


<nav class="navbar navbar-expand-lg navbar-light bg-light shadow p-3 mb-5 bg-body rounded">
  <div class="container-fluid" style="background-color: #081a2c " >
    <a class="navbar-brand" href="#"></a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarScroll" aria-controls="navbarScroll" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarScroll">
      <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">
        


      
             <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="Departamento" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">
            Departamento
          </a>
          <ul class="dropdown-menu" aria-labelledby="Departamento">
              

             <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Administrativo" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Administrativo
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Gerencia Comercial</a></li>
                     
                </ul>
            </li>
         
   




               <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Compras" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Compras
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Generar codigo de inventario</a></li>
                     <li><a class="dropdown-item" href="#">Solicitud de producto especial</a></li>
                     <li><a class="dropdown-item" href="#">Orden de abastecimiento interna</a></li>
                </ul>
            </li>

              <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Dise_Desa" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Diseño | Desarrollo
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Estadisticas de diseño</a></li>
                     <li><a class="dropdown-item" href="#">Estadisticas desarrollo</a></li>
                     <li><a class="dropdown-item" href="#">Generar código de inventario</a></li>


                    <li><a class="dropdown-item" href="OT.aspx">Ordenes de trabajo</a></li> 

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
                <a class="nav-link dropdown-toggle" href="#" id="Fact_Cart" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Facturacion y cartera
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Cierre de obra</a></li>
                     <li><a class="dropdown-item" href="#">Control de obra</a></li>
                    <li><a class="dropdown-item" href="#">Despacho de obras</a></li>
                     <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                    <li><a class="dropdown-item" href="#">Programación ordenes de T'S de SID</a></li>


                </ul>
            </li>

              <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Gest_Cali_Adno" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Gestio de calidad adnom
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Acciones de mejora</a></li>
                    <li><a class="dropdown-item" href="#">Entrega perfecta</a></li>
                </ul>
            </li>

              <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Gest_Cali_Usr" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Gestion de calidad usr
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Accion de mejora</a></li>
                </ul>
            </li>
            
              <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Instalacion" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Instalacion
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Cierre de obra</a></li>
                    <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                </ul>
            </li>

              <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Recepcion" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Recepcion
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Generar cotización</a></li>
                    <li><a class="dropdown-item" href="#">Ingresar cotizacion</a></li>
                    <li><a class="dropdown-item" href="#">Tabla de diseños</a></li>
                </ul>
            </li>

              <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Sistemas" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Sistemas
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Administracion</a></li>
                    <li><a class="dropdown-item" href="#">Asignar permiso</a></li>
                    <li><a class="dropdown-item" href="#">Configurar sede</a></li>
                    <li><a class="dropdown-item" href="#">Ventana principal</a></li>
                </ul>
            </li>

              <li class="nav-item dropend">
                <a class="nav-link dropdown-toggle" href="#" id="Ventas" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Ventas
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="ventas/Gestion_Comercial.aspx">Gestión comecial</a></li>
                    <li><a class="dropdown-item" href="ventas/Licitaciones.aspx">Licitaciones</a></li>
                    <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                    <li><a class="dropdown-item" href="ventas/Diseño_Venta.aspx">Programar diseño</a></li>
                    <li><a class="dropdown-item" href="ventas/Render_Venta.aspx">Programar render</a></li>
                    <li><a class="dropdown-item" href="ventas/Consulta_Cotizacion.aspx">Seguimiento cotizaciones</a></li>
                    <li><a class="dropdown-item" href="ventas/Solicitud_Especial.aspx">Solicitud producto especial</a></li>
                    <li><a class="dropdown-item" href="ventas/Visita_Asesores.aspx">Visitas asesores</a></li>
                </ul>
            </li>
          </ul>
        </li>



      
          
             <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="Personas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">
            Personas
          </a>
          <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
            <li><a class="dropdown-item" href="#">Cliente</a></li>
            <li><a class="dropdown-item" href="#">Empleado</a></li>

              </ul>
            </li>
          
        
             



          <li class="nav-item dropdown">
          <a class="nav-link dropdown-toggle" href="#" id="Consultas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">
            Consultas
          </a>
          <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
            <li><a class="dropdown-item" href="#">Reprocesos</a></li>
            <li><a class="dropdown-item" href="#">Despacho de obra</a></li>
              <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
              <li><a class="dropdown-item" href="frmPrincipal.aspx">Programacion de OT</a></li>
              <li><a class="dropdown-item" href="#">Todas las OT (Manuales /SID)</a></li>
            
          </ul>
        </li>
      </ul>
        <hr class="text-white-50" />
      
      
       
           
            <asp:label ID="lblBienvenida" runat="server" ForeColor="White"></asp:label>
            
       
      
  </div>
  </div>
</nav>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
         

              <%--FIN MENU PRINCIPAL--%>       
         <%--Inicio de tab--%>


       <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="OTs" data-bs-toggle="tab" href="#OrdenesDeTrabajo">Orden de trabajo</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Plano-tab" data-bs-toggle="tab" href="#Plano-content">Plano</a>
                    </li>
                     <li class="nav-item">
                        <a class="nav-link text-dark" id="Objetos-tab" data-bs-toggle="tab" href="#Objetos-content">Objetos</a>
                    </li>
                      <li class="nav-item">
                        <a class="nav-link text-dark" id="Modulos-tab" data-bs-toggle="tab" href="#Modulos-content">Módulos</a>
                    </li>
                      <li class="nav-item">
                        <a class="nav-link text-dark" id="Insumos-tab" data-bs-toggle="tab" href="#Insumos-content">Insumos</a>
                    </li>
                </ul>
            </div>
        </nav>

         <%--Comienza contenido del tab--%>
       <div class="tab-content">
              
            <div class="tab-pane fade show active" id="OrdenesDeTrabajo">
              
                
                <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
            <div class="container-fluid">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="ejemplo2">
                    <ul class="navbar-nav mx-auto contenedor-icono">



                        <div class="contenedor-icono">



                            <%--Comienza Nueva OT--%>

                           
                            <a class="icong disabled" href="#" title="Nueva OT" id="NuevaOt" onclick="NuevaOt()">
                                <i class="bi bi-file-earmark"></i>
                            </a>

                            <a class="icong disabled" href="#" title="Copiar Información en una Nueva OT" id="CopiarOt" >
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
                
        <div class="container-fluid">
           
       
                   <asp:Label ID="msgOtCerrada" runat="server" Text="CERRADA EL" BackColor="#DD0000" ForeColor="Black" Font-Size="XX-Large" Width="593px" style="text-align: center;" Visible="False"></asp:Label>
                
                  
                       
              <br />   
               <br />
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
                        <asp:DropDownList ID="dtacboTipoPedido" runat="server" Width="335px" AutoPostBack="True" DataSourceID="TiposDePedidos" DataTextField="Descripcion_TipoPedido" DataValueField="Descripcion_TipoPedido"></asp:DropDownList>
                        <asp:SqlDataSource ID="TiposDePedidos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select Descripcion_TipoPedido from tblTipoPedido order by Descripcion_TipoPedido"></asp:SqlDataSource>
                    </div>
                </div>

                <div class="col-1">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Ped.Base" runat="server" ID="lblPedBase"></asp:Label>
                        <asp:DropDownList ID="cboPedidoBase" runat="server" DataSourceID="PedidoBase" DataTextField="PedidoBase" DataValueField="PedidoBase" Width="217px"></asp:DropDownList>
                        <asp:SqlDataSource ID="PedidoBase" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT DISTINCT PedidoBase FROM tblOT WHERE (PedidoBase BETWEEN 1 AND 1000) ORDER BY PedidoBase"></asp:SqlDataSource>
                    </div>
                </div>

                <div class="col-2">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Ped.Depen" runat="server" ID="lblPedDepen"></asp:Label>
                        <asp:TextBox ID="tbPedDepen" type="number" class="form-control" runat="server" Width="90px"></asp:TextBox>
                    </div>
                </div>

                <div class="col-2">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Aprob" runat="server" ID="lblAprob"></asp:Label>
                        <asp:TextBox ID="tbAprob" type="text" class="form-control" runat="server" Visible ="false"></asp:TextBox>
                        <asp:DropDownList ID="DtaCboTipoAprobacion" runat="server" Width="197px" DataSourceID="Aprob" DataTextField="TipoAprobacion" DataValueField="IdTipoAprobacion"></asp:DropDownList>

                        <asp:SqlDataSource ID="Aprob" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select * from tblTipoAprobacion order by TipoAprobacion"></asp:SqlDataSource>

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
               
         <div class="container-fluid">
              <h6>PORTADA</h6>
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
                                <asp:TextBox ID="tbVenta" type="date" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label class="form-label" runat="server" id="inputOkVenta">Ok.Venta</label>
                                         <asp:TextBox ID="dtpFechaEntregaDibujoDespiece" type="date" class="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="inputDibujo">Ok.Dibujo</label>
                                   <asp:TextBox ID="dtpFechaEntregaProduccion" type="date" class="form-control" runat="server"></asp:TextBox>
                                </div>
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


                            <div class="input-group input-group-sm mb-2 gap-2">
                                <asp:Label class="form-label" runat="server" ID="lblSaldoOT" BorderColor="#006600" BackColor="Lime" Font-Size="XX-Large"  ></asp:Label>
                            
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
                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtValorPedido" />
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
                                        
                                    </div>
                                </div>
                            </div>



                        </div>

                    </div>


                </div>


            </div>
        </div>
                <div>
                          <h6>&nbsp;&nbsp;&nbsp; INFORMACIÓN CONTABLE</h6>
              
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
              
                     <textarea class="form-control form-control-sm" id="ObservacionCont" runat="server" cols="40" rows="7" high="60px"></textarea>
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
                            <textarea class="form-control form-control-sm" id="TextTNegociacion" runat="server" cols="25" rows="7"  ></textarea>
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
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtDcto" Width ="50px"></asp:TextBox>
                    <asp:TextBox type="text" class="form-control" runat="server" ID="txtDctoValor"></asp:TextBox>
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
            
            </div>
       
            <div class="tab-pane fade " id="Plano-content">
                
          <%--Comienza Panel de iconos--%>


    <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
        <div class="container-fluid">

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="iconosPlano">
                <ul class="navbar-nav mx-auto">



                    <div class="contenedor-icono">



                        <%--Comienza Nueva OT--%>



                        <%--Termina Nueva OT--%>

                        <a class="text-dark" href="#" title="Adicionar Objeto al Plano">
                            <i class="ib bi-pc"></i>
                        </a>
                        <a class="text-dark" href="#" title="Quitar Objeto del Plano">
                            <i class="bi bi-database-check"></i>
                        </a>
                        <a class="text-dark" href="#" title="Eliminar Objetos del Plano">
                            <i class="bi bi-fire"></i>
                        </a>
                        <a class="text-dark" href="#" title="Acabados del Plano">

                            <i class="bi bi-bar-chart-line"></i>
                        </a>
                        <a class="text-dark" href="#" title="Leer Archivo Despiece Acad">

                            <i class="bi bi-border-inner"></i>
                        </a>
                        <a class="text-dark" href="#" title="Cargar Archivo TXT XY">

                            <i class="bi bi-folder-plus"></i>
                        </a>
                        <a class="text-dark" href="#" title="Plano Bloqueado">

                            <i class="bi bi-lock"></i>
                        </a>
                        <a class="text-dark" href="#" title="Crear o Redefinir Bolsa">

                            <i class="bi bi-bag-check"></i>
                        </a>
                        <a class="text-dark" href="#" title="Adicionar/Remover Elementos de la Bolsa">

                            <i class="bi bi-bag-plus"></i>
                        </a>
                        <a class="text-dark" href="#" title="Despiece del Plano">

                            <i class="bi bi-disc-fill"></i>
                        </a>
                        <a class="text-dark" href="#" title="Generar  TXT">

                            <i class="bi bi-filetype-txt"></i>
                        </a>
                        <a class="text-dark" href="#" title="Guardar TXT">

                            <i class="bi bi-save2"></i>
                        </a>

                        <a class="text-dark" href="#" title="Exportar Plano u Orden de Trabajo">

                            <i class="bi bi-arrow-up-left-circle"></i>
                        </a>
                        <a class="text-dark" href="#" title="Visualizar/Generar Cotizacion">

                            <i class="bi bi-bag-plus"></i>
                        </a>
                        <a class="text-dark" href="#" title="Objetos no Existentes">

                            <i class="bi bi-text-indent-left"></i>
                        </a>
                        <a class="text-dark" href="#" title="Actualizar Precio Prototipo">

                            <i class="bi bi-cash-coin"></i>
                        </a>
                        <a class="text-dark" href="#" title="Generar Formato Certificado de Origen ">

                            <i class="bi bi-clipboard-check"></i>
                        </a>
                        <a class="text-dark" href="#" title="Importar Plano de Actualizacion de Bloques">

                            <i class="bi bi-file-arrow-down-fill"></i>
                        </a>

                        <ul />
                </ul>
            </div>

        </div>
    </nav>
                 <!-- Termina Panel Iconos-->


                
    <!-- comieza el formulario de Planos-->
      

                  <div class=" container-fluid Plano">

            <div class="container-fluid panel-plano ">
                <div class="  descripcion-plano">
                    <div class="item-plano">
                        <asp:Button ID="btnPlano" type="button" Text="Plano" class="btn btn-outline-secondary"
                            runat="server"></asp:Button>
                        <asp:TextBox ID="txtPlano" type="text" class="form-control  input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblCliente" class="form-label" Text="Cliente" runat="server"></asp:Label>
                        <asp:TextBox ID="txtCliente" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblArea" class="form-label" Text="Área" runat="server"></asp:Label>
                        <asp:TextBox ID="txtArea" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="Label1" class="form-label" Text="Contacto" runat="server">
                        </asp:Label>
                        <asp:TextBox ID="TextBox1" type="text" class="form-control input" runat="server">
                        </asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblAsesor" class="form-label" Text="Asesor" runat="server"></asp:Label>
                        <asp:TextBox ID="TextBox2" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>
                    <div class="item-plano">
                        <asp:Label ID="lblDibuja" class="form-label" Text="Dibuja" runat="server"></asp:Label>
                        <asp:TextBox ID="txtDibuja" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="Label2" class="form-label" Text="Bolsa" runat="server"></asp:Label>
                        <asp:TextBox ID="txtBolsa" type="text" class="form-control input " runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano2">
                        <asp:Label fid="lblResumenPlano" class="form-label" Text="Resumen del Plano"
                            runat="server">
                                    Resumen del Plano</asp:Label>
                        <textarea id="txResumen" class="form-control" style="overflow-y: scroll;"
                            runat="server" rows="3"></textarea>

                    </div>


                    <div class="item-plano2">
                        <asp:TextBox ID="cbxImagen" type="checkbox" class="form-check-input" runat="server">
                        </asp:TextBox>
                        <asp:Label ID="lblVerImagen" class="form-check-label" Text="Ver imagen" runat="server">
                        </asp:Label>
                    </div>

                </div>

                <div class=" tabla-plano overflow-auto">

                    <asp:DataGrid CssClass="table table-responsive custom-grid table-hover" ID="DataGrid1" runat="server" DataSourceID="DataGridPlano" AutoGenerateColumns="false">
                        <%--OnItemDataBound="DataGrid1_ItemDataBound" OnDataBound="DataGrid1_DataBound" --%>


                        <Columns>

                            <asp:BoundColumn DataField="Id_Numerico" HeaderText="ID" />




                            <asp:BoundColumn DataField="Descripcion_Panel" HeaderText="Descripcion" />
                            <asp:BoundColumn DataField="Altura" HeaderText="Alt" />
                            <asp:BoundColumn DataField="Ancho" HeaderText="Anch" />
                            <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" />
                            <asp:BoundColumn DataField="Precio_Venta" HeaderText="V.Und" />
                            <%-- <asp:BoundColumn DataField="" HeaderText="Sub Total" />

                     <asp:TemplateColumn HeaderText="ID">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" Text="CARLOS"  CssClass="text-dark text-decoration-none"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                            --%>
                        </Columns>


                    </asp:DataGrid>

                    <asp:SqlDataSource runat="server" ID="DataGridPlano" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                        SelectCommand="cta_Plano_Paneles" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtPlano" PropertyName="Text" Name="Plan" Type="String"></asp:ControlParameter>
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>

            <div class=" container-fluid panel-tabla  ">
                <div class="  tabla2-plano">
                </div>
            </div>

            <div class="container-fluid footer">

                <div class="item-footer">
                    <asp:Label ID="lblCantidad" class="form-label" Text="Cantidad" runat="server"></asp:Label>
                    <asp:TextBox ID="txtCantidad" type="text" class=" input" runat="server"></asp:TextBox>
                    <asp:Button ID="btnCambiar" type="button" class="btn btn-outline-secondary disabled"
                        Text="Cambiar" runat="server"></asp:Button>
                </div>

                <div class="item-footer1">
                    <asp:Label ID="lblDisp" class="form-label" Text="Dip. LA" runat="server"></asp:Label>
                    <asp:Label ID="lblValor" class="form-label" runat="server">0000</asp:Label>
                    <asp:Label ID="lblTotalObjeto" Text="Total Objetos" runat="server"></asp:Label>
                    <asp:Label ID="lblValor2" class="form-label" runat="server">000</asp:Label>
                </div>

                <div class="item-footer2">
                    <asp:Label ID="lblValorDespiece" class="form-label" Text="Valor despiece" runat="server">
                    </asp:Label>
                    <asp:Label ID="lblValor3" class="form-label" runat="server">222</asp:Label>
                </div>

            </div>

        </div>
                

   </div>
                


    </div>

</div>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.min.js"> </script>
      
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
           
</form>

      <script>

    window.onload = function () {
        var enlacesHabilitados = ["NuevaOt", "ObservacionesOt", "OtPendientes", "ActPedImp", "ImpPedAse", "ImpPedSed","CierraOt"]; // IDs de los enlaces a habilitar
        habilitarEnlaces(enlacesHabilitados);
    };

    function habilitarEnlaces(enlacesHabilitados) {
        for (var i = 0; i < enlacesHabilitados.length; i++) {
            var enlace = document.getElementById(enlacesHabilitados[i]);
            enlace.classList.add("enabled");
        }
    }

      

        function NuevaOt() {
            // Deshabilitar enlaces
            document.getElementById("NuevaOt").classList.remove("enabled");
            document.getElementById("ObservacionesOt").classList.remove("enabled");
            document.getElementById("ImpPedAse").classList.remove("enabled");
            document.getElementById("ImpPedSed").classList.remove("enabled");
            document.getElementById("OtPendientes").classList.remove("enabled");
            // Habilitar enlaces
            document.getElementById("GrabarOt").classList.add("enabled");
            document.getElementById("Cancelar").classList.add("enabled");
         
        }

        function Cancelar() {
            // Deshabilitar enlaces
            document.getElementById("Cancelar").classList.remove("enabled");
            document.getElementById("GrabarOt").classList.remove("enabled");
          
            // Habilitar enlaces
            document.getElementById("NuevaOt").classList.add("enabled");
            document.getElementById("ObservacionesOt").classList.add("enabled");
            document.getElementById("ImpPedAse").classList.add("enabled");
            document.getElementById("ImpPedSed").classList.add("enabled");
            document.getElementById("OtPendientes").classList.add("enabled");
            
        }

   
      </script>

      <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>



</body>
</html>
