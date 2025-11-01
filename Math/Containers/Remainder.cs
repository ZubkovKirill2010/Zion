namespace Zion.MathExpressions
{
    public sealed class Remainder : IMathTerm
    {
        private readonly IMathTerm A;
        private readonly IMathTerm B;

        public Remainder(IMathTerm A, IMathTerm B)
        {
            this.A = A;
            this.B = B;
        }


        public Fraction GetValue(int Accuracy)
        {
            return A.GetValue(Accuracy) % B.GetValue(Accuracy);
        }
    }
}