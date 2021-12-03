using Microsoft.AspNetCore.Http;
using System.IO;

namespace Bimswarm.Services
{
    public class LocalFileService
    {
        protected static readonly string _uploadFolder = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "App_Data", "uploads");
        protected static readonly string _downloadFolder = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), "wwwroot", "downloads");

        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;


        public static string GetUploadDirectory()
        {
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
            return _uploadFolder;
        }

        public static string GetDownloadDirectory()
        {
            if (!Directory.Exists(_downloadFolder))
            {
                Directory.CreateDirectory(_downloadFolder);
            }
            return _downloadFolder;
        }

        public static string GetUploadFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new IOException();
            return Path.Combine(GetUploadDirectory(), filename);
        }

        public static string GetDownloadFile(string filename)
        {
            return Path.Combine(GetDownloadDirectory(), filename);
        }

        public static string GetDownloadUri(string filename)
        {
            if (File.Exists(Path.Combine(GetDownloadDirectory(), filename)))
            {
                return Path.Combine(GetBaseUrl(), "downloads", filename);
            }
            return "";
        }

        public static string CopyToDownload(string filename)
        {
            var file = GetUploadFile(filename);
            var downloadFile = GetDownloadFile(filename);
            if (File.Exists(downloadFile))
            {
                File.Delete(downloadFile);
            }
            File.Copy(file, downloadFile);
            return downloadFile;
        }
        public static string GetBaseUrl()
        {
            var baseUrl = "http://" + Current.Request.Host.ToUriComponent();
            return baseUrl;
        }

    }
}
