class AVL_Tree<TKey, TValue> where TKey : System.IComparable<TKey>
{
    /// <summary>
    /// self-balancing binary search tree
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

            this.Left  = default;
            this.Right = default;
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
        Node q = p.Left;
        p.Left = q.Right;
        q.Right = p;
        fixHeight(p);
        fixHeight(q);
        return q;
    }
    private Node rotateLeft(Node q)  // left rotation around q
    {
        Node p = q.Right;
        q.Right = p.Left;
        p.Left = q;
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

    //TODO

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
        var bst = new AVL_Tree<int, string>();
        foreach (var p in personnel) bst.Insert(p.key, p.value);

        System.Console.WriteLine(bst.Traverse());
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
    }
}
