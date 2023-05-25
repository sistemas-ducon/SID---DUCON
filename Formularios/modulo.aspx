<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modulo.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Plano" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />


    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/modulo.css" rel="stylesheet" />
    <title>Modulo</title>
</head>


    <body>

        
    <!--Comienza Panel principal de nombres -->
    <nav class="navbar navbar-expand-sm navbar-light bg-light">
        <div class="container">

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
                data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false"
                aria-label="Toggle navigation">
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
    <!-- Termina Panel principal de nombres-->


    <!--Comienza Panel de iconos-->
    <nav class="navbar navbar-expand-sm navbar-light bg-light">
        <div class="container-fluid">

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2"
                aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="ejemplo2">
                <ul class="navbar-nav mx-auto">

                    <div class="contenedor-icono">

                        <!--icons planos-->

                        <i class="bi bi-file-earmark"></i>
                        <i class="bi bi-file-medical"></i>
                        <i class="bi bi-wrench"></i>
                        <i class="bi bi-file-earmark-ruled"></i>
                        <i class="bi bi-database-down"></i>
                        <i class="bi bi-files"></i>
                        <i class="bi bi-check-lg"></i>


                    </div>
            </div>
    </nav>
    <!--Termina Panel de iconos-->


         <div class="container-fluid Modulo">

            <form class="control" action="#" runat="server">

                <div class="container-fluid superior">

                    <div class="item">
                        <asp:Label ID="lblGrupo" Class="form-label " runat="server" Text="Grupo"></asp:Label>
                        <asp:DropDownList ID="DblGrupo" class="form-control grupo" runat="server"></asp:DropDownList>
                    </div>

                    <div class="item">
                         <asp:Label ID="lblCriterio" Class="form-label" runat="server" Text="Criterio"></asp:Label>
                        <asp:TextBox ID="txtCriterio"  class="form-control criterio1" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txtCriterio2"  class="form-control criterio2" runat="server"></asp:TextBox>
                    </div>

                    <div class="item">
                        <asp:Label ID="lblAltura" Class="form-label" runat="server" Text="Altura"></asp:Label>
                        <asp:TextBox ID="txtAltura"  class="form-control altura" runat="server"></asp:TextBox>
                        <asp:Label ID="lblMedida" Class="form-label" runat="server" Text="Cms"></asp:Label>
                    </div>

                    <div class="item">

                        <asp:Button ID="btnBuscarActivos" Class="btn btn-outline-secondary" runat="server" Text="Buscar " />
                    </div>

                </div>

                <div class="container-fluid central">

                     <div class="Title-table">
                            <h4>Titulo tabla </h4>
                        </div>

                    <div class="item">
                        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                           <table class="table table-bordered border-secondary">
                        <thead>
                          <tr>
                            <th scope="col">#</th>
                            <th scope="col">First</th>
                            <th scope="col">Last</th>
                            <th scope="col">Handle</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr>
                            <th scope="row">1</th>
                            <td>Mark</td>
                            <td>Otto</td>
                            <td>@mdo</td>
                          </tr>
                          <tr>
                            <th scope="row">2</th>
                            <td>Jacob</td>
                            <td>Thornton</td>
                            <td>@fat</td>
                          </tr>
                          <tr>
                            <th scope="row">3</th>
                            <td colspan="2">Larry the Bird</td>
                            <td>@twitter</td>
                          </tr>
                        </tbody>
                      </table>

                    </div>

                    <!--Aqui va el gridview central -->

                </div>


                <div class="container-fluid inferior">
                    <!--Aqui va uno div con el grid inferioi-->

                     <div class="Title-table">
                            <h4>Titulo tabla </h4>
                        </div>

                    <div class="item">
                        <asp:GridView ID="GridView2" runat="server"></asp:GridView>

                       

                        <table class="table table-bordered border-secondary ">
                            <thead>
                                <tr>
                                    <th scope="col">#</th>
                                    <th scope="col">First</th>
                                    <th scope="col">Last</th>
                                    <th scope="col">Handle</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <th scope="row">1</th>
                                    <td>Mark</td>
                                    <td>Otto</td>
                                    <td>@mdo</td>
                                </tr>
                                <tr>
                                    <th scope="row">2</th>
                                    <td>Jacob</td>
                                    <td>Thornton</td>
                                    <td>@fat</td>
                                </tr>
                                <tr>
                                    <th scope="row">3</th>
                                    <td colspan="2">Larry the Bird</td>
                                    <td>@twitter</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

              

            </form>


        </div> <!--div principal objeto -->



        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
       
        
    </body>
    

</html>



