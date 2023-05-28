using Chalesh.Core.Services;
using Chalesh.Core.Utils;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class HandleRequestService2 : SendRequestService1.SendRequestService1Base
    {
        private readonly IConfiguration _cfg;
        private readonly ILogger<HandleRequestService2> _logger;
        private IStoreData _storeData;

        public HandleRequestService2(IConfiguration cfg, ILogger<HandleRequestService2> logger, IStoreData store)
        {
            _cfg = cfg;
            _logger = logger;
            _storeData = store;
        }
        public override async Task<HelloReply> RequestService1(DataService2ToServic1 request, ServerCallContext context)
        {
            // If it is active, it is allowed to receive
            if (_storeData.GetMainServiceDataOut().IsEnabled)
            {
                
                KafkaService kafkaService = new KafkaService();
                string topicName = _cfg.GetValue<string>("TopicName");
                _logger.LogDebug("Topic Name: ", topicName);
                kafkaService.Producer(topicName, request.ToString());
            }
            return await Task.FromResult(new HelloReply
            {
                Message = "OK "
            });
        }
    }
}
