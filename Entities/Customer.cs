namespace Bakery.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string ContactPerson { get; set; }

    public IList<CustomerAddresses> CustomerAddresses { get; set; }
    public IList<Order> Orders { get; set; }
}
