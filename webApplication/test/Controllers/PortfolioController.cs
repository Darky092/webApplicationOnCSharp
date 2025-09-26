using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Contracts.portfolio;

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
        /// <param name="id">Portfolio</param>
        /// <returns></returns>

        // GET api/<PortfolioController>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _portfolioService.GetById(id);
            var response = result.Adapt<GetPortfolioResponse>();
            return Ok(response);
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
        /// <param name="portfolio">Portfolio</param>
        /// <returns></returns>

        // POST api/<PortfolioController>
        [HttpPost]
        public async Task<IActionResult> Add(CreatePortfolioRequest portfolio)
        {
            var request = portfolio.Adapt<portfolio>();
            await _portfolioService.Create(request);
            return Ok();
        }

        /// <summary>
        /// Delete achevement  by id
        /// </summary>
        /// <remarks>
        /// Enter user id
        /// </remarks>
        /// <param name="achievement">Portfolio</param>
        /// <param name="id">Portfolio</param>
        /// <returns></returns>

        // DELETE api/<PortfolioController>

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, string achievement)
        {
            await _portfolioService.Delete(id, achievement);
            return Ok();
        }
    }
}