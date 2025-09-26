using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.group;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        /// <summary>
        /// Get Groups
        /// </summary>
        /// <param name="model">Group</param>
        /// <returns></returns>

        // GET api/<GroupController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _groupService.GetAll());
        }

        /// <summary>
        /// Get Group by id
        /// </summary>
        /// <remarks>
        /// Enter group id
        /// </remarks>
        /// <param name="model">Group</param>
        /// <returns></returns>

        // GET api/<GroupController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _groupService.GetById(id);
            var response = result.Adapt<group>();
            return Ok(response);
        }
        /// <summary>
        /// Add new Group
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "groupname" : "cool",
        ///        "course" : 1,
        ///        "curatorid" : 1,
        ///        "specialty" : "proggramist",
        ///        "institutionid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Group</param>
        /// <returns></returns>

        // POST api/<GroupController>
        [HttpPost]
        public async Task<IActionResult> Add(CreateGroupRequest group)
        {
            var request = group.Adapt<group>();
            await _groupService.Create(request);
            return Ok();
        }
        /// <summary>
        /// Update Group
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "groupid" : 1,
        ///        "groupname" : "cool",
        ///        "course" : 1,
        ///        "curatorid" : 1,
        ///        "specialty" : "proggramist",
        ///        "institutionid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Group</param>
        /// <returns></returns>

        // PUT api/<GroupController>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateGroupRequest group)
        {
            var request = group.Adapt<group>();
            await _groupService.Update(request);
            return Ok();
        }
        /// <summary>
        /// Delete Group by id
        /// </summary>
        /// <remarks>
        /// Enter group id
        /// </remarks>
        /// <param name="model">Group</param>
        /// <returns></returns>

        // DELETE api/<GroupController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _groupService.Delete(id);
            return Ok();
        }
    }
}