using Microsoft.Extensions.Configuration;
using System.IO;

namespace Bimswarm
{
    public static class ConfigurationManager
    {
        public static IConfiguration AppSettings { get; set; }
        static ConfigurationManager()
        {
            AppSettings = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}
