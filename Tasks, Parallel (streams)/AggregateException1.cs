namespace PLINQCancellation_1
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

            int[] results = null;
            try
            {
                results = (from num in source.AsParallel().WithCancellation(cts.Token)
                           where num % 3 == 0
                           orderby num descending
                           select num).ToArray();
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

        static void UserClicksTheCancelButton(CancellationTokenSource cts)
        {
            // Wait between 150 and 500 ms, then cancel.
            // Adjust these values if necessary to make
            // cancellation fire while query is still executing.
            Random rand = new Random();
            Thread.Sleep(rand.Next(150, 500));
            cts.Cancel();
        }
    }
}

/*
Платформа PLINQ не помещает единственное OperationCanceledException в
System.AggregateException. OperationCanceledException нужно обрабатывать
в отдельном блоке catch.

Если один или несколько пользовательских делегатов создают исключение
OperationCanceledException(externalCT) (с помощью внешнего объекта
System.Threading.CancellationToken), но не создают других исключений, и
при этом запрос был определен как AsParallel().WithCancellation(externalCT),
то PLINQ выдает одно исключение OperationCanceledException(externalCT), но
не System.AggregateException.

Тем не менее, если один пользовательский делегат создает исключение
OperationCanceledException, а другой делегат создает исключение другого
типа, то оба этих исключения помещаются в AggregateException.

Общие рекомендации по отмене: 
Если отменяется пользовательский делегат, необходимо известить PLINQ о внешнем
CancellationToken и создать исключение OperationCanceledException(externalCT).

Если выполняется только отмена и нет других исключений, следует обрабатывать
OperationCanceledException, а не AggregateException. 
*/