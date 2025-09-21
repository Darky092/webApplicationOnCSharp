using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificatiionController : ControllerBase
    {
        private INotificationService _notificationService;
        public NotificatiionController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _notificationService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _notificationService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(notification notification)
        {
            await _notificationService.Create(notification);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(notification notification)
        {
            await _notificationService.Update(notification);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.Delete(id);
            return Ok();
        }
    }
}
