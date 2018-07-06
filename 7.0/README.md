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

Выражение шаблона is
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

Выражение сопоставления шаблонов switch расширяет сценарий
сохряняя при этом компактность.
Порядок выражений case имеет значение, вариант для элемента
IEnumerable должен отображаться раньше общего случая, пустой
входной последовательности:
```
using System;
using System.Linq;
using System.Collections.Generic;

class Example
{
    public static void Main()
    {
        var sum  = DiceSum(new List<object> { 5, 1, 3 });
        // sum of values = 9

        var sum2 = DiceSum(new List<object> {
            new PercentDigits(20, 1),
            new List<object> { 5, 1, 3 }
        });
        // sum of values (in all sublists) = 30
    }

    public static int DiceSum(IEnumerable<object> list)
    {
        var sum = 0;
        foreach (var item in list)
        {
            switch (item) {
                case int val:
                    sum += val; break;

                case PercentDigits digits:
                    // соответствие объекту типа PercentDigits
                    sum += (digits.Tens + digits.Ones); break;

                case IEnumerable<object> subList when
                    subList.Any(): // если последовательность
                                   // содержит элементы
                    sum += DiceSum(subList); break;

                case IEnumerable<object> subList:
                    // общий случай: элементы есть/отсутствуют
                    break;


                case null: break;

                default: // всегда вычисляется последним
                    throw new InvalidOperationException
                        ("unknown item type");
            }
        }
        return sum;
    }
}

struct PercentDigits
{
    public int Ones { get; }
    public int Tens { get; }
    // (0, 1...9) + (0, 10...90) = 0...99
    public PercentDigits(int ones, int tens)
    {
        this.Ones = ones; this.Tens = tens;
    }
}
```
___________________________________________________________________
##			"Локальные переменные и возвращаемые значения Ref"

Функция возвращает ссылку на переменную - элемент матрицы, который
нужно изменить, вместо индексов на этот элемент:
```
using System;
using static System.Console;

class MatrixSearch
{
    public static void Main()
    {
        var matrix = new int[5, 10];
        for ((int i, int k) = (0, 0); i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                matrix[i, j] = k++;

        // ref var позволяет компилятору указать тип и
        // если возвращаемое значение является ссылкой,
        // переменная становится ссылкой
        ref var item = ref MatrixSearch.Find(matrix,
                                             (val) => val == 42);
        WriteLine(item); // 42
        item = 24;
        WriteLine(matrix[4, 2]); // 24
        // следовательно хранилище в матрице изменено
    }

    // метод возвращает ссылку на внутреннее хранилище (значение в матрице)
    public static ref int Find(int[,] matrix,
                               Func<int, bool> predicate)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (predicate(matrix[i, j]))
                    return ref matrix[i, j];
            }
        }
        throw new InvalidOperationException("Not found");
    }
}
```
Ограничения:

1. Переменную ref необходимо инициализировать при объявлении.
Запрещено отделять объявление от инициализации.

2. Присвоить локальной переменной ref стандартное возвращаемое
значение метода нельзя.
Запрещено использовать операторы вида:
```
ref int i = sequence.Count();
```

3. Переменную ref нельзя возвращать другой переменной, которая
продолжает существовать даже после того, как метод будет выполнен.
Невозможно возвратить ссылку на локальную переменную или
переменную с аналогичной областью.

4. Возвращаемые значения и локальные переменные ref не могут
использоваться с асинхронными методами.
На момент, когда асинхронный метод возвращает значение,
компилятору неизвестно, присвоено ли переменной, на которую
указывает ссылка, окончательное значение.
___________________________________________________________________
##			"Локальные функции"

Позволяют объявлять методы в контексте другого метода. Очевиднее,
что локальный метод вызывается только из того контекста, в котором
он был объявлен:
```
using System;
using System.Collections.Generic;
using static System.Console;

class Example
{
    public static void Main()
    {
        // создание итератора
        var resultSet = AlphabetSubset('c', 'a');
        WriteLine("iterator created");
        // итерация
        foreach (var thing in resultSet)
            Write($"{thing}, ");
    }

    // открытый метод не является методом итератора,
    // иначе выполнение кода (проверка аргументов)
    // было бы отложено
    public static IEnumerable<char> AlphabetSubset(char start,
                                                   char end)
    {
        if (start < 'a' || start > 'z')
            throw new ArgumentOutOfRangeException(paramName:
                nameof(start), message: "start must be a letter");
        if (end < 'a' || end > 'z')
            throw new ArgumentOutOfRangeException(paramName:
                nameof(end), message: "end must be a letter");
        if (end <= start)
            throw new ArgumentException(
                $"{nameof(end)} must be greater than {nameof(start)}");

        // вызывается локальная функция
        return alphabetSubsetImplementation();

        // локальная функция создаёт перечисление
        IEnumerable<char> alphabetSubsetImplementation()
        {
            for (var c = start; c < end; c++)
                yield return c;
        }
    }
}
```

В асинхронном методе, гарантируют выдачу исключения, возникающего
при проверке параметров, до начала асинхронной работы:
```
public Task<string> LongRunningWork(string address, int index,
                                    string name)
{
    if (string.IsNullOrWhiteSpace(address))
        throw new ArgumentException(message:
            "An address is required", paramName: nameof(address));
    if (index < 0)
        throw new ArgumentOutOfRangeException(paramName:
            nameof(index), message: "The index must be non-negative");
    if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException(message:
            "You must supply a name", paramName: nameof(name));

    return longRunningWorkImplementation();

    async Task<string> longRunningWorkImplementation()
    {
        var result1 = await FirstWork(address);
        var result2 = await SecondWork(index, name);
        return $"The results are {result1} and {result2}";
    }
}
```
___________________________________________________________________
##			"Другие элементы, воплощающие выражение"

