# nameof
Оператор ```nameof``` – используется для получения простого (неполного) строкового имени
переменной, типа или члена, позволяя поддерживать код допустимым при переименовании
определений.

Получение имени для некоторого свойства объекта:
```c#
System.Console.WriteLine(nameof(Planet.Human)); // Human
```

Для извлечения полного имени применяется выражение ```nameof``` в сочетании с ```typeof```:
```c#
namespace Star
{
    class Program
    {
        static void Main() => System.Console
            .WriteLine(new Planet().Human()); // Star.Planet.Human
    }

    struct Planet
    {
        public string Human() => $"{typeof(Planet)}.{nameof(Human)}";
    }
}
```
