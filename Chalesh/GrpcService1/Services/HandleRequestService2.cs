using Chalesh.Core.Services;
using Chalesh.Core.Utils;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class HandleRequestService2 : SendRequestService1.SendRequestService1Base
    {
        private readonly IConfiguration _cfg;
        public HandleRequestService2(IConfiguration cfg)
        {
            _cfg = cfg;
        }
        public override async Task<HelloReply> RequestService1(DataService2ToServic1 request, ServerCallContext context)
        {
            // If it is active, it is allowed to receive
            if (CodeFactory.modelOut.IsEnabled)
            {
                // 
                KafkaService kafkaService = new KafkaService();
                string topicName = _cfg.GetValue<string>("TopicName");
                kafkaService.Producer(topicName, request.ToString());
            }
            return await Task.FromResult(new HelloReply
            {
                Message = "OK "
            });
        }
    }
}
