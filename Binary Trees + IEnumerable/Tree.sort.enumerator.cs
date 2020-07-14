using System;
using System.Collections;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Tree<int> tree = default;

        // заполняем данными для сортировки
        new List<int> { 5, 10, 0, 8, -12, 11, 5, -8, 15, 8, 10, 14 }
        .ForEach(e => {
            tree?.Insert(e);
            tree = tree ?? new Tree<int>(e);
        });

        // выводим результат
        Console.WriteLine(tree.Result());
        // -12  -8  0  5  5  8  8  10  10  11  14  15
        foreach (var e in tree)
            Console.Write("{0} ", e);
        // -12  -8  0  5  5  8  8  10  10  11  14  15
    }
}

public class Tree<T> : IEnumerable<T> where T : IComparable<T>
{
    public T Node { get; set; }        // узел
    public Tree<T> Left  { get; set; } // левое поддерево
    public Tree<T> Right { get; set; } // правое поддерево

    public Tree(T param) =>
        (this.Node, this.Left, this.Right) = (param, null, null);

    public void Insert(T param)               // построим двоичное дерево
    {
        T CurrentNode = this.Node;
        if (CurrentNode.CompareTo(param) > 0) // узел > параметра true '+1', иначе false '0', '-1'
        {
            if (this.Left == null) this.Left = new Tree<T>(param);   ///      L _ 5 _ R
            else this.Left.Insert(param);                            ///     /         \
        }                                                            ///    0          10
        else                                                         ///   /         /    \
        {                                                            /// -12        8      11
            if (this.Right == null) this.Right = new Tree<T>(param); ///   \       / \    /  \
            else this.Right.Insert(param);                           ///   -8     5   8  10  15
        }                                                            ///                     /
    }                                                                ///                   14

    public String Result()                    // обходим узлы в заданном порядке
    {                                         // выводим упорядоченные значения дерева
        var result = String.Empty;
        if (this.Left != null) result = this.Left.Result();

        result += $"{this.Node.ToString()} ";

        if (this.Right != null) result += this.Right.Result();
        return result;
    }

    /// <summary>
    /// ***пример реализации интерфейса IEnumerable (для использования инструкции foreach)
    /// </summary>

    // необобщённая версия метода GetEnumerator
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    // создать объект-нумератор для обхода коллекции
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new TreeEnumerator<T>(this);
}

class TreeEnumerator<T> : IEnumerator<T> where T : IComparable<T>
{
    private T currentItem = default(T);
    private Queue<T> enumData = null;   // очередь enumData
    private Tree<T> currentData = null; // ссылка на дерево

    public TreeEnumerator(Tree<T> data) => this.currentData = data;

    // необобщённая версия свойства Current
    Object IEnumerator.Current { get => throw new NotImplementedException(); }

    // обобщённая версия свойства Current
    T IEnumerator<T>.Current {
        get {
            // проверяем отработал ли метод MoveNext
            if (this.enumData == null)
                throw new InvalidOperationException("Use MoveNext before calling Current");

            return this.currentItem;
        }
    }

    // при первом вызове инициализируем данные, используемые нумератором
    Boolean IEnumerator.MoveNext()
    {
        if (this.enumData == null) {
            this.enumData = new Queue<T>();             // создаём экземпляр коллекции (очередь)
            populate(this.enumData, this.currentData);  // заполняем очередь из дерева
        }
        if (this.enumData.Count > 0) {                  // на повторном вызове метода MoveNext переходим к следующему элементу
            this.currentItem = this.enumData.Dequeue(); // извлекаем элемент из очереди и обновляем свойство Current
            return true;
        }
        return false;
    }

    private void populate(Queue<T> enumData, Tree<T> currentData)
    {
        if (currentData.Left != null)
            populate(enumData, currentData.Left);

        enumData.Enqueue(currentData.Node);

        if (currentData.Right != null)
            populate(enumData, currentData.Right);
    }

    void IEnumerator.Reset() => throw new NotImplementedException();

    // нумератор не использует ресурсы, требующие высвобождения
    void IDisposable.Dispose() { }
}
