namespace Reporting.SalesWebAPI.Models
{
    public class SaleDetailInfo
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ProductName { get; set; }
    }
}
