using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AutoMagazine.Models.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Обязательное поле")]
        public required string Name { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Обязательное поле")]
        public required string Description { get; set; }

        [Display(Name = "Цена")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше нуля")]
        public decimal Price { get; set; }

        [Display(Name = "Картинка")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Категория")]
        public Guid CategoryId { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]//указывается для системных полей, которые не отображаются
        public Category? Category { get; set; }
    }
}
