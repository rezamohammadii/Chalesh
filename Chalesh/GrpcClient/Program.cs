// See https://aka.ms/new-console-template for more information
using Chalesh.Core.Utils;
using Grpc.Net.Client;

using GrpcClient;

Console.WriteLine("Press any key to exit...");

using var chanel = GrpcChannel.ForAddress("https://localhost:7146", new GrpcChannelOptions
{
    HttpHandler = new SocketsHttpHandler
    {
        // Automatic handle request online client connected to server based MaxConnectionsPerServer
        EnableMultipleHttp2Connections = true,

        // Set Max Connection from receive data main service 
        MaxConnectionsPerServer = CodeFactory.modelOut.NumberOfActiveClients,

        // After next time cloese connection 
        ConnectTimeout = TimeSpan.FromSeconds(CodeFactory.modelOut.ExpirationTime),

    },

    // Http Connection is no accept
    DisposeHttpClient = true,

});
var client = new SendRequestToClient.SendRequestToClientClient(chanel);
var reply = client.BidirectionalStream(new HelloRequest { Message = "clien-1" });

//// Recevied message from service 1
//   Console.WriteLine("Responsing: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

