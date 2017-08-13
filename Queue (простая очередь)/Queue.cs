...
    class Program
    {
        ...
            Queue<string> myQueue = new Queue<string>();
            // 1. the principle of "who came first, leaves first"
            // 2. with expandable on demand
            // 3. all items are added sequentially, priority for empty positions
            // 4. you can add items by index
            // 5. the extracted item is removed from the queue
            // 6. also, you can delete items on demand
            myQueue.Add("First");
            myQueue.Add(3, "Third");            // added to the third position
            myQueue.Add("Second");
            Console.WriteLine(myQueue.Get(3));  // "Third"
            Console.WriteLine(myQueue.Get());   // "First"
            Console.WriteLine(myQueue.Get());   // "Second"

            myQueue.Add("Sixth");
            myQueue.Add("Fifth");
            myQueue.Add("Fourth");
            myQueue.Del(3);                     // Fourth now removed
            myQueue.Sort();                     // Sixth, Fifth -> Fifth, Sixth abc...

            Console.WriteLine(myQueue.Count()); // "2"
            Console.WriteLine(myQueue.Get());   // "Fifth"
            Console.WriteLine(myQueue.Get());   // "Sixth"
    }

    class Queue<T> where T : IComparable<T>
    {
        // добавить элемент
        public void Add(T item)
        {
            if (this.element == this.arr.Length) arrayCheck(1);
            for (int i = 0; i < this.arr.Length; i++)
            { if(arr[i] == null) this.arr[i] = item; }
            this.element++;
        }
        public void Add(byte index, T item)
        {
            if (index <= 0) throw new IndexOutOfRangeException("Incorrect index!");
            while (index - 1 >= this.arr.Length) { arrayCheck(1); }
            arrayCheck(2, index);
            this.arr[index - 1] = item;
            this.element++;
        }

        // извлечь элемент
        public T Get()
        {
            if (this.element == 0) throw new IndexOutOfRangeException("The queue is empty!");
            var item = this.arr[0];
            arrayCheck(0);
            this.element--;

            return item;
        }
        public T Get(byte index)
        {
            if (this.element == 0) throw new IndexOutOfRangeException("The queue is empty!");
            if (index <= 0) throw new IndexOutOfRangeException("Incorrect index!");
            var item = this.arr[index - 1];
            arrayCheck(-1, index);
            this.element--;

            return item;
        }

        // удалить элемент
        public void Del(byte index)
        {
            if (this.arr.Length < index - 1 || arr[index - 1] == null)
                throw new IndexOutOfRangeException("The queue does not carry this item!");
            if (0 < index)
            {
                arrayCheck(-1, index);
                this.element--;
            }
        }

        // сортировать элементы
        public void Sort()
        { if (this.arr.Length > 1) arrayCheck(3); }

        // показать список
        public int Count() => this.arr.Length;


        #region вспомогательный метод
        private void arrayCheck(sbyte param, byte index = 0)
        {
            switch (param) {
                // увеличить массив (добавление следующего элемента)
                case 1 :
                T[] buf = new T[this.arr.Length];

                for (int i = 0; i < this.arr.Length; i++)
                { buf[i] = this.arr[i]; }

                this.arr = new T[this.arr.Length + 1];

                for (int i = 0; i < buf.Length; i++)
                { this.arr[i] = buf[i]; }
                break;

                // (добавление элемента по индексу)
                case 2 :
                buf = new T[this.arr.Length];
                int j = 0;
                for (int i = 0; i < this.arr.Length; i++)
                { if (i != index - 1) buf[i] = this.arr[j++]; }

                this.arr = buf;
                break;

                // уменьшить массив (извлечение следующего элемента)
                case 0 :
                buf = new T[this.arr.Length - 1];

                for (int i = 0; i < this.arr.Length - 1; i++)
                { buf[i] = this.arr[i + 1]; }

                this.arr = new T[buf.Length];
                this.arr = buf;
                break;

                // уменьшить массив (удаление/извлечение элемента по индексу)
                case -1 :
                buf = new T[this.arr.Length - 1];
                j = 0;
                for (int i = 0; i < this.arr.Length; i++)
                { if (i != index - 1) buf[j++] = this.arr[i]; }

                this.arr = buf;
                break;

                // сортировать элементы
                case 3 :
                for (int i = 0; i < this.arr.Length; i++) {
                    if (this.arr[i] == null) {
                        for (j = i + 1; j < this.arr.Length; j++) {
                            if (arr[j] != null) {
                                var temp = this.arr[i];
                                this.arr[i] = this.arr[j];
                                this.arr[j] = temp;
                                break;
                            }
                        }
                    }
                } byte k = 0;
                for (int i = 0; i < this.arr.Length; i++) { if (arr[i] == null) k++; }
                for (int min = 0; min < this.arr.Length - 1 - k; min++) {
                    for (int max = min + 1; max < this.arr.Length - k; max++)
                    {
                        if (this.arr[min].CompareTo(this.arr[max]) >= 0)
                        {
                            var temp = this.arr[min];
                            this.arr[min] = this.arr[max];
                            this.arr[max] = temp;
                        }
                    }
                } break;
            }
        }
        #endregion


        private T[] arr;
        private byte element;
        public Queue() {
            this.element = 0;
            this.arr = new T[this.element + 1];
        }
        public Queue(byte size) {
            this.element = 0;
            if (0 == size) size++;
            this.arr = new T[this.element + size];
        }
    }

