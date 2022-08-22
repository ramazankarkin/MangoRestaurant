using Mango.WEB.Models;

namespace Mango.WEB.Services.IServices
{
    public interface ICouponService
    {
       
        Task<T> GetCoupon<T>(string couponCode, string token = null);

    }
}
