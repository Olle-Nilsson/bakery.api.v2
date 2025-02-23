using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.ViewModels.Product;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal PricePerUnit { get; set; }
    public float Weight { get; set; }
    public int UnitSize { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime DateOfManufacture { get; set; }
}
