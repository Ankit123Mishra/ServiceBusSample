using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerService
{
    public class ServiceBusConfiguration
    {
        public string ConnectionString { get; set; }
        public string TopicName { get; set; }
        public string SubscriptionName { get; set; }

    }
}
