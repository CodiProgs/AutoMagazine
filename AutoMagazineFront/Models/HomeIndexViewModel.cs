using AutoMagazine.Models.Entities;

namespace AutoMagazineFront.Models
{
    public class HomeIndexViewModel
    {
        public required string Title { get; set; }
        public IEnumerable<Product> Products { get; set; } = [];
        public required PagingInfo PagingInfo { get; set; }
        public Guid? CurrentCategoryId { get; set; }
    }
}
