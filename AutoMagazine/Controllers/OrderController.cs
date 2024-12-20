using AutoMagazine.Data;
using AutoMagazine.Models;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMagazine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public OrderController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult ApiGetOrder()
        {
            if (Request.Headers.TryGetValue("userId", out Microsoft.Extensions.Primitives.StringValues value))
            {
                var userId = value.ToString();

                User? user = db.Users.FirstOrDefault(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    return Unauthorized(new { message = "Пользователь не найден" });
                }

                List<CartItem> cartItems = [.. db.CartItems.Include(c => c.Product).Include(c => c.Cart).Where(c => c.Cart!.UserId == Guid.Parse(userId))];

                if (cartItems.Count == 0)
                {
                    return BadRequest(new { message = "Ваша корзина пуста" });
                }

                decimal totalAmount = 0;

                foreach (CartItem cartItem in cartItems)
                {
                    totalAmount += cartItem.Product!.Price;
                }

                return Ok(new CreateOrderDto
                {
                    CountItem = cartItems.Count,
                    TotalAmount = totalAmount
                });
                }
            else
            {
                return BadRequest(new { message = "Вы не авторизованы" });
            }
        }

        [HttpPost]
        public IActionResult ApiCreateOrder(CreateOrderDto dto)
        {
            if (Request.Headers.TryGetValue("userId", out Microsoft.Extensions.Primitives.StringValues value))
            {
                var userId = value.ToString();

                User? user = db.Users.FirstOrDefault(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    return Unauthorized(new { message = "Пользователь не найден" });
                }

                List<CartItem> cartItems = [.. db.CartItems.Include(c => c.Product).Include(c => c.Cart).Where(c => c.Cart!.UserId == Guid.Parse(userId))];

                if (cartItems.Count == 0)
                {
                    return BadRequest(new { message = "Ваша корзина пуста" });
                }

                Order order = new()
                {
                    TotalAmount = dto.TotalAmount
                };

                order.CreatedDate = DateTime.Now;
                order.UserId = user.Id;

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (CartItem el in cartItems)
                {
                    var orderItem = new OrderItem()
                    {
                        OrderId = order.Id,
                        Price = el.Product!.Price,
                        ProductId = el.ProductId,
                    };
                    db.CartItems.Remove(el); // удаляем товары из корзины, после создания заказа
                    db.OrderItems.Add(orderItem);
                }

                db.SaveChanges();

                return Ok("Заказ создан");
            }
            else
            {
                return BadRequest(new { message = "Вы не авторизованы" });
            }
        } 

    }
}
