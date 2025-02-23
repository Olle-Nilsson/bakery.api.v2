using Bakery.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerAddresses> CustomerAddresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.ProductId });
        modelBuilder.Entity<CustomerAddresses>().HasKey(ca => new { ca.CustomerId, ca.AddressId });
    }
}