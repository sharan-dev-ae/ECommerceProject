using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using APIGatewayService.Models;


namespace APIGatewayService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForwardController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Dictionary<string, string> _serviceMap;

        public ForwardController(IHttpClientFactory httpClientFactory, IOptions<MicroserviceSettings> options)
        {
            _httpClientFactory = httpClientFactory;

            var settings = options.Value;

            _serviceMap = new Dictionary<string, string>
            {
                { "category", settings.Category },
                { "product", settings.Product },
                { "company", settings.Company }
            };
        }

        [HttpGet("{service}/{**path}")]
        public async Task<IActionResult> Get(string service, string path)
        {
            if (!_serviceMap.ContainsKey(service.ToLower()))
                return BadRequest("Invalid service");

            var client = _httpClientFactory.CreateClient("ForwardingClient");

            var serviceUrl = _serviceMap[service.ToLower()];

            var forwardUrl = $"{serviceUrl}/{path}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, forwardUrl);

            if (Request.Headers.ContainsKey("Authorization"))
            {
                requestMessage.Headers.Authorization =
                    AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            }
            Console.WriteLine($"Forwarding request to: {forwardUrl}");

            var response = await client.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPost("{service}/{**path}")]
        public async Task<IActionResult> Post(string service, string path, [FromBody] object body)
        {
            if (!_serviceMap.ContainsKey(service.ToLower()))
                return BadRequest("Invalid service");

            var client = _httpClientFactory.CreateClient("ForwardingClient");

            var serviceUrl = _serviceMap[service.ToLower()];

            var forwardUrl = $"{serviceUrl}/{path}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, forwardUrl)
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json")
            };

            if (Request.Headers.ContainsKey("Authorization"))
            {
                requestMessage.Headers.Authorization =
                    AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            }

            var response = await client.SendAsync(requestMessage);

            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }
    }

 
}
