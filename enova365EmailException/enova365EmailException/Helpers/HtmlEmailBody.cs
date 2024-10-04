using System;

namespace enova365EmailException.Helpers
{
    internal class HtmlEmailBody
    {
        // Metoda tworząca treść emaila w przypadku wystąpienia wyjątku
        public string ConstructExceptionEmailBody(Exception ex, string additionalMessage)
        {
            var body = @"
            <html>
            <head>
                <style>
                    body { font-family: Verdana, Geneva, sans-serif; line-height: 1.5; background-color: #f3f4f6; color: #333; }
                    .header { background-color: #6aab2e; color: #fff; padding: 15px; text-align: center; font-size: 1.6em; border-radius: 8px; }
                    .content { background-color: #ffffff; padding: 20px; margin: 20px auto; border-radius: 8px; max-width: 800px; border: 1px solid #c9c9c9; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); }
                    .exception-header { font-weight: bold; color: #003366; font-size: 1.2em; margin-top: 15px; }
                    .exception-info { margin-bottom: 15px; font-size: 1em; border-bottom: 1px solid #dcdcdc; padding-bottom: 10px; }
                    .stacktrace { background-color: #f5f5f5; padding: 10px; border: 1px solid #ccc; border-radius: 4px; font-family: Consolas, monospace; white-space: pre-wrap; overflow-x: auto; margin-top: 10px; }
                    .footer { background-color: #ececec; color: #777; padding: 15px; text-align: center; font-size: 0.9em; border-top: 1px solid #dcdcdc; border-radius: 8px; margin-top: 20px; }
                    a { color: #0070c0; text-decoration: none; }
                </style>
            </head>
            <body>
                <div class='header'>enova365 - Raport o wyjątku</div>
                <div class='content'>";

            // Dodanie dodatkowej wiadomości, jeśli jest dostępna
            if (!string.IsNullOrEmpty(additionalMessage))
            {
                body += $@"
                <div class='exception-info'>
                    <p class='exception-header'>Dodatkowa wiadomość:</p>
                    <p>{System.Net.WebUtility.HtmlEncode(additionalMessage)}</p>
                </div>";
            }

            // Dodanie szczegółów dotyczących wyjątku
            body += $@"
                <div class='exception-info'>
                    <p class='exception-header'>Typ wyjątku:</p>
                    <p>{System.Net.WebUtility.HtmlEncode(ex.GetType().FullName)}</p>
                </div>
                <div class='exception-info'>
                    <p class='exception-header'>Wiadomość:</p>
                    <p>{System.Net.WebUtility.HtmlEncode(ex.Message)}</p>
                </div>
                <div class='exception-info'>
                    <p class='exception-header'>Źródło:</p>
                    <p>{System.Net.WebUtility.HtmlEncode(ex.Source)}</p>
                </div>
                <div class='exception-info'>
                    <p class='exception-header'>Ślad stosu:</p>
                    <pre class='stacktrace'>{System.Net.WebUtility.HtmlEncode(ex.StackTrace)}</pre>
                </div>";

            // Dodanie informacji o wewnętrznym wyjątku, jeśli istnieje
            if (ex.InnerException != null)
            {
                body += $@"
                <div class='exception-info'>
                    <h3 class='exception-header'>-- Wewnętrzny wyjątek --</h3>
                    <p class='exception-header'>Typ wyjątku:</p>
                    <p>{System.Net.WebUtility.HtmlEncode(ex.InnerException.GetType().FullName)}</p>
                    <p class='exception-header'>Wiadomość:</p>
                    <p>{System.Net.WebUtility.HtmlEncode(ex.InnerException.Message)}</p>
                    <p class='exception-header'>Ślad stosu:</p>
                    <pre class='stacktrace'>{System.Net.WebUtility.HtmlEncode(ex.InnerException.StackTrace)}</pre>
                </div>";
            }

            body += @"
                </div>
                <div class='footer'>
                    enova365 - Automatyczna wiadomość o wyjątku.
                </div>
            </body>
            </html>";

            return body;
        }
    }
}