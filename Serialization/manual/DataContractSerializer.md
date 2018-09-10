_______________________________________________________________________________
# Сериализатор контрактов данных
_______________________________________________________________________________

#### Использование: ####

1. Выбрать класс сериализатор – ```DataContractSerializer``` или
```NetDataContractSerializer```:
* ```DataContractSerializer``` обеспечивает слабую привязку типов .NET к типам
контрактов данных, поэтому требуется предварительная явная регистрация
сериализируемых подтипов (производных типов), для сопоставления имени контракта
данных с корректным типом .NET.
* ```DataContractSerializer``` предохраняет эквивалентность ссылок только по
требованию.
* ```NetDataContractSerializer``` осуществляет тесную привязку типов .NET к типам
контрактов данных, записывая полные имена типов и сборок для сериализируемых
типов, не требуется пререгистрация
(подобный вывод является патентованным, следовательно, при десериализации он
также полагается на наличие определённого типа .NET в конкретном пространстве
имён и сборке).
* ```NetDataContractSerializer``` всегда предохраняет эквивалентность ссылок.

2. Декорировать типы и члены, подлежащие сериализации, атрибутами ```DataContract```
и ```DataMember``` соответственно (это установит тип в неявно сериализуемый).

3. Создать экземпляр сериализатора и вызвать его метод ```WriteObject``` или
```ReadObject``` для явной сериализации и десериализации:
```c#
using System.IO;
using System.Runtime.Serialization;
using static System.Console;

class Program
{
    static void Main()
    {
        var person = new Person { Name = "Alexander", Age = 32 };
        var dcs = new DataContractSerializer(typeof(Person));
        using (var stream = File.Create("person.xml"))
            dcs.WriteObject(stream, person); // сериализировать

        Person p;
        using (var stream = File.OpenRead("person.xml"))
            p = (Person)dcs.ReadObject(stream); // десериализировать

        WriteLine("{0} {1}", p.Name, p.Age); // Alexander 32
    }
}

[DataContract]
public class Person
{
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public int Age { get; set; }
}
```
*конструктор ```DataContractSerializer``` (в отличие от ```NetDataContractSerializer```)
требует указания типа корневого объекта, который явно сериализируется, в данном
примере – ```Person```
_______________________________________________________________________________
# Работа с сериализатором
_______________________________________________________________________________

Сериализаторы ```DataContractSerializer``` и ```NetDataContractSerializer``` по умолчанию
примениют форматер XML.

Работая с классом ```XmlWriter``` можно добавить в вывод отступы для лучшей
читаемости:
```c#
var person = new Person { Name = "Alexander", Age = 32 };
var dcs = new System.Runtime.Serialization.DataContractSerializer(typeof(Person));

var settings = new System.Xml.XmlWriterSettings() { Indent = true };
using (var writer = System.Xml.XmlWriter.Create("person.xml", settings))
    dcs.WriteObject(writer, person); // сериализировать

System.Diagnostics.Process.Start("person.xml");
/* Output:
<?xml version="1.0" encoding="ISO-8859-1"?>
<Person xmlns="http://schemas.datacontract.org/2004/07/"
        xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
    <Age>32</Age>
    <Name>Alexander</Name>
</Person>
*/
```

Где имя XML-элемента ```<Person>``` есть имя контракта данных, по умолчанию
соответствующее имени типа .NET, можно переопределить явно задав имя контракта
данных:
```c#
[DataContract(Name = "Candidate")]
public class Person { ... }
```

Пространство имён XML отражает пространство имён контракта данных
```http://schemas.datacontract.org/2004/07/```, а также пространство имён типа .NET.
Можно переопределять в известной форме:
```c#
[DataContract(Namespace = "http://github.com/sharpist/")]
public class Person { ... }
```
*указание имени и пространства имён отменяет связь между идентичностью
контракта и именем типа .NET, гарантируя что сериализация не будет затронута,
при изменении имени или пространства имён

Переопределяются имена данных-членов:
```c#
[DataContract(Name = "Candidate", Namespace = "http://github.com/sharpist/")]
public class Person
{
    [DataMember(Name = "FirstName")]
    public string Name { get; set; }
    [DataMember(Name = "ClaimedAge")]
    public int Age { get; set; }
}
```

Атрибут ```DataMember``` поддерживает как открытые, так и закрытые поля и свойства.
Тип данных должен быть:
* любой примитивный тип.
* ```DateTime```, ```TimeSpan```, ```Guid```, ```Uri```, ```Enum```.
* версии, допускающие ```null```, указанных выше типов.
* ```byte[]```.
* любой "известный" тип, отмеченный ```DataContract```.
* любой тип, реализующий интерфейс ```IEnumerable```.
* любой тип с атрибутом ```Serializable``` или реализующий интерфейс ```ISerializable```.
* любой тип, реализующий интерфейс ```IXmlSerializable```.
_______________________________________________________________________________
# Указание двоичного форматера
_______________________________________________________________________________

Применение двоичного форматера совместно с объектом ```DataContractSerializer``` либо
```NetDataContractSerializer```:
```c#
var person = new Person { Name = "Alexander", Age = 32 };
var dcs = new DataContractSerializer(typeof(Person));

using (var stream = new MemoryStream())
{
    using (var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
        dcs.WriteObject(writer, person); // сериализировать

    Person p;
    using (var stream2 = new MemoryStream(stream.ToArray()))
    {
        using (var reader = XmlDictionaryReader.CreateBinaryReader(
            stream2, XmlDictionaryReaderQuotas.Max))
            p = (Person)dcs.ReadObject(reader); // десериализировать
    }
    WriteLine("{0} {1}", p.Name, p.Age); // Alexander 32
}
```
_______________________________________________________________________________
# Сериализация подклассов
_______________________________________________________________________________

```NetDataContractSerializer``` поддерживает сериализацию подклассов без отдельного
вмешательства. Подклассы должны иметь атрибут ```DataContract```:
```c#
[DataContract]
public class Person
{
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public int Age { get; set; }
}

[DataContract]
public class Student : Person
{ ... }

[DataContract]
public class Teacher : Person
{ ... }
```
*при сериализации подтипов ```NetDataContractSerializer``` показывает низкую
производительность

```DataContractSerializer``` необходимо информировать о всех подтипах. Таким
образом, для обеспечения безопасности предотвращается десериализация
непредвиденных типов.

Указать все разрешённые "известные" подтипы можно при создании экземпляра
```DataContractSerializer```:
```c#
var dcs = new DataContractSerializer(typeof(Person),
    new Type[] { typeof(Student), typeof(Teacher) }); // подтипы Student и Teacher
```
Это можно также сделать в самом типе при помощи атрибута ```KnownType```:
```c#
[DataContract, KnownType(typeof(Student)), KnownType(typeof(Teacher))]
public class Person { ... }
```
