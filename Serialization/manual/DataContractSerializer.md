## С е р и а л и з а т о р&nbsp;&nbsp;&nbsp;к о н т р а к т о в&nbsp;&nbsp;&nbsp;д а н н ы х

#### Содержание: ####

[Сериализатор контрактов данных](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Сериализатор-контрактов-данных)

[Работа с сериализатором](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Работа-с-сериализатором)

[Указание двоичного форматера](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Указание-двоичного-форматера)

[Сериализация подклассов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Сериализация-подклассов)

[Объектные ссылки](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Объектные-ссылки)

[Предохранение объектных ссылок](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Предохранение-объектных-ссылок)

[Переносимость версий](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Переносимость-версий)

[Упорядочение членов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Упорядочение-членов)

[Пустые значения и null](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Пустые-значения-и-null)

[Коллекции](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Коллекции)

[Подклассы в качестве элементов коллекций](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Подклассы-в-качестве-элементов-коллекций)

[Настройка имён коллекции и элементов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Настройка-имён-коллекции-и-элементов)

[Расширение контрактов данных](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Расширение-контрактов-данных)

[Взаимодействие через Serializable](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/DataContractSerializer.md#Взаимодействие-через-serializable)
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
_______________________________________________________________________________
# Коллекции
_______________________________________________________________________________

Сериализаторы контрактов данных способны сохранять и повторно заполнять
перечислимые коллекции.

Например, дополнив класс ```Person``` списком ```List<>``` адресов:
```c#
[DataContract]
public class Person
{
    ...
    [DataMember]
    public List<Address> Addresses { get; set; }
}

[DataContract]
public class Address
{
    [DataMember]
    public string Street { get; set; }
    [DataMember]
    public string Postcode { get; set; }
}
```
И проинициализировав следующим образом:
```c#
var person = new Person
{
    Addresses = new List<Address>
    {
        new Address { Street = "Yaroslavl", Postcode = "150000" },
        new Address { Street = "Voronezh",  Postcode = "394000" }
    }
};
```
Получаем результат сериализации:
```xml
<Person ... >
    <Addresses>
        <Address>
            <Postcode>150000</Postcode>
            <Street>Yaroslavl</Street>
        </Address>
        <Address>
            <Postcode>394000</Postcode>
            <Street>Voronezh</Street>
        </Address>
    </Addresses>
    ...
</Person>
```
*сериализатор не указывает тип коллекции, избегая потенциальной ошибки при
смене типа между сериализацией и десериализацией

Для конкретизации типа коллекции можно использовать интерфейс ```IList```. Однако,
десериализатор не имеет возможности узнать, какого конкретного типа требуется
создать объект.

Решением будет замена данного участника класса закрытым полем и предоставление
открытого свойства для доступа к нему:
```c#
[DataContract]
public class Person
{
    ...
    [DataMember(Name = "Addresses")]
    private List<Address> _addresses;
    public IList<Address> Addresses
    {
        get { return _addresses; }
        set { _addresses = value as List<Address>; }
    }
}
```
_______________________________________________________________________________
# Подклассы в качестве элементов коллекций
_______________________________________________________________________________

Сериализатор корректно обрабатывает элементы коллекции, являющиеся подклассами.
Для этого объявляются допустимые "известные" типы:

```c#
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
Добавив ```USAddress``` наравне с ```Address``` к списку адресов получим:
```xml
<Person ... >
    <Addresses>
        ...
        <Address>
            <Postcode>394000</Postcode>
            <Street>Voronezh</Street>
        </Address>
        <Address i:type="USAddress">
            <Postcode>440000</Postcode>
            <Street>Penza</Street>
        </Address>
    </Addresses>
    ...
</Person>
```
_______________________________________________________________________________
# Настройка имён коллекции и элементов
_______________________________________________________________________________

Для подкласса класса коллекции можно определить имя, описывающее каждый элемент:
```c#
[CollectionDataContract(ItemName = "Residence")]
public class AddressList : List<Address>
{ ... }

[DataContract]
public class Person
{
    ...
    [DataMember]
    public AddressList Addresses { get; set; }
}
```
```xml
...
    <Addresses>
        <Residence>
            <Postcode>150000</Postcode>
            <Street>Yaroslavl</Street>
        </Residence>
    ...
```
*атрибут ```CollectionDataContract``` имеет также аргументы ```Namespace``` и ```Name```, который
применяется когда коллекция сериализируется как корневой объект, а не свойство другого объекта

Управление сериализацией словарей:
```c#
[CollectionDataContract(ItemName  = "Entry",
                        KeyName   = "Kind",
                        ValueName = "Number")]
public class PhoneNumberList : Dictionary<string, string>
{ ... }

[DataContract]
public class Person
{
    ...
    [DataMember]
    public PhoneNumberList PhoneNumbers;
}
```
```xml
...
    <PhoneNumbers>
        <Entry>
            <Kind>Mobile</Kind>
            <Number>905 635 00 00</Number>
        </Entry>
        <Entry>
            <Kind>Home</Kind>
            <Number>4852 00 00 00</Number>
        </Entry>
    </PhoneNumbers>
...
```
_______________________________________________________________________________
# Расширение контрактов данных
_______________________________________________________________________________

#### Ловушки сериализации и десериализации: ####

Группа атрибутов для pre/post вызова служебных (закрытых) методов, выполняющих
обработку, выходящих за рамки сериализации, участников класса.

* ```OnSerializing``` метод перед сериализацией.
* ```OnSerialized``` метод после сериализации.
* ```OnDeserializing``` метод перед десериализацией.
* ```OnDeserialized``` метод после десериализации.

Пример использования атрибутов ```OnSerializing``` и ```OnDeserialized```:
```c#
[DataContract]
public class Person
{
    // специальная коллекция не подлежащая сериализации
    private SerializationUnfriendlyType addresses;

    // коллекция подлежащая сериализации
    [DataMember(Name = "Addresses")]
    private SerializationFriendlyType _serializationFriendlyAddresses;
    public SerializationFriendlyType Addresses
    {
        get { return _serializationFriendlyAddresses; }
        set { _serializationFriendlyAddresses = value; }
    }

    // запускается перед сериализацией
    [OnSerializing]
    private void prepareForSerialization(StreamingContext sc)
    {
        // ***копирование addresses в _serializationFriendlyAddresses***
        // addresses = new SerializationUnfriendlyType
        // {
        //     new Address { Street = "Voronezh",  Postcode = "394000" },
        //     new Address { Street = "Penza",     Postcode = "440000" }
        // };
        // addresses.ForEach(a => _serializationFriendlyAddresses.Add(a));
    }

    // запускается после десериализации
    [OnDeserialized]
    private void completeDeserialization(StreamingContext sc)
    {
        // ***копирование _serializationFriendlyAddresses в addresses***
        // addresses = new SerializationUnfriendlyType();
        // _serializationFriendlyAddresses.ForEach(a => addresses.Add(a));
    }
}
```
Инициализация экземпляра исходными значениями:
```c#
var person = new Person
{
    Addresses = new SerializationFriendlyType
    {
        new Address { Street = "Yaroslavl", Postcode = "150000" }
    }
};
```
Результат после сериализации:
```xml
...
    <Addresses>
        <Address>
            <Postcode>150000</Postcode>
            <Street>Yaroslavl</Street>
        </Address>
        <Address>
            <Postcode>394000</Postcode>
            <Street>Voronezh</Street>
        </Address>
        <Address>
            <Postcode>440000</Postcode>
            <Street>Penza</Street>
        </Address>
    </Addresses>
...
```

Метод ```OnSerializing``` может применяться для условной сериализации полей:
```c#
[DataContract]
public class Test
{
    // не подлежит сериализации
    public DateTime DateOfBirth;

    [DataMember] public bool Confidential;

    // условно сериализируется
    [DataMember(Name = "DateOfBirth", EmitDefaultValue = false)] // исключать значение по умолчанию
    DateTime? _tempDateOfBirth;

    // запускается перед сериализацией
    [OnSerializing]
    void prepareForSerialization(StreamingContext sc)
    {
        if (Confidential)
            _tempDateOfBirth = DateOfBirth;
        else
            _tempDateOfBirth = null;
    }
}
```

Метод ```OnDeserializing``` может применяться как псевдоконструктор для
десериализации и инициализации полей исключённых из сериализации
(десериализаторы контрактов данных пропускают инициализаторы полей и
конструкторы):
```c#
[DataContract]
public class Test
{
    bool _editable = true;
    public Test() { _editable = true; }
    
    // запускается перед десериализацией
    [OnDeserializing]
    void init(StreamingContext sc)
    {
        _editable = true;
    }
}
```
*без метода ```OnDeserializing``` поле ```_editable``` в десериализированном экземпляре
всегда будет ```false```

Подтипы могут реализовывать свои версии методов ловушек сериализации и
десериализации.
_______________________________________________________________________________
# Взаимодействие через ```Serializable```
_______________________________________________________________________________

Для обеспечения совместимости с двоичной сериализацией, широко используемой
в коде на платформе .NET Framework, сериализатор контрактов данных способен
сериализировать типы, отмеченные атрибутами механизма двоичной сериализации.

Помимо работы с старыми типами, двойное взаимодействие обслуживает типы, для
которых, требуются оба механизма сериализации, однако, совмещать атрибуты не
допускается.

Неотделимые от атрибутов механизма двоичной сериализации типы (```string```, ```DateTime```)
обрабатываются сериализатором контрактов данных отдельно с фильтрацией базовых
типов. Остальные типы для двоичной сериализации, обрабатываются по правилам
механизма двоичной сериализации (такие типы ожидают вкл. предохранения
объектных ссылок!).

Правила регистрации "известных" типов также применимы к объектам и подобъектам,
сериализуемым средствами двоичной сериализации.

Пример класса ```Person``` с ```Serializable``` участником класса:
```c#
[DataContract]
public class Person
{
    ...
    [DataMember] public Address MailingAddress;
}

[Serializable]
public class Address
{
    public string Street, Postcode;
}
```
Эффективно сформатированный результат сериализации:
```xml
<Person ... >
    ...
    <MailingAddress>
        <Postcode>150000</Postcode>
        <Street>Yaroslavl</Street>
    </MailingAddress>
...
```
