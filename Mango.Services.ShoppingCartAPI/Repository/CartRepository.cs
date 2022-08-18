using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db; 
        private IMapper _mapper; // ProductDto'yu product'a çevirmek için gerekli.

        public CartRepository(ApplicationDbContext db, IMapper mapper) // Dependency injection'a örnek.
        {
            _db = db;
            _mapper = mapper;
        }



        public async Task<bool> ClearCart(string userId)
        {
            // Eğer girilen kullanıcı id'ye karşılık gelen cartHeader satırı varsa
            // o cartHeader satırına karşılık gelen CartDetail classındaki (birden fazla olabilir)
            // cartDetails satırlarını(objelerini) siliyoruz.
            // o cartHeader satırını(objesini) CartHeader classından siliyoruz.
            // böylece Cart Class'ının sahip olduğu CartHeader ve CartDetail bilgisini silerek
            // Clear Cart yapmış olduk.


            var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);// Databaseden girilen userid'ye karşılık gelen Header objesini çekiyoruz.
            if(cartHeaderFromDb != null) // Eğer girilen kullanıcı id'ye karşılık gelen cartHeader satırı varsa

            {   // o cartHeader satırına karşılık gelen CartDetail classındaki (birden fazla olabilir)
                // cartDetails satırlarını(objelerini) siliyoruz.
                _db.CartDetails
                    .RemoveRange(_db.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CardHeaderId));
                // o cartHeader satırını(objesini) CartHeader classından siliyoruz.
                _db.CartHeaders.Remove(cartHeaderFromDb);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartDTO> CreateUpdateCart(CartDTO cartDTO)
        { // check if product exists in database, if not create it!
          // check if header is null, create header and details
          // if header is not null, check if details has same product
          // if it has then update the count else create details.

            Cart cart = _mapper.Map<Cart>(cartDTO); // cartDTO objesini cart objesine convert ediyoruz.
            // check if product exists in database, if not create it!
            var prodInDb = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == cartDTO.CartDetails.FirstOrDefault().ProductId);
            if(prodInDb == null)
            { // if not create it!
                _db.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _db.SaveChangesAsync();
            }
            // check if header is null
            var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
            // check if header is null
            if(cartHeaderFromDb == null)
            {
                //create header and details
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CardHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null; // burayı null yapmamızın sebebi yukardaki statement'da
                                                                  // zaten Product ekliyoruz. Aşağıdaki statement da aynı productId'li produtct'ı eklemeye çalışıyoruz.
                                                                  // CartDetail da Product olduğu için oda eklemeye yapmaya çalışçak ve hata verricek.
                                                                  // hata almamak için onu null yapıyoruz. Bu kısmı tam anlamadım tekrar kontrol et.
                _db.CartDetails.Add(cart.CartDetails.FirstOrDefault()); 
                await _db.SaveChangesAsync();
            }
            else
            {   // if header is not null, check if details has same product
                var cartDetailsFromDb =await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u =>u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CardHeaderId);

                // if it has then update the count else create details.

                // else create details
                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CardHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
                //if it has then update the count
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    _db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }


            }

            return _mapper.Map<CartDTO>(cart); // Cart objesini CartDTO objesine convert ediyoruz. 
        }

        public async Task<CartDTO> GetCartByUserId(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await _db.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId)
            };
            
            cart.CartDetails = _db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CardHeaderId).Include(u => u.Product);
            // Include(u => u.Product) bu include ile beraber CartDetails içindeki Product bilgisini de çekiyoruz.
            // cart objemiz artık bu product bilgilerine ulaşabilir.
            return _mapper.Map<CartDTO>(cart); // cart objesini cartDTO objesine convert ediyoruz.
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                //Girilen cartDetailsId ile database cartDetails tablosundaki cartDetailsId aynı olan objeyi buluyoruz.
                CartDetails cartDetails = await _db.CartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);

                //cartDetail id'si bilinen objenin sahip olduğu cartHeaderId'nin tüm CartDetails tablosundaki sayısını buluyoruz.
                int totalCountOfCartItems = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails); // database'den siliyoruz bulduğumuz objeyi.
                if (totalCountOfCartItems == 1)//  totalCountOfCartItems = 1 olması bize şunu gösterir. cardHeaderId sadece sildiğimiz
                                               //  cartDetail objemizde varmış diğer cartDetail objelerinde aynı cardHeaderId yokmuş.
                {
                    var cartHeaderToRemove = await _db.CartHeaders.FirstOrDefaultAsync(u => u.CardHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove); // burda sadece sildiğimiz cartDetails de bulunan cartHeaders'ı database'den sildik. 
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
            
        }
    }
}
