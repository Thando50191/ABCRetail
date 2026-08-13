using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace ABCRetailAzure.Services
{
    public class AzureStorageService
    {
        private readonly string _connectionString;

        public AzureStorageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage")
                ?? throw new InvalidOperationException("Azure Storage connection string is missing.");
        }

        public TableClient GetCustomersTable()
        {
            var tableServiceClient = new TableServiceClient(_connectionString);

            var tableClient = tableServiceClient.GetTableClient("Customers");

            tableClient.CreateIfNotExists();

            return tableClient;
        }

        public TableClient GetProductsTable()
        {
            var tableServiceClient = new TableServiceClient(_connectionString);

            var tableClient = tableServiceClient.GetTableClient("Products");

            tableClient.CreateIfNotExists();

            return tableClient;
        }

        public BlobContainerClient GetProductImagesContainer()
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);

            var containerClient =
                blobServiceClient.GetBlobContainerClient("product-images");

            containerClient.CreateIfNotExists();

            return containerClient;
        }
        public QueueClient GetOrderProcessingQueue()
        {
            var queueServiceClient = new QueueServiceClient(_connectionString);

            var queueClient = queueServiceClient.GetQueueClient("order-processing");

            queueClient.CreateIfNotExists();

            return queueClient;
        }
        public ShareClient GetLogFileShare()
        {
            var shareClient = new ShareClient(
                _connectionString,
                "abc-retail-logs");

            shareClient.CreateIfNotExists();

            return shareClient;
        }
    }
} 
