namespace Zion
{
    [Serializable]
    public struct ColorChar : IColorText, IBinaryObject<ColorChar>
    {
        public char Char;
        public Color Color;

        public ColorChar(char Char)
        {
            this.Char = Char;
            Color = Color.White;
        }
        public ColorChar(char Char, Color Color)
        {
            this.Char = Char;
            this.Color = Color;
        }

        public override string ToString()
        {
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Char}\u001b[0m";
        }

        public string ToColorString() => ToString();
        

        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Char);
            Writer.Write(Color);
        }
        public static ColorChar Read(BinaryReader Reader)
        {
            return new ColorChar
            (
                Reader.ReadChar(),
                Reader.Read<Color>()
            );
        }
    }
}