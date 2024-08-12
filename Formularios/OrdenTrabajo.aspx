<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrdenTrabajo.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.OrdenTrabajo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/OrdenTrabajo.css" />
    <title>Ordenes de Trabajo</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />


    <script>
        // Mostrar y Ocultar  acabados plano
        function mostrarModal() {
            $('#ModalAcabados').modal('show');
        }
        function ocultarModal() {
            $('#ModalAcabados').modal('hide');
        }
    </script>

    <script>
        // Mostrar y ocultar  modal para cargar archivo y leer  TXT
        function mostrarModalArchivo() {
            $('#ModalArchivo').modal('show');
        }
        function ocultarModalArchivo() {
            $('#ModalArchivo').modal('hide');
        }

        // Mostrar y ocultar  modal para cargar archivo y leer  TXT
        function mostrarModal_TXT_XY() {
            $('#modalArchivoAcad_XY').modal('show');
        }
        function ocultarModal_TXT_XY() {
            $('#modalArchivoAcad_XY').modal('hide');
        }


    </script>

    <script type="text/javascript">
        function MostrarSpiner() {
            ocultarModalArchivo();
            // Muestra el modal de carga
            $('#loadingModal1').modal('show');
        }
        // Función para ocultar el modal
        function OcultarSpiner() {
            $('#loadingModal1').modal('hide');
        }

    </script>

    <script type="text/javascript">
        function CargarExcel() {
            // Muestra el modal de carga Excel
            $('#loadingModalExcel').modal('show');
            iniciarCambiosExcel();
            setTimeout(function () {
                document.getElementById("btnTerminarDescarga").disabled = false;
            }, 1500); // 10 segundos


        }
        // Función para ocultar el modal Excel
        function CerrarCargarExcel() {
            $('#loadingModalExcel').modal('hide');
        }

    </script>

    <script type="text/javascript">
        function CargarOK() {
            // Muestra el modal de carga Boton Ok
            $('#OkCargando').modal('show');
            iniciarCambios();
        }
        // Función para ocultar el modal Boton Ok
        function CerrarCargarOK() {
            $('#OkCargando').modal('hide');
        }

    </script>

    <script>
        // Para camabiar los mensajes en el modal de espera
        var mensajesEspera = [
            "Cargando...",
            "Validando Información de la O.T",
            "Por favor, espere..."

        ];
        var indiceMensaje = 0;

        // Función para cambiar el mensaje cada 2 segundos
        function cambiarMensaje() {
            // Obtener el elemento del mensaje
            var mensajeElemento = document.getElementById("mensajeCargando");

            // Cambiar el texto del mensaje al siguiente mensaje en el arreglo
            mensajeElemento.textContent = mensajesEspera[indiceMensaje];

            // Incrementar el índice para el siguiente mensaje
            indiceMensaje++;

            // Si alcanzamos el final del arreglo, reiniciamos el índice
            if (indiceMensaje >= mensajesEspera.length) {
                indiceMensaje = 0;
            }
        }

        // Función para iniciar el cambio de mensajes
        function iniciarCambios() {
            // Llamar a la función cambiarMensaje cada 2 segundos
            setInterval(cambiarMensaje, 3000);
        }


    </script>

    <script>
        // Para camabiar los mensajes en el modal de espera
        var mensajesEspera = [
            "Por favor, espere mientras el archivo se descarga....",
            "No haga clic fuera de este mensaje..",
            "Al finalizar la descarga, presione 'Terminar Descarga'..."

        ];
        var indiceMensaje = 0;

        // Función para cambiar el mensaje cada 2 segundos
        function cambiarMensaje() {
            // Obtener el elemento del mensaje
            var mensajeElemento = document.getElementById("mensajeCargando1");

            // Cambiar el texto del mensaje al siguiente mensaje en el arreglo
            mensajeElemento.textContent = mensajesEspera[indiceMensaje];

            // Incrementar el índice para el siguiente mensaje
            indiceMensaje++;

            // Si alcanzamos el final del arreglo, reiniciamos el índice
            if (indiceMensaje >= mensajesEspera.length) {
                indiceMensaje = 0;
            }
        }

        // Función para iniciar el cambio de mensajes
        function iniciarCambiosExcel() {
            // Llamar a la función cambiarMensaje cada 2 segundos
            setInterval(cambiarMensaje, 3000);
        }


    </script>

    <script>
        function reflejarContenido() {
            // Obtener el contenido del primer textarea
            var contenidoTextarea1 = document.getElementById("txObs1").value;

            // Mostrar el mismo contenido en el segundo textarea
            document.getElementById("txObs2").value = contenidoTextarea1;
        }
    </script>

    <script>
        window.onload = function () {
            var textarea1 = document.getElementById('txObs1');
            var textarea2 = document.getElementById('txObs2');

            textarea1.oninput = function () {
                textarea2.value = textarea1.value;
            };
        };
    </script>

    <script type="text/javascript">
        function imprimirDatos() {

            // Ocultar el LinkButton antes de imprimir
            document.getElementById('<%= imprimirDatos.ClientID %>').style.display = 'none';

            // Imprimir
            window.print();

            $('#modalImprimir').modal('hide');

        }
    </script>

    <script type="text/javascript">
        function imprimirDatos2() {

            // Ocultar el LinkButton antes de imprimir
            document.getElementById('<%= btnImprimir2.ClientID %>').style.display = 'none';

            // Imprimir
            window.print();

            $('#modalImprimir2').modal('hide');

        }
    </script>

    <script>
        function activarPestana(pestanaId, contenidoId) {
            setTimeout(function () {
                // Desactivar la pestaña actualmente activa
                document.querySelector(".nav-link.active").classList.remove("active");
                document.querySelector(".tab-pane.show.active").classList.remove("show", "active");

                // Activar la nueva pestaña
                document.getElementById(pestanaId).classList.add("active");
                document.getElementById(contenidoId).classList.add("show", "active");
            }, 350); // 350 milisegundos de retardo
        }

        function MostralModalObjetosNo() {
            //Mostrar Modal de Objetos no Existentes 
            $('#modalObjNoExistente').modal('show');
        }

        function mostrarModalExito() {
            //Mostrar Modal de Objetos no Existentes 
            $('#ModalExito').modal('show');
        }

    </script>

    <script>
        $(document).ready(function () {
            $('#<%= btnCargarTXT_XY.ClientID %>').on('click', function () {
                showProgressBar();
            });
        });

        function showProgressBar() {
            var progressBar = $('#progressBar');
            var progressMessage = $('#progressMessage');
            progressBar.css('width', '0%');
            progressBar.attr('aria-valuenow', 0);

            // Array de mensajes
            var messages = ["Cargando...", "Por favor, espere...", "Estamos procesando su solicitud...", "Gracias por su paciencia..."];
            var messageIndex = 0;

            // Simular progreso
            var interval = setInterval(function () {
                var currentWidth = parseInt(progressBar.attr('aria-valuenow'));
                if (currentWidth >= 100) {
                    // Reiniciar la barra de progreso
                    currentWidth = 0;
                    messageIndex = (messageIndex + 1) % messages.length; // Cambiar el mensaje
                } else {
                    currentWidth += 3;
                }
                progressBar.css('width', currentWidth + '%');
                progressBar.attr('aria-valuenow', currentWidth);
                progressMessage.text(messages[messageIndex]); // Actualizar el mensaje
            }, 500);
        }

    </script>




</head>

