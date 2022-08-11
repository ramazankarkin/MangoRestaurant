using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Identity.DbContexts
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // by default this uses identity user.
                                                                           //  ApplicationUser ekledik.
          
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


    }
}
