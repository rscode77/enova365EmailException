using Soneta.Business;
using Soneta.CRM;
using Soneta.CRM.Config;

namespace enova365EmailException.Services
{
    internal class EmailService
    {
        private readonly KontoPocztowe _emailConfigurationData;
        private readonly Session _session;

        public EmailService(KontoPocztowe emailConfigurationData)
        {
            _emailConfigurationData = emailConfigurationData;
            _session = emailConfigurationData.Session;
        }

        // Metoda do wysyłania wiadomości email
        public void SendEmailException(string body, string title, string address)
        {
            using (var s = _session.Login.CreateSession(false, true, "Sending email"))
            {
                using (ITransaction tt = s.Logout(true))
                {
                    var crm = s.GetCRM();

                    // Utworzenie wiadomości email z odpowiednią konfiguracją
                    var emailMessage = new WiadomoscRobocza
                    {
                        KontoPocztowe = _emailConfigurationData,
                        Temat = title,
                        Do = address,
                        Tresc = body
                    };

                    // Dodanie wiadomości do systemu CRM
                    crm.WiadomosciEmail.AddRow(emailMessage);

                    // Zatwierdzenie transakcji
                    tt.Commit();

                    // Wysłanie wiadomości przy użyciu MailHelper
                    MailHelper.SendMessage(emailMessage);
                }
            }
        }
    }
}