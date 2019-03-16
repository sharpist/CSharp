## Полиморфизм
_____________________________________________________________________________________

#### Полиморфизм
– обработка во время выполнения объектов производого класса подобно объектам базового
класса:
```c#
class Base { public string Name; }
class Child : Base { }

static void Main()
{
    Child child = new Child { Name = "child" };
    Display(child);
}

static void Display(Base param) => // Child является Base
    Console.WriteLine(param.Name); // child
```

Виртуальные методы позволяют работать с группами связанных объектов универсальным
способом.
Применяется виртуальный метод для вызова соответствующего метода на любой производный
класс через единый вызов в метод базового класса:
```c#
Form form = null; // базовый для Form2...
switch (new Random().Next(2, 6)) // 2.3...5
{
    case 2 : form = new Form2();
        break;

    case 3 : form = new Form3();
        break;

    case 4 : form = new Form4();
        break;

    case 5 : form = new Form5();
        break;
}
// полиморфизм
form.ShowDialog(); // отобразить форму как окно
```
_____________________________________________________________________________________

#### Приведение и преобразования
```c#
// приведение вверх
Child c = new Child();
Base b = c;
// приведение вниз
Child child = (Child)b;
```

Операция ```as```:
```c#
b = new Base();
child = b as Child;
Console.WriteLine(child == null); // True
```

Операция ```is```:
```c#
// является ли объект производным от конкретного типа
if (b is Child)
    Console.WriteLine(((Child)b).Name);
```
