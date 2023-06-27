using DU_Summer_2023_Capstone.Data;
using DU_Summer_2023_Capstone.Data.Interfaces;
using DU_Summer_2023_Capstone.Data.Models;
using DU_Summer_2023_Capstone.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IPizzaRepository, PizzaRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(sp => Cart.GetCart(sp));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Services.AddMvc();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddRazorPages();

var app = builder.Build();

// auto runs pending migrations on local server initialize
using (var scope = app.Services.CreateScope())
{
    var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    applicationDbContext.Database.Migrate();

    Config.CreateRoles(scope.ServiceProvider).Wait();

    // Creating roles and admin user
    //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    // var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    // string email = "admin@pizzares.com";


    // if (!await roleManager.RoleExistsAsync("Admin"))
    // {
    //     await roleManager.CreateAsync(new IdentityRole("Admin"));
    // }

    // if (await userManager.FindByEmailAsync(email) == null)
    // {
    //     var admin = new IdentityUser
    //     {
    //         Email = email,
    //         UserName = email
    //     };

    //     var result = await userManager.CreateAsync(admin, "pizzares@password");
    //     if (result.Succeeded)
    //     {
    //         await userManager.AddToRoleAsync(admin, "Admin");
    //     }
    // }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

//defines the URL pattern (www.pizzarestaurant.com/Pages/Index/{id}) & controller to use
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}/{id?}",
        defaults: new
        {
            controller = "Home",
            action = "Index"
        });
});

app.MapRazorPages();

DbInitializer.Seed(app);
Console.WriteLine("Hello World");

app.Run();

