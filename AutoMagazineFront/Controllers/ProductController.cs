using System.Text;
using AutoMagazine.Models.Entities;
using AutoMagazineFront.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMagazineFront.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IWebHostEnvironment env;

        public ProductController(HttpClient httpClient, IWebHostEnvironment env)
        {
            this.httpClient = httpClient;
            this.env = env;
        }

        public async Task<IActionResult> SingleProduct(Guid productId)
        {
            var response = await httpClient.GetAsync($"http://localhost:5123/api/product/{productId}");

            if (response != null && response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(apiResponse);

                return View(product);
            }
            else
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
            }
        }

        public async Task<IActionResult> ProductList()
        {
            var response = await httpClient.GetStringAsync("http://localhost:5123/api/product/all");

            var products = JsonConvert.DeserializeObject<List<Product>>(response);

            return View(products.ToList());
        }

        public async Task<IActionResult> AddProduct()
        {
            var response = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");

            var categories = JsonConvert.DeserializeObject<List<Category>>(response);

            return View(new ProductViewModel
            {
                Categories = [.. categories],
            });
        }

        public async Task<IActionResult> EditProduct(Guid productId)
        {
            var response = await httpClient.GetAsync($"http://localhost:5123/api/product/{productId}");
            var responseCategories = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");

            if (response != null && response.IsSuccessStatusCode && responseCategories != null)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(apiResponse);
                var categories = JsonConvert.DeserializeObject<List<Category>>(responseCategories);
                return View(new ProductViewModel
                {
                    Categories = [.. categories],
                    Product = product
                });
            }
            else
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile? uploadedFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedFile == null || uploadedFile.Length == 0)
                    {
                        ModelState.AddModelError("Product.ImageUrl", "Выберите изображение");

                        var responseCategories = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");
                        var categories = JsonConvert.DeserializeObject<List<Category>>(responseCategories);

                        return View(new ProductViewModel
                        {
                            Categories = [.. categories],
                            Product = product
                        });
                    }

                    string path = $"/img/{uploadedFile.FileName}";
                    product.ImageUrl = path;

                    using var filestream = new FileStream(env.WebRootPath + path, FileMode.Create);
                    await uploadedFile.CopyToAsync(filestream);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ImageUrl", ex.Message);

                    var responseCategories = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");
                    var categories = JsonConvert.DeserializeObject<List<Category>>(responseCategories);

                    return View(new ProductViewModel
                    {
                        Categories = [.. categories],
                        Product = product
                    });
                }

                var jsonDto = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonDto, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5123/api/product", content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    return RedirectToRoute(new { controller = "Product", action = "ProductList" });
                }
                else
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                    foreach (var error in errorResponse)
                    {

                        if (error.Key == "message")
                        {
                            ModelState.AddModelError("", error.Value);
                        }
                        else
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }

                    return View(product);
                }

            }
            else
            {
                var responseCategories = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");
                var categories = JsonConvert.DeserializeObject<List<Category>>(responseCategories);

                return View(new ProductViewModel
                {
                    Categories = [.. categories],
                    Product = product
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product, IFormFile? uploadedFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedFile == null || uploadedFile.Length == 0)
                    {
                        ModelState.AddModelError("Product.ImageUrl", "Выберите изображение");

                        var responseCategories = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");
                        var categories = JsonConvert.DeserializeObject<List<Category>>(responseCategories);

                        return View(new ProductViewModel
                        {
                            Categories = [.. categories],
                            Product = product
                        });
                    }

                    string path = $"/img/{uploadedFile.FileName}";
                    product.ImageUrl = path;

                    using var filestream = new FileStream(env.WebRootPath + path, FileMode.Create);
                    await uploadedFile.CopyToAsync(filestream);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ImageUrl", ex.Message);
                    var responseCategories = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");
                    var categories = JsonConvert.DeserializeObject<List<Category>>(responseCategories);

                    return View(new ProductViewModel
                    {
                        Categories = [.. categories],
                        Product = product
                    });
                }

                var jsonDto = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonDto, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync("http://localhost:5123/api/product", content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    return RedirectToRoute(new { controller = "Product", action = "ProductList" });
                }
                else
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                    foreach (var error in errorResponse)
                    {

                        if (error.Key == "message")
                        {
                            ModelState.AddModelError("", error.Value);
                        }
                        else
                        {
                            ModelState.AddModelError(error.Key, error.Value);
                        }
                    }

                    return View(product);
                }
            }
            else
            {
                var responseCategories = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");
                var categories = JsonConvert.DeserializeObject<List<Category>>(responseCategories);

                return View(new ProductViewModel
                {
                    Categories = [.. categories],
                    Product = product
                });
            }
        }

        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5123/api/product/{productId}");

            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToRoute(new { controller = "Product", action = "ProductList" });
            }
            else
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
            }
        }
    }
}
