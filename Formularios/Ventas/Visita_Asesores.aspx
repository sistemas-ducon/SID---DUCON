<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visita_Asesores.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Visita_Asesores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>


    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/Visita_Asesores.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>
<body>

    <nav class="navbar navbar-light bg-light">
        <div class="container d-flex justify-content-center">
            <span class="navbar-brand mb-0 h1">Visita Asesor
            </span>
        </div>
    </nav>

    <nav class="navbar navbar-light bg-light">
        <div class="container d-flex justify-content-center">
            <ul class="nav nav-tabs">
                <li class="nav-item">
                    <a class="nav-link text-dark active" id="Visitas-tab" data-bs-toggle="tab" href="#Visitas-content">Registro Visitas</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" id="Estadistica-tab" data-bs-toggle="tab" href="#Estadisticas-content">Estadisticas</a>
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


                        <a class="icong disabled" href="#" title="Nueva Visita" id="NuevaVisita" onclick="NuevaVisita()">
                            <i class="bi bi-file-earmark"></i>
                        </a>

                        <a class="icong disabled" href="#" title="Grabar Visita" id="GrabarVisita">
                            <i class="bi bi-save2"></i>
                        </a>

                        <a class="icong disabled" href="#" title="Modificar Visita" id="ModificarVisita">
                            <i class="bi bi-wrench"></i>
                        </a>
                        <a class="icong disabled Cancelar" href="#" title="Cancelar" id="CancelarVisita"  onclick="CancelarVisita()">
                            <i class="bi bi-x-lg"></i>
                        </a>
                        <a class="icong disabled Cancelar" href="#" title="Exportar" id="Exportar">
                            <i class="bi bi-airplane-engines"></i>
                        </a>


                        <ul />
                </ul>
            </div>

        </div>
    </nav>




    <form id="form1" runat="server">


        <div class="tab-content">

            <div class="tab-pane fade show active" id="Visitas-content">

                <div class="container p-1">

                    <div class="row pb-1">

                        <div class="col-4">
                            <div class="input-group input-group-sm  mb-2 gap-2 ">
                                <asp:Label class="form-label" Text="Asesor" runat="server" ID="lbAsesor"></asp:Label>
                                <asp:DropDownList class="form-control" ID="ddlAsesor" runat="server"  AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-4">
                            <div class="input-group input-group-sm  mb-2 gap-2 ">
                                <asp:Label class="form-label" Text="Visitas Por" runat="server" ID="lbVisitasPor"></asp:Label>
                                <asp:DropDownList class="form-control" ID="ddlVisitasPor" runat="server" disabled="false"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-1">
                        </div>

                        <div class="col-3">
                            <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                <label class="form-label" runat="server" id="lbFecha">Fecha </label>
                                <input type="date" id="fecha" class="form-control" runat="server" disabled="disabled" />
                            </div>
                        </div>


                    </div>

                    <div class="row pb-1">

                        <div class="col-4">
                            <div class="input-group input-group-sm  mb-2 gap-2 ">
                                <asp:Button ID="btnCliente" type="button" Text="Cliente" class="btn btn-outline-secondary"
                                    runat="server" Enabled="false"></asp:Button>
                                <asp:TextBox ID="tbCliente" type="text" class="form-control" runat="server" disabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4">
                            <div class="input-group input-group-sm  mb-2 gap-2">
                                <asp:Label ID="lbTelefono" class="form-label" Text="Telefono" runat="server"></asp:Label>
                                <asp:TextBox ID="tbTelefono" type="text" class="form-control " runat="server" disabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-4">
                            <div class="input-group input-group-sm  mb-2 gap-2">
                                <asp:Label ID="lbCotizacion" class="form-label" Text="Cotizacion" runat="server"></asp:Label>
                                <asp:TextBox ID="tbCotizacion" type="text" class="form-control " runat="server" disabled="false"></asp:TextBox>
                            </div>
                        </div>


                    </div>

                    <div class="row pb-1">

                        <div class="col-6">
                            <div class="input-group input-group-sm  mb-2 gap-2">
                                <asp:Label ID="lbContacto" class="form-label" Text="Contacto" runat="server"></asp:Label>
                                <asp:TextBox ID="tbContacto" type="text" class="form-control " runat="server" disabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-6">
                            <div class="input-group input-group-sm  mb-2 gap-2">
                                <asp:Label ID="lbMailCont" class="form-label" Text="Mail Contacto" runat="server"></asp:Label>
                                <asp:TextBox ID="tbMailCont" type="text" class="form-control " runat="server" disabled="false"></asp:TextBox>
                            </div>
                        </div>


                    </div>

                    <div class="row pb-1">

                        <div class="col-12">
                            <div class="input-group input-group-sm  mb-2 gap-2">
                                <asp:Label ID="lbObservaciones" class="form-label" Text="Obs." runat="server"></asp:Label>
                                <textarea class="form-control form-control-sm" id="txObs" runat="server" cols="29" rows="3" disabled="disabled"></textarea>

                            </div>
                        </div>




                    </div>

                    <div class="row pt-3">

                        <div class="col-5">
                            <div class="input-group input-group-sm  mb-2 gap-2">
                                <asp:Label ID="lbVistaEntre" class="form-label" Text="Visitas entre" runat="server"></asp:Label>
                                <asp:TextBox ID="fecha1" type="date" runat="server" class="form-control"></asp:TextBox>
                                <asp:TextBox ID="fecha2" type="date" runat="server" class="form-control"></asp:TextBox>


                            </div>
                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm  mb-2 gap-2">

                                <asp:Button ID="Button1" type="button" Text="Consultar" class="btn btn-outline-secondary"
                                    runat="server" OnClick="Consultar" OnClientClick="return validarDropDownList()"></asp:Button>
                            </div>
                        </div>


                    </div>


                    <div class="container mt-4">
                        <div class="row justify-content-center">
                            <div class="border rounded p-2">
                                <div class="row">
                                    <div class="col-12">
                                        <div class="table-responsive mb-2 gap-2" style="max-height: 10rem; overflow-x: auto;">
                                            <h6 class="datagrid-header text-center">Visita Asesores: <asp:Literal runat="server" ID="DateRangeLiteral"></asp:Literal></h6>
                                            <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGrid1" runat="server" DataSourceID="VisitaAse" AutoGenerateColumns="false">
                                                <HeaderStyle Font-Bold="true" />
                                                <Columns>

                                                    <asp:BoundColumn DataField="NombreCompañía" HeaderText="Cliente" />
                                                    <asp:BoundColumn DataField="NombreContacto" HeaderText="Contacto" />
                                                    <asp:BoundColumn DataField="Telefono" HeaderText="Telefono" ItemStyle-CssClass="no-wrap" />
                                                    <asp:BoundColumn DataField="MailContacto" HeaderText="Mail" />
                                                    <asp:BoundColumn DataField="NombreCausa" HeaderText="Visita Por" />
                                                    <asp:BoundColumn DataField="FechaVisita" HeaderText="Fecha Ingreso" />
                                                    <asp:BoundColumn DataField="Cotizacion" HeaderText="Cotizacion " />
                                                </Columns>
                                            </asp:DataGrid>
                                        </div>

                                        <asp:SqlDataSource runat="server" ID="VisitaAse" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_GetVisitasAsesor" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlAsesor" PropertyName="SelectedValue" Name="Asesor" Type="String"></asp:ControlParameter>
                                                <asp:ControlParameter ControlID="fecha1" PropertyName="Text" Name="FechaInicio" Type="DateTime"></asp:ControlParameter>
                                                <asp:ControlParameter ControlID="fecha2" PropertyName="Text" Name="FechaFin" Type="DateTime"></asp:ControlParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>





            </div>

            <div class="tab-pane fade " id="Estadisticas-content">

                <div class="container ">

                    <div class="row pt-1">

                        <div class="col-7">
                            <div class="input-group input-group-sm  mb-2 gap-2">
                                <asp:Label ID="Label1" class="form-label" Text="Visitas entre" runat="server"></asp:Label>
                                <input type="date" id="fecha3" class="form-control" runat="server" />
                                <asp:Label ID="Label2" class="form-label" Text=" Y " runat="server"></asp:Label>
                                <input type="date" id="fecha4" class="form-control" runat="server" />

                            </div>
                        </div>

                        <div class="col-2">
                            <div class="input-group input-group-sm  mb-2 gap-2">

                                <asp:Button ID="Button2" type="button" Text="Consultar" class="btn btn-outline-secondary"
                                    runat="server" ></asp:Button>
                            </div>
                        </div>


                    </div>

                    <div class="row pt-3">

                        <div class="col-12">
                            <div class="Grid">
                            </div>

                        </div>

                        <div class="col-6">
                            <div class="Graficas">
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-12">
                            <nav class="navbar navbar-light bg-light">
                                <div class="container d-flex justify-content-center">
                                    <ul class="nav nav-tabs">
                                        <li class="nav-item">
                                            <a class="nav-link text-dark active" id="DetalleVis-tab" data-bs-toggle="tab" href="#DetalleVis-content">Detalle Visitas</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link text-dark" id="EstadisticaTipo-tab" data-bs-toggle="tab" href="#EstadisticasTipo-content">Estadisticas Tipo Visitas</a>
                                        </li>
                                    </ul>
                                </div>
                            </nav>

                            <div class="tab-content">

                                <div class="tab-pane fade show active" id="DetalleVis-content">
                                    <div class="container">
                                        <h1>Detalle Visitas</h1>
                                    </div>
                                </div>

                                <div class="tab-pane fade " id="EstadisticasTipo-content">
                                    <div class="container">
                                        <h1>Estadisticas Visitas</h1>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>

                </div>


            </div>

        </div>



    </form>


    <script>
        // Obtener la fecha actual
        const fechaActual = new Date();

        // Obtener el primer día del mes actual
        const primerDiaMes = new Date(fechaActual.getFullYear(), fechaActual.getMonth(), 1);
        const primerDiaMesFormateado = primerDiaMes.toISOString().slice(0, 10); // Formato: YYYY-MM-DD

        // Formatear la fecha actual en formato "YYYY-MM-DD"
        const fechaFormateada = fechaActual.toISOString().slice(0, 10);

        // Asignar las fechas a los campos de input
        document.getElementById("fecha").value = fechaFormateada;
        document.getElementById("fecha1").value = primerDiaMesFormateado;
        document.getElementById("fecha2").value = fechaFormateada;
        document.getElementById("fecha3").value = primerDiaMesFormateado;
        document.getElementById("fecha4").value = fechaFormateada;


        // Habilitar enlaces
        document.getElementById("NuevaVisita").classList.add("enabled");

        function validarDropDownList() {
            var ddl = document.getElementById("ddlAsesor").value;
            if (ddl === '0') {
                alert('Favor ingresar el Asesor Comercial.');
                return false; // Evita enviar el formulario si el DropDownList está vacío.
            }
            return true; // Envía el formulario si el DropDownList tiene un valor seleccionado.
        }


        function NuevaVisita() {
            if (validarDropDownList()) {
                // Habilitar enlaces
                document.getElementById("GrabarVisita").classList.add("enabled");
                document.getElementById("CancelarVisita").classList.add("enabled");
                // Deshabilitar enlaces
                document.getElementById("NuevaVisita").classList.remove("enabled");

            }

        }

        function CancelarVisita() {
            // Habilitar enlaces
            document.getElementById("NuevaVisita").classList.add("enabled");
          
            // Deshabilitar enlaces
            document.getElementById("GrabarVisita").classList.remove("enabled");
            document.getElementById("CancelarVisita").classList.remove("enabled");
           
        }

        






    </script>


   
     



    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
