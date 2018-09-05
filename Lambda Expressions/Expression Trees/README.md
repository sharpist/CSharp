___________________________________________________________________________________________
# "Деревья выражений" – 
#### код в виде древовидной структуры, где каждый узел является выражением. ####
___________________________________________________________________________________________

В результате выполнения дерева выражений может возвращаться значение или
выполняться действие, такое как вызов метода.
Деревья выражений, представляющие лямбда-выражения, имеют тип
```LambdaExpression``` (когда тип делегата неизвестен) или ```Expression<TDelegate>```.


Можно создавать деревья выражений на основе анонимного лямбда-выражения
(однострочной лямбда-функции).
### Компилятор C# выводит дерево выражений: ###
```c#
Expression<Func<int, bool>> lambda = num => num < 5;
```
*нельзя использовать лямбды операторов (многострочные лямбды)


Допускается создание деревьев выражений с помощью пространства
имен ```System.Linq.Expressions```.

При этом задействуется класс ```Expression```, который содержит статические
методы фабрики, позволяющие строить узлы дерева выражений
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
___________________________________________________________________________________________
# "Синтаксический анализ деревьев выражений" – 
#### разбор дерева выражений на составляющие. ####
___________________________________________________________________________________________

Например, дерево выражений, представляющее лямбда-выражение ```num => num < 5```, можно разложить:
```c#
// создание дерева выражений
Expression<Func<int, bool>> exprTree = num => num < 5;

// разбор дерева выражений 
ParameterExpression param     = (ParameterExpression)exprTree.Parameters[0];
BinaryExpression    operation = (BinaryExpression)exprTree.Body;
ParameterExpression left      = (ParameterExpression)operation.Left;
ConstantExpression  right     = (ConstantExpression)operation.Right;

Console.WriteLine("Разложенное выражение: {0} => {1} {2} {3}", param.Name,
                                   left.Name, operation.NodeType, right.Value);
// Разложенное выражение: num => num LessThan 5
```
___________________________________________________________________________________________
# "Компиляция деревьев выражений" – 
#### процесс компиляции дерева выражений и выполнения результирующего кода. ####
___________________________________________________________________________________________

Для выполнения деревьев выражений вызывается метод ```Compile```, чтобы создать
исполняемый делегат, затем вызывается делегат.

Тип ```Expression<TDelegate>``` предоставляет метод ```Compile```, который компилирует код,
представляемый деревом выражений, в исполняемый делегат:
```c#
// создание дерева выражений
Expression<Func<int, bool>> expr = num => num < 5;

// компиляция дерева выражений в делегат
Func<int, bool> result = expr.Compile();

// вызов делегата и запись результата в консоль
Console.WriteLine(result(4)); // True


// упрощенный синтаксис компиляции и запуска  
Console.WriteLine(expr.Compile()(4)); // True
```
*если лямбда-выражение имеет тип ```LambdaExpression``` (когда тип делегата неизвестен)
вызывается метод ```DynamicInvoke``` для делегата
___________________________________________________________________________________________
# "Изменение деревьев выражений" – 
#### создание копии существующего дерева выражений и внесение изменения. ####
___________________________________________________________________________________________

Так как деревья выражений являются неизменяемыми напрямую, изменения применяются к
создаваемой копии.

Класс ```ExpressionVisitor``` используется для прохода по существующему дереву выражений и
копированию каждого пройденного узла:
```c#
// наследует от класса ExpressionVisitor 
public class AndAlsoModifier : ExpressionVisitor
{
    public Expression Modify(Expression expression)
    {
        return Visit(expression);
    }

    // переопределяет метод VisitBinary,
    // так как условная операция AND является двоичным выражением
    protected override Expression VisitBinary(BinaryExpression b)
    {
        if (b.NodeType == ExpressionType.AndAlso)
        {
            Expression left  = this.Visit(b.Left);
            Expression right = this.Visit(b.Right);

            // сделать данное двоичное выражение операцией OrElse вместо операции AndAlso
            return Expression.MakeBinary(ExpressionType.OrElse, left, right, b.IsLiftedToNull, b.Method);
        }
        // если выражение не представляет условную операцию AND
        // выполнить реализацию базового класса
        return base.VisitBinary(b);
    }
}
```

