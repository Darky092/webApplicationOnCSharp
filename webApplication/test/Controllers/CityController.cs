using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {


        private ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _cityService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _cityService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(city city)
        {
            await _cityService.Create(city);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(city city)
        {
            await _cityService.Update(city);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _cityService.Delete(id);
            return Ok();
        }
    }
}
