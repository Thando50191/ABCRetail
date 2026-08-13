using ABCRetail.Models;
using ABCRetailAzure.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AzureStorageService _storageService;

        public ProductsController(AzureStorageService storageService)
        {
            _storageService = storageService;
        }

        public IActionResult Index()
        {
            var tableClient = _storageService.GetProductsTable();

            var products = tableClient.Query<Products>().ToList();

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products product, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var containerClient = _storageService.GetProductImagesContainer();

                var fileName = Guid.NewGuid().ToString()
                               + Path.GetExtension(imageFile.FileName);

                var blobClient = containerClient.GetBlobClient(fileName);

                using (var stream = imageFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                product.ImageUrl = blobClient.Uri.ToString();
            }

            product.PartitionKey = "Products";
            product.RowKey = Guid.NewGuid().ToString();

            var tableClient = _storageService.GetProductsTable();

            tableClient.AddEntity(product);

            return RedirectToAction(nameof(Index));
        }
    }
    
}
