<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ChessOnlineWebApp.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Forgot your Password?</h3>
    <h5>Enter the Email Address of your account. A one time token to reset your password will be mailed to you.</h5>
    <div class="row">
        <div class="col-sm-4 form-group">
            
            <br />
            
            Email Address: <asp:TextBox ID="EmailID" CssClass="form-control" runat="server"></asp:TextBox>
            <br />
            <br />
            
      
        <asp:Button ID="SendToken" CssClass="btn btn-primary" runat="server" Text="Go" OnClick="SendToken_Click" />
        <br />
        <br />
        <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Text=""></asp:Label>
          </div>
    </div>
</asp:Content>