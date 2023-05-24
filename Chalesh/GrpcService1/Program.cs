using Grpc.Net.Client;
using GrpcService1;
using GrpcService1.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapGrpcService<FirstRequestService>();
app.MapGrpcService<HandeleRequestClient>();
app.MapGrpcService<HandleRequestService>();
app.MapGrpcService<HandleRequestService2>();
app.MapGet("/", () => "Ready for connected client and service");
app.Run();
// Press Ctrl+C for shutdown service 
app.WaitForShutdown();