using System.Numerics;

namespace Zion.STP
{
    public sealed class IntegerTokenReader<T, I> : ITokenReader where T : IValueToken<I>, new() where I : IComparable<I>, INumber<I>, new()
    {
        public readonly NumberParsingParameters<I> NumberParameters;

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
                return ReadBinary(Source, out Token, IsNegative, CharCount + 2);
            }
            if (Source.Begins("0o", out Source))
            {
                return ReadOctal(Source, out Token, IsNegative, CharCount + 2);
            }
            if (Source.Begins("0x", out Source))
            {
                return ReadHexadecimal(Source, out Token, IsNegative, CharCount + 2);
            }

            return ReadDecimal(Source, out Token, IsNegative, CharCount);
        }


        private bool ReadBinary(TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            Token = default!;

            I Value = new();
            NumberParsingParameters<I> NumberParameters = this.NumberParameters;

            int Bits = 0;

            while (!Source.IsEnd)
            {
                char Char = Source.Current;

                if (Char == '_')
                {
                    Source.MoveNext();
                    CharCount++;
                    continue;
                }
                if (this.NumberParameters.Suffixes.Contains(Char))
                {
                    Source.MoveNext();
                    CharCount++;
                    break;
                }

                if (Char is '0' or '1')
                {
                    if (Bits++ >= NumberParameters.BitCount)
                    {
                        Token = ReadErrorToken(Source, CharCount, static Char => Char is '0' or '1');
                        return true;
                    }

                    Value = NumberParameters.LeftShift(Value, 1);

                    if (Char == '1')
                    {
                        Value = NumberParameters.Or(Value, 1);
                    }
                }
                else
                {
                    return false;
                }

                Source.MoveNext();
                CharCount++;
            }

            if (IsNegative)
            {
                Value = NumberParameters.Multiply(Value, -1);
            }

            Token = new T() { Value = Value, Length = CharCount, Status = TokenStatus.Valid };
            return true;
        }

        private bool ReadOctal(TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            return Read(Source, out Token, IsNegative, CharCount, IsOctal, 3, 8);
        }

        private bool ReadDecimal(TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            Token = default!;

            I Value = new();
            NumberParsingParameters<I> NumberParameters = this.NumberParameters;

            while (!Source.IsEnd)
            {
                char Char = Source.Current;

                if (Char == '_')
                {
                    Source.MoveNext();
                    CharCount++;
                    continue;
                }
                if (NumberParameters.Suffixes.Contains(Char))
                {
                    Source.MoveNext();
                    CharCount++;
                    break;
                }

                if (IsDecimal(Char, out int Digit))
                {
                    if (IsNegative)
                    {
                        Digit = -Digit;
                    }

                    if (CheckOverflow(Value, NumberParameters, 10, Digit))
                    {
                        Token = ReadErrorToken(Source, CharCount, static Char => Char >= '0' && Char <= '9');
                        return true;
                    }

                    Value = NumberParameters.Multiply(Value, 10);
                    Value = NumberParameters.Sum(Value, Digit);
                }
                else
                {
                    return false;
                }

                Source.MoveNext();
                CharCount++;
            }

            Token = new T() { Value = Value, Length = CharCount, Status = TokenStatus.Valid };
            return true;
        }

        private bool ReadHexadecimal(TextSource Source, out IToken Token, bool IsNegative, int CharCount)
        {
            return Read(Source, out Token, IsNegative, CharCount, IsHexadecimal, 4, 16);
        }


        private bool Read(TextSource Source, out IToken Token, bool IsNegative, int CharCount, IsDigit IsDigit, int BinaryLength, int CalculusSystem)
        {
            Token = default!;

            I Value = new();
            NumberParsingParameters<I> NumberParameters = this.NumberParameters;

            while (!Source.IsEnd)
            {
                char Char = Source.Current;

                if (Char == '_')
                {
                    Source.MoveNext();
                    CharCount++;
                    continue;
                }
                if (NumberParameters.Suffixes.Contains(Char))
                {
                    Source.MoveNext();
                    CharCount++;
                    break;
                }

                if (IsDigit(Char, out int Digit))
                {
                    if (IsNegative)
                    {
                        Digit = -Digit;
                    }

                    if (CheckOverflow(Value, NumberParameters, CalculusSystem, Digit))
                    {
                        Token = ReadErrorToken(Source, CharCount, Char => IsDigit(Char, out _));
                        return true;
                    }

                    Value = NumberParameters.LeftShift(Value, BinaryLength);
                    Value = NumberParameters.Or(Value, Digit);
                }
                else
                {
                    return false;
                }

                Source.MoveNext();
                CharCount++;
            }

            Token = new T() { Value = Value, Length = CharCount, Status = TokenStatus.Valid };
            return true;
        }

        private IToken ReadErrorToken(TextSource Source, int CharCount, Func<char, bool> IsDigit)
        {
            NumberParsingParameters<I> Parameters = NumberParameters;

            while (!Source.IsEnd)
            {
                char Current = Source.Current;

                if (!(Current == '_' || IsDigit(Current)))
                {
                    if (Parameters.Suffixes.Contains(Current))
                    {
                        Source.MoveNext();
                    }
                    break;
                }

                Source.MoveNext();
                CharCount++;
            }

            return new T() { Value = default!, Length = CharCount, Status = TokenStatus.HasErrors };
        }


        private static bool IsOctal(char Char, out int Digit)
        {
            if (Char >= '0' && Char <= '7')
            {
                Digit = Char - '0';
                return true;
            }
            Digit = default;
            return false;
        }

        private static bool IsDecimal(char Char, out int Digit)
        {
            if (Char >= '0' && Char <= '9')
            {
                Digit = Char - '0';
                return true;
            }
            Digit = default;
            return false;
        }

        private static bool IsHexadecimal(char Char, out int Digit)
        {
            if (Char >= '0' && Char <= '9')
            {
                Digit = Char - '0';
                return true;
            }
            if (Char >= 'a' && Char <= 'f')
            {
                Digit = Char - 'a' + 10;
                return true;
            }
            if (Char >= 'A' && Char <= 'F')
            {
                Digit = Char - 'A' + 10;
                return true;
            }
            Digit = default;
            return false;
        }


        private bool CheckOverflow(I Value, NumberParsingParameters<I> Data, int CalculusSystem, int Digit)
        {
            if (!Data.HasMaxValue)
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

                I MaxDivision = Data.Divide(Data.MaxValue, CalculusSystem);

                if (Value.CompareTo(MaxDivision) > 0)
                {
                    return true;
                }

                if (Value.CompareTo(MaxDivision) == 0)
                {
                    int MaxRemainder = Data.Remainder(Data.MaxValue, CalculusSystem);
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

                I MinDivision = Data.Divide(Data.MinValue, CalculusSystem);

                if (Value.CompareTo(MinDivision) < 0)
                {
                    return true;
                }

                if (Value.CompareTo(MinDivision) == 0 && Digit < Data.Remainder(Data.MinValue, CalculusSystem))
                {
                    return true;
                }
            }

            return false;
        }


        private delegate bool IsDigit(char Char, out int Digit);
    }
}