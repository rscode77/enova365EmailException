using enova365EmailException.Common;
using enova365EmailException.Helpers;
using enova365EmailException.Services;
using enova365EmailException.Workers;
using Soneta.Business;
using System;

[assembly: Worker(typeof(EmailExceptionWorker))]
namespace enova365EmailException.Workers
{
    public class EmailExceptionWorker
    {
        private readonly EmailService _emailService;
        private readonly EmailConfigurationService _emailConfigurationService;
        private readonly ConfigManagerHelper _sonetaCfgExtender;
        private readonly Log _log;

        public EmailExceptionWorker(Session session)
        {
            // Inicjalizacja obiektu Log
            _log = new Log("EmailException", true);
            // Inicjalizacja SonetaCfgExtender
            _sonetaCfgExtender = new ConfigManagerHelper(session);
            // Inicjalizacja email configuration
            _emailConfigurationService = new EmailConfigurationService(session, _sonetaCfgExtender, _log);
            // Inicjalizacja usługi EmailService
            _emailService = new EmailService(_emailConfigurationService.GetEmailConfiguration());
        }

        public void SendEmailException(Exception ex, string exceptionTitle, string additionalMessage)
        {
            string subject = $"Wystąpił wyjątek: {exceptionTitle}";
            string body = new EmailBody().ConstructExceptionEmailBody(ex, additionalMessage);

            try
            {
                // Rozdzielenie adresów email odbiorcy
                var emailList = _emailConfigurationService.GetRecipientAddress().Split(';');

                foreach (var email in emailList)
                {
                    var trimmedEmail = email.Trim();
                    if (!string.IsNullOrEmpty(trimmedEmail))
                    {
                        // Wysyłanie wiadomości email
                        _emailService.SendEmailException(body, subject, trimmedEmail);
                    }
                }
            }
            catch (Exception e)
            {
                // Logowanie błędu w przypadku niepowodzenia wysyłki email
                _log.WriteLine($"Wystąpił błąd podczas wysyłania EmailException: {e.Message}");
            }
        }
    }
}