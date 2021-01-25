using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailSender
{
    static class EmailSenderClass
    {
        public static void Send(string to, string message)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            NetworkCredential credential = new NetworkCredential("dpeoject@gmail.com", "");
            client.Credentials = credential;

            MailAddress sender = new MailAddress("dpeoject@gmail.com", "Development Test Mail");

            MailAddress receiver = new MailAddress(to);

            MailMessage mail = new MailMessage(sender, receiver);

            mail.Subject = "Test";
            mail.Body = message;
            client.Send(mail);
        }
    }
}
