namespace AutoMagazine.Models
{
    public class CreateOrderDto
    {
        public decimal TotalAmount { get; set; }
        public int CountItem { get; set; }
    }
}
