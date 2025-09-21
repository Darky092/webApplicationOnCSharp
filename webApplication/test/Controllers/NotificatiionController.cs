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
        /// <summary>
        /// Get all notifications 
        /// </summary>
        /// <param name="model">Notificarion</param>
        /// <returns></returns>

        // GET api/<NotificarionController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _notificationService.GetAll());
        }
        /// <summary>
        /// Get notification by notification id 
        /// </summary>
        /// <remarks>
        /// Enter notification id
        /// </remarks>
        /// <param name="model">Notificarion</param>
        /// <returns></returns>

        // GET api/<NotificarionController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _notificationService.GetById(id));
        }

        /// <summary>
        /// Add new notificarion to user
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "notificationid" : 1,
        ///        "userid" : 1,
        ///        "note" : "12123",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Notificarion</param>
        /// <returns></returns>

        // POST api/<NotificarionController>
        [HttpPost]
        public async Task<IActionResult> Add(notification notification)
        {
            await _notificationService.Create(notification);
            return Ok();
        }
        /// <summary>
        /// Update notification 
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     PUT /Todo
        ///     {
        ///        "notificationid" : 1,
        ///        "userid" : 1,
        ///        "note" : "12123",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Notificarion</param>
        /// <returns></returns>

        // PUT api/<NotificarionController>

        [HttpPut]
        public async Task<IActionResult> Update(notification notification)
        {
            await _notificationService.Update(notification);
            return Ok();
        }
        /// <summary>
        /// Delete notification by notification id 
        /// </summary>
        /// <remarks>
        /// Enter notification id
        /// </remarks>
        /// <param name="model">Notificarion</param>
        /// <returns></returns>

        // DELETE api/<NotificarionController>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.Delete(id);
            return Ok();
        }
    }
}
