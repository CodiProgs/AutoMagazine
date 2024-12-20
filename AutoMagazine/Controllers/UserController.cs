using AutoMagazine.Data;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMagazine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult ApiGetUserById(Guid id)
        {
            User? user = db.Users.Find(id);

            if (user == null)
            {
                return BadRequest(new {message = "Пользователь не найден" });
            }

            return Ok(user);
        }
    }
}
