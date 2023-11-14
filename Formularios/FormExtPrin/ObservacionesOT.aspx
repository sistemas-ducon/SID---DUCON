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
                                <div class="col-7">
                                    <div class="p-3 m-2 border" style="height: 26rem;">
                                        <div class="row">
                                        <div class="col-12">
                                            <div class="row justify-content-center">
                                                <div class="border rounded p-1 special-border" style="height: auto; min-height: 19rem;">
                                                    <%-- DATAGRID--%>

                                                    <div class="table-responsive mb-2 gap-2" style="max-height: 19rem; overflow-x: auto;">
                                                        <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm"
                                                            ID="DataGrid1" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource1"
                                                            DataKeyField="Id_Observacion" OnItemCommand="DataGrid1_ItemCommand" OnSelectedIndexChanged="DataGrid1_SelectedIndexChanged">
                                                            <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="SelecOt" runat="server" CommandName="Select" CommandArgument='<%# Container.ItemIndex %>'
                                                                            Text="<i class='bi bi-pencil-square text-dark'></i>" />
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
                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL_PRUEBAConnectionString%>"
                                                            SelectCommand="SELECT O.Id_OT, O.Consecutivo_Pedido, O.FechaObservacion, O.Nombre_Emisor, T.Aplicacion, T.Descripcion, O.Observacion, O.Id_Observacion
                                                               FROM tblOTObservacion AS O
                                                               INNER JOIN tblTipoObservacion AS T ON O.ID_TipoObservacion = T.ID_TipoObservacion
                                                               WHERE Id_OT = @Id_OT">
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
                                            <div class="col-9">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label ID="Label1" runat="server" CssClass="col-form-label-sm" Text="T.Obs."></asp:Label>
                                                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-3">
                                                <div class="input-group input-group-sm gap-2">
                                     <asp:Label ID="Label2" runat="server" CssClass="col-form-label-sm" Text="F.Actividad"></asp:Label>
                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control form-control-sm" type="Date"></asp:TextBox>
                                </div>
                                    </div>
                            </div>
                                    </div>
                                    <div class="p-3 m-2 border" style="height: 26rem;">
                                    <h6>Observación</h6>
                                        <textarea id="TextArea1" runat="server" class="form-control" style="height: 13rem;">
                                        </textarea>
                                        <div class="border rounded p-1 special-border mt-1" style="height: auto; min-height: 8rem;">
                                            <h6 class="text-center">Receptores de la Observación Seleccionada</h6>

                                             <div class="table-responsive table-responsive-sm gap-2" style="max-height: 7rem; overflow-x: auto;">
                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid3" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource3">
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:BoundColumn DataField="Nombre_Receptor" HeaderText="Nombre" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Leida" HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="FechaLectura" HeaderText="F.Lectura" ItemStyle-CssClass="auto-width-column" />
                                                </Columns>
                                            </asp:DataGrid>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL_PRUEBAConnectionString %>"
                                                SelectCommand="SELECT Nombre_Receptor, Leida, FechaLectura, Id_Observacion FROM tblOTObservacion_Receptor WHERE Id_Observacion = @Id_Observacion">
                                                <SelectParameters>
                                                    <asp:Parameter Name="Id_Observacion" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                                  </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-5">
                                    <div class="p-3 m-2 border" style="height: 52.5rem;">

                                        <div class="border rounded p-1 special-border" style="max-height: 40rem; overflow-x: auto;">
                                            <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm p-1" ID="DataGrid2" runat="server" AutoGenerateColumns="false" DataSourceID="SqlDataSource2">
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                    <asp:BoundColumn HeaderText="Departamento/Cargo" DataField="Cargo" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn HeaderText="Nombre" DataField="Nombre_Receptor" ItemStyle-CssClass="auto-width-column" />
                                                </Columns>
                                            </asp:DataGrid>

                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL_PRUEBAConnectionString%>"
                                                SelectCommand="SELECT DISTINCT E.Cargo, B.Nombre_Receptor FROM tblOTObservacion_Receptor AS B JOIN tblEmpleado AS E ON B.Receptor = E.Cedula"></asp:SqlDataSource>
                                        </div>
                                        <div class="container-fluid">
                                            <div class="row col-12">
                                                <asp:Label ID="Label3" runat="server" Text="Receptores por defecto" CssClass="col-form-label-sm"></asp:Label>
                                            </div>
                                            <div class="row col-12">
                                                <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </div>
                                            <div class="row col-12 container">
                                                <div class="col-4">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="Label4" runat="server" Text="OT" CssClass="col-form-label-sm gap-2"></asp:Label>
                                                        <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-4">
                                                    <div class="input-group input-group-sm gap-2">
                                                        <asp:Label ID="Label5" runat="server" Text="Pedido" CssClass="col-form-label-sm"></asp:Label>
                                                        <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row col-12">
                                                <div class="input-group input-group-sm gap-2">
                                                    <asp:Label ID="Label6" runat="server" Text="Obra" CssClass="col-form-label-sm"></asp:Label>
                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="row col-12 container">
                                            <%--     <asp:Button ID="Button1" runat="server" Text="Grabar Observacion" CssClass="mt-2 btn-sm btn-outline-dark btn" OnClick="Button1_Click"/>--%>
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
                            <div class="col-7">
                                <div class="p-3 m-2 border" style="height: 25rem;">
                                 <h6 class="text-center">Observaciones por Leer</h6>
                                     <div class="table-responsive table-responsive-sm gap-2 border" style="max-height: 18rem; overflow-x: auto;">
                                           <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid5" runat="server" AutoGenerateColumns="false">                                     
                                                <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                                <Columns>
                                                      <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="SelecOtOb" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                                            Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                    <asp:BoundColumn HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn  HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn  HeaderText="Tipo Observacion" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn HeaderText="Fecha Obs." ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn  HeaderText="Emisor" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn  HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />                                                                                                        
                                                </Columns>
                                     </asp:DataGrid>
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
                                                                        <asp:LinkButton ID="SelecOtOb" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                                            Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="Id_OT" HeaderText="OT" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Consecutivo_Pedido" HeaderText="Pedido" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="ID_TipoObservacion" HeaderText="Tipo Observacion" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="FechaObservacion" HeaderText="Fecha Obs." ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Nombre_Receptor" HeaderText="Receptor" ItemStyle-CssClass="auto-width-column" />
                                                    <asp:BoundColumn DataField="Leida" HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />                                                     
                                                     <asp:BoundColumn DataField="Emisor" ItemStyle-CssClass="auto-width-column" visible="false"/>
                                                      <asp:BoundColumn DataField="Nombre_Obra" ItemStyle-CssClass="auto-width-column" visible="false"/>
                                                    <asp:BoundColumn DataField="Observacion" ItemStyle-CssClass="auto-width-column" visible="false"/>
                                                </Columns>
                                     </asp:DataGrid>
                                         <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:BD_ISIDSQL_PRUEBAConnectionString %>"
                                             SelectCommand="SELECT Id_OT, Consecutivo_Pedido, ID_TipoObservacion, FechaObservacion, B.Nombre_Receptor, B.Leida, Emisor, Observacion, Nombre_Obra
                                                FROM tblOTObservacion AS A
                                                INNER JOIN tblOTObservacion_Receptor AS B ON A.Id_Observacion = B.Id_Observacion
                                                WHERE Emisor = '1037610897' AND Leida = '0'"></asp:SqlDataSource>
                                     </div>
                                </div>
                            </div>
                            <div class="col-5">
                                <div class="p-3 m-2 border" style="height: 25rem;">
                                    <h6>Observacion</h6>
                                    <textarea id="TextArea3" class="form-control" cols="20" rows="2" style="height: 13rem;"></textarea>

                                    <h6 class="text-center mt-2">Receptores de la Observación Seleccionada</h6>
                                    <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid6" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="SelecOtOb" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                        Text="<i class='bi bi-pencil-square text-dark'></i>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn HeaderText="Nombre" ItemStyle-CssClass="auto-width-column"></asp:BoundColumn>
                                            <asp:BoundColumn HeaderText="Leida" ItemStyle-CssClass="auto-width-column" />
                                            <asp:BoundColumn HeaderText="F.Lectura" ItemStyle-CssClass="auto-width-column" />                                                                      
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                                <div class="p-3 m-2 border" style="height: 25rem;">
                                    <div class="row col-12 container">
                                        <div class="input-group input-group-sm gap-2">
                                            <asp:Label ID="Label7" runat="server" Text="Obra" CssClass="col-form-label-sm"></asp:Label>
                                            <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control form-control-sm mt-2"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row col-12 container mt-2">
                                        <h6>Observación</h6>
                                        <textarea id="TextArea2" runat="server" class="form-control" style="height: 17rem;">
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
                             
                            <div class="col-10">
                                <div class="p-3 m-2 border" style="height: 15rem;">
                                    <h3>Título 1</h3>
                                    <p>Contenido del div 1</p>
                                </div>                            
                            </div>
                              <div class="col-2">
                                    <div class="p-3 m-2 border" style="height: 15rem;">
                                    <h3>Título 2</h3>
                                    <p>Contenido del div 2</p>
                                </div>
                              </div>                                                                                     
                        </div>
                         <div class="d-flex">
                               
                            <div class="col-8">
                                <div class="p-3 m-2 border" style="height: 20rem;">
                                    <h3>Título 1</h3>
                                    <p>Contenido del div 1</p>
                                </div>                            
                            </div>
                              <div class="col-4">
                                    <div class="p-3 m-2 border" style="height: 20rem;">
                                    <h3>Título 2</h3>
                                    <p>Contenido del div 2</p>
                                </div>
                              </div>                                                                                     
                        </div>

                          <div class="d-flex">
                               
                            <div class="col-4">
                                <div class="p-3 m-2 border" style="height: 12rem;">
                                    <h3>Título 1</h3>
                                    <p>Contenido del div 1</p>
                                </div>                            
                            </div>
                              <div class="col-4">
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
                            <div class="col-12">
                                <div class="p-3 m-2 border" style="height: 40rem;">
                                    <h6 class="text-center">Actividades Pendientes</h6>
                                   <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" ID="DataGrid7" runat="server" AutoGenerateColumns="false">
                                        <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="SelecOtOb" runat="server" CommandName="SelectOb" CommandArgument='<%# Container.ItemIndex %>'
                                                        Text="<i class='bi bi-pencil-square text-dark'></i>" />
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
                               
                            <div class="col-6">
                                <div class="p-3 m-2 border" style="height: 12rem;">
                                    <h6>Observacion</h6>
                                    <textarea id="TextArea7" cols="20" rows="2" class="form-control" style="height: 8rem;"></textarea>
                                </div>                            
                            </div>
                              <div class="col-6">
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
