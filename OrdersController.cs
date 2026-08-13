using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using Azure.Storage.Queues;
using System.Text.Json;
using ABCRetail.Models;
using ABCRetailAzure.Services;
using Azure;

namespace ABCRetail.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AzureStorageService _storageService;

        public OrdersController(AzureStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet]


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }
            order.OrderId = Guid.NewGuid().ToString();
            order.Status = "Processing";

            QueueClient queueClient = _storageService.GetOrderProcessingQueue();
            string message = JsonSerializer.Serialize(order);
            
            await queueClient.SendMessageAsync(message);
            
            ViewBag.Message = "Order sent for processing successfully.";
            
                  return View(order);


        }
    }
}