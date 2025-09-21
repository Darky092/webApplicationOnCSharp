using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _institutionService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _institutionService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(institution institution)
        {
            await _institutionService.Create(institution);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(institution institution)
        {
            await _institutionService.Update(institution);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _institutionService.Delete(id);
            return Ok();
        }
    }
}
