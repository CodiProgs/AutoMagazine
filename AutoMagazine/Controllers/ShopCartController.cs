using AutoMagazine.Data;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMagazine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopCartController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public ShopCartController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet()]
        public IActionResult ApiGetCartItems()
        {
            if (Request.Headers.TryGetValue("userId", out Microsoft.Extensions.Primitives.StringValues value))
            {
                var userId = value.ToString();

                User? user = db.Users.FirstOrDefault(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    return Unauthorized(new { message = "Пользователь не найден" });
                }

                List<CartItem> cartItems = [.. db.CartItems.Include(c => c.Product).Include(c => c.Cart!.User).Where(c => c.Cart!.UserId == Guid.Parse(userId))];
                return Ok(cartItems.ToList());
            }
            else
            {
                return Unauthorized(new { message = "Вы не авторизованы" });
            }
        }

        [HttpGet()]
        [Route("{id:guid}")]
        public IActionResult ApiAddToCart(Guid id)
        {
            if (Request.Headers.TryGetValue("userId", out Microsoft.Extensions.Primitives.StringValues value))
            {
                var userId = value.ToString();

                User? user = db.Users.FirstOrDefault(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    return Unauthorized(new { message = "Пользователь не найден" });
                }

                Product? product = db.Products.Find(id);

                if (product == null)
                {
                    return NotFound(new { message = "Продукт не найден" });
                }

                Cart? cart = db.Carts.Include(c => c.User).Include(c => c.CartItems).Where(c => c.UserId == Guid.Parse(userId)).FirstOrDefault();

                if (cart == null)
                {
                    Cart newCart = new()
                    {
                        UserId = Guid.Parse(userId)
                    };

                    db.Carts.Add(newCart);
                    db.SaveChanges();


                    cart = newCart;
                }

                CartItem cartItem = new()
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                };

                db.CartItems.Add(cartItem);
                db.SaveChanges();

                return Ok("Товар добавлен в корзину");
            }
            else
            {
                return Unauthorized(new { message = "Вы не авторизованы" });
            }
        }

        [HttpDelete()]
        [Route("{id:guid}")]
        public IActionResult ApiRemoveFromCart(Guid id)
        {
            if (Request.Headers.TryGetValue("userId", out Microsoft.Extensions.Primitives.StringValues value))
            {
                var userId = value.ToString();

                User? user = db.Users.FirstOrDefault(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    return Unauthorized(new { message = "Пользователь не найден" });
                }

                CartItem? cartItem = db.CartItems.Find(id);

                if (cartItem == null)
                {
                    return BadRequest(new { message = "Продукт не найден" });
                }

                db.CartItems.Remove(cartItem);
                db.SaveChanges();

                return Ok("Товар удален из корзины");
            }
            else
            {
                return Unauthorized(new { message = "Вы не авторизованы" });
            }
        }
    }
}
