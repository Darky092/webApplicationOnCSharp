using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.lecture;
using webApplication.Contracts.students_group;
using webApplication.Authorization;

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
        /// <param name="model">StudentsGroup</param>
        /// <returns></returns>

        // GET api/<StudentsGroupController>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(await _studentsGroupService.GetAll());
        }
        /// <summary>
        /// Get student and group by lecture id
        /// </summary>
        /// <remarks>
        /// Enter lectur id
        /// </remarks>
        /// <param name="userid">StudentsGroup</param>
        /// <returns></returns>

        // GET api/<StudentsGroupController>
        [Authorize]
        [HttpGet("{userid}")]

        public async Task<IActionResult> GetById(int userid)
        {

            var result = await _studentsGroupService.GetById(userid);
            var response = result.Adapt<List<GetStudentsGroupsResponse>>();
            return Ok(response);
        }

        /// <summary>
        /// Get students by lecture id
        /// </summary>
        /// <remarks>
        /// Enter lectur id
        /// </remarks>
        /// <param name="lectureId">StudentsGroup</param>
        /// <returns></returns>

        // GET api/<StudentsGroupController>
        [Authorize]

        [HttpGet("lecture/{lectureId}/students")]
        public async Task<IActionResult> GetStudentsByLectureId(int lectureId)
        {
            var students = await _studentsGroupService.GetStudentsByLectureId(lectureId);

            
            var dtos = students.Select(s => new Client.Components.bodies.User
            {
                userid = s.userid,
                name = s.name,
                surname = s.surname,
                patronymic = s.patronymic,
                email = s.email,
                telephonnumber = s.telephonnumber,
                role = s.role,
                isactive = s.isactive,
                createdat = s.createdat.HasValue ?
                    new DateTimeOffset(s.createdat.Value, TimeSpan.Zero) :
                    (DateTimeOffset?)null
            }).ToList();

            return Ok(dtos);
        }


        /// <summary>
        /// Get lecture and group by lecture id
        /// </summary>
        /// <remarks>
        /// Enter lectur id
        /// </remarks>
        /// <param name="userid">StudentsGroup</param>
        /// <returns></returns>

        // GET api/<StudentsGroupController>
        [Authorize]
        [HttpGet("{userid}/lectures")]
        public async Task<IActionResult> GetLectures(int userid)
        {

            var result = await _studentsGroupService.GetLecturesByUserId(userid);
            //var response = result.Adapt<GetLectureResponse>();
            return Ok(result);
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
        /// <param name="students_group">StudentsGroup</param>
        /// <returns></returns>

        // POST api/<StudentsGroupController>
        [AllowAnonymous]
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
        /// <param name="model">StudentsGroup</param>
        /// <returns></returns>

        // DELETE api/<StudentsGroupController>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int groupid, int userid)
        {
            await _studentsGroupService.Delete(groupid, userid);
            return Ok();
        }
    }
}