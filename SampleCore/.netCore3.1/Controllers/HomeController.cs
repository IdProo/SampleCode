using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Helpers;
using WebApplication1.Models;
using Oid = WebApplication1.Models.Oid;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.Claims = User.Claims;
            string email = User.Claims.FirstOrDefault(c => c.Type.Equals(Oid.Email)).Value;
            string token = await HttpContext.GetTokenAsync("access_token");
            UserModel user = await UserHelper.User(email, token);

            List<ManageRolesModel> roles = await UserHelper.Roles(user.UserId, token);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            ICollection<string> myCookies = Request.Cookies.Keys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies.Delete(cookie);
            }
            await HttpContext.SignOutAsync();
            return SignOut("Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
