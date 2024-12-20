using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutoMagazine.Models.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User? User { get; set; }

        [JsonIgnore]
        public List<CartItem>? CartItems { get; set; } = new();
    }
}
