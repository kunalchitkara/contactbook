using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace ContactBook.Util
{
    public static class Email
    {
        public static bool SendEmail(string toEmailId, string subject, string emailBody, string fromEmailId = "", string ccEmailId = "", string bccEmailId = "", string toDisplayName = "", string fromDisplayName = "", bool withSignature = false)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                string smtpServer = AppSettings.GetSetting("smtp.server");
                string smtpUser = AppSettings.GetSetting("smtp.user");
                string smtpPassword = AppSettings.GetSetting("smtp.pwd");
                int smtpPort = Convert.ToInt16(AppSettings.GetSetting("smtp.port"));

                string[] toList = toEmailId.Split(',');
                if (toList.Length == 0)
                {
                    return false;
                }
                if (toDisplayName == "" && toList.Length == 1)
                {
                    toDisplayName = toEmailId;
                }
                if (fromDisplayName == "")
                {
                    fromDisplayName = fromEmailId;
                }
                if (fromEmailId == "")
                {
                    fromEmailId = AppSettings.GetSetting("smtp.from");
                    fromDisplayName = AppSettings.GetSetting("smtp.from_name");
                }
                fromDisplayName = AppSettings.GetSetting("smtp.from_name");
                MailboxAddress from = new MailboxAddress(fromDisplayName, fromEmailId);

                message.From.Add(from);
                foreach (var address in toList)
                {
                    message.To.Add(new MailboxAddress(address));
                }
                if (!string.IsNullOrEmpty(ccEmailId))
                {
                    message.Cc.Add(new MailboxAddress(ccEmailId));
                }
                bccEmailId = AppSettings.GetSetting("email.admin.recipients");
                if (!string.IsNullOrEmpty(bccEmailId))
                {
                    string[] bccList = bccEmailId.Split(',');
                    foreach (var address in bccList)
                    {
                        message.Bcc.Add(new MailboxAddress(address));
                    }
                }
                BodyBuilder bodyBuilder = new BodyBuilder();
                message.Subject = subject;
                if (withSignature)
                {
                    emailBody += "<p/>Cheers<br/>Team Magneefire";
                }
                bodyBuilder.HtmlBody = emailBody;
                message.Body = bodyBuilder.ToMessageBody();
                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(smtpServer, smtpPort);

                emailClient.Authenticate(smtpUser, smtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
                emailClient.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void SendTechLogs(string subject, string message)
        {
            string toAddress = AppSettings.GetSetting("email.error.recipients");
            SendEmail(toAddress, subject, AppSettings.GetSetting("app.url") + " " + message);
        }

        public static void SendTechLogs(string subject, Exception ex)
        {
            string toAddress = AppSettings.GetSetting("email.error.recipients");
            SendEmail(toAddress, subject, AppSettings.GetSetting("app.url") + " " + "\nMessage: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
        }

        public static void SendTechLogs(string subject, Exception ex, string details)
        {
            string toAddress = AppSettings.GetSetting("email.error.recipients");
            SendEmail(toAddress, subject, AppSettings.GetSetting("app.url") + " " + "\nMessage: " + ex.Message + "\nStack Trace: " + ex.StackTrace + "\nDetails: " + details);
        }

    }
}