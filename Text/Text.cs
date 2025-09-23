using System.Collections.ObjectModel;
using System.Text;

namespace Zion
{
    public static class Text
    {
        public static readonly ReadOnlyDictionary<char, char> Brackets = new ReadOnlyDictionary<char, char>
        (
            new Dictionary<char, char>()
            {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' }
            }
        );


        #region Edit
        public static string RemoveChars(this string? String, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);

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
            ArgumentNullException.ThrowIfNullOrEmpty(String);

            char[] Result = String.ToCharArray();

            Result[0] = char.ToUpper(Result[0]);
            for (int i = 1; i < Result.Length; i++)
            {
                Result[i] = char.ToLower(Result[i]);
            }

            return new string(Result);
        }

        public static string[] SplitIntoLines(this string String)
        {
            ArgumentNullException.ThrowIfNull(String);
            if (String.Length == 0)
            {
                return Array.Empty<string>();
            }

            return String.Split(Environment.NewLine, StringSplitOptions.None).TrimEmptyLines();
        }

        public static string[] TrimEmptyLines(this string[] StringArray)
        {
            ArgumentNullException.ThrowIfNull(StringArray);
            if (StringArray.Length == 0)
            {
                return StringArray;
            }

            int Start = 0;
            int End = StringArray.Length - 1;

            while (Start <= End && string.IsNullOrWhiteSpace(StringArray[Start]))
            {
                Start++;
            }

            while (End >= Start && string.IsNullOrWhiteSpace(StringArray[End]))
            {
                End--;
            }

            if (Start > End)
            {
                return Array.Empty<string>();
            }

            return StringArray[Start..(End + 1)];
        }

        public static string Centering(this string String, int TotalLength, char PaddingChar = ' ')
        {
            ArgumentNullException.ThrowIfNull(String);
            if (TotalLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(TotalLength), $"TotalLength(={TotalLength}) <= 0");
            }

            if (String.Length >= TotalLength)
            {
                return String;
            }

            int LeftPadding = TotalLength / String.Length;
            int RightPadding = TotalLength - LeftPadding - String.Length;

