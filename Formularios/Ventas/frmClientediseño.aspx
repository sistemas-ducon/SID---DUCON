<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClientediseño.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Ventas.frmClientediseño" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <form id="form1" runat="server">
        <div>

                    <nav class="navbar navbar-expand-sm navbar-light bg-light">
            <div class="container">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">



                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="../../Formularios/Ventas/frmClientediseño.aspx">Cliente</a>
                        </li>
                    </ul>


                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page"  href="Ventas/Plano.aspx">Contacto</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav mx-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Ventas/Objetos.aspx">Consultas</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Ventas/Modulo.aspx">Clientes Nuevos</a>
                        </li>
                    </ul>

                





                </div>
            </div>
        </nav>



        </div>

        <br />
        <h2 style="font-family: helvetica; background-color: #00002D; color: #FFFFFF;">&nbsp;&nbsp;&nbsp;
            <img id="imgClien" alt="" class="auto-style1" src="https://cdn.pixabay.com/photo/2016/06/03/15/35/customer-service-1433641_1280.png" />Clientes</h2>
        <h5 style="font-family: helvetica; background-color: #C0C0C0; color: #000080;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Listado de clientes registrados en el sistema</h5>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBox1" runat="server" Width="206px" >Buscar por nit</asp:TextBox>
        <asp:TextBox ID="txtBuscarCliente" runat="server" Width="316px" AutoPostBack="True">Buscar por nombre del cliente</asp:TextBox>
        <asp:Button ID="cmdBuscarCliente" runat="server" Text="Buscar" BackColor="#000040" ForeColor="White" Width="75px" />
        <br />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="White" BorderStyle="Solid" BorderWidth="50px" CellPadding="4" CellSpacing="2" DataKeyNames="Nit" DataSourceID="SqlDataSourceClientes" Font-Size="Smaller" ForeColor="Black" Width="1171px" Height="326px">
            <Columns>
                <asp:BoundField DataField="Nit" HeaderText="Nit" ReadOnly="True" SortExpression="Nit" />
                <asp:BoundField DataField="Nombre_Compañia" HeaderText="Nombre_Compañia" SortExpression="Nombre_Compañia" />
                <asp:BoundField DataField="AsesorComercial" HeaderText="AsesorComercial" ReadOnly="True" SortExpression="AsesorComercial" />
                <asp:BoundField DataField="FCreación" HeaderText="FCreación" SortExpression="FCreación" />
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
            <EditRowStyle BorderStyle="Dashed" />
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="#00002F" Font-Bold="True" ForeColor="White" Width="50px" />
            <PagerStyle BackColor="#CCCCCC" BorderStyle="Double" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="Gray" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="NIT"></asp:Label>
&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBox2" runat="server" Height="22px" Width="229px"></asp:TextBox>
        <asp:Label ID="Label2" runat="server" Text="Cliente"></asp:Label>
&nbsp;<asp:TextBox ID="TextBox3" runat="server" Width="209px"></asp:TextBox>
&nbsp;&nbsp;
        <asp:Label ID="Label3" runat="server" Text="Teléfono"></asp:Label>
&nbsp;
        <asp:TextBox ID="TextBox4" runat="server" Width="217px"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Text="Procedencia"></asp:Label>
&nbsp;
        <asp:DropDownList ID="DropDownList1" runat="server" Width="189px">
        </asp:DropDownList>
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label5" runat="server" Text="Dirección"></asp:Label>
        <asp:TextBox ID="TextBox5" runat="server" Width="373px"></asp:TextBox>
        <asp:Label ID="Label6" runat="server" Text="Ciudad"></asp:Label>
        <asp:DropDownList ID="DropDownList2" runat="server" Width="189px">
        </asp:DropDownList>
        <asp:Label ID="Label7" runat="server" Text="Compartido Con"></asp:Label>
        <asp:TextBox ID="TextBox6" runat="server" Width="236px"></asp:TextBox>
&nbsp;<asp:CheckBox ID="CheckBox1" runat="server" Text="Compartir" />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="cmdNevoCliente" runat="server" Text="Nuevo" />
        <asp:Button ID="Button2" runat="server" Text="Modificar" />
        <asp:Button ID="Button3" runat="server" Text="Grabar" />
        <asp:Button ID="Button4" runat="server" Text="Eliminar" />
        <asp:Button ID="Button5" runat="server" Text="Cancelar" />
        <br />
        <br />
        <br />
        <asp:SqlDataSource ID="SqlDataSourceClientes" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="SELECT a.Id_Cliente AS Nit, a.NombreCompañía AS Nombre_Compañia, b.Nombre + b.Apellidos AS AsesorComercial, a.Fecha_Creacion AS FCreación FROM tblCliente AS a INNER JOIN tblAsesorComercial AS b ON b.Cedula = a.Asesor"></asp:SqlDataSource>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
