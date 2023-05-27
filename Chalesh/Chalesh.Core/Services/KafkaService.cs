using Chalesh.Core.Models;
using Chalesh.Core.Utils;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chalesh.Core.Services
{
    public class KafkaService : IKafkaService
    {

        public void Listen(Action<string> message, string topic)
        {
            var config = new Dictionary<string, object>
            {
                {"group.id", "grpc_consumer" },
                {"bootstrap.servers","localhost:9092" },
                {"enable.auto.commit", "false" }
            };
            using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Subscribe(topic);
                consumer.OnMessage += (_, msg) =>
                {
                    message(msg.Value);
                };
            }
        }

        public async void Producer(string topic, string message)
        {
            var config = new Dictionary<string, object>
            {
                {"bootstrap.servers","localhost:9092" },
            };
            // Producer func 
            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {

                await producer.ProduceAsync(topic, null, message);
                producer.Flush(10);

            }
        }
    }
}
