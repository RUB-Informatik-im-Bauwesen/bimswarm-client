using Newtonsoft.Json;
using Bimswarm.Models.CdeModels;
using Bimswarm.Models.ToolchainModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bimswarm.Models.DataTransferObjects;
using Bimswarm.Identity;
using Microsoft.AspNetCore.Http;

namespace Bimswarm.Services
{
    public class PlatformService
    {
        internal readonly HttpClient client = new HttpClient();

        public PlatformService()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Current.User.FindFirst(ClaimTypeHandler.Token).Value);
        }

        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;


        #region Base_Methods
        public async Task<string> HttpGet(string httpQuery)
        {
            HttpResponseMessage responseContent = await client.GetAsync(RequestUri.BuildUri(BimswarmHosts.Platform,httpQuery));
            if (responseContent.IsSuccessStatusCode)
            {
                return await responseContent.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
        public async Task<HttpResponseMessage> HttpPut(string httpQuery, HttpContent content)
        {
            return await client.PutAsync(RequestUri.BuildUri(BimswarmHosts.Platform, httpQuery), content);
        }
        #endregion
                

        #region Toolchain-instances     

        public async Task<List<Toolchain>> GetToolchainInstanceForUserAndProduct()
        {

            string productid = ConfigurationManager.AppSettings["swarm:productID"];
            var stringResult = await HttpGet("toolchain/instance/product/" + productid + "/status/=pending");
            var resToolchains = JsonConvert.DeserializeObject<List<ToolchainDTO>>(stringResult);
            List<Toolchain> results = new List<Toolchain>();

            if (resToolchains != null)
            {
                foreach (var elem in resToolchains)
                {
                    var metadataDto = await GetToolchainMetadata(elem.toolchainMetadataId);
                    results.Add(new Toolchain(elem, metadataDto));
                }
            }
            return results;
        }

        public async Task<Toolchain> GetToolchainInstance(string instanceId)
        {
            string resultString = await HttpGet("toolchain/instance/" + instanceId);
            var resultTemplate = JsonConvert.DeserializeObject<ToolchainDTO>(resultString);
            var metadataDto = await GetToolchainMetadata(resultTemplate.toolchainMetadataId);
            return new Toolchain(resultTemplate, metadataDto);
        }


        #endregion

        #region Toolchain-items   
        public async Task<ItemStatus> GetItemStatus(long id)
        {
            string stringResult = await HttpGet("toolchainitem/" + id + "/status");
            return JsonConvert.DeserializeObject<ItemStatus>(stringResult); ;
        }

        public async Task<HttpResponseMessage> PutItemStatus(long id, ItemStatus status)
        {
            return await HttpPut("toolchainitem/" + id + "/status", new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json"));
        }
        #endregion        

        #region Formats

        public async Task<List<FormatType>> GetFormats()
        {

            var stringResult = await HttpGet("formattype");

            var resToolchains = JsonConvert.DeserializeObject<List<FormatType>>(stringResult);
            if (resToolchains == null)
            {
                resToolchains = new List<FormatType>();
            }
            return resToolchains;
        }

        public async Task<FormatType> GetFormat(string id)
        {
            var stringResult = await HttpGet("formattype/" + id);
            return JsonConvert.DeserializeObject<FormatType>(stringResult);
        }
        #endregion

        #region container

        public async Task<CdeContainer> GetContainer(string id)
        {
            var resultString = await HttpGet("ocdecontainer/" + id);
            var resultContainer = JsonConvert.DeserializeObject<CdeContainerDTO>(resultString);
            return new CdeContainer(resultContainer);
        }

        public async Task<CdeContent> GetCdeContent(string id)
        {
            var stringResult = await HttpGet("ocdecontent/" + id);
            var resultContent = JsonConvert.DeserializeObject<CdeContentDTO>(stringResult);
            return new CdeContent(resultContent);
        }
        #endregion

        #region Metadata

        public async Task<ToolchainMetadataDTO> GetToolchainMetadata(string id)
        {
            var stringResult = await HttpGet("metadata/" + id);
            return JsonConvert.DeserializeObject<ToolchainMetadataDTO>(stringResult);
        }
        #endregion

    }

}