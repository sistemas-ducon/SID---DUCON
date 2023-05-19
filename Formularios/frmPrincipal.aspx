﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPrincipal.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.OrdenesDeTrabajo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/frmPrincipal.css" rel="stylesheet" />
    <title>Ordenes de trabajo</title>
</head>
<body>
    <div class="wrapper">

        <%--Comienza Panel principal de nombres--%>


        <nav class="navbar navbar-expand-sm navbar-light bg-light">
            <div class="container">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">



                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="#">Ordenes de trabajo</a>
                        </li>
                    </ul>


                    <ul class="navbar-nav me-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="Plano.aspx">Plano</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav mx-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="#">Objetos</a>
                        </li>
                    </ul>

                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="#">Modulos</a>
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



                                <%--Comienza Nueva OT--%>

                                <i data-bs-toggle="modal" data-bs-target="#exampleModal" class="bi bi-file-earmark"></i>




                                <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" data-bs-backdrop="static">
                                    <div class="modal-dialog modal-dialog modal-fullscreen">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">Nueva OT</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                            </div>
                                            <div class="modal-body">
                                                </div>
                                            </div>
                                        </div>
                                    </div>





                                <%--Termina Nueva OT--%>



                                <i class="bi bi-files"></i>
                                <i class="bi bi-file-earmark-text"></i>
                                <i class="bi bi-wrench"></i>
                                <i class="bi bi-file-earmark-excel"></i>
                                <i class="bi bi-pen"></i>
                                <i class="bi bi-eye"></i>
                                <i class="bi bi-printer"></i>
                                <i class="bi bi-coin"></i>
                                <i class="bi bi-x-lg"></i>
                                <i class="bi bi-sunglasses"></i>
                                <i class="bi bi-check-square"></i>
                                <i class="bi bi-person-lines-fill"></i>
                                <i class="bi bi-house-up"></i>
                                <i class="bi bi-sign-stop"></i>
                                <i class="bi bi-hammer"></i>
                                <i class="bi bi-lightning-charge"></i>
                                <i class="bi bi-key"></i>
                                <i class="bi bi-code-square"></i>
                                <i class="bi bi-box-arrow-up"></i>
                                <i class="bi bi-x-square"></i>

                                <ul />
                        </ul>
                    </div>

                </div>
            </nav>
    </div>




    <%--Termina Panel de iconos--%>






    <%--Comienza Formulario Principal--%>


    <form class="frmPrincipal" runat="server">

    <div class="container-fluid">

        <div class="row">


            <div class="col-1">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label class="form-label" Text="OT" runat="server" ID="lblOT"></asp:Label>
                    <asp:TextBox ID="tbOT" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>


            <div class="col-1">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label class="form-label" Text="Pedido" runat="server" ID="lblPedido"></asp:Label>
                    <asp:TextBox ID="tbPedido" type="number" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-1">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label ID="LblZona" Text="Zona" CssClass="form-label" runat="server"></asp:Label>
                    <asp:TextBox ID="tbZona" type="text" class="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="col-3">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label class="form-label" Text="T.Ped" runat="server" ID="lblTped"></asp:Label>
                    <asp:TextBox ID="tbTped" type="text" class="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="col-1">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label class="form-label" Text="Ped.Base" runat="server" ID="lblPedBase"></asp:Label>
                    <asp:TextBox ID="tbPedBase" type="number" class="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="col-1">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label class="form-label" Text="Ped.Depen" runat="server" ID="lblPedDepen"></asp:Label>
                    <asp:TextBox ID="tbPedDepen" type="number" class="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="col-3">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Label class="form-label" Text="Aprob" runat="server" ID="lblAprob"></asp:Label>
                    <asp:TextBox ID="tbAprob" type="text" class="form-control" runat="server"></asp:TextBox>
                </div>
            </div>


            <div class="col-1">
                <div class="input-group input-group-sm mb-2 gap-2">
                    <asp:Button ID="btnNuevoPedido" runat="server" class="bi bi-file-earmark btn btn-secondary"></asp:Button>
                    <asp:Button ID="btnAcabados" runat="server" class="btn btn-secondary"></asp:Button>
                    <asp:Button ID="btnOk" runat="server" type="button" Text="OK" class="btn btn-secondary"></asp:Button>
                </div>
            </div>

            <div class="row">

                <div class="col-3">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Obra" runat="server" ID="lblObra"></asp:Label>
                        <asp:TextBox ID="tbObra" type="text" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-3">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Dir" runat="server" ID="lblDir"></asp:Label>
                        <asp:TextBox ID="tbDir" type="text" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-3">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Contac" runat="server" ID="lblContac"></asp:Label>
                        <asp:TextBox ID="tbContac" type="text" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="col-3">
                    <div class="input-group input-group-sm mb-2 gap-2">
                        <asp:Label class="form-label" Text="Email" runat="server" ID="lblEmail"></asp:Label>
                        <asp:TextBox ID="tbEmail" type="email" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="row">


                    <div class="col-3">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:Label class="form-label" Text="Recibe" runat="server" ID="lblRecibe"></asp:Label>
                            <asp:TextBox ID="tbRecibe" type="text" class="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-3">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:Label class="form-label" Text="Ciudad" runat="server" ID="lblCiudad"></asp:Label>
                            <asp:TextBox ID="tbCiudad" type="text" class="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-2">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:Label class="form-label" Text="Tel" runat="server" ID="lblTel"></asp:Label>
                            <asp:TextBox ID="tbTel" type="tel" class="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-1">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:Label class="form-label" Text="Cel" runat="server" ID="lblCel"></asp:Label>
                            <asp:TextBox ID="tbCel" type="tel" class="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>


                    <div class="col-2">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:Label class="form-label" Text="Pais" runat="server" ID="lblPais"></asp:Label>
                            <asp:TextBox ID="tbPais" type="text" class="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-1">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:Label class="form-label" Text="H.Total" runat="server" ID="lblHTotal"></asp:Label>
                            <asp:TextBox ID="tbHTotal" type="number" class="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>



            </div>
        </div>
    </div>















    <%--Pantalla intermedia--%>




    <div class="container-fluid">
        <div class="observaciones">

            <div class="div-1">


                <div class="row">
                    <div class="mb-2 gap-2">
                        <textarea id="Observacion5Id" runat="server" class="form-control form-control-ms"></textarea>
                    </div>
                </div>
            </div>





            <div class="div-2">

                <div class="row">
                    <div class="col-6">

                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:Label class="form-label" Text="Venta" runat="server" ID="lblVenta"></asp:Label>
                            <asp:TextBox ID="tbVenta" type="text" class="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <label class="form-label" runat="server" id="inputOkVenta">Ok.Venta</label>
                            <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputOkVenta" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label class="form-label" runat="server" id="inputDibujo">Ok.Dibujo</label>
                                <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputDibujo" />
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label class="form-label" runat="server" id="inputEmpaque">Empaque</label>
                                <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputEmpaque" />
                            </div>
                        </div>

                        <div class="col-6">
                            <div class="input-group input-group-sm mb-2 gap-2">
                                <label class="form-label" runat="server" id="inputRealEmp">Real Emp.</label>
                                <input type="date" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputRealEmp" />
                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-12">
                            <div class="table-responsive mb-1 gap-2">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th scope="col">Di</th>
                                            <th scope="col">Coor</th>
                                            <th scope="col">Coordinado</th>
                                            <th scope="col">F.Despacho</th>
                                            <th scope="col">Despachado</th>
                                            <th scope="col">Despachado</th>
                                            <th scope="col">Entregado</th>
                                            <th scope="col">Entrega</th>
                                            <th scope="col">Receptor</th>
                                            <th scope="col">Celular</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <th scope="row"></th>
                                            <td>ARGFSD</td>
                                            <td>SDGFSDFG</td>
                                            <td>SFSF</td>
                                            <td>SAFSA</td>
                                            <td>SFS</td>
                                            <td>SFS</td>
                                            <td>SFS</td>
                                            <td>SAFSA</td>
                                            <td>SFSAF</td>

                                        </tr>

                                    </tbody>
                                </table>


                            </div>
                        </div>
                    </div>


                </div>

            </div>

        </div>
    </div>

    <div class="container-fluid">
        <div class="observaciones">

            <div class="div-1">


                <div class="row">
                    <div class="mb-2 gap-2">
                        <textarea id="Observacion1Id" runat="server" class="form-control form-control-lg"></textarea>
                    </div>
                </div>
            </div>





            <div class="div-3">



                <div class="row">

                    <div class="col-4">

                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:label class="form-label" Text="Supervisor" runat="server" id="lblSupervisor"></asp:label>
                            <asp:TextBox id="tbSupervisor" type="text" class="form-control" runat="server" />
                        </div>
                    </div>

                </div>

                <div class="row">


                    <div class="col-5">
                        <div class="input-group input-group-sm mb-2">
                            <div class="input-group-prepend">
                                <asp:button class="btn btn-outline-secondary" Text="Plano+" runat="server" type="button"></asp:button>
                            </div>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="tbPlano" />
                        </div>
                    </div>



                    <div class="col-7">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:label class="form-label" Text="Bolsa" runat="server" id="lblBolsa"></asp:label>
                            <asp:TextBox type="text" class="form-control" runat="server" ID="tbBolsa"/>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-4">

                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:label class="form-label" Text="Fabrica" runat="server" id="lblFabrica"></asp:label>
                            <select name="" id="inputFabrica" class="custom-select form-control">
                                <option selected=""></option>
                                <option value="01">Medellin</option>
                                <option value="02">Bogota</option>
                            </select>
                        </div>
                    </div>

                    <div class="col-2">
                        <div class="input-group input-group-sm mb-2 gap-2">
                            <asp:label class="form-label" Text="V.Pedido" runat="server" id="lblVPedido"></asp:label>
                        </div>
                    </div>


                    <div class="col-6">
                        <%--<input type="text" class="form-control" runat="server" aria-label="Sizing example input" aria-describedby="inputSaldo" />--%>
                    </div>

                    <div class="row">

                        <div class="col-4">

                            <div class="input-group input-group-sm mb-2 gap-3">
                                <asp:label class="form-label" Text="Instala" runat="server" id="lblInstala"></asp:label>
                                <select name="" id="inputInstala" class="custom-select form-control">
                                    <option selected=""></option>
                                    <option value="01">Medellin</option>
                                    <option value="02">Bogota</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-8">

                            <div class="input-group-sm mb-1 gap-2">

                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <asp:button class="btn btn-outline-secondary" runat="server" Text="TXT" type="button"></asp:button>
                                    </div>
                                    <input type="text" class="form-control" placeholder="" aria-label="" aria-describedby="basic-addon1" />
                                </div>
                            </div>
                        </div>



                    </div>

                </div>


            </div>


        </div>
    </div>


    <div class="informacion_Contable">
  
        <%--<table class="table table-bordered">
  <thead>
    <tr>
      <th scope="col">#</th>
      <th scope="col">First</th>
      <th scope="col">Last</th>
      <th scope="col">Handle</th>
    </tr>
  
             
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
      <tr>
      <th scope="row">3</th>
      <td colspan="2">Larry the Bird</td>
      <td>@twitter</td>
    </tr>
      <tr>
      <th scope="row">3</th>
      <td colspan="2">Larry the Bird</td>
      <td>@twitter</td>
    </tr>
  </tbody>
</table>--%>
    </div>
       


    </form>



    


  
    
     
   



  




         

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>


