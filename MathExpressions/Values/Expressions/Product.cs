namespace Zion.MathExpressions
{
    public sealed class Product : IExpression
    {
        private List<ProductNumber> Members;

        public Product()
        {
            Members = new List<ProductNumber>(10);
        }

        public override string ToString()
        {
            return $"({string.Join(' ', Members)})";
        }


        public Fraction GetValue()
        {
            Fraction Result = new Fraction(1, 1);

            foreach (ProductNumber Item in Members)
            {
                if (Item.IsProduct)
                {
                    Result *= Item.Value.GetValue();
                }
                else
                {
                    Result /= Item.Value.GetValue();
                }
            }

            return Result;
        }

        public ProductNumber Add(ProductNumber Value)
        {
            Members.Add(Value);
            return Value;
        }
        public IExpression Add(IExpression Value, bool IsProduct = true)
        {
            Members.Add(new ProductNumber(Value, IsProduct));
            return Value;
        }

        public IExpression GetExpression()
        {
            if (Members.Count == 1)
            {
                return Members[0].Value;
            }
            return this;
        }
    }
}