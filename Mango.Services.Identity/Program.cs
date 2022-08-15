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
//�stteki Token provider kullan�c� �ifresini unuttu�u zaman, token �retmek i�in kullan�lacak.
//Projeye identity eklemi� olduk ama identity server hen�z eklemedik.

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.EmitStaticAudienceClaim = true; // development s�recinde oldu�umuz i�in 
}).AddInMemoryIdentityResources(SD.IdentityResources).AddInMemoryApiScopes(SD.ApiScopes)
.AddInMemoryClients(SD.Clients).AddAspNetIdentity<ApplicationUser>().AddDeveloperSigningCredential();

//AddDeveloperSigningCredential() -> Ba�lang�� zaman�nda ge�ici anahtar materyali olu�turur. Bu geli�tirme senaryolar� i�indir.
//Olu�turulan anahtar, varsay�lan olarak yerel dizinde kal�c� olacakt�r.

//builder.Services.AddDeveloperSigningCredential(); // otamatik olarak key �reticek sadece geli�tirme yapmak amac�yla

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

app.UseIdentityServer(); // Identity server'� pipeline'e ekledik.


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

