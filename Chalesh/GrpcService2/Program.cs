// See https://aka.ms/new-console-template for more information
using Chalesh.Core.Utils;
using Grpc.Net.Client;
using GrpcService2;
using Newtonsoft.Json.Linq;

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
JObject obj = new JObject(File.ReadAllText("/appsettings.json"));
for (int i = 0; i < obj.Count; i++)
{
    keyValuePairs.Add(obj["Types"]["RegexName"].ToString(), obj["Types"]["Pattern"].ToString());
}

// Get string into quotes
var channel = GrpcChannel.ForAddress("https://localhost:7146", new GrpcChannelOptions
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
    var req2 = new DataService2ToServic1 {Engine = keyValuePairs.Keys.First(), Id = 123, MessageLenght = "10", IsValid = true  };
    var request2 = new SendRequestService2.SendRequestService2Client(channel);

    var serverReply2 = await request2.RequestService2Async(req2);
    foreach (var item in keyValuePairs)
    {
        CodeFactory.GetSpecificStingWithRegex(serverReply2.Message, item.Value);

    } 
}