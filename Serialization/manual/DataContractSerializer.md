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
применяют форматер XML.

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

#### Тип данных должен быть: ####
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
вмешательства.
Подклассы должны иметь атрибут ```DataContract```:
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

#### ```DataContractSerializer``` необходимо проинформировать о всех подклассах. ####
Таким образом, для обеспечения безопасности предотвращается десериализация
непредвиденных типов.

Указать все разрешённые "известные" подклассы можно при создании экземпляра
```DataContractSerializer```:
```c#
var dcs = new DataContractSerializer(typeof(Person),
    new Type[] { typeof(Student), typeof(Teacher) }); // подтипы Student и Teacher
```
Это можно также сделать в корневом типе при помощи атрибута ```KnownType```:
```c#
[DataContract, KnownType(typeof(Student)), KnownType(typeof(Teacher))]
public class Person { ... }
```
_______________________________________________________________________________
# Объектные ссылки
_______________________________________________________________________________

Все ссылки на другие объекты также сериализируются. При создании подклассов
```Address``` применяются те же правила, что и при создании подклассов
корневого типа:
```c#
[DataContract]
public class Person
{
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public int Age { get; set; }
    [DataMember]
    public Address HomeAddress { get; set; }
}

[DataContract, KnownType(typeof(USAddress))]
public class Address
{
    [DataMember]
    public string Street { get; set; }
    [DataMember]
    public string Postcode { get; set; }
}

[DataContract]
public class USAddress : Address
{ ... }
```
Можно сообщить экземпляру ```DataContractSerializer``` о классе ```USAddress```:
```c#
var dcs = new DataContractSerializer(typeof(Person),
    new Type[] { typeof(USAddress) }); // подтип USAddress
```
*сообщать о классе ```Address``` нет необходимости, так как он является объявленным
типом члена ```HomeAddress```
_______________________________________________________________________________
# Предохранение объектных ссылок
_______________________________________________________________________________

```NetDataContractSerializer``` всегда предохраняет эквивалентность ссылок.

#### ```DataContractSerializer``` предохраняет только по требованию. ####

Когда на объект имеются ссылки из разных мест, ```DataContractSerializer``` запишет
его многократно.

Таким образом, изменив пример так, чтобы класс ```Person``` также хранил рабочий адрес:
```c#
[DataContract]
public class Person
{
    ...
    [DataMember]
    public Address HomeAddress { get; set; }
    [DataMember]
    public Address WorkAddress { get; set; }
}
```
Проинициализировав его экземпляр следующим образом:
```c#
var person = new Person { Name = "Alexander", Age = 32 };
person.HomeAddress = new Address { Street = "Yaroslavl", Postcode = "150000" };
person.WorkAddress = person.HomeAddress; // теперь две ссылки на объект
```
После сериализации получим задвоение деталей адреса:
```xml
<Person ... >
    ...
    <HomeAddress>
        <Postcode>150000</Postcode>
        <Street>Yaroslavl</Street>
    </HomeAddress>
    ...
    <WorkAddress>
        <Postcode>150000</Postcode>
        <Street>Yaroslavl</Street>
    </WorkAddress>
</Person>
```
Следовательно, при последующей десериализации ```HomeAddress``` и ```WorkAddress``` это уже
два разных объекта. Теряется ссылочная целостность и пригодность циклических
ссылок.

Получить ссылочную целостность можно, указав ```true``` для аргумента
```preserveObjectReferences``` при конструировании ```DataContractSerializer```:
```c#
var dcs = new DataContractSerializer(typeof(Person),
    null, 1000, false, true, null); // preserveObjectReferences = true
```
*третий аргумент (в примере = 1000) обязателен, задаёт максимум объектных
ссылок, которые сериализатор отслеживает (при превышении выдаёт исключение,
предотвращая атаку типа отказа в обслуживании)

