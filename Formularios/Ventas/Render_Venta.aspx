<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Render_Venta.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Render_Venta" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>Render Departamento Ventas</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/Render_Venta.css" />



    <script>
        function focusAndScrollToRow(rowId) {
            var row = document.getElementById(rowId);
            if (row) {
                row.setAttribute('tabindex', '-1'); // Make it focusable
                row.focus();
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
        }

        function SeleccionarFilayEnfocarRender(rowIndex) {
            var dataGrid = document.getElementById('<%= DataGridRenders.ClientID %>'); // Reemplaza DataGrid1 por el ID de tu DataGrid
            if (dataGrid && dataGrid.rows && dataGrid.rows.length > rowIndex + 1) { // Ajusta el índice para excluir el encabezado
                var row = dataGrid.rows[rowIndex + 1]; // Suma 1 para omitir el encabezado
                row.style.background = 'radial-gradient(circle, #b5bbc1, #23273be6)';
                row.style.color = '#ffffff'; // Cambia el color de la letra a blanco

                // Hacer scroll hasta la fila
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });

                // Establecer el foco en la fila
                row.setAttribute('tabindex', '-1'); // Hacerla enfocable
                row.focus();

            }
        }

        // Mostrar  modal pausar render
        function mostralMoldalPausarRender() {

            $('#ConfirmarPausarRender').modal('show');
        }

        // Mostrar  modal despausar render
        function mostralMoldalDespausarRender() {


            var numeroRender = document.getElementById('<%= NumeroRender.ClientID %>').innerText;
            // Actualiza el contenido del span con el valor del Label
            document.getElementById('Span_Id_Render3').innerText = numeroRender;


            $('#ConfirmarDespausarRender').modal('show');
        }

        // Mostrar  modal despausar render
        function mostralMoldalDevolverRender() {


            var numeroRender = document.getElementById('<%= NumeroRender.ClientID %>').innerText;
            // Actualiza el contenido del span con el valor del Label
            document.getElementById('Span_Id_Render1').innerText = numeroRender;


            $('#ConfirmarDevolRender').modal('show');
        }

        // Mostrar  modal eliminar render
        function mostralMoldalEliminarRender() {


            var numeroRender = document.getElementById('<%= NumeroRender.ClientID %>').innerText;
            // Actualiza el contenido del span con el valor del Label
            document.getElementById('Span_Id_Render2').innerText = numeroRender;


            $('#ConfirmarEliminarRender').modal('show');
        }

        function mostralMoldalDevolverRenderJustificacion() {
            $('#DevolverRenderJustificacion').modal('show');
        }

        function confirmProgramarRender(event) {

            var IdRender = document.getElementById("NumeroRender").innerHTML;
            var Nombre = document.getElementById("tbCliente").value;

            // Validamos el Área del Usuario 
            var AreaDepar = '<%= Session["Departamento"] %>';


            if (AreaDepar.toUpperCase() === "VENTAS") {
                if (IdRender === "") {
                    alert("No se ha seleccionado ningún render");
                    return false;
                } else {
                    var mensaje = "Una vez aprobado el Render no podrá modificarlo. Está seguro de programar el render: " + IdRender + " " + Nombre + " ?";

                    var result = confirm(mensaje);
                    if (result) {
                        // Llamar al evento del botón de eliminar en el servidor
                        $(event.target).removeAttr('onclick');
                        $(event.target).click();
                    }
                    return false; // Previene que el evento del botón se ejecute dos veces
                }
            } else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {

                if (IdRender === "") {
                    alert("No se ha seleccionado ningún render");
                    return false;
                } else {
                    var mensaje = "Una vez terminado el Render no podrá modificarlo. Está seguro de Terminar el render: " + IdRender + " " + Nombre + " ?";

                    var result = confirm(mensaje);
                    if (result) {
                        // Llamar al evento del botón de eliminar en el servidor
                        $(event.target).removeAttr('onclick');
                        $(event.target).click();
                    }
                    return false; // Previene que el evento del botón se ejecute dos veces
                }
            }

        }


        //Funcion para habilitar Modificar Cuando dan Click en linkButton Del DataGrid  dibujo ss
        function HabilitarEnlacesDibujo1() {
            // Habilitar enlaces
            document.getElementById("ModificarRender").classList.remove("disabled");
            document.getElementById("ModificarRender").classList.add("enabled", "AzulActivo");

            document.getElementById("GrabarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("GrabarRender").classList.add("disabled");

            // Si la página de Render es para el Área de Dibujo, se habilita Devolver, Pausar y Eliminar Render
            document.getElementById("DevolverRender").classList.remove("disabled");
            document.getElementById("DevolverRender").classList.add("enabled", "AzulActivo");

            document.getElementById("PausarRender").classList.remove("disabled");
            document.getElementById("PausarRender").classList.add("enabled", "AzulActivo");

            document.getElementById("EliminarRender").classList.remove("disabled");
            document.getElementById("EliminarRender").classList.add("enabled", "AzulActivo");
        }

        function HabilitarEnlacesDibujo2() {
            // Habilitar enlaces
            document.getElementById("ModificarRender").classList.remove("disabled");
            document.getElementById("ModificarRender").classList.add("enabled", "AzulActivo");

            document.getElementById("GrabarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("GrabarRender").classList.add("disabled");

            // Si la página de Render es para el Área de Dibujo, se habilita Devolver, Pausar y Eliminar Render
            document.getElementById("DevolverRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("DevolverRender").classList.add("disabled");

            document.getElementById("PausarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("PausarRender").classList.add("disabled");

            document.getElementById("DespausarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("DespausarRender").classList.add("disabled");

            document.getElementById("EliminarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("EliminarRender").classList.add("disabled");
        }

        function HabEnlRenderPausado() {

            // Habilitar enlaces
            document.getElementById("DespausarRender").classList.remove("disabled");
            document.getElementById("DespausarRender").classList.add("enabled", "AzulActivo");

            // Mostrar el LinkButton "PausarSolicitud"
            var DespausarRender = document.getElementById("<%= DespausarRender.ClientID %>");
            var PausarRender = document.getElementById("<%= PausarRender.ClientID %>");

            if (DespausarRender) {
                DespausarRender.style.display = '';
            }

            if (PausarRender) {
                PausarRender.style.display = 'none';
            }

        }

        function HabEnlRenderPausado1() {

            // Mostrar el LinkButton "PausarSolicitud"
            var DespausarRender = document.getElementById("<%= DespausarRender.ClientID %>");
            var PausarRender = document.getElementById("<%= PausarRender.ClientID %>");

            if (DespausarRender) {
                DespausarRender.style.display = 'none';
            }

            if (PausarRender) {
                PausarRender.style.display = '';
            }

        }

        function HabEnlRenderPausado2() {

            // Mostrar el LinkButton "PausarSolicitud"
            var DespausarRender = document.getElementById("<%= DespausarRender.ClientID %>");
            var PausarRender = document.getElementById("<%= PausarRender.ClientID %>");

            if (DespausarRender) {
                DespausarRender.style.display = '';
            }

            if (PausarRender) {
                PausarRender.style.display = 'none';
            }

        }


    </script>




</head>


<body translate="no">
    <form id="formRenderVenta" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light navbar-custom">
            <div class="container d-flex justify-content-center ">
                <ul class="nav nav-tabs gap-5" id="miPestañas">


                    <li class="nav-item">
                        <a class="nav-link text-white active" id="Render-tab" data-bs-toggle="tab" href="#Render-Content"><i class="bi bi-file-text"></i> Bitacora Renders</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="Programacion-tab" data-bs-toggle="tab" href="#Programacion-Content"><i class="bi bi-table"></i> Programación</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="BuscarRender-tab" data-bs-toggle="tab" href="#BuscarRender-Content"><i class="bi bi-search"></i> Buscar Renders</a>
                    </li>
                </ul>
            </div>
        </nav>

        <nav class="navbar navbar-expand-sm navbar-light bg-lights gap-2">
            <div class="container-fluid">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="ejemplo2">
                    <ul class="navbar-nav mx-auto contenedor-icono">

                        <div class="contenedor-icono">

                            <%--Comienza Nueva OT--%>



                            <a class="icong disabled btn btn-sm shadow-sm" title="Nuevo Render" id="NuevoRender" onclick="NuevoRender()">
                                <i class="bi bi-file-earmark-check-fill"></i>
                            </a>

                            <asp:LinkButton class="icong disabled btn btn-sm shadow-sm" runat="server" title="Guardar Render" ID="GrabarRender" OnClick="GuardarModificarRender" OnClientClick="return validarFormularioRender();">
                                  <i class="bi bi-floppy-fill"></i>
                            </asp:LinkButton>

                            <a class="icong disabled btn btn-sm shadow-sm" title="Modificar Render" id="ModificarRender" onclick="ModificarRender()">
                                <i class="bi bi-wrench-adjustable"></i>
                            </a>


                            <a class="icong disabled btn btn-sm shadow-sm" title="Regresar el Render a un Proceso Anterior" id="DevolverRender" runat="server" onclick="mostralMoldalDevolverRender();">
                                <i class="bi bi-skip-backward-circle"></i>
                            </a>

                            <a class="icong disabled btn btn-sm shadow-sm" title="Pausar Render" id="PausarRender" runat="server" onclick="mostralMoldalPausarRender();">
                                <i class="bi bi-pause-circle-fill"></i>
                            </a>

                            <a class="icong disabled btn btn-sm shadow-sm" title="Despausar Render" id="DespausarRender" onclick="mostralMoldalDespausarRender();" style="display: none" runat="server">
                                <i class="bi bi-play-circle-fill"></i>
                            </a>

                            <a class="icong disabled btn btn-sm shadow-sm" title="Importar Render" id="ImportarRender">
                                <i class="bi bi-arrow-down-circle-fill"></i>
                            </a>

                            <a class="icong disabled btn btn-sm shadow-sm" title="Cancelar" id="CancelarRender" onclick="Cancelar()">
                                <i class="bi bi-x-circle-fill"></i>
                            </a>

                            <a class="icong disabled btn btn-sm shadow-sm" title="Eliminar Render " id="EliminarRender" runat="server" onclick="mostralMoldalEliminarRender();">
                                <i class="bi bi-trash-fill"></i>
                            </a>


                            <ul />
                    </ul>

                    <span id="ErrorValidacionRender" style="color: red;"></span>
                </div>

            </div>
        </nav>


        <div class="tab-content">

            <div class="tab-pane fade show active p-2 m-2" id="Render-Content">
                <asp:UpdatePanel ID="PanelRender" runat="server">
                    <ContentTemplate>

                        <div class="container-fluid Principal gap-3 mb-2  border shadow rounded  p-3  ">

                            <div class=" container-fluid rounded border gap-2  ">

                                <div class="row pt-1 mt-1 pb-1 mb-1">
                                    <div class="col-sm-5">
                                        <div class="input-group input-group-sm  mb-2 gap-4 ">
                                            <asp:Label ID="lbIngreso" class="form-label" Text="Ingreso" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbIngreso" type="datetime-local" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbIngresoServidor" type="datetime-local" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4 justify-content-center">
                                            <asp:Label ID="lbRender" CssClass="NumeroRender" Text="Render #" runat="server"></asp:Label>
                                            <asp:Label ID="NumeroRender" CssClass="NumeroRender" Text="" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-3">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbUltActiv" class="form-label" Text="U.Activ" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbUltActiv" type="datetime-local" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbUltActivServidor" type="datetime-local" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbEntrega" class="form-label" Text="Entrega" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbEntrega" type="datetime-local" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbEntregaServidor" type="datetime-local" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbFechaOk" class="form-label" Text="Fecha OK" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbFechaOk" type="datetime-local" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbFechaOkServidor" type="datetime-local" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbDiseño" class="form-label" Text="Diseño" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbDiseño" type="text" class="form-control" runat="server" disabled="disabled" EnableViewState="true"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCliente" class="form-label" Text="Cliente" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCliente" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbAsesor" class="form-label" Text="Asesor" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlAsesor" runat="server" OnDataBound="ddlAsesores_DataBound"></asp:DropDownList>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbProyecto" class="form-label" Text="Proyecto" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbProyecto" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbContacto" class="form-label" Text="Contacto" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbContacto" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-4">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCelular" class="form-label" Text="Celular" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCelular" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbMail" class="form-label" Text="Mail" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbMail" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbTelefono" class="form-label" Text="Teléfono" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbTelefono" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-5">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPlano" class="form-label" Text="Plano" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPlano" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:Button class="btn btn-outline-secondary" ID="btnPlano" type="button" Text="..." runat="server" Enabled="false"></asp:Button>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbZona" class="form-label" Text="Zona" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlZona" runat="server">
                                                <asp:ListItem Value="">Seleccione</asp:ListItem>
                                                <asp:ListItem Value="01">01</asp:ListItem>
                                                <asp:ListItem Value="02">02</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbImagenes" class="form-label" Text="Imágenes" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbImagenes" type="Number" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1">

                                    <div class="col-sm-6">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbAreasRenderizar" class="form-label" Text="Áreas a Renderizar." runat="server"></asp:Label>
                                            <textarea class="form-control form-control-sm" id="txAreaRender" runat="server" cols="29" rows="4" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbObsVentas" class="form-label" Text="Observación Ventas." runat="server"></asp:Label>
                                            <textarea class="form-control form-control-sm" id="txObsVentas" runat="server" cols="29" rows="4" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1">

                                    <div class="col-sm-6">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbSegPausas" class="form-label" Text="Seguimiento de Pausas." runat="server"></asp:Label>
                                            <textarea class="form-control form-control-sm" id="txSegPausas" runat="server" cols="29" rows="4" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
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
                                <asp:TextBox ID="tbTerminadoDibujo" class="form-control" runat="server" CssClass="hidden-checkbox"></asp:TextBox>

                                <div class="row pt-1 mt-1 pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lblinea" class="form-label" Text="Línea" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbLinea" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbSup" class="form-label" Text="Sup" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbSup" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbAcc" class="form-label" Text="Acc" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbAcc" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCantos" class="form-label" Text="Cantos" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCantos" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPerfil" class="form-label" Text="Perfil" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPerfil" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPaneles" class="form-label" Text="Paneles" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPaneles" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbArch" class="form-label" Text="Arch" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbArch" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbSillas" class="form-label" Text="Sillas" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbSillas" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbPantallas" class="form-label" Text="Pantallas" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbPantallas" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbEspArq" class="form-label" Text="Espacio Arquitectónico" runat="server"></asp:Label>
                                            <asp:CheckBox ID="chxEspArq" runat="server" Enabled="false" />
                                        </div>
                                    </div>

                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-6">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class=" input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbMuebles" class="form-label" Text="Muebles" runat="server"></asp:Label>
                                                    <textarea class="form-control form-control-sm" id="txMuebles" runat="server" cols="29" rows="7" disabled="disabled"></textarea>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row pt-1 mt-1 pb-1 mb-1">
                                            <div class="col-sm-6">
                                                <div class=" input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAmbientacion" class="form-label" Text="Ambientación" runat="server"></asp:Label>
                                                    <asp:CheckBox ID="chxAmbientacion" runat="server" Enabled="false" />
                                                </div>
                                            </div>

                                            <div class="col-sm-6">
                                                <div class=" input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAnimacion" class="form-label" Text="Animación" runat="server"></asp:Label>
                                                    <asp:CheckBox ID="chxAnimacion" runat="server" Enabled="false" />
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="col-sm-6">

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAcaPisoYZocalo" class="form-label" Text="Acabados Piso y Zócalo" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbAcaPisZoc" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAcaMuros" class="form-label" Text="Acabados Muros" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbAcaMuros" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbIluminacion" class="form-label" Text="Iluminación y  Tipos de lámparas" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbIluminacion" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="input-group-sm  mb-1 gap-4">
                                                    <asp:Label ID="lbAntepecho" class="form-label" Text="Sillar o antepecho y ventanas" runat="server"></asp:Label>
                                                    <asp:TextBox ID="tbAntepecho" type="text" class="form-control " runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                </div>

                                <div class="row justify-content-end">

                                    <div class="col-sm-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-2 justify-content-center">
                                            <asp:Button class="btn btn-warning  " ID="btnProgramarRender" runat="server" Text="Programar" OnClick="ProgramarRender" OnClientClick="return confirmProgramarRender(event);" />
                                        </div>
                                    </div>

                                </div>



                            </div>

                        </div>

                        <!--Modal confirmar Pausar Render  -->
                        <div id="ConfirmarPausarRender" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                            <div class="modal-dialog modal-lg modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h6 class="modal-title text-center">Pausar Render  </h6>
                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <div class="row pb-2">
                                                <div class="col-sm-12">
                                                    <div class="input-group-sm gap-2">
                                                        <asp:Label ID="lbJusti" runat="server" Text="Razón de la pausa:"></asp:Label>
                                                        <textarea class="form-control form-control-sm" id="txJusticiacionPausaRender" runat="server" cols="25" rows="5"></textarea>
                                                    </div>
                                                </div>
                                            </div>

                                            <h6>Por favor justifique la causa de la pausa del render y presione aceptar </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnPausarRender_Si" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-primary" Style="width: 5rem;" OnClick="btnPausarRender_Si_Click" />
                                            <asp:Button runat="server" ID="btnPausarRender_No" Text="Cancelar" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-sm btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal confirmar Despausar Render  -->
                        <div id="ConfirmarDespausarRender" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h6 class="modal-title text-center">Despausar Render </h6>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿Desea despausar el Render N°  <span runat="server" id="Span_Id_Render3"></span>?</h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnDespausarRender_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm  btn-outline-primary" Style="width: 5rem;" OnClick="btnDespausarRender_SI_Click" />
                                            <asp:Button runat="server" ID="btnDespausarRender_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-sm btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal confirmar Devolver Solicitud  -->
                        <div id="ConfirmarDevolRender" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h6 class="modal-title text-center">Devolver Render </h6>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿Está seguro de devolver el Render N°:  <span runat="server" id="Span_Id_Render1"></span>
                                                <br />
                                                al proceso anterior?</h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnDevolverRender_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm  btn-outline-primary" Style="width: 5rem;" OnClick="btnDevolverRender_SI_Click" />
                                            <asp:Button runat="server" ID="btnDevolverRender_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-sm btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal Devolver Render  justificar  -->
                        <div id="DevolverRenderJustificacion" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                            <div class="modal-dialog modal-lg modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h6 class="modal-title text-center">Devolver Render  </h6>
                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <div class="row pb-2">
                                                <div class="col-sm-12">
                                                    <div class="input-group-sm gap-2">
                                                        <asp:Label ID="Label1" runat="server" Text="Razón de la devolución:"></asp:Label>
                                                        <textarea class="form-control form-control-sm" id="txJustificacionDevolucion" runat="server" cols="25" rows="5"></textarea>
                                                    </div>
                                                </div>
                                            </div>

                                            <h6>Por favor justifique la causa de la devolución del render y presione aceptar </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnDevolverJustificacion_SI" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-primary" Style="width: 5rem;" OnClick="btnDevolverJustificacion_SI_Click" />
                                            <asp:Button runat="server" ID="Button4" Text="Cancelar" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-sm btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal confirmar eliminar Solicitud  -->
                        <div id="ConfirmarEliminarRender" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger text-white">
                                        <h6 class="modal-title text-center">Devolver Render </h6>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿Está seguro de eliminar el render N°:  <span runat="server" id="Span_Id_Render2"></span>? </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnEliminarRender_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm  btn-outline-danger" Style="width: 5rem;" OnClick="btnEliminarRender_SI_Click" />
                                            <asp:Button runat="server" ID="btnEliminarRender_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal adjuntar documentos y Terminar Render Solicitud  -->
                        <div id="AdjuntarDocYTerminar" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h6 class="modal-title text-center">Adjuntar Documentos Render</h6>
                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <p>
                                                Adjunte todos los documentos adicionales desde la misma carpeta y presione 'Aceptar'.
                                                Si no necesita adjuntar documentos, solo presione 'Aceptar'.
                                            </p>
                                            <div class="row pt-2">
                                                <div class="col-10">
                                                    <div class="input-group input-group-sm">
                                                        <input type="file" id="FileUpload1" name="FileUpload1" multiple="multiple" class="form-control" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnAdjuntarYProgramarRender" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-primary" Style="width: 5rem;" OnClick="btnAdjuntarYTerminarRender_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>




                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAdjuntarYProgramarRender" />
                    </Triggers>

                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade  " id="Programacion-Content">
                <asp:UpdatePanel ID="PanelProgramacion" runat="server">
                    <ContentTemplate>

                        <div class="row" style="display: none;">
                            <asp:TextBox ID="tbId_Fila" type="text" class="form-control form-control-sm" placeHolder="Número de fila" runat="server"></asp:TextBox>
                        </div>

                        <div class="container p-2 border rounded shadow ">

                            <div class="card m-2">

                                <div class="card-header mb-3 " style="height: 2.9rem; background: repeating-radial-gradient(#fff,#eeebebe6)" id="headerDes" runat="server">

                                    <div class="row pb-2">

                                        <div class="col-lg-6">
                                        </div>

                                        <div class="col-lg-6 col-xs-12" id="BusDesDiv" runat="server">
                                            <div class="input-group input-group-sm gap-2 justify-content-end">
                                                <asp:TextBox ID="ID_Render_Buscado" CssClass="form-control form-control-sm  text-center fw-bold" Style="width: 15rem; max-width: 15rem;" placeHolder="N° Render" ToolTip="Digiteel número de render que desea buscar " runat="server"></asp:TextBox>
                                                <asp:LinkButton runat="server" Text="Buscar" CssClass="icong enabled shadow-sm btn btn-sm AzulActivo rounded" ID="btnBuscarRender" title="Buscar Render" Style="width: 2rem; font-size: 1.1rem;" OnClick="btnBuscarRender_Click">
                                                          <i class="bi bi-search"></i>
                                                </asp:LinkButton>

                                            </div>
                                        </div>


                                    </div>
                                </div>


                                <div class="card-body p-1" runat="server">

                                    <div class="row">

                                        <div class="col-lg-5 col-md-8 col-sm-8 col-xs-12">
                                            <div class=" input-group input-group-sm justify-content-around  mb-2 gap-2">
                                                <asp:Button ID="btnTrabajarRender" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Trabajar Render" OnClick="btnTrabajarRender_Click" />
                                                <asp:Button ID="btnDesprogramarRender" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Desprogramar" OnClick="btnDesprogramarRender_Click" />
                                            </div>
                                        </div>

                                        <div class="col-lg-3 col-md-4 col-sm-4 col-xs-12"></div>

                                        <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12 ">
                                            <div class="input-group  input-group-sm  mb-2 gap-2 justify-content-center">
                                                <asp:CheckBox ID="chxConvenciones" runat="server" OnCheckedChanged="chxConvenciones_CheckedChanged" AutoPostBack="true" />
                                                <asp:Label ID="lbConenciones" class="col-form-label-sm" Text="Ver Convenciones" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-lg-2 col-md-6 col-sm-6 col-xs-10 ">
                                            <div class=" input-group input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbZona2" class="form-label" Text="Zona" runat="server"></asp:Label>
                                                <asp:DropDownList class="form-control" ID="ddlZona2" runat="server" OnSelectedIndexChanged="RenderPorZonaX" AutoPostBack="true" DataTextField="Zona" DataValueField="Zona" DataSourceID="Zonas" OnDataBound="ddlZona2_DataBound">
                                                </asp:DropDownList><asp:SqlDataSource runat="server" ID="Zonas" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select Zona from tblRender group by Zona"></asp:SqlDataSource>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="container-fluid Bajo pt-2">
                                        <div class="row justify-content-center">
                                            <div class="border rounded p-2">
                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 27rem; height: 27rem; overflow-x: auto;">
                                                            <h5 class="datagrid-header text-start">Programación</h5>
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridRenders" runat="server" DataSourceID="CargarRenders" AutoGenerateColumns="false" OnItemDataBound="DataGridRenders_ItemDataBound" OnItemCommand="DataGridRenders_LinkButton">
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
                                                                    <asp:TemplateColumn HeaderText="Nombre-Render">
                                                                        <ItemTemplate>
                                                                            <span title='<%# Eval("Nombre") %>'>
                                                                                <%# Eval("Nombre") %>
                                                                            </span>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
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

                                                            <asp:SqlDataSource runat="server" ID="CargarRenders" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT   ROW_NUMBER() OVER (ORDER BY [Fecha_Ingreso]) AS Turno, Cliente +'-'+ Nombre_Render AS Nombre,    * FROM    tblRender WHERE     TerminadoRender = 0  order by Fecha_Ingreso Asc "></asp:SqlDataSource>
                                                            <asp:SqlDataSource ID="RenderPorZona" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT  ROW_NUMBER() OVER (ORDER BY [Fecha_Ingreso]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre, * from tblRender where Zona=@Parametro and TerminadoRender = 0 ORDER BY Fecha_Ingreso Asc ">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ddlZona2" PropertyName="SelectedValue" Name="Parametro"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>


                                                            <asp:SqlDataSource ID="RenderPorId" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT  ROW_NUMBER() OVER (ORDER BY [Fecha_Ingreso]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre, * from tblRender where Id_Render = @ID_Render and TerminadoRender = 0 ORDER BY Fecha_Ingreso Asc ">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="ID_Render_Buscado" PropertyName="Text" Name="ID_Render"></asp:ControlParameter>
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>


                                                        </div>




                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>

                        <!-- Modal convenciones -->
                        <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog  modal-dialog-centered ">
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
                                                            <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #72459b; white-space: nowrap"></div>
                                                            <label for="lbNoProgamado" class="form-label">Render No programado por Ventas</label>
                                                        </div>

                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #70ede4"></div>
                                                            <label for="lbPausado" class="form-label">Render Pausado</label>
                                                        </div>
                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group" style="width: 20px; height: 20px; border: 1px; background-color: #c86868"></div>
                                                            <label for="lbEspera" class="form-label">No cumplidos y en espera</label>
                                                        </div>
                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #efdd79"></div>
                                                            <label for="lbNormal" class="form-label">Programación Normal</label>
                                                        </div>
                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #77a765"></div>
                                                            <label for="lbTerminado" class="form-label">Render Terminados 100%</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>



                                    </div>

                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-sm btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <!--Modal Trabajar en Render  -->
                        <div id="modalConRender" class="modal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h6 class="modal-title text-center">Trabajar en Render </h6>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿ Desea trabajar en el render número:   <span runat="server" id="NumRender1"></span>?</h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnTrabajarRender_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm  btn-outline-primary" Style="width: 5rem;" OnClick="btnTrabajarRender_SI_Click" />
                                            <asp:Button runat="server" ID="Button2" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-sm btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal Desprogramar en Render  -->
                        <div id="ConfirDespRender" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h6 class="modal-title text-center">Desprogramar Render </h6>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿ Desea desprogramar el render número:   <span runat="server" id="NumRender2"></span>?</h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnDesprogramarRender_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm  btn-outline-primary" Style="width: 5rem;" OnClick="btnDesprogramarRender_SI_Click" />
                                            <asp:Button runat="server" ID="Button3" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-sm btn-outline-secondary" Style="width: 5rem;" />
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
                        <div class="container">

                            <div class="container-fluid border rounded shadow-sm bg-light ">

                                <div class="row pt-2">

                                    <div class="col-lg-8 col-xs-12">
                                        <div class="input-group input-group-sm  mb-2 gap-3">
                                            <asp:Label ID="lbFechaIngreso" class="form-label" Text="Fecha Ingreso" runat="server"></asp:Label>
                                            <asp:TextBox ID="FechaIni" type="date" runat="server" class="form-control"></asp:TextBox>
                                            <asp:Label ID="Label2" class="form-label" Text=" Y " runat="server"></asp:Label>
                                            <asp:TextBox ID="FechaFin" type="date" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-xs-12">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:Button ID="btnConsultar" type="button" Text="Consultar" class="btn btn-outline-secondary" runat="server" OnClick="ConsultarRender"></asp:Button>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-lg-1 col-md-2">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbClienteX" class="form-label" Text="Cliente" runat="server"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-2">

                                            <asp:TextBox ID="tbClienteX" type="text" class="form-control " runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-lg-1 col-md-2">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbProyectoX" class="form-label" Text="Proyecto" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:TextBox ID="tbProyectoX" type="text" class="form-control " runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-lg-1 col-md-2">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbNumeroRender" class="form-label" Text="Render N°" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:TextBox ID="tbNumeroRender" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="row justify-content-center pt-3 m-1">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Render Filtrados</h5>

                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="BuscarRender" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridBuscarRender_ItemDataBound" OnItemCommand="DataGridBuscarRenders_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerRenders2" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>"
                                                                    OnClientClick="activarTab('Render-Content');" />
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

                                                <asp:SqlDataSource ID="RenderFecha" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,  * from tblRender where Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC  ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="FechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="RenderCliente" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT  ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,   * FROM tblRender WHERE Cliente LIKE '%' + @NombreCliente + '%' AND Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="FechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbClienteX" PropertyName="Text" Name="NombreCliente"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="RenderNombreRender" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT  ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,  * FROM tblRender WHERE Nombre_Render LIKE '%' + @NombreRender + '%' AND Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC ">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="FechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="FechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbProyectoX" PropertyName="Text" Name="NombreRender"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="RenderXIdRender" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select  ROW_NUMBER() OVER (ORDER BY [Id_Render]) AS Turno,Cliente +'-'+ Nombre_Render AS Nombre,  * from tblRender where Id_Render = @IdRender AND Fecha_Ingreso between  @FechaIni and  @FechaFin ORDER BY  Fecha_Ingreso DESC ">
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

            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

        </div>

    </form>

    <script>

        // Validamos el Área del Usuario 
        var AreaDepar = '<%= Session["Departamento"] %>';


        if (AreaDepar.toUpperCase() === "VENTAS") {

            // se Habilitan y deshabilitan botones 

            document.getElementById("NuevoRender").classList.remove("disabled");
            document.getElementById("NuevoRender").classList.add("enabled", "AzulActivo");

            // PENDIENTE REVISAR SI SE NECESITA
            //document.getElementById("ImportarRender").classList.remove("disabled");
            //document.getElementById("ImportarRender").classList.add("enabled", "AzulActivo");S

            document.getElementById("CancelarRender").classList.remove("disabled");
            document.getElementById("CancelarRender").classList.add("enabled", "RojoCancelar");

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona2") {
                    dropDownLists[j].disabled = true;
                    dropDownLists[j].value = "";
                }
            }

        }
        else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {

            document.getElementById("CancelarRender").classList.remove("disabled");
            document.getElementById("CancelarRender").classList.add("enabled", "RojoCancelar");

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona2") {
                    dropDownLists[j].disabled = true;
                    dropDownLists[j].value = "";
                }
            }


            var ActivarTap = '<%= Session["ActivarTapBitaRender"] %>';
            var TerminadoVentas = '<%= Session["terVenta"] %>';
            var TerminadoDibujo = '<%= Session["terDibujo"] %>';
            var Pausado = '<%= Session["pausadoRender"] %>';

            if (ActivarTap === "1") {

                // Se Asigna el valor de Terminado dibujo para control 
                var TerDibujo = document.getElementById("tbTerminadoDibujo");
                TerDibujo.value = TerminadoDibujo;


                // Quitar 'active' de la pestaña actualmente activa y su contenido
                $('#Programacion-tab').removeClass('active');
                $('#Programacion-Content').removeClass('active show');

                // Activa la pestaña de Programación
                $('#Render-tab').addClass('active');
                $('#Render-Content').addClass('active show');

                if (TerminadoVentas === "True") {
                    HabilitarEnlacesDibujo1();

                    if (Pausado === "True") {
                        HabEnlRenderPausado();
                    }
                    else {
                        HabEnlRenderPausado1();
                    }

                } else {
                    HabilitarEnlacesDibujo2();

                    if (Pausado === "True") {
                        HabEnlRenderPausado2();
                    }
                    else {
                        HabEnlRenderPausado1();
                    }

                }

                // Llamado ajax para limpiar las variables de session 
                $.ajax({
                    type: "POST",
                    url: "Render_Venta.aspx/EliminarTapActRender",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                    },
                    error: function (error) {

                    }
                });

            }
            else {
                // Quitar 'active' de la pestaña actualmente activa y su contenido
                $('#Render-tab').removeClass('active');
                $('#Render-Content').removeClass('active show');

                // Activa la pestaña de Programación
                $('#Programacion-tab').addClass('active');
                $('#Programacion-Content').addClass('active show');
                $(".contenedor-icono").hide();
            }

            // PENDIENTE REVISAR SI SE NECESITA 
            //document.getElementById("ImportarRender").classList.remove("disabled");
            //document.getElementById("ImportarRender").classList.add("enabled", "AzulActivo");


        }

    </script>

    <script>

        // Ocultar el div con clase "contenedor-icono" cuando se activa la pestaña "Info-content" 
        $(document).ready(function () {
            $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var targetTab = $(e.target).attr("href");
                if (targetTab === "#BuscarRender-Content") {
                    $(".contenedor-icono").hide();
                } else if (targetTab === "#Programacion-Content") {
                    $(".contenedor-icono").hide();
                } else {
                    $(".contenedor-icono").show();
                }
            });
        });
    </script>

    <script>

        //Funcion para habilitar Modificar Cuando dan Click en linkButton Del DataGrid  ventas 
        function HabilitarEnlaces1() {

            // Habilitar enlaces
            document.getElementById("ModificarRender").classList.remove("disabled");
            document.getElementById("ModificarRender").classList.add("enabled", "AzulActivo");

            document.getElementById("GrabarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("GrabarRender").classList.add("disabled",);

            document.getElementById("NuevoRender").classList.remove("disabled");
            document.getElementById("NuevoRender").classList.add("enabled", "AzulActivo");


            // Si la pagina de Render Es para el Area de Dibujo , Se habilita Devolver, Pausar y Eliminar Render
            /* 
             document.getElementById("DevolverRender").classList.add("enabled");
             document.getElementById("PausarRender").classList.add("enabled");
             document.getElementById("EliminarRender").classList.add("enabled");
             */

        }

        function HabilitarEnlacesDibujo3() {

            // Habilitar enlaces
            document.getElementById("ModificarRender").classList.remove("disabled");
            document.getElementById("ModificarRender").classList.add("enabled", "AzulActivo");

            document.getElementById("GrabarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("GrabarRender").classList.add("disabled");

            document.getElementById("NuevoRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("NuevoRender").classList.add("disabled");


            // Si la pagina de Render Es para el Area de Dibujo , Se habilita Devolver, Pausar y Eliminar Render

            document.getElementById("DevolverRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("DevolverRender").classList.add("disabled");

            document.getElementById("PausarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("PausarRender").classList.add("disabled");

            document.getElementById("DespausarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("DespausarRender").classList.add("disabled");

            document.getElementById("EliminarRender").classList.remove("enabled", "AzulActivo");
            document.getElementById("EliminarRender").classList.add("disabled");


        }

        function NuevoRender() {
            var AreaDepar = '<%= Session["Departamento"] %>';


            if (AreaDepar.toUpperCase() === "VENTAS") {

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

                // Establece la fecha al primer día del año actual
                var FechaActualAnio = new Date();
                FechaActualAnio.setMonth(0); // Establece el mes a enero (0)
                FechaActualAnio.setDate(1); // Establece el día al primero (1)

                // Formatea las fechas en el formato deseado (YYYY-MM-DDTHH:MM)
                var fechaActualFormateada = formatearFechaConHora(fechaActual);
                var fecha4DiasDespuesFormateada = formatearFechaConHora(fecha4DiasDespues);
                var fechaFormateada2 = formatearFechaConHora(FechaActualAnio);

                // Asigna las fechas a los TextBox correspondientes por su ID
                document.getElementById("tbIngreso").value = fechaActualFormateada;
                document.getElementById("tbIngresoServidor").value = fechaActualFormateada;

                document.getElementById("tbUltActiv").value = fechaActualFormateada;
                document.getElementById("tbUltActivServidor").value = fechaActualFormateada;

                document.getElementById("tbEntrega").value = fecha4DiasDespuesFormateada;
                document.getElementById("tbEntregaServidor").value = fecha4DiasDespuesFormateada;

                document.getElementById("tbFechaOk").value = fechaFormateada2;
                document.getElementById("tbFechaOkServidor").value = fechaFormateada2;





                // Deshabilitar y habilitar botones  

                document.getElementById("NuevoRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("NuevoRender").classList.add("disabled");

                document.getElementById("ModificarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("ModificarRender").classList.add("disabled");


                document.getElementById("GrabarRender").classList.remove("disabled");
                document.getElementById("GrabarRender").classList.add("enabled", "AzulActivo");


            } else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {

            }


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

            var AreaDepar = '<%= Session["Departamento"] %>';


            if (AreaDepar.toUpperCase() === "VENTAS") {
                var tbTerminadoVentas = document.getElementById("tbTerminadoVentas");
                var terminadoVentasValue = tbTerminadoVentas.value;


                if (terminadoVentasValue.toLowerCase() === "true") {
                    // No se puede modificar, muestra un mensaje de error
                    alert("El render ya fue aprobado para Dibujo y Despiece, este departamento lo debe habilitar para ser modificado");
                }
                else {
                    // Habilitar y Dehabilitar botones
                    document.getElementById("GrabarRender").classList.remove("disabled");
                    document.getElementById("GrabarRender").classList.add("enabled", "AzulActivo");

                    document.getElementById("NuevoRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("NuevoRender").classList.add("disabled");


                    document.getElementById("ModificarRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("ModificarRender").classList.add("disabled");

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
            }
            else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {

                var tbTerminadoDibujo = document.getElementById("tbTerminadoDibujo");
                var terminadoDibValor = tbTerminadoDibujo.value;

                if (terminadoDibValor === "False") {
                    // Habilitar los TextArea
                    var textAreas = document.querySelectorAll("textarea");
                    for (var k = 0; k < textAreas.length; k++) {

                        if (textAreas[k].id === "txObsDibujo") {
                            textAreas[k].disabled = false;
                            textAreas[k].focus();
                        }

                    }



                    // Habilitar y Dehabilitar botones
                    document.getElementById("GrabarRender").classList.remove("disabled");
                    document.getElementById("GrabarRender").classList.add("enabled", "AzulActivo");

                    document.getElementById("NuevoRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("NuevoRender").classList.add("disabled");


                    document.getElementById("ModificarRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("ModificarRender").classList.add("disabled");

                    document.getElementById("DevolverRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("DevolverRender").classList.add("disabled");

                    document.getElementById("PausarRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("PausarRender").classList.add("disabled");

                    document.getElementById("DespausarRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("DespausarRender").classList.add("disabled");

                    document.getElementById("EliminarRender").classList.remove("enabled", "AzulActivo");
                    document.getElementById("EliminarRender").classList.add("disabled");
                } else {
                    // No se puede modificar, muestra un mensaje de error
                    alert("El render ya fue terminado por Dibujo y Despiece y no puede ser modificado");
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

            location.reload();
        }

        function AlertaBuscar(mensaje) {
            alert(mensaje);
        }

        function validarFormularioRender() {

            var AreaDepar = '<%= Session["Departamento"] %>';
            var isValid = true;

            if (AreaDepar.toUpperCase() === "VENTAS") {
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
                var ChkEspArq = document.getElementById("chxEspArq");
                var ChkEspArqValue = ChkEspArq.checked;




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
                } else if (AcaPisZoc === "" && ChkEspArqValue) {
                    ErrorValidacionRender.innerHTML = "El Campo Acabados Piso y Zócalo es obligatorio.";
                    isValid = false;
                } else if (AcaMuros === "" && ChkEspArqValue) {
                    ErrorValidacionRender.innerHTML = "El Campo Acabados Muros es obligatorio.";
                    isValid = false;
                } else if (Iluminacion === "" && ChkEspArqValue) {
                    ErrorValidacionRender.innerHTML = "El Campo Iluminación y Tipos de lámparas es obligatorio.";
                    isValid = false;
                } else if (Antepecho === "" && ChkEspArqValue) {
                    ErrorValidacionRender.innerHTML = "El Campo Sillar o antepecho es obligatorio.";
                    isValid = false;
                }


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

        // Escuchar el evento keydown en el documento
        document.addEventListener('keydown', function (event) {
            // Verificar si la tecla presionada es "Enter" (código de tecla 13)
            if (event.key === "Enter") {
                // Obtener el elemento que tiene el foco actualmente
                var focusedElement = document.activeElement;

                // Verificar si el elemento enfocado no es un textarea
                if (focusedElement.tagName !== 'TEXTAREA') {
                    // Prevenir la acción predeterminada del evento
                    event.preventDefault();

                }
                // Verificar si el elemento cual  pestaña esta activa  activa
                var activeTab = document.querySelector('.tab-content .tab-pane.active');
                if (activeTab) {
                    if (focusedElement.id === "FechaIni" || focusedElement.id === "FechaFin" || focusedElement.id === "tbClienteX" || focusedElement.id === "tbProyectoX" || focusedElement.id === "tbNumeroRender") {
                        document.getElementById('<%= btnConsultar.ClientID %>').click();
                    }
                    else if (focusedElement.id === "ID_Render_Buscado") {
                        document.getElementById('<%= btnBuscarRender.ClientID %>').click();
                    }

                }


            }
        });

        // Función para formatear una fecha con horas y minutos
        function formatearFechaConHora(fecha) {
            var dia = fecha.getDate().toString().padStart(2, '0');
            var mes = (fecha.getMonth() + 1).toString().padStart(2, '0');
            var anio = fecha.getFullYear();
            var horas = fecha.getHours().toString().padStart(2, '0');
            var minutos = fecha.getMinutes().toString().padStart(2, '0');

            return `${anio}-${mes}-${dia}T${horas}:${minutos}`;
        }

        function ControlCamporYBotones() {


            // Validamos el Área del Usuario 
            var AreaDepar = '<%= Session["Departamento"] %>';


            if (AreaDepar.toUpperCase() === "VENTAS") {
                // Habilitar enlaces
                document.getElementById("NuevoRender").classList.remove("disabled");
                document.getElementById("NuevoRender").classList.add("enabled", "AzulActivo");

                document.getElementById("ModificarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("ModificarRender").classList.add("disabled");

                document.getElementById("GrabarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("GrabarRender").classList.add("disabled",);


            } else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {
                // Habilitar enlaces y deshabilitar 
                document.getElementById("ModificarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("ModificarRender").classList.add("disabled",);

                document.getElementById("GrabarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("GrabarRender").classList.add("disabled",);

                document.getElementById("GrabarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("GrabarRender").classList.add("disabled",);

                document.getElementById("DevolverRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("DevolverRender").classList.add("disabled");

                document.getElementById("PausarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("PausarRender").classList.add("disabled");

                document.getElementById("DespausarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("DespausarRender").classList.add("disabled");

                document.getElementById("EliminarRender").classList.remove("enabled", "AzulActivo");
                document.getElementById("EliminarRender").classList.add("disabled",);
            }




            // Limpiamos los texbox tipo 
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "ID_Render_Buscado" && textBoxes[i].id !== "tbClienteX" && textBoxes[i].id !== "tbProyectoX" && textBoxes[i].id !== "tbNumeroRender") {
                    textBoxes[i].disabled = true;
                    textBoxes[i].value = "";

                }

            }

            // Limpiamos los texbox tipo number 
            var textBoxes1 = document.querySelectorAll("input[type='number']");
            for (var i = 0; i < textBoxes1.length; i++) {

                textBoxes1[i].disabled = true;
                textBoxes1[i].value = "";

            }

            // limpiamos loe textbox tipo Fecha 
            var textBoxes2 = document.querySelectorAll("input[type='datetime-local']");
            for (var i = 0; i < textBoxes2.length; i++) {

                textBoxes2[i].disabled = true;
                textBoxes2[i].value = "";

            }

            // Limpiar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {
                textAreas[k].disabled = true;
                textAreas[k].value = "";

            }

            // Limpiamos los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id !== "ddlZona2") {
                    dropDownLists[j].disabled = true; // Activa los dropdowns si es necesario
                    dropDownLists[j].value = ""; // Establece el valor en vacío
                }

            }

            //Limpiamos el numero de render 
            var numeroRender = document.getElementById("NumeroRender");
            numeroRender.innerText = "";
            numeroRender.value = "";


        }

    </script>



</body>
</html>
