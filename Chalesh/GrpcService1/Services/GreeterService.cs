using Chalesh.Core.Models;
using Grpc.Core;
using GrpcService1;

namespace GrpcService1.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task BidirectionalStream(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                var response = new HelloReply { Message = $"Received '{request.Name}'" };
                await responseStream.WriteAsync(response);
            }
        }
    }
}