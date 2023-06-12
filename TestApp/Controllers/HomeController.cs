using AramexApp.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using TestApp.Models;
using TestApp.Models.ViewModels;
using static AramexApp.Serilaize.SerializeHelper;

namespace TestApp.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        Uri baseAddress = new Uri("https://localhost:7148/api");
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(string ReturnUrl)
        {
            ViewData["returnurl"] = ReturnUrl;
            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string? ReturnUrl)
       {
            if (ModelState.IsValid)
            {
                try
                {
                    StringContent content_model = SerializationHelper.SerializeModel(model);
                    HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/User/Login", content_model).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        UserModel user = new UserModel();
                        var content = response.Content.ReadAsStringAsync().Result;
                     
                        if (content == "false")
                        {
                            ModelState.AddModelError("Email","Invalid Credentials");
                            ModelState.AddModelError("Password", "Invalid Credentials");
                            return View("Index");
                        }
                        else
                        {
                            user = JsonConvert.DeserializeObject<UserModel>(content);
                            var claims = new List<Claim>
                                    {
                                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                                    };
                            var claimsIdentity = new ClaimsIdentity(claims, "Login");

                           await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                           /* string returnUrl = HttpContext.Request.Query["ReturnUrl"];*/
                            if (ReturnUrl == null)
                            {
                                TempData["success"] = "Logged in done succesfully";
                                return RedirectToAction("Submit");
                            }
                            else
                            {
                                TempData["success"] = "Logged in done succesfully";
                                return Redirect(ReturnUrl);
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ViewData["ErrorMessage"] = "Error: " + ex.Message;
                    return View("Index");
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = "Error: " + ex.Message;
                    return View("Index");
                }
            }
            return View("Index");
        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Submit()
        {
            return View();
        }
       
        [HttpGet]
        [Authorize]
        public IActionResult Trackview()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    StringContent content_model = SerializationHelper.SerializeModel(model);
                    HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/User/Register", content_model).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;

                        if (content == "false")
                        {
                            ViewData["ErrorMessage"] = "Email Already exist";
                            return View("Register");
                        }
                        else
                        {
                            TempData["success"] = "registartion is done succesfully";
                            return RedirectToAction("Index");
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = "Error: " + ex.Message;
                    return View("Register");
                }
            }
                return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}