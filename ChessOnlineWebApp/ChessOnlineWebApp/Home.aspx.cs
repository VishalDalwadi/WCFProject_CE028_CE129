using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChessOnlineWebApp
{
    public partial class Home : System.Web.UI.Page
    {
        protected bool IsLoggedIn = false;
        protected void Page_Load(object sender, EventArgs e)
        {
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
    }
}