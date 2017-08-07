...
    class Program
    {
        ...
            Queue<string> myQueue = new Queue<string>();

            myQueue.Add("First");
            myQueue.Add("Second");
            myQueue.Add("Third");
            myQueue.Add("Fourth");

            Console.WriteLine(myQueue.Get()); // First
            Console.WriteLine(myQueue.Get()); // Second
            Console.WriteLine(myQueue.Get()); // Third
            Console.WriteLine(myQueue.Get()); // Fourth
    }

    class Queue<T>
    {
        private byte a, g;                               // флаги индексов
        private byte e;                                  // флаг кол-во элементов
        private T[] _arr;                                // массив
        public T[] arr {
            get { return this._arr; }
            set { this._arr = value; }
        }
        public Queue() {
            this.a = 0;
            this.g = 0;
            this.e = 0;
            this.arr = new T[this.a + 1];
        }

        public void Add(T param)                         // добавить в очередь
        {
            #region Check Array
            if (this.e == this.arr.Length)               // увеличить массив если заполнен
            {
                T[] rec = new T[this.arr.Length];        // массив копия

                for(int i = 0; i < this.arr.Length; i++)
                { rec[i] = this.arr[i]; }

                this.arr = new T[this.a + 1];            // увеличить массив

                for (int i = 0; i < rec.Length; i++)
                { this.arr[i] = rec[i]; }
            }
            #endregion
            this.arr[this.a++] = param;
            //this.a %= (byte)this.arr.Length;           // обнулить флаг (not used)
            this.e++;
        }

        public T Get()                                   // получить из очереди
        {
            var item = this.arr[this.g++];
            //this.g %= (byte)this.arr.Length;           // обнулить флаг (not used)
            this.e--;
            return item;
        }
    }
}
