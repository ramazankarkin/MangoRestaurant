using Microsoft.EntityFrameworkCore;



namespace Mango.Services.ShoppingCartAPI.DbContexts
{


    public class ApplicationDbContext : DbContext // Entity Framework'u kullanabilmek icin implement ettik.

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //public DbSet<Product> Products { get; set; } // we will create table name of Products

    }


}
