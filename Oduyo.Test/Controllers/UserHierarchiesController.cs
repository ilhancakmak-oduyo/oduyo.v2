using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserHierarchiesController : ControllerBase
    {
        private readonly IUserHierarchyService _userHierarchyService;

        public UserHierarchiesController(IUserHierarchyService userHierarchyService)
        {
            _userHierarchyService = userHierarchyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserHierarchyDto dto)
        {
            var hierarchy = await _userHierarchyService.CreateHierarchyAsync(dto.ParentUserId, dto.ChildUserId);
            return Ok(hierarchy);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userHierarchyService.DeleteHierarchyAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("children/{parentUserId}")]
        public async Task<IActionResult> GetChildUsers(int parentUserId)
        {
            var users = await _userHierarchyService.GetChildUsersAsync(parentUserId);
            return Ok(users);
        }

        [HttpGet("parent/{childUserId}")]
        public async Task<IActionResult> GetParentUser(int childUserId)
        {
            var user = await _userHierarchyService.GetParentUserAsync(childUserId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet("is-child/{parentUserId}/{childUserId}")]
        public async Task<IActionResult> IsUserChildOf(int parentUserId, int childUserId)
        {
            var isChild = await _userHierarchyService.IsUserChildOfAsync(parentUserId, childUserId);
            return Ok(new { IsChild = isChild });
        }
    }

    public class CreateUserHierarchyDto
    {
        public int ParentUserId { get; set; }
        public int ChildUserId { get; set; }
    }
}
