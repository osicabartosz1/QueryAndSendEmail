using System.Net;
using System.Net.Mail;
using System.Xml;

namespace QueryAndSendEmail
{
    public class Mail
    {

        public static string FromMail = "";
        public static string Password = "";
        public static string ToMail = "";

        string Author;
        string Subject;
        string Body;

        public Mail(string author, string subject, string body)
        {
            Author = author;
            Subject = subject;
            Body = body;
        }

        public static void SendProgressMail2Step()
        {
            SendMail2Step("smtp.gmail.com", 587, FromMail, Password, ToMail, Query.DataBaseName + "Query", Query.GetProgressTable(), Array.Empty<string>());
            Console.WriteLine("Mail has been sent");
        }

        public static void watchMail2Step()
        {
            Mail mail = null;
            try
            {
                mail = GetNewestMail(FromMail, Password);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            if (mail is null)
            {
                Console.WriteLine("Mailbox empty");
                return;
            }
            else 
            {
                Console.WriteLine($"Code has found mail from {mail.Author} with title {mail.Subject}");
                ToMail = mail.Author;
                switch (mail.Subject) 
                {
                    case "GetProgress": { SendProgressMail2Step(); break; }
                    default: { break; }
                }
            }
            Console.WriteLine("Mail has been check");
        }

        private static Mail GetNewestMail(string FromMail, string Password)
        {
            string encoded = TextToBase64(FromMail + ":" + Password);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, @"https://mail.google.com/mail/feed/atom");
            HttpClient httpClient = new HttpClient();

            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", "Basic" + encoded);
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

            Stream stream = httpResponseMessage.Content.ReadAsStream();
            XmlReader reader = XmlReader.Create(stream);
            XmlDocument doc = new XmlDocument();

            doc.Load(reader);

            int numberOfUnreadedMails = doc.GetElementsByTagName("entry").Count;
            if (numberOfUnreadedMails != 0)
            {
                string title = doc.SelectSingleNode(@"//*[local-name()='entry'][1]/*[local-name()='title']").InnerText;
                string summary = doc.SelectSingleNode(@"//*[local-name()='entry'][1]/*[local-name()='summary']").InnerText;
                string email = doc.SelectSingleNode(@"//*[local-name()='entry'][1]//*[local-name()='email']").InnerText;
                return new Mail(email, title, summary);
            }
            else 
            {
                return null;
            }
        }
        public static string TextToBase64(string S)
        {
            System.Text.ASCIIEncoding encode = new System.Text.ASCIIEncoding();
            byte[] bytes = encode.GetBytes(S);
            return System.Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        private static void SendMail2Step(string SMTPServer, int SMTP_Port, string FromMail, string Password, string ToMail, string Subject, string Body, string[] FileNames)
        {
            var smtpClient = new SmtpClient(SMTPServer, SMTP_Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            smtpClient.Credentials = new NetworkCredential(FromMail, Password);
            var message = new MailMessage(new MailAddress(FromMail, "MailForC#"), new MailAddress(ToMail, ToMail));
            message.CC.Add(FromMail);
            message.Subject = Subject;
            message.Body = Body;
            smtpClient.Send(message);
        }
    }
}