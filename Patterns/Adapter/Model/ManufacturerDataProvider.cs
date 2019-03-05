using System.Collections.Generic;

namespace Adapter.Model
{
    // провайдер данных
    public struct ManufacturerDataProvider
    {
        public static IEnumerable<Manufacturer> GetData() =>
            new List<Manufacturer>
            {
                new Manufacturer { Brand = "Ferrari",    Country = "Italy",  Founded = 1929 },
                new Manufacturer { Brand = "McLaren",    Country = "UK",     Founded = 1989 },
                new Manufacturer { Brand = "Saleen",     Country = "USA",    Founded = 1983 },
                new Manufacturer { Brand = "Bugatti",    Country = "France", Founded = 1909 },
                new Manufacturer { Brand = "Koenigsegg", Country = "Sweden", Founded = 1994 }
            };
    }
}
