<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerarCodigoInventario.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.DiseñoYDesarrollo.GenerarCodigoInventario" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/OrdenTrabajo.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js"></script>


    <title></title>
</head>
<body>

    <script>

        function triggerPostBack(target, value) {
            // Guardar el elemento enfocado actualmente
            var focusedElement = document.activeElement;

            // Realizar el postback
            __doPostBack(target, value);


        }

        function restoreFocus() {
            // Reestablecer el foco después del postback
            setTimeout(function () {
                //txtCodigoGrupo
                var input = document.getElementById('<%= tb_Buscar_Insumo_Filtro_Changed.ClientID %>');
                     console.log(input);
                if (input) {
                    input.focus();
                    input.selectionStart = input.value.length; // Coloca el cursor al final del texto
                    input.selectionEnd = input.value.length;
                }
                 
             }, 0); // 0 para hacerlo inmediatamente después del postback

        }

        function mostrarMensajeEspera() {
            document.getElementById('mensajeEspera').style.display = 'block';
        }

    </script>

    <style>
        .form-control-codigos input[type="checkbox"] {
            margin-right: 8px; /* Ajusta el espacio a tu preferencia */
        }

        .checkbox-container {
            min-height: 150px; /* Ajusta el valor según el espacio deseado */
            border: 1px solid #ccc; /* Opcional: para visualizar el área */
            padding: 10px; /* Espaciado interno */
        }

    </style>

    <div class="modal fade" id="Generar_Codigos_Inventario_Modal_Error" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="alertModalLabel"
 aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header alert-danger">
                <h5 class="modal-title" id="alertModalLabel">Error</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <asp:Label ID="lblModalErrorMessage" runat="server" CssClass="text-danger"></asp:Label>
            </div>
        </div>
    </div>
