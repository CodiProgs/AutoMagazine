using AutoMagazine.Data;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMagazine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult ApiGetProductById(Guid id)
        {
            Product? product = db.Products.Find(id);

            if (product == null)
            {
                return BadRequest(new { message = "Товар не найден" });
            }

            return Ok(product);
        }

        [HttpGet("all")]
        public IActionResult ApiGetAllProducts()
        {
            return Ok(db.Products.ToList());
        }

        [HttpPost]
        public IActionResult ApiAddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value?.Errors.First().ErrorMessage
                    );

                return BadRequest(errors);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return Ok("Продукт создан");
        }

        [HttpPut]
        public IActionResult ApiUpdateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value?.Errors.First().ErrorMessage
                    );

                return BadRequest(errors);
            }

            db.Products.Update(product);
            db.SaveChanges();

            return Ok("Продукт обновлен");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult ApiDeleteProduct(Guid id)
        {
            Product? product = db.Products.Find(id);

            if (product == null)
            {
                return BadRequest("Продукт не найден");
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok("Продукт удален");
        }
    }
}
