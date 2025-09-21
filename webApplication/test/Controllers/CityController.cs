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
        /// <summary>
        /// Get citys
        /// </summary>
        /// <param name="model">City</param>
        /// <returns></returns>

        // GET api/<CityController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _cityService.GetAll());
        }
        /// <summary>
        /// Get city by id
        /// </summary>
        /// <remarks>
        /// Enter city id
        /// </remarks>
        /// <param name="model">City</param>
        /// <returns></returns>

        // GET api/<CityController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _cityService.GetById(id));
        }
        /// <summary>
        /// Add new City
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "cityname" : "cool",
        ///        "postalcode" : "123",
        ///        "county" : "Moscow",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">City</param>
        /// <returns></returns>

        // POST api/<CityController>
        [HttpPost]
        public async Task<IActionResult> Add(city city)
        {
            await _cityService.Create(city);
            return Ok();
        }
        /// <summary>
        /// Update City
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "cityid" : 1,
        ///        "cityname" : "cool",
        ///        "postalcode" : "123",
        ///        "county" : "Moscow",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">City</param>
        /// <returns></returns>

        // PUT api/<CityController>
        [HttpPut]
        public async Task<IActionResult> Update(city city)
        {
            await _cityService.Update(city);
            return Ok();
        }
        /// <summary>
        /// Delete City by id
        /// </summary>
        /// <remarks>
        /// Enter city id
        /// </remarks>
        /// <param name="model">City</param>
        /// <returns></returns>

        // DELETE api/<CityController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _cityService.Delete(id);
            return Ok();
        }
    }
}