</div>


    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <!-- se utiliza para administrar los scripts y servicios que necesitan los controles AJAX en la página.-->

        <!--- aqui define un contenedor para el contenido de la pestaña -->
        <div class="tab-content">
            <!--esta es una pequeña especifica, que por defecto se  muestra activa por defecto--->
            <div class="tab-pane fade show active" id="Tap_Grupo">
                <div class="container border border-solid  p-3 shadow mt-4">
                    <!-- Primera fila -->
                    <div class="container border border-solid p-1 shadow mt-1 ">

                        <!-- Primera fila -->
                        <div class="row pt-2 mt-2 container">

                            <div class="col-3 col-md-auto col-sm-6">
                                <asp:Label class="form-label" Text="Seleccione un grupo: " runat="server" ID="Selec_Grup"></asp:Label>
                            </div>

                            <%--Select grupo insumo--%>
                            <div class="col-lg-auto col-md-8 col-sm-6">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="DropDownGrupoChange">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <%--Label grupo--%>
                            <div class="col-xl-2 col-lg-3 col-md-4 col-sm-6">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label ID="CodigogrupoLabel" runat="server" CssClass="form-label "  Text="Código de grupo: "></asp:Label>
                                            
                                    <asp:Label ID="lbl_Codigo_Grupo" runat="server" CssClass="form-label " Text=""></asp:Label>
                                </div>
                            </div>

                            <div class="col-lg-4 col-sm-6">
                                <div class="input-group input-group-sm mb-2 gap-2">
                                    <asp:Label CssClass="form-label" Text="Grupo:" runat="server" ID="Grupo_a"></asp:Label>
                                            
                                    <asp:Label ID="Grupo" runat="server" CssClass="form-label " Text=""></asp:Label>
                                        
                                </div>
                            </div>

                        </div>
                           
                        <!-- Segunda fila -->
                        <div class="row col-12 pt-2 mt-3 container">
                                    
                            <div class="col-auto" >
                                <label class="form-label form-control-" id="">Nombre nuevo insumo:</label>
                            </div>

                            <%--Input nombre de insumo--%>
                            <div class="col-lg-4 col-md-2 ">
                                <asp:TextBox ID="tb_Nuevo_Nombre_Insumo" runat="server" class=" form-control form-control-sm mb-2 " TextMode="SingleLine" 
                                    oninput="this.value = this.value.toUpperCase();">
                                </asp:TextBox>
                            </div>


                            <div class="col-auto">
                                    <label class="form-label" id="">Nombre insumo:</label>
                            </div>

                            <%--Input Buscar--%>
                            <div class="col-lg-3 col-md-2 ">
                                <asp:TextBox ID="tb_Buscar_Insumo_Filtro_Changed" runat="server" type="text" class="form-control form-control-sm mb-2 " placeholder="Buscar..."
                                    onkeyup="triggerPostBack('tb_Buscar_Insumo_Filtro_Changed', this.value)">
                                </asp:TextBox>

                            </div>

                        </div>

                    </div>

                    <!-- Tercera fila -->
                    <div class="container border border-solid p-2 shadow mt-3">
                        <div class="row pt-1 mt-3 container">

                            <!-- Sección de los codigos y el Botón -->
                            <div class="col-5">
                                <div class="form-group">
                                    <%--<div class="row">--%>
                                        <div class="col-12 ">

                                            <div class="row input-group input-group-sm">
                                                <label class="" id="">Unidad de medida</label>
                                                <%--Select unidad de medida--%>
                                                <div class="col-lg-5 col-md-12 mb-sm-2">
                                                    <asp:DropDownList ID="DropDown_Unidad_Medida" runat="server" CssClass="form-select form-select-sm form-control-sm"></asp:DropDownList>
                                                </div>

                                                <div class="col-lg-7 col-md-12">
                                                    <asp:Button CssClass="btn btn-sm btn-outline-secondary w-100"  ID="Button2" runat="server" 
                                                        Text="Consultar nuevos codigos"
                                                         OnClick="ConsultarCodigosInusumos" OnClientClick="mostrarMensajeEspera();"/>
                                                </div>
                                            </div>


                                            <%--<asp:Label CssClass="" Text="Seleccione los insumos a crear" runat="server" ID="Label4"></asp:Label>--%>

                                            <label class="form-label my-2" id="">Seleccione los insumos a crear</label>

                                            <%--Codigos generados--%>
                                            <div class="checkbox-container">
                                                <asp:CheckBoxList ID="listCodigosGenerados" runat="server" CssClass="form-control-codigos" Rows="10"></asp:CheckBoxList>
                                            </div>

                                            <%--Boton crear insumo--%>
                                            <div class="text-center mt-3">
                                                <!-- Aquí añadimos la clase text-center para centrar el botón -->
                                                <asp:Button CssClass="btn btn-sm btn-outline-secondary btn-light w-100" ID="Button1" runat="server" Text="Crear insumos seleccionados" OnClick="BtnCrearNuevoInsumos"/>
                                            </div>

                                            <div style="text-align:center; font-weight:700; font-size:large; margin-top:10px;">
                                                <%--Label status--%>
                                                <asp:Label ID="lblStatus" Text="" runat="server"/>
                                            </div>

                                            <%--Mensaje de espera--%>
                                            <div id="mensajeEspera" class="col-12" style="display:none; text-align:center; font-weight:bold; font-size:small; margin-top:10px;">
                                                <div class="spinner-border text-dark me-2" role="status">
                                                    <span class="visually-hidden">Loading...</span>
                                                </div>

                                                <p>Consultando código(s), por favor espere...</p>
                                            </div>

                                        </div>
                                    <%--</div>--%>

                                </div>
                            </div>


                            <!-- Sección del  datagrid -->
                            <div class="col-7">
                                <div class="row justify-content-center">
                                    <div class="border rounded px-2 pb-2 overflow-auto" style="max-height: 20rem; height: 20rem">
                                        <h6 class="datagrid-header text-center mt-2">Modulo</h6>
                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="gv_Datos_Insumos_Por_Grupo" 
                                            runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                            <HeaderStyle  Font-Bold="true" CssClass="datagrid-header " />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="...">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="color" 
                                                            CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="itecodigo" HeaderText="Codigo" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="itedesclarg" HeaderText="Descripcion" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="iteump" HeaderText="Un. De Medida" ItemStyle-CssClass="auto-width-column" />
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>

                </div>
            </div>
         </div>
    </form>

    


</body>
</html>

