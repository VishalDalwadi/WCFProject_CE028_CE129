using ChessOnlineWebApp.GamesManagementServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ChessOnlineWebApp
{
    public partial class Home : System.Web.UI.Page
    {
        protected bool IsLoggedIn = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            Message.Text = "";
            find_player_button.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(find_player_button, null) + "; game = {}; $('#data').html('');");
            HttpCookie token_cookie = Request.Cookies.Get("token_cookie");
            if (token_cookie != null)
            {
                try
                {
                    using (AuthorizationServiceReference.AuthorizationServiceClient authZClient =
                        new AuthorizationServiceReference.AuthorizationServiceClient())
                    {
                        AuthorizationServiceReference.User user = authZClient.AuthorizeUser(token_cookie.Value);
                        IsLoggedIn = true;
                        Session["username"] = user.Username;
                    }
                }
                catch (FaultException<AuthorizationServiceReference.AuthorizationFault>)
                {
                    Response.Cookies["token_cookie"].Expires = DateTime.Now.AddDays(-1);
                }

            }
        }

        protected void logout_button_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Cookies["token_cookie"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("~/Home.aspx");
        }

        protected void find_player_button_Click(object sender, EventArgs e)
        {
            HttpCookie token_cookie = Request.Cookies.Get("token_cookie");
            if (token_cookie != null)
            {
                try
                {
                    using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                    {
                        game_topic.Value = gmsClient.FindMatch(token_cookie.Value);
                        find_player_button.Enabled = false;
                        show_saved_games_button.Enabled = false;
                        start_game_button.Enabled = true;
                        data.InnerHtml = "";
                    }
                }
                catch (FaultException<GamesManagementServiceReference.GamesManagementFault>)
                {
                    Response.Cookies["token_cookie"].Expires = DateTime.Now.AddDays(-1);
                    Message.ForeColor = System.Drawing.Color.Red;
                    Message.Text = "Sorry for the inconvenience but we need you to login again!";
                    Response.Redirect("~/Login.aspx");
                    Response.AddHeader("REFRESH", "3;URL=Login.aspx");
                }
                catch (Exception)
                {
                    Message.ForeColor = System.Drawing.Color.Red;
                    Message.Text = "Sorry! Couldn't find a player for you. Please try again!";
                }
            }
        }

        [WebMethod]
        public static void SaveGame(string game_string, string played_as)
        {
            try
            {
                using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                {
                    HttpCookie token_cookie = HttpContext.Current.Request.Cookies.Get("token_cookie");
                    Game game = new Game()
                    {
                        GameString = game_string,
                        PlayedAs = played_as == "w" ? Game.Player.White : Game.Player.Black
                    };
                    gmsClient.SaveGame(game, token_cookie.Value);
                }
            }
            catch (Exception)
            {
                //
            }
        }

        [WebMethod]
        public static void DeleteGame(string game_id)
        {
            try
            {
                using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                {
                    HttpCookie token_cookie = HttpContext.Current.Request.Cookies.Get("token_cookie");
                    Game game = new Game()
                    {
                        GameId = Int64.Parse(game_id)
                    };
                    gmsClient.DeleteGame(game, token_cookie.Value);
                    HttpContext.Current.Response.Redirect("Home.aspx");
                }
            }
            catch (Exception)
            {
                //
            }
        }

        protected void show_saved_games_button_Click(object sender, EventArgs e)
        {
            try
            {
                using (GamesManagementServiceClient gmsClient = new GamesManagementServiceClient())
                {
                    HttpCookie token_cookie = HttpContext.Current.Request.Cookies.Get("token_cookie");
                    Game[] games = gmsClient.GetAllSavedGames(token_cookie.Value);
                    start_game_button.Enabled = false;
                    find_player_button.Enabled = true;
                    show_saved_games_button.Enabled = true;
                    if (games.Length == 0) return;
                    string html = "<table id='saved_games'><thead><tr><th>No.</th><th></th><th></th></tr></thead>";
                    for (int i = 0; i < games.Length; i++)
                    {
                        html += "<tr><td style='padding-left: 1%; width: 10%;'>" + (i+1) + "</td><td><input type='button' value='Play Game' onclick='play_game(\"" + games[i].GameString + "\");'></input></td><td><input type='button' value='Delete Game' onclick='delete_game(\"" + games[i].GameId + "\");'></input></td></tr>";
                    }
                    html += "</table>";
                    data.Attributes.Add("innerHtml", html);
                }
            }
            catch (Exception)
            {
                //
            }
        }
    }
}