using System;

namespace Bimswarm.Services
{
    public static class RequestUri
    {
        public static Uri BuildUri(BimswarmHosts host, string route)
        {
            var webroot = GetHost(host);
            webroot = webroot.TrimEnd('/');
            route = route.TrimStart('/');
            return new Uri(string.Format("{0}/{1}", webroot, route));
        }
        public static Uri BuildUri(string host, string route)
        {
            host = host.TrimEnd('/');
            route = route.TrimStart('/');
            return new Uri(string.Format("{0}/{1}", host, route));
        }

        private static string GetHost(BimswarmHosts host)
        {
            return host switch
            {
                BimswarmHosts.Platform => ConfigurationManager.AppSettings["swarm:endpoint"],
                BimswarmHosts.SSO => ConfigurationManager.AppSettings["swarm:ssoEndpoint"],
                BimswarmHosts.CDE => ConfigurationManager.AppSettings["swarm:cdeEndpoint"],
                _ => string.Empty,
            };
        }
    }

    public enum BimswarmHosts
    {
        Platform,
        SSO,
        CDE
    }
}
