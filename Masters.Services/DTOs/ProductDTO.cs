using Masters.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masters.Services.DTOs
{
    public class ProductDTO : BaseEntity
    {
        public string? Name { get; set; }
        public int ProductTypeId { get; set; }
        public ProductType? Category { get; set; }
    }
}
