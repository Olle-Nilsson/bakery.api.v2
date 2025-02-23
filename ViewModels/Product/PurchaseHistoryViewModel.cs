using Bakery.ViewModels.Customer;

namespace Bakery.ViewModels.Product;

public class PurchaseHistoryViewModel : ProductViewModel
{
    public IList<CustomersViewModel> Customers { get; set; }
}
