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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _attendanceService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _attendanceService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(attendance attendance)
        {
            await _attendanceService.Create(attendance);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(attendance attendance)
        {
            await _attendanceService.Update(attendance);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceService.Delete(id);
            return Ok();
        }
    }
}
