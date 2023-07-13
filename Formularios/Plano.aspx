<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Plano.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Plano" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />


    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/Plano.css" rel="stylesheet" />
    <title>Plano</title>
</head>
<body>



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
                        <a class="nav-link active" aria-current="page" href="modulo.aspx">modulo</a>
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
                <ul class="navbar-nav mx-auto">



                    <div class="contenedor-icono">



                        <%--Comienza Nueva OT--%>








                        <%--Termina Nueva OT--%>

                        <a class="text-dark" href="#" title="Adicionar Objeto al Plano">
                            <i class="ib bi-pc"></i>
                        </a>
                        <a class="text-dark" href="#" title="Quitar Objeto del Plano">
                            <i class="bi bi-database-check"></i>
                        </a>
                        <a class="text-dark" href="#" title="Eliminar Objetos del Plano">
                            <i class="bi bi-fire"></i>
                        </a>
                        <a class="text-dark" href="#" title="Acabados del Plano">

                            <i class="bi bi-bar-chart-line"></i>
                        </a>
                        <a class="text-dark" href="#" title="Leer Archivo Despiece Acad">

                            <i class="bi bi-border-inner"></i>
                        </a>
                        <a class="text-dark" href="#" title="Cargar Archivo TXT XY">

                            <i class="bi bi-folder-plus"></i>
                        </a>
                        <a class="text-dark" href="#" title="Plano Bloqueado">

                            <i class="bi bi-lock"></i>
                        </a>
                        <a class="text-dark" href="#" title="Crear o Redefinir Bolsa">

                            <i class="bi bi-bag-check"></i>
                        </a>
                        <a class="text-dark" href="#" title="Adicionar/Remover Elementos de la Bolsa">

                            <i class="bi bi-bag-plus"></i>
                        </a>
                        <a class="text-dark" href="#" title="Despiece del Plano">

                            <i class="bi bi-disc-fill"></i>
                        </a>
                        <a class="text-dark" href="#" title="Generar  TXT">

                            <i class="bi bi-filetype-txt"></i>
                        </a>
                        <a class="text-dark" href="#" title="Guardar TXT">

                            <i class="bi bi-save2"></i>
                        </a>

                        <a class="text-dark" href="#" title="Exportar Plano u Orden de Trabajo">

                            <i class="bi bi-arrow-up-left-circle"></i>
                        </a>
                        <a class="text-dark" href="#" title="Visualizar/Generar Cotizacion">

                            <i class="bi bi-bag-plus"></i>
                        </a>
                        <a class="text-dark" href="#" title="Objetos no Existentes">

                            <i class="bi bi-text-indent-left"></i>
                        </a>
                        <a class="text-dark" href="#" title="Actualizar Precio Prototipo">

                            <i class="bi bi-cash-coin"></i>
                        </a>
                        <a class="text-dark" href="#" title="Generar Formato Certificado de Origen ">

                            <i class="bi bi-clipboard-check"></i>
                        </a>
                        <a class="text-dark" href="#" title="Importar Plano de Actualizacion de Bloques">

                            <i class="bi bi-file-arrow-down-fill"></i>
                        </a>


                        <ul />
                </ul>
            </div>

        </div>
    </nav>


    <!-- Termina Panel Iconos-->

    <!-- comieza Panel Pricipal de Planos-->

    <form class="control" action="#" runat="server">
        <div class=" container-fluid Plano">

            <div class="container-fluid panel-plano ">
                <div class="  descripcion-plano">
                    <div class="item-plano">
                        <asp:Button ID="btnPlano" type="button" Text="Plano" class="btn btn-outline-secondary"
                            runat="server"></asp:Button>
                        <asp:TextBox ID="txtPlano" type="text" class="form-control  input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblCliente" class="form-label" Text="Cliente" runat="server"></asp:Label>
                        <asp:TextBox ID="txtCliente" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblArea" class="form-label" Text="Área" runat="server"></asp:Label>
                        <asp:TextBox ID="txtArea" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblContacto" class="form-label" Text="Contacto" runat="server">
                        </asp:Label>
                        <asp:TextBox ID="txtContacto" type="text" class="form-control input" runat="server">
                        </asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblAsesor" class="form-label" Text="Asesor" runat="server"></asp:Label>
                        <asp:TextBox ID="txtAsesor" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>
                    <div class="item-plano">
                        <asp:Label ID="lblDibuja" class="form-label" Text="Dibija" runat="server"></asp:Label>
                        <asp:TextBox ID="txtDibuja" type="text" class="form-control input" runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano">
                        <asp:Label ID="lblBolsa" class="form-label" Text="Bolsa" runat="server"></asp:Label>
                        <asp:TextBox ID="txtBolsa" type="text" class="form-control input " runat="server"></asp:TextBox>
                    </div>

                    <div class="item-plano2">
                        <asp:Label fid="lblResumenPlano" class="form-label" Text="Resumen del Plano"
                            runat="server">
                                    Resumen del Plano</asp:Label>
                        <textarea id="txResumen" class="form-control" style="overflow-y: scroll;"
                            runat="server" rows="3"></textarea>

                    </div>


                    <div class="item-plano2">
                        <asp:TextBox ID="cbxImagen" type="checkbox" class="form-check-input" runat="server">
                        </asp:TextBox>
                        <asp:Label ID="lblVerImagen" class="form-check-label" Text="Ver imagen" runat="server">
                        </asp:Label>
                    </div>

                </div>

                <div class=" tabla-plano overflow-auto">

        


                </div>
            </div>

            <div class=" container-fluid panel-tabla  ">
                <div class="  tabla2-plano">

                    <table class="table">
                        <thead>
                            <tr>
                                <th scope="col">Mod</th>
                                <th scope="col">Tipo Modulo</th>
                                <th scope="col">Descripción</th>
                                <th scope="col">OK</th>
                                <th scope="col">Pos</th>
                                <th scope="col">Altura</th>
                                <th scope="col">Cant</th>
                                <th scope="col">Lado</th>
                                <th scope="col">Grupo</th>
                                <th scope="col">Responsable</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>

                                <td>Mark</td>
                                <td>Otto</td>
                                <td>@mdo</td>
                                <td>Mark</td>
                                <td>Otto</td>
                                <td>@mdo</td>
                                <td>Mark</td>
                                <td>Otto</td>
                                <td>@mdo</td>
                                <td>Mark</td>

                            </tr>
                            <tr>

                                <td>Jacob</td>
                                <td>Thornton</td>
                                <td>@fat</td>
                                <td>Mark</td>
                                <td>Otto</td>
                                <td>@mdo</td>
                                <td>Mark</td>
                                <td>Otto</td>
                                <td>@mdo</td>
                                <td>Mark</td>
                            </tr>
                            <tr>
                                <td>Thornton</td>
                                <td>@fat</td>
                                <td>Mark</td>
                                <td>Otto</td>
                                <td>@mdo</td>
                                <td>Mark</td>
                                <td>Otto</td>
                                <td>@mdo</td>
                                <td>Mark</td>
                                <td>@twitter</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="container-fluid footer">

                <div class="item-footer">
                    <asp:Label ID="lblCantidad" class="form-label" Text="Cantidad" runat="server"></asp:Label>
                    <asp:TextBox ID="txtCantidad" type="text" class=" input" runat="server"></asp:TextBox>
                    <asp:Button ID="btnCambiar" type="button" class="btn btn-outline-secondary disabled"
                        Text="Cambiar" runat="server"></asp:Button>
                </div>

                <div class="item-footer1">
                    <asp:Label ID="lblDisp" class="form-label" Text="Dip. LA" runat="server"></asp:Label>
                    <asp:Label ID="lblValor" class="form-label" runat="server">0000</asp:Label>
                    <asp:Label ID="lblTotalObjeto" Text="Total Objetos" runat="server"></asp:Label>
                    <asp:Label ID="lblValor2" class="form-label" runat="server">000</asp:Label>
                </div>

                <div class="item-footer2">
                    <asp:Label ID="lblValorDespiece" class="form-label" Text="Valor despiece" runat="server">
                    </asp:Label>
                    <asp:Label ID="lblValor3" class="form-label" runat="server">222</asp:Label>
                </div>

            </div>

        </div>
    </form>

    <!-- Termina Panel Pricipal de Planos-->


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>



