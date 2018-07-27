using static System.Console;

// многократно используемый настраиваемый атрибут
[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct,
                       AllowMultiple = true)
]
public class Author : System.Attribute
{
    string name;
    public double version;

    public Author(string name)
    {
        this.name = name;
        version = 1.0;
    }
    public string GetName() => this.name;
}

// несколько атрибутов одного типа применяются к классу
[Author("P. Ackerman")]
[Author("R. Koch", version = 2.0)]
public class SampleClass
{
    // P. Ackerman's code goes here...  
    // R. Koch's code goes here...  
}

class TestAuthorAttribute
{
    public static void Test() => PrintAuthorInfo(typeof(SampleClass));

    private static void PrintAuthorInfo(System.Type t)
    {
        WriteLine($"Информация об авторах для {t}:");
        // использование отражения
        var attrs = System.Attribute.GetCustomAttributes(t);

        // вывод данных
        foreach (var attr in attrs)
            if (attr is Author) {
                var a = (Author)attr;
                WriteLine($"\t{a.GetName()},\tверсия {a.version:f}");
            }
    }
    // Информация об авторах для SampleClass:
    //         P. Ackerman,    версия 1,00
    //         R. Koch,        версия 2,00
}

class Program { static void Main() => TestAuthorAttribute.Test(); }