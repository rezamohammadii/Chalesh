using Chalesh.Core.Models;
using Chalesh.Core.Services;
using Chalesh.Core.Utils;
using Grpc.Net.Client;
using GrpcService1;
using GrpcService1.Services;
using Newtonsoft.Json;
using System.Text;

public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
       
        // Add services to the container.
        builder.Services.AddGrpc();

        builder.Services.AddSingleton<IStoreData, StoreDataService>();
        builder.Services.AddSingleton<IKafkaService, KafkaService>();

        var app = builder.Build();

        // Handle background service for healthcheck
        builder.Services.AddHostedService<HealthCheckService>();


        // Configure the HTTP request pipeline.
        app.MapGrpcService<FirstRequestService>();
        app.MapGrpcService<HandeleRequestClient>();
        app.MapGrpcService<HandleRequestService>();
        app.MapGrpcService<HandleRequestService2>();
        app.MapGet("/", () => "Ready for connected client and service");
        app.Run();
        // Press Ctrl+C for shutdown service 
        app.WaitForShutdown();

    }

}


