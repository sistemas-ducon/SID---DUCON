<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Render_Venta.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Render_Venta" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>Render Departamento Ventas</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/Render_Venta.css" />
    <script>
         function confirmProgramarRender(event) {

             var IdRender = document.getElementById("NumeroRender").innerHTML;
             var Nombre = document.getElementById("tbCliente").value;
             var mensaje = "Una vez aprobado el Render no podrá modificarlo. Esta seguro de Terminar el render: " + IdRender + " " + Nombre + " ?";

             var result = confirm(mensaje);
             if (result) {
                 // Llamar al evento del botón de eliminar en el servidor
                 $(event.target).removeAttr('onclick');
                 $(event.target).click();
             }
             return false; // Previene que el evento del botón se ejecute dos veces
         }
    </script>
</head>


<body>
    <form id="formRenderVenta" runat="server">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center ">
                <ul class="nav nav-tabs gap-5" id="miPestañas">


                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="Render-tab" data-bs-toggle="tab" href="#Render-Content">Programar Render</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="BuscarRender-tab" data-bs-toggle="tab" href="#BuscarRender-Content">Buscar Render</a>
                    </li>
                </ul>
            </div>
        </nav>

        <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
            <div class="container-fluid">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="ejemplo2">
                    <ul class="navbar-nav mx-auto contenedor-icono">

                        <div class="contenedor-icono">

                            <%--Comienza Nueva OT--%>



                            <a class="icong disabled" href="#" title="Nuevo Render" id="NuevoRender" onclick="NuevoRender()">
                                <i class="bi bi-file-earmark"></i>
                            </a>

                            <asp:LinkButton class="icong disabled" runat="server" title="Guardar Render" ID="GrabarRender" OnClick="GuardarModificarRender" OnClientClick="return validarFormularioRender();">
                                        <i class="bi bi-save2"></i>
                            </asp:LinkButton>

                            <a class="icong disabled" href="#" title="Modificar Render" id="ModificarRender" onclick="ModificarRender()">
                                <i class="bi bi-wrench"></i>
                            </a>


                            <asp:LinkButton class="icong disabled" title="Regresar el Render a un Proceso Anterior" ID="DevolverRender" runat="server">
                                  <i class="bi bi-skip-backward-circle"></i>
                            </asp:LinkButton>


                            <a class="icong disabled " href="#" title="Actualizar Render" id="Actualizar">
                                <i class="bi bi-arrow-clockwise"></i>
                            </a>

                            <asp:LinkButton class="icong disabled" title="Pausar Render" ID="PausarRender" runat="server">
                                <i class="bi bi-pause-circle"></i>
                            </asp:LinkButton>

                            <a class="icong disabled " href="#" title="Importar Render" id="ImportarRender">
                                <i class="bi bi-arrow-bar-down"></i>
                            </a>

                            <a class="icong disabled " href="#" title="Cancelar" id="CancelarRender" onclick="Cancelar()">
                                <i class="bi bi-x-lg"></i>
                            </a>

                            <asp:LinkButton class="icong disabled" title="Eliminar Render " ID="EliminarRender" runat="server">
                                  <i class="bi bi-trash"></i>
                            </asp:LinkButton>


                            <ul />
                    </ul>

                        <span id="ErrorValidacionRender" style="color: red;"></span>
                </div>

            </div>
        </nav>


        <div class="tab-content">

            <div class="tab-pane fade show active" id="Render-Content">
                <asp:UpdatePanel ID="PanelRender" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container-fluid Principal mb-2   ">

                            <div class=" container-fluid rounded border gap-2  ">

                                <div class="row pt-1 mt-1 pb-1 mb-1">
                                    <div class="col-5">
                                        <div class="input-group input-group-sm  mb-2 gap-4 ">
                                            <asp:Label ID="lbIngreso" class="form-label" Text="Ingreso" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbIngreso" type="date" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbIngresoServidor" type="date" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4 justify-content-center">
                                            <asp:Label ID="lbRender" CssClass="NumeroRender" Text="Render #" runat="server"></asp:Label>
                                            <asp:Label ID="NumeroRender" CssClass="NumeroRender" Text="Numero" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row pb-1 mb-1">
                                    <div class="col-3">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbUltActiv" class="form-label" Text="U.Activ" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbUltActiv" type="date" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbUltActivServidor" type="date" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-3">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbEntrega" class="form-label" Text="Entrega" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbEntrega" type="date" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbEntregaServidor" type="date" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-3">
                                        <div class="input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbFechaOk" class="form-label" Text="Fecha OK" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbFechaOk" type="date" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbFechaOkServidor" type="date" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-3">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbDiseño" class="form-label" Text="Diseño" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbDiseño" type="text" class="form-control" runat="server" disabled="disabled" EnableViewState="true" ></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">
                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCliente" class="form-label" Text="Cliente" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCliente" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbAsesor" class="form-label" Text="Asesor" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlAsesor" runat="server" OnDataBound="ddlAsesores_DataBound"></asp:DropDownList>
                                        </div>
                                    </div>



                                </div>

                                <div class="row pb-1 mb-1">
                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbProyecto" class="form-label" Text="Proyecto" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbProyecto" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbContacto" class="form-label" Text="Contacto" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbContacto" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>



                                </div>

                                <div class="row pb-1 mb-1">
                                    <div class="col-4">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCelular" class="form-label" Text="Celular" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCelular" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-4">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbMail" class="form-label" Text="Mail" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbMail" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-4">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbTelefono" class="form-label" Text="Teléfono" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbTelefono" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>



                                </div>

                                <div class="row pb-1 mb-1">
                                    <div class="col-5">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPlano" class="form-label" Text="Plano" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPlano" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:Button class="btn btn-outline-secondary" ID="btnPlano" type="button" Text="..." runat="server" Enabled="false"></asp:Button>
                                        </div>
                                    </div>
                                    <div class="col-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbZona" class="form-label" Text="Zona" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlZona" runat="server">
                                                <asp:ListItem Value="01">01</asp:ListItem>
                                                <asp:ListItem Value="02">02</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-4">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbImagenes" class="form-label" Text="Imágenes" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbImagenes" type="Number" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>



                                </div>

                                <div class="row pb-1">

                                    <div class="col-6">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbAreasRenderizar" class="form-label" Text="Áreas a Renderizar." runat="server"></asp:Label>
                                            <textarea class="form-control form-control-sm" id="txAreaRender" runat="server" cols="29" rows="4" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbObsVentas" class="form-label" Text="Observación Ventas." runat="server"></asp:Label>
                                            <textarea class="form-control form-control-sm" id="txObsVentas" runat="server" cols="29" rows="4" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1">

                                    <div class="col-6">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbSegPausas" class="form-label" Text="Seguimiento de Pausas." runat="server"></asp:Label>
                                            <textarea class="form-control form-control-sm" id="txSegPausas" runat="server" cols="29" rows="4" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-6">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbObsDibujo" class="form-label" Text="Observación Dibujo." runat="server"></asp:Label>
                                            <textarea class="form-control form-control-sm" id="txObsDibujo" runat="server" cols="29" rows="4" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class=" container-fluid border rounded">
                                <h5 class="text-sm-start">Acabados</h5>
                                <asp:TextBox ID="tbTerminadoVentas" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>

                                <div class="row pt-1 mt-1 pb-1 mb-1">

                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lblinea" class="form-label" Text="Línea" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbLinea" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbSup" class="form-label" Text="Sup" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbSup" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbAcc" class="form-label" Text="Acc" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbAcc" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCantos" class="form-label" Text="Cantos" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCantos" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPerfil" class="form-label" Text="Perfil" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPerfil" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPaneles" class="form-label" Text="Paneles" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPaneles" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbArch" class="form-label" Text="Arch" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbArch" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbSillas" class="form-label" Text="Sillas" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbSillas" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>


                                <div class="row pb-1 mb-1">

                                    <div class="col-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPantallas" class="form-label" Text="Pantallas" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPantallas" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbEspArq" class="form-label" Text="Espacio Arquitectónico" runat="server"></asp:Label>
                                            <asp:CheckBox ID="chxEspArq" runat="server" Enabled="false" />
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class=" input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbMuebles" class="form-label" Text="Muebles" runat="server"></asp:Label>
                                                    <textarea class="form-control form-control-sm" id="txMuebles" runat="server" cols="29" rows="7" disabled="disabled"></textarea>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row pt-1 mt-1 pb-1 mb-1">
                                            <div class="col-6">
                                                <div class=" input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAmbientacion" class="form-label" Text="Ambientación" runat="server"></asp:Label>
                                                    <asp:CheckBox ID="chxAmbientacion" runat="server" Enabled="false" />
                                                </div>
                                            </div>

                                            <div class="col-6">
                                                <div class=" input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAnimacion" class="form-label" Text="Animación" runat="server"></asp:Label>
                                                    <asp:CheckBox ID="chxAnimacion" runat="server" Enabled="false" />
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="col-6">

                                        <div class="row">
                                            <div class="col-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAcaPisoYZocalo" class="form-label" Text="Acabados Piso y Zócalo" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbAcaPisZoc" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAcaMuros" class="form-label" Text="Acabados Muros" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbAcaMuros" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbIluminacion" class="form-label" Text="Iluminación y  Tipos de lámparas" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbIluminacion" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAntepecho" class="form-label" Text="Sillar o antepecho y ventanas" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbAntepecho" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-3">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:CheckBox ID="chxConvenciones" runat="server" OnCheckedChanged="chxConvenciones_CheckedChanged" AutoPostBack="true" />
                                            <asp:Label ID="lbConenciones" class="form-label" Text="Ver Convenciones" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbZona2" class="form-label" Text="Zona" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlZona2" runat="server" OnSelectedIndexChanged="RenderPorZonaX" AutoPostBack="true" DataTextField="Zona" DataValueField="Zona" DataSourceID="Zonas" OnDataBound="ddlZona2_DataBound">
                                            </asp:DropDownList><asp:SqlDataSource runat="server" ID="Zonas" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select Zona from tblRender group by Zona"></asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="col-3 ">
                                        <div class=" input-group input-group-sm  mb-2 gap-2 justify-content-center ">
                                            <asp:Button class="btn btn-outline-secondary" ID="btnTrabajarRender" runat="server" Text="Trabajar Render" />
                                        </div>
                                    </div>

                                    <div class="col-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-2 justify-content-center">
                                            <asp:Button class="btn btn-warning  " ID="btnProgramarRender" runat="server" Text="Programar" OnClick="ProgramarRender" OnClientClick="return confirmProgramarRender(event);" />
                                        </div>
                                    </div>

                                </div>

                                <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                    <div class="modal-dialog  ">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">Convenciones</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row justify-content-center mb-3">
                                                    <div class="border rounded p-2">
                                                        <div class="row">
                                                            <div class="col-12">
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: mediumpurple; white-space: nowrap"></div>
                                                                    <label for="lbNoProgamado" class="form-label">Render No programado por Ventas</label>
                                                                </div>

                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: aqua"></div>
                                                                    <label for="lbPausado" class="form-label">Render Pausado</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group" style="width: 20px; height: 20px; border: 1px; background-color: red"></div>
                                                                    <label for="lbEspera" class="form-label">No cumplidos y en espera</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: yellow"></div>
                                                                    <label for="lbNormal" class="form-label">Programación Normal</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: lawngreen"></div>
                                                                    <label for="lbTerminado" class="form-label">Render Terminados 100%</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>



                                            </div>

                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                            </div>

                                        </div>
                                    </div>
                                </div>


                            </div>


                        </div>

                        <div class="container-fluid Bajo">
                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-start">Programacion</h5>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGridRenders" runat="server" DataSourceID="CargarRenders" AutoGenerateColumns="false" OnItemDataBound="DataGridRenders_ItemDataBound" OnItemCommand="DataGridRenders_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerRenders" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="Turno" HeaderText="Turno" />
                                                        <%-- Turno[1]--%>
                                                        <asp:BoundColumn DataField="Id_Render" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [2]--%>
                                                        <asp:BoundColumn DataField="Nombre" HeaderText="Nombre-Render" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [3]--%>
                                                        <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Activado" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [4]--%>
                                                        <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="Entregado" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [5]--%>
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [6]--%>
                                                        <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [7]--%>
                                                        <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [8]--%>
                                                        <asp:BoundColumn DataField="FechaRenderOk" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [9]--%>
                                                        <asp:BoundColumn DataField="Numero_Diseño" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [10]--%>
                                                        <asp:BoundColumn DataField="Nombre_Render" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [11]--%>
                                                        <asp:BoundColumn DataField="Contacto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [12]--%>
                                                        <asp:BoundColumn DataField="Fecha_Ingreso" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [13]--%>
                                                        <asp:BoundColumn DataField="Celular" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [14]--%>
                                                        <asp:BoundColumn DataField="Mail" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [15]--%>
                                                        <asp:BoundColumn DataField="Telefono" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [16]--%>
                                                        <asp:BoundColumn DataField="Plano" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [17]--%>
                                                        <asp:BoundColumn DataField="Imagenes" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [18]--%>
                                                        <asp:BoundColumn DataField="Areas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [19]--%>
                                                        <asp:BoundColumn DataField="Observaciones_Ventas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [20]--%>
                                                        <asp:BoundColumn DataField="SeguimientoPausa" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [21]--%>
                                                        <asp:BoundColumn DataField="Observacion_Dibujo" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [22]--%>
                                                        <asp:BoundColumn DataField="Linea" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [23]--%>
                                                        <asp:BoundColumn DataField="AcabadoSuperficie" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [24]--%>
                                                        <asp:BoundColumn DataField="AcabadoAccesorios" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [25]--%>
                                                        <asp:BoundColumn DataField="Cantos" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [26]--%>
                                                        <asp:BoundColumn DataField="AcabadoPerfileria" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [27]--%>
                                                        <asp:BoundColumn DataField="AcabadoPaneles" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [28]--%>
                                                        <asp:BoundColumn DataField="Archivadores" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [29]--%>
                                                        <asp:BoundColumn DataField="Sillas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [30]--%>
                                                        <asp:BoundColumn DataField="Pantallas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [31]--%>
                                                        <asp:BoundColumn DataField="EspacioArquitectonico" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [32]--%>
                                                        <asp:BoundColumn DataField="Muebles" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [33]--%>
                                                        <asp:BoundColumn DataField="PisoyZocalo" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [34]--%>
                                                        <asp:BoundColumn DataField="Muros" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [35]--%>
                                                        <asp:BoundColumn DataField="Iluminacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [36]--%>
                                                        <asp:BoundColumn DataField="Sillar" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [37]--%>
                                                        <asp:BoundColumn DataField="Ambientacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [38]--%>
                                                        <asp:BoundColumn DataField="Animacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [39]--%>
                                                        <asp:BoundColumn DataField="ProgramadoVentas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [40]--%>
                                                        <asp:BoundColumn DataField="Pausado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [41]--%>
                                                        <asp:BoundColumn DataField="TerminadoRender" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [42]--%>
                                                         <asp:BoundColumn DataField="Cliente" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [43]--%>
                                                    </Columns>
                                                </asp:DataGrid>

                                                <asp:SqlDataSource runat="server" ID="CargarRenders" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT   ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno, Cliente +'-'+ Nombre_Render AS Nombre,    * FROM    tblRender WHERE     TerminadoRender = 0  "></asp:SqlDataSource>
                                                <asp:SqlDataSource ID="RenderPorZona" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select  ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre, * from tblRender where Zona=@Parametro and TerminadoRender = 0 ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlZona2" PropertyName="SelectedValue" Name="Parametro"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>




                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade  " id="BuscarRender-Content">
                <asp:UpdatePanel ID="PanelBuscarRender" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <div class="row pt-2">
                                <div class="col-8">
                                    <div class="input-group input-group-sm  mb-2 gap-3">
                                        <asp:Label ID="lbFechaIngreso" class="form-label" Text="Fecha Ingreso" runat="server"></asp:Label>
                                        <asp:TextBox ID="FechaIni" type="date" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label ID="Label2" class="form-label" Text=" Y " runat="server"></asp:Label>
                                        <asp:TextBox ID="FechaFin" type="date" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Button ID="btnConsultar" type="button" Text="Consultar" class="btn btn-outline-secondary" runat="server" OnClick="ConsultarRender"></asp:Button>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbClienteX" class="form-label" Text="Cliente" runat="server"></asp:Label>

                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">

                                        <asp:TextBox ID="tbClienteX" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbProyectoX" class="form-label" Text="Proyecto" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:TextBox ID="tbProyectoX" type="text" class="form-control " runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbNumeroRender" class="form-label" Text="Render N." runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:TextBox ID="tbNumeroRender" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row justify-content-center pt-3">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 30rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Render Filtrados</h5>

                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="BuscarRender" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridBuscarRender_ItemDataBound" OnItemCommand="DataGridBuscarRenders_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerRenders2" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" 
                                                                     OnClientClick="activarTab('Render-Content');"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="Turno" HeaderText="Turno" />
                                                        <%-- Turno[1]--%>
                                                        <asp:BoundColumn DataField="Id_Render" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [2]--%>
                                                        <asp:BoundColumn DataField="Nombre" HeaderText="Nombre-Render" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [3]--%>
                                                        <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Activado" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [4]--%>
                                                        <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="Entregado" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [5]--%>
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [6]--%>
                                                        <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [7]--%>
                                                        <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [8]--%>
                                                        <asp:BoundColumn DataField="FechaRenderOk" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [9]--%>
                                                        <asp:BoundColumn DataField="Numero_Diseño" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [10]--%>
                                                        <asp:BoundColumn DataField="Nombre_Render" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [11]--%>
                                                        <asp:BoundColumn DataField="Contacto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [12]--%>
                                                        <asp:BoundColumn DataField="Fecha_Ingreso" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [13]--%>
                                                        <asp:BoundColumn DataField="Celular" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [14]--%>
                                                        <asp:BoundColumn DataField="Mail" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [15]--%>
                                                        <asp:BoundColumn DataField="Telefono" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [16]--%>
                                                        <asp:BoundColumn DataField="Plano" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [17]--%>
                                                        <asp:BoundColumn DataField="Imagenes" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [18]--%>
                                                        <asp:BoundColumn DataField="Areas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [19]--%>
                                                        <asp:BoundColumn DataField="Observaciones_Ventas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [20]--%>
                                                        <asp:BoundColumn DataField="SeguimientoPausa" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [21]--%>
                                                        <asp:BoundColumn DataField="Observacion_Dibujo" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [22]--%>
                                                        <asp:BoundColumn DataField="Linea" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [23]--%>
                                                        <asp:BoundColumn DataField="AcabadoSuperficie" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [24]--%>
                                                        <asp:BoundColumn DataField="AcabadoAccesorios" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [25]--%>
                                                        <asp:BoundColumn DataField="Cantos" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [26]--%>
                                                        <asp:BoundColumn DataField="AcabadoPerfileria" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [27]--%>
                                                        <asp:BoundColumn DataField="AcabadoPaneles" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [28]--%>
                                                        <asp:BoundColumn DataField="Archivadores" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [29]--%>
                                                        <asp:BoundColumn DataField="Sillas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [30]--%>
                                                        <asp:BoundColumn DataField="Pantallas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [31]--%>
                                                        <asp:BoundColumn DataField="EspacioArquitectonico" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [32]--%>
                                                        <asp:BoundColumn DataField="Muebles" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [33]--%>
                                                        <asp:BoundColumn DataField="PisoyZocalo" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [34]--%>
                                                        <asp:BoundColumn DataField="Muros" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [35]--%>
                                                        <asp:BoundColumn DataField="Iluminacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [36]--%>
                                                        <asp:BoundColumn DataField="Sillar" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [37]--%>
                                                        <asp:BoundColumn DataField="Ambientacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [38]--%>
                                                        <asp:BoundColumn DataField="Animacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [39]--%>
                                                        <asp:BoundColumn DataField="ProgramadoVentas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [40]--%>
                                                        <asp:BoundColumn DataField="Pausado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [41]--%>
                                                        <asp:BoundColumn DataField="TerminadoRender" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [42]--%>
                                                           <asp:BoundColumn DataField="Cliente" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- [43]--%>
                                                    </Columns>

                                                </asp:DataGrid>

                                                <asp:SqlDataSource ID="RenderFecha" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,  * from tblRender where Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC  ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="FechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="RenderCliente" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT  ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,   * FROM tblRender WHERE Cliente LIKE '%' + @NombreCliente + '%' AND Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC ">
                                                    <SelectParameters>
                                                         <asp:ControlParameter ControlID="FechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbClienteX" PropertyName="Text" Name="NombreCliente"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="RenderNombreRender" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT  ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,  * FROM tblRender WHERE Nombre_Render LIKE '%' + @NombreRender + '%' AND Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC ">
                                                    <SelectParameters>
                                                          <asp:ControlParameter ControlID="FechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbProyectoX" PropertyName="Text" Name="NombreRender"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="RenderXIdRender" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select  ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,  * from tblRender where Id_Render = @IdRender AND Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC ">
                                                    <SelectParameters>
                                                         <asp:ControlParameter ControlID="FechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbNumeroRender" PropertyName="Text" Name="IdRender"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

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

    <script>

       // se Habilitan enlaces 
        document.getElementById("NuevoRender").classList.add("enabled");        
        document.getElementById("ImportarRender").classList.add("enabled");      
        document.getElementById("CancelarRender").classList.add("enabled");

        // Habilitar o deshabilitar los DropDownList
        var dropDownLists = document.querySelectorAll("select");
        for (var j = 0; j < dropDownLists.length; j++) {

            if (dropDownLists[j].id != "ddlZona2") {
                dropDownLists[j].disabled = true;
                dropDownLists[j].value = "";
            }
        }
    </script>

    <script>

        // Ocultar el div con clase "contenedor-icono" cuando se activa la pestaña "Info-content" 
        $(document).ready(function () {
            $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var targetTab = $(e.target).attr("href");
                if (targetTab === "#BuscarRender-Content") {
                    $(".contenedor-icono").hide();
                } else {
                    $(".contenedor-icono").show();
                }
            });
        });
    </script>

    <script>
       

        //Funcion para habilitar Modificar Cuando dan Click en linkButton Del DataGrid 
        function HabilitarEnlaces1() {

            // Habilitar enlaces
            document.getElementById("ModificarRender").classList.add("enabled");

            // Si la pagina de Render Es para el Area de Dibujo , Se habilita Devolver, Pausar y Eliminar Render
            /* 
             document.getElementById("DevolverRender").classList.add("enabled");
             document.getElementById("PausarRender").classList.add("enabled");
             document.getElementById("EliminarRender").classList.add("enabled");
             */

        }

        function NuevoRender() {


            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {
                dropDownLists[j].disabled = false;
            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {

                if (textAreas[k].id != "txSegPausas" && textAreas[k].id != "txObsDibujo") {
                    textAreas[k].disabled = false;
                }

            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].disabled = false;
            }


            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='Number']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].disabled = false;
            }

            // Habilitar o deshabilitar los TextBox Type date
            var idsHabilitados = ["tbIngreso", "tbUltActiv", "tbEntrega"];
            var textBoxes = document.querySelectorAll("input[type='date']");
            for (var x = 0; x < textBoxes.length; x++) {

                // Comprueba si el ID del TextBox está en la lista de IDs habilitados
                if (idsHabilitados.includes(textBoxes.id)) {
                    textBoxes.readOnly = false; // Habilitar el TextBox
                } else {
                    textBoxes.readOnly = true; // Deshabilitar el TextBox
                }
            }



            var checkBoxesToEnable = ["chxAnimacion", "chxAmbientacion", "chxEspArq"];

            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = false; // Habilita el CheckBox
                }
            }

            // Se cambia el Numero de Render por Definir 
            var label = document.getElementById("NumeroRender");
            label.textContent = "Por Definir";


            // Obtén la fecha actual
            var fechaActual = new Date();
            // Calcula la fecha para 4 días después de la fecha actual
            var fecha4DiasDespues = new Date();
            fecha4DiasDespues.setDate(fechaActual.getDate() + 4);

            // Formatea las fechas en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaActualFormateada = fechaActual.toISOString().split('T')[0];
            var fecha4DiasDespuesFormateada = fecha4DiasDespues.toISOString().split('T')[0];



            var FechaActualAnio = new Date();

            // Establece la fecha al primer día del año actual
            FechaActualAnio.setMonth(0); // Establece el mes a enero (0)
            FechaActualAnio.setDate(1); // Establece el día al primero (1)


            // Formatea la fecha en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaFormateada2 = FechaActualAnio.toISOString().split('T')[0];




            // Asigna las fechas a los TextBox correspondientes por su ID
            document.getElementById("tbIngreso").value = fechaActualFormateada;
            document.getElementById("tbIngresoServidor").value = fechaActualFormateada;

            document.getElementById("tbUltActiv").value = fechaActualFormateada;
            document.getElementById("tbUltActivServidor").value = fechaActualFormateada;

            document.getElementById("tbEntrega").value = fecha4DiasDespuesFormateada;
            document.getElementById("tbEntregaServidor").value = fecha4DiasDespuesFormateada;


            document.getElementById("tbFechaOk").value = fechaFormateada2;
            document.getElementById("tbFechaOkServidor").value = fechaFormateada2;





            // Deshabilitar enlace Nuevo Render 
            document.getElementById("NuevoRender").classList.remove("enabled");

            // Habilitar enlace Grabar Render
            document.getElementById("GrabarRender").classList.add("enabled");

          



            $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Render_Venta.aspx/NuevoRender", // La URL debe apuntar al método en el servidor
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // La llamada al servidor fue exitosa, puedes realizar acciones adicionales aquí
                },
                error: function (error) {
                    // Manejar errores si los hay
                }
            });

         }

        function ModificarRender() {


            var tbTerminadoVentas = document.getElementById("tbTerminadoVentas");
            var terminadoVentasValue = tbTerminadoVentas.value;


            if (terminadoVentasValue.toLowerCase() === "true")
            {
                // No se puede modificar, muestra un mensaje de error
                alert("El render ya fue aprobado para Dibujo y Despiece, este departamento lo debe habilitar para ser modificado");
            }
            else
            {
                // Habilitar enlace grabar
                document.getElementById("GrabarRender").classList.add("enabled");

                // Dehabilitar enlace Moficiar
                document.getElementById("ModificarRender").classList.remove("enabled");


                // Habilitar o deshabilitar los DropDownList
                var dropDownLists = document.querySelectorAll("select");
                for (var j = 0; j < dropDownLists.length; j++) {
                    dropDownLists[j].disabled = false;
                }

                // Habilitar los TextArea
                var textAreas = document.querySelectorAll("textarea");
                for (var k = 0; k < textAreas.length; k++) {

                    if (textAreas[k].id != "txSegPausas" && textAreas[k].id != "txObsDibujo") {
                        textAreas[k].disabled = false;
                    }

                }

                // Habilitar o deshabilitar los TextBox Type text
                var textBoxes = document.querySelectorAll("input[type='text']");
                for (var i = 0; i < textBoxes.length; i++) {
                    textBoxes[i].disabled = false;
                }


                // Habilitar o deshabilitar los TextBox Type text
                var textBoxes = document.querySelectorAll("input[type='Number']");
                for (var i = 0; i < textBoxes.length; i++) {
                    textBoxes[i].disabled = false;
                }

                var idsHabilitados = ["tbIngreso", "tbUltActiv", "tbEntrega"];

                // Habilitar o deshabilitar los TextBox Type text
                var textBoxes = document.querySelectorAll("input[type='date']");
                for (var x = 0; x < textBoxes.length; x++) {

                    // Comprueba si el ID del TextBox está en la lista de IDs habilitados
                    if (idsHabilitados.includes(textBoxes.id)) {
                        textBoxes.readOnly = false; // Habilitar el TextBox
                    } else {
                        textBoxes.readOnly = true; // Deshabilitar el TextBox
                    }
                }

                var checkBoxesToEnable = ["chxAnimacion", "chxAmbientacion", "chxEspArq"];

                for (var i = 0; i < checkBoxesToEnable.length; i++) {
                    var checkBoxId = checkBoxesToEnable[i];
                    var checkBox = document.getElementById(checkBoxId);

                    if (checkBox) {
                        checkBox.disabled = false; // Habilita el CheckBox
                    }
                }

            }

             $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Render_Venta.aspx/ModificarRender", // La URL debe apuntar al método en el servidor
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // La llamada al servidor fue exitosa, puedes realizar acciones adicionales aquí
                },
                error: function (error) {
                    // Manejar errores si los hay
                }
            });




        }

        function Cancelar() {
            // Habilitar enlace Nuevo Render 
            document.getElementById("NuevoRender").classList.add("enabled");

            // Deshabilitar  enlace Importar Render
            document.getElementById("GrabarRender").classList.remove("enabled");

            // Habilitar enlace Importar Render
            document.getElementById("Actualizar").classList.remove("enabled");


            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona2") {
                    dropDownLists[j].disabled = true;
                }
            }


            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {

                if (textAreas[k].id != "txSegPausas" && textAreas[k].id != "txObsDibujo") {
                    textAreas[k].disabled = true;
                }

            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {


                if (textBoxes[i].id != "tbClienteX" && textBoxes[i].id != "tbProyectoX" && textBoxes[i].id != "tbNumeroRender") {
                    textBoxes[i].disabled = true;
                }

               
            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='Number']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].disabled = true;
            }


            var idsHabilitados = ["tbIngreso", "tbUltActiv", "tbEntrega"];

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='date']");
            for (var x = 0; x < textBoxes.length; x++) {

                // Comprueba si el ID del TextBox está en la lista de IDs habilitados
                if (idsHabilitados.includes(textBoxes.id)) {
                    textBoxes.readOnly = false; // Habilitar el TextBox
                } else {
                    textBoxes.readOnly = true; // Deshabilitar el TextBox
                }
            }



            var checkBoxesToEnable = ["chxAnimacion", "chxAmbientacion", "chxEspArq"];

            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = true; // Habilita el CheckBox
                }
            }



        }

        function AlertaBuscar(mensaje) {
            alert(mensaje);
        }

        function validarFormularioRender() {
            var Diseño = document.getElementById("tbDiseño").value;
            var Cliente = document.getElementById("tbCliente").value;
            var Asesor = document.getElementById("ddlAsesor").value
            var Proyecto = document.getElementById("tbProyecto").value;
            var Contacto = document.getElementById("tbContacto").value;
            var Celular = document.getElementById("tbCelular").value;
            var Mail = document.getElementById("tbMail").value;
            var Telefono = document.getElementById("tbTelefono").value;
            var Plano = document.getElementById("tbPlano").value;
            var Zona = document.getElementById("ddlZona").value;
            var Imagenes = document.getElementById("tbImagenes").value;
            var ObsAreaRender = document.getElementById("txAreaRender").value;
            var ObsVentas = document.getElementById("txObsVentas").value;
            //
            var Linea = document.getElementById("tbLinea").value;
            var Sup = document.getElementById("tbSup").value;
            var Acc = document.getElementById("tbAcc").value;
            var Cantos = document.getElementById("tbCantos").value;
            var Perfil = document.getElementById("tbPerfil").value;
            var Paneles = document.getElementById("tbPaneles").value;
            var Arch = document.getElementById("tbArch").value;
            var Sillas = document.getElementById("tbSillas").value;
            var Pantallas = document.getElementById("tbPantallas").value;
            var Muebles = document.getElementById("txMuebles").value;
            var AcaPisZoc = document.getElementById("tbAcaPisZoc").value;
            var AcaMuros = document.getElementById("tbAcaMuros").value;
            var Iluminacion = document.getElementById("tbIluminacion").value;
            var Antepecho = document.getElementById("tbAntepecho").value;



            var isValid = true;

            if (Diseño === "") {
                ErrorValidacionRender.innerHTML = "Campo Diseño obligatorio.";
                isValid = false;
            } else if (Cliente === "") {
                ErrorValidacionRender.innerHTML = "Campo Cliente obligatorio.";
                isValid = false;
            } else if (Asesor === "") {
                ErrorValidacionRender.innerHTML = "Por favor, seleccione un  Asesor.";
                isValid = false;
            } else if (Proyecto === "") {
                ErrorValidacionRender.innerHTML = "Campo Proyecto obligatorio.";
                isValid = false;
            }
            else if (Contacto === "") {
                ErrorValidacionRender.innerHTML = "Campo Contacto obligatorio.";
                isValid = false;
            } else if (Celular === "") {
                ErrorValidacionRender.innerHTML = "Campo Celular obligatorio.";
                isValid = false;
            } else if (Mail === "") {
                ErrorValidacionRender.innerHTML = "Campo Mail obligatorio.";
                isValid = false;
            }
            else if (Telefono === "") {
                ErrorValidacionRender.innerHTML = "Campo Teléfono obligatorio.";
                isValid = false;
            } else if (Plano === "") {
                ErrorValidacionRender.innerHTML = "Campo Plano obligatorio.";
                isValid = false;
            } else if (Zona === "") {
                ErrorValidacionRender.innerHTML = "Campo Zona obligatorio.";
                isValid = false;
            } else if (Imagenes === "") {
                ErrorValidacionRender.innerHTML = "Campo Imágenes obligatorio.";
                isValid = false;
            } else if (ObsAreaRender === "") {
                ErrorValidacionRender.innerHTML = "El Campo Área Renderizar es obligatorio.";
                isValid = false;
            } else if (ObsVentas === "") {
                ErrorValidacionRender.innerHTML = "El Campo Obs. Ventas es obligatorio.";
                isValid = false;
            }
            else if (Linea === "") {
                ErrorValidacionRender.innerHTML = "El campo Línea es obligatorio.";
                isValid = false;
            } else if (Sup === "") {
                ErrorValidacionRender.innerHTML = "El campo Sup es obligatorio.";
                isValid = false;
            } else if (Acc === "") {
                ErrorValidacionRender.innerHTML = "El campo Acc es obligatorio.";
                isValid = false;
            } else if (Cantos === "") {
                ErrorValidacionRender.innerHTML = "El campo Cantos es obligatorio.";
                isValid = false;
            }
            else if (Perfil === "") {
                ErrorValidacionRender.innerHTML = "El Campo Perfil es obligatorio.";
                isValid = false;
            } else if (Paneles === "") {
                ErrorValidacionRender.innerHTML = "El Campo Paneles es obligatorio.";
                isValid = false;
            } else if (Arch === "") {
                ErrorValidacionRender.innerHTML = "El campo  Arch  es obligatorio.";
                isValid = false;
            }
            else if (Sillas === "") {
                ErrorValidacionRender.innerHTML = "El Campo Sillas es obligatorio.";
                isValid = false;
            } else if (Pantallas === "") {
                ErrorValidacionRender.innerHTML = "El Campo Pantallas es obligatorio.";
                isValid = false;
            } else if (Muebles === "") {
                ErrorValidacionRender.innerHTML = "El Campo Muebles es obligatorio.";
                isValid = false;
            } else if (AcaPisZoc === "") {
                ErrorValidacionRender.innerHTML = "El Campo Acabados Piso y Zócalo es obligatorio.";
                isValid = false;
            } else if (AcaMuros === "") {
                ErrorValidacionRender.innerHTML = "El Campo Acabados Muros es obligatorio.";
                isValid = false;
            } else if (Iluminacion === "") {
                ErrorValidacionRender.innerHTML = "El Campo Iluminación y Tipos de lámparas es obligatorio.";
                isValid = false;
            } else if (Antepecho === "") {
                ErrorValidacionRender.innerHTML = "El Campo Sillar o antepecho es obligatorio.";
                isValid = false;
            }



            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
        }

        //Funcion para cuando seleccionan un desarrollo o una cotizacion nos lleva al formulario
        function activarTab(tabId) {
            // Oculta todas las pestañas
            $('#miPestañas a.BuscarRender-Content').removeClass('active');
            $('.tab-pane').removeClass('active show');

            // Activa la pestaña deseada
            $('#miPestañas a[href="#' + tabId + '"]').tab('show');
        }

    </script>




    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
