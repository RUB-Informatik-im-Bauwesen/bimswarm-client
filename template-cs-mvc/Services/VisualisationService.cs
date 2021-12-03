using Newtonsoft.Json;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bimswarm.Models.VisualisationModels;

namespace Bimswarm.Services
{
    public class VisualisationService
    {
        public static HttpClient client = new HttpClient();
        public static async Task<VisSessionModel> RequestVisualisation()
        {
            var requestUri = "https://hubtest-gw3.instant3dhub.org/i3dhub-swarm/visservice/requestVisService";
            string username = "i3dhub-test";
            string password = "ToogBag)Blysow4";
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            var jsonResult = "";
            HttpResponseMessage responseContent = await client.GetAsync(requestUri);
            if (responseContent.IsSuccessStatusCode)
            {
                jsonResult = await responseContent.Content.ReadAsStringAsync();
            }
            else
            {
                jsonResult = "";
            }
            return JsonConvert.DeserializeObject<VisSessionModel>(jsonResult);
        }
        public static async Task<VisIdModel> AddModel(VisSessionModel session, VisAddFileModel file)
        {

            var requestUri = session.restEndpoint;
            string username = "i3dhub-test";
            string password = "ToogBag)Blysow4";
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            var jsonResult = "";
            var body = JsonConvert.SerializeObject(file);
            HttpResponseMessage responseContent = await client.PostAsync(requestUri + "/swarmAddModel", new StringContent(body, Encoding.UTF8, "application/json"));
            if (responseContent.IsSuccessStatusCode)
            {
                jsonResult = await responseContent.Content.ReadAsStringAsync();
            }
            else
            {
                jsonResult = "";
            }
            return JsonConvert.DeserializeObject<VisIdModel>(jsonResult);
        }

    }
}
