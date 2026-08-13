using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;
using ABCRetailAzure.Services;


namespace ABCRetail.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AzureStorageService _storageService;
            public CustomersController(AzureStorageService storageService)
        {
             _storageService = storageService;
        }
        public IActionResult Index()
        {
            var tableClient = _storageService.GetCustomersTable();
            var customers = tableClient.Query<Customer>().ToList();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        
            {
            return View();
            }
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }
            customer.PartitionKey = "Customers";
            customer.RowKey = Guid.NewGuid().ToString();

            var tableClient =  _storageService.GetCustomersTable();
            tableClient.AddEntity(customer);
            return RedirectToAction(nameof(Index));
        }
    }
}
