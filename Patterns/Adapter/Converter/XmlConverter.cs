using Adapter.Model;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Adapter.Converter
{
    public class XmlConverter
    {
        public XDocument GetXML()
        {
            var xAttributes = from m in ManufacturerDataProvider.GetData()
                              select new XElement("Manufacturer",
                                                  new XAttribute("Brand", m.Brand),
                                                  new XAttribute("Country", m.Country),
                                                  new XAttribute("Founded", m.Founded));

            var xElement = new XElement("Manufacturers");
            xElement.Add(xAttributes);

            var xDocument = new XDocument();
            xDocument.Add(xElement);

            Console.WriteLine($"XML:\n{xDocument}");
            return xDocument;
        }
    }
}
