using enova365EmailException.Helpers;
using enova365EmailException.UI.Views;
using Soneta.Business;
using Soneta.Config;
using Soneta.CRM;
using Soneta.CRM.Config;
using System.Collections.Generic;
using System.Linq;

[assembly: Worker(typeof(EmailConfigurationPage))]

namespace enova365EmailException.UI.Views
{
    public class EmailItem
    {
        private readonly SonetaCfgExtender _sonetaCfgExtender;
        public string Email { get; }

        public EmailItem(SonetaCfgExtender sonetaCfgExtender, string email)
        {
            _sonetaCfgExtender = sonetaCfgExtender;
            Email = email;
        }
        public bool SelectedEmail
        {
            get => _sonetaCfgExtender.GetValue<string>("SelectedEmailString", string.Empty) == Email;
            set => _sonetaCfgExtender.SetValue("SelectedEmailString", value, AttributeType._string);
        }
    }

    public class EmailConfigurationPage
    {
        [Context]
        public Session Session { get; set; }

        private readonly SonetaCfgExtender _sonetaCfgExtender;
        public EmailConfigurationPage()
        {
            _sonetaCfgExtender = new SonetaCfgExtender(Session);
        }

        // List of email addresses with selection status
        public List<EmailItem> EmailAddresses => GetEmailItems();

        // Property for recipient email address
        public string RecipientEmail
        {
            get => _sonetaCfgExtender.GetValue<string>("RecipientEmail", string.Empty);
            set => _sonetaCfgExtender.SetValue("RecipientEmail", value, AttributeType._string);
        }

        // Method to retrieve email items
        private List<EmailItem> GetEmailItems()
        {
            return Session.GetCRM()
                          .KontaPocztowe
                          .OfType<KontoPocztowe>()
                          .Select(email => new EmailItem(_sonetaCfgExtender, email.Nazwa))
                          .ToList();
        }
    }
}
