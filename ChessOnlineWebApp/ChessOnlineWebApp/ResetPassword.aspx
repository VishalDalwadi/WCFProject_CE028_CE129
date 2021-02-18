<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="ChessOnlineWebApp.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h3>Reset your password</h3>
    <h5>Enter the token sent to you in your mail and enter a new password</h5>
    <form id="form1" runat="server">
        <div>
            
            Email Address:
            <asp:TextBox ID="EmailID" runat="server"></asp:TextBox>
            <br />
            <br />
            
            New Password:
            <asp:TextBox ID="NewPassword" TextMode="Password" runat="server"></asp:TextBox>
            <br />
            <br />
            Enter Token:
            <asp:TextBox ID="Token" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="ResetPass" runat="server" Text="Reset Password" OnClick="ResetPass_Click" />
            
            <br />
            <br />
            <asp:Label ID="ErrorLabel" ForeColor="Red" runat="server" Text=""></asp:Label>
            
        </div>
    </form>
</body>
</html>
