using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChessOnlineWebApp
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Register_Click(object sender, EventArgs e)
        {
            string username = Username.Text;
            string email = EmailID.Text;
            string pwd = Password.Text;
            using (UserProfileServiceReference.UserProfileManagementServiceClient client = new UserProfileServiceReference.UserProfileManagementServiceClient())
            {
                string status_msg = null;
                bool username_taken = client.IsUsernameTaken(username);
                bool email_exists = client.UserWithEmailIdExists(email);
                if (username_taken)
                {
                    status_msg = "This username is unavailable :/ \n";
                }
                if (email_exists)
                {
                    status_msg += "An account with this email already exists -.- \n";
                }
                if (status_msg == null)
                {
                    UserProfileServiceReference.User user = new UserProfileServiceReference.User();
                    user.Username = username;
                    user.Password = pwd;
                    user.EmailID = email;
                    client.RegisterUser(user);
                    StatusMsg.ForeColor = System.Drawing.Color.Green;
                    StatusMsg.Text = "Registration Successful! Redirecting you to Login page ...";
                    Response.AddHeader("REFRESH", "3;URL=Login.aspx");
                }
                else
                {
                    StatusMsg.ForeColor = System.Drawing.Color.Red;
                    StatusMsg.Text = status_msg;
                }
            }
        }
    }
}