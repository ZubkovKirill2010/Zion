using System.Collections;

namespace Zion.STP.Dynamic
{
    public sealed partial class RelativeTokenContainer<TPointer> : ITokenContainer<TPointer> where TPointer : TextPointer<TPointer>
    {
        private abstract class BaseNode : IEnumerable<Token>
        {
            public TPointer Position;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public abstract IEnumerator<Token> GetEnumerator();
        }

        private sealed class Node : BaseNode
        {
            public BaseNode? Left;
            public BaseNode? Right;

            public Node(BaseNode? Left)
            {
                this.Left = Left;
            }

            public Node(BaseNode? Left, BaseNode? Right) : this(Left)
            {
                this.Right = Right;
            }

            public override IEnumerator<Token> GetEnumerator()
            {
                if (Left is not null)
                {
                    foreach (Token Token in Left) { yield return Token; }
                }
                if (Right is not null)
                {
                    foreach (Token Token in Right) { yield return Token; }
                }
            }
        }

        private sealed class FinalNode : BaseNode
        {
            public readonly List<Token> Tokens;

            public FinalNode()
            {
                Tokens = new List<Token>();
            }
            public FinalNode(Token Token) : this()
            {
                Add(Token);
            }

            public void Add(Token Token)
            {
                Tokens.Add(Token);
            }

            public override IEnumerator<Token> GetEnumerator()
            {
                return Tokens.GetEnumerator();
            }
        }
    }
}