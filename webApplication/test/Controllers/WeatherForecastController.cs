using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> Summaries = new()
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<string> Get()
        {
            return Summaries;
        }
        [HttpPost]
        public IActionResult Add(string name) {
            Summaries.Add(name);
            return Ok();
        
        }
        [HttpPut]
        public IActionResult Update(int index, string name)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return BadRequest("Такой индекс неверный");
            }
            Summaries[index] = name;
            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete(int index)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return BadRequest("Такой индекс неверный");
            }
            Summaries.RemoveAt(index);
            return Ok();
        }
        [HttpGet("index")]
        public string GetOne(int index)
        {
            if (index < 0 || index >= Summaries.Count)
            {
                return "NaN";
            }
            return Summaries[index];
        }
        [HttpGet("find-by-name")]
        public int CountRows(string same) 
        {
            int count = 0;
            foreach (string x in Summaries) 
            {
                if (same == x) {
                    count++;
                }
            }
            return count;
        }
        [HttpGet("sort-strategy")]
        public IActionResult GetAll(int? sortStrategy) 
        {
            if (sortStrategy == null)
            {
                return Ok(Summaries);
            }
            else if (sortStrategy == 1)
            {
                Summaries.Sort();
                return Ok(Summaries);

            }
            else if (sortStrategy == -1)
            {
                Summaries.Sort();
                Summaries.Reverse();
                return Ok(Summaries);
            }
            else {
                return BadRequest("Некорректное значение параметра sortStrategy");
            }
        
        }

    }
}
