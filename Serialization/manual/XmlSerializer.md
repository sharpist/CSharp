## X M L&nbsp;&nbsp;&nbsp;&nbsp;с е р и а л и з а т о р

#### Содержание: ####

[XML сериализатор](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#xml-сериализатор)

[Атрибуты XML сериализации](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Атрибуты-xml-сериализации)

[Упорядочение XML-элементов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Упорядочение-xml-элементов)

[Подклассы](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Подклассы)

* [Создание подкласcов из корневого типа](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Создание-подкласcов-из-корневого-типа)

* [Сериализация дочерних объектов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Сериализация-дочерних-объектов)

* [Создание подклассов из дочерних объектов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Создание-подклассов-из-дочерних-объектов)

* [Сериализация коллекций](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Сериализация-коллекций)

* [Подклассы в качестве элементов коллекции](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/XmlSerializer.md#Подклассы-в-качестве-элементов-коллекции)
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
установленной последовательности тип будет гарантированно десериализирован.
_______________________________________________________________________________
# Подклассы
_______________________________________________________________________________

#### Создание подкласcов из корневого типа: ####

Корневой тип имеет два подкласса ```Student``` и ```Teacher```:
```c#
public class Person { public string Name; }
public class Student : Person { }
public class Teacher : Person { }
```

Класс ```Person``` включает реализацию метода сериализации корневого типа:
```c#
public class Person
{
    public string Name;

    public void SerializePerson(Person p, string path)
    {
        var xs = new XmlSerializer(typeof(Person));
        using (var s = File.Create(path))
            xs.Serialize(s, p);
    }
}
...
```

Но для того, чтобы включённый метод работал также с объектом типа ```Student``` или
```Teacher```, требуется задекларировать существующие подклассы для экземпляра типа
```XmlSerializer```.

Установить атрибут ```XmlInclude``` указывающий на подклассы:
```c#
[XmlInclude(typeof(Student))]
[XmlInclude(typeof(Teacher))]
public class Person
{ ... }
```
Или регистрировать подтипы при конструировании экземпляра ```XmlSerializer```:
```c#
var xs = new XmlSerializer(typeof(Person),
    new System.Type[] { typeof(Student), typeof(Teacher) });
```

Демонстрация:
```c#
var p = new Person();
var s = new Student { Name = "Alexander" };
...
s.SerializePerson(s, "person.xml");
```
И результат:
```xml
<Person xsi:type="Student"
        xmlns:xsd="http://www.w3.org/2001/XMLSchema"
        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <Name>Alexander</Name>
</Person>
```
*подтип помещён в атрибут ```type```

Исследуя атрибут десериализатор создаст объект соответствующего типа ```Student```
вместо ```Person```.

Имя типа в XML-атрибуте ```type``` можно переназначать применяя к подклассу атрибут
```XmlType```:
```c#
...
[XmlType("Candidate")]
public class Student : Person { }
```

#### Сериализация дочерних объектов: ####

XML сериализатор автоматически рекурсивно обрабатывает все объектные ссылки:
```c#
public class Person
{
    public string Name;
    // поле HomeAddress – объектная ссылка
    public Address HomeAddress = new Address();
    ...
}

public class Address { public string Street, PostCode; }
```

Выполнив инициализацию следующим образом:
```c#
var p = new Person { Name = "Alexander" };
p.HomeAddress.Street = "prospect Dzerzhinsky";
p.HomeAddress.PostCode = "150044";
```
Получаем вывод:
```xml
<Person ... >
    <Name>Alexander</Name>
    <HomeAddress>
        <Street>prospect Dzerzhinsky</Street>
        <PostCode>150044</PostCode>
    </HomeAddress>
</Person>
```

В ситуациях, когда два и более поля или свойства ссылаются на один идентичный
объект, данный объект сериализируется многократно.

Следовательно, если предохранение объектных ссылок критично важно, нужен другой
механизм сериализации.

XML сериализация не обеспечивает предохранение объектных ссылок.

#### Создание подклассов из дочерних объектов: ####

Класс ```Person``` ссылается на подклассы ```Address```:
```c#
public class Person
{
    public string Name;
    public Address HomeAddress = new USAddress();
}

public class Address { public string Street, PostCode; }
public class USAddress : Address { }
public class AUAddress : Address { }
```
Выполнить сериализацию класса ```Person```, в зависимости от требуемой формы
взаимодействия и структуры XML, можно различными способами. 

Чтобы имя XML-элемента соответствовало имени поля или свойства с подтипом,
записанным в атрибуте ```type```:
```xml
<Person ... >
    ...
    <HomeAddress xsi:type="USAddress">
        ...
    </HomeAddress>
</Person>
```
Необходимо добавить атрибут ```XmlInclude``` для декларирования подклассов с классом
```Address```:
```c#
[XmlInclude(typeof(USAddress))]
[XmlInclude(typeof(AUAddress))]
public class Address { public string Street, PostCode; }
...
```

Чтобы имя XML-элемента соотносилось с именем подтипа:
```xml
<Person ... >
    ...
    <USAddress>
        ...
    </USAddress>
</Person>
```
Следует добавить атрибут ```XmlElement``` полю или свойству базового класса:
```c#
public class Person
{
    public string Name;
    [XmlElement("Address", typeof(Address))]
    [XmlElement("USAddress", typeof(USAddress))]
    [XmlElement("AUAddress", typeof(AUAddress))]
    public Address HomeAddress = new USAddress();
}
```
*если в атрибуте ```XmlElement``` не указано имя, берётся стандартное имя типа

#### Сериализация коллекций: ####

XML сериализатор поддерживает автономную сериализацию конкретных типов
коллекций.
Класс ```Person``` определяет в качестве участника класса список ```Addresses```:
```c#
public class Person
{
    public string Name;
    public List<Address> Addresses = new List<Address>();
}

public class Address { public string Street, PostCode; }
```
Следующий вывод получен без вспомогательных действий:
```xml
<Person ... >
    <Name>...</Name>
    <Addresses>
        <Address>
            <Street>...</Street>
            <PostCode>...</PostCode>
        </Address>
        ...
    </Addresses>
</Person>
```

Внешний XML-элемент (Addresses) можно переименовать, применив к полю или
свойству коллекции атрибут ```XmlArray```. Атрибут ```XmlArrayItem``` переназначает имена
для внутренних XML-элементов:
```c#
public class Person
{
    public string Name;
    [XmlArray("AddressDirectory")]
    [XmlArrayItem("Location")]
    public List<Address> Addresses = new List<Address>();
}
```
*показанные атрибуты позволяют указать пространства имён XML

Результат после переназначения:
```xml
<Person ... >
    <Name>...</Name>
    <AddressDirectory>
        <Location>
            <Street>...</Street>
            <PostCode>...</PostCode>
        </Location>
        ...
    </AddressDirectory>
</Person>
```

Чтобы исключить внешний XML-элемент из сериализации, нужно добавить к полю или
свойству коллекции атрибут ```XmlElement```:
```c#
public class Person
{
    public string Name;
    [XmlElement("Address")]
    public List<Address> Addresses = new List<Address>();
}
```
```xml
<Person ... >
    <Name>...</Name>
    <Address>
        <Street>...</Street>
        <PostCode>...</PostCode>
    </Address>
    ...
</Person>
```

#### Подклассы в качестве элементов коллекции: ####

Когда элементы коллекции представляют подклассы, используются правила,
распространяемые на подклассы.

Например, ```Addresses``` участник класса ```Person```, являющийся списком типа
```List<Address>``` – работает с типом ```Address``` и его производным ```USAddress``` подтипом.

Поэтому, чтобы закодировать все элементы-подклассы, базовому классу
устанавливаются атрибуты ```XmlInclude```, декларирующие все существующие подтипы:
```c#
public class Person
{
    public string Name;
    public List<Address> Addresses = new List<Address>();
}

[XmlInclude(typeof(USAddress))]
[XmlInclude(typeof(AUAddress))]
...
public class Address { public string Street, PostCode; }
public class USAddress : Address { }
public class AUAddress : Address { }
...
```
Теперь список ```Addresses``` может быть инициализирован следующим образом:
```c#
var p = new Person { Name = "Alexander" };
p.Addresses.Add(new USAddress
{
    Street   = "prospect Dzerzhinsky",
    PostCode = "150044"
});
p.Addresses.Add(new AUAddress
{
    Street   = "street Deputatskaya",
    PostCode = "150000"
});
...
```
Полученный вывод:
```xml
<Person ... >
    <Name>...</Name>
    <Addresses>
        <Address xsi:type="USAddress">
            <Street>...</Street>
            <PostCode>...</PostCode>
        </Address>
        <Address xsi:type="AUAddress">
            <Street>...</Street>
            <PostCode>...</PostCode>
        </Address>
        ...
    </Addresses>
</Person>
```

Чтобы наименования XML-элементов, являющихся подклассами, соответствовали
пренадлежащим типам:
```xml
<Person ... >
    <Name>...</Name>
    <!--необязательный внешний элемент-->
    <Addresses>
        <USAddress>
            <Street>...</Street>
            <PostCode>...</PostCode>
        </USAddress>
        <AUAddress>
            <Street>...</Street>
            <PostCode>...</PostCode>
        </AUAddress>
        ...
    <!--необязательный внешний элемент-->
    </Addresses>
</Person>
```
Необходимо полю или свойству коллекции задать атрибуты ```XmlArrayItem```, если нужно
включить внешний элемент коллекции или ```XmlElement```, чтобы исключить:
```c#
[XmlArrayItem("Address", typeof(Address))]
[XmlArrayItem("USAddress", typeof(USAddress))]
[XmlArrayItem("AUAddress", typeof(AUAddress))]
public List<Address> Addresses = new List<Address>();
```
*включить внешний элемент коллекции
