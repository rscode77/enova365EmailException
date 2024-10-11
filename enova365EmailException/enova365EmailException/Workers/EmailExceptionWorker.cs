using enova365EmailException.Common;
using enova365EmailException.Helpers;
using enova365EmailException.Services;
using enova365EmailException.Workers;
using Microsoft.Extensions.DependencyInjection;
using Soneta.Business;
using System;

[assembly: Worker(typeof(EmailExceptionWorker))]
namespace enova365EmailException.Workers
{
    public class EmailExceptionWorker
    {
        private readonly Log _log;
        private readonly ConfigManagerHelper _sonfigManagerHelper;
        private readonly EmailConfigurationService _emailConfigurationService;
        private readonly EmailService _emailService;

        public EmailExceptionWorker(Session session)
        {
            var services = new ServiceCollection();
            services.AddSingleton<Log>(provider => new Log("EmailException", true));
            services.AddTransient<ConfigManagerHelper>(provider => new ConfigManagerHelper(session));
            services.AddTransient<EmailConfigurationService>(provider =>
            {
                var log = provider.GetRequiredService<Log>();
                var configManagerHelper = provider.GetRequiredService<ConfigManagerHelper>();
                return new EmailConfigurationService(session, configManagerHelper, log);
            });
            services.AddTransient<EmailService>(provider =>
            {
                var emailConfigurationService = provider.GetRequiredService<EmailConfigurationService>();
                var emailConfiguration = emailConfigurationService.GetEmailConfiguration();
                return new EmailService(emailConfiguration);
            });

            var serviceProvider = services.BuildServiceProvider();
            _log = serviceProvider.GetRequiredService<Log>();
            _sonfigManagerHelper = serviceProvider.GetRequiredService<ConfigManagerHelper>();
            _emailConfigurationService = serviceProvider.GetRequiredService<EmailConfigurationService>();
            _emailService = serviceProvider.GetRequiredService<EmailService>();
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