namespace Bakery.Entities;

public class Address
{
    public int Id { get; set; }
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public int AddressTypeId { get; set; }

    public AddressType AddressType { get; set; }
    public IList<CustomerAddresses> CustomerAddresses { get; set; }
}
