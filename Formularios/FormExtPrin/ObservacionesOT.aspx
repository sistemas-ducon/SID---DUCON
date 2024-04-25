<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObservacionesOT.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.FormExtPrin.ObservacionesOT" %>

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

    <link type="text/css" href="../../Recursos/CSS/FormExtPrin/ObservacionesOT.css" rel="stylesheet" />

    <title>Observaciones OT</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs" id="myTabs">

                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Observaciones-tab" data-bs-toggle="tab" href="#Observaciones-content">Observaciones a la OT</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Personales-tab" data-bs-toggle="tab" href="#Personales-content">Observaciones Personales</a>
                    </li>


                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Todas-tab" data-bs-toggle="tab" href="#Todas-content">Todas las Observaciones</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Pendientes-tab" data-bs-toggle="tab" href="#Pendientes-content">Actividades Pendientes</a>
                    </li>

                </ul>
            </div>
        </nav>
        <div class="tab-content" id="myTabContent">

            <div class="tab-pane fade show active" id="Observaciones-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <div class="d-flex">
                                <div class="col-lg-7 col-md-6 col-sm-12 col-xs-12">

                                    <div class="p-3 m-2 border" style="height: 22rem;">

                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <div class="row justify-content-center">
                                                    <div class="border rounded p-1 special-border" style="height: auto; min-height: 17rem;">
                                                        <%-- DATAGRID--%>

                                                        <div class="table-responsive mb-2 gap-2" style="max-height: 15rem; overflow-x: auto;">
                                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm"
                                                                ID="DataGrid1" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource1"
                                                                DataKeyField="Id_Observacion" OnItemCommand="DataGrid1_ItemCommand" OnSelectedIndexChanged="DataGrid1_SelectedIndexChanged">
                                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                                <Columns>
                                                                    <asp:TemplateColumn>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="SelecOt" CssClass="Tam" runat="server" CommandName="Select" CommandArgument='<%# Container.ItemIndex %>'
                                                                                Text="<i class='bi bi-pencil-square'></i>" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn HeaderText="OT" DataField="Id_OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="Ped" DataField="Consecutivo_Pedido" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn HeaderText="Tipo Observacion" DataField="Aplicacion" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn HeaderText="Descripcion" DataField="Descripcion" ItemStyle-CssClass="auto-width-column" />
                                                                    <asp:BoundColumn HeaderText="Fecha Obs." DataField="FechaObservacion" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn HeaderText="F.Ingreso" DataField="Nombre_Emisor" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Observacion" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Id_Observacion" ItemStyle-CssClass="auto-width-column" Visible="false"></asp:BoundColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL%>"
                                                                SelectCommand="SELECT O.Id_OT, O.Consecutivo_Pedido, O.FechaObservacion, O.Nombre_Emisor, T.Aplicacion, T.Descripcion, O.Observacion, O.Id_Observacion
                                                               FROM tblOTObservacion AS O
                                                               INNER JOIN tblTipoObservacion AS T ON O.ID_TipoObservacion = T.ID_TipoObservacion
                                                               WHERE Id_OT = @Id_OT ORDER BY O.Id_OT, O.Consecutivo_Pedido DESC, O.FechaObservacion DESC ">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="Id_OT" SessionField="Id_OT" Type="String" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row mt-2">
                                            <div class="col-lg-7 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label ID="Label1" runat="server" CssClass="col-form-label-sm fw-bold" Text="T.Obs."></asp:Label>
                                                    <asp:DropDownList ID="ddlTipoObservacion" runat="server" CssClass="form-control form-control-sm" OnSelectedIndexChanged="ddlTipoObservacion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                                                   
                                                </div>
                                            </div>
                                            <div class="col-lg-5 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label ID="Label2" runat="server" CssClass="col-form-label-sm" Text="F.Actividad"></asp:Label>
                                                    <asp:TextBox ID="tbfechaActividad" runat="server" CssClass="form-control form-control-sm" type="date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="p-2 m- border" style="height: 24rem;">
                                        <h6>Observación</h6>
                                        <textarea id="txObservacion" runat="server" class="form-control form-control-sm" style="height: 10rem;"> </textarea>


                                        <div class="border rounded p-1 special-border mt-1" style="height: auto; min-height: 10rem;">
                                            <h6 class="text-center">Receptores de la Observación Seleccionada</h6>

                                            <div class="table-responsive table-responsive-sm gap-2" style="max-height: 9rem; overflow-x: auto;">
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid3" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource3">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Nombre_Receptor" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="LeidaTexto" HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="FechaLectura" HeaderText="F.Lectura" ItemStyle-CssClass="auto-width-column" />
                                                    </Columns>
                                                </asp:DataGrid>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>"
                                                    SelectCommand="SELECT Nombre_Receptor, CASE  WHEN Leida = 1 THEN 'SI' ELSE 'NO'
                                                      END AS LeidaTexto, FechaLectura, Id_Observacion  FROM tblOTObservacion_Receptor WHERE Id_Observacion = @Id_Observacion">
                                                    <SelectParameters>
                                                        <asp:Parameter Name="Id_Observacion" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                                <div class="col-lg-5 col-md-6 col-sm-12 col-xs-12">

                                    <div class="p-3 m-2 border" style="height: 46.5rem;">

                                        <div class="border rounded p-1 special-border" style="max-height: 25rem; overflow-x: auto;">

                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm p-1" ID="DataGridReceptorMail" runat="server" AutoGenerateColumns="false" OnItemCommand="DataGridReceptorMail_ItemCommand" DataSourceID="SqlDataSource2">
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="...">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" runat="server" CssClass="Tam" CommandName="VerMail" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn HeaderText="Departamento/Cargo" DataField="Cargo" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Nombre" DataField="NombreCompleto" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn HeaderText="" DataField="Mail" Visible="false" />
                                                    <asp:BoundColumn HeaderText="" DataField="Cedula" Visible="false" />

                                                </Columns>
                                            </asp:DataGrid>

                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL%>"
                                                SelectCommand="SELECT Cedula, Nombre + ' ' + Apellidos AS NombreCompleto, Cargo,Mail
                                                    FROM tblEmpleado
                                                    WHERE Activo = 1 AND ReceptorObservaciones = 1
                                                    ORDER BY Cargo ASC, Nombre ASC"></asp:SqlDataSource>

                                        </div>

                                        <div class="container-fluid pt-2 ">

                                            <div class="row pt-2 ">
                                                <div class="col-6">
                                                    <asp:Label ID="Label3" runat="server" Text="Receptores por defecto" CssClass="col-form-label-sm fw-bold"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row pt-2 ">
                                                <div class="col-12">
                                                    <asp:TextBox ID="tbReceptorCorreo" runat="server" CssClass=" form-control-sm"></asp:TextBox>
                                                </div>

                                            </div>

                                             <div class="row pt-2 ">
                                                <div class="col-12">
                                                    <asp:TextBox ID="tbRecepTipoObs" runat="server" CssClass=" form-control form-control-sm" placeHolder="Correos por tipo de observación"></asp:TextBox>
                                                </div>

                                            </div>

                                            <div class="row pt-2 ">
                                                <div class="col-12">
                                                    <asp:TextBox ID="tbCedulaRecp" runat="server" CssClass=" form-control form-control-sm" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="tbNombreRecp" runat="server" CssClass=" form-control form-control-sm" Visible="false"></asp:TextBox>
                                                </div>

                                            </div>

                                            <div class="row pt-3 ">

                                                <div class="col-1">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="Label4" runat="server" Text="OT" CssClass="col-form-label-sm gap-2 fw-bold"></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="col-4">
                                                    <div class="input-group input-group-sm gap-2">

                                                        <asp:TextBox ID="tbOT" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-4">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="Label5" runat="server" Text="Pedido" CssClass="col-form-label-sm fw-bold"></asp:Label>
                                                        <asp:TextBox ID="tbPedido" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="row pt-2">

                                                <div class="col-1">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="Label6" runat="server" Text="Obra" CssClass="col-form-label-sm fw-bold"></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="col-8">
                                                    <div class="input-group input-group-sm gap-2">

                                                        <asp:TextBox ID="tbNombreObra" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="row  mt-4">

                                                <div class="col-md-7">
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Button ID="BtnGrabarObservacion" CssClass="btn btn-sm btn-outline-secondary" runat="server" Text="Grabar Observacion" OnClick="GrabarObservacion_Click" />
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

            <div class="tab-pane fade" id="Personales-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="d-flex">
                            <div class="col-lg-7 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 25rem;">
                                    <h6 class="text-center">Observaciones por Leer</h6>
                                    <div class="table-responsive table-responsive-sm gap-2 border" style="height: 18rem; overflow-x: auto;">
                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid5" runat="server" AutoGenerateColumns="false">
                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="SelecOtOb" OnClick="lnkSelectRow_Click" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                            Text="<i class='bi bi-pencil-square'></i>" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn HeaderText="Tipo Observacion" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="FechaObservacion" HeaderText="Fecha Obs." ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Nombre_Emisor" HeaderText="Emisor" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="LeidaTexto" HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Observacion" HeaderText="Observacion" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                <asp:BoundColumn DataField="Id_Observacion" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                <asp:BoundColumn DataField="Nombre_Obra" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>"
                                            SelectCommand="SELECT   tblOTObservacion_Receptor.Nombre_Receptor, tblOTObservacion.*, CASE 
                                                        WHEN tblOTObservacion_Receptor.Leida = 1 THEN 'SI'
                                                        ELSE 'NO'
                                                      END AS LeidaTexto,  tblOTObservacion.FechaObservacion
                                                FROM tblOTObservacion
                                                INNER JOIN tblOTObservacion_Receptor ON tblOTObservacion.Id_Observacion = tblOTObservacion_Receptor.Id_Observacion
                                                WHERE tblOTObservacion_Receptor.Receptor = @CedulaLogeada AND tblOTObservacion_Receptor.Leida = 0
                                                ORDER BY tblOTObservacion.FechaObservacion">


                                            <SelectParameters>
                                                <asp:SessionParameter Name="CedulaLogeada" SessionField="CedulaLogeada" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <div class="container-fluid mt-2">
                                        <div class="row">
                                            <div class="col-lg-10 col-md-6 col-sm-12 col-xs-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label runat="server" class="col-form-label-sm">Obra</asp:Label>
                                                    <asp:TextBox ID="TextBox7" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-2 col-md-6 col-sm-12 col-xs-12">
                                                <asp:Button ID="Button1" runat="server" Text="Responder" CssClass="btn btn-sm btn-outline-dark" Enabled="false" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="p-3 m-2 border" style="height: 25rem;">
                                    <h6 class="text-center">Observaciones Grabadas y No Leidas</h6>
                                    <div class="table-responsive table-responsive-sm gap-2" style="max-height: 21rem; overflow-x: auto;">
                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid4" runat="server" AutoGenerateColumns="false"
                                            OnItemCommand="DataGrid2_ItemCommand" DataSourceID="SqlDataSource4">
                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="SelecOtOb" OnClick="lnkSelectRow4_Click" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                            Text="<i class='bi bi-pencil-square'></i>" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="ID_TipoObservacion" HeaderText="Tipo Observacion" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="FechaObservacion" HeaderText="Fecha Obs." ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Nombre_Receptor" HeaderText="Receptor" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="LeidaTexto" HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="Emisor" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                <asp:BoundColumn DataField="Nombre_Obra" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                                <asp:BoundColumn DataField="Observacion" ItemStyle-CssClass="auto-width-column" Visible="false" />
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>"
                                            SelectCommand="SELECT CASE WHEN tblOTObservacion_Receptor.Leida = 1 THEN 'SI'
                                                        ELSE 'NO'
                                                      END AS LeidaTexto, tblOTObservacion_Receptor.*, tblOTObservacion.*
                                            FROM tblOTObservacion
                                            INNER JOIN tblOTObservacion_Receptor ON tblOTObservacion.Id_Observacion = tblOTObservacion_Receptor.Id_Observacion
                                            WHERE (((tblOTObservacion.Emisor)=@CedulaLogeada) AND ((tblOTObservacion_Receptor.Leida)=0))">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="CedulaLogeada" SessionField="CedulaLogeada" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-5 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 25rem;">
                                    <h6>Observacion</h6>
                                    <textarea id="TextArea3" runat="server" class="form-control form-control-sm" cols="20" rows="2" style="height: 11rem;"></textarea>

                                    <h6 class="text-center mt-2">Receptores de la Observación Seleccionada</h6>
                                    <div class="table-responsive table-responsive-sm gap-2 border" style="max-height: 8rem; overflow-x: auto;">
                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid6" runat="server" AutoGenerateColumns="false">
                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                            <Columns>
                                                <asp:TemplateColumn>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="SelecOtOb" OnClick="lnkSelectRow6_Click" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                            Text="<i class='bi bi-pencil-square'></i>" />
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Nombre_Receptor" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="LeidaText" HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />
                                                <asp:BoundColumn DataField="FechaLectura" HeaderText="F.Lectura" ItemStyle-CssClass="auto-width-column" />
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>

                                </div>
                                <div class="p-3 m-2 border" style="height: 25rem;">
                                    <div class="row col-lg-12 col-md-6 col-sm-12 col-xs-12 container">
                                        <div class="input-group input-group-sm gap-2">
                                            <asp:Label ID="Label7" runat="server" Text="Obra" CssClass="col-form-label-sm"></asp:Label>
                                            <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-lg-12 col-md-6 col-sm-12 col-xs-12 container mt-2">
                                        <h6>Observación</h6>
                                        <textarea id="TextArea2" runat="server" class="form-control form-control-sm" style="height: 17rem;">
                                        </textarea>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Todas-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div class="d-flex">

                            <div class="col-lg-10 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 15rem;">
                                    <h3>Título 1</h3>
                                    <p>Contenido del div 1</p>
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 15rem;">
                                    <h3>Título 2</h3>
                                    <p>Contenido del div 2</p>
                                </div>
                            </div>
                        </div>

                        <div class="d-flex">

                            <div class="col-lg-8 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 20rem;">
                                    <h3>Título 1</h3>
                                    <p>Contenido del div 1</p>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 20rem;">
                                    <h3>Título 2</h3>
                                    <p>Contenido del div 2</p>
                                </div>
                            </div>
                        </div>

                        <div class="d-flex">

                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 12rem;">
                                    <h3>Título 1</h3>
                                    <p>Contenido del div 1</p>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 12rem;">
                                    <h3>Título 2</h3>
                                    <p>Contenido del div 2</p>
                                </div>
                            </div>
                        </div>


                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="tab-pane fade" id="Pendientes-content">
                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-lg-12 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 40rem;">
                                    <h6 class="text-center">Actividades Pendientes</h6>
                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid7" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="SelecOtOb" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                        Text="<i class='bi bi-pencil-square'></i>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                            <asp:BoundColumn HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Fecha Obs." ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Emisor" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Aplición" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Causa Observación" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="Fecha Act." ItemStyle-CssClass="auto-width-column" />
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                            </div>
                        </div>

                        <div class="d-flex">

                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 12rem;">
                                    <h6>Observacion</h6>
                                    <textarea id="TextArea7" cols="20" rows="2" class="form-control" style="height: 8rem;"></textarea>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
                                <div class="p-3 m-2 border" style="height: 12rem;">
                                    <h6>Receptores</h6>
                                    <textarea id="TextArea8" cols="20" rows="2" class="form-control" style="height: 8rem;"></textarea>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <%--   <div class="modal" id="miModalExito" tabindex="-1" style="display: none;">
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
                       <p>No se pudo completar la acción</p>
                   </div>
                   <div class="modal-footer">
                       <!-- Puedes agregar botones u opciones aquí si es necesario -->
                   </div>
               </div>
           </div>
       </div>--%>
        <script>
            $(document).ready(function () {
                if (habilitarObservaciones) {
                    // Habilitar el tab "Observaciones"
                    $("#Observaciones-tab").removeClass("disabled");
                    $("#Observaciones-tab").addClass("active");
                    $("#Observaciones-content").addClass("show active");

                    // Deshabilitar el tab "Personales"
                    $("#Personales-tab").removeClass("active");
                    $("#Personales-content").removeClass("show active");
                } else {
                    // Habilitar el tab "Personales"
                    $("#Personales-tab").addClass("active");
                    $("#Personales-content").addClass("show active");

                    // Deshabilitar el tab "Observaciones"
                    $("#Observaciones-tab").addClass("disabled");
                    $("#Observaciones-content").removeClass("show active");
                }
            });

        </script>

    </form>


    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
</body>
</html>
