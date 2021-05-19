using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProducerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerService
{
    public class ServiceBusTopicSender
    {
        private ServiceBusClient _bus;
        public ServiceBusTopicSender(ILogger<ServiceBusTopicSender> logger, ServiceBusConfig serviceBusConfig)
        {
            Logger = logger;
            ServiceBusConfig = serviceBusConfig;
        }

        public ILogger<ServiceBusTopicSender> Logger { get; }
        public ServiceBusConfig ServiceBusConfig { get; }

        public Task Connect()
        {
            try
            {
                _bus = new ServiceBusClient(ServiceBusConfig.ConnectionString);
                Logger.LogInformation($"Connection to Service Bus Topic {ServiceBusConfig.TopicName} successfully established!");
            }
            catch(Exception ex)
            {
                Logger.LogError("Connection to Service Bus Failed!");
                Logger.LogError(ex.Message);
            }
            return Task.CompletedTask;
        }

        public async Task SendMessageAsync(ProductPriceMessage priceMessage)
        {
            ServiceBusSender sender = _bus.CreateSender(ServiceBusConfig.TopicName);
            Logger.LogInformation("Sending price query to service bus");
            string data = JsonConvert.SerializeObject(priceMessage);
            ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(data));

            await sender.SendMessageAsync(message);
        }

        public async Task CloseAsync()
        {
            await _bus.DisposeAsync();
        }
    }
}
