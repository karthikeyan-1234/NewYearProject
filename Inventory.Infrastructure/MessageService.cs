using Confluent.Kafka;


using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Inventory.Domain.Contracts;

namespace Inventory.Infrastructure
{
    public class MessageService : IMessageService
    {
        ProducerConfig config;

        public MessageService(IConfiguration configuration) {
            var bootStrapServer = configuration!.GetSection("Kafka:BootstrapServers").Value;
            config = new ProducerConfig { BootstrapServers = bootStrapServer };
        }

        public async Task PublishMessage<T> (T obj,string topic)
        {
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonSerializer.Serialize(obj) });
                producer.Flush();
            }
        }


    }
}
