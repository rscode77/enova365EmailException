<h2>enova365EmailException</h2>

<h3>Opis</h3>

<b>enova365EmailException</b> to dodatek do systemu ERP enova365, który umożliwia automatyczne wysyłanie wiadomości e-mail zawierających szczegółowe informacje o wyjątkach (błędach) występujących w systemie. Dzięki temu administratorzy i deweloperzy mogą być natychmiast powiadamiani o problemach, co pozwala na szybką reakcję i minimalizację potencjalnych przestojów lub błędów w działaniu aplikacji.

Automatyczne powiadomienia e-mail: Po wystąpieniu wyjątku w systemie enova365, dodatek generuje i wysyła szczegółowy raport na wskazany adres e-mail.
Konfigurowalność: Możliwość dostosowania ustawień, takich jak adresy odbiorców, serwer SMTP, treść wiadomości czy poziom szczegółowości raportu.
Integracja z enova365: Bezproblemowa integracja z istniejącym środowiskiem enova365 bez konieczności wprowadzania znaczących zmian w konfiguracji systemu.

<h3>Konfiguracja</h3>

Dodatek konfigurujemy w systemowych ustawieniach enova365 w zakładce:

<b>EmailExceptions ➔ Konfiguracja email</b>

![obraz](https://github.com/user-attachments/assets/51a33c7e-3889-4e00-afc5-6af4916148c3)

<h3>Kroki konfiguracji:</h3>

<b>1. Wybór konta pocztowego:</b>
- Wybieramy skonfigurowane konto pocztowe, z którego będą wysyłane wiadomości e-mail.
- Konto powinno mieć uprawnienia do wysyłania wiadomości do odbiorców spoza organizacji (jeśli to konieczne).

<b>2. Dodanie adresów e-mail odbiorców:</b>
- W polu Adresy e-mail odbiorców wpisujemy adresy e-mail osób, które mają otrzymywać powiadomienia o wyjątkach.
- Adresy rozdzielamy znakiem specjalnym średnika ';'.

Przykład:

> admin@example.com;developer@example.com;support@example.com

<b>3. Zapisanie ustawień:</b>
- Po wprowadzeniu wszystkich danych klikamy przycisk Zapisz, aby zatwierdzić konfigurację.

> [!TIP]
> Sposób użycia w algorytmie aplikacji enova365

> var emailExceptionWorker = new enova365EmailException.Workers.EmailExceptionWorker(Session);
> emailExceptionWorker.SendEmailException(Exception ex, string emailTitle, string additionalMessage);

> [!NOTE]
> Zrzut przedstawiający fragment przesłanej wiadomości e-mail.

![email](https://github.com/user-attachments/assets/90bfa701-5e9e-4143-9780-87538f71137c)
