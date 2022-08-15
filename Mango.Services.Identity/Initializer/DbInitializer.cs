using IdentityModel;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        { 
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            if(_roleManager.FindByNameAsync(SD.Admin).Result == null) // Database'de henüz rol tanımlanmamış. yani uygulamayı ilk defa çalıştırıyoruz.  
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult(); // rol atıyoruz. Async method olduğu için GetAwaiter().GetResult() eklememiz lazım
                                                                                               // Eğer eklemezsek çalışmaz çünkü diğer satıra geçmeden Rol atamasını yapana kadar beklememiz lazım.
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else { return; } // Eğer roller zaten atanmışsa birşey yapma.

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "05301234567",
                FirstName = "Ramazan",
                LastName = "Karkin"

            };


            _userManager.CreateAsync(adminUser, "Admin123!").GetAwaiter().GetResult(); // User oluşturduk 
            _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult(); // User'a admin rolünü verdik.

            var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[] //Burda veritabanındaki AspNetRoleClaims tablosu, belirli bir role atanmış talepleri tutuyor.
             {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName + " " + adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, SD.Admin),



             }).Result;
            //--------------------------------------------------------------------
            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "05301234567",
                FirstName = "Enes",
                LastName = "Senol"

            };


            _userManager.CreateAsync(customerUser, "Cus123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();

            var temp2 = _userManager.AddClaimsAsync(adminUser, new Claim[]
             {
                new Claim(JwtClaimTypes.Name, customerUser.FirstName + " " + customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role, SD.Customer),



             }).Result;
        }
    }
}
