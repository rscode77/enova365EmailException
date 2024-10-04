using enova365EmailException.Common;
using enova365EmailException.Helpers;
using enova365EmailException.Services;
using enova365EmailException.Workers;
using Soneta.Business;
using Soneta.CRM;
using Soneta.CRM.Config;
using System;
using System.Linq;

[assembly: Worker(typeof(EmailExceptionWorker), typeof(Kontrahenci))]
namespace enova365EmailException.Workers
{
    public class EmailExceptionWorker
    {
        private readonly EmailService _emailService;
        private readonly KontoPocztowe _emailConfiguration;
        private readonly Log _log;
        private readonly SonetaCfgExtender _sonetaCfgExtender;

        private readonly string _receipientEmail;

        public EmailExceptionWorker(Session session)
        {
            // Inicjalizacja obiektu Log
            _log = new Log("EmailException", true);
            // Inicjalizacja SonetaCfgExtender z wykorzystaniem sesji
            _sonetaCfgExtender = new SonetaCfgExtender(session);

            // Pobranie adresu email odbiorcy z konfiguracji
            _receipientEmail = _sonetaCfgExtender.GetValue<string>(Constants.ReceipientEmailNode, string.Empty);
            // Pobranie zaznaczonego adresu email z konfiguracji
            var selectedEmailAccount = _sonetaCfgExtender.GetValue<string>(Constants.SelectedEmailNode, string.Empty);

            // Sprawdzenie, czy wybrano konto pocztowe
            if (string.IsNullOrEmpty(selectedEmailAccount))
            {
                LogError("Nie wybrano konta pocztowego będącego adresatem wiadomości.");
                return;
            }

            // Sprawdzenie, czy podano adres email odbiorcy
            if (string.IsNullOrEmpty(_receipientEmail))
            {
                LogError("Nie podano adresu email odbiorcy.");
                return;
            }

            // Pobranie konfiguracji konta email na podstawie nazwy konta
            _emailConfiguration = session.GetCRM().KontaPocztowe
                .OfType<KontoPocztowe>().FirstOrDefault(x => x.Nazwa == selectedEmailAccount);

            // Sprawdzenie, czy odnaleziono konfigurację konta email
            if (_emailConfiguration == null)
            {
                LogError("Nie odnaleziono konfiguracji konta email nadawcy.");
            }

            // Inicjalizacja usługi EmailService
            _emailService = new EmailService(_emailConfiguration);
        }

        [Action("Send exception", Icon = ActionIcon.Phone, Mode = ActionMode.SingleSession, Target = ActionTarget.ToolbarWithText)]
        public void SendEmailException()
        {
            var ex = new Exception("Testowy wyjątek");
            var exceptionTitle = "Testowy wyjątek";
            var message = "To jest testowa wiadomość.";

            string subject = $"Wystąpił wyjątek: {exceptionTitle}";
            string body = new HtmlEmailBody().ConstructExceptionEmailBody(ex, message);

            try
            {
                // Rozdzielenie adresów email odbiorcy
                var emailList = _receipientEmail.Split(';');

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
                LogError($"Wystąpił błąd podczas wysyłania EmailException: {e.Message}");
            }
        }

        // Metoda do logowania błędów
        public void LogError(string errorMessage)
        {
            _log.WriteLine(errorMessage);
        }
    }
}