Результат выглядит следующим образом:
```xml
<Person ...
        xmlns:z="http://schemas.microsoft.com/2003/10/Serialization/"
        z:Id="1">
    ...
    <HomeAddress z:Id="2">
        <Postcode z:Id="3">150000</Postcode>
        <Street z:Id="4">Yaroslavl</Street>
    </HomeAddress>
    ...
    <WorkAddress i:nil="true" z:Ref="2"/>
</Person>
```
*добавлено патентованное пространство имён для атрибутов ```Id``` и ```Ref```
(```HomeAddress``` как и ```WorkAddress``` ссылаются на уникальный объект)
_______________________________________________________________________________
# Переносимость версий
_______________________________________________________________________________

Участники класса могуть добавляться или удалаться без нарушения прямой/обратной
совместимоти. Десериализаторы контрактов данных пропускают данные без атрибута
```DataMember``` и не замечают, когда в потоке сериализации отсутствуют данные, для
которых в классе предусмотрен атрибут ```DataMember```.

Реализовав интерфейс ```IExtensibleDataObject``` нераспознанные данные можно записать
в "чёрный ящик" для дальнейшего восстановления при повторной сериализации.
```c#
[DataContract]
public class Person : IExtensibleDataObject
{
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public int Age { get; set; }
    // добавлено единственное свойство установки "чёрного ящика"
    ExtensionDataObject IExtensibleDataObject.ExtensionData { get; set; }
}
```

Однако, для критических участников типа, можно затребовать форсированное
присутствие, указав атрибут ```IsRequired```:
```c#
...
[DataMember(IsRequired = true)]
public int ID { get; set; }
```
_______________________________________________________________________________
# Упорядочение членов
_______________________________________________________________________________

Сериализаторы контрактов данных чувствительны к порядку следования членов,
десериализаторы пропускают любые неупорядоченные члены.

#### При сериализации члены записываются в порядке: ####

1. От базового класса к подклассу.

2. От низких значений ```Order``` к высоким значениям ```Order```.

3. Алфавитный порядок.

Поэтому в показанных выше примерах ```Age``` располагается перед ```Name```.
Элемент ```Order``` может позиционировать ```Name``` перед ```Age```:
```c#
[DataContract]
public class Person
{
    [DataMember(Order = 0)]
    public string Name { get; set; }
    [DataMember(Order = 1)]
    public int Age { get; set; }
}
```
Результат:
```xml
<Person ... >
<Name>Alexander</Name>
<Age>32</Age>
</Person>
```
Порядок может задаваться для достижения определённой схемы XML.
Одновременно, порядок следования XML-элементов сопоставляется с порядком
следования участников класса (конфликтов не возникнет).

Если не требуется определённая форма взаимодействия ```Order``` можно не указывать.

Когда применяется упорядочение по алфавиту расхождение между сериализацией и
десериализацией при добавлении и удалении членов не произойдёт (за исключением
перемещения участников класса между базовым классом и подклассом).
_______________________________________________________________________________
# Пустые значения и null
_______________________________________________________________________________

#### Два сценария обработки участника класса с пустым значением или null: ####
* записать явно пустое значение или null.
```xml
<Name i:nil="true"/>
```
* исключить член из вывода сериализации.

Привести сериализатор к сокрытию данных участников класса можно выставив ```false```
для свойства ```EmitDefaultValue```:
```c#
[DataContract]
public class Person
{
    [DataMember(EmitDefaultValue = false)]
    public string Name { get; set; }
    [DataMember(EmitDefaultValue = false)]
    public int Age { get; set; }
}
```
Выгодно, для экономии и реализации XML-схемы, ожидающей применения
необязательных элементов.

Так, член ```Name``` исключается, если значение null, а член ```Age``` – если 0.
```c#
[DataContract]
public class Person
{
    [DataMember(EmitDefaultValue = false)]
    public string Name { get; set; } = "Alexander";
    [DataMember(EmitDefaultValue = false)]
    public int Age { get; set; }     = 0;
}
```
```xml
<Person ... >
<Name>Alexander</Name>
</Person>
```
*отсутствует запись ```<Age>32</Age>```

