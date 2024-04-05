<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadisticasVentas.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Administrativo.EstadisticasVentas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Estadisticas Venta</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/Administrativo/EstadisticaVenta.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>


    <script>
        function GenerarGrafica1(nombres1, cantidades1) {
            var ctx1 = document.getElementById("grafica1").getContext('2d');


            var myChart1 = new Chart(ctx1, {
                type: 'bar',
                data: {
                    labels: nombres1,
                    datasets: [{
                        label: 'Estadisticas Venta Por Asesor',
                        data: cantidades1,
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1
                    }]
                },
                options: {

                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Asesores' // Título del eje X
                            },
                            ticks: {
                                font: {
                                    size: 7
                                }
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Ventas' // Título del eje Y
                            },
                            beginAtZero: true
                        }
                    }
                }
            });


        }

        function GenerarGrafica2(nombres1, cantidades1) {
            var ctx1 = document.getElementById("grafica2").getContext('2d');

            // Definir un arreglo de colores
            var colores = [
                'rgba(173, 216, 230, 0.3)',   // Enero: Azul claro
                'rgba(255, 105, 180, 0.3)',   // Febrero: Rosa claro
                'rgba(144, 238, 144, 0.3)',   // Marzo: Verde claro
                'rgba(255, 215, 0, 0.3)',     // Abril: Amarillo claro
                'rgba(152, 251, 152, 0.3)',   // Mayo: Verde pastel
                'rgba(255, 165, 0, 0.3)',     // Junio: Naranja claro
                'rgba(255, 99, 71, 0.3)',     // Julio: Rojo claro
                'rgba(255, 223, 186, 0.3)',   // Agosto: Amarillo claro
                'rgba(210, 105, 30, 0.3)',    // Septiembre: Marrón claro
                'rgba(255, 140, 0, 0.3)',     // Octubre: Naranja oscuro
                'rgba(255, 69, 0, 0.3)',      // Noviembre: Rojo oscuro
                'rgba(255, 255, 224, 0.3)'    // Diciembre: Amarillo pálido
            ];



            // Crear un arreglo de colores para los datos
            var backgroundColors = [];
            for (var i = 0; i < nombres1.length; i++) {
                backgroundColors.push(colores[i % colores.length]); // Usar un color diferente para cada etiqueta de mes
            }

            var myChart1 = new Chart(ctx1, {
                type: 'bar',
                data: {
                    labels: nombres1,
                    datasets: [{
                        label: 'Estadísticas Venta Por Año',
                        data: cantidades1,
                        backgroundColor: backgroundColors, // Asignar los colores
                        borderColor: 'rgba(0, 0, 0, 0.6)', // Cambiar el color del borde a negro
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Meses' // Título del eje X
                            },
                            ticks: {
                                font: {
                                    size: 7
                                }
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Ventas' // Título del eje Y
                            },
                            beginAtZero: true
                        }
                    }
                }
            });
        }


    </script>




