namespace Zion.STP
{
    public class Symbol
    {
        public readonly Node Source;
        public readonly Symbol? Parent;
        public readonly List<Symbol> Childs;

        public Symbol(Node Source)
        {
            this.Source = Source;
            this.Childs = new List<Symbol>();
        }

        public Symbol(Node Source, Symbol? Parent) : this(Source)
        {
            this.Parent = Parent;
        }
    }
}