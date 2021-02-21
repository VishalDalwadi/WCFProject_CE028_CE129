using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
namespace ChessOnlineWebApp
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ResetPass_Click(object sender, EventArgs e)
        {
            string npwd = NewPassword.Text;
            string token = Token.Text;
            string email_id = EmailID.Text;
            try
            {
                using (UserProfileServiceReference.UserProfileManagementServiceClient client = new UserProfileServiceReference.UserProfileManagementServiceClient())
                {
                    client.ResetPassword(token, email_id, npwd);
                    ErrorLabel.ForeColor = System.Drawing.Color.Green;
                    ErrorLabel.Text = "Password Reset Successfully! Redirecting you to Login page ...";
                    Response.AddHeader("REFRESH", "3;URL=Login.aspx");

                }
            }
            catch (FaultException ex)
            {
                ErrorLabel.Text = ex.Reason.ToString();
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Some unexpected error occurred :/";
                ErrorLabel.Text += ex.Message;
            }
        }
    }
}