namespace Zion.STP
{
    public interface ITemplate
    {
        public bool IsMatch(StringView String, int Start, out Token Block);
    }

    public interface ITokenTemplate : ITemplate
    {
        public Color Color { get; }
    }

    public interface IGroupTemplate : ITemplate, IEnumerable<ITemplate>
    {
        public int Count { get; }

        public ITemplate this[int Index] { get; }
    }
}