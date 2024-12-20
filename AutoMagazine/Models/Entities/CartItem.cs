using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutoMagazine.Models.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public Guid CartId { get; set; }

        [JsonIgnore]
        public Cart? Cart { get; set; }

        public Guid ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
