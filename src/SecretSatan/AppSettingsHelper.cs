using System.Configuration;

namespace SecretSatan
{
    internal static class AppSettingsHelper
    {
        public static string EmailSmtpUrl
        {
            get { return ConfigurationManager.AppSettings["EmailSmtpUrl"]; }
        }

        public static int EmailSmtpPort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["EmailSmtpPort"]); }
        }

        public static string EmailLogin
        {
            get { return ConfigurationManager.AppSettings["EmailLogin"]; }
        }

        public static string EmailPassword
        {
            get { return ConfigurationManager.AppSettings["EmailPassword"]; }
        }

        public static bool SmtpSslEnabled
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["SmtpSslEnabled"]); }
        }
        public static string SystemEmailAddress
        {
            get { return ConfigurationManager.AppSettings["SystemEmailAddress"]; }
        }
        public static string SystemEmailSenderName
        {
            get { return ConfigurationManager.AppSettings["SystemEmailSenderName"]; }
        }
    }
}