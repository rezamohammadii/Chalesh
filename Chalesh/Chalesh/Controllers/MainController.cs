using Chalesh.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chalesh.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {


        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] MainServiceDataModelIn modelIn)
        {
            MainServiceDataModelOut modelOut = new MainServiceDataModelOut()
            {
                ExpirationTime = DateTime.Now.AddHours(3),
                IsEnabled = true,
                NumberOfActiveClients = 3
            };

            return Ok(modelOut);
        }
    }
}