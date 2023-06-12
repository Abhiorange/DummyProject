using DataModels.DataModel;
using DataModels.ViewModels;
using DataRepository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Aramex.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly AramexContext _aramexContext;
        private readonly IUser _userrepo;
        public UserController(ILogger<UserController> logger, AramexContext aramexContext,IUser userrepo)
        {
            _logger = logger;
            _aramexContext = aramexContext;
            _userrepo = userrepo;
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
                var user = _userrepo.Login(model);
                if(user == null)
                {
                    return Content("false");
                }
                else
                {
                    return Ok(user);
                }
                
            }
            
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(model == null)
            {
                return BadRequest("Model null here");
            }
            var response = _userrepo.Register(model);
            if(response==false)
            {
                return Content("false");
            }
            else
            {
                return Ok("Employee Added SuccessFully");
            }
         
        }
    }
}
