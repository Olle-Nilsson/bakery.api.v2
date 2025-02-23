using Bakery.ViewModels.Product;

namespace Bakery.Interfaces;

public interface IProductRepository
{
    public Task<bool> Add(ProductPostViewModel model);
    public Task<IList<ProductViewModel>> List();
    public Task<ProductViewModel> Find(int id);
    public Task<bool> Update(int id, decimal price);
    public Task<IList<PurchaseHistoryViewModel>> History();
}
