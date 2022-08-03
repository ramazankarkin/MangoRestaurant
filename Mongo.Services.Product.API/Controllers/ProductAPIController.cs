using Microsoft.AspNetCore.Mvc;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Repository;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDTO _response;
        private IProductRepository _productRepository;

        public ProductAPIController(IProductRepository productRepository)
        {
            
            _productRepository = productRepository;
            this._response = new ResponseDTO();
        }

        public IActionResult Index()
        {
        }
    }
}
