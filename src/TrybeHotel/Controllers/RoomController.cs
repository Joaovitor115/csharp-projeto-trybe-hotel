using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{HotelId}")]
        public IActionResult GetRoom(int HotelId)
        {
            var rooms = _repository.GetRooms(HotelId);
            return Ok(rooms);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public IActionResult PostRoom([FromBody] Room room)
        {
            var rooms = _repository.AddRoom(room);
            return Created("", rooms);
        }

        [HttpDelete("{RoomId}")]
        [Authorize(Policy = "admin")]
        public IActionResult Delete(int RoomId)
        {
            _repository.DeleteRoom(RoomId);
            return NoContent();
        }
    }
}