using enova365EmailException.Common;
using enova365EmailException.Helpers;
using Soneta.Config;

namespace enova365EmailException.Entities
{
    internal class EmailItem
    {
        private readonly SonetaCfgExtender _sonetaCfgExtender;
        public string Email { get; }

        // Konstruktor klasy EmailItem, wykorzystujący Dependency Injection dla SonetaCfgExtender
        public EmailItem(SonetaCfgExtender sonetaCfgExtender, string email)
        {
            _sonetaCfgExtender = sonetaCfgExtender;
            Email = email;
        }

        // Właściwość określająca, czy dany email jest wybranym emailem
        // W przypadku ustawienia na true, zapisuje ten email jako wybrany w konfiguracji
        public bool SelectedEmail
        {
            get => _sonetaCfgExtender.GetValue<string>(Constants.SelectedEmailNode, string.Empty) == Email;
            set
            {
                if (value)
                {
                    // Jeśli wartość jest ustawiona na true, zapisuje aktualny email jako wybrany
                    _sonetaCfgExtender.SetValue(Constants.SelectedEmailNode, Email, AttributeType._string);
                }
                else
                {
                    // Jeśli wartość jest ustawiona na false, usuwa wybrany email, jeśli jest nim aktualny email
                    var currentSelected = _sonetaCfgExtender.GetValue<string>(Constants.SelectedEmailNode, string.Empty);
                    if (currentSelected == Email)
                    {
                        _sonetaCfgExtender.SetValue(Constants.SelectedEmailNode, string.Empty, AttributeType._string);
                    }
                }
            }
        }
    }
}
