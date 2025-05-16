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
    public class CompanyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public CompanyController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiOptions)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiOptions.Value;
        }

        private string CompanyApiUrl => $"{_apiSettings.BaseUrl}/{_apiSettings.CompanyEndpoint}";

        public async Task<IActionResult> Index(Guid? editId = null)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(CompanyApiUrl);
            var companies = new List<Company>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                companies = JsonSerializer.Deserialize<List<Company>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var viewModel = new CompanyListViewModel
            {
                Companies = companies,
                EditCompany = editId.HasValue ? companies.FirstOrDefault(c => c.Id == editId.Value) : null
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Company company)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Invalid company data.";
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(company);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(CompanyApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Company created successfully!";
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Failed to create company.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Company company)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Invalid company data.";
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(company);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{CompanyApiUrl}/{company.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Company updated successfully!";
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Failed to update company.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{CompanyApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Company deleted successfully!";
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Failed to delete company.";
            }

            return RedirectToAction("Index");
        }
    }
}
