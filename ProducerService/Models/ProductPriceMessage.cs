using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerService.Models
{
    public class ProductPriceMessage
    {
        public string ClientName { get; set; }
        public string MessageId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Qty { get; set; }
    }
}
