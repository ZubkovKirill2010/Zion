namespace Zion.STP
{
    public interface ITemplate
    {
        bool IsMatch(StringView String, int Start, out Block Block);
    }

    public interface ITokenTemplate : ITemplate
    {
        RGBColor Color { get; init; }
    }

    public interface IGroupTemplate : ITemplate, IEnumerable<ITemplate>
    {
        int Count { get; }

        ITemplate this[int Index] { get; }
    }
}