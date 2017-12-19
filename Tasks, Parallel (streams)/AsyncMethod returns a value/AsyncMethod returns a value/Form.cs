using System;
using System.Threading.Tasks;

namespace AsyncMethod_returns_a_value
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form() { // конструктор
            InitializeComponent();
        }


        private async void StartButton_Click(object sender, EventArgs e)
        {
            // await в вызове асинхронного метода возвращающего значение
            var result = await MethodAsync();

            for (int i = 0; i < 200; i++) { // вывод результата
                textBox.Text += result[i].ToString() + (i+1<200?" ":"");
            }
        }


        // асинхронный метод создающий результат
        // возвращает Task<TResult>
        private async Task<int[]> MethodAsync()
        {
            // длительная операция
            Task<int[]> task = Task.Run(() => longOperation());
            await       task;
            return      task.Result;
        }


        // длительная операция
        private int[] longOperation()
        {
            var ran = new Random();
            var arr = new int[25000]; // заполнить массив
            for (int i = 0; i < arr.Length; i++) { arr[i] = ran.Next(1, 99); }

            // упорядочить значения
            for (int i = 0; i < arr.Length - 1; i++)
            {
                bool f = false;

                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int buf = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = buf;

                        f = true;
                    }
                }
                if (f == false) break;
            }
            return arr;
        }
    }
}
