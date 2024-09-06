<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Diseño_Venta.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Diseño_Venta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
      <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/css/bootstrap.min.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/Ventas/Diseño_Venta.css" rel="stylesheet" />
    <title>Diseño - Departamento de Ventas</title>

      <script>
          function focusAndScrollToRow(rowId) {
              var row = document.getElementById(rowId);
              if (row) {
                  row.setAttribute('tabindex', '-1'); // Make it focusable
                  row.focus();
                  row.scrollIntoView({ behavior: 'smooth', block: 'center' });
              }
          }
      </script>

    <script>
        function activarPestana(pestanaId, contenidoId) {
            // Desactivar la pestaña actualmente activa
            var activeTab = document.querySelector(".nav-link.active");
            if (activeTab) {
                activeTab.classList.remove("active");
            }

            var activePane = document.querySelector(".tab-pane.show.active");
            if (activePane) {
                activePane.classList.remove("show", "active");
            }

            // Mostrar y activar la nueva pestaña
            var newTab = document.getElementById(pestanaId);
            var newPane = document.getElementById(contenidoId);

            if (newTab) {
                newTab.style.display = 'block';
                newTab.classList.add("active");
            }

            if (newPane) {
                newPane.classList.add("show", "active");
            }
        }
    </script>


    <script>
        function triggerFileUpload2() {
            document.getElementById('<%= FileUpload1.ClientID %>').click();
        }

        function showFileName2() {
            var fileUpload = document.getElementById('<%= FileUpload1.ClientID %>');
        var textBox = document.getElementById('<%= TextBox1.ClientID %>');
        if (fileUpload.files.length > 0) {
            textBox.value = fileUpload.files[0].name;
        }
        }

    </script>

 <script type="text/javascript">
     function showLoadingAnimation2() {
         var loadingAnimation2 = $('#loadingAnimation2');
         var progressBar2 = $('#progressBar2');
         var progressMessage2 = $('#progressMessage2');

         loadingAnimation2.show(); // Mostrar la animación de carga

         // Array de mensajes
         var messages2 = ["Cargando...", "Por favor, espere...", "Estamos procesando su solicitud...", "Gracias por su paciencia..."];
         var messageIndex2 = 0;

         // Cambiar mensajes de forma periódica
         var interval2 = setInterval(function () {
             messageIndex2 = (messageIndex2 + 1) % messages2.length; // Cambiar el mensaje
             progressMessage2.text(messages2[messageIndex2]); // Actualizar el mensaje

             // Simulación de progreso
             var progress2 = (messageIndex2 + 1) * 25; // Incrementar el progreso
             progressBar2.css('width', progress2 + '%').attr('aria-valuenow', progress2); // Actualizar la barra de progreso
         }, 2000); // Cambiar mensaje cada 2 segundos

         // Detener la animación cuando el progreso se complete o se termine la operación
         $('#<%= Btn.ClientID %>').on('click', function () {
             clearInterval(interval2);
             progressBar2.css('width', '100%').attr('aria-valuenow', 100); // Completar la barra de progreso
         });
     }
 </script>

    <script type="text/javascript">
        function showLoadingAnimation3() {
            var loadingAnimation2 = $('#loadingAnimation3');
            var progressBar2 = $('#progressBar3');
            var progressMessage2 = $('#progressMessage3');

            loadingAnimation2.show(); // Mostrar la animación de carga

            // Array de mensajes
            var messages2 = ["Cargando...", "Por favor, espere...", "Estamos procesando su solicitud...", "Gracias por su paciencia..."];
            var messageIndex2 = 0;

            // Cambiar mensajes de forma periódica
            var interval2 = setInterval(function () {
                messageIndex2 = (messageIndex2 + 1) % messages2.length; // Cambiar el mensaje
                progressMessage2.text(messages2[messageIndex2]); // Actualizar el mensaje

                // Simulación de progreso
                var progress2 = (messageIndex2 + 1) * 25; // Incrementar el progreso
                progressBar2.css('width', progress2 + '%').attr('aria-valuenow', progress2); // Actualizar la barra de progreso
            }, 2000); // Cambiar mensaje cada 2 segundos

            // Detener la animación cuando el progreso se complete o se termine la operación
            $('#<%= BtnAdjuntarOtro.ClientID %>').on('click', function () {
                clearInterval(interval2);
                progressBar2.css('width', '100%').attr('aria-valuenow', 100); // Completar la barra de progreso
            });

            // Detener la animación cuando el progreso se complete o se termine la operación
            $('#<%= BtnNoAdjuntarOtro.ClientID %>').on('click', function () {
                clearInterval(interval2);
                progressBar2.css('width', '100%').attr('aria-valuenow', 100); // Completar la barra de progreso
            });
        }
    </script>


<script>
    function triggerFileUpload() {
        document.getElementById('<%= FileUpload2.ClientID %>').click();
    }

    function showFileName() {
        var fileUpload = document.getElementById('<%= FileUpload2.ClientID %>');
        var textBox = document.getElementById('<%= txtFileName.ClientID %>');
        if (fileUpload.files.length > 0) {
            textBox.value = fileUpload.files[0].name;
        }
    }

    $(document).ready(function () {
        $('#<%= btnCargar.ClientID %>').on('click', function () {
            showLoadingAnimation();
        });
    });

    function showLoadingAnimation() {
        var loadingAnimation = $('#loadingAnimation');
        var progressBar = $('#progressBar');
        var progressMessage = $('#progressMessage');
        
        loadingAnimation.show(); // Mostrar la animación de carga

        // Array de mensajes
        var messages = ["Cargando...", "Por favor, espere...", "Estamos procesando su solicitud...", "Gracias por su paciencia..."];
        var messageIndex = 0;

        // Cambiar mensajes de forma periódica
        var interval = setInterval(function () {
            messageIndex = (messageIndex + 1) % messages.length; // Cambiar el mensaje
            progressMessage.text(messages[messageIndex]); // Actualizar el mensaje

            // Simulación de progreso
            var progress = (messageIndex + 1) * 25; // Incrementar el progreso
            progressBar.css('width', progress + '%').attr('aria-valuenow', progress); // Actualizar la barra de progreso
        }, 2000); // Cambiar mensaje cada 2 segundos

        // Detener la animación cuando el formulario se envíe
        $('#<%= btnCargar.ClientID %>').on('click', function () {
            clearInterval(interval);
            progressBar.css('width', '100%').attr('aria-valuenow', 100); // Completar la barra de progreso
        });
    }
    </script>

      





