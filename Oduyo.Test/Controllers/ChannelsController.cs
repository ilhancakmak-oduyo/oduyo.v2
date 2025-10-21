using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelService _channelService;

        public ChannelsController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChannelDto dto)
        {
            var channel = await _channelService.CreateChannelAsync(dto);
            return Ok(channel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChannelDto dto)
        {
            var channel = await _channelService.UpdateChannelAsync(id, dto);
            if (channel == null)
                return NotFound();
            return Ok(channel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _channelService.DeleteChannelAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var channel = await _channelService.GetChannelByIdAsync(id);
            if (channel == null)
                return NotFound();
            return Ok(channel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var channels = await _channelService.GetAllChannelsAsync();
            return Ok(channels);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var channels = await _channelService.GetActiveChannelsAsync();
            return Ok(channels);
        }
    }
}
