<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Insumos.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Insumos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />


    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/insumos.css" rel="stylesheet" />
    <title>Insumos</title>
</head>
<body>

     <header>
            <nav class="navbar navbar-expand-lg navbar-light bg-light pt-0">

                <div class="container-fluid" style="background-color: #081a2c">

                    <div class="collapse navbar-collapse" id="navbarScroll">

                        <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="Departamento" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Departamento</a>
                                <ul class="dropdown-menu" aria-labelledby="Departamento">

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle  " href="#" id="Administrativo" role="button" data-bs-toggle="dropdown" aria-expanded="false">Administrativo </a>
                                        <ul class="dropdown-menu">
                                            <li class="nav-item dropdown ">
                                                <a class="nav-link dropdown-toggle " href="#" id="Gerencia" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="padding-left: 1rem">Gerencia Comercial </a>
                                                <ul class="dropdown-menu ">
                                                    <li><a class="dropdown-item" href="#">Actualizar Precios</a></li>
                                                    <li><a class="dropdown-item" href="#">Estadisticas de Venta</a></li>
                                                    <li><a class="dropdown-item" href="#">Seguimiento de Cotizaciones</a></li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Compras" role="button" data-bs-toggle="dropdown" aria-expanded="false">Compras</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Generar codigo de inventario</a></li>
                                            <li><a class="dropdown-item" href="#">Solicitud de producto especial</a></li>
                                            <li><a class="dropdown-item" href="#">Orden de abastecimiento interna</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Dise_Desa" role="button" data-bs-toggle="dropdown" aria-expanded="false">Diseño | Desarrollo</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Estadisticas de diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Estadisticas desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Generar código de inventario</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                            <li><a class="dropdown-item" href="#">Bitacora renders</a></li>
                                            <li><a class="dropdown-item" href="#">Bitacora desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Bitacora diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Estadisticas desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Estadisticas dibujo</a></li>
                                            <li><a class="dropdown-item" href="#">Reproceso dibujo</a></li>
                                            <li><a class="dropdown-item" href="#">Reproceso desarrollo</a></li>
                                            <li><a class="dropdown-item" href="#">Diseño en el exterior</a></li>
                                            <li><a class="dropdown-item" href="#">Tabla de diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Plano</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Fact_Cart" role="button" data-bs-toggle="dropdown" aria-expanded="false">Facturacion y cartera</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Cierre de obra</a></li>
                                            <li><a class="dropdown-item" href="#">Control de obra</a></li>
                                            <li><a class="dropdown-item" href="#">Despacho de obras</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                            <li><a class="dropdown-item" href="#">Programación ordenes de T'S de SID</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Gest_Cali_Adno" role="button" data-bs-toggle="dropdown" aria-expanded="false">Gestio de calidad adnom</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Acciones de mejora</a></li>
                                            <li><a class="dropdown-item" href="#">Entrega perfecta</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Gest_Cali_Usr" role="button" data-bs-toggle="dropdown" aria-expanded="false">Gestion de calidad usr</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Accion de mejora</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Instalacion" role="button" data-bs-toggle="dropdown" aria-expanded="false">Instalacion</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Cierre de obra</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Recepcion" role="button" data-bs-toggle="dropdown" aria-expanded="false">Recepcion</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Generar cotización</a></li>
                                            <li><a class="dropdown-item" href="#">Ingresar cotizacion</a></li>
                                            <li><a class="dropdown-item" href="#">Tabla de diseños</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Sistemas" role="button" data-bs-toggle="dropdown" aria-expanded="false">Sistemas</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Administracion</a></li>
                                            <li><a class="dropdown-item" href="#">Asignar permiso</a></li>
                                            <li><a class="dropdown-item" href="#">Configurar sede</a></li>
                                            <li><a class="dropdown-item" href="#">Ventana principal</a></li>
                                        </ul>
                                    </li>

                                    <li class="nav-item dropend">
                                        <a class="nav-link dropdown-toggle " href="#" id="Ventas" role="button" data-bs-toggle="dropdown" aria-expanded="false">Ventas</a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Gestión comecial</a></li>
                                            <li><a class="dropdown-item" href="#">Licitaciones</a></li>
                                            <li><a class="dropdown-item" href="frmPrincipal.aspx">Ordenes de trabajo</a></li>
                                            <li><a class="dropdown-item" href="#">Programar diseño</a></li>
                                            <li><a class="dropdown-item" href="#">Programar render</a></li>
                                            <li><a class="dropdown-item" href="#">Seguimiento cotizaciones</a></li>
                                            <li><a class="dropdown-item" href="#">Solicitud producto especial</a></li>
                                            <li><a class="dropdown-item" href="#">Visitas asesores</a></li>
                                        </ul>
                                    </li>
                                </ul>

                            </li>


                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="Personas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Personas </a>
                                <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                    <li><a class="dropdown-item" href="#">Cliente</a></li>
                                    <li><a class="dropdown-item" href="#">Empleado</a></li>

                                </ul>
                            </li>

                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" href="#" id="Consultas" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="color: #FFFFFF">Consultas</a>
                                <ul class="dropdown-menu" aria-labelledby="navbarScrollingDropdown">
                                    <li><a class="dropdown-item" href="#">Reprocesos</a></li>
                                    <li><a class="dropdown-item" href="#">Despacho de obra</a></li>
                                    <li class="nav-item dropend ">
                                        <a class="nav-link dropdown-toggle " href="#" id="OT" role="button" data-bs-toggle="dropdown" aria-expanded="false" style="padding-left: 1rem">Ordenes de trabajo </a>
                                        <ul class="dropdown-menu">
                                            <li><a class="dropdown-item" href="#">Programacion de OT</a></li>
                                            <li><a class="dropdown-item" href="#">Todas las OT (Manuales /SID)</a></li>
                                        </ul>
                                    </li>

                                </ul>

                            </li>
                        </ul>

                        <asp:Label ID="lblBienvenida" runat="server" ForeColor="White"></asp:Label>

                    </div>

                </div>

            </nav>
        </header>

    <%--Comienza Panel principal de nombres--%>
    <nav class="navbar navbar-expand-sm navbar-light bg-light">
        <div class="container">

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">



                <ul class="navbar-nav me-auto">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="frmPrincipal.aspx">Ordenes de trabajo</a>
                    </li>
                </ul>


                <ul class="navbar-nav me-auto">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="Plano.aspx">Plano</a>
                    </li>
                </ul>

                <ul class="navbar-nav mx-auto">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="Objetos.aspx">Objetos</a>
                    </li>
                </ul>

                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="modulo.aspx">Modulos</a>
                    </li>
                </ul>

                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link active" aria-current="page" href="Insumos.aspx">Insumos</a>
                    </li>
                </ul>





            </div>
        </div>
    </nav>
    <%--Termina Panel principal de nombres--%>


    <%--Comienza Panel de iconos--%>
    <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
        <div class="container-fluid">

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="ejemplo2">
                <ul class="navbar-nav mx-auto contenedor-icono">

                    <div class="contenedor-icono">

                         <a class="text-dark" href="#" title="Nuevo Insumo">

                            <i class="bi bi-file-earmark"></i>
                        </a>
                        <a class="text-dark" href="#" title="...">

                            <i class="bi bi-file-earmark-ruled"></i>
                        </a>
                        <a class="text-dark" href="#" title="Modificar Insumo">

                            <i class="bi bi-wrench"></i>
                        </a>
                        <a class="text-dark" href="#" title="Eliminar Insumo">

                            
                            <i class="bi bi-database-x"></i>
                        </a>
                        <a class="text-dark" href="#" title="Copiar Insumo">

                            <i class="bi bi-files"></i>
                        </a>
                        <a class="text-dark" href="#" title="Buscar Insumo">

                            <i class="bi bi-search"></i>
                        </a>
                        <a class="text-dark" href="#" title="Actualizar">

                               <i class="bi bi-disc"></i>
                        </a>

                        
                        
                        
                     
                        
                        
                     

                    </div>
                </ul>
            </div>

        </div>
    </nav>
    <%--Termina Panel de iconos--%>


    <div class="container insumos  overflow-auto mt-5" style="height:300rem">
        <form class="control" action="#" runat="server">
        <%--FORMA DE HACERLO CON DATAGRIDVIEW--%>

