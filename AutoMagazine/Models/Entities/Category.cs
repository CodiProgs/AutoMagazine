using System.ComponentModel.DataAnnotations;

namespace AutoMagazine.Models.Entities
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public required string Name { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
