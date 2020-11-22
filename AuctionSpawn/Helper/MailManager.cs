using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AuctionSpawn.Helper
{
    public static class MailManager
    {
        public static bool SendHtmlEmail(string toEmailAddress, string subject, string htmlEmailBody)
        {
            bool success = false;

            // add from,to mailaddresses
            MailAddress from = new MailAddress(Configuration.FromEmailAddress);
            MailAddress to = new MailAddress(Configuration.ToEmailAddress);
            MailMessage myMail = new MailMessage(from, to);

            // set subject and encoding
            myMail.Subject = subject;
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Email body
            myMail.Body = htmlEmailBody;
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            myMail.IsBodyHtml = true;

            success = SendHtmlEmail(myMail);
            
            return success;
        }
        

        public static bool SendHtmlEmail(MailMessage myMail)
        {
            bool success = false;
            
            SmtpClient mySmtpClient = new SmtpClient();

            mySmtpClient.Host = Configuration.SmtpHost;
            mySmtpClient.Port = Configuration.SmtpPort;
            mySmtpClient.EnableSsl = Configuration.SmtpEnableSsl;

            mySmtpClient.UseDefaultCredentials = Configuration.SmtpUseDefaultCredentials;
            mySmtpClient.Credentials = new System.Net.NetworkCredential(Configuration.Username, Configuration.Pass);

            mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                mySmtpClient.Send(myMail);
                success = true;
                Console.WriteLine("Email Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email", ex);
            }
            
            return success;

        }

    }
}