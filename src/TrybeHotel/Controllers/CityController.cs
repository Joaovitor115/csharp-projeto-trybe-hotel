using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("city")]
    public class CityController : Controller
    {
        private readonly ICityRepository _repository;
        public CityController(ICityRepository repository)
        {
            _repository = repository;
        }

        // 2. Desenvolva o endpoint GET /city
        [HttpGet]
        public IActionResult GetCities()
        {

            var cities = _repository.GetCities();

            return Ok(cities);
        }

        // 3. Desenvolva o endpoint POST /city
        [HttpPost]
        public IActionResult PostCity([FromBody] City city)
        {

            _repository.AddCity(city);

            return Created("", city);

        }
        [HttpPut]
        public IActionResult PutCity([FromBody] City city)
        {

            _repository.UpdateCity(city);

            return Ok(city);
        }
    }
}