using Chalesh.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chalesh.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {


        private readonly ILogger<MainController> _logger;
        private readonly IConfiguration _cfg;

        public MainController(ILogger<MainController> logger, IConfiguration cfg)
        {
            _logger = logger;
            _cfg = cfg;
        }

        [HttpPost]
        public IActionResult Post([FromBody] MainServiceDataModelIn modelIn)
        {
            MainServiceDataModelOut modelOut = new MainServiceDataModelOut()
            {
                ExpirationTime = _cfg.GetValue<int>("ExpirationTime"),
                IsEnabled = _cfg.GetValue<bool>("IsEnabled"),
                NumberOfActiveClients = _cfg.GetValue<int>("NumberOfActiveClients"),
            };

            return Ok(modelOut);
        }
    }
}