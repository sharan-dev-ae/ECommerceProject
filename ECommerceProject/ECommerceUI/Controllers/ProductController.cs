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
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;

        public ProductController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiOptions)
        {
            _httpClientFactory = httpClientFactory;
            _apiSettings = apiOptions.Value;
        }

        private string ProductApiUrl => $"{_apiSettings.BaseUrl}/{_apiSettings.ProductEndpoint}";

        public async Task<IActionResult> Index(Guid? editId = null)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(ProductApiUrl);
            var products = new List<Product>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                products = JsonSerializer.Deserialize<List<Product>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var viewModel = new ProductListViewModel
            {
                Products = products,
                EditProduct = editId.HasValue ? products.FirstOrDefault(p => p.Id == editId.Value) : null
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Invalid product data.";
                return RedirectToAction("Index");
            }

            product.ImageUrls = new List<string>();
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ProductApiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Product created successfully!";
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Failed to create product.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Invalid product data.";
                return RedirectToAction("Index");
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{ProductApiUrl}/{product.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Product updated successfully!";
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Failed to update product.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{ProductApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertType"] = "success";
                TempData["AlertMessage"] = "Product deleted successfully!";
            }
            else
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = "Failed to delete product.";
            }

            return RedirectToAction("Index");
        }
    }
}
