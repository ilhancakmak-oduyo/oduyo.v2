using Oduyo.Domain.DTOs;

namespace Oduyo.Infrastructure.Interfaces
{
    /// <summary>
    /// Google Calendar entegrasyonu için servis interface'i.
    /// Demo toplantıları için Google Meet linkleri oluşturur ve takvim etkinlikleri yönetir.
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// Google Calendar'da yeni bir etkinlik oluşturur.
        /// Google Meet linki otomatik olarak oluşturulur.
        /// </summary>
        /// <param name="eventDto">Etkinlik detayları</param>
        /// <returns>Oluşturulan etkinliğin Google Calendar Event ID'si</returns>
        Task<string> CreateEventAsync(CalendarEventDto eventDto);

        /// <summary>
        /// Mevcut bir takvim etkinliğini günceller.
        /// </summary>
        /// <param name="eventId">Google Calendar Event ID</param>
        /// <param name="eventDto">Güncellenmiş etkinlik detayları</param>
        Task UpdateEventAsync(string eventId, CalendarEventDto eventDto);

        /// <summary>
        /// Takvim etkinliğini siler (iptal eder).
        /// </summary>
        /// <param name="eventId">Google Calendar Event ID</param>
        Task DeleteEventAsync(string eventId);

        /// <summary>
        /// Takvim etkinliğinin detaylarını getirir.
        /// </summary>
        /// <param name="eventId">Google Calendar Event ID</param>
        /// <returns>Etkinlik detayları</returns>
        Task<CalendarEventDto> GetEventAsync(string eventId);

        /// <summary>
        /// Belirli bir tarih aralığındaki etkinlikleri listeler.
        /// </summary>
        /// <param name="startDate">Başlangıç tarihi</param>
        /// <param name="endDate">Bitiş tarihi</param>
        /// <returns>Etkinlik listesi</returns>
        Task<List<CalendarEventDto>> GetEventsAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Etkinliğe katılımcı ekler.
        /// </summary>
        /// <param name="eventId">Google Calendar Event ID</param>
        /// <param name="attendeeEmails">Eklenecek katılımcı e-postaları</param>
        Task AddAttendeesAsync(string eventId, List<string> attendeeEmails);

        /// <summary>
        /// Etkinliğe hatırlatma ekler.
        /// </summary>
        /// <param name="eventId">Google Calendar Event ID</param>
        /// <param name="minutesBefore">Etkinlikten kaç dakika önce hatırlatma gönderilecek</param>
        Task AddReminderAsync(string eventId, int minutesBefore);
    }
}
