using Mango.WEB.Models;

namespace Mango.WEB.Services.IServices
{
    public interface ICartService
    {
        Task<T> GetCartByUserIdAsync<T>(string userId, string token = null);
        Task<T> AddToCartAsync<T>(CartDTO cartDTO, string token = null);
        Task<T> UpdateToCartAsync<T>(CartDTO cartDTO, string token = null);
        Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string token = null);
        Task<T> ApplyCoupon<T>(CartDTO cartDTO, string token = null);
        Task<T> RemoveCoupon<T>(string userId, string token = null);
        Task<T> Checkout<T>(CartHeaderDTO cartHeader, string token = null);







    }
}
