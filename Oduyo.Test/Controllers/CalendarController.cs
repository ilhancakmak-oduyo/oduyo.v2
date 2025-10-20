using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpPost("events")]
        public async Task<IActionResult> CreateEvent([FromBody] CalendarEventDto eventDto)
        {
            var eventId = await _calendarService.CreateEventAsync(eventDto);
            return Ok(new { EventId = eventId });
        }

        [HttpPut("events/{eventId}")]
        public async Task<IActionResult> UpdateEvent(string eventId, [FromBody] CalendarEventDto eventDto)
        {
            await _calendarService.UpdateEventAsync(eventId, eventDto);
            return Ok();
        }

        [HttpDelete("events/{eventId}")]
        public async Task<IActionResult> DeleteEvent(string eventId)
        {
            await _calendarService.DeleteEventAsync(eventId);
            return Ok();
        }

        [HttpGet("events/{eventId}")]
        public async Task<IActionResult> GetEvent(string eventId)
        {
            var calendarEvent = await _calendarService.GetEventAsync(eventId);
            if (calendarEvent == null)
                return NotFound();
            return Ok(calendarEvent);
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEvents([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var events = await _calendarService.GetEventsAsync(startDate, endDate);
            return Ok(events);
        }

        [HttpPost("events/{eventId}/attendees")]
        public async Task<IActionResult> AddAttendees(string eventId, [FromBody] List<string> attendeeEmails)
        {
            await _calendarService.AddAttendeesAsync(eventId, attendeeEmails);
            return Ok();
        }

        [HttpPost("events/{eventId}/reminder")]
        public async Task<IActionResult> AddReminder(string eventId, [FromBody] AddReminderDto dto)
        {
            await _calendarService.AddReminderAsync(eventId, dto.MinutesBefore);
            return Ok();
        }
    }

    public class AddReminderDto
    {
        public int MinutesBefore { get; set; }
    }
}
