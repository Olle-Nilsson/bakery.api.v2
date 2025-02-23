using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.ViewModels.Order;

public class OrdersViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public decimal LinePrice { get; set; }
}
