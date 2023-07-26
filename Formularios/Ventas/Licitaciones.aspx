<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Licitaciones.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Licitaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/Licitaciones.css" />
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
                        <a class="nav-link text-dark active" id="Detalle-tab" data-bs-toggle="tab" href="#Detalle-content">Detalle Licitaciones</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Info-tab" data-bs-toggle="tab" href="#Info-content">Infor. General Licitaciones</a>
                    </li>
                </ul>
            </div>
        </nav>
    </div>

    <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
        <div class="container-fluid">

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="ejemplo2">
                <ul class="navbar-nav mx-auto contenedor-icono">



                    <div class="contenedor-icono">



                        <%--Comienza Nueva OT--%>


                        <a class="icong disabled " href="#" title="Nueva Licitacion" id="NuevaLic"   onclick="NuevaLic()"  >
                            <i class="bi bi-file-earmark"></i>
                        </a>

                        <a class="icong disabled" href="#" title="Grabar Licitacion" id="GrabarLic">
                            <i class="bi bi-save2"></i>
                        </a>

                        <a class="icong disabled" href="#" title="Modificar Licitacion" id="ModificarLic">
                            <i class="bi bi-wrench"></i>
                        </a>
                        <a class="icong disabled Cancelar" href="#" title="Cancelar" id="CancelarLic" onclick="Cancelar() ">
                            <i class="bi bi-x-lg"></i>
                        </a>



                        <ul />
                </ul>
            </div>

        </div>
    </nav>





 








        <div class="tab-content">

            <div class="tab-pane fade show active" id="Detalle-content">

                <div class="container p-1">


                    <div class="superior">
                        <div class="row pb-1">


                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbAsesor">Asesor</label>
                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:DropDownList class="form-control" ID="ddlAsesor" runat="server" disabled="disabled"></asp:DropDownList>

                                </div>
                            </div>

                            <div class="col-4">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <asp:TextBox type="text" ID="tbCampoBlanco" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="lbContacto1">Contacto</label>
                                    <asp:TextBox type="text" ID="tbContacto1" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>


                        </div>

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbTelefono1">Telefono</label>
                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:TextBox type="text" ID="tbtelefono" runat="server" class="form-control" disabled="false"></asp:TextBox>


                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbCelular">Celular</label>
                                    <asp:TextBox type="text" ID="tbCelular" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-4">
                                <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="lbMail">Mail</label>
                                    <asp:TextBox type="text" ID="tbMail" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>


                        </div>

                    </div>

                    <div class="Media-Alta">

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="Label1">Licitación</label>
                                </div>
                            </div>

                            <div class="col-2">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <asp:TextBox type="text" ID="tbLicitacion" runat="server" class="form-control" disabled="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-2">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbId">ID</label>
                                    <asp:TextBox type="text" ID="tbID" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="lbRegistro">F. Registro</label>
                                    <asp:TextBox type="date" ID="tbFRegistro" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>
                            <div class="col-3">
                                <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="lbApertura">Apertura</label>
                                    <asp:TextBox type="date" ID="tbApertura" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>


                        </div>

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" text="" runat="server" id="lbEstado">Estado</label>
                                </div>
                            </div>

                            <div class="col-2">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <asp:DropDownList class="form-control" ID="ddlEstado" runat="server" disabled="false"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-2">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbClausura">Clausura</label>
                                    <asp:TextBox type="date" ID="tbClausura" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="lbAdendas">Adendas</label>
                                    <asp:TextBox type="date" ID="tbAdendas" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>
                            <div class="col-3">
                                <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="lbObs">Obs.</label>
                                    <asp:TextBox type="date" ID="tbObs" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>


                        </div>

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbProceso">Proceso</label>
                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:DropDownList class="form-control" ID="ddlProceso" runat="server" disabled="false"></asp:DropDownList>

                                </div>
                            </div>

                            <div class="col-4">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbValor">Valor</label>
                                    <asp:TextBox type="text" ID="tbValor" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                    <label class="form-label" runat="server" id="lbPresupuesto">Presupuesto</label>
                                    <asp:TextBox type="text" ID="tbPresupuesto" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>


                        </div>
                    </div>

                    <div class="Media">

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbDirEntrega">Dir Entrega</label>
                                </div>
                            </div>

                            <div class="col-5">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:TextBox type="text" ID="tbDirEntrega" runat="server" class="form-control" disabled="false"></asp:TextBox>


                                </div>
                            </div>



                            <div class="col-5">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" text="" runat="server" id="Label2">Ciudad</label>
                                    <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" disabled="false"></asp:DropDownList>

                                </div>
                            </div>


                        </div>

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbLink">Link</label>
                                </div>
                            </div>

                            <div class="col-10">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:TextBox type="text" ID="tbLink" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>

                        </div>

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbProyecto">Proyecto</label>
                                </div>
                            </div>

                            <div class="col-10">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:TextBox type="text" ID="tbProyecto" runat="server" class="form-control" disabled="false"></asp:TextBox>

                                </div>
                            </div>

                        </div>

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbDirProyecto">Dir Proyecto</label>
                                </div>
                            </div>

                            <div class="col-5">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:TextBox type="text" ID="tbDirProyecto" runat="server" class="form-control" disabled="false"></asp:TextBox>


                                </div>
                            </div>
                            <div class="col-5">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" text="" runat="server" id="Label3">Ciudad</label>
                                    <asp:DropDownList class="form-control" ID="ddlCiudadP" runat="server" disabled="false"></asp:DropDownList>

                                </div>
                            </div>


                        </div>
                    </div>

                    <div class="Baja">

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbObsGen">
                                        Observación<br />
                                        General</label>
                                </div>
                            </div>

                            <div class="col-10">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <textarea class="form-control" id="tbObsGen" runat="server" disabled="disabled"></textarea>


                                </div>
                            </div>


                        </div>

                        <div class="row pb-1">

                            <div class="col-1">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">
                                    <label class="form-label" runat="server" id="lbCausa">Causa</label>
                                </div>
                            </div>

                            <div class="col-3">
                                <div class="input-group input-group-sm  mb-2 gap-2 ">

                                    <asp:DropDownList class="form-control" ID="ddlCausa" runat="server" disabled="false"></asp:DropDownList>


                                </div>
                            </div>


                        </div>

                    </div>


                </div>


            </div>

            <div class="tab-pane fade " id="Info-content">

                <div class="container p-1">

                    <h1>Info Licitaciones </h1>

                </div>


            </div>

        </div>



    </form>

    <script>

        // Habilitar enlaces
        document.getElementById("NuevaLic").classList.add("enabled");

        function NuevaLic() {
            // Deshabilitar enlaces
            document.getElementById("NuevaLic").classList.remove("enabled");


            // Habilitar enlaces
            document.getElementById("GrabarLic").classList.add("enabled");
            document.getElementById("CancelarLic").classList.add("enabled");


            // Habilitar o deshabilitar los TextBox
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].disabled = !textBoxes[i].disabled;
            }

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {
                dropDownLists[j].disabled = !dropDownLists[j].disabled;
            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {
                textAreas[k].disabled = !textAreas[k].disabled;
            }


            $.ajax({
                type: "POST",
                url: "Licitaciones.aspx/Ejemplo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    // 'data' contendrá la respuesta del servidor (la lista de nombres)
                    // Utiliza 'data' para llenar el DropDownList
                    var ddlAsesor = document.getElementById("<%= ddlAsesor.ClientID %>");
                     ddlAsesor.options.length = 0; // Limpiar opciones existentes

                     // Agregar opción inicial
                     var option = document.createElement("option");
                     option.value = "0";
                     option.text = "-- Seleccione --";
                     ddlAsesor.appendChild(option);

                     // Agregar opciones recibidas desde el servidor
                     for (var i = 0; i < data.d.length; i++) {
                         var option = document.createElement("option");
                         option.value = data.d[i].Cedula; // Valor de la opción
                         option.text = data.d[i].NombreCompleto; // Texto a mostrar
                         ddlAsesor.appendChild(option);
                     }
                 },
                 error: function (error) {
                     console.log("Error al llamar al servicio web: " + error);
                 }
             });



       


        }

        function Cancelar() {
            // Deshabilitar enlaces
            document.getElementById("GrabarLic").classList.remove("enabled");
            document.getElementById("CancelarLic").classList.remove("enabled");


            // Habilitar enlaces
            document.getElementById("NuevaLic").classList.add("enabled");

            // Habilitar o deshabilitar los TextBox
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {
                textBoxes[i].disabled = true;;
            }

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {
                dropDownLists[j].disabled = true;
            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {
                textAreas[k].disabled = true;
            }
        
        }



    </script>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