</head>
<body translate="no">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Literal ID="litModalScript" runat="server"></asp:Literal>



        <nav class="navbar navbar-light bg-light navbar-custom shadow">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">
                    <li class="nav-item">
                        <a class="nav-link text-white fw-bold" id="Diseño-BitacoraFPV-001-tab" data-bs-toggle="tab" href="#Diseño-BitacoraFPV-001-content"> <i class="bi bi-file-text"></i> Diseño-Bitacora FPV-001</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white active fw-bold" id="Programacion-tab" data-bs-toggle="tab" href="#Programacion-content"><i class="bi bi-table"></i> Programación</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white fw-bold" id="Buscar-tab" data-bs-toggle="tab" href="#Buscar-content" style="display: none;"><i class="bi bi-search"></i> Buscar Diseño</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white fw-bold" id="Buscar-tabPlano" data-bs-toggle="tab" href="#Plano-content" style="display: none;"><i class="bi bi-file-image-fill"></i> Plano</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-white fw-bold" id="Despiece-tab" data-bs-toggle="tab" href="#Despiece-content" style="display: none;"><i class="bi bi-tools"></i> Despiece</a>
                    </li>

                </ul>
            </div>
        </nav>

        <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade" id="Despiece-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container p-1 mt-3 border shadow">
                            <div class="row">
                                <!-- Primera columna -->
                                <div class="col-lg-7 col-md-6 col-sm-12" id="primeraColumna">
                                    <div class="p-3 m-2 shadow-sm" style="min-height: 50rem;">
                                       <asp:Label runat="server" ID="PlanoDiseArea"></asp:Label>
                                        <div class="table-responsive mb-2 gap-2" style="max-height: 48rem; overflow-x: auto;">
                                            <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGridDespiece" runat="server" AutoGenerateColumns="false"
                                                OnItemDataBound="DataGridDespiece_ItemDataBound" OnItemCommand="DataGridDespiece_ItemCommand"  DataKeyField="Id_Numerico">
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnSelec2" runat="server" CommandName="Id_Numerico" CommandArgument='<%# Container.ItemIndex %>'
                                                                Text='<%# Eval("Id_Numerico") != DBNull.Value ? "<i class=\"bi bi-pencil-square text-dark\"></i>" : "" %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    
                                                    <asp:BoundColumn DataField="Id_Numerico" HeaderText="ID" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Descripcion" ItemStyle-CssClass="auto-width-column2">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("Descripcion_Grupo") %>' Font-Bold='<%# Eval("IsGroupRow").ToString() == "True" ? true : false %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Ancho" HeaderText="A" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Precio_Venta" HeaderText="V.Unitario" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="ValorActual" HeaderText="Sub Total" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                     <asp:BoundColumn DataField="RevisadoDibujo" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                     <asp:BoundColumn DataField="ID_GrupoObjeto" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                   
                                                </Columns>
                                            </asp:DataGrid>  
                                        </div>
                                    </div>
                                </div>
                                <!-- Segunda columna -->
                                <div class="col-lg-5 col-md-6 col-sm-12" id="segundaColumna">
                                    <div class="p-3 m-2" style="max-height: 50rem; min-height: 50rem;">
                                        <div class="row p-3">
                                            <div class="col-12 d-flex align-items-center">
                                                <!-- Botón Agregar -->
                                                <div class="col-auto p-3 m-1" style="min-height: 8rem;">
                                  <asp:LinkButton runat="server" title="Nuevo plano" ID="LinkButton10" CssClass="btn btn-sm button-enabled" OnClick="Objetos_Click">
                                        <img src="https://i.ibb.co/Qp7VGnN/icons8-xbox-cruz-windows-11-filled-96.png" alt="Nuevo plano" style="width: 30px; height: 30px;" />
                                    </asp:LinkButton>
                                </div>
                                <!-- Espaciador flexible -->
                                <div class="col-lg-3 col-md-2 col-sm-2 col-1"></div>
                                <!-- Cantidad -->
                                <div class="col-lg-7 col-md-8 col-sm-10 col-12">
                                    <div class="p-3 m-2" style="min-height: 8rem;">
                                        <div class="input-group input-group-sm gap-2">
                                            <asp:Label runat="server" ID="Label18" class="col-form-label-sm">Cantidad</asp:Label>
                                            <asp:TextBox ID="TextCamCan" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                            <asp:Button runat="server" ID="BtnCambiarCantidad" Text="Cambiar" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" OnClick="ModaldeConfirmacionCambiarCantidad_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Módulos del panel -->
                        <div class="container mb-3" style="max-height: 18rem; min-height: 18rem;">
                          
                                <div class="col-12">
                                    <h6>Módulos del panel</h6>
                                       <div class="table-responsive mb-2 gap-2" style="max-height: 16rem; overflow-x: auto;">
                                            <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGrid6" runat="server" AutoGenerateColumns="false"  OnItemCommand="DataGrid6_ItemCommand">
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="BtnSelec2" runat="server" CommandName="Id_Modulo" CommandArgument='<%# Container.ItemIndex %>'
                                                            Text="<i class='bi bi-pencil-square'></i>"/>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>     
                                                      <asp:BoundColumn DataField="Id_Modulo" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Descripcion_Modulo" HeaderText="Descripción" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Ubicacion_Modulo" HeaderText="P" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Altura" HeaderText="H" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="A" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Cantidad" HeaderText="C" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Lado" HeaderText="L" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>

                                                </Columns>
                                            </asp:DataGrid>
                                       </div>
                                </div>

                        </div>
                                        <div class="container mb-3">
                                            <div class="row align-items-center">
                                                <div class="col-lg-6 col-md-4 col-sm-12 mb-2">
                                                    <div class="input-group input-group-sm">
                                                        <asp:CheckBox runat="server" ID="CheckBox2" CssClass="form-check" onchange="cambiarAnchoColumnas(this)" />
                                                        <asp:Label runat="server" ID="Label20" class="form-label ms-2" Text="Ampliar Modulos"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="col-lg-2 col-md-4 col-sm-12 mb-2"></div>
                                                <div class="col-lg-4 col-md-4 col-sm-12 mb-2">
                                                    <div class="d-flex justify-content-end gap-2">
                                                        <asp:LinkButton runat="server" title="Nuevo objeto" ID="BtnNueObjDes" CssClass="btn btn-sm shadow button-enabled" OnClick="BtnNueObjDes_Click">
                                            <i class="bi bi-file-earmark-fill GrisClaro"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" title="Adicionar Modulo" ID="BtnAdiModDes" CssClass="btn btn-sm shadow button-enabled" OnClick="BtnAdiMod_Click">
                                             <img src="https://i.ibb.co/xCpDzy1/icons8-documentos-96.png" alt="Nuevo plano" style="width: 15px; height: 18px;" />
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" title="Configurar Objeto" ID="BtnConObjDes" CssClass="btn btn-sm shadow button-enabled" OnClick="BtnConObjDes_Click">
                                         <i class="bi bi-wrench-adjustable GrisClaro"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Observaciones Plano -->
                                        <div class="container mt-4 mb-2">
                                            <div class="row">
                                                <div class="col-12">
                                                    <h6>Observaciones Plano</h6>
                                                    <textarea id="TextArea1" runat="server" rows="5" class="form-control shadow-sm"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Link Buttons -->
                                        <div class="container mt-3">
                                            <div class="row">
                                                <div class="col-12 d-flex gap-2">
                                                    <asp:LinkButton runat="server" title="Nuevo plano" ID="LinkButton7" OnClick="LinkButton7_Click">
                                                         <img src="https://i.ibb.co/BCtb1QS/icons8-archivo-dxf-autocad-windows-11-color-310.png" alt="Nuevo plano" style="width: 40px; height: 40px;" />
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" title="Nuevo plano" ID="LinkButton8" CssClass="btn btn-sm">
                                                         <img src="https://img.icons8.com/3d-fluency/94/print.png" alt="Nuevo plano" style="width: 40px; height: 40px;" />
                                                    </asp:LinkButton>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="modalObjNoExistente" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false" aria-labelledby="staticBackdropLabel">
                            <div class="modal-dialog modal-lg modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-title d-flex align-items-center justify-content-center text-white p-2" style="background: #0863a4">
                                        <h5 class="text-white m-0">Objetos no existentes</h5>
                                    </div>
                                    <div class="modal-body bg-light">
                                        <div class="row justify-content-center mb-3">
                                            <div class="border rounded p-2">
                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="table-responsive mb-1 gap-2" style="max-height: 20.7rem; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridObjNoExiste"
                                                                runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemDataBound="DataGridObjNoExiste_ItemDataBound" OnItemCommand="DataGridObjNoExiste_ItemCommand">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="BtnObjetoNoExistente" runat="server" CommandName="ID_Objeto"
                                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="" HeaderText="Item" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="ID_Objeto" HeaderText="Objeto" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Ancho" HeaderText="Ancho" ItemStyle-CssClass="auto-width-column" />
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
                                    <div class="modal-footer bg-light">
                                        <div class="d-flex col-12">
                                            <div class="col-10">
                                                <asp:TextBox ID="TextObjNoExi" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm grande"></asp:TextBox>
                                            </div>
                                            <div class="col-2">
                                                <asp:LinkButton runat="server" title="Nuevo plano" ID="ExcelDeObjetosNoExistentes" OnClick="ExcelDeObjetosNoExistentes_Click">
                                    <img src="https://i.ibb.co/86fR8JK/icons8-microsoft-excel-2019-48.png" alt="Nuevo plano" style="width: 40px; height: 40px;" />
                                                </asp:LinkButton>
                                                <asp:Button runat="server" ID="BtnCerrarObjNoExi" class="btn btn-sm border" data-bs-dismiss="modal" aria-label="Close" Text="Cerrar" OnClick="BtnCerrarObjNoExi_Click"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="DigitarCantidad" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Despiece</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Por favor digita la cantidad que deseas cambiar</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="DiseñoTerminado" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>No puede revisar el Despiece, una vez que el Diseño este terminado</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="modal23" class="modal fade" tabindex="-1" role="dialog">
                            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                                <div class="modal-content">
                                    <div class="modal-body">
                                        <h6>Resumen Dibujante</h6>
                                        <div class="row">
                                            <div class="border rounded">
                                                <div class="table-responsive" style="max-height: 400px">
                                                    <asp:DataGrid ID="DataGridNoExistentes" runat="server" AutoGenerateColumns="False" Class="table table-bordered table-hover table-sm">
                                                        <Columns>
                                                            <asp:BoundColumn DataField="Item" HeaderText="Item" />
                                                            <asp:BoundColumn DataField="Objeto" HeaderText="Objeto" />
                                                            <asp:BoundColumn DataField="Ancho" HeaderText="Ancho" />
                                                            <asp:BoundColumn DataField="Cant" HeaderText="Cant" />
                                                            <asp:BoundColumn DataField="Observacion" HeaderText="Observación" />
                                                        </Columns>
                                                    </asp:DataGrid>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="SeleccionFila" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Despiece</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Selecciona la fila donde deseas aplicar el cambio</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="ModaldeConfirmacionCambiarCantidad" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Despiece</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="ModaldeConfirmacionCambiarCantidad2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Si" aria-label="Close" OnClick="BtnCambiarCantidad_Click"></asp:Button>
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="No" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>



        
                        <asp:HiddenField ID="hdnUserConfirmed" runat="server" />
                        <asp:Button ID="btnHidden" runat="server" Style="display:none;" OnClick="btnHidden_Click" />

      


                    </ContentTemplate>
                      <Triggers>
                        <asp:PostBackTrigger ControlID="ExcelDeObjetosNoExistentes" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Plano-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container">
                            <div class="row">
                                <div class="border shadow rounded m-2" style="min-height: 50rem">
                                    <div class="rounded m-2">
                                        <div class="container border p-1">
                                            <div class="d-flex flex-wrap">
                                                <!-- Columna 1 -->
                                                <div class="col-lg-3 col-md-4 col-sm-12 col-xs-12">
                                                    <div class="p-3 m-1 border bg-light shadow-sm" style="height: 25rem;">
                                                        <div class="row mt-1">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="lblPlano" class="col-form-label-sm">Plano</asp:Label>
                                                                    <asp:TextBox ID="TextPlano" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label9" class="col-form-label-sm">Lectura Despiece</asp:Label>
                                                                    <asp:TextBox ID="TextLecDes" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label8" class="col-form-label-sm">Dibujante</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="DropBib" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" DataSourceID="CargarDibujante" DataTextField="Dibujante"></asp:DropDownList>
                                                                    <asp:SqlDataSource ID="CargarDibujante" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select Nombre + ' ' + Apellidos as Dibujante from tblEmpleado  where tblEmpleado.Dependencia=2"></asp:SqlDataSource>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label10" class="col-form-label-sm">Asesor</asp:Label>
                                                                    <asp:DropDownList runat="server" ID="DropAsesor" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" DataSourceID="CargarAsesor" DataValueField="Asesor"></asp:DropDownList>
                                                                    <asp:SqlDataSource ID="CargarAsesor" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Select Nombre + ' ' + Apellidos as Asesor, Cedula from tblAsesorComercial where activo=1 Order By nombre asc"></asp:SqlDataSource>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label11" class="col-form-label-sm">Cliente:</asp:Label>
                                                                    <asp:TextBox ID="TextClienteDise" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label12" class="col-form-label-sm">Area:</asp:Label>
                                                                    <textarea id="TextAreaArea" runat="server" class="form-control"></textarea>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label13" class="col-form-label-sm">Contacto:</asp:Label>
                                                                    <asp:TextBox ID="TextContactoPlano" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Button runat="server" Text="Nuevo" ID="BtnNuePlano" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" OnClick="NuevoPlano_Click" />
                                                                    <asp:Button runat="server" Text="Grabar" ID="BtnGrabPlano" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" OnClick="GrabarPlanoDise_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Button runat="server" Text="Modificar" ID="BtnModificarPlano" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" OnClick="ModificarPlano_Click" />
                                                                    <asp:Button runat="server" Text="Cancelar" ID="BtnCancelarPlano" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" OnClick="CancelarPlano_Click" />
                                                                    <asp:Button runat="server" Text="Eliminar" ID="BtnEliminarPlano" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" OnClick="EliminarPlanoDise_Click" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- Columna 2 -->
                                                <div class="col-lg-9 col-md-8 col-sm-12 col-xs-12">
                                                    <div class="p-3 m-2 border shadow-sm bg-light" style="height: 25rem;">
                                                        <div class="table-responsive mb-2 gap-2 bg-white" style="max-height: 22rem; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm"
                                                                ID="DataGridPlano" runat="server" AutoGenerateColumns="false" OnItemCommand="DatagridPlano_ItemCommand">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="SelecOt" CssClass="Tam" runat="server" CommandName="SelectPlano" CommandArgument='<%# Container.ItemIndex %>'
                                                                                Text="<i class='bi bi-pencil-square'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn HeaderText="Plano" DataField="Plano" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Cliente" DataField="Nombre_Cliente" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn HeaderText="Área" DataField="Area" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn HeaderText="OT" DataField="Id_OT" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn HeaderText="Ped" DataField="COnsecutivo_Pedido" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Dibujante" DataField="RealizadoPor" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="LdD" DataField="PlaFechalecturaDespiece" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Asesor" DataField="AsesorComercial" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>

                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="container border shadow-sm mt-2">
                                            <div class="d-flex flex-wrap">
                                                <!-- Buscar Plano -->
                                                <div class="p-4 col-lg-3 col-md-12 col-sm-12 col-xs-12 d-flex border">
                                                    <div class="p-1" style="flex-grow: 1;">
                                                        <h6>Buscar Plano</h6>
                                                        <div class="row mt-2">
                                                            <div class="col-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label14" class="col-form-label-sm">Plano</asp:Label>
                                                                    <asp:TextBox ID="TextBuscarPlano" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row mt-2">
                                                            <div class="col-12">
                                                                <div class="input-group input-group-sm gap-2">
                                                                    <asp:Label runat="server" ID="Label15" class="col-form-label-sm">Cliente</asp:Label>
                                                                    <asp:TextBox ID="TextBuscarClientePlano" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="p-1 mt-lg-5 mt-md-3 mt-sm-3 mt-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:LinkButton runat="server" title="Buscar Plano" ID="BtnBuscarPlano" OnClick="BtnBuscarPlano_Click" CssClass="linkButtonClicked2 shadow">
                                                                        <i class="bi bi-search AzulClaro grande p-2"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>

                                                <!-- Observacion Plano -->
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="p-1 border" style="height: 12rem;">
                                                        <h6 class="text-center">Observacion Plano</h6>
                                                        <div class="d-flex flex-wrap">
                                                            <div class="col-lg-2 col-md-4 col-sm-12 col-xs-12">
                                                                <div class="p-1" style="height: 8rem;">
                                                                    <div class="row mt-2">
                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                            <div class="form-group">
                                                                                <asp:Label runat="server" ID="Label16" class="col-form-label-sm">Opcion</asp:Label>
                                                                                <asp:DropDownList runat="server" ID="DropDownList2" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm">
                                                                                    <asp:ListItem Value=""></asp:ListItem>
                                                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row mt-2">
                                                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                            <div class="form-group">
                                                                                <asp:Label runat="server" ID="Label17" class="col-form-label-sm">Cantidad</asp:Label>
                                                                                <asp:TextBox ID="TextCanDes" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-lg-9 col-md-8 col-sm-12 col-xs-12 p-2">
                                                                <textarea id="TextAreaObsPla" runat="server" rows="5" class="form-control"></textarea>
                                                            </div>
                                                            <div class="col-lg-1 col-md-8 col-sm-12 col-xs-12 p-2">
                                                                <asp:Button runat="server" ID="BtnAsiPlaDis" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" Text="Asignar" OnClick="BtnAsiPlaDis_Click" />
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


                        <div class="modal fade" id="ConfirmarModificarPlanoDise" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Modificar Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="ConfirmarModificarPlanoDise2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" OnClick="SiModificarPlanoDise_Click" Text="Si"></asp:Button>
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="No" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="InsercionExitosaPlano" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Insercion Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="InsercionExitosaPlano2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="PlanoYaAsignado" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Asignar Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="PlanoYaAsignado2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="PlanoEliminadoExito" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Insercion Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>El plano se elimino exitosamente!</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="campoFaltantePlano" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Validar campos</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Falta llenar el campo: <span id="campoFaltantePlano2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="ActualizacionExitosaPlano" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Actualizar Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>El plano se Actualizo exitosamente</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="modal fade" id="AsignadoConExito" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Asignar Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>El plano ha sido asignado exitosamente al diseño.</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close" OnClick="AcutlizarDatagrid5_Click1"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="PlanoExistente" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>El plano que esta intentando insertar, ya existe</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="modal fade" id="ConfirEliminacionPlano" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="ConfirEliminacionPlano2"></span></p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" OnClick="ConfirmarEliminarPlano" Text="Si"></asp:Button>
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="No" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="PlanoOriginalNoEncontrado" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>No se encontró el plano original en la sesión.</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="PlanoExistenteModificar" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Plano Diseño</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>El plano que esta intentando Modificar, ya existe</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="ErrorPlano" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>No fue posible terminar esta acción, por favor valide la informacion insertada y vuelva a interntarlo</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Buscar-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container">
                            <div class="row">
                                <div class="border shadow rounded m-2" style="min-height: 50rem">
                                    <div class="rounded m-2">
                                        <div class="row mt-1">
                                            <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12 p-2">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" ID="Label2" class="col-form-label-sm">Fecha de Ingreso</asp:Label>
                                                    <asp:TextBox ID="TextFechDeIng" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" type="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12 p-2">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" ID="Label3" class="col-form-label-sm">Y</asp:Label>
                                                    <asp:TextBox ID="Texty" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm" type="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                                <div class="input-group input-group-sm gap-2 p-2">
                                                    <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                                                    <asp:Label runat="server" ID="Label7" class="col-form-label-sm">Ver Convenciones</asp:Label>

                                                </div>
                                            </div>

                                        </div>

                                        <div class="row mt-2">
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                                <div class="input-group input-group-sm gap-2 p-2">
                                                    <asp:Label runat="server" ID="Label4" class="col-form-label-sm">Diseño N.</asp:Label>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                <div class="input-group input-group-sm gap-2 p-2">
                                                    <asp:Label runat="server" ID="Label5" class="col-form-label-sm">Cliente</asp:Label>
                                                    <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-5 col-md-6 col-sm-6 col-xs-12">
                                                <div class="input-group input-group-sm gap-2 p-2">
                                                    <asp:Label runat="server" ID="Label6" class="col-form-label-sm">Proyecto.</asp:Label>
                                                    <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control form-control-sm linkButtonClicked2 shadow-sm"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-1 col-md-6 col-sm-6 col-xs-12">
                                                <div class="input-group input-group-sm gap-2 p-2">
                                                    <asp:Button ID="But" runat="server" Text="Buscar" OnClick="But_Click" CssClass="btn btn-outline-dark shadow-sm linkButtonClicked2 shadow" />

                                                </div>
                                            </div>



                                            <div id="modal1" class="modal fade" tabindex="-1" role="dialog">
                                                <div class="modal-dialog modal-dialog-centered" role="document">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                        </div>
                                                        <div class="modal-body" id="modalContent1">

                                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                                <div class="input-group bg-success-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                <label for="noproVen" class="form-label">Diseños No programados por Ventas</label>
                                                            </div>

                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group bg-white-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                <label for="NocumenEsp" class="form-label">No cumplidos y en espera de Dibujo y Despiece</label>
                                                            </div>

                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group bg-warning-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                <label for="Pendiente" class="form-label">Pendientes Por Dibujo y Despiece</label>
                                                            </div>


                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group bg-penAprCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                <label for="penAprCot" class="form-label">Pendientes por Aprobacion para Cotizar</label>
                                                            </div>
                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group bg-penCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                <label for="penCot" class="form-label">Diseños Pendientes por Cotizacion</label>
                                                            </div>
                                                            <div class="input-group input-group-sm mb-1 gap-2">
                                                                <div class="input-group bg-terminado-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                <label for="pausados" class="form-label">Diseños Terminados 100%</label>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="table-responsive table-responsive-sm mb-2 gap-2" style="max-height: 45rem; overflow-x: auto;">
                                        <asp:DataGrid Class="table table-bordered table-hover table-sm form-control-sm" ID="DataGrid4" runat="server" OnItemDataBound="DataGridBusDis_ItemDataBound" OnItemCommand="DataGridBusDise_ItemCommand"
                                            AutoGenerateColumns="false">
                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>

                                                        <asp:LinkButton ID="lnkCliee" runat="server" CommandName="Numero_Diseño"
                                                            CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" OnClick="lnkCliee_Click" OnClientClick="activarTab('BitacoraDesarrollo-content');" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkSelectRow" runat="server" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Container.ItemIndex + 1 %>' CssClass="text-white text-decoration-none" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Nombre_Diseño" HeaderText="Descripcion" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Fecha_Ingreso" HeaderText="F.Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Prog" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="UltimaActivacion" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Cliente" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ProgramadoVentas" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="PasarACotizar" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="TerminadoDibujo" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Pausado" HeaderText="Pausado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CotizaciónOK" HeaderText="CotizaciónOK" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>

                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                            SelectCommand="SELECT tblDiseño.*, tblDiseño.Fecha_Ingreso, tblDiseño.Nombre_Diseño FROM tblDiseño"></asp:SqlDataSource>





                                    </div>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Diseño-BitacoraFPV-001-content">
                <asp:UpdatePanel runat="server" ID="UpdateDiseñoBitacora" UpdateMode="Conditional">
                    <ContentTemplate>
                      

                            <nav class="navbar navbar-expand-sm navbar-light bg-light gap-2">
                                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                    <span class="navbar-toggler-icon"></span>
                                </button>

                                <%-- BOTONES--%>
                                <div class="collapse navbar-collapse" id="ejemplo2">
                                    <ul class="navbar-nav mx-auto contenedor-icono">
                                        <div class="contenedor-icono ">


                                            <asp:LinkButton runat="server" title="Nuevo Diseño" ID="NuevoDisBit" Enabled="false" OnClick="NuevoDisBit_Click">
                                                <i class="bi bi-file-earmark-fill"></i> 
                                            </asp:LinkButton>

                                            <!-- Modal -->
                                            <div class="modal fade" id="modall" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                                <div class="modal-dialog modal-dialog-centered">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-success">
                                                            <h5 class="modal-title d-flex align-items-center justify-content-center text-white" id="modallLabel">Dejar Infomación</h5>

                                                        </div>
                                                        <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                                            <p>Desea Limpiar los campos del diseño?</p>
                                                        </div>
                                                        <div class="modal-footer  d-flex align-items-center justify-content-center">
                                                            <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" OnClick="SiButton_Click" Text="Si"></asp:Button>
                                                            <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="No" OnClick="NoButton_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="modal" id="ShowCase" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                                <div class="modal-dialog modal-dialog-centered">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-dark">
                                                            <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Showcase</h5>

                                                        </div>
                                                        <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                                            <p>Desea modificar Showcase?</p>
                                                        </div>
                                                        <div class="modal-footer d-flex align-items-center justify-content-center">
                                                            <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" OnClick="BtnModificarSi_Click" Text="Si"></asp:Button>
                                                            <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" OnClick="BtnModificarNo_Click" Text="No"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="modal fade" id="llenarClienteDib" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                                <div class="modal-dialog modal-dialog-centered">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-dark">
                                                            <h5 class="modal-title d-flex align-items-center justify-content-center text-white">llenar cliente</h5>
                                                        </div>
                                                        <div class="modal-body form-control-sm">
                                                            <p>¿Desea modificar la información del cliente?</p>
                                                        </div>
                                                        <div class="modal-footer  d-flex align-items-center justify-content-center">
                                                            <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" Text="Si"></asp:Button>
                                                            <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" Text="No"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="modal" id="NumeroDiseñoNoValido" tabindex="-1">
                                                <div class="modal-dialog modal-dialog-centered">
                                                    <div class="modal-content">
                                                        <div class="modal-header bg-dark">
                                                            <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>

                                                        </div>
                                                        <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                                            <p>Parece que aún no tienes un diseño definido. Por favor, completa la información del diseño y luego podrás acceder a la documentación.</p>
                                                        </div>
                                                        <div class="modal-footer d-flex align-items-center justify-content-center">
                                                            <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close" Text="Aceptar"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <asp:LinkButton runat="server" title="Grabar Diseño" ID="Grabar" Enabled="false" OnClick="btnInsertar_Click">
                                               <i class="bi-floppy-fill"></i>
                                            </asp:LinkButton>

                                            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje"></asp:Label>

                                            <asp:LinkButton runat="server" title="Modificar Diseño" ID="Modificar" Enabled="false" OnClick="Modificar_Click">
                                               <i class="bi bi-wrench-adjustable"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" title="Documentacion Diseño" ID="DocBitacora" Enabled="false" OnClick="DocBitacora_Click">
                                            <%--<i class="bi bi-send-plus"></i>--%>
                                                <i class="bi bi-paperclip"></i> <%--Icono Documentacion--%>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" title="Regresar Diseño" ID="RegresarDiseño" Enabled="false" OnClick="RegresarDise_Click" OnClientClick="return handleClientClick();">
                                                <i class="bi bi-arrow-left-square-fill"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" title="Adicionar Elemento" ID="AdicionarElemento" Enabled="false">
                                              <i class="bi bi-table"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" title="Actualizar Diseños" ID="ActualizarDiseno" Enabled="false">
                                              <i class="bi bi-arrow-right-square"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" title="Pausar Diseño" ID="PausarDiseño" Enabled="false" OnClick="PausarDiseño_Click">
                                              <i class="bi bi-stop-circle-fill"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" title="Cancelar" ID="Cancelar" Enabled="false" OnClick="Cancelar_Click">
                                               <i class="bi bi-x-circle-fill"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" title="Eliminar Diseño" ID="EliminarDiseño" Enabled="false">
                                                    <i class="bi bi-trash-fill"></i>
                                            </asp:LinkButton>




                                        </div>
                                    </ul>
                                </div>
                            </nav>

                          <div class="container-fluid">
                        <div id="miDiv" runat="server" data-div="miDiv" style="display: block">
                            <%--  1/4--%>
                         
                              
                                    <div class="border shadow-sm bg-light p-1 m-2">
                                       
                                        <div class="row">
                                            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">

                                                <div class="row d-flex justify-content-between mt-2">
                                                    <div class="col-md-8 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" class="col-form-label-sm">Diseño#:</asp:Label>
                                                            <asp:Label ID="lblNumDise" runat="server" Style="font-size: 20px; color: black; font-weight: bold; margin-bottom: 10px; font-family: 'Times New Roman'">Número</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <asp:Button runat="server" ID="BtnBus" type="button" OnClientClick="mostrarTab(); return false;" class="btn-outline-dark btn m-2 shadow-sm btn-sm linkButtonClicked2" Text="..." />
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-12 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Button runat="server" ID="Button1" CssClass="btn-outline-dark btn btn-white" Text="Cliente" OnClick="CargarVSC_Click" OnClientClick="abrirOtraPestaña();" />
                                                                <asp:TextBox ID="TextCliente" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row d-flex justify-content-between mt-1">
                                                        <div class="col-md-7 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Label runat="server" ID="lblDir" class="col-form-label-sm">Dir</asp:Label>
                                                                <asp:TextBox ID="TextDir" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-5 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Label runat="server" ID="lblDescuento" class="col-form-label-sm">Descuento</asp:Label>
                                                                <asp:TextBox ID="TextDes" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-6 col-6">
                                                        <asp:Label runat="server" ID="lblIngDis" class="col-form-label-sm">Ingreso de Diseño</asp:Label>
                                                        <asp:TextBox ID="TextIngDis" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-6">
                                                        <asp:Label runat="server" ID="lblUltAct" class="col-form-label-sm">Ultima Activacion</asp:Label>
                                                        <asp:TextBox ID="TextUltAc" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-12">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label runat="server" ID="lblPro" class="col-form-label-sm">Proyecto</asp:Label>
                                                            <asp:TextBox ID="TextProyecto" runat="server" CssClass="form-control form-control-sm" MaxLength="49"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblPla" class="col-form-label-sm">Plano</asp:Label>
                                                            <asp:TextBox ID="TextPla" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-2 gap-2">
                                                            <asp:Label runat="server" ID="lblUrg" class="col-form-label-sm">Urgente</asp:Label>
                                                            <asp:CheckBox ID="ChecUrgent" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-2 gap-2">
                                                            <asp:Label runat="server" ID="lblCotizar" class="col-form-label-sm">Cotizar</asp:Label>

                                                            <asp:CheckBox ID="ChecCot" runat="server" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-5 col-6">
                                                        <asp:Label runat="server" ID="lblEnt" class="col-form-label-sm">Entrega</asp:Label>
                                                        <asp:TextBox ID="TextEntrega" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-5 col-6">
                                                        <asp:Label runat="server" ID="lblEntDib" class="col-form-label-sm">Fecha Ok Dibujo</asp:Label>
                                                        <asp:TextBox ID="TextFecOkDib" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2 col-6">
                                                        <asp:Label runat="server" ID="lblZon" class="col-form-label-sm">Zona</asp:Label>
                                                        <asp:DropDownList ID="TextZona" runat="server" CssClass="form-control-sm form-control">
                                                            <asp:ListItem Text="" Value="" />
                                                            <asp:ListItem Text="01" Value="01" />
                                                            <asp:ListItem Text="02" Value="02" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-8 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblCon" class="col-form-label-sm">Contacto</asp:Label>
                                                            <asp:TextBox ID="TextContacto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblTel" class="col-form-label-sm">Tel</asp:Label>
                                                            <asp:TextBox ID="TextTel" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label runat="server" ID="lblMaiTer" class="col-form-label-sm">Mail Term</asp:Label>
                                                            <asp:CheckBox ID="ChecMailTer" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label runat="server" ID="lblCotVia" class="col-form-label-sm">Cotiza Viá</asp:Label>
                                                            <asp:CheckBox ID="ChecCotVia" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label runat="server" ID="lblCotTte" class="col-form-label-sm">Cotiza Tte</asp:Label>
                                                            <asp:CheckBox ID="CheckBox4" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-6 col-12">
                                                        <asp:Label runat="server" ID="lblAse" class="col-form-label-sm">Asesor</asp:Label>
                                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control form-control-sm" Enabled="false" DataSourceID="SqlDataSource2" DataTextField="NombreCompleto" DataValueField="Cedula">
                                                        </asp:DropDownList>

                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                            SelectCommand="SELECT Cedula, Nombre + ' ' + Apellidos AS NombreCompleto FROM tblAsesorComercial WHERE Activo = '1' ORDER BY nombre ASC;"></asp:SqlDataSource>


                                                    </div>
                                                    <div class="col-md-6 col-12">
                                                        <asp:Label runat="server" ID="lblPre" class="col-form-label-sm">Pre</asp:Label>

                                                        <asp:DropDownList ID="TextPre" runat="server" CssClass="form-control-sm form-control">
                                                            <asp:ListItem Text="" Value="" />
                                                            <asp:ListItem Text="Completa detallada" Value="Completa detallada" />
                                                            <asp:ListItem Text="Completa por prototipo" Value="Completa por prototipo" />
                                                            <asp:ListItem Text="Zona detallada" Value="Zona detallada" />
                                                            <asp:ListItem Text="Zona por Prototipo" Value="Zona por Prototipo" />
                                                            <asp:ListItem Text="Piso detallado" Value="Piso detallado" />
                                                            <asp:ListItem Text="Piso por prototipo" Value="Piso por prototipo" />
                                                            <asp:ListItem Text="Piso por zona detallada" Value="Piso por zona detallada" />
                                                            <asp:ListItem Text="En observaciones de Ventas" Value="En observaciones de Ventas" />
                                                        </asp:DropDownList>






                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblCel" class="col-form-label-sm">Cel</asp:Label>
                                                            <asp:TextBox ID="TextCel" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-8 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblMai" class="col-form-label-sm">Mail</asp:Label>
                                                            <asp:TextBox ID="TextMail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row d-flex justify-content-between">
                                                        <div class="col-md-7 col-3">
                                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                                <asp:Label runat="server" ID="lblCiuPro" class="col-form-label-sm">Ciudad proyecto</asp:Label>
                                                                <asp:DropDownList ID="TextCiuPro" runat="server" DataSourceID="sqlDataSourceCiudades"
                                                                    DataTextField="CiudadDepartamento" DataValueField="id_Ciudad_Aut" />

                                                                <asp:SqlDataSource ID="sqlDataSourceCiudades" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                                    SelectCommand="SELECT tblCiudad.id_Ciudad_Aut, tblCiudad.NombreCiudad + ' - ' + tblDepartamentoPais.NombreDepartamento AS CiudadDepartamento
                                                                        FROM tblCiudad
                                                                        INNER JOIN tblCostoTransporte ON tblCiudad.id_Ciudad_Aut = tblCostoTransporte.tte_ID_Ciudad
                                                                        INNER JOIN tblDepartamentoPais ON tblCiudad.Id_Departamento = tblDepartamentoPais.Id_Departamento_Auto
                                                                        GROUP BY tblCiudad.id_Ciudad_Aut, tblCiudad.NombreCiudad + ' - ' + tblDepartamentoPais.NombreDepartamento
                                                                        ORDER BY tblCiudad.NombreCiudad + ' - ' + tblDepartamentoPais.NombreDepartamento;"></asp:SqlDataSource>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-5 col-4 mt-1">
                                                            <asp:Button runat="server" ID="BtnProgramar" CssClass="btn-outline-dark btn btn-sm btn-white fw-bold" Text="PROGRAMAR" OnClick="BtnProgramar_Click" Enabled="false" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                       
                                    </div>
                                </div>
                           
                            <%-- 2/4--%>
                           
                              
                                    <div class="border shadow-sm bg-light m-2">

                                        <div class="row">
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12 border">
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblConCab" runat="server" class="form-label" Style="font-size: 16px; font-weight: bold;">Conduccion de Cables</asp:Label>
                                                        <asp:CheckBox ID="ChecConDeCab" runat="server" AutoPostBack="True" OnCheckedChanged="ChecConDeCab_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm  gap-2">
                                                            <asp:Label ID="lblPis" runat="server" class="col-form-label-sm g-5">Piso</asp:Label>
                                                            <asp:CheckBox ID="ChecPiso" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm  gap-2">
                                                            <asp:Label ID="lblDiv" runat="server" class="col-form-label-sm g-5">División</asp:Label>
                                                            <asp:CheckBox ID="ChecDiv" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label ID="lblCie" runat="server" class="col-form-label-sm">Cielo</asp:Label>
                                                            <asp:CheckBox ID="ChecCie" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label ID="lblCan" runat="server" class="col-form-label-sm">Canaleta</asp:Label>
                                                            <asp:CheckBox ID="ChecCan" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 col-6">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblBteEle" runat="server" class="col-form-label-sm">Bte.Elec</asp:Label>
                                                        <asp:CheckBox ID="ChecBteEle" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 col-6">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblBteSw" runat="server" class="col-form-label-sm">Bte Sw</asp:Label>
                                                        <asp:CheckBox ID="ChecBteSw" runat="server" />
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12 border">
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblSujPt" runat="server" class="form-label" Style="font-size: 16px; font-weight: bold;">Sujeción PT</asp:Label>
                                                        <asp:CheckBox ID="ChecSujPt" runat="server" AutoPostBack="true" OnCheckedChanged="ChecSujPt_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 col-6">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblAlCie" runat="server" class="col-form-label-sm">Al Cielo</asp:Label>
                                                        <asp:CheckBox ID="ChecAlCie" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblPerRef" runat="server" class="col-form-label-sm">Perfil Refuerzo</asp:Label>
                                                        <asp:CheckBox ID="ChecPerRef" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblGuaEsc" runat="server" class="col-form-label-sm">Guarda Escobas</asp:Label>
                                                        <asp:CheckBox ID="ChecGuaEsc" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm p-1 gap-2">
                                                        <asp:Label ID="lblHTotCms" runat="server" class="col-form-label-sm">H.Total(Cms)</asp:Label>
                                                        <asp:TextBox ID="TexHTot" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 border">
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblEsyMat" runat="server" class="form-label col-12 text-dark text-uppercase" Style="font-size: 16px; font-weight: bold;">Especificaciones y Materiales</asp:Label>
                                                            <asp:CheckBox ID="CheckEsyMat" runat="server" AutoPostBack="true" OnCheckedChanged="CheckEsyMat_CheckedChanged" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblLin" runat="server" class="col-form-label-sm">Linea</asp:Label>
                                                            <asp:TextBox ID="TextLin" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblMos" runat="server" class="col-form-label-sm">Mostrador</asp:Label>
                                                            <asp:TextBox ID="TextMos" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblSup" runat="server" class="col-form-label-sm">Superficies</asp:Label>
                                                            <asp:TextBox ID="TextSup" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label ID="lblBal" runat="server" class="col-form-label-sm">Balance</asp:Label>
                                                            <asp:CheckBox ID="CheckBox16" runat="server" CssClass="form-check" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblSop" runat="server" class=" col-form-label-sm">Soporte</asp:Label>
                                                            <asp:TextBox ID="TextSop" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblGav" runat="server" class="col-form-label-sm">Gaveta</asp:Label>
                                                            <asp:TextBox ID="TextGav" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblPan" runat="server" class="col-form-label-sm">Paneles</asp:Label>
                                                            <asp:TextBox ID="TextPan" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblTPie" runat="server" class="col-form-label-sm">T.Piernas</asp:Label>
                                                            <asp:TextBox ID="TextTapPie" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblRep" runat="server" class="col-form-label-sm">Repisa</asp:Label>
                                                            <asp:TextBox ID="TextRep" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblTipVid" runat="server" class="col-form-label-sm">Tipo Vidrio</asp:Label>
                                                            <asp:TextBox ID="TextTipVid" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblPant" runat="server" class="col-form-label-sm">Pantallas</asp:Label>
                                                            <asp:TextBox ID="TextPant" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblArc" runat="server" class="col-form-label-sm">Arch</asp:Label>
                                                            <asp:TextBox ID="TextArch" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12 border">
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblMue" runat="server" class="form-label" Style="font-size: 16px; font-weight: bold;">Muebles</asp:Label>
                                                        <asp:CheckBox ID="ChecMue" runat="server" AutoPostBack="true" OnCheckedChanged="ChecMue_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lblCoc" runat="server" class="col-form-label-sm">Coco</asp:Label>
                                                        <asp:TextBox ID="TextCoc" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lblEntr" runat="server" class="col-form-label-sm">Entrepaño</asp:Label>
                                                        <asp:TextBox ID="TextEnt" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lblPuer" runat="server" class="col-form-label-sm">Puertas</asp:Label>
                                                        <asp:TextBox ID="TextPuer" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                               
                            
                            <%--  3/4--%>
                            
                               
                                    <div class="border p-1 shadow-sm bg-light m-2">
                                        <div class="row">
                                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                <h6>Observaciones Ventas</h6>
                                                <textarea id="TextObsVen" class="form-control form-control-sm" style="height: 100px" runat="server"></textarea>
                                            </div>
                                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                <h6>Observaciones de Dibujo y Despiece</h6>
                                                <textarea id="TextObsDibDes" class="form-control form-control-sm" style="height: 100px" runat="server" readonly="readonly"></textarea>
                                            </div>
                                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                <h6>Seguimiento de Pausas y Devoluciones</h6>
                                                <textarea id="TextSegPauDev" class="form-control form-control-sm" style="height: 100px" runat="server" readonly="readonly"></textarea>
                                            </div>
                                        </div>
                                    </div>
                               
                          
                            <%-- 4/4--%>
                           
                               
                                    <div class="border rounded p-1 shadow-sm bg-light m-2">
                                        <div class="row">
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                                <div class="border rounded p-1" style="height: 250px">
                                                    <h6>ShowCase</h6>
                                                    <div class="col-md-10 col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox18" runat="server" />
                                                            <asp:Label ID="lblPrePpt" runat="server" class="col-form-label-sm">Presentación PPT</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-10 col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox19" runat="server" />
                                                            <asp:Label ID="lblIma" runat="server" class="col-form-label-sm">Imágenes</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-10 col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox20" runat="server" />
                                                            <asp:Label ID="lblAcc" runat="server" class="col-form-label-sm">Accesorios</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-10 col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox21" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox21_CheckedChanged" />
                                                            <asp:Label ID="lblTieRea" runat="server" class="col-form-label-sm">Tiempo Real</asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-10 col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:TextBox ID="TextFec" runat="server" CssClass="form-control-sm form-control" type="date" OnTextChanged="ValidarFecha" AutoPostBack="true"></asp:TextBox>
                                                            <asp:TextBox ID="TextFech" runat="server" CssClass="form-control-sm form-control" type="datetime"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-10 col-12">
                                                        <asp:Label ID="lblUbi" runat="server" class="col-form-label-sm">Ubicación</asp:Label>
                                                        <asp:TextBox ID="TextUbi" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-10 col-md-6 col-sm-6 col-xs-12">
                                                <div class="container-fluid">
                                                    <div class="row justify-content-center">

                                                        <div class="border rounded p-1" style="height: 250px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-sm table-bordered table-hover form-control-sm" ID="Datagrid5" runat="server" DataSourceID="SqldatasourceTxt"
                                                                AutoGenerateColumns="false" OnItemCommand="Datagrid5_ItemCommand" OnItemDataBound="DataGrid5_ItemDataBound">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkSelectRow" runat="server" CommandName="id_PlanoDiseno"
                                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="id_PlanoDiseno" HeaderText="ID" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Plano" HeaderText="Plano" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Area" HeaderText="Area" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SubTotalZona" HeaderText="Valor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SubTotalZona" HeaderText="Sub Total" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Opcion" HeaderText="Opc" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Observacion" HeaderText="Observacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="RealizadoPor" HeaderText="Realizado Por" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Composicion" HeaderText="Composición" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="FechalecturaDespiece" HeaderText="Despiece" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                </Columns>   
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="SqldatasourceTxt" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                                SelectCommand="SELECT pd.[id_PlanoDiseno], pd.[Plano], p.[RealizadoPor], p.[Area], pd.[SubTotalZona], pd.[Cantidad], pd.[SubTotalZona], pd.[Opcion], pd.[Observacion], pd.[Composicion], pd.[FechalecturaDespiece]
                                                                   FROM [tblPlanoDiseño] pd
                                                                   INNER JOIN [tblPlano] p ON pd.[Plano] = p.[Plano]
                                                                   WHERE (pd.[Numero_Diseño] = @NumeroDiseño) order by opcion, Plano asc"
                                                                DataSourceMode="DataSet">
                                                                <SelectParameters>
                                                                    <asp:Parameter Name="NumeroDiseño" Type="Int32" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>



                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-sm" Style="background-color: #ff6a00"> 
                                                   <i class="bi bi-currency-dollar"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="BtnDespiece" CssClass="btn btn-sm" OnClick="BtnDespiece_Click"> 
                                                  <i class="bi bi-pencil-square"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="BtnPlano" CssClass="btn btn-sm" OnClick="BtnPlano_Click"> 
                                                <img src="https://i.ibb.co/BCtb1QS/icons8-archivo-dxf-autocad-windows-11-color-310.png" alt="Plano" style="width: 23px; height: 23px;" />
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="LinkButton4" CssClass="btn btn-sm"> 
                                                 <i class="bi bi-menu-app"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="BtnVisCotPreAct" CssClass="btn btn-sm" Style="background-color: #00ff21" OnClick="VisualizarCotPrecioActual_Click">  
                                                   <i class="bi bi-currency-dollar"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                          

                        

                        </div>

                        <div id="FechaSC" class="modal" tabindex="-1">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-dark">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">S_I_Ducon</h5>
                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p><span id="FechaSC2"></span></p>
                                    </div>
                                </div>
                            </div>
                        </div>

 

                     
                                 <!--Modal confirmar Devolver diseño  -->
                            <div id="ConfirmarRegresoDelDiseno" class="modal" tabindex="-1" style="display: none;">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header navbar-custom d-flex align-items-center justify-content-center shadow text-white">
                                            <h5 class="modal-title text-center">Devolver Diseño</h5>

                                        </div>
                                        <div class="modal-body border rounded">
                                            <div class="container-fluid">
                                                <h6><span id="ConfirmarRegresoDelDiseno2"></span></h6>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="btnDevolverSolicitud_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-primary" Style="width: 5rem;" OnClick="AbrirModalObservaciones_Click" />
                                                <asp:Button runat="server" ID="btnDevolver_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" OnClientClick="ControlBtnCliente(); return false;" />
                                            </div>

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
                                            <asp:LinkButton ID="btnCerrarDetener" data-bs-dismiss="modal" runat="server" aria-label="Close" Style="color: white !important; margin-right: 1.5rem; font-size: 1.8rem; text-decoration: none;" OnClick="btnCerrarDevolver_Click">
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
                                                                        <asp:Label ID="Label21" runat="server" CssClass="form-label-sm fw-bold" Text="T.Obs."></asp:Label>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-7">
                                                                    <div class="input-group input-group-sm gap-2">

                                                                        <asp:DropDownList ID="ddlTipoObservacion" runat="server" CssClass="form-control form-control-sm" DataTextField="TipoObservacion" DataValueField="Id_TipoObservacion" DataSourceID="TipoObservacion" OnSelectedIndexChanged="ddlTipoObservacion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                                                       <asp:SqlDataSource runat="server" ID="TipoObservacion" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT 
                                                                                        Id_TipoObservacion, Aplicacion,Descripcion, Aplicacion + ' - ' + Descripcion as TipoObservacion,
                                                                                        DestinatarioPorDefecto,Programable, AlDirectorComercial
                                                                                        FROM tblTipoObservacion 
                                                                                        WHERE Aplicacion Like '%DEVOLUCIÓN DISEÑO%' 
                                                                                        AND Activa = 1
                                                                                        ORDER BY Aplicacion ASC , Descripcion ASC"></asp:SqlDataSource>

                                                                    </div>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <div class="input-group input-group-sm gap-2">
                                                                        <asp:Label ID="Label22" runat="server" CssClass="col-form-label-sm" Text="F.Actividad"></asp:Label>
                                                                        <asp:TextBox ID="tbfechaActividad" runat="server" CssClass="form-control form-control-sm" type="date"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <h6>Observación</h6>
                                                                    <textarea id="txObservacion" runat="server" class="form-control form-control-sm" style="height: 25rem;"> </textarea>
                                                                </div>
                                                            </div>

                                                        </div>

                                                        <div class="col-md-6 p-2 mt-2 border">

                                                            <div class="" style="height: 35.5rem;">

                                                                <div class="border rounded p-1 special-border" style="max-height: 20rem; height: 22rem; overflow-x: auto;">
                                                                    <h6 class="datagrid-header text-center">Receptores de Correo</h6>
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
                                                                        SelectCommand="SELECT Cedula, Nombre + ' ' + Apellidos AS NombreCompleto, Cargo,Mail FROM tblEmpleado
                                                                                   WHERE Activo = 1 AND ReceptorObservaciones = 1 ORDER BY Cargo ASC, Nombre ASC"></asp:SqlDataSource>

                                                                </div>

                                                                <div class="container-fluid pt-2 ">

                                                                    <div class="row pt-2 ">
                                                                        <div class="col-6">
                                                                            <asp:Label ID="Label23" runat="server" Text="Receptores por defecto" CssClass="col-form-label-sm fw-bold"></asp:Label>
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

                                 
       <div class="modal fade" id="ModalRazonPausar" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-title shadow d-flex align-items-center justify-content-center text-white p-2" style="background:#0863a4;">
                <h5 class="modal-title d-flex align-items-center justify-content-center text-white">PAUSA DE DISEÑO</h5>
            </div>
            <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                <div class=" d-flex flex-wrap">
                    <div class="col-9">
                        <p>Razón de la pausa</p>
                    </div>
                    <div class="col-3">
                        <div class="input-group input-group-sm gap-2">
                            <asp:Button runat="server" ID="BtnAceptar" type="button" class="btn btn-sm linkButtonClicked2 shadow-sm btn-outline-dark" data-bs-dismiss="modal" Text=" Aceptar" aria-label="Close" OnClick="BtnAceptar_Click"></asp:Button>
                            <asp:Button runat="server" type="button" class="btn btn-sm linkButtonClicked2 shadow-sm btn-outline-dark" data-bs-dismiss="modal" Text="Cancelar" aria-label="Close"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer" style="display: block;">
                <div class="row">   
                         <textarea class="form-control form-control-sm" id="Razon" runat="server" cols="12" rows="3"></textarea>  
                </div>
            </div>
        </div>
    </div>
