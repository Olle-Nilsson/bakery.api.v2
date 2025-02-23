using System.Text.Json;
using Bakery.Entities;

namespace Bakery.Data
{
    public static class Seed
    {
        public static async Task LoadAddressTypes(DataContext context)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (context.AddressTypes.Any()) return;

            var json = File.ReadAllText("Data/json/addressTypes.json");
            var types = JsonSerializer.Deserialize<List<AddressType>>(json, options);

            if (types is not null && types.Count > 0)
            {
                await context.AddressTypes.AddRangeAsync(types);
                await context.SaveChangesAsync();
            }
        }
    }
}