```c#
// создается выражение, содержащее условную операцию AND
Expression<Func<string, bool>> expr =
    name => name.Length > 10 && name.StartsWith("G");

Console.WriteLine(expr);
// name => ((name.Length > 10) AndAlso name.StartsWith("G"))

// создается экземпляр класса AndAlsoModifier
// для передачи выражения в метод Modify
AndAlsoModifier treeModifier = new AndAlsoModifier();
Expression      modifiedExpr = treeModifier.Modify((Expression)expr);

Console.WriteLine(modifiedExpr);
// name => ((name.Length > 10) OrElse name.StartsWith("G"))
```
___________________________________________________________________________________________
# "Использование деревьев выражений для построения динамических запросов" – 
#### создание LINQ запроса во время выполнения, когда характеристики запроса неизвестны во время компиляции. ####
___________________________________________________________________________________________

В LINQ деревья выражений используются для представления структурированных
запросов к источникам данных, реализующим интерфейс ```IQueryable<T>```:

1. Методы фабрики в пространстве имен ```System.Linq.Expressions``` используются для создания
деревьев выражений, представляющих общий запрос.

2. Выражения, представляющие вызовы методов стандартных операторов запросов,
ссылаются на реализации Queryable этих методов.

3. Итоговое дерево выражений передается в реализацию ```CreateQuery<TElement>(Expression)```
поставщика источника данных ```IQueryable``` для создания исполняемого запроса
типа ```IQueryable```.

```c#
string[] companies =
{
    "Consolidated Messenger", "Alpine Ski House",     "Southridge Video",         "City Power & Light",
    "Coho Winery",            "Wide World Importers", "Graphic Design Institute", "Adventure Works",
    "Humongous Insurance",    "Woodgrove Bank",       "Margie's Travel",          "Northwind Traders",
    "Blue Yonder Airlines",   "Trey Research",        "The Phone Company",        "Wingtip Toys",
    "Lucerne Publishing",     "Fourth Coffee"
};
IQueryable<String> queryableData = companies.AsQueryable<string>();       // данные IQueryable для запроса
ParameterExpression pe = Expression.Parameter(typeof(string), "company"); // создать выражение, представляющее параметр предикату


/// ***** companies.Where(company => (company.ToLower() == "coho winery" || company.Length > 16)).OrderBy(company => company) *****


// создать дерево выражений, представляющее выражение 'company.ToLower() == "coho winery"'
Expression left  = Expression.Call(pe, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
Expression right = Expression.Constant("coho winery");
Expression e1    = Expression.Equal(left, right);

// создать дерево выражений, представляющее выражение 'company.Length > 16'
           left  = Expression.Property(pe, typeof(string).GetProperty("Length"));
           right = Expression.Constant(16, typeof(int));
Expression e2    = Expression.GreaterThan(left, right);

// объединить деревья выражений, создав дерево выражений, представляющее
// выражение '(company.ToLower() == "coho winery" || company.Length > 16)'
Expression predicateBody = Expression.OrElse(e1, e2);


// создать дерево выражений, представляющее выражение
// 'queryableData.Where(company => (company.ToLower() == "coho winery" || company.Length > 16))'
MethodCallExpression whereCallExpression = Expression.Call(
    typeof(Queryable),
    "Where",
    new Type[] { queryableData.ElementType },
    queryableData.Expression,
    Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe }));


// создать дерево выражений, представляющее выражение
// 'whereCallExpression.OrderBy(company => company)'
MethodCallExpression orderByCallExpression = Expression.Call(
    typeof(Queryable),
    "OrderBy",
    new Type[] { queryableData.ElementType, queryableData.ElementType },
    whereCallExpression,
    Expression.Lambda<Func<string, string>>(pe, new ParameterExpression[] { pe }));




// создать исполняемый запрос из дерева выражений
IQueryable<string> results = queryableData.Provider.CreateQuery<string>(orderByCallExpression);
results?.ToList().ForEach(company => Console.WriteLine(company)); // перечисление результатов
/*
    Blue Yonder Airlines  
    City Power & Light  
    Coho Winery  
    Consolidated Messenger  
    Graphic Design Institute  
    Humongous Insurance  
    Lucerne Publishing  
    Northwind Traders  
    The Phone Company  
    Wide World Importers  
*/
```
