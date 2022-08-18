﻿using Mango.WEB.Models;
using Mango.WEB.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {

            List<ProductDTO> list = new();
            var response = await _productService.GetAllProductsAsync<ResponseDTO>("");
            if (response != null && response.IsSUCCESS)
            {// DeserializeObject<List<ProductDTO>> burda ProductDTO listesi tipine convert ediyoruz.
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductDTO model = new();
            var response = await _productService.GetProductByIdAsync<ResponseDTO>(productId, "");
            if (response != null && response.IsSUCCESS)
            {// DeserializeObject<List<ProductDTO>> burda ProductDTO listesi tipine convert ediyoruz.
                model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            }
            return View(model);


        }
        [HttpPost]
        [ActionName("Details")] 
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductDTO productDTO)
        {
            CartDTO cartDTO = new()
            {
                CartHeader = new CartHeaderDTO
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };
            CartDetailsDTO cartDetails = new CartDetailsDTO()
            {
                Count = productDTO.Count,
                ProductId = productDTO.ProductId
            };

            var resp = await _productService.GetProductByIdAsync<ResponseDTO>(productDTO.ProductId, "");
            if(resp != null && resp.IsSUCCESS)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(resp.Result));
            }
            List<CartDetailsDTO> cartDetailsDTOs = new();
            cartDetailsDTOs.Add(cartDetails);
            cartDTO.CartDetails = cartDetailsDTOs;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResp = await _cartService.AddToCartAsync<ResponseDTO>(cartDTO, accessToken);
            if (addToCartResp != null && addToCartResp.IsSUCCESS)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDTO);

            //ProductDTO model = new();
            //var response = await _productService.GetProductByIdAsync<ResponseDTO>(productId, "");
            //if (response != null && response.IsSUCCESS)
            //{// DeserializeObject<List<ProductDTO>> burda ProductDTO listesi tipine convert ediyoruz.
            //    model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            //}
            //return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public async Task<IActionResult> Login()
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token"); // burdan debug yaparsak JWT token'e ulaşabiliriz.
                                                                               
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc"); // oidc -> openID connect
        }
    }
}