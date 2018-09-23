## X M L&nbsp;&nbsp;&nbsp;&nbsp;с е р и а л и з а т о р

#### Содержание: ####

[XML сериализатор](https://github.com/)

[...]()

[...]()

[...]()
_______________________________________________________________________________
# XML сериализатор
_______________________________________________________________________________

Технология неявно применяется веб-службами ASMX и предусматривает сериализацию
типов .NET в файлы XML.

#### Использование: ####
(на основе атрибутов или реализации интерфейса ```IXmlSerializable```).

Классы для сериализации и десериализации объектов содержатся в пространстве
имён ```System.Xml.Serialization```.

1. Создать файл или открыть на чтение в файловом потоке.

2. Воспользоваться методами ```Serialize(Stream, Object)``` или ```Deserialize(Stream)```
экземпляра класса форматера (```XmlSerializer```), предоставляющего базовую
функциональность для форматеров сериализации среды CLR:
* методы ```Serialize``` и ```Deserialize``` работают с объектами ```Stream```,
```XmlWriter/XmlReader```, ```TextWriter/TextReader```.
* класс ```XmlSerializer``` способен сериализировать типы без атрибутов.
* по умолчанию сериализируются все открытые поля и свойства.
* сериализация XML реализует политику отключения (opt-out), поэтому участники
класса, не подлежащие сериализации, отмечаются явно с помощью атрибута ```XmlIgnore```.

Например, класс ```Person```:
```c#
public class Person
{
    public string Name;
    public int Age;
    [System.Xml.Serialization.XmlIgnore]
    public System.DateTime DateOfBirth;
}
```
Сериализировать/десериализировать объект класса ```Person``` можно следующим образом:
```c#
var p = new Person { Name = "Alexander", Age = 32 };

// сериализация
var xs = new System.Xml.Serialization.XmlSerializer(typeof(Person));
using (var s = System.IO.File.Create("person.xml"))
    xs.Serialize(s, p);

// десериализация
Person p2;
using (var s = System.IO.File.OpenRead("person.xml"))
    p2 = (Person)xs.Deserialize(s);

// вывод
System.Console.WriteLine("{0} {1}", p2.Name, p2.Age); // Alexander 32
System.Diagnostics.Process.Start("person.xml");
/* Output:
<?xml version="1.0" encoding="ISO-8859-1"?>
<Person xmlns:xsd="http://www.w3.org/2001/XMLSchema"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <Name>Alexander</Name>
    <Age>32</Age>
</Person>
*/
```
