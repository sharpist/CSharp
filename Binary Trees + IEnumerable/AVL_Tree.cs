using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// "AVL Tree" self-balancing binary search tree
/// C# implementation provided by Alexander Usov
/// free copy with source link please
/// </summary>
class AVL_Tree<TKey, TValue> : IEnumerable<TValue> where TKey : IEquatable<TKey>, IComparable<TKey>
{
    sealed class Node : IEnumerable<TValue>
    {
        public TKey   Key   { get; } = default;
        public TValue Value { get; } = default;
        public Byte Height;
        public Node Left, Right;
        public Node(TKey key, TValue value) =>
            (Key, Value, Height, Left, Right) = (key, value, 1, null, null);

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

    int height(Node p) => p?.Height ?? 0;
    int bFactor(Node p) => height(p.Right) - height(p.Left);
    void fixHeight(Node p)
    {
        (int hl, int hr) = (height(p.Left), height(p.Right));
        p.Height = Convert.ToByte(((hl > hr) ? hl : hr) + 1);
    }

    Node rotateLeft(Node q)  // left rotation around q
    {
        Node p  = q.Right;
        q.Right = p.Left;
        p.Left  = q;
        fixHeight(q);
        fixHeight(p);
        return p;
    }
    Node rotateRight(Node p) // right rotation around p
    {
        Node q  = p.Left;
        p.Left  = q.Right;
        q.Right = p;
        fixHeight(p);
        fixHeight(q);
        return q;
    }

    Node balance(Node p)     // node balancing p
    {
        fixHeight(p);
        if (bFactor(p) == -2)
        {
            if (bFactor(p.Left) > 0)
                p.Left = rotateLeft(p.Left);
            return rotateRight(p);
        }
        if (bFactor(p) == 2)
        {
            if (bFactor(p.Right) < 0)
                p.Right = rotateRight(p.Right);
            return rotateLeft(p);
        }
        return p;            // balancing isn't needed
    }
    /// <summary>
    ///                     [4] Александр
    ///                     /            \
    ///                    /              \
    ///             Илья [2]              [7] Наталья
    ///            /       \              /          \
    ///           /         \            /            \
    ///  Андрей [1] Татьяна [3]        [6] Николай    [9] Полина
    ///                                /              /         \
    ///                               /              /           \
    ///                    Катерина [5]    Дмитрий [8]           [10] Ольга
    /// </summary>
    Node root;

    public void Insert(TKey key, TValue value)
    {
        root = insert(root);
        Node insert(Node p)
        {
            if (p == null) return new Node(key, value);
            if (p.Key.CompareTo(key) > 0)
                p.Left = insert(p.Left);
            else
                p.Right = insert(p.Right);
            return balance(p);
        }
    }

    public void Remove(TKey key)
    {
        root = remove(root);
        Node remove(Node p)
        {
            if (p == null) return default;
            if (p.Key.CompareTo(key) > 0)
                p.Left = remove(p.Left);
            else if (p.Key.CompareTo(key) < 0)
                p.Right = remove(p.Right);
            else // p.Key == key
            {
                (Node q, Node r) = (p.Left, p.Right);
                p = null;
                if (r == null) return q;
                Node min  = findMin(r);
                min.Right = removeMin(r);
                min.Left  = q;
                return balance(min);
            }
            return balance(p);
        }
    }
    Node findMin(Node p) => (p.Left != null) ? findMin(p.Left) : p;
    Node removeMin(Node p)
    {
        if (p.Left == null) return p.Right;
        p.Left = removeMin(p.Left);
        return balance(p);
    }

    public TValue Find(TKey key)
    {
        return (!IsEmpty) ? find(root) : default;
        TValue find(Node p)
        {
            if (p.Key.Equals(key)) return p.Value;
            if (p.Key.CompareTo(key) > 0)
                return (p.Left != null) ? find(p.Left) : default;
            else
                return (p.Right != null) ? find(p.Right) : default;
        }
    }
    public bool IsEmpty => root == null;

    sealed class Enumerator : IEnumerator<TValue>
    {
        Queue<TValue> enumData;
        TValue current = default;
        public Enumerator(Node data) => enumData = new Queue<TValue>(iterator(data));

        bool IEnumerator.MoveNext() => enumData.TryDequeue(out current);
        TValue IEnumerator<TValue>.Current => (current != default) ? current
            : throw new InvalidOperationException("Use MoveNext before calling Current");

        IEnumerable<TValue> iterator(Node data)
        {
            if (data == null) yield break;
            if (data.Left != null)
                foreach (TValue value in data.Left)
                    yield return value;

            yield return data.Value;

            if (data.Right != null)
                foreach (TValue value in data.Right)
                    yield return value;
        }
        object IEnumerator.Current => throw new NotImplementedException();
        void IEnumerator.Reset() => throw new NotSupportedException();
        void IDisposable.Dispose() { }
    }
    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => new Enumerator(root);
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}


class Program
{
    static void Main()
    {
        var avl = new AVL_Tree<int, string>();

        foreach (var p in new[] {
            new { key = 4, value = "Александр" }, new { key = 1, value = "Андрей" },
            new { key = 9, value = "Полина" },    new { key = 3, value = "Татьяна" },
            new { key = 5, value = "Катерина" },  new { key = 10, value = "Ольга" },
            new { key = 2, value = "Илья" },      new { key = 7, value = "Наталья" },
            new { key = 6, value = "Николай" },   new { key = 8, value = "Дмитрий" }
        }) avl.Insert(p.key, p.value);

        foreach (var value in avl) Console.WriteLine(value);
        /* Output results sorted by key:
            Андрей
            Илья
            Татьяна
            Александр
            Катерина
            Николай
            Наталья
            Дмитрий
            Полина
            Ольга
        */
        Console.WriteLine("\n" + avl.Find(5) + "\n");
        /* Output results found by key:
            Катерина
        */
        avl.Remove(1);
        avl.Remove(2);
        foreach (var value in avl) Console.WriteLine(value);
        /* Output results after removal by key:
            Татьяна
            Александр
            Катерина
            Николай
            Наталья
            Дмитрий
            Полина
            Ольга
        */
    }
}
