using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureGroupsController : ControllerBase
    {
        private ILecturesGroupsService _lecturesGropesService;
        public LectureGroupsController(ILecturesGroupsService lecturesGroupsService)
        {
            _lecturesGropesService = lecturesGroupsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _lecturesGropesService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _lecturesGropesService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(lectures_group lectures_group)
        {
            await _lecturesGropesService.Create(lectures_group);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(lectures_group lectures_group)
        {
            await _lecturesGropesService.Update(lectures_group);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _lecturesGropesService.Delete(id);
            return Ok();
        }
    }
}
