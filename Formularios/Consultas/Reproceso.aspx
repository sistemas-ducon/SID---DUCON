<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reproceso.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Consultas.Reproceso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Reprocesos</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
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
                                        <asp:Button ID="btnNotificar" CssClass="btn btn-outline-secondary" runat="server" Text="Notificar" />


                                    </div>
                                </div>

                                <div class="col-2 pt-2 mt-2  ">
                                    <div class="input-group input-group-sm  mb-2 gap-2 ">
                                        <asp:CheckBox ID="chkConvenciones" runat="server" />
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
                                                                <asp:LinkButton ID="lnkView" runat="server" CommandName="VerReproceso" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
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

                                                <asp:DropDownList ID="ddlElemento" runat="server" class="form-control" DataTextField="Descripcion" DataValueField="Descripcion" DataSourceID="Elementos" OnDataBound="ddlElemento_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="Elementos" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="select * from tblElementoReproceso where activo = 1 order by descripcion"></asp:SqlDataSource>
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
                                                <asp:DropDownList ID="ddlArea1" runat="server" DataTextField="Descripcion" DataValueField="Descripcion" class="form-control" DataSourceID="AreaConsulta" OnDataBound="ddlArea1_DataBound"></asp:DropDownList><asp:SqlDataSource runat="server" ID="AreaConsulta" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL %>" SelectCommand="SELECT
                                                                             Id_Area,Descripcion,CAST(mailResponsable AS NVARCHAR(MAX))as MailResponsable,
                                                                             ResponsableReproceso From tblAreaReproceso Where (Activo = 1) order by descripcion "></asp:SqlDataSource>
                                            </div>
                                        </div>

                                        <div class="col-6 pt-1">

                                            <div class="input-group input-group-sm mb-2 gap-5 justify-content-center">
                                                <asp:LinkButton runat="server" title="Cerrar" ID="Cerrar">
                                                         <i class="bi bi-door-open" style="color: blue; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Agregar" ID="Agregar">
                                                         <i class="bi bi-file-earmark-plus" style="color: green; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Eliminar" ID="Eliminar">
                                                         <i class="bi bi-trash3" style="color: red; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                            </div>

                                        </div>

                                    </div>


                                </div>

                                <div class="col-7">

                                    <div class="row">
                                        <div class="col-12">
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
                                                                        <asp:LinkButton ID="lnkView" runat="server" CommandName="VerDetalleReproceso" CommandArgument='<%# Container.ItemIndex %>' Text="<i class='bi bi-pencil-square bi-4x'></i>" />
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

                                                                <%-- Campos oscultos pero que se muestran en el formulario empieza en el 13]--%>

                                                                <asp:BoundColumn DataField="Precio" Visible="false" />

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
                                            </div>
                                        </div>

                                        <div class="col-2">

                                            <div class="input-group input-group-sm mb-2 pt-3 mt-2 gap-2">

                                                <asp:LinkButton runat="server" title="Guardar" ID="Guardar">
                                                      <i class="bi bi-save2" style="color:blue; font-size:1rem; font-weight:600;"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" title="Adjuntar" ID="Adjuntar">
                                                        <i class="bi bi-paperclip" style="color:blue; font-size:1rem; font-weight:600;"></i>
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
                                                <asp:LinkButton runat="server" title="Guardar" ID="Redireccionar">
                                                     <i class="bi bi-arrow-90deg-right" style="color:green; font-size:1rem; font-weight:600"></i>
                                                </asp:LinkButton>
                                            </div>
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

                         

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>


        </div>


        <script>

            function validarFechas() {

                // Obtenemos los valores de los texxbox
                var fechaInicio = document.getElementById('fechaIni').value;
                var fechaFin = document.getElementById('fechaFin').value;

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

                // Si las fechas son válidas y la fecha de inicio es anterior a la fecha de fin, permitir la acción
                return true;
            }

            // Función para validar si la fecha está en el rango permitido (a partir de 1900)
            function validarRangoFechas(fecha) {
                var year = parseInt(fecha.split("-")[0]);
                return year >= 1900;
            }

        </script>

    </form>



    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>


</body>
</html>
