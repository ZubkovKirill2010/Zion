namespace Zion.MathExpressions
{
    public sealed class FunctionHandler
    {
        private readonly Dictionary<string, MathFunction> Functions;
        private readonly Dictionary<string, Fraction> Variables;

        public readonly string String;
        public readonly int Start;
        public int Index;

        private char Current => String[Index];
        public bool Finished { get; private set; }

        public FunctionHandler(MathExpressionParser Parser, string String, int Start)
        {
            Functions = Parser.Functions;
            Variables = Parser.Variables;

            this.String = String;
            this.Start = Start;
            Index = Start;
        }

        public IMathTerm ReadMathTerm()
        {
            return ReadMathTerm(out IMathTerm Result) ? Result : throw new ParsingException(String, Index, "Couldn't convert string to mathematical expression");
        }

        public bool ReadMathTerm(out IMathTerm Result)
        {
            Result = null;
            if (Finished) { return false; }

            MathExpressionParser Parser = new MathExpressionParser(String[Index..], Variables, Functions);

            Result = Parser.ParseBeforeSeporator(out int End);
            Index += End;

            return MemberParsed();
        }


        public int ReadInt32()
        {
            return ReadInt32(out int Result) ? Result : throw new ParsingException(String, Index, "Couldn't convert string to mathematical expression");
        }

        public bool ReadInt32(out int Result)
        {
            Result = default;
            if (Finished) { return false; }

            int End = String.Skip(Index, char.IsDigit);

            if (!int.TryParse(String[Index..End], out Result))
            {
                return false;
            }

            Index = End;

            return MemberParsed();
        }


        public bool MemberParsed()
        {
            if (Index >= String.Length)
            {
                throw new ParsingException(String, Index, "No closing brackets ')'");
            }
            if (Current is '|' or '\\')
            {
                Index++;

                return true;
            }
            if (Current == ')')
            {
                Index++;
                Finished = true;
                return true;
            }

            return false;
        }
    }
}