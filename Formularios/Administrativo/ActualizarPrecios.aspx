<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="ActualizarPrecios.aspx.cs"
Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Administrativo.ActualizarPrecios"
%>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head runat="server">
    <meta charset="utf-8" />
    <title>Actualizar Precios</title>
    <link
      rel="icon"
      href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png"
    />
    <link
      rel="stylesheet"
      href="../../Recursos/CSS/Administrativo/ActualizarPrecios.css"
    />
    <link
      href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css"
      rel="stylesheet"
    />
  </head>
  <body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" />

      <div class="container mt-4 border rounded p-3">
        <!-- PASO 1 -->
        <div class="mb-3">
          <div class="step-title mb-2">PASO 1.</div>
          <div class="align-items-center section-header">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>
    <asp:Button
      ID="btnActualizarPrecios"
      runat="server"
      Text="Actualizar Precios Objetos"
      OnClick="btnActualizarPrecios_Click"
      CssClass="w-100 btn btn-outline-secondary fw-bold text-dark text-white-hover"
    />
    <asp:Timer
      ID="Timer1"
      runat="server"
      Interval="1000"
      OnTick="Timer1_Tick"
      Enabled="false"
    />
  </ContentTemplate>
  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
  </Triggers>
</asp:UpdatePanel>

          </div>
        </div>
        <hr class="hr-negro-grueso" />
        <!-- PASO 2 -->
        <div class="mb-3 d-flex">
          <div class="step-title">PASO 2.</div>
          <div class="row align-items-center mb-2 ms-auto">
            <div class="d-flex">
              <asp:Label
                ID="lblGrupo"
                CssClass="form-label fw-bold small"
                Text="Seleccione grupo de las estaciones de trabajo"
                runat="server"
              ></asp:Label>
             <asp:DropDownList
                CssClass="form-select form-select-sm"
                ID="ddlGrupo"
                runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"
              />
            </div>
          </div>
        </div>

        <!-- Tabla y controles -->
        <div class="d-flex mb-3">
          <div
            class="col-11 border rounded p-2"
            style="height: 300px; overflow: auto"
          >
            <asp:DataGrid
              CssClass="table table-bordered table-sm table-hover form-control-sm mt-2 "
              ID="DatagridActPrecio"
              runat="server"
              AutoGenerateColumns="false"
              ShowHeaderWhenEmpty="true"
              OnItemDataBound="DatagridActPrecio_ItemDataBound"
            >
              <HeaderStyle Font-Bold="true" CssClass="datagrid-header" />
              <Columns>
                    <asp:BoundColumn
    DataField="IdPanel"
    HeaderText="Estacion"
    ItemStyle-CssClass="auto-width-column"
  /> 
                <asp:BoundColumn
                  DataField="ValorActual"
                  HeaderText="Valor Actual"
                  ItemStyle-CssClass="auto-width-column"
                />
                      <asp:BoundColumn
      DataField="ValorNuevo"
      HeaderText="Valor Nuevo"
      ItemStyle-CssClass="auto-width-column"
    />
                <asp:BoundColumn
                  DataField="RazonNoActualizacion"
                  HeaderText="Razón de la No Actualización"
                  ItemStyle-CssClass="auto-width-column"
                />
              </Columns>
            </asp:DataGrid>
          </div>
          <!-- Icono en la esquina inferior derecha -->
          <div class="col-1 d-flex flex-column justify-content-end">
            <asp:LinkButton
              class="icong disabled"
              runat="server"
              title="Exportar Excel"
              ID="ExportarExcel"
            >
              <i class="custom-icon ms-3"></i>
            </asp:LinkButton>
          </div>
        </div>

        <!-- Botón deshabilitado -->
        <div class="text-center">
             <asp:Button
   ID="btnActualizarPreciosEstacionesTrabajo"
   runat="server"
   Text="Actualizar Precios Estaciones de Trabajo"
   OnClick="btnActualizarPreciosEstacionesTrabajo_Click"
   CssClass="btn btn-outline-secondary w-50 fw-bold text-dark text-white-hover"
                 Enabled="false"
 />
        </div>
      </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js"></script>
  </body>
</html>
