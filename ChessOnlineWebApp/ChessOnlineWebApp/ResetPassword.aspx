<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="ChessOnlineWebApp.ResetPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Reset your password</h3>
    <h5>Enter the token sent to you in your mail and enter a new password</h5>
    <div class="row">
        <div class="col-sm-4 form-group">
            
            Email Address:
            <asp:TextBox ID="EmailID" CssClass="form-control" runat="server"></asp:TextBox>
            <br />
            <br />
            
            New Password:
            <asp:TextBox ID="NewPassword" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
            <br />
            <br />
            Enter Token:
            <asp:TextBox ID="Token" CssClass="form-control" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="ResetPass" CssClass="btn btn-primary" runat="server" Text="Reset Password" OnClick="ResetPass_Click" />
            
            <br />
            <br />
            <asp:Label ID="ErrorLabel" ForeColor="Red" runat="server" Text=""></asp:Label>
            
        </div>
        </div>
</asp:Content>
