using Microsoft.AspNetCore.Mvc;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize] // Sisteme giriş yapan kişiler bu istekleri yapabilecek artık.
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
        [Authorize]
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

        [HttpPost]
        [Authorize]

        public async Task<Object/* ResponseDTO*/> Post([FromBody] ProductDTO productDTO) // burda Task içine yazdığımız şey bizim dönüş tipimizi belirtiyor.
        {
            try
            {
                ProductDTO model = await _productRepository.CreateUpdateProduct(productDTO);
                _response.Result = model;
            }
            catch (Exception ex)
            {

                _response.IsSUCCESS = false;
                _response.ErrorMessages = new List<string> { ex.ToString() }; // Error mesajını log'luyoruz.
            }
            return _response;
        }

        [HttpPut]
        [Authorize]

        public async Task<Object/* ResponseDTO*/> Put([FromBody] ProductDTO productDTO) // burda Task içine yazdığımız şey bizim dönüş tipimizi belirtiyor.
        {
            try
            {
                ProductDTO model = await _productRepository.CreateUpdateProduct(productDTO);
                _response.Result = model;
            }
            catch (Exception ex)
            {

                _response.IsSUCCESS = false;
                _response.ErrorMessages = new List<string> { ex.ToString() }; // Error mesajını log'luyoruz.
            }
            return _response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")] // Silme işlemi sadece Admin için tanımlı
        [Route("{id}")]

        public async Task<Object/* ResponseDTO*/> Delete(int id) // burda Task içine yazdığımız şey bizim dönüş tipimizi belirtiyor.
        {
            try
            {
                bool isSuccess = await _productRepository.DeleteProduct(id);
                _response.Result = isSuccess;
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
