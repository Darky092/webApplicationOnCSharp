using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.students_group;

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

        public async Task<IActionResult> GetById(int groupid, int userid)
            {

            var result = await _studentsGroupService.GetById(groupid, userid);
            var response = result.Adapt<GetStudentsGroupsResponse>();
            return Ok(response);
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
        public async Task<IActionResult> Create(CreateStudentsGroupsRequest students_group)
            {
            var stuDbo = students_group.Adapt<students_group>();
            await _studentsGroupService.Create(stuDbo);
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
        public async Task<IActionResult> Delete(int groupid, int userid)
            {
            await _studentsGroupService.Delete(groupid, userid);
            return Ok();
            }
        }
    }