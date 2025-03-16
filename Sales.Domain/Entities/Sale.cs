using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Domain.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
