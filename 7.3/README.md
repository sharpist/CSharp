#			[C# 7.0](https://github.com/sharpist/C_Sharp/tree/master/7.0#c-70) | [C# 7.1](https://github.com/sharpist/C_Sharp/tree/master/7.1#c-71) | [C# 7.2](https://github.com/sharpist/C_Sharp/tree/master/7.2#c-72) | C# 7.3
#### Содержание: ####

Повышение производительности безопасного кода:

[Индексирование полей ```fixed``` без закрепления](https://github.com/sharpist/C_Sharp/tree/master/7.3#Индексирование-полей-fixed-без-закрепления)

[Локальные переменные ```ref``` могут быть переназначены](https://github.com/sharpist/C_Sharp/tree/master/7.3#Локальные-переменные-ref-могут-быть-переназначены)

[...](https://github.com/)

[...](https://github.com/)

[...](https://github.com/)

[...](https://github.com/)

Улучшения существующих функций:

[...](https://github.com/)

[...](https://github.com/)

___________________________________________________________________
```
Расширяются возможности гарантированно безопасного кода – 
приоритетно использовать безопасные конструкции.
```
##			"Индексирование полей ```fixed``` без закрепления"

Представлена следующая структура с массивом фиксированного размера
([Буферы фиксированного размера](https://github.com/sharpist/C_Sharp/tree/master/Fixed#Буферы-фиксированного-размера)):
```
unsafe struct MyBuffer
{
    public fixed int fixedBuffer[10];
}
```
В более ранних версиях C# переменную необходимо закрепить, чтобы
получить доступ к целым числам, входящим в ```fixedBuffer```:
```
// оператор fixed устанавливает указатель на первый элемент
unsafe void AccessMyBuffer()
{
    fixed (int* intPtr = myBuffer.fixedBuffer)
    {
        int p = intPtr[5];
    }
}
```
Теперь такой код компилируется в безопасном контексте, не требуется
объявлять второй фиксированный указатель ```int* intPtr``` на ```fixedBuffer```.
Контекст ```unsafe``` по-прежнему является обязательным.

Переменная ```p``` обращается к одному элементу в ```fixedBuffer```. Для этого
не нужно объявлять отдельную переменную ```int*```.
```
class MyClass
{
    MyBuffer myBuffer = default;

    unsafe void AccessMyBuffer()
    {
        int p = myBuffer.fixedBuffer[5];
    }
}
```
___________________________________________________________________
##			"Локальные переменные ```ref``` могут быть переназначены"

