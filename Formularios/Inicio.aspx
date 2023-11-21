﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Inicio.Inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link href="../Recursos/CSS/Inicio.css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SID</title>
</head>
<body>
    <form id="Form1" runat="server">


        <header style="background-color: #CCCCCC">

            <img src="https://www.ducon.com.co/images/logo_ducon.png" style="margin: 2rem" />

        </header>





        <nav class="navbar navbar-expand-lg navbar-light bg-light shadow p-3 mb-5 bg-body form-control-sm">
            <div class="container-fluid rounded-3" style="background-color: #081a2c">  
                  <a class="navbar-brand" href="#"></a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarScroll" aria-controls="navbarScroll" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>

                <div class="collapse navbar-collapse" id="navbarScroll">
                    <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll">
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle text-white" href="#" id="Departamento" role="button" data-bs-toggle="dropdown" aria-expanded="false">Departamento
                            </a>
                            <ul class="dropdown-menu" aria-labelledby="Departamento">
                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Administrativo" role="button" data-bs-toggle="dropdown" aria-expanded="false">Administrativo
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Gerencia Comercial</a></li>

                                    </ul>
                                </li>
                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Compras" role="button" data-bs-toggle="dropdown" aria-expanded="false">Compras
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Generar codigo de inventario</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Solicitud de producto especial</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Orden de abastecimiento interna</a></li>
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Dise_Desa" role="button" data-bs-toggle="dropdown" aria-expanded="false">Diseño | Desarrollo
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Estadisticas de diseño</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Estadisticas desarrollo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Generar código de inventario</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="OrdenTrabajo.aspx">Ordenes de trabajo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Bitacora renders</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Bitacora desarrollo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Bitacora diseño</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Estadisticas desarrollo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Estadisticas dibujo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Reproceso dibujo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Reproceso desarrollo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Diseño en el exterior</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Tabla de diseño</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Plano</a></li>
                                    </ul>
                                </li>
                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Fact_Cart" role="button" data-bs-toggle="dropdown" aria-expanded="false">Facturacion y cartera
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Cierre de obra</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Control de obra</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Despacho de obras</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Programación ordenes de T'S de SID</a></li>
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Gest_Cali_Adno" role="button" data-bs-toggle="dropdown" aria-expanded="false">Gestio de calidad adnom
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Acciones de mejora</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Entrega perfecta</a></li>
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Gest_Cali_Usr" role="button" data-bs-toggle="dropdown" aria-expanded="false">Gestion de calidad usr
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Accion de mejora</a></li>
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Instalacion" role="button" data-bs-toggle="dropdown" aria-expanded="false">Instalacion
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Cierre de obra</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Recepcion" role="button" data-bs-toggle="dropdown" aria-expanded="false">Recepcion
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Generar cotización</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Ingresar cotizacion</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Tabla de diseños</a></li>
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Sistemas" role="button" data-bs-toggle="dropdown" aria-expanded="false">Sistemas
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="#">Administracion</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Asignar permiso</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Configurar sede</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="#">Ventana principal</a></li>
                                    </ul>
                                </li>

                                <li class="nav-item dropend">
                                    <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Ventas" role="button" data-bs-toggle="dropdown" aria-expanded="false">Ventas
                                    </a>
                                    <ul class="dropdown-menu">
                                        <li><a class="dropdown-item form-control-sm" href="ventas/Gestion_Comercial.aspx">Gestión comecial</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="ventas/Licitaciones.aspx">Licitaciones</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="OrdenTrabajo.aspx">Ordenes de trabajo</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="ventas/Diseño_Venta.aspx">Programar diseño</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="ventas/Render_Venta.aspx">Programar render</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="ventas/Consulta_Cotizacion.aspx">Seguimiento cotizaciones</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="ventas/Solicitud_Especial.aspx">Solicitud producto especial</a></li>
                                        <li><a class="dropdown-item form-control-sm" href="ventas/Visita_Asesores.aspx">Visitas asesores</a></li>
                                    </ul>
                                </li>
                            </ul>
                        </li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Personas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Persona
                            </a>
                            <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                <li><a class="dropdown-item form-control-sm" href="#">Cliente</a></li>
                                <li><a class="dropdown-item form-control-sm" href="#">Empleado</a></li>
                            </ul>
                        </li>
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle form-control-sm" href="#" id="Consultas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Consultas
                            </a>
                            <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                <li><a class="dropdown-item form-control-sm" href="#">Reprocesos</a></li>
                                <li><a class="dropdown-item form-control-sm" href="#">Despacho de obra</a></li>
                                <li><a class="dropdown-item form-control-sm" href="OrdenTrabajo.aspx">Ordenes de trabajo</a></li>
                                <li><a class="dropdown-item form-control-sm" href="OrdenTrabajo.aspx">Programacion de OT</a></li>
                                <li><a class="dropdown-item form-control-sm" href="#">Todas las OT (Manuales /SID)</a></li>
                            </ul>
                        </li>
                    </ul>
                    <hr class="text-white-50" />
                    <asp:Label ID="lblBienvenida" runat="server" ForeColor="White"></asp:Label>
                    <asp:Button class="btn btn-light" type="button" ID="BtnCerrar" runat="server" Text="Cerrar" OnClick="BtnCerrar_Click" BackColor="#081a2c" BorderColor="#081a2c" ForeColor="White" />



                </div>
            </div>
        </nav>
    </form>
    <script>
        // Evitar el cierre del menú al hacer clic dentro del menú
        document.addEventListener("DOMContentLoaded", function () {
            var dropdownMenus = document.querySelectorAll('.dropdown-menu');

            dropdownMenus.forEach(function (menu) {
                menu.addEventListener('click', function (event) {
                    event.stopPropagation(); // Evitar la propagación del evento de clic
                });
            });
        });

    </script>


  
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
