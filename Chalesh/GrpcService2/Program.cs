// See https://aka.ms/new-console-template for more information
using Chalesh.Core.Utils;
using Grpc.Net.Client;
using GrpcService2;

var handler = new HttpClientHandler();

// Check TLS when sending data
handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls;
handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
handler.UseProxy = false;

var firstMessage = new Service2SendData
{
    Id = CodeFactory.GenerateGuidFromMacAddress().ToString(),
    Type = "EngineType"
};
Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
// Get string into quotes
string pattern = "\"([^\"]*)\"";
var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    HttpHandler = handler,
    
    DisposeHttpClient = true,
});

// By connecting each service 2 to service 1, a message is sent to introduce the service
var request1 = new ValidService2.ValidService2Client(channel);
var serverReply = await request1.FirstRequestService2Async(firstMessage);

// 
while (true)
{
    var req2 = new Empty { };
    var request2 = new SendRequestService2.SendRequestService2Client(channel);

    var serverReply2 = await request2.RequestService2Async(req2);
    keyValuePairs.Add(serverReply2.Message, pattern);

   // var request3 = new 

}