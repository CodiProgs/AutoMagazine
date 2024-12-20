using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AutoMagazine.Models.Entities
{
    public class User
    {
        [BindNever]
        public Guid Id { get; set; }

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Обязательное поле")]
        public required string FullName { get; set; }

        [Display(Name = "Адрес электронной почты")]
        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Неверный адрес электронной почты")]
        public required string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Обязательное поле")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Обязательное поле")]
        [Phone(ErrorMessage = "Неверный номер телефона")]
        public required string Phone { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Обязательное поле")]
        public required string Address { get; set; }
    }
}
