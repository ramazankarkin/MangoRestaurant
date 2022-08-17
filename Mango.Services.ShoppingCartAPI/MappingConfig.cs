using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() // static olduğu için direkt olarak Program.cs'den çalıştırabiliriz.
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>().ReverseMap();// ProductDTO classındaki property isimleri ile Product classındaki property isimleri aynı olduğu sürece otomatik olarak
                config.CreateMap<CartHeaderDTO, CartHeader>().ReverseMap();// map işlemi yapılacak eğer property isimleri farklı olsaydı bunu tek tek elle birer birer map'lememiz gerekecekti. 
                config.CreateMap<CartDetailsDTO, CartDetails>().ReverseMap();// Maplemek istediğimiz property isimleri aynı olduğu için tek tek yazmadık.
                config.CreateMap<CartDTO, Cart>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
