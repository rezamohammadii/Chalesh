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

        builder.Services.AddTransient<IStoreData, StoreDataService>();

        var app = builder.Build();
        // Handle request for 30 sec to healthcheck
        Timer timer = new Timer(async _ =>
        {
            await HealthCheck(builder.Configuration);
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));


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
    private static async Task HealthCheck(IConfiguration _cfg)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        ILogger logger = loggerFactory.CreateLogger<Program>();

        try
        {
            StoreDataService store = new StoreDataService();
            // Create MainService Data Model For Send Request Main Service
            MainServiceDataModelIn mainService = new MainServiceDataModelIn();

            mainService.Id = CodeFactory.GenerateGuidFromMacAddress();
            mainService.SystemTime = DateTime.Now;
            mainService.NumberofConnectedClients = _cfg.GetValue<int>("NumberofConnectedClients");

            // Read Thumbprint from setting file
            string expectedThumbprint = _cfg.GetValue<string>("Thumbprint");
            int maxRetries = 10;
            int retryCount = 0;
            bool success = false;
            string thumbPrint = "";

            // It will try 10 times until it is OK 
            while (retryCount < maxRetries && !success)
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) =>
                {
                    // Get thumbprint from main service
                    thumbPrint = cert!.Thumbprint;
                    logger.LogInformation("[*] Server thumbPrint: ", thumbPrint);
                    return true;
                };
                HttpClient client = new HttpClient();

                // Set the URL for the POST request
                string url = _cfg.GetValue<string>("MainServeiceAddress");
                logger.LogInformation("[*] Server url : ", url);

                // Create a new HttpRequestMessage with the POST method
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                // Add any necessary headers or content to the request here
                request.Content = new StringContent(mainService.ToString()!, Encoding.UTF8, "application/json");

                // Send the POST request and wait for the response
                HttpResponseMessage response = await client.SendAsync(request);

                // Do something with the response here if needed
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    // Check validate thumbprint
                    if (thumbPrint == expectedThumbprint)
                    {
                        Console.WriteLine("SSL thumbprint matches!");

                        // Get response from main service
                        string responseContent = await response.Content.ReadAsStringAsync();

                        // Convert received data (string) format in MainServiceDataModelOut object
                        var model = JsonConvert.DeserializeObject<MainServiceDataModelOut>(responseContent);

                        // Pass DeserializeObject to global object                       
                        store.StoreMainServiceDataOut(model!);
                    }
                    else
                    {
                        Console.WriteLine("SSL thumbprint does not match!");
                    }
                    success = true;
                }
                else
                {
                    retryCount++;
                }
            }

        }
        catch (Exception ex)
        {
            logger.LogError("[!] Error exception method healthchek ",ex.Message);
            logger.LogTrace("[!] Trace exception method healthchek ",ex.StackTrace);
        }
    }

}


