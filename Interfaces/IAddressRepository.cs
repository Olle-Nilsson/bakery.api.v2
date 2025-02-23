using Bakery.Entities;
using Bakery.ViewModels.Address;

namespace Bakery.Interfaces;

public interface IAddressRepository
{
    public Task<Address> Add(AddressPostViewModel model);
}
