using Bakery.Data;
using Bakery.Entities;
using Bakery.Interfaces;
using Bakery.ViewModels.Customer;
using Bakery.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Bakery.Repositories;

public class ProductRepository(DataContext context) : IProductRepository
{
    private readonly DataContext _context = context;
    public async Task<bool> Add(ProductPostViewModel model)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p =>
                p.Name.ToLower().Trim() == model.Name.ToLower().Trim());

            if (product is not null) throw new Exception("Product already exists");

            product = new Product
            {
                Name = model.Name,
                PricePerUnit = model.PricePerUnit,
                Weight = model.Weight,
                UnitSize = model.UnitSize,
                ExpirationDate = model.ExpirationDate,
                DateOfManufacture = model.DateOfManufacture
            };
            await _context.Products.AddAsync(product);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ProductViewModel> Find(int id)
    {
        try
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync()
            ?? throw new Exception($"No product exists with Id {id}");

            var view = new ProductViewModel
            {
                Name = product.Name,
                PricePerUnit = product.PricePerUnit,
                Weight = product.Weight,
                UnitSize = product.UnitSize,
                ExpirationDate = product.ExpirationDate,
                DateOfManufacture = product.DateOfManufacture
            };

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<PurchaseHistoryViewModel>> History()
    {
        var response = await _context.Products
        .Include(o => o.OrderItems)
            .ThenInclude(o => o.Order)
            .ThenInclude(o => o.Customer)
        .ToListAsync();
        var products = response.Select(p => new PurchaseHistoryViewModel
        {
            Id = p.Id,
            Name = p.Name,
            PricePerUnit = p.PricePerUnit,
            Weight = p.Weight,
            UnitSize = p.UnitSize,
            ExpirationDate = p.ExpirationDate,
            DateOfManufacture = p.DateOfManufacture,
            Customers = [.. p.OrderItems.Select(c => new CustomersViewModel{
                Id = c.Order.Customer.Id,
                Name = c.Order.Customer.Name,
                Email = c.Order.Customer.Email,
                Phone = c.Order.Customer.Phone,
                ContactPerson = c.Order.Customer.ContactPerson
            })]
        });

        return [.. products];
    }

    public async Task<IList<ProductViewModel>> List()
    {
        var response = await _context.Products.ToListAsync();
        var products = response.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            PricePerUnit = p.PricePerUnit,
            Weight = p.Weight,
            UnitSize = p.UnitSize,
            ExpirationDate = p.ExpirationDate,
            DateOfManufacture = p.DateOfManufacture
        });

        return [.. products];
    }

    public async Task<bool> Update(int id, decimal price)
    {
        try
        {
            var result = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            result.PricePerUnit = price;
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
