using Mango.Services.CouponAPI.Models.DTO;
using Mango.Services.CouponAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponAPIController : Controller
    {

        private readonly ICouponRepository _couponRepository;
        protected ResponseDTO _responseDTO;

        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet("{code}")]
        public async Task<object> GetDiscountForCode(string code)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponByCode(code);
                _responseDTO.Result = coupon;
            }
            catch (Exception ex)
            {

                _responseDTO.IsSUCCESS = false;
                _responseDTO.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDTO;
        }
       
    }
}
