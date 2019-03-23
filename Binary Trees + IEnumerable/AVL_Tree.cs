/// <summary>
/// "AVL Tree" self-balancing binary search tree
/// C# implementation provided by Alexander Usov
/// free copy with source link please
/// </summary>
class AVL_Tree<TKey, TValue> : System.Collections.Generic.IEnumerable<TValue> where TKey : System.IEquatable<TKey>, System.IComparable<TKey>
{
    sealed class Node : System.Collections.Generic.IEnumerable<TValue>
    {
        public TKey   Key   { get; } = default;
        public TValue Value { get; } = default;
        public sbyte  Height;
        public Node   Left, Right;
        public Node(TKey key, TValue value)
        {
            Key    = key;
            Value  = value;
            Height = 1; Left = Right = null;
        }
        System.Collections.Generic.IEnumerator<TValue> System.Collections.Generic.IEnumerable<TValue>.GetEnumerator() => new Enumerator(this);
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => throw new System.NotImplementedException();
    }

    int height(Node p) => p?.Height ?? 0;
    int bfactor(Node p) => height(p.Right) - height(p.Left);
    void fixheight(Node p)
    {
        int hl = height(p.Left),
            hr = height(p.Right);
        p.Height = (sbyte)((hl > hr ? hl : hr) + 1);
    }

    Node rotateleft(Node q)  // left rotation around q
    {
        Node p  = q.Right;
        q.Right = p.Left;
        p.Left  = q;
        fixheight(q);
        fixheight(p);
        return p;
    }
    Node rotateright(Node p) // right rotation around p
    {
        Node q  = p.Left;
        p.Left  = q.Right;
        q.Right = p;
        fixheight(p);
        fixheight(q);
        return q;
    }

    Node balance(Node p)     // node balancing p
    {
        fixheight(p);
        if (bfactor(p) == -2)
        {
            if (bfactor(p.Left) > 0)
                p.Left = rotateleft(p.Left);
            return rotateright(p);
        }
        if (bfactor(p) == 2)
        {
            if (bfactor(p.Right) < 0)
                p.Right = rotateright(p.Right);
            return rotateleft(p);
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
                Node q = p.Left;
                Node r = p.Right;
                p = null;
                if (r == null) return q;
                Node min  = findmin(r);
                min.Right = removemin(r);
                min.Left  = q;
                return balance(min);
            }
            return balance(p);
        }
    }
    Node findmin(Node p) => (p.Left != null) ? findmin(p.Left) : p;
    Node removemin(Node p)
    {
        if (p.Left == null) return p.Right;
        p.Left = removemin(p.Left);
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

    System.Collections.Generic.IEnumerator<TValue> System.Collections.Generic.IEnumerable<TValue>.GetEnumerator() => new Enumerator(root);
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => throw new System.NotImplementedException();

    sealed class Enumerator : System.Collections.Generic.IEnumerator<TValue>
    {
        Node data;
        System.Collections.Generic.Queue<TValue> enumData;
        public Enumerator(Node data)
        {
            this.data = data;
            enumData  = new System.Collections.Generic.Queue<TValue>(iterator());
        }
        TValue System.Collections.Generic.IEnumerator<TValue>.Current => enumData.Dequeue();
        bool System.Collections.IEnumerator.MoveNext() => enumData.Count > 0;

        System.Collections.Generic.IEnumerable<TValue> iterator()
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
        object System.Collections.IEnumerator.Current => throw new System.NotImplementedException();
        void System.Collections.IEnumerator.Reset() => throw new System.NotSupportedException();
        void System.IDisposable.Dispose() { }
    }
}


class Program
{
    static void Main()
    {
        var avl = new AVL_Tree<int, string>();

        // filling tree structure
        foreach (var p in new[] {
            new { key = 4, value = "Александр" }, new { key = 1, value = "Андрей" },
            new { key = 9, value = "Полина" },    new { key = 3, value = "Татьяна" },
            new { key = 5, value = "Катерина" },  new { key = 10, value = "Ольга" },
            new { key = 2, value = "Илья" },      new { key = 7, value = "Наталья" },
            new { key = 6, value = "Николай" },   new { key = 8, value = "Дмитрий" }
        }) avl.Insert(p.key, p.value);

        // tree enumeration
        foreach (var value in avl) System.Console.WriteLine(value);
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

        // tree research
        System.Console.WriteLine("\n" + avl.Find(5) + "\n");
        /* Output results found by key:
            Катерина
        */

        // removal
        avl.Remove(1);
        avl.Remove(2);
        foreach (var value in avl) System.Console.WriteLine(value);
        /* Output results after deletion by key:
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
