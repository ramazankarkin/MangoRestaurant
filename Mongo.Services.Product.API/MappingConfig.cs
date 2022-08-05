 using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>();// ProductDTO classındaki property isimleri ile Product classındaki property isimleri aynı olduğu sürece otomatik olarak
                config.CreateMap<Product, ProductDTO>();// map işlemi yapılacak eğer property isimleri farklı olsaydı bunu tek tek elle birer birer map'lememiz gerekecekti. 
                                                        // Maplemek istediğimiz property isimleri aynı olduğu için tek tek yazmadık.

            });
            return mappingConfig;
        }
    }
}
