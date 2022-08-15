using Mango.Services.Identity;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Initializer;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
//Üstteki Token provider kullanýcý þifresini unuttuðu zaman, token üretmek için kullanýlacak.
//Projeye identity eklemiþ olduk ama identity server henüz eklemedik.

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.EmitStaticAudienceClaim = true; // development sürecinde olduðumuz için 
}).AddInMemoryIdentityResources(SD.IdentityResources).AddInMemoryApiScopes(SD.ApiScopes)
.AddInMemoryClients(SD.Clients).AddAspNetIdentity<ApplicationUser>().AddDeveloperSigningCredential();

//AddDeveloperSigningCredential() -> Baþlangýç zamanýnda geçici anahtar materyali oluþturur. Bu geliþtirme senaryolarý içindir.
//Oluþturulan anahtar, varsayýlan olarak yerel dizinde kalýcý olacaktýr.

//builder.Services.AddDeveloperSigningCredential(); // otamatik olarak key üreticek sadece geliþtirme yapmak amacýyla

builder.Services.AddScoped<IDbInitializer, DbInitializer>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



var scope = app.Services.CreateScope();

var initializerService = scope.ServiceProvider.GetService<IDbInitializer>();

initializerService.Initialize();


/*
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
*/


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer(); // Identity server'ý pipeline'e ekledik.


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

