using Mango.Services.ProductAPI;
using Mango.WEB.Services;
using Mango.WEB.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//--------------------------------------------------------------------------------
builder.Services.AddHttpClient<IProductService, ProductService>();
SD.ProductAPIBase = builder.Configuration["ServiceURLs:ProductAPI"];
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";

}).AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10)).AddOpenIdConnect("oidc", options =>
{ // ekledi�imiz t�m optionlar openID connection i�indi bu ekledi�imiz verileri SD klas�ndan ald�k.
    options.Authority = builder.Configuration["ServiceURLs:IdentityAPI"];
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClientId = "mango";
    options.ClientSecret = "secret"; // normal �artlarda random guid olmas� laz�m
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
app.UseAuthentication(); // authentication authorization'dan �nce gelmeli her zaman.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
