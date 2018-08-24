using System;
using System.Collections.Generic;

namespace BST
{
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


            Console.WriteLine(bst.Traverse());
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

            Console.WriteLine(bst.Find(5));
            /* Output results found by key:
            Катерина
            */

        }
    }

    class BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey   Key   { get; private set; }
        public TValue Value { get; private set; }

        public BinarySearchTree<TKey, TValue> Left  { get; private set; }
        public BinarySearchTree<TKey, TValue> Right { get; private set; }


        public BinarySearchTree(TKey key, TValue value)
        {
            this.Key   = key;
            this.Value = value;

            this.Left  = null;
            this.Right = null;
        }


        public void Insert(TKey key, TValue value)
        {                                                                        //               10 Мария
            TKey CurrentNode = this.Key;                                         //              /  \
                                                                                 //           L /      R
            if (CurrentNode.CompareTo(key) > 0) // this.Key > key => 1           //            /
            {                                                                    //           4 Александр
                if (this.Left == null)                                           //          / \
                    this.Left = new BinarySearchTree<TKey, TValue>(key, value);  //         /   \ R
                else                                                             // Андрей 1     \
                    this.Left.Insert(key, value);                                //         \     5 Катерина
            }                                                                    //          \     \
            else if (CurrentNode.CompareTo(key) < 0) // this.Key < key => -1     //   Татьяна 3     \
            {                                                                    //          /       7 Наталья
                if (this.Right == null)                                          //         /       / \
                    this.Right = new BinarySearchTree<TKey, TValue>(key, value); //   Илья 2       /   \
                else                                                             //               /     9 Полина
                    this.Right.Insert(key, value);                               //      Николай 6     /
            }                                                                    //                   /
            else this.Value = value;                                             //          Дмитрий 8
        }


        public String Traverse()
        {
            string result = String.Empty;

            if (this.Left != null) result = this.Left.Traverse();

            result += $"{this.Value.ToString()}\n";

            if (this.Right != null) result += this.Right.Traverse();

            return result;
        }


        public TValue Find(TKey key)
        {
            TValue result = default;

            if (this.Key != null)
            {
                if (this.Key.Equals(key)) return this.Value;
                else
                {
                    if (this.Left != null)
                        if (this.Key.CompareTo(key) > 0) return this.Left.Find(key);

                    if (this.Right != null)
                        if (this.Key.CompareTo(key) < 0) return this.Right.Find(key);
                }
            }
            return result;
        }
    }
}
