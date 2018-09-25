## X M L&nbsp;&nbsp;&nbsp;&nbsp;с е р и а л и з а т о р

#### Содержание: ####

[XML сериализатор](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#xml-сериализатор)

[Атрибуты XML сериализации](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Атрибуты-xml-сериализации)

[Упорядочение XML-элементов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Упорядочение-xml-элементов)

[Подклассы]()

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
* #### вместо атрибута ```OnDeserializing``` при десериализации предварительно выполняется обязательный явный/неявный конструктор без параметров. ####
* #### перед десериализацией выполняются инициализаторы полей. ####
* десериализатор переносим в контексте версий и допускает лишние данные или
отсутствующие элементы и атрибуты.

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
*класс ```Person``` включает неявный конструктор без параметров

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

XML сериализатор может работать с большинством типов данных.

#### Тип данных должен представлять: ####
* любой примитивный тип.
* ```DateTime```, ```TimeSpan```, ```Guid```.
* версии, допускающие ```null```, указанных выше типов.
* ```byte[]```.
* тип ```XmlAttribute``` или ```XmlElement```.
* любой тип, реализующий интерфейс ```IXmlSerializable```.
* любой тип коллекции.
_______________________________________________________________________________
# Атрибуты XML сериализации
_______________________________________________________________________________

Изначально участники класса сериализируются стандартно в XML-элементы:
```xml
<Person ... >
    <Name>Alexander</Name>
    <Age>32</Age>
</Person>
```
Предусматривается возможность принудительного включения в XML-атрибут с помощью
атрибута ```XmlAttribute```:
```c#
public class Person
{
    [XmlAttribute] public string Name;
    [XmlAttribute] public int Age;

    [XmlElement] // по умолчанию
    public System.DateTime DateOfBirth;
}
```
Результат:
```xml
<Person Age="32"
        Name="Alexander"
        xmlns:xsd="http://www.w3.org/2001/XMLSchema"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <DateOfBirth>0001-01-01T00:00:00</DateOfBirth>
</Person>
```

Дополнительно можно назначить имя для элемента/атрибута:
```c#
public class Person
{
    [XmlAttribute("FirstName")] public string Name;
    [XmlAttribute("RoughAge")] public int Age;

    [XmlElement("DOB")]
    public System.DateTime DateOfBirth;
}
```
Заданы специальные имена:
```xml
<Person RoughAge="32"
        FirstName="Alexander"
        ... >
    <DOB>0001-01-01T00:00:00</DOB>
</Person>
```

Так как, по умолчанию XML сериализатор не заполняет стандартное пространство
имён, для его указания атрибутам ```XmlElement``` и ```XmlAttribute``` назначается
соответствующий аргумент ```Namespace```.

Также, с применением атрибута ```XmlRoot```, можно выбрать имя и пространство имён
типу:
```c#
[XmlRoot("Candidate", Namespace = "http://mynamespace/test/")]
public class Person
{ ... }
```
_______________________________________________________________________________
# Упорядочение XML-элементов
_______________________________________________________________________________

XML сериализатор сохраняет элементы в порядке их определения в классе. Порядок
можно задавать через значение для аргумента ```Order``` в атрибуте ```XmlElement```:
```c#
public class Person
{
    [XmlElement(Order = 2)] public string Name;
    [XmlElement(Order = 1)] public int Age;
    ...
}
```
*аргумент ```Order``` при использовании выставляется всем участникам 

Порядок элементов изменён:
```xml
<Person ... >
    <Age>32</Age>
    <Name>Alexander</Name>
    ...
</Person>
```
Порядок следования элементов не оказывает влияния на десериализацию, при любой
установленной последовательности тип будет гарантировано десериализирован.
_______________________________________________________________________________
# Подклассы
_______________________________________________________________________________

