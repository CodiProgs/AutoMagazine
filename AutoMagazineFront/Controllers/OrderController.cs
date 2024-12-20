using System.Net.Http;
using System.Text;
using AutoMagazine.Models;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMagazineFront.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient httpClient;

        public OrderController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> Checkout()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var userId = HttpContext.Session.GetString("UserId")!;
                httpClient.DefaultRequestHeaders.Add("userId", userId);

                var response = await httpClient.GetAsync("http://localhost:5123/api/order");

                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var createOrderResponse = JsonConvert.DeserializeObject<CreateOrderDto>(apiResponse);

                    return View(createOrderResponse);
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

        [HttpPost]
        public async Task<IActionResult> Checkout(CreateOrderDto dto)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var userId = HttpContext.Session.GetString("UserId")!;
                httpClient.DefaultRequestHeaders.Add("userId", userId);
                
                var jsonDto = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonDto, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5123/api/order", content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Complete");
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

        public IActionResult Complete()
        {
            ViewBag.Message = "Заказ обработан";
            return View();
        }
    }
}
