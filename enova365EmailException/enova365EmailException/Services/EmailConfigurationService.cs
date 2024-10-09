using enova365EmailException.Common;
using enova365EmailException.Helpers;
using Soneta.Business;
using Soneta.CRM;
using Soneta.CRM.Config;
using System.Linq;

namespace enova365EmailException.Services
{
    internal class EmailConfigurationService
    {
        private readonly Session _session;
        private readonly ConfigManagerHelper _sonetaCfgExtender;
        private readonly Log _log;

        public EmailConfigurationService(Session session, ConfigManagerHelper sonetaCfgExtender, Log log)
        {
            _session = session;
            _sonetaCfgExtender = sonetaCfgExtender;
            _log = log;
        }

        public string GetRecipientAddress()
        {
            return GetConfigurationValue(Constants.ReceipientEmailNode, "Nie podano adresu email odbiorcy.");
        }

        public KontoPocztowe GetEmailConfiguration()
        {
            var selectedEmailAccount = GetConfigurationValue(Constants.SelectedEmailNode, "Nie wybrano konta pocztowego będącego adresatem wiadomości.");

            if (string.IsNullOrEmpty(selectedEmailAccount))
                return null;

            // Pobranie konfiguracji konta email na podstawie nazwy konta
            var emailConfiguration = _session.GetCRM().KontaPocztowe
                .OfType<KontoPocztowe>()
                .FirstOrDefault(x => x.Nazwa == selectedEmailAccount);

            // Sprawdzenie, czy odnaleziono konfigurację konta email
            if (emailConfiguration == null)
            {
                _log.WriteLine("Nie odnaleziono konfiguracji konta email nadawcy.");
            }

            return emailConfiguration;
        }

        private string GetConfigurationValue(string node, string errorMessage)
        {
            var value = _sonetaCfgExtender.GetValue<string>(node, string.Empty);
            if (string.IsNullOrEmpty(value))
            {
                _log.WriteLine(errorMessage);
            }
            return value;
        }
    }
}
