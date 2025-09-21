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
        /// <summary>
        /// Get all lectures
        /// </summary>
        /// <param name="model">Lecture</param>
        /// <returns></returns>

        // GET api/<LectureController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _lectureService.GetAll());
        }
        /// <summary>
        /// Get lecture by id
        /// </summary>
        /// <remarks>
        /// Enter lecture id
        /// </remarks>
        /// <param name="model">Lecture</param>
        /// <returns></returns>

        // GET api/<LectureController>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _lectureService.GetById(id));
        }

        /// <summary>
        /// Add new lecture
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "lecturename" : 1,
        ///        "descriiption" : "parallel  cuda programming",
        ///        "starttime" : 20:20:20,
        ///        "endtime" : 21:21:21,
        ///        "teacherid" : 1,
        ///        "roomid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Lecture</param>
        /// <returns></returns>

        // POST api/<LectureController>
        [HttpPost]
        public async Task<IActionResult> Add(lecture lecture)
        {
            await _lectureService.Create(lecture);
            return Ok();
        }
        /// <summary>
        /// Update lecture
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     Put /Todo
        ///     {
        ///        "lectureid" : 1
        ///        "lecturename" : 1,
        ///        "descriiption" : "parallel  cuda programming",
        ///        "starttime" : 20:20:20,
        ///        "endtime" : 21:21:21,
        ///        "teacherid" : 1,
        ///        "roomid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Lecture</param>
        /// <returns></returns>

        // PUT api/<LectureController>
        [HttpPut]
        public async Task<IActionResult> Update(lecture lecture)
        {
            await _lectureService.Update(lecture);
            return Ok();
        }
        /// <summary>
        /// Delete lecture by id
        /// </summary>
        /// <remarks>
        /// Enter lecture id
        /// </remarks>
        /// <param name="model">Lecture</param>
        /// <returns></returns>

        // DELETE api/<LectureController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _lectureService.Delete(id);
            return Ok();
        }
    }
}
