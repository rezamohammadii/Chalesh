using Chalesh.Core.Models;
using Chalesh.Core.Services;
using Chalesh.Core.Utils;
using Newtonsoft.Json;
using System.Text;

namespace GrpcService1.Services
{
    public class HealthCheckService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private int executionCount = 0;
        private readonly ILogger<HealthCheckService> _logger;
        private readonly IConfiguration _cfg;
        public void Dispose()
        {
            _timer?.Dispose();
        }
        public HealthCheckService(ILogger<HealthCheckService> logger, IConfiguration cfg)
        {
            _logger = logger;
            _cfg = cfg;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Request Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }
        private async void DoWork(object? state)
        {        
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
                        _logger.LogInformation("[*] Server thumbPrint: ", thumbPrint);
                        return true;
                    };
                    HttpClient client = new HttpClient();

                    // Set the URL for the POST request
                    string url = _cfg.GetValue<string>("MainServeiceAddress");
                    _logger.LogInformation("[*] Server url : ", url);

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
                _logger.LogError("[!] Error exception method healthchek ", ex.Message);
                _logger.LogTrace("[!] Trace exception method healthchek ", ex.StackTrace);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
