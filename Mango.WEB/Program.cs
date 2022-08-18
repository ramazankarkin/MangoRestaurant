using Mango.WEB;
using Mango.WEB.Services;
using Mango.WEB.Services.IServices;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//--------------------------------------------------------------------------------
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();

SD.ProductAPIBase = builder.Configuration["ServiceURLs:ProductAPI"];
SD.ShoppingCartAPIBase = builder.Configuration["ServiceURLs:ShoppingCartAPI"];

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";

}).AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10)).AddOpenIdConnect("oidc", options =>
{ // eklediðimiz tüm optionlar openID connection içindi bu eklediðimiz verileri SD klasýndan aldýk.
    options.Authority = builder.Configuration["ServiceURLs:IdentityAPI"];
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClientId = "mango";
    options.ClientSecret = "secret"; // normal þartlarda random guid olmasý lazým,
    options.ClaimActions.MapJsonKey("role","role","role");
    options.ClaimActions.MapJsonKey("sub", "sub", "sub");
    options.ClaimActions.MapJsonKey("role", "role", "role");

    options.ResponseType = "code";
    options.TokenValidationParameters.NameClaimType = "name";
    options.TokenValidationParameters.RoleClaimType = "role";
    options.Scope.Add("mango");
    options.SaveTokens = true;
});

//--------------------------------------------------------------------------------
var app = builder.Build();

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
app.UseAuthentication(); // authentication authorization'dan önce gelmeli her zaman.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
