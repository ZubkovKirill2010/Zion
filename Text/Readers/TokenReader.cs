//using System.Text;

//namespace Zion
//{
//    public class TokenReader : TextReader
//    {
//        private string String;
//        private Predicate<char> SkipCharacter;
//        public int Index { get; private set; }

//        public TokenReader(string String)
//        {
//            ArgumentNullException.ThrowIfNull(String);
//            this.String = String;
//            SkipCharacter = Char => false;
//        }
//        public TokenReader(string String, Predicate<char> SkipCharacter)
//        {
//            ArgumentNullException.ThrowIfNull(String);
//            ArgumentNullException.ThrowIfNull(SkipCharacter);
//            this.String = String;
//            this.SkipCharacter = SkipCharacter;
//        }


//        public string? ReadToken()
//        {
//            SkipCharacters();

//            int NextChar = Reader.Peek();
//            if (NextChar == -1)
//            {
//                return null;
//            }

//            StringBuilder buffer = new StringBuilder();

//            while (true)
//            {
//                NextChar = Reader.Peek();
//                if (NextChar == -1)
//                {
//                    break;
//                }

//                char CurrentChar = (char)NextChar;
//                if (IsSkiping(CurrentChar))
//                {
//                    break;
//                }

//                buffer.Append(CurrentChar);
//                Reader.Read();
//            }

//            if (buffer.Length == 0)
//            {
//                return null;
//            }

//            return buffer.ToString();
//        }

//        public IEnumerable<string> ReadTokens()
//        {
//            string? Token;
//            while ((Token = ReadToken()) is not null)
//            {
//                yield return Token;
//            }
//        }

//        private void SkipCharacters()
//        {
//            while (true)
//            {
//                int NextChar = Reader.Peek();
//                if (NextChar == -1)
//                {
//                    break;
//                }

//                char CurrentChar = (char)NextChar;
//                if (!IsSkiping(CurrentChar))
//                {
//                    break;
//                }

//                Reader.Read();
//            }
//        }

//        public override int Peek()
//        {
//            return String[Index];
//        }

//        public override int Read()
//        {
//            return String[Index++];
//        }

//        public override int Read(char[] Buffer, int Index, int Count)
//        {
//            return Reader.Read(Buffer, Index, Count);
//        }

//        public override string? ReadLine()
//        {
//            return Reader.ReadLine();
//        }

//        public override string ReadToEnd()
//        {
//            return Reader.ReadToEnd();
//        }


//        protected override void Dispose(bool Disposing)
//        {
//            if (Disposing)
//            {
//                String = null;
//            }
//            base.Dispose(Disposing);
//        }
//    }
//}