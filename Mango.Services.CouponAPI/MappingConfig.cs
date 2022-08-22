using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;

namespace Mango.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() // static olduğu için direkt olarak Program.cs'den çalıştırabiliriz.
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDTO, Coupon>().ReverseMap();// CouponDTO classındaki property isimleri ile Coupon classındaki property isimleri aynı olduğu sürece otomatik olarak
                //config.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();// map işlemi yapılacak eğer property isimleri farklı olsaydı bunu tek tek elle birer birer map'lememiz gerekecekti. 
                //config.CreateMap<CartDetailsDTO, CartDetails>().ReverseMap();// Maplemek istediğimiz property isimleri aynı olduğu için tek tek yazmadık.
                //config.CreateMap<CartDTO, Cart>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
