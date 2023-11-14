<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NitOTs.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.NitOTs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
      <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/NitOts.css" rel="stylesheet" />

    <title>Cliente Obra</title>
</head>
<body>
    <form id="form1" runat="server">
           <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
          <nav class="navbar navbar-light bg-light">
                <div class="container d-flex justify-content-center">
                    <ul class="nav nav-tabs" id="myTabs">

                        <li class="nav-item active">
                            <a class="nav-link text-dark" id="Cliente-tab" data-bs-toggle="tab" href="#Cliente-content">Cliente</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" id="ConFac-tab" data-bs-toggle="tab" href="#ConFac-content">Contacto Factura</a>
                        </li>

                    </ul>
                </div>
            </nav>
         <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade show active" id="Cliente-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container-fluid">
                             <div class="row">
                            <div class="col-12">
                                <div class="p-3 m-2 border" style="height: 23rem;">
                                  
                                 
                                </div>                            
                            </div>                          
                        </div>

                              <div class="row">
                            <div class="col-12">
                                <div class="p-3 m-2 border" style="height: 20rem;">
                                  
                                 
                                </div>                            
                            </div>                          
                        </div>

                             <div class="row">
                            <div class="col-12">
                                <div class="p-3 m-2 border" style="height: 10rem;">
                                  
                                 
                                </div>                            
                            </div>                          
                        </div>
                        </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
              <div class="tab-pane fade" id="ConFac-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container-fluid">
                            <h6>Bienvenido 2</h6>
                        </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

             </div>
    </form>
     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
