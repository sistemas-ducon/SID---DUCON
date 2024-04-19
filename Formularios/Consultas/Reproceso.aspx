<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reproceso.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Consultas.Reproceso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Reprocesos</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/Consultas/Reproceso.css" />
</head>
<body translate="no">
    <form id="FormReproceso" runat="server">
        <asp:ScriptManager runat="server" />

        <nav class="navbar navbar-light bg-light">
            <div class="container d-flex justify-content-center">
                <ul class="nav nav-tabs">
                    <li class="nav-item">
                        <a class="nav-link text-dark active" id="ReproCalidad-tab" data-bs-toggle="tab" href="#ReproCalidad-content">Reprocesos Calidad</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link text-dark" id="Estadistica-tab" data-bs-toggle="tab" href="#Estadisticas_content">Estadísticas-Consolidado</a>
                    </li>

                </ul>
            </div>
        </nav>

        <div class="tab-content">

            <div class="tab-pane fade show active" id="ReproCalidad-content">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <div class="container-fluid">

                            <asp:Label ID="error" runat="server" Text="" Visible="false"></asp:Label>

                            <div class="row">

                                <div class="col-4 pt-2 mt-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2">
                                        <asp:Label ID="Label1" class="form-label" Text="Entre" runat="server"></asp:Label>
                                        <asp:TextBox ID="fechaIni" type="date" runat="server" class="form-control"></asp:TextBox>
                                        <asp:Label ID="Label2" class="form-label" Text=" Y " runat="server"></asp:Label>
                                        <asp:TextBox ID="fechaFin" type="date" runat="server" class="form-control"></asp:TextBox>

                                    </div>
                                </div>

                                <div class="col-4 pt-2 mt-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <asp:Label class="form-label" Text="Area" runat="server" ID="lbArea"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlArea" runat="server" DataTextField="Descripcion" DataValueField="Descripcion" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" AutoPostBack="true" OnDataBound="ddlArea_DataBound"></asp:DropDownList>
                                        <asp:SqlDataSource ID="ConPermiso" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand=" SELECT 
                                        Id_Area,
                                        Descripcion,
                                        CAST(mailResponsable AS NVARCHAR(MAX))as MailResponsable,
                                        ResponsableReproceso
                                        From tblAreaReproceso Where (Activo = 1)
                                        Union
                                        select 0 as Id_Area, '%' as Descripcion, '%' as MailResponsable,  '%' as ResponsableReproceso order by descripcion"></asp:SqlDataSource>

                                        <asp:SqlDataSource ID="SinPermiso" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT * 
                                                FROM tblAreaReproceso WHERE ResponsableReproceso LIKE '%' + @Cedula + '%' AND activo = 1 ORDER BY descripcion">
                                            <SelectParameters>
                                                <asp:SessionParameter SessionField="cedulalogueada" Name="Cedula"></asp:SessionParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:Label class="form-label" Text="Estado" runat="server" ID="lbEstado"></asp:Label>
                                        <asp:DropDownList class="form-control" ID="ddlEstado" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-2 pt-2 mt-2">
                                    <div class="input-group input-group-sm  mb-2 gap-2  justify-content-around">
                                        <asp:Button ID="btnConsultar" CssClass="btn btn-outline-secondary" runat="server" Text="Consultar" OnClick="btnConsultar_Click" OnClientClick="return validarFechas();" />
                                        <asp:Button ID="btnNotificar" CssClass="btn btn-outline-secondary" runat="server" Text="Notificar" OnClick="btnNotificar_Click" />


                                    </div>
                                </div>

                                <div class="col-2 pt-2 mt-2  ">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <asp:CheckBox ID="chkConvenciones" runat="server" OnCheckedChanged="chkConvenciones_CheckedChanged" AutoPostBack="true" />
                                        <asp:Label ID="lbConvenciones" runat="server" Text="Convenciones"></asp:Label>

                                    </div>
                                </div>

                            </div>

                            <div class="row justify-content-center">
                                <div class="border rounded  pt-2 mt-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 14rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-start">Reprocesos</h6>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DatagridReproceso" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemDataBound="DatagridReproceso_ItemDataBound" OnItemCommand="DatagridReproceso_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton CssClass="Tam" ID="lnkView" runat="server" CommandName="VerReproceso" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="" HeaderText="Cant" />
                                                        <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Nombre_Obra" HeaderText="Obra" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Observacion_Pedido" HeaderText="Observación" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Fecha_Entrega_Produccion" HeaderText="F. Reproceso" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Cerrado" HeaderText="completo" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Acepta" HeaderText="Aceptado" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Origen" HeaderText="Origen" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Redirigido" HeaderText="Redirigido" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Id_Area" HeaderText="Id Area" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion" HeaderText="Area" ItemStyle-CssClass="auto-width-column" />

                                                        <%-- Campos oscultos pero que se muestran en el formulario empieza en el 13]--%>

                                                        <asp:BoundColumn DataField="Precio" Visible="false" />

                                                    </Columns>
                                                </asp:DataGrid>



                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">

                                <asp:SqlDataSource ID="Reprocesos" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>">
                                    <SelectParameters>
                                        <asp:Parameter Name="Descripcion" Type="String" />
                                        <asp:Parameter Name="fechaIni" Type="DateTime" />
                                        <asp:Parameter Name="FechaFin" Type="DateTime" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:SqlDataSource ID="PorAsignar" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT
                                                    Id_OT,Consecutivo_Pedido,Nombre_Obra, Observacion_Pedido, Fecha_Entrega_Produccion,
                                                    '' AS Origen,'' AS Redirigido, 0 AS Cerrado, 'Pend' AS Acepta,0 AS Precio,'' AS Id_Area,'' AS  Descripcion                                                
                                                   from tblReporteOT 
                                                   WHERE (Id_TipoPedido = '8') AND (Fecha_Terminada_Despacho BETWEEN @fechaIni And @fechaFin) 
                                                   and   concat(Id_OT , '-' , Consecutivo_Pedido) not in 
                                                   (SELECT concat(tblReprocesoDetalle.Ot ,  '-' ,  tblReprocesoDetalle.Pedido)  FROM  tblReprocesoDetalle) 
                                                    ORDER BY Fecha_Terminada_Despacho">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="fechaIni" PropertyName="Text" Name="fechaIni"></asp:ControlParameter>
                                        <asp:ControlParameter ControlID="fechaFin" PropertyName="Text" Name="fechaFin"></asp:ControlParameter>
                                    </SelectParameters>
                                </asp:SqlDataSource>

                            </div>

                        </div>

                        <div class="container-fluid pt-2 mt-2">

                            <div class="row " style="padding-left: 0.7rem">

                                <div class="col-5 p-1 pt-4">

                                    <div class="row">

                                        <div class="col-1">
                                            <div class="input-group input-group-sm mb-2 ">
                                                <asp:Label class="form-label" Text="OT" runat="server" ID="lblOT"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-5">
                                            <div class="input-group input-group-sm mb-2">
                                                <asp:TextBox ID="tbOT" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-1"></div>

                                        <div class="col-5">
                                            <div class="input-group input-group-sm mb-2 gap-2">
                                                <asp:Label class="form-label" Text="Pedido" runat="server" ID="lbPedido"></asp:Label>
                                                <asp:TextBox ID="tbPedido" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-1">
                                            <div class="input-group input-group-sm mb-2 ">
                                                <asp:Label class="form-label" Text="Obra" runat="server" ID="lbObra"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-11">
                                            <div class="input-group input-group-sm mb-2">
                                                <asp:TextBox ID="tbObra" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-12">
                                            <div class=" input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbObservacion" runat="server" Text="Observación"></asp:Label>
                                                <textarea class="form-control form-control-sm" id="txObs" runat="server" cols="20" rows="15"></textarea>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">

                                        <div class="col-1">
                                            <div class="input-group input-group-sm mb-2">
                                                <asp:Label class="form-label" Text="Element" runat="server" ID="lbElemento"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-5">
                                            <div class=" input-group input-group-sm mb-2">

                                                <asp:DropDownList ID="ddlElemento" runat="server" class="form-control" DataTextField="Descripcion" DataValueField="Id_Elemento" DataSourceID="Elementos" OnDataBound="ddlElemento_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="Elementos" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="select * from tblElementoReproceso where activo = 1 order by descripcion"></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <div class=" input-group input-group-sm mb-2 gap-2">
                                                <asp:Label class="form-label" Text="Cantidad" runat="server" ID="lbCantidad"></asp:Label>
                                                <asp:TextBox ID="tbCantidad" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-1">
                                            <div class="input-group input-group-sm mb-2">
                                                <asp:Label class="form-label" Text="Area" runat="server" ID="lbArea1"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-5">
                                            <div class="input-group input-group-sm mb-2">
                                                <asp:DropDownList ID="ddlArea1" runat="server" DataTextField="Descripcion" DataValueField="Id_Area" class="form-control" DataSourceID="AreaConsulta" OnDataBound="ddlArea1_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="AreaConsulta" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT
                                                                             Id_Area,Descripcion,CAST(mailResponsable AS NVARCHAR(MAX))as MailResponsable,
                                                                             ResponsableReproceso From tblAreaReproceso Where (Activo = 1) order by descripcion "></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-6 pt-1">

                                            <div class="input-group input-group-sm mb-2 gap-5 justify-content-center">
                                                <asp:LinkButton runat="server" title="Cerrar" ID="CerrarReproceso" OnClick="CerrarReproceso_Click">
                                                         <i class="bi bi-door-open" style="color: blue; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Agregar" ID="Agregar" OnClick="Agregar_Click">
                                                         <i class="bi bi-file-earmark-plus" style="color: green; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Eliminar" ID="EliminarElemento1" OnClick="EliminarElemento1_Click">
                                                         <i class="bi bi-trash3" style="color: red; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                            </div>

                                        </div>

                                    </div>

                                </div>

                                <div class="col-7">

                                    <div class="row">

                                        <div class="col-6">
                                            <asp:Label ID="Id_detalle" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lbNombAreaRepro" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lbRedirigido" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lbIdElemnto" runat="server" Text="" Visible="false"></asp:Label>
                                            <span id="ErrorValidacionDoc" style="color: red;" runat="server" visible="false"></span>
                                        </div>

                                        <div class="col-6">
                                            <div class="input-group input-group-sm  mb-2 gap-2 justify-content-end ">
                                                <asp:CheckBox ID="chkcerrado" runat="server" />
                                                <asp:Label ID="lbCerrada" runat="server" Text="Cerrado"></asp:Label>
                                                <asp:CheckBox ID="chkAcepta" runat="server" OnCheckedChanged="chkAcepta_CheckedChanged" AutoPostBack="true" />
                                                <asp:Label ID="lbAcepta" runat="server" Text="Acepta"></asp:Label>

                                            </div>
                                        </div>

                                    </div>

                                    <div class="row justify-content-center" style="padding-left: 0.8rem">
                                        <div class="border rounded  ">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="table-responsive  mb-2 gap-2" style="height: 9rem; overflow-x: auto;">
                                                        <h6 class="datagrid-header text-center">Detalle Reprocesos</h6>
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DatagridDetalleReproceso" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="DetalleReproceso" OnItemCommand="DatagridDetalleReproceso_ItemCommand">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="...">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton CssClass="Tam" ID="lnkView" runat="server" CommandName="VerDetalleReproceso" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Area" HeaderText="Area" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Elemento" HeaderText="Elemento" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Precio" HeaderText="Precio" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Causa" HeaderText="Causa" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="correccion" HeaderText="Corrección" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Observacion" HeaderText="Comentario" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Id_Detalle" HeaderText="Id_Detalle" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Acepta1" HeaderText="Acepta" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="PorQue1" HeaderText="Por_qué_1" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="PorQue2" HeaderText="Por_qué_2" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="PorQue3" HeaderText="Por_qué_3" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="PorQue4" HeaderText="Por_qué_4" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="PorQue5" HeaderText="Por_qué_5" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Origen" HeaderText="Origen" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Redirigido" HeaderText="Redirigido" ItemStyle-CssClass="auto-width-column" />
                                                                <asp:BoundColumn DataField="Cerrado" HeaderText="Cerrado" ItemStyle-CssClass="auto-width-column" />


                                                                <asp:BoundColumn DataField="Precio" Visible="false" />
                                                                <asp:BoundColumn DataField="Id_Area" Visible="false" />
                                                                <asp:BoundColumn DataField="Id_Elemento" Visible="false" />


                                                            </Columns>
                                                        </asp:DataGrid><asp:SqlDataSource runat="server" ID="DetalleReproceso" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT
                                                                            IIF ( tblReprocesoDetalle.Acepta = 1,'Si',IIF( tblReprocesoDetalle.Acepta=0 ,'No','')) as Acepta1,
                                                                            IIF ( tblReprocesoDetalle.Cerrado = 1, 'Si', IIF( tblReprocesoDetalle.Cerrado=0 ,'No','')) as Cerrado,
                                                                            tblReprocesoDetalle.Id_Detalle,tblReprocesoDetalle.Ot,tblReprocesoDetalle.Pedido,
                                                                            tblReprocesoDetalle.Cantidad,tblReprocesoDetalle.Precio,tblReprocesoDetalle.Responsable,
                                                                            tblReprocesoDetalle.correccion,tblReprocesoDetalle.Observacion,
                                                                            tblReprocesoDetalle.Cerrado,tblReprocesoDetalle.Origen,tblReprocesoDetalle.Redirigido, 
                                                                            tblElementoReproceso.Descripcion AS Elemento,tblAreaReproceso.Descripcion AS Area,
                                                                            tblCausasReproceso.Descripcion AS Causa,tblReprocesoDetalle.Id_Elemento,tblReprocesoDetalle.Id_Area,
                                                                            PorQue1,PorQue2,PorQue3,PorQue4,PorQue5
                                                                            FROM tblReprocesoDetalle 
                                                                            INNER JOIN tblElementoReproceso ON tblReprocesoDetalle.Id_Elemento = tblElementoReproceso.Id_Elemento 
                                                                            INNER JOIN tblAreaReproceso ON tblReprocesoDetalle.Id_Area = tblAreaReproceso.Id_Area 
                                                                            LEFT OUTER JOIN tblCausasReproceso ON tblReprocesoDetalle.Id_Causa = tblCausasReproceso.Id_Causa
                                                                            WHERE(tblReprocesoDetalle.Ot = @Id_OT)
                                                                            AND (tblReprocesoDetalle.Pedido =@Pedido) 
                                                                            AND (tblAreaReproceso.Descripcion Like @Descripcion)">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="tbOT" PropertyName="Text" Name="Id_OT"></asp:ControlParameter>
                                                                <asp:ControlParameter ControlID="tbPedido" PropertyName="Text" Name="Pedido"></asp:ControlParameter>
                                                                <asp:Parameter Name="Descripcion"></asp:Parameter>
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>



                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">

                                        <div class="col-6">
                                            <div class=" input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Por qué 1" runat="server" ID="lbPq1"></asp:Label>
                                                <asp:TextBox ID="tbPq1" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <div class=" input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Por qué 2" runat="server" ID="lbPq2"></asp:Label>
                                                <asp:TextBox ID="tbPq2" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-6">
                                            <div class=" input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Por qué 3" runat="server" ID="lbPq3"></asp:Label>
                                                <asp:TextBox ID="tbPq3" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <div class=" input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Por qué 4" runat="server" ID="lbPq4"></asp:Label>
                                                <asp:TextBox ID="tbPq4" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-4">
                                            <div class=" input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Por qué 5" runat="server" ID="lbPq5"></asp:Label>
                                                <asp:TextBox ID="tbPq5" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-4">
                                            <div class="input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Causa Raiz" runat="server" ID="lbCausaRaiz"></asp:Label>
                                                <asp:DropDownList ID="ddlCausaRaiz" DataTextField="Descripcion" DataValueField="Descripcion" class="form-control" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-2">
                                            <div class="input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="ID Causa" runat="server" ID="lbIdCausa"></asp:Label>
                                                <asp:TextBox ID="tbIdCausa" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-2">
                                            <div class="input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Cant/Mes" runat="server" ID="lbCant"></asp:Label>
                                                <asp:TextBox ID="tbCant" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>


                                    </div>

                                    <div class="row">

                                        <div class="col-6">
                                            <div class=" input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbCorrecion" runat="server" Text="Corrección"></asp:Label>
                                                <textarea class="form-control form-control-sm" id="txCorreccion" runat="server" cols="20" rows="4"></textarea>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <div class=" input-group-sm  mb-2 gap-2">
                                                <asp:Label ID="lbComentario" runat="server" Text="Comentario"></asp:Label>
                                                <textarea class="form-control form-control-sm" id="txComentario" runat="server" cols="20" rows="4"></textarea>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">

                                        <div class="col-4">
                                            <div class="input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Persona Responsable" runat="server" ID="lbResponsable"></asp:Label>
                                                <asp:DropDownList ID="ddlResponsable" DataTextField="NombreCompleto" DataValueField="NombreCompleto" class="form-control" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-2">
                                            <div class="input-group-sm mb-1 gap-2">
                                                <asp:Label class="form-label" Text="Precio" runat="server" ID="lbPrecio"></asp:Label>
                                                <asp:TextBox ID="tbPrecio" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:TextBox ID="precioHidden" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-2">

                                            <div class="input-group input-group-sm mb-2 pt-3 mt-2 gap-2">

                                                <asp:LinkButton runat="server" title="Guardar" ID="Guardar" OnClick="Guardar_Click">
                                                      <i class="bi bi-save2" style="color:blue; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Adjuntar" ID="Adjuntar" OnClick="Adjuntar_Click">
                                                        <i class="bi bi-paperclip" style="color:blue; font-size:1rem !important; font-weight:600 !important;"></i>
                                                </asp:LinkButton>

                                            </div>
                                        </div>

                                        <div class="col-3 ">
                                            <div class="input-group input-group-sm pt-3 mt-2  mb-2 gap-2">
                                                <asp:CheckBox ID="chkPlanAccion" runat="server" />
                                                <asp:Label ID="lbPlanAccion" class="form-label" CssClass="mt-0 pt-0" Text="Generar Plan Acción" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-1">
                                            <div class="input-group input-group-sm mb-2 pt-3 mt-2 gap-2">
                                                <asp:LinkButton runat="server" title="Guardar" ID="Redireccionar" OnClick="Redireccionar_Click">
                                                     <i class="bi bi-arrow-90deg-right" style="color:green; font-size:1rem !important; font-weight:600 !important"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                            </div>

                        </div>

                        <!--Modal Cerrar Reproceso -->
                        <div id="cerrarReprocesoModal" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-primary text-white">
                                        <h5 class="modal-title text-center">Cerrar reproceso</h5>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>Estás seguro que deseas cerrar el reproceso de la OT: <span runat="server" id="Ot"></span>- <span runat="server" id="Pedido"></span>? </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="cerrarReproceso1" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-primary" Style="width: 5rem;" OnClick="cerrarReproceso1_Click1" />
                                            <asp:Button runat="server" ID="CerrarQuitarRepro" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal Eliminar Reproceso -->
                        <div id="eliminarReprocesoModal" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-danger text-white">
                                        <h5 class="modal-title text-center">Eliminar elemento</h5>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿Estás seguro que desea eliminar el elemento seleccionado?</h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="EliminarElemento" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn btn-outline-danger" Style="width: 5rem;" OnClick="EliminarElemento_Click" />
                                            <asp:Button runat="server" ID="CerrarElimnarElemento" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal Agregar Reproceso -->
                        <div id="agregarReproMoodal" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success text-white">
                                        <h5 class="modal-title text-center">Agregar reproceso</h5>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿ Estás seguro de querer agregar el reproceso?  Se notificará por correo electrónico al responsable del área de este proceso.  </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnAgregar" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-success" Style="width: 5rem;" OnClick="btnAgregar_Click" />
                                            <asp:Button runat="server" ID="Button2" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal Convenciones -->
                        <div class="modal fade" id="convenciones" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered  ">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Convenciones</h5>
                                        <asp:Button ID="moldaCerrar" type="button" data-bs-dismiss="modal" aria-label="Close" class="btn-close" runat="server" OnClick="moldaCerrar_Click" />

                                    </div>

                                    <div class="modal-body">
                                        <div class="row justify-content-center mb-3">
                                            <div class="border rounded p-2">
                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #673f8b; white-space: nowrap"></div>
                                                            <label for="lbNoAceptado" class="form-label">No se ha aceptado</label>
                                                        </div>

                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #FA721E"></div>
                                                            <label for="lbPendiente" class="form-label">Pendiente por precio</label>
                                                        </div>

                                                        <div class="input-group input-group-sm mb-1 gap-2">
                                                            <div class="input-group " style="width: 20px; height: 20px; border: 1px; background-color: #F1FF43"></div>
                                                            <label for="lbSinAsignar" class="form-label">Sin asignar</label>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>



                                    </div>

                                    <div class="modal-footer">
                                    </div>

                                </div>
                            </div>
                        </div>

                        <!--Modal Redireccionar Reproceso -->
                        <div id="redireccionarReproMoodal" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success text-white">
                                        <h5 class="modal-title text-center">Redireccionar reproceso</h5>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿ Estás seguro que deseaas redireccionar el reproceso?  Se notificará por correo electrónico al responsable del área de este proceso.  </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnRediret" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-success" Style="width: 5rem;" OnClick="btnRediret_Click" />
                                            <asp:Button runat="server" ID="Cerrar" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Modal Redireccionar Reproceso -->
                        <div id="notificarReproMoodal" class="modal" tabindex="-1" style="display: none;">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header bg-success text-white">
                                        <h5 class="modal-title text-center">Notificar Reproceso</h5>

                                    </div>
                                    <div class="modal-body border rounded">
                                        <div class="container-fluid">
                                            <h6>¿ Estás seguro que deseaas notificar los  reprocesos?  Se notificará por correo electrónico a los responsables de as áreas.  </h6>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <div class="container-fluid d-flex justify-content-center gap-5 p-0">
                                            <asp:Button runat="server" ID="btnNotRepro_SI" Text="Si" data-bs-dismiss="modal" aria-label="Close" CssClass="btn  btn-outline-success" Style="width: 5rem;" OnClick="btnNotRepro_SI_Click" />
                                            <asp:Button runat="server" ID="btnNotRepro_NO" Text="No" data-bs-dismiss="modal" aria-label="Close" CssClass=" btn btn-outline-secondary" Style="width: 5rem;" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>


            </div>

            <div class="tab-pane fade " id="Estadisticas_content" runat="server">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid">

                            <div class="row pt-2 mt-2">

                                <div class="col-4 "></div>

                                <div class="col-4 ">
                                    <h4>Estadísticas Reprocesos</h4>
                                </div>

                                <div class="col-2">
                                    <div class=" input-group input-group-sm mb-1 gap-2 justify-content-center">
                                        <asp:Label class="form-label" Text="Año" runat="server" ID="lbAnio"></asp:Label>
                                        <asp:DropDownList CssClass="form-control" ID="ddlAnioBusqueda" runat="server">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="col-1">
                                    <div class=" input-group input-group-sm mb-1 gap-2 justify-content-center">
                                        <asp:Button ID="btnConsultar1" CssClass="btn btn-outline-secondary" runat="server" Text="Consultar" OnClick="btnConsultar1_Click" />
                                    </div>
                                </div>

                                <div class="col-1">
                                    <asp:LinkButton class="icong disabled" runat="server" title="Exportar" ID="ExportarExcel" OnClick="ExportarExcel_Click">
                                         <i class="custom-icon"></i>
                                    </asp:LinkButton>
                                </div>


                            </div>

                            <div class="row gap-0 justify-content-around  pb-2 mb-2 ">

                                <div class=" col-7 border rounded pt-2 mt-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 14rem; overflow-x: auto;">

                                                <h6 class="datagrid-header text-center">Estadísticas Consolidadas</h6>

                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DatagridConsolidado" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:BoundColumn DataField="Mes" HeaderText="Mes" />
                                                        <asp:BoundColumn DataField="CantRepr" HeaderText="Cant. Repro" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="CantOT" HeaderText="OT por Mes" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="PorcRepr" HeaderText="%Reproc" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="CostoTotales" HeaderText="Costo Totales" ItemStyle-CssClass="auto-width-column" DataFormatString="{0:N0}" />
                                                        <asp:BoundColumn DataField="CostoDucon" HeaderText="Costo Ducon" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="VentasMes" HeaderText="Venta/Mes" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="PorcRepVent" HeaderText="%Rep/Vent" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Meta" HeaderText="Meta" ItemStyle-CssClass="auto-width-column" />

                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class=" col-4 border rounded  pt-2 mt-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 14rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-center">Responsable Reproceso</h6>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DatagridResponsable" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemCommand="DatagridResponsable_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lknRespo" runat="server" CommandName="VerRespXCausa" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion" Visible="false" />
                                                        <asp:BoundColumn DataField="Mes" Visible="false" />

                                                    </Columns>
                                                </asp:DataGrid>



                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row gap-0 justify-content-around pb-2 mb-2 ">

                                <div class=" col-7 border rounded pt-2 mt-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 14rem; overflow-x: auto;">

                                                <h6 class="datagrid-header text-center">Cantidad Reprocesos Por Área</h6>

                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DatagridCantidad" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemCommand="DatagridCantidad_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="Responsable">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Responsable" runat="server" CommandName="VerFullDetalle" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Responsable") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Ene">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Enero" runat="server" CommandName="VerEnero" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Enero") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Feb">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Febrero" runat="server" CommandName="VerFebrero" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Febrero") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Mar">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Marzo" runat="server" CommandName="VerMarzo" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Marzo") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn HeaderText="Abr">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Abril" runat="server" CommandName="VerAbril" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Abril") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn HeaderText="May">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Mayo" runat="server" CommandName="VerMayo" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Mayo") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Jun">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Junio" runat="server" CommandName="VerJunio" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Junio") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Jul">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Julio" runat="server" CommandName="VerJulio" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Julio") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Ago">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Agosto" runat="server" CommandName="VerAgosto" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Agosto") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Sep">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Septiembre" runat="server" CommandName="VerSeptiembre" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Septiembre") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Oct">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Octubre" runat="server" CommandName="VerOctubre" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Octubre") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn HeaderText="Nov">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Noviembre" runat="server" CommandName="VerNoviembre" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Noviembre") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Dic">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Diciembre" runat="server" CommandName="VerDiciembre" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Diciembre") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:TemplateColumn HeaderText="Total">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Total" runat="server" CommandName="VerFullDetalle" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Total") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>


                                                        <asp:TemplateColumn HeaderText="%">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="Porcentaje" runat="server" CommandName="VerFullDetalle" CommandArgument='<%# Container.ItemIndex %>' Text='<%# Bind("Porcentaje") %>' CssClass="NoLetra auto-width-column" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>




                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class=" col-4 border rounded  pt-2 mt-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 14rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-center">Causa Reproceso</h6>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGridCausa" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" OnItemCommand="DataGridCausa_ItemCommand">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:TemplateColumn HeaderText="...">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lknCausa" runat="server" CommandName="VerRespXCausa1" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                        <asp:BoundColumn DataField="Causa" HeaderText="Causa" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Descripcion" Visible="false" />
                                                        <asp:BoundColumn DataField="Mes" Visible="false" />

                                                    </Columns>
                                                </asp:DataGrid>



                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row gap-0 justify-content-around ">

                                <div class=" col-7 border rounded pt-2 mt-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 14rem; overflow-x: auto;">

                                                <h6 class="datagrid-header text-center">Precio Reproceso Por Área </h6>

                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DatagridPrecioArea" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:BoundColumn DataField="Responsable" HeaderText="Responsable" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Enero" HeaderText="Ene" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Febrero" HeaderText="Feb" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Marzo" HeaderText="Mar" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Abril" HeaderText="Abr" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Mayo" HeaderText="May" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Junio" HeaderText="Jun" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Julio" HeaderText="Jul" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Agosto" HeaderText="Ago" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Septiembre" HeaderText="Sep" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Octubre" HeaderText="Oct" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Noviembre" HeaderText="Nov" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Diciembre" HeaderText="Dic" ItemStyle-CssClass="auto-width-column" />
                                                        <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-CssClass="auto-width-column" />

                                                        <%-- Campos oscultos pero que se muestran en el formulario empieza en el 13]--%>

                                                        <asp:BoundColumn DataField="" Visible="false" />

                                                    </Columns>
                                                </asp:DataGrid>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class=" col-4 border rounded  pt-2 mt-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="table-responsive  mb-2 gap-2" style="height: 14rem; overflow-x: auto;">
                                                <h6 class="datagrid-header text-center">Responsable Causa</h6>
                                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DatagridRespoCausa" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                                    <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                    <Columns>

                                                        <asp:BoundColumn DataField="RespoCausa" HeaderText="" />
                                                        <asp:BoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-CssClass="auto-width-column" />

                                                    </Columns>
                                                </asp:DataGrid>



                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </ContentTemplate>

                    <Triggers>
                        <asp:PostBackTrigger ControlID="ExportarExcel" />
                    </Triggers>


                </asp:UpdatePanel>

            </div>


        </div>


        <script>

            function validarFechas() {

                // Obtenemos los valores de los texxbox
                var fechaInicio = document.getElementById('fechaIni').value;
                var fechaFin = document.getElementById('fechaFin').value;
                // Obtener la fecha actual
                var fechaActual = new Date();

                // Validar si las fechas están en el rango permitido
                if (!validarRangoFechas(fechaInicio) || !validarRangoFechas(fechaFin)) {

                    alert('Por favor ingrese un  fecha válidas');
                    return false;
                }

                // Convertir las cadenas de fecha en objetos Date
                var inicio = new Date(fechaInicio);
                var fin = new Date(fechaFin);

                // Verificar si las fechas son válidas
                if (isNaN(inicio.getTime()) || isNaN(fin.getTime())) {
                    // Mostrar un mensaje de error si las fechas no son válidas
                    alert('Por favor ingrese fechas válidas.');
                    return false; // Evitar que se ejecute la acción
                }

                // Verificamos si la fecha de inicio es posterior a la fecha de fin
                if (inicio > fin) {

                    alert('La fecha de inicio debe ser anterior a la fecha de fin.');
                    return false; // Evitar que se ejecute la acción
                }


                // Verificar si la fecha de fin es posterior a la fecha actual
                if (fin > fechaActual) {
                    alert('La fecha Y no puede ser posterior a la fecha actual.');
                    return false;
                }



                // Si las fechas son válidas y la fecha de inicio es anterior a la fecha de fin, permitir la acción
                return true;
            }

            // Función para validar si la fecha está en el rango permitido (a partir de 1900)
            function validarRangoFechas(fecha) {
                var year = parseInt(fecha.split("-")[0]);
                return year >= 1900;
            }


            function actualizarValorOt() {
                // Obtener el valor del TextBox
                var OT = document.getElementById('tbOT').value;
                var Ped = document.getElementById('tbPedido').value;
                // Actualizar el contenido del span con el valor del TextBox
                document.getElementById('Ot').innerText = OT;
                document.getElementById('Pedido').innerText = Ped;
            }


        </script>

    </form>



    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>


</body>
</html>
