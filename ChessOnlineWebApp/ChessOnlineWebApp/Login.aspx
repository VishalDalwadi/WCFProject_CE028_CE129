<%@ Page Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ChessOnlineWebApp.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        <h3>Login</h3>
        <div class="row">
        <div class="col-sm-4 form-group">
            Username:
            <asp:TextBox ID="Username" CssClass="form-control" runat="server"></asp:TextBox>
            <br />
            <br />
            Password:
            <asp:TextBox TextMode="Password" CssClass="form-control" ID="Password" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button CssClass="btn btn-primary" ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <a href="Register.aspx">Or Create a new Account</a>
            <br />
            <br />
            <a href="ForgotPassword.aspx">Forgot Password?</a>
        </div>

        </div>
        <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Text=""></asp:Label>
                
</asp:Content>

