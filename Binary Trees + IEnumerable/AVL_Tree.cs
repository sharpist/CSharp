class AVL_Tree<TKey, TValue> where TKey : System.IEquatable<TKey>, System.IComparable<TKey>
{
    /// <summary>
    /// AVL Tree – self-balancing binary search tree.
    /// The implementation of C# version is made by Alexander Usov.
    /// A copy with the link please. :)
    /// </summary>
    sealed class node
    {
        public sbyte Height;
        public node  Left, Right;

        public TKey   Key   { get; } = default;
        public TValue Value { get; } = default;

        public node(TKey key, TValue value)
        {
            Height = 1; Left = Right = null;

            Key   = key;
            Value = value;
        }
    }
    node root;

    int height(node p)  => p != null ? p.Height : 0;
    int bfactor(node p) => height(p.Right) - height(p.Left);
    void fheight(node p)
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
        fheight(q);
        fheight(p);
        return p;
    }
    node rotateright(node p) // right rotation around p
    {
        node q  = p.Left;
        p.Left  = q.Right;
        q.Right = p;
        fheight(p);
        fheight(q);
        return q;
    }
    node balance(node p)     // node balancing p
    {
        fheight(p);
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
    ///          /           \          /              \
    /// Андрей [1]   Татьяна [3]      [6] Николай      [9] Полина
    ///                               /                /         \
    ///                              /                /           \
    ///                   Катерина [5]      Дмитрий [8]          [10] Ольга
    /// </summary>
    public void Insert(TKey key, TValue value)
    {
        node insert(node p, TKey k, TValue v)
        {
            if (p == null) return new node(k, v);
            if (p.Key.CompareTo(k) > 0)
                p.Left = insert(p.Left, k, v);
            else
                p.Right = insert(p.Right, k, v);
            return balance(p);
        }
        this.root = insert(this.root, key, value);
    }

    public string Traverse()
    {
        string traverse(node p)
        {
            var result = System.String.Empty;

            if (p == null) throw new System.Exception("Binary tree doesn't contain elements!");
            if (p.Left != null) result = traverse(p.Left);
            result += $"{p.Value.ToString()}\n";
            if (p.Right != null) result += traverse(p.Right);
            return result;
        }
        return traverse(this.root);
    }

    public TValue Find(TKey key)
    {
        TValue find(node p, TKey k)
        {
            TValue result = default;

            if (p == null) throw new System.Exception("Binary tree doesn't contain elements!");
            if (p.Key.Equals(k)) return p.Value;
            else
            {
                if (p.Left != null)
                    if (p.Key.CompareTo(k) > 0) return find(p.Left, k);
                if (p.Right != null)
                    if (p.Key.CompareTo(k) < 0) return find(p.Right, k);
            }
            return result;
        }
        return find(this.root, key);
    }

    public void Remove(TKey key)
    {
        node remove(node p, TKey k)
        {
            if (p == null) return default;
            if (p.Key.CompareTo(k) > 0)
                p.Left = remove(p.Left, k);
            else if (p.Key.CompareTo(k) < 0)
                p.Right = remove(p.Right, k);
            else // p.Key == k
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
        this.root = remove(this.root, key);
    }
    node findmin(node p) => p.Left != null ? findmin(p.Left) : p;
    node removemin(node p)
    {
        if (p.Left == null) return p.Right;
        p.Left = removemin(p.Left);
        return balance(p);
    }

    public bool IsEmpty() => this.root == null;
}


class Program
{
    static void Main()
    {
        var personnel = new[]
        {
            new { key = 4, value = "Александр" }, new { key = 1, value = "Андрей" },
            new { key = 9, value = "Полина" },    new { key = 3, value = "Татьяна" },
            new { key = 5, value = "Катерина" },  new { key = 10, value = "Ольга" },
            new { key = 2, value = "Илья" },      new { key = 7, value = "Наталья" },
            new { key = 6, value = "Николай" },   new { key = 8, value = "Дмитрий" }
        };
        var avl = new AVL_Tree<int, string>();
        foreach (var p in personnel) avl.Insert(p.key, p.value);

        System.Console.WriteLine(avl.Traverse());
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
        System.Console.WriteLine(avl.Find(5) + "\n");
        /* Output results found by key:
        Катерина
        */
        avl.Remove(1);
        avl.Remove(2);
        System.Console.WriteLine(avl.Traverse());
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
