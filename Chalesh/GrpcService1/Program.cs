using Grpc.Net.Client;
using GrpcService1;
using GrpcService1.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

using var chanel = GrpcChannel.ForAddress("https://localhost:5001");
var client  = new Greeter.
// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


app.Run();
