namespace Zion.MathExpressions
{
    public sealed class Product : IFraction
    {
        private List<IFraction> Members;

        public Product()
        {
            Members = new List<IFraction>(4);
        }


        public void operator *=(IFraction Value)
        {
            Members.Add(Value);
        }
        
        public void operator /=(IFraction Value)
        {
            Members.Add(new Inversed(Value));
        }


        public Fraction GetValue(int Accuracy)
        {
            return Fraction.Product(Members.Select(Item => Item.GetValue(Accuracy)));
        }
    }
}