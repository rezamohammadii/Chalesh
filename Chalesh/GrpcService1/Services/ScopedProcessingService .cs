using Chalesh.Core.Services;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System.Text;

namespace rpcService1.Services
{

    internal interface IScopedProcessingService
    {
        void DoWork(CancellationToken stoppingToken);
    }
    public class ScopedProcessingService : IScopedProcessingService, IKafkaService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private IKafkaService kafka;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, IKafkaService kafka)
        {
            _logger = logger;
            this.kafka = kafka;
        }
        public void DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Listen(Console.WriteLine, "my-app" , stoppingToken);
            }
        }

        public void Listen(Action<string> message, string topic, CancellationToken stoppingToken)
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
                    Console.WriteLine(msg.Value);
                };
                while (!stoppingToken.IsCancellationRequested)
                {
                    consumer.Poll(100);
                }
            }
        }

        public void Producer(string topic, string message)
        {
            throw new NotImplementedException();
        }
    }
    public class ConsumeScopedServiceHostedService : BackgroundService
    {
        private readonly ILogger<ConsumeScopedServiceHostedService> _logger;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
            ILogger<ConsumeScopedServiceHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