<body translate="no">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light navbar-custom">
            <div class="container d-flex justify-content-center ">
                <ul class="nav nav-tabs gap-5" id="miPestañas">


                    <li class="nav-item">
                        <a class="nav-link text-white active" id="OTs-tab" data-bs-toggle="tab" href="#OTs-Content"><i class="bi bi-person-fill-gear"></i>Ordenes Trabajo</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="Plano-tab" data-bs-toggle="tab" href="#Plano-Content"><i class="bi bi-file-image-fill"></i>Plano</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-whiite " id="Objeto-tab" data-bs-toggle="tab" href="#Objeto-Content"><i class="bi bi-box-fill"></i>Objetos</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white" id="Modulo-tab" data-bs-toggle="tab" href="#Modulo-Content"><i class="bi bi-inboxes-fill"></i>Modulos</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white " id="Insumo-tab" data-bs-toggle="tab" href="#Insumo-Content"><i class="bi bi-grid-3x3-gap-fill"></i>Insumos</a>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="tab-content">

            <div class="tab-pane fade show active" id="OTs-Content">
                <asp:UpdatePanel ID="PanelOt" runat="server" UpdateMode="Conditional" DefaultButton="btnSubmit">
                    <ContentTemplate>

                        <div class="container-fluid">

                            <!--Modal para OT cerrada-->
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

                            <!--Modal Bolsa -->
                            <div class="modal fade" id="modalBolsa" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-xl ">
                                    <div class="modal-content">

                                        <div class="modal-header">
                                            <h5 class="modal-title" id="tBolsa">Bolsa de la OT: <span id="OtBolsa"></span></h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>

                                        <div class="modal-body">

                                            <div class="row pb-2 mb-2">
                                                <div class="col-3 text-start">
                                                    <input id="Checkbox1" type="checkbox" onclick="mostrarConvenciones();" />
                                                    <span>Ver Convenciones</span>

                                                </div>

                                            </div>

                                            <div class="row border rounded p-1 m-1" id="filaCovenciones" style="display: none; width: 15rem; height: 7rem">
                                                <div class="col-12">
                                                    <div class="input-group input-group-sm mb-1 gap-2">
                                                        <div class="input-group mt-2 " style="width: 10px; height: 10px; border: 1px; background-color: lawngreen"></div>
                                                        <label for="lbNoProgamado" class="form-label">100% Pedido</label>
                                                    </div>

                                                    <div class="input-group input-group-sm mb-1 gap-2">
                                                        <div class="input-group mt-2 " style="width: 10px; height: 10px; border: 1px; background-color: red"></div>
                                                        <label for="lbPausado" class="form-label">Bolsa Excedida</label>
                                                    </div>
                                                    <div class="input-group input-group-sm mb-1 gap-2">
                                                        <div class="input-group mt-2" style="width: 10px; height: 10px; border: 1px; background-color: yellow"></div>
                                                        <label for="lbEspera" class="form-label">Pendientes por pedir</label>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="row justify-content-center mb-3">
                                                <div class="border rounded p-2">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                                <h5 class="datagrid-header text-center">Bolsa</h5>
                                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridBolsa" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="DataBolsa" OnItemDataBound="DataGridBolsa_ItemDataBound">
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="otbolBolsa" HeaderText="Bolsa" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="otbolGrupoObjeto" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="otbolCantidadCotizada" HeaderText="Cant. Cot" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="otbolCantidadPedida" HeaderText="Cant. Ped" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Saldo" HeaderText="Saldo" ItemStyle-CssClass="auto-width-column" />

                                                                    </Columns>
                                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="DataBolsa" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select otbolBolsa ,otbolGrupoObjeto,otbolCantidadCotizada,otbolCantidadPedida,otbolCantidadCotizada - otbolCantidadPedida as Saldo from tblOTBolsa where otbolId_OT=@Ot order by otbolBolsa asc, otbolGrupoObjeto asc">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="tbOT" PropertyName="Text" Name="Ot"></asp:ControlParameter>
                                                                    </SelectParameters>
                                                                </asp:SqlDataSource>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>

                                        <div class="modal-footer">

                                            <!--espacio del footer del modaa-->
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <!--Modal Imprimir Info General OT -->
                            <div class="modal fade" id="modalImprimir" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-fullscreen">
                                    <div class="modal-content">


                                        <div class="modal-body">

                                            <!--contenedor  para cuando la OT no le han dado boton de Ventas -->
                                            <div class="container-fluid p-3" runat="server" id="divImprOTAbierta">

                                                <div class="row">
                                                    <div class="input-group input-group-sm justify-content-end gap-4">
                                                        <asp:LinkButton runat="server" title="Imprimir Información General de la OT" ID="imprimirDatos" CssClass=" font-size:2rem;" OnClientClick="imprimirDatos();">
                                                          <i class="bi bi-printer bi-printer-imprime"></i>
                                                        </asp:LinkButton>

                                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                    </div>
                                                </div>

                                                <div class="row pb-2">
                                                    <div class="col-12">

                                                        <div class="row justify-content-center">
                                                            <div class="border rounded p-2">
                                                                <div class="table-responsive">
                                                                    <h5 class="datagrid-header text-center fw-bold">Ducuon S.A.S</h5>
                                                                    <table class="table table-hover table-bordered table-sm border">

                                                                        <tbody>

                                                                            <tr>
                                                                                <td style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">

                                                                                        <label class="fw-bold">Orden Trabajo:  </label>
                                                                                        <label runat="server" id="lbOrdenTrabjo"></label>

                                                                                        <label class=" fw-bold ">Pedido:</label>
                                                                                        <label runat="server" id="lbPedido"></label>

                                                                                        <label class="fw-bold">Tipo Pedido:</label>
                                                                                        <label runat="server" id="lbTipoPed"></label>


                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">

                                                                                        <label class="fw-bold">Vendedor:  </label>
                                                                                        <label runat="server" id="lbVendedor"></label>

                                                                                        <label class="fw-bold">Cliente:</label>
                                                                                        <label runat="server" id="lbCliente"></label>

                                                                                    </div>

                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">

                                                                                        <label class="fw-bold">Nombre de la Obra:  </label>
                                                                                        <label runat="server" id="lbNombreObra"></label>

                                                                                    </div>
                                                                                </td>
                                                                            </tr>

                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>



                                                </div>

                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-2">
                                                        <div class="row justify-content-center">
                                                            <div class="col-11">
                                                                <div class="table-responsive  mb-2 gap-2" style="max-height: auto; overflow-x: auto;">
                                                                    <h6 class="datagrid-header text-center">ACABADOS</h6>
                                                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid2" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="Acabado">
                                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="AcabadoVentas" HeaderText="Descripción" />
                                                                        </Columns>
                                                                    </asp:DataGrid><asp:SqlDataSource runat="server" ID="Acabado" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select AcabadoVentas from tblOTAcabados where Id_OT = @OT AND Consecutivo_Pedido =  @pedido">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="tbOT" Name="OT"></asp:ControlParameter>
                                                                            <asp:ControlParameter ControlID="ddlNumbers" Name="pedido"></asp:ControlParameter>
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row pt-3 mt-3">
                                                    <div class="col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <label class="fw-bold">Se produce por: </label>
                                                            <label runat="server" id="lbProduce"></label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row pt-2" style="height: 15rem;">
                                                    <div class="col-12">
                                                        <p runat="server" id="pObservaciones"></p>
                                                    </div>
                                                </div>



                                            </div>

                                            <div class="row" id="espacio" runat="server">
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />

                                            </div>

                                            <div class="row" id="espacio1" runat="server">
                                                <br />
                                                <br />
                                                <br />

                                            </div>

                                            <div class="modal-footer d-flex flex-column align-items-stretch pt-3 ">
                                                <div class="row ">
                                                    <div class="col-12">

                                                        <div class="row justify-content-center">

                                                            <div class="table-responsive ">
                                                                <table class="table table-hover table-bordered border table-sm">

                                                                    <tbody>

                                                                        <tr>
                                                                            <td colspan="2" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">Dirección :  </label>
                                                                                    <label runat="server" id="lbDir"></label>
                                                                                    <label runat="server" id="lbCiuDep"></label>
                                                                                    <label runat="server" id="lbPai"></label>

                                                                                </div>
                                                                            </td>

                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">Recibe:  </label>
                                                                                    <label runat="server" id="lbReci"></label>

                                                                                    <label class="fw-bold">Contacto:  </label>
                                                                                    <label runat="server" id="lbconta"></label>

                                                                                </div>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">Teléfono:  </label>
                                                                                    <label runat="server" id="lbTel"></label>

                                                                                    <label class="fw-bold">Mail:  </label>
                                                                                    <label runat="server" id="lbMail"></label>

                                                                                </div>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">F. Confir Venta:  </label>
                                                                                    <label runat="server" id="lbFecConVenta"></label>
                                                                                </div>
                                                                            </td>

                                                                            <td style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">


                                                                                    <label class="fw-bold">F. Entrega Dib y Desp:  </label>
                                                                                    <label runat="server" id="lbFecDibDes"></label>
                                                                                </div>
                                                                            </td>

                                                                        </tr>

                                                                        <tr>
                                                                            <td style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">F. Entrega a Producción:  </label>
                                                                                    <label runat="server" id="lbFecEntreProd"></label>
                                                                                </div>
                                                                            </td>

                                                                            <td style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">F. Empaque:  </label>
                                                                                    <label runat="server" id="lbFecEmpaq"></label>
                                                                                </div>
                                                                            </td>

                                                                        </tr>

                                                                        <tr>
                                                                            <td style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">F. Real Despacho:  </label>
                                                                                    <label runat="server" id="Label7"></label>
                                                                                </div>

                                                                            </td>

                                                                            <td style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">F. Instalación:  </label>
                                                                                    <label runat="server" id="lbFecIns"></label>
                                                                                </div>
                                                                            </td>

                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm gap-5">

                                                                                    <label class="fw-bold">Plano :  </label>
                                                                                    <label runat="server" id="lbPlan"></label>

                                                                                    <label class="fw-bold">Dibuja y Despieza :  </label>
                                                                                    <label runat="server" id="lbDibDes"></label>


                                                                                </div>
                                                                            </td>

                                                                        </tr>



                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </div>


                                                    </div>



                                                </div>
                                            </div>


                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Imprimir Info Contable OT  -->
                            <div class="modal fade" id="modalImprimir2" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-fullscreen">
                                    <div class="modal-content">


                                        <div class="modal-body">

                                            <!--contenedor  para cuando la OT no le han dado boton de Ventas -->
                                            <div class="container-fluid p-3" runat="server" id="div1">

                                                <div class="row">
                                                    <div class="input-group input-group-sm justify-content-end gap-4">
                                                        <asp:LinkButton runat="server" title="Imprimir Información Contable de la OT" ID="btnImprimir2" CssClass=" font-size:2rem;" OnClientClick="imprimirDatos2();">
                                                          <i class="bi bi-printer bi-printer-imprime"></i>
                                                        </asp:LinkButton>

                                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                    </div>
                                                </div>

                                                <div class="row pb-2">
                                                    <div class="col-12">

                                                        <div class="row justify-content-center">
                                                            <div class="border rounded p-2">
                                                                <div class="table-responsive">
                                                                    <h5 class="datagrid-header text-center fw-bold">CARTA CONFIRMACIÓN PEDIDO</h5>
                                                                    <table class="table table-hover table-bordered table-sm border">

                                                                        <tbody>
                                                                            <tr>
                                                                                <td colspan="2" style="width: 2rem;">
                                                                                    <div class="input-group input-group-sm gap-4">

                                                                                        <label class="fw-bold ">Orden Trabajo:  </label>
                                                                                        <label runat="server" class="fw-bold" id="lbOt2"></label>

                                                                                        <label class="fw-bold ">Pedido:</label>
                                                                                        <label runat="server" class="fw-bold" id="lbPed2"></label>

                                                                                        <label class=" fw-bold">Tipo Pedido:</label>
                                                                                        <label runat="server" class="fw-bold" id="lbTipoPed2"></label>


                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-2">
                                                                                        <label class="fw-bold">F. Pedido: </label>
                                                                                        <asp:Label ID="lbFPed" runat="server"></asp:Label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1" style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Asesor Comercial:  </label>

                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1" style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label runat="server" id="lbAse2"></label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1" style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>F. Despacho: </label>
                                                                                        <label runat="server" id="lbFecDes1"></label>
                                                                                    </div>
                                                                                </td>


                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1" style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Nombre Cliente :  </label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1" style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label id="lbClie2" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1" style="white-space: nowrap;">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Cot N°: </label>
                                                                                        <label id="lbCot1" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Dirección Cliente: </label>

                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label id="lbDir1" runat="server"></label>

                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Nit: </label>
                                                                                        <label id="lbNit1" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Contacto Cliente: </label>

                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label id="lbCont" runat="server"></label>

                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Tel: </label>
                                                                                        <label id="lbTel1" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label class="fw-bold">Nombre Obra: </label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label id="lbNomObra" class="fw-bold" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Altura: </label>
                                                                                        <label id="lbAlt1" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Dir. Despacho: </label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="2">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label id="lbDirDes1" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Contacto Obra: </label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label id="lbContaObr" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Teléfono: </label>
                                                                                        <label id="lbTel3" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                            <tr>
                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Recibe Mercancia: </label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label id="lbCont3" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                                <td colspan="1">
                                                                                    <div class="input-group input-group-sm gap-4">
                                                                                        <label>Forma de Pago: </label>
                                                                                        <label id="lbForPago" runat="server"></label>
                                                                                    </div>
                                                                                </td>

                                                                            </tr>

                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>



                                                </div>

                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-2">
                                                        <div class="row justify-content-center">
                                                            <div class="col-11">
                                                                <div class="table-responsive  mb-2 gap-2" style="max-height: auto; overflow-x: auto;">
                                                                    <h6 class="datagrid-header text-center">ACABADOS</h6>
                                                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGridAcabadoModal" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="Acabado2">
                                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="AcabadoVentas" HeaderText="Descripción" />
                                                                        </Columns>
                                                                    </asp:DataGrid><asp:SqlDataSource runat="server" ID="Acabado2" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select AcabadoVentas from tblOTAcabados where Id_OT = @OT AND Consecutivo_Pedido =  @pedido">
                                                                        <SelectParameters>
                                                                            <asp:ControlParameter ControlID="tbOT" Name="OT"></asp:ControlParameter>
                                                                            <asp:ControlParameter ControlID="ddlNumbers" Name="pedido"></asp:ControlParameter>
                                                                        </SelectParameters>
                                                                    </asp:SqlDataSource>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row pt-3 mt-3">
                                                    <div class="col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <h6>Se produce por: </h6>
                                                            <label runat="server" id="lbProd2"></label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row pt-2" style="height: 15rem;">
                                                    <div class="col-12">
                                                        <p runat="server" id="observaciones2"></p>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="row" id="esp1" runat="server">
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                            </div>

                                            <div class="row" id="esp2" runat="server">
                                                <br />
                                                <br />
                                                <br />
                                                <br />
                                            </div>

                                            <div class="modal-footer d-flex flex-column align-items-stretch pt-3 ">

                                                <div class="row ">
                                                    <div class="col-12">

                                                        <div class="row justify-content-center">

                                                            <div class="table-responsive ">
                                                                <table class="table table-hover table-bordered border table-sm">

                                                                    <tbody>

                                                                        <tr>
                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label>Plano</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label>D. Troquel</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label>Espesor Sup</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label>PVC</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label>P. Cables</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label>Sujeción</label>
                                                                                </div>
                                                                            </td>
                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label>Cheq. Med</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label>Cotización</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label>O. Compra</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label>Bitácora</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="1" style="white-space: nowrap;">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">NO</label>
                                                                                </div>
                                                                            </td>


                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="6" rowspan="6">
                                                                                <div class=" input-group-sm ">
                                                                                    <label class="fw-bold">Observaciones Contables</label>
                                                                                    <br />
                                                                                    <label id="ObsConta" runat="server"></label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">Total Obra</label>

                                                                                </div>
                                                                            </td>

                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm justify-content-end">
                                                                                    <label id="lbValorTotObra" class="fw-bold" runat="server"></label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm justify-content-between ">
                                                                                    <label class="fw-bold">Dto(%)</label>
                                                                                    <label id="lbPorDto" class="fw-bold" runat="server"></label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm justify-content-end">
                                                                                    <label id="lbValorDto" class="fw-bold" runat="server"></label>
                                                                                </div>
                                                                            </td>


                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">Sub Total</label>

                                                                                </div>
                                                                            </td>

                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm justify-content-end ">
                                                                                    <label id="lbSubTo" class="fw-bold" runat="server"></label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm justify-content-between ">
                                                                                    <label class="fw-bold">Iva: </label>
                                                                                    <label id="lbPorIva" class="fw-bold">16 %</label>
                                                                                </div>
                                                                            </td>

                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm justify-content-end ">
                                                                                    <label id="lbValorIva" class="fw-bold" runat="server"></label>
                                                                                </div>
                                                                            </td>


                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm ">
                                                                                    <label class="fw-bold">Gran Total</label>

                                                                                </div>
                                                                            </td>

                                                                            <td colspan="2">
                                                                                <div class="input-group input-group-sm justify-content-end">
                                                                                    <label id="lbGranTot" class="fw-bold" runat="server"></label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>


                                                                    </tbody>
                                                                </table>
                                                            </div>

                                                        </div>


                                                    </div>



                                                </div>


                                            </div>


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


                                                <asp:LinkButton runat="server" title="Nueva OT" ID="NuevaOt" OnClick="NuevaOT_Click">
                                                    <i class="bi bi-file-earmark-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Copiar Información en una Nueva OT" ID="CopiarOt" OnClick="BtnCopInfNueOT_Click">
                                                  <i class="bi bi-stickies-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Grabar Orden de Trabajo" ID="GrabarOt" OnClick="BtnGrabar_Click">               
                                                 <i class="bi bi-floppy-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Modificar Orden de Trabajo" ID="ModificarOt" OnClick="BtnModificar_Click">
                                                  <i class="bi bi-wrench-adjustable"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Anular o Eliminar un Pedido" ID="AnularPedido">
                                                <i class="bi bi-trash3-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Documentación OT" ID="DocumentacionOt" OnClick="DocumentacionOt_Click">
                                                  <i class="bi bi-paperclip"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Observaciones OT" ID="ObservacionesOt" OnClick="BtnObservaciones_Click">
                                                    <i class="bi bi-binoculars-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Imprimir Informacion General de la OT" ID="imprimirOt" OnClick="ImprimirOt_Click">
                                                     <i class="bi bi-printer"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Reimprimir Información Contable" ID="ReimprimirOt" OnClick="ReimprimirOt_Click">
                                                         <i class="bi bi-printer-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Consultar Bolsa" ID="ConsultarBolsa" OnClick="ConsultarBolsa1">
                                                    <i class="bi bi-currency-exchange"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Cancelar" ID="Cancelar" OnClick="Cancelar_Click">
                                                <i class="bi bi-x-circle-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Visualizar OT Pendientes" ID="OtPendientes" OnClick="OtPendientes_Click">
                                                    <i class="bi bi-sunglasses"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Actualizar Pedidos Importados" ID="ActPedImp">
                                               <i class="bi bi-check-circle-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Importar Pedido Asesor" ID="ImpPedAse">
                                                     <i class="bi bi-person-lines-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Importar Pedido Sede" ID="ImpPedSed">
                                                   <i class="bi bi-house-up"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Habilitar Pedido para Ventas" ID="HabilitarPedido">
                                                     <i class="bi bi-receipt-cutoff"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Deshabilitar Orden de Trabajo para Producción" ID="DeshabilitarOt">
                                                  <i class="bi bi-sign-stop"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Indicador Obra Reactivada" ID="ObraReactivada">
                                                 <i class="bi bi-tools"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Registrar Pedido en el Sistema Administrativo" ID="RegPedSisAdm">
                                                 <i class="bi bi-triangle"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Cierra o Abre una OT" ID="CierraOt">
                                                  <i class="bi bi-key"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Simular Pasar Pedido" ID="SimularPedido">
                                                 <i class="bi bi-code-square"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Exportar Pedido" ID="ExportarPedido">
                                                 <i class="bi bi-airplane-engines"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Entrega Perfecta" ID="EntregaPerfecta">
                                                  <i class="bi bi-lightning-charge"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Anular Obra" ID="AnularObra">
                                                 <i class="bi bi-x-square"></i>
                                                </asp:LinkButton>

                                            </div>

                                        </ul>
                                    </div>

                                </div>
                            </nav>

                        </div>

                        <div class="container-fluid p-3 shadow-sm bg-light">
                            <div class="row">

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="OT" runat="server" ID="lblOT"></asp:Label>
                                        <asp:TextBox ID="tbOT" runat="server" CssClass="form-control" OnTextChanged="ObtenerInfoOt" AutoPostBack="true" placeholder="Escriba número de OT"></asp:TextBox>
                                        <%--Activa el textbox de las OT cuando carga el formulario listo para escribir caracteres--%>
                                        <script type="text/javascript">
                                            window.onload = function () {
                                                document.getElementById('<%= tbOT.ClientID %>').focus();
                                            };
                                        </script>

                                    </div>
                                </div>

                                <div class="col-lg-1 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Pedido" runat="server" ID="lblPedido"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlNumbers" runat="server" OnSelectedIndexChanged="CambioDePediido" AutoPostBack="true"></asp:DropDownList>

                                    </div>
                                </div>

                                <div class="col-lg-1 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Zona" runat="server" ID="lbZona"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlZona" runat="server">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="01">01</asp:ListItem>
                                            <asp:ListItem Value="02">02</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="T.Ped" runat="server" ID="lblTped"></asp:Label>
                                        <asp:DropDownList ID="dtacboTipoPedido" runat="server" class="form-control" DataSourceID="TiposDePedidos" DataTextField="Descripcion_TipoPedido" DataValueField="Id_TipoPedido" AutoPostBack="True" OnSelectedIndexChanged="dtacboTipoPedido_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="TiposDePedidos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT Descripcion_TipoPedido, Id_TipoPedido, EstadisticaVenta FROM tblTipoPedido WHERE Activo = '1' AND orientacion =  'COMERCIAL' ORDER BY Descripcion_TipoPedido"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-lg-1 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="P.Base" runat="server" ID="lblPedBase"></asp:Label>
                                        <asp:DropDownList ID="cboPedidoBase" runat="server" class="form-control" DataSourceID="PedidoBase" DataTextField="Consecutivo_Pedido" DataValueField="Consecutivo_Pedido" ToolTip="Pedido Base"></asp:DropDownList>
                                        <asp:SqlDataSource ID="PedidoBase" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT Consecutivo_Pedido, EstadisticaVenta
                                                                FROM tblTipoPedido
                                                                INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido
                                                                WHERE tblOT.Id_OT = @Id_OT AND EstadisticaVenta = '1'
                                                                ORDER BY tblOT.Consecutivo_Pedido DESC;
                                                                ">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="Id_OT" SessionField="Id_OT" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>
                                </div>

                                <div class="col-lg-1 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="P.Dep" runat="server" ID="lblPedDepen"></asp:Label>
                                        <asp:DropDownList ID="tbPedDepen" CssClass="form-control" runat="server" DataSourceID="sqlDataSource1" ToolTip="Pedido Dependiente"
                                            DataTextField="Consecutivo_Pedido" DataValueField="Consecutivo_Pedido">
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="sqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                            SelectCommand="SELECT Consecutivo_Pedido FROM tblOT WHERE Id_OT = @Id_OT AND PedidoBase = @pedido">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="Id_OT" SessionField="Id_OT" Type="String" />
                                                <asp:SessionParameter Name="pedido" SessionField="pedido" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>
                                </div>

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Aprob" runat="server" ID="lblAprob"></asp:Label>

                                        <asp:DropDownList ID="DtaCboTipoAprobacion" runat="server" class="form-control" DataSourceID="Aprob" DataTextField="TipoAprobacion" DataValueField="IdTipoAprobacion"></asp:DropDownList>
                                        <asp:SqlDataSource ID="Aprob" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="
                                                                                                                                                                    SELECT * FROM tblTipoAprobacion ORDER BY TipoAprobacion"></asp:SqlDataSource>

                                    </div>
                                </div>

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm justify-content-around">
                                        <asp:LinkButton runat="server" title="Nuevo Pedido" ID="btnNuevoPedido" OnClick="NuevoPedido_Click">
                                         <i class="bi bi-files"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" title="Acabados" ID="btnAcabados" OnClick="Acabados_Click">
                                        <i class="bi bi-palette"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" title="OK" Text="OK" ID="btnOk" OnClick="Boton_Ok1"></asp:LinkButton>

                                    </div>
                                </div>

                            </div>

                            <div class="row">

                                <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Obra" runat="server" ID="lblObra"></asp:Label>
                                        <asp:TextBox ID="tbObra" type="text" class="form-control" runat="server" MaxLength="49"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-4">
                                        <asp:Label class="form-label" Text="Dir" runat="server" ID="lblDir"></asp:Label>
                                        <asp:TextBox ID="tbDir" type="text" class="form-control" runat="server" MaxLength="59"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Contac" runat="server" ID="lblContac"></asp:Label>
                                        <asp:TextBox ID="tbContac" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Email" runat="server" ID="lblEmail"></asp:Label>
                                        <asp:TextBox ID="tbEmail" type="email" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                            <div class="row">

                                <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Recibe" runat="server" ID="lblRecibe"></asp:Label>
                                        <asp:TextBox ID="tbRecibe" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Ciudad" runat="server" ID="lblCiudad"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" DataTextField="NombreCiudad" DataValueField="NombreCiudad" DataSourceID="CargarCiudad" OnDataBound="ddlCiudad_DataBound"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="CargarCiudad" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT  CONCAT(tblDepartamentoPais.CodigoDepartamento ,tblCiudad.CodigoCiudad)
                                            AS CodCompleto,tblCiudad.NombreCiudad+' - '+tblDepartamentoPais.NombreDepartamento As NombreCiudad 
                                            FROM tblDepartamentoPais  INNER JOIN tblCiudad             
                                            ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento 
                                            ORDER BY CONCAT(tblCiudad.NombreCiudad , '-' , tblDepartamentoPais.NombreDepartamento)"></asp:SqlDataSource>

                                    </div>
                                </div>

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-4">
                                        <asp:Label class="form-label" Text="Tel" runat="server" ID="lblTel"></asp:Label>
                                        <asp:TextBox ID="tbTel" type="tel" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Cel" runat="server" ID="lblCel"></asp:Label>
                                        <asp:TextBox ID="tbCel" type="tel" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Pais" runat="server" ID="lblPais"></asp:Label>
                                        <asp:TextBox ID="tbPais" type="text" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-lg-1 col-md-6 col-sm-6 col-xs-12">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="H.Total" runat="server" ID="lblHTotal"></asp:Label>
                                        <asp:TextBox ID="tbHTotal" type="number" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <div class="container-fluid p-3 Observacion shadow-sm bg-light mt-2">

                            <div class="Obs1">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <textarea class="form-control form-control-sm" id="txObs1" runat="server" cols="20" rows="10" oninput="reflejarContenido()"></textarea>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="Der">

                                <div class="Arriba">

                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">

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
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <label class="form-label" runat="server" id="inputDibujo">Ok.Dibujo</label>
                                                <asp:TextBox ID="dtpFechaEntregaProduccion" type="date" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">

                                            <asp:Label ID="LabelOTCerrada" ClientIDMode="Static" runat="server" Text="OT cerrada" BackColor="#DD0000" ForeColor="white" Font-Size="X-Large" Width="14.9em" Visible="false" CssClass="rounded-label"></asp:Label>

                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <label class="form-label" runat="server" id="inputEmpaque">Empaque</label>
                                                <asp:TextBox ID="dtpEmpaque" type="date" class="form-control" runat="server" OnTextChanged="ValidarFecha" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <label class="form-label" runat="server" id="inputRealEmp">Real Emp.</label>
                                                <asp:TextBox ID="dtpRealEmpaque" type="date" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class=" Abajo">
                                    <div class="row justify-content-center">
                                        <div class="border rounded">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="table-responsive" style="max-height: 8rem; max-width: auto; overflow-x: auto;">
                                                        <h6 class="datagrid-header text-center">Despacho</h6>
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridDespacho" runat="server" DataSourceID="obtenerInfoDespacho" OnItemDataBound="DataGridDespacho_ItemDataBound">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="...">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDespacho" runat="server" CommandName="Ver" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>


                                                                <asp:BoundColumn DataField="DespachoInterno" HeaderText="DI" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                                <asp:BoundColumn DataField="DespachoCoordinado" HeaderText="Coor" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                                <asp:BoundColumn DataField="FechaDespachoCoordinado" HeaderText="Coordinado el" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Fecha_Despacho" HeaderText="F. Despacho" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Terminado_despacho" HeaderText="Despachado" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                                <asp:BoundColumn DataField="FechaRealDespacho" HeaderText="F. Real Despacho" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Entregado_Transporte" HeaderText="Entregado" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
                                                                <asp:BoundColumn DataField="Fecha_Entregado" HeaderText="F. OK.entrega" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Receptor" HeaderText="Receptor" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Celular_Receptor" HeaderText="Celular" ItemStyle-CssClass="auto-width-column" />
                                                            </Columns>

                                                        </asp:DataGrid>

                                                        <asp:SqlDataSource runat="server" ID="obtenerInfoDespacho" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="sp_ObtenerInformacionDespacho" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="tbOT" PropertyName="Text" Name="Id_OT" Type="String"></asp:ControlParameter>
                                                                <asp:ControlParameter ControlID="ddlNumbers" PropertyName="SelectedValue" Name="Pedido" Type="Int32"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>

                                                        <asp:SqlDataSource runat="server" ID="InfoDespachos"></asp:SqlDataSource>

                                                    </div>




                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <div class="container-fluid p-3 Observacion shadow-sm bg-light mt-2">

                            <div class="Obs1">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                        <div class=" input-group-sm  mb-2 gap-2">

                                            <textarea class="form-control form-control-sm" id="txObs2" runat="server" cols="20" rows="8"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="Der">

                                <div class="row">

                                    <div class="col-lg-5 col-md-6 col-sm-12 col-xs-12">

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
                                    <div class="col-lg-5 col-md-6 col-sm-12 col-xs-12">
                                        <div class="input-group input-group-sm mb-2 gap-4">
                                            <asp:Button class="btn btn-outline-secondary" Text="Plano+" runat="server" type="button" disabled="disabled"></asp:Button>
                                            <asp:Label type="text" class="form-label fw-bold" runat="server" ID="lbPlano" Text="Plano" />
                                        </div>
                                    </div>

                                    <div class="col-lg-7 col-md-6 col-sm-12 col-xs-12">
                                        <div class="input-group   mb-2 gap-2">
                                            <asp:Label class="form-label text-end" Text="Bolsa" runat="server" ID="lblBolsa"></asp:Label>
                                            <asp:TextBox type="text" class="form-control text-end" runat="server" ID="tbBolsa" />
                                        </div>

                                    </div>
                                </div>

                                <div class="row">

                                    <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label class="form-label" Text="Fabrica" runat="server" ID="lblFabrica"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlFabrica1" runat="server">
                                                <asp:ListItem Value=" "></asp:ListItem>
                                                <asp:ListItem Value="Bogotá">Bogotá</asp:ListItem>
                                                <asp:ListItem Value="Medellín">Medellín</asp:ListItem>
                                                <asp:ListItem Value="BOGOTÁ">Bogotá</asp:ListItem>
                                                <asp:ListItem Value="MEDELLÍN">Medellín</asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="col-lg-1"></div>

                                    <div class="col-lg-7 col-md-6 col-sm-12 col-xs-12 text-end fw-bold">
                                        <div class="d-flex mb-2 gap-2">
                                            <asp:Label class="form-label" runat="server" ID="lbsaldo" Text="Saldo:" Visible="false"></asp:Label>
                                            <asp:Label class="form-label" runat="server" ID="lblSaldoOT" BorderColor="#006600" BackColor="Lime" Font-Size="X-Large" Width="20em" Height="1.5em" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                </div>

                                <div class="row">

                                    <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                        <div class="input-group input-group-sm mb-2 gap-2">
                                            <asp:Label class="form-label" Text="Instala" runat="server" ID="Instala"></asp:Label>
                                            <asp:DropDownList class="form-control" ID="ddlInstala" runat="server">
                                                <asp:ListItem Value=" "></asp:ListItem>
                                                <asp:ListItem Value="Bogotá">Bogotá</asp:ListItem>
                                                <asp:ListItem Value="Medellín">Medellín</asp:ListItem>
                                                <asp:ListItem Value="BOGOTÁ">Bogotá</asp:ListItem>
                                                <asp:ListItem Value="MEDELLÍN">Medellín</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-6 col-sm-12 col-xs-12">
                                        <div class="row">
                                            <div class="col-lg-12 col-md-6 col-sm-6 col-xs-12">
                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                    <asp:Label class="form-label" Text="V. Pedido" runat="server" ID="Label1"></asp:Label>
                                                    <asp:Button class="btn btn-outline-secondary" Text="TXT" runat="server" type="button" disabled="disabled"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                        <div class="input-group  mb-1 gap-2">
                                            <asp:TextBox ID="tbValorPedido" CssClass="form-control text-end" runat="server"></asp:TextBox>
                                        </div>
                                    </div>


                                </div>

                            </div>

                        </div>

                        <div class="container-fluid p-3 shadow-sm bg-light mt-2">

                            <h5 class="p-0 m-0 mb-1 text-center">Informacion Contable </h5>
                          
                            <div runat="server" class="row" id="DivPorDefecto">
                              
                                <div class="col-sm-3" id="DivOculto" runat="server">

                                    <div class="row pb-2">

                                        <div class="col-sm-12">
                                            <div class=" input-group input-group-sm gap-1">
                                                <asp:Button ID="Nit" runat="server" Text="Nit  ..." class="bi bf btn btn-secondary" OnClick="Redireccion_Nit" />
                                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtNit"></asp:TextBox>
                                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtNombreEmp"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">

                                        <div class="col-sm-12">
                                            <div class=" input-group input-group-sm" style="gap:0.7rem;" >
                                                 <asp:Label class="form-label" Text="Contacto" runat="server" ID="lblContacto"></asp:Label>
                                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtcontacto"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">

                                        <div class="col-md-12">
                                            <div class=" input-group input-group-sm" style="gap:2.7rem;">
                                                  <asp:Label class="form-label" Text="Mail" runat="server" ID="lblMail"></asp:Label>
                                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtMail"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">                       

                                        <div class=" col-sm-12 col-xs-12">
                                            <div class=" input-group input-group-sm" style="gap:0.5rem;">
                                                 <asp:Label class="form-label" Text="Dirección" runat="server" ID="lblDireccion"></asp:Label>
                                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtDireccion"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">
                                       
                                        <div class=" col-sm-12 col-xs-12">
                                            <div class=" input-group input-group-sm gap-1 ">
                                                 <asp:Label class="form-label" Text="Municipio" runat="server" ID="lblMunicipio"></asp:Label>
                                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtMunicipio"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">

                                        <div class="col-sm-12 col-xs-12">
                                            <div class=" input-group input-group-sm " style="gap:0.8rem;">
                                                 <asp:Label class="form-label" Text="Telefono" runat="server" ID="lblTelefono"></asp:Label>
                                                <asp:TextBox type="text" class="form-control" runat="server" ID="txtTelefono"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">
                                        <div class="col-sm-12 col-xs-12">
                                            <div class=" input-group input-group-sm gap-1 ">
                                                  <asp:Label class="form-label" Text="Obs. Contable " runat="server" ID="lblObs"></asp:Label>
                                                <textarea class="form-control form-control-sm" id="ObservacionCont" runat="server" cols="20" rows="3" high="60px"></textarea>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="col-lg-12 col-md-6 col-sm-6 col-xs-12">

                                            <div class=" input-group input-group-sm">
                                                <asp:TextBox type="" class="form-control fw-bold text-white rojo" runat="server" ID="txtMensaje" Visible="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                   
                                <div class="col-sm-6" id="Principal" runat="server">

                                    <div class="row pb-1" id="PrimeraFila" runat="server">

                                        <div class="col-sm-3">
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:Button ID="btnCotizacion" runat="server" Text="Ver cotización" class="btn btn-sm btn-outline-secondary " OnClick="btnCotizacion_Click" OnClientClick="return validarCotizacion();" />
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="input-group input-group-sm gap-2 ">
                                                        <asp:TextBox type="text" class="form-control form-control-sm" runat="server" ID="txtCotizacion" OnTextChanged="txtCotizacion_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </div>

                                                </div>

                                            </div>

                                        </div>

                                        <div class="col-sm-3">

                                            <div class="row ">

                                                <div class="col-12">
                                                    <asp:Button ID="btnValorSugeroido" runat="server" Text="Valor Sugerido" class="bi bf w-100" disabled="true" />
                                                </div>

                                            </div>

                                            <div class="row justify-content-end">

                                                <div class="col-12">
                                                    <div class="input-group input-group-sm gap-2 ">
                                                        <asp:Label ID="Label4" CssClass=" form-label shadow" runat="server">$</asp:Label>
                                                        <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtValorSugerido" Style="text-align: left !important;"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                        <div class="col-sm-3">

                                            <div class="row">

                                                <div class="col-12">
                                                    <asp:Button ID="btnVscd" runat="server" Text="VCSD" class="bi bf w-100" disabled="true" />
                                                </div>

                                            </div>

                                            <div class="row justify-content-end">

                                                <div class="col-12">
                                                    <div class="input-group input-group-sm gap-2 ">
                                                        <asp:Label ID="Label5" CssClass=" form-label shadow" runat="server">$</asp:Label>
                                                        <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtVcsd" Style="text-align: left !important;"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>


                                        </div>

                                        <div class="col-sm-3">

                                            <div class="row">

                                                <div class="col-12">
                                                    <asp:Button ID="btnVccd" runat="server" Text="VCCD" class="bi bf w-100" disabled="true" />
                                                </div>

                                            </div>

                                            <div class="row justify-content-end">

                                                <div class="col-12">
                                                    <div class="input-group input-group-sm gap-2 ">
                                                        <asp:Label ID="Label6" CssClass=" form-label shadow" runat="server">$</asp:Label>
                                                        <asp:TextBox type="text" class="form-control text-end" runat="server" ID="txtVccd" Style="text-align: left !important;"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>

                                    </div>

                                    <div class="row pb-1"  id="SegundaFila" runat="server">

                                        <div class="col-sm-6">
                                            <div class="input-group input-group-sm">
                                                <asp:Button ID="btnOrdenCompra" runat="server" Text="Orden Compra" class="bi bf" disabled="true" />
                                                <asp:TextBox type="text" class="form-control form-control-sm" runat="server" ID="txtOrdenCompra"></asp:TextBox>
                                            </div>

                                        </div>

                                        <div class="col-sm-6">
                                            <div class="input-group input-group-sm justify-content-center gap-1">
                                                <asp:CheckBox class="" ID="cbxComisionCompart" runat="server" CssClass="pt-1" AutoPostBack="true" OnCheckedChanged="cbxComisionCompart_CheckedChanged" />
                                                <asp:Label ID="lblComisionCompart" class="form-label" Text="Comisión Compartida" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pb-1" id="TerceraFila" runat="server">
                                        <div class="col-sm-6">
                                            <div class="input-group input-group-sm">
                                                <asp:Button ID="btnAsesor1" runat="server" Text="Asesor" class="bi bf" disabled="true" />
                                                <asp:TextBox type="text" class="form-control form-control-sm" runat="server" ID="txtAsesor"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-sm-6">
                                            <div class="input-group input-group-sm">
                                                <asp:DropDownList ID="ddlAsesor" class=" form-control form-control-sm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAsesor_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-sm-1" id="VentaNota" runat="server">
                                            <label>Venta<br />
                                                Neta</label>
                                        </div>
                                        
                                     
                                        <div id="divDataGridContainer" runat="server" class="col-sm-9">

                                            <div class="row justify-content-center m-1 p-1">
                                                <div class="border rounded p-2 m-2">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <div class="table-responsive mb-1" style="max-height: 17rem; overflow-x: auto;">
                                                                <h5 class="datagrid-header text-center">Contable</h5>
                                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGrid" runat="server" DataSourceID="InfoContable" OnItemDataBound="DataGrid_RowDataBound">
                                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Pedido" />
                                                                        <asp:BoundColumn DataField="Descripcion_TipoPedido" HeaderText="Tipo Pedido" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Precio_Venta" HeaderText="V. Venta" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="ValorBolsa" HeaderText="V. Bolsa" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                                        <asp:BoundColumn DataField="ValorPedido" HeaderText="Valor Pedido" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="PedidoBase" HeaderText="Ref" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="" HeaderText="Total Ref" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="" HeaderText="Saldo" ItemStyle-CssClass="auto-width-column" />

                                                                    </Columns>
                                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="InfoContable" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                                    SelectCommand="SELECT Consecutivo_Pedido,Descripcion_TipoPedido,Precio_Venta,
                                                                                ValorBolsa,ValorPedido,Precio_Venta - Descuento * Precio_Venta/100 AS Subtotal,
                                                                                PedidoBase, tblTipoPedido.*,Terminada_Facturacion,Fecha_Factura,Descuento,EstadisticaVenta
                                                                                FROM tblTipoPedido INNER JOIN tblOT ON tblTipoPedido.Id_TipoPedido = tblOT.Id_TipoPedido 
                                                                                WHERE (((tblOT.Id_OT)=@Id_OT)) ORDER BY tblOT.Consecutivo_Pedido DESC ">
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
                                          
                                        <div class="col-sm-2" id="TipDeNeg" runat="server">

                                            <label>Tipo de Negociación</label>
                                            <textarea class="form-control form-control-sm" id="TextTNegociacion" runat="server" cols="25" rows="6"></textarea>

                                        </div>
                                        
                                    </div>

                                </div>
                               
                                <div class="col-sm-3" id="divDataGridContainer2" runat="server">

                                    <div class="Info1">
                                        <asp:Button ID="btnDiseño" runat="server" Text="Diseño" class="bi bf " disabled="true" />
                                        <asp:TextBox type="text" class="form-control text-end " runat="server" ID="txtDiseño"></asp:TextBox>
                                    </div>
                                    <div class="Info1">
                                        <asp:Button ID="btnComision1" runat="server" Text="D. Comision" class="bi bf" disabled="true" />
                                        <asp:TextBox type="text" class="form-control text-end  " runat="server" ID="txtComision"></asp:TextBox>
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

                        </div>

                        <!--Modal Confirmacion Boton OK-->
                        <div id="BotonOk" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title text-center">Terminar OT</h5>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>Esta seguro de Terminar la Orden de Trabajo: <span id="OTBotonOk"></span>Pedido  <span id="PedBotonOk"></span></h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnOK_Si" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-success" OnClick="Boton_Ok" AutoPostBack="true" Style="width: 5rem;" OnClientClick="CargarOK();" />
                                            <asp:Button runat="server" ID="btnOK_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="CopiarAcabados" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Copiar Acabados</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Desea copiar los acabados de la OT:</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" ID="BtnSi" Text="Si" class="btn btn-sm btn-outline-success" data-bs-dismiss="modal" aria-label="Close" OnClick="BtnSi_Click" AutoPostBack="true" />
                                        <asp:Button runat="server" ID="BtnNo" Text="No" class="btn btn-sm btn-outline-secondary" data-bs-dismiss="modal" aria-label="Close" OnClick="BtnNo_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal" id="miModalll" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Campo Faltante</h5>
                                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Falta llenar el campo: <span id="campoFaltante"></span></p>
                                    </div>
                                    <div class="modal-footer">
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="OTingresada" class="modal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">S_I_Ducon</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="OTingresada2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" OnClick="MonstrasrModalAcabados_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="PedidoIngresado" class="modal" tabindex="-1">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">S_I_Ducon</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="PedidoIngresado2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal" id="NuevoPedido" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Información General OT</h5>
                                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Desea continuar con la información general de la OT?</span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" Text="Si" class="btn btn-sm btn-outline-success" data-bs-dismiss="modal" aria-label="Close" OnClick="BtnSiNuevoPedido_Click" />
                                        <asp:Button runat="server" Text="No" class="btn btn-sm btn-outline-secondary" data-bs-dismiss="modal" aria-label="Close" OnClick="BtnNoNuevoPedido_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal" id="ActualizarCliente" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Grabar Cliente</h5>
                                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Por favor actualizar el registro de clientes.</span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" class="btn btn-sm btn-outline-dark" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="LlenarNIT" data-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">NIT</h5>
                                    </div>
                                    <div class="modal-body form-control-sm">
                                        <p>
                                            Antes de proceder, por favor completa los campos de NIT.<br />
                                            Haz clic en "Aceptar" para ingresar al NIT y continuar.
                                        </p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" Text="Aceptar" OnClick="Redireccion_Nit_Click" CssClass="btn btn-sm btn-outline-dark" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="NITvacio" data-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">VALIDAR NIT</h5>
                                    </div>
                                    <div class="modal-body form-control-sm">
                                        <p>
                                            La información relacionada con el NIT no ha sido proporcionada. Por favor, asegúrate de completar todos los campos requeridos antes de continuar.
                                        </p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" Text="Aceptar" CssClass="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="GuardarNIT" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID</h5>
                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>
                                            ¿Deseas realizar más cambios en esta Orden de Trabajo?<br />
                                            Si tienes más ajustes por hacer, puedes indicarlo aquí.
                                        </p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" Text="Si" CssClass="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" />
                                        <asp:Button runat="server" class="btn btn-sm btn-outline-dark" Text="No" data-bs-dismiss="modal" aria-label="Close" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="LlenarNITModificar" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">NIT</h5>
                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>
                                            ¿Desea modificar la información contable?
                                        </p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" Text="Si" OnClick="Redireccion_Nit" CssClass="btn btn-sm btn-outline-dark" />
                                        <asp:Button runat="server" class="btn btn-sm btn-outline-dark" Text="No" data-bs-dismiss="modal" aria-label="Close" OnClick="NoModificarNIT_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="fundido modal" id="ValidarAsesor" data-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content p-4 shadow">
                                    <div class="modal-header bg-light">
                                        <h5 class="modal-title">Validar Asesor</h5>
                                    </div>
                                    <div class="modal-body">
                                        <p><span id="ValidarAsesor1"></span></p>
                                    </div>
                                    <div class="modal-footer">
                                        <button runat="server" data-bs-dismiss="modal" aria-label="Close">Aceptar</button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="OTModificada" class="modal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">S_I_Ducon</h5>
                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="OTModificada1"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" Text="Sí" class="btn btn-sm btn-outline-success" data-bs-dismiss="modal" aria-label="Close" OnClick="BtnSiModificar_Click"></asp:Button>
                                        <asp:Button runat="server" Text="No" class="btn btn-sm btn-outline-secondary" data-bs-dismiss="modal" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnCotizacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade  " id="Plano-Content">
                <asp:UpdatePanel ID="PanelPlano" runat="server">
                    <ContentTemplate>

                        <div class="container-fluid">

                            <!--Modal para Acabados Tap Plano-->
                            <div class="modal fade" id="ModalAcabados" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-xl ">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="exampleModalLabel">Acabados Plano</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>

                                        <div class="modal-body">
                                            <div class="row justify-content-center mb-3">

                                                <div class="border rounded p-2">

                                                    <div class="row pb-2 mb-2">
                                                        <div class="col-12">
                                                            <div class="table-responsive mb-1 gap-2" style="max-height: 12rem; overflow-x: auto;">
                                                                <h5 class="datagrid-header text-center">Acabados</h5>
                                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridAcabados1" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" DataSourceID="AcabadosFinales">
                                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                    <Columns>
                                                                        <asp:TemplateColumn HeaderText="...">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkAcabT" ToolTip="VerDocumento" runat="server" CommandName="VerDocumento1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="oadDescripcionGrupoObjeto" HeaderText="Apliaca a" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="oadDescripcion_Familia" HeaderText="Familia Módulo" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="oadDescripcion_Insumo" HeaderText="Insumo" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="oadCodInvDes" HeaderText="Codigo Destino" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="oadDescripcionAcabado" HeaderText="Acabado" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="oadAplicacionAcabado" HeaderText="A.A" ItemStyle-CssClass="auto-width-column" />

                                                                    </Columns>
                                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="AcabadosFinales" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT
                                                                                    tblOTAcabadoDefinitivo.id_OTAcabadoDefinitivo,
                                                                                    tblOTAcabadoDefinitivo.oadPLano,
                                                                                    tblOTAcabadoDefinitivo.oadDescripcionGrupoObjeto,
                                                                                    tblOTAcabadoDefinitivo.oadID_Familia,
                                                                                    tblOTAcabadoDefinitivo.oadDescripcion_Familia,
                                                                                    tblOTAcabadoDefinitivo.oadIDGrupoAcabado,
                                                                                    tblOTAcabadoDefinitivo.oadDesGrupoAcabado,
                                                                                    tblOTAcabadoDefinitivo.oadId_Insumo,
                                                                                    tblOTAcabadoDefinitivo.oadCodInvOri,
                                                                                    tblOTAcabadoDefinitivo.oadDescripcion_Insumo,
                                                                                    tblOTAcabadoDefinitivo.oadCodInvDes,
                                                                                    tblOTAcabadoDefinitivo.oadDescripcionAcabado,
                                                                                    tblOTAcabadoDefinitivo.oadAplicacionAcabado,
                                                                                    tblOTAcabadoDefinitivo.oadActivo 
                                                                                    From tblOTAcabadoDefinitivo 
                                                                                    WHERE (((tblOTAcabadoDefinitivo.[oadPlano])=@plano))
                                                                                    order by tblOTAcabadoDefinitivo.oadDescripcion_Familia 
                                                                                    asc, tblOTAcabadoDefinitivo.oadDescripcionGrupoObjeto 
                                                                                    asc,tblOTAcabadoDefinitivo.oadAplicacionAcabado asc">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="txtPlano" PropertyName="Text" Name="plano"></asp:ControlParameter>
                                                                    </SelectParameters>
                                                                </asp:SqlDataSource>


                                                            </div>
                                                        </div>
                                                    </div>



                                                </div>

                                            </div>

                                            <div class="row justify-content-center mb-3">

                                                <div class="border rounded p-2">
                                                    <div class="row">

                                                        <div class="col-8">
                                                            <div class="table-responsive mb-1 gap-2" style="max-height: 12rem; overflow-x: auto;">
                                                                <h5 class="datagrid-header text-center">Acabado Ventas</h5>
                                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridAcabadoVentas" runat="server" AutoGenerateColumns="false" DataSourceID="AcabadosVentas" OnItemCommand="DataGridAcabadoVentas_ItemCommand">
                                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                    <Columns>
                                                                        <asp:TemplateColumn HeaderText="...">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkAcabV" ToolTip="VerDocumento" runat="server"
                                                                                    CommandName="VerDocumento1" CommandArgument='<%# Container.ItemIndex %>'
                                                                                    Text="<i class='bi bi-pencil-square'></i>"
                                                                                    OnClientClick="ocultarModal();" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                        <asp:BoundColumn HeaderText="Acabado de Ventas" DataField="AcabadoVentas" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn HeaderText="Entrega" DataField="Entrega" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Descripcion_Acabado" Visible="false" />
                                                                        <asp:BoundColumn DataField="Detalle_Adicional" Visible="false" />
                                                                        <asp:BoundColumn DataField="GrupoObjetoparaAcabado" Visible="false" />

                                                                    </Columns>
                                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="AcabadosVentas" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT
                                                                                            tblOTAcabados.*,
                                                                                            tblGrupoObjetoParaAcabado.*,
                                                                                            tblAcabado.* 
                                                                                            FROM tblAcabado 
                                                                                            INNER JOIN (tblGrupoObjetoParaAcabado 
                                                                                            INNER JOIN tblOTAcabados 
                                                                                            ON tblGrupoObjetoParaAcabado.ID_GrupoObjetoparaAcabado = tblOTAcabados.ID_GrupoObjetoparaAcabado) 
                                                                                            ON tblAcabado.ID_Acabado = tblOTAcabados.ID_Acabado 
                                                                                            WHERE (((tblOTAcabados.Id_OT)=@Id_Ot) 
                                                                                            AND ((tblOTAcabados.Consecutivo_Pedido)=@Pedido))">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="tbOT" PropertyName="Text" Name="Id_Ot"></asp:ControlParameter>
                                                                        <asp:ControlParameter ControlID="ddlNumbers" PropertyName="SelectedValue" Name="Pedido"></asp:ControlParameter>
                                                                    </SelectParameters>
                                                                </asp:SqlDataSource>

                                                            </div>
                                                        </div>

                                                        <div class="col-3 pt-4 mt-4 text-end">
                                                            <asp:Button ID="btnAgregarAcabado" runat="server" Text="Acabado de Plano" class="btn btn-sm btn-outline-secondary" OnClick="btnAgregarAcabado_Click" OnClientClick="ocultarModal()" />
                                                        </div>

                                                    </div>

                                                </div>

                                            </div>

                                        </div>



                                    </div>
                                </div>
                            </div>

                            <!--Modal Eliminar todos los objetos del plano -->
                            <div id="EliminarObjetos" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-danger text-white">
                                            <h5 class="modal-title text-center">Eliminar Objetos</h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>Esta seguro que desea eliminar los objetos del plano <span id="planoEliminar"></span></h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="eliminarObjeto" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-danger" Style="width: 5rem;" OnClick="BtnEliObjPla_Click" />
                                                <asp:Button runat="server" ID="CerrarEliminar" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal Eliminar un objeto del plano -->
                            <div id="EliminarObjeto" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-danger text-white">
                                            <h5 class="modal-title text-center">Eliminar Objeto</h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6>Esta seguro que desea eliminar el objetos  <span runat="server" id="ObjetoEliminar"></span>de ancho <span runat="server" id="anchoEliminar"></span>del plano ? </h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="quitarObjeto" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-danger" Style="width: 5rem;" OnClick="BtnQuiObjPla_Click" />
                                                <asp:Button runat="server" ID="CerrarQ" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--Modal de carga para excel ( Se deja por si la descargar demora un poco mas  -->
                            <div class="modal fade" id="loadingModalExcel" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">

                                        <div class="modal-header bg-dark text-white">
                                            <h5 class="modal-title text-center">Descargando Archivo</h5>
                                        </div>

                                        <div class="modal-body text-center">
                                            <div class="spinner-border" role="status">
                                                <span class="visually-hidden">Cargando...</span>
                                            </div>
                                            <p class="mt-2 fw-bold" id="mensajeCargando1">Descargando Excel...</p>

                                        </div>

                                        <div class="modal-footer justify-content-center">
                                            <asp:Button ID="btnTerminarDescarga" class="btn btn-primary" runat="server" disabled="disabled" Text="Terminar Descarga" data-bs-dismiss="modal" aria-label="Close" />
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <!--Modal Objetos no existentes -->
                            <div class="modal fade" id="modalObjNoExistente" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                                <div class="modal-dialog modal-lg modal-dialog-centered ">
                                    <div class="modal-content">

                                        <div class="modal-header pb-2 bg-primary">
                                            <h6 class="text-white m-0">Objetos no existentes</h6>
                                            <button type="button" class="btn-close" style="color: white!important;" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>

                                        <div class="modal-body">

                                            <div class="row justify-content-center mb-3">
                                                <div class="border rounded p-2">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                                <h5 class="datagrid-header text-center">Objetos</h5>
                                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridObjNoExiste" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemDataBound="DataGridObjNoExiste_ItemDataBound">
                                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="" HeaderText="Item" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="ID_Objeto" HeaderText="Objeto" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Ancho" HeaderText="Ancho" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Alto" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Observacion" HeaderText="Observación" ItemStyle-CssClass="auto-width-column" />

                                                                    </Columns>
                                                                </asp:DataGrid>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                        </div>

                                        <div class="modal-footer">
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <!--Nav iconos Planos-->
                            <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
                                <div class="container-fluid">

                                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                        <span class="navbar-toggler-icon"></span>
                                    </button>
                                    <div class="collapse navbar-collapse" id="PlanoIconos">
                                        <ul class="navbar-nav mx-auto">

                                            <div class="contenedor-icono">

                                                <asp:LinkButton runat="server" title="Adicionar Objeto al Plano" ID="BtnAdiObjPla" OnClick="BtnAdiObjPla_Click">
                                                     <i class="ib bi-pc"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Quitar Objeto del Plano" ID="BtnQuiObjPla" OnClick="QuitarObjeto_Click">
                                                  <i class="bi bi-database-check"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Eliminar Objetos del Plano" ID="BtnEliObjPla" OnClick="EliminarObjetos_Click">
                                                  <i class="bi bi-fire"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Acabados del Plano" ID="BtnAcaPla" OnClick="BtnAcaPla_Click">
                                                  <i class="bi bi-bar-chart-line"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Leer Archivo Despiece Acad" ID="BtnLeeArcDesAca" OnClick="BtnLeeArcDesAca_Click">
                                                 <i class="bi bi-border-inner"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Cargar Archivo TXT XY" ID="BtnCarArcTxtXy" OnClick="BtnCarArcTxtXy_Click">
                                                  <i class="bi bi-folder-plus"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Plano Bloqueado" ID="BtnPlaBlo" OnClick="BtnPlaBlo_Click">
                                                     <i class="bi bi-lock"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Crear o Redefinir Bolsa" ID="BtnCreRefBol">
                                                      <i class="bi bi-bag-check"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Adicionar Elementos a la Bolsa (Modificar Bolsa)" ID="BtnAdiRemEleBol">
                                                         <i class="bi bi-bag-plus"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Despiece del Plano" ID="BtnDesPla">
                                                    <i class="bi bi-disc-fill"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Generar  TXT" ID="BtnGenTxt">
                                                 <i class="bi bi-filetype-txt"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Guardar TXT" ID="BtnGuaTxt">
                                                    <%--<i class="bi bi-save2"></i>--%>
                                                    <i class="bi bi-sd-card-fill"></i> <%--Icono Guardar--%>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Exportar Plano u Orden de Trabajo" ID="BtnExpPlaOrdTra">
                                                   <i class="bi bi-arrow-up-left-circle"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Visualizar/Generar Cotizacion" ID="BtnVisGenCot" OnClick="BtnVisGenCot_Click" OnClientClick="CargarExcel();">
                                                    <i class="bi bi-bag-plus"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Objetos no Existentes" ID="BtnObjNoExi" OnClick="BtnObjNoExi_Click">
                                                    <i class="bi bi-text-indent-left"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Actualizar Precio Prototipo" ID="BtnActPrePro">
                                                     <i class="bi bi-cash-coin"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Generar Formato Certificado de Origen" ID="BtnGenForCerOrd">
                                                  <i class="bi bi-clipboard-check"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Importar Plano de Actualizacion de Bloques" ID="BtnImpPlaActBlo">
                                                 <i class="bi bi-file-arrow-down-fill"></i>
                                                </asp:LinkButton>

                                                <ul />
                                        </ul>
                                    </div>

                                </div>
                            </nav>

                            <div class=" container-fluid panel-plano">

                                <div class="container-fluid descripcion-plano border border-opacity-50 rounded p-3 shadow-sm">

                                    <div class="row pb-2">
                                        <div class="col-3">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:Button ID="btnPlano" type="button" Text="Plano" class="btn btn-outline-secondary" runat="server" OnClick="Redireccion_Plano1"></asp:Button>

                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:TextBox ID="txtPlano" type="text" class="form-control  input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pb-2">

                                        <div class="col-3">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:Label ID="lblCliente" class="form-label" Text="Cliente" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:TextBox ID="txtCliente" type="text" class="form-control input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">

                                        <div class="col-3">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:Label ID="lblArea" class="form-label" Text="Área" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:TextBox ID="txtArea" type="text" class="form-control input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">

                                        <div class="col-3">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:Label ID="Label2" class="form-label" Text="Contacto" runat="server"> </asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:TextBox ID="txtContactoPlano" type="text" class="form-control input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">
                                        <div class="col-3">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:Label ID="lblAsesor" class="form-label" Text="Asesor" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:TextBox ID="txtAsesorPlano" type="text" class="form-control input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">
                                        <div class="col-3">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:Label ID="lblDibuja" class="form-label" Text="Dibuja" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:TextBox ID="txtDibuja" type="text" class="form-control input" runat="server"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pb-2">
                                        <div class="col-3">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:Label ID="Label3" class="form-label" Text="Bolsa" runat="server"></asp:Label>

                                            </div>
                                        </div>

                                        <div class="col-9">
                                            <div class="input-group input-group-sm gap-2 ">
                                                <asp:TextBox ID="txtBolsa" type="text" class="form-control input " runat="server"></asp:TextBox>
                                            </div>
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

                                    <div class="row justify-content-center p-2">
                                        <div class="border rounded p-1 m-1">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="table-responsive mb-1" style="max-height: 7rem; overflow-x: auto;">
                                                        <h5 class="datagrid-header text-center">Acabado objeto</h5>
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridAcabado" runat="server">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                            <Columns>
                                                                <asp:BoundColumn DataField="oadDescripcionAcabado" HeaderText="Acabado" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="oadDescripcion_Insumo" HeaderText="Apliacado a" ItemStyle-CssClass="auto-width-column" />


                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </div>




                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="container-fluid tabla-plano border rounded shadow-sm">
                                    <div class="row justify-content-center p-2">
                                        <div class="border rounded p-1 m-1">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="table-responsive mb-1" style="max-height: 30rem; height: 32rem; overflow-x: auto;">
                                                        <h5 class="datagrid-header text-center">Despiece</h5>
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridDespiecePlano" runat="server" OnItemDataBound="DataGridDespiecePlano_ItemDataBound" OnItemCommand="DataGridDespiecePlano_LinkButton">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="...">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="VerPlano" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>"
                                                                            Visible='<%# !string.IsNullOrEmpty(Eval("ID")?.ToString()) %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>

                                                                <asp:TemplateColumn ItemStyle-Width="250px" ItemStyle-CssClass="auto-width-column">
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

                            <div class=" container-fluid panel-tabla border p-2 rounded  ">

                                <div class="row justify-content-center p-2">
                                    <div class="border rounded pb-2 mb-2">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="table-responsive mb-1" style="max-height: 11rem; overflow-x: auto;">
                                                    <h5 class="datagrid-header text-center">Descripcion Objetos</h5>
                                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridDescripcionObjetos" runat="server" OnItemDataBound="DataGridDescripcionObjeto_ItemDataBound" OnItemCommand="DataGridDescripcionObjeto_LinkButton">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="...">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="VerAcabado" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
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
                                                            <asp:BoundColumn DataField="ID_Familia" Visible="false" />

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
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnVisGenCot" />
                    </Triggers>
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

                                                <asp:LinkButton runat="server" title="Nuevo Objeto" ID="BtnNueObj">
                                                    <i class="bi bi-file-earmark"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="..." ID="Btnnnn">
                                                  <i class="bi bi-printer"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Modificar Objeto" ID="BtnModObj">
                                                   <i class="bi bi-wrench"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Consultar Objeto" ID="BtnConObj">
                                                  <i class="bi bi-file-earmark-ruled"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Eliminar Objeto" ID="BtnEliObj">
                                                   <i class="bi bi-database-x"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Buscar Objeto" ID="BtnBusObj">
                                                  <i class="bi bi-search"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Copiar Objeto" ID="BtnCopObj">
                                                    <i class="bi bi-files"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Actualizar Precio" ID="BtnActPre">
                                                      <i class="bi bi-currency-dollar"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Generar Lista de Precios" ID="BtnGenLisPre">
                                                        <i class="bi bi-coin"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Ir al Objeto Anterior" ID="BtnIrObjAnt">
                                                    <i class="bi bi-disc"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Chequear" ID="BtnChe">
                                                    <i class="bi bi-check-lg"></i>
                                                </asp:LinkButton>


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
                                            <asp:SqlDataSource runat="server" ID="GrupoObjetos" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select  ID_GrupoObjeto AS Valor,Descripcion_Grupo AS Descripcion from tblGrupoObjeto  order by Descripcion_Grupo "></asp:SqlDataSource>

                                        </div>
                                    </div>

                                    <div class="col-3">
                                        <div class="input-group-sm">
                                            <asp:Label class="form-label" Text="Criterio" runat="server" ID="lbCriterio"></asp:Label>
                                            <asp:TextBox ID="tbCriterio" runat="server" CssClass="form-control" onkeydown="handleEnterKeyPress(event)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-1">
                                        <div class="input-group-sm">
                                            <asp:Label class="form-label" Text="Altura" runat="server" ID="lbAltura"></asp:Label>
                                            <asp:TextBox ID="tbAltura" runat="server" CssClass="form-control" onkeydown="handleEnterKeyPress(event)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-1">
                                        <div class="input-group-sm">
                                            <asp:Label class="form-label" Text="Ancho" runat="server" ID="lbAncho"></asp:Label>
                                            <asp:TextBox ID="tbAncho" runat="server" CssClass="form-control" onkeydown="handleEnterKeyPress(event)"></asp:TextBox>
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
                                                <div class="table-responsive mb-1" style="max-height: 14rem; height: 14rem; overflow-x: auto;">

                                                    <h5 class="datagrid-header text-center">Objeto</h5>

                                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridObjetos" runat="server" DataSourceID="ObtenerDatosObjetos" OnItemCommand=" DataGridObtenerDatosObjetos_LinkButton" OnItemDataBound="DataGridObtenerDatosObjetos_ItemDataBound">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="...">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkObjetoDetallado" CssClass="Tam" runat="server" CommandName="VerObjetoDet" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
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
                                                            <asp:BoundColumn DataField="Chequeado" HeaderText="Ok" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:Si;No}" />
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

                                                    <asp:SqlDataSource ID="ObtenerDatosObjetos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommandType="StoredProcedure">
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
                                            <asp:LinkButton ID="btnDespiece" Text="Despiece" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="Reedireccion_ObjetoDespiece">

                                            </asp:LinkButton>
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
                                                <div class="table-responsive mb-1" style="max-height: 9rem; height: 9rem; overflow-x: auto;">
                                                    <h5 class="datagrid-header text-center">Modulo del Objeto</h5>
                                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" AutoGenerateColumns="false" ID="DataGridModuloObjetos" runat="server">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="...">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkView2" CssClass="Tam" runat="server" CommandName="VerModulo" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Num_Fila" HeaderText="Item" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Id_Modulo" HeaderText="Módulo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_TipoModulo" HeaderText="Tipo Módulo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Chequeado" HeaderText="OK" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Ubicacion_Modulo" HeaderText="Pos" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Altura" HeaderText="Altura" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Lado" HeaderText="Lado" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Descripcion_Familia" HeaderText="Grupo" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />


                                                        </Columns>

                                                    </asp:DataGrid>

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


                                                <asp:LinkButton runat="server" title="" ID="LinkButton1">
                                                    <i class="bi bi-file-earmark"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton2">
                                                  <i class="bi bi-file-earmark"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton3">
                                                   <i class="bi bi-file-medical"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton4">
                                                   <i class="bi bi-wrench"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton5">
                                                    <i class="bi bi-file-earmark-ruled"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton6">
                                                  <i class="bi bi-database-down"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton7">
                                                   <i class="bi bi-files"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton8">
                                                  <i class="bi bi-check-lg"></i>
                                                </asp:LinkButton>

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

                                                <asp:LinkButton runat="server" title="Nuevo Insumo" ID="LinkButton9">
                                                   <i class="bi bi-file-earmark"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="" ID="LinkButton10">
                                                  <i class="bi bi-file-earmark-ruled"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Modificar Insumo" ID="LinkButton11">
                                                   <i class="bi bi-wrench"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Eliminar Insumo" ID="LinkButton12">
                                                   <i class="bi bi-database-x"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Copiar Insumo" ID="LinkButton13">
                                                    <i class="bi bi-files"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Buscar Insumo" ID="LinkButton14">
                                                   <i class="bi bi-search"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Actualizar" ID="LinkButton15">
                                                   <i class="bi bi-disc"></i>
                                                </asp:LinkButton>

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


        <div class="modal" id="CarteraVencida" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">CLIENTE CON CARTERA VENCIDA</h5>
                        <button type="button" class="btn-close-white btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p><span id="CarteraVencida2"></span></p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

        <div id="miModallll" class="modal" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Fecha de empaque</h5>

                    </div>
                    <div class="modal-body">
                        <p>La fecha debe ser al menos 3 días laborales después de la fecha actual.</p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

        <div id="ErrorPermiso" class="modal" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white" id="modallLabel">SID_DUCON</h5>

                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>No tiene permisos para realizar esta accion</p>
                    </div>
                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                    </div>
                </div>
            </div>
        </div>

        <!--Modal para cargar archivo para leer ACAD TXT -->
        <div class="modal fade" id="ModalArchivo" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg ">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title" id="Acad">Leer Archivo Autocad</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="modal-body">

                        <div class="row">

                            <div class="col-8">
                                <asp:FileUpload CssClass="form-control " ID="LeerAcad" runat="server" accept=".txt, .xls" />
                            </div>

                            <div class="col-4 text-center">
                                <asp:Button ID="btnCargarAcad" runat="server" Text="Cargar" CssClass="btn btn-group-lg btn-outline-secondary" OnClick="btnCargarAcad_Click" OnClientClick="MostrarSpiner();" />
                            </div>

                        </div>

                    </div>

                </div>
            </div>
        </div>

        <!--Modal de carga proceso Archivo TXT -->
        <div class="modal fade" id="loadingModal1" tabindex="-1" aria-labelledby="loadingModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-body text-center">
                        <div class="spinner-border" role="status">
                            <span class="visually-hidden">Cargando...</span>
                        </div>
                        <p class="mt-2">Leyendo Archivo TXT...</p>
                    </div>
                </div>
            </div>
        </div>


        <!--Modal de carga para el proceso de Boton OK-->
        <div class="modal fade" id="OkCargando" tabindex="-1" aria-labelledby="loadingModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered ">
                <div class="modal-content">
                    <div class="modal-header pb-1 mb-1 bg-primary text-white">
                        <!-- Clase bg-primary para el fondo azul y text-white para el texto blanco -->
                        <h6 class="modal-title text-center">Terminando Orden de Trabajo</h6>
                    </div>
                    <div class="modal-body text-center border rounded p-2 m-2">
                        <div class="spinner-border" role="status">
                            <span class="visually-hidden">Cargando...</span>
                        </div>
                        <p class="mt-2 fw-bold" id="mensajeCargando">....</p>
                    </div>
                    <div class="modal-footer pt-1 mt-1">
                    </div>
                </div>
            </div>
        </div>

        <!--Modal para cargar archivo para leer ACAD TXT_XY -->
        <div class="modal fade" id="modalArchivoAcad_XY" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
            <div class="modal-dialog modal-lg modal-dialog-centered  ">
                <div class="modal-content">

                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title">Leer Archivo Autocad</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div class="modal-body">

                        <div class="row pb-3">
                            <div class="col-12">
                                <div class="progress">
                                    <div id="progressBar" class="progress-bar" role="progressbar" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                        <span id="progressMessage">Cargando...</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row pb-3">
                            <div class="col-6">
                                <h6>Por favor, cargue el archivo TXT para realizar el despiece.</h6>
                                <hr class="pt-0 mt-0 mb-2" />
                            </div>

                            <div class="col-1"></div>

                            <div class="col-5">
                                <div class="input-group input-group-sm gap-2">
                                    <asp:CheckBox ID="chkElemExit" CssClass="form-check" runat="server" ToolTip="Seleccione la casilla si desea cargar los elementos existentes." />
                                    <asp:Label ID="Label8" runat="server" Text="Cargar Elementos Existentes" ToolTip="Seleccione la casilla si desea cargar los elementos existentes."></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="row pt-3">

                            <div class="col-8">
                                <asp:FileUpload CssClass="form-control " ID="LeerAcad_XY" runat="server" accept=".txt, .xls" />
                            </div>

                            <div class="col-4 text-center">
                                <asp:Button ID="btnCargarTXT_XY" runat="server" Text="Cargar" CssClass="btn btn-group-lg btn-outline-secondary" OnClick="btnCargarTXT_XY_Click" />
                            </div>

                        </div>

                    </div>

                </div>
            </div>
        </div>

        <!--Modal exito carga TXT -->
        <div id="ModalExito" class="modal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h5 class="modal-title text-center" runat="server" id="tituloExito"></h5>

                    </div>
                    <div class="modal-body border rounded">
                        <div class="container-fluid">
                            <h6 runat="server" id="mensajeExito"></h6>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                            <asp:Button runat="server" ID="AceptarError" Text="Aceptar" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-secondary" Style="width: 5rem;" OnClick="AceptarError_Click" />
                        </div>

                    </div>
                </div>
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

    <script type="text/javascript">
        function mostrarConvenciones() {
            var filaConvenciones = document.getElementById('filaCovenciones');
            // Cambiar el estado de visibilidad
            if (filaConvenciones.style.display === 'none') {
                filaConvenciones.style.display = 'block';
            } else {
                filaConvenciones.style.display = 'none';
            }

            actualizarValor();
        }
    </script>

    <script>   
        function actualizarValor() {
            // Obtener el valor del TextBox tbOT
            var valorTextBox = document.getElementById('tbOT').value;

            // Actualizar el contenido del span con el valor del TextBox
            document.getElementById('OtBolsa').innerText = valorTextBox;
            document.getElementById('OtDocumentacion').innerText = valorTextBox;
        }
    </script>

    <script type="text/javascript">
        function validarCotizacion() {
            var txtCotizacion = document.getElementById('<%= txtCotizacion.ClientID %>');

            if (txtCotizacion.value.toUpperCase() === "NO TIENE") {
                alert('Este consecutivo de pedido no tiene cotización.');
                return false;
            }
            return true;
        }
    </script>

    <script type="text/javascript">
        function handleEnterKeyPress(event) {
            if (event.keyCode === 13) {
                event.preventDefault();  // Evitar que se envíe el formulario
                document.getElementById('<%= btnBuscarActivos.ClientID %>').click(); // Hacer clic en el botón de búsqueda
            }
        }
    </script>

    <script>   
        function actualizarValorBotonOk() {
            // Obtener el valor del TextBox
            var OT = document.getElementById('tbOT').value;
            var Ped = document.getElementById('ddlNumbers').value;
            // Actualizar el contenido del span con el valor del TextBox
            document.getElementById('OTBotonOk').innerText = OT;
            document.getElementById('PedBotonOk').innerText = Ped;
        }
    </script>

    <script>   
        function actualizarPlanoEliminar() {
            // Obtener el valor del TextBox
            var plano = document.getElementById('txtPlano').value;

            // Actualizar el contenido del span con el valor del TextBox
            document.getElementById('planoEliminar').innerText = plano;

        }

        // Poner el foco en una fila seleccionada 
        function focusAndScrollToRow(rowId) {
            var row = document.getElementById(rowId);
            if (row) {
                row.setAttribute('tabindex', '-1'); // Make it focusable
                row.focus();
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });


            }
        }
    </script>




    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
