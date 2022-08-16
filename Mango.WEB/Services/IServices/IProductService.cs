
using Mango.WEB.Models;

namespace Mango.WEB.Services.IServices
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string token); // Task Async method olduğunu belirtiyor.
                                                      // Task'ın içindeki T dönüş tipini belirtiyor.
                                                      // GetAllProductsAsync method name
                                                      // GetAllProductsAsync<T> generic method
                                                      // 
        Task<T> GetProductByIdAsync<T>(int id, string token);
        Task<T> CreateProductAsync<T>(ProductDTO productDTO, string token);
        Task<T> UpdateProductAsync<T>(ProductDTO productDTO, string token);
        Task<T> DeleteProductAsync<T>(int id, string token);



    }
}
