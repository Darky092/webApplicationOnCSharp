using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(await _roomEquipmentService.GetById(id));
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
        public async Task<IActionResult> Create(room_equipment room_equipment)
        {
            await _roomEquipmentService.Create(room_equipment);
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
        public async Task<IActionResult> Update(room_equipment room_equipment)
        {
            await _roomEquipmentService.Update(room_equipment);
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
        public async Task<IActionResult> Delete(int id)
        {
            await _roomEquipmentService.Delete(id);
            return Ok();
        }
    }
}
