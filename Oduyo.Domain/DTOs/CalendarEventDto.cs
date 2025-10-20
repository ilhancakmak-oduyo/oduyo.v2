namespace Oduyo.Domain.DTOs
{
    /// <summary>
    /// Google Calendar etkinliği için DTO
    /// </summary>
    public class CalendarEventDto
    {
        /// <summary>
        /// Etkinlik ID (Google Calendar Event ID)
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Etkinlik başlığı
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Etkinlik açıklaması
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Başlangıç zamanı
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Bitiş zamanı
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Lokasyon (fiziksel adres veya online meeting linki)
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Katılımcı e-postaları
        /// </summary>
        public List<string> Attendees { get; set; } = new List<string>();

        /// <summary>
        /// Google Meet linki
        /// </summary>
        public string MeetingLink { get; set; }

        /// <summary>
        /// Takvim ID (varsayılan: 'primary')
        /// </summary>
        public string CalendarId { get; set; } = "primary";

        /// <summary>
        /// Hatırlatmalar (dakika cinsinden)
        /// </summary>
        public List<int> Reminders { get; set; } = new List<int> { 30, 10 }; // 30 dakika ve 10 dakika önce

        /// <summary>
        /// Organizatör e-postası
        /// </summary>
        public string OrganizerEmail { get; set; }

        /// <summary>
        /// Renk ID (Google Calendar renk şeması)
        /// </summary>
        public string ColorId { get; set; }

        /// <summary>
        /// Etkinlik durumu (confirmed, tentative, cancelled)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Zaman dilimi
        /// </summary>
        public string TimeZone { get; set; } = "Europe/Istanbul";

        /// <summary>
        /// Tekrarlayan etkinlik kuralı (RRULE)
        /// </summary>
        public string RecurrenceRule { get; set; }
    }
}
