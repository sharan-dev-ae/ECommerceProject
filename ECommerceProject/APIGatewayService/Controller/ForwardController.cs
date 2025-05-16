using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            var forwardUrl = $"{_serviceMap[service.ToLower()]}/{path}";

            var request = new HttpRequestMessage(HttpMethod.Get, forwardUrl);
            ForwardAuthorizationHeader(request);

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPost("{service}/{**path}")]
        public async Task<IActionResult> Post(string service, string path, [FromBody] object body)
        {
            if (!_serviceMap.ContainsKey(service.ToLower()))
                return BadRequest("Invalid service");

            var client = _httpClientFactory.CreateClient("ForwardingClient");
            var forwardUrl = $"{_serviceMap[service.ToLower()]}/{path}";

            var request = new HttpRequestMessage(HttpMethod.Post, forwardUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            ForwardAuthorizationHeader(request);

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPut("{service}/{**path}")]
        public async Task<IActionResult> Put(string service, string path, [FromBody] object body)
        {
            if (!_serviceMap.ContainsKey(service.ToLower()))
                return BadRequest("Invalid service");

            var client = _httpClientFactory.CreateClient("ForwardingClient");
            var forwardUrl = $"{_serviceMap[service.ToLower()]}/{path}";

            var request = new HttpRequestMessage(HttpMethod.Put, forwardUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };
            ForwardAuthorizationHeader(request);

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpDelete("{service}/{**path}")]
        public async Task<IActionResult> Delete(string service, string path)
        {
            if (!_serviceMap.ContainsKey(service.ToLower()))
                return BadRequest("Invalid service");

            var client = _httpClientFactory.CreateClient("ForwardingClient");
            var forwardUrl = $"{_serviceMap[service.ToLower()]}/{path}";

            var request = new HttpRequestMessage(HttpMethod.Delete, forwardUrl);
            ForwardAuthorizationHeader(request);

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        private void ForwardAuthorizationHeader(HttpRequestMessage request)
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                request.Headers.Authorization =
                    AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            }
        }
    }
}
