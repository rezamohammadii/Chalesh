using Chalesh.Core.Models;
using Chalesh.Core.Utils;
using Confluent.Kafka;
using Newtonsoft.Json;
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
            // Producer func 
            using (var producer = new ProducerBuilder<Null, string>(KafkaConfig()).Build())
            {
                var msg = new Message<Null, string>
                {
                    Value = message
                };

                await producer.ProduceAsync(topic, msg);
            }
            return true;
        }

        public void ConsumerAsync(string topic)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string> (KafkaConfig()).Build())
            {
                consumer.Subscribe(topic);
                DateTime startTime = DateTime.Now;

                // It runs in a loop for thirty seconds and extracts the data in the queue
                while ((DateTime.Now - startTime).TotalSeconds < 30)
                {
                    var result = consumer.Consume();
                    var data = JsonConvert.DeserializeObject<ConsumerModel>(result.Message.Value);
                    CodeFactory.ConsumerModels = data;
                    Console.WriteLine($"Consumed message '{result.Message.Value}' at: '{result.TopicPartitionOffset}'.");
                }
               
            }
        }

    }
}
