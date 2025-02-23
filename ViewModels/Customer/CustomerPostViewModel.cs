using Bakery.ViewModels.Address;

namespace Bakery.ViewModels.Customer;

public class CustomerPostViewModel
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string ContactPerson { get; set; }
    public IList<AddressPostViewModel> Addresses { get; set; }
}
