# Language Integrated Query

#### Содержание: ####

[Операции над множествами](https://github.com/sharpist/C_Sharp/tree/master/LINQ#операции-над-множествами)

[Операции запросов](https://github.com/sharpist/C_Sharp/tree/master/LINQ#операции-запросов)

[Выражения запросов](https://github.com/sharpist/C_Sharp/tree/master/LINQ#выражения-запросов)

[Продолжение запросов](https://github.com/sharpist/C_Sharp/tree/master/LINQ#продолжение-запросов)

[Множество генераторов](https://github.com/sharpist/C_Sharp/tree/master/LINQ#множество-генераторов)

[Соединение](https://github.com/sharpist/C_Sharp/tree/master/LINQ#соединение)

[Упорядочение](https://github.com/sharpist/C_Sharp/tree/master/LINQ#упорядочение)

[Группирование]()

[Операции OfType() и Cast()]()
_______________________________________________________________________________
## Операции над множествами
_______________________________________________________________________________

Две входных однотипных последовательности объединяются методами одна в конец
другой как есть, либо с удалением дубликатов:
```c#
int[] sequence1 = new[] { 1, 2, 3 },
      sequence2 = new[] { 3, 4, 5 };

IEnumerable<int> concat = sequence1.Concat(sequence2),
                 // 1, 2, 3, 3, 4, 5
                 union = sequence1.Union(sequence2);
                 // 1, 2, 3, 4, 5
```
Могут вычисляться пересечения и исключения:
```c#
IEnumerable<int> intersect = sequence1.Intersect(sequence2),
                 // 3
                 except1 = sequence1.Except(sequence2),
                 // 1, 2
                 except2 = sequence2.Except(sequence1);
                 // 4, 5
```
_______________________________________________________________________________
## Операции запросов
_______________________________________________________________________________

Операции запросов объединены в цепочки:
```c#
var sequence = new[] {
    "олеся", "аня",  "саша", "алёна",
    "таня",  "дима", "яна",  "юра" };

var query = sequence
    .Where(n => n.Contains("а"))
    .OrderBy(n => n)          // bca->abc
    .Select(n => n.ToUpper()) // abc->ABC
    .ToList();

query.ForEach(n => Console.Write(n + " "));
// АЛЁНА АНЯ ДИМА САША ТАНЯ ЮРА ЯНА
```
_______________________________________________________________________________
## Выражения запросов
_______________________________________________________________________________

Предыдущий запрос в форме выражения запроса:
```c#
var sequence = new[] {
    "олеся", "аня",  "саша", "алёна",
    "таня",  "дима", "яна",  "юра" };

var query =
    (from n in sequence
     where n.Contains("а")
     orderby n           // bca->abc
     select n.ToUpper()) // abc->ABC
     .ToList();

query.ForEach(n => Console.Write(n + " "));
// АЛЁНА АНЯ ДИМА САША ТАНЯ ЮРА ЯНА
```
#### Выражение запроса должно начинаться с ```from``` и заканчиваться ```select``` либо ```group``` + ```by```. ####

Использование выражения запросов для внешнего запроса и операции запросов в
подзапросе:
```c#
var sequence = new[] {
    "олеся", "аня",  "саша", "алёна",
    "таня",  "дима", "яна",  "юра" };

var query =
    (from n in sequence
     where n.Contains("а")
                         // subquery: minimum length
     where n.Length == sequence.Min(n2 => n2.Length)
     orderby n           // bca->abc
     select n.ToUpper()) // abc->ABC
     .ToList();

query.ForEach(n => Console.Write(n + " "));
// АНЯ ЮРА ЯНА
```
Ключевое слово ```let``` объявляет новую переменную одновременно с переменной
диапазона ```n```:
```c#
var sequence = new[] {
    "олеся", "аня",  "саша", "алёна",
    "таня",  "дима", "яна",  "юра" };

var query =
    (from n in sequence
     let subset = Regex.Replace(n, "[аиеёоуыэюя]", "") // without vowels
     where subset.Length > 1                           // length more 1
     orderby n                                         // bca->abc
     select n.ToUpper() + "\t" + subset.ToUpper())     // abc->ABC
     .ToList();

query.ForEach(n => Console.WriteLine(n));
/* Output:
    АЛЁНА   ЛН
    ДИМА    ДМ
    ОЛЕСЯ   ЛС
    САША    СШ
    ТАНЯ    ТН
*/
```
_______________________________________________________________________________
## Продолжение запросов
_______________________________________________________________________________

Чтобы добавить конструкции после завершающих ```select``` или ```group``` применяется
продолжающее слово ```into```:
```c#
var query =
    (from c in "Всему своё время. Время разрушать и время строить.".Split(' ', '.')
     select c.ToUpper() // select
     into upper         // 'c' out of context
     where upper.StartsWith("В")
     select upper)      // select
     .ToList();

query.ForEach(n => Console.Write(n + " "));
// ВСЕМУ ВРЕМЯ ВРЕМЯ ВРЕМЯ
```
_______________________________________________________________________________
## Множество генераторов
_______________________________________________________________________________

Больше одного генератора ```from``` в запросе:
```c#
var numbers = new[] { 1, 2, 3 };
var letters = new[] { 'a', 'b', 'c' };

var query =
    (from n in numbers
     from l in letters
     select n.ToString() + l)
     .ToList();

query.ForEach(n => Console.Write(n + " "));
// 1a 1b 1c 2a 2b 2c 3a 3b 3c
```
Векторное произведение фильтруется ```where```:
```c#
var sequence = new[] { "олеся", "аня",  "саша", "алёна" };

var query =
    (from n1 in sequence
     from n2 in sequence
     where n1.CompareTo(n2) < 0                 // n1 before n2 true
     orderby n1, n2                             // bca->abc
     select n1.ToUpper() + "\t" + n2.ToUpper()) // abc->ABC
     .ToList();

query.ForEach(n => Console.WriteLine(n));
/* Output:
    АЛЁНА   АНЯ
    АЛЁНА   ОЛЕСЯ
    АЛЁНА   САША
    АНЯ     ОЛЕСЯ
    АНЯ     САША
    ОЛЕСЯ   САША
*/
```
Второй генератор ```from``` допускает использование первой переменной диапазона:
```c#
var sequence = new[] {
    "Анна Каренина", "Признания Мегрэ" };

var query =
    (from fullName in sequence
     from name in fullName.Split(' ', '.')                // fullName
     select name.ToUpper() + " из " + fullName.ToUpper()) // abc->ABC
     .ToList();

query.ForEach(n => Console.WriteLine(n));
/* Output:
    АННА из АННА КАРЕНИНА
    КАРЕНИНА из АННА КАРЕНИНА
    ПРИЗНАНИЯ из ПРИЗНАНИЯ МЕГРЭ
    МЕГРЭ из ПРИЗНАНИЯ МЕГРЭ
*/
```
_______________________________________________________________________________
## Соединение
_______________________________________________________________________________

Доступно три операции слияния, основные ```Join``` и ```GroupJoin``` выполняются на основе
ключей поиска и обладают высокой производительностью, так как поиск использует
хеш-таблицы. Условие соединения должно использовать операцию эквивалентности.
```Join``` – плоский результирующий набор.
```GroupJoin``` – иерархический результирующий набор.
```c#
var customers = new[] {
    new { ID = 1, Name = "олеся" }, new { ID = 2, Name = "аня" },
    new { ID = 3, Name = "саша" } };

var purchases = new[] {
    new { CustomerID = 1, Product = "яхта" },    new { CustomerID = 2, Product = "дом" },
    new { CustomerID = 2, Product = "самолёт" }, new { CustomerID = 3, Product = "машина" } };

var query =
    (from c in customers
     join p in purchases
     on c.ID equals p.CustomerID
     select c.Name.ToUpper() + "\t" + p.Product.ToUpper()) // abc->ABC
     .ToList();

query.ForEach(n => Console.WriteLine(n));
/* Output:
    ОЛЕСЯ   ЯХТА
    АНЯ     ДОМ
    АНЯ     САМОЛЁТ
    САША    МАШИНА
*/
```
Аналогичное объединение с применением генераторов ```from```:
```c#
var query =
    (from c in customers
     from p in purchases
     where c.ID == p.CustomerID
     select c.Name.ToUpper() + "\t" + p.Product.ToUpper()) // abc->ABC
     .ToList();

query.ForEach(n => Console.WriteLine(n));
/* Output:
    ОЛЕСЯ   ЯХТА
    АНЯ     ДОМ
    АНЯ     САМОЛЁТ
    САША    МАШИНА
*/
```
Выражение запросов ```GroupJoin``` сходно с ```Join```, но требует добавления после
join-конструкции ```into``` для ввода новой переменной диапазона.

Простейшую операцию слияния представляет ```Zip```, которая возвращает
последовательность в следствии применения функции к каждой паре элементов:
```c#
var sequence1 = new[] { 1, 2, 3 };
var sequence2 = new[] { "один", "два", "три", "пропущен" };

sequence1.Zip(sequence2, (n, w) => n + " " + w)
    .ToList().ForEach(nw => Console.WriteLine(nw));
/* Output:
    1 один
    2 два
    3 три
*/
```
_______________________________________________________________________________
## Упорядочение
_______________________________________________________________________________

Сортировка последовательности выполняется через ```orderby``` с любым количеством
критериев:
```c#
var sequence = new[] {
    "олеся", "аня",  "саша", "алёна",
    "таня",  "дима", "яна",  "юра" };

var query =
    (from n in sequence
     orderby n.Length, n // length+bca->abc
     select n.ToUpper()) // abc->ABC
     .ToList();

query.ForEach(n => Console.Write(n + " "));
// АНЯ ЮРА ЯНА ДИМА САША ТАНЯ АЛЁНА ОЛЕСЯ
```
Инверсия сортировки производится добавлением в ```orderby``` после критерия слова
```descending```:
```c#
...
    orderby n.Length descending, n
...
// АЛЁНА ОЛЕСЯ ДИМА САША ТАНЯ АНЯ ЮРА ЯНА
```
_______________________________________________________________________________
## Группирование
_______________________________________________________________________________


_______________________________________________________________________________
## Операции ```OfType()``` и ```Cast()```
_______________________________________________________________________________

