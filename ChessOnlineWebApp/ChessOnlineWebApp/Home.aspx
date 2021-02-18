<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ChessOnlineWebApp.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <% if (Request.Cookies.Get("token_cookie") == null)
        {%>
    <h1>Welcome to Chess-Online</h1>
    <br />
    <h2>Have an account? <a href="Login.aspx">Login</a></h2>
    <h2>Need an account? <a href="Register.aspx">Register</a></h2>
    <%}
    else
    {%>
    <h2>Welcome, <% Response.Write(Session["username"]); %> </h2>
    <%} %>
</body>
</html>
