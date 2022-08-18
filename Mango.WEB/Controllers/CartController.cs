﻿using Mango.WEB.Models;
using Mango.WEB.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.WEB.Controllers
{
    
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDTOBasedOnLoggedInUser());
        }
        private async Task<CartDTO> LoadCartDTOBasedOnLoggedInUser() 
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDTO>(userId, accessToken);

            CartDTO cartDTO = new();
            if(response != null && response.IsSUCCESS)
            {
                cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
            }
            if(cartDTO.CartHeader != null)
            {
                foreach (var detail in cartDTO.CartDetails)
                {
                    cartDTO.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }
                
            }
            return cartDTO;
        }
    }
}
