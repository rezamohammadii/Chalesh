using Chalesh.Core.Services;
using Chalesh.Core.Utils;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class HandleRequestService : SendRequestService2.SendRequestService2Base
    {

        private IStoreData store;
        public HandleRequestService(IStoreData store)
        {
            this.store = store;
        }       
        public override Task<DataService1ToServic2> RequestService2(Empty request, ServerCallContext context)
        {
            // If it is active, it is allowed to receive
            DataService1ToServic2 dataService = new DataService1ToServic2();
            if (store.GetMainServiceDataOut().IsEnabled)
            {
                // Handle response data for service2
                dataService.Message = store.GetConsumerModel().Message;
                dataService.Id = store.GetConsumerModel().Id;
                dataService.Sender = store.GetConsumerModel().Sender;
                
            }
            return Task.FromResult(dataService);
        }
    }
}
