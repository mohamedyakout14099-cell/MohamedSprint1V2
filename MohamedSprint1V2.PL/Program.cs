using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MohamedSprint1V2.DAL.Database;
using MohamedSprint1V2.DAL.Entity;
using MohamedSprint1V2.DAL.Repo.Abstraction;
using MohamedSprint1V2.DAL.Repo.Impelementation;
using MohamedSprint1V2.DLL.Service.Abstraction;
using MohamedSprint1V2.DLL.Service.Impelementation;

var builder = WebApplication.CreateBuilder(args);
 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MohamedSprint1V2DbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<MohamedSprint1V2DbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthService, AuthService >();
builder.Services.AddScoped<IUserRepo, UserRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();  
