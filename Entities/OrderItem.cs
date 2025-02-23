using System.ComponentModel.DataAnnotations.Schema;

namespace Bakery.Entities;

public class OrderItem
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; }
    public Product Product { get; set; }
}