            return new StringBuilder(TotalLength)
                .Append(PaddingChar, LeftPadding)
                .Append(String)
                .Append(PaddingChar, RightPadding)
                .ToString();
        }

        public static string Truncate(this string String, int TotalLength)
        {
            ArgumentNullException.ThrowIfNull(String);
            if (TotalLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(TotalLength), $"TotalLength(={TotalLength}) <= 0");
            }

            return String.Length <= TotalLength ? String : String[..(TotalLength - 3)] + "...";
        }

        public static string[] Chunk(this string String, int ChunkLength)
        {
            if (string.IsNullOrEmpty(String) || ChunkLength <= 0)
            {
                return Array.Empty<string>();
            }

            string[] Chunks = new string[(int)Math.Ceiling(String.Length / (double)ChunkLength)];

            int End = Chunks.Length - 1;
            int ChunkIndex = 0;
            int Index = 0;

            while (ChunkIndex < End)
            {
                int EndIndex = Index + ChunkLength;
                Chunks[ChunkIndex] = String.Substring(Index, ChunkLength);
                Index = EndIndex;
                ChunkIndex++;
            }

            Chunks[End] = String.Substring(Index);

            return Chunks;
        }

        public static string[] ChunkFromRight(this string String, int ChunkLength)
        {
            if (string.IsNullOrEmpty(String) || ChunkLength <= 0)
            {
                return Array.Empty<string>();
            }

            string[] Chunks = new string[(String.Length + ChunkLength - 1) / ChunkLength];

            int End = Chunks.Length - 1;
            int ChunkIndex = End;
            int Index = String.Length;

            while (ChunkIndex > 0)
            {
                int StartIndex = Index - ChunkLength;
                Chunks[ChunkIndex] = String.Substring(StartIndex, ChunkLength);
                Index = StartIndex;
                ChunkIndex--;
            }

            Chunks[0] = String.Substring(0, Index);

            return Chunks;
        }

        public static string ConvertAll(this string String, Func<char, char> Converter)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Converter);

            if (String.Length == 0)
            {
                return String;
            }

            return string.Create
            (
                String.Length,
                String,
                (Span, Original) =>
                {
                    for (int i = 0; i < Original.Length; i++)
                    {
                        Span[i] = Converter(Original[i]);
                    }
                }
            );
        }

        #endregion

        #region Prefix & Suffix

        public static string RemovePrefix(this string String, string Prefix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Prefix);

            if (String.Length == 0 || String.Length < Prefix.Length)
            {
                return String;
            }

            return String.StartsWith(Prefix) ? String[Prefix.Length..] : String;
        }
        public static string RemoveSuffix(this string String, string Suffix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Suffix);

            return String.EndsWith(Suffix) ? String[Suffix.Length..] : String;
        }

        public static string EnsurePrefix(this string String, string Prefix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Prefix);

            if (Prefix.Length == 0)
            {
                return String;
            }

            return String.StartsWith(Prefix) ? String : Prefix + String;
        }
        public static string EnsureSuffix(this string String, string Suffix)
        {
            ArgumentNullException.ThrowIfNull(String);
            ArgumentNullException.ThrowIfNull(Suffix);

            if (Suffix.Length == 0)
            {
                return String;
            }

            return String.EndsWith(Suffix) ? String : String + Suffix;
        }

        public static string GetCommonStart(this IEnumerable<string> Strings)
        {
            ArgumentNullException.ThrowIfNull(Strings);
            if (Enumerable.Any(Strings, String => String is null))
            {
                throw new ArgumentNullException("StringArray[`] is null");
            }
            if (Strings.IsEmpty())
            {
                return string.Empty;
            }

            int End = Strings.Select(String => String.Length).Min();

            if (End == 0)
            {
                return string.Empty;
            }

            string First = Strings.First();

            for (int i = 0; i < End; i++)
            {
                char Char = First[i];

                if (Strings.Any(String => String[i] != Char))
                {
                    return First[..i];
                }
            }

            return First[..End];
        }

        #endregion

        #region Predicate
        public static bool Begins(this string String, int Index, string Target, bool IgnoreCase = false)
        {
            ArgumentNullException.ThrowIfNull(String);

            if (Index >= String.Length)
            {
                throw new ArgumentException($"Index(={Index}) >= String.Length(={String.Length})");
            }
            if (String.Length - Index < Target.Length)
            {
                return false;
            }

            if (IgnoreCase)
            {
                for (int i = 0; i < Target.Length; i++)
                {
                    if (char.ToLower(String[Index + i]) != char.ToLower(Target[i]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Target.Length; i++)
                {
                    if (String[Index + i] != Target[i])
                    {
                        return false;
                    }
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
        public static bool CheckBrackets(this string String, IDictionary<char, char> Brackets)
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

        public static bool TrueFor(this string String, int Start, int End, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);
            if (Start < 0)
            {
                throw new ArgumentOutOfRangeException($"Start(={Start}) < 0");
            }
            if (End > String.Length)
            {
                throw new ArgumentOutOfRangeException($"End(={End}) >= String.Length(={String.Length})");
            }

            for (int i = Start; i < End; i++)
            {
                if (!Condition(String[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsOpenBracket(this char Char)
        {
            return Brackets.ContainsKey(Char);
        }
        public static bool IsOpenBracket(this char Char, out char ClosingBracket)
        {
            return Brackets.TryGetValue(Char, out ClosingBracket);
        }

        #endregion

        #region Programming
        public static bool IsIdentifier(this string String)
        {
            ArgumentNullException.ThrowIfNull(String);

            if (String.Length == 0 ||
                char.IsNumber(String[0])
               )
            {
                return false;
            }

            for (int i = 1; i < String.Length; i++)
            {
                char Char = String[i];

                if (!(Char.IsEnglish() || Char == '_' || char.IsDigit(Char)))
                {
                    return false;
                }
            }

            return true;
        }

        public static string[] SplitIdentifier(this string String)
        {
            if (string.IsNullOrEmpty(String))
            {
                return Array.Empty<string>();
            }

            List<string> Result = new List<string>();
            StringBuilder CurrentWord = new StringBuilder();
            bool? LastWasUpper = null;
            bool LastWasDigit = false;


            void AddWord()
            {
                if (CurrentWord.Length > 0)
                {
                    Result.Add(CurrentWord.ToString());
                    CurrentWord.Clear();
                }
            }


            for (int i = 0; i < String.Length; i++)
            {
                char Char = String[i];

                if (Char == '_' || Char == ' ' || Char == '-')
                {
                    AddWord();
                    LastWasUpper = null;
                    LastWasDigit = false;
                    continue;
                }

                if (char.IsDigit(Char))
                {
                    if (!LastWasDigit && CurrentWord.Length > 0)
                    {
                        AddWord();
                    }
                    CurrentWord.Append(Char);
                    LastWasDigit = true;
                    LastWasUpper = false;
                    continue;
                }

                if (char.IsUpper(Char))
                {
                    if (CurrentWord.Length > 0 && LastWasUpper == false)
                    {
                        AddWord();
                    }
                    else if (CurrentWord.Length > 0 && LastWasUpper == true &&
                            i < String.Length - 1 && char.IsLower(String[i + 1]))
                    {
                        AddWord();
                    }

                    CurrentWord.Append(Char);
                    LastWasUpper = true;
                    LastWasDigit = false;
                }
                else
                {
                    CurrentWord.Append(Char);
                    LastWasUpper = false;
                    LastWasDigit = false;
                }
            }

            AddWord();

            return Result.ToArray();
        }


        public static string ToCamelCase(this IList<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);
            if (Strings.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder Builder = new StringBuilder(Strings.Summarize(String => String.Length) + (Underlining ? 1 : 0));

            Builder.Append(Strings[0].ToLower());
            for (int i = 1; i < Strings.Count; i++)
            {
                Builder.Append(Strings[i].Capitalize());
            }

            string Result = Builder.ToString();
            return Underlining ? '_' + Result : Result;
        }

        public static string ToPascalCase(this ICollection<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);
            if (Strings.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder Builder = new StringBuilder(Strings.Summarize(String => String.Length) + (Underlining ? 1 : 0));

            foreach (string Word in Strings)
            {
                Builder.Append(Word.Capitalize());
            }

            string Result = Builder.ToString();
            return Underlining ? '_' + Result : Result;
        }

        public static string ToSnakeCase(this ICollection<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);
            if (Strings.Count == 0)
            {
                return string.Empty;
            }

            string Result = string.Join('_', Strings.Select(String => String.ToLower()));
            return Underlining ? '_' + Result : Result;
        }

        public static string ToKebabCase(this ICollection<string> Strings, bool Underlining = false)
        {
            ArgumentNullException.ThrowIfNull(Strings);
            if (Strings.Count == 0)
            {
                return string.Empty;
            }

            string Result = string.Join('-', Strings.Select(String => String.ToLower()));
            return Underlining ? '_' + Result : Result;
        }

        #endregion

        #region Indexes
        public static int GetLevel(this string String)
        {
            int Level = 0;

            foreach (char Char in String)
            {
                if (Char == '\t')
                {
                    Level++;
                }
                else
                {
                    break;
                }
            }

            return Level;
        }

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

        public static string JoinTrimEnd(this IList<string> Array, string Seporator)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentNullException.ThrowIfNull(Seporator);

            int End = Array.EndIndexOf(s => !string.IsNullOrWhiteSpace(s));
            if (End == -1)
            {
                return string.Empty;
            }

            StringBuilder Builder = new StringBuilder(Array.Summarize(s => s.Length, End + 1) + (Seporator.Length * End));

            Builder.Append(Array[0]);
            for (int i = 1; i <= End; i++)
            {
                Builder.Append(Seporator);
                Builder.Append(Array[i]);
            }

            return Builder.ToString();
        }

        #endregion

        #region Skipping

        public static void SkipSpaces(this string String, ref int Index)
        {
            Skip(String, ref Index, char.IsWhiteSpace);
        }
        public static int SkipSpaces(this string String, int Start)
        {
            return Skip(String, Start, char.IsWhiteSpace);
        }

        public static void Skip(this string String, ref int Index, Predicate<char> Condition)
        {
            ArgumentNullException.ThrowIfNull(String);

            while (Index < String.Length && Condition(String[Index]))
            {
                Index++;
            }
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
        public static int Skip(this string String, int Start, params IEnumerable<char> SkipChars)
        {
            ArgumentNullException.ThrowIfNull(String);

            int Index = Start;
            while (Index < String.Length && SkipChars.Contains(String[Index]))
            {
                Index++;
            }
            return Index;
        }

        #endregion

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

            if (Lines.Count == 1)
            {
                foreach (char Char in Lines.First()) { yield return Char; }
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


        internal static bool IsClosingBracket(this char Char, out char Bracket)
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