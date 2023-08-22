<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Visita_Asesores.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Visita_Asesores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/Visita_Asesores.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        function GenerarGrafica(nombres, cantidades) {
            var ctx = document.getElementById("grafica").getContext('2d');

            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: nombres,
                    datasets: [{
                        label: 'Cantidad Visitas x Asesor',
                        data: cantidades,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {

                    scales: {
                        x: {
                            ticks: {
                                font: {
                                    size: 7
                                }
                            }
                        },
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });



            document.getElementById("grafica").addEventListener("dblclick", function () {
                var canvas = myChart.canvas;
                var currentWidth = canvas.width;
                var currentHeight = canvas.height;
                canvas.style.width = (currentWidth * 1) + "px";
                canvas.style.height = (currentHeight * 1) + "px";
                myChart.resize();
            });

        }

        function GenerarGrafica1(nombres1, cantidades1) {
            var ctx1 = document.getElementById("grafica1").getContext('2d');


            var myChart1 = new Chart(ctx1, {
                type: 'bar',
                data: {
                    labels: nombres1,
                    datasets: [{
                        label: 'Estadistica Tipo Visita',
                        data: cantidades1,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {

                    scales: {
                        x: {
                            ticks: {
                                font: {
                                    size: 7
                                }
                            }
                        },
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

            document.getElementById("grafica1").addEventListener("dblclick", function () {
                var canvas = myChart1.canvas;
                var currentWidth = canvas.width;
                var currentHeight = canvas.height;
                canvas.style.width = (currentWidth * 2) + "px";
                canvas.style.height = (currentHeight * 2) + "px";
                myChart1.resize();
            });

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

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

                            <a class="icong disabled" href="#" title="Modificar Visita" id="ModificarVisita" onclick="ModificarVisita()">
                                <i class="bi bi-wrench"></i>
                            </a>
                            <a class="icong disabled Cancelar" href="#" title="Cancelar" id="CancelarVisita" onclick="CancelarVisita()">
                                <i class="bi bi-x-lg"></i>
                            </a>

                            <asp:LinkButton class="icong disabled" runat="server" title="Exportar" ID="Exportar" OnClick="ExportarExel2" OnClientClick="DeshabilitarExcel">
                                         <i class="custom-icon"></i>
                            </asp:LinkButton>


                            <ul />
                    </ul>
                </div>

            </div>
        </nav>



        <div class="tab-content">

            <div class="tab-pane fade show active" id="Visitas-content">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="container p-1">

                            <div class="row pb-1">

                                <div class="col-4">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <asp:Label class="form-label" Text="Asesor" runat="server" ID="lbAsesor"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlAsesor" runat="server" OnSelectedIndexChanged="Cambio" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <asp:Label class="form-label" Text="Visitas Por" runat="server" ID="lbVisitasPor"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlVisitasPor" runat="server" disabled="false" DataTextField="NombreCausa" DataValueField="Id_Causa" DataSourceID="CausaVisita"></asp:DropDownList><asp:SqlDataSource runat="server" ID="CausaVisita" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="select * from tblCausaVisita"></asp:SqlDataSource>
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
                                        <asp:Button class="btn btn-outline-secondary" ID="btnCliente" type="button" Text="Cliente" runat="server" OnClientClick="abrirOtraPestana();"></asp:Button>
                                        <asp:TextBox ID="tbCliente" type="text" class="form-control" runat="server" disabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbTelefono" class="form-label" Text="Telefono" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbTelefono" type="text" class="form-control " runat="server" disabled="false"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1">
                                </div>

                                <div class="col-3">
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

                                        <asp:Button ID="btnConsultar" type="button" Text="Consultar" class="btn btn-outline-secondary"
                                            runat="server" OnClick="Consultar" OnClientClick="return Excel()"></asp:Button>
                                    </div>
                                </div>


                            </div>

                            <div class="container mt-4">
                                <div class="row justify-content-center">
                                    <div class="border rounded p-2">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="table-responsive mb-2 gap-2" style="max-height: 15rem; overflow-x: auto;">
                                                    <h6 class="datagrid-header text-center">Visita Asesores 
                                                        <asp:Literal runat="server" ID="DateRangeLiteral"></asp:Literal></h6>
                                                    <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGrid1" runat="server" DataSourceID="VisitaAse" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnPreRender="miDataGrid_PreRender" OnItemDataBound="DataGrid1_ItemDataBound">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header p-2" />
                                                        <Columns>


                                                            <asp:BoundColumn DataField="NombreCompañía" HeaderText="Cliente" />
                                                            <asp:BoundColumn DataField="NombreContacto" HeaderText="Contacto" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Telefono" HeaderText="Telefono" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="MailContacto" HeaderText="Mail" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="NombreCausa" HeaderText="Visita Por" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="FechaVisita" HeaderText="Fecha Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Cotizacion" HeaderText="Cotizacion" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Observacion" ItemStyle-CssClass="d-none" />

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

                            <div class="container mt-4">
                                <div class="row justify-content-center">
                                    <div class="border rounded p-2">
                                        <div class="table-responsive">
                                            <h6 class="datagrid-header text-center">Estadistica Asesor:</h6>
                                            <table class="table table-hover table-bordered">
                                                <thead class="thead-light">
                                                    <tr>
                                                        <th style="white-space: nowrap;">Rango Fechas</th>
                                                        <th style="white-space: nowrap;">Visitas</th>
                                                        <th style="white-space: nowrap;">Visita Levantamiento %</th>
                                                        <th style="white-space: nowrap;">Visita Diseño %</th>
                                                        <th style="white-space: nowrap;">Visita Cliente Nuevo %</th>
                                                        <th style="white-space: nowrap;">Visita Cierre %</th>
                                                        <th style="white-space: nowrap;">Seguimiento Cotizacion %</th>
                                                        <th style="white-space: nowrap;">Mantenimiento %</th>
                                                        <th style="white-space: nowrap;">Entrega Cotizacion %</th>
                                                        <th style="white-space: nowrap;">Cartera %</th>
                                                        <th style="white-space: nowrap;">% Total </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>

                                                        <td style="white-space: nowrap;">
                                                            <label runat="server" id="lbFechas"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbVisitas"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbLev"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbDis"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbCli"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lblCierre"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbSegCot"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbMantenimiento"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbEntregaCot"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbCartera"></label>
                                                        </td>
                                                        <td>
                                                            <label runat="server" id="lbTotal"></label>
                                                        </td>

                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>




                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <div class="tab-pane fade " id="Estadisticas-content">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="container ">

                            <div class="row pt-1">

                                <div class="col-6">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="Label1" class="form-label" Text="Visitas entre" runat="server"></asp:Label>
                                        <asp:TextBox ID="fecha5" type="date" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label ID="Label2" class="form-label" Text=" Y " runat="server"></asp:Label>
                                        <asp:TextBox ID="fecha6" type="date" runat="server" class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2">

                                        <asp:Button ID="Button2" type="button" Text="Consultar" class="btn btn-outline-secondary"
                                            runat="server" OnClick="ConsultarEstadisticas"></asp:Button>
                                    </div>
                                </div>




                            </div>

                            <div class="container mt-4">
                                <div class="row justify-content-center">
                                    <div class="border rounded p-2">
                                        <div class="row">
                                            <div class="col-6">
                                                <div class=" table-responsive mb-2 gap-2" style="max-height: 14rem; overflow-x: auto;">
                                                    <h6 class="datagrid-header text-center">Estadistica Asesores:</h6>
                                                    <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ID="DataGrid2" runat="server" DataSourceID="EstadisticaAsesores" AutoGenerateColumns="false" OnItemCommand="DataGrid2_ItemDataBound">
                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />

                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="...">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="VerDetalle" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="CodigoAsesor" HeaderText="Codigo Asesor" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="Asesor" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column" />
                                                            <asp:BoundColumn DataField="CuentaDeCausa" HeaderText="Cantidad Visitas" ItemStyle-CssClass="auto-width-column" />

                                                        </Columns>
                                                    </asp:DataGrid>
                                                    <asp:SqlDataSource runat="server" ID="EstadisticaAsesores" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="ObtenerDatosAsesoresConVisitas" SelectCommandType="StoredProcedure">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="fecha5" PropertyName="Text" Name="FechaInicio" Type="DateTime"></asp:ControlParameter>
                                                            <asp:ControlParameter ControlID="fecha6" PropertyName="Text" Name="FechaFin" Type="DateTime"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </div>

                                            </div>

                                            <div class="col-5">
                                                <canvas id="grafica" width="400" height="200"></canvas>

                                            </div>

                                            <div class="col-1">
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="ExportarExel">
                                                     <i class="custom-icon2"></i>
                                                </asp:LinkButton>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <nav class="navbar navbar-light bg-light">
                                <div class="container d-flex justify-content-center">
                                    <ul class="nav nav-tabs">
                                        <li class="nav-item">
                                            <a class="nav-link text-dark active" id="Detalle-Visita" data-bs-toggle="tab" href="#DetVis-content">Detalle Visitas</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link text-dark" id="Estadistica-Tipo-Visita" data-bs-toggle="tab" href="#EstTipVis-content">Estadisticas Tipo Visita</a>
                                        </li>

                                    </ul>
                                </div>
                            </nav>





                            <div class="tab-content">
                                <div class="tab-pane fade show active " id="DetVis-content">
                                    <asp:UpdatePanel ID="PanelDetVisitas" runat="server">
                                        <ContentTemplate>
                                            <div class="container mt-4">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-2">
                                                        <div class="row">
                                                            <div class="col-12">
                                                                <div class=" table table-responsive mb-2 gap-2" style="max-height: 15rem; overflow-x: auto;">
                                                                    <h6 class="datagrid-header text-center">Visitas Asesor en las fechas seleccionadas</h6>
                                                                    <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" PageSize="5" AllowSorting="true" ShowHeaderWhenEmpty="true" ID="DataGrid4" runat="server" AutoGenerateColumns="false" DataSourceID="LlenarDetalle">
                                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                        <Columns>


                                                                            <asp:BoundColumn DataField="NombreCompañía" HeaderText="Cliente" />
                                                                            <asp:BoundColumn DataField="NombreContacto" HeaderText="Contacto" ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn DataField="Telefono" HeaderText="Telefono Contacto" ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn DataField="Cotizacion" HeaderText="Cotizacion" ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn DataField="NombreCausa" HeaderText="Causa Por" ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn DataField="FechaVisita" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />

                                                                        </Columns>
                                                                    </asp:DataGrid><asp:SqlDataSource runat="server" ID="LlenarDetalle" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_GetVisitasAsesor" SelectCommandType="StoredProcedure">
                                                                        <SelectParameters>
                                                                            <asp:Parameter Name="Asesor" Type="String" />
                                                                            <asp:Parameter Name="FechaInicio" Type="DateTime" />
                                                                            <asp:Parameter Name="FechaFin" Type="DateTime" />
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

                                <div class="tab-pane fade " id="EstTipVis-content">
                                    <asp:UpdatePanel ID="PanelEstTipoVis" runat="server">
                                        <ContentTemplate>
                                            <div class="container mt-4">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-2">
                                                        <div class="row">
                                                            <div class="col-6">
                                                                <div class=" table-responsive mb-2 gap-2" style="max-height: 13rem; overflow-x: auto;">
                                                                    <h6 class="datagrid-header text-center">Estadisticas Tipo Visitas:</h6>
                                                                    <asp:DataGrid CssClass="table custom-grid table-hover custom-data-grid" ID="DataGrid3" runat="server" AutoGenerateColumns="false">
                                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                        <Columns>
                                                                            <asp:BoundColumn DataField="Id_Causa" HeaderText="Codigo " ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn DataField="NombreCausa" HeaderText="Tipo Visita" ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn DataField="CuentaDeCausa" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                                            <asp:BoundColumn DataField="Porcentaje" HeaderText="% Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                                        </Columns>
                                                                    </asp:DataGrid>

                                                                </div>

                                                            </div>

                                                            <div class="col-5">
                                                                <canvas id="grafica1" width="400" height="200"></canvas>

                                                            </div>

                                                            <div class="col-1">
                                                                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="ExportarExel3">
                                                                             <i class="custom-icon2"></i>
                                                                </asp:LinkButton>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>

                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

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

        // Asignar las fechas a los campos  visitas entre y fecha 
        document.getElementById("fecha").value = fechaFormateada;
        document.getElementById("fecha1").value = primerDiaMesFormateado;
        document.getElementById("fecha2").value = fechaFormateada;
        document.getElementById("fecha5").value = primerDiaMesFormateado;
        document.getElementById("fecha6").value = fechaFormateada;


        // Habilitar enlace Nueva Visita 
        document.getElementById("NuevaVisita").classList.add("enabled");

        window.onload = function () {
            // Deshabilitar el botón en el cliente
            document.getElementById("btnCliente").disabled = true;
        };


        // Ocultar el div con clase "contenedor-icono" cuando se activa la pestaña "Info-content" 
        $(document).ready(function () {
            $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var targetTab = $(e.target).attr("href");
                if (targetTab === "#Estadisticas-content") {
                    $(".contenedor-icono").hide();
                } else {
                    $(".contenedor-icono").show();
                }
            });
        });





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


                //Habilitar TextBox Cotizacion y TextArea Observaciones y Boton Cliente y deshabilitar BtnConsultar


                //Habilitar
                var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
                boton1.disabled = false;

                var ddlVisita = document.getElementById("<%= ddlVisitasPor.ClientID %>");
                ddlVisita.disabled = false;

                var fechaAct = document.getElementById("<%= fecha.ClientID %>");
                fechaAct.disabled = false;

                var cotizacion = document.getElementById("<%= tbCotizacion.ClientID %>");
                cotizacion.disabled = false;

                document.getElementById("txObs").disabled = false;

                //Deshabilitar 
                var boton2 = document.getElementById("<%= btnConsultar.ClientID %>");
                boton2.disabled = true;

                var fecha1 = document.getElementById("<%= fecha1.ClientID %>");
                fecha1.disabled = true;

                var fecha2 = document.getElementById("<%= fecha2.ClientID %>");
                fecha2.disabled = true;

            }

        }

        function CancelarVisita() {
            // Habilitar enlaces
            document.getElementById("NuevaVisita").classList.add("enabled");

            // Deshabilitar enlaces
            document.getElementById("GrabarVisita").classList.remove("enabled");
            document.getElementById("CancelarVisita").classList.remove("enabled");


            //DesHabilitar
            var boton1 = document.getElementById("<%= btnCliente.ClientID %>");
            boton1.disabled = true;

            var ddlVisita = document.getElementById("<%= ddlVisitasPor.ClientID %>");
            ddlVisita.disabled = true;

            var fechaAct = document.getElementById("<%= fecha.ClientID %>");
            fechaAct.disabled = true;

            var cotizacion = document.getElementById("<%= tbCotizacion.ClientID %>");
            cotizacion.disabled = true;

            var observacion = document.getElementById("<%= txObs.ClientID %>");
            observacion.disabled = true;

            //Habilitar 
            var boton2 = document.getElementById("<%= btnConsultar.ClientID %>");
            boton2.disabled = false;

            var fecha1 = document.getElementById("<%= fecha1.ClientID %>");
            fecha1.disabled = false;

            var fecha2 = document.getElementById("<%= fecha2.ClientID %>");
            fecha2.disabled = false;


        }

        function ModificarVisita() {
            // Habilitar enlaces 
            document.getElementById("GrabarVisita").classList.add("enabled");
            document.getElementById("CancelarVisita").classList.add("enabled");
            // Deshabilitar enlaces
            document.getElementById("NuevaVisita").classList.remove("enabled");
            document.getElementById("ModificarVisita").classList.remove("enabled");
            document.getElementById("Exportar").classList.add("disabled");


            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {
                if (dropDownLists[j].id !== "ddlAsesor") {
                    dropDownLists[j].disabled = !dropDownLists[j].disabled;
                }
            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {
                textAreas[k].disabled = !textAreas[k].disabled;
            }

            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbCliente" && textBoxes[i].id !== "tbTelefono" && textBoxes[i].id !== "tbContacto" && textBoxes[i].id !== "tbMailCont") {

                    if (textBoxes[i].id !== "tbLicitacion1") {
                        textBoxes[i].disabled = !textBoxes[i].disabled;
                    }

                }
            }

        }

        function Excel() {

            //habilitar link de Exportar Excel
            if (validarDropDownList())
            {
                document.getElementById("Exportar").classList.remove("disabled");
                document.getElementById("Exportar").classList.add("enabled");
            }
         

        
        }

        function DeshabilitarExcel() {

            //habilitar link de Exportar Excel
            document.getElementById("Exportar").classList.add("disabled");
            document.getElementById("Exportar").classList.remove("enabled");

        
        }

        function abrirOtraPestana() {
            // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
            window.open('Clientes.aspx', '_blank');
        }




    </script>

    <script type="text/javascript">

        // Función para capturar el clic en una fila del DataGrid
        function seleccionarFila(rowIndex) {
            // Obtener el DataGrid y la fila seleccionada utilizando jQuery
            var dataGrid = $("#<%= DataGrid1.ClientID %>");
            var row = dataGrid.find("tr:eq(" + (parseInt(rowIndex) + 1) + ")");

            // Verificar si se encontró la fila
            if (row.length > 0) {
                // Obtener los datos de las celdas de la fila seleccionada
                var cliente = row.find("td:eq(0)").text();
                var contacto = row.find("td:eq(1)").text();
                var telefono = row.find("td:eq(2)").text();
                var mail = row.find("td:eq(3)").text();
                var visitaPor = row.find("td:eq(4)").text();
                var FechaX = row.find("td:eq(5)").text();
                var cotizacion = row.find("td:eq(6)").text();
                var observacion = row.find("td:eq(7)").text();



                // Llenar los TextBox con los datos obtenidos
                $("#<%= tbCliente.ClientID %>").val(cliente);
                $("#<%= tbContacto.ClientID %>").val(contacto);
                $("#<%= tbTelefono.ClientID %>").val(telefono);
                $("#<%= tbMailCont.ClientID %>").val(mail);
                $("#<%= tbCotizacion.ClientID %>").val(cotizacion);                $("#<%= txObs.ClientID %>").val(observacion);


                // Seleccionar el elemento correcto del DropDownList por su valor
                $("#<%= ddlVisitasPor.ClientID %> option").filter(function () {
                    return $(this).text() === visitaPor;
                }).prop("selected", true);

                document.getElementById("ModificarVisita").classList.add("enabled");

            }
        }


    </script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
