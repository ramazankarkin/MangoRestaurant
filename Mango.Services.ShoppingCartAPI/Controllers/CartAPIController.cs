using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Messages;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartAPIController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMessageBus _messageBus;

        protected ResponseDTO _responseDTO;


        public CartAPIController(ICartRepository cartRepository, IMessageBus messageBus)
        {
            _cartRepository = cartRepository;
            _responseDTO = new ResponseDTO();
            _messageBus = messageBus;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDTO cartDTO = await _cartRepository.GetCartByUserId(userId);
                _responseDTO.Result = cartDTO;
            }
            catch (Exception ex)
            {

                _responseDTO.IsSUCCESS = false;
                _responseDTO.ErrorMessages = new List<string>() { ex.ToString() }; 
            }

            return _responseDTO;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDTO cartDTO)
        {
            try
            {   // burdaki async - await kullanımı gerçek kullanımdan farklıdır.
                // aşağıdaki await kullanımında -> Thread (line 47'de) beklesin ne zamana kadar?
                // CreateUpdateCart(cartDTO) methodundan gelen cevabın dönmesine kadar.
                // Cevap gelene kadar diğer satıra geçmeyecek.
                CartDTO cartDT = await _cartRepository.CreateUpdateCart(cartDTO);
                _responseDTO.Result = cartDT;
            }
            catch (Exception ex)
            {

                _responseDTO.IsSUCCESS = false;
                _responseDTO.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDTO;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDTO cartDTO)
        {
            try
            {
                CartDTO cartDT = await _cartRepository.CreateUpdateCart(cartDTO);
                _responseDTO.Result = cartDT;
            }
            catch (Exception ex)
            {

                _responseDTO.IsSUCCESS = false;
                _responseDTO.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDTO;
        }


        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody]int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
                _responseDTO.Result = isSuccess;
            }
            catch (Exception ex)
            {

                _responseDTO.IsSUCCESS = false;
                _responseDTO.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDTO;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                bool isSuccess = await _cartRepository.ApplyCoupon(cartDTO.CartHeader.UserId, cartDTO.CartHeader.CouponCode);
                _responseDTO.Result = isSuccess;
            }
            catch (Exception ex)
            {

                _responseDTO.IsSUCCESS = false;
                _responseDTO.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDTO;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveCoupon(userId);
                _responseDTO.Result = isSuccess;
            }
            catch (Exception ex)
            {

                _responseDTO.IsSUCCESS = false;
                _responseDTO.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _responseDTO;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDTO checkoutHeader)
        {
            try
            {
                CartDTO cartDTO =await _cartRepository.GetCartByUserId(checkoutHeader.UserId);// burası userId'ye göre cartDTO dönüyor
                if(cartDTO == null)
                {
                    return BadRequest();
                }
                checkoutHeader.CartDetails = cartDTO.CartDetails; // burda CartDetails'i dolduruyoruz.
                // logic to add message to process order.

                await _messageBus.PublishMessage(checkoutHeader, "checkoutmessagetopic");
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
