namespace ABCRetail.Models
{
    public class Order
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Status { get; set; } = "Processing";
    }
}
