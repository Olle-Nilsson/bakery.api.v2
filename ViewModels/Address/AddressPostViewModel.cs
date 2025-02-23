using System.Text.Json.Serialization;

namespace Bakery.ViewModels.Address;
public enum AddressTypeEnum
{
    Delivery = 1,
    Invoice = 2
}
public class AddressPostViewModel
{
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AddressTypeEnum AddressType { get; set; }
}
