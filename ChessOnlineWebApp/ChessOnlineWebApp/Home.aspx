<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ChessOnlineWebApp.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="static/css/chessboard-1.0.0.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="static/js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript" src="static/js/chessboard-1.0.0.min.js"></script>
    <script type="text/javascript" src="static/js/chess.js"></script>
    <script src="Scripts/jquery-3.1.1.min.js" ></script>
    <script src="Scripts/jquery.signalR-2.2.1.min.js"></script>
    <script src="signalr/hubs"></script>

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
                    <input type="button" id="find_player_button" value="Find Player" onclick="find_player()"/>
                    <input type="button" id="start_game_button" value="Start Game" onclick="start_game()" disabled="disabled" />
                    <input type="button" id="resign_game_button" value="Resign Game" onclick="resign_game()" disabled="disabled" />
                    <input type="button" id="show_saved_games_button" value="Saved Games" onclick="show_saved_games()" />
                    <input type="button" id="next_move_button" value="Next Move" onclick="next_move()" disabled="disabled" />
                    <input type="button" id="previous_move_button" value="Previous Move" onclick="previous_move()" disabled="disabled" />
                </div>
                <br />
                <div id="data" style="border: 2px solid black;">
                    <asp:Label ID="label" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        var game_token = "";
        var game = $.connection.GameHub;
        var chessboard = Chessboard('chessboard', {
            draggable: false,
            position: 'start',
            pieceTheme: 'static/img/chesspieces/wikipedia/{piece}.png'
        });

        function find_player() {
            // disable find_player_button
            // if success:
            // enable start_game_button
            // set game_token
            // else:
            // enable find_player_button
        }

        function start_game() {
            // disable start_game_button
            // enable resign_game_button
        }

        function resign_game() {

        }

        function show_saved_games() {

        }

        function next_move() {

        }

        function previous_move() {

        }
    </script>
    <%} %>
</body>
</html>
