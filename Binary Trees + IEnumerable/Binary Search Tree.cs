using System;
using System.Collections.Generic;

namespace BST
{
    class BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class BinarySearchTreeElement
        {
            public TKey   Key   { get; set; } = default;
            public TValue Value { get; set; } = default;

            public BinarySearchTree<TKey, TValue> Left  { get; set; }
            public BinarySearchTree<TKey, TValue> Right { get; set; }
        }
        private BinarySearchTreeElement root;

        public BinarySearchTree(TKey key, TValue value)
        {
            this.root = new BinarySearchTreeElement();

            this.root.Key   = key;
            this.root.Value = value;

            this.root.Left  = null;
            this.root.Right = null;
        }


        public bool IsEmpty() => this.root == null;


        public void Insert(TKey key, TValue value)
        {                                                                             //               10 Мария
            TKey CurrentNode = this.root.Key;                                         //              /  \
                                                                                      //           L /      R
            if (CurrentNode.CompareTo(key) > 0) // this.Key > key => 1                //            /
            {                                                                         //           4 Александр
                if (this.root.Left == null)                                           //          / \
                    this.root.Left = new BinarySearchTree<TKey, TValue>(key, value);  //         /   \ R
                else                                                                  // Андрей 1     \
                    this.root.Left.Insert(key, value);                                //         \     5 Катерина
            }                                                                         //          \     \
            else if (CurrentNode.CompareTo(key) < 0) // this.Key < key => -1          //   Татьяна 3     \
            {                                                                         //          /       7 Наталья
                if (this.root.Right == null)                                          //         /       / \
                    this.root.Right = new BinarySearchTree<TKey, TValue>(key, value); //   Илья 2       /   \
                else                                                                  //               /     9 Полина
                    this.root.Right.Insert(key, value);                               //      Николай 6     /
            }                                                                         //                   /
            else this.root.Value = value;                                             //          Дмитрий 8
        }


        public String Traverse()
        {
            string result = String.Empty;

            if (this.root != null)
            {
                if (this.root.Left != null) result = this.root.Left.Traverse();

                result += $"{this.root.Value.ToString()}\n";

                if (this.root.Right != null) result += this.root.Right.Traverse();
            }
            else throw new Exception("Binary tree doesn't contain elements!");
            return result;
        }


        public TValue Find(TKey key)
        {
            TValue result = default;

            if (this.root != null)
            {
                if (this.root.Key != null)
                    if (this.root.Key.Equals(key)) return this.root.Value;
                    else
                    {
                        if (this.root.Left != null)
                            if (this.root.Key.CompareTo(key) > 0) return this.root.Left.Find(key);

                        if (this.root.Right != null)
                            if (this.root.Key.CompareTo(key) < 0) return this.root.Right.Find(key);
                    }
            }
            else throw new Exception("Binary tree doesn't contain elements!");
            return result;
        }
    }


    class Program
    {
        static void Main()
        {
            // data to fill the tree
            var personnel = new Dictionary<ushort, string>
            {
                { 4, "Александр" }, { 1, "Андрей" }, { 3, "Татьяна" },
                { 5, "Катерина" },  { 2, "Илья" },   { 7, "Наталья" },
                { 6, "Николай" },   { 9, "Полина" }, { 8, "Дмитрий" }
            };
            // filling the tree
            var bst = new BinarySearchTree<ushort, string>(10, "Мария");
            foreach (var p in personnel)
                bst.Insert(p.Key, p.Value);


            var result = bst.Traverse();
            Console.WriteLine(result);
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
            Мария
            */

            result = bst.Find(5);
            Console.WriteLine(result);
            /* Output results found by key:
            Катерина
            */

            //bst.Remove(10);
            //result = bst.Traverse();
            //Console.WriteLine(result);
        }
    }
}
