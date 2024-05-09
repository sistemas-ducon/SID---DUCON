﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Inicio.Inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../Recursos/CSS/Inicio.css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SID</title>

     <script>
         function pageLoad(sender, args) {
             var dropdownMenus = document.querySelectorAll('.dropdown-menu');

             // Agrega un evento de clic a cada menú desplegable
             dropdownMenus.forEach(function (menu) {
                 menu.addEventListener('click', function (event) {
                     // Verifica si el elemento clickeado es un botón ASP.NET
                     if (!event.target.matches('[id^=Button]')) {
                         // Detiene la propagación del clic para evitar que el menú se cierre
                         event.stopPropagation();
                     }
                 });
             });

             // Agrega un evento de clic al documento para cerrar todos los menús desplegables cuando se hace clic fuera de ellos
             document.addEventListener("click", function () {
                 dropdownMenus.forEach(function (menu) {
                     // Cierra cada menú desplegable
                     var dropdown = new bootstrap.Dropdown(menu.closest('.dropdown'));
                     dropdown.hide();
                 });
             });
         }

     </script>



</head>
<body >
    <form id="Form1" runat="server" >
       <asp:ScriptManager runat="server" />

        <header style="background-color: #081a2c ">

           <%--<img src="https://i.ibb.co/c8cmwQ0/Actuallogo-SIDOKblanco.png" style="margin: 0rem" width="160px" height="40" />--%>

        </header>
        <asp:UpdatePanel ID="PanelModulo" runat="server">
            <ContentTemplate >
                <nav class="navbar navbar-expand-lg navbar-light bg-light shadow p-3 mb-5 bg-body form-control-sm" >
                    <div class="container-fluid rounded-3" style="background-color: #101321">
                        <a class="navbar-brand" href="#"></a>
                        <button class="navbar-toggler bg-white" type="button" data-bs-toggle="collapse" data-bs-target="#navbarScroll" aria-controls="navbarScroll" aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon form-control-sm"></span>
                        </button>
                        <img src="https://i.ibb.co/c8cmwQ0/Actuallogo-SIDOKblanco.png" style="margin: 0rem" width="160px" height="40" /> <%---Logo de la aplicacion---%>
                        <div class="collapse navbar-collapse navbar-expand" id="navbarScroll">
                            <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll">
                                <li class="nav-item dropdown">
                                   
                                    <a class="nav-link dropdown-toggle text-white" href="#" id="Departamento" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                      <%--Icono Departamento --%>   <i class="bi bi-building-fill"></i> Departamento                           
                                    </a>
                                    <ul class="dropdown-menu" aria-labelledby="Departamento">
                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Administrativo" role="button" data-bs-toggle="dropdown" aria-expanded="false"> <i class="bi bi-people-fill"></i>  <%-- Administrativo--%> Administrativo
                                            </a>
                                            <ul class="dropdown-menu">

                                                <li class="nav-item dropend">
                                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="GerenciaComercial" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-people-fill"></i> Gerencia Comercial
                                                    </a>
                                                    <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                                        <li>
                                                            <asp:LinkButton ID="Linkbutton7" runat="server" CssClass="dropdown-item form-control-sm" OnClick="GerenciaComercial_Click" CommandName="ActualizarPrecios"> <i class="bi bi-people-fill"></i> Actualizar Precios</asp:LinkButton> </li>
                                                        <li>
                                                            <asp:LinkButton ID="Linkbutton8" runat="server" CssClass="dropdown-item form-control-sm" OnClick="GerenciaComercial_Click" CommandName="EstadisticaVentas" Text="Estadistica Ventas" /></li>
                                                        <li>
                                                            <asp:LinkButton ID="Linkbutton9" runat="server" CssClass="dropdown-item form-control-sm" OnClick="GerenciaComercial_Click" CommandName="SeguimientoCotizaciones" Text="Seguimiento Cotizaciones" /></li>
                                                    </ul>
                                                </li>

                                                <%--  <li><asp:LinkButton runat="server" ID="LinkGerenciaComercial" class="dropdown-item form-control-sm" OnClick="GerenciaComercial_Click" CommandName="GerenciaComercial" Text="GestionComercial"/></li>--%>
                                            </ul>
                                        </li>
                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Compras" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-cart-check-fill"></i> <%-- Compras--%> Compras
                                            </a>
                                            <ul class="dropdown-menu">    
                                               
                                                <li><asp:LinkButton ID="Linkbutton10" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Compras_Click" CommandName="GenerarCodigoInventario" Text="Generar codigo de inventario"/></li>
                                                <li><asp:LinkButton ID="Linkbutton11" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Compras_Click" CommandName="SolicitudProductoEspecial" Text="Solicitud de producto especial"/></li>
                                                 <li><asp:LinkButton ID="Linkbutton12" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Compras_Click" CommandName="OrdenAbastecimientoInterna" Text="Orden de abastecimiento interna"/></li>
                                            </ul>
                                        </li>

                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Dise_Desa" role="button" data-bs-toggle="dropdown" aria-expanded="false"> <i class="bi bi-palette-fill"></i>   <%-- Diseño Desarrollo--%> Diseño | Desarrollo
                                            </a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton13" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Diseno_Click" CommandName="EstadisicaDiseño" Text="Estadisticas Diseño" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton14" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Diseno_Click" CommandName="GenerarCodigoInventario" Text="Generar código de inventario" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton15" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Diseno_Click" CommandName="OrdenTrabajo" Text="Ordenes de trabajo" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton16" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Diseno_Click" CommandName="BitacoraRenders" Text="Bitacora renders" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton17" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Diseno_Click" CommandName="BitacoraDesarrollo" Text="Bitacora desarrollo" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton18" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Diseno_Click" CommandName="BitacoraDiseno" Text="Bitacora diseño" /></li>     
                                            </ul>
                                        </li>

                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Dise_Exterior" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-palette"></i>  <%-- Diseño exterior--%>  Diseño Exterior
                                            </a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton24" runat="server" CssClass="dropdown-item form-control-sm" OnClick="DisenoExterior_Click" CommandName="TablaDiseno" Text="Tabla de diseño" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton25" runat="server" CssClass="dropdown-item form-control-sm" OnClick="DisenoExterior_Click" CommandName="Plano" Text="Plano" /></li>
                                            </ul>
                                        </li>

                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Fact_Cart" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-receipt"></i> <%-- Facturacion y cartera--%> Facturacion y cartera
                                            </a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton19" runat="server" CssClass="dropdown-item form-control-sm" OnClick="FacturacionCartera_Click" CommandName="CierreObra" Text="Cierre de obra" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton20" runat="server" CssClass="dropdown-item form-control-sm" OnClick="FacturacionCartera_Click" CommandName="ControlObra" Text="Control de obra" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton21" runat="server" CssClass="dropdown-item form-control-sm" OnClick="FacturacionCartera_Click" CommandName="DespachoObras" Text="Despacho de obras" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton22" runat="server" CssClass="dropdown-item form-control-sm" OnClick="FacturacionCartera_Click" CommandName="OrdenTrabajo" Text="Ordenes de trabajo" /></li>
                                                <li>
                                                    <asp:LinkButton ID="Linkbutton23" runat="server" CssClass="dropdown-item form-control-sm" OnClick="FacturacionCartera_Click" CommandName="ProgramaciónTsSID" Text="Programación Ordenes de T'S de SID" /></li>
                                            </ul>
                                        </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Gest_Cali_Adno" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-check-circle-fill"></i> <%-- Gestion de calidad admon--%> Gestio de calidad adnom
                                    </a>
                                    <ul class="dropdown-menu">
                                          <li><asp:LinkButton ID="Linkbutton26" runat="server" CssClass="dropdown-item form-control-sm" OnClick="GestionCalidadAdnom_Click" CommandName="AccionesMejora" Text="Acciones de mejora" /></li>
                                       <li><asp:LinkButton ID="Linkbutton27" runat="server" CssClass="dropdown-item form-control-sm" OnClick="GestionCalidadAdnom_Click" CommandName="EntregaPerfecta" Text="Entrega perfecta" /></li>   
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Gest_Cali_Usr" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-check2-circle"></i> <%-- Gestion de calidad --%> Gestion de calidad usr 
                                            </a>
                                            <ul class="dropdown-menu">
                                                <li><asp:LinkButton ID="Linkbutton28" runat="server" CssClass="dropdown-item form-control-sm" OnClick="GestionCalidadAdnom_Click" CommandName="AccionMejora" Text="Acción de mejora" /></li>
                                            </ul>
                                        </li>

                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Instalacion" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-wrench-adjustable"></i>  <%-- Icono Instalacion--%> Instalacion
                                            </a>
                                            <ul class="dropdown-menu">

                                                 <li><asp:LinkButton ID="Linkbutton29" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Instalacion_Click" CommandName="AccionesMejora" Text="Cierre de obra" /></li>
                                       <li><asp:LinkButton ID="Linkbutton30" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Instalacion_Click" CommandName="EntregaPerfecta" Text="Ordenes de trabajo" /></li>   
       
                                            </ul>
                                        </li>

                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Recepcion" role="button" data-bs-toggle="dropdown" aria-expanded="false"> <i class="bi bi-person-lines-fill"></i>  <%-- Icono Recepcion--%> Recepcion
                                            </a>
                                            <ul class="dropdown-menu">
                                                  <li><asp:LinkButton ID="Linkbutton31" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Recepcion_Click" CommandName="CierreObra" Text="Cierre de obra" /></li>
                                               <li><asp:LinkButton ID="Linkbutton32" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Recepcion_Click" CommandName="IngresarCotizacion" Text="Ingresar cotizacion" /></li>
                                                 <li><asp:LinkButton ID="Linkbutton33" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Recepcion_Click" CommandName="TablaDiseños" Text="Tabla de diseños" /></li>
                                            </ul>
                                        </li>

                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Sistemas" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-laptop"></i> <%-- Icono Sistemas--%> Sistemas
                                            </a>
                                            <ul class="dropdown-menu">
                                                <li><asp:LinkButton ID="Linkbutton34" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Sistemas_Click" CommandName="Administracion" Text="Administracion" /></li>
                                                 <li><asp:LinkButton ID="Linkbutton35" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Sistemas_Click" CommandName="AsignarPermiso" Text="Asignar permiso" /></li>
                                               <li><asp:LinkButton ID="Linkbutton36" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Sistemas_Click" CommandName="ConfigurarSede" Text="Configurar sede" /></li>
                                                <li><asp:LinkButton ID="Linkbutton37" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Sistemas_Click" CommandName="VentanaPrincipal" Text="Ventana principal" /></li>         
                                            </ul>
                                        </li>

                                        <li class="nav-item dropend">
                                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Ventas" role="button" data-bs-toggle="dropdown" aria-expanded="false"><i class="bi bi-cash-coin"></i> <%-- Icono Ventas--%> Ventas
                                            </a>

                                            <ul class="dropdown-menu">
                                              
                                                <li>
                                                    <asp:linkbutton ID="LinkGestionComercial" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="GestionComercial"> <i class="bi bi-receipt-cutoff"> </i> Gestion Comercial</asp:linkbutton>

                                                </li> 
                                                <li>
                                                    <asp:linkbutton ID="LinkLicitaciones" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="Licitaciones"> <i class="bi bi-bank2"></i> Licitaciones</asp:linkbutton></li>
                                                <li>
                                                    <asp:linkbutton ID="LinkOrdenTrabajo" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="OrdendeTrabajo"> <i class="bi bi-person-fill-gear"></i> Ordenes de Trabajo</asp:linkbutton></li>
                                                <li>
                                                    <asp:linkbutton ID="LinkProgramarDiseno" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="ProgramarDiseno"><i class="bi bi-file-earmark-image-fill"></i> Programar Diseño</asp:linkbutton></li>
                                                <li>
                                                    <asp:linkbutton ID="LinkProgramarRender" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="ProgramarRender"> <i class="bi bi-badge-3d-fill"></i> Programar Render </asp:linkbutton></li>
                                                <li>
                                                    <asp:linkbutton ID="LinkSeguimientoCotizacion" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="SeguimientoCotizacion"><i class="bi bi-file-earmark-spreadsheet-fill"></i> Seguimiento Cotizacion</asp:linkbutton></li>
                                                <li>
                                                    <asp:linkbutton ID="LinkSolicitudProductoEspecial" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="SolicitudProductoEspecial"><i class="bi bi-window-plus"></i>Solicitud producto especial</asp:linkbutton></li>
                                                <li>
                                                    <asp:linkbutton ID="LinkVisitaAsesores" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermiso_Ventas" CommandName="VisitaAsesores"><i class="bi bi-people-fill"></i> Visita Asesores</asp:linkbutton></li>
                                            </ul>
                                        </li>
                                    </ul>
                                </li>
                                  

                                <li class="nav-item dropdown">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Personas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF" > <i class="bi bi-person-fill-check"></i> <%-- Icono Persona--%> Persona 
                                    </a>
                                     
                                    <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                        <li><asp:linkbutton ID="Linkbutton1" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermisoCliente_Click" CommandName="PersonaCliente" Text="Cliente"/></li>
                                        <li><asp:linkbutton ID="Linkbutton2" runat="server" CssClass="dropdown-item form-control-sm" OnClick="ValidarPermisoEmpleado_Click"  CommandName="PersonaEmpleado" Text="Empleado"/></li>
                                    </ul>
                              
                                </li>
                                
                                


                                <li class="nav-item dropdown">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Consultas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF"><i class="bi bi-question-square-fill"></i>  <%-- Icono Consultas--%> Consultas
                                    </a>
                                    <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                        <li><asp:linkbutton ID="Linkbutton3" runat="server" CssClass="dropdown-item form-control-sm" OnClick="Reprocesos_Click"  CommandName="Reprocesos" Text="Reprocesos"/></li>
                                        <li><asp:linkbutton ID="Linkbutton4" runat="server" CssClass="dropdown-item form-control-sm" CommandName="ConsultaDespachoObra" Text="Despacho de Obra"/></li>

                                         <li class="nav-item dropend">
                                   <a class="nav-link dropdown-toggle form-control-sm" href="#" id="OrdenTrabajo" role="button" data-bs-toggle="dropdown" aria-expanded="false">Orden de Trabajo
                                    </a>
                                    <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                        <li><asp:linkbutton ID="Linkbutton5" runat="server" CssClass="dropdown-item form-control-sm"  CommandName="PersonaCliente" Text="Programacion de OT"/></li>
                                        <li><asp:linkbutton ID="Linkbutton6" runat="server" CssClass="dropdown-item form-control-sm"   CommandName="PersonaEmpleado" Text="Todas las OT (Manuales /SID)"/></li>
                                    </ul>
                                </li>
                     
                                    </ul>
                                </li>
                            </ul>

                            <hr class="text-white-50" />
                            <asp:Label ID="lblBienvenida" runat="server" ForeColor="White"></asp:Label>
                            <asp:Button class="btn btn-light" type="button" ID="BtnCerrar" runat="server" Text="Cerrar" OnClick="BtnCerrar_Click" BackColor="#101321" BorderColor="#101321" ForeColor="White" />

                        </div>
                    </div>
                </nav>
  
            </ContentTemplate>

        </asp:UpdatePanel>

           <div class="modal" id="miModalError">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Error</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <p>No tiene permiso para acceder a este modulo</p>
                            </div>
                            <div class="modal-footer">
                            </div>
                        </div>
                    </div>
                </div>

           <div class="modal" id="miModalPendiente">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">Pendiente</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <p>Formulario Pendiente</p>
                            </div>
                            <div class="modal-footer">
                            </div>
                        </div>
                    </div>
                </div>

    </form>






    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
