using AutoMagazine.Data;
using AutoMagazine.Models;
using AutoMagazine.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AutoMagazine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public AuthController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpPost("login")]
        public IActionResult ApiLogin(LoginUserDto dto)
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

            User? existUser = db.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (existUser == null)
            {
                return NotFound(new { message = "Эта почта не зарегистрирована" });
            }

            if (existUser.Password != dto.Password)
            {
                return Unauthorized(new { message = "Неверный пароль" });
            }

            return Ok(existUser.Id);
        }

        [HttpPost("register")]
        public IActionResult ApiRegister(User user)
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

            User? existUser = db.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existUser != null)
            {
                return Conflict(new { message = "Эта почта уже зарегистрирована" });
            }

            db.Users.Add(user);
            db.SaveChanges();

            User newUser = db.Users.First(u => u.Email == user.Email);

            return Ok(newUser.Id);
        }
    }
}
