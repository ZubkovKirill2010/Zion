namespace Zion.MathExpressions
{
    public sealed class FloorDivision : IMathTerm
    {
        private readonly IMathTerm A;
        private readonly IMathTerm B;

        public FloorDivision(IMathTerm A, IMathTerm B)
        {
            this.A = A;
            this.B = B;
        }


        public Fraction GetValue(int Accuracy)
        {
            return Fraction.Floor(A.GetValue(Accuracy) / B.GetValue(Accuracy));
        }
    }
}