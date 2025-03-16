namespace Reporting.SalesWebAPI.Models
{
    public class PurchaseInfo
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
