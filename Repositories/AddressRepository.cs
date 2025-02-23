using System.IO.Compression;
using Bakery.Data;
using Bakery.Entities;
using Bakery.Interfaces;
using Bakery.ViewModels.Address;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Bakery.Repositories;

public class AddressRepository(DataContext context) : IAddressRepository
{
    private readonly DataContext _context = context;

    public async Task<Address> Add(AddressPostViewModel model)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a =>
            a.AddressLine.ToLower().Trim() == model.AddressLine.ToLower().Trim() &&
            a.PostalCode.ToLower().Trim() == model.PostalCode.ToLower().Trim() &&
            a.City.ToLower().Trim() == model.City.ToLower().Trim() &&
            a.AddressTypeId == (int)model.AddressType);

        if (address is null)
        {
            address = new Address
            {
                AddressLine = model.AddressLine,
                City = model.City,
                PostalCode = model.PostalCode,
                AddressTypeId = (int)model.AddressType
            };
            await _context.AddAsync(address);
        }

        return address;
    }
}
