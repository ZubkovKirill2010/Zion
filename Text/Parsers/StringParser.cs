using System.Text;

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
            StringBuilder Builder = new StringBuilder(String.Length);

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
                            if (Index + 5 >= String.Length)
                            {
                                while (Index < String.Length)
                                {
                                    Builder.Append(String[Index]);
                                    Index++;
                                }
                                break;
                            }
                            Start = Index + 1;
                            Index += 5;

                            string Unicode = String[Start..Index];

                            try
                            {
                                Builder.Append
                                (
                                    char.ConvertFromUtf32(Convert.ToInt32(Unicode, 16))
                                );
                            }
                            catch
                            {
                                Builder.Append("\\u");
                                Builder.Append(Unicode);
                            }
                            break;

                        case 'n':
                            Builder.AppendLine();
                            break;

                        case 'x':
                            if (Index + 3 >= String.Length)
                            {
                                while (Index < String.Length)
                                {
                                    Builder.Append(String[Index]);
                                    Index++;
                                }
                                break;
                            }
                            Start = Index + 1;
                            Index += 3;

                            string xUnicode = String[Start..Index];

                            try
                            {
                                Builder.Append
                                (
                                    char.ConvertFromUtf32(Convert.ToInt32(xUnicode, 16))
                                );
                            }
                            catch
                            {
                                Builder.Append("\\x");
                                Builder.Append(xUnicode);
                            }
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
        /// If there is no closing quotation mark in the string, returns -1
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int GetEndOfExpression(string String, int Start = 0)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(String);
            ArgumentOutOfRangeException.ThrowIfWithout(Start, String);

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

            return -1;
        }

        public static int GetEndOfBracketsExpression(string String, int Start = 0)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);
            ArgumentOutOfRangeException.ThrowIfWithout(Start, String);

            IDictionary<char, char> Brackets = Text.Brackets;
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