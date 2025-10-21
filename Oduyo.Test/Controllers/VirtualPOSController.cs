using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VirtualPOSController : ControllerBase
    {
        private readonly IVirtualPOSService _virtualPOSService;

        public VirtualPOSController(IVirtualPOSService virtualPOSService)
        {
            _virtualPOSService = virtualPOSService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVirtualPOSDto dto)
        {
            var virtualPOS = await _virtualPOSService.CreateVirtualPOSAsync(dto);
            return Ok(virtualPOS);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVirtualPOSDto dto)
        {
            var virtualPOS = await _virtualPOSService.UpdateVirtualPOSAsync(id, dto);
            if (virtualPOS == null)
                return NotFound();
            return Ok(virtualPOS);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _virtualPOSService.DeleteVirtualPOSAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var virtualPOS = await _virtualPOSService.GetVirtualPOSByIdAsync(id);
            if (virtualPOS == null)
                return NotFound();
            return Ok(virtualPOS);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var virtualPOSList = await _virtualPOSService.GetAllVirtualPOSAsync();
            return Ok(virtualPOSList);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var virtualPOSList = await _virtualPOSService.GetActiveVirtualPOSAsync();
            return Ok(virtualPOSList);
        }
    }
}
