using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDTO _responseDTO;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _responseDTO = new ResponseDTO();
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
        public async Task<object> AddGetCart(CartDTO cartDTO)
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
    }
}
