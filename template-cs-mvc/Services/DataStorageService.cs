using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Bimswarm.Models.CdeModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bimswarm.Models.MiscModels;
using Bimswarm.Identity;
using Microsoft.AspNetCore.Http;

namespace Bimswarm.Services
{
    public class DataStorageService
    {
        public static HttpClient client = new HttpClient();

        public static HttpClient HttpClient()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Current.User.FindFirst(ClaimTypeHandler.Token).Value);
            return client;
        }

        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;

        #region base_methods

        public static async Task<HttpResponseMessage> HttpGetWithResponseMessage(string httpQuery)
        {
            return await HttpClient().GetAsync(httpQuery);
        }
        public static async Task<HttpResponseMessage> HttpPostWithResponse(string httpQuery, HttpContent content)
        {
            return await HttpClient().PostAsync(httpQuery, content);
        }

        #endregion




        public static async Task<ActionResult> GetContainerContentAttachment(string externalContainerId, string internalContentId, string toolchainID)
        {
            PlatformService server = new PlatformService();
            var content = await server.GetCdeContent(internalContentId);
            var toolchain = await server.GetToolchainInstance(toolchainID);
            var externalContentId = content.externalId;

            var responseMessage = await HttpGetWithResponseMessage(RequestUri.BuildUri(BimswarmHosts.CDE, $"/containers/{externalContainerId}/contents/{externalContentId}/attachment").AbsoluteUri);
            if (responseMessage.IsSuccessStatusCode)
            {
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync();
                var name = responseMessage.Headers.GetValues("filename").First();

                string guid = Guid.NewGuid().ToString();
                var datestring = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString() + " Uhr";
                var metadata = new FileMetadataDto(guid, ConfigurationManager.AppSettings["swarm:productid"], Current.User.FindFirst(ClaimTypeHandler.EMail).Value,
                    name, toolchain , new FormatType("", "", "", "", ""), datestring);

                var filename = guid + "_" + name;
                metadata.filename = name;

                var metaPath = LocalFileService.GetUploadFile(guid + ".json");
                var filePath = LocalFileService.GetUploadFile(filename);
                try
                {
                    File.WriteAllBytes(filePath, bytes);
                    using (var tw = new StreamWriter(metaPath, true))
                    {
                        tw.WriteLine(JsonConvert.SerializeObject(metadata).ToString());
                        tw.Close();
                    }
                }
                catch
                {
                    //Loggin implementieren
                    return new RedirectToRouteResult("/Home/ErrorHandler");
                }

                return new RedirectToActionResult("ViewFile", "Files", new { id = guid });
            }
            return new RedirectToRouteResult("/Home/ErrorHandler");
        }

        [HttpPost]
        public static async Task<HttpResponseMessage> PostContainerContentAttachment(string externalContainerId, string internalContentId, string filePath)
        {
            PlatformService server = new PlatformService();
            var content = await server.GetCdeContent(internalContentId);
            var externalContentId = content.externalId;

            string webaddress = RequestUri.BuildUri(BimswarmHosts.CDE, $"/containers/{externalContainerId}/contents/{externalContentId}/attachment").AbsoluteUri;
            var fullFilePath = Path.Combine(LocalFileService.GetUploadDirectory(), filePath);
            if (File.Exists(fullFilePath))
            {
                HttpContent fileStreamContent = new StreamContent(new FileStream(fullFilePath, FileMode.Open));
                using (var formData = new MultipartFormDataContent())
                {

                    formData.Add(fileStreamContent, "file", Path.GetFileName(fullFilePath));
                    var response = await HttpPostWithResponse(webaddress, formData);
                    if (!response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                    return response;
                }
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}