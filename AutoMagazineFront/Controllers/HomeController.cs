using System.Diagnostics;
using System.Net.Http;
using AutoMagazine.Data;
using AutoMagazine.Models.Entities;
using AutoMagazineFront.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMagazineFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly int pageSize = 6;

        private readonly HttpClient httpClient;

        public HomeController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int page = 1, string catId = "0")
        {
            var response = await httpClient.GetStringAsync("http://localhost:5123/api/product/all");

            var categories = JsonConvert.DeserializeObject<List<Category>>(response);

            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(response);
            products = products
                .Where(p => catId == "0" || p.CategoryId == Guid.Parse(catId))
                .OrderBy(p => p.Id)
                .ToList(); 


            return View(
                new HomeIndexViewModel
                {
                    Title = "Все автомобили",
                    Products = products.Skip((page - 1) * pageSize).Take(pageSize),
                    CurrentCategoryId = catId == "0" ? null : Guid.Parse(catId),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = products.Count()
                    }
                });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? errorMessage = null)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorMessage = errorMessage });
        }
    }
}
