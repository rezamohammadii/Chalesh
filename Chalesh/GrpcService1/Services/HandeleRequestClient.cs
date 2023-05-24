using Chalesh.Core.Models;
using Grpc.Core;
using GrpcService1;

namespace GrpcService1.Services
{
    public class HandeleRequestClient : SendRequestToClient.SendRequestToClientBase
    {
        private readonly ILogger<HandeleRequestClient> _logger;
        public HandeleRequestClient(ILogger<HandeleRequestClient> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> BidirectionalStream(HelloRequest request, ServerCallContext context)
        {
            return base.BidirectionalStream(request, context);
        }
    }
}