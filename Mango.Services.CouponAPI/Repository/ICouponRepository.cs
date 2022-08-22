using Mango.Services.CouponAPI.Models.DTO;

namespace Mango.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCode(string couponCode);
        // burda kupon kodu'nun uygulanmasını ve kaldırılmasını kontrol etmiyoruz çünkü bunun 
        // sorumluluğu ShoppingCartAPI 
        // bu Coupon servisinin amacı kuponları yönetmek 
    }


}
