<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solicitud_Especial.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Solicitud_Especial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Solicitud Especial </title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/SolicitudesEspeciales.css" />

    <script>
        function confirmProgramarSolicitud(event) {

            // Validamos el Área del Usuario 
            var AreaDepar = '<%= Session["Departamento"] %>';


            if (AreaDepar.toUpperCase() === "VENTAS")
            {
                var IdSolicitud = document.getElementById("lbNumeroSolicitud").innerHTML;
                var mensaje = "Una vez programada la solicitud, no podrá realizar modificaciones. Esta seguro de programar la solicitud: " + IdSolicitud;

                var result = confirm(mensaje);
                if (result) {

                    $(event.target).removeAttr('onclick');
                    $(event.target).click();
                }
                return false;

            } else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO")
            {
                var IdSolicitud = document.getElementById("lbNumeroSolicitud").innerHTML;
                var mensaje = "Una vez termnada la solicitud, no podrá realizar modificaciones. Esta seguro de terminar la solicitud: " + IdSolicitud;

                var result = confirm(mensaje);
                if (result) {

                    $(event.target).removeAttr('onclick');
                    $(event.target).click();
                }
                return false;

            }


          
        }

        function ActivarGuardar() {


            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbProveedor" && textBoxes[i].id !== "tbAncho" && textBoxes[i].id !== "tbAltura" && textBoxes[i].id !== "tbProfundidad"
                    && textBoxes[i].id !== "tbMaterial" && textBoxes[i].id !== "tbCliente" && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbTelefono"
                    && textBoxes[i].id !== "tbCelular" && textBoxes[i].id !== "tbMail" && textBoxes[i].id !== "tbDireccion" && textBoxes[i].id !== "tbPrecioSugerido"
                    && textBoxes[i].id !== "tbCantidad" && textBoxes[i].id !== "tbDesarrollaPor") {
                    textBoxes[i].disabled = false;


                }

            }

            var checkBoxesToEnable = ["chxViaticos"];
            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = false; // Habilita el CheckBox
                }
            }


            // Obtén la fecha actual
            var fechaActual = new Date();
            // Formatea las fechas en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaActualFormateada = fechaActual.toISOString().split('T')[0];

            var FechaActualAnio = new Date();

            // Establece la fecha al primer día del año actual
            FechaActualAnio.setMonth(0); // Establece el mes a enero (0)
            FechaActualAnio.setDate(1); // Establece el día al primero (1)
            // Formatea la fecha en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaFormateada2 = FechaActualAnio.toISOString().split('T')[0];




            // Asigna las fechas a los TextBox correspondientes por su ID
            document.getElementById("tbFechaIngreso").value = fechaActualFormateada;
            document.getElementById("tbFechaIngresoServidor").value = fechaActualFormateada;

            document.getElementById("tbFechaEntrega").value = fechaFormateada2;
            document.getElementById("tbFechaEntregaServidor").value = fechaFormateada2;

            document.getElementById("tbFechaRespuesta").value = fechaFormateada2;
            document.getElementById("tbFechaRespuestaServidor").value = fechaFormateada2;


            // Deshabilitar enlaces 
            document.getElementById("NuevaSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("NuevaSolicitud").classList.add("disabled");

            document.getElementById("ModificarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("ModificarSolicitud").classList.add("disabled");

            // Habilitar enlaces
            document.getElementById("GrabarSolicitud").classList.remove("disabled");
            document.getElementById("GrabarSolicitud").classList.add("enabled", "AzulActivo");


        }

        function confirmarQuitarDetalle(event) {

            var IdSolicitud = document.getElementById("lbNumeroSolicitud").innerHTML;
            var IdDetalle = document.getElementById("lbIdDetalle").innerHTML;

            var mensaje = "Estás seguro de eliminar el detalle " + IdDetalle + " de la solicitud " + IdSolicitud;
            var result = confirm(mensaje);
            if (!result) {
                event.preventDefault(); // Cancelar el postback
            }
            return result; // Devolver el resultado de la confirmación
        }

    </script>

    <script>
        function precio(event) {

            var mensaje = "Estás seguro de eliminar el detalle ";
            var result = confirm(mensaje);
            if (!result) {
                event.preventDefault(); // Cancelar el postback
            }
            return result; // Devolver el resultado de la confirmación
        }

        function ActiBotDetalleVentas() {
            //habilitar enlaces de Detalle
            document.getElementById("NuevoDetalle").classList.remove("disabled");
            document.getElementById("NuevoDetalle").classList.add("enabled", "AzulActivo");

            document.getElementById("ImportarDetalle").classList.remove("disabled");
            document.getElementById("ImportarDetalle").classList.add("enabled", "AzulActivo");

            document.getElementById("ModificarSolicitud").classList.remove("disabled");
            document.getElementById("ModificarSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("GrabarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("GrabarSolicitud").classList.add("disabled");

            document.getElementById("NuevaSolicitud").classList.remove("disabled");
            document.getElementById("NuevaSolicitud").classList.add("enabled", "AzulActivo");

        }

        function ControlDesplegables(elementIds) {
            // Deshabilitar cada elemento por su ID
            elementIds.forEach(function (id) {
                var element = document.getElementById(id);
                if (element) {
                    element.classList.add("disabled");
                    element.disabled = true; // Deshabilitar el dropdown
                }
            });
        }

    </script>

    <script>
        function focusAndScrollToRow(rowId) {
            var row = document.getElementById(rowId);
            if (row) {
                row.setAttribute('tabindex', '-1'); // Make it focusable
                row.focus();
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });


            }
        }

        function SeleccionarFilayEnfocarCotizacion(rowIndex) {
            var dataGrid = document.getElementById('<%= DataGrid2.ClientID %>'); // Reemplaza DataGrid1 por el ID de tu DataGrid
            if (dataGrid && dataGrid.rows && dataGrid.rows.length > rowIndex + 1) { // Ajusta el índice para excluir el encabezado
                var row = dataGrid.rows[rowIndex + 1]; // Suma 1 para omitir el encabezado
                row.style.background = 'radial-gradient(circle, #b5bbc1, #23273be6)';
                row.style.color = '#ffffff'; // Cambia el color de la letra a blanco

                // Hacer scroll hasta la fila
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });

                // Establecer el foco en la fila
                row.setAttribute('tabindex', '-1'); // Hacerla enfocable
                row.focus();

                ControlBtnCliente();
            }
        }

        function SeleccionarFilayEnfocarDesarrollo(rowIndex) {
            var dataGrid = document.getElementById('<%= DataGrid1.ClientID %>'); // Reemplaza DataGrid1 por el ID de tu DataGrid
            if (dataGrid && dataGrid.rows && dataGrid.rows.length > rowIndex + 1) { // Ajusta el índice para excluir el encabezado
                var row = dataGrid.rows[rowIndex + 1]; // Suma 1 para omitir el encabezado
                row.style.background = 'radial-gradient(circle, #b5bbc1, #23273be6)';
                row.style.color = '#ffffff'; // Cambia el color de la letra a blanco

                // Hacer scroll hasta la fila
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });

                // Establecer el foco en la fila
                row.setAttribute('tabindex', '-1'); // Hacerla enfocable
                row.focus();

                ControlBtnCliente();
            }
        }



    </script>


