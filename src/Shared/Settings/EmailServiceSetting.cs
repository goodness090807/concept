namespace Shared.Settings
{
    public class EmailServiceSetting
    {
        public const string SectionName = "EmailService";

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
