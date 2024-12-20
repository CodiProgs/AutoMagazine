using System.Text;
using AutoMagazine.Models;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoMagazineFront.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient httpClient;

        public AuthController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            if (ModelState.IsValid)
            {
                var jsonDto = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonDto, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5123/api/auth/login", content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<string>(apiResponse);


                    HttpContext.Session.SetString("UserId", loginResponse.ToString());

                    return RedirectToAction("Index", "Home");
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

                    return View(dto);
                }
            }
            else
            {
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var jsonUser = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5123/api/auth/register", content);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var registerResponse = JsonConvert.DeserializeObject<string>(apiResponse);


                    HttpContext.Session.SetString("UserId", registerResponse.ToString());

                    return RedirectToAction("Index", "Home");
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

                    return View(user);
                }
            }
            else
            {
                return View(user);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
