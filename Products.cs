using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models
{
    public class Products : ITableEntity
    {
        public string PartitionKey { get; set; } = "Products";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Double Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
