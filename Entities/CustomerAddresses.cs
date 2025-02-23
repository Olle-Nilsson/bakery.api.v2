namespace Bakery.Entities;

public class CustomerAddresses
{
    public int CustomerId { get; set; }
    public int AddressId { get; set; }

    public Customer Customer { get; set; }
    public Address Address { get; set; }
}