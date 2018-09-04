___________________________________________________________________________________________
# "Деревья выражений" – 
#### код в виде древовидной структуры, где каждый узел является выражением. ####
___________________________________________________________________________________________

Можно создавать деревья выражений на основе анонимного лямбда-выражения
(однострочной лямбда-функции).
### Компилятор C# выводит дерево выражений: ###
```c#
Expression<Func<int, bool>> lambda = num => num < 5;
```
Нельзя использовать лямбды операторов (многострочные лямбды). 


Допускается создание деревьев выражений с помощью пространства
имен ```System.Linq.Expressions```.

При этом задействуется класс ```Expression```, который содержит статические
методы фабрики, позволяющие строить узлы дерева выражения
определённого типа (производного от абстрактного Expression):

1. ```ParameterExpression``` – представляет переменную или параметр.

2. ```MethodCallExpression``` – представляет вызов метода.

*другие зависящие от выражения типы определены в пространстве имен
```System.Linq.Expressions```

### Вручную создать дерево выражений для лямбда-выражения ```num => num < 5```: ###
```c#
// создаёт узел для идентификации параметра/переменной в дереве
ParameterExpression numParam        = Expression.Parameter(typeof(int), "num");
// представляет свойства с заданными значением и типом
ConstantExpression  five            = Expression.Constant(5, typeof(int));
// представляет числовое сравнение "меньше, чем"
BinaryExpression    numLessThanFive = Expression.LessThan(numParam, five);

Expression<Func<int, bool>> lambda1 = Expression.Lambda<Func<int, bool>>(
    numLessThanFive,
    new ParameterExpression[] { numParam });
```

Деревья выражений поддерживают присваивание и выражения потока управления,
такие как циклы, условные блоки и блоки ```try-catch```.

Следовательно, самостоятельно с помощью API-интерфейса можно создавать
деревья выражений, более сложные, чем деревья, создаваемые
компилятором C# из лямбда-выражений.
Например, дерево выражений, которое вычисляет факториал числа:
```c#
// создание параметра выражения
ParameterExpression value  = Expression.Parameter(typeof(int), "value");
// создание выражения для хранения локальной переменной
ParameterExpression result = Expression.Parameter(typeof(int), "result");
// создание метки для перехода из цикла
LabelTarget label = Expression.Label(typeof(int));


// создание тела метода
BlockExpression block = Expression.Block
    (
        new[] { result }, // добавление локальной переменной
        Expression.Assign(result, Expression.Constant(1)), // присвоение константы локальной переменной: result = 1

            // добавление цикла
            Expression.Loop
            (
                Expression.IfThenElse // добавление условного блока в цикл
                (
                    // состояние: value > 1
                    Expression.GreaterThan(value, Expression.Constant(1)),


                    // если true: result *= value--
                    Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                    // если false: выйти из цикла и перейти к метке
                    Expression.Break(label, result)
                ),
                // метка для перехода
                label
            )
    );

// компилировать и выполнить дерево выражений
int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);

Console.WriteLine(factorial); // 120
```
