using System.Text;
using Zion.Text;

namespace Zion
{
    public static class StringParser
    {
        private static readonly Dictionary<char, char> SpecialChars = new()
        {
            { '"', '"' },
            { '0', '\0' },
            { 'a', '\a' },
            { 'b', '\b' },
            { 'f', '\f' },
            { 't', '\t' },
            { 'v', '\v' }
        };


        /// <summary>
        /// Returns a string with unicode characters.
        /// If the value of String is null, 
        /// the method returns an empty string.
        /// </summary>
        /// <param name="String"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Parse(string String)
        {
            if (String is null)
            {
                return string.Empty;
            }

            int Start = 0;
            int Index = 0;
            StringBuilder Builder = new StringBuilder(String.Length + 30);

            while (Index < String.Length)
            {
                if (String[Index] == '\\')
                {
                    Index++;
                    if (Index >= String.Length)
                    {
                        Builder.Append('\\');
                        break;
                    }

                    char Char = String[Index];

                    switch (Char)
                    {
                        case 'u':
                            if (Index + 4 >= String.Length)
                            {
                                while (Index < String.Length)
                                {
                                    Builder.Append(String[Index]);
                                    Index++;
                                }
                                break;
                            }
                            Start = Index + 1;
                            Index += 4;

                            Builder.Append
                            (
                                char.ConvertFromUtf32
                                (
                                    Convert.ToInt32
                                    (
                                        String[Start..Index],
                                        16
                                    )
                                )
                            );
                            break;

                        case 'n':
                            Builder.AppendLine();
                            break;

                        case 'x':
                            if (Index + 2 >= String.Length)
                            {
                                while (Index < String.Length)
                                {
                                    Builder.Append(String[Index]);
                                    Index++;
                                }
                                break;
                            }
                            Start = Index + 1;
                            Index += 2;

                            Builder.Append
                            (
                                char.ConvertFromUtf32
                                (
                                    Convert.ToInt32
                                    (
                                        String[Start..Index],
                                        16
                                    )
                                )
                            );
                            break;


                        default:
                            if (SpecialChars.TryGetValue(Char, out char SpecialChar))
                            {
                                Builder.Append(SpecialChar);
                            }
                            else
                            {
                                Builder.Append('\\');
                                Builder.Append(Char);
                            }
                            break;
                    }
                }
                else
                {
                    Builder.Append(String[Index]);
                }

                Index++;
            }

            return Builder.ToString();
        }

        /// <summary>
        /// Returns the index of the closing quotation mark
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int GetEndOfExpression(string String, int Start = 0)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(String);
            if (Start < 0 || Start >= String.Length)
            {
                throw new Exception();
                throw new ArgumentOutOfRangeException($"Start(={Start}) out of range [0-{String.Length}]");
            }
            if (String[Start] == '"')
            {
                return String.Length - 1;
            }

            int Index = Start + 1;

            while (Index < String.Length)
            {
                if (String[Index] == '"' && (String[Index - 1] != '\\' || String[Index - 2] != '\\'))
                {
                    return Index;
                }
                Index++;
            }

            return String.Length - 1;
        }

        public static int GetEndOfBracketsExpression(string String, int Start = 0)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);
            if (!Start.IsClamp(0, String.Length - 1))
            {
                throw new ArgumentOutOfRangeException($"Start(={Start}) < 0 or >= String.Length(={String.Length})");
            }

            Dictionary<char, char> Brackets = Text.Text.Brackets;
            Stack<char> Stack = new Stack<char>(String.Length / 2 + 1);
            char Current = '\0';

            for (int i = Start; i < String.Length; i++)
            {
                Current = String[i];

                if (Brackets.TryGetValue(Current, out char Bracket))
                {
                    Stack.Push(Bracket);
                }
                else if (Current.IsClosingBracket(out char ClosingBracket))
                {
                    if (Stack.Count <= 0 || ClosingBracket != Stack.Pop())
                    {
                        throw new Exception("String has an incorrect bracket arrangement");
                    }
                }

                if (Stack.Count == 0)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}