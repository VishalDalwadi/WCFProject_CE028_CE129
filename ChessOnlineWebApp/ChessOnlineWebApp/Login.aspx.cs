using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
namespace ChessOnlineWebApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;
            using (AuthenticationServiceReference.AuthenticationServiceClient client = new AuthenticationServiceReference.AuthenticationServiceClient())
            {
                try
                {
                    string token = client.AreCorrectCredentials(username, password);
                    Session["username"] = username;
                    HttpCookie token_cookie = new HttpCookie("token_cookie");
                    token_cookie.HttpOnly = true;
                    token_cookie.Value = token;
                    token_cookie.Expires = DateTime.Now.AddDays(15).AddSeconds(-1);
                    Response.Cookies.Add(token_cookie);
                    Response.Redirect("~/Home.aspx");
                }
                catch (FaultException<AuthenticationServiceReference.AuthenticationFault> ex)
                {
                    if (ex.Detail.FaultType == AuthenticationServiceReference.AuthenticationFault.AuthenticationFaultType.NoSuchUser)
                        ErrorLabel.Text = "User with the name " + username + " does not exist :/\n Check that you've entered the name correctly.\n";
                    if (ex.Detail.FaultType == AuthenticationServiceReference.AuthenticationFault.AuthenticationFaultType.InvalidPassword)
                        ErrorLabel.Text = "The password entered incorrect :/\n";
                    if (ex.Detail.FaultType == AuthenticationServiceReference.AuthenticationFault.AuthenticationFaultType.ServerFault)
                        ErrorLabel.Text = "The server encountered an error while processing your request. Please try again.\n";
                }
                catch (Exception)
                {
                    ErrorLabel.Text = "Username or Password is incorrect :/\n";
                }

            }
        }
    }
}