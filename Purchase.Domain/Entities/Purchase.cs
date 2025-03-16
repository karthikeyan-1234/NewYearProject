using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Domain.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
