<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Licitaciones.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Licitaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <link rel="stylesheet" href="../../Recursos/CSS/Ventas/Licitaciones.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <title>Licitaciones</title>
</head>
<body>

    <form id="form2" runat="server">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <span class="navbar-brand mb-0 h1">Licitaciones</span>
            </div>
        </nav>

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="Detalle-tab" data-bs-toggle="tab" href="#Detalle-Licitacion">Detalle Licitaciones</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Info-tab" data-bs-toggle="tab" href="#Info-Licitacion">Infor. General Licitaciones</a>
                    </li>
                </ul>
            </div>
        </nav>

        <nav class="navbar navbar-expand-sm navbar-light bg-light mb-3 gap-2">
            <div class="container-fluid">

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#ejemplo2" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="ejemplo2">
                    <ul class="navbar-nav mx-auto contenedor-icono">



                        <div class="contenedor-icono">



                            <%--Comienza Nueva OT--%>


                            <a class="icong disabled" title="Nueva Licitacion" id="NuevaLic" runat="server" onclick="NuevaLic()">
                                <i class="bi bi-file-earmark"></i>
                            </a>
                            <asp:LinkButton class="icong disabled" runat="server" title="Grabar Licitacion" ID="GrabarLic" OnClick="Grabar">
                                 <i class="bi bi-save2"></i>
                            </asp:LinkButton>

                            <asp:CheckBox ID="estadoLicitacion" runat="server"  />


                            <a class="icong disabled" href="#" title="Modificar Licitacion" id="ModificarLic" runat="server" onclick="ModificarLic()">
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

            <div class="tab-pane fade show active" id="Detalle-Licitacion">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="container p-1">



                            <div class="row pb-1">


                                <div class="col-1">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <label class="form-label" runat="server" id="lbAsesor">Asesor</label>

                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">

                                        <asp:DropDownList class="form-control" ID="ddlAsesor" runat="server" disabled="disabled" DataTextField="Asesor" DataValueField="Asesor" DataSourceID="CargarAsesor" OnDataBound="ddlAsesores_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="CargarAsesor" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select Apellidos + ' ' + nombre as Asesor  from tblAsesorComercial where activo = 1 order by Apellidos + ' ' + nombre asc"></asp:SqlDataSource>

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
                                        <asp:DropDownList class="form-control" ID="ddlEstado" runat="server" disabled="false" DataTextField="Estado" DataValueField="Estado" DataSourceID="EstadoLic" OnDataBound="ddlEstado_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="EstadoLic" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select EstadoLicitacion As Estado, Id_EstadoLicitacion As IdLicitacion from tblEstadoLicitacion where activa = 1 order by id_EstadoLicitacion asc"></asp:SqlDataSource>
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

                                        <asp:DropDownList class="form-control" ID="ddlProceso" runat="server" disabled="false" DataTextField="proceso" DataValueField="proceso" DataSourceID="ProcesoLic" OnDataBound="ddlProceso_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="ProcesoLic" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select DescripcionProceso As proceso, Id_ProcesoLicitacion As IdProceso from tblProcesoLicitacion where Activa =1 order by id_ProcesoLicitacion asc "></asp:SqlDataSource>

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
                                        <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server" disabled="false" DataTextField="NombreCiudad" DataValueField="NombreCiudad" DataSourceID="CargarCiudad" OnDataBound="ddlCiudad_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="CargarCiudad" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="SELECT 
                                            CONCAT(tblDepartamentoPais.CodigoDepartamento ,
                                            tblCiudad.CodigoCiudad)   AS CodCompleto,
                                            tblCiudad.NombreCiudad +'/'+tblDepartamentoPais.NombreDepartamento As NombreCiudad
                                            FROM tblDepartamentoPais 
                                            INNER JOIN tblCiudad
                                            ON tblDepartamentoPais.Id_Departamento_Auto = tblCiudad.Id_Departamento 
                                            ORDER BY CONCAT(tblCiudad.NombreCiudad , ' - ' , tblDepartamentoPais.NombreDepartamento)"></asp:SqlDataSource>

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
                                        <asp:DropDownList class="form-control" ID="ddlCiudadP" runat="server" disabled="false" DataSourceID="CargarCiudad" DataTextField="NombreCiudad" DataValueField="NombreCiudad" OnDataBound="ddlCiudad1_DataBound"></asp:DropDownList>


                                    </div>
                                </div>


                            </div>




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

                                        <asp:DropDownList class="form-control" ID="ddlCausa" runat="server" disabled="false" DataTextField="NombreCausa" DataValueField="NombreCausa" DataSourceID="Causas" OnDataBound="ddlCausa_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="Causas" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select CausaEstadoLicitacion As NombreCausa, Id_CausaEstadoLicitacion IdCausa   from tblCausaEstadoLicitacion where activa = 1 order by CausaEstadoLicitacion asc"></asp:SqlDataSource>


                                    </div>
                                </div>


                            </div>




                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

            <div class="tab-pane fade " id="Info-Licitacion">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="container p-1">


                            <div class="row">
                                <div class="col-2">
                                    <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                        <label class="form-label" runat="server" id="lbLicitacion">Licitación</label>
                                        <asp:TextBox type="text" ID="tbLicitacion1" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-4">
                                    <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                        <label class="form-label" runat="server" id="lbFechaClau">Clausura entre</label>
                                        <asp:TextBox type="date" ID="tbFechaClau" runat="server" class="form-control"></asp:TextBox>
                                        <asp:TextBox type="date" ID="tbFechaClau1" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-3">
                                    <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                        <label class="form-label" runat="server" id="lbEstado1">Estado</label>
                                        <asp:DropDownList class="form-control" ID="ddlEstado1" runat="server" DataTextField="Estado" DataValueField="Estado" DataSourceID="Estados"  OnDataBound="ddlEstado1_DataBound"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="Estados" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand=" select EstadoLicitacion As Estado, Id_EstadoLicitacion As IdLicitacion from tblEstadoLicitacion where activa = 1 order by id_EstadoLicitacion asc"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <div class="col-1 ">
                                </div>

                                <div class="col-2 ">
                                    <div class="input-group input-group-sm  input-group-sm mb-2 gap-2">
                                        <asp:Button class="btn btn-outline-secondary" ID="btn1" type="button" Text="..." runat="server" OnClick="btn1_Click" />
                                        <asp:LinkButton class="btn btn-outline-secondary" Text='<i class="bi bi-printer"></i>' ID="btn3" runat="server"></asp:LinkButton>

                                    </div>
                                </div>

                            </div>

                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row">
                                        <div class="col-12">

                                            <div class="table-responsive mb-2 gap-2" style="max-height: 18rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-center">Licitaciones en Proceso</h5>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid1" runat="server" DataSourceID="LicitacionProceso" AutoGenerateColumns="false" OnItemCommand="dataGrid1_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="Ver" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="Cod_Licitacion" HeaderText="Cod" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="NombreCompania" HeaderText="Cliente" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="FechaAperturaLicitacion" HeaderText="Apertura" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="FechaClausuraLicitacion" HeaderText="Clausura" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ValorLicitacion" HeaderText="Valor" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ValorPresupuesto" HeaderText="Presupuesto" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="LicFechaLimiteObservaciones" HeaderText="Lim. Bbs" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="LicFechaLimiteAdendas" HeaderText="Lim Adendas" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="DescripcionProceso" HeaderText="Proceso" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Asesor" Visible="false" />
                                                        <asp:BoundColumn DataField="NombreContacto" Visible="false" />
                                                        <asp:BoundColumn DataField="Telefono" Visible="false" />
                                                        <asp:BoundColumn DataField="LicCelularContacto" Visible="false" />
                                                        <asp:BoundColumn DataField="Correo_Electrónico" Visible="false" />
                                                        <asp:BoundColumn DataField="Id_Licitacion" Visible="false" />
                                                        <asp:BoundColumn DataField="LicFechadeRegistro" Visible="false" />
                                                        <asp:BoundColumn DataField="EstadoLicitacion" Visible="false" />
                                                        <asp:BoundColumn DataField="LicDireccionEntrega" Visible="false" />
                                                        <asp:BoundColumn DataField="LicCiudadDepartamentoEntrega" Visible="false" />
                                                        <asp:BoundColumn DataField="LicLinklicitacion" Visible="false" />
                                                        <asp:BoundColumn DataField="LicNombreProyecto" Visible="false" />
                                                        <asp:BoundColumn DataField="LicDireccionProyecto" Visible="false" />
                                                        <asp:BoundColumn DataField="LicCiudadDepartamentoProyecto" Visible="false" />
                                                        <asp:BoundColumn DataField="LicObservacion" Visible="false" />
                                                        <asp:BoundColumn DataField="CausaEstadoLicitacion" Visible="false" />
                                                    </Columns>
                                                </asp:DataGrid>
                                            </div>

                                            <asp:SqlDataSource runat="server" ID="LicitacionProceso" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Sp_LicitacionProceso" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddlEstado1" PropertyName="SelectedValue" Name="estadoLicitacion" Type="String"></asp:ControlParameter>
                                                    <asp:ControlParameter ControlID="tbFechaClau" PropertyName="Text" DbType="Date" Name="fechaInicio"></asp:ControlParameter>
                                                    <asp:ControlParameter ControlID="tbFechaClau1" PropertyName="Text" DbType="Date" Name="fechaFin"></asp:ControlParameter>
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                            <asp:SqlDataSource ID="LicitacionProceso2" runat="server" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="Sp_LicitacionProceso2" SelectCommandType="StoredProcedure">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="tbLicitacion1" PropertyName="Text" Name="CodLicitacion" Type="String"></asp:ControlParameter>
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </div>
                                        <div class="col-12"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="row justify-content-center">
                                <div class="border rounded p-2">
                                    <div class="row mt-3">
                                        <div class="col-6">

                                            <div class="table-responsive mb-2 gap-2" style="max-height: 14rem; overflow-x: auto;">
                                                <h5 class="datagrid-header text-left">Causas</h5>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid2" runat="server" DataSourceID="CausaLicitacion" AutoGenerateColumns="false" AutoPostBack="true">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Causa" HeaderText="Causa" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="ContCausa" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="TotalCausa" HeaderText="Total Causas" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Porcentaje" HeaderText="%" ItemStyle-CssClass="auto-width-column" />


                                                    </Columns>
                                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="CausaLicitacion" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="sp_CausasLicitacion" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddlEstado1" PropertyName="SelectedValue" Name="EstadoLicitacion" Type="String"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaClau" PropertyName="Text" DbType="Date" Name="FechaInicio"></asp:ControlParameter>
                                                        <asp:ControlParameter ControlID="tbFechaClau1" PropertyName="Text" DbType="Date" Name="FechaFin"></asp:ControlParameter>
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>



                                        </div>
                                        <div class="col-12"></div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

        </div>

    </form>

    <script>
        // Ocultar el div con clase "contenedor-icono" cuando se activa la pestaña "Info-content" 
        $(document).ready(function () {
            $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var targetTab = $(e.target).attr("href");
                if (targetTab === "#Info-Licitacion") {
                    $(".contenedor-icono").hide();
                } else {
                    $(".contenedor-icono").show();
                }
            });
        });


    </script>


    <script>

        // Habilitar enlace nueca licitacion al inicio
        document.getElementById("NuevaLic").classList.add("enabled");

        var checkBox = document.getElementById('<%= estadoLicitacion.ClientID %>');
        checkBox.style.display = 'none'; // Ocultar el checkbox



        // Función para activar los textBox y activar o desactivar los links  
        function NuevaLic() {

            // Deshabilitar enlaces
            document.getElementById("NuevaLic").classList.remove("enabled");


            // Habilitar enlaces
            document.getElementById("GrabarLic").classList.add("enabled");
            document.getElementById("CancelarLic").classList.add("enabled");


            // Habilitar o deshabilitar los TextBox Type text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbID") {

                    if (textBoxes[i].id !== "tbLicitacion1") {
                        textBoxes[i].disabled = !textBoxes[i].disabled;
                    }

                }
            }
            // Habilitar o deshabilitar los TextBox type date
            var textBoxes = document.querySelectorAll("input[type='date']");
            for (var i = 0; i < textBoxes.length; i++) {
                if (textBoxes[i].id !== "tbFechaClau") {
                    if (textBoxes[i].id !== "tbFechaClau1") {
                        textBoxes[i].disabled = !textBoxes[i].disabled;
                    }
                }
            }



            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {
                if (dropDownLists[j].id !== "ddlEstado1") {
                    dropDownLists[j].disabled = !dropDownLists[j].disabled;
                }
            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {
                textAreas[k].disabled = !textAreas[k].disabled;
            }

            var checkBox = document.getElementById('<%= estadoLicitacion.ClientID %>');

            if (checkBox.checked === false) {
                    checkBox.checked = true;
                }
           

          




        }

        // Función para Desactivar los textBox y activar o desactivar los links  
        function Cancelar() {


            // Deshabilitar enlaces
            document.getElementById("GrabarLic").classList.remove("enabled");
            document.getElementById("CancelarLic").classList.remove("enabled");
            document.getElementById("ModificarLic").classList.remove("enabled");


            // Habilitar enlaces
            document.getElementById("NuevaLic").classList.add("enabled");

            // Habilitar o deshabilitar los TextBox Type Text
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbLicitacion1") {
                    textBoxes[i].disabled = true;
                    textBoxes[i].value = "";

                }

            }

            // Habilitar o deshabilitar los TextBox Type date
            var textBoxes = document.querySelectorAll("input[type='date']");
            for (var i = 0; i < textBoxes.length; i++) {
                if (textBoxes[i].id !== "tbFechaClau") {
                    if (textBoxes[i].id !== "tbFechaClau1") {
                        textBoxes[i].disabled = true;
                        textBoxes[i].value = "";
                    }

                }
            }

            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {

                if (dropDownLists[j].id !== "ddlEstado1") {
                    dropDownLists[j].disabled = true;
                    dropDownLists[j].value = "";
                }

            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {
                textAreas[k].disabled = true;
                textAreas[k].value = "";
            }






        }

        // Funcion para activar la modificacion de la licitacion  ok
        function ModificarLic() {
            // Deshabilitar enlaces
            document.getElementById("NuevaLic").classList.remove("enabled");
            document.getElementById("ModificarLic").classList.remove("enabled");

            // Habilitar enlaces
            document.getElementById("GrabarLic").classList.add("enabled");
            document.getElementById("CancelarLic").classList.add("enabled");


            // Habilitar o deshabilitar los TextBox
            var textBoxes = document.querySelectorAll("input[type='text']");
            for (var i = 0; i < textBoxes.length; i++) {

                if (textBoxes[i].id !== "tbID") {

                    if (textBoxes[i].id !== "tbLicitacion1") {
                        textBoxes[i].disabled = !textBoxes[i].disabled;

                    }
                }

            }
            // Habilitar o deshabilitar los TextBox
            var textBoxes = document.querySelectorAll("input[type='date']");
            for (var i = 0; i < textBoxes.length; i++) {
                if (textBoxes[i].id !== "tbFechaClau") {
                    if (textBoxes[i].id !== "tbFechaClau1") {
                        textBoxes[i].disabled = !textBoxes[i].disabled;
                    }
                }
            }


            // Habilitar o deshabilitar los DropDownList
            var dropDownLists = document.querySelectorAll("select");
            for (var j = 0; j < dropDownLists.length; j++) {
                if (dropDownLists[j].id !== "ddlEstado1") {
                    dropDownLists[j].disabled = !dropDownLists[j].disabled;
                }
            }

            // Habilitar los TextArea
            var textAreas = document.querySelectorAll("textarea");
            for (var k = 0; k < textAreas.length; k++) {
                textAreas[k].disabled = !textAreas[k].disabled;
            }

            var checkBox = document.getElementById('<%= estadoLicitacion.ClientID %>');
                if (checkBox.checked === true) {
                    checkBox.checked = false;
                }
           




        }

        //Funcion para habilitar Modificar y cancelar  ok
        function HabilitarEnlaces1() {

            // Habilitar enlaces
            document.getElementById("ModificarLic").classList.add("enabled");
            document.getElementById("CancelarLic").classList.add("enabled");
        }

    </script>




    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>

</body>
</html>
