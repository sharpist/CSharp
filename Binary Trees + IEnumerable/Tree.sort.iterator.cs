...
    class Program
    {
        ...
            // данные для сортировки
            int[] arr = new int[12] { 5, 10, 0, 8, -12, 11, 5, -8, 15, 8, 10, 14 };

            // заполнение
            int f;
            Tree<int> tree = new Tree<int>(arr[f = 0]);
            for (f++; f < arr.Length; ++f)
            { tree.Insert(arr[f]); }

            // результат
            Console.WriteLine(tree.Result());
            // -12  -8  0  5  5  8  8  10  10  11  14  15

            foreach (int item in tree)
            { Console.Write($"{item} "); }
            // -12  -8  0  5  5  8  8  10  10  11  14  15
    }

    public class Tree<T> : IEnumerable<T> where T : IComparable<T>
    {
        public T Node { get; set; }               // узел
        public Tree<T> Left { get; set; }         // левое поддерево
        public Tree<T> Right { get; set; }        // правое поддерево

        public Tree(T param)
        {
            this.Node = param;
            this.Left = null;
            this.Right = null;
        }

        public void Insert(T param)               // построение двоичного дерева
        {
            T CurrentNode = this.Node;

            if (CurrentNode.CompareTo(param) > 0) // узел > параметра true '+1', иначе false '0', '-1'
            {
                if (this.Left == null) this.Left = new Tree<T>(param);   //        L _ 5 _ R
                else this.Left.Insert(param);                            //       /         \
            }                                                            //      0          10
            else                                                         //     /         /    \
            {                                                            //   -12        8      11
                if (this.Right == null) this.Right = new Tree<T>(param); //     \       / \    /  \
                else this.Right.Insert(param);                           //     -8     5   8  10  15
            }                                                            //                       /
        }                                                                //                     14

        public string Result()                    // обход узлов в необходимом порядке
        {                                         // вывод упорядоченных значений дерева
            string result = "";

            if (this.Left != null) result = this.Left.Result();

            result += $"{this.Node.ToString()} ";

            if (this.Right != null) result += this.Right.Result();

            return result;
        }


        // ***пример реализации интерфейса IEnumerable (для использования инструкции foreach)
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (this.Left != null)              // если левое поддерево не пустое
            {
                foreach (T item in this.Left)   // инструкция foreach вызывает в отношении себя метод GetEnumerator
                {
                    yield return item;
                }
            }
            yield return this.Node;
            if (this.Right != null)
            {
                foreach (T item in this.Right)
                {
                    yield return item;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() // необобщённая версия метода GetEnumerator
        {
            throw new NotImplementedException();
        }
    }

