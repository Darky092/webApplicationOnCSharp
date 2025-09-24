using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.room_equipment;

namespace webApplication.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class RoomEquipmentController : ControllerBase
        {

        public IRoomEquipmentService _roomEquipmentService;

        public RoomEquipmentController(IRoomEquipmentService roomEquipmentService)
            {
            _roomEquipmentService = roomEquipmentService;
            }
        /// <summary>
        /// Get rooms equipment
        /// </summary>
        /// <remarks>
        /// Enter room id
        /// </remarks>
        /// <param name="model">Equipment_Rooms</param>
        /// <returns></returns>
        // GET api/<EquipmentRoomsController>
        [HttpGet]
        public async Task<IActionResult> Get()
            {

            return Ok(await _roomEquipmentService.GetAll());
            }

        /// <summary>
        /// Get room equipment by room id
        /// </summary>
        /// <remarks>
        /// Enter room id
        /// </remarks>
        /// <param name="model">Equipment_Rooms</param>
        /// <returns></returns>
        // GET api/<EquipmentRoomsController>
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
            {
            var result = await _roomEquipmentService.GetById(id);
            var response = result.Adapt<GetRoomEquipmentResponse>();
            return Ok(response);
            }

        /// <summary>
        /// Add equipment to room
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "roomid" : 1,
        ///        "equipment" : 1,      
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Equipment_Rooms</param>
        /// <returns></returns>

        // POST api/<EquipmentRoomsController>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomEquipmentRequest room_equipment)
            {
            var request = room_equipment.Adapt<room_equipment>();
            await _roomEquipmentService.Create(request);
            return Ok();
            }
        /// <summary>
        /// Add equipment to room
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "roomid" : 1,
        ///        "equipment" : 1,      
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Equipment_Rooms</param>
        /// <returns></returns>
        // PUT api/<EquipmentRoomsController>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateRoomEquipmentRequest room_equipment)
            {
            var result = room_equipment.Adapt<room_equipment>();
            await _roomEquipmentService.Update(result);
            return Ok();
            }
        /// <summary>
        /// Add equipment to room
        /// </summary>
        /// <remarks>
        /// Enter room id
        /// </remarks>
        /// <param name="model">Equipment_Rooms</param>
        /// <returns></returns>
        // DELETE api/<EquipmentRoomsController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id,string equipment)
            {
            await _roomEquipmentService.Delete(id, equipment);
            return Ok();
            }
        }
    }