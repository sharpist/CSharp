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
        public TKey   Key   { get; set; }
        public TValue Value { get; set; }

        public BinarySearchTree<TKey, TValue> Left  { get; set; }
        public BinarySearchTree<TKey, TValue> Right { get; set; }


        public BinarySearchTree(TKey key, TValue value)
        {
            this.Key   = key;
            this.Value = value;

            this.Left  = null;
            this.Right = null;
        }


        public void Insert(TKey key, TValue value)
        {
            TKey CurrentNode = this.Key;

            if (CurrentNode.CompareTo(key) > 0) // this.Key > key => 1
            {                                                                    //               10 Мария
                if (this.Left == null)                                           //              /  \
                    this.Left = new BinarySearchTree<TKey, TValue>(key, value);  //           L /      R
                else                                                             //            /
                    this.Left.Insert(key, value);                                //           4 Александр
            }                                                                    //          / \
            else if (CurrentNode.CompareTo(key) < 0) // this.Key < key => -1     //         /   \ R
            {                                                                    // Андрей 1     \
                if (this.Right == null)                                          //         \     5 Катерина
                    this.Right = new BinarySearchTree<TKey, TValue>(key, value); //          \     \
                else                                                             //   Татьяна 3     \
                    this.Right.Insert(key, value);                               //          /       7 Наталья
            }                                                                    //         /       / \
            else                                                                 //   Илья 2       /   \
            {                                                                    //               /     9 Полина
                this.Key   = key;                                                //      Николай 6     /
                this.Value = value;                                              //                   /
            }                                                                    //          Дмитрий 8
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
