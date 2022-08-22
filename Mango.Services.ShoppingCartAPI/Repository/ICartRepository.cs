﻿using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
       Task<CartDTO> GetCartByUserId(string userId);
       Task<CartDTO> CreateUpdateCart(CartDTO cartDTO);

        Task<bool> RemoveFromCart(int cartDetailsId); // ürünü Kart'tan kaldırmak için

        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);

        Task<bool> ClearCart(string userId); // kullanıcıdan kartı kaldırmak için


    }
}
