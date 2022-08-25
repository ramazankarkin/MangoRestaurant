using Mango.WEB.Models;
using Mango.WEB.Services.IServices;

namespace Mango.WEB.Services
{
    public class CartService : BaseService,ICartService
    {
        private readonly IHttpClientFactory _clientFactory;

        public CartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> AddToCartAsync<T>(CartDTO cartDTO, string token = null)
        {
            return await SendAsync<T>(new ApiRequest() //Send an HTTP request as an asynchronous operation.
                                                            // Burdaki T generic type aslında ResponseDTO'dur.
            {
                APIType = SD.APIType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDTO cartDTO, string token = null)
        {
            return await SendAsync<T>(new ApiRequest() //Send an HTTP request as an asynchronous operation.
                                                       // Burdaki T generic type aslında ResponseDTO'dur.
            {
                APIType = SD.APIType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDTO cartHeader, string token = null)
        {
            return await SendAsync<T>(new ApiRequest() //Send an HTTP request as an asynchronous operation.
                                                       // Burdaki T generic type aslında ResponseDTO'dur.
            {
                APIType = SD.APIType.POST,
                Data = cartHeader,
                Url = SD.ShoppingCartAPIBase + "/api/cart/Checkout",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest()

            {
                APIType = SD.APIType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId,
                AccessToken = token
            });
        }

        public async Task<T> RemoveCoupon<T>(string userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest() //Send an HTTP request as an asynchronous operation.
                                                       // Burdaki T generic type aslında ResponseDTO'dur.
            {
                APIType = SD.APIType.POST,
                Data = userId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest() //Send an HTTP request as an asynchronous operation.
                                                            // Burdaki T generic type aslında ResponseDTO'dur.
            {
                APIType = SD.APIType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateToCartAsync<T>(CartDTO cartDTO, string token = null)
        {
            return await SendAsync<T>(new ApiRequest() //Send an HTTP request as an asynchronous operation.
                                                       // Burdaki T generic type aslında ResponseDTO'dur.
            {
                APIType = SD.APIType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/api/cart/UpdateCart",
                AccessToken = token
            });
        }
    }
}
