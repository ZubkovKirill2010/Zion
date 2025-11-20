namespace Zion.MathExpressions
{
    public sealed class RangeAmount : IMathTerm
    {
        private readonly IMathTerm A, B;

        public RangeAmount(IMathTerm Min, IMathTerm Max)
        {
            A = Min;
            B = Max;
        }

        public Fraction GetValue(int Accuracy)
        {
            return Fraction.SumRange(A.GetValue(Accuracy), B.GetValue(Accuracy));
        }
    }
}