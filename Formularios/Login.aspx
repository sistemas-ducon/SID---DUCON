<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SISTEMA_INTEGRAL_DUCON.Formularios.Login.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous"/>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="../../Recursos/CSS/login.css" rel="stylesheet" />
    <title>Login</title>
</head>
    
<body>
    
   <div class="login": style="text-align:center" >


    <div class="wrapper">
        
   <h1 class="text-center">INICIAR SESIÓN</h1>
   
        <img src="../Recursos/IMG/LoginImgSID.png" /> <%--lOGO DEL LOGIN--%>

          <form id="formulario_login" runat="server" class="needs-validation">
               
                 
             
                   
                    <div class="form-group was-validated mb-2" >
                        <%--<asp:Label ID="lblUsuario" runat="server" Text="Usuario" CssClass="form-label"></asp:Label>--%>
                       <asp:TextBox ID="tbUsuario" runat="server" CssClass="form-control" placeholder="Ingrese el usuario"></asp:TextBox>
                        <div class="invalid-feedback"> Escriba correctamente el ususario</div>
                        </div>

                   <div class="form-group was-validated mb-2">
                      <%-- <asp:Label ID="lblPassword" runat="server" Text="Contraseña" CssClass="form-label"></asp:Label>--%>
                        <asp:TextBox ID="tbPassword" CssClass="form-control" TextMode="Password" runat="server" placeholder="Ingrese la contraseña"></asp:TextBox>
                       <div class="invalid-feedback"> Escriba correctamente la contraseña</div>
                    </div>
                
                 <div class="form-group form-check mb-2">
                       
                        <asp:TextBox ID="tbcheckbox" type="checkbox" runat="server" CssClass="form-check-input"></asp:TextBox>
                     <asp:Label ID="lblcheckbox" runat="server" Text="Recordar contraseña" CssClass="form-check-label"></asp:Label>
                    </div>
                
                   <div class="row">
                    <asp:Label runat="server" ID="lblError" CssClass="lblError"></asp:Label>
                    </div>   
                       
                    
                <div class="input-group">
                    <asp:Button ID="btbIngresar" runat="server" Text="Ingresar" OnClick="btbIngresar_Click" CssClass="btn btn-dark w-100"></asp:Button>
                 </div>   
               

                       </form>  
                 </div>
                   
               
          </div>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM" crossorigin="anonymous"></script>
        <div />
       <div />
        
     </body>
    
</html>