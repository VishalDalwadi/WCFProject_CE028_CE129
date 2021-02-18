<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ChessOnlineWebApp.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="static/css/chessboard-1.0.0.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="static/js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript" src="static/js/chessboard-1.0.0.min.js"></script>

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
    <form runat="server">
        <table style="padding-left: 2%; padding-right: 2%; width: 100%;">
            <tr>
                <td style="width: 50%;">
                    <h3 style="font-family: Arial, Helvetica, sans-serif;">Welcome, <% Response.Write(Session["username"]); %></h3>
                </td>
                <td style="width: 50%; text-align: right;">
                    <asp:Button ID="Button1" Text="Logout" OnClick="logout_button_Click" runat="server" />
                </td>
            </tr>
        </table>
        <div style="padding: 1%; padding-top: 0;">
            <div id="chessboard-container" style="width: 45%; display: table-cell; padding: 2%; border: 2px solid black; border-right: 1px solid black; vertical-align: top;">
                <div id="chessboard" style="width: 104%;"></div>
            </div>
            <div id="controls_and_data" style="width: 45%; display: table-cell; padding: 2%; border: 2px solid black; border-left: 1px solid black; vertical-align: top;">
                <div id="controls" style="border: 2px solid black; padding: 1%;">
                    <asp:Button ID="find_player_button" Text="Find Player" runat="server" />
                    <asp:Button ID="start_game_button" Text="Start Game" runat="server" Enabled="false" />
                    <asp:Button ID="resign_game_button" Text="Resign Game" runat="server" Enabled="false" />
                    <asp:Button ID="show_saved_game" Text="Saved Games" runat="server" />
                    <asp:Button ID="next_saved_move" Text="Next Move" runat="server" Enabled="false" />
                    <asp:Button ID="previous_saved_move" Text="Previous Move" runat="server" Enabled="false" />
                </div>
                <br />
                <div id="data" style="border: 2px solid black;">
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        var chessboard = Chessboard('chessboard', {
            draggable: false,
            position: 'start',
            pieceTheme: 'static/img/chesspieces/wikipedia/{piece}.png'
        });
    </script>
    <%} %>
</body>
</html>
