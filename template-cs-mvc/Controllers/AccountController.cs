using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Bimswarm.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        [HttpGet]
        public ActionResult Login(string redirectUrl)
        {
            ViewBag.RedirectUrl = redirectUrl;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Home/Index");
        }


    }
}
