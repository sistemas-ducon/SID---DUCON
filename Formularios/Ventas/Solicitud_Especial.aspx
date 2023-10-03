<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solicitud_Especial.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Solicitud_Especial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Solicitud Especial </title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/SolicitudesEspeciales.css" />

     <script>
         function confirmProgramarSolicitud(event) {

             var IdSolicitud = document.getElementById("lbNumeroSolicitud").innerHTML;
             var mensaje = "Una vez programada la solicitud, no podrá realizar modificaciones. Esta seguro de programar la solicitud:" + IdSolicitud;

             var result = confirm(mensaje);
             if (result) {

                 $(event.target).removeAttr('onclick');
                 $(event.target).click();
             }
             return false;
         }


     </script>

    <script>
        function confirmarQuitarDetalle(event) {

            var IdSolicitud = document.getElementById("lbNumeroSolicitud").innerHTML;
            var IdDetalle = document.getElementById("lbIdDetalle").innerHTML;

            var mensaje = "Estás seguro de eliminar el detalle " + IdDetalle + " de la solicitud " + IdSolicitud ;
            var result = confirm(mensaje);
            if (!result) {
                event.preventDefault(); // Cancelar el postback
            }
            return result; // Devolver el resultado de la confirmación
        }

    </script>

  <script>
      function precio(event) {

          var mensaje = "Estás seguro de eliminar el detalle " ;
          var result = confirm(mensaje);
          if (!result) {
              event.preventDefault(); // Cancelar el postback
          }
          return result; // Devolver el resultado de la confirmación
      }

  </script>