</head>
<body>

    <nav class="navbar navbar-light bg-light">
        <div class="container d-flex justify-content-center">
            <ul class="nav nav-tabs">
                <li class="nav-item">
                    <a class="nav-link text-dark active" id="TipoPedido-tab" data-bs-toggle="tab" href="#TipoPedido-content">Tipo Pedido</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" id="Xmeses-tab" data-bs-toggle="tab" href="#Xmeses-content">X Meses</a>
                </li>

                <li class="nav-item">
                    <a class="nav-link text-dark " id="Xtrimestre-tab" data-bs-toggle="tab" href="#Xtrimestre-content">X Trimestre</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" id="Xrangos-tab" data-bs-toggle="tab" href="#Xrangos-content">X Rangos</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark " id="XcuotaMensual-tab" data-bs-toggle="tab" href="#XcuotaMensual-content">X Cuota Mensual</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" id="XcuotaTimensual-tab" data-bs-toggle="tab" href="#XcuotaTimensual-content">X Cuota Trimestral</a>
                </li>
            </ul>
        </div>
    </nav>


    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <div class="tab-content">

            <div class="tab-pane fade show active" id="TipoPedido-content">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class=" container-fluid ">

                            <div class="row pt-2 mt-2">

                                <div class="col-3">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Tipo Pedido" runat="server" ID="lbTipoPedido"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlTipoPedido" runat="server" DataTextField="Descripcion_TipoPedido" DataValueField="Descripcion_TipoPedido" DataSourceID="DSTipoPedido" OnDataBound="ddlTipoPedido_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="DSTipoPedido" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT * FROM tblTipoPedido ORDER BY Descripcion_TipoPedido asc"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="lbEntre" class="form-label" Text="Entre" runat="server"></asp:Label>
                                        <asp:TextBox ID="tbfechaIni" type="date" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label ID="lnY" class="form-label" Text=" Y " runat="server"></asp:Label>
                                        <asp:TextBox ID="tbfechaFin" type="date" runat="server" class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Grupo" runat="server" ID="lbGrupo"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlGrupo" runat="server" DataTextField="Grupo" DataValueField="Grupo" DataSourceID="DSGrupo" OnDataBound="ddlGrupo_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="DSGrupo" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT DISTINCT tblEmpleado.Grupo From tblEmpleado Where (((tblEmpleado.Grupo) Is Not Null)) AND tblEmpleado.Grupo <> '' ORDER BY tblEmpleado.Grupo"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2 justify-content-center ">
                                        <asp:CheckBox ID="chkAprod" runat="server" />
                                        <asp:Label ID="lbApro" runat="server" Text="A producción"></asp:Label>

                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class=" input-group input-group-sm mb-1 justify-content-center">
                                        <asp:Button ID="btnConsultar" CssClass="btn btn-outline-secondary" runat="server" Text="Consultar" OnClick="btnConsultar_Click" OnClientClick="return validarFechas();" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <asp:LinkButton class="icong disabled" runat="server" title="Exportar" ID="ExportarExcel" OnClick="ExportarExcel_Click">
                                         <i class="custom-icon"></i>
                                    </asp:LinkButton>
                                </div>


                            </div>

                            <div class="row justify-content-center p-1 m-1 pb-2" id="EstGen" runat="server">
                                <div class="border rounded">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 16rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-start">Estadistica Venta</h6>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm mt-2 " ID="DatagridEstVentas" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemDataBound="DatagridEstVentas_ItemDataBound" OnItemCommand="DatagridEstVentas_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton CssClass="Tam" ID="lnkView" runat="server" CommandName="VerDetalle" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="Codigo_Asesor" HeaderText="Codigo Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="NombreCompleto" HeaderText="Nombre Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Nombre de la Obra" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Precio_Venta" HeaderText="Precio de Venta" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descuento" HeaderText="Dto (%)" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ValorNeto" HeaderText="Valor Neto" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Confirmacion_Venta" HeaderText="F. de Pedido" ItemStyle-CssClass="auto-width-column" />



                                                        <%-- Campos oscultos pero que se muestran en el formulario empieza en el 13]--%>

                                                        <asp:BoundColumn DataField="" Visible="false" />

                                                    </Columns>
                                                </asp:DataGrid>



                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row justify-content-center p-1 m-1">
                                <div class="border rounded ">
                                    <div class="row ">

                                        <div class="col-5 border" id="EstAsesor" runat="server">
                                            <div class=" table-responsive mb-2 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-center">Estadistica Venta Por Asesor:</h6>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm mt-3" ID="DataGridXAsesor" runat="server" AutoGenerateColumns="false" OnItemDataBound="DataGridXAsesor_ItemDataBound">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="" HeaderText="POS" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Codigo_Asesor" HeaderText="Cedula Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="NombreCompleto" HeaderText="Nombre Asesor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Total" HeaderText="Total Neto" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="" HeaderText="%" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>

                                        </div>

                                        <div class="col-7 border" id="GraAsesor" runat="server">
                                            <h6 class="datagrid-header text-center">Grafico Venta por Asesor (<span id="FechaI" runat="server"></span> - <span id="FechaF" runat="server"></span> )</h6>
                                            <div class="input-group input-group-sm justify-content-end">

                                                <asp:LinkButton class="icong" runat="server" title="Expandir Grafico" ID="Expandir" OnClientClick="expandirDiv(); return false;">
                                                        <i class="bi bi-arrows-fullscreen"></i>
                                                </asp:LinkButton>

                                            </div>

                                            <canvas id="grafica1" width="600" height="200"></canvas>

                                        </div>


                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <asp:SqlDataSource ID="DSEstadisticaVenta" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>">
                                    <SelectParameters>
                                        <asp:Parameter Name="FechaIni" Type="DateTime" />
                                        <asp:Parameter Name="FechaFin" Type="DateTime" />
                                        <asp:Parameter Name="TipoPedido" Type="String" />
                                        <asp:Parameter Name="Aproduccion" Type="String" />
                                        <asp:Parameter Name="Grupo" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>


                                <asp:TextBox ID="ced" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="tbMensaje" runat="server" Visible="false"></asp:TextBox>

                            </div>

                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ExportarExcel" />
                    </Triggers>
                </asp:UpdatePanel>


            </div>

            <div class="tab-pane fade " id="Xmeses-content">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid ">

                            <div class="row pt-2 mt-2 pb-2 mb-2">

                                <div class="col-1"></div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm mb-1 ">
                                        <asp:Label class="form-label" Text="Seleccione año para estadistica" runat="server" ID="lbAnio"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class=" input-group input-group-sm mb-1 ">
                                        <asp:DropDownList CssClass="form-control" ID="ddlAnioBusqueda" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-1"></div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Nit. CLiente " runat="server" ID="lbNit"></asp:Label>
                                        <asp:TextBox ID="tbNit" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label class="form-label" Text="Zona" runat="server" ID="Label1"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlZona" runat="server">
                                            <asp:ListItem Value="%">%</asp:ListItem>
                                            <asp:ListItem Value="01">01</asp:ListItem>
                                            <asp:ListItem Value="02">02</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-1"></div>

                                <div class="col-1">
                                    <div class=" input-group input-group-sm">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="AddAnio" runat="server" Text="Agregar Año" OnClick="AddAnio_Click" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class=" input-group input-group-sm">
                                        <asp:Button CssClass="btn btn-outline-secondary" ID="Limpiar" runat="server" Text="Limpiar" OnClick="Limpiar_Click" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class=" input-group input-group-sm">
                                        <asp:LinkButton class="icong " runat="server" title="Exportar" ID="ExportarExcel2" OnClick="ExportarExcel2_Click">
                                         <i class="custom-icon"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                            </div>

                            <div class="row justify-content-center p-1 m-1 pb-2" runat="server">
                                <div class="border rounded">
                                    <div class="row">

                                        <div class="col-7" id="EstMes" runat="server">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 30rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-center">Intervalo de Mes</h6>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm mt-2 " ID="DataGridEstXMes" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemDataBound="DataGridEstXMes_ItemDataBound">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:BoundColumn DataField="" HeaderText="Mes" ItemStyle-CssClass="auto-width-column" />


                                                    </Columns>
                                                </asp:DataGrid>



                                            </div>
                                        </div>

                                        <div class="col-5 border" id="GraMes" style="height: 28rem;" runat="server">
                                            <h6 class="datagrid-header text-center mb-3">Grafico Ventas Año:  <span id="SpanAño" runat="server"></span></h6>

                                            <div class="input-group input-group-sm justify-content-end">

                                                <asp:LinkButton class="icong" runat="server" title="Expandir Grafico" ID="Expandir1" OnClientClick="expandirDiv1(); return false;">
                                                        <i class="bi bi-arrows-fullscreen"></i>
                                                </asp:LinkButton>

                                            </div>


                                            <canvas id="grafica2" width="350" height="200"></canvas>
                                        </div>

                                    </div>
                                </div>
                            </div>

                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ExportarExcel2" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>


            <div class="tab-pane fade " id="Xtrimestre-content">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div class="container ">
                            <h1>X trimestre</h1>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade " id="Xrangos-content">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div class="container ">
                            <h1>X Rangos</h1>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade " id="XcuotaMensual-content">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="container ">
                            <h1>X Couta Mensual</h1>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade " id="XcuotaTimensual-content">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <div class="container ">
                            <h1>X Cuota Trimestral.</h1>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </form>

    <script type="text/javascript">
        function expandirDiv() {
            var graAsesorDiv = document.getElementById('<%= GraAsesor.ClientID %>');

            if (graAsesorDiv.classList.contains('col-7')) {
                document.getElementById('<%= EstAsesor.ClientID %>').style.display = 'none';
                document.getElementById('<%= EstGen.ClientID %>').style.display = 'none';
                graAsesorDiv.classList.remove('col-7');
                graAsesorDiv.classList.add('col-12');
            } else {
                document.getElementById('<%= EstAsesor.ClientID %>').style.display = 'block';
                document.getElementById('<%= EstGen.ClientID %>').style.display = 'block';
                graAsesorDiv.classList.remove('col-12');
                graAsesorDiv.classList.add('col-7');
            }
        }
    </script>

    <script type="text/javascript">
        function expandirDiv1() {
            var graMesDiv = document.getElementById('<%= GraMes.ClientID %>');

            if (graMesDiv.classList.contains('col-5')) {
                document.getElementById('<%= EstMes.ClientID %>').style.display = 'none';
                graMesDiv.classList.remove('col-5');
                graMesDiv.classList.add('col-10');

            } else {
                document.getElementById('<%= EstMes.ClientID %>').style.display = 'block';
                graMesDiv.classList.remove('col-10');
                graMesDiv.classList.add('col-5');


            }
        }
    </script>

    <script>
        function validarFechas() {

            // Obtenemos los valores de los texxbox
            var fechaInicio = document.getElementById('tbfechaIni').value;
            var fechaFin = document.getElementById('tbfechaFin').value;
            // Obtener la fecha actual
            var fechaActual = new Date();

            // Validar si las fechas están en el rango permitido
            if (!validarRangoFechas(fechaInicio) || !validarRangoFechas(fechaFin)) {

                alert('Por favor ingrese un  fecha válidas');
                return false;
            }
            // Convertir las cadenas de fecha en objetos Date
            var inicio = new Date(fechaInicio);
            var fin = new Date(fechaFin);

            // Verificar si las fechas son válidas
            if (isNaN(inicio.getTime()) || isNaN(fin.getTime())) {
                // Mostrar un mensaje de error si las fechas no son válidas
                alert('Por favor ingrese fechas válidas.');
                return false; // Evitar que se ejecute la acción
            }

            // Verificamos si la fecha de inicio es posterior a la fecha de fin
            if (inicio > fin) {

                alert('La fecha de inicio debe ser anterior a la fecha de fin.');
                return false; // Evitar que se ejecute la acción
            }


            // Verificar si la fecha de fin es posterior a la fecha actual
            if (fin > fechaActual) {
                alert('La fecha Y no puede ser posterior a la fecha actual.');
                return false;
            }
            // Si las fechas son válidas y la fecha de inicio es anterior a la fecha de fin, permitir la acción
            return true;
        }

        // Función para validar si la fecha está en el rango permitido (a partir de 1900)
        function validarRangoFechas(fecha) {
            var year = parseInt(fecha.split("-")[0]);
            return year >= 1900;
        }
    </script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
