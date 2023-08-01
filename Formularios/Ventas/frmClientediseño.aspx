<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClientediseño.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.frmClientediseño" ResponseEncoding="utf-8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/Ventas/Gestion_Comercial.css" rel="stylesheet" />





    <title>Clientes Visita</title>
    <style type="text/css">
        .auto-style1 {
            width: 103px;
            height: 87px;
        }
    </style>
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
                                            <li><a class="dropdown-item" href="ventas/Gestion_Comercial.aspx">Gestión comecial</a></li>
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
    <form id="form1" runat="server" style="background-color: #FFFFFF">
        <div>

                    <nav class="navbar navbar-expand-sm navbar-light bg-light" style="background-color: #333333">
            <div class="container" style="background-color: #CCCCCC; font-family: helevetica; color: #FFFFFF;">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">



                    <a class="nav-link active" aria-current="page" href="../../Formularios/Ventas/frmClientediseño.aspx" style="background-color: #CCCCCC; font-family: helvetica; font-size: x-large; color: #000080;">&nbsp;Cliente</a> <a class="nav-link active" aria-current="page"  href="Ventas/Plano.aspx" style="font-family: helvetica; font-size: x-large; background-color: #CCCCCC; color: #000066;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Contacto</a>

                    <ul class="navbar-nav mx-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Ventas/Objetos.aspx" style="font-family: helvetica; font-size: x-large; background-color: #CCCCCC; color: #000066;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Consultas</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Ventas/Modulo.aspx" style="font-family: helvetica; font-size: x-large; background-color: #CCCCCC; color: #000080;">Clientes Nuevos</a>
                        </li>
                    </ul>

                





                </div>
            </div>
        </nav>



        </div>

        <br />
        <h2 style="font-family: helvetica; background-color: #000035; color: #CCCCCC;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <img id="imgClien" alt="" class="auto-style1" src="https://cdn.pixabay.com/photo/2016/06/03/15/35/customer-service-1433641_1280.png" />Clientes</h2>
        <h5 style="font-family: helvetica; background-color: #FFFFFF; color: #000080; font-size: medium;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Listado de clientes registrados en el sistema</h5>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtBuscarNit" runat="server" Width="206px" >Buscar por nit</asp:TextBox>
        <asp:TextBox ID="txtBusacarnom" runat="server" Width="316px" AutoPostBack="True">Buscar por nombre del cliente</asp:TextBox>
        <asp:Button ID="cmdBuscarCliente" runat="server" Text="Buscar" BackColor="#000040" ForeColor="White" Width="75px" />
        <br />
        <br />

        <div style="margin-left: 160px; background-color: #FFFFFF;">
            <asp:GridView ID="dgrdCliente" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Nit" DataSourceID="SqlDataSourceClientes" ForeColor="#333333" GridLines="None" Height="297px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" PageSize="5" Width="1180px">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:CommandField SelectText="✅" ShowSelectButton="True" />
                    <asp:BoundField DataField="Nit" HeaderText="Nit" ReadOnly="True" SortExpression="Nit" />
                    <asp:BoundField DataField="Nombre_Compañia" HeaderText="Nombre Compañia" SortExpression="Nombre_Compañia" />
                    <asp:BoundField DataField="AsesorComercial" HeaderText="Asesor Comercial" ReadOnly="True" SortExpression="AsesorComercial" />
                    <asp:BoundField DataField="Teléfono" HeaderText="Teléfono" SortExpression="Teléfono" />
                    <asp:BoundField DataField="Dirección" HeaderText="Dirección" SortExpression="Dirección" />
                    <asp:BoundField DataField="Procedencia" HeaderText="Procedencia" SortExpression="Procedencia" />
                    <asp:BoundField DataField="FCreación" HeaderText="Fecha de creación" SortExpression="FCreación" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#000020" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
                    <br />
        <br />
            &nbsp;
        <asp:Label ID="Label1" runat="server" Text="NIT"></asp:Label>
&nbsp;<asp:TextBox ID="txtId_Cliente" runat="server" Height="25px" Width="229px" ValidateRequestMode="Disabled" ></asp:TextBox>
        <asp:Label ID="Label2" runat="server" Text="Cliente"></asp:Label>
&nbsp;<asp:TextBox ID="txtNombre_Compañia" runat="server" Width="385px" ValidateRequestMode="Disabled" Height="26px"></asp:TextBox>
&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" Text="Teléfono"></asp:Label>
&nbsp;
        <asp:TextBox ID="txttelcliente" runat="server" Width="174px" ValidateRequestMode="Disabled" Height="30px"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Text="Procedencia"></asp:Label>
        <asp:DropDownList ID="dtacboProcedencia" runat="server" Width="124px">
        </asp:DropDownList>
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label5" runat="server" Text="Dirección"></asp:Label>
        <asp:TextBox ID="txtDir" runat="server" Width="361px" ValidateRequestMode="Disabled" Height="25px"></asp:TextBox>
        <asp:Label ID="Label6" runat="server" Text="Ciudad"></asp:Label>
        <asp:DropDownList ID="cboCiudad" runat="server" Width="219px">
        </asp:DropDownList>
        <asp:Label ID="Label7" runat="server" Text="Compartido Con"></asp:Label>
        <asp:TextBox ID="txtCompartidoCon" runat="server" Width="236px"></asp:TextBox>
&nbsp;<asp:CheckBox ID="CheckBox1" runat="server" Text="Compartir" />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="cmdNevoCliente" runat="server" Text="Nuevo" />
        <asp:Button ID="Button2" runat="server" Text="Modificar" />
        <asp:Button ID="Button3" runat="server" Text="Grabar" />
        <asp:Button ID="Button4" runat="server" Text="Eliminar" />
        <asp:Button ID="Button5" runat="server" Text="Cancelar" />
        <br />
        <br />
        <br />
        </div>

        <asp:SqlDataSource ID="SqlDataSourceClientes" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT a.Id_Cliente AS Nit, a.NombreCompañía AS Nombre_Compañia, 
b.Nombre + ' ' + b.Apellidos AS AsesorComercial, a.Fecha_Creacion AS FCreación, a.Teléfono, x.Procedencia, a.Dirección
FROM tblCliente AS a 
INNER JOIN tblProcedenciaCliente AS X ON X.IdProcedencia = a.IdProcedencia 
INNER JOIN tblAsesorComercial AS b ON b.Cedula = a.Asesor order by Fecha_Creacion desc"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceCiudad" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT { fn CONCAT(tblDepartamentoPais.CodigoDepartamento, tblCiudad.CodigoCiudad) } AS CodCompleto, tblCiudad.NombreCiudad FROM tblDepartamentoPais INNER JOIN tblCiudad ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento ORDER BY { fn CONCAT(tblCiudad.NombreCiudad, ' - ', tblDepartamentoPais.NombreDepartamento) }"></asp:SqlDataSource>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
