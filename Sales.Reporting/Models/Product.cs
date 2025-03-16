
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.SalesWebAPI.Models
{
    public class Product: BaseEntity
    {
        public string? Name { get; set; }
        public int ProductTypeId { get; set; }
    }
}
