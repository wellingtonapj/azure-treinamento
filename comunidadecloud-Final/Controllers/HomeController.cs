using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using comunidadecloud.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Configuration;

namespace comunidadecloud.Controllers
{
    public class HomeController : Controller
    {
        const string blobContainerName = "iamges";
        static BlobContainerClient blobContainer;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            string connectionString = "BlobEndpoint=https://sacomunidadecloud.blob.core.windows.net/;QueueEndpoint=https://sacomunidadecloud.queue.core.windows.net/;FileEndpoint=https://sacomunidadecloud.file.core.windows.net/;TableEndpoint=https://sacomunidadecloud.table.core.windows.net/;SharedAccessSignature=sv=2020-08-04&ss=b&srt=sco&sp=rwdlacitfx&se=2023-04-14T07:17:16Z&st=2022-04-13T23:17:16Z&spr=https,http&sig=NTnFjNrBmUAKESa%2Blt9WU2RxIhvn9%2BoAbqrFg%2Fmkvx0%3D";
            
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            blobContainer = blobServiceClient.GetBlobContainerClient(blobContainerName);
            await blobContainer.CreateIfNotExistsAsync(PublicAccessType.None);

            var model = new List<Blobs>();
            foreach (BlobItem blob in blobContainer.GetBlobs())
            {
                if (blob.Properties.BlobType == BlobType.Block)
                    model.Add(new Blobs{ URL = blobContainer.GetBlobClient(blob.Name).Uri.ToString()});
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
