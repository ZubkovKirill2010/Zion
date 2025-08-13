namespace Zion.Text
{
    public static partial class Text
    {
        private static readonly Dictionary<char, char> Brackets = new Dictionary<char, char>()
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' }
        };


        #region Edit
        public static string RemoveChars(this string? String, Predicate<char> Condition)
        {
            if (string.IsNullOrEmpty(String))
            {
                return string.Empty;
            }

            List<char> Result = new List<char>(String.Length);

            foreach (char Char in String)
            {
                if (Condition(Char))
                {
                    continue;
                }
                Result.Add(Char);
            }

            return new string(Result.ToArray());
        }
        public static string RemoveChars(this string? String, params char[] RemovingChars)
        {
            return RemoveChars(String, RemovingChars.Contains);
        }

        public static string UnSpace(this string? String)
        {
            return String.RemoveChars(char.IsWhiteSpace);
        }
        public static string Minimize(this string? String)
        {
            return UnSpace(String).ToLower();
        }

        public static string Duplicate(this string String, int Count)
        {
            ArgumentNullException.ThrowIfNull(String);
            if (Count < 0)
            {
                throw new ArgumentException($"Count(={Count}) < 0");
            }
            if (Count == 0)
            {
                return string.Empty;
            }

            char[] Result = new char[String.Length * Count];
            int Index = 0;
            while (Index < String.Length)
            {
                for (int i = 0; i < String.Length; i++)
                {
                    Result[Index++] = String[i];
                }
            }
            return new string(Result);
        }

        public static string Capitalize(this string String)
        {
            if (string.IsNullOrEmpty(String))
            {
                return string.Empty;
            }

            char[] Result = String.ToCharArray();

            Result[0] = char.ToUpper(Result[0]);
            for (int i = 1; i < Result.Length; i++)
            {
                Result[i] = char.ToLower(Result[i]);
            }

            return new string(Result);
        }

        #endregion

        #region Predicate
        public static bool Begins(this string String, int Index, string Target)
        {
            ArgumentNullException.ThrowIfNull(String);

            if (Index >= String.Length)
            {
                throw new ArgumentException($"Index(={Index}) >= String.Length(={String.Length})");
            }
            if (String.Length - Index >= Target.Length)
            {
                return false;
            }

            for (int i = 0; i < Target.Length; i++)
            {
                if (String[Index + i] != Target[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool Exists(this string String, string Target, out int StartIndex)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Target);

            StartIndex = -1;

            if (String.Length == 0 && Target.Length == 0)
            {
                return true;
            }
            if (Target.Length > String.Length)
            {
                return false;
            }

            StartIndex = 0;
            int End = String.Length - Target.Length;
            bool Founded = true;

            while (StartIndex <= End)
            {
                for (int j = 0; j < Target.Length; j++)
                {
                    if (String[StartIndex + j] != Target[j])
                    {
                        Founded = false;
                        break;
                    }
                }

                if (Founded)
                {
                    return true;
                }

                StartIndex++;
            }

            StartIndex = -1;
            return false;
        }

        public static bool CheckBrackets(this string String)
        {
            return CheckBrackets(String, Brackets);
        }
        public static bool CheckBrackets(this string String, Dictionary<char, char> Brackets)
        {
            if (string.IsNullOrEmpty(String) || Brackets.Count == 0)
            {
                return true;
            }

            Stack<char> Stack = new Stack<char>(String.Length / 2 + 1);
            char Current = '\0';

            for (int i = 0; i < String.Length; i++)
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
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion


        public static int CountOf(this string String, char Target)
        {
            int Count = 0;

            foreach (char Char in String)
            {
                if (Char == Target)
                {
                    Count++;
                }
            }
            return Count;
        }


        public static void Skip(this string String, ref int Index, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);

            while (Index < String.Length && Condition(String[Index]))
            {
                Index++;
            }
        }
        public static int Skip(this string String, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);

            int Index = 0;
            while (Index < String.Length && Condition(String[Index]))
            {
                Index++;
            }
            return Index;
        }
        public static int Skip(this string String, int Start, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);

            int Index = Start;
            while (Index < String.Length && Condition(String[Index]))
            {
                Index++;
            }
            return Index;
        }

        public static string GetExpression(this string String, int Start, out int End)
        {
            int OpenBrackets = 0;
            int CloseBrackets = 0;

            for (int i = Start; i < String.Length; i++)
            {
                switch (String[i])
                {
                    case '(':
                        OpenBrackets++;
                        break;
                    case ')':
                        CloseBrackets++;
                        if (OpenBrackets == CloseBrackets)
                        {
                            End = i + 1;
                            return String[(Start + 1)..i];
                        }
                        break;
                }
            }
            throw new Exception();
        }


        public static IEnumerable<char> JoinLines(this IList<IEnumerable<char>> Lines)
        {
            if (Lines is null || Lines.Count == 0)
            {
                yield break;
            }

            foreach (char Char in Lines[0])
            {
                yield return Char;
            }

            string NewLine = Environment.NewLine;

            for (int i = 1; i < Lines.Count; i++)
            {
                foreach (char Char in NewLine) { yield return Char; }
                foreach (char Char in Lines[i]) { yield return Char; }
            }
        }


        private static bool IsClosingBracket(this char Char, out char Bracket)
        {
            foreach (KeyValuePair<char, char> Pair in Brackets)
            {
                if (Char == Pair.Value)
                {
                    Bracket = Pair.Value;
                    return true;
                }
            }

            Bracket = '\0';
            return false;
        }
    }
}