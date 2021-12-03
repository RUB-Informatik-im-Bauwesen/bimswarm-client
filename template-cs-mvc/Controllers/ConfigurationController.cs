using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bimswarm.Identity;

namespace Bimswarm.Controllers
{
    [Authorize]
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var webroot = ConfigurationManager.AppSettings["swarm:endpoint"];
            var productid = ConfigurationManager.AppSettings["swarm:productID"];

            ViewBag.userid = User.FindFirst(ClaimTypeHandler.ID);
            ViewBag.mail = User.FindFirst(ClaimTypeHandler.EMail);
            ViewBag.webroot = webroot;
            ViewBag.productid = productid;
            ViewBag.token = User.FindFirst(ClaimTypeHandler.Token);

            return View();
        }

        [HttpPost]
        public ActionResult Index(string webEndpoint, string userid, string productid)
        {
            ConfigurationManager.AppSettings["swarm:endpoint"] = webEndpoint;
            ConfigurationManager.AppSettings["swarm:productID"] = productid;

            ViewBag.userid = User.FindFirst(ClaimTypeHandler.ID);
            ViewBag.mail = User.FindFirst(ClaimTypeHandler.EMail);
            ViewBag.token = User.FindFirst(ClaimTypeHandler.Token);
            ViewBag.webroot = webEndpoint;
            ViewBag.productid = productid;
            
            ViewBag.Message = "Änderungen übernommen!";

            return View();
        }


    }
}
