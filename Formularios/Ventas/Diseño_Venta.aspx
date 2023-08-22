<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Diseño_Venta.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Diseño_Venta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
   
      <link type="text/css" href="../../Recursos/CSS/Ventas/Diseño_Venta.css" rel="stylesheet" />
    <title>Diseño - Departamento de Ventas</title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      


        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="Diseño-BitacoraFPV-001-tab" data-bs-toggle="tab" href="#Diseño-BitacoraFPV-001-content">Diseño-Bitacora FPV-001</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Programacion-tab" data-bs-toggle="tab" href="#Programacion-content">Programación</a>
                    </li>
                       <li class="nav-item">
                        <a class="nav-link text-dark" id="Cotizacion-tab" data-bs-toggle="tab" href="#Cotizacion-content">Cotización</a>
                       </li>
                </ul>
            </div>
        </nav>

        <div class="tab-content">

            <div class="tab-pane fade show active" id="Diseño-BitacoraFPV-001-content">
                <asp:UpdatePanel runat="server" ID="UpdateDiseñoBitacora" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <nav class="navbar navbar-expand-sm navbar-light bg-light gap-2">
                                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                    <span class="navbar-toggler-icon"></span>
                                </button>
                                <div class="collapse navbar-collapse" id="ejemplo2">
                                    <ul class="navbar-nav mx-auto contenedor-icono">
                                        <div class="contenedor-icono">
                                            <button href="#" title="Nuevo diseño o bitacora" id="NuevaVisita" class="btn-outline-dark btn btn-white  <%--btn-block animate__animated animate__wobble--%>" runat="server" onclick="btnBitaco_Click">
                                                <i class="bi bi-file-earmark"></i>
                                            </button>
                                            <button href="#" title="Grabar Bitacora" id="Grabar" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-save2"></i>
                                            </button>
                                            <button href="#" title="Modificar Bitacora" id="Modificar" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-wrench"></i>
                                            </button>
                                            <button href="#" title="Documentacion bitacora" id="DocBitacora" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-pen"></i>
                                            </button>
                                            <button href="#" title="Regresar el diseño a un proceso anterior" id="RegresarDiseño" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-arrow-left"></i>
                                            </button>
                                            <button href="#" title="Adicionar elemento" id="AdicionarElemento" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-building-up"></i>
                                            </button>
                                            <button href="#" title="Actualizar Diseños" id="ActualizarDiseños" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-box-arrow-right"></i>
                                            </button>
                                            <button href="#" title="Pausar Diseño" id="PausarDiseño" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-pause-circle"></i>
                                            </button>
                                            <button href="#" title="Cancelar" id="Cancelar" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-x-square"></i>
                                            </button>
                                            <button href="#" title="Eliminar Diseño" id="EliminarDiseño" class="btn-outline-dark btn btn-white">
                                                <i class="bi bi-trash"></i>
                                            </button>
                                        </div>
                                    </ul>
                                </div>
                            </nav>
                        </div>
                      <%--  1/4--%>
                        <div class="container-fluid m-1">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3">

                                    <div class="row">
                                        <div class="col-md-2 col-12">
                                            <div class="input-group input-group-sm gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Diseño#:</asp:Label>
                                                <asp:Label runat="server" CssClass="destacado">Número</asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-6">
                                            <asp:Button runat="server" ID="BotonPuntos" CssClass="btn-outline-dark btn btn-white" Text="..." />
                                        </div>
                                        <div class="col-md-1 col-6">
                                            <asp:Label runat="server" class="col-form-label-sm">Ingreso de Diseño</asp:Label>
                                            <asp:TextBox ID="TextIngDis" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1 col-6">
                                            <asp:Label runat="server" class="col-form-label-sm">Ultima Activacion</asp:Label>
                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1 col-6">
                                            <asp:Label runat="server" class="col-form-label-sm">Entrega</asp:Label>
                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1 col-6">
                                            <asp:Label runat="server" class="col-form-label-sm">Fecha Ok Dibujo</asp:Label>
                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1 col-6">
                                            <asp:Label runat="server" class="col-form-label-sm">Zona</asp:Label>
                                            <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 col-12">
                                            <asp:Label runat="server" class="col-form-label-sm">Asesor</asp:Label>
                                            <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 col-12">
                                            <asp:Label runat="server" class="col-form-label-sm">Pre</asp:Label>
                                            <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3 col-12">
                                            <div class="input-group input-group-sm gap-2">
                                                <asp:Button runat="server" ID="Button1" CssClass="btn-outline-dark btn btn-white" Text="Cliente" />
                                                <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-12">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Proyecto</asp:Label>
                                                <asp:TextBox ID="TextBox8" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2 col-3">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Contacto</asp:Label>
                                                <asp:TextBox ID="TextBox9" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Tel</asp:Label>
                                                <asp:TextBox ID="TextBox10" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Cel</asp:Label>
                                                <asp:TextBox ID="TextBox11" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-2 col-6">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Mail</asp:Label>
                                                <asp:TextBox ID="TextBox12" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-2 col-6">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Dir</asp:Label>
                                                <asp:TextBox ID="TextBox13" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Descuento</asp:Label>
                                                <asp:TextBox ID="TextBox14" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Plano</asp:Label>
                                                <asp:TextBox ID="TextBox15" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-2 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Urgente</asp:Label>
                                                <asp:CheckBox ID="ChecUrgent" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-2 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Cotizar</asp:Label>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-2 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Mail Terminado</asp:Label>
                                                <asp:CheckBox ID="CheckBox2" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-2 col-3">
                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Ciudad proyecto</asp:Label>
                                                <asp:TextBox ID="TextBox16" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-2 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Cotiza Viá</asp:Label>
                                                <asp:CheckBox ID="CheckBox3" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3">
                                            <div class="input-group input-group-sm mt-2 gap-2">
                                                <asp:Label runat="server" class="col-form-label-sm">Cotiza Tte</asp:Label>
                                                <asp:CheckBox ID="CheckBox4" runat="server" />
                                            </div>
                                        </div>
                                        <div class="col-md-1 col-3 mt-1">
                                            <asp:Button runat="server" ID="Button2" CssClass="btn-outline-dark btn btn-white" Text="Programar" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                       <%-- 2/4--%>
                         <div class="container-fluid m-2">
                            <div class="row justify-content-center">
                                <div class="border rounded p-3">

                                    <div class="row">
                                        <div class="col-2 border">
                                            <div class="col-md-12 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="form-label" style="font-size: 16px; font-weight: bold;">Conduccion de Cables</asp:Label>
                                                    <asp:CheckBox ID="CheckBox5" runat="server" />
                                                </div>
                                            </div>
                                            <div class="row d-flex justify-content-between">
                                            <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm  gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm g-5">Piso</asp:Label>
                                                <asp:CheckBox ID="CheckBox6" runat="server" />                                                
                                            </div>
                                           </div>
                                                 <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm  gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm g-5">División</asp:Label>
                                                <asp:CheckBox ID="CheckBox9" runat="server" />                                                
                                            </div>
                                           </div>
                                        </div>
                                             <div class="row d-flex justify-content-between">
                                            <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Cielo</asp:Label>
                                                <asp:CheckBox ID="CheckBox7" runat="server" />
                                            </div>
                                        </div>
                                                  <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Canaleta</asp:Label>
                                                <asp:CheckBox ID="CheckBox11" runat="server" />
                                            </div>
                                        </div>
                                                 </div>
                                            <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Bte.Elec</asp:Label>
                                                <asp:CheckBox ID="CheckBox8" runat="server" />
                                            </div>
                                        </div>
                                             <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Bte Sw</asp:Label>
                                                <asp:CheckBox ID="CheckBox10" runat="server" />
                                            </div>
                                        </div>

                                        </div>
                                        <div class="col-2 border">
                                            <div class="col-md-12 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="form-label"  style="font-size: 16px; font-weight: bold;">Sujeción PT</asp:Label>
                                                    <asp:CheckBox ID="CheckBox12" runat="server" />
                                                </div>
                                            </div>
                                             <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Al Cielo</asp:Label>
                                                    <asp:CheckBox ID="CheckBox13" runat="server" />
                                                </div>
                                            </div>
                                             <div class="col-md-12 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Perfil Refuerzo</asp:Label>
                                                    <asp:CheckBox ID="CheckBox14" runat="server" />
                                                </div>
                                            </div>
                                             <div class="col-md-12 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Guarda Escobas</asp:Label>
                                                    <asp:CheckBox ID="CheckBox15" runat="server" />
                                                </div>
                                            </div>
                                             <div class="col-md-12 col-12">
                                                <div class="input-group input-group-sm p-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">H.Total(Cms)</asp:Label>
                                                    <asp:TextBox ID="TextBox17" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6 border">
                                            <div class="row d-flex justify-content-between">
                                             <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="form-label col-12 text-dark text-uppercase" style="font-size: 16px; font-weight: bold;">Especificaciones y Materiales</asp:Label>                                                  
                                                </div>
                                            </div>
                                             <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Linea</asp:Label>
                                                    <asp:TextBox ID="TextBox18" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Mostrador</asp:Label>
                                                    <asp:TextBox ID="TextBox19" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                                </div>
                                             <div class="row d-flex justify-content-between">
                                                  <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Superficies</asp:Label>
                                                    <asp:TextBox ID="TextBox20" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                                   <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-1">
                                                    <asp:Label runat="server" class="col-form-label-sm">Balance</asp:Label>
                                                    <asp:CheckBox ID="CheckBox16" runat="server" CssClass="form-check" />
                                                </div>
                                            </div>
                                                  <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class=" col-form-label-sm">Soporte</asp:Label>
                                                    <asp:TextBox ID="TextBox21" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                                  <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Gaveta</asp:Label>
                                                    <asp:TextBox ID="TextBox22" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                             </div>
                                             <div class="row d-flex justify-content-between">
                                                  <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Paneles</asp:Label>
                                                    <asp:TextBox ID="TextBox23" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                                  <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">T.Piernas</asp:Label>
                                                    <asp:TextBox ID="TextBox24" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                                 <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Repisa</asp:Label>
                                                    <asp:TextBox ID="TextBox25" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                             </div>
                                             <div class="row d-flex justify-content-between">
                                                  <div class="col-md-6 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Tipo Vidrio</asp:Label>
                                                    <asp:TextBox ID="TextBox26" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                                  <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Pantallas</asp:Label>
                                                    <asp:TextBox ID="TextBox27" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                                  <div class="col-md-3 col-6">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Arch</asp:Label>
                                                    <asp:TextBox ID="TextBox28" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                             </div>
                                        </div>
                                        <div class="col-2 border">
                                              <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="form-label" style="font-size: 16px; font-weight: bold;">Muebles</asp:Label>
                                                    <asp:CheckBox ID="CheckBox17" runat="server" />
                                                </div>
                                            </div>
                                             <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Coco</asp:Label>
                                                    <asp:TextBox ID="TextBox29" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Entrepaño</asp:Label>
                                                    <asp:TextBox ID="TextBox30" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm mt-1 gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Puertas</asp:Label>
                                                    <asp:TextBox ID="TextBox31" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                            </div>
                            </div>
                         </div>
                      <%--  3/4--%>
                        <div class="container-fluid m-2">
                            <div class="row justify-content-center">
                                <div class="border rounded p-1">
                                    <div class="row">
                                     <div class="col-4">   
                                         <h6>Observaciones Ventas</h6>
                                        <textarea id="TextObsVen" class="form-control" style="height:100px"></textarea>
                                     </div>
                                          <div class="col-4">
                                              <h6>Observaciones de Dibujo y Despiece</h6>
                                              <textarea id="TextObsDibDes" class="form-control" style="height:100px"></textarea>
                                          </div>
                                        <div class="col-4">
                                            <h6>Seguimiento de Pausas y Devoluciones</h6>
                                            <textarea id="TextSegPauDev" class="form-control" style="height: 100px"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                       <%-- 4/4--%>
                        <div class="container-fluid m-2">
                            <div class="row justify-content-center">
                                <div class="border rounded p-1">
                                     <div class="row">
                                        <div class="col-2">
                                            <div class="border rounded p-1" style="height:250px">
                                                <h6>ShowCase</h6>
                                                 <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox18" runat="server" />
                                                    <asp:Label runat="server" class="col-form-label-sm">Presentación PPT</asp:Label>                                                    
                                                </div>
                                            </div>
                                                <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox19" runat="server" />
                                                    <asp:Label runat="server" class="col-form-label-sm">Imágenes</asp:Label>                                                    
                                                </div>
                                            </div>
                                                 <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox20" runat="server" />
                                                    <asp:Label runat="server" class="col-form-label-sm">Accesorios</asp:Label>                                                    
                                                </div>
                                            </div>
                                                  <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox21" runat="server" />
                                                    <asp:Label runat="server" class="col-form-label-sm">Timpo Real</asp:Label>                                                    
                                                </div>
                                            </div>
                                                
                                                <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                 <asp:TextBox ID="TextBox32" runat="server" CssClass="form-control-sm form-control" type="Date"></asp:TextBox>
                                                 <asp:TextBox ID="TextBox33" runat="server" CssClass="form-control-sm form-control" type="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                                 <div class="col-md-10 col-12">
                                                     <asp:Label runat="server" class="col-form-label-sm">Ubicación</asp:Label>
                                                     <asp:TextBox ID="TextBox34" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                 </div>
                                            </div>
                                        </div>
                                         <div class="col-10">
                                             <div class="container-fluid">
                                                 <div class="row justify-content-center">

                                                     

                                                     <div class="border rounded p-1" style="height: 250px">
                                                     </div>
                                                 </div>
                                             </div>
                                         </div>
                                     </div>
                                </div>
                            </div>
                        </div>
                        <%--BOTONES--%>
                        <div class="container-fluid">
                            <button href="#" title="Nuevo diseño o bitacora" id="Button3" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                <i class="bi bi-currency-dollar"></i>
                            </button>
                         <button href="#" title="Nuevo diseño o bitacora" id="Button4" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                              <button href="#" title="Nuevo diseño o bitacora" id="Button5" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                              <button href="#" title="Nuevo diseño o bitacora" id="Button6" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                              <button href="#" title="Nuevo diseño o bitacora" id="Button7" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                        </div>
                       

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Programacion-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container-fluid m-2">
                            <div class="row justify-content-center">
                                <div class="border rounded p-1 special-border col-11" style="height: auto; min-height: 880px;">

                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>

                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-hover" ID="DataGrid1" runat="server" DataSourceID="SqlDataSource1"
                                                                AutoGenerateColumns="false" OnItemDataBound="DataGrid1_ItemDataBound">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Nombre de la Obra" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Nombre_Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Entrega_Dibujo_Despiece" HeaderText="F.Ingreso" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="Nueva Columna">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToDateTime(Eval("Fecha_Entrega_Dibujo_Despiece")).AddDays(2).ToString("dd/MM/yyyy hh:mm:ss tt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="RealizadoPor" HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Despacho_Produccion" HeaderText="F.Despacho" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="sp_ProBitacoraOTs" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="TextBox5" PropertyName="Text" Name="Nombre_Asesor" Type="String"></asp:ControlParameter>

                                                                </SelectParameters>
                                                            </asp:SqlDataSource>

                                                        </div>

                                            </div>
                                        </div>
                                        </div>
                                   </div>
                                        <div class="col-2">
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:Button ID="Button8" runat="server" Text="Button" CssClass="btn-outline-dark btn btn-white btn-sm btn-block animate__animated animate__tada" />
                                                </div>
                                                <div class="row">
                                                    <div class="col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label runat="server" class="col-form-label-sm">Zona</asp:Label>
                                                            <asp:TextBox ID="TextBox36" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:CheckBox ID="CheckBox22" runat="server" OnCheckedChanged="CheckBox22_CheckedChanged" AutoPostBack="true" />
                                                                <asp:Label runat="server" CssClass="col-form-label-sm">Ver Convernciones</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>                                                

                                                    <div id="modal" class="modal fade" tabindex="-1" role="dialog">
                                                        <div class="modal-dialog modal-dialog-centered" role="document">
                                                            <div class="modal-content">
                                                                <div class="modal-header">
                                                                </div>
                                                            <div class="modal-body">

                                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                                    <div class="input-group bg-success-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="noproVen" class="form-label">No prog por Ventas</label>
                                                                </div>

                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group bg-white-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="NocumenEsp" class="form-label">No cumplidos y en espera</label>
                                                                </div>

                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                        <div class="input-group bg-warning-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                        <label for="Pendiente" class="form-label">Pendientes</label>
                                                                    </div>

                                                                    <div class="input-group input-group-sm mb-1 gap-2">
                                                                        <div class="input-group bg-danger-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                        <label for="urgente" class="form-label">Urgente</label>
                                                                    </div>
                                                                 <div class="input-group input-group-sm mb-1 gap-2">
                                                                        <div class="input-group bg-penAprCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                        <label for="penAprCot" class="form-label">Pendientes por Aprobacion para Cotizar</label>
                                                                    </div>
                                                                   <div class="input-group input-group-sm mb-1 gap-2">
                                                                        <div class="input-group bg-penCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                        <label for="penCot" class="form-label">Pendientes por Cotizacion</label>
                                                                    </div>
                                                                 <div class="input-group input-group-sm mb-1 gap-2">
                                                                        <div class="input-group bg-pausados-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                        <label for="pausados" class="form-label">Pausados</label>
                                                                    </div>
                                                            </div>
                                                          
                                                        </div>
                                                    </div>
                                                </div>
                                              

                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox23" runat="server" />
                                                            <asp:Label runat="server" class="col-form-label-sm">Resumen Dibujante</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button9" runat="server" Text="Trabajar Pedido" CssClass="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__flash" OnClick="Button9_Click" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button10" runat="server" Text="Trabajar Pedido" CssClass="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" OnClick="Button10_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>


                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                          <asp:DataGrid CssClass="table table-bordered table-hover" ID="DataGrid2" runat="server" DataSourceID="DataGridDiseño" AutoGenerateColumns="false"
                                                              OnItemDataBound="DataGrid2_ItemDataBound">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Ult.Act" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="RealizadoPor" HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="PactodeEntrega" HeaderText="Pacto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                   <asp:BoundColumn DataField="ProgramadoVentas" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="PasarACotizar" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                      <asp:BoundColumn DataField="TerminadoDibujo" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Pausado" HeaderText="Pausado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridDiseño" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="sp_ProBitacoraDise" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="TextBox5" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Label runat="server" class="col-form-label-sm">Pacto de entrega</asp:Label>
                                                    <asp:TextBox ID="TextBox37" runat="server" CssClass="form-control-sm form-control" type="Date"></asp:TextBox>
                                                </div>
                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button11" runat="server" Text="Trabajar Diseño" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Button ID="Button14" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                             <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="table table-bordered table-hover" ID="DataGridDiseños" runat="server"
                                                            DataSourceID="DataGridDiseñosPorFecha" AutoGenerateColumns="false" OnItemDataBound="DataGrid3_ItemDataBound">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                 <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Descripcion-ShowCase" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                                                  
                                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Fecha" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Hora" HeaderText="Hora" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="RealizadoPor" HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Ubicacion" HeaderText="Ubicación" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Imagenes" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Terminado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                        <asp:SqlDataSource runat="server" ID="DataGridDiseñosPorFecha" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ProBitacoraShowCase" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="TextBox5" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Button ID="Button13" runat="server" Text="Trabajar ShowCase" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Button ID="Button12" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                         <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="table table-bordered table-hover" ID="DataGridRender" runat="server"
                                                            DataSourceID="DataGridRenderPorFechaYAsesor" AutoGenerateColumns="false" OnItemDataBound="DataGrid4_ItemDataBound">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                 <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Id_Render" HeaderText="ID" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Nombre-Render" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                                                                                                                          <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Activado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="TerminadoRender" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                        <asp:SqlDataSource runat="server" ID="DataGridRenderPorFechaYAsesor" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ProBitacoraRender" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="TextBox5" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                             </div>
                                                    </div>
                                        </div>
                                    </div>
                                   </div>
                                         <div class="col-2">
                                             <div class="row">
                                                 <div class="col-12">
                                                             <asp:Button ID="Button15" runat="server" Text="Trabajar Render"  class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse"/>
                                                     </div>
                                                 </div>    
                                             <div class="row">
                                                     <div class="col-12">
                                                             <asp:Button ID="Button16" runat="server" Text="Desprogramar"  class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse"/>
                                                     </div>
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
    

     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
