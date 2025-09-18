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

        public TestResult<TIn, TOut>[] Test()
        {
            return Array.ConvertAll
            (
                Tests,
                Test =>
                {
                    try
                    {
                        return new TestResult<TIn, TOut>(Test.Item1, Test.Item2, Function(Test.Item1));
                    }
                    catch (Exception Exception)
                    {
                        return new TestResult<TIn, TOut>(Test.Item1, Test.Item2, Exception);
                    }
                }
            );
        }
    }
}