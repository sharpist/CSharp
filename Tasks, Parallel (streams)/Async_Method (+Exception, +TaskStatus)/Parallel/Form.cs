using System;
using System.Threading;
using System.Threading.Tasks;

namespace Async_Method
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        { InitializeComponent(); }

        // компонент испытательной нагрузки
        int[] arr = null;
        // поле источник признака отмены
        private CancellationTokenSource tokenSource = null;


        private async void buttonStart_Click(object sender, EventArgs e)
        {
            // компонент испытательной нагрузки
            var rand = new Random();
            arr = new int[50000];
            for (int i = 0; i < arr.Length; i++)
            { arr[i] = rand.Next(1, 99); }


            // создание признака отмены
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            // задача к выполнению
            Task t = null;
            try
            {
                await (t = doWorkTask(/*params*/ token));
            }
            catch (OperationCanceledException oce)
            { info.Text = oce.Message; }

            // запрос статуса задачи
            infoStat.Text = $"{t.Status}";
        }


        private void buttonStop_Click(object sender, EventArgs e)
        {
            // инициировать отмену задачи
            if (tokenSource != null)
                tokenSource.Cancel();
        }


        private Task doWorkTask(/*params*/ CancellationToken token)
        {
            Task t = Task.Run(() =>
            {

                // компонент испытательной нагрузки
                testLoad();

                // создать исключение при запросе на отмену
                token.ThrowIfCancellationRequested();

            }, token);

            return t;
        }


        private void testLoad()
        {
            // компонент испытательной нагрузки
            for (int i = 0; i < arr.Length - 1; i++)
            {
                byte f = 0;
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int buf = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = buf;
                        f = 1;
                    }
                }
                if (f == 0) break;
            }
        }
    }
}
