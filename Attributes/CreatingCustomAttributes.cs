// многократно используемый настраиваемый атрибут
[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct |
                       System.AttributeTargets.Method,
                       AllowMultiple = true)
]
public class Author : System.Attribute
{
    string name;
    public double version;

    public Author(string name)
    {
        this.name = name;
        version = 1.0;
    }
    public string GetName() => this.name;
}

// несколько атрибутов одного типа применяются к классу
[Author("P. Ackerman")]
[Author("R. Koch", version = 1.3)]
public class SampleClass
{
    // P. Ackerman's code goes here...
    // R. Koch's code goes here...

    // атрибут применяется к методу класса
    [Author("A. Carpenter", version = 2.0)]
    public void SampleMethod()
    {
        // A. Carpenter's code goes here...
    }
}