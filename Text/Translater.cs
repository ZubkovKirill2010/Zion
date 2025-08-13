namespace Zion.Text
{
    public static class Translater
    {
        private static readonly HashSet<char> UniversalChar;
        private static readonly Dictionary<char, char> RusEngLayout;
        private static readonly Dictionary<char, char> EngRusLayout;

        static Translater()
        {
            UniversalChar = new()
            {
                ' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '!', '%', '*', '(', ')', '_', '+', '-', '=', '\\'
            };
            EngRusLayout = new()
            {
                {'q', 'й'}, {'w', 'ц'}, {'e', 'у'}, {'r', 'к'}, {'t', 'е'}, {'y', 'н'}, {'u', 'г'}, {'i', 'ш'}, {'o', 'щ'}, {'p', 'з'},
                {'a', 'ф'}, {'s', 'ы'}, {'d', 'в'}, {'f', 'а'}, {'g', 'п'}, {'h', 'р'}, {'j', 'о'}, {'k', 'л'}, {'l', 'д'},
                {'z', 'я'}, {'x', 'ч'}, {'c', 'с'}, {'v', 'м'}, {'b', 'и'}, {'n', 'т'}, {'m', 'ь'},

                {'@', '\"'}, {'#', '№'}, {'$', ';'}, {'^', ':'}, {'&', '?'},
                { '`', 'ё'}, {'[', 'х'}, {']', 'ъ'}, {';', 'ж'}, {'\'', 'э'}, {',', 'б'}, {'.', 'ю'}, {'/', '.'},
                { '~', 'Ё'}, {'{', 'Х'}, {'}', 'Ъ'}, {':', 'Ж'}, {'\"', 'Э'}, {'<', 'Б'}, {'>', 'Ю'}, {'?', ','}
            };
            RusEngLayout = EngRusLayout.Reverse();
        }


        public static string ToEnglish(string Input)
        {
            char[] InputToChar = Input.ToCharArray();

            for (int i = 0; i < Input.Length; i++)
            {
                InputToChar[i] = ToEnglish(InputToChar[i]);
            }
            return new string(InputToChar);
        }

        public static string ToRussian(string Input)
        {
            char[] InputToChar = Input.ToCharArray();

            for (int i = 0; i < Input.Length; i++)
            {
                InputToChar[i] = ToRussian(InputToChar[i]);
            }
            return new string(InputToChar);
        }


        public static string Translate(string Input)
        {
            string[] Words = Input.Split(' ');

            for (int i = 0; i < Words.Length; i++)
            {
                Words[i] = TranslateWord(Words[i]);
            }

            return string.Join(" ", Words);
        }

        public static string[] Translate(string[] Lines)
        {
            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = Translate(Lines[i]);
            }
            return Lines;
        }

        public static string TranslateWord(string Word)
        {
            if (string.IsNullOrWhiteSpace(Word))
            {
                return Word;
            }

            for (int i = 0; i < Word.Length; i++)
            {
                if (IsEnglish(Word[i]))
                {
                    return ToRussian(Word);
                }
                else if (IsRussian(Word[i]))
                {
                    return ToEnglish(Word);
                }
            }
            return Word;
        }


        public static char ToEnglish(char String)
        {
            return IsEnglish(String) ? String : Translate(String, RusEngLayout);
        }

        public static char ToRussian(char String)
        {
            return IsRussian(String) ? String : Translate(String, EngRusLayout);
        }

        public static char Translate(char String)
        {
            return IsEnglish(String) ? ToRussian(String) : ToEnglish(String);
        }


        public static bool IsEnglish(this char Char)
        {
            int CharIndex = Char;
            return 65 <= CharIndex && CharIndex <= 90 || 97 <= CharIndex && CharIndex <= 122;
        }

        public static bool IsRussian(this char Char)
        {
            int CharIndex = Char;
            return 1040 <= CharIndex && CharIndex <= 1103;
        }


        private static char Translate(char Char, Dictionary<char, char> Dictionary)
        {
            bool IsUpper = char.IsUpper(Char);
            char LowerChar = char.ToLower(Char);

            if (!UniversalChar.Contains(Char) && IsLetter(Char) && Dictionary.TryGetValue(Char, out char Value))
            {
                return IsUpper ? char.ToUpper(Value) : Value;
            }

            return Char;
        }

        private static bool IsLetter(char Input)
        {
            return IsRussian(Input) || IsEnglish(Input);
        }
    }
}