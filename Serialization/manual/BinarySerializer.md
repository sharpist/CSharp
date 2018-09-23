## Д в о и ч н ы й&nbsp;&nbsp;&nbsp;&nbsp;с е р и а л и з а т о р

#### Содержание: ####

[Двоичный сериализатор](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/BinarySerializer.md#Двоичный-сериализатор)

[Атрибуты двоичной сериализации](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/BinarySerializer.md#Атрибуты-двоичной-сериализации)

[Интерфейс ISerializable двоичной сериализации](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/BinarySerializer.md#Интерфейс-iserializable-двоичной-сериализации)

[Особенности сериализации подклассов](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/BinarySerializer.md#Особенности-сериализации-подклассов)
_______________________________________________________________________________
# Двоичный сериализатор
_______________________________________________________________________________

#### Использование: ####

Классы для сериализации и десериализации объектов содержатся в пространстве
имён ```System.Runtime.Serialization```.

1. Применить к типу атрибут ```SerializableAttribute``` или реализовать интерфейс
```ISerializable```, чтобы указать возможность сериализации экземпляров этого типа:
* атрибут ```SerializableAttribute``` не наследуется подклассами, подклассы не
являются по умолчанию сериализируемыми.
* если сериализуемый класс содержит ссылки на объекты других классов, отмеченные
```SerializableAttribute```, эти объекты тоже будут сериализованы.
* сериализируются только резервные поля автоматически реализуемых свойств.
* двоичная сериализация реализует политику отключения (opt-out), поэтому
участники класса, не подлежащие сериализации, отмечаются явно.
* атрибут ```NonSerializedAttribute``` выставит поле в классе как не подлежащее
сериализации.
* все поля должны поддерживать сериализацию подобно примитивным типам .NET
(```int```, ```string```).

```c#
[Serializable] // по умолчанию вкл. все закрытые
// и открытые поля (кроме свойств) в данном типе
public sealed class Person
{
    public string Name;
    public int Age;
}
```

2. Создать файл или открыть на чтение в файловом потоке.

3. Воспользоваться методами ```Serialize(Stream, Object)``` или ```Deserialize(Stream)```
экземпляра класса форматера (```BinaryFormatter``` или ```SoapFormatter```), предоставляющего
базовую функциональность для форматеров сериализации среды CLR:
* сериализированные данные содержат полные сведения о типе и сборке.
* десериализатор пропускает инициализаторы полей и конструкторы.
* десериализатор полностью восстанавливает объектные ссылки. В том числе,
распространяется на коллекции (которые в пространстве имён ```System.Collections.*```
отмечены как сериализируемые).

#### Форматеры двоичной сериализации: ####

```BinaryFormatter``` – самый эффективный и функциональный форматер,
генерирующий вывод меньшего размера за кротчайшее время. Поддерживает
обобщённые типы и фильтрацию несовместимых данных.
Определён в пространстве имён ```System.Runtime.Serialization.Formatters.Binary```

```SoapFormatter``` – поддерживает использование Remoting одновременно с базовым
обменом сообщениями SOAP. Не осуществляет обобщённые типы и фильтрацию
несовместимых данных.
Определён в пространстве имён ```System.Runtime.Serialization.Formatters.Soap```

#### Выполнить сериализацию и десериализацию объекта тапа ```Person```: ####
```c#
var person = new Person { Name = "Alexander", Age = 32 };

// сериализация
IFormatter formatter = new BinaryFormatter();
using (var stream = File.Create("person.bin"))
    formatter.Serialize(stream, person);

// десериализация
using (var stream = File.OpenRead("person.bin"))
{
    Person p = (Person)formatter.Deserialize(stream);
    WriteLine("{0} {1}", p.Name, p.Age); // Alexander 32
}
```
Двоичная сериализация корректно обрабатывает сложные графы объектов, без
специальной поддержки (кроме обеспечения возможности сериализации всех
участников класса).
_______________________________________________________________________________
# Атрибуты двоичной сериализации
_______________________________________________________________________________

Атрибут ```NonSerialized``` регистрирует поле, которое не должно сериализироваться.
Несериализированные участники при десериализации получают пустое значение или null,
инициализаторы полей и конструкторы игнорируются:
```c#
[Serializable]
public sealed class Person
{
    public string Name;
    public DateTime DateOfBirth;

    [NonSerialized] // поле Age вычисляется
    // нет необходимости в его сериализации
    public int Age;
}
```

#### Ловушки сериализации и десериализации: ####

Группа атрибутов для pre/post вызова служебных (закрытых) методов, выполняющих
обработку, выходящих за рамки сериализации, участников класса.

* ```OnDeserializing``` метод перед десериализацией.
* ```OnDeserialized``` метод после десериализации.

Метод ```OnDeserializing``` может применяться как псевдоконструктор для
десериализации и инициализации полей исключённых из сериализации.

Например, в классе ```Person```, задействованы ```OnDeserializing``` и ```OnDeserialized``` методы для
установки требуемого значения поля ```Valid``` и инициализации вычисляемого поля ```Age```
соответственно, в десериализированном экземпляре:
```c#
[Serializable]
public sealed class Person
{
    public string Name;
    public DateTime DateOfBirth;

    [NonSerialized] // поле Age вычисляется
    // нет необходимости в его сериализации
    public int Age;

    [NonSerialized] // после десериализации
    // поле Valid = false сбросив 2 попытки
    public bool Valid = true;
    public Person() { Valid = true; }


    [OnDeserializing] // теперь поле Valid = true
    void OnDeserializing(StreamingContext sc)
    {
        Valid = true;
    }

    [OnDeserialized] // вычисление поля Age
    void OnDeserialized(StreamingContext sc)
    {
        TimeSpan ts = DateTime.Now - DateOfBirth;
        Age = ts.Days / 365;
    }
}
```

* ```OnSerializing``` метод перед сериализацией.
* ```OnSerialized``` метод после сериализации.

Метод ```OnSerializing``` может применяться для превентивной сериализации полей.

Например, класс ```Team```, способен сериализироваться и десериализироваться только с
двоичным форматером, так как SOAP форматер не поддерживает сериализацию
обобщённых типов:
```c#
[Serializable]
public sealed class Team
{
    public string Name;
    public List<Person> Players = new List<Person>();
}
```
Можно перед сериализацией преобразовать список ```Players``` в массив,
используя ```OnSerializing``` метод.
После десериализации выполнить обратное преобразование, применив ```OnSerialized```
метод:
```c#
[Serializable]
public sealed class Team
{
    public string Name;
    [NonSerialized] // поле Players
    // не подлежит сериализации
    public List<Person> Players = new List<Person>();

    // поле для хранения массива
    Person[] _playersToSerialize;


    [OnSerializing] void OnSerializing(StreamingContext sc)
    {
        _playersToSerialize = Players.ToArray();
    }

    [OnSerialized] void OnSerialized(StreamingContext sc)
    {
        _playersToSerialize = null;
    }

    [OnDeserialized] void OnDeserialized(StreamingContext sc)
    {
        Players = new List<Person>(_playersToSerialize);
    }
}
```

#### Поддержка версий: ####

1. Конфликт обратной совместимости – возникает, когда десериализатору не
удалось обнаружить ожидаемое поле в потоке сериализации.

Атрибут ```OptionalField``` служит для добавления нового поля в сериализированные
данные без потери совместимости.

Например, чтобы добавить новое поле в имеющуюся версию класса ```Person```:
```c#
[Serializable]
public sealed class Person
{
    public string Name;
}
```
Следует декорировать добавляемое поле атрибутом ```OptionalField```:
```c#
[Serializable]
public sealed class Person
{
    public string Name;

    [OptionalField(VersionAdded = 2)] // сохранена совместимость версий
    public DateTime DateOfBirth;
}
```
*при недостатке значения ```DateOfBirth``` в потоке данных, данное поле считается
несериализированным

2. Конфликт прямой совместимости – возникает, когда десериализатор встречает
неожиданное поле и не способен корректным образом его обработать (проблема
характерна при двухсторонних коммуникациях):
* двоичный форматер отбрасывает посторонние данные.
* SOAP форматер генерирует исключение.

Двоичный форматер предоставляет лучшую поддержку версий при двухсторонних
коммуникациях.
_______________________________________________________________________________
# Интерфейс ```ISerializable``` двоичной сериализации
_______________________________________________________________________________

Интерфейс ```ISerializable``` расширяет контроль над двоичной сериализацией и
десериализацией, при помощи определения единственного метода ```GetObjectData```,
который, при сериализации, наполняет объект ```SerializationInfo``` данными из
сериализируемых полей:
```c#
public interface ISerializable
{
    void GetObjectData(SerializationInfo info, StreamingContext context);
}
```
#### Реализация интерфейса ```ISerializable```: ####

Тип, управляющий собственной сериализацией, должен предоставить метод
```GetObjectData``` и конструктор десериализации:
```c#
public virtual void GetObjectData(SerializationInfo si,
                                   StreamingContext sc)
{
    // пары "имя/значение"
    // для сериализируемых полей
    si.AddValue("Name", Name);
    si.AddValue("DateOfBirth", DateOfBirth);
}
```
При помощи интерфейса ```ISerializable``` достигнута совместимость с SOAP форматером:
```c#
[Serializable]
public class Team : ISerializable
{
    public string Name;
    public List<Person> Players;

    public virtual void GetObjectData(SerializationInfo si,
                                       StreamingContext sc)
    {
        si.AddValue("Name", Name);
        si.AddValue("PlayerData", Players.ToArray());
    }

    public Team() { }
    // конструктор десериализации
    protected Team(SerializationInfo si,
                    StreamingContext sc)
    {
        Name = si.GetString("Name");
        // десериализировать Players в массив для соответствия сериализации
        var array = (Person[])si.GetValue("PlayerData", typeof(Person[]));
        // сконструировать новый список
        Players = new List<Person>(array);
    }
}
```

В ```Get*``` методах класса ```SerializationInfo``` для предупреждения несовпадения
версий получаемых по имени данных, можно ввести свою систему нумерации версий:
```c#
public string NewName;
...

public virtual void GetObjectData(SerializationInfo si,
                                   StreamingContext sc)
{
    si.AddValue("version", 2);
    si.AddValue("NewName", NewName);
    ...
}

// конструктор десериализации
protected Team(SerializationInfo si,
                StreamingContext sc)
{
    int version = si.GetInt32("version");
    if (version >= 2) NewName = si.GetString("NewName");
    ...
}
```
_______________________________________________________________________________
# Особенности сериализации подклассов
_______________________________________________________________________________

Если сериализируемый класс, вместо реализации интерфейса ```ISerializable```
изначально полагается на атрибуты для сериализации, не являясь при этом
запечатанным, тогда отложенная реализация интерфейса ```ISerializable``` может
нарушать сериализацию экземпляров его подклассов.

Поскольку, реализации метода ```ISerializable.GetObjectData``` в базовом классе
ничего не известно об участниках производного класса, в котором к тому же
отсутствует конструктор десериализации:
```c#
[Serializable]
public class Person : ISerializable
{
    public string Name;
    public int Age;

    public virtual void GetObjectData(SerializationInfo si,
                                       StreamingContext sc)
    {
        si.AddValue("Name", Name);
        si.AddValue("Age", Age);
    }

    protected Person(SerializationInfo si,
                      StreamingContext sc)
    {
        Name = si.GetString("Name");
        Age = si.GetInt32("Age");
    }
    public Person() { }
}

[Serializable]
public sealed class Customer : Person
{
    // поле Group не сохраняется в потоке!
    public string Group;
}
```
Проблема решается изначальной реализацией интерфейса ```ISerializable``` в
сериализируемых незапечатанных классах:
```c#
[Serializable]
public class Customer : Person
{
    public string Group;

    public override void GetObjectData(SerializationInfo si,
                                        StreamingContext sc)
    {
        base.GetObjectData(si, sc);
        si.AddValue("Group", Group);
    }

    protected Customer(SerializationInfo si,
                        StreamingContext sc)
        : base(si, sc)
    {
        Group = si.GetString("Group");
    }
    public Customer() { }
}
```
