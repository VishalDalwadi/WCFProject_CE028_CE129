<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ChessOnlineWebApp.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
</head>
<body>
    <h3>Forgot your Password?</h3>
    <h5>Enter the Email Address of your account. A one time token to reset your password will be mailed to you.</h5>
    <form id="form1" runat="server">
        <div>
            
            Email Address: <asp:TextBox ID="EmailID" runat="server"></asp:TextBox>
            <br />
            <br />
            
        </div>
        <asp:Button ID="SendToken" runat="server" Text="Go" OnClick="SendToken_Click" />
        <br />
        <br />
        <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Text=""></asp:Label>
    </form>
</body>
</html>
