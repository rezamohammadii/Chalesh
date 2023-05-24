using Chalesh.Core.Utils;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class HandleRequestService : SendRequestService2.SendRequestService2Base
    {
        public override Task<DataService1ToServic2> RequestService2(Empty request, ServerCallContext context)
        {
            // If it is active, it is allowed to receive
            DataService1ToServic2 dataService = new DataService1ToServic2();
            if (CodeFactory.modelOut.IsEnabled)
            {
                // Handle response data for service2
                dataService.Message = CodeFactory.ConsumerModels.Message;
                dataService.Id = CodeFactory.ConsumerModels.Id;
                dataService.Sender = CodeFactory.ConsumerModels.Sender;
                
            }
            return Task.FromResult(dataService);
        }
    }
}
