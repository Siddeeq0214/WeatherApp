using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest(new { error = "City name is required" });
            }

            try
            {
                var weather = await _weatherService.GetWeatherAsync(city);

                if (weather == null)
                {
                    return NotFound(new { error = $"Weather data not found for {city}" });
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weather for {City}", city);
                return StatusCode(500, new { error = "An error occurred while fetching weather data" });
            }
        }

        [HttpGet]
        public IActionResult GetWeatherInfo()
        {
            return Ok(new 
            { 
                message = "Weather API is running",
                usage = "GET /api/weather/{city} to get weather data for a city"
            });
        }
    }
}