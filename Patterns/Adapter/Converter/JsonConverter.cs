using Adapter.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Adapter.Converter
{
    public class JsonConverter
    {
        private readonly IEnumerable<Manufacturer> _manufacturers;

        public JsonConverter(IEnumerable<Manufacturer> manufacturers) =>
            _manufacturers = manufacturers;

        // сериализация в формат JSON c библиотекой Newtonsoft.Json
        public void ConvertToJson()
        {
            var jsonManufacturers = (string)JsonConvert
                .SerializeObject(_manufacturers, Formatting.Indented);

            Console.WriteLine($"JSON:\n{jsonManufacturers}");
        }
    }
}
