using Mango.Services.ProductAPI.Models.DTO;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IProductRepository
    {
        
        Task<IEnumerable<ProductDTO>> GetProducts(); // Bir şeyler return etmek veya okumak için sadece Dto'ları kullanmak istiyoruz.
                                                     // Veritabanında işlem yapmak için Productdto'yu product entity(product class)  ile map'leyip kullanıcaz.
        Task<ProductDTO> GetProductById(int productId);
        Task<ProductDTO> CreateUpdateProduct(ProductDTO productDTO);
        Task<bool> DeleteProduct(int productId);
    }
}
