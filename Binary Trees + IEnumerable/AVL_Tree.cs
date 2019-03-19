/// <summary>
/// "AVL Tree" self-balancing binary search tree
/// C# implementation provided by Alexander Usov
/// free copy with source link please
/// </summary>
class AVL_Tree<TKey, TValue> : System.Collections.Generic.IEnumerable<TValue>
    where TKey : System.IEquatable<TKey>, System.IComparable<TKey>
{
    sealed class node : System.Collections.Generic.IEnumerable<TValue>
    {
        public TKey   Key   { get; } = default;
        public TValue Value { get; } = default;

        public sbyte Height;
        public node  Left, Right;

        public node(TKey key, TValue value)
        {
            Key   = key;
            Value = value;

            Height = 1; Left = Right = null;
        }

        System.Collections.Generic.IEnumerator<TValue> System.Collections.Generic.IEnumerable<TValue>.GetEnumerator()
        {
            if (Left != null)
                foreach (TValue value in Left)
                    yield return value;

            yield return Value;

            if (Right != null)
                foreach (TValue value in Right)
                    yield return value;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => throw new System.NotImplementedException();
    }

    node root;

    int height(node p) => p?.Height ?? 0;
    int bfactor(node p) => height(p.Right) - height(p.Left);
    void fixheight(node p)
    {
        int hl = height(p.Left),
            hr = height(p.Right);
        p.Height = (sbyte)((hl > hr ? hl : hr) + 1);
    }

    node rotateleft(node q)  // left rotation around q
    {
        node p  = q.Right;
        q.Right = p.Left;
        p.Left  = q;
        fixheight(q);
        fixheight(p);
        return p;
    }
    node rotateright(node p) // right rotation around p
    {
        node q  = p.Left;
        p.Left  = q.Right;
        q.Right = p;
        fixheight(p);
        fixheight(q);
        return q;
    }

    node balance(node p)     // node balancing p
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

    public bool IsEmpty => root == null;

    public void Insert(TKey key, TValue value)
    {
        root = insert(root);
        node insert(node p)
        {
            if (p == null) return new node(key, value);
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
        node remove(node p)
        {
            if (p == null) return default;
            if (p.Key.CompareTo(key) > 0)
                p.Left = remove(p.Left);
            else if (p.Key.CompareTo(key) < 0)
                p.Right = remove(p.Right);
            else // p.Key == key
            {
                node q = p.Left;
                node r = p.Right;
                p = null;
                if (r == null) return q;
                node min  = findmin(r);
                min.Right = removemin(r);
                min.Left  = q;
                return balance(min);
            }
            return balance(p);
        }
    }
    node findmin(node p) => (p.Left != null) ? findmin(p.Left) : p;
    node removemin(node p)
    {
        if (p.Left == null) return p.Right;
        p.Left = removemin(p.Left);
        return balance(p);
    }

    public TValue Find(TKey key)
    {
        return (!IsEmpty) ? find(root) : default;
        TValue find(node p)
        {
            if (p.Key.Equals(key)) return p.Value;
            if (p.Key.CompareTo(key) > 0)
                return (p.Left != null) ? find(p.Left) : default;
            else
                return (p.Right != null) ? find(p.Right) : default;
        }
    }

    System.Collections.Generic.IEnumerator<TValue> System.Collections.Generic.IEnumerable<TValue>.GetEnumerator()
    {
        if (root.Left != null)
            foreach (TValue value in root.Left)
                yield return value;

        yield return root.Value;

        if (root.Right != null)
            foreach (TValue value in root.Right)
                yield return value;
    }
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => throw new System.NotImplementedException();
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
        foreach (var value in avl)
            System.Console.WriteLine(value);
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
        foreach (var value in avl)
            System.Console.WriteLine(value);
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
