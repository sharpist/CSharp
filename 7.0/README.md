#			C# 7.0 | [C# 7.1](https://github.com/sharpist/C_Sharp/tree/master/7.1#c-71) | [C# 7.2](https://github.com/sharpist/C_Sharp/tree/master/7.2#c-72) | [C# 7.3](https://github.com/sharpist/C_Sharp/tree/master/7.3#c-73)
#### ����������: ####

[���������� out](https://github.com/sharpist/C_Sharp/tree/master/7.0#����������-out)

[�������](https://github.com/sharpist/C_Sharp/tree/master/7.0#�������)

[������ ����������](https://github.com/sharpist/C_Sharp/tree/master/7.0#������-����������)

[���������� ���������](https://github.com/sharpist/C_Sharp/tree/master/7.0#����������-���������)

[��������� ���������� � ������������ �������� Ref](https://github.com/sharpist/C_Sharp/tree/master/7.0#���������-����������-�-������������-��������-ref)

[��������� �������](https://github.com/sharpist/C_Sharp/tree/master/7.0#���������-�������)

[������ ��������, ����������� ���������](https://github.com/sharpist/C_Sharp/tree/master/7.0#������-��������-�����������-���������)

[��������� throw](https://github.com/sharpist/C_Sharp/tree/master/7.0#���������-throw)

[���������� ����������� ���� ������������ ��������](https://github.com/sharpist/C_Sharp/tree/master/7.0#����������-�����������-����-������������-��������)

[������������������ � ���������� �������� ���������](https://github.com/sharpist/C_Sharp/tree/master/7.0#������������������-�-����������-��������-���������)

___________________________________________________________________
##			"���������� out"

���������� out ����� ��������� � ������ ���������� � ������ ������,
�� ��������� ��������� �������� ����������:
```c#
if (int.TryParse(input, out int result))
    WriteLine(result);
else
    WriteLine("Could not parse input");
```
*��������� ������������� ������ �������������� ��������� ����������


����� ��� ����������� ������������ � ������� Try. ����� ����������
bool ��������, ����������� �� ����� ��� �������, � ���������� out,
���������� ���������:
```c#
if (!int.TryParse(input, out int result))
{    
    return null;
}

return result;
```
___________________________________________________________________
##			"�������"

����� ������� ������ � ������� Item1 � Item2, ��������� ��������
������� ��������:
```c#
var letters = ("a", "b");
```
����� ������� ������, ������ ���� �������� ����� ������������� ���,
�������� Alpha � Beta:
```c#
(string Alpha, string Beta) letters = ("a", "b");
```
```c#
var letters = (Alpha: "a", Beta: "b");
```
```c#
(string First, string Second) letters = (Alpha: "a", Beta: "b");
```
*����� � ������ ����� ����������, Alpha � Beta, ������������


���������� ������ ������������� �������� � ����� �������:
```c#
public (int NUM, string ABC) Function()
{
    var num = 7;
    var abc = "days";

    return (num, abc);
}
```
��� ���������� (�������������) ��������� ������������� �������
������� ��� ��������� �������� � ������� ����������� ����������:
```c#
(int num, string abc) = Function();
```
������������� ����� ���������� ��� ������ ����. ��� �����
�������� ����� Deconstruct ��� ������� ������, ���������������
����� ���������� out ���� ����������� ���������:
```c#
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
��������� ���� ����� ���������, �������� ������� ��������� ������,
������� ������������� ���� �����:
```c#
var p = new Point(3.14, 2.71);
(double X, double Y) = p;
```
��������� ������������� ������ ������� Deconstruct � ���������� out
�����������:
```c#
public void Deconstruct(out double x, out double y, out double z)
{
    x = this.X;
    y = this.Y;
    z = this.Z;
}
```
��� ����, ������������ ���������� ������� ������ ��������������
��������������� ����������:
```c#
var p = new Point(3.14, 2.71, 1.55);
var (X, Y, Z) = p;
```
___________________________________________________________________
##			"������ ����������"

���������� � ������ ```_``` ��������� ������ ��� ������ ��������,
������� �� ����������� � ���������� (���������� ��������).
�����������:
* ��� ������������� �������� ��� ���������������� �����.
* ��� ������ ������� � ����������� out.
* � �������� ������������� �������� � ����������� is � switch.
* � �������� ����������� ��������������, ����� ��������� ����
���������������� �������� ������������ ��� ������ ����������.
```c#
using System.Linq.Expressions;
using static System.Console;

class Program
{
    public static void Main()
    {
        /// <summary>
        /// � ������ ������ ����������� 3 ������������ ��������,
        /// ������� ��� ������������� ������� ���������� ��������
        /// �������������� ��� ������ ����������
        /// </summary>
        var (species, _, family, population) = animal("�������");

        WriteLine("{0}: ��������� {1}, ����������� {2}",
            species, family, population);
        // �����: ��������� �������, ����������� 4000
    }

    private static (string, string, string, int) animal(string value)
    {
        Expression<del> exp = (x) =>
        x.Item1 == value ||
        x.Item2 == value ||
        x.Item3 == value ? true : false;

        var animals = new[] {
            ("�����",   "�������", "�������",     4000),
            ("�������", "�������", "�����������", 50000)};

        foreach (var animal in animals)
            // ����� ���������� ������ �� 4 ���������
            if (exp.Compile()(animal))
                return animal;

        return ("", "", "", 0);
    }
    delegate bool del((string, string, string, int) x);
}
```
___________________________________________________________________
##			"���������� ���������"

������������� �������� ��������� ���������� ����� ��� ����� �
��������� ������, �� ��������� ��������� ������������.
�������������� ��������� is � switch, � ����� ��� ����������
������ � ������ ����� when.

��������� ������� is
�������� ����� ����� ����� ����� ������ ��������� �������� ��
������� ������������������ ���������� ����� ��������� ����������:
```c#
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

��������� ������������� �������� switch ��������� ��������
�������� ��� ���� ������������.
������� ��������� case ����� ��������, ������� ��� ��������
IEnumerable ������ ������������ ������ ������ ������, ������
������� ������������������:
```c#
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
                    // ������������ ������� ���� PercentDigits
                    sum += (digits.Tens + digits.Ones); break;

                case IEnumerable<object> subList when
                    subList.Any(): // ���� ������������������
                                   // �������� ��������
                    sum += DiceSum(subList); break;

                case IEnumerable<object> subList:
                    // ����� ������: �������� ����/�����������
                    break;


                case null: break;

                default: // ������ ����������� ���������
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
##			"��������� ���������� � ������������ �������� Ref"

������� ���������� ������ �� ���������� ([������������ ��������� ��������](https://github.com/sharpist/C_Sharp/tree/master/Ref%20returns#������������-���������-��������-�-���������-���������-����������)) � ������� �������, �������
����� ��������, ������ �������� �� ���� �������:
```c#
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

        // ref var ��������� ����������� ������� ��� �
        // ���� ������������ �������� �������� �������,
        // ���������� ���������� �������
        ref var item = ref MatrixSearch.Find(matrix,
                                             (val) => val == 42);
        WriteLine(item); // 42
        item = 24;
        WriteLine(matrix[4, 2]); // 24
        // ������������� ��������� � ������� ��������
    }

    // ����� ���������� ������ �� ���������� ��������� (�������� � �������)
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
�����������:

1. ���������� ref ���������� ���������������� ��� ����������.
��������� �������� ���������� �� �������������.

2. ��������� ��������� ���������� ref ����������� ������������
�������� ������ ������.
��������� ������������ ��������� ����:
```c#
ref int i = sequence.Count();
```

3. ���������� ref ������ ���������� ������ ����������, �������
���������� ������������ ���� ����� ����, ��� ����� ����� ��������.
���������� ���������� ������ �� ��������� ���������� ���
���������� � ����������� ��������.

4. ������������ �������� � ��������� ���������� ref �� �����
�������������� � ������������ ��������.
�� ������, ����� ����������� ����� ���������� ��������,
����������� ����������, ��������� �� ����������, �� �������
��������� ������, ������������� ��������.
___________________________________________________________________
##			"��������� �������"

��������� ��������� ������ � ��������� ������� ������. ���������,
��� ��������� ����� ���������� ������ �� ���� ���������, � �������
�� ��� ��������:
```c#
using System;
using System.Collections.Generic;
using static System.Console;

class Example
{
    public static void Main()
    {
        // �������� ���������
        var resultSet = AlphabetSubset('c', 'a');
        WriteLine("iterator created");
        // ��������
        foreach (var thing in resultSet)
            Write($"{thing}, ");
    }

    // �������� ����� �� �������� ������� ���������,
    // ����� ���������� ���� (�������� ����������)
    // ���� �� ��������
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

        // ���������� ��������� �������
        return alphabetSubsetImplementation();

        // ��������� ������� ������ ������������
        IEnumerable<char> alphabetSubsetImplementation()
        {
            for (var c = start; c < end; c++)
                yield return c;
        }
    }
}
```

� ����������� ������, ����������� ������ ����������, ������������
��� �������� ����������, �� ������ ����������� ������:
```c#
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
##			"������ ��������, ����������� ���������"

���������� �������������� �������-������ � ��������, ���������
������ ��� ������:
```c#
public override string ToString() => $"{LastName}, {FirstName}";
```
������ �������� ������ ���������� ������, ������� ����� ����
����������� ��� ��������� � ������������, ������ ����������, �
����� ������ ������� get � set ��� ������� � ������������.

����������� ���������:
```c#
public ExpressionExample(string label) => this.Label = label;
```

����������� ���������:
```c#
~ExpressionExample() => Console.Error.WriteLine("Finalized!");
```

� �������� ������ ������� get/set ����� ���������:
```c#
private string label;

public string Label
{
    get => label;
    set => this.label = value ?? "Default label";
}
```
___________________________________________________________________
##			"��������� throw"

���������� ��� �� ���������, ��� � ��� ���������� throw. �� ������,
��� ����������� ����� ��������� � ����� ������, ����� ��� ��������
���������:
```c#
public string Name
{
    get => name;
    set => name = value ??
        throw new ArgumentNullException(paramName:
            nameof(value), message: "New name must not be null");
}
```

��������� throw � ���������� �������������:
```c#
private ConfigResource loadedConfig = ConfigResourceOrDefault() ?? 
    throw new InvalidOperationException("Could not load config");
```

����� ����� ������������� ������ ���� ���������� � ������������:
```c#
public Example()
{
    loadedConfig = ConfigResourceOrDefault();
    if (loadedConfig == null)
        throw new InvalidOperationException("Could not load config");

    ConfigResource ConfigResourceOrDefault() => new ConfigResource();
}
```
___________________________________________________________________
##			"���������� ����������� ���� ������������ ��������"

Task � ��������� ���, ��� ��� ������������� ������ ��� ������
���������� � ����. ����� ����� � ������������� async ����������
������������ ��������� ��� ����������� ���������, ��������������
��������� ������ � ���� ����� �������� ������������ �����.

�������� � ���, ��� � �������, ����� ��������� �������� �����,
��� �� ��������� ���������� ������������ �������� ���������� ������,
���� ���� ��� ����������� ���������� �� ��������� ������� ������.

������ ```Task```, ```Task<T>``` � ```void```, ����������� ������ ����� ���������� �
������ ���� � ����� ���������� ���������������� ���� ������������
������ ��� ������� async, ��������������� ������������ �������
(���������� ������ GetAwaiter).
��� ```ValueTask<TResult>``` ��������� ������������ ��� �����������:
```c#
public async ValueTask<int> Function()
{
    await Task.Delay(100);
    return 5;
}
```

� �������� ����������� ������ ��������� Task ����� ������������
��������� ValueTask.
��������� ValueTask �������� ����������� � ���������� Task, ���
��������� ������� ValueTask �� ������������� �������� ������
������������� ������������ ������:
```c#
public ValueTask<int> CachedFunc()
{
    return (cache) ? new ValueTask<int>(cacheResult)
                   : new ValueTask<int>(LoadCache());
}

private int cacheResult;
private bool cache = false;

private async Task<int> LoadCache()
{
    await Task.Delay(100);
    cacheResult = 100;
    cache = true;
    return cacheResult;
}
```
����� �������, ����� ����� ���������� ��������� ����� ����
��������, ����� ���� ����������� ����, ��� ��������� ��������
����� �������� ��������� �, ����� ���������, ��� ����� �����
������ ��� �����, ��� ������� �� ��������� ������ ```Task<TResult>```
��� ������� ������ ����� �����������.

����� �� ��������� ��� ������ ������������ ������ ������ ��������
� �������� ```Task``` ��� ```Task<TResult>```. ����������, ���� ������
������������������ ����������, ��� ���������� ������������
```ValueTask<TResult>``` ������ ```Task<TResult>```.
___________________________________________________________________
##			"������������������ � ���������� �������� ���������"

�������� �������� � ����������� ���� ������ ��� ��������� ������
����� � ������������� ����.

```0b``` � ������ ��������� ��������, ��� ����� �������� � ��������
�������:
```c#
public const int One   = 0b0001;
public const int Two   = 0b0010;
public const int Four  = 0b0100;
public const int Eight = 0b1000;
```

�������� ����� ����� ���� ��������, ��� �������� ������ � ��������
���������, ����� ��������� ������� � ������� ������� ```_```:
```c#
public const int Sixteen   = 0b0001_0000;
public const int ThirtyTwo = 0b0010_0000;
public const int SixtyFour = 0b0100_0000;
public const int OneHundredTwentyEight = 0b1000_0000;
```
*����������� �������� ����� ���������� � ����� ����� ���������
```c#
public const long BillionsAndBillions = 100_000_000_000;
```
*����������� �������� ����� ������������ � ������ decimal, float
� double
