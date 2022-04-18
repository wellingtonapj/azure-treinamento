using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using cloudtreinamentos.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace cloudtreinamentos.Controllers
{
    public class HomeController : Controller
    {
        const string blobContainerName = "gallery";
        static BlobContainerClient blobContainer;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            string connectionString = "BlobEndpoint=https://cloudtreinamentossa.blob.core.windows.net/;QueueEndpoint=https://cloudtreinamentossa.queue.core.windows.net/;FileEndpoint=https://cloudtreinamentossa.file.core.windows.net/;TableEndpoint=https://cloudtreinamentossa.table.core.windows.net/;SharedAccessSignature=sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacupitfx&se=2023-04-13T13:49:23Z&st=2022-04-13T05:49:23Z&spr=https,http&sig=eVz6xeHohrZ5cUZu5odGdIkUdeVPVqS7HrKyyhJQ7Nk%3D";
            
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
