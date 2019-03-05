using Adapter.Converter;
using Adapter.Model;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Adapter.Adapter
{
    public class XmlToJsonAdapter : IXmlToJson
    {
        private readonly XmlConverter _xmlConverter;

        public XmlToJsonAdapter(XmlConverter xmlConverter) =>
            _xmlConverter = xmlConverter;

        // совместная работа между двумя разными интерфейсами
        public void ConvertXmlToJson()
        {
            var manufacturers = from m in _xmlConverter.GetXML()
                                .Descendants("Manufacturers")
                                .Elements()
                                select new Manufacturer
                                       {
                                           Brand   = m.Attribute("Brand").Value,
                                           Country = m.Attribute("Country").Value,
                                           Founded = Convert.ToInt32(m.Attribute("Founded").Value)
                                       };

            new JsonConverter(manufacturers)
                .ConvertToJson();
        }
    }
}
