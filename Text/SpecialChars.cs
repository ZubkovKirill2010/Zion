using System.Collections.ObjectModel;

namespace Zion
{
    public static class SpecialChars
    {
        public const char Pi = '\u03C0';
        public const char Sqrt = '\u221A';

        public static readonly ReadOnlyDictionary<char, (char Normal, char Index, char Degree)> Digits = new ReadOnlyDictionary<char, (char, char, char)>
        (
            new Dictionary<char, (char Normal, char Index, char Degree)>()
            {
                { '-', ('-', '\u208B', '\u207B') },
                { '+', ('+', '\u208A', '\u207A') },

                { '\u208B', ('-', '\u208B', '\u207B') },
                { '\u208A', ('+', '\u208A', '\u207A') },

                { '\u207B', ('-', '\u208B', '\u207B') },
                { '\u207A', ('+', '\u208A', '\u207A') },

                { '0', ('0', '\u2080', '\u2070') },
                { '1', ('1', '\u2081', '\u00B9') },
                { '2', ('2', '\u2082', '\u00B2') },
                { '3', ('3', '\u2083', '\u00B3') },
                { '4', ('4', '\u2084', '\u2074') },
                { '5', ('5', '\u2085', '\u2075') },
                { '6', ('6', '\u2086', '\u2076') },
                { '7', ('7', '\u2087', '\u2077') },
                { '8', ('8', '\u2088', '\u2078') },
                { '9', ('9', '\u2089', '\u2079') },

                { '\u2070', ('0', '\u2080', '\u2070') },
                { '\u00B9', ('1', '\u2081', '\u00B9') },
                { '\u00B2', ('2', '\u2082', '\u00B2') },
                { '\u00B3', ('3', '\u2083', '\u00B3') },
                { '\u2074', ('4', '\u2084', '\u2074') },
                { '\u2075', ('5', '\u2085', '\u2075') },
                { '\u2076', ('6', '\u2086', '\u2076') },
                { '\u2077', ('7', '\u2087', '\u2077') },
                { '\u2078', ('8', '\u2088', '\u2078') },
                { '\u2079', ('9', '\u2089', '\u2079') },

                { '\u2080', ('0', '\u2080', '\u2070') },
                { '\u2081', ('1', '\u2081', '\u00B9') },
                { '\u2082', ('2', '\u2082', '\u00B2') },
                { '\u2083', ('3', '\u2083', '\u00B3') },
                { '\u2084', ('4', '\u2084', '\u2074') },
                { '\u2085', ('5', '\u2085', '\u2075') },
                { '\u2086', ('6', '\u2086', '\u2076') },
                { '\u2087', ('7', '\u2087', '\u2077') },
                { '\u2088', ('8', '\u2088', '\u2078') },
                { '\u2089', ('9', '\u2089', '\u2079') }
            }
        );

        public static char ToIndexDigit(char Char)
        {
            return GetChar(Char, Group => Group.Index);
        }
        public static char NormalizeDigit(char Char)
        {
            return GetChar(Char, Group => Group.Degree);
        }
        public static char ToDegreeDigit(char Char)
        {
            return GetChar(Char, Group => Group.Degree);
        }

        private static char GetChar(char Char, Func<(char Normal, char Index, char Degree), char> GetValue)
        {
            return Digits.TryGetValue(Char, out (char Normal, char Index, char Degree) Value) ? GetValue(Value) : Char;
        }
    }
}