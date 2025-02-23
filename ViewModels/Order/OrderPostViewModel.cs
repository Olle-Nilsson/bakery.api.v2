using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.ViewModels.Order;

public class OrderPostViewModel
{
    public string OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public IList<OrderItemPostViewModel> OrderItems { get; set; }
}
