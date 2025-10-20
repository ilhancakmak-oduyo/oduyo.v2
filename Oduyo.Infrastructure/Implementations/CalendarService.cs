using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Configuration;
using Oduyo.Infrastructure.Interfaces;
using GoogleCalendarService = Google.Apis.Calendar.v3.CalendarService;

namespace Oduyo.Infrastructure.Implementations
{
    /// <summary>
    /// Google Calendar API entegrasyonu için servis implementasyonu.
    /// Service Account kullanarak server-to-server authentication yapar.
    /// </summary>
    public class CalendarService : ICalendarService
    {
        private readonly GoogleCalendarConfig _config;
        private readonly ILogger<CalendarService> _logger;
        private GoogleCalendarService _googleCalendarService;

        public CalendarService(
            IOptions<GoogleCalendarConfig> config,
            ILogger<CalendarService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        /// <summary>
        /// Google Calendar servisini başlatır (lazy initialization)
        /// </summary>
        private async Task<GoogleCalendarService> GetCalendarServiceAsync()
        {
            if (_googleCalendarService != null)
                return _googleCalendarService;

            try
            {
                // Service Account credentials yükleme
                GoogleCredential credential;

                if (!string.IsNullOrEmpty(_config.ServiceAccountKeyPath) &&
                    File.Exists(_config.ServiceAccountKeyPath))
                {
                    // JSON key file'dan credentials yükleme (CredentialFactory ile güvenli yöntem)
                    var jsonContent = await File.ReadAllTextAsync(_config.ServiceAccountKeyPath);
                    var serviceAccountCredential = ServiceAccountCredential.FromServiceAccountData(
                        new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonContent)));

                    credential = GoogleCredential.FromServiceAccountCredential(serviceAccountCredential)
                        .CreateScoped(GoogleCalendarService.Scope.Calendar);

                    _logger.LogInformation("Google Calendar credentials loaded from file: {Path}",
                        _config.ServiceAccountKeyPath);
                }
                else
                {
                    // Environment variables veya Application Default Credentials kullanma
                    credential = await GoogleCredential.GetApplicationDefaultAsync();
                    credential = credential.CreateScoped(GoogleCalendarService.Scope.Calendar);

                    _logger.LogInformation("Google Calendar credentials loaded from application default");
                }

                // Calendar service oluşturma
                _googleCalendarService = new GoogleCalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = _config.ApplicationName,
                });

                _logger.LogInformation("Google Calendar service initialized successfully");

