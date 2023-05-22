using Chalesh.Core.Models;
using Chalesh.Core.Utils;

namespace GrpcService1.Services
{
    public class HelathCheck
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IConfiguration _cfg;
        public HelathCheck(ILogger<GreeterService> logger, IConfiguration cfg)
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
                MainServiceDataModel mainService = new MainServiceDataModel();

                mainService.Id = CodeFactory.GenerateGuidFromMacAddress();
                mainService.SystemTime = DateTime.Now;
                mainService.NumberofConnectedClients = _cfg.GetValue<int>("NumberofConnectedClients");

                using var client = new HttpClient();

                // Set the URL for the POST request
                var url = _cfg.GetValue<string>("MainServeiceAddress");

                // Create a new HttpRequestMessage with the POST method
                using var request = new HttpRequestMessage(HttpMethod.Post, url);

                // Add any necessary headers or content to the request here
                request.Content = new StringContent("my post data");

                // Send the POST request and wait for the response
                var response = await client.SendAsync(request);

                // Do something with the response here if needed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
