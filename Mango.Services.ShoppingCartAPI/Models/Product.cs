using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]// ProductId primary key ama otomatik olarak üretilmeyecek kendimiz değer atıcaz.
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, 2000)]
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
    }
}
