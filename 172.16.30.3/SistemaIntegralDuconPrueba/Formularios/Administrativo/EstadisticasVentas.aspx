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
                        <div class="container p-1">
                            <h1>Tipo Pedido</h1>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <div class="tab-pane fade " id="Xmeses-content">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="container ">
                            <h1>X meses</h1>
                        </div>
                    </ContentTemplate>
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




    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
