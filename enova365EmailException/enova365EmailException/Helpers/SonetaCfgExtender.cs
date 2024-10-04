using enova365EmailException.Common;
using Soneta.Business;
using Soneta.Config;

namespace enova365EmailException.Helpers
{
    internal class SonetaCfgExtender
    {
        private readonly Session _session;

        // Konstruktor klasy SonetaCfgExtender, który przyjmuje sesję jako zależność
        public SonetaCfgExtender(Session session)
        {
            _session = session;
        }

        // Metoda ustawiająca wartość w konfiguracji
        // Używa sesji, aby ustawić wartość w określonym węźle konfiguracyjnym
        public void SetValue<T>(string name, T value, AttributeType type)
        {
            SetValue(_session, name, value, type);
        }

        // Metoda pobierająca wartość z konfiguracji
        // Jeśli wartość nie istnieje, zwraca wartość domyślną
        public T GetValue<T>(string name, T def)
        {
            return GetValue(_session, name, def);
        }

        // Prywatna metoda ustawiająca wartość w sesji
        // Dodaje nowy węzeł lub atrybut, jeśli nie istnieje
        private void SetValue<T>(Session session, string name, T value, AttributeType type)
        {
            using (var t = session.Logout(true))
            {
                var cfgManager = new CfgManager(session);

                // Znajdź lub dodaj główny węzeł konfiguracyjny
                var node1 = cfgManager.Root.FindSubNode(Constants.MainNode, false) ??
                            cfgManager.Root.AddNode(Constants.MainNode, CfgNodeType.Node);

                // Znajdź lub dodaj podrzędny węzeł konfiguracyjny
                var node2 = node1.FindSubNode(Constants.SubNode, false) ??
                            node1.AddNode(Constants.SubNode, CfgNodeType.Leaf);

                // Znajdź atrybut lub dodaj nowy, jeśli nie istnieje
                var attr = node2.FindAttribute(name, false);
                if (attr == null)
                    node2.AddAttribute(name, type, value);
                else
                    attr.Value = value;

                // Zatwierdzenie zmian do interfejsu użytkownika
                t.CommitUI();
            }
        }

        // Prywatna metoda pobierająca wartość z sesji
        // Wyszukuje odpowiedni węzeł i atrybut w konfiguracji
        private T GetValue<T>(Session session, string name, T def)
        {
            var cfgManager = new CfgManager(session);

            // Znajdź główny węzeł konfiguracyjny
            var node1 = cfgManager.Root.FindSubNode(Constants.MainNode, false);
            if (node1 == null) return def;

            // Znajdź podrzędny węzeł konfiguracyjny
            var node2 = node1.FindSubNode(Constants.SubNode, false);
            if (node2 == null) return def;

            // Znajdź atrybut i zwróć jego wartość, jeśli istnieje
            var attr = node2.FindAttribute(name, false);
            if (attr == null) return def;

            if (attr.Value == null) return def;

            return (T)attr.Value;
        }
    }
}
