using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// При каждом запуске программы создается новый объект Loan,
/// поэтому выводятся одни и те же изначальные значения.
/// 
/// Сериализация позволяет сохранять последние изменения между
/// экземплярами приложения. Например, когда процентная ставка
/// меняется со временем. 
/// </summary>
namespace Serialization
{
    class Program
    {
        static void Main()
        {
            const string FileName = @"../../../SavedLoan.bin"; // имя файла сериализованных данных

            Loan TestLoan = new Loan(10000.0, 7.5, 36, "Neil Black");

            // код для десериализации объекта из файла
            if (File.Exists(FileName))
            {
                Console.WriteLine("Reading saved file!\n");
                Stream openFileStream = File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                TestLoan = (Loan)deserializer.Deserialize(openFileStream);
                TestLoan.TimeLastLoaded = DateTime.Now;
                openFileStream.Close();
            }
            /***output after deserialization*** (second start)
            Reading saved file!

            New customer value: Henry Clay
            Initial value: 7,1
            Current value: 7,1
            */

            // обработчик событий для события PropertyChanged
            TestLoan.PropertyChanged += (_, __) =>
            Console.WriteLine($"New customer value: {TestLoan.Customer}");

            // изменить объект Loan
            TestLoan.Customer = "Henry Clay";
            Console.WriteLine($"Initial value: {TestLoan.InterestRate}");
            TestLoan.InterestRate = 7.1;
            Console.WriteLine($"Current value: {TestLoan.InterestRate}");
            /***output before serialization*** (first start)
            New customer value: Henry Clay
            Initial value: 7,5
            Current value: 7,1
            */

            // код для сериализации класса в файл
            Stream SaveFileStream = File.Create(FileName); // чтение двоичного файла
            BinaryFormatter serializer = new BinaryFormatter(); // преобразование файла
            serializer.Serialize(SaveFileStream, TestLoan); // преобразовать тип потока в тип объекта Loan
            SaveFileStream.Close();
        }
    }
}
