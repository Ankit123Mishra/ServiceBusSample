using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProducerService
{
    public class ServiceBusConfig
    {
        public string ConnectionString { get; set; }
        public string TopicName { get; set; }
    }
}
