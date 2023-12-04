<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Diseño_Venta.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Diseño_Venta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />

    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.1/xlsx.full.min.js"></script>

    <link type="text/css" href="../../Recursos/CSS/Ventas/Diseño_Venta.css" rel="stylesheet" />
    <title>Diseño - Departamento de Ventas</title>
</head>
<body>
   <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       <asp:Literal ID="litModalScript" runat="server"></asp:Literal>



        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">
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
                        <a class="nav-link text-dark" id="Buscar-tab" data-bs-toggle="tab" href="#Buscar-content" style="display: none;">Buscar Diseño</a>
                    </li>







                </ul>
            </div>
        </nav>

        <div class="tab-content" id="myTabContent">


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
                                                <asp:TextBox ID="Texty" runat="server" CssClass="form-control form-control-sm" type="Date"></asp:TextBox>
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
                                                <asp:Button ID="But" runat="server" Text="Buscar" OnClick="But_Click" />

                                            </div>
                                        </div>


                                        <div class="col-md-2 col-12">
                                            <div class="input-group input-group-sm gap-2">
                                                <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                                                <asp:Label runat="server" ID="Label7" class="col-form-label-sm">Ver Convenciones</asp:Label>

                                            </div>
                                        </div>
                                        <div id="modal1" class="modal fade" tabindex="-1" role="dialog">
                                            <div class="modal-dialog modal-dialog-centered" role="document">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                    </div>
                                                    <div class="modal-body" id="modalContent1">

                                                        <div class="input-group input-group-sm mb-2 gap-2">
                                                            <div class="input-group bg-success-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                            <label for="noproVen" class="form-label">Diseños No programados por Ventas</label>
                                                        </div>

                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group bg-white-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                            <label for="NocumenEsp" class="form-label">No cumplidos y en espera de Dibujo y Despiece</label>
                                                        </div>

                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group bg-warning-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                            <label for="Pendiente" class="form-label">Pendientes Por Dibujo y Despiece</label>
                                                        </div>


                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group bg-penAprCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                            <label for="penAprCot" class="form-label">Pendientes por Aprobacion para Cotizar</label>
                                                        </div>
                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group bg-penCot-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                            <label for="penCot" class="form-label">Diseños Pendientes por Cotizacion</label>
                                                        </div>
                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group bg-terminado-custom" style="width: 20px; height: 20px; border: 1px"></div>
                                                            <label for="pausados" class="form-label">Diseños Terminados 100%</label>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="table-responsive table-responsive-sm mb-2 gap-2" style="max-height: 300px; overflow-x: auto;">
                                        <asp:DataGrid Class="table table-bordered table-hover table-sm" ID="DataGrid4" runat="server" OnItemDataBound="DataGridBusDis_ItemDataBound" OnItemCommand="DataGridBusDise_ItemCommand"
                                            AutoGenerateColumns="false">
                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>

                                                        <asp:LinkButton ID="lnkCliee" runat="server" CommandName="Numero_Diseño"
                                                            CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" OnClick="lnkCliee_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkSelectRow" runat="server" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Container.ItemIndex + 1 %>' CssClass="text-white text-decoration-none" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Nombre_Diseño" HeaderText="Descripcion" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Fecha_Ingreso" HeaderText="F.Ingreso" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Prog" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="UltimaActivacion" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Cliente" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ProgramadoVentas" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="PasarACotizar" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="TerminadoDibujo" HeaderText="Nueva" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Pausado" HeaderText="Pausado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CotizaciónOK" HeaderText="CotizaciónOK" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceFecha" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                            SelectCommand="SELECT A.CotizaciónOK, A.ProgramadoVentas, A.PasarACotizar, A.TerminadoDibujo, A.Pausado, A.Numero_Diseño, A.Nombre_Diseño,  A.Asesor, A.Fecha_Ingreso, A.Fecha_Programada_Entrega, A.UltimaActivacion, A.Cliente, B.Cedula FROM tblDiseño A INNER JOIN tblAsesorComercial B ON (B.Nombre + ' ' + B.Apellidos) = A.Asesor WHERE A.Fecha_Ingreso BETWEEN @FechaInicio AND @FechaFin AND B.Cedula = @Cedula">
                                            <SelectParameters>
                                                <asp:ControlParameter Name="FechaInicio" ControlID="TextFechDeIng" PropertyName="Text" />
                                                <asp:ControlParameter Name="FechaFin" ControlID="Texty" PropertyName="Text" />
                                                <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceNumeroDis" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                            SelectCommand="SELECT A.CotizaciónOK, A.ProgramadoVentas, A.PasarACotizar, A.TerminadoDibujo, A.Pausado, A.Numero_Diseño, A.Nombre_Diseño,  A.Asesor, A.Fecha_Ingreso, A.Fecha_Programada_Entrega, A.UltimaActivacion, A.Cliente, B.Cedula FROM tblDiseño A INNER JOIN tblAsesorComercial B ON (B.Nombre + ' ' + B.Apellidos) = A.Asesor WHERE A.Numero_Diseño = @NumeroDis AND B.Cedula = @Cedula">
                                            <SelectParameters>
                                                <asp:ControlParameter Name="NumeroDis" ControlID="TextBox3" PropertyName="Text" />
                                                <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceNombreDiseño" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                            SelectCommand="SELECT A.CotizaciónOK, A.ProgramadoVentas, A.PasarACotizar, A.TerminadoDibujo, A.Pausado, A.Numero_Diseño, A.Nombre_Diseño, A.Asesor, A.Fecha_Ingreso, A.Fecha_Programada_Entrega, A.UltimaActivacion, A.Cliente, B.Cedula FROM tblDiseño A INNER JOIN tblAsesorComercial B ON (B.Nombre + ' ' + B.Apellidos) = A.Asesor WHERE A.Nombre_Diseño LIKE '%' + @NombreDiseño + '%' AND B.Cedula = @Cedula">
                                            <SelectParameters>
                                                <asp:ControlParameter Name="NombreDiseño" ControlID="TextBox5" PropertyName="Text" />
                                                <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                        <asp:SqlDataSource runat="server" ID="SqlDataSourceCliente" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                            SelectCommand="SELECT A.CotizaciónOK, A.ProgramadoVentas, A.PasarACotizar, A.TerminadoDibujo, A.Pausado, A.Numero_Diseño, A.Nombre_Diseño,  A.Asesor, A.Fecha_Ingreso, A.Fecha_Programada_Entrega, A.UltimaActivacion, A.Cliente, B.Cedula FROM tblDiseño A INNER JOIN tblAsesorComercial B ON (B.Nombre + ' ' + B.Apellidos) = A.Asesor WHERE A.Cliente LIKE '%' + @Cliente + '%' AND B.Cedula = @Cedula">
                                            <SelectParameters>
                                                <asp:ControlParameter Name="Cliente" ControlID="TextBox4" PropertyName="Text" />
                                                <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" />
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

                                <%-- BOTONES--%>
                                <div class="collapse navbar-collapse" id="ejemplo2">
                                    <ul class="navbar-nav mx-auto contenedor-icono">
                                        <div class="contenedor-icono ">


                                            <asp:LinkButton runat="server" ID="NuevoDisBit" Enabled="false" OnClick="NuevoDisBit_Click">
                                                <i class="bi bi-file-earmark"></i> 
                                            </asp:LinkButton>

                                            <!-- Modal -->
                                            <div class="modal fade" id="modall" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                                <div class="modal-dialog modal-dialog-centered">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h5 class="modal-title  d-flex align-items-center justify-content-center" id="modallLabel">Dejar Infomación</h5>
                                                            <asp:button type="button" runat="server" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></asp:button>
                                                        </div>
                                                        <div class="modal-body d-flex align-items-center justify-content-center">
                                                            <h6>Desea Limpiar los campos del diseño?</h6>
                                                        </div>
                                                        <div class="modal-footer  d-flex align-items-center justify-content-center">
                                                            <asp:Button runat="server" type="button" class="btn btn-secondary" data-bs-dismiss="modal" OnClick="SiButton_Click" Text="Si"></asp:Button>
                                                            <asp:Button runat="server" type="button" class="btn btn-secondary" data-bs-dismiss="modal" Text="No" OnClick="NoButton_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <asp:LinkButton runat="server" ID="Grabar" Enabled="false" OnClick="btnInsertar_Click">
                                               <i class="bi bi-save2"></i>
                                            </asp:LinkButton>

                                            <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje"></asp:Label>

                                            <asp:LinkButton runat="server" ID="Modificar" Enabled="false" OnClick="Modificar_Click">
                                               <i class="bi bi-wrench"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="DocBitacora" Enabled="false" Onclick="DocBitacora_Click">
                                            <i class="bi bi-send-plus"></i>
                                            </asp:LinkButton>



                                            <asp:LinkButton runat="server" ID="RegresarDiseño" Enabled="false">
                                                  <i class="bi bi-box-arrow-in-left"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="AdicionarElemento" Enabled="false">
                                              <i class="bi bi-file-earmark-spreadsheet"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="ActualizarDiseno" Enabled="false">
                                              <i class="bi bi-arrow-right-square"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="PausarDiseño" Enabled="false">
                                              <i class="bi bi-stop-circle"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="Cancelar" Enabled="false" OnClick="Cancelar_Click">
                                               <i class="bi bi-x-lg"></i>
                                            </asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="EliminarDiseño" Enabled="false">
                                                    <i class="bi bi-trash"></i>
                                            </asp:LinkButton>




                                        </div>
                                    </ul>
                                </div>
                            </nav>
                        </div>

                        <%-- ADJUNTAR DOCUMENTACION--%>
                        <div id="Documentacion" runat="server" style="display: none">
                            <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="container">
                                        <h6>Documentación bitacora</h6>


                                        <div class="row">

                                            <div class="col-md-12 col-12">
                                                <div class="container-fluid">
                                                    <div class="row justify-content-center">
                                                        <div class="border rounded p-1 special-border col-md-12 col-12" style="height: auto; min-height: 300px;">

                                                            <div class="table-responsive mb-2 gap-2" style="height: auto; min-height: 300px; overflow-x: auto;">

                                                                <asp:DataGrid CssClass="table table-bordered table-sm custom-grid table-hover custom-data-grid form-control-sm"
                                                                    ID="DataGridDocumento" runat="server" AutoGenerateColumns="false"
                                                                    DataSourceID="SqlDataSource3" DataKeyField="Id_OT">
                                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                    <Columns>
                                                                        <asp:TemplateColumn>
                                                                            <ItemTemplate>

                                                                                <asp:LinkButton ID="lnkSelectRow" runat="server"
                                                                                    CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" OnClick="lnkSelectRow_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="Archivo" HeaderText="Archivo" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Observacion" HeaderText="Observacion" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="FechaRegistro" HeaderText="Fecha" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:BoundColumn DataField="Id_OT" Visible="false" ItemStyle-CssClass="auto-width-column" />
                                                                        <asp:TemplateColumn>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkViewFile" runat="server"
                                                                                    CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-eye'></i>" OnClick="lnkViewFile_Click"/>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                    </Columns>
                                                                </asp:DataGrid>

                                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                    SelectCommand="SELECT TOP 0 Archivo, Observacion, Usuario, FechaRegistro, Id_OT FROM tblDocumentacion"></asp:SqlDataSource>

                                                            </div>


                                                        </div>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="row mt-1">
                                                <div class="col-md-12 col-12">
                                                    <div class="input-group input-group-sm ">
                                                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />

                                                        <asp:Button ID="GuardarButton" runat="server" Text="Guardar" OnClick="GuardarButton_Click" />
                                                        <asp:Button ID="BtnEliminar" runat="server" Text="Eliminar" OnClick="BtnEliminar_Click" />

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row mt-1">
                                                <div class="col-md-12 col-12">
                                                    <textarea id="TextArea1" runat="server" class="form-control"></textarea>
                                                </div>
                                                <div class="col-md-1 col-2">
                                                </div>
                                            </div>
                                            <div class="row mt-1">
                                                <div class="col-md-12 col-12">
                                                </div>
                                            </div>
                                            <div class="row justify-content-end mt-1">
                                                <div class="col-md-1 col-1">
                                                </div>
                                                <div class="col-md-1 col-1">
                                                </div>
                                                <div class="col-md-1 col-1">
                                                </div>
                                            </div>



                                            <div class="modal fade" id="miModalDoc" tabindex="-1" role="dialog" aria-labelledby="miModalLabel" aria-hidden="true">
                                                <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h5 class="modal-title" id="miModalLabelDoc">Título del Modal</h5>


                                                        </div>
                                                        <div class="modal-body">
                                                            <p>Desea Guardar el archivo?</p>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Button runat="server" type="button" data-dismiss="modal" Text="Si"></asp:Button>
                                                            <asp:Button runat="server" type="button" data-dismiss="modal" Text="No"></asp:Button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="GuardarButton" />
                                    <asp:PostBackTrigger ControlID="BtnEliminar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>

                        <div id="miDiv" runat="server" data-div="miDiv" style="display: block">
                            <%--  1/4--%>
                            <div class="container-fluid m-1">
                                <div class="row justify-content-center">
                                    <div class="border rounded p-1">

                                        <div class="row">
                                            <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">

                                                <div class="row d-flex justify-content-between mt-2">
                                                    <div class="col-md-8 col-6">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:Label runat="server" class="col-form-label-sm">Diseño#:</asp:Label>
                                                            <asp:Label ID="lblNumDise" runat="server" Style="font-size: 20px; color: black; font-weight: bold; margin-bottom: 10px; font-family: 'Times New Roman'">Número</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-3">
                                                        <asp:Button runat="server" ID="BtnBus" type="button" OnClientClick="mostrarTab(); return false;" class="btn-outline-dark btn btn-white m-2 shadow btn-sm" Text="..." />
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-12 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Button runat="server" ID="Button1" CssClass="btn-outline-dark btn btn-white" Text="Cliente" OnClientClick="abrirOtraPestaña();" />
                                                                <asp:TextBox ID="TextCliente" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row d-flex justify-content-between mt-1">
                                                        <div class="col-md-7 col-12">
                                                            <div class="input-group input-group-sm gap-2">
                                                                <asp:Label runat="server" ID="lblDir" class="col-form-label-sm">Dir</asp:Label>
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
                                           <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-6 col-6">
                                                        <asp:Label runat="server" ID="lblIngDis" class="col-form-label-sm">Ingreso de Diseño</asp:Label>
                                                        <asp:TextBox ID="TextIngDis" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6 col-6">
                                                        <asp:Label runat="server" ID="lblUltAct" class="col-form-label-sm">Ultima Activacion</asp:Label>
                                                        <asp:TextBox ID="TextUltAc" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
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
                                           <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-5 col-6">
                                                        <asp:Label runat="server" ID="lblEnt" class="col-form-label-sm">Entrega</asp:Label>
                                                        <asp:TextBox ID="TextEntrega" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-5 col-6">
                                                        <asp:Label runat="server" ID="lblEntDib" class="col-form-label-sm">Fecha Ok Dibujo</asp:Label>
                                                        <asp:TextBox ID="TextFecOkDib" runat="server" CssClass="form-control form-control-sm" type="datetime-local"></asp:TextBox>
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


                                           <div class="col-lg-3 col-md-6 col-sm-6 col-xs-12">
                                                <div class="row d-flex justify-content-between mt-1">
                                                    <div class="col-md-6 col-12">
                                                        <asp:Label runat="server" ID="lblAse" class="col-form-label-sm">Asesor</asp:Label>
                                                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control form-control-sm" Enabled="false" DataSourceID="SqlDataSource2" DataTextField="NombreCompleto" DataValueField="Cedula">
                                                        </asp:DropDownList>

                                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"
                                                            SelectCommand="SELECT Cedula, Nombre + ' ' + Apellidos AS NombreCompleto FROM tblAsesorComercial WHERE Activo = '1' ORDER BY nombre ASC;"></asp:SqlDataSource>


                                                    </div>
                                                    <div class="col-md-6 col-12">
                                                        <asp:Label runat="server" ID="lblPre" class="col-form-label-sm">Pre</asp:Label>
                                                        <asp:TextBox ID="TextPre" runat="server" CssClass="form-control"></asp:TextBox>






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
                                                                <asp:DropDownList ID="TextCiuPro" runat="server" DataSourceID="sqlDataSourceCiudades"
                                                                    DataTextField="CiudadDepartamento" DataValueField="id_Ciudad_Aut" />

                                                                <asp:SqlDataSource ID="sqlDataSourceCiudades" runat="server" ConnectionString="Data Source=172.16.30.3;Initial Catalog=BD_SIDSQL_PRUEBA;User ID=pcadmin;Password=password"
                                                                    SelectCommand="SELECT tblCiudad.id_Ciudad_Aut, tblCiudad.NombreCiudad + ' - ' + tblDepartamentoPais.NombreDepartamento AS CiudadDepartamento
                                                                        FROM tblCiudad
                                                                        INNER JOIN tblCostoTransporte ON tblCiudad.id_Ciudad_Aut = tblCostoTransporte.tte_ID_Ciudad
                                                                        INNER JOIN tblDepartamentoPais ON tblCiudad.Id_Departamento = tblDepartamentoPais.Id_Departamento_Auto
                                                                        GROUP BY tblCiudad.id_Ciudad_Aut, tblCiudad.NombreCiudad + ' - ' + tblDepartamentoPais.NombreDepartamento
                                                                        ORDER BY tblCiudad.NombreCiudad + ' - ' + tblDepartamentoPais.NombreDepartamento;"></asp:SqlDataSource>


                                                            </div>
                                                        </div>
                                                        <div class="col-md-5 col-4 mt-1">
                                                            <asp:Button runat="server" ID="BtnProgramar" CssClass="btn-outline-dark btn btn-sm btn-white" Text="PROGRAMAR" OnClick="BtnProgramar_Click" Enabled="false" />
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
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12 border">
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
                                                            <asp:Label ID="lblCan" runat="server" class="col-form-label-sm">Canaleta</asp:Label>
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
                                           <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12 border">
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
                                             <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 border">
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
                                             <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12 border">
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="lblMue" runat="server" class="form-label" Style="font-size: 16px; font-weight: bold;">Muebles</asp:Label>
                                                        <asp:CheckBox ID="ChecMue" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="col-md-10 col-12">
                                                    <div class="input-group input-group-sm mt-1 gap-2">
                                                        <asp:Label ID="lblCoc" runat="server" class="col-form-label-sm">Coco</asp:Label>
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
                                             <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                <h6>Observaciones Ventas</h6>
                                                <textarea id="TextObsVen" class="form-control form-control-sm" style="height: 100px" runat="server"></textarea>
                                            </div>
                                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                <h6>Observaciones de Dibujo y Despiece</h6>
                                                <textarea id="TextObsDibDes" class="form-control form-control-sm" style="height: 100px" runat="server"></textarea>
                                            </div>
                                            <div class="col-lg-4 col-md-6 col-sm-6 col-xs-12">
                                                <h6>Seguimiento de Pausas y Devoluciones</h6>
                                                <textarea id="TextSegPauDev" class="form-control form-control-sm" style="height: 100px" runat="server"></textarea>
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
                                            <div class="col-lg-2 col-md-6 col-sm-6 col-xs-12">
                                                <div class="border rounded p-1" style="height: 250px">
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
                                                            <asp:CheckBox ID="CheckBox21" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox21_CheckedChanged"/>
                                                            <asp:Label ID="lblTieRea" runat="server" class="col-form-label-sm">Tiempo Real</asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-10 col-12">
                                                        <div class="input-group input-group-sm gap-2">
                                                            <asp:TextBox ID="TextFec" runat="server" CssClass="form-control-sm form-control" type="date"></asp:TextBox>
                                                            <asp:TextBox ID="TextFech" runat="server" CssClass="form-control-sm form-control" type="time"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="col-md-10 col-12">
                                                        <asp:Label ID="lblUbi" runat="server" class="col-form-label-sm">Ubicación</asp:Label>
                                                        <asp:TextBox ID="TextUbi" runat="server" CssClass="form-control-sm form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-10 col-md-6 col-sm-6 col-xs-12">
                                                <div class="container-fluid">
                                                    <div class="row justify-content-center">

                                                        <div class="border rounded p-1" style="height: 250px; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-sm table-bordered table-hover form-control-sm" ID="DataGrid5" runat="server" DataSourceID="SqldatasourceTxt" AutoGenerateColumns="false">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="id_PlanoDiseno" HeaderText="ID" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Plano" HeaderText="Plano" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Area" HeaderText="Area" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SubTotalZona" HeaderText="Valor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cantidad" HeaderText="Cant" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SubTotalZona" HeaderText="Sub Total" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Opcion" HeaderText="Opc" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Observacion" HeaderText="Observacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="RealizadoPor" HeaderText="Realizado Por" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Composicion" HeaderText="Composición" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="FechalecturaDespiece" HeaderText="Despiece" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>

                                                                </Columns>
                                                                <ItemStyle CssClass="fila-verde" />
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="SqldatasourceTxt" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="SELECT pd.[id_PlanoDiseno], pd.[Plano], p.[RealizadoPor], p.[Area], pd.[SubTotalZona], pd.[Cantidad], pd.[SubTotalZona], pd.[Opcion], pd.[Observacion], pd.[Composicion], pd.[FechalecturaDespiece]
                                                                   FROM [tblPlanoDiseño] pd
                                                                   INNER JOIN [tblPlano] p ON pd.[Plano] = p.[Plano]
                                                                   WHERE (pd.[Numero_Diseño] = @NumeroDiseño)"
                                                                DataSourceMode="DataSet">
                                                                <SelectParameters>
                                                                    <asp:Parameter Name="NumeroDiseño" Type="Int32" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>



                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Button runat="server" ID="Button2" Text="N" Enabled="false" />
                                        <asp:Button runat="server" ID="ButtonD" Text="N" Enabled="false" />
                                        <asp:Button runat="server" ID="ButtonE" Text="N" Enabled="false" />
                                        <asp:Button runat="server" ID="ButtonS" Text="N" Enabled="false" />
                                        <asp:Button runat="server" ID="ButtonV" Text="N" Enabled="false" />
                                    </div>
                                </div>
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
                                                            <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGrid1" runat="server" DataSourceID="SqlDataSource1"
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
                                                                    <asp:TemplateColumn HeaderText="Nueva Columna" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToDateTime(Eval("Fecha_Entrega_Dibujo_Despiece")).AddDays(2).ToString("dd/MM/yyyy hh:mm:ss tt") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbDibujante" runat="server" Text='<%# Eval("RealizadoPor") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>

                                                                    <asp:BoundColumn DataField="Fecha_Despacho_Produccion" HeaderText="F.Despacho" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="sp_ProBitacoraOTs" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>

                                                                    <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" DefaultValue="ValorPorDefecto" />

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
                                                    <asp:LinkButton ID="LinkButton8" runat="server"
                                                        OnClick="Button88_Click">
                                                       <i class="bi bi-arrow-clockwise text-dark"></i>
                                                    </asp:LinkButton>



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
                                                    <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                                                        <div class="modal-content">
                                                            <div class="modal-body">
                                                                <h6>Resumen Dibujante</h6>
                                                                <div class="row">
                                                                    <div class="border rounded">
                                                                        <div class="table-responsive" style="max-height: 400px">
                                                                            <asp:DataGrid Class="table table-bordered table-hover table-sm" ID="DataGrid3" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource4">
                                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                                <Columns>
                                                                                    <asp:BoundColumn HeaderText="Dibujante" DataField="RealizadoPor" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn HeaderText="Ped" DataField="CantidadOt" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn HeaderText="Ult.Pedido" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn HeaderText="Dis" DataField="CantidadRepeticiones" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn HeaderText="Ultimo Diseño" ItemStyle-CssClass="auto-width-column" />
                                                                                    <asp:BoundColumn HeaderText="Total" DataField="Total" ItemStyle-CssClass="auto-width-column" />
                                                                                </Columns>
                                                                            </asp:DataGrid>

                                                                            <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                                SelectCommand="sp_ResumenDibujante" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button9" runat="server" Text="Trabajar Pedido" CssClass="btn-outline-dark btn btn-white btn-sm btn" OnClick="Button9_Click" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-12 mt-1">
                                                        <asp:Button ID="Button10" runat="server" Text="Trabajar Pedido" CssClass="btn-outline-dark btn btn-white btn-sm btn" OnClick="Button10_Click" />
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
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid2" runat="server" DataSourceID="DataGridDiseño" AutoGenerateColumns="false"
                                                                OnItemDataBound="DataGrid2_ItemDataBound" OnItemCommand="DataGridDise_ItemCommand">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                     <asp:TemplateColumn ItemStyle-CssClass="auto-width-column">
                                                                            <ItemTemplate>

                                                                                <asp:LinkButton ID="lnkClie" runat="server" CommandName="Numero_Diseño"
                                                                                    CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" OnClick="lnkClie_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkSelectRow" runat="server" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Container.ItemIndex + 1 %>' CssClass="text-white text-decoration-none text-dark"/>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Numero_Diseño" HeaderText="Diseño" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Descripción" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Ult.Act" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="F.Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="Dibujante" ItemStyle-CssClass="auto-width-column">
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
                                                                    <asp:BoundColumn DataField="id_CiudadProyecto" HeaderText="Ciudad" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="Urgente" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridDiseño" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>"
                                                                SelectCommand="sp_ProBitacoraDise" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>

                                                                    <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" DefaultValue="ValorPorDefecto" />

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
                                                    <asp:Label runat="server" class="col-form-label-sm" Enabled="true">Pacto de entrega</asp:Label>
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
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGridDiseños" runat="server"
                                                                DataSourceID="DataGridDiseñosPorFecha" AutoGenerateColumns="false" OnItemDataBound="DataGrid3_ItemDataBound">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                      <asp:TemplateColumn>
                                                                            <ItemTemplate>

                                                                                <asp:LinkButton ID="lnkSelectRow" runat="server"
                                                                                    CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

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
                                                                    <asp:BoundColumn DataField="SC_tiemporeal" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Presentacionppt" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Accesorios" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="SC_Ubicacion" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridDiseñosPorFecha" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ProBitacoraShowCase" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>

                                                                    <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" DefaultValue="ValorPorDefecto" />

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
                                                            <asp:DataGrid CssClass="table table-bordered table-hover table-sm form-control-sm" ID="DataGridRender" runat="server"
                                                                DataSourceID="DataGridRenderPorFechaYAsesor" AutoGenerateColumns="false" OnItemDataBound="DataGrid4_ItemDataBound">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                       <asp:TemplateColumn>
                                                                            <ItemTemplate>

                                                                                <asp:LinkButton ID="lnkSelectRow" runat="server"
                                                                                    CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square text-white'></i>" OnClick="lnkSelectRowRender_Click" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>

                                                                    <asp:TemplateColumn HeaderText="Turno" ItemStyle-CssClass="auto-width-column">
                                                                        <ItemTemplate>
                                                                            <%# Container.ItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="Id_Render" HeaderText="ID" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Nombre-Render" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="UltimaActivacion" HeaderText="Activado" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Fecha_Programada_Entrega" HeaderText="Entrega" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Asesor" HeaderText="Asesor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="RealizadoPor" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Zona" HeaderText="Zona" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="TerminadoRender" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Pausado" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="ProgramadoVentas" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Cedula" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                               
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource runat="server" ID="DataGridRenderPorFechaYAsesor" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL_PRUEBA %>" SelectCommand="sp_ProBitacoraRender" SelectCommandType="StoredProcedure">
                                                                <SelectParameters>

                                                                    <asp:SessionParameter Name="Cedula" SessionField="CedulaLogeada" Type="String" DefaultValue="ValorPorDefecto" />

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
                                                    <asp:Button ID="Button15" runat="server" Text="Trabajar Render" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:Button ID="Button16" runat="server" Text="Desprogramar" class="btn-outline-dark btn btn-white btn-sm btn animate__animated animate__pulse" />
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



       <div class="modal fade" id="miModal" tabindex="-1" role="dialog" aria-labelledby="miModalLabel" aria-hidden="true">
           <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
               <div class="modal-content">
                   <div class="modal-header">
                       <h5 class="modal-title" id="miModalLabel">Título del Modal</h5>
                       <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                           <span aria-hidden="true">&times;</span>
                       </button>

                   </div>
                   <div class="modal-body">
                   </div>
                   <div class="modal-footer">
                   </div>
               </div>
           </div>
       </div>


       <div class="modal" id="miModalll" tabindex="-1" style="display: none;">
           <div class="modal-dialog">
               <div class="modal-content">
                   <div class="modal-header">
                       <h5 class="modal-title">Mensaje</h5>
                       <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                   </div>
                   <div class="modal-body">
                       <p>Falta llenar el campo: <span id="campoFaltante"></span></p>
                   </div>
                   <div class="modal-footer">
                       <!-- Puedes agregar botones u opciones aquí si es necesario -->
                   </div>
               </div>
           </div>
       </div>

       <div class="modal" id="miModalExito" tabindex="-1" style="display: none;">
           <div class="modal-dialog">
               <div class="modal-content">
                   <div class="modal-header">
                       <h5 class="modal-title">Mensaje</h5>
                       <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                   </div>
                   <div class="modal-body">
                       <p>Los datos se guardaron correctamente</p>
                   </div>
                   <div class="modal-footer">
                       <!-- Puedes agregar botones u opciones aquí si es necesario -->
                   </div>
               </div>
           </div>
       </div>

       <div class="modal" id="miModalError" tabindex="-1" style="display: none;">
           <div class="modal-dialog">
               <div class="modal-content">
                   <div class="modal-header">
                       <h5 class="modal-title">Mensaje</h5>
                       <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                   </div>
                   <div class="modal-body">
                       <p>No se guardaron los datos correctamente</p>
                   </div>
                   <div class="modal-footer">
                       <!-- Puedes agregar botones u opciones aquí si es necesario -->
                   </div>
               </div>
           </div>
       </div>

       <div class="modal" id="miModalErrorAdj" tabindex="-1" style="display: none;">
           <div class="modal-dialog">
               <div class="modal-content">
                   <div class="modal-header">
                       <h5 class="modal-title">Mensaje</h5>
                       <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                   </div>
                   <div class="modal-body">
                       <p>No se pudo encontrar el archivo</p>
                   </div>
                   <div class="modal-footer">
                       <!-- Puedes agregar botones u opciones aquí si es necesario -->
                   </div>
               </div>
           </div>
       </div>

    </form>





    <script type="text/javascript">
        function mostrarTab() {

            var tabElementt = document.getElementById('Buscar-tab');

            // Verifica si el tab ya está visible
            if (tabElementt.style.display === 'block') {
                // Oculta el tab
                tabElementt.style.display = 'none';
            } else {
                // Muestra el tab
                tabElementt.style.display = 'block';

                // Activa el tab
                $('#Buscar-tab').tab('show');
            }


        }

    </script>

    <script>
        function abrirOtraPestaña() {
            // Utiliza window.open para abrir "Formulario2.aspx" en otra pestaña
            window.open('Clientes.aspx', '_blank');
        }
    </script>







    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
