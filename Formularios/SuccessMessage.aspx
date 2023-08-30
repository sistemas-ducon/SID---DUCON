<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuccessMessage.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.SuccessMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <script type="text/javascript">
                alert('La acción se realizó con éxito.');
                window.location.href = 'Ventas/Visita_Asesores.aspx'; // Redirigir de nuevo a la página original
            </script>
        </div>
    </form>
</body>
</html>