                return _googleCalendarService;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Google Calendar service");
                throw new InvalidOperationException("Google Calendar service could not be initialized", ex);
            }
        }

        /// <summary>
        /// Google Calendar'da yeni bir etkinlik oluşturur.
        /// Google Meet linki otomatik olarak eklenir.
        /// </summary>
        public async Task<string> CreateEventAsync(CalendarEventDto eventDto)
        {
            try
            {
                var service = await GetCalendarServiceAsync();

                // Google Calendar Event nesnesi oluşturma
                var calendarEvent = new Event
                {
                    Summary = eventDto.Title,
                    Description = eventDto.Description,
                    Location = eventDto.Location,
                    Start = new EventDateTime
                    {
                        DateTimeDateTimeOffset = new DateTimeOffset(eventDto.StartTime),
                        TimeZone = eventDto.TimeZone
                    },
                    End = new EventDateTime
                    {
                        DateTimeDateTimeOffset = new DateTimeOffset(eventDto.EndTime),
                        TimeZone = eventDto.TimeZone
                    },
                    Attendees = eventDto.Attendees?.Select(email => new EventAttendee
                    {
                        Email = email,
                        ResponseStatus = "needsAction"
                    }).ToList(),
                    Reminders = new Event.RemindersData
                    {
                        UseDefault = false,
                        Overrides = eventDto.Reminders?.Select(minutes => new EventReminder
                        {
                            Method = "email",
                            Minutes = minutes
                        }).ToList() ?? new List<EventReminder>
                        {
                            new EventReminder { Method = "email", Minutes = _config.DefaultReminderMinutes },
                            new EventReminder { Method = "popup", Minutes = 10 }
                        }
                    },
                    Status = "confirmed"
                };

                // Google Meet conference ekleme
                if (_config.AutoAddGoogleMeet)
                {
                    calendarEvent.ConferenceData = new ConferenceData
                    {
                        CreateRequest = new CreateConferenceRequest
                        {
                            RequestId = Guid.NewGuid().ToString(),
                            ConferenceSolutionKey = new ConferenceSolutionKey
                            {
                                Type = "hangoutsMeet"
                            }
                        }
                    };
                }

                // Renk ayarlama (opsiyonel)
                if (!string.IsNullOrEmpty(eventDto.ColorId))
                {
                    calendarEvent.ColorId = eventDto.ColorId;
                }

                // Tekrarlayan etkinlik kuralı (opsiyonel)
                if (!string.IsNullOrEmpty(eventDto.RecurrenceRule))
                {
                    calendarEvent.Recurrence = new List<string> { eventDto.RecurrenceRule };
                }

                // Event oluşturma isteği
                var request = service.Events.Insert(calendarEvent, eventDto.CalendarId ?? _config.DefaultCalendarId);

                // Conference data için gerekli
                if (_config.AutoAddGoogleMeet)
                {
                    request.ConferenceDataVersion = 1;
                }

                request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All;

                // Event oluşturma
                var createdEvent = await request.ExecuteAsync();

                _logger.LogInformation(
                    "Calendar event created successfully. Event ID: {EventId}, Title: {Title}",
                    createdEvent.Id,
                    eventDto.Title);

                // Google Meet linkini loglama
                if (createdEvent.ConferenceData?.EntryPoints != null)
                {
                    var meetLink = createdEvent.ConferenceData.EntryPoints
                        .FirstOrDefault(ep => ep.EntryPointType == "video")?.Uri;

                    if (!string.IsNullOrEmpty(meetLink))
                    {
                        _logger.LogInformation("Google Meet link created: {MeetLink}", meetLink);
                    }
                }

                return createdEvent.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create calendar event: {Title}", eventDto.Title);
                throw new InvalidOperationException($"Failed to create calendar event: {eventDto.Title}", ex);
            }
        }

        /// <summary>
        /// Mevcut bir takvim etkinliğini günceller.
        /// </summary>
        public async Task UpdateEventAsync(string eventId, CalendarEventDto eventDto)
        {
            try
            {
                var service = await GetCalendarServiceAsync();

                // Mevcut eventi getir
                var existingEvent = await service.Events.Get(
                    eventDto.CalendarId ?? _config.DefaultCalendarId,
                    eventId).ExecuteAsync();

                // Güncelleme
                existingEvent.Summary = eventDto.Title ?? existingEvent.Summary;
                existingEvent.Description = eventDto.Description ?? existingEvent.Description;
                existingEvent.Location = eventDto.Location ?? existingEvent.Location;

                if (eventDto.StartTime != default)
                {
                    existingEvent.Start = new EventDateTime
                    {
                        DateTimeDateTimeOffset = new DateTimeOffset(eventDto.StartTime),
                        TimeZone = eventDto.TimeZone
                    };
                }

                if (eventDto.EndTime != default)
                {
                    existingEvent.End = new EventDateTime
                    {
                        DateTimeDateTimeOffset = new DateTimeOffset(eventDto.EndTime),
                        TimeZone = eventDto.TimeZone
                    };
                }

                if (eventDto.Attendees != null && eventDto.Attendees.Any())
                {
                    existingEvent.Attendees = eventDto.Attendees.Select(email => new EventAttendee
                    {
                        Email = email,
                        ResponseStatus = "needsAction"
                    }).ToList();
                }

                // Update isteği
                var request = service.Events.Update(
                    existingEvent,
                    eventDto.CalendarId ?? _config.DefaultCalendarId,
                    eventId);

                request.SendUpdates = EventsResource.UpdateRequest.SendUpdatesEnum.All;

                await request.ExecuteAsync();

                _logger.LogInformation("Calendar event updated successfully. Event ID: {EventId}", eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update calendar event: {EventId}", eventId);
                throw new InvalidOperationException($"Failed to update calendar event: {eventId}", ex);
            }
        }

        /// <summary>
        /// Takvim etkinliğini siler (iptal eder).
        /// </summary>
        public async Task DeleteEventAsync(string eventId)
        {
            try
            {
                var service = await GetCalendarServiceAsync();

                var request = service.Events.Delete(_config.DefaultCalendarId, eventId);
                request.SendUpdates = EventsResource.DeleteRequest.SendUpdatesEnum.All;

                await request.ExecuteAsync();

                _logger.LogInformation("Calendar event deleted successfully. Event ID: {EventId}", eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete calendar event: {EventId}", eventId);
                throw new InvalidOperationException($"Failed to delete calendar event: {eventId}", ex);
            }
        }

        /// <summary>
        /// Takvim etkinliğinin detaylarını getirir.
        /// </summary>
        public async Task<CalendarEventDto> GetEventAsync(string eventId)
        {
            try
            {
                var service = await GetCalendarServiceAsync();

                var calendarEvent = await service.Events.Get(_config.DefaultCalendarId, eventId).ExecuteAsync();

                // Google Meet linkini al
                var meetLink = calendarEvent.ConferenceData?.EntryPoints
                    ?.FirstOrDefault(ep => ep.EntryPointType == "video")?.Uri;

                return new CalendarEventDto
                {
                    EventId = calendarEvent.Id,
                    Title = calendarEvent.Summary,
                    Description = calendarEvent.Description,
                    StartTime = calendarEvent.Start.DateTimeDateTimeOffset?.DateTime ?? DateTime.Parse(calendarEvent.Start.Date),
                    EndTime = calendarEvent.End.DateTimeDateTimeOffset?.DateTime ?? DateTime.Parse(calendarEvent.End.Date),
                    Location = calendarEvent.Location,
                    MeetingLink = meetLink,
                    Attendees = calendarEvent.Attendees?.Select(a => a.Email).ToList() ?? new List<string>(),
                    Status = calendarEvent.Status,
                    TimeZone = calendarEvent.Start.TimeZone ?? "Europe/Istanbul"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get calendar event: {EventId}", eventId);
                throw new InvalidOperationException($"Failed to get calendar event: {eventId}", ex);
            }
        }

        /// <summary>
        /// Belirli bir tarih aralığındaki etkinlikleri listeler.
        /// </summary>
        public async Task<List<CalendarEventDto>> GetEventsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var service = await GetCalendarServiceAsync();

                var request = service.Events.List(_config.DefaultCalendarId);
                request.TimeMinDateTimeOffset = new DateTimeOffset(startDate);
                request.TimeMaxDateTimeOffset = new DateTimeOffset(endDate);
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var events = await request.ExecuteAsync();

                return events.Items?.Select(e => new CalendarEventDto
                {
                    EventId = e.Id,
                    Title = e.Summary,
                    Description = e.Description,
                    StartTime = e.Start.DateTimeDateTimeOffset?.DateTime ?? DateTime.Parse(e.Start.Date),
                    EndTime = e.End.DateTimeDateTimeOffset?.DateTime ?? DateTime.Parse(e.End.Date),
                    Location = e.Location,
                    MeetingLink = e.ConferenceData?.EntryPoints
                        ?.FirstOrDefault(ep => ep.EntryPointType == "video")?.Uri,
                    Attendees = e.Attendees?.Select(a => a.Email).ToList() ?? new List<string>(),
                    Status = e.Status
                }).ToList() ?? new List<CalendarEventDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get calendar events");
                throw new InvalidOperationException("Failed to get calendar events", ex);
            }
        }

        /// <summary>
        /// Etkinliğe katılımcı ekler.
        /// </summary>
        public async Task AddAttendeesAsync(string eventId, List<string> attendeeEmails)
        {
            try
            {
                var service = await GetCalendarServiceAsync();

                var calendarEvent = await service.Events.Get(_config.DefaultCalendarId, eventId).ExecuteAsync();

                // Mevcut katılımcıları al
                var existingAttendees = calendarEvent.Attendees?.ToList() ?? new List<EventAttendee>();

                // Yeni katılımcıları ekle
                foreach (var email in attendeeEmails)
                {
                    if (!existingAttendees.Any(a => a.Email == email))
                    {
                        existingAttendees.Add(new EventAttendee
                        {
                            Email = email,
                            ResponseStatus = "needsAction"
                        });
                    }
                }

                calendarEvent.Attendees = existingAttendees;

                // Güncelleme
                var request = service.Events.Update(calendarEvent, _config.DefaultCalendarId, eventId);
                request.SendUpdates = EventsResource.UpdateRequest.SendUpdatesEnum.All;

                await request.ExecuteAsync();

                _logger.LogInformation(
                    "Attendees added to calendar event. Event ID: {EventId}, Count: {Count}",
                    eventId,
                    attendeeEmails.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add attendees to calendar event: {EventId}", eventId);
                throw new InvalidOperationException($"Failed to add attendees to calendar event: {eventId}", ex);
            }
        }

        /// <summary>
        /// Etkinliğe hatırlatma ekler.
        /// </summary>
        public async Task AddReminderAsync(string eventId, int minutesBefore)
        {
            try
            {
                var service = await GetCalendarServiceAsync();

                var calendarEvent = await service.Events.Get(_config.DefaultCalendarId, eventId).ExecuteAsync();

                // Mevcut hatırlatmaları al
                var reminders = calendarEvent.Reminders?.Overrides?.ToList() ?? new List<EventReminder>();

                // Yeni hatırlatma ekle
                if (!reminders.Any(r => r.Minutes == minutesBefore))
                {
                    reminders.Add(new EventReminder
                    {
                        Method = "email",
                        Minutes = minutesBefore
                    });

                    calendarEvent.Reminders = new Event.RemindersData
                    {
                        UseDefault = false,
                        Overrides = reminders
                    };

                    // Güncelleme
                    var request = service.Events.Update(calendarEvent, _config.DefaultCalendarId, eventId);
                    await request.ExecuteAsync();

                    _logger.LogInformation(
                        "Reminder added to calendar event. Event ID: {EventId}, Minutes: {Minutes}",
                        eventId,
                        minutesBefore);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add reminder to calendar event: {EventId}", eventId);
                throw new InvalidOperationException($"Failed to add reminder to calendar event: {eventId}", ex);
            }
        }
    }
}
