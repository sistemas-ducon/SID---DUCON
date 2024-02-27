<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PruebaExcel.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.PruebaExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="../../Recursos/CSS/OrdenTrabajo.css" />
    <title>Prueba Excel</title>
    <link rel="icon" href="https://neufert-cdn.archdaily.net/uploads/account_logo/logo/736/large_ADCO__Logo__Ducon.png" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container">
                 <div class="row justify-content-center mb-3">
                <div class="border rounded p-2">
                    <div class="row">
                        <div class="col-12">
                            <div class="table-responsive mb-1 gap-2" style="max-height: 20rem; overflow-x: auto;">
                                <h5 class="datagrid-header text-center">Empleados</h5>
                                <asp:DataGrid CssClass="table table-bordered table-sm table-hover form-control-sm" PageSize="5" AllowSorting="true" ID="DataGridPruebaExcel" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataSourceID="Empleados">
                                    <Columns>
                                        <asp:BoundColumn DataField="Nombre" HeaderText="Empleado" ItemStyle-CssClass="auto-width-column" />
                                        <asp:BoundColumn DataField="Cedula" HeaderText="Documento" ItemStyle-CssClass="auto-width-column" />
                                    

                                    </Columns>
                                </asp:DataGrid><asp:SqlDataSource runat="server" ID="Empleados" ConnectionString="<%$ ConnectionStrings:BD_SIDSQL %>" SelectCommand="select * from tblEmpleado where Zona = '01' and Activo = 1"></asp:SqlDataSource>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <asp:Button ID="PruebaExcel1" runat="server" Text="Button" OnClick="PruebaExcel_Click"  />
            </div>
            </div>

           



        </div>
    </form>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>



</body>
</html>
