// See https://aka.ms/new-console-template for more information
using Chalesh.Core.Utils;
using Grpc.Net.Client;
using GrpcClient;
string data = @"{
        ""Id"":""UniqueId"",
        ""Type"":""EngineType""}";
using var chanel = GrpcChannel.ForAddress("https://localhost:5003", new GrpcChannelOptions
{
    HttpHandler = new SocketsHttpHandler
    {
        EnableMultipleHttp2Connections = true,
        MaxConnectionsPerServer = CodeFactory.modelOut.NumberOfActiveClients,
        ConnectTimeout = TimeSpan.FromSeconds(CodeFactory.modelOut.ExpirationTime)
    }
});
var client = new Greeter.GreeterClient(chanel);
var reply = await client.SayHelloAsync(
    new HelloRequest { Name = data });

// Recevied message from service 1
Console.WriteLine("Responsing: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();