</div>

                        
          




                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnVisCotPreAct" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade show active" id="Programacion-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container-fluid m-2">
                            <div class="row justify-content-center">
                                <div class="border shadow rounded p-1 special-border col-11" style="height: auto; min-height: 880px;">

                                    <div class="row">
                                        <div class="col-lg-10 col-md-9 col-sm-9 col-xs-12">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border shadow-sm" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGrid1" runat="server"
                                                                AutoGenerateColumns="false" OnItemDataBound="DataGrid1_ItemDataBound" OnItemCommand="DataGrid1_ItemCommand" ShowHeaderWhenEmpty="true" PageSize="5"
                                                                AllowSorting="true">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>

                                                                    <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="BtnCargarOT" runat="server" CommandName="Id_OT"
                                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Nombre de la Obra" ItemStyle-CssClass="auto-width-column2"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Nombre_Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Entrega_Dibujo_Despiece" HeaderText="F.Ingreso" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToDateTime(Eval("Fecha_Entrega_Dibujo_Despiece")).AddDays(2).ToString("dd/MM/yyyy hh:mm:ss tt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbDibujante" runat="server" Text='<%# Eval("RealizadoPor") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:BoundColumn DataField="Fecha_Despacho_Produccion" HeaderText="F.Despacho" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>


                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-md-3 col-sm-3 col-xs-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-3 col-sm-3 col-xs-12">
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-6 col-md-12 col-sm-12 col-xs-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lbZona" runat="server" class="col-form-label-sm">Zona</asp:Label>
                                                        <asp:DropDownList ID="DropDownListOptions" runat="server" CssClass="form-control-sm form-control shadow-sm linkButtonClicked2" OnSelectedIndexChanged="DropDownListOptions_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="%" Value="%" />
                                                            <asp:ListItem Text="01" Value="01" />
                                                            <asp:ListItem Text="02" Value="02" />
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
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
                                                            <div class="modal-body" id="modalContent">

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
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox23" runat="server" OnCheckedChanged="CheckBox23_CheckedChanged" AutoPostBack="true" />
                                                            <asp:Label runat="server" class="col-form-label-sm">Resumen Dibujante</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div id="modal2" class="modal fade" tabindex="-1" role="dialog">
                                                <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                                                    <div class="modal-content">
                                                        <div class="modal-body">
                                                            <h6>Resumen Dibujante</h6>
                                                            <div class="row">
                                                                <div class="border rounded">
                                                                    <div class="table-responsive" style="max-height: 400px">
                                                                        <asp:DataGrid Class="table table-bordered table-hover table-sm" ID="DataGrid3" runat="server" AutoGenerateColumns="false">
                                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                            <Columns>
                                                                                <asp:BoundColumn HeaderText="Dibujante" DataField="RealizadoPor" ItemStyle-CssClass="auto-width-column" />
                                                                                <asp:BoundColumn HeaderText="Ped" DataField="CantidadOt" ItemStyle-CssClass="auto-width-column" />
                                                                                <asp:BoundColumn HeaderText="Ult.Pedido" ItemStyle-CssClass="auto-width-column" />
                                                                                <asp:BoundColumn HeaderText="Dis" DataField="CantidadRepeticiones" ItemStyle-CssClass="auto-width-column" />
                                                                                <asp:BoundColumn HeaderText="Ultimo Diseño" ItemStyle-CssClass="auto-width-column" />
                                                                                <asp:BoundColumn HeaderText="Total" DataField="Total" ItemStyle-CssClass="auto-width-column" />
                                                                            </Columns>
                                                                        </asp:DataGrid>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 mt-1">
                                                    <asp:Button ID="BtnTrabPed" runat="server" Text="Trabajar Pedido" CssClass="btn-outline-dark btn btn-white btn-sm btn full-width-btn" OnClick="BtnTrabPed_Click" />
                                                </div>
                                            </div>
                                            <div class="row mt-2">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Button ID="BtnDesPed" runat="server" Text="Desprogramar" CssClass="btn-outline-dark btn btn-white btn-sm btn full-width-btn" OnClick="BtnDesPed_Click" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-10 col-md-9 col-sm-9 col-xs-12">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border shadow-sm" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid2" runat="server" AutoGenerateColumns="false"
                                                                OnItemDataBound="DataGrid2_ItemDataBound" OnItemCommand="DataGridDise_ItemCommand">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>

                                                                            <asp:LinkButton ID="lnkClie" runat="server" CommandName="Numero_Diseño"
                                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkSelectRow" runat="server" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Container.ItemIndex + 1 %>' CssClass="text-white text-decoration-none text-dark" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Descripción" ItemStyle-CssClass="auto-width-column2" />
                                                                    <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Ult.Act" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbDibujante2" runat="server" Text='<%# Eval("RealizadoPor") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="PactodeEntrega" HeaderText="Pacto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ProgramadoVentas" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="PasarACotizar" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="TerminadoDibujo" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Pausado" HeaderText="Pausado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="id_CiudadProyecto" HeaderText="Ciudad" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Urgente" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridDiseño" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>"
                                                                SelectCommand="sp_ProBitacoraDise" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>

                                                                    <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" DefaultValue="ValorPorDefecto" />

                                                                </SelectParameters>
                                                            </asp:SqlDataSource>


                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-md-3 col-sm-3 col-xs-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 mt-1">
                                                    <asp:Label runat="server" class="col-form-label-sm" Enabled="true">Pacto de entrega</asp:Label>
                                                    <asp:TextBox ID="TextPacEnt" runat="server" CssClass="form-control-sm form-control full-width-btn linkButtonClicked2 shadow-sm" type="datetime-local"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row mt-2">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 mt-1">
                                                    <asp:Button ID="BtnTrabDis" runat="server" Text="Trabajar Diseño" class="btn-outline-dark btn btn-white btn-sm btn full-width-btn" OnClick="BtnTrabDis_Click" />
                                                </div>
                                            </div>
                                            <div class="row mt-2">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 mt-1">
                                                    <asp:Button ID="BtnDesDis" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn full-width-btn" OnClick="btndesdis_click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-10 col-md-9 col-sm-9 col-xs-12">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border shadow-sm" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGridDiseños" runat="server"
                                                                AutoGenerateColumns="false" OnItemDataBound="DataGrid3_ItemDataBound" OnItemCommand="DataGridSC_ItemCommand">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>

                                                                            <asp:LinkButton ID="lnkSelectRow" runat="server" CommandName="Numero_Diseño"
                                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" OnClick="CargarDiseñoSC_Click" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Descripcion-ShowCase" ItemStyle-CssClass="auto-width-column2"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Fecha" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Hora" HeaderText="Hora" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Dibujante" HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Ubicacion" HeaderText="Ubicación" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Imagenes" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Terminado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_tiemporeal" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Presentacionppt" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Accesorios" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Ubicacion" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridDiseñosPorFecha" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="sp_ProBitacoraShowCase" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>

                                                                    <asp:SessionParameter Name="NombreUsuario" SessionField="usuariologueado" Type="String" DefaultValue="ValorPorDefecto" />

                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-md-3 col-sm-3 col-xs-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Button ID="BtnTrabShoCas" runat="server" Text="Trabajar ShowCase" class="btn-outline-dark btn btn-white btn-sm btn full-width-btn" OnClick="TrabSC_Click" />
                                                </div>
                                            </div>
                                            <div class="row mt-2">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Button ID="BtnDesSC" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn full-width-btn" OnClick="BtnDesSC_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-10 col-md-9 col-sm-9 col-xs-12">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border shadow-sm" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGridRender" runat="server"
                                                                AutoGenerateColumns="false" OnItemDataBound="DataGrid4_ItemDataBound" OnItemCommand="DatagridRender_ItemCommand">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkSelectRow" runat="server" CommandName="Id_Render"
                                                                                CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Id_Render" HeaderText="ID" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Nombre-Render" ItemStyle-CssClass="auto-width-column2"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Activado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="TerminadoRender" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Pausado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ProgramadoVentas" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>

                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridRenderPorFechaYAsesor" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="sp_ProBitacoraRender" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="Zona" SessionField="ZonaLogeada" Type="String" DefaultValue="ValorPorDefecto" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-md-3 col-sm-3 col-xs-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Button ID="BtnTrabRen" runat="server" Text="Trabajar Render" class="btn-outline-dark btn btn-white btn-sm btn full-width-btn" OnClick="BtnTrabRender_Click" />
                                                </div>
                                            </div>
                                            <div class="row mt-2">
                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Button ID="BtnDesRen" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn full-width-btn" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                        </div>

                          <div class="modal fade" id="ConfimacionTrabajarDise" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                              <div class="modal-dialog modal-dialog-centered">
                                  <div class="modal-content">
                                      <div class="modal-header bg-dark">
                                          <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Trabajar Diseño</h5>

                                      </div>
                                      <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                          <p><span id="contenidoConfirmacionTrabDise"></span></p>
                                      </div>
                                      <div class="modal-footer  d-flex align-items-center justify-content-center">
                                          <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" OnClick="SiTrabajarDise" Text="Si"></asp:Button>
                                          <button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">No</button>
                                      </div>
                                  </div>
                              </div>
                          </div>


                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>


        <div class="modal fade" id="miModal" tabindex="-1" role="dialog" aria-labelledby="miModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="miModalLabel">Título del Modal</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>

                    </div>
                    <div class="modal-body">
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="miModalll" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Campo Faltante</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Falta llenar el campo: <span id="campoFaltante"></span></p>
                    </div>
                    <div class="modal-footer">
                        <!-- Puedes agregar botones u opciones aquí si es necesario -->
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="miModalExito" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Exito</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Los datos se guardaron correctamente</p>
                    </div>
                    <div class="modal-footer">
                        <!-- Puedes agregar botones u opciones aquí si es necesario -->
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="miModalExitoInsAct" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Programación del diseño</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>Las Fechas: Ingreso del diseño, Ultima Activación y Entrega, se ajustaran cuando programe el diseño</p>
                    </div>
                    <div class="modal-footer">
                        <!-- Puedes agregar botones u opciones aquí si es necesario -->
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="miModalError" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Error</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>No se guardaron los datos correctamente</p>
                    </div>
                    <div class="modal-footer">
                        <!-- Puedes agregar botones u opciones aquí si es necesario -->
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="ErrorModiciarDiseno" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Modificar Diseño</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>El Diseño ya fue aprobado para Dibujo y Despiece, este departamento lo debe habilitar para ser modificado</p>
                    </div>
                    <div class="modal-footer">
                        <!-- Puedes agregar botones u opciones aquí si es necesario -->
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="miModalErrorAdj" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Mensaje</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>No se pudo encontrar el archivo</p>
                    </div>
                    <div class="modal-footer">
                        <!-- Puedes agregar botones u opciones aquí si es necesario -->
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="miModalExitoDocumentacion" tabindex="-1" style="display: none;">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Exito</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>La documentacion se guardo exitosamente</p>
                    </div>
                    <div class="modal-footer">
                    </div>
                </div>
            </div>
        </div>

        <div class="modal" id="ProgramarDiseño" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Programar Diseño</h5>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>
                            <spam id="ProgramarDiseño2"></spam>
                        </p>
                    </div>
                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                        <asp:Button runat="server" Text="Si" OnClick="ProgramarVentas_Click" CssClass="btn btn-sm btn-outline-dark" />
                        <asp:Button runat="server" Text="No" data-bs-dismiss="modal" aria-label="Close" OnClick="NOProgramarDiseño_Click" CssClass="btn btn-sm btn-outline-dark" />
                    </div>
                </div>
            </div>
        </div>

         <div id="ConfirmarTerminarDiseño" class="modal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true"">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header navbar-custom d-flex align-items-center justify-content-center shadow text-white">
                                            <h5 class="modal-title text-center">Terminar Diseño</h5>

                                        </div>
                                        <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                            <div class="container-fluid">
                                                <h6><span id="ConfirmarTerminarDiseño2"></span></h6>

                                                   <div id="loadingAnimation2" class="loading-animation" style="display: none;">
                    <div class="progress">
                        <div id="progressBar2" class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                    <div id="progressMessage2" class="progress-message">Cargando...</div>
                </div>
                                            </div>

                                        </div>
                                        <div class="modal-footer">
                                            <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                                <asp:Button runat="server" ID="Btn" Text="SI" CssClass="btn btn-sm btn-outline-success text-dark fw-bold linkButtonClicked2" OnClick="TerminarSi_Click" OnClientClick="showLoadingAnimation2();"/>
                                                <asp:Button runat="server" ID="Button3" Text="NO" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-danger text-dark fw-bold linkButtonClicked2" OnClick="NOTerminarDiseño_Click" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

        <div class="modal" id="ProgramarDiseñoCotizacion" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title text-white text-center">Programar Diseño</h5>
                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>
                            <spam id="ProgramarDiseñoCotizacion2"></spam>
                        </p>
                    </div>
                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                        <asp:Button runat="server" Text="Si" OnClick="ProgramarDiseñoCotizacion_Click" CssClass="btn btn-sm btn-outline-dark" />
                        <asp:Button runat="server" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-dark" />
                    </div>
                </div>
            </div>
        </div>

 <div class="modal fade" id="RegresarDise" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header bg-dark">
                <h5 class="modal-title text-white text-center">Programar Diseño</h5>
            </div>
            <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                <p>Desea regresar el diseño para el departamento de Dibujo y despiece?</p>
            </div>
            <div class="modal-footer  d-flex align-items-center justify-content-center">
                <asp:Button runat="server" Text="Si" OnClick="UpdateRegresarDise_Click" CssClass="btn btn-sm btn-outline-dark" />
                <asp:Button runat="server" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-sm btn-outline-dark" />
            </div>
        </div>
    </div>
</div>

        <div class="modal fade" id="ValidarDiseñoPendiente" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Validacion Diseño Asignado</h5>

                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>El diseño ya tiene responsable asignado. </p>
                    </div>
                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                        <button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Aceptar</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="ValidarPactoDeEntrega" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-dark">
                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">Validacion Pacto de Entrega</h5>

                    </div>
                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                        <p>No puede trabajar el diseño por que el pacto de entrega debe ser superior a la fecha actual</p>
                    </div>
                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                        <button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" aria-label="Close">Aceptar</button>
                    </div>
                </div>
            </div>
        </div>

<div class="modal fade" id="CargarTXToXLS" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content shadow">
            <div class="modal-header bg-success">
                <h5 class="modal-title d-flex align-items-center justify-content-center text-white">CARGAR TXT O XLS</h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div class="text-center mb-3">
                    <div class="input-group input-group-sm gap-2 justify-content-center">
                        <asp:CheckBox ID="chkElemExit" CssClass="form-check" runat="server" ToolTip="Seleccione la casilla si desea cargar los elementos existentes." />
                        <asp:Label ID="Label19" runat="server" Text="Cargar Elementos Existentes" ToolTip="Seleccione la casilla si desea cargar los elementos existentes."></asp:Label>
                    </div>
                </div>
                <div class="d-flex align-items-center form-control-sm justify-content-center">
                    <p>Por favor selecciona el txt que deseas cargar</p>
                    <asp:LinkButton runat="server" title="Nuevo plano" ID="LinkButton3" OnClientClick="triggerFileUpload(); return false;">
                        <img src="https://i.ibb.co/BCtb1QS/icons8-archivo-dxf-autocad-windows-11-color-310.png" alt="Nuevo plano" style="width: 40px; height: 40px;" />
                    </asp:LinkButton>
                    <asp:FileUpload runat="server" ID="FileUpload2" Style="display: none;" OnChange="showFileName();" />
                    <asp:TextBox runat="server" ID="txtFileName" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                </div>
                <!-- Barra de progreso -->
                <div id="loadingAnimation" class="loading-animation" style="display: none;">
                    <div class="progress">
                        <div id="progressBar" class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                    <div id="progressMessage" class="progress-message">Cargando...</div>
                </div>
            </div>
            <div class="modal-footer d-flex align-items-center justify-content-center bg-light">
                <asp:Button ID="btnCargar" runat="server" type="button" class="btn btn-sm linkButtonClicked2 shadow-sm btn-outline-dark" OnClick="btnCargar_Click" Text="Cargar" OnClientClick="showLoadingAnimation(); return true;"></asp:Button>
            </div>
        </div>
    </div>
</div>




        <div class="modal fade" id="CargarElementosExistentes" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>Desea cargar los elementos Existentes?</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm linkButtonClicked2 shadow-sm btn-outline-dark" data-bs-dismiss="modal" Text="Si" aria-label="Close"></asp:Button>
                                         <asp:Button runat="server" type="button" class="btn btn-sm linkButtonClicked2 shadow-sm btn-outline-dark" data-bs-dismiss="modal" Text="No" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

           <div class="modal fade" id="DiseñoPausado" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger">
                                        <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>

                                    </div>
                                    <div class="modal-body d-flex align-items-center form-control-sm justify-content-center">
                                        <p>El diseño está pausado, no se puede terminar.</p>
                                    </div>
                                    <div class="modal-footer  d-flex align-items-center justify-content-center">
                                        <asp:Button runat="server" type="button" class="btn btn-sm btn-outline-dark" data-bs-dismiss="modal" Text="Aceptar" aria-label="Close"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>

             <div class="modal fade" id="AdjuntarOtroArchivo" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header navbar-custom d-flex align-items-center justify-content-center">
                <h5 class="modal-title d-flex align-items-center justify-content-center text-white">SID_DUCON</h5>
            </div>
            <div class="modal-body row align-items-center form-control-sm justify-content-center">
                <div class="row">
                <p>Desea adjuntar algún archivo o plano al mail del diseño terminado?</p>
                    </div>
                  <div class="row">
                    <p class="text-muted">Nota: Si necesita adjuntar varios archivos, estos deben ser seleccionados desde la misma carpeta.</p>
                </div>
            </div>
            <div class="modal-footer d-flex align-items-center justify-content-center">
                     <div class="row">
                         <div class="col-3">
             <asp:LinkButton runat="server" title="Nuevo plano" ID="LinkButton5" OnClientClick="triggerFileUpload2(); return false;">
                        <i class="bi bi-cloud-arrow-up-fill ColorAzulActivo masGrande"></i>
                    </asp:LinkButton>
                          </div>
                          <div class="col-9 mt-3">
                   <asp:FileUpload runat="server" ID="FileUpload1" AllowMultiple="true" Style="display: none;" OnChange="showFileName2();" />
                    <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control linkButtonClicked2" ReadOnly="True"></asp:TextBox>
                    </div>
                         </div>
                <div class="row container">
                    <div id="loadingAnimation3" class="loading-animation" style="display: none;">
                    <div class="progress">
                        <div id="progressBar3" class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>
                    <div id="progressMessage3" class="progress-message">Cargando...</div>
                </div>
                    </div>
                </div>
              <div class="modal-footer d-flex align-items-center justify-content-center">
                   <asp:Button runat="server" type="button" ID="BtnAdjuntarOtro" class="btn btn-sm btn-outline-success text-dark fw-bold linkButtonClicked2" Text="ADJUNTAR" OnClick="AdjuntarOtroArchivo_Click" OnClientClick="showLoadingAnimation3();"></asp:Button>
              <asp:Button runat="server" type="button" ID="BtnNoAdjuntarOtro" class="btn btn-sm btn-outline-danger text-dark fw-bold linkButtonClicked2" Text="NO" OnClick="EnviarCorreoTerminado_Click" OnClientClick="showLoadingAnimation3();"></asp:Button>

            </div>
        </div>
    </div>
</div>

         
    </form>

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



              }
          });
      </script>

    <script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        // Seleccionar el tab específico por su ID
        var planoContent = document.getElementById('<%= UpdatePanel2.ClientID %>');

        // Agregar un listener para capturar la tecla "Enter"
        planoContent.addEventListener("keydown", function(event) {
            // Verificar si la tecla presionada es "Enter"
            if (event.key === "Enter") {

                if (focusedElement.tagName !== 'TEXTAREA') {
                    // Prevenir la acción predeterminada del evento
                    event.preventDefault();

                }
                event.preventDefault(); // Evitar el comportamiento predeterminado de "Enter"
                // Disparar el click en el LinkButton
                document.getElementById('<%= But.ClientID %>').click();
            }
        });
    });
    </script>

     <script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        // Seleccionar el tab específico por su ID
        var planoContent = document.getElementById('<%= UpdatePanel3.ClientID %>');

        // Agregar un listener para capturar la tecla "Enter"
        planoContent.addEventListener("keydown", function(event) {
            // Verificar si la tecla presionada es "Enter"
            if (event.key === "Enter") {

                if (focusedElement.tagName !== 'TEXTAREA') {
                    // Prevenir la acción predeterminada del evento
                    event.preventDefault();

                }
                event.preventDefault(); // Evitar el comportamiento predeterminado de "Enter"
                // Disparar el click en el LinkButton
                document.getElementById('<%= BtnBuscarPlano.ClientID %>').click();
            }
        });
    });
     </script>

    <script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        // Seleccionar el tab específico por su ID
        var planoContent = document.getElementById('<%= UpdateDiseñoBitacora.ClientID %>');

        // Agregar un listener para capturar la tecla "Enter"
        planoContent.addEventListener("keydown", function(event) {
            // Verificar si la tecla presionada es "Enter"
            if (event.key === "Enter") {

                if (focusedElement.tagName !== 'TEXTAREA') {
                    // Prevenir la acción predeterminada del evento
                    event.preventDefault();

                }
                event.preventDefault(); // Evitar cualquier acción asociada con "Enter"
            }
        });
    });
    </script>



    <script>
        function activarTab(tabId) {
            // Oculta todas las pestañas
            $('#myTabs a.Programacion-content').removeClass('active');
            $('.tab-pane').removeClass('active show');

            // Activa la pestaña deseada
            $('#myTabs a[href="#Diseño-BitacoraFPV-001-content"]').tab('show');
        }

    </script>

    <script type="text/javascript">
        function mostrarTab() {

            var tabElementt = document.getElementById('Buscar-tab');

            // Verifica si el tab ya está visible
            if (tabElementt.style.display === 'block') {
                // Oculta el tab
                tabElementt.style.display = 'none';
            } else {
                // Muestra el tab
                tabElementt.style.display = 'block';

                // Activa el tab
                $('#Buscar-tab').tab('show');
            }


        }

    </script>

    <script type="text/javascript">
        function mostrarTabPlano() {

            var tabElementt = document.getElementById('Buscar-tabPlano');

            // Verifica si el tab ya está visible
            if (tabElementt.style.display === 'block') {
                // Oculta el tab
                tabElementt.style.display = 'none';
            } else {
                // Muestra el tab
                tabElementt.style.display = 'block';

                // Activa el tab
                $('#Buscar-tabPlano').tab('show');
            }


        }

    </script>

    <script type="text/javascript">
        function mostrarTabDespiece() {

            var tabElementt = document.getElementById('Despiece-tab');

            // Verifica si el tab ya está visible
            if (tabElementt.style.display === 'block') {
                // Oculta el tab
                tabElementt.style.display = 'none';
            } else {
                // Muestra el tab
                tabElementt.style.display = 'block';

                // Activa el tab
                $('#Despiece-tab').tab('show');
            }


        }

    </script>

     <script type="text/javascript">
         function cerrarTab() {

             var tabElementt = document.getElementById('Despiece-tab');
             
                // Oculta el tab
                 tabElementt.style.display = 'none';

         }

     </script>

    <script>
        function abrirOtraPestaña() {
            // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
            window.open('Clientes.aspx', '_blank');
        }
    </script>

    <script>
    function cambiarAnchoColumnas() {
        var primeraColumna = document.getElementById('primeraColumna');
        var segundaColumna = document.getElementById('segundaColumna');

        if (document.getElementById('<%= CheckBox2.ClientID %>').checked) {
            primeraColumna.classList.remove('col-lg-7');
            primeraColumna.classList.add('col-lg-6');

            segundaColumna.classList.remove('col-lg-5');
            segundaColumna.classList.add('col-lg-6');
        } else {
            primeraColumna.classList.remove('col-lg-6');
            primeraColumna.classList.add('col-lg-7');

            segundaColumna.classList.remove('col-lg-6');
            segundaColumna.classList.add('col-lg-5');
        }
    }
</script>


   <script>
       function handleClientClick() {
           // No hacer nada y permitir el postback
           return true;
       }

       function mostralMoldalDevolver() {
           var lbNumeroSolicitud = document.getElementById('<%= lblNumDise.ClientID %>').innerText;
           document.getElementById('Span_Id_Sol1').innerText = lbNumeroSolicitud;
           $('#ConfirmarRegresoDelDiseno').modal('show');
       }

       function CerrarModalDevolver() {
           $('#ConfirmarRegresoDelDiseno').modal('hide');
       }
   </script>

   
     <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.10.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
