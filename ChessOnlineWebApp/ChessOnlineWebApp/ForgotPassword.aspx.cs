using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
namespace ChessOnlineWebApp
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SendToken_Click(object sender, EventArgs e)
        {
            string emailID = EmailID.Text;
            try
            {
                using (UserProfileServiceReference.UserProfileManagementServiceClient client = new UserProfileServiceReference.UserProfileManagementServiceClient())
                {
                    client.SendPasswordResetToken(emailID);
                }
                Response.Redirect("~/ResetPassword.aspx");
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