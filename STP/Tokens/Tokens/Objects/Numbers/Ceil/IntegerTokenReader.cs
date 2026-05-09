using System.Numerics;

namespace Zion.STP
{
    public sealed class IntegerTokenReader<T, I> : ITokenReader where T : IValueToken<I>, new() where I : IComparable<I>, INumber<I>, new()
    {
        public readonly NumberParsingParameters<I> NumberParameters;
        
        public bool IgnoreSuffixes { get; init; }

        public IntegerTokenReader(NumberParsingParameters<I> Parameters)
        {
            NumberParameters = Parameters;
        }


        public bool Read(ref TextSource Source, out IToken Token)
        {
            bool IsNegative = false;
            int CharCount = 0;

            if (Source.Current == '-')
            {
                if (NumberParameters.OnlyPositive)
                {
                    Token = default!;
                    return false;
                }

                Source.MoveNext();
                IsNegative = true;
                CharCount++;
            }

            if (Source.Begins("0b", out Source))
            {
                return ReadBinary(ref Source, out Token, IsNegative, CharCount + 2);
            }
            if (Source.Begins("0o", out Source))
            {
                return ReadOctal(ref Source, out Token, IsNegative, CharCount + 2);
            }
            if (Source.Begins("0x", out Source))
            {
                return ReadHexadecimal(ref Source, out Token, IsNegative, CharCount + 2);
            }

            return ReadDecimal(ref Source, out Token, IsNegative, CharCount);
        }


        private bool ReadBinary(ref TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            return Read(ref Source, out Token, IsNegative, CharCount, IsBinary, 1, 2);
        }

        private bool ReadOctal(ref TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            return Read(ref Source, out Token, IsNegative, CharCount, IsOctal, 3, 8);
        }

        private bool ReadHexadecimal(ref TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            return Read(ref Source, out Token, IsNegative, CharCount, IsHexadecimal, 4, 16);
        }

        private bool ReadDecimal(ref TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            SkipZeros(Source, ref CharCount);
            InitializeVariables(out Token, out I Value, out NumberParsingParameters<I> Parameters);

            while (!Source.IsEnd)
            {
                if (IsSeporator(Source, ref CharCount))
                {
                    continue;
                }

                if (BeginsSuffix(ref Source, ref CharCount))
                {
                    break;
                }

                if (IsDecimal(Source.Current, out int Digit))
                {
                    if (IsNegative)
                    {
                        Digit = -Digit;
                    }

                    if (CheckOverflow(Value, Digit))
                    {
                        Token = ReadErrorToken(ref Source, CharCount, static Char => Char.IsDigit());
                        return true;
                    }

                    Value = Parameters.Multiply(Value, 10);

                    if (Digit != 0)
                    {
                        Value = Parameters.Sum(Value, Digit);
                    }
                }
                else
                {
                    return false;
                }

                Source.MoveNext();
                CharCount++;
            }

            Token = CreateToken(Value, CharCount);
            return true;
        }


        private bool Read(ref TextSource Source, out IToken Token, bool IsNegative, int CharCount, SafeConverter<char, int> IsDigit, int BinaryLength, int CalculusSystem)
        {
            SkipZeros(Source, ref CharCount);
            InitializeVariables(out Token, out I Value, out NumberParsingParameters<I> Parameters);

            int Bits = 0;
            Action<int> AddBits = AddFirstBits;

            void AddFirstBits(int Count)
            {
                AddBits = AddBitsOther;
                Bits += GetBitsCount(Count, BinaryLength);
            }

            void AddBitsOther(int Count)
            {
                Bits += BinaryLength;
            }

            while (!Source.IsEnd)
            {
                if (IsSeporator(Source, ref CharCount))
                {
                    continue;
                }

                if (BeginsSuffix(ref Source, ref CharCount))
                {
                    break;
                }


                if (IsDigit(Source.Current, out int Digit))
                {
                    if (IsNegative)
                    {
                        Digit = -Digit;
                    }

                    AddBits(Digit);

                    if (Bits > Parameters.BitCount)
                    {
                        Token = ReadErrorToken(ref Source, CharCount, Char => IsDigit(Char, out _));
                        return true;
                    }

                    Value = Parameters.LeftShift(Value, BinaryLength);

                    if (Digit != 0)
                    {
                        Value = Parameters.Or(Value, Digit);
                    }
                }
                else
                {
                    return false;
                }

                Source.MoveNext();
                CharCount++;
            }

            Token = CreateToken(Value, CharCount);
            return true;
        }

