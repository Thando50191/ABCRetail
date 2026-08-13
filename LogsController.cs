using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ABCRetailAzure.Services;

namespace ABCRetail.Controllers
{
    public class LogsController : Controller
    {
        private readonly AzureStorageService _storageService;

        public LogsController(AzureStorageService storageService)
        {
            _storageService = storageService;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(string logMessage)
        {
            if (string.IsNullOrWhiteSpace(logMessage))
            {
                ViewBag.Message = "Please enter a log message.";
                return View();
            }

            var shareClient = _storageService.GetLogFileShare();

            var directory = shareClient.GetRootDirectoryClient();

            string fileName = $"Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            var fileClient = directory.GetFileClient(fileName);

            byte[] bytes = Encoding.UTF8.GetBytes(logMessage);

            using var stream = new MemoryStream(bytes);

            try
            {
                await fileClient.CreateAsync(stream.Length);

                stream.Position = 0;

                await fileClient.UploadRangeAsync(
                    new HttpRange(0, stream.Length),
                    stream);

                ViewBag.Message = "Log file saved successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var shareClient = _storageService.GetLogFileShare();

            var directory = shareClient.GetRootDirectoryClient();

            var files = new List<string>();

            await foreach (var item in directory.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                {
                    files.Add(item.Name);
                }
            }

            return View(files);
        }
    }
}
