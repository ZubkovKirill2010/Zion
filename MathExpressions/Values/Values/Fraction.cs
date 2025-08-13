namespace Zion.MathExpressions
{
    public struct Fraction : IExpression, IBinaryObject<Fraction>
    {
        public static readonly Fraction _Zero = new Fraction(0);
        public static readonly Fraction _One = new Fraction(1);
        public static readonly Fraction _Pi = new Fraction(Math.PI);

        public double Divisible, Divider;

        public Fraction(double Ceil)
        {
            Divisible = Ceil;
            Divider = 1;
        }
        public Fraction(double Ceil, double Divisible, double Divider)
        {
            this.Divisible = (Ceil * Divider) + Divisible;
            this.Divider = Divider;
        }
        public Fraction(double Divisible, double Divider)
        {
            this.Divisible = Divisible;
            this.Divider = Divider;
        }

        public override string ToString()
        {
            return $"{Divisible}/{Divider}";
        }


        public Fraction GetValue()
        {
            return this;
        }


        public double ToDouble()
        {
            return Divisible / Divider;
        }

        public Fraction Reverse()
        {
            return new Fraction(Divider, Divisible);
        }

        public static explicit operator double(Fraction Fraction)
        {
            return Fraction.ToDouble();
        }

        public static Fraction operator +(Fraction A, Fraction B)
        {
            if (A.Divider != B.Divider)
            {
                double OldADivider = A.Divider;

                A.Divisible *= B.Divider;
                A.Divider *= B.Divider;

                B.Divisible *= OldADivider;
                B.Divider *= OldADivider;
            }

            return new Fraction
            (
                A.Divisible + B.Divisible,
                A.Divider
            );
        }
        public static Fraction operator -(Fraction A, Fraction B)
        {
            if (A.Divider != B.Divider)
            {
                double OldADivider = A.Divider;

                A.Divisible *= B.Divider;
                A.Divider *= B.Divider;

                B.Divisible *= OldADivider;
                B.Divider *= OldADivider;
            }

            return new Fraction
            (
                A.Divisible - B.Divisible,
                A.Divider
            );
        }
        public static Fraction operator *(Fraction A, bool IsPositive)
        {
            if (!IsPositive)
            {
                return A * -1;
            }
            return A;
        }
        public static Fraction operator *(Fraction A, double B)
        {
            A.Divisible *= B;
            return A;
        }

        public static Fraction operator *(Fraction A, Fraction B)
        {
            return new Fraction(A.Divisible * B.Divisible, A.Divider * B.Divider);
        }
        public static Fraction operator /(Fraction A, Fraction B)
        {
            return new Fraction(A.Divisible * B.Divider, A.Divider * B.Divisible);
        }


        public bool IsZero()
        {
            return Divisible == 0;
        }


        public static Fraction Sqrt(Fraction Fraction)
        {
            return new Fraction(Math.Sqrt(Fraction.Divisible), Math.Sqrt(Fraction.Divider));
        }
        public static Fraction Sin(Fraction Fraction)
        {
            return new Fraction(Math.Sin(Fraction.ToDouble()));
        }
        public static Fraction Cos(Fraction Fraction)
        {
            return new Fraction(Math.Cos(Fraction.ToDouble()));
        }
        public static Fraction Tg(Fraction Fraction)
        {
            return new Fraction(Math.Tan(Fraction.ToDouble()));
        }
        public static Fraction Ctg(Fraction Fraction)
        {
            double Tangens = Math.Tan(Fraction.ToDouble());
            if (Tangens == 0)
            {
                throw new DivideByZeroException("Котангенс не определен для данного угла");
            }
            return new Fraction(1.0 / Tangens);
        }
        public static Fraction Abs(Fraction Fraction)
        {
            return new Fraction(Math.Abs(Fraction.Divisible), Math.Abs(Fraction.Divider));
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Divisible);
            Writer.Write(Divider);
        }
        public static Fraction Read(BinaryReader Reader)
        {
            return new Fraction
            (
                Reader.ReadDouble(),
                Reader.ReadDouble()
            );
        }
    }
}