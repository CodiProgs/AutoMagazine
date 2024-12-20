using System.Net.Http;
using System.Threading.Tasks;
using AutoMagazine.Data;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMagazineFront.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly HttpClient httpClient;

        public NavigationMenuViewComponent(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SelectedCategory = RouteData?.Values["catId"];

            var response = await httpClient.GetStringAsync("http://localhost:5123/api/category/all");

            var categories = JsonConvert.DeserializeObject<List<Category>>(response);

            return View(categories);

        }
    }
}
