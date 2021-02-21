<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ChessOnlineWebApp.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Register</h3>
    <div class="row">
    <div class="col-sm-4 form-group">
        Username:&nbsp;
        <asp:TextBox ID="Username" CssClass="form-control" runat="server"></asp:TextBox>
        <br />
        <br />
        Email ID:&nbsp;
        <asp:TextBox ID="EmailID" CssClass="form-control" runat="server"></asp:TextBox>
        <br />
        <br />
        Password:&nbsp;
        <asp:TextBox ID="Password" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button CssClass="btn btn-primary" ID="RegisterButton" runat="server" OnClick="Register_Click" Text="Register" />
        &nbsp;&nbsp;&nbsp;
        <a href="Login.aspx">Already have an account? Login</a>
        <br />
        <br />
        <asp:Label ID="StatusMsg" runat="server" Text=""></asp:Label>
    </div>
    </div>
</asp:Content>
