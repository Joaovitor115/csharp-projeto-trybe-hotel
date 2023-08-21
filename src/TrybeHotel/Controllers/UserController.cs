using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;



namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]

        public IActionResult GetUsers()
        {
            var users = _repository.GetUsers();
            if (users == null)
            {
                return Unauthorized();
            }
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            var isUserRegistered = _repository.GetUserByEmail(user.Email!);
            if (isUserRegistered != null)
            {
                return Conflict(new { message = "User email already exists" });
            }

            return Created("user", _repository.Add(user)
);
        }
    }
}
