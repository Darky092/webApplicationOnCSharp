using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        /// <summary>
        /// Get all attendances
        /// </summary>
        /// <remarks>
        /// Enter attendance id
        /// </remarks>
        /// <param name="model">attendance</param>
        /// <returns></returns>

        // GET api/<AttendanceController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _attendanceService.GetAll());
        }
        /// <summary>
        /// Get attendance by attendance id
        /// </summary>
        /// <remarks>
        /// Enter attendance id
        /// </remarks>
        /// <param name="model">attendance</param>
        /// <returns></returns>

        // GET api/<AttendanceController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _attendanceService.GetById(id));
        }
        /// <summary>
        /// Add attendance to user
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "lectureid" : 1,
        ///        "userid" : 1,
        ///        "ispresent" : true,
        ///        "note" : "text",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">attendance</param>
        /// <returns></returns>

        // POST api/<AttendanceController>
        [HttpPost]
        public async Task<IActionResult> Add(attendance attendance)
        {
            await _attendanceService.Create(attendance);
            return Ok();
        }
        /// <summary>
        /// Update attendance to user
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "attendanceid" : 1,
        ///        "lectureid" : 1,
        ///        "userid" : 1,
        ///        "ispresent" : true,
        ///        "note" : "text",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">attendance</param>
        /// <returns></returns>
 
        // PUT api/<AttendanceController>
        [HttpPut]
        public async Task<IActionResult> Update(attendance attendance)
        {
            await _attendanceService.Update(attendance);
            return Ok();
        }
        /// <summary>
        /// Delete attendance by attendance id
        /// </summary>
        /// <remarks>
        /// Enter attendance id
        /// </remarks>
        /// <param name="model">attendance</param>
        /// <returns></returns>

        // DELETE api/<AttendanceController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceService.Delete(id);
            return Ok();
        }
    }
}
