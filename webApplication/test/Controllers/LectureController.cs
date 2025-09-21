using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private ILectureService _lectureService;
        public LectureController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _lectureService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _lectureService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(lecture lecture)
        {
            await _lectureService.Create(lecture);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(lecture lecture)
        {
            await _lectureService.Update(lecture);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _lectureService.Delete(id);
            return Ok();
        }
    }
}
