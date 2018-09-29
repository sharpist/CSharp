## И н т е р ф е й с&nbsp;&nbsp;&nbsp;&nbsp;I X m l S e r i a l i z a b l e

#### Содержание: ####

[Интерфейс IXmlSerializable]()

[...]()

[...]()
_______________________________________________________________________________
# Интерфейс IXmlSerializable
_______________________________________________________________________________

Интерфейс ```IXmlSerializable``` – расширяет сериализацию XML и контрактов данных для
сложных задач сериализации, предоставляя полный контроль над записываемым или
читаемым XML.

#### Определение интерфейса ```IXmlSerializable```: ####

```c#
public interface IXmlSerializable
{
    System.Xml.Schema.XmlSchema GetSchema();
    void ReadXml(System.Xml.XmlReader reader);
    void WriteXml(System.Xml.XmlWriter writer);
}
```

#### Требования интерфейса ```IXmlSerializable```: ####

* метод ```ReadXml``` должен считывать:
внешний начальный элемент -> содержимое -> внешний конечный элемент.
* метод ```WriteXml``` должен записывать только содержимоое.

#### Реализация интерфейса ```IXmlSerializable```: ####

Сериализация и десериализация экземпляра ```Address``` через XML сериализатор
осуществляется автоматическим вызовом методов ```WriteXml``` и ```ReadXml```.

```c#
public class Address : System.Xml.Serialization.IXmlSerializable
{
    public string Street, PostCode;

    public XmlSchema GetSchema() => null;
    public void ReadXml(XmlReader reader)
    {
        reader.ReadStartElement();
        Street   = reader.ReadElementContentAsString("Street",   "");
        PostCode = reader.ReadElementContentAsString("PostCode", "");
        reader.ReadEndElement();
    }
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("Street",   Street);
        writer.WriteElementString("PostCode", PostCode);
    }
}
```

Реализация интерфейса ```IXmlSerializable```, классом коллекции, замещает правила
XML сериализатора для сериализации коллекций, что позволяет сериализировать
коллекции с дополнительными полями или свойствами, которые иначе были бы
проигнорированы.

#### Использование интерфейса ```IXmlSerializable```: ####

Класс ```Person``` определён следующим образом:
```c#
public class Person
{
    public string Name;
    // обеспечивается реализацией IXmlSerializable
    public Address HomeAddress;
}
```
*обращение к реализации ```IXmlSerializable``` происходит селективно
(только для сериализации поля ```HomeAddress```)

Инициализация:
```c#
var p = new Person
{
    Name = "Alexander", HomeAddress = new Address
    {
        Street = "prospect Dzerzhinsky", PostCode = "150044"
    }
};
```
Результат:
```xml
<Person ... >
    <Name>Alexander</Name>
    <HomeAddress>
        <Street>prospect Dzerzhinsky</Street>
        <PostCode>150044</PostCode>
    </HomeAddress>
</Person>
```
