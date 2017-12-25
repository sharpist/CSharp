using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var test1 = new Test1();
                test1.NameOf_UsingNameofExpressionInArgumentNullException();

            var test2 = new Test2();
                test2.Nameof_ExtractsName();
        }
    }

/*
Иногда в коде нужно использовать «магические строки»
- обычные строки в C#, которые сопоставляются с программными элементами в коде.

Например, когда возникает исключение ArgumentNullException,
и необходимо использовать строку для имени соответствующего
параметра, который оказался недопустимым.

Чтобы обойти ограничения (магические строки не проверяются при компиляции и не происходит автоматического обновления строки)
C# предоставляет доступ к имени программного элемента
(класса, метода, параметра, атрибута) в коде с помощью выражения nameof (извлекает имя).
*/

    class Test1
    {
        void ThrowArgumentNullExceptionUsingNameOf(string param1)
        {
            throw new ArgumentNullException(nameof(param1)); // выбросить исключение
        }            // извлечь имя параметра 'param1' передать в свойство ParamName

        [TestMethod]
        public void NameOf_UsingNameofExpressionInArgumentNullException()
        {
            try
            {   // вызов метода
                ThrowArgumentNullExceptionUsingNameOf("data");
                Assert.Fail("недостижимый код");
            }
            catch (ArgumentNullException exception)
            {
                Assert.AreEqual<string>("param1", exception.ParamName); // обработать исключение с параметром 'param1'
            }               // утверждение не выполняется если не равны
        }
    }


    class Test2
    {
        [TestMethod]
        public void Nameof_ExtractsName() // определение различных элементов
        {
            Assert.AreEqual<string>("Test2", nameof(Test2)); // класс Test2

            Assert.AreEqual<string>("TestMethodAttribute", nameof(TestMethodAttribute)); // атрибут TestMethod

            Assert.AreEqual<string>("Nameof_ExtractsName", string.Format("{0}", nameof(Nameof_ExtractsName))); // метод Nameof_ExtractsName
        }
    }
}
