using System.Runtime.Serialization;
using Bakery.Data;
using Bakery.Entities;
using Bakery.Interfaces;
using Bakery.ViewModels.Address;
using Bakery.ViewModels.Customer;
using Bakery.ViewModels.Order;
using Bakery.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Bakery.Repositories;

public class OrderRepository(DataContext context) : IOrderRepository
{
    private readonly DataContext _context = context;

    public async Task<bool> Add(OrderPostViewModel model)
    {
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o =>
                o.OrderNumber == model.OrderNumber);

            if (order is not null) throw new Exception(
                $"Order with OrderNumber {model.OrderNumber} already exists");

            var customer = await _context.Customers.FirstOrDefaultAsync(c =>
                c.Id == model.CustomerId)
                ?? throw new Exception($"No Customer with id {model.CustomerId} exists");

            order = new Order
            {
                OrderDate = DateTime.Now,
                OrderNumber = model.OrderNumber,
                Customer = customer,
                OrderItems = []
            };


            foreach (var item in model.OrderItems)
            {
                var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == item.ProductId)
                    ?? throw new Exception($"No Product with id {item.ProductId} exists");

                var orderItem = new OrderItem
                {
                    Product = product,
                    Order = order,
                    Quantity = item.Quantity,
                    Price = product.PricePerUnit * item.Quantity
                };

                order.OrderItems.Add(orderItem);
                order.LinePrice += orderItem.Price;
            }

            await _context.Orders.AddAsync(order);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<OrderViewModel> Find(string orderNumber)
    {
        try
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Product)
                .Include(o => o.Customer)
                    .ThenInclude(o => o.CustomerAddresses)
                    .ThenInclude(o => o.Address)
                    .ThenInclude(o => o.AddressType)
                .Where(o => o.OrderNumber == orderNumber)
                .SingleOrDefaultAsync()
            ?? throw new Exception($"No order exists with OrderNumber {orderNumber}");

            var view = new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                LinePrice = order.LinePrice,
                Products = [.. order.OrderItems.Select(p => new ProductViewModel
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    PricePerUnit = p.Product.PricePerUnit,
                    Weight = p.Product.Weight,
                    UnitSize = p.Product.UnitSize,
                    ExpirationDate = p.Product.ExpirationDate,
                    DateOfManufacture = p.Product.DateOfManufacture
                })],
                Customer = new CustomerViewModel
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Phone = order.Customer.Phone,
                    Email = order.Customer.Email,
                    ContactPerson = order.Customer.ContactPerson,
                    Addresses = [.. order.Customer.CustomerAddresses.Select(c => new AddressViewModel{
                        Id = c.Address.Id,
                        AddressLine = c.Address.AddressLine,
                        City = c.Address.City,
                        PostalCode = c.Address.PostalCode,
                        AddressType = c.Address.AddressType.Value
                    })]
                }
            };

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<OrderViewModel>> Find(DateTime orderDate)
    {
        try
        {
            var orders = await _context.Orders
                .Where(o => o.OrderDate.Date == orderDate.Date)
                .Include(o => o.OrderItems)
                    .ThenInclude(o => o.Product)
                .Include(o => o.Customer)
                    .ThenInclude(o => o.CustomerAddresses)
                    .ThenInclude(o => o.Address)
                    .ThenInclude(o => o.AddressType)
                .ToListAsync()
            ?? throw new Exception($"No order has been palced on {orderDate}");

            var view = orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                LinePrice = order.LinePrice,
                Products = [.. order.OrderItems.Select(p => new ProductViewModel
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    PricePerUnit = p.Product.PricePerUnit,
                    Weight = p.Product.Weight,
                    UnitSize = p.Product.UnitSize,
                    ExpirationDate = p.Product.ExpirationDate,
                    DateOfManufacture = p.Product.DateOfManufacture
                })],
                Customer = new CustomerViewModel
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Phone = order.Customer.Phone,
                    Email = order.Customer.Email,
                    ContactPerson = order.Customer.ContactPerson,
                    Addresses = [.. order.Customer.CustomerAddresses.Select(c => new AddressViewModel{
                        Id = c.Address.Id,
                        AddressLine = c.Address.AddressLine,
                        City = c.Address.City,
                        PostalCode = c.Address.PostalCode,
                        AddressType = c.Address.AddressType.Value
                    })]
                }
            });

            return [.. view];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<OrdersViewModel>> List()
    {
        var response = await _context.Orders.ToListAsync();
        var orders = response.Select(o => new OrderViewModel
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            OrderNumber = o.OrderNumber,
            CustomerId = o.CustomerId,
            LinePrice = o.LinePrice
        });

        return [.. orders];
    }
}
