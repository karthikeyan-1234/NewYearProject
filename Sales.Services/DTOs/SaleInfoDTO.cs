using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Services.DTOs
{
    public class SaleInfoDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public IEnumerable<SaleDetailInfoDTO?> ?SaleDetails { get; set; }
    }
}
