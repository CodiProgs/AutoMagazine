using System.Text;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMagazineFront.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient httpClient;

        public CategoryController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> CategoryList()
        {
            var response = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");

            var categories = JsonConvert.DeserializeObject<List<Category>>(response);

            return View(categories.ToList());
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        public async Task<IActionResult> EditCategory(Guid categoryId)
        {
            var response = await httpClient.GetAsync($"http://localhost:5123/api/category/{categoryId}");

            if (response != null && response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var category = JsonConvert.DeserializeObject<Category>(apiResponse);

                return View(category);
            }
            else
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {

            if (ModelState.IsValid)
            {
                var jsonDto = JsonConvert.SerializeObject(category);
                var content = new StringContent(jsonDto, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5123/api/category", content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    return RedirectToRoute(new { controller = "Category", action = "CategoryList" });
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

                    return View(category);
                }
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {

            if (ModelState.IsValid)
            {
                var jsonDto = JsonConvert.SerializeObject(category);
                var content = new StringContent(jsonDto, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync("http://localhost:5123/api/category", content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    return RedirectToRoute(new { controller = "Category", action = "CategoryList" });
                }
                else
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                    if (errorResponse.First().Key == "message")
                    {
                        return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
                    }
                    
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

                    return View(category);
                }
            }
            else
            {
                return View(category);
            }
        }

        public async Task<IActionResult> DeleteCategory(Guid categoryId)
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5123/api/category/{categoryId}");

            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToRoute(new { controller = "Category", action = "CategoryList" });
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
