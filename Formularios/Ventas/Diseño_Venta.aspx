<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Diseño_Venta.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Diseño_Venta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>
    
      <link type="text/css" href="../../Recursos/CSS/Ventas/Diseño_Venta.css" rel="stylesheet" />
    <title>Diseño - Departamento de Ventas</title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      


        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Diseño-BitacoraFPV-001-tab" data-bs-toggle="tab" href="#Diseño-BitacoraFPV-001-content">Diseño-Bitacora FPV-001</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="Programacion-tab" data-bs-toggle="tab" href="#Programacion-content">Programación</a>
                    </li>
                       <li class="nav-item">
                        <a class="nav-link text-dark" id="Cotizacion-tab" data-bs-toggle="tab" href="#Cotizacion-content">Cotización</a>
                       </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Buscar-tab" data-bs-toggle="tab" href="#Buscar-content" style="display:none;">Buscar Diseño</a>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="tab-content">

             <div class="tab-pane fade" id="Buscar-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container">
                            <div class="row">
                                                                            <div class="border rounded">
                                                                                <div class="row mt-1">
                                                                                    <div class="col-md-7 col-12">
                                                                                        <div class="input-group input-group-sm gap-2">
                                                                                            <asp:Label runat="server" ID="Label2" class="col-form-label-sm">Fecha de Ingreso</asp:Label>
                                                                                            <asp:TextBox ID="TextFechDeIng" runat="server" CssClass="form-control form-control-sm" type="Date"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-5 col-12">
                                                                                        <div class="input-group input-group-sm gap-2">
                                                                                            <asp:Label runat="server" ID="Label3" class="col-form-label-sm">Y</asp:Label>
                                                                                            <asp:TextBox ID="Texty" runat="server" CssClass="form-control form-control-sm"  type="Date"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                
                                                                                 <div class="row mt-2">
                                                                                    <div class="col-md-2 col-12">
                                                                                        <div class="input-group input-group-sm gap-2">
                                                                                            <asp:Label runat="server" ID="Label4" class="col-form-label-sm">Diseño N.</asp:Label>
                                                                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-3 col-12">
                                                                                        <div class="input-group input-group-sm gap-2">
                                                                                            <asp:Label runat="server" ID="Label5" class="col-form-label-sm">Cliente</asp:Label>
                                                                                            <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                      <div class="col-md-3 col-12">
                                                                                        <div class="input-group input-group-sm gap-2">
                                                                                            <asp:Label runat="server" ID="Label6" class="col-form-label-sm">Proyecto.</asp:Label>
                                                                                            <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="col-md-2 col-12">
                                                                                        <div class="input-group input-group-sm gap-2">
                                                                                        <asp:Button ID="But" runat="server" Text="Buscar" OnClick="But_Click"/>
                                                                                             
                                                                                        </div>
                                                                                    </div>
                                                                                       

                                                                                       <div class="col-md-2 col-12">
                                                                                        <div class="input-group input-group-sm gap-2">
                                                                                             <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                                             <asp:Label runat="server" ID="Label7" class="col-form-label-sm">Ver Convenciones</asp:Label>
                                                                                        </div>
                                                                                    </div>

                                                                                </div>
                                                                                
                                                                               <div class="table-responsive table-responsive-sm mb-2 gap-2" style="max-height: 300px; overflow-x: auto;">
                                                                                    <asp:DataGrid Class="table table-bordered table-hover table-sm" ID="DataGrid4" runat="server"
                                                                                        AutoGenerateColumns="false">
                                                                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                                        <Columns>
                                                                                            <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column" />
                                                                                            <asp:BoundColumn DataField="Nombre_Diseño" HeaderText="Descripcion" ItemStyle-CssClass="auto-width-column" />
                                                                                            <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                                                            <asp:BoundColumn DataField="Fecha_Ingreso" HeaderText="F.Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                                                            <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Prog" ItemStyle-CssClass="auto-width-column" />
                                                                                            <asp:BoundColumn DataField="UltimaActivacion" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column" />
                                                                                            <asp:BoundColumn DataField="Cliente" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                                        </Columns>
                                                                                    </asp:DataGrid>
                                                                                   <asp:SqlDataSource runat="server" ID="SqlDataSourceFecha" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                                       SelectCommand="SELECT Numero_Diseño, Nombre_Diseño, Asesor, Fecha_Ingreso, Fecha_Programada_Entrega, UltimaActivacion, Cliente FROM tblDiseño WHERE Fecha_Ingreso BETWEEN @FechaInicio AND @FechaFin">
                                                                                       <SelectParameters>
                                                                                           <asp:ControlParameter Name="FechaInicio" ControlID="TextFechDeIng" PropertyName="Text" />
                                                                                           <asp:ControlParameter Name="FechaFin" ControlID="Texty" PropertyName="Text" />
                                                                                       </SelectParameters>
                                                                                   </asp:SqlDataSource>
                                                                                  <asp:SqlDataSource runat="server" ID="SqlDataSourceNumeroDis" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                                       SelectCommand="SELECT Numero_Diseño, Nombre_Diseño, Asesor, Fecha_Ingreso, Fecha_Programada_Entrega, UltimaActivacion, Cliente FROM tblDiseño WHERE Numero_Diseño = @NumeroDis">
                                                                                       <SelectParameters>
                                                                                           <asp:ControlParameter Name="NumeroDis" ControlID="TextBox3" PropertyName="Text" />
                                                                                       </SelectParameters>
                                                                                   </asp:SqlDataSource>

                                                                                   <asp:SqlDataSource runat="server" ID="SqlDataSourceNombreDiseño" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                                       SelectCommand="SELECT Numero_Diseño, Nombre_Diseño, Asesor, Fecha_Ingreso, Fecha_Programada_Entrega, UltimaActivacion, Cliente FROM tblDiseño WHERE Nombre_Diseño LIKE '%' + @NombreDiseño + '%'">
                                                                                       <SelectParameters>
                                                                                           <asp:ControlParameter Name="NombreDiseño" ControlID="TextBox5" PropertyName="Text" />
                                                                                       </SelectParameters>
                                                                                   </asp:SqlDataSource>

                                                                                   <asp:SqlDataSource runat="server" ID="SqlDataSourceCliente" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                                       SelectCommand="SELECT Numero_Diseño, Nombre_Diseño, Asesor, Fecha_Ingreso, Fecha_Programada_Entrega, UltimaActivacion, Cliente FROM tblDiseño WHERE Cliente LIKE '%' + @Cliente + '%'">
                                                                                       <SelectParameters>
                                                                                           <asp:ControlParameter Name="Cliente" ControlID="TextBox4" PropertyName="Text" />
                                                                                       </SelectParameters>
                                                                                   </asp:SqlDataSource>




                                                                               </div>
                                                                            </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Diseño-BitacoraFPV-001-content">
                <asp:UpdatePanel runat="server" ID="UpdateDiseñoBitacora" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <nav class="navbar navbar-expand-sm navbar-light bg-light gap-2">
                                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                                    <span class="navbar-toggler-icon"></span>
                                </button>
                                <div class="collapse navbar-collapse" id="ejemplo2">
                                    <ul class="navbar-nav mx-auto contenedor-icono">
                                        <div class="contenedor-icono">


                                            <asp:Button runat="server" ID="NuevoDisBit" Text="N"  enabled="false" OnClick="NuevoDisBit_Click">
                                               
                                            </asp:Button>

                                            <asp:Button runat="server" ID="Grabar" Text="G" enabled="false">
                                               
                                            </asp:Button>

                                            <asp:Button runat="server" ID="Modificar" Text="M" enabled="false">
                                               
                                            </asp:Button>

                                            <asp:Button runat="server" ID="DocBitacora" Text="DB" enabled="false">
                                               
                                            </asp:Button>

                                            <asp:Button runat="server" ID="RegresarDiseño"  Text="R" enabled="false">
                                                
                                            </asp:Button>

                                            <asp:Button runat="server" ID="AdicionarElemento"  Text="AD" enabled="false">
                                               
                                            </asp:Button>

                                            <asp:Button runat="server" ID="ActualizarDiseno" Text="AC" enabled="false">
                                              
                                            </asp:Button>

                                            <asp:Button runat="server" ID="PausarDiseño" Text="PD" enabled="false">
                                                
                                            </asp:Button>

                                            <asp:Button runat="server" ID="Cancelar" Text="X" enabled="false" OnClick="Cancelar_Click">
                                               
                                            </asp:Button>

                                            <asp:Button runat="server" ID="EliminarDiseño" Text="E" enabled="false">
                                               
                                            </asp:Button>



                                        </div>
                                    </ul>
                                </div>
                            </nav>
                        </div>
                      <div id="miDiv" runat="server" data-div="miDiv">
                            <%--  1/4--%>
                            <div class="container-fluid m-1">
                                <div class="row justify-content-center">
                                    <div class="border rounded p-1">

                                        <div class="row">
                                            <div class="col-3">

                                                <div class="row d-flex justify-content-between mt-2">
                                                    <div class="col-md-8 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" class="col-form-label-sm">Diseño#:</asp:Label>
                                                            <asp:Label ID="lblNumDise" runat="server" CssClass="destacado">Número</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                       <asp:Button runat="server" id="BtnBus" type="button" OnClientClick="mostrarTab(); return false;" class="btn-outline-dark btn btn-white m-2 shadow" Text="..." />

                                                 
                                                    </div>
                 
                                                       <div class="row">
                                                        <div class="col-md-12 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Button runat="server" ID="Button1" CssClass="btn-outline-dark btn btn-white" Text="Cliente" />
                                                                <asp:TextBox ID="TextCliente" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row d-flex justify-content-between mt-1">
                                                        <div class="col-md-7 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Label runat="server" Id="lblDir" class="col-form-label-sm">Dir</asp:Label>
                                                                <asp:TextBox ID="TextDir" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-5 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Label runat="server" ID="lblDescuento" class="col-form-label-sm">Descuento</asp:Label>
                                                                <asp:TextBox ID="TextDes" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-3">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-6 col-6"> 
                                                        <asp:Label runat="server" ID="lblIngDis" class="col-form-label-sm">Ingreso de Diseño</asp:Label>
                                                        <asp:TextBox ID="TextIngDis" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-6">
                                                        <asp:Label runat="server" ID="lblUltAct" class="col-form-label-sm">Ultima Activacion</asp:Label>
                                                        <asp:TextBox ID="TextUltAc" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12 col-12">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label runat="server" ID="lblPro" class="col-form-label-sm">Proyecto</asp:Label>
                                                            <asp:TextBox ID="TextProyecto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblPla" class="col-form-label-sm">Plano</asp:Label>
                                                            <asp:TextBox ID="TextPla" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-2 gap-2">
                                                            <asp:Label runat="server" ID="lblUrg" class="col-form-label-sm">Urgente</asp:Label>
                                                            <asp:CheckBox ID="ChecUrgent" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-2 gap-2">
                                                            <asp:Label runat="server" ID="lblCotizar" class="col-form-label-sm">Cotizar</asp:Label>

                                                            <asp:CheckBox ID="ChecCot" runat="server" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="col-3 ">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-5 col-6">
                                                        <asp:Label runat="server" ID="lblEnt" class="col-form-label-sm">Entrega</asp:Label>
                                                        <asp:TextBox ID="TextEntrega" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-5 col-6">
                                                        <asp:Label runat="server" ID="lblEntDib" class="col-form-label-sm">Fecha Ok Dibujo</asp:Label>
                                                        <asp:TextBox ID="TextFecOkDib" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2 col-6">
                                                        <asp:Label runat="server" ID="lblZon" class="col-form-label-sm">Zona</asp:Label>
                                                        <asp:TextBox ID="TextZona" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-8 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblCon" class="col-form-label-sm">Contacto</asp:Label>
                                                            <asp:TextBox ID="TextContacto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblTel" class="col-form-label-sm">Tel</asp:Label>
                                                            <asp:TextBox ID="TextTel" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label runat="server" ID="lblMaiTer" class="col-form-label-sm">Mail Term</asp:Label>
                                                            <asp:CheckBox ID="ChecMailTer" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label runat="server" ID="lblCotVia" class="col-form-label-sm">Cotiza Viá</asp:Label>
                                                            <asp:CheckBox ID="ChecCotVia" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label runat="server" ID="lblCotTte" class="col-form-label-sm">Cotiza Tte</asp:Label>
                                                            <asp:CheckBox ID="CheckBox4" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="col-3 ">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-6 col-12">
                                                        <asp:Label runat="server" ID="lblAse" class="col-form-label-sm">Asesor</asp:Label>
                                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control form-control-sm" Enabled="false" DataSourceID="SqlDataSource2" DataTextField="NombreCompleto">
                                                        </asp:DropDownList>

                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"
                                                            SelectCommand="SELECT Nombre + ' ' + Apellidos AS NombreCompleto FROM tblAsesorComercial WHERE Activo = '1'"></asp:SqlDataSource>


                                                    </div>
                                                    <div class="col-md-6 col-12">
                                                        <asp:Label runat="server" ID="lblPre" class="col-form-label-sm">Pre</asp:Label>
                                                        <asp:TextBox ID="TextPre" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-4 col-3">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblCel" class="col-form-label-sm">Cel</asp:Label>
                                                            <asp:TextBox ID="TextCel" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-8 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" ID="lblMai" class="col-form-label-sm">Mail</asp:Label>
                                                            <asp:TextBox ID="TextMail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row d-flex justify-content-between">
                                                        <div class="col-md-7 col-3">
                                                            <div class="input-group input-group-sm mt-1 gap-2">
                                                                <asp:Label runat="server" ID="lblCiuPro" class="col-form-label-sm">Ciudad proyecto</asp:Label>
                                                                <asp:TextBox ID="TextCiuPro" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-5 col-4 mt-1">
                                                            <asp:Button runat="server" ID="BtnProgramar" CssClass="btn-outline-dark btn btn-sm btn-white" Text="PROGRAMAR" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%-- 2/4--%>
                            <div class="container-fluid m-2">
                                <div class="row justify-content-center">
                                    <div class="border rounded p-3">

                                        <div class="row">
                                            <div class="col-2 border">
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblConCab" runat="server" class="form-label" Style="font-size: 16px; font-weight: bold;">Conduccion de Cables</asp:Label>
                                                        <asp:CheckBox ID="ChecConDeCab" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm  gap-2">
                                                            <asp:Label ID="lblPis" runat="server" class="col-form-label-sm g-5">Piso</asp:Label>
                                                            <asp:CheckBox ID="ChecPiso" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm  gap-2">
                                                            <asp:Label ID="lblDiv" runat="server" class="col-form-label-sm g-5">División</asp:Label>
                                                            <asp:CheckBox ID="ChecDiv" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label ID="lblCie" runat="server" class="col-form-label-sm">Cielo</asp:Label>
                                                            <asp:CheckBox ID="ChecCie" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label id="lblCan" runat="server" class="col-form-label-sm">Canaleta</asp:Label>
                                                            <asp:CheckBox ID="ChecCan" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 col-6">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblBteEle" runat="server" class="col-form-label-sm">Bte.Elec</asp:Label>
                                                        <asp:CheckBox ID="ChecBteEle" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 col-6">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblBteSw" runat="server" class="col-form-label-sm">Bte Sw</asp:Label>
                                                        <asp:CheckBox ID="ChecBteSw" runat="server" />
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-2 border">
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblSujPt" runat="server" class="form-label" Style="font-size: 16px; font-weight: bold;">Sujeción PT</asp:Label>
                                                        <asp:CheckBox ID="ChecSujPt" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-6 col-6">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblAlCie" runat="server" class="col-form-label-sm">Al Cielo</asp:Label>
                                                        <asp:CheckBox ID="ChecAlCie" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblPerRef" runat="server" class="col-form-label-sm">Perfil Refuerzo</asp:Label>
                                                        <asp:CheckBox ID="ChecPerRef" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblGuaEsc" runat="server" class="col-form-label-sm">Guarda Escobas</asp:Label>
                                                        <asp:CheckBox ID="ChecGuaEsc" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm p-1 gap-2">
                                                        <asp:Label ID="lblHTotCms" runat="server" class="col-form-label-sm">H.Total(Cms)</asp:Label>
                                                        <asp:TextBox ID="TexHTot" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6 border">
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblEsyMat" runat="server" class="form-label col-12 text-dark text-uppercase" Style="font-size: 16px; font-weight: bold;">Especificaciones y Materiales</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblLin" runat="server" class="col-form-label-sm">Linea</asp:Label>
                                                            <asp:TextBox ID="TextLin" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblMos" runat="server" class="col-form-label-sm">Mostrador</asp:Label>
                                                            <asp:TextBox ID="TextMos" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblSup" runat="server" class="col-form-label-sm">Superficies</asp:Label>
                                                            <asp:TextBox ID="TextSup" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-1">
                                                            <asp:Label ID="lblBal" runat="server" class="col-form-label-sm">Balance</asp:Label>
                                                            <asp:CheckBox ID="CheckBox16" runat="server" CssClass="form-check" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblSop" runat="server" class=" col-form-label-sm">Soporte</asp:Label>
                                                            <asp:TextBox ID="TextSop" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblGav" runat="server" class="col-form-label-sm">Gaveta</asp:Label>
                                                            <asp:TextBox ID="TextGav" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblPan" runat="server" class="col-form-label-sm">Paneles</asp:Label>
                                                            <asp:TextBox ID="TextPan" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblTPie" runat="server" class="col-form-label-sm">T.Piernas</asp:Label>
                                                            <asp:TextBox ID="TextTapPie" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblRep" runat="server" class="col-form-label-sm">Repisa</asp:Label>
                                                            <asp:TextBox ID="TextRep" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row d-flex justify-content-between">
                                                    <div class="col-md-6 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblTipVid" runat="server" class="col-form-label-sm">Tipo Vidrio</asp:Label>
                                                            <asp:TextBox ID="TextTipVid" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblPant" runat="server" class="col-form-label-sm">Pantallas</asp:Label>
                                                            <asp:TextBox ID="TextPant" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3 col-6">
                                                        <div class="input-group input-group-sm mt-1 gap-2">
                                                            <asp:Label ID="lblArc" runat="server" class="col-form-label-sm">Arch</asp:Label>
                                                            <asp:TextBox ID="TextArch" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-2 border">
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblMue" runat="server" class="form-label" Style="font-size: 16px; font-weight: bold;">Muebles</asp:Label>
                                                        <asp:CheckBox ID="ChecMue" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label id="lblCoc" runat="server" class="col-form-label-sm">Coco</asp:Label>
                                                        <asp:TextBox ID="TextCoc" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lblEntr" runat="server" class="col-form-label-sm">Entrepaño</asp:Label>
                                                        <asp:TextBox ID="TextEnt" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lblPuer" runat="server" class="col-form-label-sm">Puertas</asp:Label>
                                                        <asp:TextBox ID="TextPuer" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--  3/4--%>
                            <div class="container-fluid m-2">
                                <div class="row justify-content-center">
                                    <div class="border rounded p-1">
                                        <div class="row">
                                            <div class="col-4">
                                                <h6>Observaciones Ventas</h6>
                                                <textarea id="TextObsVen" class="form-control" style="height: 100px" runat="server"></textarea>
                                            </div>
                                            <div class="col-4">
                                                <h6>Observaciones de Dibujo y Despiece</h6>
                                                <textarea id="TextObsDibDes" class="form-control" style="height: 100px" runat="server"></textarea>
                                            </div>
                                            <div class="col-4">
                                                <h6>Seguimiento de Pausas y Devoluciones</h6>
                                                <textarea id="TextSegPauDev" class="form-control" style="height: 100px" runat="server"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                       <%-- 4/4--%>
                        <div class="container-fluid m-2">
                            <div class="row justify-content-center">
                                <div class="border rounded p-1">
                                     <div class="row">
                                        <div class="col-2">
                                            <div class="border rounded p-1" style="height:250px">
                                                <h6>ShowCase</h6>
                                                 <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox18" runat="server" />
                                                    <asp:Label ID="lblPrePpt" runat="server" class="col-form-label-sm">Presentación PPT</asp:Label>                                                    
                                                </div>
                                            </div>
                                                <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox19" runat="server" />
                                                    <asp:Label ID="lblIma" runat="server" class="col-form-label-sm">Imágenes</asp:Label>                                                    
                                                </div>
                                            </div>
                                                 <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox20" runat="server" />
                                                    <asp:Label ID="lblAcc" runat="server" class="col-form-label-sm">Accesorios</asp:Label>                                                    
                                                </div>
                                            </div>
                                                  <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:CheckBox ID="CheckBox21" runat="server" />
                                                    <asp:Label ID="lblTieRea" runat="server" class="col-form-label-sm">Tiempo Real</asp:Label>                                                    
                                                </div>
                                            </div>
                                                
                                                <div class="col-md-10 col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                 <asp:TextBox ID="TextFec" runat="server" CssClass="form-control-sm form-control" type="Date"></asp:TextBox>
                                                 <asp:TextBox ID="TextFech" runat="server" CssClass="form-control-sm form-control" type="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                                 <div class="col-md-10 col-12">
                                                     <asp:Label ID="lblUbi" runat="server" class="col-form-label-sm">Ubicación</asp:Label>
                                                     <asp:TextBox ID="TextUbi" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                 </div>
                                            </div>
                                        </div>
                                         <div class="col-10">
                                             <div class="container-fluid">
                                                 <div class="row justify-content-center">
                                                     
                                                   

                                                     <div class="border rounded p-1" style="height: 250px">
                                                     </div>
                                                 </div>
                                             </div>
                                         </div>
                                     </div>
                                </div>
                            </div>
                        </div>
                        <%--BOTONES--%>
                        <div class="container-fluid">
                            <button href="#" title="Nuevo diseño o bitacora" id="Button3" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                <i class="bi bi-currency-dollar"></i>
                            </button>
                         <button href="#" title="Nuevo diseño o bitacora" id="Button4" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                              <button href="#" title="Nuevo diseño o bitacora" id="Button5" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                              <button href="#" title="Nuevo diseño o bitacora" id="Button6" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                              <button href="#" title="Nuevo diseño o bitacora" id="Button7" class="btn-outline-dark btn btn-white  btn-block animate__animated animate__wobble" runat="server" onclick="btnBitaco_Click">
                                               <i class="bi bi-currency-dollar"></i>
                                            </button>
                        </div>
                       
                          </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade show active" id="Programacion-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="container-fluid m-2">
                            <div class="row justify-content-center">
                                <div class="border rounded p-1 special-border col-11" style="height: auto; min-height: 880px;">

                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>

                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover" ID="DataGrid1" runat="server" DataSourceID="SqlDataSource1"
                                                                AutoGenerateColumns="false" OnItemDataBound="DataGrid1_ItemDataBound">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Nombre de la Obra" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Nombre_Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Entrega_Dibujo_Despiece" HeaderText="F.Ingreso" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="Nueva Columna">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToDateTime(Eval("Fecha_Entrega_Dibujo_Despiece")).AddDays(2).ToString("dd/MM/yyyy hh:mm:ss tt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Dibujante">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbDibujante" runat="server" Text='<%# Eval("RealizadoPor") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:BoundColumn DataField="Fecha_Despacho_Produccion" HeaderText="F.Despacho" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="sp_ProBitacoraOTs" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="DropDownList1" PropertyName="Text" Name="Nombre_Asesor" Type="String"></asp:ControlParameter>

                                                                </SelectParameters>
                                                            </asp:SqlDataSource>

                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:ImageButton ID="ImageButton8" runat="server" CssClass="btn btn-white border-dark"
                                                        ImageUrl="../../TreeLineImages/arrow-repeat.png" OnClick="Button88_Click"
                                                        ToolTip="Actualizar datagrids" />

                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-6">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lbZona" runat="server" class="col-form-label-sm">Zona</asp:Label>
                                                        <asp:DropDownList ID="DropDownListOptions" runat="server" CssClass="form-control-sm form-control" OnSelectedIndexChanged="DropDownListOptions_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="%" Value="%" />
                                                            <asp:ListItem Text="01" Value="01" />
                                                            <asp:ListItem Text="02" Value="02" />
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox22" runat="server" OnCheckedChanged="CheckBox22_CheckedChanged" AutoPostBack="true" />
                                                            <asp:Label runat="server" CssClass="col-form-label-sm">Ver Convernciones</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div id="modal" class="modal fade" tabindex="-1" role="dialog">
                                                    <div class="modal-dialog modal-dialog-centered" role="document">
                                                        <div class="modal-content">
                                                            <div class="modal-header">
                                                            </div>
                                                            <div class="modal-body" id="modalContent">

                                                                <div class="input-group input-group-sm mb-2 gap-2">
                                                                    <div class="input-group bg-success-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="noproVen" class="form-label">No prog por Ventas</label>
                                                                </div>

                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group bg-white-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="NocumenEsp" class="form-label">No cumplidos y en espera</label>
                                                                </div>

                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group bg-warning-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="Pendiente" class="form-label">Pendientes</label>
                                                                </div>

                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group bg-danger-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="urgente" class="form-label">Urgente</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group bg-penAprCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="penAprCot" class="form-label">Pendientes por Aprobacion para Cotizar</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group bg-penCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="penCot" class="form-label">Pendientes por Cotizacion</label>
                                                                </div>
                                                                <div class="input-group input-group-sm mb-1 gap-2">
                                                                    <div class="input-group bg-pausados-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                                    <label for="pausados" class="form-label">Pausados</label>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:CheckBox ID="CheckBox23" runat="server" OnCheckedChanged="CheckBox23_CheckedChanged" AutoPostBack="true" />
                                                            <asp:Label runat="server" class="col-form-label-sm">Resumen Dibujante</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div id="modal2" class="modal fade" tabindex="-1" role="dialog">
                                                    <div class="modal-dialog modal-dialog-centered modal-sm" role="document">
                                                        <div class="modal-content">
                                                            <div class="modal-body">
                                                                <h6>Resumen Dibujante</h6>
                                                                <div class="row">
                                                                    <div class="border rounded">
                                                                        <div class="table-responsive" style="height: 400px; width: 300px">
                                                                            <asp:DataGrid Class="table table-bordered table-hover table-sm" ID="DataGrid3" runat="server"
                                                                                DataSourceID="DataGridResumenDibujante" AutoGenerateColumns="false">
                                                                                 <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                                <Columns>
                                                                                    <asp:BoundColumn DataField="Dibujante" HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn DataField="Ped" HeaderText="Ped" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn DataField="UltPedido" HeaderText="UltPedido" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn DataField="Dis" HeaderText="Dis" ItemStyle-CssClass="auto-width-column"/>
                                                                                    <asp:BoundColumn DataField="PactoEntrega" HeaderText="PactoEntrega" ItemStyle-CssClass="auto-width-column"/>
                                                                                    <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-CssClass="auto-width-column"/>
                                                                                </Columns>
                                                                            </asp:DataGrid>
                                                                            <asp:SqlDataSource runat="server" ID="DataGridResumenDibujante" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ResumenDibujante" SelectCommandType="StoredProcedure">                                                                              
                                                                            </asp:SqlDataSource>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button9" runat="server" Text="Trabajar Pedido" CssClass="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__flash" OnClick="Button9_Click" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button10" runat="server" Text="Trabajar Pedido" CssClass="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" OnClick="Button10_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                          <asp:DataGrid CssClass="table table-bordered table-sm table-hover" ID="DataGrid2" runat="server" DataSourceID="DataGridDiseño" AutoGenerateColumns="false"
                                                              OnItemDataBound="DataGrid2_ItemDataBound" OnItemCommand="DataGridDise_ItemCommand">
                                                              <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                              <Columns>
                                                                  <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                      <ItemTemplate>
                                                                          <asp:LinkButton ID="lnkClie" runat="server" CommandName="Numero_Diseño" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Container.ItemIndex + 1 %>' CssClass="text-white text-decoration-none" OnClick="lnkClie_Click" />
                                                                      </ItemTemplate>
                                                                  </asp:TemplateColumn>
                                                                  <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                  <asp:BoundColumn HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                                  <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Ult.Act" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                  <asp:TemplateColumn HeaderText="Dibujante">
                                                                      <ItemTemplate>
                                                                          <asp:Label ID="lbDibujante2" runat="server" Text='<%# Eval("RealizadoPor") %>'></asp:Label>
                                                                      </ItemTemplate>
                                                                  </asp:TemplateColumn>
                                                                  <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="PactodeEntrega" HeaderText="Pacto" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="ProgramadoVentas" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="PasarACotizar" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="TerminadoDibujo" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="Pausado" HeaderText="Pausado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                              </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridDiseño" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="sp_ProBitacoraDise" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="DropDownList1" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter> 
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Label runat="server" class="col-form-label-sm">Pacto de entrega</asp:Label>
                                                    <asp:TextBox ID="TextBox37" runat="server" CssClass="form-control-sm form-control" type="Date"></asp:TextBox>
                                                </div>
                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button11" runat="server" Text="Trabajar Diseño" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Button ID="Button14" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                             <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover" ID="DataGridDiseños" runat="server"
                                                            DataSourceID="DataGridDiseñosPorFecha" AutoGenerateColumns="false" OnItemDataBound="DataGrid3_ItemDataBound">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                 <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Descripcion-ShowCase" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                                                  
                                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Fecha" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Hora" HeaderText="Hora" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="RealizadoPor" HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Ubicacion" HeaderText="Ubicación" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Imagenes" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="SC_Terminado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                        <asp:SqlDataSource runat="server" ID="DataGridDiseñosPorFecha" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ProBitacoraShowCase" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="DropDownList1" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Button ID="Button13" runat="server" Text="Trabajar ShowCase" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-12 mt-1">
                                                    <asp:Button ID="Button12" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-10">
                                            <div class="container-fluid m-1">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 212px;">
                                                        <%-- DATAGRID--%>
                                                         <div class="table-responsive mb-2 gap-2" style="max-height: 212px; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="table table-bordered table-hover table-sm" ID="DataGridRender" runat="server"
                                                            DataSourceID="DataGridRenderPorFechaYAsesor" AutoGenerateColumns="false" OnItemDataBound="DataGrid4_ItemDataBound">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                 <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Id_Render" HeaderText="ID" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn HeaderText="Nombre-Render" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>                                                                                                                          <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Activado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                <asp:BoundColumn DataField="TerminadoRender" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                        <asp:SqlDataSource runat="server" ID="DataGridRenderPorFechaYAsesor" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ProBitacoraRender" SelectCommandType="StoredProcedure">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="DropDownList1" PropertyName="Text" Name="Asesor" Type="String"></asp:ControlParameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                             </div>
                                                    </div>
                                        </div>
                                    </div>
                                   </div>
                                         <div class="col-2">
                                             <div class="row">
                                                 <div class="col-12">
                                                             <asp:Button ID="Button15" runat="server" Text="Trabajar Render"  class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse"/>
                                                     </div>
                                                 </div>    
                                             <div class="row">
                                                     <div class="col-12">
                                                             <asp:Button ID="Button16" runat="server" Text="Desprogramar"  class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse"/>
                                                     </div>
                                                 </div>          
                                            </div>
                                        </div>
                                        </div>
                                    </div>                                                      
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            


        </div>

    </form>



    <%--<script>
       
        

        var grabarButton = document.getElementById('<%= Grabar.ClientID %>');
        var docBitacoraButton = document.getElementById('<%= DocBitacora.ClientID %>');
        var regresarDiseñoButton = document.getElementById('<%= RegresarDiseño.ClientID %>');
        var pausarDiseñoButton = document.getElementById('<%= PausarDiseño.ClientID %>');
        var eliminarDiseñoButton = document.getElementById('<%= EliminarDiseño.ClientID %>');

        // Obtener los elementos de enlace por sus IDs
        var enlace = document.getElementById("NuevoDisBit");

        var modificarLink = document.getElementById("Modificar");
        var adicionarElementoLink = document.getElementById("AdicionarElemento");
        var actualizarDisenoLink = document.getElementById("ActualizarDiseno");
        var cancelarLink = document.getElementById("Cancelar");

        var deshabilitarGrabar = true; // Cambia esto según tus condiciones
        var deshabilitarDocBitacora = true; // Cambia esto según tus condiciones
        var deshabilitarRegresarDiseño = true; // Cambia esto según tus condiciones
        var deshabilitarPausarDiseño = true; // Cambia esto según tus condiciones
        var deshabilitarEliminarDiseño = true; // Cambia esto según tus condiciones

        // Comprobar si deseas deshabilitar los enlaces
        var deshabilitarModificar = true; // Cambia esto según tus condiciones
        var deshabilitarAdicionarElemento = true; // Cambia esto según tus condiciones
        var deshabilitarActualizarDiseno = true; // Cambia esto según tus condiciones
        var deshabilitarCancelar = true; // Cambia esto según tus condiciones
        var deshabilitarEnlace = true; // Cambia esto según tus condiciones


        if (deshabilitarGrabar) {
            grabarButton.disabled = true;
            grabarButton.classList.add("disabled");
        }

        function checkDisabled(linkButton) {
            if (linkButton.disabled) {
                return false; // Cancelar el clic si el LinkButton está deshabilitado
            }
            return true; // Dejar que el clic se procese si el LinkButton está habilitado
        }


        if (deshabilitarDocBitacora) {
            docBitacoraButton.disabled = true;
            docBitacoraButton.classList.add("disabled");
        }

        if (deshabilitarRegresarDiseño) {
            regresarDiseñoButton.disabled = true;
            regresarDiseñoButton.classList.add("disabled");
        }

        if (deshabilitarPausarDiseño) {
            pausarDiseñoButton.disabled = true;
            pausarDiseñoButton.classList.add("disabled");
        }

        if (deshabilitarEliminarDiseño) {
            eliminarDiseñoButton.disabled = true;
            eliminarDiseñoButton.classList.add("disabled");
        }

        if (deshabilitarEnlace) {
            // Deshabilitar el enlace
            enlace.href = "javascript:void(0);";
            // Cambiar el estilo del enlace cuando esté deshabilitado
            enlace.classList.add("disabled");
            // Evitar que el enlace sea clickeable
            enlace.addEventListener("click", function (event) {
                event.preventDefault();
            });
        }

        if (deshabilitarModificar) {
            modificarLink.href = "javascript:void(0);";
            modificarLink.classList.add("disabled");
            modificarLink.addEventListener("click", function (event) {
                event.preventDefault();
            });
        }

        if (deshabilitarAdicionarElemento) {
            adicionarElementoLink.href = "javascript:void(0);";
            adicionarElementoLink.classList.add("disabled");
            adicionarElementoLink.addEventListener("click", function (event) {
                event.preventDefault();
            });
        }

        if (deshabilitarActualizarDiseno) {
            actualizarDisenoLink.href = "javascript:void(0);";
            actualizarDisenoLink.classList.add("disabled");
            actualizarDisenoLink.addEventListener("click", function (event) {
                event.preventDefault();
            });
        }

        if (deshabilitarCancelar) {
            cancelarLink.href = "javascript:void(0);";
            cancelarLink.classList.add("disabled");
            cancelarLink.addEventListener("click", function (event) {
                event.preventDefault();
            });
        }

        // Habilitar el elemento 'NuevoDisBit'
        enlace.disabled = false;
        enlace.classList.remove("disabled");

        // Habilitar el elemento 'ActualizarDiseno'
        actualizarDisenoLink.href = "#"; // Restaurar el enlace
        actualizarDisenoLink.classList.remove("disabled");
        actualizarDisenoLink.removeEventListener("click", function (event) {
            event.preventDefault();
        });

        // Habilitar el elemento 'Cancelar'
        cancelarLink.href = "#"; // Restaurar el enlace
        cancelarLink.classList.remove("disabled");
        cancelarLink.removeEventListener("click", function (event) {
            event.preventDefault();
        });

        // Obtener el div por su identificador
        var div = document.getElementById('miDiv');

        // Deshabilitar el div
        div.setAttribute('disabled', 'true');

        // Deshabilitar todos los elementos secundarios del div
        var elements = div.getElementsByTagName('*');
        for (var i = 0; i < elements.length; i++) {
            elements[i].setAttribute('disabled', 'true');
        }

       
            
            
    </script>



   <script>

       
       // Obtener el elemento 'NuevoDisBit' por su ID
       var nuevoDisBitLink = document.getElementById("NuevoDisBit");

       // Obtener el elemento 'ActualizarDiseno' por su ID
       var actualizarDisenoLink = document.getElementById("ActualizarDiseno");

       // Obtener el LinkButton 'Grabar' por su ID
       var grabarButton = document.getElementById("<%= Grabar.ClientID %>");

       // Obtener el elemento 'Cancelar' por su ID
       var cancelarLink = document.getElementById("Cancelar");

       // Obtener el div 'miDiv' por su ID
       var miDiv = document.getElementById("miDiv");

       // Agregar un controlador de eventos para el clic en 'NuevoDisBit'
       nuevoDisBitLink.addEventListener("click", function (event) {
           event.preventDefault(); // Evitar que el enlace se comporte normalmente

           // 1. Deshabilitar el elemento 'NuevoDisBit'
           nuevoDisBitLink.disabled = true;
           nuevoDisBitLink.classList.add("disabled");

           // 2. Deshabilitar el elemento 'ActualizarDiseno'
           actualizarDisenoLink.disabled = true;
           actualizarDisenoLink.classList.add("disabled");

           // 3. Habilitar el LinkButton 'Grabar'
           grabarButton.disabled = false;
           grabarButton.classList.remove("disabled");

           // 4. Habilitar el elemento 'Cancelar'
           cancelarLink.disabled = false;
           cancelarLink.classList.remove("disabled");

           // 5. Habilitar el div 'miDiv'
           miDiv.disabled = false;

           // También puedes deshabilitar los elementos secundarios del div 'miDiv' si es necesario
           var elements = miDiv.getElementsByTagName("*");
           for (var i = 0; i < elements.length; i++) {
               elements[i].disabled = false;
           }

           lblNumDise.innerText = "Por Definir";

           // Obtener el label 'lblCotizar' por su ID
           var labelCotizar = document.getElementById("<%= lblCotizar.ClientID %>");

           labelCotizar.style.color = "red";
           labelCotizar.style.fontWeight = "bold";
       });

       // Obtener el elemento 'NuevoDisBit' por su ID
       var nuevoDisBitLink = document.getElementById("NuevoDisBit");

       // Obtener el elemento 'ActualizarDiseno' por su ID
       var actualizarDisenoLink = document.getElementById("ActualizarDiseno");

       // Obtener el LinkButton 'Grabar' por su ID
       var grabarButton = document.getElementById("<%= Grabar.ClientID %>");

        // Obtener el elemento 'Cancelar' por su ID
        var cancelarLink = document.getElementById("Cancelar");

        // Obtener el div 'miDiv' por su ID
        var miDiv = document.getElementById("miDiv");

        // Agregar un controlador de eventos para el clic en 'Cancelar'
        cancelarLink.addEventListener("click", function (event) {
            event.preventDefault(); // Evitar que el enlace se comporte normalmente

            if (!grabarButton.disabled) {
                // 1. Deshabilitar el div 'miDiv'
                miDiv.disabled = true;

                // También puedes deshabilitar los elementos secundarios del div 'miDiv' si es necesario
                var elements = miDiv.getElementsByTagName("*");
                for (var i = 0; i < elements.length; i++) {
                    elements[i].disabled = true;
                }

                // 1. Habilitar nuevamente el elemento 'NuevoDisBit'
                nuevoDisBitLink.disabled = false;
                nuevoDisBitLink.classList.remove("disabled");

                // 2. Deshabilitar el LinkButton 'Grabar'
                grabarButton.disabled = true;
                grabarButton.classList.add("disabled");

                // 3. Habilitar el elemento 'ActualizarDiseno'
                actualizarDisenoLink.disabled = false;
                actualizarDisenoLink.classList.remove("disabled");

            }
            

            var labelCotizar = document.getElementById("<%= lblCotizar.ClientID %>");
            labelCotizar.style.color = "black"; // Restaurar el color original (negro)
            labelCotizar.style.fontWeight = "normal"; // Restaurar el peso de la fuente original (sin negrita)

            lblNumDise.innerText = "Número";


        });
       
   </script>

   
    <script type="text/javascript">
        function disableElements() {
            var elements = document.getElementsByClassName('disabled-action');
            for (var i = 0; i < elements.length; i++) {
                elements[i].setAttribute('disabled', 'disabled');
                elements[i].classList.add('disabled'); // Agregar una clase 'disabled' para cambiar el estilo si es necesario.
            }

        }
       

        function checkDisabled(sender) {
            if (sender.getAttribute('disabled') === 'disabled') {
                alert('Los elementos están deshabilitados.');
                return false; // Impedir la acción si los elementos están deshabilitados.
            }
            return true; // Permitir la acción si los elementos no están deshabilitados.
        }
    </script>

    

    <script>
        // Obtén los LinkButton por su clase CSS
        var linkButtons = document.querySelectorAll('.disabled-action');

        linkButtons.forEach(function (linkButton) {
            linkButton.addEventListener("click", function (event) {
                if (linkButton.getAttribute('data-enabled') === 'false') {
                    event.preventDefault(); // Evitar el postback si el botón está deshabilitado
                }
            });
        });
    </script>

    --%>



<script type="text/javascript">
    function mostrarTab() {
        // Cambia el estilo del tab para hacerlo visible
        document.getElementById('Buscar-tab').style.display = 'block';

        // Activa el tab
        $('#Buscar-tab').tab('show');
    }
</script>



  


     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
