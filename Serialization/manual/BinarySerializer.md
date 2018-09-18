## Д в о и ч н ы й&nbsp;&nbsp;&nbsp;&nbsp;с е р и а л и з а т о р

#### Содержание: ####

[Двоичный сериализатор](https://github.com/sharpist/C_Sharp/blob/master/Serialization/manual/BinarySerializer.md#Двоичный-сериализатор)

[Атрибуты двоичной сериализации](https://github.com/)

[...](https://github.com/)

[...](https://github.com/)
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

2. Создать поток записи/открытия на чтение двоичного файла.

3. Воспользоваться методами ```Serialize(Stream, Object)``` и ```Deserialize(Stream)```
экземпляра класса форматера (BinaryFormatter или SoapFormatter), предоставляющего
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

Например, в классе Person, задействованы ```OnDeserializing``` и ```OnDeserialized``` методы для
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

Метод ```OnSerializing``` может применяться для условной и превентивной
сериализации полей.

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