</head>
<body>

    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="miPestañas">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="BitacoraDesarrollo-tab" data-bs-toggle="tab" href="#BitacoraDesarrollo-content">Desarrollo Bitacora- PQ-006</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Programacion-tab" data-bs-toggle="tab" href="#Programacion-content">Programacion</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link text-dark " id="BuscarDesarrollo-tab" data-bs-toggle="tab" href="#BuscarDesarrollo-content">Buscar Desarrollos</a>
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

                            <asp:CheckBox ID="chkEstadoGuardarSolicitud" runat="server" CssClass="hidden-textBox" />

                            <a class="icong disabled" href="#" title="Nueva Solicitud" id="NuevaSolicitud" onclick="NuevaSolicitud()">
                                <i class="bi bi-file-earmark"></i>
                            </a>

                            <asp:LinkButton class="icong disabled" title="Pausar Solicitud" ID="PausarSolicitud" runat="server">
                                <i class="bi bi-pause-circle"></i> 
                            </asp:LinkButton>

                            <asp:LinkButton class="icong disabled" runat="server" title="Guardar Solicitud" ID="GrabarSolicitud" OnClick="GuardarModificarSolicitud" OnClientClick="return validarFormularioSolicitud();">
                                        <i class="bi bi-save2"></i>
                            </asp:LinkButton>

                            <a class="icong disabled" href="#" title="Modificar Solicitud" id="ModificarSolicitud" onclick="ModificarSolicitud()">
                                <i class="bi bi-wrench"></i>
                            </a>

                            <a class="icong disabled " href="#" title="Observaciones" id="Observaciones">
                                <i class="bi bi-eye"></i>
                            </a>

                            <asp:LinkButton class="icong disabled" title="Devolver Solicitud a Ventas" ID="DevolverSolicitud" runat="server">
                                  <i class="bi bi-skip-backward-circle"></i>
                            </asp:LinkButton>

                            <asp:LinkButton class="icong disabled" title="Detener Pedido PE" ID="DetenerPE" runat="server">
                                 <i class="bi bi-stop-circle"></i>
                            </asp:LinkButton>


                            <asp:LinkButton class="icong disabled" title="Cancelar" ID="CancelarSolicitud" OnClientClick="CancelarSolicitud();" OnClick="LimpiarCampos" runat="server">
                                  <i class="bi bi-x-lg"></i>
                            </asp:LinkButton>

                            <asp:LinkButton class="icong disabled" title="Eliminar Solicitud " ID="EliminarSolicitud" runat="server">
                                  <i class="bi bi-trash"></i>
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
                        <div class="container-fluid border ">

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
                                            <asp:ListItem Value="">-- Seleccione --</asp:ListItem>
                                            <asp:ListItem Value="COMPRAS">COMPRAS</asp:ListItem>
                                            <asp:ListItem Value="DESARROLLO DE PRODUCTO">DESARROLLO DE PRODUCTO</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-sm-2">
                                    <div class=" input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbTipo" class="col-form-label-sm" Text="Tipo" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control form-control-sm" ID="ddlTipo" runat="server">
                                            <asp:ListItem Value="">-- Seleccione --</asp:ListItem>
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
                                <div class="col-3">

                                    <div class="input-group input-group-sm  mb-2 gap-3 justify-content-center">
                                        <asp:Label ID="lbProyecto" class="col-form-label-sm" Text="Proyecto" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbProyecto" type="text" class="form-control form-control-sm " runat="server" disabled="disabled" required=""></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbCiudad" class="col-form-label-sm" Text="Ciudad" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" DataTextField="NombreCiudad" DataValueField="NombreCiudad" DataSourceID="CargarCiudad" OnDataBound="ddlCiudad_DataBound"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="CargarCiudad" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT 
                                            CONCAT(tblDepartamentoPais.CodigoDepartamento ,
                                            tblCiudad.CodigoCiudad)   AS CodCompleto,
                                            tblCiudad.NombreCiudad+' - '+tblDepartamentoPais.NombreDepartamento As NombreCiudad
                                            FROM tblDepartamentoPais 
                                            INNER JOIN tblCiudad
                                            ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento 
                                            ORDER BY CONCAT(tblCiudad.NombreCiudad , '-' , tblDepartamentoPais.NombreDepartamento)"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class=" input-group-sm  mb-2 gap-4 justify-content-center">
                                        <asp:CheckBox ID="chxViaticos" runat="server" Enabled="false" />
                                        <asp:Label ID="lbViaticoYTransporte" class=" col-form-label-sm" Text="Cotizar Viaticos y Transporte" runat="server"></asp:Label>

                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class=" input-group input-group-sm  mb-2 gap-4 justify-content-center">
                                        <asp:Label ID="lbCotizacionEsp" class="col-form-label-sm" Text="Cotización ESP" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbCotizacionEsp" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row pb-1 mb-1">
                                <div class="col-3">
                                    <div class=" input-group input-group-sm  mb-2 gap-4">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="btnCliente" runat="server" Text="Cliente" OnClick="GuardarDatosSesion" OnClientClick="abrirOtraPestana();" />
                                        <asp:TextBox ID="tbCliente" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                        <asp:TextBox ID="tbClienteServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbContacto" class=" col-form-label-sm  " Text="Contacto" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbContacto" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                        <asp:TextBox ID="tbContactoServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbTelefono" class="col-form-label-sm " Text="Telefono" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbTelefono" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                        <asp:TextBox ID="tbTelefonoServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbCelular" class="col-form-label-sm" Text="Celular" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbCelular" type="text" class="form-control form-control-sm" runat="server" disabled="disabled"></asp:TextBox>
                                        <asp:TextBox ID="tbCelularServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="col-3">
                                    <div class=" input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbDesarrollado" class="col-form-label-sm" Text="Desarrolado Por" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbDesarrollaPor" type="text" class="form-control form-control-sm" runat="server" Text="" disabled="disabled"></asp:TextBox>

                                    </div>
                                </div>

                            </div>

                            <div class="row pt-1 mt-1 pb-1 mb-1">
                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-3 justify-content-center">
                                        <asp:Label ID="lbMail" class="col-form-label-sm" Text="Mail" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbMail" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                        <asp:TextBox ID="tbMailServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="col-3">
                                    <div class=" input-group input-group-sm  mb-2 gap-2 justify-content-center">
                                        <asp:Label ID="lbDireccion" class="col-form-label-sm" Text="Dirección" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbDireccion" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>
                                        <asp:TextBox ID="tbDireccionServidor" type="text" class="form-control form-control-sm " runat="server" CssClass="hidden-textBox"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbAsesor" class="col-form-label-sm" Text="Asesor" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control form-control-sm" ID="ddlAsesor" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:CheckBox ID="chxDesComplejo" runat="server" Enabled="false" />
                                        <asp:Label ID="lb" class=" col-form-label-sm" Text="Desarrollo Complejo" runat="server"></asp:Label>
                                        <asp:Button ID="ConfirmarComplejo" CssClass="btn btn-outline-secondary" runat="server" Text="Confirmar Complejo" />
                                    </div>
                                </div>



                            </div>

                        </div>

                        <div class="container-fluid Principal-centro pt-1 mt-1 gap-1 ">
                            <div class="container-fluid  izq">
                                <h6 class="p-0 m-0">Producto</h6>

                                <div class="row">
                                    <div class=" col-6-sm">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <textarea class="form-control form-control-sm" id="txDescProduc" runat="server" cols="25" rows="3" disabled="disabled"></textarea>
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

                                            <asp:TextBox ID="tbCantidad" type="text" class="form-control form-control-sm " runat="server" disabled="disabled"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="container-fluid  centro">
                                <h6 class="p-1 m-0">Especificaciones Generales</h6>
                                <div class="row">
                                    <div class=" col-6-sm">
                                        <div class=" input-group-sm  mb-2 gap-2">
                                            <textarea class="form-control form-control-sm" id="txEspGen" runat="server" cols="25" rows="10" disabled="disabled"></textarea>
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
                                            <asp:Label ID="lbUrgente" class="form-label" Text="Urgente" runat="server"></asp:Label>
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
                                            <asp:TextBox ID="tbPrecioSugerido" type="text" class="form-control form-control-lg PrecioSugerido " runat="server" disabled="disabled"></asp:TextBox>
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
                                            <div class="table-responsive  mb-2 gap-2" style="max-height: 10rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-center">Detalle Desarrollo</h6>
                                                <asp:DataGrid CssClass="table table-bordered custom-grid table-hover custom-data-grid" ID="DataGridDetalleSolicitud" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="DetalleSolicitud" OnItemCommand="DataGridDetalleSolicitud_LinkButton" OnItemDataBound="DataGridDetalleSolicitud_ItemDataBound">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerDetalleSolicitud" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
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



                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource ID="DetalleSolicitud" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand=" Select *
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


                            <div class="row">

                                <div class=" col-4">
                                    <h6 class="p-1 m-0">Información Detalle Solicitud Origen </h6>
                                    <div class=" input-group-sm  mb-2 gap-2">
                                        <textarea class="form-control form-control-sm" id="txInformacionDetalle" runat="server" cols="25" rows="5" disabled="disabled"></textarea>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <h6 class="p-1 m-0">Seguimiento Pausas </h6>
                                    <div class=" input-group-sm  mb-2 gap-2">
                                        <textarea class="form-control form-control-sm" id="txSegPausa" runat="server" cols="25" rows="5" disabled="disabled"></textarea>
                                    </div>

                                </div>

                                <div class="col-4">
                                    <nav class="navbar navbar-expand-sm navbar-light bg-light  mt-lg-4">
                                        <div class="container-fluid">

                                            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                                <span class="navbar-toggler-icon"></span>
                                            </button>
                                            <div class="collapse navbar-collapse" id="ejemplo3">
                                                <ul class="navbar-nav mx-auto contenedor-icono">

                                                    <div class="contenedor-icono">

                                                        <asp:CheckBox ID="chxGuardarDetalle" runat="server" CssClass="hidden-textBox" />

                                                        <a class="icong disabled" href="#" title="Nueva Detalle" id="NuevoDetalle" onclick="NuevoDetalle()">
                                                            <i class="bi bi-file-earmark"></i>
                                                        </a>

                                                        <a class="icong disabled" href="#" title="Importar Detalle de la Solicitud de Origen" id="ImportarDetalle">
                                                            <i class="bi bi-arrow-bar-down"></i>
                                                        </a>

                                                        <asp:LinkButton class="icong disabled" runat="server" title="Guardar Detalle" ID="GrabarDetalle" OnClick="GuardarModificarDetalle">
                                                           <i class="bi bi-save2"></i>
                                                        </asp:LinkButton>

                                                        <a class="icong disabled" href="#" title="Modificar Detalle" id="ModificarDetalle" onclick="ModificarDetalle()">
                                                            <i class="bi bi-pencil-square"></i>
                                                        </a>

                                                        <asp:LinkButton class="icong disabled" title="Documentacion Producto" ID="Documentacion" runat="server" OnClientClick="abrirOtraPestana2();">
                                                          <i class="bi bi-paperclip"></i>
                                                        </asp:LinkButton>


                                                        <asp:LinkButton class="icong disabled" title="Carrito" ID="Carrito" runat="server">
                                                         <i class="bi bi-cart4"></i>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton class="icong disabled" title="Carrito" ID="OkCompras" runat="server">
                                                        <i class="bi bi-arrow-down-left-circle"></i>
                                                        </asp:LinkButton>
                                                       
                                                        <asp:LinkButton class="icong disabled" title="Quitar Detalle " ID="QuitarDetalle" runat="server" OnClick="EliminarDetalle" OnClientClick="return confirmarQuitarDetalle(event);">
                                                         <i class="bi bi-dash-circle"></i>
                                                        </asp:LinkButton>
                                                     
                                                        <asp:Label ID="lbIdDetalle" runat="server" Text="Label"></asp:Label>

                                                        <ul />
                                                </ul>
                                            </div>

                                        </div>
                                    </nav>

                                    <div class=" input-group input-group-sm  mt-2 gap-2 justify-content-center">
                                        <asp:Button class=" btn btn-warning  " ID="btnProgramarSolicitud" runat="server" Text="Programar" OnClick="ProgramarSolicitud"  OnClientClick="return confirmProgramarSolicitud(event);" />
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
                        <div class="container-fluid ">

                            <%-- Desarrollo--%>

                            <h4 style="border-radius: 0.5rem; height: 2.5rem" class="text-center pb-2 mb-2" title="Listado de Ordenes de Trabajo (OT) Correspondiente a Desarrollo">OTs Desarrollo</h4>
                            <asp:TextBox ID="tbNombreAsesor" type="text" class="form-control form-control-sm" CssClass="hidden-textBox" runat="server"></asp:TextBox>

                            <div class="row">
                                <div class="col-2">
                                    <div class="input-group  input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbZona" class="form-label" Text="Zona" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlZona" runat="server" DataSourceID="Zona" DataTextField="Zona" DataValueField="Zona" OnSelectedIndexChanged="CambioZona" AutoPostBack="true" OnDataBound="ddlZona_DataBound"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="Zona" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="
                                        select Zona from tblRender group by Zona"></asp:SqlDataSource>

                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-2">
                                        <asp:Button ID="btnTrabajarSolicitud" CssClass="btn btn-outline-secondary" runat="server" Text="Trabajar Solicitud" />
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-2">
                                        <asp:Button ID="btnDesprogramar" CssClass="btn btn-outline-secondary" runat="server" Text="Desprogramar" />
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group  input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbFechaPactoentrega" class=" col-form-label-sm" Text="Pacto Entrega" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbFechaPactoentrega" type="date" class="form-control " runat="server"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group  input-group-sm  mb-2 gap-2">
                                        <asp:CheckBox ID="chxConvenciones" OnCheckedChanged="chxConvenciones_CheckedChanged" AutoPostBack="true" runat="server" />
                                        <asp:Label ID="lbConvenciones" class=" col-form-label-sm" Text="Convenciones" runat="server"></asp:Label>


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
                                                                    <label for="lbNoProgamado" class="form-label">No programado por Ventas</label>
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
                                                                    <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: darkorange"></div>
                                                                    <label for="lbTerminado" class="form-label">Urgente</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: aqua"></div>
                                                                    <label for="lbPausado" class="form-label">Pausados</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #83F455"></div>
                                                                    <label for="lbPausado" class="form-label">Desarrollo Complejo </label>
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

                            <div class="row pb-lg-2 mb-lg-2 justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 12rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-start">Desarrollo</h6>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" ID="DataGrid1" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="CargarDesarrollos" OnItemDataBound="DataGridDesarrollo_ItemDataBound" OnItemCommand="DataGridSolicitudPE_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerDesarrollo" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>"
                                                                    OnClientClick="activarTab('BitacoraDesarrollo-content');" />
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

                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource runat="server" ID="Desarrollo" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="spObtenerSolicitudesDiseEspe" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlZona" PropertyName="SelectedValue" Name="Zona" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="CargarDesarrollos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="   SELECT *
                                                        FROM tblSoliciDiseEspe  WHERE Terminado = 0  AND TipoSolicitud ='DESARROLLO' AND Asesor =@Asesor ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>



                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <%-- cotizaciones--%>
                            <h4 style="border-radius: 0.5rem; height: 2.5rem" class="text-center" title="Listado  Correspondiente a Cotizaciones">Cotizaciones</h4>

                            <div class="row">
                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-4">
                                        <asp:Label ID="lbZona2" class="form-label" Text="Zona" runat="server"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlZona2" runat="server" DataSourceID="Zona2" DataTextField="Zona" DataValueField="Zona" OnSelectedIndexChanged="CambioZona2" AutoPostBack="true" OnDataBound="ddlZona2_DataBound"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="Zona2" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="
                                        select Zona from tblRender group by Zona"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-2">
                                        <asp:Button ID="btnTrbajarCotizacion" CssClass="btn btn-outline-secondary" runat="server" Text="Trabajar Cotización" />
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm  mb-2 gap-2">
                                        <asp:Button ID="btnDesprogramar1" CssClass="btn btn-outline-secondary" runat="server" Text="Desprogramar" />
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group  input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbPactoEntrega1" class=" col-form-label-sm" Text="Pacto Entrega" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbPactoEntrega" type="date" class="form-control " runat="server"></asp:TextBox>

                                    </div>
                                </div>

                            </div>

                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 12rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-start">Cotizacion</h6>
                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" ID="DataGrid2" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="CargarCotizaciones" OnItemDataBound="DataGridCotizacion_ItemDataBound" OnItemCommand="DataGridSolicitudPE_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerCotizacion" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>"
                                                                    OnClientClick="activarTab('BitacoraDesarrollo-content');" />
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
                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource runat="server" ID="Cotizaciones" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="spObtenerSolicitudCotizaciones" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlZona2" PropertyName="SelectedValue" Name="Zona" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                <asp:SqlDataSource ID="CargarCotizaciones" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="   SELECT *
                                                        FROM tblSoliciDiseEspe  WHERE Terminado = 0  AND TipoSolicitud ='COTIZACIÓN' AND Asesor =@Asesor ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
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


            <div class="tab-pane fade " id="BuscarDesarrollo-content">
                <asp:UpdatePanel ID="PanelBuscar" runat="server">
                    <ContentTemplate>
                        <div class="container">

                            <%-- Buscar Diseño Especial --%>


                            <div class="row pt-2">
                                <div class="col-8">
                                    <div class="input-group input-group-sm  mb-2 gap-3">
                                        <asp:Label ID="lbFechaIni" class="form-label" Text="Fecha Ingreso" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbFechaIni" type="date" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label ID="lbY" class="form-label" Text=" Y " runat="server"></asp:Label>
                                        <asp:TextBox ID="tbFechaFin" type="date" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Button ID="btnConsultar" type="button" Text="Consultar" class="btn btn-outline-secondary" runat="server" OnClick="ConsultarSolicitud"></asp:Button>
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
                                        <asp:Label ID="lbNumeroSolicitud1" class="form-label" Text="Solicitud N." runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:TextBox ID="tbSolicitud1" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <%-- Pendiente por añadir Convenciones al buscador --%>


                            <div class="row justify-content-center pt-3">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive mb-2 gap-2" style="max-height: 23rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Solicitudes Filtradas</h5>

                                                <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="BuscarDesarrollo" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridBuscarDesarrollo_ItemDataBound" OnItemCommand="DataGridSolicitudPE_LinkButton">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                    <Columns>


                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerBuscado" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>"
                                                                    OnClientClick="activarTab('BitacoraDesarrollo-content');" />
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
                                                    </Columns>

                                                </asp:DataGrid>
                                                <asp:SqlDataSource ID="SolicXFecha" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand=" SELECT *
                                                                             FROM tblSoliciDiseEspe WHERE Asesor = @Asesor AND Fecha_Ingreso between @FechaIni and @FechaFin  ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="SolicXProyecto" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand=" SELECT * 
                                                                       FROM tblSoliciDiseEspe  WHERE Asesor = @Asesor AND  Proyecto  LIKE '%' + @Proyecto + '%' AND Fecha_Ingreso between @FechaIni and @FechaFin ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbProyectoX" PropertyName="Text" Name="Proyecto"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="SolicitudXID" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand=" SELECT * 
                                                                       FROM tblSoliciDiseEspe  WHERE Asesor = @Asesor AND  ID_Solicitud  LIKE '%' + @Solicitud + '%' AND Fecha_Ingreso between @FechaIni and @FechaFin ORDER BY Fecha_Ingreso ASC;">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="tbNombreAsesor" PropertyName="Text" Name="Asesor"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaIni" PropertyName="Text" Name="FechaIni"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaFin" PropertyName="Text" Name="FechaFin"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbSolicitud1" PropertyName="Text" Name="Solicitud"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource ID="solicitudXCliente" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand=" SELECT * 
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
        // se Habilitan enlaces Iniciales 
        document.getElementById("NuevaSolicitud").classList.add("enabled");
        document.getElementById("Observaciones").classList.add("enabled");
        document.getElementById("CancelarSolicitud").classList.add("enabled");

    


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
            document.getElementById("NuevaSolicitud").classList.remove("enabled");
            document.getElementById("ModificarSolicitud").classList.remove("enabled");


            // Habilitar enlaces
            document.getElementById("GrabarSolicitud").classList.add("enabled");


            //Habilitar
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = false;



            var checkBox = document.getElementById('<%= chkEstadoGuardarSolicitud.ClientID %>');
            checkBox.checked = true; // Marcar el CheckBox para saber si insertar o modificar


            // Limpiar el contenido del label
            var label = document.getElementById("lbNumeroSolicitud");
            label.textContent = "";



        }

        function CancelarSolicitud() {
            // Habilitar enlaces 
            document.getElementById("NuevaSolicitud").classList.add("enabled");



            // Deshabilitar enlaces
            document.getElementById("GrabarSolicitud").classList.remove("enabled");
            document.getElementById("ModificarSolicitud").classList.remove("enabled");


            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    if (dropDownLists[j].id != "ddlZona2") {
                        dropDownLists[j].disabled = true;
                        dropDownLists[j].value = "";
                    }
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

        }

        function ModificarSolicitud() {
            // Habilitar enlaces 
            document.getElementById("GrabarSolicitud").classList.add("enabled");

            // Deshabilitar enlaces
            document.getElementById("NuevaSolicitud").classList.remove("enabled");
            document.getElementById("ModificarSolicitud").classList.remove("enabled");

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


            var checkBox = document.getElementById('<%= chkEstadoGuardarSolicitud.ClientID %>');
            checkBox.checked = false; // Desmarcar el CheckBox

        }

        function HabilitarEnlaces1() {

            // Habilitar enlaces de Detalle
            document.getElementById("NuevoDetalle").classList.add("enabled");
            document.getElementById("ImportarDetalle").classList.add("enabled");

            // Habilitar Enlaces de la  Solcitud
            document.getElementById("NuevaSolicitud").classList.add("enabled");
            document.getElementById("ModificarSolicitud").classList.add("enabled");


            // Deshabilitar enlaces de la solicitud
            document.getElementById("GrabarSolicitud").classList.remove("enabled");

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    if (dropDownLists[j].id != "ddlZona2") {
                        dropDownLists[j].disabled = true;

                    }

                }
            }

            document.getElementById("txInformacionDetalle").value = "";



        }

        function HabilitarEnlaces2() {

            // Habilitar enlaces
            document.getElementById("ModificarDetalle").classList.add("enabled");
            document.getElementById("Documentacion").classList.add("enabled");
            document.getElementById("QuitarDetalle").classList.add("enabled");
            document.getElementById("NuevoDetalle").classList.add("enabled");
            document.getElementById("ImportarDetalle").classList.add("enabled");


            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    if (dropDownLists[j].id != "ddlZona2") {
                        dropDownLists[j].disabled = true;

                    }
                }

            }

        }

        function HabilitarEnlaces3() {

            // Habilitar enlaces
            document.getElementById("Documentacion").classList.add("enabled");

            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    if (dropDownLists[j].id != "ddlZona2") {
                        dropDownLists[j].disabled = true;

                    }
                }

            }



        }

        function HabilitarEnlaces4() {

            // Habilitar Enlaces de la  Solcitud
            document.getElementById("NuevaSolicitud").classList.add("enabled");

            document.getElementById("ModificarSolicitud").classList.remove("enabled");
            document.getElementById("GrabarSolicitud").classList.remove("enabled");


            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id != "ddlZona") {
                    if (dropDownLists[j].id != "ddlZona2") {
                        dropDownLists[j].disabled = true;

                    }
                }

            }



        }

        function abrirOtraPestana() {
            // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
            window.open('Clientes.aspx', '_blank');
        }

        function NuevoDetalle() {

            // Habilitar Enlaces de la  Solcitud
            document.getElementById("GrabarDetalle").classList.add("enabled");



            // Deshabilitar enlaces de la solicitud
            document.getElementById("NuevoDetalle").classList.remove("enabled");
            document.getElementById("ImportarDetalle").classList.remove("enabled");
            document.getElementById("ModificarDetalle").classList.remove("enabled");
            document.getElementById("QuitarDetalle").classList.remove("enabled");
            document.getElementById("Documentacion").classList.remove("enabled");

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbProyecto" && textBoxes[i].id !== "tbSolicitudOrigen" && textBoxes[i].id !== "tbCotizacionEsp" && textBoxes[i].id !== "tbCliente"
                    && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbTelefono" && textBoxes[i].id !== "tbTelefono"
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
                    checkBox.disabled = false; // Habilita el CheckBox
                }
            }


            // Limpiar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {

                if (textAreas[k].id != "txobsCompras" && textAreas[k].id != "txObsDesarrollo" && textAreas[k].id != "txInformacionDetalle" && textAreas[k].id != "txSegPausa") {
                    textAreas[k].disabled = false;
                }

            }

            var checkBox = document.getElementById('<%= chxGuardarDetalle.ClientID %>');
            checkBox.checked = true; // Marcar el CheckBox para saber si insertar o modificar



        }

        function ModificarDetalle() {

            // Habilitar Enlaces de la  Solcitud
            document.getElementById("GrabarDetalle").classList.add("enabled");



            // Deshabilitar enlaces de la solicitud
            document.getElementById("NuevoDetalle").classList.remove("enabled");
            document.getElementById("ImportarDetalle").classList.remove("enabled");
            document.getElementById("ModificarDetalle").classList.remove("enabled");

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbProyecto" && textBoxes[i].id !== "tbSolicitudOrigen" && textBoxes[i].id !== "tbCotizacionEsp" && textBoxes[i].id !== "tbCliente"
                    && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbTelefono" && textBoxes[i].id !== "tbTelefono"
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
                    checkBox.disabled = false; // Habilita el CheckBox
                }
            }


            // Limpiar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {

                if (textAreas[k].id != "txobsCompras" && textAreas[k].id != "txObsDesarrollo" && textAreas[k].id != "txInformacionDetalle" && textAreas[k].id != "txSegPausa") {
                    textAreas[k].disabled = false;
                }

            }

            var checkBox = document.getElementById('<%= chxGuardarDetalle.ClientID %>');
            checkBox.checked = false; // Marcar el CheckBox para saber si insertar o modificar

        }

        function abrirOtraPestana2() {
            // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
            window.open('AdjuntarDocumentos.aspx', '_blank');
        }

        //Funcion para cuando seleccionan un desarrollo o una cotizacion no lleva al formulario
        function activarTab(tabId) {
            // Oculta todas las pestañas
            $('#miPestañas a.Programacion-content').removeClass('active');
            $('.tab-pane').removeClass('active show');

            // Activa la pestaña deseada
            $('#miPestañas a[href="#' + tabId + '"]').tab('show');
        }

        function validarFormularioSolicitud() {
            var proyecto = document.getElementById("tbProyecto").value;
            var dirigido = document.getElementById("ddlDirigido").value;
            var Tipo = document.getElementById("ddlTipo").value;
            var CotEsp = document.getElementById("tbCotizacionEsp").value;
            var Asesor = document.getElementById("ddlAsesor").value;
            var Cliente = document.getElementById("tbClienteServidor").value;

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
            } else if (CotEsp === "") {
                ErrorValidacion.innerHTML = "El campo Cotizacion Esp  es obligatorio.";
                isValid = false;
            }
            else if (Asesor === "") {
                ErrorValidacion.innerHTML = "El Campo Asesor es obligatorio.";
                isValid = false;
            } else if (Cliente === "") {
                ErrorValidacion.innerHTML = "El Campo Cliente es obligatorio.";
                isValid = false;
            }



            // Devuelve true si los campos son válidos, de lo contrario, devuelve false
            return isValid;
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


</body>




</html>
