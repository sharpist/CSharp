using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Slide
{
    class Program
    {
        public static void Main()
        {
            ///<girl>
            ///  <id>1</id>
            ///  <name>Dasha</name>
            ///  <age>21</age>
            ///  <hair>blonde</hair>
            ///  <weight>42</weight>
            ///</girl>

            // получить источник данных
            String xmlString = File.ReadAllText(@"Разметка.xml");
            XDocument xdoc = XDocument.Load(new StringReader(xmlString));


            // извлечь по id 1
            Console.WriteLine(GetXmlValue("1", "name", xdoc)); // имя
            Console.WriteLine(GetXmlValue("1", "age", xdoc));  // возраст
        }

        private static string GetXmlValue(string id, string getData, XDocument xdoc)
        {
            return xdoc.Root.Descendants("girl").Elements().
                Where(node => node.Name == "id" && node.Value == id).
                Single<XElement>().NodesAfterSelf().
                Where(values => ((XElement)values).Name == getData).
                Cast<XElement>().Single<XElement>().Value;
        }
    }
}