</head>
<body translate="no">

    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light navbar-custom">
            <div class="container d-flex justify-content-center gap-2">
                <ul class="nav nav-tabs gap-4" id="miPestañas">
                    <li class="nav-item">
                        <a class="nav-link text-white active" id="BitacoraDesarrollo-tab" data-bs-toggle="tab" href="#BitacoraDesarrollo-content"><i class="bi bi-file-text"></i> Desarrollo Bitacora- PQ-006</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="Programacion-tab" data-bs-toggle="tab" href="#Programacion-content"><i class="bi bi-table"></i> Programacion</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link text-white " id="BuscarDesarrollo-tab" data-bs-toggle="tab" href="#BuscarDesarrollo-content"><i class="bi bi-search"></i> Buscar Solicitudes</a>
                    </li>

                </ul>
            </div>
        </nav>

        <nav class="navbar navbar-expand-sm navbar-light  bg-light  gap-2">
            <div class="container-fluid">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="ejemplo2">
                    <ul class="navbar-nav mx-auto contenedor-icono">

                        <div class="contenedor-icono">

                            <%--Comienza Nueva OT--%>



                            <a class="icong disabled shadow-sm btn btn-sm" href="#" title="Nueva Solicitud" id="NuevaSolicitud" onclick="NuevaSolicitud()">
                                <i class="bi bi-file-earmark-check-fill"></i>
                            </a>

                            <a class="icong disabled shadow-sm btn btn-sm" title="Pausar Solicitud" id="PausarSolicitud" runat="server" onclick="mostralMoldalPausar();">
                                <i class="bi bi-pause-circle-fill"></i>
                            </a>
                            <a class="icong disabled shadow-sm btn btn-sm " runat="server" title="Despausar" id="DespausarSolicitud" style="display: none;" onclick="mostralMoldalDespausar();">
                                <i class="bi bi-play-circle-fill"></i>
                            </a>
                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm " runat="server" title="Guardar Solicitud" ID="GrabarSolicitud" OnClick="GuardarModificarSolicitud" OnClientClick="return validarFormularioSolicitud();">                                 
                                                               <i class="bi bi-floppy-fill"></i>
                            </asp:LinkButton>

                            <a class="icong disabled shadow-sm btn btn-sm" href="#" title="Modificar Solicitud" id="ModificarSolicitud" onclick="ModificarSolicitud()">
                                <i class="bi bi-wrench-adjustable"></i>
                            </a>

                            <a class="icong disabled shadow-sm btn btn-sm" href="#" title="Observaciones" id="Observaciones" onclick="abrirObservaciones();">
                                <i class="bi bi-binoculars-fill"></i>
                            </a>

                            <a class="icong disabled shadow-sm btn btn-sm" href="#" title="Devolver Solicitud a Ventas" id="DevolverSolicitud" onclick="mostralMoldalDevolver()">
                                <i class="bi bi-skip-backward-circle"></i>
                            </a>

                            <a class="icong disabled shadow-sm btn btn-sm" title="Detener Pedido PE" id="DetenerPE" runat="server" onclick="mostralMoldalDetener();">
                                <i class="bi bi-stop-circle"></i>
                            </a>


                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm " title="Cancelar" ID="CancelarSolicitud" OnClientClick="CancelarSolicitud();" OnClick="LimpiarCampos" runat="server">
                                                             <i class="bi bi-x-circle-fill"></i>
                            </asp:LinkButton>

                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm" title="Eliminar Solicitud " ID="EliminarSolicitud" runat="server">
                                                            <i class="bi bi-trash-fill"></i>
                            </asp:LinkButton>


                            <ul />
                    </ul>


                    <span id="ErrorValidacion" style="color: red;"></span>

                </div>

            </div>
        </nav>


        <div class="tab-content">

            <div class="tab-pane fade  show active" id="BitacoraDesarrollo-content">
                <asp:UpdatePanel ID="PanelBitacora" runat="server">
                    <ContentTemplate>

                        <div class="container-fluid shadow rounded mt-2  ">

                            <div class="container-fluid border shadow-sm rounded bg-light mb-2 ">

                                <div class="row pt-1 mt-1 pb-1 mb-1 ">

                                    <div class="col-sm-1">
                                        <div class="  input-group-sm  mb-2 gap-4 ">
                                            <asp:Label ID="lbSolicitud" CssClass="lbDesarrollo col-form-label-sm" Text="Solicitud #" runat="server"></asp:Label>
                                            <asp:Label ID="lbNumeroSolicitud" CssClass="lbDesarrolloNumero col-form-label-sm" Text="" runat="server" disabled="disabled"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" mb-2 gap-4  input-group-sm">
                                            <asp:Label ID="lbFechaIngreso" CssClass="col-form-label-sm" Text="Ingreso" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbFechaIngreso" type="date" CssClass="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbFechaIngresoServidor" type="date" class="form-control" runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" mb-2 gap-4  input-group-sm">
                                            <asp:Label ID="lbFechaEntrega" class="col-form-label-sm" Text="Entrega" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbFechaEntrega" type="date" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbFechaEntregaServidor" type="date" class="form-control" runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" mb-2 gap-4  input-group-sm">
                                            <asp:Label ID="lbFechaRespuesa" class="col-form-label-sm" Text="Fecha Respuesta" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbFechaRespuesta" type="date" class="form-control form-control-sm" runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbFechaRespuestaServidor" type="date" class="form-control" runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbDirigido" class=" col-form-label-sm" Text="Dirigido a" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control form-control-sm" ID="ddlDirigido" runat="server">
                                                <asp:ListItem Value="">Seleccione</asp:ListItem>
                                                <asp:ListItem Value="COMPRAS">COMPRAS</asp:ListItem>
                                                <asp:ListItem Value="DESARROLLO DE PRODUCTO">DESARROLLO DE PRODUCTO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbTipo" class="col-form-label-sm" Text="Tipo" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control form-control-sm" ID="ddlTipo" runat="server">
                                                <asp:ListItem Value="">Seleccione</asp:ListItem>
                                                <asp:ListItem Value="COTIZACIÓN">COTIZACIÓN</asp:ListItem>
                                                <asp:ListItem Value="DESARROLLO">DESARROLLO</asp:ListItem>
                                                <asp:ListItem Value="PRODUCTO EN LINEA">PRODUCTO EN LINEA</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-1">
                                        <div class=" input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbSolicitudOrigen" class="col-form-label-sm" Text="SolicitudOrigen" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbSolicitudOrigen" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>


                                </div>

                                <div class="row pt-1 mt-1 pb-1 mb-1">
                                    <div class="col-sm-3">

                                        <div class="input-group input-group-sm  mb-2 gap-3 justify-content-center">
                                            <asp:Label ID="lbProyecto" class="col-form-label-sm" Text="Proyecto" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbProyecto" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbProyectoServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCiudad" class="col-form-label-sm" Text="Ciudad" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" DataTextField="NombreCiudad" DataValueField="NombreCiudad" DataSourceID="CargarCiudad" OnDataBound="ddlCiudad_DataBound"></asp:DropDownList>
                                            <asp:SqlDataSource runat="server" ID="CargarCiudad" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT 
                                            CONCAT(tblDepartamentoPais.CodigoDepartamento ,
                                            tblCiudad.CodigoCiudad)   AS CodCompleto,
                                            tblCiudad.NombreCiudad+' - '+tblDepartamentoPais.NombreDepartamento As NombreCiudad
                                            FROM tblDepartamentoPais 
                                            INNER JOIN tblCiudad
                                            ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento 
                                            ORDER BY CONCAT(tblCiudad.NombreCiudad , '-' , tblDepartamentoPais.NombreDepartamento)"></asp:SqlDataSource>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class=" input-group-sm  mb-2 gap-4 justify-content-center">
                                            <asp:CheckBox ID="chxViaticos" runat="server" Enabled="false" />
                                            <asp:Label ID="lbViaticoYTransporte" class=" col-form-label-sm" Text="Cotizar Viaticos y Transporte" runat="server"></asp:Label>

                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-4 justify-content-center">
                                            <asp:Label ID="lbCotizacionEsp" class="col-form-label-sm" Text="Cotización ESP" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCotizacionEsp" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row pb-1 mb-1">

                                    <div class="col-sm-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Button CssClass="btn btn-outline-secondary" ID="btnCliente" runat="server" Text="Cliente" OnClick="GuardarDatosSesion" OnClientClick="abrirOtraPestana();" />
                                            <asp:TextBox ID="tbCliente" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbClienteServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbContacto" class=" col-form-label-sm  " Text="Contacto" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbContacto" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbContactoServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbTelefono" class="col-form-label-sm " Text="Telefono" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbTelefono" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbTelefonoServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbCelular" class="col-form-label-sm" Text="Celular" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbCelular" type="text" class="form-control form-control-sm" runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbCelularServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbDesarrollado" class="col-form-label-sm" Text="Desarrolado Por" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbDesarrollaPor" type="text" class="form-control form-control-sm" runat="server" Text="" disabled="disabled"></asp:TextBox>

                                        </div>
                                    </div>

                                </div>

                                <div class="row pt-1 mt-1 pb-1 mb-1">

                                    <div class="col-sm-3">
                                        <div class="input-group input-group-sm  mb-2 gap-3 justify-content-center">
                                            <asp:Label ID="lbMail" class="col-form-label-sm" Text="Mail" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbMail" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbMailServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class=" input-group input-group-sm  mb-2 gap-2 justify-content-center">
                                            <asp:Label ID="lbDireccion" class="col-form-label-sm" Text="Dirección" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbDireccion" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                            <asp:TextBox ID="tbDireccionServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="input-group input-group-sm  mb-2 gap-4">
                                            <asp:Label ID="lbAsesor" class="col-form-label-sm" Text="Asesor" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control form-control-sm" ID="ddlAsesor" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:CheckBox ID="chxDesComplejo" CssClass="pt-1" runat="server" Enabled="false" />
                                            <asp:Label ID="lb" class=" col-form-label-sm" Text="Desarrollo Complejo" runat="server"></asp:Label>
                                            <asp:Button ID="ConfirmarComplejo" CssClass="btn btn-outline-secondary" runat="server" ToolTip="Para cambiar el estado complejo de la solicitud, marque la casilla y luego haga clic en Confirmar Complejo" Text="Confirmar Complejo" OnClick="ConfirmarComplejo_Click" />
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="container-fluid Principal-centro  mt-1 p-2 border rounded shadow-sm bg-light mb-2">

                                <div class="container-fluid izq">
                                    <h6 class="p-0 m-0">Producto</h6>

                                    <div class="row">
                                        <div class=" col-6-sm">
                                            <div class=" input-group-sm  mb-2 gap-2">
                                                <textarea class="form-control form-control-sm" id="txDescProduc" runat="server" cols="25" rows="3" maxlength="99" placeholder="Escriba máximo 99 caracteres" disabled="disabled"></textarea>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-12-sm">
                                            <div class=" input-group input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbProveedor" class="col-form-label-sm" Text="Proveedor Venta" runat="server"></asp:Label>
                                                <asp:TextBox ID="tbProveedor" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-12-sm">
                                            <div class="input-group input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbMedidas" class="col-form-label-sm" Text="Medidas(Cms)" runat="server"></asp:Label>
                                                <asp:TextBox ID="tbAncho" type="text" class="form-control form-control-sm " runat="server" placeholder="Ancho" title="Ancho" disabled="disabled"></asp:TextBox>
                                                <asp:TextBox ID="tbAltura" type="text" class="form-control form-control-sm " runat="server" placeholder="Altura" title="Alto" disabled="disabled"></asp:TextBox>
                                                <asp:TextBox ID="tbProfundidad" type="text" class="form-control form-control-sm " runat="server" placeholder="profundidad" title="Profundidad" disabled="disabled"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-3">
                                            <div class=" input-group input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbMaterial" class="col-form-label-sm" Text="Material" runat="server"></asp:Label>


                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class=" input-group input-group-sm  mb-2 gap-2">

                                                <asp:TextBox ID="tbMaterial" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-3">
                                            <div class=" input-group input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbCantidad" class="col-form-label-sm" Text="Cantidad" runat="server"></asp:Label>


                                            </div>
                                        </div>

                                        <div class="col-4">
                                            <div class=" input-group input-group-sm  mb-2 gap-2">

                                                <asp:TextBox ID="tbCantidad" type="number" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>

                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="container-fluid  centro">
                                    <h6 class="p-1 m-0">Especificaciones Generales</h6>
                                    <div class="row">
                                        <div class=" col-6-sm">
                                            <div class=" input-group-sm  mb-2 gap-2">
                                                <textarea class="form-control form-control-sm" id="txEspGen" runat="server" cols="25" rows="11" disabled="disabled"></textarea>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                                <div class="container-fluid  Der">

                                    <h6 class="p-0 m-0">Observacion Compra</h6>
                                    <div class="row">
                                        <div class=" col-6-sm">
                                            <div class=" input-group-sm  mb-2 gap-2">
                                                <textarea class="form-control form-control-sm" id="txobsCompras" runat="server" cols="25" rows="2" disabled="disabled"></textarea>
                                            </div>
                                        </div>

                                    </div>

                                    <h6 class="p-0 m-0">Observaciones Desarrollo</h6>
                                    <div class="row">
                                        <div class=" col-6-sm">
                                            <div class=" input-group-sm  mb-2 gap-2">
                                                <textarea class="form-control form-control-sm" id="txObsDesarrollo" runat="server" cols="25" rows="2" disabled="disabled"></textarea>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row ">

                                        <div class="col-4">
                                            <div class=" input-group input-group-sm  mb-2 gap-2">
                                                <asp:CheckBox ID="chxUrgente" runat="server" Enabled="false" />
                                                <asp:Label ID="lbUrgente" class="form-label pt-1" Text="Urgente" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-4">
                                            <div class=" input-group input-group-sm  ">
                                                <asp:Button ID="btnConUrgente" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Confirmar Urgente" ToolTip="Para cambiar el estado urgente de la solicitud, marque la casilla y luego haga clic en Confirmar Urgente" OnClick="btnConUrgente_Click" />
                                            </div>
                                        </div>

                                    </div>

                                    <div class=" row ">
                                        <div class="col-4">
                                            <div class=" input-group input-group-sm   gap-2">
                                                <asp:Label ID="lbPrecioSugerido" class="form-label" Text="Precio Sugerido" runat="server"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class=" input-group input-group-sm   gap-2">
                                                <asp:TextBox ID="tbPrecioSugerido" type="text" class="form-control PrecioSugerido " runat="server" disabled="disabled"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>


                                </div>

                                <div class="container-fluid  DerCompras" runat="server" id="DerCompras">

                                    <div class="border p-1">
                                        <h6 class="p-0 m-0">Compras</h6>
                                        <hr class="p-0 m-0 mb-3 w-50" />

                                        <div class="row pb-2 ">
                                            <div class="col-6">
                                                <div class="input-group-sm gap-1">
                                                    <asp:Label ID="lbCosto" runat="server" Text="Costo"></asp:Label>
                                                    <asp:TextBox CssClass="form-control form-control-sm" ID="tbCostoC" runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <div class="input-group-sm gap-1">
                                                    <asp:Label ID="lbFactor" runat="server" Text="Factor"></asp:Label>
                                                    <asp:TextBox CssClass="form-control" ID="tbFactorC" runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row pb-2">
                                            <div class="col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label ID="lbProve" runat="server" Text="Proveedor"></asp:Label>
                                                    <asp:TextBox CssClass="form-control form-control-sm" ID="tbProve" runat="server" disabled="disabled"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="border p-1 ">
                                        <h6 class="p-0 m-0">Desarrollo Producto</h6>
                                        <hr class="p-0 m-0 mb-2 w-50" />

                                        <div class="row pb-2">

                                            <div class="col-6">
                                                <div class="input-group-sm gap-1">
                                                    <asp:Label ID="lbCostoD" runat="server" Text="Costo"></asp:Label>
                                                    <asp:TextBox CssClass="form-control form-control-sm" ID="tbCostoD" type="number" min="0" runat="server" disabled="disabled" oninput="calcularPrecioSugerido()" Enabled="true"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-6">
                                                <div class="input-group-sm gap-1">
                                                    <asp:Label ID="lbFactorD" runat="server" Text="Factor"></asp:Label>
                                                    <asp:TextBox CssClass="form-control form-control-sm" ID="tbFactorD" type="number" runat="server" disabled="disabled" oninput="calcularPrecioSugerido()" Enabled="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row pb-2">
                                            <div class="col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label ID="lbCatego" runat="server" Text="Categoria"></asp:Label>
                                                    <asp:DropDownList ID="ddlCatego" CssClass="form-control form-control-sm" runat="server" disabled="disabled"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>


                                </div>

                            </div>

                            <div class="container-fluid Bajo shadow-sm border rounded">

                                <div class="row justify-content-center">
                                    <div class="border rounded p-2">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="table-responsive  mb-2 gap-2" style="max-height: 10rem; overflow-x: auto;">
                                                    <h6 class="datagrid-header text-center">Detalle Desarrollo</h6>
                                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGridDetalleSolicitud" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="DetalleSolicitud" OnItemCommand="DataGridDetalleSolicitud_LinkButton" OnItemDataBound="DataGridDetalleSolicitud_ItemDataBound">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="...">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="VerDetalleSolicitud" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Id_SolicitudDetalle" HeaderText="ID" />
                                                            <asp:BoundColumn DataField="Producto" HeaderText="Producto" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Dimension" HeaderText="Dimension" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="PrecioSugerido" HeaderText="P. Sugerido" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="RedirigidoaCompras" HeaderText="Red" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                            <asp:BoundColumn DataField="Redirigidoel" HeaderText="Reedirido el" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="ComprasOk" HeaderText="Ok Compras" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                            <asp:BoundColumn DataField="FechaComprasOk" HeaderText="Fecha Compras OK" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="RealizadoPor" HeaderText="Realizado Por" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="ID_SolicitudOrigen" HeaderText="Solicitud Origen" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Id_SolicitudDetalleOrigen" HeaderText="Detalle Origen" ItemStyle-CssClass="auto-width-column" />

                                                            <%-- Campos oscultos pero que se muestran en el formulario empieza en el 13]--%>

                                                            <asp:BoundColumn DataField="ProveedorSugerido" Visible="false" />
                                                            <asp:BoundColumn DataField="Ancho" Visible="false" />
                                                            <asp:BoundColumn DataField="Alto" Visible="false" />
                                                            <asp:BoundColumn DataField="Profundidad" Visible="false" />

                                                            <asp:BoundColumn DataField="Material" Visible="false" />
                                                            <asp:BoundColumn DataField="Cantidad" Visible="false" />
                                                            <asp:BoundColumn DataField="EspecificacionesTecnicas" Visible="false" />
                                                            <asp:BoundColumn DataField="ObservacionCompras" Visible="false" />
                                                            <asp:BoundColumn DataField="ObservacionDesarrollo" Visible="false" />
                                                            <asp:BoundColumn DataField="Urgente" Visible="false" />
                                                            <asp:BoundColumn DataField="InformacionDetalleOrigen" Visible="false" />

                                                            <%-- Campos de Compras y Desarrollo --%>
                                                            <asp:BoundColumn DataField="CostoCompras" Visible="false" />
                                                            <asp:BoundColumn DataField="FactorCompras" Visible="false" />
                                                            <asp:BoundColumn DataField="Costo" Visible="false" />
                                                            <asp:BoundColumn DataField="Factor" Visible="false" />
                                                            <asp:BoundColumn DataField="Categoria" Visible="false" />
                                                            <asp:BoundColumn DataField="Proveedor" Visible="false" />

                                                        </Columns>
                                                    </asp:DataGrid>
                                                    <asp:SqlDataSource ID="DetalleSolicitud" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand=" Select *
                                                              , concat(Ancho , 'x' , Alto , 'x' , Profundidad) as Dimension  from tblSoliciDiseEspeDeta  where  ID_Solicitud= @IdSolicitud">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lbNumeroSolicitud" PropertyName="Text" Name="IdSolicitud"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>




                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row Bajo1">

                                    <div class=" col-md-4">
                                        <h6 class="p-1 m-0">Información Detalle Solicitud Origen </h6>
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <textarea class="form-control form-control-sm" id="txInformacionDetalle" runat="server" cols="25" rows="5" disabled="disabled"></textarea>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <h6 class="p-1 m-0">Seguimiento Pausas </h6>
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <textarea class="form-control form-control-sm" id="txSegPausa" runat="server" cols="25" rows="5" disabled="disabled"></textarea>
                                        </div>

                                    </div>

                                    <div class="col-md-4">
                                        <nav class="navbar navbar-expand-sm navbar-light bg-light  mt-lg-4">
                                            <div class="container-fluid">

                                                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                                    <span class="navbar-toggler-icon"></span>
                                                </button>
                                                <div class="collapse navbar-collapse" id="ejemplo3">
                                                    <ul class="navbar-nav mx-auto contenedor-icono">

                                                        <div class="contenedor-icono">



                                                            <a class="icong disabled shadow-sm btn btn-sm" href="#" title="Nueva Detalle" id="NuevoDetalle" onclick="NuevoDetalle()">
                                                                <i class="bi bi-file-earmark-check-fill"></i>
                                                            </a>

                                                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm" runat="server" title="Importar Detalle de la Solicitud de Origen" ID="ImportarDetalle" OnClick="ImportarDetalle_Click">
                                                            <i class="bi bi-arrow-down-circle-fill"></i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm" runat="server" title="Guardar Detalle" ID="GrabarDetalle" OnClick="GuardarModificarDetalle" OnClientClick="return validarFormularioDetalle();">
                                                       <i class="bi bi-floppy-fill"></i>
                                                            </asp:LinkButton>

                                                            <a class="icong disabled shadow-sm btn btn-sm" href="#" title="Modificar Detalle" id="ModificarDetalle" onclick="ModificarDetalle()">
                                                                <i class="bi bi-wrench-adjustable"></i>
                                                            </a>

                                                            <a class="icong disabled shadow-sm btn btn-sm" title="Documentacion Producto" id="Documentacion" runat="server" onclick="abrirOtraPestana2();">
                                                                <i class="bi bi-paperclip"></i>
                                                            </a>


                                                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm" title="RedirigirCompras" ID="RedirigirCompras" runat="server" OnClick="RedirigirCompras_Click">
                                                         <i class="bi bi-cart4"></i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm" title="Ok compras" ID="OkCompras" runat="server">
                                                        <i class="bi bi-arrow-down-left-circle"></i>
                                                            </asp:LinkButton>

                                                            <asp:LinkButton class="icong disabled shadow-sm btn btn-sm" title="Quitar Detalle " ID="QuitarDetalle" runat="server" OnClick="EliminarDetalle" OnClientClick="return confirmarQuitarDetalle(event);">
                                                      <i class="bi bi-dash-circle-fill"></i>
                                                            </asp:LinkButton>

                                                            <asp:Label ID="lbIdDetalle" runat="server" Text=""></asp:Label>

                                                            <ul />
                                                    </ul>
                                                    <span id="ErrorValidacionDetalle" style="color: red;"></span>


                                                </div>

                                            </div>
                                        </nav>

                                        <div class=" input-group input-group-sm  mt-2 gap-2 justify-content-center">
                                            <asp:Button class=" btn btn-warning  " ID="btnProgramarSolicitud" runat="server" Text="Programar" OnClick="ProgramarSolicitud" OnClientClick="return confirmProgramarSolicitud(event);" />
                                        </div>
                                    </div>


                                </div>

                            </div>

                            <!--Modal confirmar Importar -->
                            <div id="confirmarImportar" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-success text-white">
                                            <h5 class="modal-title text-center">Importar  Detalle</h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Esta seguro de importar los detalles de esta solicitud?  </h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnImportar" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-success" Style="width: 5rem;" OnClick="btnImportar_Si_Click" />
                                                <asp:Button runat="server" ID="btnClose" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClick="btnClose_Click" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal confirmar Desarrollo Complejo  -->
                            <div id="confirDesCompl" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Desarrolo Complejo </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea cambiar el estado complejo de la solicitud
                                                <br />
                                                    N°  <span runat="server" id="SpanId_sol"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnConfComlplejo_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnConfComlplejo_SI_Click" />
                                                <asp:Button runat="server" ID="Button4" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal confirmar Urgente  -->
                            <div id="confirSolUrgente" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Urgente </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea cambiar el estado urgente de la solicitud
                                                <br />
                                                    N°  <span runat="server" id="SpanId_Sol_Urg"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnConUrg" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnConUrg_Click" />
                                                <asp:Button runat="server" ID="Button6" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal confirmar Devolver Solicitud  -->
                            <div id="ConfirmarDevolSoli" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Devolver Solicitud </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea devolver la solicitud N°  <span runat="server" id="Span_Id_Sol1"></span>
                                                    <br />
                                                    al proceso de ventas ?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnDevolverSolicitud_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnDevolverSolicitud_SI_Click" />
                                                <asp:Button runat="server" ID="btnDevolver_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Cerrar Devolver  -->
                            <div id="modalCerrarDev" class="modal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-danger  text-white">
                                            <h5 class="modal-title text-center"><i class="bi bi-exclamation-circle" style="font-size: 1.5rem;"></i>Observación no grabada </h5>
                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>Para devolver la solicitud, es necesario registrar la observación.</h6>
                                            </div>
                                        </div>
                                        <div class="modal-footer justify-content-center">
                                            <asp:Button runat="server" ID="btnRedireccionar_Sol" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-danger " Style="width: 5rem;" OnClick="btnRedireccionar_Sol_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Observacion  -->
                            <div id="ObservacionDevolverDetener" class="modal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" style="display: none;">
                                <div class="modal-dialog modal-fullscreen">
                                    <div class="modal-content">

                                        <div class="modal-header p-0 text-white" style="background-color: #23273be6">
                                            <h5 class="modal-title text-center" style="padding-left: 2rem;">Observacion </h5>
                                            <asp:LinkButton ID="btnCerrarDevolver" data-bs-dismiss="modal" runat="server" aria-label="Close" Style="color: white !important; margin-right: 1.5rem; font-size: 1.8rem; text-decoration: none;" OnClick="btnCerrarDevolver_Click">
                                            <i class="bi bi-x-circle"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnCerrarDetener" data-bs-dismiss="modal" runat="server" aria-label="Close" Style="color: white !important; margin-right: 1.5rem; font-size: 1.8rem; text-decoration: none;" OnClick="btnCerrarDetener_Click">
                                            <i class="bi bi-x-circle"></i>
                                            </asp:LinkButton>

                                        </div>

                                        <div class="modal-body border rounded">

                                            <div class="container-fluid">

                                                <div class="container pt-2">

                                                    <div class="row pt-2 border p-3 rounded shadow g-2">

                                                        <div class="col-md-6 mt-2 p-3 border rounded  p-1">

                                                            <div class="row g-1 pb-2">

                                                                <div class="col-md-1">
                                                                    <div class="input-group input-group-sm ">
                                                                        <asp:Label ID="lbOt" CssClass="form-label fw-bold" runat="server" Text="OT: "></asp:Label>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-5">
                                                                    <div class="input-group input-group-sm ">
                                                                        <asp:TextBox ID="tbOt" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="input-group input-group-sm gap-2">
                                                                        <asp:Label ID="lbPed" CssClass="form-label fw-bold" runat="server" Text="Pedido: "></asp:Label>
                                                                        <asp:TextBox ID="tbPed" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="row g-1 pb-2 pt-2">

                                                                <div class="col-md-1">
                                                                    <div class="input-group input-group-sm">
                                                                        <asp:Label ID="lbObra" CssClass="form-label" runat="server" Text="Obra: "></asp:Label>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-11">
                                                                    <div class="input-group input-group-sm gap-2">

                                                                        <asp:TextBox ID="tbObra" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                                    </div>

                                                                </div>

                                                            </div>

                                                            <div class="row g-1 mt-2">

                                                                <div class="col-md-1">
                                                                    <div class="input-group input-group-sm">
                                                                        <asp:Label ID="Label2" runat="server" CssClass="form-label-sm fw-bold" Text="T.Obs."></asp:Label>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-7">
                                                                    <div class="input-group input-group-sm gap-2">

                                                                        <asp:DropDownList ID="ddlTipoObservacion" runat="server" CssClass="form-control form-control-sm" DataTextField="TipoObservacion" DataValueField="Id_TipoObservacion" DataSourceID="TipoObservacion" OnSelectedIndexChanged="ddlTipoObservacion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                                                        <asp:SqlDataSource runat="server" ID="TipoObservacion" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT 
                                                                                        Id_TipoObservacion, Aplicacion,Descripcion, Aplicacion + ' - ' + Descripcion as TipoObservacion,
                                                                                        DestinatarioPorDefecto,Programable, AlDirectorComercial
                                                                                        FROM tblTipoObservacion 
                                                                                        WHERE Aplicacion Like '%DEVOLUCIÓN SOLICITUD PE%' 
                                                                                        AND Activa = 1
                                                                                        ORDER BY Aplicacion ASC , Descripcion ASC"></asp:SqlDataSource>

                                                                    </div>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <div class="input-group input-group-sm gap-2">
                                                                        <asp:Label ID="Label3" runat="server" CssClass="col-form-label-sm" Text="F.Actividad"></asp:Label>
                                                                        <asp:TextBox ID="tbfechaActividad" runat="server" CssClass="form-control form-control-sm" type="date"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <h6>Observación</h6>
                                                                    <textarea id="txObservacion" runat="server" class="form-control form-control-sm" style="height: 15rem;"> </textarea>
                                                                </div>
                                                            </div>

                                                        </div>

                                                        <div class="col-md-6 p-2 mt-2 border">

                                                            <div class="" style="height: 35.5rem;">

                                                                <div class="border rounded p-1 special-border" style="max-height: 20rem; height: 22rem; overflow-x: auto;">
                                                                    <h6 class="datagrid-header text-center">Recptores de Correo</h6>
                                                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm p-1" ID="DataGridReceptorMail" DataSourceID="DSRecptores" runat="server" AutoGenerateColumns="false" OnItemCommand="DataGridReceptorMail_ItemCommand">
                                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                        <Columns>
                                                                            <asp:TemplateColumn HeaderText="...">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="VerMail" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" OnClientClick="CerrarModalDevolver();" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateColumn>
                                                                            <asp:BoundColumn HeaderText="Departamento/Cargo" DataField="Cargo" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                            <asp:BoundColumn HeaderText="Nombre" DataField="NombreCompleto" ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn HeaderText="" DataField="Mail" Visible="false" />
                                                                            <asp:BoundColumn HeaderText="" DataField="Cedula" Visible="false" />

                                                                        </Columns>
                                                                    </asp:DataGrid>

                                                                    <asp:SqlDataSource ID="DSRecptores" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL%>"
                                                                        SelectCommand="SELECT Cedula, Nombre + ' ' + Apellidos AS NombreCompleto, Cargo,Mail    FROM tblEmpleado
                                                                                   WHERE Activo = 1 AND ReceptorObservaciones = 1 ORDER BY Cargo ASC, Nombre ASC"></asp:SqlDataSource>

                                                                </div>

                                                                <div class="container-fluid pt-2 ">

                                                                    <div class="row pt-2 ">
                                                                        <div class="col-6">
                                                                            <asp:Label ID="Label4" runat="server" Text="Receptores por defecto" CssClass="col-form-label-sm fw-bold"></asp:Label>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row pt-2 ">
                                                                        <div class="col-12">
                                                                            <asp:TextBox ID="tbReceptorCorreo" ReadOnly="true" runat="server" CssClass=" form-control form-control-sm"></asp:TextBox>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row pt-2 ">
                                                                        <div class="col-12">
                                                                            <asp:TextBox ID="tbRecepTipoObs" ReadOnly="true" runat="server" CssClass=" form-control form-control-sm" placeHolder="Correos por tipo de observación"></asp:TextBox>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row pt-2 ">
                                                                        <div class="col-12">
                                                                            <asp:TextBox ID="tbCedulaRecp" runat="server" CssClass=" form-control form-control-sm" Visible="false"></asp:TextBox>
                                                                            <asp:TextBox ID="tbNombreRecp" runat="server" CssClass=" form-control form-control-sm" Visible="false"></asp:TextBox>
                                                                        </div>

                                                                    </div>

                                                                    <div class="row  mt-4">

                                                                        <div class="col-md-7">
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <asp:Button ID="BtnGrabarObservacion" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Grabar Observacion" OnClick="BtnGrabarObservacion_Click" />
                                                                            <asp:Button ID="btnGrabarObservacionDetener" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Grabar Observacion" OnClick="btnGrabarObservacionDetener_Click" />
                                                                        </div>
                                                                    </div>


                                                                </div>

                                                            </div>

                                                        </div>

                                                    </div>

                                                </div>

                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>

                            <!--Modal confirmar Pausar Solicitud  -->
                            <div id="ConfirmarPausarSol" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                                <div class="modal-dialog modal-lg modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Pausar desarrollo  </h5>
                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <div class="row pb-2">
                                                    <div class="col-sm-12">
                                                        <div class="input-group-sm gap-2">
                                                            <asp:Label ID="lbJusti" runat="server" Text="Razón de la pausa:"></asp:Label>
                                                            <textarea class="form-control form-control-sm" id="txJustificacionPausa" runat="server" cols="25" rows="5"></textarea>
                                                        </div>
                                                    </div>
                                                </div>

                                                <h6>Por favor justifique la causa de la pausa de la solicitid y presione aceptar </h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnPausar_Si" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnPausar_Si_Click" />
                                                <asp:Button runat="server" ID="btnPausar_No" Text="Cancelar" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal confirmar Despausar Solicitud  -->
                            <div id="ConfirmarDespausarSol" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Despausar Solicitud </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea Despausar la solicitud N°  <span runat="server" id="Span_Id_Sol3"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnDespausar_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnDespausar_SI_Click" />
                                                <asp:Button runat="server" ID="Button8" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal confirmar Detener Solicitud  -->
                            <div id="ConfirmarDetenerSol" class="modal" tabindex="-1" style="display: none;" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                                <div class="modal-dialog  modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Detener desarrollo  </h5>
                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Esta seguro de detener la solicitud <span runat="server" id="Span_Id_Sol4"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnDetenerPE_SI" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnDetenerPE_SI_Click" />
                                                <asp:Button runat="server" ID="btnDetenerPE_NO" Text="Cancelar" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Cerrar Devolver  -->
                            <div id="modalCerrarDet" class="modal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-danger  text-white">
                                            <h5 class="modal-title text-center"><i class="bi bi-exclamation-circle" style="font-size: 1.5rem;"></i>Observación no grabada </h5>
                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>Para detener la solicitud, es necesario registrar la observación.</h6>
                                            </div>
                                        </div>
                                        <div class="modal-footer justify-content-center">
                                            <asp:Button runat="server" ID="Button1" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-danger " Style="width: 5rem;" OnClick="btnRedireccionar_Sol_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <!--Modal confirmar Redirigir a compras -->
                            <div id="modalRedirigirCompras" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Redirección  compras </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Esta seguro de redirigir el detalle para compras?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnRedirigir_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnRedirigir_SI_Click" />
                                                <asp:Button runat="server" ID="btnRedirigir_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>


                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <div class="tab-pane fade" id="Programacion-content">
                <asp:UpdatePanel ID="PanelProgamacion" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid m-2 ">

                            <div class="card m-3">

                                <div class="card-header" style="height: 2.9rem; background: repeating-radial-gradient(#fff,#eeebebe6)" id="headerDes" runat="server">
                                    <%-- Desarrollo--%>
                                    <div class="row pb-2">

                                        <div class="col-md-2 text-end">

                                            <asp:TextBox ID="tbNombreAsesor" type="text" class="form-control form-control-sm" CssClass="hidden-textBox" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4 text-center">
                                            <h5 id="tituloDes" runat="server" visible="false">Desarrollos</h5>
                                        </div>

                                        <div class="col-md-6 " id="BusDesDiv" runat="server">
                                            <div class="input-group input-group-sm gap-2 justify-content-end">
                                                <asp:TextBox ID="ID_Sol_Dib" CssClass="form-control form-control-sm  text-center fw-bold" Style="width: 15rem; max-width: 15rem;" placeHolder="N° Desarrollo" ToolTip="Digite la solcitud que desea buscar " runat="server" OnTextChanged="ID_Sol_Dib_TextChanged"></asp:TextBox>
                                                <asp:LinkButton runat="server" Text="Buscar" CssClass="icong enabled shadow-sm btn btn-sm AzulActivo rounded" ID="BuscarSol" title="Buscar Solicitud" Style="width: 2rem; font-size: 1.1rem;" OnClick="BuscarSol_Click">
                                                        <i class="bi bi-search"></i>
                                                </asp:LinkButton>
                                                <asp:CheckBox ID="chkVerDes" runat="server" ToolTip="Ocultar desarrollos" OnCheckedChanged="chkVerDes_CheckedChanged" AutoPostBack="true" />
                                            </div>
                                        </div>


                                    </div>
                                </div>

                                <div class="card-body shadow-sm" id="bodyDes" runat="server">


                                    <%-- Fila control Click--%>
                                    <div class="row" style="display: none;">
                                        <div class="col-1">
                                            <asp:TextBox ID="filaAntior" type="text" class="form-control form-control-sm" placeHolder="fila anterior" runat="server"></asp:TextBox>

                                        </div>

                                        <div class="col-1">
                                            <asp:TextBox ID="filaActual" type="text" class="form-control form-control-sm" placeHolder="fila Actual" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-1">
                                            <asp:TextBox ID="ContadorClic" type="text" class="form-control form-control-sm" placeHolder="Contador" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-1">
                                            <asp:TextBox ID="tbId_Fila" type="text" class="form-control form-control-sm" placeHolder="Contador" runat="server"></asp:TextBox>
                                        </div>

                                    </div>

                                    <div class="row pb-2">

                                        <div class="col-md-5">
                                            <div class=" input-group input-group-sm justify-content-around  mb-2" style="padding-left: 3rem;">
                                                <asp:Button ID="btnTrabajarSolicitud" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Trabajar Solicitud" OnClick="btnTrabajarSolicitud_Click" />
                                                <asp:Button ID="btnDesprogramar" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Desprogramar" OnClick="btnDesprogramar_Click" />
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="input-group  input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbFechaPactoentrega" class=" col-form-label-sm" Text="Pacto Entrega" runat="server"></asp:Label>
                                                <asp:TextBox ID="tbFechaPactoentrega" type="date" class="form-control " runat="server"></asp:TextBox>

                                            </div>
                                        </div>

                                        <div class="col-md-2 justify-content-center">
                                            <div class="input-group  input-group-sm  mb-2 gap-2">
                                                <asp:CheckBox ID="chxConvenciones" OnCheckedChanged="chxConvenciones_CheckedChanged" AutoPostBack="true" runat="server" />
                                                <asp:Label ID="lbConvenciones" class=" col-form-label-sm" Text="Convenciones" runat="server"></asp:Label>


                                            </div>
                                        </div>

                                        <div class="col-md-2" style="padding-right: 4rem;">
                                            <div class="input-group  input-group-sm  mb-2 gap-4">
                                                <asp:Label ID="lbZona" class="form-label" Text="Zona" runat="server"></asp:Label>
                                                <asp:DropDownList class="form-control" ID="ddlZona" runat="server" DataSourceID="Zona" DataTextField="Zona" DataValueField="Zona" OnSelectedIndexChanged="CambioZona" AutoPostBack="true" OnDataBound="ddlZona_DataBound"></asp:DropDownList>
                                                <asp:SqlDataSource runat="server" ID="Zona" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="
                                        select Zona from tblRender group by Zona"></asp:SqlDataSource>

                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-lg-2 mb-lg-2 p-2 justify-content-center">
                                        <div class="border border-2 rounded p-2">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="row">
                                                    </div>
                                                    <div class="table-responsive mb-2 gap-2" style="max-height: 15rem; height: 15rem; overflow-x: auto;">

                                                        <h5 class="datagrid-header text-Start" style="padding-left: 1rem;">Desarrollos</h5>

                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid1" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="CargarDesarrollos" OnItemDataBound="DataGridDesarrollo_ItemDataBound" OnItemCommand="DataGridSolicitudPE_LinkButton">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText=". . .">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" runat="server" CommandName="VerDesarrollo" CssClass="Tam link-button" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>"
                                                                            OnClientClick='<%# "return function() { return activarTab(\"BitacoraDesarrollo-content\", \"" + Eval("ID_Solicitud") + "\"); }();" %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:BoundColumn DataField="ID_Solicitud" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Proyecto" HeaderText="Proyecto" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Ingreso" HeaderText="Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Dirigidoa" HeaderText="Para" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="TipoSolicitud" HeaderText="Tipo de Solicitud	" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ProgramadoVentas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Pausado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <%-- Columnas para mostrar en el datagrid Empieza desde el 11 --%>
                                                                <asp:BoundColumn DataField="Fecha_Programada_Entrega" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="FechaRespuesta" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="id_SolicitudOrigen" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="CiudadProyecto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="CotizarViaTte" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Cotizacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Cliente" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Contacto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Telefono" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Celular" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Mail" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Direccion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="SeguimientoPausa" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="DesarrolloComplejo" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Urgente" Visible="false" ItemStyle-CssClass="auto-width-column" />

                                                            </Columns>
                                                        </asp:DataGrid>

                                                        <asp:SqlDataSource runat="server" ID="Desarrollo" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="spObtenerSolicitudesDiseEspe" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddlZona" PropertyName="SelectedValue" Name="Zona" Type="String"></asp:ControlParameter>
                                                                <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                        <asp:SqlDataSource ID="CargarDesarrollos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="   SELECT *
                                                        FROM tblSoliciDiseEspe  WHERE Terminado = 0  AND TipoSolicitud ='DESARROLLO' AND Asesor =@Asesor ORDER BY Fecha_Ingreso ASC;">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                        <asp:SqlDataSource ID="SolUnica" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblSoliciDiseEspe
                                                      WHERE Terminado = 0  AND Dirigidoa='DESARROLLO DE PRODUCTO'  AND  ProgramadoVentas = 1  
                                                       AND TipoSolicitud ='DESARROLLO' And ID_Solicitud = @IdSolcicitud ORDER BY Fecha_Ingreso ASC ">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ID_Sol_Dib" PropertyName="Text" Name="IdSolcicitud"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>





                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="card m-3">

                                <div class="card-header" style="height: 2.9rem; background: repeating-radial-gradient(#fff,#eeebebe6)" id="headerCot" runat="server">

                                    <div class="row pb-2">

                                        <div class="col-md-2"></div>

                                        <div class="col-md-4 text-center">
                                            <h5 id="tituloCot" runat="server" visible="false">Cotizaciones</h5>
                                        </div>

                                        <div class="col-md-6 " id="BusCotDiv" runat="server">
                                            <div class="input-group input-group-sm gap-2 justify-content-end">
                                                <asp:TextBox ID="ID_Cot_Dib" CssClass="form-control form-control-sm fw-bold  text-center" Style="width: 15rem; max-width: 15rem;" placeHolder="N° Cotización" ToolTip="Digite la solcitud que desea buscar " runat="server" OnTextChanged="ID_Cot_Dib_TextChanged"></asp:TextBox>
                                                <asp:LinkButton runat="server" CssClass="icong enabled shadow-sm btn btn-sm AzulActivo rounded" Text="Buscar" ID="BuscarCot" title="Buscar Solicitud" Style="width: 2rem; font-size: 1rem;" OnClick="BuscarCot_Click">
                                                  <i class="bi bi-search"></i>
                                                </asp:LinkButton>
                                                <asp:CheckBox ID="chkVerCot" runat="server" ToolTip="Ocultar cotizaciones" OnCheckedChanged="chkVerCot_CheckedChanged" AutoPostBack="true" />
                                            </div>
                                        </div>

                                    </div>

                                </div>

                                <div class="card-body shadow-sm" id="bodyCot" runat="server">

                                    <%-- cotizaciones--%>

                                    <div class="row pb-2">

                                        <div class="col-md-5 text-center" style="padding-left: 3rem;">
                                            <div class=" input-group input-group-sm justify-content-around  mb-2 gap-2">
                                                <asp:Button ID="btnTrbajarCotizacion" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Trabajar Cotización" OnClick="btnTrbajarCotizacion_Click" />
                                                <asp:Button ID="btnDesprogramar1" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Desprogramar" OnClick="btnDesprogramar1_Click" />
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="input-group  input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbPactoEntrega1" class=" col-form-label-sm" Text="Pacto Entrega" runat="server"></asp:Label>
                                                <asp:TextBox ID="tbPactoEntrega" type="date" class="form-control " runat="server"></asp:TextBox>

                                            </div>
                                        </div>

                                        <div class="col-1"></div>

                                        <div class="col-md-3">
                                        </div>



                                    </div>

                                    <div class="row justify-content-center p-2">
                                        <div class="border border-2 rounded p-2">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="table-responsive mb-2 gap-2" style="max-height: 15rem; height: 15rem; overflow-x: auto;">
                                                        <h5 class="datagrid-header text-start" style="padding-left: 1rem;">Cotizaciones</h5>

                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid2" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="CargarCotizaciones" OnItemDataBound="DataGridCotizacion_ItemDataBound" OnItemCommand="DataGridSolicitudPE_LinkButton">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText=". . .">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" runat="server" CommandName="VerCotizacion" CssClass="Tam" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>"
                                                                            OnClientClick='<%# "return function() { return activarTab(\"BitacoraDesarrollo-content\", \"" + Eval("ID_Solicitud") + "\"); }();" %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>


                                                                <asp:BoundColumn DataField="ID_Solicitud" HeaderText="ID" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Proyecto" HeaderText="Proyecto" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Ingreso" HeaderText="Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Dirigidoa" HeaderText="Para" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="TipoSolicitud" HeaderText="Tipo de Solicitud	" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="ProgramadoVentas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Pausado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <%-- Columnas para mostrar en el datagrid Empieza desde el 11 --%>
                                                                <asp:BoundColumn DataField="Fecha_Programada_Entrega" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="FechaRespuesta" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="id_SolicitudOrigen" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="CiudadProyecto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="CotizarViaTte" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Cotizacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Cliente" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Contacto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Telefono" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Celular" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Mail" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Direccion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="SeguimientoPausa" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="DesarrolloComplejo" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Urgente" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                            </Columns>
                                                        </asp:DataGrid>

                                                        <asp:SqlDataSource runat="server" ID="Cotizaciones" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="spObtenerSolicitudCotizaciones" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddlZona" PropertyName="SelectedValue" Name="Zona" Type="String"></asp:ControlParameter>
                                                                <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                        <asp:SqlDataSource ID="CargarCotizaciones" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="   SELECT *
                                                        FROM tblSoliciDiseEspe  WHERE Terminado = 0  AND TipoSolicitud ='COTIZACIÓN' AND Asesor =@Asesor ORDER BY Fecha_Ingreso ASC;">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                        <asp:SqlDataSource ID="CotUnica" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblSoliciDiseEspe
                                                      WHERE Terminado = 0  AND Dirigidoa='DESARROLLO DE PRODUCTO'  AND  ProgramadoVentas = 1  
                                                       AND TipoSolicitud ='COTIZACIÓN' And ID_Solicitud = @IdSolcicitud ORDER BY Fecha_Ingreso ASC ">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ID_Cot_Dib" PropertyName="Text" Name="IdSolcicitud"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>


                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <!--Modal Trabajar en Desarrollo  -->
                            <div id="confirTrabaSol" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Trabajar en Desarrollo </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea trabajar en la  solicitud número:   <span runat="server" id="NumSol"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnTraSol_Si" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnTraSol_Si_Click" />
                                                <asp:Button runat="server" ID="Button2" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Desprogramar en Desarrollo  -->
                            <div id="confirDespSol" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Desprogramar Desarrollo </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea Desprogramar la  solicitud número:   <span runat="server" id="NumSol1"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnDesprogramar_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnDesprogramar_SI_Click" />
                                                <asp:Button runat="server" ID="Button3" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Trabajar en Cotización  -->
                            <div id="confirTrabaCot" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Trabajar en Cotización </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea trabajar en la  Cot número:   <span runat="server" id="NumSol2"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnProCot_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnProCot_SI_Click" />
                                                <asp:Button runat="server" ID="Button5" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Desprogramar en Desarrollo  -->
                            <div id="confirDespCot" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-primary text-white">
                                            <h5 class="modal-title text-center">Desprogramar Cotización </h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>¿ Desea Desprogramar la  Cotización número:   <span runat="server" id="NumSol3"></span>?</h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnDesCot_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="btnDesCot_SI_Click" />
                                                <asp:Button runat="server" ID="Button7" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Convenciones   -->
                            <div class="modal fade" id="myModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header text-white" style="background-color: #23273be6">
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
                                                                <label class="form-label">No programado por Ventas</label>
                                                            </div>


                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group" style="width: 20px; height: 20px; border: 1px; background-color: #c86868"></div>
                                                                <label class="form-label">No cumplidos y en espera</label>
                                                            </div>


                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #efdd79"></div>
                                                                <label class="form-label">Programación Normal</label>
                                                            </div>


                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #e9a270"></div>
                                                                <label class="form-label">Urgente</label>
                                                            </div>
                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #70ede4"></div>
                                                                <label class="form-label">Pausados</label>
                                                            </div>
                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #77a765"></div>
                                                                <label class="form-label">Desarrollo Complejo </label>
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
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>


            <div class="tab-pane fade " id="BuscarDesarrollo-content">
                <asp:UpdatePanel ID="PanelBuscar" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid m-2">

                            <%-- Buscar Diseño Especial --%>

                            <div class="container-fluid border shadow-sm  rounded-1 bg-light pb-3 mb-3">
                                <div class="row pt-2">
                                    <div class="col-lg-8 col-xs-12">
                                        <div class="input-group input-group-sm  mb-2 gap-3">
                                            <asp:Label ID="lbFechaIni" class="form-label" Text="Fecha Ingreso" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbFechaIni" type="date" runat="server" class="form-control"></asp:TextBox>
                                            <asp:Label ID="lbY" class="form-label" Text=" Y " runat="server"></asp:Label>
                                            <asp:TextBox ID="tbFechaFin" type="date" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-xs-12">
                                        <div class="input-group input-group-sm  mb-1 ">
                                            <asp:Button ID="btnConsultar" type="button" Text="Consultar" class="btn btn-outline-secondary" runat="server" OnClick="ConsultarSolicitud"></asp:Button>
                                        </div>
                                    </div>
                                </div>

                                <div class="row p-1">
                                    <div class="col-lg-1 col-md-2">
                                        <div class="input-group input-group-sm  mb-1 ">
                                            <asp:Label ID="lbProyectoX" class="form-label" Text="Proyecto" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-sm-6">
                                        <div class="input-group input-group-sm  mb-1 ">
                                            <asp:TextBox ID="tbProyectoX" type="text" class="form-control " runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row p-1">
                                    <div class="col-lg-1 col-md-2">
                                        <div class="input-group input-group-sm  mb-1 ">
                                            <asp:Label ID="lbClienteX" class="form-label" Text="Cliente" runat="server"></asp:Label>

                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-sm-6">
                                        <div class="input-group input-group-sm  mb-1">

                                            <asp:TextBox ID="tbClienteX" type="text" class="form-control " runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row p-1">
                                    <div class="col-lg-1 col-md-2">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:Label ID="lbNumeroSolicitud1" class="form-label" Text="Solicitud N." runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-sm-6">
                                        <div class="input-group input-group-sm  mb-2 gap-2">
                                            <asp:TextBox ID="tbSolicitud1" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row justify-content-center  p-1 m-1  border shadow rounded ">
                                <div class="border rounded ">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 25rem; height: 25rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Solicitudes Filtradas</h5>

                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="BuscarDesarrollo" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridBuscarDesarrollo_ItemDataBound" OnItemCommand="DataGridSolicitudPE_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>


                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerBuscado" CssClass="Tam" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>"
                                                                    OnClientClick='<%# "return function() { return activarTab(\"BitacoraDesarrollo-content\", \"" + Eval("ID_Solicitud") + "\"); }();" %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:BoundColumn DataField="ID_Solicitud" HeaderText="ID" />
                                                        <asp:BoundColumn DataField="Proyecto" HeaderText="Proyecto" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Ingreso" HeaderText="Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Dirigidoa" HeaderText="Para" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="TipoSolicitud" HeaderText="Tipo de Solicitud	" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ProgramadoVentas" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Pausado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Terminado" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <%-- Columnas para mostrar en el datagrid Empieza desde el 11 --%>
                                                        <asp:BoundColumn DataField="Fecha_Programada_Entrega" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="FechaRespuesta" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="id_SolicitudOrigen" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="CiudadProyecto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="CotizarViaTte" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cotizacion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cliente" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Contacto" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Telefono" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Celular" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Mail" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Direccion" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="SeguimientoPausa" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DesarrolloComplejo" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Urgente" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>

                                                </asp:DataGrid>
                                                <asp:SqlDataSource ID="SolicXFecha" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand=" SELECT *
                                                                             FROM tblSoliciDiseEspe WHERE Asesor = @Asesor AND Fecha_Ingreso between @FechaIni and @FechaFin  ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="SolicXProyecto" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand=" SELECT * 
                                                                       FROM tblSoliciDiseEspe  WHERE Asesor = @Asesor AND  Proyecto  LIKE '%' + @Proyecto + '%' AND Fecha_Ingreso between @FechaIni and @FechaFin ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbProyectoX" PropertyName="Text" Name="Proyecto"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="SolicitudXID" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand=" SELECT * 
                                                                       FROM tblSoliciDiseEspe  WHERE Asesor = @Asesor AND  ID_Solicitud  LIKE '%' + @Solicitud + '%' AND Fecha_Ingreso between @FechaIni and @FechaFin ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbSolicitud1" PropertyName="Text" Name="Solicitud"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="solicitudXCliente" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand=" SELECT * 
                                                                   FROM tblSoliciDiseEspe  WHERE Asesor = @Asesor AND  Cliente  LIKE '%' + @Cliente + '%' AND Fecha_Ingreso between @FechaIni and @FechaFin ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbClienteX" PropertyName="Text" Name="Cliente"></asp:ControlParameter>
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

            // Quitar 'active' de la pestaña actualmente activa y su contenido
            $('#Programacion-tab').removeClass('active');
            $('#Programacion-content').removeClass('active show');

            // Activa la pestaña de Programación
            $('#BitacoraDesarrollo-tab').addClass('active');
            $('#BitacoraDesarrollo-content').addClass('active show');

            ControlHeaderCard();

            // se Habilitan enlaces Iniciales 
            document.getElementById("NuevaSolicitud").classList.remove("disabled");
            document.getElementById("NuevaSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("Observaciones").classList.remove("disabled");
            document.getElementById("Observaciones").classList.add("enabled", "AzulActivo");

            document.getElementById("CancelarSolicitud").classList.remove("disabled");
            document.getElementById("CancelarSolicitud").classList.add("enabled", "RojoCancelar");

            // Control del boton  nuevo y modificar 
            var controlBotones = '<%= Session["controlBotones"] %>';

            if (controlBotones === "1") {

                // Control del boton  nuevo y modificar 
                var nuevasol = '<%= Session["nuevaSol"] %>';

                if (nuevasol === "1") {
                    NuevaSolicitud1();

                    $.ajax({
                        type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                        url: "Solicitud_Especial.aspx/NuevaSolicitud1", // La URL debe apuntar al método en el servidor
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
                else if (nuevasol === "2") {
                    ModificarSolicitud();
                    $.ajax({
                        type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                        url: "Solicitud_Especial.aspx/ModificarSolicitud1", // La URL debe apuntar al método en el servidor
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            // La llamada al servidor fue exitosa, puedes realizar acciones adicionales aquí
                        },
                        error: function (error) {
                            // Manejar errores si los hay
                        }
                    });;
                }
            } else {
                $.ajax({
                    type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                    url: "Solicitud_Especial.aspx/LimpiarSessionError", // La URL debe apuntar al método en el servidor
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        // La llamada al servidor fue exitosa, puedes realizar acciones adicionales aquí
                    },
                    error: function (error) {
                        // Manejar errores si los hay
                    }
                });;
            }



        }
        else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {

            // Quitar 'active' de la pestaña actualmente activa y su contenido
            $('#BitacoraDesarrollo-tab').removeClass('active');
            $('#BitacoraDesarrollo-content').removeClass('active show');

            // Activa la pestaña de Programación
            $('#Programacion-tab').addClass('active');
            $('#Programacion-content').addClass('active show');

            $(".contenedor-icono").hide();

            // se Habilitan enlaces Iniciales

            document.getElementById("Observaciones").classList.remove("disabled");
            document.getElementById("Observaciones").classList.add("enabled", "AzulActivo");

            document.getElementById("CancelarSolicitud").classList.remove("disabled");
            document.getElementById("CancelarSolicitud").classList.add("enabled", "RojoCancelar");


            //Control de Dropdownlist
            var Lista = ["ddlDirigido", "ddlCiudad", "ddlTipo", "ddlAsesor"];
            ControlDesplegables(Lista)

            // Control Boton cliente 
            ControlBtnCliente();




        }


        // Obtener la fecha actual
        const fechaActual = new Date();

        // Obtener el primer día del mes actual
        const primerDiaMes = new Date(fechaActual.getFullYear(), fechaActual.getMonth(), 1);
        const primerDiaMesFormateado = primerDiaMes.toISOString().slice(0, 10); // Formato: YYYY-MM-DD

        // Formatear la fecha actual en formato "YYYY-MM-DD"
        const fechaFormateada = fechaActual.toISOString().slice(0, 10);

        // Asignar las fechas a los campos  visitas entre y fecha 
        document.getElementById("tbFechaIni").value = primerDiaMesFormateado;

        document.getElementById("tbFechaFin").value = fechaFormateada;

        document.getElementById("btnCliente").disabled = true;

        function NuevaSolicitud() {


            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = false;
                }

            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbProveedor" && textBoxes[i].id !== "tbAncho" && textBoxes[i].id !== "tbAltura" && textBoxes[i].id !== "tbProfundidad"
                    && textBoxes[i].id !== "tbMaterial" && textBoxes[i].id !== "tbCliente" && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbTelefono"
                    && textBoxes[i].id !== "tbCelular" && textBoxes[i].id !== "tbMail" && textBoxes[i].id !== "tbDireccion" && textBoxes[i].id !== "tbPrecioSugerido"
                    && textBoxes[i].id !== "tbCantidad" && textBoxes[i].id !== "tbDesarrollaPor") {
                    textBoxes[i].disabled = false;


                }

            }

            var checkBoxesToEnable = ["chxViaticos"];

            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = false; // Habilita el CheckBox
                }
            }


            // Obtén la fecha actual
            var fechaActual = new Date();
            // Formatea las fechas en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaActualFormateada = fechaActual.toISOString().split('T')[0];

            var FechaActualAnio = new Date();

            // Establece la fecha al primer día del año actual
            FechaActualAnio.setMonth(0); // Establece el mes a enero (0)
            FechaActualAnio.setDate(1); // Establece el día al primero (1)
            // Formatea la fecha en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaFormateada2 = FechaActualAnio.toISOString().split('T')[0];




            // Asigna las fechas a los TextBox correspondientes por su ID
            document.getElementById("tbFechaIngreso").value = fechaActualFormateada;
            document.getElementById("tbFechaIngresoServidor").value = fechaActualFormateada;

            document.getElementById("tbFechaEntrega").value = fechaFormateada2;
            document.getElementById("tbFechaEntregaServidor").value = fechaFormateada2;

            document.getElementById("tbFechaRespuesta").value = fechaFormateada2;
            document.getElementById("tbFechaRespuestaServidor").value = fechaFormateada2;


            // Deshabilitar enlaces y Habilitar
            document.getElementById("NuevaSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("NuevaSolicitud").classList.add("disabled",);

            document.getElementById("ModificarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("ModificarSolicitud").classList.add("disabled");

            document.getElementById("GrabarSolicitud").classList.remove("disabled",);
            document.getElementById("GrabarSolicitud").classList.add("enabled", "AzulActivo");


            //Habilitar
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = false;


            // Limpiar el contenido del label
            var label = document.getElementById("lbNumeroSolicitud");
            label.textContent = "";



            $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Solicitud_Especial.aspx/NuevaSolicitud", // La URL debe apuntar al método en el servidor
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

        function NuevaSolicitud1() {


            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = false;
                }

            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbProveedor" && textBoxes[i].id !== "tbAncho" && textBoxes[i].id !== "tbAltura" && textBoxes[i].id !== "tbProfundidad"
                    && textBoxes[i].id !== "tbMaterial" && textBoxes[i].id !== "tbCliente" && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbTelefono"
                    && textBoxes[i].id !== "tbCelular" && textBoxes[i].id !== "tbMail" && textBoxes[i].id !== "tbDireccion" && textBoxes[i].id !== "tbPrecioSugerido"
                    && textBoxes[i].id !== "tbCantidad" && textBoxes[i].id !== "tbDesarrollaPor") {
                    textBoxes[i].disabled = false;


                }

            }

            var checkBoxesToEnable = ["chxViaticos"];

            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = false; // Habilita el CheckBox
                }
            }


            // Obtén la fecha actual
            var fechaActual = new Date();
            // Formatea las fechas en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaActualFormateada = fechaActual.toISOString().split('T')[0];

            var FechaActualAnio = new Date();

            // Establece la fecha al primer día del año actual
            FechaActualAnio.setMonth(0); // Establece el mes a enero (0)
            FechaActualAnio.setDate(1); // Establece el día al primero (1)
            // Formatea la fecha en el formato deseado (por ejemplo, YYYY-MM-DD)
            var fechaFormateada2 = FechaActualAnio.toISOString().split('T')[0];




            // Asigna las fechas a los TextBox correspondientes por su ID
            document.getElementById("tbFechaIngreso").value = fechaActualFormateada;
            document.getElementById("tbFechaIngresoServidor").value = fechaActualFormateada;

            document.getElementById("tbFechaEntrega").value = fechaFormateada2;
            document.getElementById("tbFechaEntregaServidor").value = fechaFormateada2;

            document.getElementById("tbFechaRespuesta").value = fechaFormateada2;
            document.getElementById("tbFechaRespuestaServidor").value = fechaFormateada2;


            // Deshabilitar enlaces y habilitar enlaces
            document.getElementById("NuevaSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("NuevaSolicitud").classList.add("disabled",);

            document.getElementById("ModificarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("ModificarSolicitud").classList.add("disabled");

            document.getElementById("GrabarSolicitud").classList.remove("disabled",);
            document.getElementById("GrabarSolicitud").classList.add("enabled", "AzulActivo");


            //Habilitar
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = false;





            $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Solicitud_Especial.aspx/NuevaSolicitud", // La URL debe apuntar al método en el servidor
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

        function CancelarSolicitud() {
            // Habilitar enlaces  y habilitar enlaces
            document.getElementById("NuevaSolicitud").classList.remove("disabled");
            document.getElementById("NuevaSolicitud").classList.add("enabled", "AzulActivado");

            document.getElementById("GrabarSolicitud").classList.remove("enabled", "AzulActivado");
            document.getElementById("GrabarSolicitud").classList.add("disabled");

            document.getElementById("ModificarSolicitud").classList.remove("enabled", "AzulActivado");
            document.getElementById("ModificarSolicitud").classList.add("disabled");



            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                    dropDownLists[j].value = "";
                }

            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {

                textAreas[k].value = "";
            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {
                if (textBoxes[i].id !== "tbNombreAsesor" && textBoxes[i].id != "tbProyectoX" && textBoxes[i].id !== "tbClienteX" && textBoxes[i].id != "tbSolicitud1") {
                    textBoxes[i].disabled = true;
                    textBoxes[i].value = "";
                }
            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='date']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].value = "";
            }


            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='Number']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].value = "";
            }


            var checkBoxesToEnable = ["chxViaticos", "chxDesComplejo"];

            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = true; // Habilita el CheckBox
                }
            }



            // Limpiar el contenido del label
            var label = document.getElementById("lbNumeroSolicitud");
            label.textContent = "";

            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;

            $.ajax({
                type: "POST",
                url: "Solicitud_Especial.aspx/Cancelar",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                error: function (error) {

                }
            });




        }

        function ModificarSolicitud() {


            // Habilitar enlaces deshabilitar enlaces
            document.getElementById("GrabarSolicitud").classList.remove("disabled");
            document.getElementById("GrabarSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("NuevaSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("NuevaSolicitud").classList.add("disabled");

            document.getElementById("ModificarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("ModificarSolicitud").classList.add("disabled");

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {
                dropDownLists[j].disabled = false;

            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbProveedor" && textBoxes[i].id !== "tbAncho" && textBoxes[i].id !== "tbAltura" && textBoxes[i].id !== "tbProfundidad"
                    && textBoxes[i].id !== "tbMaterial" && textBoxes[i].id !== "tbCliente" && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbTelefono"
                    && textBoxes[i].id !== "tbCelular" && textBoxes[i].id !== "tbMail" && textBoxes[i].id !== "tbDireccion" && textBoxes[i].id !== "tbPrecioSugerido"
                    && textBoxes[i].id !== "tbCantidad" && textBoxes[i].id !== "tbDesarrollaPor") {
                    textBoxes[i].disabled = false;


                }

            }
            var checkBoxesToEnable = ["chxViaticos"];

            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = false; // Habilita el CheckBox
                }
            }

            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = false;


            $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Solicitud_Especial.aspx/ModificarSolicitud", // La URL debe apuntar al método en el servidor
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

        function HabilEnla1Ventas() {

            // Habilitar y deshabilitar  enlaces de Detalle
            document.getElementById("NuevoDetalle").classList.remove("disabled");
            document.getElementById("NuevoDetalle").classList.add("enabled", "AzulActivo");

            document.getElementById("ImportarDetalle").classList.remove("disabled");
            document.getElementById("ImportarDetalle").classList.add("enabled", "AzulActivo");

            // Habilitar y deshabilitar Enlaces de la  Solcitud
            document.getElementById("NuevaSolicitud").classList.remove("disabled");
            document.getElementById("NuevaSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("ModificarSolicitud").classList.remove("disabled");
            document.getElementById("ModificarSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("GrabarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("GrabarSolicitud").classList.add("disabled");

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }
            }

            document.getElementById("txInformacionDetalle").value = "";

            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;

            ControlHeaderCard();

        }

        function HabEnlDiseño() {

            // Habilitar botones de DevolverSolicitud , btnProgramarSolicitud , ConfirmarComplejo,btnConUrgente
            document.getElementById("DevolverSolicitud").classList.remove("disabled");
            document.getElementById("DevolverSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("DetenerPE").classList.remove("enabled", "AzulActivo");
            document.getElementById("DetenerPE").classList.add("disabled");

            document.getElementById("PausarSolicitud").classList.remove("disabled");
            document.getElementById("PausarSolicitud").classList.add("enabled", "AzulActivo")


            // Se habilitan los CheckBox Urgente y Desarrollo complejo




            // Se controla el boton de cliente
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;

            // Controlamos los dropdownlist

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }
            }

            // Mostrar el LinkButton "PausarSolicitud"
            var DespausarSolicitud = document.getElementById("<%= DespausarSolicitud.ClientID %>");
            var PausarSolicitud = document.getElementById("<%= PausarSolicitud.ClientID %>");
            if (DespausarSolicitud) {
                DespausarSolicitud.style.display = 'none';
            }


            if (PausarSolicitud) {
                PausarSolicitud.style.display = '';
            }


        }

        function HabEnlDiseñoPausado() {

            // Habilitar botones de DevolverSolicitud , btnProgramarSolicitud , ConfirmarComplejo,btnConUrgente
            document.getElementById("DevolverSolicitud").classList.remove("disabled");
            document.getElementById("DevolverSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("DetenerPE").classList.remove("enabled", "AzulActivo");
            document.getElementById("DetenerPE").classList.add("disabled");

            document.getElementById("DespausarSolicitud").classList.remove("disabled");
            document.getElementById("DespausarSolicitud").classList.add("enabled", "AzulActivo")

            // Se habilitan los CheckBox Urgente y Desarrollo complejo




            // Se controla el boton de cliente
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;

            // Controlamos los dropdownlist

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }
            }

            // Mostrar el LinkButton "PausarSolicitud"
            var DespausarSolicitud = document.getElementById("<%= DespausarSolicitud.ClientID %>");
            var PausarSolicitud = document.getElementById("<%= PausarSolicitud.ClientID %>");

            if (DespausarSolicitud) {
                DespausarSolicitud.style.display = '';
            }

            if (PausarSolicitud) {
                PausarSolicitud.style.display = 'none';
            }


        }

        function HabEnlDiseño2() {

            // para controlar los botones cuando se traen los datos del datagrid
            document.getElementById("DetenerPE").classList.remove("disabled");
            document.getElementById("DetenerPE").classList.add("enabled", "AzulActivo");

            document.getElementById("DevolverSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("DevolverSolicitud").classList.add("disabled");


            document.getElementById("PausarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("PausarSolicitud").classList.add("disabled");

            document.getElementById("DespausarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("DespausarSolicitud").classList.add("disabled");

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }
            }

            // Se controla el boton de cliente
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;


        }

        function HabEnlDiseño3() {

            document.getElementById("DevolverSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("DevolverSolicitud").classList.add("disabled");

            document.getElementById("DetenerPE").classList.remove("enabled", "AzulActivo");
            document.getElementById("DetenerPE").classList.add("disabled");

            document.getElementById("PausarSolicitud").classList.remove("disabled");
            document.getElementById("PausarSolicitud").classList.add("enabled", "AzulActivo")

            // Se controla el boton de cliente
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;


            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }
            }

            var DespausarSolicitud = document.getElementById("<%= DespausarSolicitud.ClientID %>");
            if (DespausarSolicitud) {
                DespausarSolicitud.style.display = 'none';
            }



        }

        function HabEnlDiseño3Pausado() {

            document.getElementById("DevolverSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("DevolverSolicitud").classList.add("disabled");

            document.getElementById("DetenerPE").classList.remove("enabled", "AzulActivo");
            document.getElementById("DetenerPE").classList.add("disabled");

            document.getElementById("DespausarSolicitud").classList.remove("disabled");
            document.getElementById("DespausarSolicitud").classList.add("enabled", "AzulActivo")

            // Se controla el boton de cliente
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;


            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }
            }

            // Mostrar el LinkButton "PausarSolicitud"
            var DespausarSolicitud = document.getElementById("<%= DespausarSolicitud.ClientID %>");
            var PausarSolicitud = document.getElementById("<%= PausarSolicitud.ClientID %>");
            if (DespausarSolicitud) {
                DespausarSolicitud.style.display = 'none';
            }


            if (PausarSolicitud) {
                PausarSolicitud.style.display = '';
            }



        }

        function ControlBtnCliente() {
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;

        }

        function HabilitarEnlaces2() {

            // Habilitar enlaces
            document.getElementById("ModificarDetalle").classList.remove("disabled");
            document.getElementById("ModificarDetalle").classList.add("enabled", "AzulActivo");

            document.getElementById("Documentacion").classList.remove("disabled");
            document.getElementById("Documentacion").classList.add("enabled", "AzulActivo");

            document.getElementById("QuitarDetalle").classList.remove("disabled");
            document.getElementById("QuitarDetalle").classList.add("enabled", "RojoCancelar");

            document.getElementById("NuevoDetalle").classList.remove("disabled");
            document.getElementById("NuevoDetalle").classList.add("enabled", "AzulActivo");


            document.getElementById("ImportarDetalle").classList.remove("disabled");
            document.getElementById("ImportarDetalle").classList.add("enabled", "AzulActivo");


            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }

            }

            ControlHeaderCard();

        }

        function HabilitarEnlaces3() {

            // Habilitar enlaces
            document.getElementById("Documentacion").classList.remove("disabled");
            document.getElementById("Documentacion").classList.add("enabled", "AzulActivo");

            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }

            }

            ControlHeaderCard();

        }

        function HabilitarEnlaces4() {

            // Habilitar y deshabilitar Enlaces de la  Solcitud
            document.getElementById("NuevaSolicitud").classList.remove("disabled");
            document.getElementById("NuevaSolicitud").classList.add("enabled", "AzulActivo");

            document.getElementById("ModificarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("ModificarSolicitud").classList.add("disabled");

            document.getElementById("GrabarSolicitud").classList.remove("enabled", "AzulActivo");
            document.getElementById("GrabarSolicitud").classList.add("disabled");

            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    dropDownLists[j].disabled = true;
                }

            }

            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;

            ControlHeaderCard();
        }

        function abrirOtraPestana() {

            document.getElementById("tbCliente").value = "";
            // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
            window.open('Clientes.aspx', '_blank');

           
        }

        function NuevoDetalle() {

            // Habilitar Enlaces de la  Solcitud
            document.getElementById("GrabarDetalle").classList.remove("disabled");
            document.getElementById("GrabarDetalle").classList.add("enabled", "AzulActivo");



            // Deshabilitar enlaces de la solicitud
            document.getElementById("NuevoDetalle").classList.remove("enabled", "AzulActivo");
            document.getElementById("ImportarDetalle").classList.remove("enabled", "AzulActivo");
            document.getElementById("ModificarDetalle").classList.remove("enabled", "AzulActivo");
            document.getElementById("QuitarDetalle").classList.remove("enabled");
            document.getElementById("Documentacion").classList.remove("enabled", "AzulActivo");

            document.getElementById("NuevoDetalle").classList.add("disabled");
            document.getElementById("ImportarDetalle").classList.add("disabled");
            document.getElementById("ModificarDetalle").classList.add("disabled");
            document.getElementById("QuitarDetalle").classList.add("disabled");
            document.getElementById("Documentacion").classList.add("disabled");

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbProyecto" && textBoxes[i].id !== "tbSolicitudOrigen" && textBoxes[i].id !== "tbCotizacionEsp" && textBoxes[i].id !== "tbCliente" && textBoxes[i].id !== "tbClienteServidor" &&
                    textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbContactoServidor" && textBoxes[i].id !== "tbTelefono" && textBoxes[i].id !== "tbTelefonoServidor" && textBoxes[i].id !== "tbCelular" && textBoxes[i].id !== "tbCelularServidor" && textBoxes[i].id !== "tbMail" && textBoxes[i].id !== "tbMailServidor" && textBoxes[i].id !== "tbDireccion" && textBoxes[i].id !== "tbDireccionServidor" && textBoxes[i].id !== "tbPrecioSugerido") {
                    textBoxes[i].disabled = false;
                    textBoxes[i].value = "";

                }

            }


            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='number']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].disabled = false;
                textBoxes[i].value = "";
            }



            var checkBoxesToEnable = ["chxUrgente"];

            for (var i = 0; i < checkBoxesToEnable.length; i++) {
                var checkBoxId = checkBoxesToEnable[i];
                var checkBox = document.getElementById(checkBoxId);

                if (checkBox) {
                    checkBox.disabled = true; // Habilita el CheckBox
                }
            }


            // Limpiar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {

                if (textAreas[k].id != "txobsCompras" && textAreas[k].id != "txObsDesarrollo" && textAreas[k].id != "txInformacionDetalle" && textAreas[k].id != "txSegPausa") {
                    textAreas[k].disabled = false;
                    textAreas[k].value = "";
                }

            }

            $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Solicitud_Especial.aspx/NuevoDetalle", // La URL debe apuntar al método en el servidor
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

        function ModificarDetalle() {


            if (AreaDepar.toUpperCase() === "VENTAS") {
                // Habilitar Enlaces de la  Solcitud
                document.getElementById("GrabarDetalle").classList.remove("disabled");
                document.getElementById("GrabarDetalle").classList.add("enabled", "AzulActivo");


                // Deshabilitar enlaces de la solicitud

                document.getElementById("NuevoDetalle").classList.remove("enabled", "AzulActivo");
                document.getElementById("NuevoDetalle").classList.add("disabled");

                document.getElementById("ImportarDetalle").classList.remove("enabled", "AzulActivo");
                document.getElementById("ImportarDetalle").classList.add("disabled");

                document.getElementById("ModificarDetalle").classList.remove("enabled", "AzulActivo");
                document.getElementById("ModificarDetalle").classList.add("disabled");



                // Habilitar o deshabilitar los TextBox Type text
                var textBoxes = document.querySelectorAll("input[type='text']");
                for (var i = 0; i < textBoxes.length; i++) {

                    if (textBoxes[i].id !== "tbProyecto" && textBoxes[i].id !== "tbSolicitudOrigen" && textBoxes[i].id !== "tbCotizacionEsp" && textBoxes[i].id !== "tbCliente"
                        && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbTelefono" && textBoxes[i].id !== "tbDesarrollaPor"
                        && textBoxes[i].id !== "tbCelular" && textBoxes[i].id !== "tbMail" && textBoxes[i].id !== "tbDireccion" && textBoxes[i].id !== "tbPrecioSugerido") {
                        textBoxes[i].disabled = false;


                    }

                }


                // Habilitar o deshabilitar los TextBox Type text
                var textBoxes = document.querySelectorAll("input[type='number']");
                for (var i = 0; i < textBoxes.length; i++) {
                    textBoxes[i].disabled = false;
                }



                var checkBoxesToEnable = ["chxUrgente"];

                for (var i = 0; i < checkBoxesToEnable.length; i++) {
                    var checkBoxId = checkBoxesToEnable[i];
                    var checkBox = document.getElementById(checkBoxId);

                    if (checkBox) {
                        checkBox.disabled = true; // Habilita el CheckBox
                    }
                }


                // Limpiar los TextArea
                var textAreas = document.querySelectorAll("textarea");
                for (var k = 0; k < textAreas.length; k++) {

                    if (textAreas[k].id != "txobsCompras" && textAreas[k].id != "txObsDesarrollo" && textAreas[k].id != "txInformacionDetalle" && textAreas[k].id != "txSegPausa") {
                        textAreas[k].disabled = false;
                    }

                }
            } else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {


                // Control de botones de detalle 

                document.getElementById("GrabarDetalle").classList.remove("disabled");
                document.getElementById("GrabarDetalle").classList.add("enabled", "AzulActivo");

                document.getElementById("ModificarDetalle").classList.remove("enabled", "AzulActivo");
                document.getElementById("ModificarDetalle").classList.add("disabled");

                document.getElementById("RedirigirCompras").classList.remove("enabled", "AzulActivo");
                document.getElementById("RedirigirCompras").classList.add("disabled");


                // Constrol de Campos

                // Limpiar los TextArea
                var textAreas = document.querySelectorAll("textarea");
                for (var k = 0; k < textAreas.length; k++) {

                    if (textAreas[k].id != "txobsCompras" && textAreas[k].id != "txDescProduc" && textAreas[k].id != "txInformacionDetalle" && textAreas[k].id != "txSegPausa" && textAreas[k].id != "txEspGen") {
                        textAreas[k].disabled = false;
                    }

                }

                // Habilitar o deshabilitar los TextBox Type text
                var textBoxes = document.querySelectorAll("input[type='number']");
                for (var i = 0; i < textBoxes.length; i++) {

                    if (textBoxes[i].id !== "tbProyecto" && textBoxes[i].id !== "tbSolicitudOrigen" && textBoxes[i].id !== "tbCantidad") {
                        textBoxes[i].disabled = false;
                    }

                }



            }






            $.ajax({
                type: "POST", // Puede ser "GET" o "POST" según tus necesidades
                url: "Solicitud_Especial.aspx/ModificarDetalle", // La URL debe apuntar al método en el servidor
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

        function abrirOtraPestana2() {


            // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
            window.open('AdjuntarDocumentos.aspx', '_blank');

            // Validamos el Área del Usuario
            var AreaDepar = '<%= Session["Departamento"] %>';


            if (AreaDepar.toUpperCase() === "VENTAS") {
                ControlHeaderCard();
            } else {
                ControlBtnCliente();
            }



        }

        function abrirObservaciones() {

            // Obtener los valores de los textbox del DOM
            var solicitud1 = document.getElementById("lbNumeroSolicitud").innerText;
            var cliente1 = document.getElementById("tbClienteServidor").value;
            var proyecto1 = document.getElementById("tbProyectoServidor").value;

            // Realizar la petición AJAX
            $.ajax({
                type: "POST",
                url: "Solicitud_Especial.aspx/ObservacionesRedirect",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    solicitud: solicitud1,
                    cliente: cliente1,
                    proyecto: proyecto1
                }),
                success: function (response) {
                    // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
                    window.open('../FormExtPrin/ObservacionesOT.aspx', '_blank');
                },
                error: function (error) {
                    // Manejar errores si los hay
                }
            });




        }

        //Funcion para cuando seleccionan un desarrollo o una cotizacion nos lleva al formulario 
        function activarTab(tabId, IdSolicitud) {

            var AreaDepar = '<%= Session["Departamento"] %>';

            if (AreaDepar.toUpperCase() === "VENTAS") {
                $('#miPestañas a.Programacion-content').removeClass('active');
                $('.tab-pane').removeClass('active show');

                // Activa la pestaña deseada
                $('#miPestañas a[href="#' + tabId + '"]').tab('show');
            }
            else if (AreaDepar.toUpperCase() === "DISEÑO" || AreaDepar.toUpperCase() === "DESARROLLO DE PRODUCTO") {

                var cont = document.getElementById('ContadorClic').value;

                if (cont == "") {
                    document.getElementById('filaAntior').value = IdSolicitud;
                    document.getElementById('filaActual').value = IdSolicitud;

                    document.getElementById('ContadorClic').value = "1";


                } else if (cont == "1") {
                    document.getElementById('filaAntior').value = document.getElementById('filaActual').value;
                    document.getElementById('filaActual').value = IdSolicitud;


                    if (document.getElementById('filaAntior').value === document.getElementById('filaActual').value) {
                        document.getElementById('filaAntior').value = "";
                        document.getElementById('filaActual').value = "";
                        document.getElementById('ContadorClic').value = "";

                        $('#miPestañas a.Programacion-content').removeClass('active');
                        $('.tab-pane').removeClass('active show');

                        // Activa la pestaña deseada
                        $('#miPestañas a[href="#' + tabId + '"]').tab('show');

                    }
                    else {
                        document.getElementById('filaAntior').value = document.getElementById('filaActual').value;
                        document.getElementById('filaActual').value = IdSolicitud;
                        document.getElementById('ContadorClic').value = "1";
                    }

                }

            }
        }

        function validarFormularioSolicitud() {
            var proyecto = document.getElementById("tbProyecto").value;
            var dirigido = document.getElementById("ddlDirigido").value;
            var Tipo = document.getElementById("ddlTipo").value;
            var CotEsp = document.getElementById("tbCotizacionEsp").value;
            var Asesor = document.getElementById("ddlAsesor").value;
            var Cliente = document.getElementById("tbClienteServidor").value;
            var solicitudOrigen = document.getElementById("tbSolicitudOrigen").value;
            var regex = /^[0-9]+$/;
            var isValid = true;

            if (proyecto === "") {
                ErrorValidacion.innerHTML = "El campo proyecto es obligatorio.";
                isValid = false;
            } else if (dirigido === "") {
                ErrorValidacion.innerHTML = "El campo Dirigido a es obligatorio.";
                isValid = false;
            } else if (Tipo === "") {
                ErrorValidacion.innerHTML = "El campo Tipo Solicitud es obligatorio.";
                isValid = false;
            } else if (solicitudOrigen === "") {
                ErrorValidacion.innerHTML = "El campo  Solicitud Origen  es obligatorio.";
                isValid = false;
            } else if (!regex.test(solicitudOrigen)) {
                ErrorValidacion.innerHTML = "El campo Solicitud Origen debe ser un número.";
                isValid = false;
            }
            else if (CotEsp === "") {
                ErrorValidacion.innerHTML = "El campo Cotizacion Esp  es obligatorio.";
                isValid = false;
            }
            else if (Asesor === "0") {
                ErrorValidacion.innerHTML = "El Campo Asesor es obligatorio.";
                isValid = false;
            } else if (Cliente === "") {
                ErrorValidacion.innerHTML = "El Campo Cliente es obligatorio.";
                isValid = false;
            }



            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
        }

        function ActivarBotonDetalle1() {
            var boton2 = document.getElementById("<%= btnProgramarSolicitud.ClientID %>");
            boton2.disabled = false;

        }

        function validarFormularioDetalle() {

            var producto = document.getElementById("txDescProduc").value;
            var proveedor = document.getElementById("tbProveedor").value;
            var Ancho = document.getElementById("tbAncho").value;
            var Altura = document.getElementById("tbAltura").value;
            var Profundidad = document.getElementById("tbProfundidad").value;
            var Material = document.getElementById("tbMaterial").value;
            var Cantidad = document.getElementById("tbCantidad").value;
            var EspecificacionesGenerales = document.getElementById("txEspGen").value;
            var isValid = true;


            if (producto === "") {
                ErrorValidacionDetalle.innerHTML = "El campo producto es obligatorio.";
                isValid = false;
            } else if (proveedor === "") {
                ErrorValidacionDetalle.innerHTML = "El campo proveedor a es obligatorio.";
                isValid = false;
            } else if (Ancho === "") {
                ErrorValidacionDetalle.innerHTML = "El campo Ancho  es obligatorio.";
                isValid = false;
            } else if (Altura === "") {
                ErrorValidacionDetalle.innerHTML = "El campo Altura  es obligatorio.";
                isValid = false;
            }
            else if (Profundidad === "") {
                ErrorValidacionDetalle.innerHTML = "El Campo Profundidad es obligatorio.";
                isValid = false;
            } else if (Material === "") {
                ErrorValidacionDetalle.innerHTML = "El Campo Material es obligatorio.";
                isValid = false;
            } else if (Cantidad === "") {
                ErrorValidacionDetalle.innerHTML = "El Campo Cantidad es obligatorio.";
                isValid = false;
            } else if (EspecificacionesGenerales === "") {
                ErrorValidacionDetalle.innerHTML = "El Campo Especificaciones Generales es obligatorio.";
                isValid = false;
            }



            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
        }

        function HabilitarBotDetalleD() {

            // Habilitar enlaces
            document.getElementById("Documentacion").classList.remove("disabled");
            document.getElementById("Documentacion").classList.add("enabled", "AzulActivo");


            document.getElementById("ModificarDetalle").classList.remove("disabled");
            document.getElementById("ModificarDetalle").classList.add("enabled", "AzulActivo");

            document.getElementById("RedirigirCompras").classList.remove("disabled");
            document.getElementById("RedirigirCompras").classList.add("enabled", "AzulActivo");

        }

        function ControlHeaderCard() {
            var headerCot1 = document.getElementById('<%= headerCot.ClientID %>');
            if (headerCot1) {
                headerCot1.style.display = 'none';
            }

            var headerDes = document.getElementById('<%= headerDes.ClientID %>');
            if (headerDes) {
                headerDes.style.display = 'none';
            }
            ControlBtnCliente();
        }

        function ControlBuscarSolicitud() {

            ControlBtnCliente
            ControlHeaderCard();
        }

    </script>

    <script>

        // Ocultar el div con clase "contenedor-icono" cuando se activa la pestaña "Info-content" 
        $(document).ready(function () {
            $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var targetTab = $(e.target).attr("href");
                if (targetTab === "#BuscarDesarrollo-content" || targetTab === "#Programacion-content") {
                    $(".contenedor-icono").hide();
                } else {
                    $(".contenedor-icono").show();
                }
            });
        });


    </script>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const scriptToExecute = '<%= Session["ScriptEspecifico"] %>';
            if (scriptToExecute) {
                eval(scriptToExecute);

                $.ajax({
                    type: "POST",
                    url: "Solicitud_Especial.aspx/LimpiarVaribleSessiondetalle",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                });


            }
        });

    </script>

    <script>
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
                    if (focusedElement.id === "tbSolicitud1" || focusedElement.id === "tbClienteX" || focusedElement.id === "tbProyectoX" || focusedElement.id === "tbFechaIni" || focusedElement.id === "tbFechaFin") {
                        document.getElementById('<%= btnConsultar.ClientID %>').click();
                    }

                    if (focusedElement.id === "ID_Sol_Dib") {
                        document.getElementById('<%= BuscarSol.ClientID %>').click();
                    }

                    if (focusedElement.id === "ID_Cot_Dib") {
                        document.getElementById('<%= BuscarCot.ClientID %>').click();
                    }

                }


            }
        });
    </script>

    <script>
        // Mostrar  modal devolver a ventas 
        function mostralMoldalDevolver() {

            // Cambiamos el valor del span
            // Obtén el valor del Label de ASP.NET
            var lbNumeroSolicitud = document.getElementById('<%= lbNumeroSolicitud.ClientID %>').innerText;
            // Actualiza el contenido del span con el valor del Label
            document.getElementById('Span_Id_Sol1').innerText = lbNumeroSolicitud;


            $('#ConfirmarDevolSoli').modal('show');
        }

        function CerrarModalDevolver() {
            $('#ConfirmarDevolSoli').modal('hide');
        }

        // Mostrar  modal pausar solicitud
        function mostralMoldalPausar() {

            $('#ConfirmarPausarSol').modal('show');
        }

        function CerrarModalDevolver() {
            $('#ConfirmarPausarSol').modal('hide');
        }

        // Mostrar  modal pausar solicitud
        function mostralMoldalDespausar() {

            // Cambiamos el valor del span
            // Obtén el valor del Label de ASP.NET
            var lbNumeroSolicitud = document.getElementById('<%= lbNumeroSolicitud.ClientID %>').innerText;
            // Actualiza el contenido del span con el valor del Label
            document.getElementById('Span_Id_Sol3').innerText = lbNumeroSolicitud;

            $('#ConfirmarDespausarSol').modal('show');
        }

        // Mostrar  modal pausar solicitud
        function mostralMoldalDetener() {

            var lbNumeroSolicitud = document.getElementById('<%= lbNumeroSolicitud.ClientID %>').innerText;
            // Actualiza el contenido del span con el valor del Label
            document.getElementById('Span_Id_Sol4').innerText = lbNumeroSolicitud;

            $('#ConfirmarDetenerSol').modal('show');
        }

    </script>

    <script>
        function calcularPrecioSugerido() {
            var tbCostoD = document.getElementById('<%= tbCostoD.ClientID %>');
            var tbFactorD = document.getElementById('<%= tbFactorD.ClientID %>');
            var tbPrecioSugerido = document.getElementById('<%= tbPrecioSugerido.ClientID %>');

            var Costo = parseFloat(tbCostoD.value);
            var factor = parseFloat(tbFactorD.value);

            if (!isNaN(Costo) && !isNaN(factor) && factor != 100) {
                var PrecioSugerido = Costo / (1 - factor / 100);
                tbPrecioSugerido.value = Math.round(PrecioSugerido); // Redondear a entero
            } else {
                tbPrecioSugerido.value = "";
            }
        }
    </script>


</body>


</html>
