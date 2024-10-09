using enova365EmailException.Common;
using enova365EmailException.Entities;
using enova365EmailException.Helpers;
using enova365EmailException.Views;
using Soneta.Business;
using Soneta.Config;
using Soneta.CRM;
using Soneta.CRM.Config;
using System.Collections.Generic;
using System.Linq;

[assembly: Worker(typeof(EmailConfigurationPage))]
namespace enova365EmailException.Views
{
    internal class EmailConfigurationPage
    {
        private readonly Session _session;
        private readonly ConfigManagerHelper _sonetaCfgExtender;
        public EmailConfigurationPage(Session session)
        {
            _session = session;
            _sonetaCfgExtender = new ConfigManagerHelper(_session);
        }

        // Właściwość zwracająca listę dostępnych adresów email
        // Tworzy obiekty EmailItem dla każdego konta pocztowego
        public List<EmailItem> EmailAddresses => GetEmailItems();

        // Metoda pomocnicza do pobierania listy kont pocztowych
        // Używa sesji, aby uzyskać dostęp do kont pocztowych CRM i tworzy listę obiektów EmailItem
        private List<EmailItem> GetEmailItems() => _session.GetCRM().KontaPocztowe.OfType<KontoPocztowe>()
                .Select(email => new EmailItem(_sonetaCfgExtender, email.Nazwa)).ToList();

        // Właściwość określająca adres email odbiorcy
        // Pozwala na pobieranie i ustawianie adresu email w konfiguracji za pomocą SonetaCfgExtender
        public string RecipientEmail
        {
            get => _sonetaCfgExtender.GetValue<string>(Constants.ReceipientEmailNode, string.Empty);
            set => _sonetaCfgExtender.SetValue(Constants.ReceipientEmailNode, value, AttributeType._string);
        }
    }
}

