using System.ComponentModel.DataAnnotations.Schema;

namespace Bakery.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; }
    public int CustomerId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LinePrice { get; set; }

    public Customer Customer { get; set; }
    public IList<OrderItem> OrderItems { get; set; }
}