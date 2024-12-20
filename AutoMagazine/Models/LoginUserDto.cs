using System.ComponentModel.DataAnnotations;

namespace AutoMagazine.Models
{
    public class LoginUserDto
    {
        [Display(Name = "Адрес электронной почты")]
        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Неверный адрес электронной почты")]
        public required string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Обязательное поле")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        [DataType(DataType.Password)] // хелперы создают элемент ввода, у которого атрибут type имеет значение "password"
        public required string Password { get; set; }
    }
}
