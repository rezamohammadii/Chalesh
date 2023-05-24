using Chalesh.Core.Models;
using Chalesh.Core.Utils;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class FirstRequestService : ValidService2.ValidService2Base
    {
        public override Task<Empty> FirstRequestService2(Service2SendData request, ServerCallContext context)
        {
            Service2DetailModel service = new Service2DetailModel();
            service.Id = request.Id;
            service.Type = request.Type;
            CodeFactory.Service2Detail.Enqueue(service);
            return base.FirstRequestService2(request, context);
        }
    }
}
