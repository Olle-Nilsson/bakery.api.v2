using Bakery.Data;
using Bakery.Entities;
using Bakery.Interfaces;
using Bakery.ViewModels.Address;
using Bakery.ViewModels.Customer;
using Bakery.ViewModels.Order;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Bakery.Repositories;

public class CustomerRepository(DataContext context, IAddressRepository repo) : ICustomerRepository
{
    private readonly DataContext _context = context;
    private readonly IAddressRepository _repo = repo;

    public async Task<bool> Add(CustomerPostViewModel model)
    {
        try
        {
            if (await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower().Trim() == model.Email.ToLower().Trim()) is not null)
                throw new Exception("Kunden finns redan");

            var customer = new Customer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                ContactPerson = model.ContactPerson
            };

            await _context.Customers.AddAsync(customer);

            foreach (var a in model.Addresses)
            {
                var address = await _repo.Add(a);
                await _context.CustomerAddresses.AddAsync(new CustomerAddresses
                {
                    Customer = customer,
                    Address = address
                });
            }

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<CustomerViewModel> Find(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Include(c => c.CustomerAddresses)
                    .ThenInclude(c => c.Address)
                    .ThenInclude(c => c.AddressType)
                .Include(c => c.Orders)
                .SingleOrDefaultAsync()
            ?? throw new Exception($"No customer exists with Id {id}");

            var view = new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                ContactPerson = customer.ContactPerson,
                Addresses = [.. customer.CustomerAddresses.Select(c => new AddressViewModel
                {
                    Id = c.AddressId,
                    AddressLine = c.Address.AddressLine,
                    PostalCode = c.Address.PostalCode,
                    City = c.Address.City,
                    AddressType = c.Address.AddressType.Value
                })],
                Orders = [.. customer.Orders.Select(o => new OrdersViewModel{
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    OrderNumber = o.OrderNumber,
                    CustomerId = o.CustomerId,
                    LinePrice = o.LinePrice,
                })]
            };

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<CustomersViewModel>> List()
    {
        var response = await _context.Customers.ToListAsync();
        var customers = response.Select(c => new CustomersViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            ContactPerson = c.ContactPerson
        });

        return [.. customers];
    }

    public async Task<bool> Update(int id, string contactPerson)
    {
        try
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
            customer.ContactPerson = contactPerson;

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


    }
}
