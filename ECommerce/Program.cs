using ECommerce;
using ECommerce.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// SQL Connection String
var connectionString = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString)
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ProdutoRepository>();
builder.Services.AddTransient<ItemPedidoRepository>();
builder.Services.AddTransient<PedidoRepository>();
builder.Services.AddTransient<CadastroRepository>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

await app.MigrateDatabaseAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

//app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Loja}/{action=Carrossel}/{codigo?}");

app.Run();
