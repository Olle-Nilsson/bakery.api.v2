using System.ComponentModel.DataAnnotations.Schema;

namespace Bakery.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerUnit { get; set; }
    public float Weight { get; set; }
    public int UnitSize { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime DateOfManufacture { get; set; }

    public IList<OrderItem> OrderItems { get; set; }
}
