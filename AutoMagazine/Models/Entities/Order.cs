using System.ComponentModel.DataAnnotations;

namespace AutoMagazine.Models.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime CreatedDate { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше нуля")]
        public decimal TotalAmount { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; }

        [MinLength(1, ErrorMessage = "Заказ должен содержать хотя бы один элемент")]
        public List<OrderItem>? Items { get; set; }
    }
}
