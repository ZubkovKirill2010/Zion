namespace Zion.FileSystem.Images
{
    public interface IImage
    {
        public int Width  { get; }
        public int Height { get; }

        public bool HasAlpha { get; }

        public RGBColor  this[int X, int Y] { get; }
        public RGBAColor this[int X, int Y] { get; }
    }
}