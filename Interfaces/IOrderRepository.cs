using Bakery.ViewModels.Order;

namespace Bakery.Interfaces;

public interface IOrderRepository
{
    public Task<bool> Add(OrderPostViewModel model);
    public Task<OrderViewModel> Find(string orderNumber);
    public Task<IList<OrderViewModel>> Find(DateTime orderDate);
    public Task<IList<OrdersViewModel>> List();
}
