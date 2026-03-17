using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuanLyXemPhim.Models.Entities;

namespace QuanLyXemPhim.Controllers.Api
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersApiController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userManager.Users.ToList());
        }
    }
}