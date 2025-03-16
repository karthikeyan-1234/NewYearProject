using Purchases.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchases.Services.DTOs
{
    public class PurchaseResponse
    {
        public Purchase? Result { get; set; }
        public PurchaseDetail[]? PurchaseDetails { get; set; }
    }
}
