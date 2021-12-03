using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bimswarm.Models;
using Bimswarm.Models.ToolchainModels;
using Bimswarm.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace Bimswarm.Controllers
{
    [Authorize]
    public class ToolchainController : Controller
    {
        private readonly HttpClient client = new HttpClient();
        private readonly PlatformService server = new PlatformService();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Toolchain> toolchainInstances;
            try
            {
                toolchainInstances = await server.GetToolchainInstanceForUserAndProduct();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                return View("ErrorHandler", new ErrorViewModel("Verbindungsfehler", "Die ausgewählte Ansicht konnte auf Grund eines Fehlers bei der Verbindung zu BIMSWARM nicht geladen werden.", e.ToString()));
            }

            ViewBag.ToolchainsTemplates = toolchainInstances;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Instance(string id)
        {

            try
            {
                ViewBag.Types = await server.GetFormats();
                var instance = await server.GetToolchainInstance(id);
                return View(instance);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                return View("ErrorHandler",
                    new ErrorViewModel("Verbindungsfehler beim Laden",
                        "Die ausgewählte Vorlage konnte auf Grund eines Fehlers bei der Verbindung zu BIMSWARM nicht geladen werden.",
                        e.ToString()));
            }
        }


        [HttpPost]
        public async Task<ActionResult> UpdateStatus(string itemid, string instanceid, string status)
        {
            ItemStatus itemStatus = (ItemStatus)Enum.Parse(typeof(ItemStatus), status);

            HttpResponseMessage response = await server.PutItemStatus(long.Parse(itemid), itemStatus);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Instance", new { id = instanceid });
            }
            else
            {
                return View("ErrorHandler", new ErrorViewModel("Fehler " + response.StatusCode, "Der Status konnte auf Grund eines Fehlers nicht geändert werden. Fehlermeldung: " + response.ReasonPhrase, ""));
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAttachment(string contentid, string containerid, string instanceid)
        {
            var attach = await DataStorageService.GetContainerContentAttachment(containerid, contentid, instanceid);
            return attach;
        }

        [HttpPost]
        public async Task<ActionResult> PostAttachment(string contentid, string containerid, string file, string instanceid, string itemid)
        {
            var attach = await DataStorageService.PostContainerContentAttachment(containerid, contentid, file);
            if (attach.IsSuccessStatusCode)
            {
                var test = await server.PutItemStatus(long.Parse(itemid), ItemStatus.SUCCESS);
            }
            else
            {
                var test = await server.PutItemStatus(long.Parse(itemid), ItemStatus.FAILED);
            }
            return RedirectToAction("Instance", new { id = instanceid });
        }


        [HttpGet]
        [Route("/Toolchain/ToolchainViewer/{id}")]
        public async Task<ActionResult> ToolchainViewer([FromRoute] string id)
        {
            try
            {
                var toolchainTemplate = await server.GetToolchainInstance(id);
                if (toolchainTemplate == null)
                    return NotFound();
                return Json(toolchainTemplate);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                return BadRequest();
            }
        }
    }
}