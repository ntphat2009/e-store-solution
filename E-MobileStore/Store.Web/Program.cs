using Microsoft.AspNetCore.Http.Features;
using Store.WebService.APIs;
using Store.WebService.APIs.Interfaces;
using Store.WebService.Services;
using Store.WebService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Register services
builder.Services.AddScoped<IProductApi, ProductApi>();
builder.Services.AddScoped<IProductWebService, ProductWebService>();
builder.Services.AddScoped<IProductImageApi, ProductImageApi>();
builder.Services.AddScoped<IProductImageWebService, ProductImageWebService>();
builder.Services.AddScoped<ICategoryApi, CategoryApi>();
builder.Services.AddScoped<ICategoryWebService, CategoryWebService>();
builder.Services.AddScoped<IBannerApi, BannerApi>();
builder.Services.AddScoped<IBannerWebService, BannerWebService>();
builder.Services.AddScoped<INewsApi, NewsApi>();
builder.Services.AddScoped<INewsWebService, NewsWebService>();
builder.Services.AddScoped<IFlashSaleApi, FlashSaleApi>();
builder.Services.AddScoped<IFlashSaleWebService, FlashSaleWebService>();
builder.Services.AddScoped<IStoreApi, StoreApi>();
builder.Services.AddScoped<IStoreWebService, StoreWebService>();
builder.Services.AddScoped<IAuthenticationApi, AuthenticationApi>();
builder.Services.AddScoped<IAuthenWebService, AuthenWebService>();
builder.Services.AddScoped<IHomeViewService, HomeViewService>();
builder.Services.AddScoped<IProductCategoryViewService, ProductCategoryViewService>();
builder.Services.AddScoped<IProductDetailViewService, ProductDetailViewService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromHours(4);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100485760; // Giới hạn kích thước file upload (ở đây là 10 MB)
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/PageNotFound");
    }
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.UseCors("MyPolicy");
//web admin
app.MapControllerRoute(
    name: "login",
    pattern: "quan-tri-vien/thong-tin-dang-nhap",
    defaults: new { area = "Admin", controller = "Authen", action = "Index" }
    );
app.MapControllerRoute(
    name: "quan-tri-vien",
    pattern: "quan-tri-vien",
    defaults: new { area = "Admin", controller = "Home", action = "Index" }
    );
//Web
app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "category",
    pattern: "{categoryUrl}",
    defaults: new { controller = "Category", action = "Index", categoryUrl = "categoryUrl" }
);
app.MapControllerRoute(
	name: "login",
	pattern: "nguoi-dung/dang-nhap",
	defaults: new { controller = "Authentication", action = "Index" }
);
app.MapControllerRoute(
    name: "detail",
    pattern: "{categoryUrl}/{productUrl}",
    defaults: new { controller = "ProductDetail", action = "Index", categoryUrl = "categoryUrl", productUrl= "productUrl" }
);
app.MapControllerRoute(
    name: "ProductSearchResults",
    pattern: "{controller=Common}/{action=ProductSearchResults}/{search?}",
    defaults: new { controller = "Common" }
);
app.Run();
