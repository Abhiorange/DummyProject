using Aramex.DataModels;
using Aramex.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aramex.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AramexContext _aramexContext;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,AramexContext aramexContext)
        {
            _logger = logger;
            _aramexContext = aramexContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
       /* [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Employee null here");
            }
            else
            {
               var user=_aramexContext.Users.FirstOrDefault(u=>u.Email.Equals(model.Email.ToLower()));
                if (user == null)
                {
                    return Content("user does not exist");
                }
                var userpassword = _aramexContext.Users.FirstOrDefault(u => u.Password.Equals(model.Password) && u.Email.Equals(model.Email.ToLower()));
                if (userpassword == null)
                {
                    return Content("password is not correct");
                }
                else
                {
                    var passwordsMatch = string.Compare(model.Password, userpassword.Password, StringComparison.Ordinal) == 0;
                    if (passwordsMatch == false)
                    {
                        return Content("password is not correct");
                    }
                }
              
                return Ok("User is Authenticated");
            }
        }*/
    }
}