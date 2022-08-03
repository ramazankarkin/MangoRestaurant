using AutoMapper;
using Mongo.Services.ProductAPI.Models;
using Mongo.Services.ProductAPI.Models.DTO;

namespace Mongo.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>();
                config.CreateMap<Product, ProductDTO>();

            });
            return mappingConfig;
        }
    }
}
