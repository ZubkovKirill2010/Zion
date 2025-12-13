using System.Diagnostics;

namespace Zion.Diagnostics
{
    public sealed class Tester<TIn, TOut>
    {
        private readonly (TIn, TOut)[] Tests;
        private readonly Func<TIn, TOut> Function;


        public Tester((TIn, TOut)[] Tests, Func<TIn, TOut> Function)
        {
            this.Tests = Tests;
            this.Function = Function;
        }

        public async Task<TestResult<TIn, TOut>[]> Run() => await Run(TimeSpan.FromSeconds(10));
        public async Task<TestResult<TIn, TOut>[]> Run(TimeSpan MaxTime)
        {
            TestResult<TIn, TOut>[] Results = new TestResult<TIn, TOut>[Tests.Length];

            ParallelOptions Options = new ParallelOptions
            {
                CancellationToken = CancellationToken.None
            };

            await Parallel.ForEachAsync(Enumerable.Range(0, Tests.Length), Options, async (i, Token) =>
            {
                using CancellationTokenSource TimeoutToken = new CancellationTokenSource(MaxTime);
                (TIn, TOut) Test = Tests[i];
                Stopwatch Stopwatch = Stopwatch.StartNew();

                try
                {
                    Task<TOut> FunctionTask = Task.Run(() => Function(Test.Item1));
                    Task TimeoutTask = Task.Delay(MaxTime, TimeoutToken.Token);

                    Task CompletedTask = await Task.WhenAny(FunctionTask, TimeoutTask);

                    if (CompletedTask == FunctionTask)
                    {
                        TOut Result = await FunctionTask;
                        Stopwatch.Stop();
                        Results[i] = new TestResult<TIn, TOut>(Test.Item1, Test.Item2, Stopwatch.Elapsed, Result);
                    }
                    else
                    {
                        TimeoutToken.Cancel();
                        Stopwatch.Stop();
                        Results[i] = new TestResult<TIn, TOut>(Test.Item1, Test.Item2);
                    }
                }
                catch (OperationCanceledException) when (TimeoutToken.Token.IsCancellationRequested)
                {
                    Stopwatch.Stop();
                    Results[i] = new TestResult<TIn, TOut>(Test.Item1, Test.Item2);
                }
                catch (Exception Exception)
                {
                    Stopwatch.Stop();
                    Results[i] = new TestResult<TIn, TOut>(Test.Item1, Test.Item2, Stopwatch.Elapsed, Exception);
                }
            });

            return Results;
        }
    }
}