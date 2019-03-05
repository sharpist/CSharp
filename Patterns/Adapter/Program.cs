using Adapter.Adapter;
using Adapter.Converter;

namespace Adapter
{
    /// <summary>
    /// "Adapter" – это шаблон проектирования, который позволяет
    /// несовместимым интерфейсам работать совместно.
    /// 
    /// Так объекты из разных интерфейсов способны обмениваться
    /// данными.
    /// </summary>
    class Program
    {
        static void Main() =>
            new XmlToJsonAdapter(new XmlConverter())
            .ConvertXmlToJson();
        /* Output:
            XML:
            <Manufacturers>
              <Manufacturer Brand="Ferrari" Country="Italy" Founded="1929" />
              <Manufacturer Brand="McLaren" Country="UK" Founded="1989" />
              <Manufacturer Brand="Saleen" Country="USA" Founded="1983" />
              <Manufacturer Brand="Bugatti" Country="France" Founded="1909" />
              <Manufacturer Brand="Koenigsegg" Country="Sweden" Founded="1994" />
            </Manufacturers>
            JSON:
            [
              {
                "Brand": "Ferrari",
                "Country": "Italy",
                "Founded": 1929
              },
              {
                "Brand": "McLaren",
                "Country": "UK",
                "Founded": 1989
              },
              {
                "Brand": "Saleen",
                "Country": "USA",
                "Founded": 1983
              },
              {
                "Brand": "Bugatti",
                "Country": "France",
                "Founded": 1909
              },
              {
                "Brand": "Koenigsegg",
                "Country": "Sweden",
                "Founded": 1994
              }
            ]
        */
    }
}
