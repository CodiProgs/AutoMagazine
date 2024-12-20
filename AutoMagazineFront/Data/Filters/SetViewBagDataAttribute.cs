using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AutoMagazine.Models.Entities;
using Newtonsoft.Json;

namespace AutoMagazineFront.Data.Filters
{
    public class SetViewBagDataAttribute : ActionFilterAttribute
    {
        private readonly HttpClient httpClient;

        public SetViewBagDataAttribute(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = (Controller)context.Controller;

            if (controller.HttpContext.Session.GetString("UserId") != null)
            {
                controller.ViewBag.IsLoggedIn = true;
                var userId = Guid.Parse(controller.HttpContext.Session.GetString("UserId")!);

                try
                {
                    var response = await httpClient.GetStringAsync("http://localhost:5123/api/user/" + userId);

                    var user = JsonConvert.DeserializeObject<User>(response);

                    if (user != null)
                    {
                        controller.ViewBag.FullName = user.FullName;
                    }
                    else
                    {
                        controller.ViewBag.IsLoggedIn = false;
                    }
                }
                catch (Exception ex)
                {
                    controller.ViewBag.IsLoggedIn = false;
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                controller.ViewBag.IsLoggedIn = false;
            }

            base.OnActionExecuting(context);
        }

    }
}
