using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsGroupController : ControllerBase
    {
        public IStudentsGroupService _studentsGroupService;

        public StudentsGroupController(IStudentsGroupService studentsGroupService)
        {
            _studentsGroupService = studentsGroupService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(await _studentsGroupService.GetAll());
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _studentsGroupService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(students_group students_group)
        {
            await _studentsGroupService.Create(students_group);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(students_group students_group)
        {
            await _studentsGroupService.Update(students_group);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentsGroupService.Delete(id);
            return Ok();
        }
    }
}
