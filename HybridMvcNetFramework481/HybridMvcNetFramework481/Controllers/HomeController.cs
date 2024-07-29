using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HybridMvcNetFramework481.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public async Task<ActionResult> Login()
        {
            var result = await HttpContext.GetOwinContext().Authentication.AuthenticateAsync("cookie");
            var accessToken = result.Properties.Dictionary[OpenIdConnectParameterNames.AccessToken];
            var idToken = result.Properties.Dictionary[OpenIdConnectParameterNames.IdToken];
            Debug.WriteLine(accessToken);
            Debug.WriteLine(idToken);
            ViewBag.Message = "Successfully login!";
            return View();
        }
    }
}