using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.lectures_groups;

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
        /// <summary>
        /// GET lectures and groups
        /// </summary>
        /// <param name="model">Lectures_group</param>
        /// <returns></returns>

        // GET api/<Lectures_groupController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
            {
            return Ok(await _lecturesGropesService.GetAll());
            }

        /// <summary>
        /// GET lectures by group id
        /// </summary>
        /// <remarks>
        /// Enter group id
        /// </remarks>
        /// <param name="model">Lectures_group</param>
        /// <returns></returns>

        // GET api/<Lectures_groupController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            {
            var result = await _lecturesGropesService.GetById(id);
            var response = result.Adapt<lectures_group>();
            return Ok(response);
            }

        /// <summary>
        /// Add new lecture to group
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
        /// <param name="model">Lectures_group</param>
        /// <returns></returns>

        // POST api/<Lectures_groupController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateLecturesGroupsRequest lectures_group)
            {
            var request = lectures_group.Adapt<lectures_group>();
            await _lecturesGropesService.Create(request);
            return Ok();
            }
        /// <summary>
        /// Update lecture or group
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
        /// <param name="model">Lectures_group</param>
        /// <returns></returns>

        // PUT api/<Lectures_groupController>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateLectureGroupRequest lectures_group)
            {
            var request = lectures_group.Adapt<lectures_group>();
            await _lecturesGropesService.Update(request);
            return Ok();
            }
        /// <summary>
        /// Delete lecture by group id
        /// </summary>
        /// <remarks>
        /// enter group id
        /// </remarks>
        /// <param name="model">Lectures_group</param>
        /// <returns></returns>

        // DELETE api/<Lectures_groupController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            {
            await _lecturesGropesService.Delete(id);
            return Ok();
            }
        }
    }