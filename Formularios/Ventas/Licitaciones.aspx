<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Licitaciones.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Licitaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>            
              <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <span class="navbar-brand mb-0 h1">Licitaciones</span>
            </div>
        </nav>

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="Detalle-tab" data-bs-toggle="tab" href="#Gestion-content">Detalle Licitaciones</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Info-tab" data-bs-toggle="tab" href="#Seguimiento-content">Infor. General Licitaciones</a>
                    </li>
                </ul>
            </div>
        </nav>
            </div>
    </form>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
