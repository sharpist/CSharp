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


        private void buttonStop_Click(object sender, EventArgs e)
        {
            // инициировать отмену задачи
            if (tokenSource != null)
                tokenSource.Cancel();
        }


        private async void buttonStart_Click(object sender, EventArgs e)
        {
            #region ArrayFilling
            info.Text = null; infoStat.Text = null;
            // компонент испытательной нагрузки
            var rand = new Random();
            arr = new int[25000];
            for (int i = 0; i < arr.Length; i++)
            { arr[i] = rand.Next(1, 99); }
            #endregion

            // создание признака отмены
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            // задача к выполнению
            Task task = Task.Run(() => doWorkTask(/*params*/ token), token);
            try
            {
                await task;
            }
            catch (OperationCanceledException oce)
            { info.Text = oce.Message; }

            // запрос статуса задачи
            infoStat.Text = $"{task.Status}";
        }

        private void doWorkTask(/*params*/ CancellationToken token)
        {
            // компонент испытательной нагрузки
            for (int i = 0; i < arr.Length - 1; i++)
            {
                byte f = 0;
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    // создать исключение при запросе на отмену
                    token.ThrowIfCancellationRequested();

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
