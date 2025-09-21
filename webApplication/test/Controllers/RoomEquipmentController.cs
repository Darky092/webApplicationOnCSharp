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

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return Ok(await _roomEquipmentService.GetAll());
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _roomEquipmentService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(room_equipment room_equipment)
        {
            await _roomEquipmentService.Create(room_equipment);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(room_equipment room_equipment)
        {
            await _roomEquipmentService.Update(room_equipment);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _roomEquipmentService.Delete(id);
            return Ok();
        }
    }
}
