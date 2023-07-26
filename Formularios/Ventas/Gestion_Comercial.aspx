<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gestion_Comercial.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Gestion_Comercial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/Ventas/Gestion_Comercial.css" rel="stylesheet" />


    <title>Gestion Comercial</title>
</head>
<body>


    <form id="form1" runat="server">

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <span class="navbar-brand mb-0 h1">Gestion Comercial</span>
            </div>
        </nav>

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="Gestion-tab" data-bs-toggle="tab" href="#Gestion-content">Gestion de Clientes</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Seguimiento-tab" data-bs-toggle="tab" href="#Seguimiento-content">Seguimiento Cliente</a>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="tab-content container">

            <%--TAB GESTION DE CLIENTES--%>

            <div class="tab-pane fade show active" id="Gestion-content">

                <h6 class="mt-2">Clientes</h6>
                <div class="row">
                    <div class="col-10 border-rectangle">

                        <%-- AQUI VA EL DATAGRID #1--%>
                    </div>

                    <div class="col-2 d-flex flex-column" style="margin-top: 10rem;">
                        <asp:Button runat="server" Text="Consultar" CssClass="btn-outline-dark btn btn-sm btn-light mb-2" />
                        <asp:Button runat="server" Text="Tomar Cliente" CssClass="btn-outline-dark btn btn-sm btn-light mb-2" />
                        <asp:Button runat="server" Text="Asignados" CssClass="btn-outline-dark btn btn-sm btn-light mb-2" />
                        <asp:Button runat="server" Text="Historial" CssClass="btn-outline-dark btn btn-sm btn-light mb-2" />



                    </div>

                </div>




                <h6 class="mt-2">Mis Clientes</h6>
                <div class="row">
                    <div class="col-10 border-rectangle">

                        <%--AQUI VA EL DATAGRID #2--%>
                    </div>
                    <div class="col-2 d-flex flex-column">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <div class="input-group bg-success" style="width: 20px; height: 20px; border: 1px solid#808080"></div>
                            <label for="asignado" class="form-label">Asignado</label>
                        </div>

                        <div class="input-group input-group-sm mb-2 gap-2">
                            <div class="input-group bg-white" style="width: 20px; height: 20px; border: 1px solid#808080"></div>
                            <label for="asignado" class="form-label">No Asignado</label>
                        </div>


                        <div class="input-group input-group-sm mb-2 gap-2">
                            <div class="input-group bg-warning" style="width: 20px; height: 20px; border: 1px solid#808080"></div>
                            <label for="asignado" class="form-label">A Tiempo</label>
                        </div>

                        <div class="input-group input-group-sm mb-2 gap-2">
                            <div class="input-group bg-danger" style="width: 20px; height: 20px; border: 1px solid#808080"></div>
                            <label for="asignado" class="form-label">Atrasado</label>
                        </div>
                    </div>
                </div>


            </div>

           <%-- TAB SEGUIMIENTO CLIENTE--%>

            <div class="tab-pane fade" id="Seguimiento-content">


                <div class="container mt-4">
                    <div class="row justify-content-center">
                        <div class="border rounded p-3">

                            <h6>Informacion Cliente</h6>

                            <div class="row">
                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" class="col-form-label-sm">NIT</asp:Label>
                                        <asp:TextBox CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" class="col-form-label-sm">Cliente</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" class="col-form-label-sm">Contacto</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="col-form-label-sm">Telefono</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="col-form-label-sm">Correo</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="col-form-label-sm">Asesor</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="col-form-label-sm">Fecha Creacion</asp:Label>
                                        <asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label runat="server" CssClass="col-form-label-sm">Fecha Cumpleaños</asp:Label>
                                        <asp:TextBox runat="server" Type="date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-2">
                                    <asp:Button runat="server" Text="Cliente" CssClass="btn-outline-dark  btn btn-light" />
                                </div>

                            </div>

                        <%--</div>--%>
                    </div>
                </div>




                <div class="container mt-4">
                    <div class="row justify-content-center">
                        <div class="border rounded p-3">

                            <div class="row">
                                <h6 id="titulo">Seguimiento</h6>
                            </div>

                            <div class="row">
                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2" runat="server" id="elementoCambiar">
                                        <asp:Label runat="server" CssClass="col-form-label-sm">Fecha Programada</asp:Label>
                                        <asp:TextBox runat="server" type="date" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label ID="lblRecibeLlamada" runat="server" CssClass="col-form-label-sm">Recibe Llamada</asp:Label>
                                        <asp:TextBox ID="TextRecibeLlamada" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm mb-2 gap-2">
                                        <asp:Label ID="Label1" runat="server" CssClass="col-form-label-sm">Proximo Seguimiento</asp:Label>
                                        <asp:TextBox ID="TextBox1" type="date" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <h6>Observacion</h6>
                            </div>


                            <div class="row">
                                <div class="col-10">
                                    <textarea runat="server" class="form-control"></textarea>
                                </div>
                                <div class="col-2 d-flex flex-column">
                                    <button runat="server" type="button" class="btn-outline-dark btn btn-sm btn-light mb-2">Grabar Seguimiento</button>
                                    <button type="button" class="btn-outline-dark btn btn-sm btn-light" runat="server" onclick="cambiarContenido()">Historial</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="container mt-4">
                    <div class="row justify-content-center">
                        <div class="border rounded p-3">
                            <h6 class="row justify-content-center">Cotizaciones</h6>

                             <div class="row">
                    <div class="col-10 border-rectangle">

                        <%-- AQUI VA EL DATAGRID #1--%>
                    </div>

                    <div class="col-2 d-flex flex-column mb-2 gap-2">
                        <asp:Label runat="server" CssClass="form-label-sm" ID="LblEstado">Estado</asp:Label>
                        <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="DropUsuario"></asp:DropDownList>

                        <asp:Label runat="server" CssClass="form-label-sm" ID="Label2">Causa</asp:Label>
                         <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="DropDownList1"></asp:DropDownList>

                        <asp:Label runat="server" CssClass="form-label-sm" ID="Label3">Competencia</asp:Label>
                         <asp:DropDownList runat="server" CssClass="form-control form-control-sm" ID="DropDownList2"></asp:DropDownList>

                           <asp:Button runat="server" Text="Cambiar Estado" CssClass="btn-outline-dark btn btn-sm btn-light" />
                    </div>


                </div>

                           </div>
                        </div>
                    </div>

                    </div>
                </div>
            </div>

       <script>
           // Variable global para controlar el estado del contenido
           var contenidoCambiado = false;

           function cambiarTitulo() {
               // Obtiene el elemento h6 con el id "titulo"
               var tituloElement = document.getElementById("titulo");

               // Cambia el contenido del h6 a "Historial" o "Seguimiento" según el estado
               tituloElement.innerHTML = contenidoCambiado ? "Seguimiento" : "Historial";
           }

           function cambiarContenido() {
               // Cambia el estado de la variable
               contenidoCambiado = !contenidoCambiado;

               // Llama a la función para cambiar el título
               cambiarTitulo();

               // Obtiene el elemento div que deseas cambiar
               var elementoCambiar = document.getElementById("elementoCambiar");

               // Si el contenido ha sido cambiado, restaura los elementos originales
               if (contenidoCambiado) {
                   elementoCambiar.innerHTML =
                       '<asp:Label runat="server" CssClass="col-form-label-sm">Resposable</asp:Label><asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>';
                   '<asp:Label runat="server" CssClass="col-form-label-sm">Resposable</asp:Label><asp:TextBox runat="server" CssClass="form-control"></asp:TextBox>'
               } else {
                   // Si el contenido no ha sido cambiado, muestra el contenido original
                   elementoCambiar.innerHTML =
                       '<asp:Label runat="server" CssClass="col-form-label-sm">Fecha Programada</asp:Label><asp:TextBox runat="server" type="date" CssClass="form-control"></asp:TextBox>';
               }
           }
       </script>


    </form>
 

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
