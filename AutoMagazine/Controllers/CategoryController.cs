using AutoMagazine.Data;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoMagazine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public CategoryController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult ApiGetCategoryById(Guid id)
        {
            Category? category = db.Categories.Find(id);

            if (category == null)
            {
                return BadRequest(new { message = "Категория не найдена" });
            }

            return Ok(category);
        }

        [HttpGet("all")]
        public IActionResult ApiGetAllCategories()
        {
            return Ok(db.Categories.ToList());
        }

        [HttpPost]
        public IActionResult ApiAddCategory(Category category)
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

            db.Categories.Add(category);
            db.SaveChanges();

            return Ok("Категория создана");
        }

        [HttpPut]
        public IActionResult ApiUpdateCategory(Category category)
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

            Category existCategory = db.Categories.Find(category.Id);

            if (existCategory == null)
            {
                return BadRequest(new { message = "Категория не найдена" });
            }
            db.Entry(existCategory).State = EntityState.Detached;
            db.Categories.Update(category);
            db.SaveChanges();

            return Ok("Категория обновлена");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult ApiDeleteCategory(Guid id)
        {
            Category? category = db.Categories.Find(id);

            if (category == null)
            {
                return BadRequest(new { message = "Такой категории не существует" });
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok("Категория удалена");
        }
    }
}
