namespace Domain.Entities
{
    internal class EmailConfigurationData
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Recipient { get; set; }
    }
}
