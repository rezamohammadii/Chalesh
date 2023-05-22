using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Services
{
    public class KafkaService
    {  
        
        public ProducerConfig KafkaConfig()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "my-app"
            };
            return config;
        }
        public async Task<bool> ProduceAsync(string topic, string message)
        {

            using (var producer = new ProducerBuilder<Null, string>(KafkaConfig()).Build())
            {
                var msg = new Message<Null, string>
                {
                    Value = message
                };

                await producer.ProduceAsync("my-topic", msg);
            }
            return true;
        }

        public void ConsumerAsync(string topic, string message)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(KafkaConfig()).Build())
            {
                consumer.Subscribe(topic);

                while (true)
                {
                    var result = consumer.Consume();

                    Console.WriteLine($"Consumed message '{result.Message.Value}' at: '{result.TopicPartitionOffset}'.");
                }
            }
        }

    }
}
