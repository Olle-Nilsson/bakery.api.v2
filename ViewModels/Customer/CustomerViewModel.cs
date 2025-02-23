using Bakery.ViewModels.Address;
using Bakery.ViewModels.Order;

namespace Bakery.ViewModels.Customer;

public class CustomerViewModel : CustomersViewModel
{
    public IList<AddressViewModel> Addresses { get; set; }
    public IList<OrdersViewModel> Orders { get; set; }
}
