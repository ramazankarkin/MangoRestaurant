using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db; //
        private IMapper _mapper; // ProductDto'yu product'a çevirmek için gerekli.

        public ProductRepository(ApplicationDbContext db, IMapper mapper) // Dependency injection'a örnek.
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateUpdateProduct(ProductDTO productDTO)
        {
            Product product = _mapper.Map<ProductDTO, Product>(productDTO); // ProductDTO yu product'a dönüştürüp product'a atıyoruz.
            if(product.ProductId > 0) // update veya create olduğunu anlamak için koyduk.
            {
                _db.Products.Update(product); // Burdaki Update methodu Entity Framework'den geliyor biz yazmadık bu methodu.
            }
            else
            {
                _db.Products.Add(product); // Burdaki Add methodu Entity Framework'den geliyor biz yazmadık bu methodu.
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductDTO > (product); // Tekrar Product'u ProductDTO'ya dönüştürüyoruz.
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
                if(product == null) // Eğer yukardaki id'ye uygun bir product yoksa null kontrolü yapıyoruz.
                {
                    return false;
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync(); // Database'de değişiklik yaptığımız için ekledik. 
                return true;                  // Okuma işlemi yaptığımızda değişiklik yapmadığımız için 
            }                                 // SaveChangesAsync() yok orda.
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<ProductDTO> GetProductById(int productId)
        {
            Product product = await _db.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync(); // id için sadece bir ürün olduğu için FirstOrDefault dedik.

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            IEnumerable<Product> productList = await _db.Products.ToListAsync(); // Database'deki Products table'ından tüm productları liste şeklinde alıyoruz.

            return _mapper.Map <List<ProductDTO >>(productList); // Product Listesini tutan productList'i  PrdoctDTO'ya dönüştürüp return ediyoruz.
        }
    }
}
 