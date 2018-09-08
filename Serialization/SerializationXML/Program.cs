using System;
using System.Xml.Serialization;

namespace SerializationXML
{
    public class Book // только открытые типы подлежат сериализации
    {
        public String title;

        public Book() // необходим открытый конструктор без параметров
        { /**/ }
    }


    class Program
    {
        static void Main()
        {
            WriteXML();
            ReadXML();
        }


        static void WriteXML()
        {
            Book book  = new Book();       // сериализуемый объект
            book.title = "Fahrenheit 451"; // любая информация для чтения


            // сериализация
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Book));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationXML.xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, book);
            file.Close();
        }

        static void ReadXML()
        {
            // десериализация
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(Book));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationXML.xml";
            System.IO.StreamReader file = new System.IO.StreamReader(path);

            Book book = (Book)reader.Deserialize(file);
            file.Close();


            Console.WriteLine(book.title);
            /***output after deserialization***
            Fahrenheit 451
            */
        }
    }
}
