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
                // Obtiene el mensaje personalizado de la consulta
                var message = "<%= HttpUtility.UrlDecode(Request.QueryString["message"]) %>";
                alert(message);

                // Obtiene la URL de redirección de la consulta
                var redirectUrl = "<%= HttpUtility.UrlDecode(Request.QueryString["redirectUrl"]) %>";

                // Redirige a la página indicada desde la página de acción
                window.location.href = redirectUrl;
            </script>
        </div>
    </form>
</body>
</html>
