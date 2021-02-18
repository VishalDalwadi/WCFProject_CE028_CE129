<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ChessOnlineWebApp.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <a href="Home.aspx">Back to Home</a>
    <form id="form1" runat="server">
        <h3>Register</h3>
        <div>
            Username:&nbsp;
            <asp:TextBox ID="Username" runat="server"></asp:TextBox>
            <br />
            <br />
            Email ID:&nbsp;
            <asp:TextBox ID="EmailID" runat="server"></asp:TextBox>
            <br />
            <br />
            Password:&nbsp;
            <asp:TextBox ID="Password" TextMode="Password" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="RegisterButton" runat="server" OnClick="Register_Click" Text="Register" />

            <br />
            <br />
            <asp:Label ID="StatusMsg" runat="server" Text=""></asp:Label>

        </div>
    </form>
</body>
</html>
