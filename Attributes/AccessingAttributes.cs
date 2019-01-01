using System.Collections.Generic;
using System.Reflection;
using static System.Console;

// многократно используемый настраиваемый атрибут
[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct |
                       System.AttributeTargets.Method,
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
[Author("R. Koch", version = 1.3)]
public class SampleClass
{
    // P. Ackerman's code goes here...
    // R. Koch's code goes here...

    // атрибут применяется к методу класса
    [Author("A. Carpenter", version = 2.0)]
    public void SampleMethod()
    {
        // A. Carpenter's code goes here...
    }
}

class TestAuthorAttribute
{
    public static void Test() => PrintAuthorInfo(typeof(SampleClass));

    private static void PrintAuthorInfo(System.Type t)
    {
        WriteLine($"Информация об авторах класса {t}:");
        // использование отражения
        var attrs = System.Attribute.GetCustomAttributes(t);

        // вывод данных
        foreach (var attr in attrs)
            if (attr is Author) {
                var a = (Author)attr;
                WriteLine($"\t{a.GetName()},\tверсия {a.version:f}");
            }


        // найти атрибуты на уровне методов
        WriteLine($"\nИнформация об авторах методов класса {t}:");
        var meths = t.GetMethods();
        #region доступ к закрытым членам
            // вместо открытия доступа к методам, класс TypeInfo
            // реализует свойства, возвращающие IEnumerable<T>,
            // унаследованные члены исключаются из результата
            // IEnumerable<MemberInfo> meths = t.GetTypeInfo().DeclaredMethods; // или DeclaredMembers от контекста
        #endregion
        foreach (var meth in meths) {
            var a = (Author)System.Attribute.GetCustomAttribute(meth, typeof(Author));
            if (a != null)
                WriteLine($"\t{a.GetName()},\tверсия {a.version:f}");
            else
                WriteLine($"Нет атрибута в функции члена {meth.ToString()}");
        }
    }
    // Информация об авторах класса SampleClass:
    //         P. Ackerman,    версия 1,00
    //         R. Koch,        версия 1,30
    //
    // Информация об авторах методов класса SampleClass:
    //         A. Carpenter,   версия 2,00
}

class Program { static void Main() => TestAuthorAttribute.Test(); }