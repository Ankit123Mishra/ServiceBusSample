using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProducerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerService.Controllers
{
    public class PriceController : Controller
    {
        private readonly ServiceBusTopicSender serviceBus;
        public ILogger<PriceController> Logger { get; }

        public PriceController(ILogger<PriceController> logger, ServiceBusTopicSender serviceBus)
        {
            Logger = logger;
            this.serviceBus = serviceBus;
        }

        [Route("productprice")]
        [HttpPost("")]
        public async Task<IActionResult> SendProductQuery([FromBody] ProductPriceMessage priceMessage)
        {
            Logger.LogInformation("New Price Query Receieved!");
            Logger.LogInformation("Client Name: " + priceMessage.ClientName);
            Logger.LogInformation("Customer Id: " + priceMessage.CustomerId);
            Logger.LogInformation("Product Id: " + priceMessage.ProductId);
            Logger.LogInformation("Qty: " + priceMessage.Qty);

            await serviceBus.SendMessageAsync(priceMessage);

            // TODO: return price message response object
            return Ok(priceMessage);
        }
    }
}
