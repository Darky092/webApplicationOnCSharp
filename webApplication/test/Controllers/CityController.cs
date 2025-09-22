using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.city;

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
            var result = await _cityService.GetById(id);
            var response = result.Adapt<GetCityResponse>();
            return Ok(response);
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
        public async Task<IActionResult> Add(CreateCityRequest city)
        {
            var request = city.Adapt<city>();
            await _cityService.Create(request);
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
        public async Task<IActionResult> Update(UpdateCityRequest city)
        {
            var request = city.Adapt<city>();
            await _cityService.Update(request);
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
