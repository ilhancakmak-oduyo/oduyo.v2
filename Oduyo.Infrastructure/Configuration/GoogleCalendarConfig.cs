namespace Oduyo.Infrastructure.Configuration
{
    /// <summary>
    /// Google Calendar API yapılandırması.
    /// appsettings.json'dan okunur.
    /// </summary>
    public class GoogleCalendarConfig
    {
        /// <summary>
        /// Configuration section adı
        /// </summary>
        public const string SectionName = "GoogleCalendar";

        /// <summary>
        /// Google OAuth 2.0 Client ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Google OAuth 2.0 Client Secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// OAuth 2.0 Redirect URI
        /// </summary>
        public string RedirectUri { get; set; }

        /// <summary>
        /// Service Account JSON Key File yolu
        /// (Server-to-Server authentication için)
        /// </summary>
        public string ServiceAccountKeyPath { get; set; }

        /// <summary>
        /// Service Account Email
        /// </summary>
        public string ServiceAccountEmail { get; set; }

        /// <summary>
        /// Kullanılacak varsayılan takvim ID'si
        /// </summary>
        public string DefaultCalendarId { get; set; } = "primary";

        /// <summary>
        /// Application Name (Google API'ye gönderilir)
        /// </summary>
        public string ApplicationName { get; set; } = "Oduyo CRM";

        /// <summary>
        /// Google Meet otomatik olarak eklensin mi?
        /// </summary>
        public bool AutoAddGoogleMeet { get; set; } = true;

        /// <summary>
        /// Varsayılan hatırlatma süresi (dakika)
        /// </summary>
        public int DefaultReminderMinutes { get; set; } = 30;

        /// <summary>
        /// OAuth token'ları saklamak için kullanılacak yol
        /// </summary>
        public string TokenStoragePath { get; set; } = "./tokens";

        /// <summary>
        /// API istekleri için timeout (saniye)
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Retry policy için maksimum deneme sayısı
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;
    }
}
