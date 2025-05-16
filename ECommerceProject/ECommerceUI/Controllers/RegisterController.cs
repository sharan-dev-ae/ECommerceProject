using ECommerceUI.Models;
using ECommerceUI.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public RegisterController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiOptions)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiOptions.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var registerUrl = $"{_apiSettings.BaseUrl}/{_apiSettings.RegisterEndpoint}";
            var response = await client.PostAsync(registerUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Error = "Registration failed. Please try again.";
            return View(model);
        }
    }
}
