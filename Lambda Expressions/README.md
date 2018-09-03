___________________________________________________________________________________________

# "Лямбда-выражения" – 
#### анонимная функция, с помощью которой можно создавать типы делегатов или деревьев выражений. ####
___________________________________________________________________________________________

Создать лямбда-выражение:
```c#
delegate int del(int i);
static void Main()
{
    del myDelegate = x => x * x;
    int j = myDelegate(5); // j = 25
}
```

Создать тип дерева выражений:
```c#
using System.Linq.Expressions;

class Program
{
    static void Main()
    {
        Expression<del> myET = x => x * x;
    }
}
```
___________________________________________________________________________________________

# "Асинхронные лямбда-выражения" – 
#### (включающие асинхронную обработку) можно легко создавать с помощью ключевых слов ```async``` и ```await```. ####
___________________________________________________________________________________________

Например, следующий код содержит обработчик событий, вызывающий и ожидающий асинхронный
метод ```methodAsync```:
```c#
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private async void button_Click(object sender, EventArgs e)
    {
        // methodAsync returns a Task
        await methodAsync();
        textBox.Text += "\r\nControl returned to Click event handler.\n";
    }

    private async Task methodAsync()
    {
        // the following line simulates a task-returning asynchronous process
        await Task.Delay(1000);
    }
}
```

Такой же обработчик событий можно добавить с помощью асинхронного лямбда-выражения:
```c#
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        button.Click += async (sender, e) =>
        {
            // methodAsync returns a Task
            await methodAsync();
            textBox.Text += "\nControl returned to Click event handler.\n";
        };
    }

    private async Task methodAsync()
    {
        // The following line simulates a task-returning asynchronous process
        await Task.Delay(1000);
    }
}
```
*модификатор ```async``` перед списком параметров лямбда-выражения
___________________________________________________________________________________________
# "Лямбды в стандартных операторах запросов" – 
#### таких как метод ```Count```, имеющий входной параметр типа ```Func<T,TResult>``` универсальный делегат. ####
___________________________________________________________________________________________

Делегаты ```Func``` полезны для инкапсуляции пользовательских выражений:
```c#
Func<int, bool> myFunc = x => x == 5;
bool result = myFunc(4); // returns false of course
```
Выражения могут применяться к каждому элементу в наборе исходных данных:
```c#
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
int oddNumbers = numbers.Count(n => n % 2 == 1); // 5, 1, 3, 9, 7
```

```c#
var firstNumbersLessThan6 = numbers.TakeWhile(n => n < 6); // all numbers up to 6
```
