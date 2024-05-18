
using EnglishLearningProject.OptionsModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EnglishLearningProject.Services
{
    public class EmailService : IEmailService
      {

        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }


        public async Task SendResetPasswordEmail(string resetEmailLink, string ToEmail)
        {
            var smptClient = new SmtpClient();
            smptClient.Host = _emailSettings.host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(_emailSettings.senderEmail, _emailSettings.password);
            smptClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_emailSettings.senderEmail);
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "Localhost | Şifre sıfırlama link";
            mailMessage.Body = @$"<h4> Şifrenizi sıfırlamak için aşağıdaki linke tıklayınız.</h4>
              <p> <a href = '{resetEmailLink}' > Şifre Sıfırla </a> </p>
             ";
            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }
    }
}
