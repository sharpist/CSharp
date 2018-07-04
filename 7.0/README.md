___________________________________________________________________
##			"Переменные out"

Переменные out можно объявлять в списке аргументов в вызове метода,
не записывая отдельный оператор объявления:
```
if (int.TryParse(input, out int result))
    WriteLine(result);
else
    WriteLine("Could not parse input");
```
*допустимо использование неявно типизированной локальной переменной


Часто эта возможность используется в шаблоне Try. Метод возвращает
bool значение, указывающее на успех или неудачу, и переменную out,
содержащую результат:
```
if (!int.TryParse(input, out int result))
{    
    return null;
}

return result;
```
___________________________________________________________________
##			"Кортежи"

Чтобы создать кортеж с членами Item1 и Item2, назначьте значение
каждому элементу:
```
var letters = ("a", "b");
```
Можно создать кортеж, каждый член которого имеет семантическое имя,
например Alpha и Beta:
```
(string Alpha, string Beta) letters = ("a", "b");
```
```
var letters = (Alpha: "a", Beta: "b");
```
```
(string First, string Second) letters = (Alpha: "a", Beta: "b");
```
*имена в правой части назначения, Alpha и Beta, игнорируются


Объявление метода возвращающего значения в форме кортежа:
```
public (int NUM, string ABC) Function()
{
    var num = 7;
    var abc = "days";

    return (num, abc);
}
```
При распаковке (деконструкции) элементов возвращаемого методом
кортежа для каждомого значения в кортеже объявляется переменная:
```
(int num, string abc) = Function();
```
Деконструкцию можно обеспечить для любого типа. Для этого
создаётся метод Deconstruct как элемент класса, предоставляющий
набор аргументов out всем извлекаемым свойствам:
```
public class Point
{
    public Point(double x, double y)
    {
        this.X = x; this.Y = y;
    }
    public double X { get; }
    public double Y { get; }

    public void Deconstruct(out double x, out double y)
    {
        x = this.X;
        y = this.Y;
    }
}
```
Отдельные поля можно извлекать, назначая кортежу метод Point:
```
var p = new Point(3.14, 2.71);
(double X, double Y) = p;
```
___________________________________________________________________
##			"Пустые переменные"

Переменная с именем "_" доступная только для записи значений,
которые не потребуются в дальнейшем (абстрактно замещает).
Применяются:
* при деконструкции кортежей или пользовательских типов.
* при вызове методов с параметрами out.
* в операции сопоставления шаблонов с выражениями is и switch.
* в качестве автономного идентификатора, когда требуется явно
идентифицировать значение присваивания как пустую переменную.
```
using static System.Console;

class Example
{
    public static void Main()
    {
        // в вызове метода учитываются 3 возвращаемых значения,
        // поэтому при деконструкции кортежа оставшееся значение
        // обрабатывается как пустая переменная
        var (species, _, family, population) = Function("Тигр");

        WriteLine("{0}: семейство {1}, численность {2}",
            species, family, population);
    }

    private static (string, string, string, int)
        // метод возвращает кортеж из 4 элементов
        Function(string species)
    {
        if (species == "Тигр")
        {
            var genus  = "пантеры";
            var family = "кошачьи";
            var population = 4000;

            return (species, genus, family, population);
        }
        if (species == "Косатка")
        {
            var genus  = "оркинус";
            var family = "дельфиновые";
            var population = 50000;

            return (species, genus, family, population);
        }

        return ("", "", "", 0);
    }
}
// Тигр: семейство кошачьи, численность 4000
```
___________________________________________________________________
##			"Регулярные выражения"

Сопоставление шаблонов позволяет отправлять метод для типов и
элементов данных, не связанных иерархией наследования.
Поддерживаются выражения is и switch, а также для добавления
правил в шаблон слово when.

Выражение is
помогает найти сумму чисел когда вместо отдельных значений во
входной последовательности содержится сразу несколько подсписков:
```
using System.Collections.Generic;

class Example
{
    public static void Main()
    {
        var sum  = DiceSum(new List<object> { 5, 1, 3 });
        // sum of values = 9

        var sum2 = DiceSum(new List<object> {
            new List<object> { 5, 1, 3 },
            new List<object> { 5, 1, 3 }
        });
        // sum of values (in all sublists) = 18
    }

    public static int DiceSum(IEnumerable<object> list)
    {
        var sum = 0;
        foreach (var item in list)
        {
            if (item is int val)
                sum += val;

            else if (item is IEnumerable<object> subList)
                sum += DiceSum(subList);
        }
        return sum;
    }
}
```

Обновления оператора switch
