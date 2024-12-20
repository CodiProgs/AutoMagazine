using System.Text;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMagazineFront.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly HttpClient httpClient;

        public ShopCartController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var userId = HttpContext.Session.GetString("UserId")!;
                httpClient.DefaultRequestHeaders.Add("userId", userId);

                var response = await httpClient.GetAsync("http://localhost:5123/api/shopcart");

                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(apiResponse);

                    return View(cartItems);
                }
                else
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                    return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
                }

            }
            else
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "Вы не авторизованы" });
            }
        }

        public async Task<RedirectToActionResult> AddToCart(string productId)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var userId = HttpContext.Session.GetString("UserId")!;
                httpClient.DefaultRequestHeaders.Add("userId", userId);

                var response = await httpClient.GetAsync($"http://localhost:5123/api/shopcart/{productId}");

                if (response != null && response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                    return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
                }

            }
            else
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "Вы не авторизованы" });
            }
        }

        public async Task<RedirectToActionResult> RemoveFromCart(Guid itemCartId)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var userId = HttpContext.Session.GetString("UserId")!;
                httpClient.DefaultRequestHeaders.Add("userId", userId);

                var response = await httpClient.DeleteAsync($"http://localhost:5123/api/shopcart/{itemCartId}");

                if (response != null && response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);

                    return RedirectToAction("Error", "Home", new { errorMessage = errorResponse.First().Value });
                }

            }
            else
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "Вы не авторизованы" });
            }
        }
    }
}
