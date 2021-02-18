<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ChessOnlineWebApp.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <% if (!IsLoggedIn)
        {%>
    <h1>Welcome to Chess-Online</h1>
    <br />
    <h2>Have an account? <a href="Login.aspx">Login</a></h2>
    <h2>Need an account? <a href="Register.aspx">Register</a></h2>
    <%}
        else
        {%>
    <h2>Welcome, <% Response.Write(Session["username"]); %> </h2>
    <form runat="server">
        <asp:Button ID="logout_button" Text="Logout" OnClick="logout_button_Click" runat="server" />
    </form>

    <asp:Label ID="ErrorMessage" runat="server"></asp:Label>
    <%} %>
</body>
</html>
