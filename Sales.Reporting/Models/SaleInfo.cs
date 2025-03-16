namespace Reporting.SalesWebAPI.Models
{
    public class SaleInfo
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
