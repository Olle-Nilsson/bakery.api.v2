using Bakery.ViewModels.Customer;
using Bakery.ViewModels.Product;

namespace Bakery.ViewModels.Order;

public class OrderViewModel : OrdersViewModel
{
    public IList<ProductViewModel> Products { get; set; }
    public CustomerViewModel Customer { get; set; }
}
