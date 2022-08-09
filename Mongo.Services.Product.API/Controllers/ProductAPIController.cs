﻿using Microsoft.AspNetCore.Mvc;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Repository;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDTO _response; // Generic'le çalışmak istemediğimiz için ResponseDTO classını oluşturduk.
        private IProductRepository _productRepository; 

        public ProductAPIController(IProductRepository productRepository)
        {
            
            _productRepository = productRepository;
            this._response = new ResponseDTO(); 
        }

        [HttpGet]
        public async Task<Object/* ResponseDTO*/> Get() // burda Task içine yazdığımız şey bizim dönüş tipimizi belirtiyor.
        {
            try
            {
                IEnumerable<ProductDTO> productDtos = await _productRepository.GetProducts();
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {

                _response.IsSUCCESS = false;
                _response.ErrorMessages = new List<string> { ex.ToString() }; // Error mesajını log'luyoruz.
            }
            return _response;
        }

        [HttpGet]
        [Route("{id}")] // Eğer burdaki Route kaldırırsak Request product için bu class'a geldiği zaman hangi
                        // [HttpGet] methoduna gideceğini bilemediği için error vericek.
                        // ikinci yani bu method'un id beklediğini [Route("{id}")] sayesinde anlıyor.
                        // aşağıdaki method'un parametresine bakarak anlayamıyor.
                        // O yüzden [Route("{id}")] eklemeksek hangi methoda gideceğini anlamayacağı için error vericek.
        public async Task<Object/* ResponseDTO*/> Get(int id) // burda Task içine yazdığımız şey bizim dönüş tipimizi belirtiyor.
        {
            try
            {
                ProductDTO productDtos = await _productRepository.GetProductById(id);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {

                _response.IsSUCCESS = false;
                _response.ErrorMessages = new List<string> { ex.ToString() }; // Error mesajını log'luyoruz.
            }
            return _response;
        }
    }
}
