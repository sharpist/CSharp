## ```Switch Statements & Switch Expressions```

#### C# 8.0 вводит ```switch expressions```, предоставляющие:
* более лаконичный синтаксис
* возвращаемое значение (так как это выражение)
* полную интеграцию с шаблоном сопоставления

Синтаксис для ```switch expressions```:
```c#
public static System.String Display(object o) =>
    o switch
    {
        Point { X: 0, Y: 0 }         => "origin",
        Point { X: var x, Y: var y } => $"({x}, {y})",
        _ => "unknown" // default
    };
```

Можно использовать деконструкцию кортежа и позицию параметров:
```c#
public static State ChangeState(State current, Transition transition, bool hasKey) =>
    (current, transition) switch
    {
        (Opened, Close)              => Closed,
        (Closed, Open)               => Opened,
        (Closed, Lock)   when hasKey => Locked,
        (Locked, Unlock) when hasKey => Closed,
        _ => throw new InvalidOperationException($"Invalid transition")
    };
```
Не требуется определять переменную или явный тип для каждого из случаев,
тестируемый кортеж сопоставляется с кортежами, определёнными для
каждого из случаев.

