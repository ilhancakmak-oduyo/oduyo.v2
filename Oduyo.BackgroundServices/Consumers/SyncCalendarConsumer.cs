using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.BackgroundServices.Consumers
{
    public class SyncCalendarConsumer : IConsumer<SyncCalendarMessage>
    {
        private readonly ICalendarService _calendarService;
        private readonly IDemoService _demoService;
        private readonly ILogger<SyncCalendarConsumer> _logger;

        public SyncCalendarConsumer(
            ICalendarService calendarService,
            IDemoService demoService,
            ILogger<SyncCalendarConsumer> logger)
        {
            _calendarService = calendarService;
            _demoService = demoService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SyncCalendarMessage> context)
        {
            var message = context.Message;

            try
            {
                var eventId = await _calendarService.CreateEventAsync(new CalendarEventDto
                {
                    Title = message.Title,
                    StartTime = message.StartTime,
                    EndTime = message.EndTime,
                    Location = message.Location,
                    Attendees = message.Attendees
                });

                // Update demo with calendar event ID
                await _demoService.UpdateCalendarEventIdAsync(message.DemoId, eventId);

                _logger.LogInformation("Calendar event created for demo {DemoId}", message.DemoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync calendar for demo {DemoId}", message.DemoId);
                // Don't throw - calendar sync failure shouldn't break the flow
            }
        }
    }

    public interface ICalendarService
    {
        Task<string> CreateEventAsync(CalendarEventDto eventDto);
    }

    public class CalendarEventDto
    {
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public List<string> Attendees { get; set; }
    }
}
