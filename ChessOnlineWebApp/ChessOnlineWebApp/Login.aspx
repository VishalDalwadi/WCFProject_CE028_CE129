<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ChessOnlineWebApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <a href="Home.aspx">Back to Home</a>
    <form id="form1" runat="server">
        <h3>Login</h3>
        <asp:Label ID="MsgLabel" runat="server" ForeColor="Green" Text=""></asp:Label>
        <div>
            Username:
            <asp:TextBox ID="Username" runat="server"></asp:TextBox>
            <br />
            <br />
            Password:
            <asp:TextBox TextMode="Password" ID="Password" runat="server"></asp:TextBox>
            <br />
        <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Text=""></asp:Label>
            <br />
            <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" />
            
        </div>
    </form>
</body>
</html>
