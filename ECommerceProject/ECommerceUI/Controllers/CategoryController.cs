using ECommerceUI.Models;
using ECommerceUI.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace ECommerceUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public CategoryController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiOptions)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiOptions.Value;
        }

        private string CategoryApiUrl => $"{_apiSettings.BaseUrl}/{_apiSettings.CategoryEndpoint}";

        public async Task<IActionResult> Index(Guid? editId = null)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(CategoryApiUrl);
            var categories = new List<Category>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                categories = JsonSerializer.Deserialize<List<Category>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var viewModel = new CategoryListViewModel
            {
                Categories = categories,
                EditCategory = editId.HasValue ? categories.FirstOrDefault(c => c.Id == editId.Value) : null
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Invalid category data.";
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(CategoryApiUrl, content);

            TempData["AlertType"] = response.IsSuccessStatusCode ? "success" : "error";
            TempData["AlertMessage"] = response.IsSuccessStatusCode
                ? "Category created successfully!"
                : "Failed to create category.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Invalid category data.";
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{CategoryApiUrl}/{category.Id}", content);

            TempData["AlertType"] = response.IsSuccessStatusCode ? "success" : "error";
            TempData["AlertMessage"] = response.IsSuccessStatusCode
                ? "Category updated successfully!"
                : "Failed to update category.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{CategoryApiUrl}/{id}");

            TempData["AlertType"] = response.IsSuccessStatusCode ? "success" : "error";
            TempData["AlertMessage"] = response.IsSuccessStatusCode
                ? "Category deleted successfully!"
                : "Failed to delete category.";

            return RedirectToAction("Index");
        }
    }
}
