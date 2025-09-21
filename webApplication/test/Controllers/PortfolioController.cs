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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _portfolioService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _portfolioService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(portfolio portfolio)
        {
            await _portfolioService.Create(portfolio);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(portfolio portfolio)
        {
            await _portfolioService.Update(portfolio);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _portfolioService.Delete(id);
            return Ok();
        }
    }
}
