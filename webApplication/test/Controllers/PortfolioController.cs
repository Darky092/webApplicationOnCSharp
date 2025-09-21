using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private IportfolioService _portfolioService;
        public PortfolioController(IportfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }
        /// <summary>
        /// Get achevement by user id
        /// </summary>
        /// <param name="model">Portfolio</param>
        /// <returns></returns>

        // GET api/<PortfolioController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _portfolioService.GetAll());
        }
        /// <summary>
        /// Get achevement by user id
        /// </summary>
        /// <remarks>
        /// Enter user id
        /// </remarks>
        /// <param name="model">Portfolio</param>
        /// <returns></returns>

        // GET api/<PortfolioController>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _portfolioService.GetById(id));
        }
        /// <summary>
        /// Add new achevement to user
        /// </summary>
        /// <remarks>
        /// Query Example
        ///
        ///     POST /Todo
        ///     {
        ///        "userid" : 1,
        ///        "achevement" : "12123",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Portfolio</param>
        /// <returns></returns>

        // POST api/<PortfolioController>
        [HttpPost]
        public async Task<IActionResult> Add(portfolio portfolio)
        {
            await _portfolioService.Create(portfolio);
            return Ok();
        }
        /// <summary>
        /// Update achevement  
        /// </summary>
        /// <remarks>
        /// Query Example
        ///     PUT /Todo
        ///     {
        ///        "userid" : 1,
        ///        "achevement" : "12123",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Portfolio</param>
        /// <returns></returns>

        // PUT api/<PortfolioController>
        [HttpPut]
        public async Task<IActionResult> Update(portfolio portfolio)
        {
            await _portfolioService.Update(portfolio);
            return Ok();
        }
        /// <summary>
        /// Update achevement  
        /// </summary>
        /// <remarks>
        /// Enter user id
        /// </remarks>
        /// <param name="model">Portfolio</param>
        /// <returns></returns>

        // DELETE api/<PortfolioController>

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _portfolioService.Delete(id);
            return Ok();
        }
    }
}
