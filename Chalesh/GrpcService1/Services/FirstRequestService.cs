using Chalesh.Core.Models;
using Chalesh.Core.Services;
using Chalesh.Core.Utils;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class FirstRequestService : ValidService2.ValidService2Base
    {
        private IStoreData store;
        public FirstRequestService(IStoreData store)
        {
            this.store = store;
        }
        public override Task<Empty> FirstRequestService2(Service2SendData request, ServerCallContext context)
        {
            Service2DetailModel service = new Service2DetailModel();
            service.Id = request.Id;
            service.Type = request.Type;
            store.StoreService2DetailOut(service);
            return base.FirstRequestService2(request, context);
        }
    }
}
