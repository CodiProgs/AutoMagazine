using System.ComponentModel.DataAnnotations;

namespace AutoMagazine.Models.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше нуля")]
        public decimal Price { get; set; } // Цена товара на момент заказа

        public Guid OrderId { get; set; }

        public Order? Order { get; set; }

        public Guid ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
