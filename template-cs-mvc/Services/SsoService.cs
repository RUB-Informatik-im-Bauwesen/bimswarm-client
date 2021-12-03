using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bimswarm.Services
{
    public static class SsoService
    {
        static HttpClient client = new HttpClient();

        public static async Task<string> HttpGet(string httpQuery)
        {
            string webroot = ConfigurationManager.AppSettings["swarm:ssoEndpoint"];
            HttpResponseMessage responseContent = await client.GetAsync(webroot + httpQuery);
            if (responseContent.IsSuccessStatusCode)
            {
                return await responseContent.Content.ReadAsStringAsync();
            }
            return "[]";
        }


        public static async Task<string> GetPublicKey()
        {
            client.DefaultRequestHeaders.Clear();
            var resultString = await HttpGet("sso/oauth/token_key");

            var jObject = Newtonsoft.Json.Linq.JObject.Parse(resultString);
            var key = (string)jObject["value"];
            var res = key.Split("\n");
            return res[1];
        }

        public static async Task<HttpResponseMessage> GetToken(string code)
        {
            var clientId = ConfigurationManager.AppSettings["swarm:clientID"];
            var encryptedclientSecret = ConfigurationManager.AppSettings["swarm:clientSecret"];
            var authenticationString = $"{clientId}:{encryptedclientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            var webroot = ConfigurationManager.AppSettings["swarm:ssoEndpoint"];
            var requestUrl = webroot + "sso/oauth/token?code=" + code + "&grant_type=authorization_code&scope=read";
            return await client.PostAsync(requestUrl, null);
        }
    }
}
