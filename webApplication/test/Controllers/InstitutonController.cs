using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.institution;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutonController : ControllerBase
    {

        private IInstitutionService _institutionService;
        public InstitutonController(IInstitutionService institutionService)
        {
            _institutionService = institutionService;
        }
        /// <summary>
        /// Get institutions
        /// </summary>
        /// <param name="model">Institution</param>
        /// <returns></returns>

        // GET api/<InstitutionController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _institutionService.GetAll());
        }
        /// <summary>
        /// Get institution by id
        /// </summary>
        /// <remarks>
        /// Enter institution id
        /// </remarks>
        /// <param name="model">Institution</param>
        /// <returns></returns>

        // GET api/<InstitutionController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _institutionService.GetById(id);
            var response = result.Adapt<GetInstitutionResponse>();
            return Ok(response);
        }
        /// <summary>
        /// Add new institution
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "institutionname" : "cool",
        ///        "street" : "streer",
        ///        "phone" : "+79032432030",
        ///        "website" : "linc",
        ///        "cityid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Institution</param>
        /// <returns></returns>

        // POST api/<InstitutionController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateInstitutionRequest institution)
        {
            var request = institution.Adapt<institution>();
            await _institutionService.Create(request);
            return Ok();
        }
        /// <summary>
        /// Update institution
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "institutionid" : 1,
        ///        "institutionname" : "cool",
        ///        "street" : "streer",
        ///        "phone" : "+79032432030",
        ///        "website" : "linc",
        ///        "cityid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Institution</param>
        /// <returns></returns>

        // PUT api/<InstitutionController>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateInstitutionRequest institution)
        {
            var request = institution.Adapt<institution>();
            await _institutionService.Update(request);
            return Ok();
        }
        /// <summary>
        /// Delete institution
        /// </summary>
        /// <remarks>
        /// Enter institution id
        /// </remarks>
        /// <param name="model">Institution</param>
        /// <returns></returns>

        // Delete api/<InstitutionController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _institutionService.Delete(id);
            return Ok();
        }
    }
}