<%--    <asp:GridView class="table table-responsive custom-grid" ID="GridView1" runat="server" DataSourceID="DataGridInsumos" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField ControlStyle-CssClass="text-decoration-none text-dark" HeaderText="Insumo">
            <ItemTemplate>
                <asp:LinkButton ID="lnkInsumo" runat="server" Text='<%# Eval("Id_Insumo") %>' OnClientClick="mostrarModal(); return false;"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ID_Inventario" HeaderText="Cod.Inv" />
        <asp:BoundField DataField="Descripcion_Insumo" HeaderText="Descripcion" />
        <asp:BoundField DataField="Descripcion" HeaderText="Tipo Insumo" />
        <asp:BoundField DataField="Valor_Unitario" HeaderText="Valor Unitario" />
        <asp:BoundField DataField="Factor_Ganancia" HeaderText="F.G" />
        <asp:BoundField DataField="Factor_Desperdicio" HeaderText="F.D" />
        <asp:BoundField DataField="AplicacionAcabado" HeaderText="A.A" />
        <asp:BoundField DataField="FechaCreacion" HeaderText="Creacion" />
        <asp:BoundField DataField="FechaActualizacion" HeaderText="U.Actualizacion" />
        <asp:BoundField DataField="Responsable" HeaderText="Responsable" />
    </Columns>
