using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace EmailService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmailService" in both code and config file together.
    public class EmailService : IEmailService
    {
        void IEmailService.SendEmail(string email_id, string subject, string message, bool isMessageHtml)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(ConfigurationManager.AppSettings.Get("sender_email_id"),
                    email_id, subject, message)
                {
                    IsBodyHtml = isMessageHtml
                };
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("sender_email_id"),
                    ConfigurationManager.AppSettings.Get("sender_password")),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                throw new FaultException("Exception occured while sending email");
            }
        }
    }
}
