# Индексаторы
_______________________________________________________________________________

### Определение индексатора: ###
```c#
struct StructIndexers
{
    // определено несколько индексаторов:

    // индексатор 1
    public String this[int key]
    {
        get { return stor[key]; }
        set { stor[key] = value; }
    }
    // индексатор 2
    public Int32 this[string key]
    {
        get { return dict[key]; }
        set { dict[key] = value; }
    }

    static string[] stor;
    static Dictionary<string, int> dict;
    static StructIndexers()
    {
        stor = new string[100];
        dict = new Dictionary<string, int>();
    }
}
```
### Использование индексаторов: ###
```c#
StructIndexers myStruct;

myStruct[0] = "Hello World!";
myStruct[1] = "What You Doing Today?";

Console.WriteLine("{0} {1}", myStruct[0], myStruct[1]);
// Hello World! What You Doing Today?


myStruct["One"] = 1;
myStruct["Two"] = 2;
myStruct["Three"] = 3;

Console.WriteLine("{0} {1} {2}",
    myStruct["One"], myStruct["Two"], myStruct["Three"]
);
// 1 2 3
```
### Побитовые операции с индексаторами: ###
```c#
struct IntBits
{
    // индексатор
    public bool this[int index]
    {
        get
        {
            return (bits & (1 << index)) != 0;
        }
        set
        {
            if (value)
                bits |= (1 << index);
            else
                bits &= ~(1 << index);
        }
    }

    private int bits;
    public IntBits(int param)
    { this.bits = param; }
}
```
### Демонстрация: ###
```c#
var bits = new IntBits(126); // (126) 0111 1110

bool peek = bits[6]; // 0[1]11 1110   извлечение булева значения с индексом 6 = true
bits[0] = true;      // 0111 111[1]   установка бита с индексом 0 в true
bits[3] = false;     // 0111 [0]111   установка бита с индексом 3 в false
bits[6] ^= true;     // 0[0]11 0111   инвертирует значение бита с индексом 6 = false

// теперь в bits содержится значение (55) 0011 0111
```
