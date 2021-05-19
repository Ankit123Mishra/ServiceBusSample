using Azure.Messaging.ServiceBus;
using ConsumerService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerService
{
    public class ServiceBusConsumer : IHostedService
    {
        private ServiceBusProcessor _processor;
        public ServiceBusConsumer(ILogger<ServiceBusConsumer> logger, IConfiguration configuration, ServiceBusConfiguration serviceBusConfiguration)
        {
            Logger = logger;
            Configuration = configuration;
            ServiceBusConfiguration = serviceBusConfiguration;
        }

        private ServiceBusClient _client;
        public ILogger<ServiceBusConsumer> Logger { get; }
        public IConfiguration Configuration { get; }
        public ServiceBusConfiguration ServiceBusConfiguration { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new ServiceBusClient(ServiceBusConfiguration.ConnectionString);

            _processor = _client.CreateProcessor(ServiceBusConfiguration.TopicName, ServiceBusConfiguration.SubscriptionName, new ServiceBusProcessorOptions()
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            });

            Logger.LogInformation($"Connected to Service Bus {ServiceBusConfiguration.TopicName}");

            await RegisterOnMessageHandlerAndReceiveMessages();
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            _processor.ProcessMessageAsync += MessageHandler;

            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync();
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var priceMessage = JsonConvert.DeserializeObject<ProductPriceMessage>(Encoding.UTF8.GetString(args.Message.Body));
            Logger.LogInformation($"Message Received from {ServiceBusConfiguration.TopicName} to ATG Communication Hub");
            Logger.LogInformation("Client Name: " + priceMessage.ClientName);
            Logger.LogInformation("Customer Id: " + priceMessage.CustomerId);
            Logger.LogInformation("Product Id: " + priceMessage.ProductId);
            Logger.LogInformation("Qty: " + priceMessage.Qty);

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Logger.LogError(args.Exception, "Message handler encountered an exception");
            var context = args.Exception;

            Logger.LogDebug($"- Entity Path: {args.EntityPath}");
            Logger.LogDebug($"- Error Message: {context.Message}");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Service Bus Connection Successfully Closed!");
            await _client.DisposeAsync();
        }
    }
}
