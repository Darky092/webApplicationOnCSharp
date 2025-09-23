using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.room;
using webApplication.Contracts.room_equipment;

namespace webApplication.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
        {


        public IRoomService _roomService;

        public RoomController(IRoomService roomService)
            {
            _roomService = roomService;
            }
        /// <summary>
        /// Get rooms
        /// </summary>
        /// <param name="model">Rooms</param>
        /// <returns></returns>

        // GET api/<RoomController>
        [HttpGet]
        public async Task<IActionResult> Get()
            {

            return Ok(await _roomService.GetAll());
            }
        /// <summary>
        /// Get room by id
        /// </summary>
        /// <remarks>
        /// Enter room id
        /// </remarks>
        /// <param name="model">Rooms</param>
        /// <returns></returns>

        // GET api/<RoomController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            {
            var result = await _roomService.GetById(id);
            var response = result.Adapt<GetRoomResponse>();
            return Ok(response);
            }
        /// <summary>
        /// Add new room
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "roomid" : 1,
        ///        "roomnumber" : "12123",
        ///        "institutionid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Rooms</param>
        /// <returns></returns>

        // POST api/<RoomController>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomRequest room)
            {
            var request = room.Adapt<room>();
            await _roomService.Create(request);
            return Ok();
            }
        /// <summary>
        /// Add new room
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "roomid" : 1,
        ///        "roomnumber" : "12123",
        ///        "institutionid" : 1,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Rooms</param>
        /// <returns></returns>

        // PUT api/<RoomController>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoomRequest room)
            {
            var request = room.Adapt<room>();
            await _roomService.Update(request);
            return Ok();
            }
        /// <summary>
        /// Delete room by id
        /// </summary>
        /// <remarks>
        /// Enter room id
        /// </remarks>
        /// <param name="model">Rooms</param>
        /// <returns></returns>

        // DELETE api/<RoomController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            {
            await _roomService.Delete(id);
            return Ok();
            }

        }
    }