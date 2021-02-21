<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ChessOnlineWebApp.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%: Page.Title %> Chess-Online </title>
    <link href="static/css/chessboard-1.0.0.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="static/js/jquery-3.5.1.min.js"></script>
    <script type="text/javascript" src="static/js/chess.js"></script>
    <script type="text/javascript" src="static/js/chessboard-1.0.0.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.signalR-2.2.2.min.js"></script>
    <script src="signalr/hubs"></script>

</head>
<body>
    <% if (!IsLoggedIn)
        {
            Response.Redirect("Login.aspx");
        }
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
                    <asp:Button ID="find_player_button" Text="Find Player" runat="server" OnClick="find_player_button_Click" />
                    <asp:Button ID="start_game_button" Text="Start Game" runat="server" OnClientClick="return false;" Enabled="false" />
                    <asp:Button ID="resign_game_button" Text="Resign Game" runat="server" OnClientClick="return false;" Enabled="false" />
                    <asp:Button ID="show_saved_games_button" Text="Saved Games" runat="server" OnClick="show_saved_games_button_Click" />
                    <input id="next_move_button" type="button" value="Next Move" disabled="disabled" />
                    <input id="previous_move_button" type="button" value="Previous Move" disabled="disabled" />
                    <asp:HiddenField ID="game_topic" runat="server" />
                </div>
                <br />
                <div id="data" style="border: 2px solid black;" runat="server">
                    <asp:Label ID="Message" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    </form>

    <script type="text/javascript">
        var game = {};

        $(function () {
            $.connection.gameHub.client.playingAs = function (_playing_as) {
                game.chess = new Chess();
                chessboard.start();
                if (_playing_as === "W") {
                    chessboard.orientation('white');
                    game.playing_as = 'w';
                } else {
                    chessboard.orientation('black');
                    game.playing_as = 'b';
                }
                let data = '<h3 style="padding-left: 2%;">You are playing as: ' + chessboard.orientation() + '</h3>' +
                    '<div id="game_string"></div>' +
                    '<h3 id="move" style="padding-left: 2%;"></h3>';
                $('#data').html(data);
                if (game.playing_as === 'w') {
                    $('#move').html('Make your move.');
                } else {
                    $('#move').html('Waiting for white to make a move.');
                }
            };

            $.connection.gameHub.client.playerResigned = function () {
                let retval = confirm('Player Resigned! Would you like to save the game?');
                if (retval) {
                    PageMethods.SaveGame(game.chess.pgn(), game.playing_as);
                }
                $.connection.gameHub.server.endGame(game_topic);
                chessboard.clear();
                $('#find_player_button').removeAttr('disabled');
                $('#show_saved_games_button').removeAttr('disabled');
                $('#resign_game_button').attr('disabled', 'disabled');
                $('#data').html('<h3 style="padding-left: 2%;">Player resigned.</h3>');
            };

            $.connection.gameHub.client.playMove = function (move) {
                game.chess.move(JSON.parse(move));
                chessboard.position(game.chess.fen());
                if (game.chess.in_checkmate()) {
                    let retval = confirm('You lost! Would you like to save the game?');
                    if (retval) {
                        PageMethods.SaveGame(game.chess.pgn(), game.playing_as);
                    }
                    $.connection.gameHub.server.endGame(game.game_topic);
                    $('#data').html('<h3>You lost!</h3>');
                } else if (game.chess.in_stalemate() || game.chess.in_threefold_repetition() || game.chess.insufficient_material()) {
                    let retval = confirm('Game ended in draw! Would you like to save the game?');
                    if (retval) {
                        PageMethods.SaveGame(game.chess.pgn(), game.playing_as);
                    }
                    $.connection.gameHub.server.endGame(game.game_topic);
                    $('#data').html('Game Draw!');
                } else {
                    $('#game_string').html('<p style="padding-left: 2%;">' + game.chess.pgn() + '</p>');
                    $('#move').html('Make your move.');
                }
            };

            $.connection.hub.start().done(function () {
                $('#start_game_button').click(function () {
                    let game_topic = $('#game_topic').val();
                    game.game_topic = game_topic;
                    $.connection.gameHub.server.startGame(game_topic);
                    $('#start_game_button').attr('disabled', 'disabled');
                    $('#resign_game_button').removeAttr('disabled');
                });

                $('#resign_game_button').click(function () {
                    $.connection.gameHub.server.resignGame(game.game_topic);
                    let retval = confirm('Would you like to save the game?');
                    if (retval) {
                        PageMethods.SaveGame(game.chess.pgn(), game.playing_as);
                    }
                    $.connection.gameHub.server.endGame(game.game_topic);
                    chessboard.clear();
                    $('#find_player_button').removeAttr('disabled');
                    $('#show_saved_games_button').removeAttr('disabled');
                    $('#resign_game_button').attr('disabled', 'disabled');
                    $('#data').html('');
                });
            });

            window.onbeforeunload = function () {
                $.connection.hub.stop();
            };
        });

        function onDragStart(source, piece, position, orientation) {
            // do not pick up pieces if the game is over
            if (game.chess.game_over()) return false

            if (game.chess.turn() !== game.playing_as) {
                return false;
            }

            if ((orientation === 'white' && piece.search(/^w/) === -1) ||
                (orientation === 'black' && piece.search(/^b/) === -1)) {
                return false
            }
        }

        function onDrop(source, target) {
            var move = game.chess.move({
                from: source,
                to: target,
                promotion: 'q'
            })

            if (move === null) return 'snapback'

            $.connection.gameHub.server.sendMove(game.game_topic, JSON.stringify(move));

            if (game.chess.in_checkmate()) {
                $('#data').html('<h3 style="padding-left: 2%;">You Win!</h3>');
                let retval = confirm('You won! Would you like to save the game?');
                if (retval) {
                    PageMethods.SaveGame(game.chess.pgn(), game.playing_as);
                }
                $.connection.gameHub.server.endGame(game.game_topic);
                $('#data').html('You won!');
            } else if (game.chess.in_stalemate() || game.chess.in_threefold_repetition() || game.chess.insufficient_material()) {
                let retval = confirm('Game ended in Draw! Would you like to save the game?');
                if (retval) {
                    PageMethods.SaveGame(game.chess.pgn(), game.playing_as);
                }
                $.connection.gameHub.server.endGame(game.game_topic);
                $('#data').html('Game Draw!');
            }

            $('#game_string').html('<p style="padding-left: 2%;">' + game.chess.pgn() + '</p>');
            if (game.playing_as === 'w') {
                $('#move').html('Waiting for black to make a move.');
            } else {
                $('#move').html('Waiting for white to make a move.');
            }
        }

        function onSnapEnd() {
            chessboard.position(game.chess.fen());
        }

        var chessboard = Chessboard('chessboard', {
            draggable: true,
            pieceTheme: 'static/img/chesspieces/wikipedia/{piece}.png',
            onDragStart: onDragStart,
            onDrop: onDrop,
            onSnapEnd: onSnapEnd
        });

        function play_game(gameString) {
            game.chess = new Chess();
            let saved_chess = new Chess();
            saved_chess.load_pgn(gameString);
            game.saved_moves = saved_chess.history();
            game.current_move = 0;
            chessboard.clear();
            chessboard.start();
            console.log(saved_chess.history());
            $('#data').html('');
            $('#next_move_button').removeAttr('disabled');
            $('#next_move_button').on('click', next_move);
            $('#previous_move_button').removeAttr('disabled');
            $('#previous_move_button').on('click', previous_move);
            next_move();
        }

        function delete_game(gameId) {
            PageMethods.DeleteGame(gameId);
        }

        function next_move() {
            if (game.current_move < game.saved_moves.length) {
                game.chess.move(game.saved_moves[game.current_move]);
                chessboard.position(game.chess.fen());
                game.current_move += 1;
            }
        }

        function previous_move() {
            if (game.current_move > 0) {
                game.chess.undo();
                chessboard.position(game.chess.fen());
                game.current_move -= 1;
            }
        }
    </script>
    <%} %>
</body>
</html>
