<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Plano.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Insumos" %>

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


    <div class="container insumos">
        <div class=" container-fluid">
            <table class="table table-bordered border-secondary">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">First</th>
                        <th scope="col">Last</th>
                        <th scope="col">Handle</th>
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
                        <td>Mark</td>
                        <td>Otto</td>
                        <td>@mdo</td>
                    </tr>
                    <tr>
                        <th scope="row">2</th>
                        <td>Jacob</td>
                        <td>Thornton</td>
                        <td>@fat</td>
                        <td>Mark</td>
                        <td>Otto</td>
                        <td>@mdo</td>
                    </tr>
                    <tr>
                        <th scope="row">3</th>
                        <td >Larry the Bird</td>
                        <td>@twitter</td>
                        <td>@twitter</td>
                        <td>Mark</td>
                        <td>Otto</td>
                        <td>@mdo</td>
                    </tr>
                     <tr>
                        <th scope="row">3</th>
                        <td >Larry the Bird</td>
                        <td>@twitter</td>
                        <td>@twitter</td>
                         <td>Mark</td>
                        <td>Otto</td>
                        <td>@mdo</td>
                    </tr>
                     <tr>
                        <th scope="row">3</th>
                        <td >Larry the Bird</td>
                        <td>@twitter</td>
                        <td>@twitter</td>
                         <td>Mark</td>
                        <td>Otto</td>
                        <td>@mdo</td>
                    </tr>
                     <tr>
                        <th scope="row">3</th>
                        <td >Larry the Bird</td>
                        <td>@twitter</td>
                        <td>@twitter</td>
                         <td>Mark</td>
                        <td>Otto</td>
                        <td>@mdo</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>



