using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bimswarm.Models;


namespace Bimswarm.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        
        public ActionResult Index()
        {
            ViewBag.Title = "FileViewer";
            return View();
        }


        public ActionResult ErrorHandler()
        {

            return View(new ErrorViewModel("Unbekannter Fehler", "Es ist ein unbekannter Fehler aufgetreten.", ""));
        }
    }
}
