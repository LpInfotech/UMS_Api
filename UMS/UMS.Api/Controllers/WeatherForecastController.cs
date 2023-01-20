using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

namespace UMS.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet]
        public  async Task<string> GetUserAsync() 
        {
            var clientId = _configuration.GetValue<string>("AzureAd:ClientId");
            var tenantId = _configuration.GetValue<string>("AzureAd:TenantId");
            var clientSecert = _configuration.GetValue<string>("AzureAd:SecretId");
            var clientSecertCredentials = new ClientSecretCredential(tenantId, clientId, clientSecert);
            GraphServiceClient graphServiceClient = new GraphServiceClient(clientSecertCredentials);
            try
            {
                var user = graphServiceClient.Users.Request().Select(x => x.DisplayName).GetAsync().Result;
            }
            catch (Exception ex)
            {

                var error =  ex;
            }

            return "";
        }
    }
}