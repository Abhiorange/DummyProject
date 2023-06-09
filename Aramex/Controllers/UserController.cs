using Aramex.DataModels;
using Aramex.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Aramex.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AramexContext _aramexContext;
        public UserController(ILogger<UserController> logger, AramexContext aramexContext)
        {
            _logger = logger;
            _aramexContext = aramexContext;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Employee null here");
            }
            else
            {
                var user = _aramexContext.Users.FirstOrDefault(u => u.Email.Equals(model.Email.ToLower()));
                if (user == null)
                {
                    return Content("false");
                }
                var userpassword = _aramexContext.Users.FirstOrDefault(u => u.Password.Equals(model.Password) && u.Email.Equals(model.Email.ToLower()));
                if (userpassword == null)
                {
                    return Content("false");
                }
                else
                {
                    var passwordsMatch = string.Compare(model.Password, userpassword.Password, StringComparison.Ordinal) == 0;
                    if (passwordsMatch == false)
                    {
                        return Content("false");
                    }
                }

                return Ok(userpassword);
            }
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(model == null)
            {
                return BadRequest("Model null here");
            }
            var existingUser = _aramexContext.Users.FirstOrDefault(u => u.Email == model.Email);

            if (existingUser != null)
            {
                return Content("false");
            }
            else
            {
                var user = new User
                {
                    Email= model.Email,
                    UserName=model.UserName,
                    Password=model.Password,
                };
                _aramexContext.Add(user);
                _aramexContext.SaveChanges();
                return Ok("Employee Added SuccessFully");
            }
        }
    }
}