</asp:GridView>

    <asp:SqlDataSource runat="server" ID="DataGridInsumos" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>
        "
        SelectCommand="ctaInsumos" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

    <div id="myModal" class="modal fade" role="dialog">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link active" data-toggle="tab" href="#insumo">Insumo</a>
                       
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#tipoInsumo">Tipo Insumo</a>
                        
                    </li>
                </ul>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="tab-content">
                    <div id="insumo" class="tab-pane fade show active">
                        <!-- Contenido de la pestaña "Insumo" -->
                    </div>
                    <div id="tipoInsumo" class="tab-pane fade">
                        <!-- Contenido de la pestaña "Tipo Insumo" -->
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    function mostrarModal() {
        $('#myModal').modal('show');
    }
</script>--%>

    <%-- FORMA DE HACERLO CON UN DATAGRID--%>



<asp:DataGrid Class="table table-responsive custom-grid" ID="DataGrid1" runat="server" DataSourceID="DataGridInsumos" AutoGenerateColumns="false" OnItemCommand="DataGrid1_ItemCommand" DataKeyField="Id_Insumo">

        <Columns>

            <asp:TemplateColumn HeaderText="Insumo">
                <ItemTemplate>
                    <asp:LinkButton CssClass="text-decoration-none text-dark" ID="lnkInsumo" runat="server" Text='<%# Eval("Id_Insumo") %>'
                        CommandName="RedirectToInsumoConsultar" CommandArgument='<%# Container.ItemIndex %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>

            <asp:BoundColumn DataField="ID_Inventario" HeaderText="Cod.Inv" />
            <asp:BoundColumn DataField="Descripcion_Insumo" HeaderText="Descripcion" />
            <asp:BoundColumn DataField="Descripcion" HeaderText="Tipo Insumo" />
            <asp:BoundColumn DataField="Valor_Unitario" HeaderText="Valor Unitario" />
            <asp:BoundColumn DataField="Factor_Ganancia" HeaderText="F.G" />
            <asp:BoundColumn DataField="Factor_Desperdicio" HeaderText="F.D" />
            <asp:BoundColumn DataField="AplicacionAcabado" HeaderText="A.A" />
            <asp:BoundColumn DataField="FechaCreacion" HeaderText="Creacion" />
            <asp:BoundColumn DataField="FechaActualizacion" HeaderText="U.Actualizacion" />
            <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" />

        </Columns>

    </asp:DataGrid>

    <asp:SqlDataSource runat="server" ID="DataGridInsumos" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>
        "
        SelectCommand="ctaInsumos" SelectCommandType="StoredProcedure"></asp:SqlDataSource>

        

    </form>
    </div>

    

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>


         <script type="text/javascript">
             function redirectToInsumoConsultar(source, eventArgs) {
                 if (eventArgs.get_commandName() === "RedirectInsumoConsultar") {
                     var index = eventArgs.get_commandArgument();
                     var grid = document.getElementById("<%= DataGrid1.ClientID %>");
                     var idInsumo = grid.rows[index + 1].cells[0].innerHTML; // El índice + 1 es necesario para omitir el encabezado de la tabla
                     window.location.href = "Insumos_Consultar.aspx?Id_Insumo=" + idInsumo;
                 }
             }
         </script>

</body>
</html>



