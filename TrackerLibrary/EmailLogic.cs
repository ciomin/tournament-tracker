using System.Net.Mail;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(string to, string subject, string body)
        {
            SendEmail(new List<string> { to }, new List<string>(), subject, body);
        }

        public static void SendEmail(List<string> to, List<string> bcc, string subject, string body)
        {
            // Check if sending emails is enabled
            if (GlobalConfig.AppKeyLookup("EmailSettings:SendEmails")?.ToLower() != "true")
            {
                return; // Exit the method without sending the email
            }

            // Otherwise proceed with sending the emails
            MailAddress fromMailAddress = new MailAddress(
                GlobalConfig.AppKeyLookup("EmailSettings:SenderEmail"),
                GlobalConfig.AppKeyLookup("EmailSettings:SenderDisplayName")
            );

            MailMessage mail = new MailMessage();
            foreach (string email in to)
            {
                mail.To.Add(email); 
            }
            
            foreach (string email in bcc)
            {
                mail.Bcc.Add(email); 
            }

            mail.From = fromMailAddress;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient
            {
                Host = GlobalConfig.AppKeyLookup("EmailSettings:SmtpServer"),
                Port = int.Parse(GlobalConfig.AppKeyLookup("EmailSettings:SmtpPort")),
                EnableSsl = false,
                Credentials = new System.Net.NetworkCredential(
                    GlobalConfig.AppKeyLookup("EmailSettings:SmtpUsername"),
                    GlobalConfig.AppKeyLookup("EmailSettings:SmtpPassword"))
            };


            client.Send(mail);
        }

    }
}
