using BlogCore.Data;
using BlogCore.DL.Data.Inicializer;
using BlogCore.DL.Data.Repository;
using BlogCore.DL.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultSqlConnections") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();
builder.Services.AddControllersWithViews();


//Agregar WorkContainer
builder.Services.AddScoped<IWorkContainer, WorkContainer>();

//First Step:Agregar Siembra de datos Seeding para que se cree un primer usuario y roles por default
builder.Services.AddScoped<IinitializerDb, InitializerDb>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
DataSeeding();



app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();



//Function
//Second Step: Metodo que ejecuta la Siembra de datos por default en la base de datos.
void DataSeeding()
{
    using (var scope = app.Services.CreateScope())
    {
        var initializerDb = scope.ServiceProvider.GetRequiredService<IinitializerDb>();
        initializerDb.Initializer();
    }
}