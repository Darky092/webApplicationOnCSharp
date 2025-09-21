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
        /// <summary>
        /// Get lecture and group
        /// </summary>
        /// <param name="model">LectureGroup</param>
        /// <returns></returns>

        // GET api/<StudentsGroupController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(await _studentsGroupService.GetAll());
        }
        /// <summary>
        /// Get lecture and group by lecture id
        /// </summary>
        /// <remarks>
        /// Enter lectur id
        /// </remarks>
        /// <param name="model">LectureGroup</param>
        /// <returns></returns>

        // GET api/<StudentsGroupController>
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _studentsGroupService.GetById(id));
        }
        /// <summary>
        /// Add lecture to group
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "groupid" : 1,
        ///        "lectureid" : 1,      
        ///     }
        ///
        /// </remarks>
        /// <param name="model">LectureGroup</param>
        /// <returns></returns>

        // POST api/<StudentsGroupController>
        [HttpPost]
        public async Task<IActionResult> Create(students_group students_group)
        {
            await _studentsGroupService.Create(students_group);
            return Ok();
        }
        /// <summary>
        /// Update lecture to group
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "groupid" : 1,
        ///        "lectureid" : 1,      
        ///     }
        ///
        /// </remarks>
        /// <param name="model">LectureGroup</param>
        /// <returns></returns>

        // PUT api/<StudentsGroupController>
        [HttpPut]
        public async Task<IActionResult> Update(students_group students_group)
        {
            await _studentsGroupService.Update(students_group);
            return Ok();
        }
        /// <summary>
        /// Add lecture to group
        /// </summary>
        /// <remarks>
        /// Enter lectur id
        /// </remarks>
        /// <param name="model">LectureGroup</param>
        /// <returns></returns>

        // DELETE api/<StudentsGroupController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentsGroupService.Delete(id);
            return Ok();
        }
    }
}
