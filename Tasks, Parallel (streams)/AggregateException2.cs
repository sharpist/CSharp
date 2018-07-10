namespace PLINQCancellation_2
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            int[] source = Enumerable.Range(1, 10000000).ToArray();
            var cts = new CancellationTokenSource();

            // Start a new asynchronous task that will cancel the 
            // operation from another thread. Typically you would call
            // Cancel() in response to a button click or some other
            // user interface event.
            Task.Factory.StartNew(() =>
            {
                UserClicksTheCancelButton(cts);
            });

            double[] results = null;
            try
            {
                results = (from num in source.AsParallel().WithCancellation(cts.Token)
                           where num % 3 == 0
                           select Function(num, cts.Token)).ToArray();
            }
            catch (OperationCanceledException e)
            {
                WriteLine(e.Message);
            }
            catch (AggregateException ae)
            {
                if (ae.InnerExceptions != null)
                {
                    foreach (Exception e in ae.InnerExceptions)
                        WriteLine(e.Message);
                }
            }
            finally
            {
                cts.Dispose();
            }

            if (results != null)
            {
                foreach (var v in results)
                    WriteLine(v);
            }
            WriteLine();
            ReadKey();
        }

        // A toy method to simulate work.
        static double Function(int n, CancellationToken ct)
        {
            // If work is expected to take longer than 1 ms
            // then try to check cancellation status more
            // often within that work.
            for (int i = 0; i < 5; i++)
            {
                // Work hard for approx 1 millisecond.
                Thread.SpinWait(50000);

                // Check for cancellation request.
                ct.ThrowIfCancellationRequested();
            }
            // Anything will do for our purposes.
            return Math.Sqrt(n);
        }

        static void UserClicksTheCancelButton(CancellationTokenSource cts)
        {
            // Wait between 150 and 500 ms, then cancel.
            // Adjust these values if necessary to make
            // cancellation fire while query is still executing.
            Random rand = new Random();
            Thread.Sleep(rand.Next(150, 500));
            WriteLine("Press 'c' to cancel");
            if (ReadKey().KeyChar == 'c')
                cts.Cancel();
        }
    }
}

/*
При обработке отмены в пользовательском коде, нет необходимости использовать
WithCancellation в определении запроса.
Но рекомендуется это сделать, так как WithCancellation не влияет на производительность
запросов, но позволяет обрабатывать отмену как в операторах запроса, так и в
пользовательском коде.

Чтобы обеспечить высокую скорость реагирования системы, рекомендуется проверять
отмену приблизительно каждую миллисекунду. Впрочем, здесь считается допустимым любой
период вплоть до 10 мс.
Частота проверок должна быть такой, чтобы не снижать производительность кода.

Если удаляется перечислитель, например когда код прерывается вне цикла foreach,
выполняющего итерацию по результатам запроса, то этот запрос отменяется без
создания исключений. 
*/