// See https://aka.ms/new-console-template for more information
using Chalesh.Core.Utils;
using Grpc.Net.Client;
using GrpcService2;

Console.WriteLine("Hello, World!");

var handler = new HttpClientHandler();
handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls;
handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
handler.UseProxy = false;
var firstMessage = new Service2SendData
{
    Id = CodeFactory.GenerateGuidFromMacAddress().ToString(),
    Type = "EngineType"
};

var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    HttpHandler = handler,
    DisposeHttpClient = true,
});
var client1 = new ValidService2.ValidService2Client(channel);
var serverReply = await client1.FirstRequestService2Async(firstMessage);