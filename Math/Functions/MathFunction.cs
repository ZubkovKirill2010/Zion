namespace Zion.MathExpressions
{
    public readonly struct MathFunction
    {
        public readonly MathFunctionHandler Handler;
        public readonly ParameterInfo[][] Overloads;
        public readonly string Info;

        public MathFunction(MathFunctionHandler Handler, ParameterInfo[][] Overloads, string Info)
        {
            this.Handler = Handler;
            this.Overloads = Overloads;
            this.Info = Info;
        }
        public MathFunction(MathFunctionHandler Handler, ParameterInfo[] Overloads, string Info)
            : this(Handler, [Overloads], Info) { }
        public MathFunction(MathFunctionHandler Handler, ParameterInfo Overloads, string Info)
            : this(Handler, [[Overloads]], Info) { }
    }

    public readonly struct ParameterInfo
    {
        public readonly Type Type;
        public readonly string Name;
        public bool IsEnumerable { get; init; }

        public ParameterInfo(Type Type, string Name)
        {
            this.Type = Type;
            this.Name = Name;
        }

        public override string ToString()
        {
            return $"{(IsEnumerable ? "param" : string.Empty)} {Type.Name} {Name}";
        }
    }
}