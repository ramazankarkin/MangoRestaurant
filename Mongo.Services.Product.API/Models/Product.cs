using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    public class Product
    {
        [Key]// Primary Key, Biz key annotation'ı eklemek bile Entity Framework productId yazdığımız için otomatik Primary key yapıcaktı.
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
