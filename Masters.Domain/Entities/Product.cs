using Masters.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masters.Domain.Entities
{
    public class Product: BaseEntity
    {
        public string? Name { get; set; }
        public int ProductTypeId { get; set; }
    }
}
