using Chalesh.Core.Models;
using Chalesh.Core.Utils;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GrpcService1.Services
{
    public class HelathCheck
    {
        private readonly ILogger<HelathCheck> _logger;
        private readonly IConfiguration _cfg;
        public HelathCheck(ILogger<HelathCheck> logger, IConfiguration cfg)
        {
            _logger = logger;
            _cfg = cfg;
        }
        public void SendRequest()
        {
            var httpClient = new HttpClient();

            // Set up a timer to trigger the request every 30 seconds.
            var timer = new Timer(async _ =>
            {
                await SendRequestAsync(httpClient);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        }
        private async Task SendRequestAsync(HttpClient httpClient)
        {
            try
            {
                // Create MainService Data Model For Send Request Main Service
                MainServiceDataModelIn mainService = new MainServiceDataModelIn();

                mainService.Id = CodeFactory.GenerateGuidFromMacAddress();
                mainService.SystemTime = DateTime.Now;
                mainService.NumberofConnectedClients = _cfg.GetValue<int>("NumberofConnectedClients");

                // Read Thumbprint from setting file
                var expectedThumbprint = _cfg.GetValue<string>("Thumbprint");
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
                        return true;
                    };
                    var client = new HttpClient();

                    // Set the URL for the POST request
                    var url = _cfg.GetValue<string>("MainServeiceAddress");

                    // Create a new HttpRequestMessage with the POST method
                    var request = new HttpRequestMessage(HttpMethod.Post, url);

                    // Add any necessary headers or content to the request here
                    request.Content = new StringContent(mainService.ToString()!, Encoding.UTF8, "application/json");

                    // Send the POST request and wait for the response
                    var response = await client.SendAsync(request);

                    // Do something with the response here if needed
                    if (response.StatusCode== System.Net.HttpStatusCode.OK)
                    {

                        // Check validate thumbprint
                        if (thumbPrint == expectedThumbprint)
                        {
                            Console.WriteLine("SSL thumbprint matches!");

                            // Get response from main service
                            var responseContent  = await response.Content.ReadAsStringAsync();

                            // Convert received data (string) format in MainServiceDataModelOut object
                            var model = JsonConvert.DeserializeObject<MainServiceDataModelOut>(responseContent);

                            // Pass DeserializeObject to global object
                            CodeFactory.modelOut = model;
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
                _logger.LogError(ex.Message);
            }
        }
    }
}
