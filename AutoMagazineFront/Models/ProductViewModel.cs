using AutoMagazine.Models.Entities;

namespace AutoMagazineFront.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = null!;
        public List<Category> Categories { get; set; } = new();
    }
}