        private IToken ReadErrorToken(ref TextSource Source, int CharCount, Func<char, bool> IsDigit)
        {
            NumberParsingParameters<I> Parameters = NumberParameters;

            while (!Source.IsEnd)
            {
                char Current = Source.Current;

                Source.Skip(Char => Char == '_' || IsDigit(Char), out int Skipped);
                CharCount += Skipped;

                BeginsSuffix(ref Source, ref CharCount);

                break;
            }

            return new T() { Value = default!, Length = CharCount, Status = TokenStatus.HasErrors };
        }


        private bool CheckOverflow(I Value, int Digit)
        {
            NumberParsingParameters<I> Parameters = NumberParameters;

            if (!Parameters.HasMaxValue)
            {
                return false;
            }

            I Zero = I.Zero;

            if (int.IsPositive(Value.CompareTo(Zero)))
            {
                if (int.IsNegative(Digit))
                {
                    return true;
                }

                I MaxDivision = Parameters.Divide(Parameters.MaxValue, 10);

                if (Value.CompareTo(MaxDivision) > 0)
                {
                    return true;
                }

                if (Value.CompareTo(MaxDivision) == 0)
                {
                    int MaxRemainder = Parameters.Remainder(Parameters.MaxValue, 10);
                    if (Digit > MaxRemainder)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (int.IsPositive(Digit))
                {
                    return true;
                }

                I MinDivision = Parameters.Divide(Parameters.MinValue, 10);

                if (Value.CompareTo(MinDivision) < 0)
                {
                    return true;
                }

                if (Value.CompareTo(MinDivision) == 0 && Digit < Parameters.Remainder(Parameters.MinValue, 10))
                {
                    return true;
                }
            }

            return false;
        }

        private bool BeginsSuffix(ref TextSource Source, ref int CharCount)
        {
            NumberParsingParameters<I> Parameters = NumberParameters;

            if (!IgnoreSuffixes && Translater.IsEnglish(Source.Current))
            {
                foreach (string Suffix in Parameters.Suffixes)
                {
                    if (Source.Begins(Suffix, out Source))
                    {
                        CharCount += Suffix.Length;
                        return true;
                    }
                }
            }

            return false;
        }

        private void InitializeVariables(out IToken Token, out I Value, out NumberParsingParameters<I> Parameters)
        {
            Token = default!;
            Value = new();
            Parameters = NumberParameters;
        }


        private static bool IsBinary(char Char, out int Digit)
        {
            return Char.IsBinaryDigit(out Digit);
        }

        private static bool IsOctal(char Char, out int Digit)
        {
            return Char.IsOctalDigit(out Digit);
        }

        private static bool IsDecimal(char Char, out int Digit)
        {
            return Char.IsDigit(out Digit);
        }

        private static bool IsHexadecimal(char Char, out int Digit)
        {
            return Char.IsHexadecimalDigit(out Digit);
        }


        private static bool IsSeporator(TextSource Source, ref int CharCount)
        {
            if (Source.Current == '_')
            {
                Source.MoveNext();
                CharCount++;
                return true;
            }
            return false;
        }

        private static void SkipZeros(TextSource Source, ref int CharCount)
        {
            Source.Skip('0', out int ZerosReaded);
            CharCount += ZerosReaded;
        }

        private static int GetBitsCount(int Value, int BitsCount)
        {
            BitsCount--;

            int Index = 1 << BitsCount;

            for (int i = BitsCount; i >= 0; i--)
            {
                if ((Value & Index) != 0)
                {
                    return i + 1;
                }
                Index >>= 1;
            }

            return 0;
        }

        private static T CreateToken(I Value, int Length)
        {
            return new T() { Value = Value, Length = Length, Status = TokenStatus.Valid };
        }
    }
}