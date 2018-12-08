using ADYC.Util.Interfaces;
using System;
using System.Net.Mail;

namespace ADYC.Util.Messages
{
    public class EmailMesageService : IMessageService
    {
        public bool Sent(string to, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("j.arriaga.apps@gmail.com", "EqVFgAgsFW4l");

                MailMessage mm = new MailMessage("admin@adyc.com", to, subject, body);

                client.Send(mm);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
