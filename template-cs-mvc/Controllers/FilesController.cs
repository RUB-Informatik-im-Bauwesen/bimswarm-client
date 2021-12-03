using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Bimswarm.Identity;
using Bimswarm.Models.CdeModels;
using Bimswarm.Models.MiscModels;
using Bimswarm.Models.ToolchainModels;
using Bimswarm.Models.VisualisationModels;
using Bimswarm.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bimswarm.Models;

namespace Bimswarm.Controllers
{

    [Authorize]
    public class FilesController : Controller
    {
        private readonly HttpClient client = new HttpClient();
        private readonly PlatformService server = new PlatformService();


        [HttpGet]
        public async Task<ActionResult> Index()
        {

            try
            {
                var types = await server.GetFormats();

                if (types != null)
                {
                    ViewBag.Types = types;
                }
                else
                {
                    ViewBag.Types = new List<FormatType>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                return View("ErrorHandler", new ErrorViewModel("Verbindungsfehler", "Die ausgewählte Ansicht konnte auf Grund eines Fehlers bei der Verbindung zu BIMSWARM nicht geladen werden.", e.ToString()));
            }

            List<FileMetadataDto> list = new List<FileMetadataDto>();
            DirectoryInfo d = new DirectoryInfo(LocalFileService.GetUploadDirectory());
            foreach (var file in d.GetFiles("*.json"))
            {
                list.Add(JsonConvert.DeserializeObject<FileMetadataDto>(System.IO.File.ReadAllText(file.FullName)));
            }

            ViewBag.Files = list.Where(m => m.user == User.FindFirst(ClaimTypeHandler.EMail).Value);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(List<IFormFile> files, string filetype)
        {
            var format = await server.HttpGet("/formattype/" + filetype);
            var type = JsonConvert.DeserializeObject<FormatType>(format);
            var formFile = files.First();
            if (formFile.Length > 0)
            {
                string guid = Guid.NewGuid().ToString();
                var datestring = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString() + " Uhr";
                var metadata =
                    new FileMetadataDto(
                        guid,
                        ConfigurationManager.AppSettings["swarm:productid"],
                        User.FindFirst(ClaimTypeHandler.EMail).Value,
                        formFile.FileName,
                        new Toolchain(),
                        type,
                        datestring);


                var metaPath = LocalFileService.GetUploadFile(guid + ".json");
                var filePath = LocalFileService.GetUploadFile(metadata.GetFileName());



                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    using (var tw = new StreamWriter(metaPath, true))
                    {
                        tw.WriteLine(JsonConvert.SerializeObject(metadata).ToString());
                        tw.Close();
                    }

                    List<FileMetadataDto> list = new List<FileMetadataDto>();
                    DirectoryInfo d = new DirectoryInfo(LocalFileService.GetUploadDirectory());
                    foreach (var file in d.GetFiles("*.json"))
                    {
                        list.Add(JsonConvert.DeserializeObject<FileMetadataDto>(System.IO.File.ReadAllText(file.FullName)));
                    }
                    ViewBag.Files = list;

                }
                catch
                {
                    return RedirectToPage("/Home/ErrorHandler");
                }
            }

            return Redirect("/Files/Index");
        }

        [HttpGet]
        public ActionResult ViewFile(string id)
        {

            string filename = id + ".json";
            string file = LocalFileService.GetUploadFile(filename);
            if (System.IO.File.Exists(file))
            {
                ViewBag.Message = "Die Datei wurde gefunden!";
                var res = JsonConvert.DeserializeObject<FileMetadataDto>(System.IO.File.ReadAllText(file));
                ViewBag.Name = res.filename.Split('_').Last();
                ViewBag.Title = "Datei " + ViewBag.Name;
                ViewBag.File = res;
            }
            else
            {
                ViewBag.Message = "Die Datei wurde nicht gefunden.";
                ViewBag.Title = "Datei nicht gefunden.";
            }


            return View();
        }

        [HttpPost]
        public ActionResult ViewFile(string guid, string filename)
        {
            string metaFile = LocalFileService.GetUploadFile(guid + ".json");
            string file = LocalFileService.GetUploadFile(filename); ;
            if (System.IO.File.Exists(metaFile) && System.IO.File.Exists(file))
            {
                var res = JsonConvert.DeserializeObject<FileMetadataDto>(System.IO.File.ReadAllText(metaFile));
                return PhysicalFile(file, "application/ifc", res.filename);
            }
            else
            {
                return Redirect("/Files/Index");
            }
        }

        [HttpPost]
        public ActionResult DeleteFile(string guid, string filename)
        {
            var metaPath = LocalFileService.GetUploadFile(guid + ".json");
            var filePath = LocalFileService.GetUploadFile(filename);

            System.IO.File.Delete(metaPath);
            System.IO.File.Delete(filePath);

            return Redirect("/Files/Index");
        }


        [HttpPost]
        public async Task<ActionResult> VisualizeFile(string guid, string filename)
        {


            try
            {
                var visSession = await VisualisationService.RequestVisualisation();
                var endpoint = visSession.restEndpoint;
                VisAddFileModel model = new VisAddFileModel
                {
                    modelURI = ""
                };
                var viewerAddress = visSession.viewerAddress;

                string metaFile = LocalFileService.GetUploadFile(guid + ".json");
                string file = LocalFileService.GetUploadFile(filename); ;
                if (System.IO.File.Exists(metaFile) && System.IO.File.Exists(file))
                {
                    var res = JsonConvert.DeserializeObject<FileMetadataDto>(System.IO.File.ReadAllText(metaFile));
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/uploads/";
                    //model.modelURI = Path.Combine(path, res.filename);
                    var downloadFile = LocalFileService.CopyToDownload(filename);
                    var Uri = LocalFileService.GetDownloadUri(filename);
                    model.modelURI = Uri;
                }
                Thread.Sleep(5000);
                var modelId = await VisualisationService.AddModel(visSession, model);

                return Redirect(viewerAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
                return View("ErrorHandler", new ErrorViewModel("Verbindungsfehler", "Verbindung zum Visualisierungsservice fehlgeschlagen.", e.ToString()));
            }
        }
    }
}
