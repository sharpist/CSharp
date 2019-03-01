using Microsoft.VisualStudio.TestTools.UnitTesting;

class Program
{
    static void Main()
    {
        new Test1().NameOf_ArgumentNullException();
        new Test2().NameOf_ExtractName();
    }
}
/// <summary>
/// Иногда в коде приходится использовать "магические строки" (обычные строки в C#),
/// которые сопоставляются с программными элементами в коде.
/// 
/// Когда применяется строка для имени соответствующего параметра, который оказался
/// недопустимым, возникает исключение ArgumentNullException.
/// Так как "магические строки" не проверяются при компиляции и не происходит их
/// автоматического обновления, чтобы обойти ограничения C# предоставляет доступ к
/// имени программного элемента (класса, метода, параметра, атрибута) в коде с
/// помощью выражения nameof.
/// </summary>
[TestClass]
public class Test1
{
    [TestMethod]
    public void NameOf_ArgumentNullException()
    {
        try
        {
            ArgumentNullException("data");
            Assert.Fail("недостижимый код");
        }
        catch (System.ArgumentNullException ex)
        {
            // создать исключение, если значения не равны
            Assert.AreEqual<string>("param", ex.ParamName);
        }
    }

    void ArgumentNullException(string param)
        // создать исключение и извлечь имя параметра
        => throw new System.ArgumentNullException(nameof(param));
}

[TestClass]
public class Test2
{
    [TestMethod]
    public void NameOf_ExtractName()
    {
        // создать исключение, если значения не равны
        Assert.AreEqual<string>("Test2", nameof(Test2)); // OK

        Assert.AreEqual<string>("TestMethodAttribute", nameof(TestMethodAttribute)); // OK

        Assert.AreEqual<string>(
            "NameOf_ExtractName", string.Format("{0}", nameof(NameOf_ExtractName))); // OK
    }
}
