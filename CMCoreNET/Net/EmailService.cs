using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;

namespace CMCoreNET.Net
{
    public static class EmailService
    {
        public static void SendEmail(
            IEnumerable<string> toAddresses,
            string fromAddress,
            string subject,
            string body)
        {
            if (!toAddresses.Any() ||
                string.IsNullOrEmpty(subject) ||
                string.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException("SendEmail", "Invalid email request");
            }

            if (string.IsNullOrEmpty(fromAddress))
            {
                fromAddress = ConfigurationManager.AppSettings["Email.DefaultFrom"];
            }

            var message = new MailMessage();
            toAddresses.ForEach(message.To.Add);
            message.From = new MailAddress(fromAddress);
            message.Subject = subject;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;

            string host = ConfigurationManager.AppSettings["Email.Host"];
            int port = int.Parse(ConfigurationManager.AppSettings["Email.Port"]);

            SmtpClient client = new SmtpClient(host, port);
            client.Send(message);
        }
    }
}
