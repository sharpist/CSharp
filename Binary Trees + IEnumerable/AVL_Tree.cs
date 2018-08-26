class AVL_Tree<TKey, TValue> where TKey : System.IComparable<TKey>
{
    /// <summary>
    /// AVL Tree – self-balancing binary search tree.
    /// The implementation of C# version is made by Alexander Usov.
    /// A copy with the link please. :)
    /// </summary>
    private class Node
    {
        public sbyte Height;

        public TKey   Key                 = default;
        public TValue Value { get; set; } = default;

        public Node Left  { get; set; }
        public Node Right { get; set; }

        public Node(TKey key, TValue value)
        {
            this.Height = 1;

            this.Key   = key;
            this.Value = value;

            this.Left  = null;
            this.Right = null;
        }
    }

    private Node root;

    private sbyte height(Node p)        => (sbyte)(p != null ? p.Height : 0);
    private sbyte balanceFactor(Node p) => (sbyte)(height(p.Right) - height(p.Left));
    private void  fixHeight(Node p)
    {
        var hl = height(p.Left);
        var hr = height(p.Right);
        p.Height = (sbyte)((hl > hr ? hl : hr) + 1);
    }

    private Node rotateRight(Node p) // right rotation around p
    {
        Node q  = p.Left;
        p.Left  = q.Right;
        q.Right = p;
        fixHeight(p);
        fixHeight(q);
        return q;
    }
    private Node rotateLeft(Node q)  // left rotation around q
    {
        Node p  = q.Right;
        q.Right = p.Left;
        p.Left  = q;
        fixHeight(q);
        fixHeight(p);
        return p;
    }
    private Node balance(Node p)     // node balancing p
    {
        fixHeight(p);
        if (balanceFactor(p) == 2)
        {
            if (balanceFactor(p.Right) < 0)
                p.Right = rotateRight(p.Right);
            return rotateLeft(p);
        }
        if (balanceFactor(p) == -2)
        {
            if (balanceFactor(p.Left) > 0)
                p.Left = rotateLeft(p.Left);
            return rotateRight(p);
        }
        return p;                    // balancing isn't needed
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
    public void Insert(TKey key, TValue value) => this.root = insert(this.root, key, value);
    private Node insert(Node p, TKey key, TValue value)
    {
        if (p == null) return new Node(key, value);
        if (p.Key.CompareTo(key) > 0)
            p.Left = insert(p.Left, key, value);
        else
            p.Right = insert(p.Right, key, value);
        return balance(p);
    }
    public string Traverse() => traverse(this.root);
    private string traverse(Node p)
    {
        var result = System.String.Empty;

        if (p == null) throw new System.Exception("Binary tree doesn't contain elements!");
        if (p.Left != null) result = traverse(p.Left);
        result += $"{p.Value.ToString()}\n";
        if (p.Right != null) result += traverse(p.Right);
        return result;
    }
    public TValue Find(TKey key) => find(this.root, key);
    private TValue find(Node p, TKey key)
    {
        TValue result = default;

        if (p == null) throw new System.Exception("Binary tree doesn't contain elements!");
        if (p.Key.Equals(key)) return p.Value;
        else
        {
            if (p.Left != null)
                if (p.Key.CompareTo(key) > 0) return find(p.Left, key);
            if (p.Right != null)
                if (p.Key.CompareTo(key) < 0) return find(p.Right, key);
        }
        return result;
    }
    public void Remove(TKey key) => this.root = remove(this.root, key);
    private Node remove(Node p, TKey key)
    {
        if (p == null) return default;
        if (p.Key.CompareTo(key) > 0)
            p.Left = remove(p.Left, key);
        else if (p.Key.CompareTo(key) < 0)
            p.Right = remove(p.Right, key);
        else
        {
            Node q = p.Left;
            Node r = p.Right;
            p = null;
            if (r == null) return q;
            Node min  = findMin(r);
            min.Right = removeMin(r);
            min.Left  = q;
            return balance(min);
        }
        return balance(p);
    }
    private Node findMin(Node p) => p.Left != null ? findMin(p.Left) : p;
    private Node removeMin(Node p)
    {
        if (p.Left == null) return p.Right;
        p.Left = removeMin(p.Left);
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

        System.Console.WriteLine(avl.Find(5));
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
