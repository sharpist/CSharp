# Language Integrated Query

#### Содержание: ####

[Операции над множествами]()

[Операции запросов]()

[Выражения запросов]()

[Продолжение запросов]()

[Множество генераторов]()
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
