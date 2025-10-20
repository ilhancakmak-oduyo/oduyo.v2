using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuditLogDto dto)
        {
            var auditLog = await _auditLogService.CreateAuditLogAsync(dto);
            return Ok(auditLog);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserAuditLogs(int userId)
        {
            var auditLogs = await _auditLogService.GetUserAuditLogsAsync(userId);
            return Ok(auditLogs);
        }

        [HttpGet("entity/{entity}/{entityId}")]
        public async Task<IActionResult> GetEntityAuditLogs(string entity, int entityId)
        {
            var auditLogs = await _auditLogService.GetEntityAuditLogsAsync(entity, entityId);
            return Ok(auditLogs);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetAuditLogsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var auditLogs = await _auditLogService.GetAuditLogsByDateRangeAsync(startDate, endDate);
            return Ok(auditLogs);
        }
    }
}
