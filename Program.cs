using Bakery.Data;
using Bakery.Interfaces;
using Bakery.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(9, 2, 0));

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
    // options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();

    await Seed.LoadAddressTypes(context);
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}
app.MapControllers();

app.Run();