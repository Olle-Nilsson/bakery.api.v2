namespace Bakery.ViewModels.Product;

public class ProductPostViewModel
{
    public string Name { get; set; }
    public decimal PricePerUnit { get; set; }
    public float Weight { get; set; }
    public int UnitSize { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime DateOfManufacture { get; set; }
}
