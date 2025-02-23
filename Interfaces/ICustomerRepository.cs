using Bakery.ViewModels.Customer;

namespace Bakery.Interfaces;

public interface ICustomerRepository
{
    public Task<bool> Add(CustomerPostViewModel model);
    public Task<IList<CustomersViewModel>> List();
    public Task<CustomerViewModel> Find(int id);
    public Task<bool> Update(int id, string contactPerson);
}
